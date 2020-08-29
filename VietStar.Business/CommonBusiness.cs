using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.FileProfile;
using VietStar.Entities.Infrastructures;
using VietStar.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using static VietStar.Entities.Commons.Enums;
using VietStar.Business.Infrastructures;
using System.IO;
using Dapper;
using NPOI.SS.UserModel;

namespace VietStar.Business
{
    public class CommonBusiness : BaseBusiness, ICommonBusiness
    {
        protected readonly ICommonRepository _rpCommon;
        protected readonly IFileProfileRepository _rpFile;
        protected readonly IPartnerRepository _rpPartner;
        protected readonly ILocationRepository _rpLocation;
        protected readonly IProductRepository _rpProduct;
        protected readonly IEmployeeRepository _rpEmployee;
        protected IServiceProvider _svProvider;
        public CommonBusiness(ICommonRepository commonRepository,
            IFileProfileRepository fileProfileRepository,
            IPartnerRepository partnerRepository,
            IEmployeeRepository employeeRepository,
            IProductRepository productRepository,
            ILocationRepository locationRepository,
            IServiceProvider serviceProvider,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCommon = commonRepository;
            _rpFile = fileProfileRepository;
            _svProvider = serviceProvider;
            _rpPartner = partnerRepository;
            _rpLocation = locationRepository;
            _rpProduct = productRepository;
            _rpEmployee = employeeRepository;
        }
        public async Task<List<OptionSimple>> GetPartnerscheckDupAsync()
        {
            return await _rpCommon.GetListForCheckCustomerDuplicateAsync();
        }
        public async Task<List<OptionSimple>> GetPartnersAsync()
        {
            var result = await _rpPartner.GetsAync(_process.User.OrgId);
            return result;
        }
        public async Task<List<OptionSimple>> GetProvincesAsync()
        {
            var result = await _rpLocation.GetProvincesAync();
            return result;
        }
        public async Task<List<OptionSimple>> GetDistrictsAsync(int provinceId)
        {
            var result = await _rpLocation.GetDistrictsAync(provinceId);
            return result;
        }
        public async Task<List<OptionSimple>> GetStatusList(string profileType)
        {
            if (_process.User.OrgId == 2)
            {
                profileType = _process.User.isAdmin ? profileType : _process.User.RoleCode;
            }
            var result = await _rpCommon.GetProfileStatusByRoleCode(profileType, _process.User.OrgId, _process.User.RoleCode);
            return result;
        }



        public async Task<List<OptionSimple>> GetProductsAsync(int partnerId)
        {
            var result = await _rpProduct.GetsAync(partnerId, _process.User.OrgId);
            return result;
        }

        public async Task<List<OptionSimple>> GetSalesAsync()
        {
            IEmployeeBusiness bizEmployee = _svProvider.GetService<IEmployeeBusiness>();
            var result = await bizEmployee.GetSalesAsync();
            return result;
        }

        public async Task<List<OptionSimple>> GetCouriersAsync()
        {
            var result = await _rpEmployee.GetCouriersAsync(_process.User.OrgId);
            return result;
        }

        public async Task<List<DynamicParameters>> ReadXlsxFileAsync(MemoryStream stream, ProfileType profileType, string configCode)
        {

            var importExelFrameWork = await _rpCommon.GetImportFrameworkByTypeAsync((int)profileType);
            if (importExelFrameWork == null)
                return ToResponse<List<DynamicParameters>>(null, "Không tìm thấy importExelFrameWork");
            var config = await _rpCommon.GetSystemConfigByCodeAsync(configCode);
            //return null;

            var workBook = WorkbookFactory.Create(stream);
            var sheet = workBook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var hasData = rows.MoveNext();
            var param = new DynamicParameters();
            var pars = new List<DynamicParameters>();
            int skipCell = 0;
            if (sheet.PhysicalNumberOfRows - 2 > config.Value)
            {
                return ToResponse<List<DynamicParameters>>(null, $"Số dòng của file không được nhiều hơn {config.Value}");
            }
            for (int i = 2; i < sheet.PhysicalNumberOfRows; i++)
            {
                try
                {
                    param = new DynamicParameters();
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.Cells.Count > 1)
                        {
                            bool isNullRow = row.Cells.Count < 20 ? true : false;
                            if (isNullRow)
                                continue;
                        }

                        foreach (var col in importExelFrameWork)
                        {
                            try
                            {
                                if (row.GetCell(col.Position) == null)
                                {
                                    param.Add(col.Name, string.Empty);
                                    skipCell += 1;
                                }
                                else
                                {
                                    param.Add(col.Name, TryGetValueFromCell(row.Cells[col.Position - skipCell].ToString(), col.ValueType));
                                }

                            }
                            catch (Exception e)
                            {
                                param = null;
                            }

                        }
                        if (param != null)
                        {
                            skipCell = 0;
                            pars.Add(param);

                        }

                    }
                }
                catch
                {

                }

            }
            return pars;
        }
    }
}

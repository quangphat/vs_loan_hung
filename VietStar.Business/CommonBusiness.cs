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
            IPartnerRepository  partnerRepository,
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
            var result = await _rpCommon.GetProfileStatusByRoleCode(profileType, _process.User.OrgId, _process.User.Rolecode);
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
            var result = await _rpEmployee.GetCouriers(_process.User.OrgId);
            return result;
        }
    }
}

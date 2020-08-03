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
        public async Task<List<FileProfileType>> GetProfileFileTypeByType(string profileType)
        {
            if (string.IsNullOrWhiteSpace(profileType))
            {
                AddError("Dữ liệu không hợp lệ");
                return null;
            }
            profileType = profileType.ToLower();
            int type = 0;
            switch (profileType)
            {
                case "common":
                    type = (int)ProfileType.Common;
                    break;
                case "courier":
                    type = (int)ProfileType.Courier;
                    break;
                case "mcredit":
                    type = (int)ProfileType.MCredit;
                    break;
                case "company":
                    type = (int)ProfileType.Company;
                    break;
                case "revoke":
                    type = (int)ProfileType.RevokeDebt;
                    break;
            }
            var result = await _rpFile.GetByType(type);
            return result;
        }
        public async Task<object> UploadFile(IFormFile file, int key, int fileId, int type, string rootPath)
        {
            if (file == null)
                return null;
            string fileUrl = "";
            var _type = string.Empty;
            string deleteURL = string.Empty;
            //string root = Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}");
            IMediaBusiness bizMedia = _svProvider.GetService<IMediaBusiness>();
            var result = new FileModel();
            try
            {

                if (!BusinessExtensions.IsNotValidFileSize(file.Length))
                {
                    using (Stream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        result = await bizMedia.UploadAsync(stream, key.ToString(), file.FileName, rootPath);
                    }
                    deleteURL = fileId <= 0 ? $"/hoso/delete?key={key}" : $"/hoso/delete/0/{fileId}";
                }


                if (_type.IndexOf("pdf") > 0)
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = result.Name,
                                                url = deleteURL,
                                                key =key,
                                                type="pdf",
                                                width ="120px"
                                                }
                                        },
                        append = false
                    };
                    return config;
                }
                else
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = result.Name,
                                                url = deleteURL,
                                                key =key,
                                                width ="120px"
                                            }
                                        },
                        append = false
                    };
                    return config;
                }

            }
            catch (Exception e)
            {
                return null;
            }
           ;
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

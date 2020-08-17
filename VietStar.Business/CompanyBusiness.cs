using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Company;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class CompanyBusiness : BaseBusiness, ICompanyBusiness
    {
        protected readonly ICompanyRepository _rpCompany;
        public CompanyBusiness(ICompanyRepository companyRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCompany = companyRepository;
        }

        public async Task<DataPaging<List<CompanyIndexModel>>> SearchsAsync(string freeText, int page, int limit)
        {
            var datas = await _rpCompany.GetsAsync(freeText, page, limit);
            if (datas == null)
            {
                return DataPaging.Create(datas, 0);
            }
            return DataPaging.Create(datas, datas.FirstOrDefault().TotalRecord);
        }
    }
}

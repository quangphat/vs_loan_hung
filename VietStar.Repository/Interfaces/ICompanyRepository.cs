using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Company;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<CompanyIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit);
    }
}


using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.FileProfile;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business.Interfaces
{
    public interface ICommonBusiness
    {
        Task<List<DynamicParameters>> ReadXlsxFileAsync(MemoryStream stream, ProfileType profileType, string configCode);
        Task<List<OptionSimple>> GetPartnerscheckDupAsync();
        Task<List<OptionSimple>> GetPartnersAsync();
        Task<List<OptionSimple>> GetProductsAsync(int partnerId);
        Task<List<OptionSimple>> GetSalesAsync();
        Task<List<OptionSimple>> GetCouriersAsync();
        Task<List<OptionSimple>> GetProvincesAsync();
        Task<List<OptionSimple>> GetDistrictsAsync(int provinceId);
        Task<List<OptionSimple>> GetStatusList(string profileType);
        
    }
}

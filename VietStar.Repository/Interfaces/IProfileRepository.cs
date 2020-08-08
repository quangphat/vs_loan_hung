using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Profile;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IProfileRepository
    {
        Task<RepoResponse<bool>> UpdateAsync(ProfileAddSql model, int profileId, int updatedBy);
        Task<List<ProfileIndexModel>> GetsAsync(int userId
            , DateTime fromDate
            , DateTime toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , string sort = "desc"
            , string sortField = "updatedtime"
            , int page = 1, int limit = 20);
        Task<RepoResponse<int>> CreateAsync(ProfileAddSql model, int createdBy);
        Task<RepoResponse<ProfileDetail>> GetByIdAsync(int profileId);
    }
}

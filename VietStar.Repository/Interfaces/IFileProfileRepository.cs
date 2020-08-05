using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.FileProfile;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IFileProfileRepository
    {
        Task<bool> DeleteByIdAsync(int fileId, string guidId);
        Task<RepoResponse<int>> Add(ProfileFileAddSql model);
        Task<List<FileProfileType>> GetByType(int profileType);
    }
}


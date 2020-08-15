using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.CheckDup;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface ICheckDupRepository
    {
        Task<List<CheckDupIndexModel>> GetsAsync(
            string freeText,
            int page,
            int limit,
            int userId);
        Task<CheckDupAddSql> GetByIdAsync(int id);
        Task<RepoResponse<int>> CreateAsync(CheckDupAddSql model, int createdBy);
        Task<bool> AddNoteAsync(CheckDupNote note);
    }
}


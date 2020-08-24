using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.GroupModels;

namespace VietStar.Repository.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<GroupModel>> GetParentGroupsByUserIdAsync(int userId);
        Task<List<GroupModel>> GetGroupByUserId(int userId);
        Task<List<GroupModel>> GetChildGroupBaseParentSequenceCodeByParentId(int parentGroupId, int userId);
        Task<List<GroupModel>> GetParentGroupsAsync(int userId);
        Task<List<GroupModel>> GetChildGroupByParentIdAsync(int parentGroupId, int userId);
        Task<List<GroupIndexModel>> GetChildGroupByParentIdForPagingAsync(int page, int limit, int parentGroupId, int userId);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.GroupModels;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IGroupBusiness
    {
        Task<List<GroupModel>> GetChildByParentIdAsync(int parentId);
        Task<List<GroupModel>> GetApproveGroupByUserId();
        Task<List<GroupModel>> GetGroupByUserId();
        Task<List<GroupModel>> GetParentGroups();
        Task<DataPaging<List<GroupIndexModel>>> SearchAsync(int parentId, int page = 1, int limit = 10);
    }
}

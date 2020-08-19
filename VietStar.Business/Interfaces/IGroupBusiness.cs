using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.GroupModels;
using VietStar.Entities.ViewModels;

namespace VietStar.Business.Interfaces
{
    public interface IGroupBusiness
    {
        Task<List<GroupModel>> GetApproveGroupByUserId();
        Task<List<GroupModel>> GetGroupByUserId();
        Task<List<GroupModel>> GetParentGroups();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.GroupModels;

namespace VietStar.Repository.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<GroupModel>> GetGroupByUserId(int userId);
        Task<List<GroupModel>> GetChildGroupByParentId(int parentGroupId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.GroupModels;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class GroupBusiness : BaseBusiness, IGroupBusiness
    {
        protected readonly IGroupRepository _rpGroup;
        public GroupBusiness(IGroupRepository groupRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpGroup = groupRepository;
        }
        public async Task<List<GroupModel>> GetGroupByUserId()
        {
            var groups = await _rpGroup.GetGroupByUserId(_process.User.Id);
            if (groups == null || !groups.Any())
                return null;
            var result = new List<GroupModel>();
            var temp = new List<GroupModel>();
            foreach(var group in groups)
            {
                if (temp.FirstOrDefault(p => group.ParentSequenceCode.Contains($".{p.Id}.") || group.ParentSequenceCode.EndsWith($".{p.Id.ToString()}")) != null)
                    continue;
                temp.RemoveAll(p => group.ParentSequenceCode.Contains($".{p.Id}.") || group.ParentSequenceCode.EndsWith($".{p.Id.ToString()}"));
                temp.Add(group);
                if(temp.Count >0)
                {
                    foreach(var item in temp)
                    {
                        var children = await _rpGroup.GetChildGroupByParentId(item.Id);
                        if(children!=null || children.Any())
                        {
                            result.AddRange(GenerateChildList(children, $"{item.ParentSequenceCode}.{item.Id}", _process.User.Id));
                        }
                    }
                    
                }
            }
            return result;
        }

        private List<GroupModel> GenerateChildList(List<GroupModel> groups, string parentSequenceCode, int leaderId)
        {
            if (groups == null || !groups.Any())
                return null;
            var lstResult = new List<GroupModel>();
            var stack = new Stack<GroupModel>();
            var lstFind = new List<GroupModel>();
            string parentCodeTemp = parentSequenceCode;
            do
            {
                if (stack.Count > 0)
                {
                    var nhom = stack.Pop();
                    if (nhom != null)
                    {
                        string[] tempArray = nhom.ParentSequenceCode.Split('.');
                        if (tempArray.Length > 1)
                        {
                            for (int i = 0; i < tempArray.Length - 1; i++)
                            {
                                nhom.Name = "-" + nhom.Name;
                            }
                        }
                        lstResult.Add(nhom);
                        parentCodeTemp = nhom.ParentSequenceCode + "." + nhom.Id;
                    }
                }
                lstFind = groups.FindAll(x => x.ParentSequenceCode.Equals(parentCodeTemp));
                if (lstFind != null)
                {
                    for (int i = lstFind.Count - 1; i >= 0; i--)
                    {
                        if (lstFind[i].ParentSequenceCode == parentSequenceCode && lstFind[i].LeaderId != leaderId)
                            continue;
                        stack.Push(lstFind[i]);
                        groups.Remove(lstFind[i]);
                    }
                }
            } while (stack.Count > 0);
            return lstResult;
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class EmployeeBusiness : BaseBusiness, IEmployeeBusiness
    {
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly IGroupRepository _rpGroup;
        public EmployeeBusiness(IEmployeeRepository employeeRepository, IGroupRepository groupRepository,IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpEmployee = employeeRepository;
            _rpGroup = groupRepository;
        }
        public async Task<List<OptionSimple>> GetSalesAsync()
        {
            var result = new List<OptionSimple>();
            var groups = await _rpGroup.GetGroupByUserId(_process.User.Id);
            if (groups != null)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    var members = await _rpEmployee.GetMemberByGroupIdIncludeChild(groups[i].Id, _process.User.Id);
                    result.AddRange(members);

                }
                result.DistinctBy(p => p.Id);
            }

            return result;
        }
        public async Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId)
        {
            if (groupId > 0)
                return await _rpEmployee.GetMemberByGroupIdIncludeChild(groupId, _process.User.Id);
            var result = new List<OptionSimple>();
            var groups = await _rpGroup.GetGroupByUserId(_process.User.Id);
            if (groups != null)
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    var members = await _rpEmployee.GetMemberByGroupId(groups[i].Id, _process.User.Id);
                    result.AddRange(members);
                   
                }
                result.DistinctBy(p => p.Id);
            }
            return result;
        }

        public async Task<bool> GetStatusAsync(int userId)
        {
            return await _rpEmployee.GetStatus(userId);
        }
        

        public async Task<Account> LoginAsync(LoginModel model)
        {
            if(model==null)
            {
                AddError(Errors.invalid_data);
                return null;
            }

            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                //AddError($"connStr:{_rpEmployee.ConnectStr}, username: {username}, password: {password}");
                AddError(Errors.username_or_password_must_not_be_empty);
                return null;
            }
            var encodePass = Utils.getMD5(model.Password);

            var account = await _rpEmployee.Login(model.UserName, encodePass);

            if (account == null)
            {
                AddError(Errors.invalid_username_or_pass);
                return null;
            }
            account.Permissions = await _rpEmployee.GetPermissions(account.Rolecode);
            return account;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
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
        public EmployeeBusiness(IEmployeeRepository employeeRepository,IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpEmployee = employeeRepository;
        }
        public async Task<bool> GetStatus(int userId)
        {
            return await _rpEmployee.GetStatus(userId);
        }
        public async Task<List<string>> GetPermission(int userId)
        {
            var result = await _rpEmployee.GetPermissions(userId);
            return result;
        }

        public async Task<Account> Login(LoginModel model)
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
            account.Permissions = await _rpEmployee.GetPermissions(account.Id);
            return account;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.ViewModels
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public string Rolecode { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Permissions { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }
}

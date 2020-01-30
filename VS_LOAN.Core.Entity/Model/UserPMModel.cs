using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public  class UserPMModel
    {
        private int _idUser = 0;
        public int IDUser
        {
            get
            {
                return _idUser;
            }
            set
            {
                _idUser = value;
            }
        }
        private string _code = string.Empty;
        private string _userName = string.Empty;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }
        private string _password = string.Empty;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        private string _fullName = string.Empty;
        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName = value;
            }
        }
       
        private string _email = string.Empty;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }
        private string _phone = string.Empty;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }
       
        private int _loginNetwork = -1;
        public int LoginNetwork
        {
            get
            {
                return _loginNetwork;
            }
            set
            {
                _loginNetwork = value;
            }
        }
        private int _typeUser = -1;
        public int TypeUser
        {
            get
            {
                return _typeUser;
            }
            set
            {
                _typeUser = value;
            }
        }
        private int _gender = -1;
        public int Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
            }
        }

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }
        public int UserType { get; set; }
    }
}

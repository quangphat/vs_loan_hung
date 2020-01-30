using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Web.Helpers
{
    public class GlobalData
    {
        
        public static UserPMModel User
        {
            get
            {
                if (HttpContext.Current.Session[Constant.SESSION_LOGIN] != null)
                {
                    return (UserPMModel)HttpContext.Current.Session[Constant.SESSION_LOGIN];
                }
                else
                    return null; 
            }
            set
            {
                HttpContext.Current.Session[Constant.SESSION_LOGIN] = value;
            }
            
        }
        
        public static bool IsLogin
        {
            get
            {
                return (HttpContext.Current.Session[Constant.SESSION_LOGIN] != null);
            }
        }

        public static string Rules
        {
            get
            {
                if (HttpContext.Current.Session[Constant.SESSION_LIST_RULE] != null)
                {
                    return (string)HttpContext.Current.Session[Constant.SESSION_LIST_RULE];
                }
                else return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[Constant.SESSION_LIST_RULE] = value;
            }
        }
        public static string LinkBack
        {
            get
            {
                if (HttpContext.Current.Session[Constant.SESSION_LINKBACK] != null)
                {
                    return (string)HttpContext.Current.Session[Constant.SESSION_LINKBACK];
                }
                else return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[Constant.SESSION_LINKBACK] = value;
            }
        }
        public static string LagActive
        {
            get
            {
                if (HttpContext.Current.Session[Constant.SESSION_ACTIVELAG] != null)
                {
                    return (string)HttpContext.Current.Session[Constant.SESSION_ACTIVELAG];
                }
                else
                    return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[Constant.SESSION_ACTIVELAG] = value;
            }

        }
        public static bool FirstAD
        {
            get
            {
                if (HttpContext.Current.Session[Constant.FIRST_AD] != null)
                {
                    return (bool)HttpContext.Current.Session[Constant.FIRST_AD];
                }
                else
                {
                    HttpContext.Current.Session[Constant.FIRST_AD] = true;
                    return (bool)HttpContext.Current.Session[Constant.FIRST_AD];
                }
            }
            set
            {
                HttpContext.Current.Session[Constant.FIRST_AD] = value;
            }

        }
    }
}
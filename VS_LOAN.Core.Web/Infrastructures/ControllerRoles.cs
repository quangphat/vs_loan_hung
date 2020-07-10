using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Infrastructures
{
    public static class ControllerRoles
    {
        public static Dictionary<string, ActionInfo> Roles = new Dictionary<string, ActionInfo> {
            { "mcedit_checkcat", new ActionInfo{ _formindex = IndexMenu.M_8_1, _href = "MCredit/CheckCat", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
            { "mcedit_checkcic", new ActionInfo{ _formindex = IndexMenu.M_8_2, _href = "MCredit/CheckCIC", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
            { "mcedit_checkdup", new ActionInfo{ _formindex = IndexMenu.M_8_3, _href = "MCredit/CheckDuplicate", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
            { "mcedit_checkstatus", new ActionInfo{ _formindex = IndexMenu.M_8_4, _href = "MCredit/CheckStatus", _mangChucNang = new int[] { (int)QuyenIndex.Public } } }
        };
    }
}
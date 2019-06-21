using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VS_LOAN.Core.Web.Common
{
    public class GroupPositionAD
    {
        private static string[] _manageApprovePerDiem = new string[2] { "M", "SM"};

        public static string[] ManageApprovePerDiem
        {
            get
            {
                return _manageApprovePerDiem;
            }

            set
            {
                _manageApprovePerDiem = value;
            }
        }
        private static string[] _partnerApprovePerDiem = new string[2] { "P", "SP" };

        public static string[] PartnerApprovePerDiem
        {
            get
            {
                return _partnerApprovePerDiem;
            }

            set
            {
                _partnerApprovePerDiem = value;
            }
        }
    }
}
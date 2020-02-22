using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Infrastructures
{
    public class EcApiPath
    {
        public static string BasePathDev = "http://localhost:5000";
        public static string BasePath = "http://112.213.89.5/plesk-site-preview/vietbankfc.api";
        public static string TokenPath = "/api/EcCredit/token";
        public static string Step2 = "/api/EcCredit/step2";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCreditService.Infrastructure
{
    public static class ECApiPath
    {
        public static string ECBasePathTest = "https://apipreprod.easycredit.vn/api";
        public static string LoanRequest = "/loanServices/v1/loanRequest";
        public static string ECGetToken = "https://apipreprod.easycredit.vn/api/uaa/oauth/token?grant_type=client_credentials";
    }
}

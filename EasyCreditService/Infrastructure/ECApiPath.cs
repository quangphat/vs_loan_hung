using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCreditService.Infrastructure
{
    public static class ECApiPath
    {
        public static string ECClientId = "vietbank_loan_client";
        public static string ECClientSecret = "QhjtghAyH8tOuVMprJnn";
        public static string ECBasePathTest = "https://apipreprod.easycredit.vn/api";
        public static string LoanRequest = "/loanServices/v1/loanRequest";
        public static string ECGetTokenBasePath = "https://apipreprod.easycredit.vn";
        public static string ECGetTokenPath = "/api/uaa/oauth/token?grant_type=client_credentials";
    }
}

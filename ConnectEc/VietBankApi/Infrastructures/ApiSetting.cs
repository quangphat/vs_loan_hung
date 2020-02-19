using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VietBankApi.Infrastructures
{
    public class ApiSetting
    {
        public string SFTPPath { get; set; }
        public string SftpPort { get; set; }
        public string SftpUsername { get; set; }
        public string SftpPassword { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BasePath { get; set; }
        public string TokentPath { get; set; }
        public string SqlConn { get; set; }
        public string Step2 { get; set; }
    }
}

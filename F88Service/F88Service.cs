using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.F88Model;
using VS_LOAN.Core.Utility;
namespace F88Service
{
    public class F88Service
    {
        //public static readonly string F88BasePath = "http://118.70.129.116:1809";
        public static readonly string F88BasePath = "https://apilienket.f88.vn";

        private async Task<HttpResponseMessage> PostLadipageReturnID(LadipageModel model)
        {
            HttpClient _httpClient = new HttpClient();
            var result = await _httpClient.Post(F88BasePath, "/LadipageReturnID", null, model);
            return result;
        }

        public async Task<bool> LadipageReturnID(LadipageModel model)
        {
            if (model == null)
                return false;
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Phone))
                return false;
            try
            {
                var result = await PostLadipageReturnID(model);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}

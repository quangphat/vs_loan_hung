using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.F88Model;
using VS_LOAN.Core.Utility;
namespace F88Service
{
    public class F88Service
    {
       
        private async Task<ResponseModel> PostLadipageReturnID(LadipageModel model)
        {
            model.Select1 = "";
            model.ReferenceType = 10;
            HttpClient _httpClient = new HttpClient();
            var response = await _httpClient.Post<ResponseModel>(F88ApiPath.F88BasePath, "/LadipageReturnID", data: model);

            return response.Data;
        }
        
        public async Task<F88ResponseModel> LadipageReturnID(LadipageModel model)
        {
            if (model == null)
                return ToF88Response(false);
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Phone))
                return ToF88Response(false, "Tên hoặc số điện thoại không được bỏ trống");
            try
            {
                var result = await PostLadipageReturnID(model);
                return ToF88Response(result);
            }
            catch (Exception e)
            {
                return ToF88Response(false, e.Message);
            }
        }
        public F88ResponseModel ToF88Response(ResponseModel model)
        {
            var result = new F88ResponseModel();
            result.Success = false;
            result.Message = "";
            if (model.Code == 200 && model.Data != null)
            {
                if (model.Data.Count() == 2)
                {
                    result.Success = false;
                    result.Message = "Số điện thoại đã tồn tại";
                    return result;
                }
                if (model.Data.Count() == 4)
                {
                    result.Success = true;
                    result.Message = "Thành công";
                    return result;
                }
                return result;
            }
            else
            {
                result.Success = false;
                result.Message = $"Code:{model.Code} . Message: {model.Message}";
                return result;
            }
            return result;
        }
        public F88ResponseModel ToF88Response(bool success, string message = null)
        {
            return new F88ResponseModel
            {
                Success = success,
                Message = message
            };
        }
    }
}

using MCreditService;
using MCreditService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class MiraeDeferController : BaseController
    {

    

        protected readonly IMiraeDeferRepository _deferRepository;

        protected readonly IMiraeService _odcService;
     
        public static ProvinceResponseModel _provinceResponseModel;
        public MiraeDeferController(          
            IMiraeService odcService,

            IMiraeDeferRepository deferRepository
            ) : base()
        {
         
         
            _odcService = odcService;
    
            _deferRepository = deferRepository;
        }

        public async Task<JsonResult> PushPendHistory(string deferStatus,int appId, string documentType , string comment)
        {

        

            var multiForm = new MultipartFormDataContent();
            multiForm.Add(new StringContent("EXT_SBK"), "usersname");
            multiForm.Add(new StringContent("mafc123!"), "password");
            multiForm.Add(new StringContent("S1"), "defercode");
            multiForm.Add(new StringContent(deferStatus), "deferstatus");
            multiForm.Add(new StringContent(appId.ToString()), "appid");
            multiForm.Add(new StringContent("EXT_SBK"), "userid");

            multiForm.Add(new StringContent(comment), "comment");
            if (string.IsNullOrEmpty(documentType))
            {
                 if(string.IsNullOrEmpty(comment))
                {
                    return ToJsonResponse(false, "", new PushToHistoryReponse() { Data = "Bạn chưa ghi chú", Message = "Bạn chưa ghi chú", Success = false });
                }
              
            }
            else
            {
                if(Request.Files.Count<1)
                {

                }
                var file = Request.Files[0];


                byte[] fileByte;

                using (Stream inputStream = file.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    fileByte = memoryStream.ToArray();
                }
              multiForm.Add(new ByteArrayContent(fileByte),documentType, file.FileName);
             
            }

            var reponse = await _odcService.PushToPendHistory(multiForm);
            if(string.IsNullOrEmpty(reponse.Data))
            {
                if (reponse.Success)
                {
                    reponse.Data = "Bổ sung thành công";
                }
                else
                {
                    reponse.Data = "Bổ sung thất bại";
                }

            }
            return ToJsonResponse(reponse.Success, "", reponse);

        }


        public async Task<JsonResult> GetAlLDefer(int appId )
        {
            var profiles = await _deferRepository.GetDeferById(appId);
                
            if (profiles == null || !profiles.Any())
            {
                return ToJsonResponse(true, "", DataPaging.Create(null as List<MiraeDeferSearchModel>, 0));
            }
            var result = DataPaging.Create(profiles, profiles[0].TotalRecord);
            return ToJsonResponse(true, "", result);
        }
    
        //public async Task<ActionResult> Index()
        //{

        //    await _odcService.CheckAuthen();


        //    return View("Temp");
        //}



        public async Task<JsonResult> GetAllMiraeDeferType()
        {
            var result = await _deferRepository.GetAllMiraeDeferType();
            return ToJsonResponse(true, "", data: result);

        }

      

       
    }
}
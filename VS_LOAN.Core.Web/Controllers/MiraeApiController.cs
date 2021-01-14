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



using System.Net;

using System.Security.Principal;

using System.Threading;

using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using Unity;
using System.Dynamic;

namespace VS_LOAN.Core.Web.Controllers
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var userName = usernamePasswordArray[0];
                var password = usernamePasswordArray[1];
                UserPMModel user = new UserPMBLL().DangNhap(userName, MD5.getMD5(password));
                if (user != null)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                    Thread.CurrentPrincipal = principal;
                    base.OnAuthorization(actionContext);
                }
                else
                {

                    HandleUnathorized(actionContext);
                }
            }
            else
            {

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }



        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage((HttpStatusCode)401) { ReasonPhrase = "Unauthorized user" };
        }
    }



    public class MiraeApiController : BaseApiController
    {




        public readonly IMiraeDeferRepository _miraeDeferRepository;
        public readonly IMiraeRepository _miraeRepository;
        public readonly IMiraeService _miraeService;
        public readonly IMiraeMaratialRepository _miraeMaratialRepository;
        public MiraeApiController()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IMiraeDeferRepository, MiraeDeferRepository>();
            container.RegisterType<INoteRepository, NoteRepository>();
            container.RegisterType<IMiraeRepository, MiraeRepository>();
            container.RegisterType<IMiraeMaratialRepository, MiraeMaratialRepository>();
            container.RegisterType<ILogRepository, LogRepository>();
            container.RegisterType<IMiraeService, MiraeService>();
            container.RegisterType<IMiraeMaratialRepository, MiraeMaratialRepository>();
            
            _miraeService = container.Resolve<IMiraeService>();
            _miraeDeferRepository = container.Resolve<IMiraeDeferRepository>();
            _miraeRepository = container.Resolve<IMiraeRepository>();
            _miraeMaratialRepository = container.Resolve<IMiraeMaratialRepository>();
        }
       
            public MiraeApiController(IMiraeDeferRepository miraeDeferRepository) : base()
        {


            this._miraeDeferRepository = miraeDeferRepository;


        }
               
        [BasicAuthentication]
       
        public async Task<HttpResponseMessage> UploadDefer(UploadDeferRequest requests)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, requests);
            var item = new MiraeDeferModel()
            {
                Client_name = requests.client_name,
                CreatedBy = 0,
                CreatedTime = DateTime.Now,
                Defer_code = requests.defer_code,
                Defer_note = requests.defer_note,
                Defer_time = requests.defer_time,
                UpdatedTime = DateTime.Now,
                Id_f1 = requests.id_f1,
                UpdatedBy = 0
            };
             var appId = 0;
            try
            {
                appId = int.Parse(item.Id_f1);
            }
            catch (Exception)
            {
                appId = 0;
            }
            var profile = await _miraeRepository.GetByAppid(appId);
            var itemReponse = new UploadDeferReponse()
            {
                status = "SUCCESS",
                message = null,
                data = new UploadDeferReponseItem()
                {
                    client_name = item.Client_name,
                    defer_code = item.Defer_code,
                    defer_note = item.Defer_note,
                    defer_time = item.Defer_time,
                    id_f1 = item.Id_f1
                }
            };
          
            if (profile==null)
            {
                itemReponse.message = String.Format("{0} không tồn tại.",
                        item.Id_f1);
                itemReponse.status = "ERROR";


                item.StatusDefer = itemReponse.status;
                await this._miraeDeferRepository.Add(item);


            }
            else
            {
                itemReponse.message = "";
                itemReponse.status = "SUCCESS";
                item.StatusDefer = itemReponse.status;
                await this._miraeDeferRepository.Add(item);
            }
           
            return this.Request.CreateResponse(HttpStatusCode.OK, itemReponse);
        }

        public async Task<HttpResponseMessage> uploadStatusF1(List<UploadStatusRequest> requests)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, requests);
            var responseList = new List<UploadStatusReponse>();
            foreach (var item1 in requests)
            {
                var appId = 0;

                try
                {
                    appId = int.Parse(item1.id_f1);

                }
                catch (Exception)
                {
                    appId = 0;
                }
                var profile = await _miraeRepository.GetByAppid(appId);
                var message = string.Empty;
                var success = true;
                var itemReponse = new UploadStatusReponseItem()
                {
                    status_f1 = item1.status_f1,
                    client_name = item1.client_name,
                    f1_time = item1.f1_time,
                    id_f1 = item1.id_f1,
                    f1_no = item1.f1_no
                };
                if (profile == null)
                {
                    message = "Không tồn tại mã id_f1";
                    success = false;
                }
                else
                {
                    success = true;
                  
                } 
                

                if(success ==true)
                {
                    // update status
                    DateTime dt = DateTime.Now;
                    try
                    {
                        dt = DateTime.Parse(item1.f1_time);
                    }
                    catch (Exception)
                    {
                    }
                     await _miraeRepository.UpdatStatusClient(new ClientUpdateStatusRequest() {
                            AppId = appId,
                            BussinessTime = dt,
                            Status = item1.status_f1,
                            Reason = item1.reason,
                            Rejeccode = item1.rejected_code
                    });
                  
                }
                else
                {
                }

                var reponseItem = new UploadStatusReponse()
                {
                    message = message,
                    status = success == true ? "SUCCESS" : "ERROR",
                    data = itemReponse
                };

                responseList.Add(reponseItem);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, responseList);
        }

        

    }
}
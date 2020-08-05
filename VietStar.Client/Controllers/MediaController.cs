using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    [Route("media")]
    public class MediaController : VietStarBaseController
    {
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IHostingEnvironment _hosting;
        public MediaController(CurrentProcess currentProcess
            , IHostingEnvironment hosting
            , IMediaBusiness mediaBusiness):base(currentProcess)
        {
            _bizMedia = mediaBusiness;
            _hosting = hosting;
        }
        [HttpGet("GetFileType/{profileType}")]
        public async Task<IActionResult> GetFileType(string profileType)
        {
            var result = await _bizMedia.GetProfileFileTypeByType(profileType);
            return ToResponse(result);
        }
        [HttpPost("UploadFile/{key}/{fileType}/{profileId}/{fileId}/{guidId}")]
        public async Task<IActionResult> Upload(int key, int fileType, int profileId,int fileId, string guidId)
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizMedia.UploadFileAsync(file, fileId , key, guidId, profileId, fileType, _hosting.ContentRootPath);
            return Json(result);
        }
        [HttpDelete("delete/{fileId}/{guidId}")]
        public async Task<IActionResult> Upload(int fileId, string guidId)
        {
            
            var result = await _bizMedia.DeleteByIdAsync(fileId, guidId);
            return ToResponse(result);
        }
    }
}
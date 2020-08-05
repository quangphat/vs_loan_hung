﻿using System;
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
        [HttpGet("GetFileType/{profileType}/{profileId}")]
        public async Task<IActionResult> GetFileType(string profileType, int profileId = 0)
        {
            var result = await _bizMedia.GetProfileFileTypeByTypeAsync(profileType, profileId, _hosting.ContentRootPath);
            return ToResponse(result);
        }
        [HttpPost("UploadFile/{key}/{fileType}/{profileId}/{fileId}/{guidId}")]
        public async Task<IActionResult> Upload(int key, int fileType, int profileId,int fileId, string guidId)
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizMedia.UploadFileAsync(file, key:key,fileId:fileId ,guildId: guidId,profileId: profileId,type: fileType,rootPath: _hosting.ContentRootPath);
            return Json(result);
        }
        [HttpPost("delete/{fileId}/{guidId}")]
        public async Task<IActionResult> Delete(int fileId, string guidId)
        {
            
            var result = await _bizMedia.DeleteByIdAsync(fileId, guidId);
            return ToResponse(result);
        }
    }
}
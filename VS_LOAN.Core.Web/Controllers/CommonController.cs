using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Web.Controllers
{
    public class CommonController : BaseController
    {
        protected readonly INoteRepository _rpNote;
        public CommonController(INoteRepository noteRepository):base()
        {
            _rpNote = noteRepository;
        }
        public async Task<JsonResult> Comments(int profileId, int type)
        {
            var result = await _rpNote.GetNoteByTypeAsync(profileId, type);
            return ToJsonResponse(true, "", data: result);
        }
    }
}
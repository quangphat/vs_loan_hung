using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Infrastuctures;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.CheckDup;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Business
{
    public class CheckDupBusiness : BaseBusiness, ICheckDupBusiness
    {
        protected readonly ICheckDupRepository _rpCheckDup;
        protected readonly INoteRepository _rpNote;
        public CheckDupBusiness(ICheckDupRepository checkdupRepository, INoteRepository noteRepository) : base()
        {
            _rpCheckDup = checkdupRepository;
            _rpNote = noteRepository;
        }
        public async Task<BaseResponse<bool>> UpdateAsync(CheckDupEditModel model, int updatedBy)
        {
            if (model == null)
            {
                return ToResponse(false, false, "Dữ liệu không hợp lệ");
            }
            if (model.PartnerId <= 0)
                return ToResponse(false, false, "Vui lòng chọn đối tác");
            var checkDup = _mapper.Map<CheckDupAddSql>(model);

            var result = await _rpCheckDup.UpdateAsync(checkDup, updatedBy);
            if (!result.success)
                return ToResponse(result);
            if (!string.IsNullOrWhiteSpace(model.Note))
            {
                var note = new GhichuModel
                {
                    Noidung = model.Note,
                    HosoId = checkDup.Id,
                    CommentTime = DateTime.Now,
                    TypeId = (int)NoteType.CheckDup
                };
               await  _rpNote.AddNoteAsync(note);
            }

            return ToResponse(true);
        }

        public async Task<List<GhichuViewModel>> GetNotesAsync(int checkDupId)
        {
            return await _rpCheckDup.GetNoteByIdAsync(checkDupId);
        }

        public async Task<DataPaging<List<CheckDupIndexModel>>> GetsAsync(
            string freeText,
            int page,
            int limit, int userId)
        {
            var datas = await _rpCheckDup.GetsAsync(freeText, page, limit, userId);
            var result = DataPaging.Create(datas, datas.FirstOrDefault()==null ? 0 : datas.FirstOrDefault().TotalRecord);
            return result;
        }

        public async Task<CheckDupAddSql> GetByIdAsync(int id)
        {
            var result = await _rpCheckDup.GetByIdAsync(id);
            return result;
        }

        public async Task<BaseResponse<int>> CreateAsync(CheckDupAddModel model, int createdBy)
        {
            if (model.PartnerId <= 0)
                return ToResponse(0, false, "Vui lòng chọn đối tác");
            var obj = _mapper.Map<CheckDupAddSql>(model);
            obj.CICStatus = (int)CheckDupCICStatus.NotDebt;
            obj.PartnerStatus = (int)CheckDupPartnerStatus.NotCheck;
            var response = await _rpCheckDup.CreateAsync(obj,createdBy);
            if (response.data > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.Note))
                {
                    var note = new GhichuModel
                    {
                        Noidung = model.Note,
                        HosoId = response.data,
                        CommentTime = DateTime.Now,
                        TypeId = (int)NoteType.CheckDup
                    };
                    await _rpNote.AddNoteAsync(note);
                }
                return ToResponse(response.data);
            }
            return ToResponse(0,false, response.error);
        }
    }
}

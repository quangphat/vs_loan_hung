using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.CheckDup;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class CheckDupBusiness : BaseBusiness, ICheckDupBusiness
    {
        protected readonly ICheckDupRepository _rpCheckDup;
        public CheckDupBusiness(ICheckDupRepository checkdupRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCheckDup = checkdupRepository;
        }
        public async Task<bool> UpdateAsync(CheckDupEditModel model)
        {
            if (model == null || model.CheckDup == null)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            if (model.Partners == null || !model.Partners.Any())
                return ToResponse(false, "Vui lòng chọn đối tác");
            var partner = model.Partners[0];
            bool isMatch = partner.IsSelect;


            var checkDup = _mapper.Map<CheckDupAddSql>(model.CheckDup);
            checkDup.IsMatch = isMatch;
            checkDup.MatchCondition = isMatch ? partner.Name : string.Empty;
            checkDup.NotMatch = isMatch == false ? partner.Name : string.Empty;
            var result = await _rpCheckDup.UpdateAsync(checkDup);
            if (!result.success)
                return ToResponse(result);
            if (!string.IsNullOrWhiteSpace(model.CheckDup.Note))
            {
                var note = new CheckDupNote
                {
                    Note = model.CheckDup.Note,
                    CustomerId = checkDup.Id,
                    CreatedBy = _process.User.Id
                };
                _rpCheckDup.AddNoteAsync(note);
            }

            return true;
        }
        public async Task<List<CheckDupNoteViewModel>> GetNotesAsync(int checkDupId)
        {
            return await _rpCheckDup.GetNoteByIdAsync(checkDupId);
        }
        public async Task<DataPaging<List<CheckDupIndexModel>>> GetsAsync(
            string freeText,
            int page,
            int limit)
        {
            var datas = await
                 _rpCheckDup.GetsAsync(freeText, page, limit, _process.User.Id);
            var result = DataPaging.Create(datas, datas.FirstOrDefault().TotalRecord);
            return result;
        }
        public async Task<CheckDupAddSql> GetByIdAsync(int id)
        {
            var result = await _rpCheckDup.GetByIdAsync(id);
            return result;
        }
        public async Task<int> CreateAsync(CheckDupAddModel model)
        {
            if (model.Partners == null || !model.Partners.Any())
                return ToResponse(0, "Vui lòng chọn đối tác");
            var partner = model.Partners[0];
            bool isMatch = partner.IsSelect;
            var obj = _mapper.Map<CheckDupAddSql>(model);
            var response = await _rpCheckDup.CreateAsync(obj, _process.User.Id);
            if (response.data > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.Note))
                {
                    var note = new CheckDupNote
                    {
                        Note = model.Note,
                        CustomerId = response.data,
                        CreatedBy = obj.CreatedBy
                    };
                    _rpCheckDup.AddNoteAsync(note);
                }
                return response.data;
            }
            return ToResponse(0,response.error);
        }
    }
}

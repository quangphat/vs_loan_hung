using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using VietStar.Business.Interfaces;
using VietStar.Entities.Collection;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using Microsoft.Extensions.DependencyInjection;
using VietStar.Entities.Commons;
using Dapper;
using System.IO;

namespace VietStar.Business
{
    public class RevokeDebtBusiness : BaseBusiness, IRevokeDebtBusiness
    {
        protected readonly IRevokeDebtRepository _rpRevoke;
        protected IServiceProvider _svProvider;
        public RevokeDebtBusiness(IRevokeDebtRepository revokeDebtRepository,
            IServiceProvider serviceProvider,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _svProvider = serviceProvider;
            _rpRevoke = revokeDebtRepository;
        }

        public async Task<DataPaging<List<RevokeDebtSearch>>> SearchAsync(string freeText, 
            string status,
            int page, 
            int limit, 
            int groupId = 0,
            int assigneeId = 0,
            DateTime? fromDate = null, 
            DateTime? toDate = null, 
            int dateType = 1,
            int processStatus = -1)
        {
            fromDate = fromDate.HasValue ? fromDate.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var data = await _rpRevoke.SearchAsync(_process.User.Id, freeText, status, page, limit, groupId, assigneeId, fromDate, toDate, dateType, processStatus);
            if (data == null || !data.Any())
            {
                return DataPaging.Create(null as List<RevokeDebtSearch>, 0);
            }
            var result = DataPaging.Create(data, data[0].TotalRecord);
            return result;
        }

        public async Task<string> InsertFromFileAsync(IFormFile file)
        {
            if (file == null)
                return ToResponse(string.Empty,Errors.file_cannot_be_null);
            var bizCommon = _svProvider.GetService<ICommonBusiness>();
            var inputParams = null as List<DynamicParameters>;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                inputParams = await bizCommon.ReadXlsxFileAsync(stream, Entities.Commons.Enums.ProfileType.RevokeDebt, Constants.revoke_debt_max_row_import);
            }
            if(inputParams ==null)
            {
                return $"Đã import thành công {0} dòng";
            }

            var result = await _rpRevoke.InsertManyByParameterAsync(inputParams, _process.User.Id);
            return $"Đã import thành công {result} dòng";
        }

        public async Task<RevokeDebtSearch> GetByIdAsync(int profileId)
        {
            var result = await _rpRevoke.GetByIdAsync(profileId, _process.User.Id);
            return result;
        }

        public async Task<bool> DeleteByIdAsync(int profileId)
        {
            return await _rpRevoke.DeleteByIdAsync(_process.User.Id, profileId);
        }

        public async Task<bool> UpdateStatusAsync(int profileId, int status)
        {
            return await _rpRevoke.UpdateStatusAsync(_process.User.Id, profileId, status);
        }

        public async Task<bool> UpdateSimpleAsync(RevokeSimpleUpdate model, int profileId)
        {
            if (model == null)
                return ToResponse(false, Errors.invalid_data);
            if(model.AssigneeId==0)
            {
                return ToResponse(false, "Vui lòng chọn người xử lý");
            }
            return await _rpRevoke.UpdateSimpleAsync(model, _process.User.Id, profileId);
        }
    }
}

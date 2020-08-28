using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Collection;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class RevokeDebtBusiness : BaseBusiness, IRevokeDebtBusiness
    {
        protected readonly IRevokeDebtRepository _rpRevokce;
        public RevokeDebtBusiness(IRevokeDebtRepository revokeDebtRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpRevokce = revokeDebtRepository;
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
            var data = await _rpRevokce.SearchAsync(_process.User.Id, freeText, status, page, limit, groupId, assigneeId, fromDate, toDate, dateType, processStatus);
            if (data == null || !data.Any())
            {
                return DataPaging.Create(null as List<RevokeDebtSearch>, 0);
            }
            var result = DataPaging.Create(data, data[0].TotalRecord);
            return result;
        }

    }
}

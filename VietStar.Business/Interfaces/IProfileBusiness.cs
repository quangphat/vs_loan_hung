﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IProfileBusiness
    {
        Task<DataPaging<List<ProfileIndexModel>>> Gets(DateTime? fromDate, DateTime? toDate, int dateType = 1, int groupId = 0, int memberId = 0, string status = null, string freeText = null, int page = 1, int limit = 20);
    }
}

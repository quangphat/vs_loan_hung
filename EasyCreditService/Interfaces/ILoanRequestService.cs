﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;

namespace EasyCreditService.Interfaces
{
    public interface ILoanRequestService
    {
        Task<string> CreateLoan(LoanInfoRequestModel model, int type = 0);
    }
}

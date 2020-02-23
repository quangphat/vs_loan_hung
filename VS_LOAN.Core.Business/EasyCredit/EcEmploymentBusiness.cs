using LoanRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Business.EasyCredit
{
    public class EcEmploymentBusiness : BaseBusiness, IEcEmploymentBusiness
    {
        protected readonly IEcEmploymentRepository _rpEcEmployment;
        public EcEmploymentBusiness(CurrentProcess currentProcess, IEcEmploymentRepository ecEmploymentRepository) : base(typeof(EcEmploymentBusiness), currentProcess)
        {
            _rpEcEmployment = ecEmploymentRepository;
        }

        public async Task<List<StringOptionSimple>> GetEmployment(string type)
        {
            if(string.IsNullOrWhiteSpace(type))
            {
                _process.Error = errors.model_null;
                return null;
            }
            return await _rpEcEmployment.GetEmployment(type);
        }
    }
}

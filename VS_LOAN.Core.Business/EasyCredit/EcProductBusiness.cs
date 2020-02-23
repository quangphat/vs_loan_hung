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
    public class EcProductBusiness : BaseBusiness, IEcProductBusiness
    {
	protected readonly IEcProductRepository _rpEcProduct;
        public EcProductBusiness(CurrentProcess currentProcess, IEcProductRepository ecproductRepository) : base(typeof(EcProductBusiness), currentProcess)
        {
            _rpEcProduct = ecproductRepository;
        }

        public async Task<List<StringOptionSimple>> GetSimples(string occupationCode)
        {
            if(string.IsNullOrWhiteSpace(occupationCode))
            {
                _process.Error = errors.model_null;
                return null;
            }
            return await _rpEcProduct.GetSimples(occupationCode);
        }
    }
}

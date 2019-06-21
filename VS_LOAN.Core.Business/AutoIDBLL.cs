using VS_LOAN.Core.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Business
{
  public  class AutoIDBLL
    {
        public string GetAutoID(ref AutoIDModel autoId)
        {
            string value = "";
            if (autoId == null)
                autoId = new AutoIDModel();
            string suffixes = "00" + DateTime.Now.Month.ToString();
            suffixes = suffixes.Substring(suffixes.Length - 2, 2);
            if (autoId.Prefix == DateTime.Now.Year.ToString().Substring(2, 2))
            {

                if (autoId.Suffixes == suffixes)
                {
                    autoId.Value++;
                }
                else
                {
                    autoId.Suffixes = suffixes;
                    autoId.Value = 1;
                }
            }
            else
            {
                autoId.Prefix = DateTime.Now.Year.ToString().Substring(2, 2);
                autoId.Suffixes = suffixes;
                autoId.Value = 1;
            }

            int length = 6;
            string valueDefault = "000000" + autoId.Value.ToString();
            value = valueDefault.Substring(valueDefault.Length - length, length);

            return autoId.Prefix + autoId.Suffixes + value;
        }
    }
}

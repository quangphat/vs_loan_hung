using System.Linq.Expressions;

namespace SCTV.Scrm.Web.Helpers.DataTables
{
    public interface IParser
    {
        Expression GetSearchExpression();
    }
}
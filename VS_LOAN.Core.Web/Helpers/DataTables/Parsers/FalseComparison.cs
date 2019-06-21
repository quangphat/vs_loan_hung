using System.Linq.Expressions;

namespace SCTV.Scrm.Web.Helpers.DataTables
{
    public class FalseComparison : IParser
    {
        public static readonly Expression ReturnsFalse = Expression.Equal(Expression.Constant(false), Expression.Constant(true));
        public Expression GetSearchExpression()
        {
            return ReturnsFalse;
        }
    }
}
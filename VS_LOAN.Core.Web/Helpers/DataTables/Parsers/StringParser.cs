using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SCTV.Scrm.Web.Helpers.DataTables
{
    public class StringParser : IParser
    {
        private readonly SearchExpression expression;
        private readonly PropertyInfo propertyToSearch;
        private readonly ParameterExpression paramExpression;

        public StringParser(SearchExpression expression, PropertyInfo propertyToSearch, ParameterExpression paramExpression)
        {
            this.expression = expression;
            this.propertyToSearch = propertyToSearch;
            this.paramExpression = paramExpression;
        }

        public Expression GetSearchExpression()
        {
            try
            {
                var value = Expression.Constant(expression.Value.ToLower());
                return StringParserExpressionFactory.GetExpression(value, paramExpression, propertyToSearch, expression.Type);
            }
            catch (Exception)
            {
                return FalseComparison.ReturnsFalse;
            }
        }
    }
}
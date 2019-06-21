using System;
using System.Linq.Expressions;
using System.Reflection;
using SCTV.Scrm.Web.Helpers.DataTables;

namespace SCTV.Scrm.Web.Helpers.DataTables
{
    public class StringParserExpressionFactory
    {
        public static Expression GetExpression(ConstantExpression value, ParameterExpression parameterExpression, PropertyInfo propertyToSearch, ComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case ComparisonType.NotEquals:
                case ComparisonType.Equals:
                case ComparisonType.GreaterThanOrEquals:
                case ComparisonType.GreaterThan:
                case ComparisonType.LessThanOrEquals:
                case ComparisonType.LessThan:
                    return ComparableParserExpressionFactory.GetExpression(value,parameterExpression,propertyToSearch,comparisonType);
                case ComparisonType.StartsWith:
                    return GetLowweredExpression(value, parameterExpression, propertyToSearch, "StartsWith", new[] { typeof(string) });
                case ComparisonType.EndsWith:
                    return GetLowweredExpression(value, parameterExpression, propertyToSearch, "EndsWith", new[] { typeof(string) });
                default:
                    return GetLowweredExpression(value, parameterExpression, propertyToSearch, "StartsWith", new[] { typeof(string) });
            }
        }

        private static Expression GetLowweredExpression(ConstantExpression value, ParameterExpression parameterExpression, PropertyInfo propertyToSearch, string type, Type[] paramters = null)
        {
            var toLowerCall = Expression.Call(Expression.Property(Expression.Property(parameterExpression, "Val"), propertyToSearch), "ToLower", new Type[0]);
            var expression = Expression.Call(toLowerCall, GetMethod(type, paramters), value);

            var nullExpression = Expression.Constant(null, paramters[0]);
            var notNullExpression = Expression.NotEqual(Expression.Property(Expression.Property(parameterExpression, "Val"), propertyToSearch), nullExpression);

            var returnExpression =  Expression.AndAlso(notNullExpression, expression);

            return returnExpression;
        }

        private static MethodInfo GetMethod(string type, Type[] paramters)
        {
            return paramters == null
                       ? typeof (string).GetMethod(type)
                       : typeof (string).GetMethod(type, paramters);
        }
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SCTV.Scrm.Web.Helpers.DataTables
{
    public class ParseFactory
    {
        public static IParser GetParser(string searchText, PropertyInfo propertyToSearch, ParameterExpression paramExpression)
        {
            var expression = GetExpression(searchText);
            if (propertyToSearch.PropertyType == typeof(string))
            {
                return new StringParser(expression, propertyToSearch, paramExpression);
            }
            if (typeof(IComparable).IsAssignableFrom(propertyToSearch.PropertyType))
            {
                return new ComparableParsers(expression, propertyToSearch, paramExpression);
            }

            return new FalseComparison();
        }

        private static SearchExpression GetExpression(string searchText)
        {
            if(searchText.Length > 2)
            switch (searchText.Substring(0,2))
            {
                case "==":
                    return new SearchExpression { Type = ComparisonType.Equals, Value = searchText.Substring(2) };
                case ">=":
                    return new SearchExpression { Type = ComparisonType.GreaterThanOrEquals, Value = searchText.Substring(2) };
                case "<=":
                    return new SearchExpression { Type = ComparisonType.LessThanOrEquals, Value = searchText.Substring(2) };
                case "!=":
                    return new SearchExpression { Type = ComparisonType.NotEquals, Value = searchText.Substring(2) };
            }
            if (searchText.Length > 1)
            {
                switch (searchText.Substring(0, 1))
                {
                    case "*":
                        return new SearchExpression { Type = ComparisonType.EndsWith, Value = searchText.Substring(1) };
                    case "=":
                        return new SearchExpression { Type = ComparisonType.Equals, Value = searchText.Substring(1) };
                    case ">":
                        return new SearchExpression { Type = ComparisonType.GreaterThan, Value = searchText.Substring(1) };
                    case "<":
                        return new SearchExpression { Type = ComparisonType.LessThan, Value = searchText.Substring(1) };
                    case "-":
                    case "!":
                        return new SearchExpression { Type = ComparisonType.NotEquals, Value = searchText.Substring(1) };
                }
                switch (searchText.Last())
                {
                    case '*':
                        return new SearchExpression { Type = ComparisonType.StartsWith, Value = searchText.Substring(0,searchText.Length-1) };
                }
            }

            return new SearchExpression {Type = ComparisonType.Default, Value = searchText};
        }
    }
}
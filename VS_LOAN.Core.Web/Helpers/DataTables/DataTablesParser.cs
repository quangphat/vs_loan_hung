using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace SCTV.Scrm.Web.Helpers.DataTables
{
    public class DataTableParser<T> where T : class, new()
    {
        private const string IndividualSortKeyPrefix = "iSortCol_";
        private const string IndividualSortDirectionKeyPrefix = "sSortDir_";
        private const string IndividualSearcColPrefix = "mDataProp_";
        private const string IndividualSearchKeyPrefix = "bSearchable_";
        private const string IndividualSearchTextPrefix = "sSearch_";
        private const string DisplayStart = "iDisplayStart";
        private const string DisplayLength = "iDisplayLength";
        private const string ECHO = "sEcho";
        private const string AscendingSort = "asc";
        private readonly Type type;
        private readonly PropertyInfo[] properties;
        private readonly PropertyInfo[] searchableProperties;
        private readonly PropertyInfo[] sortableProperties;

        public DataTableParser()
        {
            type = typeof(T);
            properties = type.GetProperties();
            searchableProperties = GetSearchableProperties(properties);
            sortableProperties = GetSortableProperties(properties);
        }

        public FormatedList ParseForClientSide(IQueryable<T> queriable)
        {
            var list = new FormatedList();
            list.aaData = queriable
                            .AsEnumerable()
                            .Select(SelectProperties)
                            .ToList();
            return list;
        }

        public FormatedList Parse(IQueryable<T> queriable)
        {
            var request = GetRequestFromHttpContext();
            var list = new FormatedList();
            list.Import(properties.Select(x => x.Name).ToArray());
            list.sEcho = request.TableEcho;

            //var totalCount = queriable.Count();
            var orderedQueryable = ApplySort(queriable, request);
            var setData = orderedQueryable.Select(x => new PreFilterTotal<T> { Val = x });
            var unpagedData = setData.Where(ApplyGenericSearch(request));
            unpagedData = unpagedData.Where(IndividualPropertySearch(request));

            var filteredCount = unpagedData.Count();
            var postFilter = unpagedData.Select(x => new PostFilterTotal<T>
                                {
                                    Val = x.Val
                                });

            var pagedResults = postFilter
                .Skip(request.Skip)
                .Take(request.Take);
            var results = pagedResults.ToList();

            if (!results.Any())
            {
                list.aaData = new List<T>().Select(SelectProperties);
                return list;
            }

            list.aaData = results.Select(x => x.Val).Select(SelectProperties).ToList();
            list.iTotalDisplayRecords = filteredCount;
            list.iTotalRecords = filteredCount;

            return list;
        }

        private DataTableRequest GetRequestFromHttpContext()
        {
            var httpRequest = new HttpRequestWrapper(HttpContext.Current.Request);
            var sortColumns = new List<DataTableColumn>();
            foreach (var key in httpRequest.Params.AllKeys.Where(x => x.StartsWith(IndividualSortKeyPrefix)))
            {
                var sortcolumn = int.Parse(httpRequest[key]);

                if (sortcolumn < 0 || sortcolumn >= properties.Length)
                    break;
                var sortdir = httpRequest[IndividualSortDirectionKeyPrefix + key.Replace(IndividualSortKeyPrefix, string.Empty)];
                sortColumns.Add(new DataTableColumn
                                    {
                                        ColumnIndex = sortcolumn,
                                        SortDirection = sortdir
                                    });
            }

            foreach (var key in httpRequest.Params.AllKeys.Where(x => x.StartsWith(IndividualSearchKeyPrefix)))
            {
                var searchable = bool.Parse(httpRequest[key]);
                
                if (!searchable)
                    continue;
                var searchCol = int.Parse(httpRequest[IndividualSearcColPrefix + key.Replace(IndividualSearchKeyPrefix, string.Empty)]);
                var searchText = httpRequest[IndividualSearchTextPrefix + key.Replace(IndividualSearchKeyPrefix, string.Empty)];
                var sortColumn = sortColumns.FirstOrDefault(x => x.ColumnIndex == searchCol);
                if(sortColumn != null)
                {
                    sortColumn.SearchTerm = searchText;
                }
                else
                {
                    sortColumns.Add(new DataTableColumn
                    {
                        ColumnIndex = searchCol,
                        SearchTerm = searchText
                    });
                }
            }

            string search = httpRequest["sSearch"];
            int echo = int.Parse(httpRequest[ECHO] ?? "1");
            int skip, take;
            int.TryParse(httpRequest[DisplayStart], out skip);
            int.TryParse(httpRequest[DisplayLength], out take);

            return new DataTableRequest
            {
                AllSearch = search,
                Skip = skip,
                Take = take,
                TableEcho = echo,
                Columns = sortColumns
            };
        }

        private IOrderedQueryable<T> ApplySort(IQueryable<T> queriable, DataTableRequest request)
        {
            IOrderedQueryable<T> orderedQueryable = null;
            foreach (var dataTableColumn in request.Columns)
            {
                var sortcolumn = dataTableColumn.ColumnIndex;
                if (sortcolumn < 0 || sortcolumn >= properties.Length)
                    continue;

                var property = properties[sortcolumn];
                if (!sortableProperties.Contains(property))
                    continue;

                var sortdir = dataTableColumn.SortDirection;
                if(string.IsNullOrEmpty(sortdir))
                    continue;

                orderedQueryable = SortOnColumn(queriable, orderedQueryable, sortcolumn, sortdir);
            }

            return orderedQueryable ?? SortOnColumn(queriable, null, 0, AscendingSort);
        }

        private IOrderedQueryable<T> SortOnColumn(IQueryable<T> queriable, IOrderedQueryable<T> orderedQueryable, int sortcolumn, string sortDirection)
        {
            var method = typeof(DataTableParser<T>).GetMethod("ApplyTypeSort", BindingFlags.NonPublic | BindingFlags.Instance);
            var generic = method.MakeGenericMethod(properties[sortcolumn].PropertyType);
            return (IOrderedQueryable<T>)generic.Invoke(this, 
                                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                                        null,
                                                        new object[] { queriable, orderedQueryable, sortcolumn, sortDirection }, 
                                                        null);
        }

        // ReSharper disable UnusedMember.Local - called via reflection
        private IOrderedQueryable<T> ApplyTypeSort<TSearch>(IQueryable<T> queriable, IOrderedQueryable<T> orderedQueryable, int sortcolumn, string sortdir)
        // ReSharper restore UnusedMember.Local
        {
            var paramExpr = Expression.Parameter(typeof(T), "val");
            var property = Expression.Property(paramExpr, properties[sortcolumn]);
            var propertyExpr = Expression.Lambda<Func<T, TSearch>>(property, paramExpr);

            if (string.IsNullOrEmpty(sortdir) || sortdir.Equals(AscendingSort, StringComparison.OrdinalIgnoreCase))
                orderedQueryable = orderedQueryable == null
                                       ? queriable.OrderBy(propertyExpr)
                                       : orderedQueryable.ThenBy(propertyExpr);
            else
                orderedQueryable = orderedQueryable == null
                                       ? queriable.OrderByDescending(propertyExpr)
                                       : orderedQueryable.ThenByDescending(propertyExpr);

            return orderedQueryable;
        }

        private PropertyInfo[] GetSortableProperties(IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.ToArray();
            //return propertyInfos.Where(x => x.GetCustomAttributes(typeof(SortableAttribute), false).Any()).ToArray();
        }

        private PropertyInfo[] GetSearchableProperties(IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.ToArray();
            //return propertyInfos.Where(x => x.GetCustomAttributes(typeof(SearchableAttribute), false).Any()).ToArray();
        }

        private Func<T, IEnumerable<object>> SelectProperties
        {
            get
            {
                return value => properties.Select(prop =>
                                                      {
                                                          var obj = prop.GetValue(value, new object[0]);

                                                          if (obj is DateTime)
                                                          {
                                                              var dateTime = (DateTime)obj;
                                                              var epoch = new DateTime(1970, 1, 1);
                                                              var span = new TimeSpan(dateTime.Ticks - epoch.Ticks);
                                                              return Convert.ToString(span.TotalMilliseconds);
                                                          }
                                                          if (obj is bool)
                                                          {
                                                              return Convert.ToString(obj);
                                                          }
                                                          if (obj is TimeZoneInfo)
                                                          {
                                                              return ((TimeZoneInfo)obj).StandardName;
                                                          }
                                                          return obj;
                                                      }).ToList();
            }
        }

        private Expression<Func<PreFilterTotal<T>, bool>> IndividualPropertySearch(DataTableRequest request)
        {
            var paramExpression = Expression.Parameter(typeof(PreFilterTotal<T>), "val");
            Expression compoundExpression = Expression.Constant(true);

            foreach (var dataTableColumn in request.Columns)
            {
                var query = dataTableColumn.SearchTerm;

                if (string.IsNullOrEmpty(query))
                    continue;

                var searchColumn = dataTableColumn.ColumnIndex;
                if (searchColumn < 0 || searchColumn >= properties.Length)
                    continue;

                var propertyToSearch = properties[searchColumn];
                if (!searchableProperties.Contains(propertyToSearch))
                    continue;

                var expression = ParseFactory.GetParser(query, propertyToSearch, paramExpression).GetSearchExpression();
                compoundExpression = Expression.And(compoundExpression, expression);
            }
            var result = Expression.Lambda<Func<PreFilterTotal<T>, bool>>(compoundExpression, paramExpression);
            return result;
        }

        private Expression<Func<PreFilterTotal<T>, bool>> ApplyGenericSearch(DataTableRequest request)
        {
            string search = request.AllSearch;

            if (string.IsNullOrEmpty(search) || properties.Length == 0)
                return x => true;

            var paramExpression = Expression.Parameter(typeof(PreFilterTotal<T>), "val");
            var searchValues = GetSearchTerms(search).ToList();

            Expression compoundExpression = null;
            foreach (var searchValue in searchValues)
            {
                if (string.IsNullOrEmpty(searchValue)) continue;

                Expression singleValue = Expression.Constant(false);
                var tempPropertyQueries = GetSearchQueries(searchValue, paramExpression).ToList();
                List<Expression> propertyQueries = new List<Expression>();

                var searchableColumns = request.Columns.OrderBy(c => c.ColumnIndex).Select(c => c.ColumnIndex).ToList();
                for (int i = 0; i < tempPropertyQueries.Count(); i++)
                {
                    if (searchableColumns.Contains(i))
                    {
                        propertyQueries.Add(tempPropertyQueries[i]);
                    }
                }

                foreach (var propertyQuery in propertyQueries)
                {
                    singleValue = Expression.Or(singleValue, propertyQuery);
                }

                //compoundExpression = Expression.And(compoundExpression, singleValue);
                if (compoundExpression == null)
                {
                    compoundExpression = singleValue;
                }
                else
                {
                    compoundExpression = Expression.Or(compoundExpression, singleValue);
                }
                
            }

            var result = Expression.Lambda<Func<PreFilterTotal<T>, bool>>(compoundExpression, paramExpression);
            return result;
        }

        private IEnumerable<string> GetSearchTerms(string value)
        {
            var quotedList = new List<string>();

            var regex = new Regex("[^ ]*(\"([^\"]+)\")[^ ]*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            for (var match = regex.Match(value); match.Success; match = match.NextMatch())
            {
                quotedList.Add(match.Groups[0].Value);
            }

            foreach (var quoted in quotedList)
            {
                var index = value.IndexOf(quoted, StringComparison.OrdinalIgnoreCase);
                value = value.Substring(0, index) +
                        value.Substring(index + quoted.Length, value.Length - (index + quoted.Length));
            }

            regex = new Regex("[^ ]*[^ ]+[^ ]*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            for (var match = regex.Match(value); match.Success; match = match.NextMatch())
            {
                quotedList.Add(match.Groups[0].Value);
            }

            return quotedList.Select(x => x.Replace("\"", ""));
        }

        private IEnumerable<Expression> GetSearchQueries(string searchExpression, ParameterExpression paramExpression)
        {
            return properties.Where(x => searchableProperties.Contains(x)).Select(property => ParseFactory.GetParser(searchExpression, property, paramExpression).GetSearchExpression());
        }
    }
}
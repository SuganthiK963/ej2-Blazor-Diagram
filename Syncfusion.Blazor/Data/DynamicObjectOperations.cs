using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Syncfusion.Blazor.Data;
using System.Dynamic;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// DataOperation class that performs data operation in DynamicObject type data sources.
    /// </summary>
    public static class DynamicObjectOperation
    {
        /// <summary>
        /// Executes the query against the given data source and returns the resultant records.
        /// </summary>
        /// <param name="DataSource">Input data source.</param>
        /// <param name="queries">Query to be executed against data source.</param>
        /// <returns>IEnumerable - resultant records.</returns>
        public static IEnumerable PerformDataOperations(IEnumerable DataSource, DataManagerRequest queries)
        {
            IDictionary<string, Type> columnTypes = DataUtil.GetColumnType((IEnumerable)DataSource);
            Type sourceType = DataSource.GetElementType();
            if (sourceType == null)
            {
                Type type1 = sourceType.GetType();
                sourceType = type1.GetElementType();
            }

            if (queries != null && queries.Where?.Count > 0) // perform Filtering
            {
                DataSource = DynamicObjectOperation.PerformFiltering(DataSource, queries.Where, queries.Where[0].Operator, columnTypes);
            }

            if(queries != null && queries.Where?.Count > 0 && queries.Search?.Count > 0)
            {
                var methodInfo = typeof(Enumerable).GetMethod("Cast");
                var genericMethod = methodInfo.MakeGenericMethod(sourceType);
                DataSource = genericMethod.Invoke(null, new[] { DataSource }) as IEnumerable;
            }

            if (queries != null && queries.Search?.Count > 0) // perform Searching
            {
                DataSource = DynamicObjectOperation.PerformSearching(DataSource, queries.Search, columnTypes);
            }

            if (queries != null && queries.Sorted?.Count > 0) // perform Sorting
            {
                DataSource = DynamicObjectOperation.PerformSorting(DataSource.AsQueryable(), queries.Sorted);
            }

            return DataSource;
        }

        /// <summary>
        /// Sorts the given data source.
        /// </summary>
        /// <param name="dataSource">Input data source to be sorted.</param>
        /// <param name="sortedColumns">List of sort criteria.</param>
        /// <returns>IQuerable.</returns>
        public static IQueryable PerformSorting(IQueryable dataSource, List<Sort> sortedColumns)
        {
            bool firstTime = true;
            IQueryable<IDynamicMetaObjectProvider> dt = dataSource.Cast<IDynamicMetaObjectProvider>().AsQueryable();
            IOrderedQueryable<IDynamicMetaObjectProvider> data;
            Type sourceType = dt.GetObjectType();
            List<SortedColumn> sortedColumn = new List<SortedColumn>();
            if (sortedColumns != null && sortedColumns.Count > 1)
            {
                sortedColumns.Reverse();
            }

            foreach (var column in sortedColumns ?? new List<Sort>())
            {
                var direction = (SortOrder)Enum.Parse(typeof(SortOrder), column.Direction.ToString(), true);
                sortedColumn.Add(new SortedColumn { Direction = direction, Field = column.Name });
            }

            var count = dataSource.Cast<object>().Count();
            if (count == 0)
            {
                return dt;
            }

            var isDynamicObjectType = dataSource.Cast<object>().ToList()[0].GetType().BaseType == typeof(DynamicObject);
            var isExpandoObjectType = dataSource.Cast<object>().ToList()[0].GetType() == typeof(ExpandoObject);
            foreach (var column in sortedColumn)
            {
                if (column.Direction == SortOrder.Ascending)
                {
                    if (firstTime)
                    {
                        if (isDynamicObjectType)
                        {
                            dt = dt.OrderBy(x => DataUtil.GetObject(column.Field, x));
                        }
                        else if (isExpandoObjectType)
                        {
                            dt = dt.OrderBy(x => DataUtil.GetObject(column.Field, x));
                        }

                        firstTime = false;
                    }
                    else
                    {
                        if (isDynamicObjectType)
                        {
                            data = (IOrderedQueryable<IDynamicMetaObjectProvider>)dt;
                            dt = data.ThenBy(x => DataUtil.GetObject(column.Field, x));
                        }
                        else if (isExpandoObjectType)
                        {
                            data = (IOrderedQueryable<IDynamicMetaObjectProvider>)dt;
                            dt = data.ThenBy(x => DataUtil.GetObject(column.Field, x));
                        }
                    }
                }
                else
                {
                    if (firstTime)
                    {
                        if (isDynamicObjectType)
                        {
                            dt = dt.OrderByDescending(x => DataUtil.GetObject(column.Field, x));
                            firstTime = false;
                        }
                        else if (isExpandoObjectType)
                        {
                            dt = dt.OrderByDescending(x => DataUtil.GetObject(column.Field, x));
                        }
                    }
                    else
                    {
                        if (isDynamicObjectType)
                        {
                            data = (IOrderedQueryable<IDynamicMetaObjectProvider>)dt;
                            dt = data.ThenByDescending(x => DataUtil.GetObject(column.Field, x));
                        }
                        else if (isExpandoObjectType)
                        {
                            data = (IOrderedQueryable<IDynamicMetaObjectProvider>)dt;
                            dt = data.ThenByDescending(x => DataUtil.GetObject(column.Field, x));
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Apply the given filter criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Input data source.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Condition to merge two filter criteria.</param>
        /// <param name="columnTypes">Type collection of each property in data source.</param>
        /// <returns></returns>
        public static IQueryable PerformFiltering(IEnumerable dataSource, List<WhereFilter> whereFilter, string condition, IDictionary<string, Type> columnTypes = null)
        {
            IQueryable<IDynamicMetaObjectProvider> data = dataSource.Cast<IDynamicMetaObjectProvider>().ToList().AsQueryable();
            var paramExpression = Expression.Parameter(typeof(object));
            Expression predicate = PredicateBuilder(dataSource, whereFilter, condition, paramExpression, columnTypes);
            if (predicate == null)
            {
                return data;
            }

            var bExp = Expression.Lambda<Func<IDynamicMetaObjectProvider, bool>>(predicate, paramExpression);
            data = data.Where(bExp);
            return data;
        }

        private static Type GetColumnType(IEnumerable dataSource, string filterString, bool nullable = true)
        {
            Type type = null; _ = nullable;
            var propertyList = filterString.Split(".");
            var propCount = propertyList.Length;
            bool isComplex = propCount > 1;
            var rowType = dataSource.Cast<IDynamicMetaObjectProvider>().ToList()[0].GetType();
            if (rowType.IsSubclassOf(typeof(DynamicObject)))
            {
                var rowData = dataSource.Cast<DynamicObject>().ToList()[0];
                if (DataUtil.GetDynamicValue(rowData,filterString) == null)
                {
                    rowData = dataSource.Cast<DynamicObject>().Where(x => DataUtil.GetDynamicValue(x, filterString) != null).FirstOrDefault();
                }

                object propertyValue = null;
                if (isComplex) // handling Dynamic complex object and type
                {
                    for (int i = 0; i < propCount; i++)
                    {
                        if (propertyValue is DynamicObject)
                        {
                            (propertyValue as DynamicObject).TryGetMember(new DataMemberBinder(propertyList[i], false), out var value);
                            propertyValue = value;
                        }
                        else
                        {
                            rowData.TryGetMember(new DataMemberBinder(propertyList[i], false), out var value);
                            propertyValue = value;
                        }
                    }
                }
                else
                {
                    rowData.TryGetMember(new DataMemberBinder(filterString, false), out var value);
                    propertyValue = value;
                }

                return propertyValue?.GetType();
            }

            foreach (var item in dataSource.Cast<ExpandoObject>().ToList())
            {
                IDictionary<string, object> propertyValues = item;
                var value = propertyValues[filterString];
            }

            return type;
        }

        private static Type ColumnType(IDictionary<string, Type> columns, string field, object data = null)
        {
            Type type = null;
            var Fields = field.Split('.');
            IDictionary<string, Type> dynamicType = null;
            var Complex = Fields.Length;

            if (Complex > 1)
            {
                for (var i = 0; i < Complex; i++)
                {
                    if (data != null && data is ExpandoObject)
                    {
                        var customData = new object();
                        if (i != 0)
                        {
                            customData = data.GetType().GetProperty(Fields[i - 1])?.GetValue(data);
                            dynamicType = customData != null ? DataUtil.GetColumnType(new List<object>() { customData }, true) : null;
                            if (dynamicType != null && dynamicType.ContainsKey(Fields[i]))
                            {
                                type = dynamicType[Fields[i]];
                            }
                        }
                        else
                        {
                            var expandoData = (IDictionary<string, object>)data;

                            dynamicType = DataUtil.GetColumnType(new List<object>() { expandoData[Fields[i]] }, true);
                            type = dynamicType[Fields[i + 1]];
                        }
                    }
                }
            }
            else
            {
                type = columns[field];
            }

            return type;
        }

        /// <summary>
        /// Apply the given search criteria against the data source and returns the filtered records.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="searchFilter">List of search criteria.</param>
        /// <returns>IEnumerable - searched records.</returns>
        /// <param name="columnTypes">Type collection of each property in data source.</param>
        public static IQueryable PerformSearching(IEnumerable dataSource, List<SearchFilter> searchFilter, IDictionary<string, Type> columnTypes = null)
        {
            IQueryable<IDynamicMetaObjectProvider> data = null;
            Type columnType = null;
            Type type = dataSource.GetElementType();
            Type t = typeof(object);
            if (type == null)
            {
                Type type1 = dataSource?.GetType();
                type = type1?.GetElementType();
            }

            foreach (var filter in searchFilter ?? new List<SearchFilter>())
            {
                var paramExpression = Expression.Parameter(typeof(object));
                var initialLoop = true;
                Expression predicate = null;
                var op = filter.Operator;
                if (op == "equal")
                {
                    op = "equals";
                }
                else if (op == "notequal")
                {
                    op = "notequals";
                }

                FilterType FilterType = (FilterType)Enum.Parse(typeof(FilterType), op.ToString(), true);
                foreach (string fields in filter.Fields)
                {
                    if (columnTypes != null)
                    {
                        columnType = ColumnType(columnTypes, fields, dataSource.Cast<ExpandoObject>().ToList().ElementAt(0));
                    }
                    else
                    {
                        columnType = GetColumnType(dataSource, fields);
                    }

                    if (initialLoop)
                    {
                        predicate = dataSource.AsQueryable().Predicate(paramExpression, fields, filter.Key, FilterType, FilterBehavior.StringTyped, false, type, columnType);
                        initialLoop = false;
                    }
                    else
                    {
                        predicate = predicate.OrPredicate(dataSource.AsQueryable().Predicate(paramExpression, fields, filter.Key, FilterType, FilterBehavior.StringTyped, false, type, columnType));
                    }
                }

                var qu = Expression.Lambda<Func<IDynamicMetaObjectProvider, bool>>(predicate, paramExpression);
                List<IDynamicMetaObjectProvider> dt = dataSource.Cast<IDynamicMetaObjectProvider>().ToList();
                data = dt.AsQueryable().Where(qu);
            }

            return data;
        }

        /// <summary>
        /// Generates predicate from the filter criteria.
        /// </summary>
        /// <param name="dataSource">Data source to be filtered.</param>
        /// <param name="whereFilter">List of filter criteria.</param>
        /// <param name="condition">Condition to merge two filter criteria.</param>
        /// <param name="paramExpression">Parameter expression.</param>
        /// <param name="columnTypes">Type collection of each property in data source.</param>
        /// <returns>Expression.</returns>
        public static Expression PredicateBuilder(IEnumerable dataSource, List<WhereFilter> whereFilter, string condition, ParameterExpression paramExpression, IDictionary<string, Type> columnTypes = null)
        {
            Type columnType = null;
            Type type = dataSource.GetElementType();
            if (type == null)
            {
                Type type1 = dataSource?.GetType();
                type = type1?.GetElementType();
            }

            Expression predicate = null;
            foreach (var filter in whereFilter ?? new List<WhereFilter>())
            {
                if (filter.IsComplex)
                {
                    if (predicate == null)
                    {
                        predicate = PredicateBuilder(dataSource, filter.predicates, filter.Condition, paramExpression, columnTypes);
                    }
                    else
                    {
                        if (condition == "or")
                        {
                            predicate = predicate.OrElsePredicate(PredicateBuilder(dataSource, filter.predicates, filter.Condition, paramExpression, columnTypes));
                        }
                        else
                        {
                            predicate = predicate.AndAlsoPredicate(PredicateBuilder(dataSource, filter.predicates, filter.Condition, paramExpression, columnTypes));
                        }
                    }
                }
                else
                {
                    var op = filter.Operator;
                    if (op == "equal")
                    {
                        op = "equals";
                    }
                    else if (op == "notequal")
                    {
                        op = "notequals";
                    }

                    FilterType filterType = (FilterType)Enum.Parse(typeof(FilterType), op.ToString(), true);
                    var value = filter.value;
                    if (columnTypes != null)
                    {
                        // TODOComplex: check the datasource while parsing
                        columnType = ColumnType(columnTypes, filter.Field, dataSource.Cast<IDynamicMetaObjectProvider>().ToList().ElementAt(0));
                    }
                    else
                    {
                        columnType = GetColumnType(dataSource, filter.Field);
                    }

                    if (columnType == null)
                    {
                        return predicate;
                    }

                    if (predicate == null)
                    {
                        predicate = dataSource.AsQueryable().Predicate(paramExpression, filter.Field, value, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, columnType);
                    }
                    else
                    {
                        if (condition == "or")
                        {
                            predicate = predicate.OrElsePredicate(dataSource.AsQueryable().Predicate(paramExpression, filter.Field, value, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, columnType));
                        }
                        else
                        {
                            predicate = predicate.AndPredicate(dataSource.AsQueryable().Predicate(paramExpression, filter.Field, value, filterType, FilterBehavior.StringTyped, !filter.IgnoreCase, type, columnType));
                        }
                    }
                }
            }

            return predicate;
        }
    }
}

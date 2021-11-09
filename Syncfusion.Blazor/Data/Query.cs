using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Provides methods to generate query which can be executed against data source using <see cref="SfDataManager"/>.
    /// </summary>
    /// <remarks>Methods in this class are chainable.</remarks>
    [JsonConverter(typeof(QueryConverter))]
    public class Query
    {
        /// <summary>
        /// Provides various method that allow user to generate query.
        /// These queries is used by <see cref="SfDataManager"/> to process given data source and returns resultant records.
        /// </summary>
        public DataManagerRequest Queries { get; set; } = new DataManagerRequest();

        /// <summary>
        /// Specifies the primary key value.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Specifies the foreign key value.
        /// </summary>
        public string FKey { get; set; }

        /// <summary>
        /// Specifies the table name.
        /// </summary>
        /// <remarks>Table name is used by the remote adaptors.</remarks>
        public string FromTable { get; set; }

        /// <summary>
        /// Specifies the lookup table names.
        /// </summary>
        public string[] Lookups { get; set; }

        /// <summary>
        /// Specifies the relation table/resource names.
        /// </summary>
        public List<object> Expands { get; set; }

        /// <summary>
        /// Gets the sort column details.
        /// </summary>
        public object[] SortedColumns { get; set; }

        /// <summary>
        /// Gets the group column details.
        /// </summary>
        public object[] GroupedColumns { get; set; }

        /// <summary>
        /// Specifies the sub query details.
        /// </summary>
        public string SubQuerySelector { get; set; }

        /// <summary>
        /// Specifies the sub query.
        /// </summary>
        public Query SubQuery { get; set; }

        /// <summary>
        /// Specifies the presence of child.
        /// </summary>
        public bool IsChild { get; set; }

        /// <summary>
        /// Gets the additional parameters to be used.
        /// </summary>
        public IDictionary<string, object> Params { get; set; }

        /// <summary>
        /// Specifies that count value is required in responses from remote services.
        /// </summary>
        public bool IsCountRequired { get; set; }

        /// <summary>
        /// Gets the data manager instance.
        /// </summary>
        public DataManager DataManager { get; set; }

        /// <summary>
        /// Gets the list of distinct values.
        /// </summary>
        public List<string> Distincts { get; set; }

        /// <summary>
        /// Gets the id mapping value used for child data source process.
        /// </summary>
        public string IdMapping { get; set; }

        /// <summary>
        /// Adds the table or resource name.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>Query.</returns>
        public Query From(string tableName)
        {
            FromTable = tableName;
            Queries.Table = tableName;
            return this;
        }

        /// <summary>
        /// Sets the current page index and page size.
        /// </summary>
        /// <param name="pageIndex">Specifies the current page.</param>
        /// <param name="pageSize">Specifies the page size.</param>
        /// <returns>Query.</returns>
        /// <remarks>Use this method to get chunk of records from the given data source.</remarks>
        public Query Page(int pageIndex, int pageSize)
        {
            var skip = pageIndex;
            Queries.Take = pageSize;
            Queries.Skip = (skip - 1) * Queries.Take;
            return this;
        }

        /// <summary>
        /// Sets the take index.
        /// </summary>
        /// <param name="pageSize">Maximum index of the record.</param>
        /// <returns>Query.</returns>
        public Query Take(int pageSize)
        {
            Queries.Take = pageSize;
            return this;
        }

        /// <summary>
        /// Sets the current page index and page size.
        /// </summary>
        /// <param name="pageIndex">Specifies the current page.</param>
        /// <param name="pageSize">Specifies the page size.</param>
        /// <returns>Query.</returns>
        public Query Skip(int pageIndex, int pageSize)
        {
            Queries.Skip = pageIndex - (1 * Queries.Take); _ = pageSize;
            return this;
        }

        /// <summary>
        /// Sets the skip index.
        /// </summary>
        /// <param name="skip">Number of records to skip.</param>
        /// <returns>Query.</returns>
        public Query Skip(int skip)
        {
            Queries.Skip = skip;
            return this;
        }

        /// <summary>
        /// Gets the range of records.
        /// </summary>
        /// <param name="start">Range start index.</param>
        /// <param name="end">Range end index.</param>
        /// <returns>Query.</returns>
        public Query Range(int start, int end)
        {
            Queries.Skip = start;
            Queries.Take = end;
            return this;
        }

        /// <summary>
        /// Selects the given fields from data source.
        /// </summary>
        /// <param name="fieldNames">List of fields to select.</param>
        /// <returns>Query.</returns>
        public Query Select(List<string> fieldNames)
        {
            Queries.Select = Queries.Select ?? new List<string>();
            fieldNames?.ForEach((select) => Queries.Select.Add(select));
            return this;
        }

        /// <summary>
        /// Filters the records with the given query.
        /// </summary>
        /// <param name="fieldName">Specifies the field name.</param>
        /// <param name="operator">Specifies the operator.</param>
        /// <param name="value">Specifies the filter value.</param>
        /// <param name="ignoreCase">Performs case sensitive filter.</param>
        /// <param name="ignoreAccent">Ignore accents/diacritic words during filtering.</param>
        /// <returns>Query.</returns>
        /// <remarks>Multiple Where method can be chained to create complex filter criteria.</remarks>
        public Query Where(string fieldName, string @operator = null, object value = null, bool ignoreCase = false, bool ignoreAccent = false)
        {
            Queries.Where = Queries.Where ?? new List<WhereFilter>(); _ = ignoreAccent;
            Queries.Where.Add(new WhereFilter() { Field = fieldName, Operator = @operator, value = value, IgnoreCase = ignoreCase });
            return this;
        }

        /// <summary>
        /// Filters the records with the given query.
        /// </summary>
        /// <param name="predicate">Specifies the predicate to be used.</param>
        /// <returns>Query.</returns>
        /// <remarks>Multiple Where method can be chained to create complex filter criteria.</remarks>
        public Query Where(WhereFilter predicate)
        {
            Queries.Where = Queries.Where ?? new List<WhereFilter>();
            Queries.Where.Add(predicate);
            return this;
        }

        /// <summary>
        /// Filters the records with the given query.
        /// </summary>
        /// <param name="predicates">Specifies the list of predicates to be used.</param>
        /// <returns>Query</returns>
        /// <remarks>Multiple Where method can be chained to create complex filter criteria.</remarks>
        public Query Where(List<WhereFilter> predicates)
        {
            if (predicates == null) { throw new ArgumentNullException(nameof(predicates)); }
            Queries.Where = this.Queries.Where ?? new List<WhereFilter>();
            predicates.ForEach((predicate) => this.Queries.Where.Add(predicate));
            return this;
        }

        /// <summary>
        /// Searches the records with the given query.
        /// </summary>
        /// <param name="searchKey">Specifies the search key.</param>
        /// <param name="fieldNames">Specifies the field names.</param>
        /// <param name="operator">Specifies the search operator.</param>
        /// <param name="ignoreCase">Performs case sensitive search.</param>
        /// <param name="ignoreAccent">Ignore accents/diacritic words during searching.</param>
        /// <returns></returns>
        /// <remarks>Multiple Search method can be chained to create complex search criteria.</remarks>
        public Query Search(string searchKey, List<string> fieldNames, string @operator = null, bool ignoreCase = false, bool ignoreAccent = false)
        {
            @operator = @operator != null && @operator != "none" ? @operator.ToString() : "contains"; _ = ignoreCase; _ = ignoreAccent;
            Queries.Search = Queries.Search ?? new List<SearchFilter>();
            Queries.Search.Add(new SearchFilter() { Fields = fieldNames, Operator = @operator, Key = searchKey });
            return this;
        }

        /// <summary>
        /// Specifies that count is expected in remote service response.
        /// </summary>
        /// <returns>Query.</returns>
        public Query RequiresCount()
        {
            Queries.RequiresCounts = true;
            IsCountRequired = true;
            return this;
        }

        /// <summary>
        /// Sorts the data source.
        /// </summary>
        /// <param name="name">Specifies the sort name.</param>
        /// <param name="direction">Specifies the sort direction.</param>
        /// <returns>Query.</returns>
        public Query Sort(string name, string direction = null)
        {
            var sortDirection = direction ?? "Ascending";
            Queries.Sorted = Queries.Sorted ?? new List<Sort>();
            Queries.Sorted.Add(new Sort() { Name = name, Direction = sortDirection});
            return this;
        }

        /// <summary>
        /// Sorts the data source.
        /// </summary>
        /// <param name="name">Specifies the sort name.</param>
        /// <param name="direction">Specifies the sort direction.</param>
        /// <param name="comparer">Specifies the comparer object.</param>
        /// <returns>Query.</returns>
        public Query Sort(string name, string direction = null, object comparer = null)
        {
            var sortDirection = direction ?? "Ascending";
            Queries.Sorted = Queries.Sorted ?? new List<Sort>();
            Queries.Sorted.Add(new Sort() { Name = name, Direction = sortDirection, Comparer = comparer });
            return this;
        }

        /// <summary>
        /// Groups the data source.
        /// </summary>
        /// <param name="fieldNames">Specifies the column names to group.</param>
        /// <param name="groupFormat">Specifies the group format.</param>
        /// <returns>Query.</returns>
        public Query Group(List<string> fieldNames, IDictionary<string, string> groupFormat = null)
        {
            Queries.Group = Queries.Group ?? new List<string>();
            foreach(string fieldName in fieldNames ?? new List<string>())
            {
                Queries.Group.Add(fieldName);
            }

            if (groupFormat != null)
            {
                Queries.GroupByFormatter = Queries.GroupByFormatter ?? new Dictionary<string, string>();
                Queries.GroupByFormatter = Queries.GroupByFormatter.Concat(groupFormat).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }

            return this;
        }

        /// <summary>
        /// Groups the data source.
        /// </summary>
        /// <param name="fieldName">Specifies the column name.</param>
        /// <returns>Query.</returns>
        public Query Group(string fieldName)
        {
            Queries.Group = Queries.Group ?? new List<string>();
            Queries.Group.Add(fieldName);
            return this;
        }

        /// <summary>
        /// Groups the data source.
        /// </summary>
        /// <param name="fieldName">Specifies the column name.</param>
        /// <param name="columnFormat">Specifies the column format.</param>
        /// <returns>Query.</returns>
        public Query Group(string fieldName, string columnFormat)
        {
            Queries.Group = Queries.Group ?? new List<string>();
            Queries.Group.Add(fieldName);
            Queries.GroupByFormatter = Queries.GroupByFormatter ?? new Dictionary<string, string>();
            if (Queries.GroupByFormatter.ContainsKey(fieldName))
            {
                Queries.GroupByFormatter[fieldName] = columnFormat;
            }
            else
            {
                Queries.GroupByFormatter.Add(fieldName, columnFormat);
            }

            return this;
        }

        /// <summary>
        /// Performs the aggregate operation in the aggregate type.
        /// </summary>
        /// <param name="field">Specifies the field name.</param>
        /// <param name="type">Specifies the aggregate type.</param>
        /// <returns>Query.</returns>
        /// <remarks>Multiple aggregation can be performed by chaining Aggregates method.</remarks>
        public Query Aggregates(string field, string type)
        {
            Queries.Aggregates = Queries.Aggregates ?? new List<Aggregate>();
            Queries.Aggregates.Add(new Aggregate() { Field = field, Type = type });
            return this;
        }

        /// <summary>
        /// Adds additional parameters to the HTTP request sent by <see cref="SfDataManager"/>.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="value">Additional parameter value.</param>
        /// <returns>Query.</returns>
        public Query AddParams(string key, object value)
        {
            Queries.Params = Queries.Params ?? new Dictionary<string, object>();
            if (Queries.Params.ContainsKey(key))
            {
                Queries.Params[key] = value;
            }
            else
            {
                Queries.Params.Add(key, value);
            }

            return this;
        }

        /// <summary>
        /// Gets the distinct records of the given field from the data source.
        /// </summary>
        /// <param name="fieldNames">List of field names.</param>
        /// <returns>Query.</returns>
        /// <remarks>Applicable only for ODataV4Adaptor.</remarks>
        public Query Distinct(List<string> fieldNames)
        {
            Queries.Distinct = Queries.Distinct ?? new List<string>();
            fieldNames?.ForEach((distinct) => Queries.Distinct.Add(distinct));
            return this;
        }

        /// <summary>
        /// Sets the relational tables to be eager loaded.
        /// </summary>
        /// <param name="fieldNames">List of relational table names.</param>
        /// <returns>Query.</returns>
        /// <remarks>Given table names should be eager loaded. Lazy loading is not supported.</remarks>
        public Query Expand(List<string> fieldNames)
        {
            Queries.Expand = Queries.Expand ?? new List<string>();
            fieldNames?.ForEach((expand) => Queries.Expand.Add(expand));
            return this;
        }

        /// <summary>
        /// Performs deep cloning of the given Query.
        /// </summary>
        /// <returns>Query.</returns>
        public Query Clone()
        {
            Query clone = new Query();
            clone.Queries = JsonConvert.DeserializeObject<DataManagerRequest>(JsonConvert.SerializeObject(Queries));
            clone.IsCountRequired = IsCountRequired;
            return clone;
        }

        /// <summary>
        /// Compares given Query instance by value.
        /// </summary>
        /// <param name="source">Source Query instance.</param>
        /// <param name="destination">Destination Query instance.</param>
        /// <returns></returns>
        public static bool IsEqual(Query source, Query destination)
            => JsonConvert.SerializeObject(source?.Queries).Equals(JsonConvert.SerializeObject(destination?.Queries), StringComparison.Ordinal);
    }

    /// <summary>
    /// Converts Query class to and from string respectively.
    /// </summary>
    /// <exclude/>
    public class QueryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Query);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Query query = (Query)value;
            DataManagerRequest request = query?.Queries;
            StringBuilder sb = new StringBuilder();
            sb.Append("new sf.data.Query()");
            if (request.Table != null)
            {
                sb.Append('.').Append($"from(\"{request.Table}\")");
            }

            if (request.Aggregates != null)
            {
                request.Aggregates.ForEach((agg) => sb.Append('.').Append($"aggregate(\"{agg.Field}\",\"{agg.Type}\")"));
            }

            if (request.Expand != null)
            {
                string[] expand = request.Expand.ToArray<string>();
                sb.Append('.').Append($"expand([\"{string.Join("\",\"", expand)}\"])");
            }

            if (request.Group != null)
            {
                request.Group.ForEach((grp) => sb.Append('.').Append($"group(\"{grp}\")"));
            }

            if (request.Params != null)
            {
                foreach (var item in request.Params)
                {
                    sb.Append('.').Append($"addParams(\"{item.Key}\", \"{item.Value}\")");
                }
            }

            if (request.RequiresCounts)
            {
                sb.Append('.').Append($"requiresCount()");
            }

            if (request.Search != null)
            {
                request.Search.ForEach((search) =>
                sb.Append('.').Append($"search(\"{search.Key}\", [\"{string.Join("\",\"", search.Fields)}\"], \"{search.Operator}\")"));
            }

            if (request.Select != null)
            {
                sb.Append('.').Append($"select([\"{string.Join("\",\"", request.Select.ToArray())}\"])");
            }

            if (request.Skip != 0)
            {
                sb.Append('.').Append($"skip({request.Skip})");
            }

            if (request.Sorted != null)
            {
                request.Sorted.ForEach((sort) => sb.Append('.').Append($"sortBy(\"{sort.Name}\", \"{sort.Direction}\")"));
            }

            if (request.Take != 0)
            {
                sb.Append('.').Append($"take({request.Take})");
            }

            if (request.Where != null)
            {
                request.Where.ForEach((filter) => sb.Append('.').Append($"where(sf.data.Predicate.fromJson({JsonConvert.SerializeObject(filter)}))"));
            }

            string tmp = JsonConvert.SerializeObject(sb.ToString(), Formatting.Indented);

            writer?.WriteRawValue(tmp);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new Query();
        }
    }
}

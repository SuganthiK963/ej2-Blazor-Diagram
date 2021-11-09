using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Reflection;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Handles request and response to and from OData service.
    /// </summary>
    public class ODataAdaptor : AdaptorBase
    {
        public RemoteOptions Options { get; set; }

        public override string GetName() => nameof(ODataAdaptor);

        public override bool IsRemote() => true;

        internal PvtOptions Pvt = new PvtOptions();

        internal string RootUrl { get; set; }

        public string ResourceTableName { get; set; }

        internal Type ModelType { get; set; }

        public ODataAdaptor(DataManager dataManager)
            : base(dataManager)
        {
            Options = new RemoteOptions()
            {
                Accept = "application/json;odata=light;q=1,application/json;odata=verbose;q=0.5",
                MultipartAccept = "multipart/mixed",
                SortBy = "$orderby",
                Select = "$select",
                Skip = "$skip",
                Take = "$top",
                Count = "$inlinecount",
                Where = "$filter",
                Group = "group",
                Aggregates = "aggregates",
                Expand = "$expand",
                Batch = "$batch",
                ChangeSet = "--changeset_",
                BatchPre = "batch_",
                ContentId = "Content-Id: ",
                BatchContent = "Content-Type: multipart/mixed; boundary=",
                ChangeSetContent = "Content-Type: application/http\nContent-Transfer-Encoding: binary ",
                BatchChangeSetContentType = "Content-Type: application/json; charset=utf-8 ",
                UpdateType = HttpMethod.Put
            };
        }

        public override void SetModelType(Type type)
        {
            ModelType = type;
        }
 
        public override void BeforeSend(HttpRequestMessage request)
        {
            request?.Headers.Add("Accept", Options.Accept);
            request?.Headers.Add("DataServiceVersion", "2.0");
            request?.Headers.Add("MaxDataServiceVersion", "2.0");
        }

        private void ProcessPartialQuery(DataManagerRequest queries, IDictionary<string, object> request)
        {
            if (queries == null) { throw new ArgumentNullException(nameof(queries)); }
            if (queries.RequiresCounts)
            {
                request[Options.Count] = OnCount(queries.RequiresCounts);
            }

            if (queries.Sorted?.Count > 0)
            {
                request[Options.SortBy] = OnEachSort(queries);
            }

            if (queries.Search == null && Pvt.Search != null)
            {
                Pvt.Search = null;
            }
        }

        private void ProcessSearchQuery(DataManagerRequest queries, IDictionary<string, object> request)
        {
            if (queries == null) { throw new ArgumentNullException(nameof(queries)); }
            if (queries.Search?.Count > 0)
            {
                if (Options.EnableODataSearchFallback)
                {
                    OnSearchFallback(queries, request);
                }
                else
                {
                    foreach (SearchFilter filter in queries.Search)
                    {
                        OnEachSearch(filter);
                        OnSearch(Pvt.Search as List<WhereFilter>);
                    }

                    if (Options.Search != null)
                    {
                        request[Options.Search] = Pvt.Search;
                    }
                }
            }
        }

        private void ProcessFilterQuery(DataManagerRequest queries, IDictionary<string, object> request)
        {
            if (queries == null) { throw new ArgumentNullException(nameof(queries)); }
            if (queries.Where?.Count > 0 || (Options.Search == null && Pvt.Search != null))
            {
                var filters = queries.Where ?? new List<WhereFilter>();
                if (Options.Search == null && Pvt.Search != null)
                {
                    filters.Add(Pvt.Search as WhereFilter);
                }

                string[] filArr = new string[filters.Count];
                int i = 0;
                foreach (WhereFilter filter in filters)
                {
                    filArr[i] = OnEachWhere(filter, queries);
                    i++;
                }

                request[Options.Where] = OnWhere(filArr);
            }
        }

        public override object ProcessQuery(DataManagerRequest queries)
        {
            if (queries == null) { throw new ArgumentNullException(nameof(queries)); }
            IDictionary<string, object> req = new Dictionary<string, object>();

            ProcessPartialQuery(queries, req);

            ProcessSearchQuery(queries, req);

            ProcessFilterQuery(queries, req);

            if (Options.Apply != null && queries.Distinct?.Count > 0)
            {
                req[Options.Apply] = OnDistinct(queries.Distinct);
            }

            if (Options.Expand != null && queries.Distinct == null)
            {
                if (queries.Expand?.Count > 0 && queries.Select?.Count > 0)
                {
                    string expandQuery = OnExpand(queries.Select, queries.Expand);
                    if (!string.IsNullOrEmpty(expandQuery))
                    {
                        req[Options.Expand] = expandQuery;
                    }
                }
                else if (queries.Expand?.Count > 0)
                {
                    req[Options.Expand] = OnExpand(queries.Expand);
                }
            }

            if (queries.Select?.Count > 0 && queries.Distinct == null)
            {
                req[Options.Select] = OnSelect(queries.Select);
            }

            if (queries.Group?.Count > 0)
            {
                Pvt.Groups = queries.Group;
            }

            if (queries.Aggregates?.Count > 0)
            {
                Pvt.Aggregates = queries.Aggregates;
            }

            if (queries.Take != 0)
            {
                req[Options.Skip] = queries.Skip;
                req[Options.Take] = queries.Take;
            }

            string temp = ConvertToQueryString(req);
            temp = DataUtil.GetUrl(DataManager.Url, queries.Table, temp);
            RequestOptions option = new RequestOptions() { Url = temp, RequestMethod = HttpMethod.Get, PvtData = Pvt, BaseUrl = DataManager.BaseUri };
            if (queries.Params != null)
            {
                AddParams(option, queries);
            }

            return option;
        }

        public async override Task<object> PerformDataOperation<T>(object queries)
        {
            HttpRequestMessage request = (HttpRequestMessage)queries;
            HttpResponseMessage data = await DataManager.HttpHandler.SendRequest(request);
            return data;
        }

        public override async Task<object> ProcessResponse<T>(object data, DataManagerRequest queries)
        {
            if (data == null || queries == null) { return data; }
            HttpResponseMessage response = (HttpResponseMessage)SfBaseUtils.ChangeType(data, typeof(HttpResponseMessage));
            string responseContent = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            DataResult args = new DataResult();
            OData<T> responseData = null;
            if (queries.RequiresCounts)
            {
                responseData = JsonConvert.DeserializeObject<OData<T>>(responseContent, settings);
            }
            else
            {
                responseData = new OData<T>();
                responseData.Value = JsonConvert.DeserializeObject<ODataNoCount<T>>(responseContent, settings).D;
            }

            if (responseData.Metadata != null)
            {
                string[] dataUrl = responseData.Metadata.Split(new string[] { "/$metadata#" }, StringSplitOptions.None);
                RootUrl = dataUrl[0];
                ResourceTableName = dataUrl[1];
            }

            if (responseData.D != null)
            {
                args.Result = responseData.D.Results.Cast<object>().ToList();
                if (responseData.D.count != 0 || responseData.D._count != 0)
                {
                    args.Count = responseData.count != 0 ? responseData.count : responseData.D._count;
                }
            }
            else
            {
                args.Result = responseData.Value.Cast<object>().ToList();
            }

            if (queries.Aggregates?.Count > 0)
            {
                args.Aggregates = DataUtil.PerformAggregation(args.Result, queries.Aggregates);
            }

            if (queries.Group?.Count > 0 && queries.ServerSideGroup)
            {
                var groupeData = (IEnumerable)args.Result;
                foreach (var group in queries.Group)
                {
                    groupeData = DataUtil.Group<T>(groupeData, group, queries.Aggregates, 0, null);
                }

                args.Result = groupeData;
            }

            if (responseData.count != 0 || responseData.Count != 0)
            {
                args.Count = responseData.count != 0 ? responseData.count : responseData.Count;
            }

            return queries.RequiresCounts ? args : (object)args.Result;
        }

        public override object Insert(DataManager dataManager, object data, string tableName = null, Query query = null, int position = 0)
        {
            var url = DataUtil.GetUrl(DataManager.InsertUrl ?? DataManager.CrudUrl ?? DataManager.Url, tableName);
            RequestOptions option = new RequestOptions() { Url = url, RequestMethod = HttpMethod.Post, BaseUrl = DataManager.BaseUri, Data = data, Queries = query };
            if (query?.Queries.Params != null)
            {
                AddParams(option, query.Queries);
            }

            return option;
        }

        public override object Update(DataManager dataManager, string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null)
        {
            if (Options.UpdateType.Method == "PATCH" && original != null)
            {
                data = data == null ? data : DataUtil.CompareAndRemove(data, original, keyField);
            }

            string url = DataUtil.GetUrl(DataManager.UpdateUrl ?? DataManager.CrudUrl ?? DataManager.Url, tableName);
            string urlKey = DataUtil.GetODataUrlKey(data, keyField);
            var method = Options.UpdateType;
            url = url.EndsWith("/", StringComparison.Ordinal) ? url.Substring(0, url.Length - 1) + urlKey : url + urlKey;
            RequestOptions option = new RequestOptions() { Url = url, RequestMethod = method, BaseUrl = DataManager.BaseUri, Data = data, Queries = query };
            if (query?.Queries.Params != null)
            {
                AddParams(option, query.Queries);
            }

            return option;
        }

        public override object Remove(DataManager dataManager, string keyField, object value, string tableName = null, Query query = null)
        {
            var requestData = new CRUDModel<object>()
            {
                Key = value,
                KeyColumn = keyField,
                Action = "remove"
            };
            string url = DataUtil.GetUrl(DataManager.RemoveUrl ?? DataManager.CrudUrl ?? DataManager.Url, tableName);
            string urlKey = DataUtil.GetODataUrlKey(null, keyField, value);
            url = url.EndsWith("/", StringComparison.Ordinal) ? url.Substring(0, url.Length - 1) + urlKey : url + urlKey;
            RequestOptions option = new RequestOptions() { Url = url, RequestMethod = HttpMethod.Delete, BaseUrl = DataManager.BaseUri, Data = requestData, Queries = query };
            if (query?.Queries.Params != null)
            {
                AddParams(option, query.Queries);
            }

            return option;
        }

        public override object BatchUpdate(DataManager dataManager, object changed, object added, object deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null)
        {
            string initialGuid = Options.BatchPre + Guid.NewGuid();
            string url = RootUrl != null ? DataUtil.GetUrl(RootUrl, Options.Batch) :
                DataUtil.GetUrl(DataManager.BatchUrl ?? DataManager.CrudUrl ?? DataManager.Url, Options.Batch);
            RemoteArgs args = new RemoteArgs()
            {
                Url = e?.Url,
                Key = e?.Key,
                Cid = 1,
                CSet = Options.ChangeSet.Replace("--", "", StringComparison.Ordinal) + Guid.NewGuid()
            };
            var data = new CRUDModel<object>()
            {
                Added = (added as IEnumerable).Cast<object>().ToList(),
                Changed = (changed as IEnumerable).Cast<object>().ToList(),
                Deleted = (deleted as IEnumerable).Cast<object>().ToList(),
                Table = e?.Url ?? ResourceTableName,
            };
            string dataUrl = DataManager.BatchUrl ?? DataManager.CrudUrl ?? DataManager.Url;
            if (!dataUrl.StartsWith("http", StringComparison.Ordinal))
            {
                dataUrl = DataUtil.GetUrl(DataManager.BaseUri, DataUtil.GetUrl(dataUrl, e?.Url ?? string.Empty));
            }
            else if(e?.Url != null)
            {
                dataUrl = DataUtil.GetUrl(dataUrl, e.Url);
            }

            RequestOptions option = new RequestOptions() { RequestMethod = HttpMethod.Post, Url = url, Data = data, ContentType = initialGuid, CSet = args.CSet, BaseUrl = dataUrl, keyField = keyField, Original = original, Accept = Options.Accept, UpdateType = Options.UpdateType };
            if (query?.Queries.Params != null)
            {
                AddParams(option, query.Queries);
            }

            return option;
        }

        public override async Task<object> ProcessCrudResponse<T>(object data, DataManagerRequest queries)
        {
            if (data == null || queries == null) { return data; }
            HttpResponseMessage response = (HttpResponseMessage)SfBaseUtils.ChangeType(data, typeof(HttpResponseMessage));
            string responseContent = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            CrudResponse<T> result = JsonConvert.DeserializeObject<CrudResponse<T>>(responseContent, settings);
            return result != null ? (result.D ?? result.Results) : (object)result;
        }

        public async override Task<object> ProcessBatchResponse<T>(object data, DataManagerRequest queries)
        {
            if (data == null || queries == null) { return data; }
            HttpResponseMessage response = (HttpResponseMessage)SfBaseUtils.ChangeType(data, typeof(HttpResponseMessage));
            var stringContent = await response.Content.ReadAsStringAsync();
            var guid = response.Content.Headers.ContentType.ToString();
            guid = guid.Substring(guid.IndexOf("=batchresponse", StringComparison.Ordinal) + 1);
            var contentData = stringContent.Split(new string[] { guid }, StringSplitOptions.None);
            if (contentData.Length < 2)
            {
                return new { };
            }

            var addedStr = contentData[1];
            var reg = Regex.Match(addedStr, @"(?:\bContent-Type.+boundary=)(changesetresponse.+)");
            if (reg.Captures.Count > 0)
            {
                addedStr = addedStr.Replace(reg.Captures[0].ToString(), "", StringComparison.Ordinal);
                var changeGuid = reg.Groups[1].ToString();
                var addedStrArr = addedStr.Split(new string[] { changeGuid }, StringSplitOptions.None);
                List<T> added = new List<T>();
                for (var i = addedStrArr.Length - 1; i > -1; i--)
                {
                    if (!Regex.IsMatch(addedStrArr[i], @"\{[\s\S]+\}") && (!Regex.IsMatch(addedStrArr[i], @"\bContent-ID:") ||
                        !Regex.IsMatch(addedStrArr[i], @"\bHTTP.+201")))
                    {
                        continue;
                    }

                    string responseContent = Regex.Match(addedStrArr[i], @"\{[\s\S]+\}").ToString();
                    var settings = new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    };
                    added.Add(JsonConvert.DeserializeObject<T>(responseContent, settings));
                }

                return new CRUDModel<T>() { Added = added };
            }
            else
            {
                return new CRUDModel<T>() { };
            }
        }

        public override void AddParams(RequestOptions options, DataManagerRequest queries)
        {
            if (options != null && queries != null)
            {
                options.Url = DataUtil.GetUrl(options.Url, "", DataUtil.ToQueryParams(queries.Params));
            }
        }
        internal static string ConvertToQueryString(IDictionary<string, object> req)
        {
            string[] res = new string[req.Count];
            int i = 0;
            foreach (KeyValuePair<string, object> entry in req)
            {
                res[i] = $"{entry.Key}={entry.Value}";
                i = i + 1;
            }

            return string.Join("&", res);
        }

        internal string OnComplexPredicate(WhereFilter filter, DataManagerRequest query, bool requiresCast = false)
        {
            string[] returnValues = new string[filter.predicates.Count]; _ = requiresCast;
            for (int i = 0; i < filter.predicates.Count; i++)
            {
                if (filter.predicates[i].Field != null && filter.predicates[i].Field.Contains(".", StringComparison.Ordinal))
                {
                    filter.predicates[i].Field = filter.predicates[i].Field.Replace(".", "/", StringComparison.Ordinal);
                }

                returnValues[i] = $"({OnEachWhere(filter.predicates[i], query)})";
            }

            return string.Join($" {filter.Condition} ", returnValues);
        }

        internal string OnEachWhere(WhereFilter filter, DataManagerRequest query, bool requiresCast = false)
        {
            return filter.IsComplex ? OnComplexPredicate(filter, query, requiresCast) : OnPredicate(filter, query, requiresCast);
        }

        /// <summary>
        /// Generates request filter query string from the Query value.
        /// </summary>
        /// <param name="filter">Filter criteria.</param>
        /// <param name="query">Query value.</param>
        /// <param name="requiresCast">Performs value cast. Applicable on search operation.</param>
        /// <returns></returns>
        public virtual string OnPredicate(WhereFilter filter, DataManagerRequest query, bool requiresCast = false)
        {
            if (filter == null) { throw new ArgumentNullException(nameof(filter)); }
            string returnValue = "";
            string @operator = "";
            string guid = string.Empty;
            object val = filter.value;
            Type type = filter.value?.GetType();
            string field = filter.Field;

            if (type == typeof(System.Text.Json.JsonElement))
            {
                val = OnTypeCheck(val?.ToString(), field, ModelType);
                type = val?.GetType();
            }

            // Handle if val is a date
            if (type == typeof(DateTime))
            {
                val = ((DateTime)val).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                val = $"datetime\\ {val} \\";

                // TODO
                // Two kind of time zones available. Need to add Options.LocalTime?
            }
            else if (type == typeof(DateTimeOffset))
            {
                val = ((DateTimeOffset)val).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                val = $"datetime\\ {val} \\";
            }

            if (val != null && val.GetType().GetTypeInfo().IsEnum)
            {
                val = $"'{val}'";
            }

            if (type == typeof(string))
            {
                var newVal = val;
                val = Uri.EscapeDataString((string)val);
                val = $"'{val}'";
                if (requiresCast)
                {
                    field = $"cast({field}, 'Edm.string')";
                }

                if (Guid.TryParse((string)newVal, out var guidVal))
                {
                    guid = "guid";
                }

                if (filter.IgnoreCase)
                {
                    field = $"tolower({field})";
                    val = (val as string).ToLowerInvariant();
                }
            }

            if (type == typeof(bool))
            {
                val = val.ToString().ToLowerInvariant();
            }

            var isIntField = DataUtil.odBiOperator.ContainsKey(filter.Operator);
            if (isIntField == true)
            {
                @operator = DataUtil.odBiOperator[filter.Operator];
                returnValue += field;
                returnValue += @operator;
                if (val == null)
                {
                    return $"{returnValue}null";
                }

                return returnValue + val;
            }

            if (GetName() == "ODataV4Adaptor")
            {
                @operator = DataUtil.Odv4UniOperator[filter.Operator];
            }
            else
            {
                @operator = DataUtil.odUniOperator[filter.Operator];
            }

            if (@operator == "substringof")
            {
                string temp = (string)val;
                val = field;
                field = temp;
            }

            returnValue += @operator + "(";
            returnValue += field + ",";
            if (!string.IsNullOrEmpty(guid))
            {
                returnValue += guid;
            }

            if (val == null)
            {
                returnValue += "null)";
            }
            else
            {
                returnValue += val + ")";
            }

            return returnValue;
        }

        public static object OnTypeCheck(string value, string field, Type ModelType)
        {
            Type type = null; 
            if (field != null && ModelType != null)
            {
                type = GetPropertyType(field, ModelType);
            }
            if (type == typeof(string))
            {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            else if (int.TryParse(value, out int t))
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            else if (uint.TryParse(value, out uint ut))
            {
                return Convert.ToUInt32(value, CultureInfo.InvariantCulture);
            }
            else if (double.TryParse(value, out double d))
            {
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            else if (float.TryParse(value, out float f))
            {
                return Convert.ToSingle(value, CultureInfo.InvariantCulture);
            }
            else if (decimal.TryParse(value, out decimal dec))
            {
               return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
            else if (long.TryParse(value, out long l))
            {
               return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
            }
            else if (ulong.TryParse(value, out ulong u))
            {
               return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
            }
            else if (short.TryParse(value, out short s))
            {
               return Convert.ToInt16(value, CultureInfo.InvariantCulture);
            }
            else if (ushort.TryParse(value, out ushort us))
            {
               return Convert.ToUInt16(value, CultureInfo.InvariantCulture);
            }
            else if (Guid.TryParse(value, out Guid g))
            {
                return Guid.Parse(value);
            }
            else if (bool.TryParse(value, out bool b))
            {
               return Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }
            else if (DateTime.TryParse(value, out DateTime dt) || DateTimeOffset.TryParse(value, out DateTimeOffset dto))
            {
                return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }

            return value;
        }

        public override Type GetModelType()
        {
            return ModelType;
        }

        internal static Type GetPropertyType(string field, Type ModelType)
        {
            Type type = null;
            if (field.Contains(".", StringComparison.Ordinal))
            {
                var fields = field.Split(".");
                var count = fields.Length;
                for (var j = 0; j < count; j++)
                {
                    if (j == 0)
                    {
                        type = ModelType.GetProperty(fields[j]).PropertyType;
                    }
                    else
                    {
                        type = type.GetProperty(fields[j])?.PropertyType;
                    }
                }
            }
            else
            {
                type = ModelType.GetProperty(field)?.PropertyType;
            }
            return type;
        }

        /// <summary>
        /// Returns sort query string based on query.
        /// </summary>
        /// <param name="queries">Query value.</param>
        /// <returns>string.</returns>
        public static string OnEachSort(DataManagerRequest queries)
        {
            string sorted = string.Empty;
            string[] sorArr = new string[queries?.Sorted.Count ?? 0];
            for (int i = 0; i < queries.Sorted.Count; i++)
            {
                if (queries.Sorted[i].Name != null && queries.Sorted[i].Name.Contains(".", StringComparison.Ordinal))
                {
                    queries.Sorted[i].Name = queries.Sorted[i].Name.Replace(".", "/", StringComparison.Ordinal);
                }

                sorArr[i] = queries.Sorted[i].Name + (queries.Sorted[i].Direction?.ToLowerInvariant() == "descending" ? " desc" : "");
            }

            Array.Reverse(sorArr);
            sorted = string.Join(",", sorArr);
            return sorted;
        }

        /// <summary>
        /// Returns search query string based on query.
        /// </summary>
        /// <param name="e">Search query.</param>
        public virtual void OnEachSearch(SearchFilter e)
        {
            List<WhereFilter> filters = new List<WhereFilter>();

            foreach(string field in e?.Fields)
            {
                filters.Add(new WhereFilter() { Field = field, Operator = e.Operator, value = e.Key, IgnoreCase = e.IgnoreCase });
            }

            Pvt.Search = filters;
        }

        /// <summary>
        /// Returns search query string based on query.
        /// </summary>
        /// <param name="e">List of filter queries.</param>
        public virtual void OnSearch(List<WhereFilter> e)
        {
            Pvt.Search = WhereFilter.Or(e);
        }

        /// <summary>
        /// Returns search query string based on query.
        /// </summary>
        /// <param name="e">List of filter queries.</param>
        /// <param name="additionParams">Additional parameters.</param>
        /// <returns>string.</returns>
        public virtual string OnSearch(List<WhereFilter> e, object additionParams = null)
        {
            return string.Empty;
        }

        private static bool ParseSearchValueLong(string field, Type type, SearchFilter search, List<WhereFilter> predicate)
        {
            object searchValue = search.Key; bool added = false;
            if ((type == typeof(int) || (type == typeof(int?))) && int.TryParse(search.Key.ToString(), out int t))
            {
                added = true;
                searchValue = Convert.ToInt32(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(uint) || (type == typeof(uint?))) && uint.TryParse(search.Key.ToString(), out uint ut))
            {
                added = true;
                searchValue = Convert.ToUInt32(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(double) || type == typeof(double?)) && double.TryParse(search.Key.ToString(), out double d))
            {
                added = true;
                searchValue = Convert.ToDouble(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(float) || type == typeof(float?)) && float.TryParse(search.Key.ToString(), out float f))
            {
                added = true;
                searchValue = Convert.ToSingle(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(decimal) || type == typeof(decimal?)) && decimal.TryParse(search.Key.ToString(), out decimal dec))
            {
                added = true;
                searchValue = Convert.ToDecimal(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(long) || type == typeof(long?)) && long.TryParse(search.Key.ToString(), out long l))
            {
                added = true;
                searchValue = Convert.ToUInt64(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(ulong) || type == typeof(ulong?)) && ulong.TryParse(search.Key.ToString(), out ulong u))
            {
                added = true;
                searchValue = Convert.ToUInt64(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            return added;
        }

        private static void ParseSearchValueShort(string field, Type type, SearchFilter search, List<WhereFilter> predicate)
        {
            object searchValue = search.Key;
            if ((type == typeof(short) || type == typeof(short?)) && short.TryParse(search.Key.ToString(), out short s))
            {
                searchValue = Convert.ToInt16(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(ushort) || type == typeof(ushort?)) && ushort.TryParse(search.Key.ToString(), out ushort us))
            {
                searchValue = Convert.ToUInt16(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(Guid) || type == typeof(Guid?)) && Guid.TryParse(search.Key.ToString(), out Guid g))
            {
                searchValue = Guid.Parse(search.Key);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
            else if ((type == typeof(bool) || type == typeof(bool?)) && bool.TryParse(search.Key.ToString(), out bool b))
            {
                searchValue = Convert.ToBoolean(search.Key, CultureInfo.InvariantCulture);
                predicate.Add(new WhereFilter() { Field = field, IgnoreCase = false, value = searchValue, Operator = "equal" });
            }
        }

        private static bool IsPrimitive(Type type)
            => type == typeof(int) || type == typeof(int?) || type == typeof(double) || type == typeof(double?) || type == typeof(float) || type == typeof(float?) || type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?) || type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?) || type == typeof(uint) || type == typeof(uint?) || type == typeof(decimal) || type == typeof(decimal?) || type == typeof(Guid?) || type == typeof(Guid) || type == typeof(bool?) || type == typeof(bool);

        internal void OnSearchFallback(DataManagerRequest queries, IDictionary<string, object> request)
        {
            string[] filArr = new string[queries.Search.Count];
            int i = 0;
            var predicate = new List<WhereFilter>();
            var predicateList = new List<WhereFilter>();
            WhereFilter filter = new WhereFilter();
            foreach (SearchFilter search in queries.Search)
            {
                foreach (String field in search.Fields)
                {
                    var type = field.Contains(".", StringComparison.Ordinal) ? ModelType.GetProperty(field.ToString().Split(".")[0])?.PropertyType.GetProperty(field.ToString().Split(".")[1])?.PropertyType : ModelType.GetProperty(field.ToString())?.PropertyType;
                    if (type == typeof(string) && !Guid.TryParse(search.Key.ToString(), out Guid gs))
                    {
                        predicate.Add(new WhereFilter() { Field = field, IgnoreCase = true, value = search.Key, Operator = search.Operator });
                    }
                    else if (IsPrimitive(type))
                    {
                        object searchValue = search.Key;
                        if (!ParseSearchValueLong(field, type, search, predicate))
                        {
                            ParseSearchValueShort(field, type, search, predicate);
                        }
                    }
                    else if (type.GetTypeInfo().IsEnum && Enum.TryParse(ModelType.GetProperty(field).PropertyType, search.Key, out var newenum))
                    {
                        predicate.Add(new WhereFilter() { Field = field, IgnoreCase = true, value = Enum.Parse(ModelType.GetProperty(field).PropertyType, search.Key), Operator = "equal" });
                    }
                }

                filter = new WhereFilter() { Condition = "or", IsComplex = true, predicates = predicate };
                filArr[i] = OnEachWhere(filter, queries, true);
                i++;
            }

            if (queries.Where?.Count > 0)
            {
                queries.Where?.Add(filter);
                predicate = queries.Where;
                predicateList.Add(new WhereFilter() { Condition = "and", IsComplex = true, predicates = predicate });
                queries.Where = predicateList;
            }
            else
            {
                request[Options.Where] = OnWhere(filArr);
            }
        }

        internal static string OnWhere(string[] filters)
        {
            return string.Join(" and ", filters);
        }

        internal static string onSortBy(string[] e)
        {
            _ = e;
            return string.Empty;
        }

        /// <summary>
        /// Returns count query string based on query.
        /// </summary>
        /// <param name="e">Request count value if it is true.</param>
        /// <returns>string.</returns>
        public virtual string OnCount(bool e)
        {
            return e ? "allpages" : string.Empty;
        }

        /// <summary>
        /// Returns expand query string based on query.
        /// </summary>
        /// <param name="expands">List of relational table names.</param>
        /// <returns>string.</returns>
        public static string OnExpand(List<string> expands)
        {
            return string.Join(",", expands);
        }

        /// <summary>
        /// Returns expand query string based on query.
        /// </summary>
        /// <param name="selects">List of fields to select in relational tables.</param>
        /// <param name="expands">List of relational table names.</param>
        /// <returns>string.</returns>
        public virtual string OnExpand(List<string> selects, List<string> expands)
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns select query string based on query.
        /// </summary>
        /// <param name="selects">List of field names to select.</param>
        /// <returns>string.</returns>
        public virtual string OnSelect(List<string> selects)
        {
            return string.Join(",", selects);
        }

        /// <summary>
        /// Returns distinct query string based on query.
        /// </summary>
        /// <param name="distincts">List of field names.</param>
        /// <returns>string.</returns>
        /// <remarks>Applicable only for ODataV4 services.</remarks>
        public virtual string OnDistinct(List<string> distincts)
        {
            return string.Empty;
        }

        /// <summary>
        /// Check if given value is a valid date or not.
        /// </summary>
        /// <param name="date">Input date string.</param>
        /// <returns>bool.</returns>
        protected static bool CheckDate(String date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt);
        }
    }

    /// <summary>
    /// Hold private options.
    /// </summary>
    internal class PvtOptions
    {
        internal object Groups { get; set; }

        internal List<Aggregate> Aggregates { get; set; }

        internal object Search { get; set; }

        internal int ChangeSet { get; set; }

        internal List<object> Searches { get; set; }

        internal int position { get; set; }
    }

    /// <summary>
    /// Schema for OData service response.
    /// </summary>
    /// <typeparam name="T">Type of the model.</typeparam>
    internal class OData<T>
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }

        public List<T> Value { get; set; }

        public OData<T> D { get; set; }

        public List<T> Results { get; set; }

        [JsonProperty("__count")]
        public int _count { get; set; }

        public int Count { get; set; }

        [JsonProperty("odata.count")]
        public int count { get; set; }

        public List<T> Items { get; set; }

        public T CrudData { get; set; }

        public IDictionary<string, object> Aggregates { get; set; }
    }

    /// <summary>
    /// Schema for OData service response with no count.
    /// </summary>
    /// <typeparam name="T">Type of the model.</typeparam>
    internal class ODataNoCount<T>
    {
        public List<T> D { get; set; }
    }

    /// <summary>
    /// Remote arguments.
    /// </summary>
    internal class RemoteArgs
    {
        internal string Guid { get; set; }

        internal string Url { get; set; }

        internal string Key { get; set; }

        internal int Cid { get; set; }

        internal string CSet { get; set; }
    }

    /// <summary>
    /// Schema for CRUD OData service response.
    /// </summary>
    /// <typeparam name="T">Type of the model.</typeparam>
    internal class CrudResponse<T>
    {
        [JsonProperty("d")]
        public T D { get; set; }

        public T Results { get; set; }
    }
}

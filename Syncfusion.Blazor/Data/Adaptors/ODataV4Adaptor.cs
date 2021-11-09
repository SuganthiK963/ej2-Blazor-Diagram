using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Handles request and response to and from ODataV4 service.
    /// </summary>
    public class ODataV4Adaptor : ODataAdaptor
    {
        public override string GetName() => nameof(ODataV4Adaptor);

        public override bool IsRemote() => true;

        public ODataV4Adaptor(DataManager dataManager)
            : base(dataManager)
        {
            Options = new RemoteOptions()
            {
                RequestType = "get",
                Accept = "application/json, text/javascript, */*; q=0.01",
                MultipartAccept = "multipart/mixed",
                SortBy = "$orderby",
                Select = "$select",
                Skip = "$skip",
                Take = "$top",
                Count = "$count",
                Where = "$filter",
                Group = "group",
                Aggregates = "aggregates",
                Search = "$search",
                Expand = "$expand",
                Batch = "$batch",
                ChangeSet = "--changeset_",
                BatchPre = "batch_",
                ContentId = "Content-Id: ",
                BatchContent = "Content-Type: multipart/mixed; boundary=",
                ChangeSetContent = "Content-Type: application/http\nContent-Transfer-Encoding: binary ",
                BatchChangeSetContentType = "Content-Type: application/json; charset=utf-8 ",
                UpdateType = new HttpMethod("PATCH"),
                LocalTime = false,
                Apply = "$apply"
            };
        }

        public override async Task<object> ProcessResponse<T>(object data, DataManagerRequest queries)
        {
            HttpResponseMessage response = (HttpResponseMessage)SfBaseUtils.ChangeType(data, typeof(HttpResponseMessage));
            string responseContent = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            DataResult args = new DataResult();
            ODataV4<T> responseData = JsonConvert.DeserializeObject<ODataV4<T>>(responseContent, settings);
            if (responseData?.Context != null)
            {
                string[] dataUrl = responseData.Context.Split(new string[] { "/$metadata#" }, StringSplitOptions.None);
                RootUrl = dataUrl[0];
                ResourceTableName = dataUrl[1];
            }

            if (responseData?.Value != null)
            {
                args.Result = responseData.Value.Cast<object>().ToList();
            }

            if (queries != null && queries.Aggregates?.Count > 0)
            {
                args.Aggregates = DataUtil.PerformAggregation(args.Result, queries.Aggregates);
            }

            if (queries != null && queries.Group?.Count > 0 && queries.ServerSideGroup)
            {
                var groupeData = (IEnumerable)args.Result;
                foreach (var group in queries.Group)
                {
                    groupeData = DataUtil.Group<T>(groupeData, group, queries.Aggregates, 0, null);
                }

                args.Result = groupeData;
            }

            if (responseData?.Count != 0)
            {
                args.Count = responseData.Count;
            }

            return queries != null && queries.RequiresCounts ? args : (object)args.Result;
        }

        public override async Task<object> ProcessCrudResponse<T>(object data, DataManagerRequest queries)
        {
            HttpResponseMessage response = (HttpResponseMessage)SfBaseUtils.ChangeType(data, typeof(HttpResponseMessage));
            string responseContent = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            T result = JsonConvert.DeserializeObject<T>(responseContent, settings);
            return (object)result;
        }

        public override string OnCount(bool e)
        {
            return e ? "true" : string.Empty;
        }

        public override string OnPredicate(WhereFilter filter, DataManagerRequest query, bool requiresCast = false)
        {
            string returnValue = string.Empty;
            object value = filter?.value;
            bool isDate = value?.GetType() == typeof(DateTime) || value?.GetType() == typeof(DateTimeOffset);
            if (value?.GetType() == typeof(System.Text.Json.JsonElement))
            {
                value = OnTypeCheck(value?.ToString(), filter?.Field, ModelType);
                isDate = value?.GetType() == typeof(DateTime) || value?.GetType() == typeof(DateTimeOffset);
            }
            returnValue = base.OnPredicate(filter, query, requiresCast);
            if (isDate)
            {
                returnValue = returnValue.Replace("datetime\\", "", StringComparison.Ordinal)
                    .Replace("\\", "", StringComparison.Ordinal);
            }
            else if (value?.GetType() == typeof(string))
            {
                if (Guid.TryParse((string)value, out var guid))
                {
                    returnValue = returnValue.Replace("guid", "", StringComparison.Ordinal)
                        .Replace("'", "", StringComparison.Ordinal);
                }
            }

            return returnValue;
        }

        public override void OnEachSearch(SearchFilter e)
        {
            if (e != null)
            {
                var search = Pvt.Searches ?? new List<object>();
                e.Key = System.Web.HttpUtility.UrlEncode(e.Key);
                search.Add(e.Key);
                Pvt.Searches = search;
            }
        }

        public override void OnSearch(List<WhereFilter> e)
        {
            Pvt.Search = string.Join(" OR ", Pvt.Searches);
        }

        public static object OnDistinct(string[] distinctFields) // Need to add in process query of OData
        {
            _ = distinctFields;
            return new object();
        }

        public override void BeforeSend(HttpRequestMessage request)
        {
            if(request != null && (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put || request.Method == new HttpMethod("PATCH")))
            {
                request.Headers.Add("Prefer", "return=representation");
            }

            request?.Headers.Add("Accept", Options.Accept);
        }

        public override string OnExpand(List<string> selects, List<string> expands)
        {
            IDictionary<string, string> selected = new Dictionary<string, string>();
            IDictionary<string, string> expanded = new Dictionary<string, string>();
            List<string> selecteds = selects.Where(select => select.IndexOf(".", StringComparison.Ordinal) > -1).ToList();
            selecteds?.ForEach(select =>
            {
                string[] splits = select.Split('.');
                if (!selected.ContainsKey(splits[0]))
                {
                    selected.Add(splits[0], splits[1]);
                }
            });
            expands = expands ?? new List<string>();

            // Auto expand from select query
            foreach (var select in selected)
            {
                if (!expands.Contains(select.Key))
                {
                    expands.Add(select.Key);
                }
            }

            expands.ForEach(expand =>
            {
                if (selected.ContainsKey(expand))
                {
                    expanded[expand] = $"{expand}({Options.Select}={string.Join(",", selected[expand])})";
                }
                else
                {
                    expanded[expand] = expand;
                }
            });
            return string.Join(",", expanded.Values.ToArray());
        }

        public override string OnSelect(List<string> selects)
        {
            selects = selects.Where(select => !select.Contains('.', StringComparison.Ordinal)).ToList();
            return string.Join(",", selects);
        }

        public override string OnDistinct(List<string> distincts)
        {
            return $"groupby(({string.Join(",", distincts)}))";
        }
    }

    /// <summary>
    /// Schema for ODataV4 service response.
    /// </summary>
    /// <typeparam name="T">Type of the model.</typeparam>
    internal class ODataV4<T>
    {
        [JsonProperty("@odata.context")]
        internal string Context { get; set; }

        [JsonProperty("@odata.count")]
        internal int Count { get; set; }

        [JsonProperty("value")]
        internal List<T> Value { get; set; }
    }
}

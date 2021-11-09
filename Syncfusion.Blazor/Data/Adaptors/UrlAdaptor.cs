using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Handles request and response to and from any remote service.
    /// </summary>
    public class UrlAdaptor : AdaptorBase
    {
        public UrlAdaptor(DataManager dataManager)
            : base(dataManager)
        {
        }

        public override string GetName() => nameof(UrlAdaptor);

        public override bool IsRemote() => true;

        public override object ProcessQuery(DataManagerRequest queries)
        {
            return new RequestOptions() { Url = DataManager.Url, RequestMethod = HttpMethod.Post, BaseUrl = DataManager.BaseUri, Data = queries };
        }

        public override async Task<object> PerformDataOperation<T>(object queries)
        {
            HttpRequestMessage request = (HttpRequestMessage)queries;
            HttpResponseMessage data = await DataManager.HttpHandler.SendRequest(request);
            return data;
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
            if (queries != null && queries.RequiresCounts)
            {
                UrlResult<T> dr = JsonConvert.DeserializeObject<UrlResult<T>>(responseContent, settings);
                if (queries.Aggregates?.Count > 0)
                {
                    if (dr.Aggregates != null && dr.Aggregates.Count > 0)
                    {
                        args.Aggregates = dr.Aggregates;
                    }
                    else
                    {
                        args.Aggregates = DataUtil.PerformAggregation(dr.Result, queries.Aggregates);
                    }
                }

                args.Result = dr.Result.Cast<object>().ToList();
                if (queries.Group?.Count > 0 && queries.ServerSideGroup)
                {
                    var groupeData = (IEnumerable)args.Result;
                    if (queries.LazyLoad)
                    {
                        groupeData = DataUtil.Group<T>(groupeData, queries.Group[0], queries.Aggregates, 0, null, queries.LazyLoad, queries.LazyExpandAllGroup);
                        dr.Count = groupeData.Cast<object>().Count();
                    }
                    else
                    {
                        foreach (var group in queries.Group)
                        {
                            groupeData = DataUtil.Group<T>(groupeData, group, queries.Aggregates, 0, null, queries.LazyLoad, queries.LazyExpandAllGroup);
                        }
                    }

                    args.Result = groupeData;
                }

                args.Count = dr.Count;
            }
            else
            {
                List<T> dr = JsonConvert.DeserializeObject<List<T>>(responseContent, settings);
                args.Result = dr.Cast<object>().ToList();
                if (queries != null && queries.Group?.Count > 0 && queries.ServerSideGroup)
                {
                    var groupeData = (IEnumerable)dr;
                    if (queries.LazyLoad)
                    {
                        groupeData = DataUtil.Group<T>(groupeData, queries.Group[0], queries.Aggregates, 0, null, queries.LazyLoad, queries.LazyExpandAllGroup);
                        args.Result = groupeData;
                    }
                    else
                    {
                        foreach (var group in queries.Group)
                        {
                            groupeData = DataUtil.Group<T>(groupeData, group, queries.Aggregates, 0, null, queries.LazyLoad, queries.LazyExpandAllGroup);
                        }

                        args.Result = groupeData.Cast<object>().ToList();
                    }
                }
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
            return JsonConvert.DeserializeObject<T>(responseContent, settings);
        }

        public override object Insert(DataManager dataManager, object data, string tableName = null, Query query = null, int position = 0)
        {
            var requestData = new CRUDModel<object>()
            {
                Value = data,
                Action = "insert",
                Params = query?.Queries?.Params,
                Table = tableName
            };
            RequestOptions option = new RequestOptions() { Url = DataManager.InsertUrl ?? DataManager.CrudUrl ?? DataManager.Url, RequestMethod = HttpMethod.Post, BaseUrl = DataManager.BaseUri, Data = requestData, Queries = query };
            return option;
        }

        public override object Update(DataManager dataManager, string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null)
        {
            var requestData = new CRUDModel<object>()
            {
                Value = data,
                KeyColumn = keyField,
                Action = "update",
                Key = data?.GetType().GetProperty(keyField).GetValue(data),
                Params = query?.Queries?.Params,
                Table = tableName
            };
            RequestOptions option = new RequestOptions() { Url = DataManager.UpdateUrl ?? DataManager.CrudUrl ?? DataManager.Url, RequestMethod = HttpMethod.Post, BaseUrl = DataManager.BaseUri, Data = requestData, Queries = query };
            return option;
        }

        public override object Remove(DataManager dataManager, string keyField, object value, string tableName = null, Query query = null)
        {
            var requestData = new CRUDModel<object>()
            {
                Key = value,
                KeyColumn = keyField,
                Action = "remove",
                Params = query?.Queries?.Params,
                Table = tableName
            };
            RequestOptions option = new RequestOptions() { Url = DataManager.RemoveUrl ?? DataManager.CrudUrl ?? DataManager.Url, RequestMethod = HttpMethod.Post, BaseUrl = DataManager.BaseUri, Data = requestData, Queries = query };
            return option;
        }

        public override object BatchUpdate(DataManager dataManager, object changed, object added, object deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null)
        {
            var requestData = new CRUDModel<object>()
            {
                Changed = (changed as IEnumerable).Cast<object>().ToList(),
                Added = (added as IEnumerable).Cast<object>().ToList(),
                Deleted = (deleted as IEnumerable).Cast<object>().ToList(),
                Action = "Batch",
                Table = e?.Url,
                Key = e?.Key,
                Params = query?.Queries?.Params
            };
            RequestOptions option = new RequestOptions() { Url = DataManager.BatchUrl ?? DataManager.CrudUrl ?? DataManager.Url, RequestMethod = HttpMethod.Post, BaseUrl = DataManager.BaseUri, Data = requestData, Queries = query };
            return option;
        }
    }

    /// <summary>
    /// Schema for URL service response.
    /// </summary>
    /// <typeparam name="T">Type of the model.</typeparam>
    internal class UrlResult<T>
    {
        [JsonProperty("result")]
        public List<T> Result { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("aggregates")]
        public IDictionary<string, object> Aggregates { get; set; }
    }
}

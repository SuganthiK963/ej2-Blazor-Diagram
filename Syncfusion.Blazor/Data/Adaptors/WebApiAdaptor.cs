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
    /// Handles request and response to and from Web API controller.
    /// </summary>
    public class WebApiAdaptor : ODataAdaptor
    {
        public WebApiAdaptor(DataManager dataManager)
            : base(dataManager)
        {
        }

        public override string GetName() => nameof(WebApiAdaptor);

        public override bool IsRemote() => true;

        public override object Insert(DataManager dataManager, object data, string tableName = null, Query query = null, int position = 0)
        {
            var url = DataUtil.GetUrl(DataManager.InsertUrl ?? DataManager.CrudUrl ?? DataManager.Url, tableName);
            RequestOptions option = new RequestOptions() { Url = url, RequestMethod = HttpMethod.Post, Data = JsonConvert.SerializeObject(data), BaseUrl = DataManager.BaseUri };
            if (query?.Queries.Params != null)
            {
                AddParams(option, query.Queries);
            }

            return option;
        }

        public override object Remove(DataManager dataManager, string keyField, object value, string tableName = null, Query query = null)
        {
            var url = DataUtil.GetUrl(DataManager.RemoveUrl ?? DataManager.CrudUrl ?? DataManager.Url, tableName);
            url = DataUtil.GetUrl(url, value?.ToString());
            RequestOptions option = new RequestOptions() { Url = url, RequestMethod = HttpMethod.Delete, Data = JsonConvert.SerializeObject(value), BaseUrl = DataManager.BaseUri };
            if (query?.Queries.Params != null)
            {
                AddParams(option, query.Queries);
            }

            return option;
        }

        public override object Update(DataManager dataManager, string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null)
        {
            var url = DataUtil.GetUrl(DataManager.UpdateUrl ?? DataManager.CrudUrl ?? DataManager.Url, tableName);
            RequestOptions option = new RequestOptions() { Url = url, RequestMethod = HttpMethod.Put, Data = JsonConvert.SerializeObject(data), BaseUrl = DataManager.BaseUri };
            if (query?.Queries.Params != null)
            {
                AddParams(option, query.Queries);
            }

            return option;
        }

        public override void BeforeSend(HttpRequestMessage request)
        {
            request?.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
        }

        public override object ProcessQuery(DataManagerRequest queries)
        {
            RequestOptions option = new RequestOptions();
            option = (RequestOptions)base.ProcessQuery(queries);
            if (queries?.Aggregates?.Count > 0)
            {
                IDictionary<string, object> req = new Dictionary<string, object>();
                req[Options.Aggregates] = JsonConvert.SerializeObject(queries.Aggregates);
                string temp = ConvertToQueryString(req);
                option.Url = DataUtil.GetUrl(option.Url, string.Empty, temp);
            }

            return option;
        }

        public async override Task<object> ProcessResponse<T>(object data, DataManagerRequest queries)
        {
            HttpResponseMessage response = (HttpResponseMessage)SfBaseUtils.ChangeType(data, typeof(HttpResponseMessage));
            string responseContent = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            DataResult args = new DataResult();
            OData<T> responseData;

            if (queries != null && queries.RequiresCounts)
            {
                responseData = JsonConvert.DeserializeObject<OData<T>>(responseContent, settings);
            }
            else
            {
                responseData = new OData<T>()
                {
                    Items = JsonConvert.DeserializeObject<List<T>>(responseContent, settings)
                };
            }

            if (responseData.Count != 0)
            {
                args.Count = responseData.Count;
            }

            if (responseData.Items?.Count > 0)
            {
                args.Result = responseData.Items.Cast<object>().ToList();
            }

            if (queries != null && queries.Aggregates?.Count > 0)
            {
                if (responseData.Aggregates != null && responseData.Aggregates.Count > 0)
                {
                    args.Aggregates = responseData.Aggregates;
                }
                else
                {
                    args.Aggregates = DataUtil.PerformAggregation(args.Result, queries.Aggregates);
                }
            }

            if (queries != null && queries.Group?.Count > 0 && queries.ServerSideGroup)
            {
                var groupeData = (IEnumerable)args.Result;
                if (queries.LazyLoad)
                {
                    groupeData = DataUtil.Group<T>(groupeData, queries.Group[0], queries.Aggregates, 0, null, queries.LazyLoad, queries.LazyExpandAllGroup);
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
    }
}

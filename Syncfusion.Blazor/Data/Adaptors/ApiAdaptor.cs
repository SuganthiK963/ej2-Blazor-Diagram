using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Handles request and response to and from Web API created for ApiController.
    /// </summary>
    public class ApiAdaptor : UrlAdaptor
    {
        public ApiAdaptor(DataManager dataManager)
            : base(dataManager)
        {
        }

        public override string GetName() => nameof(ApiAdaptor);

        public override object ProcessQuery(DataManagerRequest queries)
        {
            RequestOptions option = new RequestOptions()
            {
                Url = DataManager.Url,
                RequestMethod = HttpMethod.Get,
                BaseUrl = DataManager.BaseUri
            };
            option.Url = DataUtil.GetUrl(option.Url, "", $"dataManager=({JsonConvert.SerializeObject(queries)})");
            if (queries?.Params != null)
            { 
                AddParams(option, queries); 
            }

            return option;
        }

        public override object Update(DataManager dataManager, string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null)
        {
            RequestOptions option = (RequestOptions)base.Update(dataManager, keyField, data, tableName, query, original, updateProperties);
            option.Url = DataUtil.GetUrl(option.Url, DataUtil.GetKeyValue(keyField, data));
            option.RequestMethod = HttpMethod.Put;
            return option;
        }

        public override object Remove(DataManager dataManager, string keyField, object value, string tableName = null, Query query = null)
        {
            RequestOptions option = (RequestOptions)base.Remove(dataManager, keyField, value, tableName, query);
            option.Url = DataUtil.GetUrl(option.Url, DataUtil.GetKeyValue(keyField, value));
            option.RequestMethod = HttpMethod.Delete;
            return option;
        }
        public override void AddParams(RequestOptions options, DataManagerRequest queries)
        {
            if (options == null) { throw new ArgumentNullException(nameof(options)); }
            options.Url = DataUtil.GetUrl(options.Url, "", DataUtil.ToQueryParams(queries?.Params));
        }
    }
}

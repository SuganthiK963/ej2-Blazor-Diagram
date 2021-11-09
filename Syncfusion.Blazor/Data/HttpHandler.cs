using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Interface for http handler used by data manager.
    /// </summary>
    /// <exclude/>
    internal interface IHttpHandler
    {
        HttpClient GetClient();

        Task<HttpResponseMessage> SendRequest(HttpRequestMessage data);
    }

    /// <summary>
    /// Bas class for http handler used by data manager.
    /// </summary>
    /// <exclude/>
    internal class HttpHandlerBase : IHttpHandler
    {
        public HttpHandlerBase(HttpClient http)
        {
            client = http;
        }

        private HttpClient client { get; set; }
        /// <summary>
        /// Returns http client.
        /// </summary>
        /// <returns>HttpClient</returns>
        public virtual HttpClient GetClient() => client ?? (client = new HttpClient());

        public virtual async Task<HttpResponseMessage> SendRequest(HttpRequestMessage data) => await Task.FromResult<HttpResponseMessage>(null);
    }

    /// <summary>
    /// Handles HttpClient instance creation. Also build and sends HttpMessages request.
    /// </summary>
    /// <exclude/>
    internal class HttpHandler : HttpHandlerBase
    {
        public HttpHandler(HttpClient client) : base(client)
        {
        }

        public HttpClient Client { get; set; }

        public override async Task<HttpResponseMessage> SendRequest(HttpRequestMessage data)
        {
            Client = GetClient();
           HttpResponseMessage returnData = null;
            try
            {
                returnData = await Client.SendAsync(data);
                if (returnData.IsSuccessStatusCode)
                {
                    return returnData;
                }
                else
                {
                    returnData.EnsureSuccessStatusCode();
                    return null;
                }
            }
            catch (Exception e)
            {
                HttpContent _content = returnData == null ? null : returnData.Content;
                string content = _content == null ? "" : await _content.ReadAsStringAsync();
                throw new HttpRequestException(e.Message, new HttpRequestException(content)); //BC!!
            }
        }

        public static HttpRequestMessage PrepareRequest(RequestOptions options)
        {
            if (!options.Url.StartsWith("http", StringComparison.Ordinal))
            {
                options.Url = DataUtil.GetUrl(options.BaseUrl, options.Url);
            }

            HttpRequestMessage req = new HttpRequestMessage() { RequestUri = new Uri(options.Url), Method = options.RequestMethod };
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            settings.NullValueHandling = NullValueHandling.Ignore;
            if (req.Method.Equals(HttpMethod.Patch))
            {
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            }

            string serializedData = options.Data == null ? string.Empty : (options.Data.GetType() == typeof(string) ? (string)options.Data : JsonConvert.SerializeObject(options.Data, Formatting.Indented, settings));
            if (req.Method != HttpMethod.Get && req.Method != HttpMethod.Head)
            {
                var stringContent = new StringContent(serializedData, Encoding.UTF8, options.ContentType);
                req.Content = stringContent;
            }

            return req;
        }

        public static HttpRequestMessage PrepareBatchRequest(RequestOptions options, Type ModelType = null)
        {
            if (!options.Url.StartsWith("http", StringComparison.Ordinal))
            {
                options.Url = DataUtil.GetUrl(options.BaseUrl, options.Url);
            }

            HttpRequestMessage req = new HttpRequestMessage() { RequestUri = new Uri(options.Url), Method = options.RequestMethod };
            MultipartContent batchContent = new MultipartContent("mixed", options.ContentType);
            CRUDModel<object> batchRecords = options.Data as CRUDModel<object>;
            int count = 0;
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            settings.NullValueHandling = NullValueHandling.Ignore;
            if (batchRecords?.Added?.Count > 0)
            {
                foreach (object data in batchRecords.Added)
                {
#pragma warning disable CA2000
                    MultipartContent changeSet = new MultipartContent("mixed", options.CSet);
#pragma warning restore CA2000
                    HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, options.BaseUrl);
                    postRequest.Content = new StringContent(JsonConvert.SerializeObject(data, Formatting.None, settings), Encoding.UTF8, "application/json");
                    postRequest.Headers.Add("Accept", options.Accept);
                    postRequest.Headers.Add("Content-Id", count.ToString(CultureInfo.InvariantCulture));
                    count += 1;

#pragma warning disable CA2000
                    HttpMessageContent postRequestContent = new HttpMessageContent(postRequest);
#pragma warning restore CA2000
                    postRequestContent.Headers.Remove("Content-Type");
                    postRequestContent.Headers.Add("Content-Type", "application/http");
                    postRequestContent.Headers.Add("Content-Transfer-Encoding", "binary");

                    changeSet.Add(postRequestContent);
                    batchContent.Add(changeSet);
                }
            }

            if (batchRecords?.Changed?.Count > 0)
            {
                for (int i = 0; i < batchRecords.Changed.Count; i++)
                {
#pragma warning disable CA2000
                    MultipartContent changeSet = new MultipartContent("mixed", options.CSet);
#pragma warning restore CA2000
                    var value = DataUtil.GetVal(batchRecords.Changed, i, options.keyField);
                    string urlKey = DataUtil.GetODataUrlKey(null, options.keyField, value, ModelType);
                    HttpRequestMessage putRequest = new HttpRequestMessage(options.UpdateType, $"{options.BaseUrl}{urlKey}");
                    var orgData = (options.Original as IEnumerable)?.Cast<object>().ToList().Where((e) =>
                    {
                        return DataUtil.GetVal(batchRecords.Changed, i, options.keyField)?.ToString() == e.GetType().GetProperty(options.keyField).GetValue(e)?.ToString();
                    }).ToList();
                    var changedData = DataUtil.CompareAndRemove(batchRecords.Changed[i], orgData?[0], options.keyField);
                    putRequest.Content = new StringContent(JsonConvert.SerializeObject(changedData, Formatting.None, settings), Encoding.UTF8, "application/json");
                    putRequest.Headers.Add("Accept", options.Accept);
                    putRequest.Headers.Add("Content-Id", count.ToString(CultureInfo.InvariantCulture));
                    count += 1;

#pragma warning disable CA2000
                    HttpMessageContent postRequestContent = new HttpMessageContent(putRequest);
#pragma warning restore CA2000
                    postRequestContent.Headers.Remove("Content-Type");
                    postRequestContent.Headers.Add("Content-Type", "application/http");
                    postRequestContent.Headers.Add("Content-Transfer-Encoding", "binary");

                    changeSet.Add(postRequestContent);
                    batchContent.Add(changeSet);
                }
            }

            if (batchRecords?.Deleted?.Count > 0)
            {
                foreach (object data in batchRecords.Deleted)
                {
#pragma warning disable CA2000
                    MultipartContent changeSet = new MultipartContent("mixed", options.CSet);
#pragma warning restore CA2000
                    string urlKey = DataUtil.GetODataUrlKey(data, options.keyField);
                    HttpRequestMessage deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{options.BaseUrl}{urlKey}");
                    deleteRequest.Content = new StringContent(JsonConvert.SerializeObject(data, Formatting.None, settings), Encoding.UTF8, "application/json");
                    deleteRequest.Headers.Add("Accept", "application/json;odata=light;q=1,application/json;odata=verbose;q=0.5");
                    deleteRequest.Headers.Add("Content-Id", count.ToString(CultureInfo.InvariantCulture));
                    count += 1;
#pragma warning disable CA2000
                    HttpMessageContent postRequestContent = new HttpMessageContent(deleteRequest);
#pragma warning restore CA2000
                    postRequestContent.Headers.Remove("Content-Type");
                    postRequestContent.Headers.Add("Content-Type", "application/http");
                    postRequestContent.Headers.Add("Content-Transfer-Encoding", "binary");

                    changeSet.Add(postRequestContent);
                    batchContent.Add(changeSet);
                }
            }

            req.Content = batchContent;
            return req;
        }
    }
}

using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Converters;
using Syncfusion.Blazor.Data;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Collections;
using System.Linq;
using System.Dynamic;
using Syncfusion.Blazor.Internal;
using System.Globalization;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// The DataManager is a data management component used for performing data operations in applications.
    /// It acts as an abstraction for using local data source - IEnumerable and remote data source - web services returning JSON or oData.
    /// </summary>
    public class DataManager : OwningComponentBase
    {
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        [Inject]
        internal HttpClient httpClient { get; set; }

        /// <exclude/>
        [JsonIgnore]
        [Inject]
        public IServiceProvider ServiceProvider { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public BaseAdaptor BaseAdaptor { get; set; }

        /// <summary>
        /// Specifies the HttpClient instance to be used  by DataManager.
        /// </summary>
        /// <remarks>Use HttpClientInstance property to inject named HttpClient into DataManager.</remarks>
        [Parameter]
        [JsonIgnore]
        public HttpClient HttpClientInstance { get; set; }

        /// <summary>
        /// Specifies the endpoint URL. DataManager requests this URL when data is needed.
        /// </summary>
        [Parameter]
        [DefaultValue("")]
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the custom adaptor class type. DataManager uses this type value to instantiate custom adaptor class.
        /// </summary>
        /// <remarks>DataManager uses Activator.CreateInstance method to create custom adaptor instance.
        /// Injecting services into custom adaptor class, is not supported while using this approach.
        /// To inject and use services, provide custom adaptor as a Blazor component by extending DataAdaptor class.</remarks>
        [Parameter]
        [JsonIgnore]
        [JsonProperty("adaptorInstance")]
        public Type AdaptorInstance { get; set; }

        /// <summary>
        /// Gets or sets the data adaptor to be used by DataManager.
        /// <list type="bullet">
        /// <item>
        /// <term>BlazorAdaptor</term>
        /// <description>Default. BlazorAdaptor is used to process Enumerable data. It contains methods to process the given collection based on the queries.</description>
        /// </item>
        /// <item>
        /// <term>ODataAdaptor</term>
        /// <description>
        /// OData Adaptor provies ability to consume and manipulate data from OData services.
        /// </description>
        /// </item>
        /// <item>
        /// <term>ODataV4Adaptor</term>
        /// <description>
        /// ODatav4 Adaptor provies ability to consume and manipulate data from OData v4 services
        /// </description>
        /// </item>
        /// <item>
        /// <term>WebApiAdaptor</term>
        /// <description>
        /// WebApi Adaptor provies ability to consume and manipulate data from WebApi services.
        /// This adaptor is targeted to interact with Web API created using OData endpoint, it is extended from ODataAdaptor
        /// </description>
        /// </item>
        /// <item>
        /// <term>UrlAdaptor</term>
        /// <description>
        /// URL Adaptor is used when you are required to interact with all kind of remote services to retrieve data.
        /// </description>
        /// </item>
        /// <item>
        /// <term>RemoteSaveAdaptor</term>
        /// <description>
        /// Remote Save Adaptor is used for binding JSON data.
        /// It interacts with remote services only for CRUD operations.
        /// </description>
        /// </item>
        /// <item>
        /// <term>CustomAdaptor</term>
        /// <description>
        /// CustomAdaptor specifies that own data query and manipulation logic has been provided using custom adaptor component
        /// extended from DataAdaptor class.
        /// </description>
        /// </item>
        /// <item>
        /// <term>JsonAdaptor</term>
        /// <description>
        /// JsonAdaptor is used to process JSON data at the client side. It contains methods to process the given JSON data based on the queries.
        /// </description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        [DefaultValue(Adaptors.BlazorAdaptor)]
        [JsonProperty("adaptor")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Adaptors Adaptor { get; set; } = Adaptors.BlazorAdaptor;

        /// <summary>
        /// Holds adaptor instance.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [JsonIgnore]
        [JsonProperty("dataAdaptor")]
        public IAdaptor DataAdaptor { get; set; }

        /// <summary>
        /// Holds dotnet instance.
        /// </summary>
        /// <exclude/>
        [JsonIgnore]
        public DotNetObjectReference<object> DotNetObjectRef { get; set; }

        /// <summary>
        /// Specifies the insert URL.
        /// </summary>
        /// <remarks>Applicable for remote adaptors.
        /// When Insert method is called, request will be made to InsertUrl.
        /// If InsertUrl is not provided, Url is used as fallback.</remarks>
        [Parameter]
        [JsonProperty("insertUrl")]
        public string InsertUrl { get; set; }

        /// <summary>
        /// Specifies the remove URL.
        /// </summary>
        /// <remarks>Applicable for remote adaptors.
        /// When Remove method is called, request will be made to RemoveUrl.
        /// If RemoveUrl is not provided, Url is used as fallback.</remarks>
        [Parameter]
        [JsonProperty("removeUrl")]
        public string RemoveUrl { get; set; }

        /// <summary>
        /// Specifies the update URL.
        /// </summary>
        /// <remarks>Applicable for remote adaptors.
        /// When Update method is called, request will be made to UpdateUrl.
        /// If UpdateUrl is not provided, Url is used as fallback.</remarks>
        [Parameter]
        [JsonProperty("updateUrl")]
        public string UpdateUrl { get; set; }

        /// <summary>
        /// Specifies the CRUD URL.
        /// </summary>
        /// <remarks>Applicable for remote adaptors.
        /// When Insert, Remove or Update method is called, request will be made to CrudUrl.
        /// If CrudUrl is not provided, Url is used as fallback.</remarks>
        [Parameter]
        [JsonProperty("crudUrl")]
        public string CrudUrl { get; set; }

        /// <summary>
        /// Specifies the batch url.
        /// </summary>
        /// <remarks>Applicable for remote adaptors.
        /// When SaveChanges method is called, request will be made to BatchUrl.
        /// If BatchUrl is not provided, Url is used as fallback.</remarks>
        [Parameter]
        [JsonProperty("batchUrl")]
        public string BatchUrl { get; set; }

        /// <summary>
        /// Specifies the IEnumerable collection. This data could be queried and manipulated.
        /// </summary>
        [Parameter]
        [JsonProperty("json")]
        [JsonConverter(typeof(DataSourceTypeConverter))]
        public IEnumerable<object> Json { get; set; }

        /// <summary>
        /// Specifies the key/value pair of headers.
        /// </summary>
        /// <remarks>
        /// Use Headers to add any custom headers to the request made by DataManager.
        /// Users can also send authentication bearer token using Headers property.
        /// </remarks>
        [Parameter]
        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Specifies the accept type.
        /// </summary>
        [Parameter]
        [JsonProperty("accept")]
        public bool Accept { get; set; }

        /// <summary>
        /// Specifies the IEnumerale data.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [JsonProperty("data")]
        public object Data { get; set; }

        /// <summary>
        /// Specifies the time limit to clear the cached data.
        /// </summary>
        [Parameter]
        [JsonProperty("timeTillExpiration")]
        public int TimeTillExpiration { get; set; }

        /// <summary>
        /// Specifies the caching page size.
        /// </summary>
        [Parameter]
        [JsonProperty("cachingPageSize")]
        public int CachingPageSize { get; set; }

        /// <summary>
        /// Enables data caching.
        /// </summary>
        [Parameter]
        [JsonProperty("enableCaching")]
        public bool EnableCaching { get; set; }

        /// <summary>
        /// Specifies the request type for sending data fetching.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [JsonProperty("requestType")]
        public string RequestType { get; set; }

        /// <summary>
        /// Specifies the primary key value.
        /// </summary>
        [Parameter]
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// When true, then indicates that the request is a cross-domain request.
        /// </summary>
        [Parameter]
        [JsonProperty("crossDomain")]
        public bool CrossDomain { get; set; }

        /// <summary>
        /// Specifies the jsonp request.
        /// </summary>
        [Parameter]
        [JsonProperty("jsonp")]
        public string Jsonp { get; set; }

        /// <summary>
        /// Specifies the data type.
        /// </summary>
        [Parameter]
        [JsonProperty("dataType")]
        public string DataType { get; set; }

        /// <summary>
        /// Enables offline mode in datamanager.
        /// </summary>
        /// <remarks>
        /// Applicable for remote data source. If offline is true then initial request will be made to fetch
        /// data. Further actions will be handled at the in-memory data and no more request will be made to the service.
        /// Cached data is stored in the JSON property.
        /// </remarks>
        [Parameter]
        [JsonProperty("offline")]
        public bool Offline { get; set; }

        /// <summary>
        /// Sepcifies requires format.
        /// </summary>
        [Parameter]
        [JsonProperty("requiresFormat")]
        public bool RequiresFormat { get; set; }

        [DefaultValue(false)]
        [JsonProperty("isDataManager")]
        internal bool IsDataManager { get; set; }

        /// <summary>
        /// unique identifier.
        /// </summary>
        /// <exclude/>
        private int guid { get; set; }

        /// <summary>
        /// unique identifier
        /// </summary>
        /// <exclude/>
        [JsonProperty("guid")]
        public int UniqueGuid
        {
            get
            {
                Random random = new Random();
                if (guid == 0)
                {
#pragma warning disable CA5394
                    guid = random.Next(1, 100000);
#pragma warning restore CA5394
                    return guid;
                }
                else
                {
                    return guid;
                }
            }
        }

        /// <summary>
        /// Parent component of DataManager.
        /// </summary>
        /// <exclude/>
        [CascadingParameter]
        protected object Parent { get; set; }

        /// <summary>
        /// Parent component of DataManager.
        /// </summary>
        /// <exclude/>
        [CascadingParameter]
        protected BaseComponent BaseParent { get; set; }

        /// <summary>
        /// Defines the child content.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets navigation manager to get base url.
        /// </summary>
        /// <exclude/>
        [Inject]
        [JsonIgnore]
        private NavigationManager UriHelper { get; set; }

        /// <summary>
        /// Gets the Base URL.
        /// </summary>
        /// <remarks>BaseUri will be used to get absolute of Url, InsertUrl, UpdateUrl and RemoveUrl properties.</remarks>
        public string BaseUri { get; set; }

        /// <summary>
        /// Specifies the http client handler.
        /// </summary>
        /// <exclude/>
        [JsonIgnore]
        internal HttpHandler HttpHandler;

        public DataManager()
        {
            InitDataManagerAdaptor();
            HttpHandler = new HttpHandler(HttpClientInstance ?? httpClient);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseUri = (HttpClientInstance ?? httpClient)?.BaseAddress?.OriginalString ?? UriHelper.BaseUri;
            InitDataManagerAdaptor();
            HttpHandler = new HttpHandler(HttpClientInstance ?? httpClient);
            IsDataManager = true;
            if (Parent != null)
            {
                Type ParentType = Parent.GetType();
                ParentType.GetProperty("DataManager")?.SetValue(Parent, this);
                PropertyInfo Field = Parent.GetType().GetProperty("jsProperty", BindingFlags.Instance | BindingFlags.NonPublic);
                string JSProperty = Field?.GetValue(Parent).ToString() ?? string.Empty;
                BaseComponent MainParent = null;

                // components extended from SfBaseComponent will not have JsProperty
                if (string.IsNullOrEmpty(JSProperty))
                {
                    if (Adaptor == Adaptors.CustomAdaptor)
                    {
                        BaseAdaptor = new BaseAdaptor(AdaptorInstance, Parent, this);
                    }

                    // Dynamic SfDataManager insertion
                    if (Parent is SfDataBoundComponent _comp)
                    {
                        if (_comp.IsRendered && !_comp.PropertyChanges.ContainsKey("DataSource"))
                        {
                            _comp.PropertyChanges.Add("DataSource", this);
                            await _comp.OnPropertyChanged();
                        }
                    }

                    return;
                }

                if (!JSProperty.Contains("sf.", StringComparison.Ordinal))
                {
                    PropertyInfo MainParentProperty = Parent.GetType().GetProperty("BaseParent", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    MainParent = (BaseComponent)MainParentProperty.GetValue(Parent);
                    MainParent.DataManagerContainer[JSProperty + ".dataSource"] = this;
                }

                if (Adaptor == Adaptors.CustomAdaptor)
                {
                    BaseAdaptor = new BaseAdaptor(AdaptorInstance, Parent, this);
                    DotNetObjectRef = DotNetObjectReference.Create<object>(BaseAdaptor);

                    if (!JSProperty.Contains("sf.", StringComparison.Ordinal))
                    {
                        MainParent.ChildDotNetObjectRef.Add(UniqueGuid.ToString(CultureInfo.InvariantCulture), DotNetObjectRef);
                    }
                    else
                    {
                        BaseParent.ChildDotNetObjectRef.Add(UniqueGuid.ToString(CultureInfo.InvariantCulture), DotNetObjectRef);
                    }

                    BaseParent.updateDictionary("dataSource_custom", this, BaseParent.BindableProperties);
                }
                object value = Adaptor == Adaptors.JsonAdaptor ? (object)this : (object)new DefaultAdaptor(JSProperty + ".dataSource", this, Adaptor);
                if (!JSProperty.Contains("sf.", StringComparison.Ordinal))
                {
                    MainParent.updateDictionary(JSProperty + ".dataSource", value, MainParent.BindableProperties);
                }
                else
                {
                    BaseParent.updateDictionary("dataSource", value, BaseParent.BindableProperties);
                }
            }
        }

        /// <summary>
        /// If returns true, Json property will be serialized.
        /// </summary>
        /// <returns>bool.</returns>
        /// <exclude/>
        public bool ShouldSerializeJson()
        {
            // don't serialize the Json property if Adaptor is not JsonAdaptor.
            return Adaptor == Adaptors.JsonAdaptor;
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class.</typeparam>
        /// <param name="query">Query class which will be executed against data source.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> ExecuteQuery<T>(Query query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            return await ExecuteQuery<T>(query.Queries);
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class.</typeparam>
        /// <param name="query">Query class which will be executed against data source.</param>
        /// <returns>Task.</returns>
        public async Task<object> ExecuteQueryAsync<T>(Query query)
        {
            return await ExecuteQuery<T>(query);
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class</typeparam>
        /// <param name="queries">Query class which will be executed against data source.</param>
        /// <returns>Task</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> ExecuteQuery<T>(DataManagerRequest queries)
        {
            if (Adaptor == Adaptors.CustomAdaptor)
            {
                return await BaseAdaptor.Read(queries);
            }

            if (DataAdaptor != null && DataAdaptor.IsRemote())
            {
                if (Offline && queries != null)
                {
                    return await ProcessOffline<T>(queries);
                }
                DataAdaptor.SetModelType(typeof(T));
                object request = DataAdaptor.ProcessQuery(queries);
                using (var queryRequest = HttpHandler.PrepareRequest(request as RequestOptions))
                {
                    BeforeSend(queryRequest as HttpRequestMessage);
                    object dataResult = await DataAdaptor?.PerformDataOperation<T>(queryRequest);
                    object finalData = await DataAdaptor?.ProcessResponse<T>(dataResult, queries);
                    return finalData;
                }
            }
            else
            {
                DataManagerRequest request = (DataManagerRequest)DataAdaptor?.ProcessQuery(queries);
                object dataResult = await DataAdaptor?.PerformDataOperation<T>(request);
                object finalData = await DataAdaptor?.ProcessResponse<T>(dataResult, request);
                return finalData;
            }
        }

        /// <summary>
        /// Executes given query and returns resultant data.
        /// </summary>
        /// <typeparam name="T">Type of the model class</typeparam>
        /// <param name="queries">Query class which will be executed against data source.</param>
        /// <returns>Task</returns>
        public async Task<object> ExecuteQueryAsync<T>(DataManagerRequest queries)
        {
            return await ExecuteQuery<T>(queries);
        }

        internal async Task<object> ProcessOffline<T>(DataManagerRequest queries)
        {
            // Fetch remote data
            var query = new DataManagerRequest();
            query.Params = queries.Params;
            object request = DataAdaptor.ProcessQuery(query);
            using (var queryRequest = HttpHandler.PrepareRequest(request as RequestOptions))
            {
                BeforeSend(queryRequest as HttpRequestMessage);
                object dataResult = await DataAdaptor.PerformDataOperation<T>(queryRequest);
                object finalData = await DataAdaptor.ProcessResponse<T>(dataResult, query);


                //Assign to Json property
                Json = (IEnumerable<object>)finalData;
                DataAdaptor = new BlazorAdaptor(this);

                //Process with actual query and send data
                DataManagerRequest request1 = (DataManagerRequest)DataAdaptor.ProcessQuery(queries);
                object dataResult1 = await DataAdaptor.PerformDataOperation<T>(request1);
                object finalData1 = await DataAdaptor.ProcessResponse<T>(dataResult1, request1);
                return finalData1;
            }
        }

        /// <summary>
        /// Invoked before sending http request.
        /// </summary>
        /// <param name="request">HttpRequestMessage instance.</param>
        public void BeforeSend(HttpRequestMessage request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            DataAdaptor.BeforeSend(request);
            if (Headers?.Count > 0)
            {
                IDictionary<string, string> headers = Headers;
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        internal void InitDataManagerAdaptor()
        {
            switch (Adaptor)
            {
                case Adaptors.ODataAdaptor:
                    DataAdaptor = new ODataAdaptor(this);
                    break;
                case Adaptors.ODataV4Adaptor:
                    DataAdaptor = new ODataV4Adaptor(this);
                    break;
                case Adaptors.RemoteSaveAdaptor:
                    break;
                case Adaptors.WebApiAdaptor:
                    DataAdaptor = new WebApiAdaptor(this);
                    break;

                // case Adaptors.ApiAdaptor:
                //    DataAdaptor = new ApiAdaptor(this);
                //    break;
                case Adaptors.UrlAdaptor:
                    DataAdaptor = new UrlAdaptor(this);
                    break;
                default:
                    if (!string.IsNullOrEmpty(Url))
                    {
                        DataAdaptor = new ODataAdaptor(this);
                    }
                    else
                    {
                        DataAdaptor = new BlazorAdaptor(this);
                    }

                    break;
            }
        }

        /// <summary>
        /// Performs the new item add operation.
        /// </summary>
        /// <param name="data">New item to be added.</param>
        /// <param name="tableName">Table name to insert new item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="position">Position to insert the new item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> Insert<T>(object data, string tableName = null, Query query = null, int position = 0)
        {
            if (Adaptor == Adaptors.CustomAdaptor)
            {
                return await BaseAdaptor.Insert(data);
            }

            if (DataAdaptor.IsRemote())
            {
                var request = DataAdaptor.Insert(this, data, tableName, query, position);
                using (var queryRequest = HttpHandler.PrepareRequest(request as RequestOptions))
                {
                    BeforeSend(queryRequest as HttpRequestMessage);
                    var reqMessage = await DataAdaptor.PerformDataOperation<T>(queryRequest);
                    return await DataAdaptor.ProcessCrudResponse<T>(reqMessage, query?.Queries);
                }
            }
            else
            {
                if (data != null && typeof(IDynamicMetaObjectProvider).IsAssignableFrom(data.GetType()))
                {
                    return DataAdaptor.Insert(this, data as IDynamicMetaObjectProvider, tableName, query, position);
                }
                else
                {
                    if (Json is Array)
                    {
                        return BlazorAdaptor.InsertArray<T>(this, data, position);
                    }
                    else
                    {
                        return DataAdaptor.Insert(this, data, tableName, query, position);
                    }
                }
            }
        }

        /// <summary>
        /// Performs the new item add operation.
        /// </summary>
        /// <param name="data">New item to be added.</param>
        /// <param name="tableName">Table name to insert new item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="position">Position to insert the new item.</param>
        /// <returns>Task.</returns>
        public async Task<object> InsertAsync<T>(object data, string tableName = null, Query query = null, int position = 0)
        {
            return await Insert<T>(data, tableName, query, position);
        }

        /// <summary>
        /// Performs the update operation.
        /// </summary>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="data">Specifies the updated record.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending changed items alone to the server.</param>
        /// <param name="updateProperties">Specifies the field names to be updated.</param>
        /// <returns>object.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> Update<T>(string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null)
        {
            if (Adaptor == Adaptors.CustomAdaptor)
            {
                return await BaseAdaptor.Update(data, keyField, null);
            }

            if (DataAdaptor.IsRemote())
            {
                var request = DataAdaptor.Update(this, keyField, data, tableName, query, original, updateProperties);
                using (var queryRequest = HttpHandler.PrepareRequest(request as RequestOptions))
                {
                    BeforeSend(queryRequest as HttpRequestMessage);
                    var reqMessage = await DataAdaptor.PerformDataOperation<T>(queryRequest);
                    return await DataAdaptor.ProcessCrudResponse<T>(reqMessage, query?.Queries);
                }
            }
            else
            {
                if (data != null && typeof(IDynamicMetaObjectProvider).IsAssignableFrom(data.GetType()))
                {
                    return DataAdaptor.Update(this, keyField, (IDynamicMetaObjectProvider)data);
                }
                else
                {
                    return DataAdaptor.Update(this, keyField, data, tableName, query, original, updateProperties);
                }
            }
        }

        /// <summary>
        /// Performs the update operation.
        /// </summary>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="data">Specifies the updated record.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending changed items alone to the server.</param>
        /// <param name="updateProperties">Specifies the field names to be updated.</param>
        /// <returns>object.</returns>
        public async Task<object> UpdateAsync<T>(string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null)
        {
            return await Update<T>(keyField, data, tableName, query, original, updateProperties);
        }

        /// <summary>
        /// Performs the remove operation.
        /// </summary>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="value">Specifies the primary key field value.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <returns>object.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> Remove<T>(string keyField, object value, string tableName = null, Query query = null)
        {
            if (Adaptor == Adaptors.CustomAdaptor)
            {
                return await BaseAdaptor.Remove(value, keyField, null);
            }

            if (DataAdaptor.IsRemote())
            {
                var request = DataAdaptor.Remove(this, keyField, value, tableName, query);
                using (var queryRequest = HttpHandler.PrepareRequest(request as RequestOptions))
                {
                    BeforeSend(queryRequest as HttpRequestMessage);
                    var reqMessage = await DataAdaptor.PerformDataOperation<T>(queryRequest);
                    return await DataAdaptor.ProcessCrudResponse<T>(reqMessage, query?.Queries);
                }
            }
            else
            {
                if (Json is Array)
                {
                    return BlazorAdaptor.RemoveArray<T>(this, keyField, value);
                }
                else
                {
                    return DataAdaptor.Remove(this, keyField, value);
                }
            }
        }

        /// <summary>
        /// Performs the remove operation.
        /// </summary>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="value">Specifies the primary key field value.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <returns>object.</returns>
        public async Task<object> RemoveAsync<T>(string keyField, object value, string tableName = null, Query query = null)
        {
            return await Remove<T>( keyField, value, tableName, query);
        }

        /// <summary>
        /// Performs the batch update operation. Enables user to perform add, update and remove items from data source in a single request.
        /// </summary>
        /// <param name="changed">Specifies the changed records.</param>
        /// <param name="added">Specifies the added records.</param>
        /// <param name="deleted">Specifies the deleted records.</param>
        /// <param name="keyField">Specifies the primary key field.</param>
        /// <param name="dropIndex">Specifies the record position, from which new records will be added.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="Original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> SaveChanges<T>(object changed, object added, object deleted, string keyField, int? dropIndex, string tableName = null, Query query = null, object Original = null)
        {
            Data.Utils e = new Data.Utils()
            {
                Url = tableName,
                Key = keyField
            };
            if (Adaptor == Adaptors.CustomAdaptor)
            {
                return await BaseAdaptor.BatchUpdate(changed, added, deleted, keyField, null, dropIndex);
            }

            if (DataAdaptor.IsRemote())
            {
                var request = DataAdaptor.BatchUpdate(this, changed, added, deleted, e, keyField, dropIndex, query, Original);
                HttpRequestMessage queryRequest = null;
                if ((DataAdaptor.GetName() == nameof(ODataAdaptor) || DataAdaptor.GetName() == nameof(ODataV4Adaptor)) && (keyField != null))
                {
                    Type ModelType = ODataAdaptor.GetPropertyType(keyField, DataAdaptor.GetModelType());
                    queryRequest = HttpHandler.PrepareBatchRequest(request as RequestOptions, ModelType);
                }
                else
                {
                    queryRequest = HttpHandler.PrepareRequest(request as RequestOptions);
                }
                using (queryRequest)
                {
                    BeforeSend(queryRequest as HttpRequestMessage);
                    var reqMessage = await DataAdaptor.PerformDataOperation<T>(queryRequest);
                    if (DataAdaptor.GetName() == nameof(ODataAdaptor) || DataAdaptor.GetName() == nameof(ODataV4Adaptor))
                        return await DataAdaptor.ProcessBatchResponse<T>(reqMessage, query?.Queries);
                    else
                        return await DataAdaptor.ProcessCrudResponse<CRUDModel<T>>(reqMessage, query?.Queries);
                }
            }
            else
            {
                object data = null;
                if (changed != null && (changed as IList).Count > 0)
                {
                    data = changed;
                }
                else if (added != null && (added as IList).Count > 0)
                    data = added;
                else
                {
                    data = deleted;
                }

                var isDynamicType = typeof(IDynamicMetaObjectProvider).IsAssignableFrom((data as IEnumerable).Cast<object>().ToList().FirstOrDefault()?.GetType());
                if (isDynamicType)
                {
                    return DataAdaptor.BatchUpdate(this, (changed as IEnumerable).Cast<IDynamicMetaObjectProvider>().ToList(), (added as IEnumerable).Cast<IDynamicMetaObjectProvider>().ToList(), (deleted as IEnumerable).Cast<IDynamicMetaObjectProvider>().ToList(), e, keyField, dropIndex, query);
                }
                else
                {
                    if (Json is Array)
                    {
                        return BlazorAdaptor.BatchUpdateArray<T>(this, changed, added, deleted, keyField, dropIndex);
                    }
                    else
                    {
                        return DataAdaptor.BatchUpdate(this, changed, added, deleted, e, keyField, dropIndex, query);
                    }
                }
            }
        }

        /// <summary>
        /// Performs the batch update operation. Enables user to perform add, update and remove items from data source in a single request.
        /// </summary>
        /// <param name="changed">Specifies the changed records.</param>
        /// <param name="added">Specifies the added records.</param>
        /// <param name="deleted">Specifies the deleted records.</param>
        /// <param name="keyField">Specifies the primary key field.</param>
        /// <param name="dropIndex">Specifies the record position, from which new records will be added.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="Original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        public async Task<object> SaveChangesAsync<T>(object changed, object added, object deleted, string keyField, int? dropIndex, string tableName = null, Query query = null, object Original = null)
        {
            return await SaveChanges<T>(changed, added, deleted, keyField, dropIndex, tableName, query, Original);
        }

        /// <summary>
        /// Dispose unmanaged resources in the Syncfusion Blazor component.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose unmanaged resources in the Syncfusion Blazor component.
        /// </summary>
        /// <param name="disposing">Boolean value to dispose the object.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            if (disposing)
            {
                DotNetObjectRef?.Dispose();
                BaseParent?.ChildDotNetObjectRef.Clear();
            }
        }
    }

    /// <summary>
    /// Defines the members of the query.
    /// </summary>
    /// <remarks>DataManagerRequest is used to model bind posted data at server side.</remarks>
    // [TypeConverter(typeof(DataManagerTypeConverter))]
    public class DataManagerRequest
    {
        /// <summary>
        /// Specifies the records to skip.
        /// </summary>
        [JsonProperty("skip")]
        public int Skip { get; set; }

        /// <summary>
        /// Specifies the records to take.
        /// </summary>
        [JsonProperty("take")]
        public int Take { get; set; }

        /// <summary>
        /// Specifies the anti-forgery key.
        /// </summary>
        /// <exclude/>
        [JsonProperty("antiForgery")]
        public string antiForgery { get; set; }

        /// <summary>
        /// Sepcifies that the count is required in response.
        /// </summary>
        [JsonProperty("requiresCounts")]
        public bool RequiresCounts { get; set; }

        /// <summary>
        /// Specifies the table name.
        /// </summary>
        [JsonProperty("table")]
        public string Table { get; set; }

        /// <summary>
        /// Specifies the parent id mapping value.
        /// </summary>
        [JsonProperty("IdMapping")]
        public string IdMapping { get; set; }

        /// <summary>
        /// Specifies the grouped column details.
        /// </summary>
        [JsonProperty("group")]
        public List<string> Group { get; set; }

        /// <summary>
        /// Specifies the select column details.
        /// </summary>
        [JsonProperty("select")]
        public List<string> Select { get; set; }

        /// <summary>
        /// Specifies the relational table names to be eagerloaded.
        /// </summary>
        [JsonProperty("expand")]
        public List<string> Expand { get; set; }

        /// <summary>
        /// Speccifies the sort criteria.
        /// </summary>
        [JsonProperty("sorted")]
        public List<Sort> Sorted { get; set; }

        /// <summary>
        /// Specifies the search criteria.
        /// </summary>
        [JsonProperty("search")]
        public List<SearchFilter> Search { get; set; }

        /// <summary>
        /// Specifies the filter criteria.
        /// </summary>
        [JsonProperty("where")]
        public List<WhereFilter> Where { get; set; }

        /// <summary>
        /// Specifies the aggregate details.
        /// </summary>
        [JsonProperty("aggregates")]
        public List<Aggregate> Aggregates { get; set; }

        /// <summary>
        /// Specifies additional parameters.
        /// </summary>
        [JsonProperty("params")]
        public IDictionary<string, object> Params { get; set; }

        /// <summary>
        /// Specifies the field names to find distinct values.
        /// </summary>
        [JsonProperty("distinct")]
        public List<string> Distinct { get; set; }

        /// <summary>
        /// Holds field and format method to handle group by format.
        /// </summary>
        public IDictionary<string, string> GroupByFormatter { get; set; }

        /// <summary>
        /// Specifies that perform in-built grouping.
        /// </summary>
        public bool ServerSideGroup { get; set; } = true;

        /// <summary>
        /// Sepcifies that the filtered records is required in response.
        /// </summary>
        public bool RequiresFilteredRecords { get; set; }

        /// <summary>
        /// Specifies that perform lazy load grouping.
        /// </summary>
        public bool LazyLoad { get; set; }

        /// <summary>
        /// Specifies that to perform expand all for lazy load grouping.
        /// </summary>
        public bool LazyExpandAllGroup { get; set; }
    }

    /// <summary>
    /// Abstract class for Data adaptors.
    /// </summary>
    /// <remarks>
    /// Extend DataAdaptor component while creating custom adaptor component. DataAdaptor component is extended from
    /// <see cref="Microsoft.AspNetCore.Components.OwningComponentBase"></see> so that
    /// services can be accessed from <see cref="Microsoft.AspNetCore.Components.OwningComponentBase.ScopedServices"/> property.
    /// </remarks>
    public abstract class DataAdaptor : OwningComponentBase, IDataAdaptor
    {
        internal static JsonSerializerSettings SerializeSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
        };

        internal BaseComponent Parent;

        public void SetParent(BaseComponent parent)
        {
            Parent = parent;
        }

        [CascadingParameter]
        internal SfDataManager DataManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (DataManager.AdaptorInstance == null)
            {
                DataManager.BaseAdaptor.Instance = this;
                DataManager.BaseAdaptor.Instance.SetParent(DataManager.BaseAdaptor.ParentComponent as BaseComponent);
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Performs data Read operation synchronously.
        /// </summary>
        public virtual object Read(DataManagerRequest dataManagerRequest, string key = null) => Parent.DataProcess(JsonConvert.SerializeObject(dataManagerRequest, Formatting.Indented, SerializeSettings), key);

        /// <summary>
        /// Performs data Read operation asynchronously.
        /// </summary>
        public virtual Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null) => Task.FromResult<object>(null);

        /// <summary>
        /// Performs Insert operation synchronously.
        /// </summary>
        public virtual object Insert(DataManager dataManager, object data, string key) => null;

        /// <summary>
        /// Performs Insert operation asynchronously.
        /// </summary>
        public virtual Task<object> InsertAsync(DataManager dataManager, object data, string key) => Task.FromResult<object>(new AsyncHandler());

        /// <summary>
        /// Performs Remove operation synchronously.
        /// </summary>
        public virtual object Remove(DataManager dataManager, object data, string keyField, string key) => null;

        /// <summary>
        /// Performs Remove operation asynchronously..
        /// </summary>
        public virtual Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key) => Task.FromResult<object>(new AsyncHandler());

        /// <summary>
        /// Performs Update operation synchronously.
        /// </summary>
        public virtual object Update(DataManager dataManager, object data, string keyField, string key) => null;

        /// <summary>
        /// Performs Update operation asynchronously.
        /// </summary>
        public virtual Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key) => Task.FromResult<object>(new AsyncHandler());

        /// <summary>
        /// Performs Batch CRUD operations synchronously.
        /// </summary>
        public virtual object BatchUpdate(DataManager dataManager, object changedRecords, object addedRecords, object deletedRecords, string keyField, string key, int? dropIndex) => null;

        /// <summary>
        /// Performs Batch CRUD operations asynchronously.
        /// </summary>
        public virtual Task<object> BatchUpdateAsync(DataManager dataManager, object changedRecords, object addedRecords, object deletedRecords, string keyField, string key, int? dropIndex) => Task.FromResult<object>(new AsyncHandler());
    }

    /// <summary>
    /// Abstract class for Data adaptors.
    /// </summary>
    /// <remarks>
    /// Extend DataAdaptor{T} component while creating custom adaptor component. DataAdaptor{T} component is extended from
    /// <see cref="Microsoft.AspNetCore.Components.OwningComponentBase{TService}"></see> so that
    /// services can be accessed from <see cref="Microsoft.AspNetCore.Components.OwningComponentBase{TService}.Service"/> property.
    /// </remarks>
    public abstract class DataAdaptor<T> : OwningComponentBase<T>, IDataAdaptor
    {
        internal static JsonSerializerSettings SerializeSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
        };

        internal BaseComponent Parent { get; set; }

        [CascadingParameter]
        internal SfDataManager DataManager { get; set; }

        public void SetParent(BaseComponent parent)
        {
            Parent = parent;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DataManager.BaseAdaptor.Instance = this;
            DataManager.BaseAdaptor.Instance.SetParent(DataManager.BaseAdaptor.ParentComponent as BaseComponent);
        }

        /// <summary>
        /// Performs data Read operation synchronously.
        /// </summary>
        public virtual object Read(DataManagerRequest dataManagerRequest, string key = null) => Parent.DataProcess(JsonConvert.SerializeObject(dataManagerRequest, Formatting.Indented, SerializeSettings), key);

        /// <summary>
        /// Performs data Read operation asynchronously.
        /// </summary>
        public virtual Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null) => Task.FromResult<object>(null);

        /// <summary>
        /// Performs Insert operation synchronously.
        /// </summary>
        public virtual object Insert(DataManager dataManager, object data, string key) => null;

        /// <summary>
        /// Performs Insert operation asynchronously.
        /// </summary>
        public virtual Task<object> InsertAsync(DataManager dataManager, object data, string key) => Task.FromResult<object>(new AsyncHandler());

        /// <summary>
        /// Performs Remove operation synchronously.
        /// </summary>
        public virtual object Remove(DataManager dataManager, object data, string keyField, string key) => null;

        /// <summary>
        /// Performs Remove operation asynchronously..
        /// </summary>
        public virtual Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key) => Task.FromResult<object>(new AsyncHandler());

        /// <summary>
        /// Performs Update operation synchronously.
        /// </summary>
        public virtual object Update(DataManager dataManager, object data, string keyField, string key) => null;

        /// <summary>
        /// Performs Update operation asynchronously.
        /// </summary>
        public virtual Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key) => Task.FromResult<object>(new AsyncHandler());

        /// <summary>
        /// Performs Batch CRUD operations synchronously.
        /// </summary>
        public virtual object BatchUpdate(DataManager dataManager, object changedRecords, object addedRecords, object deletedRecords, string keyField, string key, int? dropIndex) => null;

        /// <summary>
        /// Performs Batch CRUD operations asynchronously.
        /// </summary>
        public virtual Task<object> BatchUpdateAsync(DataManager dataManager, object changedRecords, object addedRecords, object deletedRecords, string keyField, string key, int? dropIndex) => Task.FromResult<object>(new AsyncHandler());
    }
}

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Defines the sort descriptor.
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// Gets the field name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the sort direction.
        /// </summary>
        public string Direction { get; set; }
         
        /// <summary>
        /// Gets the sort comparer
        /// </summary>
        public object Comparer { get; set; }
    }

    /// <summary>
    /// Defines members for creating search criteria.
    /// </summary>
    public class SearchFilter
    {
        /// <summary>
        /// Collection of fields to search.
        /// </summary>
        public List<string> Fields { get; set; }

        /// <summary>
        /// Specifies the search key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Specifies the search operator. By default, contains operator will be used.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Specifies that incasesensitive search to be done.
        /// </summary>
        public bool IgnoreCase { get; set; }
    }

    /// <summary>
    /// Defines the members of the aggregate.
    /// </summary>
    public class Aggregate
    {
        /// <summary>
        /// Specifies the field name.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// Specifies the aggregate type.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// Defines the members to build filter criteria.
    /// </summary>
    public class WhereFilter
    {
        /// <summary>
        /// Specifies the field name.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// Specifies that filter should be incasesensitive.
        /// </summary>
        [JsonProperty("ignoreCase")]
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Specifies that ignore accent/diacritic letters while searching.
        /// </summary>
        [JsonProperty("ignoreAccent")]
        public bool IgnoreAccent { get; set; }

        /// <summary>
        /// When true it specifies that the filter criteria is a complex one.
        /// </summary>
        [JsonProperty("isComplex")]
        public bool IsComplex { get; set; }

        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        [JsonProperty("operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Provides the complex filter merge condition.
        /// </summary>
        [JsonProperty("condition")]
        public string Condition { get; set; }

        /// <summary>
        /// Specifies the filter value.
        /// </summary>
        [JsonProperty("value")]
        public object value { get; set; }

        /// <summary>
        /// Specifies the collection filter criteria.
        /// </summary>
        [JsonProperty("predicates")]
        public List<WhereFilter> predicates { get; set; }

        /// <summary>
        /// Merge the give collection of predicates using And condition.
        /// </summary>
        /// <param name="predicates">List of predicates.</param>
        /// <returns>WhereFilter.</returns>
        public static WhereFilter And(List<WhereFilter> predicates)
        {
            return new WhereFilter() { Condition = "and", IsComplex = true, predicates = predicates };
        }

        /// <summary>
        /// Merge the give collection of predicates using Or condition.
        /// </summary>
        /// <param name="predicates">List of predicates.</param>
        /// <returns>WhereFilter.</returns>
        public static WhereFilter Or(List<WhereFilter> predicates)
        {
            return new WhereFilter() { Condition = "or", IsComplex = true, predicates = predicates };
        }

        /// <summary>
        /// Merge the give predicate using And condition.
        /// </summary>
        /// <param name="fieldName">Specifies the field name.</param>
        /// <param name="operator">Specifies the filter operator.</param>
        /// <param name="value">Specifies the filter value.</param>
        /// <param name="ignoreCase">Performs incasesensitive filtering.</param>
        /// <param name="ignoreAccent">Ignores accent/diacritic letters while filtering.</param>
        /// <returns></returns>
        public WhereFilter And(string fieldName, string @operator = null, object value = null, bool ignoreCase = false, bool ignoreAccent = false)
        {
            WhereFilter predicate = new WhereFilter()
            {
                Field = fieldName,
                Operator = @operator,
                value = value,
                IgnoreCase = ignoreCase,
                IgnoreAccent = ignoreAccent
            };
            WhereFilter combined = new WhereFilter()
            {
                Condition = "and",
                IsComplex = true,
                predicates = new List<WhereFilter>()
                {
                    this,
                    predicate
                }
            };
            return combined;
        }

        /// <summary>
        /// Merge the give predicate using And condition.
        /// </summary>
        /// <param name="predicate">Predicate to be merged.</param>
        /// <returns>WhereFilter.</returns>
        public WhereFilter And(WhereFilter predicate)
        {
            WhereFilter combined = new WhereFilter()
            {
                Condition = "and",
                IsComplex = true,
                predicates = new List<WhereFilter>()
                {
                    this,
                    predicate
                }
            };
            return combined;
        }

        /// <summary>
        /// Merge the give predicate using Or condition.
        /// </summary>
        /// <param name="fieldName">Specifies the field name.</param>
        /// <param name="operator">Specifies the filter operator.</param>
        /// <param name="value">Specifies the filter value.</param>
        /// <param name="ignoreCase">Performs incasesensitive filtering.</param>
        /// <param name="ignoreAccent">Ignores accent/diacritic letters while filtering.</param>
        /// <returns></returns>
        public WhereFilter Or(string fieldName, string @operator = null, object value = null, bool ignoreCase = false, bool ignoreAccent = false)
        {
            WhereFilter predicate = new WhereFilter()
            {
                Field = fieldName,
                Operator = @operator,
                value = value,
                IgnoreCase = ignoreCase,
                IgnoreAccent = ignoreAccent
            };
            WhereFilter combined = new WhereFilter()
            {
                Condition = "or",
                IsComplex = true,
                predicates = new List<WhereFilter>()
                {
                    this,
                    predicate
                }
            };
            return combined;
        }

        /// <summary>
        /// Merge the give predicate using Or condition.
        /// </summary>
        /// <param name="predicate">Predicate to be merged.</param>
        /// <returns>WhereFilter.</returns>
        public WhereFilter Or(WhereFilter predicate)
        {
            WhereFilter combined = new WhereFilter()
            {
                Condition = "or",
                IsComplex = true,
                predicates = new List<WhereFilter>()
                {
                    this,
                    predicate
                }
            };
            return combined;
        }
    }

    /// <summary>
    /// Provide adaptor information which sends to client side.
    /// </summary>
    /// <exclude/>
    public class DefaultAdaptor
    {
        [Parameter]
        [JsonProperty("adaptor")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Adaptors Adaptor { get; set; } = Adaptors.BlazorAdaptor;

        [JsonProperty("adaptorName")]
        public Adaptors AdaptorName { get; set; }

        [JsonProperty("key")]
        internal string Key { get; set; }

        [JsonProperty("url")]
        [DefaultValue("")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("offline")]
        public bool Offline { get; set; }

        public DefaultAdaptor()
        {
        }

        public DefaultAdaptor(string key, Adaptors adaptorName = Adaptors.BlazorAdaptor)
        {
            Key = key;
            AdaptorName = adaptorName;
        }

        public DefaultAdaptor(string key, DataManager manager, Adaptors adaptorName = Adaptors.BlazorAdaptor)
        {
            Key = key;
            AdaptorName = adaptorName;
            Url = manager?.Url;
            Offline = manager?.Offline ?? false;
        }
    }

    /// <summary>
    /// Defines the members of the data manager operation result.
    /// </summary>
    public class DataResult : DataResult<object>
    {
    }

    /// <summary>
    /// Defines the members of the data manager operation result.
    /// </summary>
    /// <typeparam name="T">Type of the data source element.</typeparam>
    public class DataResult<T>
    {
        /// <summary>
        /// Gets the result of the data operation.
        /// </summary>
        [JsonProperty("result")]
        public IEnumerable Result { get; set; }

        /// <summary>
        /// Gets the total count of the records in data source.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets the aggregate result based on the aggregate query.
        /// </summary>
        [JsonProperty("aggregates")]
        public IDictionary<string, object> Aggregates { get; set; }

        /// <summary>
        /// Gets the filtered records.
        /// </summary>
        public IEnumerable FilteredRecords { get; set; }
    }

    /// <summary>
    /// Handles custom adaptor logic.
    /// </summary>
    /// <exclude/>
    public class BaseAdaptor
    {
        public IDataAdaptor Instance { get; set; }
        public Type GenericType { get; set; }
        public Object ParentComponent { get; set; }
        public DataManager DataManagerInstance { get; set; }
        public static JsonSerializerSettings DeserializeSettings { get; set; } = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            NullValueHandling = NullValueHandling.Ignore
        };
        public static JsonSerializerSettings SerializeSettings { get; set; } = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
        };

        public BaseAdaptor(Type type, object parentComponent, DataManager dataManagerInstance)
        {
            ParentComponent = parentComponent;
            DataManagerInstance = dataManagerInstance;
            if (type != null && DataManagerInstance != null)
            {
                Instance = (DataAdaptor)DataManagerInstance.ServiceProvider.GetService(type);
                Instance = Instance == null ? (DataAdaptor)Activator.CreateInstance(type) : Instance;
                Instance.SetParent(parentComponent as BaseComponent);
            }

            GenericType = ParentComponent.GetType();
            if (GenericType.IsGenericType && GenericType.GetGenericArguments().Length > 0)
            {
                GenericType = GenericType.GetGenericArguments()[0];
            }
            else
            {
                GenericType = null;
            }
        }

        [JSInvokable]
        public async Task<string> BaseRead(string request, string key)
        {
            DataManagerRequest dataRequest = JsonConvert.DeserializeObject<DataManagerRequest>(request);
            object data = await Read(dataRequest, key);
            return data.GetType().Name == "String" ? data as string : JsonConvert.SerializeObject(data, Formatting.Indented, SerializeSettings);
        }

        public async Task<object> Read(DataManagerRequest dataRequest, string key = null)
        {
            var task = Instance.ReadAsync(dataRequest);
            if (task.Status == TaskStatus.RanToCompletion)
            {
                return (await task) ?? Instance.Read(dataRequest, key);
            }

            return (task.Status == TaskStatus.Canceled) ? Instance.Read(dataRequest, key) : await task;

            // if (task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
            // {
            //    data = await task;
            // }
            // else
            // {
            //    data = Instance.Read(dataRequest, key);
            // }
            // return data;
        }

        [JSInvokable]
        public async Task<string> BaseInsert(string baseData, string key)
        {
            object data = null;
            if (GenericType != null)
            {
                data = JsonConvert.DeserializeObject(baseData, GenericType, DeserializeSettings);
            }
            else
            {
                data = JsonConvert.DeserializeObject<object>(baseData, DeserializeSettings);
            }

            object value = await Insert(data, key);
            return JsonConvert.SerializeObject(value, Formatting.Indented, SerializeSettings);
        }

        public async Task<object> Insert(object data, string key = null)
        {
            var task = Instance.InsertAsync(DataManagerInstance, data, key);
            if (task.Status == TaskStatus.RanToCompletion)
            {
                var value = await task;
                if (value != null && value is AsyncHandler)
                {
                    return Instance.Insert(DataManagerInstance, data, key);
                }
                else
                {
                    return value;
                }
            }

            return (task.Status == TaskStatus.Canceled) ? Instance.Insert(DataManagerInstance, data, key) : await task;
        }

        [JSInvokable]
        public async Task<string> BaseRemove(string baseData, string keyField, string key)
        {
            object data = null;
            if (GenericType != null)
            {
                PropertyInfo keyFieldProperty = GenericType.GetProperty(keyField);
                if (keyFieldProperty == null)
                {
                    PropertyInfo[] props = GenericType.GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        foreach (object attr in prop.GetCustomAttributes())
                        {
                            if ((attr as JsonPropertyAttribute) != null && (attr as JsonPropertyAttribute).PropertyName == keyField)
                            {
                                keyFieldProperty = prop;
                            }
                        }
                    }
                }

                Type propertyType = keyFieldProperty.PropertyType;
                data = (propertyType.Name == "Guid") ? JsonConvert.DeserializeObject<Guid>(baseData) : SfBaseUtils.ChangeType(JsonConvert.DeserializeObject(baseData, DeserializeSettings), propertyType);
            }
            else
            {
                data = JsonConvert.DeserializeObject<object>(baseData, DeserializeSettings);
            }

            object val = await Remove(data, keyField, key);
            return JsonConvert.SerializeObject(val, Formatting.Indented, SerializeSettings);
        }

        public async Task<object> Remove(object data, string keyField, string key)
        {
            var task = Instance.RemoveAsync(DataManagerInstance, data, keyField, key);
            if (task.Status == TaskStatus.RanToCompletion)
            {
                var value = await task;
                if (value != null && value is AsyncHandler)
                {
                    Instance.Remove(DataManagerInstance, data, keyField, key);
                }
                else
                {
                    return value;
                }
            }

            return (task.Status == TaskStatus.Canceled) ? Instance.Remove(DataManagerInstance, data, keyField, key) : await task;
        }

        [JSInvokable]
        public async Task<object> BaseUpdate(string baseData, string keyField, string key)
        {
            object data = null;
            if (GenericType != null)
            {
                data = JsonConvert.DeserializeObject(baseData, GenericType, DeserializeSettings);
            }
            else
            {
                data = JsonConvert.DeserializeObject<object>(baseData, DeserializeSettings);
            }

            object value = await Update(data, keyField, key);
            return JsonConvert.SerializeObject(value, Formatting.Indented, SerializeSettings);
        }

        public async Task<object> Update(object data, string keyField, string key)
        {
            var task = Instance.UpdateAsync(DataManagerInstance, data, keyField, key);
            if (task.Status == TaskStatus.RanToCompletion)
            {
                var value = await task;
                if (value != null && value is AsyncHandler)
                {
                    return Instance.Update(DataManagerInstance, data, keyField, key);
                }
                else
                {
                    return value;
                }
            }

            return (task.Status == TaskStatus.Canceled) ? Instance.Update(DataManagerInstance, data, keyField, key) : await task;
        }

        [JSInvokable]
        public async Task<object> BaseBatchUpdate(string changed, string added, string deleted, string keyField, string key, int? dropIndex)
        {
            object changedRecords = null;
            object addedRecords = null;
            object deletedRecords = null;
            if (changed != null)
            {
                if (GenericType != null)
                {
                    Type GenType = typeof(List<>).MakeGenericType(GenericType);
                    changedRecords = JsonConvert.DeserializeObject(changed, GenType, DeserializeSettings);
                }
                else
                {
                    changedRecords = JsonConvert.DeserializeObject<object>(changed, DeserializeSettings);
                }
            }

            if (added != null)
            {
                if (GenericType != null)
                {
                    Type GenType = typeof(IEnumerable<>).MakeGenericType(GenericType);
                    addedRecords = JsonConvert.DeserializeObject(added, GenType, DeserializeSettings);
                }
                else
                {
                    addedRecords = JsonConvert.DeserializeObject<object>(added, DeserializeSettings);
                }
            }

            if (deleted != null)
            {
                if (GenericType != null)
                {
                    Type GenType = typeof(IEnumerable<>).MakeGenericType(GenericType);
                    deletedRecords = JsonConvert.DeserializeObject(deleted, GenType, DeserializeSettings);
                }
                else
                {
                    deletedRecords = JsonConvert.DeserializeObject<object>(deleted, DeserializeSettings);
                }
            }

            object value = await BatchUpdate(changedRecords, addedRecords, deletedRecords, keyField, key, dropIndex);

            return JsonConvert.SerializeObject(value, Formatting.Indented, SerializeSettings);
        }

        public async Task<object> BatchUpdate(object changedRecords, object addedRecords, object deletedRecords,
            string keyField, string key, int? dropIndex)
        {
            var task = Instance.BatchUpdateAsync(DataManagerInstance, changedRecords, addedRecords, deletedRecords, keyField, key, dropIndex);
            if (task.Status == TaskStatus.RanToCompletion)
            {
                var value = await task;
                if (value != null && value is AsyncHandler)
                {
                    return Instance.BatchUpdate(DataManagerInstance, changedRecords, addedRecords, deletedRecords, keyField, key, dropIndex);
                }
                else
                {
                    return value;
                }
            }

            return (task.Status == TaskStatus.Canceled) ? Instance.BatchUpdate(DataManagerInstance, changedRecords, addedRecords, deletedRecords, keyField, key, dropIndex) : await task;
        }
    }

    /// <summary>
    /// Defines the members of the grouped record.
    /// </summary>
    /// <typeparam name="T">Type of the data source elements.</typeparam>
    public class Group<T> : List<Group<T>>
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        [JsonProperty("groupGuid")]
        public string GroupGuid { get; set; }

        /// <summary>
        /// Specifies the level of this group.
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// Specifies the count of child if any.
        /// </summary>
        [JsonProperty("childLevels")]
        public int ChildLevels { get; set; }

        /// <summary>
        /// Specifies the ungrouped records.
        /// </summary>
        [JsonProperty("records")]
        public object[] Records { get; set; }

        /// <summary>
        /// Specifies the group key value.
        /// </summary>
        [JsonProperty("key")]
        public object Key { get; set; }

        /// <summary>
        /// Specifies the count of items in this group.
        /// </summary>
        [JsonProperty("count")]
        public int CountItems { get; set; }

        /// <summary>
        /// Specifies the items of the group.
        /// </summary>
        [JsonProperty("items")]
        public IEnumerable Items { get; set; }

        /// <summary>
        /// Specifies the aggregates of this group.
        /// </summary>
        [JsonProperty("aggregates")]
        public object Aggregates { get; set; }

        /// <summary>
        /// Specifies the field value.
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }

        /// <summary>
        /// Specifies the header text of the field.
        /// </summary>
        [JsonProperty("headerText")]
        public string HeaderText { get; set; }

        /// <summary>
        /// Specifies the foreign key.
        /// </summary>
        [JsonProperty("foreignKey")]
        public string ForeignKey { get; set; }

        /// <summary>
        /// Specifies the result.
        /// </summary>
        [JsonProperty("result")]
        public object Result { get; set; }

        /// <summary>
        /// Specifies the grouped data.
        /// </summary>
        internal IEnumerable GroupedData { get; set; }
    }

    /// <summary>
    /// Interface for Data adaptors.
    /// </summary>
    public interface IDataAdaptor
    {
        public void SetParent(BaseComponent parent);

        /// <summary>
        /// Performs data Read operation synchronously.
        /// </summary>
        public object Read(DataManagerRequest dataManagerRequest, string key = null);

        /// <summary>
        /// Performs data Read operation asynchronously.
        /// </summary>
        public Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null);

        /// <summary>
        /// Performs Insert operation synchronously.
        /// </summary>
        public object Insert(DataManager dataManager, object data, string key);

        /// <summary>
        /// Performs Insert operation asynchronously.
        /// </summary>
        public Task<object> InsertAsync(DataManager dataManager, object data, string key);

        /// <summary>
        /// Performs Remove operation synchronously.
        /// </summary>
        public object Remove(DataManager dataManager, object data, string keyField, string key);

        /// <summary>
        /// Performs Remove operation asynchronously..
        /// </summary>
        public Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key);

        /// <summary>
        /// Performs Update operation synchronously.
        /// </summary>
        public object Update(DataManager dataManager, object data, string keyField, string key);

        /// <summary>
        /// Performs Update operation asynchronously.
        /// </summary>
        public Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key);

        /// <summary>
        /// Performs Batch CRUD operations synchronously.
        /// </summary>
        public object BatchUpdate(DataManager dataManager, object changedRecords, object addedRecords, object deletedRecords, string keyField, string key, int? dropIndex);

        /// <summary>
        /// Performs Batch CRUD operations asynchronously.
        /// </summary>
        public Task<object> BatchUpdateAsync(DataManager dataManager, object changedRecords, object addedRecords, object deletedRecords, string keyField, string key, int? dropIndex);
    }
}
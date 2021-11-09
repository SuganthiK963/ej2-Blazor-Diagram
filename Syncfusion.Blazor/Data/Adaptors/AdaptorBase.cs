using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Dynamic;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Provides the members for data adaptor class.
    /// </summary>
    /// <exclude/>
    public interface IAdaptor
    {
        /// <summary>
        /// Returns the adaptor name.
        /// </summary>
        /// <returns>string.</returns>
        public string GetName();

        /// <summary>
        /// Runs the data operation synchronously.
        /// </summary>
        /// <param name="runSync">Enables synchronous data operation.</param>
        public void SetRunSyncOnce(bool runSync);

        /// <summary>
        /// Read query from <see cref="Syncfusion.Blazor.Data.Query"/> and make it understandable by
        /// data source.
        /// </summary>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>object.</returns>
        public object ProcessQuery(DataManagerRequest queries);

        /// <summary>
        /// Process the data operation response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data manager instance.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public Task<object> ProcessResponse<T>(object data, DataManagerRequest queries);

        /// <summary>
        /// Process the CRUD operation response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data manager instance.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public Task<object> ProcessCrudResponse<T>(object data, DataManagerRequest queries);

        /// <summary>
        /// Performs data operation. If its a remote data source then make a server request.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public Task<object> PerformDataOperation<T>(object queries);

        /// <summary>
        /// Returns true if data source is remote service.
        /// </summary>
        /// <returns>bool.</returns>
        public bool IsRemote();

        /// <summary>
        /// To get model type.
        /// </summary>
        public void SetModelType(Type type);

        /// <summary>
        /// To get model type.
        /// </summary>
        public Type GetModelType();
        
        /// <summary>
        /// Handles the new item add operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="data">New item to be added.</param>
        /// <param name="tableName">Table name to insert new item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="position">Position to insert the new item.</param>
        /// <returns>object.</returns>
        public object Insert(DataManager dataManager, object data, string tableName = null, Query query = null, int position = 0);

        /// <summary>
        /// Handles the item update operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="data">Specifies the updated record.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending changed items alone to the server.</param>
        /// <param name="updateProperties">Specifies the field names to be updated.</param>
        /// <returns>object.</returns>
        public object Update(DataManager dataManager, string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null);

        /// <summary>
        /// Handles the remove operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="value">Specifies the primary key field value.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <returns>object.</returns>
        public object Remove(DataManager dataManager, string keyField, object value, string tableName = null, Query query = null);

        /// <summary>
        /// Handles the batch update operation. Enables user to perform add, update and remove items from data source in a single request.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="changed">Specifies the changed records.</param>
        /// <param name="added">Specifies the added records.</param>
        /// <param name="deleted">Specifies the deleted records.</param>
        /// <param name="e">Specifies the url and its key.</param>
        /// <param name="keyField">Specifies the primary key field.</param>
        /// <param name="dropIndex">Specifies the record position, from which new records will be added.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        public object BatchUpdate(DataManager dataManager, object changed, object added, object deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null);

        /// <summary>
        /// Handles the new item add operation in dynamic objects.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="data">New item to be added.</param>
        /// <param name="tableName">Table name to insert new item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="position">Position to insert the new item.</param>
        /// <returns>object.</returns>
        public object Insert(DataManager dataManager, IDynamicMetaObjectProvider data, string tableName = null, Query query = null, int position = 0);

        /// <summary>
        /// Handles the item update operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="data">Specifies the updated record.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        public object Update(DataManager dataManager, string keyField, IDynamicMetaObjectProvider data, string tableName = null, Query query = null, object original = null);

        /// <summary>
        /// Handles the batch update operation for Dynamic objects. Enables user to perform add, update and remove items from data source in a single request.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="changed">Specifies the changed records.</param>
        /// <param name="added">Specifies the added records.</param>
        /// <param name="deleted">Specifies the deleted records.</param>
        /// <param name="e">Specifies the url and its key.</param>
        /// <param name="keyField">Specifies the primary key field.</param>
        /// <param name="dropIndex">Specifies the record position, from which new records will be added.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        public object BatchUpdate(DataManager dataManager, List<IDynamicMetaObjectProvider> changed, List<IDynamicMetaObjectProvider> added, List<IDynamicMetaObjectProvider> deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null);

        /// <summary>
        /// Adds additional paramerters from Query instance to server request.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="queries"></param>
        public void AddParams(RequestOptions options, DataManagerRequest queries);

        /// <summary>
        /// Invoked before sending server request.
        /// </summary>
        /// <param name="request">Specifies the HttpRequestMessage instance.</param>
        public void BeforeSend(HttpRequestMessage request);

        /// <summary>
        /// Process the data operation batch response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public Task<object> ProcessBatchResponse<T>(object data, DataManagerRequest queries);
    }

    /// <summary>
    /// Base class for all data adaptors.
    /// </summary>
    public class AdaptorBase : IAdaptor
    {
        public AdaptorBase(DataManager dataManager) => DataManager = dataManager;

        /// <summary>
        /// Specifies the data manager instance.
        /// </summary>
        public DataManager DataManager { get; set; }

        /// <summary>
        /// When true, runs data operation synchronously. Applicable only for BlazorAdaptor.
        /// </summary>
        public bool RunSyncOnce { get; set; }
        /// <summary>
        /// Runs the data operation synchronously.
        /// </summary>
        /// <param name="runSync">Enables synchronous data operation.</param>
        public virtual void SetRunSyncOnce(bool runSync) => RunSyncOnce = false;

        /// <summary>
        /// Returns the adaptor name.
        /// </summary>
        /// <returns>string.</returns>
        public virtual string GetName() => nameof(AdaptorBase);

        /// <summary>
        /// Returns true if data source is remote service.
        /// </summary>
        /// <returns>bool.</returns>
        public virtual bool IsRemote() => false;

        /// <summary>
        /// To get model type.
        /// </summary>
        public virtual void SetModelType(Type type)
        {
        }

        /// <summary>
        /// To get model type.
        /// </summary>
        public virtual Type GetModelType() => default;

        /// <summary>
        /// Read query from <see cref="Syncfusion.Blazor.Data.Query"/> and make it understandable by
        /// data source.
        /// </summary>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>object.</returns>
        public virtual object ProcessQuery(DataManagerRequest queries) => default;

        /// <summary>
        /// Performs data operation. If its a remote data source then make a server request.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public virtual async Task<object> PerformDataOperation<T>(object queries) => await Task.FromResult<object>(null);

        /// <summary>
        /// Process the data operation response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data manager instance.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public virtual async Task<object> ProcessResponse<T>(object data, DataManagerRequest queries) => await Task.FromResult<object>(data);

        /// <summary>
        /// Process the CRUD operation response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data manager instance.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public virtual async Task<object> ProcessCrudResponse<T>(object data, DataManagerRequest queries) => await Task.FromResult<object>(data);

        /// <summary>
        /// Handles the new item add operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="data">New item to be added.</param>
        /// <param name="tableName">Table name to insert new item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="position">Position to insert the new item.</param>
        /// <returns>object.</returns>
        public virtual object Insert(DataManager dataManager, object data, string tableName = null, Query query = null, int position = 0) => data;

        /// <summary>
        /// Handles the item update operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="data">Specifies the updated record.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <param name="updateProperties">Specifies the field names to be updated.</param>
        /// <returns>object.</returns>
        public virtual object Update(DataManager dataManager, string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null) => data;

        /// <summary>
        /// Handles the remove operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="value">Specifies the primary key field value.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <returns>object.</returns>
        public virtual object Remove(DataManager dataManager, string keyField, object value, string tableName = null, Query query = null) => value;

        /// <summary>
        /// Handles the batch update operation. Enables user to perform add, update and remove items from data source in a single request.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="changed">Specifies the changed records.</param>
        /// <param name="added">Specifies the added records.</param>
        /// <param name="deleted">Specifies the deleted records.</param>
        /// <param name="e">Specifies the url and its key.</param>
        /// <param name="keyField">Specifies the primary key field.</param>
        /// <param name="dropIndex">Specifies the record position, from which new records will be added.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        public virtual object BatchUpdate(DataManager dataManager, object changed, object added, object deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null) => new { changed, added, deleted };

        /// <summary>
        /// Handles the new item add operation in dynamic objects.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="data">New item to be added.</param>
        /// <param name="tableName">Table name to insert new item.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="position">Position to insert the new item.</param>
        /// <returns>object.</returns>
        public virtual object Insert(DataManager dataManager, IDynamicMetaObjectProvider data, string tableName = null, Query query = null, int position = 0) => data;

        /// <summary>
        /// Handles the item update operation.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="keyField">Specifies the primary key.</param>
        /// <param name="data">Specifies the updated record.</param>
        /// <param name="tableName">Table name of the update item.</param>
        /// <param name="query">Query instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        public virtual object Update(DataManager dataManager, string keyField, IDynamicMetaObjectProvider data, string tableName = null, Query query = null, object original = null) => data;

        /// <summary>
        /// Handles the batch update operation for Dynamic objects. Enables user to perform add, update and remove items from data source in a single request.
        /// </summary>
        /// <param name="dataManager">DataManager instance.</param>
        /// <param name="changed">Specifies the changed records.</param>
        /// <param name="added">Specifies the added records.</param>
        /// <param name="deleted">Specifies the deleted records.</param>
        /// <param name="e">Specifies the url and its key.</param>
        /// <param name="keyField">Specifies the primary key field.</param>
        /// <param name="dropIndex">Specifies the record position, from which new records will be added.</param>
        /// <param name="query">Query class instance.</param>
        /// <param name="original">Specifies the original data. Uses this original data for sending change items alone to the server.</param>
        /// <returns>object.</returns>
        public virtual object BatchUpdate(DataManager dataManager, List<IDynamicMetaObjectProvider> changed, List<IDynamicMetaObjectProvider> added, List<IDynamicMetaObjectProvider> deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null) => new { changed, added, deleted };

        /// <summary>
        /// Adds additional paramerters from Query instance to server request.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="queries"></param>
        public virtual void AddParams(RequestOptions options, DataManagerRequest queries)
        {
        }

        /// <summary>
        /// Invoked before sending server request.
        /// </summary>
        /// <param name="request">Specifies the HttpRequestMessage instance.</param>
        public virtual void BeforeSend(HttpRequestMessage request)
        {
        }

        /// <summary>
        /// Process the data operation batch response/result from the data source and make it understandable by user end.
        /// </summary>
        /// <typeparam name="T">Specifies the data source model type.</typeparam>
        /// <param name="data">Specifies the data.</param>
        /// <param name="queries">Specifies the query.</param>
        /// <returns>Task.</returns>
        public virtual async Task<object> ProcessBatchResponse<T>(object data, DataManagerRequest queries) => await Task.FromResult<object>(data);
    }

    /// <summary>
    /// Defines internal adaptor options.
    /// </summary>
    /// <exclude/>
    public struct RemoteOptions: IEquatable<RemoteOptions>
    {
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the request type.
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// Gets or sets the sort field name.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets the select field name.
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// Gets or sets the records to skip.
        /// </summary>
        public string Skip { get; set; }

        /// <summary>
        /// Gets or sets the group criteria.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the records to take.
        /// </summary>
        public string Take { get; set; }

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// Gets or sets the filter criteria.
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// Gets or sets the aggregate details.
        /// </summary>
        public string Aggregates { get; set; }

        /// <summary>
        /// Gets or sets the navigation table.
        /// </summary>
        public string Expand { get; set; }

        /// <summary>
        /// Gets or sets the accept type.
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// Gets or sets the multipart type.
        /// </summary>
        public string MultipartAccept { get; set; }

        /// <summary>
        /// Gets or sets the batch value.
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// Gets or sets the change set.
        /// </summary>
        public string ChangeSet { get; set; }

        /// <summary>
        /// Gets or sets the batch prefix.
        /// </summary>
        public string BatchPre { get; set; }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// Gets or sets the batch content.
        /// </summary>
        public string BatchContent { get; set; }

        /// <summary>
        /// Gets or sets the change set.
        /// </summary>
        public string ChangeSetContent { get; set; }

        /// <summary>
        /// Gets or sets the batch change set.
        /// </summary>
        public string BatchChangeSetContentType { get; set; }

        /// <summary>
        /// Gets or sets the http update type.
        /// </summary>
        public HttpMethod UpdateType { get; set; }

        /// <summary>
        /// Enables local time conversion.
        /// </summary>
        public bool LocalTime { get; set; }

        /// <summary>
        /// Gets or sets the odatav4 $apply value.
        /// </summary>
        public string Apply { get; set; }

        /// <summary>
        /// Enable odatav4 search by $filter.
        /// </summary>
        public bool EnableODataSearchFallback { get; set; }

        /// <summary>
        /// Compares the specified instance and the current instance of RemoteOptions
        ///     for value equality.
        /// </summary>
        /// <param name="obj">The instance to compare.</param>
        /// <returns>true.</returns>
        public override bool Equals(object obj)
        {
            return true;
        }
        /// <summary>
        /// Compares the specified instance and the current instance of RemoteOptions
        ///     for value equality.
        /// </summary>
        /// <param name="other">The instance to compare.</param>
        /// <returns>true.</returns>
        public bool Equals(RemoteOptions other) => true;

        /// <summary>
        /// Returns the hash code.
        /// </summary>
        /// <returns>int.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Handles equal
        /// </summary>
        /// <param name="point1">argument one</param>
        /// <param name="point2">argument two</param>
        /// <returns>bool</returns>
        public static bool operator ==(RemoteOptions point1, RemoteOptions point2)
        {
            return point1.Equals(point2);
        }
        /// <summary>
        /// Handles unequal
        /// </summary>
        /// <param name="point1">argument one</param>
        /// <param name="point2">argument two</param>
        /// <returns>bool</returns>
        public static bool operator !=(RemoteOptions point1, RemoteOptions point2)
        {
            return !point1.Equals(point2);
        }
    }

    /// <summary>
    /// Defines the members of the CRUD arguments send during server request. Use this class to model
    /// bind request parameters while using UrlAdaptor.
    /// </summary>
    /// <typeparam name="T">Type of the data.</typeparam>
    public class CRUDModel<T>
    {
        public CRUDModel()
        {
        }

        /// <summary>
        /// Specifies the action initiated the request. Possible values are add, update or remove.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Specifies the Table name(if any) to be updated.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// Specifies the primary key column name.
        /// </summary>
        public string KeyColumn { get; set; }

        /// <summary>
        /// Specifies the Primary key value.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Specifies the modified/added record. For batch operation use Added, Changed and Deleted property.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Specifies the list of added records while batch editing.
        /// </summary>
        /// <remarks>The Added property will holds values on batch editing only.</remarks>
        public List<T> Added { get; set; }

        /// <summary>
        /// Specifies the list of updated records while batch editing.
        /// </summary>
        /// <remarks>The Changed property will holds values on batch editing only.</remarks>
        public List<T> Changed { get; set; }

        /// <summary>
        /// Specifies the list of deleted records while batch editing.
        /// </summary>
        /// <remarks>The Deleted property will holds values on batch editing only.</remarks>
        public List<T> Deleted { get; set; }

        /// <summary>
        /// Holds the additional parameters passed.
        /// </summary>
        public IDictionary<string, object> Params { get; set; }
    }

    /// <summary>
    /// Class holds URL and Key for batch operation.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Specifies the batch url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Specifies the batch primary key value.
        /// </summary>
        public string Key { get; set; }
    }

    /// <summary>
    /// Defines members of the request option for remote data handling.
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// Specifies the service url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Specifies the application base url.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Specifies the Http request method.
        /// </summary>
        public HttpMethod RequestMethod { get; set; }

        /// <summary>
        /// Specifies the data to be posted.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Specifies the source query value.
        /// </summary>
        public Query Queries { get; set; }

        /// <summary>
        /// Specifies the content type. By default, application/json is used.
        /// </summary>
        public string ContentType { get; set; } = "application/json";

        internal PvtOptions PvtData { get; set; }

        internal object Original { get; set; }

        internal Func<object[], int, string, string> UrlFunc { get; set; }

        internal Func<object[], int, string> DataFunc { get; set; }

        internal string CSet { get; set; }

        internal string keyField { get; set; }

        internal string Accept { get; set; }

        internal HttpMethod UpdateType { get; set; }
    }
}

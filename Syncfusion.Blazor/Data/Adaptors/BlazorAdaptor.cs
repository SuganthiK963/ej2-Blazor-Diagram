using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Data
{
#pragma warning disable BL0005
    /// <summary>
    /// Handles data operation in IEnumerable data source.
    /// </summary>
    public class BlazorAdaptor : AdaptorBase
    {
        public BlazorAdaptor(DataManager dataManager)
            : base(dataManager)
        {
        }

        public override string GetName() => nameof(BlazorAdaptor);

        public override void SetRunSyncOnce(bool runSync) => RunSyncOnce = runSync;

        public override object ProcessQuery(DataManagerRequest queries)
        {
            return SfBaseUtils.ChangeType(queries, typeof(DataManagerRequest));
        }
        public async override Task<object> PerformDataOperation<T>(object queries)
        {
            IEnumerable DataSource = DataManager.Json; //Component data source should be propagated here.            
            DataManagerRequest query = (DataManagerRequest)SfBaseUtils.ChangeType(queries, typeof(DataManagerRequest));
            DataResult DataObject;
            /*
             * Using Task.Run will start a new thread and UI flashing will happens.
             * https://github.com/dotnet/aspnetcore/issues/15266
             */
            if (!RunSyncOnce)
            {
                return await Task.Run(() => 
                {
                    DataObject = DataOperationInvoke<T>(DataSource, query);
                    return query.RequiresCounts ? DataObject : (object)DataObject.Result;
                });
            }
            else
            {
                RunSyncOnce = false;
                DataObject = DataOperationInvoke<T>(DataSource, query);
                return await Task.FromResult<object>(query.RequiresCounts ? DataObject : (object)DataObject.Result);
            }
        }

        /// <summary>
        /// Performs data operation.
        /// </summary>
        /// <typeparam name="T">Type of the data source item.</typeparam>
        /// <param name="DataSource">Data source value.</param>
        /// <param name="queries">Query to be processed.</param>
        /// <returns>DataResult.</returns>
        public DataResult DataOperationInvoke<T>(
            IEnumerable DataSource, DataManagerRequest queries)
        {
            DataResult DataObject = new DataResult();
            
            if (queries == null) { throw new ArgumentNullException(nameof(queries)); }

            if (DataSource == null || !DataSource.Cast<object>().Any())
            {
                DataObject.Result = DataSource;
                return DataObject;
            }

            if (DataSource.GetType() == typeof(List<ExpandoObject>) || typeof(T) == typeof(ExpandoObject) || DataSource.GetElementType() == typeof(ExpandoObject) || typeof(IDynamicMetaObjectProvider).IsAssignableFrom(DataSource.GetElementType()?.BaseType))
            {
                DataSource = DynamicObjectOperation.PerformDataOperations(DataSource, queries);
            }
            else
            {
                if (queries.Where?.Count > 0) // perform Filtering
                {
                    DataSource = DataOperations.PerformFiltering(DataSource, queries.Where, queries.Where[0].Operator);
                }

                if (queries.Search?.Count > 0) // perform Searching
                {
                    DataSource = DataOperations.PerformSearching(DataSource, queries.Search);
                }

                if (queries.Sorted?.Count > 0) // perform Sorting
                {
                    DataSource = DataOperations.PerformSorting(DataSource, queries.Sorted);
                }

                if (queries.RequiresFilteredRecords)
                {
                    DataObject.FilteredRecords = DataSource.Cast<object>().ToList();
                }
            }

            DataObject.Count = DataSource.Cast<object>().Count();
            IEnumerable AggregateData = DataSource;
            if (queries.Skip != 0) // perform Paging
            {
                DataSource = DataOperations.PerformSkip(DataSource, queries.Skip);
            }

            if (queries.Take != 0)
            {
                DataSource = DataOperations.PerformTake(DataSource, queries.Take);
            }

            if (queries.IdMapping != null && queries.Where != null)
            {
                DataSource = CollectChildRecords(DataSource, queries);
            }

            if (queries.Aggregates?.Count > 0)
            {
                DataObject.Aggregates = DataUtil.PerformAggregation(AggregateData, queries.Aggregates);
            }

            if (queries.Group?.Count > 0 && queries.ServerSideGroup)
            {
                DataObject = GroupResult<T>(queries, DataSource, DataObject);
            }
            else
            {
                DataObject.Result = DataSource.Cast<object>().ToList();
            }

            return DataObject;
        }

        public static DataResult GroupResult<T>(DataManagerRequest queries, IEnumerable DataSource, DataResult DataObject)
        {
            if (queries == null) { throw new ArgumentNullException(nameof(queries)); }
            if (DataObject == null) { throw new ArgumentNullException(nameof(DataObject)); }
            if (!queries.LazyLoad)
            {
                foreach (var group in queries.Group)
                {
                    DataSource = DataUtil.Group<T>(DataSource, group, queries.Aggregates, 0, queries.GroupByFormatter, queries.LazyLoad, queries.LazyExpandAllGroup);
                }
            }
            else if (queries.LazyLoad)
            {
                DataSource = DataUtil.Group<T>(DataSource, queries.Group[0], queries.Aggregates, 0, queries.GroupByFormatter, queries.LazyLoad, queries.LazyExpandAllGroup);
                DataObject.Count = DataSource.Cast<object>().Count();
            }
            DataObject.Result = DataSource;
            return DataObject;
        }

        /// <summary>
        /// Performs data operation on child records.
        /// </summary>
        /// <param name="datasource">Data source value.</param>
        /// <param name="dm">Query to be processed.</param>
        /// <returns>IEnumerable.</returns>
        public IEnumerable CollectChildRecords(IEnumerable datasource, DataManagerRequest dm)
        {
            if (datasource == null || dm == null) { return null; }
            var data = SfBaseUtils.ChangeType(datasource, datasource.GetType());
            IEnumerable DataSource = (IEnumerable)data;
            string IdMapping = dm.IdMapping;
            object[] TaskIds = Array.Empty<object>();
            if (DataSource.GetType() == typeof(List<ExpandoObject>) || DataSource.GetElementType() == typeof(ExpandoObject))
            {
                foreach (var rec in datasource.Cast<ExpandoObject>().ToList())
                {
                    IDictionary<string, object> propertyValues = rec;
                    object taskid = propertyValues[IdMapping];
                    TaskIds = TaskIds.Concat(new object[] { taskid }).ToArray();
                }
            }
            else
            {
                foreach (var rec in datasource)
                {
                    object taskid = rec.GetType().GetProperty(IdMapping).GetValue(rec);
                    TaskIds = TaskIds.Concat(new object[] { taskid }).ToArray();
                }
            }

            IEnumerable ChildRecords = null;
            IEnumerable records = null;
            foreach (object id in TaskIds)
            {
                dm.Where[0].value = id;
                if (DataSource.GetType() == typeof(List<ExpandoObject>) || DataSource.GetElementType() == typeof(ExpandoObject))
                {
                    records = DynamicObjectOperation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
                }
                else
                {
                    records = DataOperations.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
                }

                ChildRecords = ChildRecords == null || (ChildRecords.AsQueryable().Count() == 0) ? records : ((IEnumerable<object>)ChildRecords).Concat((IEnumerable<object>)records);
            }

            if (ChildRecords != null)
            {
                ChildRecords = CollectChildRecords(ChildRecords, dm);
                if (dm.Sorted != null && dm.Sorted.Count > 0) // perform Sorting
                {
                    if (DataSource.GetType() == typeof(List<ExpandoObject>) || DataSource.GetElementType() == typeof(ExpandoObject))
                    {
                        ChildRecords = DynamicObjectOperation.PerformSorting(ChildRecords.AsQueryable(), dm.Sorted);
                    }
                    else
                    {
                        ChildRecords = DataOperations.PerformSorting(ChildRecords, dm.Sorted);
                    }
                }

                datasource = ((IEnumerable<object>)datasource).Concat((IEnumerable<object>)ChildRecords);
            }

            return datasource;
        }

        public async override Task<object> ProcessResponse<T>(object data, DataManagerRequest queries)
        {
            if (queries != null && queries.RequiresCounts)
            {
                if (queries.Group?.Count > 0)
                {
                    return await Task.FromResult<DataResult<object>>(data as DataResult<object>);
                }

                return await Task.FromResult<DataResult<object>>(data as DataResult<object>);
            }
            else
            {
                if (queries?.Group?.Count > 0)
                {
                    if (queries.LazyLoad)
                    {
                        return await Task.FromResult<Group<T>>((Group<T>)((IEnumerable)data).Cast<Group<T>>());
                    }
                    else
                    {
                        return await Task.FromResult<List<Group<T>>>(((IEnumerable)data).Cast<Group<T>>().ToList());
                    }
                }

                return await Task.FromResult<List<T>>(((IEnumerable)data).Cast<T>().ToList());
            }
        }

        public override object Insert(DataManager dataManager, object data, string tableName = null, Query query = null, int position = 0)
        {
            IEnumerable dataSource = DataManager.Json ?? Enumerable.Empty<object>().ToList();
            ((IList)dataSource).Insert(position, data);
            return data;
        }

        public override object Update(DataManager dataManager, string keyField, object data, string tableName = null, Query query = null, object original = null, IDictionary<string, object> updateProperties = null)
        {
            IEnumerable dataSource = DataManager.Json;  
            string updatedKey = DataUtil.GetObject(keyField, data ?? null).ToString();

            foreach (var item in (IList)dataSource)
            {
                string oldKey = DataUtil.GetObject(keyField, item).ToString();

                if (oldKey == updatedKey)
                {
                    foreach (var property in data.GetType().GetProperties())
                    {
                        if (property.SetMethod != null && (updateProperties != null ? updateProperties.ContainsKey(property.Name) : true))
                        {
                            item.GetType().GetProperty(property.Name).SetValue(item, property.GetValue(data));
                        }
                    }

                    data = item;
                }
            }

            return data;
        }

        public override object Remove(DataManager dataManager, string keyField, object value, string tableName = null, Query query = null)
        {
            IEnumerable data = DataManager.Json;
            var result = (data as IEnumerable).Cast<object>().ToList();

            foreach (var item in result)
            {
                string oldKey = null;
                if (item.GetType() == typeof(ExpandoObject))
                {
                    IDictionary<string, object> propertyValues = (ExpandoObject)item;
                    oldKey = DataUtil.GetObject(keyField, propertyValues).ToString();
                }
                else if (item.GetType().BaseType == typeof(DynamicObject))
                {
                    oldKey = DataUtil.GetDynamicValue(item as DynamicObject, keyField).ToString();
                }
                else
                {
                    oldKey = DataUtil.GetObject(keyField, item).ToString();
                }
                if (oldKey == value?.ToString())
                {
                    (data as IList).Remove(item);
                }
            }

            return value;
        }

        public override object BatchUpdate(DataManager dataManager, object changed, object added, object deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null)
        {
            var data = DataManager.Json ?? Enumerable.Empty<object>().ToList();
            object changedRecords = null;
            object addedRecords = null;
            object deletedRecords = null;
            List<object> originalUpdatedRecords = new List<object>();
            string oldKey = null;

            /////Update Operation

            if (changed != null)
            {
                changedRecords = changed;
                foreach (var rec in (IEnumerable)changedRecords)
                {
                    foreach (var item in (IList)data)
                    {
                        oldKey = DataUtil.GetObject(keyField, item).ToString();
                        string updatedKey = DataUtil.GetObject(keyField, rec).ToString();

                        if (oldKey == updatedKey)
                        {
                            foreach (var property in rec.GetType().GetProperties())
                            {
                                if (property.SetMethod != null)
                                {
                                    item.GetType().GetProperty(property.Name).SetValue(item, property.GetValue(rec));
                                }

                                originalUpdatedRecords.Add(item);
                            }
                        }
                    }
                }

                changedRecords = originalUpdatedRecords;
            }

            /////Insert Operation

            if (added != null)
            {
                addedRecords = added;

                foreach (var newrec in (IEnumerable)addedRecords)
                {
                    if (dropIndex == null)
                    {
                        ((IList)data).Add(newrec);
                    }
                    else
                    {
                        ((IList)data).Insert((int)dropIndex, newrec);
                        dropIndex++;
                    }
                }
            }

            /////Delete Operation

            if (deleted != null)
            {
                deletedRecords = deleted;
                var result = (data as IEnumerable).Cast<object>().ToList();
                foreach (var delrec in (IEnumerable)deletedRecords)
                {
                    foreach (var item in result)
                    {
                        oldKey = item.GetType().GetProperty(keyField).GetValue(item).ToString();
                        string deletedKey = delrec.GetType().GetProperty(keyField).GetValue(delrec).ToString();
                        if (oldKey == deletedKey)
                        {
                            ((IList)data).Remove(item);
                        }
                    }
                }
            }

            return new { changedRecords, addedRecords, deletedRecords };
        }

        public static object BatchUpdateArray<T>(DataManager dataManager, object changed, object added, object deleted, string keyField, int? dropIndex)
        {
            if (dataManager == null) { return new { changedRecords = changed, addedRecords = added, deletedRecords = deleted }; }
            IEnumerable data = dataManager.Json ?? Enumerable.Empty<object>().ToList();
            data = (data as IEnumerable<T>).ToList();
            object changedRecords = null;
            object addedRecords = null;
            object deletedRecords = null;
            List<object> originalUpdatedRecords = new List<object>();
            string oldKey = null;

            /////Update Operation

            if (changed != null)
            {
                changedRecords = changed;
                foreach (var rec in (IEnumerable)changedRecords)
                {
                    foreach (var item in (IList)data)
                    {
                        oldKey = item.GetType().GetProperty(keyField).GetValue(item).ToString();
                        string updatedKey = rec.GetType().GetProperty(keyField).GetValue(rec).ToString();
                        if (oldKey == updatedKey)
                        {
                            foreach (var property in rec.GetType().GetProperties())
                            {
                                if (property.SetMethod != null)
                                {
                                    item.GetType().GetProperty(property.Name).SetValue(item, property.GetValue(rec));
                                }

                                originalUpdatedRecords.Add(item);
                            }
                        }
                    }
                }

                changedRecords = originalUpdatedRecords;
            }

            /////Insert Operation

            if (added != null)
            {
                addedRecords = added;

                foreach (var newrec in (IEnumerable)addedRecords)
                {
                    if (dropIndex == null)
                    {
                        ((IList)data).Add(newrec);
                    }
                    else
                    {
                        ((IList)data).Insert((int)dropIndex, newrec);
                        dropIndex++;
                    }
                }
            }

            /////Delete Operation

            if (deleted != null)
            {
                deletedRecords = deleted;
                var result = (data as IEnumerable).Cast<object>().ToList();
                foreach (var delrec in (IEnumerable)deletedRecords)
                {
                    foreach (var item in result)
                    {
                        oldKey = item.GetType().GetProperty(keyField).GetValue(item).ToString();
                        string deletedKey = delrec.GetType().GetProperty(keyField).GetValue(delrec).ToString();
                        if (oldKey == deletedKey)
                        {
                            ((IList)data).Remove(item);
                        }
                    }
                }
            }

            data = (data as IEnumerable<T>).ToArray();
            dataManager.Json = data as IEnumerable<object>;
            return new { changedRecords, addedRecords, deletedRecords };
        }

        public static object InsertArray<T>(DataManager dataManager, object data, int position)
        {
            if (dataManager == null) { return data; }
            IEnumerable dataSource = dataManager.Json ?? Enumerable.Empty<object>().ToList();
            dataSource = (dataSource as IEnumerable<T>).ToList();
            ((IList)dataSource).Insert(position, data);
            dataSource = (dataSource as IEnumerable<T>).ToArray();
            dataManager.Json = dataSource as IEnumerable<object>;
            return data;
        }
        
        public static object RemoveArray<T>(DataManager dataManager, string keyField, object value)
        {
            if (dataManager == null) { return value; }
            IEnumerable data = dataManager.Json;
            data = (data as IEnumerable<T>).ToList();
            var result = (data as IEnumerable).Cast<object>().ToList();
            foreach (var item in result)
            {
                string oldKey = null;
                oldKey = item.GetType().GetProperty(keyField).GetValue(item).ToString();
                if (oldKey == value?.ToString())
                {
                    {
                        (data as IList).Remove(item);
                        data = (data as IEnumerable<T>).ToArray();
                        dataManager.Json = data as IEnumerable<object>;
                    }
                }
            }

            return value;
        }

        public override object Insert(DataManager dataManager, IDynamicMetaObjectProvider data, string tableName = null, Query query = null, int position = 0)
        {
            IEnumerable dataSource = DataManager.Json ?? Enumerable.Empty<object>().ToList();
            IDictionary<string, Type> columnTypes = DataUtil.GetColumnType(dataSource, false);
            bool isDynamicObject = typeof(DynamicObject).IsAssignableFrom((dataSource as IEnumerable).Cast<object>().ToList().FirstOrDefault()?.GetType());
            if ((dataSource as IEnumerable<object>).Any())
            {
                if (isDynamicObject)
                {
                    var dataMembers = (data as DynamicObject)?.GetDynamicMemberNames().ToArray();
                    foreach (var property in dataMembers)
                    {
                        var oldData = (dataSource as IEnumerable).Cast<object>().ToList().FirstOrDefault();
                        var newValue = DataUtil.GetDynamicValue(data as DynamicObject, property);
                        var oldValue = DataUtil.GetDynamicValue(oldData as DynamicObject, property);
                        if (newValue != null && oldValue != null)
                        {
                            (data as DynamicObject).TrySetMember(new DataSetMemberBinder(property, false), SfBaseUtils.ChangeType(newValue, oldValue.GetType()));
                        }
                        else
                        {
                            (data as DynamicObject).TrySetMember(new DataSetMemberBinder(property, false), newValue);
                        }
                    }
                }
                else
                {
                    IDictionary<string, object> propertyValues = (ExpandoObject)data;
                    foreach (var property in propertyValues?.Keys.ToList())
                    {
                        if (columnTypes.ContainsKey(property) && propertyValues[property] != null)
                        {
                            Type type = columnTypes[property];
                            ((IDictionary<string, object>)data)[property] = SfBaseUtils.ChangeType(propertyValues[property], type);
                        }
                        else
                        {
                            ((IDictionary<string, object>)data)[property] = propertyValues[property];
                        }
                    }
                }
            }

            ((IList)dataSource).Insert(position, data);
            return data?.ToString();
        }

        public override object Update(DataManager dataManager, string keyField, IDynamicMetaObjectProvider data, string tableName = null, Query query = null, object original = null)
        {
            IEnumerable dataSource = DataManager.Json;
            IDictionary<string, Type> columnTypes = null;
            string updatedKey = null;
            columnTypes = DataUtil.GetColumnType(dataSource, false);
            var isDynamicObject = data?.GetType()?.BaseType == typeof(DynamicObject);
            if (isDynamicObject)
            {
                updatedKey = DataUtil.GetDynamicValue(data as DynamicObject, keyField).ToString();
            }
            else
            {
                IDictionary<string, object> updatedProps = (ExpandoObject)data;
                if (DataUtil.GetObject(keyField, updatedProps) != null)
                {
                    updatedKey = DataUtil.GetObject(keyField, updatedProps).ToString();
                }
            }

            foreach (var item in (IList)dataSource)
            {
                string oldKey = null;

                if (isDynamicObject)
                {
                    oldKey = DataUtil.GetDynamicValue(item as DynamicObject, keyField).ToString();
                }
                else
                {
                    IDictionary<string, object> oldProps = (ExpandoObject)item;
                    if (DataUtil.GetObject(keyField, oldProps) != null)
                    {
                        oldKey = DataUtil.GetObject(keyField, oldProps).ToString();
                    }
                }

                if (oldKey == updatedKey)
                {
                    if (isDynamicObject)
                    {
                        var dataMembers = (data as DynamicObject).GetDynamicMemberNames().ToArray();
                        foreach (var dataMember in dataMembers)
                        {
                            if (dataMember != keyField)
                            {
                                var newValue = DataUtil.GetDynamicValue(data as DynamicObject, dataMember);
                                var oldValue = DataUtil.GetDynamicValue(item as DynamicObject, dataMember);
                                if (newValue != null && oldValue != null)
                                {
                                    (item as DynamicObject).TrySetMember(new DataSetMemberBinder(dataMember, false), SfBaseUtils.ChangeType(newValue, oldValue.GetType()));
                                }
                                else
                                {
                                    (item as DynamicObject).TrySetMember(new DataSetMemberBinder(dataMember, false), newValue);
                                }
                            }
                        }
                    }
                    else
                    {
                        IDictionary<string, object> propertyValues = (ExpandoObject)data;
                        var propKeys = propertyValues.Keys.Count;
                        var propValues = propertyValues.Keys.ToList();
                        for (var i = 0; i < propKeys; i++)
                        {
                            string property = propValues[i];
                            if (((IDictionary<string, object>)item).ContainsKey(property))
                            {
                                if (propertyValues[property] != null)
                                {
                                    Type type = ((IDictionary<string, object>)item)[property] != null ? ((IDictionary<string, object>)item)[property].GetType() : columnTypes[property];
                                    if (type == propertyValues[property].GetType())
                                    {
                                        ((IDictionary<string, object>)item)[property] = propertyValues[property];
                                    }
                                    else
                                    {
                                        ((IDictionary<string, object>)item)[property] = SfBaseUtils.ChangeType(propertyValues[property], type);
                                    }
                                }
                                else
                                {
                                    ((IDictionary<string, object>)item)[property] = propertyValues[property];
                                }
                            }
                            else
                            {
                                ((IDictionary<string, object>)item).Add(property, propertyValues[property]);
                            }
                        }
                    }

                    data = (IDynamicMetaObjectProvider)item;
                }
            }

            return data;
        }

        private static List<object> BatchUpdateDynamic(object record, object item, string keyField, List<object> originalUpdatedRecords)
        {
            string updatedKey = DataUtil.GetDynamicValue(record as DynamicObject, keyField)?.ToString();
            string oldKey = DataUtil.GetDynamicValue(item as DynamicObject, keyField)?.ToString();
            var updatedProps = (item as DynamicObject).GetDynamicMemberNames().ToArray();
            if (oldKey == updatedKey)
            {
                foreach (var property in updatedProps)
                {
                    if (property != keyField && property != "BlazId")
                    {
                        var newValue = DataUtil.GetDynamicValue(record as DynamicObject, property);
                        var oldValue = DataUtil.GetDynamicValue(item as DynamicObject, property);
                        if (newValue != null && oldValue != null)
                        {
                            (item as DynamicObject).TrySetMember(new DataSetMemberBinder(property, false), SfBaseUtils.ChangeType(newValue, oldValue.GetType()));
                        }
                        else
                        {
                            (item as DynamicObject).TrySetMember(new DataSetMemberBinder(property, false), newValue);
                        }
                    }
                }
                originalUpdatedRecords.Add(item);
            }

            return originalUpdatedRecords;
        }

        private static List<object> BatchUpdateExpando(object record, object item, string keyField, List<object> originalUpdatedRecords, IDictionary<string, Type> columnTypes) 
        {
            IDictionary<string, object> updatedProps = (ExpandoObject)record;
            var updatedValue = DataUtil.GetObject(keyField, updatedProps);
            string updatedKey = null;
            string oldKey = null;
            if (updatedValue != null)
            {
                updatedKey = updatedValue.ToString();
            }

            IDictionary<string, object> oldProps = (ExpandoObject)item;
            var oldValue = DataUtil.GetObject(keyField, oldProps);
            if (oldValue != null)
            {
                oldKey = oldValue.ToString();
            }

            if (oldKey == updatedKey)
            {
                foreach (var property in updatedProps.Keys)
                {
                    if (((IDictionary<String, object>)item).ContainsKey(property))
                    {
                        if (updatedProps[property] != null)
                        {
                            Type type = ((IDictionary<String, object>)item)[property] != null ? ((IDictionary<String, object>)item)[property].GetType() : columnTypes[property];
                            if (type == updatedProps[property].GetType())
                            {
                                ((IDictionary<String, object>)item)[property] = updatedProps[property];
                            }
                            else
                            {
                                ((IDictionary<String, object>)item)[property] = SfBaseUtils.ChangeType(updatedProps[property], type);
                            }
                        }
                        else
                        {
                            ((IDictionary<String, object>)item)[property] = updatedProps[property];
                        }
                    }
                }

                originalUpdatedRecords.Add(item);
            }
            return originalUpdatedRecords;
        }

       
        public override object BatchUpdate(DataManager dataManager, List<IDynamicMetaObjectProvider> changed, List<IDynamicMetaObjectProvider> added, List<IDynamicMetaObjectProvider> deleted, Utils e, string keyField, int? dropIndex, Query query = null, object original = null)
        {
            var data = DataManager.Json ?? Enumerable.Empty<object>().ToList();
            object changedRecords = null;
            object addedRecords = null;
            object deletedRecords = null;
            List<object> originalUpdatedRecords = new List<object>();
            string oldKey = null;
            IDictionary<string, Type> columnTypes = DataUtil.GetColumnType((IEnumerable)data, false);
            /////Update Operation
            bool isDynamicObject = typeof(DynamicObject).IsAssignableFrom((data as IEnumerable).Cast<object>().ToList().FirstOrDefault()?.GetType());
            if (changed != null)
            {
                changedRecords = changed;

                foreach (var rec in (IEnumerable)changedRecords)
                {
                    foreach (var item in (IList)data)
                    {
                        if (isDynamicObject)
                        {
                            originalUpdatedRecords = BlazorAdaptor.BatchUpdateDynamic(rec, item, keyField, originalUpdatedRecords);
                        }
                        else
                        {
                            originalUpdatedRecords = BlazorAdaptor.BatchUpdateExpando(rec, item, keyField, originalUpdatedRecords, columnTypes);       
                        }
                    }
                }

                changedRecords = originalUpdatedRecords;
            }

            /////Insert Operation

            if (added != null)
            {
                addedRecords = added;
                foreach (var newrec in (IEnumerable)addedRecords)
                {
                    if (isDynamicObject)
                    {
                        var updatedProps = (newrec as DynamicObject).GetDynamicMemberNames().ToArray();
                        foreach (var property in updatedProps)
                        {
                            var oldData = (data as IEnumerable).Cast<object>().ToList().FirstOrDefault();
                            var newValue = DataUtil.GetDynamicValue(newrec as DynamicObject, property);
                            var oldValue = DataUtil.GetDynamicValue(oldData as DynamicObject, property);
                            if (newValue != null && oldValue != null)
                            {
                                (newrec as DynamicObject).TrySetMember(new DataSetMemberBinder(property, false), SfBaseUtils.ChangeType(newValue, oldValue.GetType()));
                            }
                            else
                            {
                                (newrec as DynamicObject).TrySetMember(new DataSetMemberBinder(property, false), newValue);
                            }
                        }
                    }
                    else
                    {
                        IDictionary<string, object> propertyValues = (ExpandoObject)newrec;
                        foreach (var property in propertyValues.Keys.ToList())
                        {
                            if (columnTypes != null && columnTypes.ContainsKey(property))
                            {
                                Type type = columnTypes[property];
                                if (propertyValues[property] != null)
                                {
                                    ((IDictionary<string, object>)newrec)[property] = SfBaseUtils.ChangeType(propertyValues[property], type);
                                }
                                else
                                {
                                    ((IDictionary<string, object>)newrec)[property] = propertyValues[property];
                                }
                            }
                        }
                    }

                    if (dropIndex == null)
                    {
                        ((IList)data).Add(newrec);
                    }
                    else
                    {
                        ((IList)data).Insert((int)dropIndex, newrec);
                        dropIndex++;
                    }
                }
            }

            /////Delete Operation

            if (deleted != null)
            {
                deletedRecords = deleted;
                var result = (data as IEnumerable).Cast<IDynamicMetaObjectProvider>().ToList();
                foreach (var delrec in (IEnumerable)deletedRecords)
                {
                    foreach (var item in result)
                    {
                        string deletedKey = null;
                        if (isDynamicObject)
                        {
                            oldKey = DataUtil.GetDynamicValue(item as DynamicObject, keyField)?.ToString();
                            deletedKey = DataUtil.GetDynamicValue(delrec as DynamicObject, keyField)?.ToString();
                        }
                        else
                        {
                            IDictionary<string, object> oldProps = (ExpandoObject)item;
                            oldKey = DataUtil.GetObject(keyField, oldProps).ToString();
                            IDictionary<string, object> updatedProps = (ExpandoObject)delrec;
                            deletedKey = DataUtil.GetObject(keyField, updatedProps).ToString();
                        }

                        if (oldKey == deletedKey)
                        {
                            (data as IList).Remove(item);
                        }
                    }
                }
            }

            return new { changedRecords, addedRecords, deletedRecords };
        }
    }
#pragma warning restore BL0005
}

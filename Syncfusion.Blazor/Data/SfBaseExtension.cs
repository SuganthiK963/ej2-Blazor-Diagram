using System;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using System.Runtime.CompilerServices;
using System.Globalization;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Diagrams")]

namespace Syncfusion.Blazor
{
#pragma warning disable CA1708 // Identifiers should differ by more than case
    public abstract class SfBaseExtension : SfBaseComponent, IBaseInit
#pragma warning restore CA1708 // Identifiers should differ by more than case
    {
        #region private properties

        /// <summary>
        /// This member is used only to prevent raising JS interop call for dependent controls (i.e. overview) before rendered the diagram.
        /// </summary>
        private static bool isDiagramRendered { get; set; }

        protected int uniqueId { get; set; }

        private List<string> directParamKeys = new List<string>();
        #endregion

        #region protected properties
#pragma warning disable CA1716 // Identifiers should not match keywords
        protected virtual string nameSpace { get; set; }

#pragma warning restore CA1716 // Identifiers should not match keywords
        protected virtual string jsProperty { get; set; }

        protected virtual SfBaseExtension mainParent { get; set; }

        protected virtual JSInteropAdaptor CreateJsAdaptor() => default;
        #endregion

        #region internal properties
        internal bool IsClientChanges { get; set; }

        internal bool IsEventTriggered { get; set; }

        internal bool IsAutoInitialized { get; set; }

        internal bool isObservableCollectionChanged { get; set; }

        internal List<object> InvokedEvents { get; set; } = new List<object>();

        internal List<string> ObservableChangedList { get; set; } = new List<string>();

        internal List<ScriptModules> DynamicScripts { get; set; } = new List<ScriptModules>();

        internal Dictionary<string, object> ObservableData = new Dictionary<string, object>();
        internal Dictionary<string, EventData> DelegateList = new Dictionary<string, EventData>();
        internal Dictionary<string, object> ChildDotNetObjectRef = new Dictionary<string, object>();

        internal Dictionary<string, object> ClientChanges { get; set; } = new Dictionary<string, object>();

        internal Dictionary<string, object> DirectParameters { get; set; } = new Dictionary<string, object>();

        [Inject]
        [JsonIgnore]
        internal ISyncfusionStringLocalizer Localizer { get; set; }

        [JsonIgnore]
        internal Dictionary<string, DataManager> DataManagerContainer { get; set; } = new Dictionary<string, DataManager>();
        #endregion

        #region public properties

        /// <exclude/>
        // Moved this property from internal to public for accessing in Syncfusion.Blazor.Core WASM.
        public string LocaleText { get; set; }

        /// <exclude/>
        public virtual string ID { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public virtual Type ModelType { get; set; }

        /// <exclude/>
        [JsonIgnore]
#pragma warning disable CA1721 // Property names should not match get methods
        public DataManager DataManager { get; set; }
#pragma warning restore CA1721 // Property names should not match get methods

        /// <exclude/>
        [JsonIgnore]
        public bool TemplateClientChanges { get; set; }

        /// <exclude/>
        [JsonProperty("guid")]
        public int UniqueId
        {
            get
            {
                Random random = new Random();
                if (uniqueId == 0)
                {
#pragma warning disable CA5394 // Do not use insecure randomness
                    uniqueId = random.Next(1, 100000);
#pragma warning restore CA5394 // Do not use insecure randomness
                    return uniqueId;
                }
                else
                {
                    return uniqueId;
                }
            }
        }

        /// <exclude/>
        [JsonIgnore]
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual Dictionary<string, object> DataContainer { get; set; } = new Dictionary<string, object>();

        /// <exclude/>
        [JsonIgnore]
        public virtual Dictionary<string, object> DataHashTable { get; set; } = new Dictionary<string, object>();
#pragma warning restore CA2227 // Collection properties should be read only
        #endregion

        #region life cycle methods
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Notify to the component for the required scripts loaded.
                await OnAfterScriptRendered();

                var tempDictionary = new Dictionary<string, object>();
                foreach (var key in directParamKeys)
                {
                    var initValue = GetType().GetProperty(key)?.GetValue(this);
                    SfBaseUtils.UpdateDictionary(key, initValue, tempDictionary);
                }

                DirectParameters = tempDictionary.ToDictionary(prop => prop.Key, prop => prop.Value);
                tempDictionary = null;
                IsRendered = false;
            }

            if (nameSpace != null)
            {
                if (firstRender && !IsClientChanges)
                {
                    if (nameSpace.Contains("Diagram", StringComparison.Ordinal))
                    {
                        isDiagramRendered = false;
                    }

                    await InitComponent();
                    if (nameSpace.Contains("Diagram", StringComparison.Ordinal))
                    {
                        isDiagramRendered = true;
                    }
                }
                else
                {
                    await DataBind();
                }
            }

            isObservableCollectionChanged = false;
            ObservableChangedList = new List<string>();
        }

        internal async Task<bool> IsDiagramScriptRendered()
        {
            if (isDiagramRendered || (nameSpace.Contains("Diagram", StringComparison.Ordinal) && IsRendered))
            {
                return true;
            }
            else
            {
                await Task.Delay(10);
                return await IsDiagramScriptRendered();
            }
        }

        #endregion

        internal async Task InitComponent()
        {
            // The below condition avoid to reinitialize the already rendered component through ResourceManager.
            // Some components may used StateHasChange at OnAfterRenderAsync which cause reinitializing the component again even after rendering firstRender.
            // This is applicable only for dynamic script loading
            if (IsRendered)
            {
                return;
            }
#if !NETSTANDARD
            // load init-interop script.
            await SfBaseUtils.ImportModule(JSRuntime, SfScriptModules.SfBase, SyncfusionService.ScriptHashKey);
            await OnInitRenderAsync();
#endif
#if NETSTANDARD
            if (SyncfusionService.IsScriptRendered)
            {
                await OnInitRenderAsync();
            }
            else
            {
                SyncfusionService.SfBaseExtensions.Add(this);
            }
#endif
        }

        /// <exclude/>
        public override Task SetParametersAsync(ParameterView parameters)
        {
            // parameters.SetParameterProperties(this);
            if (DirectParameters.Count == 0)
            {
                foreach (var parameter in parameters)
                {
                    if (!parameter.Cascading)
                    {
                        directParamKeys.Add(parameter.Name);
                    }
                }
            }

            return base.SetParametersAsync(parameters);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnInitRenderAsync()
        {
            await SfBaseUtils.ImportModule(JSRuntime, SfScriptModules.SfBaseExtended, SyncfusionService.ScriptHashKey);
            // load component specific dependency scripts.
            await SfBaseUtils.ImportModules(JSRuntime, DynamicScripts, SyncfusionService.ScriptHashKey);

            if (SyncfusionService.IsFirstBaseResource)
            {
                var currentCulture = Intl.CurrentCulture;
                SyncfusionService.IsFirstBaseResource = false;
                await SfBaseUtils.InvokeMethod(JSRuntime, "sfBlazor.loadCldr", GlobalizeJsonGenerator.GetGlobalizeJsonString(currentCulture));
                await SfBaseUtils.InvokeMethod(JSRuntime, "sfBlazor.setCulture", currentCulture.Name, Intl.GetCultureFormats(currentCulture.Name));
            }

            DotnetObjectReference = DotNetObjectReference.Create<object>(this);
            string bindableProps = SerialiazeBindableProp(PropertyChanges);
            var key = nameSpace + ".dataSource";
            if (DataContainer.ContainsKey(key) && DataContainer[key] != null)
            {
                SetDataHashTable(key, (IEnumerable)DataContainer[key]);
            }

            if (nameSpace.Contains("Overview", StringComparison.Ordinal))
            {
                await IsDiagramScriptRendered();
            }

            await Init<string>(JSRuntime, ID, GetUpdateModel(true), GetEventList(), nameSpace, DotnetObjectReference, bindableProps, null, ChildDotNetObjectRef, null, LocaleText);
            IsRendered = true;
            if (nameSpace.Contains("Diagram", StringComparison.Ordinal))
            {
                isDiagramRendered = true;
            }

            await OnAfterClientRendered(true);

            // set initial property changes in component create event
            if (DelegateList.ContainsKey("created") && PropertyChanges.Count > 0)
            {
                await DataBind();
            }

            PropertyChanges.Clear();
            TemplateClientChanges = false;
            isObservableCollectionChanged = false;
            ObservableChangedList = new List<string>();
            DynamicScripts.Clear();
        }

        internal virtual async Task OnAfterClientRendered(bool firstRender)
        {
            await Task.CompletedTask;
        }

        internal override void ComponentDispose()
        {
        }

        internal void CommonDispose()
        {
            DataManager?.Dispose();
            Localizer = null;
            mainParent = null;
            ClientChanges.Clear();
            InvokedEvents.Clear();
            directParamKeys.Clear();
            DirectParameters.Clear();
            ObservableData.Clear();
            DelegateList.Clear();
            DynamicScripts.Clear();
            DataContainer.Clear();
            DataManagerContainer.Clear();
            DataHashTable.Clear();
            ObservableChangedList.Clear();
            ChildDotNetObjectRef.Clear();
            UnWireObservableEvents();
        }

        public override void Dispose()
        {
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                CommonDispose();
                ComponentDispose();
                if (nameSpace != null && IsRendered)
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CA2012 // Use ValueTasks correctly
                    InvokeMethod<object>(JSRuntime, ID, "destroy", null, null, nameSpace);
#pragma warning restore CA2012 // Use ValueTasks correctly
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }

        public async Task Refresh()
        {
            if (nameSpace != null)
            {
                await InvokeMethod<object>(JSRuntime, ID, "refresh", null, null, nameSpace);
            }
        }

        /// <exclude/>
#pragma warning disable CA1801 // Review unused parameters
        private async Task DataBind(bool hasStateChanged = false)
#pragma warning restore CA1801 // Review unused parameters
        {
            IsEventTriggered = false;
            ClearClientChanges();
            if (IsRendered && nameSpace != null && PropertyChanges.Count > 0)
            {
                string bindableProps = SerialiazeBindableProp(PropertyChanges);
                PropertyChanges.Clear();
                await Update<object>(JSRuntime, ID, bindableProps, nameSpace);
            }
            else
            {
                PropertyChanges.Clear();
            }

            if (IsRendered)
            {
                DynamicScripts.Clear();
            }

            InvokedEvents = new List<object>();
        }

        protected void ClearClientChanges(bool clearBindables = false)
        {
            if (IsClientChanges || clearBindables)
            {
                foreach (var property in ClientChanges)
                {
                    if (PropertyChanges.ContainsKey(property.Key))
                    {
                        PropertyChanges.Remove(property.Key);
                    }
                }

                if (!clearBindables)
                {
                    IsClientChanges = false;
                    ClientChanges.Clear();
                }
            }
        }

#pragma warning disable CA1801 // Review unused parameters
        internal void SetDataHashTable(string key, IEnumerable DataSource)
#pragma warning restore CA1801 // Review unused parameters
        {
            foreach (var Data in DataSource)
            {
                System.Guid guid = System.Guid.NewGuid();
                DataHashTable.Add("BlazTempId_" + guid.ToString(), Data);
            }
        }

        internal static string ConvertJsonString(object jsonElement)
        {
            string jsonString = jsonElement.ToString();
#pragma warning disable CA1307 // Specify StringComparison
#pragma warning disable CA2249 // Consider using 'string.Contains' instead of 'string.IndexOf'
            if (jsonString.IndexOf(",", StringComparison.Ordinal) == -1)
            {
                jsonString = jsonString.Replace("\\", string.Empty);
            }

            if ((jsonString.IndexOf("\"[", StringComparison.Ordinal) != jsonString.IndexOf("\"[\\", StringComparison.Ordinal)) || (jsonString.IndexOf("\"{", StringComparison.Ordinal) != jsonString.IndexOf("\"{\\", StringComparison.Ordinal)))
            {
                if ((jsonString.LastIndexOf("\"{", StringComparison.Ordinal) > 0) && (jsonString.IndexOf(",", StringComparison.Ordinal) == -1))
                {
                    jsonString = jsonString.Replace("\"[", "[").Replace("]\"", "]")
                                .Replace("\"{", "{").Replace("}\"", "}");
                }
            }
#pragma warning restore CA1307 // Specify StringComparison
#pragma warning restore CA2249 // Consider using 'string.Contains' instead of 'string.IndexOf'
            return jsonString;
        }

        internal async Task InvokeMethod(string methodName, string moduleName = null, params object[] methodParams)
        {
            methodParams = (methodParams != null && methodParams.Length > 0) ? methodParams : null;
            await InvokeMethod<object>(JSRuntime, ID, methodName, moduleName, methodParams, nameSpace);
        }

        // Invoke object return type methods
        internal async Task<T> InvokeMethod<T>(string methodName, bool isObjectReturnType, string moduleName = null, params object[] methodParams)
        {
            methodParams = (methodParams != null && methodParams.Length > 0) ? methodParams : null;
            if (!isObjectReturnType)
            {
                return await InvokeMethod<T>(JSRuntime, ID, methodName, moduleName, methodParams, nameSpace);
            }
            else
            {
                string ReturnValue = await InvokeMethod<string>(JSRuntime, ID, methodName, moduleName, methodParams, nameSpace);
                var Settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if (ReturnValue == null)
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(ReturnValue, Settings);
                }
            }
        }

        private void ObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            isObservableCollectionChanged = true;
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)item).PropertyChanged -= ObservablePropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)item).PropertyChanged += ObservablePropertyChanged;
                    }
                }
            }
        }

        private void ObservablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            isObservableCollectionChanged = true;
        }

        protected virtual void WireObservableEvents(object collection)
        {
            if (collection != null && collection.GetType().IsGenericType)
            {
                if (collection is INotifyCollectionChanged)
                {
                    ((INotifyCollectionChanged)collection).CollectionChanged += new NotifyCollectionChangedEventHandler(ObservableCollectionChanged);
                }

                if (collection is INotifyPropertyChanged)
                {
                    List<object> enumerableCollection = new List<object>((IEnumerable<object>)collection);
                    var firstItem = enumerableCollection.FirstOrDefault();
                    if (firstItem is INotifyPropertyChanged)
                    {
                        foreach (var item in enumerableCollection)
                        {
                            ((INotifyPropertyChanged)item).PropertyChanged += new PropertyChangedEventHandler(ObservablePropertyChanged);
                        }
                    }
                }
            }
        }

        private void UnWireObservableEvents()
        {
            if (ObservableData.Count > 0)
            {
                foreach (var collection in ObservableData)
                {
                    if (collection.Value is INotifyCollectionChanged)
                    {
                        ((INotifyCollectionChanged)collection.Value).CollectionChanged -= ObservableCollectionChanged;
                    }

                    if (collection.Value is INotifyPropertyChanged)
                    {
                        List<object> enumerableCollection = new List<object>((IEnumerable<object>)collection.Value);
                        var firstItem = enumerableCollection.FirstOrDefault();
                        if (firstItem is INotifyPropertyChanged)
                        {
                            foreach (var item in enumerableCollection)
                            {
                                ((INotifyPropertyChanged)item).PropertyChanged -= ObservablePropertyChanged;
                            }
                        }
                    }
                }
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal async virtual Task<T> UpdateProperty<T>(string key, T publicValue, T privateValue, bool isDataSource = false, bool isObservable = false)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
#pragma warning disable CA1307 // Specify StringComparison
            var propertyKey = !jsProperty.StartsWith("sf.", StringComparison.Ordinal) ? $"{jsProperty}.{key}" : key;
#pragma warning restore CA1307 // Specify StringComparison
            var dataKey = !string.IsNullOrEmpty(jsProperty) ? $"{jsProperty}.{key}" : key;
            var baseComponent = mainParent != null ? mainParent : this;
            var subString = key.Substring(1);
#pragma warning disable CA1304 // Specify CultureInfo
            string publicKey = char.ToUpper(key[0]) + subString;
#pragma warning restore CA1304 // Specify CultureInfo

            T finalResult = publicValue;
            if (isDataSource || isObservable)
            {
                T directParam = GetDirectParam<T>(publicKey);
                var dataSource = isObservable ? publicValue : GetDataManager(publicValue, dataKey);
                if (baseComponent.DataContainer.ContainsKey(dataKey))
                {
                    if (!EqualityComparer<T>.Default.Equals(publicValue, directParam) && !IsClientChanges)
                    {
                        SfBaseUtils.UpdateDictionary(propertyKey, dataSource, baseComponent.PropertyChanges);
                        if (!(publicValue is DefaultAdaptor) && !(typeof(int[,]).Name == publicValue.GetType().Name))
                        {
                            DataHashTable.Clear();
                            SetDataHashTable(key, (IEnumerable<object>)publicValue);
                            baseComponent.DataContainer[dataKey] = publicValue;
                            DirectParameters[publicKey] = publicValue;
                        }
                    }
                }
                else
                {
                    baseComponent.DataContainer.Add(dataKey, publicValue);
                    SfBaseUtils.UpdateDictionary(propertyKey, dataSource, baseComponent.PropertyChanges);

                    // this.UnWireObservableEvents(publicValue);
                    WireObservableEvents(publicValue);
                    if (!ObservableData.ContainsKey(dataKey))
                    {
                        ObservableData.Add(dataKey, publicValue);
                    }
                }
            }
            else if (CompareValues(publicValue, privateValue))
            {
                // The below code snippets are used to find any API changes occurrs or not in application side.
                // Here we find differences for public and private changes.
                // if any changes occurs, then update in PropertyChanges collection and send JS interop/client calls to update in UI.
                // otherwise, don't update in PropertyChanges collection and prevent the JS interop/client calls.
                bool forceUpdate = false;
                T directParam = GetDirectParam<T>(publicKey);

                var isClientChanges = baseComponent.IsClientChanges;
                if (isClientChanges)
                {
                    if (IsEventTriggered)
                    {
                        // To handle clientside changes with event
                        isClientChanges = (ClientChanges.ContainsKey(dataKey) && CompareValues(publicValue, privateValue)) || !CompareValues(directParam, publicValue);
                    }
                }

                if ((CompareValues(directParam, publicValue) || !baseComponent.IsRendered) && !isClientChanges)
                {
                    forceUpdate = true;
                    DirectParameters[publicKey] = publicValue;
                }
                else
                {
                    finalResult = publicValue = privateValue;
                }

                // Update bindable properties for c# side changes alone, since the client changes already reflected in the UI.
                if (forceUpdate && !isDataSource)
                {
                    SfBaseUtils.UpdateDictionary(propertyKey, publicValue, baseComponent.PropertyChanges);
                }
            }

            dataKey = null;
            propertyKey = null;
            publicKey = null;
            subString = null;
            return finalResult;
        }

        internal T GetDirectParam<T>(string publicKey)
        {
            // Get the previous state from direct parameters for testing with current public property
            T directParam = DirectParameters.ContainsKey(publicKey) ? (T)DirectParameters[publicKey] : default(T);
            return directParam;
        }

#pragma warning disable CA1822 // Mark members as static
        internal bool CompareValues<T>(T oldValue, T newValue)
#pragma warning restore CA1822 // Mark members as static
        {
            var valueType = oldValue?.GetType();
            var isValueCollection = valueType != null && (valueType.Namespace != null && valueType.Namespace.Contains("Collections", StringComparison.Ordinal) || valueType.IsArray);

            if (isValueCollection)
            {
                var oldString = JsonConvert.SerializeObject(oldValue);
                var newString = JsonConvert.SerializeObject(newValue);
                return !string.Equals(oldString, newString, StringComparison.Ordinal);
            }
            else
            {
                return !EqualityComparer<T>.Default.Equals(oldValue, newValue);
            }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> Trigger(string eventName, string arg)
        {
            EventData data = DelegateList[eventName];

            // Deserialize the event arguments with generic type T
            var Settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
            var eventArgs = JsonConvert.DeserializeObject(arg, data.ArgumentType, Settings);

            // Set jsRuntime to event arguments
            if (eventArgs != null && data.ArgumentType.Namespace != "System")
            {
                PropertyInfo JSRunTimeProperty = data.ArgumentType.GetProperty("JsRuntime", BindingFlags.NonPublic | BindingFlags.Instance);
                JSRunTimeProperty.SetValue(eventArgs, JSRuntime);
            }

            // this.IsDataBound = false;
            IsEventTriggered = true;

            // clear bindable properties from client changes
            ClearClientChanges(true);

            // Invoke the event handler method
            dynamic argumentData = eventArgs;
            dynamic fn = data.Handler;
            await fn.InvokeAsync(argumentData);

            // Return the event argument changes to client side
            return JsonConvert.SerializeObject(argumentData);
        }

        internal object InvokeGenericMethod(string name, Type type, Type parentType, params object[] arguments)
        {
            MethodInfo method = parentType.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (name == "SetEvent")
            {
                // SetEvent to complex properties
                if (nameSpace == null && !jsProperty.Contains("sf.", StringComparison.Ordinal))
                {
                    arguments = arguments.Concat(new object[] { mainParent }).ToArray();
                }

                // SetEvent for nested tag Events
                else
                {
                    arguments = arguments.Concat(new object[] { null }).ToArray();
                }
            }

            return method.MakeGenericMethod(type).Invoke(this, arguments);
        }

        internal static string ConvertToProperCase(string text)
        {
            var property = char.ToUpper(text[0], CultureInfo.CurrentCulture) + text.Substring(1);
            return property;
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UpdateModel(Dictionary<string, object> properties)
        {
            IsClientChanges = true;
            if (properties != null)
            {
                UpdateComponentModel(properties, this);
            }

            await OnParametersSetAsync();
            StateHasChanged();
        }

#pragma warning disable CA1801 // Review unused parameters
        internal void UpdateComponentModel(Dictionary<string, object> properties, SfBaseExtension parentObject, bool isAutoInitialized = false)
#pragma warning restore CA1801 // Review unused parameters
        {
            foreach (string key in properties.Keys)
            {
                string propKey = key;
                string actualKey = key;
                int? sfIndex = null;
#pragma warning disable CA1307 // Specify StringComparison
                if (key.IndexOf("-", StringComparison.Ordinal) != -1)
#pragma warning restore CA1307 // Specify StringComparison
                {
                    var keyIndex = key.Split('-');
                    propKey = keyIndex[0];
#pragma warning disable CA1305 // Specify IFormatProvider
                    sfIndex = Convert.ToInt32(keyIndex[1]);
#pragma warning restore CA1305 // Specify IFormatProvider
                }

                var parentType = parentObject.GetType();
                PropertyInfo publicProperty = parentType.GetProperty(SfBaseExtension.ConvertToProperCase(propKey));
                PropertyInfo property = parentType.GetProperty("_" + propKey, BindingFlags.Instance | BindingFlags.NonPublic);
                if (property == null && parentType.BaseType != null && parentType.BaseType.Namespace.Contains("Syncfusion", StringComparison.Ordinal))
                {
                    property = parentType.BaseType.GetProperty("_" + propKey, BindingFlags.Instance | BindingFlags.NonPublic);
                    if (property == null && parentType.BaseType.BaseType != null && parentType.BaseType.BaseType.Namespace.Contains("Syncfusion", StringComparison.Ordinal))
                    {
                        property = parentType.BaseType.BaseType.GetProperty("_" + propKey, BindingFlags.Instance | BindingFlags.NonPublic);
                    }
                }

                Type propertyType = property?.PropertyType;
                if (property != null)
                {
                    var propertyValue = publicProperty.GetValue(parentObject);
                    propertyType = propertyValue != null && Nullable.GetUnderlyingType(propertyType) == null ? propertyValue.GetType() : propertyType;

                    // Update complex value changes
                    if (propertyValue is SfBaseExtension && properties[propKey] != null && !propertyType.IsPrimitive && !propertyType.IsValueType && !propertyType.IsEnum && propertyType != typeof(string))
                    {
                        UpdateComponentModel(JsonConvert.DeserializeObject<Dictionary<string, object>>(properties[propKey].ToString()), (SfBaseExtension)propertyValue);
#pragma warning disable CA1508 // Avoid dead conditional code
                        property?.SetValue(parentObject, publicProperty.GetValue(parentObject), null);
#pragma warning restore CA1508 // Avoid dead conditional code
                    }

                    // Update collection value changes
                    else if ((propertyType.Namespace != null && propertyType.Namespace.Contains("Collection", StringComparison.Ordinal) || propertyType.IsArray) && properties[actualKey] != null)
                    {
                        var value = propertyType.IsArray ? UpdateArrayValue(propertyType, properties[actualKey]) :
                            UpdateCollectionValue(propertyValue, propertyType, sfIndex, properties[actualKey], parentObject.IsAutoInitialized);
#pragma warning disable CA1508 // Avoid dead conditional code
                        property?.SetValue(parentObject, value, null);
#pragma warning restore CA1508 // Avoid dead conditional code
                        if (parentObject.IsAutoInitialized)
                        {
                            publicProperty?.SetValue(parentObject, value, null);
                        }

                        var parentComponent = parentObject.mainParent != null ? parentObject.mainParent : parentObject;
                        SfBaseUtils.UpdateDictionary(parentObject.jsProperty + "." + propKey, value, parentComponent.ClientChanges);
                    }

                    // Update property value changes to its parent object
                    else
                    {
                        var value = SfBaseUtils.ChangeType(properties[propKey], propertyType, true);
#pragma warning disable CA1508 // Avoid dead conditional code
                        property?.SetValue(parentObject, value, null);
#pragma warning restore CA1508 // Avoid dead conditional code
                        if (parentObject.IsAutoInitialized)
                        {
                            publicProperty?.SetValue(parentObject, value, null);
                        }

                        var parentComponent = parentObject.mainParent != null ? parentObject.mainParent : parentObject;
                        SfBaseUtils.UpdateDictionary(parentObject.jsProperty + "." + propKey, value, parentComponent.ClientChanges);
                    }
                }
            }
        }

        internal object UpdateCollectionValue(object propertyValue, Type propertyType, int? sfIndex, object model, bool isAutoInitialized = false)
        {
            object value;
            bool isAutoAnalyze = isAutoInitialized;
            if (sfIndex == null)
            {
                value = model;
                return SfBaseUtils.ChangeType(value, propertyType, true);
            }

            IList list = (IList)propertyValue;
            list = list != null ? list : (IList)Activator.CreateInstance(propertyType);
            var collectionValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.ToString());

            // Update null value collection
            if (propertyValue == null || list.Count == 0)
            {
                value = SfBaseUtils.ChangeType("[" + model + "]", propertyType, true);
            }

            // Update index based collection values
            else
            {
                if (sfIndex <= list.Count - 1)
                {
                    if (collectionValue.ContainsKey("sfAction") && (string)collectionValue["sfAction"] == "pop")
                    {
                        list.RemoveAt((int)sfIndex);
                    }
                    else
                    {
                        UpdateComponentModel(collectionValue, (SfBaseExtension)list[(int)sfIndex], isAutoAnalyze);
                    }

                    value = list;
                }
                else
                {
                    list.Add(SfBaseUtils.ChangeType(model, list[0].GetType(), true));
                    value = list;
                }
            }

            return SfBaseUtils.ChangeType(value, propertyType, true);
        }

#pragma warning disable CA1822 // Mark members as static
        internal object UpdateArrayValue(Type propertyType, object model)
#pragma warning restore CA1822 // Mark members as static
        {
            return JsonConvert.DeserializeObject(model.ToString(), propertyType);
        }

        internal virtual void SetEvent<T>(string name, EventCallback<T> eventCallback, SfBaseExtension BaseParent = null)
        {
            var Base = BaseParent != null ? BaseParent : this;
            if (Base.DelegateList.ContainsKey(name))
            {
                Base.DelegateList[name] = new EventData().Set<T>(eventCallback, typeof(T));
            }
            else
            {
                Base.DelegateList.Add(name, new EventData().Set<T>(eventCallback, typeof(T)));
            }
        }

        internal virtual object GetEvent(string name)
        {
            if (DelegateList.ContainsKey(name) == false)
            {
                return null;
            }

            return DelegateList[name].Handler;
        }

        internal string[] GetEventList()
        {
            string[] list = new string[DelegateList.Count];
            int len = 0;
            foreach (string key in DelegateList.Keys)
            {
                list.SetValue(key, len);
                len++;
            }

            return list;
        }

#pragma warning disable CA1822 // Mark members as static
        private JsonSerializerSettings GetJsonSerializerSettings()
#pragma warning restore CA1822 // Mark members as static
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
            settings.Converters.Add(new NonFlagStringEnumConverter());
            return settings;
        }

#pragma warning disable CA1822 // Mark members as static
        internal string SerialiazeBindableProp(Dictionary<string, object> bindableProp)
#pragma warning restore CA1822 // Mark members as static
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new NonFlagStringEnumConverter());
            return JsonConvert.SerializeObject(bindableProp, Formatting.Indented, settings);
        }

        protected virtual string GetSerializedModel()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, GetJsonSerializerSettings());
        }

        internal string GetSerializedModel(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, GetJsonSerializerSettings());
        }

        protected virtual string GetUpdateModel(bool isInit = false)
        {
            if (isInit)
            {
                string defaultSerializedModel = GetSerializedModel();
                if (PropertyChanges.Count > 0)
                {
                    Dictionary<string, object> defaultProps = JsonConvert.DeserializeObject<Dictionary<string, object>>(defaultSerializedModel);
                    foreach (KeyValuePair<string, object> property in PropertyChanges)
                    {
                        if (defaultProps.ContainsKey(property.Key))
                        {
                            defaultProps[property.Key] = property.Value;
                        }
                        else
                        {
                            defaultProps.Add(property.Key, property.Value);
                        }
                    }

                    return JsonConvert.SerializeObject(defaultProps, Formatting.Indented, GetJsonSerializerSettings());
                }

                return defaultSerializedModel;
            }
            else
            {
                return SerialiazeBindableProp(PropertyChanges);
            }
        }

#pragma warning disable CA1034 // Nested types should not be visible
        public class DataRequest
#pragma warning restore CA1034 // Nested types should not be visible
        {
            [JsonProperty("result")]
            public object Result { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> DataProcess(string dataManager, string key)
        {
            DataManagerRequest dm = Newtonsoft.Json.JsonConvert.DeserializeObject<DataManagerRequest>(dataManager);

            DataRequest DataObject = new DataRequest();
            DataManager dManager = DataManagerContainer.ContainsKey(key) ? DataManagerContainer[key] : DataManager;
            object result = null;
            if (dManager != null)
            {
                result = await dManager.ExecuteQuery<object>(dm);
                if (DataContainer.ContainsKey(key) && DataContainer[key] == null)
                {
                    DataContainer[key] = result;
                }
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver()
            };
            settings.Converters.Add(new BlazorIdJsonConverter(DataHashTable));
            var JsonResult = JsonConvert.SerializeObject(
                result,
                Formatting.Indented, settings);
            return JsonResult;
        }

        protected object GetDataManager(object dataSource, string key = null)
        {
            if (dataSource is Syncfusion.Blazor.Data.SfDataManager || dataSource is Syncfusion.Blazor.DataManager)
            {
                return dataSource;
            }

            // For Blazor Adaptor
            if (dataSource != null)
            {
                var type = dataSource.GetType();
                if (typeof(IEnumerable).IsAssignableFrom(type) ^ typeof(IEnumerable<object>).IsAssignableFrom(type))
                {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                    DataManager = new DataManager() { Json = ((IEnumerable)dataSource).Cast<object>() };
                }
                else
                {
                    DataManager = new DataManager() { Json = (IEnumerable<object>)dataSource };
                }
            }
            else if (DataManager == null)
            {
                DataManager = new DataManager() { Json = Enumerable.Empty<object>() };
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            }

            if (mainParent != null && dataSource != null)
            {
                if (mainParent.DataManagerContainer.ContainsKey(key))
                {
                    mainParent.DataManagerContainer[key] = DataManager;
                }
                else
                {
                    mainParent.DataManagerContainer.Add(key, DataManager);
                }
            }
            var dataAdaptor = new DefaultAdaptor(key); 
            return dataAdaptor;
        }

        /// <exclude/>
        // Template Rendering
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateTemplate(string Name, string TemplateData, string TemplateID, List<string> TemplateItems, List<string> BlazTempIds)
        {
            if (TemplateData != null && BlazTempIds != null && BlazTempIds.Count == 0)
            {
                GetType().GetProperty(Name + "Data")
                     .SetValue(this, JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(TemplateData, new JsonSerializerSettings()
                     {
                         DateParseHandling = DateParseHandling.DateTimeOffset
                     }));
            }
            else if (BlazTempIds != null && BlazTempIds.Count != 0)
            {
                Dictionary<string, object> HashDataTable = DataHashTable;
                List<Dictionary<string, object>> TempData = new List<Dictionary<string, object>>();
                foreach (var KeyValue in BlazTempIds)
                {
                    object Data = HashDataTable.FirstOrDefault(x => x.Key == KeyValue).Value;
                    Dictionary<string, object> Temp = new Dictionary<string, object>();
                    if (Data != null)
                    {
                        Temp.Add(KeyValue, Data);
                        TempData.Add(Temp);
                    }
                }

                GetType().GetProperty(Name + "Data").SetValue(this, TempData);
            }

            GetType().GetProperty(Name + "ID").SetValue(this, TemplateID);
            if (GetType().GetProperty("TemplateClientChanges") != null)
            {
                GetType().GetProperty("TemplateClientChanges").SetValue(this, true);
            }

            if (TemplateItems != null)
            {
                GetType().GetProperty(Name + "Items").SetValue(this, TemplateItems);
            }

            StateHasChanged();
        }

#pragma warning disable CA1822 // Mark members as static
        internal object GetObject(Dictionary<string, object> Data, Type ModelType)
#pragma warning restore CA1822 // Mark members as static
        {
            var ModelInstance = Data.GetType().GetConstructors().Any(c => c.GetParameters().Length == 0) ? FormatterServices.GetUninitializedObject(ModelType) : Activator.CreateInstance(ModelType);

            // var ModelInstance = Activator.CreateInstance(ModelType);
            foreach (var KeyValue in Data)
            {
                bool isBlazorId = KeyValue.Key.Split('_').Length > 0 ? KeyValue.Key.Split('_')[0] == "BlazTempId" : false;
                var pascalCase = SfBaseExtension.ConvertToProperCase(KeyValue.Key);
                var Property = ModelType.GetProperty(KeyValue.Key);
                Property = Property != null ? Property : ModelType.GetProperty(pascalCase);
                if (Property != null && isBlazorId.ToString() == "False")
                {
                    if (Property.CanWrite || Property.SetMethod != null)
                    {
                        Property.SetValue(ModelInstance, SfBaseUtils.ChangeType(KeyValue.Value, Property.PropertyType));
                    }
                }
                else if (isBlazorId.ToString() == "True")
                {
                    return KeyValue.Value;
                }
            }

            return ModelInstance;
        }

        internal void InitTemplates(string template, object templateParent, DotNetObjectReference<object> dotnetInstance = null)
        {
            var BaseComp = templateParent as SfBaseExtension;
            if (!BaseComp.ChildDotNetObjectRef.ContainsKey(template))
            {
                BaseComp.ChildDotNetObjectRef.Add(template, dotnetInstance);
            }
            else
            {
                ChildDotNetObjectRef.Add(template, dotnetInstance);
            }
        }

        internal async Task SetTemplates(string TemplateName, string TemplateID, RenderFragment<dynamic> Template, List<Dictionary<string, object>> TemplateData, int UniqueId, bool TemplateClientChanges = true, DotNetObjectReference<object> dotnetInstance = null)
        {
            if (TemplateData == null && Template != null)
            {
                await SetTemplateInstance<string>(JSRuntime, TemplateName, dotnetInstance, UniqueId);
            }

            if (TemplateData != null && TemplateClientChanges)
            {
                await SetTemplates<string>(JSRuntime, TemplateID);
            }
        }

        #region SyncfusionInterop
#pragma warning disable CA1822 // Mark members as static
        internal async ValueTask<T> Init<T>(IJSRuntime jsRuntime, string elementId, object model, string[] events, string nameSpace, DotNetObjectReference<object> helper, string bindableProps, Dictionary<string, object> htmlAttributes = null, Dictionary<string, object> templateRefs = null, DotNetObjectReference<object> adaptor = null, string localeText = null)
#pragma warning restore CA1822 // Mark members as static
        {
            try
            {
                return await jsRuntime.InvokeAsync<T>("sfBlazor.initialize", elementId, model, events, nameSpace, helper, bindableProps, htmlAttributes, templateRefs, adaptor, localeText);
            }
            catch (Exception e)
            {
                string Issue = nameSpace + " - #" + elementId + " - Init had internal server error \n";
                return await LogError<T>(jsRuntime, e, Issue);
                throw;
            }
        }

#pragma warning disable CA1822 // Mark members as static
        internal async ValueTask<T> Update<T>(IJSRuntime jsRuntime, string elementId, string model, string nameSpace)
#pragma warning restore CA1822 // Mark members as static
        {
            try
            {
                return await jsRuntime.InvokeAsync<T>("sfBlazor.setModel", elementId, model, nameSpace);
            }
            catch (InvalidOperationException e)
            {
                string Issue = nameSpace + " - #" + elementId + " - Update model had internal server error \n";
                return await LogError<T>(jsRuntime, e, Issue);
            }
        }

        internal async ValueTask<T> InvokeMethod<T>(IJSRuntime jsRuntime, string elementId, string methodName, string moduleName, object[] args, string nameSpace, ElementReference? element = null)
        {
            try
            {
                // enum type conversion issue in System.Text.Json serialization - https://github.com/dotnet/corefx/issues/38568
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                };
                return await jsRuntime.InvokeAsync<T>("sfBlazor.invokeMethod", elementId, methodName, moduleName, JsonConvert.SerializeObject(args, jsonSerializerSettings), element);
            }
            catch (InvalidOperationException e)
            {
                string Issue = nameSpace + " - #" + elementId + " - invokeMethod had internal server error \n";
                return await LogError<T>(jsRuntime, e, Issue);
            }
        }

#pragma warning disable CA1822 // Mark members as static
        internal async ValueTask<T> SetTemplateInstance<T>(IJSRuntime jsRuntime, string templateName, DotNetObjectReference<object> helper, int guid)
#pragma warning restore CA1822 // Mark members as static
        {
            try
            {
                return await jsRuntime.InvokeAsync<T>("sfBlazor.setTemplateInstance", templateName, helper, guid);
            }
            catch (InvalidOperationException e)
            {
                return await LogError<T>(jsRuntime, e);
            }
        }

#pragma warning disable CA1822 // Mark members as static
        internal async ValueTask<T> SetTemplates<T>(IJSRuntime jsRuntime, string templateID)
#pragma warning restore CA1822 // Mark members as static
        {
            try
            {
                return await jsRuntime.InvokeAsync<T>("sfBlazor.setTemplate", templateID);
            }
            catch (InvalidOperationException e)
            {
                return await LogError<T>(jsRuntime, e);
            }
        }

        internal static ValueTask<T> LogError<T>(IJSRuntime jsRuntime, Exception e, string message = "")
        {
            try
            {
                ErrorMessage error = new ErrorMessage();
                error.Message = message + e.Message;
                error.Stack = e.StackTrace;
                if (e.InnerException != null)
                {
                    error.Message = message + e.InnerException.Message;
                    error.Stack = e.InnerException.StackTrace;
                }

                return jsRuntime.InvokeAsync<T>("sfBlazor.throwError", error);
            }
            catch (InvalidOperationException)
            {
                return new ValueTask<T>();
            }
        }
        #endregion
    }

    /// <summary>
    /// Represents the event argument data.
    /// </summary>
    internal class EventData
    {
        // EventCallback handler method
        public object Handler { get; set; }

        // Event argument type
        public Type ArgumentType { get; set; }

        // Update event data in the instance.
        public EventData Set<T>(EventCallback<T> action, Type type)
        {
            Handler = action;
            ArgumentType = type;
            return this;
        }
    }
}

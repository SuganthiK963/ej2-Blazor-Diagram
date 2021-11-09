using System;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using Syncfusion.Blazor.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Runtime.CompilerServices;
using System.Globalization;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Charts")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DocumentEditor")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.HeatMap")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.InPlaceEditor")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Maps")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.PdfViewer")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.PivotView")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.ProgressBar")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.SmithChart")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Sparkline")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.StockChart")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.BulletChart")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Inputs")]

namespace Syncfusion.Blazor
{
    /// <summary>
    /// A Base Component for all the Syncfusion Blazor UI components.
    /// </summary>
#pragma warning disable CA1708 // Identifiers should differ by more than case
    public abstract class BaseComponent : OwningComponentBase, IBaseInit
#pragma warning restore CA1708 // Identifiers should differ by more than case
    {
        #region private properties
        protected int uniqueId { get; set; }

        private List<string> directParamKeys = new List<string>();
        #endregion

        #region protected properties
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        [JsonIgnore]
        [CascadingParameter]
        protected EditContext EditContext { get; set; }

        protected virtual string nameSpace { get; set; }

        protected virtual string jsProperty { get; set; }

        protected virtual BaseComponent mainParent { get; set; }

        protected virtual JSInteropAdaptor CreateJsAdaptor() => default;

        protected DotNetObjectReference<object> DotNetObjectRef { get; set; }
        #endregion

        #region internal properties
        internal bool IsDataBound { get; set; }

        internal bool IsRerendering { get; set; }

        internal bool IsClientChanges { get; set; }

        internal bool IsServerRendered { get; set; }

        internal bool IsEventTriggered { get; set; }

        internal bool IsPropertyChanged { get; set; }

        internal bool IsAutoInitialized { get; set; }

        internal bool isObservableCollectionChanged { get; set; }

        internal Dictionary<string, object> htmlAttributes { get; set; }

        internal List<object> InvokedEvents { get; set; } = new List<object>();

        internal List<string> ObservableChangedList { get; set; } = new List<string>();

        internal List<ScriptModules> DynamicScripts { get; set; } = new List<ScriptModules>();

        internal List<ScriptModules> DependentScripts { get; set; } = new List<ScriptModules>();

        internal Dictionary<string, object> ObservableData = new Dictionary<string, object>();
        internal Dictionary<string, EventData> DelegateList = new Dictionary<string, EventData>();
        internal Dictionary<string, object> ChildDotNetObjectRef = new Dictionary<string, object>();

        internal Dictionary<string, object> ClientChanges { get; set; } = new Dictionary<string, object>();

        internal Dictionary<string, object> DirectParameters { get; set; } = new Dictionary<string, object>();

        internal Dictionary<string, object> BindableProperties { get; set; } = new Dictionary<string, object>();

        [JsonIgnore]
        internal JSInteropAdaptor JsAdaptor { get; set; }

        [Inject]
        [JsonIgnore]
        internal SyncfusionBlazorService SyncfusionService { get; set; }

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
        public bool IsRendered { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public virtual Type ModelType { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public DataManager DataManager { get; set; }

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
        public virtual Dictionary<string, object> DataContainer { get; set; } = new Dictionary<string, object>();

        /// <exclude/>
        [JsonIgnore]
        public virtual Dictionary<string, object> DataHashTable { get; set; } = new Dictionary<string, object>();
        #endregion

        #region life cycle methods
        protected override async Task OnInitializedAsync()
        {
            JsAdaptor = CreateJsAdaptor();
            JsAdaptor?.Init();
            await base.OnInitializedAsync();
            IsRerendering = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var tempDictionary = new Dictionary<string, object>();
                foreach (var key in directParamKeys)
                {
                    var initValue = GetType().GetProperty(key)?.GetValue(this);
                    updateDictionary(key, initValue, tempDictionary);
                }

                DirectParameters = tempDictionary.ToDictionary(prop => prop.Key, prop => prop.Value);
                tempDictionary = null;
            }

            if (nameSpace != null && !IsServerRendered)
            {
                if (firstRender && !IsClientChanges)
                {
                    await InitComponent();
                }
                else
                {
                    await DataBind();
                }
            }

            IsRerendering = true;
            isObservableCollectionChanged = false;
            ObservableChangedList = new List<string>();
        }
        #endregion

        internal async Task InitComponent()
        {
            // The below condition avoid to reinitialize the already rendered component through ResourceManager.
            // Some components may used StateHasChange at OnAfterRenderAsync which cause reinitializing the component again even after rendering firstRender.
            // This is applicable only for dynamic script loading
            if (!SyncfusionService.options.IgnoreScriptIsolation && IsRendered)
            {
                return;
            }

#if !NETSTANDARD
            if (!SyncfusionService.options.IgnoreScriptIsolation)
            {
                // load init-interop script.
                await SfBaseUtils.ImportModule(jsRuntime, SfScriptModules.SfBase, SyncfusionService.ScriptHashKey);
            }
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

        internal virtual async Task InitialRendered()
        {
            await Task.CompletedTask;
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnInitRenderAsync()
        {
            if (!SyncfusionService.options.IgnoreScriptIsolation || SyncfusionService.IsEnabledScriptIsolation)
            {
                await SfBaseUtils.ImportModule(jsRuntime, SfScriptModules.SfBaseExtended, SyncfusionService.ScriptHashKey);
                if (DependentScripts.Count > 0)
                {
                    foreach (var dependentModule in DependentScripts)
                    {
                        await SfBaseUtils.ImportScripts(jsRuntime, dependentModule, SyncfusionService.ScriptHashKey);
                    }
                }


                // load component specific dependency scripts.
                await SfBaseUtils.ImportModules(jsRuntime, DynamicScripts, SyncfusionService.ScriptHashKey);
            }

            if (SyncfusionService.IsFirstBaseResource)
            {
                var currentCulture = Intl.CurrentCulture;
                SyncfusionService.IsFirstBaseResource = false;
                await SfBaseUtils.InvokeMethod(jsRuntime, "sfBlazor.loadCldr", GlobalizeJsonGenerator.GetGlobalizeJsonString(currentCulture));
                await SfBaseUtils.InvokeMethod(jsRuntime, "sfBlazor.setCulture", string.IsNullOrEmpty(currentCulture.Name) ? "en-US" : currentCulture.Name, Intl.GetCultureFormats(currentCulture.Name));
            }

            DotNetObjectRef = DotNetObjectReference.Create<object>(this);
            string bindableProps = serialiazeBindableProp(BindableProperties);
            var key = nameSpace + ".dataSource";
            if (DataContainer.ContainsKey(key) && DataContainer[key] != null)
            {
                SetDataHashTable(key, (IEnumerable)DataContainer[key]);
            }

            await SyncfusionInterop.Init<string>(jsRuntime, ID, getUpdateModel(true), GetEventList(), nameSpace, DotNetObjectRef, bindableProps, htmlAttributes, ChildDotNetObjectRef, JsAdaptor?.GetRef(), LocaleText);
            IsRendered = true;

            // set initial property changes in component create event
            if (DelegateList.ContainsKey("created") && BindableProperties.Count > 0)
            {
                await DataBind();
            }

            BindableProperties.Clear();
            TemplateClientChanges = false;
            IsRerendering = true;
            isObservableCollectionChanged = false;
            ObservableChangedList = new List<string>();
            DynamicScripts.Clear();
            DependentScripts.Clear();
            await InitialRendered();
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

        internal virtual void ComponentDispose()
        {
        }

        internal void CommonDispose()
        {
            EditContext = null;
            DataManager?.Dispose();
            Localizer = null;
            mainParent = null;
            BindableProperties.Clear();
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
            htmlAttributes?.Clear();
            ChildDotNetObjectRef.Clear();
            UnWireObservableEvents();
            DotNetObjectRef?.Dispose();
        }

        public virtual void Dispose()
        {
            CommonDispose();
            ComponentDispose();
            if (nameSpace != null && IsRendered)
            {
                ValueTask<object> valueTask = SyncfusionInterop.InvokeMethod<object>(jsRuntime, ID, "destroy", null, null, nameSpace);
            }

            JsAdaptor?.Dispose();
            JsAdaptor = null;
        }

        public async void Refresh()
        {
            if (nameSpace != null && IsRendered)
            {
                await SyncfusionInterop.InvokeMethod<object>(jsRuntime, ID, "refresh", null, null, nameSpace);
            }
        }

        /// <exclude/>
#pragma warning disable CA1801 // Review unused parameters
        public async Task DataBind(bool hasStateChanged = false)
#pragma warning restore CA1801 // Review unused parameters
        {
            IsDataBound = false;
            IsEventTriggered = false;
            if (ClientChanges.Count > 0)
            {
                await OnClientChanged(ClientChanges);
            }

            clearClientChanges();
            if (IsRendered && nameSpace != null && BindableProperties.Count > 0)
            {
                if (!SyncfusionService.options.IgnoreScriptIsolation)
                {
                    await SfBaseUtils.ImportModules(jsRuntime, DynamicScripts, SyncfusionService.ScriptHashKey);
                }
                string bindableProps = serialiazeBindableProp(BindableProperties);
                BindableProperties.Clear();
                await SyncfusionInterop.Update<object>(jsRuntime, ID, bindableProps, nameSpace);
            }
            else
            {
                BindableProperties.Clear();
            }

            IsPropertyChanged = false;
            IsDataBound = true;
            if (IsRendered)
            {
                DynamicScripts.Clear();
                DependentScripts.Clear();
            }

            InvokedEvents = new List<object>();
        }

        protected void clearClientChanges(bool clearBindables = false)
        {
            if ((IsClientChanges && !IsDataBound) || clearBindables)
            {
                foreach (var property in ClientChanges)
                {
                    if (BindableProperties.ContainsKey(property.Key))
                    {
                        BindableProperties.Remove(property.Key);
                    }
                }

                if (!clearBindables)
                {
                    IsClientChanges = false;
                    ClientChanges.Clear();
                }
            }
        }

        internal void updateDictionary(string key, object value, Dictionary<string, object> dictionary)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
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
            if (!jsonString.Contains(",", StringComparison.Ordinal))
            {
                jsonString = jsonString.Replace("\\", string.Empty, StringComparison.Ordinal);
            }

            if (jsonString.IndexOf("\"[", StringComparison.Ordinal) == jsonString.IndexOf("\"[\\", StringComparison.Ordinal) && jsonString.IndexOf("\"{", StringComparison.Ordinal) == jsonString.IndexOf("\"{\\", StringComparison.Ordinal))
            {
                return jsonString;
            }
            if ((jsonString.IndexOf("\"[", StringComparison.Ordinal) != jsonString.IndexOf("\"[\\", StringComparison.Ordinal)) || (jsonString.IndexOf("\"{", StringComparison.Ordinal) != jsonString.IndexOf("\"{\\", StringComparison.Ordinal)))
            {
                if ((jsonString.LastIndexOf("\"{", StringComparison.Ordinal) > 0) && (!jsonString.Contains(",", StringComparison.Ordinal)))
                {
                    jsonString = jsonString.Replace("\"[", "[", StringComparison.Ordinal).Replace("]\"", "]", StringComparison.Ordinal)
                                .Replace("\"{", "{", StringComparison.Ordinal).Replace("}\"", "}", StringComparison.Ordinal);
                }
            }

            return jsonString;
        }

        // Invoke void return type methods
        internal async Task InvokeMethod(string methodName, string moduleName = null, params object[] methodParams)
        {
            await IsScriptRendered();
            methodParams = (methodParams != null && methodParams.Length > 0) ? methodParams : null;
            await SyncfusionInterop.InvokeMethod<object>(jsRuntime, ID, methodName, moduleName, methodParams, nameSpace);
        }

        // Invoke object return type methods
        internal async Task<T> InvokeMethod<T>(string methodName, bool isObjectReturnType, string moduleName = null, params object[] methodParams)
        {
            await IsScriptRendered();
            methodParams = (methodParams != null && methodParams.Length > 0) ? methodParams : null;
            if (!isObjectReturnType)
            {
                return await SyncfusionInterop.InvokeMethod<T>(jsRuntime, ID, methodName, moduleName, methodParams, nameSpace);
            }
            else
            {
                string ReturnValue = await SyncfusionInterop.InvokeMethod<string>(jsRuntime, ID, methodName, moduleName, methodParams, nameSpace);
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task InvokeSet<T>(string moduleName, string methodName, params object[] methodParams)
        {
            await SyncfusionInterop.InvokeSet<T>(jsRuntime, ID, moduleName, methodName, methodParams, nameSpace);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<T> InvokeGet<T>(string moduleName, string methodName)
        {
            return await SyncfusionInterop.InvokeGet<T>(jsRuntime, ID, moduleName, methodName, nameSpace);
        }

        private void observableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            isObservableCollectionChanged = true;
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)item).PropertyChanged -= observablePropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)item).PropertyChanged += observablePropertyChanged;
                    }
                }
            }
        }

        private void observablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            isObservableCollectionChanged = true;
        }

        protected virtual void WireObservableEvents(object collection)
        {
            if (collection != null && collection.GetType().IsGenericType)
            {
                if (collection is INotifyCollectionChanged)
                {
                    ((INotifyCollectionChanged)collection).CollectionChanged += new NotifyCollectionChangedEventHandler(observableCollectionChanged);
                }

                if (collection is INotifyPropertyChanged)
                {
                    List<object> enumerableCollection = new List<object>((IEnumerable<object>)collection);
                    var firstItem = enumerableCollection.FirstOrDefault();
                    if (firstItem is INotifyPropertyChanged)
                    {
                        foreach (var item in enumerableCollection)
                        {
                            ((INotifyPropertyChanged)item).PropertyChanged += new PropertyChangedEventHandler(observablePropertyChanged);
                        }
                    }
                }
            }
        }

        private void UnWireObservableEvents()
        {
            // Dictionary<string, object> Observedata = (ObervableData != null) ? (Dictionary<string, object>)ObervableData : this.ObservableData;
            if (ObservableData.Count > 0)
            {
                foreach (var collection in ObservableData)
                {
                    if (collection.Value is INotifyCollectionChanged)
                    {
                        ((INotifyCollectionChanged)collection.Value).CollectionChanged -= observableCollectionChanged;
                    }

                    if (collection.Value is INotifyPropertyChanged)
                    {
                        List<object> enumerableCollection = new List<object>((IEnumerable<object>)collection.Value);
                        var firstItem = enumerableCollection.FirstOrDefault();
                        if (firstItem is INotifyPropertyChanged)
                        {
                            foreach (var item in enumerableCollection)
                            {
                                ((INotifyPropertyChanged)item).PropertyChanged -= observablePropertyChanged;
                            }
                        }
                    }
                }
            }
        }

        internal async virtual Task<T> updateProperty<T>(string key, T publicValue, T privateValue, object eventCallback = null, Expression<Func<T>> expression = null, bool isDataSource = false, bool isObservable = false)
        {
            var propertyKey = !jsProperty.StartsWith("sf.", StringComparison.Ordinal) ? $"{jsProperty}.{key}" : key;
            var dataKey = !string.IsNullOrEmpty(jsProperty) ? $"{jsProperty}.{key}" : key;
            var baseComponent = mainParent != null ? mainParent : this;
            var subString = key.Substring(1);
            string publicKey = char.ToUpper(key[0], CultureInfo.CurrentCulture) + subString;

            T finalResult = publicValue;
            if (isDataSource || isObservable)
            {
                T directParam = getDirectParam<T>(publicKey);
                var dataSource = isObservable ? publicValue : GetDataManager(publicValue, dataKey);
                if (baseComponent.DataContainer.ContainsKey(dataKey))
                {
                    if (!EqualityComparer<T>.Default.Equals(publicValue, directParam) && !IsClientChanges)
                    {
                        updateDictionary(propertyKey, dataSource, baseComponent.BindableProperties);
                        var typeValue = new HashSet<string>(new[] { typeof(int[,]).Name, typeof(double[,]).Name, typeof(int?[,]).Name });
                        if (!(publicValue is DefaultAdaptor) && !typeValue.Contains(publicValue.GetType().Name))
                        {
                            DataHashTable.Clear();
                            SetDataHashTable(key, (IEnumerable<object>)publicValue);
                            baseComponent.DataContainer[dataKey] = publicValue;
                            DirectParameters[publicKey] = publicValue;
                            baseComponent.IsPropertyChanged = IsRerendering;
                        }
                    }
                    else if (privateValue != null && IsClientChanges && ClientChanges.ContainsKey(key))
                    {
                        finalResult = (T)SfBaseUtils.ChangeType(privateValue, publicValue.GetType());
                        baseComponent.DataContainer[dataKey] = publicValue;
                        DataHashTable.Clear();
                        SetDataHashTable(key, (IEnumerable<object>)publicValue);
                        baseComponent.IsPropertyChanged = IsRerendering;
                    }

                    if (isObservableCollectionChanged && !ObservableChangedList.Contains(key))
                    {
                        ObservableChangedList.Add(key);
                        if ((publicValue as IEnumerable<object>).Count() != DataHashTable.Count)

                        // Below commentted line breaks the performance in webassembly when it enabled.
                        // if (!object.Equals((publicValue as IEnumerable<object>), this.DataHashTable))
                        {
                            DataHashTable.Clear();
                            SetDataHashTable(key, (IEnumerable<object>)publicValue);
                            baseComponent.IsPropertyChanged = IsRerendering;
                        }

                        updateDictionary(propertyKey, dataSource, baseComponent.BindableProperties);
                    }
                }
                else
                {
                    baseComponent.DataContainer.Add(dataKey, publicValue);
                    updateDictionary(propertyKey, dataSource, baseComponent.BindableProperties);

                    // this.UnWireObservableEvents(publicValue);
                    WireObservableEvents(publicValue);
                    if (!ObservableData.ContainsKey(dataKey))
                    {
                        ObservableData.Add(dataKey, publicValue);
                    }

                    // this.ObservableData.Add(dataKey, publicValue);
                }
            }

            // check private and public property value changes for setting proper value
            // !!! Don't change this logic without proper testing, this may cause problems in below scenarios,
            // data-binding using button click
            // direct property value changes from client side user interaction and then component re-rendering from eventcallback
            // one-way or two-way data-binding from c# and then component re-rendering from eventcallback
            else if (CompareValues(publicValue, privateValue))
            {
                bool forceUpdate = false;
                EventCallback<T> eventMethod;
                T directParam = getDirectParam<T>(publicKey);

                var isClientChanges = baseComponent.IsClientChanges;
                if (isClientChanges)
                {
                    if (IsEventTriggered)
                    {
                        // To handle clientside changes with event
                        isClientChanges = (ClientChanges.ContainsKey(dataKey) && CompareValues(publicValue, privateValue)) || !CompareValues(directParam, publicValue);

                        // To handle the changed value inside component event for the second time
                        // forceUpdate = !isClientChanges;
                        // publicValue = forceUpdate ? publicValue : privateValue;
                    }

                    // else
                    // {
                    // To handle client side changes without events
                    // publicValue = privateValue;
                    // }
                }

                // To handle property binding using external button click

                // else if (!isClientChanges && !this.IsEventTriggered) {
                //    directParam = privateValue;
                // }

                // !this.IsRendered - Resolve property binding from OnAfterRender initial rendering
                // !isClientChanges - Resolve client changes not affect with publicValue
                if ((CompareValues(directParam, publicValue) || !baseComponent.IsRendered) && !isClientChanges)
                {
                    forceUpdate = true;
                    DirectParameters[publicKey] = publicValue;
                    baseComponent.IsPropertyChanged = true;
                }
                else
                {
                    finalResult = publicValue = privateValue;
                }

                // Checking eventcallback for two-way notification
                if (eventCallback != null)
                {
                    eventMethod = (EventCallback<T>)eventCallback;
                    if (eventMethod.HasDelegate && !InvokedEvents.Contains(eventMethod))
                    {
                        DirectParameters[publicKey] = publicValue;
                        if (!(publicValue is null) && !publicValue.GetType().IsArray)
                        {
                            InvokedEvents.Add(eventMethod);
                        }

                        await eventMethod.InvokeAsync(publicValue);
                    }
                }

                if (expression != null)
                {
                    EditContext?.NotifyFieldChanged(Microsoft.AspNetCore.Components.Forms.FieldIdentifier.Create<T>(expression));
                }

                // Update bindable properties for c# side changes alone, since the client changes already reflected in the UI.
                if (forceUpdate && !isDataSource)
                {
                    updateDictionary(propertyKey, publicValue, baseComponent.BindableProperties);
                }
            }

            // else if(this.mainParent != null && this.mainParent.IsRendered && !this.IsRerendering && !this.IsRendered && !baseComponent.IsClientChanges)
            // {
            //     this.RenderNewChild();
            //     this.IsRerendering = true;
            // }
            dataKey = null;
            propertyKey = null;
            publicKey = null;
            subString = null;
            return finalResult;
        }

        internal T getDirectParam<T>(string publicKey)
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

        // The below scenario is used to add a new child component, when its parent component is rerender in the page.
        // i.e. The parent is already rendered in the page and a new child will be added dynamically at the time of rerendering.
        internal void RenderNewChild()
        {
            var childString = getSerializedModel();
            var childData = JsonConvert.DeserializeObject<Dictionary<string, object>>(childString);
            childData.Add("isNewComponent", true);
            foreach (var property in childData)
            {
                var jsKey = jsProperty + "." + property.Key;
                DirectParameters[property.Key] = property.Value;
                updateDictionary(jsKey, property.Value, mainParent.BindableProperties);
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
                if (JSRunTimeProperty != null)
                {
                    JSRunTimeProperty.SetValue(eventArgs, jsRuntime);
                }
            }

            IsDataBound = false;
            IsEventTriggered = true;

            // clear bindable properties from client changes
            clearClientChanges(true);

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
#pragma warning disable CA1062 // Validate arguments of public methods
            UpdateComponentModel(properties, this);
#pragma warning restore CA1062 // Validate arguments of public methods
            await OnParametersSetAsync();
            StateHasChanged();
        }

#pragma warning disable CA1801 // Review unused parameters
        internal void UpdateComponentModel(Dictionary<string, object> properties, BaseComponent parentObject, bool isAutoInitialized = false)
#pragma warning restore CA1801 // Review unused parameters
        {
            foreach (string key in properties.Keys)
            {
                string propKey = key;
                string actualKey = key;
                int? sfIndex = null;
                if (key.IndexOf("-", StringComparison.Ordinal) != -1)
                {
                    var keyIndex = key.Split('-');
                    propKey = keyIndex[0];
                    sfIndex = Convert.ToInt32(keyIndex[1], CultureInfo.CurrentCulture);
                }

                var parentType = parentObject.GetType();
                PropertyInfo publicProperty = parentType.GetProperty(BaseComponent.ConvertToProperCase(propKey));
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
                    if (propertyValue is BaseComponent && properties[propKey] != null && !propertyType.IsPrimitive && !propertyType.IsValueType && !propertyType.IsEnum && propertyType != typeof(string))
                    {
                        UpdateComponentModel(JsonConvert.DeserializeObject<Dictionary<string, object>>(properties[propKey].ToString()), (BaseComponent)propertyValue);
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
                        updateDictionary(parentObject.jsProperty + "." + propKey, value, parentComponent.ClientChanges);
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
                        updateDictionary(parentObject.jsProperty + "." + propKey, value, parentComponent.ClientChanges);
                    }
                }
            }
        }

        internal object UpdateCollectionValue(object propertyValue, Type propertyType, int? sfIndex, object model, bool isAutoInitialized = false)
        {
            object value;
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
                        UpdateComponentModel(collectionValue, (BaseComponent)list[(int)sfIndex], isAutoInitialized);
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

        internal virtual async Task OnClientChanged(IDictionary<string, object> properties)
        {
            await Task.CompletedTask;
        }

        internal virtual void SetEvent<T>(string name, EventCallback<T> eventCallback, BaseComponent BaseParent = null)
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
        private JsonSerializerSettings getJsonSerializerSettings()
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
        internal string serialiazeBindableProp(Dictionary<string, object> bindableProp)
#pragma warning restore CA1822 // Mark members as static
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new NonFlagStringEnumConverter());
            return JsonConvert.SerializeObject(bindableProp, Formatting.Indented, settings);
        }

        protected virtual string getSerializedModel()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, getJsonSerializerSettings());
        }

        protected virtual string getUpdateModel(bool isInit = false)
        {
            if (isInit)
            {
                string defaultSerializedModel = getSerializedModel();
                if (BindableProperties.Count > 0)
                {
                    Dictionary<string, object> defaultProps = JsonConvert.DeserializeObject<Dictionary<string, object>>(defaultSerializedModel);
                    foreach (KeyValuePair<string, object> property in BindableProperties)
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

                    return JsonConvert.SerializeObject(defaultProps, Formatting.Indented, getJsonSerializerSettings());
                }

                return defaultSerializedModel;
            }
            else
            {
                return serialiazeBindableProp(BindableProperties);
            }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ErrorHandling(string message, string stack)
        {
            Console.Error.WriteLine(message + "\n" + stack);
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

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> Insert(string value, string key, int position, string query = null)
        {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
            if (DataContainer.ContainsKey(key))
            {
                DataManager dManager = DataManagerContainer.ContainsKey(key) ? DataManagerContainer[key] : DataManager;
                Type DataType = null;
                if (DataContainer[key] == null)
                {
                    DataContainer[key] = Enumerable.Empty<object>().ToList();
                    dManager.Json = (IEnumerable<object>)DataContainer[key];
                }

                DataType = GetType();
                var data = DataContainer[key];
                DataType = DataType ?? data.GetType();
                if (ModelType != null || (DataType.IsGenericType && DataType.GetGenericArguments().Length > 0))
                {
                    Type typ = ModelType != null ? ModelType : DataType.GetGenericArguments()[0];
                    var newrec = JsonConvert.DeserializeObject(value, typ, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                    var qry = new Query();
                    qry.Queries = JsonConvert.DeserializeObject<DataManagerRequest>(query);
#pragma warning disable CA1508 // Avoid dead conditional code
                    var addedRecord = await dManager.Insert<object>(newrec, qry?.Queries?.Table, qry, position);
#pragma warning restore CA1508 // Avoid dead conditional code

                    if (DataHashTable.Count != 0)
                    {
                        Random Trandom = new Random();
#pragma warning disable CA5394 // Do not use insecure randomness
                        DataHashTable.Add("BlazTempId_" + Trandom.Next(), newrec);
#pragma warning restore CA5394 // Do not use insecure randomness
                    }

                    var result = JsonConvert.SerializeObject(
                        addedRecord ?? newrec, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new DefaultContractResolver()
                        });
                    return result;
                }
                else
                {
                    return null;
                }
            }

            return null;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> Update(string value, string keyField, string key, string query = null)
        {
            if (DataContainer.ContainsKey(key))
            {
                DataManager dManager = DataManagerContainer.ContainsKey(key) ? DataManagerContainer[key] : DataManager;
                var data = DataContainer[key];
                Type DataType = GetType();
                DataType = DataType ?? data.GetType();
                if (ModelType != null || (DataType.IsGenericType && DataType.GetGenericArguments().Length > 0))
                {
                    Type typ = ModelType != null ? ModelType : DataType.GetGenericArguments()[0];
                    IDictionary<string, object> updateProperties = JsonConvert.DeserializeObject<IDictionary<string, object>>(value);
                    var updatedrec = JsonConvert.DeserializeObject(value, typ, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                    var qry = new Query();
                    qry.Queries = JsonConvert.DeserializeObject<DataManagerRequest>(query);
#pragma warning disable CA1508 // Avoid dead conditional code
                    var updatedResult = await dManager.Update<object>(keyField, updatedrec, qry?.Queries?.Table, qry, null, updateProperties);
#pragma warning restore CA1508 // Avoid dead conditional code
                    var result = JsonConvert.SerializeObject(
                        updatedResult ?? updatedrec, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new DefaultContractResolver()
                        });

                    return result;
                }
                else
                {
                    return value;
                }
            }

            return null;
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> Remove(string value, string keyField, string key, string query = null)
        {
            if (DataContainer.ContainsKey(key))
            {
                DataManager dManager = DataManagerContainer.ContainsKey(key) ? DataManagerContainer[key] : DataManager;
                var data = DataContainer[key];
                Type DataType = GetType();
                DataType = DataType ?? data.GetType();
                var qry = new Query();
                qry.Queries = JsonConvert.DeserializeObject<DataManagerRequest>(query);
#pragma warning disable CA1508 // Avoid dead conditional code
                var removedRecord = await dManager.Remove<object>(keyField, value, qry?.Queries?.Table, qry);
#pragma warning restore CA1508 // Avoid dead conditional code
                var result = removedRecord != null ? JsonConvert.SerializeObject(
                        removedRecord, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new DefaultContractResolver()
                        }) : key;
                return result;
            }

            return null;
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<object> BatchUpdate(string changed, string added, string deleted, string keyField, string key, int? dropIndex, string query = null)
        {
            if (DataContainer.ContainsKey(key))
            {
                var data = DataContainer[key];
                Type DataType = GetType();
                DataType = DataType ?? data.GetType();
                object changedRecords = null;
                object addedRecords = null;
                object deletedRecords = null;
                List<object> originalUpdatedRecords = new List<object>();
                var qry = new Query();
                qry.Queries = JsonConvert.DeserializeObject<DataManagerRequest>(query);
                if (ModelType != null || (DataType.IsGenericType && DataType.GetGenericArguments().Length > 0))
                {
                    Type typ = ModelType != null ? ModelType : DataType.GetGenericArguments()[0];
                    Type genType = typeof(IEnumerable<>).MakeGenericType(typ);

                    /////Update Operation

                    if (changed != null)
                    {
                        changedRecords = JsonConvert.DeserializeObject(changed, genType, new JsonSerializerSettings
                        {
                            DateTimeZoneHandling = DateTimeZoneHandling.Local
                        });
                    }

                    /////Insert Operation

                    if (added != null)
                    {
                        addedRecords = JsonConvert.DeserializeObject(added, genType, new JsonSerializerSettings
                        {
                            DateTimeZoneHandling = DateTimeZoneHandling.Local
                        });

                        foreach (var newrec in (IEnumerable)addedRecords)
                        {
                            // ((IList)data).Insert(dropIndex, newrec);
                            if (DataHashTable.Count != 0)
                            {
                                Random Trandom = new Random();
#pragma warning disable CA5394 // Do not use insecure randomness
                                DataHashTable.Add("BlazTempId_" + Trandom.Next(), newrec);
#pragma warning restore CA5394 // Do not use insecure randomness
                            }

                            // dropIndex++;
                        }
                    }

                    /////Delete Operation

                    if (deleted != null)
                    {
                        deletedRecords = JsonConvert.DeserializeObject(deleted, genType, new JsonSerializerSettings
                        {
                            DateTimeZoneHandling = DateTimeZoneHandling.Local
                        });
                    }

                    DataManager dManager = DataManagerContainer.ContainsKey(key) ? DataManagerContainer[key] : DataManager;
#pragma warning disable CA1508 // Avoid dead conditional code
                    var result = await dManager.SaveChanges<object>(changedRecords, addedRecords, deletedRecords, keyField, dropIndex, qry?.Queries?.Table, qry);
#pragma warning restore CA1508 // Avoid dead conditional code
                    return JsonConvert.SerializeObject(
                        result ?? new { changedRecords, addedRecords, deletedRecords }, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new DefaultContractResolver()
                        });
                }
                else
                {
                    return JsonConvert.SerializeObject(
                       new { changedRecords, addedRecords, deletedRecords }, Formatting.Indented, new JsonSerializerSettings
                       {
                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                           ContractResolver = new DefaultContractResolver()
                       });
                }
            }

            return null;
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

        internal object GetObject(Dictionary<string, object> Data, Type ModelType)
        {
            // Handling Parameterless Constructor
            // var ModelInstance = Data.GetType().GetConstructors().Any(c => c.GetParameters().Length == 0) ? FormatterServices.GetUninitializedObject(ModelType) : Activator.CreateInstance(ModelType);
            var ModelInstance = Activator.CreateInstance(ModelType);

            foreach (var KeyValue in Data)
            {
                bool isBlazorId = KeyValue.Key.Split('_').Length > 0 ? KeyValue.Key.Split('_')[0] == "BlazTempId" : false;
                var pascalCase = BaseComponent.ConvertToProperCase(KeyValue.Key);
                var Property = ModelType.GetProperty(KeyValue.Key);
                Property = Property != null ? Property : ModelType.GetProperty(pascalCase);
                if (Property != null && isBlazorId.ToString() == "False")
                {
                    if (IsCollection(Property.PropertyType, KeyValue.Value))
                    {
                        string CollectionValue = JsonConvert.SerializeObject(KeyValue.Value);
                        Type dataType = Property.PropertyType.GetTypeInfo();
                        if (!Property.PropertyType.IsGenericType)
                        {
                            dataType = typeof(IEnumerable<object>);
                        }

                        var Value = JsonConvert.DeserializeObject(CollectionValue, dataType);
                        Property.SetValue(ModelInstance, Value);
                    }
                    else if (IsComplexObject(Property.PropertyType, KeyValue.Value))
                    {
                        string ComplexValue = JsonConvert.SerializeObject(KeyValue.Value);
                        Dictionary<string, object> Value = JsonConvert.DeserializeObject<Dictionary<string, object>>(ComplexValue);
                        var Info = GetObject(Value, Property.PropertyType);
                        Property.SetValue(ModelInstance, Info);
                    }
                    else if (Property.CanWrite || Property.SetMethod != null)
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

        internal static bool IsCollection(Type type, object propertyValue)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(List<>)) || type.GetGenericTypeDefinition().Equals(typeof(ICollection<>)) || type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)) || type.IsAssignableFrom(typeof(IEnumerable))) && propertyValue is JArray)
            {
                return true;
            }

            return false;
        }

        internal static bool IsComplexObject(Type type, object propertyValue)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsComplexObject(type.GetGenericArguments()[0].GetTypeInfo(), propertyValue);
            }

            if (propertyValue is JObject)
            {
                return true;
            }

            if (propertyValue == null || propertyValue is string || type.GetTypeInfo().IsPrimitive || propertyValue is long || propertyValue is double || propertyValue is decimal || propertyValue is DateTimeOffset || propertyValue is DateTime || type.GetTypeInfo().IsEnum || typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
            {
                return false;
            }

            return true;
        }

        internal void InitTemplates(string template, object templateParent)
        {
            var BaseComp = templateParent as BaseComponent;
            if (!BaseComp.ChildDotNetObjectRef.ContainsKey(template))
            {
                BaseComp.ChildDotNetObjectRef.Add(template, DotNetObjectReference.Create<object>(this));
            }
            else
            {
                ChildDotNetObjectRef.Add(template, DotNetObjectReference.Create<object>(this));
            }
        }

        internal void InitTemplates(string template, object templateParent, DotNetObjectReference<object> dotnetInstance = null)
        {
            var BaseComp = templateParent as BaseComponent;
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
            await IsScriptRendered();
            if (TemplateData == null && Template != null)
            {
                await SyncfusionInterop.SetTemplateInstance<string>(jsRuntime, TemplateName, dotnetInstance, UniqueId);
            }

            if (TemplateData != null && TemplateClientChanges)
            {
                await SyncfusionInterop.SetTemplates<string>(jsRuntime, TemplateID);
            }
        }

        internal async Task SetTemplates(string TemplateName, string TemplateID, RenderFragment<dynamic> Template, List<Dictionary<string, object>> TemplateData, int UniqueId, bool TemplateClientChanges = true)
        {
            await IsScriptRendered();
            if (TemplateData == null && Template != null)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                await SyncfusionInterop.SetTemplateInstance<string>(jsRuntime, TemplateName, DotNetObjectReference.Create<object>(this), UniqueId);
#pragma warning restore CA2000 // Dispose objects before losing scope
            }

            if (TemplateData != null && TemplateClientChanges)
            {
                await SyncfusionInterop.SetTemplates<string>(jsRuntime, TemplateID);
            }
        }

        internal async Task SetContainerTemplates(string TemplateName, string TemplateID, RenderFragment Template, List<Dictionary<string, object>> TemplateData, int UniqueId)
        {
            await IsScriptRendered();
            if (TemplateData == null && Template != null)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                await SyncfusionInterop.SetTemplateInstance<string>(jsRuntime, TemplateName, DotNetObjectReference.Create<object>(this), UniqueId);
#pragma warning restore CA2000 // Dispose objects before losing scope
            }

            if (TemplateData != null)
            {
                await SyncfusionInterop.SetTemplates<string>(jsRuntime, TemplateID);
            }
        }

        internal async Task SetContainerTemplates(string TemplateName, string TemplateID, RenderFragment Template, List<Dictionary<string, object>> TemplateData, int UniqueId, DotNetObjectReference<object> dotnetInstance = null)
        {
            await IsScriptRendered();
            if (TemplateData == null && Template != null)
            {
                await SyncfusionInterop.SetTemplateInstance<string>(jsRuntime, TemplateName, dotnetInstance, UniqueId);
            }

            if (TemplateData != null)
            {
                await SyncfusionInterop.SetTemplates<string>(jsRuntime, TemplateID);
            }
        }

        internal async Task<bool> IsScriptRendered()
        {
            if ((!SyncfusionService.options.IgnoreScriptIsolation && SyncfusionService.IsScriptRendered) || SyncfusionService.options.IgnoreScriptIsolation)
            {
                return true;
            }
            else
            {
                await Task.Delay(10);
                return await IsScriptRendered();
            }
        }
    }
}

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// An interface for BaseComponent.
    /// </summary>
    public interface IBaseComponent
    {
        public bool IsRendered { get; set; }

        public bool TemplateClientChanges { get; set; }

        public Type ModelType { get; set; }

        public Dictionary<string, object> DataContainer { get; set; }

        public Dictionary<string, object> DataHashTable { get; set; }

        public DataManager DataManager { get; set; }

        public string GetJSNamespace();
    }
}

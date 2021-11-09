using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.Collections;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Data")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Buttons")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Cards")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Spinner")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.BarcodeGenerator")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.CircularGauge")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.LinearGauge")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Notifications")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.SplitButtons")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Layouts")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Popups")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Inputs")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Lists")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Calendars")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Navigations")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Diagrams")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Diagram")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DropDowns")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.RichTextEditor")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.QueryBuilder")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Schedule")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Kanban")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Grids")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.TreeGrid")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Gantt")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.TreeMap")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Charts")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DocumentEditor")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.HeatMap")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.InPlaceEditor")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Maps")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.PdfViewer")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.PivotView")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.ProgressBar")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.RangeNavigator")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.FileManager")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.SmithChart")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Sparkline")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.StockChart")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.BulletChart")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DataVizCommon")]

namespace Syncfusion.Blazor
{
    /// <summary>
    /// A Base Component for all the Syncfusion Blazor UI components to implement the common functionalities.
    /// </summary>
    public abstract class SfBaseComponent : OwningComponentBase
    {
        #region internal properties
        [Inject]
        internal IJSRuntime JSRuntime { get; set; }

        [Inject]
        internal SyncfusionBlazorService SyncfusionService { get; set; }

        internal bool IsRendered { get; set; }

        internal SfScriptModules ScriptModules { get; set; }

        internal Dictionary<string, object> PropertyChanges { get; set; }

        internal Dictionary<string, object> PrevChanges { get; set; }

        internal List<ScriptModules> DependentScripts { get; set; } = new List<ScriptModules>();

        internal DotNetObjectReference<object> DotnetObjectReference { get; set; }

        #endregion

        #region life cycle methods

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            PropertyChanges = new Dictionary<string, object>();
            PrevChanges = new Dictionary<string, object>();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                DotnetObjectReference = DotNetObjectReference.Create<object>(this);
                IsRendered = firstRender;
#if !NETSTANDARD
                // load init-interop script.
                if (!SyncfusionService.options.IgnoreScriptIsolation || SyncfusionService.isNativeDependent)
                {
                    await SfBaseUtils.ImportModule(JSRuntime, SfScriptModules.SfBase, SyncfusionService.ScriptHashKey);
                    await SfBaseUtils.ImportDependencies(JSRuntime, DependentScripts, ScriptModules, SyncfusionService.ScriptHashKey);
                }

                // Notify to the component for the required scripts loaded.
                await OnAfterScriptRendered();
#endif
#if NETSTANDARD
                if (SyncfusionService.IsScriptRendered)
                {
                    if (!SyncfusionService.options.IgnoreScriptIsolation || SyncfusionService.isNativeDependent)
                    {
                        await SfBaseUtils.ImportDependencies(JSRuntime, DependentScripts, ScriptModules, SyncfusionService.ScriptHashKey);
                    }

                    // Notify to the component for the required scripts loaded.
                    await OnAfterScriptRendered();
                }
                else
                {
                    var scriptDependency = new ScriptDependencies(JSRuntime, DependentScripts, ScriptModules, SyncfusionService.ScriptHashKey);
                    scriptDependency.OnAfterScriptRendered = InvokeScriptRendered;
                    SyncfusionService.ScriptDependencies.Add(scriptDependency);
                }
#endif
            }

            PropertyChanges?.Clear();
            PrevChanges?.Clear();
        }
        #endregion

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
                DotnetObjectReference?.Dispose();
                PropertyChanges?.Clear();
                DependentScripts?.Clear();
                ComponentDispose();
            }
        }

        /// <summary>
        /// The virtual method to override the Dispose method at component side.
        /// </summary>
        internal virtual void ComponentDispose()
        {
            // Dispose component objects here.
        }

        /// <summary>
        /// Notify the component about the required scripts are rendered in the web page.
        /// </summary>
        internal virtual async Task OnAfterScriptRendered()
        {
            await Task.CompletedTask;
        }

#if NETSTANDARD
        /// <summary>
        /// Invoke method for ScriptDependencies class to notify script rendered in web page.
        /// </summary>
        internal async void InvokeScriptRendered()
        {
            await OnAfterScriptRendered();
        }
#endif

        /// <summary>
        /// Invokes JSInterop for void return type methods.
        /// </summary>
        internal async Task InvokeMethod(string methodName, params object[] methodParams)
        {
            await SfBaseUtils.InvokeMethod(JSRuntime, methodName, methodParams);
        }

        /// <summary>
        /// Invokes JSInterop for object return type methods.
        /// </summary>
        internal async Task<T> InvokeMethod<T>(string methodName, bool isObjectReturnType, params object[] methodParams)
        {
            if (!isObjectReturnType)
            {
                // The return type of invoke method doesn't need to be serialize such as int, string, double, float etc...
                return await SfBaseUtils.InvokeMethod<T>(JSRuntime, methodName, methodParams);
            }
            else
            {
                // The return type of invoke method needs to be serialize to get its actual result.
                string returnValue = await SfBaseUtils.InvokeMethod<string>(JSRuntime, methodName, methodParams);
                T result = default(T);
                if (returnValue != null)
                {
                    result = JsonConvert.DeserializeObject<T>(returnValue, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                }

                return result;
            }
        }

        /// <summary>
        /// Notify the property value changes and it will be tracked in the PropertyChanges.
        /// </summary>
        /// <param name="propertyName">Name of the property needs to be compared.</param>
        /// <param name="publicValue">Public value of the property.</param>
        /// <param name="privateValue">Private value of the property.</param>
		/// <param name="updatePrevProps">Private value of the Previous property</param>
        /// <returns>Returns T.</returns>
        internal T NotifyPropertyChanges<T>(string propertyName, T publicValue, T privateValue, bool updatePrevProps = false)
        {
            if (!SfBaseUtils.Equals(publicValue, privateValue))
            {
                SfBaseUtils.UpdateDictionary(propertyName, publicValue, PropertyChanges);
            }
             if (updatePrevProps) 
             {
                 SfBaseUtils.UpdateDictionary(propertyName, privateValue, PrevChanges);
             }

            return publicValue;
        }

        /// <summary>
        /// Wire or unwire observable events to a specific collection object.
        /// </summary>
        /// <param name="propertyName">ObservableCollection property name to track it in PropertyChanges.</param>
        /// <param name="dataValue">A ObservableCollection data object to bind or unbind the events.</param>
        /// <param name="unwire">Set true to unwire observable events to the ObservableCollection object.</param>
        internal void UpdateObservableEventsForObject(string propertyName, object dataValue, bool unwire = false)
        {
            if (dataValue is IEnumerable)
            {
                UpdateObservableEvents(propertyName, dataValue as IEnumerable, unwire);
            }
        }

        /// <summary>
        /// Wire or unwire observable events to a specific ObservableCollection object.
        /// </summary>
        /// <param name="propertyName">ObservableCollection property name to track it in PropertyChanges.</param>
        /// <param name="dataValue">A ObservableCollection data object to bind or unbind the events.</param>
        /// <param name="unwire">Set true to unwire observable events to the ObservableCollection object.</param>
        internal void UpdateObservableEvents(string propertyName, IEnumerable dataValue, bool unwire = false)
        {
            if (dataValue == null)
                return;

            var dataValueColl = dataValue as INotifyCollectionChanged;
            if (dataValueColl != null)
            {
                if (!unwire)
                {
                    dataValueColl.CollectionChanged += (sender, e) => ObservableCollectionChanged(propertyName, sender, e);
                }
                else
                {
                    dataValueColl.CollectionChanged -= (sender, e) => ObservableCollectionChanged(propertyName, sender, e);
                }
            }

            var propertyChanged = dataValue as INotifyPropertyChanged;
            if (propertyChanged != null)
            {
                var enumerator = dataValue.GetEnumerator();
                if (enumerator == null || !enumerator.MoveNext())
                    return;

                var fistItemPropertyChanged = enumerator.Current as INotifyPropertyChanged;
                if (fistItemPropertyChanged == null)
                    return;

                if (!unwire)
                {
                    foreach (var item in dataValue)
                    {
                        ((INotifyPropertyChanged)item).PropertyChanged += ObservablePropertyChanged;
                    }
                }
                else
                {
                    foreach (var item in dataValue)
                    {
                        ((INotifyPropertyChanged)item).PropertyChanged -= ObservablePropertyChanged;
                    }
                }
            }
        }

        /// <summary>
        /// INotifyCollectionChanged event handler method to track the changes.
        /// </summary>
        private void ObservableCollectionChanged(string propertyName, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var propertyChanged = item as INotifyPropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged.PropertyChanged -= ObservablePropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var propertyChanged = item as INotifyPropertyChanged;
                    if (propertyChanged != null)
                    {
                        propertyChanged.PropertyChanged += ObservablePropertyChanged;
                    }
                }
            }

            SfBaseUtils.UpdateDictionary(propertyName, sender, PropertyChanges);
            OnObservableChange(propertyName, sender, true, e);
        }

        /// <summary>
        /// INotifyPropertyChanged event handler method to track the changes.
        /// </summary>
        private void ObservablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SfBaseUtils.UpdateDictionary(e.PropertyName, sender, PropertyChanges);
            SfBaseUtils.UpdateDictionary(e.PropertyName + "EventArgs", e, PropertyChanges);
            OnObservableChange(e.PropertyName, sender);
        }

        /// <summary>
        /// Overridable Method for INotifyCollectionChanged event handler to track the changes.
        /// </summary>
        /// <param name="propertyName">Observable property name.</param>
        /// <param name="sender">Observable model object.</param>
        /// <param name="isCollectionChanged">Sets true if the observable collection changed.</param>
        /// <param name="e">Changed Event Args</param>
        protected virtual void OnObservableChange(string propertyName, object sender, bool isCollectionChanged = false, NotifyCollectionChangedEventArgs e = null)
        {
            // Implement component logic here for observable changes.
        }
    }

#if NETSTANDARD
    /// <summary>
    /// ScriptDependencies class to load the dependent scripts in load time.
    /// </summary>
    internal class ScriptDependencies
    {
        public ScriptDependencies(IJSRuntime jsRuntime, List<ScriptModules> dependentScripts, SfScriptModules scriptModules, string hashKey)
        {
            DependentScripts = dependentScripts;
            ScriptModules = scriptModules;
            JSRuntime = jsRuntime;
            HashKey = hashKey;
        }

        public List<ScriptModules> DependentScripts { get; set; }

        public SfScriptModules ScriptModules { get; set; }

        public IJSRuntime JSRuntime { get; set; }

        public string HashKey { get; set; }

        public Action OnAfterScriptRendered { get; set; }

        public async Task ImportScripts()
        {
            await SfBaseUtils.ImportDependencies(JSRuntime, DependentScripts, ScriptModules, HashKey);
            OnAfterScriptRendered();
        }
    }
#endif
}
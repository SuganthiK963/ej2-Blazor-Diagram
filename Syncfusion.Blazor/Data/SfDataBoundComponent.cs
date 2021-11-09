using System;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Gantt")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Grids")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Charts")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.RangeNavigator")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Kanban")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.QueryBuilder")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Schedule")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Maps")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.TreeMap")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.TreeGrid")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DropDowns")]

namespace Syncfusion.Blazor
{
    public abstract class SfDataBoundComponent : SfBaseComponent
    {
        [JsonIgnore]
        public DataManager DataManager { get; set; }

        protected virtual SfBaseComponent MainParent { get; set; }

        internal List<string> directParamKeys { get; set; } = new List<string>();

        internal Dictionary<string, object> DirectParameters { get; set; } = new Dictionary<string, object>();

        internal bool IsRerendering { get; set; }

        /// <exclude/>
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (directParamKeys.Count == 0)
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

        protected object SetDataManager<T>(object dataSource)
        {
            if (dataSource is Syncfusion.Blazor.Data.SfDataManager sf || dataSource is Syncfusion.Blazor.DataManager dm)
            {
                return dataSource;
            }
#pragma warning disable BL0005

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
                DataManager = new DataManager() { Json = Enumerable.Empty<T>().Cast<object>().ToList() };
            }
#pragma warning restore BL0005
            return DataManager;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DirectParameters = new Dictionary<string, object>();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            IsRerendering = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                // Added direct parameters from directParamKeys
                foreach (var key in directParamKeys)
                {
                    DirectParameters = DirectParameters == null ? new Dictionary<string, object>() : DirectParameters;
                    var initValue = GetType().GetProperty(key)?.GetValue(this);
                    SfBaseUtils.UpdateDictionary(key, initValue, DirectParameters);
                }
            }
        }

        internal async Task OnPropertyChanged()
        {
            await OnParametersSetAsync();
        }

        /// <summary>
        /// Processing the property value changes and invoking the events for two-way bindings.
        /// </summary>
        internal async virtual Task<T> UpdateProperty<T>(string propertyName, T publicValue, T privateValue, object eventCallback = null, Expression<Func<T>> expression = null)
        {
            T finalResult = publicValue;
            if (!EqualityComparer<T>.Default.Equals(publicValue, privateValue))
            {
                // Get the direct parameter value
                T directParam = DirectParameters.ContainsKey(propertyName) ? (T)DirectParameters[propertyName] : publicValue;
                var hasEventDelegate = eventCallback != null && ((EventCallback<T>)eventCallback).HasDelegate;
                var isPropertyBinding = !SfBaseUtils.Equals<T>(publicValue, directParam) && IsRerendering;
                var baseComponent = MainParent != null ? MainParent : this;
                var isTwoWayBinding = IsRerendering && hasEventDelegate;

                // Validate and assign public or private values to the property based on changes
                finalResult = (isTwoWayBinding || isPropertyBinding || !baseComponent.IsRendered) ? publicValue : privateValue;

                // Checking eventcallback for two-way notification
                if (hasEventDelegate)
                {
                    var eventMethod = (EventCallback<T>)eventCallback;
                    await eventMethod.InvokeAsync(finalResult);
                }

                if (isPropertyBinding)
                {
                    DirectParameters[propertyName] = finalResult;
                    SfBaseUtils.UpdateDictionary(propertyName, finalResult, PropertyChanges);
                }
            }

            return finalResult;
        }

        internal override void ComponentDispose()
        {
            DirectParameters?.Clear();
            if (DataManager?.Json != null)
            {
                UpdateObservableEvents("DataSource", DataManager.Json, true);
            }

            DataManager?.Dispose();
        }
    }
}

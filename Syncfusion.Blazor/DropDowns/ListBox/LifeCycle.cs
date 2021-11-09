using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Text.Json;
using System.Collections.Generic;


namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// ListBox component used to display a list of items. Users can select one or more items in the list using a checkbox or by keyboard selection.
    /// It supports sorting, grouping, reordering and drag and drop of items.
    /// </summary>
    public partial class SfListBox<TValue, TItem> : SfDropDownBase<TItem>
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            listboxValue = Value;
            cssClass = CssClass;
            enabled = Enabled;
            enableRtl = EnableRtl;
            allowDragAndDrop = AllowDragAndDrop;
            Initialize();
            UpdateSize();
            ScriptModules = SfScriptModules.SfListBox;
            DependentScripts.Add(Blazor.Internal.ScriptModules.Sortable);
            DependentScripts.Add(Blazor.Internal.ScriptModules.Popup);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            cssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            enabled = NotifyPropertyChanges(nameof(Enabled), Enabled, enabled);
            allowDragAndDrop = NotifyPropertyChanges(nameof(AllowDragAndDrop), AllowDragAndDrop, allowDragAndDrop);
            NotifyPropertyChanges(nameof(Value), Value, listboxValue);
            listboxValue = Value = await SfBaseUtils.UpdateProperty(Value, listboxValue, ValueChanged, DropDownsEditContext, ValueExpression);
            if (PropertyChanges.Count > 0)
            {
                for (var i = 0; i < PropertyChanges.Keys.Count; i++)
                {
                    string key = PropertyChanges.Keys.ElementAt(i);
                    if (key == nameof(SortOrder))
                    {
                        await RenderItems();
                        await UpdateSelectedValue(GetDataByValue(Value));
                    }
                    else if (key == nameof(Value))
                    {
                        await UpdateSelectedValue(GetDataByValue(Value));
                    }
                    else if (key == nameof(DataSource) || key == nameof(Query))
                    {
                        await UpdateDataSource();
                    }
                    else if (key == nameof(CssClass) || key == nameof(EnableRtl) || key == nameof(Enabled))
                    {
                        Initialize();
                    }
                    else if (key == nameof(AllowDragAndDrop))
                    {
                        await InvokeMethod(PROPERTYCHANGED, element, AllowDragAndDrop);
                    }
                }
            }
            UpdateHTMLAttributes();
        }

        private void UpdateHTMLAttributes()
        {
            if (this.HtmlAttributes != null)
            {
                foreach (var item in (Dictionary<string, object>)this.HtmlAttributes)
                {
                    if (item.Key == "id")
                    {
                        id = item.Value?.ToString();
                    }
                }
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (Fields == null)
                {
                    SetFields();
                }

                if (selectionSettings == null)
                {
                    selectionSettings = new ListBoxSelectionSettings();
                }

                if (ListData == null || !ListData.Any())
                {
                    await UpdateDataSource();
                }

                await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = CREATED });
                await InvokeMethod(INITIALIZE, element, Scope, AllowDragAndDrop, DotnetObjectReference);
                await Task.Yield();
                await GetScopedListbox(Scope);
                if (Scope != null && toolbarSettings != null)
                {
					if (scopeListbox != null )
					{
						scopeListbox.scopeListbox = this;
						StateHasChanged();
					}
                    scopeListbox.scopeListbox = this;
                    StateHasChanged();
                }

                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { id });
                    localStorageValue = string.IsNullOrEmpty(localStorageValue) ? null : localStorageValue;
                    if (localStorageValue != null)
                    {
                        var persistValue = JsonSerializer.Deserialize<string[]>(localStorageValue);
                        TValue ValueColl = (TValue)SfBaseUtils.ChangeType(persistValue, typeof(TValue));
                        listboxValue = Value = await SfBaseUtils.UpdateProperty(ValueColl, listboxValue, ValueChanged, DropDownsEditContext, ValueExpression);
                        await UpdateSelectedValue(GetDataByValue(Value));
                        StateHasChanged();
                    }
                }
            }
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                InvokeMethod(DESTROY, null).ContinueWith(t =>
                {
                    SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, null).GetAwaiter();
                }, TaskScheduler.Current);
            }
        }
    }
}
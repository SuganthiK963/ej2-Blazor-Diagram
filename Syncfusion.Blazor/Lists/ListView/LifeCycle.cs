using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using Syncfusion.Blazor.Lists.Internal;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// Component for executing complex list items common functionalities.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public partial class SfListView<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            optionsInternal = new ListBaseOptionModel<TValue>() { ShowIcon = ShowIcon, ShowCheckBox = ShowCheckBox, ExpandCollapse = true, AriaAttributes = ariaAttributes };
            ID = !string.IsNullOrEmpty(ID) ? ID : SfBaseUtils.GenerateID(IDPREFIX);
            ScriptModules = SfScriptModules.SfListView;
            UpdateListViewDataSource();
            if (EnableVirtualization)
            {
                UpdateVirtualIndex();
            }

            UpdateChildProperties("fields", ListFields);
            ListQuery = Query;
            ListDataSource = DataSource;
            ListCssClass = CssClass;
            ListEnableRtl = EnableRtl;
            ListEnabled = Enabled;
            ListSortOrder = SortOrder;
            ListHeaderTitle = HeaderTitle;
            ListShowCheckBox = ShowCheckBox;
            ShowListIcon = ShowIcon;
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            ListQuery = NotifyPropertyChanges(nameof(Query), Query, ListQuery);
            ListDataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, ListDataSource);
            ListEnableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, ListEnableRtl);
            ListCssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, ListCssClass);
            ListEnabled = NotifyPropertyChanges(nameof(Enabled), Enabled, ListEnabled);
            ListSortOrder = NotifyPropertyChanges(nameof(SortOrder), SortOrder, ListSortOrder);
            ListHeaderTitle = NotifyPropertyChanges(nameof(HeaderTitle), HeaderTitle, ListHeaderTitle);
            ListShowCheckBox = NotifyPropertyChanges(nameof(ShowCheckBox), ShowCheckBox, ListShowCheckBox);
            ShowListIcon = NotifyPropertyChanges(nameof(ShowIcon), ShowIcon, ShowListIcon);
            if (!AfterUpdateData) {
                await DyanamicPropertyUpdate();
            }
            AfterUpdateData = false;
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>Task.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if ((firstRender || QueryUpdated) && !isSelection)
            {
                // Fetch the data from Data Manager
                await UpdateAfterRenderDataSource();
                if (firstRender && ListViewEvents != null && ListViewEvents.Created.HasDelegate)
                {
                    await SfBaseUtils.InvokeEvent<object>(ListViewEvents.Created, null);
                }
            }
            isSelection = false;
            if (isDSUpdated)
            {
                isDSUpdated = false;
                ListElementDetails<TValue> elementDetails = GetLiElementData(new ListElementReference() { Key = DATASOURCEKEY }, true);
                await InvokeMethod("sfBlazor.ListView.setCheckedItems", ElementRef, elementDetails.Id != null ? elementDetails.Id : new List<string>());
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            await InvokeMethod("sfBlazor.ListView.initialize", ElementRef, DotnetObjectReference, GetProperties(), ListViewEvents?.Clicked.HasDelegate, viewPortElementCount / VIRTUALSCROLLDIFF, ListDataSource);
            await SfBaseUtils.InvokeEvent<ActionEventsArgs>(ListViewEvents?.OnActionBegin, new ActionEventsArgs() { Name = "ActionBegin" });
        }
    }
}
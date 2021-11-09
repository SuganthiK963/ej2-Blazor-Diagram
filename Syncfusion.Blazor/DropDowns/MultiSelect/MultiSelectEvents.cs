using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Specifies the MultiSelect Events of the component.
    /// </summary>
    /// <typeparam name="TValue">Specifies the value type.</typeparam>
    /// <typeparam name="TItem">Specifies the type of MultiSelectEvents.</typeparam>
    public partial class MultiSelectEvents<TValue, TItem> : SfBaseComponent
    {
        [CascadingParameter]
        private SfMultiSelect<TValue, TItem> BaseParent { get; set; }

        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [Parameter]
        public EventCallback<ActionBeginEventArgs> OnActionBegin { get; set; }

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [Parameter]
        public EventCallback<ActionCompleteEventArgs<TItem>> OnActionComplete { get; set; }

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [Parameter]
        public EventCallback<Exception> OnActionFailure { get; set; }

        /// <summary>
        /// Fires when popup opens before animation.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Event triggers when the input get focus-out.
        /// </summary>
        [Parameter]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// Fires each time when selection changes happened in list items after model and input value get affected.
        /// </summary>
        [Parameter]
        public EventCallback<MultiSelectChangeEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Event triggers when the chip selection.
        /// </summary>
        [Parameter]
        public EventCallback<ChipSelectedEventArgs<TItem>> ChipSelected { get; set; }

        /// <summary>
        /// Triggers before the popup is closed. If you cancel this event, the popup remains opened.
        /// </summary>
        [Parameter]
        public EventCallback<PopupEventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the CustomValue is selected.
        /// </summary>
        [Parameter]
        public EventCallback<CustomValueEventArgs<TItem>> CustomValueSpecifier { get; set; }

        /// <summary>
        /// Triggers when data source is populated in the popup list.
        /// </summary>
        [Parameter]
        public EventCallback<DataBoundEventArgs> DataBound { get; set; }

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers event,when user types a text in search box.
        /// </summary>
        [Parameter]
        public EventCallback<FilteringEventArgs> Filtering { get; set; }

        /// <summary>
        /// Event triggers when the input get focused.
        /// </summary>
        [Parameter]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Fires when popup opens after animation completion.
        /// </summary>
        [Parameter]
        public EventCallback<PopupEventArgs> Opened { get; set; }

        /// <summary>
        /// Triggers after the popup has been closed.
        /// </summary>
        [Parameter]
        public EventCallback<ClosedEventArgs> Closed { get; set; }

        /// <summary>
        /// Fires after the selected item removed from the widget.
        /// </summary>
        [Parameter]
        public EventCallback<RemoveEventArgs<TItem>> ValueRemoved { get; set; }

        /// <summary>
        /// Fires before the selected item removed from the widget.
        /// </summary>
        [Parameter]
        public EventCallback<RemoveEventArgs<TItem>> OnValueRemove { get; set; }

        /// <summary>
        /// Triggers when an item in the popup is selected by the user either with mouse/tap or with keyboard navigation.
        /// </summary>
        [Parameter]
        public EventCallback<SelectEventArgs<TItem>> OnValueSelect { get; set; }

        /// <summary>
        /// Fires after select all process completion.
        /// </summary>
        [Parameter]
        public EventCallback<SelectAllEventArgs<TItem>> SelectedAll { get; set; }

        /// <summary>
        /// Fires after cleared all item using clear icon.
        /// </summary>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.MouseEventArgs> Cleared { get; set; }

        /// <summary>
        /// Fires before set the selected item as chip in the component.
        /// </summary>
        [Parameter]
        public EventCallback<TaggingEventArgs<TItem>> OnChipTag { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.MultiselectEvents = this;
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }
    }
}

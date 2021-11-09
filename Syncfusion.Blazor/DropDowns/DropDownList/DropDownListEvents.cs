using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Specifies the DropDownList Events of the component.
    /// </summary>
    /// <typeparam name="TValue">Specifies the value type.</typeparam>
    /// <typeparam name="TItem">Specifies the type of SfDropDownList.</typeparam>
    public partial class DropDownListEvents<TValue, TItem> : SfBaseComponent
    {
        /// <summary>
        /// Specifies the base parent of the component.
        /// </summary>
        [CascadingParameter]
        protected SfDropDownList<TValue, TItem> BaseParent { get; set; }

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
        /// Triggers when the popup before opens.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Triggers when focus moves out from the component.
        /// </summary>
        [Parameter]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// <para>Triggers when an item in a popup is selected or when the model value is changed by user.</para>
        /// <para>Use Change event to configure the cascading DropDownList.</para>
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs<TValue, TItem>> ValueChange { get; set; }

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
        /// Triggers on typing a character in the filter bar when the AllowFiltering is enabled.
        /// </summary>
        [Parameter]
        public EventCallback<FilteringEventArgs> Filtering { get; set; }

        /// <summary>
        /// Triggers when the component is focused.
        /// </summary>
        [Parameter]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers when the popup opens.
        /// </summary>
        [Parameter]
        public EventCallback<PopupEventArgs> Opened { get; set; }

        /// <summary>
        /// Triggers after the popup has been closed.
        /// </summary>
        [Parameter]
        public EventCallback<ClosedEventArgs> Closed { get; set; }

        /// <summary>
        /// Triggers when an item in the popup is selected by the user either with mouse/tap or with keyboard navigation.
        /// </summary>
        [Parameter]
        public EventCallback<SelectEventArgs<TItem>> OnValueSelect { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.DropdownlistEvents = this;
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }
    }
}

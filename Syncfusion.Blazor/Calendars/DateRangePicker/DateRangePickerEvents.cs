namespace Syncfusion.Blazor.Calendars
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
    using Syncfusion.Blazor;
    using Syncfusion.Blazor.Calendars;

    /// <summary>
    /// Specifies the DateRangePicker Events of the component.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of DateRangePickerEvents.</typeparam>
    public class DateRangePickerEvents<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        [Parameter]
        public EventCallback<BlurEventArgs> Blur { get; set; }

        /// <summary>
        /// Triggers when the Calendar value is changed.
        /// </summary>
        [Parameter]
        public EventCallback<RangePickerEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Triggers when daterangepicker value is cleared using clear button.
        /// </summary>
        [Parameter]
        public EventCallback<ClearedEventArgs> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [Parameter]
        public EventCallback<RangePopupEventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggers when DateRangePicker is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when DateRangePicker is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        ///  Triggers when the input gets focus.
        /// </summary>
        [Parameter]
        public EventCallback<FocusEventArgs> Focus { get; set; }

        /// <summary>
        /// Triggers when the Calendar is navigated to another view or within the same level of view.
        /// </summary>
        [Parameter]
        public EventCallback<NavigatedEventArgs> Navigated { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [Parameter]
        public EventCallback<RangePopupEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Triggers when each day cell of the Calendar is rendered.
        /// </summary>
        [Parameter]
        public EventCallback<RenderDayCellEventArgs> OnRenderDayCell { get; set; }

        /// <summary>
        /// Triggers on selecting the start and end date.
        /// </summary>
        [Parameter]
        public EventCallback<RangePickerEventArgs<TValue>> RangeSelected { get; set; }

        [CascadingParameter]
        private SfDateRangePicker<TValue> BaseParent { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            BaseParent.DaterangepickerEvents = this;
        }
    }
}
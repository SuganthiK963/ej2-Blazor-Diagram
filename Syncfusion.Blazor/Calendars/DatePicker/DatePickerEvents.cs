using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Specifies the DatePicker Events of the component.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of DatePickerEvents.</typeparam>
    public class DatePickerEvents<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the BaseParent.
        /// </summary>
        /// <exclude/>
        [CascadingParameter]
        protected SfDatePicker<TValue> BaseParent { get; set; }

        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        [Parameter]
        public EventCallback<BlurEventArgs> Blur { get; set; }

        /// <summary>
        /// Triggers when the Calendar value is changed.
        /// </summary>
        [Parameter]
        public EventCallback<ChangedEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Triggers after selecting the value from DatePicker.
        /// </summary>
        [Parameter]
        public EventCallback<SelectedEventArgs<TValue>> Selected { get; set; }

        /// <summary>
        /// Triggers when datepicker value is cleared using clear button.
        /// </summary>
        [Parameter]
        public EventCallback<ClearedEventArgs> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [Parameter]
        public EventCallback<PopupObjectArgs> OnClose { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        [Parameter]
        public EventCallback<FocusEventArgs> Focus { get; set; }

        /// <summary>
        /// Triggers when the Calendar is navigated to another level or within the same level of view.
        /// </summary>
        [Parameter]
        public EventCallback<NavigatedEventArgs> Navigated { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [Parameter]
        public EventCallback<PopupObjectArgs> OnOpen { get; set; }

        /// <summary>
        /// Triggers when each day cell of the Calendar is rendered.
        /// </summary>
        [Parameter]
        public EventCallback<RenderDayCellEventArgs> OnRenderDayCell { get; set; }

        /// <summary>
        /// Triggers while initial rendring of the component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.DatepickerEvents = this;
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Specifies the DateTimePicker Events of the component.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of DateTimePickerEvents.</typeparam>
    public class DateTimePickerEvents<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        private SfDateTimePicker<TValue> BaseParent { get; set; }

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
        /// Triggers after selecting the value from DatePicker and TimePicker.
        /// </summary>
        [Parameter]
        public EventCallback<SelectedEventArgs<TValue>> Selected { get; set; }

        /// <summary>
        /// Triggers when dateptimeicker value is cleared using clear button.
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
        /// Triggers while rendering the each popup list item.
        /// </summary>
        public EventCallback<ItemEventArgs<TValue>> OnItemRender { get; set; }

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
        /// Triggers when the component is initially rendered.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            BaseParent.DatetimepickerEvents = this;
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }
    }
}

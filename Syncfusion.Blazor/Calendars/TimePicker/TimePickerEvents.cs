using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Specifies the TimePicker Events of the component.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of TimePickerEvents.</typeparam>
    public class TimePickerEvents<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Triggers when the control loses the focus.
        /// </summary>
        /// <exclude/>
        [CascadingParameter]
        protected SfTimePicker<TValue> BaseParent { get; set; }

        /// <summary>
        /// Triggers when the control loses the focus.
        /// </summary>
        [Parameter]
        public EventCallback<BlurEventArgs> Blur { get; set; }

        /// <summary>
        /// Triggers when the value is changed.
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Triggers after selecting the value from TimePicker.
        /// </summary>
        [Parameter]
        public EventCallback<SelectedEventArgs<TValue>> Selected { get; set; }

        /// <summary>
        /// Triggers when timepicker value is cleared using clear button.
        /// </summary>
        [Parameter]
        public EventCallback<ClearedEventArgs> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [Parameter]
        public EventCallback<PopupEventArgs> OnClose { get; set; }

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
        /// Triggers when the control gets focused.
        /// </summary>
        [Parameter]
        public EventCallback<FocusEventArgs> Focus { get; set; }

        /// <summary>
        /// Triggers while rendering the each popup list item.
        /// </summary>
        [Parameter]
        public EventCallback<ItemEventArgs<TValue>> OnItemRender { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [Parameter]
        public EventCallback<PopupEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.TimepickerEvents = this;
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }
    }
}

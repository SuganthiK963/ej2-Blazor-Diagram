using Syncfusion.Blazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Specifies the Calendar Events of the component.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of CalendarEvents.</typeparam>
    public class CalendarEvents<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        private SfCalendar<TValue> BaseParent { get; set; }

        /// <summary>
        /// Triggers when the Calendar value is changed.
        /// </summary>
        [Parameter]
        public EventCallback<ChangedEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Triggers after selecting the value from Calendar.
        /// </summary>
        [Parameter]
        public EventCallback<SelectedEventArgs<TValue>> Selected { get; set; }

        /// <summary>
        /// Triggers after deselecting the value from Calendar. This event will trigger when enable the multiple date selection.
        /// </summary>
        [Parameter]
        public EventCallback<DeSelectedEventArgs<TValue>> DeSelected { get; set; }

        /// <summary>
        /// Triggers when Calendar is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when Calendar is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the Calendar is navigated to another level or within the same level of view.
        /// </summary>
        [Parameter]
        public EventCallback<NavigatedEventArgs> Navigated { get; set; }

        /// <summary>
        /// Triggers when each day cell of the Calendar is rendered.
        /// </summary>
        [Parameter]
        public EventCallback<RenderDayCellEventArgs> OnRenderDayCell { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the calendar component.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            BaseParent.CalendarEvents = this;
        }
    }
}

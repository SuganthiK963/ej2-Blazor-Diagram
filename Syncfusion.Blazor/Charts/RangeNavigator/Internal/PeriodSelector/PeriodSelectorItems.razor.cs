using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Calendars;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.RangeNavigator.Internal
{
    /// <summary>
    /// Specifies the period selector for range navigator.
    /// </summary>
    public partial class PeriodSelectorItems
    {
        private SfToolbar toolbar;
        private ElementReference divElement;

        internal SfDateRangePicker<DateTime?> RangePicker { get; set; }

        /// <summary>
        /// Specifies the toolbar item for period selector.
        /// </summary>
        [Parameter]
        public List<ToolbarItem> RangeToolbarItems { get; set; } = new List<ToolbarItem>();

        /// <summary>
        /// Specifies the minimum value for period selector.
        /// </summary>
        [Parameter]
        public DateTime Min { get; set; }

        /// <summary>
        /// Specifies the maximum value for period selector.
        /// </summary>
        [Parameter]
        public DateTime Max { get; set; }

        /// <summary>
        /// Specifies the start date for period selector.
        /// </summary>
        [Parameter]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Specifies the end date for period selector.
        /// </summary>
        [Parameter]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Specifies the height for period selector.
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        [CascadingParameter]
        internal SfRangeNavigator RangeNavigator { get; set; }

        [Parameter]
        public string Content { get; set; } = "Date Range";

        private static void OnCreated()
        {
            PeriodSelector.OnCreated();
        }

        internal void DateRangeStartEnd(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            InvokeAsync(StateHasChanged);
        }

        private async Task OnToolbarClick(ClickEventArgs args)
        {
            await RangeNavigator?.PeriodSelectorModule?.OnToolbarClick(args);
        }

        private void OnToolbarCreated()
        {
            RangeNavigator?.PeriodSelectorModule?.OnToolbarCreated();
        }

        private async Task OnChanged(RangePickerEventArgs<DateTime?> args)
        {
            await RangeNavigator?.PeriodSelectorModule?.OnChanged(args);
        }

        private async Task RangeClick()
        {
            await RangePicker.Show();
            await InvokeAsync(StateHasChanged);
        }

        internal void Dispose()
        {
            RangePicker = null;
            toolbar = null;
            RangeToolbarItems = null;
            RangeNavigator = null;
        }
    }
}
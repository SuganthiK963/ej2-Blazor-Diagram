using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Range Navigator events customization are mentioned through this.
    /// </summary>
    public class RangeNavigatorEvents : SfBaseComponent
    {
        [CascadingParameter]
        internal SfRangeNavigator RangeNavigator { get; set; }

        /// <summary>
        /// Triggers after the range navigator loaded.
        /// </summary>
        [Parameter]
        public EventCallback<RangeLoadedEventArgs> Loaded { get; set; }

        /// <summary>
        /// Triggers after resizing of chart.
        /// </summary>
        [Parameter]
        public EventCallback<RangeResizeEventArgs> Resized { get; set; }

        /// <summary>
        /// Triggers before the tooltip for series is rendered.
        /// </summary>
        [Parameter]
        public EventCallback<RangeTooltipRenderEventArgs> TooltipRender { get; set; }

        /// <summary>
        /// Triggers for each label renderig.
        /// </summary>
        [Parameter]
        public Action<RangeLabelRenderEventArgs> LabelRender { get; set; }

        /// <summary>
        /// Triggers value changed when start and end value for range navigator.
        /// </summary>
        [Parameter]
        public Action<ChangedEventArgs> Changed { get; set; }

        /// <summary>
        /// Triggers before  print.
        /// </summary>
        [Parameter]
        public EventCallback<EventArgs> OnPrintCompleted { get; set; }

        /// <summary>
        /// Triggers value changed.
        /// </summary>
        [Parameter]
        public Action<RangeSelectorRenderEventArgs> SelectorRender { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            RangeNavigator.RangeNavigatorEvents = this;
        }

        internal override void ComponentDispose()
        {
            RangeNavigator = null;
        }
    }
}
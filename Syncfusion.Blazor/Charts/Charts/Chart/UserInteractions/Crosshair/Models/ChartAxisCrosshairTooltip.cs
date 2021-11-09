using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the axis's crosshair tooltip.
    /// </summary>
    public partial class ChartAxisCrosshairTooltip : ChartSubComponent
    {
        [CascadingParameter]
        private ChartAxis Axis { get; set; }

        /// <summary>
        /// If set to true, crosshair ToolTip will be visible.
        /// @default false.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// The fill color of the ToolTip accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Options to customize the crosshair ToolTip text.
        /// </summary>
        [Parameter]
        public ChartCrosshairTextStyle TextStyle { get; set; } = new ChartCrosshairTextStyle();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Axis = (ChartAxis)Tracker;
            Axis.UpdateAxisProperties("CrosshairTooltip", this);
        }

        internal void UpdateChildProperties(string key, object item)
        {
            if (key == nameof(TextStyle))
            {
                TextStyle = (ChartCrosshairTextStyle)item;
            }
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Axis = null;
            ChildContent = null;
            TextStyle = null;
        }
    }
}
using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the crosshair for the charts.
    /// </summary>
    public class ChartCrosshairSettings : ChartSubComponent
    {
        [CascadingParameter]
        private SfChart Chart { get; set; }

        /// <summary>
        /// Specifies the dashArray for crosshair.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary>
        /// If set to true, crosshair line becomes visible.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// Options to customize the crosshair line.
        /// </summary>
        [Parameter]
        public ChartCrosshairLine Line { get; set; } = new ChartCrosshairLine();

        /// <summary>
        /// Specifies the line type. Horizontal mode enables the horizontal line and Vertical mode enables the vertical line. They are,
        /// None: Hides both vertical and horizontal crosshair lines.
        /// Both: Shows both vertical and horizontal crosshair lines.
        /// Vertical: Shows the vertical line.
        /// Horizontal: Shows the horizontal line.
        /// </summary>
        [Parameter]
        public LineType LineType { get; set; } = LineType.Both;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Chart.Crosshair = this;
        }

        internal void UpdateCrosshairProperties(string key, object keyValue)
        {
            if (key == nameof(Line))
            {
                Line = (ChartCrosshairLine)keyValue;
            }
        }

        internal override void ComponentDispose()
        {
            Chart = null;
            ChildContent = null;
        }
    }
}
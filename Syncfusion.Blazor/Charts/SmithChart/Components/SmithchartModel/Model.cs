using Syncfusion.Blazor.Charts.SmithChart.Internal;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifying the smith chart data points.
    /// </summary>
    public class SmithChartPoint
    {
        /// <summary>
        /// Specifying the reactance to the series data.
        /// </summary>
        public double Reactance { get; set; }

        /// <summary>
        /// Specifying the resistance to the series data.
        /// </summary>
        public double Resistance { get; set; }

        /// <summary>
        /// Specifying the tooltip mapping name to series tooltip.
        /// </summary>
        public string Tooltip { get; set; }

        internal List<string> TemplateID { get; set; } = new List<string>();

        internal List<Size> TemplateSize { get; set; } = new List<Size>();
    }
}
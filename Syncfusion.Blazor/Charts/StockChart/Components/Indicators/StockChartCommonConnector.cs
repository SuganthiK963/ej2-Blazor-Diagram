using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart indicator connector line.
    /// </summary>
    public class StockChartCommonConnector : SfBaseComponent
    {
        /// <summary>
        /// Color of the connector line.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// dashArray of the connector line.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary>
        /// Length of the connector line in pixels.
        /// </summary>
        [Parameter]
        public string Length { get; set; }

        /// <summary>
        /// specifies the type of the connector line. They are
        ///  Smooth
        ///  Line.
        /// </summary>
        [Parameter]
        public ConnectorType Type { get; set; } = ConnectorType.Line;

        /// <summary>
        /// Width of the connector line in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;
    }
}
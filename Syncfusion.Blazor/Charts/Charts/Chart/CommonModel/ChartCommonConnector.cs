using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the connector line style.
    /// </summary>
#pragma warning disable CA1063
    public partial class ChartDefaultConnector : ChartSubComponent, IDisposable
#pragma warning restore CA1063
    {
        /// <summary>
        /// Specifies the type of the connector line.
        /// </summary>
        [Parameter]
        public ConnectorType Type { get; set; } = ConnectorType.Line;

        /// <summary>
        /// Specifies the color of the connector line.
        /// </summary>
        [Parameter]
        public virtual string Color { get; set; }

        /// <summary>
        /// Specifies the width of the connector line.
        /// </summary>
        [Parameter]
        public virtual double Width { get; set; } = 1;

        /// <summary>
        /// Specifies the length of the connector line.
        /// </summary>
        [Parameter]
        public string Length { get; set; }

        /// <summary>
        /// Specifies the dashArray for the connector line.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; }
    }
}
using System;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Options to customize the selected data of the chart component.
    /// </summary>
    public class ChartDefaultSelectedData : ChartSubComponent
    {
        /// <summary>
        /// Sets and gets the series index for the seleceted data.
        /// </summary>
        [Parameter]
        public int Series { get; set; }

        /// <summary>
        /// Sets and gets the point index for the seleceted data.
        /// </summary>
        [Parameter]
        public int Point { get; set; }
    }
}
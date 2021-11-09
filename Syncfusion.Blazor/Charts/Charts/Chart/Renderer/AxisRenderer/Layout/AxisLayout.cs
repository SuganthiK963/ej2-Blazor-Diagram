using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal abstract class AxisLayout
    {
        protected const string SPACE = " ";

        internal bool IsPolar { get; set; }

        internal Size PlotAreaSize { get; set; }

        internal SfChart Chart { get; set; }

        internal Point Offset { get; set; }

        internal Rect SeriesClipRect { get; set; }

        internal double Radius { get; set; }

        protected SvgRendering SvgRenderer { get; set; } = new SvgRendering();

        protected CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        internal abstract void AddAxis(ChartAxis axis);

        internal abstract void RemoveAxis(ChartAxis axis);

        internal abstract void ClearAxes();

        internal abstract void ComputePlotAreaBounds(Rect rect);

        internal abstract void AxisRenderingCalculation(ChartAxisRenderer renderer);
    }
}

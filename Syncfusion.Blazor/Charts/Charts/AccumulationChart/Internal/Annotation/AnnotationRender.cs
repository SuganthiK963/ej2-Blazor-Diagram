using System;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Charts
{
    public partial class AccumulationChartAnnotation
    {
        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal void RenderAnnotationElement(RenderTreeBuilder builder)
        {
            ChartInternalLocation location = CoordinateUnits == Units.Pixel ? SetAnnotationPixelValue() : SetAnnotationPointValue();
            if (!double.IsNaN(location.X) && !double.IsNaN(location.X))
            {
                builder.OpenElement(SvgRendering.Seq++, "div");
                builder.AddAttribute(SvgRendering.Seq++, "id", ChartInstance.ID + "_Annotation_" + ChartInstance.Annotations.IndexOf(this));
                builder.AddAttribute(SvgRendering.Seq++, "style", "transform: translate(-50%, -50%); position: absolute; z-index: 1;" + "top :" + location.Y.ToString(culture) + "px; left:" + location.X.ToString(culture) + "px");
                builder.AddContent(SvgRendering.Seq++, ContentTemplate);
                builder.CloseElement();
            }
        }

        private ChartInternalLocation SetAnnotationPixelValue()
        {
            ChartInternalLocation finalLocation = new ChartInternalLocation(double.NaN, double.NaN);
            Rect result = Region == Regions.Chart ? new Rect(0, 0, ChartInstance.AvailableSize.Width, ChartInstance.AvailableSize.Height) : ChartInstance.VisibleSeries[0].AccumulationBound;
            finalLocation.X = DataVizCommonHelper.StringToNumber(X, result.Width) + result.X;
            finalLocation.Y = DataVizCommonHelper.StringToNumber(Y, result.Height) + result.Y;
            return finalLocation;
        }

        private ChartInternalLocation SetAnnotationPointValue()
        {
            AccumulationPoints resultPoint = null;
            Type pointXType;
            foreach (AccumulationPoints point in ChartInstance.Series[0].Points)
            {
                pointXType = point.X.GetType();
                if ((pointXType == typeof(string) && point.X.ToString() == X && point.Y == long.Parse(Y, null)) ||
                    (pointXType != typeof(string) && Convert.ToDouble(point.X, null) == Convert.ToDouble(X, null) && point.Y == long.Parse(Y, null)))
                {
                    resultPoint = point;
                    break;
                }
            }

            return resultPoint.SymbolLocation;
        }
    }
}
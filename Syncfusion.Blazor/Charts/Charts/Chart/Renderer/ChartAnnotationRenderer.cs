using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartAnnotationRenderer : ChartRenderer, IChartElementRenderer
    {
        private ChartInternalLocation location;

        private object x;

        private string y;

        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal ChartAnnotation Annotation { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.AnnotationContainer.AddRenderer(this);
            Annotation.Renderer = this;
        }

        public void InvalidateRender()
        {
        }

        public void HandleLayoutChange()
        {
        }

        internal void CalculateRenderingOption()
        {
            RendererShouldRender = true;
            x = Annotation.X;
            y = Annotation.Y;
            location = Annotation.CoordinateUnits == Units.Pixel ? SetAnnotationPixelValue() : SetAnnotationPointValue();
        }

        private ChartInternalLocation SetAnnotationPixelValue()
        {
            ChartInternalLocation finalLocation = new ChartInternalLocation(double.NaN, double.NaN);
            Rect result = Annotation.Region == Regions.Chart ? new Rect(0, 0, Owner.InitialRect.Width, Owner.InitialRect.Height) : Owner.AxisContainer.AxisLayout.SeriesClipRect;
            if (result != null)
            {
                finalLocation.X = ChartHelper.StringToNumber(x.ToString(), result.Width) + result.X;
                finalLocation.Y = ChartHelper.StringToNumber(y, result.Height) + result.Y;
            }

            return finalLocation;
        }

        private ChartInternalLocation SetAnnotationPointValue()
        {
            ChartInternalLocation pointLocation = new ChartInternalLocation(double.NaN, double.NaN);
            ChartAxisRenderer chartXAxisRenderer = null, chartYAxisRenderer = null;
            ValueType axisType;
            double pointXValue = double.NaN;
            bool isInverted = Owner.RequireInvertedAxis;
            string label;
            foreach (ChartAxisRenderer axis in Owner.AxisContainer.Renderers)
            {
                if (Annotation.XAxisName == axis.Axis.Name || (Annotation.XAxisName == null && axis.Axis.GetName() == "PrimaryXAxis"))
                {
                    chartXAxisRenderer = axis;
                    axisType = chartXAxisRenderer.Axis.ValueType;
                    if (axisType == ValueType.Category || axisType == ValueType.DateTimeCategory)
                    {
                        label = axisType == ValueType.Category ? Convert.ToString(x, culture) : Convert.ToString(ChartHelper.GetTime(Convert.ToDateTime(x, culture)), culture);
                        pointXValue = Array.IndexOf(chartXAxisRenderer.Labels.ToArray(), label);
                    }
                    else if (axisType == ValueType.DateTime)
                    {
                        pointXValue = ChartHelper.GetTime(Convert.ToDateTime(x, culture));
                    }
                    else
                    {
                        pointXValue = Convert.ToDouble(Convert.ToString(x, culture), null);
                    }
                }
                else if (Annotation.YAxisName == axis.Axis.Name || (Annotation.YAxisName == null && axis.Axis.GetName() == "PrimaryYAxis"))
                {
                    chartYAxisRenderer = axis;
                }
            }

            if (WithInChartArea(chartXAxisRenderer, chartYAxisRenderer, pointXValue))
            {
                pointLocation = ChartHelper.GetPoint(pointXValue, Convert.ToDouble(y, null), chartXAxisRenderer, chartYAxisRenderer);
                pointLocation.X += isInverted ? chartYAxisRenderer.Rect.X : chartXAxisRenderer.Rect.X;
                pointLocation.Y += isInverted ? chartXAxisRenderer.Rect.Y : chartYAxisRenderer.Rect.Y;
            }

            return pointLocation;
        }

        private bool WithInChartArea(ChartAxisRenderer chartXAxisRenderer, ChartAxisRenderer chartYAxisRenderer, double pointXValue)
        {
            return chartXAxisRenderer != null && chartYAxisRenderer != null && ChartHelper.WithIn(chartXAxisRenderer.Axis.ValueType == ValueType.Logarithmic ? ChartHelper.LogBase(pointXValue, chartXAxisRenderer.Axis.LogBase) : pointXValue, chartXAxisRenderer.VisibleRange) && ChartHelper.WithIn(chartYAxisRenderer.Axis.ValueType == ValueType.Logarithmic ? ChartHelper.LogBase(Convert.ToDouble(y, null), chartYAxisRenderer.Axis.LogBase) : Convert.ToDouble(y, null), chartYAxisRenderer.VisibleRange);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (location != null && builder != null)
            {
                int seq = 0;
                if (!double.IsNaN(location.X) && !double.IsNaN(location.Y))
                {
                    string annotationId = Owner.ID + "_Annotation_" + Owner.AnnotationContainer.Renderers.IndexOf(this);
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "id", annotationId);
                    builder.AddAttribute(seq++, "style", "transform: translate(-50%, -50%); position: absolute; z-index: 1;" + "top :" + location.Y.ToString(culture) + "px; left:" + location.X.ToString(culture) + "px");
                    builder.AddContent(seq++, Annotation.ContentTemplate);
                    builder.CloseElement();
                    RendererShouldRender = false;
                }
            }
        }
    }
}

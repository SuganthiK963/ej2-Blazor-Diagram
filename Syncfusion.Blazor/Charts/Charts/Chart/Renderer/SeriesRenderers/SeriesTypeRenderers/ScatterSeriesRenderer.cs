using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class ScatterSeriesRenderer : LineBaseSeriesRenderer
    {
        private List<SymbolOptions> symbolOptions = new List<SymbolOptions>();

        private ChartHelper helper { get; set; } = new ChartHelper();

        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            if (Series.Animation.Enable && Owner.ShouldAnimateSeries)
            {
                AnimationOptions = new AnimationOptions(SeriesElementId(), AnimationType.Marker);
            }
        }

        internal override bool ShouldRenderMarker()
        {
            return false;
        }

        private void CalculateDirection()
        {
            symbolOptions.Clear();
            List<Point> visiblePoints = EnableComplexProperty();
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                ChartInternalLocation startLocation = /*Owner.Redraw && */point.SymbolLocations.Count > 0 ? point.SymbolLocations[0] : null;
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoints[i - 1] : null, point, i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null, XAxisRenderer) && IsWithInChartArea(point))
                {
                    PointRenderEventArgs argsData = new PointRenderEventArgs(Constants.POINTRENDER, false, point, Series, SetPointColor(point, Interior), SetBorderColor(point, new ChartEventBorder { Width = Series.Border.Width, Color = Series.Border.Color }), Series.Marker.Width, Series.Marker.Height, Series.Marker.Shape);
                    if (Owner.ChartEvents?.OnPointRender != null)
                    {
                        Owner.ChartEvents?.OnPointRender.Invoke(argsData);
                    }

                    if (!argsData.Cancel)
                    {
                        point.SymbolLocations.Add(ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis));
                        point.Interior = argsData.Fill;
                        CalculateSeriesElements(point, argsData);
                    }
                    else
                    {
                        point.Marker = new MarkerSettingModel { Visible = true };
                    }
                }
            }
        }

        private void CalculateSeriesElements(Point point, PointRenderEventArgs argsData)
        {
            ChartMarker marker = Series.Marker;
            PathOptions shapeOption = new PathOptions
            {
                Id = Owner.ID + "_Series_" + Index + "_Point_" + point.Index,
                Fill = argsData.Fill,
                StrokeWidth = argsData.Border.Width,
                Stroke = argsData.Border.Color,
                Opacity = Series.Opacity
            };
            shapeOption.Visibility = string.Empty;
            DynamicOptions.PathId.Add(shapeOption.Id);
            SymbolOptions symbol = ChartMarkerRenderer.CalculateSymbol(point.SymbolLocations[0], argsData.Shape.ToString(), new Size(argsData.Width, argsData.Height), marker.ImageUrl, shapeOption, Owner);
            point.Regions.Add(new Rect(point.SymbolLocations[0].X - marker.Width, point.SymbolLocations[0].Y - marker.Height, 2 * marker.Width, 2 * marker.Height));
            point.Marker = new MarkerSettingModel
            {
                Border = argsData.Border,
                Fill = argsData.Fill,
                Height = argsData.Height,
                Visible = true,
                Width = argsData.Width,
                Shape = argsData.Shape
            };
            symbolOptions.Add(symbol);
        }

        private bool IsWithInChartArea(Point point)
        {
            double xvalue = ChartHelper.LogWithIn(point.XValue, XAxisRenderer.Axis),
                yvalue = ChartHelper.LogWithIn(point.YValue, YAxisRenderer.Axis);
            return xvalue >= XAxisRenderer.VisibleRange.Start && xvalue <= XAxisRenderer.VisibleRange.End && yvalue >= YAxisRenderer.VisibleRange.Start && yvalue <= YAxisRenderer.VisibleRange.End;
        }

        protected void RenderSeriesElements(RenderTreeBuilder builder)
        {
            foreach (SymbolOptions symbolOption in symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    Owner.SvgRenderer.RenderEllipse(builder, symbolOption.EllipseOption);
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    Owner.SvgRenderer.RenderPath(builder, symbolOption.PathOption);
                }
                else if (symbolOption.ShapeName == ShapeName.image)
                {
                    Owner.SvgRenderer.RenderImage(builder, symbolOption.ImageOption);
                }
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            RenderSeriesElements(builder);
            builder.CloseElement();
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
            base.UpdateDirection();
        }

        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = Series.Visible;
            if (RendererShouldRender)
            {
                switch (property)
                {
                    case "Fill":
                        UpdateShapeFill();
                        break;
                    case "Color":
                        UpdateShapeColor();
                        break;
                    case "Width":
                        UpdateShapeBorderWidth();
                        break;
                    case "Opacity":
                        UpdateShapeOpacity();
                        break;
                }

                InvalidateRender();
            }
        }

        private void UpdateShapeColor()
        {
            string borderColor = Series.Border.Color;
            foreach (SymbolOptions symbolOption in symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.Stroke = borderColor;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.Stroke = borderColor;
                }
            }
        }

        private void UpdateShapeFill()
        {
            string fill = Interior;
            foreach (SymbolOptions symbolOption in symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.Fill = fill;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.Fill = fill;
                }
            }
        }

        private void UpdateShapeOpacity()
        {
            double opacity = Series.Opacity;
            foreach (SymbolOptions symbolOption in symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.Opacity = opacity;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.Opacity = opacity;
                }
            }
        }

        internal void UpdateShapeBorderWidth()
        {
            double width = Series.Border.Width;
            foreach (SymbolOptions symbolOption in symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.StrokeWidth = width;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.StrokeWidth = width;
                }
            }
        }

        protected override bool IsMarker()
        {
            return false;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            ChartHelper.Dispose();
            helper = null;
        }
    }
}
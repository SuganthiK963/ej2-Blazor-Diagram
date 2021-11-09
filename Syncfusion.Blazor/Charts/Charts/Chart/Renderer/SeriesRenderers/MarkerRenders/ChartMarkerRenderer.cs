using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class ChartMarkerRenderer : ChartRenderer, IChartElementRenderer
    {
        private string seriesIndex;
        private List<SymbolOptions> symbolOptions = new List<SymbolOptions>();

        [Parameter]
        public ChartSeries Series { get; set; }

        internal ChartSeriesRenderer SeriesRenderer { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series.Marker.Renderer = this;
        }

        public void HandleLayoutChange()
        {
            OnLayoutChange();
        }

        protected virtual void OnLayoutChange()
        {
            // ProcessRenderQueue();
        }

        public void InvalidateRender()
        {
            InvokeAsync(StateHasChanged);
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = Series.Marker.Visible;
            SeriesRenderer = Series.Renderer;
            seriesIndex = Convert.ToString(SeriesRenderer.Index, CultureInfo.InvariantCulture);
            if (RendererShouldRender)
            {
                CalculateRenderTreeBuilderOptions();
                SeriesRenderer.CalculateMarkerClipPath();
            }
        }

        protected virtual void CalculateRenderTreeBuilderOptions()
        {
            symbolOptions = new List<SymbolOptions>();
            if (!SeriesRenderer.ShouldRenderMarker() || !SeriesRenderer.Series.Visible)
            {
                return;
            }

            foreach (Point point in SeriesRenderer.Points)
            {
                if (point.Visible && point.SymbolLocations.Count >= 1)
                {
                    foreach (ChartInternalLocation location in point.SymbolLocations)
                    {
                        CalculateSeriesRendererMarkerOptions(SeriesRenderer.Series, point, location, point.SymbolLocations.IndexOf(location));
                    }
                }
            }
        }

        // This method moved and renamed from DrawSymbol in Charthelper
        internal static SymbolOptions CalculateSymbol(ChartInternalLocation location, string shape, Size size, string url, PathOptions option, SfChart chart)
        {
            ChartInternalLocation currentLocation = null;
            if (chart != null && shape == "Circle")
            {
                string[] locations = ChartHelper.AppendTextElements(chart, option.Id, location.X, location.Y, "cx", "cy");
                currentLocation = new ChartInternalLocation(Convert.ToDouble(locations[0], CultureInfo.InvariantCulture), Convert.ToDouble(locations[1], CultureInfo.InvariantCulture));
            }

            SymbolOptions shapeoption = ChartHelper.CalculateShapes(currentLocation != null ? currentLocation : location, size, shape, url, option, false);
            if (shapeoption.ShapeName == ShapeName.path)
            {
                shapeoption.PathOption.Direction = ChartHelper.AppendPathElements(chart, shapeoption.PathOption.Direction, shapeoption.PathOption.Id);
                shapeoption.PathOption.Visibility = option.Visibility;
            }

            if (shapeoption.ShapeName == ShapeName.ellipse)
            {
                shapeoption.EllipseOption.Visibility = option.Visibility;
            }

            if (shapeoption.ShapeName == ShapeName.image)
            {
                shapeoption.ImageOption.Visibility = option.Visibility;
            }

            return shapeoption;
        }

        protected virtual void CalculateSeriesRendererMarkerOptions(ChartSeries series, Point point, ChartInternalLocation location, int index)
        {
            ChartMarker marker = series.Marker;
            ChartEventBorder border = new ChartEventBorder() { Color = marker.Border.Color, Width = marker.Border.Width }, eventBorder;
            string borderColor = marker.Border.Color;
            location.X = location.X + marker.Offset.X;
            location.Y = location.Y - marker.Offset.Y;
            string fill = !string.IsNullOrEmpty(marker.Fill) ? marker.Fill : (series.Type == ChartSeriesType.BoxAndWhisker ? !string.IsNullOrEmpty(point.Interior) ? point.Interior : SeriesRenderer.Interior : "#ffffff");
            SeriesRenderer.DynamicOptions.MarkerParentId = SeriesRenderer.SeriesSymbolId();
            border.Color = !string.IsNullOrEmpty(borderColor) ? borderColor : SeriesRenderer.SetPointColor(point, SeriesRenderer.Interior);
            string symbolId = Owner.ID + "_Series_" + seriesIndex + "_Point_" + point.Index + "_Symbol" + (!double.IsNaN(index) && index != 0 ? Convert.ToString(index, null) : string.Empty);
            eventBorder = new ChartEventBorder
            {
                Color = (series.Type == ChartSeriesType.BoxAndWhisker) ? (!string.IsNullOrEmpty(borderColor) && borderColor != Constants.TRANSPARENT) ? borderColor : ChartHelper.GetSaturationColor(fill, -0.6) : border.Color,
                Width = border.Width
            };
            PointRenderEventArgs argsData = new PointRenderEventArgs(Constants.POINTRENDER, false, point, series, point.IsEmpty ? series.EmptyPointSettings.Fill ?? fill : fill, eventBorder, marker.Width, marker.Height, marker.Shape);
            argsData.Border = SeriesRenderer.SetBorderColor(point, new ChartEventBorder() { Width = argsData.Border.Width, Color = argsData.Border.Color });
            if (Owner.ChartEvents?.OnPointRender != null)
            {
                Owner.ChartEvents.OnPointRender.Invoke(argsData);
            }

            SeriesRenderer.DynamicOptions.IsCircle = argsData.Shape == ChartShape.Circle;
            if (!argsData.Cancel)
            {
                object y = SeriesRenderer.GetMarkerY(point);
                PathOptions shapeOption = new PathOptions(symbolId, string.Empty, string.Empty, argsData.Border.Width, argsData.Border.Color, argsData.Series.Marker.Opacity, !string.IsNullOrEmpty(argsData.Series.Marker.Fill) ? argsData.Series.Marker.Fill : argsData.Fill);
                SeriesRenderer.DynamicOptions.MarkerSymbolId.Add(shapeOption.Id);
                symbolOptions.Add(CalculateSymbol(location, argsData.Shape.ToString(), new Size(argsData.Width, argsData.Height), marker.ImageUrl, shapeOption, Owner));
                point.Marker = new MarkerSettingModel() { Border = argsData.Border, Fill = argsData.Fill, Height = argsData.Height, Visible = true, Shape = argsData.Shape, Width = argsData.Width };
            }
            else
            {
                location = null;
                point.Marker = new MarkerSettingModel() { Visible = false };
            }
        }

        protected void RenderMarkers(RenderTreeBuilder builder)
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

        internal void UpdateDirection()
        {
            RendererShouldRender = Series.Marker.Visible;
            if (RendererShouldRender)
            {
                CalculateRenderTreeBuilderOptions();
            }

            InvalidateRender();
        }

        internal void ToggleVisibility()
        {
            RendererShouldRender = true;
            if (Series.Marker.Visible)
            {
                SeriesRenderer.CalculateMarkerClipPath();
                CalculateRenderTreeBuilderOptions();
            }
            else
            {
                symbolOptions.Clear();
            }

            InvalidateRender();
        }

        internal void MarkerColorChanged()
        {
            RendererShouldRender = Series.Marker.Visible;
            if (RendererShouldRender)
            {
                UpdateMarkerColor();
                InvalidateRender();
            }
        }

        internal void UpdateCustomization(string property)
        {
            RendererShouldRender = Series.Marker.Visible;
            if (RendererShouldRender)
            {
                switch (property)
                {
                    case "Fill":
                        UpdateMarkerFill();
                        break;
                    case "Color":
                        UpdateMarkerColor();
                        break;
                    case "Width":
                        UpdateMarkerBorderWidth();
                        break;
                    case "Opacity":
                        UpdateMarkerOpacity();
                        break;
                }

                InvalidateRender();
            }
        }

        private void UpdateMarkerColor()
        {
            string fill = !string.IsNullOrEmpty(Series.Marker.Border.Color) ? Series.Marker.Border.Color : SeriesRenderer.Interior;
            foreach (SymbolOptions symbolOption in symbolOptions)
            {
                if (symbolOption.ShapeName == ShapeName.ellipse)
                {
                    symbolOption.EllipseOption.Stroke = fill;
                }
                else if (symbolOption.ShapeName == ShapeName.path)
                {
                    symbolOption.PathOption.Stroke = fill;
                }
            }
        }

        private void UpdateMarkerFill()
        {
            string fill = !string.IsNullOrEmpty(Series.Marker.Fill) ? Series.Marker.Fill : "#ffffff";
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

        private void UpdateMarkerOpacity()
        {
            double opacity = Series.Marker.Opacity;
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

        internal void UpdateMarkerBorderWidth()
        {
            double width = Series.Marker.Border.Width;
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

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            if (SeriesRenderer != null && Series.Marker.Visible && SeriesRenderer.Series.Visible)
            {
                SeriesRenderer.RenderMarkerClipPath(builder);
                RenderMarkers(builder);
                builder.CloseElement();
            }
        }
    }
}

using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.Charts.Chart.Models;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class BubbleSeriesRenderer : ChartSeriesRenderer
    {
        private List<SymbolOptions> symbolOptions = new List<SymbolOptions>();

        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
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
            double segmentRadius, radius;
            double clipValue = Math.Max(Owner.InitialRect.Height, Owner.InitialRect.Width);
            double percentChange = clipValue / 100;
            double maxRadius = Series.MaxRadius * percentChange;
            double minRadius = Series.MinRadius * percentChange;
            double maximumSize = 0;
            ChartInternalLocation startLocation;
            if (double.IsNaN(Series.MaxRadius) || double.IsNaN(Series.MinRadius))
            {
                foreach (ChartSeries value1 in Owner.SeriesContainer.Elements)
                {
                    if (value1.Type == ChartSeriesType.Bubble && value1.Visible && (double.IsNaN(value1.MaxRadius) || double.IsNaN(value1.MinRadius)))
                    {
                        maximumSize = value1.Renderer.MaxSize > maximumSize ? value1.Renderer.MaxSize : maximumSize;
                    }
                }

                minRadius = maxRadius = 1;
                radius = ((clipValue / 5) / 2) * maxRadius;
            }
            else
            {
                maximumSize = Series.Renderer.MaxSize;
                radius = maxRadius - minRadius;
            }

            foreach (BubblePoint bubblePoint in Series.Renderer.Points)
            {
                startLocation = /*Owner.Redraw && */ bubblePoint.SymbolLocations.Count > 0 ? bubblePoint.SymbolLocations[0] : null;
                bubblePoint.SymbolLocations = new List<ChartInternalLocation>();
                bubblePoint.Regions = new List<Rect>();
                if (bubblePoint.Visible && ChartHelper.WithInRange(bubblePoint.Index - 1 > -1 ? Series.Renderer.Points[bubblePoint.Index - 1] : null, bubblePoint, bubblePoint.Index + 1 < Series.Renderer.Points.Count ? Series.Renderer.Points[bubblePoint.Index + 1] : null, XAxisRenderer))
                {
                    if (double.IsNaN(Series.MaxRadius) || double.IsNaN(Series.MinRadius))
                    {
                        segmentRadius = (bubblePoint.Size != null) ? radius * Math.Abs((double)bubblePoint.Size / maximumSize) : 0;
                    }
                    else
                    {
                        segmentRadius = (bubblePoint.Size != null) ? minRadius + (radius * Math.Abs((double)bubblePoint.Size / maximumSize)) : 0;
                    }

                    segmentRadius = segmentRadius != 0 && !double.IsNaN(segmentRadius) ? segmentRadius : minRadius;

                    PointRenderEventArgs argsData = new PointRenderEventArgs(Constants.POINTRENDER, false, bubblePoint, Series, SetPointColor(bubblePoint, Interior), SetBorderColor(bubblePoint, new ChartEventBorder { Width = Series.Border.Width, Color = Series.Border.Color }), 2 * segmentRadius, 2 * segmentRadius);

                    if (Owner.ChartEvents?.OnPointRender != null)
                    {
                        Owner.ChartEvents?.OnPointRender.Invoke(argsData);
                    }

                    if (!argsData.Cancel)
                    {
                        bubblePoint.SymbolLocations.Add(ChartHelper.GetPoint(XAxisRenderer.GetPointValue(bubblePoint.XValue), YAxisRenderer.GetPointValue(bubblePoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis));
                        bubblePoint.Interior = argsData.Fill;
                        PathOptions shapeOption = new PathOptions
                        {
                            Id = Owner.ID + "_Series_" + Series.Renderer.Index + "_Point_" + bubblePoint.Index,
                            Fill = argsData.Fill,
                            StrokeWidth = argsData.Border.Width,
                            Stroke = argsData.Border.Color,
                            Opacity = Series.Opacity
                        };
                        shapeOption.Visibility = string.Empty;
                        DynamicOptions.PathId.Add(shapeOption.Id);
                        SymbolOptions symbol = ChartMarkerRenderer.CalculateSymbol(bubblePoint.SymbolLocations[0], ChartShape.Circle.ToString(), new Size(argsData.Width, argsData.Height), Series.Marker.ImageUrl, shapeOption, Owner);
                        bubblePoint.Regions.Add(new Rect(
                             bubblePoint.SymbolLocations[0].X - segmentRadius,
                             bubblePoint.SymbolLocations[0].Y - segmentRadius,
                             2 * segmentRadius,
                             2 * segmentRadius));
                        bubblePoint.Marker = new MarkerSettingModel
                        {
                            Border = argsData.Border,
                            Fill = argsData.Fill,
                            Height = argsData.Height,
                            Visible = true,
                            Shape = ChartShape.Circle,
                            Width = argsData.Width
                        };
                        symbolOptions.Add(symbol);
                    }
                    else
                    {
                        bubblePoint.Marker = new MarkerSettingModel { Visible = false };
                    }
                }
            }
        }

        protected override void ProcessExpandoObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string tempSize = Series.Size;
            string pointColor = Series.PointColorMapping;
            int index = 0;
            BubblePoint point;
            foreach (object data in CurrentViewData)
            {
                IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                object x, y, size, color;
                expandoData.TryGetValue(x_Name, out x);
                expandoData.TryGetValue(y_Name, out y);
                expandoData.TryGetValue(tempSize, out size);
                expandoData.TryGetValue(pointColor, out color);
                point = new BubblePoint()
                {
                    X = x,
                    Y = y,
                    Size = size,
                    Interior = Convert.ToString(color, Culture),
                    Text = Convert.ToString(GetTextMapping(), Culture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessDynamicObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string size = Series.Size;
            string pointColor = Series.PointColorMapping;
            int index = 0;
            BubblePoint point;
            foreach (object data in CurrentViewData)
            {
                point = new BubblePoint()
                {
                    X = ChartHelper.GetDynamicMember(data, x_Name),
                    Y = ChartHelper.GetDynamicMember(data, y_Name),
                    Size = ChartHelper.GetDynamicMember(data, size),
                    Interior = Convert.ToString(ChartHelper.GetDynamicMember(data, pointColor), Culture),
                    Text = Convert.ToString(ChartHelper.GetDynamicMember(data, GetTextMapping()), Culture),
                    Tooltip = Convert.ToString(ChartHelper.GetDynamicMember(data, Series.TooltipMappingName), Culture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessJObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string size = Series.Size;
            string pointColor = Series.PointColorMapping;
            int index = 0;
            BubblePoint point;
            foreach (object data in CurrentViewData)
            {
                JObject jsonObject = (JObject)data;
                point = new BubblePoint()
                {
                    X = jsonObject.GetValue(x_Name, StringComparison.InvariantCulture),
                    Y = jsonObject.GetValue(y_Name, StringComparison.InvariantCulture),
                    Size = jsonObject.GetValue(size, StringComparison.InvariantCulture),
                    Interior = Convert.ToString(jsonObject.GetValue(pointColor, StringComparison.InvariantCulture), Culture),
                    Text = Convert.ToString(jsonObject.GetValue(GetTextMapping(), StringComparison.InvariantCulture), Culture),
                    Tooltip = Convert.ToString(jsonObject.GetValue(Series.TooltipMappingName, StringComparison.Ordinal), Culture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            PropertyAccessor x = new PropertyAccessor(firstDataType.GetProperty(x_Name));
            PropertyAccessor y = new PropertyAccessor(firstDataType.GetProperty(y_Name));
            PropertyAccessor size = new PropertyAccessor(firstDataType.GetProperty(Series.Size));
            PropertyAccessor pointColor = new PropertyAccessor(firstDataType.GetProperty(Series.PointColorMapping));
            PropertyAccessor textMapping = new PropertyAccessor(firstDataType.GetProperty(GetTextMapping()));
            PropertyAccessor tooltipMapping = new PropertyAccessor(firstDataType.GetProperty(Series.TooltipMappingName));
            int index = 0;
            BubblePoint point;
            foreach (object data in CurrentViewData)
            {
                point = new BubblePoint()
                {
                    X = x.GetValue(data),
                    Y = y.GetValue(data),
                    Size = size.GetValue(data),
                    Interior = Convert.ToString(pointColor.GetValue(data), Culture),
                    Text = Convert.ToString(textMapping.GetValue(data), Culture),
                    Tooltip = Convert.ToString(tooltipMapping.GetValue(data), Culture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
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

        internal override bool FindVisibility(Point point)
        {
            BubblePoint bubblePoint = point as BubblePoint;
            double y_AxisMin = YAxisRenderer.Axis.Minimum != null ? Convert.ToDouble(YAxisRenderer.Axis.Minimum, null) : double.NaN;
            double y_AxisMax = YAxisRenderer.Axis.Maximum != null ? Convert.ToDouble(YAxisRenderer.Axis.Maximum, null) : double.NaN;
            if (Owner.ChartAreaType == ChartAreaType.PolarAxes && ((!double.IsNaN(y_AxisMin) && bubblePoint.YValue < y_AxisMin) || (!double.IsNaN(y_AxisMax) && bubblePoint.YValue > y_AxisMax)))
            {
                return true;
            }

            SetXYMinMax(bubblePoint.XValue, bubblePoint.YValue);
            YData.Add(point.YValue);
            MaxSize = Math.Max(MaxSize, bubblePoint.Size == null || double.IsNaN((double)bubblePoint.Size) ? MaxSize : (double)bubblePoint.Size);
            return bubblePoint.X.Equals(null) || bubblePoint.Y == null || double.IsNaN(Convert.ToDouble(bubblePoint.Y, Culture));
        }

        protected override bool IsMarker()
        {
            return false;
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
    }
}

using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    /// <summary>
    /// Specifies the rendering of tooltip in smith chart component.
    /// </summary>
    public partial class SmithChartTooltip
    {
        private SfSmithChart smithchart;
        private double locationX;
        private double locationY;
        private Size elementSize;
        private SvgTooltip svgTooltip;
        private bool isTooltipRendered;
        private StringComparison comparison = StringComparison.InvariantCulture;

        internal SmithChartTooltip(SfSmithChart chart)
        {
            smithchart = chart;
        }

        internal TooltipComponent TemplateTooltip { get; set; }

        internal Timer Timer { get; set; } = new Timer();

        private static Point GetTemplateLocation(Rect bounds, Point symbolLocation, double width, double height, double markerHeight)
        {
            Point location = new Point(symbolLocation.X, symbolLocation.Y);
            double elementWidth = width + 10, elementHeight = height + 10, boundsX = bounds.X, boundsY = bounds.Y;
            location = new Point(location.X - (width / 2), location.Y - height - markerHeight);
            if (location.Y < boundsY)
            {
                location.Y = (symbolLocation.Y < 0 ? 0 : symbolLocation.Y) + markerHeight;
            }

            if (location.Y + elementHeight + 12 > boundsY + bounds.Height)
            {
                location.Y = (symbolLocation.Y > bounds.Height ? bounds.Height : symbolLocation.Y) - height - 12 - markerHeight;
            }

            if (location.X < boundsX)
            {
                location.X = boundsX;
            }

            if (location.X + elementWidth > boundsX + bounds.Width)
            {
                location.X -= (location.X + elementWidth) - (boundsX + bounds.Width);
            }

            return new Point(location.X, location.Y);
        }

        internal async void MouseMoveHandler(string targetID)
        {
            if (targetID.Contains("_Series_", comparison) || targetID.Contains("_Marker", comparison))
            {
                await SmithChartTooltipRender();
            }
            else if (isTooltipRendered)
            {
                RemoveTooltip(1000);
            }
        }

        private async Task SmithChartTooltipRender()
        {
            for (int i = 0; i < smithchart.Series.Count; i++)
            {
                SmithChartSeries series = smithchart.Series[i];
                double markerHeight = series.Marker.Visible ? series.Marker.Height + (series.Marker.Border.Width * 2) : 0;
                ClosestPoint closestPoint;
                if (series.Tooltip.Visible && series.Visible)
                {
                    closestPoint = ClosestPointXY(smithchart.MouseX, smithchart.MouseY, series, i);
                    if (closestPoint.Location != null)
                    {
                        isTooltipRendered = true;
                        if (series.Tooltip.Template != null)
                        {
                            SmithChartPoint tooltipTemp = new SmithChartPoint
                            {
                                Reactance = series.ActualPoints[(int)closestPoint.Index].Reactance,
                                Resistance = series.ActualPoints[(int)closestPoint.Index].Resistance
                            };
                            Point symbolLocation = new Point(closestPoint.Location.X, closestPoint.Location.Y);
                            Point templateLocation = GetTemplateLocation(smithchart.Bounds, symbolLocation, 0, 0, markerHeight);
                            TemplateTooltip.ChangeContent(series.Tooltip.Template, templateLocation, tooltipTemp, false);
                            await GetElementSize();
                            if (elementSize != null)
                            {
                                templateLocation = GetTemplateLocation(smithchart.Bounds, symbolLocation, elementSize.Width, elementSize.Height, markerHeight);
                                TemplateTooltip.ChangeContent(series.Tooltip.Template, templateLocation, tooltipTemp, true);
                            }
                        }
                        else
                        {
                            await CreateTooltip(closestPoint.Index, series);
                        }

                        break;
                    }
                    else if (svgTooltip != null)
                    {
                        RemoveTooltip(1000);
                    }
                }
            }
        }

        private async Task GetElementSize()
        {
            elementSize = await smithchart.InvokeMethod<Size>(SmithChartConstants.GETTEMPLATESIZE, false, new object[] { smithchart.ID + "_smithchart_tooltip_div" });
            if (elementSize == null)
            {
            }
        }

        private async Task CreateTooltip(double pointIndex, SmithChartSeries series)
        {
            SmithChartPoint currentPoint = series.ActualPoints[(int)pointIndex];
            double pointX = currentPoint.Resistance, pointY = currentPoint.Reactance;
            string tooltip = !string.IsNullOrEmpty(currentPoint.Tooltip) ? currentPoint.Tooltip.ToString() : null;
            string tooltipText = pointX + " " + ":" + " " + "<b>" + pointY + "</b>";
            double markerHeight = series.Marker.Visible ? series.Marker.Height : 0;
            SmithChartTooltipEventArgs argsData = new SmithChartTooltipEventArgs()
            {
                EventName = "tooltipRender",
                Text = !string.IsNullOrEmpty(tooltip) ? tooltip : tooltipText,
                HeaderText = "<b>" + series.Name + "</b>",
                Template = series.Tooltip.Template,
                Point = currentPoint
            };
            SfSmithChart.InvokeEvent<SmithChartTooltipEventArgs>(smithchart.SmithChartEvents?.TooltipRender, argsData);
            if (!argsData.Cancel)
            {
                if (svgTooltip == null)
                {
                    svgTooltip = new SvgTooltip()
                    {
                        Location = new TooltipLocation { X = locationX, Y = locationY - markerHeight },
                        Header = argsData.HeaderText,
                        Content = new string[] { argsData.Text },
                        Border = new TooltipBorder { Color = series.Tooltip.Border.Color, Width = series.Tooltip.Border.Width },
                        Fill = string.IsNullOrEmpty(series.Tooltip.Fill) ? smithchart.SmithChartThemeStyle.TooltipFill : series.Tooltip.Fill,
                        Data = new Point
                        {
                            X = currentPoint.Reactance,
                            Y = currentPoint.Resistance
                        },
                        Template = argsData.Template != null ? argsData.Template.ToString() : string.Empty,
                        AreaBounds = new TooltipAreaBounds { X = smithchart.Bounds.X, Y = smithchart.Bounds.Y, Height = smithchart.Bounds.Height, Width = smithchart.Bounds.Width },
                        Palette = new string[] { series.Interior },
                        Shapes = new Shape[] { Shape.Circle },
                        AvailableSize = smithchart.AvailableSize,
                        Theme = smithchart.Theme.ToString(),
                        Opacity = series.Tooltip.Opacity,
                        ArrowPadding = 8,
                        TextStyle = new TooltipTextStyle
                        {
                            Size = "13px",
                            FontStyle = "Normal",
                            FontWeight = "Normal",
                            FontFamily = string.IsNullOrEmpty(smithchart.SmithChartThemeStyle.FontFamily) ? "Roboto, Segoe UI, Noto, Sans-serif" : smithchart.SmithChartThemeStyle.FontFamily,
                            Opacity = double.IsNaN(smithchart.SmithChartThemeStyle.TooltipFillOpacity) ? 1 : smithchart.SmithChartThemeStyle.TooltipFillOpacity
                        }
                    };
                }
                else
                {
                    svgTooltip.Location = new TooltipLocation { X = locationX, Y = locationY - markerHeight };
                    svgTooltip.Header = argsData.HeaderText;
                    svgTooltip.Content = new string[] { argsData.Text };
                    svgTooltip.Border = new TooltipBorder { Color = series.Tooltip.Border.Color, Width = series.Tooltip.Border.Width };
                    svgTooltip.Fill = string.IsNullOrEmpty(series.Tooltip.Fill) ? smithchart.SmithChartThemeStyle.TooltipFill : series.Tooltip.Fill;
                    svgTooltip.Data = new Point
                    {
                        X = currentPoint.Reactance,
                        Y = currentPoint.Resistance
                    };
                    svgTooltip.Template = argsData.Template != null ? argsData.Template.ToString() : string.Empty;
                    svgTooltip.AreaBounds = new TooltipAreaBounds { X = smithchart.Bounds.X, Y = smithchart.Bounds.Y, Height = smithchart.Bounds.Height, Width = smithchart.Bounds.Width };
                    svgTooltip.Palette = new string[] { series.Interior };
                    svgTooltip.Shapes = new Shape[] { Shape.Circle };
                    svgTooltip.AvailableSize = smithchart.AvailableSize;
                    svgTooltip.Theme = smithchart.Theme.ToString();
                    svgTooltip.Opacity = series.Tooltip.Opacity;
                    svgTooltip.TextStyle = new TooltipTextStyle
                    {
                        Size = "13px",
                        FontStyle = "Normal",
                        FontWeight = "Normal",
                        FontFamily = smithchart.SmithChartThemeStyle.FontFamily ?? "Roboto, Segoe UI, Noto, Sans-serif",
                        Opacity = double.IsNaN(smithchart.SmithChartThemeStyle.TooltipFillOpacity) ? svgTooltip.TextStyle.Opacity : smithchart.SmithChartThemeStyle.TooltipFillOpacity
                    };
                }

                await smithchart.InvokeMethod(SmithChartConstants.RENDERTOOLTIP, new object[] { JsonSerializer.Serialize(svgTooltip), smithchart.ID + "_smithchart_tooltip_div", smithchart.Element });
            }
        }

        internal async Task MouseUpHandler()
        {
            await SmithChartTooltipRender();
        }

        internal void MouseLeaveHandler()
        {
            RemoveTooltip(1000);
        }

        private void RemoveTooltip(double fadeOutDuration)
        {
            isTooltipRendered = false;
            Timer.Stop();
            Timer.Interval = fadeOutDuration;
            Timer.AutoReset = false;
            if (smithchart.TooltipContent != null)
            {
                Timer.Elapsed += OnTimeOut;
                Timer.Start();
            }
        }

        private ClosestPoint ClosestPointXY(double x, double y, SmithChartSeries series, int seriesIndex)
        {
            double pointIndex = double.NaN;
            Point chartPoint, closePoint = null;
            for (int j = 0; j < series.ActualPoints.Count; j++)
            {
                chartPoint = smithchart.SeriesModule.GetLocation(seriesIndex, j);
                locationX = chartPoint.X;
                locationY = chartPoint.Y;
                pointIndex = j;
                double a = x - chartPoint.X;
                double b = y - chartPoint.Y;
                double distance = Math.Abs(Math.Sqrt((a * a) + (b * b)));
                if (distance < series.Marker.Width)
                {
                    closePoint = chartPoint;
                    pointIndex = j;
                    break;
                }
            }

            return new ClosestPoint { Location = closePoint, Index = pointIndex };
        }

        private void OnTimeOut(object source, ElapsedEventArgs e)
        {
            smithchart?.InvokeMethod(SmithChartConstants.FADEOUT, new object[] { smithchart.Element });
            (source as Timer).Stop();
        }

        internal void Dispose()
        {
            smithchart = null;
            svgTooltip = null;
            TemplateTooltip = null;
            elementSize = null;
            Timer = null;
        }
    }
}
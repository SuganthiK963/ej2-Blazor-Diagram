using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class Crosshair
    {
        private const string SPACE = " ";
        private bool isBottom;
        private bool isTop;
        private bool isLeft;
        private Size elementSize;
        private ChartInternalLocation arrowLocation;
        private double rx = 2;
        private double ry = 2;
        private SfChart chart;
        private string horizontalCross = string.Empty;
        private string verticalCross = string.Empty;
        private AxisTooltipAttributes axisTooltip;
        private SvgAxisGroup crossGroup;
        private double valueX;
        private double valueY;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal Crosshair(SfChart sf_Chart)
        {
            chart = sf_Chart;
            crossGroup = chart.CrossGroup;
        }

        internal void MouseUpHandler()
        {
            if (chart.StartMove)
            {
                RemoveCrosshair(2000);
            }

            RemoveCrosshairContent();
        }

        internal void RemoveCrosshair(int duration)
        {
            if (!crossGroup.IsFirstRender && crossGroup.Attributes["opacity"].ToString() != "0")
            {
                crossGroup.StartFadeOut(duration);
            }
        }

        internal void MouseLeaveHandler()
        {
            RemoveCrosshair(1000);
            RemoveCrosshairContent();
        }

        private void RemoveCrosshairContent()
        {
            // chart.CrosshairContent = null;
            horizontalCross = verticalCross = string.Empty;
            axisTooltip?.PathAttributes.Clear();
            axisTooltip?.TextAttributes.Clear();
        }

        internal void MouseMoveHandler(ChartInternalMouseEventArgs eventArgs)
        {
            if (eventArgs.Type == "touchmove" && chart.StartMove && eventArgs.PreventDefault)
            {
                // TODO: Need to include this condition in (Browser.isIos || Browser.isIos7).
                // eventArgs.preventDefault();
            }

            if (!chart.DisableTrackTooltip)
            {
                if (ChartHelper.WithInBounds(chart.MouseX, chart.MouseY, chart.AxisContainer.AxisLayout.SeriesClipRect) && (chart.StartMove || !chart.IsTouch))
                {
                    CreateCrosshair();
                }
                else
                {
                    RemoveCrosshair(1000);
                }
            }
        }

        internal bool LongPress()
        {
            if (ChartHelper.WithInBounds(chart.MouseX, chart.MouseY, chart.AxisContainer.AxisLayout.SeriesClipRect))
            {
                CreateCrosshair();
            }

            return false;
        }

        private void CreateCrosshair()
        {
            Rect chartRect = chart.AxisContainer.AxisLayout.SeriesClipRect;
            if (chart.Tooltip.Enable && !ChartHelper.WithInBounds(chart.TooltipModule.ValueX, chart.TooltipModule.ValueY, chartRect))
            {
                return;
            }

            valueX = chart.Tooltip.Enable ? chart.TooltipModule.ValueX : chart.MouseX;
            valueY = chart.Tooltip.Enable ? chart.TooltipModule.ValueY : chart.MouseY;
            if (!crossGroup.IsFirstRender)
            {
                crossGroup.SetOpacity(1);
            }

            if (chart.Crosshair.LineType == LineType.Both || chart.Crosshair.LineType == LineType.Horizontal)
            {
                horizontalCross = "M " + chartRect.X.ToString(culture) + SPACE + valueY.ToString(culture) + " L " + (chartRect.X + chartRect.Width).ToString(culture) + SPACE + valueY.ToString(culture);
            }

            if (chart.Crosshair.LineType == LineType.Both || chart.Crosshair.LineType == LineType.Vertical)
            {
                verticalCross = "M " + valueX.ToString(culture) + SPACE + chartRect.Y.ToString(culture) + " L " + valueX.ToString(culture) + SPACE + (chartRect.Y + chartRect.Height).ToString(culture);
            }

            CrosshairMoveEventArgs crosshairAxisArgs = (chart.ChartEvents?.OnCrosshairMove != null) ? new CrosshairMoveEventArgs() : null;
            if (crossGroup.IsFirstRender)
            {
                axisTooltip = RenderAxisTooltip(chart, chartRect, crosshairAxisArgs);
                SetAxisGroupElementsAtrributes();
            }
            else
            {
                axisTooltip = RenderAxisTooltip(chart, chartRect, crosshairAxisArgs);
                crossGroup.ChangeCrossHairValues(horizontalCross, verticalCross, axisTooltip.PathAttributes, axisTooltip.TextAttributes);
            }

            chart.ChartEvents?.OnCrosshairMove?.Invoke(crosshairAxisArgs);
        }

        private void SetAxisGroupElementsAtrributes()
        {
            Dictionary<string, object> groupAttribute = new Dictionary<string, object>
            {
                { "id", chart.ID + Constants.USERINTERACTION },
                { "opacity", "1" },
                { "style", "pointer-events:none" }
            };
            Dictionary<string, object> pathAttribute = new Dictionary<string, object>
            {
                { "chartId", chart.ID },
                { "lineWidth", chart.Crosshair.Line.Width },
                { "lineColor", chart.Crosshair.Line.Color ?? chart.ChartThemeStyle.CrosshairLine },
                { "dashArray", chart.Crosshair.DashArray },
                { "horizontalDir", horizontalCross },
                { "verticalDir", verticalCross },
                { "pathAttributes", axisTooltip.PathAttributes },
                { "textAttributes", axisTooltip.TextAttributes },
            };
            crossGroup.SetAttributes(groupAttribute, pathAttribute);
        }

        private string GetAxisText(ChartAxis axis)
        {
            isBottom = isTop = isLeft = false;
            double pointValue, labelValue = axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0;
            ChartAxisRenderer axisRenderer = axis.Renderer;
            if (axisRenderer.Orientation == Orientation.Horizontal)
            {
                pointValue = ChartHelper.GetValueByPoint(Math.Abs(valueX - axisRenderer.Rect.X), axisRenderer.Rect.Width, axisRenderer.Orientation, axisRenderer.VisibleRange, axis.IsInversed) + labelValue;
                isBottom = !axis.OpposedPosition;
                isTop = axis.OpposedPosition;
            }
            else
            {
                pointValue = ChartHelper.GetValueByPoint(Math.Abs(valueY - axisRenderer.Rect.Y), axisRenderer.Rect.Height, axisRenderer.Orientation, axisRenderer.VisibleRange, axis.IsInversed) + labelValue;
                isLeft = !axis.OpposedPosition;
            }

            if (axis.ValueType == ValueType.DateTime)
            {
                return Intl.GetDateFormat(new DateTime(1970, 1, 1).AddMilliseconds(pointValue), axisRenderer.DateFormat);
            }
            else if (axis.ValueType == ValueType.Category)
            {
                return (pointValue < axisRenderer.Labels.Count) ? axisRenderer.Labels.ElementAt((int)Math.Floor(pointValue)) : string.Empty;
            }
            else if (axis.ValueType == ValueType.DateTimeCategory)
            {
                return (axisRenderer as DateTimeCategoryAxisRenderer).GetIndexedAxisLabel(axisRenderer.Labels.ElementAt((int)Math.Floor(pointValue)));
            }
            else if (axis.ValueType == ValueType.Logarithmic)
            {
                return ChartAxisRenderer.FormatAxisValue(Math.Pow(axis.LogBase, pointValue), axisRenderer.GetFormat().Contains("{value}", StringComparison.InvariantCulture), axis.LabelFormat);
            }
            else
            {
                pointValue = Math.Round(pointValue, 3);
                bool customLabelFormat = !string.IsNullOrEmpty(axis.LabelFormat) && axis.LabelFormat.Contains("{value}", StringComparison.InvariantCulture);
                return customLabelFormat ? axis.LabelFormat.Replace("{value}", ChartAxisRenderer.FormatAxisValue(pointValue, customLabelFormat, axis.LabelFormat), StringComparison.InvariantCulture) : ChartAxisRenderer.FormatAxisValue(pointValue, customLabelFormat, axis.LabelFormat);
            }
        }

        private Rect TooltipLocation(string text, ChartAxis axis, Rect bounds, Rect axisRect)
        {
            Rect tooltipRect;
            bool islabelInside = axis.LabelPosition == AxisPosition.Inside;
            double scrollBarHeight = axis.ScrollbarSettings.Enable /*|| (axis.ZoomingScrollBar != null)*/ ? axis.ScrollBarHeight : 0;
            elementSize = ChartHelper.MeasureText(text, axis.CrosshairTooltip.TextStyle.GetChartFontOptions());
            if (axis.Renderer.Orientation == Orientation.Horizontal)
            {
                double y_Location = islabelInside ? axisRect.Y - elementSize.Height - 20 : axisRect.Y + scrollBarHeight;
                double height = islabelInside ? axisRect.Y - elementSize.Height - 10 : axisRect.Y + 10;
                arrowLocation = new ChartInternalLocation(valueX, y_Location);
                tooltipRect = new Rect(valueX - (elementSize.Width / 2) - 5, height + (!islabelInside ? scrollBarHeight : 0), elementSize.Width + 10, elementSize.Height + 10);
                if (axis.OpposedPosition)
                {
                    tooltipRect.Y = islabelInside ? axisRect.Y : axisRect.Y - (elementSize.Height + 20) - scrollBarHeight;
                }

                if (tooltipRect.X < bounds.X)
                {
                    tooltipRect.X = bounds.X;
                }

                if (tooltipRect.X + tooltipRect.Width > bounds.X + bounds.Width)
                {
                    tooltipRect.X -= tooltipRect.X + tooltipRect.Width - (bounds.X + bounds.Width);
                }

                if (arrowLocation.X + 5 > tooltipRect.X + tooltipRect.Width - rx)
                {
                    arrowLocation.X = tooltipRect.X + tooltipRect.Width - rx - 5;
                }

                if (arrowLocation.X - 5 < tooltipRect.X + rx)
                {
                    arrowLocation.X = tooltipRect.X + rx + 5;
                }
            }
            else
            {
                scrollBarHeight = scrollBarHeight * (axis.OpposedPosition ? 1 : -1);
                arrowLocation = new ChartInternalLocation(axisRect.X, valueY);
                tooltipRect = new Rect((islabelInside ? axisRect.X - scrollBarHeight : axisRect.X - elementSize.Width - 20) + scrollBarHeight, valueY - (elementSize.Height / 2) - 5, elementSize.Width + 10, elementSize.Height + 10);
                if (axis.OpposedPosition)
                {
                    tooltipRect.X = islabelInside ? axisRect.X - elementSize.Width - 10 : axisRect.X + 10 + scrollBarHeight;
                    if ((tooltipRect.X + tooltipRect.Width) > chart.AvailableSize.Width)
                    {
                        arrowLocation.X -= (tooltipRect.X + tooltipRect.Width) - chart.AvailableSize.Width;
                        tooltipRect.X -= tooltipRect.X + tooltipRect.Width - chart.AvailableSize.Width;
                    }
                }
                else if (tooltipRect.X < 0)
                {
                    arrowLocation.X -= tooltipRect.X;
                    tooltipRect.X = 0;
                }

                if (tooltipRect.Y < bounds.Y)
                {
                    tooltipRect.Y = bounds.Y;
                }

                if (tooltipRect.Y + tooltipRect.Height >= bounds.Y + bounds.Height)
                {
                    tooltipRect.Y -= tooltipRect.Y + tooltipRect.Height - (bounds.Y + bounds.Height);
                }

                if (arrowLocation.Y + (10 / 2) > tooltipRect.Y + tooltipRect.Height - ry)
                {
                    arrowLocation.Y = tooltipRect.Y + tooltipRect.Height - ry - (10 / 2);
                }

                if (arrowLocation.Y - (10 / 2) < tooltipRect.Y + ry)
                {
                    arrowLocation.Y = tooltipRect.Y + ry + (10 / 2);
                }
            }

            return tooltipRect;
        }

        private AxisTooltipAttributes RenderAxisTooltip(SfChart chart, Rect chartRect, CrosshairMoveEventArgs crosshairAxisargs)
        {
            List<Dictionary<string, object>> pathAttributes = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> textAttributes = new List<Dictionary<string, object>>();
            List<ChartAxisRenderer> axisRenderers = chart.AxisContainer.Renderers.Cast<ChartAxisRenderer>().ToList();
            for (int k = 0, length = axisRenderers.Count; k < length; k++)
            {
                ChartAxisRenderer axisRenderer = axisRenderers[k];
                ChartAxis axis = axisRenderer.Axis;
                Rect axisRect = !axis.PlaceNextToAxisLine ? axisRenderer.Rect : axisRenderer.UpdatedRect;
                if ((valueX <= (axisRect.X + axisRect.Width) && axisRect.X <= valueX && axisRect.Width != 0) || (valueY <= (axisRect.Y + axisRect.Height) && axisRect.Y <= valueY && axisRect.Height != 0))
                {
                    string axisText = GetAxisText(axis);
                    if (string.IsNullOrEmpty(axisText))
                    {
                        continue;
                    }

                    if (crosshairAxisargs != null)
                    {
                        crosshairAxisargs.AxisInfo.Add(new CrosshairAxisInfo(axis.Name, axisText));
                    }

                    if (axis.CrosshairTooltip.Enable)
                    {
                        Rect rect = TooltipLocation(axisText, axis, chartRect, axisRect);
                        PathOptions pathOptions = new PathOptions(chart.ID + "_axis_tooltip_" + k, string.Empty, string.Empty, 1, string.Empty, 1, axis.CrosshairTooltip.Fill ?? chart.ChartThemeStyle.CrosshairFill);
                        TextOptions textOptions = new TextOptions("0", "0", axis.CrosshairTooltip.TextStyle.Color ?? chart.ChartThemeStyle.CrosshairLabel, axis.CrosshairTooltip.TextStyle.GetFontOptions(), axisText, "start", chart.ID + "_axis_tooltip_text_" + k);
                        pathOptions.Direction = ChartHelper.FindDirection(rx, ry, rect, arrowLocation, 10, isTop, isBottom, isLeft, valueX, valueY);
                        textOptions.Text = axisText;
#pragma warning disable CA1305
                        textOptions.X = (rect.X + 5).ToString(culture);
                        textOptions.Y = (rect.Y + 5 + (elementSize.Height * 0.75)).ToString(culture);
#pragma warning restore CA1305
                        pathAttributes.Add(chart.SvgRenderer.GetOptions(pathOptions));
                        textAttributes.Add(chart.SvgRenderer.GetOptions(textOptions));
                    }
                }
            }

            return new AxisTooltipAttributes { PathAttributes = pathAttributes, TextAttributes = textAttributes };
        }

        internal void Dispose()
        {
            chart = null;
            elementSize = null;
            arrowLocation = null;
            crossGroup = null;
            axisTooltip = null;
        }
    }
}
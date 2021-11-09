using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Sparkline.Internal;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The Sparkline Charts is a replacement for normal charts to display trends in a very small area. Customize sparklines completely by changing the series or axis type and by adding markers, data labels, range bands, and more.
    /// </summary>
    /// <typeparam name="TValue">Represents the generic data type of the sparkline control.</typeparam>
    public partial class SfSparkline<TValue>
    {
        private ElementReference element;
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private StringComparison comparison = StringComparison.InvariantCulture;
        private ElementInfo elementInfo;

        internal RenderFragment ChartContent { get; set; }

        internal double TrackerX { get; set; }

        internal string TrackerColor { get; set; } = "transparent";

        internal double TrackerY { get; set; }

        internal string ClipId { get; set; }

        internal string ClipPath { get; set; }

        internal Size AvailableSize { get; set; }

        internal Style ThemeStyle { get; set; }

        internal SvgRendering Rendering { get; set; } = new SvgRendering();

        /// <summary>
        /// The method is used to render the sparkline again.
        /// </summary>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Refresh()
        {
            ChartContent = null;
            elementInfo = await InvokeMethod<ElementInfo>("sfBlazor.Sparkline.getElementSize", false, new object[] { element, Height, Width });
            CalculateSvgSize();
        }

        /// <summary>
        /// The method is used to render the sparkline again.
        /// </summary>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RefreshAsync()
        {
            await Refresh();
        }

        private async void CalculateSvgSize(bool isCDNScript = false)
        {
            double containerWidth = (double.IsNaN(elementInfo.Width) | elementInfo.Width <= 0) ? elementInfo.ParentWidth <= 0 ? 100 : elementInfo.ParentWidth : elementInfo.Width,
            containerHeight = (double.IsNaN(elementInfo.Height) | elementInfo.Height <= 0) ? elementInfo.ParentHeight <= 0 ? 50 : elementInfo.ParentHeight : elementInfo.Height,
            controlHeight = SparklineHelper.StringToNumber(Height, containerHeight, isCDNScript),
            controlWidth = SparklineHelper.StringToNumber(Width, containerWidth, isCDNScript);
            AvailableSize = new Size(controlWidth > 0 ? controlWidth : containerWidth > 0 ? containerWidth : 100, controlHeight > 0 ? controlHeight : containerHeight > 0 ? containerHeight : (elementInfo.IsDevice ? Math.Min(elementInfo.WindowWidth, elementInfo.WindowHeight) : containerHeight), 0, 0, false);
            await AxisCalculation();
            RenderSparkline();
        }

        internal async Task AxisCalculation()
        {
            if (DataPoints.Count > 0)
            {
                await FindRanges(DataPoints, isNumericArray);
            }
        }

        internal void RenderSparkline()
        {
            Rendering = new SvgRendering();
            ChartContent = RenderElements();
            StateHasChanged();
        }

        private RenderFragment RenderElements() => builder =>
        {
            int seq = 0;
            builder.OpenElement(seq, "svg");
            builder.AddMultipleAttributes(seq++, new Dictionary<string, object>
            {
                { "height", AvailableSize.Height },
                { "width", AvailableSize.Width },
                { "id", ID + "_svg" }
            });
            RenderBorder(builder);
            RenderSeries(builder);
            builder.CloseElement();
        };

        private void RenderBorder(RenderTreeBuilder builder)
        {
            ClipId = ID + "_Sparkline_Clip_Path";
            RectOptions borderRect;
            double borderWidth = ContainerArea?.Border != null ? ContainerArea.Border.Width : 0;
            if (ContainerArea != null && (borderWidth > 0 || !ContainerArea.Background.Equals("transparent", comparison)))
            {
                borderRect = new RectOptions(ID + "_SparklineBorder", borderWidth / 2, borderWidth / 2, AvailableSize.Width - borderWidth, AvailableSize.Height - borderWidth, borderWidth, ContainerArea.Border != null ? ContainerArea.Border.Color : string.Empty, ContainerArea.Background ?? ThemeStyle.Background, 0, 0, 1);
                DrawRectangle(builder, borderRect);
            }

            Margin margin = MarkerSettings?.Visible != null && MarkerSettings?.Visible.Count > 0 ? new Margin(0, 0, 0, 0) : new Margin(Padding.Top, Padding.Left, Padding.Bottom, Padding.Right);
            borderRect = new RectOptions(ID + "_Sparkline_Clip_Rect", margin.Left, margin.Top, AvailableSize.Width - (margin.Left + margin.Right), AvailableSize.Height - (margin.Top + margin.Bottom), 0, "transparent", "transparent");
            Rendering.OpenClipPath(builder, ClipId);
            DrawRectangle(builder, borderRect);
            builder.CloseElement();
        }

        private void RenderSeries(RenderTreeBuilder builder)
        {
            ClipPath = "url(#" + ClipId + ')';
            DrawAxis(builder);
            SeriesRenderingEventArgs args = new SeriesRenderingEventArgs()
            {
                LineWidth = LineWidth,
                Border = new Border() { Color = Border.Color, Width = Border.Width },
                Fill = Fill
            };
            Events?.OnSeriesRendering?.Invoke(args);
            if (VisiblePoints == null || args.Cancel)
            {
                return;
            }

            if (Type != SparklineType.Pie && Type != SparklineType.WinLoss && RangeBandSettings.Count > 0)
            {
                RenderRangeBands(builder);
            }

            if (Type == SparklineType.Line)
            {
                DrawLine(builder, VisiblePoints, args);
            }
            else if (Type == SparklineType.Area)
            {
                DrawArea(builder, VisiblePoints, args);
            }
            else if (Type == SparklineType.Column)
            {
                DrawColumn(builder, VisiblePoints, args);
            }
            else if (Type == SparklineType.WinLoss)
            {
                DrawWinLoss(builder, VisiblePoints, args);
            }
            else if (Type == SparklineType.Pie)
            {
                DrawPie(builder, VisiblePoints, args);
            }

            RenderMarker(builder, VisiblePoints);
            RenderLabels(builder, VisiblePoints);
            Events?.OnLoaded?.Invoke(new System.EventArgs());
        }

        private void DrawAxis(RenderTreeBuilder builder)
        {
            if (Type != SparklineType.WinLoss && Type != SparklineType.Pie && AxisSettings?.LineSettings != null && AxisSettings.LineSettings.Visible)
            {
                Rendering.RenderLine(builder, ID + "_Sparkline_XAxis", Padding.Left, AxisHeight, AvailableSize.Width - Padding.Right, AxisHeight, AxisColor, AxisWidth);
            }
        }

        private void RenderRangeBands(RenderTreeBuilder builder)
        {
            if (RangeBandSettings?.Count > 0)
            {
                double axisMin = double.IsNaN(AxisSettings.MinY) ? minPoint : AxisSettings.MinY;
                double axisMax = double.IsNaN(AxisSettings.MaxY) ? maxPoint : AxisSettings.MaxY;
                Rendering.OpenGroupElement(builder, ID + "_Sparkline_RangeBand_G", string.Empty, string.Empty);
                for (int i = 0; i < RangeBandSettings.Count; i++)
                {
                    if (axisMin <= RangeBandSettings[i].StartRange || axisMax >= RangeBandSettings[i].EndRange)
                    {
                        CreateRangeBand(builder, RangeBandSettings[i], i);
                    }
                }

                builder.CloseElement();
            }
        }

        private void RenderMarker(RenderTreeBuilder builder, List<SparklineValues> points)
        {
            if (MarkerSettings != null)
            {
                if (Type == SparklineType.Pie || Type == SparklineType.WinLoss || MarkerSettings.Visible == null || MarkerSettings.Visible.Count == 0)
                {
                    return;
                }

                CircleOptions option = new CircleOptions(string.Empty, "0", "0", (MarkerSettings.Size / 2).ToString(culture), string.Empty, MarkerSettings.Border.Width, MarkerSettings.Border.Color, MarkerSettings.Opacity, MarkerSettings.Fill);
                double highPos = 0, lowPos = 0;
                string visible = string.Join(',', MarkerSettings.Visible);
                if (visible.Contains("High", comparison) || visible.Contains("Low", comparison) || visible.Contains("All", comparison))
                {
                    double[] pointsYPos = points.Select(z => z.MarkerPosition).ToArray();
                    highPos = pointsYPos.Min();
                    lowPos = pointsYPos.Max();
                }

                bool isElementOpen = false;
                for (int i = 0, length = points.Count; i < length; i++)
                {
                    bool render = visible.Contains("All", comparison);
                    option.Id = ID + "_Sparkline_Marker_" + i.ToString(culture);
                    option.Fill = MarkerSettings.Fill;
                    render = GetSpecialPoint(render, points[i], option, i, highPos, lowPos, length, visible);
                    option.Stroke = !string.IsNullOrEmpty(MarkerSettings.Border.Color) ? MarkerSettings.Border.Color : option.Fill;
                    MarkerRenderingEventArgs markerArgs = new MarkerRenderingEventArgs()
                    {
                        Border = new Border() { Color = option.Stroke, Width = MarkerSettings.Border.Width },
                        Fill = option.Fill,
                        PointIndex = i,
                        X = points[i].Location.X,
                        Y = points[i].Location.Y,
                        Size = MarkerSettings.Size
                    };
                    Events?.OnMarkerRendering?.Invoke(markerArgs);
                    if (render && !markerArgs.Cancel)
                    {
                        if (render && !isElementOpen)
                        {
                            Rendering.OpenGroupElement(builder, ID + "_Sparkline_Marker_G", string.Empty, ClipPath);
                            isElementOpen = true;
                        }

                        option.Cx = markerArgs.X.ToString(culture);
                        option.Cy = markerArgs.Y.ToString(culture);
                        option.Fill = markerArgs.Fill;
                        option.Stroke = markerArgs.Border.Color;
                        option.StrokeWidth = markerArgs.Border.Width;
                        option.R = (markerArgs.Size / 2).ToString(culture);
                        option.AccessibilityText = points[i].XName.ToString();
                        Rendering.RenderCircle(builder, option);
                        if (i == length - 1 && isElementOpen)
                        {
                            builder.CloseElement();
                        }
                    }
                    else if (i == length - 1 && isElementOpen)
                    {
                        builder.CloseElement();
                    }
                }
            }
        }

        private void CreateRangeBand(RenderTreeBuilder builder, SparklineRangeBand range, int index)
        {
            double height = AvailableSize.Height - (Padding.Top * 2),
            width = AvailableSize.Width - (Padding.Left * 2),
            startHeight = height - ((height / UnitY) * (range.StartRange - minPoint)) + Padding.Top,
            endHeight = height - ((height / UnitY) * (range.EndRange - minPoint)) + Padding.Top;
            if (endHeight > (height + Padding.Top))
            {
                endHeight = height + Padding.Top;
            }
            else if (endHeight < (0 + Padding.Top))
            {
                endHeight = Padding.Top;
            }

            if (startHeight > (height + Padding.Top))
            {
                startHeight = height + Padding.Top;
            }
            else if (startHeight < (0 + Padding.Top))
            {
                startHeight = Padding.Top;
            }

            Rendering.RenderPath(builder, ID + "_RangeBand_" + index, "M " + Padding.Left.ToString(culture) + SPACE + startHeight.ToString(culture) + " L " + (width + Padding.Left).ToString(culture) + SPACE + startHeight.ToString(culture) + " L " + (width + Padding.Left).ToString(culture) + SPACE + endHeight.ToString(culture) + " L " + Padding.Left.ToString(culture) + SPACE + endHeight.ToString(culture) + " Z", "0", LineWidth, "transparent", range.Opacity, !string.IsNullOrEmpty(range.Color) ? range.Color : ThemeStyle.RangeBand);
        }

        private void DrawRectangle(RenderTreeBuilder builder, RectOptions options, string accessText = "")
        {
            Rendering.RenderPath(builder, options.Id, CalculateRoundedRectPath(options), "0", options.StrokeWidth, options.Stroke, options.Opacity, options.Fill, accessText);
        }

        internal string CalculateRoundedRectPath(RectOptions rect)
        {
            return "M " + rect.X.ToString(culture) + SPACE + rect.Y.ToString(culture) + " Q " + rect.X.ToString(culture) + SPACE + rect.Y.ToString(culture) + SPACE + rect.X.ToString(culture) + SPACE + rect.Y.ToString(culture) + " L " + (rect.X + rect.Width).ToString(culture) + SPACE + rect.Y.ToString(culture) + " Q " + (rect.X + rect.Width).ToString(culture) + SPACE + rect.Y.ToString(culture) + SPACE +
            (rect.X + rect.Width).ToString(culture) + SPACE + rect.Y.ToString(culture) + SPACE + "L " + (rect.X + rect.Width).ToString(culture) + SPACE + (rect.Y + rect.Height).ToString(culture) + " Q " + (rect.X + rect.Width).ToString(culture) + SPACE + (rect.Y + rect.Height).ToString(culture) + SPACE + (rect.X + rect.Width).ToString(culture) + SPACE +
            (rect.Y + rect.Height).ToString(culture) + " L " + rect.X.ToString(culture) + SPACE + (rect.Y + rect.Height).ToString(culture) + " Q " + rect.X.ToString(culture) + SPACE + (rect.Y + rect.Height).ToString(culture) + SPACE + rect.X.ToString(culture) + SPACE + (rect.Y + rect.Height).ToString(culture) + " L " + rect.X.ToString(culture) + SPACE +
            rect.Y.ToString(culture) + " Z";
        }

        internal void RenderTrackerLine(SparklineValues points)
        {
            TrackerColor = !string.IsNullOrEmpty(TooltipSettings.TrackLineSettings.Color) ? TooltipSettings.TrackLineSettings.Color : ThemeStyle.TrackerLine;
            TrackerX = points.Location.X;
            StateHasChanged();
        }

        /// <summary>
        /// Specifies to render the component, based on property changes.
        /// </summary>
        /// <param name="propertyChanges">List changed properties.</param>
        /// <param name="parent">The class belongs too.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnPropertyChanged(Dictionary<string, object> propertyChanges, string parent)
        {
            bool isRender = true;
            if (AvailableSize != null && propertyChanges.Any())
            {
                if (!string.IsNullOrEmpty(parent) && parent.Equals(nameof(SfSparkline<TValue>), comparison))
                {
                    if (propertyChanges != null && propertyChanges.ContainsKey("Theme"))
                    {
                        ThemeStyle = SparklineHelper.GetStyle(Theme);
                    }

                    if (propertyChanges != null && (propertyChanges.ContainsKey("ValueType") || propertyChanges.ContainsKey("DataSource") || propertyChanges.ContainsKey("Query") || propertyChanges.ContainsKey("YName") || propertyChanges.ContainsKey("XName")))
                    {
                        await ProcessData();
                        CalculateSvgSize();
                        isRender = false;
                    }

                    if (isRender && propertyChanges != null && (propertyChanges.ContainsKey("RangePadding") || propertyChanges.ContainsKey("Type") || propertyChanges.ContainsKey("Height") || propertyChanges.ContainsKey("Width")))
                    {
                        CalculateSvgSize();
                        isRender = false;
                    }
                    else
                    {
                        isRender = true;
                    }
                }

                if (parent.Equals(nameof(SparklinePadding), comparison) || parent.Equals(nameof(SparklineAxisLineSettings), comparison))
                {
                    CalculateSvgSize();
                    isRender = false;
                }
            }

            if (isRender)
            {
                StateHasChanged();
            }
        }
    }
}
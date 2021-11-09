using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Linq;
using System.ComponentModel;
using Microsoft.JSInterop;
using System.Globalization;
using System.Threading.Tasks;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.RangeNavigator.Internal
{
    /// <summary>
    /// Specifies the slider of range navigator.
    /// </summary>
    public class RangeSlider : SfDataBoundComponent
    {
        private const string SPACE = " ";
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private StringComparison comparison = StringComparison.InvariantCulture;
        private SfRangeNavigator chart;
        private string elementId;
        private string currentSlider;
        private SvgRect rectElement;
        private double labelIndex;
        private bool thumbVisible;
        private double thumpY;
        private double sliderWidth;
        private double previousMoveX;
        private double thumpPadding;
        private string thumbColor;

        internal RangeSlider(SfRangeNavigator rangeNavigator)
        {
            chart = rangeNavigator;
            Points = new List<DataPoint>();
            RangeNavigatorThumbSettings thumb = rangeNavigator.NavigatorStyleSettings.Thumb;
            thumbVisible = rangeNavigator.ThemeStyle.ThumbWidth != 0 && rangeNavigator.ThemeStyle.ThumbHeight != 0;
            elementId = rangeNavigator.Id;
            thumpPadding = rangeNavigator.ThemeStyle.ThumbWidth / 2;
            thumbColor = rangeNavigator.DisableRangeSelector ? "transparent" : (thumb.Fill ?? rangeNavigator.ThemeStyle.ThumbBackground);
        }

        internal double CurrentStart { get; set; } = double.NaN;

        internal double CurrentEnd { get; set; } = double.NaN;

        internal List<DataPoint> Points { get; set; }

        internal Rect LeftRect { get; set; }

        internal Rect RightRect { get; set; }

        internal Rect MidRect { get; set; }

        internal double SliderY { get; set; }

        internal double StartX { get; set; }

        internal double EndX { get; set; }

        internal bool IsDrag { get; set; }

        internal SvgSliderGroup LeftSliderSvgGroup { get; set; }

        internal SvgSliderGroup RightSliderSvgGroup { get; set; }

        internal void Render(SfRangeNavigator range, RenderTreeBuilder builder)
        {
            RangeNavigatorStyleSettings style = range.NavigatorStyleSettings;
            string disabledColor = range.DisableRangeSelector ? "transparent" : null;
            double y = range.InitialClipRect.Y + (range.InitialClipRect.Height / 2);
            thumpY = y - (range.ThemeStyle.ThumbHeight / 2);
            SetSliderRects(range.StartValue, range.EndValue);
            double padding = range.InitialClipRect.X, selectedX = range.IsRtlEnabled() ? EndX : StartX, rightPadding = range.IsRtlEnabled() ? StartX : EndX;
            range.SvgRenderer.OpenGroupElement(builder, elementId + "_sliders", string.Empty, string.Empty, range.DisableRangeSelector ? "pointer-events:none" : string.Empty);
            range.SvgRenderer.RenderRect(builder, elementId + "_leftUnSelectedArea", range.InitialClipRect.X, range.InitialClipRect.Y, selectedX - padding, range.InitialClipRect.Height, 0, "transparent", disabledColor ?? style.UnselectedRegionColor ?? range.ThemeStyle.UnselectedRectColor);
            range.SvgRenderer.RenderRect(builder, elementId + "_rightUnSelectedArea", rightPadding, range.InitialClipRect.Y, range.InitialClipRect.Width - (rightPadding - padding), range.InitialClipRect.Height, 0, "transparent", disabledColor ?? style.UnselectedRegionColor ?? range.ThemeStyle.UnselectedRectColor);
            range.SvgRenderer.RenderRect(builder, new RectOptions(elementId + "_SelectedArea", selectedX, range.InitialClipRect.Y, sliderWidth, range.InitialClipRect.Height, 0, "transparent", disabledColor ?? style.SelectedRegionColor ?? range.ThemeStyle.SelectedRegionColor, 0, 0, 1, "", "cursor: -webkit-grab; cursor: grab;"));
            CreateThump(range, elementId + "_LeftSlider", builder, "translate(" + (StartX - thumpPadding).ToString(culture) + ", 0)");
            CreateThump(range, elementId + "_RightSlider", builder, "translate(" + (EndX - thumpPadding).ToString(culture) + ", 0)");
            builder.CloseElement();
        }

        private void SetSliderRects(double start, double end)
        {
            double padding = chart.InitialClipRect.X;
            DoubleRange axisRange = chart.ChartSeries.XAxisRenderer.ActualRange;
            bool isLeightWeight = chart.Series.Count > 0 ? false : true;
            if (!(end >= start))
            {
                start = new double[] { end, end = start }.FirstOrDefault();
            }

            start = end >= start ? start : new double[] { end, end = start }.FirstOrDefault();
            start = Math.Max(start, axisRange.Start);
            end = Math.Min(end, axisRange.End);
            StartX = padding + RangeNavigatorHelper.GetXLocation(start, axisRange, chart.InitialClipRect.Width, chart.IsRtlEnabled());
            EndX = padding + RangeNavigatorHelper.GetXLocation(end, axisRange, chart.InitialClipRect.Width, chart.IsRtlEnabled());
            sliderWidth = Math.Abs(EndX - StartX);
            double rightPadding = chart.IsRtlEnabled() ? StartX : EndX, left = 0, leftX = chart.IsRtlEnabled() ? EndX : StartX, rightX = chart.IsRtlEnabled() ? StartX : EndX;
            LeftRect = new Rect(isLeightWeight ? left + padding : padding, isLeightWeight ? 0 : chart.InitialClipRect.Y, isLeightWeight ? leftX - padding : leftX, isLeightWeight ? thumpY : chart.InitialClipRect.Height);
            RightRect = new Rect(isLeightWeight ? left + rightX : rightX, isLeightWeight ? 0 : chart.InitialClipRect.Y, chart.InitialClipRect.Width - (rightPadding - padding), isLeightWeight ? thumpY : chart.InitialClipRect.Height);
            MidRect = new Rect(isLeightWeight ? leftX + left : 0, isLeightWeight ? 0 : chart.InitialClipRect.Y, isLeightWeight ? Math.Abs(EndX - StartX) : rightX, isLeightWeight ? thumpY : chart.InitialClipRect.Height);
            CurrentStart = start;
            CurrentEnd = end;
        }

        private void CreateThump(SfRangeNavigator range, string id, RenderTreeBuilder builder, string transform)
        {
            thumbColor = range.DisableRangeSelector ? "transparent" : (range.NavigatorStyleSettings.Thumb.Fill ?? range.ThemeStyle.ThumbBackground);
            range.ThemeStyle.ThumbWidth = double.IsNaN(range.NavigatorStyleSettings.Thumb.Width) ? 20 : range.NavigatorStyleSettings.Thumb.Width;
            range.ThemeStyle.ThumbHeight = double.IsNaN(range.NavigatorStyleSettings.Thumb.Height) ? 20 : range.NavigatorStyleSettings.Thumb.Height;
            RangeNavigatorThumbSettings thump = range.NavigatorStyleSettings.Thumb;
            RangeThemeStyle style = range.ThemeStyle;
            double y = range.InitialClipRect.Y + (range.InitialClipRect.Height / 2), x = thumpPadding, tickLength = (range.ThemeStyle.ThumbHeight / 2) - 5;
            string disabledColor = range.DisableRangeSelector ? "transparent" : null,
            lineColor = disabledColor ?? thump.Border.Color ?? style.ThumpLineColor;
            SliderOptions sliderGroup = new SliderOptions() { Id = id, Transform = transform, Style = "cursor: ew-resize" };
            int seq = 0;
            List<object> pathOptions = new List<object>();
            pathOptions.Add(new PathOptions(id + "_ThumbLine", "M" + SPACE + x.ToString(culture) + SPACE + range.InitialClipRect.Y.ToString(culture) + SPACE + "L" + SPACE + x.ToString(culture) + SPACE + (range.InitialClipRect.Y + range.InitialClipRect.Height).ToString(culture) + SPACE, null, thump.Border.Width, range.Series != null ? lineColor : "transparent", 1, "transparent"));
            thumpY = y - (range.ThemeStyle.ThumbHeight / 2);
            SliderY = range.InitialClipRect.Y > thumpY ? thumpY : range.InitialClipRect.Y;
            if (!range.DisableRangeSelector)
            {
                range.Rendering.RenderClipPath(builder, range.Id + "_shadow", new Rect { Width = range.ThemeStyle.ThumbWidth, Height = range.ThemeStyle.ThumbHeight, X = 0, Y = thumpY });
            }

            if (thump.Type == ThumbType.Circle)
            {
                pathOptions.Add(new EllipseOptions(id + "_ThumbSymbol", Convert.ToString(range.ThemeStyle.ThumbWidth / 2, culture), Convert.ToString(range.ThemeStyle.ThumbHeight / 2, culture), Convert.ToString(x, culture), Convert.ToString(y, culture), null, thump.Border.Width, lineColor, 1, disabledColor ?? thumbColor));
            }
            else
            {
                pathOptions.Add(new RectOptions(id + "_ThumbSymbol", x - (range.ThemeStyle.ThumbWidth / 2), y - (range.ThemeStyle.ThumbHeight / 2), range.ThemeStyle.ThumbWidth, range.ThemeStyle.ThumbHeight, thump.Border.Width, lineColor, disabledColor ?? thumbColor, 2, 2));
            }

            if (thumbVisible)
            {
                string direction = "M" + SPACE + (x + 2).ToString(culture) + SPACE + (y + tickLength).ToString(culture) + SPACE + "L" + SPACE + (x + 2).ToString(culture) + SPACE + (y - tickLength).ToString(culture) + SPACE +
                    "M" + SPACE + x.ToString(culture) + SPACE + (y + tickLength).ToString(culture) + SPACE + "L" + SPACE + x.ToString(culture) + SPACE + (y - tickLength).ToString(culture) + SPACE +
                    "M" + SPACE + (x - 2).ToString(culture) + SPACE + (y + tickLength).ToString(culture) + SPACE + "L" + SPACE + (x - 2).ToString(culture) + SPACE + (y - tickLength).ToString(culture) + SPACE;
                pathOptions.Add(new PathOptions(id + "_ThumbGrip", direction, null, 1, disabledColor ?? range.ThemeStyle.GripColor, 1, "transparent"));
            }

            sliderGroup.ShapeOptions = pathOptions;
            builder.OpenComponent<SvgSliderGroup>(seq++);
            builder.AddMultipleAttributes(seq++, range.SvgRenderer.GetOptions(sliderGroup));
            if (id.Contains("Left", comparison))
            {
                builder.AddComponentReferenceCapture(seq++, ins => { LeftSliderSvgGroup = (SvgSliderGroup)ins; });
            }
            else
            {
                builder.AddComponentReferenceCapture(seq++, ins => { RightSliderSvgGroup = (SvgSliderGroup)ins; });
            }

            builder.CloseComponent();
        }

        internal void SetSlider(double start, double end, bool trigger, bool showTooltip)
        {
            SetSliderRects(start, end);
            double padding = chart.InitialClipRect.X, selectedX = chart.IsRtlEnabled() ? EndX : StartX, rightPadding = chart.IsRtlEnabled() ? StartX : EndX;
            sliderWidth = Math.Abs(EndX - StartX);
            rectElement = chart.SvgRenderer.RectElementList.Find(item => item.Id == (chart.Id + "_SelectedArea"));
            rectElement.ChangeX(selectedX);
            rectElement.ChangeWidth(sliderWidth);
            rectElement = chart.SvgRenderer.RectElementList.Find(item => item.Id == (chart.Id + "_leftUnSelectedArea"));
            rectElement.ChangeWidth(selectedX - padding);
            rectElement = chart.SvgRenderer.RectElementList.Find(item => item.Id == (chart.Id + "_rightUnSelectedArea"));
            rectElement.ChangeX(rightPadding);
            rectElement.ChangeWidth(chart.InitialClipRect.Width - (rightPadding - padding));
            LeftSliderSvgGroup.ChangeTransform("translate(" + (StartX - thumpPadding).ToString(culture) + ", 0)");
            RightSliderSvgGroup.ChangeTransform("translate(" + (EndX - thumpPadding).ToString(culture) + ", 0)");
            if (trigger)
            {
                TriggerEvent(chart.ChartSeries.XAxisRenderer.ActualRange);
            }

            if (showTooltip)
            {
                chart.TooltipModule?.RenderThumbTooltip(this);
            }

            chart.UpdateChartData?.Invoke(Math.Min(start, end), Math.Max(start, end), null);
            if (chart.PeriodSelectorSettings?.Periods.Count > 0)
            {
                chart.PeriodSelectorModule.TriggerChange = false;
                chart.PeriodSelectorModule.SelectorItems?.DateRangeStartEnd(new DateTime(1970, 1, 1).AddMilliseconds(chart.StartValue), new DateTime(1970, 1, 1).AddMilliseconds(chart.EndValue));
            }
        }

        internal void TriggerEvent(DoubleRange range)
        {
            ChartAxisRenderer xaxis = chart.ChartSeries.XAxisRenderer;
            RangeValueType valueType = (RangeValueType)xaxis.Axis.ValueType;
            chart.StartValue = CurrentStart;
            chart.EndValue = CurrentEnd;
            if (chart.RangeNavigatorEvents?.Changed != null)
            {
                SfRangeNavigator.InvokeEvent<ChangedEventArgs>(chart.RangeNavigatorEvents?.Changed, new ChangedEventArgs()
                {
                    Start = (valueType == RangeValueType.DateTime) ? new DateTime(1970, 1, 1).AddMilliseconds(CurrentStart) as object :
                    (valueType == RangeValueType.Logarithmic ? Math.Pow(xaxis.Axis.LogBase, CurrentStart) : CurrentStart),
                    End = valueType == RangeValueType.DateTime ? new DateTime(1970, 1, 1).AddMilliseconds(CurrentEnd) as object :
                    (valueType == RangeValueType.Logarithmic ? Math.Pow(xaxis.Axis.LogBase, CurrentEnd) : CurrentEnd),
                    EventName = RangeConstants.CHANGED,
                    SelectedData = RangeNavigatorHelper.GetExactData(Points, CurrentStart, CurrentEnd),
                    ZoomPosition = (chart.IsRtlEnabled() ? (range.End - CurrentEnd) : (CurrentStart - range.Start)) / range.Delta,
                    ZoomFactor = (CurrentEnd - CurrentStart) / range.Delta
                });
            }
        }

        private double GetRangeValue(double x)
        {
            return RangeNavigatorHelper.GetRangeValueXByPoint(x, chart.InitialClipRect.Width, chart.ChartSeries.XAxisRenderer.ActualRange, chart.IsRtlEnabled());
        }

        internal async Task MouseUpHandler()
        {
            DoubleRange range = chart.ChartSeries.XAxisRenderer.ActualRange;
            bool enabledTooltip = chart.Tooltip.Enable;
            if (currentSlider == "UnSelectedArea")
            {
                double maxValue, start, end, difference = chart.EndValue - chart.StartValue;
                if (chart.MouseDownX < StartX)
                {
                    maxValue = Math.Max(GetRangeValue(chart.MouseDownX - (sliderWidth / 2) - chart.InitialClipRect.X), range.Start);
                    end = chart.IsRtlEnabled() ? maxValue : (maxValue + difference);
                    start = chart.IsRtlEnabled() ? (maxValue - difference) : maxValue;
                }
                else
                {
                    maxValue = Math.Min(GetRangeValue(chart.MouseDownX + (sliderWidth / 2) - chart.InitialClipRect.X), range.End);
                    start = chart.IsRtlEnabled() ? maxValue : (maxValue - difference);
                    end = chart.IsRtlEnabled() ? (maxValue + difference) : maxValue;
                }

                await PerformAnimation(start, end);
            }
            else if (currentSlider == "firstLevelLabels" || currentSlider == "secondLevelLabels")
            {
                VisibleLabels secondLabel;
                double levelValue;
                if (currentSlider == "firstLevelLabels")
                {
                    secondLabel = labelIndex + 1 < chart.RangeAxisModule.FirstLevelLabels.Count ? chart.RangeAxisModule.FirstLevelLabels[(int)labelIndex + 1] : null;
                    levelValue = chart.RangeAxisModule.FirstLevelLabels[(int)labelIndex].Value;
                }
                else
                {
                    secondLabel = labelIndex + 1 < chart.RangeAxisModule.SecondLevelLabels.Count ? chart.RangeAxisModule.SecondLevelLabels[(int)labelIndex + 1] : null;
                    levelValue = chart.RangeAxisModule.SecondLevelLabels[(int)labelIndex].Value;
                }

                await PerformAnimation(levelValue, secondLabel != null ? (chart.AllowIntervalData ? secondLabel.Value - 1 : secondLabel.Value) : range.End);
            }

            if (IsDrag && chart.AllowSnapping)
            {
                await PerformSnapping(CurrentStart, CurrentEnd, false, enabledTooltip);
            }

            if (currentSlider != null && chart.PeriodSelectorSettings.Periods.Count > 0)
            {
                chart.PeriodSelectorModule.TriggerChange = false;
                chart.PeriodSelectorModule.SelectorItems?.DateRangeStartEnd(new DateTime(1970, 1, 1).AddMilliseconds(chart.StartValue), new DateTime(1970, 1, 1).AddMilliseconds(chart.EndValue));
            }

            SvgRect rectElement = chart.SvgRenderer.RectElementList.Find(item => item.Id == (chart.Id + "_SelectedArea"));
            rectElement.ChangeStyle("cursor: -webkit-grab; cursor: grab;");
            chart.StartValue = CurrentStart;
            chart.EndValue = CurrentEnd;
            IsDrag = false;
            labelIndex = double.NaN;
            currentSlider = null;
        }

        internal void MouseMoveHandler()
        {
            DoubleRange axisRange = chart.ChartSeries.XAxisRenderer.ActualRange;
            Rect bounds = chart.InitialClipRect;
            double start, end;

            // For drag issue
            // currentSlider = GetCurrentSlider(target);
            if (IsDrag && chart.MouseX >= bounds.X)
            {
                switch (currentSlider)
                {
                    case "Left":
                        chart.StartValue = GetRangeValue(Math.Abs(chart.MouseX - bounds.X));
                        break;
                    case "Right":
                        chart.EndValue = GetRangeValue(Math.Abs(chart.MouseX - bounds.X));
                        break;
                    case "Middle":
                        start = Math.Max(GetRangeValue(Math.Abs(StartX - (previousMoveX - chart.MouseX) - bounds.X)), axisRange.Start);
                        end = Math.Min(GetRangeValue(Math.Abs(EndX - (previousMoveX - chart.MouseX) - bounds.X)), axisRange.End);
                        if (Math.Floor(Math.Abs(RangeNavigatorHelper.GetXLocation(end, axisRange, bounds.Width, chart.IsRtlEnabled()) - RangeNavigatorHelper.GetXLocation(start, axisRange, bounds.Width, chart.IsRtlEnabled()))) == Math.Floor(sliderWidth))
                        {
                            chart.StartValue = start;
                            chart.EndValue = end;
                        }

                        break;
                }

                SetSlider(chart.StartValue, chart.EndValue, true, chart.Tooltip.Enable);
                previousMoveX = chart.MouseX;
            }
        }

        internal async Task MouseCancelHandler()
        {
            if (IsDrag && chart.AllowSnapping)
            {
                await PerformSnapping(CurrentStart, CurrentEnd, false, chart.Tooltip.Enable);
            }

            IsDrag = false;
            currentSlider = null;
            chart.StartValue = CurrentStart;
            chart.EndValue = CurrentEnd;
        }

        internal async Task MouseDownHandler(string target)
        {
            currentSlider = GetCurrentSlider(target);
            rectElement = chart.SvgRenderer.RectElementList.Find(item => item.Id == (chart.Id + "_SelectedArea"));
            rectElement.ChangeStyle("cursor: -webkit-grab; cursor: grab;");
            IsDrag = !(currentSlider == "UnSelectedArea" || string.IsNullOrEmpty(currentSlider));
            previousMoveX = chart.MouseDownX;
            if (currentSlider == "Left" || currentSlider == "Right" || currentSlider == "Middle")
            {
                await chart.InvokeMethod("sfBlazor.RangeNavigator.setValueOnSliderSelect", new object[] { chart.Element, SliderChangeValues() });
            }

            if (IsDrag && chart.Tooltip.DisplayMode == TooltipDisplayMode.OnDemand)
            {
                chart.TooltipModule?.RenderThumbTooltip(this);
            }
        }

        internal Dictionary<string, object> SliderChangeValues()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            DoubleRange axisRange = chart.ChartSeries.XAxisRenderer.ActualRange;
            Rect bounds = chart.InitialClipRect;
            data.Add("rangeMin", axisRange.Start);
            data.Add("rangeMax", axisRange.End);
            data.Add("rangeDelta", axisRange.Delta);
            data.Add("boundsX", bounds.X);
            data.Add("boundsWidth", bounds.Width);
            data.Add("enableRtl", chart.IsRtlEnabled());
            data.Add("startX", StartX);
            data.Add("endX", EndX);
            data.Add("startValue", chart.StartValue);
            data.Add("endValue", chart.EndValue);
            data.Add("sliderWidth", sliderWidth);
            data.Add("defferedUpdate", false);
            data.Add("enableTooltip", chart.Tooltip.Enable);
            data.Add("valueType", chart.ValueType.ToString());
            data.Add("isDrag", IsDrag);
            data.Add("previousMoveX", previousMoveX);
            data.Add("currentSlider", currentSlider);
            data.Add("isTooltipHide", chart.Tooltip.DisplayMode == TooltipDisplayMode.OnDemand);
            data.Add("thumpPadding", thumpPadding);
            data.Add("isLeightWeight", chart.Series.Count == 0);
            data.Add("thumpY", thumpY);
            data.Add("boundsY", bounds.Y);
            data.Add("boundsHeight", bounds.Height);
            data.Add("format", chart.Tooltip.Format ?? chart.LabelFormat);
            data.Add("logBase", chart.XAxisRenderer.Axis.LogBase);
            data.Add("useGrouping", chart.UseGroupingSeparator);
            return data;
        }

        private string GetCurrentSlider(string id)
        {
            if (id.Contains(elementId + "_LeftSlider", comparison))
            {
                return "Left";
            }
            else if (id.Contains(elementId + "_RightSlider", comparison))
            {
                return "Right";
            }
            else if (id.Contains(elementId + "_SelectedArea", comparison))
            {
                return "Middle";
            }
            else if (id.Contains("UnSelectedArea", comparison))
            {
                return "UnSelectedArea";
            }
            else if (id.Contains(elementId + "_AxisLabel_", comparison) && chart.ValueType == RangeValueType.DateTime)
            {
                labelIndex = Convert.ToDouble(id.Split("_AxisLabel_")[1], null);
                return "firstLevelLabels";
            }
            else if (id.Contains(elementId + "_SecondaryLabel", comparison) && chart.ValueType == RangeValueType.DateTime)
            {
                labelIndex = Convert.ToDouble(id.Split("_SecondaryLabel_")[1], null);
                return "secondLevelLabels";
            }
            else
            {
                if (chart.PeriodSelectorModule != null)
                {
                    chart.PeriodSelectorModule.TriggerChange = true;
                }

                return null;
            }
        }

        /// <summary>
        /// JS interopt to perform the snapping the range navigator thump.
        /// </summary>
        /// <param name="start">Represents the mouse start range.</param>
        /// <param name="end">Represents the mouse end range.</param>
        /// <param name="trigger">Represents the event state.</param>
        /// <param name="tooltip">Represents the tooltip visibility state.</param>
        /// <exclude />
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task PerformSnapping(double start, double end, bool trigger, bool tooltip)
        {
            chart.RangeAxisModule.LowerValues.Add(chart.ChartSeries.XAxisRenderer.ActualRange.End);
            double[] lowerValues = chart.RangeAxisModule.LowerValues.ToArray();
            SetSlider(RangeNavigatorHelper.GetNearestValue(lowerValues, start), RangeNavigatorHelper.GetNearestValue(lowerValues, end), trigger, tooltip);
            chart.StartValue = CurrentStart;
            chart.EndValue = CurrentEnd;
            await chart.UpdateValue(chart.StartValue, chart.EndValue);
        }

        internal async Task PerformAnimation(double start, double end)
        {
            await chart.InvokeMethod(RangeConstants.PERFORMANIMATION, new object[] { chart.Element, start, end, chart.AnimationDuration, CurrentStart, CurrentEnd, chart.Tooltip.Enable, chart.AllowSnapping, DotNetObjectReference.Create<object>(chart.RangeSliderModule), SliderChangeValues() });
        }

        internal override void ComponentDispose()
        {
            chart = null;
            rectElement = null;
            Points = null;
            LeftRect = null;
            RightRect = null;
            MidRect = null;
            LeftSliderSvgGroup?.Dispose();
            RightSliderSvgGroup?.Dispose();
            LeftSliderSvgGroup = null;
            RightSliderSvgGroup = null;
        }
    }
}
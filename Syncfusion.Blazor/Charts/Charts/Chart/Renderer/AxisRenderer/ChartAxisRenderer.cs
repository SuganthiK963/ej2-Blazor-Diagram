using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Internal;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public struct DoubleRange : IEquatable<DoubleRange>
    {
        #region Members

        /// <summary>
        /// Initializes c_empty.
        /// </summary>
        private static readonly DoubleRange c_empty = new DoubleRange(double.NaN, double.NaN);
        private bool m_isempty;

        /// <summary>
        /// Initializes m_start.
        /// </summary>
        private double m_start;

        /// <summary>
        /// Initializes m_end.
        /// </summary>
        private double m_end;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Empty value.
        /// </summary>
        public static DoubleRange Empty
        {
            get
            {
                return c_empty;
            }
        }

        /// <summary>
        /// Gets the Start value.
        /// </summary>
        public double Start
        {
            get
            {
                return m_start;
            }
        }

        /// <summary>
        /// Gets the End value.
        /// </summary>
        public double End
        {
            get
            {
                return m_end;
            }
        }

        /// <summary>
        /// Gets the Delta value.
        /// </summary>
        public double Delta
        {
            get
            {
                return m_end - m_start;
            }
        }

        /// <summary>
        /// Gets the median.
        /// </summary>
        /// <value>The median.</value>
        public double Median
        {
            get
            {
                return (m_start + m_end) / 2d;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsEmpty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return m_isempty;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> struct.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="end">The end value.</param>
        public DoubleRange(double start, double end)
        {
            if (!double.IsNaN(start) && !double.IsNaN(end))
            {
                this.m_isempty = false;
            }
            else
            {
                this.m_isempty = true;
            }

            if (start > end)
            {
                m_start = end;
                m_end = start;
            }
            else
            {
                m_start = start;
                m_end = end;
            }
        }
        #endregion

        #region Operators

        /// <summary>
        /// The operator method
        /// </summary>
        /// <param name="leftRange">The left DoubleRange</param>
        /// <param name="rightRange">The right DoubleRange</param>
        /// <returns>The left range</returns>
        public static bool operator ==(DoubleRange leftRange, DoubleRange rightRange)
        {
            return leftRange.Equals(rightRange);
        }

        /// <summary>
        /// The operator method
        /// </summary>
        /// <param name="leftRange">The left range</param>
        /// <param name="rightRange">The right range</param>
        /// <returns>The inverse left range</returns>
        public static bool operator !=(DoubleRange leftRange, DoubleRange rightRange)
        {
            return !leftRange.Equals(rightRange);
        }

        /// <summary>
        /// Union operator.
        /// </summary>
        /// <param name="leftRange">First double range.</param>
        /// <param name="rightRange">Second double range.</param>
        /// <returns>The Union value.</returns>
        public static DoubleRange Add(DoubleRange leftRange, DoubleRange rightRange)
        {
            return Union(leftRange, rightRange);
        }

        /// <summary>
        /// Union operator.
        /// </summary>
        /// <param name="range">First double range.</param>
        /// <param name="value">Second double range.</param>
        /// <returns>The Union value.</returns>
        public static DoubleRange Add(DoubleRange range, double value)
        {
            return Union(range, value);
        }

        /// <summary>
        /// The operator.
        /// </summary>
        /// <param name="range">The DoubleRange.</param>
        /// <param name="value">The double value.</param>
        /// <returns>The range value.</returns>
        public static bool Compare(DoubleRange range, double value)
        {
            return range.m_start > value;
        }

        /// <summary>
        /// Return bool value from the given DoubleRange.
        /// </summary>
        /// <param name="range">The DoubleRange.</param>
        /// <param name="value">The double range.</param>
        /// <returns>The bool value.</returns>
        public static bool Compare(DoubleRange range, DoubleRange value)
        {
            return range.m_start > value.m_start && range.m_end > value.m_end;
        }

        /// <summary>
        /// return Bool value from doublerange.
        /// </summary>
        /// <param name="range">The DoubleRange.</param>
        /// <param name="value">The double range.</param>
        /// <returns>The bool value.</returns>
        public static bool CompareTo(DoubleRange range, DoubleRange value)
        {
            return range.m_start < value.m_start && range.m_end < value.m_end;
        }


        /// <summary>
        /// The operator.
        /// </summary>
        /// <param name="range">The DoubleRange.</param>
        /// <param name="value">The double value.</param>
        /// <returns>The range value.</returns>
        public static bool CompareTo(DoubleRange range, double value)
        {
            return range.m_end < value;
        }

        #endregion

        #region Public methods


        /// <summary>
        /// Create range by array of double.
        /// </summary>
        /// <param name="values">The values</param>
        /// <returns>The DoubleRange</returns>
        public static DoubleRange Union(params double[] values)
        {
            double min = double.MaxValue;
            double max = double.MinValue;

            if (values != null)
            {
                foreach (double val in values)
                {
                    if (double.IsNaN(val))
                    {
                        min = val;
                    }
                    else
                    if (min > val)
                    {
                        min = val;
                    }

                    if (max < val)
                    {
                        max = val;
                    }
                }
            }

            return new DoubleRange(min, max);
        }

        /// <summary>
        /// Unions the specified left range with right range.
        /// </summary>
        /// <param name="leftRange">The left range.</param>
        /// <param name="rightRange">The right range.</param>
        /// <returns>The DoubleRange</returns>
        public static DoubleRange Union(DoubleRange leftRange, DoubleRange rightRange)
        {
            if (leftRange.IsEmpty)
            {
                return rightRange;
            }
            else if (rightRange.IsEmpty)
            {
                return leftRange;
            }

            return new DoubleRange(Math.Min(leftRange.m_start, rightRange.m_start), Math.Max(leftRange.m_end, rightRange.m_end));
        }

        /// <summary>
        /// Unions the specified range with value.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <returns>The DoubleRange</returns>
        public static DoubleRange Union(DoubleRange range, double value)
        {
            if (range.IsEmpty)
            {
                return new DoubleRange(value, value);
            }

            return new DoubleRange(Math.Min(range.m_start, value), Math.Max(range.m_end, value));
        }

        /// <summary>
        /// Scales the specified range by value.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <returns>The DoubleRange</returns>
        public static DoubleRange Scale(DoubleRange range, double value)
        {
            if (range.IsEmpty)
            {
                return range;
            }

            return new DoubleRange(range.m_start - value * range.Delta, range.m_end + value * range.Delta);
        }

        /// <summary>
        /// Offsets the specified range by value.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="value">The value.</param>
        /// <returns>The DoubleRange</returns>
        public static DoubleRange Offset(DoubleRange range, double value)
        {
            if (range.IsEmpty)
            {
                return range;
            }

            return new DoubleRange(range.m_start + value, range.m_end + value);
        }

        /// <summary>
        /// Checks whether intersection region of two ranges is not empty.
        /// </summary>
        /// <param name="range">the DoubleRange</param>
        /// <returns><b>true</b> if  intersection is not empty</returns>
        public bool Intersects(DoubleRange range)
        {
            if (this.IsEmpty || this.IsEmpty)
            {
                return false;
            }

            return this.Inside(range.m_start) || this.Inside(range.m_end) || range.Inside(this.m_start) || range.Inside(this.m_end);
        }

        /// <summary>
        /// Checks whether intersection region of two ranges is not empty.
        /// </summary>
        /// <param name="start">The start value</param>
        /// <param name="end">The end value</param>
        /// <returns> true if  intersection is not empty</returns>
        public bool Intersects(double start, double end)
        {
            return this.Intersects(new DoubleRange(start, end));
        }

        /// <summary>
        /// Checks whether the given value is inside the axis range
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True if value is inside</returns>
        public bool Inside(double value)
        {
            if (this.IsEmpty)
            {
                return false;
            }

            return (value <= m_end) && (value >= m_start);
        }

        /// <summary>
        /// Checks whether the given range is inside the axis range
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>True is range is inside</returns>
        public bool Inside(DoubleRange range)
        {
            if (this.IsEmpty)
            {
                return false;
            }

            return m_start <= range.m_start && m_end >= range.m_end;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if obj and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is DoubleRange)
            {
                DoubleRange range = (DoubleRange)obj;
                return (m_start == range.m_start) && (m_end == range.m_end);
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return m_start.GetHashCode() ^ m_end.GetHashCode();
        }

        public bool Equals(DoubleRange other)
        {
           return (m_start == other.m_start) && (m_end == other.m_end);
        }
        #endregion
    }

    public class ChartAxisRenderer : ChartRenderer, IChartElementRenderer
    {
        private Rect availableRect;

        protected CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal ChartAxis Axis { get; set; }

        internal ChartAxis CrossInAxis { get; set; }

        internal int Index { get; set; }

        internal Rect Rect { get; set; } = new Rect();

        internal Rect UpdatedRect { get; set; } = new Rect();

        internal double Min { get; set; }

        internal double Max { get; set; }

        internal ChartHelper ChartHelper { get; set; } = new ChartHelper();

        internal List<IRequireAxis> RegisteredElements { get; private set; } = new List<IRequireAxis>();

        internal SfChart Chart { get; set; }

        internal Size Size { get; set; }

        internal double ActualInterval { get; set; }

        internal DoubleRange ActualRange { get; set; }

        internal double VisibleInterval { get; set; }

        internal DoubleRange VisibleRange { get; set; }

        internal Orientation Orientation { get; set; }

        internal int MaxPointLength { get; set; }

        internal double[] IntervalDivs { get; set; } = new double[] { 10, 5, 2, 1 };

        internal bool IsStack100 { get; set; }

        internal List<ChartSeriesRenderer> SeriesRenderer { get; set; } = new List<ChartSeriesRenderer>();

        internal List<VisibleLabels> VisibleLabels { get; set; } = new List<VisibleLabels>();

        internal List<string> TitleCollection { get; set; } = new List<string>();

        internal double Angle { get; set; }

        internal Size MaxLabelSize { get; set; } = new Size(0, 0);

        internal string RotatedLabel { get; set; }

        internal List<string> Labels { get; set; } = new List<string>();

        internal double DateTimeInterval { get; set; }

        internal string DateFormat { get; set; }

        internal AxisRenderInfo AxisRenderInfo { get; set; } = new AxisRenderInfo();

        internal ValueType Type { get; set; }

        internal IntervalType ActualIntervalType { get; set; }

        protected double PaddingInterval { get; set; }

        protected double IsColumn { get; set; }

        protected bool IsIntervalInDecimal { get; set; } = true;

        internal Size AxisAvailabelSize { get; set; }

        internal Scrollbar ZoomingScrollBar { get; set; }

        internal List<ChartSeries> Series { get; set; }

        internal double MultiLevelLabelHeight { get; set; }

        internal MultiLevelLabelRenderer MultiLevelLabelRenderer { get; set; }

        internal bool IsAxisInside { get; set; }

        internal bool IsTickInside { get; set; }

        internal bool IsAxisLabelInside { get; set; }

        internal ChartAxisOutsideRenderer OutSideRenderer { get; set; }

        internal double CrossAt { get; set; } = double.NaN;

        public static void OnSeriesChanged()
        {
        }

        public void HandleLayoutChange()
        {
        }

        protected override void OnInitialized()
        {
            Owner.AxisContainer.AddRenderer(this);
            SvgRenderer = Owner.SvgRenderer;
            Axis.Renderer = this;
            Chart = Axis.Container;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            if (availableRect != rect)
            {
                availableRect = rect;
            }
        }

        internal double FindTickSize()
        {
            if (Axis.TickPosition == AxisPosition.Inside)
            {
                return 0;
            }

            if (CrossInAxis != null && IsInside(CrossInAxis.Renderer.VisibleRange, CrossAt))
            {
                return 0;
            }

            return Axis.MajorTickLines.Height;
        }

        internal double FindLabelSize(double innerPadding, double axisWidth)
        {
            double titleSize = 0;
            ChartHelper helper = new ChartHelper();
            if (!string.IsNullOrEmpty(Axis.Title))
            {
                ChartFontOptions axisTitleStyle = Axis.TitleStyle.GetChartFontOptions();
                titleSize = ChartHelper.MeasureText(Axis.Title, axisTitleStyle).Height + innerPadding;
                if (!double.IsNaN(axisWidth))
                {
                    Axis.Renderer.TitleCollection = ChartHelper.GetTitle(Axis.Title, axisTitleStyle, axisWidth);
                    titleSize = titleSize * Axis.Renderer.TitleCollection.Count;
                }
            }

            if (Axis.LabelPosition == AxisPosition.Inside)
            {
                return titleSize + innerPadding;
            }

            double diff, diffValue;
            double labelSize = titleSize + innerPadding + Constants.AXISPADDING + ((Axis.Renderer.Orientation == Orientation.Vertical) ? Axis.Renderer.MaxLabelSize.Width : Axis.Renderer.MaxLabelSize.Height) + Axis.Renderer.MultiLevelLabelHeight;
            if (CrossInAxis != null && Axis.PlaceNextToAxisLine)
            {
                DoubleRange range = CrossInAxis.Renderer.VisibleRange;
                double size = (CrossInAxis.Renderer.Orientation == Orientation.Horizontal) ? CrossInAxis.Renderer.Rect.Width : CrossInAxis.Renderer.Rect.Height;
                if (double.IsNaN(size) && size == 0)
                {
                    return 0;
                }
                else if (IsInside(range))
                {
                    diffValue = FindDifference(CrossInAxis);
                    diff = diffValue * (size / range.Delta);
                    diff = diffValue * ((size - (diff < labelSize ? (labelSize - diff) : 0)) / range.Delta);
                    labelSize = (diff < labelSize) ? (labelSize - diff) : 0;
                }
            }

            return labelSize;
        }

        private double FindDifference(ChartAxis crossAxis)
        {
            double range;
            if (Axis.OpposedPosition)
            {
                range = crossAxis.IsInversed ? crossAxis.Renderer.VisibleRange.Start : crossAxis.Renderer.VisibleRange.End;
            }
            else
            {
                range = crossAxis.IsInversed ? crossAxis.Renderer.VisibleRange.End : crossAxis.Renderer.VisibleRange.Start;
            }

            return Math.Abs(CrossAt - range);
        }

        internal bool IsInside(DoubleRange range)
        {
            return ChartHelper.Inside(CrossAt, range) || (!Axis.OpposedPosition && CrossAt >= range.End) || (Axis.OpposedPosition && CrossAt <= range.Start);
        }

        protected DoubleRange TriggerRangeRender(DoubleRange range)
        {
            AxisRangeCalculatedEventArgs argsData = new AxisRangeCalculatedEventArgs(Constants.AXISRANGECALCULATED, false, range.Start, range.End, VisibleInterval, Rect, Axis.Name);

            Chart.ChartEvents.OnAxisActualRangeCalculated.Invoke(argsData);
            if (!argsData.Cancel)
            {
                VisibleInterval = argsData.Interval;
                return new DoubleRange(argsData.Minimum, argsData.Maximum);
            }

            return range;
        }

        protected void TriggerLabelRender(double tempInterval, string text)
        {
            AxisLabelRenderEventArgs argsData = new AxisLabelRenderEventArgs(Constants.AXISLABELRENDER, false, Axis, text, tempInterval, Axis.LabelStyle);
            if (Chart?.ChartEvents?.OnAxisLabelRender != null /*&& chart.ChartAxisLayoutPanel.IsAxisLabelRender*/)
            {
                Chart.ChartEvents.OnAxisLabelRender.Invoke(argsData);
            }

            if (!argsData.Cancel)
            {
                VisibleLabels.Add(new VisibleLabels(Axis.EnableTrim ? ChartHelper.TextTrim(Axis.MaximumLabelWidth, argsData.Text, Axis.LabelStyle.GetChartFontOptions()) : argsData.Text, argsData.Value, argsData.LabelStyle as ChartAxisLabelStyle, argsData.Text));
            }
        }

        protected void CalculateAutoIntervalOnBothAxisRange(DoubleRange visibleRange)
        {
            if (Orientation == Orientation.Horizontal /*&& Chart.ZoomSettings.Mode == ZoomMode.X*/)
            {
                SetYAxisMinMax(Chart.AxisContainer.Axes.Values.ToList(), visibleRange);
            }

            if (Orientation == Orientation.Vertical /*&& Chart.ZoomSettings.Mode == ZoomMode.Y*/)
            {
                SetXAxisMinMax(Chart.AxisContainer.Axes.Values.ToList(), visibleRange);
            }
        }

        private void SetYAxisMinMax(List<ChartAxis> axisCollection, DoubleRange visibleRange)
        {
            for (int i = 0; i < SeriesRenderer.Count; i++)
            {
                ChartSeriesRenderer series = SeriesRenderer[i];
                List<double> pointYValue = new List<double>();
                foreach (Point points in series.Points)
                {
                    if ((points.XValue > visibleRange.Start) && (points.XValue < visibleRange.End))
                    {
                        pointYValue.Add(points.YValue);
                    }
                }

                foreach (ChartAxis axis in axisCollection)
                {
                    if (Orientation == Orientation.Vertical && series != null && pointYValue.Count > 0)
                    {
                        series.YMin = pointYValue.Min();
                        series.YMax = pointYValue.Max();
                    }
                }
            }
        }

        private void SetXAxisMinMax(List<ChartAxis> axisCollection, DoubleRange visibleRange)
        {
            for (int i = 0; i < SeriesRenderer.Count; i++)
            {
                ChartSeriesRenderer series = SeriesRenderer[i];
                List<double> pointXValue = new List<double>();
                foreach (Point points in series.Points)
                {
                    if ((points.YValue > visibleRange.Start) && (points.YValue < visibleRange.End))
                    {
                        pointXValue.Add(points.XValue);
                    }
                }

                foreach (ChartAxis axis in axisCollection)
                {
                    if (Orientation == Orientation.Horizontal && series != null && pointXValue.Count > 0)
                    {
                        series.XMin = pointXValue.Min();
                        series.XMax = pointXValue.Max();
                    }
                }
            }
        }

        internal static string FormatAxisValue(object formatValue, bool isCustom, string format)
        {
            if (formatValue.GetType().Equals(typeof(double[])))
            {
                return "NAN";
            }

            return Intl.GetNumericFormat(formatValue, isCustom ? string.Empty : format);
        }

        internal string FormatValue(double tempInterval)
        {
            string labelFormat = GetFormat();
            Regex regex = new Regex("(?<=[\\.])[0-9]+");
            bool isFraction = regex.Match(Convert.ToString(tempInterval, CultureInfo.InvariantCulture)).Value.Length > 2,
            isCustom = labelFormat.Contains("{value}", StringComparison.InvariantCulture),
            isNumericFormat = labelFormat.Contains('n', StringComparison.InvariantCulture) || labelFormat.Contains('p', StringComparison.InvariantCulture) || labelFormat.Contains('c', StringComparison.InvariantCulture);
            if (isFraction && !isNumericFormat)
            {
                tempInterval = Convert.ToDouble(tempInterval.ToString("N2", null), null);
            }

            string formatValue = FormatAxisValue(tempInterval, labelFormat.Contains("{value}", StringComparison.InvariantCulture), labelFormat);
            return isCustom ? labelFormat.Replace("{value}", formatValue, StringComparison.InvariantCulture) : formatValue;
        }

        public void InvalidateRender()
        {
            StateHasChanged();
        }

        internal static Type GetRendererType(ValueType type)
        {
            switch (type)
            {
                case ValueType.Double:
                    return typeof(NumericAxisRenderer);
                case ValueType.DateTime:
                    return typeof(DateTimeAxisRenderer);
                case ValueType.Category:
                    return typeof(CategoryAxisRenderer);
                case ValueType.DateTimeCategory:
                    return typeof(DateTimeCategoryAxisRenderer);
                case ValueType.Logarithmic:
                    return typeof(LogarithmicAxisRenderer);
                default:
                    return null;
            }
        }

        internal void ComputeSize(Size availableSize)
        {
            AxisAvailabelSize = availableSize;
            Rect = new Rect(0, 0, availableSize.Width, availableSize.Height);
            Axis.ScrollBarHeight = Owner.AxisContainer.ScrollbarModule != null && ((Owner.ZoomingModule != null && Owner.ZoomSettings.EnableScrollbar && Axis.EnableScrollbarOnZooming && Owner.ZoomingModule.IsZoomed && (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0)) || Axis.ScrollbarSettings.Enable) ? 16 : 0;
            CalculateRangeAndInterval();
            GenerateVisibleLabels();
        }

        internal void ChangeAxisRange(bool updateRange = true)
        {
            RendererShouldRender = true;
            ClearAxisInfo();
            if (updateRange)
            {
                CalculateRangeAndInterval();
            }

            GenerateVisibleLabels();
            UpdateAxisRendering();
        }

        internal void UpdateAxisRendering()
        {
            RendererShouldRender = true;
            Owner.AxisContainer.AxisLayout?.AxisRenderingCalculation(this);
            ProcessRenderQueue();
            Owner.AnnotationContainer?.UpdateRenderers();
        }

        public override void ProcessRenderQueue()
        {
            InvokeAsync(StateHasChanged);
            OutSideRenderer?.ProcessRenderQueue();
        }

        internal bool IsFixedRange()
        {
            return Axis.Minimum != null && Axis.Maximum != null;
        }

        internal bool NeedAxisLayoutChange(double min, double max)
        {
            int maxDigits = Math.Max(ActualRange.Start.ToString(CultureInfo.InvariantCulture).Length, ActualRange.End.ToString(CultureInfo.InvariantCulture).Length);
            int minDigits = Math.Max(min.ToString(CultureInfo.InvariantCulture).Length, max.ToString(CultureInfo.InvariantCulture).Length);
            return minDigits != maxDigits;
        }

        protected virtual Size ComputeDesiredSize(Size availableSize)
        {
            return default(Size);
        }

        internal void CalculateRangeAndInterval()
        {
            DoubleRange range = CalculateActualRange();

            VisibleInterval = ActualInterval = CalculateActualInterval(range);

            ActualRange = ApplyRangePadding(range, ActualInterval);

            VisibleRange = CalculateVisibleRange(ActualRange);
        }

        protected void GetMaxLabelWidth()
        {
            double pointX, previousEnd = 0;
            bool isIntersect = false, isAxisLabelBreak;
            Angle = Axis.LabelRotation;
            MaxLabelSize = new Size(0, 0);
            LabelIntersectAction action = Axis.LabelIntersectAction;
            VisibleLabels label;
            ChartHelper helper = new ChartHelper();
            ChartFontOptions axisLabelStyle = Axis.LabelStyle.GetChartFontOptions();
            for (int i = 0, len = VisibleLabels.Count; i < len; i++)
            {
                label = VisibleLabels[i];
                isAxisLabelBreak = label.OriginalText.Contains("<br>", StringComparison.InvariantCulture);
                if (isAxisLabelBreak)
                {
                    label.Size = ChartHelper.MeasureText(label.OriginalText.Replace("<br>", " ", StringComparison.InvariantCulture), axisLabelStyle);
                    label.BreakLabelSize = ChartHelper.MeasureText(Axis.EnableTrim ? string.Join("<br>", label.TextArr) : label.OriginalText, axisLabelStyle);
                }
                else
                {
                    label.Size = ChartHelper.MeasureText(label.Text, axisLabelStyle);
                }

                double width = isAxisLabelBreak ? label.BreakLabelSize.Width : label.Size.Width;
                if (width > MaxLabelSize.Width)
                {
                    MaxLabelSize.Width = width;
                    RotatedLabel = label.Text;
                }

                double height = isAxisLabelBreak ? label.BreakLabelSize.Height : label.Size.Height;
                if (height > MaxLabelSize.Height)
                {
                    MaxLabelSize.Height = height;
                }

                if (isAxisLabelBreak)
                {
                    label.TextArr = Axis.EnableTrim ? new string[] { label.Text } : label.OriginalText.Split("<br>");
                }

                if (action == LabelIntersectAction.None || action == LabelIntersectAction.Hide || action == LabelIntersectAction.Trim)
                {
                    continue;
                }

                if ((action != LabelIntersectAction.None || Angle % 360 == 0) && Orientation == Orientation.Horizontal && Rect.Width > 0 && !isIntersect)
                {
                    double width1 = isAxisLabelBreak ? label.BreakLabelSize.Width : label.Size.Width;
                    double height1 = isAxisLabelBreak ? label.BreakLabelSize.Height : label.Size.Height;
                    pointX = (ChartHelper.ValueToCoefficient(label.Value, this) * Rect.Width) + Rect.X;
                    pointX = pointX - (width1 / 2);
                    if (Axis.EdgeLabelPlacement == EdgeLabelPlacement.Shift)
                    {
                        if (i == 0 && pointX < Rect.X)
                        {
                            pointX = Rect.X;
                        }

                        if (i == VisibleLabels.Count - 1 && ((pointX + width1) > (Rect.X + Rect.Width)))
                        {
                            pointX = Rect.X + Rect.Width - width1;
                        }
                    }

                    switch (action)
                    {
                        case LabelIntersectAction.MultipleRows:
                            if (i > 0)
                            {
                                FindMultiRows(i, pointX, label, isAxisLabelBreak);
                            }

                            break;
                        case LabelIntersectAction.Rotate45:
                        case LabelIntersectAction.Rotate90:
                            if (i > 0 && (!Axis.IsInversed ? pointX <= previousEnd : pointX + width1 >= previousEnd))
                            {
                                Angle = (action == LabelIntersectAction.Rotate45) ? 45 : 90;
                                isIntersect = true;
                            }

                            break;
                        default:
                            if (isAxisLabelBreak)
                            {
                                string[] result;
                                List<string> result1 = new List<string>();
                                string str;
                                for (int index = 0; index < label.TextArr.Length; index++)
                                {
                                    result = ChartHelper.TextWrap(label.TextArr[index], Rect.Width / VisibleLabels.Count, axisLabelStyle).ToArray();
                                    if (result.Length > 1)
                                    {
                                        for (int j = 0; j < result.Length; j++)
                                        {
                                            str = result[j];
                                            result1.Add(str);
                                        }
                                    }
                                    else
                                    {
                                        result1.Add(result[0]);
                                    }
                                }

                                label.TextArr = result1.ToArray();
                            }
                            else
                            {
                                label.TextArr = ChartHelper.TextWrap(label.Text, Rect.Width / VisibleLabels.Count, axisLabelStyle).ToArray();
                            }

                            double lheight = height1 * label.TextArr.Length;
                            if (lheight > MaxLabelSize.Height)
                            {
                                MaxLabelSize.Height = lheight;
                            }

                            break;
                    }

                    previousEnd = Axis.IsInversed ? pointX : pointX + width1;
                }
            }

            if (Angle != 0 && Orientation == Orientation.Horizontal)
            {
                // I264474: Fix for datasource bind im mounted console error ocurred
                RotatedLabel = string.IsNullOrEmpty(RotatedLabel) ? string.Empty : RotatedLabel;
                if (RotatedLabel.Contains("<br>", StringComparison.InvariantCulture))
                {
                    MaxLabelSize = ChartHelper.MeasureText(RotatedLabel, axisLabelStyle);
                }
                else
                {
                    MaxLabelSize = ChartHelper.RotateTextSize(axisLabelStyle, RotatedLabel, Angle);
                }
            }

            if (Axis.MultiLevelLabels.Count > 0)
            {
                MultiLevelLabelRenderer = new MultiLevelLabelRenderer();
                MultiLevelLabelRenderer.IniMultilevelLabel(this);
            }
        }

        private double UpdateCrossAt(ChartAxis axis, object crossAt)
        {
            switch (axis.ValueType)
            {
                case ValueType.DateTime:
                    return ((DateTime)crossAt - new DateTime(1970, 1, 1)).TotalMilliseconds;
                case ValueType.Category:
                    string crossValue = crossAt.ToString();
                    return !float.Parse(crossValue, null).Equals(float.NaN) ? float.Parse(crossValue, null) : Labels.IndexOf(crossValue);
                case ValueType.Logarithmic:
                    return ChartHelper.LogBase(Convert.ToDouble(crossAt, null), axis.LogBase);
                default:
                    return Convert.ToDouble(crossAt, culture);
            }
        }

        internal bool IsInside(DoubleRange range, double crossValue)
        {
            return ChartHelper.Inside(crossValue, range) || (!Axis.OpposedPosition && crossValue >= range.End) || (Axis.OpposedPosition && crossValue <= range.Start);
        }

        internal void UpdateCrossValue()
        {
            if (Axis.CrossesAt == null)
            {
                UpdatedRect = Rect;
                return;
            }

            CrossAt = UpdateCrossAt(CrossInAxis, Axis.CrossesAt);
            if (double.IsNaN(CrossAt) || !IsInside(CrossInAxis.Renderer.VisibleRange, CrossAt))
            {
                UpdatedRect = Rect;
                return;
            }

            double crossValue = CrossAt;
            DoubleRange range = CrossInAxis.Renderer.VisibleRange;
            if (!Axis.OpposedPosition && CrossAt > range.End)
            {
                crossValue = range.End;
            }
            else if (CrossAt < range.Start)
            {
                crossValue = range.Start;
            }

            UpdatedRect = new Rect() { X = Rect.X, Y = Rect.Y, Height = Rect.Height, Width = Rect.Width };
            if (Orientation == Orientation.Horizontal)
            {
                crossValue = CrossInAxis.Renderer.Rect.Height - (ChartHelper.ValueToCoefficient(crossValue, CrossInAxis.Renderer) * CrossInAxis.Renderer.Rect.Height);
                UpdatedRect.Y = CrossInAxis.Renderer.Rect.Y + crossValue;
            }
            else
            {
                crossValue = ChartHelper.ValueToCoefficient(crossValue, CrossInAxis.Renderer) * CrossInAxis.Renderer.Rect.Width;
                UpdatedRect.X = CrossInAxis.Renderer.Rect.X + crossValue;
            }
        }

        private void FindMultiRows(int length, double currentX, VisibleLabels currentLabel, bool isBreakLabels)
        {
            VisibleLabels label;
            double pointX, breakLabelwidth;
            List<double> store = new List<double>();
            bool isMultiRows;
            for (int i = length - 1; i >= 0; i--)
            {
                label = VisibleLabels[i];
                breakLabelwidth = isBreakLabels ? label.BreakLabelSize.Width : label.Size.Width;
                pointX = (ChartHelper.ValueToCoefficient(label.Value, this) * Rect.Width) + Rect.X;
                isMultiRows = !Axis.IsInversed ? currentX < (pointX + (breakLabelwidth * 0.5)) : currentX + currentLabel.Size.Width > (pointX - (breakLabelwidth * 0.5));
                if (isMultiRows)
                {
                    store.Add(label.Index);
                    currentLabel.Index = (currentLabel.Index > label.Index) ? currentLabel.Index : label.Index + 1;
                }
                else
                {
                    currentLabel.Index = store.IndexOf(label.Index) > -1 ? currentLabel.Index : label.Index;
                }
            }

            double height = ((isBreakLabels ? currentLabel.BreakLabelSize.Height : currentLabel.Size.Height) * currentLabel.Index) + (5 * (currentLabel.Index - 1));
            if (height > MaxLabelSize.Height)
            {
                MaxLabelSize.Height = height;
            }
        }

        private DoubleRange CalculateActualRange()
        {
            Min = double.NaN;
            Max = double.NaN;
            if (!ChartHelper.SetRange(Axis))
            {
                ChartAxis axis = Axis;
                foreach (ChartSeriesRenderer seriesRenderer in SeriesRenderer)
                {
                    ChartSeries series = seriesRenderer.Series;
                    if (!series.Visible)
                    {
                        continue;
                    }

                    PaddingInterval = 0;
                    MaxPointLength = seriesRenderer.Points.Count;
                    string type = series.Type.ToString();
                    if ((((type.Contains("Column", StringComparison.InvariantCulture) || type.Contains("Histogram", StringComparison.InvariantCulture)) && Orientation == Orientation.Horizontal) || (type.Contains("Bar", StringComparison.InvariantCulture) && Orientation == Orientation.Vertical)) && ((seriesRenderer.XAxisRenderer.Axis.ValueType == ValueType.Double || seriesRenderer.XAxisRenderer.Axis.ValueType == ValueType.DateTime) && seriesRenderer.XAxisRenderer.Axis.RangePadding == ChartRangePadding.Auto))
                    {
                        PaddingInterval = ChartHelper.GetMinPointsDelta(seriesRenderer.XAxisRenderer.Axis, axis.Renderer.SeriesRenderer) * 0.5;
                    }

                    if (Orientation == Orientation.Horizontal)
                    {
                        if (Chart.RequireInvertedAxis)
                        {
                            YAxisRange(series);
                        }
                        else
                        {
                            FindMinMax(seriesRenderer.XMin - PaddingInterval, seriesRenderer.XMax + PaddingInterval);
                        }
                    }

                    if (Orientation == Orientation.Vertical)
                    {
                        IsColumn += series.Type == ChartSeriesType.Column || series.Type == ChartSeriesType.Bar || series.DrawType == ChartDrawType.Column ? 1 : 0;
                        if (Chart.RequireInvertedAxis)
                        {
                            FindMinMax(seriesRenderer.XMin - PaddingInterval, seriesRenderer.XMax + PaddingInterval);
                        }
                        else
                        {
                            YAxisRange(series);
                        }
                    }
                }
            }

            return InitializeDoubleRange();
        }

        private void YAxisRange(ChartSeries series)
        {
            FindMinMax(series.Renderer.YMin, series.Renderer.YMax);
        }

        private void FindMinMax(double min, double max)
        {
            Min = (double.IsNaN(Min) || Min > min) ? min : Min;
            Max = (double.IsNaN(Max) || Max < max) ? max : (Max == Min && Max < 0 && Min < 0) ? 0 : Max;
        }

        internal virtual DoubleRange InitializeDoubleRange()
        {
            bool isCategory = Axis.ValueType == ValueType.Category || Axis.ValueType == ValueType.DateTimeCategory;

            // calculate axis Maximum of category & datetimcategory axis when all series make unvisible using legend
            if (isCategory && double.IsNaN(Max) && !SeriesRenderer.Any(series => series.Series.Visible) && Axis.Maximum == null)
            {
                List<int> points = new List<int>();
                foreach (ChartSeriesRenderer series in SeriesRenderer)
                {
                    points.Add(series.Points.Count - 1);
                }

                Max = points.Max();
            }

            Min = Axis.Minimum != null ? Convert.ToDouble(Axis.Minimum, null) : (double.IsNaN(Min) || double.IsPositiveInfinity(Min)) ? 0 : Min;
            Max = Axis.Maximum != null ? Convert.ToDouble(Axis.Maximum, null) : (double.IsNaN(Max) || double.IsNegativeInfinity(Max)) ? 5 : Max;
            Max = (Min == Max && !isCategory) ? Min + 1 : Max;
            return new DoubleRange(Min, Max);
        }

        internal virtual double CalculateActualInterval(DoubleRange range)
        {
            return 1.0;
        }

        protected virtual DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            return range;
        }

        internal virtual DoubleRange CalculateVisibleRange(DoubleRange actualRange)
        {
            if (Chart.ChartAreaType == ChartAreaType.CartesianAxes && (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0))
            {
                actualRange = CalculateVisibleRangeOnZooming();
                if (Axis.EnableAutoIntervalOnZooming && Axis.ValueType != ValueType.Category)
                {
                    CalculateAutoIntervalOnBothAxisRange(actualRange);
                    VisibleInterval = CalculateNumericNiceInterval(actualRange.Delta);
                }
            }

            if (Chart.ChartEvents?.OnAxisActualRangeCalculated != null)
            {
                actualRange = TriggerRangeRender(actualRange);
            }

            return actualRange;
        }

        internal virtual void GenerateVisibleLabels()
        {
            RendererShouldRender = true;
        }

        protected double CalculateNumericNiceInterval(double delta)
        {
            double actualDesiredIntervalsCount = GetActualDesiredIntervalsCount();
            double niceInterval = delta / actualDesiredIntervalsCount;
            double minInterval = Math.Pow(10, Math.Floor(ChartHelper.LogBase(niceInterval, 10)));
            if (!double.IsNaN(Axis.DesiredIntervals))
            {
                return niceInterval;
            }

            foreach (double interval in IntervalDivs)
            {
                double currentInterval = minInterval * interval;
                if (actualDesiredIntervalsCount < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }

        protected double GetActualDesiredIntervalsCount()
        {
            if (double.IsNaN(Axis.DesiredIntervals))
            {
                return Axis.Renderer.Orientation == Orientation.Horizontal ? Math.Max(AxisAvailabelSize.Width * 0.533 * Axis.MaximumLabels / 100, 1) : Math.Max(AxisAvailabelSize.Height * Axis.MaximumLabels / 100, 1);
            }
            else
            {
                return Axis.DesiredIntervals;
            }
        }

        protected ChartRangePadding GetRangePadding()
        {
            ChartRangePadding padding = Axis.RangePadding;
            if (padding != ChartRangePadding.Auto)
            {
                return padding;
            }

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    if (Chart.RequireInvertedAxis)
                    {
                        padding = IsStack100 || Chart.IsStockChart ? ChartRangePadding.Round : ChartRangePadding.Normal;
                    }
                    else
                    {
                        padding = ChartRangePadding.None;
                    }

                    break;
                case Orientation.Vertical:
                    if (!Chart.RequireInvertedAxis)
                    {
                        padding = IsStack100 || Chart.IsStockChart ? ChartRangePadding.Round : ChartRangePadding.Normal;
                    }
                    else
                    {
                        padding = ChartRangePadding.None;
                    }

                    break;
            }

            return padding;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (availableRect != null && builder != null)
            {
                if (Owner.AxisContainer.AxisLayout is CartesianAxisLayout)
                {
                    RenderCartesianAxisInsideCollection(builder);
                }
                else
                {
                    RenderPolarRadarAxisInsideCollection(builder);
                }

                RendererShouldRender = false;
            }
        }

        protected DoubleRange CalculateVisibleRangeOnZooming()
        {
            double start, end;
            if (!Axis.IsInversed)
            {
                start = ActualRange.Start + (Axis.ZoomPosition * ActualRange.Delta);
                end = start + (Axis.ZoomFactor * ActualRange.Delta);
            }
            else
            {
                start = ActualRange.End - (Axis.ZoomPosition * ActualRange.Delta);
                end = start - (Axis.ZoomFactor * ActualRange.Delta);
            }

            if (start < ActualRange.Start)
            {
                end = end + (ActualRange.Start - start);
                start = ActualRange.Start;
            }

            if (end > ActualRange.End)
            {
                start = start - (end - ActualRange.End);
                end = ActualRange.End;
            }

            return new DoubleRange(start, end);
        }

        internal string GetFormat()
        {
            if (!string.IsNullOrEmpty(Axis.LabelFormat))
            {
                if (Axis.LabelFormat.IndexOf('p', StringComparison.InvariantCulture) == 0 && !Axis.LabelFormat.Contains("{value}", StringComparison.InvariantCulture) && IsStack100)
                {
                    return "{value}%";
                }

                return Axis.LabelFormat;
            }

            return IsStack100 ? "{value}%" : string.Empty;
        }

        internal void CustomizeGridRenderingOptions(string key)
        {
            RendererShouldRender = true;
            switch (key)
            {
                case "MajorGridLines":
                    if (AxisRenderInfo.AxisGridOptions.ContainsKey(key))
                    {
                        foreach (PathOptions option in AxisRenderInfo.AxisGridOptions[key])
                        {
                            option.Stroke = Axis.MajorGridLines.Color;
                            option.StrokeWidth = Axis.MajorGridLines.Width;
                            option.StrokeDashArray = Axis.MajorGridLines.DashArray;
                        }
                    }
                    else
                    {
                        foreach (CircleOptions option in AxisRenderInfo.MajorGridCircleOptions)
                        {
                            option.Stroke = Axis.MajorGridLines.Color;
                            option.StrokeWidth = Axis.MajorGridLines.Width;
                            option.StrokeDashArray = Axis.MajorGridLines.DashArray;
                        }
                    }

                    break;
                case "LabelStyle":
                    foreach (TextOptions option in AxisRenderInfo.AxisLabelOptions)
                    {
                        option.Fill = Axis.LabelStyle.Color;
                        option.FontSize = Axis.LabelStyle.Size;
                    }

                    break;
            }
        }

        internal void OnThemeChange()
        {
            RendererShouldRender = true;
            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderInfo.AxisGridOptions)
            {
                if (AxisRenderInfo.AxisGridOptions.ContainsKey(keyValue.Key))
                {
                    string themeColor = AxisThemeColor(keyValue.Key);
                    AxisRenderInfo.AxisGridOptions[keyValue.Key].ForEach(option => option.Stroke = themeColor);
                }
            }

            foreach (CircleOptions circle in AxisRenderInfo.MajorGridCircleOptions)
            {
                circle.Stroke = Chart.ChartThemeStyle.MajorGridLine;
            }

            foreach (TextOptions text in AxisRenderInfo.AxisLabelOptions)
            {
                text.Fill = Chart.ChartThemeStyle.AxisLabel;
            }

            if (AxisRenderInfo.AxisTitleOption != null)
            {
                AxisRenderInfo.AxisTitleOption.Fill = Chart.ChartThemeStyle.AxisTitle;
            }
        }

        private string AxisThemeColor(string key)
        {
            switch (key)
            {
                case Constants.MAJORGRIDLINE:
                    return Chart.ChartThemeStyle.MajorGridLine;
                case Constants.MAJORTICKLINE:
                    return Chart.ChartThemeStyle.MajorTickLine;
                case Constants.MINORGRIDLINE:
                    return Chart.ChartThemeStyle.MinorGridLine;
                case Constants.MINORTICKLINE:
                    return Chart.ChartThemeStyle.MinorTickLine;
            }

            return string.Empty;
        }

        internal void ClearAxisInfo()
        {
            AxisRenderInfo.AxisGridOptions.Clear();
            AxisRenderInfo.AxisLabelOptions.Clear();
            AxisRenderInfo.AxisTitleOption = null;
            AxisRenderInfo.AxisLine = null;
            AxisRenderInfo.AxisBorder = null;
            AxisRenderInfo.MajorGridCircleOptions.Clear();
        }

        private void RenderCartesianAxisInsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer.OpenGroupElement(builder, Owner.ID + "AxisGroup" + Index + "Inside");
            if (AxisRenderInfo.AxisLine != null && !IsAxisInside)
            {
                DrawLine(builder, AxisRenderInfo.AxisLine);
            }

            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderInfo.AxisGridOptions)
            {
                if (keyValue.Key.Equals(Constants.MAJORGRIDLINE, StringComparison.Ordinal) || keyValue.Key.Equals(Constants.MINORGRIDLINE, StringComparison.Ordinal))
                {
                    DrawLine(builder, keyValue.Value);
                }

                if (!IsTickInside)
                {
                    if (keyValue.Key.Equals(Constants.MAJORTICKLINE, StringComparison.Ordinal))
                    {
                        DrawLine(builder, keyValue.Value);
                    }

                    if (keyValue.Key.Equals(Constants.MINORTICKLINE, StringComparison.Ordinal))
                    {
                        DrawLine(builder, keyValue.Value);
                    }
                }
            }

            if (!IsAxisLabelInside)
            {
                SvgRenderer.OpenGroupElement(builder, Owner.ID + "AxisLabels" + Index);
                foreach (TextOptions option in AxisRenderInfo.AxisLabelOptions)
                {
                    ChartHelper.TextElement(builder, SvgRenderer, option);
                }

                builder.CloseElement();
                if (AxisRenderInfo.AxisBorder != null)
                {
                    SvgRenderer.RenderPath(builder, AxisRenderInfo.AxisBorder, "pointer-events: none");
                }

                MultiLevelLabelRenderer?.RenderMultilevelLabel(builder);
            }

            if (AxisRenderInfo.AxisTitleOption != null && !IsAxisInside)
            {
                ChartHelper.TextElement(builder, SvgRenderer, AxisRenderInfo.AxisTitleOption);
            }

            builder.CloseElement();
        }

        internal void RenderPolarRadarAxisInsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer.OpenGroupElement(builder, Chart.ID + "AxisGroup" + Index);
            foreach (CircleOptions option in AxisRenderInfo.MajorGridCircleOptions)
            {
                SvgRenderer.RenderCircle(builder, option);
            }

            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderInfo.AxisGridOptions)
            {
                if (keyValue.Key.Equals(Constants.MAJORGRIDLINE, StringComparison.Ordinal) || keyValue.Key.Equals(Constants.MINORGRIDLINE, StringComparison.Ordinal))
                {
                    DrawLine(builder, keyValue.Value);
                }
            }

            builder.CloseElement();
        }

        internal void DrawLine(RenderTreeBuilder builder, List<PathOptions> pathOptionCollection)
        {
            foreach (PathOptions axisLineOption in pathOptionCollection)
            {
                SvgRenderer.RenderPath(builder, axisLineOption.Id, axisLineOption.Direction, axisLineOption.StrokeDashArray, axisLineOption.StrokeWidth, axisLineOption.Stroke);
            }
        }

        internal void DrawLine(RenderTreeBuilder builder, PathOptions axisLineOption)
        {
            SvgRenderer.RenderPath(builder, axisLineOption.Id, axisLineOption.Direction, axisLineOption.StrokeDashArray, axisLineOption.StrokeWidth, axisLineOption.Stroke);
        }

        internal virtual double GetPointValue(double x)
        {
            return x;
        }

        internal virtual bool IsDefaultRenderer()
        {
            return false;
        }
    }
}

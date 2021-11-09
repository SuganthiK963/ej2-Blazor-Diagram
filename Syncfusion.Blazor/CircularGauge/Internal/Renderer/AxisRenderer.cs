using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Represents to render the axis.
    /// </summary>
    internal class AxisRenderer
    {
        private const string SPACE = " ";
        private const double BASEFONT = 100;
        private PointF textLocation;
        private List<double> farSizes = new List<double>();
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisRenderer"/> class.
        /// </summary>
        internal AxisRenderer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisRenderer"/> class.
        /// </summary>
        /// <param name="parent">represents the axis properties.</param>
        internal AxisRenderer(SfCircularGauge parent)
        {
            Parent = parent;
            RangeColors = ThemeStyle.GetRangePalette(Parent.Theme);
        }

        /// <summary>
        /// Gets or sets to render the axis.
        /// </summary>
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties to render the axis.
        /// </summary>
        internal List<Axis> AxisCollection { get; set; } = new List<Axis>();

        /// <summary>
        /// Gets or sets the properties of the axis.
        /// </summary>
        internal Axis AxisSetting { get; set; } = new Axis();

        /// <summary>
        /// Gets or sets the properties of the annotation.
        /// </summary>
        internal AnnotationRenderer AnnotationRenderer { get; set; }

        /// <summary>
        /// Gets or sets the properties of the range.
        /// </summary>
        internal RangeRenderer RangeRenderer { get; set; }

        /// <summary>
        /// Gets or sets the properties of the pointer.
        /// </summary>
        internal PointerRenderer PointerRenderer { get; set; }

        /// <summary>
        /// Gets or sets the properties of the legend.
        /// </summary>
        internal LegendRenderer LegendRenderer { get; set; }

        /// <summary>
        /// Gets or sets the properties of the tooltip.
        /// </summary>
        internal TooltipRenderer TooltipRenderer { get; set; }

        /// <summary>
        /// Gets or sets the properties of the title.
        /// </summary>
        internal TitleRenderer TitleRender { get; set; }

        /// <summary>
        /// Gets or sets the properties of the range colors.
        /// </summary>
        internal string[] RangeColors { get; set; }

        /// <summary>
        /// This method to check whether it's a complete circle for circular gauge.
        /// </summary>
        /// <param name="startAngle">represents angle to start.</param>
        /// <param name="endAngle">represents angle to end.</param>
        /// <returns>return the boolean value.</returns>
        internal static bool IsCompleteAngle(double startAngle, double endAngle)
        {
            double totalAngle = endAngle - startAngle;
            totalAngle = (totalAngle <= 0) ? (totalAngle + 360) : totalAngle;
            return Math.Floor(totalAngle / 360) != 0;
        }

        /// <summary>
        /// This method is to get the degree for circular gauge.
        /// </summary>
        /// <param name="startAngle">represents the value of start.</param>
        /// <param name="endAngle">represents the value of end.</param>
        /// <returns>returns the value as double.</returns>
        internal static double GetDegreeValue(double startAngle, double endAngle)
        {
            double degreeValue = endAngle - startAngle;
            return degreeValue < 0 ? degreeValue + 360 : degreeValue;
        }

        /// <summary>
        /// This method is to get the angle from value for circular gauge.
        /// </summary>
        /// <param name="currentValue">represent the current value of axis.</param>
        /// <param name="maximumValue">represents the maximum value.</param>
        /// <param name="minimumValue">represents the minimum value.</param>
        /// <param name="startAngle">represents the start of the angle.</param>
        /// <param name="endAngle">represents the end of the angle.</param>
        /// <param name="isClockWise">represents the direction of the axis.</param>
        /// <returns>return the point locations.</returns>
        internal static double GetAngleFromValue(double currentValue, double maximumValue, double minimumValue, double startAngle, double endAngle, bool isClockWise)
        {
            endAngle -= IsCompleteAngle(startAngle, endAngle) ? 0.0001 : 0;
            startAngle -= 90;
            endAngle -= 90;
            double angle = 0;
            if (isClockWise)
            {
                angle = ((currentValue - minimumValue) * (GetDegreeValue(startAngle, endAngle)
                    / (maximumValue - minimumValue))) + startAngle;
            }
            else
            {
                angle = endAngle - ((currentValue - minimumValue) * (GetDegreeValue(startAngle, endAngle) /
                    (maximumValue - minimumValue)));
                angle = angle < 0 ? 360 + angle : angle;
            }

            angle = Math.Round(angle) >= 360 ? (angle - 360) : Math.Round(angle) < 0 ? (360 + angle) : angle;
            return angle;
        }

        /// <summary>
        /// This method to get the location from angle for circular gauge.
        /// </summary>
        /// <param name="degree">represent the value in degree to calculate the location of angle.</param>
        /// <param name="radius">represents the radius value to calculate the location of angle.</param>
        /// <param name="center">represents the center point to calculate the location of angle.</param>
        /// <returns>return the point locations.</returns>
        internal static PointF GetLocationFromAngle(double degree, double radius, PointF center)
        {
            double radian = (degree * Math.PI) / 180;
            return new PointF
            {
                X = (float)((Math.Cos(radian) * radius) + center.X),
                Y = (float)((Math.Sin(radian) * radius) + center.Y),
            };
        }

        /// <summary>
        /// This method is to get the value from angle for circular gauge.
        /// </summary>
        /// <param name="angle">represent the angle of axis.</param>
        /// <param name="maximumValue">represents the maximum value.</param>
        /// <param name="minimumValue">represents the minimum value.</param>
        /// <param name="startAngle">represents the start of the angle.</param>
        /// <param name="endAngle">represents the end of the angle.</param>
        /// <param name="isClockWise">represents the direction of the axis.</param>
        /// <returns>return the point locations.</returns>
        internal static double GetValueFromAngle(double angle, double maximumValue, double minimumValue, double startAngle, double endAngle, bool isClockWise)
        {
            endAngle -= IsCompleteAngle(startAngle, endAngle) ? 0.0001 : 0;
            angle = angle < startAngle ? (angle + 360) : angle;
            if (isClockWise)
            {
                return (((angle - startAngle) / GetDegreeValue(startAngle, endAngle)) * (maximumValue - minimumValue)) + minimumValue;
            }
            else
            {
                return maximumValue - ((((angle - startAngle) / GetDegreeValue(startAngle, endAngle)) * (maximumValue - minimumValue)) + minimumValue);
            }
        }

        /// <summary>
        /// This method is to get angle from location for circular gauge.
        /// </summary>
        /// <param name="midPoint">represent the current value of axis.</param>
        /// <param name="location">represents the maximum value.</param>
        /// <returns>return the point locations.</returns>
        internal static double GetAngleFromLocation(PointF midPoint, PointF location)
        {
            double angle = Math.Atan2(location.Y - midPoint.Y, location.X - midPoint.X);
            angle = Math.Round((angle < 0 ? (6.283 + angle) : angle) * (180 / Math.PI)) - 270;
            angle += angle < 0 ? 360 : 0;
            return angle;
        }

        /// <summary>
        /// Represents to measure height and width of the text.
        /// </summary>
        /// <param name="text">represent the text of the axis.</param>
        /// <param name="fontSize">represents the size of the text.</param>
        /// <returns>return the text size.</returns>
        internal static SizeD MeasureText(string text, double fontSize)
        {
            SizeD charSize = new SizeD();
            double width = 0;
            double height = 0;
            for (int i = 0; i < text.Length; i++)
            {
                charSize = GetCharSize(text[i]);
                width += charSize.Width;
                height = Math.Max(charSize.Height, height);
            }

            SizeD size = new SizeD() { Width = (width * fontSize) / BASEFONT, Height = (height * fontSize) / BASEFONT };
            return size;
        }

        /// <summary>
        /// Calculate the center points in the circular gauge.
        /// </summary>
        internal void CalculateMidPoint()
        {
            Rect rectBound = AxisSetting.RectValue;
            double centerX = !string.IsNullOrEmpty(Parent.CenterX) ? SfCircularGauge.StringToNumber(Parent.CenterX, Parent.AvailableSize.Width) :
                rectBound.X + (rectBound.Width / 2);
            double centerY = !string.IsNullOrEmpty(Parent.CenterY) ? SfCircularGauge.StringToNumber(Parent.CenterY, Parent.AvailableSize.Height) :
                rectBound.Y + (rectBound.Height / 2);
            AxisSetting.MidPoint = new PointF { X = (float)centerX, Y = (float)centerY };
        }

        /// <summary>
        /// This method to get the path direction of the circular gauge.
        /// </summary>
        /// <param name="center">represents the value for center.</param>
        /// <param name="start">represents the start range.</param>
        /// <param name="end">represents the end range.</param>
        /// <param name="radius">represents the radius of the range.</param>
        /// <param name="startWidth">represents the startwidth of the range.</param>
        /// <param name="endWidth">represents the endwidth of the range.</param>
        /// <param name="axis">represents the properties of axis.</param>
        /// <param name="range">represents the properties of range.</param>
        /// <returns>return the value as string.</returns>
        internal string GetPathArc(PointF center, double start, double end, double radius, double startWidth, double endWidth, CircularGaugeAxis axis, CircularGaugeRange range)
        {
            string path = string.Empty;
            end -= IsCompleteAngle(start, end) ? 0.0001 : 0;
            double degreeValue = GetDegreeValue(start, end);
            if (range != null && (startWidth != 0 || endWidth != 0))
            {
#pragma warning disable CA1508
                double startRadius = range != null ? (range.Position == PointerRangePosition.Outside ? radius + startWidth : range.Position == PointerRangePosition.Cross
                    && axis.Direction == GaugeDirection.AntiClockWise ? radius - ((endWidth + startWidth) / 2) : radius - startWidth) : radius - startWidth;
                double endRadius = range != null ? (range.Position == PointerRangePosition.Outside ? radius + endWidth : range.Position == PointerRangePosition.Cross &&
                    axis.Direction == GaugeDirection.ClockWise ? radius - ((endWidth + startWidth) / 2) : radius - endWidth) : radius - endWidth;
                double arcRadius = range != null ? (range.Position == PointerRangePosition.Outside ? radius + ((startWidth + endWidth) / 2) :
                    range.Position == PointerRangePosition.Cross ? (radius - ((startWidth + endWidth) / 4) - ((axis.Direction == GaugeDirection.ClockWise ? startWidth : endWidth) / 2)) : radius - ((startWidth + endWidth) / 2)) : radius - ((startWidth + endWidth) / 2);
                double insideArcRadius = range != null && range.Position == PointerRangePosition.Cross ?
                    radius + ((startWidth + endWidth) / 4) - ((axis.Direction == GaugeDirection.ClockWise ? startWidth : endWidth) / 2) : radius;
                double insideEndRadius = range != null && range.Position == PointerRangePosition.Cross && axis.Direction == GaugeDirection.ClockWise ?
                    radius - ((startWidth - endWidth) / 2) : radius;
                double insideStartRadius = range != null && range.Position == PointerRangePosition.Cross && axis.Direction == GaugeDirection.AntiClockWise ?
                    radius + ((startWidth - endWidth) / 2) : radius;
#pragma warning restore CA1508
                return RangeRenderer.GetRangePath(GetLocationFromAngle(start, insideStartRadius, center), GetLocationFromAngle(end, insideEndRadius, center), GetLocationFromAngle(start, startRadius, center), GetLocationFromAngle(end, endRadius, center), insideArcRadius, arcRadius, arcRadius, (degreeValue < 180) ? 0 : 1);
            }
            else
            {
                path = GetCirclePath(
                GetLocationFromAngle(start, radius, center), GetLocationFromAngle(end, radius, center), radius, degreeValue < 180 ? 0 : 1);
            }

            return path;
        }

        /// <summary>
        /// Calculate the outer background path in circular gauge.
        /// </summary>
        /// <param name="axis">represents the properties of axis.</param>
        /// <param name="axisSetting">represents the properties of axis settings.</param>
        internal void DrawAxisOuterLine(CircularGaugeAxis axis, Axis axisSetting)
        {
            if (!string.IsNullOrEmpty(axis.Background))
            {
                axisSetting.AxesOuterBackground = !string.IsNullOrEmpty(axis.Background) ? axis.Background : "transparent";
                axisSetting.AxisOuterPath = GetPathArc(axisSetting.MidPoint, 0, 360, Math.Min(axisSetting.RectValue.Width, axisSetting.RectValue.Height) / 2, 0, 0, axis, null);
            }
            else
            {
                axisSetting.AxisOuterPath = null;
            }
        }

        /// <summary>
        /// Represents to render the circular gauge component.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task Render()
        {
            TitleRender = new TitleRenderer();
            LegendRenderer = new LegendRenderer(Parent);
            if (Parent.Axes != null)
            {
                for (int i = 0; i < Parent.Axes.Count; i++)
                {
                    AxisSetting = new Axis();
                    AxisSetting.AxisVisible = true;
                    AxisSetting.AxisIndex = i;
                    CalculateBounds();
                    if (Parent.LegendSettings != null && Parent.LegendSettings.Visible)
                    {
                        AxisSetting.LegendVisible = true;
                        LegendRenderer.GetLegendOptions(Parent.Axes[i], i);
                    }

                    if (Parent.AllowMargin || Parent.LegendSettings != null)
                    {
                        CalculateMidPoint();
                    }

                    await ComputeSize(Parent.Axes[i]);
                    AxisCollection.Add(AxisSetting);
                }

                for (int j = 0; j < AxisCollection.Count; j++)
                {
                    AxisSetting = AxisCollection[j];
                    RangeRenderer = new RangeRenderer(Parent);
                    PointerRenderer = new PointerRenderer(Parent);
                    AnnotationRenderer = new AnnotationRenderer(Parent);
                    MeasureAxisSize(j);
                    CheckAngles(Parent.Axes[j]);
                    await CalculateAxesRadius(Parent.Axes[j]);
                    await RenderElements(Parent.Axes[j], AxisSetting, j);
                    AxisCollection[j].RangeCollection = RangeRenderer.RangeCollection;
                    AxisCollection[j].PointerCollection = PointerRenderer.PointerCollection;
                    AxisCollection[j].PointerAnimate = PointerRenderer.PointerAnimate;
                    AxisCollection[j].RangeAnimate = PointerRenderer.RangeAnimate;
                    if (Parent.Tooltip != null && Parent.Tooltip.Enable)
                    {
                        TooltipRenderer = new TooltipRenderer(Parent);
                        AxisSetting.TooltipVisible = true;
                    }
                }

                if (Parent.LegendSettings != null && Parent.LegendSettings.Visible)
                {
                    LegendRenderer.RenderLegend();
                }
            }
            else
            {
                AxisSetting = new Axis();
                RangeRenderer = new RangeRenderer(Parent);
                AxisSetting.AxisVisible = true;
                CalculateBounds();
                RenderBorder();
                RenderTitle(AxisSetting);
                AxisCollection.Add(AxisSetting);
            }
        }

        /// <summary>
        /// This method is to set the color to the major tick lines in the axis.
        /// </summary>
        /// <param name="axis">represent the properties of axis.</param>
        /// <param name="tickValue">represents the value of ticks.</param>
        /// <returns>return the point locations.</returns>
        internal string SetMajorTickColor(CircularGaugeAxis axis, double tickValue)
        {
            string rangeColor = axis.MajorTicks != null && axis.MajorTicks.UseRangeColor ? GetRangeColor(tickValue, axis) : string.Empty;
            rangeColor = !string.IsNullOrEmpty(rangeColor) ? rangeColor : (axis.MajorTicks != null && !string.IsNullOrEmpty(axis.MajorTicks.Color)
                ? axis.MajorTicks.Color : Parent.ThemeStyles.MajorTickColor);
            return rangeColor;
        }

        /// <summary>
        /// This method is to set the color to the minor tick lines in the axis.
        /// </summary>
        /// <param name="axis">represent the value of axis.</param>
        /// <param name="index">represents the axis index of the value.</param>
        /// <returns>return the color to range.</returns>
        internal string SetMinorTickColor(CircularGaugeAxis axis, double index)
        {
            string rangeColor = axis.MinorTicks != null && axis.MinorTicks.UseRangeColor ? GetRangeColor(index, axis) : string.Empty;
            rangeColor = !string.IsNullOrEmpty(rangeColor) ? rangeColor : (axis.MinorTicks != null && !string.IsNullOrEmpty(axis.MinorTicks.Color)
                ? axis.MinorTicks.Color : Parent.ThemeStyles.MinorTickColor);
            return rangeColor;
        }

        /// <summary>
        /// Represents to render the title in circular gauge.
        /// </summary>
        /// <param name="axisSetting">represent the properties of axis.</param>
        internal void RenderTitle(Axis axisSetting)
        {
            TitleRender.Description = string.IsNullOrEmpty(Parent.Description) ? Parent.Title : Parent.Description;
            string fontSize = Parent.TitleStyle != null && !string.IsNullOrEmpty(Parent.TitleStyle.Size) ? Parent.TitleStyle.Size :
                (Parent.ThemeStyles != null && !string.IsNullOrEmpty(Parent.ThemeStyles.FontSize)) ? Parent.ThemeStyles.FontSize : "15px";
            string fontWeight = Parent.TitleStyle != null && !string.IsNullOrEmpty(Parent.TitleStyle.FontWeight) ? Parent.TitleStyle.FontWeight :
                (Parent.ThemeStyles != null && !string.IsNullOrEmpty(Parent.ThemeStyles.TitleFontWeight)) ? Parent.ThemeStyles.TitleFontWeight : "normal";
            string fontStyle = Parent.TitleStyle != null && !string.IsNullOrEmpty(Parent.TitleStyle.FontStyle) ? Parent.TitleStyle.FontStyle : "normal";
            string fontFamily = Parent.TitleStyle != null && !string.IsNullOrEmpty(Parent.TitleStyle.FontFamily) ? Parent.TitleStyle.FontFamily : (Parent.ThemeStyles != null && !string.IsNullOrEmpty(Parent.ThemeStyles.FontFamily)) ? Parent.ThemeStyles.FontFamily : "Segoe UI";
            TitleRender.TitleFontStyle = "font-size:" + fontSize + "; font-style:" + fontStyle + "; font-weight:" + fontWeight + "; font-family:" + fontFamily + ";user-select:none";
            TitleRender.TitleOpacity = Parent.TitleStyle != null ? Parent.TitleStyle.Opacity : 1;
            TitleRender.TitleSetting = new TextSetting
            {
                X = axisSetting.ActualWidth / 2,
                Y = Parent.Margin != null ? Parent.Margin.Top + (3 * (TitleRender.TitleSize.Height / 4)) : (10 + (3 * (TitleRender.TitleSize.Height / 4))),
                Anchor = "middle",
                Text = Parent.Title,
                Fill = Parent.TitleStyle != null && !string.IsNullOrEmpty(Parent.TitleStyle.Color) ? Parent.TitleStyle.Color : (Parent.ThemeStyles != null ? Parent.ThemeStyles.TitleFontColor : "transparent"),
            };
        }

        /// <summary>
        /// Represents to set the color to the label.
        /// </summary>
        /// <param name="axis">represent the properties of the axis.</param>
        /// <param name="labelValue">represents the value of the label.</param>
        /// <returns>return the point locations.</returns>
        internal string SetLabelColor(CircularGaugeAxis axis, double labelValue)
        {
            string rangeColor = (axis.LabelStyle != null && axis.LabelStyle.UseRangeColor) ? GetRangeColor(labelValue, axis) : string.Empty;
            rangeColor = !string.IsNullOrEmpty(rangeColor) ? rangeColor : (axis.LabelStyle != null && axis.LabelStyle.Font != null && !string.IsNullOrEmpty(axis.LabelStyle.Font.Color) ? axis.LabelStyle.Font.Color : Parent.ThemeStyles.LabelColor);
            return rangeColor;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal void ComponentDispose()
        {
            Parent = null;
            AxisCollection = null;
            AxisSetting = null;
            RangeRenderer?.Dispose();
            PointerRenderer?.Dispose();
            LegendRenderer?.Dispose();
            TooltipRenderer?.Dispose();
            AnnotationRenderer?.Dispose();
            RangeRenderer = null;
            PointerRenderer = null;
            LegendRenderer = null;
            TooltipRenderer = null;
            TitleRender = null;
            farSizes = null;
            RangeColors = null;
        }

        private static SizeD GetCharSize(char character)
        {
            double charWidth = BASEFONT * 0.75;
            FontInfo font = new FontInfo();
            font.Chars.TryGetValue(character, out charWidth);
            return new SizeD() { Width = charWidth * BASEFONT / 16.0, Height = BASEFONT * 1.3 };
        }

        private static double CalculateSum(int index, List<double> values)
        {
            double sum = 0;
            for (; index < values.Count; index++)
            {
                sum += values[index];
            }

            return sum;
        }

        private static double GetOffsetAxisLabelSize(double angle, double size)
        {
            return ((angle >= 20 && angle <= 60) || (angle >= 120 && angle <= 160) || (angle >= 200 && angle <= 240) ||
            (angle >= 300 && angle <= 340)) ? size / 5 : 0;
        }

        private static PointF GetAxisLabelStartPosition(PointF actualLocation, double textWidth, string anchorPosition)
        {
            if (anchorPosition == "end")
            {
                actualLocation.X = (float)(actualLocation.X - textWidth);
            }
            else if (anchorPosition == "middle")
            {
                actualLocation.X = (float)(actualLocation.X - (textWidth / 2));
            }
            else
            {
                actualLocation.X = actualLocation.X;
            }

            return actualLocation;
        }

        private static double CalculateNiceInterval(double maxValue, double minValue, double radius, double degree)
        {
            double delta = maxValue - minValue;
            double circumference = 2 * Math.PI * radius * (degree / 360);
            double intervalCount = Math.Max(circumference * (0.533 * 3 / 100), 1);
            double tickInterval = delta / intervalCount;
            double minInterval = Math.Pow(10, Math.Floor(Math.Log(tickInterval) / Math.Log(10)));
            double[] niceIntervalCount = { 10, 5, 2, 1 };
            for (var i = 0; i < niceIntervalCount.Length; i++)
            {
                double tickIntervals = niceIntervalCount[i];
                double currentTickInterval = minInterval * tickIntervals;
                if (intervalCount < (delta / currentTickInterval))
                {
                    break;
                }

                tickInterval = currentTickInterval;
            }

            return tickInterval;
        }

        private static bool FindAxisLabelCollision(PointF previousLocation, double previousWidth, double previousHeight, PointF currentLocation, double currentWidth, double currentHeight)
        {
            return (previousLocation.X > (currentLocation.X + currentWidth)) ||
            ((previousLocation.X + previousWidth) < currentLocation.X) ||
            ((previousLocation.Y + previousHeight) < currentLocation.Y) ||
            (previousLocation.Y > (currentLocation.Y + currentHeight));
        }

        private static void CheckAngles(CircularGaugeAxis axis)
        {
            axis.AngleStart = axis.StartAngle >= 360 ? 360 : axis.StartAngle <= -360 ? -360 : axis.StartAngle;
            axis.AngleEnd = axis.EndAngle >= 360 ? 360 : axis.EndAngle <= -360 ? -360 : axis.EndAngle;
        }

        private void CalculateBounds()
        {
            bool isUpper = false;
            bool isLower = false;
            double width = 0;
            double height = 0;
            double radius = 0;
            TitleRender.TitleSize = new SizeD
            {
                Width = 0,
                Height = 0
            };
            if (!string.IsNullOrEmpty(Parent.Title))
            {
                double fontSize = Parent.TitleStyle != null && !string.IsNullOrEmpty(Parent.TitleStyle.Size) && !Parent.TitleStyle.Size.Contains("px", StringComparison.InvariantCulture) ?
                    Convert.ToDouble(Parent.TitleStyle.Size.Replace("px", string.Empty, StringComparison.InvariantCulture), culture) : 15;
                TitleRender.TitleSize = MeasureText(Parent.Title, fontSize);
            }

            double top = (Parent.Margin != null ? Parent.Margin.Top : 10) + (Parent.Border != null ? Parent.Border.Width : 0) + TitleRender.TitleSize.Height;
            double left = (Parent.Margin != null ? Parent.Margin.Left : 10) + (Parent.Border != null ? Parent.Border.Width : 0);

            // When navigating to another page from the page with CircularGauge component before its loading, the component throws a error. To resolve this error, the below condition check is used.
            if (Parent.AvailableSize != null)
            {
                width = Parent.AvailableSize.Width - left - (Parent.Margin != null ? Parent.Margin.Right : 10) - (Parent.Border != null ? Parent.Border.Width : 0);
                height = Parent.AvailableSize.Height - top - (Parent.Margin != null ? Parent.Margin.Bottom : 10) - (Parent.Border != null ? Parent.Border.Width : 0);
                radius = Math.Min(width, height) / 2;
                if ((!string.IsNullOrEmpty(Parent.Width) && (Parent.Width.IndexOf("%", StringComparison.InvariantCulture) != -1 || Parent.Width.IndexOf("px", StringComparison.InvariantCulture) != -1)) || Parent.AvailableSize.Width != 0)
                {
                    AxisSetting.Width = Parent.AvailableSize.Width - left - (Parent.Margin != null ? Parent.Margin.Right : 10) - (Parent.Border != null ? Parent.Border.Width : 0);
                }

                if ((!string.IsNullOrEmpty(Parent.Height) && (Parent.Height.IndexOf("%", StringComparison.InvariantCulture) != -1 || Parent.Height.IndexOf("px", StringComparison.InvariantCulture) != -1)) || Parent.AvailableSize.Height != 0)
                {
                    AxisSetting.Height = Parent.AvailableSize.Height - top - (Parent.Border != null ? Parent.Border.Width : 0) - (Parent.Margin != null ? Parent.Margin.Bottom : 10);
                }
            }

            double boundRadius = Math.Min(AxisSetting.Width, AxisSetting.Height) / 2;
            if (Parent.MoveToCenter && Parent.Axes.Count == 1 && string.IsNullOrEmpty(Parent.CenterX) && string.IsNullOrEmpty(Parent.CenterY))
            {
                AxisSetting.RectValue = new Rect { X = left, Y = top, Width = AxisSetting.Width, Height = AxisSetting.Height };
            }

            if (!Parent.AllowMargin && Parent.LegendSettings == null)
            {
                // When navigating to another page from the page with circular-gauge component before its loading, the component throws a error. To resolve this error, the below condition check is used.
                for (int j = 0; j < (Parent.Axes != null ? Parent.Axes.Count : 0); j++)
                {
                    bool isUpperAngle = Parent.Axes[j].StartAngle >= 270 && Parent.Axes[j].StartAngle <= 360 && Parent.Axes[j].EndAngle >= 0 && Parent.Axes[j].EndAngle <= 90;
                    bool isLowerAngle = Parent.Axes[j].StartAngle <= 90 && Parent.Axes[j].StartAngle <= 180 &&
                    Parent.Axes[j].EndAngle >= 180 && Parent.Axes[j].EndAngle >= 270 && Parent.Axes[j].StartAngle != 0 && Parent.Axes[j].EndAngle != 360;
                    isUpper = isUpperAngle ? isUpperAngle : isUpper;
                    isLower = isLowerAngle ? isLowerAngle : isLower;
                    double radiusPercent = Parent.Axes[j].Radius != null ? radius * (Convert.ToDouble(Parent.Axes[0].Radius.Split("%")[0], culture) / 100) : radius;
                    RadiusAndCenterCalculation(top, left, width, height, radius, isUpperAngle, isLowerAngle, true, radiusPercent, isUpper, isLower);
                }
            }
            else
            {
                AxisSetting.RectValue = new Rect
                {
                    X = left + (AxisSetting.Width / 2) - boundRadius,
                    Y = top + (AxisSetting.Height / 2) - boundRadius,
                    Width = Convert.ToDouble(boundRadius) * 2,
                    Height = Convert.ToDouble(boundRadius) * 2
                };
            }

            if (Parent.AllowMargin || Parent.LegendSettings != null)
            {
                AxisSetting.GaugeRect = AxisSetting.RectValue;
            }
        }

        private void RadiusAndCenterCalculation(double top, double left, double width, double height, double radius, bool isUpperAngle, bool isLowerAngle, bool isFullPercent, double? radiusPercent, bool isUpper, bool isLower)
        {
            Rect rect = new Rect();
            double bottom = (Parent.Margin != null ? Parent.Margin.Bottom : 0) + (Parent.Border != null ? Parent.Border.Width : 0);
            double widthRadius = 0;
            double centerX = 0;
            double centerY = 0;
            if (Parent.MoveToCenter && Parent.Axes.Count == 1 && string.IsNullOrEmpty(Parent.CenterX) && string.IsNullOrEmpty(Parent.CenterY))
            {
                rect = new Rect { X = left, Y = top, Width = width, Height = height };
            }
            else
            {
                if (width > height && ((isLowerAngle && isLower) || (isUpperAngle && isUpper)))
                {
                    widthRadius = width / 2;
                    double heightValue = isUpper && isLower ? (height / 2) : (height * 0.75);
                    if (widthRadius > heightValue)
                    {
                        widthRadius = heightValue;
                    }

                    rect = new Rect
                    {
                        X = left + (width / 2) - widthRadius,
                        Y = top + (height / 2) - widthRadius,
                        Width = widthRadius * 2,
                        Height = widthRadius * 2
                    };
                }
                else
                {
                    if (height > width)
                    {
                        double heightRadius = height / 2;
                        rect = new Rect { X = left + (width / 2) - radius, Y = top + (height / 2) - heightRadius, Width = radius * 2, Height = heightRadius * 2 };
                    }
                    else
                    {
                        rect = new Rect { X = left + (width / 2) - radius, Y = top + (height / 2) - radius, Width = radius * 2, Height = radius * 2 };
                    }
                }
            }

            AxisSetting.GaugeRect = AxisSetting.RectValue = rect;
            centerX = Parent.CenterX != null ?
                SfCircularGauge.StringToNumber(Parent.CenterX, Parent.AvailableSize.Width) : AxisSetting.GaugeRect.X + (AxisSetting.GaugeRect.Width / 2);
            centerY = (isUpperAngle || isLowerAngle) ? (isUpperAngle ?
                      (AxisSetting.GaugeRect.Height * 0.75) + AxisSetting.GaugeRect.Y - bottom
                      : (AxisSetting.GaugeRect.Height * 0.25) + AxisSetting.GaugeRect.Y) : AxisSetting.GaugeRect.Y + (AxisSetting.GaugeRect.Height / 2);
            centerY = !isFullPercent && (isUpperAngle || isLowerAngle) ? (AxisSetting.GaugeRect.Height / 2) + AxisSetting.GaugeRect.Y + (Convert.ToDouble(radiusPercent, culture) * 0.75 * 0.6) : centerY;
#pragma warning disable CS8073
            if (Parent.Axes.Count > 1 && AxisSetting.MidPoint != null)
#pragma warning restore CS8073
            {
                isUpper = isUpperAngle ? isUpperAngle : isUpper;
                isLower = isLowerAngle ? isLowerAngle : isLower;
                if (isUpper && isLower)
                {
                    centerY = (Parent.AvailableSize.Height / 2) - bottom;
                }
            }

            AxisSetting.MidPoint = new PointF { X = Convert.ToSingle(centerX), Y = Convert.ToSingle(centerY) };
        }

        private void RenderBorder()
        {
            AxisSetting.OuterBackground = !string.IsNullOrEmpty(Parent.Background) ? Parent.Background : (Parent.ThemeStyles != null ? Parent.ThemeStyles.BackgroundColor : "transparent");
            AxisSetting.BorderStrokeWidth = Parent.Border != null ? Parent.Border.Width : 0;
            AxisSetting.BorderStrokeColor = Parent.Border != null && !string.IsNullOrEmpty(Parent.Border.Color) ? Parent.Border.Color : "transparent";
            double borderStrokeWidth = AxisSetting.BorderStrokeWidth;
            if (borderStrokeWidth > 0 || !string.IsNullOrEmpty(Parent.Background) || Parent.Axes != null)
            {
                double actualHeight = Parent.AvailableSize.Height;
                AxisSetting.ActualWidth = Parent.AvailableSize.Width;
                AxisSetting.BorderX = borderStrokeWidth / 2;
                AxisSetting.BorderY = borderStrokeWidth / 2;
                AxisSetting.BorderRectWidth = AxisSetting.ActualWidth - borderStrokeWidth;
                AxisSetting.BorderRectHeight = actualHeight - borderStrokeWidth;
                AxisSetting.RectValue = new Rect
                {
                    X = AxisSetting.RectValue.X - (borderStrokeWidth / 2),
                    Y = AxisSetting.RectValue.Y - (borderStrokeWidth / 2),
                    Width = AxisSetting.RectValue.Width - borderStrokeWidth,
                    Height = AxisSetting.RectValue.Height - borderStrokeWidth,
                };
            }
        }

        private void CalculateVisibleRange(CircularGaugeAxis axis)
        {
            double interval = axis.MajorTicks != null ? axis.MajorTicks.Interval : 0;
            double minimumValue = Math.Min(axis.Minimum != 0 ? axis.Minimum : 0, axis.Maximum);
            double maximumValue = Math.Max(axis.Minimum, axis.Maximum != 0 ? axis.Maximum : 100);
            if (axis.Pointers != null)
            {
                for (int i = 0; i < axis.Pointers.Count; i++)
                {
                    double pointerValue = axis.AxisValues.PointerValue[i] != 0 ? ((axis.AxisValues.PointerValue[i] < minimumValue) ? minimumValue :
                        ((axis.AxisValues.PointerValue[i] > maximumValue) ? maximumValue : axis.AxisValues.PointerValue[i])) : minimumValue;
                    minimumValue = axis.Minimum != 0 ? Math.Min(pointerValue, minimumValue) : minimumValue;
                    maximumValue = axis.Maximum != 0 ? Math.Max(pointerValue, maximumValue) : maximumValue;
                }
            }

            minimumValue = (minimumValue == maximumValue) ? ((interval != 0) ? minimumValue - interval : minimumValue - 1) : minimumValue;
            AxisSetting.TickLineSetting = new TickLine
            {
                Minimum = minimumValue,
                Maximum = maximumValue,
                Interval = interval,
            };
            AxisSetting.TickLineSetting.Interval = CalculateNumericIntervals(axis);
        }

        private double CalculateNumericIntervals(CircularGaugeAxis axis)
        {
            if (axis.MajorTicks != null && axis.MajorTicks.Interval != 0)
            {
                return axis.MajorTicks.Interval;
            }
            else
            {
                double totalAngle = axis.AngleEnd - axis.AngleStart;
                totalAngle = totalAngle <= 0 ? totalAngle + 360 : totalAngle;
                return CalculateNiceInterval(AxisSetting.TickLineSetting.Maximum, AxisSetting.TickLineSetting.Minimum, AxisSetting.CurrentRadius != 0 ? AxisSetting.CurrentRadius : AxisSetting.RectValue.Width / 2, totalAngle);
            }
        }

        private string DrawAxisLine(CircularGaugeAxis axis)
        {
            bool isCompleteAngle = IsCompleteAngle(axis.AngleStart, axis.AngleEnd);
            AxisSetting.AxisLinePath = GetPathArc(AxisSetting.MidPoint, (!isCompleteAngle ? axis.AngleStart : 0) - 90, (!isCompleteAngle ? axis.AngleEnd : 360) - 90, AxisSetting.CurrentRadius, 0, 0, axis, null);
            AxisSetting.Stroke = axis.LineStyle != null && !string.IsNullOrEmpty(axis.LineStyle.Color) ? axis.LineStyle.Color : Parent.ThemeStyles.LineColor;
            AxisSetting.StrokeWidth = axis.LineStyle != null ? axis.LineStyle.Width : 2;
            AxisSetting.DashArray = axis.LineStyle != null ? axis.LineStyle.DashArray : string.Empty;
            return string.Empty;
        }

        private async Task RenderElements(CircularGaugeAxis axis, Axis axisSetting, int index)
        {
            RenderBorder();
            RenderTitle(axisSetting);
            await RenderAxes(axis, axisSetting, index);
        }

        private async Task RenderAxes(CircularGaugeAxis axis, Axis axisSetting, int index)
        {
            DrawAxisOuterLine(axis, axisSetting);
            DrawAxisLine(axis);
            if (axis.Ranges != null)
            {
                RangeRenderer.DrawAxisRange(axis);
            }

            DrawMajorTickLines(axis);
            DrawMinorTickLines(axis);
            DrawAxisLabels(axis);
            if (axis.Pointers != null)
            {
                PointerRenderer.DrawPointers(axis, index);
            }

            if (axis.Annotations != null)
            {
                AxisSetting.AnnotationVisible = true;
                await AnnotationRenderer.RenderAnnotation(axis);
                Parent.AxisRenderer.AxisCollection[index].Annotations = AnnotationRenderer.AnnotationCollection;
            }
        }

        private async Task CalculateAxesRadius(CircularGaugeAxis axis)
        {
            double rangeMaximumRadius = 0;
            double xmarginDifference = (Parent.Margin != null ? Parent.Margin.Left : 10) +
                (Parent.Margin != null ? Parent.Margin.Right : 10);
            double ymarginDifference = (Parent.Margin != null ? Parent.Margin.Top : 10) +
                (Parent.Margin != null ? Parent.Margin.Bottom : 10);
            double totalRadius = Math.Min(AxisSetting.RectValue.Width, AxisSetting.RectValue.Height) / 2;
            AxisSetting.CurrentRadius = !string.IsNullOrEmpty(axis.Radius) ? SfCircularGauge.StringToNumber(axis.Radius, totalRadius) : totalRadius;
            if (axis.Ranges != null)
            {
                double maximumRadius = 0;
                for (int i = 0; i < axis.Ranges.Count; i++)
                {
                    maximumRadius = rangeMaximumRadius;
                    string radius = string.Empty;
                    if (!string.IsNullOrEmpty(axis.Ranges[i].Radius) && axis.Ranges[i].Radius.Contains("%", StringComparison.InvariantCulture))
                    {
                        radius = axis.Ranges[i].Radius.Replace("%", string.Empty, StringComparison.InvariantCulture);
                        if (radius.Contains(".", StringComparison.InvariantCulture))
                        {
                            radius = radius.Split('.')[0];
                        }
                    }

                    rangeMaximumRadius = !string.IsNullOrEmpty(axis.Ranges[i].Radius) ? (axis.Ranges[i].Radius.IndexOf("%", StringComparison.InvariantCulture) > 0 ?
                        int.Parse(radius, culture) : 100) : 0;
                    rangeMaximumRadius = Math.Max(rangeMaximumRadius, maximumRadius);
                }
            }

            AxisSetting.CurrentRadius = (rangeMaximumRadius > 100 && string.IsNullOrEmpty(axis.Radius)) ?
                (AxisSetting.CurrentRadius * 100) / rangeMaximumRadius : AxisSetting.CurrentRadius;
            AxisSetting.CurrentRadius = AxisSetting.CurrentRadius - AxisSetting.NearSize;
            if (Parent.MoveToCenter && Parent.Axes.Count == 1 &&
            string.IsNullOrEmpty(Parent.CenterX) && string.IsNullOrEmpty(Parent.CenterY))
            {
                double axisStart = axis.AngleStart;
                bool isCompleteAngle = IsCompleteAngle(axisStart, axis.AngleEnd);
                axisStart = !isCompleteAngle ? axisStart : 0;
                PointF startPoint = GetLocationFromAngle(axisStart - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint);
                double axisEnd = !isCompleteAngle ? axis.AngleEnd : 360;
                axisEnd -= IsCompleteAngle(axisStart, axisEnd) ? 0.0001 : 0;
                PointF endPoint = GetLocationFromAngle(axisEnd - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint);
                double startXDifference = 0;
                double startYDifference = 0;
                double endXDifference = 0;
                double endYDifference = 0;
                PointF newPoint;
                if (axisStart < axisEnd || Math.Abs(axisStart - axisEnd) > 90)
                {
                    if (axisStart >= 270 && axisStart <= 360 && ((axisEnd > 270 && axisEnd <= 360) || (axisEnd >= 0 && axisEnd <= 180)))
                    {
                        startXDifference = Math.Abs(AxisSetting.RectValue.X - Math.Abs(startPoint.X - AxisSetting.RectValue.X));
                        newPoint = (axisEnd <= 360 && axisEnd >= 270) ? AxisSetting.MidPoint : (axisEnd <= 90) ? endPoint :
                            GetLocationFromAngle(90 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint);
                        endXDifference = Math.Abs(newPoint.X - AxisSetting.RectValue.Width);
                        startPoint = (axisEnd <= 360 && axisEnd >= 270) ? endPoint :
                            GetLocationFromAngle(360 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint);
                        startYDifference = Math.Abs(startPoint.Y - AxisSetting.RectValue.Y);
                        endPoint = ((axisEnd <= 360 && axisEnd >= 270) || (axisEnd >= 0 && axisEnd < 90)) ?
                            AxisSetting.MidPoint : (axisEnd >= 90 && axisEnd <= 180) ? endPoint :
                                GetLocationFromAngle(180 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint);
                        endYDifference = Math.Abs(endPoint.Y - (AxisSetting.RectValue.Y + AxisSetting.RectValue.Height));
                    }
                    else if (axisStart >= 0 && axisStart < 90 && axisEnd >= 0 && axisEnd <= 270)
                    {
                        startYDifference = Math.Abs(startPoint.Y - AxisSetting.RectValue.Y);
                        newPoint = (axisEnd >= 180) ? GetLocationFromAngle(180 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint) :
                            endPoint;
                        endYDifference = Math.Abs(newPoint.Y - (AxisSetting.RectValue.Y + AxisSetting.RectValue.Height));
                        startPoint = (axisEnd >= 180) ? endPoint : AxisSetting.MidPoint;
                        startXDifference = Math.Abs(AxisSetting.RectValue.X - Math.Abs(startPoint.X - AxisSetting.RectValue.X));
                        endPoint = (axisEnd >= 90) ? GetLocationFromAngle(90 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint) : endPoint;
                        endXDifference = Math.Abs(endPoint.X - AxisSetting.RectValue.Width);
                    }
                    else if (axisStart >= 90 && axisStart < 180 && axisEnd > 90 && axisEnd <= 360)
                    {
                        newPoint = (axisEnd <= 180) ? AxisSetting.MidPoint : (axisEnd >= 270) ?
                            GetLocationFromAngle(270 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint) : endPoint;
                        startXDifference = Math.Abs(newPoint.X - AxisSetting.RectValue.X);
                        endXDifference = Math.Abs(startPoint.X - AxisSetting.RectValue.Width);
                        startPoint = (axisEnd > 270) ? GetLocationFromAngle(axisEnd - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint) :
                            AxisSetting.MidPoint;
                        startYDifference = Math.Abs(AxisSetting.RectValue.Y - startPoint.Y);
                        endPoint = (axisEnd >= 180) ? GetLocationFromAngle(180 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint) : endPoint;
                        endYDifference = Math.Abs(endPoint.Y - (AxisSetting.RectValue.Y + AxisSetting.RectValue.Height));
                    }
                    else if (axisStart >= 180 && axisStart <= 270 && ((axisEnd <= 360 && axisEnd >= 270) ||
                      (axisEnd <= 180 && axisEnd >= 0)))
                    {
                        newPoint = (axisEnd > 180 && axisEnd < 270) ? endPoint :
                            GetLocationFromAngle(270 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint);
                        startXDifference = Math.Abs(AxisSetting.RectValue.X - Math.Abs(newPoint.X - AxisSetting.RectValue.X));
                        newPoint = (axisEnd >= 180 && axisEnd <= 360) ? AxisSetting.MidPoint : endPoint;
                        endXDifference = Math.Abs(newPoint.X - AxisSetting.RectValue.Width);
                        newPoint = (axisEnd > 180 && axisEnd < 270) ? AxisSetting.MidPoint : (axisEnd >= 270 && axisEnd <= 360) ?
                            endPoint : GetLocationFromAngle(360 - 90, AxisSetting.CurrentRadius, AxisSetting.MidPoint);
                        startYDifference = Math.Abs(newPoint.Y - AxisSetting.RectValue.Y);
                        endYDifference = Math.Abs(startPoint.Y - (AxisSetting.RectValue.Y + AxisSetting.RectValue.Height));
                    }

                    if ((startXDifference > 0 || endXDifference > 0) && (startYDifference > 0 || endYDifference > 0))
                    {
                        double xdifference = Math.Abs((startXDifference + endXDifference) - xmarginDifference);
                        double ydifference = Math.Abs((startYDifference + endYDifference) - ymarginDifference);
                        double midPointX = AxisSetting.MidPoint.X - (startXDifference / 2) + (endXDifference / 2);
                        double midPointY = AxisSetting.MidPoint.Y - (startYDifference / 2) + (endYDifference / 2);
                        AxisSetting.MidPoint = new PointF((float)midPointX, (float)midPointY);
                        totalRadius = (Math.Min(AxisSetting.Width, AxisSetting.Height) / 2) +
                            (Math.Min(xdifference, ydifference) / 2);
                        AxisSetting.CurrentRadius = (!string.IsNullOrEmpty(axis.Radius) ? SfCircularGauge.StringToNumber(axis.Radius, totalRadius) : totalRadius) - AxisSetting.NearSize;
                    }
                }
            }

            AxisSetting.TickLineSetting.Interval = CalculateNumericIntervals(axis);
            RadiusCalculateEventArgs arguments = new RadiusCalculateEventArgs()
            {
                Cancel = false,
                CurrentRadius = AxisSetting.CurrentRadius,
                MidPoint = AxisSetting.MidPoint,
            };
            await SfBaseUtils.InvokeEvent<RadiusCalculateEventArgs>(Parent.CircularGaugeEvents?.OnRadiusCalculate, arguments);
            await CalculateVisibleLabels(axis);
        }

        private string GetCirclePath(PointF start, PointF end, double radius, double clockWise)
        {
            return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " A " + radius.ToString(culture) + SPACE +
            radius.ToString(culture) + " 0 " + clockWise.ToString(culture) + " 1 " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture);
        }

        private void DrawMajorTickLines(CircularGaugeAxis axis)
        {
            for (double i = AxisSetting.TickLineSetting.Minimum; i <= AxisSetting.TickLineSetting.Maximum; i += AxisSetting.TickLineSetting.Interval)
            {
                AxisSetting.MajorTickValues.Add(i);
                CalculateMajorAxisTicks(i, axis);
            }

            AxisSetting.MajorTickDashArray = axis.MajorTicks != null ? axis.MajorTicks.DashArray : AxisSetting.MajorTickDashArray;
        }

        private void CalculateMajorAxisTicks(double tickValue, CircularGaugeAxis axis)
        {
            double axisLineWidth = (AxisSetting.StrokeWidth / 2) + (axis.MajorTicks != null ? axis.MajorTicks.Offset : 0);
            double angle = GetAngleFromValue(tickValue, AxisSetting.TickLineSetting.Maximum, AxisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise);
            double majorTickHeight = axis.MajorTicks != null ? axis.MajorTicks.Height : 10;
            PointF startPoint = GetLocationFromAngle(angle, AxisSetting.CurrentRadius + (axis.MajorTicks != null ? (axis.MajorTicks.Position == Position.Outside ? axisLineWidth : axis.MajorTicks.Position == Position.Cross ? (majorTickHeight / 2) - axis.MajorTicks.Offset : -axisLineWidth) : -axisLineWidth), AxisSetting.MidPoint);
            PointF endPoint = GetLocationFromAngle(angle, AxisSetting.CurrentRadius + (axis.MajorTicks != null ? (axis.MajorTicks.Position == Position.Outside ? axisLineWidth : axis.MajorTicks.Position == Position.Cross ? (majorTickHeight / 2) - axis.MajorTicks.Offset : -axisLineWidth) : -axisLineWidth) + (axis.MajorTicks != null ? (axis.MajorTicks.Position == Position.Outside ? majorTickHeight : -majorTickHeight) : -10), AxisSetting.MidPoint);
            AxisSetting.MajorTickLineStroke.Add(SetMajorTickColor(axis, tickValue));
            AxisSetting.MajorTickLineStrokeWidth = axis.MajorTicks != null ? axis.MajorTicks.Width : 2;
            AxisSetting.MajorTickLinePath.Add("M " + startPoint.X.ToString(culture) + SPACE + startPoint.Y.ToString(culture) + " L " + endPoint.X.ToString(culture) + SPACE + endPoint.Y.ToString(culture) + SPACE);
        }

        private string GetRangeColor(double index, CircularGaugeAxis axis)
        {
            string rangeColor = string.Empty;
            for (int i = 0; i < axis.Ranges.Count; i++)
            {
                double minimumRange = Math.Min(axis.AxisValues.RangeStart[i], axis.AxisValues.RangeEnd[i]);
                double maximumRange = Math.Max(axis.AxisValues.RangeStart[i], axis.AxisValues.RangeEnd[i]);
                if (index >= minimumRange && maximumRange >= index)
                {
                    rangeColor = !string.IsNullOrEmpty(RangeRenderer.RangeCollection[i].RangeFillColor) ? RangeRenderer.RangeCollection[i].RangeFillColor : RangeColors[i % RangeColors.Length];
                    break;
                }
            }

            return rangeColor;
        }

        private void DrawMinorTickLines(CircularGaugeAxis axis)
        {
            double minorInterval = axis.MinorTicks != null ? axis.MinorTicks.Interval == 0 ? AxisSetting.TickLineSetting.Interval / 2 :
                 axis.MinorTicks.Interval : AxisSetting.TickLineSetting.Interval / 2;
            for (double i = AxisSetting.TickLineSetting.Minimum; i <= AxisSetting.TickLineSetting.Maximum; i += minorInterval)
            {
                if (AxisSetting.MajorTickValues.IndexOf(i) < 0)
                {
                    CalculateMinorAxisTicks(i, axis);
                }
            }

            AxisSetting.MinorTickDashArray = axis.MinorTicks != null ? axis.MinorTicks.DashArray : AxisSetting.MinorTickDashArray;
        }

        private void CalculateMinorAxisTicks(double index, CircularGaugeAxis axis)
        {
            double axisLineWidth = (AxisSetting.StrokeWidth / 2) + (axis.MinorTicks != null ? axis.MinorTicks.Offset : 0);
            double minorTickAngle = GetAngleFromValue(index, AxisSetting.TickLineSetting.Maximum, AxisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise);
            double minorTickHeight = axis.MinorTicks != null ? axis.MinorTicks.Height : 5;
            PointF minorTickStart = GetLocationFromAngle(minorTickAngle, AxisSetting.CurrentRadius + (axis.MinorTicks != null ? (axis.MinorTicks.Position == Position.Outside ? axisLineWidth : axis.MinorTicks.Position == Position.Cross ? (minorTickHeight / 2) - axis.MinorTicks.Offset : -axisLineWidth) : -axisLineWidth), AxisSetting.MidPoint);
            PointF minorTickEnd = GetLocationFromAngle(minorTickAngle, AxisSetting.CurrentRadius + (axis.MinorTicks != null ? (axis.MinorTicks.Position == Position.Outside ? axisLineWidth : axis.MinorTicks.Position == Position.Cross ? (minorTickHeight / 2) - axis.MinorTicks.Offset : -axisLineWidth) : -axisLineWidth) + (axis.MinorTicks != null ? (axis.MinorTicks.Position == Position.Outside ? minorTickHeight : -minorTickHeight) : -5), AxisSetting.MidPoint);
            AxisSetting.MinorTickLineStroke.Add(SetMinorTickColor(axis, index));
            AxisSetting.MinorTickLineStrokeWidth = axis.MinorTicks != null ? axis.MinorTicks.Width : 2;
            AxisSetting.MinorTickPath.Add("M " + minorTickStart.X.ToString(culture) + SPACE + minorTickStart.Y.ToString(culture) + " L " + minorTickEnd.X.ToString(culture) + SPACE + minorTickEnd.Y.ToString(culture) + SPACE);
        }

        private async Task CalculateVisibleLabels(CircularGaugeAxis axis)
        {
            AxisSetting.LabelSettingCollection = new List<LableSetting>();
            for (double i = AxisSetting.TickLineSetting.Minimum; i <= AxisSetting.TickLineSetting.Maximum; i += AxisSetting.TickLineSetting.Interval)
            {
                string labelText = string.Empty;
                try
                {
#pragma warning disable CA1305
                    labelText = (axis.LabelStyle != null && !string.IsNullOrEmpty(axis.LabelStyle.Format) && !axis.LabelStyle.Format.Contains("{value}", StringComparison.InvariantCulture)) ? Intl.GetNumericFormat(i, axis.LabelStyle.Format) : i.ToString();
                    labelText = (axis.LabelStyle != null && !string.IsNullOrEmpty(axis.LabelStyle.Format)) ? (axis.LabelStyle.Format.Contains("{value}", StringComparison.InvariantCulture) ?
                        axis.LabelStyle.Format.Replace("{value}", labelText, StringComparison.InvariantCulture) : labelText) :
                        (axis.RoundingPlaces != 0 ? i.ToString("N" + axis.RoundingPlaces) : labelText);
#pragma warning restore CA1305
                }
#pragma warning disable CA1031
                catch
#pragma warning restore CA1031
                {
#pragma warning disable CA1305
                    labelText = i.ToString();
#pragma warning restore CA1305
                }

                labelText = Parent.EnableGroupingSeparator ? labelText : labelText.Replace(",", string.Empty, StringComparison.InvariantCulture);
                AxisLabelRenderEventArgs args = new AxisLabelRenderEventArgs()
                {
                    Cancel = false,
                    Text = labelText,
                    Value = i,
                };
                await SfBaseUtils.InvokeEvent<AxisLabelRenderEventArgs>(Parent.CircularGaugeEvents?.AxisLabelRendering, args);
                LableSetting lableSetting = new LableSetting
                {
                    Text = args.Cancel ? labelText : args.Text,
                    Value = i,
                    TextSize = new SizeD
                    {
                        Width = 0,
                        Height = 0,
                    },
                };
                AxisSetting.LabelSettingCollection.Add(lableSetting);
            }

            double lastLabelValue = AxisSetting.LabelSettingCollection.Count > 0 ? AxisSetting.LabelSettingCollection[AxisSetting.LabelSettingCollection.Count - 1].Value : double.NaN;
            if (!double.IsNaN(lastLabelValue) && lastLabelValue != AxisSetting.TickLineSetting.Maximum && axis.ShowLastLabel)
            {
                LableSetting lableSetting = new LableSetting
                {
#pragma warning disable CA1305
                    Text = AxisSetting.TickLineSetting.Maximum.ToString(),
#pragma warning restore CA1305
                    Value = AxisSetting.TickLineSetting.Maximum,
                    TextSize = new SizeD
                    {
                        Width = 0,
                        Height = 0,
                    },
                };
                AxisSetting.LabelSettingCollection.Add(lableSetting);
            }

            GetMaxLabelWidth(axis);
        }

        private void DrawAxisLabels(CircularGaugeAxis axis)
        {
            double labelRadius = AxisSetting.CurrentRadius;
            Position labelPosition = axis.LabelStyle != null ? axis.LabelStyle.Position : Position.Inside;
            Position majorTicksPosition = axis.MajorTicks != null ? axis.MajorTicks.Position : Position.Inside;
            Position minorTicksPosition = axis.MinorTicks != null ? axis.MinorTicks.Position : Position.Inside;
            double opposedPadding = (labelPosition == Position.Inside && majorTicksPosition == Position.Outside &&
                minorTicksPosition == Position.Outside) || (labelPosition == Position.Outside &&
                minorTicksPosition == Position.Inside && majorTicksPosition == Position.Inside) ?
                (axis.LineStyle != null ? axis.LineStyle.Width : 2) + (AxisSetting.CurrentRadius / 20) :
                (labelPosition == majorTicksPosition ? AxisSetting.CurrentRadius / 20 : AxisSetting.CurrentRadius / 40);
            double labelPadding = axis.LabelStyle != null ? (axis.LabelStyle.ShouldMaintainPadding ? 10 : opposedPadding) : 10;
            if (axis.LabelStyle != null && labelPosition == Position.Outside)
            {
                labelRadius += AxisSetting.NearSize - (AxisSetting.MaxLabelSize.Height + (AxisSetting.StrokeWidth / 2));
            }
            else if (axis.LabelStyle != null && labelPosition == Position.Cross)
            {
                labelRadius = labelRadius - (AxisSetting.MaxLabelSize.Height / 4) - axis.LabelStyle.Offset;
            }
            else
            {
                labelRadius -= AxisSetting.FarSize - (AxisSetting.MaxLabelSize.Height + (AxisSetting.StrokeWidth / 2)) + (axis.LabelStyle != null && axis.LabelStyle.AutoAngle ? labelPadding : 0);
            }

            PointF lastLabelLocation = GetAxisLastLabelPosition(axis, labelRadius);
            PointF currentLocation = default(PointF);
            PointF previousLocation = default(PointF);
            double currentTextWidth = 0;
            double currentTextHeight = 0;
            double previousTextWidth = 0;
            double previousTextHeight = 0;
            HiddenLabel hiddenLabel = axis.LabelStyle != null ? axis.LabelStyle.HiddenLabel : HiddenLabel.None;
            bool isLabelAutoAngle = axis.LabelStyle != null ? axis.LabelStyle.AutoAngle : false;
            bool labelsVisible;
            bool labelFontAvailable = axis.LabelStyle != null && axis.LabelStyle.Font != null;
            string fontSize = labelFontAvailable && !string.IsNullOrEmpty(axis.LabelStyle.Font.Size) ? axis.LabelStyle.Font.Size : "12px";
            string fontWeight = labelFontAvailable && !string.IsNullOrEmpty(axis.LabelStyle.Font.FontWeight) ? axis.LabelStyle.Font.FontWeight : "normal";
            string fontStyle = labelFontAvailable && !string.IsNullOrEmpty(axis.LabelStyle.Font.FontStyle) ? axis.LabelStyle.Font.FontStyle : "normal";
            string fontFamily = labelFontAvailable && !string.IsNullOrEmpty(axis.LabelStyle.Font.FontFamily) ? axis.LabelStyle.Font.FontFamily : Parent.ThemeStyles.LabelFontFamily;
            AxisSetting.LabelFontStyle = "pointer-events: none; user-select:none; font-size:" + fontSize + "; font-style:" + fontStyle + "; font-weight:" + fontWeight + "; font-family:" + fontFamily;
            AxisSetting.LabelOpacity = labelFontAvailable ? axis.LabelStyle.Font.Opacity : 1;
            for (var i = 0; i < AxisSetting.LabelSettingCollection.Count; i++)
            {
                LableSetting currentLabel = AxisSetting.LabelSettingCollection[i];
                double labelAngle = Math.Round(GetAngleFromValue(
                    currentLabel.Value, AxisSetting.TickLineSetting.Maximum, AxisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise));
                PointF labelLocation = GetLocationFromAngle(labelAngle, labelRadius, AxisSetting.MidPoint);
                string labelAnchor = FindAnchor(labelLocation, labelAngle, currentLabel, axis);
                if (axis.LabelStyle != null && !axis.LabelStyle.AutoAngle)
                {
                    labelLocation = textLocation;
                }

                if (axis.HideIntersectingLabel)
                {
                    currentLocation = GetLocationFromAngle(labelAngle, labelRadius, AxisSetting.MidPoint);
                    currentTextWidth = AxisSetting.LabelSettingCollection[i].TextSize.Width;
                    currentTextHeight = !isLabelAutoAngle ? AxisSetting.LabelSettingCollection[i].TextSize.Height : currentTextWidth;
                    currentTextHeight = currentTextHeight - GetOffsetAxisLabelSize(labelAngle, currentTextHeight);
                    currentLocation = GetAxisLabelStartPosition(currentLocation, currentTextWidth, labelAnchor);
                    if (i == 0)
                    {
                        previousLocation = GetLocationFromAngle(labelAngle, labelRadius, AxisSetting.MidPoint);
                        previousTextWidth = AxisSetting.LabelSettingCollection[i].TextSize.Width;
                        previousTextHeight = !isLabelAutoAngle ? AxisSetting.LabelSettingCollection[i].TextSize.Height : previousTextWidth;
                        previousTextHeight = previousTextHeight - GetOffsetAxisLabelSize(labelAngle, previousTextHeight);
                        previousLocation = GetAxisLabelStartPosition(previousLocation, previousTextWidth, labelAnchor);
                    }
                }

                if ((i == 0 && hiddenLabel == HiddenLabel.First) || (i == (AxisSetting.LabelSettingCollection.Count - 1) && hiddenLabel == HiddenLabel.Last))
                {
                    continue;
                }

                if (axis.HideIntersectingLabel && (i != 0))
                {
                    bool showLastlabel = ((i != (AxisSetting.LabelSettingCollection.Count - 1)) && (IsCompleteAngle(axis.AngleStart, axis.AngleEnd) ||
                        axis.ShowLastLabel)) ? FindAxisLabelCollision(lastLabelLocation, AxisSetting.LastTextWidth, AxisSetting.LastTextHeight, currentLocation, currentTextWidth, currentTextHeight) : true;
                    labelsVisible = FindAxisLabelCollision(previousLocation, previousTextWidth, previousTextHeight, currentLocation, currentTextWidth, currentTextHeight) && showLastlabel;
                }
                else
                {
                    labelsVisible = true;
                }

                if (labelsVisible || i == AxisSetting.LabelSettingCollection.Count - 1)
                {
                    currentLabel.Text = (!axis.ShowLastLabel && ((IsCompleteAngle(axis.AngleStart, axis.AngleEnd) && hiddenLabel != HiddenLabel.First) || !labelsVisible)
                    && axis.HideIntersectingLabel && (i == (AxisSetting.LabelSettingCollection.Count - 1))) ? SPACE : currentLabel.Text;
                    currentLabel.Text = (axis.ShowLastLabel && axis.HideIntersectingLabel && IsCompleteAngle(axis.AngleStart, axis.AngleEnd)
                    && (i == 0)) ? SPACE : currentLabel.Text;
                    TextSetting textValues = new TextSetting
                    {
                        X = labelLocation.X,
                        Y = labelLocation.Y,
                        Anchor = labelAnchor,
                        Text = currentLabel.Text,
                        Transform = axis.LabelStyle != null && axis.LabelStyle.AutoAngle ? ("rotate(" + (labelAngle + 90).ToString(culture) + "," + labelLocation.X.ToString(culture) + ","
                            + labelLocation.Y.ToString(culture) + ")") : string.Empty,
                        Fill = SetLabelColor(axis, currentLabel.Value),
                    };
                    AxisSetting.TextSettingCollection.Add(textValues);
                    if (axis.HideIntersectingLabel)
                    {
                        previousTextWidth = currentLabel.TextSize.Width;
                        previousTextHeight = !isLabelAutoAngle ? currentLabel.TextSize.Height : previousTextWidth;
                        previousTextHeight = previousTextHeight - GetOffsetAxisLabelSize(labelAngle, previousTextHeight);
                        previousLocation = currentLocation;
                    }
                }
            }
        }

        private PointF GetAxisLastLabelPosition(CircularGaugeAxis axis, double radius)
        {
            PointF lastLabelLocation = default(PointF);
            if (axis.HideIntersectingLabel)
            {
                HiddenLabel hiddenLabel = axis.LabelStyle != null ? axis.LabelStyle.HiddenLabel : HiddenLabel.None;
                bool isLabelAutoAngle = axis.LabelStyle != null ? axis.LabelStyle.AutoAngle : false;
                double lastLabelAngle = Math.Round(GetAngleFromValue(AxisSetting.LabelSettingCollection[AxisSetting.LabelSettingCollection.Count - 1].Value, AxisSetting.TickLineSetting.Maximum, AxisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise));
                lastLabelLocation = GetLocationFromAngle(lastLabelAngle, radius, AxisSetting.MidPoint);
                string lastLabelAnchor = FindAnchor(lastLabelLocation, lastLabelAngle, AxisSetting.LabelSettingCollection[AxisSetting.LabelSettingCollection.Count - 1], axis);
                lastLabelLocation = textLocation;
                AxisSetting.LastTextWidth = (!axis.ShowLastLabel && IsCompleteAngle(axis.AngleStart, axis.AngleEnd) && (hiddenLabel != HiddenLabel.First)) ?
                    AxisSetting.LabelSettingCollection[0].TextSize.Width : AxisSetting.LabelSettingCollection[AxisSetting.LabelSettingCollection.Count - 1].TextSize.Width;
                AxisSetting.LastTextHeight = (!axis.ShowLastLabel && IsCompleteAngle(axis.AngleStart, axis.AngleEnd) && (hiddenLabel != HiddenLabel.First)) ?
                    (!isLabelAutoAngle ? AxisSetting.LabelSettingCollection[0].TextSize.Height : AxisSetting.LabelSettingCollection[0].TextSize.Width) :
                    (!isLabelAutoAngle ? AxisSetting.LabelSettingCollection[AxisSetting.LabelSettingCollection.Count - 1].TextSize.Height :
                        AxisSetting.LabelSettingCollection[AxisSetting.LabelSettingCollection.Count - 1].TextSize.Width);
                AxisSetting.LastTextHeight = AxisSetting.LastTextHeight - GetOffsetAxisLabelSize(lastLabelAngle, AxisSetting.LastTextHeight);
                lastLabelLocation = GetAxisLabelStartPosition(lastLabelLocation, AxisSetting.LastTextWidth, lastLabelAnchor);
            }

            return lastLabelLocation;
        }

        private string FindAnchor(PointF labelLocation, double angle, LableSetting label, CircularGaugeAxis axis)
        {
            if (axis.LabelStyle != null && axis.LabelStyle.AutoAngle)
            {
                return "middle";
            }

            double height = label != null ? label.TextSize.Height : 0;
            string labelAnchor = axis.LabelStyle == null || axis.LabelStyle.Position == Position.Inside ?
                ((angle > 120 && angle < 240) ? "start" : ((angle > 300 || angle < 60) ? "end" : "middle")) :
                ((angle > 120 && angle < 240) ? "end" : ((angle > 300 || angle < 60) ? "start" : "middle"));
            double labelLocationY = axis.LabelStyle == null || axis.LabelStyle.Position == Position.Inside ?
                ((angle >= 240 && angle <= 300) ? (height / 2) :
                    (angle >= 60 && angle <= 120) ? 0 : height / 4) :
                ((angle >= 240 && angle <= 300) ? 0 :
                    (angle >= 60 && angle <= 120) ? height / 2 : height / 4);
            textLocation = new PointF(labelLocation.X, (float)(labelLocation.Y + labelLocationY));
            return labelAnchor;
        }

        private void MeasureAxisSize(int index)
        {
            double sum = CalculateSum(index, farSizes);
            AxisSetting.RectValue = new Rect
            {
                X = AxisSetting.RectValue.X + sum,
                Y = AxisSetting.RectValue.Y + sum,
                Width = AxisSetting.RectValue.Width - (sum * 2),
                Height = AxisSetting.RectValue.Height - (sum * 2),
            };
        }

        private async Task CalculateAxisValues(CircularGaugeAxis axis)
        {
            CalculateVisibleRange(axis);
            await CalculateVisibleLabels(axis);
        }

        private async Task ComputeSize(CircularGaugeAxis axis)
        {
            double axisPadding = 5;
            await CalculateAxisValues(axis);
            double lineSize = axis.LineStyle != null ? axis.LineStyle.Width / 2 : 1;
            double outerHeight = 0;
            double innerHeight = 0;
            double heightForCross = 0;
            Position majorTickPosition = axis.MajorTicks != null ? axis.MajorTicks.Position : Position.Inside;
            Position minorTickPosition = axis.MinorTicks != null ? axis.MinorTicks.Position : Position.Inside;
            Position labelPosition = axis.LabelStyle != null ? axis.LabelStyle.Position : Position.Inside;
            double majorTickHeight = axis.MajorTicks != null ? axis.MajorTicks.Height : 10;
            double minorTickHeight = axis.MinorTicks != null ? axis.MinorTicks.Height : 5;
            heightForCross = majorTickPosition == Position.Cross ?
            majorTickHeight / 2 : heightForCross;
            heightForCross = (majorTickPosition == Position.Cross && heightForCross <
                minorTickHeight / 2) ? minorTickHeight / 2 : heightForCross;
            heightForCross = (majorTickPosition == Position.Cross && heightForCross <
                 AxisSetting.MaxLabelSize.Height / 2) ? AxisSetting.MaxLabelSize.Height / 2 : heightForCross;
            lineSize = lineSize < heightForCross ? heightForCross : lineSize;
            double majorTickOffset = axis.MajorTicks != null ? axis.MajorTicks.Offset : 0;
            double minorTickOffset = axis.MinorTicks != null ? axis.MinorTicks.Offset : 0;
            double labelOffset = axis.LabelStyle != null ? axis.LabelStyle.Offset : 0;
            double labelPadding = axis.LabelStyle != null ? (axis.LabelStyle.ShouldMaintainPadding ? 10 : 0) : 10;
            outerHeight += !(majorTickPosition == Position.Outside && minorTickPosition == Position.Outside &&
                labelPosition == Position.Outside) ? axisPadding : 0;
            outerHeight += (majorTickPosition == Position.Outside ? (majorTickHeight + lineSize) : 0) +
                (labelPosition == Position.Outside ? (AxisSetting.MaxLabelSize.Height + labelOffset + labelPadding) : 0) +
                ((minorTickPosition == Position.Outside && !(majorTickPosition == Position.Outside)) ?
                    (minorTickHeight + lineSize) : 0) + lineSize;
            outerHeight += (majorTickPosition == Position.Outside && minorTickPosition == Position.Outside) ?
                Math.Max(majorTickOffset, minorTickOffset) : (majorTickPosition == Position.Outside ?
                     majorTickOffset : minorTickPosition == Position.Outside ? minorTickOffset : 0);
            innerHeight += ((majorTickPosition == Position.Inside) ? (majorTickHeight + lineSize) : 0) +
                ((labelPosition == Position.Inside) ? (AxisSetting.MaxLabelSize.Height + labelOffset + labelPadding) : 0) +
                ((minorTickPosition == Position.Inside && majorTickPosition == Position.Outside) ?
                    (minorTickHeight + lineSize) : 0) + lineSize;
            innerHeight += ((majorTickPosition == Position.Inside) && (minorTickPosition == Position.Inside)) ?
                Math.Max(majorTickOffset, minorTickOffset) : ((majorTickPosition == Position.Inside) ?
                     majorTickOffset : (minorTickPosition == Position.Inside) ? minorTickOffset : 0);
            if (farSizes.Count > 0)
            {
                farSizes[farSizes.Count - 1] += innerHeight + outerHeight;
            }

            AxisSetting.NearSize = outerHeight - axisPadding;
            AxisSetting.FarSize = innerHeight;
            outerHeight = (Parent.Axes.Count == (farSizes.Count + 1)) ? 0 : outerHeight;
            farSizes.Add(outerHeight);
        }

        private void GetMaxLabelWidth(CircularGaugeAxis axis)
        {
            AxisSetting.MaxLabelSize = new SizeD
            {
                Width = AxisSetting.MaxLabelSize != null ?
                AxisSetting.MaxLabelSize.Width : 0,
                Height = AxisSetting.MaxLabelSize != null ?
                AxisSetting.MaxLabelSize.Height : 0,
            };
            for (int i = 0; i < AxisSetting.LabelSettingCollection.Count; i++)
            {
                double labelFontSize = axis.LabelStyle != null && axis.LabelStyle.Font != null && !string.IsNullOrEmpty(axis.LabelStyle.Font.Size) ?
                   (axis.LabelStyle.Font.Size.IndexOf("px", StringComparison.InvariantCulture) > 0 ?
                   float.Parse(axis.LabelStyle.Font.Size.Replace("px", string.Empty, StringComparison.InvariantCulture), culture) : float.Parse(axis.LabelStyle.Font.Size, culture)) : 12;
                AxisSetting.LabelSettingCollection[i].TextSize = MeasureText(AxisSetting.LabelSettingCollection[i].Text, labelFontSize);
                AxisSetting.MaxLabelSize.Width = AxisSetting.LabelSettingCollection[i].TextSize.Width > AxisSetting.MaxLabelSize.Width ?
                    AxisSetting.LabelSettingCollection[i].TextSize.Width : AxisSetting.MaxLabelSize.Width;
                AxisSetting.MaxLabelSize.Height = AxisSetting.LabelSettingCollection[i].TextSize.Height > AxisSetting.MaxLabelSize.Height ?
                    AxisSetting.LabelSettingCollection[i].TextSize.Height : AxisSetting.MaxLabelSize.Height;
            }
        }
    }
}

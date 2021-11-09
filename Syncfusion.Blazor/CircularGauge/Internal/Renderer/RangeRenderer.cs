using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Specifies the properties and methods to render the ranges in Circular Gauge.
    /// </summary>
    internal class RangeRenderer
    {
        private const string SPACE = " ";
        private double rangeCalculatedValue;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeRenderer"/> class.
        /// </summary>
        /// <param name="parent">represent the properties of the range.</param>
        internal RangeRenderer(SfCircularGauge parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets or sets the properties of the circular gauge.
        /// </summary>
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties to render multiple ranges.
        /// </summary>
        internal List<Range> RangeCollection { get; set; } = new List<Range>();

        /// <summary>
        /// Gets or sets the properties of the range.
        /// </summary>
        internal Range RangeSetting { get; set; } = new Range();

        /// <summary>
        /// This method is used to set the range value dynamically in the circular gauge.
        /// </summary>
        /// <param name="axis">Specifies the properties of the axis in which the range value is to changed..</param>
        /// <param name="range">Specifies the properties of the range in which the range value is to changed..</param>
        /// <param name="start">Specifies the properties of the range in which the range start value is to changed.</param>
        /// <param name="end">Specifies the properties of the axis in which the range end value is to changed.</param>
        /// <param name="parent">Specifies the properties of the ciruclar gauge.</param>
        internal void SetRangeValue(CircularGaugeAxis axis, CircularGaugeRange range, double start, double end, Range parent)
        {
            bool isClockWise = axis.Direction == GaugeDirection.ClockWise;
            double startValue = Math.Min(Math.Max(start, axis.Minimum), end);
            double endValue = Math.Min(Math.Max(start, end), axis.Maximum);
            double startAngle = AxisRenderer.GetAngleFromValue(startValue, axis.Maximum, axis.Minimum, axis.AngleStart, axis.AngleEnd, isClockWise);
            double endAngle = AxisRenderer.GetAngleFromValue(endValue, axis.Maximum, axis.Minimum, axis.AngleStart, axis.AngleEnd, isClockWise);
            if (!string.IsNullOrEmpty(range.StartWidth) && float.Parse(range.StartWidth, culture) > 0)
            {
                RangeSetting.RangeStartWidth = ConvertToPixel(range.StartWidth, RangeSetting.RangeCurrentRadius);
            }

            if (!string.IsNullOrEmpty(range.EndWidth) && float.Parse(range.EndWidth, culture) > 0)
            {
                RangeSetting.RangeEndWidth = ConvertToPixel(range.EndWidth, RangeSetting.RangeCurrentRadius);
            }

            string rangeRadius = !string.IsNullOrEmpty(range.Radius) ? range.Radius : "100%";
            RangeSetting.RangeCurrentRadius = SfCircularGauge.StringToNumber(
                rangeRadius, parent.RangeCurrentRadius);
            endAngle = axis.Direction == GaugeDirection.ClockWise ? endAngle : 0;
            RangeSetting.RangeEndWidth = axis.Direction == GaugeDirection.ClockWise ? RangeSetting.RangeEndWidth : 0;
            if (!double.IsNaN(range.RoundedCornerRadius) && range.RoundedCornerRadius > 0)
            {
                double roundedValue = range.RoundedCornerRadius * 0.25;
                double previousRangeStart = (((RangeSetting.RangeCurrentRadius - (RangeSetting.RangeStartWidth / 2)) * (startAngle * Math.PI / 180)) -
                    (range.RoundedCornerRadius / roundedValue)) / (RangeSetting.RangeCurrentRadius - (RangeSetting.RangeStartWidth / 2)) * 180 / Math.PI;
                double previousRangeEnd = (((RangeSetting.RangeCurrentRadius - (RangeSetting.RangeEndWidth / 2)) * (endAngle * Math.PI / 180)) +
                    (range.RoundedCornerRadius / roundedValue)) / (RangeSetting.RangeCurrentRadius - (RangeSetting.RangeEndWidth / 2)) * 180 / Math.PI;
                double roundedStartAngle = ((RangeSetting.RangeCurrentRadius * (startAngle * Math.PI / 180)) +
                    range.RoundedCornerRadius) / RangeSetting.RangeCurrentRadius * 180 / Math.PI;
                double roundedEndAngle = ((RangeSetting.RangeCurrentRadius * (endAngle * Math.PI / 180)) -
                    range.RoundedCornerRadius) / RangeSetting.RangeCurrentRadius * 180 / Math.PI;
                RangeSetting.RangeRoundedPath = GetRoundedPathArc(Parent.AxisRenderer.AxisSetting.MidPoint, Math.Floor(roundedStartAngle), Math.Ceiling(roundedStartAngle), previousRangeStart, previousRangeEnd, RangeSetting.RangeCurrentRadius, RangeSetting.RangeStartWidth, RangeSetting.RangeEndWidth);
            }
            else
            {
                RangeSetting.RangePath = Parent.AxisRenderer.GetPathArc(Parent.AxisRenderer.AxisSetting.MidPoint, Math.Round(startAngle), Math.Round(endAngle), RangeSetting.RangeCurrentRadius, RangeSetting.RangeStartWidth, RangeSetting.RangeEndWidth, axis, range);
            }
        }

        /// <summary>
        /// This method is used to render the range element in the axis.
        /// </summary>
        /// <param name="axis">Specifies the properties of the axis.</param>
        internal void DrawAxisRange(CircularGaugeAxis axis)
        {
            if (axis.Ranges != null)
            {
                for (int i = 0; i < axis.Ranges.Count; i++)
                {
                    RangeSetting = new Range();
                    bool isFirstRange = i == 0;
                    bool isLastRange = i == axis.Ranges.Count - 1;
                    DrawRange(axis, axis.Ranges[i], RangeSetting, isFirstRange, isLastRange, i);
                    string gradientColor = string.Empty;
                    if (axis.Ranges[i].LinearGradient != null)
                    {
                        gradientColor = GetLinearGradientColor(axis.Ranges[i], i);
                    }

                    if (axis.Ranges[i].RadialGradient != null && string.IsNullOrEmpty(gradientColor))
                    {
                        gradientColor = GetRadialGradientColor(axis.Ranges[i], i);
                    }

                    RangeSetting.RangeFillColor = !string.IsNullOrEmpty(gradientColor) ? gradientColor : (!string.IsNullOrEmpty(axis.Ranges[i].Color) ?
                        axis.Ranges[i].Color : Parent.AxisRenderer.RangeColors[i % Parent.AxisRenderer.RangeColors.Length]);
                    RangeSetting.Opacity = !double.IsNaN(axis.Ranges[i].Opacity) ? axis.Ranges[i].Opacity : 1;
                    RangeCollection.Add(RangeSetting);
                }
            }
        }

        /// <summary>
        /// This method creates the path of the range element.
        /// </summary>
        /// <param name="start">Specifies the start value of the range.</param>
        /// <param name="end">Specifies the end value of the range.</param>
        /// <param name="innerStart">Specifies the inner start value of the range.</param>
        /// <param name="innerEnd">Specifies the inner end value of the range.</param>
        /// <param name="radius">Specifies the radius of the range.</param>
        /// <param name="startRadius">Specifies the start radius of the range.</param>
        /// <param name="endRadius">Specifies the end radius of the range.</param>
        /// <param name="clockWise">Specifies the direction of the range.</param>
        /// <returns>return the value as string.</returns>
        internal string GetRangePath(PointF start, PointF end, PointF innerStart, PointF innerEnd, double radius, double startRadius, double endRadius, int clockWise)
        {
            return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " A " + radius.ToString(culture) + SPACE + radius.ToString(culture) + " 0 " + clockWise.ToString(culture) + " 1 " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture) +
                " L " + innerEnd.X.ToString(culture) + SPACE + innerEnd.Y.ToString(culture) + " A " + endRadius.ToString(culture) + SPACE + startRadius.ToString(culture) + " 0 " + clockWise.ToString(culture) + " 0 " + innerStart.X.ToString(culture) + SPACE + innerStart.Y.ToString(culture) + " Z";
        }

        /// <summary>
        /// This method is invoked when the range element is dragged.
        /// </summary>
        /// <param name="x">Specifies the x position of the drag event.</param>
        /// <param name="y">Specifies the y position of the drag event.</param>
        /// <param name="rangeIndex">Specifies the range index of the range.</param>
        /// <param name="axisIndex">Specifies the axis index of the range.</param>
        /// <param name="startAngle">Specifies the start angle of the range.</param>
        /// <param name="endAngle">Specifies the end angle of the range.</param>
        /// <param name="direction">Specifies the direction of the range.</param>
        /// <param name="maximum">Specifies the maximum value of the axis.</param>
        /// <param name="minimum">Specifies the minimum value of the axis.</param>
        /// <param name="axis">Specifies the properties of the axis.</param>
        internal void RangeDrag(double x, double y, int rangeIndex, int axisIndex, double startAngle, double endAngle, GaugeDirection direction, double maximum, double minimum, CircularGaugeAxis axis)
        {
            PointF point = new PointF((float)x, (float)y);
            double angle = AxisRenderer.GetAngleFromLocation(Parent.AxisRenderer.AxisSetting.MidPoint, point);
            double pointerValue = AxisRenderer.GetValueFromAngle(angle, maximum, minimum, startAngle, endAngle, direction == GaugeDirection.ClockWise);
            rangeCalculatedValue = rangeCalculatedValue != 0 ? rangeCalculatedValue : axis.AxisValues.RangeStart[rangeIndex];
            if (pointerValue < maximum && pointerValue > minimum)
            {
                double add = axis.AxisValues.RangeEnd[rangeIndex] - axis.AxisValues.RangeStart[rangeIndex];
                double div = add / 2;
                double avg = axis.AxisValues.RangeStart[rangeIndex] + div;
                axis.AxisValues.RangeStart[rangeIndex] = (pointerValue < avg) ? pointerValue : (rangeCalculatedValue < avg ? rangeCalculatedValue : axis.AxisValues.RangeStart[rangeIndex]);
                axis.AxisValues.RangeEnd[rangeIndex] = (pointerValue < avg) ? ((rangeCalculatedValue > avg) ? rangeCalculatedValue : axis.AxisValues.RangeEnd[rangeIndex]) : pointerValue;
                rangeCalculatedValue = pointerValue;
                bool isFirstRange = rangeIndex == 0;
                bool isLastRange = rangeIndex == axis.Ranges.Count - 1;
                Axis axisSetting = Parent.AxisRenderer.AxisCollection[axisIndex];
                DrawRange(axis, axis.Ranges[rangeIndex], axisSetting.RangeCollection[rangeIndex], isFirstRange, isLastRange, rangeIndex);
                int count = 0;
                for (double i = axisSetting.TickLineSetting.Minimum; i <= axisSetting.TickLineSetting.Maximum; i += axisSetting.TickLineSetting.Interval)
                {
                    axisSetting.MajorTickLineStroke[count] = Parent.AxisRenderer.SetMajorTickColor(axis, i);
                    count++;
                }

                double minorInterval = axis.MinorTicks != null ? axis.MinorTicks.Interval == 0 ? axisSetting.TickLineSetting.Interval / 2 :
                 axis.MinorTicks.Interval : axisSetting.TickLineSetting.Interval / 2;
                int minorCount = 0;
                for (double i = axisSetting.TickLineSetting.Minimum; i <= axisSetting.TickLineSetting.Maximum; i += minorInterval)
                {
                    if (axisSetting.MajorTickValues.IndexOf(i) < 0)
                    {
                        axisSetting.MinorTickLineStroke[minorCount] = Parent.AxisRenderer.SetMinorTickColor(axis, i);
                        minorCount++;
                    }
                }

                for (int j = 0; j < axisSetting.LabelSettingCollection.Count; j++)
                {
                    axisSetting.TextSettingCollection[j].Fill = Parent.AxisRenderer.SetLabelColor(axis, axisSetting.LabelSettingCollection[j].Value);
                }
            }
        }

        /// <summary>
        /// This method is used to create the linear gradient color for the range element.
        /// </summary>
        /// <param name="range">Specifies the properties of the range element.</param>
        /// <param name="index">Specifies the index number of the range element.</param>
        /// <returns>returns the value as string.</returns>
        internal string GetLinearGradientColor(CircularGaugeRange range, int index)
        {
            RangeSetting.StartValue = !string.IsNullOrEmpty(range.LinearGradient.StartValue) ? (range.LinearGradient.StartValue.IndexOf("%", StringComparison.InvariantCulture) != -1 ?
                range.LinearGradient.StartValue : range.LinearGradient.StartValue + "%") : string.Empty;
            RangeSetting.EndValue = !string.IsNullOrEmpty(range.LinearGradient.EndValue) ? (range.LinearGradient.EndValue.IndexOf("%", StringComparison.InvariantCulture) != -1 ?
                range.LinearGradient.EndValue : range.LinearGradient.EndValue + "%") : string.Empty;
            RangeSetting.LinearColorString = !string.IsNullOrEmpty(RangeSetting.StartValue) || !string.IsNullOrEmpty(RangeSetting.EndValue) ? Parent.ID + "_LinearGradient_range" + index :
                string.Empty;
            return !string.IsNullOrEmpty(RangeSetting.LinearColorString) ? "url(#" + RangeSetting.LinearColorString + ")" : string.Empty;
        }

        /// <summary>
        /// This method is used to create the radial gradient color for the range element.
        /// </summary>
        /// <param name="range">Specifies the properties of the range element.</param>
        /// <param name="index">Specifies the index number of the range element.</param>
        /// /// <returns>returns the value as string.</returns>
        internal string GetRadialGradientColor(CircularGaugeRange range, int index)
        {
            RadialGradient radialGradient = range.RadialGradient;
            RangeSetting.Radius = !string.IsNullOrEmpty(RangeSetting.Radius) ? ProcessGradientString(radialGradient.Radius) : string.Empty;
            RangeSetting.OuterPositionX = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.X) ?
                ProcessGradientString(radialGradient.OuterPosition.X) : string.Empty;
            RangeSetting.OuterPositionY = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.Y) ?
                ProcessGradientString(radialGradient.OuterPosition.Y) : string.Empty;
            RangeSetting.InnerPositionX = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.X) ?
                ProcessGradientString(radialGradient.InnerPosition.X) : string.Empty;
            RangeSetting.InnerPositionY = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.Y) ?
                ProcessGradientString(radialGradient.InnerPosition.Y) : string.Empty;
            RangeSetting.RadialColorString = !string.IsNullOrEmpty(RangeSetting.Radius) || !string.IsNullOrEmpty(RangeSetting.OuterPositionX) || !string.IsNullOrEmpty(RangeSetting.InnerPositionX) || !string.IsNullOrEmpty(RangeSetting.InnerPositionY)
                ? Parent.ID + "_RadialGradient_range_" + index : string.Empty;
            return !string.IsNullOrEmpty(RangeSetting.RadialColorString) ? "url(#" + RangeSetting.RadialColorString + ")" : string.Empty;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal void Dispose()
        {
            Parent = null;
            RangeCollection = null;
            RangeSetting = null;
        }

        private static double ConvertToPixel(string pointValue, double maximumDimension)
        {
            if (!string.IsNullOrEmpty(pointValue))
            {
                return pointValue.Contains('%', StringComparison.InvariantCulture) ? (maximumDimension / 100) * int.Parse(pointValue.Replace("%", string.Empty, StringComparison.InvariantCulture), CultureInfo.InvariantCulture) : int.Parse(pointValue, CultureInfo.InvariantCulture);
            }

            return double.NaN;
        }

        private static string ProcessGradientString(string gradientValue)
        {
            return gradientValue.IndexOf("%", StringComparison.InvariantCulture) != -1 ? gradientValue : gradientValue + "%";
        }

        private void DrawRange(CircularGaugeAxis axis, CircularGaugeRange range, Range rangeSetting, bool isFirstRange, bool isLastRange, int index)
        {
            Axis axisSetting = Parent.AxisRenderer.AxisSetting;
            AxisInternal axisValues = axis.AxisValues;
            rangeSetting.RangeCurrentDistanceFromScale = !string.IsNullOrEmpty(range.Offset) ? SfCircularGauge.StringToNumber(range.Offset, axisSetting.CurrentRadius) : 0;
            CalculateRangeRadius(range, rangeSetting);
            bool isNumber = double.TryParse(range.StartWidth, out double startWidthValue);
            rangeSetting.RangeStartWidth = !string.IsNullOrEmpty(range.StartWidth) ? (range.StartWidth.IndexOf("%", StringComparison.InvariantCulture) > 0 ?
                ConvertToPixel(range.StartWidth, rangeSetting.RangeCurrentRadius) : (isNumber ? startWidthValue : 10)) : 10;
            isNumber = double.TryParse(range.EndWidth, out double endWidthValue);
            rangeSetting.RangeEndWidth = !string.IsNullOrEmpty(range.EndWidth) ? (range.EndWidth.IndexOf("%", StringComparison.InvariantCulture) > 0 ?
                ConvertToPixel(range.EndWidth, rangeSetting.RangeCurrentRadius) : (isNumber ? endWidthValue : 10)) : 10;
            rangeSetting.RangeCurrentRadius = CalculateRangeRadiusWithPosition(axis, range, rangeSetting.RangeStartWidth);
            TickLine tickLine = axisSetting.TickLineSetting;
            double startValue = Math.Min(Math.Max(axisValues.RangeStart[index], tickLine.Minimum), axis.AxisValues.RangeEnd[index]);
            double endValue = Math.Min(Math.Max(axisValues.RangeStart[index], axisValues.RangeEnd[index]), tickLine.Maximum);
            double startAngle = AxisRenderer.GetAngleFromValue(startValue, tickLine.Maximum, tickLine.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise);
            double endAngle = AxisRenderer.GetAngleFromValue(endValue, tickLine.Maximum, tickLine.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise);
            bool isAngleCross360 = startAngle > endAngle;
            if (!double.IsNaN(axis.RangeGap) && axis.RangeGap > 0)
            {
                startAngle = (isFirstRange && !axis.StartAndEndRangeGap) ? startAngle : startAngle + (axis.RangeGap / Math.PI);
                endAngle = (isLastRange && !axis.StartAndEndRangeGap) ? endAngle : endAngle - (axis.RangeGap / Math.PI);
            }

            if ((startValue != endValue) && (isAngleCross360 ? startAngle < (endAngle + 360) : (startAngle < endAngle)))
            {
                double tempValue = endAngle;
                endAngle = axis.Direction == GaugeDirection.ClockWise ? endAngle : startAngle;
                startAngle = axis.Direction == GaugeDirection.ClockWise ? startAngle : tempValue;
                tempValue = rangeSetting.RangeEndWidth;
                rangeSetting.RangeEndWidth = axis.Direction == GaugeDirection.ClockWise ? rangeSetting.RangeEndWidth : rangeSetting.RangeStartWidth;
                rangeSetting.RangeStartWidth = axis.Direction == GaugeDirection.ClockWise ? rangeSetting.RangeStartWidth : tempValue;
                double roundedValue = range.RoundedCornerRadius * 0.25;
                double previousRangeStart = (((rangeSetting.RangeCurrentRadius - (rangeSetting.RangeStartWidth / 2)) * ((startAngle * Math.PI) / 180)) -
                    (range.RoundedCornerRadius / roundedValue)) / (rangeSetting.RangeCurrentRadius - (rangeSetting.RangeStartWidth / 2)) * 180 / Math.PI;
                double previousRangeEnd = (((rangeSetting.RangeCurrentRadius - (rangeSetting.RangeEndWidth / 2)) * (endAngle * Math.PI / 180)) +
                    (range.RoundedCornerRadius / roundedValue)) / (rangeSetting.RangeCurrentRadius - (rangeSetting.RangeEndWidth / 2)) * 180 / Math.PI;
                double roundedStartAngle = ((((rangeSetting.RangeCurrentRadius * ((startAngle * Math.PI) / 180)) +
                    range.RoundedCornerRadius) / rangeSetting.RangeCurrentRadius) * 180) / Math.PI;
                double roundedEndAngle = ((rangeSetting.RangeCurrentRadius * (endAngle * Math.PI / 180)) -
                    range.RoundedCornerRadius) / rangeSetting.RangeCurrentRadius * 180 / Math.PI;

                if (!double.IsNaN(range.RoundedCornerRadius) && range.RoundedCornerRadius > 0)
                {
                    rangeSetting.RangeRoundedPath = GetRoundedPathArc(axisSetting.MidPoint, Math.Floor(roundedStartAngle), Math.Ceiling(roundedEndAngle), previousRangeStart, previousRangeEnd, rangeSetting.RangeCurrentRadius, rangeSetting.RangeStartWidth, rangeSetting.RangeEndWidth);
                }
                else
                {
                    rangeSetting.RangePath = Parent.AxisRenderer.GetPathArc(axisSetting.MidPoint, Math.Floor(startAngle), Math.Ceiling(endAngle), rangeSetting.RangeCurrentRadius, rangeSetting.RangeStartWidth, rangeSetting.RangeEndWidth, axis, range);
                }
            }
        }

        private void CalculateRangeRadius(CircularGaugeRange range, Range rangeSetting)
        {
            string radius = !string.IsNullOrEmpty(range.Radius) ? range.Radius : "100%";
            rangeSetting.RangeCurrentRadius = SfCircularGauge.StringToNumber(radius, Parent.AxisRenderer.AxisSetting.CurrentRadius);
        }

        private string GetRoundedPathArc(PointF center, double actualStart, double actualEnd, double oldStart, double oldEnd, double radius, double startWidth, double endWidth)
        {
            actualEnd -= AxisRenderer.IsCompleteAngle(actualStart, actualEnd) ? 0.0001 : 0;
            double rangeRoundDegree = AxisRenderer.GetDegreeValue(actualStart, actualEnd);
            double roundStartRadius = radius - startWidth;
            double roundEndRadius = radius - endWidth;
            double roundArcRadius = radius - ((startWidth + endWidth) / 2);
            return GetRoundedPath(AxisRenderer.GetLocationFromAngle(actualStart, radius, center), AxisRenderer.GetLocationFromAngle(actualEnd, radius, center), AxisRenderer.GetLocationFromAngle(oldEnd, radius, center), AxisRenderer.GetLocationFromAngle(oldEnd, roundEndRadius, center), AxisRenderer.GetLocationFromAngle(oldStart, radius, center), AxisRenderer.GetLocationFromAngle(oldStart, roundStartRadius, center), AxisRenderer.GetLocationFromAngle(actualStart, roundStartRadius, center), AxisRenderer.GetLocationFromAngle(actualEnd, roundEndRadius, center), radius, roundArcRadius, roundArcRadius, (rangeRoundDegree < 180) ? 0 : 1);
        }

        private string GetRoundedPath(PointF start, PointF end, PointF outerOldEnd, PointF innerOldEnd, PointF outerOldStart, PointF innerOldStart, PointF innerStart, PointF innerEnd, double radius, double startRadius, double endRadius, double clockWise)
        {
            return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " A " + radius.ToString(culture) + SPACE + radius.ToString(culture) + " 0 " + clockWise.ToString(culture) + " 1 " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture) +
                " C " + outerOldEnd.X.ToString(culture) + SPACE + outerOldEnd.Y.ToString(culture) + SPACE + innerOldEnd.X.ToString(culture) + SPACE + innerOldEnd.Y.ToString(culture) + SPACE + innerEnd.X.ToString(culture) + SPACE + innerEnd.Y.ToString(culture) +
                " A " + endRadius.ToString(culture) + SPACE + startRadius.ToString(culture) + " 0 " + clockWise.ToString(culture) + " 0 " + innerStart.X.ToString(culture) + SPACE + innerStart.Y.ToString(culture) +
                " C " + innerOldStart.X.ToString(culture) + SPACE + innerOldStart.Y.ToString(culture) + SPACE + outerOldStart.X.ToString(culture) + SPACE + outerOldStart.Y.ToString(culture) + SPACE + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " Z";
        }

        private double CalculateRangeRadiusWithPosition(CircularGaugeAxis axis, CircularGaugeRange range, double startWidth)
        {
            double lineWidth = axis.LineStyle != null ? axis.LineStyle.Width : 2;
            return range.Position != PointerRangePosition.Auto && string.IsNullOrEmpty(range.Radius) ?
                (range.Position == PointerRangePosition.Outside ? (RangeSetting.RangeCurrentRadius + (lineWidth / 2) + RangeSetting.RangeCurrentDistanceFromScale) :
                (range.Position == PointerRangePosition.Inside ? (RangeSetting.RangeCurrentRadius - (lineWidth / 2) - RangeSetting.RangeCurrentDistanceFromScale) :
                (RangeSetting.RangeCurrentRadius + (startWidth / 2) - RangeSetting.RangeCurrentDistanceFromScale))) : RangeSetting.RangeCurrentRadius;
        }
    }
}

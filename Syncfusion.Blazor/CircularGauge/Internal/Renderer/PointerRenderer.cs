using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Specifies the properties and methods to render pointer element.
    /// </summary>
    internal class PointerRenderer
    {
        private const string SPACE = " ";
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerRenderer"/> class.
        /// </summary>
        /// <param name="parent">represent the properties of the axis.</param>
        internal PointerRenderer(SfCircularGauge parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets or sets the properties of the circular gauge.
        /// </summary>
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties to render the pointers.
        /// </summary>
        internal List<PointerSetting> PointerCollection { get; set; } = new List<PointerSetting>();

        /// <summary>
        /// Gets or sets the properties to render the pointer.
        /// </summary>
        internal PointerSetting PointerSetting { get; set; } = new PointerSetting();

        /// <summary>
        /// Gets or sets the properties to animate the pointer.
        /// </summary>
        internal List<AnimationOptions> PointerAnimate { get; set; } = new List<AnimationOptions>();

        /// <summary>
        /// Gets or sets the properties to animate the range.
        /// </summary>
        internal List<AnimationOptions> RangeAnimate { get; set; } = new List<AnimationOptions>();

        /// <summary>
        /// This method is used to set the pointer value dynamically in the Circular Gauge.
        /// </summary>
        /// <param name="axis">Specifies the properties of the axis.</param>
        /// <param name="pointer">Specifies the properties of the pointer.</param>
        /// <param name="currentValue">Specifies the current value of the pointer.</param>
        /// <param name="pointerSetting">Specifies the properties of the pointer settings.</param>
        /// <param name="axisSetting">Specifies the properties of axis.</param>
        internal void SetPointerValue(CircularGaugeAxis axis, CircularGaugePointer pointer, double currentValue, PointerSetting pointerSetting, Axis axisSetting = null)
        {
            if (axisSetting == null)
            {
                axisSetting = Parent.AxisRenderer.AxisSetting;
            }

            if (currentValue < axisSetting.TickLineSetting.Minimum)
            {
                currentValue = axisSetting.TickLineSetting.Minimum;
            }

            bool isRangeBarRender = currentValue == axisSetting.TickLineSetting.Minimum && pointer.Type == PointerType.RangeBar;
            pointerSetting.CurrentValue = currentValue;
            double startAngle = AxisRenderer.GetAngleFromValue(axisSetting.TickLineSetting.Minimum, axisSetting.TickLineSetting.Maximum, axisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise);
            double endAngle = AxisRenderer.GetAngleFromValue(currentValue, axisSetting.TickLineSetting.Maximum, axisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise);
            if (axis.Direction == GaugeDirection.ClockWise)
            {
                endAngle = startAngle == endAngle && !isRangeBarRender ? endAngle + 1 : endAngle;
            }
            else
            {
                double tempAngle = endAngle;
                endAngle = startAngle == endAngle && !isRangeBarRender ? startAngle : startAngle;
                startAngle = startAngle == endAngle && !isRangeBarRender ? tempAngle - 1 : tempAngle;
            }

            double roundedRadius = Math.Min(Math.Min(pointer.PointerWidth, pointer.RoundedCornerRadius), pointer.Value) / 2;
            double minimumRadius = roundedRadius * 0.25;
            if (currentValue <= minimumRadius)
            {
                roundedRadius = (currentValue < 6) ? 8 : roundedRadius;
                roundedRadius /= 2;
                minimumRadius = roundedRadius * 0.25;
            }

            double previousStartValue = (((pointerSetting.CurrentRadius - (pointer.PointerWidth / 2)) * (startAngle * Math.PI / 180)) -
                (roundedRadius / minimumRadius)) / (pointerSetting.CurrentRadius - (pointer.PointerWidth / 2)) * 180 / Math.PI;
            double previousEndValue = (((pointerSetting.CurrentRadius - (pointer.PointerWidth / 2)) * ((endAngle * Math.PI) / 180)) +
                (roundedRadius / minimumRadius)) / (pointerSetting.CurrentRadius - (pointer.PointerWidth / 2)) * 180 / Math.PI;
            double roundStartAngle = pointerSetting.CurrentRadius * (startAngle * Math.PI / 180) / pointerSetting.CurrentRadius * 180 / Math.PI;
            double roundEndAngle = pointerSetting.CurrentRadius * (endAngle * Math.PI / 180) / pointerSetting.CurrentRadius * 180 / Math.PI;
            if (!double.IsNaN(pointerSetting.CurrentRadius))
            {
                CalculatePointerRadius(axis, pointer);
            }

            if (pointer.Type == PointerType.RangeBar)
            {
                if (pointer.RoundedCornerRadius != 0 && currentValue != 0 && !isRangeBarRender)
                {
                    pointerSetting.PointerRangeRoundPath = GetRoundedPathArc(axisSetting.MidPoint, Math.Floor(roundStartAngle), Math.Ceiling(roundEndAngle), previousStartValue, previousEndValue, pointerSetting.CurrentRadius, pointer.PointerWidth, pointer.PointerWidth, roundedRadius);
                    roundedRadius = 0;
                }
                else
                {
                    pointerSetting.PointerRangePath = GetCompletePathArc(axisSetting.MidPoint, startAngle, endAngle, pointerSetting.CurrentRadius, pointerSetting.CurrentRadius - pointer.PointerWidth, isRangeBarRender);
                }
            }
            else if (pointer.Type == PointerType.Marker && (pointer.MarkerShape == GaugeShape.Text) && !string.IsNullOrEmpty(pointer.Text))
            {
                pointerSetting.PointerAngle = "rotate(" + (AxisRenderer.GetAngleFromValue(currentValue, axisSetting.TickLineSetting.Maximum, axisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise) + 90).ToString(culture) + "," + pointerSetting.Location.X.ToString(culture) + "," + pointerSetting.Location.Y.ToString(culture) + ")";
            }
            else
            {
                pointerSetting.PointerAngle = "rotate(" + AxisRenderer.GetAngleFromValue(currentValue, axisSetting.TickLineSetting.Maximum, axisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise).ToString(culture) + "," + axisSetting.MidPoint.X.ToString(culture) + "," + axisSetting.MidPoint.Y.ToString(culture) + ")";
            }
        }

        /// <summary>
        /// This method is used to render the pointers for circular gauge.
        /// </summary>
        /// <param name="axis">Specifies the properties of the axis.</param>
        /// <param name="index">Specifies the index of the axis.</param>
        internal void DrawPointers(CircularGaugeAxis axis, int index)
        {
            if (axis.Pointers != null)
            {
                for (int i = 0; i < axis.Pointers.Count; i++)
                {
                    double pointerValue = axis.AxisValues.PointerValue[i];
                    PointerSetting = new PointerSetting();
                    PointerSetting.CurrentDistanceFromScale = !string.IsNullOrEmpty(axis.Pointers[i].Offset) ? SfCircularGauge.StringToNumber(axis.Pointers[i].Offset, Parent.AxisRenderer.AxisSetting.CurrentRadius) : 0;
                    CalculatePointerRadius(axis, axis.Pointers[i]);
                    if (axis.Pointers[i].Type == PointerType.Needle)
                    {
                        DrawNeedlePointer(axis.Pointers[i]);
                    }
                    else if (axis.Pointers[i].Type == PointerType.Marker)
                    {
                        DrawMarkerPointer(axis, axis.Pointers[i], pointerValue);
                    }
                    else if (axis.Pointers[i].Type == PointerType.RangeBar)
                    {
                        DrawRangeBarPointer(axis.Pointers[i], PointerSetting);
                    }

                    PointerSetting.Description = !string.IsNullOrEmpty(axis.Pointers[i].Description) ? axis.Pointers[i].Description : "Pointer:" + pointerValue;
                    PointerCollection.Add(PointerSetting);
#pragma warning disable CA1508
                    if (Parent.AllowAnimation && (axis.Pointers[i].Animation == null || (axis.Pointers[i].Animation != null && axis.Pointers[i].Animation.Enable)))
#pragma warning restore CA1508
                    {
                        SetPointerValue(axis, axis.Pointers[i], 0, PointerCollection[i]);
                        DoPointerAnimation(axis.Pointers[i], axis, pointerValue, index);
                    }
                    else
                    {
                        SetPointerValue(axis, axis.Pointers[i], pointerValue, PointerCollection[i]);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to animate the range bar pointer.
        /// </summary>
        /// <param name="midPoint">Specifies the mid point of the axis.</param>
        /// <param name="rangeLinear">Specifies the properties of the range.</param>
        /// <param name="radius">Specifies the radius of the axis.</param>
        /// <param name="innerRadius">Specifies the inner radius of the axis.</param>
        /// <param name="minimumAngle">Specifies the minimum angle.</param>
        /// <param name="pointerIndex">Specifies the index of the pointer.</param>
        /// <param name="axisIndex">Specifies the index of the axis.</param>
        internal void RangeBarPathAnimation(PointF midPoint, double rangeLinear, double radius, double innerRadius, double minimumAngle, double pointerIndex, double axisIndex)
        {
            Parent.AxisRenderer.AxisCollection[(int)axisIndex].PointerCollection[(int)pointerIndex].PointerRangePath = GetCompletePathArc(midPoint, minimumAngle, rangeLinear + 0.0001, radius, innerRadius, false);
        }

        /// <summary>
        /// This method is used to animate the range bar pointer with rounded corners.
        /// </summary>
        /// <param name="midPoint">Specifies the mid point of the range bar pointer.</param>
        /// <param name="actualStart">Specifies the actual start of the range bar pointer.</param>
        /// <param name="actualEnd">Specifies the actual end of the range bar pointer.</param>
        /// <param name="oldStart">Specifies the old start of the range bar pointer.</param>
        /// <param name="oldEnd">Specifies the old end of the range bar pointer.</param>
        /// <param name="radius">Specifies the radius of the range bar pointer.</param>
        /// <param name="pointerWidth">Specifies the width of the pointer.</param>
        /// <param name="pointerIndex">Specifies the index of the pointer.</param>
        /// <param name="axisIndex">Specifies the index of the axis.</param>
        /// <param name="roundRadius">Specifies the radius of the rounded range bar pointer.</param>
        internal void RoundedRangeBarPathAnimation(PointF midPoint, double actualStart, double actualEnd, double oldStart, double oldEnd, double radius, double pointerWidth, double pointerIndex, double axisIndex, double roundRadius)
        {
            Parent.AxisRenderer.AxisCollection[(int)axisIndex].PointerCollection[(int)pointerIndex].PointerRangeRoundPath = GetRoundedPathArc(midPoint, actualStart, actualEnd, oldStart, oldEnd, radius, pointerWidth, pointerWidth, roundRadius);
        }

        /// <summary>
        /// This method is invoked when the pointer is dragged.
        /// </summary>
        /// <param name="location">Specifies the location of the drag event.</param>
        /// <param name="axisIndex">Specifies the index of the axis for the drag event.</param>
        /// <param name="pointerIndex">Specifies the index of the pointer for the drag event.</param>
        /// <returns>return the value as boolean.</returns>
        internal bool PointerDrag(PointF location, int axisIndex, int pointerIndex)
        {
            bool isWithinRange = false;
            Axis axisSetting = Parent.AxisRenderer.AxisCollection[axisIndex];
            TickLine range = axisSetting.TickLineSetting;
            double pointerValue = AxisRenderer.GetValueFromAngle(AxisRenderer.GetAngleFromLocation(axisSetting.MidPoint, location), range.Maximum, range.Minimum, Parent.Axes[axisIndex].StartAngle, Parent.Axes[axisIndex].EndAngle, Parent.Axes[axisIndex].Direction == GaugeDirection.ClockWise);
            if (pointerValue >= range.Minimum && pointerValue <= range.Maximum)
            {
                isWithinRange = true;
                Parent.Axes[axisIndex].AxisValues.PointerValue[pointerIndex] = pointerValue;
                SetPointerValue(Parent.Axes[axisIndex], Parent.Axes[axisIndex].Pointers[pointerIndex], pointerValue, axisSetting.PointerCollection[pointerIndex], axisSetting);
            }

            return isWithinRange;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal void Dispose()
        {
            Parent = null;
            PointerCollection = null;
            PointerSetting = null;
            PointerAnimate = null;
            RangeAnimate = null;
        }

        private static string ProcessGradientString(string gradientValue)
        {
            return gradientValue.IndexOf("%", StringComparison.InvariantCulture) != -1 ? gradientValue : gradientValue + "%";
        }

        private static Circle CalculateShapes(PointF midPointLocation, SizeD size)
        {
            return new Circle
            {
                RadiusX = size.Width / 2,
                RadiusY = size.Height / 2,
                CenterX = midPointLocation.X,
                CenterY = midPointLocation.Y,
            };
        }

        private void DrawRangeBarPointer(CircularGaugePointer pointer, PointerSetting pointerSetting)
        {
            string gradientRangeBarColor = string.Empty;
            if (pointer.LinearGradient != null)
            {
                gradientRangeBarColor = GetPointerLinearGradientColor(pointer);
            }

            if (pointer.RadialGradient != null && string.IsNullOrEmpty(gradientRangeBarColor))
            {
                gradientRangeBarColor = GetPointerRadialGradientColor(pointer);
            }

            pointerSetting.RangeBarStroke = pointer.Border != null ? pointer.Border.Color : "#DDDDDD";
            pointerSetting.RangeBarStrokeWidth = pointer.Border != null ? pointer.Border.Width : 0;
            pointerSetting.RangeBarColor = !string.IsNullOrEmpty(gradientRangeBarColor) ? gradientRangeBarColor : pointer.Color != null ? pointer.Color : Parent.ThemeStyles.PointerColor;
        }

        private void DrawNeedlePointer(CircularGaugePointer pointer)
        {
            PointerSetting.NeedleLocation = AxisRenderer.GetLocationFromAngle(0, PointerSetting.CurrentRadius, Parent.AxisRenderer.AxisSetting.MidPoint);

            if ((pointer.NeedleStartWidth == 0) && (pointer.NeedleEndWidth == 0) && (((pointer.PointerWidth == 0 ? 20 : pointer.PointerWidth) / 2) == 0))
            {
                PointerSetting.NeedleDirection = "M " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + Parent.AxisRenderer.AxisSetting.MidPoint.Y.ToString(culture) + " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + Parent.AxisRenderer.AxisSetting.MidPoint.Y.ToString(culture) +
                " L " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + Parent.AxisRenderer.AxisSetting.MidPoint.Y.ToString(culture) + " Z";
            }
            else
            {
                PointerSetting.NeedleDirection = "M " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y - (pointer.PointerWidth / 2) - pointer.NeedleEndWidth).ToString(culture) + " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + Parent.AxisRenderer.AxisSetting.MidPoint.Y.ToString(culture) +
            " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y + pointer.NeedleStartWidth).ToString(culture) + " L " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y + (pointer.PointerWidth / 2) + pointer.NeedleEndWidth).ToString(culture) + " Z";
            }

            double radius = SfCircularGauge.StringToNumber(pointer.NeedleTail != null ? pointer.NeedleTail.Length : "0%", PointerSetting.CurrentRadius);
            PointerSetting.RectPath = "M " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y - (pointer.PointerWidth / 2)).ToString(culture) + " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y - (pointer.PointerWidth / 2)).ToString(culture) +
                " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y + (pointer.PointerWidth / 2)).ToString(culture) + " L " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y + (pointer.PointerWidth / 2)).ToString(culture);
            if (!double.IsNaN(radius) || radius != 0)
            {
                PointerSetting.NeedleLocation = AxisRenderer.GetLocationFromAngle(180, radius, Parent.AxisRenderer.AxisSetting.MidPoint);
                PointerSetting.Direction = "M " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y - (pointer.PointerWidth / 2)).ToString(culture) +
                    " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y - (pointer.PointerWidth / 2)).ToString(culture) +
                    " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y + (pointer.PointerWidth / 2)).ToString(culture) +
                    " L " + Parent.AxisRenderer.AxisSetting.MidPoint.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y + (pointer.PointerWidth / 2)).ToString(culture) + " Z";
                PointerSetting.RectPath += " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y + (pointer.PointerWidth / 2)).ToString(culture) + " L " + PointerSetting.NeedleLocation.X.ToString(culture) + SPACE + (Parent.AxisRenderer.AxisSetting.MidPoint.Y - (pointer.PointerWidth / 2)).ToString(culture);
            }

            SizeD capSize = new SizeD
            {
                Width = pointer.Cap != null ? pointer.Cap.Radius * 2 : 16,
                Height = pointer.Cap != null ? pointer.Cap.Radius * 2 : 16,
            };
            PointerSetting.CapPath = CalculateShapes(Parent.AxisRenderer.AxisSetting.MidPoint, capSize);
            string gradientColor = string.Empty;
            if (pointer.Cap != null)
            {
                if (pointer.Cap.LinearGradient != null)
                {
                    gradientColor = GetCapLinearGradientColor(pointer);
                }

                if (pointer.Cap.RadialGradient != null && string.IsNullOrEmpty(gradientColor))
                {
                    gradientColor = GetCapRadialGradientColor(pointer);
                }
            }

            PointerSetting.CapColor = !string.IsNullOrEmpty(gradientColor) ? gradientColor : (pointer.Cap != null && !string.IsNullOrEmpty(pointer.Cap.Color) ? pointer.Cap.Color : Parent.ThemeStyles.CapColor);
            PointerSetting.CapStroke = pointer.Cap != null && pointer.Cap.Border != null && !string.IsNullOrEmpty(pointer.Cap.Border.Color) ? pointer.Cap.Border.Color : Parent.ThemeStyles.CapColor;
            PointerSetting.CapStrokeWidth = pointer.Cap != null && pointer.Cap.Border != null ? pointer.Cap.Border.Width : 0;
            gradientColor = string.Empty;
            if (pointer.NeedleTail != null)
            {
                if (pointer.NeedleTail.LinearGradient != null)
                {
                    gradientColor = GetNeedleLinearGradientColor(pointer);
                }

                if (pointer.NeedleTail.RadialGradient != null && string.IsNullOrEmpty(gradientColor))
                {
                    gradientColor = GetNeedleRadialGradientColor(pointer);
                }
            }

            PointerSetting.NeedleStroke = pointer.NeedleTail != null && pointer.NeedleTail.Border != null && !string.IsNullOrEmpty(pointer.NeedleTail.Border.Color) ? pointer.NeedleTail.Border.Color : "transparent";
            PointerSetting.NeedleStrokeWidth = pointer.NeedleTail != null && pointer.NeedleTail.Border != null ? pointer.NeedleTail.Border.Width : 0;
            PointerSetting.NeedleColor = !string.IsNullOrEmpty(gradientColor) ? gradientColor : (pointer.NeedleTail != null && pointer.NeedleTail.Color != null ? pointer.NeedleTail.Color : Parent.ThemeStyles.NeedleTailColor);
            if (pointer.Border != null)
            {
                PointerSetting.PointerBorderStroke = !string.IsNullOrEmpty(pointer.Border.Color) ? pointer.Border.Color : "transparent";
                PointerSetting.PointerBorderStrokeWidth = !double.IsNaN(pointer.Border.Width) ? pointer.Border.Width : 0;
            }

            gradientColor = string.Empty;
            if (pointer.LinearGradient != null)
            {
                gradientColor = GetPointerLinearGradientColor(pointer);
            }

            if (pointer.RadialGradient != null && string.IsNullOrEmpty(gradientColor) && string.IsNullOrEmpty(gradientColor))
            {
                gradientColor = GetPointerRadialGradientColor(pointer);
            }

            PointerSetting.PointerColor = !string.IsNullOrEmpty(gradientColor) ? gradientColor : !string.IsNullOrEmpty(pointer.Color) ? pointer.Color : Parent.ThemeStyles.PointerColor;
        }

        private void DrawMarkerPointer(CircularGaugeAxis axis, CircularGaugePointer pointer, double pointerValue)
        {
            double angle = Math.Round(AxisRenderer.GetAngleFromValue(pointerValue, axis.Maximum != 0 ? axis.Maximum : 100, axis.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise));
            GaugeShape shape = pointer.MarkerShape;
            if (!string.IsNullOrEmpty(pointer.Radius) && (pointer.MarkerShape == GaugeShape.InvertedTriangle || pointer.MarkerShape == GaugeShape.Triangle))
            {
                shape = ((pointer.Position == PointerRangePosition.Outside || pointer.Position == PointerRangePosition.Cross) && pointer.MarkerShape == GaugeShape.Triangle) ?
                GaugeShape.InvertedTriangle : ((pointer.Position == PointerRangePosition.Inside) &&
                    (pointer.MarkerShape == GaugeShape.InvertedTriangle)) ? GaugeShape.Triangle : pointer.MarkerShape;
            }

            PointerSetting.Location = AxisRenderer.GetLocationFromAngle((pointer.MarkerShape == GaugeShape.Text) ? angle : 0, PointerSetting.CurrentRadius, Parent.AxisRenderer.AxisSetting.MidPoint);
            string gradientPointerColor = string.Empty;
            if (pointer.LinearGradient != null)
            {
                gradientPointerColor = GetPointerLinearGradientColor(pointer);
            }

            if (pointer.RadialGradient != null && string.IsNullOrEmpty(gradientPointerColor))
            {
                gradientPointerColor = GetPointerRadialGradientColor(pointer);
            }

            PointerSetting.PointerColor = !string.IsNullOrEmpty(gradientPointerColor) ? gradientPointerColor : (!string.IsNullOrEmpty(pointer.Color) ? pointer.Color : Parent.ThemeStyles.PointerColor);
            if (pointer.MarkerShape == GaugeShape.Text)
            {
                PointerSetting.PointerColor = pointer.TextStyle != null && !string.IsNullOrEmpty(pointer.TextStyle.Color) ? pointer.TextStyle.Color : PointerSetting.PointerColor;
                PointerSetting.TextStyle = pointer.TextStyle != null ? "font-size:" + pointer.TextStyle.Size + ";" + "color:" + pointer.TextStyle.Color + ";" + "font-family:" + pointer.TextStyle.FontFamily + ";" + "opacity:" + pointer.TextStyle.Opacity + ";" + "font-weight:" + pointer.TextStyle.FontWeight + ";" + "font-style:" + pointer.TextStyle.FontStyle + ";" : string.Empty;
            }
            else if (pointer.MarkerShape == GaugeShape.Circle)
            {
                PointerSetting.Circle = new Circle
                {
                    RadiusX = pointer.MarkerHeight / 2,
                    RadiusY = pointer.MarkerWidth / 2,
                    CenterX = PointerSetting.Location.X,
                    CenterY = PointerSetting.Location.Y,
                };
            }
            else if (pointer.MarkerShape == GaugeShape.Image)
            {
                PointerSetting.ImageX = PointerSetting.Location.X + (-pointer.MarkerWidth / 2);
                PointerSetting.ImageY = PointerSetting.Location.Y + (-pointer.MarkerHeight / 2);
            }
            else
            {
                PointerSetting.MarkerShape = CalculateShape(PointerSetting.Location, shape, pointer.MarkerHeight, pointer.MarkerWidth);
            }

            PointerSetting.MarkerStroke = pointer.Border != null ? pointer.Border.Color : "#DDDDDD";
            PointerSetting.MarkerStrokeWidth = pointer.Border != null ? pointer.Border.Width : 0;
            PointerSetting.MarkerColor = !string.IsNullOrEmpty(pointer.Color) ? pointer.Color : Parent.ThemeStyles.PointerColor;
        }

        private string CalculateShape(PointF location, GaugeShape shape, double markerHeight, double markerWidth)
        {
            string path = string.Empty;
            double locationX = location.X;
            double locationY = location.Y;
            double height = markerHeight;
            double width = markerWidth;
            double x = location.X + (-width / 2);

            if (shape == GaugeShape.Rectangle)
            {
                path = "M" + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE +
                  "L" + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE +
                  "L" + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE +
                  "L" + SPACE + x.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE +
                  "L" + SPACE + x.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE + "Z";
            }
            else if (shape == GaugeShape.Diamond)
            {
                path = "M" + SPACE + x.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE +
                "L" + SPACE + locationX.ToString(culture) + SPACE + (locationY + (-height / 2)).ToString(culture) + SPACE +
                "L" + SPACE + (locationX + (width / 2)).ToString(culture) + SPACE + locationY.ToString(culture) + SPACE +
                "L" + SPACE + locationX.ToString(culture) + SPACE + (locationY + (height / 2)).ToString(culture) + SPACE +
                "L" + SPACE + x.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE + "Z";
            }
            else if (shape == GaugeShape.Triangle)
            {
                path = "M" + SPACE + locationX.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE +
                    "L" + SPACE + (locationX - height).ToString(culture) + SPACE + (locationY - (width / 2)).ToString(culture) +
                    "L" + SPACE + (locationX - height).ToString(culture) + SPACE + (locationY + (width / 2)).ToString(culture) + "Z";
            }
            else if (shape == GaugeShape.InvertedTriangle)
            {
                path = "M" + SPACE + locationX.ToString(culture) + SPACE + locationY.ToString(culture) + SPACE +
                    "L" + SPACE + (locationX + height).ToString(culture) + SPACE + (locationY - (width / 2)).ToString(culture) +
                    "L" + SPACE + (locationX + height).ToString(culture) + SPACE + (locationY + (width / 2)).ToString(culture) + "Z";
            }

            return path;
        }

        private void CalculatePointerRadius(CircularGaugeAxis axis, CircularGaugePointer pointer)
        {
            double padding = 5;
            PointerSetting.CurrentRadius = !string.IsNullOrEmpty(pointer.Radius) ?
                SfCircularGauge.StringToNumber(pointer.Radius, Parent.AxisRenderer.AxisSetting.CurrentRadius) : pointer.Position != PointerRangePosition.Auto ?
                    CalculatePointerRadiusForPosition(axis, pointer) : (Parent.AxisRenderer.AxisSetting.CurrentRadius - (Parent.AxisRenderer.AxisSetting.FarSize + padding));
        }

        private double CalculatePointerRadiusForPosition(CircularGaugeAxis axis, CircularGaugePointer pointer)
        {
            double axisLineWidth = axis.LineStyle != null ? axis.LineStyle.Width : 2;
            double currentRadius = Parent.AxisRenderer.AxisSetting.CurrentRadius;
            if (pointer.MarkerShape == GaugeShape.Text)
            {
                double pointerSize;
                if (pointer.TextStyle != null)
                {
                    char[] character = { 'p', 'x' };
                    string textStyle = pointer.TextStyle.Size.TrimEnd(character);
                    pointerSize = double.Parse(textStyle, culture);
                }
                else
                {
                    pointerSize = 16;
                }

                double offset = pointer.Position == PointerRangePosition.Cross ? pointerSize / 5 : 0;
                double radius = pointer.Position == PointerRangePosition.Inside ? (currentRadius - (pointerSize / 1.2) - (axisLineWidth / 2) - offset -
                    PointerSetting.CurrentDistanceFromScale) : pointer.Position == PointerRangePosition.Outside ?
                    (currentRadius + (axisLineWidth / 2) + (pointerSize / 4) + offset + PointerSetting.CurrentDistanceFromScale) :
                    (currentRadius - (pointerSize / 6) - offset - PointerSetting.CurrentDistanceFromScale);
                return radius;
            }
            else
            {
                double rangeBarOffset = pointer.Type == PointerType.RangeBar ? pointer.PointerWidth : 0;
                double offset = pointer.Type == PointerType.Marker ? ((pointer.MarkerShape == GaugeShape.InvertedTriangle ||
                    pointer.MarkerShape == GaugeShape.Triangle) ? (pointer.Position == PointerRangePosition.Cross ? pointer.MarkerWidth / 2 : 0) :
                    pointer.MarkerWidth / 2) : 0;
                double radius = pointer.Position == PointerRangePosition.Inside ? (currentRadius - (axisLineWidth / 2) - offset - PointerSetting.CurrentDistanceFromScale) :
                    pointer.Position == PointerRangePosition.Outside ? (currentRadius + rangeBarOffset + (axisLineWidth / 2) + offset + PointerSetting.CurrentDistanceFromScale) :
                        (currentRadius + (rangeBarOffset / 2) - PointerSetting.CurrentDistanceFromScale -
                            ((pointer.MarkerShape == GaugeShape.InvertedTriangle || pointer.MarkerShape == GaugeShape.Triangle) ? offset : 0));
                return radius;
            }
        }

        private string GetRoundedPathArc(PointF center, double actualStart, double actualEnd, double oldStart, double oldEnd, double radius, double startWidth, double endWidth, double roundedCornerRadius)
        {
            actualEnd -= AxisRenderer.IsCompleteAngle(actualStart, actualEnd) ? 0.0001 : 0;
            double roundDegree = AxisRenderer.GetDegreeValue(actualStart, actualEnd);
            startWidth = radius < startWidth ? radius : startWidth;
            endWidth = radius < endWidth ? radius : endWidth;
            double startRadius = radius - startWidth;
            double endRadius = radius - endWidth;
            double arcRadius = radius - ((startWidth + endWidth) / 2);
            double degreeMidValue = 180;
            if (!double.IsNaN(roundedCornerRadius))
            {
                oldEnd += radius >= startWidth ? roundedCornerRadius / radius : 0;
                oldStart -= radius >= startWidth ? roundedCornerRadius / radius : 0;
                actualStart += radius >= startWidth ? roundedCornerRadius / 2 : 0;
                actualEnd -= radius >= startWidth ? roundedCornerRadius / 2 : 0;
                degreeMidValue += roundedCornerRadius;
            }

            return GetRoundedPath(
                AxisRenderer.GetLocationFromAngle(actualStart, radius, center), AxisRenderer.GetLocationFromAngle(actualEnd, radius, center), AxisRenderer.GetLocationFromAngle(oldEnd, radius, center), AxisRenderer.GetLocationFromAngle(oldEnd, endRadius, center), AxisRenderer.GetLocationFromAngle(oldStart, radius, center), AxisRenderer.GetLocationFromAngle(oldStart, startRadius, center), AxisRenderer.GetLocationFromAngle(actualStart, startRadius, center), AxisRenderer.GetLocationFromAngle(actualEnd, endRadius, center), radius, arcRadius, arcRadius, (roundDegree <= degreeMidValue) ? 0 : 1);
        }

        private string GetRoundedPath(PointF start, PointF end, PointF outerOldEnd, PointF innerOldEnd, PointF outerOldStart, PointF innerOldStart, PointF innerStart, PointF innerEnd, double radius, double startRadius, double endRadius, double clockWise)
        {
            return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) +
            " A " + radius.ToString(culture) + SPACE + radius.ToString(culture) + " 0 " +
            clockWise.ToString(culture) + " 1 " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture) +
            " C " + outerOldEnd.X.ToString(culture) + SPACE + outerOldEnd.Y.ToString(culture) + SPACE + innerOldEnd.X.ToString(culture) + SPACE +
            innerOldEnd.Y.ToString(culture) + SPACE + innerEnd.X.ToString(culture) + SPACE + innerEnd.Y.ToString(culture) +
            " A " + endRadius.ToString(culture) + SPACE + startRadius.ToString(culture) + " 0 " +
            clockWise.ToString(culture) + " 0 " + innerStart.X.ToString(culture) + SPACE + innerStart.Y.ToString(culture) +
            " C " + innerOldStart.X.ToString(culture) + SPACE + innerOldStart.Y.ToString(culture) + SPACE + outerOldStart.X.ToString(culture) + SPACE +
            outerOldStart.Y.ToString(culture) + SPACE + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " Z";
        }

        private string GetCompletePathArc(PointF center, double actualStart, double actualEnd, double radius, double innerRadius, bool isClockwise)
        {
            actualEnd -= AxisRenderer.IsCompleteAngle(actualStart, actualEnd) && !isClockwise ? 0.0001 : 0;
            double roundDegree = AxisRenderer.GetDegreeValue(actualStart, actualEnd);
            return GetCompletePath(
               AxisRenderer.GetLocationFromAngle(actualStart, radius, center), AxisRenderer.GetLocationFromAngle(actualEnd, radius, center), radius, AxisRenderer.GetLocationFromAngle(actualStart, innerRadius, center), AxisRenderer.GetLocationFromAngle(actualEnd, innerRadius, center), innerRadius, (roundDegree < 180) ? 0 : 1);
        }

        private string GetCompletePath(PointF start, PointF end, double radius, PointF innerStart, PointF innerEnd, double innerRadius, double clockWise)
        {
            return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " A " + radius.ToString(culture) + SPACE + radius.ToString(culture) + " 0 " + clockWise.ToString(culture) +
            " 1 " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture) + " L " + innerEnd.X.ToString(culture) + SPACE + innerEnd.Y.ToString(culture) + " A " + innerRadius.ToString(culture) +
            SPACE + innerRadius.ToString(culture) + " 0 " + clockWise.ToString(culture) + ",0 " + innerStart.X.ToString(culture) + SPACE + innerStart.Y.ToString(culture) + " Z";
        }

        private string GetCapLinearGradientColor(CircularGaugePointer pointer)
        {
            LinearGradient linearGradient = pointer.Cap.LinearGradient;
            PointerSetting.CapLinearStartValue = !string.IsNullOrEmpty(linearGradient.StartValue) ? ProcessGradientString(linearGradient.StartValue) : string.Empty;
            PointerSetting.CapLinearEndValue = !string.IsNullOrEmpty(linearGradient.EndValue) ? ProcessGradientString(linearGradient.EndValue) : string.Empty;
            PointerSetting.CapLinearString = !string.IsNullOrEmpty(PointerSetting.CapLinearStartValue) || !string.IsNullOrEmpty(PointerSetting.CapLinearEndValue) ? Parent.ID + "linearGradient_cap"
                : string.Empty;
            return !string.IsNullOrEmpty(PointerSetting.CapLinearString) ? "url(#" + PointerSetting.CapLinearString + ")" : string.Empty;
        }

        private string GetCapRadialGradientColor(CircularGaugePointer pointer)
        {
            RadialGradient radialGradient = pointer.Cap.RadialGradient;
            PointerSetting.CapRadialRadius = !string.IsNullOrEmpty(radialGradient.Radius) ? ProcessGradientString(radialGradient.Radius) : string.Empty;
            PointerSetting.CapOuterPositionX = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.X) ?
                ProcessGradientString(radialGradient.OuterPosition.X) : string.Empty;
            PointerSetting.CapOuterPositionY = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.Y) ?
                ProcessGradientString(radialGradient.OuterPosition.Y) : string.Empty;
            PointerSetting.CapInnerPositionX = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.X) ?
                ProcessGradientString(radialGradient.InnerPosition.X) : string.Empty;
            PointerSetting.CapInnerPositionY = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.Y) ?
                ProcessGradientString(radialGradient.InnerPosition.Y) : string.Empty;
            PointerSetting.CapRadialString = !string.IsNullOrEmpty(PointerSetting.CapRadialRadius) || !string.IsNullOrEmpty(PointerSetting.CapOuterPositionX) || !string.IsNullOrEmpty(PointerSetting.CapOuterPositionY)
                || !string.IsNullOrEmpty(PointerSetting.CapInnerPositionX) || !string.IsNullOrEmpty(PointerSetting.CapInnerPositionY) ?
                Parent.ID + "_radialGradient_cap" : string.Empty;
            return !string.IsNullOrEmpty(PointerSetting.CapRadialString) ? "url(#" + PointerSetting.CapRadialString + ")" : string.Empty;
        }

        private string GetNeedleLinearGradientColor(CircularGaugePointer pointer)
        {
            LinearGradient linearGradient = pointer.NeedleTail.LinearGradient;
            PointerSetting.NeedleLinearStartValue = !string.IsNullOrEmpty(linearGradient.StartValue) ? ProcessGradientString(linearGradient.StartValue) : string.Empty;
            PointerSetting.NeedleLinearEndValue = !string.IsNullOrEmpty(linearGradient.EndValue) ? ProcessGradientString(linearGradient.EndValue) : string.Empty;
            PointerSetting.NeedleLinearString = !string.IsNullOrEmpty(PointerSetting.NeedleLinearStartValue) || !string.IsNullOrEmpty(PointerSetting.NeedleLinearEndValue) ?
                Parent.ID + "_linearGradient_needleTail" : string.Empty;
            return !string.IsNullOrEmpty(PointerSetting.NeedleLinearString) ? "url(#" + PointerSetting.NeedleLinearString + ")" : string.Empty;
        }

        private string GetNeedleRadialGradientColor(CircularGaugePointer pointer)
        {
            RadialGradient radialGradient = pointer.NeedleTail.RadialGradient;
            PointerSetting.NeedleRadialRadius = !string.IsNullOrEmpty(radialGradient.Radius) ? (radialGradient.Radius.IndexOf("%", StringComparison.InvariantCulture) != -1 ?
            radialGradient.Radius : int.Parse(radialGradient.Radius, culture).ToString(culture) + "%") : string.Empty;
            PointerSetting.NeedleOuterPositionX = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.X) ? ProcessGradientString(radialGradient.OuterPosition.X) : string.Empty;
            PointerSetting.NeedleOuterPositionY = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.Y) ? ProcessGradientString(radialGradient.OuterPosition.Y) : string.Empty;
            PointerSetting.NeedleInnerPositionX = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.X) ? ProcessGradientString(radialGradient.InnerPosition.X) : string.Empty;
            PointerSetting.NeedleInnerPositionY = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.X) ? ProcessGradientString(radialGradient.InnerPosition.Y) : string.Empty;
            PointerSetting.NeedleRadialString = !string.IsNullOrEmpty(PointerSetting.NeedleRadialRadius) || !string.IsNullOrEmpty(PointerSetting.NeedleOuterPositionX) || !string.IsNullOrEmpty(PointerSetting.NeedleOuterPositionY)
                || !string.IsNullOrEmpty(PointerSetting.NeedleInnerPositionX) || !string.IsNullOrEmpty(PointerSetting.NeedleInnerPositionY) ?
                Parent.ID + "_radialGradient_needleTail" : string.Empty;
            return !string.IsNullOrEmpty(PointerSetting.NeedleRadialString) ? "url(#" + PointerSetting.NeedleRadialString + ")" : string.Empty;
        }

        private string GetPointerLinearGradientColor(CircularGaugePointer pointer)
        {
            LinearGradient linearGradient = pointer.LinearGradient;
            PointerSetting.PointerLinearStartValue = !string.IsNullOrEmpty(linearGradient.StartValue) ? ProcessGradientString(linearGradient.StartValue) : string.Empty;
            PointerSetting.PointerLinearEndValue = !string.IsNullOrEmpty(linearGradient.EndValue) ? ProcessGradientString(linearGradient.EndValue) : string.Empty;
            PointerSetting.PointerLinearString = !string.IsNullOrEmpty(PointerSetting.PointerLinearStartValue) || !string.IsNullOrEmpty(PointerSetting.PointerLinearEndValue) ?
                Parent.ID + "_pointer_linearGradient" : string.Empty;
            return !string.IsNullOrEmpty(PointerSetting.PointerLinearString) ? "url(#" + PointerSetting.PointerLinearString + ")" : string.Empty;
        }

        private string GetPointerRadialGradientColor(CircularGaugePointer pointer)
        {
            RadialGradient radialGradient = pointer.RadialGradient;
            PointerSetting.PointerRadialRadius = !string.IsNullOrEmpty(radialGradient.Radius) ? (radialGradient.Radius.IndexOf("%", StringComparison.InvariantCulture) != -1 ?
            radialGradient.Radius : int.Parse(radialGradient.Radius, culture).ToString(culture) + "%") : string.Empty;
            PointerSetting.PointerOuterPositionX = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.X) ?
                ProcessGradientString(radialGradient.OuterPosition.X) : string.Empty;
            PointerSetting.PointerOuterPositionY = radialGradient.OuterPosition != null && !string.IsNullOrEmpty(radialGradient.OuterPosition.Y) ?
                ProcessGradientString(radialGradient.OuterPosition.Y) : string.Empty;
            PointerSetting.PointerInnerPositionX = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.X) ?
                ProcessGradientString(radialGradient.InnerPosition.X) : string.Empty;
            PointerSetting.PointerInnerPositionY = radialGradient.InnerPosition != null && !string.IsNullOrEmpty(radialGradient.InnerPosition.Y) ?
                ProcessGradientString(radialGradient.InnerPosition.Y) : string.Empty;
            PointerSetting.PointerRadialString = !string.IsNullOrEmpty(PointerSetting.PointerRadialRadius) || !string.IsNullOrEmpty(PointerSetting.PointerOuterPositionX) || !string.IsNullOrEmpty(PointerSetting.PointerOuterPositionY)
                || !string.IsNullOrEmpty(PointerSetting.PointerInnerPositionX) || !string.IsNullOrEmpty(PointerSetting.PointerInnerPositionY) ? Parent.ID + "_pointer_radialGradient" : string.Empty;
            return !string.IsNullOrEmpty(PointerSetting.PointerRadialString) ? "url(#" + PointerSetting.PointerRadialString + ")" : string.Empty;
        }

        private void DoPointerAnimation(CircularGaugePointer pointer, CircularGaugeAxis axis, double currentPointerValue, int index)
        {
            double animateStart = Parent.AxisRenderer.AxisSetting.TickLineSetting.Minimum;
            if (pointer.Type == PointerType.RangeBar)
            {
                PerformRangeBarAnimation(animateStart, currentPointerValue, axis, pointer, PointerSetting.CurrentRadius, PointerSetting.CurrentRadius - pointer.PointerWidth, index);
            }
            else
            {
                PerformNeedleAnimation(animateStart, currentPointerValue, axis, pointer);
            }
        }

        private void PerformRangeBarAnimation(double animateStart, double animateEnd, CircularGaugeAxis axis, CircularGaugePointer pointer, double radius, double innerRadius, int index)
        {
            Axis currentAxis = Parent.AxisRenderer.AxisCollection[index];
            bool isClockWise = axis.Direction == GaugeDirection.ClockWise;
            double startAngle = AxisRenderer.GetAngleFromValue(animateStart, currentAxis.TickLineSetting.Maximum, currentAxis.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, isClockWise);
            double minimumAngle = AxisRenderer.GetAngleFromValue(currentAxis.TickLineSetting.Minimum, currentAxis.TickLineSetting.Maximum, currentAxis.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, isClockWise);
            double pointAngle = AxisRenderer.GetAngleFromValue(animateEnd, currentAxis.TickLineSetting.Maximum, currentAxis.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, isClockWise);
            double minimumRadius = radius * 0.25;
            double oldStart = 0;
            double roundedRadius = Math.Min(Math.Min(pointer.PointerWidth, pointer.RoundedCornerRadius), pointer.Value) / 2;
            if (!double.IsNaN(roundedRadius))
            {
                minimumAngle = PointerSetting.CurrentRadius * (minimumAngle * Math.PI / 180) / PointerSetting.CurrentRadius * 180 / Math.PI;
                pointAngle = PointerSetting.CurrentRadius * (pointAngle * Math.PI / 180) / PointerSetting.CurrentRadius * 180 / Math.PI;
                oldStart = (((PointerSetting.CurrentRadius - (pointer.PointerWidth / 2)) * (startAngle * Math.PI / 180)) -
                    (radius / minimumRadius)) / (PointerSetting.CurrentRadius - (pointer.PointerWidth / 2)) * 180 / Math.PI;
            }

            AnimationOptions animation = new AnimationOptions
            {
                Duration = pointer.Animation != null ? pointer.Animation.Duration : 1500,
                Start = animateStart,
                End = animateEnd,
                IsClockWise = isClockWise,
                StartAngle = startAngle,
                EndAngle = startAngle > pointAngle ? (pointAngle + 360) : pointAngle,
                MidPoint = currentAxis.MidPoint,
                Radius = radius,
                InnerRadius = innerRadius,
                MinimumAngle = minimumAngle,
                OldStart = oldStart,
                PointerWidth = pointer.PointerWidth,
                RoundRadius = roundedRadius
            };
            RangeAnimate.Add(animation);
        }

        private void PerformNeedleAnimation(double animateStart, double animateEnd, CircularGaugeAxis axis, CircularGaugePointer pointer)
        {
            bool isClockWise = axis.Direction == GaugeDirection.ClockWise;
            double startAngle = AxisRenderer.GetAngleFromValue(animateStart, Parent.AxisRenderer.AxisSetting.TickLineSetting.Maximum, Parent.AxisRenderer.AxisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, isClockWise);
            double pointAngle = AxisRenderer.GetAngleFromValue(animateEnd, Parent.AxisRenderer.AxisSetting.TickLineSetting.Maximum, Parent.AxisRenderer.AxisSetting.TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, isClockWise);
            AnimationOptions animation = new AnimationOptions
            {
                Duration = pointer.Animation != null ? pointer.Animation.Duration : 1500,
                Start = animateStart,
                End = animateEnd,
                IsClockWise = isClockWise,
                StartAngle = startAngle,
                EndAngle = startAngle > pointAngle ? (pointAngle + 360) : pointAngle,
                MidPoint = Parent.AxisRenderer.AxisSetting.MidPoint,
            };
            PointerAnimate.Add(animation);
        }
    }
}

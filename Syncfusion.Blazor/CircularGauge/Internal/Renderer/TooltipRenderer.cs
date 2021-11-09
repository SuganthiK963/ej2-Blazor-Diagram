using Microsoft.AspNetCore.Components;
using System;
using System.Drawing;
using Syncfusion.Blazor.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Specifies the properties and methods to render the tooltip in the Circular Gauge.
    /// </summary>
    internal class TooltipRenderer
    {
        private const int ARROWPADDING = 12;
        private BoundingClientRect rectBounds = new BoundingClientRect();
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipRenderer"/> class.
        /// </summary>
        /// <param name="parent">represent the properties of the tooltip.</param>
        public TooltipRenderer(SfCircularGauge parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets or sets the properties of the circular gauge.
        /// </summary>
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of the tooltip.
        /// </summary>
        internal TooltipCollection TooltipSetting { get; set; } = new TooltipCollection();

        /// <summary>
        /// Gets or sets the bounds of the tooltip.
        /// </summary>
        internal BoundingClientRect TooltipBounds { get; set; } = new BoundingClientRect();

        /// <summary>
        /// Renders the tooltip template for circular gaugeslates the legend pages for circular gauge.
        /// </summary>
        /// <param name="content">Specifies the value for the tooltip template.</param>
        /// <returns>Returns the RenderFragment.</returns>
        internal RenderFragment RenderTooltipTemplate(RenderFragment content)
        {
            RenderFragment fragment = builder =>
            {
                int seq = 0;
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "id", Parent.ID + "_Tooltip_TooltipTemplate");
                builder.AddContent(seq++, content);
                builder.CloseElement();
            };
            return fragment;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal void Dispose()
        {
            Parent = null;
            rectBounds = null;
            TooltipSetting = null;
            TooltipBounds = null;
        }

        /// <summary>
        /// Renders the tooltip for circular gauge.
        /// </summary>
        /// <param name="mouseLocation">Specifies the location of the mouse over event for tooltip rendering.</param>
        /// <param name="rectBounds">Specifies the rect bounds of the mouse over event for tooltip rendering.</param>
        /// <param name="axisIndex">Specifies the index number of the axis of the mouse over event for tooltip rendering.</param>
        /// <param name="index">Specifies the index of the mouse over event for tooltip rendering..</param>
        /// <param name="isRange">Specifies the type of the tooltip is range.</param>
        /// <param name="isPointer">Specifies the type of the tooltip is pointer.</param>
        /// <param name="isAnnotation">Specifies the type of the tooltip is annotation.</param>
        /// <returns>return the arguments of the event of the tooltip.</returns>
        internal TooltipRenderEventArgs RenderTooltip(PointF mouseLocation, BoundingClientRect rectBounds, int axisIndex, int index, bool isRange, bool isPointer, bool isAnnotation)
        {
            double angle = 0;
            TooltipSetting.IsPointer = TooltipSetting.IsRange = TooltipSetting.IsAnnotation = false;
            CircularGaugeAxis axis = Parent.Axes[axisIndex];
            PointF location = PointF.Empty;
            TooltipRenderEventArgs args = null;
            SizeD textSize = new SizeD();
            if (isPointer)
            {
                TooltipSetting.IsPointer = true;
#pragma warning disable CA1305
                TooltipSetting.PointerValue = !string.IsNullOrEmpty(Parent.Tooltip.Format) && !Parent.Tooltip.Format.Contains("{value}", StringComparison.InvariantCulture) ?
                    Intl.GetNumericFormat(Parent.Axes[axisIndex].AxisValues.PointerValue[index], Parent.Tooltip.Format) : axis.AxisValues.PointerValue[index].ToString();
                TooltipSetting.PointerValue = !string.IsNullOrEmpty(Parent.Tooltip.Format) ? (Parent.Tooltip.Format.Contains("{value}", StringComparison.InvariantCulture) ?
                    Parent.Tooltip.Format.Replace("{value}", TooltipSetting.PointerValue, StringComparison.InvariantCulture) : TooltipSetting.PointerValue)
                    : TooltipSetting.PointerValue;
#pragma warning restore CA1305
                TooltipSetting.PointerValue = Parent.EnableGroupingSeparator ? TooltipSetting.PointerValue : TooltipSetting.PointerValue.Replace(",", string.Empty, StringComparison.InvariantCulture);
                CircularGaugeTooltipTextStyle textStyle = Parent.Tooltip.TextStyle;
                TooltipSetting.PointerFontSize = textStyle != null && !string.IsNullOrEmpty(textStyle.Size) ? textStyle.Size : "13px";
#pragma warning disable CS0618
                if (Parent.Tooltip.TooltipTemplate == null)
#pragma warning restore CS0618
                {
                    TooltipSetting.PointerFontColor = textStyle != null && !string.IsNullOrEmpty(textStyle.Color) ? textStyle.Color : Parent.ThemeStyles.TooltipFontColor;
                    TooltipSetting.PointerFontFamily = textStyle != null && !string.IsNullOrEmpty(textStyle.FontFamily) ? textStyle.FontFamily : Parent.ThemeStyles.FontFamily;
                    TooltipSetting.PointerFontStyle = textStyle != null && !string.IsNullOrEmpty(textStyle.FontStyle) ? textStyle.FontStyle : string.Empty;
                    TooltipSetting.PointerFontWeight = textStyle != null && !string.IsNullOrEmpty(textStyle.FontWeight) ? textStyle.FontWeight : string.Empty;
                    TooltipSetting.PointerFontOpacity = textStyle != null ? textStyle.Opacity : 1;
                }
                else
                {
                    double fontSize = TooltipSetting.PointerFontSize.IndexOf("px", StringComparison.InvariantCulture) > 0 ?
                        float.Parse(TooltipSetting.PointerFontSize.Replace("px", string.Empty, StringComparison.InvariantCulture), culture) : float.Parse(TooltipSetting.PointerFontSize, culture);
                    textSize = AxisRenderer.MeasureText(TooltipSetting.PointerValue, fontSize);
                }

                angle = AxisRenderer.GetAngleFromValue(axis.AxisValues.PointerValue[index], Parent.AxisRenderer.AxisCollection[axisIndex].TickLineSetting.Maximum, Parent.AxisRenderer.AxisCollection[axisIndex].TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise) % 360;
                location = AxisRenderer.GetLocationFromAngle(angle, Parent.AxisRenderer.AxisCollection[axisIndex].CurrentRadius, Parent.AxisRenderer.AxisCollection[axisIndex].MidPoint);

                args = CreateTooltipRenderEventArguments(false, false, TooltipSetting.PointerValue, "Pointer", new PointF() { X = location.X, Y = location.Y });
            }
            else if (isRange)
            {
                TooltipSetting.IsRange = true;
#pragma warning disable CA2000
                CircularGaugeRangeTooltipSettings rangeTooltipSetting = Parent.Tooltip.RangeSettings != null ? Parent.Tooltip.RangeSettings : new CircularGaugeRangeTooltipSettings();
#pragma warning restore CA2000
                angle = AxisRenderer.GetAngleFromValue(
                axis.AxisValues.RangeEnd[index] - Math.Abs((axis.AxisValues.RangeEnd[index] - axis.AxisValues.RangeStart[index]) / 2), Parent.AxisRenderer.AxisCollection[axisIndex].TickLineSetting.Maximum, Parent.AxisRenderer.AxisCollection[axisIndex].TickLineSetting.Minimum, axis.AngleStart, axis.AngleEnd, axis.Direction == GaugeDirection.ClockWise) % 360;
                double startValue = Parent.Axes[axisIndex].AxisValues.RangeStart[index];
                double endValue = Parent.Axes[axisIndex].AxisValues.RangeEnd[index];
                TooltipSetting.RangeTooltipStart = "Start: " + startValue + "<br>End: " + endValue;
                bool isRangeTextStyle = Parent.Tooltip.RangeSettings != null && Parent.Tooltip.RangeSettings.TextStyle != null;
                CircularGaugeRangeTooltipTextStyle textStyle = isRangeTextStyle ? Parent.Tooltip.RangeSettings.TextStyle : null;
                TooltipSetting.RangeFontSize = isRangeTextStyle && !string.IsNullOrEmpty(textStyle.Size) ? textStyle.Size : "13px";
#pragma warning disable CA1508
                if (Parent.Tooltip.RangeSettings == null || Parent.Tooltip.RangeSettings?.Template == null)
#pragma warning restore CA1508
                {
                    try
                    {
#pragma warning disable CA1305
                        string formattedStart = !rangeTooltipSetting.Format.Contains("{start}", StringComparison.InvariantCulture) ? Intl.GetNumericFormat(startValue, rangeTooltipSetting.Format) : startValue.ToString();
                        string formattedEnd = !rangeTooltipSetting.Format.Contains("{end}", StringComparison.InvariantCulture) ? Intl.GetNumericFormat(endValue, rangeTooltipSetting.Format) : endValue.ToString();
#pragma warning restore CA1305
                        formattedStart = Parent.EnableGroupingSeparator ? formattedStart : formattedStart.Replace(",", string.Empty, StringComparison.InvariantCulture);
                        formattedEnd = Parent.EnableGroupingSeparator ? formattedEnd : formattedEnd.Replace(",", string.Empty, StringComparison.InvariantCulture);
                        TooltipSetting.RangeTooltipStart = "Start: " + formattedStart + "<br>End: " + formattedEnd;
                        string formattedString = rangeTooltipSetting.Format.Replace("{start}", formattedStart, StringComparison.InvariantCulture).Replace("{end}", formattedEnd, StringComparison.InvariantCulture);
                        TooltipSetting.RangeTooltipStart = rangeTooltipSetting.Format.IndexOf("{start}", StringComparison.InvariantCulture) != -1 || rangeTooltipSetting.Format.IndexOf("{end}", StringComparison.InvariantCulture) != -1 ? formattedString : TooltipSetting.RangeTooltipStart;
                    }
#pragma warning disable CA1031
                    catch
#pragma warning restore CA1031
                    {
                    }

                    TooltipSetting.RangeTooltipFill = Parent.Tooltip.RangeSettings != null && !string.IsNullOrEmpty(Parent.Tooltip.RangeSettings.Fill) ? Parent.Tooltip.RangeSettings.Fill : Parent.ThemeStyles.TooltipFillColor;
                    TooltipSetting.RangeTooltipStroke = Parent.Tooltip.RangeSettings != null && Parent.Tooltip.RangeSettings.Border != null ?
                        Parent.Tooltip.RangeSettings.Border.Color : "transparent";
                    TooltipSetting.RangeTooltipStrokeWidth = Parent.Tooltip.RangeSettings != null && Parent.Tooltip.RangeSettings.Border != null ?
                        Parent.Tooltip.RangeSettings.Border.Width : 0;
                    TooltipSetting.RangeFontColor = isRangeTextStyle && !string.IsNullOrEmpty(textStyle.Color) ? textStyle.Color : Parent.ThemeStyles.TooltipFontColor;
                    TooltipSetting.RangeFontFamily = isRangeTextStyle && !string.IsNullOrEmpty(textStyle.FontFamily) ? textStyle.FontFamily : Parent.ThemeStyles.FontFamily;
                    TooltipSetting.RangeFontStyle = isRangeTextStyle && !string.IsNullOrEmpty(textStyle.FontStyle) ? textStyle.FontStyle : string.Empty;
                    TooltipSetting.RangeFontWeight = isRangeTextStyle && !string.IsNullOrEmpty(textStyle.FontWeight) ? textStyle.FontWeight : string.Empty;
                    TooltipSetting.RangeFontOpacity = isRangeTextStyle ? textStyle.Opacity : 1;
                }
                else
                {
                    double rangeFontSize = TooltipSetting.RangeFontSize.IndexOf("px", StringComparison.InvariantCulture) > 0 ? float.Parse(TooltipSetting.RangeFontSize.Replace("px", string.Empty, StringComparison.InvariantCulture), culture) : float.Parse(TooltipSetting.RangeFontSize, culture);
                    textSize = AxisRenderer.MeasureText(TooltipSetting.RangeTooltipStart, rangeFontSize);
                }

                location = AxisRenderer.GetLocationFromAngle(angle, Parent.AxisRenderer.AxisCollection[axisIndex].CurrentRadius, Parent.AxisRenderer.AxisCollection[axisIndex].MidPoint);
                if (Parent.Tooltip.RangeSettings != null && Parent.Tooltip.RangeSettings.Template != null)
                {
                    location.X = (float)GetTemplateXLocation(angle, location.X);
                }

                args = CreateTooltipRenderEventArguments(false, false, TooltipSetting.RangeTooltipStart, "Range", new PointF() { X = location.X, Y = location.Y });
            }
            else if (isAnnotation)
            {
                TooltipSetting.IsAnnotation = true;
                angle = axis.Annotations[index].Angle - 90;
                location = AxisRenderer.GetLocationFromAngle(angle, SfCircularGauge.StringToNumber(Parent.Axes[axisIndex].Annotations[index].Radius, Parent.AxisRenderer.AxisCollection[axisIndex].CurrentRadius), Parent.AxisRenderer.AxisCollection[axisIndex].MidPoint);
                if (Parent.Tooltip.AnnotationSettings != null && !string.IsNullOrEmpty(Parent.Tooltip.AnnotationSettings.Format))
                {
#pragma warning disable CA1508
                    TooltipSetting.AnnotationTooltipFormat = Parent.Tooltip.AnnotationSettings != null ? Parent.Tooltip.AnnotationSettings.Format : string.Empty;
                    TooltipSetting.AnnotationTooltipFill = Parent.Tooltip.AnnotationSettings != null && Parent.Tooltip.AnnotationSettings.Fill != null ? Parent.Tooltip.AnnotationSettings.Fill : Parent.ThemeStyles.TooltipFillColor;
                    TooltipSetting.AnnotationTooltipStroke = Parent.Tooltip.AnnotationSettings != null && Parent.Tooltip.AnnotationSettings.Border != null ?
                        Parent.Tooltip.AnnotationSettings.Border.Color : "transparent";
                    TooltipSetting.AnnotationTooltipStrokeWidth = Parent.Tooltip.AnnotationSettings != null && Parent.Tooltip.AnnotationSettings.Border != null ?
                        Parent.Tooltip.AnnotationSettings.Border.Width : 0;
                    bool isAnnotTextStyle = Parent.Tooltip.AnnotationSettings != null && Parent.Tooltip.AnnotationSettings.TextStyle != null;
#pragma warning restore CA1508
                    CircularGaugeAnnotationTooltipTextStyle textStyle = isAnnotTextStyle ? Parent.Tooltip.AnnotationSettings.TextStyle : null;
                    TooltipSetting.AnnotationFontColor = isAnnotTextStyle && !string.IsNullOrEmpty(textStyle.Color) ? textStyle.Color : Parent.ThemeStyles.TooltipFontColor;
                    TooltipSetting.AnnotationFontFamily = isAnnotTextStyle && !string.IsNullOrEmpty(textStyle.FontFamily) ? textStyle.FontFamily : Parent.ThemeStyles.FontFamily;
                    TooltipSetting.AnnotationFontSize = isAnnotTextStyle && !string.IsNullOrEmpty(textStyle.Size) ? textStyle.Size : "13px";
                    TooltipSetting.AnnotationFontStyle = isAnnotTextStyle && !string.IsNullOrEmpty(textStyle.FontStyle) ? textStyle.FontStyle : string.Empty;
                    TooltipSetting.AnnotationFontWeight = isAnnotTextStyle && !string.IsNullOrEmpty(textStyle.FontWeight) ? textStyle.FontWeight : string.Empty;
                    TooltipSetting.AnnotationFontOpacity = isAnnotTextStyle ? textStyle.Opacity : 1;
                }
                else if (Parent.Tooltip.AnnotationSettings != null && Parent.Tooltip.AnnotationSettings.Template != null)
                {
                    location.X = (float)GetTemplateXLocation(angle, location.X);
                }
                else
                {
                    return null;
                }

                args = CreateTooltipRenderEventArguments(false, false, string.Empty, "Annotation", new PointF() { X = location.X, Y = (float)location.Y });
            }

            if ((Parent.Tooltip.ShowAtMousePosition && isPointer) || (Parent.Tooltip.RangeSettings != null && Parent.Tooltip.RangeSettings.ShowAtMousePosition && isRange))
            {
                TooltipSetting.Location = mouseLocation;
                TooltipSetting.TooltipRect = new Rect() { X = rectBounds.X, Y = rectBounds.Y, Width = rectBounds.Width, Height = rectBounds.Height };
            }
            else
            {
                FindPosition(rectBounds, angle, location);
                TooltipSetting.Location = location;
            }

#pragma warning disable CS0618
            if ((Parent.Tooltip.TooltipTemplate != null && isPointer) || (Parent.Tooltip.RangeSettings != null && Parent.Tooltip.RangeSettings.Template != null && isRange)
#pragma warning restore CS0618
                || (Parent.Tooltip.AnnotationSettings != null && Parent.Tooltip.AnnotationSettings.Template != null && isAnnotation))
            {
                Rect bounds = GetTooltipLocation(TooltipSetting.TooltipRect, location, textSize);
                TooltipSetting.Location.X = (float)bounds.X;
                TooltipSetting.Location.Y = (float)bounds.Y;
            }

            TooltipSetting.TooltipColor = !string.IsNullOrEmpty(Parent.Tooltip.Fill) ? Parent.Tooltip.Fill : Parent.ThemeStyles.TooltipFillColor;
            if (Parent.Tooltip.Border != null)
            {
#pragma warning disable CA1508
                TooltipSetting.TooltipStroke = Parent.Tooltip.Border != null && !string.IsNullOrEmpty(Parent.Tooltip.Border.Color) ? Parent.Tooltip.Border.Color : "transparent";
                TooltipSetting.TooltipStrokeWidth = Parent.Tooltip.Border != null && !double.IsNaN(Parent.Tooltip.Border.Width) ? Parent.Tooltip.Border.Width : 0;
#pragma warning restore CA1508
            }

            return args;
        }

        private static double GetTemplateXLocation(double angle, double x)
        {
            if ((angle >= 150 && angle <= 250) || (angle >= 330 && angle <= 360) || (angle >= 0 && angle <= 45))
            {
                x += 10;
            }

            return x;
        }

        private static TooltipRenderEventArgs CreateTooltipRenderEventArguments(bool appendInBody, bool cancel, string content, string type, PointF location)
        {
            TooltipRenderEventArgs args = new TooltipRenderEventArgs()
            {
                AppendInBodyTag = appendInBody,
                Cancel = cancel,
                Content = content,
                Type = type,
                Location = location,
            };
            return args;
        }

        private BoundingClientRect FindPosition(BoundingClientRect rect, double angle, PointF location)
        {
            double addLeft = 0;
            double addTop = 0;
            double addWidth = 0;
            double addHeight = 0;

            if (angle >= 0 && angle < 45)
            {
                TooltipSetting.ArrowInverted = true;
                addLeft = (angle >= 15 && angle <= 30) ? location.Y : 0;
                TooltipSetting.TooltipRect = new Rect() { X = rect.X, Y = rect.Y + addTop, Width = rect.Width, Height = rect.Height };
            }
            else if ((angle >= 45 && angle < 90) || (angle >= 90 && angle < 135))
            {
                TooltipSetting.ArrowInverted = false;
                TooltipSetting.TooltipRect = new Rect() { X = rect.X, Y = rect.Y + location.Y, Width = rect.Width, Height = rect.Height };
            }
            else if (angle >= 135 && angle < 180)
            {
                TooltipSetting.ArrowInverted = true;
                addTop = angle >= 150 && angle <= 160 ? location.Y : 0;
                TooltipSetting.TooltipRect = new Rect() { X = rect.X - rect.Width, Y = rect.Y + addTop, Width = rect.Width, Height = rect.Height };
            }
            else if (angle >= 180 && angle < 225)
            {
                TooltipSetting.ArrowInverted = true;
                addHeight = (angle >= 200 && angle <= 225) ? Math.Abs(rect.Y - location.Y) : rect.Height;
                TooltipSetting.TooltipRect = new Rect() { X = rect.X - rect.Width, Y = rect.Y, Width = rect.Width, Height = addHeight };
            }
            else if (angle >= 225 && angle < 270)
            {
                TooltipSetting.ArrowInverted = false;
                addWidth = (angle >= 250 && angle <= 290) ? rect.Width : Math.Abs(rect.X - location.X);
                TooltipSetting.TooltipRect = new Rect() { X = rect.X, Y = rect.Y, Width = addWidth, Height = rect.Height };
            }
            else if (angle >= 270 && angle < 315)
            {
                TooltipSetting.ArrowInverted = false;
                addLeft = (angle >= 270 && angle > 290) ? location.X : 0;
                TooltipSetting.TooltipRect = new Rect() { X = rect.X + addLeft, Y = rect.Y, Width = rect.Width, Height = rect.Height };
            }
            else if (angle >= 315 && angle <= 360)
            {
                TooltipSetting.ArrowInverted = true;
                addHeight = (angle >= 315 && angle <= 340) ? Math.Abs(rect.Y - location.Y) : rect.Height;
                TooltipSetting.TooltipRect = new Rect() { X = rect.X, Y = rect.Y, Width = rect.Width, Height = addHeight };
            }

            return new BoundingClientRect()
            {
                X = TooltipSetting.TooltipRect.X,
                Y = TooltipSetting.TooltipRect.Y,
                Width = TooltipSetting.TooltipRect.Width,
                Height = TooltipSetting.TooltipRect.Height,
                Left = TooltipSetting.TooltipRect.X,
                Top = TooltipSetting.TooltipRect.Y,
            };
        }

        private Rect GetTooltipLocation(Rect bounds, PointF symbolLocation, SizeD elementSize)
        {
            PointF location = new PointF() { X = symbolLocation.X, Y = symbolLocation.Y };
            double width = elementSize.Width + 10;
            double height = elementSize.Height + 10;
            double boundsX = bounds.X;
            double boundsY = bounds.Y;
            if (!TooltipSetting.ArrowInverted)
            {
                location = new PointF() { X = (float)(location.X - (elementSize.Width / 2)), Y = (float)(location.Y - elementSize.Height - ARROWPADDING) };
                if (location.Y < boundsY)
                {
                    location.Y = (float)(symbolLocation.Y < 0 ? 0 : symbolLocation.Y);
                }

                if (location.Y + height + ARROWPADDING > boundsY + bounds.Height)
                {
                    location.Y = (float)(symbolLocation.Y > bounds.Height ? bounds.Height : symbolLocation.Y - elementSize.Height - ARROWPADDING);
                }

                if (location.X < boundsX)
                {
                    location.X = (float)boundsX;
                }

                if (location.X + width > boundsX + bounds.Width)
                {
                    location.X -= (float)(location.X + width - (boundsX + bounds.Width));
                }
            }
            else
            {
                location = new PointF() { X = location.X, Y = (float)(location.Y - (elementSize.Height / 2)) };
                if (location.X + width + ARROWPADDING > boundsX + bounds.Width)
                {
                    location.X = (float)((symbolLocation.X > bounds.Width ? bounds.Width : symbolLocation.X) - (width + ARROWPADDING));
                }

                if (location.X < boundsX)
                {
                    location.X = (float)(symbolLocation.X < 0 ? 0 : symbolLocation.X);
                }

                if (location.Y <= boundsY)
                {
                    location.Y = (float)boundsY;
                }

                if (location.Y + height >= boundsY + bounds.Height)
                {
                    location.Y -= (float)(location.Y + height - (boundsY + bounds.Height));
                }
            }

            return new Rect() { X = location.X, Y = location.Y, Width = width, Height = height };
        }
    }
}

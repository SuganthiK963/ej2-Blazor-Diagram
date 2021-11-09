using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Specifies the properties and methods to render the tooltip in SVG element.
    /// </summary>
    public partial class SvgTooltip
    {
        private const double BASEFONT = 100;
        private const string SPACE = " ";
        private float arrowPadding = 12;
        private int padding = 5;
        private TooltipPath path;
        private TooltipTextSetting textSetting;
        private List<TextSpan> textCollection;
        private SvgProperties svgProperties;
        private SizeF elementSize;
        private double tipRadius = 1;
        private double elementLeft;
        private double elementTop;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Gets or sets the id of the tooltip.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the size of the tooltip text.
        /// </summary>
        [Parameter]
        public string TextSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the tooltip text.
        /// </summary>
        [Parameter]
        public string FontColor { get; set; }

        /// <summary>
        /// Gets or sets the font weight of the tooltip text.
        /// </summary>
        [Parameter]
        public string FontWeight { get; set; } = "Normal";

        /// <summary>
        /// Gets or sets the font family of the tooltip text.
        /// </summary>
        [Parameter]
        public string FontFamily { get; set; } = "Segoe UI";

        /// <summary>
        /// Gets or sets the font style of the tooltip text.
        /// </summary>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        /// <summary>
        /// Gets or sets the font opacity of the tooltip text.
        /// </summary>
        [Parameter]
        public double FontOpacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the color of the border to the tooltip.
        /// </summary>
        [Parameter]
        public string BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the width of the border to the tooltip.
        /// </summary>
        [Parameter]
        public double BorderWidth { get; set; }

        /// <summary>
        /// Gets or sets the x value of the tooltip.
        /// </summary>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y value of the tooltip.
        /// </summary>
        [Parameter]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the height of the tooltip.
        /// </summary>
        [Parameter]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the tooltip.
        /// </summary>
        [Parameter]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the show the shadow.
        /// </summary>
        [Parameter]
        public bool EnableShadow { get; set; } = true;

        /// <summary>
        /// Gets or sets the fill color of the tooltip.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Gets or sets opacity of the tooltip.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the content of the tooltip.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the location x of the tooltip.
        /// </summary>
        [Parameter]
        public double LocationX { get; set; }

        /// <summary>
        /// Gets or sets the location y of the tooltip.
        /// </summary>
        [Parameter]
        public double LocationY { get; set; }

        /// <summary>
        /// Gets or sets the rx value of the tooltip.
        /// </summary>
        [Parameter]
        public double RX { get; set; } = 2;

        /// <summary>
        /// Gets or sets the ry value of the tooltip.
        /// </summary>
        [Parameter]
        public double RY { get; set; } = 2;

        /// <summary>
        /// Gets or sets the marginx value of the tooltip.
        /// </summary>
        [Parameter]
        public double MarginX { get; set; } = 5;

        /// <summary>
        /// Gets or sets the marginy value of the tooltip.
        /// </summary>
        [Parameter]
        public double MarginY { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether or not to render the tooltip from right to left.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show the tooltip is inverted.
        /// </summary>
        [Parameter]
        public bool IsInverted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show the tooltip arrow.
        /// </summary>
        [Parameter]
        public bool RenderArrow { get; set; }

        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        [Parameter]
        public string ControlName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the internet explorer is enabled.
        /// </summary>
        [Parameter]
        public bool IsIE { get; set; }

        /// <summary>
        /// Calculate to measure the size of the width and height of the tooltip rect.
        /// </summary>
        /// <param name="text">represents the text of the tooltip.</param>
        /// <param name="fontSize">represents the size of the tooltip.</param>
        /// <returns>returns the size.</returns>
        internal static SizeF MeasureText(string text, double fontSize)
        {
            SizeF charSize = default(SizeF);
            double width = 0, height = 0;
            for (int i = 0; i < text.Length; i++)
            {
                charSize = GetCharSize(text[i]);
                width += charSize.Width;
                height = Math.Max(charSize.Height, height);
            }

            SizeF size = new SizeF() { Width = (float)((width * fontSize) / BASEFONT), Height = (float)((height * fontSize) / BASEFONT) };
            return size;
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            RenderTooltip();
        }

        private static SizeF GetCharSize(char character)
        {
            double charWidth = BASEFONT * 0.75;
            FontInfo font = new FontInfo();
            font.Chars.TryGetValue(character, out charWidth);
            return new SizeF() { Width = (float)(charWidth * BASEFONT / 16.0), Height = (float)(BASEFONT * 1.3) };
        }

        private void RenderTooltip()
        {
            GetPathValues();
            RenderText();
            RenderTooltipElement();
        }

        private void GetPathValues()
        {
            path = new TooltipPath();
            path.Fill = !string.IsNullOrEmpty(Fill) ? Fill : "transparent";
            path.Opacity = Opacity;
            path.Stroke = !string.IsNullOrEmpty(BorderColor) ? BorderColor : "transparent";
        }

        private void RenderText()
        {
            double fontSize = !string.IsNullOrEmpty(TextSize) ? (TextSize.IndexOf("px", StringComparison.InvariantCulture) > -1 ? double.Parse(TextSize.Replace("px", string.Empty, StringComparison.InvariantCulture), CultureInfo.InvariantCulture) : double.Parse(TextSize, CultureInfo.InvariantCulture)) : 13;
            bool isRow = true;
            bool isColumn = true;
            float spaceWidth = 4;
            textSetting = new TooltipTextSetting();
            textSetting.X = MarginX * 2;
            textSetting.Y = (MarginY * 2) + (padding * 2) + (MarginY == 2 ? 3 : 0);
            textSetting.Anchor = EnableRtl ? "end" : "start";
            double size = 13;
            double dy = (22 / size) * double.Parse(TextSize.Replace("px", string.Empty, StringComparison.InvariantCulture), CultureInfo.InvariantCulture);
            double subWidth;
            string[] lines;
            double width = 0, height = 0;
            this.textCollection = new List<TextSpan>();
            string[] textCollection = Content.Replace("</br>", "<br>", StringComparison.InvariantCulture).Replace("<br/>", "<br>", StringComparison.InvariantCulture).Split("<br>");
            for (int j = 0; j < textCollection.Length; j++)
            {
                subWidth = 0;
                isColumn = true;
                height += dy;
                lines = textCollection[j].Replace(":", "<br>:<br>", StringComparison.InvariantCulture).Split("<br>");
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    TextSpan textSpan = new TextSpan();
                    if ((!isColumn && string.IsNullOrWhiteSpace(line)) || !string.IsNullOrEmpty(line.Trim()))
                    {
                        subWidth += !string.IsNullOrWhiteSpace(line) ? spaceWidth : 0;
                        if (isColumn && !isRow)
                        {
                            textSpan = new TextSpan() { X = MarginX * 2, Text = line, DY = dy };
                        }
                        else if (isRow && isColumn)
                        {
                            textSpan = new TextSpan() { X = MarginX * 2, Text = line };
                        }
                    }

                    if (line.Contains("<b>", StringComparison.InvariantCulture))
                    {
                        line = line.Replace("<b>", string.Empty, StringComparison.InvariantCulture).Replace("</b>", string.Empty, StringComparison.InvariantCulture);
                        textSpan.FontWeight = "bold";
                    }

                    isColumn = false;
                    textSpan.Text = line;
                    this.textCollection.Add(textSpan);
                    subWidth += MeasureText(line, fontSize).Width;
                    isRow = false;
                }

                subWidth -= spaceWidth;
                width = Math.Max(width, subWidth);
            }

            elementSize = new SizeF() { Height = (float)height, Width = (float)(width + (width > 0 ? (2 * MarginX) : 0)) };
        }

        private void RenderTooltipElement()
        {
            bool isTop = false, isLeft = false, isBottom = false;
            float x = 0, y = 0;
            PointF arrowLocation = PointF.Empty;
            PointF tipLocation = PointF.Empty;
            RectangleF rect = GetTooltipLocation(out arrowLocation, out tipLocation);
            if (!IsInverted)
            {
                isTop = rect.Y < LocationY;
                isBottom = !isTop;
                y = isTop ? 0 : arrowPadding;
            }
            else
            {
                isLeft = rect.X < LocationX;
                x = isLeft ? 0 : arrowPadding;
            }

            double start = BorderWidth / 2;
            RectangleF pointRect = new RectangleF() { X = (float)(start + x), Y = (float)(start + y), Width = (float)(rect.Width - start), Height = (float)(rect.Height - start) };
            elementLeft = rect.X;
            elementTop = rect.Y;
            svgProperties = new SvgProperties();
            svgProperties.Width = rect.Width + BorderWidth + (!IsInverted ? 0 : arrowPadding) + 5;
            svgProperties.Height = rect.Height + BorderWidth + (!(!IsInverted) ? 0 : arrowPadding) + 5;
            path.Direction = FindDirection(pointRect, isTop, isBottom, isLeft, arrowLocation, tipLocation, tipRadius);
            textSetting.Transform = ChangeText(isBottom, !isLeft && !isTop && !isBottom);
            if (!RenderArrow)
            {
                if (isBottom)
                {
                    elementTop += 4;
                }
                else if (isTop)
                {
                    elementTop += 8;
                }
            }
        }

        private string FindDirection(RectangleF rect, bool isTop, bool isBottom, bool isLeft, PointF arrowLocation, PointF tipLocation, double tipRadius)
        {
            string direction = string.Empty;
            double startX = rect.X;
            double startY = rect.Y;
            double width = rect.X + rect.Width;
            double height = rect.Y + rect.Height;
            tipRadius = tipRadius != 0 ? tipRadius : 0;
            if (isTop)
            {
                direction += "M " + startX.ToString(culture) + SPACE + (startY + RY).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (startX + RX).ToString(culture) + SPACE + startY.ToString(culture) + SPACE +
                    " L " + SPACE + (width - RX).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (startY + RY).ToString(culture);
                direction += " L " + width.ToString(culture) + SPACE + (height - RY).ToString(culture) + " Q " + width.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (width - RX).ToString(culture) + SPACE + height.ToString(culture);
                direction += " L " + (arrowLocation.X + (arrowPadding / 2)).ToString(culture) + SPACE + height.ToString(culture);
                if (RenderArrow)
                {
                    direction += " L" + SPACE + (tipLocation.X + tipRadius).ToString(culture) + SPACE + (height + arrowPadding - tipRadius).ToString(culture);
                    direction += " Q " + tipLocation.X.ToString(culture) + SPACE + (height + arrowPadding).ToString(culture) + SPACE + (tipLocation.X - tipRadius).ToString(culture) + SPACE + (height + arrowPadding - tipRadius).ToString(culture);
                }

                if ((arrowLocation.X - (arrowPadding / 2)) > startX)
                {
                    direction += " L " + (arrowLocation.X - (arrowPadding / 2)).ToString(culture) + SPACE + height.ToString(culture) + " L" + SPACE + (startX + RX).ToString(culture) + SPACE + height.ToString(culture) + " Q " + startX.ToString(culture) + SPACE +
                        height.ToString(culture) + SPACE + startX.ToString(culture) + SPACE + (height - RY).ToString(culture) + " z";
                }
                else
                {
                    double pathHeight = RenderArrow ? height + RY : height + RY - 2;
                    direction += " L" + SPACE + startX.ToString(culture) + SPACE + pathHeight.ToString(culture) + " z";
                }
            }
            else if (isBottom)
            {
                direction += "M " + startX.ToString(culture) + SPACE + (startY + RY).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (startX + RX).ToString(culture) + SPACE + startY.ToString(culture);
                if (RenderArrow)
                {
                    direction += " L " + (arrowLocation.X - (arrowPadding / 2)).ToString(culture) + SPACE + startY.ToString(culture);
                    direction += " L " + (tipLocation.X - tipRadius).ToString(culture) + SPACE + (arrowLocation.Y + tipRadius).ToString(culture);
                    direction += " Q " + tipLocation.X.ToString(culture) + SPACE + arrowLocation.Y.ToString(culture) + SPACE + (tipLocation.X + tipRadius).ToString(culture) + SPACE + (arrowLocation.Y + tipRadius).ToString(culture);
                }

                direction += " L " + (arrowLocation.X + (arrowPadding / 2)).ToString(culture) + SPACE + startY.ToString(culture) + " L" + SPACE
                    + (width - RX).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (startY + RY).ToString(culture);
                direction += " L " + width.ToString(culture) + SPACE + (height - RY).ToString(culture) + " Q " + width.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (width - RX).ToString(culture) + SPACE + height.ToString(culture) +
                    " L " + (startX + RX).ToString(culture) + SPACE + height.ToString(culture) + " Q " + startX.ToString(culture) + SPACE + height.ToString(culture) + SPACE + startX.ToString(culture) + SPACE + (height - RY).ToString(culture) + " z";
            }
            else if (isLeft)
            {
                direction += "M " + startX.ToString(culture) + SPACE + (startY + RY).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (startX + RX).ToString(culture) + SPACE + startY.ToString(culture);
                direction += " L" + SPACE + (width - RX).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (startY + RY).ToString(culture) + " L" + SPACE + width.ToString(culture) + SPACE + (arrowLocation.Y - (arrowPadding / 2)).ToString(culture);
                direction += " L " + (width + arrowPadding - tipRadius).ToString(culture) + SPACE + (tipLocation.Y - tipRadius).ToString(culture);
                direction += " Q " + (width + arrowPadding).ToString(culture) + SPACE + tipLocation.Y.ToString(culture) + SPACE + (width + arrowPadding - tipRadius).ToString(culture) + SPACE + (tipLocation.Y + tipRadius).ToString(culture);
                direction += " L " + width.ToString(culture) + SPACE + (arrowLocation.Y + (arrowPadding / 2)).ToString(culture) + " L " + width.ToString(culture) + SPACE + (height - RY).ToString(culture) + " Q " + width.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (width - RX).ToString(culture) + SPACE + height.ToString(culture);
                direction += " L " + (startX + RX).ToString(culture) + SPACE + height.ToString(culture) + " Q " + startX.ToString(culture) + SPACE + height.ToString(culture) + SPACE + startX.ToString(culture) + SPACE + (height - RY).ToString(culture) + " z";
            }
            else
            {
                direction += "M " + (startX + RX).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + startX.ToString(culture) + SPACE
                    + startY.ToString(culture) + SPACE + startX.ToString(culture) + SPACE + (startY + RY).ToString(culture) + " L" + SPACE + startX.ToString(culture) + SPACE + (arrowLocation.Y - (arrowPadding / 2)).ToString(culture);
                direction += " L " + (startX - arrowPadding + tipRadius).ToString(culture) + SPACE + (tipLocation.Y - tipRadius).ToString(culture);
                direction += " Q " + (startX - arrowPadding).ToString(culture) + SPACE + tipLocation.Y.ToString(culture) + SPACE + (startX - arrowPadding + tipRadius).ToString(culture) + SPACE + (tipLocation.Y + tipRadius).ToString(culture);
                direction += " L " + startX.ToString(culture) + SPACE + (arrowLocation.Y + (arrowPadding / 2)).ToString(culture) +
                    " L" + SPACE + startX.ToString(culture) + SPACE + (height - RY).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (startX + RX).ToString(culture) + SPACE + height.ToString(culture);
                direction += " L " + (width - RX).ToString(culture) + SPACE + height.ToString(culture) + " Q " + width.ToString(culture) + SPACE + height.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (height - RY).ToString(culture) +
                    " L " + width.ToString(culture) + SPACE + (startY + RY).ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (width - RX).ToString(culture) + SPACE + startY.ToString(culture) + " z";
            }

            return direction;
        }

        private RectangleF GetTooltipLocation(out PointF arrowLocation, out PointF tipLocation)
        {
            arrowLocation = new PointF(0, 0);
            tipLocation = new PointF(0, 0);
            PointF location = new PointF() { X = (float)LocationX, Y = (float)LocationY };
            double width = elementSize.Width + (2 * MarginX);
            double height = elementSize.Height + (2 * MarginY);
            double boundX = X;
            double boundY = Y;
            if (!IsInverted)
            {
                location = new PointF() { X = location.X - (elementSize.Width / 2) - padding, Y = location.Y - elementSize.Height - (2 * padding) - arrowPadding };
                arrowLocation.X = tipLocation.X = (float)(width / 2);
                if (location.Y < boundY)
                {
                    location.Y = (float)(LocationY < 0 ? 0 : LocationY);
                }

                if (location.Y + height + arrowPadding > boundY + Height)
                {
                    location.Y = (float)((LocationY > Height ? Height : LocationY) - elementSize.Height - (2 * padding) - arrowPadding);
                }

                tipLocation.X = (float)width / 2;
                if (location.X < boundX)
                {
                    arrowLocation.X -= (float)(boundX - location.X);
                    tipLocation.X -= (float)(boundX - location.X);
                    location.X = (float)boundX;
                }

                if (location.X + width > boundX + Width)
                {
                    arrowLocation.X += (float)((location.X + width) - (boundX + Width));
                    tipLocation.X += (float)((location.X + width) - (boundX + Width));
                    location.X -= (float)((location.X + width) - (boundX + Width));
                }

                if (arrowLocation.X + (arrowPadding / 2) > width - RX)
                {
                    arrowLocation.X = (float)(width - RX - (arrowPadding / 2));
                    tipLocation.X = (float)width;
                    tipRadius = 0;
                }

                if (arrowLocation.X - (arrowPadding / 2) < RX)
                {
                    arrowLocation.X = (float)(RX + (arrowPadding / 2));
                    tipLocation.X = 0;
                    tipRadius = 0;
                }
            }
            else
            {
                location = new PointF() { X = location.X, Y = location.Y - (elementSize.Height / 2) - padding };
                arrowLocation.Y = tipLocation.Y = (float)(height / 2);
                if (location.X + width + arrowPadding > boundX + Width)
                {
                    location.X = (float)((LocationX > Width ? Width : LocationX) - (width + arrowPadding));
                }

                if (location.X < boundX)
                {
                    location.X = (float)(LocationX < 0 ? 0 : LocationX);
                }

                if (location.Y <= boundY)
                {
                    arrowLocation.Y -= (float)(boundY - location.Y);
                    location.Y -= (float)(boundY - location.Y);
                    location.Y = (float)boundY;
                }

                if (location.Y + height >= boundY + Height)
                {
                    arrowLocation.Y += (float)((location.Y + height) - (boundY + Height));
                    tipLocation.Y += (float)((location.Y + height) - (boundY + Height));
                    location.Y -= (float)((location.Y + height) - (boundY + Height));
                }

                if (arrowLocation.Y + (arrowPadding / 2) > height - RY)
                {
                    arrowLocation.Y = (float)(height - RY - (arrowPadding / 2));
                    tipLocation.Y = (float)height;
                    tipRadius = 0;
                }

                if (arrowLocation.Y - (arrowPadding / 2) < RY)
                {
                    arrowLocation.Y = (float)(RY + (arrowPadding / 2));
                    tipLocation.Y = 0;
                    tipRadius = 0;
                }
            }

            return new RectangleF() { X = ControlName == "LinearGauge" ? (float)X : location.X, Y = ControlName == "LinearGauge" ? (float)Y : location.Y, Width = (float)width, Height = (float)height };
        }

        private string ChangeText(bool isBottom, bool isRight)
        {
            string transform = string.Empty;
            if (isBottom)
            {
                transform = "translate(0," + arrowPadding.ToString(culture) + ")";
            }
            else if (isRight)
            {
                transform = "translate(" + arrowPadding.ToString(culture) + ")";
            }

            return transform;
        }
    }

    /// <summary>
    /// Represents to render the svg properties of the legend.
    /// </summary>
    public class SvgProperties
    {
        /// <summary>
        /// Gets or sets the value of height.
        /// </summary>
        internal double Height { get; set; }

        /// <summary>
        /// Gets or sets the value of width.
        /// </summary>
        internal double Width { get; set; }
    }

    /// <summary>
    /// Represents to render the path of the tooltip.
    /// </summary>
    public class TooltipPath
    {
        /// <summary>
        /// Gets or sets the stroke width of the tooltip.
        /// </summary>
        internal double StokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the fill color of the tooltip.
        /// </summary>
        internal string Fill { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the tooltip.
        /// </summary>
        internal double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the stroke of the tooltip.
        /// </summary>
        internal string Stroke { get; set; }

        /// <summary>
        /// Gets or sets the direction of the tooltip.
        /// </summary>
        internal string Direction { get; set; }
    }

    /// <summary>
    /// Represents to render the tooltip text.
    /// </summary>
    public class TooltipTextSetting
    {
        /// <summary>
        /// Gets or sets the x position of the tooltip.
        /// </summary>
        internal double X { get; set; }

        /// <summary>
        /// Gets or sets the y position of the tooltip.
        /// </summary>
        internal double Y { get; set; }

        /// <summary>
        /// Gets or sets the anchor position of the tooltip text.
        /// </summary>
        internal string Anchor { get; set; }

        /// <summary>
        /// Gets or sets the transform position of the tooltip text.
        /// </summary>
        internal string Transform { get; set; }
    }

    /// <summary>
    /// Represents to render the text of the tooltip.
    /// </summary>
    public class TextSpan
    {
        /// <summary>
        /// Gets or sets the x position of the tooltip.
        /// </summary>
        internal double X { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the Y position of the tooltip.
        /// </summary>
        internal double DY { get; set; } = double.NaN;

        /// <summary>
        /// Gets or sets the properties of tooltip text.
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        /// Gets or sets the font weight of the tooltip text.
        /// </summary>
        internal string FontWeight { get; set; } = string.Empty;
    }
}
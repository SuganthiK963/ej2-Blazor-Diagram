using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    /// <summary>
    /// Specifies the X and Y axis of point.
    /// </summary>
    public class Point
    {
        internal Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        public Point()
        {
        }

        /// <summary>
        /// Specifies the X position.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Specifies the X position.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// Specifies the height and width of element.
    /// </summary>
    public class Size
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> class.
        /// </summary>
        /// <param name="width">Represents the width of element.</param>
        /// <param name="height">Represents the height of element.</param>
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> class.
        /// </summary>
        public Size()
        {
        }

        /// <summary>
        ///  Specifies the width of element.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        ///  Specifies the height of element.
        /// </summary>
        public double Height { get; set; }
    }

    /// <summary>
    /// Specifies the margin of element.
    /// </summary>
    public class DomRect : Size
    {
        /// <summary>
        /// Specifies the left margin of element.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Specifies the top margin of element.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Specifies the right margin of element.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// Specifies the bottom margin of element.
        /// </summary>
        public double Bottom { get; set; }
    }

    internal class ClosestPoint : Point
    {
        internal Point Location { get; set; }

        internal double Index { get; set; }
    }

    internal class LineSegment
    {
        internal double X1 { get; set; }

        internal double X2 { get; set; }

        internal double Y1 { get; set; }

        internal double Y2 { get; set; }
    }

    /// <summary>
    /// Specifies the data point of smith chart.
    /// </summary>
    public class SmithChartPointData
    {
        internal SmithChartPointData(SmithChartPoint point, SmithChartSeries series, double lierIndex = 0)
        {
            Point = point;
            Series = series;
            LierIndex = lierIndex;
        }

        internal SmithChartPointData()
        {
        }

        /// <summary>
        /// Specifies the point of series.
        /// </summary>
        public SmithChartPoint Point { get; set; }

        /// <summary>
        /// Specifies the series.
        /// </summary>
        public SmithChartSeries Series { get; set; }

        /// <summary>
        /// Specifies the index.
        /// </summary>
        public double LierIndex { get; set; }
    }

    internal class PointRegion : Point
    {
        internal Point Point { get; set; }
    }

    internal class LabelCollection
    {
        internal double CenterX { get; set; }

        internal double CenterY { get; set; }

        internal double Radius { get; set; }

        internal double Value { get; set; }
    }

    internal class LabelRegion
    {
        internal Rect Bounds { get; set; }

        internal string LabelText { get; set; }
    }

    internal class HorizontalLabelCollection : LabelCollection
    {
        internal LabelRegion Region { get; set; }
    }

    internal class RadialLabelCollection : HorizontalLabelCollection
    {
        internal double Angle { get; set; }
    }

    internal class Direction
    {
        internal double CounterClockwise { get; set; }

        internal double Clockwise { get; set; } = 1;
    }

    internal class GridArcPoints
    {
        internal Point StartPoint { get; set; }

        internal Point EndPoint { get; set; }

        internal double RotationAngle { get; set; } = double.NaN;

        internal double SweepDirection { get; set; } = double.NaN;

        internal bool IsLargeArc { get; set; }

        internal Size Size { get; set; }
    }

    internal class DataLabelTextOptions : Point
    {
        internal string Id { get; set; }

        internal string Text { get; set; }

        internal string Fill { get; set; }

        internal SmithChartDataLabelTextStyle Font { get; set; }

        internal double XPosition { get; set; }

        internal double YPosition { get; set; }

        internal double Width { get; set; }

        internal double Height { get; set; }

        internal Point Location { get; set; }

        internal SmithChartLabelPosition LabelOptions { get; set; }

        internal bool Visible { get; set; }

        internal bool ConnectorFlag { get; set; }
    }

    internal class SmithChartLabelPosition : Point
    {
        internal double TextX { get; set; }

        internal double TextY { get; set; }
    }

    internal class LabelOption
    {
        internal List<DataLabelTextOptions> TextOptions { get; set; }
    }

    internal class SmithChartThemeStyle
    {
        internal SmithChartThemeStyle(string axisLabel, string axisLine, string majorGridLine, string minorGridLine, string chartTitle, string legendLabel, string background, string areaBorder, string tooltipFill, string dataLabel, string tooltipBoldLabel, string tooltipLightLabel, string tooltipHeaderLine)
        {
            AxisLabel = axisLabel;
            AxisLine = axisLine;
            MajorGridLine = majorGridLine;
            MinorGridLine = minorGridLine;
            ChartTitle = chartTitle;
            LegendLabel = legendLabel;
            Background = background;
            AreaBorder = areaBorder;
            TooltipFill = tooltipFill;
            DataLabel = dataLabel;
            TooltipBoldLabel = tooltipBoldLabel;
            TooltipLightLabel = tooltipLightLabel;
            TooltipHeaderLine = tooltipHeaderLine;
        }

        internal string AxisLabel { get; set; }

        internal string AxisLine { get; set; }

        internal string MajorGridLine { get; set; }

        internal string MinorGridLine { get; set; }

        internal string ChartTitle { get; set; }

        internal string LegendLabel { get; set; }

        internal string Background { get; set; }

        internal string AreaBorder { get; set; }

        internal string TooltipFill { get; set; }

        internal string DataLabel { get; set; }

        internal string TooltipBoldLabel { get; set; }

        internal string TooltipLightLabel { get; set; }

        internal string TooltipHeaderLine { get; set; }

        internal string FontFamily { get; set; }

        internal string FontSize { get; set; }

        internal string LabelFontFamily { get; set; }

        internal double TooltipFillOpacity { get; set; } = double.NaN;

        internal double TooltipTextOpacity { get; set; }
    }

    internal class LegendSeries
    {
        internal LegendSeries(string text, string fill, Shape shape, bool visible, string type, double seriesIndex = 0)
        {
            Text = text;
            Fill = fill;
            Shape = shape;
            Visible = visible;
            Type = type;
            SeriesIndex = seriesIndex;
        }

        internal LegendSeries()
        {
        }

        internal bool Render { get; set; }

        internal string Text { get; set; }

        internal string Fill { get; set; }

        internal Shape Shape { get; set; }

        internal bool Visible { get; set; }

        internal string Type { get; set; }

        internal Size Bounds { get; set; }

        internal double SeriesIndex { get; set; }
    }

    /// <summary>
    /// Specifies the mouse information.
    /// </summary>
    public class SmithChartInternalMouseEventArgs
    {
        /// <summary>
        /// Specifies the type of browser.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Specifies the identification of pointer.
        /// </summary>
        public double PointerId { get; set; }

        /// <summary>
        /// Specifies the X position of mouse.
        /// </summary>
        public double MouseX { get; set; }

        /// <summary>
        /// Specifies the Y position of mouse.
        /// </summary>
        public double MouseY { get; set; }

        /// <summary>
        /// Specifies the pointer type.
        /// </summary>
        public string PointerType { get; set; }

        /// <summary>
        /// Specifies the target element.
        /// </summary>
        public string Target { get; set; }
    }

    /// <summary>
    /// Specifies the text style of tooltip content.
    /// </summary>
    public class TooltipTextStyle
    {
        /// <summary>
        /// Specifies the font style.
        /// </summary>
        [JsonPropertyName("fontStyle")]
        public string FontStyle { get; set; }

        /// <summary>
        /// Specifies the font color opactity.
        /// </summary>
        [JsonPropertyName("opacity")]
        public double Opacity { get; set; }

        /// <summary>
        /// Specifies the font color.
        /// </summary>
        [JsonPropertyName("color")]
        public string Color { get; set; }

        /// <summary>
        /// Specifies the font size.
        /// </summary>
        [JsonPropertyName("size")]
        public string Size { get; set; }

        /// <summary>
        /// Specifies the font family.
        /// </summary>
        [JsonPropertyName("fontFamily")]
        public string FontFamily { get; set; }

        /// <summary>
        /// Specifies the font weight.
        /// </summary>
        [JsonPropertyName("fontWeight")]
        public string FontWeight { get; set; }
    }

    /// <summary>
    /// Specifies the color and width of tooltip border.
    /// </summary>
    public class TooltipBorder
    {
        /// <summary>
        /// Specifies the color of border.
        /// </summary>
        [JsonPropertyName("color")]
        public string Color { get; set; }

        /// <summary>
        /// Specifies the width of border.
        /// </summary>
        [JsonPropertyName("width")]
        public double Width { get; set; }
    }

    /// <summary>
    /// Specifies the location of tooltip.
    /// </summary>
    public class TooltipLocation
    {
        /// <summary>
        /// Specifies the X position.
        /// </summary>
        [JsonPropertyName("x")]
        public double X { get; set; }

        /// <summary>
        /// Specifies the Y position.
        /// </summary>
        [JsonPropertyName("y")]
        public double Y { get; set; }
    }

    /// <summary>
    /// Specifies height and width of tooltip element.
    /// </summary>
    public class TooltipAreaBounds : TooltipLocation
    {
        /// <summary>
        /// Specifies the width of tooltip element.
        /// </summary>
        [JsonPropertyName("width")]
        public double Width { get; set; }

        /// <summary>
        /// Specifies the height of tooltip element.
        /// </summary>
        [JsonPropertyName("height")]
        public double Height { get; set; }
    }

    /// <summary>
    /// Specifies the style of tooltip SVG.
    /// </summary>
    public class SvgTooltip
    {
        /// <summary>
        /// Specifies the fill color.
        /// </summary>
        [JsonPropertyName("fill")]
        public string Fill { get; set; }

        /// <summary>
        /// Specifies the header.
        /// </summary>
        [JsonPropertyName("header")]
        public string Header { get; set; }

        /// <summary>
        /// Specifies the opacity.
        /// </summary>
        [JsonPropertyName("opacity")]
        public double Opacity { get; set; }

        /// <summary>
        /// Specifies the style of text.
        /// </summary>
        [JsonPropertyName("textStyle")]
        public TooltipTextStyle TextStyle { get; set; }

        /// <summary>
        /// Specifies the template of tooltip.
        /// </summary>
        [JsonPropertyName("template")]
        public string Template { get; set; }

        /// <summary>
        /// Specifies the duration.
        /// </summary>
        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        /// <summary>
        /// Specifies color and width of tooltip border.
        /// </summary>
        [JsonPropertyName("border")]
        public TooltipBorder Border { get; set; }

        /// <summary>
        /// Specifies content of tooltip.
        /// </summary>
        [JsonPropertyName("content")]
        public string[] Content { get; set; }

        /// <summary>
        /// Specifies the location of tooltip element.
        /// </summary>
        [JsonPropertyName("clipBounds")]
        public TooltipLocation ClipBounds { get; set; }

        /// <summary>
        /// Specifies the palette color.
        /// </summary>
        [JsonPropertyName("palette")]
        public string[] Palette { get; set; }

        /// <summary>
        /// Specifies the shape.
        /// </summary>
        [JsonPropertyName("shapes")]
        public Shape[] Shapes { get; set; }

        /// <summary>
        /// Specifies the location of tooltip.
        /// </summary>
        [JsonPropertyName("location")]
        public TooltipLocation Location { get; set; }

        /// <summary>
        /// Specifies the tooltip arrow padding.
        /// </summary>
        [JsonPropertyName("arrowPadding")]
        public double ArrowPadding { get; set; }

        /// <summary>
        /// Specifies the data of tooltip.
        /// </summary>
        [JsonPropertyName("data")]
        public Point Data { get; set; }

        /// <summary>
        /// Specifies the theme of tooltip.
        /// </summary>
        [JsonPropertyName("theme")]
        public string Theme { get; set; }

        /// <summary>
        /// Specifies the bounds of tooltip.
        /// </summary>
        [JsonPropertyName("areaBounds")]
        public TooltipAreaBounds AreaBounds { get; set; }

        /// <summary>
        /// Specifies the available size of tooltip element.
        /// </summary>
        [JsonPropertyName("availableSize")]
        public Size AvailableSize { get; set; }
    }
}
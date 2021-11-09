namespace Syncfusion.Blazor.Charts.BulletChart.Internal
{
    internal static class Constant
    {
        internal const string TRANSPARENT = "transparent";
        internal const string SPACE = " ";
        internal const string DOT = "...";
    }

    /// <summary>
    /// Represents the height and width of the DOM element.
    /// </summary>
    public class SizeInfo
    {
        /// <summary>
        /// Sets or gets the height of the element.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Sets or gets the width of the element.
        /// </summary>
        public double Width { get; set; }
    }

    internal class Border : ClipModel
    {
        internal string ID { get; set; } = string.Empty;

        internal string BackGround { get; set; } = string.Empty;
    }

    internal class ColorRange : Border
    {
        internal Rect RangeRect { get; set; } = new Rect();

        internal double Opacity { get; set; }
    }

    /// <summary>
    /// Represents the X and Y position of element.
    /// </summary>
    public class Rect : SizeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> class.
        /// </summary>
        public Rect()
        {
        }

        internal Rect(double height, double width, double x, double y)
        {
            Height = height;
            Width = width;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Sets or gets the X position.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Sets or gets the Y position.
        /// </summary>
        public double Y { get; set; }
    }

    internal class Margin
    {
        internal Margin()
        {
        }

        internal Margin(double bottom, double left, double right, double top)
        {
            Bottom = bottom;
            Left = left;
            Right = right;
            Top = top;
        }

        internal double Bottom { get; set; }

        internal double Left { get; set; }

        internal double Right { get; set; }

        internal double Top { get; set; }
    }

    internal class PageInfo
    {
        internal string Color { get; set; } = "#a6a6a6";

        internal string Fill { get; set; } = "#a6a6a6";

        internal Rect LeftPage { get; set; } = new Rect();

        internal string LeftPagePath { get; set; }

        internal double Opacity { get; set; } = 1;

        internal TextStyle PageText { get; set; } = new TextStyle();

        internal string RectFill { get; set; } = Constant.TRANSPARENT;

        internal Rect RightPage { get; set; } = new Rect();

        internal string RightPagePath { get; set; }

        internal Rect TextLocation { get; set; } = new Rect();

        internal string Text { get; set; } = string.Empty;

        internal string Transform { get; set; } = string.Empty;

        internal int LeftTabIndex { get; set; }

        internal int RightTabIndex { get; set; }
    }

    internal class BaseModel : Rect
    {
        internal double X1 { get; set; }

        internal double Y1 { get; set; }

        internal double X2 { get; set; }

        internal double Y2 { get; set; }

        internal double StrokeWidth { get; set; }

        internal string Stroke { get; set; } = string.Empty;
    }

    internal class ClipModel
    {
        internal double StrokeWidth { get; set; }

        internal string Stroke { get; set; } = string.Empty;

        internal Rect Bound { get; set; } = new Rect();
    }

    /// <summary>
    /// Represents the style of the component.
    /// </summary>
    public class Style
    {
        internal Style(string majorTickLineColor, string minorTickLineColor, string background, string labelFontColor, string categoryFontColor, string labelFontFamily, string tooltipFill, string legendLabel, string tooltipFontColor, string featuredMeasureColor, string comparativeMeasureColor, string titleFontColor, string dataLabelFontColor, string titleFontFamily, string subTitleFontColor, string subTitleFontFamily, string[] rangeStrokes)
        {
            MajorTickLineColor = majorTickLineColor;
            MinorTickLineColor = minorTickLineColor;
            Background = background;
            LabelFontColor = labelFontColor;
            CategoryFontColor = categoryFontColor;
            LabelFontFamily = labelFontFamily;
            TooltipFill = tooltipFill;
            LegendLabel = legendLabel;
            TooltipFontColor = tooltipFontColor;
            FeaturedMeasureColor = featuredMeasureColor;
            ComparativeMeasureColor = comparativeMeasureColor;
            TitleFontColor = titleFontColor;
            DataLabelFontColor = dataLabelFontColor;
            TitleFontFamily = titleFontFamily;
            SubTitleFontColor = subTitleFontColor;
            SubTitleFontFamily = subTitleFontFamily;
            RangeStrokes = rangeStrokes;
        }

        internal string MajorTickLineColor { get; set; }

        internal string MinorTickLineColor { get; set; }

        internal string Background { get; set; }

        internal string LabelFontColor { get; set; }

        internal string CategoryFontColor { get; set; }

        internal string LabelFontFamily { get; set; }

        internal string TooltipFill { get; set; }

        internal string LegendLabel { get; set; }

        internal string TooltipFontColor { get; set; }

        internal string FeaturedMeasureColor { get; set; }

        internal string ComparativeMeasureColor { get; set; }

        internal string TitleFontColor { get; set; }

        internal string DataLabelFontColor { get; set; }

        internal string TitleFontFamily { get; set; }

        internal string SubTitleFontColor { get; set; }

        internal string SubTitleFontFamily { get; set; }

        internal string[] RangeStrokes { get; set; }
    }

    internal class TextStyle
    {
        internal string Color { get; set; } = string.Empty;

        internal bool EnableTrim { get; set; } = true;

        internal string FontFamily { get; set; } = string.Empty;

        internal string FontStyle { get; set; } = "Normal";

        internal string FontWeight { get; set; } = "Normal";

        internal double MaximumTitleWidth { get; set; }

        internal double Opacity { get; set; } = 1;

        internal string Size { get; set; } = "12px";

        internal Alignment TextAlignment { get; set; } = Alignment.Center;

        internal TextOverflow TextOverflow { get; set; } = TextOverflow.None;

        internal bool EnableRangeColor { get; set; }
    }

    internal class ChartTitle : Rect
    {
        internal string Anchor { get; set; } = string.Empty;

        internal string Transform { get; set; } = string.Empty;

        internal int TabIndex { get; set; }
    }

    internal class LabelModel : ChartTitle
    {
        internal string Text { get; set; } = string.Empty;

        internal string Color { get; set; } = string.Empty;
    }

    internal class LegendModel : Rect
    {
        internal string Text { get; set; } = string.Empty;

        internal string Fill { get; set; } = string.Empty;

        internal LegendShape Shape { get; set; } = LegendShape.Rectangle;

        internal int Index { get; set; }

        internal bool Render { get; set; }

        internal SizeInfo TextSize { get; set; }

        internal ChartTitle TextInfo { get; set; } = new ChartTitle();

        internal Rect Location { get; set; } = new Rect();

        internal double StrokeWidth { get; set; }

        internal string Path { get; set; } = string.Empty;
    }

    internal class MeasureModel
    {
        internal double PointX { get; set; }

        internal double Width { get; set; }

        internal double LastPointX { get; set; }
    }

    internal class BarInfo : BaseModel
    {
        internal string ID { get; set; } = string.Empty;

        internal string Fill { get; set; } = string.Empty;

        internal Border Border { get; set; }

        internal double Opacity { get; set; } = 1;

        internal string Transform { get; set; } = string.Empty;

        internal string Style { get; set; } = string.Empty;

        internal string Path { get; set; } = string.Empty;
    }
}
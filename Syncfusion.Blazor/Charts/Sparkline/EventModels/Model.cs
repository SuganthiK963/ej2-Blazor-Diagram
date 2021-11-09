namespace Syncfusion.Blazor.Sparkline.Internal
{
    internal class Margin
    {
        internal Margin(double top, double left, double bottom, double right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        internal double Top { get; set; }

        internal double Left { get; set; }

        internal double Right { get; set; }

        internal double Bottom { get; set; }
    }

    internal class Style
    {
        internal Style(string axisLine, string dataLabel, string rangeBand, string tooltipFill, string background, string tooltipFont, string trackerLine, string fontFamily = "", int tooltipFillOpacity = 0, double tooltipTextOpacity = 0, string labelFontFamily = "")
        {
            AxisLine = axisLine;
            DataLabel = dataLabel;
            RangeBand = rangeBand;
            TooltipFill = tooltipFill;
            Background = background;
            TooltipFont = tooltipFont;
            TrackerLine = trackerLine;
            FontFamily = fontFamily;
            TooltipFillOpacity = tooltipFillOpacity;
            TooltipTextOpacity = tooltipTextOpacity;
            LabelFontFamily = labelFontFamily;
        }

        internal string AxisLine { get; set; }

        internal string DataLabel { get; set; }

        internal string RangeBand { get; set; }

        internal string TooltipFill { get; set; }

        internal string Background { get; set; }

        internal string TooltipFont { get; set; }

        internal string TrackerLine { get; set; }

        internal string FontFamily { get; set; }

        internal int TooltipFillOpacity { get; set; }

        internal double TooltipTextOpacity { get; set; }

        internal string LabelFontFamily { get; set; }
    }

    internal class EdgeLabelOption
    {
        internal EdgeLabelOption(double x, bool render)
        {
            X = x;
            Render = render;
        }

        internal double X { get; set; }

        internal bool Render { get; set; }
    }

    /// <summary>
    /// Specifies the color and width of the border.
    /// </summary>
    public class Border
    {
        /// <summary>
        /// Sets or gets the color of border.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Sets or gets the width of border.
        /// </summary>
        public double Width { get; set; }
    }

    /// <summary>
    /// Specifies the size of element.
    /// </summary>
    public class Size : ElementInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size"/> class.
        /// </summary>
        /// <param name="width">Represents the width of element.</param>
        /// <param name="height">Represents the height of element.</param>
        /// <param name="parentHeight">Represents the parent height of element.</param>
        /// <param name="parentWidth">Represents the parent width of element.</param>
        /// <param name="isDevice">Represents the device information.</param>
        /// <param name="windowWidth">Represents the window width.</param>
        /// <param name="windowHeight">Represents the window height.</param>
        public Size(double width, double height, double parentHeight, double parentWidth, bool isDevice, double windowWidth = 0, double windowHeight = 0)
        {
            Width = width;
            Height = height;
            ParentHeight = parentHeight;
            ParentWidth = parentWidth;
            IsDevice = isDevice;
            WindowHeight = windowHeight;
            WindowWidth = windowWidth;
        }
    }

    /// <summary>
    /// Specifies the information of element.
    /// </summary>
    public class ElementInfo
    {
        /// <summary>
        /// Sets or gets the width of element.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Sets or gets the height of element.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Sets or gets the parent width of element.
        /// </summary>
        public double ParentWidth { get; set; }

        /// <summary>
        /// Sets or gets the parent height of element.
        /// </summary>
        public double ParentHeight { get; set; }

        /// <summary>
        /// Sets or gets device information.
        /// </summary>
        public bool IsDevice { get; set; }

        /// <summary>
        /// Sets or gets the window width.
        /// </summary>
        public double WindowWidth { get; set; }

        /// <summary>
        /// Sets or gets the window height.
        /// </summary>
        public double WindowHeight { get; set; }
    }

    internal class SparklineDataModel
    {
        internal SparklineDataModel(double xname, double yname, object xvalue = null)
        {
            XName = xname;
            YName = yname;
            XValue = xvalue;
        }

        internal object XValue { get; set; }

        internal double XName { get; set; }

        internal double YName { get; set; }
    }

    internal class SparklineValues : RectInfo
    {
        internal double Percent { get; set; }

        internal double Degree { get; set; }

        internal double MarkerPosition { get; set; }

        internal double XVal { get; set; }

        internal double YVal { get; set; }

        internal Location Location { get; set; } = new Location();

        internal double StartAngle { get; set; } = double.NaN;

        internal double EndAngle { get; set; } = double.NaN;

        internal object XName { get; set; }
    }

    /// <summary>
    ///  Represents the X and Y position of element.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Specifies the X position.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Specifies the Y position.
        /// </summary>
        public double Y { get; set; }
    }

    internal class PointRegion
    {
        internal bool IsPointRegion { get; set; }

        internal int PointIndex { get; set; }
    }

    internal class RectInfo : Location
    {
        internal RectInfo()
        {
        }

        internal RectInfo(double height, double width, double x = 0, double y = 0)
        {
            Height = height;
            Width = width;
            X = x;
            Y = y;
        }

        internal double Height { get; set; }

        internal double Width { get; set; }
    }

    internal class TextStyle
    {
        internal string Color { get; set; } = string.Empty;

        internal string FontFamily { get; set; } = string.Empty;

        internal string FontStyle { get; set; } = "Normal";

        internal string FontWeight { get; set; } = "Normal";

        internal double Opacity { get; set; } = 1;

        internal string Size { get; set; } = "12px";
    }

    internal class SvgProperties
    {
        internal double Height { get; set; }

        internal double Width { get; set; }
    }

    internal class TooltipPath
    {
        internal string Fill { get; set; }

        internal string Stroke { get; set; }

        internal string Direction { get; set; }
    }

    internal class TooltipTextSetting
    {
        internal double X { get; set; }

        internal double Y { get; set; }

        internal string Anchor { get; set; }

        internal string Transform { get; set; }
    }
}
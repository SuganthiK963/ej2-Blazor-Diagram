using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Syncfusion.Blazor.Charts.Internal;
using System.Runtime.InteropServices;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    public class FontData
    {
        internal double Average { get; set; }

        internal IDictionary<char, double> Chars { get; set; }
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IMouseEventArgs { }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IPointEventArgs { }

    public class ScrollbarThemeStyle
    {
        internal string BackRect { get; set; }

        internal string Thumb { get; set; }

        internal string Circle { get; set; }

        internal string CircleHover { get; set; }

        internal string Arrow { get; set; }

        internal string Grip { get; set; }

        internal string ArrowHover { get; set; }

        internal string BackRectBorder { get; set; }
    }

    public class AxisTooltipAttributes
    {
        internal List<Dictionary<string, object>> PathAttributes { get; set; }

        internal List<Dictionary<string, object>> TextAttributes { get; set; }
    }

    public class Touches
    {
        public double PageX { get; set; }

        public double PageY { get; set; }

        public double PointerId { get; set; }
    }

    public class ZoomAxisRange
    {
        internal double ActualMin { get; set; }

        internal double ActualDelta { get; set; }

        internal double Min { get; set; }

        internal double Delta { get; set; }
    }

    public class Thickness : DomRect
    {
        internal Thickness(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }

    public class ChartInternalLocation : SymbolLocation
    {
        public ChartInternalLocation(double locationX, double locationY)
        {
            X = locationX;
            Y = locationY;
        }
    }

    public class LabelLocation : SymbolLocation
    {
        internal LabelLocation(double locationX, double locationY)
        {
            X = locationX;
            Y = locationY;
        }
    }

    public class ConnectPoints
    {
        internal Point First { get; set; }

        internal Point Last { get; set; }
    }

    public class FromTo
    {
        internal double From { get; set; }

        internal double To { get; set; }
    }

    public class RectPosition
    {
        internal double Position { get; set; }

        internal double RectCount { get; set; }
    }

    public class SymbolLocation
    {
        public double X { get; set; }

        public double Y { get; set; }
    }

    public class AtrPoints
    {
        public object X { get; set; }

        public double Y { get; set; }
    }

    public class DomRect : Size
    {
        public double Left { get; set; }

        public double Top { get; set; }

        public double Right { get; set; }

        public double Bottom { get; set; }
    }

    public class Browser
    {
        public string BrowserName { get; set; }

        public bool IsPointer { get; set; }

        public bool IsDevice { get; set; }

        public bool IsTouch { get; set; }

        public bool IsIos { get; set; }
    }

    public class PolarArc
    {
        internal PolarArc(double startAngle = 0, double endAngle = 0, double innerRadius = 0, double radius = 0, double currentXPosition = 0)
        {
            StartAngle = startAngle;
            EndAngle = endAngle;
            InnerRadius = innerRadius;
            Radius = radius;
            CurrentXPosition = currentXPosition;
        }

        public double StartAngle { get; set; }

        public double EndAngle { get; set; }

        public double InnerRadius { get; set; }

        public double Radius { get; set; }

        public double CurrentXPosition { get; set; }
    }

    public class PointXY : AtrPoints
    {
        public int? SeriesIndex { get; set; }

        public int? PointIndex { get; set; }
    }

    public class FinancialPoint : Point
    {
        public object High { get; set; }

        public object Low { get; set; }

        public object Open { get; set; }

        public object Close { get; set; }

        public object Volume { get; set; }

        // public string Tooltip { get; set; }
    }

    public class BoxPoint : Point
    {
        public double UpperQuartile { get; set; }

        public double LowerQuartile { get; set; }

        public double Median { get; set; }

        public double[] Outliers { get; set; }

        public double[] YValueCollection { get; set; }

        public double Average { get; set; }
    }

    public class BubblePoint : Point
    {
        public object Size { get; set; }
    }

    public class Point : TemplateData
    {
        // public double UpperQuartile { get; set; }
           
        // public double LowerQuartile { get; set; }
           
        // public double Median { get; set; }
           
        // public double[] Outliers { get; set; }
           
        // public double[] YValueCollection { get; set; }
           
        // public double Average { get; set; }

        public double Error { get; set; }

        public bool Visible { get; set; } = true;

        public string Tooltip { get; set; }

        // public string Color { get; set; }

        public List<ChartInternalLocation> SymbolLocations { get; set; } = new List<ChartInternalLocation>();

        public List<Rect> Regions { get; set; } = new List<Rect>();

        public double XValue { get; set; }

        public double YValue { get; set; }

        public int Index { get; set; }

        public double Percentage { get; set; }

        // public object Size { get; set; }

        public PolarArc RegionData { get; set; }

        public bool IsEmpty { get; set; }

        public double Minimum { get; set; }

        public double Maximum { get; set; }

        public string Interior { get; set; }

        // public string Direction { get; set; } = string.Empty;

        public bool IsSelected { get; set; }

        // public bool IsPointInRange { get; set; } = true;

        public MarkerSettingModel Marker { get; set; }

        public List<string> TemplateID { get; set; } = new List<string>();

        public List<Size> TemplateSize { get; set; } = new List<Size>();

        // public bool IsPointRender { get; set; }
    }

    public class VisibleLabels
    {
        internal VisibleLabels(string text, double value, ChartAxisLabelStyle labelStyle, string originalText, Size size = null, Size breakLabelSize = null, double index = 1)
        {
            Text = text;
            TextArr = new string[] { text };
            OriginalText = originalText;
            Value = value;
            LabelStyle = labelStyle;
            Size = size != null ? size : Size;
            BreakLabelSize = breakLabelSize != null ? breakLabelSize : BreakLabelSize;
            Index = index;
        }

        internal string Text { get; set; }

        internal string[] TextArr { get; set; }

        internal double Value { get; set; }

        internal ChartAxisLabelStyle LabelStyle { get; set; }

        internal Size Size { get; set; } = new Size(0, 0);

        internal Size BreakLabelSize { get; set; } = new Size(0, 0);

        internal double Index { get; set; }

        internal string OriginalText { get; set; }
    }

    public class ChartBorderType
    {
        internal double Width { get; set; }

        internal string Color { get; set; }
    }

    public class ChartThemeStyle
    {
        internal string AxisLabel { get; set; }

        internal string AxisTitle { get; set; }

        internal string AxisLine { get; set; }

        internal string MajorGridLine { get; set; }

        internal string MinorGridLine { get; set; }

        internal string MajorTickLine { get; set; }

        internal string MinorTickLine { get; set; }

        internal string ChartTitle { get; set; }

        internal string LegendLabel { get; set; }

        internal string Background { get; set; }

        internal string AreaBorder { get; set; }

        internal string ErrorBar { get; set; }

        internal string CrosshairLine { get; set; }

        internal string CrosshairFill { get; set; }

        internal string CrosshairLabel { get; set; }

        internal string TooltipFill { get; set; }

        internal string TooltipBoldLabel { get; set; }

        internal string TooltipLightLabel { get; set; }

        internal string TooltipHeaderLine { get; set; }

        internal string MarkerShadow { get; set; }

        internal string SelectionRectFill { get; set; }

        internal string SelectionRectStroke { get; set; }

        internal string SelectionCircleStroke { get; set; }
    }

    public class RangeThemeStyle
    {
        internal string GridLineColor { get; set; }

        internal string AxisLineColor { get; set; }

        internal string LabelFontColor { get; set; }

        internal string UnselectedRectColor { get; set; }

        internal string ThumpLineColor { get; set; }

        internal string ThumbBackground { get; set; }

        internal string ThumbHoverColor { get; set; }

        internal string GripColor { get; set; }

        internal string SelectedRegionColor { get; set; }

        internal string Background { get; set; }

        internal string TooltipBackground { get; set; }

        internal string TooltipFontColor { get; set; }

        internal double ThumbWidth { get; set; }

        internal double ThumbHeight { get; set; }
    }

    public class PointData
    {
        internal PointData(Point point, ChartSeries series, double lierIndex = 0)
        {
            Point = point;
            Series = series;
            LierIndex = lierIndex;
        }

        internal PointData()
        {
            // To create empty instance for further calculation not initial.
        }

        public Point Point { get; set; }

        public ChartSeries Series { get; set; }

        public double LierIndex { get; set; }
    }

    public class ClientXY
    {
        public double ClientX { get; set; }

        public double ClientY { get; set; }
    }

    // We need to pass this member to js So all are should public
    public class DynamicAnimation
    {
        public List<string> PathId { get; set; }

        public List<string> PreviousDirection { get; set; }

        public List<string> CurrentDirection { get; set; }

        public List<SymbolLocation> PreviousSymbolLocation { get; set; }

        public string MarkerParentId { get; set; }

        public List<string> MarkerSymbolId { get; set; }

        public bool IsCircle { get; set; }

        public List<string> PreviousSymbolDirection { get; set; }

        public string DataLabelParentId { get; set; }

        public List<string> DataLabelTextId { get; set; }

        public List<SymbolLocation> PreviousTextLocation { get; set; }

        public List<string> ErrorShapeId { get; set; }

        public List<string> ErrorCapId { get; set; }

        public List<string> ErrorShapePDir { get; set; }

        public List<string> ErrorShapeCDir { get; set; }

        public List<string> ErrorCapPDir { get; set; }

        public List<string> ErrorCapCDir { get; set; }
    }

    public class ChartInternalMouseEventArgs : ClientXY
    {
        public bool PreventDefault { get; set; }

        public List<Touches> Touches { get; set; }

        public string Type { get; set; }

        public double PointerId { get; set; }

        public double MouseX { get; set; }

        public double MouseY { get; set; }

        public string PointerType { get; set; }

        public string Target { get; set; }

        public ClientXY ChangedTouches { get; set; }
    }

    public class ChartMouseWheelArgs : ClientXY
    {
        public double Detail { get; set; }

        public double WheelDelta { get; set; }

        public double MouseX { get; set; }

        public double MouseY { get; set; }

        public string BrowserName { get; set; }

        public string Target { get; set; }

        public bool IsPointer { get; set; }
    }

    public class TemplateData
    {
        public object X { get; set; }

        public object Y { get; set; }

        public string Text { get; set; }

        // public string Start { get; set; }

        // public string End { get; set; }

        // public string Value { get; set; }
    }

    public class StackValues
    {
        internal StackValues(List<double> startValue, List<double> endValue)
        {
            StartValues = startValue;
            EndValues = endValue;
        }

        internal List<double> StartValues { get; set; } = new List<double>();

        internal List<double> EndValues { get; set; } = new List<double>();
    }

    public class ControlPoints
    {
        internal ControlPoints(ChartInternalLocation controlPoint1, ChartInternalLocation controlPoint2)
        {
            ControlPoint1 = controlPoint1;
            ControlPoint2 = controlPoint2;
        }

        internal ChartInternalLocation ControlPoint1 { get; set; }

        internal ChartInternalLocation ControlPoint2 { get; set; }
    }

    public class HistogramValues
    {
        internal double SDValue { get; set; }

        internal double Mean { get; set; }

        internal double BinWidth { get; set; }

        internal List<double> YValues { get; set; }
    }

    public class BoxPlotQuartile
    {
        internal double Minimum { get; set; }

        internal double Maximum { get; set; }

        internal List<double> Outliers { get; set; }

        internal double UpperQuartile { get; set; }

        internal double LowerQuartile { get; set; }

        internal double Average { get; set; }

        internal double Median { get; set; }
    }

    public class SlopeIntercept
    {
        internal double Slope { get; set; }

        internal double Intercept { get; set; }
    }

    public class AnimationOptions
    {
        internal AnimationOptions(string id, AnimationType type)
        {
            Id = id;
            Type = type;
        }

        // We need to pass this member to js So this should public
        public string Id { get; set; }

        internal AnimationType Type { get; set; }
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class DynamicPathAnimationOptions
    {
        public string ParentId { get; set; }

        public string Id { get; set; }

        public string PreviousDir { get; set; }

        public string CurrentDir { get; set; }
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class DynamicRectAnimationOptions
    {
        public string Id { get; set; }

        public Rect PreviousRect { get; set; }

        public Rect CurrentRect { get; set; }
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class DynamicTextAnimationOptions
    {
        public string Id { get; set; }

        public virtual double PreLocationX { get; set; }

        public virtual double PreLocationY { get; set; }

        public double CurLocationX { get; set; }

        public double CurLocationY { get; set; }

        public string X { get; set; } = "x";

        public string Y { get; set; } = "y";
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class DynamicAccTextAnimationOptions : DynamicTextAnimationOptions
    {
        public override double PreLocationX { get; set; } = double.NaN;

        public override double PreLocationY { get; set; } = double.NaN;
    }

    public class Mean
    {
        internal Mean(double verticalStandardMean, double verticalSquareRoot, double horizontalStandardMean, double horizontalSquareRoot, double verticalMean, double horizontalMean)
        {
            VerticalStandardMean = verticalStandardMean;
            HorizontalStandardMean = horizontalStandardMean;
            VerticalSquareRoot = verticalSquareRoot;
            HorizontalSquareRoot = horizontalSquareRoot;
            VerticalMean = verticalMean;
            HorizontalMean = horizontalMean;
        }

        internal double VerticalStandardMean { get; set; }

        internal double HorizontalStandardMean { get; set; }

        internal double VerticalSquareRoot { get; set; }

        internal double HorizontalSquareRoot { get; set; }

        internal double VerticalMean { get; set; }

        internal double HorizontalMean { get; set; }
    }

    public class BollingerPoints
    {
        internal double UpperBound { get; set; }

        internal double LowerBound { get; set; }

        internal double MiddleBound { get; set; }
    }

    public class LegendOption
    {
        internal LegendOption()
        {
            // To create empty instance for further calculation not initial.
        }

        internal LegendOption(string text, string fill, LegendShape shape, bool visible, string type, [Optional] string accType, [Optional] ChartShape markerShape, [Optional] bool markerVisibility, [Optional] double pointIndex, [Optional] double seriesIndex, [Optional] string seriesBorderColor, [Optional] double seriesBorderWidth, [Optional] double seriesWidth, [Optional] ChartDefaultFont textStyle)
        {
            Text = text;
            Fill = fill;
            Shape = shape;
            Visible = visible;
            Type = type;
            AccType = accType;
            MarkerVisibility = markerVisibility;
            MarkerShape = markerShape;
            PointIndex = pointIndex;
            SeriesIndex = seriesIndex;
            SeriesBorderColor = seriesBorderColor;
            SeriesBorderWidth = seriesBorderWidth;
            SeriesWidth = seriesWidth;
            TextStyle = textStyle;
        }

        public bool Render { get; set; }

        public string Text { get; set; }

        public string Fill { get; set; }

        public LegendShape Shape { get; set; }

        public bool Visible { get; set; }

        public string Type { get; set; }

        public string AccType { get; set; }

        public Size TextSize { get; set; }

        public ChartInternalLocation Location { get; set; } = new ChartInternalLocation(0, 0);

        public double PointIndex { get; set; }

        public double SeriesIndex { get; set; }

        public ChartShape MarkerShape { get; set; }

        public bool MarkerVisibility { get; set; }

        public string SeriesBorderColor { get; set; }

        public double SeriesBorderWidth { get; set; }

        public double SeriesWidth { get; set; }

        public ChartDefaultFont TextStyle { get; set; }
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class MarkerAnimationInfo
    {
        public string MarkerElementId { get; set; }

        public string MarkerClipPathId { get; set; }

        public List<double> PointIndex { get; set; } = new List<double>();

        public List<double> LowPointIndex { get; set; } = new List<double>();

        public List<double> PointX { get; set; } = new List<double>();

        public List<double> PointY { get; set; } = new List<double>();

        public List<double> LowPointX { get; set; } = new List<double>();

        public List<double> LowPointY { get; set; } = new List<double>();
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class ErrorBarAnimationInfo
    {
        public string ErrorBarElementId { get; set; }

        public string ErrorBarClipPathId { get; set; }
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class DataLabelAnimatioInfo
    {
        public string ShapeGroupId { get; set; }

        public string TextGroupId { get; set; }

        public List<string> TemplateId { get; set; } = new List<string>();
    }

    // NOTE: Need to pass all these members to JS so they are declared as public
    public class InitialAnimationInfo
    {
        public string Type { get; set; }

        public string ElementId { get; set; }

        public string ClipPathId { get; set; }

        public List<double> PointIndex { get; set; } = new List<double>();

        public List<double> PointX { get; set; } = new List<double>();

        public List<double> PointY { get; set; } = new List<double>();

        public List<double> PointWidth { get; set; } = new List<double>();

        public List<double> PointHeight { get; set; } = new List<double>();

        public double Duration { get; set; }

        public double Delay { get; set; }

        public bool IsInvertedAxis { get; set; }

        public string StrokeDashArray { get; set; }

        public MarkerAnimationInfo MarkerInfo { get; set; }

        public ErrorBarAnimationInfo ErrorBarInfo { get; set; }

        public DataLabelAnimatioInfo DataLabelInfo { get; set; }
    }

    /// <summary>
    /// Defines the crosshair axis information.
    /// </summary>
    public class CrosshairAxisInfo
    {
        public CrosshairAxisInfo(string axisName, string axisLabel)
        {
            AxisName = axisName;
            AxisLabel = axisLabel;
        }

        /// <summary>
        /// Define the specific axis name.
        /// </summary>
        public string AxisName { get; private set; }

        /// <summary>
        /// Define the specific axis label.
        /// </summary>
        public string AxisLabel { get; private set; }
    }

    public class Size
    {
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public Size()
        {
        }

        public double Width { get; set; }

        public double Height { get; set; }

        public static bool operator ==(Size a, Size b)
        {
            return a?.Width == b?.Width && a?.Height == b?.Height;
        }

        public static bool operator !=(Size a, Size b)
        {
            return a?.Width != b?.Width || a?.Height != b?.Height;
        }

        public static Size operator -(Size a, Size b)
        {
            if (a != null && b != null)
            {
                return new Size() { Width = a.Width - b.Width, Height = a.Height - b.Height };
            }

            return new Size();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return Equals(obj);
        }

        public override int GetHashCode()
        {
            return GetHashCode();
        }

        public static Size Subtract(Size left, Size right)
        {
            if (left != null && right != null)
            {
                return new Size() { Width = left.Width - right.Width, Height = left.Height - right.Height };
            }

            return new Size();
        }
    }

    public class ChartFontOptions : FontOptions
    {
        internal TextOverflow TextOverflow { get; set; }

        internal Alignment TextAlignment { get; set; }
    }

    internal class AxisRenderInfo
    {
        internal Dictionary<string, List<PathOptions>> AxisGridOptions { get; set; } = new Dictionary<string, List<PathOptions>>();

        internal List<TextOptions> AxisLabelOptions { get; set; } = new List<TextOptions>();

        internal List<CircleOptions> MajorGridCircleOptions { get; set; } = new List<CircleOptions>();

        internal TextOptions AxisTitleOption { get; set; }

        internal PathOptions AxisLine { get; set; }

        internal PathOptions AxisBorder { get; set; }
    }
}
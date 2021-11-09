using System.Collections.Generic;
using System.Drawing;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Specifies the size of the rect in circular gauge.
    /// </summary>
    public class Rect
    {
        /// <summary>
        /// Gets or sets the x position of the rectangle.
        /// </summary>
        internal double X { get; set; }

        /// <summary>
        /// Gets or sets the y position of the rectangle.
        /// </summary>
        internal double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        internal double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        internal double Height { get; set; }
    }

    /// <summary>
    /// Specifies the size of the circular gauge.
    /// </summary>
    public class SizeD
    {
        /// <summary>
        /// Gets or sets the width of the circular gauge.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the circular gauge.
        /// </summary>
        public double Height { get; set; }
    }

    /// <summary>
    /// Specifies the size and browser information of the circular gauge.
    /// </summary>
    public class ElementInfo : SizeD
    {
        /// <summary>
        /// Gets or sets the whether the circular gauge is rendered in IE or other browsers.
        /// </summary>
        public bool IsIE { get; set; }
    }

    /// <summary>
    /// Specifies the tick line properties of the axis in circular gauge.
    /// </summary>
    public class TickLine
    {
        /// <summary>
        /// Gets or sets the minimum value of the axis.
        /// </summary>
        internal double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the axis.
        /// </summary>
        internal double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the interval of the axis.
        /// </summary>
        internal double Interval { get; set; }
    }

    /// <summary>
    /// Specifies the text properties of the axis labels in circular gauge.
    /// </summary>
    public class TextSetting
    {
        /// <summary>
        /// Gets or sets the x position in axis label.
        /// </summary>
        internal double X { get; set; }

        /// <summary>
        /// Gets or sets the y position in axis label.
        /// </summary>
        internal double Y { get; set; }

        /// <summary>
        /// Gets or sets the anchor position as beginning or the end.
        /// </summary>
        internal string Anchor { get; set; }

        /// <summary>
        /// Gets or sets the content of the text.
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        /// Gets or sets the transform position to the text.
        /// </summary>
        internal string Transform { get; set; }

        /// <summary>
        /// Gets or sets the fill color to the text.
        /// </summary>
        internal string Fill { get; set; }
    }

    /// <summary>
    /// Specifies the label properties of the axis in circular gauge.
    /// </summary>
    public class LableSetting
    {
        /// <summary>
        /// Gets or sets the content of the text.
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        /// Gets or sets the value of the text.
        /// </summary>
        internal double Value { get; set; }

        /// <summary>
        /// Gets or sets the size of the text.
        /// </summary>
        internal SizeD TextSize { get; set; }
    }

    /// <summary>
    /// Specifies the axis properties in circular gauge.
    /// </summary>
    public class Axis
    {
        /// <summary>
        /// Gets or sets the near size of the axis.
        /// </summary>
        internal double NearSize { get; set; }

        /// <summary>
        /// Gets or sets the far size of the axis.
        /// </summary>
        internal double FarSize { get; set; }

        /// <summary>
        /// Gets or sets the outer path of the axis.
        /// </summary>
        internal string AxisOuterPath { get; set; }

        /// <summary>
        /// Gets or sets the line path of the axis.
        /// </summary>
        internal string AxisLinePath { get; set; }

        /// <summary>
        /// Gets or sets the outer background of the axis.
        /// </summary>
        internal string AxesOuterBackground { get; set; }

        /// <summary>
        /// Gets or sets the stroke of the axis line.
        /// </summary>
        internal string Stroke { get; set; }

        /// <summary>
        /// Gets or sets the width of the stroke in the axis line.
        /// </summary>
        internal double StrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the dash array of the axis line.
        /// </summary>
        internal string DashArray { get; set; }

        /// <summary>
        /// Gets or sets the path of the major tick lines.
        /// </summary>
        internal List<string> MajorTickLinePath { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the stroke of the major tick lines.
        /// </summary>
        internal List<string> MajorTickLineStroke { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the stroke width of the major tick lines.
        /// </summary>
        internal double MajorTickLineStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the path of the minor tick lines.
        /// </summary>
        internal List<string> MinorTickPath { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the dash array of the major tick lines.
        /// </summary>
        internal string MajorTickDashArray { get; set; } = "0";

        /// <summary>
        /// Gets or sets the dash array of the minor tick lines.
        /// </summary>
        internal string MinorTickDashArray { get; set; } = "0";

        /// <summary>
        /// Gets or sets the style of the labels.
        /// </summary>
        internal string LabelFontStyle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the opacity of the labels.
        /// </summary>
        internal double LabelOpacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the text properties.
        /// </summary>
        internal List<TextSetting> TextSettingCollection { get; set; } = new List<TextSetting>();

        /// <summary>
        /// Gets or sets a value indicating whether or not the axis is visible.
        /// </summary>
        internal bool AxisVisible { get; set; }

        /// <summary>
        /// Gets or sets the width of the axis line.
        /// </summary>
        internal double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the axis line.
        /// </summary>
        internal double Height { get; set; }

        /// <summary>
        /// Gets or sets the rect value of the circular gauge.
        /// </summary>
        internal Rect RectValue { get; set; }

        /// <summary>
        /// Gets or sets the mid points in the circular gauge.
        /// </summary>
        internal PointF MidPoint { get; set; }

        /// <summary>
        /// Gets or sets the tick properties.
        /// </summary>
        internal TickLine TickLineSetting { get; set; }

        /// <summary>
        /// Gets or sets the current radius of the circular gauge.
        /// </summary>
        internal double CurrentRadius { get; set; }

        /// <summary>
        /// Gets or sets the major tick properties.
        /// </summary>
        internal List<double> MajorTickValues { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the stroke of the minor tick line.
        /// </summary>
        internal List<string> MinorTickLineStroke { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the stroke width of the minor tick line.
        /// </summary>
        internal double MinorTickLineStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the label properties.
        /// </summary>
        internal List<LableSetting> LabelSettingCollection { get; set; } = new List<LableSetting>();

        /// <summary>
        /// Gets or sets the rect of the circular gauge.
        /// </summary>
        internal Rect GaugeRect { get; set; }

        /// <summary>
        /// Gets or sets the size of the maximum label in the axis.
        /// </summary>
        internal SizeD MaxLabelSize { get; set; }

        /// <summary>
        /// Gets or sets the border of x in circular gauge.
        /// </summary>
        internal double BorderX { get; set; }

        /// <summary>
        /// Gets or sets the border of y in circular gauge.
        /// </summary>
        internal double BorderY { get; set; }

        /// <summary>
        /// Gets or sets the border of the width.
        /// </summary>
        internal double BorderRectWidth { get; set; }

        /// <summary>
        /// Gets or sets the border of the height.
        /// </summary>
        internal double BorderRectHeight { get; set; }

        /// <summary>
        /// Gets or sets the border of the stroke width.
        /// </summary>
        internal double BorderStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the border of the stroke color.
        /// </summary>
        internal string BorderStrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the width of the circular gauge.
        /// </summary>
        internal double ActualWidth { get; set; }

        /// <summary>
        /// Gets or sets the outer background of the circular gauge.
        /// </summary>
        internal string OuterBackground { get; set; }

        /// <summary>
        /// Gets or sets the width of the last text in the labels.
        /// </summary>
        internal double LastTextWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the last text in the labels.
        /// </summary>
        internal double LastTextHeight { get; set; }

        /// <summary>
        /// Gets or sets the range properties.
        /// </summary>
        internal List<Range> RangeCollection { get; set; }

        /// <summary>
        /// Gets or sets the pointer properties.
        /// </summary>
        internal List<PointerSetting> PointerCollection { get; set; }

        /// <summary>
        /// Gets or sets the index of the axis.
        /// </summary>
        internal int AxisIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the legend is visible.
        /// </summary>
        internal bool LegendVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the annotation is visible.
        /// </summary>
        internal bool AnnotationVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the tooltip is visible.
        /// </summary>
        internal bool TooltipVisible { get; set; }

        /// <summary>
        /// Gets or sets the animation of the pointer properties.
        /// </summary>
        internal List<AnimationOptions> PointerAnimate { get; set; }

        /// <summary>
        /// Gets or sets the animation of the range properties.
        /// </summary>
        internal List<AnimationOptions> RangeAnimate { get; set; }

        /// <summary>
        /// Gets or sets the annotation properties.
        /// </summary>
        internal List<Annotation> Annotations { get; set; } = new List<Annotation>();
    }

    /// <summary>
    /// Specifies the title properties.
    /// </summary>
    public class TitleRenderer
    {
        /// <summary>
        /// Gets or sets the description value of the title.
        /// </summary>
        internal string Description { get; set; }

        /// <summary>
        /// Gets or sets the size of the title.
        /// </summary>
        internal SizeD TitleSize { get; set; }

        /// <summary>
        /// Gets or sets the styles of the title.
        /// </summary>
        internal string TitleFontStyle { get; set; }

        /// <summary>
        /// Gets or sets the opacity value of the title.
        /// </summary>
        internal double TitleOpacity { get; set; }

        /// <summary>
        /// Gets or sets the title properties.
        /// </summary>
        internal TextSetting TitleSetting { get; set; }
    }

    /// <summary>
    /// Specifies the properties in the ranges.
    /// </summary>
    public class Range
    {
        /// <summary>
        /// Gets or sets the start width of the range.
        /// </summary>
        internal double RangeStartWidth { get; set; }

        /// <summary>
        /// Gets or sets the end width of the range.
        /// </summary>
        internal double RangeEndWidth { get; set; }

        /// <summary>
        /// Gets or sets the rounded path of the range.
        /// </summary>
        internal string RangeRoundedPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the path of the range.
        /// </summary>
        internal string RangePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current radius of the range.
        /// </summary>
        internal double RangeCurrentRadius { get; set; }

        /// <summary>
        /// Gets or sets the current distance from the scale of the range.
        /// </summary>
        internal double RangeCurrentDistanceFromScale { get; set; }

        /// <summary>
        /// Gets or sets the fill color of the range.
        /// </summary>
        internal string RangeFillColor { get; set; }

        /// <summary>
        /// Gets or sets the start value of the range.
        /// </summary>
        internal string StartValue { get; set; }

        /// <summary>
        /// Gets or sets the end value of the range.
        /// </summary>
        internal string EndValue { get; set; }

        /// <summary>
        /// Gets or sets the linear color value as string in the range.
        /// </summary>
        internal string LinearColorString { get; set; }

        /// <summary>
        /// Gets or sets the radius of the range.
        /// </summary>
        internal string Radius { get; set; }

        /// <summary>
        /// Gets or sets the outer x position of the range.
        /// </summary>
        internal string OuterPositionX { get; set; }

        /// <summary>
        /// Gets or sets the outer y position of the range.
        /// </summary>
        internal string OuterPositionY { get; set; }

        /// <summary>
        /// Gets or sets the inner x position of the range.
        /// </summary>
        internal string InnerPositionX { get; set; }

        /// <summary>
        /// Gets or sets the inner y position of the range.
        /// </summary>
        internal string InnerPositionY { get; set; }

        /// <summary>
        /// Gets or sets the radial color as string in the range.
        /// </summary>
        internal string RadialColorString { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the range.
        /// </summary>
        internal double Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indication whether or not the visibility of the range.
        /// </summary>
        internal string Visibility { get; set; } = "visibility:visible";
    }

    /// <summary>
    /// Specifies the properties of the pointer.
    /// </summary>
    public class PointerSetting
    {
        /// <summary>
        /// Gets or sets the current distance from scale of the pointer.
        /// </summary>
        internal double CurrentDistanceFromScale { get; set; }

        /// <summary>
        /// Gets or sets the shape of the pointer.
        /// </summary>
        internal string MarkerShape { get; set; }

        /// <summary>
        /// Gets or sets the location of the marker.
        /// </summary>
        internal PointF Location { get; set; }

        /// <summary>
        /// Gets or sets the image x position of the marker.
        /// </summary>
        internal double ImageX { get; set; }

        /// <summary>
        /// Gets or sets the image y position of the marker.
        /// </summary>
        internal double ImageY { get; set; }

        /// <summary>
        /// Gets or sets the styles of the marker text.
        /// </summary>
        internal string TextStyle { get; set; }

        /// <summary>
        /// Gets or sets the stroke of the marker.
        /// </summary>
        internal string MarkerStroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke width of the marker.
        /// </summary>
        internal double MarkerStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the color of the marker.
        /// </summary>
        internal string MarkerColor { get; set; }

        /// <summary>
        /// Gets or sets the circle properties.
        /// </summary>
        internal Circle Circle { get; set; }

        /// <summary>
        /// Gets or sets the current radius of the marker.
        /// </summary>
        internal double CurrentRadius { get; set; }

        /// <summary>
        /// Gets or sets the current value of the pointer.
        /// </summary>
        internal double CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the angle of the pointer.
        /// </summary>
        internal string PointerAngle { get; set; }

        /// <summary>
        /// Gets or sets the path of the pointer type as range.
        /// </summary>
        internal string PointerRangePath { get; set; }

        /// <summary>
        /// Gets or sets the rounded path of the pointer type as range.
        /// </summary>
        internal string PointerRangeRoundPath { get; set; }

        /// <summary>
        /// Gets or sets the needle location of the pointer.
        /// </summary>
        internal PointF NeedleLocation { get; set; }

        /// <summary>
        /// Gets or sets the needle direction of the pointer.
        /// </summary>
        internal string NeedleDirection { get; set; }

        /// <summary>
        /// Gets or sets the rect path for the pointer.
        /// </summary>
        internal string RectPath { get; set; }

        /// <summary>
        /// Gets or sets the direction of the pointer.
        /// </summary>
        internal string Direction { get; set; }

        /// <summary>
        /// Gets or sets the path of the cap.
        /// </summary>
        internal Circle CapPath { get; set; }

        /// <summary>
        /// Gets or sets the needle stroke of the pointer.
        /// </summary>
        internal string NeedleStroke { get; set; }

        /// <summary>
        /// Gets or sets the needle stroke width of the pointer.
        /// </summary>
        internal double NeedleStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the needle color of the pointer..
        /// </summary>
        internal string NeedleColor { get; set; }

        /// <summary>
        /// Gets or sets the cap color of the pointer.
        /// </summary>
        internal string CapColor { get; set; }

        /// <summary>
        /// Gets or sets the cap stroke of the pointer.
        /// </summary>
        internal string CapStroke { get; set; }

        /// <summary>
        /// Gets or sets the cap stroke width of the pointer.
        /// </summary>
        internal double CapStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the description value of the pointer.
        /// </summary>
        internal string Description { get; set; }

        /// <summary>
        /// Gets or sets the stroke width of the border for the pointer.
        /// </summary>
        internal double PointerBorderStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the stroke of the border for the pointer.
        /// </summary>
        internal string PointerBorderStroke { get; set; }

        /// <summary>
        /// Gets or sets the color of the pointer.
        /// </summary>
        internal string PointerColor { get; set; }

        /// <summary>
        /// Gets or sets the stroke of the rangebar in the pointer.
        /// </summary>
        internal string RangeBarStroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke width of the rangebar in the pointer.
        /// </summary>
        internal double RangeBarStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the color of the rangebar in the pointer.
        /// </summary>
        internal string RangeBarColor { get; set; }

        /// <summary>
        /// Gets or sets the linear start value of the cap in pointer.
        /// </summary>
        internal string CapLinearStartValue { get; set; }

        /// <summary>
        /// Gets or sets the linear end value of the cap in pointer.
        /// </summary>
        internal string CapLinearEndValue { get; set; }

        /// <summary>
        /// Gets or sets the linear string value of the cap in pointer.
        /// </summary>
        internal string CapLinearString { get; set; }

        /// <summary>
        /// Gets or sets the radial radius of the cap in pointer.
        /// </summary>
        internal string CapRadialRadius { get; set; }

        /// <summary>
        /// Gets or sets the outer position of X to the cap.
        /// </summary>
        internal string CapOuterPositionX { get; set; }

        /// <summary>
        /// Gets or sets the outer position of Y to the cap.
        /// </summary>
        internal string CapOuterPositionY { get; set; }

        /// <summary>
        /// Gets or sets the inner position of X to the cap.
        /// </summary>
        internal string CapInnerPositionX { get; set; }

        /// <summary>
        /// Gets or sets the inner position of Y to the cap.
        /// </summary>
        internal string CapInnerPositionY { get; set; }

        /// <summary>
        /// Gets or sets the radial string to the cap.
        /// </summary>
        internal string CapRadialString { get; set; }

        /// <summary>
        /// Gets or sets the linear start value to the needle.
        /// </summary>
        internal string NeedleLinearStartValue { get; set; }

        /// <summary>
        /// Gets or sets the linear end value to the needle.
        /// </summary>
        internal string NeedleLinearEndValue { get; set; }

        /// <summary>
        /// Gets or sets the linear string value to the needle.
        /// </summary>
        internal string NeedleLinearString { get; set; }

        /// <summary>
        /// Gets or sets the radial radius to the pointer needle.
        /// </summary>
        internal string NeedleRadialRadius { get; set; }

        /// <summary>
        /// Gets or sets the outer position of x to the needle.
        /// </summary>
        internal string NeedleOuterPositionX { get; set; }

        /// <summary>
        /// Gets or sets the outer position of y to the needle.
        /// </summary>
        internal string NeedleOuterPositionY { get; set; }

        /// <summary>
        /// Gets or sets the inner position of x to the needle.
        /// </summary>
        internal string NeedleInnerPositionX { get; set; }

        /// <summary>
        /// Gets or sets the inner position of x to the needle.
        /// </summary>
        internal string NeedleInnerPositionY { get; set; }

        /// <summary>
        /// Gets or sets the radial string to the needle.
        /// </summary>
        internal string NeedleRadialString { get; set; }

        /// <summary>
        /// Gets or sets the linear start value of the pointer.
        /// </summary>
        internal string PointerLinearStartValue { get; set; }

        /// <summary>
        /// Gets or sets the linear end value of the pointer.
        /// </summary>
        internal string PointerLinearEndValue { get; set; }

        /// <summary>
        /// Gets or sets the linear string of the pointer.
        /// </summary>
        internal string PointerLinearString { get; set; }

        /// <summary>
        /// Gets or sets the radial radius of the pointer.
        /// </summary>
        internal string PointerRadialRadius { get; set; }

        /// <summary>
        /// Gets or sets the outer position of x to the pointer.
        /// </summary>
        internal string PointerOuterPositionX { get; set; }

        /// <summary>
        /// Gets or sets the outer position of y to the pointer.
        /// </summary>
        internal string PointerOuterPositionY { get; set; }

        /// <summary>
        /// Gets or sets the inner position of x to the pointer.
        /// </summary>
        internal string PointerInnerPositionX { get; set; }

        /// <summary>
        /// Gets or sets the inner position of y to the pointer.
        /// </summary>
        internal string PointerInnerPositionY { get; set; }

        /// <summary>
        /// Gets or sets the radial string to the pointer.
        /// </summary>
        internal string PointerRadialString { get; set; }
    }

    /// <summary>
    /// Specifies the properties of the legend.
    /// </summary>
    public class Legend
    {
        /// <summary>
        /// Gets or sets the text of the legend.
        /// </summary>
        internal string Text { get; set; }

        /// <summary>
        /// Gets or sets the fill color of the legend.
        /// </summary>
        internal string Fill { get; set; }

        /// <summary>
        /// Gets or sets index of the range.
        /// </summary>
        internal int RangeIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the axis.
        /// </summary>
        internal int AxisIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the legend is rendered.
        /// </summary>
        internal bool LegendRender { get; set; }

        /// <summary>
        /// Gets or sets the size of the text in the legend.
        /// </summary>
        internal SizeD LegendTextSize { get; set; }

        /// <summary>
        /// Gets or sets the location of the legend.
        /// </summary>
        internal PointF LegendLocation { get; set; }

        /// <summary>
        /// Gets or sets the toggle fill to the legend.
        /// </summary>
        internal string LegendToggleFill { get; set; } = "black";
    }

    /// <summary>
    /// Gets or sets the properties of the legend.
    /// </summary>
    public class LegendSetting
    {
        /// <summary>
        /// Gets or sets the properties of the legend items.
        /// </summary>
        internal List<Legend> LegendItemCollections { get; set; } = new List<Legend>();

        /// <summary>
        /// Gets or sets the border of the rect to the legend.
        /// </summary>
        internal Rect LegendBorderRect { get; set; }

        /// <summary>
        /// Gets or sets the border of the stroke to the legend.
        /// </summary>
        internal string BorderStroke { get; set; }

        /// <summary>
        /// Gets or sets the background value to the legend.
        /// </summary>
        internal string Background { get; set; }

        /// <summary>
        /// Gets or sets the border of the width to the legend.
        /// </summary>
        internal double BorderStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the bounds of the legend.
        /// </summary>
        internal Rect LegendBounds { get; set; }

        /// <summary>
        /// Gets or sets the maximum height of the legend item.
        /// </summary>
        internal double MaximumItemHeight { get; set; }

        /// <summary>
        /// Gets or sets the size of the legend text.
        /// </summary>
        internal double LegendFontSize { get; set; } = 14;

        /// <summary>
        /// Gets or sets the path of the legend.
        /// </summary>
        internal List<Path> LegendShapePaths { get; set; } = new List<Path>();

        /// <summary>
        /// Gets or sets the padding to the legend.
        /// </summary>
        internal double Padding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the paging is enabled.
        /// </summary>
        internal bool LegendPaging { get; set; }

        /// <summary>
        /// Gets or sets the stroke of the legend.
        /// </summary>
        internal string LegendStroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke width of the legend.
        /// </summary>
        internal double LegendStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the legend.
        /// </summary>
        internal double LegendOpacity { get; set; }

        /// <summary>
        /// Gets or sets the properties of legend circles.
        /// </summary>
        internal List<Circle> LegendCircles { get; set; } = new List<Circle>();

        /// <summary>
        /// Gets or sets the colors to the legends.
        /// </summary>
        internal List<Legend> LegendColors { get; set; } = new List<Legend>();

        /// <summary>
        /// Gets or sets the total pages in the legend.
        /// </summary>
        internal double TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the row count of the legend.
        /// </summary>
        internal double TotalRowCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum column width of the legend.
        /// </summary>
        internal double MaximumColumnWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum columns of the legend.
        /// </summary>
        internal double MaximumColumns { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the legend.
        /// </summary>
        internal double MaximumWidth { get; set; }

        /// <summary>
        /// Gets or sets the row count of the legend.
        /// </summary>
        internal double RowCount { get; set; }

        /// <summary>
        /// Gets or sets the text collections of the legend.
        /// </summary>
        internal List<TextSetting> LegendTextCollections { get; set; } = new List<TextSetting>();

        /// <summary>
        /// Gets or sets the locations of the legend.
        /// </summary>
        internal List<PointF> LegendLocations { get; set; } = new List<PointF>();

        /// <summary>
        /// Gets or sets the left path for the page of the legend.
        /// </summary>
        internal Path LegendPageLeftPath { get; set; }

        /// <summary>
        /// Gets or sets the right path for the page of the legend.
        /// </summary>
        internal Path LegendPageRightPath { get; set; }

        /// <summary>
        /// Gets or sets the current page of the legend.
        /// </summary>
        internal int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Gets or sets the regions of pages in the legend.
        /// </summary>
        internal List<Rect> PagingRegions { get; set; } = new List<Rect>();

        /// <summary>
        /// Gets or sets the page text for the legend.
        /// </summary>
        internal TextSetting LegendPageText { get; set; } = new TextSetting();

        /// <summary>
        /// Gets or sets the collections of pagex in the legend.
        /// </summary>
        internal List<int> PageXCollections { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets the translate page to the legend.
        /// </summary>
        internal string LegendPageTranslate { get; set; }

        /// <summary>
        /// Gets or sets the translation of the legend.
        /// </summary>
        internal string LegendTranslate { get; set; }

        /// <summary>
        /// Gets or sets the clip path height of the legend.
        /// </summary>
        internal double ClipPathHeight { get; set; }

        /// <summary>
        /// Gets or sets the style of the legend text.
        /// </summary>
        internal string LegendTextStyle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the page is enabled.
        /// </summary>
        internal bool IsPagingEnabled { get; set; }
    }

    /// <summary>
    /// Specifies the index value of the legend.
    /// </summary>
    public class LegendIndex
    {
        /// <summary>
        /// Gets or sets the index value of the axis.
        /// </summary>
        internal int AxisIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the range.
        /// </summary>
        internal int RangeIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the toggle legend is enabled.
        /// </summary>
        internal bool IsToggled { get; set; }
    }

    /// <summary>
    /// Specifies the properties of the path in circular gauge.
    /// </summary>
    public class Path
    {
        /// <summary>
        /// Gets or sets path of the shape in circular gauge.
        /// </summary>
        internal string ShapePath { get; set; }

        /// <summary>
        /// Gets or sets the image path in circular gauge.
        /// </summary>
        internal Rect ImagePath { get; set; }
    }

    /// <summary>
    /// Specifies the properties to render the annotations.
    /// </summary>
    public class Annotation
    {
        /// <summary>
        /// Gets or sets the template styles to the annotations.
        /// </summary>
        internal string AnnotationTemplateStyles { get; set; }

        /// <summary>
        /// Gets or sets the content template to the annotation.
        /// </summary>
        internal string ContentTemplate { get; set; }

        /// <summary>
        /// Gets or sets the location of the annotation.
        /// </summary>
        internal PointF AnnotationLocation { get; set; }

        /// <summary>
        /// Gets or sets the position of the annotation.
        /// </summary>
        internal string AnnotationPosition { get; set; }

        /// <summary>
        /// Gets or sets the styles of the annotation text.
        /// </summary>
        internal string AnnotationTextStyle { get; set; }

        /// <summary>
        /// Gets or sets the id of the annotation.
        /// </summary>
        internal string AnnotationID { get; set; }

        /// <summary>
        /// Gets or sets the left position of annotation.
        /// </summary>
        internal string LeftPosition { get; set; }

        /// <summary>
        /// Gets or sets the top position of annotation.
        /// </summary>
        internal string TopPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the annotation template is enabled.
        /// </summary>
        internal bool IsAnnotationTemplate { get; set; }

        /// <summary>
        /// Gets or sets the description value to the annotation.
        /// </summary>
        internal string Description { get; set; } = "Annotation";

        /// <summary>
        /// Gets or sets a value indicating whether or not the template is enabled.
        /// </summary>
        internal bool IsTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the visibility of the annotation.
        /// </summary>
        internal string Visibility { get; set; } = "visibility:visible";
    }

    /// <summary>
    /// Specifies of the properties of animations in circular gauge.
    /// </summary>
    public class AnimationOptions
    {
        /// <summary>
        /// Gets or sets the start value to the animation.
        /// </summary>
        internal double Start { get; set; }

        /// <summary>
        /// Gets or sets the duration value to the animation.
        /// </summary>
        internal double Duration { get; set; }

        /// <summary>
        /// Gets or sets the end value to the animation.
        /// </summary>
        internal double End { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the axis in clockwise.
        /// </summary>
        internal bool IsClockWise { get; set; }

        /// <summary>
        /// Gets or sets the angle of the start to the axis.
        /// </summary>
        internal double StartAngle { get; set; }

        /// <summary>
        /// Gets or sets the angle of the end to the axis.
        /// </summary>
        internal double EndAngle { get; set; }

        /// <summary>
        /// Gets or sets the mid point of the circular gauge.
        /// </summary>
        internal PointF MidPoint { get; set; }

        /// <summary>
        /// Gets or sets the radius of the circular gauge.
        /// </summary>
        internal double Radius { get; set; }

        /// <summary>
        /// Gets or sets the inner radius of the circular gauge.
        /// </summary>
        internal double InnerRadius { get; set; }

        /// <summary>
        /// Gets or sets the minimum angle of the circular gauge.
        /// </summary>
        internal double MinimumAngle { get; set; }

        /// <summary>
        /// Gets or sets the old start value for the range.
        /// </summary>
        internal double OldStart { get; set; }

        /// <summary>
        /// Gets or sets the width of the pointer.
        /// </summary>
        internal double PointerWidth { get; set; }

        /// <summary>
        /// Gets or sets the rounded radius.
        /// </summary>
        internal double RoundRadius { get; set; }
    }

    /// <summary>
    /// Specifies the properties of the tooltip.
    /// </summary>
    public class TooltipCollection
    {
        /// <summary>
        /// Gets or sets the location of the tooltip.
        /// </summary>
#pragma warning disable SA1401
        internal PointF Location;
#pragma warning restore SA1401

        /// <summary>
        /// Gets or sets the rect value of the tooltip.
        /// </summary>
        internal Rect TooltipRect { get; set; } = new Rect();

        /// <summary>
        /// Gets or sets the stroke of the tooltip.
        /// </summary>
        internal string TooltipStroke { get; set; }

        /// <summary>
        /// Gets or sets the width of the tooltip.
        /// </summary>
        internal double TooltipStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the color of the tooltip.
        /// </summary>
        internal string TooltipColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the tooltip arrow is inverted.
        /// </summary>
        internal bool ArrowInverted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the tooltip type as range.
        /// </summary>
        internal bool IsRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the tooltip type as pointer.
        /// </summary>
        internal bool IsPointer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the tooltip type as annotation.
        /// </summary>
        internal bool IsAnnotation { get; set; }

        /// <summary>
        /// Gets or sets the start range of the tooltip.
        /// </summary>
        internal string RangeTooltipStart { get; set; }

        /// <summary>
        /// Gets or sets the format for the tooltip annotation.
        /// </summary>
        internal string AnnotationTooltipFormat { get; set; }

        /// <summary>
        /// Gets or sets the fill color for annotation tooltip.
        /// </summary>
        internal string AnnotationTooltipFill { get; set; }

        /// <summary>
        /// Gets or sets the stroke for annotation tooltip.
        /// </summary>
        internal string AnnotationTooltipStroke { get; set; }

        /// <summary>
        /// Gets or sets the width for annotation tooltip.
        /// </summary>
        internal double AnnotationTooltipStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the fill color for tooltip range.
        /// </summary>
        internal string RangeTooltipFill { get; set; }

        /// <summary>
        /// Gets or sets the fill color for tooltip range.
        /// </summary>
        internal string RangeTooltipStroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke width for tooltip range.
        /// </summary>
        internal double RangeTooltipStrokeWidth { get; set; }

        /// <summary>
        /// Gets or sets the value of the pointer.
        /// </summary>
        internal string PointerValue { get; set; }

        /// <summary>
        /// Gets or sets the font color of the pointer.
        /// </summary>
        internal string PointerFontColor { get; set; }

        /// <summary>
        /// Gets or sets the font family of the pointer.
        /// </summary>
        internal string PointerFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the style of the pointer.
        /// </summary>
        internal string PointerFontStyle { get; set; }

        /// <summary>
        /// Gets or sets the size of the pointer.
        /// </summary>
        internal string PointerFontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight of the pointer.
        /// </summary>
        internal string PointerFontWeight { get; set; }

        /// <summary>
        /// Gets or sets font opacity of the pointer.
        /// </summary>
        internal double PointerFontOpacity { get; set; }

        /// <summary>
        /// Gets or sets the font color of the range.
        /// </summary>
        internal string RangeFontColor { get; set; }

        /// <summary>
        /// Gets or sets the font family of the range.
        /// </summary>
        internal string RangeFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font style of the range.
        /// </summary>
        internal string RangeFontStyle { get; set; }

        /// <summary>
        /// Gets or sets the font size of the range.
        /// </summary>
        internal string RangeFontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight of the range.
        /// </summary>
        internal string RangeFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font opacity of the range.
        /// </summary>
        internal double RangeFontOpacity { get; set; }

        /// <summary>
        /// Gets or sets the font color of the annotation.
        /// </summary>
        internal string AnnotationFontColor { get; set; }

        /// <summary>
        /// Gets or sets the font family of the annotation.
        /// </summary>
        internal string AnnotationFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font style of the annotation.
        /// </summary>
        internal string AnnotationFontStyle { get; set; }

        /// <summary>
        /// Gets or sets the font size of the annotation.
        /// </summary>
        internal string AnnotationFontSize { get; set; }

        /// <summary>
        /// Gets or sets the font weight of the annotation.
        /// </summary>
        internal string AnnotationFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the font opacity of the annotation.
        /// </summary>
        internal double AnnotationFontOpacity { get; set; }
    }

    /// <summary>
    /// Specifies the client rect properties in circular gauge.
    /// </summary>
    public class BoundingClientRect
    {
        /// <summary>
        /// Gets or sets x value of the bounds in circular gauge.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y value of the bounds in circular gauge.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the bounds in circular gauge.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the bounds in circular gauge.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the top of the bounds in circular gauge.
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the bottom of the bounds in circular gauge.
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// Gets or sets the left of the bounds in circular gauge.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the right of the bounds in circular gauge.
        /// </summary>
        public double Right { get; set; }
    }

    /// <summary>
    /// Specifies the properties of the circle.
    /// </summary>
    public class Circle
    {
        /// <summary>
        /// Gets or sets the radius x value of the circular gauge.
        /// </summary>
        internal double RadiusX { get; set; }

        /// <summary>
        /// Gets or sets the radius y value of the circular gauge.
        /// </summary>
        internal double RadiusY { get; set; }

        /// <summary>
        /// Gets or sets the center x value of the circular gauge.
        /// </summary>
        internal float CenterX { get; set; }

        /// <summary>
        /// Gets or sets the center y value of the circular gauge.
        /// </summary>
        internal float CenterY { get; set; }
    }

    /// <summary>
    /// Specifies the interval of the axis.
    /// </summary>
    public class AxisInternal
    {
        /// <summary>
        /// Gets or sets the value of the pointer.
        /// </summary>
        internal List<double> PointerValue { get; set; }

        /// <summary>
        /// Gets or sets the start value of the range.
        /// </summary>
        internal List<double> RangeStart { get; set; }

        /// <summary>
        /// Gets or sets the end value of the range.
        /// </summary>
        internal List<double> RangeEnd { get; set; }

        /// <summary>
        /// Gets or sets the content of the annotation.
        /// </summary>
        internal List<string> AnnotationContent { get; set; }
    }

    /// <summary>
    /// Specifies the properties of the themes.
    /// </summary>
    public class ThemeStyle
    {
        /// <summary>
        /// Gets or sets background color of the axis.
        /// </summary>
        internal string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the cap.
        /// </summary>
        internal string CapColor { get; set; }

        /// <summary>
        /// Gets or sets the font family of the label text.
        /// </summary>
        internal string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font size of the label text.
        /// </summary>
        internal string FontSize { get; set; }

        /// <summary>
        /// Gets or sets the color of the label text in the axis.
        /// </summary>
        internal string LabelColor { get; set; }

        /// <summary>
        /// Gets or sets the font family of the label text in the axis.
        /// </summary>
        internal string LabelFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the color of the line in the axis.
        /// </summary>
        internal string LineColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the major tick line in the axis.
        /// </summary>
        internal string MajorTickColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the minor tick line in the axis.
        /// </summary>
        internal string MinorTickColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the needle.
        /// </summary>
        internal string NeedleColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the needle tail.
        /// </summary>
        internal string NeedleTailColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the pointer.
        /// </summary>
        internal string PointerColor { get; set; }

        /// <summary>
        /// Gets or sets the color for the title text.
        /// </summary>
        internal string TitleFontColor { get; set; }

        /// <summary>
        /// Gets or sets the fill color of the tooltip.
        /// </summary>
        internal string TooltipFillColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the tooltip.
        /// </summary>
        internal double TooltipFillOpacity { get; set; }

        /// <summary>
        /// Gets or sets the color of the tooltip text.
        /// </summary>
        internal string TooltipFontColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the tooltip text.
        /// </summary>
        internal double TooltipTextOpacity { get; set; }

        internal string TitleFontWeight { get; set; }

        /// <summary>
        /// Specifies the properties of the themes.
        /// </summary>
        /// <param name="theme">represent the name of the theme.</param>
        internal static ThemeStyle GetThemeStyle(Theme theme)
        {
            ThemeStyle themeStyle = null;
            switch (theme)
            {
                case Theme.MaterialDark:
                case Theme.FabricDark:
                case Theme.BootstrapDark:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "#333232",
                        CapColor = "#9A9A9A",
                        LabelColor = "#DADADA",
                        LineColor = "#C8C8C8",
                        MajorTickColor = "#C8C8C8",
                        MinorTickColor = "#9A9A9A",
                        NeedleColor = "#9A9A9A",
                        NeedleTailColor = "#9A9A9A",
                        PointerColor = "#9A9A9A",
                        TitleFontColor = "#ffffff",
                        TooltipFillColor = "#ffffff",
                        TooltipFontColor = "#000000",
                        FontFamily = "Segoe UI",
                        LabelFontFamily = "Segoe UI",
                        TitleFontWeight = "Normal",
                    };
                    break;
                case Theme.HighContrast:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "#000000",
                        CapColor = "#FFFFFF",
                        LabelColor = "#FFFFFF",
                        LineColor = "#FFFFFF",
                        MajorTickColor = "#FFFFFF",
                        MinorTickColor = "#FFFFFF",
                        NeedleColor = "#FFFFFF",
                        NeedleTailColor = "#FFFFFF",
                        PointerColor = "#FFFFFF",
                        TitleFontColor = "#FFFFFF",
                        TooltipFillColor = "#FFFFFF",
                        TooltipFontColor = "#000000",
                        FontFamily = "Segoe UI",
                        LabelFontFamily = "Segoe UI",
                        TitleFontWeight = "Normal",
                    };
                    break;
                case Theme.Bootstrap4:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "#FFFFFF",
                        CapColor = "#6C757D",
                        LabelColor = "#212529",
                        LineColor = "#DEE2E6",
                        MajorTickColor = "#ADB5BD",
                        MinorTickColor = "#CED4DA",
                        NeedleColor = "#6C757D",
                        NeedleTailColor = "#6C757D",
                        PointerColor = "#6C757D",
                        TitleFontColor = "#212529",
                        TooltipFillColor = "#000000",
                        TooltipFontColor = "#FFFFFF",
                        FontFamily = "HelveticaNeue-Medium",
                        LabelFontFamily = "HelveticaNeue",
                        FontSize = "15px",
                        TooltipFillOpacity = 1,
                        TooltipTextOpacity = 0.9,
                        TitleFontWeight = "Normal",
                    };
                    break;
                case Theme.Tailwind:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "rgba(255,255,255, 0.0)",
                        CapColor = "#1F2937",
                        LabelColor = "#6B7280",
                        LineColor = "#E5E7EB",
                        MajorTickColor = "#9CA3AF",
                        MinorTickColor = "#9CA3AF",
                        NeedleColor = "#1F2937",
                        NeedleTailColor = "#1F2937",
                        PointerColor = "#1F2937",
                        TitleFontColor = "#374151",
                        TooltipFillColor = "#111827",
                        TooltipFontColor = "#F9FAFB",
                        FontFamily = "Inter",
                        LabelFontFamily = "Inter",
                        FontSize = "14px",
                        TooltipFillOpacity = 1,
                        TooltipTextOpacity = 0.9,
                        TitleFontWeight = "500"
                    };
                    break;
                case Theme.TailwindDark:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "rgba(255,255,255, 0.0)",
                        CapColor = "#9CA3AF",
                        LabelColor = "#9CA3AF",
                        LineColor = "#374151",
                        MajorTickColor = "#6B7280",
                        MinorTickColor = "#6B7280",
                        NeedleColor = "#9CA3AF",
                        NeedleTailColor = "#9CA3AF",
                        PointerColor = "#9CA3AF",
                        TitleFontColor = "#D1D5DB",
                        TooltipFillColor = "#F9FAFB",
                        TooltipFontColor = "#1F2937",
                        FontFamily = "Inter",
                        LabelFontFamily = "Inter",
                        FontSize = "14px",
                        TooltipFillOpacity = 1,
                        TooltipTextOpacity = 0.9,
                        TitleFontWeight = "500",
                    };
                    break;
                case Theme.Bootstrap5:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "rgba(255,255,255, 0.0)",
                        CapColor = "#1F2937",
                        LabelColor = "#495057",
                        LineColor = "#E5E7EB",
                        MajorTickColor = "#9CA3AF",
                        MinorTickColor = "#9CA3AF",
                        NeedleColor = "#1F2937",
                        NeedleTailColor = "#1F2937",
                        PointerColor = "#1F2937",
                        TitleFontColor = "#343A40",
                        TooltipFillColor = "#212529",
                        TooltipFontColor = "#F9FAFB",
                        FontFamily = "Helvetica Neue",
                        LabelFontFamily = "Helvetica Neue",
                        FontSize = "14px",
                        TooltipFillOpacity = 1,
                        TooltipTextOpacity = 1,
                        TitleFontWeight = "500"
                    };
                    break;
                case Theme.Bootstrap5Dark:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "rgba(255,255,255, 0.0)",
                        CapColor = "#ADB5BD",
                        LabelColor = "#CED4DA",
                        LineColor = "#343A40",
                        MajorTickColor = "#6C757D",
                        MinorTickColor = "#6C757D",
                        NeedleColor = "#ADB5BD",
                        NeedleTailColor = "#ADB5BD",
                        PointerColor = "#ADB5BD",
                        TitleFontColor = "#E9ECEF",
                        TooltipFillColor = "#E9ECEF",
                        TooltipFontColor = "#212529",
                        FontFamily = "Helvetica Neue",
                        LabelFontFamily = "Helvetica Neue",
                        FontSize = "14px",
                        TooltipFillOpacity = 1,
                        TooltipTextOpacity = 1,
                        TitleFontWeight = "500"
                    };
                    break;
                default:
                    themeStyle = new ThemeStyle()
                    {
                        BackgroundColor = "#FFFFFF",
                        CapColor = "#757575",
                        LabelColor = "#212121",
                        LineColor = "#E0E0E0",
                        MajorTickColor = "#9E9E9E",
                        MinorTickColor = "#9E9E9E",
                        NeedleColor = "#757575",
                        NeedleTailColor = "#757575",
                        PointerColor = "#757575",
                        TitleFontColor = "#424242",
                        TooltipFillColor = "#363F4C",
                        TooltipFontColor = "#ffffff",
                        FontFamily = "Segoe UI",
                        LabelFontFamily = "Segoe UI",
                        TitleFontWeight = "Normal",
                    };
                    break;
            }

            return themeStyle;
        }

        internal static string[] GetRangePalette(Theme theme)
        {
            string[] palette;
            switch (theme)
            {
                case Theme.Tailwind:
                    palette = new string[] { "#0369A1", "#14B8A6", "#15803D", "#334155", "#5A61F6", "#65A30D", "#8B5CF6", "#9333EA", "#F59E0B", "#F97316" };
                    break;
                case Theme.TailwindDark:
                    palette = new string[] { "#10B981", "#22D3EE", "#2DD4BF", "#4ADE80", "#8B5CF6", "#E879F9", "#F472B6", "#F87171", "#F97316", "#FCD34D" };
                    break;
                case Theme.Bootstrap5:
                    palette = new string[] { "#262E0B", "#668E1F", "#AF6E10", "#862C0B", "#1F2D50", "#64680B", "#311508", "#4C4C81", "#0C7DA0", "#862C0B" };
                    break;
                case Theme.Bootstrap5Dark:
                    palette = new string[] { "#5ECB9B", "#A860F1", "#EBA844", "#557EF7", "#E9599B", "#BFC529", "#3BC6CF", "#7A68EC", "#74B706", "#EA6266" };
                    break;
                default:
                    palette = new string[] { "#50c917", "#27d5ff", "#fcde0b", "#ffb133", "#ff5985" };
                    break;
            }

            return palette;
        }
    }
}

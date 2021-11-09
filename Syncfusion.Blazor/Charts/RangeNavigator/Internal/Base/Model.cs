using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.RangeNavigator.Internal
{
    /// <summary>
    /// Specifies the slider information.
    /// </summary>
    public class SliderOptions
    {
        /// <summary>
        /// Specifies the transform.
        /// </summary>
        public string Transform { get; set; }

        /// <summary>
        /// Specifies the id of the element.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Specifies the shape of slider thump.
        /// </summary>
        public List<object> ShapeOptions { get; set; }

        /// <summary>
        /// Specifies the style.
        /// </summary>
        public string Style { get; set; }
    }

    /// <summary>
    /// Specifies the data point of the range navigator.
    /// </summary>
    public class DataPoint
    {
        internal object X { get; set; }

        internal object Y { get; set; }

        internal double XValue { get; set; }

        internal double YValue { get; set; }

        internal int Index { get; set; }

        internal bool Visible { get; set; }
    }

    internal class TooltipData
    {
        internal string Start { get; set; }

        internal string End { get; set; }

        internal string Value { get; set; }
    }

    internal class PeriodSelectorControl
    {
        internal double SeriesXMin { get; set; }

        internal double SeriesXMax { get; set; }

        internal double StartValue { get; set; }

        internal double EndValue { get; set; }

        internal RangeSlider RangeSlider { get; set; }

        internal bool DisableRangeSelector { get; set; }

        internal List<RangeNavigatorPeriod> Periods { get; set; } = new List<RangeNavigatorPeriod>();

        internal SfRangeNavigator RangeNavigatorControl { get; set; }
    }

    internal class RangeNavigatorStyle
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
}
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts
{
    public interface ILegend
    {
        public string Width { get; set; }
        public string Height { get; set; }
        public double Padding { get; set; }
        public double ShapeHeight { get; set; }
        public double ShapeWidth { get; set; }
        public double ShapePadding { get; set; }
        public LegendPosition Position { get; set; }
        public Alignment Alignment { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public double TabIndex { get; set; }
        public bool ToggleVisibility { get; set; }
    }

    public interface ILegendBaseMethods
    {
        public void GetRenderPoint(LegendOption legendOption, ChartInternalLocation start, double textPadding, LegendOption prevLegend, Rect rect, int count, int firstLegend);
        public void GetLegendBounds(Size availableSize, Rect legendBounds, Rect rect, Size maxLabelSize);
        public string ChartID { get; set; }
    }
}
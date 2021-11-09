using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Blazor.Charts
{
    public interface ILegendBase
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

        public double Opacity { get; set; }
    }

    public interface ILegendMethods
    {
        public void GetRenderPoint(LegendOption legendOption, ChartInternalLocation start, double textPadding, LegendOption prevLegend, int count, int firstLegend);

        public void GetLegendBounds(Size availableSize, Rect rect, Size maxLabelSize);
    }

    public class LegendSymbols
    {
        public SymbolOptions FirstSymbol { get; set; } = new SymbolOptions();

        public TextOptions TextOption { get; set; } = new TextOptions();

        public SymbolOptions SecondSymbol { get; set; }

        public int Index { get; set; }
    }
}

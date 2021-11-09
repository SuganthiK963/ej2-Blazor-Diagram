using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    public static class AccumulationChartConstants
    {
        internal const string GETCHARCOLLECTIONSIZE = "sfBlazor.getCharCollectionSize";
        internal const string INITAILELEMENTBOUNDSBYID = "sfBlazor.getElementBoundsById";
        internal const string ONTEXTRENDER = "OnDataLabelRender";
        internal const string ONPOINTRENDER = "OnPointRender";
        internal const string CHARTINTROP = "sfBlazor.AccumulationChart.";
        internal const string GETELEMENTBOUNDSBYID = CHARTINTROP + "getElementBoundsById";
        internal const string GETPARENTELEMENTBOUNDSBYID = CHARTINTROP + "getParentElementBoundsById";
        internal const string GETCHARSIZEBYFONT = CHARTINTROP + "getCharSizeByFontKeys";
        internal const string WIREEVENTS = CHARTINTROP + "wireEvents";
        internal const string UNWIREEVENTS = CHARTINTROP + "unWireEvents";
        internal const string RENDERTOOLTIP = CHARTINTROP + "renderTooltip";
        internal const string PERFORMANIMATION = CHARTINTROP + "performAnimation";
        internal const string SETATTRIBUTE = CHARTINTROP + "setElementAttribute";
        internal const string GETATTRIBUTE = CHARTINTROP + "getElementAttribute";
        internal const string PIEANIMATE = CHARTINTROP + "doAnimation";
        internal const string PIECHANGEPATH = CHARTINTROP + "ChangePiePath";
        internal const string ANIMATEREDRAWELEMENT = CHARTINTROP + "animateRedrawElement";
        internal const string LEGENDRENDER = "OnLegendItemRender";
        internal const string FADEOUT = CHARTINTROP + "fadeOut";
        internal const string SVG = "_svg";
        internal const string INITIALIZE = CHARTINTROP + "initialize";
        internal const string DESTROY = CHARTINTROP + "destroy";
        internal const string GETCHARSIZEBYFONTKEYS = "sfBlazor.getCharSizeByFontKeys";
        internal const string RTL = "rtl";
        internal const string CHARTINTROP_GETCHARCOLLECTIONSIZE = CHARTINTROP + "getCharCollectionSize";
    }

    public class AccumulationPoints
    {
        // For passing JS method Attributes
        public object X { get; set; }

        public double? Y { get; set; }

        public bool Visible { get; set; } = true;

        public string Text { get; set; }

        public string Tooltip { get; set; }

        public string SliceRadius { get; set; }

        public string OriginalText { get; set; }

        public string Label { get; set; }

        public string Color { get; set; }

        public double Percentage { get; set; }

        public ChartInternalLocation SymbolLocation { get; set; }

        public int Index { get; set; }

        public double MidAngle { get; set; }

        public double StartAngle { get; set; }

        public double EndAngle { get; set; }

        public double LabelAngle { get; set; }

        public Rect Region { get; set; }

        public Rect LabelRegion { get; set; }

        public bool LabelVisible { get; set; } = true;

        public AccumulationLabelPosition? LabelPosition { get; set; }

        public double YRatio { get; set; }

        public double HeightRatio { get; set; }

        public ChartInternalLocation LabelOffset { get; set; }

        public bool IsExplode { get; set; }

        public bool IsClubbed { get; set; }

        public bool IsSliced { get; set; }

        public double Start { get; set; }

        public double Degree { get; set; }

        public Rect InitialLabelRegion { get; set; }

        public double IsLabelUpdated { get; set; }

        public Size TextSize { get; set; }

        public AccumulationTextRenderEventArgs AccTextArgs { get; set; }

        public AccumulationPointRenderEventArgs AccPointArgs { get; set; }

        public string Id { get; set; }

        public string TemplateID { get; set; }

        public bool LegendVisible { get; set; } = true;
    }

    public class PropertyUpdate
    {
        internal bool RefreshElements { get; set; }

        internal bool RefreshBounds { get; set; }
    }

    public class AccPointData
    {
        internal AccumulationPoints Point { get; set; }

        internal AccumulationChartSeries Series { get; set; }

        internal double LierIndex { get; set; }

        internal AccPointData(AccumulationPoints point, AccumulationChartSeries series, double lierIndex = 0)
        {
            Point = point;
            Series = series;
            LierIndex = lierIndex;
        }
    }

    public class PieLegendAnimation
    {
        // For passing JS method Attributes
        public AccumulationPoints Point { get; set; }

        public double Degree { get; set; }

        public double Start { get; set; }

        public PathOptions PathOption { get; set; }

        public string PreDirection { get; set; }

        public string CurDirection { get; set; }

        public double Radius { get; set; }

        public double InnerRadius { get; set; }
    }
}
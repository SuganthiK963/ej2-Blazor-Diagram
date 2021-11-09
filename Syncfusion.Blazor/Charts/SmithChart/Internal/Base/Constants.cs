using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    internal static class SmithChartConstants
    {
        internal const string SMITHCHARTINTROP = "sfBlazor.SmithChart.";
        internal const string GETELEMENTBOUNDSBYID = SMITHCHARTINTROP + "getElementBoundsById";
        internal const string GETELEMENTBOUNDS = "sfBlazor.getElementBoundsById";
        internal const string DOLINEARANIMATION = SMITHCHARTINTROP + "doLinearAnimation";
        internal const string FADEOUT = SMITHCHARTINTROP + "fadeOut";
        internal const string GETTEMPLATESIZE = SMITHCHARTINTROP + "getTemplateSize";
        internal const string BORDERID = "_ChartBorder";
        internal const string RESIZED = "Resized";
        internal const string LOADED = "Loaded";
        internal const string SERIESCLIPRECTID = "_ChartSeriesClipRect_";
        internal const string GETCHARSIZEBYFONT = SMITHCHARTINTROP + "getCharSizeByFontKeys";
        internal const string GETCHARCOLLECTIONSIZE = "sfBlazor.getCharCollectionSize";
        internal const string RENDERTOOLTIP = SMITHCHARTINTROP + "renderTooltip";
        internal const string NUMPATTERN = @"[^0-9\.]+";
    }
}
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The interface specifies the properties of the accumulation chart component.
    /// </summary>
    public interface IAccumulationChart
    {
        /// <summary>
        ///  Sets and gets the id for the accumulation chart component.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The background color of the chart, which accepts value in hex, rgba as a valid CSS color string.
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// The background image of the chart that accepts value in string as url link or location of an image.
        /// </summary>
        public string BackgroundImage { get; set; }

        /// <summary>
        /// Specifies the dataSource for the AccumulationChart.
        /// </summary>
        public IEnumerable<object> DataSource { get; set; }

        /// <summary>
        /// If set true, enables the animation for both chart and accumulation.
        /// </summary>
        public bool EnableAnimation { get; set; }

        /// <summary>
        /// If set true, enables the border in pie and accumulation chart while mouse moving.
        /// </summary>
        public bool EnableBorderOnMouseMove { get; set; }

        /// <summary>
        /// If set true, labels for the point will be placed smartly without overlapping.
        /// </summary>
        public bool EnableSmartLabels { get; set; }

        /// <summary>
        /// Specifies the height for the AccumulationChart.
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Specifies whether point has to get highlighted or not.
        /// </summary>
        public AccumulationSelectionMode HighlightMode { get; set; }

        /// <summary>
        /// Specifies whether series or data point has to be selected.
        /// </summary>
        public SelectionPattern HighlightPattern { get; set; }

        /// <summary>
        /// If set true, enables the multi selection in accumulation chart.
        /// </summary>
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// Specifies whether point has to get selected or not.
        /// </summary>
        public AccumulationSelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Specifies whether series or data point for accumulation chart has to be selected.
        /// </summary>
        public SelectionPattern SelectionPattern { get; set; }

        /// <summary>
        /// Sets and gets the subTitle for accumulation chart.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Specifies the theme for accumulation chart.
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// Sets and gets title for accumulation chart.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Specifies whether a grouping separator should be used for a number.
        /// </summary>
        public bool EnableGroupingSeparator { get; set; }

        /// <summary>
        /// Specifies the width for the AccumulationChart.
        /// </summary>
        public string Width { get; set; }
#pragma warning disable CA2227
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<AccumulationChartAnnotation> Annotations { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<AccumulationChartSeries> Series { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<AccumulationChartSelectedDataIndex> SelectedDataIndexes { get; set; }
#pragma warning restore CA2227
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartLegendSettings LegendSettings { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartMargin Margin { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartSubTitleStyle SubTitleStyle { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartTitleStyle TitleStyle { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartTooltipSettings Tooltip { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartEvents AccumulationChartEvents { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ChartHelper ChartHelper { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Task OnAccumulationChartParametersSet();

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object chartItems, object parent = null);

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartBorder Border { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccumulationChartCenter Center { get; set; }
    }

    public class AccumulationChartDataPointInfo
    {
        /// <summary>
        /// Sets and gets accumulation point color.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Sets and gets the accumulation point percentage value.
        /// </summary>
        public double Percentage { get; set; }

        /// <summary>
        /// Sets and gets accumulation x point value.
        /// </summary>
        public object X { get; set; }

        /// <summary>
        /// Sets and gets accumulation Y point value.
        /// </summary>
        public object Y { get; set; }

        /// <summary>
        /// Sets and gets accumulation pointInformation.
        /// </summary>
        public PointInfo Data { get; set; }

        internal AccumulationChartDataPointInfo(object x, object y, double percent, string label, PointInfo data = null)
        {
            X = x;
            Y = y;
            Percentage = percent;
            Label = label;
            Data = data;
        }
    }
}
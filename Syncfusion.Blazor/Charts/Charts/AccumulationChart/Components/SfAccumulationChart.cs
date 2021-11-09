using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Data;
using System;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Accumulation chart is a Data visualization component which is divided into slices to represent the given numerical values.
    /// In this each slice represents its corresponding numerical value.
    /// </summary>
    public partial class SfAccumulationChart : SfDataBoundComponent, IAccumulationChart
    {
        internal RenderFragment AnnotationContent { get; set; }

        internal RenderFragment DatalabelTemplate { get; set; }

        internal RenderFragment TooltipContent { get; set; }

        internal RenderFragment TrimTooltip { get; set; }

        internal RenderFragment StyleFragment { get; set; }

        internal RenderFragment ChartContent { get; set; }

        internal double MouseX { get; set; }

        internal double MouseY { get; set; }

        internal bool animateSeries { get; set; } = true;

        internal List<DynamicAccTextAnimationOptions> AnimateTextElements { get; set; } = new List<DynamicAccTextAnimationOptions>();

        private List<string> fontKeys { get; set; } = new List<string>();

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool AllowExport { get; set; }

        /// <summary>
        /// The background color of the chart, which accepts data in hex, rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background { get; set; }

        private string background { get; set; }

        /// <summary>
        /// The background image of the chart that accepts data in string as url link or location of an image.
        /// </summary>
        [Parameter]
        public string BackgroundImage { get; set; }

        private string backgroundImage { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartBorder Border { get; set; } = new AccumulationChartBorder();

        private AccumulationChartBorder border { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartCenter Center { get; set; } = new AccumulationChartCenter();

        private AccumulationChartCenter center { get; set; }

        /// <summary>
        /// Specifies the dataSource for the AccumulationChart. It can be an array of JSON objects or an instance of DataManager.
        ///
        /// </summary>
        [Parameter]
        public IEnumerable<object> DataSource { get; set; }

        private IEnumerable<object> dataSource { get; set; }

        /// <summary>
        /// Specifies Query to select data from dataSource. This property is applicable only when the dataSource is `Ej.DataManager`.
        /// </summary>
        [Parameter]
        public Query Query { get; set; }

        private Query query { get; set; }

        /// <summary>
        /// Triggers when dataSource for the chart changed.
        /// </summary>
        [Parameter]
        public EventCallback<object> DataSourceChanged { get; set; }

        /// <summary>
        /// If set true, enables the animation for accumulation chart.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        private bool enableAnimation { get; set; }

        /// <summary>
        /// If set true, enables the border in pie chart while mouse moving.
        /// </summary>
        [Parameter]
        public bool EnableBorderOnMouseMove { get; set; } = true;

        private bool enableBorderOnMouseMove { get; set; }

        /// <summary>
        /// If set true, labels for the point will be placed smartly without overlapping.
        /// </summary>
        [Parameter]
        public bool EnableSmartLabels { get; set; } = true;

        private bool enableSmartLabels { get; set; }

        /// <summary>
        /// The height of the chart as a string in order to provide input as both like '100px' or '100%'.
        /// If specified as '100%, chart will render to the full height of its parent element.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "100%";

        private string height { get; set; }

        /// <summary>
        /// Specifies whether point has to get highlighted or not. Takes chartItems either 'None 'or 'Point'.
        /// </summary>
        [Parameter]
        public AccumulationSelectionMode HighlightMode { get; set; }

        private AccumulationSelectionMode highlightMode { get; set; }

        /// <summary>
        /// Specifies whether series or data point has to be selected. They are,
        /// none: sets none as highlighting pattern to accumulation chart.
        /// chessboard: sets chess board as highlighting pattern to accumulation chart.
        /// dots: sets dots as highlighting pattern to accumulation chart.
        /// diagonalForward: sets diagonal forward as highlighting pattern to accumulation chart.
        /// crosshatch: sets crosshatch as highlighting pattern to accumulation chart.
        /// pacman: sets pacman highlighting  pattern to accumulation chart.
        /// diagonalbackward: sets diagonal backward as highlighting pattern to accumulation chart.
        /// grid: sets grid as highlighting pattern to accumulation chart.
        /// turquoise: sets turquoise as highlighting pattern to accumulation chart.
        /// star: sets star as highlighting  pattern to accumulation chart.
        /// triangle: sets triangle as highlighting pattern to accumulation chart.
        /// circle: sets circle as highlighting  pattern to accumulation chart.
        /// tile: sets tile as highlighting pattern to accumulation chart.
        /// horizontaldash: sets horizontal dash as highlighting pattern to accumulation chart.
        /// verticaldash: sets vertical dash as highlighting pattern to accumulation chart.
        /// rectangle: sets rectangle as highlighting  pattern to accumulation chart.
        /// box: sets box as highlighting pattern to accumulation chart.
        /// verticalstripe: sets vertical stripe as highlighting  pattern to accumulation chart.
        /// horizontalstripe: sets horizontal stripe as highlighting  pattern to accumulation chart.
        /// bubble: sets bubble as highlighting  pattern to accumulation chart.
        /// </summary>
        [Parameter]
        public SelectionPattern HighlightPattern { get; set; }

        private SelectionPattern highlightPattern { get; set; }

        /// <summary>
        /// If set true, enables the multi selection in accumulation chart. It requires `SelectionMode` to be `Point`.
        /// </summary>
        [Parameter]
        public bool IsMultiSelect { get; set; }

        private bool isMultiSelect { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartLegendSettings LegendSettings { get; set; } = new AccumulationChartLegendSettings();

        private AccumulationChartLegendSettings legendSettings { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartMargin Margin { get; set; } = new AccumulationChartMargin();

        private AccumulationChartMargin margin { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
#pragma warning disable CA2227
        public List<AccumulationChartSelectedDataIndex> SelectedDataIndexes { get; set; } = new List<AccumulationChartSelectedDataIndex>();

        private List<AccumulationChartSelectedDataIndex> selectedDataIndexes { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public List<AccumulationChartAnnotation> Annotations { get; set; } = new List<AccumulationChartAnnotation>();

        private List<AccumulationChartAnnotation> annotations { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public List<AccumulationChartSeries> Series { get; set; } = new List<AccumulationChartSeries>();

        private List<AccumulationChartSeries> series { get; set; }
#pragma warning restore CA2227

        /// <summary>
        /// Specifies whether point has to get selected or not. Takes chartItems either 'None 'or 'Point'.
        /// </summary>
        [Parameter]
        public AccumulationSelectionMode SelectionMode { get; set; }

        private AccumulationSelectionMode selectionMode { get; set; }

        /// <summary>
        /// Specifies whether series or data point for accumulation chart has to be selected. They are,
        /// none: sets none as selecting pattern to accumulation chart .
        /// chessboard: sets chess board as selecting pattern accumulation chart .
        /// dots: sets dots as  selecting pattern accumulation chart .
        /// diagonalForward: sets diagonal forward as selecting pattern to accumulation chart .
        /// crosshatch: sets crosshatch as selecting pattern to accumulation chart.
        /// pacman: sets pacman selecting pattern to accumulation chart.
        /// diagonalbackward: sets diagonal backward as selecting pattern to accumulation chart.
        /// grid: sets grid as selecting pattern to accumulation chart.
        /// turquoise: sets turquoise as selecting pattern to accumulation chart.
        /// star: sets star as selecting pattern to accumulation chart.
        /// triangle: sets triangle as selecting pattern to accumulation chart.
        /// circle: sets circle as selecting pattern to accumulation chart.
        /// tile: sets tile as selecting pattern to accumulation chart.
        /// horizontaldash: sets horizontal dash as selecting pattern to accumulation chart.
        /// verticaldash: sets vertical dash as selecting pattern to accumulation chart.
        /// rectangle: sets rectangle as selecting pattern.
        /// box: sets box as selecting pattern to accumulation chart.
        /// verticalstripe: sets vertical stripe as  selecting pattern to accumulation chart.
        /// horizontalstripe: sets horizontal stripe as selecting pattern to accumulation chart.
        /// bubble: sets bubble as selecting pattern to accumulation chart.
        /// </summary>
        [Parameter]
        public SelectionPattern SelectionPattern { get; set; }

        private SelectionPattern selectionPattern { get; set; }

        /// <summary>
        /// Specifies the SubTitle of the accumulation chart.
        /// </summary>
        [Parameter]
        public string SubTitle { get; set; }

        private string subTitle { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartSubTitleStyle SubTitleStyle { get; set; } = new AccumulationChartSubTitleStyle();

        private AccumulationChartSubTitleStyle subTitleStyle { get; set; }

        /// <summary>
        /// Specifies the theme for accumulation chart.
        /// </summary>
        [Parameter]
        public Theme Theme { get; set; } = Theme.Bootstrap4;

        private Theme theme { get; set; }

        /// <summary>
        /// Specifies the Title of the accumulation chart.
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        private string title { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartTitleStyle TitleStyle { get; set; } = new AccumulationChartTitleStyle();

        private AccumulationChartTitleStyle titleStyle { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartTooltipSettings Tooltip { get; set; } = new AccumulationChartTooltipSettings();

        private AccumulationChartTooltipSettings tooltip { get; set; }

        /// <summary>
        /// Specifies whether a grouping separator should be used for a number.
        /// </summary>
        [Parameter]
        public bool EnableGroupingSeparator { get; set; }

        /// <summary>
        /// The width of the chart as a string in order to provide input as both like '100px' or '100%'.
        /// If specified as '100%, chart will render to the full width of its parent element.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        private string width { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public AccumulationChartEvents AccumulationChartEvents { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnableCanvas { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnableExport { get; set; }

        /// <summary>
        /// Sets id attribute for chart element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Sets content for chart element including HTML and its customizations.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the chart element.
        /// </summary>
        [Parameter]
        public string CustomClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable/ Disable the chart element render from right to left.
        /// </summary>
        [Parameter]
        public bool EnableRTL { get; set; }

        private void SetAccFontKeys()
        {
            List<string> keys = new List<string>();
            keys.Add(TitleStyle.GetFontKey());
            keys.Add(SubTitleStyle.GetFontKey());
            keys.Add(Series.First().DataLabel.Font.GetFontKey());
            keys.Add(LegendSettings.TextStyle.GetFontKey());
            fontKeys = keys.Distinct().ToList();
        }

        /// <summary>
        /// The method is used to get the container div direction.
        /// </summary>
        private string GetDirection()
        {
            return EnableRTL ? AccumulationChartConstants.RTL : string.Empty;
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object chartItems, object parent = null)
        {
            switch (key)
            {
                case nameof(LegendSettings):
                    LegendSettings = legendSettings = (AccumulationChartLegendSettings)chartItems;
                    break;
                case nameof(Margin):
                    Margin = margin = (AccumulationChartMargin)chartItems;
                    break;
                case nameof(TitleStyle):
                    TitleStyle = titleStyle = (AccumulationChartTitleStyle)chartItems;
                    break;
                case nameof(SubTitleStyle):
                    SubTitleStyle = subTitleStyle = (AccumulationChartSubTitleStyle)chartItems;
                    break;
                case nameof(Center):
                    Center = center = (AccumulationChartCenter)chartItems;
                    break;
                case nameof(Tooltip):
                    Tooltip = tooltip = (AccumulationChartTooltipSettings)chartItems;
                    break;
                case nameof(Annotations):
                    Annotations = annotations = (List<AccumulationChartAnnotation>)chartItems;
                    break;
                case nameof(SelectedDataIndexes):
                    SelectedDataIndexes = selectedDataIndexes = (List<AccumulationChartSelectedDataIndex>)chartItems;
                    break;
                case nameof(Border):
                    Border = border = (AccumulationChartBorder)chartItems;
                    break;
                default:
                    if (key == nameof(Series))
                    {
                        Series = series = (List<AccumulationChartSeries>)chartItems;
                    }
                    else if (parent != null)
                    {
                        ((AccumulationChartSeries)parent).UpdateSeriesProperties(key, chartItems);
                    }

                    break;
            }
        }
    }
}
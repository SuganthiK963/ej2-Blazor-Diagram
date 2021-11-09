using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json.Serialization;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Specialized;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Series represents the collection of data in the Accumulation chart.
    /// Gets and set the Series of the Accumulation series.
    /// </summary>
    public partial class AccumulationChartSeries
    {
        [CascadingParameter]
        private AccumulationChartSeriesCollection seriesCollection { get; set; }

        [CascadingParameter]
        private SfAccumulationChart accumulationChart { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Options for customizing the animation for series.
        /// </summary>
        [Parameter]
        public AccumulationChartAnimation Animation { get; set; } = new AccumulationChartAnimation();

        private AccumulationChartAnimation animation { get; set; }

        /// <summary>
        /// Options for customizing the border of the series.
        /// </summary>
        [Parameter]
        public AccumulationChartSeriesBorder Border { get; set; } = new AccumulationChartSeriesBorder();

        private AccumulationChartSeriesBorder border { get; set; }

        /// <summary>
        /// The data label for the series.
        /// </summary>
        [Parameter]
        public AccumulationDataLabelSettings DataLabel { get; set; } = new AccumulationDataLabelSettings();

        private AccumulationDataLabelSettings dataLabel { get; set; }

        /// <summary>
        /// Specifies the dataSource for the series. It can be an array of JSON objects or an instance of DataManager.
        /// </summary>
        [Parameter]
        public IEnumerable<object> DataSource { get; set; }

        private IEnumerable<object> dataSource { get; set; }

        /// <summary>
        /// Options to customize the empty points in series.
        /// </summary>
        [Parameter]
        public AccumulationChartEmptyPointSettings EmptyPointSettings { get; set; } = new AccumulationChartEmptyPointSettings();

        private AccumulationChartEmptyPointSettings emptyPointSettings { get; set; }

        /// <summary>
        /// To enable or disable tooltip for a series.
        /// </summary>
        [Parameter]
        public bool EnableTooltip { get; set; } = true;

        private bool enableTooltip { get; set; }

        /// <summary>
        /// End angle for a series.
        /// </summary>
        [Parameter]
        public double EndAngle { get; set; } = double.NaN;

        private double endAngle { get; set; }

        /// <summary>
        /// If set true, series points will be exploded on mouse click or touch.
        /// </summary>
        [Parameter]
        public bool Explode { get; set; }

        private bool explode { get; set; }

        /// <summary>
        /// If set true, all the points in the series will get exploded on load.
        /// </summary>
        [Parameter]
        public bool ExplodeAll { get; set; }

        private bool explodeAll { get; set; }

        /// <summary>
        /// Index of the point, to be exploded on load.
        /// </summary>
        [Parameter]
        public double ExplodeIndex { get; set; } = double.NaN;

        private double explodeIndex { get; set; }

        /// <summary>
        /// Distance of the point from the center, which takes values in both pixels and percentage.
        /// </summary>
        [Parameter]
        public string ExplodeOffset { get; set; } = "30%";

        private string explodeOffset { get; set; }

        /// <summary>
        /// Defines the distance between the segments of a funnel/pyramid series. The range will be from 0 to 1.
        /// </summary>
        [Parameter]
        public double GapRatio { get; set; }

        private double gapRatio { get; set; }

        /// <summary>
        /// AccumulationSeries y values less than groupMode are combined into single slice named others.
        /// </summary>
        [Parameter]
        public GroupMode GroupMode { get; set; } = GroupMode.Value;

        private GroupMode groupMode { get; set; }

        /// <summary>
        /// AccumulationSeries y values less than groupTo are combined into single slice named others.
        /// </summary>
        [Parameter]
        public string GroupTo { get; set; } = string.Empty;

        private string groupTo { get; set; }

        /// <summary>
        /// Defines the height of the funnel/pyramid with respect to the chart area.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "80%";

        private string height { get; set; }

        /// <summary>
        /// When the innerRadius value is greater than 0 percentage, a donut will appear in pie series. It takes values only in percentage.
        /// </summary>
        [Parameter]
        public string InnerRadius { get; set; } = "0";

        private string innerRadius { get; set; }

        /// <summary>
        /// The shape of the legend. Each series has its own legend shape. They are
        /// Circle - Renders a circle.
        /// Rectangle - Renders a rectangle.
        /// Triangle - Renders a triangle.
        /// Diamond - Renders a diamond.
        /// Cross - Renders a cross.
        /// HorizontalLine - Renders a horizontalLine.
        /// VerticalLine - Renders a verticalLine.
        /// Pentagon - Renders a pentagon.
        /// InvertedTriangle - Renders a invertedTriangle.
        /// SeriesType -Render a legend shape based on series type.
        /// </summary>
        [Parameter]
        public LegendShape LegendShape { get; set; } = LegendShape.SeriesType;

        private LegendShape legendShape { get; set; }

        /// <summary>
        /// Specifies the series name.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        private string name { get; set; }

        /// <summary>
        /// Defines the height of the funnel neck with respect to the chart area.
        /// </summary>
        [Parameter]
        public string NeckHeight { get; set; } = "20%";

        private string neckHeight { get; set; }

        /// <summary>
        /// Defines the width of the funnel neck with respect to the chart area.
        /// </summary>
        [Parameter]
        public string NeckWidth { get; set; } = "20%";

        private string neckWidth { get; set; }

        /// <summary>
        /// The opacity of the series.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        private double opacity { get; set; }

        /// <summary>
        /// Palette for series points.
        /// </summary>
        [Parameter]
#pragma warning disable CA1819
        public string[] Palettes { get; set; } = Array.Empty<string>();

#pragma warning restore CA1819
        private string[] palettes { get; set; }

        /// <summary>
        /// The DataSource field that contains the color value of point.
        /// It is applicable for series.
        /// </summary>
        [Parameter]
        public string PointColorMapping { get; set; } = string.Empty;

        private string pointColorMapping { get; set; }

        /// <summary>
        /// Defines how the values have to be reflected, whether through height/surface of the segments.
        /// </summary>
        [Parameter]
        public PyramidMode PyramidMode { get; set; }

        private PyramidMode pyramidMode { get; set; }

        /// <summary>
        /// Specifies Query to select data from dataSource. This property is applicable only when the dataSource is `Ej.DataManager`.
        /// </summary>
        [Parameter]
        public Query Query { get; set; } = new Query();

        private Query query { get; set; }

        /// <summary>
        /// Radius of the pie series and its values in percentage.
        /// </summary>
        [Parameter]
        public string Radius { get; set; } = "80%";

        private string radius { get; set; }

        /// <summary>
        /// Custom style for the selected series or points.
        /// </summary>
        [Parameter]
        public string SelectionStyle { get; set; } = string.Empty;

        private string selectionStyle { get; set; }

        /// <summary>
        /// Sets and gets the Start angle for a series.
        /// </summary>
        [Parameter]
        public double StartAngle { get; set; }

        private double startAngle { get; set; }

        /// <summary>
        /// The provided value will be considered as a tooltip Mapping name.
        /// </summary>
        [Parameter]
        public string TooltipMappingName { get; set; } = string.Empty;

        private string tooltipMappingName { get; set; }

        /// <summary>
        /// Specify the type of the series in accumulation chart.
        /// </summary>
        [Parameter]
        public AccumulationType Type { get; set; }

        private AccumulationType type { get; set; }

        /// <summary>
        /// Specifies the series visibility.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        private bool visible { get; set; }

        /// <summary>
        /// Defines the width of the funnel/pyramid with respect to the chart area.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "80%";

        private string width { get; set; }

        /// <summary>
        /// The DataSource field which contains the x value.
        /// </summary>
        [Parameter]
        public string XName { get; set; } = string.Empty;

        private string xName { get; set; }

        /// <summary>
        /// The DataSource field which contains the y value.
        /// </summary>
        [Parameter]
        public string YName { get; set; } = string.Empty;

        private string yName { get; set; }

        private bool isObservableWired { get; set; }

        private string dataSourceType { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            if (DataSource != null && DataManager == null)
            {
                SetDataManager<object>(DataSource);
                dataSourceType = DataSource.GetType().Name;
            }
            else if (accumulationChart.DataSource != null)
            {
                SetDataManager<object>(accumulationChart.DataSource);
                dataSourceType = accumulationChart.DataSource.GetType().Name;
            }

            seriesCollection.SeriesCollection.Add(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            SfAccumulationChart ChartInstance = (SfAccumulationChart)accumulationChart;
            if (DataManager != null)
            {
                DataModule = (IEnumerable<object>)await DataManager.ExecuteQuery<object>(Query);
            }
            else if (ChartInstance.DataManager != null)
            {
                DataModule = (IEnumerable<object>)await ChartInstance.DataManager.ExecuteQuery<object>(Query);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!object.ReferenceEquals(DataSource, dataSource))
            {
                UpdateObservableEventsForObject(nameof(DataSource), dataSource, true);
                UpdateObservableEventsForObject(nameof(DataSource), DataSource);
            }

            await base.OnParametersSetAsync();
            animation = Animation = NotifyPropertyChanges(nameof(Animation), Animation, animation);
            border = Border = NotifyPropertyChanges(nameof(Border), Border, border);
            dataLabel = DataLabel = NotifyPropertyChanges(nameof(DataLabel), DataLabel, dataLabel);
            dataSource = DataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, dataSource);
            emptyPointSettings = EmptyPointSettings = NotifyPropertyChanges(nameof(EmptyPointSettings), EmptyPointSettings, emptyPointSettings);
            enableTooltip = EnableTooltip = NotifyPropertyChanges(nameof(EnableTooltip), EnableTooltip, enableTooltip);
            endAngle = EndAngle = NotifyPropertyChanges(nameof(EndAngle), EndAngle, endAngle);
            explode = Explode = NotifyPropertyChanges(nameof(Explode), Explode, explode);
            explodeAll = ExplodeAll = NotifyPropertyChanges(nameof(ExplodeAll), ExplodeAll, explodeAll);
            explodeIndex = ExplodeIndex = NotifyPropertyChanges(nameof(ExplodeIndex), ExplodeIndex, explodeIndex);
            explodeOffset = ExplodeOffset = NotifyPropertyChanges(nameof(ExplodeOffset), ExplodeOffset, explodeOffset);
            gapRatio = GapRatio = NotifyPropertyChanges(nameof(GapRatio), GapRatio, gapRatio);
            groupMode = GroupMode = NotifyPropertyChanges(nameof(GroupMode), GroupMode, groupMode);
            groupTo = GroupTo = NotifyPropertyChanges(nameof(GroupTo), GroupTo, groupTo);
            height = Height = NotifyPropertyChanges(nameof(Height), Height, height);
            innerRadius = InnerRadius = NotifyPropertyChanges(nameof(InnerRadius), InnerRadius, innerRadius);
            legendShape = LegendShape = NotifyPropertyChanges(nameof(LegendShape), LegendShape, legendShape);
            name = Name = NotifyPropertyChanges(nameof(Name), Name, name);
            neckWidth = NeckWidth = NotifyPropertyChanges(nameof(NeckWidth), NeckWidth, neckWidth);
            neckHeight = NeckHeight = NotifyPropertyChanges(nameof(NeckHeight), NeckHeight, neckHeight);
            opacity = Opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            palettes = Palettes = NotifyPropertyChanges(nameof(Palettes), Palettes, palettes);
            pointColorMapping = PointColorMapping = NotifyPropertyChanges(nameof(PointColorMapping), PointColorMapping, pointColorMapping);
            pyramidMode = PyramidMode = NotifyPropertyChanges(nameof(PyramidMode), PyramidMode, pyramidMode);
            query = Query = NotifyPropertyChanges(nameof(Query), Query, query);
            radius = Radius = NotifyPropertyChanges(nameof(Radius), Radius, radius);
            selectionStyle = SelectionStyle = NotifyPropertyChanges(nameof(SelectionStyle), SelectionStyle, selectionStyle);
            startAngle = StartAngle = NotifyPropertyChanges(nameof(StartAngle), StartAngle, startAngle);
            tooltipMappingName = TooltipMappingName = NotifyPropertyChanges(nameof(TooltipMappingName), TooltipMappingName, tooltipMappingName);
            type = Type = NotifyPropertyChanges(nameof(Type), Type, type);
            visible = Visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            width = Width = NotifyPropertyChanges(nameof(Width), Width, width);
            xName = XName = NotifyPropertyChanges(nameof(XName), XName, xName);
            yName = YName = NotifyPropertyChanges(nameof(YName), YName, yName);
            if (DataSource != null)
            {
                isObservableWired = DataVizCommonHelper.BindObservable(this, nameof(DataSource), DataSource, isObservableWired);
                var propKeys = PropertyChanges?.Keys.ToList();
                foreach (string property in propKeys)
                {
                    if (property == "DataSource")
                    {
                        SetDataManager<object>(DataSource);
                        DataModule = (IEnumerable<object>)await DataManager.ExecuteQuery<object>(Query);
                    }
                }
            }

            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)accumulationChart).PropertyChanges.TryAdd(nameof(IAccumulationChart.Series), this);
                await accumulationChart.OnAccumulationChartParametersSet();
                PropertyChanges.Clear();
            }
        }

        internal async Task SeriesPropertyChanged()
        {
            await OnParametersSetAsync();
#pragma warning restore CA2007
        }

        internal void UpdateSeriesProperties(string key, object seriesItems)
        {
            if (key == nameof(DataLabel))
            {
                dataLabel = DataLabel = (AccumulationDataLabelSettings)seriesItems;
            }
            else if (key == nameof(Animation))
            {
                animation = Animation = (AccumulationChartAnimation)seriesItems;
            }
            else if (key == nameof(Border))
            {
                border = Border = (AccumulationChartSeriesBorder)seriesItems;
            }
            else if (key == nameof(EmptyPointSettings))
            {
                emptyPointSettings = EmptyPointSettings = (AccumulationChartEmptyPointSettings)seriesItems;
            }
        }

        internal override void ComponentDispose()
        {
            accumulationChart = null;
            ChildContent = null;
            DataLabel = null;
            Animation = null;
            Border = null;
            EmptyPointSettings = null;
        }

        public override void Dispose()
        {
            UpdateObservableEventsForObject(nameof(DataSource), DataSource, true);
        }

        protected override void OnObservableChange(string propertyName, object sender, bool isCollectionChanged = false, NotifyCollectionChangedEventArgs e = null)
        {
            if (PropertyChanges.ContainsKey("DataSource") && !IsDisposed)
            {
                try
                {
                    DataModule = (IEnumerable<object>)DataSource;
                    accumulationChart.Refresh(false);
                }
                catch
                {
                    if (!IsDisposed)
                    {
                        throw;
                    }
                }

                PropertyChanges.Remove("DataSource");
            }
        }
    }
}
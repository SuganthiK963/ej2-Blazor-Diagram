using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.SmithChart.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the series of the chart.
    /// </summary>
    public partial class SmithChartSeries : SfDataBoundComponent
    {
        private string reactance;
        private string resistance;
        private string tooltipMappingName;
        private double width;
        private double animationDuration;
        private IEnumerable<object> dataSource;
        private bool enableAnimation;
        private bool enableSmartLabels;
        private string fill;
        private string name;
        private IEnumerable<SmithChartPoint> points;
        private double opacity;
        private bool visible;

        [CascadingParameter]
        internal SmithChartSeriesCollection Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Perform animation of series based on animation duration.
        /// </summary>
        [Parameter]
        public double AnimationDuration { get; set; } = 2000;

        /// <summary>
        /// Specifies the data source.
        /// </summary>
        [Parameter]
        public IEnumerable<object> DataSource { get; set; }

        /// <summary>
        /// Enable or disable the animation of series.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; }

        /// <summary>
        /// Avoid the overlap of data labels.
        /// </summary>
        [Parameter]
        public bool EnableSmartLabels { get; set; }

        /// <summary>
        /// Color for series.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// The name of the series visible in legend.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Opacity for series.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Points for series.
        /// </summary>
        [Parameter]
        public IEnumerable<SmithChartPoint> Points { get; set; }

        /// <summary>
        /// Reactance name from data source.
        /// </summary>
        [Parameter]
        public string Reactance { get; set; } = string.Empty;

        /// <summary>
        /// Resistance name from data source.
        /// </summary>
        [Parameter]
        public string Resistance { get; set; } = string.Empty;

        /// <summary>
        /// Tooltip mapping name for the series.
        /// </summary>
        [Parameter]
        public string TooltipMappingName { get; set; } = string.Empty;

        /// <summary>
        /// Visibility of the series.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Width of the series.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        internal LabelOption LabelOption { get; set; } = new LabelOption();

        internal int Index { get; set; }

        internal string Interior { get; set; }

        internal List<SmithChartPoint> ActualPoints { get; set; } = new List<SmithChartPoint>();

        internal IEnumerable<object> CurrentViewData { get; set; }

        internal SmithChartSeriesTooltip Tooltip { get; set; } = new SmithChartSeriesTooltip();

        internal SmithChartSeriesMarker Marker { get; set; } = new SmithChartSeriesMarker();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (DataSource != null)
            {
                CurrentViewData = await UpdatedSeriesData();
            }

            Parent.Series.Add(this);
        }

        internal async Task<IEnumerable<object>> UpdatedSeriesData()
        {
            if (DataManager == null)
            {
                SetDataManager<object>(DataSource);
            }

            DataManager?.DataAdaptor.SetRunSyncOnce(true);
            return (IEnumerable<object>)await DataManager.ExecuteQuery<object>(new Data.Query());
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            animationDuration = NotifyPropertyChanges(nameof(AnimationDuration), AnimationDuration, animationDuration);
            UpdateObservableEvents(nameof(DataSource), DataSource);
            dataSource = await UpdateProperty(nameof(DataSource), DataSource, dataSource);
            enableAnimation = NotifyPropertyChanges(nameof(EnableAnimation), EnableAnimation, enableAnimation);
            enableSmartLabels = NotifyPropertyChanges(nameof(EnableSmartLabels), EnableSmartLabels, enableSmartLabels);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            name = NotifyPropertyChanges(nameof(Name), Name, name);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            UpdateObservableEvents(nameof(Points), Points);
            points = NotifyPropertyChanges(nameof(Points), Points, points);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            reactance = NotifyPropertyChanges(nameof(Reactance), Reactance, reactance);
            resistance = NotifyPropertyChanges(nameof(Resistance), Resistance, resistance);
            tooltipMappingName = NotifyPropertyChanges(nameof(TooltipMappingName), TooltipMappingName, tooltipMappingName);
            if (PropertyChanges.Any() && IsRendered)
            {
                if (PropertyChanges.ContainsKey(nameof(DataSource)) && DataSource != null)
                {
                    CurrentViewData = await UpdatedSeriesData();
                }

                await BaseParent.PropertyChanged(PropertyChanges, "Series");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            UpdateObservableEvents(nameof(DataSource), DataSource, true);
            UpdateObservableEvents(nameof(Points), Points, true);
            Parent.Series?.Remove(this);
            Parent = null;
            BaseParent = null;
            ChildContent = null;
            DataSource = dataSource = null;
            Marker = null;
            Points = points = null;
            Tooltip = null;
            ActualPoints = null;
            CurrentViewData = null;
            LabelOption = null;
        }
    }
}
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the series for range navigator.
    /// </summary>
    public partial class RangeNavigatorSeries : SfDataBoundComponent
    {
        private string dashArray;
        private object dataSource;
        private string fill;
        private double opacity;
        private Query query;
        private RangeNavigatorType type;
        private double width;
        private string xname;
        private string yname;

        [CascadingParameter]
        internal RangeNavigatorSeriesCollection Parent { get; set; }

        [CascadingParameter]
        internal SfRangeNavigator ChartInstance { get; set; }

        internal RangeNavigatorAnimation Animation { get; set; } = new RangeNavigatorAnimation();

        internal RangeNavigatorSeriesBorder Border { get; set; } = new RangeNavigatorSeriesBorder();

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the pattern of dashes and gaps to stroke the lines in `Line` type series.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = "0";

        /// <summary>
        /// It defines the data source for a series.
        /// </summary>
        [Parameter]
        public object DataSource { get; set; }

        /// <summary>
        /// The fill color for the series that accepts value in hex and rgba as a valid CSS color string.
        /// It also represents the color of the signal lines in technical indicators.
        /// For technical indicators, the default value is 'blue' and for series, it has null.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// The opacity for the background.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// It defines the query for the data source.
        /// </summary>
        [Parameter]
        public Query Query { get; set; }

        /// <summary>
        /// It defines the series type of the range navigator.
        /// </summary>
        [Parameter]
        public RangeNavigatorType Type { get; set; } = RangeNavigatorType.Line;

        /// <summary>
        /// The stroke width for the series that is applicable only for `Line` type series.
        /// It also represents the stroke width of the signal lines in technical indicators.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// It defines the xName for the series.
        /// </summary>
        [Parameter]
        public string XName { get; set; }

        /// <summary>
        /// It defines the yName for the series.
        /// </summary>
        [Parameter]
        public string YName { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            UpdatedSeriesData();
            ChartInstance.Series.Add(this);
            ChartInstance.UpdateChildProperties("Series", ChartInstance.Series);
        }

        internal async void UpdatedSeriesData()
        {
            if (DataSource != null && DataManager == null)
            {
                SetDataManager<object>(DataSource);
            }
            else if (ChartInstance.DataSource != null)
            {
                SetDataManager<object>(ChartInstance.DataSource);
            }

            if (DataManager == null)
            {
                return;
            }

            DataManager.DataAdaptor.SetRunSyncOnce(true);
            CurrentViewData = (IEnumerable<object>)await DataManager.ExecuteQuery<object>((Query != null) ? Query : new Data.Query());
        }

        internal async Task SeriesPropertyChanged()
        {
            await OnParametersSetAsync();
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            dashArray = NotifyPropertyChanges(nameof(DashArray), DashArray, dashArray);
            dataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, dataSource);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            query = NotifyPropertyChanges(nameof(Query), Query, query);
            type = NotifyPropertyChanges(nameof(Type), Type, type);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            xname = NotifyPropertyChanges(nameof(XName), XName, xname);
            yname = NotifyPropertyChanges(nameof(YName), YName, yname);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            if ((bool)PropertyChanges?.Keys.Any(m => m.Contains("DataSource", System.StringComparison.InvariantCulture)))
            {
                SetDataManager<object>(DataSource);
                DataManager.DataAdaptor.SetRunSyncOnce(true);
                CurrentViewData = (IEnumerable<object>)await DataManager.ExecuteQuery<object>(new Data.Query());
            }

            if (PropertyChanges.Any())
            {
                SfBaseUtils.UpdateDictionary(nameof(ChartInstance.Series), ChartInstance.Series, ChartInstance.PropertyChanges);
                await ChartInstance.OnRangeNaivgatorParametersSet();
                PropertyChanges.Clear();
            }
        }

        internal void UpdateSeriesProperties(string key, object keyValue)
        {
            if (key == nameof(Animation))
            {
                Animation = (RangeNavigatorAnimation)keyValue;
            }
            else if (key == nameof(Border))
            {
                Border = (RangeNavigatorSeriesBorder)keyValue;
            }
        }
    }
}
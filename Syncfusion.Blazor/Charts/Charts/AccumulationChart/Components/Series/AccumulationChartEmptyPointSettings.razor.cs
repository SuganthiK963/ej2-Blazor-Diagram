using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets configurarion of empty point settings for the chart.
    /// </summary>
    public partial class AccumulationChartEmptyPointSettings
    {
        [CascadingParameter]
        private AccumulationChartSeries series { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Options to customize the border of empty points.
        /// </summary>
        [Parameter]
        public AccumulationChartEmptyPointBorder Border { get; set; } = new AccumulationChartEmptyPointBorder();

        private AccumulationChartEmptyPointBorder border { get; set; }

        /// <summary>
        /// To customize the fill color of empty points.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        private string fill { get; set; }

        /// <summary>
        /// To customize the mode of empty points.
        /// </summary>
        [Parameter]
        public EmptyPointMode Mode { get; set; }

        private EmptyPointMode mode { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            series.UpdateSeriesProperties("EmptyPointSettings", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            border = Border = NotifyPropertyChanges(nameof(Border), Border, border);
            fill = Fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            mode = Mode = NotifyPropertyChanges(nameof(Mode), Mode, mode);
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)series).PropertyChanges.TryAdd(nameof(series.EmptyPointSettings), this);
                PropertyChanges.Clear();
                await series.SeriesPropertyChanged();
            }
        }

        internal async Task EmptyPointPropertyChanged()
        {
            await OnParametersSetAsync();
#pragma warning restore CA2007
        }

        internal override void ComponentDispose()
        {
            series = null;
            ChildContent = null;
            Border = null;
        }

        internal void UpdateEmptyPointProperties(AccumulationChartEmptyPointBorder keyValue)
        {
            border = Border = (AccumulationChartEmptyPointBorder)keyValue;
        }
    }
}
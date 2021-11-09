using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the border configuration for the series.
    /// </summary>
    public partial class AccumulationChartSeriesBorder
    {
        /// <summary>
        /// Border color for the series.
        /// </summary>
        public override string Color { get => base.Color; set => base.Color = value; }

        /// <summary>
        /// Border width for the series.
        /// </summary>
        public override double Width { get => base.Width; set => base.Width = value; }

        [CascadingParameter]
        private AccumulationChartSeries series { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            series.UpdateSeriesProperties("Border", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)series).PropertyChanges.TryAdd(nameof(series.Border), this);
                PropertyChanges.Clear();
                await series.SeriesPropertyChanged();
#pragma warning restore CA2007
            }
        }

        internal override void ComponentDispose()
        {
            series = null;
            ChildContent = null;
        }
    }
}
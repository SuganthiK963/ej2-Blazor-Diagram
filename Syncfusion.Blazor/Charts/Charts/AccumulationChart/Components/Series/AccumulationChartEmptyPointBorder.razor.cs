using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the emptyPoint's border for the Accumulation chart.
    /// </summary>
    public partial class AccumulationChartEmptyPointBorder
    {
        [CascadingParameter]
        private AccumulationChartEmptyPointSettings emptyPoint { get; set; }

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
            emptyPoint.UpdateEmptyPointProperties(this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                ((SfBaseComponent)emptyPoint).PropertyChanges.TryAdd(nameof(emptyPoint.Border), this);
                PropertyChanges.Clear();
                await emptyPoint.EmptyPointPropertyChanged();
#pragma warning restore CA2007
            }
        }

        internal override void ComponentDispose()
        {
            emptyPoint = null;
            ChildContent = null;
        }
    }
}
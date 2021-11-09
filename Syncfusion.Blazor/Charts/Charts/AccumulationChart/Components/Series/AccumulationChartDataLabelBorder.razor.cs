using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and the border for Accumulation chart's datalabel.
    /// </summary>
    public partial class AccumulationChartDataLabelBorder
    {
        [CascadingParameter]
        private AccumulationDataLabelSettings dataLabel { get; set; }

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
            dataLabel.UpdateDataLabelProperties("Border", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                ((SfBaseComponent)dataLabel).PropertyChanges.TryAdd(nameof(dataLabel.Border), this);
                PropertyChanges.Clear();
                await dataLabel.DataLabelPropertyChanged();
#pragma warning restore CA2007
            }
        }

        internal override void ComponentDispose()
        {
            dataLabel = null;
            ChildContent = null;
        }
    }
}
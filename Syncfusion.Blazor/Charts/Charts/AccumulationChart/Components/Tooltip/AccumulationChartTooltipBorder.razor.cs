using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the border of the accumulation chart's tooltip.
    /// </summary>
    public partial class AccumulationChartTooltipBorder
    {
        [CascadingParameter]
        private AccumulationChartTooltipSettings tooltip { get; set; }

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
#pragma warning restore CA2007
            tooltip.UpdateTooltipProperties("Border", this);
        }

        internal override void ComponentDispose()
        {
            tooltip = null;
            ChildContent = null;
        }
    }
}
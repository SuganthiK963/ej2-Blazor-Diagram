using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the Textstyle of the Accumulation chart's tooltip.
    /// </summary>
    public partial class AccumulationChartTooltipTextStyle
    {
        [CascadingParameter]
        private AccumulationChartTooltipSettings tooltip { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Options to customize the tooltip.
        /// </summary>
        public override string Size { get; set; } = "13px";

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
#pragma warning restore CA2007
            tooltip.UpdateTooltipProperties("TextStyle", this);
        }

        internal override void ComponentDispose()
        {
            tooltip = null;
            ChildContent = null;
        }
    }
}
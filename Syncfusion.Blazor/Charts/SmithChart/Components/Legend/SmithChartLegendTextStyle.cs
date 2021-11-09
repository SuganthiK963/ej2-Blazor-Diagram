using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the text style for legend title of the chart.
    /// </summary>
    public class SmithChartLegendTextStyle : SmithChartCommonFont
    {
        [CascadingParameter]
        internal SmithChartLegendSettings DynamicParent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Font size of the text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "13px";

        /// <summary>
        /// Specifies the color of the legend text.
        /// </summary>
        [Parameter]
        public override string Color { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.TextStyle = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "LegendSettings");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            DynamicParent = null;
            BaseParent = null;
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart PrimaryXAxis.
    /// </summary>
    public partial class StockChartPrimaryXAxis : StockChartCommonAxis
    {
        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the type of data the axis is handling.
        /// Double:  Renders a numeric axis.
        /// DateTime: Renders a dateTime axis.
        /// Category: Renders a category axis.
        /// Logarithmic: Renders a log axis.
        /// </summary>
        [Parameter]
        public override ValueType ValueType { get; set; } = ValueType.DateTime;

        /// <summary>
        /// Unique identifier of an axis.
        /// To associate an axis with the series, set this name to the xAxisName/yAxisName properties of the series.
        /// </summary>
        [Parameter]
        public override string Name { get; set; } = Constants.PRIMARYXAXIS;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.PrimaryXAxis = this;
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            ChildContent = null;
        }
    }
}
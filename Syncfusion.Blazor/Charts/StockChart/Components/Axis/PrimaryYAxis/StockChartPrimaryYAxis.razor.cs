using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart PrimaryYAxis.
    /// </summary>
    public partial class StockChartPrimaryYAxis : StockChartCommonAxis
    {
        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// If set to true, the axis will render at the opposite side of its default position.
        /// </summary>
        [Parameter]
        public override bool OpposedPosition { get; set; } = true;

        /// <summary>
        /// Specifies the placement of a labels to the axis line. They are,
        /// inside: Renders the labels inside to the axis line.
        /// outside: Renders the labels outside to the axis line.
        /// </summary>
        [Parameter]
        public override AxisPosition LabelPosition { get; set; } = AxisPosition.Inside;

        /// <summary>
        /// Unique identifier of an axis.
        /// To associate an axis with the series, set this name to the xAxisName/yAxisName properties of the series.
        /// </summary>
        [Parameter]
        public override string Name { get; set; } = Constants.PRIMARYYAXIS;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.PrimaryYAxis = this;
        }

        internal override void ComponentDispose()
        {
            ChildContent = null;
        }
    }
}
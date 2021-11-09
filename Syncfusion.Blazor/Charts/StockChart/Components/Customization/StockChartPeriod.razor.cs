using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart period.
    /// </summary>
    public partial class StockChartPeriod : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartPeriods Periods { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Count value for the button.
        /// </summary>
        [Parameter]
        public double Interval { get; set; } = 1;

        /// <summary>
        /// IntervalType of button.
        /// </summary>
        [Parameter]
        public RangeIntervalType IntervalType { get; set; } = RangeIntervalType.Years;

        /// <summary>
        /// To select the default period.
        /// </summary>
        [Parameter]
        public bool Selected { get; set; }

        /// <summary>
        /// Text to be displayed on the button.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Periods.Periods.Add(this);
        }

        internal override void ComponentDispose()
        {
            Periods = null;
            ChildContent = null;
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the period element for an periodselector in range navigator.
    /// </summary>
    public partial class RangeNavigatorPeriod : SfBaseComponent
    {
        [CascadingParameter]
        internal RangeNavigatorPeriods Parent { get; set; }

        /// <summary>
        /// Gets and sets the interval value for the periods.
        /// </summary>
        [Parameter]
        public double Interval { get; set; } = 1;

        /// <summary>
        /// Gets and sets the IntervalType of period.
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
            Parent.UpdateChildProperty(this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
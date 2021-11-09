using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the range band of the sparkline component.
    /// </summary>
    public partial class SparklineRangeBand : SfBaseComponent
    {
        private string color;
        private double endRange;
        private double opacity;
        private double startRange;

        [CascadingParameter]
        internal SparklineRangeBandSettings Parent { get; set; }

        [CascadingParameter]
        internal ISparkline BaseParent { get; set; }

        /// <summary>
        /// Sets and gets sparkline rangeband color.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Sets and gets the end range.
        /// </summary>
        [Parameter]
        public double EndRange { get; set; }

        /// <summary>
        /// Sets and gets the rangeband opacity.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Sets and gets the start range.
        /// </summary>
        [Parameter]
        public double StartRange { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.RangeBand.Add(this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            endRange = NotifyPropertyChanges(nameof(EndRange), EndRange, endRange);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            startRange = NotifyPropertyChanges(nameof(StartRange), StartRange, startRange);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.OnPropertyChanged(PropertyChanges, nameof(SparklineRangeBand));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
        }
    }
}
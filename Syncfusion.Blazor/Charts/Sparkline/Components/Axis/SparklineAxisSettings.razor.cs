using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the configures of sparkline axis settings.
    /// </summary>
    public partial class SparklineAxisSettings : SfBaseComponent
    {
        private double maxX;
        private double maxY;
        private double minX;
        private double value;
        private double minY;

        [CascadingParameter]
        internal ISparkline Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets the sparkline X axis maximum value.
        /// </summary>
        [Parameter]
        public double MaxX { get; set; } = double.NaN;

        /// <summary>
        /// Sets and gets the sparkline Y axis maximum value.
        /// </summary>
        [Parameter]
        public double MaxY { get; set; } = double.NaN;

        /// <summary>
        /// Sets and gets the sparkline X axis minimum value.
        /// </summary>
        [Parameter]
        public double MinX { get; set; } = double.NaN;

        /// <summary>
        /// Sets and gets the sparkline Y axis minimum value.
        /// </summary>
        [Parameter]
        public double MinY { get; set; } = double.NaN;

        /// <summary>
        /// Sets and gets the sparkline horizontal axis line position.
        /// </summary>
        [Parameter]
        public double Value { get; set; }

        internal SparklineAxisLineSettings LineSettings { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.AxisSettings = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            maxX = NotifyPropertyChanges(nameof(MaxX), MaxX, maxX);
            maxY = NotifyPropertyChanges(nameof(MaxY), MaxY, maxY);
            minX = NotifyPropertyChanges(nameof(MinX), MinX, minX);
            minY = NotifyPropertyChanges(nameof(MinY), MinY, minY);
            value = NotifyPropertyChanges(nameof(Value), Value, value);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(SparklineAxisLineSettings));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            LineSettings = null;
        }
    }
}
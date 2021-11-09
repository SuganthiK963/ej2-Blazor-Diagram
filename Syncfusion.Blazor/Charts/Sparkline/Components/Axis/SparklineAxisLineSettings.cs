using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the configures of line settings in sparkline axis.
    /// </summary>
    public class SparklineAxisLineSettings : SfBaseComponent
    {
        private string color;
        private string dashArray;
        private double opacity;
        private bool visible;
        private double width;

        [CascadingParameter]
        internal SparklineAxisSettings Parent { get; set; }

        [CascadingParameter]
        internal ISparkline BaseParent { get; set; }

        /// <summary>
        /// Sets and gets the sparkline axis line color.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Sets and gets the sparkline axis line dashArray.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary>
        /// Sets and gets the sparkline axis line opacity.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Sets and gets the axis line visibility.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Sets and gets the sparkline axis line width.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.LineSettings = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            dashArray = NotifyPropertyChanges(nameof(DashArray), DashArray, dashArray);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.OnPropertyChanged(PropertyChanges, nameof(SparklineAxisLineSettings));
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
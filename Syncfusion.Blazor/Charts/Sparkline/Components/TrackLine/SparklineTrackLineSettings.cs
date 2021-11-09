using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the track line of the sparkline component.
    /// </summary>
    public class SparklineTrackLineSettings : SfBaseComponent
    {
        private string color;
        private bool visible;
        private double width;

        [CascadingParameter]
        internal ISparklineTooltip Parent { get; set; }

        /// <summary>
        /// Sets and gets the tracker line color.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Sets and gets the tracker line visibility.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Sets and gets the tracker line width.
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
            Parent.UpdateChildProperties(nameof(SparklineTrackLineSettings), this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
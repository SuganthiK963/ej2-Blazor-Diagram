using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the margin of the chart.
    /// </summary>
    public partial class SmithChartMargin : SfBaseComponent
    {
        private double bottom = 10;
        private double left = 10;
        private double right = 10;
        private double top = 10;

        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets and sets the bottom margin for the chart component.
        /// </summary>
        [Parameter]
        public double Bottom { get; set; } = 10;

        /// <summary>
        /// Gets and sets the left margin for the chart component.
        /// </summary>
        [Parameter]
        public double Left { get; set; } = 10;

        /// <summary>
        /// Gets and sets the right margin for the chart component.
        /// </summary>
        [Parameter]
        public double Right { get; set; } = 10;

        /// <summary>
        /// Gets and sets the top margin for the chart component.
        /// </summary>
        [Parameter]
        public double Top { get; set; } = 10;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Margin = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            bottom = NotifyPropertyChanges(nameof(Bottom), Bottom, bottom);
            top = NotifyPropertyChanges(nameof(Top), Top, top);
            left = NotifyPropertyChanges(nameof(Left), Left, left);
            right = NotifyPropertyChanges(nameof(Right), Right, right);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.PropertyChanged(PropertyChanges, "Margin");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}
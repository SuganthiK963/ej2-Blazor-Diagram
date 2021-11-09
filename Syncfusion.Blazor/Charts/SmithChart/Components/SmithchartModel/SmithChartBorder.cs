using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the border of the chart.
    /// </summary>
    public partial class SmithChartBorder
    {
        private string color;
        private double opacity;
        private double width;

        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Color for the border.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "transparent";

        /// <summary>
        /// Opacity for the border.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Width for the border.
        /// </summary>
        [Parameter]
        public double Width { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Border = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.PropertyChanged(PropertyChanges, "Border");
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
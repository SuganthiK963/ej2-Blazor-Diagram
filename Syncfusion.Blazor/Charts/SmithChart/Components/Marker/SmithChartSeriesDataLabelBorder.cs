using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines data label border settings for series.
    /// </summary>
    public partial class SmithChartSeriesDataLabelBorder
    {
        private string color;
        private double width;

        [CascadingParameter]
        internal SmithChartSeriesDatalabel Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Border color for data label.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "white";

        /// <summary>
        /// Border width for data label.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 0.1;

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
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "Series");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            BaseParent = null;
        }
    }
}
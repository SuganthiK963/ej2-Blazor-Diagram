using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the item style for legend in the chart.
    /// </summary>
    public partial class SmithChartLegendItemStyle
    {
        private double height;
        private double width;

        [CascadingParameter]
        internal SmithChartLegendSettings Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specify the height for legend item.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 10;

        /// <summary>
        /// Specify the width for legend item.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 10;

        internal LegendItemStyleBorder Border { get; set; } = new LegendItemStyleBorder();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.ItemStyle = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "LegendSettings");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            BaseParent = null;
        }
    }
}
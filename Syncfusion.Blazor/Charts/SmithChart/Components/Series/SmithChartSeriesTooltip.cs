using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the tooltip of the chart series.
    /// </summary>
    public partial class SmithChartSeriesTooltip
    {
        private string fill;
        private double opacity;
        private bool visible;

        [CascadingParameter]
        internal SmithChartSeries Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Color for the tooltip.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Opacity for the tooltip.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 0.95;

        /// <summary>
        /// Template for the tooltip.
        /// </summary>
        [Parameter]
        public RenderFragment<object> Template { get; set; }

        /// <summary>
        /// Visibility of the tooltip.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        internal SmithChartSeriesTooltipBorder Border { get; set; } = new SmithChartSeriesTooltipBorder();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Tooltip = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
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
            Template = null;
            Border = null;
            BaseParent = null;
        }
    }
}
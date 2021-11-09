using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the major grid lines for radial axis of the chart.
    /// </summary>
    public partial class SmithChartRadialMajorGridLines
    {
        private bool visible;
        private double opacity;

        [CascadingParameter]
        internal SmithChartRadialAxis DynamicParent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI component.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Visible of the major grid lines.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Opacity for the major grid lines.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.MajorGridLines = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            if (PropertyChanges.Count != 0 && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "RadialAxis");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            DynamicParent = null;
            ChildContent = null;
            BaseParent = null;
        }
    }
}
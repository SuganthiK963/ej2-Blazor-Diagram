using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the minor grid lines for radial axis of the chart.
    /// </summary>
    public partial class SmithChartRadialMinorGridLines
    {
        private bool visible;
        private int count;

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
        /// Visible of the minor grid lines.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Width of the minor grid lines.
        /// </summary>
        [Parameter]
        public override double Width { get; set; } = 1;

        /// <summary>
        /// Gets and sets count for minor grid lines.
        /// </summary>
        [Parameter]
        public int Count { get; set; } = 8;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.MinorGridLines = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            count = NotifyPropertyChanges(nameof(Count), Count, count);
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
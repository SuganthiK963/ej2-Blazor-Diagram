using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the axis label style for horizontal axis of the chart.
    /// </summary>
    public partial class SmithChartHorizontalAxisLabelStyle : SmithChartCommonFont
    {
        [CascadingParameter]
        internal SmithChartHorizontalAxis DynamicParent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Size of the horizontal axis label.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "12px";

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.LabelStyle = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Count != 0 && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "HorizontalAxis");
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
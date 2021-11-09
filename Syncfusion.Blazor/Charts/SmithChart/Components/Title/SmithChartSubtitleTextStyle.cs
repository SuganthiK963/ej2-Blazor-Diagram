using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the text style for subtitle of the chart.
    /// </summary>
    public partial class SmithChartSubtitleTextStyle : SmithChartCommonFont
    {
        [CascadingParameter]
        internal SmithChartSubtitle DynamicParent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Font size of the text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "13px";

        /// <summary>
        /// Specifies the color of the title text.
        /// </summary>
        [Parameter]
        public override string Color { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.TextStyle = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "Title");
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
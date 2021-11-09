using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the tilte for legend of the chart.
    /// </summary>
    public partial class SmithChartLegendTitle
    {
        private string text;
        private SmithChartAlignment textAlignment;
        private bool visible;

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
        /// Description for legend title.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Text for legend title.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Alignment for legend title.
        /// </summary>
        [Parameter]
        public SmithChartAlignment TextAlignment { get; set; } = SmithChartAlignment.Center;

        /// <summary>
        /// Visibility for legend title.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        internal SmithChartLegendTitleTextStyle TextStyle { get; set; } = new SmithChartLegendTitleTextStyle();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Title = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            text = NotifyPropertyChanges(nameof(Text), Text, text);
            textAlignment = NotifyPropertyChanges(nameof(TextAlignment), TextAlignment, textAlignment);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
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
            TextStyle = null;
            BaseParent = null;
        }
    }
}
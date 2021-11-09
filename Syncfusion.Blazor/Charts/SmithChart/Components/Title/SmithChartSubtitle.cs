using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the subtitle of the chart.
    /// </summary>
    public partial class SmithChartSubtitle
    {
        private bool enableTrim;
        private double maximumWidth;
        private SmithChartAlignment textAlignment;
        private string text;
        private bool visible;

        [CascadingParameter]
        internal SmithChartTitle Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Description for the subtitle.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Trim the subtitle.
        /// </summary>
        [Parameter]
        public bool EnableTrim { get; set; } = true;

        /// <summary>
        /// Maximum width of the subtitle.
        /// </summary>
        [Parameter]
        public double MaximumWidth { get; set; } = double.NaN;

        /// <summary>
        /// Text for the subtitle.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Text alignment for the subtitle.
        /// </summary>
        [Parameter]
        public SmithChartAlignment TextAlignment { get; set; } = SmithChartAlignment.Far;

        /// <summary>
        /// Visibility of the subtitle.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        internal SmithChartSubtitleTextStyle TextStyle { get; set; } = new SmithChartSubtitleTextStyle();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Subtitle = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enableTrim = NotifyPropertyChanges(nameof(EnableTrim), EnableTrim, enableTrim);
            maximumWidth = NotifyPropertyChanges(nameof(MaximumWidth), MaximumWidth, maximumWidth);
            text = NotifyPropertyChanges(nameof(Text), Text, text);
            textAlignment = NotifyPropertyChanges(nameof(TextAlignment), TextAlignment, textAlignment);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            text = NotifyPropertyChanges(nameof(Text), Text, text);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "Title");
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
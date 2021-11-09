using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the title of the chart.
    /// </summary>
    public partial class SmithChartTitle
    {
        private bool enableTrim;
        private double maximumWidth;
        private string text;
        private SmithChartAlignment textAlignment;
        private bool visible;

        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Description for the title.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Trim the title.
        /// </summary>
        [Parameter]
        public bool EnableTrim { get; set; } = true;

        /// <summary>
        /// Maximum width of the subtitle.
        /// </summary>
        [Parameter]
        public double MaximumWidth { get; set; } = double.NaN;

        /// <summary>
        /// Text for the title.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Text alignment for the title.
        /// </summary>
        [Parameter]
        public SmithChartAlignment TextAlignment { get; set; } = SmithChartAlignment.Center;

        /// <summary>
        /// Visibility of the title.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        internal SmithChartTitleTextStyle TextStyle { get; set; } = new SmithChartTitleTextStyle();

        internal SmithChartTitleFont Font { get; set; } = new SmithChartTitleFont();

        internal SmithChartSubtitle Subtitle { get; set; } = new SmithChartSubtitle();

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
            enableTrim = NotifyPropertyChanges(nameof(EnableTrim), EnableTrim, enableTrim);
            maximumWidth = NotifyPropertyChanges(nameof(MaximumWidth), MaximumWidth, maximumWidth);
            text = NotifyPropertyChanges(nameof(Text), Text, text);
            textAlignment = NotifyPropertyChanges(nameof(TextAlignment), TextAlignment, textAlignment);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            text = NotifyPropertyChanges(nameof(Text), Text, text);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.PropertyChanged(PropertyChanges, "Title");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Font = null;
            Subtitle = null;
            TextStyle = null;
        }
    }
}
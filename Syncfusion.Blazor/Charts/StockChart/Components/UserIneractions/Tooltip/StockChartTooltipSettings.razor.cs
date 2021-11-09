using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart tooltip.
    /// </summary>
    public partial class StockChartTooltipSettings : SfBaseComponent
    {
        private bool enable;

        internal string HeaderContent { get; set; }

        internal string FormatContent { get; set; }

        internal StockChartTooltipBorder Border { get; set; }

        internal StockChartTooltipTextStyle TextStyle { get; set; }

        [CascadingParameter]
        internal SfStockChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Duration for the ToolTip animation.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 300;

        /// <summary>
        /// Enables / Disables the visibility of the tooltip.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// If set to true, ToolTip will animate while moving from one point to another.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Enables / Disables the visibility of the marker.
        /// </summary>
        [Parameter]
        public bool EnableMarker { get; set; } = false;

        /// <summary>
        /// To wrap the tooltip long text based on available space.
        /// This is only application for chart tooltip.
        /// </summary>
        [Parameter]
        public bool EnableTextWrap { get; set; }

        /// <summary>
        /// Fade Out duration for the ToolTip hide.
        /// </summary>
        [Parameter]
        public double FadeOutDuration { get; set; } = 1000;

        /// <summary>
        /// The fill color of the tooltip that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Format the ToolTip content.
        /// </summary>
        [Parameter]
        public string Format
        {
            get
            {
                return FormatContent;
            }

            set
            {
                FormatContent = value;
            }
        }

        /// <summary>
        /// Header for tooltip.
        /// </summary>
        [Parameter]
        public string Header
        {
            get
            {
                return HeaderContent;
            }

            set
            {
                HeaderContent = value;
            }
        }

        /// <summary>
        /// The fill color of the tooltip that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 0.75;

        /// <summary>
        /// If set to true, a single ToolTip will be displayed for every index.
        /// </summary>
        [Parameter]
        public bool Shared { get; set; } = true;

        /// <summary>
        /// Custom template to format the ToolTip content. Use ${x} and ${y} as the placeholder text to display the corresponding data point.
        /// </summary>
        [Parameter]
        public RenderFragment<object> Template { get; set; }

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
            enable = NotifyPropertyChanges(nameof(Enable), Enable, enable);
            if (PropertyChanges.Any() && IsRendered)
            {
                PropertyChanges.Clear();
                Parent.OnStockChartPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            TextStyle = null;
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines data label settings for series.
    /// </summary>
    public partial class SmithChartSeriesDatalabel
    {
        private string fill;
        private double opacity;
        private bool visible;

        [CascadingParameter]
        internal SmithChartSeriesMarker Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Color for data label.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Opacity for data label.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Showing data label template.
        /// </summary>
        [Parameter]
        public RenderFragment<SmithChartPoint> Template { get; set; }

        internal SmithChartDataLabelTextStyle TextStyle { get; set; } = new SmithChartDataLabelTextStyle();

        internal SmithChartSeriesDataLabelBorder Border { get; set; } = new SmithChartSeriesDataLabelBorder();

        internal SmithChartDataLabelConnectorLine ConnectorLine { get; set; } = new SmithChartDataLabelConnectorLine();

        /// <summary>
        /// Visibility for data label.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.DataLabel = this;
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
            Border = null;
            Template = null;
            TextStyle = null;
            ConnectorLine = null;
            BaseParent = null;
        }
    }
}
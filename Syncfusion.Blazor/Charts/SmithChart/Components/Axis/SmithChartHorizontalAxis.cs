using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the horizontal axis of the chart.
    /// </summary>
    public partial class SmithChartHorizontalAxis
    {
        private SmithChartLabelIntersectAction labelIntersectAction;
        private AxisLabelPosition labelPosition;
        private bool visible;

        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Axis labels will be hide when overlap with each other.
        /// </summary>
        [Parameter]
        public SmithChartLabelIntersectAction LabelIntersectAction { get; set; } = SmithChartLabelIntersectAction.Hide;

        /// <summary>
        /// Position of  axis line.
        /// </summary>
        [Parameter]
        public AxisLabelPosition LabelPosition { get; set; } = AxisLabelPosition.Outside;

        /// <summary>
        /// Visibility of the axis.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        internal SmithChartHorizontalAxisLine AxisLine { get; set; } = new SmithChartHorizontalAxisLine();

        internal SmithChartHorizontalAxisLabelStyle LabelStyle { get; set; } = new SmithChartHorizontalAxisLabelStyle();

        internal SmithChartHorizontalMajorGridLines MajorGridLines { get; set; } = new SmithChartHorizontalMajorGridLines();

        internal SmithChartHorizontalMinorGridLines MinorGridLines { get; set; } = new SmithChartHorizontalMinorGridLines();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.HorizontalAxis = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            labelIntersectAction = NotifyPropertyChanges(nameof(LabelIntersectAction), LabelIntersectAction, labelIntersectAction);
            labelPosition = NotifyPropertyChanges(nameof(LabelPosition), LabelPosition, labelPosition);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "HorizontalAxis");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
            ChildContent = null;
            AxisLine = null;
            LabelStyle = null;
            MajorGridLines = null;
            MinorGridLines = null;
        }
    }
}

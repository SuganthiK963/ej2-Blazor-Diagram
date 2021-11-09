using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the radial axis of the chart.
    /// </summary>
    public partial class SmithChartRadialAxis
    {
        private SmithChartLabelIntersectAction labelIntersectAction;
        private AxisLabelPosition labelPosition;
        private bool visible;

        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI component.
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
        /// Visibility of  axis.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        internal SmithChartRadialAxisLabelStyle LabelStyle { get; set; } = new SmithChartRadialAxisLabelStyle();

        internal SmithChartRadialMajorGridLines MajorGridLines { get; set; } = new SmithChartRadialMajorGridLines();

        internal SmithChartRadialMinorGridLines MinorGridLines { get; set; } = new SmithChartRadialMinorGridLines();

        internal SmithChartRadialAxisLine AxisLine { get; set; } = new SmithChartRadialAxisLine();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.RadialAxis = this;
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
                await BaseParent.PropertyChanged(PropertyChanges, "RadialAxis");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            AxisLine = null;
            LabelStyle = null;
            MajorGridLines = null;
            MinorGridLines = null;
            BaseParent = null;
        }
    }
}
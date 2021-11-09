using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the margin of the datalabel.
    /// </summary>
    public partial class ChartDataLabelMargin : ChartDefaultMargin
    {
        private ChartDataLabel datalabel;

        /// <summary>
        /// Specifies the bottom margin of the datalabel.
        /// </summary>
        [Parameter]
        public override double Bottom { get; set; } = 5;

        /// <summary>
        /// Specifies the left margin of the datalabel.
        /// </summary>
        [Parameter]
        public override double Left { get; set; } = 5;

        /// <summary>
        /// Specifies the right margin of the datalabel.
        /// </summary>
        [Parameter]
        public override double Right { get; set; } = 5;

        /// <summary>
        /// Specifies the top margin of the datalabel.
        /// </summary>
        [Parameter]
        public override double Top { get; set; } = 5;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            datalabel = (ChartDataLabel)Tracker;
            datalabel.UpdateDatalabelProperties("Margin", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            datalabel.UpdateDatalabelProperties("Margin", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            datalabel = null;
            ChildContent = null;
        }
    }
}
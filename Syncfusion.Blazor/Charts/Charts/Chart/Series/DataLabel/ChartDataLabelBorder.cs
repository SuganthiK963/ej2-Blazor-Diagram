using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the datalabel border.
    /// </summary>
    public class ChartDataLabelBorder: ChartDefaultBorder
    {
        private ChartDataLabel datalabel;

        /// <summary>
        /// Specifies the width of the datalabel border.
        /// </summary>
        [Parameter]
        public override double Width { get; set; } = double.NaN;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            datalabel = (ChartDataLabel)Tracker;
            datalabel.UpdateDatalabelProperties("Border", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            datalabel.UpdateDatalabelProperties("Border", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            datalabel = null;
            ChildContent = null;
        }
    }
}
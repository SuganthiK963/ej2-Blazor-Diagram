using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the multilevel label text.
    /// </summary>
    public class ChartAxisMultiLevelLabelTextStyle : ChartDefaultFont
    {
        [CascadingParameter]
        private ChartMultiLevelLabel Parent { get; set; }

        /// <summary>
        /// Color for the text.
        /// </summary>
        [Parameter]
        public override string Color { get; set; }

        /// <summary>
        /// Font size for the text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "12px";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartMultiLevelLabel)Tracker;
            Parent.UpdateMultiLevelLabelProperties("TextStyle", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent = null;
            ChildContent = null;
        }
    }
}
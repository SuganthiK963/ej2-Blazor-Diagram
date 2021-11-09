using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the multilevel label border.
    /// </summary>
    public class ChartAxisMultiLevelLabelBorder : ChartSubComponent
    {
        [CascadingParameter]
        private ChartMultiLevelLabel Parent { get; set; }

        /// <summary>
        /// The color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Border type for labels
        /// Rectangle
        /// Without Top Border
        /// Without Top and BottomBorder
        /// Without Border
        /// Brace
        /// CurlyBrace.
        /// </summary>
        [Parameter]
        public BorderType Type { get; set; }

        /// <summary>
        /// The width of the border in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartMultiLevelLabel)Tracker;
            Parent.UpdateMultiLevelLabelProperties("Border", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent = null;
            ChildContent = null;
        }
    }
}
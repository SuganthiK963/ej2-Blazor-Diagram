using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the tooltip settings for the annotation in range navigator.
    /// </summary>
    public partial class RangeNavigatorRangeTooltipSettings
    {
        [CascadingParameter]
        internal SfRangeNavigator Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// It defines display mode for tooltip.
        /// </summary>
        [Parameter]
        public TooltipDisplayMode DisplayMode { get; set; } = TooltipDisplayMode.OnDemand;

        /// <summary>
        /// Enables / Disables the visibility of the tooltip.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// The fill color of the tooltip that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Format the ToolTip content.
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// The fill color of the tooltip that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 0.85;

        /// <summary>
        /// Sets or gets the tempate of tooltip.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Template
        {
            get
            {
                return TooltipTemplate;
            }

            set
            {
                TooltipTemplate = value;
            }
        }

        internal string TooltipTemplate { get; set; }

        internal RangeNavigatorTooltipTextStyle TextStyle { get; set; } = new RangeNavigatorTooltipTextStyle();

        internal RangeNavigatorTooltipBorder Border { get; set; } = new RangeNavigatorTooltipBorder();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Tooltip", this);
        }

        internal void UpdateTooltipProperties(string key, object keyValue)
        {
            if (key == nameof(Border))
            {
                Border = (RangeNavigatorTooltipBorder)keyValue;
            }
            else if (key == nameof(TextStyle))
            {
                TextStyle = (RangeNavigatorTooltipTextStyle)keyValue;
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            TextStyle = null;
            Border = null;
        }
    }
}
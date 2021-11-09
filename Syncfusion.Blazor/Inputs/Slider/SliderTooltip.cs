using System.ComponentModel;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs.Slider.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// This class is used to set a tooltip for slider component.
    /// </summary>
    public partial class SliderTooltip : SfBaseComponent
    {
        [CascadingParameter]
        internal ISlider Parent { get; set; }

        /// <summary>
        /// Specifies the ChildContent.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// It is used to customize the Tooltip which accepts custom CSS class names that define
        ///  specific user-defined styles and themes to be applied on the Tooltip element.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// It is used to customize the Tooltip content to the desired format
        ///  using internationalization or events (custom formatting).
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// It is used to show or hide the Tooltip of Slider Component.
        /// </summary>
        [Parameter]
        public bool IsVisible { get; set; }

        /// <summary>
        /// It is used to denote the position for the tooltip element in the Slider. The available options are:
        ///   Before - Tooltip is shown in the top of the horizontal slider bar or at the left of the vertical slider bar.
        ///   After - Tooltip is shown in the bottom of the horizontal slider bar or at the right of the vertical slider bar.
        /// </summary>
        [Parameter]
        public TooltipPlacement Placement { get; set; }

        /// <summary>
        /// It is used to determine the device mode to show the Tooltip.
        /// If it is in desktop, it will show the Tooltip content when hovering on the target element.
        /// If it is in touch device. It will show the Tooltip content when tap and holding on the target element.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TooltipShowOn ShowOn { get; set; } = TooltipShowOn.Auto;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent?.UpdateChildProperties("tooltip", this);
        }
    }
}
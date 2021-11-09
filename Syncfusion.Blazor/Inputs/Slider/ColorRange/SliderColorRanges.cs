using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs.Slider.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the collections of colors to the slider track based on start and end value of each color.
    /// </summary>
    public partial class SliderColorRanges : SfBaseComponent
    {
        [CascadingParameter]
        internal ISlider Parent { get; set; }

        /// <summary>
        /// Get and set the ChildContent.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<ColorRangeDataModel> ColorRange { get; set; } = new List<ColorRangeDataModel>();

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>"Task".</returns>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Parent.UpdateChildProperties("colorRange", ColorRange);
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ColorRange = null;
        }
    }
}

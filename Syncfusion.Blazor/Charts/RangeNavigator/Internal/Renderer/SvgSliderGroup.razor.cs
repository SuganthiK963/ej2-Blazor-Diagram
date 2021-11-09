using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.RangeNavigator.Internal
{
    /// <summary>
    /// Specifies the slider of range navigator.
    /// </summary>
    public partial class SvgSliderGroup
    {
        /// <summary>
        /// Specifies the identification of element.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Specifies the transform of element.
        /// </summary>
        [Parameter]
        public string Transform { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the style of element.
        /// </summary>
        [Parameter]
        public string Style { get; set; }

        /// <summary>
        /// Specifies the shape information of the slider thump.
        /// </summary>
        [Parameter]
        public List<object> ShapeOptions { get; set; }

        /// <summary>
        /// Specifies to update the transform of element.
        /// </summary>
        /// <param name="transform">Represents the transform.</param>
        public void ChangeTransform(string transform)
        {
            if (!Transform.Equals(transform, System.StringComparison.Ordinal))
            {
                Transform = transform;
                InvokeAsync(StateHasChanged);
            }
        }

        internal void Dispose()
        {
            ShapeOptions = null;
        }
    }
}
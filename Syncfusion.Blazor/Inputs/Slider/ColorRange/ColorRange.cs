using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// This class represent the color, start and end position of slider track.
    /// </summary>
    public partial class ColorRange : SfBaseComponent
    {
        [CascadingParameter]
        internal SliderColorRanges Parent { get; set; }

        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Get and set the Color.
        /// </summary>
        // Specifies the color for slider track
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Get and set the start.
        /// </summary>
        // Specifies the start position of the color track
        [Parameter]
        public double Start { get; set; }

        /// <summary>
        /// Get and set the End.
        /// </summary>
        // Specifies the end position of the color track
        [Parameter]
        public double End { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task" representing the asynchronous operation.</returns>
        protected override Task OnInitializedAsync()
        {
            Parent?.ColorRange.Add(new ColorRangeDataModel() { Color = Color, End = End, Start = Start });
            return base.OnInitializedAsync();
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            Color = null;
        }
    }
}

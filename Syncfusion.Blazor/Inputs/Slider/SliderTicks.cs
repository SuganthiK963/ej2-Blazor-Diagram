using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs.Slider.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// This class is used to render ticks to slider component.
    /// </summary>
    public partial class SliderTicks : SfBaseComponent
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
        /// It is used to customize the Slider scale value to the desired format using Internationalization or events(custom formatting).
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// It is used to denote the distance between two major (large) ticks from the scale of the Slider.
        /// </summary>
        [Parameter]
        public double LargeStep { get; set; } = 10;

        /// <summary>
        /// It is used to denote the position of the ticks in the Slider. The available options are:.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Placement Placement { get; set; }

        /// <summary>
        /// We can show or hide the small ticks in the Slider, which will be appeared in between the largeTicks.
        /// </summary>
        [Parameter]
        public bool ShowSmallTicks { get; set; }

        /// <summary>
        /// It is used to denote the distance between two minor (small) ticks from the scale of the Slider.
        /// </summary>
        [Parameter]
        public double SmallStep { get; set; } = 1;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent?.UpdateChildProperties("ticks", this);
        }
    }
}
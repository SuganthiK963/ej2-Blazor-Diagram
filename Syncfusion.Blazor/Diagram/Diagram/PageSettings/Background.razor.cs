using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the characteristics of the background of the diagram.
    /// </summary>
    public class BackgroundStyle : SfBaseComponent
    {
        private string source = string.Empty;
        private string color = "transparent";
        private Scale scale = Scale.None;
        private ImageAlignment align = ImageAlignment.None;

        [CascadingParameter]
        [JsonIgnore]
        internal PageSettings Parent { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the Source value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<string> ImageSourceChanged { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the color value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<string> BackgroundChanged { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the scale value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<Scale> ImageScaleChanged { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the align value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<ImageAlignment> ImageAlignChanged { get; set; }

        /// <summary>
        /// Defines the source of the background image.
        /// </summary>
        [Parameter]
        [JsonPropertyName("imageSource")]
        public string ImageSource { get; set; } = string.Empty;

        /// <summary>
        /// Defines the background color of diagram.
        /// </summary>
        [Parameter]
        [JsonPropertyName("background")]
        public string Background { get; set; } = "transparent";

        /// <summary>
        /// Defines how the background image should be scaled/stretched.
        /// </summary>
        [Parameter]
        [JsonPropertyName("imageScale")]
        public Scale ImageScale { get; set; } = Scale.None;

        /// <summary>
        /// Defines how to align the background image over the diagram area.
        /// </summary>
        [Parameter]
        [JsonPropertyName("imageAlign")]
        public ImageAlignment ImageAlign { get; set; } = ImageAlignment.None;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            source = ImageSource;
            color = Background;
            scale = ImageScale;
            align = ImageAlign;
            Parent.UpdateBackgroundValues(this);
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            source = BaseUtil.UpdateDictionary(nameof(ImageSource), source, ImageSource, PropertyChanges);
            color = BaseUtil.UpdateDictionary(nameof(Background), color, Background, PropertyChanges);
            scale = BaseUtil.UpdateDictionary(nameof(ImageScale), scale, ImageScale, PropertyChanges);
            align = BaseUtil.UpdateDictionary(nameof(ImageAlign), align, ImageAlign, PropertyChanges);
            if (PropertyChanges.Any())
                Parent.UpdateBackgroundValues(this);
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering, otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (PropertyChanges.Any())
            {
                this.Parent.Parent.DiagramStateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
        }
        internal async Task<BackgroundStyle> PropertyUpdate(BackgroundStyle background)
        {
            ImageSource = await SfBaseUtils.UpdateProperty(background.ImageSource, ImageSource, ImageSourceChanged, null, null);
            Background  = await SfBaseUtils.UpdateProperty(background.Background, Background, BackgroundChanged, null, null);
            ImageScale  = await SfBaseUtils.UpdateProperty(background.ImageScale, ImageScale, ImageScaleChanged, null, null);
            ImageAlign = await SfBaseUtils.UpdateProperty(background.ImageAlign, ImageAlign, ImageAlignChanged, null, null);
            return this;
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (Parent != null)
            {
                Parent = null;
            }
            source = null;
            ImageSource = null;
            color = null;
            Background  = null;

        }
    }
}
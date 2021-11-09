using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Sets the diagram's current zoom value, zoom factor, scroll state, and viewport size.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px">
    ///     <ScrollSettings @bind-VerticalOffset="@verticalOffset" @bind-HorizontalOffset="@horizontalOffset" @bind-ScrollLimit="@scrollLimit" @bind-CurrentZoom="@currentZoom" @bind-MinZoom="minZoom" @bind-MaxZoom="@maxZoom">
    ///     </ScrollSettings>
    /// </SfDiagramComponent>    
    /// @code
    /// {
    ///     double verticalOffset { get; set; } = 300;
    ///     double horizontalOffset { get; set; } = -1000;
    ///     ScrollLimitMode scrollLimit { get; set; } = ScrollLimitMode.Infinity;
    ///     double currentZoom { get; set; } = 1;
    ///     double minZoom { get; set; } = 0.2;
    ///     double maxZoom { get; set; } = 1.5;
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public partial class ScrollSettings : SfBaseComponent
    {
        private ScrollLimitMode scrollLimit = ScrollLimitMode.Diagram;

        internal double HOffset;

        internal double VOffset;

        internal double CZoom = 1;

        internal double minimumZoom = 0.2;

        internal double maximumZoom = 30;

        [CascadingParameter]
        [JsonIgnore]
        internal SfDiagramComponent Parent { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the horizontal offset changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> HorizontalOffsetChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the vertical offset changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> VerticalOffsetChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the current zoom changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> CurrentZoomChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the minimum zoom changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> MinZoomChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the maximum zoom changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> MaxZoomChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the scroll limit changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<ScrollLimitMode> ScrollLimitChanged { get; set; }

        /// <summary>
        /// Sets the child content for the ScrollSettings
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the horizontal offset of the scroller.  
        /// </summary>
        [Parameter]
        [JsonPropertyName("horizontalOffset")]
        public double HorizontalOffset { get; set; }

        /// <summary>
        /// Gets or sets the Vertical offset of the scroller. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("verticalOffset")]
        public double VerticalOffset { get; set; }
        /// <summary>
        /// Gets or sets the diagram's currentZoom value. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("currentZoom")]
        public double CurrentZoom { get; set; } = 1;

        /// <summary>
        /// Gets or sets the scroller's minimum zoom value.
        /// </summary>
        [Parameter]
        [JsonPropertyName("minZoom")]
        public double MinZoom { get; set; } = 0.2;

        /// <summary>
        /// Gets or sets the scroller's maximum zoom value.
        /// </summary>
        [Parameter]
        [JsonPropertyName("maxZoom")]
        public double MaxZoom { get; set; } = 30;

        /// <summary>
        /// Gets or sets the upper limit of values of the scrollable range.
        /// </summary>
        [Parameter]
        [JsonPropertyName("scrollLimit")]
        public ScrollLimitMode ScrollLimit { get; set; } = ScrollLimitMode.Diagram;

        internal static ScrollSettings Initialize()
        {
            ScrollSettings scrollsettings = new ScrollSettings();
            return scrollsettings;
        }

        #region Life Cycle methods     
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            PropertyInitialized();
            Parent.UpdateScrollSettings(this);
        }

        private void PropertyInitialized()
        {
            HOffset = HorizontalOffset;
            VOffset = VerticalOffset;
            CZoom = CurrentZoom;
            minimumZoom = MinZoom;
            maximumZoom = MaxZoom;
            scrollLimit = ScrollLimit;
        }
        /// <summary>
        /// Method invoked when any changes in component state occur.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            if (!Parent.ScrollActions.HasFlag(ScrollActions.Interaction) && !Parent.ScrollActions.HasFlag(ScrollActions.PublicMethod))
            {
                BaseUtil.UpdateDictionary(nameof(HorizontalOffset), HOffset, HorizontalOffset, PropertyChanges);
                BaseUtil.UpdateDictionary(nameof(VerticalOffset), VOffset, VerticalOffset, PropertyChanges);
                BaseUtil.UpdateDictionary(nameof(CurrentZoom), CZoom, CurrentZoom, PropertyChanges);
                BaseUtil.UpdateDictionary(nameof(MinZoom), minimumZoom, MinZoom, PropertyChanges);
                BaseUtil.UpdateDictionary(nameof(MaxZoom), maximumZoom, MaxZoom, PropertyChanges);
                BaseUtil.UpdateDictionary(nameof(ScrollLimit), scrollLimit, ScrollLimit, PropertyChanges);
            }
            if (Parent.ScrollActions.HasFlag(ScrollActions.Interaction))
            {
                Parent.ScrollActions &= ~ScrollActions.Interaction;
            }
            if (Parent.ScrollActions.HasFlag(ScrollActions.PublicMethod))
            {
                Parent.ScrollActions &= ~ScrollActions.PublicMethod;
            }
            await base.OnParametersSetAsync().ConfigureAwait(true);
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (PropertyChanges.Any())
            {
                Parent.ScrollActions |= ScrollActions.PropertyChange;
                double hPan = (-HOffset + HorizontalOffset);
                double vPan = (-VOffset + VerticalOffset);
                if (hPan != 0 || vPan != 0)
                {
                    this.Parent.Pan(hPan, vPan);
                }
                if ((!CZoom.Equals(CurrentZoom) || !minimumZoom.Equals(MinZoom) || !maximumZoom.Equals(MaxZoom)) && Parent?.Scroller != null)
                {
                    minimumZoom = MinZoom;
                    maximumZoom = MaxZoom;
                    Parent.Scroller.CurrentZoom = CurrentZoom;
                    CZoom = CurrentZoom;
                    Parent.DiagramContent.UpdateScrollOffset();
                }
                //Parent.DiagramStateHasChanged();
                Parent.ScrollActions &= ~ScrollActions.PropertyChange;
            }
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
        }

        #endregion

        internal async void UpdateHorizontalValue()
        {
            HorizontalOffset = await SfBaseUtils.UpdateProperty(HOffset, HorizontalOffset, HorizontalOffsetChanged, null, null);
        }

        internal async void UpdateVerticalValue()
        {
            VerticalOffset = await SfBaseUtils.UpdateProperty(VOffset, VerticalOffset, VerticalOffsetChanged, null, null);
        }

        internal async void UpdateCurrentZoomValue()
        {
            CurrentZoom = await SfBaseUtils.UpdateProperty(CZoom, CurrentZoom, CurrentZoomChanged, null, null);
        }

        internal async Task PropertyUpdate(ScrollSettings scrollSettings)
        {
            HorizontalOffset = await SfBaseUtils.UpdateProperty(scrollSettings.HorizontalOffset, HorizontalOffset, HorizontalOffsetChanged, null, null);
            VerticalOffset = await SfBaseUtils.UpdateProperty(scrollSettings.VerticalOffset, VerticalOffset, VerticalOffsetChanged, null, null);
            CurrentZoom = await SfBaseUtils.UpdateProperty(scrollSettings.CurrentZoom, CurrentZoom, CurrentZoomChanged, null, null);
            MinZoom = await SfBaseUtils.UpdateProperty(scrollSettings.MinZoom, MinZoom, MinZoomChanged, null, null);
            MaxZoom = await SfBaseUtils.UpdateProperty(scrollSettings.MaxZoom, MaxZoom, MaxZoomChanged, null, null);
            ScrollLimit = await SfBaseUtils.UpdateProperty(scrollSettings.ScrollLimit, ScrollLimit, ScrollLimitChanged, null, null);
            PropertyInitialized();
            Parent?.UpdateScrollSettings(this);
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (ChildContent != null)
            {
                ChildContent = null;
            }
            if (Parent != null)
            {
                Parent = null;
            }
        }
    }
}

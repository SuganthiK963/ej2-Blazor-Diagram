using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Specifies how to customize the appearance, width, and height of the diagram page.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent @ref="diagram" Width="100%" Height="500px" Nodes="@nodes" Connectors="@connectors">
    ///     <PageSettings Width = "@PageWidth" Height="@PageHeight" Orientation="@pageOrientation" MultiplePage="@IsMultiplePage" ShowPageBreaks="@IsShowPageBreaks">
    ///         <PageMargin Left = "@marginLeft" Right="@marginRight" Top="@marginTop" Bottom="@marginBottom"></PageMargin>
    ///     </PageSettings>
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     SfDiagramComponent diagram;
    ///     double PageWidth = 300;
    ///     double PageHeight = 300;
    ///     PageOrientation pageOrientation = PageOrientation.Landscape;
    ///     bool IsMultiplePage = true;
    ///     bool IsShowPageBreaks = true;
    ///     double marginLeft = 10;
    ///     double marginRight = 10;
    ///     double marginTop = 10;
    ///     double marginBottom = 10;
    ///     DiagramObjectCollection<Node> nodes; DiagramObjectCollection<Connector> connectors;
    ///     protected override void OnInitialized()
    ///     {
    ///         nodes = new DiagramObjectCollection<Node>()
    ///         {
    ///             new Node { ID = "node1", Width = 150, Height = 100, OffsetX = 100, OffsetY = 100, Annotations = new DiagramObjectCollection<ShapeAnnotation>() { new ShapeAnnotation() { Content = "Node1" } } },
    ///             new Node { ID = "node2", Width = 80, Height = 130, OffsetX = 200, OffsetY = 200, Annotations = new DiagramObjectCollection<ShapeAnnotation>() { new ShapeAnnotation() { Content = "Node2" } } },
    ///             new Node { ID = "node3", Width = 100, Height = 75, OffsetX = 300, OffsetY = 350, Annotations = new DiagramObjectCollection<ShapeAnnotation>() { new ShapeAnnotation() { Content = "Node3" } } }
    ///         };
    ///         connectors = new DiagramObjectCollection<Connector> 
    ///         {
    ///             new Connector { ID="connector1", SourcePoint=new DiagramPoint { X=300, Y=400}, TargetPoint = new DiagramPoint { X = 500, Y = 500 } }
    ///         };
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public partial class PageSettings : SfBaseComponent
    {
        private double? width = 1123;
        private double? height = 794;
        private PageOrientation orientation = PageOrientation.Landscape;
        private BoundaryConstraints boundaryConstraints = BoundaryConstraints.Infinity;
        private bool multiplePage;
        private bool showPageBreaks;

        [JsonIgnore]
        internal string DiagramID { get; set; } = string.Empty;

        [CascadingParameter]
        [JsonIgnore]
        internal SfDiagramComponent Parent { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the width value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double?> WidthChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the height value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double?> HeightChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the Orientation value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<PageOrientation> OrientationChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the BoundaryConstraints value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<BoundaryConstraints> BoundaryConstraintsChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the ShowPageBreaks value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<bool> ShowPageBreaksChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the MultiplePage value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<bool> MultiplePageChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the Background value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<bool> BackgroundChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the Margin value changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<bool> MarginChanged { get; set; }

        /// <summary>
        /// Gets or sets the child content of the page settings.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the width of the diagram page.
        /// </summary>
        [Parameter]
        [JsonPropertyName("width")]
        public double? Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the diagram page.
        /// </summary>
        [Parameter]
        [JsonPropertyName("height")]
        public double? Height { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the page in the diagram. By default, the orientation of the page is set to landscape.
        /// </summary>
        [Parameter]
        [JsonPropertyName("orientation")]
        public PageOrientation Orientation { get; set; } = PageOrientation.Landscape;

        /// <summary>
        /// Allows the user to customize the interactive region.
        /// </summary>
        /// <remarks>
        /// The boundary constraints property restricts or customizes the interactive region, out of which the elements cannot be dragged, resized or rotated.
        /// </remarks>
        [Parameter]
        [JsonPropertyName("boundaryConstraints")]
        public BoundaryConstraints BoundaryConstraints { get; set; } = BoundaryConstraints.Infinity;

        /// <summary>
        /// Allows users to enable or disable multiple pages.
        /// </summary>
        /// <remarks>
        /// When multiple pages are enabled, the size of the page dynamically increases or decreases to split the single page into multiple pages and completely fit the diagram within the page boundaries. 
        /// </remarks>
        [Parameter]
        [JsonPropertyName("multiplePage")]
        public bool MultiplePage { get; set; }

        /// <summary>
        /// Allows the user to enable or disable the page break lines.
        /// </summary>
        /// <remarks>
        /// The ShowPageBreaks property is used as a visual guide to see how pages are split into multiple pages. By default, it is false. If it is true, then the page break lines will be visible.
        /// </remarks>
        [Parameter]
        [JsonPropertyName("showPageBreaks")]
        public bool ShowPageBreaks { get; set; }

        /// <summary>
        /// Specifies the extra space around the diagram content. The default values for the margin are set to 25 on all sides.
        /// </summary>
        /// <example>
        /// <code>
        /// <SfDiagramComponent Width="100%" Height="500px" Nodes="@nodes" Connectors="@connectors">
        ///     <PageSettings Width = "@PageWidth" Height="@PageHeight" Orientation="@pageOrientation" MultiplePage="@IsMultiplePage" ShowPageBreaks="@IsShowPageBreaks">
        ///         <PageMargin Left = "10" Right="10" Top="10" Bottom="10"></PageMargin>
        ///     </PageSettings>
        /// </SfDiagramComponent>
        /// </code>
        /// </example>
        [JsonPropertyName("margin")]
        public PageMargin Margin { get; set; }

        /// <summary>
        /// Defines the page background.
        /// </summary>
        /// <remarks>
        /// Users can customize the background of the diagram page by using the background property.  The Source property of background allows the user to set the path of the image to the background, whereas the Color property of the background allows the user to set a color to the background of the diagram page. By default, it is set to transparent.
        /// </remarks>
        [JsonPropertyName("background")]
        public BackgroundStyle Background { get; set; }

        internal void ValidatePageSize()
        {
            double? temp;
            if (Orientation == PageOrientation.Portrait)
            {
                if (Width > Height)
                {
                    temp = Height;
                    Height = Width;
                    Width = temp;
                }
            }
            else
            {
                if (Height > Width)
                {
                    temp = Width;
                    Width = Height;
                    Height = temp;
                }
            }
        }

        internal static PageSettings Initialize()
        {
            PageSettings pagesettings = new PageSettings();
            pagesettings.Background ??= new BackgroundStyle();
            pagesettings.Margin ??= new PageMargin();
            return pagesettings;
        }

        internal void UpdateBackgroundValues(BackgroundStyle background)
        {
            Background = background ?? new BackgroundStyle();
            Parent.UpdatePageSettings(this);
        }

        internal void UpdateMarginValues(PageMargin margin)
        {
            Margin = margin ?? new PageMargin();
            Parent.UpdatePageSettings(this);
        }

        #region Life Cycle methods
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            width = Width;
            height = Height;
            orientation = Orientation;
            boundaryConstraints = BoundaryConstraints;
            multiplePage = MultiplePage;
            showPageBreaks = ShowPageBreaks;
            UpdateBackgroundValues(this.Background);
            UpdateMarginValues(this.Margin);
            Parent.UpdatePageSettings(this);
        }
        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            width = BaseUtil.UpdateDictionary(nameof(Width), width, Width, PropertyChanges);
            height = BaseUtil.UpdateDictionary(nameof(Height), height, Height, PropertyChanges);
            orientation = BaseUtil.UpdateDictionary(nameof(Orientation), orientation, Orientation, PropertyChanges);
            boundaryConstraints = BaseUtil.UpdateDictionary(nameof(BoundaryConstraints), boundaryConstraints, BoundaryConstraints, PropertyChanges);
            multiplePage = BaseUtil.UpdateDictionary(nameof(MultiplePage), multiplePage, MultiplePage, PropertyChanges);
            showPageBreaks = BaseUtil.UpdateDictionary(nameof(ShowPageBreaks), showPageBreaks, ShowPageBreaks, PropertyChanges);
            if (PropertyChanges.Any())
                Parent.UpdatePageSettings(this);
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
                this.Parent.DiagramStateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
        }

        #endregion

        internal async Task PropertyUpdate(PageSettings pageSettings)
        {
            Width = await SfBaseUtils.UpdateProperty(pageSettings.Width, Width, WidthChanged, null, null);
            Height = await SfBaseUtils.UpdateProperty(pageSettings.Height, Height, HeightChanged, null, null);
            Orientation = await SfBaseUtils.UpdateProperty(pageSettings.Orientation, Orientation, OrientationChanged, null, null);
            MultiplePage = await SfBaseUtils.UpdateProperty(pageSettings.MultiplePage, MultiplePage, MultiplePageChanged, null, null);
            ShowPageBreaks = await SfBaseUtils.UpdateProperty(pageSettings.ShowPageBreaks, ShowPageBreaks, ShowPageBreaksChanged, null, null);
            BoundaryConstraints = await SfBaseUtils.UpdateProperty(pageSettings.BoundaryConstraints, BoundaryConstraints, BoundaryConstraintsChanged, null, null);

            if (pageSettings.Background != null)
                Background = await Background.PropertyUpdate(pageSettings.Background);
            else Background = null;
            if (pageSettings.Margin != null)
                Margin = await Margin.PropertyUpdate(pageSettings.Margin) as PageMargin;
            else Margin = null;
            width = Width;
            height = Height;
            orientation = Orientation;
            boundaryConstraints = BoundaryConstraints;
            multiplePage = MultiplePage;
            showPageBreaks = ShowPageBreaks;
            if (Parent != null)
            {
                UpdateBackgroundValues(this.Background);
                UpdateMarginValues(this.Margin);
                Parent.UpdatePageSettings(this);
            }
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (DiagramID != null)
            {
                DiagramID = null;
            }
            if (Parent != null)
            {
                Parent = null;
            }
            if (ChildContent != null)
            {
                ChildContent = null;
            }
            if (Width != null)
            {
                Width = null;
            }
            if (width != null)
            {
                width = null;
            }
            if (Height != null)
            {
                Height = null;
            }
            if (height != null)
            {
                height = null;
            }

            if (Margin != null)
            {
                Margin.Dispose();
                Margin = null;
            }
            if (Background != null)
            {
                Background.Dispose();
                Background = null;
            }
        }
    }
}
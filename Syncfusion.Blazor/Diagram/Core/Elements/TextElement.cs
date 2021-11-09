using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the text element which is used to display text or annotations.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="1000px" @bind-Nodes="Nodes" SetNodeTemplate="SetTemplate">
    /// </SfDiagramComponent>    
    /// @code
    /// { 
    ///     DiagramObjectCollection<Node> Nodes = new DiagramObjectCollection<Node>();
    ///     private ICommonElement SetTemplate(IDiagramObject node)
    ///     {
    ///         if ((node as Node).ID == "node2")
    ///         {
    ///             Canvas container = new Canvas();
    ///             TextElement textElement = new TextElement();
    ///             textElement.Content = "node2content";
    ///             container.Children.Add(textElement);
    ///             return container;
    ///         }
    ///          return null;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class TextElement : DiagramElement
    {
        private TextBounds textWrapBounds;
        private ObservableCollection<SubTextElement> textNodes = new ObservableCollection<SubTextElement>();
        /// <summary>
        /// The image source
        /// </summary>
        private string textContent;
        internal bool CanMeasure { get; set; } = true;
        internal bool CanConsiderBounds { get; set; } = true;
        /// <summary>
        /// sets the hyperlink color to blue
        /// </summary>
        internal HyperlinkSettings Hyperlink = new HyperlinkSettings() { Color = "blue" };
        /// <summary>
        /// The constraints for the text element
        /// </summary>
        internal AnnotationConstraints Constraints;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextElement"/>.
        /// </summary>
        public TextElement() : base()
        {
            this.Style.Fill = "transparent";
            Style = new TextStyle()
            {
                Color = "black",
                Fill = "transparent",
                StrokeColor = "black",
                StrokeWidth = 1,
                FontFamily = "Arial",
                FontSize = 12,
                WhiteSpace = WhiteSpace.CollapseSpace,
                TextWrapping = TextWrap.WrapWithOverflow,
                TextAlign = TextAlign.Center,
                Italic = false,
                Bold = false,
                TextDecoration = TextDecoration.None,
                StrokeDashArray = string.Empty,
                Opacity = 5,
                Gradient = null,
                TextOverflow = TextOverflow.Wrap
            };
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TextElement"/> from the given TextElement.
        /// </summary>
        /// <param name="src">TextElement.</param>
        public TextElement(TextElement src) : base(src)
        {
            if (src != null)
            {
                CanMeasure = src.CanMeasure;
                CanConsiderBounds = src.CanConsiderBounds;
                if (src.Hyperlink != null)
                {
                    Hyperlink = src.Hyperlink.Clone() as HyperlinkSettings;
                }
            }
        }
        /// <summary>
        /// Gets or sets the content of the text element that is to be displayed.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content
        {
            get => textContent;
            set
            {
                if (this.textContent != value)
                {
                    this.textContent = value;
                    this.IsDirt = true;
                }
            }
        }
        /// <summary>
        /// Gets or sets the child element for the text element.
        /// </summary>
        [JsonIgnore]
        internal ObservableCollection<SubTextElement> ChildNodes
        {
            get => textNodes;
            set
            {
                if (textNodes != value)
                {
                    textNodes = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the wrapBounds for the text.
        /// </summary>
        [JsonIgnore]
        internal TextBounds WrapBounds
        {
            get => textWrapBounds;
            set
            {
                if (textWrapBounds != value)
                {
                    textWrapBounds = value;
                    this.IsDirt = true;
                }
            }
        }

        internal void RefreshTextElement()
        {
            this.IsDirt = true;
        }

        /// <summary>
        /// Measures the minimum size that is required for the text element
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        internal override DiagramSize Measure(DiagramSize availableSize)
        {
            DiagramSize size = new DiagramSize();
            if (this.IsDirt && this.CanMeasure)
            {
                TextElementUtils obj = DomUtil.MeasureText(this.ID);
                if (obj != null)
                {
                    obj.Content = this.Content;
                    obj.Style = this.Style as TextStyle;
                    size = obj.Bounds;
                    this.WrapBounds = obj.WrapBounds;
                    this.ChildNodes = obj.ChildNodes;
                }
            }
            else
            {
                size = this.DesiredSize;
            }
            if (this.Width == null || this.Height == null)
            {
                this.DesiredSize = new DiagramSize() { Width = size.Width, Height = size.Height };
            }
            else
            {
                this.DesiredSize = new DiagramSize() { Width = (double)this.Width, Height = (double)this.Height };
            }
            this.DesiredSize = this.ValidateDesiredSize(this.DesiredSize, availableSize);
            return this.DesiredSize;
        }

        /// <summary>
        /// Arranges the text element
        /// </summary>
        /// <param name="desiredSize"></param>
        /// <param name="isStack"></param>
        internal override DiagramSize Arrange(DiagramSize desiredSize, bool? isStack)
        {
            this.ActualSize = desiredSize;
            this.UpdateBounds();
            this.IsDirt = false;
            return this.ActualSize;
        }
        /// <summary>
        /// Creates a new object that is a copy of the current text element.
        /// </summary>
        /// <returns>TextElement</returns>
        public override object Clone()
        {
            return new TextElement(this);
        }
        internal override void Dispose()
        {
            base.Dispose();
            if (textWrapBounds != null)
            {
                textWrapBounds = null;
            }
            if (textNodes != null)
            {
                for (int i = 0; i < textNodes.Count; i++)
                {
                    textNodes[i].Dispose();
                    textNodes[i] = null;
                }
                textNodes.Clear();
                textNodes = null;
            }
            textContent = null;
            if (Hyperlink != null)
            {
                Hyperlink.Dispose();
                Hyperlink = null;
            }

        }
    }
}

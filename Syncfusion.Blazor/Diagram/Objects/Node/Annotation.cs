using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents a control for creating a link to another Web page.
    /// </summary>
    /// <remarks>
    /// The hyperlink can be customized by adding text and color. The Hyperlink can be set to the Annotations of the node/connector.
    /// </remarks>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     // Position of the node
    ///     OffsetX = 250,
    ///     OffsetY = 250,
    ///     // Size of the node
    ///     Width = 100,
    ///     Height = 100,
    ///     Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "white" },
    ///     // Initialize annotation collection
    ///     Annotations = new DiagramObjectCollection<ShapeAnnotation>() 
    ///     { 
    ///         // Add text as hyperlink.
    ///            new ShapeAnnotation { Hyperlink = new Hyperlink{ Content = "Syncfusion", Link = "https://www.syncfusion.com" } }
    ///     },
    /// };
    /// ]]>
    /// </code>
    /// </example>

    public class HyperlinkSettings : DiagramObject
    {
        private string color = "blue";
        private string content;
        private string link;
        private TextDecoration textDecoration = TextDecoration.None;
        /// <summary>
        /// Creates a new instance of the Hyperlink from the given Hyperlink.
        /// </summary>
        /// <param name="src">Hyperlink</param>
        public HyperlinkSettings(HyperlinkSettings src) : base(src)
        {
            if (src != null)
            {
                color = src.color;
                content = src.content;
                link = src.link;
                textDecoration = src.textDecoration;
            }
        }
        /// <summary>
        /// Initializes a new instance of the Hyperlink.
        /// </summary>
        public HyperlinkSettings() : base()
        {
        }
        /// <summary>
        /// Gets or sets the fill color of the hyperlink. Color is string type.
        /// </summary>
        [JsonPropertyName("color")]
        public string Color
        {
            get => color;
            set
            {
                if (color != value)
                {
                    Parent?.OnPropertyChanged(nameof(Color), value, color, this);
                    color = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the content of the hyperlink.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content
        {
            get => content;
            set
            {
                if (content != value)
                {
                    Parent?.OnPropertyChanged(nameof(Content), value, content, this);
                    content = value;
                }
            }
        }
        /// <summary>
        /// Defines the link to be set for the hyperlink.
        /// </summary>
        [JsonPropertyName("link")]
        public string Url
        {
            get => link;
            set
            {
                if (link != value)
                {
                    Parent?.OnPropertyChanged(nameof(Url), value, link, this);
                    link = value;
                }
            }
        }

        /// <summary>
        /// Defines the text-decoration for the content of the hyperlink.
        /// </summary>
        /// <remarks>
        /// The types of text decorations are underline, overline, linethrough and none. By default, TextDecoration is set to None.
        /// </remarks>
        [JsonPropertyName("textDecoration")]
        public TextDecoration TextDecoration
        {
            get => textDecoration;
            set
            {
                if (textDecoration != value)
                {
                    Parent?.OnPropertyChanged(nameof(TextDecoration), value, textDecoration, this);
                    textDecoration = value;
                }
            }
        }
        /// <summary>
        /// Creates a new hyperlink that is a copy of the current hyperlink.
        /// </summary>
        /// <returns>Hyperlink</returns>
        public override object Clone()
        {
            return new HyperlinkSettings(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            color = null;
            content = null;
            link = null;
        }
    }
    /// <summary>
    /// Defines the textual content of nodes/connectors. 
    /// </summary>
    public class Annotation : DiagramObject
    {
        private string id;
        internal bool IsDirtAnnotation;
        private string content = string.Empty;
        private bool visibility = true;
        private AnnotationConstraints constraints = AnnotationConstraints.InheritReadOnly;
        private HyperlinkSettings hyperlink;
        private double? width;
        private double? height;
        private double rotationAngle;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Center;
        private Margin margin = new Margin();
        private Margin dragLimit = new Margin();
        private Dictionary<string, object> additionalInfo = new Dictionary<string, object>();
        private TextStyle style = new TextStyle() { StrokeWidth = 0, StrokeColor = "transparent", Fill = "transparent" };
        /// <summary>
        /// Creates a new instance of an Annotation from the given Annotation.
        /// </summary>
        /// <param name="src">Annotation</param>
        public Annotation(Annotation src) : base(src)
        {
            if (src != null)
            {
                id = string.IsNullOrEmpty(src.ID) ? BaseUtil.RandomId() : src.ID;
                IsDirtAnnotation = src.IsDirtAnnotation;
                content = src.content;
                visibility = src.visibility;
                Constraints = src.constraints;
                if (src.hyperlink != null)
                {
                    hyperlink = src.hyperlink.Clone() as HyperlinkSettings;
                }
                width = src.width;
                height = src.height;
                rotationAngle = src.rotationAngle;
                horizontalAlignment = src.horizontalAlignment;
                verticalAlignment = src.verticalAlignment;
                if (src.margin != null)
                {
                    margin = src.margin.Clone() as Margin;
                }
                if (src.dragLimit != null)
                {
                    dragLimit = src.dragLimit.Clone() as Margin;
                }
                additionalInfo = src.additionalInfo;
                if (src.style != null)
                {
                    style = src.style.Clone() as TextStyle;
                };
            }
        }
        /// <summary>
        /// Initializes a new instance of an Annotation.
        /// </summary>
        public Annotation() : base()
        {
            id = BaseUtil.RandomId();
        }
        /// <summary>
        /// Gets or sets the unique id of the node's or connector's annotation. 
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none"> 1. The ID needs to be unique. While creating a label, the user should not use the same id for other labels</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 2. If multiple labels have the same ID, then unexpected behavior could occur.</td>
        /// </tr>        
        /// </table>
        /// </remarks>
        [JsonPropertyName("id")]
        public string ID
        {
            get => id;
            set
            {
                if (id != value)
                {
                    Parent?.OnPropertyChanged(nameof(ID), value, id, this);
                    id = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the textual information of the node/connector.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Node node = new Node()
        /// {
        ///     // Position of the node
        ///     OffsetX = 250,
        ///     OffsetY = 250,
        ///     // Size of the node
        ///     Width = 100,
        ///     Height = 100,
        ///     Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "white" },
        ///     // Initialize annotation collection
        ///     Annotations = new DiagramObjectCollection<ShapeAnnotation>() 
        ///     { 
        ///         new ShapeAnnotation { Content = "Node" }
        ///     },
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("content")]
        public string Content
        {
            get => content;
            set
            {
                if (content != value)
                {
                    if (Parent != null)
                    {
                        IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Content), value, content, this);
                    }
                    content = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the annotation. By default, it is visible (True). 
        /// </summary>
        [JsonPropertyName("visibility")]
        public bool Visibility
        {
            get => visibility;
            set
            {
                if (visibility != value)
                {
                    Parent?.OnPropertyChanged(nameof(Visibility), value, visibility, this);
                    visibility = value;
                }
            }
        }
        /// <summary>
        /// Enable or disable certain behaviors of the label. All the interactive functionalities are enabled by default.
        /// </summary>
        /// <remarks>
        /// For instance, the user can disable annotation editing and be able to make it read-only. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Node node = new Node()
        /// {
        ///     // Position of the node
        ///     OffsetX = 250,
        ///     OffsetY = 250,
        ///     // Size of the node
        ///     Width = 100,
        ///     Height = 100,
        ///     Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "white" },
        ///     // Initialize annotation collection
        ///     Annotations = new DiagramObjectCollection<ShapeAnnotation>() 
        ///     { 
        ///         new ShapeAnnotation 
        ///         { 
        ///             Content = "Node",
        ///             Constraints=AnnotationConstraints.ReadOnly,
        ///         }
        ///     },
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("constraints")]
        public AnnotationConstraints Constraints
        {
            get => constraints;
            set
            {
                if (constraints != value)
                {
                    Parent?.OnPropertyChanged(nameof(Constraints), value, constraints, this);
                    constraints = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the hyperlink of the annotation. 
        /// </summary>
        /// <remarks>
        /// Users can be able to show hyperlinks as annotation content.
        /// </remarks>
        [JsonPropertyName("hyperlink")]
        public HyperlinkSettings Hyperlink
        {
            get
            {
                if (hyperlink != null && hyperlink.Parent == null)
                    hyperlink.SetParent(this, nameof(Hyperlink));
                return hyperlink;
            }
            set
            {
                if (hyperlink != value)
                {
                    if (Parent != null)
                    {
                        IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Hyperlink), value, hyperlink, this);
                    }
                    hyperlink = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the width of the annotation. If width is not specified, it displays based on the content's width.
        /// </summary>
        [JsonPropertyName("width")]
        public double? Width
        {
            get => width;
            set
            {
                if (!Equals(width, value))
                {
                    if (Parent != null)
                    {
                        IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Width), value, width, this);
                    }
                    width = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the height of the annotation. If the height is not specified, it renders based on the content's height. 
        /// </summary>
        [JsonPropertyName("height")]
        public double? Height
        {
            get => height;
            set
            {
                if (!Equals(height, value))
                {
                    if (Parent != null)
                    {
                        IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Height), value, height, this);
                    }
                    height = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the annotation. It is 0 by default. 
        /// </summary>
        [JsonPropertyName("rotationAngle")]
        public double RotationAngle
        {
            get => rotationAngle;
            set
            {
                if (!rotationAngle.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(RotationAngle), value, rotationAngle, this);
                    rotationAngle = value;
                }
            }
        }

        /// <summary>
        /// Represents the appearance of an annotation. 
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Node node = new Node()
        /// {
        ///     ID = "node1",
        ///     Width = 100,
        ///     Height = 100,
        ///     OffsetX = 100,
        ///     OffsetY = 100,
        ///     // Set the textAlign as left for the content
        ///     Annotations = new DiagramObjectCollection<ShapeAnnotation>()
        ///     {
        ///         new ShapeAnnotation 
        ///         { 
        ///             Content = "Text align is set as Left",
        ///             Style = new TextStyle() { TextAlign = TextAlign.Left}
        ///         }
        ///     },
        ///     Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "white" },
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("style")]
        public TextStyle Style
        {
            get
            {
                if (style != null && style.Parent == null)
                    style.SetParent(this, nameof(Style));
                return style;
            }
            set
            {
                if (style != value)
                {
                    if (Parent != null)
                    {
                        IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Style), value, style, this);
                    }
                    style = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the text to the parent node/connector.
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">The following options are used to define the Horizontal Alignment of the annotation. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 1. Stretch - Stretches the diagram element throughout the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 2. Left - Aligns the diagram element at the left of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 3. Right - Aligns the diagram element at the right of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 4. Center - Aligns the diagram element at the center of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 5. Auto - Aligns the diagram element based on the characteristics of the node.</td>
        /// </tr>
        /// </table>
        /// </remarks>
        [JsonPropertyName("horizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment != value)
                {
                    Parent?.OnPropertyChanged(nameof(HorizontalAlignment), value, horizontalAlignment, this);
                    horizontalAlignment = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the text to the parent node/connector. 
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">The following options are used to define the Vertical Alignment of the annotation. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 1. Stretch  - Stretches the diagram element throughout the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 2. Top - Aligns the diagram element at the top of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 3. Bottom - Aligns the diagram element at the bottom of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 4. Center - Aligns the diagram element at the center of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 5. Auto - Aligns the diagram element based on the characteristics of the node.</td>
        /// </tr>
        /// </table>
        /// </remarks>
        [JsonPropertyName("verticalAlignment")]
        public VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment != value)
                {
                    Parent?.OnPropertyChanged(nameof(VerticalAlignment), value, verticalAlignment, this);
                    verticalAlignment = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space around an annotation present in the node/connector.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Annotations = new DiagramObjectCollection<ShapeAnnotation>()
        /// {
        ///     new ShapeAnnotation 
        ///     { 
        ///         Content = "Task1",
        ///         Margin = new Margin() { Top = 10},
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("margin")]
        public Margin Margin
        {
            get
            {
                if (margin != null && margin.Parent == null)
                    margin.SetParent(this, nameof(Margin));
                return margin;
            }
            set
            {
                if (margin != value)
                {
                    if (Parent != null)
                    {
                        IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Margin), value, margin, this);
                    }
                    margin = value;
                }
            }
        }
        /// <summary>
        /// Sets the space to be left between an annotation and its parent node/connector
        /// </summary>
        [JsonPropertyName("dragLimit")]
        internal Margin DragLimit
        {
            get
            {
                if (dragLimit != null && dragLimit.Parent == null)
                    dragLimit.SetParent(this, nameof(DragLimit));
                return dragLimit;
            }
            set
            {
                if (dragLimit != value)
                {
                    Parent?.OnPropertyChanged(nameof(DragLimit), value, dragLimit, this);
                    dragLimit = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the custom properties of an annotation.
        /// </summary>
        /// <remarks>
        /// Enables the user to store data of any data type. It will be serialized and deserialized automatically while saving and opening the diagram. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Dictionary<string, object> dict = new Dictionary<string, object>();
        /// dict.Add("node", "Annotation1");
        /// Annotations = new DiagramObjectCollection<ShapeAnnotation>()
        /// {
        ///     new ShapeAnnotation()
        ///     {
        ///        Content = "Annotation",
        ///        AdditionalInfo = dict
        ///     }
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("additionalInfo")]
        public Dictionary<string, object> AdditionalInfo
        {
            get => additionalInfo;
            set
            {
                if (additionalInfo != value)
                {
                    Parent?.OnPropertyChanged(nameof(AdditionalInfo), value, additionalInfo, this);
                    additionalInfo = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current annotation. 
        /// </summary>
        /// <returns>A new object that is a copy of this annotation</returns>
        public override object Clone()
        {
            return new Annotation(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (margin != null)
            {
                margin = null;
            }

            if (dragLimit != null)
            {
                dragLimit = null;
            }

            if (style != null)
            {
                style = null;
            }
        }
    }

    /// <summary>
    /// Defines the textual description of nodes/connectors with respect to bounds
    /// Represents the block of text displayed over the node.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     // Position of the node
    ///     OffsetX = 250,
    ///     OffsetY = 250,
    ///     // Size of the node
    ///     Width = 100,
    ///     Height = 100,
    ///     Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "white" },
    ///     // Initialize the annotation collection
    ///     Annotations = new DiagramObjectCollection<ShapeAnnotation>() { new ShapeAnnotation { Content = "Node" }, Offset = new DiagramPoint() { X = 0, Y = 0 } },
    /// };
    /// ]]>
    /// </code>
    /// </example>
    public class ShapeAnnotation : Annotation
    {
        private DiagramPoint offset = new DiagramPoint() { X = 0.5, Y = 0.5 };
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeAnnotation"/>.
        /// </summary>
        public ShapeAnnotation() : base()
        {
        }
        /// <summary>
        /// Creates a new instance of the <see cref="ShapeAnnotation"/> from the given <see cref="ShapeAnnotation"/>.
        /// </summary>
        /// <param name="src">ShapeAnnotation</param>
        public ShapeAnnotation(ShapeAnnotation src) : base(src)
        {
            if (src != null && src.offset != null)
            {
                offset = src.offset.Clone() as DiagramPoint;
            }
        }
        /// <summary>
        /// Gets or sets the position of the annotation to its parent bounds.
        /// </summary>
        [JsonPropertyName("offset")]
        public DiagramPoint Offset
        {
            get
            {

                if (offset != null && offset.Parent == null)
                    offset.SetParent(this, nameof(Offset));
                return offset;
            }
            set
            {
                if (offset != value)
                {
                    Parent?.OnPropertyChanged(nameof(Offset), value, offset, this);
                    offset = value;
                }
            }
        }
        /// <summary>
        /// Creates a new annotation that is a copy of the current annotation.
        /// </summary>
        /// <returns>ShapeAnnotation</returns>
        public override object Clone()
        {
            return new ShapeAnnotation(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (offset != null)
            {
                offset = null;
            }
        }
    }
    /// <summary>
    /// Represents the block of text displayed over the connector.
    /// </summary>
    public class PathAnnotation : Annotation
    {
        private double offset = 0.5;
        private DiagramPoint displacement = new DiagramPoint();
        private AnnotationAlignment alignment = AnnotationAlignment.Center;
        private bool segmentAngle;
        /// <summary>
        /// Creates a new instance of the PathAnnotation from the given PathAnnotation.
        /// </summary>
        /// <param name="src">PathAnnotation</param>
        public PathAnnotation(PathAnnotation src) : base(src)
        {
            if (src != null)
            {
                offset = src.offset;
                displacement = src.displacement;
                alignment = src.alignment;
                segmentAngle = src.segmentAngle;
            }
        }
        /// <summary>
        /// Initializes a new instance of the PathAnnotation.
        /// </summary>
        public PathAnnotation() : base()
        {
        }
        /// <summary>
        /// Gets or sets the offset of an annotation in a connector. By default, it is 0.5 
        /// </summary>
        [JsonPropertyName("offset")]
        public double Offset
        {
            get => offset;
            set
            {
                if (!offset.Equals(value))
                {
                    double oldValue = offset;
                    offset = value;
                    Parent?.OnPropertyChanged(nameof(Offset), value, oldValue, this);
                }
            }
        }
        /// <summary>
        /// Gets or sets the displacement (margin) of an annotation from its actual position. Applicable only for connector.
        /// </summary>
        [JsonPropertyName("displacement")]
        public DiagramPoint Displacement
        {
            get
            {
                if (displacement != null && displacement.Parent == null)
                    displacement.SetParent(this, nameof(Displacement));
                return displacement;
            }
            set
            {
                if (displacement != value)
                {
                    DiagramPoint oldValue = displacement;
                    displacement = value;
                    Parent?.OnPropertyChanged(nameof(Displacement), value, oldValue, this);
                }
            }
        }
        /// <summary>
        /// Gets or sets the alignment of an annotation in a connector. By default, it is aligned at the center. 
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none"> Below are the available alignment options</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 1. Center, Annotation placed on the connector segment.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 2. Before,Annotation is placed at the top of the connector segment.</td>
        /// </tr>   
        /// <tr>
        /// <td style = "border: none"> 3. After, Annotation is placed at the bottom of the connector segment.</td>
        /// </tr> 
        /// </table>
        /// </remarks>
        [JsonPropertyName("alignment")]
        public AnnotationAlignment Alignment
        {
            get => alignment;
            set
            {
                if (alignment != value)
                {
                    AnnotationAlignment oldValue = alignment;
                    alignment = value;
                    Parent?.OnPropertyChanged(nameof(Alignment), value, oldValue, this);
                    alignment = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the segment angle of the connector.
        /// </summary>
        [JsonPropertyName("segmentAngle")]
        public bool SegmentAngle
        {
            get => segmentAngle;
            set
            {
                if (segmentAngle != value)
                {
                    Parent?.OnPropertyChanged(nameof(SegmentAngle), value, segmentAngle, this);
                    segmentAngle = value;
                }
            }
        }
        /// <summary>
        /// Creates a new PathAnnotation that is a copy of the current annotation.
        /// </summary>
        /// <returns>PathAnnotation</returns>
        public override object Clone()
        {
            return new PathAnnotation(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (displacement != null)
            {
                displacement.Dispose();
                displacement = null;
            }
        }
    }
}
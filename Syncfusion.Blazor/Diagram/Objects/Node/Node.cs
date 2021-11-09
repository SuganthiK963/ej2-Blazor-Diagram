using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Diagram.SymbolPalette;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{

    /// <summary>
    /// Represents the shape that is used to visualize geometrical information. 
    /// </summary>
    [JsonConverter(typeof(NodeJsonConverter))]
    public class Node : NodeBase
    {
        [JsonIgnore]
        internal bool IsDirtNode { get; set; }
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Top;
        [JsonIgnore]
        internal string ProcessId { get; set; } = string.Empty;
        private NodeConstraints constraints = NodeConstraints.Default;
        private Shape shape = new BasicShape();
        private double borderWidth;
        [JsonIgnore]
        internal Bounds TreeBounds { get; set; }
        private string borderColor = "none";
        private Shadow shadow = new Shadow();
        private string backgroundColor = "transparent";
        private ShapeStyle style = new ShapeStyle() { Fill = "white" };
        private double rotationAngle;
        private double? maxHeight;
        private double? maxWidth;
        private double? minHeight;
        private double? minWidth;
        private double? height;
        private double? width;
        private DiagramPoint pivot = new DiagramPoint() { X = 0.5, Y = 0.5 };
        private double offsetY;
        private double offsetX;
        private bool isExpanded = true;
        private DiagramObjectCollection<PointPort> ports = new DiagramObjectCollection<PointPort>();
        private DiagramObjectCollection<ShapeAnnotation> annotations = new DiagramObjectCollection<ShapeAnnotation>();
        private DiagramObjectCollection<NodeFixedUserHandle> fixedUserHandles = new DiagramObjectCollection<NodeFixedUserHandle>();
        private object data;
        private ICommonElement template;
        [JsonIgnore]
        internal DiagramSize NativeSize { get; set; } = null;
        /// <summary>
        /// Gets information about the outgoing connectors of the node. 
        /// </summary>
        /// <remarks>
        /// It returns the ID of the outgoing connectors.
        /// </remarks>
        [JsonPropertyName("outEdges")]
        public List<string> OutEdges { get; internal set; } = new List<string>();
        /// <summary>
        /// Gets information about the incoming connectors of the node. 
        /// </summary>
        /// <remarks>
        /// It returns the ID of the incoming connectors.
        /// </remarks>
        [JsonPropertyName("inEdges")]
        public List<string> InEdges { get; internal set; } = new List<string>();
        /// <summary>
        /// Initializes a new instance of the Node.
        /// </summary>
        public Node() : base()
        {
            annotations.Parent = this;
            ports.Parent = this;
            fixedUserHandles.Parent = this;
        }
        /// <summary>
        /// Creates a new instance of the Node from the given Node.
        /// </summary>
        /// <param name="src">Node</param>
        public Node(Node src) : base(src)
        {
            if (src != null)
            {
                isExpanded = src.IsExpanded;
                IsDirtNode = src.IsDirtNode;
                offsetX = src.offsetX;
                offsetY = src.offsetY;
                rotationAngle = src.rotationAngle;
                maxHeight = src.maxHeight;
                NativeSize = src.NativeSize;
                data = src.data;
                maxWidth = src.maxWidth;
                minHeight = src.minHeight;
                minWidth = src.minWidth;
                horizontalAlignment = src.horizontalAlignment;
                borderWidth = src.borderWidth;
                borderColor = src.borderColor;
                constraints = src.constraints;
                height = src.height;
                width = src.width;
                ProcessId = src.ProcessId;
                backgroundColor = src.backgroundColor;
                verticalAlignment = src.verticalAlignment;
                if (src.style != null)
                {
                    style = src.style.Clone() as ShapeStyle;
                    if (style != null) style.SetParent(this, nameof(Style));
                }
                if (src.pivot != null)
                {
                    pivot = src.pivot.Clone() as DiagramPoint;
                    if (pivot != null) pivot.Parent = this;
                }
                if (src.shape != null)
                {
                    shape = src.shape.Clone() as Shape;
                    if (shape != null) shape.Parent = this;
                }
                if (src.shadow != null)
                {
                    shadow = src.shadow.Clone() as Shadow;
                    shadow.Parent = this;
                }
                ports = src.ports;
                fixedUserHandles = src.fixedUserHandles;
                annotations = new DiagramObjectCollection<ShapeAnnotation>();
                if (src.annotations.Count > 0)
                {
                    foreach (ShapeAnnotation annotation in src.annotations)
                    {
                        ShapeAnnotation annotationCLone = annotation.Clone() as ShapeAnnotation;
                        annotations.Add(annotationCLone);
                    }
                    annotations.Parent = this;
                }
                ports = new DiagramObjectCollection<PointPort>();
                if (src.ports.Count > 0)
                {
                    foreach (PointPort pointPort in src.ports)
                    {
                        PointPort port = pointPort.Clone() as PointPort;
                        ports.Add(port);
                    }
                    ports.Parent = this;
                }
                fixedUserHandles = new DiagramObjectCollection<NodeFixedUserHandle>();
                if (src.fixedUserHandles.Count > 0)
                {
                    foreach (NodeFixedUserHandle userHandle in src.fixedUserHandles)
                    {
                        NodeFixedUserHandle handle = userHandle.Clone() as NodeFixedUserHandle;
                        fixedUserHandles.Add(handle);
                    }
                    fixedUserHandles.Parent = this;
                }
                OutEdges = new List<string>();
                InEdges = new List<string>();
            }
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the node. By default, it is 0. 
        /// </summary>
        [JsonPropertyName("offsetX")]
        public double OffsetX
        {
            get => offsetX;
            set
            {
                if (!offsetX.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(OffsetX), value, offsetX, this);
                    offsetX = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the node. By default, it is 0. 
        /// </summary>
        [JsonPropertyName("offsetY")]
        public double OffsetY
        {
            get => offsetY;
            set
            {
                if (!offsetY.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(OffsetY), value, offsetY, this);
                    offsetY = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets a collection of PointPort (connection points). 
        /// </summary>
        /// <remarks>
        /// Ports act as the connection points between nodes and allow them to create connections with only those specific points. There may be any number of ports in a node. You can   modify the ports' appearance, visibility, positioning and can add custom shapes to them. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Node node = new Node()
        /// {
        ///    // Position of the node
        ///    OffsetX = 250,
        ///    OffsetY = 250,
        ///    // Size of the node
        ///    Width = 100,
        ///    Height = 100,
        ///    Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "white" },
        ///    // Initialize port collection
        ///    Ports = new DiagramObjectCollection<PointPort>()
        ///    {
        ///        // Set the position for the port
        ///        new PointPort()
        ///        {
        ///            Style = new ShapeStyle() { Fill = "gray" }, 
        ///            Offset = new DiagramPoint() { X = 0.5, Y = 0.5 }, 
        ///            Visibility = PortVisibility.Visible
        ///        }
        ///    }
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("ports")]
        public DiagramObjectCollection<PointPort> Ports
        {
            get
            {
                if (ports != null && ports.Parent == null)
                    ports.Parent = this;
                return ports;
            }
            set
            {
                if (value != null && ports != value)
                {
                    SfDiagramComponent diagram = null;
                    if (ports.Parent is NodeBase portsParent)
                    {
                        diagram = portsParent.Parent as SfDiagramComponent;
                    }
                    if (diagram != null)
                    {
                        if (diagram.IsRendered)
                        {
                            diagram.StartGroupAction();
                        }
                        if (annotations != null)
                            diagram.RemovePorts(ports.Parent as NodeBase, ports);
                    }
                    ports = value;
                    ports.Parent = this;
                    if (this.Parent != null && Wrapper != null)
                    {
                        _ = ports.InitializePorts(ports);
                        Parent.OnPropertyChanged(nameof(Ports), value, ports, this);
                    }
                    if (diagram != null && diagram.IsRendered)
                    {
                        diagram.EndGroupAction();
                    }
                }
                else
                    ports = value;
            }
        }
        /// <summary>
        /// Represents the collection of textual information contained in the node.
        /// </summary>
        /// <remarks>
        /// The text found in the node can be edited at runtime. Users are able to modify the   style, visibility, width, height, and content of the annotation. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Node node = new Node()
        /// {
        ///    // Position of the node
        ///    OffsetX = 250,
        ///    OffsetY = 250,
        ///    // Size of the node
        ///    Width = 100,
        ///    Height = 100,
        ///    Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "white" },
        ///    // Initialize annotation collection
        ///    Annotations = new DiagramObjectCollection<ShapeAnnotation>() 
        ///    { 
        ///         new ShapeAnnotation 
        ///         { 
        ///             Content = "Node" 
        ///         }
        ///    },
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("annotations")]
        public DiagramObjectCollection<ShapeAnnotation> Annotations
        {
            get
            {
                if (annotations != null && annotations.Parent == null)
                    annotations.Parent = this;
                return annotations;
            }
            set
            {
                if (value != null && annotations != value)
                {
                    SfDiagramComponent diagram = ((NodeBase)annotations.Parent).Parent as SfDiagramComponent;
                    if (diagram != null)
                    {
                        if (diagram.IsRendered)
                        {
                            diagram.StartGroupAction();
                        }
                        if (annotations != null)
                            diagram.RemoveLabels(annotations.Parent as NodeBase, annotations);
                    }
                    annotations = value;
                    annotations.Parent = this;
                    if (this.Parent != null && Wrapper != null)
                    {
                        _ = annotations.InitializeAnnotations(annotations);
                        Parent.OnPropertyChanged(nameof(Annotations), value, annotations, this);
                    }
                    if (diagram != null && diagram.IsRendered)
                    {
                        diagram.EndGroupAction();
                    }
                }
                else
                    annotations = value;
            }
        }
        /// <summary>
        /// Gets or sets the collection of the fixed user handle of the node. 
        /// </summary>
        /// <remarks>
        /// The fixed user handles are used to add some frequently used commands around the node and connector even without selecting it. 
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
        ///     // A fixed user handle is created and stored in fixed user handle collection of Node.
        ///     FixedUserHandles = new DiagramObjectCollection<NodeFixedUserHandle>()
        ///     {
        ///         new NodeFixedUserHandle()
        ///         {
        ///             ID = "user1",
        ///             Height = 20, 
        ///             Width = 20, 
        ///             Visibility = true,
        ///             Padding = new Margin() { Bottom = 1, Left = 1, Right = 1, Top = 1 }, 
        ///             Margin = new Margin() { Right = 20 },Offset = new DiagramPoint() { X = 0, Y = 0 }, 
        ///             PathData = "M60.3,18H27.5c-3,0-5.5,2.4-5.5,5.5v38.2h5.5V23.5h32.7V18z M68.5,28.9h-30c-3,0-5.5,2.4-5.5,5.5v38.2c0,3,2.4,5.5,5.5,5.5h30c3,0,5.5-2.4,5.5-5.5V34.4C73.9,31.4,71.5,28.9,68.5,28.9z M68.5,72.5h-30V34.4h30V72.5z"
        ///         },
        ///     }
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("fixedUserHandles")]
        public DiagramObjectCollection<NodeFixedUserHandle> FixedUserHandles
        {
            get => fixedUserHandles;
            set
            {
                if (value != null && fixedUserHandles != value)
                {
                    fixedUserHandles = value;
                    fixedUserHandles.Parent = this;
                    if (this.Parent != null && Wrapper != null)
                    {
                        Parent.OnPropertyChanged(nameof(FixedUserHandles), value, fixedUserHandles, this);
                    }
                }
                else
                    fixedUserHandles = value;
            }
        }
        [JsonPropertyName("isExpanded")]
        internal bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded != value)
                {
                    Parent?.OnPropertyChanged(nameof(IsExpanded), value, isExpanded, this);
                    isExpanded = value;
                }
            }
        }
        /// <summary>
        /// Node rotation angle will be based on pivot values which range from 0 to 1 like offset values. By default, the Pivot values are set to X= 0.5 and Y=0.5.
        /// </summary>
        /// <remarks>
        /// The below list explains the pivot values.  
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none"> 1. When X= 0 and Y= 0, OffsetX and OffsetY values are considered to be at the top-left corner of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none"> 2. When X= 0.5 and Y= 0.5, OffsetX and OffsetY values are considered to be at the node’s center point.</td>
        /// </tr>      
        /// <tr>
        /// <td style = "border: none"> 3. When X= 1 and Y= 1, OffsetX and OffsetY values are considered to be at the bottom-right corner of the node.</td>
        /// </tr>      
        /// </table>
        /// </remarks>
        [JsonPropertyName("pivot")]
        public DiagramPoint Pivot
        {
            get
            {
                if (pivot != null && pivot.Parent == null)
                    pivot.SetParent(this, nameof(Pivot));
                return pivot;
            }
            set
            {
                if (pivot != value)
                {
                    Parent?.OnPropertyChanged(nameof(Pivot), value, pivot, this);
                    pivot = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the width of the node. If it is not specified, the node renders based on the content's width. 
        /// </summary>
        /// <remarks>
        /// The width of a node does not include borders or margins.
        /// </remarks>
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
                        IsDirtNode = true;
                        Parent.OnPropertyChanged(nameof(Width), value, width, this);
                    }
                    width = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the node. If it is not specified, the node renders based on the content's height.
        /// </summary>
        /// <remarks>
        /// The height of a node does not include borders or margins. 
        /// </remarks>
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
                        IsDirtNode = true;
                        Parent.OnPropertyChanged(nameof(Height), value, height, this);
                    }
                    height = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum width of the node. By default, it is undefined. 
        /// </summary>
        [JsonPropertyName("minWidth")]
        public double? MinWidth
        {
            get => minWidth;
            set
            {
                if (!Equals(minWidth, value))
                {
                    if (Parent != null)
                    {
                        IsDirtNode = true;
                        Parent.OnPropertyChanged(nameof(MinWidth), value, minWidth, this);
                    }
                    minWidth = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum height of the node. By default, it is undefined. 
        /// </summary>
        [JsonPropertyName("minHeight")]
        public double? MinHeight
        {
            get => minHeight;
            set
            {
                if (!Equals(minHeight, value))
                {
                    if (Parent != null)
                    {
                        IsDirtNode = true;
                        Parent.OnPropertyChanged(nameof(MinHeight), value, minHeight, this);
                    }
                    minHeight = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum width of the node. By default, it is undefined. 
        /// </summary>
        [JsonPropertyName("maxWidth")]
        public double? MaxWidth
        {
            get => maxWidth;
            set
            {
                if (!Equals(maxWidth, value))
                {
                    if (Parent != null)
                    {
                        IsDirtNode = true;
                        Parent.OnPropertyChanged(nameof(MaxWidth), value, maxWidth, this);
                    }
                    maxWidth = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum height of the node. By default, it is undefined. 
        /// </summary>
        [JsonPropertyName("maxHeight")]
        public double? MaxHeight
        {
            get => maxHeight;
            set
            {
                if (!Equals(maxHeight, value))
                {
                    if (Parent != null)
                    {
                        IsDirtNode = true;
                        Parent.OnPropertyChanged(nameof(MaxHeight), value, maxHeight, this);
                    }
                    maxHeight = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle of the node. The default value is 0. 
        /// </summary>
        [JsonPropertyName("rotateAngle")]
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
        /// Represents the appearance of the node. 
        /// </summary>
        /// <example>
        /// <code lang="Razor">
        /// <![CDATA[
        /// <SfDiagramComponent Height="600px" Nodes="@nodes" />
        ///  @code
        ///  {
        ///     //Initialize the node collection with node
        ///     DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>();
        ///     protected override void OnInitialized()
        ///     {
        ///         Node node = new Node()
        ///         {
        ///             ID = "node1",
        ///             //Size of the node
        ///             Height = 100,
        ///             Width = 100,
        ///             //Position of the node
        ///             OffsetX = 100,
        ///             OffsetY = 100,
        ///             //Set the type of shape as flow
        ///             Style = new ShapeStyle() 
        ///             {
        ///                 Fill = "#6BA5D7", 
        ///                 StrokeDashArray = "5,5", 
        ///                 StrokeColor = "red", 
        ///                 StrokeWidth = 2 
        ///             }
        ///         };
        ///         nodes.Add(node);
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("style")]
        public ShapeStyle Style
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
                    Parent?.OnPropertyChanged(nameof(Style), value, style, this);
                    style = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the shadow appearance of a node. 
        /// </summary>
        /// <remarks>
        /// The Shadow effect on a node is disabled by default. It can be enabled with the constraint property of the node. The Angle, Distance, and Opacity of the shadow can be customized. 
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
        ///     Style = new ShapeStyle() { Fill = "#6BA5D7", StrokeColor = "white" },
        ///     Constraints = NodeConstraints.Default | NodeConstraints.Shadow,
        ///     Shadow = new Shadow()
        ///     {
        ///         Angle = 50,
        ///         Color = "Blue",
        ///         Opacity = 0.8,
        ///         Distance = 20
        ///     }
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("shadow")]
        public Shadow Shadow
        {
            get
            {
                if (shadow != null && shadow.Parent == null)
                    shadow.SetParent(this, nameof(Shadow));
                return shadow;
            }
            set
            {
                if (shadow != value)
                {
                    Parent?.OnPropertyChanged(nameof(Shadow), value, shadow, this);
                    shadow = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the node's background color. By default, it is transparent.
        /// </summary>
        [JsonPropertyName("backgroundColor")]
        public string BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (backgroundColor != value)
                {
                    Parent?.OnPropertyChanged(nameof(BackgroundColor), value, backgroundColor, this);
                    backgroundColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the border color of the node. By default, it is black.
        /// </summary>
        [JsonPropertyName("borderColor")]
        public string BorderColor
        {
            get => borderColor;
            set
            {
                if (borderColor != value)
                {
                    Parent?.OnPropertyChanged(nameof(BorderColor), value, borderColor, this);
                    borderColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the border width of the node. By default, it is 1px.
        /// </summary>
        [JsonPropertyName("borderWidth")]
        public double BorderWidth
        {
            get => borderWidth;
            set
            {
                if (!borderWidth.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(BorderWidth), value, borderWidth, this);
                    borderWidth = value;
                }
            }
        }

        /// <summary>
        /// Returns the record of the data source as data. Each record on the data source represents a node in an automatic layout.
        /// </summary>
        /// <remarks>
        /// Nodes can be generated automatically with the information provided through the data. This is applicable only while using an automatic layout. You can get the datasource details in a node through the data. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfDiagramComponent Height="600px" NodeCreating="@OnNodeCreating" ConnectorCreating="@OnConnectorCreating">
        ///     <DataSourceSettings Id = "Id" ParentId="ParentId" DataSource="@DataSource"/>
        ///         <Layout Type = "LayoutType.MindMap" >
        ///             < LayoutMargin Top="20" Left="20"/>
        ///         </Layout>
        /// </SfDiagramComponent>
        /// 
        /// @code
        /// {
        ///     private void OnNodeCreating(IDiagramObject obj)
        ///     {
        ///         Node node = obj as Node;
        ///         node.Height = 100;
        ///         node.Width = 100;
        ///         node.BackgroundColor = "#6BA5D7";
        ///         node.Style = new ShapeStyle() { Fill = "#6495ED", StrokeWidth = 1, StrokeColor = "white" };
        ///         node.Shape = new BasicShape() { Type = Shapes.Basic };
        ///         MindMapDetails mindmapData = node.Data as MindMapDetails;
        ///         node.Annotations = new DiagramObjectCollection<ShapeAnnotation>()
        ///         {
        ///             new ShapeAnnotation()
        ///             {
        ///                 Content = mindmapData.Label
        ///             }
        ///         };
        ///     }
        ///     private void OnConnectorCreating(IDiagramObject connector)
        ///     {
        ///         (connector as Connector).Type = ConnectorSegmentType.Orthogonal;
        ///         (connector as Connector).TargetDecorator.Shape = DecoratorShape.Arrow;
        ///         (connector as Connector).Style = new ShapeStyle() { StrokeColor = "#6d6d6d" };
        ///         (connector as Connector).CornerRadius = 5;
        ///     }
        ///     //Business class
        ///     public class MindMapDetails
        ///     {
        ///         public string Id { get; set; }
        ///         public string Label { get; set; }
        ///         public string ParentId { get; set; }
        ///         public string Branch { get; set; }
        ///         public string Fill { get; set; }
        ///     }
        ///     public object DataSource = new List<object>()
        ///     {
        ///         new MindMapDetails() { Id = "1",Label = "Creativity", ParentId = "", Branch = "Root"},
        ///         new MindMapDetails() { Id = "2",  Label = "Brainstorming", ParentId = "1", Branch = "Right" },
        ///         new MindMapDetails() { Id = "3",  Label = "Complementing", ParentId = "1", Branch = "Left" },
        ///     };
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("data")]
        public object Data
        {
            get => data;
            set
            {
                if (data != value)
                {
                    Parent?.OnPropertyChanged(nameof(Data), value, data, this);
                    data = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the geometrical representation of a node. 
        /// </summary>
        /// <remarks>
        ///  The Diagram provides some built-in shapes, such as BasicShape, FlowShape, Path, etc. You can also add custom shapes to the node.
        /// </remarks>
        /// <example>
        /// <code lang="Razor">
        /// <![CDATA[
        /// <SfDiagramComponent Height="600px" Nodes="@nodes" />
        ///  @code
        ///  {
        ///     //Initialize the node collection with node
        ///     DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>();
        ///     protected override void OnInitialized()
        ///     {
        ///         Node node = new Node()
        ///         {
        ///             ID = "node1",
        ///             //Size of the node
        ///             Height = 100,
        ///             Width = 100,
        ///             //Position of the node
        ///             OffsetX = 100,
        ///             OffsetY = 100,
        ///             //Set the type of shape as flow
        ///             Shape = new FlowShape()
        ///             {
        ///                 Type = Shapes.Flow,
        ///                 Shape = FlowShapesType.DirectData
        ///             }
        ///         };
        ///         nodes.Add(node);
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [JsonConverter(typeof(ShapeJsonConverter))]
        [JsonPropertyName("shape")]
        public Shape Shape
        {
            get
            {
                if (shape != null && shape.Parent == null)
                    shape.SetParent(this, nameof(Shape));
                return shape;
            }
            set
            {
                if (shape != value)
                {
                    Parent?.OnPropertyChanged(nameof(Shape), value, shape, this);
                    shape = value;
                }
            }
        }
        [JsonIgnore]
        internal ICommonElement Template
        {
            get => template;
            set
            {
                if (!Equals(template, value))
                {
                    template = value;
                }
            }
        }

        /// <summary>
        /// Enables or disables certain features of the node. By default, all the interactive functionalities are enabled. 
        /// </summary>
        /// <remarks>
        /// Features like dragging, resizing, and rotation of the node can be disabled.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Node node = new Node()
        /// {
        ///    ID = "node1",
        ///    Height = 100,
        ///    Width = 100,
        ///    OffsetX = 100,
        ///    OffsetY = 100,
        ///    Style = new ShapeStyle() { Fill = "#6495ED", StrokeColor = "White" },
        ///    //set the NodeConstraints constraints...
        ///    Constraints = NodeConstraints.Default & ~NodeConstraints.Rotate
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("constraints")]
        public NodeConstraints Constraints
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
        /// Gets or sets the horizontal alignment of the node. 
        /// </summary>
        /// <remarks>
        /// Describes how a node should be positioned or stretched in HorizontalAlignment. 
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
        /// Gets or sets the vertical alignment of the node. 
        /// </summary>
        /// <remarks>
        /// Describes how a node should be positioned or stretched in VerticalAlignment. 
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
        /// Allows to initialize the UI of a node
        /// </summary>
        internal DiagramElement Init()
        {
            DiagramElement content = new DiagramElement();
            switch (Shape.Type)
            {
                case Shapes.Basic:
                    if (Shape is BasicShape basicShape)
                    {
                        BasicShapeType shape = basicShape.Shape;
                        if (shape == BasicShapeType.Rectangle)
                        {
                            content = new DiagramElement { CornerRadius = basicShape.CornerRadius, IsRectElement = true };
                        }
                        else if (shape == BasicShapeType.Polygon)
                        {
                            content = new PathElement { Data = basicShape.PolygonPath };
                        }
                        else
                        {
                            content = new PathElement() { Data = Dictionary.GetShapeData(shape.ToString()) };
                        }
                    }

                    break;
                case Shapes.Flow:
                    content = new PathElement() { Data = Dictionary.GetShapeData(((FlowShape)Shape).Shape.ToString()) };
                    break;
                case Shapes.Path:
                    content = new PathElement() { Data = ((PathShape)Shape).Data };
                    break;
                case Shapes.Image:
                    content = new ImageElement
                    {
                        Source = ((ImageShape)this.Shape).Source,
                        ImageAlign = ((ImageShape)this.Shape).ImageAlign,
                        ImageScale = ((ImageShape)this.Shape).Scale
                    };
                    break;
                case Shapes.HTML:
                    content = new DiagramHtmlElement();
                    break;
                case Shapes.SVG:
                    content = new DiagramSvgElement();
                    break;
                case Shapes.Bpmn:
                    BpmnDiagrams bpmnDiagrams = this.Parent is SfDiagramComponent component ? component.BpmnDiagrams : (((Palette)this.Parent).Parent as Syncfusion.Blazor.Diagram.SymbolPalette.SfSymbolPaletteComponent).BpmnDiagrams;
                    BpmnSubProcess subProcess = ((BpmnShape)this.shape).Activity.SubProcess;
                    SfDiagramComponent diagram = this.Parent is SfDiagramComponent parent ? parent : null;
                    content = bpmnDiagrams.InitBpmnContent(content, this);
                    if (subProcess.Processes != null && subProcess.Processes.Count > 0)
                    {
                        DiagramObjectCollection<string> children = subProcess.Processes;
                        foreach (string i in children)
                        {
                            if (diagram != null)
                            {
                                Node node = (diagram.NameTable[i] as Node);
                                if (diagram.NameTable[i] != null && (string.IsNullOrEmpty(node?.ProcessId) || node.ProcessId == this.ID))
                                {
                                    subProcess.processesID = this.ID;
                                    ((Node)((SfDiagramComponent)this.Parent).NameTable[i]).ProcessId = this.ID;
                                    if (this.Height < (((Node)((SfDiagramComponent)this.Parent).NameTable[i]).Margin.Top + ((Node)((SfDiagramComponent)this.Parent).NameTable[i]).Height))
                                    {
                                        if (node != null)
                                            this.Height += (node.Margin.Top + (node.Height - this.Height));
                                    }
                                    if (node != null && this.Width < (node.Margin.Left + (node.Width)))
                                    {
                                        this.Width += (node.Margin.Left + (node.Width - this.Width));
                                    }
                                    if (subProcess.Collapsed)
                                    {
                                        (this.Parent as SfDiagramComponent).DiagramContent.UpdateElementVisibility(node.Wrapper, node, !subProcess.Collapsed);
                                    }
                                    (content as DiagramContainer).Children.Add(((this.Parent as SfDiagramComponent).NameTable[i] as Node).Wrapper as ICommonElement);
                                    ICommonElement canvas = (content as DiagramContainer).Children[0];
                                    BpmnDiagrams.SetSizeForBpmnActivity(
                                        (this.Shape as BpmnShape).Activity, content as Canvas, Convert.ToInt32(BaseUtil.GetDoubleValue(canvas.Width ?? this.Width)), Convert.ToInt32(BaseUtil.GetDoubleValue(canvas.Height ?? this.Height)), this);

                                }
                            }
                        }
                    }
                    Style.StrokeColor = content.Style.StrokeColor;
                    Style.Fill = "Transparent";
                    break;
            }
            content.ID = ID + "_content";
            content.RelativeMode = RelativeMode.Object;
            if (Width.HasValue)
                content.Width = Width.Value;
            if (Height.HasValue)
                content.Height = Height.Value;
            if ((shape.Type == Shapes.SVG || shape.Type == Shapes.HTML) && (Width == null || Height == null))
            {
                content.Width = Width ?? 100;
                content.Height = Height ?? 100;
            }
            if (MinHeight.HasValue)
                content.MinHeight = MinHeight.Value;
            if (MaxHeight.HasValue)
                content.MaxHeight = MaxHeight.Value;
            if (MinWidth.HasValue)
                content.MinWidth = MinWidth.Value;
            if (MaxWidth.HasValue)
                content.MaxWidth = MaxWidth.Value;
            content.HorizontalAlignment = HorizontalAlignment.Stretch;
            content.VerticalAlignment = VerticalAlignment.Stretch;
            if (ConstraintsUtil.CanShadow(this))
            {
                content.Shadow = Shadow;
            }
            UpdateContentStyle(content, Style);
            return content;
        }

        private static void UpdateContentStyle(DiagramElement content, ShapeStyle style)
        {
            content.Style = style.Clone() as ShapeStyle;
        }

        /// <summary>
        /// Initialize the container for the node
        /// </summary>
        internal DiagramContainer InitContainer()
        {
            // Creates canvas element
            DiagramContainer canvas = ((this is NodeGroup groupContainer) && groupContainer.Children != null && groupContainer.Children.Length > 0) ? new DiagramContainer() : new Canvas();
            canvas.ID = ID;
            canvas.OffsetX = OffsetX;
            canvas.OffsetY = OffsetY;
            canvas.Visible = IsVisible;
            canvas.HorizontalAlignment = HorizontalAlignment;
            canvas.VerticalAlignment = VerticalAlignment;
            //if (Container != null)
            //{
            //    canvas.Width = Width;
            //    canvas.Height = Height;
            //    if (Container.Type === 'Stack')
            //    {
            //        canvas.Orientation = container.orientation;
            //    }
            //}
            canvas.Style.Fill = BackgroundColor;
            canvas.Style.StrokeColor = BorderColor;
            canvas.Style.StrokeWidth = BorderWidth;
            canvas.RotationAngle = RotationAngle;
            canvas.MinHeight = MinHeight;
            canvas.MinWidth = MinWidth;
            canvas.MaxHeight = MaxHeight;
            canvas.MaxWidth = MaxWidth;
            canvas.Pivot = Pivot;
            canvas.Margin = Margin;
            canvas.Flip = Flip;
            Wrapper = canvas;
            return canvas;
        }

        internal void InitPorts(DiagramContainer container)
        {
            if (Ports != null)
            {
                foreach (PointPort port in Ports)
                {
                    InitPort(container, port);
                }
            }
        }
        internal void InitPort(DiagramContainer container, PointPort port)
        {
            DiagramElement portWrapper = InitPortWrapper(port);
            // wrapperContent;
            //let contentAccessibility: Function = getFunction(accessibilityContent);
            //if (contentAccessibility)
            //{
            //    wrapperContent = contentAccessibility(portWrapper, this);
            //}
            portWrapper.Description = portWrapper.ID; //wrapperContent ? wrapperContent : portWrapper.id;
            portWrapper.InversedAlignment = false;
            portWrapper.ElementActions |= ElementAction.ElementIsPort;

            container.Children.Add(portWrapper);
        }
        internal DiagramElement InitPortWrapper(Port ports)
        {
            // Creates port element
            PathElement portContent = new PathElement
            {
                Height = ports.Height,
                Width = ports.Width
            };
            string pathData = (ports.Shape == PortShapes.Custom) ? ports.PathData : Dictionary.GetShapeData(ports.Shape.ToString());
            portContent.ID = ID + '_' + (ports.ID);
            portContent.Margin = ports.Margin;
            portContent.Data = pathData;
            portContent.Style = ports.Style.Clone() as ShapeStyle;
            if (portContent.Style != null) portContent.Style.Gradient = null;
            portContent.HorizontalAlignment = ports.HorizontalAlignment;
            portContent.VerticalAlignment = ports.VerticalAlignment;
            portContent = DiagramUtil.UpdatePortEdges(portContent, Flip, ports as PointPort) as PathElement;
            if (Width != 0 || Height != 0)
            {
                if (portContent != null) portContent.Float = true;
            }
            portContent.RelativeMode = RelativeMode.Point;
            portContent.Visible = ConstraintsUtil.CheckPortRestriction(ports, PortVisibility.Visible) > 0
                                      && !(ConstraintsUtil.CheckPortRestriction(ports, PortVisibility.Hover) > 0)
                                      && !(ConstraintsUtil.CheckPortRestriction(ports, PortVisibility.Connect) > 0);
            portContent.ElementActions |= ElementAction.ElementIsPort;
            return portContent;
        }
        internal void InitAnnotations(DiagramContainer container, bool? virtualize = null)
        {
            if (annotations != null)
            {
                for (int i = 0; i < annotations.Count; i++)
                {
                    ICommonElement annotation = InitAnnotationWrapper(annotations[i], virtualize);
                    annotation.Description = annotation.ID;
                    annotation.InversedAlignment = false;
                    container.Children.Add(annotation);
                }
            }
        }
        internal DiagramElement InitFixedUserHandles(NodeFixedUserHandle fixedUserHandle)
        {
            Canvas fixedUserHandleContainer = new Canvas
            {
                Float = true
            };
            ObservableCollection<ICommonElement> children = new ObservableCollection<ICommonElement>();
            fixedUserHandle.ID = string.IsNullOrEmpty(fixedUserHandle.ID) ? BaseUtil.RandomId() : fixedUserHandle.ID;
            fixedUserHandleContainer.ID = this.ID + '_' + fixedUserHandle.ID;
            fixedUserHandleContainer.Children = children;
            fixedUserHandleContainer.Height = fixedUserHandle.Height;
            fixedUserHandleContainer.Width = fixedUserHandle.Width;
            fixedUserHandleContainer.Style.StrokeColor = fixedUserHandle.Stroke;
            fixedUserHandleContainer.Style.Fill = fixedUserHandle.Fill;
            fixedUserHandleContainer.Style.StrokeWidth = fixedUserHandle.StrokeThickness;
            fixedUserHandleContainer.Margin = fixedUserHandle.Margin as Margin;
            fixedUserHandleContainer.Visible = fixedUserHandle.Visibility;
            fixedUserHandleContainer.CornerRadius = fixedUserHandle.CornerRadius;
            fixedUserHandleContainer.HorizontalAlignment = HorizontalAlignment.Center;
            fixedUserHandleContainer.VerticalAlignment = VerticalAlignment.Center;
            var offset = GetFixedUserHandleOffet(fixedUserHandle as NodeFixedUserHandle);
            fixedUserHandleContainer.SetOffsetWithRespectToBounds(offset.X, offset.Y, UnitMode.Fraction);
            fixedUserHandleContainer.RelativeMode = RelativeMode.Point;
            DiagramElement symbolIcon = InitFixedUserHandlesSymbol(fixedUserHandle, fixedUserHandleContainer);
            fixedUserHandleContainer.Children.Add(symbolIcon);
            fixedUserHandleContainer.Description = fixedUserHandleContainer.ID;
            fixedUserHandleContainer.InversedAlignment = false;
            return fixedUserHandleContainer;
        }
        private static DiagramElement InitFixedUserHandlesSymbol(NodeFixedUserHandle options, Canvas fixedUserHandleContainer)
        {
            PathElement fixedUserHandleContent = new PathElement
            {
                Data = options.PathData,
                Height = options.Height > 10 ? options.Height - (options.Padding.Bottom + options.Padding.Top) : options.Height,
                Width = options.Width > 10 ? options.Width - (options.Padding.Left + options.Padding.Right) : options.Width,
                Visible = fixedUserHandleContainer.Visible,
                ID = fixedUserHandleContainer.ID + "_shape",
                InversedAlignment = false,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Style = new ShapeStyle()
                {
                    Fill = options.IconStroke,
                    StrokeColor = options.IconStroke,
                    StrokeWidth = options.IconStrokeThickness
                }
            };
            fixedUserHandleContent.SetOffsetWithRespectToBounds(0.5, 0.5, UnitMode.Fraction);
            fixedUserHandleContent.RelativeMode = RelativeMode.Object;
            fixedUserHandleContent.Description = string.IsNullOrEmpty(fixedUserHandleContainer.Description) ? "" : fixedUserHandleContainer.Description;
            return fixedUserHandleContent;
        }
        private static DiagramPoint GetFixedUserHandleOffet(NodeFixedUserHandle fixedUserHandle)
        {
            NodeFixedUserHandle userHandle = fixedUserHandle as NodeFixedUserHandle;
            return new DiagramPoint() { X = userHandle.Offset.X, Y = userHandle.Offset.Y };
        }

        internal ICommonElement InitAnnotationWrapper(Annotation annotation, bool? virtualize)
        {
            TextElement annotationContent = new TextElement()
            {
                CanMeasure = (bool)!virtualize,
                Style = annotation.Style.Clone() as TextStyle
            };
            if (annotationContent.Style != null)
            {
                annotationContent.Style.Gradient = null;
                if (annotation.Hyperlink != null)
                {
                    HyperlinkSettings link = annotation.Hyperlink ?? null;
                    ((TextStyle)annotationContent.Style).TextDecoration = link?.TextDecoration ?? annotationContent.Hyperlink.TextDecoration;
                    ((TextStyle)annotationContent.Style).Color =
                        link != null ? link.Color : annotationContent.Hyperlink.Color;
                    annotationContent.Hyperlink.Url = annotation.Hyperlink?.Url;
                    annotationContent.Hyperlink.Content = annotation.Hyperlink?.Content;
                    annotationContent.Hyperlink.TextDecoration = annotation.Hyperlink.TextDecoration;
                    annotationContent.Content = !string.IsNullOrEmpty(link.Content) ? link.Content :
                        annotationContent.Hyperlink != null ? annotationContent.Hyperlink.Url : annotation.Content;
                }
            }

            annotationContent.Constraints = annotation.Constraints;
            annotationContent.Height = annotation.Height;
            annotationContent.Width = annotation.Width;
            annotationContent.Visible = annotation.Visibility;
            annotationContent.RotationAngle = annotation.RotationAngle;
            annotationContent.ID = ID + '_' + annotation.ID;
            if (width != null)
            {
                if (annotation.Width == null || (annotation.Width > width &&
                    (annotation.Style.TextWrapping == TextWrap.Wrap || annotation.Style.TextWrapping == TextWrap.WrapWithOverflow)))
                {
                    annotationContent.Width = width;
                }
            }
            annotationContent.Margin = annotation.Margin;
            annotationContent.HorizontalAlignment = annotation.HorizontalAlignment;
            annotationContent.VerticalAlignment = annotation.VerticalAlignment;
            if (annotation is ShapeAnnotation label)
                annotationContent.SetOffsetWithRespectToBounds(label.Offset.X, label.Offset.Y, UnitMode.Fraction);
            if (width != null || height != null)
            {
                annotationContent.Float = true;
            }
            annotationContent.RelativeMode = RelativeMode.Point;
            if (annotationContent.Hyperlink.Url == null)
            {
                annotationContent.Content = annotation.Content;
            }
            return annotationContent;
        }
        /// <summary>
        /// Creates a new object that is a copy of the current node. 
        /// </summary>
        /// <returns>Node</returns>
        public override object Clone()
        {
            return new Node(this);
        }

        internal override void Dispose()
        {
            if (TreeBounds != null)
            {
                TreeBounds = null;
            }

            if (shadow != null)
            {
                shadow.Dispose();
                shadow = null;
            }

            if (style != null)
            {
                style.Dispose();
                style = null;
            }

            if (pivot != null)
            {
                pivot.Dispose();
                pivot = null;

                if (ports != null && ports.Count > 0)
                {
                    for (int i = 0; i < ports.Count; i++)
                    {
                        ports[i].Dispose();
                        ports[i] = null;
                    }
                    ports.Clear();
                    ports = null;
                }

                if (annotations != null && annotations.Count > 0)
                {
                    for (int i = 0; i < annotations.Count; i++)
                    {
                        annotations[i].Dispose();
                        annotations[i] = null;
                    }
                    annotations.Clear();
                    annotations = null;
                }

                if (fixedUserHandles != null && fixedUserHandles.Count > 0)
                {
                    for (int i = 0; i < fixedUserHandles.Count; i++)
                    {
                        fixedUserHandles[i].Dispose();
                        fixedUserHandles[i] = null;
                    }
                    fixedUserHandles.Clear();
                    fixedUserHandles = null;
                }

                if (template != null)
                {
                    template.Dispose();
                    template = null;
                }

                if (NativeSize != null)
                {
                    NativeSize.Dispose();
                    NativeSize = null;
                }

                if (OutEdges != null && OutEdges.Count > 0)
                {
                    OutEdges.Clear();
                    OutEdges = null;
                }

                if (InEdges != null && InEdges.Count > 0)
                {
                    InEdges.Clear();
                    InEdges = null;
                }

                base.Dispose();
            }
        }
    }
    /// <summary>
    /// Represents a cluster of multiple nodes and connectors into a single element. It acts like a container for its children (nodes, groups, and connectors).
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Height = "500px" @ref="diagram" Nodes="@nodes">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     SfDiagramComponent diagram;
    ///     DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>();
    ///     protected override void OnInitialized()
    ///     {
    ///         Node node1 = createNode("node1", 100, 100);
    ///         Node node2 = createNode("node2", 300, 100);
    ///         NodeGroup groupnode = new NodeGroup();
    ///         // Grouping node 1 and node 2 into a single group
    ///         groupnode.Children = new string[] { "node1", "node2" };
    ///         nodes.Add(node1);
    ///         nodes.Add(node2);
    ///         nodes.Add(groupnode);
    ///     }
    ///     public Node createNode(string id, double offx, double offy)
    ///     {
    ///         Node node = new Node()
    ///         {
    ///             ID = id,
    ///             OffsetX = offx,
    ///             OffsetY = offy,
    ///             Height = 100,
    ///             Width = 100,
    ///             Style = new ShapeStyle() { Fill = "#6495ED" }
    ///         };
    ///         return node;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class NodeGroup : Node
    {
        private string[] children;
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeGroup"/>.
        /// </summary>
        public NodeGroup() : base()
        {
            Style.StrokeColor = "transparent";
            Style.Fill = "transparent";
            Style.StrokeWidth = 0;
        }
        /// <summary>
        /// Creates a new instance of the <see cref="NodeGroup"/> from the given <see cref="NodeGroup"/>.
        /// </summary>
        /// <param name="src"></param>
        public NodeGroup(NodeGroup src) : base(src)
        {
            if (src != null)
            {
                children = (string[])(src.children.Clone());
            }
        }

        /// <summary>
        /// Gets or sets the children of the group element
        /// </summary>
        [JsonPropertyName("children")]
        public string[] Children
        {
            get => children;
            set
            {
                if (children != value)
                {
                    Parent?.OnPropertyChanged(nameof(Children), value, children, this);
                    children = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current group.
        /// </summary>
        /// <returns>Group</returns>
        public override object Clone()
        {
            return new NodeGroup(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (Children != null && Children.Length > 0)
            {
                Array.Clear(children, 0, children.Length);
            }
        }
    }
}

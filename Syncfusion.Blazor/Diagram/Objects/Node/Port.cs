using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents a port or connection point of the node. 
    /// </summary>
    /// <remarks>
    /// Ports act as the connection points of node and allow them to create connections with only those specific points. There may be any number of ports in a node. You can able to modify the ports appearance, visibility, positioning, and can add custom shapes to it.
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
    ///     // Initialize port collection
    ///     Ports = new DiagramObjectCollection<PointPort>()
    ///     {
    ///         // Sets the position for the port
    ///         new PointPort() 
    ///         { 
    ///             Style = new ShapeStyle() { Fill = "gray" }, 
    ///             Offset = new DiagramPoint() { X = 0.5, Y = 0.5 }, 
    ///             Visibility = PortVisibility.Visible
    ///         }
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="PointPort"/>
    public class Port : DiagramObject
    {
        private string id;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Center;
        private Margin margin = new Margin();
        private double width = 12;
        private double height = 12;
        private ShapeStyle style = new ShapeStyle();
        private PortShapes shape = PortShapes.Square;
        private PortVisibility visibility = PortVisibility.Connect;
        private string pathData;
        private PortConstraints constraints = PortConstraints.Default;
        private Dictionary<string, object> additionalInfo = new Dictionary<string, object>();
        private List<string> outEdges = new List<string>();
        private List<string> inEdges = new List<string>();
        /// <summary>
        /// Creates a new instance of the Port from the given Port.
        /// </summary>
        /// <param name="src">Port</param>
        public Port(Port src) : base(src)
        {
            if (src != null)
            {
                id = string.IsNullOrEmpty(src.ID) ? BaseUtil.RandomId() : src.ID;
                horizontalAlignment = src.horizontalAlignment;
                verticalAlignment = src.verticalAlignment;
                margin = src.margin;
                width = src.width;
                height = src.height;
                if (src.style != null)
                {
                    style = src.style.Clone() as ShapeStyle;
                }
                shape = src.shape;
                visibility = src.visibility;
                pathData = src.pathData;
                constraints = src.constraints;
                additionalInfo = src.additionalInfo;
                outEdges = new List<string>();
                inEdges = new List<string>();
            }
        }
        /// <summary>
        /// Initializes a new instance of the Port.
        /// </summary>
        public Port() : base()
        {
            id = BaseUtil.RandomId();
        }

        /// <summary>
        /// Represents the unique id of the diagram object. 
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">1. The ID needs to be unique. While creating a port, the user should not use the same id to other ports.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">2. The ID needs to be unique. While creating a port, the user should not use the same id to other ports.</td>
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
        /// Sets the horizontal alignment of the port with respect to its immediate parent(node/connector)
        /// </summary>
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
        /// Gets or sets the vertical alignment of the port to its immediate parent(node/connector). 
        /// </summary>
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
        /// Defines the space from the actual offset values of the port. The default values for the Margin are 0 on all sides. 
        /// </summary>
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
                    Parent?.OnPropertyChanged(nameof(Margin), value, margin, this);
                    margin = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the width of the port. By default, it is 12px. 
        /// </summary>
        /// <remarks>
        /// The width of a port does not include borders or margins.
        /// </remarks>
        [JsonPropertyName("width")]
        public double Width
        {
            get => width;
            set
            {
                if (!width.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Width), value, width, this);
                    width = value;
                }
            }
        }
        /// <summary>
        ///  Sets the height of the port
        /// </summary>
        [JsonPropertyName("height")]
        public double Height
        {
            get => height;
            set
            {
                if (!height.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Height), value, height, this);
                    height = value;
                }
            }
        }
        /// <summary>
        /// Represents the appearance of the port. 
        /// </summary>
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
        /// Represents the shape (built-in shape) of the port. By default, it appears in Square shape.
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">The below list of shape types is used to define the port shape.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">1.   X - Sets the shape to X.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">2.   Circle - Sets the shape to Circle. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">3.   Square - Sets the shape to Square. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">4.   Custom - Sets the shape to Custom..</td>
        /// </tr>
        /// </table>
        /// </remarks>
        [JsonPropertyName("shape")]
        public PortShapes Shape
        {
            get => shape;
            set
            {
                if (shape != value)
                {
                    Parent?.OnPropertyChanged(nameof(Shape), value, shape, this);
                    shape = value;
                }
            }
        }
        /// <summary>
        /// Represents the visibility of the port. By default, the port becomes visible when the mouse hovers over the node. 
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">  The below list of options is used to control the visibility of the ports. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">1.   Visible - Default value. The port is visible</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">2.   Hidden - The port is hidden.  </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">3.   Hover - Shows the port when the mouse hovers a node. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">4.   Connect - Shows the port when a connection endpoint is dragged over a node.  </td>
        /// </tr>
        /// </table>
        /// </remarks>
        [JsonPropertyName("visibility")]
        public PortVisibility Visibility
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
        /// Represents the custom geometry(shape) of the port. 
        /// </summary>
        /// <remarks>
        /// To create a custom-shaped port, the user must set the shape to ‘custom’ and then the PathData. (A custom graphics path is a set of connected lines, curves, and other simple graphics objects, including rectangles, ellipses, and text. A path works as a single graphics object, so any effect applied to the graphic path will also be applied to the port..) 
        /// </remarks>
        [JsonPropertyName("pathData")]
        public string PathData
        {
            get => pathData;
            set
            {
                if (pathData != value)
                {
                    Parent?.OnPropertyChanged(nameof(PathData), value, pathData, this);
                    pathData = value;
                }
            }
        }

        /// <summary>
        /// Defines the constraints of port
        /// </summary>
        [JsonPropertyName("constraints")]
        public PortConstraints Constraints
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
        /// Gets or sets the custom properties of a port.
        /// </summary>
        /// <remarks>
        /// Enables the user to store data of any data type. It will be serialized and deserialized automatically while saving and opening the diagram. 
        /// </remarks>
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
        /// Defines the collection of connectors that are connected to the port. 
        /// </summary>
        [JsonPropertyName("outEdges")]
        public List<string> OutEdges
        {
            get => outEdges;
            set
            {
                if (outEdges != value)
                {
                    Parent?.OnPropertyChanged(nameof(OutEdges), value, outEdges, this);
                    outEdges = value;
                }
            }
        }
        /// <summary>
        /// Defines the collection of connectors that are connected to the port.
        /// </summary>
        [JsonPropertyName("inEdges")]
        public List<string> InEdges
        {
            get => inEdges;
            set
            {
                if (inEdges != value)
                {
                    Parent?.OnPropertyChanged(nameof(InEdges), value, inEdges, this);
                    inEdges = value;
                }
            }
        }
        /// <summary>
        /// Creates a new port that is a copy of the current port.
        /// </summary>
        /// <returns>Port</returns>
        public override object Clone()
        {
            return new Port(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (margin != null)
            {
                margin.Dispose();
                margin = null;
            }

            if (style != null)
            {
                style.Dispose();
                style = null;
            }

            if (outEdges != null && outEdges.Count > 0)
            {
                outEdges.Clear();
                outEdges = null;
            }
            if (inEdges != null && inEdges.Count > 0)
            {
                inEdges.Clear();
                inEdges = null;
            }
            id = null;
            pathData = null;
            if (additionalInfo != null)
            {
                additionalInfo = null;
            }
        }
    }

    /// <summary>
    /// Defines the behavior of a port (connection point) that sticks to a point. 
    /// </summary>
    /// <remarks>
    /// Ports act as the connection points of node and allow them to create connections with only those specific points. There may be any number of ports in a node. You can able to modify the Ports appearance, visibility, positioning, and can add custom shapes to it.
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
    ///     // Initialize port collection
    ///     Ports = new DiagramObjectCollection<PointPort>()
    ///     {
    ///         // Sets the position for the port
    ///         new PointPort() 
    ///         { 
    ///         Style = new ShapeStyle() { Fill = "gray" }, 
    ///         Offset = new DiagramPoint() { X = 0.5, Y = 0.5 }, 
    ///         Visibility = PortVisibility.Visible
    ///         }
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    public class PointPort : Port
    {
        private DiagramPoint point = new DiagramPoint() { X = 0.5, Y = 0.5 };
        /// <summary>
        /// Creates a new instance of the PointPort from the given PointPort.
        /// </summary>
        /// <param name="src">PointPort</param>
        public PointPort(PointPort src) : base(src)
        {
            if (src?.point != null)
            {
                point = src.point.Clone() as DiagramPoint;
            }
        }
        /// <summary>
        /// Initializes a new instance of the PointPort.
        /// </summary>
        public PointPort() : base()
        {
        }
        /// <summary>
        /// Defines the position of the port with respect to the boundaries of node.
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
        ///     // Initialize port collection
        ///     Ports = new DiagramObjectCollection<PointPort>()
        ///     {
        ///         // Sets the position for the port
        ///         new PointPort() 
        ///         { 
        ///             Style = new ShapeStyle() { Fill = "gray" }, 
        ///             Offset = new DiagramPoint() { X = 0.5, Y = 0.5 }, 
        ///             Visibility = PortVisibility.Visible
        ///         }
        ///     }
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("offset")]
        public DiagramPoint Offset
        {
            get
            {
                if (point != null && point.Parent == null)
                    point.SetParent(this, nameof(Offset));
                return point;
            }
            set
            {
                if (point != value)
                {
                    Parent?.OnPropertyChanged(nameof(Offset), value, point, this);
                    point = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current port. 
        /// </summary>
        /// <returns>PointPort</returns>
        public override object Clone()
        {
            return new PointPort(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (point != null)
            {
                point = null;
            }
        }
    }
}

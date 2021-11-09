using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{

    /// <summary>
    /// Represents the behavior of fixeduserhandle. 
    /// </summary>
    public class FixedUserHandle : DiagramObject
    {
        private string id;
        private string pathData = string.Empty;
        private string fill = "transparent";
        private string stroke = "transparent";
        private string iconStroke = "black";
        private double iconStrokeThickness;
        private double strokeThickness = 1;
        private double cornerRadius;
        private bool visibility = true;
        private double width = 10;
        private double height = 10;
        private Margin padding = new Margin();
        /// <summary>
        /// Creates a new instance of the FixedUserHandle from the given FixedUserHandle. 
        /// </summary>
        /// <param name="src">FixedUserHandle.</param>
        public FixedUserHandle(FixedUserHandle src) : base(src)
        {
            if (src != null)
            {
                id = string.IsNullOrEmpty(src.ID) ? BaseUtil.RandomId() : src.ID;
                pathData = src.pathData;
                fill = src.fill;
                stroke = src.stroke;
                iconStroke = src.iconStroke;
                iconStrokeThickness = src.iconStrokeThickness;
                strokeThickness = src.strokeThickness;
                cornerRadius = src.cornerRadius;
                visibility = src.visibility;
                width = src.width;
                height = src.height;
                padding = (Margin)src.padding.Clone();
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedUserHandle"/>.
        /// </summary>
        public FixedUserHandle() : base()
        {
            id = BaseUtil.RandomId();
        }

        /// <summary>
        /// Gets or sets the unique id of the diagram object.
        /// </summary>
        [JsonPropertyName("id")]
        public string ID
        {
            get => id;
            set
            {
                if (!string.Equals(id, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(ID), value, id, this);
                    id = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the shape information of the fixed user handle.
        /// </summary>
        [JsonPropertyName("pathData")]
        public string PathData
        {
            get => this.pathData;
            set
            {
                if (!string.Equals(this.pathData, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(PathData), value, pathData, this as IDiagramObject);
                    this.pathData = value;
                }
            }

        }
        /// <summary>
        /// Gets or sets the fill color of the fixed user handle. By default, it is transparent.
        /// </summary>
        [JsonPropertyName("fill")]
        public string Fill
        {
            get => fill;
            set
            {
                if (!string.Equals(fill, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(Fill), value, fill, this as IDiagramObject);
                    fill = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the stroke color of the fixed user handle container. By default, it is transparent.
        /// </summary>
        [JsonPropertyName("stroke")]
        public string Stroke
        {
            get => stroke;
            set
            {
                if (!string.Equals(stroke, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(Stroke), value, stroke, this);
                    stroke = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the stroke width of the fixed user handle.
        /// </summary>
        [JsonPropertyName("iconStrokeThickness")]
        public double IconStrokeThickness
        {
            get => iconStrokeThickness;
            set
            {
                if (!iconStrokeThickness.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(IconStrokeThickness), value, iconStrokeThickness, this);
                    iconStrokeThickness = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the height of the fixed user handle. By default, it is 10px.
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
        /// Gets or sets the width of the fixed user handle. By default, it is 10.
        /// </summary>
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
        /// Gets or sets the corner radius of the fixed user handle container. By default, it is 0. 
        /// </summary>
        [JsonPropertyName("cornerRadius")]
        public double CornerRadius
        {
            get => cornerRadius;
            set
            {
                if (!cornerRadius.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(CornerRadius), value, cornerRadius, this as IDiagramObject);
                    cornerRadius = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the stroke color of the fixed user handle. By default, it is black.
        /// </summary>
        [JsonPropertyName("iconStroke")]
        public string IconStroke
        {
            get => iconStroke;
            set
            {
                if (!string.Equals(iconStroke, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(IconStroke), value, iconStroke, this);
                    iconStroke = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the stroke width of the fixed user handle container. By default, it is 1px.
        /// </summary>
        [JsonPropertyName("strokeThickness")]
        public double StrokeThickness
        {
            get => strokeThickness;
            set
            {
                if (!strokeThickness.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(StrokeThickness), value, strokeThickness, this);
                    strokeThickness = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the space between the fixed user handle and the container. By default, it is 0.
        /// </summary>
        [JsonPropertyName("padding")]
        public Margin Padding
        {
            get
            {
                if (padding != null && padding.Parent == null)
                    padding.SetParent(this, nameof(Padding));
                return padding;
            }
            set
            {
                if (!Equals(padding, value))
                {
                    Parent?.OnPropertyChanged(nameof(Padding), value, padding, this);
                    padding = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the visibility of the fixed user handle. By default, it is True.
        /// </summary>
        [JsonPropertyName("visibility")]
        public bool Visibility
        {
            get => visibility;
            set
            {
                if (!visibility.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Visibility), value, visibility, this);
                    visibility = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current FixedUserHandle.
        /// </summary>
        /// <returns>FixedUserHandle</returns>
        public override object Clone()
        {
            return new FixedUserHandle(this);
        }

        internal override void Dispose()
        {
            id = null;
            pathData = null;
            fill = null;
            stroke = null;
            iconStroke = null;
            if (padding != null)
            {
                padding.Dispose();
                padding = null;
            }
        }
    }

    /// <summary>
    /// Represents the node’s fixed user handle. 
    /// </summary>
    public class NodeFixedUserHandle : FixedUserHandle
    {
        private DiagramPoint offset = new DiagramPoint() { X = 0, Y = 0 };
        private Margin margin = new Margin();

        /// <summary>
        /// Creates a new instance of the <see cref="NodeFixedUserHandle"/> from the given NodeFixedUserHandle.
        /// </summary>
        /// <param name="src">NodeFixedUserHandle.</param>
        public NodeFixedUserHandle(NodeFixedUserHandle src) : base(src)
        {
            if (src != null)
            {
                offset = (DiagramPoint)src.offset.Clone();
                margin = (Margin)src.margin.Clone();
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeFixedUserHandle"/>.
        /// </summary>
        public NodeFixedUserHandle() : base()
        {
        }

        /// <summary>
        /// Gets or sets the position of the node fixed user handle.
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
                if (!Equals(offset, value))
                {
                    Parent?.OnPropertyChanged(nameof(Offset), value, offset, this);
                    offset = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the extra space around the outer boundaries of the fixed user handle. By default, it is 0 from all the sides.
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
                if (!Equals(margin, value))
                {
                    Parent?.OnPropertyChanged(nameof(Margin), value, margin, this);
                    margin = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current NodeFixedUserHandle.
        /// </summary>
        /// <returns>NodeFixedUserHandle</returns>
        public override object Clone()
        {
            return new NodeFixedUserHandle(this);
        }

        internal override void Dispose()
        {
            if (offset != null)
            {
                offset.Dispose();
                offset = null;
            }
            if (margin != null)
            {
                margin.Dispose();
                margin = null;
            }
            base.Dispose();
        }
    }
    /// <summary>
    /// Represents the connector fixed user handle.
    /// </summary>
    /// <remarks>
    /// The fixed user handles are used to add some frequently used commands around the node and connector even without selecting it.
    /// </remarks>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// Connector connector = new Connector()
    /// {
    ///     SourcePoint = new DiagramPoint() { X = 100, Y = 100 },
    ///     TargetPoint = new DiagramPoint() { X = 200, Y = 200 },
    ///     Type = ConnectorSegmentType.Orthogonal,
    ///     Style = new TextStyle() { StrokeColor = "#6495ED" },
    ///     // A fixed user handle is created and stored in the fixed user handle collection of the Connector.
    ///     FixedUserHandles = new DiagramObjectCollection<ConnectorFixedUserHandle>()
    ///     {
    ///         new ConnectorFixedUserHandle()
    ///         {
    ///             ID = "user1",
    ///             Height = 25,
    ///             Width = 25,
    ///             Offset = 0.5,
    ///             Alignment = FixedUserHandleAlignment.After,
    ///             Displacement = new DiagramPoint { Y = 10 },
    ///             Visibility = true,Padding = new Margin() { Bottom = 1, Left = 1, Right = 1, Top = 1 },
    ///             PathData = "M60.3,18H27.5c-3,0-5.5,2.4-5.5,5.5v38.2h5.5V23.5h32.7V18z M68.5,28.9h-30c-3,0-5.5,2.4-5.5,5.5v38.2c0,3,2.4,5.5,5.5,5.5h30c3,0,5.5-2.4,5.5-5.5V34.4C73.9,31.4,71.5,28.9,68.5,28.9z M68.5,72.5h-30V34.4h30V72.5z"
    ///         }
    ///     },
    /// };
    /// ]]>
    /// </code>
    /// </example>

    public class ConnectorFixedUserHandle : FixedUserHandle
    {
        private double offset;
        private DiagramPoint displacement = new DiagramPoint() { X = 0, Y = 0 };
        private FixedUserHandleAlignment alignment = FixedUserHandleAlignment.Center;

        /// <summary>
        /// Creates a new instance of the <see cref="ConnectorFixedUserHandle"/> from the given <see cref="ConnectorFixedUserHandle"/>.
        /// </summary>
        /// <param name="src">ConnectorFixedUserHandle.</param>
        /// 
        public ConnectorFixedUserHandle(ConnectorFixedUserHandle src) : base(src)
        {
            if (src != null)
            {
                offset = src.offset;
                displacement = (DiagramPoint)src.displacement.Clone();
                alignment = src.alignment;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorFixedUserHandle"/>.
        /// </summary>
        public ConnectorFixedUserHandle() : base()
        {
        }

        /// <summary>
        /// Gets or sets the position of the connector fixed user handle. By default, it is 0.
        /// </summary>
        [JsonPropertyName("offset")]

        public double Offset
        {
            get => offset;
            set
            {
                if (!offset.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Offset), value, offset, this);
                    offset = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the alignment of the fixed user handle. By default, it is aligned at the center.
        /// </summary>
        /// <remarks>
        /// The below list explains the alignment options.
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border:none">1. Center, aligns the fixedUserHandle on the connector segment.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border:none">2. Before, aligns the fixedUserHandle on top of a connector segment.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border:none">3. After, aligns the fixedUserHandle at the bottom of a connector segment.</td>
        /// </tr>
        /// </table>
        /// </remarks>
        [JsonPropertyName("alignment")]
        public FixedUserHandleAlignment Alignment
        {
            get => alignment;
            set
            {
                if (!Equals(alignment, value))
                {
                    Parent?.OnPropertyChanged(nameof(Alignment), value, alignment, this);
                    alignment = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the displacement(margin) of the fixed user handle. Applicable only if the parent is a connector.
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
                if (!Equals(displacement, value))
                {
                    Parent?.OnPropertyChanged(nameof(Displacement), value, displacement, this);
                    displacement = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current <see cref="ConnectorFixedUserHandle"/>.
        /// </summary>
        ///  <returns>ConnectorFixedUserHandle</returns>        
        public override object Clone()
        {
            return new ConnectorFixedUserHandle(this);
        }

        internal override void Dispose()
        {
            if (displacement != null)
            {
                displacement.Dispose();
                displacement = null;
            }
            base.Dispose();
        }
    }
}
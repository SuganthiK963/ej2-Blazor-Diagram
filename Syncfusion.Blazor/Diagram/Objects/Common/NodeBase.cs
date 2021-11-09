using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the common behavior of nodes, connectors, and groups. 
    /// </summary>
    public class NodeBase : DiagramObject
    {
        private string id;
        private int zIndex = -1;
        private Margin margin = new Margin();
        private bool isVisible = true;
        private FlipDirection flip = FlipDirection.None;

        private Dictionary<string, object> additionalInfo = new Dictionary<string, object>();
        private DiagramContainer wrapper;
        [JsonIgnore]
        internal bool canShapeLayout { get; set; } = true;
        internal string ParentId { get; set; } = string.Empty;
        /// <summary>
        /// Creates a new instance of the <see cref="NodeBase"/> from the given NodeBase.
        /// </summary>
        /// <param name="src">NodeBase</param>
        public NodeBase(NodeBase src) : base(src)
        {
            if (src != null)
            {
                id = string.IsNullOrEmpty(src.ID) ? BaseUtil.RandomId() : src.ID;
                zIndex = src.zIndex;
                if (src.margin != null)
                {
                    margin = src.margin.Clone() as Margin;
                }
                isVisible = src.isVisible;
                canShapeLayout = src.canShapeLayout;
                flip = src.flip;
                ParentId = src.ParentId;
                additionalInfo = new Dictionary<string, object>(src.additionalInfo);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeBase"/>.
        /// </summary>
        public NodeBase() : base()
        {
            id = BaseUtil.RandomId();
        }

        /// <summary>
        /// Gets or sets the unique id of the diagram object.
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">1. ID needs to be unique to use. While creating a node, the user should not provide the same id to other nodes.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">2. When drag and drop a new node from symbolpalette, ID will be generated automatically.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">3. If multiple nodes having same ID, then unexpected behaviour might happen. </td>
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
        /// Gets or sets the UI of a node/connector.
        /// </summary>
        [JsonIgnore]
        internal DiagramContainer Wrapper
        {
            get => wrapper;
            set
            {
                if (!Equals(wrapper, value))
                {
                    wrapper = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the visual order of the node/connector.
        /// </summary>
        /// <remarks>
        /// The property specifies the stack order of the node. A node with greater stack order is always in front of a node with a lower stack order.
        /// </remarks>
        [JsonPropertyName("zIndex")]
        internal int ZIndex
        {
            get => zIndex;
            set
            {
                if (zIndex != value)
                {
                    Parent?.OnPropertyChanged(nameof(ZIndex), value, zIndex, this);
                    zIndex = value;
                }
            }
        }

        /// <summary>
        /// The margin adds extra space around an element's outside boundaries. The default values for the margin are 0 from all the sides.
        /// </summary>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">Left , represents the value of the left margin.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">Top , represents the top margin of the node. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">Right , represents the right margin of the node.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">Bottom , represents the bottom margin of the node. </td>
        /// </tr>
        /// </table>
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
        /// Gets or sets the visibility of the node/connector, by default it is true(visible).
        /// </summary>
        [JsonPropertyName("isVisible")]
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible != value)
                {
                    Parent?.OnPropertyChanged(nameof(IsVisible), value, isVisible, this);
                    isVisible = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the value indicates whether the node should be automatically positioned or not. Applicable, if layout option is enabled.
        /// </summary>
        [JsonPropertyName("canAutoLayout")]
        public bool CanAutoLayout
        {
            get => canShapeLayout;
            set
            {
                if (canShapeLayout != value)
                {
                    Parent?.OnPropertyChanged(nameof(CanAutoLayout), value, canShapeLayout, this);
                    canShapeLayout = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the custom properties of a node/connector.
        /// </summary>
        /// <remarks>
        /// Enables the user to store data of any datatype. It will be serialized and deserialized automatically while saving and opening the diagram.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Dictionary<string, object> NodeInfo = new Dictionary<string, object>();
        /// NodeInfo.Add("nodeInfo", "Central Node");
        /// // A node is created and stored in nodes collection.
        /// Node node = new Node()
        /// {
        ///     // Position of the node
        ///     OffsetX = 250,
        ///     OffsetY = 250,
        ///     // Size of the node
        ///     Width = 100,
        ///     Height = 100,
        ///     Style = new ShapeStyle() { Fill = "#6BA5D7", StrokeColor = "white" },
        ///     AdditionalInfo = NodeInfo
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
        /// Gets or sets a value to flip the element in Horizontal/Vertical directions.
        /// </summary>
        [JsonPropertyName("flip")]
        internal FlipDirection Flip
        {
            get => flip;
            set
            {
                if (flip != value)
                {
                    Parent?.OnPropertyChanged(nameof(Flip), value, flip, this);
                    flip = value;
                }
            }
        }
        /// <summary>
        /// Creates a new diagram element that is the copy of the current diagram element.
        /// </summary>
        /// <returns>A new diagram element that is the copy of this diagram element</returns>
        public override object Clone()
        {
            return new NodeBase(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            id = null;
            ParentId = null;
            if (margin != null)
            {
                margin.Dispose();
                margin = null;
            }
            if (Wrapper != null)
            {
                wrapper.Dispose();
            }
        }
    }
    /// <summary>
    /// Represents the class that acts as the base class for the diagram object. 
    /// </summary>
    public class DiagramObject : IDiagramObject
    {
        [JsonIgnore]
        internal IDiagramObject Parent { get; set; }
        [JsonIgnore]
        internal string PropertyName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramObject"/>.
        /// </summary>
        public DiagramObject()
        {
        }
        /// <summary>
        /// Creates a new instance of the <see cref="DiagramObject"/> from the given <see cref="DiagramObject"/>.
        /// </summary>
        /// <param name="src">DiagramObject</param>
        public DiagramObject(DiagramObject src)
        {
            if (src != null)
            {
                Parent = src.Parent;
                PropertyName = src.PropertyName;
            }
        }
        /// <summary>
        /// Invoked when the effective value of any property on this diagram objects has been updated.
        /// </summary>
        public void OnPropertyChanged(string propertyName, object newVal, object oldVal, IDiagramObject container)
        {
            Parent?.OnPropertyChanged(propertyName, newVal, oldVal, container);
        }
        /// <summary>
        /// Gets the parent of an object when the method is invoked.
        /// </summary>
        /// <returns>returns a parent of an object.</returns>
        public IDiagramObject GetParent()
        {
            return Parent;
        }
        internal void SetParent(IDiagramObject parentVal, string propertyName)
        {
            Parent = parentVal;
            PropertyName = propertyName;
        }
        /// <summary>
        /// Creates a new diagram object that is a copy of the current diagram object.
        /// </summary>
        /// <returns>DiagramObject</returns>
        public virtual object Clone()
        {
            return new DiagramObject(this);
        }

        internal virtual void Dispose()
        {
            Parent = null;
            PropertyName = null;
        }
    }

    internal class PropertyChangeValues
    {
        internal object OldValue { get; set; }
        internal object NewValue { get; set; }
    }
}

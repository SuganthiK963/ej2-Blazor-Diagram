using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{

    /// <summary>
    /// Enables/Disables certain features of the diagram. 
    /// </summary>
    [Flags]
    public enum DiagramConstraints
    {
        /// <summary>
        /// Disables all the functionalities of the diagram.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies whether a bridge can be created for all the connectors.
        /// </summary>
        Bridging = 1 << 1,
        /// <summary>
        /// Enables or disables the undo or redo functionality over the diagram.
        /// </summary>
        UndoRedo = 1 << 2,
        /// <summary>
        /// Decides whether interaction should happen on the diagram or not.
        /// </summary>
        UserInteraction = 1 << 3,
        /// <summary>
        /// Decides whether the public API needs to be enabled or not.
        /// </summary>
        ApiUpdate = 1 << 4,
        /// <summary>
        /// Decides whether the diagram can be editable or not.
        /// </summary>
        PageEditable = 1 << 3 | 1 << 4,
        /// <summary>
        /// Specifies whether zooming-related action can be enabled or not.
        /// </summary>
        Zoom = 1 << 5,
        /// <summary>
        /// Enables or disables panning actions only on the x-axis (horizontal panning). 
        /// </summary>
        PanX = 1 << 6,
        /// <summary>
        /// Enables or disables panning actions only on the y-axis (vertical panning). 
        /// </summary>
        PanY = 1 << 7,
        /// <summary>
        /// Specifies the panning action of the diagram on both axis.
        /// </summary>
        Pan = 1 << 6 | 1 << 7,
        /// <summary>
        /// Specifies  whether the zooming ratio can be maintained or not while editing the label.
        /// </summary>
        ZoomTextEdit = 1 << 8,
        /// <summary>
        /// Enables all the functionalities of the diagram.
        /// </summary>
        Default = 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5 | 1 << 6 | 1 << 7
    }

    /// <summary>
    /// Specifies how the diagram elements have to be flipped.
    /// </summary>
    internal enum FlipDirection
    {
        /// <summary>
        /// Flip the diagram shape horizontally.
        /// </summary>
        [EnumMember(Value = "Horizontal")]
        Horizontal,
        /// <summary>
        /// Flip the diagram shape vertically.
        /// </summary>
        [EnumMember(Value = "Vertical")]
        Vertical,
        /// <summary>
        /// Flip the diagram shape to both horizontally and vertically. 
        /// </summary>
        [EnumMember(Value = "Both")]
        Both,
        /// <summary>
        /// No flip will be applied and this is the default value. 
        /// </summary>
        [EnumMember(Value = "None")]
        None,
    }

    /// <summary>
    /// Represents the alignment of the diagram elements based on its immediate parent. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Height="600px" Nodes="@nodes">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     public DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>() { };
    ///     protected override void OnInitialized()
    ///     {
    ///         // A node is created and stored in nodes array.
    ///         Node Node = new Node()
    ///         {
    ///             /// Initialize the port collection
    ///             Ports = new DiagramObjectCollection<PointPort>()
    ///             {
    ///                 new PointPort()
    ///                 {
    ///                     HorizontalAlignment = HorizontalAlignment.Left
    ///                 }
    ///             },
    ///         };
    ///         nodes.Add(Node);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// Stretch the diagram element horizontally to its immediate parent.
        /// </summary>
        [EnumMember(Value = "Stretch")]
        Stretch,
        /// <summary>
        /// Align the diagram element horizontally to the left side of its immediate parent.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,
        /// <summary>
        /// Align the diagram element horizontally to the right side of its immediate parent.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
        /// <summary>
        /// Align the diagram element horizontally to the center of its immediate parent.
        /// </summary>
        [EnumMember(Value = "Center")]
        Center,
        /// <summary>
        /// Aligns the diagram element based on its immediate parent’s horizontal alignment property.
        /// </summary>
        [EnumMember(Value = "Auto")]
        Auto,
    }

    /// <summary>
    /// Represents the alignment of the fixeduserhandle with respect to its immediate parent. 
    /// </summary>
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
    public enum FixedUserHandleAlignment
    {
        /// <summary>
        /// Aligns the fixedUserHandle on the connector segment. 
        /// </summary>
        [EnumMember(Value = "Center")]
        Center,
        /// <summary>
        /// Aligns the fixedUserHandle on top of a connector segment.
        /// </summary>
        [EnumMember(Value = "Before")]
        Before,
        /// <summary>
        /// Aligns the fixedUserHandle at the bottom of a connector segment.
        /// </summary>
        [EnumMember(Value = "After")]
        After
    }
    /// <summary>
    /// Specifies the type of gridlines.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="500px">
    ///     <SnapSettings GridType = "GridType.Dots" >
    ///         <HorizontalGridLines LineColor="Blue" @bind-LineIntervals="@HInterval"
    ///                           @bind-DotIntervals="@HDotInterval"></HorizontalGridLines>
    ///         <VerticalGridLines LineColor = "Blue" @bind-LineIntervals="@VInterval"
    ///                          @bind-DotIntervals="@VDotInterval"></VerticalGridLines>
    ///     </SnapSettings>
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///    public double[] HDotInterval { get; set; } = new double[] { 3, 20, 1, 20, 1, 20 };
    ///    public double[] VDotInterval { get; set; } = new double[] { 3, 20, 1, 20, 1, 20, 1, 20, 1, 20 };
    ///    public double[] HInterval { get; set; } = new double[] { 1.25, 18.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75 };
    ///    public double[] VInterval { get; set; } = new double[] { 1.25, 18.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75, 0.25, 19.75 };
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum GridType
    {
        /// <summary>
        /// Renders grid patterns as lines.
        /// </summary>
        [EnumMember(Value = "Lines")]
        Lines,
        /// <summary>
        /// Renders grid patterns as dots.
        /// </summary>
        [EnumMember(Value = "Dots")]
        Dots,
    }

    /// <summary>
    /// Represents the alignment of the diagram elements based on its immediate parent. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Height="600px" Nodes="@nodes">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     public DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>() { };
    ///     protected override void OnInitialized()
    ///     {
    ///         // A node is created and stored in nodes array.
    ///         Node Node = new Node()
    ///         {
    ///             /// Initialize the port collection
    ///             Ports = new DiagramObjectCollection<PointPort>()
    ///             {
    ///                 new PointPort()
    ///                 {
    ///                     VerticalAlignment = VerticalAlignment.Top
    ///                 }
    ///             },
    ///         };
    ///         nodes.Add(Node);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum VerticalAlignment
    {
        /// <summary>
        /// Stretch the diagram element vertically to its immediate parent.
        /// </summary>
        [EnumMember(Value = "Stretch")]
        Stretch,
        /// <summary>
        /// Align the diagram element vertically to the top side of its immediate parent.
        /// </summary>
        [EnumMember(Value = "Top")]
        Top,
        /// <summary>
        /// Align the diagram element vertically to the bottom side of its immediate parent.
        /// </summary>
        [EnumMember(Value = "Bottom")]
        Bottom,
        /// <summary>
        /// Align the diagram element vertically to the center of its immediate parent.
        /// </summary>
        [EnumMember(Value = "Center")]
        Center,
        /// <summary>
        /// Aligns the diagram element based on its immediate parent’s vertical alignment property.
        /// </summary>
        [EnumMember(Value = "Auto")]
        Auto,
    }

    /// <summary>
    /// Represents the decoration of a text in the text block. 
    /// </summary>
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum TextDecoration
    {
        /// <summary>
        /// Draws a horizontal line above the text.
        /// </summary>
        [EnumMember(Value = "Overline")]
        Overline,
        /// <summary>
        /// Draws a horizontal line under the text.
        /// </summary>
        [EnumMember(Value = "Underline")]
        Underline,
        /// <summary>
        /// Draws a horizontal line through the text of a node or a connector.
        /// </summary>
        [EnumMember(Value = "LineThrough")]
        LineThrough,
        /// <summary>
        /// Represents the default appearance of a text.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
    }

    /// <summary>
    /// Represents the alignment of the text inside the text block. 
    /// </summary>
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum TextAlign
    {
        /// <summary>
        /// Sets the alignment of the text to the left in the text block.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,
        /// <summary>
        /// Sets the alignment of the text to the right in the text block. 
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
        /// <summary>
        /// Sets the alignment of the text at the center of the text block .
        /// </summary>
        [EnumMember(Value = "Center")]
        Center,
        /// <summary>
        /// Sets the alignment of the text in respective to left and right margins.
        /// </summary>
        [EnumMember(Value = "Justify")]
        Justify,
    }

    /// <summary>
    /// Specifies a value that indicates whether to render ellipses (...) to indicate text overflow. 
    /// </summary>
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum TextOverflow
    {
        /// <summary>
        /// Wraps the text to the next line, when it exceeds its bounds.
        /// </summary>
        [EnumMember(Value = "Wrap")]
        Wrap,
        /// <summary>
        /// Ellipsis hides the text if the text size exceeds the boundary.
        /// </summary>
        [EnumMember(Value = "Ellipsis")]
        Ellipsis,
        /// <summary>
        /// The text is restricted to the node/connector boundary and the text will not be overflown.
        /// </summary>
        [EnumMember(Value = "Clip")]
        Clip,
    }

    /// <summary>
    /// Specifies how to control the overflow of text in the node boundaries. The wrapping property defines how to wrap the text.
    /// </summary>
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum TextWrap
    {
        /// <summary>
        /// Text-wrapping occurs when the text overflows beyond the available node width. However, the text may overflow beyond the node width in  case of a very long word.
        /// </summary>
        [EnumMember(Value = "WrapWithOverflow")]
        WrapWithOverflow,
        /// <summary>
        /// The text will be wrapped within the boundary.
        /// </summary>
        [EnumMember(Value = "Wrap")]
        Wrap,
        /// <summary>
        /// The text will not be wrapped. If a lengthy text exists, the boundary will not be a limit.
        /// </summary>
        [EnumMember(Value = "NoWrap")]
        NoWrap,
    }

    /// <summary>
    /// Specifies how the white space and the new line characters should be set.  
    /// </summary>
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    internal enum WhiteSpace
    {
        /// <summary>
        /// Preserves (Includes) all empty spaces and empty lines.
        /// </summary>
        [EnumMember(Value = "PreserveAll")]
        PreserveAll,
        /// <summary>
        /// Collapses (Excludes) all consequent empty spaces and empty lines.
        /// </summary>
        [EnumMember(Value = "CollapseSpace")]
        CollapseSpace,
        /// <summary>
        /// Collapses (Excludes) the consequent spaces into one.
        /// </summary>
        [EnumMember(Value = "CollapseAll")]
        CollapseAll,
    }

    /// <summary>
    /// Specifies the type of transition between two or more colors. 
    /// </summary>
    internal enum GradientType
    {
        /// <summary>
        /// Sets the type of gradient to linear.
        /// </summary>
        [EnumMember(Value = "Linear")]
        Linear,
        /// <summary>
        /// Sets the type of gradient to radial.
        /// </summary>
        [EnumMember(Value = "Radial")]
        Radial,
    }
    /// <summary>
    /// Specifies the relative mode.
    /// </summary>
    public enum RelativeMode
    {
        /// <summary>
        /// DiagramPoint - Diagram elements will be aligned with respect to a point
        /// </summary>
        Point,
        /// <summary>
        /// Object - Diagram elements will be aligned with respect to its immediate parent
        /// </summary>
        Object
    }
    /// <summary>
    /// Specifies the type of Transform.
    /// </summary>
    [Flags]
    internal enum Transform
    {
        /// <summary>
        /// Self - Sets the transform type as Self
        /// </summary>
        Self,
        /// <summary>
        /// Parent - Sets the transform type as Parent
        /// </summary>
        Parent
    }
    /// <summary>
    /// Specifies the element action.
    /// </summary>
    [Flags]
    internal enum ElementAction
    {
        /// <summary>
        /// Disables all element actions are none.
        /// </summary>
        None = 0,
        /// <summary>
        /// Enable the element action is Port.
        /// </summary>
        ElementIsPort = 1 << 1,
        /// <summary>
        /// Enable the element action as Group.
        /// </summary>
        ElementIsGroup = 1 << 2,
    }
    /// <summary>
    /// Specifies the type of unit mode.
    /// </summary>
    public enum UnitMode
    {
        /// <summary>
        /// Absolute - Sets the unit mode type as Absolute
        /// </summary>
        Absolute,
        /// <summary>
        /// Fraction - Sets the unit mode type as Fraction
        /// </summary>
        Fraction
    }


    /// <summary>
    /// Specifies the shape of the ports.
    /// </summary>
    /// <remarks>
    /// To know more about using port shapes, refer to the Ports.
    /// To apply the X port shape, use the below code
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Height="600px" Nodes="@nodes">
    /// </SfDiagramComponent>
    /// @code{
    ///   DiagramObjectCollection<Node> nodes;
    ///   protected override void OnInitialized()
    ///   {
    ///     //Initialize the NodeCollection.
    ///     nodes = new DiagramObjectCollection<Node>();
    ///     Node Node = new Node()
    ///     {
    ///       ID = "node1",
    ///       Height = 100,
    ///       Width = 100,
    ///       OffsetX = 100,
    ///       OffsetY = 100,
    ///     };
    ///     Node.Ports = new DiagramObjectCollection<PointPort>()
    ///     {
    ///      new PointPort()
    ///      {
    ///          ID="port1",
    ///          Offset=new DiagramPoint(){X=0,Y=0.5},
    ///          Shape=PortShapes.X,
    ///          Visibility=PortVisibility.Hover|PortVisibility.Connect,
    ///          //set the PortConstraints...
    ///          Constraints=PortConstraints.Draw
    ///       }
    ///     };
    ///     nodes.Add(Node);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum PortShapes
    {
        /// <summary>
        /// Sets the shape of the port to X.
        /// </summary>
        [EnumMember(Value = "X")]
        X,
        /// <summary>
        /// Sets the shape of the port to Circle.
        /// </summary>
        [EnumMember(Value = "Circle")]
        Circle,
        /// <summary>
        /// Sets the shape of the port to Square.
        /// </summary>
        [EnumMember(Value = "Square")]
        Square,
        /// <summary>
        /// Sets the shape of the port to Custom.
        /// </summary>
        [EnumMember(Value = "Custom")]
        Custom,
    }

    /// <summary>
    /// Represents the visibility options of the port.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Height="600px" Nodes="@nodes">
    /// </SfDiagramComponent>
    /// @code 
    /// {
    ///   DiagramObjectCollection<Node> nodes;
    ///   protected override void OnInitialized()
    ///   {
    ///    //Initialize the NodeCollection.
    ///    nodes = new DiagramObjectCollection<Node>();
    ///    Node Node = new Node()
    ///    {
    ///      ID = "node1",
    ///      Height = 100,
    ///      Width = 100,
    ///      OffsetX = 100,
    ///      OffsetY = 100,
    ///    };
    ///    Node.Ports = new DiagramObjectCollection<PointPort>()
    ///    {
    ///      new PointPort()
    ///      {
    ///        ID="port1",
    ///        Offset=new DiagramPoint(){X=0,Y=0.5},
    ///        Shape=PortShapes.X,
    ///        Visibility=PortVisibility.Hover|PortVisibility.Connect,
    ///        //set the PortConstraints...
    ///        Constraints=PortConstraints.Draw
    ///       }
    ///     };
    ///     nodes.Add(Node);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [Flags]
    public enum PortVisibility
    {
        /// <summary>
        /// Shows the port when a connector endpoint is dragged over a node.
        /// </summary>
        Connect = 1 << 3,
        /// <summary>
        /// Always hides the port.
        /// </summary>
        Hidden = 1 << 1,
        /// <summary>
        /// Shows the port when the mouse hovers over a node. 
        /// </summary>
        Hover = 1 << 2,
        /// <summary>
        /// Always shows the port.
        /// </summary>
        Visible = 1 << 0
    }
    /// <summary>
    /// Defines how the specified selected items are aligned when calling the align command.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// private void OnAlignLeft()
    /// {
    ///     diagram.SetAlign(AlignmentOptions.Left,null, AlignmentMode.Object);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum AlignmentMode
    {
        /// <summary>
        /// Aligns the selected objects based on the first object in the selected list. 
        /// </summary>
        Object,
        /// <summary>
        /// Aligns the selected objects based on the selection boundary. 
        /// </summary>
        Selector
    }
    /// <summary>
    /// Defines how the selected objects should be positioned.
    /// </summary>
    public enum AlignmentOptions
    {
        /// <summary>
        /// Aligns all the selected objects to the left of the selection boundary.
        /// </summary>
        Left,
        /// <summary>
        ///	Aligns all the selected objects to the right of the selection boundary.
        /// </summary>
        Right,
        /// <summary>
        /// Aligns all the selected objects at the top of the selection boundary.
        /// </summary>
        Top,
        /// <summary>
        ///	Aligns all the selected objects at the bottom of the selection boundary.
        /// </summary>
        Bottom,
        /// <summary>
        /// Aligns all the selected objects at the center of the selection boundary.
        /// </summary>
        Center,
        /// <summary>
        /// Aligns all the selected objects at the middle of the selection boundary.
        /// </summary>
        Middle,
    }
    /// <summary>
    /// Distribute the options that enable you to place the selected objects on the page at equal intervals from each other. 
    /// </summary>
    /// <remarks>
    /// The selected objects are equally spaced within the selection boundary.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// private void Distribute()
    /// {
    ///     diagram.SetDistribute(DistributeOptions.Left);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum DistributeOptions
    {
        /// <summary>
        /// Distributes the objects based on the distance between the right and left sides of the adjacent objects.
        /// </summary>
        RightToLeft,
        /// <summary>
        /// Distributes the objects based on the distance between the centers of the adjacent objects.
        /// </summary>
        Center,
        /// <summary>
        /// Distributes the objects based on the distance between the left sides of the adjacent objects.
        /// </summary>
        Left,
        /// <summary>
        /// Distributes the objects based on the distance between the right sides of the adjacent objects.
        /// </summary>
        Right,
        /// <summary>
        /// Distributes the objects based on the distance between the bottom sides of the adjacent objects.
        /// </summary>
        Bottom,
        /// <summary>
        /// Distributes the objects based on the distance between the top sides of the adjacent objects.
        /// </summary>
        Top,
        /// <summary>
        /// Distributes the objects based on the distance between the bottom and top sides of the adjacent objects.
        /// </summary>
        BottomToTop,
        /// <summary>
        /// Distributes the objects based on the distance between the vertical centers of the adjacent objects.
        /// </summary>
        Middle,
    }
    /// <summary>
    /// Specifies how to equally size the selected nodes with respect to the first selected object.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// private void OnSameSize()
    /// {
    ///    diagram.SetSameSize(SizingMode.Size);
    /// }
    /// private void OnSameWidth()
    /// {
    ///    diagram.SetSameSize(SizingMode.Width);
    /// }
    /// private void OnSameHeight()
    /// {
    ///    diagram.SetSameSize(SizingMode.Height);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum SizingMode
    {
        /// <summary>
        /// Scales the selected objects both vertically and horizontally.
        /// </summary>
        Size,
        /// <summary>
        /// Scales the height of the selected objects.
        /// </summary>
        Height,
        /// <summary>
        /// Scales the width of the selected objects.
        /// </summary>
        Width,
    }
    /// <summary>
    /// Enables or disables certain features of the port. 
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
    ///             Visibility=PortVisibility.Hover|PortVisibility.Connect,
    ///             Constraints = PortConstraints.Draw
    ///         }
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    [Flags]
    public enum PortConstraints
    {
        /// <summary>
        /// Disables all the port functionalities.
        /// </summary>
        None = 0,
        /// <summary>
        /// Enables or disables creating the connection when the mouse hovers on the port.
        /// </summary>
        Draw = 1 << 1,
        /// <summary>
        /// Enables or disables connecting only the target end of the connector.
        /// </summary>
        InConnect = 1 << 2,
        /// <summary>
        /// Enables or disables connecting only the source end of the connector.
        /// </summary>
        OutConnect = 1 << 3,
        /// <summary>
        /// Enables all constraints of a port.
        /// </summary>
        Default = 1 << 2 | 1 << 3,
    }
    /// <summary>
    /// Specifies the type of node.
    /// </summary>
    public enum Shapes
    {
        /// <summary>
        /// Allows defining a shape from the available built-in basic shapes.
        /// </summary>
        [EnumMember(Value = "Basic")]
        Basic,
        /// <summary>
        /// Allows defining a custom node from path data.
        /// </summary>
        [EnumMember(Value = "Path")]
        Path,
        /// <summary>
        /// Allows creating an image node.
        /// </summary>
        [EnumMember(Value = "Image")]
        Image,
        /// <summary>
        /// Allows defining a shape from the available built-in flow shapes.
        /// </summary>
        [EnumMember(Value = "Flow")]
        Flow,
        /// <summary>
        /// Allows defining a shape from the available built-in BPMN shapes.
        /// </summary>
        [EnumMember(Value = "Bpmn")]
        Bpmn,
        /// <summary>
        /// Allows creating a native SVG node.
        /// </summary>
        [EnumMember(Value = "SVG")]
        SVG,
        /// <summary>
        /// Allows setting a custom template for a node.
        /// </summary>
        [EnumMember(Value = "HTML")]
        HTML,
    }

    /// <summary>
    /// Represents the available built-in basic shapes.
    /// </summary>
    public enum BasicShapeType
    {
        /// <summary>
        /// Sets the type of basic shape as a Rectangle.
        /// </summary>
        [EnumMember(Value = "Rectangle")]
        Rectangle,
        /// <summary>
        /// Sets the type of basic shape as an Ellipse.
        /// </summary>
        [EnumMember(Value = "Ellipse")]
        Ellipse,
        /// <summary>
        /// Sets the type of basic shape as a Hexagon.
        /// </summary>
        [EnumMember(Value = "Hexagon")]
        Hexagon,
        /// <summary>
        /// Sets the type of basic shape as a Parallelogram.
        /// </summary>
        [EnumMember(Value = "Parallelogram")]
        Parallelogram,
        /// <summary>
        /// Sets the type of basic shape as a Triangle.
        /// </summary>
        [EnumMember(Value = "Triangle")]
        Triangle,
        /// <summary>
        /// Sets the type of basic shape as a Plus.
        /// </summary>
        [EnumMember(Value = "Plus")]
        Plus,
        /// <summary>
        /// Sets the type of basic shape as a Star.
        /// </summary>
        [EnumMember(Value = "Star")]
        Star,
        /// <summary>
        /// Sets the type of basic shape as a Pentagon.
        /// </summary>
        [EnumMember(Value = "Pentagon")]
        Pentagon,
        /// <summary>
        /// Sets the type of basic shape as a Heptagon.
        /// </summary>
        [EnumMember(Value = "Heptagon")]
        Heptagon,
        /// <summary>
        /// Sets the type of basic shape as a Octagon.
        /// </summary>
        [EnumMember(Value = "Octagon")]
        Octagon,
        /// <summary>
        /// Sets the type of basic shape as a Trapezoid.
        /// </summary>
        [EnumMember(Value = "Trapezoid")]
        Trapezoid,
        /// <summary>
        /// Sets the type of basic shape as a Decagon.
        /// </summary>
        [EnumMember(Value = "Decagon")]
        Decagon,
        /// <summary>
        /// Sets the type of basic shape as a Right Triangle.
        /// </summary>
        [EnumMember(Value = "RightTriangle")]
        RightTriangle,
        /// <summary>
        /// Sets the type of basic shape as a Cylinder.
        /// </summary>
        [EnumMember(Value = "Cylinder")]
        Cylinder,
        /// <summary>
        /// Sets the type of basic shape as a Diamond.
        /// </summary>
        [EnumMember(Value = "Diamond")]
        Diamond,
        /// <summary>
        /// Sets the type of basic shape as a Polygon.
        /// </summary>
        [EnumMember(Value = "Polygon")]
        Polygon,
    }

    /// <summary>
    /// Specifies the type of process flow shape.
    /// </summary>
    /// <remarks>
    /// To learn more about using flow shapes, refer <see href="https://blazor.syncfusion.com/documentation/diagram-component/shapes">Shapes</see>.
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
    public enum FlowShapeType
    {
        /// <summary>
        /// Sets the flow shape type to Terminator.
        /// </summary>
        [EnumMember(Value = "Terminator")]
        Terminator,
        /// <summary>
        /// Sets the flow shape type to Process.
        /// </summary>
        [EnumMember(Value = "Process")]
        Process,
        /// <summary>
        /// Sets the flow shape type to Decision.
        /// </summary>
        [EnumMember(Value = "Decision")]
        Decision,
        /// <summary>
        /// Sets the flow shape type to Document.
        /// </summary>
        [EnumMember(Value = "Document")]
        Document,
        /// <summary>
        /// Sets the flow shape type to PreDefinedProcess.
        /// </summary>
        [EnumMember(Value = "PreDefinedProcess")]
        PreDefinedProcess,
        /// <summary>
        /// Sets the flow shape type to PaperTap.
        /// </summary>
        [EnumMember(Value = "PaperTap")]
        PaperTap,
        /// <summary>
        /// Sets the flow shape type to DirectData.
        /// </summary>
        [EnumMember(Value = "DirectData")]
        DirectData,
        /// <summary>
        /// Sets the flow shape type to SequentialData.
        /// </summary>
        [EnumMember(Value = "SequentialData")]
        SequentialData,
        /// <summary>
        /// Sets the flow shape type to Sort.
        /// </summary>
        [EnumMember(Value = "Sort")]
        Sort,
        /// <summary>
        /// Sets the flow shape type to MultiDocument.
        /// </summary>
        [EnumMember(Value = "MultiDocument")]
        MultiDocument,
        /// <summary>
        /// Sets the flow shape type to Collate.
        /// </summary>
        [EnumMember(Value = "Collate")]
        Collate,
        /// <summary>
        /// Sets the flow shape type to SummingJunction.
        /// </summary>
        [EnumMember(Value = "SummingJunction")]
        SummingJunction,
        /// <summary>
        /// Sets the flow shape type to Or.
        /// </summary>
        [EnumMember(Value = "Or")]
        Or,
        /// <summary>
        /// Sets the flow shape type to internal storage.
        /// </summary>
        [EnumMember(Value = "InternalStorage")]
        InternalStorage,
        /// <summary>
        /// Sets the flow shape type to Extract.
        /// </summary>
        [EnumMember(Value = "Extract")]
        Extract,
        /// <summary>
        /// Sets the flow shape type to ManualOperation.
        /// </summary>
        [EnumMember(Value = "ManualOperation")]
        ManualOperation,
        /// <summary>
        /// Sets the flow shape type to Merge.
        /// </summary>
        [EnumMember(Value = "Merge")]
        Merge,
        /// <summary>
        /// Sets the flow shape type to OffPageReference.
        /// </summary>
        [EnumMember(Value = "OffPageReference")]
        OffPageReference,
        /// <summary>
        /// Sets the flow shape type to SequentialAccessStorage.
        /// </summary>
        [EnumMember(Value = "SequentialAccessStorage")]
        SequentialAccessStorage,
        /// <summary>
        /// Sets the flow shape type to Annotation.
        /// </summary>
        [EnumMember(Value = "Annotation")]
        Annotation,
        /// <summary>
        /// Sets the flow shape type to Data.
        /// </summary>
        [EnumMember(Value = "Data")]
        Data,
        /// <summary>
        /// Sets the flow shape type to Card.
        /// </summary>
        [EnumMember(Value = "Card")]
        Card,
        /// <summary>
        /// Sets the flow shape type to Delay.
        /// </summary>
        [EnumMember(Value = "Delay")]
        Delay,
        /// <summary>
        /// Sets the flow shape type to Preparation.
        /// </summary>
        [EnumMember(Value = "Preparation")]
        Preparation,
        /// <summary>
        /// Sets the flow shape type to Display.
        /// </summary>
        [EnumMember(Value = "Display")]
        Display,
        /// <summary>
        /// Sets the flow shape type to ManualInput.
        /// </summary>
        [EnumMember(Value = "ManualInput")]
        ManualInput,
        /// <summary>
        /// Sets the flow shape type to LoopLimit.
        /// </summary>
        [EnumMember(Value = "LoopLimit")]
        LoopLimit,
        /// <summary>
        /// Sets the flow shape type to stored data.
        /// </summary>
        [EnumMember(Value = "StoredData")]
        StoredData,
    }

    /// <summary>
    /// Specifies the node constraints allow the users to enable or disable certain behaviors and features of the diagram nodes.
    /// </summary>
    [Flags]
    public enum NodeConstraints
    {
        /// <summary>
        /// Disable all node Constraints.
        /// </summary>
        None = 0,
        /// <summary>
        /// Enables or disables the selection of a node in the diagram.
        /// </summary>
        Select = 1 << 1,
        /// <summary>
        /// Enables or disables the dragging functionality of a node.
        /// </summary>
        Drag = 1 << 2,
        /// <summary>
        /// Enables or disables node rotation. It is done with the help of a curvy arrow.
        /// </summary>
        Rotate = 1 << 3,
        /// <summary>
        /// Enables or disables to display the nodes shadow.
        /// </summary>
        Shadow = 1 << 4,
        /// <summary>
        /// Enables or disables the mouse pointers events when clicking with a mouse.
        /// </summary>
        PointerEvents = 1 << 5,
        /// <summary>
        /// Enables or disables node deletion.
        /// </summary>
        Delete = 1 << 6,
        /// <summary>
        /// Enables node to allow only in coming connections.
        /// </summary>
        InConnect = 1 << 7,
        /// <summary>
        /// Enables node to allow only out coming connections.
        /// </summary>
        OutConnect = 1 << 8,
        /// <summary>
        /// AllowDrop allows dropping a node.
        /// </summary>
        AllowDrop = 1 << 9,
        /// <summary>
        /// It enables or disables the resizing of the node in the NorthEast direction.
        /// </summary>
        ResizeNorthEast = 1 << 10,
        /// <summary>
        /// It enables or disables the resizing of the node in the East direction.
        /// </summary>
        ResizeEast = 1 << 11,
        /// <summary>
        /// It enables or disables the resizing of the node in the SouthEast direction.
        /// </summary>
        ResizeSouthEast = 1 << 12,
        /// <summary>
        /// It enables or disables the resizing of the node in the South direction.
        /// </summary>
        ResizeSouth = 1 << 13,
        /// <summary>
        /// It enables or disables the resizing of the node in the SouthWest direction.
        /// </summary>
        ResizeSouthWest = 1 << 14,
        /// <summary>
        /// It enables or disables the resizing of the node in the West direction.
        /// </summary>
        ResizeWest = 1 << 15,
        /// <summary>
        /// It enables or disables the resizing of the node in the NorthWest direction.
        /// </summary>
        ResizeNorthWest = 1 << 16,
        /// <summary>
        /// It enables or disables the resizing of the node in the North direction.
        /// </summary>
        ResizeNorth = 1 << 17,
        /// <summary>
        /// Enables the Aspect ratio of the node.
        /// </summary>
        AspectRatio = 1 << 18,
        /// <summary>
        /// Enables the ReadOnly mode(Write operations cannot be done) for the annotation in the node.
        /// </summary>
        ReadOnly = 1 << 19,
        /// <summary>
        /// Enables to hide all resize thumbs for the node.
        /// </summary>
        HideThumbs = 1 << 20,
        /// <summary>
        /// Enables or Disables the expansion or compression of a node.
        /// </summary>
        Resize = 1 << 10 | 1 << 11 | 1 << 12 | 1 << 13 | 1 << 14 | 1 << 15 | 1 << 16 | 1 << 17,
        /// <summary>
        /// Enables all the constraints for a node
        /// </summary>
        Default = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 6 | 1 << 7 | 1 << 8 | 1 << 5 | 1 << 10 | 1 << 11 |
    1 << 12 | 1 << 13 | 1 << 14 | 1 << 15 | 1 << 16 | 1 << 17,
        /// <summary>
        /// It allows the node to inherit the interaction option from the parent object.
        /// </summary>
        Inherit = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 6,
    }

    /// <summary>
    /// Enables or disables certain behaviors and features of the connectors. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// Connector connector = new Connector()
    /// {
    ///    ID = "connector1",
    ///    Type = ConnectorSegmentType.Straight,
    ///    SourcePoint = new DiagramPoint() { X = 100, Y = 100 },
    ///    TargetPoint = new DiagramPoint() { X = 200, Y = 200 },
    ///    //set the ConnectorConstraints...
    ///    Constraints = ConnectorConstraints.Default & ~ConnectorConstraints.Select
    /// };
    /// ]]>
    /// </code>
    /// </example>
    [Flags]
    public enum ConnectorConstraints
    {
        /// <summary>
        /// Disables all the connector Constraints.
        /// </summary>
        None = 0,
        /// <summary>
        /// Enables or disables the selection of a connector.
        /// </summary>
        Select = 1 << 1,
        /// <summary>
        /// Enables or disables the deletion of a connector.
        /// </summary>
        Delete = 1 << 2,
        /// <summary>
        /// Enables or disables the connector from being dragged.
        /// </summary>
        Drag = 1 << 3,
        /// <summary>
        /// Enables or disables the connector's source end from being dragged.
        /// </summary>
        DragSourceEnd = 1 << 4,
        /// <summary>
        /// Enables or disables the connector’s target end from being dragged.
        /// </summary>
        DragTargetEnd = 1 << 5,
        /// <summary>
        /// Enables or disables the control point and endpoint of every segment in a connector for editing.
        /// </summary>
        DragSegmentThumb = 1 << 6,
        /// <summary>
        /// Enables or disables the interaction of the connector.
        /// </summary>
        Interaction = 1 << 1 | 1 << 3 | 1 << 4 | 1 << 5 | 1 << 6,
        /// <summary>
        /// Enables to trigger a drop event when any object is dragged or dropped onto the connector.
        /// </summary>
        AllowDrop = 1 << 7,
        /// <summary>
        /// Enables or disables bridging to the connector.
        /// </summary>
        Bridging = 1 << 8,
        /// <summary>
        /// Enables or disables inheriting the value of bridging from the diagram.
        /// </summary>
        InheritBridging = 1 << 9,
        /// <summary>
        /// Enables to set the pointer-events.
        /// </summary>
        PointerEvents = 1 << 10,
        /// <summary>
        /// Enables or disables read-only for the connector.
        /// </summary>
        ReadOnly = 1 << 11,
        /// <summary>
        /// Enables or disables connecting to the nearest node.
        /// </summary>
        ConnectToNearByNode = 1 << 12,
        /// <summary>
        /// Enables or disables connecting to the nearest port.
        /// </summary>
        ConnectToNearByPort = 1 << 13,
        /// <summary>
        /// Enables or disables connecting to the nearest elements.
        /// </summary>
        ConnectToNearByElement = 1 << 12 | 1 << 13,
        /// <summary>
        /// Enables all constraints for the connector.
        /// </summary>
        Default = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5 | 1 << 9 | 1 << 10 | 1 << 12 | 1 << 13,
    }


    /// <summary>
    /// Specifies the segment type of the connector.
    /// </summary>
    public enum ConnectorSegmentType
    {
        /// <summary>
        /// Sets the segment type as Straight.
        /// </summary>
        [EnumMember(Value = "Straight")]
        Straight,
        /// <summary>
        /// Sets the segment type as Orthogonal.
        /// </summary>
        [EnumMember(Value = "Orthogonal")]
        Orthogonal,
        /// <summary>
        /// Sets the segment type as Polyline.
        /// </summary>
        [EnumMember(Value = "Polyline")]
        Polyline,
        /// <summary>
        /// Sets the segment type as Bezier.
        /// </summary>
        [EnumMember(Value = "Bezier")]
        Bezier,
    }

    /// <summary>
    /// Specifies the decorator shape of the connector. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="500px" Connectors="@connectors">
    ///     <SnapSettings Constraints = "@snapConstraints" ></SnapSettings>
    /// </SfDiagramComponent >
    /// @code
    /// {
    ///     SnapConstraints snapConstraints = SnapConstraints.None;
    ///     //Define the diagram's connector collection
    ///     DiagramObjectCollection<Connector> connectors = new DiagramObjectCollection<Connector>();
    ///     protected override void OnInitialized()
    ///     {
    ///         Connector Connector = new Connector()
    ///         {
    ///             ID = "connector1",
    ///             // Set the source and target point of the connector
    ///             SourcePoint = new DiagramPoint() { X = 100, Y = 100 },
    ///             TargetPoint = new DiagramPoint() { X = 200, Y = 200 },
    ///             // Type of the connector segment
    ///             Type = ConnectorSegmentType.Straight,
    ///             TargetDecorator = new DecoratorSettings()
    ///             {
    ///                 Shape = DecoratorShape.Arrow,
    ///                 Style = new ShapeStyle()
    ///                 {
    ///                     Fill = "#6f409f",
    ///                     StrokeColor = "#6f409f",
    ///                     StrokeWidth = 1
    ///                 }
    ///             }
    ///         };
    ///         connectors.Add(Connector);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum DecoratorShape
    {
        /// <summary>
        /// Sets the decorator’s shape to arrow.
        /// </summary>
        [EnumMember(Value = "Arrow")]
        Arrow,
        /// <summary>
        /// Sets the decorator's shape to none.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Sets the decorator shape to diamond.
        /// </summary>
        [EnumMember(Value = "Diamond")]
        Diamond,
        /// <summary>
        /// Sets the decorator shape to open arrow.
        /// </summary>
        [EnumMember(Value = "OpenArrow")]
        OpenArrow,
        /// <summary>
        /// Sets the decorator shape to circle.
        /// </summary>
        [EnumMember(Value = "Circle")]
        Circle,
        /// <summary>
        /// Sets the decorator shape to square.
        /// </summary>
        [EnumMember(Value = "Square")]
        Square,
        /// <summary>
        /// Sets the decorator shape to fletch.
        /// </summary>
        [EnumMember(Value = "Fletch")]
        Fletch,
        /// <summary>
        /// Sets the decorator shape to open fletch
        /// </summary>
        [EnumMember(Value = "OpenFletch")]
        OpenFletch,
        /// <summary>
        /// Sets the decorator shape to in arrow
        /// </summary>
        [EnumMember(Value = "InArrow")]
        InArrow,
        /// <summary>
        /// Sets the decorator shape to out arrow
        /// </summary>
        [EnumMember(Value = "OutArrow")]
        OutArrow,
        /// <summary>
        /// Sets the decorator’s shape to double arrow
        /// </summary>
        [EnumMember(Value = "DoubleArrow")]
        DoubleArrow,
        /// <summary>
        /// Sets the decorator’s shape to custom
        /// </summary>
        [EnumMember(Value = "Custom")]
        Custom,
    }


    /// <summary>
    /// Specifies the orthogonal connector's connection segment direction. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px"  @bind-Connectors="@connectors">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     public DiagramObjectCollection<Connector> connectors = new DiagramObjectCollection<Connector>();    
    ///     Connector connector = new Connector()
    ///     {
    ///         ID = "connector",
    ///         SourceID = "node13",
    ///         TargetID = "node14",
    ///         Type = ConnectorSegmentType.Orthogonal,
    ///         Segments = new DiagramObjectCollection<ConnectorSegment>() 
    ///         { 
    ///             new OrthogonalSegment() 
    ///             { 
    ///                 Length = 70, 
    ///                 Type = ConnectorSegmentType.Orthogonal, 
    ///                 Direction = Direction.Right 
    ///             }, 
    ///             new OrthogonalSegment() 
    ///             { 
    ///                 Length = 20, 
    ///                 Type = ConnectorSegmentType.Orthogonal, 
    ///                 Direction = Direction.Bottom 
    ///             } 
    ///         }
    ///     };
    ///     connectors.Add(connector);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum Direction
    {
        /// <summary>
        /// Sets the direction of the connector segment to Left.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,
        /// <summary>
        /// Sets the direction of the connector segment to Right.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
        /// <summary>
        ///Sets the direction of the connector segment to Top.
        /// </summary>
        [EnumMember(Value = "Top")]
        Top,
        /// <summary>
        /// Sets the direction of the connector segment to Bottom.
        /// </summary>
        [EnumMember(Value = "Bottom")]
        Bottom,
    }


    [Flags]
    internal enum MatrixTypes
    {
        Identity = 0,
        Translation = 1,
        Scaling = 2,
        Unknown = 4
    }

    [Flags]
    internal enum ScrollActions
    {
        None = 0,
        PropertyChange = 1 << 1,
        Interaction = 1 << 2,
        PublicMethod = 1 << 3
    }
    /// <summary>
    /// Enables or disables certain features and behaviors of the annotations.
    /// </summary>
    [Flags]
    public enum AnnotationConstraints
    {
        /// <summary>
        /// Disables all the functionalities of annotation.
        /// </summary>
        None = 0,
        /// <summary>
        /// Enables the user to only read the annotation (cannot be edited).
        /// </summary>
        ReadOnly = 1 << 1,
        /// <summary>
        /// Enables or disables the user from inheriting the ReadOnly option from the parent.
        /// </summary>
        InheritReadOnly = 1 << 2,
    }

    internal enum NoOfSegments
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five
    }
    /// <summary>
    /// It specifies the alignment of the diagram elements based on its immediate parent.
    /// </summary>
    public enum AnnotationAlignment
    {
        /// <summary>
        /// Annotation placed on the connector segment.
        /// </summary>
        [EnumMember(Value = "Center")]
        Center,
        /// <summary>
        /// Annotation placed on top of the connector segment.
        /// </summary>
        [EnumMember(Value = "Before")]
        Before,
        /// <summary>
        /// Annotation placed at the bottom of the connector segment.
        /// </summary>
        [EnumMember(Value = "After")]
        After,
    }


    /// <summary>
    /// Defines the orientation of the Page
    /// </summary>
    public enum PageOrientation
    {
        /// <summary>
        /// Display with page Width is more than the page Height
        /// </summary>
        Landscape,

        /// <summary>
        /// Display with page Height is more than the page width
        /// </summary>
        Portrait
    }


    /// <summary>
    /// Allows users to specify the region of the diagram in which they can interact with it. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px">
    ///     <PageSettings @bind-BoundaryConstraints="@constraints" >
    ///     </PageSettings>
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     BoundaryConstraints constraints = BoundaryConstraints.Diagram;
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum BoundaryConstraints
    {
        /// <summary>
        /// Allow interactions to take place at the infinite height and width of the diagram region.
        /// </summary>
        [EnumMember(Value = "Infinity")]
        Infinity,
        /// <summary>
        /// Allow interactions to take place inside the diagram height and width.
        /// </summary>
        [EnumMember(Value = "Diagram")]
        Diagram,
        /// <summary>
        /// Allow interactions to take place around the height and width mentioned in the page settings.
        /// </summary>
        [EnumMember(Value = "Page")]
        Page,
    }

    /// <summary> 
    /// Specifies the region that has to be printed or exported in diagram. 
    /// </summary> 
    public enum DiagramPrintExportRegion
    {
        /// <summary> 
        /// Specifies the region within the x,y, width and height values of page settings is printed or exported. 
        /// </summary> 
        [EnumMember(Value = "PageSettings")]
        PageSettings,
        /// <summary> 
        /// Specifies the content of the diagram without empty space around the content is printed or exported. 
        /// </summary> 
        [EnumMember(Value = "Content")]
        Content,
        /// <summary> 
        /// Specifies the region specified using <see cref="DiagramExportSettings.ClipBounds"/> property is exported. This is applicable for exporting only. 
        /// </summary> 
        ///<remarks>This is applicable only for diagram exporting</remarks> 
        [EnumMember(Value = "ClipBounds")]
        ClipBounds,
    }

    /// <summary>
    /// Specifies the process of overlaying images of the same scene under different condition of the image.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node5 = new Node()
    /// {
    ///   OffsetX = 500,
    ///   OffsetY = 300,
    ///   Shape = new ImageShape()
    ///   {
    ///     Type = Shapes.Image, ImageAlign = ImageAlignment.None, Scale = Scale.None,
    ///     Source = " https://www.syncfusion.com/content/images/nuget/sync_logo_icon.png" 
    ///   },
    /// };
    /// nodes.Add(node5);
    /// ]]>
    /// </code>
    /// </example>
    public enum ImageAlignment
    {
        /// <summary>
        /// Sets the none alignments for the image.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Sets the smallest X value of the view port and  smallest Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMinYMin")]
        XMinYMin,
        /// <summary>
        /// Sets the midpoint X value of the view port and  smallest Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMidYMin")]
        XMidYMin,
        /// <summary>
        /// Sets the maximum X value of the view port and  smallest Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMaxYMin")]
        XMaxYMin,
        /// <summary>
        /// Sets the smallest X value of the view port and  midpoint Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMinYMid")]
        XMinYMid,
        /// <summary>
        /// Sets the smallest X value of the view port and  midpoint Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMidYMid")]
        XMidYMid,
        /// <summary>
        /// Sets the maximum X value of the view port and  midpoint Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMaxYMid")]
        XMaxYMid,
        /// <summary>
        /// Sets the smallest X value of the view port and  maximum Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMinYMax")]
        XMinYMax,
        /// <summary>
        /// Sets the midpoint X value of the view port and  maximum Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMidYMax")]
        XMidYMax,
        /// <summary>
        /// Sets the maximum X value of the view port and  maximum Y value of the view port for the image.
        /// </summary>
        [EnumMember(Value = "XMaxYMax")]
        XMaxYMax,
    }

    /// <summary>
    /// Indicates whether or not the image should be scaled uniformly.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="1000px">
    /// <BackgroundStyle ImageSource = "@imageSource" ImageScale="@imageScale”></BackgroundStyle>
    /// </SfDiagramComponent>
    /// string imageSource = "https://www.w3schools.com/images/w3schools_green.jpg";
    /// Scale imageScale = Scale.Slice;
    /// ]]>
    /// </code>
    /// </example>
    public enum Scale
    {
        /// <summary>
        /// There will be no scaling of the image.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// stretches the content in both the x and y dimensions until it fills the width or height provided.
        /// </summary>
        [EnumMember(Value = "Meet")]
        Meet,
        /// <summary>
        /// Preserves the aspect ratio of the content but scales up the graphic until it fills both the width and height provided (clipping the content that overflows the bounds).
        /// </summary>
        [EnumMember(Value = "Slice")]
        Slice,
    }

    /// <summary>
    /// Defines the scrollable region of the diagram.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="1000px">
    /// <ScrollSettings @bind-ScrollLimit="@scrollLimit">
    /// </ScrollSettings>
    /// </SfDiagramComponent>
    /// @code
    /// { 
    ///     ScrollLimitMode scrollLimit { get; set; } = ScrollLimitMode.Infinity;
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum ScrollLimitMode
    {
        /// <summary>
        /// Enables scrolling to view the diagram content.
        /// </summary>
        [EnumMember(Value = "Diagram")]
        Diagram,
        /// <summary>
        /// The Diagram will be extended when we try to scroll the through it.
        /// </summary>
        [EnumMember(Value = "Infinity")]
        Infinity,
        /// <summary>
        /// Enables scrolling to view the specified area.
        /// </summary>
        [EnumMember(Value = "Limited")]
        Limited,
    }
    /// <summary>
    /// File Format type for export.
    /// </summary>
    public enum DiagramExportFormat
    {
        /// <summary>
        /// Save the file in JPG Format
        /// </summary>
        [EnumMember(Value = "JPEG")]
        JPEG,
        /// <summary>
        /// Save the file in PNG Format
        /// </summary>
        [EnumMember(Value = "PNG")]
        PNG,
        /// <summary>
        /// Save the file in SVG Format
        /// </summary>
        [EnumMember(Value = "SVG")]
        SVG
    }

   
    /// <summary>
    /// Describes how content is resized to fill its allocated space. 
    /// </summary>
    internal enum Stretch
    {
        /// <summary>
        /// Does not preserve the aspect ratio. Scales image to fit  the view box fully into the available space. Proportions will be distorted.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Stretch the image to fit into the available space.
        /// </summary>
        [EnumMember(Value = "Stretch")]
        Stretch,
        /// <summary>
        /// Preserves the aspect ratio and scales the view box to fit within the available space.
        /// </summary>
        [EnumMember(Value = "Meet")]
        Meet,
        /// <summary>
        /// Preserves the aspect ratio and slices of any part of the image that does not fit inside the available space.
        /// </summary>
        [EnumMember(Value = "Slice")]
        Slice,
    }
    /// <summary>
    /// Specifies the types of automatic layout. 
    /// </summary>
    public enum LayoutType
    {
        /// <summary>
        /// None of the layouts are applied.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Defines the type of layout as a Hierarchical Tree.
        /// </summary>
        [EnumMember(Value = "HierarchicalTree")]
        HierarchicalTree,
        /// <summary>
        /// Defines the type of layout as an Organizational Chart.
        /// </summary>
        [EnumMember(Value = "OrganizationalChart")]
        OrganizationalChart,
        /// <summary>
        /// Defines the type of layout as a Mind Map.
        /// </summary>
        [EnumMember(Value = "MindMap")]
        MindMap
    }

    /// <summary>
    /// Specifies the collection of sub tree alignments in an organizational chart. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Height = "600px" NodeCreating="@NodeDefaults" ConnectorCreating="@ConnectorDefaults">
    ///     <DataSourceSettings ID = "Id" ParentID="Team" DataSource="@DataSource"></DataSourceSettings>   
    ///     <Layout Type = "LayoutType.OrganizationalChart"
    ///             @bind-HorizontalSpacing="@HorizontalSpacing" 
    ///             @bind-VerticalSpacing="@VerticalSpacing" 
    ///             GetLayoutInfo="GetLayoutInfo">
    ///     </Layout>
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///    int HorizontalSpacing = 40;
    ///    int VerticalSpacing = 50;
    ///    private TreeInfo GetLayoutInfo(IDiagramObject obj, TreeInfo options)
    ///    {
    ///         options.AlignmentType = SubTreeAlignmentType.Alternate;
    ///         options.Orientation = Orientation.Vertical;
    ///         return options;         
    ///    }
    ///    private void ConnectorDefaults(IDiagramObject connector)
    ///    {
    ///        (connector as Connector).Type = ConnectorSegmentType.Orthogonal;
    ///        (connector as Connector).TargetDecorator.Shape = DecoratorShape.None;
    ///        (connector as Connector).Style = new ShapeStyle() { StrokeColor = "#6d6d6d" };
    ///        (connector as Connector).Constraints = 0;
    ///        (connector as Connector).CornerRadius = 5;
    ///    }
    ///    private void NodeDefaults(IDiagramObject obj)
    ///    {
    ///        Node node = obj as Node;
    ///        node.Height = 50;
    ///        node.Width = 150;
    ///        node.Style = new ShapeStyle() { Fill = "#6495ED", StrokeWidth = 1, StrokeColor = "Black" };
    ///    }
    ///    public class OrgChartDataModel 
    ///    {
    ///        public string Id  { get; set; }
    ///        public string Team { get; set; }
    ///        public string Role { get; set; }
    ///    }
    ///    public object DataSource = new List<object>()
    ///    {
    ///        new OrgChartDataModel() { Id= "1", Role= "General Manager" },
    ///        new OrgChartDataModel() { Id= "2", Role= "Human Resource Manager", Team= "1" },
    ///        new OrgChartDataModel() { Id= "3", Role= "Design Manager", Team= "1" },
    ///        new OrgChartDataModel() { Id= "4", Role= "Operation Manager", Team= "1" },
    ///        new OrgChartDataModel() { Id= "5", Role= "Marketing Manager", Team= "1" }
    ///    };
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum SubTreeAlignmentType
    {
        /// <summary>
        /// Aligns the child nodes at the left of the parent in a horizontal/vertical sub tree.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,
        /// <summary>
        /// Aligns the child nodes at the right of the parent in a horizontal/vertical sub tree.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
        /// <summary>
        /// Aligns the child nodes at the center of the parent in a horizontal sub tree.
        /// </summary>
        [EnumMember(Value = "Center")]
        Center,
        /// <summary>
        /// Aligns the child nodes alternatively on both left and right sides in a vertical sub tree.
        /// </summary>
        [EnumMember(Value = "Alternate")]
        Alternate,
        /// <summary>
        /// Aligns the child nodes horizontally to balance the width and height of the sub tree.
        /// </summary>
        [EnumMember(Value = "Balanced")]
        Balanced
    }
    /// <summary>
    /// Specifies the orientation of the automatic layout. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent @ref="@Diagram" Height="499px" InteractionController="@InteractionController.ZoomPan" ConnectorCreating="@ConnectorDefaults" NodeCreating="@NodeDefaults">
    ///     <DataSourceSettings ID = "Name" ParentID="Category" DataSource="DataSource"> </DataSourceSettings>
    ///         <Layout @bind-Type="type" @bind-HorizontalSpacing="@HorizontalSpacing" @bind-Orientation="@orientation" @bind-VerticalSpacing="@VerticalSpacing" @bind-HorizontalAlignment="@horizontalAlignment" @bind-VerticalAlignment="@verticalAlignment" GetLayoutInfo="GetLayoutInfo">                
    ///         </Layout>            
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///    SfDiagramComponent Diagram;
    ///    public int? HValue { get; set; } = 30;
    ///    public int? VValue { get; set; } = 30;
    ///    LayoutType type = LayoutType.HierarchicalTree;
    ///    LayoutOrientation orientation = LayoutOrientation.TopToBottom;
    ///    HorizontalAlignment horizontalAlignment = HorizontalAlignment.Auto;
    ///    VerticalAlignment verticalAlignment = VerticalAlignment.Auto;
    ///    int HorizontalSpacing = 30;
    ///    int VerticalSpacing = 30;
    ///    private void ConnectorDefaults(IDiagramObject connector)
    ///    {
    ///        (connector as Connector).Type = ConnectorSegmentType.Orthogonal;
    ///        (connector as Connector).TargetDecorator.Shape = DecoratorShape.None;
    ///        (connector as Connector).Style = new ShapeStyle() { StrokeColor = "#6d6d6d" };
    ///        (connector as Connector).Constraints = 0;
    ///        (connector as Connector).CornerRadius = 5;
    ///    }
    ///    private TreeInfo GetLayoutInfo(IDiagramObject obj, TreeInfo options)
    ///    {
    ///        options.EnableSubTree = true;
    ///        options.Orientation = Orientation.Horizontal;
    ///        return options;
    ///    }
    ///    private void NodeDefaults(IDiagramObject obj)
    ///    {
    ///        Node node = obj as Node;
    ///        if (node.Data is System.Text.Json.JsonElement)
    ///        {
    ///            node.Data = System.Text.Json.JsonSerializer.Deserialize<HierarchicalDetails>(node.Data.ToString());
    ///        }
    ///        HierarchicalDetails hierarchicalData = node.Data as HierarchicalDetails;
    ///        node.Style = new ShapeStyle() { Fill = "#659be5", StrokeColor = "none", StrokeWidth = 2, };
    ///        node.BackgroundColor = "#659be5";
    ///        node.Width = 150;
    ///        node.Height = 50;
    ///        node.Annotations = new DiagramObjectCollection<ShapeAnnotation>()
    ///        {
    ///            new ShapeAnnotation()
    ///            {
    ///                Content = hierarchicalData.Name,
    ///                Style =new TextStyle(){Color = "white"}
    ///            }
    ///        };
    ///    }
    ///    public class HierarchicalDetails 
    ///    {
    ///        public string Name { get; set; }
    ///        public string FillColor { get; set; }
    ///        public string Category { get; set; }
    ///    }
    ///    public List<HierarchicalDetails> DataSource = new List<HierarchicalDetails>()
    ///    {
    ///        new HierarchicalDetails(){ Name ="Diagram", Category="",FillColor="#659be5"},
    ///        new HierarchicalDetails(){ Name ="Layout", Category="Diagram",FillColor="#659be5"},
    ///        new HierarchicalDetails(){ Name ="Tree layout", Category="Layout",FillColor="#659be5"},
    ///        new HierarchicalDetails(){ Name ="Organizational chart", Category="Layout",FillColor="#659be5"},
    ///        new HierarchicalDetails(){ Name ="Hierarchical tree", Category="Tree layout",FillColor="#659be5"},
    ///        new HierarchicalDetails(){ Name ="Radial tree", Category="Tree layout",FillColor="#659be5"},
    ///        new HierarchicalDetails(){ Name ="Mind map", Category="Hierarchical tree",FillColor="#659be5"},
    ///    };
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum LayoutOrientation
    {
        /// <summary>
        /// Renders the layout from top to bottom.
        /// </summary>
        [EnumMember(Value = "TopToBottom")]
        TopToBottom,
        /// <summary>
        /// Renders the layout from bottom to top.
        /// </summary>
        [EnumMember(Value = "BottomToTop")]
        BottomToTop,
        /// <summary>
        /// Renders the layout from left to right.
        /// </summary>
        [EnumMember(Value = "LeftToRight")]
        LeftToRight,
        /// <summary>
        /// Renders the layout from right to left.
        /// </summary>
        [EnumMember(Value = "RightToLeft")]
        RightToLeft
    }

    /// <summary>
    /// Representing the placement of child elements in a vertical or horizontal stack
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px"  SetNodeTemplate="SetTemplate">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///  private ICommonElement SetTemplate(IDiagramObject node)
    ///  {
    ///    var table = new StackPanel();
    ///    table.Orientation = Orientation.Horizontal;
    ///    return table;
    ///  }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum Orientation
    {
        /// <summary>
        /// Sets the orientation to Horizontal.
        /// </summary>
        [EnumMember(Value = "Horizontal")]
        Horizontal,
        /// <summary>
        /// Sets the orientation to Vertical.
        /// </summary>
        [EnumMember(Value = "Vertical")]
        Vertical,
    }

    /// <summary>
    /// It specifies the action to be performed in the diagram while interacting.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// public string cursor(Actions action, bool active, string handle)
    /// {
    ///     string cursors = null;
    ///     if (action == Actions.Drag)
    ///     cursors = "-webkit-grabbing";
    ///     return cursors;
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [Flags]
    public enum Actions
    {
        /// <summary>
        /// None of the actions are performed by the cursor.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Notifies when the selection happens.
        /// </summary>
        [EnumMember(Value = "Select")]
        Select,
        /// <summary>
        /// Notifies when the dragging is ongoing.
        /// </summary>
        [EnumMember(Value = "Drag")]
        Drag,
        /// <summary>
        /// Notifies that the node is currently resizing to west direction.
        /// </summary>
        [EnumMember(Value = "ResizeWest")]
        ResizeWest,
        /// <summary>
        /// Notifies that the connector source points are currently dragging.
        /// </summary>
        [EnumMember(Value = "ConnectorSourceEnd")]
        ConnectorSourceEnd,
        /// <summary>
        /// Notifies that the connector Target points are currently dragging.
        /// </summary>
        [EnumMember(Value = "ConnectorTargetEnd")]
        ConnectorTargetEnd,
        /// <summary>
        /// Notifies that the node is currently resizing to east direction.
        /// </summary>
        [EnumMember(Value = "ResizeEast")]
        ResizeEast,
        /// <summary>
        /// Notifies that the node is currently resizing to south direction.
        /// </summary>
        [EnumMember(Value = "ResizeSouth")]
        ResizeSouth,
        /// <summary>
        /// Notifies that the node is currently resizing to north direction.
        /// </summary>
        [EnumMember(Value = "ResizeNorth")]
        ResizeNorth,
        /// <summary>
        /// Notifies that the node is currently resizing to south east direction.
        /// </summary>
        [EnumMember(Value = "ResizeSouthEast")]
        ResizeSouthEast,
        /// <summary>
        /// Notifies that the node is currently resizing to south west direction.
        /// </summary>
        [EnumMember(Value = "ResizeSouthWest")]
        ResizeSouthWest,
        /// <summary>
        /// Notifies that the node is currently resizing to north east direction.
        /// </summary>
        [EnumMember(Value = "ResizeNorthEast")]
        ResizeNorthEast,
        /// <summary>
        /// Notifies that the node is currently resizing to north west direction.
        /// </summary>
        [EnumMember(Value = "ResizeNorthWest")]
        ResizeNorthWest,
        /// <summary>
        /// Notifies that the node is currently rotating.
        /// </summary>
        [EnumMember(Value = "Rotate")]
        Rotate,
        /// <summary>
        /// Notifies that the node is currently panning.
        /// </summary>
        [EnumMember(Value = "Pan")]
        Pan,
        /// <summary>
        /// Notifies the bezier connector source points are currently dragging.
        /// </summary>
        [EnumMember(Value = "BezierSourceThumb")]
        BezierSourceThumb,
        /// <summary>
        /// Notifies that the bezier connector target points are currently dragging.
        /// </summary>
        [EnumMember(Value = "BezierTargetThumb")]
        BezierTargetThumb,
        /// <summary>
        /// Notifies that the connector segment endpoint is currently dragging.
        /// </summary>
        [EnumMember(Value = "SegmentEnd")]
        SegmentEnd,
        /// <summary>
        /// Notifies that the connector orthogonal thumb is currently dragging.
        /// </summary>
        [EnumMember(Value = "OrthogonalThumb")]
        OrthogonalThumb,
        /// <summary>
        /// Notifies that the fixed user handle is currently active.
        /// </summary>
        [EnumMember(Value = "FixedUserHandle")]
        FixedUserHandle,
        /// <summary>
        /// Notifies the hyperlink action when the mouse pointer is hovering over it.
        /// </summary>
        [EnumMember(Value = "Hyperlink")]
        Hyperlink,
        /// <summary>
        /// Notifies the drawing of shapes using the drawing tool.
        /// </summary>
        [EnumMember(Value = "Draw")]
        Draw,
        /// <summary>
        /// Notifies the port draw action when drawing the connector from the port.
        /// </summary>
        [EnumMember(Value = "PortDraw")]
        PortDraw,
        /// <summary>
        /// Notifies to interact the zoom action through touch.
        /// </summary>
        [EnumMember(Value = "PinchZoom")]
        PinchZoom
    }
    /// <summary>
    /// Allows the users to customize the selection, zooming, and interaction behavior of the diagram. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent @ref="@diagram" Width="1000px"  Height="1000px" InteractionController="Tools" DrawingObject="@DrawingObject" Nodes="@nodes">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///    SfDiagramComponent diagram;
    ///    InteractionController Tools = InteractionController.ContinuousDraw;
    ///    IDiagramObject DrawingObject { get; set; }
    ///    DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>();
    ///    protected override void OnInitialized()
    ///    {
    ///        Node node = new Node()
    ///        {
    ///            Shape = new FlowShape() { Type = Shapes.Flow, Shape = FlowShapeType.Decision }
    ///        };
    ///        DrawingObject = node;
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [Flags]
    public enum InteractionController
    {
        /// <summary>
        /// It disables the selection, zooming, and interaction behavior of the diagram.
        /// </summary>
        None = 0,
        /// <summary>
        /// It allows users to select one node or connector at a time.
        /// </summary>
        SingleSelect = 1 << 0,
        /// <summary>
        /// It allows users to select multiple nodes and connectors. It won't allow selecting a single node/connector.
        /// </summary>
        MultipleSelect = 1 << 1,
        /// <summary>
        /// It allows users to pan the diagram.
        /// </summary>
        ZoomPan = 1 << 2,
        /// <summary>
        /// It enables users to draw the drawing objects at once. 
        /// </summary>
        DrawOnce = 1 << 3,
        /// <summary>
        /// It enables users to draw the drawing objects continuously.
        /// </summary>
        ContinuousDraw = 1 << 4,
        /// <summary>
        /// By default, it allows users to select an individual as well as multiple nodes and connectors.
        /// </summary>
        Default = 1 << 0 | 1 << 1,
    }

    /// <summary>
    /// Defines how to handle the selected items via rubber band selection.
    /// </summary>
    public enum RubberBandSelectionMode
    {
        /// <summary>
        /// Selects the objects that are contained within the selected region.
        /// </summary>
        [EnumMember(Value = "CompleteIntersect")]
        CompleteIntersect,
        /// <summary>
        /// Selects the objects that are partially intersected with the selected region.
        /// </summary>
        [EnumMember(Value = "PartialIntersect")]
        PartialIntersect,
    }

    /// <summary>
    /// Enables or disables certain behaviors and features of the selector.
    /// </summary>
    [Flags]
    public enum SelectorConstraints
    {
        /// <summary>
        /// Hides all the selector elements.
        /// </summary>
        None = 0,
        /// <summary>
        /// Enables or disables the source thumb of the connector.
        /// </summary>
        ConnectorSourceThumb = 1 << 1,
        /// <summary>
        /// Enables or disables the target thumb of the connector.
        /// </summary>
        ConnectorTargetThumb = 1 << 2,
        /// <summary>
        /// Enables or disables the bottom right resize handle of the selector.
        /// </summary>
        ResizeSouthEast = 1 << 3,
        /// <summary>
        /// Enables or disables the bottom left resize handle of the selector. 
        /// </summary>
        ResizeSouthWest = 1 << 4,
        /// <summary>
        /// Enables or disables the top right resize handle of the selector.
        /// </summary>
        ResizeNorthEast = 1 << 5,
        /// <summary>
        /// Enables or disables the top left resize handle of the selector.
        /// </summary>
        ResizeNorthWest = 1 << 6,
        /// <summary>
        /// Enables or disables the middle right resize handle of the selector.
        /// </summary>
        ResizeEast = 1 << 7,
        /// <summary>
        /// Enables or disables the middle left resize handle of the selector.
        /// </summary>
        ResizeWest = 1 << 8,
        /// <summary>
        /// Enables or disables the bottom center resize handle of the selector.
        /// </summary>
        ResizeSouth = 1 << 9,
        /// <summary>
        /// Enables or disables the top center resize handle of the selector.
        /// </summary>
        ResizeNorth = 1 << 10,
        /// <summary>
        /// Enables or disables the rotate handle of the selector.
        /// </summary>
        Rotate = 1 << 11,
        /// <summary>
        ///  Enables or disables the user handles of the selector .
        /// </summary>
        UserHandle = 1 << 12,
        /// <summary>
        /// Enables or disables all handles of the selector.
        /// </summary>
        All = ResizeAll | Rotate | UserHandle,
        /// <summary>
        /// Enables or disables all resize handles of the selector.
        /// </summary>
        ResizeAll = ResizeSouthEast | ResizeSouthWest | ResizeNorthEast |
    ResizeNorthWest | ResizeEast | ResizeWest | ResizeSouth | ResizeNorth | ConnectorSourceThumb | ConnectorTargetThumb,

    }

    /// <summary>
    /// Specifies whether an object is added/removed from the diagram.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="1000px" SelectionChanging="@OnSelectionChanging">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     private void OnSelectionChanging(SelectionChangingEventArgs args)
    ///     {
    ///         if ((args != null) && (args.NewValue != null) && (args.OldValue != null))
    ///         {
    ///             CollectionChangedAction type = args.Type;
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>

    public enum CollectionChangedAction
    {
        /// <summary>
        /// Indicates that the object has been added to the diagram.
        /// </summary>
        [EnumMember(Value = "Add")]
        Add,
        /// <summary>
        /// Indicates that the object has been removed from the diagram.
        /// </summary>
        [EnumMember(Value = "Remove")]
        Remove,
    }

    /// <summary>
    /// Stores certain diagram state information that differ based on the usecase. For example, diagram rendering is in progress or any interaction currently in progress. 
    /// </summary>
    [Flags]
    public enum DiagramAction
    {
        ///<summary>
        /// It indicates that the component was currently in a rendering state.
        ///</summary>
        Render = 1 << 1,

        /// <summary>
        /// It indicates that the current action was initiated through public API methods.
        /// </summary>
        PublicMethod = 1 << 2,

        /// <summary>
        /// It indicates whether the drag or rotate or resize interaction is in progress.
        /// </summary>
        Interactions = 1 << 3,

        /// <summary>
        /// It indicates the layout process is currently in progress.
        /// </summary>
        Layouting = 1 << 4,
        /// <summary>
        /// It indicates whether a group node is currently dragging in the state.
        /// </summary>
        IsGroupDragging = 1 << 5,

        /// <summary>
        /// It indicates that the diagram undo/redo action is in progress.
        /// </summary>
        UndoRedo = 1 << 6,
        /// <summary>
        /// It indicates group action in progress.
        /// </summary>
        Group = 1 << 7,
        /// <summary>
        /// It indicates whether the diagram drawing tool is currently active or not.
        /// </summary>
        DrawingTool = 1 << 8,
        /// <summary>
        /// It indicates whether any annotation is currently being edited in the state. 
        /// </summary>
        EditText = 1 << 9
    }
    [Flags]
    internal enum RealAction
    {
        PreventDrag = 1 << 1,
        PreventScale = 1 << 2,
        PreventDataInit = 1 << 3,
        /// <summary>
        /// Indicates whether JS calling has been made.
        /// </summary>
        MeasureDataJSCall = 1 << 4,

        /// <summary>
        /// Indicates to prevent the whole diagram refresh.
        /// </summary>
        PreventRefresh = 1 << 5,

        /// <summary>
        /// Indicates whether path data is measuring has been made.
        /// </summary>
        PathDataMeasureAsync = 1 << 6,

        /// <summary>
        /// Enable the group action.
        /// </summary>
        EnableGroupAction = 1 << 7,

        /// <summary>
        /// Indicates to prevent the diagram refresh.
        /// </summary>
        PreventEventRefresh = 1 << 8,
        /// <summary>
        /// Indicates to diagram is preformed scroll actions.
        /// </summary>
        ScrollActions = 1 << 9,
        /// <summary>
        /// Indicates to prevent the path data measure.
        /// </summary>
        PreventPathDataMeasure = 1 << 10,
        /// <summary>
        /// Indicates the symbol is being dragged from the palette
        /// </summary>
        SymbolDrag = 1 << 11,

        /// <summary>
        /// Indicates to collection change event is cancelled.
        /// </summary>
        CancelCollectionChange = 1 << 12,
        /// <summary>
        /// Indicates to collection change event is cancelled.
        /// </summary>
        GroupingCollectionChange = 1 << 13,
        /// <summary>
        /// Indicates to refresh the selector layer.
        /// </summary>
        RefreshSelectorLayer = 1 << 14,
        /// <summary>
        /// Indicates to prevent the drag source while group dragging.
        /// </summary>
        PreventDragSource = 1 << 15
    }

    /// <summary>
    /// Specifies to enables/disables the handles for the selected items
    /// </summary>
    [Flags]
    internal enum ThumbsConstraints
    {
        None,
        /// <summary>
        /// Sets the source thumb of the connector.
        /// </summary>
        ConnectorSource = 1 << 2,
        /// <summary>
        /// Sets the target thumb of the connector.
        /// </summary>
        ConnectorTarget = 1 << 3,
        /// <summary>
        /// Sets all handles of the selected items.
        /// </summary>
        Default = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5 | 1 << 6 | 1 << 7 | 1 << 8 | 1 << 9 | 1 << 10 | 1 << 11,
        /// <summary>
        /// Sets the middle right resize handle of the selected items.
        /// </summary>
        ResizeEast = 1 << 5,
        /// <summary>
        /// Sets the top center resize handle of the selected items.
        /// </summary>
        ResizeNorth = 1 << 11,
        /// <summary>
        /// Sets the top right resize handle of the selected items.
        /// </summary>
        ResizeNorthEast = 1 << 4,
        /// <summary>
        /// Sets the top left resize handle of the selected items.
        /// </summary>
        ResizeNorthWest = 1 << 10,
        /// <summary>
        /// Sets the bottom center resize handle of the selected items.
        /// </summary>
        ResizeSouth = 1 << 7,
        /// <summary>
        /// Sets the bottom right resize handle of the selected items.
        /// </summary>
        ResizeSouthEast = 1 << 6,
        /// <summary>
        /// Sets the bottom left resize handle of the selected items. 
        /// </summary>
        ResizeSouthWest = 1 << 8,
        /// <summary>
        /// Sets the middle left resize handle of the selected items.
        /// </summary>
        ResizeWest = 1 << 9,
        /// <summary>
        /// Sets the rotate handle of the selected items.
        /// </summary>
        Rotate = 1 << 1
    }
    /// <summary>
    ///  Represents the type of the BPMN shapes in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Gateway, 
    ///         Gateway = new BpmnGateway() 
    ///         {              
    ///             Type = BpmnGateways.None
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnShapes
    {
        /// <summary>
        /// Represents the Bpmn Shape as Event.
        /// </summary>
        [EnumMember(Value = "Event")]
        Event,
        /// <summary>
        /// Represents the Bpmn Shape as Gateway.
        /// </summary>
        [EnumMember(Value = "Gateway")]
        Gateway,
        /// <summary>
        /// Represents the Bpmn Shape as Message.
        /// </summary>
        [EnumMember(Value = "Message")]
        Message,
        /// <summary>
        /// Represents the Bpmn Shape as Data Object.
        /// </summary>
        [EnumMember(Value = "DataObject")]
        DataObject,
        /// <summary>
        /// Represents the Bpmn Shape as Data Source.
        /// </summary>
        [EnumMember(Value = "DataSource")]
        DataSource,
        /// <summary>
        /// Represents the Bpmn Shape as Activity.
        /// </summary>
        [EnumMember(Value = "Activity")]
        Activity,
        /// <summary>
        /// Represents the Bpmn Shape as Group.
        /// </summary>
        [EnumMember(Value = "Group")]
        Group,
        /// <summary>
        /// Represents the shape as Text Annotation
        /// </summary>
        [EnumMember(Value = "TextAnnotation")]
        TextAnnotation,
    }

    /// <summary>
    /// In BPMN, the events are expressed as circles in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Event, 
    ///         Events = new BpmnSubEvent() 
    ///         { 
    ///             Event = BpmnEvents.Intermediate, 
    ///             Trigger = BpmnTriggers.None 
    ///         }
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnEvents
    {
        /// <summary>
        /// Represents an event that starts a new process instance in the diagram.
        /// </summary>
        [EnumMember(Value = "Start")]
        Start,
        /// <summary>
        /// Represents a process that can only continue once an event has been caught in the diagram.
        /// </summary>
        [EnumMember(Value = "Intermediate")]
        Intermediate,
        /// <summary>
        /// Represents the final step of the process.
        /// </summary>
        [EnumMember(Value = "End")]
        End,
        /// <summary>
        /// Represents the start of an activity along with the trigger type. 
        /// </summary>
        [EnumMember(Value = "NonInterruptingStart")]
        NonInterruptingStart,
        /// <summary>
        /// Represents the continuing once along with the trigger type. 
        /// </summary>
        [EnumMember(Value = "NonInterruptingIntermediate")]
        NonInterruptingIntermediate,
        /// <summary>
        /// Represents the intermediate events that can be attached to the trigger.
        /// </summary>
        [EnumMember(Value = "ThrowingIntermediate")]
        ThrowingIntermediate,

    }
    /// <summary>
    /// Represents the type of the Bpmn Triggers in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Event, 
    ///         Events = new BpmnSubEvent() 
    ///         { 
    ///             Event = BpmnEvents.ThrowingIntermediate, 
    ///             Trigger = BpmnTriggers.None 
    ///         }
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnTriggers
    {
        /// <summary>
        /// Sets the type of the task to None.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Represents the process is started, it is facilitated, or it is completed.
        /// </summary>
        [EnumMember(Value = "Message")]
        Message,
        /// <summary>
        /// Represents the time and date that triggers the process.
        /// </summary>
        [EnumMember(Value = "Timer")]
        Timer,
        /// <summary>
        /// Represents when an escalation occurs, a step is triggered and passed to another position within the organization.
        /// </summary>
        [EnumMember(Value = "Escalation")]
        Escalation,
        /// <summary>
        /// Represents a sub-process that is a component of a bigger process.
        /// </summary>
        [EnumMember(Value = "Link")]
        Link,
        /// <summary>
        ///  Represents an error trigger that will always interrupt the process that is contained in it.
        /// </summary>
        [EnumMember(Value = "Error")]
        Error,
        /// <summary>
        /// Represents a refund that occurs when procedures are partially successful.
        /// </summary>
        [EnumMember(Value = "Compensation")]
        Compensation,
        /// <summary>
        /// Represents a signal that is shared by several processes.
        /// </summary>
        [EnumMember(Value = "Signal")]
        Signal,
        /// <summary>
        /// Represents a process that is started by several triggers.
        /// </summary>
        [EnumMember(Value = "Multiple")]
        Multiple,
        /// <summary>
        /// Represents an instance of a process that does not begin, continue, or finish until all the potential events have occurred.
        /// </summary>
        [EnumMember(Value = "Parallel")]
        Parallel,
        /// <summary>
        /// Represents the cancelling of process.
        /// </summary>
        [EnumMember(Value = "Cancel")]
        Cancel,
        /// <summary>
        /// Represents a process that begins or continues when a business condition or business rule is met.
        /// </summary>
        [EnumMember(Value = "Conditional")]
        Conditional,
        /// <summary>
        /// Represents a process step to be immediately terminated.
        /// </summary>
        [EnumMember(Value = "Terminate")]
        Terminate
    }
    /// <summary>
    /// It allows to control as well as merge and split the process flow in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Gateway, 
    ///         Gateway = new BpmnGateway() 
    ///         {              
    ///             Type = BpmnGateways.None
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnGateways
    {
        /// <summary>
        /// Sets the type of the gateway to None.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Represents and evaluates the state of the business process.
        /// </summary>
        [EnumMember(Value = "Exclusive")]
        Exclusive,
        /// <summary>
        /// It breaks the process flow into one or more flows.
        /// </summary>
        [EnumMember(Value = "Inclusive")]
        Inclusive,
        /// <summary>
        /// Represents two concurrent tasks in a business flow.
        /// </summary>
        [EnumMember(Value = "Parallel")]
        Parallel,
        /// <summary>
        /// It is used for the most complex flows in the business process.
        /// </summary>
        [EnumMember(Value = "Complex")]
        Complex,
        /// <summary>
        /// It is similar to an exclusive gateway because both involve one path in the flow.
        /// </summary>
        [EnumMember(Value = "EventBased")]
        EventBased,
        /// <summary>
        /// It evaluates the state of the business process and, based on the condition.
        /// </summary>
        [EnumMember(Value = "ExclusiveEventBased")]
        ExclusiveEventBased,
        /// <summary>
        /// It allows multiple processes to happen at the same time.
        /// </summary>
        [EnumMember(Value = "ParallelEventBased")]
        ParallelEventBased
    }
    /// <summary>
    /// Represents the transferring of data into or out of an Activity in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.DataObject, 
    ///         DataObject = new BpmnDataObject() 
    ///         { 
    ///             Collection = true, 
    ///             Type = BpmnDataObjects.Output 
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnDataObjects
    {
        /// <summary>
        /// Represents the information flowing through the process.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Represents the input for the entire process in the diagram.
        /// </summary>
        [EnumMember(Value = "Input")]
        Input,
        /// <summary>
        /// Represents the data result of the entire process in the diagram.
        /// </summary>
        [EnumMember(Value = "Output")]
        Output
    }
    /// <summary>
    /// Defines the work that a company or organization performs in a business process using nodes in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Activity, 
    ///         Activity = new BpmnActivity() 
    ///         { 
    ///             Activity = BpmnActivities.Task, 
    ///             Task = new BpmnTask() { Type = BpmnTasks.BusinessRule } 
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnActivities
    {
        /// <summary>
        /// Represents the activity within a process flow. we can create a task when the activity cannot be broken down to a finer level of detail.
        /// </summary>
        [EnumMember(Value = "Task")]
        Task,
        /// <summary>
        /// Represents none of the Bpmn Activities are  performed.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Represents the sub-process is a compound activity that represents a collection of other tasks and sub-processes.
        /// </summary>
        [EnumMember(Value = "SubProcess")]
        SubProcess
    }
    /// <summary>
    /// Represents the task that repeats over and over again in sequence.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Activity, 
    ///         Activity = new BpmnActivity()
    ///         {              
    ///             Activity = BpmnActivities.Task, 
    ///             Task = new BpmnTask() { Loop = BpmnLoops.ParallelMultiInstance } 
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnLoops
    {
        /// <summary>
        /// Sets the type of the Bpmn loop as None, in which none of the loops will exist.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Sets the type of the Bpmn loop to Standard with a repeat symbol in it.
        /// </summary>
        [EnumMember(Value = "Standard")]
        Standard,
        /// <summary>
        /// The Parallel Multi-Instance marking denotes that the sub-process can operate in parallel with other identical sub-processes.
        /// </summary>
        [EnumMember(Value = "ParallelMultiInstance")]
        ParallelMultiInstance,
        /// <summary>
        /// Represents the instances being executed one by one.
        /// </summary>
        [EnumMember(Value = "SequenceMultiInstance")]
        SequenceMultiInstance
    }
    /// <summary>
    /// Represents the activity within a process flow in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Activity, 
    ///         Activity = new BpmnActivity() 
    ///         { 
    ///             Activity = BpmnActivities.Task, 
    ///             Task = new BpmnTask { Call = true }
    ///         }
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnTasks
    {
        /// <summary>
        /// Sets the type of the task to None.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Sets a task that represents a Web service, an automated application, or other kinds of service in completing the task.
        /// </summary>
        [EnumMember(Value = "Service")]
        Service,
        /// <summary>
        /// Sets a task that represents the process has to wait for a message to arrive in order to continue.
        /// </summary>
        [EnumMember(Value = "Receive")]
        Receive,
        /// <summary>
        /// Sets a task that represents sending a Message to another element.
        /// </summary>
        [EnumMember(Value = "Send")]
        Send,
        /// <summary>
        /// Sets a task that represents the process that has to be Instantiated for a message to arrive in order to continue.
        /// </summary>
        [EnumMember(Value = "InstantiatingReceive")]
        InstantiatingReceive,
        /// <summary>
        ///  Represents the aid of any business process execution engine or any application.
        /// </summary>
        [EnumMember(Value = "Manual")]
        Manual,
        /// <summary>
        /// Represents the process of inserting data into a Business Rules Engine and eventually receiving the results.
        /// </summary>
        [EnumMember(Value = "BusinessRule")]
        BusinessRule,
        /// <summary>
        /// Represents that the task is completed by a human performer using a software application.
        /// </summary>
        [EnumMember(Value = "User")]
        User,
        /// <summary>
        /// Represents that the task will be completed when the script is completed.
        /// </summary>
        [EnumMember(Value = "Script")]
        Script
    }
    /// <summary>
    /// It allows to control as well as merge and split the process flow in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Activity,
    ///         Activity = new BpmnActivity() { Activity = BpmnActivities.SubProcess, SubProcess = new BpmnSubProcess { Collapsed = false, Type = BpmnSubProcessTypes.Transaction, Processes = new DiagramObjectCollection<string>() { "new" } } },
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnSubProcessTypes
    {
        /// <summary>
        /// Represents none of the sub process are executed.
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Represents a composed activity that is included within a process.
        /// </summary>
        [EnumMember(Value = "Transaction")]
        Transaction,
        /// <summary>
        /// Represents the actions that are to be performed and expressed as circles in the diagram.
        /// </summary>
        [EnumMember(Value = "Event")]
        Event
    }
    /// <summary>
    /// Represents the type of the BPMN boundary.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     ID = "node1",          
    ///     Width = 70,
    ///     Height = 70, 
    ///     OffsetX = 100,
    ///     OffsetY = 300,
    ///     Shape = new BpmnShape() 
    ///     { 
    ///         Type = Shapes.Bpmn, 
    ///         Shape = BpmnShapes.Activity, 
    ///         Activity = new BpmnActivity() 
    ///         { 
    ///             Activity = BpmnActivities.SubProcess, 
    ///             SubProcess = new BpmnSubProcess { Collapsed = true, Boundary = BpmnBoundary.Default } 
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnBoundary
    {
        /// <summary>
        /// Sets the type of the boundary to Default that will be similar to the normal behaviour of the node.
        /// </summary>
        [EnumMember(Value = "Default")]
        Default,
        /// <summary>
        /// Sets the type of  boundary to Call when the activity of the process execution arrives.
        /// </summary>
        [EnumMember(Value = "Call")]
        Call,
        /// <summary>
        /// Sets the type of  boundary to Event on an activity boundary that can  be triggered.
        /// </summary>
        [EnumMember(Value = "Event")]
        Event
    }
    /// <summary>
    /// Represents the transferring of data into or out of an Activity in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Connector connector4 = new Connector() 
    /// {
    ///     ID = "connector4",
    ///     SourcePoint = new DiagramPoint() { X = 100, Y = 300 },
    ///     TargetPoint = new DiagramPoint() { X = 300, Y = 400 },
    ///     Type = ConnectorSegmentType.Straight,
    ///     Shape = new BpmnFlow() { Type = ConnectionShapes.Bpmn, Flow = BpmnFlows.Association, Association = BpmnAssociationFlows.Default }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnFlows
    {
        /// <summary>
        /// Represents precisely expressing the order of execution in the flow of diagram.
        /// </summary>
        [EnumMember(Value = "Sequence")]
        Sequence,
        /// <summary>
        /// Represents the direction to indicate read or write access in the diagram.
        /// </summary>
        [EnumMember(Value = "Association")]
        Association,
        /// <summary>
        /// Represents the communication that crosses the boundaries of your process in the diagram.
        /// </summary>
        [EnumMember(Value = "Message")]
        Message
    }
    /// <summary>
    /// Represents moving the data between the data objects, inputs, and outputs of the activities using nodes in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Connector connector4 = new Connector() 
    /// {
    ///     ID = "connector4",
    ///     SourcePoint = new DiagramPoint() { X = 100, Y = 300 },
    ///     TargetPoint = new DiagramPoint() { X = 300, Y = 400 },
    ///     Type = ConnectorSegmentType.Straight,
    ///     Shape = new BpmnFlow() { Type = ConnectionShapes.Bpmn, Flow = BpmnFlows.Association, Association = BpmnAssociationFlows.Default }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnAssociationFlows
    {
        /// <summary>
        /// Sets the type of Association flow to default  which  represents the next process in a flow or object.
        /// </summary>
        [EnumMember(Value = "Default")]
        Default,
        /// <summary>
        /// Sets the type of Association flow to Direction in which the data will be transferred to the targeted object.
        /// </summary>
        [EnumMember(Value = "Directional")]
        Directional,
        /// <summary>
        /// Sets the type of Association flow to BiDirectional in which the data will be transferred to both the source and target objects.
        /// </summary>
        [EnumMember(Value = "BiDirectional")]
        BiDirectional
    }
    /// <summary>
    /// Represents the flow of messages between separate nodes in the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Connector connector4 = new Connector() 
    /// {
    ///     ID = "connector4",
    ///     SourcePoint = new DiagramPoint() { X = 100, Y = 300 },
    ///     TargetPoint = new DiagramPoint() { X = 300, Y = 400 },
    ///     Type = ConnectorSegmentType.Straight,
    ///     Shape = new BpmnFlow() 
    ///     { 
    ///         Type = ConnectionShapes.Bpmn, 
    ///         Flow = BpmnFlows.Message, 
    ///         Message = BpmnMessageFlows.NonInitiatingMessage 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnMessageFlows
    {
        /// <summary>
        /// Represents a default connector line with a circular source decorator.
        /// </summary>
        [EnumMember(Value = "Default")]
        Default,
        /// <summary>
        /// Represents a default connector line with a circular source decorator unfilled with a message icon.
        /// </summary>
        [EnumMember(Value = "InitiatingMessage")]
        InitiatingMessage,
        /// <summary>
        /// Represents a default connector line with a circular source decorator filled with a message icon.
        /// </summary>
        [EnumMember(Value = "NonInitiatingMessage")]
        NonInitiatingMessage
    }
    /// <summary>
    /// Represents the type of the Bpmn Sequence flows
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Connector connector4 = new Connector() 
    /// {
    ///     ID = "connector4",
    ///     SourcePoint = new DiagramPoint() { X = 100, Y = 300 },
    ///     TargetPoint = new DiagramPoint() { X = 300, Y = 400 },
    ///     Type = ConnectorSegmentType.Straight,
    ///     Shape = new BpmnFlow() 
    ///     { 
    ///         Type = ConnectionShapes.Bpmn, 
    ///         Flow = BpmnFlows.Sequence, 
    ///         Sequence = BpmnSequenceFlows.Default 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal enum BpmnSequenceFlows
    {
        /// <summary>
        /// Represents the normal connector with any additional details in it.
        /// </summary>
        [EnumMember(Value = "Normal")]
        Normal,
        /// <summary>
        /// Represents the default association of a flow Node.
        /// </summary>
        [EnumMember(Value = "Default")]
        Default,
        /// <summary>
        /// Represents the conditional expression in the activity of it.
        /// </summary>
        [EnumMember(Value = "Conditional")]
        Conditional
    }
    /// <summary>
    /// Defines the connection shapes
    /// </summary>
    internal enum ConnectorShapeType
    {
        /// <summary>
        /// Sets the type of the connection shape as None
        /// </summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>
        /// Sets the type of the connection shape as Bpmn
        /// </summary>
        [EnumMember(Value = "Bpmn")]
        Bpmn,
        /// <summary>
        /// Sets the type of the connection shape as UMLActivity
        /// </summary>
        [EnumMember(Value = "UmlActivity")]
        UmlActivity,
        /// <summary>
        /// Sets the type of the connection shape as UMLClassifier
        /// </summary>
        [EnumMember(Value = "UmlClassifier")]
        UmlClassifier
    }
    /// <summary>
    /// Defines the Alignment position
    /// </summary>
    public enum BranchType
    {
        /// <summary>
        /// Sets the branch type as Left
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,
        /// <summary>
        /// Sets the branch type as Right
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
        /// <summary>
        /// Sets the branch type as SubLeft
        /// </summary>
        [EnumMember(Value = "SubLeft")]
        SubLeft,
        /// <summary>
        /// Sets the branch type as SubRight
        /// </summary>
        [EnumMember(Value = "SubRight")]
        SubRight,
        /// <summary>
        /// Sets the branch type as Root
        /// </summary>
        [EnumMember(Value = "Root")]
        Root
    }

    internal enum SymbolPaletteEvent
    {
        [EnumMember(Value = "SelectionChange")]
        SelectionChange,
        [EnumMember(Value = "NodeDefaults")]
        NodeDefaults,
        [EnumMember(Value = "ConnectorDefaults")]
        ConnectorDefaults,
        [EnumMember(Value = "OnExpanding")]
        OnExpanding
    }

    internal enum DiagramEvent
    {
        [EnumMember(Value = "SelectionChanging")]
        SelectionChanging,
        [EnumMember(Value = "SelectionChanged")]
        SelectionChanged,
        [EnumMember(Value = "PositionChanging")]
        PositionChanging,
        [EnumMember(Value = "PositionChanged")]
        PositionChanged,
        [EnumMember(Value = "ConnectionChanging")]
        ConnectionChanging,
        [EnumMember(Value = "ConnectionChanged")]
        ConnectionChanged,
        [EnumMember(Value = "SourcePointChanging")]
        SourcePointChanging,        
        [EnumMember(Value = "SourcePointChanged")]
        SourcePointChanged,
        [EnumMember(Value = "TargetPointChanging")]
        TargetPointChanging,
        [EnumMember(Value = "TargetPointChanged")]
        TargetPointChanged,
        [EnumMember(Value = "FixedUserHandleClick")]
        FixedUserHandleClick,
        [EnumMember(Value = "RotationChanging")]
        RotationChanging,
        [EnumMember(Value = "RotationChanged")]
        RotationChanged,
        [EnumMember(Value = "SizeChanging")]
        SizeChanging,        
        [EnumMember(Value = "SizeChanged")]
        SizeChanged,
        [EnumMember(Value = "HistoryChanged")]
        HistoryChanged,
        [EnumMember(Value = "HistoryAdding")]
        HistoryAdding,
        [EnumMember(Value = "Undo")]
        Undo,
        [EnumMember(Value = "Redo")]
        Redo,
        [EnumMember(Value = "DragDrop")]
        DragDrop,
        [EnumMember(Value = "Dragging")]
        Dragging,
        [EnumMember(Value = "DragStart")]
        DragStart,
        [EnumMember(Value = "DragLeave")]
        DragLeave,
        [EnumMember(Value = "KeyDown")]
        KeyDown,
        [EnumMember(Value = "KeyUp")]
        KeyUp,
        [EnumMember(Value = "CollectionChanging")]
        CollectionChanging,        
        [EnumMember(Value = "CollectionChanged")]
        CollectionChanged,
        [EnumMember(Value = "SegmentCollectionChange")]
        SegmentCollectionChange,
        [EnumMember(Value = "Click")]
        Click,
        [EnumMember(Value = "MouseEnter")]
        MouseEnter,
        [EnumMember(Value = "MouseHover")]
        MouseHover,
        [EnumMember(Value = "MouseLeave")]
        MouseLeave,
        [EnumMember(Value = "ScrollChanged")]
        ScrollChanged,
        [EnumMember(Value = "PropertyChanged")]
        PropertyChanged,
        [EnumMember(Value = "TextChanged")]
        TextChanged
    }
    /// <summary>
    /// Specifies a combination of key modifiers, on recognition of which the command will be executed.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <summary>
    /// Notifies when to execute the custom keyboard commands .
    /// </summary>
    /// <remarks>
    /// The following code illustrates how to create a custom command.
    /// </remarks>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent @ref="@diagram" Height="600px" Nodes="@nodes">
    /// @* Initializing the custom commands*@
    ///     <CommandManager Commands = "@command" Execute="@CommandExecute" CanExecute="@canexe">
    ///     </CommandManager>
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     // Reference to the diagram
    ///     SfDiagramComponent diagram;
    ///     DiagramObjectCollection<KeyboardCommand> command = new DiagramObjectCollection<KeyboardCommand>()
    ///     {
    ///         new KeyboardCommand()
    ///         {
    ///             Name = "CustomGroup",
    ///             Gesture = new KeyGesture() { Key = Keys.G, Modifiers = ModifierKeys.Control }
    ///         },
    ///         new KeyboardCommand()
    ///         {
    ///             Name = "CustomUnGroup",
    ///             Gesture = new KeyGesture() { Key = Keys.U, Modifiers = ModifierKeys.Control }
    ///         },
    ///     };
    ///     // Define the diagram's nodes collection
    ///     DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>();
    ///     public void canexe(CommandKeyArgs args)
    ///     {
    ///         args.CanExecute = true;
    ///     }
    ///     public void CommandExecute(CommandKeyArgs args)
    ///     {
    ///         if (args.Gesture.Modifiers == ModifierKeys.Control && args.Gesture.Key == Keys.G)
    ///         {
    ///             //Custom command to group the selected nodes
    ///             diagram.Group();
    ///         }
    ///         if (args.Gesture.Modifiers == ModifierKeys.Control && args.Gesture.Key == Keys.U)
    ///         {
    ///             DiagramSelectionSettings selector = diagram.SelectionSettings;
    ///             //Custom command to ungroup the selected items
    ///             if (selector.Nodes.Count > 0 && selector.Nodes[0] is NodeGroup)
    ///             {
    ///                 if ((selector.Nodes[0] as NodeGroup).Children.Length > 0)
    ///                 {
    ///                     diagram.UnGroup();
    ///                 }
    ///             }
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [Flags]
    public enum ModifierKeys
    {
        /// <summary>
        /// Specifies when no modifiers are pressed.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies the Ctrl key as a key modifier.
        /// </summary>
        Control = 1 << 0,
        /// <summary>
        /// Specifies meta key in mac.
        /// </summary>
        Meta = 1 << 3,
        /// <summary>
        /// Specifies the alt key as a key modifier.
        /// </summary>
        Alt = 1 << 1,
        /// <summary>
        /// Specifies the shift key as a key modifier.
        /// </summary>
        Shift = 1 << 2,
    }
    ///<summary>
    /// Represents the class for arranging the nodes and connectors in a tree structure.
    /// </summary>
    public enum Keys
    {
        /// <summary>
        /// Sets the key value as null when none keys is pressed.
        /// </summary>
        None,
        /// <summary>
        /// Sets the key value as 0 when key 0 is pressed.
        /// </summary>
        Number0 = 96,
        /// <summary>
        /// Sets the key value as 1 when key 1 is pressed.
        /// </summary>
        Number1 = 97,
        /// <summary>
        /// Sets the key value as 2 when key 2 is pressed.
        /// </summary>
        Number2 = 98,
        /// <summary>
        /// Sets the key value as 3 when key 3 is pressed.
        /// </summary>
        Number3 = 99,
        /// <summary>
        /// Sets the key value as 4 when key 4 is pressed.
        /// </summary>
        Number4 = 100,
        /// <summary>
        /// Sets the key value as 5 when key 5 is pressed.
        /// </summary>
        Number5 = 101,
        /// <summary>
        /// Sets the key value as 6 when key 6 is pressed.
        /// </summary>
        Number6 = 102,
        /// <summary>
        /// Sets the key value as 7 when key 7 is pressed.
        /// </summary>
        Number7 = 103,
        /// <summary>
        /// Sets the key value as 8 when key 8 is pressed.
        /// </summary>
        Number8 = 104,
        ///<summary>
        /// Sets the key value as 9 when key 9 is pressed.
        /// </summary>
        Number9 = 105,
        /// <summary>
        /// Sets the key value as A when key A is pressed.
        /// </summary>
        A = 65,
        /// <summary>
        /// Sets the key value as B when key B is pressed.
        /// </summary>
        B = 66,
        ///<summary>
        /// Sets the key value as C when key C is pressed.
        /// </summary>
        C = 67,
        ///<summary>
        /// Sets the key value as D when key D is pressed.
        /// </summary>
        D = 68,
        /// <summary>
        /// Sets the key value as E when key E is pressed.
        /// </summary>
        E = 69,
        /// <summary>
        /// Sets the key value as F when key F is pressed.
        /// </summary>
        F = 70,
        /// <summary>
        /// Sets the key value as G when key G is pressed.
        /// </summary>
        G = 71,
        /// <summary>
        /// Sets the key value as H when key H is pressed.
        /// </summary>
        H = 72,
        /// <summary>
        /// Sets the key value as I when key I is pressed.
        /// </summary>
        I = 73,
        /// <summary>
        /// Sets the key value as J when key J is pressed.
        /// </summary>
        J = 74,
        /// <summary>
        /// Sets the key value as K when key K is pressed.
        /// </summary>
        K = 75,
        /// <summary>
        /// Sets the key value as L when key L is pressed.
        /// </summary>
        L = 76,
        /// <summary>
        /// Sets the key value as M when key M is pressed.
        /// </summary>
        M = 77,
        /// <summary>
        /// Sets the key value as N when key N is pressed.
        /// </summary>
        N = 78,
        /// <summary>
        /// Sets the key value as O when key O is pressed.
        /// </summary>
        O = 79,
        /// <summary>
        /// Sets the key value as P when key P is pressed.
        /// </summary>
        P = 80,
        /// <summary>
        /// Sets the key value as Q when key Q is pressed.
        /// </summary>
        Q = 81,
        /// <summary>
        /// Sets the key value as R when key R is pressed.
        /// </summary>
        R = 82,
        /// <summary>
        /// Sets the key value as S when key S is pressed.
        /// </summary>
        S = 83,
        /// <summary>
        /// Sets the key value as T when key T is pressed.
        /// </summary>
        T = 84,
        /// <summary>
        /// Sets the key value as U when key U is pressed.
        /// </summary>
        U = 85,
        /// <summary>
        /// Sets the key value as V when key V is pressed.
        /// </summary>
        V = 86,
        /// <summary>
        /// Sets the key value as W when key W is pressed.
        /// </summary>
        W = 87,
        /// <summary>
        /// Sets the key value as X when key X is pressed.
        /// </summary>
        X = 88,
        /// <summary>
        /// Sets the key value as Y when key Y is pressed.
        /// </summary>
        Y = 89,
        /// <summary>
        /// Sets the key value as Z when key Z is pressed.
        /// </summary>
        Z = 90,
        /// <summary>
        /// Sets the key value as left when left arrow key is pressed.
        /// </summary>
        ArrowLeft = 37,
        /// <summary>
        /// Sets the key value as up when up arrow key is pressed.
        /// </summary>
        ArrowUp = 38,
        /// <summary>
        /// Sets the key value as right when right arrow key is pressed.
        /// </summary>
        ArrowRight = 39,
        /// <summary>
        /// Sets the key value as down when left down key is pressed.
        /// </summary>
        ArrowDown = 40,
        /// <summary>
        /// Sets the key value as Escape when escape key is pressed.
        /// </summary>
        Escape = 27,
        /// <summary>
        /// Sets the key value as Space when space key is pressed.
        /// </summary> 
        Space = 32,
        /// <summary>
        /// Sets the key value as PageUp when page up key is pressed.
        /// </summary>
        PageUp = 33,
        /// <summary>
        ///  Sets the key value as PageDown when page down key is pressed.
        /// </summary>
        PageDown = 34,
        /// <summary>
        ///  Sets the key value as End when end key is pressed.
        /// </summary>
        End = 35,
        /// <summary>
        /// Sets the key value as Home when home key is pressed.
        /// </summary>
        Home = 36,
        /// <summary>
        /// Sets the key value as Delete when delete key is pressed.
        /// </summary>
        Delete = 46,
        /// <summary>
        ///  Sets the key value as Tab when tab key is pressed.
        /// </summary>
        Tab = 9,
        /// <summary>
        /// Sets the key value as enter when enter key is pressed.
        /// </summary>
        Enter = 13,
        /// <summary>
        /// Sets the key value as BackSpace when BackSpace key is pressed.
        /// </summary>
        BackSpace = 8,
        /// <summary>
        /// Sets the key value as F1 when F1 key is pressed.
        /// </summary>
        F1 = 112,
        /// <summary>
        /// Sets the key value as F2 when F2 key is pressed.
        /// </summary>
        F2 = 113,
        /// <summary>
        /// Sets the key value as F3 when F3 key is pressed.
        /// </summary>
        F3 = 114,
        /// <summary>
        /// Sets the key value as F4 when F4 key is pressed.
        /// </summary>
        F4 = 115,
        /// <summary>
        /// Sets the key value as F5 when F5 key is pressed.
        /// </summary>
        F5 = 116,
        /// <summary>
        /// Sets the key value as F6 when F6 key is pressed.
        /// </summary>
        F6 = 117,
        /// <summary>
        /// Sets the key value as F7 when F7 key is pressed.
        /// </summary>
        F7 = 118,
        /// <summary>
        /// Sets the key value as F8 when F8 key is pressed.
        /// </summary>
        F8 = 119,
        /// <summary>
        /// Sets the key value as F9 when F9 key is pressed.
        /// </summary>
        F9 = 120,
        ///<summary>
        /// Sets the key value as F10 when F10 key is pressed.
        /// </summary>
        F10 = 121,
        ///<summary>
        /// Sets the key value as F11 when F11 key is pressed.
        /// </summary>
        F11 = 122,
        ///<summary>
        /// Sets the key value as F12 when F12 key is pressed.
        /// </summary>
        F12 = 123,
        ///<summary>
        /// Sets the key value as Star when star key is pressed.
        /// </summary>
        Star = 56,
        ///<summary>
        /// Sets the key value as Plus when plus key is pressed.
        /// </summary>
        Plus = 187,
        ///<summary>
        /// Sets the key value as Minus when minus key is pressed.
        /// </summary>
        Minus = 189
    }
    /// <summary>
    /// Enables or disables certain features of snapping. 
    /// </summary>
    [Flags]
    public enum SnapConstraints
    {
        /// <summary>
        /// Disables all the functionalities of snapping.
        /// </summary>
        None = 0,
        /// <summary>
        /// Displays only the horizontal gridlines in the diagram.
        /// </summary>
        ShowHorizontalLines = 1 << 0,
        /// <summary>
        /// Displays only the vertical gridlines in the diagram.
        /// </summary>
        ShowVerticalLines = 1 << 1,
        /// <summary>
        /// Display both the horizontal and the vertical gridlines.
        /// </summary>
        ShowLines = 1 | 2,
        /// <summary>
        /// Enables the object to snap only with horizontal gridlines.
        /// </summary>
        SnapToHorizontalLines = 1 << 2,
        /// <summary>
        /// Enables the object to snap only with vertical gridlines.
        /// </summary>
        SnapToVerticalLines = 1 << 3,
        /// <summary>
        /// Enables the object to snap with both the horizontal and vertical gridlines.
        /// </summary>
        SnapToLines = 4 | 8,
        /// <summary>
        /// Enables the object to snap with other objects in the diagram.
        /// </summary>
        SnapToObject = 1 << 4,
        /// <summary>
        /// Enables all the functionalities of snapping.
        /// </summary>
        All = 1 | 2 | 4 | 8 | 16
    }
    internal enum SnapAlignment
    {
        Left,
        Right,
        Top,
        Bottom,
        TopBottom,
        BottomTop,
        LeftRight,
        RightLeft,
        CenterX,
        CenterY
    }

    ///<summary>
    /// Defines the entry type
    /// </summary>
    public enum HistoryEntryType
    {
        ///<summary>
        ///PositionChanged - Sets the entry type as PositionChanged 
        /// </summary>
        [EnumMember(Value = "PositionChanged")]
        PositionChanged,
        ///<summary>
        ///ConnectionChanged - Sets the entry type as ConnectionChanged 
        /// </summary>
        [EnumMember(Value = "ConnectionChanged")]
        ConnectionChanged,
        ///<summary>
        ///StartGroup - Sets the entry type as StartGroup
        /// </summary>
        [EnumMember(Value = "StartGroup")]
        StartGroup,
        ///<summary>
        ///EndGroup - Sets the entry type as EndGroup
        /// </summary>
        [EnumMember(Value = "EndGroup")]
        EndGroup,
        ///<summary>
        ///RotationChanged - Sets the entry type as RotationChanged
        /// </summary>
        [EnumMember(Value = "RotationChanged")]
        RotationChanged,
        ///<summary>
        ///PropertyChanged - Sets the entry type as PropertyChanged
        /// </summary>
        [EnumMember(Value = "PropertyChanged")]
        PropertyChanged,
        ///<summary>
        ///CollectionChanged - Sets the entry type as CollectionChanged
        /// </summary>
        [EnumMember(Value = "CollectionChanged")]
        CollectionChanged,
        ///<summary>
        ///LabelCollectionChanged - Sets the entry type as LabelCollectionChanged
        /// </summary>
        [EnumMember(Value = "LabelCollectionChanged")]
        LabelCollectionChanged,
        ///<summary>
        ///PortCollectionChanged - Sets the entry type as PortCollectionChanged
        /// </summary>
        [EnumMember(Value = "PortCollectionChanged")]
        PortCollectionChanged,
        ///<summary>
        ///Group - Sets the entry type as Group
        /// </summary>
        [EnumMember(Value = "Group")]
        Group,
        ///<summary>
        ///UnGroup - Sets the entry type as UnGroup
        /// </summary>
        [EnumMember(Value = "UnGroup")]
        UnGroup,
        ///<summary>
        ///SegmentChanged - Sets the entry type as SegmentChanged
        /// </summary>
        [EnumMember(Value = "SegmentChanged")]
        SegmentChanged,
        ///<summary>
        ///AnnotationPropertyChanged - Sets the entry type as AnnotationPropertyChanged
        /// </summary>
        [EnumMember(Value = "AnnotationPropertyChanged")]
        AnnotationPropertyChanged,
        ///<summary>
        ///Undo - Sets the entry type as Undo
        /// </summary>
        [EnumMember(Value = "Undo")]
        Undo,
        ///<summary>
        ///Redo - Sets the entry type as Redo
        /// </summary>
        [EnumMember(Value = "Redo")]
        Redo,
        ///<summary>
        ///SizeChanged - Sets the entry type as SizeChanged
        /// </summary>
        [EnumMember(Value = "SizeChanged")]
        SizeChanged,
    }

    /// <summary>
    /// Defines the kind of entry category from which the history will be added or modified by the user or internally.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// private void onCustomentry()
    /// {
    ///     Node cloneobject = node1.Clone() as Node;
    ///     cloneobject.AddInfo = new Dictionary<string, object>();
    ///     cloneobject.AddInfo.Add(cloneobject.ID, "Description");
    ///     HistoryEntry entry1 = new HistoryEntry();
    ///     entry1.ChangeType = HistoryEntryChangeType.Insert;
    ///     entry1.Category = EntryCategory.ExternalEntry;
    ///     entry1.UndoObject = (cloneobject) as Node;
    ///     diagram.HistoryManager.Push(entry1);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    internal enum EntryCategory
    {
        /// <summary>
        /// Sets the entry category type as Internal when it`s added internally.
        /// </summary>       
        InternalEntry,
        /// <summary>
        /// Sets the entry category type as External when it`s added by the user.
        /// </summary>   
        ExternalEntry
    }

    /// <summary>
    /// Specifies the state of history actions such as undo and redo.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" HistoryChanged="@OnHistoryChanged">
    /// </SfDiagramComponent>
    /// private void OnHistoryChanged(HistoryChangedEventArgs arg)
    /// {
    ///     if (arg.Entry != null)
    ///     {
    ///         HistoryChangedAction historyChangeArgs = arg.ActionTrigger;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum HistoryChangedAction
    {
        /// <summary>
        /// Defines the history of action as custom action.
        /// </summary>
        CustomAction,
        /// <summary>
        /// Sets the history action as Undo when performing an undo action.
        /// </summary>
        Undo,
        /// <summary>
        /// Sets the history action as Redo when performing the redo action.
        /// </summary>
        Redo
    }

    /// <summary>
    /// Defines the value that specifies the buttons on a mouse device.
    /// </summary>
    public enum MouseButtons
    {
        /// <summary>
        /// Represents the left mouse button.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,
        /// <summary>
        /// Represents the middle mouse button
        /// </summary>
        [EnumMember(Value = "Middle")]
        Middle,
        /// <summary>
        /// Represents the right mouse button.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
    }
    /// <summary>
    /// Defines the change type from which the history will be entered.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" HistoryChanged="@OnHistoryChanged">
    /// </SfDiagramComponent>
    /// private void OnHistoryChanged(HistoryChangedEventArgs arg)
    /// {
    ///     if (arg.Entry != null)
    ///     {
    ///         HistoryEntryChangeType historyEntryChangeType = arg.CollectionChangedAction;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public enum HistoryEntryChangeType
    {
        /// <summary>
        /// Represents none of the history entries to insert/remove.
        /// </summary>
        None,
        /// <summary>
        /// Represents the history inserted into the entry.
        /// </summary>
        Insert,
        /// <summary>
        /// Represents the history  removed from the entry.
        /// </summary>
        Remove
    }
}
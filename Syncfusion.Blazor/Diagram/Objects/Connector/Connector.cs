using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Tests")]
namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// A line which represents the relationship between two points, nodes, or ports. 
    /// </summary>
    public class Connector : NodeBase
    {
        private ConnectorConstraints constraints = ConnectorConstraints.Default;
        private DiagramPoint sourcePoint = new DiagramPoint() { X = 0, Y = 0 };
        private DiagramPoint targetPoint = new DiagramPoint() { X = 0, Y = 0 };
        private string sourceId = string.Empty;
        private BpmnFlow shape = new BpmnFlow();
        private string targetId = string.Empty;
        private string sourcePortId = string.Empty;
        private string targetPortId = string.Empty;
        private double cornerRadius;
        private double hitPadding = 10;
        private double sourcePadding;
        private double targetPadding;
        private double connectionPadding;
        private double bridgeSpace = 10;
        private ConnectorSegmentType type = ConnectorSegmentType.Straight;
        [JsonIgnore]
        internal DiagramObjectCollection<ConnectorSegment> SegmentCollection { get; set; } = new DiagramObjectCollection<ConnectorSegment>();
        private ShapeStyle style = new ShapeStyle() { StrokeWidth = 1, StrokeColor = "black" };
        private DecoratorSettings sourceDecorator = new DecoratorSettings() { Shape = DecoratorShape.None };
        private DecoratorSettings targetDecorator = new DecoratorSettings() { Shape = DecoratorShape.Arrow };
        private DiagramObjectCollection<PathAnnotation> annotations = new DiagramObjectCollection<PathAnnotation>();
        private DiagramObjectCollection<ConnectorFixedUserHandle> fixedUserHandles = new DiagramObjectCollection<ConnectorFixedUserHandle>();
        internal bool Visited;
        [JsonIgnore]
        internal List<Bridge> Bridges { get; set; } = new List<Bridge>();
        [JsonIgnore]
        internal DiagramElement SourceWrapper { get; set; }
        [JsonIgnore]
        internal DiagramElement TargetWrapper { get; set; }
        [JsonIgnore]
        internal DiagramElement SourcePortWrapper { get; set; }
        [JsonIgnore]
        internal DiagramElement TargetPortWrapper { get; set; }
        [JsonIgnore]
        internal List<DiagramPoint> IntermediatePoints { get; set; }
        [JsonIgnore]
        internal DiagramRect Bounds { get; set; }
        /// <summary>
        /// Creates a new instance of the Connector from the given Connector.
        /// </summary>
        /// <param name="src">Connector</param>
        public Connector(Connector src) : base(src)
        {
            if (src != null)
            {
                sourcePadding = src.sourcePadding;
                constraints = src.constraints;
                sourceId = src.sourceId;
                targetId = src.targetId;
                sourcePortId = src.sourcePortId;
                targetPortId = src.targetPortId;
                cornerRadius = src.cornerRadius;
                hitPadding = src.hitPadding;
                sourcePadding = src.sourcePadding;
                targetPadding = src.targetPadding;
                connectionPadding = src.connectionPadding;
                bridgeSpace = src.bridgeSpace;
                type = src.type;
                if (src.shape != null)
                {
                    shape = src.shape.Clone() as BpmnFlow;
                    shape.Parent = this;
                }
                if (src.sourcePoint != null)
                {
                    sourcePoint = src.sourcePoint.Clone() as DiagramPoint;
                    sourcePoint.Parent = this;
                }
                if (src.targetPoint != null)
                {
                    targetPoint = src.targetPoint.Clone() as DiagramPoint;
                    targetPoint.Parent = this;
                }
                if (src.style != null)
                {
                    style = src.style.Clone() as ShapeStyle;
                    style.Parent = this;
                }
                if (src.sourceDecorator != null)
                {
                    sourceDecorator = src.sourceDecorator.Clone() as DecoratorSettings;
                    sourceDecorator.Parent = this;
                }
                if (src.targetDecorator != null)
                {
                    targetDecorator = src.targetDecorator.Clone() as DecoratorSettings;
                    targetDecorator.Parent = this;
                }
                SegmentCollection = new DiagramObjectCollection<ConnectorSegment>();
                if (src.SegmentCollection.Count > 0)
                {
                    foreach (ConnectorSegment segment in src.SegmentCollection)
                    {
                        ConnectorSegment connectorSegment = segment.Clone() as ConnectorSegment;
                        SegmentCollection.Add(connectorSegment);
                    }
                    SegmentCollection.Parent = this;
                }
                annotations = new DiagramObjectCollection<PathAnnotation>();
                if (src.annotations.Count > 0)
                {
                    foreach (PathAnnotation annotation in src.annotations)
                    {
                        PathAnnotation pathAnnotation = annotation.Clone() as PathAnnotation;
                        annotations.Add(pathAnnotation);
                    }
                    annotations.Parent = this;
                }
                fixedUserHandles = new DiagramObjectCollection<ConnectorFixedUserHandle>();
                if (src.fixedUserHandles.Count > 0)
                {
                    foreach (ConnectorFixedUserHandle fixedUserHandle in src.fixedUserHandles)
                    {
                        ConnectorFixedUserHandle userHandle = fixedUserHandle.Clone() as ConnectorFixedUserHandle;
                        fixedUserHandles.Add(userHandle);
                    }
                    fixedUserHandles.Parent = this;
                }
                Visited = src.Visited;
            }
        }
        /// <summary>
        /// Initializes a new instance of the Connector.
        /// </summary>
        public Connector() : base()
        {
            annotations.Parent = this;
            fixedUserHandles.Parent = this;
            SegmentCollection.Parent = this;
        }
        /// <summary>
        /// Enables or disables certain features of the connector. By default, all the interactive functionalities are enabled. 
        /// </summary>
        /// <remarks>
        /// Features like dragging, selection can be disabled. Refer <see href="https://blazor.syncfusion.com/documentation/diagram-component/constraints#connector-constraints"> ConnectorConstraints </see> for better understanding.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Connector connector = new Connector()
        /// {
        ///     ID = "connector1",
        ///     Type = ConnectorSegmentType.Straight,
        ///     SourcePoint = new DiagramPoint() { X = 100, Y = 100 },
        ///     TargetPoint = new DiagramPoint() { X = 200, Y = 200 },
        ///     //set the ConnectorConstraints...
        ///     Constraints = ConnectorConstraints.Default & ~ConnectorConstraints.Select
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("constraints")]
        public ConnectorConstraints Constraints
        {
            get => constraints;
            set
            {
                if (constraints != value)
                {
                    Parent?.OnPropertyChanged(nameof(Constraints), value, constraints, this);
                    constraints = value;
                    (Parent as SfDiagramComponent)?.DiagramContent.UpdateBridging();
                }
            }
        }

        /// <summary>
        /// Gets or sets the beginning point of the connector. 
        /// </summary>
        [JsonPropertyName("sourcePoint")]
        public DiagramPoint SourcePoint
        {
            get
            {
                if (sourcePoint != null && sourcePoint.Parent == null)
                    sourcePoint.SetParent(this, nameof(SourcePoint));
                return sourcePoint;
            }
            set
            {
                if (sourcePoint != value)
                {
                    if (Parent != null && this.Wrapper != null)
                        Parent.OnPropertyChanged(nameof(SourcePoint), value, sourcePoint, this);
                    sourcePoint = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the endpoint of the connector.
        /// </summary>
        [JsonPropertyName("targetPoint")]
        public DiagramPoint TargetPoint
        {
            get
            {
                if (targetPoint != null && targetPoint.Parent == null)
                    targetPoint.SetParent(this, nameof(TargetPoint));
                return targetPoint;
            }
            set
            {
                if (targetPoint != value)
                {
                    if (Parent != null && this.Wrapper != null)
                        Parent.OnPropertyChanged(nameof(TargetPoint), value, targetPoint, this);
                    targetPoint = value;
                }
            }
        }
        /// <summary>
        /// Represents the shape of the connector. It is applicable only if it is a Bpmn connector.
        /// </summary>
        /// <remarks>
        /// For more information, refer to <see href="https://blazor.syncfusion.com/documentation/diagram-component/bpmn-shapes/bpmn-connectors">Bpmn Connector</see>. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Connector connector = new Connector()
        /// {
        ///     // Unique Id of the connector
        ///     ID = "connector1",
        ///     // Start and end points of the connector
        ///     SourcePoint = new DiagramPoint() { X = 100, Y = 200 },
        ///     TargetPoint = new DiagramPoint() { X = 300, Y = 200 },
        ///     // Set the type to Bpmn, flow to Association and association to bidirectional
        ///     Shape = new BpmnFlow()
        ///     {
        ///         Type = ConnectionShapes.Bpmn,
        ///         Flow = BpmnFlows.Association,
        ///         Association = BpmnAssociationFlows.BiDirectional
        ///     }
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("shape")]
        internal BpmnFlow Shape
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
                    //Parent = this.Parent;
                    Parent?.OnPropertyChanged(nameof(Shape), value, shape, this);
                    shape = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the collection of the fixed user handle of the connector.
        /// </summary>
        /// <remarks>
        /// The fixed user handles are used to add some frequently used commands around the node and connector. It is visible even without selecting the nodes or connectors. 
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
        [JsonPropertyName("fixedUserHandles")]
        public DiagramObjectCollection<ConnectorFixedUserHandle> FixedUserHandles
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
        /// <summary>
        /// Gets or sets the unique id of the source node of the connector. 
        /// </summary>
        [JsonPropertyName("sourceID")]
        public string SourceID
        {
            get => sourceId;
            set
            {
                if (sourceId != value)
                {
                    Parent?.OnPropertyChanged(nameof(SourceID), value, sourceId, this);
                    sourceId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unique id of the target node of the connector. 
        /// </summary>
        [JsonPropertyName("targetID")]
        public string TargetID
        {
            get => targetId;
            set
            {
                if (targetId != value)
                {
                    Parent?.OnPropertyChanged(nameof(TargetID), value, targetId, this);
                    targetId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unique id of the source port of the connector. 
        /// </summary>
        [JsonPropertyName("sourcePortID")]
        public string SourcePortID
        {
            get => sourcePortId;
            set
            {
                if (sourcePortId != value)
                {
                    Parent?.OnPropertyChanged(nameof(SourcePortID), value, sourcePortId, this);
                    sourcePortId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the unique id of the target port of the connector. 
        /// </summary>
        [JsonPropertyName("targetPortID")]
        public string TargetPortID
        {
            get => targetPortId;
            set
            {
                if (targetPortId != value)
                {
                    Parent?.OnPropertyChanged(nameof(TargetPortID), value, targetPortId, this);
                    targetPortId = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the corner radius of the connector. By default, it is 0. 
        /// </summary>
        [JsonPropertyName("cornerRadius")]
        public double CornerRadius
        {
            get => cornerRadius;
            set
            {
                if (!cornerRadius.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(CornerRadius), value, cornerRadius, this);
                    cornerRadius = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the hit padding value of the connector. Hit padding value represents the range of the connector selection. By default, it is 10px. 
        /// </summary>
        [JsonPropertyName("hitPadding")]
        public double HitPadding
        {
            get => hitPadding;
            set
            {
                if (!hitPadding.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(HitPadding), value, hitPadding, this);
                    hitPadding = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the space to be left between the source node and the source point of a connector. By default, it is 0. 
        /// </summary>
        [JsonPropertyName("sourcePadding")]
        public double SourcePadding
        {
            get => sourcePadding;
            set
            {
                if (!sourcePadding.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(SourcePadding), value, sourcePadding, this);
                    sourcePadding = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the space to be left between the target node and the target point of a connector. By default, it is 0. 
        /// </summary>
        [JsonPropertyName("targetPadding")]
        public double TargetPadding
        {
            get => targetPadding;
            set
            {
                if (!targetPadding.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(TargetPadding), value, targetPadding, this);
                    targetPadding = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the connection padding value of the connector. By default, it is 0. 
        /// </summary>
        /// <remarks>
        /// The connector connects with the node or port while dragging its source/target thumb towards the padding region. 
        /// </remarks>
        [JsonPropertyName("connectionPadding")]
        public double ConnectionPadding
        {
            get => connectionPadding;
            set
            {
                if (!connectionPadding.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(ConnectionPadding), value, connectionPadding, this);
                    connectionPadding = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the line bridges. By default, the width is 10px. 
        /// </summary>
        [JsonPropertyName("bridgeSpace")]
        public double BridgeSpace
        {
            get => bridgeSpace;
            set
            {
                if (!bridgeSpace.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(BridgeSpace), value, bridgeSpace, this);
                    bridgeSpace = value;
                }
            }
        }

        /// <summary>
        /// Represents the type of the connector. By default, it is straight.
        /// </summary>
        [JsonPropertyName("type")]
        public ConnectorSegmentType Type
        {
            get => type;
            set
            {
                if (!Equals(type, value))
                {
                    Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                    type = value;
                }
            }
        }

        /// <summary>
        /// Represents the collection of connector segments. 
        /// </summary>
        [JsonPropertyName("segments")]
        public DiagramObjectCollection<ConnectorSegment> Segments
        {
            get => SegmentCollection;
            set
            {
                if (value != null && SegmentCollection != value)
                {
                    SegmentCollection?.ClearItemElements(SegmentCollection);
                    SegmentCollection = value;
                    SegmentCollection.Parent = this;
                    if (Parent != null && Wrapper != null)
                    {
                        Parent.OnPropertyChanged(nameof(Segments), value, SegmentCollection, this);
                    }
                }
                else
                    SegmentCollection = value;
            }
        }
        /// <summary>
        /// Represents the collection of textual information contained in the connector.
        /// </summary>
        /// <remarks>
        /// The text found in the connector can be edited at runtime. Users can modify the   Style, Visibility, Width, Height, and content of the annotation. 
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Connector connector = new Connector()
        /// {        
        ///     SourcePoint = new DiagramPoint() { X = 300, Y = 40 },
        ///     TargetPoint = new DiagramPoint() { X = 400, Y = 160 },
        ///     Type = ConnectorSegmentType.Orthogonal,
        ///     Style = new TextStyle() { StrokeColor = "#6495ED" },
        ///     // Initialize the annotation collection
        ///     Annotations = new DiagramObjectCollection<PathAnnotation>() 
        ///     { 
        ///         new PathAnnotation { Content = "Annotation" }
        ///     },
        /// };
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("annotations")]
        public DiagramObjectCollection<PathAnnotation> Annotations
        {
            get
            {
                if (annotations != null && annotations.Parent == null)
                    annotations.Parent = this;
                return annotations;
            }
            set
            {
                if (value != null && annotations != null && annotations != value)
                {
                    SfDiagramComponent diagram = ((NodeBase)annotations.Parent).Parent as SfDiagramComponent;
                    if (diagram != null)
                    {
                        if (diagram.IsRendered)
                        {
                            diagram.StartGroupAction();
                        }
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
        /// Represents the appearance of the connector. 
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
        /// Gets or sets the source decorator (sourcePoint's shape) of the connector. By default, its shape is none.
        /// </summary>
        [JsonPropertyName("sourceDecorator")]
        public DecoratorSettings SourceDecorator
        {
            get
            {
                if (sourceDecorator != null && sourceDecorator.Parent == null)
                    sourceDecorator.SetParent(this, nameof(SourceDecorator));
                return sourceDecorator;
            }
            set
            {
                if (sourceDecorator != value)
                {
                    Parent?.OnPropertyChanged(nameof(SourceDecorator), value, sourceDecorator, this);
                    sourceDecorator = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the target decorator (target points shape) of the connector. By default, its shape is an arrow. 
        /// </summary>
        [JsonPropertyName("targetDecorator")]
        public DecoratorSettings TargetDecorator
        {
            get
            {
                if (targetDecorator != null && targetDecorator.Parent == null)
                    targetDecorator.SetParent(this, nameof(TargetDecorator));
                return targetDecorator;
            }
            set
            {
                if (targetDecorator != value)
                {
                    Parent?.OnPropertyChanged(nameof(TargetDecorator), value, targetDecorator, this);
                    targetDecorator = value;
                }
            }
        }

        internal Canvas Init()
        {
            Canvas container = new Canvas(); PathElement segment = new PathElement();
            DiagramRect bounds;
            ID = !string.IsNullOrEmpty(ID) ? ID : BaseUtil.RandomId();
            segment.ID = ID + "_path";
            segment = GetSegmentElement(this, segment);
            List<DiagramPoint> points = GetConnectorPoints();
            points = ClipDecorators(this, points);
            Bounds = bounds = DiagramRect.ToBounds(points);
            container.Width = bounds.Width;
            container.Height = bounds.Height;
            container.OffsetX = bounds.X + container.Pivot.X * bounds.Width;
            container.OffsetY = bounds.Y + container.Pivot.Y * bounds.Height;
            PathElement bpmnElement = null;
            switch (Shape.Type)
            {
                case ConnectorShapeType.Bpmn:
                    bpmnElement = new PathElement();
                    BpmnFlows flow = shape.Flow;
                    switch (flow)
                    {
                        case BpmnFlows.Sequence:
                            bpmnElement = GetBpmnSequenceFlow();
                            break;
                        case BpmnFlows.Association:
                            bpmnElement.Visible = false;
                            this.GetBpmnAssociationFlow();
                            break;
                        case BpmnFlows.Message:
                            bpmnElement = GetBpmnMessageFlow();
                            segment = GetSegmentElement(this, segment);
                            this.UpdateShapePosition(bpmnElement);
                            break;
                    }
                    break;

            }
            List<DiagramPoint> anglePoints = IntermediatePoints;
            if (type == Diagram.ConnectorSegmentType.Bezier)
            {
                BezierSegment firstSegment = Segments[0] as BezierSegment;
                BezierSegment lastSegment = Segments[^1] as BezierSegment;
                anglePoints = new List<DiagramPoint>() {
                    lastSegment != null && !DiagramPoint.IsEmptyPoint(lastSegment.Point2) ? lastSegment.Point2: lastSegment?.BezierPoint2,
                    firstSegment != null && !DiagramPoint.IsEmptyPoint(firstSegment.Point1) ? firstSegment.Point1: firstSegment?.BezierPoint1
                };
            }
            PathElement srcDecorator = GetDecoratorElement(
                points[0], anglePoints[1], sourceDecorator);
            PathElement tDecorator = GetDecoratorElement(
                points[^1], anglePoints[^2], this.targetDecorator);
            srcDecorator.ID = ID + "_srcDec";
            tDecorator.ID = ID + "_tarDec";
            segment.Style = style;
            segment.Style.Fill = "transparent";
            container.Style.StrokeColor = "transparent";
            container.Style.Fill = "transparent";
            container.Style.StrokeWidth = 0;
            container.Children = new ObservableCollection<ICommonElement>() { segment, srcDecorator, tDecorator };
            container.ID = ID;
            if (bpmnElement != null && Shape.Type == ConnectorShapeType.Bpmn)
            {
                container.Children.Add(bpmnElement);
            }
            container.OffsetX = segment.OffsetX;
            container.OffsetY = segment.OffsetY;
            container.Width = segment.Width;
            container.Height = segment.Height;
            foreach (ConnectorFixedUserHandle connectorFixedUserHandle in this.FixedUserHandles)
            {
                DiagramElement handle = this.GetFixedUserHandle(connectorFixedUserHandle, this.IntermediatePoints, Bounds);
                container.Children.Add(handle);
            }
            Wrapper = container;
            return container;
        }
        internal DiagramPoint Scale(double sw, double sh, double width, double height)
        {
            double tx = 0;
            double ty = 0;

            if (this.Wrapper != null)
            {
                DiagramRect outerBounds = ConnectorUtil.GetOuterBounds(this);
                double connWidth = (this.Wrapper.Bounds.Width) - 2;
                double connHeight = (this.Wrapper.Bounds.Height) - 2;
                tx = (outerBounds.Width - connWidth);
                ty = (outerBounds.Height - connHeight);
                sw = (width - (Math.Max(tx, ty))) / connWidth;
                sh = (height - (Math.Max(tx, ty))) / connHeight;
                tx = ty = Math.Min(tx, ty);
            }

            sw = sh = Math.Min(sw, sh);
            Matrix matrix = Matrix.IdentityMatrix();
            DiagramElement refObject = this.Wrapper;
            if (refObject != null)
            {
                Matrix.RotateMatrix(matrix, -refObject.RotationAngle, refObject.OffsetX, refObject.OffsetY);
                Matrix.ScaleMatrix(matrix, sw, sh, refObject.OffsetX, refObject.OffsetY);
                Matrix.RotateMatrix(matrix, refObject.RotationAngle, refObject.OffsetX, refObject.OffsetY);
            }

            List<DiagramPoint> points = this.IntermediatePoints;
            List<DiagramPoint> transformedPoints = new List<DiagramPoint>();

            for (int i = 0; i < points.Count; i++)
            {
                transformedPoints.Add(Matrix.TransformPointByMatrix(matrix, points[i]));

            }
            this.sourcePoint = transformedPoints[0];
            this.targetPoint = transformedPoints[points.Count - 1];
            transformedPoints = this.IntermediatePoints = ConnectorUtil.FindConnectorPoints(this);
            DiagramUtil.UpdateConnector(this, transformedPoints);
            DiagramPoint value = new DiagramPoint
            {
                X = tx,
                Y = ty
            };
            return value;
        }
        internal DiagramElement GetFixedUserHandle(ConnectorFixedUserHandle fixedUserHandle, List<DiagramPoint> points, DiagramRect bounds)
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
            fixedUserHandleContainer.Visible = fixedUserHandle.Visibility;
            fixedUserHandleContainer.CornerRadius = fixedUserHandle.CornerRadius;
            this.UpdateUserHandle(fixedUserHandle, points, bounds, fixedUserHandleContainer);
            DiagramElement symbolIcon = InitFixedUserHandlesSymbol(fixedUserHandle, fixedUserHandleContainer);
            fixedUserHandleContainer.Children.Add(symbolIcon);
            fixedUserHandleContainer.Description = fixedUserHandleContainer.ID;
            return fixedUserHandleContainer;
        }
        private static DiagramElement InitFixedUserHandlesSymbol(ConnectorFixedUserHandle options, Canvas fixedUserHandleContainer)
        {
            PathElement fixedUserHandleContent = new PathElement()
            {
                Data = options.PathData,
                Height =
                    options.Height > 10 ? options.Height - (options.Padding.Bottom + options.Padding.Top) : options.Height,
                Width =
                    options.Width > 10 ? options.Width - (options.Padding.Left + options.Padding.Right) : options.Width,
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
            fixedUserHandleContent.Description = string.IsNullOrEmpty(fixedUserHandleContainer.Description) ? string.Empty : fixedUserHandleContainer.Description;
            return fixedUserHandleContent;
        }
        internal void UpdateShapePosition(DiagramElement element)
        {
            double segmentOffset = 0.5;
            DiagramPoint pt = new DiagramPoint(); double length = 0;
            List<DiagramPoint> anglePoints = IntermediatePoints;
            for (int i = 0; i < anglePoints.Count - 1; i++)
            {
                length += Distance(anglePoints[i], anglePoints[i + 1]);
                double offsetLength = length * segmentOffset;
                if (length >= offsetLength)
                {
                    double angle = DiagramPoint.FindAngle(anglePoints[i], anglePoints[i + 1]);
                    pt = DiagramPoint.Transform(anglePoints[i], angle, offsetLength);
                }
            }
            element.OffsetX = pt.X;
            element.OffsetY = pt.Y;
        }
        internal PathElement GetBpmnMessageFlow()
        {
            PathElement segmentMessage = new PathElement();
            this.targetDecorator.Shape = DecoratorShape.Arrow;
            this.targetDecorator.Width = 5;
            this.targetDecorator.Height = 10;
            BpmnMessageFlows bpmnFlow = this.shape.Message;
            this.sourceDecorator.Shape = DecoratorShape.Circle;
            if ((bpmnFlow == BpmnMessageFlows.InitiatingMessage) ||
                (bpmnFlow == BpmnMessageFlows.NonInitiatingMessage))
            {
                this.Style.StrokeDashArray = "4 4";
                segmentMessage.ID = this.ID + '_' + bpmnFlow;
                segmentMessage.Width = 25; segmentMessage.Height = 15;
                segmentMessage.Data = "M0,0 L19.8,12.8 L40,0 L0, 0 L0, 25.5 L40, 25.5 L 40, 0";
                segmentMessage.HorizontalAlignment = HorizontalAlignment.Center;
                segmentMessage.VerticalAlignment = VerticalAlignment.Center; segmentMessage.Transform = Transform.Self;
                segmentMessage.Style.Fill = bpmnFlow == BpmnMessageFlows.NonInitiatingMessage ? "lightgrey" : "white";
            }
            return segmentMessage;
        }
        internal void GetBpmnAssociationFlow()
        {
            BpmnAssociationFlows associationFlows = this.shape.Association;
            switch (associationFlows)
            {
                case BpmnAssociationFlows.Default:
                    this.targetDecorator.Shape = DecoratorShape.Arrow;
                    break;
                case BpmnAssociationFlows.BiDirectional:
                    this.targetDecorator.Shape = DecoratorShape.Arrow;
                    this.sourceDecorator.Shape = DecoratorShape.Arrow;
                    this.sourceDecorator.Style.Fill = "white";
                    this.Style.StrokeDashArray = "4 4";
                    break;
                case BpmnAssociationFlows.Directional:
                    this.Style.StrokeDashArray = "2 2";
                    this.targetDecorator.Shape = DecoratorShape.Arrow;

                    break;

            }
        }
        internal PathElement GetBpmnSequenceFlow()
        {
            PathElement segment = new PathElement();
            PathElement pathSeq = new PathElement();
            BpmnSequenceFlows bpmnSequenceFlows = this.Shape.Sequence;
            if (bpmnSequenceFlows == BpmnSequenceFlows.Normal && this.type != Diagram.ConnectorSegmentType.Bezier)
            {
                this.TargetDecorator.Shape = DecoratorShape.Arrow;
            }
            switch (bpmnSequenceFlows)
            {
                case BpmnSequenceFlows.Default:
                    _ = this.GetSegmentElement(this, segment);
                    List<DiagramPoint> anglePoints = this.IntermediatePoints;
                    pathSeq = ConnectorUtil.UpdatePathElement(anglePoints, this);
                    this.TargetDecorator.Shape = DecoratorShape.Arrow;
                    break;
                case BpmnSequenceFlows.Conditional:
                    this.TargetDecorator.Shape = DecoratorShape.Arrow; this.SourceDecorator.Shape = DecoratorShape.Diamond;
                    this.SourceDecorator.Style.Fill = "white";
                    pathSeq.ID = this.ID + this.shape.Type;
                    break;
            }
            return pathSeq;
        }
        internal ICommonElement GetAnnotationElement(PathAnnotation annotation, List<DiagramPoint> points, DiagramRect bounds)
        {
            TextElement text = new TextElement
            {
                Content = annotation.Content,
                Constraints = annotation.Constraints,
                Visible = annotation.Visibility,
                RotationAngle = annotation.RotationAngle,
                HorizontalAlignment = annotation.HorizontalAlignment,
                VerticalAlignment = annotation.VerticalAlignment,
                Width = annotation.Width,
                Height = annotation.Height,
                InversedAlignment = true
            };
            ((TextStyle)text.Style).TextOverflow = TextOverflow.Wrap;
            text.Margin = annotation.Margin;
            text.ID = this.ID + '_' + annotation.ID;
            if (bounds.Width == 0)
            {
                bounds.Width = this.style.StrokeWidth;
            }
            if (bounds.Height == 0)
            {
                bounds.Height = this.style.StrokeWidth;
            }
            text.Style = annotation.Style;
            text.Description = text.ID;
            this.UpdateAnnotation(annotation, points, bounds, text);
            return text;
        }
        internal void UpdateUserHandle(ConnectorFixedUserHandle annotation, List<DiagramPoint> points, DiagramRect bounds, Canvas canvas)
        {
            DiagramPoint pivotPoint = new DiagramPoint() { X = 0, Y = 0 };
            canvas.Width = annotation != null && annotation.Width != 10 ? annotation.Width : bounds.Width;
            GetLoop getPointLoop = DiagramUtil.GetFixedUserHandlePosition(points, annotation);
            DiagramPoint newPoint = getPointLoop.Point;
            if (bounds.Width == 0)
            {
                bounds.Width = this.style.StrokeWidth;
            }
            if (bounds.Height == 0)
            {
                bounds.Height = this.style.StrokeWidth;
            }
            DiagramPoint offsetPoint = new DiagramPoint() { X = ((newPoint.X - bounds.X) / bounds.Width), Y = ((newPoint.Y - bounds.Y) / bounds.Height) };
            pivotPoint.X = bounds.Width * offsetPoint.X;
            pivotPoint.Y = bounds.Height * offsetPoint.Y;
            GetAlignment align = DiagramUtil.AlignLabelOnUserHandleSegments(annotation, getPointLoop.Angle, points);
            HorizontalAlignment hAlign = align.HorizontalAlign;
            VerticalAlignment vAlign = align.VerticalAlign;
            HorizontalAlignment h = HorizontalAlignment.Center;
            VerticalAlignment v = VerticalAlignment.Center;
            if (hAlign == HorizontalAlignment.Left)
            {
                h = HorizontalAlignment.Left;
                if (annotation != null) pivotPoint.X += annotation.Displacement.X;
            }
            else if (hAlign == HorizontalAlignment.Right)
            {
                h = HorizontalAlignment.Right;
                if (annotation != null) pivotPoint.X -= annotation.Displacement.X;
            }
            else if (hAlign == HorizontalAlignment.Center)
            {
                h = HorizontalAlignment.Center;
            }
            if (vAlign == VerticalAlignment.Top)
            {
                v = VerticalAlignment.Top;
                if (annotation != null) pivotPoint.Y += annotation.Displacement.Y;
            }
            else if (vAlign == VerticalAlignment.Bottom)
            {
                v = VerticalAlignment.Bottom;
                if (annotation != null) pivotPoint.Y -= annotation.Displacement.Y;
            }
            else if (vAlign == VerticalAlignment.Center)
            {
                v = VerticalAlignment.Center;
            }
            canvas.HorizontalAlignment = h;
            canvas.VerticalAlignment = v;
            canvas.SetOffsetWithRespectToBounds(pivotPoint.X, pivotPoint.Y, UnitMode.Absolute);
            canvas.RelativeMode = RelativeMode.Point;
        }
        internal void UpdateAnnotation(PathAnnotation annotation, List<DiagramPoint> points, DiagramRect bounds, TextElement textElement)
        {
            DiagramPoint pivotPoint = new DiagramPoint() { X = 0, Y = 0 };
            //if (!(textElement instanceof DiagramHtmlElement || diagram_element_1.DiagramElement) && (!canRefresh)) {
            //    textElement.refreshTextElement();
            //}
            textElement.Width = annotation.Width ?? bounds.Width;
            GetLoop getPointLoop = DiagramUtil.GetAnnotationPosition(points, annotation);
            DiagramPoint newPoint = getPointLoop.Point;
            if (annotation.SegmentAngle)
            {
                textElement.RotationAngle = annotation.RotationAngle + getPointLoop.Angle;
                textElement.RotationAngle = (textElement.RotationAngle + 360) % 360;
            }
            if (bounds.Width == 0)
            {
                bounds.Width = this.style.StrokeWidth;
            }
            if (bounds.Height == 0)
            {
                bounds.Height = this.style.StrokeWidth;
            }
            DiagramPoint offsetPoint = new DiagramPoint() { X = ((newPoint.X - bounds.X) / bounds.Width), Y = ((newPoint.Y - bounds.Y) / bounds.Height) };
            pivotPoint.X = bounds.Width * offsetPoint.X;
            pivotPoint.Y = bounds.Height * offsetPoint.Y;
            GetAlignment align = DiagramUtil.AlignLabelOnSegments(annotation, getPointLoop.Angle, points);
            HorizontalAlignment hAlign = align.HorizontalAlign;
            VerticalAlignment vAlign = align.VerticalAlign;
            HorizontalAlignment h = annotation.HorizontalAlignment;
            VerticalAlignment v = annotation.VerticalAlignment;
            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    h = HorizontalAlignment.Left;
                    pivotPoint.X += annotation.Displacement.X;
                    break;
                case HorizontalAlignment.Right:
                    h = HorizontalAlignment.Right;
                    pivotPoint.X -= annotation.Displacement.X;
                    break;
                case HorizontalAlignment.Center:
                    h = HorizontalAlignment.Center;
                    break;
            }
            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    v = VerticalAlignment.Top;
                    pivotPoint.Y += annotation.Displacement.Y;
                    break;
                case VerticalAlignment.Bottom:
                    v = VerticalAlignment.Bottom;
                    pivotPoint.Y -= annotation.Displacement.Y;
                    break;
                case VerticalAlignment.Center:
                    v = VerticalAlignment.Center;
                    break;
            }
            textElement.HorizontalAlignment = h;
            textElement.VerticalAlignment = v;
            textElement.SetOffsetWithRespectToBounds(pivotPoint.X, pivotPoint.Y, UnitMode.Absolute);
            textElement.RelativeMode = RelativeMode.Point;
        }
        internal PathElement GetSegmentElement(Connector connector, PathElement segmentElement)
        {
            List<DiagramPoint> points = GetConnectorPoints();
            IntermediatePoints = points;
            segmentElement.StaticSize = true;
            segmentElement = UpdateSegmentElement(connector, points, segmentElement);
            return segmentElement;
        }

        private static PathElement GetDecoratorElement(DiagramPoint offsetPoint, DiagramPoint adjacentPoint, DecoratorSettings decorator)
        {
            PathElement decEle = new PathElement { Transform = Transform.Self };
            UpdateDecoratorElement(decEle, offsetPoint, adjacentPoint, decorator);
            return decEle;
        }

        internal static void UpdateDecoratorElement(DiagramElement element, DiagramPoint pt, DiagramPoint adjacentPoint, DecoratorSettings decorator)
        {
            element.OffsetX = pt.X;
            element.OffsetY = pt.Y;
            element.Width = decorator.Width;
            element.Height = decorator.Height;
            element.RotationAngle = DiagramPoint.FindAngle(pt, adjacentPoint);
            element.Pivot.X = decorator.Pivot.X;
            element.Pivot.Y = decorator.Pivot.Y;
            element.Style = decorator.Style;
            ((PathElement)element).Data = decorator.Shape == DecoratorShape.Custom ? decorator.PathData : Dictionary.GetShapeData(decorator.Shape.ToString());
            ((PathElement)element).CanMeasurePath = true;
        }

        internal List<DiagramPoint> GetConnectorPoints()
        {
            List<DiagramPoint> points;
            if ((this.Parent as SfDiagramComponent) != null && (this.Parent as SfDiagramComponent)?.Layout != null)
            {
                points = ConnectorUtil.FindConnectorPoints(this, (this.Parent as SfDiagramComponent)?.Layout);
            }
            else
            {
                points = ConnectorUtil.FindConnectorPoints(this);
            }
            List<DiagramPoint> newPoints = points.GetRange(0, points.Count);
            if (newPoints.Count > 0)
            {
                sourcePoint = newPoints[0];
                targetPoint = newPoints[^1];
            }
            return newPoints;
        }
        internal PathElement UpdateSegmentElement(Connector connector, List<DiagramPoint> points, PathElement element)
        {
            DiagramRect bounds = new DiagramRect();
            string segmentPath = GetSegmentPath(connector, points);
            if (connector.type == Diagram.ConnectorSegmentType.Bezier)
            {
                if (SegmentCollection.Count > 0)
                {
                    for (int i = 0; i < SegmentCollection.Count; i++)
                    {
                        BezierSegment segment = SegmentCollection[i] as BezierSegment;
                        if (connector.SegmentCollection[i] is BezierSegment connectorSegment)
                        {
                            DiagramPoint point1 = segment != null && !DiagramPoint.IsEmptyPoint(segment.Point1) ? connectorSegment.Point1 : connectorSegment.BezierPoint1;
                            DiagramPoint point2 = segment != null && !DiagramPoint.IsEmptyPoint(segment.Point2) ? connectorSegment.Point2 : connectorSegment.BezierPoint2;
                            if (segment != null && segment.Points != null && segment.Points.Count > 0)
                                bounds.UniteRect(GetBezierBounds(segment.Points[0], point1, point2, segment.Points[1]));
                        }
                    }
                }
            }
            else
            {
                bounds = DiagramRect.ToBounds(points);
            }
            element.Width = bounds.Width;
            element.Height = bounds.Height;
            element.OffsetX = bounds.X + bounds.Width / 2;
            element.OffsetY = bounds.Y + bounds.Height / 2;
            element.Data = segmentPath;
            DiagramContainer wrapper = connector.Wrapper;
            if (wrapper != null)
            {
                wrapper.OffsetX = element.OffsetX;
                wrapper.OffsetY = element.OffsetY;
                wrapper.Width = bounds.Width;
                wrapper.Height = bounds.Height;
            }
            return element;
        }
        internal Connector GetMessage(Connector connection)
        {
            connection.targetDecorator.Shape = DecoratorShape.Arrow;
            connection.targetDecorator.Width = 5;
            connection.targetDecorator.Height = 10;
            connection.sourceDecorator.Shape = DecoratorShape.Circle;
            connection.sourceDecorator.Width = 10;
            connection.sourceDecorator.Height = 10;
            connection.sourceDecorator.Style.Fill = "black";
            ((SfDiagramComponent)this.Parent).DiagramContent.UpdateBpmnConnectorProperties(connection);
            connection.Wrapper.Children[3] = GetBpmnMessageFlow();
            connection.Wrapper.Children[0] = GetSegmentElement(this, connection.Wrapper.Children[0] as PathElement);
            this.UpdateShapePosition((PathElement)connection.Wrapper.Children[3]);
            return connection;
        }
        internal Connector GetAssociation(Connector connection)
        {
            BpmnAssociationFlows bpmnAssociationFlows = connection.Shape.Association;
            switch (bpmnAssociationFlows)
            {
                case BpmnAssociationFlows.Default:
                    connection.Type = Diagram.ConnectorSegmentType.Straight;
                    connection.Style.StrokeDashArray = string.Empty;
                    connection.TargetDecorator.Shape = DecoratorShape.Arrow;
                    connection.TargetDecorator.Style.Fill = "black";
                    connection.SourceDecorator.Shape = DecoratorShape.None;
                    if (connection.Wrapper.Children[3] != null)
                    {
                        connection.Wrapper.Children[3].Visible = false;
                    }
                    ((SfDiagramComponent)this.Parent).DiagramContent.UpdateBpmnConnectorProperties(connection);
                    break;
                case BpmnAssociationFlows.BiDirectional:
                    connection.Type = Diagram.ConnectorSegmentType.Straight;
                    connection.Style.StrokeDashArray = "2 2";
                    connection.TargetDecorator.Shape = DecoratorShape.Arrow;
                    connection.TargetDecorator.Style.Fill = "black";
                    connection.SourceDecorator.Shape = DecoratorShape.Arrow;
                    connection.SourceDecorator.Width = 5;
                    connection.SourceDecorator.Height = 10;
                    connection.SourceDecorator.Style.Fill = "white";
                    if (connection.Wrapper.Children[3] != null)
                    {
                        connection.Wrapper.Children[3].Visible = false;
                    }
                    ((SfDiagramComponent)this.Parent).DiagramContent.UpdateBpmnConnectorProperties(connection);
                    break;
                case BpmnAssociationFlows.Directional:
                    connection.Type = Diagram.ConnectorSegmentType.Straight;
                    connection.Style.StrokeDashArray = "2 2";
                    connection.TargetDecorator.Shape = DecoratorShape.Arrow;
                    connection.TargetDecorator.Style.Fill = "black";
                    connection.SourceDecorator.Shape = DecoratorShape.None;
                    if (connection.Wrapper.Children[3] != null)
                    {
                        connection.Wrapper.Children[3].Visible = false;
                    }
                    ((SfDiagramComponent)this.Parent).DiagramContent.UpdateBpmnConnectorProperties(connection);
                    break;
            }
            return connection;
        }
        internal Connector GetSequence(Connector connection)
        {
            BpmnSequenceFlows bpmnSequenceFlows = connection.shape.Sequence;
            switch (bpmnSequenceFlows)
            {
                case BpmnSequenceFlows.Default:
                    connection.Type = Diagram.ConnectorSegmentType.Straight;
                    connection.TargetDecorator.Shape = DecoratorShape.Arrow;
                    connection.TargetDecorator.Style.Fill = "black";
                    ((PathElement)connection.Wrapper.Children[1]).Data = string.Empty;
                    connection.Style.StrokeDashArray = string.Empty;
                    connection.SourceDecorator.Shape = DecoratorShape.None;
                    ((SfDiagramComponent)this.Parent).DiagramContent.UpdateBpmnConnectorProperties(connection);
                    PathElement segment = new PathElement();
                    _ = connection.GetSegmentElement(this, segment);
                    List<DiagramPoint> anglePoints = connection.IntermediatePoints;
                    this.targetDecorator.Shape = DecoratorShape.Arrow;
                    PathInformation pathSeqData = new PathInformation();
                    ICommonElement child = connection.Wrapper.Children[3];

                    if (child is PathElement pathElement)
                    {
                        for (int j = 0; j < anglePoints.Count - 1; j++)
                        {
                            pathSeqData = ConnectorUtil.FindPath(anglePoints[j], anglePoints[j + 1]);
                            pathElement.Data = pathSeqData.Path;
                            child.ID = connection.ID + '_' + (connection.Shape.Sequence);
                            child.OffsetX = pathSeqData.Points.X;
                            child.OffsetY = pathSeqData.Points.Y;
                            child.RotationAngle = 45;
                            child.Transform = Transform.Self;
                            pathElement.IsBpmnSequenceDefault = true;
                        }
                        pathElement.AbsoluteBounds.X = pathSeqData.StartPoint.X < pathSeqData.TargetPoint.X
                            ? pathSeqData.StartPoint.X
                            : pathSeqData.TargetPoint.X;
                        pathElement.AbsoluteBounds.Y = pathSeqData.StartPoint.Y < pathSeqData.TargetPoint.Y
                            ? pathSeqData.StartPoint.Y
                            : pathSeqData.TargetPoint.Y;
                        pathElement.AbsoluteBounds.Width =
                            Math.Abs(pathSeqData.StartPoint.X - pathSeqData.TargetPoint.X);
                        pathElement.AbsoluteBounds.Height =
                            Math.Abs(pathSeqData.StartPoint.Y - pathSeqData.TargetPoint.Y);
                    }
                    break;
                case BpmnSequenceFlows.Conditional:
                    connection.Type = Diagram.ConnectorSegmentType.Straight;
                    connection.Style.StrokeDashArray = string.Empty;
                    connection.TargetDecorator.Shape = DecoratorShape.Arrow;
                    connection.TargetDecorator.Style.Fill = "black";
                    connection.SourceDecorator.Shape = DecoratorShape.Diamond;
                    connection.SourceDecorator.Style.Fill = "white";
                    connection.SourceDecorator.Width = 20;
                    connection.SourceDecorator.Height = 10;
                    ((SfDiagramComponent)this.Parent).DiagramContent.UpdateBpmnConnectorProperties(connection);
                    if (connection.Wrapper.Children[3] != null)
                    {
                        connection.Wrapper.Children[3].Visible = false;
                    }
                    break;
                case BpmnSequenceFlows.Normal:
                    connection.Type = Diagram.ConnectorSegmentType.Straight;
                    connection.Style.StrokeDashArray = string.Empty;
                    connection.TargetDecorator.Shape = DecoratorShape.Arrow;
                    connection.Style.Fill = "black";
                    connection.SourceDecorator.Shape = DecoratorShape.None;
                    ((SfDiagramComponent)this.Parent).DiagramContent.UpdateBpmnConnectorProperties(connection);
                    if (connection.Wrapper.Children[3] != null)
                    {
                        connection.Wrapper.Children[3].Visible = false;
                    }
                    break;
            }
            return connection;
        }
        internal void UpdateShapeElement(Connector connector)
        {
            DiagramElement element = new DiagramElement();
            switch (connector.shape.Type)
            {
                case ConnectorShapeType.Bpmn:
                    if (connector.Wrapper.Children[3] is PathElement)
                    {
                        element = connector.Wrapper.Children[3] as PathElement;
                    }
                    if (connector.shape.Flow == BpmnFlows.Message)
                    {
                        this.UpdateShapePosition(element);
                    }
                    break;
            }
        }
        internal static List<DiagramPoint> ClipDecorators(Connector connector, List<DiagramPoint> pts)
        {
            if (connector != null && pts != null)
            {
                if (connector.SourceDecorator.Shape != DecoratorShape.None)
                {
                    pts[0] = ClipDecorator(connector, pts, true);
                }
                if (connector.TargetDecorator.Shape != DecoratorShape.None)
                {
                    pts[^1] = ClipDecorator(connector, pts, false);
                }
            }
            return pts;
        }

        private static DiagramPoint ClipDecorator(Connector connector, List<DiagramPoint> points, bool isSource)
        {
            DiagramPoint point = new DiagramPoint() { X = 0, Y = 0 };
            int length = points.Count;
            DiagramPoint start = !isSource ? points[length - 1] : points[0];
            DiagramPoint end = !isSource ? points[length - 2] : points[1];
            double len = DiagramPoint.DistancePoints(start, end);
            len = (len == 0) ? 1 : len;
            double strokeWidth = 1;
            DiagramElement node = isSource ? connector.SourceWrapper : connector.TargetWrapper;
            if (node != null)
            {
                strokeWidth = node.Style.StrokeWidth;
            }
            double width = strokeWidth - 1;
            point.X = Math.Round(start.X + width * (end.X - start.X) / len);
            point.Y = Math.Round(start.Y + width * (end.Y - start.Y) / len);
            if ((isSource && connector.sourceDecorator.Shape != DecoratorShape.None) ||
                (!isSource && connector.targetDecorator.Shape != DecoratorShape.None))
            {
                point = DiagramPoint.AdjustPoint(point, end, true, strokeWidth / 2);
            }
            return point;
        }

        private static DiagramRect GetBezierBounds(DiagramPoint startPoint, DiagramPoint controlPoint1, DiagramPoint controlPoint2, DiagramPoint endPoint)
        {
            double minx = 0; double miny = 0; double maxX = 0; double maxY = 0;
            double toleranceValue = 3;
            double max = (Distance(controlPoint1, startPoint) +
                Distance(controlPoint2, controlPoint1) +
                Distance(endPoint, controlPoint2)) / toleranceValue;
            if (max != 0)
            {
                double t; double x; double y;
                for (int i = 0; i <= max; i++)
                {
                    t = i / max;
                    x = (1 - t) * (1 - t) * (1 - t) * startPoint.X +
                        3 * t * (1 - t) * (1 - t) * controlPoint1.X +
                        3 * t * t * (1 - t) * controlPoint2.X +
                        t * t * t * endPoint.X;
                    y = (1 - t) * (1 - t) * (1 - t) * startPoint.Y +
                            3 * t * (1 - t) * (1 - t) * controlPoint1.Y +
                            3 * t * t * (1 - t) * controlPoint2.Y +
                            t * t * t * endPoint.Y;
                    if (i == 0)
                    {
                        minx = maxX = x;
                        miny = maxY = y;
                    }
                    else
                    {
                        minx = Math.Min(x, minx);
                        miny = Math.Min(y, miny);
                        maxX = Math.Max(x, maxX);
                        maxY = Math.Max(y, maxY);
                    }
                }
            }
            return new DiagramRect() { X = minx, Y = miny, Width = maxX - minx, Height = maxY - miny };
        }
        internal static double Distance(DiagramPoint pt1, DiagramPoint pt2)
        {
            return DiagramPoint.DistancePoints(pt1, pt2);
        }

        internal string GetSegmentPath(Connector connector, List<DiagramPoint> points)
        {
            string path = string.Empty;
            if (connector != null && points != null)
            {
                DiagramPoint getPt;
                DiagramPoint st = null; int length = points.Count;
                List<DiagramPoint> pts = new List<DiagramPoint>();
                int j = 0;
                while (j < length)
                {
                    pts.Add(new DiagramPoint() { X = points[j].X, Y = points[j].Y });
                    j++;
                }
                for (int m = 0; m < connector.Bridges.Count; m++)
                {
                    Bridge bridge = connector.Bridges[m];
                    bridge.Rendered = false;
                }
                pts = ClipDecorators(connector, pts);
                if (CornerRadius > 0 && Type != Diagram.ConnectorSegmentType.Bezier)
                {
                    for (j = 0; j < pts.Count - 1; j++)
                    {
                        getPt = pts[j];
                        if (j == 0) { path = "M" + getPt.X + " " + getPt.Y; }
                        double segLength = DiagramPoint.DistancePoints(pts[j], pts[j + 1]);
                        if (segLength > 0)
                        {
                            DiagramPoint end;
                            if (j < pts.Count - 2)
                            {
                                end = segLength < CornerRadius * 2 ? DiagramPoint.AdjustPoint(pts[j], pts[j + 1], false, segLength / 2) : DiagramPoint.AdjustPoint(pts[j], pts[j + 1], false, cornerRadius);
                            }
                            else { end = pts[j + 1]; }
                            if (j > 0)
                            {
                                if (segLength < cornerRadius * 2)
                                {
                                    st = DiagramPoint.AdjustPoint(pts[j], pts[j + 1], true, segLength / 2);
                                    if (j < pts.Count - 2) { end = null; }
                                }
                                else { st = DiagramPoint.AdjustPoint(pts[j], pts[j + 1], true, cornerRadius); }
                            }
                            if (st != null) { path += "Q" + getPt.X + " " + getPt.Y + " " + st.X + " " + st.Y; }
                            if (end != null)
                            {
                                if (connector.Bridges.Count > 0)
                                {
                                    path = BridgePath(connector, path, j);
                                    if (connector.Type == Diagram.ConnectorSegmentType.Orthogonal)
                                    {
                                        path = BridgePath(connector, path, j + 1);
                                    }
                                }
                                path += " L" + end.X + " " + end.Y;
                            }
                        }
                    }
                }
                else
                {
                    if (type == Diagram.ConnectorSegmentType.Bezier)
                    {
                        Direction? direction = null; ObservableCollection<ConnectorSegment> segments = Segments; BezierSegment segment;
                        for (j = 0; j < segments.Count; j++)
                        {
                            segment = segments[j] as BezierSegment;
                            if (segment != null)
                            {
                                if (pts.Count > 2)
                                {
                                    segment.BezierPoint1 = new DiagramPoint { X = 0, Y = 0 };
                                    segment.BezierPoint2 = new DiagramPoint { X = 0, Y = 0 };

                                }
                                if (DiagramPoint.IsEmptyPoint(segment.Point1) && segment.Vector1.Angle == 0 &&
                                    segment.Vector1.Distance == 0)
                                {
                                    if ((!string.IsNullOrEmpty(connector.SourceID) ||
                                         !string.IsNullOrEmpty(SourcePortID)) && SourceWrapper != null)
                                    {
                                        direction = GetDirection(SourceWrapper.Bounds, pts[j], true);
                                    }

                                    segment.BezierPoint1 = GetBezierPoints(pts[j], pts[j + 1], direction);
                                }
                                else if (segment.Vector1.Angle != 0 || segment.Vector1.Distance != 0)
                                {
                                    segment.BezierPoint1 = DiagramPoint.Transform(pts[j], segment.Vector1.Angle,
                                        segment.Vector1.Distance);
                                }
                                else
                                {
                                    segment.BezierPoint1 = new DiagramPoint()
                                    {
                                        X = segment.Point1.X != 0 ? segment.Point1.X : segment.BezierPoint1.X,
                                        Y = segment.Point1.Y != 0 ? segment.Point1.Y : segment.BezierPoint1.Y
                                    };
                                }

                                if (DiagramPoint.IsEmptyPoint(segment.Point2) && segment.Vector2.Angle == 0 &&
                                    segment.Vector2.Distance == 0)
                                {
                                    if ((!string.IsNullOrEmpty(connector.targetId) ||
                                         !string.IsNullOrEmpty(targetPortId)) && TargetWrapper != null)
                                    {
                                        direction = GetDirection(TargetWrapper.Bounds, pts[j + 1], true);
                                    }

                                    segment.BezierPoint2 = GetBezierPoints(pts[j + 1], pts[j], direction);
                                }
                                else if (segment.Vector2.Angle != 0 || segment.Vector2.Distance != 0)
                                {
                                    segment.BezierPoint2 = DiagramPoint.Transform(pts[j + 1], segment.Vector2.Angle,
                                        segment.Vector2.Distance);
                                }
                                else
                                {
                                    segment.BezierPoint2 = new DiagramPoint
                                    {
                                        X = (segment.Point2.X != 0) ? segment.Point2.X : segment.BezierPoint2.X,
                                        Y = (segment.Point2.Y != 0) ? segment.Point2.Y : segment.BezierPoint2.Y
                                    };
                                }
                            }
                        }

                        if (segments.Count > 0)
                        {
                            if (segments[0] is BezierSegment bezierSegment)
                            {
                                pts.Insert(1,
                                    new DiagramPoint()
                                    {
                                        X = bezierSegment.BezierPoint1.X,
                                        Y = bezierSegment.BezierPoint1.Y
                                    });
                                pts.Insert(pts.Count - 1,
                                    new DiagramPoint()
                                    {
                                        X = bezierSegment.BezierPoint2.X,
                                        Y = bezierSegment.BezierPoint2.Y
                                    });
                            }

                            pts = ClipDecorators(connector, pts);
                            for (j = 0; j < segments.Count; j++)
                            {
                                segment = segments[j] as BezierSegment;
                                if (j == 0)
                                {
                                    path = "M" + pts[0].X + " " + pts[0].Y;
                                }

                                if (segment != null)
                                {
                                    string lastPoint = (j == segments.Count - 1)
                                        ? pts[^1].X.ToString(CultureInfo.InvariantCulture) + " " +
                                          pts[^1].Y.ToString(CultureInfo.InvariantCulture)
                                        : segment.Points[^1].X.ToString(CultureInfo.InvariantCulture) + " " +
                                          segment.Points[^1].Y.ToString(CultureInfo.InvariantCulture);
                                    path += "C" +
                                            segment.BezierPoint1.X + " " + segment.BezierPoint1.Y + " " +
                                            segment.BezierPoint2.X + " "
                                            + segment.BezierPoint2.Y + " " + lastPoint;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < pts.Count; k++)
                        {
                            getPt = pts[k];
                            if (k == 0) { path = "M" + getPt.X + " " + getPt.Y; }
                            if (k > 0)
                            {
                                path = BridgePath(connector, path, k);
                                path += " " + "L" + getPt.X + " " + getPt.Y;
                            }
                        }
                    }
                }
            }
            return path;
        }

        private static Direction GetDirection(DiagramRect bounds, DiagramPoint points, bool excludeBounds)
        {
            DiagramPoint center = bounds.Center;
            double part = excludeBounds ? 45 : 180 / (2 + 2 / (bounds.Height / bounds.Width));
            double one35 = 180 - part;
            double two25 = one35 + (2 * part);
            double three15 = 360 - part;
            double angle = DiagramPoint.FindAngle(points, center); // Check the method return correct value
            if (angle > part && angle < one35)
            {
                return Direction.Top;
            }
            else if (angle > one35 && angle < two25)
            {
                return Direction.Right;
            }
            else if (angle > two25 && angle < three15)
            {
                return Direction.Bottom;
            }
            else
            {
                return Direction.Left;
            }
        }

        private static string BridgePath(Connector connector, string path, double pointIndex)
        {
            string pathData = path;
            if (connector.Bridges.Count > 0)
            {
                if (connector.type == Diagram.ConnectorSegmentType.Straight && connector.SegmentCollection.Count < 2)
                {
                    for (int n = 0; n < connector.Bridges.Count; n++)
                    {
                        Bridge bridge = connector.Bridges[n];
                        if (!bridge.Rendered)
                        {
                            pathData += " L" + bridge.StartPoint.X + " " + bridge.StartPoint.Y + bridge.Path;
                            bridge.Rendered = true;
                        }
                    }
                }
                else if (connector.type == Diagram.ConnectorSegmentType.Orthogonal || (connector.type == Diagram.ConnectorSegmentType.Straight && connector.Segments.Count > 1))
                {
                    for (int n = 0; n < connector.Bridges.Count; n++)
                    {
                        Bridge bridge = connector.Bridges[n];
                        if (bridge.SegmentPointIndex.Equals(pointIndex) && !bridge.Rendered)
                        {
                            pathData += " L" + bridge.StartPoint.X + " " + bridge.StartPoint.Y + bridge.Path;
                            bridge.Rendered = true;
                        }
                    }
                }
            }
            return pathData;
        }
        internal static DiagramPoint GetBezierPoints(DiagramPoint sourcePoint, DiagramPoint targetPoint, Direction? direction = null)
        {
            double distance = 60;
            DiagramPoint value = new DiagramPoint { X = 0, Y = 0 };
            if (direction == null)
            {
                if (Math.Abs(Math.Round((targetPoint.X - sourcePoint.X) * Math.Pow(10, 2)) / Math.Pow(10, 2)) >= Math.Abs(Math.Round((targetPoint.Y - sourcePoint.Y) * Math.Pow(10, 2)) / Math.Pow(10, 2)))
                {
                    direction = sourcePoint.X < targetPoint.X ? Direction.Right : Direction.Left;
                }
                else
                {
                    direction = sourcePoint.Y < targetPoint.Y ? Direction.Bottom : Direction.Top;
                }
            }
            switch (direction)
            {
                case Direction.Bottom:
                case Direction.Top:
                    distance = Math.Min(Math.Abs(sourcePoint.Y - targetPoint.Y) * 0.45, distance);
                    value = new DiagramPoint { X = sourcePoint.X, Y = sourcePoint.Y + (direction == Direction.Bottom ? distance : -distance) };
                    break;
                case Direction.Right:
                case Direction.Left:
                    distance = Math.Min(Math.Abs(sourcePoint.X - targetPoint.X) * 0.45, distance);
                    value = new DiagramPoint { X = sourcePoint.X + (direction == Direction.Right ? distance : -distance), Y = sourcePoint.Y };
                    break;
            }
            return value;
        }
        internal static bool IsEmptyVector(Vector element)
        {
            if (element.Distance == 0 && element.Angle == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates a new connector that is a copy of the current connector.
        /// </summary>
        /// <returns>Connector</returns>
        public override object Clone()
        {
            return new Connector(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (sourcePoint != null)
            {
                sourcePoint.Dispose();
                sourcePoint = null;
            }

            if (targetPoint != null)
            {
                targetPoint.Dispose();
                targetPoint = null;
            }

            sourceId = null;
            targetId = null;
            sourcePortId = null;
            targetPortId = null;

            if (shape != null)
            {
                shape = null;
            }
            if (style != null)
            {
                style.Dispose();
                style = null;
            }
            if (SegmentCollection != null)
            {
                for (int i = 0; i < SegmentCollection.Count; i++)
                {
                    if (SegmentCollection[i] != null)
                    {
                        SegmentCollection[i].Dispose();
                    }
                    SegmentCollection[i] = null;
                }
                SegmentCollection.Clear();
                SegmentCollection = null;
            }
            if (sourceDecorator != null)
            {
                sourceDecorator.Dispose();
                sourceDecorator = null;
            }
            if (targetDecorator != null)
            {
                targetDecorator.Dispose();
                targetDecorator = null;
            }

            if (annotations != null)
            {
                for (int i = 0; i < annotations.Count; i++)
                {
                    annotations[i].Dispose();
                    annotations[i] = null;
                }

                annotations.Clear();
                annotations = null;
            }
            if (fixedUserHandles != null)
            {
                for (int i = 0; i < fixedUserHandles.Count; i++)
                {
                    fixedUserHandles[i].Dispose();
                    fixedUserHandles[i] = null;
                }

                fixedUserHandles.Clear();
                fixedUserHandles = null;
            }
        }
    }
}
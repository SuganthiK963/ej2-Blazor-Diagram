using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Defines the behavior of shape.
    /// </summary>
    public class Shape : DiagramObject
    {
        private Shapes type = Shapes.Basic;        
        /// <summary>
        /// Creates a new instance of the Shape from the given Shape.
        /// </summary>
        /// <param name="src"></param>
        public Shape(Shape src) : base(src)
        {
            if (src != null)
            {
                type = src.type;
            }
        }
        /// <summary>
        /// Initializes a new instance of the Shape.
        /// </summary>
        public Shape() : base()
        {

        }

        /// <summary>
        /// Gets or sets the type of node shape.
        /// </summary>
        [JsonPropertyName("type")]
        public Shapes Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                    type = value;
                }
            }
        }
        /// <summary>
        /// Creates a new Shape that is a copy of the current Shape. 
        /// </summary>
        /// <returns>Shape</returns>
        public override object Clone()
        {
            return new Shape(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }
    /// <summary>
    /// Represents the shape of the connector.
    /// </summary>
    public class ConnectorShape : DiagramObject
    {
        private ConnectorShapeType type = ConnectorShapeType.None;
        /// <summary>
        /// Creates a new instance of the <see cref="ConnectorShape"/> from the given <see cref="ConnectorShape"/>.
        /// </summary>
        /// <param name="src">ConnectorShape</param>
        public ConnectorShape(ConnectorShape src) : base(src)
        {
            if (src != null)
            {
                type = src.type;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorShape"/>.
        /// </summary>
        public ConnectorShape() : base()
        {
        }
        /// <summary>
        /// Defines the type of connector shape.
        /// </summary>
        [JsonPropertyName("type")]
        internal ConnectorShapeType Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                    type = value;
                }
            }
        }
        /// <summary>
        /// Creates a new connector shape that is a copy of the current shape.
        /// </summary>
        /// <returns>ConnectorShape</returns>
        public override object Clone()
        {
            return new ConnectorShape(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }
    /// <summary>
    /// Represents the elements that are connected together to form a complete process flow in the diagram. 
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
    internal class BpmnFlow : ConnectorShape
    {

        private BpmnFlows flow = BpmnFlows.Sequence;
        private BpmnSequenceFlows sequence = BpmnSequenceFlows.Normal;
        private BpmnMessageFlows message = BpmnMessageFlows.Default;
        private BpmnAssociationFlows association = BpmnAssociationFlows.Default;
        /// <summary>
        /// Creates a new instance of the BpmnFlow from the given BpmnFlow.
        /// </summary>
        /// <param name="src">BpmnFlow</param>
        public BpmnFlow(BpmnFlow src) : base(src)
        {
            if (src != null)
            {
                flow = src.flow;
                sequence = src.sequence;
                message = src.message;
                association = src.association;
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnFlow.
        /// </summary>
        public BpmnFlow() : base()
        {
        }

        /// <summary>
        ///  Sets the type of the Bpmn flows. 
        /// </summary>
        [JsonPropertyName("flow")]
        public BpmnFlows Flow
        {
            get => flow;
            set
            {
                if (flow != value)
                {
                    Parent?.OnPropertyChanged(nameof(Flow), value, flow, this);
                    flow = value;
                }
            }
        }
        /// <summary>
        /// Sets the type of the Bpmn Sequence flows. 
        /// </summary>
        [JsonPropertyName("sequence")]
        public BpmnSequenceFlows Sequence
        {
            get => sequence;
            set
            {
                if (sequence != value)
                {
                    Parent?.OnPropertyChanged(nameof(Sequence), value, sequence, this);
                    sequence = value;
                }
            }
        }
        /// <summary>
        /// Sets the type of the Bpmn Message flows. 
        /// </summary>
        [JsonPropertyName("message")]
        public BpmnMessageFlows Message
        {
            get => message;
            set
            {
                if (message != value)
                {
                    Parent?.OnPropertyChanged(nameof(Message), value, message, this);
                    message = value;
                }
            }
        }
        /// <summary>
        /// Represents the type of data movement between the data objects, inputs, and outputs of the activities using nodes in the diagram. 
        /// </summary>
        [JsonPropertyName("association")]
        public BpmnAssociationFlows Association
        {
            get => association;
            set
            {
                if (association != value)
                {
                    Parent?.OnPropertyChanged(nameof(Association), value, association, this);
                    association = value;
                }
            }
        }
        /// <summary>
        /// Creates a new BpmnFlow that is a copy of the current BpmnFlow. 
        /// </summary>
        /// <returns>BpmnFlow</returns>
        public override object Clone()
        {
            return new BpmnFlow(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }
    /// <summary>
    /// Draws a series of connected lines and curves.
    /// </summary>
    public class PathShape : Shape
    {
        private string data;
        /// <summary>
        /// Creates a new instance of the Path from the given Path.
        /// </summary>
        /// <param name="src">Path</param>
        public PathShape(PathShape src) : base(src)
        {
            if (src != null)
            {
                data = src.data;
            }
        }
        /// <summary>
        /// Initializes a new instance of the Path.
        /// </summary>
        public PathShape() : base()
        {
            Type = Shapes.Path;
        }

        /// <summary>
        /// Gets or sets a Geometry that specifies the shape to be drawn.
        /// </summary>
        [JsonPropertyName("data")]
        public string Data
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
        /// Creates a new path data that is a copy of the current path data.
        /// </summary>
        /// <returns>Path</returns>
        public override object Clone()
        {
            return new PathShape(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            data = null;
        }
    }
    /// <summary>
    /// Represents the behavior of the image shape.
    /// </summary>
    public class ImageShape : Shape
    {
        private string source;

        private ImageAlignment imageAlign = ImageAlignment.None;
        private Scale scale = Scale.None;
        /// <summary>
        /// Initializes a new instance of the Image.
        /// </summary>
        public ImageShape() : base()
        {
            Type = Shapes.Image;
        }
        /// <summary>
        /// Creates a new instance of the Image from the given Image.
        /// </summary>
        /// <param name="src">Image</param>
        public ImageShape(ImageShape src) : base(src)
        {
            if (src != null)
            {
                source = src.source;
                imageAlign = src.imageAlign;
                scale = src.scale;
            }
        }

        /// <summary>
        /// Gets or sets the ImageSource of the image. 
        /// </summary>
        [JsonPropertyName("source")]
        public string Source
        {
            get => source;
            set
            {
                if (source != value)
                {
                    Parent?.OnPropertyChanged(nameof(Source), value, source, this);
                    source = value;
                }
            }
        }

        /// <summary>
        /// Sets the alignment of the image within the node boundary.
        /// </summary>
        [JsonPropertyName("imageAlign")]
        public ImageAlignment ImageAlign
        {
            get => imageAlign;
            set
            {
                if (imageAlign != value)
                {
                    Parent?.OnPropertyChanged(nameof(ImageAlign), value, imageAlign, this);
                    imageAlign = value;
                }
            }
        }
        /// <summary>
        /// Allows you to stretch the image as you desire (either to maintain the proportion or to stretch).
        /// </summary>
        [JsonPropertyName("scale")]
        public Scale Scale
        {
            get => scale;
            set
            {
                if (scale != value)
                {
                    Parent?.OnPropertyChanged(nameof(Scale), value, scale, this);
                    scale = value;
                }
            }
        }
        /// <summary>
        /// Creates a new image that is a copy of the current image.
        /// </summary>
        /// <returns>Image</returns>
        public override object Clone()
        {
            return new ImageShape(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            source = null;
        }
    }
    /// <summary>
    /// Gets or sets the behavior of the basic shape.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// Node builtInNode = new Node()
    /// {
    ///     // Position of the node
    ///     OffsetX = 250,
    ///     OffsetY = 250,
    ///     // Size of the node
    ///     Width = 100,
    ///     Height = 100,
    ///     Shape = new FlowShape()
    ///     {
    ///         Type = Shapes.Flow,Shape = FlowShapesType.DirectData
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    public class BasicShape : Shape
    {
        private BasicShapeType shape = BasicShapeType.Rectangle;
        private double cornerRadius;
        private DiagramPoint[] points;
        internal string PolygonPath;
        /// <summary>
        /// Initializes a new instance of the BasicShape.
        /// </summary>
        public BasicShape() : base()
        {

            Type = Shapes.Basic;
        }
        /// <summary>
        /// Creates a new instance of the BasicShape from the given BasicShape.
        /// </summary>
        /// <param name="src">BasicShape</param>
        public BasicShape(BasicShape src) : base(src)
        {
            if (src != null)
            {
                shape = src.shape;
                cornerRadius = src.cornerRadius;
                PolygonPath = src.PolygonPath;
                if (src.points != null)
                {
                    points = src.points;
                }
            }
        }

        /// <summary>
        /// Defines the available built-in basic shapes.
        /// </summary>
        [JsonPropertyName("shape")]
        public BasicShapeType Shape
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
        /// Gets or sets the corner radius of the node shape. It is applicable only to a rectangle shape.
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
        /// Defines the collection of points to draw a polygon. It is applicable only to a polygon shape.
        /// </summary>
        [JsonPropertyName("points")]
        public DiagramPoint[] Points
        {
            get => points;
            set
            {
                if (points != value)
                {
                    Parent?.OnPropertyChanged(nameof(Points), value, points, this);
                    DiagramUtil.SetParentForObservableCollection(points, this, nameof(Points));
                    points = value;
                }
            }
        }
        /// <summary>
        /// Creates a new shape that is a copy of the current basic shape.
        /// </summary>
        /// <returns>BasicShape</returns>
        public override object Clone()
        {
            return new BasicShape(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (points != null)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].Dispose();
                    points[i] = null;
                }
                Array.Clear(points, 0, points.Length);
                points = null;
            }
            PolygonPath = null;
        }
    }

    /// <summary>
    /// Specifies the behavior of the flow shape.
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
    public class FlowShape : Shape
    {
        private FlowShapeType shape = FlowShapeType.Terminator;
        /// <summary>
        /// Initializes a new instance of the FlowShape.
        /// </summary>
        public FlowShape() : base()
        {
            Type = Shapes.Flow;
        }
        /// <summary>
        /// Creates a new instance of the FlowShape from the given FlowShape.
        /// </summary>
        /// <param name="src">FlowShape</param>
        public FlowShape(FlowShape src) : base(src)
        {
            if (src != null)
            {
                shape = src.shape;
            }
        }


        /// <summary>
        /// Defines the available built-in flow shapes.
        /// </summary>
        [JsonPropertyName("shape")]
        public FlowShapeType Shape
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
        /// Creates a new shape that is a copy of the current FlowShape.
        /// </summary>
        /// <returns>FlowShape</returns>
        public override object Clone()
        {
            return new FlowShape(this);
        }
    }
    /// <summary>
    /// Represents the behaviour of the BPMN shapes in the diagram. 
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
    internal class BpmnShape : Shape
    {
        private BpmnShapes shape = BpmnShapes.Event;
        private BpmnAnnotation annotation = new BpmnAnnotation();
        private DiagramObjectCollection<BpmnAnnotation> annotations = new DiagramObjectCollection<BpmnAnnotation>();
        private BpmnSubEvent events = new BpmnSubEvent();
        private BpmnGateway gateway = new BpmnGateway();
        private BpmnActivity activity = new BpmnActivity();
        private BpmnDataObject dataObject = new BpmnDataObject();
        /// <summary>
        /// Sets whether the task is global or not
        /// </summary>
        internal int annotationId;
        /// <summary>
        /// Initializes a new instance of the BpmnShape.
        /// </summary>
        public BpmnShape() : base()
        {
            annotations.Parent = this;
            annotation.Parent = this;
            Type = Shapes.Bpmn;
        }
        /// <summary>
        /// Creates a new instance of the BpmnShape from the given BpmnShape.
        /// </summary>
        /// <param name="src">BpmnShape</param>
        public BpmnShape(BpmnShape src) : base(src)
        {
            if (src != null)
            {
                shape = src.shape;
                if (src.annotation != null)
                {
                    annotation = src.annotation.Clone() as BpmnAnnotation;
                }
                if (src.events != null)
                {
                    events = src.events.Clone() as BpmnSubEvent;
                }
                if (src.gateway != null)
                {
                    gateway = src.gateway.Clone() as BpmnGateway;
                }
                if (src.activity != null)
                {
                    activity = src.activity.Clone() as BpmnActivity;
                }
                if (src.dataObject != null)
                {
                    dataObject = src.dataObject.Clone() as BpmnDataObject;
                }
                annotations = new DiagramObjectCollection<BpmnAnnotation>();
                if (src.annotations.Count > 0)
                {
                    foreach (BpmnAnnotation bpmnAnnotation in src.annotations)
                    {
                        BpmnAnnotation annotationClone = bpmnAnnotation.Clone() as BpmnAnnotation;
                        annotations.Add(annotationClone);
                    }
                }
                annotations.Parent = this;
            }
        }

        /// <summary>
        /// Represents the type of the BPMN shape.
        /// </summary>
        [JsonPropertyName("shape")]
        public BpmnShapes Shape
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
        /// Represents a note to a process element or to the business process itself using a node in the diagram. 
        /// </summary>
        [JsonPropertyName("annotation")]
        public BpmnAnnotation Annotation
        {
            get
            {
                if (annotation != null && annotation.Parent == null)
                    annotation.SetParent(this, nameof(Annotation));
                return annotation;
            }
            set
            {
                if (annotation != value)
                {
                    Parent?.OnPropertyChanged(nameof(Annotation), value, annotation, this);
                    annotation = value;
                }
            }
        }
        /// <summary>
        /// Represents the collection of annotations to be displayed over the bpmn nodes. 
        /// </summary>
        [JsonPropertyName("annotations")]
        public DiagramObjectCollection<BpmnAnnotation> Annotations
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
                    annotations = value;
                    annotations.Parent = this;
                    if (this.Parent != null)
                    {
                        Parent.OnPropertyChanged(nameof(Annotations), value, annotations, this);
                    }
                }
            }
        }

        /// <summary>
        ///  Represents the actions that are to be performed and are expressed as circles in the diagram.
        /// </summary>
        [JsonPropertyName("events")]
        public BpmnSubEvent Events
        {
            get
            {
                if (events != null && events.Parent == null)
                    events.SetParent(this, nameof(Events));
                return events;
            }
            set
            {
                if (events != value)
                {
                    Parent?.OnPropertyChanged(nameof(Events), value, events, this);
                    events = value;
                }
            }
        }

        /// <summary>
        ///   It allows to control as well as merge and split the process flow in the diagram.
        /// </summary>
        [JsonPropertyName("gateway")]
        public BpmnGateway Gateway
        {
            get
            {
                if (gateway != null && gateway.Parent == null)
                    gateway.SetParent(this, nameof(Gateway));
                return gateway;
            }
            set
            {
                if (gateway != value)
                {
                    Parent?.OnPropertyChanged(nameof(Gateway), value, gateway, this);
                    gateway = value;
                }
            }
        }

        /// <summary>
        ///  Represents the work that a company or organization performs in a business process using a node in the diagram. 
        /// </summary>
        [JsonPropertyName("activity")]
        public BpmnActivity Activity
        {
            get
            {
                if (activity != null && activity.Parent == null)
                    activity.SetParent(this, nameof(Activity));
                return activity;
            }
            set
            {
                if (activity != value)
                {
                    Parent?.OnPropertyChanged(nameof(Activity), value, activity, this);
                    activity = value;
                }
            }
        }

        /// <summary>
        /// Represents the transferring of data into or out of an activity in the diagram. 
        /// </summary>
        [JsonPropertyName("dataObject")]
        public BpmnDataObject DataObject
        {
            get
            {
                if (dataObject != null && dataObject.Parent == null)
                    dataObject.SetParent(this, nameof(DataObject));
                return dataObject;
            }
            set
            {
                if (dataObject != value)
                {
                    Parent?.OnPropertyChanged(nameof(DataObject), value, dataObject, this);
                    dataObject = value;
                }
            }
        }
        /// <summary>
        /// Creates a new BpmnShape that is a copy of the current BpmnShape. 
        /// </summary>
        /// <returns>BpmnShape</returns>
        public override object Clone()
        {
            return new BpmnShape(this);
        }
    }
    /// <summary>
    /// Represents a set of additional tasks categorized together in the diagram. 
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
    ///             SubProcess = new BpmnSubProcess() { Collapsed = false, Type = BpmnSubProcessTypes.Transaction, Processes = new DiagramObjectCollection<string>() { "new" } } 
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal class BpmnSubProcess : DiagramObject
    {
        private BpmnSubProcessTypes type = BpmnSubProcessTypes.None;
        private bool adhoc;
        private BpmnBoundary boundary = BpmnBoundary.Default;
        private bool compensation;
        private BpmnLoops loop = BpmnLoops.None;
        private bool collapsed = true;
        private List<BpmnSubEvent> events = new List<BpmnSubEvent>();
        private BpmnTransactionSubProcess transaction = new BpmnTransactionSubProcess();
        private DiagramObjectCollection<string> processes = new DiagramObjectCollection<string>();
        /// <summary>
        /// Defines the ID of the sub process
        /// </summary>

        internal string processesID;
        /// <summary>
        /// Initializes a new instance of the BpmnSubProcess.
        /// </summary>
        public BpmnSubProcess() : base()
        {
            processes.Parent = this;
        }
        /// <summary>
        /// Creates a new instance of the BpmnSubProcess from the given BpmnSubProcess.
        /// </summary>
        /// <param name="src">BpmnSubProcess</param>
        public BpmnSubProcess(BpmnSubProcess src) : base(src)
        {
            if (src != null)
            {
                type = src.type;
                adhoc = src.adhoc;
                boundary = src.boundary;
                compensation = src.compensation;
                loop = src.loop;
                collapsed = src.collapsed;
                events = src.events;
                if (src.transaction != null)
                {
                    transaction = src.transaction.Clone() as BpmnTransactionSubProcess;
                }
                processes = src.processes;
                processes.Parent = this;
            }
        }

        /// <summary>
        /// Sets the specific type of the subprocess.
        /// </summary>
        [JsonPropertyName("type")]
        public BpmnSubProcessTypes Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                    type = value;
                }
            }
        }

        /// <summary>
        /// Defines whether the sub process is without any prescribed order or not. 
        /// </summary>
        [JsonPropertyName("adhoc")]
        public bool Adhoc
        {
            get => adhoc;
            set
            {
                if (adhoc != value)
                {
                    Parent?.OnPropertyChanged(nameof(Adhoc), value, adhoc, this);
                    adhoc = value;
                }
            }
        }

        /// <summary>
        /// Represents the type of the BPMN boundary. 
        /// </summary>
        [JsonPropertyName("boundary")]
        public BpmnBoundary Boundary
        {
            get => boundary;
            set
            {
                if (boundary != value)
                {
                    Parent?.OnPropertyChanged(nameof(Boundary), value, boundary, this);
                    boundary = value;
                }
            }
        }


        /// <summary>
        /// Represents a collection of tasks that describe some parts of the compensation method. 
        /// </summary>
        [JsonPropertyName("compensation")]
        public bool Compensation
        {
            get => compensation;
            set
            {
                if (compensation != value)
                {
                    Parent?.OnPropertyChanged(nameof(Compensation), value, compensation, this);
                    compensation = value;
                }
            }
        }



        /// <summary>
        /// Represents that the sub-process repeats itself in sequence.
        /// </summary>
        [JsonPropertyName("loop")]
        public BpmnLoops Loop
        {
            get => loop;
            set
            {
                if (loop != value)
                {
                    Parent?.OnPropertyChanged(nameof(Loop), value, loop, this);
                    loop = value;
                }
            }
        }

        /// <summary>
        ///  Represents whether the task is global or not.
        /// </summary>
        [JsonPropertyName("collapsed")]
        public bool Collapsed
        {
            get => collapsed;
            set
            {
                if (collapsed != value)
                {
                    Parent?.OnPropertyChanged(nameof(Collapsed), value, collapsed, this);
                    collapsed = value;
                }
            }
        }

        /// <summary>
        /// Represents when an event occurs at the start, finish, or middle of a process, it is referred to as an event. 
        /// </summary>
        [JsonPropertyName("events")]
        public List<BpmnSubEvent> Events
        {
            get => events;
            set
            {
                if (events != value)
                {
                    Parent?.OnPropertyChanged(nameof(Events), value, events, this);
                    events = value;
                }
            }
        }

        /// <summary>
        /// Represents a composed activity that is included in a process. 
        /// </summary>
        [JsonPropertyName("transaction")]
        public BpmnTransactionSubProcess Transaction
        {
            get => transaction;
            set
            {
                if (transaction != value)
                {
                    Parent?.OnPropertyChanged(nameof(Transaction), value, transaction, this);
                    transaction = value;
                }
            }
        }

        /// <summary>
        /// Represents a set of tasks categorized together in the subprocess.
        /// </summary>
        [JsonPropertyName("processes")]
        public DiagramObjectCollection<string> Processes
        {
            get
            {
                if (processes != null && processes.Parent == null)
                    processes.Parent = this;
                return processes;
            }
            set
            {
                if (value != null && processes != value)
                {
                    processes = value;
                    Parent?.OnPropertyChanged(nameof(Processes), value, processes, this);
                }
                else
                    processes = value;
            }
        }
        /// <summary>
        /// Creates a new BpmnSubProcess that is a copy of the current BpmnSubProcess. 
        /// </summary>
        /// <returns>BpmnSubProcess</returns>
        public override object Clone()
        {
            return new BpmnSubProcess(this);
        }
    }
    /// <summary>
    /// Interface for a class BpmnAnnotation
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
    ///             Collection = false, 
    ///             Type = BpmnDataObjects.Input 
    ///         }, 
    ///         Annotation = new BpmnAnnotation() { ID = "ann1", Text = "Node1" },
    ///         Annotations = new DiagramObjectCollection<BpmnAnnotation>() 
    ///         {
    ///             new BpmnAnnotation() 
    ///             { 
    ///                 Angle = 170, 
    ///                 Length = 150, 
    ///                 Text = "left", 
    ///                 ID = "left" 
    ///             },
    ///         }
    ///     }     
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal class BpmnAnnotation : ShapeAnnotation
    {

        private int angle;
        private readonly int height;
        private readonly int width;
        private int length;
        private string nodeId = string.Empty;

        private string text = string.Empty;
        /// <summary>
        /// Creates a new instance of the BpmnAnnotation from the given BpmnAnnotation.
        /// </summary>
        /// <param name="src">BpmnAnnotation</param>
        public BpmnAnnotation(BpmnAnnotation src) : base(src)
        {
            if (src != null)
            {
                angle = src.angle;
                height = src.height;
                width = src.width;
                length = src.length;
                nodeId = src.nodeId;
                text = src.text;
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnAnnotation.
        /// </summary>
        public BpmnAnnotation() : base()
        {

        }

        /// <summary>
        /// Represents the text to be displayed in the BPMN annotation. 
        /// </summary>
        [JsonPropertyName("text")]
        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    Parent?.OnPropertyChanged(nameof(Text), value, text, this);
                    text = value;
                }
            }
        }

        /// <summary>
        /// Sets the angle between the BPMN shape and the annotation. 
        /// </summary>
        [JsonPropertyName("angle")]
        public int Angle
        {
            get => angle;
            set
            {
                if (angle != value)
                {
                    Parent?.OnPropertyChanged(nameof(Angle), value, angle, this);
                    angle = value;
                }
            }
        }

        /// <summary>
        /// Sets the distance between the BPMN shape and the annotation. 
        /// </summary>
        [JsonPropertyName("length")]
        public int Length
        {
            get => length;
            set
            {
                if (length != value)
                {
                    Parent?.OnPropertyChanged(nameof(Length), value, length, this);
                    length = value;
                }
            }
        }

        /// <summary>
        /// Sets the id of the BPMN annotation.
        /// </summary>
        [JsonPropertyName("nodeId")]
        public string NodeId
        {
            get => nodeId;
            set
            {
                if (nodeId != value)
                {
                    Parent?.OnPropertyChanged(nameof(NodeId), value, nodeId, this);
                    nodeId = value;
                }
            }
        }
        /// <summary>
        /// Creates a new Bpmn Annotation that is a copy of the current Bpmn Annotation.
        /// </summary>
        /// <returns>BpmnAnnotation</returns>
        public override object Clone()
        {
            return new BpmnAnnotation(this);
        }
    }
    /// <summary>
    /// Represents a composed activity that is included within a process in the diagram. 
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
    ///             SubProcess = new BpmnSubProcess() { Collapsed = true, Type = BpmnSubProcessTypes.Transaction, Transaction = new BpmnTransactionSubProcess() { Failure = { Visible = false }, Success = { Visible = false }, Cancel = { Visible = false } } },
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal class BpmnTransactionSubProcess : DiagramObject
    {
        private BpmnSubEvent failure = new BpmnSubEvent() { ID = "failure", Event = BpmnEvents.Intermediate, Trigger = BpmnTriggers.Error, Offset = new DiagramPoint() { X = 0.25, Y = 1 } };
        private BpmnSubEvent cancel = new BpmnSubEvent() { ID = "cancel", Event = BpmnEvents.Intermediate, Trigger = BpmnTriggers.Cancel, Offset = new DiagramPoint() { X = 0.75, Y = 1 } };

        private BpmnSubEvent success = new BpmnSubEvent() { ID = "success", Event = BpmnEvents.End, Offset = new DiagramPoint() { X = 1, Y = 0.5 } };
        /// <summary>
        /// Creates a new instance of the BpmnTransactionSubProcess from the given BpmnTransactionSubProcess.
        /// </summary>
        /// <param name="src">BpmnTransactionSubProcess</param>
        public BpmnTransactionSubProcess(BpmnTransactionSubProcess src) : base(src)
        {
            if (src != null)
            {
                if (src.failure != null)
                {
                    failure = src.failure.Clone() as BpmnSubEvent;
                }
                if (src.cancel != null)
                {
                    cancel = src.cancel.Clone() as BpmnSubEvent;
                }
                if (src.success != null)
                {
                    success = src.success.Clone() as BpmnSubEvent;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnTransactionSubProcess.
        /// </summary>
        public BpmnTransactionSubProcess() : base()
        {
        }

        /// <summary>
        /// Represents the fact that the flow can be terminated by the end node even if it encounters an error.
        /// </summary>
        [JsonPropertyName("success")]
        public BpmnSubEvent Success
        {
            get => success;
            set
            {
                if (success != value)
                {
                    Parent?.OnPropertyChanged(nameof(Success), value, success, this);
                    success = value;
                }
            }
        }

        /// <summary>
        /// Represents if a subprocess finishes with an error event ,then it is not performed.
        /// </summary>
        [JsonPropertyName("failure")]
        public BpmnSubEvent Failure
        {
            get => failure;
            set
            {
                if (failure != value)
                {
                    Parent?.OnPropertyChanged(nameof(Failure), value, failure, this);
                    failure = value;
                }
            }
        }

        /// <summary>
        /// Represents a transaction is canceled if the execution reaches the cancelled end event. 
        /// </summary>
        [JsonPropertyName("cancel")]
        public BpmnSubEvent Cancel
        {
            get => cancel;
            set
            {
                if (cancel != value)
                {
                    Parent?.OnPropertyChanged(nameof(Cancel), value, cancel, this);
                    cancel = value;
                }
            }
        }
        /// <summary>
        /// Creates a new BpmnTransactionSubProcess that is a copy of the current BpmnTransactionSubProcess. 
        /// </summary>
        /// <returns>BpmnTransactionSubProcess</returns>
        public override object Clone()
        {
            return new BpmnTransactionSubProcess(this);
        }
    }
    /// <summary>
    /// In BPMN, the events are expressed as circles in the diagram. 
    /// </summary>
    /// <remarks>
    /// When an event occurs at the start, finish, or middle of a process, it is referred to as an event.
    /// </remarks>
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
    internal class BpmnEvent : DiagramObject
    {

        private BpmnEvents shape = BpmnEvents.Start;
        private BpmnTriggers trigger = BpmnTriggers.None;
        /// <summary>
        /// Creates a new instance of the BpmnEvent from the given BpmnEvent.
        /// </summary>
        /// <param name="src">BpmnEvent</param>
        public BpmnEvent(BpmnEvent src) : base(src)
        {
            if (src != null)
            {
                shape = src.shape;
                trigger = src.trigger;
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnEvent.
        /// </summary>
        public BpmnEvent() : base()
        {
        }

        /// <summary>
        /// Represents the type of the event shape. 
        /// </summary>
        [JsonPropertyName("shape")]
        public BpmnEvents Shape
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
        /// Represents the type of the trigger. 
        /// </summary>
        [JsonPropertyName("trigger")]
        public BpmnTriggers Trigger
        {
            get => trigger;
            set
            {
                if (trigger != value)
                {
                    Parent?.OnPropertyChanged(nameof(Trigger), value, trigger, this);
                    trigger = value;
                }
            }
        }
        /// <summary>
        /// Creates a new BpmnEvent that is a copy of the current BpmnEvent. 
        /// </summary>
        /// <returns>BpmnEvent</returns>
        public override object Clone()
        {
            return new BpmnEvent(this);
        }
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
    internal class BpmnGateway : DiagramObject
    {
        private BpmnGateways type = BpmnGateways.None;
        /// <summary>
        /// Creates a new instance of the BpmnGateway from the given BpmnGateway.
        /// </summary>
        /// <param name="src">BpmnGateway</param>
        public BpmnGateway(BpmnGateway src) : base(src)
        {
            if (src != null)
            {
                type = src.type;
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnGateway.
        /// </summary>
        public BpmnGateway() : base()
        {
        }

        /// <summary>
        /// Represents the type of the BPMN Gateway. 
        /// </summary>
        [JsonPropertyName("type")]
        public BpmnGateways Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                    type = value;
                }
            }
        }
        /// <summary>
        /// Creates a new BpmnGateway that is a copy of the current BpmnGateway. 
        /// </summary>
        /// <returns>BpmnGateway</returns>
        public override object Clone()
        {
            return new BpmnGateway(this);
        }

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
    internal class BpmnDataObject : DiagramObject
    {
        private BpmnDataObjects type = BpmnDataObjects.None;
        /// <summary>
        /// Creates a new instance of the BpmnDataObject from the given BpmnDataObject.
        /// </summary>
        /// <param name="src">BpmnDataObject</param>
        public BpmnDataObject(BpmnDataObject src) : base(src)
        {
            if (src != null)
            {
                type = src.type;
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnDataObject.
        /// </summary>
        public BpmnDataObject() : base()
        {
        }

        private bool collection;

        /// <summary>
        /// Represents the type of the BPMN data object. 
        /// </summary>
        [JsonPropertyName("type")]
        public BpmnDataObjects Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                    type = value;

                }
            }
        }

        /// <summary>
        ///  Represents whether the data object is a collection of information or not. 
        /// </summary>
        [JsonPropertyName("collection")]
        public bool Collection
        {
            get => collection;
            set
            {
                if (collection != value)
                {
                    Parent?.OnPropertyChanged(nameof(Collection), value, collection, this);
                    collection = value;
                }
            }
        }
        /// <summary>
        /// Creates a new BPMN data object that is a copy of the current BPMN data object. 
        /// </summary>
        /// <returns>BpmnDataObject</returns>
        public override object Clone()
        {
            return new BpmnDataObject(this);
        }
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
    ///             Task = new BpmnTask() { Call = true } 
    ///         } 
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal class BpmnTask : DiagramObject
    {

        private BpmnTasks type = BpmnTasks.None;
        private BpmnLoops loop = BpmnLoops.None;
        private bool call;
        private bool compensation;
        /// <summary>
        /// Creates a new instance of the BpmnTask from the given BpmnTask.
        /// </summary>
        /// <param name="src">BpmnTask</param>
        public BpmnTask(BpmnTask src) : base(src)
        {
            if (src != null)
            {
                type = src.type;
                loop = src.loop;
                call = src.call;
                compensation = src.compensation;
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnTask.
        /// </summary>
        public BpmnTask() : base()
        {
        }

        /// <summary>
        /// Represents the type of the BPMNTask. 
        /// </summary>
        [JsonPropertyName("type")]
        public BpmnTasks Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                    type = value;
                }
            }
        }

        /// <summary>
        /// Represents that the sub-process repeats itself in sequence. 
        /// </summary>
        [JsonPropertyName("loop")]
        public BpmnLoops Loop
        {
            get => loop;
            set
            {
                if (loop != value)
                {
                    Parent?.OnPropertyChanged(nameof(Loop), value, loop, this);
                    loop = value;
                }
            }
        }

        /// <summary>
        /// Represents an Activity defined in a process that is external to the current process definition. 
        /// </summary>
        [JsonPropertyName("call")]
        public bool Call
        {
            get => call;
            set
            {
                if (call != value)
                {
                    Parent?.OnPropertyChanged(nameof(Call), value, call, this);
                    call = value;
                }
            }
        }

        /// <summary>
        /// Represents a collection of tasks that describe some parts of the compensation method. 
        /// </summary>
        [JsonPropertyName("compensation")]
        public bool Compensation
        {
            get => compensation;
            set
            {
                if (compensation != value)
                {
                    Parent?.OnPropertyChanged(nameof(Compensation), value, compensation, this);
                    compensation = value;
                }
            }
        }
        /// <summary>
        /// Creates a new BpmnTask that is a copy of the current BpmnTask. 
        /// </summary>
        /// <returns>BpmnTask</returns>
        public override object Clone()
        {
            return new BpmnTask(this);
        }
    }
    /// <summary>
    /// Represents the work that a company or organization performs in a business process using a node in the diagram. 
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
    internal class BpmnActivity : DiagramObject
    {
        private BpmnActivities activity = BpmnActivities.Task;
        private BpmnTask task = new BpmnTask();
        private BpmnSubProcess subProcess = new BpmnSubProcess();
        /// <summary>
        /// Creates a new instance of the BpmnActivity from the given BpmnActivity.
        /// </summary>
        /// <param name="src">BpmnActivity</param>
        public BpmnActivity(BpmnActivity src) : base(src)
        {
            if (src != null)
            {
                activity = src.activity;
                if (src.task != null)
                {
                    task = src.task.Clone() as BpmnTask;
                }
                if (src.subProcess != null)
                {
                    subProcess = src.subProcess.Clone() as BpmnSubProcess;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the BpmnActivity.
        /// </summary>
        public BpmnActivity() : base()
        {

        }

        /// <summary>
        /// Represents the process that is external to the current process definition. 
        /// </summary>
        /// <remarks>
        /// It allows you to create a reusable process definition that can be reused in multiple other process definitions. 
        /// </remarks>
        [JsonPropertyName("activity")]
        internal BpmnActivities Activity
        {
            get => activity;
            set
            {
                if (activity != value)
                {
                    Parent?.OnPropertyChanged(nameof(Activity), value, activity, this);
                    activity = value;
                }
            }
        }

        /// <summary>
        /// Represents the activity within a process flow. we can create a task when the activity cannot be broken down to a finer level of detail. 
        /// </summary>
        [JsonPropertyName("task")]
        internal BpmnTask Task
        {
            get
            {
                if (task != null && task.Parent == null)
                    task.SetParent(this, nameof(Task));
                return task;
            }
            set
            {
                if (task != value)
                {
                    Parent?.OnPropertyChanged(nameof(Task), value, task, this);
                    task = value;
                }
            }
        }

        /// <summary>
        /// Represents the compound activity that represents a collection of other tasks and sub-processes
        /// </summary>
        /// <remarks>
        /// We can split a complex process into multiple levels, which allows you to focus on a particular area in a single process diagram.
        /// </remarks>
        [JsonPropertyName("subProcess")]
        internal BpmnSubProcess SubProcess
        {
            get
            {
                if (subProcess != null && subProcess.Parent == null)
                    subProcess.SetParent(this, nameof(SubProcess));
                return subProcess;
            }
            set
            {
                if (subProcess != value)
                {
                    Parent?.OnPropertyChanged(nameof(SubProcess), value, subProcess, this);
                    subProcess = value;
                }
            }
        }
        /// <summary>
        /// Creates a new Bpmn Activity that is a copy of the current Bpmn Activity. 
        /// </summary>
        /// <returns>BpmnActivity</returns>
        public override object Clone()
        {
            return new BpmnActivity(this);
        }
    }
    /// <summary>
    /// Represents the behaviour of the BpmnSubEvent in the diagram. 
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
    ///             Event = BpmnEvents.Start, 
    ///             Trigger = BpmnTriggers.None 
    ///         }
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    internal class BpmnSubEvent : DiagramObject
    {
        private string id;
        private BpmnEvents events = BpmnEvents.Start;
        private BpmnTriggers trigger = BpmnTriggers.None;
        private DiagramPoint offset = new DiagramPoint() { X = 0.5, Y = 0.5 };
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Center;
        private DiagramObjectCollection<PointPort> ports = new DiagramObjectCollection<PointPort>();
        private DiagramObjectCollection<ShapeAnnotation> annotations = new DiagramObjectCollection<ShapeAnnotation>();
        private double? height;
        private double? width;
        private Margin margin = new Margin();
        private bool visible = true;
        private DiagramContainer wrapper;

        /// <summary>
        /// Initializes a new instance of the BpmnSubEvent.
        /// </summary>
        public BpmnSubEvent() : base()
        {
            id = BaseUtil.RandomId();
            ports.Parent = this;
            annotations.Parent = this;
        }
        /// <summary>
        /// Creates a new instance of the BpmnSubEvent from the given BpmnSubEvent.
        /// </summary>
        /// <param name="src">BpmnSubEvent</param>
        public BpmnSubEvent(BpmnSubEvent src) : base(src)
        {
            if (src != null)
            {
                id = string.IsNullOrEmpty(src.ID) ? BaseUtil.RandomId() : src.ID;
                events = src.events;
                trigger = src.trigger;
                if (src.offset != null)
                {
                    offset = src.offset.Clone() as DiagramPoint;
                }
                horizontalAlignment = src.horizontalAlignment;
                verticalAlignment = src.verticalAlignment;
                annotations = src.annotations;
                ports = src.ports;
                height = src.height;
                width = src.width;
                visible = src.visible;
                if (src.wrapper != null)
                {
                    wrapper = src.wrapper.Clone() as DiagramContainer;
                }
                if (src.margin != null)
                {
                    margin = src.margin.Clone() as Margin;
                }
                ports = new DiagramObjectCollection<PointPort>();
                if (src.ports.Count > 0)
                {
                    foreach (PointPort pointPort in src.ports)
                    {
                        PointPort port = pointPort.Clone() as PointPort;
                        ports.Add(port);
                    }
                }
                ports.Parent = this;
                annotations = new DiagramObjectCollection<ShapeAnnotation>();

                if (src.annotations.Count > 0)
                {
                    foreach (ShapeAnnotation annotation in src.annotations)
                    {
                        ShapeAnnotation shapeAnnotation = annotation.Clone() as ShapeAnnotation;
                        annotations.Add(shapeAnnotation);
                    }
                }
                annotations.Parent = this;
            }
        }
        /// <summary>
        ///  Represents the unique id of the diagram object. 
        /// </summary>
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
        /// Sets or gets the UI of a node/connector
        /// </summary>
        [JsonIgnore]
        internal DiagramContainer Wrapper
        {
            get => wrapper;
            set
            {
                if (wrapper != value)
                {
                    wrapper = value;
                }
            }
        }
        /// <summary>
        /// Sets the type of the BPMN Event. 
        /// </summary>
        [JsonPropertyName("event")]
        public BpmnEvents Event
        {
            get => events;
            set
            {
                if (events != value)
                {
                    Parent?.OnPropertyChanged(nameof(Event), value, events, this);
                    events = value;
                }
            }
        }

        /// <summary>
        /// Defines the type of the trigger.
        /// </summary>
        [JsonPropertyName("trigger")]
        public BpmnTriggers Trigger
        {
            get => trigger;
            set
            {
                if (trigger != value)
                {
                    Parent?.OnPropertyChanged(nameof(Trigger), value, trigger, this);
                    trigger = value;
                }
            }
        }
        /// <summary>
        /// Defines the position of the sub event.
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
        /// Gets or sets the width of the node. If it is not specified, the node renders based on the content's width.
        /// </summary>
        [JsonPropertyName("width")]
        public double? Width
        {
            get => width;
            set
            {
                if (!Equals(width, value))
                {
                    //IsDirtNode = true;
                    Parent?.OnPropertyChanged(nameof(Width), value, width, this);
                    width = value;
                }
            }
        }

        /// <summary>
        ///  Gets or sets the height of the node. If it is not specified, the node renders based on the content's height. 
        /// </summary>
        [JsonPropertyName("height")]
        public double? Height
        {
            get => height;
            set
            {
                if (!Equals(height, value))
                {
                    Parent?.OnPropertyChanged(nameof(Height), value, height, this);
                    height = value;
                }
            }
        }
        /// <summary>
        /// Sets the visibility of the sub event.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible
        {
            get => visible;
            set
            {
                if (!visible.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Visible), value, visible, this);
                    visible = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets a collection of PointPort (connection points). 
        /// </summary>
        /// <remarks>
        /// Ports act as the connection points between the nodes and allow them to create connections only with those specific points. There may be any number of ports in a node. You can   modify the port’s appearance, visibility, positioning and can add custom shapes to them. 
        /// </remarks>
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
                    ports?.ClearItemElements(ports);
                    ports = value;
                    ports.Parent = this;
                    if (this.Parent != null && Wrapper != null)
                    {
                        Parent.OnPropertyChanged(nameof(Ports), value, ports, this);
                    }
                }
                else
                    ports = value;
            }
        }
        /// <summary>
        /// Represents the collection of textual information contained in the node. 
        /// </summary>
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
                    annotations?.ClearItemElements(annotations);
                    annotations = value;
                    annotations.Parent = this;
                    if (this.Parent != null && Wrapper != null)
                    {
                        Parent.OnPropertyChanged(nameof(Annotations), value, annotations, this);
                    }
                }
                else
                    annotations = value;
            }
        }
        /// <summary>
        ///  Gets or sets the horizontal alignment of the node. 
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
        /// Defines the space to be left between the node and its immediate parent.
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
        /// Creates a new BpmnSubEvent that is a copy of the current BpmnSubEvent. 
        /// </summary>
        /// <returns>BpmnSubEvent</returns>
        public override object Clone()
        {
            return new BpmnSubEvent(this);
        }
    }
}


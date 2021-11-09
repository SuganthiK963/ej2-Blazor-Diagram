using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the segment of the connector.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Connectors="@connectors">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///   public DiagramObjectCollection<Connector> connectors = new DiagramObjectCollection<Connector>();
    ///   protected override void OnInitialized()
    ///   {
    ///     Connector connector = new Connector()
    ///     {
    ///        ID="straight",
    ///        SourcePoint = new DiagramPoint() { X = 100, Y = 200 },
    ///        TargetPoint = new DiagramPoint() { X = 300, Y = 200 },
    ///        Segments = new DiagramObjectCollection<ConnectorSegment>()
    ///        {
    ///            //Create a new straight segment 
    ///            new StraightSegment(){Point=new DiagramPoint(420,300)},
    ///         }
    ///     };
    ///    connectors.Add(connector);
    ///   }
    ///  }
    /// ]]>
    /// </code>
    /// </example>

    [JsonConverter(typeof(ConnectorSegmentJsonConverter))]
    public class ConnectorSegment : DiagramObject
    {
        internal string ID { get; set; }
        private ConnectorSegmentType type = ConnectorSegmentType.Straight;
        private bool allowDrag = true;
        [JsonIgnore]
        internal List<DiagramPoint> Points { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="ConnectorSegment"/> from the given <see cref="ConnectorSegment"/>.
        /// </summary>
        /// <param name="src">ConnectorSegment</param>
        public ConnectorSegment(ConnectorSegment src) : base(src)
        {
            if (src != null)
            {
                ID = string.IsNullOrEmpty(src.ID) ? BaseUtil.RandomId() : src.ID;
                allowDrag = src.allowDrag;
                type = src.type;
                Points = new List<DiagramPoint>();
                if (src.Points != null && src.Points.Count > 0)
                {
                    foreach (DiagramPoint point in src.Points)
                    {
                        DiagramPoint point1 = point.Clone() as DiagramPoint;
                        Points.Add(point1);
                    }
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorSegment"/>.
        /// </summary>
        public ConnectorSegment()
        {
            ID = BaseUtil.RandomId();
        }
        /// <summary>
        /// Defines the type of the segment.
        /// </summary>
        [JsonPropertyName("type")]
        public ConnectorSegmentType Type
        {
            get => type;
            set
            {
                Parent?.OnPropertyChanged(nameof(Type), value, type, this);
                type = value;
            }
        }

        /// <summary>
        /// Defines whether the segment can be dragged or not.
        /// </summary>
        [JsonPropertyName("allowDrag")]
        public bool AllowDrag
        {
            get => allowDrag;
            set
            {
                if (allowDrag != value)
                {
                    Parent?.OnPropertyChanged(nameof(AllowDrag), value, allowDrag, this);
                    allowDrag = value;
                }
            }
        }
        /// <summary>
        /// Creates a new segment that is a copy of the current segment.
        /// </summary>
        /// <returns>ConnectorSegment</returns>
        public override object Clone()
        {
            return new ConnectorSegment(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (ID != null)
            {
                ID = null;
            }
            if (Points != null)
            {
                Points.Clear();
                Points = null;
            }
        }
    }
    /// <summary>
    /// Represents how a straight segment can be created for a connector.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Connectors="@connectors">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///   public DiagramObjectCollection<Connector> connectors = new DiagramObjectCollection<Connector>();
    ///   protected override void OnInitialized()
    ///   {
    ///     Connector connector = new Connector()
    ///     {
    ///        ID="straight",
    ///        SourcePoint = new DiagramPoint() { X = 100, Y = 200 },
    ///        TargetPoint = new DiagramPoint() { X = 300, Y = 200 },
    ///        Segments = new DiagramObjectCollection<ConnectorSegment>()
    ///        {
    ///            //Create a new straight segment 
    ///            new StraightSegment(){Point=new DiagramPoint(420,300)},
    ///         }
    ///     };
    ///    connectors.Add(connector);
    ///   }
    ///  }
    /// ]]>
    /// </code>
    /// </example>
    public class StraightSegment : ConnectorSegment
    {
        private DiagramPoint point = new DiagramPoint() { X = 0, Y = 0 };
        /// <summary>
        /// Creates a new instance of the StraightSegment from the given StraightSegment.
        /// </summary>
        /// <param name="src">StraightSegment</param>
        public StraightSegment(StraightSegment src) : base(src)
        {
            if (src != null && src.point != null)
            {
                point = src.point.Clone() as DiagramPoint;
            }
        }
        /// <summary>
        /// Initializes a new instance of the StraightSegment.
        /// </summary>
        public StraightSegment()
        {
        }
        /// <summary>
        /// Gets or sets the endpoint of the connector segment.
        /// </summary>
        [JsonPropertyName("point")]
        public DiagramPoint Point
        {
            get
            {
                if (point != null && point.Parent == null)
                    point.SetParent(this, nameof(Point));
                return point;
            }
            set
            {
                if (point != value)
                {
                    Parent?.OnPropertyChanged(nameof(Point), value, point, this);
                    point = value;
                }
            }
        }
        /// <summary>
        /// Creates a new segment that is a copy of the current straight segment.
        /// </summary>
        /// <returns>StraightSegment</returns>
        public override object Clone()
        {
            return new StraightSegment(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (point != null)
            {
                point.Dispose();
                point = null;
            }
        }

    }
    /// <summary>
    /// Represents the bezier segment of the connector.
    /// </summary>
    public class BezierSegment : StraightSegment
    {
        [JsonIgnore]
        internal DiagramPoint BezierPoint1 { get; set; }
        [JsonIgnore]
        internal DiagramPoint BezierPoint2 { get; set; }
        private DiagramPoint point1 = new DiagramPoint { X = 0, Y = 0 };
        private DiagramPoint point2 = new DiagramPoint { X = 0, Y = 0 };
        private Vector vector1 = new Vector();
        private Vector vector2 = new Vector();
        /// <summary>
        /// Initializes a new instance of the BezierSegment.
        /// </summary>
        public BezierSegment()
        {
            Type = ConnectorSegmentType.Bezier;
        }
        /// <summary>
        /// Creates a new instance of the BezierSegment from the given BezierSegment.
        /// </summary>
        /// <param name="src">BezierSegment</param>
        public BezierSegment(BezierSegment src) : base(src)
        {
            if (src != null)
            {
                if (src.BezierPoint1 != null)
                {
                    BezierPoint1 = src.BezierPoint1.Clone() as DiagramPoint;
                }
                if (src.BezierPoint2 != null)
                {
                    BezierPoint2 = src.BezierPoint2.Clone() as DiagramPoint;
                }
                if (src.point1 != null)
                {
                    point1 = src.point1.Clone() as DiagramPoint;
                }
                if (src.point2 != null)
                {
                    point2 = src.point2.Clone() as DiagramPoint;
                }
                if (src.vector1 != null)
                {
                    vector1 = src.vector1.Clone() as Vector;
                }
                if (src.vector2 != null)
                {
                    vector2 = src.vector2.Clone() as Vector;
                }
            }
        }
        /// <summary>
        /// Gets or sets the first control point of the bezier connector.
        /// </summary>
        [JsonPropertyName("point1")]
        public DiagramPoint Point1
        {
            get
            {
                if (point1 != null && point1.Parent == null)
                    point1.SetParent(this, nameof(Point1));
                return point1;
            }
            set
            {
                if (point1 != value)
                {
                    Parent?.OnPropertyChanged(nameof(Point1), value, point1, this);
                    point1 = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the second control point of the bezier connector.
        /// </summary>
        [JsonPropertyName("point2")]
        public DiagramPoint Point2
        {
            get
            {
                if (point2 != null && point2.Parent == null)
                    point2.SetParent(this, nameof(Point2));
                return point2;
            }
            set
            {
                if (point2 != value)
                {
                    Parent?.OnPropertyChanged(nameof(Point2), value, point2, this);
                    point2 = value;
                }
            }
        }

        /// <summary>
        /// Defines the length and angle between the source point and the first control point of the diagram.
        /// </summary>
        [JsonPropertyName("vector1")]
        public Vector Vector1
        {
            get
            {
                if (vector1 != null && vector1.Parent == null)
                    vector1.SetParent(this, nameof(Vector1));
                return vector1;
            }
            set
            {
                if (vector1 != value)
                {
                    Parent?.OnPropertyChanged(nameof(Vector1), value, vector1, this);
                    vector1 = value;
                }
            }
        }

        /// <summary>
        /// Defines the length and angle between the target point and the second control point of the diagram.
        /// </summary>
        [JsonPropertyName("vector2")]
        public Vector Vector2
        {
            get
            {
                if (vector2 != null && vector2.Parent == null)
                    vector2.SetParent(this, nameof(Vector2));
                return vector2;
            }
            set
            {
                if (vector2 != value)
                {
                    Parent?.OnPropertyChanged(nameof(Vector2), value, vector2, this);
                    vector2 = value;
                }
            }
        }
        /// <summary>
        /// Creates a new bezier segment that is a copy of the current segment.
        /// </summary>
        /// <returns>BezierSegment</returns>
        public override object Clone()
        {
            return new BezierSegment(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (BezierPoint1 != null)
            {
                BezierPoint1.Dispose();
                BezierPoint1 = null;
            }

            if (BezierPoint2 != null)
            {
                BezierPoint2.Dispose();
                BezierPoint2 = null;
            }
            if (point1 != null)
            {
                point1.Dispose();
                point1 = null;
            }
            if (point2 != null)
            {
                point2.Dispose();
                point2 = null;
            }
            if (vector1 != null)
            {
                vector1.Dispose();
                vector1 = null;
            }
            if (vector2 != null)
            {
                vector2.Dispose();
                vector2 = null;
            }
        }
    }
    /// <summary>
    /// Describes the length and angle between the control point and the start point of the bezier segment. 
    /// </summary>
    public class Vector : DiagramObject
    {
        private double angle;
        private double distance;
        /// <summary>
        /// Creates a new instance of the Vector from the given Vector.
        /// </summary>
        /// <param name="src">Vector</param>
        public Vector(Vector src) : base(src)
        {
            if (src != null)
            {
                angle = src.angle;
                distance = src.distance;
            }
        }
        /// <summary>
        /// Initializes a new instance of the Vector.
        /// </summary>
        public Vector()
        {
        }

        /// <summary>
        /// Defines the angle between the connector endpoint and control point of the bezier segment. 
        /// </summary>
        [JsonPropertyName("angle")]
        public double Angle
        {
            get => angle;
            set
            {
                if (!angle.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Angle), value, angle, this);
                    angle = value;
                }
            }
        }

        /// <summary>
        /// Defines the distance between the connector endpoint and control point of the bezier segment.
        /// </summary>
        [JsonPropertyName("distance")]
        public double Distance
        {
            get => distance;
            set
            {
                if (!distance.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Distance), value, distance, this);
                    distance = value;
                }
            }
        }
        /// <summary>
        /// Creates a new vector that is the copy of the current vector.
        /// </summary>
        /// <returns>Vector</returns>
        public override object Clone()
        {
            return new Vector(this);
        }

        internal override void Dispose()
        {
            angle = 0;
            distance = 0;
        }
    }
    /// <summary>
    /// Represents how an orthogonal segment can be created with length and direction.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Segments = new DiagramObjectCollection<ConnectorSegment>()
    /// {
    ///     new OrthogonalSegment 
    ///     {
    ///         Length = 100,
    ///         Type = ConnectorSegmentType.Orthogonal,
    ///         Direction = Direction.Right, 
    ///      },
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class OrthogonalSegment : ConnectorSegment
    {
        private double? length;
        private Direction? direction;
        /// <summary>
        /// Creates a new <see cref="OrthogonalSegment "/> from the given <see cref="OrthogonalSegment "/>.
        /// </summary>
        /// <param name="src">OrthogonalSegment</param>
        public OrthogonalSegment(OrthogonalSegment src) : base(src)
        {
            if (src != null)
            {
                length = src.length;
                direction = src.direction;
            }
        }
        /// <summary>
        /// Initializes a new instance of the OrthogonalSegment.
        /// </summary>
        public OrthogonalSegment()
        {
            Type = ConnectorSegmentType.Orthogonal;
        }
        /// <summary>
        /// Gets or sets the length of orthogonal segment , by default it is null.
        /// </summary>
        [JsonPropertyName("length")]
        public double? Length
        {
            get => length;
            set
            {
                if (!length.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Length), value, length, this);
                    length = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the direction of the orthogonal segment.
        /// </summary>
        /// <remarks>
        /// <table style = "border: none">
        /// <tr>
        /// <td style = "border: none">1.Left, Sets the connector segment direction as left.</td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">2.Right, Sets the connector segment direction as right. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">3.Top, Sets the connector segment direction as top. </td>
        /// </tr>
        /// <tr>
        /// <td style = "border: none">4.Bottom, Sets the connector segment direction as bottom. </td>
        /// </tr>
        /// </table>
        /// </remarks>
        [JsonPropertyName("direction")]
        public Direction? Direction
        {
            get => direction;
            set
            {
                if (!direction.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Direction), value, direction, this);
                    direction = value;
                }
            }
        }
        /// <summary>
        /// Creates a new orthogonal segment that is a copy of the current segment.
        /// </summary>
        /// <returns>OrthogonalSegment</returns>
        public override object Clone()
        {
            return new OrthogonalSegment(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (direction != null)
            {
                direction = null;
            }
            if (length != null)
            {
                length = null;
            }
        }
    }
}

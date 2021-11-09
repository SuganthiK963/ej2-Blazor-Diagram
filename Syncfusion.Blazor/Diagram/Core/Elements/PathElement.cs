using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the class which defines how to align the path based on offsetX and offsetY.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" @bind-Nodes="Nodes" SetNodeTemplate="SetTemplate">
    /// </SfDiagramComponent>    
    /// @code
    /// { 
    ///   DiagramObjectCollection<Node> Nodes = new DiagramObjectCollection<Node>();
    ///   private ICommonElement SetTemplate(IDiagramObject node)
    ///   {
    ///     if ((node as Node).ID == "node2")
    ///     {
    ///       Canvas container = new Canvas();
    ///       PathElement diagramElement = new PathElement();
    ///       diagramElement.Style.Fill = "green";
    ///       diagramElement.Data = "M150 0 L75 200 L225 200 Z";
    ///       container.Children.Add(diagramElement);
    ///       return container;
    ///     }
    ///     return null;
    ///   }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class PathElement : DiagramElement
    {
        private object pointTimer;
        private string pathData = string.Empty;
        [JsonIgnore]
        internal List<DiagramPoint> Points { get; set; } = new List<DiagramPoint>() { };
        [JsonIgnore]
        internal bool CanMeasurePath { get; set; }
        [JsonIgnore]
        internal bool IsBpmnSequenceDefault { get; set; }
        [JsonIgnore]
        internal DiagramRect AbsoluteBounds { get; set; } = new DiagramRect();
        /// <summary>
        /// Creates a new instance of the <see cref="PathElement"/> from the given PathElement.
        /// </summary>
        /// <param name="src">path element.</param>
        public PathElement(PathElement src) : base(src)
        {
            if (src != null)
            {
                pathData = src.pathData;
                CanMeasurePath = src.CanMeasurePath;
                IsBpmnSequenceDefault = src.IsBpmnSequenceDefault;
                if (src.AbsoluteBounds != null)
                {
                    AbsoluteBounds = src.AbsoluteBounds.Clone() as DiagramRect;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathElement"/> class.
        /// </summary>
        public PathElement() : base()
        {
        }
        /// <summary>
        /// Gets or sets the geometry of the path element.
        /// </summary>
        [JsonPropertyName("data")]
        public string Data
        {
            get => this.pathData;
            set
            {
                if (this.pathData != value)
                {
                    this.pathData = value;
                    this.IsDirt = true;
                }
            }

        }
        /// <summary>
        /// Gets or sets whether the path has to be transformed to fit the given x,y, width, height.
        /// </summary>
        [JsonPropertyName("transformPath")]
        public bool TransformPath { get; set; } = true;

        /// <summary>
        /// Gets or sets the equivalent path, that will have the origin as 0,0.
        /// </summary>
        [JsonPropertyName("absolutePath")]
        public string AbsolutePath { get; set; } = string.Empty;

        internal List<DiagramPoint> GetPoints()
        {
            if (this.pointTimer != null)
            {
                Task.Delay(200);
                this.Points = null;
                this.pointTimer = null;
            }
            this.Points = DiagramUtil.GetPathPoints(this);
            return DomUtil.TranslatePoints(this, this.Points);
        }
        /// <summary>
        /// Measures the minimum space that is required to render the element
        /// </summary>
        internal override DiagramSize Measure(DiagramSize availableSize)
        {
            //Performance issue - Avoiding measuring the connector path
            if (this.StaticSize && this.Width != null && this.Height != null)
            {
                this.AbsoluteBounds = new DiagramRect
                (
                    this.OffsetX - BaseUtil.GetDoubleValue(this.Width) * this.Pivot.X,
                    this.OffsetY - BaseUtil.GetDoubleValue(this.Height) * this.Pivot.Y,
                    BaseUtil.GetDoubleValue(this.Width),
                    BaseUtil.GetDoubleValue(this.Height)
                );
            }
            else if (AbsoluteBounds != null && (this.IsDirt && !IsBpmnSequenceDefault && (this.TransformPath || (this.Width == null || this.Height == null))
                && AbsoluteBounds.Height == 0 || this.CanMeasurePath))
            {
                //Measure the element only when the path data is changed/ size is not specified
                this.AbsoluteBounds = DomUtil.MeasurePath(!string.IsNullOrEmpty(this.Data) ? this.Data : string.Empty);
            }
            if (this.Width == null)
            {
                DiagramRect absoluteBounds = this.AbsoluteBounds;
                if (absoluteBounds != null)
                    this.DesiredSize = new DiagramSize()
                    {
                        Width = absoluteBounds.Width,
                        Height = this.Height != null ? BaseUtil.GetDoubleValue(this.Height) : absoluteBounds.Height
                    };
            }
            else if (this.Height == null)
            {
                DiagramRect absoluteBounds = this.AbsoluteBounds;
                if (absoluteBounds != null)
                    this.DesiredSize = new DiagramSize()
                    { Width = BaseUtil.GetDoubleValue(this.Width), Height = absoluteBounds.Height };
            }
            else
            {
                this.DesiredSize = new DiagramSize() { Width = BaseUtil.GetDoubleValue(this.Width), Height = BaseUtil.GetDoubleValue(this.Height) };
            }
            this.DesiredSize = this.ValidateDesiredSize(this.DesiredSize, availableSize);
            this.CanMeasurePath = false;
            DiagramSize desiredSize = this.DesiredSize;
            return desiredSize;
        }

        /// <summary>
        /// Arranges the path element
        /// </summary>
        internal override DiagramSize Arrange(DiagramSize desiredSize, bool? isStack)
        {
            if (ActualSize != null && (this.IsDirt || !Equals(ActualSize.Width, desiredSize.Width) || !Equals(ActualSize.Height, desiredSize.Height)))
            {
                this.IsDirt = true;
                this.AbsolutePath = this.UpdatePath(this.Data, this.AbsoluteBounds, desiredSize);
                if (!this.StaticSize)
                {
                    this.Points = null;
                }
            }
            this.ActualSize = this.DesiredSize;
            this.UpdateBounds();
            this.IsDirt = false;
            return this.ActualSize;
        }

        /// <summary>
        /// Translates the path to 0,0 and scales the path based on the actual size
        /// </summary>
        internal string UpdatePath(string path, DiagramRect bounds, DiagramSize actualSize)
        {
            bool isScale = false; string newPathString;
            double scaleX = -bounds.X;
            double scaleY = -bounds.Y;
            if (actualSize != null && (!Equals(actualSize.Width, bounds.Width) || !Equals(actualSize.Height, bounds.Height)))
            {
                scaleX = BaseUtil.GetDoubleValue(actualSize.Width) / (bounds.Width != 0 ? bounds.Width : 1);
                scaleY = BaseUtil.GetDoubleValue(actualSize.Height) / (bounds.Height != 0 ? bounds.Height : 1);
                isScale = true;
            }
            List<PathSegment> arrayCollection = PathUtil.ProcessPathData(path);
            arrayCollection = PathUtil.SplitArrayCollection(arrayCollection);
            if ((isScale || this.IsDirt) && this.TransformPath)
            {
                newPathString = PathUtil.TransformPath(arrayCollection, scaleX, scaleY, isScale, bounds.X, bounds.Y, 0, 0);
            }
            else
            {
                newPathString = PathUtil.GetPathString(arrayCollection);
            }
            return newPathString;
        }

        /// <summary>
        /// Creates a new element that is a copy of the current path element
        /// </summary>
        public override object Clone()
        {
            return new PathElement(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (this.Points != null)
            {
                for (int i = 0; i < this.Points.Count; i++)
                {
                    this.Points[i].Dispose();
                    this.Points[i] = null;
                }
                this.Points.Clear();
                this.Points = null;
            }
            if (AbsoluteBounds != null)
            {
                AbsoluteBounds.Dispose();
                AbsoluteBounds = null;
            }

            pathData = null;
            pointTimer = null;
            AbsolutePath = null;
        }
    }
}
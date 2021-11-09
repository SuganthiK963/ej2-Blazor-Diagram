using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents a visual representation of the selected elements. It behaves like a container for single or multiple selected elements. 
    /// </summary>
    public class DiagramSelectionSettings : IDiagramObject
    {
        private DiagramObjectCollection<UserHandle> userHandles = new DiagramObjectCollection<UserHandle>() { };
        [JsonIgnore]
        internal DiagramContainer Wrapper { get; set; }
        internal IDiagramObject Parent { get; set; }
        internal ThumbsConstraints ThumbsConstraints { get; set; }
        internal DiagramRect RubberBandBounds { get; set; } = new DiagramRect(0, 0, 0, 0);
        internal bool IsRubberBandSelection { get; set; }

        /// <summary>
        /// Specifies the collection of selected nodes.
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<Node> Nodes { get; set; } = new ObservableCollection<Node>();
        /// <summary>
        /// Specifies the collection of selected connectors.
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<Connector> Connectors { get; set; } = new ObservableCollection<Connector>();

        /// <summary>
        /// Gets or sets the width of the selection region. Its value differs based on the selected region.
        /// </summary>  
        [JsonPropertyName("width")]
        public double Width { get; set; }
        /// <summary>
        /// Gets or sets the height of the selected region.
        /// </summary>  
        [JsonPropertyName("height")]
        public double Height { get; set; }
        /// <summary>
        /// Gets or sets the X-coordinate of the selected region. Its value may vary based on the selected region.
        /// </summary>  
        [JsonPropertyName("offsetX")]
        public double OffsetX { get; set; }
        /// <summary>
        /// Gets or sets the Y-coordinate of the selected region. Its value differs based on the selected region.
        /// </summary>  
        [JsonPropertyName("offsetY")]
        public double OffsetY { get; set; }
        /// <summary>
        /// Gets or sets the angle at which the node should be rotated. 
        /// </summary>  
        [JsonPropertyName("rotateAngle")]
        public double RotationAngle { get; set; }
        /// <summary>
        /// Gets or sets the ratio/fractional value relative to the node, based on which the node will be transformed (positioning, scaling, and rotation). 
        /// </summary>  
        [JsonPropertyName("pivot")]
        public DiagramPoint Pivot { get; set; } = new DiagramPoint() { X = 0.5, Y = 0.5 };
        
        /// <summary>
        /// Defines the collection of UserHandles.
        /// </summary>  
        [JsonPropertyName("userHandles")]
        public DiagramObjectCollection<UserHandle> UserHandles
        {
            get => userHandles;
            set
            {
                if (value != null && userHandles != value)
                {
                    userHandles = value;
                    userHandles.Parent = this;
                }
                else
                    userHandles = value;
            }
        }
        /// <summary>
        /// Specifies whether the diagram objects can be selected when the selection region intersects with the objects or only when the complete object's bounds are within the selection region.
        /// </summary> 
        [JsonPropertyName("rubberBandSelectionMode")]
        public RubberBandSelectionMode RubberBandSelectionMode { get; set; } = RubberBandSelectionMode.CompleteIntersect;
        /// <summary>
        /// Enables or disables certain behaviors of the selector.  
        /// </summary>
        [JsonPropertyName("constraints")]
        public SelectorConstraints Constraints { get; set; } = SelectorConstraints.All;
        internal DiagramContainer Init(SfDiagramComponent diagram)
        {
            DiagramContainer container = new DiagramContainer
            {
                MeasureChildren = false,
                Children = new ObservableCollection<ICommonElement>()
            };
            if (this.Nodes.Count > 0 || this.Connectors.Count > 0)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    Node node = this.Nodes[i];
                    DiagramContainer wrapper = node.Wrapper;
                    container.Children.Add(wrapper);
                }
                for (int j = 0; j < this.Connectors.Count; j++)
                {
                    Connector connector = this.Connectors[j];
                    DiagramContainer wrapper = connector.Wrapper;
                    container.Children.Add(wrapper);
                }
            }
            this.Wrapper = container;
            Parent = diagram;
            return container;
        }
        /// <summary>
        /// Invoked when the effective value of any property on this selector has been updated.
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="container">IDiagramObject</param>
        public async void OnPropertyChanged(string propertyName, object newVal, object oldVal, IDiagramObject container)
        {
            if (this.UserHandles.Count > 0 && this.Parent != null)
            {
                Dictionary<string, string> measurePathDataCollection = new Dictionary<string, string>() { };
                foreach (UserHandle handle in this.UserHandles)
                {
                    if (!string.IsNullOrEmpty(handle.PathData) && Dictionary.GetMeasurePathBounds(handle.PathData) == null)
                    {
                        DiagramLayerContent.AddMeasurePathDataCollection(handle.PathData, measurePathDataCollection);
                    }
                }
                if (measurePathDataCollection.Count > 0)
                {
                    await DomUtil.MeasureBounds(measurePathDataCollection, null, null, null);
                    foreach (string key in measurePathDataCollection.Keys)
                    {
                        if (Dictionary.GetMeasurePathBounds(key) != null)
                        {
                            (Parent as SfDiagramComponent)?.DiagramStateHasChanged();
                        }
                    }
                    measurePathDataCollection.Clear();
                }
            }
            if (container is UserHandle && this.Parent != null)
            {
                DiagramRenderer.RenderSelector(new SelectionFragmentParameter { Selector = this, Transform = ((SfDiagramComponent)this.Parent).Scroller.Transform });
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramSelectionSettings"/>.
        /// </summary>
        public DiagramSelectionSettings()
        {

        }
        /// <summary>
        /// Creates a new instance of the Selector from the given Selector.
        /// </summary>
        /// <param name="src">Selector</param>
        public DiagramSelectionSettings(DiagramSelectionSettings src)
        {
            if (src != null)
            {
                UserHandles = src.UserHandles;
                OffsetY = src.OffsetY;
                Wrapper = src.Wrapper;
                Width = src.Width;
                Height = src.Height;
                OffsetX = src.OffsetX;
                RotationAngle = src.RotationAngle;
                Parent = src.Parent;
                RubberBandSelectionMode = src.RubberBandSelectionMode;
                Constraints = src.Constraints;
                ThumbsConstraints = src.ThumbsConstraints;

                IsRubberBandSelection = src.IsRubberBandSelection;
                if (src.Pivot != null)
                {
                    Pivot = src.Pivot.Clone() as DiagramPoint;
                }
                if (src.RubberBandBounds != null)
                {
                    RubberBandBounds = src.RubberBandBounds.Clone() as DiagramRect;
                }
                Nodes = new ObservableCollection<Node>();
                if (src.Nodes.Any())
                {
                    foreach (Node node in src.Nodes)
                    {
                        Node newNode = node.Clone() as Node;
                        Nodes.Add(newNode);
                    }
                }
                UserHandles = new DiagramObjectCollection<UserHandle>();
                if (src.UserHandles.Any())
                {
                    foreach (UserHandle userHandle in src.UserHandles)
                    {
                        UserHandle handle = userHandle.Clone() as UserHandle;
                        UserHandles.Add(handle);
                    }
                }
                Connectors = new ObservableCollection<Connector>();
                if (src.Connectors.Any())
                {
                    foreach (Connector conn in src.Connectors)
                    {
                        Connector connector = conn.Clone() as Connector;
                        Connectors.Add(connector);
                    }
                }
            }
        }
        /// <summary>
        /// Creates a new selector that is a copy of the current selector.
        /// </summary>
        /// <returns>Selector</returns>
        public virtual object Clone()
        {
            return new DiagramSelectionSettings(this);
        }

        internal void Dispose()
        {
            if (Nodes != null)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    Nodes[i].Dispose();
                    Nodes[i] = null;
                }
                Nodes.Clear();
                Nodes = null;
            }
            if (Connectors != null)
            {
                for (int i = 0; i < Connectors.Count; i++)
                {
                    Connectors[i].Dispose();
                    Connectors[i] = null;
                }
                Connectors.Clear();
            }

            if (userHandles != null)
            {
                for (int i = 0; i < userHandles.Count; i++)
                {
                    userHandles[i].Dispose();
                    userHandles[i] = null;
                }
                userHandles.Clear();
                userHandles = null;
            }
            if (Wrapper != null)
            {
                Wrapper.Dispose();
                Wrapper = null;
            }
            if (Pivot != null)
            {
                Pivot.Dispose();
                Pivot = null;
            }
            if (Parent != null)
            {
                Parent = null;
            }
            if (RubberBandBounds != null)
            {
                RubberBandBounds.Dispose();
                RubberBandBounds = null;
            }
        }
    }
}
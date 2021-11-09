using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the objects that are rendered in the diagram.
    /// </summary>
    public partial class DiagramLayerContent
    {
        private const string TYPE = "Type";
        private const string SOURCEDECORATOR = "SourceDecorator";
        private const string SHAPE = "Shape";
        private const string WIDTH = "Width";
        private const string HEIGHT = "Height";
        private const string STYLE = "Style";
        private const string STROKEDASHARRAY = "StrokeDashArray";
        private const string TARGETDECORATOR = "TargetDecorator";
        private const string FILL = "Fill";
        private const string GETBOUNDINGCLIENTRECT = "GetBoundingClientRect";
        private const string PARENTDIVGETBOUNDINGCLIENTRECT = "ParentDivGetBoundingClientRect";
        private const string GETSCROLLERBOUNDS = "GetScrollerBounds";
        private const string GETSCROLLERWIDTH = "GetScrollerWidth";
        private const string NONE = "none";
        private const string GROUPCONTAINER = "group_container";
        private const string PATH = "Path";
        private const string ANNOTATIONS = "Annotations";
        private const string NODES = "Nodes";
        private const string CONNECTORS = "Connectors";

        [CascadingParameter]
        internal SfDiagramComponent Parent { get; set; }
        internal ConnectorBridging ConnectorBridging { get; set; } = new ConnectorBridging();

        [Inject]
        internal IJSRuntime JsRuntime { get; set; }
        internal DiagramObjectCollection<Connector> TextAnnotationConnectors = new DiagramObjectCollection<Connector>();
        internal static Dictionary<string, string> MeasurePathDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal static Dictionary<string, string> MeasureCustomPathDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal static Dictionary<string, TextElementUtils> MeasureTextDataCollection { get; set; } = new Dictionary<string, TextElementUtils>() { };
        internal static Dictionary<string, string> MeasureImageDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal static Dictionary<string, string> MeasureNativeDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal Dictionary<string, DiagramRect> PathTable { get; set; } = new Dictionary<string, DiagramRect>() { };
        internal Dictionary<string, string> CustomMeasurePathDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal Dictionary<string, GradientBrush> GradientCollection = new Dictionary<string, GradientBrush>() { };
        internal int UpdateCustomBoundsCount;
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Parent.FirstRender = true;
                if (Parent.IsScriptRendered)
                    await this.OnScriptRendered();
            }
            else
            {
                if (Parent.DiagramAction.HasFlag(DiagramAction.Layouting))
                {
                    await Parent.DoLayout();
                    Parent.DiagramAction &= ~DiagramAction.Layouting;
                    Parent.DiagramStateHasChanged();
                }
                if (Parent != null && Parent.NodeCollection.Count > 0 && MeasureNativeDataCollection != null && MeasureNativeDataCollection.Count > 0)
                {
                    Parent.FirstRender = false;
                    for (int i = 0; i < Parent.NodeCollection.Count; i++)
                    {
                        if (Parent.NodeCollection[i].Shape.Type == Shapes.SVG && Parent.NodeCollection[i].NativeSize == null)
                        {
                            await DomUtil.MeasureBounds(null, null, null, MeasureNativeDataCollection);
                            MeasureNativeDataCollection.Clear();
                            Parent.DiagramStateHasChanged();
                            break;
                        }
                    }
                }
                Parent.FirstRender = false;
            }
        }
        internal async Task OnScriptRendered()
        {
            if (Parent != null)
            {
                await LoadDiagram(false);
                Parent.DiagramAction |= DiagramAction.Render;
                Parent.IsScriptRendered = false;
            }
        }
        internal async Task LoadDiagram(bool isSerialize)
        {
            Dictionary.MeasureCustomBounds.Clear();
            Dictionary.MeasureTextBounds.Clear();
            Dictionary.MeasureNativeELementBounds.Clear();
            SfDiagramComponent.IsProtectedOnChange = false;
            if (!isSerialize)
            {
                if (Parent.DataSourceSettings != null && Parent.DataSourceSettings.DataSource != null)
                {
                    await InitData();
                }
            }
            GetDefaults();
            GetMeasurePathDataCollection();
            if (MeasurePathDataCollection.Count > 0 || (this.Parent.EventHandler.BoundingRect == null) || MeasureTextDataCollection.Count > 0 || (this.Parent.Scroller.scrollerWidth != 0) || MeasureImageDataCollection.Count > 0)
            {
                MeasurePathDataCollection.Add(this.Parent.ID + "_content", GETBOUNDINGCLIENTRECT);
                MeasurePathDataCollection.Add(this.Parent.ID, PARENTDIVGETBOUNDINGCLIENTRECT);
                await DomUtil.MeasureBounds(MeasurePathDataCollection, MeasureTextDataCollection, MeasureImageDataCollection, null);
                MeasurePathDataCollection.Clear();
                MeasureCustomPathDataCollection.Clear();
                MeasureTextDataCollection.Clear();
                MeasureImageDataCollection.Clear();
                MeasureNativeDataCollection.Clear();
                Parent.FirstRender = true;
                if (!isSerialize && Parent.Layout != null && Parent.DataSourceSettings != null)
                    Parent.DiagramAction |= DiagramAction.Layouting;
                this.Parent.EventHandler.BoundingRect = Dictionary.MeasureCustomBounds[GETBOUNDINGCLIENTRECT];
                DiagramRect bounds = Dictionary.MeasureCustomBounds[PARENTDIVGETBOUNDINGCLIENTRECT];
                DiagramRect scrollerBounds = Dictionary.MeasureCustomBounds[GETSCROLLERBOUNDS];
                DiagramRect scroller = Dictionary.MeasureCustomBounds[GETSCROLLERWIDTH];
                this.Parent.Scroller.SetViewPortSize(bounds.Width, bounds.Height);
                this.Parent.DiagramCanvasScrollBounds = scrollerBounds;
                this.Parent.Scroller.scrollerWidth = scroller.Width;
            }
            await MeasureCustomPathPoints();
            await InitObjects();
            Parent.InitCommands();
            Parent.Scroller.SetSize();
            Parent.Scroller.UpdateScrollOffsets(null, null, true);

            if (Parent.BasicElements != null && Parent.BasicElements.Count > 0)
            {
                for (int i = 0; i < Parent.BasicElements.Count; i++)
                {
                    ICommonElement element = Parent.BasicElements[i];
                    DiagramSize size = new DiagramSize() { Width = element.Width ?? 0, Height = element.Height ?? 0 };
                    element.Measure(size);
                    element.Arrange(element.DesiredSize, element is Canvas);
                }
            }
            SfDiagramComponent.IsProtectedOnChange = true;
            await Parent.InitRemoteData();
            if ((!Parent.DiagramAction.HasFlag(DiagramAction.Layouting)) && !isSerialize && (Parent.Layout != null))
                await Parent.DoLayout();
            Parent.DiagramStateHasChanged();
        }
        /// <summary>
        /// This method returns a boolean to indicate if a component’s UI can be rendered. 
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ShouldRender()
        {
            if (Parent.RealAction.HasFlag(RealAction.PreventRefresh))
            {
                return false;
            }
            return true;
        }

        private void GetDefaults()
        {
            if (Parent.Nodes != null)
            {
                int i;
                for (i = 0; i < Parent.Nodes.Count; i++)
                {
                    Parent.NodeCreating.InvokeAsync(Parent.Nodes[i]);
                    ICommonElement content = Parent.SetNodeTemplate?.Invoke(Parent.Nodes[i]);
                    Parent.Nodes[i].Template = content;
                }
            }
            if (Parent.Connectors != null)
            {
                int i;
                for (i = 0; i < Parent.Connectors.Count; i++)
                {
                    Parent.ConnectorCreating.InvokeAsync(Parent.Connectors[i]);
                }
            }
        }

        private async Task InitObjects()
        {
            await InitLayerObjects();
            UpdateBridging();
        }
        internal static void UpdatePathElementOffset(Connector connector)
        {
            if (connector.Wrapper != null)
            {
                ICommonElement defaultWrapper = DiagramUtil.GetWrapper(connector, connector.Wrapper, "Default") as DiagramElement;
                connector.Wrapper.Children.Remove(defaultWrapper);
                List<DiagramPoint> anglePoints = connector.IntermediatePoints;
                PathElement pathElement = ConnectorUtil.UpdatePathElement(anglePoints, connector);
                connector.Wrapper.Children.Add(pathElement);
            }
        }
        internal async Task InitData()
        {
            DataManager dataSourceSettings = Parent.DataSourceSettings.DataManager;
            if (this.Parent.DataBindingModule != null)//&& !(this.realActions & enum_6.RealAction.PreventDataInit))
            {
                if (Parent.DataSourceSettings != null && (!string.IsNullOrEmpty(Parent.DataSourceSettings.DataManager.Url) || (dataSourceSettings.Adaptor == Adaptors.BlazorAdaptor &&
                        !string.IsNullOrEmpty(dataSourceSettings.Url)) || dataSourceSettings.Adaptor == Adaptors.CustomAdaptor))
                {
                    await this.Parent.DataBindingModule.InitSource(this.Parent.DataSourceSettings, this.Parent);
                }
                else
                {
                    this.Parent.DataBindingModule.InitData(this.Parent.DataSourceSettings, this.Parent);
                }
            }
        }
        private async Task InitLayerObjects()
        {
            Parent.RealAction |= RealAction.PreventRefresh;
            Dictionary<string, Node> bpmnTable = new Dictionary<string, Node>();
            Dictionary<string, IDiagramObject> tempTable = new Dictionary<string, IDiagramObject>();
            List<string> groups = new List<string>();
            List<string> connectors = new List<string>();

            foreach (Node obj in this.Parent.Nodes)
            {
                tempTable[obj.ID] = obj;
            }
            foreach (Connector obj in this.Parent.Connectors)
            {
                tempTable[obj.ID] = obj;
            }

            if (Parent.Nodes != null)
            {
                int i; GradientCollection = new Dictionary<string, GradientBrush>();
                for (i = 0; i < Parent.Nodes.Count; i++)
                {
                    DiagramObjectCollection<string> processes = Parent.Nodes[i].Shape is BpmnShape bpmnShape ? bpmnShape.Activity.SubProcess.Processes : null;
                    if (Parent.Nodes[i] is NodeGroup && (Parent.Nodes[i] as NodeGroup)?.Children != null && (Parent.Nodes[i] as NodeGroup)?.Children.Length > 0)
                        groups.Add(Parent.Nodes[i].ID);
                    else if (processes != null && processes.Count > 0)
                    {
                        bpmnTable.Add(Parent.Nodes[i].ID, Parent.Nodes[i]);
                    }
                    else
                    {
                        InitNodes(Parent.Nodes[i]);
                    }
                }
            }
            if (Parent.Connectors != null)
            {
                int i;
                for (i = 0; i < Parent.Connectors.Count; i++)
                {
                    Connector connector = Parent.Connectors[i];
                    if (!(string.IsNullOrEmpty(connector.SourceID)) && !(string.IsNullOrEmpty(connector.TargetID)))
                    {
                        if (tempTable[connector.SourceID] is Node sourceNode && sourceNode.Wrapper != null && tempTable[connector.TargetID] is Node targetNode && targetNode.Wrapper != null)
                        {
                            InitObject(connector);
                        }
                        else
                        {
                            connectors.Add(connector.ID);
                        }
                    }
                    else
                    {
                        InitObject(connector);
                    }
                }
            }
            if (groups.Count > 0)
            {
                foreach (string id in groups)
                {
                    Node groupNode = tempTable[id] as Node;
                    InitNodes(groupNode);
                }
            }
            if (connectors.Count > 0)
            {
                foreach (string id in connectors)
                {
                    Connector connector = tempTable[id] as Connector;
                    InitObject(connector);
                }
            }
            if (TextAnnotationConnectors.Count > 0)
            {
                int i;
                for (i = 0; i < TextAnnotationConnectors.Count; i++)
                {
                    InitObject(TextAnnotationConnectors[i]);
                }
            }
            foreach (string obj in bpmnTable.Keys)
            {
                InitNodes(bpmnTable[obj]);
                Node node = this.Parent.NameTable[obj] as Node;
                BpmnDiagrams bpmn = new BpmnDiagrams();
                bpmn.UpdateDocks(node, this.Parent);
            }
            await MeasureTextElements();
            Parent.RealAction &= ~RealAction.PreventRefresh;
        }

        internal async Task MeasureCustomPathPoints()
        {
            if (Parent.NodeCollection.Count > 0)
            {
                for (int i = 0; i < Parent.NodeCollection.Count; i++)
                {
                    Node node = Parent.NodeCollection[i];
                    if (node.Shape.Type == Shapes.Path && node.IsVisible)
                    {
                        PathShape path = node.Shape as PathShape;
                        DiagramRect val = DomUtil.MeasurePath(path != null && !string.IsNullOrEmpty(path.Data) ? path.Data : string.Empty);
                        string data = DomUtil.UpdatePath(path.Data, val);
                        AddMeasureCustomPathDataCollection(data, path.Data, MeasureCustomPathDataCollection);
                    }
                }
            }
            if (MeasureCustomPathDataCollection != null && MeasureCustomPathDataCollection.Count > 0)
            {
                Parent.RealAction |= RealAction.MeasureDataJSCall;
                await DomUtil.PathPoints(MeasureCustomPathDataCollection);
            }
        }

        private async Task MeasureTextElements()
        {
            Dictionary<string, TextElementUtils> textCollection = new Dictionary<string, TextElementUtils>();
            if (Parent.ConnectorCollection.Count > 0)
            {
                for (int i = 0; i < Parent.ConnectorCollection.Count; i++)
                {
                    Connector connector = Parent.ConnectorCollection[i];
                    for (int j = 0; j < connector.Annotations.Count; j++)
                    {
                        PathAnnotation annotation = connector.Annotations[j];
                        if (annotation != null)
                        {
                            AddMeasureTextCollection(connector, annotation, null, textCollection);
                            connector.Wrapper.Children.Add(connector.GetAnnotationElement(annotation, connector.IntermediatePoints, connector.Bounds));
                        }
                    }
                }
                if (textCollection.Count > 0)
                    await DomUtil.MeasureBounds(null, textCollection, null, null);
                for (int i = 0; i < Parent.ConnectorCollection.Count; i++)
                {
                    Connector connector = Parent.ConnectorCollection[i];
                    connector.Wrapper.Measure(new DiagramSize() { Width = connector.Wrapper.Width, Height = connector.Wrapper.Height });
                    connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                }
            }
            if (Parent.NodeCollection.Count > 0)
            {
                for (int i = 0; i < Parent.NodeCollection.Count; i++)
                {
                    Node node = Parent.NodeCollection[i];
                    if (node.Shape.Type == Shapes.SVG && node.IsVisible)
                    {
                        AddMeasureNativeCollection(node, MeasureNativeDataCollection);
                    }
                }
            }
        }
        internal async Task DoLayout()
        {
            bool update = false;

            DiagramPoint viewPort = new DiagramPoint() { X = this.Parent.Scroller.ViewPortWidth, Y = this.Parent.Scroller.ViewPortHeight };
            if (this.Parent.Layout != null && this.Parent.Layout.Type != LayoutType.None)
            {
                SfDiagramComponent parent = Parent;
                LayoutType layoutType = parent.Layout.Type;
                if ((layoutType == LayoutType.HierarchicalTree || layoutType == LayoutType.OrganizationalChart) && parent.HierarchicalTree != null)
                {
                    parent.HierarchicalTree.UpdateLayout(parent.NodeCollection, parent.NameTable, parent.Layout, viewPort);
                    update = true;
                }
                else if (parent.Layout.Type == LayoutType.MindMap && parent.MindMap != null)
                {
                    MindMap.UpdateLayout(parent.NodeCollection, parent.NameTable, parent.Layout, viewPort, parent.DataSourceSettings?.ID, parent.DataSourceSettings?.Root);
                    update = true;
                }
            }
            if (update)
            {
                await UpdateProperty(null, true);
                foreach (Connector connector in this.Parent.Connectors)
                {
                    this.Parent.SpatialSearch.UpdateQuad(connector.Wrapper);
                }
                foreach (Node node in this.Parent.Nodes)
                {
                    UpdateConnectorEdges(node);
                }
            }
        }
        private void InitNodes(Node node)
        {
            InitObject(node);
        }
        internal void InitObject(object element)
        {
            NodeBase obj = element as NodeBase; Type elementType = element.GetType();
            if (elementType == typeof(Node) || elementType == typeof(NodeGroup))
            {
                Node node = element as Node;
                InitNode(node);
            }
            if (elementType == typeof(Connector))
            {
                Connector connector = element as Connector;
                InitConnector(connector);
                this.UpdateEdges(connector);
            }
            this.Parent.NameTable[obj.ID] = obj;
            if (obj is NodeGroup @group && @group.Children != null && @group.Children.Length > 0)
            {
                this.Parent.DiagramAction |= DiagramAction.Group;
                this.UpdateGroupOffset(obj, true);
                this.Parent.DiagramAction &= ~DiagramAction.Group;
                for (int i = 0; i < @group.Children.Length; i++)
                {
                    if (Parent.NameTable[@group.Children[i]] is NodeBase node)
                    {
                        node.ParentId = @group.ID;
                    }
                }
            }
        }
        internal void UpdateGroupOffset(object obj, bool isUpdateSize, double offsetX = 0, double offsetY = 0)
        {
            if (obj is NodeGroup node)
            {
                Node nodeObj = this.Parent.NameTable[node.ID] as Node;
                if (node.Children != null && node.Children.Length > 0)
                {
                    bool isProtect = SfDiagramComponent.IsProtectedOnChange;
                    SfDiagramComponent.IsProtectedOnChange = true;
                    if (!this.Parent.RealAction.HasFlag(RealAction.PreventScale) && !this.Parent.RealAction.HasFlag(RealAction.PreventDrag))
                    {
                        if (nodeObj != null)
                        {
                            if (nodeObj.OffsetX != 0 && (this.Parent.RealAction.HasFlag(RealAction.EnableGroupAction) || !this.Parent.DiagramAction.HasFlag(DiagramAction.PublicMethod)))
                            {
                                this.Parent.RealAction |= RealAction.PreventScale;
                                double diffX = Parent.FirstRender || offsetX == 0
                                    ? (nodeObj.OffsetX - node.Wrapper.OffsetX)
                                    : offsetX - node.Wrapper.OffsetX;
                                nodeObj.OffsetX = node.Wrapper.OffsetX;
                                double diffY = Parent.FirstRender || offsetY == 0
                                    ? (nodeObj.OffsetY - node.Wrapper.OffsetY)
                                    : offsetY - node.Wrapper.OffsetY;
                                nodeObj.OffsetY = node.Wrapper.OffsetY;
                                if (node.Flip == FlipDirection.None)
                                {
                                    this.Parent.Drag(nodeObj, diffX, diffY);
                                }

                                this.Parent.RealAction &= ~RealAction.PreventScale;
                            }
                            else
                            {
                                if (offsetX == 0)
                                    nodeObj.OffsetX = node.Wrapper.OffsetX;
                            }

                            if (nodeObj.OffsetY != 0 && (this.Parent.RealAction.HasFlag(RealAction.EnableGroupAction)
                                                         || (!this.Parent.DiagramAction.HasFlag(DiagramAction.PublicMethod))))
                            {
                                this.Parent.RealAction |= RealAction.PreventScale;
                                double diffY = Parent.FirstRender || offsetY == 0
                                    ? (nodeObj.OffsetY - node.Wrapper.OffsetY)
                                    : offsetY - node.Wrapper.OffsetY;
                                nodeObj.OffsetY = node.Wrapper.OffsetY;
                                if (node.Flip == FlipDirection.None)
                                {
                                    this.Parent.Drag(nodeObj, 0, diffY);
                                }

                                this.Parent.RealAction &= ~RealAction.PreventScale;
                            }
                            else
                            {
                                if (offsetY == 0)
                                    nodeObj.OffsetY = node.Wrapper.OffsetY;
                            }

                            if (!this.Parent.DiagramAction.HasFlag(DiagramAction.Group))
                            {
                                SfDiagramComponent.IsProtectedOnChange = false;
                                nodeObj.Width = node.Wrapper.ActualSize.Width;
                                nodeObj.Height = node.Wrapper.ActualSize.Height;
                                SfDiagramComponent.IsProtectedOnChange = true;
                            }
                        }
                    }
                    SfDiagramComponent.IsProtectedOnChange = isProtect;
                }

                if (isUpdateSize)
                {
                    if (nodeObj != null && nodeObj.Width != null)
                    {
                        this.ScaleObject(node, BaseUtil.GetDoubleValue(nodeObj.Width),
                            true);
                    }
                    else
                    {
                        if (nodeObj != null) nodeObj.Width = node.Wrapper.ActualSize.Width;
                    }

                    if (nodeObj != null && nodeObj.Height != null)
                    {
                        this.ScaleObject(node, BaseUtil.GetDoubleValue(nodeObj.Height),
                            false);
                    }
                    else
                    {
                        if (nodeObj != null) nodeObj.Height = node.Wrapper.ActualSize.Height;
                    }
                }
            }
        }
        internal void UpdateGroupSize(NodeBase node)
        {
            if (!string.IsNullOrEmpty(node.ParentId))
            {
                if (this.Parent.NameTable[node.ParentId] is Node tempNode)
                {
                    if (!string.IsNullOrEmpty(tempNode.ParentId))
                    {
                        this.UpdateGroupSize(tempNode);
                    }
                    else
                    {
                        tempNode.Wrapper.Measure(new DiagramSize());
                        tempNode.Wrapper.Arrange(tempNode.Wrapper.DesiredSize);
                        this.UpdateGroupOffset(tempNode, false, tempNode.Wrapper.OffsetX, tempNode.Wrapper.OffsetY);
                    }
                }
            }
        }
        private void ScaleObject(Node obj, double size, bool isWidth)
        {
            double? actualSize = isWidth ? obj.Wrapper.ActualSize.Width : obj.Wrapper.ActualSize.Height;
            double sw = (isWidth) ? 1 + ((size - BaseUtil.GetDoubleValue(actualSize)) / BaseUtil.GetDoubleValue(actualSize)) : 1;
            double sh = (isWidth) ? 1 : 1 + ((size - BaseUtil.GetDoubleValue(actualSize)) / BaseUtil.GetDoubleValue(actualSize));
            Parent.RealAction |= RealAction.PreventDrag;
            Parent.Scale(obj, sw, sh, new DiagramPoint() { X = 0.5, Y = 0.5 });
            Parent.RealAction &= ~RealAction.PreventDrag;
        }
        private void InitNode(Node obj)
        {
            ICommonElement content = obj.Template;
            DiagramContainer canvas = obj.InitContainer();
            DiagramContainer portContainer = new Canvas();

            canvas.Children ??= new DiagramObjectCollection<ICommonElement>() { };
            if (obj is NodeGroup groupObj && groupObj.Children != null && groupObj.Children.Length > 0)
            {
                canvas.MeasureChildren = false;

                portContainer.ID = groupObj.ID + GROUPCONTAINER;
                portContainer.Style.Fill = NONE;
                portContainer.Style.StrokeColor = NONE;
                portContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
                portContainer.VerticalAlignment = VerticalAlignment.Stretch;
                canvas.Style = groupObj.Style;
                portContainer.PreventContainer = true;

                canvas.Children.Add(portContainer);
                for (int i = 0; i < groupObj.Children.Length; i++)
                {
                    if (this.Parent.NameTable[groupObj.Children[i]] != null)
                    {
                        if (this.Parent.NameTable[groupObj.Children[i]] is NodeBase child)
                        {
                            canvas.Children.Add(child.Wrapper);
                            canvas.ElementActions |= ElementAction.ElementIsGroup;
                            child.Wrapper.Flip = child.Wrapper.Flip == FlipDirection.None
                                ? groupObj.Wrapper.Flip
                                : child.Wrapper.Flip;
                        }
                    }
                }
            }
            else
            {
                content ??= obj.Init();
                canvas.Children.Add(content);
            }
            DiagramContainer container = (obj is NodeGroup grpNode) && grpNode.Children != null && grpNode.Children.Length > 0 ? portContainer : canvas;

            obj.Template = null;
            obj.InitAnnotations(container, false);
            obj.InitPorts(container);
            foreach (NodeFixedUserHandle nodeFixedUserHandle in obj.FixedUserHandles)
            {
                DiagramElement handle = obj.InitFixedUserHandles(nodeFixedUserHandle);
                container.Children.Add(handle);
            }
            canvas.Measure(new DiagramSize() { Width = obj.Width ?? 0, Height = obj.Height ?? 0 });
            canvas.Arrange(canvas.DesiredSize, false);
            if (obj.Style.Gradient != null && !GradientCollection.ContainsKey(canvas.Children[0].ID))
            {
                if (!GradientCollection.ContainsKey(canvas.Children[0].ID))
                    GradientCollection.Add(canvas.Children[0].ID, obj.Style.Gradient);
                else
                    GradientCollection[canvas.Children[0].ID] = obj.Style.Gradient;
            }
            if (!(this.Parent.NameTable.ContainsKey(obj.ID)))
            {
                this.Parent.NameTable.Add(obj.ID, obj);
                this.Parent.UpdateQuad(obj);
            }
        }

        internal Node GetNode(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                DiagramObjectCollection<Node> nodes = Parent.Nodes;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].ID == id)
                    {
                        return nodes[i];
                    }
                }
                if (this.Parent.NameTable.ContainsKey(id))
                {
                    if (this.Parent.NameTable[id] is Node node && node.ID == id)
                    {
                        return node;
                    }
                }
                else if(this.Parent.DataBindingModule!=null && this.Parent.DataBindingModule.nodes!=null && this.Parent.DataBindingModule.nodes.Count>0)
                {
                    DiagramObjectCollection<Node> dataNodes = this.Parent.DataBindingModule.nodes;
                    for (int i = 0; i < dataNodes.Count; i++)
                    {
                        if (dataNodes[i].ID == id)
                        {
                            return dataNodes[i];
                        }
                    }
                }
            }
            return null;
        }

        internal Connector GetConnector(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                DiagramObjectCollection<Connector> connectors = Parent.Connectors;
                for (int i = 0; i < connectors.Count; i++)
                {
                    if (connectors[i].ID == id)
                    {
                        return connectors[i];
                    }
                }
                if (this.Parent.NameTable.ContainsKey(id))
                {
                    if (this.Parent.NameTable[id] is Connector connector && connector.ID == id)
                    {
                        return connector;
                    }
                }
                else if (this.Parent.DataBindingModule != null && this.Parent.DataBindingModule.connectors != null && this.Parent.DataBindingModule.connectors.Count > 0)
                {
                    DiagramObjectCollection<Connector> dataConnectors = this.Parent.DataBindingModule.connectors;
                    for (int i = 0; i < dataConnectors.Count; i++)
                    {
                        if (dataConnectors[i].ID == id)
                        {
                            return dataConnectors[i];
                        }
                    }
                }
            }
            return null;
        }

        private static PointPort GetPort(Node node, string portId)
        {
            if (!string.IsNullOrEmpty(portId) && node != null)
            {
                DiagramObjectCollection<PointPort> ports = node.Ports;
                for (int i = 0; i < ports.Count; i++)
                {
                    if (ports[i].ID == portId)
                    {
                        return ports[i];
                    }
                }
            }
            return null;
        }

        internal async Task RefreshDataSource()
        {
            SfDiagramComponent.IsProtectedOnChange = false;
            Parent.ClearObjects();
            await InitData();
            GetDefaults();
            GetMeasurePathDataCollection();
            await DomUtil.MeasureBounds(MeasurePathDataCollection, MeasureTextDataCollection, null, null);
            Parent.SpatialSearch = new SpatialSearch();
            await InitObjects();
            SfDiagramComponent.IsProtectedOnChange = true;
            await Parent.DoLayout();
        }

        private static PointPort GetConnectedPort(Node node, Connector connector, bool isSource)
        {
            if (node != null && node.Ports != null && node.Ports.Count > 0)
            {
                for (int i = 0; i < node.Ports.Count; i++)
                {
                    PointPort port = node.Ports[i];
                    if (port.ID == connector.SourcePortID && isSource)
                    {
                        return port;
                    }
                    else if (port.ID == connector.TargetPortID && !isSource)
                    {
                        return port;
                    }
                }
            }
            return null;
        }

        private static PointPort FindInOutConnectPorts(Node node, bool isInConnect)
        {
            PointPort port = null;
            if (node != null)
            {
                port = DiagramUtil.GetInOutConnectPorts(node, isInConnect);
            }
            return port;
        }

        private void InitConnector(Connector obj, bool independentObj = true)
        {
            Node sourceNode = GetNode(obj.SourceID);
            Node targetNode = GetNode(obj.TargetID);
            PointPort port = GetConnectedPort(sourceNode, obj, true);
            PointPort targetPort = GetConnectedPort(targetNode, obj, false);
            PointPort outPort = FindInOutConnectPorts(sourceNode, false);
            Port inPort = FindInOutConnectPorts(targetNode, true);
            if ((sourceNode != null && ConstraintsUtil.CanOutConnect(sourceNode)) || (!string.IsNullOrEmpty(obj.SourcePortID)
                && outPort != null && ConstraintsUtil.CanPortOutConnect(outPort)))
            {
                obj.SourceWrapper = GetEndNodeWrapper(sourceNode, obj, true) as DiagramElement;
                if (!string.IsNullOrEmpty(obj.SourcePortID))
                {
                    if (port != null && port.Constraints != PortConstraints.None && ConstraintsUtil.CheckPortConstraints(port, PortConstraints.OutConnect))
                    {
                        if (sourceNode != null)
                            obj.SourcePortWrapper = DiagramUtil.GetWrapper(sourceNode,
                                sourceNode.Wrapper, obj.SourcePortID) as DiagramElement;
                    }
                }
            }
            if ((targetNode != null && ConstraintsUtil.CanInConnect(targetNode)) || (!string.IsNullOrEmpty((obj as Connector).TargetPortID)
                && ConstraintsUtil.CheckPortConstraints(inPort, PortConstraints.InConnect)))
            {
                obj.TargetWrapper = GetEndNodeWrapper(targetNode, obj, false) as DiagramElement;
                if (!string.IsNullOrEmpty(obj.TargetPortID))
                {
                    if (targetPort != null && targetPort.Constraints != PortConstraints.None && ConstraintsUtil.CheckPortConstraints(targetPort, PortConstraints.InConnect))
                    {
                        if (targetNode != null)
                            obj.TargetPortWrapper = DiagramUtil.GetWrapper(
                                targetNode, targetNode.Wrapper, obj.TargetPortID) as DiagramElement;
                    }
                }
            }
            if (!independentObj)
            {
                List<DiagramPoint> points = obj.GetConnectorPoints();
                DiagramUtil.UpdateConnector(obj, points);
            }
            if (independentObj) { obj.Init(); }
            for (int k = 0; k < obj.Wrapper.Children.Count; k++)
            {
                if (obj.Wrapper.Children[k] is PathElement pathElement && PathTable.ContainsKey(pathElement.Data))
                {
                    pathElement.AbsoluteBounds =
                        PathTable[pathElement.Data];
                }
            }
            obj.Wrapper.Measure(new DiagramSize());
            obj.Wrapper.Arrange(obj.Wrapper.DesiredSize);
            for (int j = 0; j < obj.Wrapper.Children.Count; j++)
            {
                if (obj.Wrapper.Children[j] is PathElement pathElement && !PathTable.ContainsKey(pathElement.Data))
                {
                    PathTable.Add(pathElement.Data,
                        pathElement.AbsoluteBounds);
                }
            }
            this.Parent.NameTable.Add(obj.ID, obj);
            this.Parent.UpdateQuad(obj);
        }

        internal void GetMeasurePathDataCollection()
        {
            MeasurePathDataCollection = new Dictionary<string, string>() { };
            Dictionary.MeasureCustomBounds = new Dictionary<string, DiagramRect>() { };
            MeasureTextDataCollection = new Dictionary<string, TextElementUtils>() { };
            MeasureImageDataCollection = new Dictionary<string, string>() { };
            MeasureNativeDataCollection = new Dictionary<string, string>() { };
            MeasureCustomPathDataCollection = new Dictionary<string, string>() { };
            if (Parent != null)
            {
                int i;
                if (Parent.BasicElements != null)
                {
                    for (i = 0; i < Parent.BasicElements.Count; i++)
                    {
                        ICommonElement element = Parent.BasicElements[i];
                        if (element is PathElement pathElement)
                        {
                            AddMeasurePathDataCollection(pathElement.Data, MeasurePathDataCollection);
                        }
                        else if (element is StackPanel)
                        {
                            MeasureNodeTemplate(null, element, MeasurePathDataCollection, MeasureTextDataCollection);
                        }
                    }
                }
                else if (Parent.Nodes != null && Parent.Nodes.Count > 0)
                {
                    for (i = 0; i < Parent.Nodes.Count; i++)
                        GetMeasureNodeData(Parent.Nodes[i], MeasurePathDataCollection, MeasureTextDataCollection, MeasureImageDataCollection, MeasureNativeDataCollection);
                }
                else if (Parent.Connectors != null && Parent.Connectors.Count > 0)
                {
                    for (i = 0; i < Parent.Connectors.Count; i++)
                    {
                        Connector connector = Parent.Connectors[i];
                        GetMeasureConnectorData(connector, MeasurePathDataCollection);
                    }
                }
                if (Parent.SelectionSettings != null && Parent.SelectionSettings.UserHandles.Count > 0)
                {
                    for (i = 0; i < Parent.SelectionSettings.UserHandles.Count; i++)
                        GetMeasureUserHandleData(Parent.SelectionSettings.UserHandles[i], MeasurePathDataCollection);
                }
            }
        }

        internal static void GetMeasureConnectorData(Connector connector, Dictionary<string, string> measurePathData)
        {
            DecoratorSettings sourceDecorator = connector.SourceDecorator;
            DecoratorSettings targetDecorator = connector.TargetDecorator;
            if (sourceDecorator.Shape == DecoratorShape.Custom)
                AddMeasurePathDataCollection(sourceDecorator.PathData, measurePathData);
            if (targetDecorator.Shape == DecoratorShape.Custom)
                AddMeasurePathDataCollection(targetDecorator.PathData, measurePathData);
            if (connector.FixedUserHandles.Count > 0)
            {
                foreach (ConnectorFixedUserHandle userHandle in connector.FixedUserHandles)
                {
                    if (!string.IsNullOrEmpty(userHandle.PathData))
                    {
                        AddMeasurePathDataCollection(userHandle.PathData, measurePathData);
                    }
                }
            }
        }
        internal static void GetMeasureUserHandleData(UserHandle userHandle, Dictionary<string, string> measurePathData)
        {
            string shape = userHandle.PathData;
            if (!string.IsNullOrEmpty(shape))
            {
                AddMeasurePathDataCollection(shape, measurePathData);
            }
        }

        internal static void GetMeasureNodeData(Node node, Dictionary<string, string> measurePathData, Dictionary<string, TextElementUtils> measureTextData, Dictionary<string, string> measureImageData, Dictionary<string, string> measureNativeData)
        {
            Shape shape = node.Shape;
            Shapes type = shape.Type;
            if (node.Template != null)
            {
                MeasureNodeTemplate(node, node.Template, measurePathData, measureTextData);
            }
            if (type == Shapes.Path && !string.IsNullOrEmpty((shape as PathShape)?.Data))
            {
                AddMeasurePathDataCollection(((PathShape)shape).Data, measurePathData);
            }
            else if (type == Shapes.Image && !string.IsNullOrEmpty((shape as ImageShape)?.Source))
            {
                AddMeasureImageSourceCollection(((ImageShape)shape).Source, node, measureImageData);
            }
            else if (type == Shapes.SVG)
            {
                AddMeasureNativeCollection(node, measureNativeData);
            }
            else if (type == Shapes.Basic && shape is BasicShape basicShape && basicShape.Shape == BasicShapeType.Polygon)
            {
                DiagramPoint[] points = basicShape.Points;
                if (points.Length > 0)
                {
                    string path = PathUtil.GetPolygonPath(points);
                    basicShape.PolygonPath = path;
                    AddMeasurePathDataCollection(path, measurePathData);
                }
            }
            DiagramObjectCollection<PointPort> ports = node.Ports;
            foreach (PointPort port in ports)
            {
                if (port.Shape == PortShapes.Custom)
                    AddMeasurePathDataCollection(port.PathData, measurePathData);
            }
            if (node.Annotations != null)
            {
                DiagramObjectCollection<ShapeAnnotation> annotations = node.Annotations;
                for (int j = 0; j < annotations.Count; j++)
                {
                    ShapeAnnotation annotation = annotations[j];
                    AddMeasureTextCollection(node, annotation, null, measureTextData);
                }
            }
            if (node.FixedUserHandles.Count > 0)
            {
                foreach (NodeFixedUserHandle userHandle in node.FixedUserHandles)
                {
                    if (!string.IsNullOrEmpty(userHandle.PathData))
                    {
                        AddMeasurePathDataCollection(userHandle.PathData, measurePathData);
                    }
                }
            }
            if (node.Shape is BpmnShape bpmnShape)
            {
                if (bpmnShape.Annotations.Count > 0)
                {
                    DiagramObjectCollection<BpmnAnnotation> annotations = bpmnShape.Annotations;
                    for (int j = 0; j < annotations.Count; j++)
                    {
                        BpmnAnnotation annotation = annotations[j];
                        AddMeasureTextCollection(node, annotation, null, measureTextData);
                    }
                }
                if (bpmnShape.Annotation != null)
                {
                    BpmnAnnotation annotation = bpmnShape.Annotation;
                    AddMeasureTextCollection(node, annotation, null, measureTextData);
                }
                if (bpmnShape.Activity.SubProcess.Events.Count > 0)
                {
                    List<BpmnSubEvent> events = bpmnShape.Activity.SubProcess.Events;
                    for (int j = 0; j < events.Count; j++)
                    {
                        DiagramObjectCollection<ShapeAnnotation> annotations = events[j].Annotations;
                        for (int i = 0; i < annotations.Count; i++)
                        {
                            ShapeAnnotation annotation = annotations[i];
                            AddMeasureTextCollection(node, annotation, null, measureTextData);
                        }
                    }
                }
            }
        }

        private static void MeasureNodeTemplate(Node node, ICommonElement template, Dictionary<string, string> measurePathData, Dictionary<string, TextElementUtils> measureTextData)
        {
            if (template is PathElement pathElement)
                AddMeasurePathDataCollection(pathElement.Data, measurePathData);
            else if (template is TextElement textElement)
                AddMeasureTextCollection(node, null, textElement, measureTextData);
            else if ((node != null && node.Template is Canvas canvas && canvas.Children.Count > 0) || template is StackPanel)
            {
                ObservableCollection<ICommonElement> children = template is StackPanel stackPanel ? stackPanel.Children : (node.Template as Canvas)?.Children;
                if (children != null)
                {
                    for (int j = 0; j < children.Count; j++)
                    {
                        ICommonElement child = children[j];
                        MeasureNodeTemplate(node, child, measurePathData, measureTextData);
                    }
                }
            }
        }
        internal static void AddAnnotationToHistory(NodeBase obj, Annotation annotation)
        {
            if (obj.Parent != null && !((SfDiagramComponent)obj.Parent).DiagramAction.HasFlag(DiagramAction.UndoRedo))
            {
                if (annotation != null)
                {
                    InternalHistoryEntry entry = new InternalHistoryEntry()
                    {
                        Type = HistoryEntryType.LabelCollectionChanged,
                        ChangeType = HistoryEntryChangeType.Insert,
                        RedoObject = obj.Clone() as IDiagramObject,
                        UndoObject = annotation.Clone() as IDiagramObject,
                        Category = EntryCategory.InternalEntry,
                    };
                    ((SfDiagramComponent)obj.Parent).AddHistoryEntry(entry);
                }
            }
        }
        internal static void AddMeasureTextCollection(NodeBase obj, Annotation annotation, TextElement textElement, Dictionary<string, TextElementUtils> textDataCollection)
        {
            Node node = obj as Node;
            double? width = textElement?.Width; double? height = textElement?.Height;
            if (obj is Connector connector)
            {
                width = connector.Bounds.Width;
                height = connector.Bounds.Height;
            }
            if (node != null)
            {
                width = node.Width < node.MinWidth ? node.MinWidth : node.Width;
                width = width > node.MaxWidth ? node.MaxWidth : width;
                height = node.Height < node.MinHeight ? node.MinHeight : node.Height;
                height = height > node.MaxHeight ? node.MaxHeight : height;
            }
            TextElementUtils textElementUtils = null;
            string id = annotation != null ? obj.ID + "_" + annotation.ID : textElement != null ? textElement.ID : string.Empty;
            if (node != null && node.Shape is BpmnShape shape && (shape.Shape == BpmnShapes.TextAnnotation || shape.Annotations.Count > 0))
            {
                if (annotation != null)
                {
                    id = (obj.ID + "_" + annotation.ID + "_text");
                    annotation.Content = annotation is BpmnAnnotation bpmnAnnotation
                        ? bpmnAnnotation.Text
                        : ((ShapeAnnotation)annotation).Content;
                }
            }
            DiagramSize size = new DiagramSize() { Width = width, Height = height };
            if (Dictionary.GetMeasureTextBounds(id) == null)
            {
                textElementUtils = new TextElementUtils()
                {
                    Content = annotation != null ? annotation.Content : textElement.Content,
                    Style = annotation != null ? annotation.Style : textElement.Style as TextStyle,
                    Bounds = new DiagramSize()
                    {
                        Width = annotation != null ? annotation.Width : textElement.Width,
                        Height = annotation != null ? annotation.Height : textElement.Height
                    },
                    NodeSize = size
                };
            }
            else
            {
                if (annotation != null)
                {
                    textElementUtils = Dictionary.GetMeasureTextBounds(id);                    
                    if (node != null && textElementUtils.NodeSize == null)
                    {
                        textElementUtils.NodeSize = size;
                        double? tempWidth = textElementUtils.NodeSize.Width < node.MinWidth ? node.MinWidth : textElementUtils.NodeSize.Width;
                        tempWidth = tempWidth > node.MaxWidth ? node.MaxWidth : tempWidth;

                        double? tempHeight = textElementUtils.NodeSize.Height < node.MinHeight ? node.MinHeight : textElementUtils.NodeSize.Height;
                        tempHeight = tempHeight > node.MaxHeight ? node.MaxHeight : tempHeight;

                        textElementUtils.NodeSize = new DiagramSize() { Width = tempWidth, Height = tempHeight };
                    }
                }
            }
            if (textElementUtils != null)
            {
                HyperlinkSettings link = annotation?.Hyperlink;
                if (link != null)
                {
                    bool isDirtAnnotation = annotation.IsDirtAnnotation;
                    bool isProtectChange = SfDiagramComponent.IsProtectedOnChange;
                    SfDiagramComponent.IsProtectedOnChange = false;
                    textElementUtils.Style ??= annotation.Style;
                    textElementUtils.Style.Color = link.Color;
                    textElementUtils.Style.TextDecoration = link.TextDecoration;
                    textElementUtils.Content = !string.IsNullOrEmpty(link.Content) ? link.Content : link.Url;
                    annotation.IsDirtAnnotation = isDirtAnnotation;
                    SfDiagramComponent.IsProtectedOnChange = isProtectChange;
                }

                if (!textDataCollection.ContainsKey(id))
                {
                    textDataCollection.Add(id, textElementUtils);
                }
                else
                {
                    textDataCollection[id] = textElementUtils;
                }
            }
        }
        internal static void AddMeasurePathDataCollection(string data, Dictionary<string, string> collection)
        {
            if (Dictionary.GetMeasurePathBounds(data) == null && !MeasurePathDataCollection.ContainsKey(data))
            {
                if (!collection.ContainsKey(data))
                {
                    collection.Add(data, PATH);
                }
            }
        }
        internal static void AddMeasureImageSourceCollection(string data, Node node, Dictionary<string, string> collection)
        {
            if (Dictionary.GetMeasureImageBounds(node.ID) == null && !MeasureImageDataCollection.ContainsKey(data))
            {
                collection.Add(node.ID + "_content", data);
            }
        }
        internal static void AddMeasureNativeCollection(Node node, Dictionary<string, string> collection)
        {
            if (Dictionary.GetMeasureNativeElementBounds(node.ID) == null && !MeasureNativeDataCollection.ContainsKey(node.ID))
            {
                if (!collection.ContainsKey(node.ID))
                {
                    collection.Add(node.ID, node.ID + "_content_native_element");
                }
            }
        }
        internal static void AddMeasureCustomPathDataCollection(string data, string nodeData, Dictionary<string, string> collection)
        {
            if (!string.IsNullOrEmpty(nodeData) && Dictionary.GetCustomPathPointsCollection(nodeData) == null && !MeasureCustomPathDataCollection.ContainsKey(data))
            {
                collection.Add(data, nodeData);
            }
        }
        // Property change at runtime
        internal async Task OnPropertyChanged(Dictionary<string, object> property, object newVal, object oldVal, object parent)
        {
            if (!(parent is SfDiagramComponent))
            {
                AddCustomPathDataCollection(property, parent);
                IDiagramObject propertyParentVal = (parent as DiagramObject)?.GetParent();
                Dictionary<string, object> properties = new Dictionary<string, object>();
                string propertyName = (parent as DiagramObject)?.PropertyName;
                PropertyInfo collectionProp = propertyParentVal.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
                object collection = collectionProp?.GetValue(propertyParentVal);
                if (collection != null && collection.GetType().IsGenericType)
                {
                    Type collectionType = collection.GetType();
                    MethodInfo methodInfo = collectionType.GetMethod("IndexOf");
                    if (methodInfo != null)
                    {
                        string index = methodInfo.Invoke(collection, new object[] { parent }).ToString();
                        if (index != "-1")
                        {
                            Dictionary<string, object> indexProperties = new Dictionary<string, object>
                            {
                                {index, property}
                            };
                            properties.Add(propertyName, indexProperties);
                        }
                        else
                            return;
                    }
                }
                else
                {
                    properties.Add(propertyName, property);
                }
                await OnPropertyChanged(properties, newVal, oldVal, propertyParentVal);
            }
            else
            {
                if (Parent.IsBeginUpdate)
                {
                    UpdateChangedPropertyDictionary(Parent.PropertyChanges, property);
                }
                else
                {
                    Parent.PropertyChanges = property;
                    await UpdateProperty(newVal);
                    Parent.DiagramStateHasChanged();
                }
            }
        }

        private Dictionary<string, TextElementUtils> UpdateNodeTextElementUtils(Dictionary<string, object> propertyChanges, TextElementUtils textElementUtils, ref bool isUpdate)
        {
            bool tempUpdate = false;
            bool isProtectChange = SfDiagramComponent.IsProtectedOnChange;
            SfDiagramComponent.IsProtectedOnChange = false;
            Dictionary<string, TextElementUtils> textData = new Dictionary<string, TextElementUtils>();
            foreach (string key in propertyChanges.Keys)
            {
                object value = propertyChanges[key];
                if (value is PropertyChangeValues propertyChangeValues)
                    value = propertyChangeValues.NewValue;
                switch (key)
                {
                    case CONNECTORS:
                    case NODES:
                        Dictionary<string, object> indexes = value as Dictionary<string, object>;
                        NodeBase obj;
                        foreach (string indx in indexes.Keys)
                        {
                            _ = int.TryParse(indx, out int index);
                            if (key == NODES)
                                obj = Parent.Nodes[index];
                            else
                                obj = Parent.Connectors[index];
                            if ((indexes[indx] as Dictionary<string, object>).ContainsKey(ANNOTATIONS))
                            {
                                UpdateTextElementUtils(obj, indexes[indx] as Dictionary<string, object>, null, textData, ref tempUpdate);
                            }
                            if (obj is Node node && node.IsDirtNode)
                            {
                                for (int i = 0; i < node.Annotations.Count; i++)
                                {
                                    UpdateAnnotationTextUtils(node, textData, indexes[indx] as Dictionary<string, object>, i.ToString(CultureInfo.InvariantCulture));
                                }
                                node.IsDirtNode = false;
                            }
                        }
                        break;
                    case "Width":
                        tempUpdate = true;
                        double? width = (double?)value;
                        if (width != null)
                            textElementUtils.NodeSize.Width = width;
                        break;
                    case "Height":
                        tempUpdate = true;
                        double? height = (double?)value;
                        if (height != null)
                            textElementUtils.NodeSize.Height = height;
                        break;
                    case "MinWidth":
                        tempUpdate = true;
                        double? minW = (double?)value;
                        if (minW != null)
                            textElementUtils.NodeSize.Width = textElementUtils.NodeSize.Width > minW ? textElementUtils.NodeSize.Width : minW;
                        break;
                    case "MaxWidth":
                        tempUpdate = true;
                        double? maxW = (double?)value;
                        if (maxW != null)
                            textElementUtils.NodeSize.Width = textElementUtils.NodeSize.Width < maxW ? textElementUtils.NodeSize.Width : maxW;
                        break;
                    case "MinHeight":
                        tempUpdate = true;
                        double? minH = (double?)value;
                        if (minH != null)
                            textElementUtils.NodeSize.Height = textElementUtils.NodeSize.Height > minH ? textElementUtils.NodeSize.Height : minH;
                        break;
                    case "MaxHeight":
                        tempUpdate = true;
                        double? maxH = (double?)value;
                        if (maxH != null)
                            textElementUtils.NodeSize.Height = textElementUtils.NodeSize.Height < maxH ? textElementUtils.NodeSize.Height : maxH;
                        break;
                }
            }
            if (tempUpdate)
            {
                isUpdate = true;
            }
            SfDiagramComponent.IsProtectedOnChange = isProtectChange;
            return textData;
        }

        private void UpdateAnnotationTextUtils(NodeBase obj, Dictionary<string, TextElementUtils> textData, Dictionary<string, object> indexes, string index)
        {
            Node node = obj as Node;
            Annotation annotation = null;
            bool shouldMeasure = false;
            if (((SfDiagramComponent)obj.Parent).DiagramAction.HasFlag(DiagramAction.UndoRedo))
            {
                shouldMeasure = IsMeasure(indexes);
            }
            bool isUpdate = false;
            _ = int.TryParse(index, out int idx);
            if (node != null)
                annotation = node.Annotations[idx];
            else if (obj is Connector connector) annotation = connector.Annotations[idx];
            if (annotation != null)
            {
                TextElement annotationWrapper = DiagramUtil.GetWrapper(obj, obj.Wrapper, annotation.ID) as TextElement;
                string id = obj.ID + "_" + annotation.ID;
                TextElementUtils elementUtils = Dictionary.GetMeasureTextBounds(id);
                if (annotation.IsDirtAnnotation || shouldMeasure || node is { IsDirtNode: true })
                {
                    if (!(indexes.ContainsKey(ANNOTATIONS)))
                    {
                        if (((SfDiagramComponent)obj.Parent).DiagramAction.HasFlag(DiagramAction.UndoRedo))
                        {
                            elementUtils.Bounds = new DiagramSize()
                            {
                                Width = obj.Wrapper.Bounds.Width,
                                Height = obj.Wrapper.Bounds.Height,
                            };
                        }
                        else
                        {
                            elementUtils.Bounds = new DiagramSize()
                            {
                                Width = annotation.Width ?? elementUtils.Bounds.Width,
                                Height = annotation.Height ?? elementUtils.Bounds.Height
                            };
                        }

                        elementUtils.NodeSize = new DiagramSize() { Width = obj.Wrapper.Bounds.Width, Height = obj.Wrapper.Bounds.Height };
                    }
                    if ((annotation.IsDirtAnnotation || shouldMeasure) && indexes.ContainsKey(index))
                    {
                        UpdateTextElementUtils(obj, indexes[index] as Dictionary<string, object>, elementUtils, textData, ref isUpdate);
                        annotation.IsDirtAnnotation = false;
                    }
                    if (node is { IsDirtNode: true })
                        UpdateNodeTextElementUtils(indexes, elementUtils, ref isUpdate);
                    if (isUpdate)
                        AddMeasureTextCollection(obj, annotation, null, textData);
                    annotationWrapper?.RefreshTextElement();
                }
            }
        }

        private static bool IsMeasure(Dictionary<string, object> indexes)
        {
            if (indexes.Any())
            {
                foreach (string index in indexes.Keys)
                {
                    PropertyChangeValues values = null;
                    if (indexes[index] is Dictionary<string, object> key)
                    {
                        if (key.ContainsKey("Content"))
                        {
                            values = key["Content"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("Hyperlink"))
                        {
                            values = key["Hyperlink"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("Width"))
                        {
                            values = key["Width"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("Height"))
                        {
                            values = key["Height"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("Style"))
                        {
                            values = key["Style"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("Margin"))
                        {
                            values = key["Margin"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey(TYPE))
                        {
                            values = key[TYPE] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("Bold"))
                        {
                            values = key["Bold"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("FontFamily"))
                        {
                            values = key["FontFamily"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("FontSize"))
                        {
                            values = key["FontSize"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("Italic"))
                        {
                            values = key["Italic"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("TextAlign"))
                        {
                            values = key["TextAlign"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("TextDecoration"))
                        {
                            values = key["TextDecoration"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("TextOverflow"))
                        {
                            values = key["TextOverflow"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("TextWrapping"))
                        {
                            values = key["TextWrapping"] as PropertyChangeValues;
                        }
                        if (key.ContainsKey("TextWrapping"))
                        {
                            values = key["TextWrapping"] as PropertyChangeValues;
                        }
                        if (values != null && values.NewValue != values.OldValue)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }
        private Dictionary<string, TextElementUtils> UpdateTextElementUtils(NodeBase node, Dictionary<string, object> propertyChanges, TextElementUtils textElementUtils, Dictionary<string, TextElementUtils> textData, ref bool isUpdate)
        {
            isUpdate = false;
            foreach (string key in propertyChanges.Keys)
            {
                object value = propertyChanges[key];
                if (value is PropertyChangeValues propertyChangeValues)
                    value = propertyChangeValues.NewValue;
                switch (key)
                {
                    case ANNOTATIONS:
                        Dictionary<string, object> indexes = value as Dictionary<string, object>;
                        foreach (string indx in indexes.Keys)
                        {
                            UpdateAnnotationTextUtils(node, textData, indexes, indx);
                        }
                        return textData;
                    case "Style":
                        if (value is Dictionary<string, object> objects)
                            UpdateTextElementUtils(node, objects, textElementUtils, null, ref isUpdate);
                        else
                            textElementUtils.Style = value as TextStyle;
                        break;
                    case "TextAlign":
                        isUpdate = true;
                        if (Enum.TryParse(value.ToString(), out TextAlign textAlign))
                        {
                            textElementUtils.Style.TextAlign = textAlign;
                        }
                        break;
                    case "TextWrapping":
                        isUpdate = true;
                        if (Enum.TryParse(value.ToString(), out TextWrap textWrap))
                        {
                            textElementUtils.Style.TextWrapping = textWrap;
                            SetAnnotationSize(textElementUtils);
                        }
                        break;
                    case "TextDecoration":
                        isUpdate = true;
                        if (Enum.TryParse(value.ToString(), out TextDecoration textDecoration))
                        {
                            textElementUtils.Style.TextDecoration = textDecoration;
                            SetAnnotationSize(textElementUtils);
                        }
                        break;
                    case "TextOverflow":
                        isUpdate = true;
                        if (Enum.TryParse(value.ToString(), out TextOverflow textOverflow))
                        {
                            textElementUtils.Style.TextOverflow = textOverflow;
                            //If we set textOverflow, then need to set annotation's size as textElementUtils's bounds to measure text value at runtime.
                            //based on given size of the node/annotation.
                            SetAnnotationSize(textElementUtils);
                        }
                        if(UpdateCustomBoundsCount > 0)
                        {
                            Parent.RealAction |= RealAction.PreventRefresh;
                        }
                        break;
                    case "Bold":
                        isUpdate = true;
                        textElementUtils.Style.Bold = (bool)value;
                        SetAnnotationSize(textElementUtils);
                        break;
                    case "FontFamily":
                        isUpdate = true;
                        textElementUtils.Style.FontFamily = value.ToString();
                        SetAnnotationSize(textElementUtils);
                        break;
                    case "FontSize":
                        isUpdate = true;
                        textElementUtils.Style.FontSize = (double)value;
                        SetAnnotationSize(textElementUtils);
                        break;
                    case "Italic":
                        isUpdate = true;
                        textElementUtils.Style.Italic = (bool)value;
                        SetAnnotationSize(textElementUtils);
                        break;
                    case "WhiteSpace":
                        isUpdate = true;
                        if (Enum.TryParse(value.ToString(), out WhiteSpace whiteSpace))
                        {
                            textElementUtils.Style.WhiteSpace = whiteSpace;
                        }
                        break;
                    case "Width":
                        isUpdate = true;
                        textElementUtils.Bounds.Width = (double?)value;
                        break;
                    case "Height":
                        isUpdate = true;
                        textElementUtils.Bounds.Height = (double?)value;
                        break;
                    case "Content":
                        isUpdate = true;
                        textElementUtils.Content = value.ToString();
                        break;
                    case "Hyperlink":
                        isUpdate = true;
                        TextElement annotationWrapper = DiagramUtil.GetWrapper((node as NodeBase), (node as NodeBase).Wrapper, (textElementUtils.Style.Parent as Annotation).ID) as TextElement;
                        UpdateHyperlink(annotationWrapper, value as Dictionary<string, object>, (textElementUtils.Style.Parent as Annotation));
                        break;
                }
            }
            return textData;
        }

        private static void SetAnnotationSize(TextElementUtils textElementUtils)
        {
            if (textElementUtils.Style.Parent is Annotation annotation)
                textElementUtils.Bounds = new DiagramSize() { Width = annotation.Width, Height = annotation.Height };
        }

        private void UpdateChangedPropertyDictionary(Dictionary<string, object> changedProp, Dictionary<string, object> currentChangedProp)
        {
            foreach (string key in currentChangedProp.Keys)
            {
                if (changedProp != null)
                {
                    if (changedProp.ContainsKey(key))
                    {
                        if (changedProp[key] is Dictionary<string, object>)
                            UpdateChangedPropertyDictionary(changedProp[key] as Dictionary<string, object>, currentChangedProp[key] as Dictionary<string, object>);
                        else
                            changedProp[key] = currentChangedProp[key];
                    }
                    else
                    {
                        changedProp.Add(key, currentChangedProp[key]);
                    }
                }
            }
        }
        internal async Task UpdateProperty(object newVal = null, bool isLayout = false)
        {
            Parent.RealAction |= RealAction.PathDataMeasureAsync;
            UpdateCustomBoundsCount++;
            await UpdateCustomBounds();
            UpdateCustomBoundsCount--;
            if (UpdateCustomBoundsCount == 0)
            {
                Parent.RealAction &= ~RealAction.PathDataMeasureAsync;
                bool protectOnChange = SfDiagramComponent.IsProtectedOnChange;
                SfDiagramComponent.IsProtectedOnChange = false;
                for (int i = 0; i < Parent.PropertyChanges.Count; i++)
                {
                    KeyValuePair<string, object> entry = Parent.PropertyChanges.ElementAt(i);
                    switch (entry.Key)
                    {
                        case NODES:
                            NodeProperty(Parent.PropertyChanges[entry.Key] as Dictionary<string, object>, newVal, isLayout);
                            break;
                        case CONNECTORS:
                            if (Parent.PropertyChanges.Count > 0 && Parent.PropertyChanges[entry.Key] != null)
                            {
                                ConnectorPropertyChange(Parent.PropertyChanges[entry.Key] as Dictionary<string, object>, true);
                            }
                            break;
                    }
                }
                UpdateBridging();

                Parent.PropertyChanges.Clear();
                SfDiagramComponent.IsProtectedOnChange = protectOnChange;
            }
        }

        internal void NodeProperty(Dictionary<string, object> properties, object newVal, bool isLayout)
        {
            bool update = true;
            bool sizeChanged = false; bool isRotating = false;
            bool isUndoRedo = false;
            double oldBpmnOffsetX = 0;
            double oldBpmnOffsetY = 0;
            if (Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo))
            {
                isUndoRedo = true;
            }
            foreach (string index in properties.Keys)
            {
                _ = int.TryParse(index, out int indx);
                Node node = Parent.NodeCollection[indx];
                DiagramContainer container = node.Wrapper;
                DiagramContainer tempWrapper = node.Wrapper.Clone() as DiagramContainer;
                Dictionary<string, object> childProperties = properties[index] as Dictionary<string, object>;
                foreach (string propertyName in childProperties.Keys)
                {
                    update = true;
                    object value = childProperties[propertyName];
                    if (value is PropertyChangeValues propertyChangeValues)
                        value = propertyChangeValues.NewValue;
                    if (value != null)
                    {
                        switch (propertyName)
                        {
                            case "Width":

                                if (!(node is NodeGroup))
                                {
                                    container.Children[0].Width = value as double?;
                                }
                                else
                                {
                                    this.ScaleObject(node, (double)value, true);
                                }
                                if (isUndoRedo)
                                {
                                    node.Width = (double)value;
                                }
                                sizeChanged = true;
                                break;
                            case "Height":
                                if (!(node is NodeGroup))
                                {
                                    container.Children[0].Height = value as double?;
                                }
                                else
                                {
                                    this.ScaleObject(node, (double)value, false);
                                }
                                if (isUndoRedo)
                                {
                                    node.Height = (double)value;
                                }
                                sizeChanged = true;
                                break;
                            case "OffsetX":
                                if (this.Parent.DiagramAction.HasFlag(DiagramAction.Layouting) || ConstraintsUtil.CanMove(node) && ConstraintsUtil.CanPageEditable(Parent))
                                {
                                    container.OffsetX = tempWrapper.OffsetX = (double)value;
                                    object oldValueX = childProperties[propertyName];
                                    oldBpmnOffsetX = (double)(oldValueX as PropertyChangeValues).OldValue;
                                    if (isUndoRedo)
                                    {
                                        node.OffsetX = (double)value;
                                    }
                                }
                                break;
                            case "OffsetY":
                                if (this.Parent.DiagramAction.HasFlag(DiagramAction.Layouting) || ConstraintsUtil.CanMove(node) && ConstraintsUtil.CanPageEditable(Parent))
                                {
                                    container.OffsetY = tempWrapper.OffsetY = (double)value;
                                    object oldValueY = childProperties[propertyName];
                                    oldBpmnOffsetY = (double)(oldValueY as PropertyChangeValues).OldValue;
                                    if (isUndoRedo)
                                    {
                                        node.OffsetY = (double)value;
                                    }
                                }
                                break;
                            case "Pivot":
                                container.Pivot = value as DiagramPoint;
                                if (isUndoRedo)
                                {
                                    node.Pivot = value as DiagramPoint;
                                }
                                break;
                            case "MinWidth":
                                container.MinWidth = container.Children[0].MinWidth = value as double?;
                                if (isUndoRedo)
                                {
                                    node.MinWidth = (double)value;
                                }
                                sizeChanged = true;
                                break;
                            case "MinHeight":
                                container.MinHeight = container.Children[0].MinHeight = value as double?;
                                if (isUndoRedo)
                                {
                                    node.MinHeight = (double)value;
                                }
                                sizeChanged = true;
                                break;
                            case "MaxWidth":
                                container.MaxWidth = container.Children[0].MaxWidth = value as double?;
                                if (isUndoRedo)
                                {
                                    node.MaxWidth = (double)value;
                                }
                                sizeChanged = true;
                                break;
                            case "MaxHeight":
                                container.MaxHeight = container.Children[0].MaxHeight = value as double?;
                                if (isUndoRedo)
                                {
                                    node.MaxHeight = (double)value;
                                }
                                sizeChanged = true;
                                break;
                            case "RotationAngle":
                                if (node.Constraints.HasFlag(NodeConstraints.Rotate))
                                {
                                    if (node is NodeGroup && !Parent.RealAction.HasFlag(RealAction.EnableGroupAction))
                                    {
                                        List<NodeBase> children = new List<NodeBase>() { node };
                                        this.Parent.Rotate(node, (double)value - node.Wrapper.RotationAngle, null);
                                    }
                                    container.RotationAngle = (double)value;
                                    if (isUndoRedo)
                                    {
                                        node.RotationAngle = (double)value;
                                    }
                                    isRotating = true;
                                }
                                break;
                            case "Margin":
                                UpdateMargin(node.Margin, container, value as Dictionary<string, object>);
                                sizeChanged = true;
                                break;
                            case "Constraints":
                                if (Enum.TryParse(value.ToString(), out NodeConstraints constraints))
                                {
                                    if (constraints.HasFlag(NodeConstraints.Shadow) && container.Children[0].Shadow == null)
                                    {
                                        container.Children[0].Shadow = node.Shadow;
                                        if (isUndoRedo)
                                        {
                                            node.Shadow = node.Shadow;
                                        }
                                    }
                                    else if (!constraints.HasFlag(NodeConstraints.Shadow) && container.Children[0].Shadow != null)
                                    {
                                        container.Children[0].Shadow = null;
                                        if (isUndoRedo)
                                        {
                                            node.Shadow = new Shadow();
                                        }
                                    }
                                    if (constraints == NodeConstraints.None || !constraints.HasFlag(NodeConstraints.Select))
                                    {
                                        Parent.CommandHandler.ClearSelection();
                                    }
                                    foreach (Node sNode in Parent.SelectionSettings.Nodes)
                                    {
                                        if (node == sNode)
                                        {
                                            sNode.Constraints = constraints;
                                        }
                                    }
                                    DiagramObjectCollection<IDiagramObject> objs = new DiagramObjectCollection<IDiagramObject>(Parent.SelectionSettings.Nodes);

                                    SfDiagramComponent.UpdateThumbConstraints(objs, Parent.SelectionSettings);
                                }
                                break;
                            case "Shadow":
                                if (ConstraintsUtil.CanShadow(node))
                                {
                                    UpdateShadow(node.Shadow, container.Children[0].Shadow, value as Dictionary<string, object>);
                                }
                                update = false;
                                break;
                            case "Shape":
                                DiagramUtil.UpdateShape(node, value as Dictionary<string, object>, newVal);
                                break;
                            case "BackgroundColor":
                                container.Style.Fill = value.ToString();
                                if (isUndoRedo)
                                {
                                    node.BackgroundColor = value.ToString();
                                }
                                update = false;
                                break;
                            case ANNOTATIONS:
                                AnnotationProperty(value as Dictionary<string, object>, node);
                                break;
                            case "Style":
                                UpdateStyle(node.Style, container.Children[0], value as Dictionary<string, object>);
                                update = false;
                                break;
                            case "Ports":
                                PortProperty(value as Dictionary<string, object>, node);
                                break;
                            case "FixedUserHandles":
                                FixedUserHandlePropertyChange(value as Dictionary<string, object>, node);
                                break;
                        }
                    }
                }
                if (node.Shape.Type == Shapes.Bpmn && node.Parent is SfDiagramComponent)
                {
                    BpmnSubProcess subProcess = (node.Shape as BpmnShape).Activity.SubProcess;
                    for (int i = 0; i < node.InEdges.Count; i++)
                    {
                        string val = node.InEdges[i];
                        if (!(node.Parent as SfDiagramComponent).NameTable.ContainsKey(val))
                            node.InEdges.Remove(val);
                    }
                    for (int i = 0; i < node.OutEdges.Count; i++)
                    {
                        string val = node.OutEdges[i];
                        if (!(node.Parent as SfDiagramComponent).NameTable.ContainsKey(val))
                            node.OutEdges.Remove(val);
                    }
                }
                if (node.Shape.Type == Shapes.Bpmn && sizeChanged)
                {
                    this.Parent.BpmnDiagrams.UpdateBpmnSize(node);
                    sizeChanged = false;
                }

                if (update)
                {
                    DiagramRect existingInnerBounds = node.Wrapper.Bounds;
                    if (node.Shape.Type == Shapes.Bpmn && node.Parent is SfDiagramComponent)
                    {
                        BpmnDiagrams.UpdateTextAnnotationProp(node, oldBpmnOffsetX, oldBpmnOffsetY);
                    }
                    container.Measure(new DiagramSize() { Width = container.Bounds.Width, Height = container.Bounds.Height });
                    container.Arrange(container.DesiredSize);
                    if (!isLayout)
                    {
                        this.Parent.CommandHandler.ConnectorSegmentChange(node, existingInnerBounds, (node.Wrapper.RotationAngle != 0));
                    }
                    if (existingInnerBounds != node.Wrapper.OuterBounds)
                    {
                        this.Parent.SpatialSearch.UpdateQuad(node.Wrapper);
                    }
                    if (!isLayout || this.Parent.Layout.Type != LayoutType.OrganizationalChart)
                        UpdateConnectorEdges(node);
                    if (!Parent.FirstRender && !Parent.DiagramAction.HasFlag(DiagramAction.IsGroupDragging) && !isRotating
                        && !Parent.RealAction.HasFlag(RealAction.EnableGroupAction))
                    {
                        this.UpdateGroupOffset(node, false, tempWrapper.OffsetX, tempWrapper.OffsetY);
                    }
                    if (!string.IsNullOrEmpty(node.ParentId) && this.Parent.NameTable.ContainsKey(node.ParentId))
                    {
                        NodeBase parent = this.Parent.NameTable[node.ParentId] as NodeBase;

                        parent.Wrapper.Measure(new DiagramSize() { Width = parent.Wrapper.Width, Height = node.Wrapper.Height });
                        parent.Wrapper.Arrange(parent.Wrapper.DesiredSize);
                    }
                    if (existingInnerBounds != node.Wrapper.OuterBounds && !Parent.FirstRender && !Parent.DiagramAction.HasFlag(DiagramAction.IsGroupDragging) && !isRotating
                       && !Parent.RealAction.HasFlag(RealAction.EnableGroupAction))
                    {
                        UpdateGroupSize(node);
                    }
                }
            }
        }

        internal void ConnectorPropertyChange(Dictionary<string, object> properties, bool? disableBridging = false)
        {
            bool update = true;
            bool canReMeasure = false;
            object newValue = null; object oldValue = null;
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            foreach (string index in properties.Keys)
            {
                Connector connector;
                if (index.Contains("connector", StringComparison.InvariantCulture))
                {
                    connector = Parent.NameTable[index] as Connector;
                }
                else
                {
                    _ = int.TryParse(index, out int indx);
                    connector = Parent.ConnectorCollection[indx];
                }
                bool toGetPoints = false; bool toUpdateEdges = false;
                DiagramContainer container = connector.Wrapper; List<DiagramPoint> points = new List<DiagramPoint>();
                Dictionary<string, object> childProperties = properties[index] as Dictionary<string, object>;
                foreach (string propertyName in childProperties.Keys)
                {
                    update = true;
                    object value = childProperties[propertyName];
                    if (propertyName == "SourceID" || propertyName == "SourcePortID" || propertyName == "SourcePadding" || propertyName == "TargetPadding" || propertyName == "BridgeSpace" || propertyName == "ConnectionPadding" ||
                        propertyName == "TargetID" || propertyName == "TargetPortID" || propertyName == TYPE || propertyName == "Segments" || propertyName == "CornerRadius")
                    {
                        if (childProperties[propertyName] is PropertyChangeValues)
                        {
                            newValue = (childProperties[propertyName] as PropertyChangeValues).NewValue;
                            oldValue = (childProperties[propertyName] as PropertyChangeValues).OldValue;
                        }
                    }
                    else
                    {
                        newValue = childProperties[propertyName];
                        oldValue = childProperties[propertyName];
                    }
                    if (propertyName == TYPE || propertyName == "SourcePoint" || propertyName == "TargetPoint" ||
                        propertyName == "Segments" || propertyName == "SourcePadding" || propertyName == "TargetPadding" ||
                        propertyName == "CornerRadius")
                    {
                        toGetPoints = true;
                    }

                    switch (propertyName)
                    {
                        case TYPE:
                            connector.Segments = new DiagramObjectCollection<ConnectorSegment>();
                            if (Enum.TryParse(newValue.ToString(), out ConnectorSegmentType segments1))
                            {
                                connector.Type = segments1;
                            }
                            break;
                        case "BpmnFlow":
                            ConnectorUtil.UpdateConnectorShape(connector, value as Dictionary<string, object>);
                            break;
                        case "CornerRadius":
                            if (newValue != null) connector.CornerRadius = (double)newValue;
                            break;
                        case "BridgeSpace":
                            if (newValue != null) connector.BridgeSpace = (double)newValue;
                            break;
                        case "ConnectionPadding":
                            if (newValue != null) connector.ConnectionPadding = (double)newValue;
                            break;
                        case "Segments":
                            DiagramObjectCollection<ConnectorSegment> segments = newValue as DiagramObjectCollection<ConnectorSegment>;
                            if (segments.Count == 0 || (segments.Count > 0 && connector.Type == segments[0].Type))
                            {

                            }
                            break;
                        case "SourcePoint":
                            UpdatePointProperties(connector.SourcePoint, newValue as Dictionary<string, object>);
                            break;
                        case "TargetPoint":
                            UpdatePointProperties(connector.TargetPoint, newValue as Dictionary<string, object>);
                            break;
                        case "SourcePadding":
                            if (newValue != null) connector.SourcePadding = (double)newValue;
                            break;
                        case "TargetPadding":
                            if (newValue != null) connector.TargetPadding = (double)newValue;
                            break;
                        case "SourceID":
                        case "SourcePortID":
                            if (oldValue != newValue)
                            {
                                Node sourceNode = (propertyName == "SourceID")
                                    ? GetNode(newValue.ToString())
                                    : GetNode(connector.SourceID);
                                string portId = (propertyName == "SourcePortID")
                                    ? newValue.ToString()
                                    : connector.SourcePortID;
                                PointPort outPort = GetPort(sourceNode, portId);
                                if (propertyName == "SourceID")
                                {
                                    connector.SourceID = newValue.ToString();

                                    if ((sourceNode != null && ConstraintsUtil.CanOutConnect(sourceNode)) || connector.SourceWrapper != null)
                                    {
                                        connector.SourceWrapper = sourceNode != null ? GetEndNodeWrapper(sourceNode, connector, true) as DiagramElement : null;

                                    }
                                    if ((!string.IsNullOrEmpty(connector.SourcePortID) && ConstraintsUtil.CanPortOutConnect(outPort)) || connector.SourcePortWrapper != null)
                                    {
                                        connector.SourcePortWrapper = sourceNode != null ? DiagramUtil.GetWrapper(
                                            sourceNode, sourceNode.Wrapper, connector.SourcePortID) as DiagramElement : null;
                                    }
                                }
                                if (propertyName == "SourcePortID")
                                {
                                    connector.SourcePortID = newValue.ToString();
                                    if (sourceNode != null && ConstraintsUtil.CanOutConnect(sourceNode) && !string.IsNullOrEmpty(connector.SourcePortID) && ConstraintsUtil.CanPortOutConnect(outPort))
                                    {
                                        connector.SourcePortWrapper = DiagramUtil.GetWrapper(sourceNode, sourceNode.Wrapper, newValue.ToString()) as DiagramElement;
                                    }
                                    else
                                    {
                                        connector.SourcePortWrapper = null;
                                    }
                                }
                                string oldNodeId = propertyName == "SourceID" ? oldValue as string : connector.SourceID;
                                string oldPortId = propertyName == "SourcePortID" ? oldValue as string : connector.SourcePortID;
                                RemoveEdges(GetNode(oldNodeId), oldPortId, connector.ID, false);
                                toUpdateEdges = true;
                            }
                            break;
                        case "TargetID":
                        case "TargetPortID":
                            if (oldValue != newValue)
                            {
                                Node targetNode = (propertyName == "TargetID") ? GetNode(newValue.ToString()) : GetNode(connector.TargetID);
                                string portId = (propertyName == "TargetPortID") ? newValue.ToString() : connector.TargetPortID;
                                PointPort inPort = GetPort(targetNode, portId);
                                if (propertyName == "TargetID")
                                {
                                    connector.TargetID = newValue.ToString();
                                    if ((targetNode != null && ConstraintsUtil.CanOutConnect(targetNode)) || connector.TargetWrapper != null)
                                    {
                                        connector.TargetWrapper = targetNode != null ? GetEndNodeWrapper(targetNode, connector, true) as DiagramElement : null;
                                    }
                                    if ((!string.IsNullOrEmpty(connector.TargetPortID) && ConstraintsUtil.CanPortOutConnect(inPort)) || connector.TargetPortWrapper != null)
                                    {
                                        connector.TargetPortWrapper = targetNode != null ? DiagramUtil.GetWrapper(
                                            targetNode, targetNode.Wrapper, connector.TargetPortID) as DiagramElement : null;
                                    }
                                }
                                if (propertyName == "TargetPortID")
                                {
                                    connector.TargetPortID = newValue.ToString();
                                    if (targetNode != null && ConstraintsUtil.CanOutConnect(targetNode) && !string.IsNullOrEmpty(connector.TargetPortID) && ConstraintsUtil.CanPortOutConnect(inPort))
                                    {
                                        connector.TargetPortWrapper = DiagramUtil.GetWrapper(targetNode, targetNode.Wrapper, newValue.ToString()) as DiagramElement;
                                    }
                                    else
                                        connector.TargetPortWrapper = null;
                                }
                                string oldNodeId = propertyName == "TargetID" ? oldValue as string : connector.TargetID;
                                string oldPortId = propertyName == "TargetPortID" ? oldValue as string : connector.TargetPortID;
                                RemoveEdges(GetNode(oldNodeId), oldPortId, connector.ID, true);
                                toUpdateEdges = true;
                            }
                            break;
                        case "Style":
                            UpdateStyle(connector.Style, connector.Wrapper.Children[0], newValue as Dictionary<string, object>);
                            update = false;
                            break;
                        case "SourceDecorator":
                            toGetPoints = DecoratorPropertyChange(connector.Wrapper.Children[1] as PathElement, newValue as Dictionary<string, object>, connector.SourceDecorator, toGetPoints, isUndoRedo);
                            break;
                        case "TargetDecorator":
                            toGetPoints = DecoratorPropertyChange(connector.Wrapper.Children[2] as PathElement, newValue as Dictionary<string, object>, connector.TargetDecorator, toGetPoints, isUndoRedo);
                            break;
                        case ANNOTATIONS:
                            AnnotationProperty(newValue as Dictionary<string, object>, connector);
                            break;
                        case "FixedUserHandles":
                            FixedUserHandlePropertyChange(value as Dictionary<string, object>, connector);
                            break;
                        case "Constraints":
                            if (Enum.TryParse((value as PropertyChangeValues).NewValue.ToString(), out ConnectorConstraints constraints))
                            {
                                if (constraints == ConnectorConstraints.None || !constraints.HasFlag(ConnectorConstraints.Select))
                                {
                                    Parent.CommandHandler.ClearSelection();
                                }
                                foreach (Connector sConnector in Parent.SelectionSettings.Connectors)
                                {
                                    if (connector == sConnector)
                                    {
                                        sConnector.Constraints = constraints;
                                    }
                                }
                                DiagramObjectCollection<IDiagramObject> connectorsCollection = new DiagramObjectCollection<IDiagramObject>(Parent.SelectionSettings.Connectors);
                                SfDiagramComponent.UpdateThumbConstraints(connectorsCollection, Parent.SelectionSettings);
                            }
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(connector.TargetID))
                {
                    connector.TargetWrapper = GetEndNodeWrapper(GetNode(connector.TargetID), connector, false) as DiagramElement;
                }
                if (!string.IsNullOrEmpty(connector.SourceID))
                {
                    connector.SourceWrapper = GetEndNodeWrapper(GetNode(connector.SourceID), connector, true) as DiagramElement;
                }
                if (toUpdateEdges)
                {
                    UpdateEdges(connector);
                }
                if (toGetPoints || toUpdateEdges)
                {
                    points = points.Count > 0 ? points : connector.GetConnectorPoints();
                    DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                }
                if (connector.Shape != null)
                {
                    connector.UpdateShapeElement(connector);
                }
                if (connector.Shape.Type == ConnectorShapeType.Bpmn && connector.Shape.Sequence == BpmnSequenceFlows.Default && connector.Shape.Flow == BpmnFlows.Sequence)
                {
                    UpdatePathElementOffset(connector);
                }
                if (disableBridging.HasValue && !disableBridging.Value)
                {
                    this.UpdateBridging();
                }
                this.Parent.SpatialSearch.UpdateQuad(connector.Wrapper);
                if (update)
                {
                    (container as Canvas).Measure(new DiagramSize() { Width = container.Bounds.Width, Height = container.Bounds.Height });
                    (container as Canvas).Arrange(container.DesiredSize, true);
                    if (!string.IsNullOrEmpty(connector.ParentId) && this.Parent.NameTable[connector.ParentId] != null)
                    {
                        NodeBase parent = this.Parent.NameTable[connector.ParentId] as NodeBase;

                        while (parent != null)
                        {
                            parent.Wrapper.Measure(new DiagramSize() { Width = parent.Wrapper.Width, Height = connector.Wrapper.Height });
                            parent.Wrapper.Arrange(parent.Wrapper.DesiredSize);
                            (parent as Node).OffsetX = parent.Wrapper.OffsetX;
                            (parent as Node).OffsetY = parent.Wrapper.OffsetY;
                            if (!string.IsNullOrEmpty(parent.ParentId))
                                parent = this.Parent.NameTable[parent.ParentId] as NodeGroup;
                            else
                                parent = null;
                        }
                    }
                }
                for (int i = 0; i < connector.Annotations.Count; i++)
                {
                    canReMeasure = true;
                    TextElement textElement = DiagramUtil.GetWrapper(connector, connector.Wrapper, connector.Annotations[i].ID) as TextElement;
                    connector.UpdateAnnotation(connector.Annotations[i], connector.IntermediatePoints, connector.Wrapper.Bounds, textElement);
                }
                if (canReMeasure)
                {
                    (container as Canvas).Measure(new DiagramSize() { Width = container.Bounds.Width, Height = container.Bounds.Height });
                    (container as Canvas).Arrange(container.DesiredSize, true);
                }
            }
        }

        internal int GetIndex(object id)
        {
            int index = 0, i = 0;

            if (Parent.NodeCollection[i].ID == id.ToString())
            {
                index = i;
            }

            return index;
        }
        internal void UpdateElementVisibility(DiagramContainer element, NodeBase obj, bool visible)
        {
            element.Visible = visible;
            if (obj is Node node)
            {
                if (!(node is NodeGroup groupObj) || groupObj.Children == null)
                {
                    element.Children[0].Visible = visible;
                    UpdateDiagramContainerVisibility(element.Children[0] as DiagramContainer, visible);
                    if (node.Shape.Type == Shapes.Bpmn)
                    {
                        BpmnDiagrams.UpdateElementVisibility(node, visible, this.Parent);
                    }
                }
                else
                {
                    foreach (string child in groupObj.Children)
                    {
                        this.UpdateElementVisibility(((NodeBase)this.Parent.NameTable[child]).Wrapper, (NodeBase)this.Parent.NameTable[child], visible);
                    }
                }
            }
        }
        private void UpdateDiagramContainerVisibility(ICommonElement element, bool visible)
        {
            if (element is DiagramContainer container)
            {
                for (int i = 0; i < container.Children.Count; i++)
                {
                    this.UpdateDiagramContainerVisibility(container.Children[i], visible);
                }
            }
            element.Visible = visible;
        }
        internal static void UpdateUserHandle(NodeBase node, DiagramContainer nodeContainer, string id)
        {
            id = nodeContainer.ID + "_" + id;
            DiagramContainer container = nodeContainer is Canvas ? nodeContainer : DiagramUtil.GetPortContainer(node);
            for (int i = 0; i < container.Children.Count; i++)
            {
                if (id == ((DiagramElement)container.Children[i]).ID)
                {
                    container.Children.Remove(container.Children[i]);
                }
            }
        }
        private void FixedUserHandlePropertyChange(Dictionary<string, object> properties, IDiagramObject parent)
        {
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            if (properties != null)
            {
                foreach (string index in properties.Keys)
                {
                    FixedUserHandle userHandle;
                    Canvas canvas;
                    int indx;
                    if (parent is Node node1)
                    {
                        _ = int.TryParse(index, out indx);
                        userHandle = node1.FixedUserHandles[indx];
                        canvas = DiagramUtil.GetWrapper(node1, node1.Wrapper, userHandle.ID) as Canvas;
                    }
                    else
                    {
                        _ = int.TryParse(index, out indx);
                        userHandle = (parent as Connector).FixedUserHandles[indx];
                        canvas = DiagramUtil.GetWrapper((parent as Connector), (parent as Connector).Wrapper, userHandle.ID) as Canvas;
                    }
                    Dictionary<string, object> property = properties[index] as Dictionary<string, object>;
                    PathElement wrapper = canvas.Children[0] as PathElement;
                    bool isMeasure = false;
                    foreach (string key in property.Keys)
                    {
                        object value = property[key];
                        if (value is PropertyChangeValues propertyChangeValues)
                            value = propertyChangeValues.NewValue;
                        switch (key)
                        {
                            case "Offset":
                                isMeasure = true;
                                if (parent is Node)
                                {
                                    NodeFixedUserHandle nodeFixedUserHandle = userHandle as NodeFixedUserHandle;
                                    DiagramPoint points = UpdateOffset(value as Dictionary<string, object>, nodeFixedUserHandle.Offset);
                                    nodeFixedUserHandle.Offset.X = points.X;
                                    nodeFixedUserHandle.Offset.Y = points.Y;
                                }
                                else
                                {
                                    ConnectorFixedUserHandle connectorFixedUserHandle = userHandle as ConnectorFixedUserHandle;
                                    if (double.TryParse(value.ToString(), out double offset))
                                        connectorFixedUserHandle.Offset = offset;
                                }
                                break;
                            case "Fill":
                                canvas.Style.Fill = value.ToString();
                                if (isUndoRedo)
                                {
                                    userHandle.Fill = value.ToString();
                                }
                                break;
                            case "HandleStrokeColor":
                                canvas.Style.StrokeColor = value.ToString();
                                if (isUndoRedo)
                                {
                                    userHandle.Stroke = value.ToString();
                                }
                                break;
                            case "IconStrokeColor":
                                wrapper.Style.Fill = value.ToString();
                                wrapper.Style.StrokeColor = value.ToString();
                                if (isUndoRedo)
                                {
                                    userHandle.IconStroke = value.ToString();
                                }
                                break;
                            case "IconStrokeWidth":
                                isMeasure = true;
                                if (double.TryParse(value.ToString(), out var iconStrokeWidth))
                                {
                                    wrapper.Style.StrokeWidth = iconStrokeWidth;
                                    if (isUndoRedo)
                                    {
                                        userHandle.IconStrokeThickness = iconStrokeWidth;
                                    }
                                }
                                break;
                            case "HandleStrokeWidth":
                                isMeasure = true;
                                if (double.TryParse(value.ToString(), out var handleStrokeWidth))
                                {
                                    canvas.Style.StrokeWidth = handleStrokeWidth;
                                    if (isUndoRedo)
                                    {
                                        userHandle.StrokeThickness = handleStrokeWidth;
                                    }
                                }
                                break;
                            case "PathData":
                                string path = value.ToString();
                                wrapper.Data = path;
                                isMeasure = true;
                                break;
                            case "Visibility":
                                if (bool.TryParse(value.ToString(), out var visibility))
                                {
                                    canvas.Visible = visibility;
                                    wrapper.Visible = visibility;
                                }
                                break;
                            case "CornerRadius":
                                isMeasure = true;
                                if (double.TryParse(value.ToString(), out var cornerRadius))
                                {
                                    userHandle.CornerRadius = cornerRadius;
                                }
                                break;
                            case "Height":
                                isMeasure = true;
                                if (double.TryParse(value.ToString(), out var height))
                                {
                                    userHandle.Height = height;
                                }
                                break;
                            case "Width":
                                isMeasure = true;
                                if (double.TryParse(value.ToString(), out var width))
                                {
                                    userHandle.Width = width;
                                }
                                break;
                            case "Alignment":
                                isMeasure = true;
                                ConnectorFixedUserHandle connectorFixedUserHandles = userHandle as ConnectorFixedUserHandle;
                                if (Enum.TryParse(value.ToString(), out FixedUserHandleAlignment alignment))
                                {
                                    connectorFixedUserHandles.Alignment = alignment;
                                }
                                break;
                            case "Displacement":
                                isMeasure = true;
                                ConnectorFixedUserHandle connectorFixedUserHandleDisplacement = userHandle as ConnectorFixedUserHandle;
                                DiagramPoint point = UpdateOffset(value as Dictionary<string, object>, connectorFixedUserHandleDisplacement.Displacement);
                                connectorFixedUserHandleDisplacement.Displacement.X = point.X;
                                connectorFixedUserHandleDisplacement.Displacement.Y = point.Y;
                                break;
                            case "Margin":
                                isMeasure = true;
                                NodeFixedUserHandle nodeFixedUserHandleMargin = userHandle as NodeFixedUserHandle;
                                UpdateMarginValue(value as Dictionary<string, object>, nodeFixedUserHandleMargin.Margin);
                                break;
                            case "Padding":
                                isMeasure = true;
                                break;
                        }
                    }
                    if (isMeasure)
                    {
                        if (parent is Node node)
                        {
                            UpdateUserHandle(node, node.Wrapper, userHandle.ID);
                            DiagramElement handle = node.InitFixedUserHandles(userHandle as NodeFixedUserHandle);
                            node.Wrapper.Children.Add(handle);
                        }
                        else
                        {
                            Connector connector = parent as Connector;
                            UpdateUserHandle(connector, connector.Wrapper, userHandle.ID);
                            DiagramElement handle = connector.GetFixedUserHandle(userHandle as ConnectorFixedUserHandle, connector.IntermediatePoints, connector.Bounds);
                            connector.Wrapper.Children.Add(handle);
                        }
                        wrapper.Measure(new DiagramSize() { Width = userHandle.Width, Height = userHandle.Height });
                        wrapper.Arrange(wrapper.DesiredSize, null);
                    }
                }
            }
        }
        private void PortProperty(Dictionary<string, object> properties, IDiagramObject parent)
        {
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            if (properties != null)
            {
                foreach (string index in properties.Keys)
                {
                    _ = int.TryParse(index, out int indx);
                    PointPort port = (parent as Node).Ports[indx];
                    Dictionary<string, object> property = properties[index] as Dictionary<string, object>;
                    PathElement wrapper = DiagramUtil.GetWrapper((parent as Node), (parent as Node).Wrapper, port.ID) as PathElement;
                    bool isMeasure = false;
                    foreach (string key in property.Keys)
                    {
                        object value = property[key];
                        if (value is PropertyChangeValues values)
                            value = values.NewValue;
                        switch (key)
                        {
                            case "Offset":
                                isMeasure = true;
                                DiagramPoint point = UpdateOffset(value as Dictionary<string, object>, port.Offset);
                                wrapper.SetOffsetWithRespectToBounds(point.X, point.Y, UnitMode.Fraction);
                                if (isUndoRedo)
                                    port.Offset = point;
                                break;
                            case "ShapeStyle":
                                UpdateStyle(port.Style, wrapper, value as Dictionary<string, object>);
                                break;
                            case "Style":
                                ShapeStyle styleProperty = (value as ShapeStyle);
                                UpdateStyle(port.Style, wrapper, value as Dictionary<string, object>);
                                break;
                            case "PathData":
                                string path = value.ToString();
                                wrapper.Data = path;
                                isMeasure = true;
                                break;
                            case "Shape":
                                if (Enum.TryParse(value.ToString(), out PortShapes shape))
                                {
                                    string pathData = shape == PortShapes.Custom ? port.PathData : Dictionary.GetShapeData(shape.ToString());
                                    if (shape != PortShapes.Custom)
                                    {
                                        port.PathData = pathData;
                                        wrapper.Data = pathData;
                                    }
                                    if (isUndoRedo)
                                        port.Shape = shape;
                                    wrapper.CanMeasurePath = true;
                                }
                                isMeasure = true;
                                break;
                            case "Visibility":
                                if (Enum.TryParse(value.ToString(), out PortVisibility visibility))
                                {
                                    PointPort tempPort = port.Clone() as PointPort;
                                    tempPort.Visibility = visibility;
                                    bool visible = (parent as Node).IsVisible && ConstraintsUtil.CheckPortRestriction(tempPort, PortVisibility.Visible) > 0;
                                    wrapper.Visible = visible;
                                    if (isUndoRedo)
                                        port.Visibility = visibility;
                                }
                                break;
                            case "Constraints":
                                if (Enum.TryParse(value.ToString(), out PortConstraints constraints))
                                {
                                    if (isUndoRedo)
                                        port.Constraints = constraints;
                                }
                                break;
                            case "Height":
                                isMeasure = true;
                                if (double.TryParse(value.ToString(), out var height))
                                {
                                    wrapper.Height = height;
                                    if (isUndoRedo)
                                        port.Height = height;
                                }
                                break;
                            case "Width":
                                isMeasure = true;
                                if (double.TryParse(value.ToString(), out var width))
                                {
                                    wrapper.Width = width;
                                    if (isUndoRedo)
                                        port.Width = width;
                                }
                                break;
                            case "VerticalAlignment":
                                isMeasure = true;
                                if (Enum.TryParse(value.ToString(), out VerticalAlignment verticalAlignment))
                                {
                                    wrapper.VerticalAlignment = verticalAlignment;
                                    if (isUndoRedo)
                                        port.VerticalAlignment = verticalAlignment;
                                }
                                break;
                            case "HorizontalAlignment":
                                isMeasure = true;
                                if (Enum.TryParse(value.ToString(), out HorizontalAlignment horizontalAlignment))
                                {
                                    wrapper.HorizontalAlignment = horizontalAlignment;
                                    if (isUndoRedo)
                                        port.HorizontalAlignment = horizontalAlignment;
                                }
                                break;
                            case "Margin":
                                isMeasure = true;
                                UpdateMargin(port.Margin, wrapper, value as Dictionary<string, object>);
                                break;
                        }
                    }
                    if (isMeasure)
                    {
                        wrapper.Measure(new DiagramSize() { Width = port.Width, Height = port.Height });
                        wrapper.Arrange(wrapper.DesiredSize, null);
                    }
                }
            }
        }

        private void AnnotationProperty(Dictionary<string, object> properties, IDiagramObject parent)
        {
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            foreach (string index in properties.Keys)
            {
                Annotation annotation;
                _ = int.TryParse(index, out int indx);
                if (parent is Node node)
                {
                    annotation = node.Annotations[indx];
                }
                else
                {
                    annotation = (parent as Connector).Annotations[indx];
                }
                Dictionary<string, object> property = properties[index] as Dictionary<string, object>;
                bool isMeasure = false;
                bool updatePathAnnotation = false;
                TextElement annotationWrapper = DiagramUtil.GetWrapper((parent as NodeBase), (parent as NodeBase).Wrapper, annotation.ID) as TextElement;
                annotationWrapper.RefreshTextElement();
                if (annotationWrapper != null)
                {
                    foreach (string key in property.Keys)
                    {
                        object value = property[key];
                        if (value is PropertyChangeValues values)
                            value = values.NewValue;
                        switch (key)
                        {
                            case "Height":
                                annotationWrapper.Height = (double?)value;
                                if (isUndoRedo)
                                    annotation.Height = (double?)value;
                                isMeasure = true;
                                break;
                            case "Width":
                                annotationWrapper.Width = (double?)value;
                                if (isUndoRedo)
                                    annotation.Width = (double?)value;
                                isMeasure = true;
                                break;
                            case "RotationAngle":
                                annotationWrapper.RotationAngle = (double)value;
                                if (isUndoRedo)
                                    annotation.RotationAngle = (double)value;
                                break;
                            case "Margin":
                                UpdateMargin(annotation.Margin, annotationWrapper, value as Dictionary<string, object>);
                                isMeasure = true;
                                break;
                            case "VerticalAlignment":
                                isMeasure = true;
                                if (Enum.TryParse(value.ToString(), out VerticalAlignment verticalAlignment))
                                {
                                    annotationWrapper.VerticalAlignment = verticalAlignment;
                                    if (isUndoRedo)
                                        annotation.VerticalAlignment = verticalAlignment;
                                }
                                break;
                            case "HorizontalAlignment":
                                isMeasure = true;
                                if (Enum.TryParse(value.ToString(), out HorizontalAlignment horizontalAlignment))
                                {
                                    annotationWrapper.HorizontalAlignment = horizontalAlignment;
                                    if (isUndoRedo)
                                        annotation.HorizontalAlignment = horizontalAlignment;
                                }
                                break;
                            case "Visibility":
                                bool visible = (bool)value;
                                bool parentVisible = (parent is Node node1) ? node1.IsVisible : (parent as Connector).IsVisible;
                                annotationWrapper.Visible = (parentVisible && visible);
                                break;
                            case "Constraints":
                                if (Enum.TryParse(value.ToString(), out AnnotationConstraints constraints))
                                {
                                    annotationWrapper.Constraints = constraints;
                                    if (isUndoRedo)
                                        annotation.Constraints = constraints;
                                }
                                break;
                            case "Style":
                                if (value is Dictionary<string, object> objects)
                                {
                                    UpdateStyle(annotation.Style, annotationWrapper, objects);
                                }
                                else
                                {
                                    annotationWrapper.Style = value as TextStyle;
                                    if (isUndoRedo)
                                        annotation.Style = value as TextStyle;
                                }
                                break;
                            case "Hyperlink":
                                UpdateHyperlink(annotationWrapper, value as Dictionary<string, object>, annotation);
                                break;
                            case "Content":
                                annotationWrapper.Content = value.ToString();
                                if (isUndoRedo)
                                    annotation.Content = value.ToString();
                                isMeasure = true;
                                break;
                            case "Offset":
                                if ((annotation is ShapeAnnotation shapeAnnotation) && shapeAnnotation.Offset != null)
                                {
                                    DiagramPoint offset = UpdateOffset(value as Dictionary<string, object>, shapeAnnotation.Offset);
                                    annotationWrapper.SetOffsetWithRespectToBounds(offset.X, offset.Y, UnitMode.Fraction);
                                    annotationWrapper.RelativeMode = RelativeMode.Point;
                                    if (isUndoRedo)
                                        shapeAnnotation.Offset = offset;
                                }
                                else
                                {
                                    if (annotation is PathAnnotation)
                                        updatePathAnnotation = true;
                                    if (isUndoRedo)
                                        (annotation as PathAnnotation).Offset = (double)value;
                                }
                                isMeasure = true;
                                break;
                            case "Displacement":
                                if (value is DiagramPoint point1)
                                {
                                    (annotation as PathAnnotation).Displacement = point1;
                                    if (isUndoRedo)
                                        (annotation as PathAnnotation).Displacement = point1;
                                }
                                else
                                {
                                    DiagramPoint point = UpdateOffset(value as Dictionary<string, object>, (annotation as PathAnnotation).Displacement);
                                    (annotation as PathAnnotation).Displacement = point;
                                    if (isUndoRedo)
                                        (annotation as PathAnnotation).Displacement = point;
                                }
                                isMeasure = true;
                                updatePathAnnotation = true;
                                break;
                            case "Alignment":
                                updatePathAnnotation = true;
                                if (Enum.TryParse(value.ToString(), out AnnotationAlignment alignment))
                                {
                                    if (isUndoRedo)
                                        (annotation as PathAnnotation).Alignment = alignment;
                                }
                                break;
                            case "SegmentAngle":
                                updatePathAnnotation = true;
                                break;
                        }
                        if (updatePathAnnotation)
                            (parent as Connector).UpdateAnnotation(annotation as PathAnnotation, (parent as Connector).IntermediatePoints, (parent as Connector).Bounds, annotationWrapper);
                        if (isMeasure)
                        {
                            annotationWrapper.Measure(new DiagramSize() { Width = annotationWrapper.Width ?? 0, Height = annotationWrapper.Height ?? 0 });
                            annotationWrapper.Arrange(annotationWrapper.DesiredSize, false);
                        }
                    }
                }
            }
        }

        private static DiagramPoint UpdateOffset(Dictionary<string, object> property, DiagramPoint oldOffset)
        {
            if (property != null)
            {
                foreach (string key in property.Keys)
                {
                    object value = property[key];
                    if (value is PropertyChangeValues values)
                        value = values.NewValue;
                    switch (key)
                    {
                        case "X":
                            oldOffset.X = (double)value;
                            break;
                        case "Y":
                            oldOffset.Y = (double)value;
                            break;
                    }
                }
            }
            return oldOffset;
        }
        private static void UpdateMarginValue(Dictionary<string, object> property, Margin oldMargin)
        {
            if (property != null)
            {
                foreach (string key in property.Keys)
                {
                    object value = property[key];
                    if (value is PropertyChangeValues values)
                        value = values.NewValue;
                    switch (key)
                    {
                        case "Right":
                            oldMargin.Right = (double)value;
                            break;
                        case "Left":
                            oldMargin.Left = (double)value;
                            break;
                        case "Bottom":
                            oldMargin.Bottom = (double)value;
                            break;
                        case "Top":
                            oldMargin.Top = (double)value;
                            break;
                    }
                }
            }
        }

        private void UpdateMargin(Margin elementMargin, ICommonElement wrapper, Dictionary<string, object> marginPpty)
        {
            Margin margin = wrapper.Margin;
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            foreach (string key in marginPpty.Keys)
            {
                object value = marginPpty[key];
                if (value is PropertyChangeValues values)
                    value = values.NewValue;
                switch (key)
                {
                    case "Right":
                        margin.Right = (double)value;
                        if (isUndoRedo)
                            elementMargin.Right = (double)value;
                        break;
                    case "Left":
                        margin.Left = (double)value;
                        if (isUndoRedo)
                            elementMargin.Left = (double)value;
                        break;
                    case "Bottom":
                        margin.Bottom = (double)value;
                        if (isUndoRedo)
                            elementMargin.Bottom = (double)value;
                        break;
                    case "Top":
                        margin.Top = (double)value;
                        if (isUndoRedo)
                            elementMargin.Top = (double)value;
                        break;
                }
            }
        }
        private void UpdateStyle(ShapeStyle style, ICommonElement wrapper, Dictionary<string, object> stylePpty)
        {
            ShapeStyle shapeStyle = wrapper.Style;
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            bool isBpmnShape = false;
            Canvas canvas = wrapper as Canvas;
            if (style.Parent is Node node && (node.Shape.Type == Shapes.Bpmn))
            {
                if (canvas != null && canvas.Children.Count > 0)
                {
                    isBpmnShape = true;
                }
            }
            foreach (string key in stylePpty.Keys)
            {
                object value = stylePpty[key];
                if (value is PropertyChangeValues values)
                    value = values.NewValue;
                switch (key)
                {
                    case "Fill":
                        if (isBpmnShape)
                        {
                            canvas.Children[0].Style.Fill = value.ToString();
                        }
                        else
                        {
                            if (shapeStyle.Parent is Connector)
                            {
                                shapeStyle.Fill = "transparent";
                            }
                            else
                            {
                                shapeStyle.Fill = value.ToString();
                            }
                        }
                        if (isUndoRedo)
                            if (style.Parent is Connector)
                            {
                                style.Fill = "transparent";
                            }
                            else
                            {
                                style.Fill = value.ToString();
                            }
                        break;
                    case "Opacity":
                        if (isBpmnShape)
                        {
                            canvas.Children[0].Style.Opacity = (double)value;
                        }
                        else
                        {
                            shapeStyle.Opacity = (double)value;
                        }
                        if (isUndoRedo)
                            style.Opacity = (double)value;
                        break;
                    case "StrokeColor":
                        if (isBpmnShape)
                        {
                            canvas.Children[0].Style.StrokeColor = value.ToString();
                        }
                        else
                        {
                            shapeStyle.StrokeColor = value.ToString();
                        }
                        if (isUndoRedo)
                            style.StrokeColor = value.ToString();
                        break;
                    case "StrokeDashArray":
                        if (isBpmnShape)
                        {
                            canvas.Children[0].Style.StrokeDashArray = value.ToString();
                        }
                        else
                        {
                            shapeStyle.StrokeDashArray = value.ToString();
                        }
                        if (isUndoRedo)
                            style.StrokeDashArray = value.ToString();
                        break;
                    case "Gradient":
                        UpdateGradient(shapeStyle, value as Dictionary<string, object>, style.Gradient);
                        break;
                    case "StrokeWidth":
                        if (isBpmnShape)
                        {
                            canvas.Children[0].Style.StrokeWidth = (double)value;
                        }
                        else
                        {
                            shapeStyle.StrokeWidth = (double)value;
                        }
                        if (isUndoRedo)
                            style.StrokeWidth = (double)value;
                        break;
                    case "TextAlign":
                        if (Enum.TryParse(value.ToString(), out TextAlign textAlign))
                        {
                            (shapeStyle as TextStyle).TextAlign = textAlign;
                            if (isUndoRedo)
                                (style as TextStyle).TextAlign = textAlign;
                        }
                        break;
                    case "TextDecoration":
                        if (Enum.TryParse(value.ToString(), out TextDecoration textDecoration))
                        {
                            (shapeStyle as TextStyle).TextDecoration = textDecoration;
                            if (isUndoRedo)
                                (style as TextStyle).TextDecoration = textDecoration;
                        }
                        break;
                    case "TextWrapping":
                        if (Enum.TryParse(value.ToString(), out TextWrap textWrap))
                        {
                            (shapeStyle as TextStyle).TextWrapping = textWrap;
                            if (isUndoRedo)
                                (style as TextStyle).TextWrapping = textWrap;
                        }
                        break;
                    case "TextOverflow":
                        if (Enum.TryParse(value.ToString(), out TextOverflow textOverflow))
                        {
                            (shapeStyle as TextStyle).TextOverflow = textOverflow;
                            if (isUndoRedo)
                                (style as TextStyle).TextOverflow = textOverflow;
                        }
                        if (UpdateCustomBoundsCount == 0)
                        {
                            Parent.RealAction &= ~RealAction.PreventRefresh;
                        }
                        break;
                    case "Color":
                        (shapeStyle as TextStyle).Color = value.ToString();
                        if (isUndoRedo)
                            (style as TextStyle).Color = value.ToString();
                        break;
                    case "Bold":
                        (shapeStyle as TextStyle).Bold = (bool)value;
                        if (isUndoRedo)
                            (style as TextStyle).Bold = (bool)value;
                        break;
                    case "FontFamily":
                        (shapeStyle as TextStyle).FontFamily = value.ToString();
                        if (isUndoRedo)
                            (style as TextStyle).FontFamily = value.ToString();
                        break;
                    case "FontSize":
                        (shapeStyle as TextStyle).FontSize = (double)value;
                        if (isUndoRedo)
                            (style as TextStyle).FontSize = (double)value;
                        break;
                    case "Italic":
                        (shapeStyle as TextStyle).Italic = (bool)value;
                        if (isUndoRedo)
                            (style as TextStyle).Italic = (bool)value;
                        break;
                    case "WhiteSpace":
                        if (Enum.TryParse(value.ToString(), out WhiteSpace whiteSpace))
                        {
                            (shapeStyle as TextStyle).WhiteSpace = whiteSpace;
                            if (isUndoRedo)
                                (style as TextStyle).WhiteSpace = whiteSpace;
                        }
                        break;
                }
            }
        }

        private void UpdateGradient(ShapeStyle shapeStyle, Dictionary<string, object> property, GradientBrush gradientContainer)
        {
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            if (property != null && gradientContainer != null)
            {
                shapeStyle.Gradient = gradientContainer.BrushType == GradientType.Linear
                    ? (GradientBrush)new LinearGradientBrush()
                    : new RadialGradientBrush();

                GradientBrush gradient = shapeStyle.Gradient;
                if (gradient != null)
                {
                    foreach (string key in property.Keys)
                    {
                        object value = property[key];
                        if (value is PropertyChangeValues values)
                            value = values.NewValue;
                        switch (key)
                        {
                            case "GradientStops":
                                if (value != null)
                                    UpdateGradientStops(shapeStyle.Gradient, gradient, value as Dictionary<string, object>);
                                break;
                            case "BrushType":
                                if (Enum.TryParse(value.ToString(), out GradientType gradientType))
                                {
                                    gradient.BrushType = gradientType;
                                    if (isUndoRedo)
                                        shapeStyle.Gradient.BrushType = gradientType;
                                }
                                break;
                            case "X1":
                                if (gradient is LinearGradientBrush linearGradient)
                                {
                                    linearGradient.X1 = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as LinearGradientBrush).X1 = (double)value;
                                }
                                break;
                            case "X2":
                                if (gradient is LinearGradientBrush gradient1)
                                {
                                    gradient1.X2 = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as LinearGradientBrush).X2 = (double)value;
                                }
                                break;
                            case "Y1":
                                if (gradient is LinearGradientBrush linearGradient1)
                                {
                                    linearGradient1.Y1 = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as LinearGradientBrush).Y1 = (double)value;
                                }
                                break;
                            case "Y2":
                                if (gradient is LinearGradientBrush gradient2)
                                {
                                    gradient2.Y2 = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as LinearGradientBrush).Y2 = (double)value;
                                }
                                break;
                            case "R":
                                if (gradient is RadialGradientBrush radialGradient)
                                {
                                    radialGradient.R = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as RadialGradientBrush).R = (double)value;
                                }
                                break;
                            case "CX":
                                if (gradient is RadialGradientBrush radialGradient1)
                                {
                                    radialGradient1.CX = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as RadialGradientBrush).CX = (double)value;
                                }
                                break;
                            case "CY":
                                if (gradient is RadialGradientBrush radialGradient2)
                                {
                                    radialGradient2.CY = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as RadialGradientBrush).CY = (double)value;
                                }
                                break;
                            case "FX":
                                if (gradient is RadialGradientBrush gradient3)
                                {
                                    gradient3.FX = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as RadialGradientBrush).FX = (double)value;
                                }
                                break;
                            case "FY":
                                if (gradient is RadialGradientBrush radialGradient3)
                                {
                                    radialGradient3.FY = (double)value;
                                    if (isUndoRedo)
                                        (shapeStyle.Gradient as RadialGradientBrush).FY = (double)value;
                                }
                                break;
                        }
                    }
                }
            }
            else
                shapeStyle.Gradient = null;
        }

        private void UpdateGradientStops(GradientBrush nodeGradient, GradientBrush gradient, Dictionary<string, object> property)
        {
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            if (property != null && property.Count > 0)
            {
                bool toAddStops = false;
                if (gradient.GradientStops == null || gradient.GradientStops.Count != property.Count)
                {
                    gradient.GradientStops = new DiagramObjectCollection<GradientStop>();
                    toAddStops = true;
                }
                DiagramObjectCollection<GradientStop> stops = gradient.GradientStops;
                foreach (string indexKey in property.Keys)
                {
                    _ = int.TryParse(indexKey, out int stopIndex);
                    Dictionary<string, object> stopValues = property[indexKey] as Dictionary<string, object>;
                    GradientStop stop = gradient.GradientStops.Count > stopIndex ? gradient.GradientStops[stopIndex] : new GradientStop();
                    GradientStop nodeStop = nodeGradient.GradientStops.Count > stopIndex ? nodeGradient.GradientStops[stopIndex] : new GradientStop();
                    foreach (string key in stopValues.Keys)
                    {
                        object value = stopValues[key];
                        if (value is PropertyChangeValues values)
                            value = values.NewValue;
                        switch (key)
                        {
                            case "Color":
                                stop.Color = value.ToString();
                                if (isUndoRedo)
                                    nodeStop.Color = value.ToString();
                                break;
                            case "Offset":
                                double? offset = (double?)value;
                                stop.Offset = offset;
                                if (isUndoRedo)
                                    nodeStop.Offset = offset;
                                break;
                            case "Opacity":
                                double opacity = (double)value;
                                stop.Opacity = opacity;
                                if (isUndoRedo)
                                    nodeStop.Opacity = opacity;
                                break;
                        }
                    }
                    if (toAddStops)
                    {
                        stops.Insert(stopIndex, stop);
                        stop.Parent = gradient;
                        if (isUndoRedo)
                            nodeStop.Parent = gradient;
                    }
                }
            }
        }

        private void UpdateShadow(Shadow nodeShadow, Shadow elementShadow, Dictionary<string, object> shadow)
        {
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            if (shadow != null)
            {
                foreach (string key in shadow.Keys)
                {
                    object value = ((PropertyChangeValues)shadow[key]).NewValue;
                    switch (key)
                    {
                        case "Angle":
                            elementShadow.Angle = (double)value;
                            if (isUndoRedo)
                                nodeShadow.Angle = (double)value;
                            break;
                        case "Color":
                            elementShadow.Color = value.ToString();
                            if (isUndoRedo)
                                nodeShadow.Color = value.ToString();
                            break;
                        case "Distance":
                            elementShadow.Distance = (double)value;
                            if (isUndoRedo)
                                nodeShadow.Distance = (double)value;
                            break;
                        case "Opacity":
                            elementShadow.Opacity = (double)value;
                            if (isUndoRedo)
                                nodeShadow.Opacity = (double)value;
                            break;
                    }
                }
            }
        }

        private void AddCustomPathDataCollection(Dictionary<string, object> property, object parent)
        {
            if (parent is PointPort || parent is Node || parent is Connector || parent is BasicShape || parent is PathShape || parent is DecoratorSettings || parent is NodeFixedUserHandle || parent is ConnectorFixedUserHandle)
            {
                foreach (string key in property.Keys)
                {
                    string data;
                    if (key == "PathData")
                    {
                        data = ((PropertyChangeValues)property[key]).NewValue as string;
                        AddMeasurePathDataCollection(data, CustomMeasurePathDataCollection);
                    }
                    else if (key.Contains("Decorator", StringComparison.InvariantCulture))
                    {
                        AddCustomPathDataCollection(property[key] as Dictionary<string, object>, parent);
                    }
                    else if (key == "Shape")
                    {
                        if (parent is Node)
                        {
                            Dictionary<string, object> shapeProperties = (property[key] as Dictionary<string, object>);
                            if (shapeProperties != null && shapeProperties.ContainsKey("Data"))
                            {
                                data = (shapeProperties["Data"] as PropertyChangeValues)?.NewValue as string;
                                AddMeasurePathDataCollection(data, CustomMeasurePathDataCollection);
                            }
                            else if (shapeProperties != null && shapeProperties.ContainsKey("Points"))
                            {
                                if ((shapeProperties["Points"] as PropertyChangeValues)?.NewValue is DiagramPoint[] points)
                                {
                                    data = PathUtil.GetPolygonPath(points);
                                    AddMeasurePathDataCollection(data, CustomMeasurePathDataCollection);
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void AddCustomPointCollection(Dictionary<string, string> property)
        {
            foreach (KeyValuePair<string, string> entry in property)
            {
                DiagramRect val = DomUtil.MeasurePath(!string.IsNullOrEmpty(entry.Key) ? entry.Key : string.Empty);
                string customdata = DomUtil.UpdatePath(entry.Key, val);
                AddMeasureCustomPathDataCollection(customdata, entry.Key, MeasureCustomPathDataCollection);
            }
        }

        private async Task UpdateCustomBounds()
        {
            bool isUpdate = false;
            Dictionary<string, TextElementUtils> textDataCollection = UpdateNodeTextElementUtils(Parent.PropertyChanges, null, ref isUpdate);
            if (!Parent.DiagramAction.HasFlag(DiagramAction.EditText) && !Parent.DiagramAction.HasFlag(DiagramAction.Layouting)
                && (CustomMeasurePathDataCollection.Count > 0 || (textDataCollection != null && textDataCollection.Count > 0)))
            {
                await DomUtil.MeasureBounds(CustomMeasurePathDataCollection, textDataCollection, null, null);
            }
            AddCustomPointCollection(CustomMeasurePathDataCollection);
            if (MeasureCustomPathDataCollection != null && MeasureCustomPathDataCollection.Count > 0)
            {
                await DomUtil.PathPoints(MeasureCustomPathDataCollection);
            }
            MeasureCustomPathDataCollection = new Dictionary<string, string>();
            CustomMeasurePathDataCollection = new Dictionary<string, string>();
        }

        private void UpdateHyperlink(ICommonElement wrapper, Dictionary<string, object> hyperlinkProperty, Annotation annotation)
        {
            TextElement textElement = wrapper as TextElement;
            HyperlinkSettings hyperlink = textElement?.Hyperlink;
            bool isUndoRedo = Parent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            foreach (string key in hyperlinkProperty.Keys)
            {
                object value = hyperlinkProperty[key];
                if (value is PropertyChangeValues values)
                    value = values.NewValue;
                switch (key)
                {
                    case "Color":
                        (textElement.Style as TextStyle).Color = hyperlink.Color = value.ToString();
                        if (isUndoRedo)
                        {
                            annotation.Style.Color = value.ToString();
                        }
                        break;
                    case "Content":
                        textElement.Content = hyperlink.Content = value.ToString();
                        if (isUndoRedo)
                        {
                            annotation.Content = value.ToString();
                        }
                        break;
                    case "Url":
                        string link = value.ToString();
                        TextStyle labelStyle = annotation.Style;
                        (textElement.Style as TextStyle).Color = !string.IsNullOrEmpty(link) ? hyperlink.Color : labelStyle.Color;
                        (textElement.Style as TextStyle).TextDecoration = !string.IsNullOrEmpty(link) ? hyperlink.TextDecoration : annotation.Style.TextDecoration;
                        textElement.Content = !string.IsNullOrEmpty(link) ? (!string.IsNullOrEmpty(hyperlink.Content) ? hyperlink.Content : link) : annotation.Content;
                        if (isUndoRedo)
                        {
                            annotation.Style.Color = (textElement.Style as TextStyle).Color;
                            annotation.Style.TextDecoration = (textElement.Style as TextStyle).TextDecoration;
                            annotation.Content = textElement.Content;
                            annotation.Hyperlink.Url = link;
                        }
                        hyperlink.Url = link;
                        break;
                    case "TextDecoration":
                        if (Enum.TryParse(value.ToString(), out TextDecoration textDecoration))
                        {
                            (textElement.Style as TextStyle).TextDecoration = hyperlink.TextDecoration = textDecoration;
                            if (isUndoRedo)
                            {
                                annotation.Style.TextDecoration = (textElement.Style as TextStyle).TextDecoration;
                            }
                        }
                        break;
                }
            }
        }

        internal static ICommonElement GetEndNodeWrapper(Node node, Connector connector, bool source)
        {
            if (node != null)
            {
                if (connector != null && node.Shape.Type == Shapes.Bpmn && node.Wrapper != null && node.Wrapper.Children[0] is Canvas)
                {
                    Canvas children = node.Wrapper?.Children[0] as Canvas;
                    if ((((BpmnShape)node.Shape).Shape == BpmnShapes.Activity))
                    {
                        if (source && ((BpmnShape)node.Shape).Activity.SubProcess.Type == BpmnSubProcessTypes.Transaction
                                   && !string.IsNullOrEmpty(connector.SourcePortID))
                        {
                            string portId = connector.SourcePortID;
                            Canvas parent = (children?.Children[0] as Canvas)?.Children[2] as Canvas;
                            if (parent?.Children != null)
                            {
                                foreach (Canvas child in parent.Children)
                                {
                                    if (child.Visible && child.ID == node.ID + '_' + portId)
                                    {
                                        return child.Children[0];
                                    }
                                }
                            }
                        }

                        if (children != null)
                            return (children.Children[0] is DiagramElement
                                ? children.Children[0]
                                : ((Canvas)children.Children[0]).Children[0]);
                    }

                    return children?.Children[0];
                }
                if (node.Wrapper != null && !ContainsMargin(node.Wrapper.Children[0]))
                {
                    if (!(node is NodeGroup))
                    {
                        return node.Wrapper.Children[0];
                    }
                }
                return node.Wrapper;
            }
            return null;
        }

        private static bool ContainsMargin(ICommonElement node)
        {
            return node.Margin != null && (node.Margin.Left != 0 || node.Margin.Top != 0 || node.Margin.Right != 0 || node.Margin.Bottom != 0);
        }

        internal static void RemoveEdges(Node node, string portId, string item, bool isInEdges)
        {
            if (node != null)
            {
                List<string> nodeEdge = isInEdges ? node.InEdges : node.OutEdges;
                DiagramUtil.RemoveItem(nodeEdge, item);
                if (!string.IsNullOrEmpty(portId))
                {
                    for (int i = 0; i < node.Ports.Count; i++)
                    {
                        PointPort port = node.Ports[i];
                        if (port.ID == portId)
                        {
                            List<string> portEdge = isInEdges ? port.InEdges : port.OutEdges;
                            DiagramUtil.RemoveItem(portEdge, item);
                        }
                    }
                }
            }
        }

        internal void UpdateEdges(Connector obj)
        {
            if (obj != null && obj.SourceID != null && !string.IsNullOrEmpty(obj.SourceID))
            {
                Node node = GetNode(obj.SourceID);
                if (node != null && node.OutEdges != null && !node.OutEdges.Contains(obj.ID))
                {
                    node.OutEdges.Add(obj.ID);
                }
                UpdatePortEdges(node, obj, false);
            }
            if (obj != null && obj.TargetID != null && !string.IsNullOrEmpty(obj.TargetID))
            {
                Node node = GetNode(obj.TargetID);
                if (node != null && node.InEdges != null && !node.InEdges.Contains(obj.ID))
                {
                    node.InEdges.Add(obj.ID);
                }
                UpdatePortEdges(node, obj, true);
            }
        }
        internal void UpdatePortVisibility(Node node, PortVisibility portVisibility, bool inverse)
        {
            if (node is Node && ConstraintsUtil.CanMove(node))
            {
                for (int i = 0; i < node.Ports.Count; i++)
                {
                    DiagramElement portElement =
                        DiagramUtil.GetWrapper(node, node.Wrapper, node.Ports[i].ID) as PathElement;
                    if ((portVisibility & PortVisibility.Connect) > 0 ||
                        (portVisibility & PortVisibility.Hover) > 0)
                    {
                        if (ConstraintsUtil.CheckPortRestriction(node.Ports[i], portVisibility) > 0)
                        {
                            if (portElement != null) portElement.Visible = !inverse;
                            Parent.RealAction &= ~RealAction.PreventRefresh;
                            Parent.DiagramStateHasChanged();
                            Parent.RealAction |= RealAction.PreventRefresh;
                        }
                    }
                }
            }
        }

        internal static void UpdatePortEdges(Node node, Connector obj, bool isInEdges)
        {
            if (node != null)
            {
                string portId = (isInEdges) ? obj.TargetPortID : obj.SourcePortID;
                for (int i = 0; i < node.Ports.Count; i++)
                {
                    PointPort port = node.Ports[i];
                    if (port.ID == portId)
                    {
                        List<string> portEdges = isInEdges ? port.InEdges : port.OutEdges;
                        if (!portEdges.Contains(obj.ID))
                        {
                            portEdges.Add(obj.ID);
                        }
                    }
                }
            }
        }

        private bool DecoratorPropertyChange(PathElement element, Dictionary<string, object> decoratorPpty, DecoratorSettings decorator, bool toGetPoints, bool isUndoRedo = false)
        {
            foreach (string key in decoratorPpty.Keys)
            {
                object value;
                if (key == "Style" || key == "Pivot")
                    value = decoratorPpty[key];
                else
                    value = ((PropertyChangeValues)decoratorPpty[key]).NewValue;
                switch (key)
                {
                    case "Width":
                        element.Width = (double)value;
                        if (isUndoRedo)
                            decorator.Width = (double)value;
                        break;
                    case "Height":
                        element.Height = (double)value;
                        if (isUndoRedo)
                            decorator.Height = (double)value;
                        break;
                    case "Pivot":
                        UpdatePointProperties(element.Pivot, decoratorPpty[key] as Dictionary<string, object>);
                        break;
                    case "Style":
                        UpdateStyle(decorator.Style, element, value as Dictionary<string, object>);
                        break;
                    case "Shape":
                        if (Enum.TryParse(value.ToString(), out DecoratorShape shape))
                        {
                            element.Data = shape == DecoratorShape.Custom ? decorator.PathData : Dictionary.GetShapeData(shape.ToString());
                        }
                        toGetPoints = true;
                        decorator.Shape = shape;
                        break;
                    case "PathData":
                        string path = value.ToString();
                        element.Data = path;
                        toGetPoints = true;
                        break;
                }
            }
            return toGetPoints;
        }

        private static void UpdatePointProperties(DiagramPoint point, Dictionary<string, object> pointProperties)
        {
            if (pointProperties != null)
            {
                foreach (string pointPty in pointProperties.Keys)
                {
                    double value = (double)((PropertyChangeValues)pointProperties[pointPty]).NewValue;
                    if (pointPty == "X")
                    {
                        point.X = value;
                    }
                    else
                    {
                        point.Y = value;
                    }
                }
            }
        }

        internal void UpdateConnectorEdges(Node actualObject)
        {
            if (actualObject.InEdges.Count > 0)
            {
                for (int j = 0; j < actualObject.InEdges.Count; j++)
                {
                    UpdateConnectorProperties(GetConnector(actualObject.InEdges[j]));
                }
            }
            if (actualObject.OutEdges.Count > 0)
            {
                for (int k = 0; k < actualObject.OutEdges.Count; k++)
                {
                    UpdateConnectorProperties(GetConnector(actualObject.OutEdges[k]));
                }
            }
        }

        internal void UpdateBpmnConnectorProperties(Connector connector)
        {
            string index = Parent.Connectors.IndexOf(connector).ToString(CultureInfo.InvariantCulture);
            Dictionary<string, object> connectorKeyValuePairs = new Dictionary<string, object>
            {
                {
                    index, new Dictionary<string, object>
                    {
                        {TYPE, new PropertyChangeValues() {NewValue = connector.Type, OldValue = connector.Type}},
                        {
                            SOURCEDECORATOR, new Dictionary<string, object>()
                            {
                                {
                                    SHAPE,
                                    new PropertyChangeValues()
                                    {
                                        NewValue = connector.SourceDecorator.Shape,
                                        OldValue = connector.SourceDecorator.Shape
                                    }
                                },
                                {
                                    WIDTH,
                                    new PropertyChangeValues()
                                    {
                                        NewValue = connector.SourceDecorator.Width,
                                        OldValue = connector.SourceDecorator.Width
                                    }
                                },
                                {
                                    HEIGHT,
                                    new PropertyChangeValues()
                                    {
                                        NewValue = connector.SourceDecorator.Height,
                                        OldValue = connector.SourceDecorator.Height
                                    }
                                },

                                {
                                    STYLE, new Dictionary<string, object>()
                                    {
                                        {
                                            FILL,
                                            new PropertyChangeValues()
                                            {
                                                NewValue = connector.SourceDecorator.Style.Fill,
                                                OldValue = connector.SourceDecorator.Style.Fill
                                            }
                                        },
                                        {
                                            STROKEDASHARRAY,
                                            new PropertyChangeValues()
                                            {
                                                NewValue = connector.SourceDecorator.Style.StrokeDashArray,
                                                OldValue = connector.SourceDecorator.Style.StrokeDashArray
                                            }
                                        },
                                    }
                                }
                            }
                        },
                        {
                            TARGETDECORATOR, new Dictionary<string, object>()
                            {
                                {
                                    SHAPE,
                                    new PropertyChangeValues()
                                    {
                                        NewValue = connector.TargetDecorator.Shape,
                                        OldValue = connector.TargetDecorator.Shape
                                    }
                                },
                                {
                                    STYLE, new Dictionary<string, object>()
                                    {
                                        {
                                            FILL,
                                            new PropertyChangeValues()
                                            {
                                                NewValue = connector.TargetDecorator.Style.Fill,
                                                OldValue = connector.TargetDecorator.Style.Fill
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };
            this.ConnectorPropertyChange(connectorKeyValuePairs);
        }

        internal void UpdateConnectorProperties(Connector connector)
        {
            string index = Parent.Connectors.IndexOf(connector).ToString(CultureInfo.InvariantCulture);
            if (index == "-1" && Parent.NameTable.ContainsKey(connector.ID))
            {
                index = connector.ID;
            }
            Dictionary<string, object> connectorKeyValuePairs = new Dictionary<string, object>
            {
                {index, new Dictionary<string, object>
                    {
                        { "SourceID", new PropertyChangeValues(){NewValue = connector.SourceID, OldValue = connector.SourceID} },
                        { "TargetID", new PropertyChangeValues(){NewValue = connector.TargetID, OldValue = connector.TargetID} },
                        { "SourcePortID", new PropertyChangeValues(){NewValue = connector.SourcePortID, OldValue = connector.SourcePortID} },
                        { "TargetPortID", new PropertyChangeValues(){NewValue = connector.TargetPortID, OldValue = connector.TargetPortID} },
                        { "SourcePoint", new Dictionary<string, object>()
                            {
                                {"X", new PropertyChangeValues(){NewValue = connector.SourcePoint.X, OldValue = connector.SourcePoint.X } },
                                {"Y", new PropertyChangeValues(){NewValue = connector.SourcePoint.Y, OldValue = connector.SourcePoint.Y } },
                            }
                        },
                        { "TargetPoint", new Dictionary<string, object>()
                                {
                                    {"X", new PropertyChangeValues(){NewValue = connector.TargetPoint.X, OldValue = connector.TargetPoint.X } },
                                    {"Y", new PropertyChangeValues(){NewValue = connector.TargetPoint.Y, OldValue = connector.TargetPoint.Y } },
                                }
                        }
                    }
                }
            };
            ConnectorPropertyChange(connectorKeyValuePairs);
        }
        internal void UpdateNodeProperties(Node newVal, Node oldVal)
        {
            string index = Parent.Nodes.IndexOf(newVal).ToString(CultureInfo.InvariantCulture);
            if (index == "-1" && Parent.NameTable.ContainsKey(newVal.ID))
            {
                index = newVal.ID;
            }
            Dictionary<string, object> nodeKeyValuePairs = new Dictionary<string, object>
            {
                {
                    index, new Dictionary<string, object>
                    {
                        { "Width", new PropertyChangeValues(){NewValue = newVal.Width, OldValue = oldVal.Width} },
                        { "Height", new PropertyChangeValues(){NewValue = newVal.Height, OldValue = oldVal.Height} },
                        { "OffsetX", new PropertyChangeValues(){NewValue = newVal.OffsetX, OldValue = oldVal.OffsetX} },
                        { "OffsetY", new PropertyChangeValues(){NewValue = newVal.OffsetY, OldValue = oldVal.OffsetY} },
                    }
                }
            };
            NodeProperty(nodeKeyValuePairs, null, false);
        }

        internal async void SetOffset(double offsetX, double offsetY)
        {
            Parent.DiagramCanvasScrollBounds.X = offsetX;
            Parent.DiagramCanvasScrollBounds.Y = offsetY;

            DiagramSize patternSize = GetPatternSize();

            RectAttributes rectAttributes = null; LineAttributes lineAttributes = null;
            PathAttributes pathAttributes = null; ObservableCollection<CircleAttributes> circleAttributes = null;
            ObservableCollection<BaseAttributes> attributes = GetSelectorAttributes();
            if (attributes != null && attributes.Count > 0)
            {
                for (int i = 0; i < attributes.Count; i++)
                {
                    if(attributes[i] is LineAttributes)
                    {
                        lineAttributes = attributes[i] as LineAttributes;
                    }
                    else if(attributes[i] is PathAttributes)
                    {
                        pathAttributes = attributes[i] as PathAttributes;
                    }
                    else if(attributes[i] is RectAttributes)
                    {
                        rectAttributes = attributes[i] as RectAttributes;
                    }
                    else if(attributes[i] is CircleAttributes)
                    {
                        if(circleAttributes == null)
                        {
                            circleAttributes = new ObservableCollection<CircleAttributes>();
                        }
                        circleAttributes.Add(attributes[i] as CircleAttributes);
                    }
                }
            }

            DiagramRect bounds = await DomUtil.UpdateInnerLayerSize(Parent.InnerLayerList, Parent.InnerLayerWidth, Parent.InnerLayerHeight, new DiagramPoint(offsetX, offsetY), Parent.Scroller.Transform, patternSize, Parent.SnapSettings.PathData, Parent.SnapSettings.DotsData, lineAttributes, pathAttributes, rectAttributes, circleAttributes);
            if (bounds != null)
            {
                Parent.DiagramCanvasScrollBounds = bounds;
            }
        }

        internal async void SetSize(double width, double height, bool isUpdateLayerSize)
        {
            Parent.InnerLayerWidth = Math.Round(width) + "px";
            Parent.InnerLayerHeight = Math.Round(height) + "px";
            if (isUpdateLayerSize)
            {
                DiagramSize patternSize = GetPatternSize();
                RectAttributes rectAttributes = null; LineAttributes lineAttributes = null;
                PathAttributes pathAttributes = null; ObservableCollection<CircleAttributes> circleAttributes = null;
                ObservableCollection<BaseAttributes> attributes = GetSelectorAttributes();
                if (attributes != null && attributes.Count > 0)
                {
                    for (int i = 0; i < attributes.Count; i++)
                    {
                        if (attributes[i] is LineAttributes)
                        {
                            lineAttributes = attributes[i] as LineAttributes;
                        }
                        else if (attributes[i] is PathAttributes)
                        {
                            pathAttributes = attributes[i] as PathAttributes;
                        }
                        else if (attributes[i] is RectAttributes)
                        {
                            rectAttributes = attributes[i] as RectAttributes;
                        }
                        else if (attributes[i] is CircleAttributes)
                        {
                            if (circleAttributes == null)
                            {
                                circleAttributes = new ObservableCollection<CircleAttributes>();
                            }
                            circleAttributes.Add(attributes[i] as CircleAttributes);
                        }
                    }
                }
                DiagramRect bounds = await DomUtil.UpdateInnerLayerSize(Parent.InnerLayerList, Parent.InnerLayerWidth, Parent.InnerLayerHeight, null, Parent.Scroller.Transform, patternSize, Parent.SnapSettings.PathData, Parent.SnapSettings.DotsData, lineAttributes, pathAttributes, rectAttributes, circleAttributes);
                if (bounds != null)
                {
                    Parent.DiagramCanvasScrollBounds = bounds;
                }
                if (!Parent.FirstRender)
                {
                    Parent.RealAction |= RealAction.PreventRefresh;
                    Parent.DiagramStateHasChanged();
                    Parent.RealAction &= ~RealAction.PreventRefresh;
                }
            }
            Parent.DiagramCanvasScrollBounds.Width = Math.Round(width);
            Parent.DiagramCanvasScrollBounds.Height = Math.Round(height);
        }

        internal void UpdateScrollOffset()
        {
            Parent.Scroller.SetScrollOffset(Parent.DiagramCanvasScrollBounds.X, Parent.DiagramCanvasScrollBounds.Y);
        }

        internal Actions FindActionToBeDone(IDiagramObject obj, ICommonElement wrapper, IDiagramObject target = null)
        {
            return Parent.EventHandler.FindActionToBeDone(obj, wrapper, target);
        }
        internal string FindHandle()
        {
            return Parent.EventHandler.FindHandleEvent();
        }

        internal InteractionControllerBase GetTool(Actions action, string handle = null)
        {
            InteractionControllerBase tool;
            if (Parent.GetCustomTool != null && ActionsUtil.HasSelection(Parent) && handle != null)
            {
                tool = Parent.GetCustomTool(action, handle);
                if (tool != null)
                {
                    return tool;
                }
            }
            tool = Parent.EventHandler.GetTool(action, null);
            return tool;
        }

        internal string GetCursor(Actions action, bool active, string handleTool)
        {
            if (Parent.GetCustomCursor != null)
            {
                string cursor = Parent.GetCustomCursor(action, active, handleTool);
                if (cursor != null)
                {
                    return cursor;
                }
            }
            return Parent.EventHandler.GetCursor(action);
        }

        internal void SetCursor(string cursor)
        {
            bool refresh = false;
            if (!Parent.RealAction.HasFlag(RealAction.PreventRefresh))
            {
                Parent.RealAction |= RealAction.PreventRefresh;
                refresh = true;
            }
            if (Parent.EventHandler.Action == Actions.Rotate && cursor == "crosshair")
            {
                Parent.RotateCssClass = "e-diagram-rotate";
            }
            else
            {
                Parent.RotateCssClass = string.Empty;
            }
            if (Parent.DiagramCursor != cursor)
            {
                Parent.DiagramCursor = cursor;
                if(Parent.EventHandler.Action != Actions.PinchZoom)
                Parent.DiagramStateHasChanged();
            }
            if (refresh)
            {
                Parent.RealAction &= ~RealAction.PreventRefresh;
            }
        }

        internal ObservableCollection<IDiagramObject> FindObjectsUnderMouse(DiagramPoint point, IDiagramObject source = null)
        {
            return this.Parent.EventHandler.FindObjectsUnderMouse(point, source);
        }
        internal IDiagramObject FindObjectUnderMouse(ObservableCollection<IDiagramObject> objects, Actions action)
        {
            return this.Parent.EventHandler.FindObjectUnderMouse(objects, action);
        }
        internal ICommonElement FindElementUnderMouse(IDiagramObject obj, DiagramPoint position, double? padding = null)
        {
            return this.Parent.EventHandler.FindElementUnderMouse(obj, position, padding);
        }

        internal IDiagramObject FindTargetObjectUnderMouse(
        ObservableCollection<IDiagramObject> objects, Actions action, DiagramPoint position, IDiagramObject source = null)
        {
            return this.Parent.EventHandler.FindTargetUnderMouse(objects, action, position, source);
        }

        internal ObservableCollection<IDiagramObject> GetNodesConnectors(ObservableCollection<IDiagramObject> selectedItems)
        {
            for (int i = 0; i < Parent.NodeCollection.Count; i++)
            {
                Node node = Parent.NodeCollection[i];
                selectedItems.Add(node);
            }
            for (int i = 0; i < Parent.ConnectorCollection.Count; i++)
            {
                Connector conn = Parent.ConnectorCollection[i];
                selectedItems.Add(conn);
            }
            return selectedItems;
        }

        internal void UpdateBridging()
        {
            for (int i = 0; i < Parent.Connectors.Count; i++)
            {
                Connector connector = Parent.Connectors[i];
                ConnectorBridging.UpdateBridging(connector, Parent);
                DiagramContainer canvas = Parent.Connectors[i].Wrapper;
                if (canvas != null && ConstraintsUtil.CanBridge(connector, Parent))
                {
                    if (canvas.Children[0] is PathElement pathSegment)
                    {
                        string data = pathSegment.Data;
                        connector.GetSegmentElement(connector, pathSegment);
                        if (pathSegment.Data != data)
                        {
                            canvas.Measure(new DiagramSize() { Width = connector.Wrapper.Width, Height = connector.Wrapper.Height });
                            canvas.Arrange(canvas.DesiredSize);
                        }
                    }
                }
            }
        }

        internal void HistoryChangeTrigger(HistoryEntryBase entry, HistoryChangedAction action)
        {
            HistoryChangedEventArgs args = new HistoryChangedEventArgs()
            {
                ActionTrigger = action, Entry = entry as HistoryEntryBase, Type = entry.Type
            };

            List<NodeBase> diagramObjectCollection = new List<NodeBase>();
            if (entry.Category == EntryCategory.InternalEntry)
            {
                if (entry.RedoObject != null)
                {
                    if (entry.RedoObject is DiagramSelectionSettings selectorRedoObj)
                    {
                        if (selectorRedoObj.Nodes.Count > 0)
                        {
                            foreach (Node node in selectorRedoObj.Nodes)
                            {
                                diagramObjectCollection.Add(node);
                            }
                        }
                        if (selectorRedoObj.Connectors != null && selectorRedoObj.Connectors.Count > 0)
                        {
                            foreach (Connector connector in selectorRedoObj.Connectors)
                            {
                                diagramObjectCollection.Add(connector);
                            }
                        }
                    }
                    else if (entry.RedoObject is NodeBase node)
                    {
                        diagramObjectCollection.Add(node);
                    }
                }
                if(entry.Type == HistoryEntryType.CollectionChanged)
                {
                    args.CollectionChangedAction = entry.ChangeType;
                }
                args.Source = diagramObjectCollection;
                if (diagramObjectCollection.Count > 0)
                {
                    this.Parent.CommandHandler.InvokeDiagramEvents(DiagramEvent.HistoryChanged, args);
                }
            }
        }

        internal void UpdateGridlines()
        {
            DiagramSize patternSize = GetPatternSize();
            DomUtil.UpdateGridLines(Parent.DiagramPattern, patternSize, Parent.SnapSettings.PathData, Parent.SnapSettings.DotsData);
        }

        internal DiagramSize GetPatternSize()
        {
            double scaleFactor = SnapSettings.ScaleSnapInterval(Parent.SnapSettings, Parent.Scroller.CurrentZoom);
            double patternWidth = (SnapSettings.GetPatternDimension(Parent.SnapSettings, Parent.SnapSettings.GridType == GridType.Lines ? true : false) * scaleFactor);
            double patternHeight = (SnapSettings.GetPatternDimension(Parent.SnapSettings, Parent.SnapSettings.GridType == GridType.Lines ? false : true) * scaleFactor);

            Parent.SnapSettings.PathData = new List<string>() { };
            Parent.SnapSettings.DotsData = new List<DiagramPoint>() { };
            ObservableCollection<BaseAttributes> Hattributes = SnapSettings.GetAttributes(true, Parent.SnapSettings, scaleFactor);
            Parent.SnapSettings.GetPathData(Hattributes);

            ObservableCollection<BaseAttributes> Vattributes = SnapSettings.GetAttributes(false, Parent.SnapSettings, scaleFactor);
            Parent.SnapSettings.GetPathData(Vattributes);

            return new DiagramSize() { Width = patternWidth, Height = patternHeight };
        }

        internal ObservableCollection<BaseAttributes> GetSelectorAttributes()
        {
            ObservableCollection<BaseAttributes> attributes = null;
            if (Parent.DiagramAction.HasFlag(DiagramAction.Render) && ActionsUtil.HasSelection(Parent))
            {
                attributes = DiagramRenderer.GetSelectorAttributes(new SelectionFragmentParameter { Selector = Parent.SelectionSettings, Transform = Parent.Scroller.Transform });
            }
            return attributes;
        }

        internal void Dispose()
        {
            if (Parent != null)
            {
                Parent = null;
            }
            if (ConnectorBridging != null)
            {
                ConnectorBridging = null;
            }

            if (TextAnnotationConnectors != null)
            {
                for (int i = 0; i < TextAnnotationConnectors.Count; i++)
                {
                    TextAnnotationConnectors[i].Dispose();
                    TextAnnotationConnectors[i] = null;
                }
                TextAnnotationConnectors.Clear();
                TextAnnotationConnectors = null;
            }
            if (MeasurePathDataCollection != null)
            {
                MeasurePathDataCollection.Clear();
                MeasurePathDataCollection = null;
            }
            if (MeasureCustomPathDataCollection != null)
            {
                MeasureCustomPathDataCollection.Clear();
                MeasureCustomPathDataCollection = null;
            }
            if (MeasureTextDataCollection != null)
            {
                MeasureTextDataCollection.Clear();
                MeasureTextDataCollection = null;
            }
            if (MeasureImageDataCollection != null)
            {
                MeasureImageDataCollection.Clear();
                MeasureImageDataCollection = null;
            }
            if (MeasureNativeDataCollection != null)
            {
                MeasureNativeDataCollection.Clear();
                MeasureNativeDataCollection = null;
            }
            if (PathTable != null)
            {
                PathTable.Clear();
                PathTable = null;
            }
            if (CustomMeasurePathDataCollection != null)
            {
                CustomMeasurePathDataCollection.Clear();
                CustomMeasurePathDataCollection = null;
            }
            if (GradientCollection != null)
            {
                GradientCollection.Clear();
                GradientCollection = null;
            }
        }
    }
}
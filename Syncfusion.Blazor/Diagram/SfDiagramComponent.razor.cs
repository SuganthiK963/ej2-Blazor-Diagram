using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Diagram.SymbolPalette;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Popups;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Renders the diagram. It contains all the properties of the diagram to be rendered.
    /// </summary>
    public partial class SfDiagramComponent : SfBaseComponent, IDiagramObject
    {
        private DiagramConstraints constraints = DiagramConstraints.Default;
        private InteractionController interactionTool = InteractionController.Default;
        private string width = "100%";
        private string height = "100%";
        private Direction bridgeDirection;
        [Inject]
        private ISyncfusionStringLocalizer localizer { get; set; }
        [JsonIgnore]
        internal LocaleProvider Localizer { get; set; }
        internal static bool IsProtectedOnChange = true;
        internal bool GroupAction;
        internal bool IsScriptRendered;
        internal int UndoRedoCount;
        internal const string CONTENT = "_content";
        internal const string PIXELVALUE = "px";
        internal const string PATTERN = "_pattern";
        internal const string GRIDLINE = "gridline";
        internal string RotateCssClass = string.Empty;
        internal string DiagramCursor = "default";
        internal DiagramLayerContent DiagramContent;
        internal SfSymbolPaletteComponent PaletteInstance;
        internal BpmnDiagrams BpmnDiagrams = new BpmnDiagrams();
        internal UndoRedo UndoRedo = new UndoRedo();
        DotNetObjectReference<DiagramEventHandler> selfReference;
        internal DiagramRect DiagramCanvasScrollBounds { get; set; }
        internal ScrollActions ScrollActions { get; set; } = ScrollActions.None;
        internal DiagramObjectCollection<Node> NodeCollection = new DiagramObjectCollection<Node>() { };
        internal DiagramObjectCollection<Connector> ConnectorCollection = new DiagramObjectCollection<Connector>() { };
        internal bool IsBeginUpdate;
        internal bool FirstRender;
        internal string InnerLayerWidth;
        internal string InnerLayerHeight;
        internal DiagramAction DiagramAction { get; set; }
        internal Dictionary<string, KeyboardCommand> Commands = new Dictionary<string, KeyboardCommand>();
        internal RealAction RealAction { get; set; }
        internal MindMap MindMap { get; set; }
        internal Snapping Snapping { get; set; }
        [JsonIgnore]
        internal DiagramDataBinding DataBindingModule { get; set; }
        [JsonIgnore]
        internal HierarchicalTree HierarchicalTree { get; set; }
        internal string ID { get; set; } = BaseUtil.RandomId();
        internal DiagramObjectCollection<ICommonElement> BasicElements { get; set; }
        internal IDiagramObject DrawingObjectTool { get; set; }
        internal Dictionary<string, IDiagramObject> NameTable = new Dictionary<string, IDiagramObject>();
        internal string[] InnerLayerList;
        internal SpatialSearch SpatialSearch;
        internal Scroller Scroller { get; set; }
        internal DiagramEventHandler EventHandler { get; set; }
        [JsonIgnore]
        internal CommandHandler CommandHandler { get; set; }
        internal string DiagramLayer;
        internal string DiagramAdornerLayer;
        internal string DiagramAdornerSvg;
        internal string SelectorElement;
        internal string DiagramAdorner;
        internal string DiagramPorts;
        internal string DiagramHtml;
        internal string DiagramUserHandle;
        internal string DiagramHtmlDiv;
        internal string DiagramUserHandleDiv;
        internal string DiagramBackground;
        internal string GridLineSvg;
        internal string SvgLayer;
        internal string DiagramContentId;
        internal string NodesGParent;
        internal string DiagramGridLine;
        internal string DiagramPattern;

        internal SfTooltip DiagramTooltip;
        internal string TooltipID = "DiagramTooltip";
        internal string TooltipContent = string.Empty;
        internal string TooltipTarget = "#helper";
        internal AnimationModel TooltipAnimation = new AnimationModel() { Open = new TooltipAnimationSettings() { } };
        internal Position TooltipPosition = Position.BottomCenter;

        /// <summary>
        /// Notifies when a change is reverted or restored. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public DiagramHistoryManager HistoryManager { get; set; } = new DiagramHistoryManager();
        /// <summary>
        /// Defines the direction of the bridge that is inserted when the connector segments are intersected.
        /// </summary>
        [Parameter]
        [JsonPropertyName("bridgeDirection")]
        public Direction BridgeDirection { get; set; } = Direction.Top;
        /// <summary>
        /// Specifies the callback to trigger when the bridge direction changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<Direction> BridgeDirectionChanged { get; set; }

        /// <summary>
        /// Configures the data source that is to be bound with the diagram. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("dataSourceSettings")]
        public DataSourceSettings DataSourceSettings { get; set; }

        /// <summary>
        /// Defines the width of the diagram.
        /// </summary>
        [JsonPropertyName("width")]
        [Parameter]
        public string Width { get; set; } = "100%";
        /// <summary>
        /// Specifies the callback to trigger when the width changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<string> WidthChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the height changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<string> HeightChanged { get; set; }

        /// <summary>
        /// Sets the child content of the diagram component.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Enables/disables certain behaviors of the diagram.
        /// </summary>
        [Parameter]
        [JsonPropertyName("constraints")]
        public DiagramConstraints Constraints
        {
            get => constraints;
            set
            {
                if (constraints != value)
                {
                    DiagramConstraints oldValue = constraints;
                    constraints = value;
                    if (this.DiagramContent != null && ((oldValue.HasFlag(DiagramConstraints.Bridging) && !value.HasFlag(DiagramConstraints.Bridging)) ||
                        (!oldValue.HasFlag(DiagramConstraints.Bridging) && value.HasFlag(DiagramConstraints.Bridging))))
                        this.DiagramContent.UpdateBridging();
                }
            }
        }
        /// <summary>
        /// Specifies the callback to trigger when the constraint changes.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DiagramConstraints> ConstraintsChanged { get; set; }
        /// <summary>
        /// Defines the precedence of the interactive tools. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("InteractionController")]
        public InteractionController InteractionController
        {
            get => interactionTool;
            set
            {
                if (interactionTool != value)
                {
                    if (IsRendered && value != interactionTool && (value == InteractionController.ZoomPan || interactionTool == InteractionController.ZoomPan))
                    {
                        DomUtil.UpdateZoomPanTool(value == InteractionController.ZoomPan);
                    }
                    interactionTool = value;
                }
            }
        }
        /// <summary>
        /// Specifies the callback to trigger when the tool changes.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<InteractionController> InteractionControllerChanged { get; set; }
        internal void UpdateMindMap(MindMap mindMap)
        {
            MindMap = mindMap ?? MindMap.Initialize();
        }
        /// <summary>
        /// Defines the gridlines and specifies how and when objects must be snapped. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("snapSettings")]
        public SnapSettings SnapSettings { get; set; }

        /// <summary>
        /// Represents the template of the diagram element. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public DiagramTemplates DiagramTemplates { get; set; }
        /// <summary>
        /// Page settings enable you to customize the appearance, width, and height of the Diagram page. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("pageSettings")]
        public PageSettings PageSettings { get; set; }

        /// <summary>
        /// Defines the current zoom value, zoom factor, scroll status, and view port size of the diagram. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("scrollSettings")]
        public ScrollSettings ScrollSettings { get; set; }
        /// <summary>
        /// Defines the height of the diagram. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("height")]
        public string Height { get; set; } = "100%";

        /// <summary>
        /// Layout is used to auto-arrange the nodes in the Diagram area. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("layout")]
        public Layout Layout { get; set; }

        /// <summary>
        /// Defines a set of custom commands and binds them with a set of desired key gestures. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("commandManager")]
        public CommandManager CommandManager { get; set; }
        /// <summary>
        /// Defines the collection of selected items, the size and position of the selector.  
        /// </summary>
        [Parameter]
        [JsonPropertyName("selectedItems")]
        public DiagramSelectionSettings SelectionSettings { get; set; } = new DiagramSelectionSettings();

        /// <summary>
        /// Defines the object to be drawn using a drawing tool. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("drawingObject")]
        public IDiagramObject DrawingObject { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the drawing object changes.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<IDiagramObject> DrawingObjectChanged { get; set; }
        /// <summary>
        /// Defines a collection of node objects, used to visualize the geometrical information. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("nodes")]
        public DiagramObjectCollection<Node> Nodes
        {
            get
            {
                if (NodeCollection != null && NodeCollection.Parent == null)
                    NodeCollection.Parent = this;
                return NodeCollection;
            }
            set
            {
                if (value != null && NodeCollection != value)
                {
                    if (IsRendered)
                        this.StartGroupAction();
                    if (NodeCollection != null)
                        DeleteDiagramElement(NodeCollection, new ObservableCollection<Connector>());
                    NodeCollection = value;
                    NodeCollection.Parent = this;
                    if (DiagramContent != null && IsRendered)
                    {
                        IsProtectedOnChange = false;
                        UpdateNameTable(NodeCollection);
                        _ = NodeCollection.InitializeNodesAndConnectors(NodeCollection);
                        IsProtectedOnChange = true;
                    }
                    if (IsRendered)
                        this.EndGroupAction();
                }
                else
                    NodeCollection = value;
            }
        }
        /// <summary>
        /// Defines a collection of connector objects, used to create links between two points, nodes or ports to represent the relationships between them. 
        /// </summary>
        [Parameter]
        [JsonPropertyName("connectors")]
        public DiagramObjectCollection<Connector> Connectors
        {
            get
            {
                if (ConnectorCollection != null && ConnectorCollection.Parent == null)
                    ConnectorCollection.Parent = this;
                return ConnectorCollection;
            }
            set
            {
                if (value != null && ConnectorCollection != value)
                {
                    if (IsRendered)
                        this.StartGroupAction();
                    if (ConnectorCollection != null)
                        DeleteDiagramElement(new ObservableCollection<Node>(), ConnectorCollection);
                    ConnectorCollection = value;
                    ConnectorCollection.Parent = this;
                    if (DiagramContent != null && IsRendered)
                    {
                        IsProtectedOnChange = false;
                        UpdateNameTable(ConnectorCollection);
                        _ = ConnectorCollection.InitializeNodesAndConnectors(ConnectorCollection);
                        IsProtectedOnChange = true;
                    }
                    if (IsRendered)
                        this.EndGroupAction();
                }
                else
                    ConnectorCollection = value;
            }
        }
        /// <summary>
        /// Specifies the callback to trigger when the node changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<DiagramObjectCollection<Node>> NodesChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the connector changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<DiagramObjectCollection<Connector>> ConnectorsChanged { get; set; }
        /// <summary>
        /// Triggers when the node or connector property changes. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<PropertyChangedEventArgs> PropertyChanged { get; set; }
        /// <summary>
        /// Triggers before the selection is change in the diagram.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<SelectionChangingEventArgs> SelectionChanging { get; set; }
        /// <summary>
        /// Triggers when the selection is changed in the diagram. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<SelectionChangedEventArgs> SelectionChanged { get; set; }
        /// <summary>
        /// Triggers when the node’s/connector's label is changed in the diagram.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<TextChangeEventArgs> TextChanged { get; set; }
        /// <summary>
        /// Triggers when a change is reverted or restored(undo/redo). 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<HistoryChangedEventArgs> HistoryChanged { get; set; }
        /// <summary>
        /// Triggers when an element drags over another diagram element. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DraggingEventArgs> Dragging { get; set; }
        /// <summary>
        /// Triggers when a symbol is dragged into the diagram from the symbol palette 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DragStartEventArgs> DragStart { get; set; }
        /// <summary>
        /// Triggers when a symbol is dragged outside of the diagram. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DragLeaveEventArgs> DragLeave { get; set; }
        /// <summary>
        /// Triggers when a symbol is dragged and dropped from the symbol palette to the drawing area. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DropEventArgs> DragDrop { get; set; }
        /// <summary>
        /// Triggers when a user presses a key. 
        /// </summary>

        [Parameter]
        [JsonIgnore]
        public EventCallback<KeyEventArgs> KeyDown { get; set; }
        /// <summary>
        /// Triggers when a user releases a key. 
        /// </summary>

        [Parameter]
        [JsonIgnore]
        public EventCallback<KeyEventArgs> KeyUp { get; set; }
        /// <summary>
        /// Triggers before the node/connector is add or remove from the diagram.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<CollectionChangingEventArgs> CollectionChanging { get; set; }
        /// <summary>
        /// Triggers when the node/connector is added or removed from the diagram. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<CollectionChangedEventArgs> CollectionChanged { get; set; }
        /// <summary>
        /// Triggers when the connector’s segment collection is updated. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<SegmentCollectionChangeEventArgs> SegmentCollectionChange { get; set; }
        /// <summary>
        /// Triggers when a node, connector or diagram is clicked.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<ClickEventArgs> Click { get; set; }
        /// <summary>
        /// Triggers when the mouse enters a node/connector. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DiagramElementMouseEventArgs> MouseEnter { get; set; }
        /// <summary>
        /// Triggers when the mouse pointer rests on the node/connector. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DiagramElementMouseEventArgs> MouseHover { get; set; }
        /// <summary>
        /// Triggers when the mouse leaves a node/connector. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<DiagramElementMouseEventArgs> MouseLeave { get; set; }
        /// <summary>
        /// Triggers when the scrollbar is updated. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<ScrollChangedEventArgs> ScrollChanged { get; set; }
        /// <summary>
        /// Triggers while dragging the elements in the diagram. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<PositionChangingEventArgs> PositionChanging { get; set; }
        /// <summary>
        /// Triggers when the node's/connector's position is changed.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<PositionChangedEventArgs> PositionChanged { get; set; }
        /// <summary>
        /// Triggers before the connector’s source or target point is connect or disconnect from the source or target.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<ConnectionChangingEventArgs> ConnectionChanging { get; set; }
        /// <summary>
        /// Triggers when the connector’s source or target point is connected or disconnected from the source or target.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<ConnectionChangedEventArgs> ConnectionChanged { get; set; }
        /// <summary>
        /// Triggers while dragging the connector’s source end in the diagram. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<EndPointChangingEventArgs> SourcePointChanging { get; set; }
        /// <summary>
        /// Triggers when the connector’s source point is changed.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<EndPointChangedEventArgs> SourcePointChanged { get; set; }
        /// <summary>
        /// Triggers while dragging the connector’s target end in the diagram.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<EndPointChangingEventArgs> TargetPointChanging { get; set; }
        /// <summary>
        /// Triggers when the connector’s target point is changed.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<EndPointChangedEventArgs> TargetPointChanged { get; set; }
        /// <summary>
        /// Triggers when a Fixed User Handle item is clicked. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<FixedUserHandleClickEventArgs> FixedUserHandleClick { get; set; }
        /// <summary>
        /// Triggers before a node is resize.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<SizeChangingEventArgs> SizeChanging { get; set; }
        /// <summary>
        /// Triggers when a node is resized.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<SizeChangedEventArgs> SizeChanged { get; set; }
        /// <summary>
        /// Triggers before the diagram elements are rotate.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<RotationChangingEventArgs> RotationChanging { get; set; }
        /// <summary>
        /// Triggers when the diagram elements are rotated.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<RotationChangedEventArgs> RotationChanged { get; set; }
        /// <summary>
        /// Triggers when the diagram layout is rendered completely. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<object> DataLoaded { get; set; }
        /// <summary>
        /// Customizes the node template.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public Func<IDiagramObject, ICommonElement> SetNodeTemplate { get; set; }
        /// <summary>
        /// This method allows users to customize the tool. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public Func<Actions, string, InteractionControllerBase> GetCustomTool { get; set; }
        /// <summary>
        /// This method allows users to create their own cursor. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public Func<Actions, bool, string, string> GetCustomCursor { get; set; }
        /// <summary>
        /// Helps to assign the default properties of nodes. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<IDiagramObject> NodeCreating { get; set; }
        /// <summary>
        /// Helps to assign the default properties of the connector. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<IDiagramObject> ConnectorCreating { get; set; }

        /// <summary>
        /// It copies the selected nodes/connectors to the clipboard. 
        /// </summary>
        public void Copy()
        {
            this.CommandHandler.Copy();
        }
        /// <summary>
        /// It removes the selected nodes/connectors and moves them to the clipboard. 
        /// </summary>
        public void Cut()
        {
            this.CommandHandler.Cut();
        }
        /// <summary>
        /// Adds the given objects/ objects in the diagram clipboard to the diagram control. 
        /// </summary>
        public void Paste()
        {
            _ = this.CommandHandler.Paste();
        }
        /// <summary>
        /// Represents a method used to get the bounds of the page.
        /// </summary>
        public DiagramRect GetPageBounds(double? originX = null, double? originY = null)
        {
            return SpatialSearch.GetPageBounds(originX, originY);
        }
        internal void UpdateQuad(IDiagramObject node)
        {
            this.SpatialSearch.ObjectTable.Add(((NodeBase)node).ID, node);
            bool modified = this.SpatialSearch.UpdateQuad(((NodeBase)node).Wrapper);
            if (modified)
            {
                this.UpdatePage();
            }
        }
        internal void UpdatePage()
        {
            if (this.DiagramAction.HasFlag(DiagramAction.Render) && !(this.DiagramAction.HasFlag(DiagramAction.Interactions)))
            {
                this.Scroller.UpdateScrollOffsets();
                this.Scroller.SetSize();
            }
        }
        private static string GetSizeValue(string real)
        {
            string value = real.IndexOf(PIXELVALUE, System.StringComparison.InvariantCulture) > 0 || real.IndexOf('%', System.StringComparison.InvariantCulture) > 0 ? real : real + "px";
            return value;
        }
        /// <summary>
        /// This method locks the diagram to prevent its visual updates until the EndUpdate() method is called. 
        /// </summary>
        public void BeginUpdate()
        {
            IsBeginUpdate = true;
        }
        /// <summary>
        /// This method unlocks the diagram after a call to the BeginUpdate(Boolean) method and causes an immediate visual update.  
        /// </summary>
        /// <returns>Task</returns>
        public async Task EndUpdate()
        {
            IsBeginUpdate = false;
            await DiagramContent.UpdateProperty();
            DiagramStateHasChanged();
        }
        /// <summary>
        /// It is used for adding nodes collection to the diagram. 
        /// </summary>
        /// <param name="items">DiagramObjectCollection</param>
        /// <returns>Task</returns>
        public async Task AddDiagramElements(DiagramObjectCollection<NodeBase> items)
        {
            if (items != null)
            {
                items.Parent = this;
                await items.AddCollection(items, this);
            }
        }
        /// <summary>
        /// It allows the user to refresh the layout at runtime. 
        /// </summary>
        /// <returns>Task</returns>
        public async Task DoLayout()
        {
            IsBeginUpdate = true;
            await DiagramContent.DoLayout();
            IsBeginUpdate = false;
            if (!(FirstRender && DiagramAction.HasFlag(DiagramAction.Layouting)))
                DiagramStateHasChanged();
        }
        /// <summary>
        /// This method returns the object based on the given id. 
        /// </summary>
        /// <param name="name">String</param>
        /// <returns>IDiagramObject</returns>
        public IDiagramObject GetObject(string name)
        {
            Node node = this.DiagramContent.GetNode(name);
            if (node != null)
            {
                return node;
            }
            Connector connector = this.DiagramContent.GetConnector(name);
            if (connector != null)
            {
                return connector;
            }
            return null;
        }
        internal void InitCommands()
        {
            if (this.CommandManager != null)
            {
                DiagramObjectCollection<KeyboardCommand> newCommands = this.CommandManager.Commands;
                Dictionary<string, KeyboardCommand> commands = new Dictionary<string, KeyboardCommand>()
                {
                    {"copy", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.C, Modifiers= ModifierKeys.Control } } },
                    {"paste", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.V, Modifiers= ModifierKeys.Control } } },
                    {"cut", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.X, Modifiers= ModifierKeys.Control } } },
                    {"delete", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.Delete } } },
                    {"selectAll", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.A, Modifiers= ModifierKeys.Control } } },
                    {"undo", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.Z, Modifiers= ModifierKeys.Control } } },
                    {"redo", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.Y, Modifiers= ModifierKeys.Control } } },
                    {"nudgeUp", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.ArrowUp} } },
                    {"nudgeRight", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.ArrowRight} } },
                    {"nudgeDown", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.ArrowDown} } },
                    {"nudgeLeft", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.ArrowLeft} } },
                    {"startEdit", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.F2 } } },
                    {"print", new KeyboardCommand(){Gesture= new KeyGesture(){Key= Keys.P , Modifiers= ModifierKeys.Control} } }
                };
                this.InitCommandManager(newCommands, commands);
            }
        }
        internal void InitCommandManager(DiagramObjectCollection<KeyboardCommand> newCommands, Dictionary<string, KeyboardCommand> commands)
        {
            if (newCommands.Count > 0)
            {
                int i;
                for (i = 0; i < newCommands.Count; i++)
                {
                    KeyboardCommand newCommand = newCommands[i];
                    if (newCommand != null && commands.ContainsKey(newCommand.Name))
                    {
                        if (newCommand.Gesture.Key != Keys.None && newCommand.Gesture.Modifiers != ModifierKeys.None)
                        {
                            commands[newCommand.Name].Gesture = newCommand.Gesture;
                        }
                        if (!string.IsNullOrEmpty(newCommand.Parameter))
                        {
                            commands[newCommand.Name].Parameter = newCommand.Parameter;
                        }
                    }
                    else
                    {
                        OverrideCommands(newCommand, commands);
                        if (newCommand?.Name != null)
                            commands[newCommand.Name] = new KeyboardCommand()
                            {
                                Gesture = newCommand.Gesture,
                                Parameter = newCommand.Parameter
                            };
                    }
                }
            }
            this.Commands = commands;
        }
        private static void OverrideCommands(KeyboardCommand newCommand, Dictionary<string, KeyboardCommand> commands)
        {
            foreach (KeyValuePair<string, KeyboardCommand> key in commands)
            {
                KeyboardCommand command = key.Value;
                if (newCommand.Gesture.Key == command.Gesture.Key && newCommand.Gesture.Modifiers == command.Gesture.Modifiers)
                {
                    commands.Remove(key.Key);
                    break;
                }
            }
        }
        internal void NudgeCommand(Direction direction)
        {
            if (this.SelectionSettings.Nodes.Count > 0 || this.SelectionSettings.Connectors.Count > 0)
            {
                this.Nudge(direction);
            }
        }

        /// <summary>
        /// Repositions the selected object by the specified delta in the given direction.
        /// </summary>
        /// <remarks>The nudge commands move the selected elements towards up, down, left, or right by 1 pixel, by default.</remarks>
        /// <param name="direction">Nudge command moves the selected elements towards the specified <seealso cref="Direction"/>.</param>
        /// <param name="nudgeDelta">The amount in delta by which to reposition the selected objects.</param>
        /// examples for the following:
        /// <example>
        /// <code>
        /// private void Nudge()
        ///  {
        ///    //Repositions the selected objects by 50 towards down direction.
        ///    diagram.Nudge(NudgeDirection.Down, 50);
        ///  }
        /// </code>
        /// </example>
        public void Nudge(Direction direction, int? nudgeDelta = null)
        {
            int tx = 0;
            int ty = 0;
            bool negativeDirection;
            if (direction == Direction.Left || direction == Direction.Right)
            {
                negativeDirection = direction == Direction.Left;
                tx = (negativeDirection ? -1 : 1) * (nudgeDelta ?? 1);
            }
            else
            {
                negativeDirection = direction == Direction.Top;
                ty = (negativeDirection ? -1 : 1) * (nudgeDelta ?? 1);
            }
            DiagramSelectionSettings obj = this.SelectionSettings;
            this.Drag(obj, tx, ty);
            InternalHistoryEntry entry = new InternalHistoryEntry()
            {
                Type = HistoryEntryType.PositionChanged,
                RedoObject = this.SelectionSettings.Clone() as IDiagramObject,
                UndoObject = obj.Clone() as DiagramSelectionSettings,
                Category = EntryCategory.InternalEntry
            };
            this.AddHistoryEntry(entry);
        }
        internal void Delete()
        {
            DeleteDiagramElement(SelectionSettings.Nodes, SelectionSettings.Connectors);
        }

        private void DeleteDiagramElement(ObservableCollection<Node> nodes, ObservableCollection<Connector> connectors)
        {
            bool groupAction = false;
            bool refresh = false;
            if (this.Constraints.HasFlag(DiagramConstraints.UndoRedo) && (nodes.Count > 0 || connectors.Count > 0))
            {
                this.StartGroupAction();
                groupAction = true;
            }
            RealAction |= RealAction.PreventRefresh;
            if (nodes.Count > 0)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    if (ConstraintsUtil.CanDelete(nodes[i]))
                    {
                        this.Nodes.Remove(nodes[i]);
                        refresh = true;
                    }
                }
            }
            if (connectors.Count > 0)
            {
                for (int j = connectors.Count - 1; j >= 0; j--)
                {
                    if (ConstraintsUtil.CanDelete(connectors[j]))
                    {
                        this.Connectors.Remove(connectors[j]);
                        refresh = true;
                    }
                }
            }
            RealAction &= ~RealAction.PreventRefresh;
            if (refresh)
                this.DiagramStateHasChanged();
            if (groupAction)
            {
                this.EndGroupAction();
            }
        }

        internal void UpdateCommandManager(CommandManager commandManager)
        {
            CommandManager = commandManager ?? CommandManager.Initialize();
            CommandManager.Parent = this;
            CommandManager.Commands.Parent = this;
        }

        internal void DragLeaveEvent()
        {
            DragLeaveEventArgs args = new DragLeaveEventArgs() { Element = this.PaletteInstance.SelectedSymbol };
            this.CommandHandler.HelperObject = null;
            this.DiagramStateHasChanged();
            this.CommandHandler.InvokeDiagramEvents(DiagramEvent.DragLeave, args);
            this.RealAction &= ~RealAction.SymbolDrag;
        }
        internal async Task DropGroupNode(Node newNode)
        {
            string[] child = ((NodeGroup)newNode).Children;
            for (int i = 0; i < child.Length; i++)
            {
                Node childNode = (Node)((Node)this.PaletteInstance.SymbolTable[child[i]]).Clone();
                childNode.OffsetX += this.EventHandler.CurrentPosition.X;
                childNode.OffsetY += this.EventHandler.CurrentPosition.Y;
                ((NodeGroup)newNode).Children[i] = childNode.ID;
                childNode.ParentId = newNode.ID;
                childNode.ID = BaseUtil.RandomId();
                ((NodeGroup)newNode).Children[i] = childNode.ID;
                await this.AddDiagramElements(new DiagramObjectCollection<NodeBase>() { childNode });
            }

            await this.AddDiagramElements(new DiagramObjectCollection<NodeBase>() { newNode });
            this.CommandHandler.Select(newNode, false);
        }
        internal async void SymbolDrop(bool isTouch)
        {
            this.RealAction &= ~RealAction.SymbolDrag;
            DropEventArgs args = new DropEventArgs()
            {
                Element = this.PaletteInstance.SelectedSymbol,
                Position = this.EventHandler.CurrentPosition,
                Target = this.PaletteInstance.SelectedSymbol,
                Cancel = false
            };
            if (this.PaletteInstance != null && this.PaletteInstance.SelectedSymbol != null && this.PaletteInstance.AllowDrag && (this.PaletteInstance.PaletteMouseDown || isTouch))
            {
                NodeBase newNode;
                if (isTouch && this.RealAction.HasFlag(RealAction.PreventRefresh))
                {
                    this.RealAction &= ~RealAction.PreventRefresh;
                }
                this.RealAction |= RealAction.MeasureDataJSCall;
                this.PaletteInstance.PreviewSymbol = null;
                if (this.PaletteInstance.SelectedSymbol is Node node)
                {
                    newNode = node is NodeGroup group1 ? (NodeGroup)group1.Clone() : (Node)node.Clone();
                    if ((newNode is NodeGroup) && !args.Cancel)
                    {
                        this.RealAction |= RealAction.SymbolDrag;
                    }
                    if (((Node)newNode).Shape.Type == Shapes.SVG || ((Node)newNode).Shape.Type == Shapes.HTML)
                    {
                        string nativeId = node.ID.Split("_")[0];
                        newNode.ID = nativeId + "_" + BaseUtil.RandomId();
                        ((Node)newNode).NativeSize = node.NativeSize;
                    }
                    if (!args.Cancel)
                    {
                        if (newNode is NodeGroup groupDrop)
                        {
                            await this.DropGroupNode(groupDrop);
                            this.RealAction &= ~RealAction.SymbolDrag;
                        }
                        else
                        {
                            this.Nodes.Add(newNode as Node);
                        }
                    }
                }
                else
                {
                    newNode = (Connector)((Connector)this.PaletteInstance.SelectedSymbol).Clone();
                    if (!args.Cancel)
                    {
                        this.ConnectorCollection.Add((Connector)newNode);
                    }
                }
            }
            this.CommandHandler.InvokeDiagramEvents(DiagramEvent.DragDrop, args);
        }
        internal void DragEnterEvent(IDiagramObject selectedObject)
        {
            DragStartEventArgs args = new DragStartEventArgs()
            {
                Element = selectedObject,
                Cancel = false
            };
            this.CommandHandler.InvokeDiagramEvents(DiagramEvent.DragStart, args);
            this.RealAction |= RealAction.SymbolDrag;
            if (args.Cancel)
            {
                this.PaletteInstance.SelectedSymbol = null;
            }
        }
        /// <summary>
        /// Allows the user to update all the properties. 
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="parent">IDiagramObject</param>
#pragma warning disable CA1725 // Parameter names should match base declaration
        public void OnPropertyChanged(string propertyName, object newVal, object oldVal, IDiagramObject parent)
#pragma warning restore CA1725 // Parameter names should match base declaration
        {
            if (parent != null && IsProtectedOnChange)
            {
                if (!(newVal is DiagramObjectCollection<ShapeAnnotation> || newVal is DiagramObjectCollection<PathAnnotation> || newVal is DiagramObjectCollection<PointPort>))
                {
                    object propertyNewValue = (newVal as IDiagramObject) != null ? ((IDiagramObject)newVal).Clone() : newVal;
                    object propertyOldValue = (oldVal as IDiagramObject) != null ? ((IDiagramObject)oldVal).Clone() : oldVal;
                    if (!(newVal is DiagramObjectCollection<ConnectorSegment>))
                    {
                        object newKeyValuePairs = DiagramUtil.AsDictionary(newVal, oldVal);
                        Dictionary<string, object> propertyChanges = new Dictionary<string, object>();
                        if (newKeyValuePairs is Dictionary<string, object> objects && objects.Count > 0)
                            propertyChanges.Add(propertyName, objects);
                        else
                            propertyChanges.Add(propertyName, new PropertyChangeValues() { NewValue = newVal, OldValue = oldVal });
                        _ = DiagramContent.OnPropertyChanged(propertyChanges, newVal, oldVal, parent);
                    }
                    else
                    {
                        Dictionary<string, object> propertyChanges = new Dictionary<string, object> { { propertyName, new PropertyChangeValues() { NewValue = newVal, OldValue = oldVal } } };
                        _ = DiagramContent.OnPropertyChanged(propertyChanges, newVal, oldVal, parent);
                    }


                    PropertyChangedEventArgs propertyChangeEvtArgs = new PropertyChangedEventArgs()
                    {
                        Element = parent,
                        PropertyName = propertyName,
                        NewValue = propertyNewValue,
                        OldValue = propertyOldValue
                    };

                    if (DiagramContent.Parent.PropertyChanges.Count > 0)
                    {
                        CommandHandler.InvokeDiagramEvents(DiagramEvent.PropertyChanged, propertyChangeEvtArgs);
                    }
                    if (!(DiagramAction.HasFlag(DiagramAction.Group)) && !FirstRender && !(DiagramAction.HasFlag(DiagramAction.UndoRedo)) && !(DiagramAction.HasFlag(DiagramAction.Interactions)))
                    {
                        InternalHistoryEntry entry = new InternalHistoryEntry()
                        {
                            Type = HistoryEntryType.PropertyChanged,
                            Category = EntryCategory.InternalEntry,
                            PropertyChangeEvtArgs = propertyChangeEvtArgs
                        };
                        if (HistoryManager != null)
                        {
                            AddHistoryEntry(entry);
                        }
                    }
                    if (newVal != null && newVal is GradientBrush gradient)
                    {
                        if (propertyChangeEvtArgs.Element is ShapeStyle style)
                        {
                            if (style.Parent is Node node && node.Wrapper != null)
                            {
                                if (DiagramContent.GradientCollection.ContainsKey(node.Wrapper.Children[0].ID))
                                {
                                    DiagramContent.GradientCollection[node.Wrapper.Children[0].ID] = gradient;
                                }
                                else
                                {
                                    DiagramContent.GradientCollection.Add(node.Wrapper.Children[0].ID, gradient);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void UpdateBridgeDirection(Direction direction)
        {
            BridgeDirection = direction;
            DiagramContent?.UpdateBridging();
        }
        /// <summary>
        /// Scales the given objects to the size of the first object in the group.
        /// </summary>
        /// <param name="sizingType">SizingMode</param>
        /// <param name="objects">DiagramObjectCollection</param>
        public void SetSameSize(SizingMode sizingType, DiagramObjectCollection<NodeBase> objects = null)
        {
            objects = ConcatSelectedObjects(objects);
            this.CommandHandler.SameSize(objects, sizingType);
        }
        /// <summary>
        /// It arranges the group of nodes/connectors at equal intervals within the group of nodes/connectors. 
        /// </summary>
        /// <param name="option">DistributeOptions</param>
        /// <param name="objects">DiagramObjectCollection</param>
        public void SetDistribute(DistributeOptions option, DiagramObjectCollection<NodeBase> objects = null)
        {
            objects = ConcatSelectedObjects(objects);
            this.CommandHandler.Distribute(objects, option);
        }
        /// <summary>
        /// Aligns the group of objects with reference to the first object in the group. 
        /// </summary>
        /// <param name="alignmentOptions">AlignmentOptions</param>
        /// <param name="objects">DiagramObjectCollection</param>
        /// <param name="alignmentMode">AlignmentMode</param>
        public void SetAlign(AlignmentOptions alignmentOptions, DiagramObjectCollection<NodeBase> objects = null, AlignmentMode alignmentMode = AlignmentMode.Object)
        {
            this.StartGroupAction();
            objects = ConcatSelectedObjects(objects);
            alignmentMode = alignmentMode == AlignmentMode.Object ? AlignmentMode.Object : AlignmentMode.Selector;
            this.CommandHandler.Align(objects, alignmentOptions, alignmentMode);
            this.EndGroupAction();
        }
        private DiagramObjectCollection<NodeBase> ConcatSelectedObjects(DiagramObjectCollection<NodeBase> objects)
        {
            if (objects == null)
            {
                objects = new DiagramObjectCollection<NodeBase>();
                foreach (Node node in SelectionSettings.Nodes)
                {
                    objects.Add(node);
                }
                foreach (Connector connector in SelectionSettings.Connectors)
                {
                    objects.Add(connector);
                }
            }
            return objects;
        }
        internal void UpdateConstraints(DiagramConstraints diagramConstraints)
        {
            Constraints = diagramConstraints;
            if (DiagramContent != null)
            {
                DiagramContent.UpdateBridging();
            }
        }
        internal void UpdateSnapSettings(SnapSettings snapSettings)
        {
            SnapSettings = snapSettings ?? SnapSettings.Initialize();
        }
        internal void UpdateHistory(DiagramHistoryManager history)
        {
            HistoryManager = history ?? DiagramHistoryManager.Initialize();
        }

        internal void UpdateSnapping(Snapping snapping)
        {
            Snapping = snapping ?? Snapping.Initialize(this);
        }
        internal void UpdatePageSettings(PageSettings pageSettings)
        {
            PageSettings = pageSettings ?? PageSettings.Initialize();
            PageSettings.DiagramID = this.ID;
        }
        internal void UpdateLayout(Layout layout)
        {
            Layout = layout ?? Layout.Initialize();
        }
        internal void UpdateScrollSettings(ScrollSettings scrollSettings)
        {
            ScrollSettings = scrollSettings ?? ScrollSettings.Initialize();
            Scroller = new Scroller(this);
        }
        internal void UpdateHierarchicalTree(HierarchicalTree hierarchicalTree)
        {
            HierarchicalTree = hierarchicalTree ?? HierarchicalTree.Initialize();
        }

        internal void DiagramStateHasChanged()
        {
            InvokeAsync(StateHasChanged);
        }
        internal string GetTransformValue()
        {
            TransformFactor transform = this.Scroller.Transform;
            double scale = transform.Scale;
            string gTransform = "translate(" + (transform.TX * scale) + "," + (transform.TY * scale) + "),scale(" + scale + ")";
            return gTransform;
        }
        /// <summary>
        /// Rotates the given nodes/connectors at the given angle. 
        /// </summary>
        /// <param name="obj">IDiagramObject</param>
        /// <param name="angle">double</param>
        /// <param name="pivot">Point</param>
        /// <returns></returns>
        public bool Rotate(IDiagramObject obj, double angle, DiagramPoint pivot = null)
        {
            bool checkBoundaryConstraints = false;
            if (obj != null)
            {
                if (pivot == null)
                    pivot = obj is DiagramSelectionSettings selector ? new DiagramPoint() { X = selector.Wrapper.OffsetX, Y = selector.Wrapper.OffsetY } : new DiagramPoint() { X = ((Node)obj).Wrapper.OffsetX, Y = ((Node)obj).Wrapper.OffsetY };
                List<NodeBase> objects = new List<NodeBase>();
                if (obj is DiagramSelectionSettings selectorRotate)
                {
                    selectorRotate.RotationAngle += angle;
                    selectorRotate.Wrapper.RotationAngle += angle;
                    DiagramRect bounds = BaseUtil.GetBounds(selectorRotate.Wrapper);
                    checkBoundaryConstraints = this.CommandHandler.CheckBoundaryConstraints(0, 0, bounds);
                    if (!checkBoundaryConstraints)
                    {
                        selectorRotate.RotationAngle -= angle;
                        selectorRotate.Wrapper.RotationAngle -= angle;
                        return checkBoundaryConstraints;
                    }
                    objects.AddRange(selectorRotate.Nodes);
                    objects.AddRange(selectorRotate.Connectors);
                    this.CommandHandler.RotateObjects(selectorRotate, objects, angle, pivot);
                }
                else
                {
                    if (obj is Node && !DiagramAction.HasFlag(DiagramAction.UndoRedo))
                    {
                        (obj as Node).RotationAngle += angle;
                        (obj as Node).Wrapper.RotationAngle += angle;
                        DiagramRect bounds = BaseUtil.GetBounds((obj as Node).Wrapper);
                        checkBoundaryConstraints = this.CommandHandler.CheckBoundaryConstraints(0, 0, bounds);
                        if (!checkBoundaryConstraints)
                        {
                            (obj as Node).RotationAngle -= angle;
                            (obj as Node).Wrapper.RotationAngle -= angle;
                            return checkBoundaryConstraints;
                        }
                    }
                    objects.Add(obj as NodeBase);
                    this.CommandHandler.RotateObjects(obj, objects, angle, pivot, false);
                }
            }
            return checkBoundaryConstraints;
        }

        ///<summary>
        /// Moves the source point of the connector
        /// </summary>     
        internal void DragSourceEnd(Connector obj, double tx, double ty)
        {
            this.CommandHandler.DragSourceEnd(obj, tx, ty);
        }

        ///<summary>
        /// Moves the target point of the connector
        /// </summary>
        internal void DragTargetEnd(Connector obj, double tx, double ty)
        {
            this.CommandHandler.DragTargetEnd(obj, tx, ty);
        }
        private void InitializePrivateVariables()
        {
            InnerLayerWidth = this.Width;
            InnerLayerHeight = this.Height;
            this.CommandHandler ??= new CommandHandler(this);
            this.SpatialSearch ??= new SpatialSearch();
            this.EventHandler ??= new DiagramEventHandler(this, CommandHandler);

            DiagramLayer = ID + "_diagramLayer_div";
            DiagramAdornerLayer = ID + "_diagramAdornerLayer";
            DiagramAdornerSvg = ID + "_diagramAdorner_svg";
            SelectorElement = ID + "_SelectorElement";
            DiagramAdorner = ID + "_diagramAdorner";
            DiagramPorts = ID + "_diagramPorts_svg";
            DiagramHtml = ID + "_htmlLayer";
            DiagramUserHandle = ID + "_diagramUserHandleLayer";
            DiagramHtmlDiv = ID + "_htmlLayer_div";
            DiagramUserHandleDiv = ID + "_diagramUserHandleLayer_div";
            DiagramBackground = ID + "_backgroundLayer_svg";
            GridLineSvg = ID + "_" + GRIDLINE + "_svg";
            SvgLayer = ID + "_diagramLayer" + "_svg";
            DiagramContentId = ID + CONTENT;
            NodesGParent = ID + "_diagramlayer";
            DiagramGridLine = ID + "_" + SfDiagramComponent.GRIDLINE;
            DiagramPattern = ID + SfDiagramComponent.PATTERN;

            InnerLayerList = new string[] {
                DiagramBackground,
                GridLineSvg,
                DiagramLayer,
                SvgLayer,
                DiagramHtml,
                DiagramPorts,
                DiagramAdornerLayer,
                DiagramAdornerSvg,
                DiagramUserHandle,
                NodesGParent,
                DiagramGridLine,
                DiagramPattern,
                DiagramContentId
            };
        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            this.ScriptModules = SfScriptModules.SfDiagramComponent;
            await base.OnInitializedAsync().ConfigureAwait(true);
            Internal.DictionaryBase.InitializeDefaultValues();
            UpdateScrollSettings(ScrollSettings);
            UpdateHierarchicalTree(HierarchicalTree);
            UpdateMindMap(MindMap);
            this.InitializePrivateVariables();
            UpdateSnapping(Snapping);
            UpdateSnapSettings(SnapSettings);
            UpdatePageSettings(PageSettings);
            UpdateDataSourceSetting(DataSourceSettings);
            UpdateHistory(HistoryManager);
            bridgeDirection = BridgeDirection;
            constraints = Constraints;
            Localizer = new LocaleProvider(localizer, LocaleStrings.DiagramComponent);
            InitHistory();
        }
        internal void UpdateDataSourceSetting(DataSourceSettings dataSourceSetting)
        {
            DataSourceSettings = dataSourceSetting;
        }
        internal static void UpdateThumbConstraints(ObservableCollection<IDiagramObject> nodes, DiagramSelectionSettings selectorModel, bool canInitialize = false)
        {
            double length = nodes.Count;
            for (int i = 0; i < length; i++)
            {
                IDiagramObject obj = nodes[i];
                ThumbsConstraints thumbConstraints = selectorModel.ThumbsConstraints;
                if (obj is Node node)
                {
                    bool hideRotate = (node.Shape.Type == Shapes.Bpmn && node.Shape is BpmnShape shape && (shape.Shape == BpmnShapes.Activity &&
                                                                                                     (shape.Activity.SubProcess.Collapsed == false)) || (node.Shape is BpmnShape bpmnShapeText &&
                                                                                                                                                                             bpmnShapeText.Shape == BpmnShapes.TextAnnotation));
                    bool hideResize = node.Shape is BpmnShape bpmnShape && (bpmnShape.Type == Shapes.Bpmn && bpmnShape.Shape == BpmnShapes.TextAnnotation);
                    if (!ConstraintsUtil.CanRotate(node) || !(thumbConstraints.HasFlag(ThumbsConstraints.Rotate)) || hideRotate)
                    {
                        thumbConstraints &= ~ThumbsConstraints.Rotate;
                    }
                    if (!CanResize(node, Actions.ResizeSouthEast) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeSouthEast)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeSouthEast;
                    }
                    if (!CanResize(node, Actions.ResizeNorthWest) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeNorthWest)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeNorthWest;
                    }
                    if (!CanResize(node, Actions.ResizeEast) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeEast)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeEast;
                    }
                    if (!CanResize(node, Actions.ResizeWest) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeWest)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeWest;
                    }
                    if (!CanResize(node, Actions.ResizeNorth) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeNorth)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeNorth;
                    }
                    if (!CanResize(node, Actions.ResizeSouth) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeSouth)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeSouth;
                    }
                    if (!CanResize(node, Actions.ResizeNorthEast) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeNorthEast)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeNorthEast;
                    }
                    if (!CanResize(node, Actions.ResizeSouthWest) || !(thumbConstraints.HasFlag(ThumbsConstraints.ResizeSouthWest)) || hideResize)
                    {
                        thumbConstraints &= ~ThumbsConstraints.ResizeSouthWest;
                    }
                }
                else if (obj is Connector connector)
                {
                    if (!canInitialize) { thumbConstraints |= ThumbsConstraints.Default; }
                    if (CanDragSourceEnd(connector))
                    {
                        thumbConstraints |= ThumbsConstraints.ConnectorSource;
                    }
                    else
                    {
                        thumbConstraints &= ~ThumbsConstraints.ConnectorSource;
                    }
                    if (CanDragTargetEnd(connector))
                    {
                        thumbConstraints |= ThumbsConstraints.ConnectorTarget;
                    }
                    else
                    {
                        thumbConstraints &= ~ThumbsConstraints.ConnectorTarget;
                    }
                }
                else
                {
                    if (!canInitialize) { thumbConstraints |= ThumbsConstraints.Default; }
                    if (!CanResize(obj as Annotation, Actions.None))
                    {
                        thumbConstraints &= ~(ThumbsConstraints.ResizeSouthEast | ThumbsConstraints.ResizeSouthWest |
                            ThumbsConstraints.ResizeSouth | ThumbsConstraints.ResizeEast | ThumbsConstraints.ResizeWest |
                            ThumbsConstraints.ResizeNorth | ThumbsConstraints.ResizeNorthEast | ThumbsConstraints.ResizeNorthWest);
                    }
                    if (!ConstraintsUtil.CanRotate(obj as Annotation))
                    {
                        thumbConstraints &= ~ThumbsConstraints.Rotate;
                    }
                }
                selectorModel.ThumbsConstraints = thumbConstraints;
            }
        }
        private static bool CanDragSourceEnd(Connector connector)
        {
            return connector.Constraints.HasFlag(ConnectorConstraints.DragSourceEnd);
        }
        private static bool CanDragTargetEnd(Connector connector)
        {
            return connector.Constraints.HasFlag(ConnectorConstraints.DragTargetEnd);
        }
        private string GetTooltipContent(DiagramSelectionSettings node)
        {
            if (EventHandler.Tool is DragController)
            {
                return Localizer.GetText("DiagramComponent_X") + ":" + Math.Round(node.OffsetX).ToString(CultureInfo.InvariantCulture) + " " + Localizer.GetText("DiagramComponent_Y") + ":" + Math.Round(node.OffsetY).ToString(CultureInfo.InvariantCulture);
            }
            else if (EventHandler.Tool is RotationController)
            {
                return Math.Round(node.RotationAngle).ToString(CultureInfo.InvariantCulture);
            }
            else if (EventHandler.Tool is ResizeController)
            {
                return Localizer.GetText("DiagramComponent_W") + ":" + Math.Round(node.Width).ToString(CultureInfo.InvariantCulture) + " " + Localizer.GetText("DiagramComponent_H") + ":" + Math.Round(node.Height).ToString(CultureInfo.InvariantCulture);
            }
            else if (EventHandler.Tool is ConnectionController || EventHandler.Tool is ConnectorEditing)
            {
                return Localizer.GetText("DiagramComponent_X") + ":" + Math.Round(EventHandler.Tool.currentPoint.X).ToString(CultureInfo.InvariantCulture) + " " + Localizer.GetText("DiagramComponent_Y") + ":" + Math.Round(EventHandler.Tool.currentPoint.Y).ToString(CultureInfo.InvariantCulture);
            }
            return String.Empty;
        }

        private static bool CanResize(IDiagramObject node, Actions direction)
        {
            bool returnValue = false;
            if (node is Node node1)
            {
                if (direction == Actions.ResizeSouthEast)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeSouthEast);
                }
                else if (direction == Actions.ResizeEast)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeEast);
                }
                else if (direction == Actions.ResizeNorthEast)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeNorthEast);
                }
                else if (direction == Actions.ResizeSouth)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeSouth);
                }
                else if (direction == Actions.ResizeNorth)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeNorth);
                }
                else if (direction == Actions.ResizeSouthWest)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeSouthWest);
                }
                else if (direction == Actions.ResizeWest)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeWest);
                }
                else if (direction == Actions.ResizeNorthWest)
                {
                    returnValue = node1.Constraints.HasFlag(NodeConstraints.ResizeNorthWest);
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Method invoked when any changes in component state occur.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            width = BaseUtil.UpdateDictionary(nameof(Width), width, Width, PropertyChanges);
            height = BaseUtil.UpdateDictionary(nameof(Height), height, Height, PropertyChanges);
            DrawingObjectTool = BaseUtil.UpdateDictionary(nameof(DrawingObject), DrawingObjectTool, DrawingObject, PropertyChanges);
            NodeCollection = BaseUtil.UpdateDictionary(nameof(Nodes), NodeCollection, Nodes, PropertyChanges);
            ConnectorCollection = BaseUtil.UpdateDictionary(nameof(Connectors), ConnectorCollection, Connectors, PropertyChanges);
            constraints = BaseUtil.UpdateDictionary(nameof(Constraints), constraints, Constraints, PropertyChanges);
            interactionTool = BaseUtil.UpdateDictionary(nameof(InteractionController), interactionTool, InteractionController, PropertyChanges);
            if (!RealAction.HasFlag(RealAction.PathDataMeasureAsync) && !RealAction.HasFlag(RealAction.EnableGroupAction) && !DiagramAction.HasFlag(DiagramAction.Interactions))
            {
                bridgeDirection = BaseUtil.UpdateDictionary(nameof(BridgeDirection), bridgeDirection, BridgeDirection,
                    PropertyChanges);
                constraints = BaseUtil.UpdateDictionary(nameof(Constraints), constraints, Constraints, PropertyChanges);
                if (PropertyChanges.Any())
                {
                    this.UpdateBridgeDirection(bridgeDirection);
                    this.UpdateConstraints(constraints);
                }
            }
        }
        internal async Task PropertyUpdate(SfDiagramComponent diagram)
        {
            Nodes = await SfBaseUtils.UpdateProperty(diagram.Nodes, Nodes, NodesChanged, null, null);
            Connectors = await SfBaseUtils.UpdateProperty(diagram.Connectors, Connectors, ConnectorsChanged, null, null);
            Width = await SfBaseUtils.UpdateProperty(diagram.Width, Width, WidthChanged, null, null);
            Height = await SfBaseUtils.UpdateProperty(diagram.Height, Height, HeightChanged, null, null);
            Constraints = await SfBaseUtils.UpdateProperty(diagram.Constraints, Constraints, ConstraintsChanged, null, null);
            InteractionController = await SfBaseUtils.UpdateProperty(diagram.InteractionController, InteractionController, InteractionControllerChanged, null, null);
            if (diagram.DataSourceSettings != null)
                await DataSourceSettings.PropertyUpdate(diagram.DataSourceSettings);
            await SnapSettings.PropertyUpdate(diagram.SnapSettings);
            await PageSettings.PropertyUpdate(diagram.PageSettings);
            await ScrollSettings.PropertyUpdate(diagram.ScrollSettings);
            if (diagram.Layout != null)
                await Layout.PropertyUpdate(diagram.Layout);
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                if (PropertyChanges.ContainsKey(nameof(Width)) || PropertyChanges.ContainsKey(nameof(Height)))
                {
                    _ = UpdateViewPort();
                }
            }
            if (this.CommandHandler.HelperObject != null && (EventHandler.Tool is DragController || EventHandler.Tool is RotationController || EventHandler.Tool is ResizeController
                || EventHandler.Tool is ConnectionController || EventHandler.Tool is ConnectorEditing))
            {
                string content = GetTooltipContent(this.CommandHandler.HelperObject);
                CommandHandler.ShowTooltip(content, EventHandler.Tool.IsTooltipVisible);
                EventHandler.Tool.IsTooltipVisible = false;
            }
            if (!this.RealAction.HasFlag(RealAction.PathDataMeasureAsync))
                await base.OnAfterRenderAsync(firstRender);
        }
        /// <summary>
        /// This method returns a boolean to indicate if a component’s UI can be rendered. 
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ShouldRender()
        {
            if (this.RealAction.HasFlag(RealAction.PreventEventRefresh))
            {
                return false;
            }
            return true;
        }

        internal void RemoveDependentConnector(Node node)
        {
            string[] edges = new string[node.OutEdges.Count + node.InEdges.Count];
            node.OutEdges.CopyTo(edges, node.InEdges.Count);
            node.InEdges.CopyTo(edges, node.OutEdges.Count);
            for (int i = edges.Length - 1; i >= 0; i--)
            {
                if (this.NameTable[edges[i]] is Connector connector)
                {
                    this.Connectors.Remove(connector);
                }
            }
        }
        internal void RemoveLabels(NodeBase obj, IList labels)
        {
            obj = this.NameTable.ContainsKey(obj.ID) ? this.NameTable[obj.ID] as NodeBase : obj as NodeBase;

            if (labels.Count > 1)
            {
                this.StartGroupAction();
            }
            this.UndoRedoCount++;
            for (int j = labels.Count - 1; j >= 0; j--)
            {
                if (obj is Node node)
                {
                    ShapeAnnotation annotation = null;
                    for (int k = node.Annotations.Count - 1; k >= 0; k--)
                    {
                        if (node.Annotations[k].ID == ((Annotation)labels[j]).ID)
                        {
                            annotation = node.Annotations[k];
                            break;
                        }
                    }
                    if (annotation != null)
                    {
                        node.Annotations.Remove(annotation);
                    }
                }
                if (obj is Connector connector)
                {
                    PathAnnotation annotation = null;
                    for (int k = connector.Annotations.Count - 1; k >= 0; k--)
                    {
                        if (connector.Annotations[k].ID == ((Annotation)labels[j]).ID)
                        {
                            annotation = connector.Annotations[k];
                            break;
                        }
                    }
                    if (annotation != null)
                    {
                        connector.Annotations.Remove(annotation);
                    }
                }
            }
            this.UndoRedoCount--;
            if (labels.Count > 1)
            {
                this.EndGroupAction();
            }
            if (this.UndoRedoCount == 0)
            {
                this.DiagramAction &= ~DiagramAction.UndoRedo;
            }
        }
        internal override async Task OnAfterScriptRendered()
        {
            IsScriptRendered = true;
            if (this.CommandManager == null)
            {
                this.CommandManager = new CommandManager();
            }
            selfReference = DotNetObjectReference.Create(EventHandler);
            DiagramSize patternSize = null;
            if (DiagramContent != null) {
                patternSize = DiagramContent.GetPatternSize();
            }
            DomUtil.CreateMeasureElements(JSRuntime, interactionTool == InteractionController.ZoomPan, InnerLayerList, InnerLayerWidth, InnerLayerHeight, DiagramContentId, selfReference, Scroller.Transform, patternSize, SnapSettings.PathData, SnapSettings.DotsData);
            DataBindingModule = new DiagramDataBinding();
            if (this.DiagramContent != null)
                await this.DiagramContent.OnScriptRendered();
        }
        internal void RemovePorts(NodeBase obj, DiagramObjectCollection<PointPort> ports)
        {
            obj = this.NameTable.ContainsKey(obj.ID) ? this.NameTable[obj.ID] as NodeBase : obj;

            if (ports.Count > 1)
            {
                this.StartGroupAction();
            }
            for (int j = ports.Count - 1; j >= 0; j--)
            {
                if (obj is Node node)
                {
                    PointPort port = null;
                    for (int k = node.Ports.Count - 1; k >= 0; k--)
                    {
                        if (node.Ports[k].ID == ports[j].ID)
                        {
                            port = node.Ports[k];
                            break;
                        }
                    }
                    if (port != null)
                    {
                        node.Ports.Remove(port);
                    }
                }
            }
            if (ports.Count > 1)
            {
                this.EndGroupAction();
            }
        }
        internal async void AddLabels(NodeBase obj, DiagramObjectCollection<Annotation> labels)
        {
            if (labels.Count > 1)
            {
                this.StartGroupAction();
            }
            this.UndoRedoCount++;
            for (int j = labels.Count - 1; j >= 0; j--)
            {
                if (obj is Node node)
                {
                    await node.Annotations.AddAsync(labels[j] as ShapeAnnotation);
                }
                if (obj is Connector connector)
                {
                    await connector.Annotations.AddAsync(labels[j] as PathAnnotation);
                }
            }
            this.UndoRedoCount--;
            if (labels.Count > 1)
            {
                this.EndGroupAction();
            }
            if (this.UndoRedoCount == 0)
            {
                this.DiagramAction &= ~DiagramAction.UndoRedo;
                DiagramStateHasChanged();
            }
        }
        internal void AddPorts(NodeBase obj, DiagramObjectCollection<PointPort> ports)
        {
            if (ports.Count > 1)
            {
                this.StartGroupAction();
            }
            for (int j = ports.Count - 1; j >= 0; j--)
            {
                if (obj is Node node)
                {
                    node.Ports.Add(ports[j]);
                }
            }
            if (ports.Count > 1)
            {
                this.EndGroupAction();
            }
        }
        internal async Task InitRemoteData()
        {
            if (DataSourceSettings != null && DataSourceSettings.DataSource == null && DataSourceSettings.DataManager != null && DataSourceSettings.DataManager is SfDataManager)
            {
                DiagramAction &= ~DiagramAction.Layouting;
                await RefreshDataSource();
                DiagramAction |= DiagramAction.Layouting;
            }
        }

        internal void UpdateNameTable(IList items)
        {
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    NodeBase obj = items[i] as NodeBase;
                    if (obj != null && obj.ID != null && ConstraintsUtil.CanDelete(obj) || (obj is Node && !this.Nodes.Contains(obj)) || (obj is Connector && !this.Connectors.Contains(obj)))
                    {
                        if (obj.ID != null && NameTable.ContainsKey(obj.ID))
                        {
                            NameTable.Remove(obj.ID);
                            if (obj is Node node && node.Shape is BpmnShape && (node.Shape as BpmnShape).Annotations.Count > 0)
                            {
                                this.BpmnDiagrams.CheckAndRemoveAnnotations(node, this);
                            }
                        }
                        if (SpatialSearch.ObjectTable.ContainsKey(obj.ID))
                        {
                            SpatialSearch.RemoveFromAQuad(obj.Wrapper);
                            SpatialSearch.ObjectTable.Remove(obj.ID);
                            SpatialSearch.UpdateBounds(obj.Wrapper);
                            DiagramContent.UpdateBridging();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// This method will refresh the layout based on the changes in the data source. 
        /// </summary>
        /// <returns>RefreshDataSource</returns>
        public async Task RefreshDataSource()
        {
            await DiagramContent.RefreshDataSource();
        }

        #region Public Methods
        internal void ClearObjects()
        {
            List<IDiagramObject> objects = new List<IDiagramObject>();

            objects.AddRange(this.NodeCollection);
            objects.AddRange(this.ConnectorCollection);

            this.RealAction |= RealAction.PreventRefresh;
            foreach (NodeBase obj in objects)
            {
                if (this.NameTable.ContainsKey(obj.ID))
                {
                    this.NameTable.Remove(obj.ID);
                    if (obj is Node node && node.Shape is BpmnShape shape && shape.Annotations.Count > 0)
                    {
                        this.BpmnDiagrams.CheckAndRemoveAnnotations(node, this);
                    }
                }
            }
            this.RealAction &= ~RealAction.PreventRefresh;
            this.SpatialSearch = new SpatialSearch();
        }
        /// <summary>
        /// Serializes the diagram control as a string. 
        /// </summary>
        /// <returns>jsonData</returns>
        public string SaveDiagram()
        {
            JsonSerializerOptions settings = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var drawObject = this.DrawingObject as IDiagramObject;
            this.DrawingObject = null;
            string jsonData = JsonSerializer.Serialize(this, settings);
            this.DrawingObject = drawObject;
            return jsonData;
        }
        /// <summary>
        /// Converts the given string into a Diagram Control. 
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>Diagram</returns>
        public async Task LoadDiagram(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                JsonSerializerOptions settings = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                SfDiagramComponent sfDiagram = JsonSerializer.Deserialize<SfDiagramComponent>(data, settings);
                IsRendered = false;
                this.ClearObjects();
                this.CommandHandler.ClearSelection();
                RealAction |= RealAction.PreventRefresh;
                await this.PropertyUpdate(sfDiagram);
                RealAction &= ~RealAction.PreventRefresh;
                await DiagramContent.LoadDiagram(true);
                IsRendered = true;
            }
        }
        internal void TextChangeEvent(string oldValue, string newValue, IDiagramObject element, Annotation annotation, TextChangeEventArgs args)
        {
            args.OldValue = oldValue;
            args.NewValue = newValue;
            args.Element = element;
            args.Annotation = annotation;
            this.CommandHandler.InvokeDiagramEvents(DiagramEvent.TextChanged, args);
        }
        private static Annotation AddAnnotation(NodeBase node)
        {
            ShapeAnnotation nodeAnnotation = new ShapeAnnotation()
            {
                Content = string.Empty,
            };
            Annotation newAnnotation = nodeAnnotation;
            if (node is NodeGroup groupNode)
            {
                Canvas groupCanvas = groupNode.Wrapper.Children[0] as Canvas;
                groupCanvas.Children.Add(groupNode.InitAnnotationWrapper(nodeAnnotation, false));
            }
            else
            {
                if (node is Node node1)
                {
                    ObservableCollection<ICommonElement> nodeCanvas = node1.Wrapper.Children;
                    nodeCanvas.Add(node1.InitAnnotationWrapper(nodeAnnotation, false));
                }
            }
            if (node is Connector connector)
            {
                PathAnnotation pathAnnotation = new PathAnnotation()
                {
                    Content = string.Empty,
                };
                connector.Wrapper.Children.Add(connector.GetAnnotationElement(pathAnnotation, connector.IntermediatePoints, connector.Bounds));
                newAnnotation = pathAnnotation;
            }
            node.Wrapper.Measure(new DiagramSize() { Width = node.Wrapper.Width, Height = node.Wrapper.Height });
            node.Wrapper.Arrange(node.Wrapper.DesiredSize);
            return newAnnotation;
        }
        private static Annotation GetAnnotation(DiagramObjectCollection<ShapeAnnotation> shapeAnnotation, DiagramObjectCollection<PathAnnotation> pathAnnotation, IDiagramObject diagramObject)
        {
            Annotation annotation;
            if ((shapeAnnotation != null && shapeAnnotation.Count > 0) || (pathAnnotation != null && pathAnnotation.Count > 0))
            {
                if (shapeAnnotation != null && shapeAnnotation.Count > 0)
                {
                    annotation = shapeAnnotation[0];
                }
                else
                {
                    annotation = pathAnnotation[0];
                }
            }
            else
            {
                annotation = AddAnnotation(diagramObject as NodeBase);
            }
            return annotation;
        }
        /// <summary>
        /// Edits the annotation of the node/connector. 
        /// </summary>
        /// <param name="diagramObject">IDiagramObject</param>
        /// <param name="id">String</param>
        public void StartTextEdit(IDiagramObject diagramObject, String id = null)
        {
            if (diagramObject != null && ((!ConstraintsUtil.CanZoomPan(this) && !ConstraintsUtil.CanMultiSelect(this)) || ConstraintsUtil.CanSingleSelect(this)))
            {
                this.DiagramAction |= DiagramAction.EditText;
                Annotation annotation = null;
                if (diagramObject is Node node)
                {
                    if (id == null)
                    {
                        annotation = GetAnnotation(node.Annotations, null, node);
                    }
                }
                else
                {
                    annotation = GetAnnotation(null, ((Connector)diagramObject).Annotations, diagramObject);
                }
                if (diagramObject is Node nodeAnnotation)
                {
                    for (int i = 0; i < nodeAnnotation.Annotations.Count; i++)
                    {
                        if (id != null && (id == nodeAnnotation.Annotations[i].ID || id == nodeAnnotation.ID + "_" + nodeAnnotation.Annotations[i].ID))
                        {
                            annotation = nodeAnnotation.Annotations[i];
                        }
                    }
                }
                else if (diagramObject is Connector connectorAnnotation)
                {
                    for (int i = 0; i < connectorAnnotation.Annotations.Count; i++)
                    {
                        if (id != null && id == connectorAnnotation.ID + "_" + connectorAnnotation.Annotations[i].ID)
                        {
                            annotation = connectorAnnotation.Annotations[i];
                        }
                    }
                }
                if (!ConstraintsUtil.EnableReadOnly(diagramObject, annotation) && annotation != null)
                {
                    TextElementUtils textElementUtils = new TextElementUtils
                    {
                        Bounds = new DiagramSize(),
                        Style = annotation.Style,
                        Content = annotation.Content
                    };
                    ICommonElement annotationWrapper = DiagramUtil.GetWrapper(diagramObject as NodeBase, (diagramObject as NodeBase).Wrapper, annotation.ID) as DiagramElement;
                    if (annotationWrapper != null)
                    {
                        textElementUtils.Bounds = new DiagramSize() { Width = annotation.Width, Height = annotation.Height };
                        DiagramPoint centerPoint = annotationWrapper.Bounds.Center;
                        textElementUtils.NodeSize = new DiagramSize()
                        {
                            Width = (diagramObject as NodeBase).Wrapper.ActualSize.Width,
                            Height = (diagramObject as NodeBase).Wrapper.ActualSize.Height
                        };
                        DiagramRect Bounds = (diagramObject is Node)
                            ? (diagramObject as Node).Wrapper.Bounds
                            : (diagramObject as Connector).Wrapper.Bounds;
                        _ = DomUtil.TextEdit(textElementUtils, centerPoint, Bounds, this.Scroller.Transform,
                            ConstraintsUtil.CanZoomTextEdit(this), annotationWrapper.ID);
                    }
                }
            }
        }
        internal void StartEditCommand()
        {
            IDiagramObject node = null;
            if (this.SelectionSettings.Nodes.Count != 0)
            {
                node = this.SelectionSettings.Nodes[0];
            }
            else if (this.SelectionSettings.Connectors.Count != 0)
            {
                node = this.SelectionSettings.Connectors[0];
            }
            this.StartTextEdit(node);

        }
        /// <summary>
        /// Selects the given collection of objects.
        /// </summary>
        /// <param name="objects">ObservableCollection</param>
        /// <param name="multipleSelection">bool</param>
        public void Select(ObservableCollection<IDiagramObject> objects, bool? multipleSelection = false)
        {
            DiagramAction |= DiagramAction.PublicMethod;
            if (objects != null && objects.Count > 0)
            {
                CommandHandler.SelectObjects(objects, multipleSelection != null && multipleSelection.Value);
            }
            DiagramAction &= ~DiagramAction.PublicMethod;
        }

        ///<summary>
        /// Select all the objects.
        ///</summary>
        public void SelectAll()
        {
            DiagramAction |= DiagramAction.PublicMethod;
            ObservableCollection<IDiagramObject> objects = new ObservableCollection<IDiagramObject>();
            for (int i = 0; i < NodeCollection.Count; i++)
            {
                objects.Add(NodeCollection[i]);
            }
            for (int i = 0; i < ConnectorCollection.Count; i++)
            {
                objects.Add(ConnectorCollection[i]);
            }
            Select(objects);
            this.RealAction |= (RealAction.PreventRefresh | RealAction.RefreshSelectorLayer);
            this.DiagramStateHasChanged();
            this.RealAction &= ~(RealAction.PreventRefresh | RealAction.RefreshSelectorLayer);
            DiagramAction &= ~DiagramAction.PublicMethod;
        }

        /// <summary>
        /// Removes the given object from the selection list. 
        /// </summary>
        public void UnSelect(IDiagramObject obj)
        {
            DiagramAction |= DiagramAction.PublicMethod;
            if (obj != null && this.CommandHandler.IsSelected(obj))
            {
                CommandHandler.UnSelect(obj);
            }
            DiagramAction &= ~DiagramAction.PublicMethod;
        }

        ///<summary>
        /// It allows the user to clear the selected nodes/connectors in the diagram. 
        /// </summary>        
        public void ClearSelection()
        {
            DiagramAction |= DiagramAction.PublicMethod;
            CommandHandler.ClearSelection(true);
            this.RealAction |= (RealAction.PreventRefresh | RealAction.RefreshSelectorLayer);
            this.DiagramStateHasChanged();
            this.RealAction &= ~(RealAction.PreventRefresh | RealAction.RefreshSelectorLayer);
            DiagramAction &= ~DiagramAction.PublicMethod;
        }

        /// <summary> 
        /// Prints the diagram pages based on <see cref="DiagramPrintSettings"/>. 
        /// </summary>
        /// <param name="printSettings">Specifies the configuration settings to print the diagram. </param> 
        ///<returns> The <see cref="Task"/> that completes when the diagram is sent to browser print preview window for printing.</returns> 
        /// examples for the following:
        ///<example>
        ///<code>
        /// DiagramPrintSettings print = new DiagramPrintSettings();
        /// print.PageWidth = 816;
        /// print.PageHeight = 1054;
        /// print.Region = DiagramPrintExportRegion.PageSettings;
        /// print.FitToPage = true;
        /// print.Orientation = PageOrientation.Landscape;
        /// print.Margin = new Margin() { Left = 10, Top = 10, Right = 10, Bottom = 10 };
        /// await diagram.PrintAsync(print);
        ///</code>
        ///</example>
        public async Task PrintAsync(DiagramPrintSettings printSettings)
        {
            if (printSettings != null)
            {
                DiagramRect bounds = GetDiagramBounds(printSettings.Region);
                await JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, "sfBlazor.Diagram.print", new object[] { printSettings, this.ID, bounds, PageSettings }).ConfigureAwait(true);
            }
        }

        /// <summary> 
        /// Exports the rendered diagram to various file types. It supports jpeg, png, svg ,bmp and pdf file types. Exported file will get download at client machine.  
        /// </summary> 
        /// <param name="fileName">Specifies the filename without extension. </param> 
        /// <param name="fileFormat"> Specifies the export type for the rendered diagram </param> 
        /// <param name="exportSettings"> Specifies the configutation settings to export the diagram </param> 
        ///<remarks>Diagram supports jepg, png and svg file types. </remarks>  
        /// examples for the following:
        ///<example>
        ///<code>
        ///  DiagramExportSettings export = new DiagramExportSettings();
        ///  export.Region = DiagramPrintExportRegion.PageSettings;
        ///  export.PageWidth = 816;
        ///  export.PageHeight = 1054;
        ///  export.Orientation = PageOrientation.Landscape;
        ///  export.FitToPage = true;
        ///  export.Margin = new Margin() { Left = 10, Top = 10, Right = 10, Bottom = 10 };
        ///  export.ClipBounds = new DiagramRect() { X = 0, Y = 0, Width = 0, Height = 0 };
        ///  //To export the diagram
        ///  await diagram.ExportAsync("diagram", format, print);
        ///</code>
        ///</example>
        public async Task ExportAsync(string fileName, DiagramExportFormat fileFormat, DiagramExportSettings exportSettings)
        {
            if (exportSettings != null)
            {
                DiagramRect bounds = GetDiagramBounds(exportSettings.Region, fileFormat);
                await JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, "sfBlazor.Diagram.exportDiagram", new object[] { exportSettings, this.ID, bounds, PageSettings, false, fileName, fileFormat, true });
            }
        }

        /// <summary> 
        /// Exports the diagram to base64 string. Exported string can be returned.  
        /// </summary> 
        /// <param name="fileFormat">Specifies the export type for the rendered diagram.</param> 
        /// <param name="exportSettings"> Specifies the configutation settings to export the diagram </param> 
        /// <returns> The exported diagram as base64 string of the specified file type</returns> 
        /// examples for the following:
        ///<example>
        ///<code>
        ///  DiagramExportSettings export = new DiagramExportSettings();
        ///  export.Region = DiagramPrintExportRegion.PageSettings;
        ///  export.PageWidth = 816;
        ///  export.PageHeight = 1054;
        ///  export.Orientation = PageOrientation.Landscape;
        ///  export.FitToPage = true;
        ///  export.Margin = new Margin() { Left = 10, Top = 10, Right = 10, Bottom = 10 };
        ///  export.ClipBounds = new DiagramRect() { X = 0, Y = 0, Width = 0, Height = 0 };
        ///  //To export the diagram
        ///   string[] base64 = await diagram.ExportAsync(DiagramExportFormat.PNG, export);
        ///</code>
        ///</example>
        public async Task<string[]> ExportAsync(DiagramExportFormat fileFormat, DiagramExportSettings exportSettings)
        {
            if (exportSettings != null)
            {
                DiagramRect bounds = GetDiagramBounds(exportSettings.Region, fileFormat);
                string data = await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfBlazor.Diagram.exportDiagram", new object[] { exportSettings, this.ID, bounds, PageSettings, false, "diagram", fileFormat, false });
                JsonSerializerOptions settings = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                string[] base64 = JsonSerializer.Deserialize<string[]>(data, settings);

                return base64;
            }
            return null;
        }

        /**   @private  */
        private DiagramRect GetDiagramBounds(DiagramPrintExportRegion mode, DiagramExportFormat diagramExport = DiagramExportFormat.PNG)
        {
            DiagramRect rect = this.GetObjectBounds(diagramExport);
            double left = rect.Left;
            double top = rect.Top;
            double right = rect.Right - rect.Left;
            double bottom = rect.Bottom - rect.Top;
            if (mode != DiagramPrintExportRegion.Content)
            {
                double width = BaseUtil.GetDoubleValue(PageSettings.Width);
                double height = BaseUtil.GetDoubleValue(PageSettings.Height);
                if (this.PageSettings != null && this.PageSettings.MultiplePage)
                {
                    left = rect.Left;
                    top = rect.Top;

                    if (width != 0)
                    {
                        left = Math.Floor(left / width) * width;
                        right = Math.Ceiling(rect.Right / width) * width - left;
                    }
                    if (height != 0)
                    {
                        top = Math.Floor(top / height) * height;
                        bottom = Math.Ceiling(rect.Bottom / height) * height - top;
                    }
                    if ((rect.Width == 0) && width != 0)
                    {
                        right = width;
                    }
                    if ((rect.Height == 0) && height != 0)
                    {
                        bottom = height;
                    }
                }
                else
                {
                    if (width != 0)
                    {
                        left = 0;
                        right = width;
                    }
                    if (height != 0)
                    {
                        top = 0;
                        bottom = height;
                    }
                }
            }
            DiagramRect svgBounds = new DiagramRect();
            svgBounds.X = left;
            svgBounds.Y = top;
            svgBounds.Width = right;
            svgBounds.Height = bottom;
            return svgBounds;
        }
        private DiagramRect GetObjectBounds(DiagramExportFormat diagramExport = DiagramExportFormat.PNG)
        {
            DiagramObjectCollection<Node> nodes = this.Nodes;
            DiagramRect nodebounds = null;
            foreach (var node in nodes)
            {
                if (node.IsVisible)
                {
                    if (((diagramExport != DiagramExportFormat.SVG && (node as Node).Shape.Type != Shapes.SVG && (node as Node).Shape.Type != Shapes.HTML)
                        || (diagramExport == DiagramExportFormat.SVG && (node as Node).Shape.Type != Shapes.HTML)))
                    {
                        if (nodebounds == null)
                        {
                            nodebounds = node.Wrapper.OuterBounds;
                        }
                        else
                        {
                            nodebounds = nodebounds.UniteRect(node.Wrapper.OuterBounds);
                        }
                    }
                }
            }
            DiagramObjectCollection<Connector> connectors = this.Connectors;
            foreach (var connector in connectors)
            {
                if (connector.IsVisible)
                {
                    if (nodebounds == null)
                    {
                        nodebounds = connector.Wrapper.OuterBounds;
                    }
                }
                else
                {
                    nodebounds = nodebounds.UniteRect(connector.Wrapper.OuterBounds);
                }
            }
            if (nodebounds == null)
            {
                nodebounds = new DiagramRect(0, 0, 0, 0);
            }
            return nodebounds;
        }
        /// <summary>
        /// It drags the given object by the specified pixels. 
        /// </summary>
        /// <param name="obj">IDiagramObject</param>
        /// <param name="tx">double</param>
        /// <param name="ty">double</param>
        public void Drag(IDiagramObject obj, double tx, double ty)
        {
            if (obj != null)
            {
                if (obj is Node node && node.Shape.Type == Shapes.Bpmn)
                {
                    bool updated = this.BpmnDiagrams.UpdateAnnotationDrag(node, this, tx, ty);
                    if (updated)
                    {
                        return;
                    }
                }
                if (obj is DiagramSelectionSettings selector)
                {
                    if (selector.Nodes != null && selector.Nodes.Count > 0)
                    {
                        for (int i = 0; i < selector.Nodes.Count; i++)
                        {
                            Drag(selector.Nodes[i], tx, ty);
                        }
                    }
                    if (selector.Connectors != null && selector.Connectors.Count > 0)
                    {
                        for (int i = 0; i < selector.Connectors.Count; i++)
                        {
                            Drag(selector.Connectors[i], tx, ty);
                        }
                    }
                }
                else
                {
                    CommandHandler.Drag(obj as NodeBase, tx, ty);
                }
            }
            if (!(this.DiagramAction.HasFlag(DiagramAction.Interactions)))
                this.UpdatePage();
        }
        /// <summary>
        /// Scales the given objects by the given ratio. 
        /// </summary>
        /// <param name="obj">IDiagramObject</param>
        /// <param name="sx">double</param>
        /// <param name="sy">double</param>
        /// <param name="pivot">Point</param>
        /// <returns>checkBoundaryConstraints</returns>
        public bool Scale(IDiagramObject obj, double sx, double sy, DiagramPoint pivot)
        {
            bool checkBoundaryConstraints = true;
            if (obj is NodeBase nodeBase && !string.IsNullOrEmpty(nodeBase.ID))
            {
                obj = this.NameTable[nodeBase.ID] ?? nodeBase;
            }
            if (obj is DiagramSelectionSettings selector)
            {
                if (selector.Nodes != null && selector.Nodes.Count > 0)
                {
                    foreach (Node node in selector.Nodes)
                    {
                        checkBoundaryConstraints = this.CommandHandler.Scale(node, sx, sy, pivot, selector);
                        if (!this.CommandHandler.CheckBoundaryConstraints(0, 0, node.Wrapper.Bounds))
                        {
                            this.CommandHandler.Scale(node, 1 / sx, 1 / sy, pivot, selector);
                        }
                    }
                }
                if (selector.Connectors != null && selector.Connectors.Count > 0)
                {
                    foreach (Connector conn in selector.Connectors)
                    {
                        this.CommandHandler.Scale(conn, sx, sy, pivot, selector);
                        if (!this.CommandHandler.CheckBoundaryConstraints(0, 0, conn.Wrapper.Bounds))
                        {
                            this.CommandHandler.Scale(conn, 1 / sx, 1 / sy, pivot, selector);
                        }
                    }
                }
            }
            else
            {
                if (obj != null)
                    this.CommandHandler.Scale(obj as NodeBase, sx, sy, pivot,
                        obj is NodeGroup groupObj ? groupObj.Children != null ? obj : null : null);
            }
            return checkBoundaryConstraints;
        }
        /// <summary>
        /// Allows the user to zoom in or zoom out. 
        /// </summary>
        /// <param name="factor">double</param>
        /// <param name="focusPoint">Point</param>
        public void Zoom(double factor, DiagramPoint focusPoint)
        {
            ScrollActions |= ScrollActions.PublicMethod;
            Scroller.Zoom(factor, 0, 0, focusPoint);
            if (this.ScrollSettings.Parent == null)
            {
                ScrollActions &= ~ScrollActions.PublicMethod;
            }
        }
        /// <summary>
        /// It is used to pan the diagram control to the horizontal and vertical offsets. 
        /// </summary>
        /// <param name="horizontalOffset">double</param>
        /// <param name="verticalOffset">double</param>
        /// <param name="focusedPoint">Point</param>
        public void Pan(double horizontalOffset, double verticalOffset, DiagramPoint focusedPoint = null)
        {
            ScrollActions |= ScrollActions.PublicMethod;
            DiagramContent.SetCursor("grabbing");
            Scroller.Zoom(1, horizontalOffset, verticalOffset, focusedPoint);
            if (this.ScrollSettings.Parent == null)
            {
                ScrollActions &= ~ScrollActions.PublicMethod;
            }
        }
        /// <summary>
        /// It is used to remove the child from the selected group node.
        /// </summary>
        /// <param name="group">NodeGroup</param>
        /// <param name="child">NodeBase</param>
        public void RemoveChild(NodeGroup group, NodeBase child)
        {
            if (child != null)
            {
                group = (NodeGroup)(group != null ? this.NameTable[group.ID] : this.NameTable[child.ParentId]);
                string id = child.ID;
                if (group != null && group.Children.Length > 0)
                {
                    for (int i = 0; i < group.Children.Length; i++)
                    {
                        if (group.Children[i] == id)
                        {
                            List<string> list = new List<string>(group.Children);
                            list.RemoveAt(i);
                            bool severDataBind = IsProtectedOnChange;
                            IsProtectedOnChange = false;
                            group.Children = list.ToArray();
                            IsProtectedOnChange = severDataBind;
                            for (int j = 0; j < group.Wrapper.Children.Count; j++)
                            {
                                if (group.Wrapper.Children[j].ID == id)
                                {
                                    group.Wrapper.Children.RemoveAt(j);
                                }
                            }
                        }
                    }
                }
                while (group != null)
                {
                    group.Wrapper.Measure(new DiagramSize());
                    group.Wrapper.Arrange(group.Wrapper.DesiredSize);
                    if (!string.IsNullOrEmpty(group.ParentId))
                        group = this.NameTable[group.ParentId] as NodeGroup;
                    else
                        group = null;
                }
                DiagramStateHasChanged();
            }
        }
        /// <summary>
        /// This method is used for adding child to the selected group node. 
        /// </summary>
        /// <param name="group">NodeGroup</param>
        /// <param name="child">NodeBase</param>
        /// <returns></returns>
        public async Task AddChild(NodeGroup group, NodeBase child)
        {
            if (group != null && child != null)
            {
                bool severDataBind = IsProtectedOnChange;
                IsProtectedOnChange = true;
                await this.AddChildExtend(group, child);
                IsProtectedOnChange = severDataBind;
                DiagramStateHasChanged();
            }
        }
        internal async Task AddChildExtend(NodeBase node, NodeBase child, int index = -1)
        {
            NodeGroup parentNode = this.NameTable[node.ID] as NodeGroup;
            if (parentNode?.Children != null)
            {
                this.DiagramAction |= DiagramAction.Group;
                string id = child.ID;
                child.ParentId = parentNode.ID;
                if (!this.NameTable.ContainsKey(id))
                {
                    await this.AddDiagramElements(new DiagramObjectCollection<NodeBase>() { child });
                }
                if (!string.IsNullOrEmpty(id) && this.NameTable.ContainsKey(id))
                {
                    if (this.NameTable[id] is NodeBase childNode)
                    {
                        childNode.ParentId = parentNode.ID;

                        if (index > -1)
                        {
                            parentNode.Children[index] = id;
                            parentNode.Wrapper.Children.RemoveAt(index);
                        }
                        else
                        {
                            List<string> list = new List<string>(parentNode.Children)
                            {
                                id
                            };
                            bool severDataBind = IsProtectedOnChange;
                            IsProtectedOnChange = false;
                            parentNode.Children = list.ToArray();
                            IsProtectedOnChange = severDataBind;
                            parentNode.Wrapper.Children.Add(childNode.Wrapper);
                        }

                        parentNode.Wrapper.Measure(new DiagramSize());
                        parentNode.Wrapper.Arrange(parentNode.Wrapper.DesiredSize);
                        if (parentNode is NodeGroup)
                        {
                            ((Node)this.NameTable[node.ID]).Width = parentNode.Wrapper.ActualSize.Width;
                            ((Node)this.NameTable[node.ID]).Height = parentNode.Wrapper.ActualSize.Height;
                            ((Node)this.NameTable[node.ID]).OffsetX = parentNode.Wrapper.OffsetX;
                            ((Node)this.NameTable[node.ID]).OffsetY = parentNode.Wrapper.OffsetY;
                            if (childNode is Node node1)
                            {
                                node1.OffsetX = node1.Wrapper.OffsetX;
                                node1.OffsetY = node1.Wrapper.OffsetY;
                            }
                        }
                    }
                }
                this.DiagramAction &= ~DiagramAction.Group;
            }
        }
        /// <summary>
        /// Group the selected nodes and connectors in the diagram. 
        /// </summary>
        public void Group()
        {
            this.CommandHandler.Group();
        }
        /// <summary>
        /// UnGroup the selected nodes and connectors in the diagram.
        /// </summary>
        public void UnGroup()
        {
            this.CommandHandler.UnGroup();
        }
        /// <summary>
        /// Restores the last action that is performed. 
        /// </summary>
        public void Undo()
        {
            if (this.UndoRedo != null && (this.constraints.HasFlag(DiagramConstraints.UndoRedo)))
            {
                this.UndoRedo.Undo(this);
            }
        }
        /// <summary>
        /// It is used to restore the last undo action. 
        /// </summary>
        public void Redo()
        {
            if (this.UndoRedo != null && (this.constraints.HasFlag(DiagramConstraints.UndoRedo)))
            {
                this.UndoRedo.Redo(this);
            }
        }
        /// <summary>
        /// Starts the grouping of actions that will be undone/restored as a whole. 
        /// </summary>
        public void StartGroupAction()
        {
            InternalHistoryEntry entry = new InternalHistoryEntry() { Type = HistoryEntryType.StartGroup, Category = EntryCategory.InternalEntry };
            if (!(this.DiagramAction.HasFlag(DiagramAction.UndoRedo)))
            {
                this.AddHistoryEntry(entry);
            }
        }
        /// <summary>
        /// It closes the grouping of actions that will be undone/restored as a whole.
        /// </summary>
        public void EndGroupAction()
        {
            InternalHistoryEntry entry = new InternalHistoryEntry() { Type = HistoryEntryType.EndGroup, Category = EntryCategory.InternalEntry };
            if (!(this.DiagramAction.HasFlag(DiagramAction.UndoRedo)))
            {
                this.AddHistoryEntry(entry);
            }
        }
        /// <summary>
        /// This method is used to clear the history.
        /// </summary>
        public void ClearHistory()
        {
            this.UndoRedo?.ClearHistory(this);
        }

        #endregion
        /// <summary>
        /// This method helps to clone the diagram.
        /// </summary>
        /// <returns>Diagram</returns>

        [Browsable(false)]
        public virtual object Clone()
        {
            return MemberwiseClone();
        }
        internal async Task PropertyUpdate(Direction bridgeDirection, DiagramConstraints constraints)
        {
            BridgeDirection = await SfBaseUtils.UpdateProperty(bridgeDirection, BridgeDirection, BridgeDirectionChanged, null, null);
            Constraints = await SfBaseUtils.UpdateProperty(constraints, Constraints, ConstraintsChanged, null, null);
        }
        internal void InitHistory()
        {
            if (this.UndoRedo != null)
            {
                UndoRedo.InitHistory(this);
            }
        }
        public void AddHistoryEntry(HistoryEntryBase entry)
        {
            if (entry != null && this.UndoRedo != null && (this.constraints.HasFlag(DiagramConstraints.UndoRedo)))
            {
                if (entry.UndoObject is Node node && node.ID == "helper")
                {
                    return;
                }
                this.UndoRedo.AddHistoryEntry(entry, this);
                if (entry.Type != HistoryEntryType.StartGroup && entry.Type != HistoryEntryType.EndGroup)
                {
                    this.DiagramContent.HistoryChangeTrigger(entry, HistoryChangedAction.CustomAction);
                }
            }
        }

        internal void UpdateDiagramTemplates(DiagramTemplates diagramTemplates)
        {
            DiagramTemplates = diagramTemplates;
        }
        internal void UpdateDrawingObject(IDiagramObject drawingObject)
        {
            DrawingObject = drawingObject;
        }
        internal void UpdateTool(InteractionController interactionTool)
        {
            InteractionController = interactionTool;
        }
        internal async Task UpdateViewPort()
        {
            if (IsRendered)
            {
                DiagramSize patternSize = this.DiagramContent.GetPatternSize();
                
                DiagramRect bounds = await DomUtil.UpdateInnerLayerSize(InnerLayerList, null, null, null, this.Scroller.Transform, patternSize, this.SnapSettings.PathData, this.SnapSettings.DotsData, null, null, null, null, DiagramContentId);
                if (bounds != null)
                {
                    this.Scroller.SetViewPortSize(bounds.Width, bounds.Height);
                    this.Scroller.SetSize(null, false);
                    this.Scroller.UpdateScrollOffsets();
                }
            }
        }
        internal void SetDefaultHistory()
        {
            HistoryManager = new DiagramHistoryManager()
            {
                CanRedo = false,
                CanUndo = false,
                UndoStack = new List<HistoryEntryBase>(),
                RedoStack = new List<HistoryEntryBase>(),
                StackLimit = HistoryManager?.StackLimit ?? double.NaN
            };
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            ChildContent = null;
            if (DiagramAction.HasFlag(DiagramAction.Render))
            {
                Dictionary.Dispose();
            }

            if (NodeCollection != null)
            {
                for (int i = 0; i < NodeCollection.Count; i++)
                {
                    NodeCollection[i].Dispose();
                    NodeCollection[i] = null;
                }
                NodeCollection.Clear();
                NodeCollection = null;
            }
            if (ConnectorCollection != null)
            {
                for (int i = 0; i < ConnectorCollection.Count; i++)
                {
                    ConnectorCollection[i].Dispose();
                    ConnectorCollection[i] = null;
                }
                ConnectorCollection.Clear();
                ConnectorCollection = null;
            }
            if (DiagramContent != null)
            {
                DiagramContent.Dispose();
                DiagramContent = null;
            }
            if (PaletteInstance != null)
            {
                //paletteInstance.ComponentDispose();
                PaletteInstance = null;
            }
            if (BpmnDiagrams != null)
            {
                BpmnDiagrams.Dispose();
                BpmnDiagrams = null;
            }
            if (UndoRedo != null)
            {
                UndoRedo = null;
            }
            selfReference?.Dispose();
            if (DiagramCanvasScrollBounds != null)
            {
                DiagramCanvasScrollBounds.Dispose();
                DiagramCanvasScrollBounds = null;
            }
            if (MindMap != null)
            {
                MindMap = null;
            }
            if (Commands != null)
            {
                Commands.Clear();
                Commands = null;
            }
            if (BasicElements != null)
            {
                BasicElements.Clear();
                BasicElements = null;
            }
            if (DrawingObjectTool != null)
            {
                DrawingObjectTool = null;
            }
            if (NameTable != null)
            {
                NameTable.Clear();
                NameTable = null;
            }
            if (SpatialSearch != null)
            {
                SpatialSearch.Dispose();
                SpatialSearch = null;
            }
            if (Scroller != null)
            {
                Scroller.Dispose();
                Scroller = null;
            }
            if (EventHandler != null)
            {
                EventHandler.Dispose();
                EventHandler = null;
            }
            if (CommandHandler != null)
            {
                CommandHandler.Dispose();
                CommandHandler = null;
            }
            if (HistoryManager != null)
            {
                HistoryManager.Dispose();
                HistoryManager = null;
            }
            if (SnapSettings != null)
            {
                SnapSettings.Dispose();
                SnapSettings = null;
            }
            if (Snapping != null)
            {
                Snapping.Dispose();
                Snapping = null;
            }
            if (PageSettings != null)
            {
                PageSettings.Dispose();
                PageSettings = null;
            }
            if (Layout != null)
            {
                Layout.Dispose();
                Layout = null;
            }
            if (ScrollSettings != null)
            {
                ScrollSettings.Dispose();
                ScrollSettings = null;
            }
            if (HierarchicalTree != null)
            {
                //HierarchicalTree.Dispose();
                HierarchicalTree = null;
            }
            if (DataSourceSettings != null)
            {
                DataSourceSettings.Dispose();
                DataSourceSettings = null;
            }
            if (CommandManager != null)
            {
                CommandManager.Dispose();
                CommandManager = null;
            }
            if (SelectionSettings != null)
            {
                SelectionSettings.Dispose();
                SelectionSettings = null;
            }
            if (DataBindingModule != null)
            {
                DataBindingModule.Dispose();
                DataBindingModule = null;
            }
            if (InnerLayerList != null && InnerLayerList.Length > 0)
            {
                Array.Clear(InnerLayerList, 0, InnerLayerList.Length);
                InnerLayerList = null;
            }
            if (DiagramTemplates != null)
            {
                DiagramTemplates.Dispose();
                DiagramTemplates = null;
            }
            if (DrawingObject != null)
            {
                DrawingObject = null;
            }
            DiagramCursor = null;
            RotateCssClass = null;
            width = null;
            height = null;
            InnerLayerWidth = null;
            InnerLayerHeight = null;
            DiagramLayer = null;
            DiagramAdornerLayer = null;
            DiagramAdornerSvg = null;
            SelectorElement = null;
            DiagramAdorner = null;
            DiagramPorts = null;
            DiagramHtml = null;
            DiagramUserHandle = null;
            DiagramHtmlDiv = null;
            DiagramUserHandleDiv = null;
            DiagramBackground = null;
            GridLineSvg = null;
            SvgLayer = null;
            DiagramContentId = null;
            //GC.Collect(); GC.WaitForPendingFinalizers();
        }
    }
}
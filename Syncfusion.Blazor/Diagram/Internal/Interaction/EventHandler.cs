using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Tests")]

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class DiagramEventHandler
    {
        private const string RESIZE = "Resize";
        private const string RESIZEJSCALL = "resize";
        private const string NOT_ALLOWED = "not-allowed";
        private const string MOUSEDOWN = "mousedown";
        private const string MOUSEMOVE = "mousemove";
        private const string MOUSEUP = "mouseup";
        private const string TOUCHSTART = "touchstart";
        private const string TOUCHMOVE = "touchmove";
        private const string TOUCHEND = "touchend";
        private const string MOUSELEAVE = "mouseleave";
        private const string MOUSEWHEEL = "mousewheel";
        private const string SCROLL = "scroll";
        private const string KEYDOWN = "keydown";
        private const string KEYUP = "keyup";
        private const string ESCAPE = "Escape";
        private const string COPY = "copy";
        private const string PASTE = "paste";
        private const string CUT = "cut";
        private const string DELETE = "delete";
        private const string SELECTALL = "selectAll";
        private const string UNDO = "undo";
        private const string REDO = "redo";
        private const string NUDGEUP = "nudgeUp";
        private const string NUDGERIGHT = "nudgeRight";
        private const string NUDGEDOWN = "nudgeDown";
        private const string NUDGELEFT = "nudgeLeft";
        private const string STARTEDIT = "startEdit";
        private const string PRINT = "print";

        internal SfDiagramComponent Diagram;
        internal CommandHandler CommandHandler;
        internal ObjectFinder ObjectFinder = new ObjectFinder();
        private DiagramMouseEventArgs initialEventArgs;
        internal InteractionControllerBase Tool;
        private string handle;
        internal DiagramRect BoundingRect;
        internal DiagramMouseEventArgs EventArgs = new DiagramMouseEventArgs();
        private NodeBase hoverElement;
        private Node hoverNode;
        internal DiagramPoint CurrentPosition;
        internal DiagramPoint PreviousPosition;
        internal bool MouseDownPreventDefault;
        internal bool IsScrolling;
        private bool inAction;
        private bool preventScrollEvent;
        private string currentHandle;
        internal Actions CurrentAction = Actions.None;
        internal Actions PreviousAction = Actions.None;
        internal bool IsMouseDown;
        private bool isBlocked;
        private NodeBase lastObjectUnderMouse;
        private DiagramPoint currentPoint;
        private DiagramPoint previousPoint;
        private ObservableCollection<IDiagramObject> GetObjectsUnderMouse;
        private IDiagramObject getObjectUnderMouse;

        internal List<ITouches> TouchStartList = new List<ITouches>();
        internal List<ITouches> TouchMoveList = new List<ITouches>();

        private bool Blocked
        {
            get => isBlocked;
            set
            {
                this.isBlocked = value;
                Diagram.DiagramContent.SetCursor(value
                    ? NOT_ALLOWED
                    : Diagram.DiagramContent.GetCursor(this.CurrentAction, this.inAction, handle));
            }
        }
        internal Actions Action
        {
            get => CurrentAction;
            set
            {
                if (value != CurrentAction || handle != currentHandle)
                {
                    CurrentAction = value;
                    currentHandle = handle;
                    Diagram.DiagramContent.SetCursor(Diagram.DiagramContent.GetCursor(CurrentAction, inAction, handle));
                }
            }
        }

        internal DiagramEventHandler(SfDiagramComponent diagram, CommandHandler commandHandler)
        {
            Diagram = diagram;
            CommandHandler = commandHandler;
        }

        internal DiagramPoint GetMousePosition(JSMouseEventArgs e)
        {
            double offsetX = 0;
            double offsetY = 0;
            DiagramPoint mousePoint = new DiagramPoint();
            DiagramRect boundingRect = BoundingRect;
            if (Diagram.DiagramCanvasScrollBounds != null)
            {
                offsetX = e.ClientX + Diagram.DiagramCanvasScrollBounds.Left - boundingRect.Left;
                offsetY = e.ClientY + Diagram.DiagramCanvasScrollBounds.Top - boundingRect.Top;
            }
            offsetX /= Diagram.Scroller.Transform.Scale;
            offsetY /= Diagram.Scroller.Transform.Scale;
            offsetX -= Diagram.Scroller.Transform.TX;
            offsetY -= Diagram.Scroller.Transform.TY;
            mousePoint.X = offsetX;
            mousePoint.Y = offsetY;
            return mousePoint;
        }

        internal DiagramMouseEventArgs GetMouseEventArgs(DiagramPoint position, DiagramMouseEventArgs args, IDiagramObject source = null, double? padding = null)
        {
            args.Position = position;
            IDiagramObject obj;
            ObservableCollection<IDiagramObject> objects = null;
            if (source == null)
            {
                if (this.Action == Actions.Drag || this.Action == Actions.ConnectorSourceEnd || this.Action == Actions.ConnectorTargetEnd ||
                    this.Action == Actions.BezierSourceThumb || this.Action == Actions.BezierTargetThumb || this.Action == Actions.SegmentEnd || this.Action == Actions.OrthogonalThumb || this.Action == Actions.Rotate
                    || (this.Action.ToString().IndexOf(RESIZE, StringComparison.InvariantCulture) != -1))
                {
                    obj = this.Diagram.SelectionSettings;
                }
                else
                {
                    if (inAction)
                    {
                        objects = this.GetObjectsUnderMouse;
                        obj = this.getObjectUnderMouse;
                        EventArgs.ActualObject = obj;
                    }
                    else
                    {
                        objects = this.Diagram.DiagramContent.FindObjectsUnderMouse(this.CurrentPosition);
                        obj = this.Diagram.DiagramContent.FindObjectUnderMouse(objects, this.Action);
                    }
                }
            }
            else
            {
                objects = this.Diagram.DiagramContent.FindObjectsUnderMouse(this.CurrentPosition, source);
                obj = this.Diagram.DiagramContent.FindTargetObjectUnderMouse(objects, this.Action, args.Position, source);
            }
            ICommonElement wrapper = null;
            if (obj != null)
            {
                wrapper = this.Diagram.DiagramContent.FindElementUnderMouse(obj, this.CurrentPosition, padding);
                if (((wrapper != null && obj is Node && !DiagramUtil.CheckPort(obj, wrapper)) || wrapper == null ||
                     !(obj is Node)) && objects != null && objects.Count > 0 && source != null && (source is DiagramSelectionSettings))
                {
                    Connector currentConnector = (source as DiagramSelectionSettings).Connectors.Count > 0 ? (source as DiagramSelectionSettings).Connectors[0] : null;
                    int i;
                    for (i = objects.Count - 1; i >= 0; i--)
                    {
                        IDiagramObject nearNode = objects[i];
                        if ((nearNode is Node) && currentConnector != null && currentConnector.ConnectionPadding > 0)
                        {
                            obj = nearNode;
                            wrapper = this.Diagram.DiagramContent.FindElementUnderMouse(obj, this.CurrentPosition, padding) as DiagramContainer;
                            if (!(currentConnector.Constraints.HasFlag(ConnectorConstraints.ConnectToNearByNode)) && (currentConnector.Constraints.HasFlag(ConnectorConstraints.ConnectToNearByPort)))
                            {
                                if ((obj as Node) != null &&
                                    (obj as Node).Ports != null && (obj as Node).Ports.Count > 0 && DiagramUtil.CheckPort(obj, wrapper))
                                {
                                    break;
                                }
                                else
                                {
                                    obj = null;
                                }
                            }
                            if ((nearNode is Node) && currentConnector.ConnectionPadding > 0
                                && (nearNode as Node).Wrapper.OuterBounds.ContainsPoint(this.CurrentPosition) &&
                                (currentConnector.Constraints.HasFlag(ConnectorConstraints.ConnectToNearByNode)) &&
                                !(currentConnector.Constraints.HasFlag(ConnectorConstraints.ConnectToNearByPort)))
                            {
                                obj = nearNode;
                                wrapper = this.Diagram.DiagramContent.FindElementUnderMouse(obj, this.CurrentPosition, 0) as DiagramContainer;
                                break;
                            }
                        }
                    }
                }
            }
            if (source == null)
            {
                args.Element = obj;
                args.SourceWrapper = wrapper;
            }
            else
            {
                args.Target = obj;
                args.TargetWrapper = wrapper;
            }
            args.ActualObject = this.EventArgs.ActualObject;
            if (args.Element is DiagramSelectionSettings selectorObj && args.ActualObject == null &&
                ((selectorObj.Nodes.Count > 0 && this.Diagram.NameTable.ContainsKey(selectorObj.Nodes[0].ID))
                || (selectorObj.Connectors.Count > 0 && this.Diagram.NameTable.ContainsKey(selectorObj.Connectors[0].ID))))
            {

                args.ActualObject = selectorObj.Nodes.Count > 0 ? this.Diagram.NameTable[selectorObj.Nodes[0].ID]
                    : this.Diagram.NameTable[selectorObj.Connectors[0].ID];
            }
            args.StartTouches = this.TouchStartList;
            args.MoveTouches = this.TouchMoveList;
            return args;
        }


        internal void MouseDown(JSMouseEventArgs e)
        {
            if (ConstraintsUtil.CanUserInteract(Diagram) || ConstraintsUtil.CanZoomPan(Diagram))
            {
                handle = null;
                if (IsMouseOnScrollBar(e))
                {
                    IsScrolling = true;
                    MouseDownPreventDefault = true;
                    return;
                }
                if (this.Diagram.DiagramAction.HasFlag(DiagramAction.DrawingTool) && Diagram.DrawingObject is Connector && (Diagram.DrawingObject as Connector).Type == ConnectorSegmentType.Polyline &&
                    Diagram.SelectionSettings.Connectors.Count > 0)
                {
                    DiagramSelectionSettings helperObject = CommandHandler.RenderHelper();
                    Connector connector = this.Diagram.SelectionSettings.Connectors[0];
                    StraightSegment segment = new StraightSegment
                    {
                        Point = new DiagramPoint() { X = this.CurrentPosition.X, Y = this.CurrentPosition.Y },
                        Points = new List<DiagramPoint>()
                    };
                    segment.Points.Add(new DiagramPoint() { X = connector.SourcePoint.X, Y = connector.SourcePoint.Y });
                    segment.Points.Add(new DiagramPoint() { X = this.CurrentPosition.X, Y = this.CurrentPosition.Y });
                    connector.SegmentCollection[^1] = segment;
                    helperObject.Connectors[0].SegmentCollection[^1] = segment;
                }
                this.IsMouseDown = true;
                Diagram.DiagramAction |= DiagramAction.Interactions;
                Diagram.RealAction |= RealAction.PreventRefresh;
                this.CurrentPosition = GetMousePosition(e);
                ObservableCollection<IDiagramObject> objects = this.ObjectFinder.FindObjectsUnderMouse(this.CurrentPosition, this.Diagram, EventArgs, null);
                IDiagramObject obj = this.ObjectFinder.FindObjectUnderMouse(this.Diagram, objects, this.Action, this.EventArgs, this.CurrentPosition);
                ICommonElement sourceElement = null; IDiagramObject target = null;
                if (obj != null)
                {
                    sourceElement = Diagram.DiagramContent.FindElementUnderMouse(obj, CurrentPosition);
                    if (sourceElement != null)
                    {
                        target = CommandHandler.FindTarget(sourceElement, obj);
                    }
                }
                inAction = false;
                handle = Diagram.DiagramContent.FindHandle();
                Action = Diagram.DiagramContent.FindActionToBeDone(obj, sourceElement, target);
                if (e.CtrlKey && e.ShiftKey)
                {
                    this.Action = Actions.SegmentEnd;
                }
                else if ((e.CtrlKey || e.ShiftKey) && !ConstraintsUtil.CanZoomPan(Diagram))
                {
                    Action = Actions.Select;
                }
                Tool = Diagram.DiagramContent.GetTool(Action, handle);
                if (Tool == null)
                {
                    Action = Actions.Select;
                    Tool = Diagram.DiagramContent.GetTool(Action);
                }
                GetMouseEventArgs(CurrentPosition, EventArgs);
                if (e.CtrlKey || e.ShiftKey)
                {
                    Info info = (e.CtrlKey && e.ShiftKey) ? new Info { CtrlKey = e.CtrlKey, ShiftKey = e.ShiftKey } : new Info { CtrlKey = true };
                    EventArgs.Info = info;
                }
                EventArgs.Position = CurrentPosition;
                if (Tool is ZoomPanController)
                {
                    preventScrollEvent = true;
                }
                if ((Tool is PolygonDrawingController || Tool is PolylineDrawingController) && !this.Diagram.DiagramAction.HasFlag(DiagramAction.DrawingTool))
                {
                    this.CommandHandler.ClearSelection();
                    this.CommandHandler.HelperObject = null;
                }
                Tool.OnMouseDown(EventArgs);
                this.initialEventArgs = new DiagramMouseEventArgs { Element = EventArgs.Element, SourceWrapper = EventArgs.SourceWrapper, Position = CurrentPosition, Info = EventArgs.Info };
                this.inAction = false;
                if (e.Type == TOUCHSTART && e.Touches.Length > 1)
                {
                    this.TouchStartList = DomUtil.AddTouchPointer(this.TouchStartList, e.Touches);
                }
            }
        }
        internal void PaletteRenderHelper(bool isNode, IDiagramObject selectedNode)
        {
            // Work around , Due to selector issue
            DiagramSize size = new DiagramSize();
            DiagramSelectionSettings selector = this.Diagram.SelectionSettings;
            selector.Wrapper.Measure(size);
            selector.Wrapper.Arrange(selector.Wrapper.DesiredSize);
            DiagramSize previewSize = this.Diagram.PaletteInstance.GetPreviewSymbolSize(this.Diagram.PaletteInstance.SelectedSymbol as NodeBase);
            selector.Width = this.Diagram.PaletteInstance.SymbolDiagramPreviewSize.Width > 0 ? (double)this.Diagram.PaletteInstance.SymbolDiagramPreviewSize.Width : selector.Wrapper.ActualSize.Width.Value;
            selector.Height = this.Diagram.PaletteInstance.SymbolDiagramPreviewSize.Height > 0 ? (double)this.Diagram.PaletteInstance.SymbolDiagramPreviewSize.Height : selector.Wrapper.ActualSize.Height.Value;
            if (isNode)
            {
                selector.OffsetX = (double)(this.CurrentPosition.X + 5 + previewSize.Width * (selectedNode as Node).Pivot.X);
                selector.OffsetY = (double)(this.CurrentPosition.Y + 5 + (previewSize.Height * (selectedNode as Node).Pivot.Y));
            }
        }

        internal void MouseMove(JSMouseEventArgs e)
        {
            if (IsScrolling)
            {
                MouseDownPreventDefault = true;
                return;
            }
            //if (ConstraintsUtil.CanUserInteract(Diagram) || (ConstraintsUtil.CanZoomPan(Diagram) && !ConstraintsUtil.DefaultTool(Diagram)))
            if (ConstraintsUtil.CanUserInteract(Diagram) || ConstraintsUtil.CanZoomPan(Diagram))
            {
                EventArgs.Position = CurrentPosition = GetMousePosition(e);
                ObservableCollection<IDiagramObject> objects = GetObjectsUnderMouse = Diagram.DiagramContent.FindObjectsUnderMouse(CurrentPosition);
                IDiagramObject obj = getObjectUnderMouse = Diagram.DiagramContent.FindObjectUnderMouse(objects, this.Action);
                IDiagramObject target = null;
                handle = null;
                if (this.Diagram.PaletteInstance != null)
                {
                    if (this.Diagram.PaletteInstance.AllowDrag &&
                        this.Diagram.PaletteInstance.PaletteMouseDown &&
                        this.Diagram.PaletteInstance.SelectedSymbol != null && !this.IsMouseDown &&
                        this.Diagram.PaletteInstance.PreviewSymbol != null)
                    {
                        MouseDown(e);
                        IDiagramObject selectedNode;
                        bool isNode = this.Diagram.PaletteInstance.SelectedSymbol is Node;
                        if (!this.Diagram.RealAction.HasFlag(RealAction.SymbolDrag))
                            this.Diagram.RealAction |= RealAction.SymbolDrag;
                        DiagramPoint currentPosition = this.CurrentPosition;
                        DiagramSize previewSize =
                            this.Diagram.PaletteInstance.GetPreviewSymbolSize(
                                this.Diagram.PaletteInstance.SelectedSymbol as NodeBase);
                        if (isNode)
                        {
                            selectedNode = this.Diagram.PaletteInstance.SelectedSymbol as Node;

                            ((Node)selectedNode).OffsetX = (double)(currentPosition.X + 5 +
                                                                      previewSize.Width *
                                                                      ((Node)selectedNode).Pivot.X);
                            ((Node)selectedNode).OffsetY = (double)(currentPosition.Y + 5 +
                                                                      previewSize.Height *
                                                                      ((Node)selectedNode).Pivot.Y);
                        }
                        else
                        {
                            selectedNode = this.Diagram.PaletteInstance.SelectedSymbol as Connector;
                            ((Connector)selectedNode).SourcePoint.X = currentPosition.X;
                            ((Connector)selectedNode).SourcePoint.Y = currentPosition.Y;
                            ((Connector)selectedNode).TargetPoint.X =
                                (double)(currentPosition.X + previewSize.Width);
                            ((Connector)selectedNode).TargetPoint.Y =
                                (double)(currentPosition.Y + previewSize.Width);
                            ((Connector)selectedNode).Wrapper.Measure(new DiagramSize());
                            ((Connector)selectedNode).Wrapper.Arrange(((Connector)selectedNode)?.Wrapper
                                .DesiredSize);
                        }
                        this.CommandHandler.SelectObjects(new ObservableCollection<IDiagramObject> { selectedNode });
                        this.PaletteRenderHelper(isNode, selectedNode);
                    }
                }
                if (obj is NodeGroup)
                {
                    //when select port visibility as hover, for group children node. its not working. This is fix for that case.
                    obj = objects[0];
                }
                bool force = false;
                if (e.Type == TOUCHMOVE && e.Touches.Length > 1)
                {
                    this.TouchMoveList = DomUtil.AddTouchPointer(this.TouchMoveList, e.Touches);
                    force = this.Action == Actions.PinchZoom ? false : true;
                }
                if (!DiagramPoint.Equals(CurrentPosition, PreviousPosition) || inAction)
                {
                    if (!this.IsMouseDown || force)
                    {
                        this.EventArgs = new DiagramMouseEventArgs();
                        ICommonElement sourceElement = null;
                        if (obj != null)
                        {
                            sourceElement = Diagram.DiagramContent.FindElementUnderMouse(obj, CurrentPosition);
                            if (obj != this.hoverElement)
                            {
                                this.Diagram.DiagramContent.UpdatePortVisibility(this.hoverElement as Node, PortVisibility.Hover, true);
                                if (obj is Node)
                                {
                                    this.hoverNode = obj as Node;
                                }
                                bool canResetElement = true;
                                this.hoverElement = canResetElement ? obj as NodeBase : this.hoverElement;
                                if (canResetElement)
                                {
                                    //this.elementEnter(this.currentPosition, false);
                                }
                                else
                                {
                                    this.hoverElement = obj as NodeBase;
                                }
                            }
                            if (sourceElement != null)
                            {
                                target = CommandHandler.FindTarget(sourceElement, obj);
                            }
                        }
                        else
                        {
                            if (hoverElement != null)
                            {
                                this.Diagram.DiagramContent.UpdatePortVisibility(this.hoverElement as Node, PortVisibility.Hover, true);
                                hoverElement = null;
                            }
                        }
                        bool isNode = false;
                        handle = Diagram.DiagramContent.FindHandle();
                        Action = Diagram.DiagramContent.FindActionToBeDone(obj, sourceElement, target);
                        GetMouseEventArgs(CurrentPosition, EventArgs);
                        Tool = this.GetTool(this.Action, handle);
                        this.MouseEvents();
                        if (this.Tool is ConnectorDrawingController || this.Tool is PolylineDrawingController || this.Tool is PolygonDrawingController)
                        {
                            Tool.OnMouseMove(EventArgs);
                        }
                        else if (e.Touches != null && this.Tool is ZoomPanController)
                        {
                            Tool.OnMouseDown(EventArgs);
                        }
                        if (Action == Actions.PortDraw)
                        {
                            Node node = (Node)hoverElement;
                            if (node != null)
                            {
                                ICommonElement portWrapper = DiagramUtil.GetWrapper(node, node.Wrapper, ((PointPort)target).ID) as DiagramElement;
                                this.CommandHandler.DrawHighlighter(portWrapper);
                            }

                            Diagram.DiagramStateHasChanged();
                        }
                        else
                        {
                            Diagram.UpdateDrawingObject(Diagram.DrawingObjectTool);
                            if (!(Diagram.DrawingObjectTool is Connector))
                            {
                                this.CommandHandler.HighlighterElement = null;
                            }
                        }

                        if (!(this.hoverElement != null && (!(this.Tool is ZoomPanController))
                            && (obj is Node) &&
                            (this.Diagram.SelectionSettings.Nodes.Count == 0)))
                        {
                            isNode = true;
                        }
                        this.Diagram.DiagramContent.UpdatePortVisibility(this.hoverElement as Node, PortVisibility.Hover, isNode);

                        if (obj == null && this.hoverElement != null)
                        {
                            this.hoverElement = null;
                            Diagram.DiagramStateHasChanged();
                        }
                        if (this.Tool is PolygonDrawingController && (Diagram.DrawingObject as Node).Shape is BasicShape && ((Diagram.DrawingObject as Node).Shape as BasicShape).Shape == BasicShapeType.Polygon)
                        {
                            Tool.OnMouseMove(EventArgs);
                        }
                        force = false;
                    }
                    else
                    {
                        if (Diagram.DrawingObject != null || Action == Actions.PortDraw)
                        {
                            if (Diagram.DrawingObject is Connector connectorObj)
                            {
                                if (!this.Diagram.DiagramAction.HasFlag(DiagramAction.DrawingTool))
                                {
                                    if ((connectorObj.Type != ConnectorSegmentType.Polyline))
                                    {
                                        this.CommandHandler.HelperObject = null;
                                        this.CommandHandler.ClearSelection();
                                        this.MouseEvents();
                                        GetMouseEventArgs(CurrentPosition, EventArgs, EventArgs.Element);
                                        Tool.OnMouseMove(EventArgs);
                                    }
                                    this.Diagram.DiagramAction |= DiagramAction.DrawingTool;
                                    Action = Actions.ConnectorTargetEnd;
                                    GetMouseEventArgs(CurrentPosition, EventArgs);
                                    Tool = this.GetTool(this.Action, handle);
                                    Tool.OnMouseDown(EventArgs);
                                }
                            }
                            if (!(Tool is PolygonDrawingController) && (this.Diagram.DrawingObject is Node && this.Diagram.SelectionSettings.Nodes.Count == 0))
                            {
                                Tool.OnMouseDown(EventArgs);
                            }
                        }
                        EventArgs.Position = CurrentPosition;
                        if (Action == Actions.Drag && ActionsUtil.IsSelected(Diagram, EventArgs.Element) && EventArgs.Element is DiagramSelectionSettings)
                        {
                            GetMouseEventArgs(CurrentPosition, EventArgs);
                        }
                        this.MouseEvents();
                        if (e.CtrlKey || e.ShiftKey)
                        {
                            Info info = (e.CtrlKey && e.ShiftKey) ? new Info() { CtrlKey = e.CtrlKey, ShiftKey = e.ShiftKey } : new Info() { CtrlKey = true };
                            this.EventArgs.Info = info;
                        }
                        CheckAction(obj);
                        double padding = GetConnectorPadding(this.EventArgs);
                        GetMouseEventArgs(this.CurrentPosition, this.EventArgs, this.EventArgs.Element, padding);
                        this.inAction = true;
                        this.initialEventArgs = null;
                        if (this.EventArgs.Target != null)
                        {
                            this.hoverElement = this.EventArgs.Target as Node;
                        }
                        var isNode = !(this.EventArgs.Target is Node) || !(obj is Node);
                        if (this.Tool is ConnectionController)
                        {
                            this.Diagram.DiagramContent.UpdatePortVisibility(this.hoverElement is Node ? this.hoverElement as Node : this.hoverNode, PortVisibility.Connect | PortVisibility.Hover, isNode);
                        }
                        if (this.hoverElement is Node
                           && this.hoverNode is Node && this.hoverNode != null && this.hoverNode.ID != this.hoverElement.ID)
                        {
                            this.Diagram.DiagramContent.UpdatePortVisibility(this.hoverNode, PortVisibility.Connect | PortVisibility.Hover, true);
                        }
                        this.hoverElement = isNode ? null : obj as Node;
                        this.hoverNode = isNode ? null : obj as Node;

                        //if (Diagram.ScrollSettings.CanAutoScroll)
                        //{
                        //    //this.checkAutoScroll(e);
                        //}
                        //else
                        {
                            if (!Blocked && Tool != null)
                            {
                                EventArgs.Element = this.Diagram.SelectionSettings;
                                if ((!Diagram.RealAction.HasFlag(RealAction.PreventRefresh)) && Diagram.RealAction.HasFlag(RealAction.SymbolDrag) && Tool is DragController)
                                {
                                    Diagram.RealAction |= RealAction.PreventRefresh;
                                }
                                Tool.OnMouseMove(EventArgs);
                            }
                        }
                        this.PreviousPosition = this.CurrentPosition;
                    }
                }
            }
        }

        internal void MouseUp(JSMouseEventArgs e)
        {
            if (ConstraintsUtil.CanUserInteract(Diagram) || ConstraintsUtil.CanZoomPan(Diagram))
            {
                if (this.Diagram.DrawingObject != null)
                {
                    if (Tool is PolygonDrawingController || (Diagram.DrawingObject is Connector connectorObj && connectorObj.Type == ConnectorSegmentType.Polyline))
                    {
                        if (e.Detail == 2)
                        {
                            this.MouseDown(e);
                            if ((Diagram.DrawingObject is Connector diagramObject && diagramObject.Type == ConnectorSegmentType.Polyline))
                            {
                                _ = this.CommandHandler.AddObjectToDiagram();
                            }
                            else
                            {
                                ((((Tool as PolygonDrawingController).DrawingObject) as Node).Shape as BasicShape).Shape = BasicShapeType.Polygon;
                                _ = this.CommandHandler.AddObjectToDiagram(((Tool as PolygonDrawingController).DrawingObject) as Node);
                            }
                            this.Diagram.DiagramAction &= ~DiagramAction.DrawingTool;

                        }
                    }
                    else
                    {
                        this.Diagram.DiagramAction &= ~DiagramAction.DrawingTool;
                        _ = this.CommandHandler.AddObjectToDiagram();
                    }

                }
                if (IsScrolling)
                {
                    IsScrolling = false; return;
                }
                Diagram.RealAction &= ~RealAction.PreventRefresh;
                if (Tool != null && (!(Tool is PolylineDrawingController) && !(Diagram.DiagramAction.HasFlag(DiagramAction.DrawingTool))))
                {
                    this.EventArgs.Position = this.CurrentPosition;
                    double padding = GetConnectorPadding(this.EventArgs);
                    if (!this.inAction)
                    {
                        ObservableCollection<IDiagramObject> objects = this.Diagram.DiagramContent.FindObjectsUnderMouse(this.CurrentPosition);
                        IDiagramObject obj = this.Diagram.DiagramContent.FindObjectUnderMouse(objects, this.Action);
                        if (e.Detail == 2 && (Tool is DragController))
                        {
                            if (obj != null)
                            {
                                ICommonElement sourceElement = Diagram.DiagramContent.FindElementUnderMouse(obj, CurrentPosition);
                                string id = null;
                                if (sourceElement != null && sourceElement is TextElement)
                                {
                                    id = (sourceElement as TextElement).ID;
                                }
                                Diagram.StartTextEdit(obj, id);
                            }
                        }
                        if (this.Action == Actions.Drag)
                        {
                            this.Action = Actions.Select;
                            this.Diagram.DiagramContent.FindObjectUnderMouse(objects, this.Action);
                            bool isMultipleSelect = true;
                            if ((!e.CtrlKey && this.IsMouseDown
                                && (this.Diagram.SelectionSettings.Nodes.Count + this.Diagram.SelectionSettings.Connectors.Count) > 1))
                            {
                                isMultipleSelect = false;
                                this.CommandHandler.ClearSelection();
                            }
                            if ((obj != null && !ActionsUtil.IsSelected(this.Diagram, obj)) || (!isMultipleSelect))
                            {
                                this.CommandHandler.SelectObjects(new ObservableCollection<IDiagramObject> { obj });
                                this.CommandHandler.UpdateThumbConstaraints();
                                this.CommandHandler.RefreshDiagram();
                            }
                        }
                    }
                    inAction = false;
                    this.IsMouseDown = false;
                    this.CurrentPosition = this.GetMousePosition(e);
                    this.GetMouseEventArgs(this.CurrentPosition, this.EventArgs, this.EventArgs.Element, padding);
                    bool ctrlKey = e.CtrlKey;
                    if (ctrlKey || e.ShiftKey)
                    {
                        Info info = (ctrlKey && e.ShiftKey) ? new Info { CtrlKey = ctrlKey, ShiftKey = e.ShiftKey } :
                                new Info { CtrlKey = true };
                        this.EventArgs.Info = info;
                    }
                    this.EventArgs.ClickCount = e.Detail;
                    if (Tool is ZoomPanController)
                    {
                        preventScrollEvent = false;
                    }
                    else if (Tool is SelectionController)
                    {
                        Diagram.RealAction |= RealAction.RefreshSelectorLayer;
                    }
                    Tool.OnMouseUp(this.EventArgs);
                    if (Tool is SelectionController)
                    {
                        Diagram.RealAction &= ~RealAction.RefreshSelectorLayer;
                    }
                }
                Diagram.DiagramAction &= ~DiagramAction.Interactions;
                if (!(this.Tool is PolygonDrawingController) && !(Diagram.DrawingObject != null && Diagram.DrawingObject is Connector && (Diagram.DrawingObject as Connector).Type == ConnectorSegmentType.Polyline))
                {
                    Tool = null;
                    Blocked = false;
                }
                if (this.hoverElement != null)
                {
                    PortVisibility portVisibility = PortVisibility.Connect;

                    if (ActionsUtil.IsSelected(this.Diagram, this.hoverElement))
                    {
                        portVisibility |= PortVisibility.Hover;
                    }
                    this.Diagram.DiagramContent.UpdatePortVisibility(this.hoverElement as Node, portVisibility, true);
                    this.hoverElement = null;
                }
            }
            this.TouchStartList = null;
            this.TouchMoveList = null;
            if (!this.inAction && this.EventArgs != null)
            {
                ClickEventArgs args = new ClickEventArgs()
                {
                    Element = EventArgs.Element != null ? this.EventArgs.Element : this.Diagram,
                    Position = this.EventArgs.Position,
                    Count = e.Detail,
                    ActualObject = this.EventArgs.ActualObject,
                    Button = (e.Button == 0) ? MouseButtons.Left : (e.Button == 1) ? MouseButtons.Right : MouseButtons.Middle
                };
                CommandHandler.InvokeDiagramEvents(DiagramEvent.Click, args);
            }
            if (ConstraintsUtil.CanUpdateScroller(Action))
            {
                this.Diagram.Scroller.SetSize();
                this.Diagram.Scroller.UpdateScrollOffsets(null, null, true);
            }
        }

        internal void MouseLeave(JSMouseEventArgs e)
        {
            if (Diagram.DrawingObject != null)
            {
                if (this.Tool is PolygonDrawingController || this.Tool is ConnectionController)
                {
                    e.Detail = 2;
                }
                this.MouseUp(e);
            }
            Diagram.RealAction &= ~RealAction.PreventRefresh;
            //Define what has to happen on mouse leave
            if (this.Tool != null && this.inAction)
            {
                if (this.Tool is SelectionController)
                {
                    Diagram.RealAction |= RealAction.RefreshSelectorLayer;
                }
                this.Tool.OnMouseLeave(this.EventArgs);
            }
            Diagram.DiagramAction &= ~DiagramAction.Interactions;
            //this.Diagram.UpdatePage();
            Blocked = false;
            this.IsMouseDown = false;
            this.inAction = false;
            this.EventArgs = new DiagramMouseEventArgs();
            this.Tool = null;
            this.TouchStartList = null;
            this.TouchMoveList = null;
        }
        private static string GetLabelID(Dictionary<string, TextElementUtils> currnentMeasuredData, string nodeId)
        {
            string keyValue = string.Empty;
            foreach (string key in currnentMeasuredData.Keys)
            {
                if (key.Contains(nodeId, StringComparison.InvariantCulture))
                {
                    keyValue = key;
                }
            }
            return keyValue;
        }
        internal void ChangeLabelContent(Annotation annotation, string id, Dictionary<string, TextElementUtils> measuredTextData, TextChangeEventArgs args, IDiagramObject selectedNode, bool addAnnotation)
        {
            string[] annotationID = id.Split("_");
            if (annotationID[1].Contains(annotation.ID, StringComparison.InvariantCulture))
            {
                DiagramSize size = measuredTextData[id].Bounds.Clone();
                string content = measuredTextData[id].Content;
                this.Diagram.TextChangeEvent(annotation.Content, measuredTextData[id].Content, selectedNode, annotation, args);
                if (!args.Cancel)
                {
                    DomUtil.UpdateMeasuredTextData(measuredTextData, Dictionary.MeasureTextBounds, null, null);
                    if (!addAnnotation)
                    {
                        annotation.Content = content;
                        measuredTextData[id].Bounds = size;
                    }
                }
            }
        }
        internal void EndEdit(object obj = null)
        {
            bool refreshTextElement = false;
            TextChangeEventArgs args = new TextChangeEventArgs();
            Dictionary<string, object> dataObj = JsonSerializer.Deserialize<Dictionary<string, object>>(obj.ToString());
            Dictionary<string, TextElementUtils> measuredTextData = JsonSerializer.Deserialize<Dictionary<string, TextElementUtils>>(dataObj["Text"].ToString());
            Annotation annotation = null;
            Node selectedNode = null;
            Connector selectedConnector = null;
            TextElement annotationWrapper = null;
            string id;
            if (Diagram.SelectionSettings.Nodes.Count > 0)
            {
                selectedNode = Diagram.SelectionSettings.Nodes[0];
                id = GetLabelID(measuredTextData, Diagram.SelectionSettings.Nodes[0].ID);
                for (int i = 0; i < selectedNode.Annotations.Count; i++)
                {
                    annotation = selectedNode.Annotations[i];
                    this.ChangeLabelContent(annotation, id, measuredTextData, args, selectedNode, false);
                }
                if (selectedNode.Annotations.Count != 0)
                {

                    annotationWrapper = DiagramUtil.GetWrapper(selectedNode, selectedNode.Wrapper, annotation.ID) as TextElement;
                }
                else
                {
                    ObservableCollection<ICommonElement> nodeContainer = selectedNode.Wrapper.Children;
                    refreshTextElement = true;
                    string content = selectedNode is NodeGroup ? measuredTextData[((DiagramContainer)nodeContainer[0]).Children[^1].ID].Content : measuredTextData[nodeContainer[^1].ID].Content;
                    ShapeAnnotation newAnnotations = new ShapeAnnotation()
                    {
                        ID = id.Split("_")[1],
                        Content = content,
                    };
                    annotation = newAnnotations;
                    this.ChangeLabelContent(newAnnotations, id, measuredTextData, args, selectedNode, true);
                    if (selectedNode is NodeGroup)
                    {
                        ((DiagramContainer)nodeContainer[0]).Children.RemoveAt(((DiagramContainer)nodeContainer[0]).Children.Count - 1);
                    }
                    else
                    {
                        nodeContainer.RemoveAt(nodeContainer.Count - 1);
                    }
                    if (!args.Cancel)
                    {
                        selectedNode.Annotations.Add(newAnnotations);
                    }
                }
            }
            if (Diagram.SelectionSettings.Connectors.Count > 0)
            {
                selectedConnector = Diagram.SelectionSettings.Connectors[0];
                id = GetLabelID(measuredTextData, selectedConnector.ID);
                for (int i = 0; i < selectedConnector.Annotations.Count; i++)
                {
                    annotation = selectedConnector.Annotations[i];
                    this.ChangeLabelContent(annotation, id, measuredTextData, args, selectedConnector, false);
                }
                if (selectedConnector.Annotations.Count != 0)
                {
                    annotationWrapper = DiagramUtil.GetWrapper(selectedConnector, selectedConnector.Wrapper, annotation.ID) as TextElement;
                }
                else
                {
                    refreshTextElement = true;
                    PathAnnotation newAnnotation = new PathAnnotation()
                    {
                        ID = id.Split("_")[1],
                        Content = measuredTextData[selectedConnector.Wrapper.Children[^1].ID].Content,

                    };
                    annotation = newAnnotation;
                    this.ChangeLabelContent(annotation, id, measuredTextData, args, selectedConnector, true);
                    selectedConnector.Wrapper.Children.RemoveAt(selectedConnector.Wrapper.Children.Count - 1);
                    if (!args.Cancel)
                    {
                        selectedConnector.Annotations.Add(newAnnotation);
                    }
                }
            }
            if (!refreshTextElement)
            {
                annotationWrapper.Content = annotation.Content;
                annotationWrapper.RefreshTextElement();
                DiagramContainer container = (selectedNode != null) ? selectedNode.Wrapper : selectedConnector.Wrapper;
                container.Measure(new DiagramSize() { Width = container.Bounds.Width, Height = container.Bounds.Height });
                container.Arrange(container.DesiredSize, true);
            }
            Diagram.DiagramStateHasChanged();
            Diagram.DiagramAction &= ~DiagramAction.EditText;
            if ((selectedNode != null) && selectedNode.IsDirtNode)
            {
                for (int i = 0; i < selectedNode.Annotations.Count; i++)
                {
                    annotation = selectedNode.Annotations[i];
                    TextElement textWrapper = DiagramUtil.GetWrapper(selectedNode, selectedNode.Wrapper, annotation.ID) as TextElement;
                    textWrapper.InversedAlignment = false;
                }
                selectedNode.IsDirtNode = false;
            }
        }
        [JSInvokable]
        public void InvokeDiagramEvents(JSMouseEventArgs args, object obj = null)
        {
            if (obj != null)
            {
                this.EndEdit(obj);
            }
            if (Diagram.IsRendered)
            {
                currentPoint = new DiagramPoint() { X = args.OffsetX, Y = args.OffsetY };
                if (args.DiagramGetBoundingClientRect != null && ((args.Type == SCROLL && !preventScrollEvent) || args.Type != SCROLL))
                {
                    this.BoundingRect = args.DiagramGetBoundingClientRect;
                }

                if ((args.Type == TOUCHSTART || args.Type == TOUCHMOVE || args.Type == TOUCHEND) && args.Touches.Length > 0)
                {
                    args.ScreenX = args.Touches[0].ScreenX;
                    args.ScreenY = args.Touches[0].ScreenY;
                    args.ClientX = args.Touches[0].ClientX;
                    args.ClientY = args.Touches[0].ClientY;
                }

                switch (args.Type)
                {
                    case MOUSEDOWN:
                    case TOUCHSTART:
                        MouseDown(args);
                        break;
                    case MOUSEMOVE:
                    case TOUCHMOVE:
                        if (Tool is ZoomPanController)
                        {
                            Diagram.DiagramCanvasScrollBounds = args.DiagramCanvasScrollBounds;
                            Diagram.UpdateDrawingObject(null);
                        }
                        // if the args have IsPan value is true, we need to pan the diagram, don't consider the current and previous point
                        if (previousPoint != null && (currentPoint.X - previousPoint.X != 0 || currentPoint.Y - previousPoint.Y != 0 || (args.IsPan.HasValue && args.IsPan.Value)))
                            MouseMove(args);
                        previousPoint = currentPoint;
                        break;
                    case MOUSEUP:
                    case TOUCHEND:
                        MouseUp(args);
                        break;
                    case MOUSELEAVE:
                        MouseLeave(args);
                        break;
                    case MOUSEWHEEL:
                        MouseWheel(args);
                        break;
                    case SCROLL:
                        Scrolled(args);
                        break;
                    case KEYDOWN:
                        KeyDown(args);
                        break;
                    case KEYUP:
                        KeyUp(args);
                        break;
                    case RESIZEJSCALL:
                        _ = Diagram.UpdateViewPort();
                        break;
                }
            }
            if (args != null)
            {
                args.Dispose();
            }
        }

        internal void KeyDown(JSMouseEventArgs evt)
        {
            if (evt.Key != null || evt.Key.Equals(ESCAPE, StringComparison.Ordinal) || evt.KeyCode == 27)
            {
                string key = evt.Key;
                bool ctrlKey = IsMetaKey(evt);
                if (this.Diagram.CommandManager != null && this.Diagram.Commands.Count > 0)
                {
                    Dictionary<string, KeyboardCommand> commands = this.Diagram.Commands;
                    foreach (KeyValuePair<string, KeyboardCommand> entry in commands)
                    {
                        KeyboardCommand command = entry.Value;
                        KeyGesture keyGesture = command.Gesture;
                        if (command != null && (keyGesture.Modifiers != ModifierKeys.None || keyGesture.Key != Keys.None))
                        {
                            if ((key.ToLower(CultureInfo.InvariantCulture) == keyGesture.Key.ToString().ToLower(CultureInfo.InvariantCulture))
                                && (((keyGesture.Modifiers == ModifierKeys.None) && (!evt.AltKey) && (!evt.ShiftKey) && (!ctrlKey)) ||
                                    (keyGesture.Modifiers != ModifierKeys.None && (ctrlKey || evt.AltKey || evt.ShiftKey) &&
                                       (AltKeyPressed(keyGesture.Modifiers) && evt.AltKey) ||
                                        (ShiftKeyPressed(keyGesture.Modifiers) && evt.ShiftKey) ||
                                        (CtrlKeyPressed(keyGesture.Modifiers) && ctrlKey))))
                            {
                                DiagramObjectCollection<KeyboardCommand> newCommands = this.Diagram.CommandManager.Commands;
                                if (newCommands.Count > 0)
                                {
                                    int k = 0;
                                    foreach (KeyboardCommand newCommand in newCommands)
                                    {
                                        k++;
                                        if (entry.Key == newCommand.Name)
                                        {
                                            CommandKeyArgs args = new CommandKeyArgs() { Name = entry.Key, CanExecute = true, Gesture = (entry.Value as KeyboardCommand).Gesture };
                                            CommandManager commandManager = this.Diagram.CommandManager;
                                            commandManager.CanExecute.InvokeAsync(args);
                                            if (args.CanExecute)
                                            {
                                                commandManager.Execute.InvokeAsync(args);
                                            }
                                            break;
                                        }
                                        else if (entry.Key != newCommand.Name && k == newCommands.Count)
                                        {
                                            this.Execute(entry.Key);
                                        }
                                    }
                                }
                                else
                                {
                                    this.Execute(entry.Key);
                                }
                            }
                        }
                    }
                }
                if (evt.Key.Equals(ESCAPE, StringComparison.Ordinal) && CommandHandler.HelperObject != null)
                {
                    CommandHandler.HelperObject = null;
                    this.IsMouseDown = false;
                    this.inAction = false;
                    if (Diagram.PaletteInstance != null && Diagram.PaletteInstance.SelectedSymbol != null)
                        Diagram.PaletteInstance.SelectedSymbol = null;
                    Diagram.DiagramStateHasChanged();
                }
            }
            KeyEventArgs keyArgs = new KeyEventArgs() { Element = this.Diagram.SelectionSettings, Key = evt.Key, KeyCode = evt.KeyCode };
            GetKeyModifier(keyArgs, evt);
            this.CommandHandler.InvokeDiagramEvents(DiagramEvent.KeyDown, keyArgs);
        }
        internal void KeyUp(JSMouseEventArgs evt)
        {
            KeyEventArgs keyArgs = new KeyEventArgs() { Element = this.Diagram.SelectionSettings, Key = evt.Key, KeyCode = evt.KeyCode };
            GetKeyModifier(keyArgs, evt);
            this.CommandHandler.InvokeDiagramEvents(DiagramEvent.KeyUp, keyArgs);
        }
        internal void Execute(string entry)
        {
            switch (entry)
            {
                case COPY:
                    this.Diagram.Copy();
                    break;
                case PASTE:
                    this.Diagram.Paste();
                    break;
                case CUT:
                    this.Diagram.Cut();
                    break;
                case DELETE:
                    this.Diagram.Delete();
                    break;
                case SELECTALL:
                    this.Diagram.SelectAll();
                    break;
                case UNDO:
                    this.Diagram.Undo();
                    break;
                case REDO:
                    this.Diagram.Redo();
                    break;
                case NUDGEUP:
                    this.Diagram.NudgeCommand(Direction.Top);
                    break;
                case NUDGERIGHT:
                    this.Diagram.NudgeCommand(Direction.Right);
                    break;
                case NUDGEDOWN:
                    this.Diagram.NudgeCommand(Direction.Bottom);
                    break;
                case NUDGELEFT:
                    this.Diagram.NudgeCommand(Direction.Left);
                    break;
                case STARTEDIT:
                    this.Diagram.StartEditCommand();
                    break;
                case PRINT:
                    DiagramPrintSettings print = new DiagramPrintSettings();
                    print.PageWidth = (double)Diagram.PageSettings.Width;
                    print.PageHeight = (double)Diagram.PageSettings.Height;
                    _=this.Diagram.PrintAsync(print);
                    break;

            }
        }
        internal static bool AltKeyPressed(ModifierKeys keyModifiers)
        {
            if (keyModifiers == ModifierKeys.Alt)
                return true;
            return false;
        }
        internal static bool CtrlKeyPressed(ModifierKeys keyModifiers)
        {
            if (keyModifiers == ModifierKeys.Control)
                return true;
            return false;
        }
        internal static bool ShiftKeyPressed(ModifierKeys keyModifiers)
        {
            if (keyModifiers == ModifierKeys.Shift)
                return true;
            return false;
        }
        internal static void GetKeyModifier(KeyEventArgs args, JSMouseEventArgs evt)
        {
            args.KeyModifiers = ModifierKeys.None;
            if (evt.CtrlKey)
            {
                args.KeyModifiers |= ModifierKeys.Control;
            }
            if (evt.ShiftKey)
            {
                args.KeyModifiers |= ModifierKeys.Shift;
            }
            if (evt.AltKey)
            {
                args.KeyModifiers |= ModifierKeys.Alt;
            }
            if (evt.MetaKey)
            {
                args.KeyModifiers |= ModifierKeys.Meta;
            }
        }
        internal static bool IsMetaKey(JSMouseEventArgs evt)
        {
            return evt.MetaKey ? evt.MetaKey : evt.CtrlKey;
        }
        internal void Scrolled(JSMouseEventArgs args)
        {
            if (!preventScrollEvent && !DiagramRect.Equals(Diagram.DiagramCanvasScrollBounds, args.DiagramCanvasScrollBounds))
            {
                Diagram.DiagramCanvasScrollBounds = args.DiagramCanvasScrollBounds;
                Diagram.DiagramContent.UpdateScrollOffset();
            }
        }

        internal void MouseWheel(JSMouseEventArgs args)
        {
            bool up = args.WheelDelta > 0 || -40 * args.Detail > 0;
            DiagramPoint mousePosition = GetMousePosition(args);
            if (args.CtrlKey)
            {
                Diagram.Zoom(up ? 1.2 : 1 / 1.2, mousePosition);
            }
            else
            {
                double change = up ? 20 : -20;
                Diagram.ScrollActions |= ScrollActions.Interaction;
                if (args.ShiftKey)
                {
                    Diagram.Scroller.Zoom(1, change, 0, mousePosition);
                }
                else
                {
                    Diagram.Scroller.Zoom(1, 0, change, mousePosition);
                }
                if (Diagram.ScrollSettings.Parent == null)
                {
                    Diagram.ScrollActions &= ~ScrollActions.Interaction;
                }
            }
        }

        #region
        internal ICommonElement FindElementUnderMouse(IDiagramObject obj, DiagramPoint position, double? padding = null)
        {
            return ObjectFinder.FindElementUnderSelectedItem(obj, position, padding);
        }

        internal ObservableCollection<IDiagramObject> FindObjectsUnderMouse(DiagramPoint position, IDiagramObject source = null)
        {
            return this.ObjectFinder.FindObjectsUnderMouse(position, this.Diagram, this.EventArgs, source);
        }

        internal IDiagramObject FindObjectUnderMouse(ObservableCollection<IDiagramObject> objects, Actions action)
        {
            return this.ObjectFinder.FindObjectUnderMouse(this.Diagram, objects, action, this.EventArgs, this.CurrentPosition);
        }
        internal IDiagramObject FindTargetUnderMouse(
            ObservableCollection<IDiagramObject> objects, Actions action, DiagramPoint position, IDiagramObject source = null)
        {
            return this.ObjectFinder.FindObjectUnderMouse(this.Diagram, objects, action, this.EventArgs, position, source);
        }
        internal Actions FindActionToBeDone(IDiagramObject obj, ICommonElement wrapper, IDiagramObject target = null)
        {
            return ActionsUtil.FindToolToActivate(obj, wrapper, this.CurrentPosition, this.Diagram, target, this.TouchStartList, this.TouchMoveList);
        }
        #endregion

        private bool IsMouseOnScrollBar(JSMouseEventArgs evt)
        {
            double x = evt.ClientX - BoundingRect.X;
            double y = evt.ClientY - BoundingRect.Y;
            double height = Diagram.Scroller.ViewPortHeight;
            double width = Diagram.Scroller.ViewPortWidth;
            DiagramPoint topLeft; DiagramPoint topRight; DiagramPoint bottomLeft; DiagramPoint bottomRight; DiagramRect bounds;
            if (height < Diagram.DiagramCanvasScrollBounds.Height)
            {
                //default scrollbar width in browser is '17pixels'.
                topLeft = new DiagramPoint { X = (width - 17), Y = 0 };
                topRight = new DiagramPoint { X = width, Y = 0 };
                bottomLeft = new DiagramPoint { X = (width - 17), Y = height };
                bottomRight = new DiagramPoint { X = width, Y = height };
                bounds = DiagramRect.ToBounds(new List<DiagramPoint> { topLeft, topRight, bottomLeft, bottomRight });
                if (bounds.ContainsPoint(new DiagramPoint { X = x, Y = y }))
                {
                    return true;
                }
            }
            if (width < Diagram.DiagramCanvasScrollBounds.Width)
            {
                topLeft = new DiagramPoint { X = 0, Y = (height - 17) };
                topRight = new DiagramPoint { X = width, Y = (height - 17) };
                bottomLeft = new DiagramPoint { X = 0, Y = height };
                bottomRight = new DiagramPoint { X = width, Y = height };
                bounds = DiagramRect.ToBounds(new List<DiagramPoint> { topLeft, topRight, bottomLeft, bottomRight });
                if (bounds.ContainsPoint(new DiagramPoint { X = x, Y = y }))
                {
                    return true;
                }
            }
            return false;
        }
        internal string FindHandleEvent()
        {
            return ActionsUtil.FindUserHandle(this.CurrentPosition, Diagram);
        }

        internal InteractionControllerBase GetTool(Actions actions, string handleId)
        {
            if (handleId == null)
            {
                switch (actions)
                {
                    case Actions.Pan:
                        return new ZoomPanController(Diagram, false);
                    case Actions.PinchZoom:
                        return new ZoomPanController(Diagram, true);
                    case Actions.Select:
                        return new SelectionController(Diagram);
                    case Actions.Drag:
                        return new DragController(Diagram);
                    case Actions.ConnectorSourceEnd:
                    case Actions.ConnectorTargetEnd:
                    case Actions.BezierSourceThumb:
                    case Actions.BezierTargetThumb:
                        return new ConnectionController(Diagram, actions);
                    case Actions.ResizeEast:
                    case Actions.ResizeNorth:
                    case Actions.ResizeNorthEast:
                    case Actions.ResizeNorthWest:
                    case Actions.ResizeSouth:
                    case Actions.ResizeSouthEast:
                    case Actions.ResizeSouthWest:
                    case Actions.ResizeWest:
                        return new ResizeController(Diagram, actions);
                    case Actions.SegmentEnd:
                    case Actions.OrthogonalThumb:
                        return new ConnectorEditing(Diagram, actions);
                    case Actions.FixedUserHandle:
                        return new FixedUserHandleController(Diagram);
                    case Actions.Rotate:
                        return new RotationController(Diagram);
                    case Actions.Draw:
                        if (Diagram.DrawingObject is Node node)
                        {
                            if (node.Shape is BasicShape basicShape && basicShape.Shape == BasicShapeType.Polygon)
                            {
                                return new PolygonDrawingController(Diagram);
                            }
                            else
                            {
                                return new NodeDrawingController(Diagram);
                            }
                        }
                        else
                        {
                            if (((Connector)Diagram.DrawingObject).Type == ConnectorSegmentType.Polyline)
                            {
                                return new PolylineDrawingController(Diagram);
                            }
                            else
                            {
                                return new ConnectorDrawingController(Diagram, Actions.ConnectorSourceEnd);
                            }
                        }
                    case Actions.Hyperlink:
                        return new LabelController(Diagram);
                    case Actions.PortDraw:
                        return new ConnectorDrawingController(Diagram, Actions.ConnectorSourceEnd);
                }
            }
            return null;
        }

        internal string GetCursor(Actions actions)
        {
            DiagramSelectionSettings item = this.Diagram.SelectionSettings;
            double rotateAngle = item.RotationAngle;
            return ActionsUtil.GetCursor(actions, rotateAngle);
        }

        private void CheckAction(IDiagramObject obj)
        {
            if (ConstraintsUtil.CanMove(obj) && ConstraintsUtil.CanSelect(obj) && this.initialEventArgs != null &&
                this.initialEventArgs.Element != null && this.Action == Actions.Select)
            {
                if ((this.Diagram.SelectionSettings.Nodes.Count == 0 || this.Diagram.SelectionSettings.Nodes.Count > 0) && !string.IsNullOrEmpty((obj as NodeBase).ParentId))
                {
                    obj = this.Diagram.NameTable[(obj as NodeBase).ParentId] as NodeBase;
                    this.initialEventArgs.Element = obj;
                    this.initialEventArgs.SourceWrapper = (obj as NodeBase).Wrapper;
                }
                if (!ActionsUtil.IsSelected(this.Diagram, this.EventArgs.Element) && this.EventArgs.Element is DiagramSelectionSettings)
                {
                    GetMouseEventArgs(CurrentPosition, EventArgs);
                }

                if (!(obj is Connector connectorObj) ||
                    !(ActionsUtil.Contains(this.CurrentPosition, connectorObj.SourcePoint, connectorObj.HitPadding)
                      || ActionsUtil.Contains(this.CurrentPosition, connectorObj.TargetPoint, connectorObj.HitPadding)))
                {
                    Action = Actions.Drag;
                }

                Tool = GetTool(this.Action, null);
                Tool.OnMouseDown(this.initialEventArgs);
            }
        }

        private static double GetConnectorPadding(DiagramMouseEventArgs eventArgs)
        {
            double padding = 0;
            if (eventArgs.Element is DiagramSelectionSettings targetObject && targetObject.Connectors.Count > 0)
            {
                Connector selectedConnector = targetObject.Connectors[0];
                padding = selectedConnector.Constraints.HasFlag(ConnectorConstraints.ConnectToNearByPort) ? selectedConnector.ConnectionPadding : 0;
            }
            return padding;
        }

        private void MouseEvents()
        {
            ObservableCollection<IDiagramObject> target = this.Diagram.DiagramContent.FindObjectsUnderMouse(this.CurrentPosition);
            for (int i = 0; i < target.Count; i++)
            {
                if (this.EventArgs.ActualObject == target[i])
                {
                    target.RemoveAt(i);
                }
            }
            DiagramElementMouseEventArgs args;
            if (this.lastObjectUnderMouse != null && (this.EventArgs.ActualObject == null || (this.lastObjectUnderMouse != this.EventArgs.ActualObject)))
            {
                args = new DiagramElementMouseEventArgs()
                {
                    Targets = null,
                    Element = this.lastObjectUnderMouse,
                    ActualObject = null
                };
                CommandHandler.InvokeDiagramEvents(DiagramEvent.MouseLeave, args);
                this.lastObjectUnderMouse = null;
            }
            args = new DiagramElementMouseEventArgs()
            {
                Targets = target,
                Element = (this.EventArgs.Element == this.EventArgs.ActualObject) ? null : this.EventArgs.Element,
                ActualObject = this.EventArgs.ActualObject
            };
            if (this.lastObjectUnderMouse == null && this.EventArgs.Element != null ||
                (this.lastObjectUnderMouse != this.EventArgs.ActualObject))
            {
                this.lastObjectUnderMouse = this.EventArgs.ActualObject as NodeBase;
                if (this.EventArgs.ActualObject != null)
                {
                    this.CommandHandler.InvokeDiagramEvents(DiagramEvent.MouseEnter, args);
                }
            }
            if (this.EventArgs.ActualObject != null)
            {
                this.CommandHandler.InvokeDiagramEvents(DiagramEvent.MouseHover, args);
            }
        }

        internal void Dispose()
        {
            if (Diagram != null)
            {
                Diagram = null;
            }
            if (CommandHandler != null)
            {
                CommandHandler = null;
            }

            if (ObjectFinder != null)
            {
                ObjectFinder = null;
            }

            if (initialEventArgs != null)
            {
                initialEventArgs.Dispose();
                initialEventArgs = null;
            }
            if (Tool != null)
            {
                Tool.Dispose();
                Tool = null;
            }
            handle = null;

            if (BoundingRect != null)
            {
                BoundingRect.Dispose();
                BoundingRect = null;
            }

            if (EventArgs != null)
            {
                EventArgs.Dispose();
                EventArgs = null;
            }

            if (hoverElement != null)
            {
                hoverElement.Dispose();
                hoverElement = null;
            }
            if (hoverNode != null)
            {
                hoverNode.Dispose();
                hoverNode = null;
            }

            if (CurrentPosition != null)
            {
                CurrentPosition.Dispose();
                CurrentPosition = null;
            }
            if (PreviousPosition != null)
            {
                PreviousPosition.Dispose();
                PreviousPosition = null;
            }
            currentHandle = null;
            if (lastObjectUnderMouse != null)
            {
                lastObjectUnderMouse.Dispose();
                lastObjectUnderMouse = null;
            }
            if (currentPoint != null)
            {
                currentPoint.Dispose();
                currentPoint = null;
            }

            if (previousPoint != null)
            {
                previousPoint.Dispose();
                previousPoint = null;
            }
            if (GetObjectsUnderMouse != null)
            {
                GetObjectsUnderMouse.Clear();
                GetObjectsUnderMouse = null;
            }

            if (getObjectUnderMouse != null)
            {
                getObjectUnderMouse = null;
            }
        }
    }


    internal class ObjectFinder
    {
        internal IDiagramObject FindObjectUnderMouse(
        SfDiagramComponent diagram, ObservableCollection<IDiagramObject> objects, Actions action,
        DiagramMouseEventArgs eventArg, DiagramPoint position = null, IDiagramObject source = null)
        {
            IDiagramObject actualTarget = null;
            if (objects != null && objects.Count > 0)
            {
                if (source != null && source is DiagramSelectionSettings)
                {
                    if ((source as DiagramSelectionSettings).Nodes.Count + (source as DiagramSelectionSettings).Connectors.Count == 1)
                    {
                        source = (source as DiagramSelectionSettings).Nodes.Count == 1 ? (source as DiagramSelectionSettings).Nodes[0] as IDiagramObject : (source as DiagramSelectionSettings).Connectors[0] as IDiagramObject;
                    }
                }

                PointPort inPort;
                if ((action == Actions.ConnectorSourceEnd && source != null) || (action == Actions.PortDraw))
                {
                    Connector connector = diagram.SelectionSettings.Connectors.Count > 0 ? diagram.SelectionSettings.Connectors[0] : null;
                    for (int i = objects.Count - 1; i >= 0; i--)
                    {
                        PointPort outPort = DiagramUtil.GetInOutConnectPorts(objects[i] as Node, false);
                        inPort = DiagramUtil.GetInOutConnectPorts(objects[i] as Node, true);
                        if (action != Actions.PortDraw && objects[i] is Node && (ConstraintsUtil.CanOutConnect(objects[i] as Node) || (outPort != null && ConstraintsUtil.CanPortOutConnect(outPort)) || ConstraintsUtil.CanInConnect(objects[i] as Node) || (inPort != null && ConstraintsUtil.CanPortInConnect(inPort))))
                        {
                            actualTarget = objects[i];
                            if (connector != null)
                            {
                                actualTarget = IsTarget(actualTarget);
                            }
                            eventArg.ActualObject = actualTarget as Node;
                            return actualTarget;
                        }
                        else if (action == Actions.PortDraw && objects[i] is Node)
                        {
                            actualTarget = objects[i];
                            return actualTarget;
                        }
                    }
                }
                else if ((action == Actions.ConnectorTargetEnd && source != null))
                {
                    for (int i = objects.Count - 1; i >= 0; i--)
                    {
                        inPort = DiagramUtil.GetInOutConnectPorts(objects[i] as Node, true);
                        if (objects[i] is Node && (ConstraintsUtil.CanInConnect(objects[i] as Node) || (inPort != null && ConstraintsUtil.CanPortInConnect(inPort))))
                        {
                            actualTarget = objects[i];
                            actualTarget = IsTarget(actualTarget as Node);
                            eventArg.ActualObject = actualTarget as Node;
                            return actualTarget;
                        }
                    }
                }
                else if (source != null && (action == Actions.Drag || (diagram.EventHandler.Tool is DragController)))
                {
                    int index = 0;
                    for (int i = 0; i < objects.Count; i++)
                    {
                        IDiagramObject temp = objects[i];
                        if (source != temp && (temp is Connector ||
                            position == null || ((NodeBase)temp).Wrapper.Bounds.ContainsPoint(position)))
                        {
                            if (ConstraintsUtil.CanAllowDrop(temp as NodeBase))
                            {
                                if (actualTarget == null)
                                {
                                    actualTarget = temp;
                                    index = ((NodeBase)actualTarget).ZIndex;
                                }
                                else
                                {
                                    actualTarget = index >= ((NodeBase)temp).ZIndex ? actualTarget : temp;
                                    index = Math.Max(index, ((NodeBase)temp).ZIndex);
                                }
                            }
                        }
                    }
                    if (actualTarget != null)
                    {
                        eventArg.ActualObject = actualTarget as Node;
                    }
                    return actualTarget;
                }
                else if ((action == Actions.Select || action == Actions.Pan) && diagram.EventHandler.Tool != null)
                {
                    for (int i = objects.Count - 1; i >= 0; i--)
                    {
                        if (objects[i] is Connector)
                        {
                            if (i > 0 && objects[i - 1] is Node nodeObject && nodeObject.Ports.Count > 0)
                            {
                                ICommonElement portElement = this.FindTargetElement(nodeObject.Wrapper, position, null);
                                if (portElement != null && (portElement.ID.Contains("_icon_content_shape", StringComparison.InvariantCulture) || portElement.ID.Contains("_icon_content_rect", StringComparison.InvariantCulture)))
                                {
                                    return nodeObject;
                                }
                                for (int j = 0; j < nodeObject.Ports.Count; j++)
                                {
                                    PointPort port = nodeObject.Ports[j];
                                    if (portElement != null && portElement.ID == (nodeObject.ID + "_" + port.ID) && port.Constraints.HasFlag(PortConstraints.Draw))
                                    {
                                        return nodeObject;
                                    }
                                }
                            }
                        }
                    }
                    actualTarget = objects[^1];
                    eventArg.ActualObject = actualTarget;
                    //if (!diagram.EventHandler.itemClick(actualTarget, true))
                    {
                        if (!string.IsNullOrEmpty(((NodeBase)actualTarget).ParentId))
                        {
                            NodeBase obj = actualTarget as NodeBase;
                            bool selected = ActionsUtil.IsSelected(diagram, obj);
                            while (obj != null)
                            {
                                if (ActionsUtil.IsSelected(diagram, obj) && !selected)
                                {
                                    break;
                                }
                                actualTarget = obj;
                                obj = string.IsNullOrEmpty(obj.ParentId) ? null : diagram.NameTable[obj.ParentId] as NodeBase;
                            }
                        }
                    }
                }
                else
                {
                    actualTarget = objects[^1];
                    if (eventArg != null && actualTarget != null)
                    {
                        eventArg.ActualObject = actualTarget as Node;
                    }
                }
            }
            return actualTarget;
        }

        internal DiagramObjectCollection<IDiagramObject> FindObjectsUnderMouse(DiagramPoint pt, SfDiagramComponent diagram, DiagramMouseEventArgs eventArgs, IDiagramObject source = null)
        {
            // finds the collection of the object that is under the mouse;
            DiagramObjectCollection<IDiagramObject> actualTarget = new DiagramObjectCollection<IDiagramObject>();
            if (source is DiagramSelectionSettings _)
            {
                if (((DiagramSelectionSettings)source).Nodes.Count + ((DiagramSelectionSettings)source).Connectors.Count == 1)
                {
                    source = ((DiagramSelectionSettings)source).Nodes.Count > 0 ? ((DiagramSelectionSettings)source).Nodes[0] : ((DiagramSelectionSettings)source).Connectors[0] as IDiagramObject;
                    if (source is NodeGroup groupObj && groupObj.Children != null && groupObj.Children.Length == 0)
                    {
                        eventArgs.ActualObject = groupObj;
                    }
                }
            }

            double endPadding = (source != null && (source is Connector) &&
                                 ((source as Connector).Constraints.HasFlag(ConnectorConstraints.ConnectToNearByNode) ||
                                  (source as Connector).Constraints.HasFlag(ConnectorConstraints.ConnectToNearByPort))) ? (source as Connector).ConnectionPadding : 0;
            ObservableCollection<IDiagramObject> objArray = diagram.SpatialSearch.FindObjects(new DiagramRect(pt.X - 50 - endPadding, pt.Y - 50 - endPadding, 100 + endPadding, 100 + endPadding));
            DiagramObjectCollection<IDiagramObject> layerTarget = new DiagramObjectCollection<IDiagramObject>();
            for (int i = 0; i < objArray.Count; i++)
            {
                IDiagramObject obj = objArray[i];
                DiagramPoint point = pt;
                DiagramRect bounds = (obj as NodeBase).Wrapper.OuterBounds;
                if (((obj != source) || diagram.DrawingObject is Connector) && ((obj is Connector) ? obj != diagram.DrawingObject : true) && (obj as NodeBase).Wrapper.Visible)
                {
                    bool pointInBounds = (!(obj is Node) || (obj as Node).RotationAngle == 0) && bounds.ContainsPoint(point, endPadding);
                    //layerTarget = new DiagramObjectCollection<IDiagramObject>();
                    if (obj is Node && (obj as Node).RotationAngle != 0)
                    {
                        DiagramContainer container = (obj as Node).Wrapper;
                        _ = BaseUtil.CornersPointsBeforeRotation(container);
                        for (int j = 0; j < container.Children.Count; j++)
                        {
                            ICommonElement child = container.Children[j];
                            Matrix matrix = Matrix.IdentityMatrix();
                            Matrix.RotateMatrix(matrix, -(child.RotationAngle + child.ParentTransform), child.OffsetX, child.OffsetY);
                            point = Matrix.TransformPointByMatrix(matrix, pt);
                            if (BaseUtil.CornersPointsBeforeRotation(child as DiagramElement).ContainsPoint(point, endPadding)) { pointInBounds = true; }
                        }
                    }
                    if (source == null || (!ActionsUtil.IsSelected(diagram, obj)))
                    {
                        if (ConstraintsUtil.CanEnablePointerEvents(obj as NodeBase))
                        {
                            if ((obj is Connector) ? DiagramUtil.IsPointOverConnector(obj as Connector, pt) : pointInBounds)
                            {
                                double padding = (obj is Connector) ? (obj as Connector).HitPadding : 0; ICommonElement element;
                                element = this.FindElementUnderMouse(obj as IDiagramObject, pt, endPadding != 0 ? endPadding : padding);
                                if (element != null && !((NodeBase)obj).ID.Contains("helper", StringComparison.InvariantCulture))
                                {
                                    DiagramUtil.InsertObjectByZIndex(obj, layerTarget);
                                }
                            }
                        }
                    }
                }
            }
            actualTarget = actualTarget.Concat(layerTarget);
            for (int i = 0; i < actualTarget.Count; i++)
            {
                if (!string.IsNullOrEmpty((actualTarget[i] as NodeBase).ParentId))
                {
                    NodeBase parentObj = diagram.NameTable[(actualTarget[i] as NodeBase).ParentId] as NodeBase;
                    if (parentObj != null)
                    {
                        ICommonElement portElement = this.FindElementUnderMouse(parentObj, pt);
                        if (portElement != null && parentObj is Node)
                        {
                            for (int j = 0; j < (parentObj as Node).Ports.Count; j++)
                            {
                                if (portElement.ID.Equals('_' + (parentObj as Node).Ports[j].ID + '$', StringComparison.Ordinal))
                                {
                                    Port port = (parentObj as Node).Ports[j];
                                    if (ConstraintsUtil.CanDraw(port))
                                    {
                                        return actualTarget;
                                    }
                                }
                            }
                        }
                    }
                    while (parentObj != null)
                    {
                        int index = actualTarget.IndexOf(parentObj);
                        if (index != -1)
                        {
                            actualTarget.RemoveAt(index);
                        }
                        else
                        {
                            break;
                        }
                        if (string.IsNullOrEmpty(parentObj.ParentId)) break;
                        parentObj = diagram.NameTable[parentObj.ParentId] as NodeBase;
                    }
                }
            }

            return actualTarget;
        }
        internal ICommonElement FindElementUnderSelectedItem(IDiagramObject obj, DiagramPoint position, double? padding = null)
        {
            if (obj is DiagramSelectionSettings selectorObj)
            {
                if (selectorObj.Nodes.Count == 1 && (selectorObj.Connectors == null || selectorObj.Connectors.Count == 0))
                {
                    return this.FindElementUnderMouse(selectorObj.Nodes[0], position);
                }
                else if ((selectorObj.Nodes == null || selectorObj.Nodes.Count > 0) && selectorObj.Connectors.Count == 1)
                {
                    return this.FindElementUnderMouse(selectorObj.Connectors[0], position);
                }
            }
            else
            {
                return this.FindElementUnderMouse(obj, position, padding);
            }
            return null;
        }

        private ICommonElement FindElementUnderMouse(IDiagramObject obj, DiagramPoint position, double? padding = null)
        {
            return this.FindTargetElement(((NodeBase)obj).Wrapper, position, padding);
        }

        internal ICommonElement FindTargetElement(DiagramContainer container, DiagramPoint position, double? padding = null)
        {
            for (int i = container.Children.Count - 1; i >= 0; i--)
            {
                ICommonElement element = container.Children[i];
                if (element != null && element.OuterBounds.ContainsPoint(position, padding ?? 0))
                {
                    if (element is DiagramContainer)
                    {
                        ICommonElement target = this.FindTargetElement(element as DiagramContainer, position);
                        if (target != null)
                        {
                            return target;
                        }
                    }
                    if (element.Bounds.ContainsPoint(position, padding ?? 0))
                    {
                        return element;
                    }
                }
            }

            if (container.Bounds.ContainsPoint(position, padding ?? 0) && container.Style.Fill != "none")
            {
                return container;
            }
            return null;
        }

        private static IDiagramObject IsTarget(IDiagramObject actualTarget)
        {
            //Connector connector = diagram.SelectedItems.Connectors[0];
            //Node node = action == Actions.ConnectorSourceEnd ? diagram.NameTable[connector.TargetID] as Node : diagram.NameTable[connector.SourceID] as Node;
            return actualTarget;
        }

    }
    internal class Info
    {
        internal bool? CtrlKey;
        internal bool? ShiftKey;
        internal void Dispose()
        {
            CtrlKey = null;
            ShiftKey = null;
        }
    }
    /// <summary>
    /// Notifies the mouse events, keyboard and scrolling action in the diagram.
    /// </summary>
    internal class JSMouseEventArgs
    {
        /// <summary>
        ///  Represents the scroller related actions to conform whether the scroller is updated or not in UI level. 
        /// </summary>
        [JsonPropertyName("isPan")]
        public bool? IsPan { get; set; }
        /// <summary>
        /// Returns whether the alt key has been pressed or not. 
        /// </summary>
        [JsonPropertyName("altKey")]
        public bool AltKey { get; set; }
        /// <summary>
        /// Represents the horizontal coordinate of a touch point relative to the  viewport. 
        /// </summary>
        [JsonPropertyName("clientX")]
        public double ClientX { get; set; }
        /// <summary>
        /// Represents the vertical coordinate of a touch point relative to the viewport.
        /// </summary>
        [JsonPropertyName("clientY")]
        public double ClientY { get; set; }
        /// <summary>
        /// Represents the x coordinate in the diagram where the mouse events happened.
        /// </summary>
        [JsonPropertyName("offsetX")]
        public double OffsetX { get; set; }
        /// <summary>
        /// Represents the y coordinate in the diagram where the mouse events happened.
        /// </summary>
        [JsonPropertyName("offsetY")]
        public double OffsetY { get; set; }
        /// <summary>
        /// Returns whether the ctrl key has been pressed or not. 
        /// </summary>
        [JsonPropertyName("ctrlKey")]
        public bool CtrlKey { get; set; }
        /// <summary>
        /// Represents the title of the symbol group. By default, it is empty.
        /// </summary>
        [JsonPropertyName("detail")]
        public int Detail { get; set; }
        /// <summary>
        /// Represents whether the mac meta key is pressed inside the diagram or not.
        /// </summary>
        [JsonPropertyName("metaKey")]
        public bool MetaKey { get; set; }
        /// <summary>
        ///  Represents the horizontal distance between the left side of the screen. 
        /// </summary>
        [JsonPropertyName("screenX")]
        public double ScreenX { get; set; }
        /// <summary>
        /// Represents the vertical distance between the top side of the screen. 
        /// </summary>
        [JsonPropertyName("screenY")]
        public double ScreenY { get; set; }
        /// <summary>
        /// Returns whether the shift key has been pressed or not. 
        /// </summary>
        [JsonPropertyName("shiftKey")]
        public bool ShiftKey { get; set; }
        /// <summary>
        /// Represents the type of action like mouse movement, down or up in the diagram.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
        /// <summary>
        /// Return the mouse scroll bar wheel data.
        /// </summary>
        [JsonPropertyName("wheelDelta")]
        public double WheelDelta { get; set; }
        /// <summary>
        /// Represents the position of the diagram where it is rendered.
        /// </summary>
        [JsonPropertyName("diagramGetBoundingClientRect")]
        public DiagramRect DiagramGetBoundingClientRect { get; set; }
        /// <summary>
        /// Represents the scroller’s left, top, width and height of the diagram. 
        /// </summary>
        [JsonPropertyName("diagramCanvasScrollBounds")]
        public DiagramRect DiagramCanvasScrollBounds { get; set; }
        /// <summary>
        /// Represents the key which is pressed inside the diagram.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
        /// <summary>
        /// Represents the key code which is pressed inside the diagram.
        /// </summary>
        [JsonPropertyName("keyCode")]
        public int KeyCode { get; set; }
        /// <summary>
        /// Represents the button that has been clicked in the diagram.
        /// </summary>
        [JsonPropertyName("button")]
        public int Button { get; set; }
        /// <summary>
        /// Represents the collection of touches while is tap inside the diagram.
        /// </summary>
        [JsonPropertyName("touches")]
        public ITouches[] Touches { get; set; }

        internal void Dispose()
        {
            if (IsPan != null)
            {
                IsPan = null;
            }

            if (DiagramGetBoundingClientRect != null)
            {
                DiagramGetBoundingClientRect.Dispose();
                DiagramGetBoundingClientRect = null;
            }

            if (DiagramCanvasScrollBounds != null)
            {
                DiagramCanvasScrollBounds = null;
            }

            AltKey = false;
            CtrlKey = false;
            MetaKey = false;
            ShiftKey = false;

            ClientX = 0;
            ClientY = 0;
            OffsetX = 0;
            OffsetY = 0;
            ScreenX = 0;
            ScreenY = 0;
            WheelDelta = 0;

            Detail = 0;
            KeyCode = 0;
            Button = 0;


            Type = null;
            Key = null;

        }
    }

    internal class ITouches
    {
        /// <summary>
        ///  Represents the horizontal distance between the left side of the diagram page. 
        /// </summary>
        [JsonPropertyName("pageX")]
        public double PageX { get; set; }
        /// <summary>
        /// Represents the vertical distance between the left side of the diagram page.
        /// </summary>
        [JsonPropertyName("pageY")]
        public double PageY { get; set; }
        /// <summary>
        /// Represents the horizontal distance between the left side of the screen. 
        /// </summary>
        [JsonPropertyName("screenX")]
        public double ScreenX { get; set; }
        /// <summary>
        /// Represents the vertical distance between the top side of the screen. 
        /// </summary>
        [JsonPropertyName("screenY")]
        public double ScreenY { get; set; }
        /// <summary>
        /// Represents the horizontal coordinate of a touch point relative to the  viewport. 
        /// </summary>
        [JsonPropertyName("clientX")]
        public double ClientX { get; set; }
        /// <summary>
        /// Represents the vertical coordinate of a touch point relative to the viewport.
        /// </summary>
        [JsonPropertyName("clientY")]
        public double ClientY { get; set; }
    }

}

using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the class for all common tools.
    /// </summary>
    public abstract class ICommonController
    {
        internal bool blocked;
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the diagram element and a mouse button is pressed.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public virtual void OnMouseDown(DiagramMouseEventArgs args) { }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the diagram element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public virtual bool OnMouseMove(DiagramMouseEventArgs args)
        {
            return !blocked;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the diagram element and a mouse button is released.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public virtual void OnMouseUp(DiagramMouseEventArgs args) { }
        internal virtual void EndAction() { }
        /// <summary>
        /// This method triggers when the mouse pointer leaves the diagram element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public virtual void OnMouseLeave(DiagramMouseEventArgs args) { }

        internal virtual void Dispose() { }
    }
    /// <summary>
    /// Represents the interactive tools.
    /// </summary>
    public class InteractionControllerBase : ICommonController
    {
        internal bool IsTooltipVisible;
        internal CommandHandler CommandHandler { get; set; }
        internal DiagramPoint currentPoint;
        /// <summary>
        /// Allows to decide whether an element in action or not.
        /// </summary>
        protected bool InAction { get; set; }
        /// <summary>
        /// Gets or sets the current position of the element.
        /// </summary>
        protected DiagramPoint CurrentPosition { get; set; }
        /// <summary>
        /// Gets or sets the previous position of the element.
        /// </summary>
        protected DiagramPoint PreviousPosition { get; set; }
        /// <summary>
        /// Gets or sets the starting point of the element.
        /// </summary>
        protected DiagramPoint StartPosition { get; set; }
        /// <summary>
        /// Gets or sets the current selected element.
        /// </summary>
        internal IDiagramObject CurrentElement { get; set; }
        /// <summary>
        /// Gets or sets the element on which mouse is clicked.
        /// </summary>
        internal IDiagramObject MouseDownElement { get; set; }
        internal DiagramSelectionSettings UndoElement { get; set; } = new DiagramSelectionSettings();
        internal DiagramSelectionSettings UndoParentElement { get; set; } = new DiagramSelectionSettings();
        internal Dictionary<string, IDiagramObject> ChildTable { get; set; } = new Dictionary<string, IDiagramObject>();
        /// <summary>
        /// Creates a new instance of the toolbase from the given toolbase.
        /// </summary>
        /// <param name="diagram">SfDiagramComponent</param>
        public InteractionControllerBase(SfDiagramComponent diagram)
        {
            if (diagram != null)
                CommandHandler = diagram.CommandHandler;
        }
        /// <summary>
        /// This method triggers when an action is going to start.
        /// </summary>
        /// <param name="currentElement"></param>
        internal void StartAction(IDiagramObject currentElement)
        {
            CurrentElement = currentElement;
            InAction = true;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the element and a mouse button is clicked.
        /// </summary>
        /// <param name="args"></param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                CurrentElement = args.Element;
                StartPosition = CurrentPosition = PreviousPosition = args.Position;
                StartAction(args.Element);
                IsTooltipVisible = true;
            }
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                CurrentPosition = args.Position;
            }
            return !blocked;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the element and a mouse button is released.  
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                CurrentPosition = args.Position;
                IsTooltipVisible = false;
                EndAction();
            }
        }

        internal override void EndAction()
        {
            if (!this.IsTooltipVisible)
            {
                this.CommandHandler.CloseTooltip();
            }
            CommandHandler = null;
            CurrentElement = null;
            CurrentPosition = null;
            blocked = false;
        }
        /// <summary>
        /// This method triggers when the mouse pointer leaves the element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            OnMouseUp(args);
        }

        internal override void Dispose()
        {
            if (CommandHandler != null)
            {
                CommandHandler = null;
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
            if (StartPosition != null)
            {
                StartPosition.Dispose();
                StartPosition = null;
            }
            if (CurrentElement != null)
            {
                CurrentElement = null;
            }
            if (MouseDownElement != null)
            {
                MouseDownElement = null;
            }
            if (UndoElement != null)
            {
                UndoElement.Dispose();
                UndoElement = null;
            }
            if (UndoParentElement != null)
            {
                UndoParentElement.Dispose();
                UndoParentElement = null;
            }
            if (ChildTable != null)
            {
                ChildTable.Clear();
                ChildTable = null;
            }
        }
        /// <summary>
        /// This method invokes when the size of the rect is updated.
        /// </summary>
        /// <param name="shape">Selector</param>
        /// <param name="startPoint">Point</param>
        /// <param name="endPoint">Point</param>
        /// <param name="corner">Actions</param>
        /// <param name="initialBounds">DiagramRect</param>
        /// <returns></returns>
        internal DiagramRect UpdateSize(DiagramSelectionSettings shape, DiagramPoint startPoint, DiagramPoint endPoint, Actions corner, DiagramRect initialBounds)
        {
            if (shape != null && initialBounds != null)
            {
                Snap horizontalSnap = new Snap { Snapped = false, Offset = 0, Left = false, Right = false };
                Snap verticalSnap = new Snap { Snapped = false, Offset = 0, Top = false, Bottom = false };
                double difX = this.CurrentPosition.X - this.StartPosition.X;
                double difY = this.CurrentPosition.Y - this.StartPosition.Y;
                bool snapEnabled = this.CommandHandler.SnappingInstance().CanSnap();
                double rotateAngle = shape.RotationAngle;
                Matrix matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, -rotateAngle, 0, 0);
                double deltaWidth = 0; double deltaHeight = 0;
                DiagramPoint diff;
                //bool isTextElement = shape is TextElement;
                double width = (double)shape.Width;// (isTextElement ? shape.Wrapper.ActualSize.Width : shape.Width);
                double height = (double)shape.Height;// (isTextElement ? shape.Wrapper.ActualSize.Height : shape.Height);
                switch (corner)
                {
                    case Actions.ResizeWest:
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY })); difX = diff.X; difY = diff.Y;
                        deltaHeight = 1;
                        difX = snapEnabled ? this.CommandHandler.SnappingInstance().SnapLeft(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds) :
                            difX;
                        deltaWidth = (initialBounds.Width - difX) / width; break;
                    case Actions.ResizeEast:
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY }));
                        difX = diff.X;
                        difY = diff.Y;
                        difX = snapEnabled ? this.CommandHandler.SnappingInstance().SnapRight(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds) :
                            difX;
                        deltaWidth = (initialBounds.Width + difX) / width;
                        deltaHeight = 1;
                        break;
                    case Actions.ResizeNorth:
                        deltaWidth = 1;
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY })); difX = diff.X; difY = diff.Y;
                        difY = snapEnabled ? this.CommandHandler.SnappingInstance().SnapTop(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds) :
                            difY;
                        deltaHeight = (initialBounds.Height - difY) / height; break;
                    case Actions.ResizeSouth:
                        deltaWidth = 1;
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY })); difX = diff.X; difY = diff.Y;
                        difY = snapEnabled ? this.CommandHandler.SnappingInstance().SnapBottom(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds) :
                            difY;
                        deltaHeight = (initialBounds.Height + difY) / height; break;
                    case Actions.ResizeNorthEast:
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY })); difX = diff.X; difY = diff.Y;
                        difX = snapEnabled ? this.CommandHandler.SnappingInstance().SnapRight(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds) :
                            difX;
                        difY = snapEnabled ? this.CommandHandler.SnappingInstance().SnapTop(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds) :
                            difY;
                        deltaWidth = (initialBounds.Width + difX) / width; deltaHeight = (initialBounds.Height - difY) / height;
                        break;
                    case Actions.ResizeNorthWest:
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY })); difX = diff.X; difY = diff.Y;
                        difY = !snapEnabled ? difY : this.CommandHandler.SnappingInstance().SnapTop(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds);
                        difX = !snapEnabled ? difX : this.CommandHandler.SnappingInstance().SnapLeft(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds);
                        deltaWidth = (initialBounds.Width - difX) / width; deltaHeight = (initialBounds.Height - difY) / height;
                        break;
                    case Actions.ResizeSouthEast:
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY })); difX = diff.X; difY = diff.Y;
                        difY = !snapEnabled ? difY : this.CommandHandler.SnappingInstance().SnapBottom(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds);
                        difX = !snapEnabled ? difX : this.CommandHandler.SnappingInstance().SnapRight(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings, endPoint == startPoint, initialBounds);
                        deltaHeight = (initialBounds.Height + difY) / height; deltaWidth = (initialBounds.Width + difX) / width;
                        break;
                    case Actions.ResizeSouthWest:
                        diff = Matrix.TransformPointByMatrix(matrix, (new DiagramPoint() { X = difX, Y = difY })); difX = diff.X; difY = diff.Y;
                        difY = snapEnabled ? this.CommandHandler.SnappingInstance().SnapBottom(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings,
                            endPoint == startPoint, initialBounds) : difY;
                        difX = snapEnabled ? this.CommandHandler.SnappingInstance().SnapLeft(
                            horizontalSnap, verticalSnap, difX, difY, shape as DiagramSelectionSettings,
                            endPoint == startPoint, initialBounds) : difX;
                        deltaWidth = (initialBounds.Width - difX) / width; deltaHeight = (initialBounds.Height + difY) / height; break;
                }
                return new DiagramRect() { Width = deltaWidth, Height = deltaHeight };
            }
            return null;
        }
    }
    /// <summary>
    /// Represents the class that helps to scale the selected objects. 
    /// </summary>
    public class ResizeController : InteractionControllerBase
    {
        /// <summary>
        /// Sets/Gets the previous mouse position
        /// </summary>
        private DiagramPoint prevPosition;

        private readonly Actions corner;

        private DiagramRect initialBounds = new DiagramRect();

        /// <summary>
        /// Creates a new instance of the ResizeTool from the given ResizeTool.
        /// </summary>
        /// <param name="diagram">SfDiagramComponent</param>
        /// <param name="corner">Actions</param>
        public ResizeController(SfDiagramComponent diagram, Actions corner) : base(diagram)
        {
            this.corner = corner;
        }
        /// <summary>
        /// This method triggers when a mouse down event that occurs while the user down`s the cursor over a selector resize handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                this.UndoElement = (IDiagramObject)(args.Element).Clone() as DiagramSelectionSettings;
                this.UndoParentElement = this.CommandHandler.GetSubProcess(args.Element);
                base.OnMouseDown(args);
                if (this.UndoElement.Nodes.Any())
                {
                    if (this.UndoElement.Nodes[0] is NodeGroup group && group.Children != null && group.Children.Any())
                    {
                        List<NodeBase> objects = new List<NodeBase>() { };
                        List<NodeBase> nodes = this.CommandHandler.GetAllDescendants(group, objects);
                        SfDiagramComponent diagram = group.Parent as SfDiagramComponent;
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            if (diagram?.GetObject(nodes[i].ID) is NodeBase node)
                                this.ChildTable[nodes[i].ID] = (IDiagramObject)(node).Clone();
                        }
                    }
                }

                if (args.Element is DiagramSelectionSettings selector)
                {
                    this.initialBounds.X = selector.OffsetX;
                    this.initialBounds.Y = selector.OffsetY;
                    this.initialBounds.Height = selector.Height;
                    this.initialBounds.Width = selector.Width;
                }
            }
        }
        /// <summary>
        /// This method triggers when a mouse move event that occurs while the use’sr mouse moves over a selector resize handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseMove(args);
                DiagramSelectionSettings obj = CommandHandler.RenderHelper();
                this.CurrentPosition = args.Position;
                DiagramRect deltaValues = this.UpdateSize(obj, this.StartPosition, this.CurrentPosition, this.corner, this.initialBounds);
                this.blocked = !(this.ScaleHelperObjects(
                    deltaValues.Width, deltaValues.Height, this.corner, this.StartPosition, this.CurrentPosition, obj, args.Element as DiagramSelectionSettings));
                this.prevPosition = this.CurrentPosition;
                CommandHandler.RefreshDiagram();
            }
            return !this.blocked;
        }
        /// <summary>
        /// This method triggers when an mouse up event that occurs while the user releases over a selector resize handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                if (CommandHandler.HelperObject is DiagramSelectionSettings obj)
                {
                    CommandHandler.HelperObject = null;
                    if ((!this.UndoElement.OffsetX.Equals(obj.OffsetX) || !this.UndoElement.OffsetY.Equals(obj.OffsetY) ||
                        !this.UndoElement.Width.Equals(obj.Width) || !this.UndoElement.Height.Equals(obj.Height)))
                    {
                        DiagramRect deltaValues = this.UpdateSize(args.Element as DiagramSelectionSettings, this.CurrentPosition, this.prevPosition, this.corner, this.initialBounds);
                        this.blocked = this.ScaleObjects(deltaValues.Width, deltaValues.Height, this.corner, (args.Element as DiagramSelectionSettings));
                        DiagramSelectionSettings newValue = new DiagramSelectionSettings()
                        {
                            OffsetX = obj.OffsetX,
                            OffsetY = obj.OffsetY,
                            Width = obj.Width,
                            Height = obj.Height
                        };
                        DiagramSelectionSettings oldValue = new DiagramSelectionSettings()
                        {
                            OffsetX = (UndoElement).OffsetX,
                            OffsetY = (UndoElement).OffsetY,
                            Width = (double)(UndoElement).Width,
                            Height = (double)(UndoElement).Height
                        };
                        SizeChangedEventArgs arg = new SizeChangedEventArgs()
                        {
                            Element = args.Element as DiagramSelectionSettings,
                            OldValue = oldValue,
                            NewValue = newValue
                        };
                        this.CommandHandler.InvokeDiagramEvents(DiagramEvent.SizeChanged, arg);
                        InternalHistoryEntry entry = new InternalHistoryEntry()
                        {
                            Type = HistoryEntryType.SizeChanged,
                            RedoObject = (args.Element).Clone() as IDiagramObject,
                            UndoObject = this.UndoElement.Clone() as IDiagramObject,
                            Category = EntryCategory.InternalEntry,
                            ChildTable = this.ChildTable
                        };
                        this.CommandHandler.StartGroupAction();
                        this.CommandHandler.AddHistoryEntry(entry);
                        if ((obj.Nodes.Count > 0) && obj.Nodes[0].ProcessId != null)
                        {
                            InternalHistoryEntry childEntry = new InternalHistoryEntry()
                            {
                                Type = HistoryEntryType.SizeChanged,
                                RedoObject = this.CommandHandler.GetSubProcess(args.Element),
                                UndoObject = this.UndoParentElement,
                                Category = EntryCategory.InternalEntry,

                            };
                            this.CommandHandler.AddHistoryEntry(childEntry);
                        }
                        this.CommandHandler.EndGroupAction();
                    }
                }
            }
            base.OnMouseUp(args);
        }
        /// <summary>
        /// This method triggers when a mouse leave event that occurs while the user leaves a selector resize handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            if (this.InAction)
            {
                this.OnMouseUp(args);
            }
        }

        internal bool ScaleHelperObjects(double deltaWidth, double deltaHeight, Actions corner, DiagramPoint startPoint, DiagramPoint endPoint, DiagramSelectionSettings source, DiagramSelectionSettings originalSource)
        {
            if (source != null && originalSource.Nodes.Count == 1 && originalSource.Nodes[0].Constraints.HasFlag(NodeConstraints.AspectRatio))
            {
                if (corner == Actions.ResizeWest || corner == Actions.ResizeEast || corner == Actions.ResizeNorth || corner == Actions.ResizeSouth)
                {
                    if (!(deltaHeight == 1 && deltaWidth == 1))
                    {
                        deltaHeight = deltaWidth = Math.Max(deltaHeight == 1 ? 0 : deltaHeight, deltaWidth == 1 ? 0 : deltaWidth);
                    }
                }
                else
                {
                    deltaHeight = deltaWidth = startPoint != endPoint ? Math.Max(deltaHeight, deltaWidth) : 0;
                }
            }

            if (source != null)
            {
                DiagramSelectionSettings oldValue = new DiagramSelectionSettings()
                {
                    OffsetX = source.OffsetX,
                    OffsetY = source.OffsetY,
                    Width = source.Width,
                    Height = source.Height
                };
                DiagramSelectionSettings helperObject = this.CommandHandler.HelperObject;
                double width = helperObject.Width;
                double height = helperObject.Height;
                double offsetX = helperObject.OffsetX;
                double offsetY = helperObject.OffsetY;
                this.blocked = this.CommandHandler.ScalePreview(deltaWidth, deltaHeight, GetPivot(this.corner), helperObject, ref width, ref height, ref offsetX, ref offsetY, false);
                DiagramSelectionSettings newValue = new DiagramSelectionSettings()
                {
                    OffsetX = offsetX,
                    OffsetY = offsetY,
                    Width = width,
                    Height = height
                };
                SizeChangingEventArgs arg = new SizeChangingEventArgs() { Element = originalSource, OldValue = oldValue, NewValue = newValue, Cancel = false };
                this.CommandHandler.InvokeDiagramEvents(DiagramEvent.SizeChanging, arg);
                if (!arg.Cancel)
                {
                    helperObject.Width = width;
                    helperObject.Height = height;
                    helperObject.OffsetX = offsetX;
                    helperObject.OffsetY = offsetY;
                }
            }
            return this.blocked;
        }
        /// <summary>
        /// Updates the size with delta width and delta height using scaling.
        /// </summary>
        /// <param name="deltaWidth"></param>
        /// <param name="deltaHeight"></param>
        /// <param name="corner"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        internal bool ScaleObjects(double deltaWidth, double deltaHeight, Actions corner, DiagramSelectionSettings source)
        {
            if (source is DiagramSelectionSettings && source.Nodes.Count == 1 && source.Nodes[0].Constraints.HasFlag(NodeConstraints.AspectRatio))
            {
                if (corner == Actions.ResizeWest || corner == Actions.ResizeEast || corner == Actions.ResizeNorth || corner == Actions.ResizeSouth)
                {
                    if (!(deltaHeight == 1 && deltaWidth == 1))
                    {
                        deltaHeight = deltaWidth = Math.Max(deltaHeight == 1 ? 0 : deltaHeight, deltaWidth == 1 ? 0 : deltaWidth);
                    }
                }
                else
                {
                    deltaHeight = deltaWidth = Math.Max(deltaHeight, deltaWidth);
                }

            }
            this.blocked = this.CommandHandler.ScaleSelectedItems(deltaWidth, deltaHeight, GetPivot(this.corner));
            return this.blocked;
        }

        private static DiagramPoint GetPivot(Actions actions)
        {
            switch (actions)
            {
                case Actions.ResizeWest:
                    return new DiagramPoint() { X = 1, Y = 0.5 };
                case Actions.ResizeEast:
                    return new DiagramPoint() { X = 0, Y = 0.5 };
                case Actions.ResizeNorth:
                    return new DiagramPoint() { X = 0.5, Y = 1 };
                case Actions.ResizeSouth:
                    return new DiagramPoint() { X = 0.5, Y = 0 };
                case Actions.ResizeNorthEast:
                    return new DiagramPoint() { X = 0, Y = 1 };
                case Actions.ResizeNorthWest:
                    return new DiagramPoint() { X = 1, Y = 1 };
                case Actions.ResizeSouthEast:
                    return new DiagramPoint() { X = 0, Y = 0 };
                case Actions.ResizeSouthWest:
                    return new DiagramPoint() { X = 1, Y = 0 };
                default:
                    break;
            }
            return new DiagramPoint() { X = 0.5, Y = 0.5 };
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (prevPosition != null)
            {
                prevPosition.Dispose();
                prevPosition = null;
            }
            if (initialBounds != null)
            {
                initialBounds.Dispose();
                initialBounds = null;
            }
        }

    }

    /// <summary>
    /// Represents the class that helps to select the objects.  
    /// </summary>
    public class SelectionController : InteractionControllerBase
    {
        internal SelectionController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when the mouse clicks on the element. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            this.InAction = true;
            base.OnMouseDown(args);
        }
        /// <summary>
        /// This method triggers when the mouse clicks and drags the selected element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseMove(args);
                //draw selected region
                if (this.InAction && !DiagramPoint.Equals(this.CurrentPosition, this.PreviousPosition) && this.CommandHandler != null)
                {
                    DiagramRect rect = DiagramRect.ToBounds(new List<DiagramPoint>() { this.PreviousPosition, this.CurrentPosition });
                    if (this.MouseDownElement != null && !ConstraintsUtil.CanMove(this.MouseDownElement))
                    {
                        this.CommandHandler.ClearObjectSelection(this.MouseDownElement);
                    }
                    else
                    {
                        this.CommandHandler.ClearSelectedItems();
                        this.CommandHandler.DrawSelectionRectangle(rect);
                    }
                }
            }
            return !this.blocked;
        }
        /// <summary>
        /// This method triggers when the mouse clicks on the selected element. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                bool refreshDiagram = false;
                //rubber band selection
                if (!DiagramPoint.Equals(this.CurrentPosition, this.PreviousPosition) && this.InAction)
                {
                    DiagramRect region = DiagramRect.ToBounds(new List<DiagramPoint>() { this.PreviousPosition, this.CurrentPosition });
                    this.CommandHandler.DoRubberBandSelection(region);
                }
                else
                {
                    //single selection
                    if (this.CommandHandler != null && (!this.CommandHandler.HasSelection() || args.Info == null || !args.Info.CtrlKey.Value))
                    {
                        if (args.Element != null)
                        {
                            this.CommandHandler.SelectObjects(new ObservableCollection<IDiagramObject> { args.Element }, false);
                            refreshDiagram = true;
                        }
                        else
                        {
                            if (CommandHandler.HasSelection())
                            {
                                this.CommandHandler.ClearSelection(true);
                                refreshDiagram = true;
                            }
                        }
                    }
                    else
                    {
                        //handling multiple selection
                        if (args.Element != null)
                        {
                            CommandHandler commandHandler = this.CommandHandler;
                            if (commandHandler != null)
                            {
                                if (!commandHandler.IsSelected(args.Element))
                                {
                                    commandHandler.SelectObjects(new ObservableCollection<IDiagramObject> { args.Element },
                                        true);
                                }
                                else if (args.ClickCount == 1)
                                {
                                    commandHandler.UnSelect(args.Element);
                                }
                            }

                            refreshDiagram = true;
                        }
                    }
                }
                this.CommandHandler.UpdateThumbConstaraints();
                if (refreshDiagram)
                {
                    this.CommandHandler.RefreshDiagram();
                }
                this.InAction = false;
            }
            base.OnMouseUp(args);
        }
        /// <summary>
        /// This method triggers when the mouse pointer leaves the selected element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            if (this.InAction)
            {
                this.OnMouseUp(args);
            }
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }

    /// <summary>
    /// Represents the class that is used to draw a polyline connector dynamically using the PolyLine Drawing Tool. 
    /// </summary>
    public class PolylineDrawingController : InteractionControllerBase
    {
        internal IDiagramObject DrawingObject { get; set; }
        internal PolylineDrawingController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when a mouse down event occurs with a drawing tool and PolyLine type in the diagram. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                this.InAction = true;
                base.OnMouseDown(args);
                if (this.DrawingObject == null)
                {
                    this.DrawingObject = this.CommandHandler.GetDrawingObject(args);
                }
            }
        }
        /// <summary>
        /// This method triggers when a mouse move event occurs with a drawing tool and PolyLine type in the diagram. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseMove(args);
            }
            return !this.blocked;
        }
        /// <summary>
        /// This method triggers when a mouse-up event occurs with a drawing tool and PolyLine type in the diagram. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseUp(args);
            }
        }
        /// <summary>
        /// This method triggers when a mouse leave event occurs with a drawing tool and PolyLine type in the diagram. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            if (args != null && this.InAction)
            {
                this.OnMouseUp(args);
            }
        }
        internal override void Dispose()
        {
            base.Dispose();
            if (DrawingObject != null)
            {
                DrawingObject = null;
            }
        }
    }

    /// <summary>
    /// Represents the class that is used to draw a connector. 
    /// </summary>
    public class ConnectorDrawingController : InteractionControllerBase
    {
        internal IDiagramObject DrawingObject { get; set; }
        internal Actions EndPoint { get; set; }
        internal ConnectorDrawingController(SfDiagramComponent diagram, Actions endPoint) : base(diagram)
        {
            this.EndPoint = endPoint;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the diagram and a mouse button is pressed.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            this.InAction = true;
            base.OnMouseDown(args);
        }
        /// <summary>
        /// This method triggers when the mouse button is clicked on the connector end thumb and the thumb is moved over the diagram.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (this.InAction)
            {
                if (args != null)
                {
                    base.OnMouseMove(args);
                    if (this.DrawingObject == null)
                    {
                        this.DrawingObject = this.CommandHandler.GetDrawingObject(args);
                    }
                    args.Element = this.DrawingObject;
                    if ((((args.Target != null) && args.Target is Node) || ((args.ActualObject != null) && (args.SourceWrapper != null) && DiagramUtil.CheckPort(args.ActualObject, args.SourceWrapper)))
                    && (this.EndPoint != Actions.ConnectorTargetEnd || (ConstraintsUtil.CanInConnect(args.Target as Node))))
                    {
                        this.CommandHandler.Connect(this.EndPoint, args);
                    }
                    this.EndPoint = Actions.ConnectorTargetEnd;
                }
            }
            else
            {
                if ((args != null) && (args.Element != null) && (args.SourceWrapper != null))
                {
                    this.CommandHandler.RenderHighlighter(args, true);
                }
                else
                {
                    this.CommandHandler.HighlighterElement = null;
                }
                this.CommandHandler.RefreshDiagram();
            }
            return !this.blocked;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the connector and a mouse button is released.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {

            base.OnMouseUp(args);
            if (this.CommandHandler != null)
            {
                _ = this.CommandHandler.AddObjectToDiagram();
            }
        }
        /// <summary>
        /// This method triggers when the mouse pointer leaves the diagram.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            if (this.InAction)
            {
                this.OnMouseUp(args);
            }
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (DrawingObject != null)
            {
                DrawingObject = null;
            }
        }
    }

    /// <summary>
    /// Represents the class that is used to draw a node that is defined by the user. 
    /// </summary>
    public class NodeDrawingController : InteractionControllerBase
    {
        /// <summary>
        /// the previous mouse position
        /// </summary>
        private DiagramPoint prevPosition;
        internal IDiagramObject DrawingObject { get; set; }
        internal NodeDrawingController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the diagram and a mouse button is pressed.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                this.InAction = true;
                this.CommandHandler.ClearSelection();
                base.OnMouseDown(args);
            }
        }
        /// <summary>
        /// This method triggers when the mouse button is pressed and a mouse pointer is moved over the diagram.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseMove(args);
                _ = CommandHandler.RenderHelper();
                if (this.DrawingObject == null)
                {
                    DiagramPoint startPoint = new DiagramPoint() { X = this.StartPosition.X, Y = this.StartPosition.Y };
                    DiagramPoint snapPoint = this.CommandHandler.GetSnappingPoints(startPoint);
                    if (!snapPoint.X.Equals(this.StartPosition.X))
                    {
                        this.DrawingObject = this.CommandHandler.GetDrawingObject(args, null, snapPoint);
                    }
                }
                else
                {
                    DiagramSelectionSettings helperObject = CommandHandler.RenderHelper();
                    this.CurrentPosition = args.Position;
                    DiagramPoint startPoint = new DiagramPoint() { X = this.StartPosition.X, Y = this.StartPosition.Y };
                    DiagramPoint endPoint = new DiagramPoint() { X = this.CurrentPosition.X, Y = this.CurrentPosition.Y };
                    DiagramPoint currentSnapPoint = this.CommandHandler.GetSnappingPoints(endPoint);
                    DiagramPoint startPositionSnapPoint = this.CommandHandler.GetSnappingPoints(startPoint);
                    double diffX = currentSnapPoint.X - startPositionSnapPoint.X;
                    double diffY = currentSnapPoint.Y - startPositionSnapPoint.Y;
                    helperObject.OffsetX = ((Node)this.DrawingObject).OffsetX + diffX / 2;
                    helperObject.OffsetY = ((Node)this.DrawingObject).OffsetY + diffY / 2;
                    double? width = ((Node)this.DrawingObject).Width;
                    if (width != null)
                        helperObject.Width = (double)width + Math.Abs(diffX);
                    double? height = ((Node)this.DrawingObject).Height;
                    if (height != null)
                        helperObject.Height = (double)height + Math.Abs(diffY);
                    this.prevPosition = this.CurrentPosition;
                    CommandHandler.RefreshDiagram();
                }
            }
            return !this.blocked;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the node and a mouse button is released.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseUp(args);
            }
        }
        /// <summary>
        /// This method triggers when the mouse pointer leaves the diagram.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            if (args != null && this.InAction)
            {
                this.OnMouseUp(args);
            }
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (prevPosition != null)
            {
                prevPosition.Dispose();
                prevPosition = null;
            }
            if (DrawingObject != null)
            {
                DrawingObject = null;
            }
        }
    }

    /// <summary>
    /// Represents the class that is used to draw a polygon shape node dynamically using the polygon Tool. 
    /// </summary>
    public class PolygonDrawingController : InteractionControllerBase
    {
        internal IDiagramObject DrawingObject { get; set; }
        internal PolygonDrawingController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when a mouse down event occurs with a drawing tool and polygon shapes in the diagram. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                this.InAction = true;
                base.OnMouseDown(args);
                DiagramPoint[] points =
                {
                    new DiagramPoint() { X = this.StartPosition.X, Y=this.StartPosition.Y },
                    new DiagramPoint() { X = this.CurrentPosition.X, Y = this.CurrentPosition.Y },
                };
                if (this.DrawingObject == null)
                {
                    this.DrawingObject = this.CommandHandler.GetDrawingObject(args, points);
                }
            }
        }
        /// <summary>
        /// This method triggers when a mouse move event occurs with a drawing tool and polygon shapes in the diagram. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {

            base.OnMouseMove(args);
            if (this.InAction)
            {
                BasicShape obj = ((this.DrawingObject as Node)?.Shape as BasicShape);
                obj.Points[^1].X = this.CurrentPosition.X;
                obj.Points[^1].Y = this.CurrentPosition.Y;
                this.CommandHandler.PolygonObject.Data = PathUtil.GetPolygonPath(obj.Points);
                CommandHandler.RefreshDiagram();
            }
            return !this.blocked;
        }
        /// <summary>
        /// This method triggers when a mouse leave event occurs with a drawing tool and polygon shapes in the diagram. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            if (args != null && this.InAction)
            {
                this.OnMouseUp(args);
            }
        }
        internal override void Dispose()
        {
            base.Dispose();
            if (DrawingObject != null)
            {
                DrawingObject = null;
            }
        }
    }

    /// <summary>
    /// Represents the class that helps to rotate the selected objects.
    /// </summary>
    public class RotationController : InteractionControllerBase
    {
        internal RotationController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when a mouse down event that occurs while the user down`s the cursor over a selector rotate handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                this.UndoElement = (IDiagramObject)(args.Element).Clone() as DiagramSelectionSettings;
                DiagramSelectionSettings undoElement = this.UndoElement;
                if (undoElement != null && undoElement.Nodes.Any())
                {
                    Node element = undoElement.Nodes[0];
                    if (!(element is NodeGroup) && element.Wrapper is { Children: { } } && element.Wrapper.Children.Count > 0)
                    {
                        List<NodeBase> objects = new List<NodeBase>();
                        List<NodeBase> nodes = this.CommandHandler.GetAllDescendants(undoElement.Nodes[0], objects);
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            if (undoElement.Nodes[0].Parent is SfDiagramComponent diagram && diagram.GetObject((nodes[i] as Node)?.ID) is Node node) this.ChildTable[(nodes[i] as Node)?.ID] = (IDiagramObject)(node).Clone();
                        }
                    }
                }
                base.OnMouseDown(args);
            }
        }
        /// <summary>
        /// This method triggers when a mouse move event that occurs while the user’s mouse moves over a selector rotating handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseMove(args);
                IDiagramObject objects = this.CommandHandler.RenderHelper();
                this.CurrentPosition = args.Position;
                DiagramPoint refPoint = new DiagramPoint() { X = ((DiagramSelectionSettings)objects).OffsetX, Y = ((DiagramSelectionSettings)objects).OffsetY };
                double angle = DiagramPoint.FindAngle(refPoint, this.CurrentPosition) + 90;
                double snapAngle = this.CommandHandler.SnapAngle(angle);
                angle = snapAngle != 0 ? snapAngle : angle;
                angle = ((angle + 360) % 360);
                double actualRotateAngle = ((DiagramSelectionSettings)args.Element).Wrapper.RotationAngle + (angle - ((DiagramSelectionSettings)args.Element).Wrapper.RotationAngle);
                DiagramSelectionSettings oldValue = new DiagramSelectionSettings() { RotationAngle = ((DiagramSelectionSettings)objects).RotationAngle };
                DiagramSelectionSettings newValue = new DiagramSelectionSettings() { RotationAngle = actualRotateAngle };
                RotationChangingEventArgs arg = new RotationChangingEventArgs()
                {
                    Element = (DiagramSelectionSettings)args.Element,
                    OldValue = oldValue,
                    NewValue = newValue,
                    Cancel = false
                };
                this.CommandHandler.InvokeDiagramEvents(DiagramEvent.RotationChanging, arg);
                if (!arg.Cancel)
                {
                    ((DiagramSelectionSettings)objects).RotationAngle = actualRotateAngle;
                    CommandHandler.RefreshDiagram();
                }
            }
            return !this.blocked;
        }
        /// <summary>
        /// This method triggers when an mouse up event that occurs while the user releases over a selector rotating handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                IDiagramObject objects = CommandHandler.HelperObject;
                if (objects != null)
                {
                    DiagramSelectionSettings oldValue = new DiagramSelectionSettings() { RotationAngle = ((DiagramSelectionSettings)args.Element).RotationAngle };
                    DiagramSelectionSettings newValue = new DiagramSelectionSettings() { RotationAngle = ((DiagramSelectionSettings)objects).RotationAngle };
                    RotationChangedEventArgs arg = new RotationChangedEventArgs()
                    {
                        Element = args.Element as DiagramSelectionSettings,
                        OldValue = oldValue,
                        NewValue = newValue
                    };
                    CommandHandler.InvokeDiagramEvents(DiagramEvent.RotationChanged, arg);
                    this.blocked = !this.CommandHandler.RotateSelectedItems(((DiagramSelectionSettings)objects).RotationAngle - ((DiagramSelectionSettings)args.Element).Wrapper.RotationAngle);
                    if (args.Element is DiagramSelectionSettings obj)
                    {
                        InternalHistoryEntry entry = new InternalHistoryEntry()
                        {
                            Type = HistoryEntryType.RotationChanged,
                            RedoObject = obj.Clone() as IDiagramObject,
                            UndoObject = this.UndoElement.Clone() as IDiagramObject,
                            Category = EntryCategory.InternalEntry,
                        };

                        CommandHandler.HelperObject = null;
                        this.CommandHandler.RefreshDiagram();
                        this.CommandHandler.AddHistoryEntry(entry);
                    }
                }
                base.OnMouseUp(args);
            }
        }
        /// <summary>
        /// This method triggers when a mouse leave event that occurs while the user leaves a selector’s rotating handles. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            if (args != null && this.InAction)
            {
                this.OnMouseUp(args);
            }
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }

    /// <summary>
    /// Represents the class that is used to handle the fixed userhandle events. 
    /// </summary>
    public class FixedUserHandleController : InteractionControllerBase
    {
        internal FixedUserHandleController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when the mouse clicks on the fixed user handle.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                this.InAction = false;
                FixedUserHandle fixedUserHandle = new FixedUserHandle();
                string iconId = args.SourceWrapper.ID;
                if (args.Element is Node node && node.FixedUserHandles.Any())
                {
                    for (int i = 0; i < node.FixedUserHandles.Count; i++)
                    {
                        if (iconId.IndexOf(node.FixedUserHandles[i].ID, StringComparison.InvariantCulture) > -1)
                        {
                            fixedUserHandle = node.FixedUserHandles[i];
                        }
                    }
                }
                else if (args.Element is Connector connector && connector.FixedUserHandles.Any())
                {
                    for (int i = 0; i < connector.FixedUserHandles.Count; i++)
                    {
                        if (iconId.IndexOf(connector.FixedUserHandles[i].ID, StringComparison.InvariantCulture) > -1)
                        {
                            fixedUserHandle = connector.FixedUserHandles[i];
                        }
                    }
                }
                FixedUserHandleClickEventArgs arg = new FixedUserHandleClickEventArgs() { Element = args.Element, FixedUserHandle = fixedUserHandle };
                CommandHandler.InvokeDiagramEvents(DiagramEvent.FixedUserHandleClick, arg);
                this.InAction = false;
                base.OnMouseUp(args);
            }
        }
        internal override void Dispose()
        {
            base.Dispose();
        }
    }

    ///<summary>
    /// Represents the class that is used to pan the diagram control on drag.
    ///</summary>
    internal class ZoomPanController : InteractionControllerBase
    {
        private readonly bool zooming;
        internal ZoomPanController(SfDiagramComponent diagram, bool zoom) : base(diagram)
        {
            zooming = zoom;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the diagram and a mouse button is clicked.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            base.OnMouseDown(args);
            InAction = true;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the diagram.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            base.OnMouseMove(args);
            if (InAction)
            {

                if (!zooming)
                {
                    double difX = CurrentPosition.X - PreviousPosition.X;
                    double difY = CurrentPosition.Y - PreviousPosition.Y;
                    CommandHandler.Scroll(difX, difY, CurrentPosition);
                }
                else if (args.MoveTouches != null && args.MoveTouches.Count > 1)
                {
                    ITouches startTouch0 = args.StartTouches[0];
                    ITouches startTouch1 = args.StartTouches[1];
                    ITouches moveTouch0 = args.MoveTouches[0];
                    ITouches moveTouch1 = args.MoveTouches[1];
                    double scale = GetDistance(moveTouch0, moveTouch1) / GetDistance(startTouch0, startTouch1);
                    DiagramPoint focusPoint = args.Position;
                    this.CommandHandler.Zoom(scale, 0, 0, focusPoint);
                    UpdateTouch(startTouch0, moveTouch0);
                    UpdateTouch(startTouch1, moveTouch1);
                }
            }
            return !blocked;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the element and a mouse button is released. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            base.OnMouseUp(args);
            InAction = false;
        }

        private static double GetDistance(ITouches touch1, ITouches touch2)
        {
            double x = touch2.PageX - touch1.PageX;
            double y = touch2.PageY - touch1.PageY;
            return Math.Sqrt((x * x) + (y * y));
        }

        private static void UpdateTouch(ITouches startTouch, ITouches moveTouch)
        {
            startTouch.PageX = moveTouch.PageX;
            startTouch.PageY = moveTouch.PageY;
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }

    /// <summary>
    /// Represents the class that helps to move (drag) the selected objects.  
    /// </summary>
    public class DragController : InteractionControllerBase
    {
        private DiagramPoint initialOffset;
        internal IDiagramObject CurrentTarget;
        /// <summary>
        /// Creates a new instance of the MoveTool from the given MoveTool.
        /// </summary>
        /// <param name="diagram"></param>
        public DragController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the element and a mouse button is clicked.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                if (args.Element is NodeBase)
                {
                    ObservableCollection<IDiagramObject> nodes = CommandHandler.GetSelectedObject();
                    CommandHandler.SelectObjects(new ObservableCollection<IDiagramObject> { args.Element }, (args.Info != null && args.Info.CtrlKey.Value), nodes);
                    DiagramSelectionSettings selectedObject = new DiagramSelectionSettings();
                    if (args.Element is Node node)
                    {
                        selectedObject.Nodes.Add(node);
                    }
                    else
                    {
                        selectedObject.Connectors.Add(args.Element as Connector);
                    }
                    this.UndoElement = (DiagramSelectionSettings)selectedObject.Clone();
                }
                else
                {
                    this.UndoElement = (DiagramSelectionSettings)(args.Element as DiagramSelectionSettings)?.Clone();
                }
                this.UndoParentElement = this.CommandHandler.GetSubProcess(args.Element);
                base.OnMouseDown(args);
                this.initialOffset = new DiagramPoint { X = 0, Y = 0 };
            }
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>Blocked</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseMove(args);
                double diffX = this.initialOffset.X + (this.CurrentPosition.X - this.PreviousPosition.X);
                double diffY = this.initialOffset.Y + (this.CurrentPosition.Y - this.PreviousPosition.Y);
                this.CommandHandler.DragOverElement(args, this.CurrentPosition);
                if (diffX != 0 || diffY != 0)
                {
                    DiagramSelectionSettings helperObject = CommandHandler.RenderHelper();
                    DiagramPoint snappedPoint = CommandHandler.SnapPoint(this.PreviousPosition, this.CurrentPosition, diffX, diffY);
                    this.initialOffset.X = diffX - snappedPoint.X;
                    this.initialOffset.Y = diffY - snappedPoint.Y;
                    DiagramSelectionSettings oldValue = new DiagramSelectionSettings { OffsetX = helperObject.OffsetX, OffsetY = helperObject.OffsetY };

                    DiagramSelectionSettings newValue = new DiagramSelectionSettings() { OffsetX = helperObject.OffsetX + snappedPoint.X, OffsetY = helperObject.OffsetY + snappedPoint.Y };
                    PositionChangingEventArgs positionChangingEventArgs = new PositionChangingEventArgs()
                    {
                        Cancel = false,
                        NewValue = newValue,
                        OldValue = oldValue,
                        Element = args.Element
                    };
                    CommandHandler.InvokeDiagramEvents(DiagramEvent.PositionChanging, positionChangingEventArgs);

                    if (!positionChangingEventArgs.Cancel)
                    {
                        DiagramRect boundsValue = BaseUtil.GetSelectorBounds(helperObject);
                        bool checkBoundaryConstraints = CommandHandler.CheckBoundaryConstraints(diffX, diffY, boundsValue);
                        if (checkBoundaryConstraints)
                        {
                            helperObject.OffsetX += snappedPoint.X;
                            helperObject.OffsetY += snappedPoint.Y;
                        }
                        if (helperObject.Connectors.Count == 1)
                        {
                            SfDiagramComponent.IsProtectedOnChange = false;
                            Connector connector = helperObject.Connectors[0];
                            CommandHandler.Drag(connector, snappedPoint.X, snappedPoint.Y, true);
                            CommandHandler.DisConnect(helperObject);
                            List<DiagramPoint> points = connector.GetConnectorPoints();
                            DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                            connector.Wrapper.Measure(new DiagramSize());
                            connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                            SfDiagramComponent.IsProtectedOnChange = true;
                        }
                    }
                    CommandHandler.RefreshDiagram();
                    this.CurrentTarget = args.Target;
                    if (this.CurrentTarget != null && (args.Element != this.CurrentTarget))
                    {
                        this.CommandHandler.DrawHighlighter((this.CurrentTarget as NodeBase)?.Wrapper);
                    }
                    else
                    {
                        this.CommandHandler.HighlighterElement = null;
                    }
                }
                this.PreviousPosition = this.CurrentPosition;
            }
            return !blocked;
        }
        /// <summary>
        /// This method triggers when the mouse pointer is moved over the element and a mouse button is released. 
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            bool isDiagram = false;
            if (args != null)
            {
                DiagramSelectionSettings helperObject = CommandHandler.HelperObject;
                if (helperObject != null)
                {
                    DiagramSelectionSettings selectedObject = new DiagramSelectionSettings();
                    if (args.Element is Node || args.Element is Connector)
                    {
                        if (args.Element is Node node)
                        {
                            selectedObject.Nodes.Add(node);
                        }
                        else
                        {
                            selectedObject.Connectors.Add(args.Element as Connector);
                        }
                    }
                    else
                    {
                        selectedObject = args.Element as DiagramSelectionSettings;
                    }
                    if ((selectedObject.Nodes.Count > 0 && (selectedObject.Nodes[0].Parent is SfDiagramComponent)) || (selectedObject.Connectors.Count > 0 && (selectedObject.Connectors[0].Parent is SfDiagramComponent)))
                    {
                        isDiagram = true;
                    }
                    if (args.Element as DiagramSelectionSettings != null && isDiagram)
                    {
                        this.CommandHandler.StartGroupAction();
                    }
                    DiagramSelectionSettings finalValue = new DiagramSelectionSettings() { OffsetX = helperObject.OffsetX, OffsetY = helperObject.OffsetY };
                    CommandHandler.UpdateSelector();
                    PositionChangedEventArgs positionChangedEventArgs = new PositionChangedEventArgs() { Element = args.Element, NewValue = finalValue, OldValue = finalValue };
                    CommandHandler.InvokeDiagramEvents(DiagramEvent.PositionChanged, positionChangedEventArgs);


                    if (selectedObject != null && isDiagram)
                    {
                        InternalHistoryEntry entry = new InternalHistoryEntry()
                        {
                            Type = HistoryEntryType.PositionChanged,
                            RedoObject = (selectedObject).Clone() as IDiagramObject,
                            UndoObject = this.UndoElement.Clone() as DiagramSelectionSettings,
                            Category = EntryCategory.InternalEntry
                        };
                        if (selectedObject.Clone() is DiagramSelectionSettings obj && (obj.Nodes.Count > 0) && !string.IsNullOrEmpty(obj.Nodes[0].ProcessId))
                        {
                            InternalHistoryEntry sizeEntry = new InternalHistoryEntry()
                            {
                                Type = HistoryEntryType.SizeChanged,
                                Category = EntryCategory.InternalEntry,
                                UndoObject = this.UndoParentElement,
                                RedoObject = this.CommandHandler.GetSubProcess(args.Element)
                            };
                            this.CommandHandler.AddHistoryEntry(sizeEntry);
                        }
                        this.CommandHandler.AddHistoryEntry(entry);
                    }

                    if (args.Element as DiagramSelectionSettings != null && isDiagram)
                    {
                        this.CommandHandler.EndGroupAction();
                    }
                    if (args.Element != null && this.CurrentTarget != null && ConstraintsUtil.CanAllowDrop(this.CurrentTarget as NodeBase))
                    {
                        DropEventArgs dropArgs = new DropEventArgs()
                        {
                            Element = args.Element,
                            Position = args.Position,
                            Target = args.Target,
                            Cancel = false
                        };
                        this.CommandHandler.InvokeDiagramEvents(DiagramEvent.DragDrop, dropArgs);
                    }
                }

                if (this.CommandHandler.HighlighterElement != null)
                {
                    this.CommandHandler.HighlighterElement = null;
                    this.CommandHandler.RefreshDiagram();
                }
                base.OnMouseUp(args);
            }
        }
        /// <summary>
        /// This method triggers when the mouse pointer leaves the element.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            this.OnMouseUp(args);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (initialOffset != null)
            {
                initialOffset.Dispose();
                initialOffset = null;
            }
            if (CurrentTarget != null)
            {
                CurrentTarget = null;
            }
        }
    }
    ///<summary>
    /// Represents the class that is used to open the hyperlink from the node’s annotation at mouse up. 
    ///</summary>
    internal class LabelController : InteractionControllerBase
    {
        internal LabelController(SfDiagramComponent diagram) : base(diagram)
        {
        }
        /// <summary>
        /// This method triggers when a mouse pointer is moved over the annotation hyperlink and a mouse button is released.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                if (args.SourceWrapper is TextElement wrapper) DomUtil.OpenUrl(wrapper.Hyperlink.Url);
                base.OnMouseUp(args);
            }
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }

    /// <summary>
    /// Represents the class that helps to connect the connector with the node.
    /// </summary>
    internal class ConnectionController : InteractionControllerBase
    {
        internal Actions EndPoint { get; set; }
        internal bool IsConnected { get; set; }
        private int selectedSegmentIndex;

        internal ConnectionController(SfDiagramComponent diagram, Actions endPoint) : base(diagram)
        {
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// This method triggers when a mouse down event occurs in a connector target point or source point.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseDown(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                this.InAction = true;
                this.UndoElement = null;
                DiagramSelectionSettings selector = args.Element as DiagramSelectionSettings;
                Connector connector = args.Element as Connector;
                if (args.Element != null)
                {
                    if (selector != null)
                    {
                        this.UndoElement = (DiagramSelectionSettings)(selector).Clone();
                    }
                    else if (connector != null)
                    {
                        this.UndoElement = new DiagramSelectionSettings() { Connectors = new ObservableCollection<Connector>() { (Connector)(connector).Clone() } };
                    }
                }
                base.OnMouseDown(args);
                if (selector != null && selector.Connectors.Count > 0)
                {
                    connector = selector.Connectors[0];
                }
                // Sets the selected segment 
                if (connector != null && (this.EndPoint == Actions.BezierSourceThumb || this.EndPoint == Actions.BezierTargetThumb))
                {
                    for (int i = 0; i < connector.Segments.Count; i++)
                    {
                        BezierSegment segment = connector.Segments[i] as BezierSegment;
                        DiagramPoint segmentPoint1 = !DiagramPoint.IsEmptyPoint(segment.Point1) ? segment.Point1 : segment.BezierPoint1;
                        DiagramPoint segmentPoint2 = !DiagramPoint.IsEmptyPoint(segment.Point2) ? segment.Point2 : segment.BezierPoint2;
                        if (Internal.ActionsUtil.Contains(this.CurrentPosition, segmentPoint1, connector.HitPadding) ||
                            Internal.ActionsUtil.Contains(this.CurrentPosition, segmentPoint2, connector.HitPadding))
                        {
                            this.selectedSegmentIndex = i;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method triggers when a mouse move event occurs in a connector target point or source point.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        /// <returns>IsConnected</returns>
        public override bool OnMouseMove(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                base.OnMouseMove(args);
                if (this.InAction)
                {
                    double diffX = (this.CurrentPosition.X - this.PreviousPosition.X);
                    double diffY = (this.CurrentPosition.Y - this.PreviousPosition.Y);

                    if (diffX != 0 || diffY != 0)
                    {
                        PointPort inPort = null;
                        PointPort outPort = null;
                        string targetPortId = string.Empty;
                        string targetNodeId = string.Empty;
                        DiagramSelectionSettings helperObject = CommandHandler.RenderHelper();
                        this.CurrentPosition = this.CommandHandler.SnapConnectorEnd(this.CurrentPosition);
                        if (args.Target != null)
                        {
                            IDiagramObject target = CommandHandler.FindTarget(args.TargetWrapper, args.Target);
                            if (target is PointPort port)
                            {
                                targetPortId = port.ID;
                                targetNodeId = (args.Target as Node)?.ID;
                            }
                            else
                            {
                                targetNodeId = (target as NodeBase)?.ID;
                            }
                            inPort = DiagramUtil.GetInOutConnectPorts(args.Target as Node, true);
                            outPort = DiagramUtil.GetInOutConnectPorts(args.Target as Node, false);
                        }
                        EndPointChangingEventArgs arg;
                        SfDiagramComponent.IsProtectedOnChange = false;
                        arg = new EndPointChangingEventArgs
                        {
                            Connector = (args.Element as DiagramSelectionSettings)?.Connectors[0],
                            TargetNodeID = targetNodeId,
                            OldValue = new DiagramPoint(this.PreviousPosition.X, this.PreviousPosition.Y),
                            NewValue = new DiagramPoint(this.CurrentPosition.X, this.CurrentPosition.Y),
                            Cancel = false,
                            TargetPortID = targetPortId
                        };
                        CommandHandler.InvokeDiagramEvents(this.EndPoint == Actions.ConnectorSourceEnd ?
                        DiagramEvent.SourcePointChanging : DiagramEvent.TargetPointChanging, arg);
                        if (!arg.Cancel)
                        {
                            currentPoint = args.Position;
                            CommandHandler.DragConnectorEnds(EndPoint, helperObject, this.CurrentPosition, this.selectedSegmentIndex, args.Target, targetPortId);
                            if (args.Target != null && args.Target is Node &&
                                ((this.EndPoint == Actions.ConnectorSourceEnd && (ConstraintsUtil.CanOutConnect(args.Target as Node) || (outPort != null && ConstraintsUtil.CanPortOutConnect(outPort))))
                                || (this.EndPoint == Actions.ConnectorTargetEnd && (ConstraintsUtil.CanInConnect(args.Target as Node) || (inPort != null && ConstraintsUtil.CanPortInConnect(inPort))))))
                            {
                                if (CommandHandler.CanDisconnect(this.EndPoint, args, targetPortId, targetNodeId))
                                {
                                    this.CommandHandler.DisConnect(CommandHandler.HelperObject, this.EndPoint);
                                    this.IsConnected = true;
                                }
                                IDiagramObject target = CommandHandler.FindTarget(args.TargetWrapper, args.Target);
                                if (target is Node)
                                {
                                    if ((ConstraintsUtil.CanInConnect(target as Node) && this.EndPoint == Actions.ConnectorTargetEnd)
                                        || (ConstraintsUtil.CanOutConnect(target as Node) && this.EndPoint == Actions.ConnectorSourceEnd))
                                    {
                                        this.CommandHandler.Connect(this.EndPoint, args);
                                        this.IsConnected = true;
                                    }
                                }
                                else
                                {
                                    bool isConnect = ConstraintsUtil.CheckConnect(target as PointPort, this.EndPoint);
                                    if (isConnect)
                                    {
                                        this.IsConnected = true;
                                        this.CommandHandler.Connect(this.EndPoint, args);
                                    }
                                }
                            }
                            else if (!EndPoint.ToString().Contains("Bezier", StringComparison.InvariantCulture))
                            {
                                this.IsConnected = true;
                                this.CommandHandler.DisConnect(CommandHandler.HelperObject, this.EndPoint);
                            }
                            Connector connector = helperObject.Connectors[0];
                            List<DiagramPoint> points = connector.GetConnectorPoints();
                            DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                            connector.Wrapper.Measure(new DiagramSize());
                            connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                            CommandHandler.RefreshDiagram();
                            SfDiagramComponent.IsProtectedOnChange = true;
                            if (string.IsNullOrEmpty(targetNodeId) && string.IsNullOrEmpty(targetPortId))
                            {
                                CommandHandler.HighlighterElement = null;
                            }
                        }
                    }
                }
                this.PreviousPosition = this.CurrentPosition;
            }
            return this.IsConnected;
        }

        /// <summary>
        /// This method triggers when a mouse-up event occurs in at a connector target point or source point.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseUp(DiagramMouseEventArgs args)
        {
            if (args != null)
            {
                DiagramSelectionSettings selector = CommandHandler.HelperObject;
                if (selector != null && selector.Connectors.Any() && this.UndoElement.Connectors.Any())
                {
                    Connector helperConnector = selector.Connectors[0];
                    Connector oldConnector = this.UndoElement.Connectors[0];
                    if (helperConnector.SourceID != oldConnector.SourceID || helperConnector.TargetID != oldConnector.TargetID ||
                        helperConnector.SourcePortID != oldConnector.SourcePortID || helperConnector.TargetPortID != oldConnector.TargetPortID)
                    {
                        ConnectionChangedEventArgs connectionChangedEventArgs = new ConnectionChangedEventArgs()
                        {
                            Connector = helperConnector,
                            ConnectorAction = this.EndPoint,
                            NewValue = new ConnectionObject() { SourceID = helperConnector.SourceID, TargetID = helperConnector.TargetID, SourcePortID = helperConnector.SourcePortID, TargetPortID = helperConnector.TargetPortID },
                            OldValue = new ConnectionObject() { SourceID = oldConnector.SourceID, TargetID = oldConnector.TargetID, SourcePortID = oldConnector.SourcePortID, TargetPortID = oldConnector.TargetPortID }
                        };
                        CommandHandler.InvokeDiagramEvents(DiagramEvent.ConnectionChanged, connectionChangedEventArgs);
                    }

                    string targetPortID = null; string targetNodeID = null;
                    if (args.Target != null)
                    {
                        IDiagramObject target = CommandHandler.FindTarget(args.TargetWrapper, args.Target);
                        if (target is PointPort)
                        {
                            targetPortID = (target as PointPort).ID;
                            targetNodeID = (args.Target as Node)?.ID;
                        }
                        else
                        {
                            targetNodeID = (target as NodeBase)?.ID;
                        }
                    }
                    if ((args.Element as DiagramSelectionSettings)?.Connectors.Count > 0)
                    {
                        EndPointChangedEventArgs arg = new EndPointChangedEventArgs
                        {
                            Connector = (args.Element as DiagramSelectionSettings)?.Connectors[0],
                            TargetNodeID = targetNodeID,
                            OldValue = (this.EndPoint == Actions.ConnectorSourceEnd) ? new DiagramPoint(oldConnector.SourcePoint.X, oldConnector.SourcePoint.Y) : new DiagramPoint(oldConnector.TargetPoint.X, oldConnector.TargetPoint.Y),
                            NewValue = (this.EndPoint == Actions.ConnectorSourceEnd) ? new DiagramPoint(helperConnector.SourcePoint.X, helperConnector.SourcePoint.Y) : new DiagramPoint(helperConnector.TargetPoint.X, helperConnector.TargetPoint.Y),
                            TargetPortID = targetPortID
                        };
                        CommandHandler.InvokeDiagramEvents(this.EndPoint == Actions.ConnectorSourceEnd ?
                        DiagramEvent.SourcePointChanged : DiagramEvent.TargetPointChanged, arg);

                        CommandHandler.HighlighterElement = null;
                        CommandHandler.UpdateConnectorProperties();
                    }
                    if (this.UndoElement != null && args.Element != null)
                    {
                        InternalHistoryEntry entry = new InternalHistoryEntry()
                        {
                            Type = HistoryEntryType.ConnectionChanged,
                            RedoObject = (args.Element).Clone() as IDiagramObject,
                            UndoObject = this.UndoElement.Clone() as DiagramSelectionSettings,
                            Category = EntryCategory.InternalEntry,
                        };
                        this.CommandHandler.AddHistoryEntry(entry);
                    }
                    if (this.EndPoint == Actions.BezierTargetThumb || this.EndPoint == Actions.BezierSourceThumb)
                    {
                        if (this.UndoElement != null && args.Element != null)
                        {
                            InternalHistoryEntry entry = new InternalHistoryEntry()
                            {
                                Type = HistoryEntryType.SegmentChanged,
                                RedoObject = (args.Element).Clone() as IDiagramObject,
                                UndoObject = this.UndoElement.Clone() as DiagramSelectionSettings,
                                Category = EntryCategory.InternalEntry,
                            };
                            this.CommandHandler.AddHistoryEntry(entry);
                        }
                    }
                }
                InAction = false;
                base.OnMouseUp(args);
            }
        }

        /// <summary>
        /// This method triggers when a mouse leave event occurs in a connector target point or source point.
        /// </summary>
        /// <param name="args">DiagramMouseEventArgs</param>
        public override void OnMouseLeave(DiagramMouseEventArgs args)
        {
            this.OnMouseUp(args);
        }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }
}
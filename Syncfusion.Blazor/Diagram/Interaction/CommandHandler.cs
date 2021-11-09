using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;


namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// 
    /// </summary>
    internal class CommandHandler
    {
        private SfDiagramComponent diagram;
        private ClipBoardObject clipboardData = new ClipBoardObject();
        private DiagramObjectCollection<NodeBase> clonedItems = new DiagramObjectCollection<NodeBase>();
        private Dictionary<string, IDiagramObject> processTable = new Dictionary<string, IDiagramObject>();
        internal DiagramSelectionSettings HelperObject { get; set; }
        internal PathElement PolygonObject { get; set; }
        internal RectAttributes HighlighterElement { get; set; }
        internal IDiagramObject CurrentDrawingObject;
        private bool isGroupDragging;

        internal CommandHandler(SfDiagramComponent diagram)
        {
            this.diagram = diagram;
        }

        internal void Scroll(double scrollX, double scrollY, DiagramPoint focusPoint = null)
        {
            //diagram.ScrollActions |= ScrollActions.Interaction;
            bool panX = ConstraintsUtil.CanPanX(diagram);
            bool panY = ConstraintsUtil.CanPanY(diagram);
            diagram.Pan(
                (panX ? scrollX : 0) * diagram.Scroller.CurrentZoom,
                (panY ? scrollY : 0) * diagram.Scroller.CurrentZoom, focusPoint);
            if (diagram.ScrollSettings.Parent == null)
            {
                //diagram.ScrollActions &= ~ScrollActions.Interaction;
            }
        }

        internal void Zoom(double scale, double scrollX, double scrollY, DiagramPoint focusPoint = null)
        {
            this.diagram.Scroller.Zoom(scale, scrollX * this.diagram.Scroller.CurrentZoom, scrollY * this.diagram.Scroller.CurrentZoom, focusPoint);
        }

        internal static IDiagramObject FindTarget(ICommonElement element, IDiagramObject argsTarget)
        {
            if (argsTarget is Node node)
            {
                if (element != null && element.ID == node.ID + "_content")
                {
                    return node;
                }
                if (element is PathElement)
                {
                    for (int i = 0; i < node.Ports.Count; i++)
                    {
                        PointPort port = node.Ports[i];
                        if (element.ID == node.ID + '_' + port.ID)
                        {
                            return port;
                        }
                    }
                }
            }
            return argsTarget;
        }
        private static double GetNearestSnapPoint(double value, double[] snapIntervals, double scale)
        {
            double diff = Math.Log(scale) / Math.Log(2);
            scale = scale > 1 ? Math.Pow(2, Math.Floor(diff)) : Math.Pow(2, Math.Ceiling(diff));
            double cutOff = 0;
            for (int i = 0; i < snapIntervals.Length; i++)
            {
                cutOff += snapIntervals[i];
            }
            cutOff /= scale;
            double quotient = Math.Floor(Math.Abs(value) / cutOff);
            double balanceValue = value % cutOff;
            double previousValue = quotient * cutOff;
            if (!previousValue.Equals(value))
            {
                if (value >= 0)
                {
                    for (int i = 0; i < snapIntervals.Length; i++)
                    {
                        if (balanceValue <= snapIntervals[i] / scale)
                        {
                            return previousValue + (balanceValue < (snapIntervals[i] / (2 * scale)) ? 0 : snapIntervals[i] / scale);
                        }
                    }
                }
                else
                {
                    previousValue *= -1;
                    for (int i = 0; i < snapIntervals.Length; i++)
                    {
                        if (Math.Abs(balanceValue) <= snapIntervals[i] / scale)
                        {
                            return previousValue - (Math.Abs(balanceValue) < (snapIntervals[i] / (2 * scale)) ? 0 : snapIntervals[i] / scale);
                        }
                        else
                        {
                            previousValue -= snapIntervals[i] / scale;
                            balanceValue += snapIntervals[i] / scale;
                        }
                    }
                }
            }
            return value;
        }
        internal DiagramPoint GetSnappingPoints(DiagramPoint point)
        {
            SnapSettings snapSettings = this.diagram.SnapSettings;
            point.X = GetNearestSnapPoint(point.X, snapSettings.HorizontalGridLines.SnapIntervals, this.diagram.Scroller.CurrentZoom);
            point.Y = GetNearestSnapPoint(point.Y, snapSettings.VerticalGridLines.SnapIntervals, this.diagram.Scroller.CurrentZoom);
            return point;
        }
        internal IDiagramObject GetDrawingObject(DiagramMouseEventArgs args, DiagramPoint[] points = null, DiagramPoint point = null)
        {
            DiagramPoint position = args.Position;
            Node nodeObject = new Node();
            Connector connectorObject = new Connector();
            IDiagramObject cloneObject;
            if (this.diagram.DrawingObject is Node drawingObject)
            {
                bool polygonNode = drawingObject.Shape is BasicShape { Shape: BasicShapeType.Polygon };
                if (this.diagram.SelectionSettings.Nodes.Count == 0 || !polygonNode)
                {
                    this.diagram.DiagramAction |= DiagramAction.DrawingTool;
                    nodeObject.OffsetX = polygonNode ? position.X : point.X;
                    nodeObject.OffsetY = polygonNode ? position.Y : point.Y;
                    nodeObject.Width = polygonNode ? 1 : 0;
                    nodeObject.Height = polygonNode ? 1 : 0;
                    diagram.DiagramContent.InitObject(nodeObject);
                    if (polygonNode)
                    {
                        List<DiagramPoint> pts = new List<DiagramPoint>();
                        DiagramPoint pt = new DiagramPoint() { X = position.X, Y = position.Y };
                        pts.Add(pt);
                        if (nodeObject.Shape is BasicShape basicShape)
                        {
                            basicShape.Points = points;
                            DiagramPoint[] newPoint = basicShape.Points;
                            basicShape.PolygonPath = PathUtil.GetPolygonPath(newPoint);
                            this.PolygonObject ??= new PathElement();
                            this.PolygonObject.Data = basicShape.PolygonPath;
                        }
                    }
                }
                else if (polygonNode)
                {
                    _ = this.RenderHelper();
                    nodeObject = this.diagram.SelectionSettings.Nodes[^1];
                    if (nodeObject.Shape is BasicShape basicShape)
                    {
                        List<DiagramPoint> pts = basicShape.Points.ToList();
                        pts.Add(new DiagramPoint() { X = position.X, Y = position.Y });
                        basicShape.Points = pts.ToArray();
                    }
                }
                cloneObject = nodeObject;
            }
            else
            {
                DiagramPoint currentPosition = this.SnapConnectorEnd(position);
                if (this.diagram.DrawingObject is Connector connector && connector.Type != ConnectorSegmentType.Polyline)
                {
                    connectorObject.Type = connector.Type;
                }
                connectorObject.TargetPoint.X = connectorObject.SourcePoint.X = currentPosition.X;
                connectorObject.TargetPoint.Y = connectorObject.SourcePoint.Y = currentPosition.Y;
                this.diagram.DiagramContent.InitObject(connectorObject);
                cloneObject = connectorObject;
                this.CurrentDrawingObject = cloneObject as Connector;
            }
            this.Select(cloneObject);
            return cloneObject;
        }
        internal void ShowTooltip(string content, bool isTooltipVisible)
        {
            if (isTooltipVisible)
            {
                _ = diagram.DiagramTooltip.Open();
                diagram.TooltipAnimation = new Popups.AnimationModel()
                {
                    Open = new Popups.TooltipAnimationSettings() { Effect = Popups.Effect.None },
                    Close = new Popups.TooltipAnimationSettings() { Effect = Popups.Effect.None }
                };
            }
            diagram.TooltipContent = content;
            _ = diagram.DiagramTooltip.RefreshPosition();
        }
        internal void CloseTooltip()
        {
            _ = diagram.DiagramTooltip.Close();
        }
        internal void ClearObjectSelection(IDiagramObject mouseDownElement)
        {
            ObservableCollection<IDiagramObject> list = this.GetSelectedObject();
            if (list.IndexOf(mouseDownElement) == -1)
            {
                ClearSelection((list.Count > 0));
                SelectObjects(new ObservableCollection<IDiagramObject> { mouseDownElement }, true);
            }
        }

        internal void DragOverElement(DiagramMouseEventArgs args, DiagramPoint currentPosition)
        {
            if (this.diagram.PaletteInstance != null && this.diagram.PaletteInstance.SelectedSymbol != null)
            {
                DraggingEventArgs arg = new DraggingEventArgs()
                {
                    Element = args.Element,
                    Position = currentPosition
                };
                this.InvokeDiagramEvents(DiagramEvent.Dragging, arg);
            }
        }

        internal void ClearSelection(bool? triggerEvent = false)
        {
            if (HasSelection())
            {
                DiagramSelectionSettings selectorModel = this.diagram.SelectionSettings;
                ObservableCollection<IDiagramObject> arrayNodes = this.GetSelectedObject();
                SelectionChangingEventArgs arg = new SelectionChangingEventArgs
                {
                    Cancel = false,
                    OldValue = arrayNodes,
                    NewValue = new ObservableCollection<IDiagramObject>(),
                    ActionTrigger = diagram.DiagramAction,
                    Type = CollectionChangedAction.Remove
                };
                if (triggerEvent != null && triggerEvent.Value)
                {
                    InvokeDiagramEvents(DiagramEvent.SelectionChanging, arg);
                }
                if (!arg.Cancel)
                {
                    selectorModel.OffsetX = 0;
                    selectorModel.OffsetY = 0;
                    selectorModel.Width = 0;
                    selectorModel.Height = 0;
                    selectorModel.RotationAngle = 0;
                    selectorModel.Nodes = new ObservableCollection<Node>();
                    selectorModel.Connectors = new ObservableCollection<Connector>();
                    selectorModel.Wrapper = null;
                    if (triggerEvent != null && triggerEvent.Value)
                    {
                        SelectionChangedEventArgs selectionChangedEventArgs = new SelectionChangedEventArgs
                        {
                            OldValue = arrayNodes,
                            NewValue = new ObservableCollection<IDiagramObject>(),
                            ActionTrigger = diagram.DiagramAction,
                            Type = CollectionChangedAction.Remove
                        };
                        InvokeDiagramEvents(DiagramEvent.SelectionChanged, selectionChangedEventArgs);
                    }
                }
            }
        }

        internal void ClearSelectedItems()
        {
            int selectedNodes = this.diagram.SelectionSettings.Nodes?.Count ?? 0;
            int selectedConnectors = this.diagram.SelectionSettings.Connectors?.Count ?? 0;
            this.ClearSelection((selectedNodes + selectedConnectors) > 0);
        }

        internal bool HasSelection()
        {
            return Internal.ActionsUtil.HasSelection(this.diagram);
        }

        internal bool IsSelected(IDiagramObject element)
        {
            return Internal.ActionsUtil.IsSelected(this.diagram, element);
        }
        internal void RotateObjects(IDiagramObject parent, List<NodeBase> objects, double angle, DiagramPoint pivot, bool includeParent = false)
        {
            bool canUndoRotatedObject = includeParent;
            Matrix matrix = Matrix.IdentityMatrix();
            Matrix.RotateMatrix(matrix, angle, pivot.X, pivot.Y);
            diagram.RealAction |= RealAction.EnableGroupAction | RealAction.PreventRefresh;
            bool isProtectChange = SfDiagramComponent.IsProtectedOnChange;
            SfDiagramComponent.IsProtectedOnChange = true;
            for (int i = 0; i < objects.Count; i++)
            {
                NodeBase obj = objects[i];
                if (obj is Node node)
                {
                    if (ConstraintsUtil.CanRotate(obj) && ConstraintsUtil.CanPageEditable(diagram))
                    {
                        if (diagram.DiagramAction.HasFlag(DiagramAction.UndoRedo))
                        {
                            includeParent = true;
                        }
                        else
                        {
                            includeParent = canUndoRotatedObject;
                        }
                        if (includeParent || parent != obj)
                        {
                            node.RotationAngle += angle;
                            node.RotationAngle = (node.RotationAngle + 360) % 360;
                            DiagramPoint newOffset = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = node.OffsetX, Y = node.OffsetY });
                            node.OffsetX = newOffset.X;
                            node.OffsetY = newOffset.Y;
                        }
                        if (!string.IsNullOrEmpty(node.ProcessId) && this.diagram.NameTable.ContainsKey(node.ProcessId))
                        {
                            Node processParent = this.diagram.NameTable[node.ProcessId] as Node;
                            _ = BpmnDiagrams.GetChildrenBound(processParent, node.ID, this.diagram);
                            BpmnDiagrams.UpdateSubProcesses(node, this.diagram);
                        }

                        if (node is NodeGroup grp && grp.Children != null && grp.Children.Length > 0)
                        {
                            this.GetChildren(grp, objects);
                        }
                    }
                }
                else if (obj is Connector)
                {
                    RotatePoints(obj as Connector, angle, pivot ?? new DiagramPoint() { X = obj.Wrapper.OffsetX, Y = obj.Wrapper.OffsetY });
                }
            }
            SfDiagramComponent.IsProtectedOnChange = isProtectChange;
            diagram.RealAction &= ~(RealAction.EnableGroupAction | RealAction.PreventRefresh);
        }
        /// <summary>
        /// Copy method.
        /// </summary>
        /// <returns></returns>
        internal void Copy()
        {
            this.clipboardData.PasteIndex = 1;
            this.clipboardData.ClipObject = this.CopyObjects();
        }
        /// <summary>
        /// copyObjects method
        /// </summary>
        /// <returns></returns>
        internal List<NodeBase> CopyObjects()
        {
            NodeBase[] selectedItems = Array.Empty<NodeBase>();
            List<NodeBase> objList = new List<NodeBase>();
            this.clipboardData.ChildTable = new Dictionary<string, IDiagramObject>();

            ObservableCollection<Node> nodes = this.diagram.SelectionSettings.Nodes;
            ObservableCollection<Connector> connectors = this.diagram.SelectionSettings.Connectors;
            if (nodes.Any())
            {
                selectedItems = nodes.ToArray();
                for (int j = 0; j < nodes.Count; j++)
                {
                    Node node = nodes[j].Clone() as Node;
                    if (node.Wrapper != null && (!node.OffsetX.Equals(node.Wrapper.OffsetX)))
                    {
                        node.OffsetX = node.Wrapper.OffsetX;
                    }
                    if (node.Wrapper != null && (!node.OffsetY.Equals(node.Wrapper.OffsetY)))
                    {
                        node.OffsetY = node.Wrapper.OffsetY;
                    }
                    this.CopyProcesses(node);
                    objList.Add(node.Clone() as Node);
                    Matrix matrix = Matrix.IdentityMatrix();
                    Matrix.RotateMatrix(matrix, -node.RotationAngle, node.OffsetX, node.OffsetY);
                    if (node is NodeGroup group && group.Children != null && group.Children.Length > 0)
                    {
                        Dictionary<string, IDiagramObject> childTable = this.clipboardData.ChildTable;
                        List<NodeBase> elements = new List<NodeBase>();
                        List<NodeBase> childNodes = this.GetAllDescendants(group, elements, true);
                        for (int i = 0; i < childNodes.Count; i++)
                        {
                            IDiagramObject tempNode = this.diagram.NameTable[childNodes[i].ID];
                            NodeBase clonedObject = tempNode.Clone() as NodeBase;
                            if (tempNode is NodeBase nodeBase)
                                childTable[nodeBase.ID] = clonedObject;

                            if (tempNode is Node)
                            {
                                if (clonedObject is Node element)
                                {
                                    DiagramPoint newOffset = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = element.OffsetX, Y = element.OffsetY });
                                    element.OffsetX = newOffset.X;
                                    element.OffsetY = newOffset.Y;
                                    element.RotationAngle -= group.RotationAngle;
                                }
                            }
                        }
                        this.clipboardData.ChildTable = childTable;
                    }
                }
            }
            if (connectors.Any())
            {
                selectedItems = selectedItems.Concat(connectors).ToArray();
                for (int j = 0; j < selectedItems.Length; j++)
                {
                    if (selectedItems[j] is Connector)
                    {
                        Connector element = null;
                        if (this.diagram.BpmnDiagrams != null &&
                            this.diagram.DiagramContent.TextAnnotationConnectors.IndexOf(selectedItems[j] as Connector) > -1)
                        {
                            string targetId = (selectedItems[j] as Connector)?.TargetID;
                            if (targetId != null)
                                element = (this.diagram.NameTable[targetId]).Clone() as Connector;
                        }
                        else
                        {
                            element = (selectedItems[j] as Connector)?.Clone() as Connector;
                        }
                        if (element != null) objList.Add(element);
                    }
                }
            }
            if (this.clipboardData.PasteIndex == 0)
            {
                foreach (NodeBase item in selectedItems)
                {
                    if (this.diagram.NameTable.ContainsKey(item.ID) && this.diagram.NameTable[item.ID] != null)
                    {
                        if (this.diagram.BpmnDiagrams != null && item is Connector connector &&
                            this.diagram.DiagramContent.TextAnnotationConnectors.IndexOf(connector) > -1)
                        {
                            this.diagram.DiagramContent.TextAnnotationConnectors.Remove(this.diagram.NameTable[connector.TargetID] as Connector);
                        }
                        else if (item is Node node)
                        {
                            if (node.Shape.Type == Shapes.Bpmn && (node.Shape as BpmnShape).Activity?.SubProcess.Processes != null && (node.Shape as BpmnShape).Activity.SubProcess.Processes.Count > 0)
                            {
                                this.diagram.BpmnDiagrams.RemoveBpmnProcesses(node, this.diagram);
                            }
                            this.diagram.Nodes.Remove(node);
                        }
                        else if (item is Connector connector1)
                        {
                            this.diagram.Connectors.Remove(connector1);
                        }
                    }
                }
                diagram.DiagramStateHasChanged();
            }
            return objList;
        }

        /// <summary>
        /// Cut method.
        /// </summary>
        internal void Cut()
        {
            bool groupAction = false;
            this.clipboardData.PasteIndex = 0;
            if (diagram.Constraints.HasFlag(DiagramConstraints.UndoRedo))
            {
                diagram.StartGroupAction();
                groupAction = true;
            }
            this.clipboardData.ClipObject = this.CopyObjects();
            if (groupAction)
            {
                diagram.EndGroupAction();
            }
        }
        private Connector CloneConnector(Connector connector, bool multiSelect)
        {
            if (connector.Clone() is Connector cloneObject)
            {
                cloneObject.ParentId = string.Empty;
                this.TranslateObject(cloneObject);
                clonedItems.Add(cloneObject);
                Connector newConnector = cloneObject;
                this.SelectObjects(new ObservableCollection<IDiagramObject>() { newConnector }, multiSelect);
                return newConnector;
            }
            return null;
        }
        internal void TranslateObject(NodeBase obj, string groupNodeId = "")
        {
            obj.ID += string.IsNullOrEmpty(groupNodeId) ? BaseUtil.RandomId() : groupNodeId;
            double diff = this.clipboardData.PasteIndex * 10;
            if (obj is Connector connector)
            {
                connector.SourcePoint = new DiagramPoint() { X = connector.SourcePoint.X + diff, Y = connector.SourcePoint.Y + diff };
                connector.TargetPoint = new DiagramPoint() { X = connector.TargetPoint.X + diff, Y = connector.TargetPoint.Y + diff };
                if (connector.Type == ConnectorSegmentType.Bezier)
                {
                    DiagramObjectCollection<ConnectorSegment> connectorSegments = connector.Segments;
                    for (int i = 0; i < connectorSegments.Count; i++)
                    {
                        BezierSegment segment = connectorSegments[i] as BezierSegment;
                        if (segment != null && !DiagramPoint.IsEmptyPoint(segment.Point1))
                        {
                            segment.Point1 = new DiagramPoint()
                            {
                                X = segment.Point1.X + diff,
                                Y = segment.Point1.Y + diff
                            };
                        }
                        if (segment != null && !DiagramPoint.IsEmptyPoint(segment.Point2))
                        {
                            segment.Point2 = new DiagramPoint()
                            {
                                X = segment.Point2.X + diff,
                                Y = segment.Point2.Y + diff
                            };
                        }
                    }
                }
                if (connector.Type == ConnectorSegmentType.Straight || connector.Type == ConnectorSegmentType.Bezier)
                {
                    if (connector.Segments != null && connector.Segments.Count > 0)
                    {
                        DiagramObjectCollection<ConnectorSegment> segments = connector.Segments;
                        for (int i = 0; i < segments.Count - 1; i++)
                        {
                            if (segments[i] is StraightSegment straightSegment)
                            {
                                straightSegment.Point.X += diff;
                                straightSegment.Point.Y += diff;
                            }
                            else if (segments[i] is BezierSegment bezierSegment)
                            {
                                bezierSegment.Point.X += diff;
                                bezierSegment.Point.Y += diff;
                            }
                        }
                    }
                }
            }
            else
            {
                ((Node)obj).OffsetX += diff;
                ((Node)obj).OffsetY += diff;
            }
        }
        private async Task<Node> CloneGroup(NodeGroup obj, bool multiSelect)
        {
            List<string> newChildren = new List<string>();
            string[] children = Array.Empty<string>();
            DiagramObjectCollection<Connector> connectorObj = new DiagramObjectCollection<Connector>();
            NodeBase newObj;
            DiagramObjectCollection<string> oldId = new DiagramObjectCollection<string>();
            children = children.Concat(obj.Children).ToArray();
            string id = Guid.NewGuid().ToString();
            DiagramObjectCollection<NodeBase> objectCollection = new DiagramObjectCollection<NodeBase>();
            if (this.clipboardData.ChildTable != null || obj.Children.Length > 0)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    NodeBase childObj;
                    if (this.clipboardData.ChildTable != null)
                    {
                        childObj = this.clipboardData.ChildTable[children[i]] as NodeBase;
                    }
                    else
                    {
                        childObj = this.diagram.NameTable[children[i]] as NodeBase;
                    }
                    if (childObj is Node node)
                        node.ParentId = string.Empty;

                    if (childObj != null)
                    {
                        if (childObj is Connector connector)
                        {
                            connectorObj.Add(connector);
                        }
                        else
                        {
                            newObj = await CloneNode(childObj as Node, multiSelect, null, id);
                            oldId.Add(childObj.ID);
                            newChildren.Add(newObj.ID);
                            objectCollection.Add(newObj);
                        }
                    }
                }
            }
            for (int k = 0; k < connectorObj.Count; k++)
            {
                if (!string.IsNullOrEmpty(connectorObj[k].SourceID) || !string.IsNullOrEmpty(connectorObj[k].TargetID))
                {
                    for (int j = 0; j < oldId.Count; j++)
                    {
                        if (connectorObj[k].SourceID == oldId[j])
                        {
                            connectorObj[k].SourceID += id;
                        }
                        if (connectorObj[k].TargetID == oldId[j])
                        {
                            connectorObj[k].TargetID += id;
                        }
                    }
                }
                newObj = CloneConnector(connectorObj[k], multiSelect);
                newChildren.Add(newObj.ID);
                objectCollection.Add(newObj);
            }
            Node parentObj = await this.CloneNode(obj, multiSelect, newChildren);
            objectCollection.Add(parentObj);
            return parentObj;
        }

        private async Task<Node> CloneNode(Node node, bool multiSelect, List<string> children = null, string groupNodeID = "", bool isProcess = false)
        {
            Node newNode = null;
            if (node.Clone() is Node cloneObject)
            {
                cloneObject.ParentId = string.Empty;
                DiagramObjectCollection<string> process = null;
                if (node.Shape is BpmnShape bpmnShape && node.Shape.Type == Shapes.Bpmn && bpmnShape.Activity != null &&
                    bpmnShape.Activity.SubProcess.Processes != null && bpmnShape.Activity.SubProcess.Processes.Any())
                {
                    process = (cloneObject.Shape as BpmnShape)?.Activity.SubProcess.Processes;
                    ((BpmnShape)cloneObject.Shape).Activity.SubProcess.Processes = null;
                }

                if (node is NodeGroup group && @group.Children != null && @group.Children.Length > 0 && (children == null))
                {
                    newNode = await this.CloneGroup(@group, multiSelect);
                }
                else
                {
                    this.TranslateObject(cloneObject, groupNodeID);
                    if (children != null && children.Count > 0 && cloneObject is NodeGroup)
                    {
                        (cloneObject as NodeGroup).Children = children.ToArray();
                    }

                    if (!isProcess)
                        clonedItems.Add(cloneObject);
                    newNode = cloneObject;
                }

                if (process != null && process.Count > 0)
                {
                    ((BpmnShape)newNode.Shape).Activity.SubProcess.Processes = process;
                    await this.CloneSubProcesses(newNode);
                }
            }

            if (newNode != null && !isProcess)
            {
                this.SelectObjects(new ObservableCollection<IDiagramObject>() { newNode }, multiSelect);
            }
            return newNode;
        }
        private async Task CloneSubProcesses(Node node)
        {
            diagram.BeginUpdate();
            DiagramObjectCollection<string> connector = new DiagramObjectCollection<string>();
            Dictionary<string, string> temp = new Dictionary<string, string>();

            if (node.Shape is BpmnShape bpmnShape && bpmnShape.Type == Shapes.Bpmn && bpmnShape.Activity?.SubProcess.Processes != null && bpmnShape.Activity.SubProcess.Processes.Any())
            {
                DiagramObjectCollection<string> process = bpmnShape.Activity.SubProcess.Processes;
                DiagramObjectCollection<NodeBase> processCollection = new DiagramObjectCollection<NodeBase>();
                DiagramObjectCollection<Node> processNodeCollection = new DiagramObjectCollection<Node>();
                if (process != null)
                {
                    for (int g = 0; g < process.Count; g++)
                    {
                        Node child = this.diagram.NameTable.ContainsKey(process[g])
                            ? this.diagram.NameTable[process[g]] as Node
                            : this.clipboardData.ProcessTable[process[g]] as Node;
                        if (child != null)
                        {
                            foreach (string j in child.OutEdges)
                            {
                                if (connector.IndexOf(j) < 0)
                                {
                                    connector.Add(j);
                                }
                            }

                            foreach (string j in child.InEdges)
                            {
                                if (connector.IndexOf(j) < 0)
                                {
                                    connector.Add(j);
                                }
                            }
                        }

                        if (this.clipboardData.ProcessTable[process[g]].Clone() is Node innerChild)
                        {
                            innerChild.ProcessId = node.ID;
                            Node newNode = await this.CloneNode(innerChild, false, null, string.Empty, true);
                            temp[process[g]] = newNode.ID;
                            process[g] = newNode.ID;
                            processCollection.Add(newNode);
                            processNodeCollection.Add(newNode);
                        }

                        foreach (string str in connector)
                        {
                            Connector con = this.diagram.NameTable.ContainsKey(str)
                                ? this.diagram.NameTable[str] as Connector
                                : this.clipboardData.ProcessTable[str] as Connector;
                            if (con?.Clone() is Connector clonedConnector)
                            {
                                clonedConnector.ID += BaseUtil.RandomId();
                                if (temp.ContainsKey(clonedConnector.SourceID))
                                {
                                    clonedConnector.SourceID = temp[clonedConnector.SourceID];
                                }

                                if (temp.ContainsKey(clonedConnector.TargetID))
                                {
                                    clonedConnector.TargetID = temp[clonedConnector.TargetID];
                                }

                                processCollection.Add(clonedConnector);
                            }
                        }
                    }
                }
                clonedItems.Remove(node);
                processCollection.Add(node);
                await diagram.AddDiagramElements(processCollection);
                foreach (Node key in processNodeCollection)
                {
                    this.diagram.BpmnDiagrams.AddBpmnProcesses(key, node.ID, this.diagram);
                }
            }
            await diagram.EndUpdate();
        }
        internal async Task Paste()
        {
            clonedItems = new DiagramObjectCollection<NodeBase>();
            if (this.clipboardData.ClipObject != null)
            {
                bool isProtect = SfDiagramComponent.IsProtectedOnChange;
                SfDiagramComponent.IsProtectedOnChange = false;
                List<NodeBase> copiedItems = this.clipboardData.ClipObject;
                if (copiedItems.Count > 0)
                {
                    bool groupAction = false;
                    bool multiSelect = copiedItems.Count != 1;
                    Dictionary<string, NodeBase> objectTable = new Dictionary<string, NodeBase>();
                    Dictionary<string, string> keyTable = new Dictionary<string, string>();
                    DiagramObjectCollection<IDiagramObject> copiedObject = new DiagramObjectCollection<IDiagramObject>();

                    if (diagram.Constraints.HasFlag(DiagramConstraints.UndoRedo))
                    {
                        diagram.StartGroupAction();
                        groupAction = true;
                    }

                    if (this.clipboardData.PasteIndex != 0)
                    {
                        this.ClearSelection();
                    }

                    foreach (var copy in copiedItems)
                    {
                        objectTable[copy.ID] = copy;
                    }
                    for (int j = 0; j < copiedItems.Count; j++)
                    {
                        NodeBase copy = copiedItems[j];
                        if (copy is Connector)
                        {
                            Connector clonedObj = copy.Clone() as Connector;
                            string nodeId = clonedObj.SourceID;
                            clonedObj.SourceID = string.Empty;
                            if (!string.IsNullOrEmpty(nodeId) && objectTable.ContainsKey(nodeId) && keyTable.ContainsKey(nodeId))
                            {
                                clonedObj.SourceID = keyTable[nodeId];
                            }
                            nodeId = clonedObj.TargetID;
                            clonedObj.TargetID = string.Empty;
                            if (!string.IsNullOrEmpty(nodeId) && objectTable.ContainsKey(nodeId) && keyTable.ContainsKey(nodeId))
                            {
                                clonedObj.TargetID = keyTable[nodeId];
                            }
                            Connector newObj = this.CloneConnector(clonedObj, multiSelect);
                            copiedObject.Add(newObj);
                            keyTable[copy.ID] = newObj.ID;
                        }
                        else
                        {
                            Node newNode = await this.CloneNode(copy as Node, multiSelect);
                            copiedObject.Add(newNode);
                            if (newNode != null)
                            {
                                keyTable[copy.ID] = newNode.ID;
                                List<string> edges = (copy as Node).InEdges;
                                if (edges != null)
                                {
                                    foreach (string edge in edges)
                                    {
                                        if (objectTable[edge] != null && keyTable[edge] != null)
                                        {
                                            Connector newConnector = this.diagram.NameTable[keyTable[edge]] as Connector;
                                            newConnector.TargetID = keyTable[copy.ID];
                                        }
                                    }
                                }
                                edges = (copy as Node).OutEdges;
                                if (edges != null)
                                {
                                    foreach (string edge in edges)
                                    {
                                        if (objectTable[edge] != null && keyTable[edge] != null)
                                        {
                                            Connector newConnector = this.diagram.NameTable[keyTable[edge]] as Connector;
                                            newConnector.SourceID = keyTable[copy.ID];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    await diagram.AddDiagramElements(clonedItems);
                    if (groupAction)
                    {
                        diagram.EndGroupAction();
                    }
                    this.SelectObjects(new ObservableCollection<IDiagramObject>() { clonedItems[^1] }, multiSelect);
                    if (multiSelect)
                    {
                        this.diagram.Select(copiedObject, true);
                    }
                    this.clipboardData.PasteIndex++;
                }
                SfDiagramComponent.IsProtectedOnChange = isProtect;
            }
            this.diagram.DiagramStateHasChanged();
        }

        private void CopyProcesses(Node node)
        {
            if (node.Shape is BpmnShape bpmnShape && bpmnShape.Type == Shapes.Bpmn && bpmnShape.Activity != null && bpmnShape.Activity.SubProcess.Processes != null && bpmnShape.Activity.SubProcess.Processes.Any())
            {
                DiagramObjectCollection<string> processes = bpmnShape.Activity.SubProcess.Processes;
                if (processes != null)
                {
                    foreach (string str in processes)
                    {
                        this.processTable[str] = this.diagram.NameTable[str].Clone() as Node;
                        if (((Node)this.processTable[str])?.Shape is BpmnShape bpmnProcessShape && bpmnProcessShape.Activity.SubProcess.Processes != null && bpmnProcessShape.Activity.SubProcess.Processes.Any())
                        {
                            this.CopyProcesses(this.processTable[str] as Node);
                        }
                    }
                }
                this.clipboardData.ProcessTable = this.processTable;
            }
        }
        private static void RotatePoints(Connector conn, double angle, DiagramPoint pivot)
        {
            if (conn.SourceWrapper == null || conn.TargetWrapper == null)
            {
                Matrix matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, angle, pivot.X, pivot.Y);
                conn.SourcePoint = Matrix.TransformPointByMatrix(matrix, conn.SourcePoint);
                conn.TargetPoint = Matrix.TransformPointByMatrix(matrix, conn.TargetPoint);
                if (conn.Shape.Type == ConnectorShapeType.Bpmn && (conn.Shape as BpmnFlow).Sequence == BpmnSequenceFlows.Default)
                {
                    UpdatePathElementOffset(conn);
                }
            }
        }
        internal List<NodeBase> GetChildren(NodeGroup node, List<NodeBase> nodes)
        {
            if (node.Children != null)
            {
                for (int i = 0; i < node.Children.Length; i++)
                {
                    if (this.diagram.NameTable.ContainsKey(node.Children[i]))
                    {
                        NodeBase child = this.diagram.NameTable[node.Children[i]] as NodeBase;
                        nodes.Add(child);
                    }
                }
            }
            return nodes;
        }
        internal double SnapAngle(double angle)
        {
            if (this.diagram.SnapSettings != null && (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToVerticalLines)) || this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToHorizontalLines))
            {
                return this.diagram.Snapping.SnapAngle(angle);
            }
            else
            {
                return 0;
            }
        }
        internal bool RotateSelectedItems(double angle)
        {
            IDiagramObject obj = this.diagram.SelectionSettings;
            return this.diagram.Rotate(obj, angle);
        }
        internal async Task AddObjectToDiagram(Node polygonNode = null)
        {
            IDiagramObject obj;
            IDiagramObject drawingObject = this.diagram.DrawingObject;
            DiagramSelectionSettings helperObject = this.HelperObject;
            if (helperObject != null)
            {
                if (this.diagram.DrawingObject is Node)
                {
                    bool isPolygonNode = drawingObject is Node { Shape: BasicShape { Shape: BasicShapeType.Polygon } };
                    Node cloneNode = isPolygonNode ? polygonNode : drawingObject as Node;
                    Node node = await this.CloneNode(cloneNode, false);
                    if (isPolygonNode)
                    {
                        DiagramRect nodeBounds = DiagramRect.ToBounds((node.Shape as BasicShape).Points.ToList());
                        node.Width = nodeBounds.Width;
                        node.Height = nodeBounds.Height;
                        node.OffsetX = nodeBounds.Center.X;
                        node.OffsetY = nodeBounds.Center.Y;
                    }
                    else
                    {
                        node.Width = helperObject.Width;
                        node.Height = helperObject.Height;
                        node.OffsetX = helperObject.OffsetX;
                        node.OffsetY = helperObject.OffsetY;
                    }
                    obj = node;
                }
                else
                {
                    Connector helperConnector = helperObject.Connectors[0];
                    var connector = this.CloneConnector(drawingObject as Connector, false);
                    if (diagram.DrawingObject is Connector { Type: ConnectorSegmentType.Polyline })
                    {
                        connector.Type = ConnectorSegmentType.Straight;
                        int i = (helperConnector.SegmentCollection.Count) - 1;
                        helperConnector.SegmentCollection.Remove(helperConnector.SegmentCollection[i]);
                    }
                    connector.SegmentCollection = helperConnector.SegmentCollection;
                    connector.SourcePoint = new DiagramPoint() { X = helperConnector.SourcePoint.X, Y = helperConnector.SourcePoint.Y };
                    connector.TargetPoint = new DiagramPoint() { X = helperConnector.TargetPoint.X, Y = helperConnector.TargetPoint.Y };
                    connector.SourceID = helperConnector.SourceID;
                    connector.SourcePortID = helperConnector.SourcePortID;
                    connector.TargetID = helperConnector.TargetID;
                    connector.TargetPortID = helperConnector.TargetPortID;
                    obj = connector;
                }

                await diagram.AddDiagramElements(new DiagramObjectCollection<NodeBase>() { obj as NodeBase });
                this.SelectObjects(new ObservableCollection<IDiagramObject>() { clonedItems[^1] }, false);
                this.diagram.UpdateDrawingObject(await Blazor.Internal.SfBaseUtils.UpdateProperty(ConstraintsUtil.CanDrawOnce(diagram) ? null : this.diagram.DrawingObject, this.diagram.DrawingObject, diagram.DrawingObjectChanged, null, null));
                this.HelperObject = null;
                this.PolygonObject = null;
                if (diagram.DrawingObject == null)
                {
                    diagram.DrawingObjectTool = null;
                }
                this.diagram.DiagramStateHasChanged();
            }
        }

        internal void Select(IDiagramObject obj, bool? multipleSelection = null)
        {
            if (ConstraintsUtil.CanSelect(obj) && !(obj is DiagramSelectionSettings) && !ActionsUtil.IsSelected(this.diagram, obj)
                && (obj as NodeBase) != null && ((NodeBase)obj).Wrapper != null && ((NodeBase)obj).Wrapper.Visible)
            {
                multipleSelection = HasSelection() ? multipleSelection : false;
                if (multipleSelection != null && !multipleSelection.Value)
                {
                    this.ClearSelection();
                }
                DiagramSelectionSettings selectorModel = this.diagram.SelectionSettings;
                if (obj is Node)
                {
                    selectorModel.Nodes.Add(obj as Node);
                }
                else
                {
                    selectorModel.Connectors.Add(obj as Connector);
                }
                if (multipleSelection != null && !multipleSelection.Value)
                {
                    selectorModel.Init(diagram);
                    if (selectorModel.Nodes.Count == 1 && selectorModel.Connectors.Count == 0)
                    {
                        selectorModel.RotationAngle = selectorModel.Nodes[0].RotationAngle;
                        selectorModel.Wrapper.RotationAngle = selectorModel.Nodes[0].RotationAngle;
                        selectorModel.Wrapper.Pivot = selectorModel.Nodes[0].Pivot;
                    }
                }
                else
                {
                    selectorModel.Wrapper.RotationAngle = selectorModel.RotationAngle = 0;
                    selectorModel.Wrapper.Children.Add(((NodeBase)obj).Wrapper);
                }
                DiagramContainer container = selectorModel.Wrapper;
                container.Measure(new DiagramSize());
                container.Arrange(container.DesiredSize);
                if (container.ActualSize.Width != null) selectorModel.Width = container.ActualSize.Width.Value;
                selectorModel.Height = container.ActualSize.Height.Value;
                selectorModel.OffsetX = container.OffsetX;
                selectorModel.OffsetY = container.OffsetY;
            }
        }

        internal ObservableCollection<IDiagramObject> GetSelectedObject()
        {
            DiagramSelectionSettings selectorModel = this.diagram.SelectionSettings;
            ObservableCollection<IDiagramObject> selectedObjects = new ObservableCollection<IDiagramObject>();
            for (int i = 0; i < selectorModel.Nodes.Count; i++)
            {
                selectedObjects.Add(selectorModel.Nodes[i]);
            }
            for (int i = 0; i < selectorModel.Connectors.Count; i++)
            {
                selectedObjects.Add(selectorModel.Connectors[i]);
            }
            return selectedObjects;

        }

        internal void UnSelect(IDiagramObject obj)
        {
            if (IsSelected(obj))
            {
                DiagramSelectionSettings selectorModel = this.diagram.SelectionSettings;
                int index;
                if (obj is Node node)
                {
                    index = selectorModel.Nodes.IndexOf(node);
                    selectorModel.Nodes.RemoveAt(index);
                }
                else
                {
                    index = selectorModel.Connectors.IndexOf(obj as Connector);
                    selectorModel.Connectors.RemoveAt(index);
                }
                index = selectorModel.Wrapper.Children.IndexOf((obj as NodeBase)?.Wrapper);
                selectorModel.Wrapper.Children.RemoveAt(index);
            }
        }

        internal void DrawSelectionRectangle(DiagramRect rect)
        {
            this.diagram.SelectionSettings.IsRubberBandSelection = true;
            this.diagram.SelectionSettings.RubberBandBounds = rect;
            this.RefreshDiagram();
        }

        internal void DoRubberBandSelection(DiagramRect region)
        {
            this.diagram.SelectionSettings.IsRubberBandSelection = false;
            this.diagram.SelectionSettings.RubberBandBounds = new DiagramRect(0, 0, 0, 0);
            ObservableCollection<IDiagramObject> selArray = new ObservableCollection<IDiagramObject>();
            ObservableCollection<IDiagramObject> rubberArray;
            selArray = this.diagram.DiagramContent.GetNodesConnectors(selArray);
            if (this.diagram.SelectionSettings.RubberBandSelectionMode == RubberBandSelectionMode.CompleteIntersect)
            {
                rubberArray = DiagramUtil.CompleteRegion(region, selArray);
            }
            else
            {
                rubberArray = this.diagram.SpatialSearch.FindObjects(region);
            }
            if (rubberArray != null && rubberArray.Count > 0)
            {
                SelectObjects(rubberArray, true);
            }
            RefreshDiagram();
        }

        internal void RefreshDiagram()
        {
            bool preventRefresh = false;
            if (diagram.EventHandler.Action == Actions.Select && !diagram.RealAction.HasFlag(RealAction.PreventRefresh))
            {
                diagram.RealAction |= RealAction.PreventRefresh;
                preventRefresh = true;
            }
            diagram.DiagramStateHasChanged();
            if (preventRefresh)
            {
                diagram.RealAction &= ~RealAction.PreventRefresh;
            }
        }

        /// <summary>
        ///When select group children as rubber band selection, check the node is in selected item.
        /// </summary>
        private static bool IsGroupObjects(ObservableCollection<IDiagramObject> objects, string id)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if ((objects[i] as NodeBase)?.ID == id)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        ///Get selected item, when select item as rubber band selection.
        /// </summary>
        private static ObservableCollection<IDiagramObject> GetSelectionItems(ObservableCollection<IDiagramObject> objects)
        {
            ObservableCollection<IDiagramObject> temp = new ObservableCollection<IDiagramObject>();
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is NodeBase nodeBase)
                {
                    if (string.IsNullOrEmpty(nodeBase.ParentId) || !string.IsNullOrEmpty(nodeBase.ParentId) && !IsGroupObjects(objects, nodeBase.ParentId))
                    {
                        temp.Add(objects[i]);
                    }
                }
            }
            return temp;
        }
        internal void SelectObjects(ObservableCollection<IDiagramObject> objects, bool? multipleSelection = false, ObservableCollection<IDiagramObject> oldSelectedObjects = null)
        {
            objects = GetSelectionItems(objects);
            oldSelectedObjects ??= this.GetSelectedObject();
            SelectionChangingEventArgs selectionChangingEventArgs = new SelectionChangingEventArgs()
            {
                OldValue = oldSelectedObjects,
                NewValue = objects,
                Cancel = false,
                ActionTrigger = diagram.DiagramAction,
                Type = CollectionChangedAction.Add
            };
            InvokeDiagramEvents(DiagramEvent.SelectionChanging, selectionChangingEventArgs);

            bool canDoMultipleSelection = ConstraintsUtil.CanMultiSelect(diagram);
            bool canDoSingleSelection = ConstraintsUtil.CanSingleSelect(diagram);

            if (!selectionChangingEventArgs.Cancel)
            {
                if (multipleSelection != null && !multipleSelection.Value)
                {
                    ClearSelection(objects.Count == 0);
                }
                if (canDoSingleSelection || canDoMultipleSelection)
                {
                    if (multipleSelection != null && !canDoMultipleSelection && ((objects.Count > 1) || (multipleSelection.Value && objects.Count == 1)))
                    {
                        if (objects.Count == 1)
                        {
                            ClearSelection();
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (multipleSelection != null && !canDoSingleSelection && objects.Count == 1 && (!multipleSelection.Value || !HasSelection()))
                    {
                        ClearSelection();
                        return;
                    }
                }

                for (int i = 0; i < objects.Count; i++)
                {
                    IDiagramObject newObj = objects[i];
                    if (newObj != null)
                    {
                        if (!ActionsUtil.HasSelection(this.diagram))
                        {
                            this.Select(newObj, i > 0 ? true : multipleSelection);
                        }
                        else
                        {
                            if (multipleSelection != null && (i > 0 || multipleSelection.Value) && newObj is NodeGroup groupObj && string.IsNullOrEmpty(groupObj.ParentId))
                            {
                                for (int i1 = 0; i1 < this.diagram.SelectionSettings.Nodes.Count; i1++)
                                {
                                    if (!string.IsNullOrEmpty(this.diagram.SelectionSettings.Nodes[i1].ParentId))
                                    {
                                        if (this.diagram.NameTable[this.diagram.SelectionSettings.Nodes[i1].ParentId] is Node parentNode)
                                        {
                                            parentNode = this.FindParent(parentNode);
                                            if (parentNode != null)
                                            {
                                                if (groupObj.ID == parentNode.ID)
                                                {
                                                    this.SelectGroup(groupObj);
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            this.SelectProcesses(newObj as Node);
                            var select = this.SelectBpmnSubProcesses(newObj as NodeBase);
                            if (select)
                            {
                                this.Select(newObj, multipleSelection != null && (i > 0 || multipleSelection.Value));
                            }
                        }
                    }
                }
            }

            SelectionChangedEventArgs selectionChangedEventArgs = new SelectionChangedEventArgs()
            {
                OldValue = oldSelectedObjects,
                NewValue = GetSelectedObject(),
                ActionTrigger = diagram.DiagramAction,
                Type = CollectionChangedAction.Add
            };
            InvokeDiagramEvents(DiagramEvent.SelectionChanged, selectionChangedEventArgs);
        }
        private Node FindParent(Node node)
        {
            if (!string.IsNullOrEmpty(node.ParentId))
            {
                node = this.diagram.NameTable[node.ParentId] as Node;
                this.FindParent(node);
            }
            return node;
        }
        private void SelectProcesses(Node newObj)
        {
            if (newObj != null && HasProcesses(newObj))
            {
                DiagramObjectCollection<string> processes = (newObj.Shape as BpmnShape)?.Activity.SubProcess.Processes;
                if (processes != null)
                {
                    for (var i = 0; i < processes.Count; i++)
                    {
                        Node innerChild = this.diagram.NameTable[processes[i]] as Node;
                        if (HasProcesses(innerChild))
                        {
                            ObservableCollection<IDiagramObject> innerChilds =
                                new ObservableCollection<IDiagramObject>() { innerChild };
                            this.SelectObjects(innerChilds, true);
                            this.UnSelect(innerChild);
                        }
                    }
                }
            }
        }
        private bool SelectBpmnSubProcesses(NodeBase node)
        {
            bool select = true;
            string parent = string.Empty;
            if (node is Node nodeBase && !string.IsNullOrEmpty(nodeBase.ProcessId))
            {
                if (ActionsUtil.IsSelected(this.diagram, this.diagram.NameTable[nodeBase.ProcessId]))
                {
                    select = false;
                }
                else
                {
                    select = this.SelectBpmnSubProcesses(this.diagram.NameTable[nodeBase.ProcessId] as Node);
                }
            }
            else if (node is Connector connector)
            {
                if (!string.IsNullOrEmpty(connector.SourceID) && this.diagram.NameTable.ContainsKey(connector.SourceID) &&
                    !string.IsNullOrEmpty((this.diagram.NameTable[connector.SourceID] as Node)?.ProcessId))
                {
                    parent = (this.diagram.NameTable[connector.SourceID] as Node)?.ProcessId;
                }
                if (!string.IsNullOrEmpty(connector.TargetID) && this.diagram.NameTable.ContainsKey(connector.TargetID) &&
                    !string.IsNullOrEmpty((this.diagram.NameTable[connector.TargetID] as Node)?.ProcessId))
                {
                    parent = (this.diagram.NameTable[connector.TargetID] as Node)?.ProcessId;
                }
                if (!string.IsNullOrEmpty(parent) && this.diagram.NameTable.ContainsKey(parent))
                {
                    if (ActionsUtil.IsSelected(this.diagram, this.diagram.NameTable[parent]))
                    {
                        return false;
                    }
                    else
                    {
                        select = this.SelectBpmnSubProcesses(this.diagram.NameTable[parent] as Node);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(node.ParentId) && this.diagram.NameTable.ContainsKey(node.ParentId))
            {
                if (ActionsUtil.IsSelected(this.diagram, this.diagram.NameTable[node.ParentId]))
                {
                    select = false;
                }
            }
            return select;
        }
        private static bool HasProcesses(Node node)
        {
            if (node != null)
            {
                if ((node.Shape.Type == Shapes.Bpmn) && (node.Shape is BpmnShape bpmnShape) && bpmnShape.Activity != null
                   && bpmnShape.Activity.SubProcess.Processes != null
                    && bpmnShape.Activity.SubProcess.Processes.Any())
                {
                    return true;
                }
            }
            return false;
        }
        private void SelectGroup(NodeGroup newObj)
        {
            for (var j = 0; j < newObj.Children.Length; j++)
            {
                IDiagramObject innerChild = this.diagram.NameTable[newObj.Children[j]];
                if (innerChild != null && innerChild is NodeGroup child)
                {
                    this.SelectGroup(child);
                }
                this.UnSelect(innerChild);
            }
        }
        internal DiagramPoint SnapPoint(DiagramPoint startPoint, DiagramPoint endPoint, double tx, double ty)
        {
            bool towardsLeft = endPoint.X < startPoint.X;
            bool towardsTop = endPoint.Y < startPoint.Y;
            var point = new DiagramPoint() { X = tx, Y = ty };
            DiagramPoint snappedPoint = point;
            if (this.diagram.SnapSettings != null && this.diagram.Snapping != null)
            {
                snappedPoint = this.diagram.Snapping.SnapPoint(HelperObject, towardsLeft, towardsTop, point, startPoint, endPoint);
            }
            return snappedPoint;
        }
        internal DiagramPoint SnapConnectorEnd(DiagramPoint currentPosition)
        {
            if (this.diagram.SnapSettings != null && (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToHorizontalLines) || (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToVerticalLines))))
            {
                this.diagram.Snapping.SnapConnectorEnd(currentPosition);
            }
            return currentPosition;

        }
        internal DiagramSelectionSettings RenderHelper()
        {
            if (HelperObject == null)
            {
                bool oldIsProtectedOnChange = SfDiagramComponent.IsProtectedOnChange;
                DiagramSelectionSettings obj = diagram.SelectionSettings;
                SfDiagramComponent.IsProtectedOnChange = false;
                DiagramSelectionSettings selector = (DiagramSelectionSettings)obj.Clone();
                selector.Nodes = new ObservableCollection<Node>();
                selector.Connectors = new ObservableCollection<Connector>();
                diagram.TooltipTarget = "#helper";
                if (obj.Connectors.Count == 1 && obj.Nodes.Count == 0)
                {
                    Connector connector = (Connector)obj.Connectors[0].Clone();
                    connector.ID = "helper" + BaseUtil.RandomId();
                    if (this.diagram.EventHandler.Tool is DragController)
                        diagram.TooltipTarget = "#" + connector.ID + "_path";
                    connector.Parent = diagram;
                    connector.Style.StrokeDashArray = "2,2";
                    diagram.DiagramContent.InitObject(connector);
                    diagram.UpdateNameTable(new List<NodeBase>() { connector });
                    selector.Connectors = new ObservableCollection<Connector>() { connector };
                }
                selector.RotationAngle = obj.RotationAngle;
                selector.Init(diagram);
                HelperObject = selector;
                SfDiagramComponent.IsProtectedOnChange = oldIsProtectedOnChange;
            }
            return HelperObject;
        }

        internal async void UpdateConnectorProperties()
        {
            if (HelperObject != null)
            {
                Connector helperConnector = HelperObject.Connectors[0];
                Connector connector = diagram.SelectionSettings.Connectors[0];
                string tempSourceId = connector.SourceID;
                string tempTargetId = connector.TargetID;
                diagram.BeginUpdate();
                connector.SourcePoint.X = helperConnector.SourcePoint.X;
                connector.SourcePoint.Y = helperConnector.SourcePoint.Y;
                connector.TargetPoint.X = helperConnector.TargetPoint.X;
                connector.TargetPoint.Y = helperConnector.TargetPoint.Y;
                connector.SourceID = helperConnector.SourceID;
                connector.TargetID = helperConnector.TargetID;
                connector.SourcePortID = helperConnector.SourcePortID;
                if (string.IsNullOrEmpty(connector.SourcePortID))
                    connector.SourcePortWrapper = null;
                connector.TargetPortID = helperConnector.TargetPortID;
                if (string.IsNullOrEmpty(connector.TargetPortID))
                    connector.TargetPortWrapper = null;
                Node sourceNode = this.diagram.DiagramContent.GetNode(connector.SourceID);
                Node targetNode = this.diagram.DiagramContent.GetNode(connector.TargetID);
                connector.SourceWrapper = sourceNode != null ? (DiagramElement)DiagramLayerContent.GetEndNodeWrapper(sourceNode, connector, true) : null;
                connector.TargetWrapper = targetNode != null ? (DiagramElement)DiagramLayerContent.GetEndNodeWrapper(targetNode, connector, false) : null;
                if (diagram.GetObject(tempSourceId) is Node node)
                {
                    node.OutEdges.Remove(helperConnector.ID);
                }
                node = diagram.GetObject(tempTargetId) as Node;
                node?.InEdges.Remove(helperConnector.ID);
                this.UpdateConnectorSegments();
                UpdateConnectorBounds(connector);
                await diagram.EndUpdate();
            }
        }

        private static void UpdateConnectorBounds(Connector connector)
        {
            if (connector.Wrapper.Children[0] is DiagramElement segment)
            {
                if (segment.Width != null && segment.Height != null)
                {
                    DiagramRect bounds = new DiagramRect()
                    {
                        X = (double)(segment.OffsetX - segment.Width / 2),
                        Y = (double)(segment.OffsetY - segment.Height / 2),
                        Width = (double)(segment.Width),
                        Height = (double)(segment.Height)
                    };
                    connector.Bounds = bounds;
                }
            }
        }
        internal bool ScaleSelectedItems(double sx, double sy, DiagramPoint pivot)
        {
            diagram.BeginUpdate();
            DiagramSelectionSettings selectedItem = this.diagram.SelectionSettings;
            bool isScale = this.diagram.Scale(selectedItem, sx, sy, pivot);
            _ = diagram.EndUpdate();
            return isScale;
        }
        internal async void UpdateSelector()
        {
            if (HelperObject != null)
            {
                double diffX = HelperObject.OffsetX - diagram.SelectionSettings.OffsetX;
                double diffY = HelperObject.OffsetY - diagram.SelectionSettings.OffsetY;
                if (diffX != 0 || diffY != 0)
                {
                    if (HelperObject.Connectors.Count == 1 && HelperObject.Nodes.Count == 0)
                    {
                        UpdateConnectorProperties();
                    }
                    else
                    {
                        diagram.BeginUpdate();
                        DragSelectedObjects(diffX, diffY);
                        HelperObject = null;
                        await diagram.EndUpdate();
                        if (isGroupDragging)
                        {
                            this.diagram.RealAction &= ~RealAction.PreventDragSource;
                            this.isGroupDragging = false;
                        }
                    }
                }
                else
                {
                    HelperObject = null;
                }
            }
        }
        internal void UpdateThumbConstaraints()
        {
            diagram.SelectionSettings.ThumbsConstraints = ThumbsConstraints.Default;
            DiagramObjectCollection<IDiagramObject> objs = new DiagramObjectCollection<IDiagramObject>(diagram.SelectionSettings.Nodes);
            SfDiagramComponent.UpdateThumbConstraints(objs, diagram.SelectionSettings);
            objs = new DiagramObjectCollection<IDiagramObject>(diagram.SelectionSettings.Connectors);
            SfDiagramComponent.UpdateThumbConstraints(objs, diagram.SelectionSettings, true);
        }
        /**   @private  */
        internal Snapping SnappingInstance()
        {
            return this.diagram.Snapping;
        }

        internal void DragSelectedObjects(double tx, double ty)
        {
            diagram.Drag(diagram.SelectionSettings, tx, ty);
        }

        internal void Drag(NodeBase obj, double tx, double ty, bool isPreventUpdate = false)
        {
            List<NodeBase> elements = new List<NodeBase>();
            List<NodeBase> parentElements = new List<NodeBase>();
            if (ConstraintsUtil.CanMove(obj) && ConstraintsUtil.CanPageEditable(diagram) && this.CheckBoundaryConstraints(tx, ty, obj.Wrapper.Bounds))
            {
                if (obj is Node node)
                {
                    if (node is NodeGroup groupObj && groupObj.Children != null && groupObj.Children.Length > 0 && !this.diagram.DiagramAction.HasFlag(DiagramAction.IsGroupDragging))
                    {
                        // While dragging the group, the group have connector with multiple segments, we need to prevent the segment update.
                        if (diagram.DiagramAction.HasFlag(DiagramAction.Interactions))
                        {
                            this.isGroupDragging = true;
                            this.diagram.RealAction |= RealAction.PreventDragSource;
                        }
                        this.diagram.DiagramAction |= DiagramAction.IsGroupDragging;
                        List<NodeBase> nodes = this.GetAllDescendants(node, elements);
                        List<NodeBase> parentNodes = this.GetAllDescendants(node, parentElements, false, true);
                        nodes.AddRange(parentNodes);
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            var tempNode = (this.diagram.NameTable[nodes[i].ID]) as NodeBase;
                            this.Drag(tempNode, tx, ty);
                        }
                        this.UpdateInnerParentProperties(node);
                        this.diagram.DiagramAction &= ~DiagramAction.IsGroupDragging;
                    }
                    node.OffsetX += tx;
                    node.OffsetY += ty;
                }
                else if (obj is Connector connector)
                {
                    bool update = connector.Type == ConnectorSegmentType.Bezier;
                    if (connector.SourceWrapper == null)
                    {
                        DragSourceEnd(connector, tx, ty, isPreventUpdate, null, null, null, update);
                    }
                    if (connector.TargetWrapper == null)
                    {
                        DragTargetEnd(connector, tx, ty, isPreventUpdate, null, null, null, update);
                    }
                    if (connector.SourceWrapper == null && connector.TargetWrapper == null)
                    {
                        DragControlPoint(connector, tx, ty);
                    }
                    UpdateConnectorBounds(connector);
                }
            }
        }

        internal void DragSourceEnd(Connector connector, double tx, double ty, bool? isPreventUpdate = false, Actions? endPoint = null, BezierSegment segment = null, DiagramPoint point = null, bool? update = false, bool? isDragSource = false, IDiagramObject target = null, string targetPortID = "")
        {
            if (ConstraintsUtil.CanDragSourceEnd(connector) && ConstraintsUtil.CanPageEditable(diagram) && endPoint != Actions.BezierSourceThumb && this.CheckBoundaryConstraints(tx, ty, connector.Wrapper.Bounds))
            {
                connector.SourcePoint.X += tx;
                connector.SourcePoint.Y += ty;
                if (endPoint == Actions.ConnectorSourceEnd && connector.Type == ConnectorSegmentType.Orthogonal)
                {
                    this.ChangeSegmentLength(connector, target, targetPortID, isDragSource ?? false);
                }
            }
            if (connector.Type == ConnectorSegmentType.Bezier)
            {
                if (segment != null)
                {
                    TranslateBezierPoints(connector, endPoint ?? Actions.ConnectorSourceEnd, tx, ty, segment, point, !update.Value);
                }
                else
                {
                    for (int i = 0; i < connector.SegmentCollection.Count; i++)
                    {
                        TranslateBezierPoints(connector, endPoint ?? Actions.ConnectorSourceEnd, tx, ty, connector.SegmentCollection[i] as BezierSegment, point, !update.Value);
                    }
                }
                bool isMultiObjectSelect = (this.diagram.SelectionSettings.Nodes.Count + this.diagram.SelectionSettings.Connectors.Count) > 1;
                if (isPreventUpdate.HasValue && !isPreventUpdate.Value && !isMultiObjectSelect)
                {
                    List<DiagramPoint> points = connector.GetConnectorPoints();
                    DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                    connector.Wrapper.Measure(new DiagramSize());
                    connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                    RefreshDiagram();
                }
            }
        }

        internal void DragTargetEnd(Connector connector, double tx, double ty, bool? isPreventUpdate = false, Actions? endPoint = null, BezierSegment segment = null, DiagramPoint point = null, bool? update = false)
        {
            ConnectorSegment Prev = null;
            if (ConstraintsUtil.CanDragTargetEnd(connector) && ConstraintsUtil.CanPageEditable(diagram) && !(endPoint is Actions.BezierTargetThumb) && this.CheckBoundaryConstraints(tx, ty, connector.Wrapper.Bounds))
            {
                connector.TargetPoint.X += tx;
                connector.TargetPoint.Y += ty;
            }
            if (endPoint == Actions.ConnectorTargetEnd && connector.Type == ConnectorSegmentType.Orthogonal && connector.Segments != null && connector.Segments.Count > 1)
            {
                Prev = connector.Segments[connector.Segments.Count - 2] as ConnectorSegment;
                if (Prev != null && connector.Segments[connector.Segments.Count - 1].Points.Count == 2)
                {
                    if ((Prev as OrthogonalSegment).Direction == Direction.Left || (Prev as OrthogonalSegment).Direction == Direction.Right)
                    {
                        Prev.Points[Prev.Points.Count - 1].X = connector.TargetPoint.X;
                    }
                    else
                    {
                        Prev.Points[Prev.Points.Count - 1].Y = connector.TargetPoint.Y;
                    }
                    (Prev as OrthogonalSegment).Length = DiagramPoint.DistancePoints(Prev.Points[0], Prev.Points[Prev.Points.Count - 1]);
                    (Prev as OrthogonalSegment).Direction = DiagramPoint.Direction(Prev.Points[0], Prev.Points[Prev.Points.Count - 1]);
                }
            }
            if (connector.Type == ConnectorSegmentType.Bezier && !this.diagram.DiagramAction.HasFlag(DiagramAction.UndoRedo))
            {
                if (segment != null)
                {
                    TranslateBezierPoints(connector, endPoint ?? Actions.ConnectorTargetEnd, tx, ty, segment, point, update != null && !update.Value);
                }
                else
                {
                    for (int i = 0; i < connector.SegmentCollection.Count; i++)
                    {
                        TranslateBezierPoints(connector, endPoint ?? Actions.ConnectorTargetEnd, tx, ty, connector.SegmentCollection[i] as BezierSegment, point, update != null && !update.Value);
                    }
                }
                bool isMultiObjectSelect = (this.diagram.SelectionSettings.Nodes.Count + this.diagram.SelectionSettings.Connectors.Count) > 1;
                if (isPreventUpdate.HasValue && !isPreventUpdate.Value && !isMultiObjectSelect)
                {
                    List<DiagramPoint> points = connector.GetConnectorPoints();
                    DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
                    connector.Wrapper.Measure(new DiagramSize());
                    connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                    RefreshDiagram();
                }
            }
        }

        internal static void DragControlPoint(Connector connector, double tx, double ty, int? segmentNumber = null)
        {
            if ((connector.Type == ConnectorSegmentType.Straight || connector.Type == ConnectorSegmentType.Bezier) && connector.Segments.Count > 0)
            {
                if (segmentNumber != null && segmentNumber.Value < (connector.Segments.Count - 1))
                {
                    if (connector.Segments[segmentNumber.Value] is StraightSegment straightSegment)
                    {
                        straightSegment.Point.X += tx;
                        straightSegment.Point.Y += ty;
                    }
                }
                else
                {
                    for (int i = 0; i < connector.Segments.Count - 1; i++)
                    {
                        if (connector.Segments[i] is StraightSegment straightSegment)
                        {
                            straightSegment.Point.X += tx;
                            straightSegment.Point.Y += ty;
                        }
                    }
                }
            }
        }

        internal void InvokeDiagramEvents(DiagramEvent eventName, object args)
        {
            diagram.RealAction |= RealAction.PreventEventRefresh;
            switch (eventName)
            {
                case DiagramEvent.SelectionChanging:
                    diagram.SelectionChanging.InvokeAsync(args as SelectionChangingEventArgs);
                    break;
                case DiagramEvent.SelectionChanged:
                    diagram.SelectionChanged.InvokeAsync(args as SelectionChangedEventArgs);
                    break;
                case DiagramEvent.PositionChanging:
                    diagram.PositionChanging.InvokeAsync(args as PositionChangingEventArgs);
                    break;
                case DiagramEvent.PositionChanged:
                    diagram.PositionChanged.InvokeAsync(args as PositionChangedEventArgs);
                    break;
                case DiagramEvent.ConnectionChanging:
                    diagram.ConnectionChanging.InvokeAsync(args as ConnectionChangingEventArgs);
                    break;
                case DiagramEvent.ConnectionChanged:
                    diagram.ConnectionChanged.InvokeAsync(args as ConnectionChangedEventArgs);
                    break;
                case DiagramEvent.SourcePointChanging:
                    diagram.SourcePointChanging.InvokeAsync(args as EndPointChangingEventArgs);
                    break;
                case DiagramEvent.SourcePointChanged:
                    diagram.SourcePointChanged.InvokeAsync(args as EndPointChangedEventArgs);
                    break;
                case DiagramEvent.TargetPointChanging:
                    diagram.TargetPointChanging.InvokeAsync(args as EndPointChangingEventArgs);
                    break;
                case DiagramEvent.TargetPointChanged:
                    diagram.TargetPointChanged.InvokeAsync(args as EndPointChangedEventArgs);
                    break;
                case DiagramEvent.FixedUserHandleClick:
                    diagram.FixedUserHandleClick.InvokeAsync(args as FixedUserHandleClickEventArgs);
                    break;
                case DiagramEvent.RotationChanging:
                    diagram.RotationChanging.InvokeAsync(args as RotationChangingEventArgs);
                    break;
                case DiagramEvent.RotationChanged:
                    diagram.RotationChanged.InvokeAsync(args as RotationChangedEventArgs);
                    break;
                case DiagramEvent.SizeChanging:
                    diagram.SizeChanging.InvokeAsync(args as SizeChangingEventArgs);
                    break;
                case DiagramEvent.SizeChanged:
                    diagram.SizeChanged.InvokeAsync(args as SizeChangedEventArgs);
                    break;
                case DiagramEvent.HistoryChanged:
                    diagram.HistoryChanged.InvokeAsync(args as HistoryChangedEventArgs);
                    break;
                case DiagramEvent.DragStart:
                    diagram.DragStart.InvokeAsync(args as DragStartEventArgs);
                    break;
                case DiagramEvent.DragDrop:
                    diagram.DragDrop.InvokeAsync(args as DropEventArgs);
                    break;
                case DiagramEvent.Dragging:
                    diagram.Dragging.InvokeAsync(args as DraggingEventArgs);
                    break;
                case DiagramEvent.DragLeave:
                    diagram.DragLeave.InvokeAsync(args as DragLeaveEventArgs);
                    break;
                case DiagramEvent.HistoryAdding:
                    diagram.HistoryManager.HistoryAdding.InvokeAsync(args as HistoryAddingEventArgs);
                    break;
                case DiagramEvent.Undo:
                    diagram.HistoryManager.Undo.InvokeAsync(args as HistoryEntryBase);
                    break;
                case DiagramEvent.Redo:
                    diagram.HistoryManager.Redo.InvokeAsync(args as HistoryEntryBase);
                    break;
                case DiagramEvent.KeyDown:
                    diagram.KeyDown.InvokeAsync(args as KeyEventArgs);
                    break;
                case DiagramEvent.KeyUp:
                    diagram.KeyUp.InvokeAsync(args as KeyEventArgs);
                    break;
                case DiagramEvent.CollectionChanging:
                    diagram.CollectionChanging.InvokeAsync(args as CollectionChangingEventArgs);
                    break;
                case DiagramEvent.CollectionChanged:
                    diagram.CollectionChanged.InvokeAsync(args as CollectionChangedEventArgs);
                    break;
                case DiagramEvent.SegmentCollectionChange:
                    diagram.SegmentCollectionChange.InvokeAsync(args as SegmentCollectionChangeEventArgs);
                    break;
                case DiagramEvent.Click:
                    diagram.Click.InvokeAsync(args as ClickEventArgs);
                    break;
                case DiagramEvent.MouseEnter:
                    diagram.MouseEnter.InvokeAsync(args as DiagramElementMouseEventArgs);
                    break;
                case DiagramEvent.MouseHover:
                    diagram.MouseHover.InvokeAsync(args as DiagramElementMouseEventArgs);
                    break;
                case DiagramEvent.MouseLeave:
                    diagram.MouseLeave.InvokeAsync(args as DiagramElementMouseEventArgs);
                    break;
                case DiagramEvent.ScrollChanged:
                    diagram.ScrollChanged.InvokeAsync(args as ScrollChangedEventArgs);
                    break;
                case DiagramEvent.PropertyChanged:
                    diagram.PropertyChanged.InvokeAsync(args as PropertyChangedEventArgs);
                    break;
                case DiagramEvent.TextChanged:
                    diagram.TextChanged.InvokeAsync(args as TextChangeEventArgs);
                    break;
            }
            diagram.RealAction &= ~RealAction.PreventEventRefresh;
        }

        internal void DisConnect(IDiagramObject obj, Actions? endPoint = null)
        {
            ConnectionObject oldChanges = new ConnectionObject(); ConnectionObject newChanges = new ConnectionObject();
            Connector connector = null;
            if (obj is DiagramSelectionSettings selector && selector.Connectors.Count > 0)
            {
                connector = selector.Connectors[0];
            }
            if (obj != null && connector != null && (ConstraintsUtil.HasSingleConnection(this.diagram)))
            {
                if (endPoint != null && (endPoint == Actions.ConnectorSourceEnd || endPoint == Actions.ConnectorTargetEnd))
                {
                    if (endPoint == Actions.ConnectorSourceEnd)
                    {
                        if (!string.IsNullOrEmpty(connector.SourceID))
                        {
                            oldChanges.SourceID = connector.SourceID;
                            newChanges.SourceID = string.Empty;
                            if (!string.IsNullOrEmpty(connector.SourcePortID))
                            {
                                oldChanges.SourcePortID = connector.SourcePortID;
                                newChanges.SourcePortID = string.Empty;
                            }
                            ConnectionChangingEventArgs connectionChangingEventArgs = new ConnectionChangingEventArgs()
                            {
                                Cancel = false,
                                Connector = connector,
                                ConnectorAction = endPoint.Value,
                                NewValue = newChanges,
                                OldValue = oldChanges
                            };
                            InvokeDiagramEvents(DiagramEvent.ConnectionChanging, connectionChangingEventArgs);
                            if (!connectionChangingEventArgs.Cancel)
                            {
                                connector.SourceID = newChanges.SourceID;
                                connector.SourceWrapper = null;
                                if (!string.IsNullOrEmpty(connector.SourcePortID))
                                {
                                    connector.SourcePortID = string.Empty;
                                    connector.SourcePortWrapper = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(connector.TargetID))
                        {
                            oldChanges.TargetID = connector.TargetID;
                            newChanges.TargetID = string.Empty;

                            if (!string.IsNullOrEmpty(connector.TargetPortID))
                            {
                                oldChanges.TargetPortID = connector.TargetPortID;
                                newChanges.TargetPortID = string.Empty;
                            }
                            ConnectionChangingEventArgs connectionChangingEventArgs = new ConnectionChangingEventArgs()
                            {
                                Cancel = false,
                                Connector = connector,
                                ConnectorAction = endPoint.Value,
                                NewValue = newChanges,
                                OldValue = oldChanges
                            };
                            InvokeDiagramEvents(DiagramEvent.ConnectionChanging, connectionChangingEventArgs);
                            if (!connectionChangingEventArgs.Cancel)
                            {
                                connector.TargetID = newChanges.TargetID;
                                connector.TargetWrapper = null;
                                if (!string.IsNullOrEmpty(connector.TargetPortID))
                                {
                                    connector.TargetPortID = string.Empty;
                                    connector.TargetPortWrapper = null;
                                }
                            }
                        }
                    }
                }
                else if (endPoint != Actions.OrthogonalThumb && endPoint != Actions.SegmentEnd && (!string.IsNullOrEmpty(connector.SourceID) || !string.IsNullOrEmpty(connector.TargetID)))
                {
                    oldChanges.SourceID = connector.SourceID;
                    oldChanges.TargetID = connector.TargetID;
                    oldChanges.SourcePortID = connector.SourcePortID;
                    oldChanges.TargetPortID = connector.TargetPortID;

                    newChanges.SourceID = string.Empty;
                    newChanges.TargetID = string.Empty;
                    newChanges.SourcePortID = string.Empty;
                    newChanges.TargetPortID = string.Empty;

                    ConnectionChangingEventArgs arg = new ConnectionChangingEventArgs()
                    {
                        Connector = connector,
                        OldValue = oldChanges,
                        NewValue = newChanges,
                        Cancel = false,
                        ConnectorAction = endPoint ?? Actions.ConnectorSourceEnd | Actions.ConnectorTargetEnd
                    };
                    this.InvokeDiagramEvents(DiagramEvent.ConnectionChanging, arg);
                    if (!arg.Cancel)
                    {
                        connector.SourceID = string.Empty;
                        connector.TargetID = string.Empty;
                        connector.SourcePortID = string.Empty;
                        connector.TargetPortID = string.Empty;

                        connector.SourceWrapper = null;
                        connector.SourcePortWrapper = null;
                        connector.TargetWrapper = null;
                        connector.TargetPortWrapper = null;

                        this.HighlighterElement = null;
                        ConnectionChangedEventArgs connectionChangedEventArgs = new ConnectionChangedEventArgs()
                        {
                            Connector = connector,
                            OldValue = oldChanges,
                            NewValue = newChanges,
                            ConnectorAction = endPoint ?? Actions.ConnectorSourceEnd | Actions.ConnectorTargetEnd
                        };
                        this.InvokeDiagramEvents(DiagramEvent.ConnectionChanged, connectionChangedEventArgs);
                    }
                }
            }
        }

        internal void Connect(Actions endPoint, DiagramMouseEventArgs args)
        {
            ConnectionObject oldChanges = new ConnectionObject();
            ConnectionObject newChanges = new ConnectionObject();
            IDiagramObject drawObject = this.CurrentDrawingObject;
            Connector connector = HelperObject != null ? HelperObject.Connectors[0] : drawObject as Connector;
            IDiagramObject target = FindTarget(args.TargetWrapper ?? args.SourceWrapper,
                    args.Target ?? args.ActualObject);

            if (target is Node node)
            {
                if (endPoint == Actions.ConnectorSourceEnd)
                {
                    oldChanges.SourceID = connector.SourceID;
                    newChanges.SourceID = node.ID;
                    oldChanges.SourcePortID = connector.SourcePortID;
                    ConnectionChangingEventArgs connectionChangingEventArgs = new ConnectionChangingEventArgs()
                    {
                        Cancel = false,
                        Connector = connector,
                        ConnectorAction = endPoint,
                        OldValue = oldChanges,
                        NewValue = newChanges
                    };
                    InvokeDiagramEvents(DiagramEvent.ConnectionChanging, connectionChangingEventArgs);
                    if (!connectionChangingEventArgs.Cancel)
                    {
                        connector.SourceID = newChanges.SourceID;
                    }
                }
                else if (endPoint == Actions.ConnectorTargetEnd)
                {
                    oldChanges.TargetID = connector.TargetID;
                    newChanges.TargetID = node.ID;
                    oldChanges.TargetPortID = connector.TargetPortID;
                    ConnectionChangingEventArgs connectionChangingEventArgs = new ConnectionChangingEventArgs()
                    {
                        Cancel = false,
                        Connector = connector,
                        ConnectorAction = endPoint,
                        OldValue = oldChanges,
                        NewValue = newChanges
                    };
                    InvokeDiagramEvents(DiagramEvent.ConnectionChanging, connectionChangingEventArgs);
                    if (!connectionChangingEventArgs.Cancel)
                    {
                        connector.TargetID = newChanges.TargetID;
                    }
                }
            }
            else
            {
                if (endPoint == Actions.ConnectorSourceEnd)
                {
                    oldChanges.SourceID = connector.SourceID;
                    oldChanges.SourcePortID = connector.SourcePortID;
                    newChanges.SourceID = args.Target != null ? (args.Target as Node)?.ID : (args.ActualObject as Node)?.ID;
                    newChanges.SourcePortID = (target as PointPort)?.ID;
                    ConnectionChangingEventArgs connectionChangingEventArgs = new ConnectionChangingEventArgs()
                    {
                        Cancel = false,
                        Connector = connector,
                        ConnectorAction = endPoint,
                        OldValue = oldChanges,
                        NewValue = newChanges
                    };
                    InvokeDiagramEvents(DiagramEvent.ConnectionChanging, connectionChangingEventArgs);
                    if (!connectionChangingEventArgs.Cancel)
                    {
                        connector.SourceID = newChanges.SourceID;
                        connector.SourcePortID = newChanges.SourcePortID;
                    }
                }
                else if (endPoint == Actions.ConnectorTargetEnd)
                {
                    oldChanges.TargetID = connector.TargetID;
                    oldChanges.TargetPortID = connector.TargetPortID;
                    newChanges.TargetID = args.Target != null ? (args.Target as Node)?.ID : (args.ActualObject as Node)?.ID;
                    newChanges.TargetPortID = (target as PointPort)?.ID;
                    ConnectionChangingEventArgs connectionChangingEventArgs = new ConnectionChangingEventArgs()
                    {
                        Cancel = false,
                        Connector = connector,
                        ConnectorAction = endPoint,
                        OldValue = oldChanges,
                        NewValue = newChanges
                    };
                    InvokeDiagramEvents(DiagramEvent.ConnectionChanging, connectionChangingEventArgs);
                    if (!connectionChangingEventArgs.Cancel)
                    {
                        connector.TargetID = newChanges.TargetID;
                        connector.TargetPortID = newChanges.TargetPortID;
                    }

                }
            }
            this.RenderHighlighter(args, null);
        }

        internal void RenderHighlighter(DiagramMouseEventArgs args, bool? connectHighlighter = false)
        {
            if (args.Target is Node || (connectHighlighter.HasValue && connectHighlighter.Value && args.Element is Node))
            {
                IDiagramObject tgt = (connectHighlighter.HasValue && connectHighlighter.Value) ? args.Element : args.Target;
                ICommonElement tgtWrap = (connectHighlighter.HasValue && connectHighlighter.Value) ? args.SourceWrapper : args.TargetWrapper;
                IDiagramObject target = FindTarget(tgtWrap, tgt);
                ICommonElement element = null;
                if (target is BpmnSubEvent)
                {
                    string portId = string.Empty;
                    if (args.Target is NodeBase node && node.Wrapper.Children[0] is Canvas canvas)
                    {
                        if ((canvas.Children[0] as Canvas)?.Children[2] is Canvas parent)
                        {
                            for (int i = 0; i < parent.Children.Count; i++)
                            {
                                ICommonElement child = parent.Children[i];
                                if (child.ID == node.ID + '_' + portId)
                                {
                                    element = (child as Canvas)?.Children[0];
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    element = target is Node ?
                        (target as Node).Wrapper : connectHighlighter.HasValue && connectHighlighter.Value ? args.SourceWrapper : args.TargetWrapper;
                }
                DrawHighlighter(element);
            }
        }

        internal void DrawHighlighter(ICommonElement element)
        {
            TransformFactor transform = diagram.Scroller.Transform;
            double width = element.ActualSize.Width ?? 2;
            double height = element.ActualSize.Height ?? 2;
            double x = ((element.OffsetX - width * element.Pivot.X) + transform.TX) * transform.Scale;
            double y = ((element.OffsetY - height * element.Pivot.Y) + transform.TY) * transform.Scale;
            RectAttributes attributes = new RectAttributes()
            {
                Width = width * transform.Scale,
                Height = height * transform.Scale,
                X = x,
                Y = y,
                Fill = "transparent",
                Stroke = "#8CC63F",
                Angle = element.RotationAngle,
                PivotX = element.Pivot.X,
                PivotY = element.Pivot.Y,
                StrokeWidth = 4,
                DashArray = string.Empty,
                Opacity = 1,
                CornerRadius = 0,
                Visible = true,
                ID = element.ID + "_highlighter",
                ClassValues = "e-diagram-highlighter"
            };
            this.HighlighterElement = attributes;
        }
        ///<summary>
        /// Translate the bezier points during the interaction
        ///</summary>
        private static void TranslateBezierPoints(Connector connector, Actions value, double tx, double ty, BezierSegment seg, DiagramPoint point = null, bool? update = null)
        {
            int index = connector.SegmentCollection.IndexOf(seg);
            if (connector.SegmentCollection[index] is BezierSegment segment)
            {
                if (value == Actions.BezierSourceThumb && (segment.Vector1.Angle != 0 || segment.Vector1.Distance != 0))
                {
                    segment.Vector1 = new Vector()
                    {
                        Distance = Connector.Distance(connector.SourcePoint, point),
                        Angle = DiagramPoint.FindAngle(connector.SourcePoint, point)
                    };
                }
                else if (value == Actions.BezierTargetThumb && (segment.Vector2.Angle != 0 || segment.Vector2.Distance != 0))
                {
                    segment.Vector2 = new Vector() { Distance = Connector.Distance(connector.TargetPoint, point), Angle = DiagramPoint.FindAngle(connector.TargetPoint, point) };
                }
                else if ((value == Actions.ConnectorSourceEnd && string.IsNullOrEmpty(connector.SourceID) || value == Actions.ConnectorTargetEnd && string.IsNullOrEmpty(connector.TargetID))
                  && update.HasValue && update.Value && Connector.IsEmptyVector(segment.Vector1) && Connector.IsEmptyVector(segment.Vector2))
                {
                    if (DiagramPoint.IsEmptyPoint(segment.Point1))
                    {
                        segment.BezierPoint1 = Connector.GetBezierPoints(connector.SourcePoint, connector.TargetPoint);
                    }
                    if (DiagramPoint.IsEmptyPoint(segment.Point2))
                    {
                        segment.BezierPoint2 = Connector.GetBezierPoints(connector.TargetPoint, connector.SourcePoint);
                    }
                }
                else if (value == Actions.BezierSourceThumb || (value == Actions.ConnectorSourceEnd && update.HasValue && !update.Value && Connector.IsEmptyVector(segment.Vector1)))
                {
                    segment.BezierPoint1.X += tx;
                    segment.BezierPoint1.Y += ty;
                    if ((!DiagramPoint.IsEmptyPoint(segment.Point1)) || (update.HasValue && update.Value))
                    {
                        segment.Point1 = new DiagramPoint() { X = segment.BezierPoint1.X, Y = segment.BezierPoint1.Y };
                    }
                }
                else if (value == Actions.BezierTargetThumb || (value == Actions.ConnectorTargetEnd && update.HasValue && !update.Value && Connector.IsEmptyVector(segment.Vector2)))
                {
                    segment.BezierPoint2.X += tx;
                    segment.BezierPoint2.Y += ty;
                    if ((!DiagramPoint.IsEmptyPoint(segment.Point2)) || (update.HasValue && update.Value))
                    {
                        segment.Point2 = new DiagramPoint() { X = segment.BezierPoint2.X, Y = segment.BezierPoint2.Y };
                    }
                }
            }
        }

        internal void DragConnectorEnds(Actions endPoint, IDiagramObject obj, DiagramPoint point, int bezierSegmentIndex, IDiagramObject target, string targetPortID = "")
        {
            Connector connector = null;
            BezierSegment segment = null;
            double tx; double ty;
            if (obj is DiagramSelectionSettings selector)
            {
                connector = selector.Connectors[0];
            }
            bool checkBezierThumb = endPoint == Actions.BezierSourceThumb || endPoint == Actions.BezierTargetThumb;

            if (connector != null && connector.Type == ConnectorSegmentType.Bezier && connector.Segments.Count > 0)
            {
                segment = connector.Segments[bezierSegmentIndex] as BezierSegment;
            }

            if (endPoint == Actions.ConnectorSourceEnd || endPoint == Actions.BezierSourceThumb)
            {
                tx = point.X - (checkBezierThumb ? segment.BezierPoint1.X : connector.SourcePoint.X);
                ty = point.Y - (checkBezierThumb ? segment.BezierPoint1.Y : connector.SourcePoint.Y);
                this.DragSourceEnd(connector, tx, ty, null, endPoint, segment, point, false, false, target, targetPortID);
            }
            else
            {
                tx = point.X - (checkBezierThumb ? segment.BezierPoint2.X : connector.TargetPoint.X);
                ty = point.Y - (checkBezierThumb ? segment.BezierPoint2.Y : connector.TargetPoint.Y);
                this.DragTargetEnd(connector, tx, ty, null, endPoint, segment, point, false);
            }
        }

        internal void UpdateConnectorSegments()
        {
            SfDiagramComponent.IsProtectedOnChange = false;
            Connector helperConnector = HelperObject.Connectors[0];
            Connector connector = diagram.SelectionSettings.Connectors[0];
            if (helperConnector.SegmentCollection.Count < connector.SegmentCollection.Count)
            {
                connector.SegmentCollection = new DiagramObjectCollection<ConnectorSegment>();
            }
            for (int i = 0; i < helperConnector.SegmentCollection.Count; i++)
            {
                var segment = helperConnector.SegmentCollection[i];
                if (segment is OrthogonalSegment helperOrthogonalSegment)
                {
                    if (connector.SegmentCollection.Count > i)
                    {
                        if (connector.SegmentCollection[i] is OrthogonalSegment orthogonalSegment)
                        {
                            orthogonalSegment.Direction = helperOrthogonalSegment.Direction;
                            orthogonalSegment.Length = helperOrthogonalSegment.Length;
                        }
                    }
                    else
                    {
                        connector.SegmentCollection.Add(new OrthogonalSegment() { Direction = helperOrthogonalSegment.Direction, Length = helperOrthogonalSegment.Length });
                    }
                }
                else if (segment is BezierSegment helperBezierSegment)
                {
                    BezierSegment bezierSegment = connector.SegmentCollection[i] as BezierSegment;
                    bezierSegment.BezierPoint1.X = helperBezierSegment.BezierPoint1.X;
                    bezierSegment.BezierPoint1.Y = helperBezierSegment.BezierPoint1.Y;

                    bezierSegment.BezierPoint2.X = helperBezierSegment.BezierPoint2.X;
                    bezierSegment.BezierPoint2.Y = helperBezierSegment.BezierPoint2.Y;

                    bezierSegment.Point.X = helperBezierSegment.Point.X;
                    bezierSegment.Point.Y = helperBezierSegment.Point.Y;

                    bezierSegment.Point1.X = helperBezierSegment.Point1.X;
                    bezierSegment.Point1.Y = helperBezierSegment.Point1.Y;

                    bezierSegment.Point2.X = helperBezierSegment.Point2.X;
                    bezierSegment.Point2.Y = helperBezierSegment.Point2.Y;

                    bezierSegment.Vector1.Angle = helperBezierSegment.Vector1.Angle;
                    bezierSegment.Vector1.Distance = helperBezierSegment.Vector1.Distance;

                    bezierSegment.Vector2.Angle = helperBezierSegment.Vector2.Angle;
                    bezierSegment.Vector2.Distance = helperBezierSegment.Vector2.Distance;
                }
                else
                {
                    StraightSegment helperStraightSegment = segment as StraightSegment;
                    if (connector.SegmentCollection.Count > i)
                    {
                        if (connector.SegmentCollection[i] is StraightSegment straightSegment)
                        {
                            straightSegment.Point.X = helperStraightSegment.Point.X;
                            straightSegment.Point.Y = helperStraightSegment.Point.Y;
                        }
                    }
                    else
                    {
                        if (helperStraightSegment != null)
                            connector.SegmentCollection.Add(new StraightSegment()
                            { Point = new DiagramPoint(helperStraightSegment.Point.X, helperStraightSegment.Point.Y) });
                    }
                }
            }
            List<DiagramPoint> points = connector.GetConnectorPoints();
            if (diagram.NameTable.ContainsKey(connector.ID))
            {
                DiagramUtil.UpdateConnector(connector, points.Count > 0 ? points : connector.IntermediatePoints);
            }
            connector.Wrapper.Measure(new DiagramSize());
            connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
            HelperObject = null;
            SfDiagramComponent.IsProtectedOnChange = true;
        }



        internal static bool CanDisconnect(Actions endPoint, DiagramMouseEventArgs args, string targetPortId, string targetNodeId)
        {
            Connector connect = null;
            if (args.Element is DiagramSelectionSettings selector)
            {
                connect = selector.Connectors[0];
            }
            if (endPoint == Actions.ConnectorSourceEnd)
            {
                return connect != null && (connect.SourceID != targetNodeId || connect.SourcePortID != targetPortId);
            }
            else if (endPoint == Actions.ConnectorTargetEnd)
            {
                return connect != null && (connect.TargetID != targetNodeId || connect.TargetPortID != targetPortId);
            }
            return false;
        }



        private void ChangeSegmentLength(Connector connector, IDiagramObject target, string targetPortId, bool isDragSource)
        {
            if (connector.SegmentCollection.Any() && (connector.SegmentCollection[0] as OrthogonalSegment)?.Direction != null
                && ((target == null && string.IsNullOrEmpty(connector.SourceID)) || isDragSource))
            {
                OrthogonalSegment first = connector.SegmentCollection[0] as OrthogonalSegment;
                OrthogonalSegment second = connector.SegmentCollection[1] as OrthogonalSegment;
                Node node = (!string.IsNullOrEmpty(connector.SourceID)) ? diagram.NameTable[connector.SourceID] as Node : null;
                first.Points[0] = connector.SourcePoint;
                if (first.Direction == Direction.Top || first.Direction == Direction.Bottom)
                {
                    first.Points[^1].X = connector.SourcePoint.X;
                    second.Points[0].Y = first.Points[^1].Y;
                }
                else
                {
                    first.Points[^1].Y = connector.SourcePoint.Y;
                    second.Points[0].X = first.Points[^1].X;
                }
                if (first.Direction != null && (first.Length != null || first.Length == 0))
                {
                    first.Length = DiagramPoint.DistancePoints(first.Points[0], first.Points[^1]);
                }
                if (second.Direction != null && (second.Length != null || second.Length == 0))
                {
                    second.Length = DiagramPoint.DistancePoints(first.Points[^1], second.Points[^1]);
                    second.Direction = DiagramPoint.Direction(
                        first.Points[^1], second.Points[^1]);
                }
                if (!string.IsNullOrEmpty(connector.SourcePortID) && first.Length < 10)
                {
                    if (connector.SegmentCollection.Count > 2)
                    {
                        if (connector.SegmentCollection[2] is OrthogonalSegment next)
                        {
                            Direction nextDirection = DiagramPoint.Direction(next.Points[0], next.Points[1]);
                            if (first.Direction == ConnectorUtil.GetOppositeDirection(nextDirection))
                            {
                                if (node != null)
                                {
                                    switch (first.Direction)
                                    {
                                        case Direction.Right:
                                            next.Points[0].X = first.Points[^1].X = node.Wrapper.Corners.MiddleRight.X + 20;
                                            break;
                                        case Direction.Left:
                                            next.Points[0].X = first.Points[^1].X = node.Wrapper.Corners.MiddleLeft.X - 20;
                                            break;
                                        case Direction.Top:
                                            next.Points[0].Y = first.Points[^1].Y = node.Wrapper.Corners.TopCenter.Y - 20;
                                            break;
                                        case Direction.Bottom:
                                            next.Points[0].Y = first.Points[^1].Y = node.Wrapper.Corners.BottomCenter.Y + 20;
                                            break;
                                    }
                                }

                                if (next.Direction != null && next.Length != null)
                                {
                                    next.Length = DiagramPoint.DistancePoints(next.Points[0], next.Points[^1]);
                                }
                                first.Length = DiagramPoint.DistancePoints(first.Points[0], first.Points[^1]);
                            }
                            else if (first.Direction == nextDirection && next.Direction != null && next.Length != null)
                            {
                                if (first.Direction == Direction.Top || first.Direction == Direction.Bottom)
                                {
                                    next.Points[0] = first.Points[0];
                                    next.Points[^1].X = next.Points[0].X;
                                }
                                else
                                {
                                    next.Points[0] = first.Points[0];
                                    next.Points[^1].Y = next.Points[0].Y;
                                }
                                next.Length = DiagramPoint.DistancePoints(next.Points[0], next.Points[^1]);
                                connector.SegmentCollection.RemoveAt(1);
                                connector.SegmentCollection.RemoveAt(0);
                            }
                            else
                            {
                                first.Length = 20;
                            }
                        }
                    }
                    else
                    {
                        first.Length = 20;
                    }
                }
                else if (first.Length < 1)
                {
                    if (!string.IsNullOrEmpty(connector.SourceID))
                    {
                        DiagramPoint secPoint = new DiagramPoint();
                        if (node != null)
                        {
                            switch (second.Direction)
                            {
                                case Direction.Right:
                                    secPoint = node.Wrapper.Corners.MiddleRight;
                                    second.Points[^1].Y = secPoint.Y;
                                    break;
                                case Direction.Left:
                                    secPoint = node.Wrapper.Corners.MiddleLeft;
                                    second.Points[^1].Y = secPoint.Y;
                                    break;
                                case Direction.Top:
                                    secPoint = node.Wrapper.Corners.TopCenter;
                                    second.Points[^1].X = secPoint.X;
                                    break;
                                case Direction.Bottom:
                                    secPoint = node.Wrapper.Corners.BottomCenter;
                                    second.Points[^1].X = secPoint.X;
                                    break;
                            }
                        }
                        second.Length = DiagramPoint.DistancePoints(secPoint, second.Points[^1]);
                        if (connector.SegmentCollection.Count > 2)
                        {
                            if (connector.SegmentCollection[2] is OrthogonalSegment next && next.Direction != null && next.Length != null)
                            {
                                next.Length = DiagramPoint.DistancePoints(
                                    second.Points[^1], next.Points[^1]);
                            }
                        }
                        connector.SegmentCollection.RemoveAt(0);
                    }
                    else
                    {
                        connector.SegmentCollection.RemoveAt(0);
                    }
                }
            }
            else
            {
                if ((target as Node) != null && string.IsNullOrEmpty(targetPortId) && connector.SourceID != (target as Node).ID &&
                    connector.SegmentCollection.Count > 0 && (connector.SegmentCollection[0] as OrthogonalSegment)?.Direction != null)
                {
                    ChangeSourceEndToNode(connector, (target as Node));
                }
                if (target != null && !string.IsNullOrEmpty(targetPortId) && connector.SourcePortID != targetPortId &&
                    connector.SegmentCollection.Count > 0 && (connector.SegmentCollection[0] as OrthogonalSegment)?.Direction != null)
                {
                    ChangeSourceEndToPort(connector, (target as Node), targetPortId);
                }
            }
        }

        private static void ChangeSourceEndToNode(Connector connector, Node target)
        {
            Corners sourceWrapper = target.Wrapper.Children[0].Corners; DiagramPoint sourcePoint; DiagramPoint sourcePoint2;
            OrthogonalSegment firstSegment = connector.SegmentCollection[0] as OrthogonalSegment;
            OrthogonalSegment nextSegment = connector.SegmentCollection[1] as OrthogonalSegment;
            DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>();
            if (firstSegment != null && (firstSegment.Direction == Direction.Right || firstSegment.Direction == Direction.Left))
            {
                sourcePoint = (firstSegment.Direction == Direction.Left) ? sourceWrapper.MiddleLeft : sourceWrapper.MiddleRight;
                if (firstSegment.Length > sourceWrapper.Width || ((firstSegment.Direction == Direction.Left &&
                    sourcePoint.X >= firstSegment.Points[0].X) || (firstSegment.Direction == Direction.Right &&
                        sourcePoint.X <= firstSegment.Points[0].X)))
                {

                    firstSegment.Points[0].Y = firstSegment.Points[^1].Y = sourcePoint.Y;
                    firstSegment.Points[0].X = sourcePoint.X;
                    firstSegment.Length = DiagramPoint.DistancePoints(
                        firstSegment.Points[0], firstSegment.Points[^1]);
                    nextSegment.Length = DiagramPoint.DistancePoints(
                        firstSegment.Points[^1], nextSegment.Points[^1]);
                }
                else
                {
                    Direction direction;
                    if (nextSegment != null && nextSegment.Direction != null)
                    {
                        direction = nextSegment.Direction.Value;
                    }
                    else
                    {
                        direction = DiagramPoint.Direction(
                            nextSegment.Points[0], nextSegment.Points[^1]);
                    }
                    sourcePoint2 = (direction == Direction.Bottom) ? sourceWrapper.BottomCenter : sourceWrapper.TopCenter;
                    if (nextSegment.Length.HasValue && nextSegment.Direction.HasValue)
                    {
                        nextSegment.Length =
                            (direction == Direction.Top) ? firstSegment.Points[^1].Y - (sourcePoint2.Y + 20) :
                                (sourcePoint2.Y + 20) - firstSegment.Points[^1].Y;
                    }
                    firstSegment.Length = firstSegment.Points[^1].X - sourcePoint2.X;
                    firstSegment.Direction = (firstSegment.Length > 0) ? Direction.Right : Direction.Left;
                    segments.Add(new OrthogonalSegment() { Direction = direction, Length = 20 });
                    for (int i = 0; i < connector.SegmentCollection.Count; i++)
                    {
                        segments.Add(connector.SegmentCollection[i] as OrthogonalSegment);
                    }
                    connector.SegmentCollection = segments;
                }
            }
            else
            {
                sourcePoint = firstSegment != null && (firstSegment.Direction == Direction.Bottom) ? sourceWrapper.BottomCenter : sourceWrapper.TopCenter;

                if (firstSegment != null && (firstSegment.Length > sourceWrapper.Height || ((firstSegment.Direction == Direction.Top &&
                    sourcePoint.Y >= firstSegment.Points[0].Y) ||
                    (firstSegment.Direction == Direction.Bottom && sourcePoint.Y <= firstSegment.Points[0].Y))))
                {
                    firstSegment.Points[0].X = firstSegment.Points[^1].X = sourcePoint.X;
                    firstSegment.Points[0].Y = sourcePoint.Y;
                    firstSegment.Length = DiagramPoint.DistancePoints(
                        firstSegment.Points[0], firstSegment.Points[^1]);
                    if (nextSegment != null)
                        nextSegment.Length = DiagramPoint.DistancePoints(
                                firstSegment.Points[^1], nextSegment.Points[^1]);
                }
                else
                {
                    sourcePoint2 = nextSegment != null && (nextSegment.Direction == Direction.Left) ? sourceWrapper.MiddleLeft : sourceWrapper.MiddleRight;

                    if (nextSegment != null && firstSegment != null)
                    {
                        Direction direction;
                        if (nextSegment.Direction.HasValue)
                        {
                            direction = nextSegment.Direction.Value;
                        }
                        else
                        {
                            direction = DiagramPoint.Direction(
                                    nextSegment.Points[0], nextSegment.Points[^1]);
                        }
                        if (nextSegment.Length.HasValue && nextSegment.Direction.HasValue)
                        {
                            nextSegment.Length = (direction == Direction.Left) ? firstSegment.Points[^1].X - (sourcePoint2.X + 20) : (sourcePoint2.X + 20) - firstSegment.Points[^1].X;
                        }
                        firstSegment.Length = firstSegment.Points[^1].Y - sourcePoint2.Y;
                        firstSegment.Direction = (firstSegment.Length > 0) ? Direction.Bottom : Direction.Top;
                        segments.Add(new OrthogonalSegment() { Direction = direction, Length = 20 });
                    }

                    for (int i = 0; i < connector.SegmentCollection.Count; i++)
                    {
                        segments.Add(connector.SegmentCollection[i] as OrthogonalSegment);
                    }
                    connector.SegmentCollection = segments;
                }
            }
        }

        private static void ChangeSourceEndToPort(Connector connector, Node target, string targetPortId)
        {
            ICommonElement port = DiagramUtil.GetWrapper(target, target.Wrapper, targetPortId);
            DiagramPoint point = new DiagramPoint(port.OffsetX, port.OffsetY);
            Direction direction = ConnectorUtil.GetPortDirection(point, BaseUtil.CornersPointsBeforeRotation(target.Wrapper), target.Wrapper.Bounds);
            OrthogonalSegment firstSegment = connector.SegmentCollection[0] as OrthogonalSegment;
            OrthogonalSegment secondSegment = connector.SegmentCollection[1] as OrthogonalSegment;
            if (firstSegment != null && firstSegment.Direction != null && firstSegment.Direction.Value != direction)
            {
                DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>();
                OrthogonalSegment segValues;
                if (firstSegment.Direction == ConnectorUtil.GetOppositeDirection(direction))
                {
                    segValues = new OrthogonalSegment(); OrthogonalSegment segValues1 = new OrthogonalSegment();
                    if (direction == Direction.Top || direction == Direction.Bottom)
                    {
                        segValues1 = (direction == Direction.Top) ? new OrthogonalSegment()
                        {
                            Type = ConnectorSegmentType.Orthogonal,
                            Direction = direction,
                            Length = Math.Abs(firstSegment.Points[0].Y - point.Y)
                        } :
                        new OrthogonalSegment()
                        {
                            Type = ConnectorSegmentType.Orthogonal,
                            Direction = direction,
                            Length = Math.Abs(point.Y - firstSegment.Points[0].Y)
                        };
                        segValues = (firstSegment.Points[0].X > point.X) ?
                                            new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = Direction.Right, Length = firstSegment.Points[0].X - point.X } :
                                            new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = Direction.Left, Length = (point.X - firstSegment.Points[0].X) };
                    }
                    else
                    {
                        segValues1 = (direction == Direction.Right) ? new OrthogonalSegment
                        {
                            Type = ConnectorSegmentType.Orthogonal,
                            Direction = direction,
                            Length = Math.Abs(firstSegment.Points[0].X - point.X)
                        } :
                           new OrthogonalSegment()
                           {
                               Type = ConnectorSegmentType.Orthogonal,
                               Direction = direction,
                               Length = Math.Abs(point.X - firstSegment.Points[0].X)
                           };
                        segValues = (firstSegment.Points[0].Y > point.Y) ?
                                            new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = Direction.Top, Length = (firstSegment.Points[0].Y - point.Y) } :
                                            new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = Direction.Bottom, Length = (point.Y - firstSegment.Points[0].Y) };
                    }
                    segments.Add(segValues1);
                    segments.Add(segValues);
                }
                else
                {
                    segValues = new OrthogonalSegment() { Type = ConnectorSegmentType.Orthogonal, Direction = direction, Length = 20 };
                    segments.Add(segValues);
                }
                if (firstSegment.Direction != ConnectorUtil.GetOppositeDirection(direction))
                {
                    if (direction == Direction.Top || direction == Direction.Bottom)
                    {
                        firstSegment.Points[0].X = point.X;
                        firstSegment.Points[0].Y = firstSegment.Points[^1].Y = (direction == Direction.Top) ?
                            point.Y - 20 : point.Y + 20;
                    }
                    else
                    {
                        firstSegment.Points[0].Y = point.Y;
                        firstSegment.Points[0].X = firstSegment.Points[^1].X = (direction == Direction.Right) ?
                            point.X + 20 : point.X - 20;
                    }
                    firstSegment.Length = DiagramPoint.DistancePoints(firstSegment.Points[0], firstSegment.Points[^1]);
                    secondSegment.Length = DiagramPoint.DistancePoints(
                        firstSegment.Points[^1], secondSegment.Points[^1]);
                }
                for (int i = 0; i < connector.SegmentCollection.Count; i++)
                {
                    segments.Add(connector.SegmentCollection[i]);
                }
                connector.SegmentCollection = segments;
            }
            else
            {
                firstSegment.Points[0] = point;
                if (direction == Direction.Top || direction == Direction.Bottom)
                {
                    firstSegment.Points[^1].X = point.X;

                }
                else
                {
                    firstSegment.Points[^1].Y = point.Y;
                }
                firstSegment.Length = DiagramPoint.DistancePoints(firstSegment.Points[0], firstSegment.Points[^1]);
                secondSegment.Length = DiagramPoint.DistancePoints(
                    firstSegment.Points[^1], secondSegment.Points[^1]);
            }
        }

        internal void ConnectorSegmentChange(Node actualObject, DiagramRect existingInnerBounds, bool isRotate)
        {
            double tx = 0; double ty = 0;
            if (!DiagramRect.Equals(existingInnerBounds, actualObject.Wrapper.Bounds))
            {
                if (actualObject.OutEdges.Any())
                {
                    for (int i = actualObject.OutEdges.Count - 1; i >= 0; i--)
                    {
                        if (actualObject.OutEdges[i].Contains("helper", StringComparison.InvariantCulture))
                        {
                            actualObject.OutEdges.Remove(actualObject.OutEdges[i]);
                        }
                    }
                    for (int k = 0; k < actualObject.OutEdges.Count; k++)
                    {

                        if (this.diagram.NameTable[actualObject.OutEdges[k]] is Connector connector)
                        {
                            bool segmentChange;
                            if (!string.IsNullOrEmpty(connector.TargetID))
                            {
                                segmentChange = !this.IsSelected(this.diagram.NameTable[connector.TargetID]);
                            }
                            else
                            {
                                segmentChange = !this.IsSelected(this.diagram.NameTable[connector.ID]);
                            }

                            if (connector.Type == ConnectorSegmentType.Orthogonal && connector.Segments != null &&
                                connector.Segments.Count > 1)
                            {
                                if (!isRotate)
                                {
                                    if (segmentChange)
                                    {
                                        DiagramRect bounds = actualObject.Wrapper.Bounds;
                                        Direction? direction = ((OrthogonalSegment)connector.SegmentCollection[0]).Direction;
                                        if (direction != null)
                                        {
                                            switch (direction
                                                .Value)
                                            {
                                                case Direction.Bottom:
                                                    tx = bounds.BottomCenter.X - existingInnerBounds.BottomCenter.X;
                                                    ty = bounds.BottomCenter.Y - existingInnerBounds.BottomCenter.Y;
                                                    break;
                                                case Direction.Top:
                                                    tx = bounds.TopCenter.X - existingInnerBounds.TopCenter.X;
                                                    ty = bounds.TopCenter.Y - existingInnerBounds.TopCenter.Y;
                                                    break;
                                                case Direction.Left:
                                                    tx = bounds.MiddleLeft.X - existingInnerBounds.MiddleLeft.X;
                                                    ty = bounds.MiddleLeft.Y - existingInnerBounds.MiddleLeft.Y;
                                                    break;
                                                case Direction.Right:
                                                    tx = bounds.MiddleRight.X - existingInnerBounds.MiddleRight.X;
                                                    ty = bounds.MiddleRight.Y - existingInnerBounds.MiddleRight.Y;
                                                    break;
                                            }
                                        }

                                        this.DragSourceEnd(connector, tx, ty, false, Actions.ConnectorSourceEnd, null,
                                            null, null, !this.diagram.RealAction.HasFlag(RealAction.PreventDragSource));
                                    }
                                }
                                else
                                {
                                    OrthogonalSegment firstSegment = connector.Segments[0] as OrthogonalSegment;
                                    Corners cornerPoints = ConnectorUtil.SwapBounds(
                                            actualObject.Wrapper, actualObject.Wrapper.Corners,
                                            actualObject.Wrapper.Bounds);
                                    DiagramPoint sourcePoint =
                                        ConnectorUtil.FindPoint(cornerPoints, firstSegment.Direction.Value);
                                    sourcePoint = ConnectorUtil.GetIntersection(
                                        connector, sourcePoint,
                                            new DiagramPoint
                                            {
                                                X = connector.SourceWrapper.OffsetX,
                                                Y = connector.SourceWrapper.OffsetY
                                            }, false);
                                    End source = new End
                                    {
                                        Corners = null,
                                        Point = sourcePoint,
                                        Margin = null,
                                        Direction = firstSegment.Direction
                                    };
                                    if (connector.Segments[1] is OrthogonalSegment secondSegment)
                                    {
                                        End target = new End
                                        {
                                            Corners = null,
                                            Point = secondSegment.Points[1],
                                            Margin = null,
                                            Direction = null
                                        };
                                        List<DiagramPoint> intermediatePoints =
                                            ConnectorUtil.OrthoConnection2Segment(source, target);
                                        firstSegment.Length =
                                            DiagramPoint.DistancePoints(intermediatePoints[0], intermediatePoints[1]);
                                        if (secondSegment.Direction.HasValue && secondSegment.Length.HasValue)
                                        {
                                            secondSegment.Length = DiagramPoint.DistancePoints(intermediatePoints[1],
                                                intermediatePoints[2]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal List<NodeBase> GetAllDescendants(NodeBase node, List<NodeBase> nodes, bool includeParent = false, bool innerParent = false)
        {
            List<NodeBase> parentNodes = new List<NodeBase>();
            if (node is NodeGroup group)
            {
                for (int i = 0; i < group.Children.Length; i++)
                {
                    if (this.diagram.NameTable.ContainsKey(group.ID))
                    {
                        node = this.diagram.NameTable[group.Children[i]] as NodeBase;
                        if (node != null)
                        {
                            if ((node is Connector) || !(node is NodeGroup groupObj) || ((groupObj is NodeGroup) && groupObj.Children == null))
                            {
                                nodes.Add(node);
                            }
                            else
                            {
                                if (includeParent)
                                {
                                    nodes.Add(groupObj);
                                }
                                if (innerParent)
                                {
                                    parentNodes.Add(groupObj);
                                }
                                nodes = this.GetAllDescendants(groupObj, nodes, false, false);
                            }
                        }
                    }
                }
            }
            return innerParent ? parentNodes : nodes;
        }
        internal bool Scale(NodeBase obj, double sw, double sh, DiagramPoint pivot, IDiagramObject refObject, bool isOutsideBoundary = false)
        {
            double x;
            double y;
            DiagramPoint refPoint = null;
            List<NodeBase> elements = new List<NodeBase>();
            if (this.diagram.NameTable[obj.ID] is NodeBase node)
            {
                DiagramElement element = node.Wrapper;
                if (refObject == null) { refObject = obj; }
                if (refObject is NodeBase nodeBase)
                {
                    DiagramContainer refWrapper = nodeBase.Wrapper;
                    x = refWrapper.OffsetX - BaseUtil.GetDoubleValue(refWrapper.ActualSize.Width) * refWrapper.Pivot.X;
                    y = refWrapper.OffsetY - BaseUtil.GetDoubleValue(refWrapper.ActualSize.Height) * refWrapper.Pivot.Y;
                    refPoint = DiagramUtil.GetPoint(x, y, BaseUtil.GetDoubleValue(refWrapper.ActualSize.Width), BaseUtil.GetDoubleValue(refWrapper.ActualSize.Height), refWrapper.RotationAngle, refWrapper.OffsetX, refWrapper.OffsetY, pivot);

                }
                else
                {
                    if (refObject is DiagramSelectionSettings refWrapper)
                    {
                        x = refWrapper.OffsetX - BaseUtil.GetDoubleValue(refWrapper.Width) * refWrapper.Pivot.X;
                        y = refWrapper.OffsetY - BaseUtil.GetDoubleValue(refWrapper.Height) * refWrapper.Pivot.Y;
                        refPoint = DiagramUtil.GetPoint(x, y, BaseUtil.GetDoubleValue(refWrapper.Width),
                            BaseUtil.GetDoubleValue(refWrapper.Height), refWrapper.RotationAngle, refWrapper.OffsetX,
                            refWrapper.OffsetY, pivot);
                    }
                }

                if (element.ActualSize.Width != null && element.ActualSize.Height != null && ConstraintsUtil.CanPageEditable(this.diagram))
                {
                    if (node is NodeGroup)
                    {
                        if (node is NodeGroup group && @group.Children != null)
                        {
                            double width = BaseUtil.GetDoubleValue(group.Wrapper.ActualSize.Width) * sw;
                            double height = BaseUtil.GetDoubleValue(group.Wrapper.ActualSize.Height) * sh;
                            if (group.MaxWidth < width)
                            {
                                sw = BaseUtil.GetDoubleValue(group.MaxWidth / group.Wrapper.ActualSize.Width);
                            }
                            if (group.MinWidth > width)
                            {
                                sw = BaseUtil.GetDoubleValue(group.MinWidth / group.Wrapper.ActualSize.Width);
                            }
                            if (group.MaxHeight < height)
                            {
                                sh = BaseUtil.GetDoubleValue(group.MaxHeight / group.Wrapper.ActualSize.Height);
                            }
                            if (group.MinHeight > height)
                            {
                                sh = BaseUtil.GetDoubleValue(group.MinHeight / group.Wrapper.ActualSize.Height);
                            }
                            List<NodeBase> nodes = this.GetAllDescendants(@group, elements, false, false);
                            foreach (NodeBase temp in nodes)
                            {
                                this.ScaleObject(sw, sh, refPoint, temp, refObject);
                            }

                            obj.Wrapper.Measure(new DiagramSize());
                            obj.Wrapper.Arrange(obj.Wrapper.DesiredSize);
                            this.diagram.DiagramContent.UpdateGroupOffset(node, false);
                            this.UpdateInnerParentProperties(@group);
                        }
                    }
                    else
                    {
                        this.ScaleObject(sw, sh, refPoint, node, refObject);
                    }
                    DiagramRect bounds = BaseUtil.GetBounds(obj.Wrapper);
                    bool checkBoundaryConstraints = this.CheckBoundaryConstraints(0, 0, bounds);
                    if (!checkBoundaryConstraints && isOutsideBoundary)
                    {
                        this.Scale(obj, 1 / sw, 1 / sh, pivot, null, true);
                        return false;
                    }
                }
            }

            return true;
        }
        internal bool ScalePreview(double sw, double sh, DiagramPoint pivot, DiagramSelectionSettings refObject, ref double width, ref double height, ref double offsetX, ref double offsetY, bool isOutsideBoundary = false)
        {
            double x = refObject.OffsetX - BaseUtil.GetDoubleValue(refObject.Width) * refObject.Pivot.X;
            double y = refObject.OffsetY - BaseUtil.GetDoubleValue(refObject.Height) * refObject.Pivot.Y;
            DiagramPoint refPoint = DiagramUtil.GetPoint(x, y, BaseUtil.GetDoubleValue(refObject.Width), BaseUtil.GetDoubleValue(refObject.Height), refObject.RotationAngle, refObject.OffsetX, refObject.OffsetY, pivot);
            if (ConstraintsUtil.CanPageEditable(this.diagram))
            {
                DiagramRect bounds = BaseUtil.GetSelectorBounds(refObject);
                bool checkBoundaryConstraints = this.CheckBoundaryConstraints(sw, sh, bounds);
                if (checkBoundaryConstraints)
                {
                    ScalePreviewObject(sw, sh, refPoint, refObject, ref width, ref height, ref offsetX, ref offsetY);
                }
                if (!checkBoundaryConstraints && isOutsideBoundary)
                {
                    this.ScalePreview(1 / sw, 1 / sh, pivot, null, ref width, ref height, ref offsetX, ref offsetY, true);
                    return false;
                }
            }
            return true;
        }
        internal bool CheckBoundaryConstraints(double tx, double ty, DiagramRect nodeBounds)
        {
            PageSettings pageSettings = this.diagram.PageSettings;
            BoundaryConstraints boundaryConstraints = pageSettings.BoundaryConstraints;
            Scroller scroller = this.diagram.Scroller;
            if (boundaryConstraints != BoundaryConstraints.Infinity)
            {
                DiagramRect selectorBounds = nodeBounds != null ? this.diagram.SelectionSettings.Wrapper.Bounds : null;
                double width = boundaryConstraints == BoundaryConstraints.Page ? BaseUtil.GetDoubleValue(pageSettings.Width) : scroller.ViewPortWidth;
                double height = boundaryConstraints == BoundaryConstraints.Page ? BaseUtil.GetDoubleValue(pageSettings.Height) : scroller.ViewPortHeight;
                if (selectorBounds != null)
                {
                    double right = (nodeBounds.Right) + tx;
                    double left = (nodeBounds.Left) + tx;
                    double top = (nodeBounds.Top) + ty;
                    double bottom = (nodeBounds.Bottom) + ty;
                    if (right <= width && left >= 0
                        && bottom <= height && top >= 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            return true;
        }
        internal void SameSize(DiagramObjectCollection<NodeBase> selectedItems, SizingMode sizingType)
        {
            if (selectedItems.Any())
            {
                DiagramPoint pivot = new DiagramPoint { X = 0.5, Y = 0.5 };
                NodeBase node = this.diagram.NameTable[selectedItems[0].ID] as NodeBase;
                selectedItems[0] = node ?? selectedItems[0];
                DiagramRect bounds = BaseUtil.GetBounds(selectedItems[0].Wrapper);
                for (int i = 1; i < selectedItems.Count; i++)
                {
                    {
                        object selectedNode = this.diagram.NameTable[selectedItems[i].ID];
                        selectedItems[i] = selectedNode != null ? selectedNode as NodeBase : selectedItems[0];
                        DiagramRect rect = BaseUtil.GetBounds(selectedItems[i].Wrapper);
                        double sw = 1;
                        double sh = 1;
                        if (sizingType == SizingMode.Width)
                        {
                            sw = bounds.Width / rect.Width;
                        }
                        else if (sizingType == SizingMode.Height)
                        {
                            sh = bounds.Height / rect.Height;
                        }
                        else if (sizingType == SizingMode.Size)
                        {
                            sw = bounds.Width / rect.Width;
                            sh = bounds.Height / rect.Height;
                        }
                        this.Scale(selectedItems[i], sw, sh, pivot, null, true);
                    }
                }
                this.UpdateSelector();
            }
        }
        internal void Distribute(DiagramObjectCollection<NodeBase> selectedItems, DistributeOptions option)
        {
            if (selectedItems.Any())
            {
                double right = 0, left = 0, top = 0, bottom = 0, center = 0, middle = 0, btt = 0;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    NodeBase node = this.diagram.NameTable[selectedItems[i].ID] as NodeBase;
                    selectedItems[i] = node ?? selectedItems[i];
                }
                selectedItems = DiagramUtil.SortByXY(selectedItems, option);
                for (int i = 1; i < selectedItems.Count; i++)
                {
                    DiagramRect bound = selectedItems[i].Wrapper.Bounds;
                    DiagramRect bounds = selectedItems[i - 1].Wrapper.Bounds;
                    right = right + bound.TopRight.X - bounds.TopRight.X;
                    left = left + bound.TopLeft.X - bounds.TopLeft.X;
                    top = top + bound.TopRight.Y - bounds.TopRight.Y;
                    bottom = bottom + bound.BottomRight.Y - bounds.BottomRight.Y;
                    center = center + bound.Center.X - bounds.Center.X;
                    middle = middle + bound.Center.Y - bounds.Center.Y;
                    btt = btt + bound.TopRight.Y - bounds.BottomRight.Y;
                }
                for (int i = 1; i < selectedItems.Count - 1; i++)
                {
                    double tx = 0;
                    double ty = 0;
                    DiagramRect prev = BaseUtil.GetBounds(selectedItems[i - 1].Wrapper);
                    DiagramRect current = BaseUtil.GetBounds(selectedItems[i].Wrapper);
                    if (option == DistributeOptions.RightToLeft || option == DistributeOptions.Center)
                    {
                        tx = prev.Center.X - current.Center.X + (center / (selectedItems.Count - 1));
                    }
                    else if (option == DistributeOptions.Right)
                    {
                        tx = prev.TopRight.X - current.TopRight.X + (right / (selectedItems.Count - 1));
                    }
                    else if (option == DistributeOptions.Left)
                    {
                        tx = prev.TopLeft.X - current.TopLeft.X + (left / (selectedItems.Count - 1));
                    }
                    else if (option == DistributeOptions.Middle)
                    {
                        ty = prev.Center.Y - current.Center.Y + (middle / (selectedItems.Count - 1));
                    }
                    else if (option == DistributeOptions.Top)
                    {
                        ty = prev.TopRight.Y - current.TopRight.Y + (top / (selectedItems.Count - 1));
                    }
                    else if (option == DistributeOptions.Bottom)
                    {
                        ty = prev.BottomRight.Y - current.BottomRight.Y + (bottom / (selectedItems.Count - 1));
                    }
                    else if (option == DistributeOptions.BottomToTop)
                    {
                        ty = prev.BottomRight.Y - current.TopRight.Y + (btt / (selectedItems.Count - 1));
                    }
                    this.Drag(selectedItems[i], tx, ty);
                    this.UpdateSelector();
                }
            }
        }

        internal void Align(DiagramObjectCollection<NodeBase> selectedItems, AlignmentOptions option, AlignmentMode type)
        {
            if (selectedItems.Any())
            {
                int i = 0;
                NodeBase node = this.diagram.NameTable[selectedItems[0].ID] as NodeBase;
                selectedItems[0] = node ?? selectedItems[0];
                DiagramRect bounds = (type == AlignmentMode.Object) ? BaseUtil.GetBounds(selectedItems[0].Wrapper) : this.diagram.SelectionSettings.Wrapper.Bounds;
                for (i = (type == AlignmentMode.Object) ? (i + 1) : i; i < selectedItems.Count; i++)
                {
                    double tx = 0;
                    double ty = 0;
                    NodeBase selectedNode = this.diagram.NameTable[selectedItems[i].ID] as NodeBase;
                    selectedItems[i] = selectedNode ?? selectedItems[i];
                    DiagramRect objectBounds = BaseUtil.GetBounds(selectedItems[i].Wrapper);
                    if (option == AlignmentOptions.Left)
                    {
                        tx = bounds.Left + objectBounds.Width / 2 - objectBounds.Center.X;
                    }
                    else if (option == AlignmentOptions.Right)
                    {
                        tx = bounds.Right - objectBounds.Width / 2 - objectBounds.Center.X;
                    }
                    else if (option == AlignmentOptions.Top)
                    {
                        ty = bounds.Top + objectBounds.Height / 2 - objectBounds.Center.Y;
                    }
                    else if (option == AlignmentOptions.Bottom)
                    {
                        ty = bounds.Bottom - objectBounds.Height / 2 - objectBounds.Center.Y;
                    }
                    else if (option == AlignmentOptions.Center)
                    {
                        tx = bounds.Center.X - objectBounds.Center.X;
                    }
                    else if (option == AlignmentOptions.Middle)
                    {
                        ty = bounds.Center.Y - objectBounds.Center.Y;
                    }
                    this.Drag(selectedItems[i], tx, ty);
                    this.UpdateSelector();
                }
            };
        }
        private void UpdateInnerParentProperties(NodeBase tempNode)
        {
            List<NodeBase> elements = new List<NodeBase>();
            bool protectChange = SfDiagramComponent.IsProtectedOnChange;
            SfDiagramComponent.IsProtectedOnChange = false;
            List<NodeBase> innerParents = this.GetAllDescendants(tempNode as Node, elements, false, true);
            for (int i = 0; i < innerParents.Count; i++)
            {
                if (this.diagram.NameTable[innerParents[i].ID] is Node obj)
                {
                    obj.OffsetX = obj.Wrapper.OffsetX;
                    obj.OffsetY = obj.Wrapper.OffsetY;
                    obj.Width = obj.Wrapper.Width;
                    obj.Height = obj.Wrapper.Height;
                }
            }
            SfDiagramComponent.IsProtectedOnChange = protectChange;
        }
        /// <summary>
        /// Apply scaling value for the preview item.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="sh"></param>
        /// <param name="pivot"></param>
        /// <param name="refObject"></param>
        /// <param name="uWidth"></param>
        /// <param name="uHeight"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        private static void ScalePreviewObject(double sw, double sh, DiagramPoint pivot, DiagramSelectionSettings refObject, ref double uWidth, ref double uHeight, ref double offsetX, ref double offsetY)
        {
            sw = sw < 0 ? 1 : sw;
            sh = sh < 0 ? 1 : sh;
            if (sw != 1 || sh != 1)
            {
                DiagramSelectionSettings node = refObject;
                var width = BaseUtil.GetDoubleValue(refObject.Width) * sw;
                var height = BaseUtil.GetDoubleValue(refObject.Height) * sh;

                sw = width / BaseUtil.GetDoubleValue(node.Width);
                sh = height / BaseUtil.GetDoubleValue(node.Height);
                Matrix matrix = Matrix.IdentityMatrix();

                DiagramSelectionSettings refWrapper = (refObject as DiagramSelectionSettings);
                Matrix.RotateMatrix(matrix, -refWrapper.RotationAngle, pivot.X, pivot.Y);
                Matrix.ScaleMatrix(matrix, sw, sh, pivot.X, pivot.Y);
                Matrix.RotateMatrix(matrix, refWrapper.RotationAngle, pivot.X, pivot.Y);
                node = refWrapper as DiagramSelectionSettings;
                DiagramPoint newPosition = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = node.OffsetX, Y = node.OffsetY });
                if (width > 0)
                {
                    uWidth = width; offsetX = newPosition.X;
                }
                if (height > 0)
                {
                    uHeight = height; offsetY = newPosition.Y;
                }

            }
        }
        internal void ScaleObject(double sw, double sh, DiagramPoint pivot, NodeBase obj, IDiagramObject refObject)
        {
            sw = sw < 0 ? 1 : sw;
            sh = sh < 0 ? 1 : sh;
            NodeBase oldValues = null;
            if (sw != 1 || sh != 1)
            {
                double width = 0; double height = 0;
                if (obj is Node nodeBase)
                {
                    bool isResize = false; DiagramRect bound = null;
                    oldValues = new Node()
                    {
                        Width = nodeBase.Wrapper.ActualSize.Width,
                        Height = nodeBase.Wrapper.ActualSize.Height,
                        OffsetX = nodeBase.Wrapper.OffsetX,
                        OffsetY = nodeBase.Wrapper.OffsetY,
                        Margin = new Margin() { Top = nodeBase.Margin.Top, Left = nodeBase.Margin.Left }
                    };
                    if (nodeBase.Shape.Type == Shapes.Bpmn && (nodeBase.Shape as BpmnShape)?.Activity.SubProcess.Processes != null
                                                        && ((BpmnShape)nodeBase.Shape).Activity.SubProcess.Processes.Count > 0)
                    {
                        bound = BpmnDiagrams.GetChildrenBound(nodeBase, nodeBase.ID, this.diagram);
                        isResize = nodeBase.Wrapper.Bounds.ContainsRect(bound);
                    }
                    width = BaseUtil.GetDoubleValue(nodeBase.Wrapper.ActualSize.Width) * sw;
                    height = BaseUtil.GetDoubleValue(nodeBase.Wrapper.ActualSize.Height) * sh;
                    if (nodeBase.MaxWidth != null && nodeBase.MaxWidth != 0)
                    {
                        width = Math.Min(BaseUtil.GetDoubleValue(nodeBase.MaxWidth), width);
                    }
                    if (nodeBase.MinWidth != null && nodeBase.MinWidth != 0)
                    {
                        width = Math.Max(BaseUtil.GetDoubleValue(nodeBase.MinWidth), width);
                    }
                    if (nodeBase.MaxHeight != null && nodeBase.MaxHeight != 0)
                    {
                        height = Math.Min(BaseUtil.GetDoubleValue(nodeBase.MaxHeight), height);
                    }
                    if (nodeBase.MinHeight != null && nodeBase.MinHeight != 0)
                    {
                        height = Math.Max(BaseUtil.GetDoubleValue(nodeBase.MinHeight), height);
                    }
                    if (isResize)
                    {
                        width = Math.Max(width, (bound.Right - nodeBase.Wrapper.Bounds.X));
                        height = Math.Max(height, (bound.Bottom - nodeBase.Wrapper.Bounds.Y));
                    }
                    sw = width / BaseUtil.GetDoubleValue(nodeBase.Wrapper.ActualSize.Width);
                    sh = height / BaseUtil.GetDoubleValue(nodeBase.Wrapper.ActualSize.Height);
                }
                Matrix matrix = Matrix.IdentityMatrix();
                if (refObject == null) { refObject = obj as Node; }
                DiagramElement refWrapper;
                if (refObject is NodeBase nodeObj)
                {
                    refWrapper = nodeObj.Wrapper;
                }
                else
                {
                    refWrapper = (refObject as DiagramSelectionSettings)?.Wrapper;
                }

                if (refWrapper != null)
                {
                    Matrix.RotateMatrix(matrix, -refWrapper.RotationAngle, pivot.X, pivot.Y);
                    Matrix.ScaleMatrix(matrix, sw, sh, pivot.X, pivot.Y);
                    Matrix.RotateMatrix(matrix, refWrapper.RotationAngle, pivot.X, pivot.Y);
                }

                if (obj is Node node)
                {
                    DiagramPoint newPosition = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = node.Wrapper.OffsetX, Y = node.Wrapper.OffsetY });
                    Node parent;
                    if (width > 0)
                    {
                        if (!string.IsNullOrEmpty(node.ProcessId))
                        {
                            parent = this.diagram.NameTable[node.ProcessId] as Node;
                            if (parent != null && (parent.MaxWidth == null || ((node.Margin.Left + width) < parent.MaxWidth)))
                            {
                                node.Width = width; node.OffsetX = newPosition.X;
                            }
                        }
                        else
                        {
                            node.Width = width; node.OffsetX = newPosition.X;
                        }
                    }
                    if (height > 0)
                    {
                        if (!string.IsNullOrEmpty(node.ProcessId))
                        {
                            parent = this.diagram.NameTable[node.ProcessId] as Node;
                            if (parent != null && (parent.MaxHeight == null || ((node.Margin.Top + height) < parent.MaxHeight)))
                            {
                                node.Height = height; node.OffsetY = newPosition.Y;
                            }
                        }
                        else
                        {
                            node.Height = height; node.OffsetY = newPosition.Y;
                        }
                    }
                    diagram.DiagramContent.UpdateNodeProperties(node, oldValues as Node);
                }
                else
                {
                    if (obj is Connector connector)
                    {
                        if (connector.SourceWrapper == null || connector.TargetWrapper == null)
                        {
                            this.ScaleConnector(connector, matrix);
                        }
                    }
                }
                IDiagramObject parentNode = (obj is Node node1) && !string.IsNullOrEmpty(node1.ProcessId) ? this.diagram.NameTable[node1.ProcessId] as Node : null;
                if (parentNode != null)
                {
                    _ = BpmnDiagrams.GetChildrenBound(parentNode, (obj as Node).ID, this.diagram);
                    BpmnDiagrams.UpdateSubProcesses((obj as Node), this.diagram);
                }
            }
        }
        private void ScaleConnector(Connector connector, Matrix matrix)
        {
            connector.SourcePoint = Matrix.TransformPointByMatrix(matrix, connector.SourcePoint);
            connector.TargetPoint = Matrix.TransformPointByMatrix(matrix, connector.TargetPoint);
            if (connector.Shape.Type == ConnectorShapeType.Bpmn && connector.Shape.Sequence == BpmnSequenceFlows.Default)
            {
                UpdatePathElementOffset(connector);
            }
            this.diagram.DiagramContent.UpdateConnectorProperties(connector);
        }

        private static void UpdatePathElementOffset(Connector connector)
        {
            connector.Wrapper.Children.RemoveAt(3);
            List<DiagramPoint> anglePoints = connector.IntermediatePoints;
            var pathElement = ConnectorUtil.UpdatePathElement(anglePoints, connector);
            connector.Wrapper.Children.Insert(3, pathElement);
        }

        internal void Group()
        {
            this.diagram.DiagramAction |= DiagramAction.Group;
            List<NodeBase> selectedItems = new List<NodeBase>();
            NodeGroup obj = new NodeGroup();
            obj.ID = "group" + obj.ID;
            selectedItems.AddRange(this.diagram.SelectionSettings.Nodes);
            selectedItems.AddRange(this.diagram.SelectionSettings.Connectors);
            List<string> list = new List<string>();
            for (int i = 0; i < selectedItems.Count; i++)
            {
                if (string.IsNullOrEmpty(selectedItems[i].ParentId))
                {
                    list.Add(selectedItems[i].ID);
                }
            }
            obj.Children = list.ToArray();
            this.diagram.Nodes.Add(obj);
            this.Select(obj, false);
            InternalHistoryEntry entry = new InternalHistoryEntry()
            {
                Type = HistoryEntryType.Group,
                RedoObject = obj,
                UndoObject = obj,
                Category = EntryCategory.InternalEntry,
            };
            diagram.AddHistoryEntry(entry);
            this.diagram.DiagramAction &= ~DiagramAction.Group;
        }

        internal void UnGroup(Node obj = null)
        {
            ObservableCollection<Node> selectedItems = new ObservableCollection<Node>();
            if (obj != null)
            {
                selectedItems.Add(obj);
            }
            else
            {
                selectedItems = this.diagram.SelectionSettings.Nodes;
            }
            if (selectedItems.Count > 0)
            {
                this.diagram.StartGroupAction();
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    if (selectedItems[i] is NodeGroup node)
                    {
                        IDiagramObject undoObject = node.Clone() as IDiagramObject;
                        CollectionChangingEventArgs args = new CollectionChangingEventArgs()
                        {
                            Cancel = false,
                            ActionTrigger = diagram.DiagramAction,
                            Element = node,
                            Action = CollectionChangedAction.Remove
                        };
                        InvokeDiagramEvents(DiagramEvent.CollectionChanging, args);
                        if (!args.Cancel)
                        {
                            diagram.RealAction |= RealAction.GroupingCollectionChange;
                            List<string> childCollection = new List<string>();
                            for (int k = 0; k < node.Children.Length; k++)
                            {
                                childCollection.Add(node.Children[k]);
                            }
                            if (node.Children != null && node.Children.Length > 0)
                            {
                                if (node.Ports != null && node.Ports.Count > 0)
                                {
                                    node.Ports.Clear();
                                }
                                if (node.Annotations != null && node.Annotations.Count > 0)
                                {
                                    node.Annotations.Clear();
                                }
                                NodeBase parentNode = string.IsNullOrEmpty(node.ParentId) ? null : this.diagram.NameTable[node.ParentId] as NodeBase;
                                for (int j = node.Children.Length - 1; j >= 0; j--)
                                {
                                    NodeBase child = this.diagram.NameTable[node.Children[j]] as NodeBase;
                                    child.ParentId = string.Empty;
                                    this.diagram.RemoveChild(node, child);
                                    if (parentNode != null && !string.IsNullOrEmpty(node.ParentId) && !string.IsNullOrEmpty(child.ID))
                                    {
                                        _ = this.diagram.AddChildExtend(parentNode, child);
                                    }
                                }
                                this.ResetDependentConnectors(node.InEdges, true);
                                this.ResetDependentConnectors(node.OutEdges, false);
                                InternalHistoryEntry entry = new InternalHistoryEntry()
                                {
                                    Type = HistoryEntryType.UnGroup,
                                    RedoObject = undoObject,
                                    UndoObject = undoObject,
                                    Category = EntryCategory.InternalEntry,
                                };
                                if (!(this.diagram.DiagramAction.HasFlag(DiagramAction.UndoRedo)))
                                    diagram.AddHistoryEntry(entry);
                                if (!string.IsNullOrEmpty(node.ParentId))
                                {
                                    this.diagram.RemoveChild(parentNode as NodeGroup, node);
                                }
                            }
                            this.diagram.Nodes.Remove(node);
                            diagram.RealAction &= ~RealAction.GroupingCollectionChange;
                            this.ClearSelection();
                        }
                    }

                    this.diagram.EndGroupAction();
                }
            }
        }
        private void ResetDependentConnectors(List<string> edges, bool isInEdges)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                Connector newConnector = this.diagram.NameTable[edges[i]] as Connector;
                if (newConnector != null)
                {
                    if (isInEdges)
                    {
                        newConnector.TargetID = string.Empty;
                        newConnector.TargetPortID = string.Empty;

                    }
                    else
                    {
                        newConnector.SourceID = string.Empty;
                        newConnector.SourcePortID = string.Empty;
                    }
                }
                this.diagram.DiagramContent.UpdateConnectorProperties(newConnector);
            }
        }

        internal void StartGroupAction()
        {
            this.diagram.StartGroupAction();
        }
        internal void EndGroupAction()
        {
            this.diagram.EndGroupAction();
        }
        internal void AddHistoryEntry(InternalHistoryEntry entry)
        {
            this.diagram.AddHistoryEntry(entry);
        }
        internal DiagramSelectionSettings GetSubProcess(IDiagramObject source)
        {
            DiagramSelectionSettings selector = new DiagramSelectionSettings()
            {
                Nodes = new ObservableCollection<Node>(),
                Connectors = new ObservableCollection<Connector>()
            };
            string process = string.Empty;
            if (source is Node node)
            {
                process = node.ProcessId;
            }
            else if (source is DiagramSelectionSettings sourceSelector && sourceSelector.Nodes != null && sourceSelector.Nodes.Any()
              && !string.IsNullOrEmpty(sourceSelector.Nodes[0].ProcessId))
            {
                process = sourceSelector.Nodes[0].ProcessId;
            }
            if (!string.IsNullOrEmpty(process))
            {
                if (this.diagram.NameTable[process] is Node element) selector.Nodes.Add(element.Clone() as Node);
                return selector;
            }
            return selector;
        }

        internal void Dispose()
        {
            if (diagram != null)
            {
                diagram = null;
            }

            if (clipboardData != null)
            {
                clipboardData.Dispose();
                clipboardData = null;
            }

            if (clonedItems != null)
            {
                for (int i = 0; i < clonedItems.Count; i++)
                {
                    clonedItems[i].Dispose();
                    clonedItems[i] = null;
                }
                clonedItems.Clear();
                clonedItems = null;
            }
            if (processTable != null)
            {
                processTable.Clear();
                processTable = null;
            }
            if (HelperObject != null)
            {
                HelperObject.Dispose();
                HelperObject = null;
            }
            if (this.PolygonObject != null)
            {
                this.PolygonObject.Dispose();
                this.PolygonObject = null;
            }
            if (HighlighterElement != null)
            {
                HighlighterElement.Dispose();
                HighlighterElement = null;
            }
        }
    }
    internal class ClipBoardObject
    {
        internal double PasteIndex { get; set; }
        internal List<NodeBase> ClipObject { get; set; }
        internal Dictionary<string, IDiagramObject> ChildTable { get; set; }
        internal Dictionary<string, IDiagramObject> ProcessTable { get; set; }

        internal void Dispose()
        {
            if (ClipObject != null)
            {
                for (int i = 0; i < ClipObject.Count; i++)
                {
                    ClipObject[i].Dispose();
                    ClipObject[i] = null;
                }
                ClipObject.Clear();
                ClipObject = null;
            }
            if (ChildTable != null)
            {
                ChildTable.Clear();
                ChildTable = null;
            }
            if (ProcessTable != null)
            {
                ProcessTable.Clear();
                ProcessTable = null;
            }
        }
    }
}
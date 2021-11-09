using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    internal class UndoRedo
    {
        private bool groupUndo;
        private double historyCount;
        private bool hasGroup;
        private double groupCount;
        internal static void InitHistory(SfDiagramComponent diagram)
        {
            diagram.SetDefaultHistory();
        }

        internal void AddHistoryEntry(HistoryEntryBase entry, SfDiagramComponent diagram)
        {
            HistoryAddingEventArgs args = new HistoryAddingEventArgs()
            {
                Entry = entry as HistoryEntryBase,
                Cancel = false
            };
            diagram.CommandHandler.InvokeDiagramEvents(DiagramEvent.HistoryAdding, args);
            if (args.Cancel)
            {
                return;
            }

            if (diagram.HistoryManager != null && diagram.HistoryManager.CanUndo && diagram.HistoryManager.CurrentEntry != null)
            {
                HistoryEntryBase entryObject = diagram.HistoryManager.CurrentEntry;
                if (entryObject.Next != null)
                {
                    if (entryObject.Previous != null)
                    {
                        HistoryEntryBase nextEntry = entryObject.Next;
                        nextEntry.Previous = null;
                        entryObject.Next = entry;
                        entry.Previous = entryObject;
                    }
                }
                else
                {
                    entryObject.Next = entry;
                    entry.Previous = entryObject;
                }
            }
            DiagramHistoryManager hList = diagram.HistoryManager;
            hList?.UpdateCurrentEntry(entry);
            if (diagram.HistoryManager != null && diagram.HistoryManager.StackLimit != 0)
            {
                if (entry.Type == HistoryEntryType.StartGroup || entry.Type == HistoryEntryType.EndGroup)
                {
                    bool value = entry.Type == HistoryEntryType.EndGroup;
                    this.SetEntryLimit(value);
                }
                if (!this.hasGroup && this.groupCount == 0)
                {
                    if (this.historyCount < diagram.HistoryManager.StackLimit)
                    {
                        this.historyCount++;
                    }
                    else
                    {
                        this.ApplyLimit(diagram.HistoryManager.CurrentEntry, diagram.HistoryManager.StackLimit, diagram);
                    }
                }
            }
            GetHistoryList(diagram);
            if (diagram.HistoryManager != null)
            {
                diagram.HistoryManager.CanUndo = true;
                diagram.HistoryManager.CanRedo = false;
            }
        }

        internal void ApplyLimit(HistoryEntryBase list, double stackLimit, SfDiagramComponent diagram, bool limitHistory = true)
        {
            if (list != null && list.Previous != null)
            {
                if (list.Type == HistoryEntryType.StartGroup || list.Type == HistoryEntryType.EndGroup)
                {
                    bool value = list.Type == HistoryEntryType.StartGroup;
                    this.SetEntryLimit(value);
                }
                if (!this.hasGroup && this.groupCount == 0)
                {
                    stackLimit--;
                }
                if (stackLimit == 0)
                {
                    if (limitHistory)
                    {
                        this.LimitHistoryStack(list.Previous, diagram);
                    }
                    if (diagram.HistoryManager.StackLimit < this.historyCount)
                    {
                        this.historyCount = diagram.HistoryManager.StackLimit;
                    }
                }
                else
                {
                    this.ApplyLimit(list.Previous, stackLimit, diagram, limitHistory);
                }
            }
            this.groupCount = 0;
        }

        internal void ClearHistory(SfDiagramComponent diagram)
        {
            DiagramHistoryManager hList = diagram.HistoryManager;
            hList.UpdateCurrentEntry(null);
            hList.CanUndo = false;
            hList.CanRedo = false;
            this.historyCount = 0;
            this.groupCount = 0;
            diagram.HistoryManager.UndoStack.Clear();
            diagram.HistoryManager.RedoStack.Clear();
        }

        private void SetEntryLimit(bool value)
        {
            if (value)
            {
                this.groupCount--;
                this.hasGroup = !value;
            }
            else
            {
                this.groupCount++;
                this.hasGroup = value;
            }
        }

        private void LimitHistoryStack(HistoryEntryBase list, SfDiagramComponent diagram)
        {
            if (list.Type != HistoryEntryType.StartGroup && list.Type != HistoryEntryType.EndGroup)
            {
                RemoveFromStack(diagram.HistoryManager.UndoStack, list);
                RemoveFromStack(diagram.HistoryManager.RedoStack, list);
            }
            if (list.Previous != null)
            {
                this.LimitHistoryStack(list.Previous, diagram);
            }
        }
        private static void RemoveFromStack(List<HistoryEntryBase> entryList, HistoryEntryBase list)
        {
            if (entryList.Count > 0)
            {
                for (int i = 0; i <= entryList.Count; i++)
                {
                    if (entryList[i].UndoObject == list.UndoObject && entryList[i].RedoObject == list.RedoObject)
                    {
                        entryList.RemoveRange(i, 1); break;
                    }
                }
            }
        }

        internal void Undo(SfDiagramComponent diagram)
        {
            HistoryEntryBase entry = GetUndoEntry(diagram);
            double endGroupActionCount = 0;
            if (entry != null)
            {
                if (entry.Category == EntryCategory.InternalEntry)
                {
                    if (entry.Type == HistoryEntryType.EndGroup)
                    {
                        endGroupActionCount++;
                        this.groupUndo = true;
                    }
                    else
                    {
                        _ = UndoEntry(entry, diagram);
                    }
                    if (this.groupUndo)
                    {
                        UndoGroupAction(entry, diagram, endGroupActionCount);
                        this.groupUndo = false;
                    }
                }
                else
                {
                    diagram.CommandHandler.InvokeDiagramEvents(DiagramEvent.Undo, entry);
                }
            }
        }
        private static void GetHistoryList(SfDiagramComponent diagram)
        {
            List<HistoryEntryBase> undoStack = new List<HistoryEntryBase>();
            List<HistoryEntryBase> redoStack = new List<HistoryEntryBase>();
            InternalHistoryEntry currentEntry = diagram.HistoryManager.CurrentEntry as InternalHistoryEntry;

            if (diagram.HistoryManager.CanUndo || diagram.HistoryManager.UndoStack.Count == 0)
            {
                GetHistroyObject(undoStack, currentEntry);
            }
            else
            {
                GetHistroyObject(redoStack, currentEntry);
            }

            while (currentEntry != null && currentEntry.Previous != null)
            {
                InternalHistoryEntry undoObj = currentEntry.Previous as InternalHistoryEntry;
                GetHistroyObject(undoStack, undoObj);
                currentEntry = currentEntry.Previous as InternalHistoryEntry;
            }

            currentEntry = diagram.HistoryManager.CurrentEntry as InternalHistoryEntry;
            while (currentEntry != null && currentEntry.Next != null)
            {
                InternalHistoryEntry redoObj = currentEntry.Next as InternalHistoryEntry;
                GetHistroyObject(redoStack, redoObj);
                currentEntry = currentEntry.Next as InternalHistoryEntry ;
            }
            diagram.HistoryManager.UndoStack = undoStack ;
            diagram.HistoryManager.RedoStack = redoStack;
        }

        private static void GetHistroyObject(List<HistoryEntryBase> list, HistoryEntryBase obj)
        {
            if (obj != null && obj.Type != HistoryEntryType.StartGroup && obj.Type != HistoryEntryType.EndGroup)
            {
                InternalHistoryEntry historyEntry = new InternalHistoryEntry()
                {
                    RedoObject = obj.RedoObject ?? null,
                    UndoObject = obj.UndoObject ?? null,
                    Type = obj.Type,
                    Category = obj.Category
                };
                list.Add(historyEntry);
            }
        }

        private static void UndoGroupAction(HistoryEntryBase entry, SfDiagramComponent diagram, double endGroupActionCount)
        {
            while (endGroupActionCount != 0)
            {
                _ = UndoEntry(entry, diagram);
                entry = GetUndoEntry(diagram);
                if (entry.Type == HistoryEntryType.StartGroup)
                {
                    endGroupActionCount--;
                }
                else if (entry.Type == HistoryEntryType.EndGroup)
                {
                    endGroupActionCount++;
                }
            }
        }

        private static async Task UndoEntry(HistoryEntryBase entry, SfDiagramComponent diagram)
        {
            if (entry.Type != HistoryEntryType.StartGroup && entry.Type != HistoryEntryType.EndGroup)
            {
                if (diagram.HistoryManager.UndoStack.Count > 0)
                {
                    List<HistoryEntryBase> addObject = new List<HistoryEntryBase>
                    {
                        diagram.HistoryManager.UndoStack[0]
                    };
                    diagram.HistoryManager.UndoStack.RemoveAt(0);
                    if (addObject.Count > 0)
                    {
                        diagram.HistoryManager.RedoStack.Insert(0, addObject[0]);
                    }
                }
            }
            bool isProtectedOnChange = SfDiagramComponent.IsProtectedOnChange;
            DiagramSelectionSettings obj = entry.UndoObject as DiagramSelectionSettings;
            SfDiagramComponent.IsProtectedOnChange = true;
            diagram.DiagramAction |= DiagramAction.UndoRedo;

            switch (entry.Type)
            {
                case HistoryEntryType.PositionChanged:
                    RecordPositionChanged(obj, diagram);
                    break;
                case HistoryEntryType.ConnectionChanged:
                    RecordConnectionChanged(obj, diagram);
                    break;
                case HistoryEntryType.PropertyChanged:
                    diagram.UndoRedoCount++;
                    await RecordPropertyChanged(entry, false, diagram);
                    diagram.UndoRedoCount--;
                    if (diagram.UndoRedoCount == 0)
                        diagram.DiagramAction &= ~DiagramAction.UndoRedo;
                    break;
                case HistoryEntryType.RotationChanged:
                    RecordRotationChanged(obj, diagram);
                    break;
                case HistoryEntryType.SizeChanged:
                    RecordSizeChanged(obj, diagram, entry);
                    break;
                case HistoryEntryType.CollectionChanged:
                    entry.IsUndo = true;
                    diagram.UndoRedoCount++;
                    await RecordCollectionChanged(entry, diagram);
                    entry.IsUndo = false;
                    diagram.UndoRedoCount--;
                    if (diagram.UndoRedoCount == 0)
                        diagram.DiagramAction &= ~DiagramAction.UndoRedo;
                    break;
                case HistoryEntryType.LabelCollectionChanged:
                    entry.IsUndo = true; RecordLabelCollectionChanged(entry, diagram); entry.IsUndo = false;
                    break;
                case HistoryEntryType.PortCollectionChanged:
                    entry.IsUndo = true; RecordPortCollectionChanged(entry, diagram); entry.IsUndo = false;
                    break;
                case HistoryEntryType.Group:
                    UnGroup(entry, diagram);
                    break;
                case HistoryEntryType.UnGroup:
                    Group(entry, diagram);
                    break;
                case HistoryEntryType.SegmentChanged:
                    RecordSegmentChanged(obj, diagram);
                    break;
            }
            if (entry.Type != HistoryEntryType.LabelCollectionChanged && entry.Type != HistoryEntryType.PortCollectionChanged && entry.Type != HistoryEntryType.CollectionChanged && entry.Type != HistoryEntryType.PropertyChanged)
                diagram.DiagramAction &= ~DiagramAction.UndoRedo;
            SfDiagramComponent.IsProtectedOnChange = isProtectedOnChange;
            diagram.DiagramContent.HistoryChangeTrigger(entry, HistoryChangedAction.Undo);
        }

        private static async Task RecordPropertyChanged(HistoryEntryBase entry, bool isRedo, SfDiagramComponent diagram)
        {
            PropertyChangedEventArgs evtArgs = (entry as InternalHistoryEntry).PropertyChangeEvtArgs;
            diagram.BeginUpdate();
            if (isRedo)
            {
                (evtArgs.Element as DiagramObject).Parent.OnPropertyChanged(evtArgs.PropertyName, evtArgs.NewValue, evtArgs.OldValue, evtArgs.Element);
            }
            else
            {
                (evtArgs.Element as DiagramObject).Parent.OnPropertyChanged(evtArgs.PropertyName, evtArgs.OldValue, evtArgs.NewValue, evtArgs.Element);
            }
            await diagram.EndUpdate();
        }
        private static void Group(HistoryEntryBase historyEntry, SfDiagramComponent diagram)
        {
            if (historyEntry.UndoObject is Node node)
            {
                diagram.Nodes.Add(node);
            }
            else if (historyEntry.UndoObject is Connector connector)
            {
                diagram.Connectors.Add(connector);
            }
        }
        private static void RecordSizeChanged(DiagramSelectionSettings obj, SfDiagramComponent diagram, HistoryEntryBase entry)
        {
            diagram.RealAction |= RealAction.PreventRefresh;
            int i;
            if (obj != null && obj.Nodes != null && obj.Nodes.Count > 0)
            {
                for (i = 0; i < obj.Nodes.Count; i++)
                {
                    Node node = obj.Nodes[i];
                    if (node is NodeGroup)
                    {
                        List<NodeBase> elements = new List<NodeBase>();
                        List<NodeBase> nodes = diagram.CommandHandler.GetAllDescendants(node, elements);
                        for (int j = 0; j < nodes.Count; j++)
                        {
                            NodeBase tempNode = (entry as InternalHistoryEntry).ChildTable[nodes[j].ID] as NodeBase;
                            if (tempNode is Node)
                            {
                                SizeChanged(tempNode as Node, diagram, entry);
                                PositionChanged(tempNode as Node, diagram);
                            }
                            else
                            {
                                ConnectionChanged(tempNode as Connector, diagram, entry);
                            }
                        }
                    }
                    else
                    {
                        SizeChanged(node, diagram, entry);
                        PositionChanged(node, diagram);
                    }
                }               
            }
            if (obj != null && obj.Connectors != null && obj.Connectors.Count > 0)
            {
                ObservableCollection<Connector> connectors = obj.Connectors;
                for (i = 0; i < connectors.Count; i++)
                {
                    Connector connector = connectors[i];
                    ConnectionChanged(connector, diagram);
                }
            }
            diagram.RealAction &= ~RealAction.PreventRefresh;
            diagram.DiagramStateHasChanged();
        }
        private static void SizeChanged(IDiagramObject obj, SfDiagramComponent diagram, HistoryEntryBase entry)
        {
            IDiagramObject node = diagram.NameTable[obj is Node ? (obj as Node).ID : obj is NodeGroup ? (obj as NodeGroup).ID : null];
            double? width = (obj as Node)?.Width;
            double? height = (obj as Node)?.Height;
            width ??= 50;
            height ??= 50;
            double scaleWidth = (double)(width / (node as Node)?.Width);
            double scaleHeight = (double)(height / (node as Node)?.Height);
            if (entry != null && (entry is InternalHistoryEntry) && (entry as InternalHistoryEntry).ChildTable != null)
            {
                (entry as InternalHistoryEntry).ChildTable[(obj as Node)?.ID] = node.Clone() as IDiagramObject;
            }
            diagram.Scale(node, scaleWidth, scaleHeight, new DiagramPoint()
            {
                X = ((Node)obj).OffsetX / (node as Node).OffsetX,
                Y = ((Node)obj).OffsetY / (node as Node).OffsetY
            });
        }

        private static void UnGroup(HistoryEntryBase entry, SfDiagramComponent diagram)
        {
            entry.RedoObject = entry.UndoObject.Clone() as NodeBase;
            Node node = entry.UndoObject as Node;
            diagram.CommandHandler.UnGroup(node);
        }
        private static void RecordPortCollectionChanged(HistoryEntryBase entry, SfDiagramComponent diagram)
        {
            PointPort port = entry.UndoObject as PointPort;
            NodeBase obj = entry.RedoObject as NodeBase;
            NodeBase node = diagram.NameTable[obj.ID] as NodeBase;
            if (entry != null && entry.ChangeType != HistoryEntryChangeType.None)
            {
                HistoryEntryChangeType changeType;
                if (entry.IsUndo)
                {
                    changeType = entry.ChangeType == HistoryEntryChangeType.Insert ? HistoryEntryChangeType.Remove : HistoryEntryChangeType.Insert;
                }
                else
                {
                    changeType = entry.ChangeType;
                }
                if (changeType == HistoryEntryChangeType.Remove)
                {
                    diagram.RemovePorts(node, new DiagramObjectCollection<PointPort>() { port });
                }
                else
                {
                    diagram.AddPorts(node, new DiagramObjectCollection<PointPort>() { port });
                }
                diagram.DiagramStateHasChanged();
            }
        }
        private static void RecordLabelCollectionChanged(HistoryEntryBase entry, SfDiagramComponent diagram)
        {
            Annotation label = entry.UndoObject as Annotation;
            NodeBase obj = entry.RedoObject as NodeBase;
            NodeBase node = diagram.NameTable[obj.ID] as NodeBase;
            if (entry != null && entry.ChangeType != HistoryEntryChangeType.None)
            {
                HistoryEntryChangeType changeType;
                if (entry.IsUndo)
                {
                    changeType = entry.ChangeType == HistoryEntryChangeType.Insert ? HistoryEntryChangeType.Remove : HistoryEntryChangeType.Insert;
                }
                else
                {
                    changeType = entry.ChangeType;
                }
                if (changeType == HistoryEntryChangeType.Remove)
                {
                    diagram.BeginUpdate();
                    diagram.RemoveLabels(node, new DiagramObjectCollection<Annotation>() { label });
                    _ = diagram.EndUpdate();
                }
                else
                {
                    diagram.AddLabels(node, new DiagramObjectCollection<Annotation>() { label });
                }
            }
        }
        private static async Task RecordCollectionChanged(HistoryEntryBase entry, SfDiagramComponent diagram)
        {
            IDiagramObject undoObject = entry.UndoObject;
            NodeBase obj = diagram.NameTable.ContainsKey((undoObject as NodeBase)?.ID) ? diagram.NameTable[(undoObject as NodeBase)?.ID] as NodeBase : undoObject as NodeBase;
            if (entry != null && entry.ChangeType != HistoryEntryChangeType.None)
            {
                HistoryEntryChangeType changeType;
                if (entry.IsUndo)
                {
                    changeType = entry.ChangeType == HistoryEntryChangeType.Insert ? HistoryEntryChangeType.Remove : HistoryEntryChangeType.Insert;
                }
                else
                {
                    changeType = entry.ChangeType;
                }
                if (changeType == HistoryEntryChangeType.Remove)
                {
                    if (obj is Node node)
                    {
                        diagram.Nodes.Remove(node);
                    }
                    else if (obj is Connector connector)
                    {
                        diagram.Connectors.Remove(connector);
                    }
                }
                else
                {
                    if (obj != null && obj is NodeBase && !string.IsNullOrEmpty(obj.ParentId))
                    {
                        NodeGroup parentNode = diagram.NameTable.ContainsKey(obj.ParentId) ? diagram.NameTable[obj.ParentId] as NodeGroup : null;
                        if (parentNode != null)
                        {
                            await diagram.AddChild(parentNode, obj);
                        }
                        else
                        {
                            await diagram.AddDiagramElements(new DiagramObjectCollection<NodeBase>() { obj });
                        }
                        diagram.DiagramAction &= ~DiagramAction.UndoRedo;
                    }
                    else if (!diagram.NameTable.ContainsKey(obj.ID))
                    {
                        await diagram.AddDiagramElements(new DiagramObjectCollection<NodeBase>() { obj });
                    }
                    if (obj is Node node && !string.IsNullOrEmpty(node.ProcessId) && diagram.NameTable.ContainsKey(node.ProcessId))
                    {
                        diagram.BpmnDiagrams.AddBpmnProcesses(node, node.ProcessId, diagram);
                    }
                }
                diagram.DiagramStateHasChanged();
            }
        }
        private static void RotationChanged(Node obj, SfDiagramComponent diagram)
        {
            if (diagram.NameTable[obj.ID] is Node node) diagram.Rotate(node, obj.RotationAngle - node.RotationAngle);
        }

        private static void RecordRotationChanged(DiagramSelectionSettings obj, SfDiagramComponent diagram)
        {
            diagram.BeginUpdate();
            DiagramSelectionSettings selectorObj = diagram.SelectionSettings;
            selectorObj.RotationAngle = obj.RotationAngle;
            if (selectorObj != null && selectorObj.Wrapper != null)
            {
                selectorObj.Wrapper.RotationAngle = obj.RotationAngle;
            }
            int i;
            if (obj != null && obj.Nodes != null && obj.Nodes.Count > 0)
            {
                for (i = 0; i < obj.Nodes.Count; i++)
                {
                    Node node = obj.Nodes[i];
                    RotationChanged(node, diagram);
                    PositionChanged(node, diagram);
                }
            }
            if (obj != null && obj.Connectors != null && obj.Connectors.Count > 0)
            {
                for (i = 0; i < obj.Connectors.Count; i++)
                {
                    Connector connector = obj.Connectors[i];
                    ConnectionChanged(connector, diagram);
                }
            }
            _ = diagram.EndUpdate();
        }

        private static void RecordPositionChanged(DiagramSelectionSettings obj, SfDiagramComponent diagram)
        {
            diagram.BeginUpdate();
            if (obj != null)
            {
                int i;
                if (obj.Nodes != null && obj.Nodes.Count > 0)
                {
                    for (i = 0; i < obj.Nodes.Count; i++)
                    {
                        Node node = obj.Nodes[i];
                        PositionChanged(node, diagram);
                    }
                }
                if (obj.Connectors != null && obj.Connectors.Count > 0)
                {
                    for (i = 0; i < obj.Connectors.Count; i++)
                    {
                        Connector connector = obj.Connectors[i];
                        ConnectionChanged(connector, diagram);
                    }
                }
            }
            _ = diagram.EndUpdate();
        }

        private static void PositionChanged(Node obj, SfDiagramComponent diagram)
        {
            Node node = (Node)diagram.NameTable[obj.ID];
            double tx = obj.OffsetX - node.OffsetX;
            double ty = obj.OffsetY - node.OffsetY;
            diagram.Drag(node, tx, ty);
        }

        private static void RecordConnectionChanged(DiagramSelectionSettings obj, SfDiagramComponent diagram)
        {
            Connector connector = obj.Connectors[0];
            diagram.BeginUpdate();
            ConnectionChanged(connector, diagram);
            _ = diagram.EndUpdate();
        }

        private static void ConnectionChanged(Connector obj, SfDiagramComponent diagram, HistoryEntryBase entry = null)
        {
            Connector connector = diagram.NameTable[obj.ID] as Connector;
            Node node;
            if (connector != null && obj.SourcePortID != connector.SourcePortID)
            {
                if (!string.IsNullOrEmpty(connector.SourceID))
                    DiagramLayerContent.RemoveEdges(diagram.NameTable[connector.SourceID] as Node, connector.SourcePortID, connector.ID, false);
                connector.SourcePortID = obj.SourcePortID;
            }
            if (connector != null && obj.TargetPortID != connector.TargetPortID)
            {
                if (!string.IsNullOrEmpty(connector.TargetID))
                    DiagramLayerContent.RemoveEdges(diagram.NameTable[connector.TargetID] as Node, connector.TargetPortID, connector.ID, true);
                connector.TargetPortID = obj.TargetPortID;
            }
            if (connector != null && obj.SourceID != connector.SourceID)
            {
                if (string.IsNullOrEmpty(obj.SourceID))
                {
                    node = diagram.NameTable[connector.SourceID] as Node;
                    if (node != null) DiagramUtil.RemoveItem(node.OutEdges, obj.ID);
                }
                else
                {
                    node = diagram.NameTable[obj.SourceID] as Node;
                    if (node != null)
                    {
                        node.OutEdges.Add(obj.ID);
                        DiagramLayerContent.UpdatePortEdges(node, obj, false);
                    }
                }
                connector.SourceID = obj.SourceID;
            }
            if (connector != null && obj.TargetID != connector.TargetID)
            {
                if (string.IsNullOrEmpty(obj.TargetID))
                {
                    node = diagram.NameTable[connector.TargetID] as Node;
                    if (node != null) DiagramUtil.RemoveItem(node.InEdges, obj.ID);
                }
                else
                {
                    node = diagram.NameTable[obj.TargetID] as Node;
                    if (node != null)
                    {
                        node.InEdges.Add(obj.ID);
                        DiagramLayerContent.UpdatePortEdges(node, obj, true);
                    }
                }
                connector.TargetID = obj.TargetID;
            }
            if ((entry as InternalHistoryEntry)?.ChildTable != null)
            {
                (entry as InternalHistoryEntry).ChildTable[obj.ID] = connector?.Clone() as Connector;
            }

            if (connector != null)
            {
                double sx = obj.SourcePoint.X - connector.SourcePoint.X;
                double sy = obj.SourcePoint.Y - connector.SourcePoint.Y;
                if (sx != 0 || sy != 0)
                {
                    diagram.CommandHandler.DragSourceEnd(connector, sx, sy);
                }
            }

            if (connector != null)
            {
                double tx = obj.TargetPoint.X - connector.TargetPoint.X;
                double ty = obj.TargetPoint.Y - connector.TargetPoint.Y;
                if (tx != 0 || ty != 0)
                {
                    diagram.CommandHandler.DragTargetEnd(connector, tx, ty);
                }
            }

            diagram.CommandHandler.UpdateSelector();

        }
        internal void Redo(SfDiagramComponent diagram)
        {
            HistoryEntryBase entry = GetRedoEntry(diagram);
            int startGroupActionCount = 0;
            if (entry != null)
            {
                if (entry.Category == EntryCategory.InternalEntry)
                {
                    if (entry.Type == HistoryEntryType.StartGroup)
                    {
                        startGroupActionCount++;
                        this.groupUndo = true;
                    }
                    else
                    {
                        _ = RedoEntry(entry, diagram);
                    }
                    if (this.groupUndo)
                    {
                        RedoGroupAction(entry, diagram, startGroupActionCount);
                        this.groupUndo = false;
                    }
                }
                else
                {
                    diagram.CommandHandler.InvokeDiagramEvents(DiagramEvent.Redo, entry);
                }
            }
        }

        private static void RedoGroupAction(HistoryEntryBase entry, SfDiagramComponent diagram, double startGroupActionCount)
        {
            while (startGroupActionCount != 0)
            {
                _ = RedoEntry(entry, diagram);
                entry = GetRedoEntry(diagram);
                if (entry.Type == HistoryEntryType.EndGroup)
                {
                    startGroupActionCount--;
                }
                else if (entry.Type == HistoryEntryType.StartGroup)
                {
                    startGroupActionCount++;
                }
            }
        }

        private static async Task RedoEntry(HistoryEntryBase historyEntry, SfDiagramComponent diagram)
        {
            bool isProtectedOnChange = SfDiagramComponent.IsProtectedOnChange;
            DiagramSelectionSettings redoObject = new DiagramSelectionSettings();
            if (historyEntry.Type != HistoryEntryType.PropertyChanged && historyEntry.Type != HistoryEntryType.CollectionChanged)
            {
                redoObject = (historyEntry.RedoObject) as DiagramSelectionSettings;
            }
            diagram.DiagramAction |= DiagramAction.UndoRedo;
            if (historyEntry.Type != HistoryEntryType.StartGroup && historyEntry.Type != HistoryEntryType.EndGroup)
            {
                if (diagram.HistoryManager.RedoStack.Count > 0)
                {
                    List<HistoryEntryBase> historyEntries = new List<HistoryEntryBase>();
                    List<HistoryEntryBase> addObject = historyEntries;
                    addObject.Add(diagram.HistoryManager.RedoStack[0]);
                    diagram.HistoryManager.RedoStack.RemoveAt(0);
                    if (addObject.Count > 0)
                    {
                        diagram.HistoryManager.UndoStack.Insert(0, addObject[0]);
                    }
                }
            }

            SfDiagramComponent.IsProtectedOnChange = true;

            switch (historyEntry.Type)
            {
                case HistoryEntryType.PositionChanged:
                    RecordPositionChanged(redoObject, diagram);
                    break;
                case HistoryEntryType.ConnectionChanged:
                    RecordConnectionChanged(redoObject, diagram);
                    break;
                case HistoryEntryType.PropertyChanged:
                    diagram.UndoRedoCount++;
                    await RecordPropertyChanged(historyEntry, true, diagram);
                    diagram.UndoRedoCount--;
                    if (diagram.UndoRedoCount == 0)
                        diagram.DiagramAction &= ~DiagramAction.UndoRedo;
                    break;
                case HistoryEntryType.RotationChanged:
                    RecordRotationChanged(redoObject, diagram);
                    break;
                case HistoryEntryType.SizeChanged:
                    RecordSizeChanged(redoObject, diagram, historyEntry);
                    break;
                case HistoryEntryType.CollectionChanged:
                    diagram.UndoRedoCount++;
                    await RecordCollectionChanged(historyEntry, diagram);
                    diagram.UndoRedoCount--;
                    if (diagram.UndoRedoCount == 0)
                        diagram.DiagramAction &= ~DiagramAction.UndoRedo;
                    break;
                case HistoryEntryType.LabelCollectionChanged:
                    RecordLabelCollectionChanged(historyEntry, diagram);
                    break;
                case HistoryEntryType.PortCollectionChanged:
                    RecordPortCollectionChanged(historyEntry, diagram);
                    break;
                case HistoryEntryType.Group:
                    Group(historyEntry, diagram);
                    break;
                case HistoryEntryType.UnGroup:
                    UnGroup(historyEntry, diagram);
                    break;
                case HistoryEntryType.SegmentChanged:
                    RecordSegmentChanged(redoObject, diagram);
                    break;
            }
            SfDiagramComponent.IsProtectedOnChange = isProtectedOnChange;
            if (historyEntry.Type != HistoryEntryType.LabelCollectionChanged && historyEntry.Type != HistoryEntryType.PortCollectionChanged && historyEntry.Type != HistoryEntryType.CollectionChanged && historyEntry.Type != HistoryEntryType.PropertyChanged)
                diagram.DiagramAction &= ~DiagramAction.UndoRedo;
            diagram.DiagramContent.HistoryChangeTrigger(historyEntry, HistoryChangedAction.Redo);
        }
        private static void RecordSegmentChanged(DiagramSelectionSettings obj, SfDiagramComponent diagram)
        {
            if (obj.Connectors != null && obj.Connectors.Count > 0)
            {
                for (int i = 0; i < obj.Connectors.Count; i++)
                {
                    Connector connector = obj.Connectors[i];
                    SegmentChanged(connector, diagram);
                }
            }
        }
        private static void SegmentChanged(Connector connector, SfDiagramComponent diagram)
        {
            if (diagram.NameTable[connector.ID] is Connector conn) conn.Segments = connector.Segments;
        }
        private static HistoryEntryBase GetUndoEntry(SfDiagramComponent diagram)
        {
            HistoryEntryBase undoEntry = null;
            DiagramHistoryManager hList = diagram.HistoryManager;
            if (hList.CanUndo)
            {
                undoEntry = hList.CurrentEntry;
                HistoryEntryBase currentObject = hList.CurrentEntry.Previous;
                if (currentObject != null)
                {
                    hList.UpdateCurrentEntry(currentObject);
                    if (!hList.CanRedo)
                    {
                        hList.CanRedo = true;
                    }
                }
                else
                {
                    hList.CanRedo = true;
                    hList.CanUndo = false;
                }
            }
            return undoEntry;
        }

        private static HistoryEntryBase GetRedoEntry(SfDiagramComponent diagram)
        {
            HistoryEntryBase redoEntry = null;
            DiagramHistoryManager hList = diagram.HistoryManager;
            if (hList.CanRedo)
            {
                HistoryEntryBase entryCurrent;
                if (hList.CurrentEntry.Previous == null && !hList.CanUndo)
                {
                    entryCurrent = hList.CurrentEntry;
                }
                else
                {
                    entryCurrent = hList.CurrentEntry.Next;
                }
                if (entryCurrent != null)
                {
                    hList.UpdateCurrentEntry(entryCurrent);
                    if (!hList.CanUndo)
                    {
                        hList.CanUndo = true;
                    }
                    if (entryCurrent.Next == null)
                    {
                        hList.CanRedo = false;
                        hList.CanUndo = true;
                    }
                }
                redoEntry = hList.CurrentEntry;
            }
            return redoEntry;
        }
    }
}

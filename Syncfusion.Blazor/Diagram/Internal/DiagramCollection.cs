using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Diagram.SymbolPalette;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when nodes or connectors or annotations or ports are added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class DiagramObjectCollection<T> : ObservableCollection<T>
    {
        private const string PORTS = "Ports";
        private const string ANNOTATIONS = "Annotations";
        private const string FIXEDUSERHANDLE = "FixedUserHandles";
        private const string GROUPCONTAINER = "group_container";
        private const string NODES = "Nodes";
        private const string CONNECTORS = "Connectors";
        private const string STOPS = "GradientStops";
        private bool groupAdded;
        private IDiagramObject parent;
        [JsonIgnore]
        internal IDiagramObject Parent
        {
            get => parent;
            set
            {
                parent = value;
                for (int i = 0; i < Count; i++)
                {
                    string propertyName;
                    if (Items[i] is Node) propertyName = NODES;
                    else if (Items[i] is Connector) propertyName = CONNECTORS;
                    else if (Items[i] is Annotation) propertyName = ANNOTATIONS;
                    else if (Items[i] is Port) propertyName = PORTS;
                    else if (Items[i] is NodeFixedUserHandle) propertyName = FIXEDUSERHANDLE;
                    else if (Items[i] is ConnectorFixedUserHandle) propertyName = FIXEDUSERHANDLE;
                    else if (Items[i] is GradientStop) propertyName = STOPS;
                    else if (Items[i] is KeyboardCommand) propertyName = "Commands";
                    else if (Items[i] is UserHandle) propertyName = "UserHandle";
                    else break;
                    if (Items[i] is DiagramObject)
                        (Items[i] as DiagramObject)?.SetParent(Parent, propertyName);
                }
            }
        }
        /// <summary>
        /// Add the objects to the diagram object collection asynchronously. It is applicable only to ports and annotations.
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Diagram Object</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// //Method for adding labels at runtime
        /// public async void AddLabel()
        /// {
        ///    ShapeAnnotation annotation = new ShapeAnnotation { Content = "Annotation" };
        ///    await (diagram.Nodes[0].Annotations as DiagramObjectCollection<ShapeAnnotation>).AddAsync(annotation);
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public async Task AddAsync(T item)
        {
            await Task.Run(() => { Items.Add(item); });
            List<IDiagramObject> list = new List<IDiagramObject>
            {
                item as IDiagramObject
            };
            if (item is Annotation)
            {
                await InitializeAnnotations(list);
            }
            else if (item is Port)
            {
                await InitializePorts(list);
            }
        }

        internal async Task AddCollection(DiagramObjectCollection<T> items, SfDiagramComponent diagram)
        {
            Dictionary<string, string> measurePathDataCollection = new Dictionary<string, string>() { };
            Dictionary<string, TextElementUtils> measureTextDataCollection = new Dictionary<string, TextElementUtils>() { };
            Dictionary<string, string> measureImageDataCollection = new Dictionary<string, string>() { };
            Dictionary<string, string> measureNativeDataCollection = new Dictionary<string, string>() { };
            await GetMeasureDataCollection(items, measurePathDataCollection, measureTextDataCollection, measureImageDataCollection, measureNativeDataCollection);
            await MeasureDataCollection(measurePathDataCollection, measureTextDataCollection, measureImageDataCollection);
            if (parent != null)
            {
                ((SfDiagramComponent)parent).RealAction |= RealAction.PreventPathDataMeasure | RealAction.PreventRefresh;
                foreach (var item in items)
                {
                    if (item is Node node)
                    {
                        diagram.Nodes.Add(node);
                    }
                    else
                    {
                        diagram.Connectors.Add(item as Connector);
                    }
                }
                ((SfDiagramComponent)parent).RealAction &= ~(RealAction.PreventPathDataMeasure | RealAction.PreventRefresh);
            }
            if (!diagram.IsBeginUpdate)
            {
                diagram.DiagramStateHasChanged();
            }
        }
        /// <summary>
        /// Initializes a new instance of the DiagramObjectCollection.
        /// </summary>
        public DiagramObjectCollection() : base()
        {
            this.CollectionChanged += ObservableCollectionChanged;
        }
        /// <summary>
        /// Creates a new instance of the DiagramObjectCollection from the given DiagramObjectCollection.
        /// </summary>
        /// <param name="collection">DiagramObjectCollection</param>
        public DiagramObjectCollection(IEnumerable<T> collection) : base(collection)
        {
            this.CollectionChanged += ObservableCollectionChanged;
        }
        /// <summary>
        /// Removes all the elements from the diagram object collection.
        /// </summary>
        protected override void ClearItems()
        {
            if (parent != null)
            {
                if (parent is SfDiagramComponent diagramComponent && (Items is IList<Node> || Items is IList<Connector>))
                {
                    diagramComponent.UpdateNameTable(this);
                }
                else if (Items is IList<ShapeAnnotation> || Items is IList<PointPort> || Items is IList<PathAnnotation>)
                {
                    ClearItemElements(this);
                }
            }
            base.ClearItems();
        }
        private async void ObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this is DiagramObjectCollection<Node> || this is DiagramObjectCollection<Connector> || this is DiagramObjectCollection<ShapeAnnotation> || this is DiagramObjectCollection<PathAnnotation> || this is DiagramObjectCollection<PointPort> || this is DiagramObjectCollection<NodeFixedUserHandle> || this is DiagramObjectCollection<ConnectorFixedUserHandle> || this is DiagramObjectCollection<BpmnAnnotation>)
            {
                if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (parent != null)
                    {
                        DiagramObject container = e.NewItems[0] as DiagramObject;
                        if (container is NodeBase)
                        {
                            await AddingNodesAndConnectors(e);
                        }
                        else if (container is Annotation || container is Port || container is NodeFixedUserHandle || container is ConnectorFixedUserHandle)
                        {
                            await AddingContainerElements(e, container);
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    RemoveCollectionChange(e);
                }
            }
            if (this is DiagramObjectCollection<UserHandle>)
            {
                if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (Parent is DiagramSelectionSettings selectorObj && selectorObj.UserHandles.Count > 0)
                    {
                        Dictionary<string, string> measurePathDataCollection = new Dictionary<string, string>() { };
                        foreach (UserHandle handle in selectorObj.UserHandles)
                        {
                            if (handle != null && !string.IsNullOrEmpty(handle.PathData) && Dictionary.GetMeasurePathBounds(handle.PathData) == null)
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
                                    (selectorObj.Parent as SfDiagramComponent)?.DiagramStateHasChanged();
                                }
                            }
                            measurePathDataCollection.Clear();
                        }
                    }
                }
            }
            if (parent is BpmnSubProcess bpmnSubProcess && !string.IsNullOrEmpty((bpmnSubProcess.Parent as BpmnActivity)?.SubProcess.processesID))
            {
                AddRemoveBpmnSubProcess(e);
            }
            await SymbolPaletteCollectionChanges(e);
            if (this is DiagramObjectCollection<KeyboardCommand>)
            {
                if (this.Parent is SfDiagramComponent diagram)
                    diagram.InitCommands();
            }
        }

        private void RemoveCollectionChange(NotifyCollectionChangedEventArgs e)
        {
            if (parent != null && e.OldItems.Count > 0)
            {
                if (parent is SfDiagramComponent && e.OldItems[0] is NodeBase)
                {
                    RemovingNodesAndConnectors(e);
                }
                if (e.OldItems[0] is Annotation || e.OldItems[0] is Port)
                {
                    if (!(Parent is BpmnShape) && !((SfDiagramComponent)((NodeBase)Parent).Parent).DiagramAction.HasFlag(DiagramAction.UndoRedo))
                    {
                        if (e.OldItems[0] is Annotation)
                        {
                            InternalHistoryEntry entry = new InternalHistoryEntry()
                            {
                                Type = HistoryEntryType.LabelCollectionChanged,
                                ChangeType = HistoryEntryChangeType.Remove,
                                RedoObject = ((NodeBase)Parent)?.Clone() as IDiagramObject,
                                UndoObject = (e.OldItems[0] as Annotation)?.Clone() as IDiagramObject,
                                Category = EntryCategory.InternalEntry,
                            };
                            ((SfDiagramComponent)(Parent as NodeBase)?.Parent)?.AddHistoryEntry(entry);
                        }
                        if (e.OldItems[0] is Port)
                        {
                            InternalHistoryEntry entry = new InternalHistoryEntry()
                            {
                                Type = HistoryEntryType.PortCollectionChanged,
                                ChangeType = HistoryEntryChangeType.Remove,
                                RedoObject = (Parent as NodeBase)?.Clone() as IDiagramObject,
                                UndoObject = (e.OldItems[0] as PointPort)?.Clone() as IDiagramObject,
                                Category = EntryCategory.InternalEntry,
                            };
                            ((Parent as NodeBase)?.Parent as SfDiagramComponent)?.AddHistoryEntry(entry);
                        }
                    }
                    if ((Parent is BpmnShape bpmnShape) && !((SfDiagramComponent)((Node)bpmnShape.Parent).Parent).DiagramAction.HasFlag(DiagramAction.UndoRedo))
                    {
                        if (e.OldItems[0] is Annotation)
                        {
                            InternalHistoryEntry entry = new InternalHistoryEntry()
                            {
                                Type = HistoryEntryType.LabelCollectionChanged,
                                ChangeType = HistoryEntryChangeType.Remove,
                                RedoObject = (bpmnShape.Parent as Node)?.Clone() as IDiagramObject,
                                UndoObject = (e.OldItems[0] as Annotation)?.Clone() as IDiagramObject,
                                Category = EntryCategory.InternalEntry,
                            };
                            (((Parent as BpmnShape)?.Parent as Node)?.Parent as SfDiagramComponent)?.AddHistoryEntry(entry);
                        }
                    }
                    if (Parent is BpmnShape)
                    {
                        ClearItemElements(e.OldItems);
                    }
                    else if (!(Parent is BpmnShape))
                    {
                        ClearItemElements(e.OldItems);
                    }

                }
            }
        }
        private async Task SymbolPaletteCollectionChanges(NotifyCollectionChangedEventArgs e)
        {
            if (parent is Palette palette && e.NewItems != null && e.NewItems[0] != null)
            {
                SfSymbolPaletteComponent paletteInstance = palette.Parent;
                if (e.NewItems[0] is NodeBase newObject)
                {
                    newObject.Parent = palette;
                    if (!paletteInstance.PaletteRealAction.HasFlag(RealAction.PreventEventRefresh))
                    {
                        paletteInstance.UpdateDictionaryValue(newObject);
                        await paletteInstance.PaletteMeasureBounds(null);
                        if (!paletteInstance.SymbolTable.ContainsKey(newObject.ID))
                        {
                            paletteInstance.SymbolTable.Add(newObject.ID, newObject);
                        }

                        paletteInstance.PrepareSymbols(newObject, true);
                    }
                }

                paletteInstance.SymbolPaletteStateHasChanged();
            }
            if (parent is SfSymbolPaletteComponent paletteComponent && e.NewItems != null && e.NewItems[0] != null)
            {
                paletteComponent.UpdateElements((e.NewItems[0] as Palette)?.Symbols);
                await paletteComponent.PaletteMeasureBounds(null);
                paletteComponent.AddPalette(e.NewItems[0] as Palette, true);
                if (paletteComponent.CanCallStateChange)
                {
                    paletteComponent.SymbolPaletteStateHasChanged();
                }
            }
        }
        private void AddRemoveBpmnSubProcess(NotifyCollectionChangedEventArgs e)
        {
            BpmnSubProcess bpmnSubProcess = parent as BpmnSubProcess;
            SfDiagramComponent diagram = (((bpmnSubProcess.Parent as BpmnActivity)?.Parent as BpmnShape).Parent as Node)?.Parent as SfDiagramComponent;
            Node node = (diagram.NameTable[(bpmnSubProcess.Parent as BpmnActivity)?.SubProcess.processesID] as Node);
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                string removedProcesses = (e.OldItems)[0] as string;
                Node removedNode = diagram.NameTable[removedProcesses] as Node;
                diagram.BpmnDiagrams.RemoveBpmnProcesses(removedNode, diagram);
                ObservableCollection<ICommonElement> containerChild = node.Wrapper.Children;
                for (int i = 0; i < containerChild.Count; i++)
                {
                    if (containerChild[i].ID == removedProcesses)
                    {
                        node.Wrapper.Children.Remove(containerChild[i]);
                        break;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (node != null && node.Shape.Type == Shapes.Bpmn && node.Parent is SfDiagramComponent diagramObject)
                {
                    if (node.Shape is BpmnShape nodeShape)
                    {
                        BpmnSubProcess subProcess = nodeShape.Activity.SubProcess;
                        if (subProcess.Processes != null && subProcess.Processes.Count > 0)
                        {
                            subProcess.processesID = node.ID;
                            DiagramObjectCollection<string> children = nodeShape.Activity.SubProcess.Processes;
                            foreach (string child in children)
                            {
                                if (diagramObject.NameTable[child] is Node node1 && (string.IsNullOrEmpty(node1.ProcessId) || node1.ProcessId == node.ID))
                                {
                                    node1.ProcessId = node.ID;
                                    double newHeight = BaseUtil.GetDoubleValue(node.Height);
                                    double newWidth = BaseUtil.GetDoubleValue(node.Width);
                                    double deltaWidth = 1;
                                    double deltaHeight = 1;
                                    if (node.Height < (node1.Margin.Top + node1.Height))
                                    {
                                        newHeight += BaseUtil.GetDoubleValue(((node1.Margin.Top + node1.Height) - node.Height));
                                        deltaHeight = (newHeight / BaseUtil.GetDoubleValue(node.Height));
                                    }
                                    if (node.Width < (node1.Margin.Left + node1.Width))
                                    {
                                        newWidth += BaseUtil.GetDoubleValue(((node1.Margin.Left + node1.Width) - node.Width));
                                        deltaWidth = (newWidth / BaseUtil.GetDoubleValue(node.Width));
                                    }
                                    if (deltaWidth > 1 && deltaHeight > 1)
                                    {
                                        diagramObject.Scale(node, deltaWidth, deltaHeight, new DiagramPoint() { X = 0, Y = 0 });
                                    }
                                    else if (deltaWidth > 1)
                                    {
                                        diagramObject.Scale(node, deltaWidth, deltaHeight, new DiagramPoint() { X = 0, Y = 0.5 });
                                    }
                                    else if (deltaHeight > 1)
                                    {
                                        diagramObject.Scale(node, deltaWidth, deltaHeight, new DiagramPoint() { X = 0.5, Y = 0 });
                                    }
                                }
                            }
                            bpmnSubProcess.processesID = node.ID;
                        }
                    }
                }
            }
        }
        private void RemovingNodesAndConnectors(NotifyCollectionChangedEventArgs e)
        {
            if (parent is SfDiagramComponent diagramComponent && !(diagramComponent.RealAction.HasFlag(RealAction.CancelCollectionChange)))
            {
                CollectionChangingEventArgs args = new CollectionChangingEventArgs()
                {
                    Cancel = false,
                    ActionTrigger = diagramComponent.DiagramAction,
                    Action = CollectionChangedAction.Remove,
                    Element = e.OldItems[0] as NodeBase
                };
                if (!diagramComponent.RealAction.HasFlag(RealAction.GroupingCollectionChange))
                    diagramComponent.CommandHandler.InvokeDiagramEvents(DiagramEvent.CollectionChanging, args);
                if (!args.Cancel)
                {
                    NodeBase node = e.OldItems[0] as NodeBase;
                    if (!diagramComponent.DiagramAction.HasFlag(DiagramAction.UndoRedo))
                    {
                        if (diagramComponent.Constraints.HasFlag(DiagramConstraints.UndoRedo) && !diagramComponent.GroupAction && (node is NodeGroup) && (node as NodeGroup).Children.Length > 0)
                        {
                            diagramComponent.StartGroupAction();
                            diagramComponent.GroupAction = true;
                        }
                    }
                    if (node is Node nodeBase)
                    {
                        diagramComponent.BpmnDiagrams?.CheckAndRemoveAnnotations(nodeBase, diagramComponent);
                        if (!diagramComponent.DiagramAction.HasFlag(DiagramAction.UndoRedo))
                        {
                            Connector connector;
                            if (nodeBase.InEdges.Count > 0)
                            {
                                for (int i = nodeBase.InEdges.Count - 1; i >= 0; i--)
                                {
                                    if (diagramComponent.NameTable.ContainsKey(nodeBase.InEdges[i]))
                                    {
                                        connector = diagramComponent.NameTable[nodeBase.InEdges[i]] as Connector;
                                        if (connector != null && connector.TargetID == nodeBase.ID)
                                        {
                                            diagramComponent.Connectors.Remove(connector);
                                        }
                                    }
                                }
                            }
                            if (nodeBase.OutEdges.Count > 0)
                            {
                                for (int i = nodeBase.OutEdges.Count - 1; i >= 0; i--)
                                {
                                    if (diagramComponent.NameTable.ContainsKey(nodeBase.OutEdges[i]))
                                    {
                                        connector = diagramComponent.NameTable[nodeBase.OutEdges[i]] as Connector;
                                        if (connector != null && connector.SourceID == nodeBase.ID)
                                        {
                                            if (diagramComponent.Connectors.Contains(connector))
                                            {
                                                diagramComponent.Connectors.Remove(connector);
                                            }
                                            if (diagramComponent.DiagramContent.TextAnnotationConnectors != null &&
                                               diagramComponent.DiagramContent.TextAnnotationConnectors.Contains(connector))
                                            {
                                                diagramComponent.DiagramContent.TextAnnotationConnectors.Remove(connector);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if ((!diagramComponent.DiagramAction.HasFlag(DiagramAction.UndoRedo)) &&
                        (!diagramComponent.RealAction.HasFlag(RealAction.GroupingCollectionChange)))
                    {
                        InternalHistoryEntry entry = new InternalHistoryEntry()
                        {
                            Type = HistoryEntryType.CollectionChanged,
                            ChangeType = HistoryEntryChangeType.Remove,
                            RedoObject = (e.OldItems[0] as NodeBase)?.Clone() as IDiagramObject,
                            UndoObject = (e.OldItems[0] as NodeBase)?.Clone() as IDiagramObject,
                            Category = EntryCategory.InternalEntry,
                        };
                        diagramComponent.AddHistoryEntry(entry);
                    }
                    if (node != null)
                    {
                        if (node is NodeGroup && !diagramComponent.DiagramAction.HasFlag(DiagramAction.UndoRedo))
                        {
                            foreach (string child in (node as NodeGroup).Children)
                            {
                                if (diagramComponent.NameTable.ContainsKey(child))
                                {
                                    IDiagramObject childNode = diagramComponent.NameTable[child];
                                    if (childNode is Node node1)
                                        diagramComponent.Nodes.Remove(node1);
                                    else
                                        diagramComponent.Connectors.Remove(childNode as Connector);
                                }
                            }
                            groupAdded = true;
                        }
                        bool isProtectedOnChange = SfDiagramComponent.IsProtectedOnChange;
                        SfDiagramComponent.IsProtectedOnChange = false;
                        diagramComponent.UpdateNameTable(e.OldItems);
                        SfDiagramComponent.IsProtectedOnChange = isProtectedOnChange;
                        if (!string.IsNullOrEmpty(node.ParentId) && diagramComponent.NameTable.ContainsKey(node.ParentId))
                        {
                            NodeGroup parentNode = diagramComponent.NameTable[node.ParentId] as NodeGroup;
                            diagramComponent.RemoveChild(parentNode, node);
                        }

                        if (node is Connector)
                        {
                            Node connectedNode;
                            if (!string.IsNullOrEmpty((node as Connector).SourceID) && diagramComponent.NameTable.ContainsKey((node as Connector).SourceID))
                            {
                                connectedNode = diagramComponent.NameTable[(node as Connector).SourceID] as Node;
                                connectedNode.OutEdges.Remove((node as Connector).ID);
                                DiagramLayerContent.RemoveEdges(connectedNode, ((node as Connector).SourcePortID), ((node as Connector).ID), false);
                            }
                            if (!string.IsNullOrEmpty((node as Connector).TargetID) && diagramComponent.NameTable.ContainsKey((node as Connector).TargetID))
                            {
                                connectedNode = diagramComponent.NameTable[(node as Connector).TargetID] as Node;
                                connectedNode.InEdges.Remove((node as Connector).ID);
                                DiagramLayerContent.RemoveEdges(connectedNode, ((node as Connector).TargetPortID), ((node as Connector).ID), true);
                            }
                        }
                        if (diagramComponent.SelectionSettings.Nodes.Contains(node as Node))
                            diagramComponent.SelectionSettings.Nodes.Remove(node as Node);
                        if (diagramComponent.SelectionSettings.Connectors.Contains(node as Connector))
                            diagramComponent.SelectionSettings.Connectors.Remove(node as Connector);

                    }
                    if (diagramComponent.GroupAction && groupAdded)
                    {
                        diagramComponent.EndGroupAction();
                        diagramComponent.GroupAction = false;
                        groupAdded = true;
                    }
                    CollectionChangedEventArgs collectionChangedEventArgs = new CollectionChangedEventArgs()
                    {
                        ActionTrigger = diagramComponent.DiagramAction,
                        Action = CollectionChangedAction.Remove,
                        Element = e.OldItems[0] as NodeBase
                    };
                    if (!diagramComponent.RealAction.HasFlag(RealAction.GroupingCollectionChange))
                        diagramComponent.CommandHandler.InvokeDiagramEvents(DiagramEvent.CollectionChanged, collectionChangedEventArgs);
                }
                else
                {
                    diagramComponent.RealAction |= RealAction.CancelCollectionChange;
                    if (e.OldItems[0] is Node)
                    {
                        diagramComponent.NodeCollection.Add(e.OldItems[0] as Node);
                    }
                    else if (e.OldItems[0] is Connector)
                    {
                        diagramComponent.ConnectorCollection.Add(e.OldItems[0] as Connector);
                    }
                    diagramComponent.RealAction &= ~RealAction.CancelCollectionChange;
                }
            }
        }

        private async Task AddingContainerElements(NotifyCollectionChangedEventArgs e, DiagramObject container)
        {
            ICommonElement parentWrapper = null;
            if (!(parent is BpmnShape))
            {
                parentWrapper = (parent is Node node) ? node.Wrapper : ((Connector)parent).Wrapper;
            }
            else if (parent is BpmnShape bpmnShape)
            {
                parentWrapper = (bpmnShape.Parent as Node)?.Wrapper;
            }

            if (parentWrapper != null)
            {
                if (container is Annotation)
                {
                    await InitializeAnnotations(e.NewItems);
                }
                else if (container is NodeFixedUserHandle)
                    await InitializeFixedUserHandle(e.NewItems);
                else if (container is ConnectorFixedUserHandle)
                    await InitializeFixedUserHandle(e.NewItems);
                else
                {
                    await InitializePorts(e.NewItems);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems.Count > 0)
            {
                ClearItemElements(e.OldItems);
            }
        }

        private async Task AddingNodesAndConnectors(NotifyCollectionChangedEventArgs e)
        {
            if (parent is SfDiagramComponent diagramComponent && !diagramComponent.RealAction.HasFlag(RealAction.CancelCollectionChange))
            {
                CollectionChangingEventArgs args = new CollectionChangingEventArgs()
                {
                    Cancel = false,
                    ActionTrigger = diagramComponent.DiagramAction,
                    Action = CollectionChangedAction.Add,
                    Element = e.NewItems[0] as NodeBase
                };
                diagramComponent.CommandHandler.InvokeDiagramEvents(DiagramEvent.CollectionChanging, args);
                if (!args.Cancel)
                {
                    await InitializeNodesAndConnectors(e.NewItems);

                    if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems.Count > 0 && e.OldItems[0] is Node)
                    {
                        diagramComponent.UpdateNameTable(e.OldItems);
                    }

                   CollectionChangedEventArgs collectionChangedEventArgs = new CollectionChangedEventArgs()
                    {
                        ActionTrigger = diagramComponent.DiagramAction,
                        Action = CollectionChangedAction.Add,
                        Element = e.NewItems[0] as NodeBase
                    };
                    diagramComponent.CommandHandler.InvokeDiagramEvents(DiagramEvent.CollectionChanged, collectionChangedEventArgs);
                }
                else
                {
                    diagramComponent.RealAction |= RealAction.CancelCollectionChange;
                    if (e.NewItems[0] is Node)
                    {
                        diagramComponent.NodeCollection.Remove(e.NewItems[0] as Node);
                    }
                    else if (e.NewItems[0] is Connector)
                    {
                        diagramComponent.ConnectorCollection.Remove(e.NewItems[0] as Connector);
                    }
                    diagramComponent.RealAction &= ~RealAction.CancelCollectionChange;
                }
            }
        }

        private async Task GetMeasureDataCollection(IList items, Dictionary<string, string> measurePathDataCollection, Dictionary<string, TextElementUtils> measureTextDataCollection, Dictionary<string, string> measureImageDataCollection, Dictionary<string, string> measureNativeDataCollection)
        {
            for (int i = 0; i < items.Count; i++)
            {
                NodeBase node = items[i] as NodeBase;
                if (parent != null)
                {
                    await GetDefaultValues(node);
                    if (node is Node)
                        DiagramLayerContent.GetMeasureNodeData(node as Node, measurePathDataCollection, measureTextDataCollection, measureImageDataCollection, measureNativeDataCollection);
                    else
                        DiagramLayerContent.GetMeasureConnectorData(node as Connector, measurePathDataCollection);
                }
            }
        }
        internal async Task InitializeNodesAndConnectors(IList items)
        {
            Dictionary<string, string> measurePathDataCollection = new Dictionary<string, string>() { };
            Dictionary<string, TextElementUtils> measureTextDataCollection = new Dictionary<string, TextElementUtils>() { };
            Dictionary<string, string> measureImageDataCollection = new Dictionary<string, string>() { };
            Dictionary<string, string> measureNativeDataCollection = new Dictionary<string, string>() { };
            if (parent != null && !((SfDiagramComponent)parent).RealAction.HasFlag(RealAction.PreventPathDataMeasure))
                _ = GetMeasureDataCollection(items, measurePathDataCollection, measureTextDataCollection, measureImageDataCollection, measureNativeDataCollection);
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (parent is SfDiagramComponent diagramComponent && !diagramComponent.DiagramAction.HasFlag(DiagramAction.UndoRedo))
                    {
                        if (items[i] is NodeBase nodeBase)
                        {
                            InternalHistoryEntry entry = new InternalHistoryEntry()
                            {
                                Type = HistoryEntryType.CollectionChanged,
                                ChangeType = HistoryEntryChangeType.Insert,
                                RedoObject = nodeBase.Clone() as IDiagramObject,
                                UndoObject = nodeBase.Clone() as IDiagramObject,
                                Category = EntryCategory.InternalEntry,
                            };
                            diagramComponent.AddHistoryEntry(entry);
                        }
                    }
                }
                await InitObjects(items, measurePathDataCollection, measureTextDataCollection, this is DiagramObjectCollection<Node> ? "Nodes" : "Connectors", measureImageDataCollection);
            }
        }
        internal async Task InitializeAnnotations(IList items)
        {
            if (items != null)
            {
                Dictionary<string, TextElementUtils> measureTextDataCollection = new Dictionary<string, TextElementUtils>() { };
                for (int i = 0; i < items.Count; i++)
                {
                    Annotation annotation = items[i] as Annotation;
                    if (parent != null && !(parent is BpmnShape) && (parent as NodeBase)?.Parent != null)
                    {
                        DiagramLayerContent.AddAnnotationToHistory(parent as NodeBase, annotation);
                        DiagramLayerContent.AddMeasureTextCollection(parent as NodeBase, annotation, null, measureTextDataCollection);
                    }
                    else if (parent is BpmnShape bpmnShape)
                    {
                        NodeBase parentNode = bpmnShape.Parent as NodeBase;
                        DiagramLayerContent.AddAnnotationToHistory(parentNode, annotation);
                        DiagramLayerContent.AddMeasureTextCollection(parentNode, annotation, null, measureTextDataCollection);
                    }
                }
                await InitObjects(items, null, measureTextDataCollection, "Annotations", null);
            }
        }
        internal async Task InitializeFixedUserHandle(IList items)
        {
            if (items != null)
            {
                Dictionary<string, string> measurePathDataCollection = new Dictionary<string, string>() { };
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] is FixedUserHandle userHandle)
                    {
                        if (userHandle.PathData != null && (parent as Node)?.Parent != null)
                        {
                            DiagramLayerContent.AddMeasurePathDataCollection(userHandle.PathData, measurePathDataCollection);
                        }
                        else if (userHandle.PathData != null && (parent as Connector)?.Parent != null)
                        {
                            DiagramLayerContent.AddMeasurePathDataCollection(userHandle.PathData, measurePathDataCollection);
                        }
                        else if (userHandle.PathData != null && (parent as Connector)?.Parent != null)
                        {
                            DiagramLayerContent.AddMeasurePathDataCollection(userHandle.PathData, measurePathDataCollection);
                        }
                    }

                }
                await InitObjects(items, measurePathDataCollection, null, FIXEDUSERHANDLE, null);
            }
        }
        internal async Task InitializePorts(IList items)
        {
            if (items != null)
            {
                Dictionary<string, string> measurePathDataCollection = new Dictionary<string, string>() { };
                for (int i = 0; i < items.Count; i++)
                {
                    Port port = items[i] as Port;
                    NodeBase nodeBase = Parent as NodeBase;
                    if (nodeBase?.Parent is SfDiagramComponent diagramComponent && !diagramComponent.DiagramAction.HasFlag(DiagramAction.UndoRedo))
                    {
                        InternalHistoryEntry entry = new InternalHistoryEntry()
                        {
                            Type = HistoryEntryType.PortCollectionChanged,
                            ChangeType = HistoryEntryChangeType.Insert,
                            RedoObject = (IDiagramObject)nodeBase.Clone(),
                            UndoObject = (IDiagramObject)port?.Clone(),
                            Category = EntryCategory.InternalEntry,
                        };
                        diagramComponent.AddHistoryEntry(entry);
                    }
                    if (port != null && port.Shape == PortShapes.Custom && port.PathData != null && nodeBase?.Parent != null)
                    {
                        DiagramLayerContent.AddMeasurePathDataCollection(port.PathData, measurePathDataCollection);
                    }
                }
                if ((Parent as NodeBase)?.Parent is SfDiagramComponent diagram)
                    diagram.DiagramAction &= ~DiagramAction.UndoRedo;
                await InitObjects(items, measurePathDataCollection, null, PORTS, null);
            }
        }
        private async Task MeasureDataCollection(Dictionary<string, string> measurePathDataCollection, Dictionary<string, TextElementUtils> measureTextDataCollection, Dictionary<string, string> measureImageDataCollection)
        {
            SfDiagramComponent diagram;
            if (Parent is BpmnShape)
            {
                diagram = ((Parent as BpmnShape)?.Parent as Node)?.Parent as SfDiagramComponent;
            }
            else
            {
                diagram = Parent is SfDiagramComponent ? Parent as SfDiagramComponent : (Parent as NodeBase)?.Parent as SfDiagramComponent;
            }
            if (diagram != null)
            {
                if (!diagram.DiagramAction.HasFlag(DiagramAction.UndoRedo) && !diagram.DiagramAction.HasFlag(DiagramAction.EditText) && ((measurePathDataCollection != null && measurePathDataCollection.Count > 0) || (measureTextDataCollection != null && measureTextDataCollection.Count > 0) || (measureImageDataCollection != null && measureImageDataCollection.Count > 0)))
                    await MeasureData(measurePathDataCollection, measureTextDataCollection, measureImageDataCollection);
                if (!diagram.RealAction.HasFlag(RealAction.PreventPathDataMeasure))
                {
                    await diagram.DiagramContent.MeasureCustomPathPoints();
                }
            }
        }

        private async Task InitObjects(IList items, Dictionary<string, string> measurePathDataCollection, Dictionary<string, TextElementUtils> measureTextDataCollection, string name, Dictionary<string, string> measureImageDataCollection)
        {
            await MeasureDataCollection(measurePathDataCollection, measureTextDataCollection, measureImageDataCollection);
            SfDiagramComponent.IsProtectedOnChange = false;
            SfDiagramComponent diagram = parent as SfDiagramComponent;
            for (int i = 0; i < items.Count; i++)
            {
                switch (name)
                {
                    case PORTS:
                        PointPort port = items[i] as PointPort;
                        if ((parent as Node)?.Parent != null)
                        {
                            Node node = (parent as Node);
                            if (parent is NodeGroup groupObj)
                            {
                                for (int j = 0; j < groupObj.Wrapper.Children.Count; j++)
                                {
                                    if (groupObj.Wrapper.Children[j].ID == groupObj.ID + GROUPCONTAINER)
                                    {
                                        Canvas container = groupObj.Wrapper.Children[j] as Canvas;
                                        node.InitPort(container, port);
                                    }
                                }
                            }
                            else
                                node.InitPort(node.Wrapper, port);
                            port.SetParent(parent, name);
                            node.Wrapper.Measure(new DiagramSize() { Width = node.Wrapper.Width, Height = node.Wrapper.Height });
                            node.Wrapper.Arrange(node.Wrapper.DesiredSize);
                            ((parent as Node).Parent as SfDiagramComponent).DiagramStateHasChanged();
                        }
                        break;
                    case ANNOTATIONS:
                        Annotation annotation = items[i] as Annotation;
                        if (parent != null && parent is NodeBase node1 && node1.Parent != null)
                        {
                            if (node1 is Node)
                            {
                                if (node1 is NodeGroup)
                                {
                                    NodeGroup group = node1 as NodeGroup;
                                    for (int j = 0; j < group.Wrapper.Children.Count; j++)
                                    {
                                        if (group.Wrapper.Children[j].ID == group.ID + GROUPCONTAINER)
                                        {
                                            Canvas container = group.Wrapper.Children[j] as Canvas;
                                            container.Children.Add((node1 as Node).InitAnnotationWrapper(annotation, false));
                                        }
                                    }
                                }
                                else
                                    node1.Wrapper.Children.Add((node1 as Node).InitAnnotationWrapper(annotation, false));
                            }
                            else
                            {
                                Connector connector = node1 as Connector;
                                connector.Wrapper.Children.Add(connector.GetAnnotationElement(annotation as PathAnnotation, connector.IntermediatePoints, connector.Bounds));
                            }
                            annotation.SetParent(node1, name);
                            node1.Wrapper.Measure(new DiagramSize() { Width = node1.Wrapper.Width, Height = node1.Wrapper.Height });
                            node1.Wrapper.Arrange(node1.Wrapper.DesiredSize);
                        }
                        else if (parent is BpmnShape bpmnShape && bpmnShape.Parent != null)
                        {
                            Node parentNode = bpmnShape.Parent as Node;
                            parentNode.Wrapper.Children.Add(parentNode.InitAnnotationWrapper(annotation, false));
                            Node bpmnNode = bpmnShape.Parent as Node;
                            SfDiagramComponent parentDiagram = bpmnNode.Parent as SfDiagramComponent;
                            DiagramContainer content = bpmnNode.Wrapper;
                            ((content as Canvas).Children[0] as Canvas).Children.Add(parentDiagram.BpmnDiagrams.GetBpmnTextAnnotation(
                            bpmnNode, annotation as BpmnAnnotation, (content as Canvas).Children[0] as Canvas));
                            _ = content.Measure(new DiagramSize() { Width = bpmnNode.Width ?? 0, Height = bpmnNode.Height ?? 0 });
                            content.Arrange(content.DesiredSize, false);
                            if (parentDiagram.DiagramContent.TextAnnotationConnectors.Count > 0)
                            {
                                int k;
                                for (k = 0; k < parentDiagram.DiagramContent.TextAnnotationConnectors.Count; k++)
                                {
                                    if (!parentDiagram.NameTable.ContainsKey(parentDiagram.DiagramContent.TextAnnotationConnectors[k].ID))
                                        parentDiagram.DiagramContent.InitObject(parentDiagram.DiagramContent.TextAnnotationConnectors[k]);
                                }
                            }
                        }
                        break;
                    case FIXEDUSERHANDLE:
                        FixedUserHandle userHandle = items[i] as FixedUserHandle;
                        NodeBase nodeParent = (parent as NodeBase);
                        if (parent is Node nodeObj && nodeParent.Parent != null)
                        {
                            DiagramElement handle = nodeObj.InitFixedUserHandles(userHandle as NodeFixedUserHandle);
                            nodeParent.Wrapper.Children.Add(handle);
                            userHandle.SetParent(nodeObj, name);
                            nodeParent.Wrapper.Measure(new DiagramSize() { Width = nodeParent.Wrapper.Width, Height = nodeParent.Wrapper.Height });
                            nodeParent.Wrapper.Arrange(nodeParent.Wrapper.DesiredSize);
                        }
                        else if (parent != null && parent is Connector connector && nodeParent.Parent != null)
                        {
                            connector.Init();
                            userHandle.SetParent(connector, name);
                            connector.Wrapper.Measure(new DiagramSize() { Width = connector.Wrapper.Width, Height = connector.Wrapper.Height });
                            connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                        }
                        break;
                    case NODES:
                    case CONNECTORS:
                        if (parent != null)
                        {
                            NodeBase nodeBase = items[i] as NodeBase;
                            nodeBase.SetParent(parent, name);
                            if (diagram.EventHandler.Action == Actions.Draw && diagram.CommandHandler.PolygonObject != null)
                            {
                                Node node = nodeBase as Node;
                                DiagramRect absoluteBounds = DomUtil.MeasurePath((node.Shape as BasicShape).PolygonPath);
                                node.Width = absoluteBounds.Width;
                                node.Height = absoluteBounds.Height;
                                node.OffsetX = absoluteBounds.Center.X;
                                node.OffsetY = absoluteBounds.Center.Y;
                            }
                            diagram.DiagramContent.InitObject(nodeBase);
                            if (nodeBase is Connector)
                            {
                                Connector connector = nodeBase as Connector;
                                if (connector.Annotations.Count > 0)
                                {
                                    for (int j = 0; j < connector.Annotations.Count; j++)
                                    {
                                        PathAnnotation annotationValue = connector.Annotations[j];
                                        if (annotationValue != null)
                                        {
                                            DiagramLayerContent.AddMeasureTextCollection(connector, annotationValue, null, measureTextDataCollection);
                                            TextElement textElement = connector.GetAnnotationElement(annotationValue, connector.IntermediatePoints, connector.Bounds) as TextElement;
                                            connector.Wrapper.Children.Add(textElement);
                                        }
                                    }
                                    if (!diagram.DiagramAction.HasFlag(DiagramAction.UndoRedo))
                                    {
                                        await DomUtil.MeasureBounds(null, measureTextDataCollection, null, null);
                                    }
                                    connector.Wrapper.Measure(new DiagramSize() { Width = connector.Wrapper.Width, Height = connector.Wrapper.Height });
                                    connector.Wrapper.Arrange(connector.Wrapper.DesiredSize);
                                    for (int j = 0; j < connector.Wrapper.Children.Count; j++)
                                    {
                                        if (connector.Wrapper.Children[j] is TextElement)
                                            (connector.Wrapper.Children[j] as TextElement).RefreshTextElement();
                                    }
                                    diagram.DiagramStateHasChanged();
                                }
                            }
                            if (diagram.PaletteInstance != null && diagram.PaletteInstance.SelectedSymbol != null)
                            {
                                diagram.CommandHandler.Select(nodeBase, false);
                                diagram.PaletteInstance.SelectedSymbol = null;
                            }
                            if (nodeBase is Connector)
                            {
                                diagram.DiagramContent.UpdateBridging();
                            }
                            DiagramObjectCollection<Connector> textAnnotation = diagram.DiagramContent.TextAnnotationConnectors;
                            if (textAnnotation != null && textAnnotation.Count > 0)
                            {
                                for (int j = 0; j < textAnnotation.Count; j++)
                                {
                                    if (!diagram.NameTable.ContainsKey(textAnnotation[j].ID))
                                    {
                                        diagram.DiagramContent.InitObject(textAnnotation[j]);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            SfDiagramComponent.IsProtectedOnChange = true;
            if (this.Parent != null)
            {
                if (diagram != null && diagram.RealAction.HasFlag(RealAction.MeasureDataJSCall) &&
                    !diagram.IsBeginUpdate)
                {
                    diagram.DiagramStateHasChanged();
                    diagram.RealAction &= ~RealAction.MeasureDataJSCall;
                }
                else if (this.Parent is NodeBase && (this.Parent as NodeBase).Parent is SfDiagramComponent && ((this.Parent as NodeBase).Parent as SfDiagramComponent).RealAction.HasFlag(RealAction.MeasureDataJSCall) && !((this.Parent as NodeBase).Parent as SfDiagramComponent).IsBeginUpdate)
                {
                    ((this.Parent as NodeBase).Parent as SfDiagramComponent).DiagramStateHasChanged();
                    ((this.Parent as NodeBase).Parent as SfDiagramComponent).RealAction &= ~RealAction.MeasureDataJSCall;
                }
                else if (((this.Parent as BpmnShape)?.Parent as NodeBase)?.Parent is SfDiagramComponent && !(((this.Parent as BpmnShape).Parent as NodeBase).Parent as SfDiagramComponent).IsBeginUpdate)
                {
                    (((this.Parent as BpmnShape).Parent as NodeBase).Parent as SfDiagramComponent).DiagramStateHasChanged();
                }

            }
        }

        private async Task GetDefaultValues(NodeBase node)
        {
            bool canGetDefaultValue = true;
            if((((SfDiagramComponent)Parent).DiagramAction.HasFlag(DiagramAction.UndoRedo)))
                canGetDefaultValue = false;
            if (node is Node node1 && canGetDefaultValue)
            {
                await ((SfDiagramComponent)Parent).NodeCreating.InvokeAsync(node);
                ICommonElement content = ((SfDiagramComponent)Parent).SetNodeTemplate?.Invoke(node);
                node1.Template = content;
            }
            if (node is Connector && canGetDefaultValue)
                await ((SfDiagramComponent)Parent).ConnectorCreating.InvokeAsync(node);
            canGetDefaultValue = true;
        }
        private async Task MeasureData(Dictionary<string, string> measurePathDataCollection, Dictionary<string, TextElementUtils> measureTextDataCollection, Dictionary<string, string> measureImageDataCollection)
        {
            if (this.Parent is SfDiagramComponent)
            {
                ((SfDiagramComponent)Parent).RealAction |= RealAction.MeasureDataJSCall;
            }
            else if (this.Parent is NodeBase)
            {
                ((SfDiagramComponent)((NodeBase)Parent).Parent).RealAction |= RealAction.MeasureDataJSCall;
            }

            await DomUtil.MeasureBounds(measurePathDataCollection, measureTextDataCollection, measureImageDataCollection, null);
        }
        internal void ClearItemElements(IList items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                DiagramObject obj = items[i] as DiagramObject;
                string id = obj switch
                {
                    NodeBase nodeBase => nodeBase.ID,
                    Annotation annotation => annotation.ID,
                    ConnectorSegment segment => segment.ID,
                    FixedUserHandle handle => handle.ID,
                    UserHandle handle => handle.ID,
                    Port port => port.ID,
                    _ => null
                };
                if (!string.IsNullOrEmpty(id) && parent is NodeBase)
                {
                    if (parent is NodeBase parentObj && parentObj.Wrapper != null)
                    {
                        ICommonElement element = DiagramUtil.GetWrapper(parentObj, parentObj.Wrapper, id);
                        if ((parentObj is NodeGroup groupObj) && groupObj.Children != null && groupObj.Children.Length > 0)
                        {
                            (groupObj.Wrapper.Children[0] as DiagramContainer).Children.Remove(element);
                        }
                        else
                            parentObj.Wrapper.Children.Remove(element);
                    }
                }
                else if (!string.IsNullOrEmpty(id) && parent is BpmnShape)
                {
                    if ((parent as BpmnShape).Parent is NodeBase parentObj && parentObj.Wrapper != null)
                    {
                        _ = DiagramUtil.RemoveBpmnWrapper(parentObj, parentObj.Wrapper.Children[0] as Canvas, id);
                    }
                }
            }
        }

        internal DiagramObjectCollection<T> Concat(DiagramObjectCollection<T> collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                this.Add(collection[i]);
            }
            return this;
        }
    }
}

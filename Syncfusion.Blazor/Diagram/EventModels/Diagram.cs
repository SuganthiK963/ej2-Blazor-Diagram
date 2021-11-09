using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Provides data for the PropertyChanged event.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" Nodes="@nodes" Connectors="@connectors" PropertyChanged="@PropertyChanged">
    /// </SfDiagramComponent>
    /// @code
    /// { 
    ///   private void PropertyChanged(PropertyChangedEventArgs args)
    ///   {
    ///     if ((args != null) && (args.Element != null) && (args.NewValue != null) && (args.OldValue != null))
    ///     {
    ///       Console.WriteLine("Changed");
    ///     }
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class PropertyChangedEventArgs
    {
        /// <summary>
        /// Gets the object where the property change has occurred.
        /// </summary>
        public IDiagramObject Element { get; internal set; }
        /// <summary>
        /// Gets the new value of the property that was changed
        /// </summary>
        public object NewValue { get; internal set; }
        /// <summary>
        /// Gets the old value of the property that was changed.
        /// </summary>
        public object OldValue { get; internal set; }
        /// <summary>
        /// Gets the name of the property that has a property change.
        /// </summary>
        public string PropertyName { get; internal set; }
    }
    /// <summary>
    /// Notifies when an element drags over another diagram element.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" Dragging="Dragging">
    /// </SfDiagramComponent>
    /// private void Dragging(DraggingEventArgs args)
    /// {
    ///     if (args.Element is DiagramSelectionSettings)
    ///     {
    ///         DiagramSelectionSettings selector = args.Element as DiagramSelectionSettings;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class DraggingEventArgs
    {
        /// <summary>
        /// Gets the node or connector that is dragged outside the diagram.
        /// </summary>
        public IDiagramObject Element { get; internal set; }
        /// <summary>
        /// Gets the mouse position of the node/connector.
        /// </summary>
        public DiagramPoint Position { get; internal set; }
    }
    /// <summary>
    /// Notifies when the element enters into the diagram from the symbol palette.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" DragStart="DragStart">
    /// </SfDiagramComponent>
    /// private void DragStart(DragStartEventArgs args)
    /// {
    ///     if (args.Element is Node)
    ///     {
    ///         (args.Element as Node).Width = 300;
    ///         (args.Element as Node).Height = 300;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class DragStartEventArgs
    {
        /// <summary>
        /// Gets the node/connector over which the symbol is dragged.
        /// </summary>
        public IDiagramObject Element { get; internal set; }
        /// <summary>
        /// Gets or sets the value that indicates whether to add or remove the symbol from the diagram.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Notifies when the element leaves the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" DragLeave="DragLeave">
    /// </SfDiagramComponent>
    /// private void DragLeave(DragLeaveEventArgs args)
    /// {
    ///     if (args.Element is Node)
    ///     {
    ///         (args.Element as Node).Width = 300;
    ///         (args.Element as Node).Height = 300;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class DragLeaveEventArgs
    {
        /// <summary>
        /// Gets the node or connector that is dragged outside the diagram.
        /// </summary>
        public IDiagramObject Element { get; internal set; }
    }
    /// <summary>
    /// Notifies when the element is dropped from the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" DragDrop="DragDrop">
    /// </SfDiagramComponent>
    /// private void DragDrop(DropEventArgs args)
    /// {
    ///     if (args.Element is Node)
    ///     {
    ///         string id = (args.Element as NodeBase).ID;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class DropEventArgs
    {
        /// <summary>
        /// Gets the node or connector that is being dropped.
        /// </summary>
        public IDiagramObject Element { get; internal set; }
        /// <summary>
        /// Gets the object from which the object will be dropped.
        /// </summary>
        public IDiagramObject Target { get; internal set; }
        /// <summary>
        /// Gets or sets the value that indicates whether to cancel the drop event or not.
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// Gets the position of the object.
        /// </summary>
        public DiagramPoint Position { get; internal set; }
    }


    /// <summary>
    /// Notifies before the select or deselect any objects from the diagram.
    /// </summary>
    public class SelectionChangingEventArgs: SelectionChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the value indicates whether the element can be selected.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Notifies when select or deselect any objects from the diagram.
    /// </summary>
    public class SelectionChangedEventArgs
    {
        /// <summary>
        /// Gets the actual cause of the event
        /// </summary>
        public DiagramAction ActionTrigger { get; internal set; }

        /// <summary>
        /// Gets the selected element after the event has triggered
        /// </summary>
        public ObservableCollection<IDiagramObject> NewValue { get; internal set; }

        /// <summary>
        /// Gets the last selected elements, it's empty if previously, not objects have selected.
        /// </summary>
        public ObservableCollection<IDiagramObject> OldValue { get; internal set; }

        /// <summary>
        /// Gets the item is added or removed from the list.
        /// </summary>
        public CollectionChangedAction Type { get; internal set; }
    }
    
    /// <summary>
    /// Notifies when the label of an element undergoes editing. 
    /// </summary>
    public class TextChangeEventArgs
    {
        /// <summary>
        /// Gets the old text value of the element. 
        /// </summary>
        public string OldValue { get; internal set; }
        /// <summary>
        /// Gets the new text value of the element that is being changed 
        /// </summary>
        public string NewValue { get; internal set; }
        /// <summary>
        /// Gets a node or connector in which annotation is being edited 
        /// </summary>
        public IDiagramObject Element { get; internal set; }
        /// <summary>
        /// Represents the annotation which is being edited. 
        /// </summary>
        public Annotation Annotation { get; internal set; }
        /// <summary>
        /// Gets or sets the value that indicates whether to cancel the event or not. 
        /// </summary>
        public bool Cancel { get; set; }
    }
    /// <summary>
    /// Provides information about current mouse events like mouse down, mouse move, etc. 
    /// </summary>
    /// <remarks>
    /// It will return the event properties when a mouse down, mouse move, mouse leave, or mouse up event occurs based on the tool which is currently active, like resize, clone tool, etc.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// public class CloneTool : DragController
    /// {
    ///     public CloneTool(SfDiagramComponent diagram) : base(diagram)
    ///     {
    ///     }
    ///     public override void OnMouseDown(DiagramMouseEventArgs args)
    ///     {
    ///         NodeBase newObject;
    ///         if (Diagram.SelectionSettings.Nodes.Count > 0)
    ///         {
    ///             newObject = (Diagram.SelectionSettings.Nodes[0]).Clone() as Node;
    ///         }
    ///         else
    ///         {
    ///             newObject = (Diagram.SelectionSettings.Connectors[0]).Clone() as Connector;
    ///         }
    ///         newObject.ID += Diagram.Nodes.Count.ToString();
    ///         Diagram.Copy();
    ///         Diagram.Paste();
    ///         ObservableCollection<IDiagramObject> obj = new ObservableCollection<IDiagramObject>() { Diagram.Nodes[Diagram.Nodes.Count - 1] as IDiagramObject };
    ///         Diagram.Select(obj);
    ///         args.Element = Diagram.SelectionSettings.Nodes[0] as IDiagramObject;
    ///         base.OnMouseDown(args);
    ///         this.InAction = true;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class DiagramMouseEventArgs
    {
        /// <summary>
        /// Gets the current position of the mouse pointer.
        /// </summary>
        public DiagramPoint Position { get; set; }

        /// <summary>
        /// Gets the actual node or connector which is selected.
        /// </summary>
        public IDiagramObject ActualObject { get; set; }


        /// <summary>
        /// Gets or sets the selector object which is currently selected.
        /// </summary>
        public IDiagramObject Element { get; set; }

        /// <summary>
        /// Gets or sets the wrapper of the source object.
        /// </summary>
        internal ICommonElement SourceWrapper { get; set; }

        /// <summary>
        /// Gets or sets the object under the source objects.
        /// </summary>
        public IDiagramObject Target { get; set; }

        /// <summary>
        /// Gets or sets the wrapper of the target object.
        /// </summary>
        internal ICommonElement TargetWrapper { get; set; }

        internal Info Info { get; set; }

        internal List<ITouches> StartTouches { get; set; }
        internal List<ITouches> MoveTouches { get; set; }

        /// <summary>
        /// Gets the number of times it clicked.
        /// </summary>
        public int ClickCount { get; set; }

        internal void Dispose()
        {
            if (Position != null)
            {
                Position.Dispose();
                Position = null;
            }
            if (ActualObject != null)
            {
                ActualObject = null;
            }
            if (Element != null)
            {
                Element = null;
            }
            if (SourceWrapper != null)
            {
                SourceWrapper.Dispose();
                SourceWrapper = null;
            }
            if (Target != null)
            {
                Target = null;
            }
            if (TargetWrapper != null)
            {
                TargetWrapper.Dispose();
                TargetWrapper = null;
            }
            if (Info != null)
            {
                Info.Dispose();
                Info = null;
            }
        }
    }

    
    /// <summary>
    /// Notifies before the node or connector is dragging or its position is changing.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" PositionChanging="@changed">
    /// </SfDiagramComponent>
    /// private void changed(PositionChangingEventArgs args)
    /// {
    ///     if (args.NewValue != null && args.OldValue != null && args.Element != null)
    ///     {
    ///         Console.WriteLine("Changing");
    ///     }
    /// }    
    /// ]]>
    /// </code>
    /// </example>
    public class PositionChangingEventArgs: PositionChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the value that indicates the user prevents dragging of element over the diagram.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Notifies when the node or connector is dragged or its position is changed.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" PositionChanged="@changed">
    /// </SfDiagramComponent>
    /// private void changed(PositionChangedEventArgs args)
    /// {
    ///     if (args.NewValue != null && args.OldValue != null && args.Element != null)
    ///     {
    ///         Console.WriteLine("Changed");
    ///     }
    /// }    
    /// ]]>
    /// </code>
    /// </example>
    public class PositionChangedEventArgs
    {
        /// <summary>
        /// Gets the selector’s current value in which the node or the connector is being dragged.
        /// </summary>
        public DiagramSelectionSettings NewValue { get; internal set; }

        /// <summary>
        /// Gets the Selector old value in which the node or the connector is being dragged.
        /// </summary>
        public DiagramSelectionSettings OldValue { get; internal set; }

        /// <summary>
        /// Gets the node or connector that is being dragged.
        /// </summary>
        public IDiagramObject Element { get; internal set; }

    }

    /// <summary>
    /// Specifies the source and the target details of the connector. 
    /// </summary>
    public class ConnectionObject
    {

        /// <summary>
        /// Gets the source node id of the connector.
        /// </summary>
        public string SourceID { get; internal set; }

        /// <summary>
        /// Gets the source port id of the connector.
        /// </summary>
        public string SourcePortID { get; internal set; }

        /// <summary>
        /// Gets the target node id of the connector.
        /// </summary>
        public string TargetID { get; internal set; }

        /// <summary>
        /// Gets the target port id of the connector.
        /// </summary>
        public string TargetPortID { get; internal set; }
    }

    /// <summary>
    /// Notifies before the connector’s source id or target id has changing.
    /// </summary>
    public class ConnectionChangingEventArgs: ConnectionChangedEventArgs
    {

        /// <summary>
        /// Defines whether the user can prevent the connection or disconnection of the connector while its endpoint is dragging.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Notifies when the connector’s source id or target id has changed.
    /// </summary>
    public class ConnectionChangedEventArgs
    {
        /// <summary>
        /// Returns the new source node or target node of the connector.
        /// </summary>
        [DefaultValue(null)]
        public Connector Connector { get; internal set; } = null;

        /// <summary>
        /// Returns whether it is a source end or a target end.
        /// </summary>
        public Actions ConnectorAction { get; internal set; }

        /// <summary>
        /// Represents the current source or target node while dragging the endpoint of the connector.
        /// </summary>
        public ConnectionObject NewValue { get; internal set; }

        /// <summary>
        /// Specifies the previous source or target node of the connector.
        /// </summary>
        public ConnectionObject OldValue { get; internal set; }
    }
    /// <summary>
    /// RotationChangingEventArgs notifies before the node/connector are rotating
    /// </summary>
    public class RotationChangingEventArgs : RotationChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the value indicates whether to cancel the change or not
        /// </summary>
        public bool Cancel { get; set; }
    }
    
    /// <summary>
    /// RotationChangedEventArgs notifies when the node/connector are rotated
    /// </summary>
    public class RotationChangedEventArgs
    {
        /// <summary>
        /// Gets the node that is selected for rotation.
        /// </summary>
        public DiagramSelectionSettings Element { get; internal set; }

        /// <summary>
        /// Gets the previous rotation angle.
        /// </summary>
        public DiagramSelectionSettings OldValue { get; internal set; }
        
        /// <summary>
        /// Gets the new rotation angle.
        /// </summary>
        public DiagramSelectionSettings NewValue { get; internal set; }
    }

    /// <summary>
    /// Notifies when the element is resizing.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Height="600px" Nodes="@nodes" SizeChanging="@OnSizeChange" />
    /// @code
    /// {
    ///     // To define the node collection
    ///     DiagramObjectCollection<Node> nodes;
    ///     protected override void OnInitialized()
    ///     {
    ///         nodes = new DiagramObjectCollection<Node>();
    ///         // A node is created and stored in the nodes collection.
    ///         Node node = new Node()
    ///         {
    ///             // Position of the node
    ///             OffsetX = 250,
    ///             OffsetY = 250,
    ///             // Size of the node
    ///             Width = 100,
    ///             Height = 100,
    ///             Style = new ShapeStyle() { Fill = "#6BA5D7", StrokeColor = "white" }
    ///         };
    ///         // Add a node
    ///         nodes.Add(node);
    ///     }
    ///     // Size change event for the diagram
    ///     public void OnSizeChange(SizeChangingEventArgs args)
    ///     {
    ///         Console.WriteLine(args.NewValue.Nodes[0].ID);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class SizeChangingEventArgs: SizeChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the value indicates whether to cancel the change or not
        /// </summary>
        public bool Cancel { get; set; }
    }
    /// <summary>
    /// Notifies when the element is resized.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Height="600px" Nodes="@nodes" SizeChanged="@SizeChanged" />
    /// @code
    /// {
    ///     // To define the node collection
    ///     DiagramObjectCollection<Node> nodes;
    ///     protected override void OnInitialized()
    ///     {
    ///         nodes = new DiagramObjectCollection<Node>();
    ///         // A node is created and stored in the nodes collection.
    ///         Node node = new Node()
    ///         {
    ///             // Position of the node
    ///             OffsetX = 250,
    ///             OffsetY = 250,
    ///             // Size of the node
    ///             Width = 100,
    ///             Height = 100,
    ///             Style = new ShapeStyle() { Fill = "#6BA5D7", StrokeColor = "white" }
    ///         };
    ///         // Add a node
    ///         nodes.Add(node);
    ///     }
    ///     // Size change event for the diagram
    ///     public void SizeChanged(SizeChangedEventArgs args)
    ///     {
    ///         Console.WriteLine(args.NewValue.Nodes[0].ID);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class SizeChangedEventArgs
    {
        /// <summary>
        /// Returns the element which has been selected for the resizing.
        /// </summary>
        public DiagramSelectionSettings Element { get; internal set; }
        /// <summary>
        /// Returns the previous width, height, offsetX and offsetY values of the element that is resized.
        /// </summary>
        public DiagramSelectionSettings OldValue { get; internal set; }
        /// <summary>
        /// Returns the new width, height, offsetX and offsetY values of the element that is resized
        /// </summary>
        public DiagramSelectionSettings NewValue { get; internal set; }
    }
    /// <summary>
    /// Represents before the source and target points of the connector are changing.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" TargetPointChanging="@OnTargetPointChanging">
    /// </SfDiagramComponent>
    /// private void OnTargetPointChanging(EndPointChangingEventArgs args)
    /// {
    ///     if (args.Connector != null)
    ///     {
    ///         Connector connector = args.Connector;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class EndPointChangingEventArgs: EndPointChangedEventArgs
    {
        /// <summary>
        /// Gets or sets the value that indicates whether to cancel the change or not.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Represents the source and target points of the connector are changed.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" TargetPointChanged="@OnTargetPointChanged">
    /// </SfDiagramComponent>
    /// private void OnTargetPointChanged(EndPointChangedEventArgs args)
    /// {
    ///     if (args.Connector != null)
    ///     {
    ///         Connector connector = args.Connector;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class EndPointChangedEventArgs
    {
        /// <summary>
        /// Gets the current connector from which the source point or target point is changed.
        /// </summary>
        public Connector Connector { get; internal set; }
        /// <summary>
        /// Gets the previous point of the mouse pointer.
        /// </summary>
        public DiagramPoint OldValue { get; internal set; }
        /// <summary>
        /// Gets the current point of the mouse pointer.
        /// </summary>
        public DiagramPoint NewValue { get; internal set; }
        /// <summary>
        /// Gets the target node of the connector while dragging the end point.
        /// </summary>
        public string TargetNodeID { get; internal set; }
        /// <summary>
        /// Gets the target port of the node that is to be connected with  the connector while dragging the end point.
        /// </summary>
        public string TargetPortID { get; internal set; }
    }

    /// <summary>
    /// Notifies when the fixed user handle gets clicked.
    /// </summary>
    public class FixedUserHandleClickEventArgs
    {
        /// <summary>
        /// Represents the instance of the clicked fixed user handle. 
        /// </summary>
        public FixedUserHandle FixedUserHandle { get; internal set; }

        /// <summary>
        /// Gets nodes/connector which have the clicked fixed user handle. 
        /// </summary>
        public IDiagramObject Element { get; internal set; }
    }
    /// <summary>
    /// Notifies when to execute the custom keyboard commands .
    /// </summary>
    /// <remarks>
    /// The following code illustrates how to create a custom command.
    /// </remarks>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent @ref="@diagram" Height="600px" Nodes="@nodes">
    /// @* Initializing the custom commands*@ 
    ///     <CommandManager Commands = "@command" Execute="@CommandExecute" CanExecute="@canexe">
    ///     </CommandManager>
    /// </SfDiagramComponent>
    /// @code
    /// { 
    ///     // Reference to the diagram
    ///     SfDiagramComponent diagram;
    ///     DiagramObjectCollection<KeyboardCommand> command = new DiagramObjectCollection<KeyboardCommand>()
    ///     {
    ///         new KeyboardCommand()
    ///         {
    ///             Name = "CustomGroup",
    ///             Gesture = new KeyGesture() { Key = Keys.G, Modifiers = ModifierKeys.Control }
    ///         },
    ///         new KeyboardCommand()
    ///         {
    ///             Name = "CustomUnGroup",
    ///             Gesture = new KeyGesture() { Key = Keys.U, Modifiers = ModifierKeys.Control }
    ///         },
    ///     };
    ///     // Define the diagram's nodes collection
    ///     DiagramObjectCollection<Node> nodes = new DiagramObjectCollection<Node>();
    ///     public void canexe(CommandKeyArgs args)
    ///     {
    ///         args.CanExecute = true;
    ///     }
    ///     public void CommandExecute(CommandKeyArgs args)
    ///     {
    ///         if (args.Gesture.Modifiers == ModifierKeys.Control && args.Gesture.Key == Keys.G)
    ///         {
    ///             //Custom command to group the selected nodes
    ///             diagram.Group();
    ///         }
    ///         if (args.Gesture.Modifiers == ModifierKeys.Control && args.Gesture.Key == Keys.U)
    ///         {
    ///             DiagramSelectionSettings selector = diagram.SelectionSettings;
    ///             //Custom command to ungroup the selected items
    ///             if (selector.Nodes.Count > 0 && selector.Nodes[0] is NodeGroup)
    ///             {
    ///                 if ((selector.Nodes[0] as NodeGroup).Children.Length > 0)
    ///                 {
    ///                     diagram.UnGroup();
    ///                 }
    ///             }
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class CommandKeyArgs
    {
        /// <summary>
        /// Specifies the name of the command.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Defines the method that determines whether the command can be executed in its current state.
        /// </summary>
        public bool CanExecute { get; set; }

        /// <summary>
        /// Specifies a combination of keys and key modifiers, on recognition of which the command should be executed.
        /// </summary>
        public KeyGesture Gesture { get; internal set; }
    }
    /// <summary>
    /// Notifies while performing the key actions.
    /// </summary>
    public class KeyEventArgs
    {
        /// <summary>
        /// Returns the selected element of the diagram.
        /// </summary>
        public IDiagramObject Element { get; internal set; }

        /// <summary>
        /// Returns the value of the key action.
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// Returns a number that represents the actual key pressed.
        /// </summary>
        public int KeyCode { get; internal set; }
        /// <summary>
        /// Returns any modifier keys that were pressed when the flick gesture occurred.
        /// </summary>
        public ModifierKeys KeyModifiers { get; internal set; }
    }
    /// <summary>
    /// Represents the class which is used to notifies while the changes occurs during undo/redo process.
    /// </summary>
    public class HistoryChangedEventArgs
    {
        /// <summary>
        /// Gets the collection of objects that are changed in the last undo/redo.
        /// </summary>
        public List<NodeBase> Source { get; internal set; }
        /// <summary>
        /// Gets the previous and new value of the history object that has been changed.
        /// </summary>
        public HistoryEntryBase Entry {get; internal set; }
        /// <summary>
        /// Gets the type of the newly added entry.
        /// </summary>
        public HistoryEntryType Type { get; internal set; }
        /// <summary>
        ///  Gets the entry's change type.
        /// </summary>
        public HistoryEntryChangeType CollectionChangedAction { get; internal set; }
        /// <summary>
        /// Gets the event action.
        /// </summary>       
        public HistoryChangedAction ActionTrigger { get; internal set; }
    }

    /// <summary>
    /// Notifies before the node/connector is added or removed from the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" CollectionChanging="@collection">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     private void collection(CollectionChangingEventArgs args)
    ///     {
    ///         if (args.Element != null)
    ///         {
    ///             Console.WriteLine("CollectionChanging");
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class CollectionChangingEventArgs : CollectionChangedEventArgs
    {

        /// <summary>
        /// Gets or sets the value that indicates whether to cancel the change or not.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Notifies while the node/connector is added or removed from the diagram.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" CollectionChanged="@collection">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     private void collection(CollectionChangedEventArgs args)
    ///     {
    ///         if (args.Element != null)
    ///         {
    ///             Console.WriteLine("CollectionChanged");
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class CollectionChangedEventArgs
    {
        /// <summary>
        /// Gets the current action, like Interactions, Drawing Tools, etc., to be performed in the diagram.
        /// </summary>
        public DiagramAction ActionTrigger { get; internal set; }

        /// <summary>
        /// Gets the actual object which is added, removed, or modified.
        /// </summary>
        public NodeBase Element { get; internal set; }

        /// <summary>
        /// Gets the type of collection change like addition or removal.
        /// </summary>
        public CollectionChangedAction Action { get; internal set; }
    }

    /// <summary>
    /// Notifies when clicking on an object or diagram.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="1000px" Click="click">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///     private void click(ClickEventArgs args)
    ///     {
    ///         if (args.ActualObject != null)
    ///         {
    ///             Console.WriteLine("Clicked");
    ///         }
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>

    public class ClickEventArgs
    {

        /// <summary>
        /// Gets the object to be clicked on. It returns null when there is no object present in the clicked position.
        /// </summary>
        public IDiagramObject ActualObject { get; internal set; }

        /// <summary>
        /// Gets the number of times the object or diagram has to be clicked.
        /// </summary>
        public int Count { get; internal set; }

        /// <summary>
        /// Gets the object if the clicked position has an object or returns the diagram.
        /// </summary>
        public IDiagramObject Element { get; internal set; }

        /// <summary>
        /// Gets the mouse button that has to be clicked.
        /// </summary>
        public MouseButtons Button { get; internal set; }

        /// <summary>
        /// Gets the clicked position in the diagram.
        /// </summary>
        public DiagramPoint Position { get; internal set; }
    }

    /// <summary>
    /// Notifies when the mouse events such as mouse enter, mouse leave, and mouseover are detected.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Width="1000px" Height="1000px" Nodes="@nodes" Connectors="@connectors" MouseEnter="@MouseEnter">
    /// </SfDiagramComponent>
    /// @code
    /// {
    ///   private void MouseEnter(DiagramElementMouseEventArgs args)
    ///   {
    ///     if ((args != null) && (args.ActualObject != null))
    ///     {
    ///         Console.WriteLine("Mouse Entered");
    ///     }
    ///   }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class DiagramElementMouseEventArgs
    {

        /// <summary>
        /// Gets the object such as a node or connector when it is dragged from the symbol palette into the diagram.
        /// </summary>
        public IDiagramObject ActualObject { get; internal set; }

        /// <summary>
        /// Gets the helper element from the symbol palette into the diagram while dragging
        /// </summary>
        public IDiagramObject Element { get; internal set; }

        /// <summary>
        /// Gets the collection of objects over which the selected items are dragged
        /// </summary>
        public ObservableCollection<IDiagramObject> Targets { get; internal set; }
    }

    /// <summary>
    /// ScrollChangedEventArgs notifies when the scroller has changed
    /// </summary>
    public class ScrollChangedEventArgs
    {
        /// <summary>
        /// gets the current close-up view of the diagram. By default, ZoomFactor is set to 1.
        /// </summary>
        public double ZoomFactor { get; internal set; }

        /// <summary>
        /// Gets the value of the horizontal scroll offset. By default, the ScrollX is set to 0.
        /// </summary>
        public double ScrollX { get; internal set; }

        /// <summary>
        /// Gets the value of the vertical scroll offset. By default, the ScrollY is set to 0.
        /// </summary>
        public double ScrollY { get; internal set; }
    }

    /// <summary>
    /// SegmentCollectionChangeEventArgs notifies while the segment of the connectors changes
    /// </summary>
    public class SegmentCollectionChangeEventArgs
    {
        /// <summary>
        /// Gets the action of diagram.
        /// </summary>
        public DiagramObjectCollection<ConnectorSegment> AddedSegments { get; internal set; }

        /// <summary>
        /// Gets or sets the value indicates whether to cancel the change or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the selected element.
        /// </summary>
        public Connector Element { get; internal set; }

        /// <summary>
        /// Gets the removed segment collection.
        /// </summary>
        public DiagramObjectCollection<ConnectorSegment> RemovedSegments { get; internal set; }

        /// <summary>
        /// Gets the type of the collection change.
        /// </summary>
        public CollectionChangedAction Type { get; internal set; }
    }

    public class HistoryAddingEventArgs
    {
        /// <summary>
        /// Gets the history object that has been added to the history list.
        /// </summary>
        public HistoryEntryBase Entry { get; internal set; }

        /// <summary>
        /// Gets or sets the value that indicates whether to cancel the added or not.
        /// </summary>
        public bool Cancel {get; set;}
    }
}

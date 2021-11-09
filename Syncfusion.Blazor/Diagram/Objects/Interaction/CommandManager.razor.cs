using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Defines the collection of commands and the corresponding key gestures. It is responsible for managing routed commands.
    /// </summary>
    /// <remarks>
    /// CommandManager provides the support to define custom commands. The custom commands are executed when the specified key gesture is recognized.
    /// </remarks>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
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
    ///         new Command()
    ///         {
    ///             Name = "CustomGroup",
    ///             Gesture = new KeyGesture() { Key = Keys.G, KeyModifiers = KeyModifiers.Control }
    ///         },
    ///         new Command()
    ///         {
    ///             Name = "CustomUnGroup",
    ///             Gesture = new KeyGesture() { Key = Keys.U, KeyModifiers = KeyModifiers.Control }
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
    ///         if (args.Gesture.KeyModifiers == KeyModifiers.Control && args.Gesture.Key == Keys.G)
    ///         {
    ///             //Custom command to group the selected nodes
    ///             diagram.Group();
    ///         }
    ///         if (args.Gesture.KeyModifiers == KeyModifiers.Control && args.Gesture.Key == Keys.U)
    ///         {
    ///             Selector selector = diagram.SelectedItems;
    ///             //Custom command to ungroup the selected items
    ///             if (selector.Nodes.Count > 0 && selector.Nodes[0] is Group)
    ///             {
    ///                 if ((selector.Nodes[0] as Group).Children.Length > 0)
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
    public partial class CommandManager : SfBaseComponent
    {
        private DiagramObjectCollection<KeyboardCommand> commandCollection = new DiagramObjectCollection<KeyboardCommand>() { };
        /// <summary>
        /// Gets or sets the child content of the Command Manager.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }
        [CascadingParameter]
        [JsonIgnore]
        internal SfDiagramComponent Parent { get; set; }

        /// <summary>
        /// Represents storing multiple command names with the corresponding command object.
        /// </summary>
        [Parameter]
        [JsonPropertyName("commands")]
        public DiagramObjectCollection<KeyboardCommand> Commands
        {
            get => commandCollection;
            set
            {
                if (value != null && commandCollection != value)
                {
                    commandCollection = value;
                    commandCollection.Parent = this.Parent;
                }
                else
                    commandCollection = value;
            }
        }
        /// <summary>
        /// Determines whether this command can execute in its current state.
        /// </summary>
        [Parameter]
        [JsonPropertyName("canExecute")]
        public EventCallback<CommandKeyArgs> CanExecute { get; set; }
        /// <summary>
        /// Executes the command on the current command target.
        /// </summary>
        [Parameter]
        [JsonPropertyName("execute")]
        public EventCallback<CommandKeyArgs> Execute { get; set; }

        internal static CommandManager Initialize()
        {
            CommandManager commandManager = new CommandManager();
            return commandManager;
        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            Parent.UpdateCommandManager(this);
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (commandCollection != null)
            {
                for (int i = 0; i < commandCollection.Count; i++)
                {
                    commandCollection[i].Dispose();
                }
                commandCollection.Clear();
                commandCollection = null;
            }
            if (ChildContent != null)
            {
                ChildContent = null;
            }
            if (Parent != null)
            {
                Parent = null;
            }
        }
    }
}
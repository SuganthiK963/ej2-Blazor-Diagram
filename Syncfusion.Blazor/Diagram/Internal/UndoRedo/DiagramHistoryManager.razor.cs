using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Notifies when changes are reverted or restored.
    /// </summary>
    public partial class DiagramHistoryManager : SfBaseComponent
    {
        [CascadingParameter]
        [JsonIgnore]
        internal SfDiagramComponent Diagram { get; set; }

        internal static DiagramHistoryManager Initialize()
        {
            DiagramHistoryManager history = new DiagramHistoryManager();
            return history;
        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            Diagram.UpdateHistory(this);
        }
        /// <summary>
        /// Sets the child content for the scroll settings
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Decides whether the history entry can be undo.
        /// </summary>
        public bool CanUndo { get; set; }
        /// <summary>
        /// Decides whether the history entry can be redo.
        /// </summary>
        public bool CanRedo { get; set; }
        /// <summary>
        /// Specifies the current entry object.
        /// </summary>

        [Parameter]
        public HistoryEntryBase CurrentEntry { get; set; }

        /// <summary>
        /// Stores a history entry on the history list.
        /// </summary>
        internal void Push(HistoryEntryBase entry)
        {
            if (entry != null)
                Diagram.AddHistoryEntry(entry);
        }

        ///<summary>
        /// The method will be called when the custom entry is in undo stage.
        /// </summary>
        /// <example>
        /// <code lang="Razor">
        /// <![CDATA[
        /// <SfDiagramComponent Width="800px" Height="800px">
        ///     <DiagramHistoryManager Undo="onCustomUndo"></DiagramHistoryManager>
        /// </SfDiagramComponent >
        /// @code
        /// {
        ///     private void onCustomUndo(HistoryEntryBase entry)
        ///     {
        ///         (entry.RedoObject) = entry.UndoObject.Clone() as Node;
        ///         (entry.UndoObject as Node).AdditionalInfo[(entry.UndoObject as Node).ID] = "Start";
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<HistoryEntryBase> Undo { get; set; }
        /// <summary>
        /// The method will be called when the custom entry is in the redo stage.
        /// </summary>
        /// <example>
        /// <code lang="Razor">
        /// <![CDATA[
        /// <SfDiagramComponent Width="800px" Height="800px">
        ///     <DiagramHistoryManager Redo="onCustomRedo"></DiagramHistoryManager>
        /// </SfDiagramComponent>
        /// @code
        /// {
        ///     private void onCustomRedo(HistoryEntryBase entry)
        ///     {
        ///         Node current = entry.UndoObject.Clone() as Node;
        ///         (entry.UndoObject as Node).AdditionalInfo[(entry.UndoObject as Node).ID] = "Description";
        ///         entry.RedoObject = current;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<HistoryEntryBase> Redo { get; set; }
        /// <summary>
        /// History allows you to revert or restore multiple changes through a single undo/redo command. It is used to start the grouping of changes.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// //Start the grouping of changes 
        /// diagram.StartGroupAction();
        /// ]]>
        /// </code>
        /// </example>
        public void StartGroupAction()
        {
            this.Diagram.StartGroupAction();
        }

        ///<summary>
        /// Used to intimate the group action is end
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// //End the grouping of changes
        /// diagram.EndGroupAction();
        /// ]]>
        /// </code>
        /// </example>
        public void EndGroupAction()
        {
            this.Diagram.EndGroupAction();
        }

        ///<summary>
        /// Decides whether the changes are stored in the history or not.
        /// </summary>
        /// <example>
        /// <code lang="Razor">
        /// <![CDATA[
        /// <SfDiagramComponent Width="800px" Height="800px">
        ///     <DiagramHistoryManager HistoryAdding="OnHistoryAdding"></DiagramHistoryManager>
        /// </SfDiagramComponent>
        /// @code
        /// {
        ///     private void OnHistoryAdding(HistoryAddingEventArgs args)
        ///     {
        ///         args.Cancel = true;
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<HistoryAddingEventArgs> HistoryAdding { get; set; }
        /// <summary>
        /// Used to store the list of entries in the undo list.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// private void onundostackcount()
        /// {
        ///     var undostackcount = diagram.HistoryManager.UndoStack.Count();
        /// }
        /// ]]>
        /// </code>
        /// </example>
        internal List<HistoryEntryBase> UndoStack { get; set; } = new List<HistoryEntryBase>();
        /// <summary>
        /// Used to store the list of entries in the redo list.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// private void onredostackcount()
        /// {
        ///     var Redostackcount = diagram.HistoryManager.RedoStack.Count();
        /// }
        /// ]]>
        /// </code>
        /// </example>
        internal List<HistoryEntryBase> RedoStack { get; set; } = new List<HistoryEntryBase>();

        ///<summary>
        /// Used to restrict or limits the number of history entry will be stored on the history list
        /// </summary>
        internal double StackLimit { get; set; }

        internal void UpdateCurrentEntry(HistoryEntryBase historyEntry)
        {
            CurrentEntry = historyEntry;
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (UndoStack != null)
            {
                UndoStack.Clear();
                UndoStack = null;
            }
            if (RedoStack != null)
            {
                RedoStack.Clear();
                RedoStack = null;
            }
            if (CurrentEntry != null)
            {
                CurrentEntry = null;
            }
            ChildContent = null;
            Diagram = null;
        }
    }
}

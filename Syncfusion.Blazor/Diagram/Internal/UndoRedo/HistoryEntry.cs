using System.Collections.Generic;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the entry of the changed history of an object. 
    /// </summary>
    public abstract class HistoryEntryBase
    {
        /// <summary>
        /// Sets the type of  entry to be stored. 
        /// </summary>
        public HistoryEntryType Type { get; set; }
        /// <summary>
        /// Gets or sets the type of object beingadded or removed. 
        /// </summary>
        internal HistoryEntryChangeType ChangeType { get; set; }

        /// <summary>
        /// Stores the changed values. 
        /// </summary>
        public IDiagramObject RedoObject { get; set; }

        /// <summary>
        /// Stores the changed values. 
        /// </summary>
        public IDiagramObject UndoObject { get; set; }

        /// <summary>
        /// Sets the category of the history entry like internal or external. Category should be external for custom action.
        /// </summary>
        internal EntryCategory Category { get; set; }

        /// <summary>
        /// Sets the next of the current undoing object. 
        /// </summary>
        public HistoryEntryBase Next { get; set; }

        /// <summary>
        /// Sets the previous of the current undoing object. 
        /// </summary>
        public HistoryEntryBase Previous { get; set; }

        /// <summary>
        ///  Gets or sets the value for undo action is activated. A user needs to set this property to true in order to record the changes that performs undo/redo actions.
        /// </summary>
        public bool IsUndo { get; set; }

        internal virtual void Dispose()
        {
            if (RedoObject != null)
            {
                RedoObject = null;
            }
            if (UndoObject != null)
            {
                UndoObject = null;
            }
            if (Previous != null)
            {
                Previous = null;
            }
            if (Next != null)
            {
                Next = null;
            }
        }
    }
    internal class InternalHistoryEntry : HistoryEntryBase
    {
        internal PropertyChangedEventArgs PropertyChangeEvtArgs { get; set; }

        ///<summary>
        /// Used to stored the entry or not
        /// </summary>
        internal Dictionary<string, IDiagramObject> ChildTable;

        internal InternalHistoryEntry() : base()
        {
            ChildTable = new Dictionary<string, IDiagramObject>();
            Category = EntryCategory.InternalEntry;
        }
        internal override void Dispose()
        {
            base.Dispose();
            if (ChildTable != null)
            {
                ChildTable.Clear();
                ChildTable = null;
            }
        }
    }
    /// <summary>
    /// Represents the entry of the changed history of an object. 
    /// </summary>
    public class HistoryEntry : HistoryEntryBase
    {
        /// <summary>
        /// To initialize custom entry of the an object. 
        /// </summary>
        public HistoryEntry() : base()
        {
            Category = EntryCategory.ExternalEntry;
        }
        internal override void Dispose()
        {
            base.Dispose();
        }
    }
}

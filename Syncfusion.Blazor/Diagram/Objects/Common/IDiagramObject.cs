using System;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the class that is the base class for all the diagram objects, and it is used to handle common actions like property changes in any diagram object. 
    /// </summary>
    public interface IDiagramObject : ICloneable
    {
        /// <summary>
        /// Invoked whenever the effective value of any property on this diagram objects has been updated.
        /// </summary>
        void OnPropertyChanged(string propertyName, object newVal, object oldVal, IDiagramObject container);
    }
}

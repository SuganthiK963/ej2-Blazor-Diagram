using Syncfusion.Blazor.Diagram;

namespace Syncfusion.Blazor.Diagram.SymbolPalette
{
    /// <summary>
    /// Represents the base class for all the symbol palette objects. It is used to handle common actions like property changes in any symbol palette objects.
    /// </summary>
    public interface IPaletteObject : IDiagramObject
    {
        /// <summary>
        /// Invoked whenever the effective value of any property in this Symbol palette object has been updated. 
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="container">IPaletteObject</param>
        void OnPropertyChanged(string propertyName, object newVal, object oldVal, IPaletteObject container);
    }
}

using System.Threading.Tasks;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// An interface for processing the Syncfusion UI component's Localization.
    /// </summary>
    public interface ISyncfusionStringLocalizer
    {
        /// <summary>
        /// ResourceManager for processing the resource file parsing.
        /// </summary>
        public System.Resources.ResourceManager ResourceManager { get; }

        /// <summary>
        /// Return the Localized value from the resource file.
        /// </summary>
        /// <param name="key">Key string to get the localized value.</param>
        /// <returns>Returns the localized string.</returns>
        public string GetText(string key);
    }
}

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// An interface for circular component reference.
    /// GridColumns -> GridColumn -> GridColumns -> GridColumn.
    /// </summary>
    public interface ISfCircularComponent
    {
        /// <summary>
        /// Update child property values from parent component.
        /// </summary>
        /// <param name="key">Child property name.</param>
        /// <param name="propertyValue">Child property value.</param>
        public void UpdateChildProperties(string key, object propertyValue);
    }

    /// <summary>
    /// Common interface for blazor components.
    /// </summary>
    public interface IBaseInit
    {
        /// <summary>
        /// Invoked after script loaded at first rendering.
        /// </summary>
        /// <returns>Task.</returns>
        public Task OnInitRenderAsync();
    }
}

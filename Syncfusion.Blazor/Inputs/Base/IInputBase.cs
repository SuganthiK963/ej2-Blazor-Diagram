using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs.Internal
{
    /// <summary>
    /// Common properties for input base component.
    /// </summary>
    public interface IInputBase
    {
        /// <summary>
        /// Triggers each time when the value of input has changed.
        /// </summary>
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Update the parent component root class and container element class.
        /// </summary>
        /// <param name="rootClass">Specifies the root class of the InputBase.</param>
        /// <param name="containerClass">Specifies the container class of the InputBase.</param>
        public void UpdateParentClass(string rootClass, string containerClass);
    }

    /// <summary>
    /// Specifies the icon handler arguments.
    /// </summary>
    public class IconHandlerArgs
    {
        /// <summary>
        /// Specifies the <see cref="System.EventArgs"/> arguments.
        /// </summary>
        public System.EventArgs eventArgs { get; set; }

        /// <summary>
        /// Specifies the name of the icon.
        /// </summary>
        public string IconName { get; set; }
    }
}
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Interface for DropDowns component.
    /// </summary>
    /// <exclude/>
    public interface IDropDowns
    {
        /// <summary>
        /// Update the child component properties.
        /// </summary>
        public void UpdateChildProperties(object fieldValue);

        /// <summary>
        /// Refresh the parent component.
        /// </summary>
        public Task CallStateHasChangedAsync();

        /// <summary>
        /// Accepts the template design and assigns it to popup list of DropDowns component, when no data is available on the component.
        /// </summary>
        public RenderFragment NoRecordsTemplate { get; set; }
        /// <summary>
        /// Accepts the template and assigns it to the popup list content of the DropDowns component, when the data fetch request from the remote server fails.
        /// </summary>
        public RenderFragment ActionFailureTemplate { get; set; }
    }
}
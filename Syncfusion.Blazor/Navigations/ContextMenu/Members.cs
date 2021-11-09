using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// ContextMenu is a graphical user interface that appears on the user right click/touch hold operation.
    /// </summary>
    public partial class SfContextMenu<TValue> : SfMenuBase<TValue>
    {
        /// <summary>
        /// Specifies the filter selector for elements inside the target in that the context menu will be opened.
        /// </summary>
        [Parameter]
        public string Filter { get; set; } = string.Empty;

        private string filter;

        /// <summary>
        /// Specifies target element selector in which the ContextMenu should be opened.
        /// </summary>
        [Parameter]
        public string Target { get; set; } = string.Empty;

        private string target;

        /// <summary>
        /// Specifies the ContextMenu triggering event name.
        /// </summary>
        [Parameter]
        public string ShowOn { get; set; } = "contextmenu";

        private string showOn;
    }
}
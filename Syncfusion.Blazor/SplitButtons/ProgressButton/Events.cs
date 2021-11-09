using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// ProgressButton visualizes the progression of an operation to indicate the user that a process is happening in the background with visual representation.
    /// </summary>
    public partial class SfProgressButton : SfBaseComponent
    {
        /// <summary>
        /// Triggers when button element is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Configures to display a toast in the custom position within the document or target.
    /// </summary>
    public class ToastPosition : SfBaseComponent
    {
        [CascadingParameter]
        private SfToast Parent { get; set; }

        /// <summary>
        /// Specifies the position of the Toast notification with respect to the target container's left edge.
        /// </summary>
        [Parameter]
        public string X { get; set; } = "Left";

        /// <summary>
        /// Specifies the position of the Toast notification with respect to the target container's top edge.
        /// </summary>
        [Parameter]
        public string Y { get; set; } = "Top";

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Parent.UpdatePosition(this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
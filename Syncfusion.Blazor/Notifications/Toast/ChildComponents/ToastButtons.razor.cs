using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// A list of buttons that are used to configure the Toast buttons.
    /// </summary>
    public partial class ToastButtons : SfBaseComponent
    {
        [CascadingParameter]
        private SfToast Parent { get; set; }

        /// <summary>
        /// Gets or sets the content of the Spinner element.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        internal List<ToastButton> ActionButtons { get; set; } = new List<ToastButton>();

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender"> Set to true if this is the first time OnAfterRender(Boolean) has been invoked.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Parent.UpdateActionButtons(ActionButtons);
            await base.OnAfterRenderAsync(firstRender);
        }

        internal void UpdateChildProperty(ToastButton button)
        {
            ActionButtons.Add(button);
        }

        internal override void ComponentDispose()
        {
            ActionButtons.Clear();
            ChildContent = null;
            Parent = null;
        }
    }
}
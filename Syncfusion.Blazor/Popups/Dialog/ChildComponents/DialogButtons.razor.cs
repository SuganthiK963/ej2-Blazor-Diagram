using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// A list of buttons that are used to configure the Dialog buttons.
    /// </summary>
    public partial class DialogButtons : SfBaseComponent
    {
        [CascadingParameter]
        internal SfDialog Parent { get; set; }

        /// <summary>
        /// Gets or sets the content of the Dialog Button element.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        internal List<DialogButton> Buttons { get; set; } = new List<DialogButton>();

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender"> Set to true if this is the first time OnAfterRender(Boolean) has been invoked.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Parent.UpdateButtons(Buttons);
            await base.OnAfterRenderAsync(firstRender);
        }

        internal int UpdateChildProperty(DialogButton button)
        {
            if (button != null)
            {
                Buttons.Add(button);
            }
            Parent.UpdateButtons(Buttons);
            return Buttons.Count - 1;
        }

        internal override void ComponentDispose()
        {
            Buttons.Clear();
            ChildContent = null;
            Parent = null;
        }
    }
}
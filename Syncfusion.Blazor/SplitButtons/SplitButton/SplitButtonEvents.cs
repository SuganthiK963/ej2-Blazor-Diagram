using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.SplitButtons
{
    public class SplitButtonEvents : SfBaseComponent
    {
        [CascadingParameter]
        private SfSplitButton Parent { get; set; }

        /// <summary>
        /// Triggers before closing the SplitButton popup.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenCloseMenuEventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggers while rendering each Popup item of SplitButton.
        /// </summary>
        [Parameter]
        public EventCallback<MenuEventArgs> OnItemRender { get; set; }

        /// <summary>
        /// Triggers before opening the SplitButton popup.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenCloseMenuEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Triggers when the primary button of SplitButton has been clicked.
        /// </summary>
        [Parameter]
        public EventCallback<ClickEventArgs> Clicked { get; set; }

        /// <summary>
        /// Triggers while closing the SplitButton popup.
        /// </summary>
        [Parameter]
        public EventCallback<OpenCloseMenuEventArgs> Closed { get; set; }

        /// <summary>
        /// Triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers while opening the SplitButton popup.
        /// </summary>
        [Parameter]
        public EventCallback<OpenCloseMenuEventArgs> Opened { get; set; }

        /// <summary>
        /// Triggers while selecting action item of SplitButton popup.
        /// </summary>
        [Parameter]
        public EventCallback<MenuEventArgs> ItemSelected { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Delegates = this;
        }
    }
}
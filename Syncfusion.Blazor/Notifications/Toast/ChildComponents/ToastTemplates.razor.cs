using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Configure templates to display within a header, content, and footer section of Toast.
    /// </summary>
    public partial class ToastTemplates : SfBaseComponent
    {
        /// <exclude/>
        /// <summary>
        /// Gets or sets the content of the Spinner element.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        private SfToast Parent { get; set; }

        /// <summary>
        /// Defines the toast title template.
        /// </summary>
        [Parameter]
        public RenderFragment Title { get; set; }

        /// <summary>
        /// Defines the toast content template.
        /// </summary>
        [Parameter]
        public RenderFragment Content { get; set; }

        /// <summary>
        /// Defines the toast template.
        /// </summary>
        [Parameter]
        public RenderFragment Template { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateTemplate(Title, Content, Template);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}
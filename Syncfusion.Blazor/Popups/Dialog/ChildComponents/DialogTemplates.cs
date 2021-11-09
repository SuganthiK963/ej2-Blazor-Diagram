using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Configure templates to display within a header, content, and footer section of Dialog.
    /// </summary>
    public class DialogTemplates : SfBaseComponent
    {
        [CascadingParameter]
        internal SfDialog Parent { get; set; }

        /// <summary>
        /// Specifies the value that can be displayed in the dialog's title area that can be configured with a plain text.
        /// The dialog will be displayed without the header if the header property is null.
        /// </summary>
        [Parameter]
        public RenderFragment Header { get; set; }

        /// <summary>
        /// Specifies the value that can be displayed in the dialog's content section.
        /// </summary>
        [Parameter]
        public RenderFragment Content { get; set; }

        /// <summary>
        /// Defines the footer template of the dialog.
        /// </summary>
        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            if (Header != null)
            {
                Parent.UpdateTemplate(nameof(Header), Header);
            }

            if (Content != null)
            {
                Parent.UpdateTemplate(nameof(Content), Content);
            }

            if (FooterTemplate != null)
            {
                Parent.UpdateTemplate(nameof(FooterTemplate), FooterTemplate);
            }
            Parent.Refresh();
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
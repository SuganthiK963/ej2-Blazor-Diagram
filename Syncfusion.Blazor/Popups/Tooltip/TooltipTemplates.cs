using Syncfusion.Blazor;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Configure templates to display content within the tooltip component.
    /// </summary>
    public class TooltipTemplates : SfBaseComponent
    {
        /// <exclude/>
        /// <summary>
        /// Defines the content which has to be passed.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        internal SfTooltip SfTooltip { get; set; }

        /// <exclude/>
        /// <summary>
        /// Defines the content to be passed.
        /// </summary>
        [Parameter]
        public RenderFragment Content { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            SfTooltip.Template = this;
            UpdateContent();
        }

        internal void UpdateContent()
        {
            if (Content != null)
            {
                // invoking the update template method in the tooltip component
                SfTooltip.UpdateTemplate(nameof(Content), Content);
            }
        }
    }
}
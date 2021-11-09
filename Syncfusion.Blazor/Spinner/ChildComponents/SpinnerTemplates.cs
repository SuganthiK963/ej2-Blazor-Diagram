using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Spinner
{
    /// <summary>
    /// Configure the template of the Spinner.
    /// </summary>
    public class SpinnerTemplates : SfBaseComponent
    {
        [CascadingParameter]
        private SfSpinner BaseParent { get; set; }

        /// <summary>
        /// Defines the template of the Spinner.
        /// </summary>
        [Parameter]
        public RenderFragment Template { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            if (Template != null)
            {
                BaseParent.UpdateTemplate(Template);
            }
        }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }
    }
}
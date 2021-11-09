using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// Specifies the spin settings for the progress button.
    /// </summary>
    public partial class ProgressButtonSpinSettings : SfBaseComponent
    {
        [CascadingParameter]
        private SfProgressButton progressButton { get; set; }

        /// <summary>
        /// Specifies the position of a spinner in the progress button. The possible values are:
        ///  Left: The spinner will be positioned to the left of the text content.
        ///  Right: The spinner will be positioned to the right of the text content.
        ///  Top: The spinner will be positioned at the top of the text content.
        ///  Bottom: The spinner will be positioned at the bottom of the text content.
        ///  Center: The spinner will be positioned at the center of the progress button.
        /// </summary>
        [Parameter]
        public SpinPosition Position { get; set; }

        /// <summary>
        /// Specifies the template content to be displayed in a spinner.
        /// </summary>
        [Parameter]
        public RenderFragment SpinTemplate { get; set; }

        /// <summary>
        /// Sets the width of a spinner.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "16";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            progressButton.UpdateChildProperties("SpinSettings", this);
        }

        internal override void ComponentDispose()
        {
            progressButton = null;
        }
    }
}

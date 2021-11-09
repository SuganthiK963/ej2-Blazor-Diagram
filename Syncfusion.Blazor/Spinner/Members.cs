using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Spinner
{
    public partial class SfSpinner : SfBaseComponent
    {
        private bool visible;
        private string indexZ;
        private SpinnerType type;

        /// <summary>
        /// Specifies the label for the Spinner.
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the Spinner.
        /// One or more custom CSS classes can be added to a Spinner.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <exclude/>
        /// <summary>
        /// Specified content of the Spinner element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the value that represents whether the Spinner component is visible.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a callback of the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        /// <summary>
        /// Specifies the size of the Spinner.
        /// </summary>
        [Parameter]
        public string Size { get; set; }

        /// <summary>
        /// Specifies the z-order for the Spinner.
        /// By default the value is AUTO.
        /// </summary>
        [Parameter]
        public string ZIndex { get; set; } = AUTO;

        /// <summary>
        /// Specifies a theme of the spinner.
        /// </summary>
        [Parameter]
        public SpinnerType Type { get; set; }
    }
}
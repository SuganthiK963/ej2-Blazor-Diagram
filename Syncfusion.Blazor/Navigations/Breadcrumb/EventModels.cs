using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Navigations
{

    /// <summary>
    /// Provides information about the <see cref="SfBreadcrumb.ItemRendering"/> event callback.
    /// </summary>
    public class BreadcrumbItemRenderingEventArgs
    {
        /// <summary>
        /// Gets or sets the breadcrumb item that is being render. 
        /// </summary>
        public BreadcrumbItem Item { get; internal set; }
    }

    /// <summary>
    /// Provides information about the <see cref="SfBreadcrumb.ItemClicked"/> event callback.
    /// </summary>
    public class BreadcrumbClickedEventArgs
    {
        /// <summary>
        /// Gets the clicked breadcrumb item.
        /// </summary>
        public BreadcrumbItem Item { get; internal set; }
    }
}

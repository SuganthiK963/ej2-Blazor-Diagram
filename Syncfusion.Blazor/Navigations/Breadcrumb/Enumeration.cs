namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies how to display breadcrumb items in <see cref="SfBreadcrumb"/> component when the Breadcrumb items exceeds Breadcrumb container or <see cref="SfBreadcrumb.MaxItems"/> property.
    /// </summary>
    public enum BreadcrumbOverflowMode
    {
        /// <summary>
        /// The specified <see cref="SfBreadcrumb.MaxItems"/> count will be visible and the remaining items will be hidden. While clicking on the previous item, the hidden item will become visible.
        /// </summary>
        Default,

        /// <summary>
        /// Only the first and last items will be visible, and the remaining items will be hidden with collapsed icon. When the collapsed icon is clicked, all items become visible and scroll will be enabled if the space is not enough to show all items.
        /// </summary>
        Collapsed
    }
}

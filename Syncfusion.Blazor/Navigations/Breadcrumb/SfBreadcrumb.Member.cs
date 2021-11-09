using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    public partial class SfBreadcrumb : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the Url based on which the Breadcrumb items are generated.
        /// </summary>
        /// <value>
        /// The value as a Url string to generate Breadcrumb items. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// You can specify only absolute Url to this property.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfBreadcrumb Url="https://blazor.syncfusion.com/demos/breadcrumb/navigation">
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        /// <seealso cref="BreadcrumbItems"/>
        [Parameter]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the list of breadcrumb items that will be populated using the <see cref="BreadcrumbItems"/> tag directive. 
        /// </summary>
        /// <value>
        /// <see cref="BreadcrumbItems"/>
        /// </value>
        [Parameter]
        public List<BreadcrumbItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BreadcrumbItem.Url"/> of the active breadcrumb item.
        /// </summary>
        /// <value>
        /// This property contains Url string of active breadcrumb item.
        /// </value>
        /// <remarks>
        /// This property is updated only when the <see cref="BreadcrumbItem.Url" /> has value.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems ActiveItem="@activeItem">
        ///         <BreadcrumbItem Text="Home" Url="https://blazor.syncfusion.com/demos/"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Components" Url="https://blazor.syncfusion.com/demos/datagrid/overview"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Navigations" Url="https://blazor.syncfusion.com/demos/menu-bar/default-functionalities"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Breadcrumb" Url="https://blazor.syncfusion.com/demos/breadcrumb/default-functionalities"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// @code {
        ///     private string activeItem = "https://blazor.syncfusion.com/demos/menu-bar/default-functionalities";
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public string ActiveItem { get; set; }
        private string activeItem { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of breadcrumb items to be visible in the breadcrumb component.
        /// If the number of items exceeds this count, then items are rendered based on <see cref="OverflowMode"/> property. 
        /// </summary>
        /// <value>
        /// The number of breadcrumb items to be visible in breadcrumb component. The default value is <c>0</c>. 
        /// </value>
        /// <remarks>
        /// The <see cref="MaxItems"/> is applicable only if the number of <see cref="BreadcrumbItem"/> is greater than <c>2</c>.
        /// </remarks>
        [Parameter]
        public int MaxItems { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates how to display breadcrumb items when the breadcrumb items count exceeds <see cref="MaxItems"/>.
        /// </summary>
        /// <value>
        /// One of the <see cref="BreadcrumbOverflowMode"/> enumeration. The default value is <see cref="BreadcrumbOverflowMode.Default"/>
        /// </value>
        /// <remarks>
        /// If the <c>OverflowMode</c> is <c>Default</c>, the exceeded items will be hidden and while clicking on the previous item, the hidden item will become visible.
        /// If the <c>OverflowMode</c> is <c>Collapsed</c>,  only the first and last items will be visible, and the remaining items will be hidden with collapsed icon.
        /// When the collapsed icon is clicked, all items become visible and scroll will be enabled if the space is not enough to show all items. 
        /// </remarks>
        /// <seealso cref="Width"/>
        [Parameter]
        public BreadcrumbOverflowMode OverflowMode { get; set; }

        /// <summary>
        /// Gets or sets the width for the Breadcrumb component container element.
        /// If the Breadcrumb items overflows the container width, the browser scrollbar will be activated.
        /// </summary>
        /// <value>
        /// It contains component width in pixel or percentage. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// If the <see cref="MaxItems"/> property is specified, the <see cref="MaxItems"/> property will be prioritized than Breadcrumb container width and responsive UI will be activated based on <see cref="OverflowMode"/>.
        /// You can set container width using CSS and HTML style attribute using <c>@attributes</c>.
        /// </remarks>
        /// <seealso cref="MaxItems"/>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets whether the built-in item navigation is enabled or not. The breadcrumb component navigates to url based on the item clicked by user.
        /// </summary>
        /// <value>
        /// <c>true</c>, if built-in item navigation is enabled; otherwise, <b>false</b>.The default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// You can handle navigation in using <see cref="ItemClicked"/> event by setting <see cref="EnableNavigation"/> as <c>false</c>.
        /// </remarks>
        [Parameter]
        public bool EnableNavigation { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the navigation is enabled for active item (last breadcrumb item).
        /// </summary>
        /// <value>
        /// <c>true</c>, if the navigation is enabled for last breadcrumb item and it is clickable. otherwise, <b>false</b>.The default value is <c>false</c>.
        /// </value>
        [Parameter]
        public bool EnableActiveItemNavigation { get; set; }

        /// <summary>
        /// Gets or sets whether to persist component's state between page reloads. When set to <c>true</c>, the <see cref="ActiveItem" /> property is persisted.
        /// </summary>
        /// <value>
        /// <c>true</c>, if the component's state persistence is enabled. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// Component's <see cref="ActiveItem"/> property will be stored in browser local storage to persist component's state when page reloads.
        /// </remarks>
        [Parameter]
        public bool EnablePersistence { get; set; }
        
        /// <summary>
        /// Gets or sets a collection of additional attributes that will applied to the Breadcrumb container element.
        /// </summary>
        /// <remarks>
        /// Additional attributes can be added by specifying as inline attributes or by specifying <c>@attributes</c> directive.
        /// </remarks>
        /// <example>
        /// In the below code example, Breadcrumb width has been specified as style attribute in <see cref="SfBreadcrumb"/> tag directive.
        /// <code><![CDATA[
        /// <SfBreadcrumb style="width:200px">
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Home" Url="https://blazor.syncfusion.com/demos/"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Components" Url="https://blazor.syncfusion.com/demos/datagrid/overview"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Navigations" Url="https://blazor.syncfusion.com/demos/menu-bar/default-functionalities"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Breadcrumb" Url="https://blazor.syncfusion.com/demos/breadcrumb/default-functionalities"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get { return htmlAttributes; } set { htmlAttributes = value; } }

        /// <exclude />
        /// <summary>
        /// Gets or sets the child content of Breadcrumb component.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <exclude/>
        /// <summary>
        /// Gets or sets a callback that updates the bound active item.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCallback<string> ActiveItemChanged { get; set; }
    }
}
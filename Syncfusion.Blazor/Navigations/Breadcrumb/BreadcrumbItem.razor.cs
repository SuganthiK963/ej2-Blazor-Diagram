using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class that represents breadcrumb component item of <see cref="SfBreadcrumb"/> component.
    /// </summary>
    /// <remarks>
    /// You can render icon only, text only, icon and text breadcrumb item by specifying value to corresponding property.
    /// Additional attributes can be added to Breadcrumb item using <c>@attributes</c> directive.
    /// </remarks>
    /// <example>
    /// In the below code example, a basic Breadcrumb item has been added using <see cref="BreadcrumbItem"/> tag directive.
    /// <code><![CDATA[
    /// <SfBreadcrumb>
    ///     <BreadcrumbItems>
    ///         <BreadcrumbItem Text="Home" Url="https://blazor.syncfusion.com/demos/"></BreadcrumbItem>
    ///         <BreadcrumbItem Text="Components" Url="https://blazor.syncfusion.com/demos/datagrid/overview"></BreadcrumbItem>
    ///         <BreadcrumbItem Text="Navigations" Url="https://blazor.syncfusion.com/demos/menu-bar/default-functionalities"></BreadcrumbItem>
    ///         <BreadcrumbItem Text="Breadcrumb" Url="https://blazor.syncfusion.com/demos/breadcrumb/default-functionalities"></BreadcrumbItem>
    ///     </BreadcrumbItems>
    /// </SfBreadcrumb>
    /// ]]></code>
    /// </example>
    public partial class BreadcrumbItem : SfBaseComponent
    {
        internal Dictionary<string, object> itemHtmlAttributes;

        [CascadingParameter]
        internal BreadcrumbItems Parent { get; set; }

        /// <summary>
        /// Gets or sets the child content for the Breadcrumb item. If the child content is not specified breadcrumb item is rendered using <see cref="Text"/> content.
        /// </summary>
        /// <value>
        /// The template content. The default value in <c>null</c>.
        /// </value>
        /// <example>
        /// In the below code example, content has been set to <see cref="BreadcrumbItem"/> using <see cref="Text"/> property and <see cref="ChildContent"/> property.
        /// <code><![CDATA[
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem><span>Components</span></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the text content of the Breadcrumb item.
        /// </summary>
        /// <value>
        /// Accepts a string value. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// Set child content within <see cref="BreadcrumbItem"/> tag directive, to render as HTML content.
        /// </remarks>
        /// <example>
        /// In the below code example, content has been set to <see cref="BreadcrumbItem"/> using <see cref="Text"/> property and <c>ChildContent</c> property.
        /// <code><![CDATA[
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Home"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem>Home</BreadcrumbItem>
        ///         <BreadcrumbItem><span>Components</span></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Url of the Breadcrumb item and that will be navigated when clicked.
        /// </summary>
        /// <value>
        /// Accepts Url string value. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// When Url has not been set, you can customize the item navigation using <see cref="Syncfusion.Blazor.Navigations.SfBreadcrumb.ItemClicked"/> event.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfBreadcrumb">
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Home" Url="https://blazor.syncfusion.com/demos/"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Components" Url="https://blazor.syncfusion.com/demos/datagrid/overview"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Navigations" Url="https://blazor.syncfusion.com/demos/menu-bar/default-functionalities"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a CSS class string to include an icon or image for the breadcrumb item. 
        /// </summary>
        /// <value>
        /// Accepts a CSS class string separated by space to include an icon or image for the breadcrumb item. The default value is <c>String.Empty</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem IconCss="e-icons e-home"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the breadcrumb item element.
        /// </summary>
        /// <remarks>
        /// Additional attributes can be added by specifying as inline attributes or by specifying <c>@attributes</c> directive.
        /// </remarks>
        /// <example>
        /// In the below code example, title attribute added as inline in <see cref="BreadcrumbItem"/> tag directive.
        /// <code><![CDATA[
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Home" title="Home"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get { return itemHtmlAttributes; } set { itemHtmlAttributes = value; } }
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this, false);
        }

        internal void UpdateChildProperties(string key, object result)
        {
            switch(key)
            {
                case "text":
                    Text = (string)result;
                    break;
                case "url":
                    Url = (string)result;
                    break;
                case "iconCss":
                    IconCss = (string)result;
                    break;
            }
        }

        internal override void ComponentDispose()
        {
            Parent?.UpdateChildProperty(this, true);
            Parent = null;
            itemHtmlAttributes = null;
            ChildContent = null;
        }
    }
}

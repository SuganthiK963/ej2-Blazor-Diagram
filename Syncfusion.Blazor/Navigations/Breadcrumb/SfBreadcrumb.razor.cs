using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Breadcrumb component is a graphical user interface that helps to identify or highlight the current location within a hierarchical structure of websites.
    /// The aim is to make the user aware of their current position in a hierarchy of website links.
    /// </summary>
    /// <remarks>
    /// Breadcrumb items can be populated either by specifying <see cref="SfBreadcrumb.Url"/> property or by specifying <see cref="BreadcrumbItem"/> within <see cref="BreadcrumbItems"/> tag directive.
    /// <see cref="BreadcrumbTemplates.ItemTemplate"/> and <see cref="BreadcrumbTemplates.SeparatorTemplate"/> can be used to customize the Breadcrumb component UI using <see cref="Syncfusion.Blazor.Navigations.BreadcrumbTemplates"/> tag directive.
    /// </remarks>
    /// <example>
    /// In the below code example, a basic Breadcrumb component initialized with <see cref="BreadcrumbItems"/> tag directive.
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
    public partial class SfBreadcrumb : SfBaseComponent
    {
        private string styleAttributes;
        private Dictionary<string, object> htmlAttributes;

        internal string IdValue;
        internal string BreadcrumbClass;
		internal BreadcrumbTemplates BreadcrumbTemplates;
                
        [Inject]
        private NavigationManager navigationManager { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised while rendering <see cref="BreadcrumbItem"/>.
        /// </summary>
        /// <remarks>
        /// You can customize the breadcrumb items rendering using <see cref="BreadcrumbItemRenderingEventArgs"/>.
        /// </remarks>
        /// <example>
        /// In the below code example, the breadcrumb item text is changed to lower casing using <c>ItemRendering</c> event.
        /// <code><![CDATA[
        /// <SfBreadcrumb ItemRendering="@ItemRendering">
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Program Files"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Commom Files"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Services"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Config.json"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// @code {
        ///     private void ItemRendering(BreadcrumbItemRenderingEventArgs args) {
        ///         args.Item.Text = args.Item.Text.ToLower();
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<BreadcrumbItemRenderingEventArgs> ItemRendering { get; set; }

        /// <summary>
        /// Gets or sets an event callback that is raised when the <see cref="BreadcrumbItem"/> is clicked. 
        /// </summary>
        /// <remarks>
        /// You can customize the item navigation using <see cref="BreadcrumbClickedEventArgs"/> by setting <seealso cref="EnableNavigation"/> as <c>false</c>.
        /// The event is raised for UI based click only.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// @inject NavigationManager NavigationManager
        /// <SfBreadcrumb ItemClicked="@ItemClicked" EnableNavigation="false">
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Program Files" Url="programfiles"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Commom Files" Url="commomfiles"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Services" Url="services"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Config" Url="config"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        /// </SfBreadcrumb>
        /// @code {
        ///     private void ItemClicked(BreadcrumbClickedEventArgs args) {
        ///         NavigationManager.NavigateTo(args.Item.Url);
        ///     }
        /// }
        /// ]]></code>
        /// </example>
        [Parameter]
        public EventCallback<BreadcrumbClickedEventArgs> ItemClicked { get; set; }

        internal void UpdateChildProperties(string key, object result)
        {
            switch (key)
            {
                case "Items":
                    Items = (List<BreadcrumbItem>)result;
                    StateHasChanged();
                    break;
                case "BreadcrumbTemplates":
                    BreadcrumbTemplates = (BreadcrumbTemplates)result;
                    break;
                case "ActiveItem":
                    ActiveItem = (string)result;
                    StateHasChanged();
                    break;
            }
        }

        internal async Task UpdateActiveItem(string url)
        {
            ActiveItem = activeItem = await SfBaseUtils.UpdateProperty(url, activeItem, ActiveItemChanged);
        }
    }
}

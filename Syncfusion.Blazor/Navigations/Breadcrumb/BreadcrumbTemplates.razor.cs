using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Represents template options in <see cref="SfBreadcrumb"/> component.
    /// </summary>
    /// <remarks>
    /// <see cref="ItemTemplate"/> or <see cref="SeparatorTemplate"/> can be set <see cref="BreadcrumbTemplates"/> tag directive.
    /// To access template's context parameter use <c>@context</c> in both <see cref="ItemTemplate"/> and <see cref="SeparatorTemplate"/>.
    /// </remarks>
    /// <example>
    /// In the below code example, both <see cref="ItemTemplate"/> and <see cref="SeparatorTemplate"/> has been specified.
    /// <code><![CDATA[
    /// <SfBreadcrumb>
    ///     <BreadcrumbItems>
    ///         <BreadcrumbItem Text="Home"></BreadcrumbItem>
    ///         <BreadcrumbItem Text="Components"></BreadcrumbItem>
    ///         <BreadcrumbItem Text="Navigations"></BreadcrumbItem>
    ///     </BreadcrumbItems>
    ///     <BreadcrumbTemplates>
    ///         <ItemTemplate>
    ///             <i>@context.Text</i>
    ///         </ItemTemplate>
    ///         <SeparatorTemplate>
    ///             <span class="e-icons e-arrow"></span>
    ///         </SeparatorTemplate>
    ///     </BreadcrumbTemplates>
    /// </SfBreadcrumb>
    /// ]]></code>
    /// </example>
    public class BreadcrumbTemplates : SfBaseComponent
    {
        [CascadingParameter]
        private SfBreadcrumb parent { get; set; }

        /// <summary>
        /// Gets or sets template as <see cref="RenderFragment"/>, that defines custom appearance of breadcrumb items.
        /// Here, context refers to the <see cref="BreadcrumbItem"/> for which the template is applied.
        /// </summary>
        /// <value>
        /// A template content that specifies the visualization of breadcrumb items. The default value in <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <c>ItemTemplate</c> used to define appearance of breadcrumb items. Use the template’s context parameter to access the <see cref="BreadcrumbItem"/> properties.
        /// Specify <see cref="ItemTemplate"/> within <see cref="BreadcrumbTemplates"/> tag directive.
        /// </remarks>        
        /// <example>
        /// In the below code example, <b>italic</b> style Breadcrumb item text has been rendered using <c>ItemTemplate</c>.
        /// <code><![CDATA[
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Home"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Components"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Navigations"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        ///     <BreadcrumbTemplates>
        ///         <ItemTemplate>
        ///             <i>@context.Text</i>
        ///         </ItemTemplate>
        ///     </BreadcrumbTemplates>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment<BreadcrumbItem> ItemTemplate { get; set; }

        /// <summary>
        /// Gets or sets template as <see cref="RenderFragment"/>, that defines custom appearance of breadcrumb items separator.
        /// Here, context refers to the <see cref="BreadcrumbItem"/> for previous and current item which the template is applied.
        /// </summary>
        /// <value>
        /// A template content that specifies the visualization of breadcrumb items separator. The default value in <c>null</c>.
        /// </value>
        /// <remarks>
        /// The <c>SeparatorTemplate</c> used to define appearance of breadcrumb items separator. Use the template’s context parameter to access the previous and current <see cref="BreadcrumbItem"/> properties.
        /// Specify <see cref="SeparatorTemplate"/> within <see cref="BreadcrumbTemplates"/> tag directive.
        /// </remarks>
        /// <example>
        /// In the below code example, caret icon is rendered as Breadcrumb items separator using <c>SeparatorTemplate</c>.
        /// <code><![CDATA[
        /// <SfBreadcrumb>
        ///     <BreadcrumbItems>
        ///         <BreadcrumbItem Text="Home"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Components"></BreadcrumbItem>
        ///         <BreadcrumbItem Text="Navigations"></BreadcrumbItem>
        ///     </BreadcrumbItems>
        ///     <BreadcrumbTemplates>
        ///         <SeparatorTemplate>
        ///             <span class="e-icons e-arrow"></span>
        ///         </SeparatorTemplate>
        ///     </BreadcrumbTemplates>
        /// </SfBreadcrumb>
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment<(BreadcrumbItem PreviousItem, BreadcrumbItem NextItem)> SeparatorTemplate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            parent.UpdateChildProperties("BreadcrumbTemplates", this);
        }

        internal override void ComponentDispose()
        {
            parent = null;
        }
    }
}

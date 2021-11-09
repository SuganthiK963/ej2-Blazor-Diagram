using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Represents a collection of <see cref="BreadcrumbItem"/>.
    /// </summary>
    /// <remarks>
    /// To generate dynamic <see cref="BreadcrumbItem"/> based on collection, use <c>@foreach</c> within <see cref="BreadcrumbItems"/> tag directive.
    /// </remarks>
    /// <example>
    /// In the below code example, a basic Breadcrumb has been rendered using <see cref="BreadcrumbItems"/> tag directive.
    /// <code><![CDATA[
    /// <SfBreadcrumb>
    ///     <BreadcrumbItems>
    ///         <BreadcrumbItem Text="Home"></BreadcrumbItem>
    ///         <BreadcrumbItem Text="Components"></BreadcrumbItem>
    ///         <BreadcrumbItem Text="Navigations"></BreadcrumbItem>
    ///     </BreadcrumbItems>
    /// </SfBreadcrumb>
    /// ]]></code>
    /// </example>
    public partial class BreadcrumbItems : SfBaseComponent
    {
        private List<BreadcrumbItem> items;

        [CascadingParameter]
        private SfBreadcrumb parent { get; set; }

        /// <exclude/>
        /// <summary>
        /// Specifies the child content for the Breadcrumb items.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            items = new List<BreadcrumbItem>();
            parent.UpdateChildProperties("Items", items);
        }

        internal void UpdateChildProperty(BreadcrumbItem item, bool isRemove)
        {
            if (isRemove)
            {
                if (items.Contains(item))
                {
                    items.Remove(item);
                    if (parent != null)
                    {
                        SfBaseUtils.UpdateDictionary("Items", items, parent.PropertyChanges);
                    }
                }
            }
            else
            {
                items.Add(item);
                parent.UpdateChildProperties("Items", items);
            }
        }

        internal override void ComponentDispose()
        {
            parent = null;
            ChildContent = null;
        }
    }
}

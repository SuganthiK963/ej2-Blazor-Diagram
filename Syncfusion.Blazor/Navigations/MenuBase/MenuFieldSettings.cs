using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Configures the field options of the Menu.
    /// </summary>
    public class MenuFieldSettings : SfBaseComponent
    {
        [CascadingParameter]
        private IMenu Parent { get; set; }

        /// <summary>
        /// Specifies the children field for Menu item.
        /// </summary>
        [Parameter]
        public string Children { get; set; } = "Items";

        /// <summary>
        /// Specifies the CSS icon field for Menu item.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; } = "IconCss";

        /// <summary>
        /// Specifies the itemId field for Menu item.
        /// </summary>
        [Parameter]
        public string ItemId { get; set; } = "Id";

        /// <summary>
        /// Specifies the parentId field for Menu item.
        /// </summary>
        [Parameter]
        public string ParentId { get; set; } = "ParentId";

        /// <summary>
        /// Specifies the separator field for Menu item.
        /// </summary>
        [Parameter]
        public string Separator { get; set; } = "Separator";

        /// <summary>
        /// Specifies the disabled field for Menu item.
        /// </summary>
        [Parameter]
        public string Disabled { get; set; } = "Disabled";

        /// <summary>
        /// Specifies the hidden field for Menu item.
        /// </summary>
        [Parameter]
        public string Hidden { get; set; } = "Hidden";

        /// <summary>
        /// Specifies the text field for Menu item.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = "Text";

        /// <summary>
        /// Specifies the URL field for Menu item.
        /// </summary>
        [Parameter]
        public string Url { get; set; } = "Url";

        /// <summary>
        /// Specifies the @attributes (additional attributes) field for Menu item.
        /// </summary>
        [Parameter]
        public string HtmlAttributes { get; set; } = "HtmlAttributes";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Fields", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Used to specify templates for rendering menu items.
    /// </summary>
    public class MenuTemplates<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        private IMenu Parent { get; set; }

        /// <summary>
        /// Specifies the template for Menu.
        /// </summary>
        [Parameter]
        public RenderFragment<TValue> Template { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("MenuTemplates", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
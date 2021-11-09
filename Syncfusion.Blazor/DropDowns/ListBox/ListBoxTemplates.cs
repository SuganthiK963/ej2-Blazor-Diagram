using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns.Internal;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Used to specify custom templates for rendering list in ListBox.
    /// </summary>
    public class ListBoxTemplates<TItem> : SfBaseComponent
    {
        [CascadingParameter]
        private IListBox Parent { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to each list item present in the listbox.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to listbox, when no data is available on the component.
        /// </summary>
        [Parameter]
        public RenderFragment NoRecordsTemplate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (ItemTemplate != null)
            {
                Parent.UpdateChildProperties(nameof(ItemTemplate), ItemTemplate);
            }

            if (NoRecordsTemplate != null)
            {
                Parent.UpdateChildProperties(nameof(NoRecordsTemplate), NoRecordsTemplate);
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
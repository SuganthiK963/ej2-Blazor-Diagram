using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A list of items that are used to configure the Accordion component.
    /// </summary>
    public partial class AccordionItems : SfBaseComponent
    {
        [CascadingParameter]
        private SfAccordion Parent { get; set; }

        /// <summary>
        /// Child Content for the Accordion items.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the collection of items for rendering Accordion items.
        /// </summary>
        internal List<AccordionItem> Items { get; set; } = new List<AccordionItem>();

        internal void UpdateChildProperty(AccordionItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateItemProperties(Items);
        }

        internal override void ComponentDispose()
        {
            Items = null;
            Parent = null;
            ChildContent = null;
        }
    }
}
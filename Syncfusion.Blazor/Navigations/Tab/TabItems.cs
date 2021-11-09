using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A list of items that are used to configure the Tabs component.
    /// </summary>
    public partial class TabItems : SfBaseComponent
    {
        [CascadingParameter]
        internal SfTab Parent { get; set; }

        /// <summary>
        /// Child Content for the Tab items.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// A list of items that are used to configure the tab item.
        /// </summary>
        public List<TabItem> Items { get; set; } = new List<TabItem>();

        internal void UpdateChildProperty(TabItem item)
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
            Parent = null;
            ChildContent = null;
        }
    }
}
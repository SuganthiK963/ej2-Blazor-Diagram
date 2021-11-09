using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Defines accordion panels.
    /// </summary>
    public partial class AccordionItem : SfBaseComponent
    {
        [CascadingParameter]
        internal AccordionItems ItemParent { get; set; }

        [CascadingParameter]
        internal SfAccordion BaseParent { get; set; }

        /// <summary>
        /// Child Content for the Accordion item.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets the header to be displayed for the Accordion item.
        /// </summary>
        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Sets the content to be displayed for the Accordion item.
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Sets the text content to be displayed for the Accordion item.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Defines the single/multiple classes (separated by a space) that should be used for Accordion item customization.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the accordion item is disabled or not.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Specify a Boolean value that indicates whether the accordion panel is expanded or not.
        /// </summary>
        [Parameter]
        public bool Expanded { get; set; }

        internal bool IsExpanded { get; set; }

        /// <summary>
        /// Sets the header text to be displayed for the Accordion item.
        /// </summary>
        [Parameter]
        public string Header { get; set; }

        /// <summary>
        /// Defines an icon with the given custom CSS class that is to be rendered before the header text.
        /// Add the css classes to the `IconCss` property and write the css styles to the defined class to set the images/icons.
        /// Adding icon is applicable only to the header.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies a Boolean value that indicates whether the accordion item is visible or not.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Sets Id attribute for accordion item.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a callback of the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<bool> ExpandedChanged { get; set; }

        internal bool IsExpandedFromIndex { get; set; }

        internal bool IsContentRendered { get; set; }

        internal int InsertAt { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ItemParent.UpdateChildProperty(this);
            SetItems();
        }

        internal void VisibleItem(bool isVisible)
        {
            Visible = isVisible;
        }

        internal async Task UpdateExpandedValue(bool isExpanded)
        {
            Expanded = IsExpanded = await SfBaseUtils.UpdateProperty(isExpanded, IsExpanded, ExpandedChanged);
        }

        private void SetItems()
        {
            if (!BaseParent.LoadOnDemand)
            {
                IsContentRendered = true;
            }

            if (BaseParent.ExpandedIndices != null && BaseParent.ExpandedIndices.Contains(ItemParent.Items.Count - 1))
            {
                IsExpandedFromIndex = true;
                IsContentRendered = true;
            }

            if ((Expanded || IsExpandedFromIndex) && (!string.IsNullOrEmpty(Content) || ContentTemplate != null))
            {
                BaseParent.ExpandedItem.Add(this);
            }
        }

        internal override void ComponentDispose()
        {
            if (ItemParent != null && ItemParent.Items != null && ItemParent.Items.Contains(this))
            {
                ItemParent.Items.Remove(this);
                SfBaseUtils.UpdateDictionary(nameof(BaseParent.Items), ItemParent.Items, BaseParent.PropertyChanges);
                BaseParent.IsItemChanged = true;
            }

            ItemParent = null;
            BaseParent = null;
            ChildContent = null;
        }
    }
}
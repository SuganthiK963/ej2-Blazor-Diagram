using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns.Internal
{
    /// <summary>
    /// The DropDowns has been provided with several options to customize each list item, group title, header, and footer elements.
    /// </summary>
    /// <typeparam name="TItem">Specifies the type of DropDownsTemplates.</typeparam>
    public partial class DropDownsTemplates<TItem> : SfBaseComponent
    {
        [CascadingParameter]
        internal SfDropDownBase<TItem> Parent { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the footer container of the popup list.
        /// </summary>
        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the header container of the popup list.
        /// </summary>
        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the group headers present in the DropDowns popup list.
        /// </summary>
        [Parameter]
        public RenderFragment<ComposedItemModel<TItem>> GroupTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to each list item present in the popup.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to popup list of DropDowns component, when no data is available on the component.
        /// </summary>
        [Parameter]
        public RenderFragment NoRecordsTemplate { get; set; }

        /// <summary>
        /// Accepts the template and assigns it to the popup list content of the DropDowns component, when the data fetch request from the remote server fails.
        /// </summary>
        [Parameter]
        public RenderFragment ActionFailureTemplate { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <exclude/>
        protected override void OnInitialized()
        {
            if (FooterTemplate != null)
            {
                Parent.UpdateDropDownTemplate(nameof(FooterTemplate), FooterTemplate);
            }

            if (HeaderTemplate != null)
            {
                Parent.UpdateDropDownTemplate(nameof(HeaderTemplate), HeaderTemplate);
            }

            if (GroupTemplate != null)
            {
                Parent.UpdateDropDownTemplate(nameof(GroupTemplate), null, null, GroupTemplate);
            }

            if (ItemTemplate != null)
            {
                Parent.UpdateDropDownTemplate(nameof(ItemTemplate), null, ItemTemplate);
            }

            if (NoRecordsTemplate != null)
            {
                Parent.UpdateDropDownTemplate(nameof(NoRecordsTemplate), NoRecordsTemplate);
            }

            if (ActionFailureTemplate != null)
            {
                Parent.UpdateDropDownTemplate(nameof(ActionFailureTemplate), ActionFailureTemplate);
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}

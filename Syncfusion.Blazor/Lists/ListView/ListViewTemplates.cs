using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// Configure templates of the ListView component.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public class ListViewTemplates<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        internal SfListView<TValue> SfListView { get; set; }

        /// <summary>
        /// The ListView component supports to customize the content of each list items with the help of Template property.
        /// </summary>
        [Parameter]
        public RenderFragment<TValue> Template { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the group headers present in the ListView.
        /// </summary>
        [Parameter]
        public RenderFragment<ComposedItemModel<TValue>> GroupTemplate { get; set; }

        /// <summary>
        /// The ListView has an option to custom design the ListView header title with the help of HeaderTemplate property.
        /// </summary>
        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            if (Template != null)
            {
                SfListView.UpdateTemplate(nameof(Template), Template);
            }

            if (GroupTemplate != null)
            {
                SfListView.UpdateTemplate(nameof(GroupTemplate), GroupTemplate);
            }

            if (HeaderTemplate != null)
            {
                SfListView.UpdateTemplate(nameof(HeaderTemplate), HeaderTemplate);
            }
        }
    }
}

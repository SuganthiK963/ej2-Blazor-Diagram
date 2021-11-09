using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns.Internal;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The Fields property maps the columns of the data table and binds the data to the component.
    /// </summary>
    public class ListBoxFieldSettings : SfBaseComponent
    {
        [CascadingParameter]
        private IListBox Parent { get; set; }

        /// <summary>
        /// Group the list items with it's related items by mapping groupBy field.
        /// </summary>
        [Parameter]
        public string GroupBy { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as styles, class, and more to the list element.
        /// If you configured both property and equivalent html attributes, then the component considers the property value.
        /// </summary>
        [Parameter]
        public string HtmlAttributes { get; set; }

        /// <summary>
        /// Maps the icon class column from data table for each list item.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Maps the text column from data table for each list item.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Maps the value column from data table for each list item.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

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
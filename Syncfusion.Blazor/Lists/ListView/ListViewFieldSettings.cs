using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// Configure handlers to handle the field settings with the ListView component.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public class ListViewFieldSettings<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        internal SfListView<TValue> Parent { get; set; }

        /// <summary>
        /// The `Child` property is used for nested navigation of listed items.
        /// </summary>
        [Parameter]
        public string Child { get; set; }

        /// <summary>
        /// Specifies the enabled state of the ListView component.
        /// You can disable the component using this property by setting its value as false.
        /// </summary>
        [Parameter]
        public string Enabled { get; set; }

        /// <summary>
        /// The `GroupBy` property is used to wraps the ListView elements into a group based on the field value.
        /// </summary>
        [Parameter]
        public string GroupBy { get; set; }

        private string ListFieldGroupBy { get; set; }

        /// <summary>
        /// The `HtmlAttributes` allows additional attributes such as id, class, etc., and
        ///  accepts n number of attributes in a key-value pair format.
        /// </summary>
        [Parameter]
        public string HtmlAttributes { get; set; }

        /// <summary>
        /// The `IconCss` is used to customize the icon fo the list items dynamically.
        ///  You can add a specific image to the icons using the `iconCss` property.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies the id field mapped in data source.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// The `IsChecked` property is used to check whether the list items are in a checked state or not.
        /// </summary>
        [Parameter]
        public string IsChecked { get; set; }

        /// <summary>
        /// The `IsVisible` property is used to check whether the list items are in visible state or not.
        /// </summary>

        internal string ListIsVisible { get; set; }

        /// <summary>
        /// The `Text` property is used to map the text value from the data source for each list item.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// The `Tooltip` is used to display the information about the target element while hovering on list items.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ListFieldGroupBy = GroupBy;
            Parent.UpdateChildProperties("fields", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            ListFieldGroupBy = NotifyPropertyChanges(nameof(GroupBy), GroupBy, ListFieldGroupBy);
            if (PropertyChanges.ContainsKey(nameof(GroupBy))) {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                Parent.optionsInternal.Fields.GroupBy =  Parent.ListFields.GroupBy;
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}

using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The Fields property maps the columns of the data table and binds the data to the component.
    /// </summary>
    public partial class ComboBoxFieldSettings : SfDataBoundComponent
    {
        [CascadingParameter]
        private IDropDowns BaseParent { get; set; }

        /// <summary>
        /// Group the list items with it's related items by mapping groupBy field.
        /// </summary>
        [Parameter]
        public string GroupBy { get; set; }

        private string groupBy { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as styles, class, and more to the list element.
        /// If you configured both property and equivalent html attributes, then the component considers the property value.
        /// </summary>
        [Parameter]
        public string HtmlAttributes { get; set; }

        private string htmlAttributes { get; set; }

        /// <summary>
        /// Maps the icon class column from data table for each list item.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        private string iconCss { get; set; }

        /// <summary>
        /// Maps the text column from data table for each list item.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        private string fieldText { get; set; }

        /// <summary>
        /// Maps the value column from data table for each list item.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        private string fieldValue { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent?.UpdateChildProperties(this);
            await BaseParent?.CallStateHasChangedAsync();
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <exclude/>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            PropertyParametersSet();
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                PropertyChanges.Clear();
                BaseParent?.UpdateChildProperties(this);
            }
        }

        private void PropertyParametersSet()
        {
            fieldText = NotifyPropertyChanges(nameof(Text), Text, fieldText);
            fieldValue = NotifyPropertyChanges(nameof(Value), Value, fieldValue);
            groupBy = NotifyPropertyChanges(nameof(GroupBy), GroupBy, groupBy);
            iconCss = NotifyPropertyChanges(nameof(IconCss), iconCss, IconCss);
            htmlAttributes = NotifyPropertyChanges(nameof(HtmlAttributes), HtmlAttributes, htmlAttributes);
        }
    }
}

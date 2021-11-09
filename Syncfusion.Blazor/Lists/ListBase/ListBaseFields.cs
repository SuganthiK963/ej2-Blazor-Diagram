using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Lists.Internal
{
    /// <summary>
    /// List base fields class.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public partial class ListBaseFields<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Specifies that fields that mapped in DataSource.
        /// </summary>
        [CascadingParameter]
        public SfListBase<TValue> ListBase { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in Child.
        /// </summary>
        [Parameter]
        public string Child { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in Enabled.
        /// </summary>
        [Parameter]
        public string Enabled { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in GroupBy.
        /// </summary>
        [Parameter]
        public string GroupBy { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in HtmlAttributes.
        /// </summary>
        [Parameter]
        public string HtmlAttributes { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in IconCss.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in Id.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in IsChecked.
        /// </summary>
        [Parameter]
        public string IsChecked { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in IsVisible.
        /// </summary>
        [Parameter]
        public string IsVisible { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in Text.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in Tooltip.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in Value.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        DefaultListBaseOptions<TValue> defaultListBaseOptions = new DefaultListBaseOptions<TValue>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            ListBaseFields<TValue> fields = defaultListBaseOptions.GetDefaultFieldsMapping();
            // Updating the list base properties and fields with user given values
            fields.Child = Child ?? fields.Child;
            fields.Enabled = Enabled ?? fields.Enabled;
            fields.GroupBy = GroupBy ?? fields.GroupBy;
            fields.HtmlAttributes = HtmlAttributes ?? fields.HtmlAttributes;
            fields.IconCss = IconCss ?? fields.IconCss;
            fields.Id = Id ?? fields.Id;
            fields.IsChecked = IsChecked ?? fields.IsChecked;
            fields.IsVisible = IsVisible ?? fields.IsVisible;
            fields.Text = Text ?? fields.Text;
            fields.Tooltip = Tooltip ?? fields.Tooltip;
            fields.Value = Value ?? fields.Value;
            ListBase.ListBaseOptionModel.Fields = fields;
        }

        /// <summary>
        /// The virtual method to override the Dispose method at component side.
        /// </summary>
        internal override void ComponentDispose()
        {
            ListBase = null;
            Child = null;
            Enabled = null;
            GroupBy = null;
            HtmlAttributes = null;
            IconCss = null;
            Id = null;
            IsChecked = null;
            IsVisible = null;
            Text = null;
            Tooltip = null;
            Value = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The MultiSelect component contains a list of predefined values from which a multiple value can be chosen.
    /// </summary>
    public partial class SfMultiSelect<TValue, TItem> : SfDropDownBase<TItem>
    {
        /// <summary>
        /// Specifies the id of the MultiSelect component.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the expression for defining the value of the bound.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }

        /// <summary>
        /// Allows user to add a
        /// Custom value the value which is not present in the suggestion list.
        /// </summary>
        [Parameter]
        public bool AllowCustomValue { get; set; }

        /// <summary>
        /// The Virtual Scrolling feature is used to display a large amount of data that you require without buffering the entire load of a huge database records in the DropDowns, that is, when scrolling, the datamanager request is sent to fetch some amount of data from the server dynamically.
        /// To achieve this scenario with DropDowns, set the EnableVirtualization to true.
        /// </summary>
        [Parameter]
        public bool EnableVirtualization { get; set; }

        /// <summary>
        /// The data can be fetched in popup based on ItemsCount, when enabled the EnableVirtualization.
        /// </summary>
        [Parameter]
        public int ItemsCount { get; set; } = 5;

        /// <summary>
        /// <para>To enable the filtering option in this component.</para>
        /// <para>Filter action performs when type in search box and collect the matched item through `Filtering` event.</para>
        /// <para>If searching character does not match, `NoRecordsTemplate` property value will be shown.</para>
        /// </summary>
        [Parameter]
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// <para>By default, the MultiSelect component fires the Change event while focus out the component.</para>
        /// <para>If you want to fires the Change event on every value selection and remove, then disable the EnabledChangeOnBlur property.</para>
        /// </summary>
        [Obsolete("This ChangeOnBlur property is deprecated. Use EnableChangeOnBlur property to achieve the functionality.")]
        [Parameter]
        public bool ChangeOnBlur { get { return EnabledChangeOnBlur; } set { EnabledChangeOnBlur = value; } }
        private bool EnabledChangeOnBlur { get; set; } = true;
        /// <summary>
        /// <para>By default, the MultiSelect component fires the Change event while focus out the component.</para>
        /// <para>If you want to fires the Change event on every value selection and remove, then disable the EnabledChangeOnBlur property.</para>
        /// </summary>
        [Parameter]
        public bool EnableChangeOnBlur { get; set; } = true;

        /// <summary>
        /// Based on the property, when item get select popup visibility state will changed.
        /// </summary>
        [Obsolete("This ClosePopupOnSelect property is deprecated. Use EnableCloseOnSelect property to achieve the functionality.")]
        [Parameter]
        public bool ClosePopupOnSelect { get { return ClosePopupOnSelectItem; } set { ClosePopupOnSelectItem = value; } }

        private bool ClosePopupOnSelectItem { get; set; } = true;

        /// <summary>
        /// Based on the property, when item get select popup visibility state will changed.
        /// </summary>
        [Parameter]
        public bool EnableCloseOnSelect { get; set; } = true;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the MultiSelect. One or more custom CSS classes can be added to a MultiSelect.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        private string cssClass { get; set; }

        /// <summary>
        /// Sets the delimiter character for 'default' and 'delimiter' visibility modes.
        /// </summary>
        [Parameter]
        public string DelimiterChar { get; set; } = ",";

        /// <summary>
        /// <para>Specifies a boolean value that indicates the whether the grouped list items are
        /// allowed to check by checking the group header in checkbox mode.</para>
        /// <para>By default, there is no checkbox provided for group headers.</para>
        /// <para>This property allows you to render checkbox for group headers and to select
        /// all the grouped items at once.</para>
        /// </summary>
        [Parameter]
        public bool EnableGroupCheckBox { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Reorder the selected items in popup visibility state.
        /// </summary>
        [Parameter]
        public bool EnableSelectionOrder { get; set; } = true;

        /// <summary>
        /// Accepts the value to be displayed as a watermark text on the filter bar.
        /// </summary>
        [Parameter]
        public string FilterBarPlaceholder { get; set; }

        /// <summary>
        /// Specifies the floating label behavior of the MultiSelect that the placeholder text floats above the MultiSelect based on the following values.
        /// <para>Possible values are: </para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the MultiSelect when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the MultiSelect.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the MultiSelect after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; }

        private Syncfusion.Blazor.Inputs.FloatLabelType floatLabelType { get; set; }

        /// <summary>
        /// Hides the selected item from the list item.
        /// </summary>
        [Parameter]
        public bool HideSelectedItem { get; set; } = true;
        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the component considers the property value.</para>
        /// </summary>
        [Parameter]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the component considers the property value.</para>
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// <para>Sets limitation to the value selection.</para>
        /// <para>Based on the limitation, list selection will be prevented.</para>
        /// </summary>
        [Parameter]
        public int MaximumSelectionLength { get; set; } = 1000;

        /// <summary>
        /// configures visibility mode for component interaction.
        /// </summary>
        [Parameter]
        public VisualMode Mode { get; set; }

        private VisualMode mode { get; set; }

        /// <summary>
        /// Whether to automatically open the popup when the control is clicked.
        /// </summary>
        [Parameter]
        public bool OpenOnClick { get; set; } = true;

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in MultiSelect. The property is depending on the FloatLabelType property.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the height of the popup list. By default, it renders based on its list item.
        /// </summary>
        [Parameter]
        public string PopupHeight { get; set; } = "300px";

        /// <summary>
        /// Gets or sets the width of the popup list and percentage values has calculated based on input width.
        /// </summary>
        [Parameter]
        public string PopupWidth { get; set; } = "100%";

        /// <summary>
        /// Specifies the boolean value whether the MultiSelect allows the user to change the value.
        /// </summary>
        [Parameter]
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies the selectAllText to be displayed on the component.
        /// </summary>
        [Parameter]
        public string SelectAllText { get; set; } = "Select All";

        /// <summary>
        /// Enables close icon with the each selected item.
        /// </summary>
        [Parameter]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Allows you to either show or hide the DropDown button on the component.
        /// </summary>
        [Parameter]
        public bool ShowDropDownIcon { get; set; }

        /// <summary>
        /// Allows you to either show or hide the selectAll option on the component.
        /// </summary>
        [Parameter]
        public bool ShowSelectAll { get; set; }

        /// <summary>
        /// Selects the list item which maps the data `Text` field in the component.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Specifies the UnSelectAllText to be displayed on the component.
        /// </summary>
        [Parameter]
        public string UnSelectAllText { get; set; } = "UnSelect All";

        /// <summary>
        /// Selects the list item which maps the data `Value` field in the component.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }

        private TValue multiValue { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the selected list item in the input element of the component.
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ValueTemplate { get; set; }

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
        /// Gets or sets the width of the component. By default, it sizes based on its parent.
        /// container dimension.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";
        /// <summary>
        /// Specifies the tab order of the DropDownList component.
        /// </summary>
        [Parameter]
        public int TabIndex { get; set; }

        /// <summary>
        /// Triggers when the content of input has changed and gets focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        /// <summary>
        /// Triggers when the content of input has changed and gets focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }

        /// <summary>
        /// Triggers each time when the value of input has changed.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Parent component of MultiSelect.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic MultiSelectParent { get; set; }
    }
}
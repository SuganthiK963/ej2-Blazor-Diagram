using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The DropDownList component contains a list of predefined values from which a single value can be chosen.
    /// </summary>
    public partial class SfDropDownList<TValue, TItem> : SfDropDownBase<TItem>
    {
        /// <summary>
        /// Specifies the id of the DropDownList component.
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
        /// <para>When AllowFiltering is set to true, show the filter bar (search box) of the component.</para>
        /// <para>The filter action retrieves matched items through the `Filtering` event based on the characters typed in the search TextBox.</para>
        /// </summary>
        [Parameter]
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the DropDownList. One or more custom CSS classes can be added to a DropDownList.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        private string cssClass { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Accepts the value to be displayed as a watermark text on the filter bar.
        /// </summary>
        [Parameter]
        public string FilterBarPlaceholder { get; set; }

        /// <summary>
        /// Specifies the floating label behavior of the DropDownList that the placeholder text floats above the DropDownList based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DropDownList when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DropDownList.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DropDownList after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; }

        private Syncfusion.Blazor.Inputs.FloatLabelType floatLabelType { get; set; }

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
        /// You can add the additional html attributes such as styles, class, and more to the root element.
        /// If you configured both property and equivalent html attributes, then the component considers the property value.
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
        /// Gets or sets the index of the selected item in the component.
        /// </summary>
        [Parameter]
        public int? Index { get; set; }

        private int? index { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the index changes.
        /// </summary>
        [Parameter]
        public EventCallback<int?> IndexChanged { get; set; }

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DropDownList. The property is depending on the FloatLabelType property.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// Specifies the height of the popup list.
        /// </summary>
        [Parameter]
        public string PopupHeight { get; set; } = "300px";

        /// <summary>
        /// Specifies the width of the popup list. By default, the popup width sets based on the width of
        /// the component.
        /// </summary>
        [Parameter]
        public string PopupWidth { get; set; } = "100%";

        /// <summary>
        /// Specifies the boolean value whether the DropDownList allows the user to change the value.
        /// </summary>
        [Parameter]
        public bool Readonly { get; set; }

        /// <summary>
        /// <para>Specifies whether to show or hide the clear button.</para>
        /// <para>When the clear button is clicked, `Value`, `Text`, and `Index` properties are reset to null.</para>
        /// </summary>
        [Parameter]
        public virtual bool ShowClearButton { get; set; }

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

        internal string DDLText { get; set; }

        /// <summary>
        /// Gets or sets the value of the selected item in the component.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }

        private TValue ddlValue { get; set; }

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
        /// <para>Specifies the width of the component. By default, the component width sets based on the width of
        /// its parent container.</para>
        /// <para>You can also set the width in pixel values.</para>
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
        /// Parent component of DropDownList.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic DropDownListParent { get; set; }
    }
}
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs.Internal
{
    /// <summary>
    /// The SfInputBase is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfInputBase : SfBaseComponent
    {
        /// <summary>
        /// Specifies the id of the TextBox component.
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
        /// <exclude/>
        [Parameter]
        public Expression<Func<string>> ValueExpression { get; set; }

        /// <summary>
        /// Specifies the edit context of the Input.
        /// </summary>
        [CascadingParameter]
        protected EditContext InputEditContext { get; set; }

        /// <summary>
        /// Specifies whether the browser is allowed to automatically enter or select a value for the TextBox.
        /// <para>By default, autocomplete is enabled for TextBox.</para>
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>On</term>
        /// <description>Specifies that autocomplete is enabled</description>
        /// </item>
        /// <item>
        /// <term>Off</term>
        /// <description>Specifies that autocomplete is disabled.</description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public string Autocomplete { get; set; } = "on";

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the TextBox. One or more custom CSS classes can be added to a TextBox.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable the persisting TextBox state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in the right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the TextBox allows the user to interact with it.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Specifies the floating label behavior of the TextBox that the placeholder text floats above the TextBox based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the TextBox when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the TextBox.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the TextBox after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public FloatLabelType FloatLabelType { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as styles, class, and more to the root element.
        /// If you configured both property and equivalent html attributes, then the component considers the property value.
        /// </summary>
        [Parameter]

        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// If you configured both property and equivalent input attribute, then the component considers the property value.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Specifies the global culture and localization of the TextBox component.
        /// </summary>
        [Obsolete("The Locale is deprecated and will no longer be used. Hereafter, the Locale works based on the current culture.")]
        [Parameter]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a boolean value that enables or disables the multiline on the TextBox.
        /// The TextBox changes from a single line to multiline when enabling this multiline mode.
        /// </summary>
        [Parameter]
        public bool Multiline { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the TextBox allows user to change the text.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the TextBox allows user to change the text.
        /// </summary>
        [Parameter]
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies the boolean value for dropdownlist default readonly property.
        /// </summary>
        [Parameter]
        public bool IsReadOnlyInput { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the clear button is displayed in TextBox.
        /// </summary>
        [Parameter]
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Specifies the behavior of the TextBox such as text, password, email, and more.
        /// </summary>
        [Parameter]
        public string Type { get; set; } = "text";

        /// <summary>
        /// Sets the content of the TextBox.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        private string _value { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Specifies the width of the TextBox component.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Specifies the tab order of the TextBox component.
        /// </summary>
        [Parameter]
        public int TabIndex { get; set; }

        /// <summary>
        /// Specifies the container attrubute of Input.
        /// </summary>
        [Parameter]
        public Dictionary<string, object> ContainerAttr { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Specifies the icon of the TextBox component.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public List<string> Buttons { get; set; }

        /// <summary>
        /// Specifies the prepend icon of the TextBox component.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public List<string> PrependButtons { get; set; }

        /// <summary>
        /// Specifies a boolean value that enable or disable the spin button on the component.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool SpinButton { get; set; }

        /// <summary>
        /// Specifies the class value that is appended to container of TextBox.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public string ContainerClass { get; set; } = string.Empty;

        /// <summary>
        /// Accepts the template design and assigns it to the selected list item in the input element of the component.
        /// </summary>
        /// <exclude/>
        public RenderFragment ValueTemplate { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the value template is displayed in TextBox.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsValueTemplate { get; set; }

        /// <summary>
        /// Gets or Set the component class to element.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public string RootClass { get; set; } = "e-control e-textbox e-lib";

        /// <summary>
        /// Specifies the prevents the click actions.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool ClickStopPropagation { get; set; }

        /// <summary>
        /// Specifies the prevents the mouse actions.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool MouseDowSpinnerPrevent { get; set; }

        /// <summary>
        /// Specifies the prevents the icon actions.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool PreventIconHandler { get; set; }

        /// <summary>
        /// Specifies the prevents the container actions.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool MousePreventContainer { get; set; }

        /// <summary>
        /// Specifies the spinner show on clear icon.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsShowSpinnerOnClear { get; set; }

        /// <summary>
        /// Specifies the spinner show on dropdown icon.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsShowSpinnerOnIcon { get; set; }
    }
}
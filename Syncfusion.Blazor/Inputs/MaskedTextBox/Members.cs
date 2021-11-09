using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The MaskedTextBox is an input element that allows to get input from the user.
    /// </summary>
    public partial class SfMaskedTextBox : SfBaseComponent
    {
        /// <summary>
        /// Specifies the id of the SfMaskedTextBox component.
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
        /// Specifies the edit context of MaskedTextBox component.
        /// </summary>
        [CascadingParameter]
        protected EditContext SfMaskedTextBoxEditContext { get; set; }

        /// <summary>
        /// Specifies the expression for defining the value of the bound.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public Expression<Func<string>> ValueExpression { get; set; }

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the SfMaskedTextBox. One or more custom CSS classes can be added to a SfMaskedTextBox.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        private string cssClass { get; set; }

        /// <summary>
        /// Enable or disable the persisting SfMaskedTextBox state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in the right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the SfMaskedTextBox allows the user to interact with it.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        private bool enabled { get; set; }

        /// <summary>
        /// Specifies the floating label behavior of the SfMaskedTextBox that the placeholder text floats above the SfMaskedTextBox based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the SfMaskedTextBox when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the SfMaskedTextBox.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the SfMaskedTextBox after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public FloatLabelType FloatLabelType { get; set; }

        private FloatLabelType floatLabelType { get; set; }

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
        /// Specifies the global culture and localization of the SfMaskedTextBox component.
        /// </summary>
        [Obsolete("The Locale is deprecated and will no longer be used. Hereafter, the Locale works based on the current culture.")]
        [Parameter]
        public string Locale { get; set; } = string.Empty;
        internal string MaskLocale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in SfMaskedTextBox. The property is depending on the FloatLabelType property.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the SfMaskedTextBox allows user to change the text.
        /// </summary>
        [Parameter]
        public bool Readonly { get; set; }

        private bool readOnly { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the clear button is displayed in SfMaskedTextBox.
        /// </summary>
        [Parameter]
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Sets the content of the SfMaskedTextBox.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        private string value { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Specifies the width of the SfMaskedTextBox component.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Specifies the tab order of the SfMaskedTextBox component.
        /// </summary>
        [Parameter]
        public int TabIndex { get; set; }

        /// <summary>
        /// Sets a value that masks the MaskedTextBox to allow/validate the user input.
        /// </summary>
        [Parameter]
        public string Mask { get; set; }

        private string mask { get; set; }

        /// <summary>
        /// Gets or sets a value that will be shown as a prompting symbol for the masked value.
        /// The symbol used to show input positions in the MaskedTextBox.
        /// </summary>
        [Parameter]
        public char PromptChar { get; set; } = '_';

        private char promptChar { get; set; }

        /// <summary>
        /// Sets the collection of values to be mapped for non-mask elements(literals)
        /// which have been set in the mask of MaskedTextBox.
        ///  <para>In the below example, non-mask elements "P" accepts values
        /// "P" , "A" , "p" , "a" and "M" accepts values "M", "m" mentioned in the custom characters collection.</para>
        /// </summary>
        [Parameter]
        public Dictionary<string, string> CustomCharacters { get; set; }

        private Dictionary<string, string> customCharacters { get; set; }

        /// <summary>
        /// Parent component of Numeric TextBox.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic MaskedTextBoxParent { get; set; }
    }
}
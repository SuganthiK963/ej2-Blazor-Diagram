using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Inputs;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The TextBox is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfTextBox : SfInputTextBase<string>
    {
        private string _cssClass { get; set; } = string.Empty; 

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
        public AutoComplete Autocomplete { get; set; }
        private AutoComplete _autocomplete { get; set; }
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
        public FloatLabelType FloatLabelType
        { 
            get { return BaseFloatLabelType; } 
            set { BaseFloatLabelType = value; }
        }
        /// <summary>
        /// Specifies the boolean value whether the TextBox allows user to change the text.
        /// </summary>
        [Parameter]
        public string Placeholder
        {
            get { return BasePlaceholder; } 
            set { BasePlaceholder = value; } 
        }
        protected override string BasePlaceholder { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the TextBox allows user to change the text.
        /// </summary>
        [Parameter]
        public bool Readonly
        {
            get { return BaseReadonly; }
            set { BaseReadonly = value; }
        }
        protected override bool BaseReadonly { get; set; }
        protected override bool BaseIsReadOnlyInput { get; set; }
        /// <summary>
        /// Specifies a boolean value that indicates whether the clear button is displayed in TextBox.
        /// </summary>
        [Parameter]
        public bool ShowClearButton
        {
            get { return BaseShowClearButton; }
            set { BaseShowClearButton = value; }
        }
        protected override bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Specifies the width of the TextBox component.
        /// </summary>
        [Parameter]
        public string Width
        {
            get { return BaseWidth; }
            set { BaseWidth = value; }
        }
        protected override string BaseWidth { get; set; }

        /// <summary>
        /// Specifies the tab order of the TextBox component.
        /// </summary>
        [Parameter]
        public int TabIndex
        {
            get { return BaseTabIndex; }
            set { BaseTabIndex = value; }
        }
        protected override int BaseTabIndex { get; set; }
        protected override FloatLabelType BaseFloatLabelType { get; set; }
        /// <summary>
        /// Specifies a boolean value that enables or disables the multiline on the TextBox.
        /// The TextBox changes from a single line to multiline when enabling this multiline mode.
        /// </summary>
        [Parameter]
        public bool Multiline 
        {
            get { return MultilineInput; } 
            set { MultilineInput = value; }
        }
        protected override bool MultilineInput { get; set; }
        /// <summary>
        /// You can add the additional html attributes such as styles, class, and more to the root element.
        /// If you configured both property and equivalent html attributes, then the component considers the property value.
        /// </summary>
        [Parameter]

        public Dictionary<string, object> HtmlAttributes
        {
            get { return BaseHtmlAttributes; } 
            set { BaseHtmlAttributes = value; } 
        }

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// If you configured both property and equivalent input attribute, then the component considers the property value.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes
        {
            get { return BaseInputAttributes; }
            set { BaseInputAttributes = value; } 
        }
        protected override Dictionary<string, object> BaseHtmlAttributes { get; set; }

        protected override Dictionary<string, object> BaseInputAttributes { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// Specifies the behavior of the TextBox such as text, password, email, and more.
        /// </summary>
        [Parameter]
        public InputType Type { get; set; }
        private InputType _type { get; set; }

        /// <summary>
        /// Parent component of TextBox.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic TextBoxParent { get; set; } 
         
    }
}
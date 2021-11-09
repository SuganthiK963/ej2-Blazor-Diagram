using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Inputs;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The NumericTextBox is used to get the number inputs from the user. The input values can be incremented or decremented by a predefined step value.
    /// </summary>
    public partial class SfNumericTextBox<TValue> : SfInputTextBase<TValue>
    {
        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private string _cssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the currency code to use in currency formatting. Possible values are the ISO 4217 currency codes, such as 'USD' for the US dollar and 'EUR' for the euro.
        /// </summary>
        [Parameter]
        public string Currency { get; set; }
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
        protected override FloatLabelType BaseFloatLabelType { get; set; }
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
        /// Specifies the number precision applied to the textbox value when the NumericTextBox is focused.
        /// </summary>
        [Parameter]
        public int? Decimals { get; set; }

        private int? _decimals { get; set; } 

        private bool _enabled { get; set; }

         

        /// <summary>
        /// Specifies the number format that indicates the display format for the value of the NumericTextBox.
        /// </summary>
        [Parameter]
        public string Format { get; set; } = "n2";

        private string format { get; set; } 

        internal string NumericLocale { get; set; } = string.Empty; 

        private bool _readonly { get; set; } 

        /// <summary>
        /// Specifies whether the up and down spin buttons will be displayed in NumericTextBox.
        /// </summary>
        [Parameter]
        public bool ShowSpinButton {
            get { return SpinButton; }
            set { SpinButton = value; }
        }

        private bool _showSpinButton { get; set; }

        /// <summary>
        /// Specifies the incremental or decremental step size for the NumericTextBox.
        /// </summary>
        [Parameter]
        public TValue Step { get; set; } = SfInputTextBase<TValue>.GetNumericValue<TValue>(STEP);

        private TValue _step { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the NumericTextBox component allows the value for the specified range.
        /// <para>True - the input value will be restricted between the min and max range. The typed value gets modified to fit the range on a focused out state.</para>
        /// <para>Else, it allows any value even out of range value, at that time of wrong value entered, the error class will be added to the component to highlight the error.</para>
        /// </summary>
        [Parameter]
        public bool StrictMode { get; set; } = true;

        /// <summary>
        /// Specifies whether the length of the decimal should be restricted during typing.
        /// </summary>
        [Parameter]
        public bool ValidateDecimalOnType { get; set; }

        private bool _validateDecimalOnType { get; set; }  

        /// <summary>
        /// Specifies a maximum value that is allowed a user can enter.
        /// </summary>
        [Parameter]
        public TValue Max { get; set; } = SfInputTextBase<TValue>.GetNumericValue<TValue>(MAX_Value);

        private TValue _max { get; set; }

        /// <summary>
        /// Specifies a minimum value that is allowed a user can enter.
        /// </summary>
        [Parameter]
        public TValue Min { get; set; } = SfInputTextBase<TValue>.GetNumericValue<TValue>(MIN_VALUE);

        private TValue _min { get; set; }

        /// <summary>
        /// Parent component of DataManager.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic NumericTextBoxParent { get; set; }
    }
}
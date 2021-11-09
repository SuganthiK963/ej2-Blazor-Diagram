using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// TimePicker is an intuitive component which provides an options to select a time value from popup list or to set a desired time value.
    /// </summary>
    public partial class SfTimePicker<TValue> : SfInputTextBase<TValue>
    {
        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        ///// <summary>
        ///// Specifies the expression for defining the value of the bound.
        ///// </summary>
        //[Parameter]
        //public Expression<Func<TValue>> ValueExpression { get; set; }
        /// <summary>
        /// Specifies the edit context of timepicker.
        /// </summary>
        [CascadingParameter]
        protected EditContext TimePickerEditContext { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the TimePicker allows user to change the value via typing. When set as false, the TimePicker allows user to change the value via picker only.
        /// </summary>
        [Parameter]
        public bool AllowEdit { get; set; } = true;

        ///// <summary>
        ///// Specifies the CSS class name that can be appended with the root element of the TimePicker. One or more custom CSS classes can be added to a TimePicker.
        ///// </summary>
        //[Parameter]
        //public string CssClass { get; set; }

        private string _cssClass { get; set; }

        private bool _enableRtl { get; set; }

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
        /// Sets or gets the required time format of value that is to be displayed in component.
        /// <para>By default, the format is based on the culture.</para>
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        private string _format { get; set; }

        /// <summary>
        /// Customizes the key actions in TimePicker. For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        [Parameter]
        public Dictionary<string, object> KeyConfigs { get; set; }

        private Dictionary<string, object> _keyConfigs { get; set; } 

        internal string TimePickerLocale { get; set; }

        /// <summary>
        /// Gets or sets the maximum time value that can be allowed to select in TimePicker.
        /// </summary>
        [Parameter]
        public DateTime Max { get; set; } = new DateTime(2099, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets or sets the minimum time value that can be allowed to select in TimePicker.
        /// </summary>
        [Parameter]
        public DateTime Min { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00);
         

        /// <summary>
        /// Specifies the scroll bar position, if there is no value is selected in the popup list or the given value is not present in the popup list.
        /// </summary>
        [Parameter]
        public DateTime? ScrollTo { get; set; }

        private DateTime? _scrollTo { get; set; } 

        /// <summary>
        /// Specifies the time interval between the two adjacent time values in the popup list.
        /// </summary>
        [Parameter]
        public int Step { get; set; } = 30;

        private int _step { get; set; }

        /// <summary>
        /// Specifies the TimePicker to act as strict. So that, it allows to enter only a valid time value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range time value with highlighted error class.</para>
        /// </summary>
        [Parameter]
        public bool StrictMode { get; set; } 

        private string _width { get; set; }

        /// <summary>
        /// specifies the z-index value of the TimePicker popup element.
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        private int _zIndex { get; set; }

        /// <summary>
        /// Parent component of TimePicker.
        /// </summary>
        /// <exclude />
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic TimePickerParent { get; set; }
    }

    internal class TimePickerClientProps<TValue>
    {
        /// <summary>
        /// Enable or disable rendering TimePicker in right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// specifies the z-index value of the TimePicker popup element.
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// Customizes the key actions in TimePicker. For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        public Dictionary<string, object> KeyConfigs { get; set; }

        /// <summary>
        /// Gets or sets the value of the TimePicker. The value is parsed based on the culture specific time format.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Specifies the width of the TimePicker component.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Specifies the scroll bar position, if there is no value is selected in the popup list or the given value is not present in the popup list.
        /// </summary>
        public DateTime? ScrollTo { get; set; }

        /// <summary>
        /// Specifies the time interval between the two adjacent time values in the popup list.
        /// </summary>
        public int Step { get; set; }
    }
}
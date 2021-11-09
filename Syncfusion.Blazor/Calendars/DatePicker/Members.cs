using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Inputs;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The DatePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    public partial class SfDatePicker<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the DatePicker allows user to change the value via typing. When set as false, the DatePicker allows user to change the value via picker only.
        /// </summary>
        [Parameter]
        public bool AllowEdit { get; set; } = true;

        private string _cssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the floating label behavior of the DatePicker that the placeholder text floats above the DatePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DatePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DatePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DatePicker after focusing it or when enters the value in it.</description>
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
        /// Specifies the format of the value that to be displayed in component.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        private string _format { get; set; }

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

        private string _locale { get; set; }

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DatePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        [Parameter]
        public string Placeholder
        {
            get { return BasePlaceholder; }
            set { BasePlaceholder = value; }
        }
        protected override string BasePlaceholder { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the DatePicker allows the user to change the text.
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
        /// Specifies a boolean value that indicates whether the clear button is displayed in DatePicker.
        /// </summary>
        [Parameter]
        public bool ShowClearButton
        {
            get { return BaseShowClearButton; }
            set { BaseShowClearButton = value; }
        }
        protected override bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Specifies the component to act as strict. So that, it allows to enter only a valid date  value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range date value with highlighted error class.</para>
        /// </summary>
        [Parameter]
        public bool StrictMode { get; set; }

        internal bool DateStrictMode { get; set; }

        /// <summary>
        /// Specifies the width of the DatePicker component.
        /// </summary>
        [Parameter]
        public string Width
        {
            get { return BaseWidth; }
            set { BaseWidth = value; }
        }
        protected override string BaseWidth { get; set; }

        /// <summary>
        /// Specifies the z-index value of the DatePicker popup element.
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Specifies the tab order of the DatePicker component.
        /// </summary>
        [Parameter]
        public int TabIndex
        {
            get { return BaseTabIndex; }
            set { BaseTabIndex = value; }
        }
        protected override int BaseTabIndex { get; set; }

        /// <summary>
        /// Parent component of DatePicker.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic DatePickerParent { get; set; }
    }

    /// <summary>
    /// Specifies the client properties of datepicker.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of DatePickerClientProps.</typeparam>
    public class DatePickerClientProps<TValue>
    {
        /// <summary>
        /// Specifies the component in readonly state. When the Component is readonly it does not allow user input.
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the DatePicker allows the user to interact with it.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Specifies the global culture and localization of the DatePicker component.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the z-index value of the datePicker popup element.
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// Customizes the key actions in Calendar.
        /// For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        public Dictionary<string, object> KeyConfigs { get; set; }

        /// <summary>
        /// Specifies whether to show or hide the clear icon in DatePicker.
        /// </summary>
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets the selected date of the Calendar.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the DatePicker allows user to change the value via typing. When set as false, the DatePicker allows user to change the value via picker only.
        /// </summary>
        public bool AllowEdit { get; set; }

        /// <summary>
        /// Sets the maximum level of view such as month, year, and decade in the Calendar.
        /// <para>Depth view should be smaller than the start view to restrict its view navigation.</para>
        /// </summary>
        public string Depth { get; set; }

        /// <summary>
        /// Sets the width of the input element in the datepicker component.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// True if the date popup is opened.
        /// </summary>
        public bool IsDatePopup { get; set; }
    }
}
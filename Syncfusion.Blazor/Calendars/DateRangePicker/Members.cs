using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;

namespace Syncfusion.Blazor.Calendars
{    
    /// <summary>
    /// The DateRangePicker is a graphical user interface component that allows user to select the date range from the calendar.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of SfDateRangePicker.</typeparam>
    public partial class SfDateRangePicker<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the expression for defining the end date of the bound.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> EndDateExpression { get; set; }

        /// <summary>
        /// Specifies the expression for defining the start date of the bound.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>> StartDateExpression { get; set; }

        /// <summary>
        /// Specifies the expression for defining the value of the bound.
        /// </summary>
        public new Expression<Func<object>> ValueExpression { get; set; }

        /// <summary>
        /// Gets or sets the selected date of the Calendar.
        /// </summary>
        public new object Value { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        public new EventCallback<object> ValueChanged { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the DateRangePicker allows user to change the value via typing. When set as false, the DateRangePicker allows user to change the value via picker only.
        /// </summary>
        [Parameter]
        public bool AllowEdit { get; set; } = true;

        private string cssClass { get; set; }

        /// <summary>
        /// Gets or sets the end date of the date range selection.
        /// </summary>
        [Parameter]
        public TValue EndDate { get; set; }

        private TValue endDate { get; set; }

        /// <summary>
        /// Triggers when end date of the DateRangePicker is changed.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> EndDateChanged { get; set; }

        /// <summary>
        /// Gets or sets the floating label behavior of the DateRangePicker that the placeholder text floats above the DateRangePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DateRangePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DateRangePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DateRangePicker after focusing it or when enters the value in it.</description>
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
        /// Gets or sets the required date format to the start and end date string.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        private string format { get; set; }

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
        ///  Gets or sets the maximum span of days that can be allowed in a date range selection.
        /// </summary>
        [Parameter]
        public int? MaxDays { get; set; }

        private int? maxDays { get; set; }

        /// <summary>
        ///  Gets or sets the minimum span of days that can be allowed in date range selection.
        /// </summary>
        [Parameter]
        public int? MinDays { get; set; }

        private int? minDays { get; set; }

        /// <summary>
        /// Gets or sets the text that is shown as a hint or placeholder until the user focuses or enter a value in DateRangePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        [Parameter]
        public string Placeholder
        {
            get { return BasePlaceholder; }
            set { BasePlaceholder = value; }
        }
        protected override string BasePlaceholder { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the DateRangePicker allows the user to change the text.
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
        /// Sets or gets the string that used between the start and end date string.
        /// </summary>
        [Parameter]
        public string Separator { get; set; } = "-";
        private string separator { get; set; }

        /// <summary>
        /// Specifies whether to show or hide the clear icon in DateRangePicker.
        /// </summary>
        [Parameter]
        public bool ShowClearButton
        {
            get { return BaseShowClearButton; }
            set { BaseShowClearButton = value; }
        }
        protected override bool BaseShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets the start date of the date range selection.
        /// </summary>
        [Parameter]
        public TValue StartDate { get; set; }

        private TValue startDate { get; set; }

        /// <summary>
        /// Parent component of DataManager.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic DateRangePickerParent { get; set; }

        /// <summary>
        /// Triggers when start date of the DateRangePicker is changed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public EventCallback<TValue> StartDateChanged { get; set; }

        /// <summary>
        /// Specifies the DateRangePicker to act as strict. So that, it allows to enter only a valid date value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range date value with highlighted error class.</para>
        /// </summary>
        [Parameter]
        public bool StrictMode { get; set; }

        /// <summary>
        /// Specifies the width of the DateRangePicker component.
        /// </summary>
        [Parameter]
        public string Width
        {
            get { return BaseWidth; }
            set { BaseWidth = value; }
        }
        protected override string BaseWidth { get; set; }

        /// <summary>
        /// specifies the z-index value of the DateRangePicker popup element.
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Specifies the Presets of the dateRangePicker popup element.
        /// </summary>
        [Parameter]
        public List<Presets> Presets { get; set; } = new List<Presets>();
    }

    /// <summary>
    /// Defines the label string of the preset range.
    /// </summary>
    public class Presets
    {
        /// <summary>
        /// Defines the label string of the preset range.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Defines the start date of the preset range.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Defines the end date of the preset range.
        /// </summary>
        public DateTime End { get; set; }
    }

    internal class DateRangePickerClientProps<TValue>
    {
        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the z-index value of the dateRangePicker popup element.
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// Specifies the Presets of the dateRangePicker popup element.
        /// </summary>
        public List<Presets> Presets { get; set; }

        /// <summary>
        /// Specifies the Presets of the dateRangePicker popup element.
        /// </summary>
        public bool IsCustomWindow { get; set; }
    }
}
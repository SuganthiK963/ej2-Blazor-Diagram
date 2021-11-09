using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Interface for change event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of ChangeEventArgs.</typeparam>
    public class ChangeEventArgs<T>
    {
        /// <summary>
        /// Returns the event parameters from NumericTextBox.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Returns true when the value of NumericTextBox is changed by user interaction. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the previously entered value of the NumericTextBox.
        /// </summary>
        public T PreviousValue { get; set; }

        /// <summary>
        /// Returns the entered value of the NumericTextBox.
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// Interface for Blur event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of NumericBlurEventArgs.</typeparam>
    public class NumericBlurEventArgs<T>
    {
        /// <summary>
        /// Returns the NumericTextBox container element.
        /// </summary>
        public ElementReference Container { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        public object Event { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the value of the NumericTextBox.
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// Interface for Focus event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of NumericFocusEventArgs.</typeparam>
    public class NumericFocusEventArgs<T>
    {
        /// <summary>
        /// Returns the NumericTextBox container element.
        /// </summary>
        public ElementReference Container { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        public object Event { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the value of the NumericTextBox.
        /// </summary>
        public T Value { get; set; }
    }

    internal class NumericClientProps
    {
        /// <summary>
        /// Specifies the component is in read-only mode or not.
        /// </summary>
        /// <exclude/>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies the component is in disabled state or not.
        /// </summary>
        /// <exclude/>
        public bool Enabled { get; set; }

        /// <summary>
        /// Specifies the locale property.
        /// </summary>
        /// <exclude/>
        public string Locale { get; set; }

        /// <summary>
        /// Specifies the ValidateDecimalOnType property.
        /// </summary>
        /// <exclude/>
        public bool ValidateDecimalOnType { get; set; }

        /// <summary>
        /// Specifies the Decimals property.
        /// </summary>
        /// <exclude/>
        public int? Decimals { get; set; }

        /// <summary>
        /// Specifies the DecimalSeparator property.
        /// </summary>
        /// <exclude/>
        public string DecimalSeparator { get; set; }
    }

    /// <summary>
    /// Interface for a class NumericTextBox.
    /// </summary>
    /// <typeparam name="T">Specifies the type of NumericTextBoxModel.</typeparam>
    public class NumericTextBoxModel<T>
    {
        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the NumericTextBox. One or more custom CSS classes can be added to a NumericTextBox.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies the currency code to use in currency formatting. Possible values are the ISO 4217 currency codes, such as 'USD' for the US dollar and 'EUR' for the euro.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Specifies the number precision applied to the textbox value when the NumericTextBox is focused.
        /// </summary>
        public int? Decimals { get; set; }

        /// <summary>
        /// Enable or disable persisting NumericTextBox state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the NumericTextBox allows the user to interact with it.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Specifies the floating label behavior of the NumericTextBox that the placeholder text floats above the NumericTextBox based on the below values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the NumericTextBox when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the NumericTextBox.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the NumericTextBox after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        public FloatLabelType FloatLabelType { get; set; }

        /// <summary>
        /// Specifies the number format that indicates the display format for the value of the NumericTextBox.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// <para>You can add the additional html attributes such as styles, class, and more to the root element.</para>
        /// <para>If you configured both property and equivalent html attributes, then the component considers the property value.</para>
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// <para>You can add the additional input attributes such as disabled, value, and more to the root element.</para>
        /// <para>If you configured both property and equivalent input attribute, then the component considers the property value.</para>
        /// </summary>
        public Dictionary<string, object> InputAttributes { get; set; }

        /// <summary>
        /// Specifies the global culture and localization of the NumericTextBox component.
        /// </summary>
        [Obsolete("Locale is deprecated and will no longer be used. Hereafter, the locale works based on the system culture.")]
        public string Locale { get; set; }

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in TextBox. The property is depending on the FloatLabelType property.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the NumericTextBox allows user to change the text.
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the clear button is displayed in NumericTextBox.
        /// </summary>
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Specifies whether the up and down spin buttons will be displayed in NumericTextBox.
        /// </summary>
        public bool ShowSpinButton { get; set; }

        /// <summary>
        /// Specifies the incremental or decremental step size for the NumericTextBox.
        /// </summary>
        public T Step { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the NumericTextBox component allows the value for the specified range.
        /// <para>True- the input value will be restricted between the min and max range. The typed value gets modified to fit the range on a focused out state.</para>
        /// <para>Else, it allows any value even out of range value, at that time of wrong value entered, the error class will be added to the component to highlight the error.</para>
        /// </summary>
        public bool StrictMode { get; set; } = true;

        /// <summary>
        /// Specifies whether the length of the decimal should be restricted during typing.
        /// </summary>
        public bool ValidateDecimalOnType { get; set; }

        /// <summary>
        /// Sets the value of the NumericTextBox.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Specifies the width of the NumericTextBox component.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Specifies the tab order of the NumericTextBox component.
        /// </summary>
        public int TabIndex { get; set; }

        /// <summary>
        /// Specifies a maximum value that is allowed a user can enter.
        /// </summary>
        public T Max { get; set; }

        /// <summary>
        /// Specifies a minimum value that is allowed a user can enter.
        /// </summary>
        public T Min { get; set; }
    }
}
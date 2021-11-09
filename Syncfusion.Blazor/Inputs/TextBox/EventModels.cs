using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Interface for changed event.
    /// </summary>
    public class ChangedEventArgs
    {
        /// <summary>
        /// Returns the TextBox container element.
        /// </summary>
        public ElementReference Container { get; set; }

        /// <summary>
        /// Returns the event parameters from TextBox.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Returns the original event.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// DEPRECATED-Returns the original event.
        /// </summary>
        public bool IsInteraction { get; set; }

        /// <summary>
        /// Returns the previously entered value of the TextBox.
        /// </summary>
        public string PreviousValue { get; set; }

        /// <summary>
        /// Returns the entered value of the TextBox.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Interface for focus event.
    /// </summary>
    public class FocusInEventArgs
    {
        /// <summary>
        /// Returns the TextBox container element.
        /// </summary>
        public ElementReference Container { get; set; }

        /// <summary>
        /// Returns the event parameters from TextBox.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Returns the entered value of the TextBox.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Interface for focus out event.
    /// </summary>
    public class FocusOutEventArgs
    {
        /// <summary>
        /// Returns the TextBox container element.
        /// </summary>
        public ElementReference Container { get; set; }

        /// <summary>
        /// Returns the event parameters from TextBox.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Returns the entered value of the TextBox.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Interface for input event.
    /// </summary>
    public class InputEventArgs
    {
        /// <summary>
        /// Returns the TextBox container element.
        /// </summary>
        public ElementReference Container { get; set; }

        /// <summary>
        /// Returns the event parameters from TextBox.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Returns the previously updated value of the TextBox.
        /// </summary>
        public string PreviousValue { get; set; }

        /// <summary>
        /// Returns the entered value of the TextBox.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Interface for a class TextBox.
    /// </summary>
    public class TextBoxModel
    {
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
        public AutoComplete Autocomplete { get; set; }

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the TextBox. One or more custom CSS classes can be added to a TextBox.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Enable or disable the persisting TextBox state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in the right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the TextBox allows the user to interact with it.
        /// </summary>
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
        public FloatLabelType FloatLabelType { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as styles, class, and more to the root element.
        /// <para>If you configured both property and equivalent html attributes, then the component considers the property value.</para>
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both property and equivalent input attribute, then the component considers the property value.</para>
        /// </summary>
        public Dictionary<string, object> InputAttributes { get; set; }

        /// <summary>
        /// Specifies the global culture and localization of the TextBox component.
        /// </summary>
        [Obsolete("Locale is deprecated and will no longer be used. Hereafter, the locale works based on the system culture.")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a boolean value that enables or disables the multiline on the TextBox.
        /// <para>The TextBox changes from a single line to multiline when enabling this multiline mode.</para>
        /// </summary>
        public bool Multiline { get; set; }

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in TextBox. The property is depending on the FloatLabelType property.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the TextBox allows user to change the text.
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the clear button is displayed in TextBox.
        /// </summary>
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Specifies the behavior of the TextBox such as text, password, email, and more.
        /// </summary>
        public InputType Type { get; set; }

        /// <summary>
        /// Sets the content of the TextBox.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Specifies the width of the TextBox component.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Specifies the tab order of the TextBox component.
        /// </summary>
        public int TabIndex { get; set; }
    }
}
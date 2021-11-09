namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Interface for a class CheckBox.
    /// </summary>
    public class CheckBoxModel<TChecked>
    {
        /// <summary>
        /// Specifies a value that indicates whether the CheckBox is `checked` or not.
        /// When set to `true`, the CheckBox will be in `checked` state.
        /// </summary>
        public TChecked Checked { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the CheckBox element.
        /// You can add custom styles to the CheckBox by using this property.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the CheckBox is `disabled` or not.
        /// When set to `true`, the CheckBox will be in `disabled` state.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// </summary>
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as disabled, value etc., to the element.
        /// If you configured both property and equivalent html attribute then the component considers the property value.
        /// </summary>
        public object HtmlAttributes { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the CheckBox is in `indeterminate` state or not.
        /// When set to `true`, the CheckBox will be in `indeterminate` state.
        /// </summary>
        public bool Indeterminate { get; set; }

        /// <summary>
        /// Defines the caption for the CheckBox, that describes the purpose of the CheckBox.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Positions label `before`/`after` the CheckBox.
        /// The possible values are:
        ///  Before - The label is positioned to left of the CheckBox.
        ///  After - The label is positioned to right of the CheckBox.
        /// </summary>
        public LabelPosition LabelPosition { get; set; }

        /// <summary>
        /// Defines `name` attribute for the CheckBox.
        /// It is used to reference form data (CheckBox value) after a form is submitted.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines `value` attribute for the CheckBox.
        /// It is a form data passed to the server when submitting the form.
        /// </summary>
        public string Value { get; set; }
    }
}

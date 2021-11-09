using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The ComboBox component allows the user to type a value or choose an option from the list of predefined options.
    /// </summary>
    public partial class SfComboBox<TValue, TItem> : SfDropDownList<TValue, TItem>
    {
        /// <summary>
        /// Specifies whether the component allows user defined value which does not exist in data source.
        /// </summary>
        [Parameter]
        public bool AllowCustom { get; set; } = true;

        /// <summary>
        /// Specifies whether suggest a first matched item in input when searching. No action happens when no matches found.
        /// </summary>
        [Parameter]
        public bool Autofill { get; set; }

        /// <summary>
        /// <para>Specifies whether to show or hide the clear button.</para>
        /// <para>When the clear button is clicked, `Value`, `Text`, and `Index` properties are reset to null.</para>
        /// </summary>
        [Parameter]
        public override bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies a boolean value that indicates whether the component validates the input or not.
        /// </summary>
        /// <value>
        /// <c>true</c>, If the ValidateOnInput is enabled for form validation, then the model value will be updated on entering the value to the input. otherwise, <b>false</b>.The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property is used to validate the form on typing into the input and updating the model value. The ValueChange event will be fired after the component lost its focus.
        /// </remarks>
        [Parameter]
        public bool ValidateOnInput { get; set; }

        /// <summary>
        /// Parent component of ComboBox.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic ComboBoxParent { get; set; }
    }
}
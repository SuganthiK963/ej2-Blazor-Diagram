using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The NumericTextBox is used to get the number inputs from the user. The input values can be incremented or decremented by a predefined step value.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of SfNumericTextBox.</typeparam>
    public class NumericTextBoxEvents<TValue> : SfInputTextBase<TValue>
    {
        /// <summary>
        /// Specifies the base parent of SfNumericTextBox.
        /// </summary>
        [CascadingParameter]
        protected SfNumericTextBox<TValue> BaseParent { get; set; }

        /// <summary>
        /// Triggers when the NumericTextBox got focus out.
        /// </summary>
        [Parameter]
        public EventCallback<NumericBlurEventArgs<TValue>> Blur { get; set; }

        /// <summary>
        /// Triggers when the value of the NumericTextBox changes.
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Triggers when the NumericTextBox component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the NumericTextBox component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the NumericTextBox got focus in.
        /// </summary>
        [Parameter]
        public EventCallback<NumericFocusEventArgs<TValue>> Focus { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.NumericEvents = this;
        }
    }
}
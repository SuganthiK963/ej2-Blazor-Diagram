using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.Blazor.Inputs;
namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The NumericTextBox is used to get the number inputs from the user. The input values can be incremented or decremented by a predefined step value.
    /// </summary>
    public partial class SfNumericTextBox<TValue> : SfInputTextBase<TValue>
    {
        /// <summary>
        /// Decrements the NumericTextBox value with specified step value.
        /// </summary>
        /// <param name="step">Specifies the value used to decrement the NumericTextBox value. If its not given then numeric value will be decremented based on the step property value.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Decrement(TValue step)
        {
            await ChangeValue(PerformAction(Value, step, "decrement"));
            await RaiseChangeEvent();
        }
        /// <summary>
        /// Decrements the NumericTextBox value with specified step value.
        /// </summary>
        /// <param name="step">Specifies the value used to decrement the NumericTextBox value. If its not given then numeric value will be decremented based on the step property value.</param>
        /// <returns>Task.</returns>
        public async Task DecrementAsync(TValue step)
        {
            await Decrement(step);
        }
        /// <summary>
        /// Sets the focus to the NumericTextBox component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.NumericTextBox.focusIn", new object[] { InputElement });
        }
        /// <summary>
        /// Sets the focus to the NumericTextBox component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusAsync()
        {
            await FocusIn();
        }
        /// <summary>
        /// Remove the focus from the NumericTextBox component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            await InvokeMethod("sfBlazor.NumericTextBox.focusOut", new object[] { InputElement });
        }
        /// <summary>
        /// Remove the focus from the NumericTextBox component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusOutAsync()
        {
            await FocusOut();
        }
        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<string> GetPersistData()
        {
            return await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
        }
        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<String> GetPersistDataAsync()
        {
            return await GetPersistData();
        }
        /// <summary>
        /// Returns the value of NumericTextBox with the format applied to the NumericTextBox.
        /// </summary>
        /// <returns>The input element value.</returns>
        [Obsolete("This GetText method is deprecated. Use GetFormattedText method to achieve the functionality.")]
        public string GetText() {
            return FormatValueAsString(InputTextValue);
        }
        /// <summary>
        /// Returns the value of NumericTextBox with the format applied to the NumericTextBox.
        /// </summary>
        /// <returns>The input element value.</returns>
        public string GetFormattedText()
        {
            return FormatValueAsString(InputTextValue);
        }

        /// <summary>
        /// Increments the NumericTextBox value with the specified step value.
        /// <param name="step">Specifies the value used to increment the NumericTextBox value.if its not given then numeric value will be incremented based on the step property value.</param>
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Increment(TValue step)
        {
            await ChangeValue(PerformAction(Value, step, "increment"));
            await RaiseChangeEvent();
        }
        /// <summary>
        /// Increments the NumericTextBox value with the specified step value.
        /// <param name="step">Specifies the value used to increment the NumericTextBox value.if its not given then numeric value will be incremented based on the step property value.</param>
        /// </summary>
        /// <returns>Task.</returns>
        public async Task IncrementAsync(TValue step)
        {
            await Increment(step);
        }
        /// <summary>
        /// Invoke the event, while paste the value to input element.
        /// </summary>
        /// <param name="beforeValue">Specifies the previous element value.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task InvokePasteHandler(string beforeValue)
        {
            var jsRunTime = JSRuntime as IJSInProcessRuntime;
            if (jsRunTime == null)
            {
                IsPasteValue = true;
                await UpdatePasteInput(beforeValue);
            }
        }

        /// <summary>
        /// Invokable the increment/decrement actions.
        /// </summary>
        /// <param name="action">Specifies the action.</param>
        /// <param name="args"><see cref="EventArgs"/> arguments.</param>
        /// <param name="currentInputValue">Specifies the input value</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ServerAction(string action, EventArgs args, string currentInputValue)
        {
            await Action(action, args, currentInputValue);
        }

        /// <summary>
        /// Invokable the component value.
        /// </summary>
        /// <returns>Task.</returns>
        /// <param name="value">Specifies the value.</param>
        /// <param name="args"><see cref="EventArgs"/> arguments.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ServerupdateValue(TValue value, EventArgs args)
        {
            await UpdateValue(value, args);
        }
    }
}
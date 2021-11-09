using System;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.Blazor.Inputs;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The TextBox is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfTextBox : SfInputTextBase<string>
    {
        /// <summary>
        /// Adding the icons to the TextBox component.
        /// </summary>
        /// <param name="position">The adding icons to the component based on position for prepend/append.</param>
        /// <param name="icons">The icons class is added to icon element.</param>
        /// <param name="events">The icon events are added to the events element</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AddIcon(string position, string icons, Dictionary<string, object> events = null)
        {
            if (!string.IsNullOrEmpty(position))
            {
                await AddIcons(position, icons, events);
            }
        }
        /// <summary>
        /// Adding the icons to the TextBox component.
        /// </summary>
        /// <param name="position">The adding icons to the component based on position for prepend/append.</param>
        /// <param name="icons">The icons class is added to icon element.</param>
        /// <returns>Task.</returns>
        public async Task AddIconAsync(string position, string icons)
        {
            await AddIcon(position, icons);
        }

        /// <summary>
        /// Sets the focus to TextBox component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.TextBox.focusIn", new object[] { InputElement });
        }
        /// <summary>
        /// Sets the focus to TextBox component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusAsync()
        {
            await FocusIn();
        }

        /// <summary>
        /// Remove the focus from TextBox component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            await InvokeMethod("sfBlazor.TextBox.focusOut", new object[] { InputElement });
        }
        /// <summary>
        /// Remove the focus from TextBox component, if the component is in focus state.
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
    }
}
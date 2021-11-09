using System;
using System.ComponentModel;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The MultiSelect component contains a list of predefined values from which a multiple value can be chosen.
    /// </summary>
    public partial class SfMultiSelect<TValue, TItem> : SfDropDownBase<TItem>
    {
        /// <summary>
        /// Sets the focus to the MultiSelect component for interaction.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.MultiSelect.focusIn", new object[] { InputElement });
        }
        /// <summary>
        /// Sets the focus to the MultiSelect component for interaction.
        /// </summary>
        public async Task FocusAsync()
        {
            await FocusIn();
        }
        /// <summary>
        /// Remove the focus from the MultiSelect component, if the component is in focus state.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            await InvokeMethod("sfBlazor.MultiSelect.focusOut", new object[] { InputElement });
        }

        /// <summary>
        /// Remove the focus from the MultiSelect component, if the component is in focus state.
        /// </summary>
        public async Task FocusOutAsync()
        {
            await FocusOut();
        }
        /// <summary>
        /// Hides the spinner loader.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task HideSpinnerAsync()
        {
            await HideSpinner();
        }
        /// <summary>
        /// Hides the spinner loader.
        /// </summary>
        public async Task HideSpinner()
        {
            if (FilterinputObj != null && FilterinputObj.SpinnerObj != null && Mode == VisualMode.CheckBox && IsListRender)
            {
                await FilterinputObj.SpinnerObj.HideAsync();
            }
            else if (SpinnerObj != null)
            {
                await SpinnerObj.HideAsync();
            }
        }

        /// <summary>
        /// Shows the spinner loader.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ShowSpinnerAsync()
        {
            await ShowSpinner();
        }
        /// <summary>
        /// Shows the spinner loader.
        /// </summary>
        public async Task ShowSpinner()
        {
            if (FilterinputObj != null && FilterinputObj.SpinnerObj != null && Mode == VisualMode.CheckBox && IsListRender)
            {
                await FilterinputObj.SpinnerObj.ShowAsync();
            }
            else if (SpinnerObj != null)
            {
                await SpinnerObj.ShowAsync();
            }
        }

        /// <summary>
        /// <para>Based on the state parameter, entire list item will be selected/deselected.</para>
        /// <para>parameter.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>True</term>
        /// <description>Selects entire list items.</description>
        /// </item>
        /// <item>
        /// <term>False</term>
        /// <description>Un Selects entire list items.</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="state">Specifies the state.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task SelectAll(bool state)
        {
            if (state)
            {
                await SelectAllValue();
            }
            else
            {
                await ClearAll();
            }
        }

        /// <summary>
        /// <para>Based on the state parameter, entire list item will be selected/deselected.</para>
        /// <para>parameter</para>
        /// <list type="bullet">
        /// <item>
        /// <term>True</term>
        /// <description>Selects entire list items.</description>
        ///</item>
        /// <item>
        /// <term>False</term>
        /// <description>Un Selects entire list items.</description>
        /// </item>
        /// </list>
        /// </summary>
		/// <param name="state">Specifies the state.</param>
        /// <returns>Task.</returns>
        public async Task SelectAllAsync(bool state)
        {
            await SelectAll(state);
        }
        /// <summary>
        /// Allows you to clear the selected values from the MultiSelect component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Clear()
        {
            await ClearAll();
        }

        /// <summary>
        /// Allows you to clear the selected values from the MultiSelect component.
        /// </summary>
		/// <returns>Task.</returns>
        public async Task ClearAsync()
        {
            await Clear();
        }
        /// <summary>
        /// Gets all the list items bound on this component.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<IEnumerable<TItem>> GetItemsAsync()
        {
            return await GetItems();
        }
        /// <summary>
        /// Gets all the list items bound on this component.
        /// </summary>
		/// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<IEnumerable<TItem>> GetItems()
        {
            if (ListData == null || !ListData.Any())
            {
                await Render(DataSource, Fields, Query);
            }

            return ListData;
        }

        /// <summary>
        /// Gets the array of data Object that matches the given array of values.
        /// </summary>
        /// <param name = "dataValue">Specifies the value(s).</param>
        /// <returns>Task.</returns>
        public async Task<List<TItem>> GetDataByValueAsync(TValue dataValue)
        {
            return await GetDataByValue(dataValue);
        }
        /// <summary>
        /// Gets the array of data Object that matches the given array of values.
        /// </summary>
        /// <param name = "dataValue">Specifies the value(s).</param>
		/// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<List<TItem>> GetDataByValue(TValue dataValue)
        {
            var data = new List<TItem>();
            if (dataValue != null)
            {
                Type valueType = typeof(TValue);
                if (valueType.IsArray || (valueType.IsGenericType && (valueType.GetGenericTypeDefinition() == typeof(List<>) || valueType.GetGenericTypeDefinition() == typeof(ICollection<>) || valueType.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
                {
                    if (!(valueType == typeof(string[]) || valueType == typeof(List<string>)))
                    {
                        if (valueType.IsArray && valueType == typeof(int?[]))
                        {
                            List<int?> values = (dataValue as int?[]).ToList();
                            foreach (var val in values)
                            {
                                data.Add(await GetDataObject(val));
                            }
                        }
                        else
                        {
                            foreach (var val in dataValue as dynamic)
                            {
                                data.Add(await GetDataObject(val));
                            }
                        }
                    }
                    else if (valueType == typeof(string[]) || valueType == typeof(List<string>))
                    {
                        List<string> values = valueType.IsArray ? (dataValue as string[]).ToList() : dataValue as List<string>;
                        foreach (var val in values)
                        {
                            data.Add(await GetDataObject(val));
                        }
                    }
                }
                else if (IsSimpleType())
                {
                    data.Add(await GetDataObject(dataValue));
                }
            }

            return data;
        }
		
		/// <summary>
        /// To filter the data from given data source by using query.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <param name="fields">Specifies the fields.</param>
        /// <returns>Task.</returns>
        public async Task FilterAsync(IEnumerable<TItem> dataSource, Query query = null, FieldSettingsModel fields = null)
        {
            await Filter(dataSource, query, fields);
        }

        /// <summary>
        /// To filter the data from given data source by using query.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <param name="fields">Specifies the fields.</param>
        /// <returns>Task.</returns>
		/// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Filter(IEnumerable<TItem> dataSource, Query query = null, FieldSettingsModel fields = null)
        {
            IsCustomFilter = true;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
            DataManager.Json = (IEnumerable<object>)dataSource;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            await SetListData(dataSource, fields, query);
            if (!IsListRender)
            {
                await ShowPopup();
            }
        }

        /// <summary>
        /// Hides the popup if it is in an open state.
        /// </summary>
		/// <returns>Task.</returns>
        public async Task HidePopupAsync()
        {
            await this.HidePopup();
        }
        /// <summary>
        /// Opens the popup that displays the list of items.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ShowPopupAsync()
        {
            await ShowPopup();
        }
        /// <summary>
        /// Opens the popup that displays the list of items.
        /// </summary>
		/// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ShowPopup()
        {
            var isCustomValue = IsTyped && !string.IsNullOrEmpty(InputValue) && Mode != VisualMode.CheckBox && (AllowCustomValue || AllowFiltering);
            if (!(SelectedData != null && HideSelectedItem && SelectedData.Count > 0 && MainData != null && SelectedData.Count == MainData.Where(item => item != null)?.Count()) || isCustomValue)
            {
                var beforeOpenArgs = new BeforeOpenEventArgs() { Cancel = false };
                await SfBaseUtils.InvokeEvent<BeforeOpenEventArgs>(MultiselectEvents?.OnOpen, beforeOpenArgs);
                if (!beforeOpenArgs.Cancel)
                {
                    if ((ListData == null || !ListData.Any()) && !BeforePopupOpen)
                    {
                        await Render(DataSource, Fields, Query);
                    }

                    var openEventArgs = OpenEventArgs();
                    BeforePopupOpen = true;
                    PopupEventArgs = OpenEventArgs();
                    await SfBaseUtils.InvokeEvent<PopupEventArgs>(MultiselectEvents?.Opened, PopupEventArgs);
                    IsModifiedPopup = false;
                    var oldString = System.Text.Json.JsonSerializer.Serialize(PopupEventArgs.Popup);
                    var newString = System.Text.Json.JsonSerializer.Serialize(openEventArgs.Popup);
                    IsModifiedPopup = !string.Equals(oldString, newString, StringComparison.Ordinal);
                    if (!PopupEventArgs.Cancel)
                    {
                        await ListFocus();
                        SetReOrder();
                        UpdatePopupState();
                        if (ShowPopupList && !IsFirstRenderPopup && AllowFiltering)
                        {
                            IsFirstRenderPopup = true;
                            MainData = ListData;
                        }
                    }
                }
                else
                {
                    BeforePopupOpen = false;
                }
            }
        }

        /// <summary>
        /// Hides the popup if it is in an open state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task HidePopup()
        {
            PopupEventArgs = new PopupEventArgs() { Cancel = false };
            await SfBaseUtils.InvokeEvent<PopupEventArgs>(MultiselectEvents?.OnClose, PopupEventArgs);
            if (!PopupEventArgs.Cancel)
            {
                var options = GetProperty();
                IsListRender = false;
                await InvokeMethod("sfBlazor.MultiSelect.closePopup", new object[] { InputElement, PopupEventArgs, options });
                var closedArgs = new ClosedEventArgs() { };
                await SfBaseUtils.InvokeEvent<ClosedEventArgs>(MultiselectEvents?.Closed, closedArgs);
            }
        }

        /// <summary>
        /// Invoke the before the popup close.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClosePopup()
        {
            await ClosePopupAction();
        }

        /// <summary>
        /// Hides the MultiSelect popup.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task Hide()
        {
            await HidePopup();
        }

        /// <summary>
        /// Invoke the blur event for the MultiSelect input.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task InvokeBlur()
        {
            var focusArgs = new Microsoft.AspNetCore.Components.Web.FocusEventArgs();
            await BlurHandler(focusArgs, false);
        }

        /// <summary>
        /// Invoke the RemainCount event for the return the MultiSelect remainder count.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task<string> RemainCount(double count)
        {
            return await Task.FromResult(Intl.GetNumericFormat<double>(count));
        }

        /// <summary>
        /// Invoke the scroll handler.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task VirtualScrollHandler()
        {
            if (Query != null && Query.Queries != null)
            {
                var takeData = Query.Queries.Take;
                var query = CloneQuery(Query);
                Query = query.Take(takeData + ItemsCount);
                await VirtualSpinnerObj.ShowAsync();
                await SetListData(DataSource, Fields, query);
                if (AllowFiltering)
                {
                    MainData = ListData;
                }

                await VirtualSpinnerObj.HideAsync();
                await ListFocus();
                StateHasChanged();
            }
        }
    }
}
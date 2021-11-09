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
    /// The DropDownList component contains a list of predefined values from which a single value can be chosen.
    /// </summary>
    public partial class SfDropDownList<TValue, TItem> : SfDropDownBase<TItem>
    {
        /// <summary>
        /// Allows you to clear the selected values from the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Clear() {
            await this.ClearAll();
        }
        /// <summary>
        /// Allows you to clear the selected values from the component.
        /// </summary>
		/// <returns>Task.</returns>
        public async Task ClearAsync()
        {
            await Clear();
        }
        /// <summary>
        /// Sets the focus to the DropDownList component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            var inputElement = IsDropDownList ? InputBaseObj?.ContainerElement : InputBaseObj?.InputElement;
            await InvokeMethod("sfBlazor.DropDownList.focusIn", new object[] { inputElement });
        }

        /// <summary>
        /// Sets the focus to the DropDownList component for interaction.
        /// </summary>
		/// <returns>Task.</returns>
        public async Task FocusAsync()
        {
            await FocusIn();
        }
        /// <summary>
        /// Remove the focus from the DropDownList component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            var inputElement = IsDropDownList ? InputBaseObj?.ContainerElement : InputBaseObj?.InputElement;
            await InvokeMethod("sfBlazor.DropDownList.focusOut", new object[] { inputElement });
        }

        /// <summary>
        /// Remove the focus from the DropDownList component, if the component is in focus state.
        /// </summary>
		/// <returns>Task.</returns>
        public async Task FocusOutAsync()
        {
            await FocusOut();
        }
        /// <summary>
        /// Gets the data Object that matches the given value.
        /// </summary>
        /// <param name="ddlValue">Specifies the DropDownList value.</param>
        /// <returns>Task.</returns>
        public TItem GetDataByValue(TValue ddlValue)
        {
            if (ListData != null)
            {
                var propertyType = typeof(TValue);
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                if (IsSimpleDataType())
                {
                    if (propertyType.IsEnum)
                    {
                        return ListData.Where(item => SfBaseUtils.Equals((TValue)Enum.Parse(propertyType, item.ToString()), ddlValue)).FirstOrDefault();
                    }
                    else
                    {
                        return ListData.Where(item => SfBaseUtils.Equals(item, ddlValue)).FirstOrDefault();
                    }
                }
                else if (propertyType == typeof(TItem))
                {
                    var ddlStringValue = System.Text.Json.JsonSerializer.Serialize(ddlValue);
                    return ListData.Where(item => string.Equals(System.Text.Json.JsonSerializer.Serialize(item), ddlStringValue, StringComparison.Ordinal)).FirstOrDefault();
                }
                else
                {
                    var fields = !string.IsNullOrEmpty(Fields?.Value) ? Fields.Value : "value";
                    ddlValue = (propertyType.IsEnum && ddlValue != null) ? (TValue)Enum.Parse(propertyType, ddlValue.ToString()) : ddlValue;
                    return ListData.Where(item => SfBaseUtils.Equals((TValue)SfBaseUtils.ChangeType(DataUtil.GetObject(fields, item), propertyType), ddlValue)).FirstOrDefault();
                }
            }

            return default;
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
        public async Task<IEnumerable<TItem>> GetItems() {
            if (ListData == null || !ListData.Any())
            {
                await Render(DataSource, Fields, Query);
            }

            return ListData;
        }

        /// <summary>
        /// Hides the spinner loader.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task HideSpinnerAsync()
        {
            await HideSpinner();
        }
        /// <summary>
        /// Hides the spinner loader.
        /// </summary>
		/// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task HideSpinner()
        {
            if (InputBaseObj != null && InputBaseObj.SpinnerObj != null)
            {
                DropdownIcon[0] = SfBaseUtils.RemoveClass(DropdownIcon[0], DISABLE_ICON);
                await InputBaseObj.SpinnerObj.HideAsync();
            }
        }

        /// <summary>
        /// Shows the spinner loader.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ShowSpinnerAsync()
        {
            await ShowSpinner();
        }
        /// <summary>
        /// Shows the spinner loader.
        /// </summary>
		/// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ShowSpinner()
        {
            if (InputBaseObj != null && InputBaseObj.SpinnerObj != null)
            {
                await InputBaseObj.SpinnerObj.ShowAsync();
            }
        }

        /// <summary>
        /// Hides the DropDownList popup.
        /// </summary>
        /// <exclude/>
        /// <returns>Task.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task Hide()
        {
            if (IsListRender)
            {
                await HidePopup();
            }
        }

        /// <summary>
        /// Hides the DropDownList popup.
        /// </summary>
		/// <returns>Task.</returns>
        public async Task HidePopupAsync()
        {
            await HidePopup();
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
        /// Invoke the before the popup close.
        /// </summary>
        /// <exclude/>
        /// <returns>Task.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClosePopup()
        {
            await ClosePopupAction();
        }

        /// <summary>
        /// Invoke the key action handler.
        /// </summary>
        /// <param name="args">Specifies KeyActions arguments.</param>
        /// <exclude/>
        /// <returns>Task.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task KeyActionHandler(KeyActions args)
        {
            if (args != null)
            {
                await KeyboardActionHandler(args);
            }
        }

        /// <summary>
        /// Invoke the scroll handler.
        /// </summary>
        /// <exclude/>
        /// <returns>Task.</returns>
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
                if (ItemData != null)
                {
                    await UpdateListSelection(ItemData, SELECTED);
                }

                await VirtualSpinnerObj.HideAsync();
                if (string.IsNullOrEmpty(TypedString))
                {
                    MainData = ListData;
                }
                StateHasChanged();
            }
        }
    }
}

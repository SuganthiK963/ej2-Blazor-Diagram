using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Syncfusion.Blazor.Inputs.Internal;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Spinner;
using System.Globalization;
using System.Reflection;
using Syncfusion.Blazor.Popups;
using System.Text.RegularExpressions;
using System.Collections;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The DropDownList component contains a list of predefined values from which a single value can be chosen.
    /// </summary>
    /// <typeparam name="TValue">Specifies the value type.</typeparam>
    /// <typeparam name="TItem">Specifies the type of SfDropDownList.</typeparam>
    public partial class SfDropDownList<TValue, TItem> : SfDropDownBase<TItem>, IInputBase, IDropDowns
    {
        private const string BACK_ICON = "e-back-icon e-icons";
        private const string FILTER_INPUT = "e-input-filter";
        private const string FILTER_PARENT = "e-filter-parent";
        private const string CONTAINER_CLASS = "e-ddl e-lib";
        private const string POPUP = "e-popup";
        private const string CONTROL_LIB = "e-control e-lib";
        private const string DDL = "e-ddl";
        private const string RTL = "e-rtl";
        private const string SINGLE_SPACE = " ";
        private const string ICON_ANIM = "e-icon-anim";
        private const string NO_RECORD_LOCALE_KEY = "DropDownList_NoRecordsTemplate";
        private const string NO_RECORD_LOCALE_VALUE = "No records found";
        private const string ACTION_FAILURE_LOCALE_KEY = "DropDownList_ActionFailureTemplate";
        private const string ACTION_FAILURE_LOCALE_VALUE = "The action failure";
        private const string READ_ONLY = "e-readonly";

        /// <summary>
        /// Specifies the string false.
        /// </summary>
        protected const string FALSE = "false";

        /// <summary>
        /// Specifies the string true.
        /// </summary>
        protected const string TRUE = "true";

        /// <summary>
        /// Specifies the root class of base component.
        /// </summary>
        protected const string NO_DATA = "e-content e-dropdownbase e-nodata";

        /// <summary>
        /// specifies the popup content.
        /// </summary>
        /// <exclude/>
        protected const string POPUP_CONTENT = "e-content e-dropdownbase";

        /// <summary>
        /// specifies the group by class.
        /// </summary>
        /// <exclude/>
        protected const string GROUP_BY = "e-dd-group";

        /// <summary>
        /// specifies the dropdown icon class.
        /// </summary>
        /// <exclude/>
        protected const string DROP_DOWN_ICON = "e-ddl-icon e-icons";

        /// <summary>
        /// specifies the dropdown header.
        /// </summary>
        /// <exclude/>
        protected const string DDL_HEADER = "e-ddl-header";

        /// <summary>
        /// specifies the dropdown footer.
        /// </summary>
        /// <exclude/>
        protected const string DDL_FOOTER = "e-ddl-footer";

        /// <summary>
        /// specifies the aria-live class.
        /// </summary>
        /// <exclude/>
        protected const string ARIA_LIVE = "aria-live";

        /// <summary>
        /// specifies the assertive string.
        /// </summary>
        protected const string ASSERTIVE = "assertive";

        /// <summary>
        /// specifies the aria-has-popup class.
        /// </summary>
        protected const string ARIA_HAS_POPUP = "aria-haspopup";

        /// <summary>
        /// specifies the aria-activedescendant class.
        /// </summary>
        /// <exclude/>
        protected const string ARIA_ACTIVE_DESCENDANT = "aria-activedescendant";

        /// <summary>
        /// specifies the null text.
        /// </summary>
        /// <exclude/>
        protected const string NULL_VALUE = "null";

        /// <summary>
        /// specifies the aria-own class.
        /// </summary>
        /// <exclude/>
        protected const string ARIA_OWN = "aria-owns";

        /// <summary>
        /// specifies the options text.
        /// </summary>
        /// <exclude/>
        protected const string OPTIONS = "_options";

        /// <summary>
        /// specifies the name of role attribute.
        /// </summary>
        /// <exclude/>
        protected const string ROLE = "role";

        /// <summary>
        /// specifies the listbox text.
        /// </summary>
        /// <exclude/>
        protected const string LIST_BOX = "listbox";

        /// <summary>
        /// specifies the name of autocorrect attribute.
        /// </summary>
        /// <exclude/>
        protected const string AUTO_CORRECT = "autocorrect";

        /// <summary>
        /// specifies the off text.
        /// </summary>
        /// <exclude/>
        protected const string OFF = "off";

        /// <summary>
        /// specifies the spellcheck attribute.
        /// </summary>
        /// <exclude/>
        protected const string SPELL_CHECK = "spellcheck";

        /// <summary>
        /// specifies the name of autocomplete attribute.
        /// </summary>
        /// <exclude/>
        protected const string AUTO_COMPLETE = "autocomplete";

        /// <summary>
        /// specifies the name of autocapitalize attribute.
        /// </summary>
        /// <exclude/>
        protected const string AUTO_CAPITAL = "autocapitalize";

        /// <summary>
        /// specifies the name of aria-expanded attribute.
        /// </summary>
        /// <exclude/>
        protected const string ARIA_EXPANDED = "aria-expanded";

        /// <summary>
        /// specifies the selected class.
        /// </summary>
        /// <exclude/>
        protected const string SELECTED = "e-active";

        /// <summary>
        /// specifies the item focus class.
        /// </summary>
        /// <exclude/>
        protected const string ITEM_FOCUS = "e-item-focus";
        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string DISABLE_ICON = "e-ddl-disable-icon";
        private const string TAB_INDEX = "tabindex";

        /// <summary>
        /// Specifies the SfInputBase component object.
        /// </summary>
        /// <exclude/>
        protected SfInputBase InputBaseObj { get; set; }

        /// <summary>
        /// Specifies the SfInputBase component object as filter input.
        /// </summary>
        /// <exclude/>
        protected SfInputBase FilterinputObj { get; set; }

        /// <summary>
        /// Specifies the root class of the component .
        /// </summary>
        /// <exclude/>
        protected string RootClass { get; set; }

        /// <summary>
        /// Specifies the container class of the component .
        /// </summary>
        /// <exclude/>
        protected string ContainerClass { get; set; }

        /// <summary>
        /// Specifies the back icon.
        /// </summary>
        /// <exclude/>
        private List<string> BackIcon { get; set; }

        /// <summary>
        /// Specifies the dropdown value.
        /// </summary>
        /// <exclude/>
        protected string DropdownValue { get; set; }

        /// <summary>
        /// Specifies the clear button stop propogation.
        /// </summary>
        /// <exclude/>
        protected bool ClearBtnStopPropagation { get; set; }

        /// <summary>
        /// Specifies the filter clear button stop propogation.
        /// </summary>
        /// <exclude/>
        private bool FilterClearBtnStopPropagation { get; set; }

        /// <summary>
        /// Specifies whether it is a prevent container or not.
        /// </summary>
        /// <exclude/>
        protected bool PreventContainer { get; set; }

        /// <summary>
        /// Specifies whether it is a device or not.
        /// </summary>
        /// <exclude/>
        protected bool IsDevice { get; set; }

        /// <summary>
        /// Specifies whether to show popup list or not.
        /// </summary>
        /// <exclude/>
        protected bool ShowPopupList { get; set; }

        /// <summary>
        /// Specifies the popup element reference.
        /// </summary>
        /// <exclude/>
        protected ElementReference PopupElement { get; set; }

        /// <summary>
        /// Specifies the popup holder element.
        /// </summary>
        /// <exclude/>
        protected ElementReference PopupHolderEle { get; set; }

        /// <summary>
        /// Specifies the popup container.
        /// </summary>
        /// <exclude/>
        protected string PopupContainer { get; set; }

        /// <summary>
        /// Specifies whether list is going to render or not.
        /// </summary>
        /// <exclude/>
        protected bool IsListRender { get; set; }

        /// <summary>
        /// Specifies whether list is rendered or not.
        /// </summary>
        /// <exclude/>
        private bool IsListRendered { get; set; }

        /// <summary>
        /// Specifies whether dropdown is clicked or not.
        /// </summary>
        /// <exclude/>
        protected bool IsDropDownClick { get; set; }

        private PopupEventArgs PopupEventArgs { get; set; }

        private bool IsModifiedPopup { get; set; }

        /// <summary>
        /// Specifies whether it is valid key or not.
        /// </summary>
        /// <exclude/>
        protected bool IsValidKey { get; set; }

        /// <summary>
        /// Specifies whether a key is typed or not.
        /// </summary>
        /// <exclude/>
        protected bool IsTyped { get; set; }

        /// <summary>
        /// Specifies whether the filter input clear icon is clicked or not.
        /// </summary>
        /// <exclude/>
        protected bool IsFilterClearClicked { get; set; }

        /// <summary>
        /// Specifies whether it is custom filtering or not.
        /// </summary>
        /// <exclude/>
        protected bool IsCustomFilter { get; set; }

        private string FilterInputValue { get; set; }

        private bool IsFilterInputChange { get; set; }

        /// <summary>
        /// Specifies the active index of the component.
        /// </summary>
        /// <exclude/>
        protected int? ActiveIndex { get; set; }

        private RenderFragment ValueTemp { get; set; }

        /// <summary>
        /// Specifies whether it is a internal focus or not.
        /// </summary>
        /// <exclude/>
        protected bool IsInternalFocus { get; set; }

        internal DropDownListEvents<TValue, TItem> DropdownlistEvents { get; set; }

        private bool IsFirstRenderPopup { get; set; }

        private Dictionary<string, object> FilterAttributes { get; set; }

        /// <summary>
        /// Specifies the container attribute.
        /// </summary>
        /// <exclude/>
        protected Dictionary<string, object> ContainerAttr { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Specifies the previous value of the component.
        /// </summary>
        /// <exclude/>
        protected TValue PreviousValue { get; set; }

        /// <summary>
        /// Specifies the previous item data.
        /// </summary>
        /// <exclude/>
        protected TItem PreviousItemData { get; set; }

        private bool IsEscapeKey { get; set; }

        private string PrevString { get; set; }

        /// <summary>
        /// Specifies the typed string.
        /// </summary>
        /// <exclude/>
        protected string TypedString { get; set; } = string.Empty;

        /// <summary>
        /// True before the popup get openend , otherwise  false.
        /// </summary>
        /// <exclude/>
        protected bool BeforePopupOpen { get; set; }

        /// <summary>
        /// Specifies whether to prevent autofill or not.
        /// </summary>
        /// <exclude/>
        protected bool PreventAutoFill { get; set; }

        private string ValidClass { get; set; }

        /// <summary>
        /// Specifies the focused data.
        /// </summary>
        /// <exclude/>
        protected ListOptions<TItem> FocusedData { get; set; }

        /// <summary>
        /// Specifies the virtual spinner object.
        /// </summary>
        /// <exclude/>
        protected SfSpinner VirtualSpinnerObj { get; set; }

        /// <summary>
        /// Specifies the component name.
        /// </summary>
        /// <exclude/>
        protected override string ComponentName { get; set; } = "SfDropDownList";

        /// <summary>
        /// Specifies whether it is prevent icon handler.
        /// </summary>
        /// <exclude/>
        protected bool PreventIconHandler { get; set; }

        /// <summary>
        /// Specifies the popup content.
        /// </summary>
        /// <exclude/>
        protected string PopupContent { get; set; }

        /// <summary>
        /// Specifies the NoDataContent.
        /// </summary>
        /// <exclude/>
        protected string NoDataContent { get; set; }

        private bool IsMutable { get; set; }

        /// <summary>
        /// Specifies the dropdown icon.
        /// </summary>
        /// <exclude/>
        protected List<string> DropdownIcon { get; set; }

        private bool PreventAltUp { get; set; }

        private KeyActions KeyAction { get; set; }

        private bool IsDropDownList { get; set; }

        private bool IsAutoComplete { get; set; }

        private bool IsComboBox { get; set; }

        /// <summary>
        /// Specifies the root class of the component.
        /// </summary>
        /// <exclude/>
        protected virtual string ROOT { get; set; } = "e-control e-dropdownlist e-lib";

        /// <summary>
        /// Invoke state change of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CallStateHasChangedAsync() => await InvokeAsync(StateHasChanged);

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DropdownIcon = new List<string>() { DROP_DOWN_ICON };
            ddlValue = Value;
            index = Index;
            cssClass = CssClass;
            floatLabelType = FloatLabelType;
            PopupContainer = DDL + SINGLE_SPACE + CONTROL_LIB + SINGLE_SPACE + POPUP;

            // Unique class added for dynamically rendered Inplace-editor components
            if (DropDownListParent != null)
            {
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, "e-editable-elements");
            }

            PreRender();
            IsAutoComplete = ComponentName == nameof(SfAutoComplete<TValue, TItem>);
            IsDropDownList = ComponentName == nameof(SfDropDownList<TValue, TItem>);
            IsComboBox = ComponentName == nameof(SfComboBox<TValue, TItem>);
            if (DropDownListParent != null && Convert.ToString(DropDownListParent.Type, CultureInfo.CurrentCulture) == "DropDownList")
            {
                DropDownListParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(DropDownListParent, this);
            }
        }

        /// <summary>
        /// Triggers before the component get rendered.
        /// </summary>
        /// <exclude/>
        protected void PreRender()
        {
            RootClass = ROOT;
            ContainerClass = CONTAINER_CLASS;

            // Unique class added for dynamically rendered Inplace-editor components
            if (DropDownListParent != null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, "e-editable-elements");
            }

            IsFirstRenderPopup = false;
            DynamicPropertyUpdate();
            if (SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)) && string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("dropdownlist");
            }

            DependencyScripts();
        }

        private void DynamicPropertyUpdate()
        {
            SetRTL();
            SetCssClass();
            SetReadOnly();
            UpdateAriaAttributes();
            if (SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)))
            {
                var tabIndex = Enabled ? this.TabIndex : -1;
                SfBaseUtils.UpdateDictionary(TAB_INDEX, tabIndex, this.ContainerAttr);
                SfBaseUtils.UpdateDictionary(TAB_INDEX, -1, InputAttributes);
                if (AllowFiltering)
                {
                    FilterAttributes = ContainerAttr != null ? new Dictionary<string, object>(ContainerAttr) : FilterAttributes;
                }
            }
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await PropertyParametersSet();
            if (PropertyChanges.Count > 0)
            {
                var changesProps = new Dictionary<string, object>(PropertyChanges);
                this.PropertyChanges.Remove(nameof(Value));
                await this.PropertyChange(changesProps);
                PropertyChanges.Remove(nameof(DataSource));
            }

            DynamicPropertyUpdate();
            UpdateValidateClass();
        }

        private async Task PropertyParametersSet()
        {
            if (!IsMutable)
            {
                this.ddlValue = this.NotifyPropertyChanges(nameof(Value), Value, ddlValue, true);
            }

            NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            floatLabelType = NotifyPropertyChanges(nameof(FloatLabelType), FloatLabelType, floatLabelType);
            NotifyPropertyChanges(nameof(Index), Index, index);
            this.index = this.NotifyPropertyChanges(nameof(Index), Index, index, true);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <param name="firstRender">true if the component rendered for the first time.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (AllowFiltering && IsDevice && IsDropDownList)
            {
                BackIcon = new List<string>() { BACK_ICON };
            }

            await ClientPopupRender();
            if (IsAutoComplete || IsComboBox)
            {
                FilterinputObj = InputBaseObj;
            }

            if (ShowPopupList && IsFilter() && IsFilterInputChange)
            {
                IsFilterInputChange = false;
                await RefreshPopup();
            }

            if (firstRender)
            {
                PopupContent = string.IsNullOrEmpty(Fields?.GroupBy) ? POPUP_CONTENT : POPUP_CONTENT + SINGLE_SPACE + GROUP_BY;
                NoDataContent = string.IsNullOrEmpty(Fields?.GroupBy) ? NO_DATA : NO_DATA + SINGLE_SPACE + GROUP_BY;
                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
                    localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                    if (!(localStorageValue == null && Value != null))
                    {
                        var persistValue = (TValue)SfBaseUtils.ChangeType(localStorageValue, typeof(TValue));
                        Value = ddlValue = await SfBaseUtils.UpdateProperty(persistValue, ddlValue, ValueChanged, DropDownsEditContext, ValueExpression);
                    }
                }

                PreviousValue = Value;
                if (Value != null || DDLText != null || Index != null)
                {
                    await InitValue();
                }

                await SfBaseUtils.InvokeEvent<object>(DropdownlistEvents?.Created, null);

                // Called In-placeEditor method for value updating
                if (DropDownListParent != null)
                {
                    DropDownListParent.GetType().GetMethod("UpdatePreviewValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(DropDownListParent, new object[] { true });
                }
            }
        }

        private async Task InitValue()
        {
            await SetListData(DataSource, Fields, Query);
            await UpdateValue();
        }

        /// <summary>
        /// Task which sets the old value.
        /// </summary>
        /// <param name="ddlValue">Specifies the DropDownList value.</param>
        /// <param name="isValueChanged"></param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task SetOldValue(object ddlValue, bool isValueChanged = false)
        {
            await Task.CompletedTask;
        }

        private async Task RenderDataItem()
        {
            if (ListData == null || !ListData.Any())
            {
                await Render(DataSource, Fields, Query);
            }
        }

        private async Task PropertyChange(Dictionary<string, object> newProps)
        {
            var newProperties = newProps.ToList();
            foreach (var prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Value):
                        this.PreviousValue = (this.PrevChanges != null && this.PrevChanges.Any() && this.PrevChanges.ContainsKey(nameof(Value))) ? (TValue)this.PrevChanges[nameof(Value)] : PreviousValue;
                        await RenderDataItem();
                        var getData = GetDataByValue(Value);
                        this.PreviousItemData = this.PreviousValue != null ? this.GetDataByValue(this.PreviousValue) : this.PreviousItemData;
                        if (ComponentName == nameof(SfDropDownList<TValue, TItem>) || getData != null)
                        {
                            if (getData == null && this.Value != null && this.EnableVirtualization)
                            {
                                await this.UpdateValue();
                                await this.UpdateSelectItem(this.ItemData);
                            } else  {
                                await UpdateSelectItem(getData);
                            }
                        }
                        else
                        {
                            await SetOldValue(prop.Value, EnableVirtualization);
                        }

                        break;
                    case nameof(Index):
                        if (!SfBaseUtils.Equals(prop.Value, ActiveIndex))
                        {
                            await RenderDataItem();

                            var indexItem = (Index != null) ? ListData.ElementAtOrDefault((int)Index) : default;
                            this.PreviousItemData = this.ItemData;
                            await UpdateSelectItem(indexItem);
                        }

                        break;
                    case nameof(DataSource):
                    case nameof(Query):
                        await Render(DataSource, Fields, Query);
                        if (Value != null && !IsTyped)
                        {
                            IsTyped = false;
                            await UpdateValue();
                        }

                        UpdateMainData();
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, cssClass);
                        PopupContainer = string.IsNullOrEmpty(PopupContainer) ? PopupContainer : SfBaseUtils.RemoveClass(PopupContainer, cssClass);
                        cssClass = CssClass;
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRendered();
                        break;
                    case nameof(SortOrder):
                        if (ListData != null && ListData.Any())
                        {
                            ListData = (SortOrder == SortOrder.None) ? ListData : (SortOrder == SortOrder.Ascending) ? ListData.OrderBy(item => DataUtil.GetObject(Fields?.Text, item)) : ListData.OrderByDescending(item => DataUtil.GetObject(Fields?.Text, item));
                            ListDataSource = (SortOrder == SortOrder.None) ? ListDataSource : (SortOrder == SortOrder.Ascending) ? ListDataSource.OrderBy(item => item.Text) : ListDataSource.OrderByDescending(item => item.Text);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Task which update main data.
        /// </summary>
        protected virtual void UpdateMainData()
        {
            MainData = ListData;
        }

        /// <summary>
        /// Task which gets the scroll value.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task GetScrollValue()
        {
            var takeData = Query.Queries.Take;
            var query = CloneQuery(Query);
            Query = query.Take(takeData + ItemsCount);
            await SetListData(DataSource, Fields, query);
            if (Value != null)
            {
                await Task.Delay(50);
            }

            await UpdateValue();
        }

        /// <summary>
        /// Task which updates the value.
        /// </summary>
        /// <returns>Task.</returns>
        protected virtual async Task UpdateValue()
        {
            if (Value != null)
            {
                ItemData = GetDataByValue(Value);
                if (ItemData == null && EnableVirtualization && DataManager != null && (DataManager.Json?.Count() > ListData?.Count() || TotalCount > ListData?.Count()))
                {
                    await GetScrollValue();
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(DDLText))
            {
                ItemData = GetDataByText(DDLText);
            }
            else if (Index != null && ListData != null)
            {
                ItemData = ListData.ElementAtOrDefault((int)Index);
            }

            await UpdateValueSelection(ItemData);
        }

        /// <summary>
        /// Task which updates the value selection.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="isMutable">Specifies whether it is mutable or not.</param>
        /// <param name="isValidateInput">Specifies whether it is validate on input event.</param>
        /// <returns></returns>
        /// <exclude/>
        protected async Task UpdateValueSelection(TItem item, bool isMutable = false, bool isValidateInput = false)
        {
            IsMutable = isMutable;
            ItemData = item;
            var dataItem = GetItemData();
            if (!isValidateInput)
            {
                await SetValue(dataItem);
            }
            await UpdateItemValue(dataItem);
            IsMutable = false;
        }

        private async Task UpdateItemValue(DataItems<TValue> dataItem)
        {
            var tempValue = ddlValue;
            Value = ddlValue = dataItem.Value;
            Value = ddlValue = await SfBaseUtils.UpdateProperty(dataItem.Value, tempValue, ValueChanged, DropDownsEditContext, ValueExpression);
            DDLText = dataItem?.Text?.ToString();
            var listItems = ListDataSource?.Where(list => !list.IsHeader)?.ToList();
            ActiveIndex = listItems?.IndexOf(listItems.Where(listitem => SfBaseUtils.Equals(listitem.CurItemData, ItemData)).FirstOrDefault());
            var tempIndex = index;
            Index = index = ActiveIndex == -1 ? null : ActiveIndex;
            Index = index = ActiveIndex == -1 ? null : await SfBaseUtils.UpdateProperty(ActiveIndex, tempIndex, IndexChanged);
        }

        /// <summary>
        /// Method which set RTL to the component.
        /// </summary>
        /// <exclude/>
        protected void SetRTL()
        {
            PopupContainer = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(PopupContainer, RTL) : SfBaseUtils.RemoveClass(PopupContainer, RTL);
        }
        
        /// <summary>
        /// Add the readonly class to the container element.
        /// </summary>
        private void SetReadOnly()
        {
            ContainerClass = Readonly ? SfBaseUtils.AddClass(ContainerClass, READ_ONLY).Trim() : SfBaseUtils.RemoveClass(ContainerClass, READ_ONLY).Trim();
        }

        /// <summary>
        /// Method which set css class to the component.
        /// </summary>
        /// <exclude/>
        protected void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass) && PopupContainer != null && !PopupContainer.Contains(CssClass, StringComparison.Ordinal))
            {
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, CssClass);
            }
        }

        /// <summary>
        /// Method which update the aria attributes.
        /// </summary>
        /// <exclude/>
        protected virtual void UpdateAriaAttributes()
        {
            if (ContainerAttr != null)
            {
                SfBaseUtils.UpdateDictionary(ARIA_LIVE, ASSERTIVE, ContainerAttr);
                SfBaseUtils.UpdateDictionary(ARIA_HAS_POPUP, TRUE, ContainerAttr);
                SfBaseUtils.UpdateDictionary(ARIA_ACTIVE_DESCENDANT, NULL_VALUE, ContainerAttr);
                SfBaseUtils.UpdateDictionary(ARIA_OWN, ID + OPTIONS, ContainerAttr);
                SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, ContainerAttr);
                SfBaseUtils.UpdateDictionary(ROLE, LIST_BOX, ContainerAttr);
                SfBaseUtils.UpdateDictionary(AUTO_CORRECT, OFF, ContainerAttr);
                SfBaseUtils.UpdateDictionary(SPELL_CHECK, FALSE, ContainerAttr);
                SfBaseUtils.UpdateDictionary(AUTO_CAPITAL, OFF, ContainerAttr);
                SfBaseUtils.UpdateDictionary(AUTO_COMPLETE, OFF, ContainerAttr);
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            if (InputBaseObj != null)
            {
                var options = GetProperty();
                await InvokeMethod("sfBlazor.DropDownList.initialize", new object[] { InputBaseObj.ContainerElement, InputBaseObj.InputElement, DotnetObjectReference, options });
                IsDevice = SyncfusionService.IsDeviceMode;
            }
        }

        /// <summary>
        /// Method which updates the dependency scripts.
        /// </summary>
        /// <exclude/>
        protected void DependencyScripts()
        {
            ScriptModules = SfScriptModules.SfDropDownList;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
        }

        private async Task SetLocalStorage(string persistId, TValue dataValue)
        {
            string serializeValue = null;
           if (!IsSimpleDataType() && ((typeof(TValue)) == typeof(TItem)))
            {
                serializeValue = dataValue != null ? System.Text.Json.JsonSerializer.Serialize(dataValue) : null;
            }
            else
            {
                serializeValue = dataValue != null ? (string)SfBaseUtils.ChangeType(dataValue, typeof(string)) : null;
            }
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, serializeValue });
        }

        private async Task ContainerFocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (Enabled && !Readonly)
            {
                if (InputBaseObj != null)
                {
                    await InputBaseObj.OnFocusHandler(args);
                }

                UpdateParentClass(RootClass, ContainerClass);
                if (!IsInternalFocus)
                {
                    await SfBaseUtils.InvokeEvent<object>(DropdownlistEvents?.Focus, null);
                }
            }
        }

        /// <summary>
        /// Triggers when the component get focused out.
        /// </summary>
        /// <param name="args">Specifies the FocusEventArgs arguments.</param>
        /// <param name="isFilterinput">Specifies whether it is a filter input or not.</param>
        /// <returns>Task.</returns>
        protected async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args, bool isFilterinput = false)
        {
            if (InputBaseObj != null && !isFilterinput)
            {
                await InputBaseObj.OnBlurHandler(args);
            }

            if (IsListRender && IsDropDownList && AllowFiltering && isFilterinput && IsDevice)
            {
                return;
            }

            if (IsListRender && !(AllowFiltering && !isFilterinput) && !IsFilterClearClicked)
            {
                await Hide();
            }

            IsDropDownClick = false;
            await OnChangeEvent();
            if (!isFilterinput && !IsListRender)
            {
                IsInternalFocus = false;
                await SfBaseUtils.InvokeEvent<object>(DropdownlistEvents?.Blur, null);
            }
        }

        private async Task ClientPopupRender()
        {
            if (ShowPopupList && IsListRendered)
            {
                IsListRendered = false;
                var options = GetProperty();
                IsListRender = true;
                await InvokeMethod("sfBlazor.DropDownList.renderPopup", new object[] { InputBaseObj?.InputElement, PopupElement, PopupHolderEle, PopupEventArgs, IsModifiedPopup, options, GetItemData() });
            }
        }

        /// <summary>
        /// Triggers while clicking the dropdown icon.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task DropDownClick()
        {
            if (Enabled && !Readonly && !(IsFilterClearClicked && ComponentName == nameof(SfDropDownList<TValue, TItem>)))
            {
                PreventContainer = IsDropDownList;
                if (IsListRender)
                {
                    await HidePopup();
                    IsInternalFocus = true;
                    await FocusIn();
                }
                else
                {
                    await FocusIn();
                    IsDropDownClick = true;

                    await Task.Delay(10);
                    await ShowPopup();
                }

                if (!IsDevice)
                {
                    PreventIconHandler = true;
                }
            }
            else
            {
                IsFilterClearClicked = false;
            }
        }

        /// <summary>
        /// Task which used to hide popup.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task HidePopup()
        {
            if (!(IsDropDownClick && IsDevice))
            {
                if (IsEscapeKey)
                {
                    DropdownValue = DDLText;
                    if (InputBaseObj != null)
                    {
                        await InputBaseObj.SetValue(DropdownValue, FloatLabelType, ShowClearButton);
                    }

                    ItemData = GetDataByText(DDLText);
                    ActiveIndex = Index;
                    IsEscapeKey = false;
                }

                PopupEventArgs = await InvokePopupEvents(false);
                if (!PopupEventArgs.Cancel)
                {
                    var options = GetProperty();
                    IsListRender = false;
                    await InvokeMethod("sfBlazor.DropDownList.closePopup", new object[] { InputBaseObj?.InputElement, PopupEventArgs, options });
                    await InvokeAfterClosed();
                }
            }

            IsDropDownClick = false;
        }

        internal DropDownClientProperties GetProperty()
        {
            return new DropDownClientProperties
            {
                EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                ZIndex = ZIndex,
                PopupHeight = PopupHeight,
                PopupWidth = PopupWidth,
                Width = Width,
                AllowFiltering = AllowFiltering,
                EnableVirtualization = EnableVirtualization,
                ModuleName = ComponentName
            };
        }

        /// <summary>
        /// Specifies the open event.
        /// </summary>
        /// <returns>PopupEventArgs arguments.</returns>
        /// <exclude/>
        protected PopupEventArgs OpenEventArgs()
        {
            return new PopupEventArgs()
            {
                Cancel = false,
                Popup = new PopupModel()
                {
                    Collision = new CollisionAxis() { X = CollisionType.Flip, Y = CollisionType.Flip },
                    Position = new PositionDataModel() { X = "left", Y = "bottom" },
                    RelateTo = InputBaseObj != null ? InputBaseObj.ContainerElement : default(ElementReference),
                    TargetType = TargetType.Relative
                }
            };
        }

        /// <summary>
        /// Invokes the popup event.
        /// </summary>
        /// <param name="isOpen">True if the popup is in open state otherwise false.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task<PopupEventArgs> InvokePopupEvents(bool isOpen)
        {
            if (ContainerAttr != null)
            {
                SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, isOpen ? TRUE : FALSE, ContainerAttr);
            }

            var popupEvents = OpenEventArgs();
            await SfBaseUtils.InvokeEvent<PopupEventArgs>(isOpen ? DropdownlistEvents?.Opened : DropdownlistEvents?.OnClose, popupEvents);
            return popupEvents;
        }

        /// <summary>
        /// Triggered when clear button is clicked.
        /// </summary>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <param name="isFilterClear">Specifies whether it is filterinput clear icon or not.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task InvokeClearBtnEvent(EventArgs args, bool isFilterClear = false)
        {
            if (FilterinputObj != null && isFilterClear)
            {
                await FilterClear();
            }
            else
            {
                await ClearAll(args);
            }
        }

        /// <summary>
        /// Triggers when filter clear icon is clicked.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task FilterClear()
        {
            if (!IsDevice)
            {
                FilterClearBtnStopPropagation = true;
            }

            IsFilterClearClicked = true;
            FilterInputValue = null;
            TypedString = string.Empty;
            await FilterinputObj.SetValue(string.Empty, Inputs.FloatLabelType.Never, false);
            await Task.Delay(50); // Added delay for cleared the filtering conent in seach input.
            await SearchList();
            IsFilterInputChange = true;
        }

        /// <summary>
        /// Task which used to clear all the items.
        /// </summary>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <returns>Task.</returns>
        protected virtual async Task ClearAll(EventArgs args = null)
        {
            IsFilterClearClicked = true;
            await UpdateValueSelection(default);
            await UpdateListSelection(default, ITEM_FOCUS);
            TypedString = string.Empty;
            await OnChangeEvent(args);
            if (!IsDevice)
            {
                ClearBtnStopPropagation = true;
            }

            await Task.Delay(100); // Added delay for cleared the input value.
            if (IsListRender)
            {
                await RefreshPopup();
            }
        }

        /// <summary>
        /// Triggers when any value provided to filter input.
        /// </summary>
        /// <param name="args">Specifies the ChangeEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task FilterInputHandler(ChangeEventArgs args)
        {
            if (ComponentName == nameof(SfDropDownList<TValue, TItem>))
            {
                FilterinputObj.ClearIconClass = SfBaseUtils.AddClass(FilterinputObj.ClearIconClass, "e-input-group-icon");
            }

            IsValidKey = true;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Triggers when paste action is performes.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task PasteHandler()
        {
            if (IsFilter())
            {
                await Task.Delay(100);    // Added delay for get the pasted content in seach input.
                TypedString = FilterinputObj?.InputTextValue;
                await SearchList(null);
            }
        }

        /// <summary>
        /// Specifies the keyup event of filter event.
        /// </summary>
        /// <param name="args">Specifies the KeyboardEventArgs arguments.</param>
        /// <returns>Task.</returns>
        protected async Task OnFilterUp(KeyboardEventArgs args)
        {
            if (args != null && !(args.CtrlKey && args.Code == "KeyV") && (IsValidKey || args.Code == "ArrowDown" || args.Code == "ArrowUp"))
            {
                IsValidKey = false;
                switch (args.Code)
                {
                    case "ArrowDown":
                    case "ArrowUp":
                        await UpdateArrowKeyAction(args);
                        break;
                    case "Delete":
                    case "Backspace":
                        await UpdateRemoveKeyAction(args);
                        break;
                    default:
                        TypedString = FilterinputObj?.InputTextValue;
                        if (ComponentName == nameof(SfAutoComplete<TValue, TItem>) && string.IsNullOrEmpty(InputBaseObj?.InputTextValue))
                        {
                            await HidePopup();
                        }

                        PreventAutoFill = false;
                        await SearchList(args);
                        IsFilterInputChange = true;
                        break;
                }
            }
            else
            {
                IsValidKey = false;
            }
        }

        private async Task UpdateArrowKeyAction(KeyboardEventArgs args)
        {

            if (ComponentName == nameof(SfAutoComplete<TValue, TItem>) && !IsListRender && !PreventAltUp)
            {
                PreventAutoFill = true;
                await SearchList(args);
            }
            else
            {
                PreventAutoFill = false;
            }

            PreventAltUp = false;
        }
        private async Task UpdateRemoveKeyAction(KeyboardEventArgs args)
        {
            TypedString = FilterinputObj?.InputTextValue;
            if (ComponentName != nameof(SfDropDownList<TValue, TItem>) && InputBaseObj != null)
            {
                ItemData = GetDataByText(TypedString);
                await UpdateListSelection(ItemData, SELECTED);
                if (string.IsNullOrEmpty(InputBaseObj.InputTextValue))
                {
                    await UpdateInputValue(string.Empty);
                }
            }

            if (((!IsListRender && !string.IsNullOrEmpty(TypedString)) || IsListRender) || (ComponentName != nameof(SfAutoComplete<TValue, TItem>) && string.IsNullOrEmpty(TypedString)))
            {
                PreventAutoFill = true;
                await SearchList(args);
            }
            else if (string.IsNullOrEmpty(TypedString) && ComponentName == nameof(SfAutoComplete<TValue, TItem>))
            {
                await HidePopup();
            }
        }

        /// <summary>
        /// Task which specifies the search list.
        /// </summary>
        /// <param name="args">Specifies the KeyboardEventArgs arguments.</param>
        /// <returns>Task.</returns>
        protected virtual async Task SearchList(KeyboardEventArgs args = null)
        {
            IsTyped = true;
            ActiveIndex = null;
            if (AllowFiltering)
            {
                var filterEventArgs = new FilteringEventArgs()
                {
                    BaseEventArgs = args,
                    Cancel = false,
                    PreventDefaultAction = false,
                    Text = TypedString
                };
                await SfBaseUtils.InvokeEvent<FilteringEventArgs>(DropdownlistEvents?.Filtering, filterEventArgs);
                if (!filterEventArgs.Cancel && !filterEventArgs.PreventDefaultAction)
                {
                    await FilteringAction(DataSource, Query, Fields);
                }
            }
        }

        /// <summary>
        /// Task which specifies the filtering action.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <param name="fields">Specifies the fields.</param>
        /// <returns>Task.</returns>
        protected async Task FilteringAction(IEnumerable<TItem> dataSource, Query query, FieldSettingsModel fields)
        {
            if (FilterinputObj != null)
            {
                BeforePopupOpen = true;
                query = string.IsNullOrEmpty(TypedString?.Trim()) ? null : query;
                if (SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)) && string.IsNullOrEmpty(TypedString))
                {
                    ListData = MainData;
                    await RenderItems();
                }
                else
                {
                    await SetListData(dataSource, fields, query);
                }

                await ListFocus();
            }
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
                await CreatePopup();
            }
        }

        /// <summary>
        /// Task used to refresh the popup list items.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task RefreshPopup()
        {
            if (InputBaseObj != null)
            {
                await InvokeMethod("sfBlazor.DropDownList.refreshPopup", new object[] { InputBaseObj.InputElement });
            }
        }

        /// <summary>
        /// Task which gets the query.
        /// </summary>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Query.</returns>
        /// <exclude/>
        protected override Query GetQuery(Query query)
        {
            Query filterQuery = new Query();
            if (AllowFiltering && FilterinputObj != null && !IsCustomFilter)
            {
                filterQuery = (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
                string filterType = string.IsNullOrEmpty(TypedString) ? "contains" : FilterType.ToString().ToLower(CultureInfo.CurrentCulture);
                var isSimpleDataType = !DataManager.IsDataManager && IsSimpleDataType();
                string fields = (!string.IsNullOrEmpty(Fields?.Text) && !isSimpleDataType) ? Fields.Text : string.Empty;
                var filterValue = !string.IsNullOrEmpty(TypedString) ? TypedString : string.Empty;
                if (fields.Split(new char[] { '.' }).Length > 1)
                {
                    List<WhereFilter> whereFilters = new List<WhereFilter>();
                    whereFilters.Add(new WhereFilter() { Field = fields, Operator = filterType, value = filterValue, IgnoreCase = IgnoreCase, IgnoreAccent = IgnoreAccent });
                    filterQuery.Where(new WhereFilter() { Condition = "or", IsComplex = true, predicates = whereFilters });
                }
                else
                {
                    filterQuery.Where(new WhereFilter() { Field = fields, Operator = filterType, value = filterValue, IgnoreCase = IgnoreCase, IgnoreAccent = IgnoreAccent });
                }
            }
            else
            {
                filterQuery = (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
            }

            return filterQuery;
        }

        private async Task ClosePopupAction()
        {
            if (IsDropDownClick)
            {
                IsInternalFocus = true;
                await FocusIn();
            }
            else if (ComponentName != nameof(SfAutoComplete<TValue, TItem>) && IsFirstRenderPopup && AllowFiltering)
            {
                if (SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)) && MainData != null && ItemData != null && MainData.IndexOf(ItemData) < 0 && Value != null)
                {
                    var newItem = new List<TItem>(MainData);
                    newItem.Add(ItemData);
                    MainData = newItem;
                }

                ListData = MainData;
                await RenderItems();
            }

            PreventAutoFill = false;
            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ICON_ANIM).Trim();
            IsListRender = false;
            ShowPopupList = false;
            BeforePopupOpen = false;
            await InvokeAsync(() => StateHasChanged());
            var dataItem = GetItemData();
            if (string.IsNullOrEmpty(InputBaseObj.InputTextValue) && !SfBaseUtils.Equals(InputBaseObj.InputTextValue, dataItem.Text))
            {
                await ClearAll();
            }

            IsFilterClearClicked = false;
            FilterinputObj = null;
            TypedString = null;
        }

        /// <summary>
        /// Task which specifies whether filter action is allowed or not.
        /// </summary>
        /// <returns>bool.</returns>
        /// <exclude/>
        protected override bool IsFilter()
        {
            return AllowFiltering;
        }

        /// <summary>
        /// Triggers before popup get opened.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task<BeforeOpenEventArgs> InvokeBeforeOpen()
        {
            var beforeOpenArgs = new BeforeOpenEventArgs() { Cancel = false };
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, TRUE, ContainerAttr);
            await SfBaseUtils.InvokeEvent<BeforeOpenEventArgs>(DropdownlistEvents?.OnOpen, beforeOpenArgs);
            return beforeOpenArgs;
        }

        /// <summary>
        /// Triggers after the popup get closed.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task<ClosedEventArgs> InvokeAfterClosed()
        {
            var closedArgs = new ClosedEventArgs() { };
            await SfBaseUtils.InvokeEvent<ClosedEventArgs>(DropdownlistEvents?.Closed, closedArgs);
            return closedArgs;
        }
        /// <summary>
        /// Opens the popup that displays the list of items.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task CreatePopup() 
        {
            var beforeOpenArgs = await InvokeBeforeOpen();
            if (!beforeOpenArgs.Cancel)
            {
                if ((ListData == null || !ListData.Any()) && !BeforePopupOpen)
                {
                    await Render(DataSource, Fields, Query);
                }

                BeforePopupOpen = true;
                PopupEventArgs = new PopupEventArgs() { Cancel = false };
                PopupEventArgs = await InvokePopupEvents(true);
                var openEventArgs = OpenEventArgs();
                IsModifiedPopup = false;
                var oldString = System.Text.Json.JsonSerializer.Serialize(PopupEventArgs.Popup);
                var newString = System.Text.Json.JsonSerializer.Serialize(openEventArgs.Popup);
                IsModifiedPopup = !string.Equals(oldString, newString, StringComparison.Ordinal);
                if (!PopupEventArgs.Cancel)
                {
                    UpdatePopupState();
                    if (ShowPopupList && !IsFirstRenderPopup && AllowFiltering && !IsAutoComplete)
                    {
                        IsFirstRenderPopup = true;
                        MainData = ListData;
                    }

                    await ListFocus();
                    await InvokeAsync(() => StateHasChanged());
                }
            }
            else
            {
                BeforePopupOpen = false;
            }
        }
        /// <summary>
        /// Opens the popup that displays the list of items.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ShowPopup()
        {
            if (IsAutoComplete || IsComboBox)
            {
                TypedString = FilterinputObj?.InputTextValue;
            }

            if (IsAutoComplete)
            {
                await Render(DataSource, Fields, Query);
            }
            await CreatePopup();
        }

        private async Task ListFocus()
        {
            if (ListData != null)
            {
                TItem selectData = IsAutoComplete ? default : ListData.FirstOrDefault();
                var className = ITEM_FOCUS;
                if (ItemData != null)
                {
                    var itemIndex = (IsSimpleDataType() || Fields == null) ? ListData.IndexOf(ItemData) : ListData.Select(item => DataUtil.GetObject(Fields.Value, item)).IndexOf(DataUtil.GetObject(Fields.Value, ItemData));
                    selectData = ((itemIndex >= 0) || IsAutoComplete) ? ItemData : ListData.FirstOrDefault();
                    className = itemIndex >= 0 ? SELECTED : ITEM_FOCUS;
                }
                await UpdateListSelection(selectData, className);
            }
        }

        /// <summary>
        /// Method which updates the popup state.
        /// </summary>
        /// <exclude/>
        protected void UpdatePopupState()
        {
            IsListRendered = true;
            ShowPopupList = true;
            if (!IsDevice)
            {
                IsDropDownClick = false;
            }

            ContainerClass = SfBaseUtils.AddClass(ContainerClass, ICON_ANIM);
        }

        /// <summary>
        /// Update the parent component class to the element.
        /// </summary>
        /// <param name="rootClass">Update the class to input element.</param>
        /// <param name="containerClass">Update the class to container element.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateParentClass(string rootClass, string containerClass)
        {
            RootClass = rootClass;
            ContainerClass = containerClass;
            UpdateValidateClass();
        }

        /// <summary>
        /// Update the dropdownlist fileds.
        /// </summary>
        /// <param name="fieldValue">Specifies the field value.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void UpdateChildProperties(object fieldValue)
        {
            var fields = (DropDownListFieldSettings)fieldValue;
            Fields = new FieldSettingsModel() { GroupBy = fields?.GroupBy, HtmlAttributes = fields?.HtmlAttributes, IconCss = fields?.IconCss, Text = fields?.Text, Value = fields?.Value };
            SetFields();
        }

        /// <summary>
        /// Task updates the input value.
        /// </summary>
        /// <param name="inputValue">Specifies the input value.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task UpdateInputValue(string inputValue)
        {
            DropdownValue = inputValue;
            if (InputBaseObj != null)
            {
                await InputBaseObj.SetValue(DropdownValue, FloatLabelType, ShowClearButton);
            }
        }

        /// <summary>
        /// Triggers when mouse click is performed.
        /// </summary>
        /// <param name="args">Specifies the ListOptions arguments.</param>
        /// <param name="eventArgs">Specifies the MouseEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task OnMouseClick(ListOptions<TItem> args, MouseEventArgs eventArgs)
        {
            if (args != null && args.CurItemData != null && !args.IsHeader)
            {
                KeyAction = null;
                await UpdateSelectedItem(args.CurItemData, eventArgs);
                UpdateValidateClass();
                if (IsListRender)
                {
                    IsDropDownClick = false;
                    await HidePopup();
                    IsInternalFocus = true;
                    await FocusIn();
                }
            }
        }

        /// <summary>
        /// Triggers when item get selected.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task<SelectEventArgs<TItem>> InvokeSelectEvent(TItem item, EventArgs args = null)
        {
            var selectEventArgs = new SelectEventArgs<TItem>()
            {
                Cancel = false,
                IsInteracted = args != null,
                Item = null,
                ItemData = item,
                E = args
            };
            await SfBaseUtils.InvokeEvent<SelectEventArgs<TItem>>(DropdownlistEvents?.OnValueSelect, selectEventArgs);
            return selectEventArgs;
        }

        /// <summary>
        /// Task which updates the selected item.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <param name="keyArgs">Specifies the KeyActions arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task UpdateSelectedItem(TItem item, EventArgs args = null, KeyActions keyArgs = null)
        {
            ItemData = item;
            var selectEventArgs = await InvokeSelectEvent(item, args);
            if (!selectEventArgs.Cancel)
            {
                await UpdateSelectItem(item, args, keyArgs);
            }
        }

        /// <summary>
        /// Task which updates the selected item.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <param name="keyArgs">Specifies the KeyActions arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task UpdateSelectItem(TItem item, EventArgs args = null, KeyActions keyArgs = null)
        {
            ItemData = item;
            var listItems = ListDataSource?.Where(list => !list.IsHeader)?.ToList();
            ActiveIndex = listItems?.IndexOf(listItems.Where(listiem => SfBaseUtils.Equals(listiem.CurItemData, ItemData)).FirstOrDefault());
            await UpdateListSelection(ItemData, SELECTED);
            await SetSelectOptions(args, keyArgs);
        }

        /// <summary>
        /// Task which update list selection.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="className">Specifies the class name.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task UpdateListSelection(TItem item, string className)
        {
            if (ListDataSource != null)
            {
                var selectedData = ListDataSource.Where(listItem => listItem.ListClass.Contains(SELECTED, StringComparison.CurrentCulture)).FirstOrDefault();
                var focusedData = ListDataSource.Where(listItem => listItem.ListClass.Contains(ITEM_FOCUS, StringComparison.CurrentCulture)).FirstOrDefault();
                if (selectedData != null)
                {
                    selectedData.ListClass = SfBaseUtils.RemoveClass(selectedData.ListClass, SELECTED);
                }

                if (focusedData != null)
                {
                    focusedData.ListClass = SfBaseUtils.RemoveClass(focusedData.ListClass, ITEM_FOCUS);
                }

                foreach (var listItem in ListDataSource)
                {
                    if (SfBaseUtils.Equals(listItem.CurItemData, item))
                    {
                        listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, className);
                        break;
                    }
                }

                if (KeyAction != null)
                {
                    StateHasChanged();
                    await InvokeMethod("sfBlazor.DropDownList.updateScrollPosition", new object[] { InputBaseObj?.InputElement, KeyAction });
                }
            }
        }

        private async Task SetSelectOptions(EventArgs args = null, KeyActions keyArgs = null)
        {
            await SetValue();
            if (keyArgs == null || !IsListRender)
            {
                await OnChangeEvent(args);
            }
        }

        /// <summary>
        /// Task which sets the value.
        /// </summary>
        /// <param name="items">Specifies the data item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task SetValue(DataItems<TValue> items = null)
        {
            var dataItem = items != null ? items : GetItemData();
            var isValueTemplate = false;
            if (ValueTemplate != null && InputBaseObj != null && ItemData != null)
            {
                isValueTemplate = true;
                InputBaseObj.ValueTemplate = ValueTemplate(ItemData);
            }

            DropdownValue = dataItem?.Text?.ToString();
            if (InputBaseObj != null)
            {
                await InputBaseObj.SetValue(DropdownValue, FloatLabelType, ShowClearButton, isValueTemplate);
            }
        }

        /// <summary>
        /// Triggers when value get changed.
        /// </summary>
        /// <param name="args">Specifies EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task InvokeChangeEvent(EventArgs args = null)
        {
            var changeEventArgs = new ChangeEventArgs<TValue, TItem>()
            {
                Value = Value,
                ItemData = ItemData,
                PreviousItemData = PreviousItemData,
                Cancel = false,
                IsInteracted = args != null
            };
            await SfBaseUtils.InvokeEvent<ChangeEventArgs<TValue, TItem>>(DropdownlistEvents?.ValueChange, changeEventArgs);
        }

        /// <summary>
        /// Triggers when value get changed.
        /// </summary>
        /// <param name="args">Specifies EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task OnChangeEvent(EventArgs args = null)
        {
            var dataItem = GetItemData();
            if (!SfBaseUtils.Equals(PreviousValue, dataItem.Value))
            {
                await UpdateItemValue(dataItem);
                await InvokeChangeEvent(args);
                if (EnablePersistence)
                {
                    await SetLocalStorage(ID, Value);
                }

                PreviousValue = Value;
                PreviousItemData = ItemData;
            }
        }

        /// <summary>
        /// Method which gets item data.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected DataItems<TValue> GetItemData(TItem item = default(TItem))
        {
            var isDefault = SfBaseUtils.Equals(item, default(TItem));
            var dataItem = (item != null && !isDefault) ? item : ItemData;
            TValue dataValue = default;
            object dataText = default;
            var isModelValue = typeof(TValue) == typeof(TItem);
            if (dataItem != null)
            {
                Type dataType = typeof(TValue);
                if (IsSimpleDataType())
                {
                    dataValue = (TValue)SfBaseUtils.ChangeType(dataItem, dataType);
                    dataText = dataItem;
                }
                else
                {
                    dataValue = isModelValue ? (TValue)SfBaseUtils.ChangeType(dataItem, dataType) : (TValue)SfBaseUtils.ChangeType(DataUtil.GetObject(Fields?.Value, dataItem), dataType);
                    dataText = DataUtil.GetObject(Fields?.Text, dataItem);
                }
            }

            return new DataItems<TValue>() { Value = dataValue, Text = dataText };
        }

        /// <summary>
        /// Task which performs the action begin event.
        /// </summary>
        /// <param name="dataSource">Specifies the datasource.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<ActionBeginEventArgs> ActionBegin(IEnumerable<TItem> dataSource, Query query = null)
        {
            var actionBeginArgs = new ActionBeginEventArgs
            {
                Cancel = false,
                //Data = dataSource,
                Query = query,
                EventName = "ActionBegin"
            };
            await SfBaseUtils.InvokeEvent<ActionBeginEventArgs>(DropdownlistEvents?.OnActionBegin, actionBeginArgs);
            await ShowSpinner();
            if (FilterinputObj != null && FilterinputObj.SpinnerObj != null && SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)))
            {
                await FilterinputObj.SpinnerObj.ShowAsync();
            }

            return actionBeginArgs;
        }

        /// <summary>
        /// Task which performs the action complete event.
        /// </summary>
        /// <param name="dataSource">Specifies the datasource.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task ActionComplete(IEnumerable<TItem> dataSource, Query query = null)
        {
            var actioncompleteArgs = new ActionCompleteEventArgs<TItem>
            {
                Cancel = false,
                Result = dataSource,
                Query = query,
                Count = dataSource != null ? dataSource.ToList().Count : 0,
                EventName = "ActionComplete"
            };
            await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(DropdownlistEvents?.OnActionComplete, actioncompleteArgs);
            if (!actioncompleteArgs.Cancel)
            {
                ListData = actioncompleteArgs.Result;
                await RenderEqualItems();
                await RenderItems();
                await HideSpinner();
                if (FilterinputObj != null && FilterinputObj.SpinnerObj != null && IsDropDownList)
                {
                    await FilterinputObj.SpinnerObj.HideAsync();
                }
            }
        }
        /// <summary>
        /// Update the custom value to the list
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task RenderEqualItems()
        {
            if (this.Value != null && !IsSimpleDataType() && Fields != null)
            {
                var isCheckValue = default(TItem);
                var isComplex = typeof(TValue) == typeof(TItem);
                var dataValue = isComplex ? DataUtil.GetObject(Fields.Value, Value) : Value;
                if (ListData != null)
                {
                    isCheckValue = ListData.Where(item => SfBaseUtils.Equals(DataUtil.GetObject(Fields.Value, item), dataValue)).FirstOrDefault();
                }
                if (isCheckValue == null)
                {
                    var predicate = new WhereFilter() { Field =  Fields.Value, Operator = "equal", value = dataValue };
                    var valueQuery = GetQuery(Query).Where(predicate);
                    var data = (DataManager != null) ? await DataManager.ExecuteQuery<TItem>(valueQuery) : null;
                    var resultData = new List<TItem>();
                    if (DataManager != null && GetQuery(Query).IsCountRequired)
                    {
                        TotalCount = ((DataResult)data).Count;
                        var dataResult = (((DataResult)data).Result == null) ? new List<object>() : ((DataResult)data).Result;
                        resultData = ((IEnumerable<object>)dataResult).Cast<TItem>().ToList();
                    }
                    else
                    {
                        var dataResult = (data == null) ? new List<object>() : data;
                        resultData = (dataResult as IEnumerable)?.Cast<TItem>()?.ToList();
                    }
                    ListData = ListData?.Concat(resultData);
                }
            }
        }

        /// <summary>
        /// Task which performs the action failure event.
        /// </summary>
        /// <param name="args">Specifies the object arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task ActionFailure(object args)
        {
            await HideSpinner();
            if (FilterinputObj != null && FilterinputObj.SpinnerObj != null && SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)))
            {
                await FilterinputObj.SpinnerObj.HideAsync();
            }

            await SfBaseUtils.InvokeEvent<Exception>(DropdownlistEvents?.OnActionFailure, (Exception)args);
        }

        /// <summary>
        /// Specifies whether it is edit textbox.
        /// </summary>
        /// <returns>bool.</returns>
        /// <exclude/>
        protected virtual bool IsEditTextBox()
        {
            return false;
        }

        private async Task PreventKeyAction(KeyActions args)
        {
            var isTabAction = args.Action == "tab" || args.Action == "close";
            if ((ListData == null || !ListData.Any()) && !isTabAction && args.Action != "escape")
            {
                await Render(DataSource, Fields, Query);
            }
        }
        private async Task TabKeyAction(KeyActions args)
        {
            if (ComponentName == nameof(SfAutoComplete<TValue, TItem>))
            {
                await SelectCurrentItem(args);
            }
            else if (IsListRender)
            {
                await HidePopup();
            }
        }
        private async Task HideKeyAction(KeyActions args)
        {
            IsEscapeKey = args.Action == "escape";
            PreventAltUp = IsListRender;
            if (IsListRender)
            {
                await HidePopup();
                if (args.Action != "close")
                {
                    IsInternalFocus = true;
                    await FocusIn();
                }
            }
        }
        private async Task KeyboardActionHandler(KeyActions args)
        {
            if (Enabled)
            {
                KeyAction = args;
                var preventAction = args.Action == "pageUp" || args.Action == "pageDown";
                var isHomeEnd = args.Action == "home" || args.Action == "end";
                var preventHomeEnd = ComponentName != nameof(SfDropDownList<TValue, TItem>) && isHomeEnd;
                var isNavigation = args.Action == "down" || args.Action == "up" || preventAction || isHomeEnd;
                if ((IsEditTextBox() || preventAction || preventHomeEnd) && !ShowPopupList)
                {
                    return;
                }

                if (!Readonly)
                {
                    await PreventKeyAction(args);
                    if (ListData != null && isNavigation && !ListData.Any())
                    {
                        return;
                    }
                    switch (args.Action)
                    {
                        case "down":
                        case "up":
                            await UpdateUpDownAction(args);
                            break;
                        case "pageDown":
                        case "pageUp":
                            await PageUpDownSelection(args);
                            break;
                        case "home":
                        case "end":
                            await UpdateHomeEndAction(args);
                            break;
                        case "space":
                            if (SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)) && !IsListRender)
                            {
                                await ShowPopup();
                            }

                            break;
                        case "open":
                            if (!IsListRender)
                            {
                                await ShowPopup();
                            }

                            break;
                        case "enter":
                            await SelectCurrentItem(args);
                            UpdateValidateClass();
                            break;
                        case "tab":
                            await TabKeyAction(args);
                            break;
                        case "escape":
                        case "close":
                        case "hide":
                            await HideKeyAction(args);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Task which specifies the custom value.
        /// </summary>
        /// <param name="isValidateInput">Specifies whether it is validate on input event.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task CustomValue(bool isValidateInput = false)
        {
            await Task.CompletedTask;
        }

        private async Task SelectCurrentItem(KeyActions args)
        {
            if (IsListRender)
            {
                FocusedData = ListDataSource.Where(listItem => listItem.ListClass.Contains(ITEM_FOCUS, StringComparison.Ordinal)).FirstOrDefault();
                if (FocusedData != null)
                {
                    await UpdateSelectedItem(FocusedData.CurItemData, args.Events, args);
                } else {
                    await CustomValue();
                }

                await OnChangeEvent(args.Events);
                await HidePopup();
                if (args.Action != "tab")
                {
                    IsInternalFocus = true;
                    await FocusIn();
                }
            }
            else if (SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)))
            {
                await ShowPopup();
            }
        }

        private async Task PageUpDownSelection(KeyActions args)
        {
            if (IsListRender)
            {
                ActiveIndex = ActiveIndex == null ? 0 : ActiveIndex;
                var listItems = ListDataSource?.Where(list => !list.IsHeader)?.ToList();
                int pageCount = await InvokeMethod<int>("sfBlazor.DropDownList.getPageCount", false, new object[] { PopupElement });
                ListOptions<TItem> previousItem;
                if (args.Action == "pageUp")
                {
                    int step = (int)ActiveIndex - pageCount;
                    previousItem = (step >= 0) ? listItems.ElementAtOrDefault(step + 1) : listItems.FirstOrDefault();
                }
                else
                {
                    int step = (int)ActiveIndex + pageCount;
                    previousItem = (step <= listItems.Count) ? listItems.ElementAtOrDefault(step - 1) : listItems.LastOrDefault();
                }

                await UpdateSelectedItem(previousItem.CurItemData, null, args);
            }
        }

        private async Task UpdateUpDownAction(KeyActions args)
        {
            var listItems = ListDataSource?.Where(list => !list.IsHeader)?.ToList();
            ListOptions<TItem> nextItem;
            var focusedItem = ListDataSource.Where(listItem => listItem.ListClass.Contains(ITEM_FOCUS, StringComparison.CurrentCulture))?.FirstOrDefault();
            if (focusedItem != null && !IsAutoComplete)
            {
                await UpdateSelectedItem(focusedItem.CurItemData, null, args);
            }
            else
            {
                var findIndex = ActiveIndex;
                findIndex = findIndex != null ? findIndex : 0;
                var index = args.Action == "down" ? findIndex + 1 : findIndex - 1;
                var startIndex = 0;
                if (IsAutoComplete && listItems != null)
                {
                    startIndex = (args.Action == "down" && ActiveIndex == null) ? 0 : listItems.Count - 1;
                    index = index < 0 ? listItems.Count - 1 : index == listItems.Count ? 0 : index;
                }
                index = ActiveIndex == null ? startIndex : index;
                ActiveIndex = index = index < 0 ? 0 : index > listItems.Count - 1 ? listItems.Count - 1 : index; 
                nextItem = listItems?.ElementAtOrDefault((int)index);
                if (nextItem != null)
                {
                    if (IsAutoComplete)
                    {
                        await UpdateAutoFillOnDown(nextItem.CurItemData);
                        await UpdateFocusItem(nextItem.CurItemData);
                    }
                    else
                    {
                        await UpdateSelectedItem(nextItem.CurItemData, null, args);
                    }
                }
            }
        }

        /// <summary>
        /// Task which update auto fill on down action.
        /// </summary>
        /// <param name="curItem">Specifies the current item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task UpdateAutoFillOnDown(TItem curItem)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Task which update the focus item.
        /// </summary>
        /// <param name="focusItem">Specifies the focus item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task UpdateFocusItem(TItem focusItem = default)
        {
            await Task.CompletedTask;
        }

        private async Task UpdateHomeEndAction(KeyActions args)
        {
            if (SfBaseUtils.Equals(ComponentName, nameof(SfDropDownList<TValue, TItem>)))
            {
                var allItems = ListDataSource.Where(list => !list.IsHeader).ToList();
                int findItem = (args.Action == "home") ? 0 : allItems.Count - 1;
                var focusItem = (args.Action == "home") ? allItems.FirstOrDefault() : allItems.LastOrDefault();
                if (ActiveIndex != findItem && focusItem != null)
                {
                    await UpdateSelectedItem(ListDataSource.ElementAtOrDefault(findItem).CurItemData, null, args);
                }
            }
        }
        /// <summary>
        /// Update the validation class to the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected void UpdateValidateClass()
        {
            if (ValueExpression != null && DropDownsEditContext != null)
            {
                var fieldIdentifier = FieldIdentifier.Create(ValueExpression);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + ValidClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, ValidClass + " ") : ContainerClass;
                ValidClass = DropDownsEditContext.FieldCssClass(fieldIdentifier);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.AddClass(ContainerClass, ValidClass) : ContainerClass;
                this.ContainerClass = Regex.Replace(this.ContainerClass, @"\s+", " ");
                if (ValidClass == INVALID || ValidClass == MODIFIED_INVALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
                }
                else if (ValidClass == MODIFIED_VALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, SUCCESS_CLASS);
                }
            }
        }

        /// <summary>
        /// Triggers when search keypress event is performed.
        /// </summary>
        /// <param name="args">Specifies the KeyboardEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task OnSearch(KeyboardEventArgs args)
        {
            if (args != null && args.Key != "Enter" && args.Code != "Space")
            {
                if (ListData == null || !ListData.Any())
                {
                    await Render(DataSource, Fields, Query);
                }

                if (ListData != null && ListData.Any() && Enabled && !Readonly)
                {
                    var searchData = IncrementalSearch(args.Key, ListData, Index, IgnoreCase);
                    if (searchData != null)
                    {
                        await UpdateSelectedItem(searchData, args);
                    }

                    await Task.Delay(50);
                    await InvokeMethod("sfBlazor.DropDownList.updateScrollPosition", new object[] { InputBaseObj?.InputElement, new KeyActions() { Action = null } });
                }
            }
        }

        internal override void UpdateDropDownTemplate(string name, RenderFragment template, RenderFragment<TItem> dataTemplate = null, RenderFragment<ComposedItemModel<TItem>> groupTemp = null)
        {
            switch (name)
            {
                case nameof(NoRecordsTemplate):
                    NoRecordsTemplate = template;
                    break;
                case nameof(ActionFailureTemplate):
                    ActionFailureTemplate = template;
                    break;
                case nameof(HeaderTemplate):
                    HeaderTemplate = template;
                    break;
                case nameof(FooterTemplate):
                    FooterTemplate = template;
                    break;
                case nameof(ItemTemplate):
                    ItemTemplate = dataTemplate;
                    break;
                case nameof(GroupTemplate):
                    GroupTemplate = groupTemp;
                    break;
                case nameof(ValueTemplate):
                    ValueTemplate = dataTemplate;
                    break;
            }
        }

        internal object GetValueByText(string text, bool ignoreCase, bool isTextByValue = false)
        {
            object valueText = default;
            var ignoreTextCase = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (ListData != null)
            {
                var data = ListData;
                var fields = Fields;
                if (IsSimpleDataType())
                {
                    var query = new Query().Where(new WhereFilter() { Field = string.Empty, Operator = "equals", value = text, IgnoreCase = ignoreCase });
                    valueText = SimpleDataExecute(data, query).FirstOrDefault();
                }
                else
                {
                    if (isTextByValue)
                    {
                        valueText = ListData.Where(item => DataUtil.GetObject(fields.Value, item) != null && DataUtil.GetObject(fields.Text, item).ToString().Equals(text, ignoreTextCase)).FirstOrDefault();
                    }
                    else
                    {
                        valueText = ListData.Where(item => DataUtil.GetObject(fields.Text, item).ToString().Equals(text, ignoreTextCase)).FirstOrDefault();
                    }
                }
            }

            return valueText;
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                var destroyArgs = new object[] { InputBaseObj?.InputElement, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, GetProperty() };
                InvokeMethod("sfBlazor.DropDownList.destroy", destroyArgs).ContinueWith(t =>
                {
                    _ = SfBaseUtils.InvokeEvent<object>(DropdownlistEvents?.Destroyed, null);
                }, TaskScheduler.Current);
                ObservableEventDisposed();
            }

            InputBaseObj = null;
            FilterinputObj = null;
            BackIcon = null;
            DropdownlistEvents = null;
            FilterAttributes = null;
            ContainerAttr = null;
            PopupEventArgs = null;
        }
    }
}

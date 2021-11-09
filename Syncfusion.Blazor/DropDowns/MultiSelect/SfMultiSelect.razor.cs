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
using Syncfusion.Blazor.Spinner;
using Syncfusion.Blazor.DropDowns;
using System.Reflection;
using System.Globalization;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Popups;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The MultiSelect component contains a list of predefined values from which a multiple value can be chosen.
    /// </summary>
    /// <typeparam name="TValue">Specifies the value type.</typeparam>
    /// <typeparam name="TItem">Specifies the type of SfMultiSelect.</typeparam>
    public partial class SfMultiSelect<TValue, TItem> : SfDropDownBase<TItem>, IDropDowns
    {
        private List<string> containerAttributes = new List<string>() { "title", "style", "class" };

        private Dictionary<string, object> inputAttr = new Dictionary<string, object>();

        private Dictionary<string, object> ContainerAttr { get; set; } = new Dictionary<string, object>();

        private string FloatLabel { get; set; }

        private ElementReference InputElement { get; set; }

        private ElementReference ContainerElement { get; set; }

        private ElementReference ChildContainerElement { get; set; }

        private ElementReference ViewElement { get; set; }

        private string InputValue { get; set; }

        private SfInputBase FilterinputObj { get; set; }

        private string RootClass { get; set; }

        private string SubContainer { get; set; }

        private string ContainerClass { get; set; }

        private string ChildContainerClass { get; set; }

        private List<string> BackIcon { get; set; }

        private string MultiselectValue { get; set; }

        private bool FilterClearBtnStopPropagation { get; set; }

        private bool PreventContainer { get; set; }

        private bool IsDevice { get; set; }

        private bool ShowPopupList { get; set; }

        private ElementReference PopupElement { get; set; }

        private ElementReference PopupHolderEle { get; set; }

        private ElementReference PopupContentEle { get; set; }

        private string PopupContainer { get; set; }

        private bool IsListRender { get; set; }

        private bool IsListRendered { get; set; }

        private bool IsDropDownClick { get; set; }

        private PopupEventArgs PopupEventArgs { get; set; }

        private bool IsValidKey { get; set; }

        private bool IsFilterClearClicked { get; set; }

        private bool IsCustomFilter { get; set; }

        private string FilterInputValue { get; set; }

        private bool IsFilterInputChange { get; set; }

        private bool IsInternalFocus { get; set; }

        internal MultiSelectEvents<TValue, TItem> MultiselectEvents { get; set; }

        private bool IsFirstRenderPopup { get; set; }

        private string ValidClass { get; set; }

        private Dictionary<string, object> FilterAttributes { get; set; }

        private TValue PreviousValue { get; set; }

        private string TypedString { get; set; } = string.Empty;

        private bool BeforePopupOpen { get; set; }

        private SfSpinner VirtualSpinnerObj { get; set; }

        /// <summary>
        /// Specifies the component name.
        /// </summary>
        /// <exclude/>
        protected override string ComponentName { get; set; } = "SfMultiSelect";

        private string PopupContent { get; set; }

        private string NoDataContent { get; set; }

        private KeyActions KeyAction { get; set; }

        private bool IsFocused { get; set; }

        private List<SelectedData<TItem>> SelectedData { get; set; } = new List<SelectedData<TItem>>();

        private bool IsChipRemove { get; set; }

        private bool IsChipClicked { get; set; }

        private List<TItem> ItemDatas { get; set; }

        private List<TItem> CustomData { get; set; } = new List<TItem>();

        private SfSpinner SpinnerObj { get; set; }

        private string SearchBoxElement { get; set; }

        private string DelimViewClass { get; set; }

        private string DelimValueClass { get; set; }

        [Inject]
        private ISyncfusionStringLocalizer Localizer { get; set; }

        private string ChipCollection { get; set; }

        private bool IsInitial { get; set; }

        private string SelectAllTxt { get; set; }

        private string UnSelectAllTxt { get; set; }

        private List<ListOptions<TItem>> SelectedListData { get; set; } = new List<ListOptions<TItem>>();

        private IEnumerable<ListOptions<TItem>> ListItemData { get; set; }

        private bool PreventKeydown { get; set; }

        private bool IsStringType { get; set; }

        private bool IsModifiedPopup { get; set; }

        private TItem CustomItem { get; set; }
        private bool isThailandCulture { get; set; }
        private bool isArabicCulture { get; set; }

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
            if (!ClosePopupOnSelectItem)
            {
                EnableCloseOnSelect = ClosePopupOnSelectItem;
            }

            if (!EnabledChangeOnBlur)
            {
                EnableChangeOnBlur = EnabledChangeOnBlur;
            }

            multiValue = Value;
            cssClass = CssClass;
            floatLabelType = FloatLabelType;
            mode = Mode;
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("multiselect");
            }

            DependencyScripts();
            ComponentInit();
            isArabicCulture = CultureInfo.CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            isThailandCulture = CultureInfo.CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal);
            var valueType = typeof(TValue);
            IsStringType = valueType == typeof(string[]) || valueType == typeof(List<string>);
            if (MultiSelectParent != null && Convert.ToString(MultiSelectParent.Type, CultureInfo.CurrentCulture) == "MultiSelect")
            {
                MultiSelectParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(MultiSelectParent, this);
            }

            CreateFloatingLabel();
        }

        private void ComponentInit()
        {
            RootClass = ROOT;
            ContainerClass = OVERALL_CONTAINER_CLASS + SINGLE_SPACE + INPUT_GROUP;
            ChildContainerClass = CONTAINER_CLASS;
            inputAttr = SfBaseUtils.UpdateDictionary(CLASS, RootClass, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(NAME, ID, inputAttr);
            PopupContainer = DDL + SINGLE_SPACE + POPUP_CONTAINER + SINGLE_SPACE + POPUP;

            // Unique class added for dynamically rendered Inplace-editor components
            if (MultiSelectParent != null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, "e-editable-elements");
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, "e-editable-elements");
            }

            SearchBoxElement = SEARCHER_BOX;
            DelimViewClass = DELIM_VALUES + SINGLE_SPACE + DELIM_VIEW + SINGLE_SPACE + DELIM_HIDE;
            DelimValueClass = DELIM_VALUES + SINGLE_SPACE + DELIM_HIDE;
            ChipCollection = CHIP_COLLECTION;
            if (Mode == VisualMode.CheckBox)
            {
                HideSelectedItem = false;
                AllowCustomValue = false;
                ClosePopupOnSelectItem = false;
                EnableCloseOnSelect = false;
                inputAttr = SfBaseUtils.UpdateDictionary(READONLY, true, inputAttr);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CHECK_BOX);
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, CHECK_BOX);
                EnableSelectionOrder = EnableGroupCheckBox ? false : EnableSelectionOrder;
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
            PreRender();
            if (PropertyChanges.Count > 0)
            {
                PreviousValue = PrevChanges != null ? Value: PreviousValue;
                var newProps = new Dictionary<string, object>(PropertyChanges);
                PropertyChanges.Remove(nameof(DataSource));
                PropertyChanges.Remove(nameof(Value));
                await PropertyChange(newProps);
                ValidateLabel();
            }

            UpdateValidateClass();
        }
        private async Task DynamicValueChange()
        {
            List<TItem> itemCollection = new List<TItem>();
            PreviousValue = (PrevChanges != null && this.PrevChanges.ContainsKey(nameof(Value))) ? (TValue)PrevChanges[nameof(Value)] : PreviousValue;
            if (ListData == null || !ListData.Any())
            {
                await Render(DataSource, Fields, Query);
            }

            itemCollection = await GetDataByValue(Value);
            await ClearSelectedValue();
            if (Value != null || itemCollection.Count >= 0)
            {
                if (itemCollection.Find((item) => item == null) == null && EnableVirtualization)
                {
                    await UpdateValue();
                    itemCollection = await GetDataByValue(Value);
                }
                await UpdateValueSelection(itemCollection);
                Text = GetDelimValues();
            }

            UpdateDelimClass();
            if (Mode != VisualMode.Box && !IsFocused)
            {
                await UpdateDelimViews();
            }
        }
        private async Task DynamicTextChange()
        {
            List<TItem> itemCollection = new List<TItem>();
            var textValues = Text.Split(DelimiterChar);
            foreach (var textItem in textValues)
            {
                itemCollection.Add(GetDataByText(textItem));
            }

            if (itemCollection.Count < SelectedData.Count)
            {
                SelectedData.Clear();
            }

            await UpdateValueSelection(itemCollection);
            if (Mode != VisualMode.Box && !IsFocused)
            {
                await UpdateDelimViews();
            }
        }
        private async Task PropertyChange(Dictionary<string, object> newProps)
        {
            var newProperties = newProps.ToList();
            List<TItem> itemCollection = new List<TItem>();
            foreach (var prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Value):
                        await DynamicValueChange();
                        break;
                    case nameof(Text):
                        await DynamicTextChange();
                        break;
                    case nameof(DataSource):
                    case nameof(Query):
                        await Render(DataSource, Fields, Query);
                        if (Value != null)
                        {
                            SelectedData = new List<SelectedData<TItem>>();
                            await UpdateValue();
                            if (Mode != VisualMode.Box && !IsFocused)
                            {
                                await UpdateDelimViews();
                            }
                        }

                        UpdateMainData();
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, cssClass);
                        PopupContainer = string.IsNullOrEmpty(PopupContainer) ? PopupContainer : SfBaseUtils.RemoveClass(PopupContainer, cssClass);
                        cssClass = CssClass;
                        break;
                    case nameof(SortOrder):
                        if (ListData != null && ListData.Any())
                        {
                            ListData = (SortOrder == SortOrder.None) ? ListData : (SortOrder == SortOrder.Ascending) ? ListData.OrderBy(item => DataUtil.GetObject(Fields?.Text, item)) : ListData.OrderByDescending(item => DataUtil.GetObject(Fields?.Text, item));
                            ListDataSource = (SortOrder == SortOrder.None) ? ListDataSource : (SortOrder == SortOrder.Ascending) ? ListDataSource.OrderBy(item => item.Text) : ListDataSource.OrderByDescending(item => item.Text);
                        }

                        break;
                    case nameof(ClosePopupOnSelectItem):
                        EnableCloseOnSelect = ClosePopupOnSelectItem;
                        break;
                    case nameof(EnabledChangeOnBlur):
                        EnableChangeOnBlur = EnabledChangeOnBlur;
                        break;
                    case nameof(FloatLabelType):
                        CreateFloatingLabel();
                        break;
                    case nameof(Mode):
                        await UpdateValue();
                        break;
                }
            }
        }

        private void UpdateMainData()
        {
            MainData = ListData;
        }

        private async Task PropertyParametersSet()
        {
            floatLabelType = NotifyPropertyChanges(nameof(FloatLabelType), FloatLabelType, floatLabelType);
            mode = NotifyPropertyChanges(nameof(Mode), Mode, mode);
            NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            multiValue = NotifyPropertyChanges(nameof(Value), Value, multiValue, true);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Triggers after the component was rendered.
        /// </summary>
        /// <param name="firstRender">true if the component rendered for the first time.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            SelectAllTxt = (Localizer.GetText(SELECT_ALL_TEXT_KEY) == null) ? SelectAllText : Localizer.GetText(SELECT_ALL_TEXT_KEY);
            UnSelectAllTxt = (Localizer.GetText(UN_SELECT_ALL_TEXT_KEY) == null) ? UnSelectAllText : Localizer.GetText(UN_SELECT_ALL_TEXT_KEY);
            if (AllowFiltering && IsDevice)
            {
                BackIcon = new List<string>() { BACK_ICON };
            }

            await ClientPopupRender();
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
                        Value = multiValue = await SfBaseUtils.UpdateProperty(persistValue, multiValue, ValueChanged, DropDownsEditContext, ValueExpression);
                    }
                }

                PreviousValue = Value;
                await SfBaseUtils.InvokeEvent<object>(MultiselectEvents?.Created, null);

                // Called In-placeEditor method for value updating
                if (MultiSelectParent != null)
                {
                    MultiSelectParent.GetType().GetMethod("UpdateMultiSelectValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MultiSelectParent, new object[] { Fields.Text });
                }
            }
        }

        private async Task InitialValueBind()
        {
            var values = await GetDataByValue(Value);
            if ((values != null && values.Count > 0) || !string.IsNullOrEmpty(Text))
            {
                IsInitial = true;
                if (Mode != VisualMode.Box)
                {
                    SearchBoxElement = SfBaseUtils.AddClass(SearchBoxElement, ZERO_SIZE);
                }

                await ShowSpinner();
                await InitValue();
            }
            else
            {
                IsInitial = true;
                await Render(DataSource, Fields, Query);
            }

            IsInitial = false;
            MainData = ListData;
        }

        private void PreRender()
        {
            ContainerAttr = SfBaseUtils.UpdateDictionary(STYLE, "width:" + Width + ";", ContainerAttr);
            ChildContainerClass = ShowDropDownIcon ? SfBaseUtils.AddClass(ChildContainerClass, DOWN_ICON) : SfBaseUtils.RemoveClass(ChildContainerClass, DOWN_ICON);
            PopupContainer = EnableGroupCheckBox ? SfBaseUtils.AddClass(PopupContainer, GROUP_CHECKBOX) : SfBaseUtils.RemoveClass(PopupContainer, GROUP_CHECKBOX);
            UpdateAriaAttributes();
            SetRTL();
            SetReadOnly();
            SetEnabled();
            SetCssClass();
            var inputTextVal = string.IsNullOrEmpty(InputValue) && Value != null ? GetDelimValues() : InputValue;
            CheckInputValue(FloatLabelType, inputTextVal);
            UpdateHtmlAttr();
            UpdateInputAttr();
        }

        internal override async Task OnAfterScriptRendered()
        {
            var options = GetProperty();
            await InvokeMethod("sfBlazor.MultiSelect.initialize", new object[] { ContainerElement, ChildContainerElement, InputElement, DotnetObjectReference, options });
            IsDevice = SyncfusionService.IsDeviceMode;
            await InitialValueBind();
        }

        private async Task ClientPopupRender()
        {
            if (ShowPopupList && IsListRendered)
            {
                IsListRendered = false;
                var options = GetProperty();
                IsListRender = true;
                await InvokeMethod("sfBlazor.MultiSelect.renderPopup", new object[] { InputElement, PopupElement, PopupHolderEle, PopupEventArgs, IsModifiedPopup, options });
            }
        }

        private void UpdateDefaultView(bool isFocus = false)
        {
            if (Mode == VisualMode.Default)
            {
                ChipCollection = isFocus ? SfBaseUtils.RemoveClass(ChipCollection, DELIM_HIDE) : SfBaseUtils.AddClass(ChipCollection, DELIM_HIDE);
            }
        }

        private DropDownClientProperties GetProperty()
        {
            var overFlowCount = Localizer.GetText(OVER_FLOW_COUNT_KEY) == null ? OVER_FLOW_COUNT_VALUE : Localizer.GetText(OVER_FLOW_COUNT_KEY);
            var totalCount = Localizer.GetText(TOTAL_COUNT_KEY) == null ? TOTAL_COUNT_VALUE : Localizer.GetText(TOTAL_COUNT_KEY);
            return new DropDownClientProperties
            {
                EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                ZIndex = ZIndex,
                PopupHeight = PopupHeight,
                PopupWidth = PopupWidth,
                Width = Width,
                AllowFiltering = AllowFiltering,
                EnableVirtualization = EnableVirtualization,
                ModuleName = ComponentName,
                DelimiterChar = DelimiterChar,
                OverFlowContent = overFlowCount,
                TotalCountContent = totalCount,
                DelimValue = SelectedData != null && SelectedData.Count > 0 ? SelectedData.Select(item => item.Text).ToList<string>() : new List<string>(),
                Mode = Mode.ToString()
            };
        }

        /// <summary>
        /// Task used to refresh the popup list items.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task RefreshPopup()
        {
            await InvokeMethod("sfBlazor.MultiSelect.refreshPopup", new object[] { InputElement });
        }
        /// <summary>
        /// Invoke the client side method for update the input value while focusing out the component.
        /// </summary>
        private async Task SetInputValue()
        {
            await InvokeMethod("sfBlazor.MultiSelect.setInputValue", new object[] { InputElement, InputValue });
        }

        /// <summary>
        /// Method which updates the dependency scripts.
        /// </summary>
        /// <exclude/>
        protected void DependencyScripts()
        {
            ScriptModules = SfScriptModules.SfMultiSelect;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
        }

        private async Task ContainerClick()
        {
            if (Enabled && !Readonly && !IsChipRemove)
            {
                PreventContainer = string.IsNullOrEmpty(InputValue) || IsChipClicked;
                if (IsListRender)
                {
                    await HidePopup();
                    IsInternalFocus = true;
                    await FocusIn();
                }
                else
                {
                    await FocusIn();
                    UpdateDefaultView(true);
                    UpdateDelimClass();
                    IsDropDownClick = true;
                    IsChipClicked = false;
                    if (OpenOnClick)
                    {
                        await ShowPopup();
                    }
                }
            }
            else
            {
                IsChipRemove = false;
            }
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
                    RelateTo = ContainerElement,
                    TargetType = TargetType.Relative
                }
            };
        }

        private async Task TypeOnOpen()
        {
            if (!OpenOnClick)
            {
                if (!string.IsNullOrEmpty(InputValue) || Mode == VisualMode.CheckBox)
                {
                    if (!IsListRender)
                    {
                        await ShowPopup();
                    }
                }
                else if (IsListRender)
                {
                    await HidePopup();
                }
            }
        }

        private void SetReOrder()
        {
            if (EnableSelectionOrder && !EnableGroupCheckBox && Mode == VisualMode.CheckBox && SelectedData != null && SelectedData.Count > 0)
            {
                SelectedListData = ListDataSource.Where(selectItem => selectItem.ListClass.Contains(SELECTED, StringComparison.Ordinal))?.ToList();
                ListItemData = ListDataSource.Where(selectItem => !selectItem.ListClass.Contains(SELECTED, StringComparison.Ordinal))?.ToList();
            }
            else
            {
                SelectedListData?.Clear();
                ListItemData = ListDataSource;
            }
        }

        private async Task ClosePopupAction()
        {
            if (IsDropDownClick)
            {
                IsInternalFocus = true;
                await FocusIn();
            }

            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ICON_ANIM).Trim();
            IsListRender = false;
            ShowPopupList = false;
            BeforePopupOpen = false;
            await InvokeAsync(() => StateHasChanged());
            IsFilterClearClicked = false;
            if (AllowCustomValue || (AllowFiltering && !string.IsNullOrEmpty(TypedString)))
            {
                if (!SfBaseUtils.Equals(ListData, MainData))
                {
                    ListData = AllowFiltering ? MainData : ListData;
                    if (!string.IsNullOrEmpty(InputValue) && !AllowFiltering && Mode != VisualMode.CheckBox)
                    {
                        ListData = MainData;
                        await CheckForCustomValue();
                    }

                    await RenderItems();
                    SetReOrder();
                }
            }

            FilterinputObj = null;
            TypedString = null;
        }

        private async Task BackIconHandler()
        {
            await HidePopup();
            IsInternalFocus = true;
            await FocusIn();
        }

        /// <summary>
        /// Task which pecifies the action begin event.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<ActionBeginEventArgs> ActionBegin(IEnumerable<TItem> dataSource, Query query = null)
        {
            var actionBeginArgs = new ActionBeginEventArgs
            {
                Cancel = false,
                Query = query,
                EventName = "ActionBegin"
            };
            if (!IsInitial || Value != null)
            {
                await SfBaseUtils.InvokeEvent<ActionBeginEventArgs>(MultiselectEvents?.OnActionBegin, actionBeginArgs);
            }

            await ShowSpinner();
            return actionBeginArgs;
        }

        /// <summary>
        /// Task which specifies the action complete event.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
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
            if (!IsInitial || Value != null)
            {
                await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(MultiselectEvents?.OnActionComplete, actioncompleteArgs);
            }

            if (!actioncompleteArgs.Cancel)
            {
                ListData = actioncompleteArgs.Result;
                if (AllowCustomValue && IsTyped)
                {
                    if (!AllowFiltering && CustomData.Count > 0)
                    {
                        ListData = CustomData.Concat(ListData);
                    }

                    await CheckForCustomValue();
                }
                else
                {
                    await RenderItems();
                }

                SetReOrder();
                var dataBoundArgs = new DataBoundEventArgs() { Items = ListData };
                await SfBaseUtils.InvokeEvent<DataBoundEventArgs>(MultiselectEvents?.DataBound, dataBoundArgs);
                if (BeforePopupOpen && !IsListRender && IsTyped)
                {
                    await ShowPopup();
                }

                await HideSpinner();
            }
        }

        private TItem GetDataText(string ddlText)
        {
            if (MainData != null)
            {
                if (IsSimpleDataType())
                {
                    return MainData.Where(item => SfBaseUtils.Equals(item, ddlText)).FirstOrDefault();
                }
                else
                {
                    var fields = !string.IsNullOrEmpty(Fields?.Text) ? Fields.Text : "text";
                    return MainData.Where(item => EqualityComparer<string>.Default.Equals((string)SfBaseUtils.ChangeType(DataUtil.GetObject(fields, item), typeof(string)), ddlText)).FirstOrDefault();
                }
            }

            return default;
        }

        private async Task<TItem> UpdateCustomValue(string customValue)
        {
            CustomItem = SetItemValue(customValue);
            if (CustomItem != null)
            {
                var customItems = new List<TItem>() { CustomItem };
                await InsertItem(customItems, 0, true);
                await ListFocus();
            }

            return CustomItem;
        }

        private async Task CheckForCustomValue(string customValue = null)
        {
            if (!string.IsNullOrEmpty(InputValue) || !string.IsNullOrEmpty(customValue))
            {
                var dataCheck = !string.IsNullOrEmpty(InputValue) ? GetDataText(InputValue) : GetDataText(customValue);
                var isDefault = dataCheck != null ? SfBaseUtils.Equals(dataCheck, default) : false;
                if (AllowCustomValue && (dataCheck == null || isDefault) && Mode != VisualMode.CheckBox)
                {
                    CustomItem = SetItemValue(InputValue);
                    if (CustomItem != null)
                    {
                        var customItems = new List<TItem>() { CustomItem };
                        await InsertItem(customItems, 0, true);
                        await ListFocus();
                    }
                    else
                    {
                        await RenderItems();
                    }
                }
            }
        }

        /// <summary>
        /// Task which specifies the action failure event.
        /// </summary>
        /// <param name="args">Specifies the object arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task ActionFailure(object args)
        {
            await HideSpinner();
            if (!IsInitial || Value != null)
            {
                await SfBaseUtils.InvokeEvent<Exception>(MultiselectEvents?.OnActionFailure, (Exception)args);
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
            IsDropDownClick = false;
            if (ShowDropDownIcon)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, ICON_ANIM);
            }
        }

        private async Task ListFocus()
        {
            RemoveFocusList();
            if (ListDataSource != null && ListDataSource.Any() && SelectedData != null && SelectedData.Count > 0)
            {
                foreach (var selectItem in SelectedData)
                {
                    var selectedItem = ListDataSource.Where(item => SfBaseUtils.Equals(item.CurItemData, selectItem.ItemData)).FirstOrDefault();
                    if (Mode != VisualMode.CheckBox && selectedItem != null && AllowFiltering && HideSelectedItem && !string.IsNullOrEmpty(TypedString))
                    {
                        ((List<ListOptions<TItem>>)ListDataSource).Remove(selectedItem);
                    }
                    else
                    {
                        await UpdateListSelection(selectedItem, SELECTED, true);
                    }
                }

                if (HideSelectedItem || (Mode != VisualMode.CheckBox && SelectedData.Count == 0))
                {
                    var focusData = ListDataSource.Where(item => !item.IsHeader && !item.ListClass.Contains(HIDE_LIST, StringComparison.Ordinal)).FirstOrDefault();
                    if (focusData != null)
                    {
                        await UpdateListSelection(focusData, ITEM_FOCUS, true);
                    }
                }
            }
            else if (ListDataSource != null && ListDataSource.Any())
            {
                var focusData = ListDataSource.Where(item => !item.IsHeader && !item.ListClass.Contains(HIDE_LIST, StringComparison.Ordinal)).FirstOrDefault();
                if (focusData != null)
                {
                    await UpdateListSelection(focusData, ITEM_FOCUS, true);
                }
            }
        }

        /// <summary>
        /// Task which updates the selected item.
        /// </summary>
        /// <param name="listItem">Specifies the list item.</param>
        /// <param name="className">Specifies the class name..</param>
        /// <param name="isAdd">Specifies whether to add the item or not.</param>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task UpdateListSelection(ListOptions<TItem> listItem, string className, bool isAdd, EventArgs args = null)
        {
            if (listItem != null)
            {
                if (args == null)
                {
                    RemoveFocusList();
                }

                className = (className == SELECTED && HideSelectedItem) ? HIDE_LIST : className;
                listItem.ListClass = isAdd ? SfBaseUtils.AddClass(listItem.ListClass, className) : SfBaseUtils.RemoveClass(listItem.ListClass, className);
                if (KeyAction != null && (KeyAction.Action != "Enter" && KeyAction.Action != "Space"))
                {
                    await Task.Delay(20);  // update the scroll position based on selection
                    await InvokeMethod("sfBlazor.MultiSelect.updateScrollPosition", new object[] { InputElement, KeyAction.Action });
                    KeyAction = null;
                }
            }
        }

        private void RemoveFocusList(bool isSelected = false)
        {
            if (ListDataSource != null && ListDataSource.Any())
            {
                RemoveOver();
                var removeClass = !isSelected ? ITEM_FOCUS : HideSelectedItem ? HIDE_LIST : SELECTED;
                var focusedData = ListDataSource.Where(listItem => listItem.ListClass.Contains(removeClass, StringComparison.CurrentCulture)).ToList();
                if (focusedData != null)
                {
                    foreach (var item in focusedData)
                    {
                        item.ListClass = SfBaseUtils.RemoveClass(item.ListClass, removeClass);
                    }
                }
            }
        }

        /// <summary>
        /// Update the MultiSelect fileds.
        /// </summary>
        /// <param name="fieldValue">Specifies the field value.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void UpdateChildProperties(object fieldValue)
        {
            var fields = (MultiSelectFieldSettings)fieldValue;
            Fields = new FieldSettingsModel() { GroupBy = fields?.GroupBy, HtmlAttributes = fields?.HtmlAttributes, IconCss = fields?.IconCss, Text = fields?.Text, Value = fields?.Value };
            SetFields();
        }

        private async Task OnMouseClick(ListOptions<TItem> args, MouseEventArgs eventArgs)
        {
            if (args != null && (args.CurItemData != null || !args.IsHeader || EnableGroupCheckBox))
            {
                KeyAction = null;
                if (args.IsHeader && EnableGroupCheckBox && args.GroupItems != null && args.GroupItems.Items != null)
                {
                    args.ListClass = args.ListClass.Contains(SELECTED, StringComparison.Ordinal) && !args.ListClass.Contains(INDETERMINATE, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(args.ListClass, SELECTED) : SfBaseUtils.AddClass(args.ListClass, SELECTED);
                    var isSelect = args.ListClass.Contains(SELECTED, StringComparison.Ordinal) || args.ListClass.Contains(INDETERMINATE, StringComparison.Ordinal);
                    var eventArg = !isSelect ? eventArgs : null;
                    for (int grpItem = 0; grpItem < args.GroupItems.Items.Count; grpItem++)
                    {
                        var item = args.GroupItems.Items[grpItem];
                        var listItem = ListDataSource.Where(list => SfBaseUtils.Equals(list.CurItemData, item)).FirstOrDefault();
                        await UpdateSelectedItem(listItem, eventArg, null, isSelect);
                    }

                    await UpdateDelimViews();
                    await UpdateItemValue();
                }
                else
                {
                    await UpdateSelectedItem(args, eventArgs);
                    await UpdateListSelection(args, ITEM_FOCUS, true);
                }

                string inputVal = GetDelimValues();
                ValidateLabel(inputVal);
                await UpdateCloseOnPopup();
            }
        }

        private async Task UpdateCloseOnPopup()
        {
            if (IsListRender && EnableCloseOnSelect)
            {
                await HidePopup();
                if (Mode == VisualMode.CheckBox)
                {
                    IsInternalFocus = true;
                    await FocusIn();
                }
            }
            else if (HideSelectedItem)
            {
                await Task.Delay(10);  // set time delay for calling the refresh popup
                await RefreshPopup();
                await ListFocus();
            }
        }

        /// <summary>
        /// Task which updates the selected item.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <param name="keyArgs">Specifies the KeyActions arguments.</param>
        /// <param name="isGroupBy">Specifies whether the items should be grouped or not.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task UpdateSelectedItem(ListOptions<TItem> item, EventArgs args = null, KeyboardEventArgs keyArgs = null, bool isGroupBy = false)
        {
            ItemData = item != null ? item.CurItemData : default;
            IsTyped = false;
            if (!AllowCustomValue)
            {
                InputValue = null;
                await SetInputValue();
            }

            if ((item != null && !item.ListClass.Contains(SELECTED, StringComparison.Ordinal)) || isGroupBy)
            {
                var selectEventArgs = new SelectEventArgs<TItem>()
                {
                    Cancel = false,
                    IsInteracted = args != null,
                    Item = null,
                    ItemData = ItemData,
                    E = args
                };
                await SfBaseUtils.InvokeEvent<SelectEventArgs<TItem>>(MultiselectEvents?.OnValueSelect, selectEventArgs);
                if (!selectEventArgs.Cancel)
                {
                    if (AllowCustomValue && Mode != VisualMode.CheckBox)
                    {
                        await CustomValueSelect(item, args, keyArgs);
                    }
                    else
                    {
                        await AddValue(item, args, keyArgs);
                    }
                }
            }
            else
            {
                await RemoveValue(item, args, keyArgs);
            }
        }

        private async Task CustomValueSelect(ListOptions<TItem> item = null, EventArgs args = null, KeyboardEventArgs keyArgs = null)
        {
            if (AllowCustomValue && MainData != null && item != null && !MainData.Contains(item.CurItemData))
            {
                var customValueSelectArgs = new CustomValueEventArgs<TItem>()
                {
                    Text = item.Text,
                    NewData = item.CurItemData,
                    Cancel = false
                };
                await SfBaseUtils.InvokeEvent<CustomValueEventArgs<TItem>>(MultiselectEvents?.CustomValueSpecifier, customValueSelectArgs);
                if (!customValueSelectArgs.Cancel)
                {
                    if (!SfBaseUtils.Equals(customValueSelectArgs.NewData, item.CurItemData))
                    {
                        ListData = ListData.Select(x => x.Equals(CustomItem) ? customValueSelectArgs.NewData : x).ToList();
                        var customValue = DataUtil.GetObject(Fields.Value, customValueSelectArgs.NewData);
                        _ = ListDataSource.Select(lists => lists.CurItemData = SfBaseUtils.Equals(lists.CurItemData, CustomItem) ? customValueSelectArgs.NewData : lists.CurItemData).ToList();
                        _ = ListDataSource.Select(lists => lists.Value = SfBaseUtils.Equals(lists.CurItemData, customValueSelectArgs.NewData) ? customValue?.ToString() : lists.Value).ToList();
                        CustomItem = customValueSelectArgs.NewData;
                    }
                    CustomData.Add(customValueSelectArgs.NewData);
                    var newItems = new List<TItem>(MainData);
                    newItems.Insert(0, customValueSelectArgs.NewData);
                    MainData = newItems;
                    await AddValue(item, args, keyArgs);
                    InputValue = null;
                    await SetInputValue();
                }
            }
            else
            {
                if (AllowCustomValue && Mode != VisualMode.CheckBox && !string.IsNullOrEmpty(InputValue))
                {
                    var removeItems = CustomItem != null ? ListData.Where(custom => !SfBaseUtils.Equals(custom, CustomItem)).ToList() : ListData;
                    if (removeItems != null && removeItems.Any())
                    {
                        ListData = removeItems;
                        await RenderItems();
                    }

                    InputValue = null;
                    await SetInputValue();
                }

                await AddValue(item, args, keyArgs);
            }
        }

        private string GetDelimValues()
        {
            if (SelectedData != null && SelectedData.Count > 0)
            {
                var textValue = SelectedData.Select(item => item.Text).ToArray();
                return string.Join(DelimiterChar + SINGLE_SPACE, textValue);
            }

            return string.Empty;
        }

        private async Task SelectAllHandler(bool state, MouseEventArgs args = null)
        {
            if (state)
            {
                await SelectAllValue(args);
            }
            else
            {
                if (Mode == VisualMode.CheckBox && IsTyped && AllowFiltering && !string.IsNullOrEmpty(TypedString))
                {
                    await ClearSelectedValue(true, args);
                }
                else
                {
                    await ClearAll(args, true);
                }
            }
        }

        private async Task SelectAllValue(MouseEventArgs args = null)
        {
            if (ListDataSource == null || !ListDataSource.Any())
            {
                await Render(DataSource, Fields, Query);
            }

            if (ListDataSource != null && ListDataSource.Any())
            {
                var listItem = ListDataSource.Select(item => item.CurItemData)?.ToList();
                await UpdateValueSelection(listItem);
                await UpdateItemValue();
                ValidateLabel(Text);
                if (Mode != VisualMode.Box)
                {
                    await UpdateDelimViews();
                }

                var selectAllArgs = new SelectAllEventArgs<TItem>()
                {
                    IsChecked = true,
                    ItemData = listItem,
                    IsInteracted = args != null
                };
                await SfBaseUtils.InvokeEvent<SelectAllEventArgs<TItem>>(MultiselectEvents?.SelectedAll, selectAllArgs);
                if (!EnableChangeOnBlur)
                {
                    await OnChangeEvent();
                }
            }
        }

        private async Task AddValue(ListOptions<TItem> item = null, EventArgs args = null, KeyboardEventArgs keyArgs = null)
        {
            if (item == null || item.ListClass.Contains(DISABLE, StringComparison.Ordinal))
            {
                return;
            }

            await UpdateListSelection(item, SELECTED, true, args);
            var tagEventArgs = new TaggingEventArgs<TItem>()
            {
                Cancel = false,
                E = args != null ? args : keyArgs,
                IsInteracted = args != null || keyArgs != null,
                ItemData = item.CurItemData,
                SetClass = string.Empty
            };
            if (Mode == VisualMode.Box || Mode == VisualMode.Default)
            {
                await SfBaseUtils.InvokeEvent<TaggingEventArgs<TItem>>(MultiselectEvents?.OnChipTag, tagEventArgs);
            }

            if (Mode == VisualMode.Delimiter)
            {
                ChildContainerClass = SfBaseUtils.AddClass(ChildContainerClass, DELIMITER_CONTAINER);
            }

            if (!tagEventArgs.Cancel)
            {
                var dataItem = GetItemData(item.CurItemData);
                if (!string.IsNullOrEmpty(tagEventArgs.SetClass))
                {
                    dataItem.ChipClass = SfBaseUtils.AddClass(dataItem.ChipClass, tagEventArgs.SetClass);
                }

                if (Mode == VisualMode.Box)
                {
                    ChipCollection = SfBaseUtils.RemoveClass(ChipCollection, DELIM_HIDE);
                }

                if (SelectedData.Count == 0 || (SelectedData.Count > 0 && SelectedData.Where(item => SfBaseUtils.Equals(item.Value, dataItem.Value)).FirstOrDefault() == null))
                {
                    SelectedData.Add(dataItem);
                }

                UpdateDelimClass();
                UpdateDefaultView(IsFocused);
                if (args != null || keyArgs != null)
                {
                    await UpdateItemValue();
                    if (Mode == VisualMode.CheckBox)
                    {
                        await UpdateDelimViews();
                    }
                }

                if (!string.IsNullOrEmpty(InputValue) || SelectedData.Count > 0)
                {
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, VALID_INPUT);
                }

                UpdateMaximumLength();
                if (!EnableChangeOnBlur)
                {
                    await OnChangeEvent(args);
                }
            }
        }

        private async Task SetSelectOptions(TItem item, EventArgs args = null, KeyboardEventArgs keyArgs = null, bool isAdd = true)
        {
            await SetValue(item, isAdd, args, keyArgs);
            if (!EnableChangeOnBlur)
            {
                await OnChangeEvent(args);
            }
        }

        private async Task SetValue(TItem item, bool isAdd, EventArgs args = null, KeyboardEventArgs keyArgs = null)
        {
            var dataItem = GetItemData(item);
            if (isAdd)
            {
                if (SelectedData.Count == 0 || (SelectedData.Count > 0 && SelectedData.Where(item => SfBaseUtils.Equals(item.Value, dataItem.Value)).FirstOrDefault() == null))
                {
                    SelectedData.Add(dataItem);
                }
            }
            else
            {
                var removeItem = SelectedData.Where(item => item.Value.Equals(dataItem.Value)).FirstOrDefault();
                SelectedData.Remove(removeItem);
            }

            if (args != null || keyArgs != null)
            {
                await UpdateItemValue();
            }

            UpdateMaximumLength();
        }

        private void UpdateMaximumLength()
        {
            if (MaximumSelectionLength != 1000 && ListDataSource != null)
            {
                if (SelectedData.Count < MaximumSelectionLength)
                {
                    var disableData = ListDataSource.Where(listItem => listItem.ListClass.Contains(DISABLE, StringComparison.CurrentCulture)).ToList();
                    if (disableData != null)
                    {
                        foreach (var item in ListDataSource)
                        {
                            item.ListClass = SfBaseUtils.RemoveClass(item.ListClass, DISABLE);
                        }
                    }
                }
                else
                {
                    var disableData = ListDataSource.Where(listItem => !listItem.ListClass.Contains(SELECTED, StringComparison.CurrentCulture)).ToList();
                    if (disableData != null && disableData.Any())
                    {
                        foreach (var item in disableData)
                        {
                            item.ListClass = SfBaseUtils.AddClass(item.ListClass, DISABLE);
                        }
                    }
                }
            }
        }

        private async Task RemoveValue(ListOptions<TItem> item = null, EventArgs args = null, KeyboardEventArgs keyArgs = null, bool isSelectAllClicked = false)
        {
            if (item == null)
            {
                return;
            }

            ItemData = item.CurItemData;
            var removeEventArgs = new RemoveEventArgs<TItem>()
            {
                Cancel = false,
                E = args,
                IsInteracted = args != null || keyArgs != null,
                ItemData = item.CurItemData
            };
            if (args != null || keyArgs != null)
            {
                await SfBaseUtils.InvokeEvent<RemoveEventArgs<TItem>>(MultiselectEvents?.OnValueRemove, removeEventArgs);
            }

            if (!removeEventArgs.Cancel)
            {
                await UpdateListSelection(item, SELECTED, false, args);
                await SetSelectOptions(ItemData, args, keyArgs, false);
                if (args != null || keyArgs != null)
                {
                    await SfBaseUtils.InvokeEvent<RemoveEventArgs<TItem>>(MultiselectEvents?.ValueRemoved, removeEventArgs);
                }

                if (string.IsNullOrEmpty(InputValue) || SelectedData.Count == 0)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, VALID_INPUT);
                }

                if (Mode == VisualMode.CheckBox && (args != null || keyArgs != null) && !isSelectAllClicked)
                {
                    await UpdateDelimViews();
                }
            }
        }

        /// <summary>
        /// Triggers when value get changed.
        /// </summary>
        /// <param name="args">Specifies EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task OnChangeEvent(EventArgs args = null)
        {
            if (!SfBaseUtils.Equals(PreviousValue, Value))
            {
                var changeEventArgs = new MultiSelectChangeEventArgs<TValue>()
                {
                    E = args,
                    OldValue = PreviousValue,
                    Value = Value,
                    IsInteracted = args != null
                };
                await SfBaseUtils.InvokeEvent<MultiSelectChangeEventArgs<TValue>>(MultiselectEvents?.ValueChange, changeEventArgs);
                PreviousValue = Value;
                if (EnablePersistence)
                {
                    await SetLocalStorage(ID, Value);
                }
            }
        }
        private TValue GetIntValues(Type valueType, List<string> values, bool isCtrlKey, string removeValue = null)
        {
            List<int> value = new List<int> { };
            foreach (var val in values)
            {
                value.Add(int.Parse(val, CultureInfo.CurrentCulture));
            }

            if (isCtrlKey && Value != null)
            {
                value.InsertRange(0, Value as IEnumerable<int>);
            }

            if (removeValue != null && value.Contains(int.Parse(removeValue, CultureInfo.CurrentCulture)))
            {
                value.Remove(int.Parse(removeValue, CultureInfo.CurrentCulture));
            }

            if (Value == null && value.Count == 0)
            {
                return default;
            }

            if (valueType.IsArray)
            {
                return valueType == typeof(int?[]) ? (TValue)(object)value.ToArray().Cast<int?>().ToArray() : (TValue)(object)value.ToArray();
            }
            else
            {
                return valueType == typeof(List<int?>) ? (TValue)(object)value.ToList().Cast<int?>().ToList() : (TValue)(object)value;
            }
        }
        private TValue GetStringValue(Type valueType, List<string> values, bool isCtrlKey, string removeValue = null)
        {
            if (isCtrlKey && Value != null)
            {
                values.InsertRange(0, Value as IEnumerable<string>);
            }

            if (removeValue != null && values.Contains(removeValue))
            {
                values.Remove(removeValue);
            }

            if (Value == null && values.Count == 0)
            {
                return default;
            }

            if (valueType.IsArray)
            {
                return (TValue)(object)values.ToArray();
            }
            else
            {
                return (TValue)(object)values;
            }
        }
        private TValue GetLongValue(Type valueType, List<string> values)
        {
            List<long> value = new List<long> { };
            foreach (var val in values)
            {
                value.Add(long.Parse(val, CultureInfo.CurrentCulture));
            }

            if (Value == null && value.Count == 0)
            {
                return default;
            }

            if (valueType.IsArray)
            {
                return valueType == typeof(long?[]) ? (TValue)(object)value.ToArray().Cast<long?>().ToArray() : (TValue)(object)value.ToArray();
            }
            else
            {
                return valueType == typeof(List<long?>) ? (TValue)(object)value.ToList().Cast<long?>().ToList() : (TValue)(object)value;
            }
        }
        private TValue GetDecimalValue(Type valueType, List<string> values)
        {
            List<decimal> value = new List<decimal> { };
            foreach (var val in values)
            {
                value.Add(decimal.Parse(val, CultureInfo.CurrentCulture));
            }

            if (Value == null && value.Count == 0)
            {
                return default;
            }

            if (valueType.IsArray)
            {
                return valueType == typeof(decimal?[]) ? (TValue)(object)value.ToArray().Cast<decimal?>().ToArray() : (TValue)(object)value.ToArray();
            }
            else
            {
                return valueType == typeof(List<decimal?>) ? (TValue)(object)value.ToList().Cast<decimal?>().ToList() : (TValue)(object)value;
            }
        }
        private TValue GetDoubleValue(Type valueType, List<string> values)
        {
            List<double> value = new List<double> { };
            foreach (var val in values)
            {
                value.Add(double.Parse(val, CultureInfo.CurrentCulture));
            }

            if (Value == null && value.Count == 0)
            {
                return default;
            }

            if (valueType.IsArray)
            {
                return valueType == typeof(double?[]) ? (TValue)(object)value.ToArray().Cast<double?>().ToArray() : (TValue)(object)value.ToArray();
            }
            else
            {
                return valueType == typeof(List<double?>) ? (TValue)(object)value.ToList().Cast<double?>().ToList() : (TValue)(object)value;
            }
        }
        private TValue GetGuidValue(Type valueType, List<string> values)
        {
            List<Guid> value = new List<Guid> { };
            foreach (var val in values)
            {
                value.Add(Guid.Parse(val));
            }

            if (Value == null && value.Count == 0)
            {
                return default;
            }

            if (valueType.IsArray)
            {
                return valueType == typeof(Guid?[]) ? (TValue)(object)value.ToArray().Cast<Guid?>().ToArray() : (TValue)(object)value.ToArray();
            }
            else
            {
                return valueType == typeof(List<Guid?>) ? (TValue)(object)value.ToList().Cast<Guid?>().ToList() : (TValue)(object)value;
            }
        }
        private TValue GetEnumValue(Type valueType, List<string> values)
        {
            List<Enum> value = new List<Enum> { };
            foreach (var val in values)
            {
                var selectedValue = SelectedData.Count > 0 ? SelectedData[0].Value : val;
                value.Add((Enum)Enum.Parse(selectedValue.GetType(), val));
            }

            if (Value == null && value.Count == 0)
            {
                return default;
            }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            if (valueType.IsArray)
            {
                return valueType == typeof(Enum?[]) ? (TValue)(object)value.ToArray().Cast<Enum?>().ToArray() : (TValue)(object)value.ToArray();
            }
            else
            {
                return valueType == typeof(List<Enum?>) ? (TValue)(object)value.ToList().Cast<Enum?>().ToList() : (TValue)(object)value;
            }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        }
        private TValue GetObjectValue(Type valueType)
        {
            List<TItem> value = new List<TItem> { };
            value = SelectedData.Select(item => item.ItemData).ToList();
            if (Value == null && value.Count == 0)
            {
                return default;
            }

            return valueType.IsArray ? (TValue)(object)value.ToArray() : (TValue)(object)value.ToList();
        }
        private TValue GetValue(List<string> values, bool isCtrlKey, string removeValue = null)
        {
            Type valueType = typeof(TValue);
            if (valueType.IsArray || valueType.IsGenericType)
            {
                var isIntType = valueType == typeof(int[]) || valueType == typeof(int?[]) || valueType == typeof(List<int>) || valueType == typeof(List<int?>);
                var isLongType = valueType == typeof(long[]) || valueType == typeof(long?[]) || valueType == typeof(List<long>) || valueType == typeof(List<long?>);
                var isDecimalType = valueType == typeof(decimal[]) || valueType == typeof(decimal?[]) || valueType == typeof(List<decimal>) || valueType == typeof(List<decimal?>);
                var isDoubleType = valueType == typeof(double[]) || valueType == typeof(double?[]) || valueType == typeof(List<double>) || valueType == typeof(List<double?>);
                var isGuidType = valueType == typeof(Guid[]) || valueType == typeof(Guid?[]) || valueType == typeof(List<Guid>) || valueType == typeof(List<Guid?>);
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                var isEnumType = valueType == typeof(Enum[]) || valueType == typeof(Enum?[]) || valueType == typeof(List<Enum>) || valueType == typeof(List<Enum?>);
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                var isObjectType = valueType == typeof(TItem[]) || valueType == typeof(List<TItem>);
                if (isIntType)
                {
                    return GetIntValues(valueType, values, isCtrlKey, removeValue);
                }
                else if (valueType == typeof(string[]) || valueType == typeof(List<string>))
                {
                    return GetStringValue(valueType, values, isCtrlKey, removeValue);
                }
                else if (isLongType)
                {
                    return GetLongValue(valueType, values);
                }
                else if (isDecimalType)
                {
                    return GetDecimalValue(valueType, values);
                }
                else if (isDoubleType)
                {
                    return GetDoubleValue(valueType, values);
                }
                else if (isGuidType)
                {
                    return GetGuidValue(valueType, values);
                }
                else if (isEnumType)
                {
                    return GetEnumValue(valueType, values);
                }
                else if (isObjectType)
                {
                    return GetObjectValue(valueType);
                }
            }
            else if (IsSimpleType() && values.Count > 0)
            {
                return (TValue)SfBaseUtils.ChangeType(string.Join(DelimiterChar, values), typeof(TValue));
            }

            return default;
        }

        private async Task GetScrollValue()
        {
            var takeData = Query.Queries.Take;
            var query = CloneQuery(Query);
            Query = query.Take(takeData + ItemsCount);
            await SetListData(DataSource, Fields, query);
            await UpdateValue();
        }

        /// <summary>
        /// Task which updates the value.
        /// </summary>
        /// <returns>Task.</returns>
        protected async Task UpdateValue()
        {
            if (Value != null)
            {
                Type propertyType = typeof(TValue);
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                if (IsSimpleType() && propertyType == typeof(string))
                {
                    var multiValues = Value.ToString().Split(DelimiterChar);
                    ItemDatas = ItemDatas != null ? ItemDatas : new List<TItem>();
                    var field = !string.IsNullOrEmpty(Fields?.Value) ? Fields.Value : "text";
                    foreach (var textItem in multiValues)
                    {
                        ItemDatas.Add(GetDataByText(textItem, field));
                    }
                }
                else
                {
                    ItemDatas = await GetDataByValue(Value);
                }

                if (EnableVirtualization)
                {
                    for (int dataItem = 0; dataItem < ItemDatas.Count; dataItem++)
                    {
                        if (ItemDatas[dataItem] == null)
                        {
                            await GetScrollValue();
                            return;
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(Text))
            {
                var textValues = Text.Split(DelimiterChar);
                ItemDatas = ItemDatas != null ? ItemDatas : new List<TItem>();
                foreach (var textItem in textValues)
                {
                    ItemDatas.Add(GetDataByText(textItem));
                }
            }

            await UpdateValueSelection(ItemDatas);
            if (Mode != VisualMode.Box)
            {
                await UpdateDelimViews();
            }
        }

        private async Task ClearSelectedValue(bool isSelectAllClicked = false, MouseEventArgs args = null, KeyboardEventArgs keyArgs = null)
        {
            if (SelectedData != null && SelectedData.Count > 0)
            {
                var removeItems = new List<SelectedData<TItem>>(SelectedData);
                for (int i = 0; i < removeItems.Count; i++)
                {
                    var itemValue = removeItems[i].Value;
                    var removeValue = ListDataSource.Where(item => SfBaseUtils.Equals(item.Value, itemValue.ToString())).FirstOrDefault();
                    await RemoveValue(removeValue, args, keyArgs, isSelectAllClicked);
                    if (!string.IsNullOrEmpty(InputValue))
                    {
                        InputValue = null;
                        await SetInputValue();
                    }
                }

                UpdateDelimClass();
                if (Mode == VisualMode.CheckBox)
                {
                    await UpdateDelimViews();
                }
            }
        }

        private async Task UpdateValueSelection(List<TItem> items)
        {
            if (items != null && items.Count > 0 && ListDataSource != null)
            {
                for (int selectItem = 0; selectItem < items.Count; selectItem++)
                {
                    var currentItem = ListDataSource.Where(listItem => !listItem.IsHeader && SfBaseUtils.Equals(listItem.CurItemData, items[selectItem])).FirstOrDefault();
                    if (currentItem != null)
                    {
                        await AddValue(currentItem);
                    }
                }
            }
        }

        private async Task InitValue()
        {
            await SetListData(DataSource, Fields, Query);
            await UpdateValue();
            string inputVal = GetDelimValues();
            ValidateLabel(inputVal);
            await InvokeAsync(() => StateHasChanged());
        }

        private async Task<TItem> GetDataObject(object ddlValue)
        {
            var dataItem = default(TItem);
            if (ListData != null)
            {
                if (IsSimpleDataType())
                {
                    if (typeof(TValue).IsEnum)
                    {
                        dataItem = ListData.Where(item => SfBaseUtils.Equals((TValue)Enum.Parse(typeof(TValue), item.ToString()), ddlValue)).FirstOrDefault();
                    }
                    else
                    {
                        dataItem = ListData.Where(item => SfBaseUtils.Equals(item, ddlValue)).FirstOrDefault();
                    }
                }
                else if (typeof(TValue) == typeof(List<TItem>) || typeof(TValue) == typeof(TItem[]) || typeof(TValue) == typeof(IEnumerable<TItem>) || typeof(TValue) == typeof(ICollection<TItem>))
                {
                    var ddlStringValue = System.Text.Json.JsonSerializer.Serialize(ddlValue);
                    dataItem = ListData.Where(item => string.Equals(System.Text.Json.JsonSerializer.Serialize(item), ddlStringValue, StringComparison.Ordinal)).FirstOrDefault();
                }
                else
                {
                    var fields = !string.IsNullOrEmpty(Fields?.Value) ? Fields.Value : "value";
                    ddlValue = typeof(TValue).IsEnum ? (TValue)Enum.Parse(typeof(TValue), ddlValue.ToString()) : ddlValue;
                    dataItem = ListData.Where(item => SfBaseUtils.Equals(DataUtil.GetObject(fields, item), ddlValue)).FirstOrDefault();
                }

                if (dataItem == null && AllowCustomValue && IsStringType && ddlValue != null && Mode != VisualMode.CheckBox)
                {
                    dataItem = await UpdateCustomValue(ddlValue.ToString());
                }
            }

            return dataItem;
        }

#pragma warning disable CA1822 // Mark members as static
        private bool IsSimpleType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return isNullable || type == typeof(string) || type == typeof(int) || type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(decimal) || type == typeof(bool);
        }

        private void UpdateDelimClass(bool isBlur = false)
        {
            if (Mode != VisualMode.Box && SelectedData != null)
            {
                if (Mode != VisualMode.CheckBox)
                {
                    DelimValueClass = isBlur ? SfBaseUtils.AddClass(DelimValueClass, DELIM_HIDE) : SfBaseUtils.RemoveClass(DelimValueClass, DELIM_HIDE);
                    DelimViewClass = isBlur ? SfBaseUtils.RemoveClass(DelimViewClass, DELIM_HIDE) : SfBaseUtils.AddClass(DelimViewClass, DELIM_HIDE);
                    SearchBoxElement = isBlur ? SfBaseUtils.AddClass(SearchBoxElement, ZERO_SIZE) : SfBaseUtils.RemoveClass(SearchBoxElement, ZERO_SIZE);
                }

                if (Mode == VisualMode.Delimiter)
                {
                    ChildContainerClass = isBlur ? SfBaseUtils.RemoveClass(ChildContainerClass, DELIMITER_CONTAINER) : SfBaseUtils.AddClass(ChildContainerClass, DELIMITER_CONTAINER);
                }

                if (SelectedData.Count == 0)
                {
                    DelimValueClass = SfBaseUtils.AddClass(DelimValueClass, DELIM_HIDE);
                    DelimViewClass = SfBaseUtils.AddClass(DelimViewClass, DELIM_HIDE);
                    SearchBoxElement = Mode != VisualMode.CheckBox ? SfBaseUtils.RemoveClass(SearchBoxElement, ZERO_SIZE) : SearchBoxElement;
                    ChildContainerClass = SfBaseUtils.RemoveClass(ChildContainerClass, DELIMITER_CONTAINER);
                }
                else if (Mode == VisualMode.CheckBox && !SearchBoxElement.Contains(ZERO_SIZE, StringComparison.Ordinal))
                {
                    SearchBoxElement = SfBaseUtils.AddClass(SearchBoxElement, ZERO_SIZE);
                    DelimViewClass = SfBaseUtils.RemoveClass(DelimViewClass, DELIM_HIDE);
                }

                if (isBlur)
                {
                    InvokeAsync(() => StateHasChanged());
                }
            }
        }

        private async Task UpdateDelimViews()
        {
            UpdateDelimClass(true);
            await Task.Delay(10);  // update the delim view and chip class before call invoke method
            if (Mode != VisualMode.Box && SelectedData != null)
            {
                await InvokeMethod("sfBlazor.MultiSelect.updateDelimViews", new object[] { InputElement, ViewElement, GetProperty() });
            }

            if (SelectedData.Count == 0 && Mode == VisualMode.CheckBox)
            {
                SearchBoxElement = SfBaseUtils.RemoveClass(SearchBoxElement, ZERO_SIZE);
            }
        }

        private async Task UpdateItemValue()
        {
            TValue listItemValue = default;
            if (SelectedData.Count > 0)
            {
                var itemValue = SelectedData.Select(item => item.Value?.ToString())?.ToList();
                listItemValue = GetValue(itemValue, false);
            }

            if (!IsInitial)
            {
                var previousValue = multiValue;
                multiValue = listItemValue;
                Value = await SfBaseUtils.UpdateProperty(listItemValue, previousValue, ValueChanged, DropDownsEditContext, ValueExpression);
            }

            Text = GetDelimValues();
        }

        private async Task SetLocalStorage(string persistId, TValue dataValue)
        {
            var serializeValue = dataValue != null ? System.Text.Json.JsonSerializer.Serialize(dataValue) : null;
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, serializeValue });
        }

        /// <summary>
        /// Method which gets item data.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected SelectedData<TItem> GetItemData(TItem item = default(TItem))
        {
            var dataItem = item;
            object dataValue = default;
            string dataText = default;
            if (dataItem != null)
            {
                if (IsSimpleDataType())
                {
                    dataValue = dataItem;
                    dataText = dataItem.ToString();
                }
                else
                {
                    dataValue = DataUtil.GetObject(Fields?.Value, dataItem);
                    dataText = DataUtil.GetObject(Fields?.Text, dataItem)?.ToString();
                }
            }

            return new SelectedData<TItem>() { Value = dataValue, Text = dataText, ItemData = item };
        }

        private async Task ClearHandler(MouseEventArgs args)
        {
            IsChipRemove = !(JSRuntime is IJSInProcessRuntime);
            if (Enabled && !Readonly)
            {
                await ClearAll(args);
                IsInternalFocus = true;
                await FocusIn();
                if (HideSelectedItem && IsListRender)
                {
                    await Task.Delay(20);     // refresh the popup after update the removed value in popup
                    await RefreshPopup();
                }
            }
        }

        private async Task ClearAll(MouseEventArgs args = null, bool isUnselectAll = false)
        {
            if (SelectedData.Count > 0)
            {
                if (AllowFiltering && IsTyped && !string.IsNullOrEmpty(InputValue))
                {
                    await FilterClear();
                }

                var listItem = SelectedData.Select(i => i.ItemData).ToList();

                // await ClearSelectedValue();
                RemoveFocusList(true);
                SelectedData = new List<SelectedData<TItem>>();
                await UpdateItemValue();
                UpdateDelimClass();
                if (Mode == VisualMode.CheckBox)
                {
                    await UpdateDelimViews();
                }

                InputValue = null;
                await SetInputValue();
                var selectAllArgs = new SelectAllEventArgs<TItem>()
                {
                    IsChecked = false,
                    ItemData = listItem,
                    IsInteracted = args != null
                };
                if (!isUnselectAll)
                {
                    await OnChangeEvent();
                }

                ValidateLabel(Text);
                UpdateMaximumLength();
                await SfBaseUtils.InvokeEvent(MultiselectEvents?.Cleared, args);
                await SfBaseUtils.InvokeEvent<SelectAllEventArgs<TItem>>(MultiselectEvents?.SelectedAll, selectAllArgs);
            }
            else if (!string.IsNullOrEmpty(InputValue))
            {
                if (AllowFiltering)
                {
                    await FilterClear();
                }

                InputValue = null;
                await SetInputValue();
            }
        }

        private async Task ChipClick(SelectedData<TItem> chipData)
        {
            if (Enabled && !IsChipRemove)
            {
                IsChipClicked = true;
                RemoveChipSelection();
                chipData.ChipClass = SfBaseUtils.AddClass(chipData.ChipClass, CHIP_SELECTED);
                var chipArgs = new ChipSelectedEventArgs<TItem>()
                {
                    ChipData = chipData.ItemData
                };
                await SfBaseUtils.InvokeEvent(MultiselectEvents?.ChipSelected, chipArgs);
            }
        }

        private async Task OnChipRemove(SelectedData<TItem> chipData, EventArgs args = null)
        {
            if (Enabled && !Readonly && chipData != null)
            {
                IsChipRemove = true;
                IsInternalFocus = true;
                await Task.Delay(10);      // input focus on remove the chip.
                await FocusIn();
                if (IsListRender && args != null)
                {
                    await HidePopup();
                }

                var removeValue = ListDataSource.Where(item => !item.IsHeader && SfBaseUtils.Equals(item.Value, chipData.Value.ToString())).FirstOrDefault();
                await RemoveValue(removeValue, args);
            }
        }

        private void RemoveChipSelection()
        {
            var focusChips = SelectedData?.Where(chip => chip.ChipClass.Contains(CHIP_SELECTED, StringComparison.Ordinal))?.ToList();
            if (focusChips != null)
            {
                foreach (var chip in focusChips)
                {
                    chip.ChipClass = SfBaseUtils.RemoveClass(chip.ChipClass, CHIP_SELECTED);
                }
            }
        }

        private void OnMouseOver(ListOptions<TItem> listItem = null)
        {
            RemoveOver();
            if (listItem != null)
            {
                listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, HOVER);
            }
        }

        private void OnMouseLeave()
        {
            RemoveOver();
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

        private void RemoveOver()
        {
            if (ListDataSource != null && ListDataSource.Any())
            {
                var hoverData = ListDataSource.Where(listItem => listItem.ListClass.Contains(HOVER, StringComparison.CurrentCulture)).FirstOrDefault();
                if (hoverData != null)
                {
                    hoverData.ListClass = SfBaseUtils.RemoveClass(hoverData.ListClass, HOVER);
                }
            }
        }

        private async Task OnKeyHandler(KeyboardEventArgs args)
        {
            if (Enabled && !Readonly && args != null)
            {
                KeyAction = new KeyActions() { Action = args.Code };
                PreventKeydown = false;
                switch (args.Code)
                {
                    case "ArrowDown":
                    case "ArrowUp":
                        await UpdateUpDownAction(args);
                        break;
                    case "Home":
                    case "End":
                        if (Mode != VisualMode.Box)
                        {
                            await UpdateHomeEndAction(args);
                        }
                        break;
                    case "PageUp":
                    case "PageDown":
                        await PageUpDownSelection(args);
                        break;
                    case "Enter":
                        if (this.ValueExpression != null && DropDownsEditContext != null)
                        {
                            PreventKeydown = true;
                            await Task.Delay(10);
                            await SelectCurrentItem(args);
                            UpdateValidateClass();
                        } else
                        {
                            await SelectCurrentItem(args);
                        }
                        break;
                    case "Space":
                        if (Mode == VisualMode.CheckBox && (FilterinputObj == null || string.IsNullOrEmpty(FilterinputObj.InputTextValue)))
                        {
                            PreventKeydown = true;
                            await SelectCurrentItem(args);
                        }

                        break;
                    case "Escape":
                    case "Tab":
                        if (IsListRender)
                        {
                            await HidePopup();
                            if (args.Code != "Tab")
                            {
                                IsInternalFocus = true;
                                await FocusIn();
                            }
                        }

                        if (args.Code == "Escape" && !IsListRender)
                        {
                            await EscapeAction();
                        }

                        break;
                }

                if (string.IsNullOrEmpty(InputValue))
                {
                    await KeyNavigation(args);
                }
            }
        }

        private async Task EscapeAction()
        {
            if (Value != null)
            {
                var tempCollection = PreviousValue != null ? await GetDataByValue(PreviousValue) : null;
                var currentCollection = await GetDataByValue(Value);
                if (Mode != VisualMode.CheckBox && tempCollection?.Count != currentCollection.Count)
                {
                    await ClearSelectedValue();
                    if (PreviousValue != null)
                    {
                        ItemDatas = await GetDataByValue(PreviousValue);
                        await UpdateValueSelection(ItemDatas);
                    }

                    Value = multiValue = await SfBaseUtils.UpdateProperty(PreviousValue, multiValue, ValueChanged, DropDownsEditContext, ValueExpression);
                    IsInternalFocus = true;
                    await FocusIn();
                    UpdateDefaultView(true);
                    UpdateDelimClass();
                }
            }
        }

        private async Task KeyNavigation(KeyboardEventArgs args)
        {
            if (Mode != VisualMode.Delimiter && Mode != VisualMode.CheckBox && SelectedData?.Count > 0 && (InputValue == null || InputValue.Trim().Length < 1))
            {
                switch (args.Code)
                {
                    case "ArrowLeft":
                    case "ArrowRight":
                        await ChipNavigation(args);
                        break;
                    case "Backspace":
                    case "Delete":
                        var chipData = SelectedData.Where((i) => i.ChipClass.Contains(CHIP_SELECTED, StringComparison.Ordinal))?.FirstOrDefault();
                        if (chipData != null)
                        {
                            await ChipNavigation(args);
                            await OnChipRemove(chipData, args);
                        } else if (args.Code == "Backspace")
                        {
                            await OnChipRemove(SelectedData[SelectedData.Count - 1], args);
                        }
                        break;
                    case "Home":
                    case "End":
                        IsChipRemove = false;
                        var selectChip = args.Code == "Home" ? SelectedData.FirstOrDefault() : SelectedData.LastOrDefault();
                        await ChipClick(selectChip);
                        break;
                }
            }
            else if (args.Code == "Backspace" && Mode == VisualMode.Delimiter && SelectedData.Count > 0)
            {
                PreventKeydown = true;
                var removeItem = SelectedData.ElementAtOrDefault(SelectedData.Count - 1);
                var listItem = ListDataSource?.Where(i => SfBaseUtils.Equals(i.CurItemData, removeItem.ItemData))?.FirstOrDefault();
                await RemoveValue(listItem, null, args);
                UpdateDelimClass();
                if (IsListRender)
                {
                    await ListFocus();
                }
            }
        }

        private async Task ChipNavigation(KeyboardEventArgs args)
        {
            var selectedChip = SelectedData.Where(i => i.ChipClass.Contains(CHIP_SELECTED, StringComparison.Ordinal))?.FirstOrDefault();
            var chipData = new SelectedData<TItem>();
            if (selectedChip == null)
            {
                chipData = SelectedData.ElementAtOrDefault(SelectedData.Count - 1);
            }
            else
            {
                var indexChip = SelectedData.IndexOf(selectedChip);
                chipData = (args.Code == "ArrowLeft") ? SelectedData.ElementAtOrDefault(indexChip - 1) : SelectedData.ElementAtOrDefault(indexChip + 1);
            }

            IsChipRemove = false;
            if (chipData != null)
            {
                await ChipClick(chipData);
            }
        }

        private async Task SelectCurrentItem(KeyboardEventArgs args)
        {
            if (IsListRender)
            {
                PreventKeydown = true;
                var focusedData = ListDataSource.Where(listItem => listItem.ListClass.Contains(ITEM_FOCUS, StringComparison.Ordinal)).FirstOrDefault();
                if (focusedData != null)
                {
                    await UpdateSelectedItem(focusedData, null, args);
                    await UpdateListSelection(focusedData, ITEM_FOCUS, true);
                }

                await UpdateCloseOnPopup();
                await OnChangeEvent(args);
                PreventKeydown = false;
            }
        }

        private async Task PageUpDownSelection(KeyboardEventArgs args)
        {
            if (IsListRender && ListDataSource != null && ListDataSource.Any())
            {
                var listItems = ListDataSource.Where(list => !list.IsHeader && !list.ListClass.Contains(HIDE_LIST, StringComparison.Ordinal))?.ToList();
                int pageCount = await InvokeMethod<int>("sfBlazor.MultiSelect.getPageCount", false, new object[] { PopupElement });
                ListOptions<TItem> previousItem;
                var focusItem = ListDataSource.Where(list => !list.IsHeader && list.ListClass.Contains(ITEM_FOCUS, StringComparison.Ordinal))?.FirstOrDefault();
                var focusIndex = focusItem != null ? ListDataSource.IndexOf(focusItem) : 0;
                if (args.Code == "PageUp")
                {
                    int step = focusIndex - pageCount;
                    previousItem = (step >= 0) ? listItems.ElementAtOrDefault(step + 1) : listItems.FirstOrDefault();
                }
                else
                {
                    int step = focusIndex + pageCount;
                    previousItem = (step <= listItems.Count) ? listItems.ElementAtOrDefault(step - 1) : listItems.LastOrDefault();
                }

                await UpdateListSelection(previousItem, ITEM_FOCUS, true);
            }
        }

        private async Task UpdateHomeEndAction(KeyboardEventArgs args)
        {
            if (ListDataSource != null && ListDataSource.Any() && IsListRender)
            {
                var listItems = ListDataSource.Where(list => !list.IsHeader && !list.ListClass.Contains(HIDE_LIST, StringComparison.Ordinal))?.ToList();
                int findItem = (args.Code == "Home") ? 0 : listItems.Where(list => !list.IsHeader && !list.ListClass.Contains(HIDE_LIST, StringComparison.Ordinal)).ToList().Count - 1;
                var focusItem = listItems.ElementAtOrDefault(findItem);
                await UpdateListSelection(focusItem, ITEM_FOCUS, true);
            }
        }

        private async Task UpdateUpDownAction(KeyboardEventArgs args)
        {
            if (args.AltKey)
            {
                if (args.Code == "ArrowDown")
                {
                    if (!IsListRender)
                    {
                        await ShowPopup();
                    }
                }
                else if (args.Code == "ArrowUp")
                {
                    if (IsListRender)
                    {
                        await HidePopup();
                    }

                    if (Mode == VisualMode.CheckBox)
                    {
                        IsInternalFocus = true;
                        await FocusIn();
                    }
                }
            }
            else if (ListDataSource != null && ListDataSource.Any() && IsListRender)
            {
                PreventKeydown = true;
                var listItems = ListDataSource.Where(list => !list.IsHeader && !list.ListClass.Contains(HIDE_LIST, StringComparison.Ordinal))?.ToList();
                if (Mode == VisualMode.CheckBox && EnableSelectionOrder && SelectedData.Count > 0)
                {
                    listItems = SelectedListData.Concat(ListItemData).Where(item => !item.IsHeader).ToList();
                }

                var focusItem = listItems.Where(item => item.ListClass.Contains(ITEM_FOCUS, StringComparison.Ordinal))?.FirstOrDefault();
                var focusIndex = focusItem != null ? listItems.IndexOf(focusItem) : 0;
                if (focusItem != null)
                {
                    focusIndex = args.Code == "ArrowDown" ? ++focusIndex : --focusIndex;
                    focusIndex = focusIndex < 0 ? 0 : focusIndex > listItems.Count ? listItems.Count - 1 : focusIndex;
                }

                ListOptions<TItem> nextItem = listItems.ElementAtOrDefault(focusIndex);
                await UpdateListSelection(nextItem, ITEM_FOCUS, true);
            }
            else if (!IsListRender)
            {
                await ShowPopup();
            }
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                var destroyArgs = new object[] { InputElement, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, GetProperty() };
                InvokeMethod("sfBlazor.MultiSelect.destroy", destroyArgs).ContinueWith(t =>
                {
                    _ = SfBaseUtils.InvokeEvent<object>(MultiselectEvents?.Destroyed, null);
                }, TaskScheduler.Current);
                ObservableEventDisposed();
                ListDataSource = null;
            }

            FilterinputObj = null;
            BackIcon = null;
            MultiselectEvents = null;
            FilterAttributes = null;
            ContainerAttr = null;
            containerAttributes = null;
            inputAttr = null;
            SelectedData = new List<SelectedData<TItem>>();
            PopupEventArgs = null;
            SelectedListData = null;
        }
    }
}
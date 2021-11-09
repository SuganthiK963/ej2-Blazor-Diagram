using System;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.ComponentModel;
using Syncfusion.Blazor;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Popups;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Interface for a class AutoComplete.
    /// </summary>
    public class AutoCompleteModel
    {
        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionBegin")]
        public ActionBeginEventArgs ActionBegin { get; set; } = null;

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionComplete")]
        public ActionCompleteEventArgs ActionComplete { get; set; } = null;

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("actionFailure")]
        public EventCallback<object> ActionFailure { get; set; }

        /// <summary>
        /// Triggers when the popup before opens.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("beforeOpen")]
        public BeforeOpenEventArgs BeforeOpen { get; set; } = null;

        /// <summary>
        /// Triggers when focus moves out from the component.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// <para>Triggers when an item in a popup is selected or when the model value is changed by user.</para>
        /// <para>Use Change event to configure the cascading AutoComplete.</para>
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("close")]
        public EventCallback<object> Close { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers on set a custom value.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("customValueSpecifier")]
        public EventCallback<object> CustomValueSpecifier { get; set; }

        /// <summary>
        /// Triggers when data source is populated in the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("dataBound")]
        public DataBoundEventArgs DataBound { get; set; } = null;

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers on typing a character in the component.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("filtering")]
        public EventCallback<object> Filtering { get; set; }

        /// <summary>
        /// Triggers when the component is focused.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers when the popup opens.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("open")]
        public EventCallback<object> Open { get; set; }

        /// <summary>
        /// Triggers when an item in the popup is selected by the user either with mouse/tap or with keyboard navigation.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("select")]
        public EventCallback<object> Select { get; set; }

        /// <summary>
        /// Specifies whether the component allows user defined value which does not exist in data source.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("allowCustom")]
        public bool AllowCustom { get; set; } = true;

        /// <summary>
        /// Specifies whether suggest a first matched item in input when searching. No action happens when no matches found.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("autofill")]
        public bool Autofill { get; set; } = false;

        /// <summary>
        /// Determines on which filter type, the component needs to be considered on search action.
        /// </summary>
        [DefaultValue(FilterType.Contains)]
        [JsonProperty("filterType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterType FilterType { get; set; } = FilterType.Contains;

        /// <summary>
        /// When set to ‘true’, highlight the searched characters on suggested list items.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("highlight")]
        public bool Highlight { get; set; } = false;

        /// <summary>
        /// Allows you to set the minimum search character length,
        /// the search action will perform after typed minimum characters.
        /// </summary>
        [DefaultValue(1)]
        [JsonProperty("minLength")]
        public int MinLength { get; set; } = 1;

        /// <summary>
        /// <para>Specifies whether to show or hide the clear button.</para>
        /// <para>When the clear button is clicked, `Value`, `Text`, and `Index` properties are reset to null.</para>
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Allows you to either show or hide the popup button on the component.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("showPopupButton")]
        public bool ShowPopupButton { get; set; } = false;

        /// <summary>
        /// Supports the specified number of list items on the suggestion popup.
        /// </summary>
        [DefaultValue(20)]
        [JsonProperty("suggestionCount")]
        public int SuggestionCount { get; set; } = 20;
    }

    /// <summary>
    /// Defines the custom value specifier event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of CustomValueSpecifierEventArgs.</typeparam>
    public class CustomValueSpecifierEventArgs<T>
    {
        /// <summary>
        /// Sets the text custom format data for set a `Value` and `Text`.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Gets the typed custom text to make a own text format and assign it to `item` argument.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Interface for a class ComboBox.
    /// </summary>
    public class ComboBoxModel
    {
        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionBegin")]
        public ActionBeginEventArgs ActionBegin { get; set; } = null;

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionComplete")]
        public ActionCompleteEventArgs ActionComplete { get; set; } = null;

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("actionFailure")]
        public EventCallback<object> ActionFailure { get; set; }

        /// <summary>
        /// Triggers when the popup before opens.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("beforeOpen")]
        public BeforeOpenEventArgs BeforeOpen { get; set; } = null;

        /// <summary>
        /// Triggers when focus moves out from the component.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// <para>Triggers when an item in a popup is selected or when the model value is changed by user.</para>
        /// <para>Use Change event to configure the cascading ComboBox.</para>
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("close")]
        public EventCallback<object> Close { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers on set a custom value.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("customValueSpecifier")]
        public EventCallback<object> CustomValueSpecifier { get; set; }

        /// <summary>
        /// Triggers when data source is populated in the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("dataBound")]
        public DataBoundEventArgs DataBound { get; set; } = null;

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers on typing a character in the component.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("filtering")]
        public EventCallback<object> Filtering { get; set; }

        /// <summary>
        /// Triggers when the component is focused.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers when the popup opens.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("open")]
        public EventCallback<object> Open { get; set; }

        /// <summary>
        /// Triggers when an item in the popup is selected by the user either with mouse/tap or with keyboard navigation.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("select")]
        public EventCallback<object> Select { get; set; }

        /// <summary>
        /// Specifies whether the component allows user defined value which does not exist in data source.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("allowCustom")]
        public bool AllowCustom { get; set; } = true;

        /// <summary>
        /// Specifies whether suggest a first matched item in input when searching. No action happens when no matches found.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("autofill")]
        public bool Autofill { get; set; } = false;

        /// <summary>
        /// <para>Specifies whether to show or hide the clear button.</para>
        /// <para>When the clear button is clicked, `Value`, `Text`, and `Index` properties are reset to null.</para>
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; } = true;
    }

    /// <summary>
    /// Defines the action begin event.
    /// </summary>
    public class ActionBeginEventArgs
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        ///  Specify the Event Name.
        /// </summary>
        [JsonProperty("eventName")]
        public string EventName { get; set; }

        /// <summary>
        /// Specify the query to begin the data.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("query")]
        public Syncfusion.Blazor.Data.Query Query { get; set; } = null;
    }

    /// <summary>
    /// Defines the action complete event.
    /// </summary>
    /// <typeparam name="TItem">Specifies the ActionCompleteEventArgs.</typeparam>
    public class ActionCompleteEventArgs<TItem>
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Return the total number for records.
        /// </summary>
        [JsonProperty("count")]
        public double Count { get; set; }

        /// <summary>
        /// Specify the Event Name.
        /// </summary>
        [JsonProperty("eventName")]
        public string EventName { get; set; }

        /// <summary>
        /// Specify the query to complete the data.
        /// </summary>
        public Syncfusion.Blazor.Data.Query Query { get; set; }

        /// <summary>
        /// Returns the selected items as JSON Object from the data source.
        /// </summary>
        public IEnumerable<TItem> Result { get; set; }
    }

    /// <summary>
    /// Specifies the action complete event.
    /// </summary>
    public class ActionCompleteEventArgs
    {
        /// <summary>
        /// Return the actual records.
        /// </summary>
        [JsonProperty("actual")]
        public object Actual { get; set; }

        /// <summary>
        /// Return the aggregates.
        /// </summary>
        [JsonProperty("aggregates")]
        public object Aggregates { get; set; }

        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Return the total number for records.
        /// </summary>
        [JsonProperty("count")]
        public double Count { get; set; }

        /// <summary>
        /// Specify the Event Name.
        /// </summary>
        [JsonProperty("eventName")]
        public string EventName { get; set; }

        /// <summary>
        /// Return Items.
        /// </summary>
        [JsonProperty("items")]
        public object Items { get; set; }

        /// <summary>
        /// Specify the query to complete the data.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("query")]
        public Syncfusion.Blazor.Data.Query Query { get; set; } = null;

        /// <summary>
        /// Return the request type.
        /// </summary>
        [JsonProperty("request")]
        public string Request { get; set; }

        /// <summary>
        /// Returns the selected items as JSON Object from the data source.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("result")]
        public object Result { get; set; } = null;

        /// <summary>
        /// Return the virtualSelectRecords.
        /// </summary>
        [JsonProperty("virtualSelectRecords")]
        public object VirtualSelectRecords { get; set; }

        /// <summary>
        /// Return XMLHttpRequest.
        /// </summary>
        [JsonProperty("xhr")]
        public object Xhr { get; set; }
    }

    /// <summary>
    /// Specifies the before open event.
    /// </summary>
    public class BeforeOpenEventArgs
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Specifies the closed event.
    /// </summary>
    public class ClosedEventArgs
    {
    }

    /// <summary>
    /// Specifies the data bound event.
    /// </summary>
    public class DataBoundEventArgs
    {
        /// <summary>
        /// Return the bounded objects.
        /// </summary>
        public object E { get; set; }

        /// <summary>
        /// Returns the selected items as JSON Object from the data source.
        /// </summary>
        public object Items { get; set; }
    }

    /// <summary>
    /// Defines the dropdown base class list.
    /// </summary>
    public class DropDownBaseClassList
    {
        /// <summary>
        /// Specifies the content.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Specifies the disabled property.
        /// </summary>
        [JsonProperty("disabled")]
        public string Disabled { get; set; }

        /// <summary>
        /// Specifies the fixedHead property.
        /// </summary>
        [JsonProperty("fixedHead")]
        public string FixedHead { get; set; }

        /// <summary>
        /// Specifies the focus property.
        /// </summary>
        [JsonProperty("focus")]
        public string Focus { get; set; }

        /// <summary>
        /// Specifies the group property.
        /// </summary>
        [JsonProperty("group")]
        public string Group { get; set; }

        /// <summary>
        /// Specifies the grouping property.
        /// </summary>
        [JsonProperty("grouping")]
        public string Grouping { get; set; }

        /// <summary>
        /// Specifies the hover property.
        /// </summary>
        [JsonProperty("hover")]
        public string Hover { get; set; }

        /// <summary>
        /// Specifies the list property.
        /// </summary>
        [JsonProperty("li")]
        public string Li { get; set; }

        /// <summary>
        /// Specifies the NoData property.
        /// </summary>
        [JsonProperty("noData")]
        public string NoData { get; set; }

        /// <summary>
        /// Specifies the root property.
        /// </summary>
        [JsonProperty("root")]
        public string Root { get; set; }

        /// <summary>
        /// Specifies the Rtl property.
        /// </summary>
        [JsonProperty("rtl")]
        public string Rtl { get; set; }

        /// <summary>
        /// Specifies the selected property.
        /// </summary>
        [JsonProperty("selected")]
        public string Selected { get; set; }
    }

    /// <summary>
    /// Specifies the filtering event.
    /// </summary>
    public class FilteringEventArgs
    {
        /// <summary>
        /// Gets the `keyup` event arguments.
        /// </summary>
        [JsonProperty("baseEventArgs")]
        public object BaseEventArgs { get; set; }

        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// To prevent the internal filtering action.
        /// </summary>
        [JsonProperty("preventDefaultAction")]
        public bool PreventDefaultAction { get; set; }

        /// <summary>
        /// Search text value.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Specifies the UpdateData property.
        /// </summary>
        [JsonProperty("updateData")]
        public object UpdateData { get; set; }
    }

    /// <summary>
    /// Specifies the focus event.
    /// </summary>
    public class FocusEventArgs
    {
        /// <summary>
        /// Specifies the event.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Specifies the focus interacted.
        /// </summary>
        [JsonProperty("isInteracted")]
        public bool IsInteracted { get; set; }
    }

    /// <summary>
    /// Defines the popup event.
    /// </summary>
    public class PopupEventArgs
    {
        /// <summary>
        /// Specifies the animation Object.
        /// </summary>
        [JsonProperty("animation")]
        public object Animation { get; set; }

        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the popuo Object.
        /// </summary>
        [JsonProperty("cancel")]
        public PopupModel Popup { get; set; }
    }

    /// <summary>
    /// Defines the result data.
    /// </summary>
    public class ResultData
    {
        /// <summary>
        /// To return the JSON result.
        /// </summary>
        [JsonProperty("result")]
        public object Result { get; set; }
    }

    /// <summary>
    /// Defines the select event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of SelectEventArgs.</typeparam>
    public class SelectEventArgs<T>
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("e")]
        public object E { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        [JsonProperty("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the selected list item.
        /// </summary>
        [JsonProperty("item")]
        public DOM Item { get; set; }

        /// <summary>
        /// Returns the selected item as JSON Object from the data source.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("itemData")]
        public T ItemData { get; set; } = default;
    }

    /// <summary>
    /// Defines the select event.
    /// </summary>
    public class SelectEventArgs
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("e")]
        public object E { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        [JsonProperty("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the selected list item.
        /// </summary>
        [JsonProperty("item")]
        public DOM Item { get; set; }

        /// <summary>
        /// Returns the selected item as JSON Object from the data source.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("itemData")]
        public object ItemData { get; set; } = null;
    }

    /// <summary>
    /// Interface for a class DropDownBase.
    /// </summary>
    public class DropDownBaseModel
    {
        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionBegin")]
        public ActionBeginEventArgs ActionBegin { get; set; } = null;

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionComplete")]
        public ActionCompleteEventArgs ActionComplete { get; set; } = null;

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("actionFailure")]
        public EventCallback<object> ActionFailure { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when data source is populated in the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("dataBound")]
        public DataBoundEventArgs DataBound { get; set; } = null;

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when an item in the popup is selected by the user either with mouse/tap or with keyboard navigation.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("select")]
        public EventCallback<object> Select { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;
    }

    /// <summary>
    /// Interface for a class FieldSettings.
    /// </summary>
    public class FieldSettingsModel
    {
        /// <summary>
        /// Group the list items with it's related items by mapping GroupBy field.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("groupBy")]
        public string GroupBy { get; set; } = null;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the component considers the property value.</para>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("htmlAttributes")]
        public string HtmlAttributes { get; set; } = null;

        /// <summary>
        /// Maps the icon class column from data table for each list item.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("iconCss")]
        public string IconCss { get; set; } = null;

        /// <summary>
        /// Maps the text column from data table for each list item.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("text")]
        public string Text { get; set; } = null;

        /// <summary>
        /// Maps the value column from data table for each list item.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("value")]
        public string Value { get; set; } = null;
    }

    /// <summary>
    /// Defines the change event.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of ChangeEventArgs.</typeparam>
    /// <typeparam name="TItem">Specifies the typr of value.</typeparam>
    public class ChangeEventArgs<TValue, TItem>
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        public object E { get; set; }

        /// <summary>
        /// Returns the root element of the component.
        /// </summary>
        public DOM Element { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the selected list item.
        /// </summary>
        public DOM Item { get; set; }

        /// <summary>
        /// Returns the selected item as JSON Object from the data source.
        /// </summary>
        public TItem ItemData { get; set; }

        /// <summary>
        /// Returns the previous selected list item.
        /// </summary>
        public DOM PreviousItem { get; set; }

        /// <summary>
        /// Returns the previous selected item as JSON Object from the data source.
        /// </summary>
        public TItem PreviousItemData { get; set; }

        /// <summary>
        /// Returns the selected value.
        /// </summary>
        public TValue Value { get; set; }
    }

    /// <summary>
    /// Specifies the change event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of ChangeEventArgs.</typeparam>
    public class ChangeEventArgs<T>
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("e")]
        public object E { get; set; }

        /// <summary>
        /// Returns the root element of the component.
        /// </summary>
        [JsonProperty("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        [JsonProperty("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the selected list item.
        /// </summary>
        [JsonProperty("item")]
        public DOM Item { get; set; }

        /// <summary>
        /// Returns the selected item as JSON Object from the data source.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("itemData")]
        public object ItemData { get; set; } = null;

        /// <summary>
        /// Returns the previous selected list item.
        /// </summary>
        [JsonProperty("previousItem")]
        public DOM PreviousItem { get; set; }

        /// <summary>
        /// Returns the previous selected item as JSON Object from the data source.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("previousItemData")]
        public object PreviousItemData { get; set; } = null;

        /// <summary>
        /// Returns the selected value.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }
    }

    /// <summary>
    /// Defines the  DropDownList class list.
    /// </summary>
    public class DropDownListClassList
    {
        /// <summary>
        /// Specifies the back icon.
        /// </summary>
        [JsonProperty("backIcon")]
        public string BackIcon { get; set; }

        /// <summary>
        /// Specifies the base.
        /// </summary>
        [JsonProperty("base")]
        public string Base { get; set; }

        /// <summary>
        /// Specifies the clear icon.
        /// </summary>
        [JsonProperty("clearIcon")]
        public string ClearIcon { get; set; }

        /// <summary>
        /// specifies the clear icon hide class.
        /// </summary>
        [JsonProperty("clearIconHide")]
        public string ClearIconHide { get; set; }

        /// <summary>
        /// Specifies the device.
        /// </summary>
        [JsonProperty("device")]
        public string Device { get; set; }

        /// <summary>
        /// Specifies the disable property.
        /// </summary>
        [JsonProperty("disable")]
        public string Disable { get; set; }

        /// <summary>
        /// Specifies the disable icon.
        /// </summary>
        [JsonProperty("disableIcon")]
        public string DisableIcon { get; set; }

        /// <summary>
        /// SPecifies the filter bar clear icon.
        /// </summary>
        [JsonProperty("filterBarClearIcon")]
        public string FilterBarClearIcon { get; set; }

        /// <summary>
        /// Specfies the filter input.
        /// </summary>
        [JsonProperty("filterInput")]
        public string FilterInput { get; set; }

        /// <summary>
        /// Specifies the filter parent.
        /// </summary>
        [JsonProperty("filterParent")]
        public string FilterParent { get; set; }

        /// <summary>
        /// Specifies the focus property.
        /// </summary>
        [JsonProperty("focus")]
        public string Focus { get; set; }

        /// <summary>
        /// Specifies the footer property.
        /// </summary>
        [JsonProperty("footer")]
        public string Footer { get; set; }

        /// <summary>
        /// Specifies the header property.
        /// </summary>
        [JsonProperty("header")]
        public string Header { get; set; }

        /// <summary>
        /// Specifies the hidden element.
        /// </summary>
        [JsonProperty("hiddenElement")]
        public string HiddenElement { get; set; }

        /// <summary>
        /// Specifies the hover property.
        /// </summary>
        [JsonProperty("hover")]
        public string Hover { get; set; }

        /// <summary>
        /// Specifies the icon property.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Specifies the icon animation property.
        /// </summary>
        [JsonProperty("iconAnimation")]
        public string IconAnimation { get; set; }

        /// <summary>
        /// Specifies the input property.
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; set; }

        /// <summary>
        /// Specifies the input focus property.
        /// </summary>
        [JsonProperty("inputFocus")]
        public string InputFocus { get; set; }

        /// <summary>
        /// Specifies the list property.
        /// </summary>
        [JsonProperty("li")]
        public string Li { get; set; }

        /// <summary>
        /// Specifies the mobile filter property.
        /// </summary>
        [JsonProperty("mobileFilter")]
        public string MobileFilter { get; set; }

        /// <summary>
        /// Specifies the popup full screen property.
        /// </summary>
        [JsonProperty("popupFullScreen")]
        public string PopupFullScreen { get; set; }

        /// <summary>
        /// Specifies the root property.
        /// </summary>
        [JsonProperty("root")]
        public string Root { get; set; }

        /// <summary>
        /// Specifies the RTL property.
        /// </summary>
        [JsonProperty("rtl")]
        public string Rtl { get; set; }

        /// <summary>
        /// Specifies the selected property.
        /// </summary>
        [JsonProperty("selected")]
        public string Selected { get; set; }

        /// <summary>
        /// Specifies the value property.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    /// <summary>
    /// Interface for a class DropDownList.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of DropDownListModel.</typeparam>
    /// <typeparam name="TItem">Specifies the typr of value.</typeparam>
    public class DropDownListModel<TValue, TItem>
    {
        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionBegin")]
        public ActionBeginEventArgs ActionBegin { get; set; } = null;

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionComplete")]
        public ActionCompleteEventArgs ActionComplete { get; set; } = null;

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("actionFailure")]
        public EventCallback<object> ActionFailure { get; set; }

        /// <summary>
        /// Triggers when the popup before opens.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("beforeOpen")]
        public BeforeOpenEventArgs BeforeOpen { get; set; } = null;

        /// <summary>
        /// Triggers when focus moves out from the component.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// <para>Triggers when an item in a popup is selected or when the model value is changed by user.</para>
        /// <para>Use Change event to configure the cascading DropDownList.</para>
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("close")]
        public EventCallback<object> Close { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when data source is populated in the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("dataBound")]
        public DataBoundEventArgs DataBound { get; set; } = null;

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers on typing a character in the filter bar when the AllowFiltering is enabled.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("filtering")]
        public EventCallback<object> Filtering { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the DropDownList allows the user to interact with it.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Triggers when the component is focused.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers when the popup opens.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("open")]
        public EventCallback<object> Open { get; set; }

        /// <summary>
        /// Triggers when an item in the popup is selected by the user either with mouse/tap or with keyboard navigation.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("select")]
        public EventCallback<object> Select { get; set; }

        /// <summary>
        /// <para>When AllowFiltering is set to true, show the filter bar (search box) of the component.</para>
        /// <para>The filter action retrieves matched items through the `Filtering` event based on
        /// the characters typed in the search TextBox.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("allowFiltering")]
        public bool AllowFiltering { get; set; } = false;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the DropDownList. One or more custom CSS classes can be added to a DropDownList.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("cssClass")]
        public string CssClass { get; set; } = null;

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;

        /// <summary>
        /// Accepts the value to be displayed as a watermark text on the filter bar.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("filterBarPlaceholder")]
        public string FilterBarPlaceholder { get; set; } = null;

        /// <summary>
        /// Specifies the floating label behavior of the DropDownList that the placeholder text floats above the DropDownList based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DropDownList when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DropDownList.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DropDownList after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        [DefaultValue(Syncfusion.Blazor.Inputs.FloatLabelType.Never)]
        [JsonProperty("floatLabelType")]
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; } = Syncfusion.Blazor.Inputs.FloatLabelType.Never;

        /// <summary>
        /// Accepts the template design and assigns it to the footer container of the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("footerTemplate")]
        public RenderFragment FooterTemplate { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the header container of the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("headerTemplate")]
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the component considers the property value.</para>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("htmlAttributes")]
        public object HtmlAttributes { get; set; } = null;

        /// <summary>
        /// Gets or sets the index of the selected item in the component.
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DropDownList. The property is depending on the FloatLabelType property.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("placeholder")]
        public string Placeholder { get; set; } = null;

        /// <summary>
        /// Specifies the height of the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("popupHeight")]
        public string PopupHeight { get; set; } = null;

        /// <summary>
        /// Specifies the width of the popup list. By default, the popup width sets based on the width of
        /// the component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("popupWidth")]
        public string PopupWidth { get; set; } = null;

        /// <summary>
        /// Specifies the boolean value whether the DropDownList allows the user to change the value.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("readonly")]
        public bool Readonly { get; set; } = false;

        /// <summary>
        /// <para>Accepts the list items either through local or remote service and binds it to the component.</para>
        /// <para>It can be an array of JSON Objects or an instance of
        /// `DataManager`.</para>
        /// </summary>
        [JsonProperty("dataSource")]
        [JsonIgnore]
        public IEnumerable<TItem> DataSource { get; set; }

        /// <summary>
        /// Accepts the template and assigns it to the popup list content of the component
        /// when the data fetch request from the remote server fails.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionFailureTemplate")]
        public RenderFragment ActionFailureTemplate { get; set; }

        /// <summary>
        /// <para>Specifies whether to show or hide the clear button.</para>
        /// <para>When the clear button is clicked, `Value`, `Text`, and `Index` properties are reset to null.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; } = false;

        /// <summary>
        /// Gets or sets the display text of the selected item in the component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("text")]
        public string Text { get; set; } = null;

        /// <summary>
        /// Gets or sets the value of the selected item in the component.
        /// </summary>
        [JsonProperty("value")]
        public TValue Value { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the selected list item in the input element of the component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("valueTemplate")]
        public RenderFragment<TValue> ValueTemplate { get; set; }

        /// <summary>
        /// Specifies the width of the component. By default, the component width sets based on the width of
        /// its parent container.<para> You can also set the width in pixel values.</para>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("width")]
        public string Width { get; set; } = null;

        /// <summary>
        /// Determines on which filter type, the component needs to be considered on search action.
        /// </summary>
        [DefaultValue(FilterType.StartsWith)]
        [JsonProperty("filterType")]
        [Parameter]
        public FilterType FilterType { get; set; }

        /// <summary>
        /// <para>When set to `false`, consider the `case-sensitive` on performing the search to find suggestions.</para>
        /// <para>By default, consider the casing.</para>
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("ignoreCase")]
        [Parameter]
        public bool IgnoreCase { get; set; } = true;

        /// <summary>
        /// ignoreAccent set to true, then ignores the diacritic characters or accents when filtering.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("ignoreAccent")]
        [Parameter]
        public bool IgnoreAccent { get; set; }

        /// <summary>
        /// Accepts the external `Query` that execute along with data processing.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("query")]
        [Parameter]
        public Syncfusion.Blazor.Data.Query Query { get; set; }

        /// <summary>
        /// The Virtual Scrolling feature is used to display a large amount of data that you require without buffering the entire load of a huge database records in the DropDowns, that is, when scrolling, the datamanager request is sent to fetch some amount of data from the server dynamically.
        /// To achieve this scenario with DropDowns, set the EnableVirtualization to true.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableVirtualization")]
        [Parameter]
        public bool EnableVirtualization { get; set; }
    }

    /// <summary>
    /// Interface for a class ListBox.
    /// </summary>
    public class ListBoxModel<T>
    {
        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionBegin")]
        public ActionBeginEventArgs ActionBegin { get; set; } = null;

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionComplete")]
        public ActionCompleteEventArgs ActionComplete { get; set; } = null;

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("actionFailure")]
        public EventCallback<object> ActionFailure { get; set; }

        /// <summary>
        /// Triggers before dropping the list item on another list item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("beforeDrop")]
        public EventCallback<object> BeforeDrop { get; set; }

        /// <summary>
        /// Triggers while rendering each list item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("beforeItemRender")]
        public EventCallback<object> BeforeItemRender { get; set; }

        /// <summary>
        /// Triggers while select / unselect the list item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers while dragging the list item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("drag")]
        public EventCallback<object> Drag { get; set; }

        /// <summary>
        /// Triggers after dragging the list item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("dragStart")]
        public EventCallback<object> DragStart { get; set; }

        /// <summary>
        /// Triggers before dropping the list item on another list item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("drop")]
        public EventCallback<object> Drop { get; set; }

        /// <summary>
        /// Triggers on typing a character in the component.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("filtering")]
        public EventCallback<object> Filtering { get; set; }

        /// <summary>
        /// If 'allowDragAndDrop' is set to true, then you can perform drag and drop of the list item.
        /// ListBox contains same 'scope' property enables drag and drop between multiple ListBox.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("allowDragAndDrop")]
        public bool AllowDragAndDrop { get; set; } = false;

        /// <summary>
        /// To enable the filtering option in this component.
        /// Filter action performs when type in search box and collect the matched item through `filtering` event.
        /// If searching character does not match, `noRecordsTemplate` property value will be shown.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("allowFiltering")]
        public bool AllowFiltering { get; set; } = false;

        /// <summary>
        /// Sets the CSS classes to root element of this component, which helps to customize the
        /// complete styles.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("cssClass")]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;

        /// <summary>
        /// Sets the height of the ListBox component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("height")]
        public object Height { get; set; } = null;

        /// <summary>
        /// Sets limitation to the value selection.
        /// Based on the limitation, list selection will be prevented.
        /// </summary>
        [DefaultValue(1000)]
        [JsonProperty("maximumSelectionLength")]
        public double MaximumSelectionLength { get; set; } = 1000;

        /// <summary>
        /// Defines the scope value to group sets of draggable and droppable ListBox.
        /// A draggable with the same scope value will be accepted by the droppable.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("scope")]
        public string Scope { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the selection mode and its type.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("selectionSettings")]
        public SelectionSettingsModel SelectionSettings { get; set; } = null;

        /// <summary>
        /// Specifies the toolbar items and its position.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("toolbarSettings")]
        public ToolbarSettingsModel ToolbarSettings { get; set; } = null;

        /// <summary>
        /// Sets the specified item to the selected state or gets the selected item in the ListBox.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }
    }

    /// <summary>
    /// Interface for a class SelectionSettings.
    /// </summary>
    public class SelectionSettingsModel
    {
        /// <summary>
        /// Set the position of the checkbox.
        /// </summary>
        [DefaultValue(CheckBoxPosition.Left)]
        [JsonProperty("checkboxPosition")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CheckBoxPosition CheckboxPosition { get; set; } = CheckBoxPosition.Left;

        /// <summary>
        /// Specifies the selection modes. The possible values are
        ///  `Single`: Allows you to select a single item in the ListBox.
        ///  `Multiple`: Allows you to select more than one item in the ListBox.
        /// </summary>
        [DefaultValue(SelectionMode.Multiple)]
        [JsonProperty("mode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SelectionMode Mode { get; set; } = SelectionMode.Multiple;

        /// <summary>
        /// If 'showCheckbox' is set to true, then 'checkbox' will be visualized in the list item.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("showCheckbox")]
        public bool ShowCheckbox { get; set; } = false;

        /// <summary>
        /// Allows you to either show or hide the selectAll option on the component.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("showSelectAll")]
        public bool ShowSelectAll { get; set; } = false;

    }

    /// <summary>
    /// Interface for a class ToolbarSettings.
    /// </summary>
    public class ToolbarSettingsModel
    {
        /// <summary>
        /// Specifies the list of tools for dual ListBox.
        /// The predefined tools are 'moveUp', 'moveDown', 'moveTo', 'moveFrom', 'moveAllTo', and 'moveAllFrom'.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("items")]
        public string[] Items { get; set; } = null;

        /// <summary>
        /// Positions the toolbar before/after the ListBox.
        /// The possible values are:
        ///  Left: The toolbar will be positioned to the left of the ListBox.
        ///  Right: The toolbar will be positioned to the right of the ListBox.
        /// </summary>
        [DefaultValue(ToolBarPosition.Right)]
        [JsonProperty("position")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ToolBarPosition Position { get; set; } = ToolBarPosition.Right;

    }

    /// <summary>
    /// Interface for update list arguments.
    /// </summary>
    public class IUpdateListArgs
    {
        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("e")]
        public object E { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        /// <summary>
        /// Specifies the  property.
        /// </summary>
        [JsonProperty("index")]
        public double Index { get; set; }

        /// <summary>
        /// Specifies the list property.
        /// </summary>
        [JsonProperty("li")]
        public DOM Li { get; set; }

        /// <summary>
        /// Specifies the module  property.
        /// </summary>
        [JsonProperty("module")]
        public string Module { get; set; }

        /// <summary>
        /// Specifies the popup element  property.
        /// </summary>
        [JsonProperty("popupElement")]
        public DOM PopupElement { get; set; }

        /// <summary>
        /// Specifies the value property.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonIgnore]
        internal IJSRuntime JsRuntime
        {
            get
            {
                return _jsRuntime;
            }

            set
            {
                if (Li != null)
                {
                    Li.JsRuntime = value;
                }

                if (PopupElement != null)
                {
                    PopupElement.JsRuntime = value;
                }

                _jsRuntime = value;
            }
        }

        internal IJSRuntime _jsRuntime { get; set; }

    }

    /// <summary>
    /// Defines the item created event.
    /// </summary>
    public class ItemCreatedArgs
    {
        /// <summary>
        /// Specifies the current data property.
        /// </summary>
        [JsonProperty("curData")]
        public object CurData { get; set; }

        /// <summary>
        /// Specifies the text property.
        /// </summary>
        [JsonProperty("item")]
        public DOM Item { get; set; }

        /// <summary>
        /// Specifies the text property.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonIgnore]
        internal IJSRuntime JsRuntime
        {
            get
            {
                return _jsRuntime;
            }

            set
            {
                if (Item != null)
                {
                    Item.JsRuntime = value;
                }

                _jsRuntime = value;
            }
        }

        internal IJSRuntime _jsRuntime { get; set; }

    }

    /// <summary>
    /// Defines the custom value event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of CustomValueEventArgs.</typeparam>
    public class CustomValueEventArgs<T>
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the newly added data.
        /// </summary>
        public T NewData { get; set; }
        /// <summary>
        /// Gets the typed custom text to make a own text format and assign it to `item` argument.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Defines the select all event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of SelectAllEventArgs.</typeparam>
    public class SelectAllEventArgs<T>
    {
        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        public object Event { get; set; }

        /// <summary>
        /// Specifies whether it is selectAll or deSelectAll.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the selected items as JSON Object from the data source.
        /// </summary>
        public IEnumerable<T> ItemData { get; set; }

        /// <summary>
        /// Returns the selected list items.
        /// </summary>
        public List<DOM> Items { get; set; }
    }

    /// <summary>
    /// Specifies the chip selected event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of ChipSelectedEventArgs.</typeparam>
    public class ChipSelectedEventArgs<T>
    {
        /// <summary>
        /// Returns the selected chip data as list object from the data source.
        /// </summary>
        public T ChipData { get; set; }
    }

    /// <summary>
    /// Defines the MultiSelect change event.
    /// </summary>
    /// <typeparam name="T">Specifies the MultiSelectChangeEventArgs.</typeparam>
    public class MultiSelectChangeEventArgs<T>
    {
        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        public object E { get; set; }

        /// <summary>
        /// Returns the root element of the component.
        /// </summary>
        public DOM Element { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the component initial Value.
        /// </summary>
        public T OldValue { get; set; }

        /// <summary>
        /// Returns the updated component Values.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }
    }

    /// <summary>
    ///  Defines the remove event.
    /// </summary>
    /// <typeparam name="T">Specifies the RemoveEventArgs.</typeparam>
    public class RemoveEventArgs<T>
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        public object E { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the selected list item.
        /// </summary>
        public DOM Item { get; set; }

        /// <summary>
        /// Returns the selected item as JSON Object from the data source.
        /// </summary>
        public T ItemData { get; set; }
    }

    /// <summary>
    /// Defines the tagging event.
    /// </summary>
    /// <typeparam name="T">Specifies the type of TaggingEventArgs.</typeparam>
    public class TaggingEventArgs<T>
    {
        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        public object E { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Returns the selected item as JSON Object from the data source.
        /// </summary>
        public T ItemData { get; set; }

        /// <summary>
        /// To set the classes to chip element.
        /// </summary>
        public string SetClass { get; set; }
    }

    /// <summary>
    /// Interface for a class MultiSelect.
    /// </summary>
    /// <typeparam name="T">Specifies the type of MultiSelectModel.</typeparam>
    public class MultiSelectModel<T>
    {
        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionBegin")]
        public ActionBeginEventArgs ActionBegin { get; set; } = null;

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("actionComplete")]
        public ActionCompleteEventArgs ActionComplete { get; set; } = null;

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("actionFailure")]
        public EventCallback<object> ActionFailure { get; set; }

        /// <summary>
        /// Fires when popup opens before animation.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("beforeOpen")]
        public BeforeOpenEventArgs BeforeOpen { get; set; } = null;

        /// <summary>
        /// Event triggers when the input get focus-out.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// Fires each time when selection changes happened in list items after model and input value get affected.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Event triggers when the chip selection.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("chipSelection")]
        public EventCallback<object> ChipSelection { get; set; }

        /// <summary>
        /// Fires when popup close after animation completion.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("close")]
        public EventCallback<object> Close { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the CustomValue is selected.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("customValueSelection")]
        public EventCallback<object> CustomValueSelection { get; set; }

        /// <summary>
        /// Triggers when data source is populated in the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("dataBound")]
        public DataBoundEventArgs DataBound { get; set; } = null;

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers event,when user types a text in search box.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("filtering")]
        public EventCallback<object> Filtering { get; set; }

        /// <summary>
        /// Event triggers when the input get focused.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Fires when popup opens after animation completion.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("open")]
        public EventCallback<object> Open { get; set; }

        /// <summary>
        /// Fires after the selected item removed from the widget.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("removed")]
        public EventCallback<object> Removed { get; set; }

        /// <summary>
        /// Fires before the selected item removed from the widget.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("removing")]
        public EventCallback<object> Removing { get; set; }

        /// <summary>
        /// Triggers when an item in the popup is selected by the user either with mouse/tap or with keyboard navigation.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("select")]
        public EventCallback<object> Select { get; set; }

        /// <summary>
        /// Fires after select all process completion.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("selectedAll")]
        public EventCallback<object> SelectedAll { get; set; }

        /// <summary>
        /// Fires before set the selected item as chip in the component.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("tagging")]
        public EventCallback<object> Tagging { get; set; }

        /// <summary>
        /// Accepts the template and assigns it to the popup list content of the MultiSelect component
        /// when the data fetch request from the remote server fails.
        /// </summary>
        [DefaultValue("Request failed")]
        [JsonProperty("actionFailureTemplate")]
        public string ActionFailureTemplate { get; set; } = "Request failed";

        /// <summary>
        /// Allows user to add a
        /// custom value the value which is not present in the suggestion list.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("allowCustomValue")]
        public bool AllowCustomValue { get; set; } = false;

        /// <summary>
        /// <para>To enable the filtering option in this component.</para>
        /// <para>Filter action performs when type in search box and collect the matched item through `Filtering` event.</para>
        /// <para>If searching character does not match, `NoRecordsTemplate` property value will be shown.</para>
        /// </summary>
        [JsonProperty("allowFiltering")]
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// <para>By default, the MultiSelect component fires the Change event while focus out the component.</para>
        /// <para>If you want to fires the Change event on every value selection and remove, then disable the ChangeOnBlur property.</para>
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("changeOnBlur")]
        public bool ChangeOnBlur { get; set; } = true;

        /// <summary>
        /// Based on the property, when item get select popup visibility state will changed.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("closePopupOnSelect")]
        public bool ClosePopupOnSelect { get; set; } = true;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the MultiSelect. One or more custom CSS classes can be added to a MultiSelect.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("cssClass")]
        public string CssClass { get; set; } = null;

        /// <summary>
        /// <para>Accepts the list items either through local or remote service and binds it to the MultiSelect component.</para>
        /// <para>It can be an array of JSON Objects or an instance of
        /// `DataManager`.</para>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("dataSource")]
        public object DataSource { get; set; } = null;

        /// <summary>
        /// Sets the delimiter character for 'default' and 'delimiter' visibility modes.
        /// </summary>
        [DefaultValue(",")]
        [JsonProperty("delimiterChar")]
        public string DelimiterChar { get; set; } = ",";

        /// <summary>
        /// <para>Specifies a boolean value that indicates the whether the grouped list items are
        /// allowed to check by checking the group header in checkbox mode.</para>
        /// <para>By default, there is no checkbox provided for group headers.</para>
        /// <para>This property allows you to render checkbox for group headers and to select
        /// all the grouped items at once.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableGroupCheckBox")]
        public bool EnableGroupCheckBox { get; set; } = false;

        /// <summary>
        /// <para>Enable or disable persisting MultiSelect state between page reloads.</para>
        /// <para>If enabled, the `Value` state will be persisted.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Value</term>
        /// </item>
        /// </list>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;

        /// <summary>
        /// Reorder the selected items in popup visibility state.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enableSelectionOrder")]
        public bool EnableSelectionOrder { get; set; } = true;

        /// <summary>
        /// Specifies a boolean value that indicates whether the MultiSelect allows the user to interact with it.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// <para>The `Fields` property maps the columns of the data table and binds the data to the component.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Text</term>
        /// <description>Maps the text column from data table for each list item.</description>
        /// </item>
        /// <item>
        /// <term>Value</term>
        /// <description>Maps the value column from data table for each list item.</description>
        /// </item>
        /// <item>
        /// <term>IconCss</term>
        /// <description>Maps the icon class column from data table for each list item.</description>
        /// </item>
        /// <item>
        /// <term>GroupBy</term>
        /// <description>Group the list items with it's related items by mapping groupBy field.</description>
        /// </item>
        /// </list>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("fields")]
        public FieldSettingsModel Fields { get; set; } = null;

        /// <summary>
        /// Accepts the value to be displayed as a watermark text on the filter bar.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("filterBarPlaceholder")]
        public string FilterBarPlaceholder { get; set; } = null;

        /// <summary>
        /// Determines on which filter type, the MultiSelect component needs to be considered on search action.
        /// </summary>
        [DefaultValue(FilterType.StartsWith)]
        [JsonProperty("filterType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterType FilterType { get; set; } = FilterType.StartsWith;

        /// <summary>
        /// Specifies the floating label behavior of the MultiSelect that the placeholder text floats above the MultiSelect based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the MultiSelect when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the MultiSelect.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the MultiSelect after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        [DefaultValue(Syncfusion.Blazor.Inputs.FloatLabelType.Never)]
        [JsonProperty("floatLabelType")]
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; } = Syncfusion.Blazor.Inputs.FloatLabelType.Never;

        /// <summary>
        /// Accepts the template design and assigns it to the footer container of the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("footerTemplate")]
        public string FooterTemplate { get; set; } = null;

        /// <summary>
        /// Accepts the template design and assigns it to the group headers present in the MultiSelect popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("groupTemplate")]
        public string GroupTemplate { get; set; } = null;

        /// <summary>
        /// Accepts the template design and assigns it to the header container of the popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("headerTemplate")]
        public string HeaderTemplate { get; set; } = null;

        /// <summary>
        /// Hides the selected item from the list item.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("hideSelectedItem")]
        public bool HideSelectedItem { get; set; } = true;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the component considers the property value.</para>
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("htmlAttributes")]
        public object HtmlAttributes { get; set; } = null;

        /// <summary>
        /// ignoreAccent set to true, then ignores the diacritic characters or accents when filtering.
        /// </summary>
        [JsonProperty("ignoreAccent")]
        public bool IgnoreAccent { get; set; }

        /// <summary>
        /// Sets case sensitive option for filter operation.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("ignoreCase")]
        public bool IgnoreCase { get; set; } = true;

        /// <summary>
        /// Accepts the template design and assigns it to each list item present in the popup.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("itemTemplate")]
        public string ItemTemplate { get; set; } = null;

        /// <summary>
        /// Specifies the global culture and localization of the MultiSelect.
        /// </summary>
        [DefaultValue("en-US")]
        [JsonProperty("locale")]
        public string Locale { get; set; } = "en-US";

        /// <summary>
        /// <para>Sets limitation to the value selection.</para>
        /// <para>Based on the limitation, list selection will be prevented.</para>
        /// </summary>
        [DefaultValue(1000)]
        [JsonProperty("maximumSelectionLength")]
        public int MaximumSelectionLength { get; set; } = 1000;

        /// <summary>
        /// configures visibility mode for component interaction.
        /// </summary>
        [DefaultValue(VisualMode.Default)]
        [JsonProperty("mode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public VisualMode Mode { get; set; } = VisualMode.Default;

        /// <summary>
        /// Accepts the template design and assigns it to popup list of MultiSelect component.
        /// when no data is available on the component.
        /// </summary>
        [DefaultValue("No records found")]
        [JsonProperty("noRecordsTemplate")]
        public string NoRecordsTemplate { get; set; } = "No records found";

        /// <summary>
        /// Whether to automatically open the popup when the control is clicked.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("openOnClick")]
        public bool OpenOnClick { get; set; } = true;

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in MultiSelect. The property is depending on the FloatLabelType property.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("placeholder")]
        public string Placeholder { get; set; } = null;

        /// <summary>
        /// Gets or sets the height of the popup list. By default, it renders based on its list item.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("popupHeight")]
        public string PopupHeight { get; set; } = null;

        /// <summary>
        /// Gets or sets the width of the popup list and percentage values has calculated based on input width.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("popupWidth")]
        public string PopupWidth { get; set; } = null;

        /// <summary>
        /// Accepts the external `Query`
        /// which will execute along with the data processing in MultiSelect.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("query")]
        public object Query { get; set; } = null;

        /// <summary>
        /// Specifies the boolean value whether the MultiSelect allows the user to change the value.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("readonly")]
        public bool Readonly { get; set; } = false;

        /// <summary>
        /// Specifies the selectAllText to be displayed on the component.
        /// </summary>
        [DefaultValue("select All")]
        [JsonProperty("selectAllText")]
        public string SelectAllText { get; set; } = "select All";

        /// <summary>
        /// Enables close icon with the each selected item.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Allows you to either show or hide the DropDown button on the component.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("showDropDownIcon")]
        public bool ShowDropDownIcon { get; set; } = false;

        /// <summary>
        /// Allows you to either show or hide the selectAll option on the component.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("showSelectAll")]
        public bool ShowSelectAll { get; set; } = false;

        /// <summary>
        /// <para>Specifies the `SortOrder` to sort the data source.</para>
        /// <para>The available type of sort orders are.</para>
        /// <list type="bullet">
        /// <item>
        /// <term>None</term>
        /// <description>The data source is not sorting.</description>
        /// </item>
        /// <item>
        /// <term>Ascending</term>
        /// <description>The data source is sorting with ascending order.</description>
        /// </item>
        /// <item>
        /// <term>Descending</term>
        /// <description>The data source is sorting with descending order.</description>
        /// </item>
        /// </list>
        /// </summary>
        [DefaultValue(SortOrder.None)]
        [JsonProperty("sortOrder")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder SortOrder { get; set; } = SortOrder.None;

        /// <summary>
        /// Selects the list item which maps the data `Text` field in the component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("text")]
        public string Text { get; set; } = null;

        /// <summary>
        /// Specifies the UnSelectAllText to be displayed on the component.
        /// </summary>
        [DefaultValue("select All")]
        [JsonProperty("unSelectAllText")]
        public string UnSelectAllText { get; set; } = "select All";

        /// <summary>
        /// Selects the list item which maps the data `Value` field in the component.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }

        /// <summary>
        /// Accepts the template design and assigns it to the selected list item in the input element of the component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("valueTemplate")]
        public string ValueTemplate { get; set; } = null;

        /// <summary>
        /// Gets or sets the width of the component. By default, it sizes based on its parent.
        /// container dimension.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("width")]
        public string Width { get; set; } = null;

        /// <summary>
        /// specifies the z-index value of the component popup element.
        /// </summary>
        [DefaultValue(1000)]
        [JsonProperty("zIndex")]
        public double ZIndex { get; set; } = 1000;
    }
}
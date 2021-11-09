using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Lists.Internal;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// The SfListView control represents the data in interactive hierarchical structure interface across different layouts or views,
    /// that also has features such as data-binding, template, grouping and virtualization.
    /// </summary>
    public partial class SfListView<TValue> : SfBaseComponent
    {
        internal const string DISPLAY = "none";
        internal const string ROOTCLASS = "e-control e-listview e-lib e-touch";
        internal const string DATASOURCEKEY = "defaultData_Key";
        internal const string IDPREFIX = "sflistview-";
        internal const string RTL = " e-rtl";
        internal const string HASHEADER = " e-has-header";
        internal const string DISABLED = " e-disabled";
        internal const string STYLE = "style";
        internal const string CLASS = "class";
        internal const string LISTVIEWWIDTH = "width";
        internal const string LISTVIEWHEIGHT = "height";
        internal const string SELECTEDDETAILS = "SelectedElementIdInfo";
        internal const string ITEMROLE = "listitem";
        internal const string LISTROLE = "list";
        internal const string GROUPITEMROLE = "option";
        internal const string WRAPPERROLE = "presentation";
        internal const string SPACE = " ";
        internal const string HEADER = "e-list-header";
        internal const string TEXT = "e-text";
        internal const string HEADERTEXT = "e-headertext";
        internal const string BACKICON = "e-icons e-icon-back e-but-back";
        internal const string HEADERTEMPLATECLASS = "e-headertemplate-text header";
        internal const string ISTRUE = "true";
        internal const string GROUPLISTITEM = "group-list-item";
        internal const int VIRTUALSCROLLDIFF = 3;

        private bool isDSUpdated;
        private int viewPortElementCount;
        private int virtualElementStartIndex;
        private int virtualElementEndIndex;
        private double listElementHeight = 36;
        private Dictionary<string, IEnumerable<TValue>> listViewDataSource = new Dictionary<string, IEnumerable<TValue>>();
        internal ListBaseOptionModel<TValue> optionsInternal = new ListBaseOptionModel<TValue>();
        private bool isSelection;
        private bool isPersistance;

        private AriaAttributesMapping ariaAttributes = new AriaAttributesMapping()
        {
            ItemRole = ITEMROLE,
            ListRole = LISTROLE,
            ItemText = string.Empty,
            GroupItemRole = GROUPITEMROLE,
            WrapperRole = WRAPPERROLE,
            Level = 1
        };

        /// <exclude/>
        /// <summary>
        /// Creating instance of object of DataManager.
        /// </summary>
        public DataManager DataManager { get; set; }

        /// <summary>
        /// Creating the element reference.
        /// </summary>
        private ElementReference ElementRef { get; set; }

        private bool QueryUpdated { get; set; }

        internal SfListBase<TValue> ListBase { get; set; }

        internal ListElementDetails<TValue> ElementDetails { get; set; }

        internal bool IsDestroyedInternal { get; set; }

        internal bool AfterUpdateData { get; set; }

        internal ListViewEvents<TValue> ListViewEvents { get; set; }

        internal void UpdateChildProperties(string key, ListViewFieldSettings<TValue> field)
        {
            if (key == "fields")
            {
                ListFields = field;
            }
        }

        /// <summary>
        /// Referring the Persistence values.
        /// </summary>
        internal class PersistenceValues
        {
            /// <summary>
            /// The `AllCheckedItems` property is used to set the list that need to be checked.
            /// </summary>
            public List<TValue> AllCheckedItems { get; set; }
            /// <summary>
            /// The `CssClass` property is used to add a user-preferred class name in the root element of the ListView,
            ///  using which we can customize the component (both CSS and functionality customization).
            /// </summary>
            public string CssClass { get; set; }

            /// <summary>
            /// Enable or disable rendering component in right to left direction.
            /// </summary>
            public bool EnableRtl { get; set; }

            /// <summary>
            /// If `Enabled` set to true, the list items are enabled.
            /// And, we can disable the component using this property by setting its value as false.
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// The `SortOrder` is used to sort the data source. The available type of sort orders are,
            ///  `None` - The data source is not sorting.
            ///  `Ascending` - The data source is sorting with ascending order.
            ///  `Descending` - The data source is sorting with descending order.
            /// </summary>
            public SortOrder SortOrder { get; set; }
        }

        /// <exclude/>
        /// <summary>
        /// trigger action complete event.
        /// </summary>
        /// <returns>Task.</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerActionComplete(IEnumerable<TValue> dataSource)
        {
            if (ListViewEvents != null && ListViewEvents.OnActionComplete.HasDelegate)
            {

                await SfBaseUtils.InvokeEvent<ActionEventsArgs>(ListViewEvents.OnActionComplete, new ActionEventsArgs()
                {
                    Name = "ActionComplete",
                    Count = dataSource != null ? dataSource.Count() : 0
                });
            }

        }

        /// <exclude/>
        /// <summary>
        /// Rendering Nested List.
        /// </summary>
        /// <param name="reference"> specifies the reference parameter.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void ListChildDataSource(ListElementReference reference)
        {
            ListElementDetails<TValue> details = reference != null ? GetLiElementData(reference, false) : null;
            listViewDataSource.Add(reference?.ElementId, SortOrder != SortOrder.None ? GetSortedData(details.Child) : details.Child);
            StateHasChanged();
            await InvokeMethod("sfBlazor.ListView.renderChildList", ElementRef, reference.ElementId, details.Id != null ? details.Id : new List<string>());
        }

        /// <exclude/>
        /// <summary>
        /// Update Li element height.
        /// </summary>
        /// <param name="height"> specifies the reference parameter.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateLiElementHeight(double height)
        {
            listElementHeight = height;
        }

        /// <exclude/>
        /// <summary>
        /// Update Li element based on scrolling Difference.
        /// </summary>
        /// <param name="listDifference"> specifies the list difference.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void VirtualScrolling(int listDifference)
        {
            virtualElementStartIndex = listDifference < 0 ? 0 : listDifference;
            virtualElementEndIndex = listDifference + viewPortElementCount;
            StateHasChanged();
            await InvokeMethod("sfBlazor.ListView.addActiveClass", ElementRef);
        }

        /// <exclude/>
        /// <summary>
        /// Rendering the LI element based on window height in virtual scrolling.
        /// </summary>
        /// <param name="componentHeight"> specifies the component height.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void GetComponenetHeight(double componentHeight)
        {
            decimal elementCount = Math.Ceiling((decimal)(componentHeight / listElementHeight));
            viewPortElementCount = virtualElementEndIndex = (int)elementCount * VIRTUALSCROLLDIFF;
            StateHasChanged();
            await InvokeMethod("sfBlazor.ListView.updateElementDifference", ElementRef, viewPortElementCount / VIRTUALSCROLLDIFF);
        }

        ///<exclude/>
        // Updating checked nodes
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateData(bool isChecked, string id)
        {
            RecursiveUpdateData(DataSource.ToList(), isChecked, id);
        }

        /// <summary>
        ///  Specifies the method RecursiveUpdateData
        /// </summary>
        /// <param name="newDataSource"></param>
        /// <param name="isChecked"></param>
        /// <param name="id"></param>
        private async void RecursiveUpdateData(List<TValue> newDataSource, bool isChecked, string id)
        {
            Dictionary<string, int> AllCheckedItem = new Dictionary<string, int>();
            for (int i = 0; i < CheckedItems.Count; i++)
            {
                string checkId = DataUtil.GetKeyValue(ListFields.Id, CheckedItems[i]);
                AllCheckedItem.Add(checkId, i);
            }
            for (int i = 0; i < newDataSource.Count; i++)
            {
                if (DataUtil.GetKeyValue(ListFields.Id, newDataSource[i]) == id)
                {
                    string CheckItemId = DataUtil.GetKeyValue(ListFields.Id, newDataSource[i]);
                    if (!AllCheckedItem.ContainsKey(CheckItemId))
                    {
                        CheckedItems.Add(newDataSource[i]);
                    }
                    else if(!isPersistance)
                    {
                        CheckedItems.RemoveAt(AllCheckedItem[CheckItemId]);
                    }
                    if (ListFields.IsChecked != null)
                    {
                        newDataSource[i].GetType().GetProperty(ListFields.IsChecked)?.SetValue(newDataSource[i], isChecked);
                        AfterUpdateData = true;
                    }
                }
                else if (ListFields.Child != null && DataUtil.GetKeyValue(ListFields.Child, newDataSource[i]) != null)
                {
                    RecursiveUpdateData((List<TValue>)GetPropertyValue(newDataSource[i], ListFields.Child), isChecked, id);
                }
            }
            await SetLocalStorage(ID, SerializeModel(this));
        }

        /// <exclude/>
        /// <summary>
        /// Back event handler invoke method.
        /// </summary>
        /// <param name="args"> specifies the list element arguements.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void TriggerBackEvent(ListElementReference args)
        {
            if (args != null)
            {
                BackEventArgs<TValue> eventArgs = new BackEventArgs<TValue>();
                eventArgs = new BackEventArgs<TValue>()
                {
                    IsInteracted = args.IsInteracted,
                    Level = args.Level
                };

                if (ListViewEvents != null)
                {
                    await SfBaseUtils.InvokeEvent(ListViewEvents.OnBack, eventArgs);
                }
            }
        }

        /// <exclude/>
        /// <summary>
        /// Click event handler invoke method.
        /// </summary>
        /// <param name="args"> specifies the list element arguements.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void TriggerClickEvent(ListElementReference args)
        {
            if (args != null)
            {
                ListElementDetails<TValue> details = GetLiElementData(args, false);

                if (args.ElementId != null && !args.ElementId.Contains(GROUPLISTITEM, StringComparison.Ordinal))
                {
                    ClickEventArgs<TValue> eventArgs = new ClickEventArgs<TValue>();
                    if (details.Id != null)
                    {
                        eventArgs = new ClickEventArgs<TValue>()
                        {
                            Cancel = false,
                            ItemData = details.ItemData,
                            Text = details.ItemText,
                            IsInteracted = args.IsInteracted,
                            IsChecked = args.IsChecked,
                            Index = details.Index[0],
                            Level = args.Level,
                            Name = "Clicked"
                        };
                    }
                    if (ListViewEvents != null && ListViewEvents.Clicked.HasDelegate)
                    {
                        isSelection = true;
                        await SfBaseUtils.InvokeEvent(ListViewEvents.Clicked, eventArgs);
                        await InvokeMethod("sfBlazor.ListView.preventSelection", ElementRef, eventArgs.Cancel);
                    }
                }
                if (this.ListFields.GroupBy != null && details.Child == null)
                {
                    await InvokeMethod("sfBlazor.ListView.setCheckedItems", ElementRef, (details.Id != null && !this.ShowCheckBox) ? details.Id : null);
                }
            }
        }

        /// <summary>
        /// get fields property values from TValue tye data source.
        /// </summary>
        private static object GetPropertyValue(TValue dataSource, string property)
        {
            Type valueType = typeof(TValue);
            return valueType.GetProperty(property)?.GetValue(dataSource, null);
        }

        // returns the list item attributes for the list
        private Dictionary<string, object> GetAttributes()
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            string classNames = ROOTCLASS;
            if (!string.IsNullOrEmpty(CssClass))
            {
                classNames += SPACE + CssClass;
            }

            if (EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl))
            {
                classNames += RTL;
            }

            if (ShowHeader)
            {
                classNames += HASHEADER;
            }

            if (!Enabled)
            {
                classNames += DISABLED;
            }

            SfBaseUtils.UpdateDictionary(CLASS, classNames, attributes);
            SfBaseUtils.UpdateDictionary("id", ID, attributes);
            SfBaseUtils.UpdateDictionary("tabindex", "0", attributes);
            SfBaseUtils.UpdateDictionary("role", "list", attributes);
            if (HtmlAttributes != null)
            {
                foreach (string key in HtmlAttributes.Keys)
                {
                    if (key != CLASS && key != STYLE)
                    {
                        SfBaseUtils.UpdateDictionary(key, HtmlAttributes[key], attributes);
                    }
                    else if (HtmlAttributes.ContainsKey(CLASS))
                    {
                        attributes["class"] = SfBaseUtils.AddClass(classNames, HtmlAttributes["class"].ToString());
                    }
                }
            }

            if (!string.IsNullOrEmpty(GetPropertyStyle()))
            {
                SfBaseUtils.UpdateDictionary(STYLE, GetPropertyStyle(), attributes);
            }

            return attributes;
        }

        /// <summary>
        /// Update the template values.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void UpdateTemplate(string name, object template)
        {
            switch (name)
            {
                case nameof(Template):
                    Template = (RenderFragment<TValue>)template;
                    break;
                case nameof(GroupTemplate):
                    GroupTemplate = (RenderFragment<ComposedItemModel<TValue>>)template;
                    break;
                case nameof(HeaderTemplate):
                    HeaderTemplate = (RenderFragment)template;
                    break;
            }
            StateHasChanged();
        }

        /// <summary>
        /// Get style Attributes value.
        /// </summary>
        /// <returns> returns property styles.</returns>
        protected string GetPropertyStyle()
        {
            string style = string.Empty;
            style = (!string.IsNullOrEmpty(Width)) ? style += UpdateStyle(LISTVIEWWIDTH, Width) : style;
            style = (!string.IsNullOrEmpty(Height)) ? style += UpdateStyle(LISTVIEWHEIGHT, Height) : style;
            if (HtmlAttributes != null && HtmlAttributes.ContainsKey(STYLE))
            {
                style += HtmlAttributes[STYLE];
            }

            return style;
        }

        /// <summary>
        /// Get style Attributes value.
        /// </summary>
        internal static string UpdateStyle(string propertyName, string propertyValue)
        {
            return propertyName + ":" + propertyValue + ((!propertyValue.Contains("px", StringComparison.Ordinal) && !propertyValue.Contains("%", StringComparison.Ordinal)) ? "px;" : ";");
        }

        /// <summary>
        /// Updating the persisting values to our component properties.
        /// </summary>
        internal static string SerializeModel(SfListView<TValue> comp)
        {
            PersistenceValues model = new PersistenceValues
            {
                AllCheckedItems = comp.CheckedItems,
                CssClass = comp.CssClass,
                Enable = comp.Enabled,
                EnableRtl = comp.EnableRtl,
                SortOrder = comp.SortOrder,
            };
            return JsonSerializer.Serialize(model, new JsonSerializerOptions() { IgnoreNullValues = true });
        }

        /// <summary>
        /// Get Public property information.
        /// </summary>
        internal Dictionary<string, object> GetProperties()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add(nameof(Enabled), Enabled);
            properties.Add(nameof(HeaderTitle), HeaderTitle);
            properties.Add(nameof(ShowHeader), ShowHeader);
            properties.Add(nameof(ShowCheckBox), ShowCheckBox);
            properties.Add(nameof(EnableVirtualization), EnableVirtualization);
            properties.Add(nameof(Animation), Animation);
            properties.Add(nameof(EnableRtl), EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl));
            properties.Add(nameof(Template), Template != null);
            properties.Add(nameof(Height), string.IsNullOrEmpty(Height) ? null : Height);
            ElementDetails = GetLiElementData(new ListElementReference() { Key = DATASOURCEKEY }, true);
            properties.Add(SELECTEDDETAILS, ElementDetails.Id != null ? ElementDetails.Id : new List<string>());
            return properties;
        }

        /// <summary>
        /// Updates the virtual index value.
        /// </summary>
        private void UpdateVirtualIndex()
        {
            int listItemDifference = 2;
            int componentHeight = 0;
            if (Height != null)
            {
                if (!Height.Contains("px", StringComparison.Ordinal) && !Height.Contains("%", StringComparison.Ordinal))
                {
                    componentHeight = int.Parse(Height, CultureInfo.CurrentCulture);
                }
                else if (Height.Contains("px", StringComparison.Ordinal))
                {
                    componentHeight = int.Parse(Height.Remove(Height.Length - listItemDifference, listItemDifference), CultureInfo.CurrentCulture);
                }
            }

            virtualElementEndIndex = viewPortElementCount = ((int)Math.Ceiling((decimal)(componentHeight / listElementHeight))) * VIRTUALSCROLLDIFF;
        }

        /// <summary>
        /// Updates listview datasource.
        /// </summary>
        /// <param name="updateSortedData"> specifies the update sorted data.</param>
        /// <param name="dataSource"> specifies the data source.</param>
        protected void UpdateListViewDataSource(bool updateSortedData = false, IEnumerable<TValue> dataSource = null)
        {
            listViewDataSource = new Dictionary<string, IEnumerable<TValue>>();
            IEnumerable<TValue> localData = dataSource;
            if (DataSource != null || dataSource != null)
            {
                if (SortOrder != SortOrder.None && updateSortedData)
                {
                    localData = GetSortedData(dataSource == null ? DataSource : dataSource);
                }
                else if (dataSource == null)
                {
                    localData = DataSource;
                }

                listViewDataSource.Add(DATASOURCEKEY, localData);
            }
        }

        /// <summary>
        /// Updating DataSource After rendering the component.
        /// </summary>
        private async Task UpdateAfterRenderDataSource()
        {
            if (DataSource != null && SortOrder != SortOrder.None)
            {
                UpdateListViewDataSource(true);
                StateHasChanged();
            }

            QueryUpdated = false;
            try
            {
                IEnumerable<TValue> dataSource = null;
                if (DataManager != null)
                {
                    if (DataSource == null && DataManager.Json != null)
                    {
                        dataSource = DataManager.Json.ToList().Cast<TValue>();
                        UpdateListViewDataSource(true, dataSource);
                        StateHasChanged();
                    }
                    else if (Query != null)
                    {
                        object getData = await this.DataManager.ExecuteQuery<TValue>(Query);
                        dataSource = ((IEnumerable<object>)getData).Cast<TValue>().ToList();
                        UpdateListViewDataSource(true, dataSource);
                        StateHasChanged();
                    }
                    else
                    {
                        object data = await DataManager.ExecuteQuery<TValue>(new Query());
                        if ((data as List<object>) != null)
                        {
                            dataSource = (data as List<object>).Cast<TValue>();
                            UpdateListViewDataSource(true, dataSource);
                            StateHasChanged();
                        }
                    }
                }

                if (EnablePersistence)
                {
                    PersistenceValues persistenceValues = await InvokeMethod<PersistenceValues>("window.localStorage.getItem", true, new object[] { ID });
                    if (persistenceValues == null)
                    {
                        await SetLocalStorage(ID, SerializeModel(this));
                    }
                    else
                    {
                        PersistProperties(persistenceValues);
                    }
                }

                await TriggerActionComplete(dataSource);
                ElementDetails = GetLiElementData(new ListElementReference() { Key = DATASOURCEKEY }, true);
                await InvokeMethod("sfBlazor.ListView.setCheckedItems", ElementRef, ElementDetails.Id != null ? ElementDetails.Id : new List<string>());
            }
            catch (Exception e)
            {
                ActionFailureEventsArgs args = new ActionFailureEventsArgs()
                {
                    Error = e,
                    Name = "OnActionFailure"
                };
                if (ListViewEvents != null && ListViewEvents.OnActionFailure.HasDelegate)
                {
                    await SfBaseUtils.InvokeEvent<ActionFailureEventsArgs>(ListViewEvents.OnActionFailure, args);
                    throw new InvalidOperationException("datasource is not valid", e);
                }
            }
        }

        /// <summary>
        /// Updates the virtual index value.
        /// </summary>
        private async Task DyanamicPropertyUpdate()
        {
            if (PropertyChanges.ContainsKey(nameof(Query)))
            {
                QueryUpdated = true;
            }

            if (PropertyChanges.ContainsKey(nameof(HeaderTitle)))
            {
                await InvokeMethod("sfBlazor.ListView.updateHeaderTitle", ElementRef, HeaderTitle);
            }

            if (EnablePersistence && PropertyChanges.Count > 0)
            {
                await SetLocalStorage(ID, SerializeModel(this));
            }

            if (PropertyChanges.ContainsKey(nameof(SortOrder)))
            {
                UpdateListViewDataSource(true);
            }

            if (PropertyChanges.ContainsKey(nameof(DataSource)))
            {
                UpdateListViewDataSource();
                isDSUpdated = true;
            }

            if (PropertyChanges.ContainsKey(nameof(ShowCheckBox)))
            {
                optionsInternal.ShowCheckBox = ShowCheckBox;
            }

            if (PropertyChanges.ContainsKey(nameof(ShowIcon)))
            {
                optionsInternal.ShowIcon = ShowIcon;
            }
        }

        /// <summary>
        /// Defines the properties of persisting component's state between page reloads.
        /// </summary>
        internal async void PersistProperties(PersistenceValues properties)
        {
            try
            {
                if (properties == null)
                {
                    return;
                }
                isPersistance = true;
                ListCssClass = CssClass = properties.CssClass;
                ListEnableRtl = EnableRtl = properties.EnableRtl;
                ListEnabled = Enabled = properties.Enable;
                ListSortOrder = SortOrder = properties.SortOrder;
                CheckedItems = properties.AllCheckedItems;
                if (CheckedItems.Count > 0) { await CheckItemsAsync(CheckedItems); }
                isPersistance = false;
                StateHasChanged();
            }
            catch (Exception exception)
            {
                throw new ArgumentNullException("Invalid Property Update", exception);
            }
        }

        /// <summary>
        /// Update the Persistence value to local storage.
        /// </summary>
        private async Task SetLocalStorage(string persistId, string dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        /// <summary>
        /// Get the sorted data.
        /// </summary>
        private IEnumerable<TValue> GetSortedData(IEnumerable<TValue> data)
        {
            return DataOperations.PerformSorting<TValue>(data, new List<Sort>() { new Sort { Direction = SortOrder.ToString(), Name = ListFields.Text } }).ToList();
        }

        /// <summary>
        /// Returns list element related data based on given elementId and datasource key.
        /// </summary>
        internal ListElementDetails<TValue> GetLiElementData(ListElementReference args, bool ischecked)
        {
            ListElementDetails<TValue> details = new ListElementDetails<TValue>();
            if (listViewDataSource != null && listViewDataSource.ContainsKey(args.Key))
            {
                Type valueType = typeof(TValue);
                List<TValue> dataSource = listViewDataSource[key: args.Key].ToList();
                List<int> index = new List<int>();
                string itemId, isChecked;
                for (int i = 0; i < dataSource.Count; i++)
                {
                    itemId = GetPropertyValue(dataSource[i], ListFields.Id)?.ToString();
                    if (ischecked && !string.IsNullOrEmpty(ListFields.IsChecked))
                    {
                        isChecked = GetPropertyValue(dataSource[i], ListFields.IsChecked).ToString().ToLower(CultureInfo.CurrentCulture);
                        if (isChecked == ISTRUE)
                        {
                            if (details.Id == null)
                            {
                                details.Id = new List<string>();
                            }

                            details.Id.Add(itemId);
                        }
                    }
                    else if (args.ElementId == itemId)
                    {
                        index.Add(i);
                        details = new ListElementDetails<TValue>()
                        {
                            Id = new List<string>() { itemId },
                            ItemData = dataSource[i],
                            ItemText = (string)GetPropertyValue(dataSource[i], ListFields.Text),
                            Index = index
                        };

                        if (ListFields.Child != null)
                        {
                            details.Child = (IEnumerable<TValue>)valueType.GetProperty(ListFields.Child)?.GetValue(dataSource[i], null);
                        }

                        return details;
                    }
                }
            }

            return details;
        }

        internal async override void ComponentDispose()
        {
            if (IsRendered && !IsDestroyedInternal)
            {
                IsDestroyedInternal = true;
                DataManager = null;
                optionsInternal = null;
                ariaAttributes = null;
                listViewDataSource = null;
                Animation = null;
                ListBase = null;
                await InvokeMethod("sfBlazor.ListView.destroy", ElementRef);
                if (ListViewEvents != null && ListViewEvents.Destroyed.HasDelegate)
                {
                    await SfBaseUtils.InvokeEvent<object>(ListViewEvents.Destroyed, null);
                }

                ListViewEvents = null;
            }
        }
    }
}
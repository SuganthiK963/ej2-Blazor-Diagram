using System.Text.Json.Serialization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// Configure member properties of the ListView component.
    /// </summary>
    public partial class SfListView<TValue> : SfBaseComponent
    {
        /// <summary>
        /// The `ID` property is used as a key to identify our element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <exclude/>
        /// <summary>
        /// Update child content from parent component.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The `Animation` property provides an option to apply different
        ///  animations to the ListView component.
        /// </summary>
        [Parameter]
        public AnimationSettings Animation { get; set; } = new AnimationSettings() { Effect = ListViewEffect.SlideLeft, Duration = 400, Easing = "ease" };

        /// <summary>
        /// The `CheckBoxPosition` is used to set the position of the check box in a list item.
        /// By default, the `checkBoxPosition` is Left, which will appear before the text content in a list item.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CheckBoxPosition CheckBoxPosition { get; set; }

        /// <summary>
        /// The `CssClass` property is used to add a user-preferred class name in the root element of the ListView,
        ///  using which you can customize the component (both CSS and functionality customization).
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        private string ListCssClass { get; set; }

        /// <summary>
        /// The `DataSource` provides the data to render the ListView component which is mapped with the fields of ListView.
        /// </summary>
        [Parameter]
        public IEnumerable<TValue> DataSource { get; set; }

        /// <summary>
        /// This dataSource property is used for internal functionalities.
        /// </summary>
        private IEnumerable<TValue> ListDataSource { get; set; }
        
        /// <summary>
        /// The `CheckedItems` property is used to set the item that need to be checked or
        /// get the details of items that are currently checked in the ListView component.
        /// The `CheckedItems` property depends upon the value of `showCheckBox` property.
        /// </summary>
        internal List<TValue> CheckedItems { get; set; } = new List<TValue>();

        /// <summary>
        /// If `Enabled` is set to true, the list items will be enabled. 
        /// You can disable the component using this property by setting its value as false.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        private bool ListEnabled { get; set; }

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        private bool ListEnableRtl { get; set; }

        /// <summary>
        /// If `EnableVirtualization` is set to true, the ListView’s performance will improve when loading a large amount of data.
        /// </summary>
        [Parameter]
        public bool EnableVirtualization { get; set; }

        /// <summary>
        /// The `Fields` is used to map keys from the dataSource which extracts the appropriate data from the dataSource
        ///  with specified mapped with the column fields to render the ListView.
        /// </summary>

        internal ListViewFieldSettings<TValue> ListFields { get; set; }

        /// <summary>
        /// The `HeaderTitle` is used to set the title of the ListView component.
        /// </summary>
        [Parameter]
        public string HeaderTitle { get; set; } = string.Empty;

        private string ListHeaderTitle { get; set; }

        /// <summary>
        /// Defines the height of the ListView component.
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        /// <summary>
        /// The `HtmlAttributes` allows additional attributes such as id, class, etc., and
        ///  accepts n number of attributes in a key-value pair format.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// The `Query` is used to fetch specific data from the data source by using the where and select keywords.
        /// </summary>
        [Parameter]
        public Data.Query Query { get; set; }

        private Data.Query ListQuery{ get; set; }

        /// <summary>
        /// If `ShowCheckBox` is set to true, the checkbox will  be shown or hidden.
        /// </summary>
        [Parameter]
        public bool ShowCheckBox { get; set; }
        private bool ListShowCheckBox { get; set; }

        /// <summary>
        /// If `ShowHeader` is set to true, the header of the ListView component will  be shown or hidden.
        /// </summary>
        [Parameter]
        public bool ShowHeader { get; set; }

        /// <summary>
        /// If `ShowIcon` is set to true, the icon of the list item will  be shown or hidden.
        /// </summary>
        [Parameter]
        public bool ShowIcon { get; set; }
        private bool ShowListIcon { get; set; }

        /// <summary>
        /// The `SortOrder` is used to sort the data source. The available type of sort orders are,
        ///  `None` - The data source will not be sorted.
        ///  `Ascending` - The data source will be sorted in ascending order.
        ///  `Descending` - The data source will be sorted in descending order.
        /// </summary>
        [Parameter]
        public SortOrder SortOrder { get; set; }

        private SortOrder ListSortOrder { get; set; }

        /// <summary>
        /// Defines the width of the ListView component.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// The ListView has an option to custom design the group header title with the help of the groupTemplate property.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment<ComposedItemModel<TValue>> GroupTemplate { get; set; }

        /// <summary>
        /// The ListView has an option to custom design the ListView header title with the help of the headerTemplate property.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// The ListView supports customizing the content of each list item with the help of template property.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment<TValue> Template { get; set; }
    }
}
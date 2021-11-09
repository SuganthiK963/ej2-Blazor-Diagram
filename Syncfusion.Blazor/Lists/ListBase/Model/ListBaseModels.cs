using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Lists.Internal;

namespace Syncfusion.Blazor.Lists.Internal
{
    /// <summary>
    /// An enum type that denotes the expand and collapse icon positions. Available options are as follows Right, Left.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum Position
    {
        /// <summary>
        /// Positions the expand collapse icon to the right end of the item.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,

        /// <summary>
        /// Positions the expand collapse icon to the left end of the item.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left
    }

    /// <exclude/>
    /// <summary>
    /// Class for data of each li element.
    /// </summary>
    public class ListElementReference
    {
        /// <summary>
        /// It is used to denote the element Id.
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// It is used to denote the datasource key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// It is used to check whether the element is checked or not.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// It is used to check whether the element is interacted or not.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// It denotes the level of the nested list items.
        /// </summary>
        public int Level { get; set; }
    }

    /// <summary>
    /// An class that holds item class list.
    /// </summary>
    public class ClassList
    {
        /// <summary>
        /// Specifies the check.
        /// </summary>
        public string Check { get; set; }

        /// <summary>
        /// It is used to check the element.
        /// </summary>
        public string Checked { get; set; }

        /// <summary>
        /// It is used to disable the element.
        /// </summary>
        public string Disabled { get; set; }

        /// <summary>
        /// It is used to group the elements.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// It is used to check whether the li element has class child or not.
        /// </summary>
        public string HasChild { get; set; }

        /// <summary>
        /// It is used to specify the icons for list item.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// It is used to specify the icon wrapper class.
        /// </summary>
        public string IconWrapper { get; set; }

        /// <summary>
        /// It is used to specify the level of the list item.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// It is used to specify the list item.
        /// </summary>
        public string Li { get; set; }

        /// <summary>
        /// It is used to specify the text for list item.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// It is used to specify the text content for list item.
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// It is used to specify UI element.
        /// </summary>
        public string Ul { get; set; }
    }

    /// <summary>
    /// An class that holds the field values Mapping.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class FieldsValueMapping<T>
    {
        /// <summary>
        /// Specifies the Child field.
        /// </summary>
        public T Child { get; set; }

        /// <summary>
        /// Specifies the GroupBy field.
        /// </summary>
        public string GroupBy { get; set; }

        /// <summary>
        /// Specifies the IconCss field.
        /// </summary>
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies the Id field.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Specifies the Text field.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the Value field.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Specifies the Tooltip field.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Specifies the Enabled field.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Specifies the IsChecked field.
        /// </summary>
        public bool? IsChecked { get; set; }

        /// <summary>
        /// Specifies the IsVisible field.
        /// </summary>
        public bool? IsVisible { get; set; }

        /// <summary>
        /// Specifies the HtmlAttributes field.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }

    /// <summary>
    /// An class that holds list Item properties related arguments.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class ListItemBase<T> : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets ListBase component.
        /// </summary>
        [CascadingParameter(Name = "ListBase")]
        protected ListBaseFoundation<T> ListBase { get; set; }

        /// <summary>
        /// Gets or sets MappedData.
        /// </summary>
        [Parameter]
        public FieldsValueMapping<List<T>> MappedData { get; set; }

        /// <summary>
        /// Gets or sets IsGroupItem field.
        /// </summary>
        [Parameter]
        public bool IsGroupItem { get; set; }

        /// <summary>
        /// Gets or sets Data field.
        /// </summary>
        [Parameter]
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets GroupItemData field.
        /// </summary>
        [Parameter]
        public ComposedItemModel<T> GroupItemData { get; set; }

        /// <summary>
        /// Gets or sets the item template field.
        /// </summary>
        [Parameter]
        public bool IsItemTemplate { get; set; }

        /// <summary>
        /// Gets or sets the group item template field.
        /// </summary>
        [Parameter]
        public bool IsGroupTemplate { get; set; }

        /// <summary>
        /// Gets or sets the random id field.
        /// </summary>
        [Parameter]
        public string RandomID { get; set; }

        /// <summary>
        /// Gets or sets the index field.
        /// </summary>
        [Parameter]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the class names field.
        /// </summary>
        [Parameter]
        public ClassList ClassNames { get; set; }

        /// <summary>
        /// Gets or sets the ListBaseOptionModel field.
        /// </summary>
        [Parameter]
        public ListBaseOptionModel<T> ListBaseOptionModel { get; set; }
    }

    /// <summary>
    /// This class holds listbase properties related arguments.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class ListCommonBase<T> : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets ListParent component.
        /// </summary>
        [CascadingParameter(Name = "ListParent")]
        public object ListParent { get; set; }

        /// <summary>
        /// Gets or sets DataSource field.
        /// </summary>
        [Parameter]
        public IEnumerable<T> DataSource { get; set; }

        /// <summary>
        /// Gets or sets ListBaseOptionModel field.
        /// </summary>
        [Parameter]
        public ListBaseOptionModel<T> ListBaseOptionModel { get; set; }

        /// <summary>
        /// Gets or sets ListBaseOptionModel field.
        /// </summary>
        [Parameter]
        public int Level { get; set; }

        /// <summary>
        /// Specifies the callback function that triggered before each list creation.
        /// </summary>
        [Parameter]
        public Action<ItemCreatedArgs<T>> ItemCreating { get; set; }

        /// <summary>
        /// Specifies the callback function that triggered after each list creation.
        /// </summary>
        [Parameter]
        public Action<ItemCreatedArgs<T>> ItemCreated { get; set; }

        /// <summary>
        /// Gets or sets item template property name.
        /// </summary>
        [Parameter]
        public string ItemTemplatePropertyName { get; set; }

        /// <summary>
        /// Gets or sets group template property name.
        /// </summary>
        [Parameter]
        public string GroupTemplatePropertyName { get; set; }

        /// <summary>
        /// Gets or sets starting position.
        /// </summary>
        [Parameter]
        public int StartingPosition { get; set; }

        /// <summary>
        /// Gets or sets list element height.
        /// </summary>
        [Parameter]
        public double LiElementHeight { get; set; }

        /// <summary>
        /// Gets or sets end position.
        /// </summary>
        [Parameter]
        public int EndPosition { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the virtual scrolling.
        /// </summary>
        [Parameter]
        public bool VirtualScrolling { get; set; }

        /// <summary>
        /// Gets or sets the checkbox position.
        /// </summary>
        [Parameter]
        public CheckBoxPosition CheckBoxPosition { get; set; } = CheckBoxPosition.Left;

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        public RenderFragment<T> Template { get; set; }

        /// <summary>
        /// Gets or sets the group template.
        /// </summary>
        public RenderFragment<ComposedItemModel<T>> GroupTemplate { get; set; }
    }

    /// <summary>
    /// An class that holds item created event arguments.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class ItemCreatedArgs<T>
    {
        /// <summary>
        /// Specifies the current data arguement.
        /// </summary>
        public T CurData { get; set; }

        /// <summary>
        /// Specifies the datasource arguement.
        /// </summary>

        internal IEnumerable<T> ListsDataSource { get; set; }

        /// <summary>
        /// Specifies the fields arguement.
        /// </summary>

        internal ListBaseFields<T> ListsFields { get; set; }

        /// <summary>
        /// Specifies the item arguement.
        /// </summary>

        internal DOM ListsItem { get; set; }

        /// <summary>
        /// Specifies the options arguement.
        /// </summary>

        internal ListBaseOptionModel<T> ListsOptions { get; set; }

        /// <summary>
        /// Specifies the text arguement.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the name arguement.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Basic ListBase Options.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class ListBaseOptionModel<T>
    {
        /// <summary>
        /// Specifies to data is singel level or not.
        /// </summary>
        public bool IsSingleLevel { get; set; }

        /// <summary>
        /// Specifies the aria attributes.
        /// </summary>
        public AriaAttributesMapping AriaAttributes { get; set; }

        /// <summary>
        /// Specifies to show collapsible icon.
        /// </summary>
        public bool ExpandCollapse { get; set; }

        /// <summary>
        /// Specifies the customizable expand icon class.
        /// </summary>
        public string ExpandIconClass { get; set; } = "e-icon-collapsible";

        /// <summary>
        /// Specifies the expand/collapse icon position.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        internal Position ExpandIconPosition { get; set; }

        /// <summary>
        /// Specifies that fields that mapped in dataSource.
        /// </summary>
        public ListBaseFields<T> Fields { get; set; }

        /// <summary>
        /// Specifies the group header template.
        /// </summary>
        public string GroupTemplate { get; set; }

        /// <summary>
        /// Specifies the ListView header template.
        /// </summary>
        public string HeaderTemplate { get; set; }

        /// <summary>
        /// Specifies when need to add additional LI item class.
        /// </summary>
        public string ItemClass { get; set; }

        /// <summary>
        /// Specifies the callback function that triggered after each list creation.
        /// </summary>
        public ItemCreatedArgs<T> ItemCreated { get; set; }

        /// <summary>
        /// Specifies the callback function that triggered before each list creation.
        /// </summary>
        public ItemCreatedArgs<T> ItemCreating { get; set; }

        /// <summary>
        /// Specifies the customized class name based on their module name.
        /// </summary>
        public string ModuleName { get; set; } = "list";

        /// <summary>
        /// Specifies to show checkBox.
        /// </summary>
        public bool ShowCheckBox { get; set; }

        /// <summary>
        /// Specifies to show icon.
        /// </summary>
        public bool ShowIcon { get; set; }

        /// <summary>
        /// Specifies the sort order.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortOrder SortOrder { get; set; }

        /// <summary>
        /// Specifies the item template.
        /// </summary>
        public string Template { get; set; }
    }
}
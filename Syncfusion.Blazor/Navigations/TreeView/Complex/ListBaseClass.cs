using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;

namespace Syncfusion.Blazor.Navigations.Internal
{
    /// <exclude/>
    /// <summary>
    /// Specifies the TreeViewListItems class.
    /// </summary>
    /// <typeparam name="TValue">"TypeParam".</typeparam>
    public class TreeViewListItems<TValue> : SfBaseComponent
    {
        internal TreeItemCreatedArgs<TValue> TreeItemCreatingArgs { get; set; }

        internal int TreeNodeLevel { get; set; } = 1;

        internal FieldsValueMapping<List<TValue>> MappedData { get; set; }

        internal string ExpandIconClass { get; set; } = "e-icon-expandable";

        /// <summary>
        /// TreeItem creating.
        /// </summary>
        protected Action<TreeItemCreatedArgs<TValue>> TreeItemCreating { get; set; }

        /// <summary>
        /// ItemCreating variable.
        /// </summary>
        protected Action<TreeItemCreatedArgs<TValue>> ItemsCreating { get; set; }

        [CascadingParameter]
        internal SfTreeView<TValue> TreeParent { get; set; }
    }

    /// <summary>
    /// Specifies the TreeOption class.
    /// </summary>
    /// <typeparam name="TValue">"TValue".</typeparam>
    public class TreeOptions<TValue>
    {
        /// <summary>
        /// Specifies the Child data of node.
        /// </summary>
        public IEnumerable<TValue> ChildData { get; set; }

        /// <summary>
        /// Specifies the Treeview fields value.
        /// </summary>
        public FieldsMapping TreeViewFields { get; set; }

        /// <summary>
        /// Specifies the Treeview node is expanded or not.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Specifies the fullrow selection enabled or not.
        /// </summary>
        public bool IsFullRowSelect { get; set; }

        /// <summary>
        /// Specifies the IconClass of Treeview.
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// Specifies the node is selected or not.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Specifies the Treeview node checked or not.
        /// </summary>
        public string IsChecked { get; set; }

        /// <summary>
        /// Specifies the Treeview node level.
        /// </summary>
        public double NodeLevel { get; set; } = 1;

        /// <summary>
        /// Specifies the tree.
        /// </summary>
        public bool IsTree { get; set; } = true;

        /// <summary>
        /// Specifies the Treeview node template.
        /// </summary>
        public RenderFragment<TValue> NodeTemplate { get; set; }

        /// <summary>
        /// Specifies the full row navigate of Treeview node.
        /// </summary>
        public bool FullRowNavigate { get; set; }

        /// <summary>
        /// Specifies the editing mode in node.
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        /// Specifies the node disabled or not.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Specifies the node already loaded or not in DOM
        /// </summary>
        public bool IsLoaded { get; set; }
    }

    /// <summary>
    /// Specifies the TreeItemCreatedArgs.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    public class TreeItemCreatedArgs<T>
    {
        /// <summary>
        /// Specifies the TreeOptions.
        /// </summary>
        public TreeOptions<T> TreeOptions { get; set; }

        /// <summary>
        /// Specifies the NodeLevel argument.
        /// </summary>
        public int NodeLevel { get; set; }

        /// <summary>
        /// Specifies the ItemData argument.
        /// </summary>
        public T ItemData { get; set; }

        /// <summary>
        /// Specifies the Item.
        /// </summary>
        public DOM Item { get; set; }

        /// <summary>
        /// Specifies the Options.
        /// </summary>
        public ListModel Options { get; set; }

        /// <summary>
        /// Specifies the Text.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Specifies the Treeview field mappings.
    /// </summary>
    public class FieldsMapping
    {
        /// <summary>
        /// Specifies the child attribute.
        /// </summary>
        public string Child { get; set; } = "Child";

        /// <summary>
        /// Specifies the Expanded field attribute.
        /// </summary>
        public string Expanded { get; set; } = "Expanded";

        /// <summary>
        /// Specifies the HasChildren field attribute.
        /// </summary>
        public string HasChildren { get; set; } = "HasChildren";

        /// <summary>
        /// Specifies the HtmlAttributes.
        /// </summary>
        public string HtmlAttributes { get; set; } = "HtmlAttributes";

        /// <summary>
        /// Specifies the IconCss field attribute.
        /// </summary>
        public string IconCss { get; set; } = "IconCss";

        /// <summary>
        /// Specifies the Id field attribute.
        /// </summary>
        public string Id { get; set; } = "Id";

        /// <summary>
        /// Specifies the ImageUrl field attribute.
        /// </summary>
        public string ImageUrl { get; set; } = "ImageUrl";

        /// <summary>
        /// Specifies the IsChecked field attribute.
        /// </summary>
        public string IsChecked { get; set; } = "IsChecked";

        /// <summary>
        /// Specifies the Selected field attribute.
        /// </summary>
        public string Selected { get; set; } = "Selected";

        /// <summary>
        /// Specifies the Text field attribute.
        /// </summary>
        public string Text { get; set; } = "Text";

        /// <summary>
        /// Specifies the Tooltip field attribute.
        /// </summary>
        public string Tooltip { get; set; } = "Tooltip";

        /// <summary>
        /// Specifies the Url field attribute.
        /// </summary>
        public string Url { get; set; } = "Url";
    }

    /// <summary>
    /// Specifies the field values Mapping.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    /// <exclude/>
    public class FieldsValueMapping<T>
    {
        /// <summary>
        /// Specifies the child field.
        /// </summary>
        public T Child { get; set; }

        /// <summary>
        /// Specifies the Expanded field.
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Specifies the HasChildren field.
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Specifies the IconCss field.
        /// </summary>
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies the Id field.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Specifies the child field.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Specifies the IsChecked field.
        /// </summary>
        public bool? IsChecked { get; set; }

        /// <summary>
        /// Specifies the Selected field.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Specifies the Text field.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the child field.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Specifies the URL field.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Specifies the HtmlAttriibutes field.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }

    /// <summary>
    /// Specifies the item created event arguments.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    public class ItemCreatedArgs<T>
    {
        /// <summary>
        /// Specifies the ItemData.
        /// </summary>
        public T ItemData { get; set; }

        /// <summary>
        /// Specifies the Item value.
        /// </summary>
        public DOM Item { get; set; }

        /// <summary>
        /// Specifies the Text of the item.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Specifies the TreeFieldsMapping.
    /// </summary>
    public class TreeFieldsMapping : FieldsMapping
    {
        /// <summary>
        /// Specifies the ParentId.
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// Specifies the item DataManager.
        /// </summary>
        public DataManager DataManager { get; set; }

        /// <summary>
        /// Specifies the Query.
        /// </summary>
        public Query Query { get; set; }

        /// <summary>
        /// Specifies the TableName.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Specifies the Children.
        /// </summary>
        public object Children { get; set; }
    }

    /// <summary>
    /// Specifies Treeview Options.
    /// </summary>
    public class ListModel
    {
        /// <summary>
        /// Specifies to show collapsible icon.
        /// </summary>
        public bool ExpandCollapse { get; set; } = true;

        /// <summary>
        /// Specifies the customizable expand icon class.
        /// </summary>
        public string ExpandIconClass { get; set; } = "e-icon-collapsible";

        /// <summary>
        /// Specifies that fields that mapped in dataSource.
        /// </summary>
        public FieldsMapping Fields { get; set; } = new FieldsMapping();

        /// <summary>
        /// If set true to this property then the entire list will be navigate-able instead of text element.
        /// </summary>
        public bool ItemNavigable { get; set; }

        /// <summary>
        /// Specifies to show icon.
        /// </summary>
        public bool ShowIcon { get; set; } = true;
    }

    /// <summary>
    /// Specifies class for expand / collapse animation settings.
    /// </summary>
    public class AnimationSettingsModel : SfBaseComponent
    {
        /// <summary>
        /// Specifies the time duration to transform content.
        /// </summary>
        [Parameter]
        public int Duration { get; set; } = 400;

        /// <summary>
        /// Specifies the easing effect applied when transforming the content.
        /// </summary>
        [Parameter]
        public string Easing { get; set; } = "linear";
    }

    /// <summary>
    /// Specifies the class for expand animation settings.
    /// </summary>
    public class AnimationExpandModel : AnimationSettingsModel
    {
        /// <summary>
        /// Specifies the animation effect for expanding the TreeView node.
        /// Default animation is given as SlideDown. You can also disable the animation by setting the animation effect as None.
        /// </summary>
        [Parameter]
        public AnimationEffect Effect { get; set; } = AnimationEffect.SlideDown;
    }

    /// <summary>
    /// Specifies the class for collapse animation settings.
    /// </summary>
    public class AnimationCollapseModel : AnimationSettingsModel
    {
        /// <summary>
        /// Specifies the animation effect for collapsing the TreeView node.
        /// Default animation is given as SlideUp. You can also disable the animation by setting the animation effect as None.
        /// </summary>
        [Parameter]
        public AnimationEffect Effect { get; set; } = AnimationEffect.SlideUp;
    }
}
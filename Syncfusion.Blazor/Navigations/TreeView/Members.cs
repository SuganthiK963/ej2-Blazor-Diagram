using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the TreeView component.
    /// </summary>
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
    public partial class SfTreeView<TValue> : SfBaseComponent, ITreeView
    {
        /// <exclude/>
        /// <summary>
        /// Get the RenderFragment content.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Indicates whether the TreeView allows drag and drop of nodes. To drag and drop a node in
        /// desktop, hold the mouse on the node, drag it to the target node and drop the node by releasing
        /// the mouse. For touch devices, drag and drop operation is performed by touch, touch move
        /// and touch end.
        /// </summary>
        [Parameter]
        public bool AllowDragAndDrop { get; set; }

        /// <summary>
        /// Enables or disables editing of the text in the TreeView node. When `AllowEditing` property is set
        /// to true, the TreeView allows you to edit the node by double clicking the node or by navigating to
        /// the node and pressing F2 key.
        /// </summary>
        [Parameter]
        public bool AllowEditing { get; set; }

        /// <summary>
        /// Enables or disables multi-selection of nodes. To select multiple nodes:
        ///  Select the nodes by holding down the CTRL key while clicking on the nodes.
        ///  Select consecutive nodes by clicking the first node to select and hold down the SHIFT key
        /// and click the last node to select.
        /// </summary>
        [Parameter]
        public bool AllowMultiSelection { get; set; }

        /// <summary>
        /// Enables or disables to wrap the text in the TreeView node. 
        /// When the `AllowTextWrap` property is set to true, the TreeView node text content will wrap to the next line
        /// when its text content exceeds the width of the TreeView node.
        /// </summary>
        [Parameter]
        public bool AllowTextWrap { get; set; }
     
        /// <summary>
        /// Specifies the type of animation applied on expanding and collapsing the nodes along with duration.
        /// </summary>
        [Parameter]
        [Obsolete("This property is deprecated.Use <TreeViewNodeAnimationSettings> tag to set animation options.")]
        public TreeViewNodeAnimationSettings Animation { get { return TreeViewAnimation; } set { TreeViewAnimation = value; } }
        internal TreeViewNodeAnimationSettings TreeViewAnimation { get; set; }


        /// <summary>
        /// Allow us to specify the parent and child nodes to get auto check while we check or uncheck a node.
        /// </summary>
        [Parameter]
        public bool AutoCheck { get; set; } = true;

        /// <summary>
        /// The `CheckedNodes` property is used to set the nodes that need to be checked or
        /// get the ID of nodes that are currently checked in the TreeView component.
        /// The `checkedNodes` property depends upon the value of `showCheckBox` property.
        /// </summary>
        [Parameter]
        public string[] CheckedNodes { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the checked nodes changes.
        /// </summary>
        [Parameter]
        public EventCallback<string[]> CheckedNodesChanged { get; set; }

        /// <summary>
        /// Specifies the CSS classes to be added with root element of the TreeView to help customize the appearance of the component.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a value that indicates whether the TreeView component is disabled or not.
        /// When set to true, user interaction will not be occurred in TreeView.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Defines the area in which the draggable element movement will be occurring. Outside that area will be restricted
        /// for the draggable element movement. By default, the draggable element movement occurs in the entire page.
        /// </summary>
        [Parameter]
        public string DropArea { get; set; }

        /// <summary>
        /// Defines whether to allow the cross-scripting site or not.
        /// </summary>
        [Parameter]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public bool EnableHtmlSanitizer { get; set; }

        /// <summary>
        /// Enables or disables persisting TreeView state between page reloads. If enabled, following APIs will persist.
        /// 1. `SelectedNodes` - Represents the nodes that are selected in the TreeView component.
        /// 2. `checkedNodes`  - Represents the nodes that are checked in the TreeView component.
        /// 3. `expandedNodes` - Represents the nodes that are expanded in the TreeView component.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the action on which the node expands or collapses. The available actions are,
        ///  `Click` - The expand/collapse operation happens when you single-click the node in both desktop and mobile devices.
        ///  `DblClick` - The expand/collapse operation happens when you double-click the node in both desktop and mobile devices.
        ///  `None` - The expand/collapse operation will not happen when you single-click or double-click the node in both desktop
        ///  and mobile devices.
        /// </summary>
        [Parameter]
        public ExpandAction ExpandOn { get; set; } = ExpandAction.DoubleClick;

        /// <summary>
        /// Represents the expanded nodes in the TreeView component. We can set the nodes that need to be
        /// expanded or get the ID of the nodes that are currently expanded by using this property.
        /// </summary>
        [Parameter]
        public string[] ExpandedNodes { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the expanded nodes changes.
        /// </summary>
        [Parameter]
        public EventCallback<string[]> ExpandedNodesChanged { get; set; }

        /// <summary>
        /// Specifies the data source and mapping fields to render TreeView nodes.
        /// </summary>
        [Parameter]
        [Obsolete("This property is deprecated.Use <TreeViewFieldsSettings> tag to set mapping fields to render treeview nodes.")]
        public TreeViewFieldsSettings<TValue> Fields { get { return TreeViewFields; } set { TreeViewFields = value; } }
        internal TreeViewFieldsSettings<TValue> TreeViewFields { get; set; }

        /// <summary>
        /// If this property is set to true, then the entire TreeView node will be navigate-able instead of text element.
        /// </summary>
        [Parameter]
        public bool FullRowNavigable { get; set; }

        /// <summary>
        /// On enabling this property, the entire row of the TreeView node gets selected by clicking a node.
        /// When disabled only the corresponding node's text gets selected.
        /// </summary>
        [Parameter]
        public bool FullRowSelect { get; set; } = true;

        /// <summary>
        /// By default, the load on demand (Lazy load) is set to true. By disabling this property, all the tree nodes are rendered at the
        /// beginning itself.
        /// </summary>
        [Parameter]
        public bool LoadOnDemand { get; set; } = true;

        /// <summary>
        /// Represents the selected nodes in the TreeView component. We can set the nodes that need to be
        /// selected or get the ID of the nodes that are currently selected by using this property.
        /// On enabling `AllowMultiSelection` property we can select multiple nodes and on disabling
        /// it we can select only a single node.
        /// </summary>
        [Parameter]
        public string[] SelectedNodes { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the selected nodes changes.
        /// </summary>
        [Parameter]
        public EventCallback<string[]> SelectedNodesChanged { get; set; }

        /// <summary>
        /// Indicates that the nodes will display CheckBoxes in the TreeView.
        /// The CheckBox will be displayed next to the expand/collapse icon of the node.
        /// </summary>
        [Parameter]
        public bool ShowCheckBox { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the nodes are sorted in the ascending or descending order,
        /// or are not sorted at all. The available types of sort order are,
        ///  `None` - The nodes are not sorted.
        ///  `Ascending` - The nodes are sorted in the ascending order.
        ///  `Descending` - The nodes are sorted in the ascending order.
        /// </summary>
        [Parameter]
        public SortOrder SortOrder { get; set; }

        /// <summary>
        /// Specify the HtmlAttributes for TreeView.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [Obsolete("This property is deprecated.Use @attributes to set additional attributes for treeview element.")]
        public Dictionary<string, object> HtmlAttributes { get { return SfHtmlAttributes; } set { SfHtmlAttributes = value; } }

        private Dictionary<string, object> SfHtmlAttributes { get; set; }

        /// <summary>
        /// Specifies the custom item template of TreeView Node item.
        /// </summary>
        [Parameter]
        [Obsolete("This property is deprecated.Use <TreeViewTemplates> tag to define render fragment content.")]
        public TreeViewTemplates<TValue> TreeViewTemplates { get { return TreeViewTemplate; } set { TreeViewTemplate = value; } }
        internal TreeViewTemplates<TValue> TreeViewTemplate { get; set; }

        /// <summary>
        /// Sets id attribute for the treeview element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        internal TreeViewNodeAnimationSettings AnimationSettings { get; set; } = new TreeViewNodeAnimationSettings() { Expand = new TreeViewAnimationExpand() { Effect = AnimationEffect.SlideDown, Duration = 400, Easing = "linear" }, Collapse = new TreeViewAnimationCollapse() { Effect = AnimationEffect.SlideUp, Duration = 400, Easing = "linear" } };
#pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        /// Specifies the Animation properties.
        /// </summary>
        /// <param name="animationSettings">"Specifies the animation settings".</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateAnimationProperties(TreeViewNodeAnimationSettings animationSettings)
        {
            var treeAnimation = animationSettings;
            if (treeAnimation == null)
            {
                treeAnimation = new TreeViewNodeAnimationSettings();
                treeAnimation.UpdateExpandProperties(treeAnimation.NodeAnimationExpand);
                treeAnimation.UpdateCollapseProperties(treeAnimation.NodeAnimationCollapse);
            }
            TreeViewAnimation = treeAnimation;
            AnimationSettings = treeAnimation;
        }
    }
}

#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
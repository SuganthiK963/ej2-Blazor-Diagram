using System;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class used for configuring the TreeView animation properties.
    /// </summary>
    public partial class TreeViewNodeAnimationSettings : SfBaseComponent
    {
        [CascadingParameter]
        private ITreeView TreeViewNodeAnimationdynamicParent { get; set; }

        /// <exclude/>
        /// <summary>
        /// Child Content for the Treeview Animation Settings.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the animation to appear when collapsing the TreeView item.
        /// </summary>
        [Parameter]
        [Obsolete("This property is deprecated.Use <TreeViewAnimationCollapse> tag to set animation options.")]
        public TreeViewAnimationCollapse Collapse { get { return NodeAnimationCollapse; } set { NodeAnimationCollapse = value; } }
        internal TreeViewAnimationCollapse NodeAnimationCollapse { get; set; }

        /// <summary>
        /// Specifies the animation to appear when expanding the TreeView item.
        /// </summary>
        [Parameter]
        [Obsolete("This property is deprecated.Use <TreeViewAnimationExpand> tag to set animation options.")]
        public TreeViewAnimationExpand Expand { get { return NodeAnimationExpand; } set { NodeAnimationExpand = value; } }
        internal TreeViewAnimationExpand NodeAnimationExpand { get; set; }

        internal void UpdateExpandProperties(TreeViewAnimationExpand animation)
        {
            NodeAnimationExpand = animation == null ? new TreeViewAnimationExpand() : animation;
        }

        internal void UpdateCollapseProperties(TreeViewAnimationCollapse animation)
        {
            NodeAnimationCollapse = animation == null ? new TreeViewAnimationCollapse() : animation;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            TreeViewNodeAnimationdynamicParent?.UpdateAnimationProperties(this);
            UpdateExpandProperties(NodeAnimationExpand);
            UpdateCollapseProperties(NodeAnimationCollapse);
        }

        internal override void ComponentDispose()
        {
            TreeViewNodeAnimationdynamicParent = null;
            ChildContent = null;
        }
    }
}
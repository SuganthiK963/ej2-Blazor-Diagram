using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the class for arranging the nodes and connectors in a tree structure.
    /// </summary>
#pragma warning disable CA1724 // Type names should not match namespaces
    public partial class Layout : SfBaseComponent
#pragma warning restore CA1724 // Type names should not match namespaces
    {
        private string fixedNode;
        private int verticalSpacing = 30;
        private DiagramRect bounds;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Auto;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Auto;

        private LayoutOrientation orientation = LayoutOrientation.TopToBottom;
        private int horizontalSpacing = 30;
        [CascadingParameter]
        [JsonIgnore]
        internal SfDiagramComponent Parent { get; set; }
        /// <summary>
        /// Sets the name of the node concerning which all the other nodes will be translated.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public string FixedNode { get; set; }
        /// <summary>
        /// Gets or sets the child content of the layout. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the fixed node changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<string> FixedNodeChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the vertical spacing changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<int> VerticalSpacingChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the type changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<LayoutType> TypeChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the horizontal alignment changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<HorizontalAlignment> HorizontalAlignmentChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the vertical alignment changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<VerticalAlignment> VerticalAlignmentChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the horizontal spacing changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<int> HorizontalSpacingChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the bounds changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<DiagramRect> BoundsChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the orientation changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<LayoutOrientation> OrientationChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the margin changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<bool> MarginChanged { get; set; }

        /// <summary>
        /// GetLayoutInfo is used to configure every subtree of the organizational chart.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public Func<IDiagramObject, TreeInfo, TreeInfo> GetLayoutInfo { get; set; }
        /// <summary>
        /// Returns the branch type of the layout. Applicable only if it is a mindmap layout. 
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public Func<IDiagramObject, BranchType> GetBranch { get; set; }

        /// <summary>
        /// Specifies the space that must be Vertically left between the nodes. It is 30, by default.
        /// </summary>
        [Parameter]
        [JsonPropertyName("verticalSpacing")]
        public int VerticalSpacing { get; set; } = 30;

        /// <summary>
        /// Aligns the layout within the given bounds.
        /// </summary>
        [Parameter]
        [JsonPropertyName("bounds")]
        public DiagramRect Bounds { get; set; }

        /// <summary>
        /// Specifies the space that must be horizontally left between the nodes. It is 30, by default.
        /// </summary>
        [Parameter]
        [JsonPropertyName("horizontalSpacing")]
        public int HorizontalSpacing { get; set; } = 30;
        /// <summary>
        /// Specifies the parent node of the layout.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public string Root { get; set; }

        /// <summary>
        /// Specifies the orientation of the layout.
        /// </summary>
        [Parameter]
        [JsonPropertyName("orientation")]
        public LayoutOrientation Orientation { get; set; } = LayoutOrientation.TopToBottom;


        /// <summary>
        /// Describes how the layout should be positioned or stretched in the HorizontalAlignment..
        /// </summary>
        [Parameter]
        [JsonPropertyName("horizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Auto;
        /// <summary>
        /// Describes how the layout should be positioned or stretched in the VerticalAlignment.
        /// </summary>
        [Parameter]
        [JsonPropertyName("verticalAlignment")]
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Auto;

        /// <summary>
        /// Defines the type of layout.
        /// </summary>
        [Parameter]
        [JsonPropertyName("type")]
        public LayoutType Type { get; set; } = LayoutType.None;

        /// <summary>
        /// Specifies the space between the viewport and the layout. By default {left:50, top:50, right:0, bottom:0}.
        /// </summary>
        [Parameter]
        [JsonPropertyName("margin")]
        public LayoutMargin Margin { get; set; } = new LayoutMargin();
        [JsonIgnore]
        internal TreeInfo LayoutInfo { get; set; } = new TreeInfo();

        internal static Layout Initialize()
        {
            Layout layout = new Layout();
            layout.LayoutInfo ??= new TreeInfo();
            layout.Margin ??= new LayoutMargin();
            return layout;
        }

        internal void UpdateMarginValues(LayoutMargin margin)
        {
            Margin = margin ?? new LayoutMargin();
        }

        #region Life Cycle methods
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            verticalSpacing = VerticalSpacing;
            horizontalSpacing = HorizontalSpacing;
            horizontalAlignment = HorizontalAlignment;
            verticalAlignment = VerticalAlignment;
            orientation = Orientation;
            UpdateMarginValues(this.Margin);
            Parent.UpdateLayout(this);
        }
        /// <summary>
        /// Method invoked when any changes in component state occur.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            verticalSpacing = BaseUtil.UpdateDictionary(nameof(VerticalSpacing), verticalSpacing, VerticalSpacing, PropertyChanges);
            horizontalSpacing = BaseUtil.UpdateDictionary(nameof(HorizontalSpacing), horizontalSpacing, HorizontalSpacing, PropertyChanges);
            horizontalAlignment = BaseUtil.UpdateDictionary(nameof(HorizontalAlignment), horizontalAlignment, HorizontalAlignment, PropertyChanges);
            verticalAlignment = BaseUtil.UpdateDictionary(nameof(VerticalAlignment), verticalAlignment, VerticalAlignment, PropertyChanges);
            orientation = BaseUtil.UpdateDictionary(nameof(Orientation), orientation, Orientation, PropertyChanges);
            fixedNode = BaseUtil.UpdateDictionary(nameof(FixedNode), fixedNode, FixedNode, PropertyChanges);
            bounds = BaseUtil.UpdateDictionary(nameof(Bounds), bounds, Bounds, PropertyChanges);
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (PropertyChanges.Any() && !firstRender)
            {
                await this.Parent.DoLayout();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        #endregion

        internal async Task PropertyUpdate(Layout layout)
        {
            HorizontalSpacing = await SfBaseUtils.UpdateProperty(layout.HorizontalSpacing, HorizontalSpacing, HorizontalSpacingChanged, null, null);
            VerticalSpacing = await SfBaseUtils.UpdateProperty(layout.VerticalSpacing, VerticalSpacing, VerticalSpacingChanged, null, null);
            Orientation = await SfBaseUtils.UpdateProperty(layout.Orientation, Orientation, OrientationChanged, null, null);
            FixedNode = await SfBaseUtils.UpdateProperty(layout.FixedNode, FixedNode, FixedNodeChanged, null, null);
            Bounds = await SfBaseUtils.UpdateProperty(layout.Bounds, Bounds, BoundsChanged, null, null);
            Type = await SfBaseUtils.UpdateProperty(layout.Type, Type, TypeChanged, null, null);
            HorizontalAlignment = await SfBaseUtils.UpdateProperty(layout.HorizontalAlignment, HorizontalAlignment, HorizontalAlignmentChanged, null, null);
            VerticalAlignment = await SfBaseUtils.UpdateProperty(layout.VerticalAlignment, VerticalAlignment, VerticalAlignmentChanged, null, null);
            if (layout.Margin != null)
                Margin = await Margin.PropertyUpdate(layout.Margin) as LayoutMargin;
        }

        internal void UpdateFixedNode(string fixedNodeObj)
        {
            FixedNode = fixedNodeObj;
        }

        internal static Layout GetDefaultMindMapLayout(Layout layoutProp)
        {
            return new Layout()
            {
                Type = LayoutType.HierarchicalTree,
                FixedNode = layoutProp.FixedNode,
                HorizontalSpacing = layoutProp.VerticalSpacing,
                VerticalSpacing = layoutProp.HorizontalSpacing,
                HorizontalAlignment = layoutProp.HorizontalAlignment,
                VerticalAlignment = layoutProp.VerticalAlignment,
                LayoutInfo = layoutProp.LayoutInfo,
                Margin = layoutProp.Margin,
                GetLayoutInfo = layoutProp.GetLayoutInfo,
                Root = layoutProp.FixedNode
            };
        }

        internal void UpdateOrientation(LayoutOrientation layoutOrientation)
        {
            Orientation = layoutOrientation;
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (fixedNode != null)
            {
                fixedNode = null;
            }
            if (bounds != null)
            {
                bounds.Dispose();
                bounds = null;
            }
            if (FixedNode != null)
            {
                FixedNode = null;
            }
            if (ChildContent != null)
            {
                ChildContent = null;
            }
            if (GetLayoutInfo != null)
            {
                GetLayoutInfo = null;
            }
            if (GetBranch != null)
            {
                GetBranch = null;
            }

            if (Parent != null)
            {
                Parent = null;
            }
            if (Bounds != null)
            {
                Bounds.Dispose();
                Bounds = null;
            }

            if (Root != null)
            {
                Root = null;
            }
            if (Margin != null)
            {
                Margin.Dispose();
                Margin = null;
            }
            if (LayoutInfo != null)
            {
                LayoutInfo = null;
            }
        }
    }

    /// <summary>
    /// Represents the behavior and appearance of the tree.
    /// </summary>
    public class TreeInfo
    {
        /// <summary>
        /// Arranges the child nodes with the parent based on its type.
        /// </summary>
        public Orientation Orientation { get; set; }
        /// <summary>
        /// Specifies the type of subtree alignments in a layout. 
        /// </summary>
        public SubTreeAlignmentType AlignmentType { get; set; }
        /// <summary>
        /// Specifies the position of the node to be arranged. 
        /// </summary>
        public double Offset { get; set; }
        /// <summary>
        /// Specifies whether the route has to be enabled or not.
        /// </summary>
        public bool EnableRouting { get; set; }
        /// <summary>
        /// Represents the children in the tree . 
        /// </summary>
        public List<string> Children { get; set; } = new List<string>();
        /// <summary>
        /// Represents the tree assistants.
        /// </summary>
        public List<string> Assistants { get; set; } = new List<string>();
        /// <summary>
        /// Sets the level of the tree. It is a double-type.
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// Represents the type that enables or disables the sub tree.
        /// </summary>
        public bool HasSubTree { get; set; }
        /// <summary>
        /// Represents the rows of a tree.
        /// </summary>
        public int? Rows { get; set; }
        /// <summary>
        /// Enables or disables the sub tree.
        /// </summary>
        public bool EnableSubTree { get; set; }
        /// <summary>
        /// Represents the type that enables or disables the root inverse.
        /// </summary>
        public bool IsRootInverse { get; set; }
    }

}
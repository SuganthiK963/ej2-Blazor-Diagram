using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the animation to appear while collapsing the TreeView item.
    /// </summary>
    public partial class TreeViewAnimationCollapse : AnimationCollapseModel
    {
        [CascadingParameter]
        private TreeViewNodeAnimationSettings Parent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateCollapseProperties(this);
        }

        /// <summary>
        /// Dispose the Expand animation value.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
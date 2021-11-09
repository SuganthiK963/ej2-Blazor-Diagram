using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The TreeView component allows you to customize the look of TreeView nodes using Templates.
    /// </summary>
    /// <typeparam name="TValue">"Specifies the Tvalue".</typeparam>
    public class TreeViewTemplates<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        private SfTreeView<TValue> Parent { get; set; }

        /// <summary>
        /// Specifies the NodeTemplate.
        /// </summary>
        [Parameter]
        public RenderFragment<TValue> NodeTemplate { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("TreeViewTemplates", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class used for configuring the Tab animation properties.
    /// </summary>
    public partial class TabAnimationSettings : SfBaseComponent
    {
        private TabAnimationNext next;
        private TabAnimationPrevious previous;

        [CascadingParameter]
        internal SfTab Parent { get; set; }

        /// <summary>
        /// Child Content for the Tab Animation Settings.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the animation to appear when moving to the next Tab content.
        /// </summary>
        [Parameter]
        public TabAnimationNext Next { get; set; }

        /// <summary>
        /// Specifies the animation to appear when moving to the previous Tab content.
        /// </summary>
        [Parameter]
        public TabAnimationPrevious Previous { get; set; }

        internal void UpdatePreviousProperties(TabAnimationPrevious animation)
        {
            var previous = animation ?? new TabAnimationPrevious();
            Previous = this.previous = previous;
        }

        internal void UpdateNextProperties(TabAnimationNext animation)
        {
            var next = animation ?? new TabAnimationNext();
            Next = this.next = next;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateAnimationProperties(this);
            UpdateNextProperties(Next);
            UpdatePreviousProperties(Previous);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            next = NotifyPropertyChanges(nameof(Next), Next, next);
            previous = NotifyPropertyChanges(nameof(Previous), Previous, previous);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}
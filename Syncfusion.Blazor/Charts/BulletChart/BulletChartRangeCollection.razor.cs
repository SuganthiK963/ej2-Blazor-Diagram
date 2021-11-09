using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Set and gets the color mapping range collection of the bullet chart component.
    /// </summary>
    public partial class BulletChartRangeCollection
    {
        [CascadingParameter]
        internal IBulletChart Parent { get; set; }

        internal List<BulletChartRange> Ranges { get; set; } = new List<BulletChartRange>();

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Ranges = Ranges;
        }

        internal override void ComponentDispose()
        {
            ChildContent = null;
            Parent = null;
            Ranges = null;
        }
    }
}
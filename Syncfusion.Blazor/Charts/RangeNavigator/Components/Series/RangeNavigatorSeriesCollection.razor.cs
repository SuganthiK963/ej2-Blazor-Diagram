using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the series collections that are added to the range navigator.
    /// </summary>
    public partial class RangeNavigatorSeriesCollection
    {
        [CascadingParameter]
        internal SfRangeNavigator Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<RangeNavigatorSeries> Series { get; set; } = new List<RangeNavigatorSeries>();

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Parent.UpdateChildProperties(nameof(Series), Series);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            Series = null;
            ChildContent = null;
        }
    }
}
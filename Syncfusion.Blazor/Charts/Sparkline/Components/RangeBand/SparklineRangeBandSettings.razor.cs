using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    ///  Sets and gets the options for providing the list of range band of the sparkline component.
    /// </summary>
    public partial class SparklineRangeBandSettings
    {
        [CascadingParameter]
        internal ISparkline Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<SparklineRangeBand> RangeBand { get; set; } = new List<SparklineRangeBand>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.RangeBandSettings = RangeBand;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}
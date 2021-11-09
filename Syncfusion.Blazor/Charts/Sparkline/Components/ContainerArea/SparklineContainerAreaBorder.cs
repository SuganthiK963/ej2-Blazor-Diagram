using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the border of the sparkline component.
    /// </summary>
    public partial class SparklineContainerAreaBorder : BorderSettings
    {
        [CascadingParameter]
        internal SparklineContainerArea Parent { get; set; }

        [CascadingParameter]
        internal ISparkline BaseParent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Border = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.OnPropertyChanged(PropertyChanges, nameof(SparklineContainerArea));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
        }
    }
}
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the X and Y axis of the sparkline data label.
    /// </summary>
    public partial class SparklineDataLabelOffset : SfBaseComponent
    {
        private double x;
        private double y;

        [CascadingParameter]
        internal SparklineDataLabelSettings Parent { get; set; }

        [CascadingParameter]
        internal ISparkline BaseParent { get; set; }

        /// <summary>
        /// Sets and gets the X axis of the data label.
        /// </summary>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Sets and gets the Y axis of the data label.
        /// </summary>
        [Parameter]
        public double Y { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Offset = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            x = NotifyPropertyChanges(nameof(X), X, x);
            y = NotifyPropertyChanges(nameof(Y), Y, y);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.OnPropertyChanged(PropertyChanges, nameof(SparklineDataLabelOffset));
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
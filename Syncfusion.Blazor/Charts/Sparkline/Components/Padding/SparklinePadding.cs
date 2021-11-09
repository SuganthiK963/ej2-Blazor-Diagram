using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the top, left, right and bottom padding of the sparkline.
    /// </summary>
    public class SparklinePadding : SfBaseComponent
    {
        private double bottom;
        private double left;
        private double right;
        private double top;

        [CascadingParameter]
        internal ISparkline Parent { get; set; }

        /// <summary>
        /// Sets and gets the sparkline bottom padding.
        /// </summary>
        [Parameter]
        public double Bottom { get; set; } = 5;

        /// <summary>
        /// Sets and gets the sparkline left padding.
        /// </summary>
        [Parameter]
        public double Left { get; set; } = 5;

        /// <summary>
        /// Sets and gets the sparkline right padding.
        /// </summary>
        [Parameter]
        public double Right { get; set; } = 5;

        /// <summary>
        /// Sets and gets the sparkline top padding.
        /// </summary>
        [Parameter]
        public double Top { get; set; } = 5;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Padding = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            bottom = NotifyPropertyChanges(nameof(Bottom), Bottom, bottom);
            top = NotifyPropertyChanges(nameof(Top), Top, top);
            left = NotifyPropertyChanges(nameof(Left), Left, left);
            right = NotifyPropertyChanges(nameof(Right), Right, right);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(SparklinePadding));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
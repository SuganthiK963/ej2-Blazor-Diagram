using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the bottom, left, right, top margin of the legend.
    /// </summary>
    public class BulletChartLegendMargin : SfBaseComponent
    {
        private double bottom;
        private double left;
        private double right;
        private double top;

        [CascadingParameter]
        internal IBulletChart BaseParent { get; set; }

        [CascadingParameter]
        internal BulletChartLegendSettings Parent { get; set; }

        /// <summary>
        /// Sets and gets the bottom margin for the legend.
        /// </summary>
        [Parameter]
        public double Bottom { get; set; } = 10.0;

        /// <summary>
        /// Sets and gets the left margin for the legend.
        /// </summary>
        [Parameter]
        public double Left { get; set; } = 10.0;

        /// <summary>
        /// Sets and gets the right margin for the legend.
        /// </summary>
        [Parameter]
        public double Right { get; set; } = 10.0;

        /// <summary>
        /// Sets and gets the top margin for the legend.
        /// </summary>
        [Parameter]
        public double Top { get; set; } = 10.0;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Margin = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            bottom = NotifyPropertyChanges(nameof(Bottom), Bottom, bottom);
            left = NotifyPropertyChanges(nameof(Left), Left, left);
            right = NotifyPropertyChanges(nameof(Right), Right, right);
            top = NotifyPropertyChanges(nameof(Top), Top, top);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await BaseParent.OnPropertyChanged(PropertyChanges, nameof(BulletChartLegendMargin));
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
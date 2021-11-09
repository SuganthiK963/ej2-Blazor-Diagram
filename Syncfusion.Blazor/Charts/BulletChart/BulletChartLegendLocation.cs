using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the custom position of the legend location using x and y values for the bullet chart component.
    /// </summary>
    public class BulletChartLegendLocation : SfBaseComponent
    {
        private double x;
        private double y;

        [CascadingParameter]
        internal BulletChartLegendSettings Parent { get; set; }

        [CascadingParameter]
        internal IBulletChart BaseParent { get; set; }

        /// <summary>
        /// Sets and gets the X coordinate of the legend.
        /// </summary>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Sets and gets the Y coordinate of the legend.
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
            Parent.Location = this;
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
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await BaseParent.ChartLegend.OnPropertyChanged(PropertyChanges, nameof(BulletChartLegendLocation));
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
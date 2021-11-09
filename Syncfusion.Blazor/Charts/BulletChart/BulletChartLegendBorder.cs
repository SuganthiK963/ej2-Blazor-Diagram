using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the option foe customizing the legend border of the bullet chart component.
    /// </summary>
    public partial class BulletChartLegendBorder : BulletChartCommonBorder
    {
        private string color;
        private double width;

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        internal BulletChartLegendSettings Parent { get; set; }

        [CascadingParameter]
        internal IBulletChart BaseParent { get; set; }

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
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await BaseParent.ChartLegend.OnPropertyChanged(PropertyChanges, nameof(BulletChartLegendBorder));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
            ChildContent = null;
        }
    }
}
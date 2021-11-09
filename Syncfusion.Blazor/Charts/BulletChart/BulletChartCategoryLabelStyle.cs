using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the category label style of the bullet chart component.
    /// </summary>
    public class BulletChartCategoryLabelStyle : BulletChartCommonFont
    {
        private string color;
        private double opacity;
        private string size;

        [CascadingParameter]
        internal IBulletChart Parent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.CategoryLabelStyle = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            size = NotifyPropertyChanges(nameof(Size), Size, size);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(BulletChartCategoryLabelStyle));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
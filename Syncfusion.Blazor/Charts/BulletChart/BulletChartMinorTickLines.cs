using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the minor tick lines of the bullet chart component.
    /// </summary>
    public class BulletChartMinorTickLines : SfBaseComponent
    {
        private string color;
        private double height;
        private double width;
        private bool enableRangeColor;

        [CascadingParameter]
        internal IBulletChart Parent { get; set; }

        /// <summary>
        /// The color of the major tick line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Sets and gets the height of the ticks lines.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 8;

        /// <summary>
        /// Sets and gets the width of the tick lines.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// Sets and gets the range color of the text.
        /// </summary>
        [Parameter]
        public bool EnableRangeColor { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.MinorTickLines = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            enableRangeColor = NotifyPropertyChanges(nameof(EnableRangeColor), EnableRangeColor, enableRangeColor);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(BulletChartMinorTickLines));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
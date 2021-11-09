using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the color and width of the border.
    /// </summary>
    public partial class ChartDefaultBorder : ChartSubComponent
    {
        private string color;
        private double width = 1;

        [CascadingParameter]
        internal SfChart Owner { get; set; }

        /// <summary>
        /// Sets and gets the color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public virtual string Color
        {
            get
            {
                return color;
            }

            set
            {
                if (color != value)
                {
                    color = value;
                    IsPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Sets and gets the the width of the border in pixels.
        /// </summary>
        [Parameter]
        public virtual double Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width != value)
                {
                    width = value;
                    IsPropertyChanged = true;
                }
            }
        }
    }
}
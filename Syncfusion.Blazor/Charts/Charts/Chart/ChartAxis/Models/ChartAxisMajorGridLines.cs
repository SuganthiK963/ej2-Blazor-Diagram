using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    public class ChartAxisMajorGridLines : ChartSubComponent
    {
        private string color;

        private double width = 1;

        private string dashArray = string.Empty;

        private bool isPropertyChanged;

        [CascadingParameter]
        private ChartAxis axis { get; set; }

        /// <summary>
        /// The color of the major grid line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color
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
                    isPropertyChanged = axis != null;
                }
            }
        }

        /// <summary>
        /// The dash array of the grid lines.
        /// </summary>
        [Parameter]
        public string DashArray
        {
            get
            {
                return dashArray;
            }

            set
            {
                if (dashArray != value)
                {
                    dashArray = value;
                    isPropertyChanged = axis != null;
                }
            }
        }

        /// <summary>
        /// The width of the line in pixels.
        /// </summary>
        [Parameter]
        public double Width
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
                    isPropertyChanged = axis != null;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            axis = (ChartAxis)Tracker;
            axis.UpdateAxisProperties("MajorGridLines", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                axis.UpdateAxisProperties("MajorGridLines", this);
            }
        }
    }
}

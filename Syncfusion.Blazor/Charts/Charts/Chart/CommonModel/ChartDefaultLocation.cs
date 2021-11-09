using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the location.
    /// </summary>
    public partial class ChartDefaultLocation : ChartSubComponent
    {
        private double x;
        private double y;

        [CascadingParameter]
        internal SfChart Chart { get; set; }

        /// <summary>
        ///  Sets and gets the x coordinate of the legend in pixels.
        /// </summary>
        [Parameter]
        public virtual double X
        {
            get
            {
                return x;
            }

            set
            {
                if (x != value)
                {
                    x = value;
                    IsPropertyChanged = true;
                }
            }
        }

        /// <summary>
        ///  Sets and gets the y coordinate of the legend in pixels.
        /// </summary>
        [Parameter]
        public virtual double Y
        {
            get
            {
                return y;
            }

            set
            {
                if (y != value)
                {
                    y = value;
                    IsPropertyChanged = true;
                }
            }
        }
    }
}
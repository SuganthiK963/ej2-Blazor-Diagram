using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the scrollbar setings of the axis.
    /// </summary>
    public partial class ChartCommonScrollbarSettings : ChartSubComponent
    {
        private bool enable;
        private double pointsLength;

        private ChartAxisScrollbarSettingsRange range = new ChartAxisScrollbarSettingsRange();

        /// <summary>
        /// Enables the scrollbar for lazy loading.
        /// </summary>
        [Parameter]
        public bool Enable
        {
            get
            {
                return enable;
            }

            set
            {
                if (enable != value)
                {
                    enable = value;
                    IsPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Defines the length of the points for numeric and logarithmic values.
        /// </summary>
        [Parameter]
        public double PointsLength
        {
            get
            {
                return pointsLength;
            }

            set
            {
                if (pointsLength != value)
                {
                    pointsLength = value;
                    IsPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Specifies the range for date time values alone.
        /// </summary>
        [Parameter]
        public ChartAxisScrollbarSettingsRange Range
        {
            get
            {
                return range;
            }

            set
            {
                if (range != value)
                {
                    range = value;
                    IsPropertyChanged = true;
                }
            }
        }
    }
}
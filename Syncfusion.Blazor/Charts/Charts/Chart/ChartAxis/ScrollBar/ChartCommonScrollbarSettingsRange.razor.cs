using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the scrollbar range of the axis.
    /// </summary>
    public partial class ChartCommonScrollbarSettingsRange : ChartSubComponent
    {
        private string maximum;
        private string minimum;

        /// <summary>
        /// Specifies the maximum range of an scrollbar.
        /// </summary>
        [Parameter]
        public string Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                if (maximum != value)
                {
                    maximum = value;
                    IsPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Specifies the minimum range of an scrollbar.
        /// </summary>
        [Parameter]
        public string Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                if (minimum != value)
                {
                    minimum = value;
                    IsPropertyChanged = true;
                }
            }
        }
    }
}
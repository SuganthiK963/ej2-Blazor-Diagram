using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the bottom, left, right, top margin of the chart component.
    /// </summary>
    public partial class ChartDefaultMargin : ChartSubComponent
    {
        internal bool isPropertyChanged;
        private double bottom = 10;
        private double top = 10;
        private double right = 10;
        private double left = 10;

        /// <summary>
        /// Sets and gets the bottom margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Bottom
        {
            get
            {
                return bottom;
            }

            set
            {
                if (bottom != value)
                {
                    bottom = value;
                    isPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Sets and gets the left margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Left
        {
            get
            {
                return left;
            }

            set
            {
                if (left != value)
                {
                    left = value;
                    isPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Sets and gets the right margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Right
        {
            get
            {
                return right;
            }

            set
            {
                if (right != value)
                {
                    right = value;
                    isPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// Sets and gets the top margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Top
        {
            get
            {
                return top;
            }

            set
            {
                if (top != value)
                {
                    top = value;
                    isPropertyChanged = true;
                }
            }
        }
    }
}
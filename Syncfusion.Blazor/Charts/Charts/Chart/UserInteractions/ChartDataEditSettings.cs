using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the drag settings for series.
    /// </summary>
    public class ChartDataEditSettings : ChartSubComponent
    {
        private bool enable;
        private string fill;
        private double maxY = double.NaN;
        private double minY = double.NaN;

        [CascadingParameter]
        private ChartSeries series { get; set; }

        /// <summary>
        /// To enable the drag the points.
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
        /// To set the color of the edited point.
        /// </summary>
        [Parameter]
        public string Fill
        {
            get
            {
                return fill;
            }

            set
            {
                if (fill != value)
                {
                    fill = value;
                    IsPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// To set the maximum y of the point.
        /// </summary>
        [Parameter]
        public double MaxY
        {
            get
            {
                return maxY;
            }

            set
            {
                if (maxY != value)
                {
                    maxY = value;
                    IsPropertyChanged = true;
                }
            }
        }

        /// <summary>
        /// To set the minimum y of the point.
        /// </summary>
        [Parameter]
        public double MinY
        {
            get
            {
                return minY;
            }

            set
            {
                if (minY != value)
                {
                    minY = value;
                    IsPropertyChanged = true;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            series = (ChartSeries)Tracker;
            series.UpdateSeriesProperties("ChartDataEditSettings", this);
        }

        internal override void ComponentDispose()
        {
            series = null;
            ChildContent = null;
        }
    }
}
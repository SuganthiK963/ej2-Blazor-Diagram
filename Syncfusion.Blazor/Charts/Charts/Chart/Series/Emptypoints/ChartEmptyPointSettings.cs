using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the Empty Point configuration of the chart.
    /// </summary>
    public class ChartEmptyPointSettings : ChartSubComponent
    {
        #region EMPTYPOINT PRIVATE FIELDS

        private EmptyPointMode mode = EmptyPointMode.Gap;
        private string fill;
        private ChartEmptyPointBorder border = new ChartEmptyPointBorder();
        #endregion

        [CascadingParameter]
        internal ChartSeries Series { get; set; }

        #region EMPTYPOINT API

        /// <summary>
        /// Sets and gets the mode of the empty point.
        /// </summary>
        [Parameter]
        public EmptyPointMode Mode
        {
            get
            {
                return mode;
            }

            set
            {
                if (mode != value)
                {
                    mode = value;
                    if (Series != null)
                    {
                        Series.Renderer?.UpdateEmptyPoint();
                    }
                }
            }
        }

        /// <summary>
        /// Sets and gets the fill of the empty point.
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
                    if (Series != null)
                    {
                        Series.Marker.Renderer?.UpdateDirection();
                    }
                }
            }
        }

        /// <summary>
        /// Sets and gets the border of the empty point.
        /// </summary>
        [Parameter]
        public ChartEmptyPointBorder Border
        {
            get
            {
                return border;
            }

            set
            {
                if (border != value)
                {
                    border = value;
                }
            }
        }
        #endregion

        internal void UpdateEmptyPointProperties(string key, object keyValue)
        {
            if (key == "Border")
            {
                Border = border = (ChartEmptyPointBorder)keyValue;
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Series.UpdateSeriesProperties("EmptyPointSettings", this);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties("EmptyPointSettings", this);
        }

        internal override void ComponentDispose()
        {
            Series = null;
            Border = null;
        }
    }
}

using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the text style of axis's labels.
    /// </summary>
    public class ChartAxisLabelStyle : ChartDefaultFont
    {
        private string size = "12px";

        private string color;

        private bool isPropertyChanged;

        [CascadingParameter]
        internal ChartAxis axis { get; set; }

        /// <summary>
        /// Unique size of the axis labels.
        /// </summary>
        [Parameter]
        public override string Size
        {
            get
            {
                return size;
            }

            set
            {
                if (size != value)
                {
                    size = value;
                    isPropertyChanged = axis != null;
                }
            }
        }

        /// <summary>
        /// Font color for the axis title.
        /// </summary>
        [Parameter]
        public override string Color
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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            axis = (ChartAxis)Tracker;
            axis.UpdateAxisProperties("LabelStyle", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                axis.UpdateAxisProperties("LabelStyle", this);
            }
        }
    }
}
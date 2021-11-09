using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of the corner radius for the reactangle type series.
    /// </summary>
    public class ChartCornerRadius : ChartSubComponent
    {
        #region CORNERRADIUS BACKING FIELD
        private double bottomLeft;
        private double bottomRight;
        private double topLeft;
        private double topRight;
        private bool isPropertyChanged;
        #endregion

        [CascadingParameter]
        private ChartSeries Series { get; set; }

        /// <summary>
        /// Sets and gets the bottom left value of the corner radius.
        /// </summary>
        [Parameter]
        public double BottomLeft
        {
            get
            {
                return bottomLeft;
            }

            set
            {
                if (bottomLeft != value)
                {
                    bottomLeft = value;
                    isPropertyChanged = Series != null;
                }
            }
        }

        /// <summary>
        /// Sets and gets the bottom right value of the corner radius.
        /// </summary>
        [Parameter]
        public double BottomRight
        {
            get
            {
                return bottomRight;
            }

            set
            {
                if (bottomRight != value)
                {
                    bottomRight = value;
                    isPropertyChanged = Series != null;
                }
            }
        }

        /// <summary>
        /// Sets and gets the top left value of the corner radius.
        /// </summary>
        [Parameter]
        public double TopLeft
        {
            get
            {
                return topLeft;
            }

            set
            {
                if (topLeft != value)
                {
                    topLeft = value;
                    isPropertyChanged = Series != null;
                }
            }
        }

        /// <summary>
        /// Sets and gets the top right value of the corner radius.
        /// </summary>
        [Parameter]
        public double TopRight
        {
            get
            {
                return topRight;
            }

            set
            {
                if (topRight != value)
                {
                    topRight = value;
                    isPropertyChanged = Series != null;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties("CornerRadius", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                Series.Renderer.UpdateDirection();
                Series.Renderer.ProcessRenderQueue();
            }
        }

        internal override void ComponentDispose()
        {
            Series = null;
            ChildContent = null;
        }
    }
}
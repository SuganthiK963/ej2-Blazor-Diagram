using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the ChartArea of the chart.
    /// </summary>
    public class ChartArea : ChartSubComponent
    {
        #region Backing private fields
        private string background = Constants.TRANSPARENT;
        private string backgroundImage;
        private double opacity = 1;
        private ChartAreaBorder border = new ChartAreaBorder();
        #endregion

        [CascadingParameter]
        internal SfChart Owner { get; set; }

        #region API

        /// <summary>
        /// The background of the chart area that accepts value in hex and rgba as a valid CSS color string..
        /// </summary>
        [Parameter]
        public string Background
        {
            get
            {
                return background;
            }

            set
            {
                if (background != value)
                {
                    background = value;
                }
            }
        }

        /// <summary>
        /// The background image of the chart area that accepts value in string as url link or location of an image.
        /// </summary>
        [Parameter]
        public string BackgroundImage
        {
            get
            {
                return backgroundImage;
            }

            set
            {
                if (backgroundImage != value)
                {
                    backgroundImage = value;
                }
            }
        }

        /// <summary>
        /// The opacity for background.
        /// </summary>
        [Parameter]
        public double Opacity
        {
            get
            {
                return opacity;
            }

            set
            {
                if (opacity != value)
                {
                    opacity = value;
                }
            }
        }

        /// <summary>
        /// Options to customize the border of the chart area.
        /// </summary>
        [Parameter]
        public ChartAreaBorder Border
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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.ChartAreaRenderer.Area = this;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner.ChartAreaRenderer.RendererShouldRender = true;
            Owner.ChartAreaRenderer.ProcessRenderQueue();
        }

        internal void UpdateBorder(ChartAreaBorder chartBorder)
        {
            border = chartBorder;
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            border.ComponentDispose();
            Owner = null;
            ChildContent = null;
        }
    }
}
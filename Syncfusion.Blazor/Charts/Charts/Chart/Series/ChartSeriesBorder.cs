using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the border of the series.
    /// </summary>
    public class ChartSeriesBorder : ChartSubComponent
    {
        #region CORNERRADIUS BACKING FIELD
        private string color;
        private double width = 1;
        private bool isPropertyChanged;
        #endregion

        [CascadingParameter]
        private ChartSeries Series { get; set; }

        /// <summary>
        /// Sets and gets the color of the border that accepts value in hex and rgba as a valid CSS color string.
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
                    Series?.Renderer?.UpdateCustomization(nameof(Color));
                    isPropertyChanged = Series != null;
                }
            }
        }

        /// <summary>
        /// Sets and gets the the width of the border in pixels.
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
                    Series?.Renderer?.UpdateCustomization(nameof(Width));
                    isPropertyChanged = Series != null;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties("Border", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
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
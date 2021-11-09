using System;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the collection of trendlines that are used to predict the trend.
    /// </summary>
    public class ChartTrendline : ChartSubComponent, IChartElement
    {
       
        #region CHARTTRENDLINE PRIVATE FIELDS

        private string name = string.Empty;
        private string dashArray = "0";
        private bool visible = true;
        private TrendlineTypes type;
        private double period = 2;
        private double polynomialOrder = 2;
        private double backwardForecast;
        private double forwardForecast;
        private bool enableTooltip = true;
        private double intercept = double.NaN;
        private string fill = string.Empty;
        private double width = 1;
        private LegendShape legendShape = LegendShape.SeriesType;
        private SfChart chart;
        #endregion

        [CascadingParameter]
        internal ChartTrendlines Parent { get; set; }

        #region TRENDLINE API

        /// <summary>
        /// Sets and gets the value of the segment series.
        /// </summary>
        [Parameter]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (name != value)
                {
                    name = value;
                    if (Renderer != null)
                    {
                        Renderer.Series.SetName(name);
                        chart.OnLayoutChange();
                    }
                }
            }
        }

        /// <summary>
        /// Sets and gets the color of the segment series.
        /// </summary>
        [Parameter]
        public string DashArray
        {
            get
            {
                return dashArray;
            }

            set
            {
                if (dashArray != value)
                {
                    dashArray = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public bool Visible
        {
            get
            {
                return visible;
            }

            set
            {
                if (visible != value)
                {
                    visible = value;
                    if (TargetSeries?.Renderer != null)
                    {
                        TargetSeries.Renderer.RendererShouldRender = true;
                        TargetSeries.Renderer.TrendLineLegendVisibility = visible;
                        TargetSeries.Renderer.ProcessRenderQueue();
                        if (chart != null && chart.LegendRenderer != null)
                        {
                            chart.LegendRenderer.RendererShouldRender = true;
                            chart.LegendRenderer.UpdateLegendFill(TargetSeries.Renderer);
                            chart.LegendRenderer.ProcessRenderQueue();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public TrendlineTypes Type
        {
            get
            {
                return type;
            }

            set
            {
                if (type != value)
                {
                    type = value;
                    if (chart != null)
                    {
                        chart.TrendlineContainer.RemoveRenderer(TargetSeries.Renderer);
                        TrendlineInitiator.InitSeriesCollection();
                        chart.TrendlineContainer.RendererShouldRender = true;
                        chart.TrendlineContainer.Prerender();
                    }
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public double Period
        {
            get
            {
                return period;
            }

            set
            {
                if (period != value)
                {
                    period = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public double PolynomialOrder
        {
            get
            {
                return polynomialOrder;
            }

            set
            {
                if (polynomialOrder != value)
                {
                    polynomialOrder = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public double BackwardForecast
        {
            get
            {
                return backwardForecast;
            }

            set
            {
                if (backwardForecast != value)
                {
                    backwardForecast = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public double ForwardForecast
        {
            get
            {
                return forwardForecast;
            }

            set
            {
                if (forwardForecast != value)
                {
                    forwardForecast = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public bool EnableTooltip
        {
            get
            {
                return enableTooltip;
            }

            set
            {
                if (enableTooltip != value)
                {
                    enableTooltip = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public double Intercept
        {
            get
            {
                return intercept;
            }

            set
            {
                if (intercept != value)
                {
                    intercept = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
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
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
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
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public LegendShape LegendShape
        {
            get
            {
                return legendShape;
            }

            set
            {
                if (legendShape != value)
                {
                    legendShape = value;
                }
            }
        }

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public ChartTrendlineMarker Marker { get; set; } = new ChartTrendlineMarker();

        /// <summary>
        /// Sets and gets the dash array of the segment series.
        /// </summary>
        [Parameter]
        public ChartTrendlineAnimation Animation { get; set; } = new ChartTrendlineAnimation();
        #endregion

        public Type RendererType { get; set; }

        public string RendererKey { get; set; } = SfBaseUtils.GenerateID("charttrendline");

        internal ChartSeries TargetSeries { get; set; }

        internal TrendlineBase TrendlineInitiator { get; set; }

        internal ChartSeriesRenderer Renderer { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartTrendlines)Tracker;
            chart = Parent.Series.Container;
            Parent.Trendlines.Add(this);
            InitTrendline();
        }

        internal void InitTrendline()
        {
            TrendlineInitiator = new TrendlineBase();
            TrendlineInitiator.Trendline = this;
            TrendlineInitiator.InitSeriesCollection();
            Parent.Series.Container.AddTrendline(this);
        }

        internal void UpdateTrendlineProperty(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Animation):
                    Animation = (ChartTrendlineAnimation)keyValue;
                    break;
                case nameof(Marker):
                    Marker = (ChartTrendlineMarker)keyValue;
                    break;
            }
        }

        internal void PolynomialOrderValue(double value)
        {
            PolynomialOrder = value;
        }

        internal void SetVisibility(bool value)
        {
            Visible = value;
        }

        internal override void ComponentDispose()
        {
            chart.TrendlineContainer.RemoveElement(this);
            Parent.Trendlines.Remove(this);
        }
    }
}

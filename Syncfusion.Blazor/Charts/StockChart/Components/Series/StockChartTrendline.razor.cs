using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart trendline.
    /// </summary>
    public partial class StockChartTrendline : SfDataBoundComponent
    {
        private double backwardForecast;
        private bool enableTooltip;
        private string fill;
        private double forwardForecast;
        private double intercept;
        private LegendShape legendShape;
        private string name;
        private double period;
        private double polynomialOrder;
        private TrendlineTypes type;
        private double width;

        internal string RendererKey;
        internal StockChartMarkerSettings Marker { get; set; } = new StockChartMarkerSettings();

        internal AnimationModel Animation { get; set; } = new AnimationModel();

        [CascadingParameter]
        internal StockChartTrendlines Parent { get; set; }

        [CascadingParameter]
        internal SfStockChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the period, by which the trend has to backward forecast.
        /// </summary>
        [Parameter]
        public double BackwardForecast { get; set; }

        /// <summary>
        /// Enables/disables tooltip for trendlines.
        /// </summary>
        [Parameter]
        public bool EnableTooltip { get; set; } = true;

        /// <summary>
        /// Defines the fill color of trendline.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Defines the period, by which the trend has to forward forecast.
        /// </summary>
        [Parameter]
        public double ForwardForecast { get; set; }

        /// <summary>
        /// Defines the intercept of the trendline.
        /// </summary>
        [Parameter]
        public double Intercept { get; set; } = default;

        /// <summary>
        /// Sets the legend shape of the trendline.
        /// </summary>
        [Parameter]
        public LegendShape LegendShape { get; set; } = LegendShape.SeriesType;

        /// <summary>
        /// Defines the name of trendline.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Defines the period, the price changes over which will be considered to predict moving average trend line.
        /// </summary>
        [Parameter]
        public double Period { get; set; } = 2;

        /// <summary>
        /// Defines the polynomial order of the polynomial trendline.
        /// </summary>
        [Parameter]
        public double PolynomialOrder { get; set; } = 2;

        /// <summary>
        /// Defines the type of the trendline.
        /// </summary>
        [Parameter]
        public TrendlineTypes Type { get; set; } = TrendlineTypes.Linear;

        /// <summary>
        /// Defines the width of the trendline.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Trendlines.Add(this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            backwardForecast = NotifyPropertyChanges(nameof(BackwardForecast), BackwardForecast, backwardForecast);
            enableTooltip = NotifyPropertyChanges(nameof(EnableTooltip), EnableTooltip, enableTooltip);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            forwardForecast = NotifyPropertyChanges(nameof(ForwardForecast), ForwardForecast, forwardForecast);
            intercept = NotifyPropertyChanges(nameof(Intercept), Intercept, intercept);
            legendShape = NotifyPropertyChanges(nameof(LegendShape), LegendShape, legendShape);
            name = NotifyPropertyChanges(nameof(Name), Name, name);
            period = NotifyPropertyChanges(nameof(Period), Period, period);
            polynomialOrder = NotifyPropertyChanges(nameof(PolynomialOrder), PolynomialOrder, polynomialOrder);
            type = NotifyPropertyChanges(nameof(Type), Type, type);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            if (PropertyChanges.Any() && IsRendered)
            {
                PropertyChanges.Clear();
                BaseParent.OnStockChartPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
            ChildContent = null;
            Animation = null;
            Marker = null;
        }
    }
}
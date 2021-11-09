using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Datalabels shows the information about the data points in the accumulation chart.
    /// </summary>
    public partial class AccumulationDataLabelSettings
    {
        [CascadingParameter]
        private AccumulationChartSeries series { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies angle for data label.
        /// </summary>
        [Parameter]
        public double Angle { get; set; }

        /// <summary>
        /// Option for customizing the border lines.
        /// </summary>
        [Parameter]
        public AccumulationChartDataLabelBorder Border { get; set; } = new AccumulationChartDataLabelBorder();

        private AccumulationChartDataLabelBorder border { get; set; }

        /// <summary>
        /// Options for customize the connector line in series.
        /// This property is applicable for Pie, Funnel and Pyramid series.
        /// The default connector length for Pie series is '4%'. For other series, it is null.
        /// </summary>
        [Parameter]
        public AccumulationChartConnector ConnectorStyle { get; set; } = new AccumulationChartConnector();

        private AccumulationChartConnector connectorStyle { get; set; }

        /// <summary>
        /// Enables rotation for data label.
        /// </summary>
        [Parameter]
        public bool EnableRotation { get; set; }

        private bool enableRotation { get; set; }

        /// <summary>
        /// The background color of the data label, which accepts value in hex, rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = Constants.TRANSPARENT;

        private string fill { get; set; }

        /// <summary>
        /// Option for customizing the data label text.
        /// </summary>
        [Parameter]
        public AccumulationChartDataLabelFont Font { get; set; } = new AccumulationChartDataLabelFont();

        private AccumulationChartDataLabelFont font { get; set; }

        /// <summary>
        /// The DataSource field which contains the data label value.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        private string name { get; set; }

        /// <summary>
        /// Specifies the position of data label. They are.
        /// Outside - Places label outside the point.
        /// Inside - Places label inside the point.
        /// </summary>
        [Parameter]
        public AccumulationLabelPosition Position { get; set; }

        private AccumulationLabelPosition position { get; set; }

        /// <summary>
        /// The roundedCornerX for the data label. It requires `Border` values not to be null.
        /// </summary>
        [Parameter]
        public double Rx { get; set; } = 5;

        private double rx { get; set; }

        /// <summary>
        /// The roundedCornerY for the data label. It requires `Border` values not to be null.
        /// </summary>
        [Parameter]
        public double Ry { get; set; } = 5;

        private double ry { get; set; }

        /// <summary>
        /// Custom template to format the data label content. Use ${point.x} and ${point.y} as a placeholder
        /// text to display the corresponding data point.
        /// </summary>
        [Parameter]
        public RenderFragment<AccumulationChartDataPointInfo> Template { get; set; }

        private RenderFragment<AccumulationChartDataPointInfo> template { get; set; }

        /// <summary>
        /// If set true, data label for series gets render.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        private bool visible { get; set; }

        /// <summary>
        /// If set true, datalabels will be visible for zero points.
        /// </summary>
        [Parameter]
        public bool ShowZero { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            series.UpdateSeriesProperties("DataLabel", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enableRotation = EnableRotation = NotifyPropertyChanges(nameof(EnableRotation), EnableRotation, enableRotation);
            fill = Fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            name = Name = NotifyPropertyChanges(nameof(Name), Name, name);
            position = Position = NotifyPropertyChanges(nameof(Position), Position, position);
            rx = Rx = NotifyPropertyChanges(nameof(Rx), Rx, rx);
            ry = Ry = NotifyPropertyChanges(nameof(Ry), Ry, ry);
            template = Template = NotifyPropertyChanges(nameof(Template), Template, template);
            visible = Visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            border = Border = NotifyPropertyChanges(nameof(Border), Border, border);
            connectorStyle = ConnectorStyle = NotifyPropertyChanges(nameof(ConnectorStyle), ConnectorStyle, connectorStyle);
            font = Font = NotifyPropertyChanges(nameof(Font), Font, font);
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)series).PropertyChanges.TryAdd(nameof(series.DataLabel), this);
                PropertyChanges.Clear();
                await series.SeriesPropertyChanged();
            }
        }

        internal async Task DataLabelPropertyChanged()
        {
            await OnParametersSetAsync();
#pragma warning restore CA2007
        }

        internal override void ComponentDispose()
        {
            series = null;
            ChildContent = null;
            Border = null;
            Font = null;
            ConnectorStyle = null;
        }

        internal void UpdateDataLabelProperties(string key, object dataLabelItem)
        {
            if (key == nameof(Border))
            {
                Border = (AccumulationChartDataLabelBorder)dataLabelItem;
            }
            else if (key == nameof(Font))
            {
                Font = (AccumulationChartDataLabelFont)dataLabelItem;
            }
            else if (key == nameof(ConnectorStyle))
            {
                ConnectorStyle = (AccumulationChartConnector)dataLabelItem;
            }
        }
    }
}
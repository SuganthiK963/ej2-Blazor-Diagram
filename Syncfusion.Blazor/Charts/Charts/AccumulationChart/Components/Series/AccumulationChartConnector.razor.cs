using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the connector line property for the Accumulation chart's datalabel.
    /// </summary>
    public partial class AccumulationChartConnector
    {
        [CascadingParameter]
        private AccumulationDataLabelSettings dataLabel { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Color of the connector line.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        private string color { get; set; }

        /// <summary>
        /// dashArray of the connector line.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        private string dashArray { get; set; }

        /// <summary>
        /// Length of the connector line in pixels.
        /// </summary>
        [Parameter]
        public string Length { get; set; }

        private string length { get; set; }

        /// <summary>
        /// specifies the type of the connector line. They are
        /// Smooth
        /// Line.
        /// </summary>
        [Parameter]
        public ConnectorType Type { get; set; }

        private ConnectorType type { get; set; }

        /// <summary>
        /// Width of the connector line in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        private double width { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            dataLabel.UpdateDataLabelProperties("ConnectorStyle", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = Color = NotifyPropertyChanges(nameof(Color), Color, color);
            dashArray = DashArray = NotifyPropertyChanges(nameof(DashArray), DashArray, dashArray);
            length = Length = NotifyPropertyChanges(nameof(Length), Length, length);
            type = Type = NotifyPropertyChanges(nameof(Type), Type, type);
            width = Width = NotifyPropertyChanges(nameof(Width), Width, width);
            if (PropertyChanges.Any() && IsRendered)
            {
                ((SfBaseComponent)dataLabel).PropertyChanges.TryAdd(nameof(dataLabel.ConnectorStyle), this);
                PropertyChanges.Clear();
                await dataLabel.DataLabelPropertyChanged();
#pragma warning restore CA2007
            }
        }

        internal override void ComponentDispose()
        {
            dataLabel = null;
            ChildContent = null;
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart stockevent.
    /// </summary>
    public partial class StockChartStockEvent : SfBaseComponent
    {
        internal StockChartStockEventsTextStyle TextStyle { get; set; } = new StockChartStockEventsTextStyle();

        internal StockChartStockEventsBorder Border { get; set; } = new StockChartStockEventsBorder();

        [CascadingParameter]
        internal StockChartStockEvents Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The background of the stock event that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background { get; set; } = "transparent";

        /// <summary>
        /// Date value of stock event in which stock event shows.
        /// </summary>
        [Parameter]
        public DateTime Date { get; set; }

        /// <summary>
        /// Specifies the description for the chart which renders in tooltip for stock event.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Corresponding values in which stock event placed.
        /// Close
        /// Open
        /// High
        /// Close.
        /// </summary>
        [Parameter]
        public string PlaceAt { get; set; } = "Close";

        /// <summary>
        /// Enables the stock events to be render on series. If it disabled, stock event rendered on primaryXAxis.
        /// </summary>
        [Parameter]
        public bool ShowOnSeries { get; set; } = true;

        /// <summary>
        /// Specifies the text for the stock chart text.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// To render stock events in particular series.
        /// </summary>
        [Parameter]
        public int[] SeriesIndexes { get; set; }

        /// <summary>
        /// Specifies type of stock events
        /// Circle
        /// Square
        /// Flag
        /// Text
        /// Sign
        /// Triangle
        /// InvertedTriangle
        /// ArrowUp
        /// ArrowDown
        /// ArrowLeft
        /// ArrowRight.
        /// </summary>
        [Parameter]
        public FlagType Type { get; set; } = FlagType.Circle;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.StockEvents.Add(this);
        }

        internal override void ComponentDispose()
        {
            Border = null;
            TextStyle = null;
            Parent = null;
            ChildContent = null;
            SeriesIndexes = null;
        }
    }
}
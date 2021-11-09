using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart events.
    /// </summary>
    public class StockChartEvents : SfBaseComponent
    {
        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Triggers before rendering the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<StockChartEventArgs> Load { get; set; }

        /// <summary>
        /// Triggers after rendering the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use 'OnLoaded' event to achieve this.")]
        [Parameter]
        public Action<StockChartEventArgs> Loaded { get; set; }

        /// <summary>
        /// Triggers after the stock chart rendering.
        /// </summary>
        [Parameter]
        public Action<StockChartEventArgs> OnLoaded { get; set; }

        /// <summary>
        /// Triggers after the zoom selection is done.
        /// </summary>
        [Parameter]
        public Action<StockChartZoomingEventArgs> OnZooming { get; set; }

        /// <summary>
        /// Triggers during point click.
        /// </summary>
        [Parameter]
        public Action<StockChartPointEventArgs> OnPointClick { get; set; }

        /// <summary>
        /// Triggers when point moved.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<StockChartPointEventArgs> PointMoved { get; set; }

        /// <summary>
        /// Triggers when the range is changed.
        /// </summary>
        [Parameter]
        public Action<StockChartRangeChangeEventArgs> RangeChange { get; set; }

        /// <summary>
        /// Triggers while click mouse on the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<StockChartMouseEventArgs> OnStockChartMouseClick { get; set; }

        /// <summary>
        /// Triggers while mouse down on the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<StockChartMouseEventArgs> OnStockChartMouseDown { get; set; }

        /// <summary>
        /// Triggers while mouse leave on the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<StockChartMouseEventArgs> OnStockChartMouseLeave { get; set; }

        /// <summary>
        /// Triggers while mouse move on the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<StockChartMouseEventArgs> OnStockChartMouseMove { get; set; }

        /// <summary>
        /// Triggers while mouse up on the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<StockChartMouseEventArgs> OnStockChartMouseUp { get; set; }

        /// <summary>
        /// Triggers when print operation is completed.
        /// </summary>
        [Parameter]
        public Action<PrintEventArgs> OnPrintComplete { get; set; }

        /// <summary>
        /// Triggers when the period is selected.
        /// </summary>
        [Parameter]
        public Action<StockChartPeriodChangedEventArgs> PeriodChanged { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.Events = this;
        }

        internal override void ComponentDispose()
        {
            StockChartInstance = null;
        }
    }
}
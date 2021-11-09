namespace Syncfusion.Blazor.Charts.Internal
{
    internal class Highlight : Selection
    {
        internal Highlight(SfChart chart)
            : base(chart, false)
        {
            chartInstance = chart;
            AddEventListener();
        }

        private SfChart chartInstance { get; set; }

        internal void PropertyChanged()
        {
            chartInstance.ShouldAnimateSeries = false;
        }

        private void AddEventListener()
        {
            chartInstance.MouseMove += MouseMoveHandler;
        }

        private void DeclarePrivateVariables()
        {
            StyleId = chartInstance.ID + "_ej2_chart_highlight";
            Unselected = chartInstance.ID + "_ej2_deselected";
            SelectedDataIndexes.Clear();
            HighlightDataIndexes.Clear();
            IsSeriesMode = chartInstance.HighlightMode == HighlightMode.Series;
        }

        internal void InvokeHighlight()
        {
            DeclarePrivateVariables();
            Series = chartInstance.SeriesContainer.Renderers;
            SeriesStyles();
            CurrentMode = (SelectionMode)chartInstance.HighlightMode;
        }

        internal new void Dispose()
        {
            base.Dispose();
            chartInstance = null;
        }
    }
}
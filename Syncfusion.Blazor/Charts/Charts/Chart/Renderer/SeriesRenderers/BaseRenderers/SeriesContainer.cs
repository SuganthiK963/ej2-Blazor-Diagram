using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartSeriesRendererContainer : ChartRendererContainer
    {
        private int seriesIndex;
        private string[] palette;
        private List<ChartSeriesType> seriesType = new List<ChartSeriesType>();
        private List<ChartDrawType> drawTypes = new List<ChartDrawType>();
        private ChartSeries paretoSeries;
        private int paretoLineSeriesRendererCount;

        internal List<IRequireAxis> ElementsRequiredAxis { get; set; } = new List<IRequireAxis>();

        internal IEnumerable<object> Data { get; set; } = new List<object>();

        internal List<Rect> DataLabelCollection { get; set; } = new List<Rect>();

        internal static List<ChartSeries> FindSeriesCollection(ChartColumnRenderer columnRenderer, ChartRowRenderer rowRenderer)
        {
            List<ChartSeries> seriesCollection = new List<ChartSeries>();
            foreach (ChartAxis rowAxis in rowRenderer.Axes)
            {
                foreach (ChartSeriesRenderer rowSeriesRenderer in rowAxis.Renderer.SeriesRenderer)
                {
                    foreach (ChartAxis axis in columnRenderer.Axes)
                    {
                        foreach (ChartSeriesRenderer columnseriesRenderer in axis.Renderer.SeriesRenderer)
                        {
                            if (columnseriesRenderer == rowSeriesRenderer && columnseriesRenderer.Series.Visible)
                            {
                                seriesCollection.Add(columnseriesRenderer.Series);
                            }
                        }
                    }
                }
            }

            return seriesCollection;
        }

        internal static List<ChartSeriesRenderer> FindAxisToSeriesCollection(ChartAxisRenderer x_axisRenderer, ChartAxisRenderer y_axisRenderer)
        {
            List<ChartSeriesRenderer> seriesCollection = new List<ChartSeriesRenderer>();
            foreach (ChartSeriesRenderer x_SeriesRenderer in x_axisRenderer.SeriesRenderer)
            {
                foreach (ChartSeriesRenderer y_SeriesRenderer in y_axisRenderer.SeriesRenderer)
                {
                    if (x_SeriesRenderer == y_SeriesRenderer)
                    {
                        seriesCollection.Add(x_SeriesRenderer);
                    }
                }
            }

            return seriesCollection;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.SeriesContainer = this;
            palette = Owner.Palettes.Length > 0 ? Owner.Palettes : ChartHelper.GetSeriesColor(Owner.Theme.ToString());
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Owner.SeriesContainer = this;
        }

        protected override void OnElementAdded(IChartElement element)
        {
            if (Owner.InitialRect != null)
            {
                StateHasChanged();
            }
        }

        protected override void OnElementRemoved(IChartElement element)
        {
            if (element != null)
            {
                RemoveRenderer((element as ChartSeries).Renderer);
                if (!Owner.ChartDisposed())
                {
                    StateHasChanged();
                }
            }
        }

        public override void AddRenderer(IChartElementRenderer renderer)
        {
            if (renderer != null && !Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Add(renderer);
                int index = Renderers.IndexOf(renderer);
                if (renderer.GetType().Equals(typeof(DefaultSeriesRenderer)))
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    OnRendererAdded(renderer, new ChartSeries());
#pragma warning restore CA2000 // Dispose objects before losing scope
                }
                else if (renderer.GetType().Equals(typeof(ParetoLineSeriesRenderer)))
                {
                    OnRendererAdded(renderer, paretoSeries);
                    paretoLineSeriesRendererCount++;
                }
                else
                {
                    OnRendererAdded(renderer, Elements[index - paretoLineSeriesRendererCount]);
                }
            }
        }

        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer is IRequireAxis)
            {
                ElementsRequiredAxis.Add(renderer as IRequireAxis);
            }

            ChartSeriesRenderer seriesRenderer = renderer as ChartSeriesRenderer;
            if (seriesRenderer != null)
            {
                seriesRenderer.Index = seriesIndex++;
                seriesRenderer.Series = element as ChartSeries;
                seriesRenderer.XAxisName = seriesRenderer.Series.XAxisName;
                seriesRenderer.YAxisName = seriesRenderer.Series.YAxisName;
                seriesRenderer.Interior = !string.IsNullOrEmpty(seriesRenderer.Series.Fill) ? seriesRenderer.Series.Fill : palette[seriesRenderer.Index % palette.Length];
                if (seriesRenderer.Series.Container == null)
                {
                    seriesRenderer.Series.Container = Owner;
                }
            }

            if (renderer != null && !renderer.GetType().Equals(typeof(DefaultSeriesRenderer)))
            {
                Owner.VisibleSeriesRenderers.Add(seriesRenderer);
            }
        }

        internal void InitparetoSeries(ChartSeries baseSeries)
        {
            if (paretoSeries == null)
            {
                paretoSeries = new ChartSeries();
                paretoSeries.Container = Owner;
                paretoSeries.SetParetoSeriesValues(baseSeries.DataSource);
                paretoSeries.RendererType = typeof(ParetoLineSeriesRenderer);
            }
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            DataLabelCollection.Clear();
            foreach (ChartRenderer renderer in Renderers)
            {
                renderer.HandleChartSizeChange(rect);
            }
        }

        public override void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in Renderers)
            {
                renderer.ProcessRenderQueue();
            }
        }

        protected override void OnRendererRemoved(IChartElementRenderer renderer)
        {
            Owner.VisibleSeriesRenderers.Remove(renderer as ChartSeriesRenderer);
            if (renderer is IRequireAxis)
            {
                ElementsRequiredAxis.Remove(renderer as IRequireAxis);
                seriesIndex--;
            }
        }

        internal void InitSeriesRendererFields()
        {
            foreach (ChartSeriesRenderer renderer in Renderers)
            {
                renderer.XAxisRenderer.Labels.Clear();
                renderer.InitSeriesRendererFields();
            }

            foreach (ChartSeriesRenderer renderer in Owner.VisibleSeriesRenderers)
            {
                renderer.InitSeriesRendererFields();
            }
        }

        internal void ProcessData()
        {
            seriesType.Clear();
            drawTypes.Clear();
            foreach (IChartElementRenderer renderer in Renderers)
            {
                var seriesRenderer = renderer as ChartSeriesRenderer;
                if (seriesRenderer != null && (seriesRenderer.Points.Count != seriesRenderer.Series.CurrentViewData.Count() || (seriesRenderer.Points.Count == 0 && Data.Any())))
                {
                    seriesRenderer.SetCurrentViewData(Data);
                    if (Owner.IsStockChart)
                    {
                        seriesRenderer.Points.Clear();
                    }
                    seriesRenderer.ProcessData();
                    seriesType.Add(seriesRenderer.Series.Type);
                    drawTypes.Add(seriesRenderer.Series.DrawType);
                }
            }

            CalculateStacking();
        }

        internal void CalculateStacking()
        {
            bool isCalculateStacking = false;
            if (seriesType.Count > 0 && (seriesType[0].ToString().Contains("Polar", StringComparison.InvariantCulture) || seriesType[0].ToString().Contains("Radar", StringComparison.InvariantCulture)) && !isCalculateStacking)
            {
                foreach (ChartDrawType type in drawTypes)
                {
                    if (type.ToString().Contains("Stacking", StringComparison.InvariantCulture) && !isCalculateStacking)
                    {
                        CalculateStackedValue(type.ToString().Contains("100", StringComparison.InvariantCulture));
                        isCalculateStacking = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (ChartSeriesType type in seriesType)
                {
                    if (type.ToString().Contains("Stacking", StringComparison.InvariantCulture) && !isCalculateStacking)
                    {
                        CalculateStackedValue(type.ToString().Contains("100", StringComparison.InvariantCulture));
                        isCalculateStacking = true;
                        break;
                    }
                }
            }
        }

        internal void UpdateStackingValues()
        {
            string type;
            foreach (ChartSeriesRenderer renderer in Renderers)
            {
                if (renderer.Series.Visible)
                {
                    type = (renderer.Series.Type == ChartSeriesType.Polar || renderer.Series.Type == ChartSeriesType.Radar) ? renderer.Series.DrawType.ToString() : renderer.Series.Type.ToString();
                    if (type.Contains("Stacking", StringComparison.InvariantCulture))
                    {
                        CalculateStackedValue(type.Contains("100", StringComparison.InvariantCulture));
                    }
                }
            }
        }

        internal void CalculateStackedValue(bool isStacking)
        {
            foreach (ChartColumnRenderer columnRenderer in Owner.ColumnContainer.Renderers)
            {
                foreach (ChartRowRenderer rowRenderer in Owner.RowContainer.Renderers)
                {
                    CalculateStackingValues(FindSeriesCollection(columnRenderer, rowRenderer), isStacking);
                }
            }
        }

        private static void FindPercentageOfStacking(List<ChartSeries> stackingSeries, List<double> values, bool isStacking100)
        {
            foreach (ChartSeries item in stackingSeries)
            {
                if (isStacking100)
                {
                    return;
                }

                foreach (Point point in ChartHelper.GetVisiblePoints(item.Renderer.Points))
                {
                    point.Percentage = Convert.ToDouble(Math.Abs(Convert.ToDouble(point.Y, null) / values[point.Index] * 100).ToString("N2", CultureInfo.InvariantCulture), null);
                }
            }
        }

        private static void CalculateStackingValues(List<ChartSeries> seriesCollection, bool isStacking100)
        {
            Dictionary<string, Dictionary<double, double>> lastPositive = new Dictionary<string, Dictionary<double, double>>();
            Dictionary<string, Dictionary<double, double>> lastNegative = new Dictionary<string, Dictionary<double, double>>();
            Dictionary<string, Dictionary<double, double>> frequencies = new Dictionary<string, Dictionary<double, double>>();
            double startMin, startMax, endMin, endMax;
            if (isStacking100)
            {
                frequencies = FindFrequencies(seriesCollection);
            }

            List<ChartSeries> stackingSeries = new List<ChartSeries>();
            List<double> stackedValues = new List<double>();
            foreach (ChartSeries series in seriesCollection)
            {
                if (series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture) || series.DrawType.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    string stackingGroup = GetStackingGroup(series);
                    if (!lastPositive.ContainsKey(stackingGroup))
                    {
                        lastPositive.Add(stackingGroup, null);
                    }

                    if (!lastNegative.ContainsKey(stackingGroup))
                    {
                        lastNegative.Add(stackingGroup, null);
                    }

                    if (lastPositive[stackingGroup] == null)
                    {
                        lastPositive[stackingGroup] = new Dictionary<double, double>();
                        lastNegative[stackingGroup] = new Dictionary<double, double>();
                    }

                    List<double> startValues = new List<double>();
                    List<double> endValues = new List<double>();
                    stackingSeries.Add(series);
                    List<Point> visiblePoints = ChartHelper.GetVisiblePoints(series.Renderer.Points);
                    for (int j = 0, pointsLength = visiblePoints.Count; j < pointsLength; j++)
                    {
                        double lastValue = 0,
                        y_Value = double.IsNaN(series.Renderer.YData[j]) ? 0 : series.Renderer.YData[j],
                        pos = visiblePoints[j].XValue;
                        if (!lastPositive[stackingGroup].ContainsKey(pos))
                        {
                            lastPositive[stackingGroup].Add(pos, 0);
                        }

                        if (!lastNegative[stackingGroup].ContainsKey(pos))
                        {
                            lastNegative[stackingGroup].Add(pos, 0);
                        }

                        if (isStacking100)
                        {
                            y_Value = y_Value / frequencies[stackingGroup][pos] * 100;
                            y_Value = !double.IsNaN(y_Value) ? y_Value : 0;
                            visiblePoints[j].Percentage = Convert.ToDouble(y_Value.ToString("N2", CultureInfo.InvariantCulture), null);
                        }
                        else
                        {
                            if (stackedValues.Count == j)
                            {
                                stackedValues.Add(Math.Abs(y_Value));
                            }
                            else
                            {
                                stackedValues[j] = stackedValues[j] + Math.Abs(y_Value);
                            }
                        }

                        if (y_Value >= 0)
                        {
                            lastValue = lastPositive[stackingGroup][pos];
                            lastPositive[stackingGroup][pos] += y_Value;
                        }
                        else
                        {
                            lastValue = lastNegative[stackingGroup][pos];
                            lastNegative[stackingGroup][pos] += y_Value;
                        }

                        startValues.Add(lastValue);
                        endValues.Add(y_Value + lastValue);
                        if (isStacking100 && (endValues[j] > 100))
                        {
                            endValues[j] = 100;
                        }
                    }

                    series.Renderer.StackedValues = new StackValues(startValues, endValues);
                    startMin = startValues.Count > 0 ? startValues.Min() : 0;
                    startMax = startValues.Count > 0 ? startValues.Max() : 0;
                    endMin = endValues.Count > 0 ? endValues.Min() : 0;
                    endMax = endValues.Count > 0 ? endValues.Max() : 0;
                    series.Renderer.YMin = startMin;
                    series.Renderer.YMax = endMax;
                    if (series.Renderer.YMin > endMin)
                    {
                        series.Renderer.YMin = isStacking100 ? -100 : endMin;
                    }

                    if (series.Renderer.YMax < startMax)
                    {
                        series.Renderer.YMax = 0;
                    }
                }
            }

            FindPercentageOfStacking(stackingSeries, stackedValues, isStacking100);
        }

        protected static string GetStackingGroup(ChartSeries series)
        {
            return series != null && series.Type.ToString().Contains("StackingArea", StringComparison.InvariantCulture) ? "StackingArea100" : series != null && series.Type.ToString().Contains("StackingLine", StringComparison.InvariantCulture) ? "StackingLine100" : series?.StackingGroup;
        }

        private static Dictionary<string, Dictionary<double, double>> FindFrequencies(List<ChartSeries> seriesCollection)
        {
            Dictionary<string, Dictionary<double, double>> frequencies = new Dictionary<string, Dictionary<double, double>>();
            foreach (ChartSeries series in seriesCollection)
            {
                series.Renderer.YAxisRenderer.IsStack100 = series.Type.ToString().Contains("100", StringComparison.InvariantCulture) ? true : false;
                List<Point> visiblePoints = ChartHelper.GetVisiblePoints(series.Renderer.Points);
                if (series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    string stackingGroup = GetStackingGroup(series);
                    if (!frequencies.ContainsKey(stackingGroup))
                    {
                        frequencies.Add(stackingGroup, null);
                    }

                    if (frequencies[stackingGroup] == null)
                    {
                        frequencies[stackingGroup] = new Dictionary<double, double>();
                    }

                    for (int j = 0, pointsLength = visiblePoints.Count; j < pointsLength; j++)
                    {
                        if (!frequencies[stackingGroup].ContainsKey(visiblePoints[j].XValue))
                        {
                            frequencies[stackingGroup].Add(visiblePoints[j].XValue, 0);
                        }

                        if (series.Renderer.YData[j] > 0)
                        {
                            frequencies[stackingGroup][visiblePoints[j].XValue] += series.Renderer.YData[j];
                        }
                        else
                        {
                            frequencies[stackingGroup][visiblePoints[j].XValue] -= series.Renderer.YData[j];
                        }
                    }
                }
            }

            return frequencies;
        }

        public ChartAreaType GetAreaType()
        {
            ChartSeriesRenderer renderer = Renderers.Count > 0 ? Renderers.First() as ChartSeriesRenderer : null;
            if (renderer != null)
            {
                ChartSeriesType type = renderer.Series.Type;
                if (type == ChartSeriesType.Polar || type == ChartSeriesType.Radar)
                {
                    return ChartAreaType.PolarAxes;
                }
                else
                {
                    return ChartAreaType.CartesianAxes;
                }
            }

            return ChartAreaType.CartesianAxes;
        }

        internal void PerformAnimation(List<InitialAnimationInfo> animationInfo)
        {
            foreach (ChartSeriesRenderer renderer in Renderers)
            {
                ChartSeries series = renderer.Series;
                if (series.Visible && series.Animation.Enable)
                {
                    renderer.PerformInitialAnimation(animationInfo);
                }
            }
        }

        private void CreateSeriesElements(RenderTreeBuilder builder, IChartElement element)
        {
            int seq = 0;
            ChartSeries series = element as ChartSeries;
            builder.OpenComponent(seq++, element.RendererType);
            builder.SetKey(element.RendererKey + "_Renderer");
            builder.CloseComponent();
            if (series.Type == ChartSeriesType.Pareto)
            {
                InitparetoSeries(series);
                builder.OpenComponent(seq++, typeof(ParetoLineSeriesRenderer));
                builder.CloseComponent();
                builder.OpenComponent(seq++, typeof(ParetoAxisRenderer));
                builder.CloseComponent();
            }
        }

        private void CreateSeriesNestedElements(RenderTreeBuilder builder, IChartElement element)
        {
            int seq = 0;
            ChartSeries series = element as ChartSeries;
            if (series.Marker.RendererType != null)
            {
                builder.OpenComponent(seq++, series.Marker.RendererType);
                builder.AddAttribute(seq++, "Series", series);
                builder.SetKey(element.RendererKey + "_MarkerRenderer");
                builder.CloseComponent();
                if (series.Type == ChartSeriesType.Pareto)
                {
                    paretoSeries.Marker.SetMarkerValues(series.Marker);
                    builder.OpenComponent(seq++, series.Marker.RendererType);
                    builder.AddAttribute(seq++, "Series", paretoSeries);
                    builder.CloseComponent();
                }

                if (series.Marker.DataLabel.RendererType != null)
                {
                    builder.OpenComponent(seq++, series.Marker.DataLabel.RendererType);
                    builder.AddAttribute(seq++, "Series", series);
                    builder.SetKey(element.RendererKey + "_LabelRenderer");
                    builder.CloseComponent();
                    if (series.Type == ChartSeriesType.Pareto)
                    {
                        paretoSeries.Marker.DataLabel.SetDataLableValues(series.Marker.DataLabel);
                        builder.OpenComponent(seq++, series.Marker.DataLabel.RendererType);
                        builder.AddAttribute(seq++, "Series", paretoSeries);
                        builder.CloseComponent();
                    }
                }
            }

            if (series.ErrorBar.RendererType != null)
            {
                builder.OpenComponent(seq++, series.ErrorBar.RendererType);
                builder.AddAttribute(seq++, "Series", series);
                builder.CloseComponent();
            }
        }

        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            int seq = 0;
            paretoLineSeriesRendererCount = 0;
            if (Elements.Any(element => (element as ChartSeries).NeedRendererRemove))
            {
                foreach (ChartSeries element in Elements)
                {
                    if (element.NeedRendererRemove)
                    {
                        element.NeedRendererRemove = false;
                        element.UpdateDataSource = false;
                        RemoveRenderer(element.Renderer);
                    }

                    CreateSeriesElements(builder, element);
                }

                foreach (ChartSeries element in Elements)
                {
                    CreateSeriesNestedElements(builder, element);
                }
            }
            else
            {
                if (ContainerUpdate && Elements.Count == 0)
                {
                    builder.OpenComponent(seq++, typeof(DefaultSeriesRenderer));
                    builder.CloseComponent();
                }
                else
                {
                    SortSeriesByZOrder();
                    foreach (IChartElement element in Elements)
                    {
                        if (element.RendererType == null)
                        {
                            continue;
                        }

                        CreateSeriesElements(builder, element);
                    }

                    foreach (ChartSeries element in Elements)
                    {
                        if (element.RendererType == null)
                        {
                            continue;
                        }

                        CreateSeriesNestedElements(builder, element);
                    }
                }
            }
        }

        internal void OnThemeChanged()
        {
            palette = Owner.Palettes.Length > 0 ? Owner.Palettes : ChartHelper.GetSeriesColor(Owner.Theme.ToString());
            foreach (ChartSeriesRenderer renderer in Renderers)
            {
                renderer.Interior = !string.IsNullOrEmpty(renderer.Series.Fill) ? renderer.Series.Fill : palette[renderer.Index % palette.Length];
                renderer.UpdateCustomization("Fill");
                renderer.ProcessRenderQueue();
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.Internal
{
    public abstract class ChartSeriesRenderer : ChartRenderer, IChartElementRenderer, IRequireAxis
    {
        private Rect markerClipRect;

        private Rect errorBarClipRect;

        [CascadingParameter]
        internal ChartRendererContainer Container { get; set; }

        protected double XLength { get; set; }

        protected double YLength { get; set; }

        internal IEnumerable<object> CurrentViewData { get; set; }

        protected AnimationOptions AnimationOptions { get; set; }

        protected CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        internal Rect ClipRect { get; set; }

        internal ChartSeries Series { get; set; }

        internal List<Point> Points { get; set; }

        internal DynamicAnimation DynamicOptions { get; set; } = new DynamicAnimation();

        internal List<string> SeriesTemplateID { get; set; } = new List<string>();

        internal string ErrorBarId { get; set; }

        internal string ErrorBarClipRectId { get; set; }

        internal int Index { get; set; }

        internal string Interior { get; set; }

        internal StackValues StackedValues { get; set; }

        internal double Position { get; set; } = double.NaN;

        internal double RectCount { get; set; } = double.NaN;

        internal HistogramValues HistogramValues { get; set; } = new HistogramValues();

        internal ElementReference SeriesElement { get; set; }

        internal ElementReference ErrorBarElement { get; set; }

        internal ElementReference SymbolElement { get; set; }

        internal ElementReference TextElement { get; set; }

        internal ElementReference ShapeElement { get; set; }

        internal int SourceIndex { get; set; }

        internal bool TrendLineLegendVisibility { get; set; } = true;

        internal bool IsParetoLine { get; set; }

        internal bool IsSeriesRender { get; set; }

        #region IREQUIREAXIS INTERFACE IMPLEMENTED MEMBERS
        public ChartAxisRenderer XAxisRenderer { get; set; }

        public ChartAxisRenderer YAxisRenderer { get; set; }

        public string XAxisName { get; set; }

        public string YAxisName { get; set; }

        public DoubleRange XRange { get; set; }

        public DoubleRange YRange { get; set; }

        public bool IsVisible { get; set; }

        public List<double> XData { get; set; }

        public List<double> YData { get; set; }

        public double XMin { get; set; }

        public double XMax { get; set; }

        public double YMin { get; set; }

        public double YMax { get; set; }

        public double MaxSize { get; set; }

        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitSeriesRendererFields();
            Owner.SeriesContainer.AddRenderer(this);
            UpdateCurrentViewData(Series.CurrentViewData);
            Series.Renderer = this;
            SvgRenderer = Owner.SvgRenderer;
            InitDynamicAnimationProperty();
            if (Owner.InitialRect != null && Series.NeedRendererUpdate)
            {
                Series.NeedRendererUpdate = false;
                Owner.AxisContainer.AssignAxisToSeries(Owner.SeriesContainer.ElementsRequiredAxis);
                XAxisRenderer = Owner.AxisContainer.Axes[Series.XAxisName].Renderer;
                YAxisRenderer = Owner.AxisContainer.Axes[Series.YAxisName].Renderer;
                YAxisRenderer.IsStack100 = Series.Type.ToString().Contains("100", StringComparison.InvariantCulture);
                if ((XAxisRenderer.Axis.ValueType == ValueType.Category || XAxisRenderer.Axis.ValueType == ValueType.DateTimeCategory) && XAxisRenderer.Axis.IsIndexed)
                {
                    UpdateCategoryData();
                }
                else
                {
                    ProcessData();
                }

                if (Series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture) || Series.DrawType.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    Owner.SeriesContainer.CalculateStackedValue(Series.Type.ToString().Contains("100", StringComparison.InvariantCulture));
                }

                Owner.DelayLayoutChange();
            }
            else if (Owner.InitialRect != null)
            {
                Owner.ProcessOnLayoutChange();
            }
        }

        internal void UpdateCurrentViewData(IEnumerable<object> currentViewData)
        {
            CurrentViewData = currentViewData;
            if (!Series.CurrentViewData.Any())
            {
                Series.CurrentViewData = currentViewData;
            }
        }

        internal void SetCurrentViewData(IEnumerable<object> currentViewData)
        {
            if (!CurrentViewData.Any())
            {
                UpdateCurrentViewData(currentViewData);
            }
        }

        internal async Task UpdateSeriesData(bool isRemoteData = false)
        {
            if (!isRemoteData)
            {
                UpdateCurrentViewData(await Series.UpdateSeriesData());
            }

            if (IsCategoryAxis())
            {
                UpdateCategoryData();
            }
            else
            {
                InitSeriesRendererFields();
                ProcessData();
            }

            if (Series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture) || Series.DrawType.ToString().Contains("Stacking", StringComparison.InvariantCulture))
            {
                Owner.SeriesContainer.CalculateStackedValue(Series.Type.ToString().Contains("100", StringComparison.InvariantCulture) || Series.DrawType.ToString().Contains("100", StringComparison.InvariantCulture));
            }

            UpdateTrendlineDataSource();
            UpdateIndicatorDataSource();

            if (XAxisRenderer.IsFixedRange() && YAxisRenderer.IsFixedRange())
            {
                ValidateUpdateDirection();
                UpdateTrendlineDirection();
                UpdateIndicatorAxis();
                UpdateIndicatorDirection();
            }
            else
            {
                if (!YAxisRenderer.IsFixedRange() && YAxisRenderer.NeedAxisLayoutChange(YMin, YMax))
                {
                    Owner.OnLayoutChange();
                }
                else
                {
                    bool isRangeChanged = false;
                    if (XAxisRenderer is DateTimeCategoryAxisRenderer && !XAxisRenderer.IsFixedRange() && XAxisRenderer.NeedAxisLayoutChange(XMin, XMax))
                    {
                        Owner.OnLayoutChange();
                    }
                    else if ((!XAxisRenderer.IsFixedRange() && XMin < XAxisRenderer.ActualRange.Start) || XMax > XAxisRenderer.ActualRange.End)
                    {
                        XAxisRenderer.ChangeAxisRange();
                        ValidateUpdateDirection();
                        isRangeChanged = true;
                    }

                    if ((!YAxisRenderer.IsFixedRange() && YMin < YAxisRenderer.ActualRange.Start) || YMax > YAxisRenderer.ActualRange.End)
                    {
                        YAxisRenderer.ChangeAxisRange();
                        ValidateUpdateDirection();
                        isRangeChanged = true;
                    }

                    if (!isRangeChanged)
                    {
                        ValidateUpdateDirection();
                    }

                    UpdateTrendlineDirection();
                    UpdateIndicatorAxis();
                    UpdateIndicatorDirection();
                }
            }
        }

        private void UpdateTrendlineDataSource()
        {
            foreach (ChartTrendline trendline in Series.Trendlines)
            {
                trendline.TrendlineInitiator.InitDataSource();
            }
        }

        private void UpdateIndicatorDataSource()
        {
            foreach (ChartIndicator indicator in Owner.IndicatorContainer.Elements)
            {
                indicator.IndicatorRenderer.InitDataSource();
            }
        }

        private void UpdateTrendlineDirection()
        {
            foreach (ChartTrendline trendline in Series.Trendlines)
            {
                trendline.Renderer.UpdateDirection();
            }
        }

        private void UpdateIndicatorDirection()
        {
            foreach (ChartIndicator indicator in Owner.IndicatorContainer.Elements)
            {
                foreach(ChartSeries targetSeries in indicator.TargetSeries)
                {
                    targetSeries.Renderer.UpdateDirection();
                }                
            }
        }

        private void UpdateIndicatorAxis()
        {
            foreach (ChartIndicator indicator in Owner.IndicatorContainer.Elements)
            {
                foreach (ChartSeries targetSeries in indicator.TargetSeries)
                {               
                    if ((!targetSeries.Renderer.YAxisRenderer.IsFixedRange() && targetSeries.Renderer.YMin < targetSeries.Renderer.YAxisRenderer.ActualRange.Start) || targetSeries.Renderer.YMax > targetSeries.Renderer.YAxisRenderer.ActualRange.End)
                    {
                        targetSeries.Renderer.YAxisRenderer.ChangeAxisRange();
                    }
                }
            }
        }

        private void ValidateUpdateDirection()
        {
            if (Series.Marker.DataLabel.Visible && IsCategoryAxis())
            {
                Series.UpdateSeriesCollection();
            }
            else
            {
                UpdateDirection();
            }
        }

        internal bool IsCategoryAxis()
        {
            return XAxisRenderer.Axis.ValueType == ValueType.Category || XAxisRenderer.Axis.ValueType == ValueType.DateTimeCategory;
        }

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
                            if (columnseriesRenderer == rowSeriesRenderer && columnseriesRenderer.Series.Visible && columnseriesRenderer.IsRectSeries())
                            {
                                seriesCollection.Add(columnseriesRenderer.Series);
                            }
                        }
                    }
                }
            }

            return seriesCollection;
        }

        internal override void OnParentParameterSet()
        {
            base.OnParentParameterSet();
        }

        void IChartElementRenderer.HandleLayoutChange()
        {
            HandleLayoutChange();
        }

        protected void HandleLayoutChange()
        {
            OnLayoutChange();
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            SeriesRenderer();
            Series.Marker.Renderer?.HandleChartSizeChange(rect);
            Series.ErrorBar.Renderer?.HandleChartSizeChange(rect);
            Series.Marker.DataLabel.Renderer?.HandleChartSizeChange(rect);
        }

        protected void GetAxisLength()
        {
            bool isInverted = Series.Container.RequireInvertedAxis;
            XLength = isInverted ? XAxisRenderer.Rect.Height : XAxisRenderer.Rect.Width;
            YLength = isInverted ? YAxisRenderer.Rect.Width : YAxisRenderer.Rect.Height;
        }

        protected void SeriesRenderer()
        {
            GetAxisLength();
            RenderSeries();
        }

        protected virtual void RenderSeries()
        {
            RendererShouldRender = true;
            FindClipRect();
        }

        public virtual string SetPointColor(Point point, string color)
        {
            color = !string.IsNullOrEmpty(point?.Interior) && !string.IsNullOrEmpty(Series.PointColorMapping) ? point.Interior : color;
            return point.IsEmpty ? (Series.EmptyPointSettings.Fill != null ? Series.EmptyPointSettings.Fill : color) : color;
        }

        internal ChartEventBorder SetBorderColor(Point point, ChartEventBorder border)
        {
            border.Width = point.IsEmpty ? Series.EmptyPointSettings.Border.Width != 0 ? Series.EmptyPointSettings.Border.Width : border.Width : border.Width;
            border.Color = point.IsEmpty ? Series.EmptyPointSettings.Border.Color ?? border.Color : border.Color;
            return border;
        }

        internal virtual void UpdateDirection()
        {
            RendererShouldRender = true;
            InvokeAsync(StateHasChanged);
            if (Series.Type != ChartSeriesType.Scatter && Series.Type != ChartSeriesType.Bubble && Series.DrawType != ChartDrawType.Scatter)
            {
                Series.Marker.Renderer?.UpdateDirection();
            }

            if ((Series.Type != ChartSeriesType.Polar || Series.Type != ChartSeriesType.Radar) && !Series.DrawType.ToString().Contains("Column", StringComparison.InvariantCulture))
            {
                if (Series.UpdateDataSource && Series.Marker.DataLabel.Renderer != null)
                {
                    List<ChartSeriesRenderer> seriesCollection = ChartSeriesRendererContainer.FindAxisToSeriesCollection(XAxisRenderer, YAxisRenderer);
                    Owner.SeriesContainer.DataLabelCollection.Clear();
                    foreach (ChartSeriesRenderer seriesRenderer in seriesCollection)
                    {
                        seriesRenderer.Series.Marker.DataLabel.Renderer?.DatalabelValueChanged();
                    }
                }
                else
                {
                    Series.Marker.DataLabel.Renderer?.DatalabelValueChanged();
                }
            }
        }

        internal virtual void UpdateCustomization(string property)
        {
            RendererShouldRender = Series.Visible;
        }

        protected void InitDynamicAnimationProperty()
        {
            DynamicOptions.PathId = new List<string>();
            DynamicOptions.CurrentDirection = new List<string>();
            DynamicOptions.MarkerParentId = string.Empty;
            DynamicOptions.IsCircle = false;
            DynamicOptions.MarkerSymbolId = new List<string>();
            DynamicOptions.DataLabelParentId = string.Empty;
            DynamicOptions.DataLabelTextId = new List<string>();
            DynamicOptions.ErrorShapeId = new List<string>();
            DynamicOptions.ErrorShapeCDir = new List<string>();
            DynamicOptions.ErrorCapId = new List<string>();
            DynamicOptions.ErrorCapCDir = new List<string>();
        }

        protected void InvalidateState()
        {
            Container.AddToRenderQueue(this);
        }

        internal static Type GetRendererType(ChartSeriesType type, ChartDrawType drawType)
        {
            switch (type)
            {
                case ChartSeriesType.Line:
                    return typeof(LineSeriesRenderer);

                case ChartSeriesType.Spline:
                    return typeof(SplineSeriesRenderer);

                case ChartSeriesType.Area:
                    return typeof(AreaSeriesRenderer);

                case ChartSeriesType.SplineArea:
                    return typeof(SplineAreaSeriesRenderer);

                case ChartSeriesType.StepLine:
                    return typeof(StepLineSeriesRenderer);

                case ChartSeriesType.StepArea:
                    return typeof(StepAreaSeriesRenderer);

                case ChartSeriesType.Column:
                    return typeof(ColumnSeriesRenderer);

                case ChartSeriesType.Bar:
                    return typeof(BarSeriesRenderer);

                case ChartSeriesType.RangeArea:
                    return typeof(RangeAreaSeriesRenderer);

                case ChartSeriesType.RangeColumn:
                    return typeof(RangeColumnSeriesRenderer);

                case ChartSeriesType.StackingColumn:
                    return typeof(StackingColumnSeriesRenderer);

                case ChartSeriesType.StackingColumn100:
                    return typeof(StackingColumn100SeriesRenderer);

                case ChartSeriesType.StackingBar:
                    return typeof(StackingBarSeriesRenderer);

                case ChartSeriesType.StackingBar100:
                    return typeof(StackingBar100SeriesRenderer);

                case ChartSeriesType.StackingLine:
                    return typeof(StackingLineSeriesRenderer);

                case ChartSeriesType.StackingLine100:
                    return typeof(StackingLine100SeriesRenderer);

                case ChartSeriesType.StackingArea:
                    return typeof(StackingAreaSeriesRenderer);

                case ChartSeriesType.StackingArea100:
                    return typeof(StackingArea100SeriesRenderer);

                case ChartSeriesType.StackingStepArea:
                    return typeof(StackingStepAreaSeriesRenderer);

                case ChartSeriesType.Bubble:
                    return typeof(BubbleSeriesRenderer);

                case ChartSeriesType.MultiColoredLine:
                    return typeof(MultiColoredLineSeriesRenderer);

                case ChartSeriesType.MultiColoredArea:
                    return typeof(MultiColoredAreaSeriesRenderer);

                case ChartSeriesType.Polar:
                case ChartSeriesType.Radar:
                    return GetPolarRadarRendererType(drawType);

                case ChartSeriesType.Hilo:
                    return typeof(HiloSeriesRenderer);
                case ChartSeriesType.HiloOpenClose:
                    return typeof(HiloOpenCloseSeriesRenderer);
                case ChartSeriesType.Candle:
                    return typeof(CandleSeriesRenderer);
                case ChartSeriesType.Scatter:
                    return typeof(ScatterSeriesRenderer);
                case ChartSeriesType.Histogram:
                    return typeof(HistogramSeriesRenderer);

                case ChartSeriesType.Waterfall:
                    return typeof(WaterfallSeriesRenderer);

                case ChartSeriesType.BoxAndWhisker:
                    return typeof(BoxAndWhiskerSeriesRenderer);

                case ChartSeriesType.Pareto:
                    return typeof(ParetoSeriesRenderer);
            }

            throw new NotImplementedException("Specified chart type is not implemented");
        }

        private static Type GetPolarRadarRendererType(ChartDrawType type)
        {
            switch (type)
            {
                case ChartDrawType.Line:
                    return typeof(PolarLineSeriesRenderer);
                case ChartDrawType.Spline:
                    return typeof(PolarSplineSeriesRenderer);
                case ChartDrawType.SplineArea:
                    return typeof(PolarSplineAreaSeriesRenderer);
                case ChartDrawType.Area:
                    return typeof(PolarAreaSeriesRenderer);
                case ChartDrawType.Column:
                    return typeof(PolarColumnSeriesRenderer);
                case ChartDrawType.StackingLine:
                    return typeof(PolarStackinglineSeriesRenderer);
                case ChartDrawType.StackingArea:
                    return typeof(PolarStackingAreaSeriesRenderer);
                case ChartDrawType.StackingColumn:
                    return typeof(PolarColumnSeriesRenderer);
                case ChartDrawType.RangeColumn:
                    return typeof(PolarRangeColumnSeriesRenderer);
                case ChartDrawType.Scatter:
                    return typeof(PolarScatterSeriesRenderer);
            }

            throw new NotImplementedException("Specified chart type is not implemented");
        }

        void IChartElementRenderer.InvalidateRender()
        {
            InvalidateRender();
        }

        protected void InvalidateRender()
        {
            StateHasChanged();
        }

        public void OnAxisChanged()
        {
        }

        protected virtual void OnLayoutChange()
        {
            ProcessRenderQueue();
        }

        public override void ProcessRenderQueue()
        {
            base.ProcessRenderQueue();
            Series.Marker.Renderer?.ProcessRenderQueue();
            Series.ErrorBar.Renderer?.ProcessRenderQueue();
            Series.Marker.DataLabel.Renderer?.ProcessRenderQueue();
        }

        internal virtual void InitSeriesRendererFields()
        {
            Points = new List<Point>();
            XData = new List<double>();
            YData = new List<double>();
            XMin = double.PositiveInfinity;
            XMax = double.NegativeInfinity;
            YMin = double.PositiveInfinity;
            YMax = double.NegativeInfinity;
        }

        // TODO: Need to overrider for trendline and indicators
        internal virtual string SeriesElementId()
        {
            return Owner.ID + "SeriesGroup" + Index;
        }

        internal virtual void ProcessData()
        {
            SeriesRenderEventArgs eventArgs = new SeriesRenderEventArgs("OnSeriesRender", false, Interior, Series.CurrentViewData, Series);
            SfChart.InvokeEvent(Owner.ChartEvents?.OnSeriesRender, eventArgs);
            CurrentViewData = eventArgs.Data;
            Interior = eventArgs.Fill;
            int len = CurrentViewData.Count();
            if (len == 0)
            {
                return;
            }

            XData = new double[len].ToList();
            Type firstDataType = CurrentViewData.First().GetType();
            string x_Name = Series.XName;
            string y_Name = Series.YName;
            string dataType = DataVizCommonHelper.FindDataType(firstDataType);
            switch (dataType)
            {
                case Constants.JOBJECT:
                    ProcessJObjectData(firstDataType, x_Name, y_Name, CurrentViewData);
                    break;
                case Constants.EXPANDOOBJECT:
                    ProcessExpandoObjectData(firstDataType, x_Name, y_Name, CurrentViewData);
                    break;
                case Constants.DYNAMICOBJECT:
                    ProcessDynamicObjectData(firstDataType, x_Name, y_Name, CurrentViewData);
                    break;
                default:
                    ProcessObjectData(firstDataType, x_Name, y_Name, CurrentViewData);
                    break;
            }

            FindSplinePoint();
        }

        protected virtual void ProcessExpandoObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string pointColor = Series.PointColorMapping;
            int index = 0;
            Point point;
            if (currentViewData != null)
            {
                foreach (object data in currentViewData)
                {
                    IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                    object x, y, color;
                    expandoData.TryGetValue(x_Name, out x);
                    expandoData.TryGetValue(y_Name, out y);
                    expandoData.TryGetValue(pointColor, out color);
                    point = new Point()
                    {
                        X = x,
                        Y = y,
                        Interior = Convert.ToString(color, CultureInfo.InvariantCulture),
                        Text = Convert.ToString(GetTextMapping(), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        protected virtual void ProcessDynamicObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string pointColor = Series.PointColorMapping, tooltip = Series.TooltipMappingName;
            int index = 0;
            Point point;
            if (currentViewData != null)
            {
                foreach (object data in currentViewData)
                {
                    point = new Point()
                    {
                        X = ChartHelper.GetDynamicMember(data, x_Name),
                        Y = ChartHelper.GetDynamicMember(data, y_Name),
                        Interior = Convert.ToString(ChartHelper.GetDynamicMember(data, pointColor), CultureInfo.InvariantCulture),
                        Text = Convert.ToString(ChartHelper.GetDynamicMember(data, GetTextMapping()), CultureInfo.InvariantCulture),
                        Tooltip = Convert.ToString(ChartHelper.GetDynamicMember(data, tooltip), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        protected virtual void ProcessJObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string pointColor = Series.PointColorMapping, tooltip = Series.TooltipMappingName;
            int index = 0;
            Point point;
            if (currentViewData != null)
            {
                foreach (object data in currentViewData)
                {
                    JObject jsonObject = (JObject)data;
                    point = new Point()
                    {
                        X = jsonObject.GetValue(x_Name, StringComparison.Ordinal),
                        Y = jsonObject.GetValue(y_Name, StringComparison.Ordinal),
                        Interior = Convert.ToString(jsonObject.GetValue(pointColor, StringComparison.Ordinal), CultureInfo.InvariantCulture),
                        Text = Convert.ToString(jsonObject.GetValue(GetTextMapping(), StringComparison.Ordinal), CultureInfo.InvariantCulture),
                        Tooltip = Convert.ToString(jsonObject.GetValue(tooltip, StringComparison.Ordinal), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        protected virtual void ProcessObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            if (firstDataType != null && currentViewData != null)
            {
                PropertyAccessor x = new PropertyAccessor(firstDataType.GetProperty(x_Name));
                PropertyAccessor y = new PropertyAccessor(firstDataType.GetProperty(y_Name));
                PropertyAccessor pointColor = new PropertyAccessor(firstDataType.GetProperty(Series.PointColorMapping));
                PropertyAccessor textMapping = new PropertyAccessor(firstDataType.GetProperty(GetTextMapping()));
                PropertyAccessor tooltipMapping = new PropertyAccessor(firstDataType.GetProperty(Series.TooltipMappingName));
                int index = 0;
                Point point;
                foreach (object data in currentViewData)
                {
                    point = new Point()
                    {
                        X = x.GetValue(data),
                        Y = y.GetValue(data),
                        Interior = Convert.ToString(pointColor.GetValue(data), CultureInfo.InvariantCulture),
                        Text = Convert.ToString(textMapping.GetValue(data), CultureInfo.InvariantCulture),
                        Tooltip = Convert.ToString(tooltipMapping.GetValue(data), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        internal virtual void GetSetXValue(Point point, int index)
        {
            if (XAxisRenderer.Axis.ValueType == ValueType.Category)
            {
                PushCategoryData(point, index, point.X.ToString());
            }
            else if (XAxisRenderer.Axis.ValueType == ValueType.DateTime || XAxisRenderer.Axis.ValueType == ValueType.DateTimeCategory)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(point.X, CultureInfo.InvariantCulture)))
                {
                    if (XAxisRenderer.Axis.ValueType == ValueType.DateTime)
                    {
                        point.XValue = ChartHelper.GetTime(Convert.ToDateTime(point.X, null));
                    }
                    else
                    {
                        PushCategoryData(point, index, ChartHelper.GetTime(Convert.ToDateTime(point.X, null)).ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    point.Visible = false;
                    point.X = null;
                    point.XValue = double.NaN;
                }
            }
            else
            {
                point.XValue = Convert.ToDouble(point.X, null);
            }

            PushData(point, index);
            Points.Add(point);
        }

        internal virtual void FindSplinePoint()
        {
        }

        internal virtual string GetTextMapping()
        {
            return Series?.Marker?.DataLabel?.Name ?? string.Empty;
        }

        protected virtual void PushData(Point point, int i)
        {
            if (XData.Count < i || point == null)
            {
                return;
            }

            point.Index = i;
            if (Series.Type != ChartSeriesType.BoxAndWhisker)
            {
                point.YValue = (point.Y != null) ? Convert.ToDouble(point.Y, null) : 0;
            }
            else
            {
                BoxPoint boxPoint = point as BoxPoint;
                boxPoint.YValueCollection = (boxPoint.Y != null) ? (double[])boxPoint.Y : null;
            }

            XData[i] = point.XValue;
        }

        internal virtual bool FindVisibility(Point point)
        {
            if (point == null)
            {
                return false;
            }

            SetXYMinMax(point.XValue, point.YValue);
            YData.Add(point.YValue);
            return point.X == null || (point.Y == null) ? true : double.IsNaN(Convert.ToDouble(point.Y, null));
        }

        protected virtual void SetXYMinMax(double xvalue, double yvalue)
        {
            bool isLogAxis = YAxisRenderer.Axis.ValueType == ValueType.Logarithmic || XAxisRenderer.Axis.ValueType == ValueType.Logarithmic;
            bool isRectSeries = Series.Type.ToString().Contains("Column", StringComparison.InvariantCulture) || Series.Type.ToString().Contains("Bar", StringComparison.InvariantCulture);
            double ymin = (isLogAxis && isRectSeries && !ChartHelper.SetRange(YAxisRenderer.Axis)) ? 1 : yvalue;
            XMin = double.IsNaN(xvalue) ? XMin : Math.Min(XMin, xvalue);
            XMax = double.IsNaN(xvalue) ? XMax : Math.Max(XMax, xvalue);
            YMin = isLogAxis ? Math.Min(YMin, (double.IsNaN(ymin) || (ymin == 0)) ? YMin : ymin) :
                Math.Min(YMin, double.IsNaN(ymin) ? YMin : ymin);
            YMax = Math.Max(YMax, double.IsNaN(yvalue) ? YMax : yvalue);
        }

        internal void UpdateEmptyPoint()
        {
            RendererShouldRender = Series.Visible;
            if (RendererShouldRender)
            {
                InitSeriesRendererFields();
                ProcessData();
                if (Series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture) || Series.DrawType.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    Owner.SeriesContainer.CalculateStackedValue(Series.Type.ToString().Contains("100", StringComparison.InvariantCulture) || Series.DrawType.ToString().Contains("100", StringComparison.InvariantCulture));
                }

                Series.UpdateSeriesCollection();
            }
        }

        // TODO: Empty point settings need to split and override other series renderer
        internal virtual void SetEmptyPoint(Point point, int i, Type type)
        {
            if (!FindVisibility(point))
            {
                point.Visible = true;
                return;
            }

            if (Series.Type == ChartSeriesType.BoxAndWhisker)
            {
                return;
            }

            point.IsEmpty = true;
            switch (GetEmptyPointMode(Series.EmptyPointSettings.Mode))
            {
                case EmptyPointMode.Zero:
                    point.Visible = true;
                    point.Y = point.YValue = YData[i] = 0;
                    break;
                case EmptyPointMode.Average:
                    CalculateAverageValue(point, i, type);
                    break;
                case EmptyPointMode.Drop:
                case EmptyPointMode.Gap:
                    YData[i] = double.NaN;
                    point.Visible = false;
                    break;
            }
        }

        internal virtual EmptyPointMode GetEmptyPointMode(EmptyPointMode mode)
        {
            return mode;
        }

        internal virtual void CalculateAverageValue(Point point, int i, Type type)
        {
            point.Y = point.YValue = YData[i] = GetAverage(type, Series.YName, i);
            point.Visible = true;
        }

        protected double GetAverage(Type type, string name, int i)
        {
            PropertyInfo prop = type?.GetProperty(name);
            IEnumerable<object> data = CurrentViewData;
            object previousPoint = i - 1 > -1 ? data.ElementAt(i - 1) : null;
            object nextPoint = i + 1 < data.Count() ? data.ElementAt(i + 1) : null;
            return (((previousPoint != null && prop.GetValue(previousPoint) != null) ? (!double.IsNaN((double)prop.GetValue(previousPoint)) ? (double)prop.GetValue(previousPoint) : 0) : 0) +
                ((nextPoint != null && prop.GetValue(nextPoint) != null) ? !double.IsNaN((double)prop.GetValue(nextPoint)) ? (double)prop.GetValue(nextPoint) : 0 : 0)) / 2;
        }

        internal virtual string ClipPathId()
        {
            return Owner.ID + Constants.SERIESCLIPRECTID + Index;
        }

        // TODO: Need to override for polar radar renderer
        internal virtual string ClipRectId()
        {
            return ClipPathId() + "_Rect";
        }

        internal string SeriesClipPath()
        {
            return "url(#" + ClipPathId() + ")";
        }

        protected virtual void CreateSeriesElements(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            CultureInfo culture = CultureInfo.InvariantCulture;
            SvgRenderer.OpenGroupElement(builder, SeriesElementId(), "translate(" + ClipRect.X.ToString(culture) + ',' + ClipRect.Y.ToString(culture) + ')', SeriesClipPath());
            SvgRenderer.RenderClipPath(builder, ClipPathId(), new Rect { X = 0, Y = 0, Width = ClipRect.Width, Height = ClipRect.Height }, Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible");
        }

        internal void FindClipRect()
        {
            Rect rect = ClipRect = new Rect();
            if (Owner.RequireInvertedAxis)
            {
                rect.X = YAxisRenderer.Rect.X;
                rect.Y = XAxisRenderer.Rect.Y;
                rect.Width = YAxisRenderer.Rect.Width;
                rect.Height = XAxisRenderer.Rect.Height;
            }
            else
            {
                rect.X = XAxisRenderer.Rect.X;
                rect.Y = YAxisRenderer.Rect.Y;
                rect.Width = XAxisRenderer.Rect.Width;
                rect.Height = YAxisRenderer.Rect.Height;
            }
        }

        protected virtual string SeriesID()
        {
            return Owner.ID + "_Series_" + Index;
        }

        internal void PushCategoryData(Point point, int index, string pointX)
        {
            if (Series.Visible)
            {
                if (!XAxisRenderer.Axis.IsIndexed)
                {
                    if (XAxisRenderer.Labels.IndexOf(pointX) < 0)
                    {
                        XAxisRenderer.Labels.Add(pointX);
                    }

                    point.XValue = XAxisRenderer.Labels.IndexOf(pointX);
                }
                else
                {
                    if (XAxisRenderer.Labels.Count - 1 >= index && !string.IsNullOrEmpty(XAxisRenderer.Labels[index]))
                    {
                        XAxisRenderer.Labels[index] += ", " + pointX;
                    }
                    else
                    {
                        XAxisRenderer.Labels.Add(pointX);
                    }

                    point.XValue = index;
                }
            }
        }

        protected virtual bool IsMarker()
        {
            return true;
        }

        protected virtual int IncFactor()
        {
            // Need to override ChartSeriesType.RangeArea || ChartSeriesType.RangeColumn) factor 2
            return 1;
        }

        internal void PerformInitialAnimation(List<InitialAnimationInfo> animationInfo)
        {
            AnimationOptions options = AnimationOptions;
            if (options == null)
            {
                return;
            }

            if (options.Type == AnimationType.Progressive)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = options.Type.ToString(), ElementId = options.Id, ClipPathId = ClipRectId(), Duration = Series.Animation.Duration, Delay = Series.Animation.Delay, StrokeDashArray = Series.DashArray });
            }
            else if (options.Type == AnimationType.Linear)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = options.Type.ToString(), ElementId = options.Id, ClipPathId = ClipRectId(), Duration = Series.Animation.Duration, Delay = Series.Animation.Delay, IsInvertedAxis = Series.Container.RequireInvertedAxis });
            }
            else if (options.Type == AnimationType.Rect)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = options.Type.ToString(), ElementId = options.Id, ClipPathId = ClipRectId(), Duration = Series.Animation.Duration, Delay = Series.Animation.Delay, IsInvertedAxis = Owner.RequireInvertedAxis });
                int animationInfoIndex = animationInfo.Count - 1;
                int count = Category() == SeriesCategories.Indicator ? 0 : 1;
                List<Point> visiblePoints = ChartHelper.GetVisiblePoints(Series.Renderer.Points);
                foreach (var point in visiblePoints)
                {
                    if (point.SymbolLocations.Count == 0 && !(Series.Type == ChartSeriesType.BoxAndWhisker && point.Regions.Count != 0))
                    {
                        continue;
                    }

                    AnimateRect(count, point, animationInfo, animationInfoIndex);
                    count++;
                }
            }
            else if (options.Type == AnimationType.Marker)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = options.Type.ToString(), ElementId = options.Id, ClipPathId = ClipRectId(), Duration = Series.Animation.Duration, Delay = Series.Animation.Delay });
                int animationInfoIndex = animationInfo.Count - 1;
                int count = 1;
                foreach (var point in Points)
                {
                    if (point.SymbolLocations.Count == 0)
                    {
                        continue;
                    }

                    animationInfo[animationInfoIndex].PointIndex.Add(count);
                    animationInfo[animationInfoIndex].PointX.Add(point.SymbolLocations[0].X);
                    animationInfo[animationInfoIndex].PointY.Add(point.SymbolLocations[0].Y);
                    count++;
                }
            }
            else if (options.Type == AnimationType.PolarRadar)
            {
                animationInfo.Add(new InitialAnimationInfo { Type = options.Type.ToString(), ElementId = SeriesElementId(), ClipPathId = ClipRectId(), Duration = Series.Animation.Duration, Delay = Series.Animation.Delay, PointX = new List<double> { (Series.Renderer.ClipRect.Width / 2) + Series.Renderer.ClipRect.X }, PointY = new List<double> { (Series.Renderer.ClipRect.Height / 2) + Series.Renderer.ClipRect.Y } });
            }

            // TODO: Need to add rect animation while column type series
            InitialAnimationInfo currentAnimationInfo = animationInfo[animationInfo.Count - 1];
            if (Series.Marker.Visible && IsMarker())
            {
                int j = 1;
                int incFactor = IncFactor();
                currentAnimationInfo.MarkerInfo = new MarkerAnimationInfo() { MarkerElementId = SeriesSymbolId(), MarkerClipPathId = Series.Renderer.MarkerClipRectId() };
                for (int i = 0; i < Points.Count; i++)
                {
                    if (Points[i].SymbolLocations != null)
                    {
                        if (Points[i].SymbolLocations.Count == 0)
                        {
                            continue;
                        }

                        currentAnimationInfo.MarkerInfo.PointIndex.Add(j);
                        currentAnimationInfo.MarkerInfo.PointX.Add(Points[i].SymbolLocations[0].X);
                        currentAnimationInfo.MarkerInfo.PointY.Add(Points[i].SymbolLocations[0].Y);

                        // if (incFactor == 2)
                        // {
                        //     ChartInternalLocation lowPoint = GetRangeLowPoint(series.Points[i].Regions[0], series);
                        //     currentAnimationInfo.MarkerInfo.LowPointIndex.Add(j + 1);
                        //     currentAnimationInfo.MarkerInfo.LowPointX.Add(lowPoint.X);
                        //     currentAnimationInfo.MarkerInfo.LowPointY.Add(lowPoint.Y);
                        // }
                        j += incFactor;
                    }
                }
            }

            if (Series.ErrorBar.Visible)
            {
                currentAnimationInfo.ErrorBarInfo = new ErrorBarAnimationInfo { ErrorBarElementId = Series.Renderer.ErrorBarId, ErrorBarClipPathId = Series.Renderer.ErrorBarClipRectId };
            }

            if (Series.Marker.DataLabel.Visible && Series.Marker.DataLabel.Template == null)
            {
                currentAnimationInfo.DataLabelInfo = new DataLabelAnimatioInfo { ShapeGroupId = Series.Marker.DataLabel.Renderer?.SeriesShapeId(), TextGroupId = Series.Marker.DataLabel.Renderer?.SeriesTextId() };
            }
            else if (Series.Marker.DataLabel.Visible && Series.Marker.DataLabel.Template != null)
            {
                currentAnimationInfo.DataLabelInfo = new DataLabelAnimatioInfo { TemplateId = SeriesTemplateID };
            }
        }

        private void AnimateRect(int index, Point point, List<InitialAnimationInfo> animationInfo, int animationInfoIndex)
        {
            bool isPlot = point.YValue < 0;
            double x, y, centerX, centerY;
            double elementHeight = point.Regions[0].Height;
            double elementWidth = point.Regions[0].Width;
            if (!Owner.RequireInvertedAxis)
            {
                x = point.Regions[0].X;
                if (Series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    y = (1 - ChartHelper.ValueToCoefficient(0, YAxisRenderer)) * YAxisRenderer.Rect.Height;
                    centerX = x;
                    centerY = y;
                }
                else
                {
                    y = +point.Regions[0].Y;
                    centerY = (SeriesType().ToString().Contains("HighLow", StringComparison.InvariantCulture) || Series.Type.ToString().Contains("Waterfall", StringComparison.InvariantCulture)) ? y + (elementHeight / 2) : (isPlot != YAxisRenderer.Axis.IsInversed) ? y : y + elementHeight;
                    centerX = isPlot ? x : x + elementWidth;
                }
            }
            else
            {
                y = +point.Regions[0].Y;
                if (Series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    x = ChartHelper.ValueToCoefficient(0, YAxisRenderer) * YAxisRenderer.Rect.Width;
                    centerX = x;
                    centerY = y;
                }
                else
                {
                    x = +point.Regions[0].X;
                    centerY = isPlot ? y : y + elementHeight;
                    centerX = (SeriesType().ToString().Contains("HighLow", StringComparison.InvariantCulture) || Series.Type.ToString().Contains("Waterfall", StringComparison.InvariantCulture)) ? x + (elementWidth / 2) : (isPlot != YAxisRenderer.Axis.IsInversed) ? x + elementWidth : x;
                }
            }

            animationInfo[animationInfoIndex].PointIndex.Add(index);
            animationInfo[animationInfoIndex].PointX.Add(centerX);
            animationInfo[animationInfoIndex].PointY.Add(centerY);
            animationInfo[animationInfoIndex].PointWidth.Add(elementWidth);
            animationInfo[animationInfoIndex].PointHeight.Add(elementHeight);
        }

        internal virtual List<string> GetLabelText(Point currentPoint, ChartSeriesRenderer seriesRenderer)
        {
            // Need to override for other series types
            /*switch (series.SeriesType)
            {
                case SeriesValueType.XY:

                    if (series.ChartInstance.ChartAreaType == ChartAreaType.PolarAxes)
                    {
                        double logWithIn = (series.StackedValues != null) ? LogWithIn(series.StackedValues.EndValues[currentPoint.Index], series.YAxis) : 0;
                        bool withIn = (series.StackedValues != null) ? WithIn(series.StackedValues.EndValues[currentPoint.Index], series.YAxis.VisibleRange) : false;
                        if ((series.DrawType == ChartDrawType.StackingLine || series.DrawType == ChartDrawType.StackingColumn || series.DrawType == ChartDrawType.StackingArea) &&
                            series.YAxis.ValueType == ValueType.Logarithmic && logWithIn != 0 || withIn)
                        {
#pragma warning disable CA1305
                            text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.YValue.ToString());
                        }
                        else if ((series.YAxis.ValueType == ValueType.Logarithmic && LogWithIn(currentPoint.YValue, series.YAxis) != 0) || WithIn(currentPoint.YValue, series.YAxis.VisibleRange))
                        {
                            text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.YValue.ToString());
                        }
                    }
                    else
                    {
                        text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.YValue.ToString());
                    }
                    break;
                case SeriesValueType.HighLow:
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : Math.Max((double)currentPoint.High, (double)currentPoint.Low).ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : Math.Min((double)currentPoint.High, (double)currentPoint.Low).ToString());
                    break;
                case SeriesValueType.HighLowOpenClose:
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : Math.Max((double)currentPoint.High, (double)currentPoint.Low).ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : Math.Min((double)currentPoint.High, (double)currentPoint.Low).ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : Math.Max((double)currentPoint.Open, (double)currentPoint.Close).ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : Math.Min((double)currentPoint.Open, (double)currentPoint.Close).ToString());
                    break;
                case SeriesValueType.BoxPlot:
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.Median.ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.Maximum.ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.Minimum.ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.UpperQuartile.ToString());
                    text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.LowerQuartile.ToString());
                    foreach (double Liers in currentPoint.Outliers)
                    {
                        text.Add(!string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : Liers.ToString());
                    }
                    break;
            }*/
            return new List<string> { !string.IsNullOrEmpty(currentPoint.Text) ? currentPoint.Text : currentPoint.YValue.ToString(CultureInfo.InvariantCulture) };
        }

        internal virtual bool IsRectSeries()
        {
            return false;
        }

        internal virtual SeriesValueType SeriesType()
        {
            // TODO: Need to override for series with their SeriesValueType
            return SeriesValueType.XY;
        }

        internal virtual bool ShouldRenderMarker()
        {
            return true;
        }

        internal virtual object GetMarkerY(Point point)
        {
            // Need to override for other series types
            // if (series.Type == ChartSeriesType.RangeArea || series.Type == ChartSeriesType.RangeColumn || series.DrawType == ChartDrawType.RangeColumn)
            // {
            //     y = index != 0 ? point.Low : point.High;
            // }
            // else if (series.Type == ChartSeriesType.BoxAndWhisker)
            // {
            //     y = point.Outliers[index];
            // }
            return point.Y;
        }

        protected string MarkerClipPathID()
        {
            return Owner.ID + "_ChartMarkerClipRect_" + Index;
        }

        protected string ErrorBarClipPathID()
        {
            return Owner.ID + "_ChartErrorBarClipRect_" + Index;
        }

        public string ErrorBarGroupID()
        {
            return Owner.ID + "ErrorBarGroup" + Index;
        }

        internal string MarkerClipPath()
        {
            return "url(#" + MarkerClipPathID() + ")";
        }

        internal string SeriesSymbolId()
        {
            return Owner.ID + "SymbolGroup" + Index;
        }

        internal void UpdateElementRef()
        {
            string series_g = Owner.ID + "SeriesGroup" + Index;
            string errorbar_g = Owner.ID + "ErrorBarGroup" + Index;
            string symbol_g = Owner.ID + "SymbolGroup" + Index;
            string text_g = Owner.ID + "TextGroup" + Index;
            string shape_g = Owner.ID + "ShapeGroup" + Index;
            SeriesElement = Owner.SvgRenderer.GroupCollection.Find(item => item.Id == series_g);
            ErrorBarElement = Owner.SvgRenderer.GroupCollection.Find(item => item.Id == errorbar_g);
            SymbolElement = Owner.SvgRenderer.GroupCollection.Find(item => item.Id == symbol_g);
            TextElement = Owner.SvgRenderer.GroupCollection.Find(item => item.Id == text_g);
            ShapeElement = Owner.SvgRenderer.GroupCollection.Find(item => item.Id == shape_g);
        }

        internal virtual string MarkerClipRectId()
        {
            return MarkerClipPathID() + "_Rect";
        }

        internal virtual void CalculateMarkerClipPath()
        {
            double explodeValue = Series.Marker.Border.Width + 13;
            bool isZoomed = false;

            // chart.ZoomingModule != null ? chart.ZoomingModule.IsZoomed : false;
            if (Series.Marker.Visible && ClipRect != null)
            {
                double markerHeight = isZoomed ? 0 : ((Series.Marker.Height + explodeValue) / 2);
                double markerWidth = isZoomed ? 0 : ((Series.Marker.Width + explodeValue) / 2);
                markerClipRect = new Rect(-markerWidth, -markerHeight, ClipRect.Width + (markerWidth * 2), ClipRect.Height + (markerHeight * 2));
            }
        }

        internal virtual void RenderMarkerClipPath(RenderTreeBuilder builder)
        {
            // Need to implemented rect animation
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            Owner.SvgRenderer.OpenGroupElement(builder, SeriesSymbolId(), "translate(" + ClipRect.X.ToString(Culture) + "," + ClipRect.Y.ToString(Culture) + ")", MarkerClipPath());
            Owner.SvgRenderer.RenderClipPath(builder, MarkerClipPathID(), markerClipRect, visibility);
        }

        internal virtual void CalculateErrorBarClipPath()
        {
            if (Series.ErrorBar.Visible)
            {
                double markerHeight = Series.Marker.Height / 2;
                double markerWidth = Series.Marker.Width / 2;
                ErrorBarId = ErrorBarGroupID();
                ErrorBarClipRectId = ErrorBarClipPathID() + "_Rect";
                errorBarClipRect = new Rect(-markerWidth, -markerHeight, ClipRect.Width + (markerWidth * 2), ClipRect.Height + (markerHeight * 2));
            }
        }

        internal virtual void RenderErrorBarClipPath(RenderTreeBuilder builder)
        {
            // Need to implemented rect animation
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            Owner.SvgRenderer.OpenGroupElement(builder, ErrorBarId, "translate(" + ClipRect.X.ToString(Culture) + "," + ClipRect.Y.ToString(Culture) + ")", "url(#" + ErrorBarClipPathID() + ")");
            Owner.SvgRenderer.RenderClipPath(builder, ErrorBarClipPathID(), errorBarClipRect, visibility);
        }

        internal virtual SeriesCategories Category()
        {
            return SeriesCategories.Series;
        }

        internal void UpdateCategoryData()
        {
            XAxisRenderer.Labels.Clear();
            foreach (ChartSeriesRenderer renderer in Owner.SeriesContainer.Renderers)
            {
                renderer.InitSeriesRendererFields();
                renderer.ProcessData();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            IsSeriesRender = !firstRender;
            if (!firstRender && Series.Container.Redraw)
            {
                await Series.Container.PerformRedrawAnimation();
            }
        }
    }

    internal class DefaultSeriesRenderer : ChartSeriesRenderer
    {
    }
}

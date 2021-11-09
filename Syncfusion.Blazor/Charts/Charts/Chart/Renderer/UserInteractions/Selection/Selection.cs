using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Runtime.InteropServices;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal partial class Selection : BaseSelection
    {
        internal Selection(SfChart chart, bool wireEvents = true)
        {
            chartInstance = chart;
            InitPrivateVariables();
            if (wireEvents)
            {
                AddEventListener();
            }
        }

        private SfChart chartInstance { get; set; }

        private Rect seriesClipRect
        {
            get
            {
                return chartInstance.AxisContainer.AxisLayout?.SeriesClipRect;
            }
        }

        private string closeIconId { get; set; } = string.Empty;

        private string draggedRect { get; set; }

        private List<object> elementsList { get; set; } = new List<object>();

        private List<SvgClass> previousSelectedEle { get; set; } = new List<SvgClass>();

        private bool dragging { get; set; }

        private bool lassoDownCompleted { get; set; }

        private Rect rectPoints { get; set; } = new Rect();

        private Dictionary<string, Rect> dragRectArray { get; set; } = new Dictionary<string, Rect>();

        private Dictionary<string, Rect> filterArray { get; set; } = new Dictionary<string, Rect>();

        private Dictionary<string, string> lassoPaths { get; set; } = new Dictionary<string, string>();

        private int targetIndex { get; set; }

        private bool rectGrabbing { get; set; }

        private bool isdrawRect { get; set; } = true;

        private bool resizing { get; set; }

        private Rect dragRect { get; set; }

        private int count { get; set; } = -1;

        private Dictionary<(int, int), List<Point>> selectedLassoPoints { get; set; } = new Dictionary<(int, int), List<Point>>();

        private int resizeMode { get; set; }

        protected List<IChartElementRenderer> Series { get; set; } = new List<IChartElementRenderer>();

        protected List<ChartSelectedDataIndex> HighlightDataIndexes { get; set; } = new List<ChartSelectedDataIndex>();

        internal SelectionMode CurrentMode { get; set; }

        internal bool IsSeriesMode { get; set; }

        internal List<ChartSelectedDataIndex> SelectedDataIndexes { get; set; } = new List<ChartSelectedDataIndex>();

        private void AddEventListener()
        {
            chartInstance.MouseMove += MouseMoveHandler;
            chartInstance.MouseDown += MouseDownHandler;
            chartInstance.MouseUp += MouseUpHandler;
            chartInstance.MouseCancel += MouseCancelHandler;
            chartInstance.MouseClick += MouseClickHandler;
        }

        protected void MouseMoveHandler(object source, ChartInternalMouseEventArgs e)
        {
            SvgClass target;
            if (chartInstance.HighlightMode != HighlightMode.None)
            {
                if (!string.IsNullOrEmpty(e.Target) || e.Target.Contains("Point", StringComparison.InvariantCulture) || e.Target.Contains("Symbol", StringComparison.InvariantCulture))
                {
                    if (e.Target.Contains("text", StringComparison.InvariantCulture))
                    {
                        target = FindDomElement(chartInstance.SvgRenderer, e.Target.Replace("text", "shape", StringComparison.InvariantCulture));
                    }
                    else
                    {
                        target = FindDomElement(chartInstance.SvgRenderer, e.Target);
                    }

                    if (target?.Class?.Contains("highlight", StringComparison.InvariantCulture) == true || target?.Class?.Contains("selection", StringComparison.InvariantCulture) == true)
                    {
                        return;
                    }

                    CalculateSelectedElements(e);
                    return;
                }
            }

            if (chartInstance.SelectionMode == SelectionMode.None)
            {
                return;
            }

            DragSelectionProcess(e);
        }

        private void MouseDownHandler(object source, ChartInternalMouseEventArgs e)
        {
            if (chartInstance.IsPointMouseDown || chartInstance.SelectionMode == SelectionMode.None || chartInstance.IsChartDrag)
            {
                return;
            }

            if (chartInstance.IsDoubleTap || !chartInstance.IsTouch || rectPoints != null)
            {
                DragStart(chartInstance.AxisContainer.AxisLayout.SeriesClipRect, chartInstance.MouseDownX, chartInstance.MouseDownY, e);
            }
        }

        private void MouseUpHandler(object source, ChartInternalMouseEventArgs e)
        {
            CompleteSelection(e);
        }

        private void MouseCancelHandler(object source, ChartInternalMouseEventArgs e)
        {
            CompleteSelection(e);
        }

        private void MouseClickHandler(object source, ChartInternalMouseEventArgs e)
        {
            CalculateSelectedElements(e);
        }

        internal void InvokeSelection()
        {
            InitPrivateVariables();
            SeriesStyles();
            if (!CurrentMode.ToString().Contains("Drag", StringComparison.InvariantCulture))
            {
                SelectDataIndex(SelectedDataIndexes.Concat(chartInstance.SelectedDataIndexes).ToList());
            }
        }

        internal void CallSeriesStyles(bool isSelection = true)
        {
            StyleId = isSelection ? chartInstance.ID + "_ej2_chart_selection" : chartInstance.ID + "_ej2_chart_highlight";
            SeriesStyles();
        }

        protected void SeriesStyles()
        {
            SelectionPattern selectionPattern = chartInstance.SelectionPattern;
            SelectionPattern highlightPattern = chartInstance.HighlightPattern;
            if (selectionPattern != SelectionPattern.None || highlightPattern != SelectionPattern.None)
            {
                foreach (ChartSeriesRenderer seriesRenderer in Series)
                {
                    string fill = FindPattern(seriesRenderer.Interior, seriesRenderer.Index, StyleId.Contains("highlight", StringComparison.Ordinal) ? highlightPattern : selectionPattern, seriesRenderer.Series.Opacity);
                    string pattern = "{" + (seriesRenderer.Series.Type == ChartSeriesType.HiloOpenClose ? "stroke" : "fill") + ":" + fill + "}";
                    pattern = pattern.Contains("None", StringComparison.InvariantCulture) ? "{}" : pattern;
                    InnerHTML += !string.IsNullOrEmpty(seriesRenderer.Series.SelectionStyle) ? string.Empty : "." + (!string.IsNullOrEmpty(seriesRenderer.Series.SelectionStyle) ? seriesRenderer.Series.SelectionStyle : StyleId + "_series" + seriesRenderer.Index + "," + "." + StyleId + "_series_" + seriesRenderer.Index + "> *") + pattern;
                }
            }

            InnerHTML += "." + Unselected + " { opacity:" + 0.3 + ";} ";
            if (InnerHTML.Contains("selection", StringComparison.InvariantCulture))
            {
                chartInstance.SelectionStyleInstance?.AppendStyleElement(InnerHTML);
                InnerHTML = string.Empty;
            }
            else if (InnerHTML.Contains("highlight", StringComparison.InvariantCulture))
            {
                chartInstance.HighlightStyleInstance?.AppendStyleElement(InnerHTML);
                InnerHTML = string.Empty;
            }
            else
            {
                chartInstance.SelectionStyleInstance?.AppendStyleElement(InnerHTML);
                chartInstance.HighlightStyleInstance?.AppendStyleElement(InnerHTML);
                InnerHTML = string.Empty;
            }
        }

        private void SelectDataIndex(List<ChartSelectedDataIndex> indexes)
        {
            foreach (ChartSelectedDataIndex index in indexes)
            {
                if (GetElementByIndex(index)?.First() != null)
                {
                    PerformSelection(index, GetElementByIndex(index)?.First().Id);
                }
            }
        }

        internal void RedrawSelection(SelectionMode oldMode)
        {
            IsSeriesMode = oldMode == SelectionMode.Series;
            List<ChartSelectedDataIndex> chartSelectedDatas = SelectedDataIndexes.GetRange(0, SelectedDataIndexes.Count).ToList();
            List<ChartSelectedDataIndex> charthighlightedDatas = HighlightDataIndexes.GetRange(0, HighlightDataIndexes.Count).ToList();
            if (StyleId.Contains("highlight", StringComparison.InvariantCulture) && HighlightDataIndexes.Count > 0)
            {
                RemoveSelectedElements(HighlightDataIndexes, chartInstance.SeriesContainer.Renderers);
                chartSelectedDatas = charthighlightedDatas;
            }
            else
            {
                RemoveSelectedElements(SelectedDataIndexes, chartInstance.SeriesContainer.Renderers);
            }

            BlurEffect(chartInstance.SeriesContainer.Renderers);
            SelectDataIndex(chartSelectedDatas);
        }

        private void RemoveSelectedElements(List<ChartSelectedDataIndex> index, List<IChartElementRenderer> seriesCollection)
        {
            index.RemoveRange(0, index.Count);
            seriesCollection.ToList().ForEach(x => RemoveStyles(GetSeriesElements(x)));
        }

        private string GetElementFromDatalabel(string targetID)
        {
            if (targetID.Contains("Text", StringComparison.InvariantCulture) && targetID.Contains("Series", StringComparison.InvariantCulture))
            {
                string primId = targetID.Split("_Text_")[0];
                ChartSeriesRenderer targetRenderer = (ChartSeriesRenderer)chartInstance.SeriesContainer.Renderers[Convert.ToInt32(primId.Split("_Point_")[0].Split("_Series_")[1], null)];
                if (targetRenderer.IsRectSeries() || targetRenderer.Series.Type == ChartSeriesType.Bubble || targetRenderer.Series.Type == ChartSeriesType.Scatter)
                {
                    return primId;
                }
                else
                {
                    return primId.Insert(primId.Length, "_Symbol");
                }
            }

            return targetID;
        }

        private void PerformSelection(ChartSelectedDataIndex index, string elementId)
        {
            IsSeriesMode = CurrentMode == SelectionMode.Series;
            if ((chartInstance.SeriesContainer.Renderers[index.Series] as ChartSeriesRenderer).Series.Type == ChartSeriesType.Area && (CurrentMode == SelectionMode.Point || CurrentMode == SelectionMode.Cluster) && elementId == chartInstance.ID + "_Series_" + index.Series)
            {
                string className = GenerateStyle(chartInstance.SeriesContainer.Renderers[index.Series]);
                if (FindElementByClass(chartInstance.SvgRenderer, className).Count > 0)
                {
                    FindTrackballElements(new List<SvgClass>() { FindElementByClass(chartInstance.SvgRenderer, className).First() }, className, index.Series);
                    BlurEffect(chartInstance.SeriesContainer.Renderers);
                }
            }

            switch (CurrentMode)
            {
                case SelectionMode.Series:
                    SelectionChart(index, GetSeriesElements(chartInstance.SeriesContainer.Renderers[index.Series]));
                    SelectionComplete(index, CurrentMode);
                    BlurEffect(chartInstance.SeriesContainer.Renderers);
                    break;
                case SelectionMode.Point:
                    if (index.Point >= 0)
                    {
                        SelectionChart(index, new List<SvgClass>() { FindDomElement(chartInstance.SvgRenderer, elementId) });
                        SelectionComplete(index, CurrentMode);
                        BlurEffect(chartInstance.SeriesContainer.Renderers);
                    }

                    break;
                case SelectionMode.Cluster:
                    if (index.Point >= 0)
                    {
                        SelectionChart(index, GetClusterElements(index));
                        SelectionComplete(index, CurrentMode);
                        BlurEffect(chartInstance.SeriesContainer.Renderers);
                    }

                    break;
            }
        }

        private string GenerateStyle(IChartElementRenderer seriesRenderer)
        {
            ChartSeries series = ((ChartSeriesRenderer)seriesRenderer).Series;
            if (seriesRenderer != null)
            {
                if (StyleId.Contains("selection", StringComparison.InvariantCulture) && chartInstance.SelectionMode != SelectionMode.None)
                {
                    Unselected = !string.IsNullOrEmpty(series.UnSelectedStyle) ? series.UnSelectedStyle : Unselected;
                }

                if (StyleId.Contains("highlight", StringComparison.InvariantCulture) && chartInstance.HighlightMode != HighlightMode.None)
                {
                    Unselected = !string.IsNullOrEmpty(series.NonHighlightStyle) ? series.NonHighlightStyle : Unselected;
                }

                return !string.IsNullOrEmpty(series.SelectionStyle) ? series.SelectionStyle : StyleId + "_series" + ((ChartSeriesRenderer)seriesRenderer).Index;
            }

            return null;
        }

        private void InitPrivateVariables()
        {
            int pointMax = 0;
            StyleId = chartInstance.ID + "_ej2_chart_selection";
            Unselected = chartInstance.ID + "_ej2_deselected";
            closeIconId = chartInstance.ID + "_ej2_drag_close";
            draggedRect = chartInstance.ID + "_ej2_drag_rect_";
            SelectedDataIndexes.Clear();
            rectPoints = null;
            IsSeriesMode = chartInstance.SelectionMode == SelectionMode.Series;
#pragma warning disable CA1806
            elementsList.Concat(chartInstance.SvgRenderer.CircleCollection).Concat(chartInstance.SvgRenderer.PathElementList).Concat(chartInstance.SvgRenderer.RectElementList).Concat(chartInstance.SvgRenderer.LineElementList);
            count = -1;
            dragRectArray.Clear();
            filterArray.Clear();
            previousSelectedEle.Clear();
            chartInstance.SeriesContainer.Renderers.ForEach(item => pointMax = Math.Max(pointMax, ((ChartSeriesRenderer)item).Points.Count));
            selectedLassoPoints?.Clear();
            ReqPatterns?.Clear();
            Series = chartInstance.SeriesContainer.Renderers;
            CurrentMode = chartInstance.SelectionMode;
        }

        private void FindTrackballElements(List<SvgClass> selectedElements, string className, int seriesIndex)
        {
            List<SvgClass> trackballElements = new List<SvgClass>();
            List<SvgClass> elements = new List<SvgClass>();
            foreach (SvgClass element in selectedElements)
            {
                if (element != null)
                {
                    selectedElements.ForEach(item =>
                    {
                        if (item?.Id?.Contains("Series_" + seriesIndex, StringComparison.InvariantCulture) == true)
                        {
                            trackballElements.Concat(FindElementByClass(chartInstance.SvgRenderer, className));
#pragma warning restore CA1806
                        }
                    });
                    if (trackballElements.Count != 0)
                    {
                        foreach (SvgClass trackballElement in trackballElements)
                        {
                            if (trackballElement.Id.Contains("Trackball", StringComparison.InvariantCulture))
                            {
                                elements.Add(trackballElement);
                            }
                        }

                        RemoveStyles(elements);
                    }
                }
            }
        }

        protected bool CheckVisibility(List<ChartSelectedDataIndex> selectedIndexes)
        {
            if (selectedIndexes.Count == 0)
            {
                return false;
            }

            bool visible = false;
            List<int> uniqueSeries = new List<int>();
            selectedIndexes.Where(x => !uniqueSeries.Contains(x.Series)).ToList().ForEach(y => uniqueSeries.Add(y.Series));
            foreach (int index in uniqueSeries)
            {
                if ((chartInstance.SeriesContainer.Renderers[index] as ChartSeriesRenderer).Series.Visible)
                {
                    visible = true;
                    break;
                }
            }

            return visible;
        }

        private void BlurEffect(List<IChartElementRenderer> visibleSeries, bool legendClick = false)
        {
            bool visibility = CheckVisibility(HighlightDataIndexes) || CheckVisibility(SelectedDataIndexes);
            foreach (ChartSeriesRenderer seriesRenderer in visibleSeries)
            {
                if (seriesRenderer.Series.Visible)
                {
                    CheckSelectionElements(chartInstance.ID + "SeriesGroup" + seriesRenderer.Index, GenerateStyle(seriesRenderer), visibility, legendClick, seriesRenderer.Index);
                    if (seriesRenderer.Series.Marker.Visible)
                    {
                        CheckSelectionElements(chartInstance.ID + "SymbolGroup" + seriesRenderer.Index, GenerateStyle(seriesRenderer), visibility, legendClick, seriesRenderer.Index);
                    }
                }
            }
        }

#pragma warning disable CA1822
        private bool PointIdRequired(ChartSeriesRenderer currentSeriesRenderer)
#pragma warning restore CA1822
        {
            if ((currentSeriesRenderer.Series.Type == ChartSeriesType.Polar || currentSeriesRenderer.Series.Type == ChartSeriesType.Radar) && (currentSeriesRenderer.Series.DrawType == ChartDrawType.Column || currentSeriesRenderer.Series.DrawType == ChartDrawType.RangeColumn || currentSeriesRenderer.Series.DrawType == ChartDrawType.Scatter || currentSeriesRenderer.Series.DrawType == ChartDrawType.StackingColumn))
            {
                return true;
            }
            else if (currentSeriesRenderer.IsRectSeries() || currentSeriesRenderer.Series.Type == ChartSeriesType.Bubble || currentSeriesRenderer.Series.Type == ChartSeriesType.Scatter)
            {
                return true;
            }

            return false;
        }

        private void CheckSelectionElements(string elementId, string className, bool visibility, bool legendClick, int series)
        {
            List<SvgClass> children = new List<SvgClass>();
            List<string> childrenIds = new List<string>();
            ChartSeriesRenderer currentRenderer = (ChartSeriesRenderer)chartInstance.SeriesContainer.Renderers[series];
            ChartSeries currentSeries = currentRenderer.Series;
            if (elementId.Contains("Series", StringComparison.InvariantCulture))
            {
                if (PointIdRequired(currentRenderer))
                {
                    childrenIds = currentRenderer.DynamicOptions.PathId;
                    if (currentSeries.Type == ChartSeriesType.Histogram)
                    {
                        childrenIds.Add(chartInstance.ID + "_Series_" + currentRenderer.Index + "_NDLine");
                    }

                    if (currentSeries.Type == ChartSeriesType.Waterfall)
                    {
                        childrenIds.Add(chartInstance.ID + "_Series_" + currentRenderer.Index + "_Connector_");
                    }

                    if (currentSeries.Type == ChartSeriesType.BoxAndWhisker)
                    {
                        childrenIds.AddRange(currentRenderer.DynamicOptions.MarkerSymbolId);
                    }
                }
                else
                {
                    childrenIds = new List<string>() { chartInstance.ID + "_Series_" + currentRenderer.Index };
                }
            }

            if (elementId.Contains("Symbol", StringComparison.InvariantCulture))
            {
                childrenIds = currentRenderer.DynamicOptions.MarkerSymbolId;
            }

            if (chartInstance.SelectionMode != SelectionMode.None || chartInstance.HighlightMode != HighlightMode.None)
            {
                childrenIds.ForEach(x => children.Add(FindDomElement(chartInstance.SvgRenderer, x)));
            }

            children.RemoveAll(item => item == null);
#pragma warning disable CA1305
            SvgClass parentEle = !currentRenderer.IsRectSeries() ? FindDomElement(chartInstance.SvgRenderer, chartInstance.ID + "_Series_" + currentRenderer.Index.ToString()) : null;
            string elementClassName = string.Empty;
            string parentClassName = parentEle?.Class;
            SvgClass selectElement = FindDomElement(chartInstance.SvgRenderer, elementId);
            foreach (SvgClass child in children)
            {
                elementClassName = child?.Class;
                if (!elementClassName?.Contains(className, StringComparison.InvariantCulture) == true && visibility && !child.Class.Contains("selection", StringComparison.InvariantCulture))
                {
                    AddSvgClass(child, Unselected);
                }
                else
                {
                    selectElement = child;
                    RemoveSvgClass(child, Unselected);
                    RemoveSvgClass(parentEle, Unselected);
                }

                if (child.Id.Contains("Trackball", StringComparison.InvariantCulture) && selectElement.Class == className)
                {
                    RemoveSvgClass(child, Unselected);
                    RemoveSvgClass(parentEle, Unselected);
                    AddSvgClass(child, className);
                }
            }

            if (elementId.Contains("Symbol", StringComparison.InvariantCulture) == true)
            {
                List<SvgClass> symbolElements = FindElementByClass(chartInstance.SvgRenderer, className);
                if (symbolElements.Count > 0 && symbolElements[0] != null)
                {
                    SvgClass symbolEle = FindDomElement(chartInstance.SvgRenderer, chartInstance.ID + "_Series_" + elementId[elementId.Length - 1]);
                    if (symbolEle?.Class?.Contains(Unselected, StringComparison.InvariantCulture) == true)
                    {
                        RemoveSvgClass(symbolEle, Unselected);
                    }
                }
            }

            if (chartInstance.LegendRenderer != null && chartInstance.LegendRenderer.LegendSettings.Visible)
            {
                SvgClass legendShape = FindDomElement(chartInstance.SvgRenderer, chartInstance.ID + "_chart_legend_shape_" + series);
                if (legendShape != null)
                {
                    RemoveSvgClass(legendShape, legendShape.Class);
                    elementClassName = selectElement != null ? selectElement.Class : string.Empty;
                    if (!elementClassName.Contains(className, StringComparison.InvariantCulture) && visibility && !IsSeriesStylesApplied(series, className))
                    {
                        AddSvgClass(legendShape, Unselected);
                        RemoveSvgClass(legendShape, className);
                    }
                    else
                    {
                        RemoveSvgClass(legendShape, Unselected);
                        if (string.IsNullOrEmpty(elementClassName) || elementClassName.Trim() == "EJ2-Trackball")
                        {
                            RemoveSvgClass(legendShape, className);
                        }
                        else
                        {
                            AddSvgClass(legendShape, className);
                        }
                    }

                    if (legendClick || IsSeriesStylesApplied(series, className))
                    {
                        AddSvgClass(legendShape, className);
                    }
                }
            }
        }

        private bool IsSeriesStylesApplied(int seriesIndex, string className)
        {
            List<string> pointElementId = new List<string>();
            ChartSeriesRenderer currentRenderer = (ChartSeriesRenderer)chartInstance.SeriesContainer.Renderers[seriesIndex];
            ChartSeriesType seriesType = currentRenderer.Series.Type;
            if (seriesType == ChartSeriesType.Polar || seriesType == ChartSeriesType.Radar)
            {
                pointElementId = (currentRenderer.IsRectSeries() || currentRenderer.Series.DrawType == ChartDrawType.Scatter) ? currentRenderer.DynamicOptions.PathId : currentRenderer.DynamicOptions.MarkerSymbolId;
            }
            else
            {
                pointElementId = (currentRenderer.IsRectSeries() || (seriesType == ChartSeriesType.Scatter || seriesType == ChartSeriesType.Bubble)) ? currentRenderer.DynamicOptions.PathId : currentRenderer.DynamicOptions.MarkerSymbolId;
            }

            List<SvgClass> pointElements = new List<SvgClass>();
            pointElementId.ForEach(x => pointElements.Add(FindDomElement(chartInstance.SvgRenderer, x)));
            pointElements.RemoveAll(item => item == null);
            return pointElements.Exists(item => item.Class.Contains(className, StringComparison.InvariantCulture));
        }

        private bool ToEquals(ChartSelectedDataIndex first, ChartSelectedDataIndex second, bool checkSeriesOnly)
        {
            return (first.Series == second.Series || (CurrentMode == SelectionMode.Cluster && !checkSeriesOnly)) && (checkSeriesOnly || (first.Point == second.Point));
        }

        internal void ApplyStyles(List<SvgClass> elements)
        {
            foreach (SvgClass element in elements)
            {
                if (element != null)
                {
                    RemoveSvgClass(element, Unselected);
                    AddSvgClass(element, GenerateStyle(FindSeriesFromId(element.Id)));
                    if (element.Id.Contains("BoxPath", StringComparison.InvariantCulture))
                    {
                        SvgClass markerEle = FindDomElement(chartInstance.SvgRenderer, element.Id.Replace("BoxPath", "Symbol", StringComparison.InvariantCulture));
                        if (markerEle != null)
                        {
                            AddSvgClass(markerEle, GenerateStyle(FindSeriesFromId(element.Id)));
                        }
                    }
                }
            }
        }

        private IChartElementRenderer FindSeriesFromId(string pointID)
        {
            return chartInstance.SeriesContainer.Renderers[short.Parse(pointID.Split("_Series_")[1].Split("_Point_")[0].Split("_")[0], null)];
        }

        private void RemoveStyles(List<SvgClass> elements)
        {
            foreach (SvgClass element in elements)
            {
                if (element != null)
                {
                    RemoveSvgClass(element, GenerateStyle(chartInstance.SeriesContainer.Renderers[ChartHelper.IndexFinder(element.Id)[0]]));
                }
            }
        }

        private List<SvgClass> GetSeriesElements(IChartElementRenderer renderer)
        {
            ChartSeriesRenderer seriesRenderer = (ChartSeriesRenderer)renderer;
            ChartSeries series = seriesRenderer.Series;
            List<SvgClass> seriesElements = new List<SvgClass>();
            if (series.Marker.Visible && series.Type != ChartSeriesType.Scatter && series.Type != ChartSeriesType.Bubble && !seriesRenderer.IsRectSeries())
            {
                seriesRenderer.DynamicOptions.MarkerSymbolId.ForEach(item => { seriesElements.Add(FindDomElement(chartInstance.SvgRenderer, item)); });
            }
            else
            {
                seriesRenderer.DynamicOptions.PathId.ForEach(item => { seriesElements.Add(FindDomElement(chartInstance.SvgRenderer, item)); });
            }
#pragma warning disable CA1304
            if (series.Type.ToString().Contains("Area", StringComparison.InvariantCulture) || series.Type.ToString().ToLower().Contains("line", StringComparison.InvariantCulture))
#pragma warning restore CA1304
            {
                seriesRenderer.DynamicOptions.PathId.ForEach(item => { seriesElements.Add(FindDomElement(chartInstance.SvgRenderer, item)); });
            }

            if (series.Type == ChartSeriesType.Histogram)
            {
                seriesElements.Add(FindDomElement(chartInstance.SvgRenderer, chartInstance.ID + "_Series_" + seriesRenderer.Index + "_NDLine"));
            }

            if (series.Type == ChartSeriesType.Polar && series.DrawType == ChartDrawType.Scatter)
            {
                seriesRenderer.DynamicOptions.PathId.ForEach(item => { seriesElements.Add(FindDomElement(chartInstance.SvgRenderer, item)); });
            }

            return seriesElements;
        }

        private List<SvgClass> GetElementByIndex(ChartSelectedDataIndex index, string suffix = "")
        {
            string elementId = chartInstance.ID + "_Series_" + index.Series + "_Point_" + index.Point;
            ChartSeries series = ((ChartSeriesRenderer)chartInstance.SeriesContainer.Renderers[index.Series]).Series;
            elementId = (!series.Renderer.IsRectSeries() && series.Type != ChartSeriesType.Scatter && series.Type != ChartSeriesType.Bubble && series.Marker.Visible) ? (elementId + "_Symbol" + suffix) : elementId;
            if (series.Type == ChartSeriesType.BoxAndWhisker)
            {
                elementId = elementId + "_BoxPath";
            }

            return new List<SvgClass>()
            {
                FindDomElement(chartInstance.SvgRenderer, elementId),
                series.Type == ChartSeriesType.RangeArea && series.Marker.Visible ? FindDomElement(chartInstance.SvgRenderer, elementId + "1") : null
            };
        }

        private void AddOrRemoveIndex(List<ChartSelectedDataIndex> indexes, ChartSelectedDataIndex index, [Optional] bool isAdd)
        {
            foreach (ChartSelectedDataIndex data in indexes.ToList())
            {
                if (ToEquals(data, index, IsSeriesMode))
                {
                    indexes.Remove(data);
                }
            }

            if (isAdd)
            {
                indexes.Add(index);
            }
        }

        private void RemoveSelection(int series, List<SvgClass> selectedElements, string seriesStyle, bool isBlurEffectNeeded)
        {
            if (selectedElements.Count > 0)
            {
                List<SvgClass> elements = new List<SvgClass>();
                selectedElements.ForEach(x => elements.Add(x));
                RemoveStyles(elements);
                IsSeriesMode = true;
#pragma warning disable CA2000
                AddOrRemoveIndex(SelectedDataIndexes, ChartSelectedDataIndex.CreateSelectedData(-1, series));
                foreach (ChartSeriesRenderer seriesRenderer in chartInstance.SeriesContainer.Renderers)
                {
                    seriesStyle = GenerateStyle(seriesRenderer);
                    if (FindElementByClass(chartInstance.SvgRenderer, seriesStyle).Count > 0)
                    {
                        elements.ForEach(x => CheckSelectionElements(x.Id, seriesStyle, true, true, series));
                        isBlurEffectNeeded = false;
                        break;
                    }
                }

                if (isBlurEffectNeeded)
                {
                    IsSeriesMode = chartInstance.SelectionMode == SelectionMode.Series;
                    BlurEffect(chartInstance.SeriesContainer.Renderers);
                }
            }
        }

        private bool IsAlreadySelected(ChartInternalMouseEventArgs events)
        {
            SvgClass targetElem = FindDomElement(chartInstance.SvgRenderer, events.Target);
            if (events.Type == "click")
            {
                CurrentMode = chartInstance.SelectionMode;
                StyleId = chartInstance.ID + "_ej2_chart_selection";
            }
            else if (events.Type == "mousemove")
            {
                CurrentMode = (SelectionMode)chartInstance.HighlightMode;
                HighlightDataIndexes.Clear();
                StyleId = chartInstance.ID + "_ej2_chart_highlight";
            }

            if (chartInstance.HighlightMode != HighlightMode.None && chartInstance.SelectionMode == SelectionMode.None && events.Type == "click")
            {
                return false;
            }

            if (chartInstance.HighlightMode != HighlightMode.None && previousSelectedEle.Count != 0 && previousSelectedEle[0] != null)
            {
                previousSelectedEle.RemoveAll(item => item == null);
                bool isElement = targetElem?.Id.Contains("Point", StringComparison.InvariantCulture) == true || targetElem?.Id.Contains("Symbol", StringComparison.InvariantCulture) == true;
                foreach (SvgClass element in previousSelectedEle)
                {
                    if (!string.IsNullOrEmpty(element.Class))
                    {
                        int[] data = ChartHelper.IndexFinder(element.Id);
                        if (element.Class.Contains("highlight", StringComparison.InvariantCulture) && (isElement || events.Type == "click"))
                        {
                            element.ChangeClass(string.Empty, null, true);
                            AddOrRemoveIndex(HighlightDataIndexes, ChartSelectedDataIndex.CreateSelectedData(data[1], data[0]));
                        }
                        else if (element.Class.Contains("highlight", StringComparison.InvariantCulture) && !isElement)
                        {
                            PerformSelection(ChartSelectedDataIndex.CreateSelectedData(data[1], data[0]), element.Id);
                        }
                    }
                }
            }

            return true;
        }

        private void CalculateSelectedElements(ChartInternalMouseEventArgs events)
        {
            int[] data;
            if (string.IsNullOrEmpty(events.Target))
            {
                return;
            }

            SvgClass targetElement = FindDomElement(chartInstance.SvgRenderer, GetElementFromDatalabel(events.Target));
            if (events.Target.Contains("Trackball", StringComparison.InvariantCulture))
            {
                targetElement = FindDomElement(chartInstance.SvgRenderer, events.Target.Split("_Trackball")[0] + "_Symbol");
                if (targetElement == null)
                {
                    targetElement = FindDomElement(chartInstance.SvgRenderer, events.Target.Split("_Trackball")[0]);
                }
            }

            if ((chartInstance.SelectionMode == SelectionMode.None && chartInstance.HighlightMode == HighlightMode.None) || !events.Target.Contains(chartInstance.ID + "_", StringComparison.InvariantCulture))
            {
                return;
            }

            if (events.Type == "mousemove")
            {
                if (targetElement == null && events.Target.Contains("Trackball", StringComparison.InvariantCulture))
                {
                    targetElement = FindDomElement(chartInstance.SvgRenderer, events.Target.Split("_Trackball_")[0]);
                }

                if (targetElement?.Class?.Contains("highlight", StringComparison.InvariantCulture) == true || targetElement?.Class?.Contains("selection", StringComparison.InvariantCulture) == true)
                {
                    return;
                }
            }

            IsAlreadySelected(events);
            if (events.Target.Contains("_Series_", StringComparison.InvariantCulture))
            {
                SvgClass element = null;
                data = ChartHelper.IndexFinder(events.Target);
                if (events.Target.Contains("_Trackball_1", StringComparison.InvariantCulture))
                {
                    element = FindDomElement(chartInstance.SvgRenderer, events.Target.Split("_Trackball")[0] + "_Symbol");
                    element = element == null ? FindDomElement(chartInstance.SvgRenderer, events.Target.Split("_Trackball_")[0]) : element;
                    data = ChartHelper.IndexFinder(element.Id);
                }
                else if (events.Target.Contains("_Trackball_0", StringComparison.InvariantCulture) || data[0] < 0 || (data[1] < 0 && !IsSeriesMode))
                {
                    return;
                }

                PerformSelection(ChartSelectedDataIndex.CreateSelectedData(data[1], data[0]), element != null ? element.Id : targetElement != null ? targetElement.Id : events.Target);
            }
        }

        internal void LegendSelection(int series, ChartInternalMouseEventArgs e)
        {
            SvgClass targetElement = FindDomElement(chartInstance.SvgRenderer, e.Target);
            if (e.Type == "mousemove")
            {
                if (e.Target.Contains("text", StringComparison.InvariantCulture))
                {
                    targetElement = FindDomElement(chartInstance.SvgRenderer, e.Target.Replace("text", "shape", StringComparison.InvariantCulture));
                }

                if (targetElement?.Class?.Contains("highlight", StringComparison.InvariantCulture) == true || targetElement?.Class?.Contains("selection", StringComparison.InvariantCulture) == true)
                {
                    return;
                }

                CurrentMode = (SelectionMode)chartInstance.HighlightMode;
            }

            bool isPreSelected = IsAlreadySelected(e);
            if (isPreSelected)
            {
                string seriesStyle = GenerateStyle(chartInstance.SeriesContainer.Renderers[series]);
                List<SvgClass> selectedElements = FindElementByClass(chartInstance.SvgRenderer, seriesStyle);
                IsSeriesMode = CurrentMode == SelectionMode.Series;
                bool isBlurEffectNeeded = true;
                if (selectedElements.Count > 0)
                {
                    RemoveSelection(series, selectedElements, seriesStyle, isBlurEffectNeeded);
                }
                else
                {
                    foreach (ChartSeriesRenderer item in chartInstance.SeriesContainer.Renderers)
                    {
                        if (item.Index != series && !chartInstance.IsMultiSelect)
                        {
                            seriesStyle = GenerateStyle(chartInstance.SeriesContainer.Renderers[item.Index]);
                            selectedElements = FindElementByClass(chartInstance.SvgRenderer, seriesStyle);
                            RemoveSelection(series, selectedElements, seriesStyle, isBlurEffectNeeded);
                        }
                    }

                    List<SvgClass> seriesElements = GetSeriesElements(chartInstance.SeriesContainer.Renderers[series]);
                    seriesElements.ForEach(x => CheckSelectionElements(x.Id, seriesStyle, false, true, series));
                    IsSeriesMode = true;
                    SelectionChart(ChartSelectedDataIndex.CreateSelectedData(-1, series), seriesElements);
                    IsSeriesMode = chartInstance.SelectionMode == SelectionMode.Series;
                    BlurEffect(chartInstance.SeriesContainer.Renderers, true);
                }
            }
        }

        private List<SvgClass> GetClusterElements(ChartSelectedDataIndex index)
        {
            List<SvgClass> clusters = new List<SvgClass>(), selectedElements;
            foreach (ChartSeriesRenderer seriesRenderer in chartInstance.SeriesContainer.Renderers)
            {
                index = ChartSelectedDataIndex.CreateSelectedData(index.Point, seriesRenderer.Index);
#pragma warning restore CA2000
                clusters.Add(GetElementByIndex(index)[0]);
                string seriesStyle = GenerateStyle(chartInstance.SeriesContainer.Renderers[index.Series]);
                selectedElements = FindElementByClass(chartInstance.SvgRenderer, seriesStyle);
                FindTrackballElements(selectedElements, seriesStyle, seriesRenderer.Index);
                if (!chartInstance.IsMultiSelect && selectedElements.Count > 0 && selectedElements.Find(item => item.Id.Contains("Point", StringComparison.InvariantCulture))?.Id != clusters[index.Series].Id)
                {
                    RemoveSelection(index.Series, selectedElements, seriesStyle, true);
                }
            }

            return clusters;
        }

        internal List<SvgClass> FindElements(IChartElementRenderer series, ChartSelectedDataIndex index, string suffix = "")
        {
            if (IsSeriesMode)
            {
                return GetSeriesElements(series);
            }
            else if (CurrentMode == SelectionMode.Cluster)
            {
                return GetClusterElements(index);
            }
            else
            {
                return GetElementByIndex(index, suffix);
            }
        }

        internal void RemoveMultiSelectElements(List<ChartSelectedDataIndex> index, ChartSelectedDataIndex currentIndex)
        {
            foreach (ChartSelectedDataIndex data in index.ToList())
            {
                if ((IsSeriesMode && !ToEquals(data, currentIndex, IsSeriesMode)) || (CurrentMode == SelectionMode.Cluster && !ToEquals(data, currentIndex, false)) || (!IsSeriesMode && ToEquals(data, currentIndex, true) && !ToEquals(data, currentIndex, false)))
                {
                    RemoveStyles(FindElements(chartInstance.SeriesContainer.Renderers[data.Series], data));
                    index.Remove(data);
                }
            }
        }

        internal void SelectionChart(ChartSelectedDataIndex index, List<SvgClass> selectedElements)
        {
            selectedElements.RemoveAll(item => item == null);
            if (CurrentMode != SelectionMode.Lasso)
            {
                if (!chartInstance.IsMultiSelect && !CurrentMode.ToString().Contains("Drag", StringComparison.InvariantCulture) && !StyleId.Contains("highlight", StringComparison.InvariantCulture) && chartInstance.SelectionMode != SelectionMode.None)
                {
                    RemoveMultiSelectElements(SelectedDataIndexes, index);
                }
            }

            if (selectedElements.Count > 0 && selectedElements[0] != null)
            {
                bool isAdd = false;
                ChartSeriesRenderer seriesRenderer = (ChartSeriesRenderer)chartInstance.SeriesContainer.Renderers[index.Series];
                SvgClass parentEle = FindDomElement(chartInstance.SvgRenderer, chartInstance.ID + (seriesRenderer.IsRectSeries() ? "SeriesGroup" : "SymbolGroup") + seriesRenderer.Index.ToString());
#pragma warning restore CA1305
                if (!string.IsNullOrEmpty(selectedElements[0].Class) && CurrentMode != SelectionMode.Cluster)
                {
                    FindTrackballElements(selectedElements, selectedElements[0].Class, seriesRenderer.Index);
                }

                if (selectedElements[0] != null && selectedElements[0].Class?.Contains(GenerateStyle(FindSeriesFromId(selectedElements[0].Id)), StringComparison.InvariantCulture) == true)
                {
                    RemoveStyles(selectedElements);
                }
                else if (parentEle != null && parentEle.Class.Contains(GenerateStyle(seriesRenderer), StringComparison.InvariantCulture) == true)
                {
                    RemoveStyles(new List<SvgClass>() { parentEle });
                }
                else
                {
                    previousSelectedEle = chartInstance.HighlightMode != HighlightMode.None ? selectedElements : new List<SvgClass>();
                    ApplyStyles(selectedElements);
                    isAdd = true;
                }

                if (StyleId.Contains("highlight", StringComparison.InvariantCulture) && chartInstance.HighlightMode != HighlightMode.None)
                {
                    AddOrRemoveIndex(HighlightDataIndexes, index, isAdd);
                }
                else
                {
                    AddOrRemoveIndex(SelectedDataIndexes, index, isAdd);
                }
            }
        }

        private static DateTime GetDatetimeValue(double x_value)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(x_value);
        }

        private void SelectionComplete(ChartSelectedDataIndex index, SelectionMode selectionMode)
        {
            List<Point> points;
            int pointIndex, seriesIndex;
            List<PointXY> selectedPointValues = new List<PointXY>();
            double y_Value;
            object selectedPointX;
            if (selectionMode == SelectionMode.Cluster)
            {
                foreach (ChartSeriesRenderer seriesRenderer in chartInstance.SeriesContainer.Renderers)
                {
                    if (seriesRenderer.Series.Visible)
                    {
                        foreach (ChartSelectedDataIndex dataItem in SelectedDataIndexes)
                        {
                            pointIndex = chartInstance.IsMultiSelect ? dataItem.Point : index.Point;
                            seriesIndex = seriesRenderer.Index;
                            points = seriesRenderer.Points;
                            if (pointIndex >= 0)
                            {
                                y_Value = seriesRenderer.Series.Type != ChartSeriesType.RangeArea ? points[pointIndex].YValue : points[pointIndex].Regions[0].Y;
                                selectedPointX = points[pointIndex].XValue;
                                if (chartInstance.AxisContainer.Axes["PrimaryXAxis"].ValueType == ValueType.Category)
                                {
                                    selectedPointX = points[pointIndex].X.ToString();
                                }
                                else if (chartInstance.AxisContainer.Axes["PrimaryXAxis"].ValueType == ValueType.DateTime)
                                {
                                    selectedPointX = GetDatetimeValue(points[pointIndex].XValue);
                                }

                                if (seriesRenderer.Category() != SeriesCategories.Indicator)
                                {
                                    selectedPointValues.Add(new PointXY()
                                    {
                                        X = selectedPointX,
                                        Y = y_Value,
                                        SeriesIndex = seriesIndex,
                                        PointIndex = pointIndex
                                    });
                                }

                                if (seriesRenderer.Series.Type == ChartSeriesType.RangeArea)
                                {
                                    selectedPointValues.Add(new PointXY()
                                    {
                                        X = selectedPointX,
                                        Y = points[pointIndex].Regions[0].Y,
                                        SeriesIndex = seriesIndex,
                                        PointIndex = pointIndex
                                    });
                                }
                            }
                        }
                    }
                }
            }
            else if (selectionMode == SelectionMode.Series)
            {
                if (chartInstance.IsMultiSelect)
                {
                    foreach (ChartSelectedDataIndex dataItem in SelectedDataIndexes)
                    {
                        seriesIndex = dataItem.Series;
                        selectedPointValues.Add(new PointXY() { SeriesIndex = seriesIndex });
                    }
                }
                else
                {
                    seriesIndex = SelectedDataIndexes.Count > 0 ? SelectedDataIndexes[0].Series : 0;
                    selectedPointValues.Add(new PointXY() { SeriesIndex = seriesIndex });
                }
            }
            else if (selectionMode == SelectionMode.Point)
            {
                foreach (ChartSelectedDataIndex dataItem in SelectedDataIndexes)
                {
                    pointIndex = dataItem.Point;
                    seriesIndex = dataItem.Series;
                    ChartSeriesRenderer seriesRenderer = (ChartSeriesRenderer)chartInstance.SeriesContainer.Renderers[seriesIndex];
                    points = seriesRenderer.Points;
                    if (!double.IsNaN(pointIndex))
                    {
                        selectedPointX = points[pointIndex].XValue;
                        y_Value = seriesRenderer.Series.Type != ChartSeriesType.RangeArea ? points[pointIndex].YValue : points[pointIndex].Regions[0].Y;
                        if (chartInstance.AxisContainer.Axes["PrimaryXAxis"].ValueType == ValueType.Category)
                        {
                            selectedPointX = points[pointIndex].X.ToString();
                        }
                        else if (chartInstance.AxisContainer.Axes["PrimaryXAxis"].ValueType == ValueType.DateTime)
                        {
                            selectedPointX = GetDatetimeValue(points[pointIndex].XValue);
                        }

                        selectedPointValues.Add(new PointXY()
                        {
                            X = selectedPointX,
                            Y = y_Value,
                            SeriesIndex = seriesIndex,
                            PointIndex = pointIndex
                        });
                    }
                }
            }

            chartInstance.ChartEvents?.OnSelectionChanged?.Invoke(new SelectionCompleteEventArgs() { SelectedDataValues = selectedPointValues, Cancel = false, Name = Constants.ONSELECTIONCHANGED });
        }

        internal void SelectionModeChanged()
        {
            chartInstance.ShouldAnimateSeries = false;
            if (!chartInstance.SelectionMode.ToString().Contains("Drag", StringComparison.InvariantCulture))
            {
                OnPropertyChanged();
            }
            else
            {
                ClearDraggedRects();
                OnPropertyChanged();
                chartInstance.ParentRect.ClearElements();
            }
        }

        internal void SelectionPatternChanged()
        {
            chartInstance.ShouldAnimateSeries = false;
        }

        internal void OnPropertyChanged()
        {
            chartInstance.ShouldAnimateSeries = false;
            SelectionMode oldMode = CurrentMode;
            CurrentMode = chartInstance.SelectionMode;
            StyleId = chartInstance.ID + "_ej2_chart_selection";
            RedrawSelection(oldMode);
        }
        internal void ClearDraggedRects()
        {
            dragRectArray.Clear();
            filterArray.Clear();
        }
    }
}
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;
using System;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Linq;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class AccumulationChartSelection : BaseSelection
    {
        internal bool IsHighlight { get; set; }

        internal List<AccumulationChartSelectedDataIndex> SelectedDataIndexes { get; set; } = new List<AccumulationChartSelectedDataIndex>();

        private SfAccumulationChart chartInstance { get; set; }

        private List<SvgPath> previousSelectedEle { get; set; } = new List<SvgPath>();

        private List<AccumulationChartSelectedDataIndex> highlightDataIndexes { get; set; } = new List<AccumulationChartSelectedDataIndex>();

        internal AccumulationChartSelection(SfAccumulationChart chart)
        {
            chartInstance = chart;
        }

        private void InitPrivateVariables()
        {
            StyleId = chartInstance.ID + "_blazor_chart_selection";
            Unselected = chartInstance.ID + "_blazor_deselected";
            ReqPatterns?.Clear();
        }

        internal void InvokeSelection()
        {
            InitPrivateVariables();
            if (chartInstance.SelectionMode != AccumulationSelectionMode.None)
            {
                SeriesStyles();
            }

            if (chartInstance.HighlightMode != AccumulationSelectionMode.None)
            {
                StyleId = StyleId.Replace("selection", "highlight", StringComparison.Ordinal);
                SeriesStyles();
            }

            BaseSelection.ApppendSelectionPattern();
        }

        internal void ProcessSelectedData()
        {
            List<AccumulationChartSelectedDataIndex> items = new List<AccumulationChartSelectedDataIndex>();
            items.AddRange(chartInstance.SelectedDataIndexes);
            items.AddRange(SelectedDataIndexes);
            DoSelectedData(items);
        }

        private void SeriesStyles()
        {
            string pointClass = string.Empty;
            if (chartInstance.SelectionPattern != SelectionPattern.None || chartInstance.HighlightPattern != SelectionPattern.None)
            {
                foreach (AccumulationChartSeries series in chartInstance.VisibleSeries)
                {
                    SelectionPattern patternName = StyleId.IndexOf("highlight", StringComparison.Ordinal) > 0 ? chartInstance.HighlightPattern : chartInstance.SelectionPattern;
                    foreach (AccumulationPoints point in series.Points)
                    {
                        string pattern = "{ fill:" + FindPattern(point.Color, point.Index, patternName, series.Opacity) + "}";
                        pointClass = string.Concat(pointClass, !string.IsNullOrEmpty(series.SelectionStyle) ? series.SelectionStyle : StyleId + "_series_" + 0 + "[id$='Point_" + point.Index + "'],." + StyleId + "_series_" + 0 + "[id$='shape_" + point.Index + "']");
                        pattern = (pattern.IndexOf("None", StringComparison.Ordinal) > -1) ? "{}" : pattern;
                        InnerHTML += !string.IsNullOrEmpty(series.SelectionStyle) ? string.Empty : "." + pointClass + ",." + StyleId + "_series_0#" + chartInstance.ID + "PointHoverBorder" + pattern;
                        pointClass = string.Empty;
                    }
                }
            }

            InnerHTML += "." + Unselected + " { opacity: 0.3;} ";
            chartInstance.StyleElementInstance?.AppendStyleElement(InnerHTML);
        }

        private void DoSelectedData(List<AccumulationChartSelectedDataIndex> selecteddata)
        {
            foreach (AccumulationChartSelectedDataIndex item in selecteddata)
            {
                SvgPath selectedPoint = chartInstance.Rendering.PathElementList.Find(x => x.Id == chartInstance.ID + "_Series_" + item.Series + "_Point_" + item.Point);
                if (selectedPoint != null)
                {
                    PerformSelection(item, chartInstance.ID + "_Series_" + item.Series + "_Point_" + item.Point);
                }
            }
        }

        internal void CalculateSelectedElements(ChartInternalMouseEventArgs events)
        {
            SvgClass targetElement = FindDomElement(chartInstance.Rendering, events.Target);
            StyleId = IsHighlight ? StyleId.Replace("selection", "highlight", StringComparison.Ordinal) : StyleId;
            if ((!events.Target.Contains(chartInstance.ID + "_", StringComparison.Ordinal)) ||
                (events.Type == "mousemove" && (targetElement?.Class?.Contains("highlight", StringComparison.InvariantCulture) == true || targetElement?.Class?.Contains("selection", StringComparison.InvariantCulture) == true)))
            {
                return;
            }

            IsAlreadySelected(events);
            if (events.Target.Contains("_Series_", StringComparison.Ordinal) || events.Target.Contains("_datalabel_", StringComparison.Ordinal))
            {
                int[] dataIndex = ChartHelper.IndexFinder(events.Target, true);
#pragma warning disable CA2000
                if (dataIndex[0] != -1 && dataIndex[1] != -1)
                {
                    PerformSelection(AccumulationChartSelectedDataIndex.CreateSelectedData(dataIndex[1], dataIndex[0]), events.Target);
                }
            }
        }

        private bool IsAlreadySelected(ChartInternalMouseEventArgs events)
        {
            SvgClass targetElem = FindDomElement(chartInstance.Rendering, events.Target);
            if (events.Type == "click")
            {
                IsHighlight = false;
                StyleId = chartInstance.ID + "_blazor_chart_selection";
            }
            else if (events.Type == "mousemove")
            {
                IsHighlight = true;
                highlightDataIndexes.Clear();
                StyleId = chartInstance.ID + "_blazor_chart_highlight";
            }

            if (chartInstance.HighlightMode != AccumulationSelectionMode.None && chartInstance.SelectionMode == AccumulationSelectionMode.None && events.Type == "click")
            {
                return false;
            }

            if (chartInstance.HighlightMode != AccumulationSelectionMode.None && previousSelectedEle.Count != 0 && previousSelectedEle[0] != null)
            {
                previousSelectedEle.RemoveAll(item => item == null);
                bool isElement = targetElem != null && targetElem.Id.Contains("Point", StringComparison.InvariantCulture);
                foreach (SvgClass svgVal in previousSelectedEle)
                {
                    if (!string.IsNullOrEmpty(svgVal.Class))
                    {
                        int[] data = ChartHelper.IndexFinder(svgVal.Id);
                        if (svgVal.Class.Contains("highlight", StringComparison.InvariantCulture) && (isElement || events.Type == "click"))
                        {
                            svgVal.ChangeClass(string.Empty, null, true);
                            AddOrRemoveIndex(highlightDataIndexes, AccumulationChartSelectedDataIndex.CreateSelectedData(data[1], data[0]));
                        }
                        else if (svgVal.Class.Contains("highlight", StringComparison.InvariantCulture) && !isElement)
                        {
                            PerformSelection(AccumulationChartSelectedDataIndex.CreateSelectedData(data[1], data[0]), svgVal.Id);
                        }
                    }
                }
            }

            return true;
        }

        private void PerformSelection(AccumulationChartSelectedDataIndex index, string elementId)
        {
            if (elementId.Contains("datalabel", StringComparison.InvariantCulture))
            {
                elementId = elementId.Replace("text", "Point", StringComparison.InvariantCulture).Replace("_datalabel", string.Empty, StringComparison.InvariantCulture);
            }

            SvgPath element = (SvgPath)FindDomElement(chartInstance.Rendering, elementId);
            if ((IsHighlight ? chartInstance.HighlightMode : chartInstance.SelectionMode) == AccumulationSelectionMode.Point && !double.IsNaN(index.Point))
            {
                Selection(index, new List<SvgPath> { element });
                BlurEffect();
            }
        }

        internal void LegendSelection(int pointIndex, ChartInternalMouseEventArgs e)
        {
            SvgClass targetElement = chartInstance.Rendering.PathElementList.Find(item => item.Id == chartInstance.ID + "_Series_0_Point_" + pointIndex);
            if (e.Type == "mousemove")
            {
                if (e.Target.Contains("text", StringComparison.InvariantCulture))
                {
                    targetElement = FindDomElement(chartInstance.Rendering, e.Target.Replace("text", "shape", StringComparison.InvariantCulture));
                }

                if (targetElement?.Class?.Contains("highlight", StringComparison.InvariantCulture) == true || targetElement?.Class?.Contains("selection", StringComparison.InvariantCulture) == true)
                {
                    return;
                }

                IsHighlight = true;
            }

            SvgPath pointElement = chartInstance.Rendering.PathElementList.Find(item => item.Id == chartInstance.ID + "_Series_0_Point_" + pointIndex);
            Selection(AccumulationChartSelectedDataIndex.CreateSelectedData(pointIndex, 0), new List<SvgPath>() { pointElement });
#pragma warning restore CA2000
            BlurEffect();
            if (e.Type == "click")
            {
                // TODO:
            }
        }

        private void Selection(AccumulationChartSelectedDataIndex index, List<SvgPath> selectedElements)
        {
            if (!chartInstance.IsMultiSelect)
            {
                RemoveMultiSelectElements(!IsHighlight ? SelectedDataIndexes : highlightDataIndexes, index);
            }

            string className = selectedElements[0] != null ? selectedElements[0].Class : null;
            SvgPath borderElement = chartInstance.Rendering.PathElementList.Find(item => item.Id == chartInstance.ID + "PointHoverBorder");
            if (selectedElements[0] != null && className?.IndexOf(GetSelectionClass(), StringComparison.Ordinal) > -1)
            {
                RemoveStyles(selectedElements, index);
                AddOrRemoveIndex(!IsHighlight ? SelectedDataIndexes : highlightDataIndexes, index);
                if (chartInstance.EnableBorderOnMouseMove && borderElement != null)
                {
                    RemoveSvgClass(borderElement, borderElement.Class);
                }
            }
            else
            {
                previousSelectedEle = chartInstance.HighlightMode != AccumulationSelectionMode.None ? selectedElements : new List<SvgPath>();
                ApplyStyles(selectedElements, index);
                if (chartInstance.EnableBorderOnMouseMove && borderElement != null)
                {
                    AddSvgClass(borderElement, selectedElements[0]?.Class);
                }

                AddOrRemoveIndex(!IsHighlight ? SelectedDataIndexes : highlightDataIndexes, index, true);
            }
        }

        internal void RedrawSelection()
        {
            (!IsHighlight ? SelectedDataIndexes : highlightDataIndexes).ForEach(index => RemoveStyles(FindElements(index), index));
            BlurEffect();
            (!IsHighlight ? SelectedDataIndexes : highlightDataIndexes).ForEach(index => PerformSelection(index, FindElements(index)[0]?.Id));
        }

        private void RemoveMultiSelectElements(List<AccumulationChartSelectedDataIndex> index, AccumulationChartSelectedDataIndex currentIndex)
        {
            foreach (AccumulationChartSelectedDataIndex item in index.ToList())
            {
                if (!item.Equals(currentIndex))
                {
                    RemoveStyles(FindElements(item), item);
                    index.Remove(item);
                }
            }
        }

        private List<SvgPath> FindElements(AccumulationChartSelectedDataIndex index)
        {
            return chartInstance.Rendering.PathElementList.FindAll(item => item.Id == chartInstance.ID + "_Series_" + index.Series + "_Point_" + index.Point);
        }

        private void ApplyStyles(List<SvgPath> elements, AccumulationChartSelectedDataIndex index)
        {
            AccumulationChartTooltip accumulationTooltip = chartInstance.AccumulationTooltipModule;
            foreach (SvgPath element in elements)
            {
                if (element != null)
                {
                    if (chartInstance.AccumulationLegendModule != null && chartInstance.LegendSettings.Visible)
                    {
                        SvgClass legendShape = FindDomElement(chartInstance.Rendering, chartInstance.ID + "_chart_legend_shape_" + index.Point);
                        RemoveSvgClass(legendShape, Unselected);
                        AddSvgClass(legendShape, GetSelectionClass());
                    }

                    RemoveSvgClass(element, Unselected);
                    double opacity = accumulationTooltip != null && accumulationTooltip.SvgTooltip != null && (accumulationTooltip.AccPreviousPoints.Count > 0 &&
                                accumulationTooltip.AccPreviousPoints[0].Point.Index != index.Point) ? accumulationTooltip.SvgTooltip.Opacity : chartInstance.VisibleSeries[index.Series].Opacity;
                    element.ChangeOpacity(opacity);
                    AddSvgClass(element, GetSelectionClass());
                }
            }
        }

        private void RemoveStyles(List<SvgPath> elements, AccumulationChartSelectedDataIndex index)
        {
            AccumulationChartTooltip accumulationTooltip = chartInstance.AccumulationTooltipModule;
            foreach (SvgPath element in elements)
            {
                if (element != null)
                {
                    if (chartInstance.AccumulationLegendModule != null && chartInstance.LegendSettings.Visible)
                    {
                        SvgClass legendShape = FindDomElement(chartInstance.Rendering, chartInstance.ID + "_chart_legend_shape_" + index.Point);
                        RemoveSvgClass(legendShape, GetSelectionClass());
                    }

                    double opacity = accumulationTooltip != null && accumulationTooltip.SvgTooltip != null && (accumulationTooltip.AccPreviousPoints[0]?.Point.Index == index.Point) ? accumulationTooltip.SvgTooltip.Opacity : chartInstance.VisibleSeries[0].Opacity;
                    element.ChangeOpacity(opacity);
                    RemoveSvgClass(element, GetSelectionClass());
                }
            }
        }

        private string GetSelectionClass()
        {
            return (!string.IsNullOrEmpty(chartInstance.Series[0].SelectionStyle) ? chartInstance.Series[0].SelectionStyle : StyleId + "_series_") + 0;
        }

        private static void AddOrRemoveIndex(List<AccumulationChartSelectedDataIndex> indexes, AccumulationChartSelectedDataIndex index, bool isAdd = false)
        {
            foreach (AccumulationChartSelectedDataIndex item in indexes.ToList())
            {
                if (item.Equals(index))
                {
                    indexes.Remove(item);
                }
            }

            if (isAdd)
            {
                indexes.Add(index);
            }
        }

        private void BlurEffect()
        {
            foreach (AccumulationChartSeries series in chartInstance.VisibleSeries)
            {
                if (series.Visible)
                {
                    CheckSelectionElements((!string.IsNullOrEmpty(series.SelectionStyle) ? series.SelectionStyle : StyleId + "_series_") + 0, CheckPointVisibility(!IsHighlight ? SelectedDataIndexes : highlightDataIndexes));
                }
            }
        }

        private bool CheckPointVisibility(List<AccumulationChartSelectedDataIndex> selectedDataIndexes)
        {
            bool visible = false;
            foreach (AccumulationChartSelectedDataIndex data in selectedDataIndexes)
            {
                if (chartInstance.VisibleSeries[0].Points.Find(item => item.Index == data.Point) != null)
                {
                    visible = true;
                    break;
                }
            }

            return visible;
        }

        private void CheckSelectionElements(string className, bool visibility, bool legendClick = true)
        {
            List<SvgPath> pointElements = chartInstance.Rendering.PathElementList.FindAll(item => item.Id.Contains("_Series_", StringComparison.Ordinal) && item.Id.Contains("_Point_", StringComparison.Ordinal));
            foreach (SvgPath pointElement in pointElements)
            {
                string elementClass = !string.IsNullOrEmpty(pointElement.Class) ? pointElement.Class : string.Empty;
                if (chartInstance.SelectionMode != AccumulationSelectionMode.None && chartInstance.HighlightMode != AccumulationSelectionMode.None)
                {
                    className = elementClass.Contains("selection", StringComparison.InvariantCulture) || elementClass.Contains("highlight", StringComparison.InvariantCulture) ? elementClass : className;
                }

                if (!elementClass.Contains(className, StringComparison.Ordinal) && visibility)
                {
                    AddSvgClass(pointElement, Unselected);
                }
                else
                {
                    RemoveSvgClass(pointElement, Unselected);
                }

                if (chartInstance.AccumulationLegendModule != null && chartInstance.LegendSettings.Visible)
                {
                    SvgClass legendShape = FindDomElement(chartInstance.Rendering, chartInstance.ID + "_chart_legend_shape_" + pointElements.IndexOf(pointElement));
                    if (legendShape != null)
                    {
                        RemoveSvgClass(legendShape, legendShape.Class);
                        if (!elementClass.Contains(className, StringComparison.Ordinal) && visibility)
                        {
                            AddSvgClass(legendShape, Unselected);
                            RemoveSvgClass(legendShape, className);
                        }
                        else
                        {
                            RemoveSvgClass(legendShape, Unselected);
                        }

                        if (legendClick && pointElement.Class.Contains(className, StringComparison.InvariantCulture))
                        {
                            AddSvgClass(legendShape, className);
                        }
                    }
                }
            }
        }

        internal override void Dispose()
        {
            base.Dispose();
            SelectedDataIndexes = null;
            highlightDataIndexes = null;
            previousSelectedEle = null;
            chartInstance = null;
        }
    }
}
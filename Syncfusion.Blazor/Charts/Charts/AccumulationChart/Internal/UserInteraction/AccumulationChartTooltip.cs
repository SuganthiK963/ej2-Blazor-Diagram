using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Timers;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class AccumulationChartTooltip : TooltipBase
    {
        private bool isTooltip { get; set; }

        internal Timer Timer { get; set; } = new Timer();

        internal AccumulationChartTooltip(SfAccumulationChart chart)
            : base(chart)
        {
        }

        internal void MouseMoveHandler(string targetID)
        {
            if (AccChartInstance.Tooltip.Enable && ChartHelper.WithInBounds(AccChartInstance.MouseX, AccChartInstance.MouseY, AccChartInstance.InitialClipRect))
            {
                Tooltip(targetID);
            }
        }

        internal void MouseUpHandler(string targetID)
        {
            if (AccChartInstance.Tooltip.Enable && ChartHelper.WithInBounds(AccChartInstance.MouseX, AccChartInstance.MouseY, AccChartInstance.InitialClipRect))
            {
                Tooltip(targetID);
                RemoveTooltip(2000);
            }
        }

        internal void MouseLeaveHandler()
        {
            RemoveTooltip(AccChartInstance.Tooltip.FadeOutDuration);
        }

        private void Tooltip(string targetID)
        {
            GetTooltipElement(isTooltip);
            RenderSeriesTooltip(!isTooltip, targetID);
        }

        private void RenderSeriesTooltip(bool isFirst, string targetID)
        {
            AccPointData data = GetPieData(targetID);
            AccCurrentPoints.Clear();
            if (data.Point != null && (AccPreviousPoints.Count == 0 || AccPreviousPoints?.First().Point != data.Point))
            {
                Timer.Stop();
                if (AccPreviousPoints.Count != 0 && data.Point.Index == AccPreviousPoints?.First().Point.Index && data.Series.Index == AccPreviousPoints?.First().Series.Index)
                {
                    return;
                }

                if (PushData(data))
                {
                    TriggerTooltipRender(data, isFirst, GetTooltipText(data, AccChartInstance.Tooltip), FindHeader(data));
                }
            }
            else if (data.Point == null && IsRemove)
            {
                RemoveTooltip(AccChartInstance.Tooltip.FadeOutDuration);
                IsRemove = false;
            }
        }

        private void TriggerTooltipRender(AccPointData point, bool isFirst, string textCollection, string headerText)
        {
            string template = string.Empty;
            PointInfo data = new PointInfo()
            {
                PointX = point.Point.X,
                PointY = point.Point.Y,
                SeriesIndex = point.Series.Index,
                SeriesName = point.Series.Name,
                PointIndex = point.Point.Index,
                PointText = point.Point.Text
            };
            TooltipRenderEventArgs argsData = new TooltipRenderEventArgs(Constants.TOOLTIPRENDER, false, data, headerText, point.Point, point.Series, template, textCollection, new ChartDefaultFont()  /*AccChartInstance.Tooltip.TextStyle*/);
            if (AccChartInstance.AccumulationChartEvents?.TooltipRender != null)
            {
                AccChartInstance.AccumulationChartEvents.TooltipRender.Invoke(argsData);
            }

            if (!argsData.Cancel)
            {
                HeaderText = argsData.HeaderText;
                FormattedText.Add(argsData.Text);
                Text = FormattedText;
                AccumulationChartDataPointInfo accTemplate = new AccumulationChartDataPointInfo(point.Point.X, point.Point.Y, point.Point.Percentage, point.Point.Label, argsData.Data);
                if (AccChartInstance.Tooltip.Template != null)
                {
                    TemplateTooltip.ChangeContent(AccChartInstance.Tooltip.Template, point.Point.SymbolLocation, null, accTemplate);
                }
                else
                {
                    CreateTooltip(null, AccChartInstance, isFirst, point.Point.SymbolLocation, new ChartInternalLocation(0, 0),
                        null, new TooltipShape[] { TooltipShape.Circle }, 0, AccChartInstance.InitialClipRect, null, new List<AccPointData>());
                }

                isTooltip = true;
            }

            IsRemove = true;
        }

        private bool PushData(AccPointData data)
        {
            if (data.Series.EnableTooltip)
            {
                AccCurrentPoints.Add(data);
                return true;
            }

            return false;
        }

        private AccPointData GetPieData(string id)
        {
            int[] index = ChartHelper.IndexFinder(id, true);
            if (index[0] != -1 && index[1] != -1 && !double.IsNaN(index[0]) && !double.IsNaN(index[1]))
            {
                AccumulationChartSeries series = AccChartInstance.VisibleSeries[0];
                if (series.EnableTooltip)
                {
                    return new AccPointData(series.Points[index[1]], series);
                }
            }

            return new AccPointData(null, null);
        }

        private string GetTooltipText(AccPointData data, AccumulationChartTooltipSettings tooltip)
        {
            return ParseTemplate(data.Point, data.Series, tooltip.Format != null ? tooltip.Format : AccChartInstance.EnableGroupingSeparator ? "${point.x} : <b>${point.separatorY}</b>" : "${point.x} : <b>${point.y}</b>");
        }

        private string FindHeader(AccPointData data)
        {
            if (string.IsNullOrEmpty(Header))
            {
                return string.Empty;
            }

            Header = ParseTemplate(data.Point, data.Series, Header);
            if (!string.IsNullOrEmpty(Header.Replace("<b>", string.Empty, StringComparison.Ordinal).Replace("</b>", string.Empty, StringComparison.Ordinal).Trim()))
            {
                return Header;
            }

            return string.Empty;
        }

        private static string ParseTemplate(AccumulationPoints point, AccumulationChartSeries series, string format)
        {
            PropertyInfo[] pointInfo = point.GetType().GetProperties();
            PropertyInfo[] seriesInfo = series.GetType().GetProperties();
            foreach (PropertyInfo dataValue in pointInfo)
            {
#pragma warning disable CA1304
                Regex val = new Regex("${point" + '.' + dataValue.Name.ToLower() + '}', RegexOptions.Multiline);
#pragma warning restore CA1304
                format = format.Replace(val.ToString(), Convert.ToString(dataValue.GetValue(point), CultureInfo.InvariantCulture), StringComparison.Ordinal);
            }

            foreach (PropertyInfo dataValue in seriesInfo)
            {
#pragma warning disable CA1308
                Regex val = new Regex("${series." + dataValue.Name.ToLowerInvariant() + '}', RegexOptions.Multiline);
#pragma warning restore CA1308
                string textValue = Convert.ToString(dataValue.GetValue(series), null);
                format = format.Replace(val.ToString(), textValue, StringComparison.Ordinal);
            }

            return format;
        }

        private void RemoveTooltip(double fadeOutDuration)
        {
            Timer.Stop();
            Timer.Interval = fadeOutDuration;
            Timer.AutoReset = false;
            if ((AccChartInstance.TooltipContent != null && AccPreviousPoints.Count > 0) || AccChartInstance.Tooltip.Template != null)
            {
                Timer.Elapsed += OnTimeOut;
                Timer.Start();
            }
        }

        private async void OnTimeOut(object source, ElapsedEventArgs e)
        {
            AccCurrentPoints.Clear();
            RemoveHighlight();
            if (AccChartInstance?.Tooltip?.Template != null)
            {
                TemplateTooltip?.TemplateFadeOut();
            }
            else
            {
                await AccChartInstance?.InvokeMethod(AccumulationChartConstants.FADEOUT, new object[] { AccChartInstance.Element });
            }
            AccPreviousPoints.Clear();
            isTooltip = false;
            (source as Timer).Stop();
        }

        internal void RemoveHighlight()
        {
            for (int i = 0, len = AccPreviousPoints.Count; i < len; i++)
            {
                if (AccPreviousPoints[i].Series.Visible)
                {
                    HighlightPoint(AccPreviousPoints[i], false);
                }
            }
        }

        private void HighlightPoint(AccPointData data, bool highlight)
        {
            SvgPath pointElement = AccChartInstance.Rendering?.PathElementList.Find(item => item.Id == data.Point.Id);
            AccumulationChartSelection selectionModule = AccChartInstance.AccumulationSelectionModule;
            bool isSelectedElement = selectionModule != null && selectionModule.SelectedDataIndexes.Count > 0;
            if (pointElement != null)
            {
                if (!isSelectedElement || isSelectedElement && pointElement.Class?.IndexOf("_blazor_chart_selection_series_", StringComparison.Ordinal) == -1)
                {
                    pointElement.ChangeOpacity(highlight ? data.Series.Opacity / 2 : data.Series.Opacity);

                    // await AccChartInstance.InvokeMethod(AccumulationChartConstants.SetAttribute, new object[] { data.Point.id, "opacity", (highlight ? data.Series.Opacity / 2 : data.Series.Opacity) });
                }
                else
                {
                    pointElement.ChangeOpacity(data.Series.Opacity);

                    // await AccChartInstance.InvokeMethod(AccumulationChartConstants.SetAttribute, new object[] { data.Point.id, "opacity", data.Series.Opacity });
                }
            }
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void TooltipAnimationComplete()
        {
            RemoveHighlight();
            SvgTooltip = null;
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void TooltipRender()
        {
            RemoveHighlight();
            AccCurrentPoints.ForEach(item => HighlightPoint(item, true));
        }

        internal override void Dispose()
        {
            base.Dispose();
            Timer.Stop();
            Timer = null;
        }
    }
}
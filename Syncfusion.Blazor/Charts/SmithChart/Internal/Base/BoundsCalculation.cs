using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.SmithChart.Internal;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSmithChart : SfDataBoundComponent
    {
        private string titleText;
        private string subtitleText;
        private bool isResize;
        private StringComparison comparison = StringComparison.InvariantCulture;

        internal SmithChartHelper Helper { get; set; } = new SmithChartHelper();

        internal ElementReference Element { get; set; }

        internal Rect Bounds { get; set; } = new Rect();

        internal Rect LegendBounds { get; set; } = new Rect();

        internal NumberFormatInfo NumberFormatter { get; set; } = new NumberFormatInfo();

        internal DomRect ElementOffset { get; set; }

        internal Size AvailableSize { get; set; }

        internal Rect ChartArea { get; set; } = new Rect();

        internal List<SmithChartSeries> VisibleSeries { get; set; } = new List<SmithChartSeries>();

        internal async Task<Size> SetContainerSize()
        {
            if (isResize)
            {
                ElementOffset = await InvokeMethod<DomRect>(SmithChartConstants.GETELEMENTBOUNDSBYID, false, new object[] { Element, Height, Width });
                isResize = false;
            }

            Size containerSize = new Size(ElementOffset.Width, ElementOffset.Height);
            AvailableSize = new Size(containerSize.Width > 0 ? containerSize.Width : 600, containerSize.Height > 0 ? containerSize.Height : 450);
            return AvailableSize;
        }

        private void CalculateVisibleSeries()
        {
            VisibleSeries.Clear();
            VisibleSeries = new List<SmithChartSeries>();
            string[] colors = DataVizCommonHelper.GetSeriesColor(Theme.ToString());
            int seriesIndex = -1;
            foreach (SmithChartSeries series in Series)
            {
                series.Index = ++seriesIndex;
                series.Interior = series.Fill != null ? series.Fill : colors[seriesIndex % colors.Length];
                series.ActualPoints = new List<SmithChartPoint>();
                GetSeriesActualPoints(series);
                VisibleSeries.Add(series);
            }
        }

        private void GetSeriesActualPoints(SmithChartSeries series)
        {
            if (series.DataSource != null && !string.IsNullOrEmpty(series.Resistance) && !string.IsNullOrEmpty(series.Reactance))
            {
                object[] currentViewData = series.CurrentViewData.ToArray();
                Type type = series.CurrentViewData.ToArray().First().GetType();
                for (int i = 0; i < currentViewData.Length; i++)
                {
                    series.ActualPoints.Add(DataPoint(series, i, type, currentViewData[i]));
                }
            }
            else
            {
                series.ActualPoints = series.Points.ToList();
            }

        }

        private SmithChartPoint DataPoint(SmithChartSeries series, int i, Type type, object currentData)
        {
            if (series.ActualPoints.Count < i)
            {
                return null;
            }

            SmithChartPoint point = new SmithChartPoint();
            if (type.Name == "JObject")
            {
                JObject o = (JObject)currentData;
                point.Reactance = (double)o.GetValue(series.Reactance, comparison);
                point.Resistance = (double)o.GetValue(series.Resistance, comparison);
                point.Tooltip = (string)o.GetValue(series.TooltipMappingName, comparison);
            }
            else
            {
                PropertyInfo reactance = type.GetProperty(series.Reactance), resistance = type.GetProperty(series.Resistance), tooltipProp = type.GetProperty(series.TooltipMappingName);
                point.Reactance = reactance != null ? Convert.ToDouble(reactance.GetValue(currentData), null) : double.NaN;
                point.Resistance = resistance != null ? Convert.ToDouble(resistance.GetValue(currentData), null) : double.NaN;
                point.Tooltip = tooltipProp != null ? Convert.ToString(tooltipProp.GetValue(currentData), null) : null;
            }

            return point;
        }

        private void ProcessData()
        {
            RefreshChart();
            TriggerLoadedEvent();
        }

        internal void RefreshChart()
        {
            SmithChartLegendModule = new SmithChartLegend(this, SmithChartThemeStyle);
            CalculateBounds();
            InstanceInitialization();
            CreateChart();
        }

        private void CalculateBounds()
        {
            Bounds = new Rect(Margin.Left, Margin.Top, AvailableSize.Width, AvailableSize.Height);
            titleText = subtitleText = string.Empty;
            double titleHeight = 0, subTitleHeight = 0, titleWidth, maxWidth = 0;
            if (Title.Visible && !string.IsNullOrEmpty(Title.Text))
            {
                titleText = Title.EnableTrim ? SmithChartHelper.TextTrim(Bounds.Width, Title.Text, Title.TextStyle) : Title.Text;
                titleHeight = SmithChartHelper.MeasureText(Title.Text, Title.TextStyle).Height;
                if (Title.Subtitle.Visible && !string.IsNullOrEmpty(Title.Subtitle.Text))
                {
                    titleWidth = SmithChartHelper.MeasureText(titleText, Title.TextStyle).Width;
                    maxWidth = titleWidth > maxWidth ? titleWidth : maxWidth;
                    subtitleText = Title.Subtitle.EnableTrim ? SmithChartHelper.TextTrim(maxWidth, Title.Subtitle.Text, Title.Subtitle.TextStyle) : Title.Subtitle.Text;
                    subTitleHeight = SmithChartHelper.MeasureText(Title.Subtitle.Text, Title.Subtitle.TextStyle).Height;
                }
            }

            SmithChartHelper.SubtractRect(Bounds, new Rect(0, subTitleHeight + titleHeight, Margin.Right + Margin.Left, Margin.Top + Margin.Bottom));
            if (LegendSettings.Visible)
            {
                LegendBounds = SmithChartLegendModule.CalculateLegendBounds();
            }

            Bounds = CalculateAreaBounds();
        }

        private double[] GetLegendSpace()
        {
            LegendPosition position = LegendSettings.Position;
            double subtitleHeight = 0, modelsubTitleHeight = 0, topLegendHeight = 0, bottomLegendHeight = 0, space,
            modelTitleHeight = 0, itemPadding = 10, legendBorder = LegendSettings.Border.Width, leftLegendWidth = 0, rightLegendWidth = 0;
            if (LegendSettings.Visible)
            {
                space = LegendBounds.Width + (itemPadding / 2) + ElementSpacing + (2 * legendBorder);
                leftLegendWidth = position == LegendPosition.Left ? space : 0;
                rightLegendWidth = position == LegendPosition.Right ? space : 0;
                topLegendHeight = position == LegendPosition.Top ? LegendBounds.Height : 0;
                bottomLegendHeight = position == LegendPosition.Bottom ? ElementSpacing + LegendBounds.Height : 0;
            }

            subtitleHeight = SmithChartHelper.MeasureText(Title.Subtitle.Text, Font).Height;
            modelsubTitleHeight = (string.IsNullOrEmpty(Title.Subtitle.Text) || !Title.Subtitle.Visible) ? 0 : subtitleHeight;
            return new double[] { leftLegendWidth, rightLegendWidth, topLegendHeight, bottomLegendHeight, modelTitleHeight, modelsubTitleHeight };
        }

        private Rect CalculateAreaBounds()
        {
            double x, y, width, height, rightSpace;
            double[] spaceValue = GetLegendSpace();
            x = Convert.ToDouble(spaceValue[0]) + Margin.Left + Border.Width;
            rightSpace = Convert.ToDouble(spaceValue[1]) + Margin.Left + Margin.Right + (2 * Border.Width);
            width = AvailableSize.Width - (x + rightSpace);
            y = Margin.Top + (2 * ElementSpacing) + Convert.ToDouble(spaceValue[4]) + Convert.ToDouble(spaceValue[5]) + Convert.ToDouble(spaceValue[2]) + Border.Width;
            height = AvailableSize.Height - (Convert.ToDouble(spaceValue[4]) + (2 * ElementSpacing) + Convert.ToDouble(spaceValue[5]) + Margin.Top + Convert.ToDouble(spaceValue[2]) + Convert.ToDouble(spaceValue[3]));
            return new Rect(x, y, width, height);
        }
    }
}
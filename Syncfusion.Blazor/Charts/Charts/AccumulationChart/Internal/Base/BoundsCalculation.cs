using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The accumulation chart control is used to visualize the data in the different types of graphical representations with built-in features like Grouping, legends, tooltip, and more.
    /// </summary>
    public partial class SfAccumulationChart : SfDataBoundComponent, IAccumulationChart
    {
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ChartHelper ChartHelper { get; set; } = new ChartHelper();

        internal Rect InitialClipRect { get; set; } = new Rect();

        internal Size AvailableSize { get; set; } = new Size(600, 450);

        internal List<AccumulationChartSeries> VisibleSeries { get; set; } = new List<AccumulationChartSeries>();

        internal ChartInternalLocation Origin { get; set; }

        internal double ExplodeDistance { get; set; }

        internal bool LegendClickRedraw { get; set; }

        internal bool ControlRendered { get; set; }

        internal NumberFormatInfo NumberFormatter { get; set; } = new NumberFormatInfo();

        internal DomRect ElementOffset { get; set; } = new DomRect();

        internal TrimTooltipBase TooltipBase { get; set; }

        internal StyleElement StyleElementInstance { get; set; }

        internal AccumulationType AccType
        {
            get
            {
                if (Series.Count != 0)
                {
                    return Series.First().Type;
                }
                else
                {
                    return AccumulationType.Pie;
                }
            }
        }

        private bool shouldRender { get; set; } = true;

        internal ElementReference Element { get; set; }

        private int legendChangeDuration { get; set; }

        private List<string> titleCollection { get; set; }

        private List<string> subTitleCollection { get; set; }

        private TemplateDataLabel datalabelTemplate { get; set; }

        private AnnotationElements annotationRender { get; set; }

        internal DomRect SecondaryElementOffset { get; set; }

        private void SetContainerSize()
        {
            Size ContainerSize = new Size(ElementOffset.Width, ElementOffset.Height);
            double height = ChartHelper.StringToNumber(Height, ContainerSize.Height);
            double width = ChartHelper.StringToNumber(Width, ContainerSize.Width);
            AvailableSize = new Size(
                  width > 0 ? width : ContainerSize.Width > 0 ? ContainerSize.Width : AvailableSize.Width,
                  height > 0 ? height : ContainerSize.Height > 0 ? ContainerSize.Height : AvailableSize.Height);
        }

        private void CalculateVisibleSeries()
        {
            VisibleSeries.Clear();
            int seriesIndex = -1;
            foreach (AccumulationChartSeries accSeries in Series)
            {
                if (Series.First().Type == accSeries.Type)
                {
                    accSeries.Index = seriesIndex++;
                    VisibleSeries.Add(accSeries);
                    break;
                }
            }
        }

        private void ProcessData()
        {
            foreach (AccumulationChartSeries seriesValue in VisibleSeries)
            {
                if (seriesValue.DataModule != null && seriesValue.DataModule.Any())
                {
                    seriesValue.GetPoints(seriesValue.DataModule, this);
                }
                else
                {
                    seriesValue.Points.Clear();
                    seriesValue.ClubbedPoints.Clear();
                }
            }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            GetOtherLanguageCharSize();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            if (VisibleSeries.Count > 0)
            {
                RefreshChart();
            }
        }

        internal void RefreshChart()
        {
            AccumulationLegendModule = new AccumulationChartLegend(this, AccChartThemeStyle);
            DoGroupingProcess();
            CalculateBounds();
            InstanceInitialization();
            CreateChart();
        }

        private void DoGroupingProcess()
        {
            AccumulationChartSeries series = VisibleSeries?.First();
            if (series.DataModule != null && !string.IsNullOrEmpty(series.LastGroupTo) && series.LastGroupTo != series.GroupTo)
            {
                series.GetPoints(series.DataModule, this);
            }
        }

        private void CalculateBounds()
        {
            InitialClipRect = new Rect(Margin.Left, Margin.Top, AvailableSize.Width, AvailableSize.Height);
            titleCollection = subTitleCollection = null;
            double titleHeight = 0, subTitleHeight = 0, titleWidth = 0, maxWidth = 0;
            if (!string.IsNullOrEmpty(Title))
            {
                titleCollection = ChartHelper.GetTitle(Title, GetFontOptions(TitleStyle), InitialClipRect.Width);
                titleHeight = ChartHelper.MeasureText(Title, GetFontOptions(TitleStyle)).Height * titleCollection.Count;
                if (!string.IsNullOrEmpty(SubTitle))
                {
                    foreach (string titleText in titleCollection)
                    {
                        titleWidth = ChartHelper.MeasureText(titleText, GetFontOptions(TitleStyle)).Width;
                        maxWidth = titleWidth > maxWidth ? titleWidth : maxWidth;
                    }

                    subTitleCollection = ChartHelper.GetTitle(SubTitle, GetFontOptions(SubTitleStyle), maxWidth);
                    subTitleHeight = ChartHelper.MeasureText(SubTitle, GetFontOptions(SubTitleStyle)).Height * subTitleCollection.Count;
                }
            }

            ChartHelper.SubtractRect(InitialClipRect, new Rect(0, subTitleHeight + titleHeight, Margin.Right + Margin.Left, Margin.Top + Margin.Bottom));
            CalculateLegendBounds();
        }

        internal static ChartFontOptions GetFontOptions(ChartCommonFont font)
        {
            return new ChartFontOptions { Color = font.Color, Size = font.Size, FontFamily = font.FontFamily, FontWeight = font.FontWeight, FontStyle = font.FontStyle, TextAlignment = font.TextAlignment, TextOverflow = font.TextOverflow };
        }

        private void CalculateLegendBounds()
        {
            if (LegendSettings.Visible)
            {
                AccumulationLegendModule.GetLegendOptions(VisibleSeries);
                AccumulationLegendModule.CalculateLegendBounds(InitialClipRect, AvailableSize, LegendSettings, Margin, "AccumulationChart", Series[0].Type.ToString());
            }
        }

        private async Task CalculateSecondaryElementPosition()
        {
            DomRect svgOffset = await InvokeMethod<DomRect>(AccumulationChartConstants.GETELEMENTBOUNDSBYID, false, new object[] { SvgId, false });
            DomRect elementOffset = await InvokeMethod<DomRect>(AccumulationChartConstants.GETELEMENTBOUNDSBYID, false, new object[] { ID, false });
            SecondaryElementOffset.Left = Math.Max(svgOffset.Left - elementOffset.Left, 0);
            SecondaryElementOffset.Top = Math.Max(svgOffset.Top - elementOffset.Top, 0);
        }
    }
}
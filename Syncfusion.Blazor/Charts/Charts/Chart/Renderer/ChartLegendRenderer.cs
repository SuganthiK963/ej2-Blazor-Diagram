using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartLegendRenderer : LegendBase, ILegendMethods
    {
        internal Rect availableRect;
        private int seriesIndex;

        internal ChartHelper Helper { get; set; } = new ChartHelper();

        internal ChartLegendSettings LegendSettings { get; set; }

        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            Owner.LegendRenderer = this;
            ChartId = Owner.ID;
        }

        internal string GetFontKey()
        {
            SetDefaultStyle();
            return LegendSettings.TextStyle.FontWeight + Constants.UNDERSCORE + LegendSettings.TextStyle.FontStyle + Constants.UNDERSCORE + LegendSettings.TextStyle.FontFamily;
        }

        private void SetDefaultStyle()
        {
            if (LegendSettings != null)
            {
                return;
            }

            LegendSettings = new ChartLegendSettings();
            Legend = (ILegendBase)LegendSettings;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            if (availableRect != rect && rect != null)
            {
                RendererShouldRender = true;
                LegendOptions = new List<LegendSymbols>();
                PagingOptions = new List<LegendSymbols>();
                if (LegendSettings.Visible)
                {
                    GetLegendOptions();
                    if (LegendCollection.Count == 0)
                    {
                        return;
                    }

                    CalculateLegendBounds(rect, Owner.AvailableSize, Owner.Margin, "Chart", null);
                    CalculateRenderTreeBuilderOptions();
                }

                availableRect = rect;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            if (availableRect != null && LegendOptions.Count != 0 && Position != LegendPosition.Custom)
            {
                base.BuildRenderTree(builder);
                Owner.SvgRenderer.OpenGroupElement(builder, LegendID + "_g");
                RenderLegend(builder, Owner.SvgRenderer, LegendSettings.Border);
                builder.CloseElement();
            }

            RendererShouldRender = false;
        }

        internal void GetLegendOptions()
        {
            LegendCollection = new List<LegendOption>();
            BaseLegendRef = this;
            ThemeStyle = Owner.ChartThemeStyle;
            ChartSeries series;
            Position = LegendSettings.Position;
            IsInverse = LegendSettings.IsInversed || Owner.EnableRTL;
            BorderWidth = LegendSettings.Border.Width;
            foreach (ChartSeriesRenderer seriesRenderer in Owner.VisibleSeriesRenderers)
            {
                series = seriesRenderer.Series;
                if (seriesRenderer.Category() != SeriesCategories.Indicator && !string.IsNullOrEmpty(series.Name))
                {
                    bool visible = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? seriesRenderer.TrendLineLegendVisibility : series.Visible;
                    LegendCollection.Add(new LegendOption(
                        text: series.Name,
                        fill: seriesRenderer.Interior,
                        shape: series.LegendShape,
                        seriesWidth: series.Width,
                        textStyle: LegendSettings.TextStyle,
                        seriesBorderColor: series.Border.Color,
                        seriesBorderWidth: series.Border.Width,
                        visible: visible,
                        type: (series.Type == ChartSeriesType.Polar || series.Type == ChartSeriesType.Radar) ? series.DrawType.ToString() : series.Type.ToString(),
                        markerShape: series.Marker.Shape,
                        markerVisibility: series.Marker.Visible));
                }
            }
        }

        /// <summary>
        /// The method is used to check whether current legend group within the legend bounds.
        /// </summary>
        /// <param name="previousBound">Specifies the pervious legend group total width.</param>
        /// <param name="textWidth">Specifies the current legend text width.</param>
        private bool IsWithinBounds(double previousBound, double textWidth)
        {
            if(!IsInverse)
            {
                return (previousBound + textWidth) > (LegendBounds.X + LegendBounds.Width + (Legend.ShapeWidth / 2));
            }
            else
            {
                return (previousBound - textWidth) < (LegendBounds.X - (Legend.ShapeWidth / 2));
            }
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetRenderPoint(LegendOption legendOption, ChartInternalLocation start, double textPadding, LegendOption prevLegend, int count, int firstLegend)
        {
            if (legendOption == null || start == null || prevLegend == null)
            {
                return;
            }
            double textWidth = textPadding + prevLegend.TextSize.Width, availableWidth, previousBound = (!IsInverse) ? prevLegend.Location.X + (textWidth - 0.5) : (prevLegend.Location.X + 0.5) - textWidth;
            if (IsWithinBounds(previousBound, legendOption.TextSize.Width + textPadding) || IsVertical)
            {
                legendOption.Location.X = start.X;
                legendOption.Location.Y = (count == firstLegend) ? prevLegend.Location.Y : prevLegend.Location.Y + MaxItemHeight + Legend.Padding;
            }
            else
            {
                legendOption.Location.X = (count == firstLegend) ? prevLegend.Location.X : previousBound;
                legendOption.Location.Y = prevLegend.Location.Y;
            }

            availableWidth = (!IsInverse) ? (LegendBounds.X + LegendBounds.Width) - (legendOption.Location.X + textPadding - (Legend.ShapeWidth / 2)) : (legendOption.Location.X - textPadding + (Legend.ShapeWidth / 2)) - LegendBounds.X;
            legendOption.Text = ChartHelper.TextTrim(availableWidth, legendOption.Text, LegendSettings.TextStyle.GetChartFontOptions());
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetLegendBounds(Size availableSize, Rect rect, Size maxLabelSize)
        {
            double extraHeight = 0, extraWidth = 0, maximumWidth = 0, rowWidth = 0, columnHeight = 0, rowCount = 0;
            bool render = false;
            MaxItemHeight = Math.Max(ChartHelper.MeasureText("MeasureText", LegendSettings.TextStyle.GetChartFontOptions()).Height, Legend.ShapeHeight);
            if (!IsVertical)
            {
#pragma warning disable CA1062
                extraHeight = string.IsNullOrEmpty(Legend.Height) ? (availableSize.Height / 100 * 5) : 0;
            }
            else
            {
                extraWidth = string.IsNullOrEmpty(Legend.Width) ? ((availableSize.Width / 100) * 5) : 0;
            }

            LegendBounds.Height += extraHeight;
            LegendBounds.Width += extraWidth;
            foreach (LegendOption legendOption in LegendCollection)
            {
                LegendRenderEventArgs LegendEvent = new LegendRenderEventArgs()
                {
                    Name = Constants.LEGENDRENDER,
                    Text = legendOption.Text,
                    Fill = legendOption.Fill,
                    Shape = legendOption.Shape,
                    MarkerShape = legendOption.MarkerShape
                };
                SfChart.InvokeEvent(Owner.ChartEvents?.OnLegendItemRender, LegendEvent);

                if (Constants.REGSUB.IsMatch(LegendEvent.Text))
                {
                    LegendEvent.Text = ChartHelper.GetUniCode(LegendEvent.Text, Constants.SUBPATTERN, Constants.REGSUB);
                }

                if (Constants.REGSUP.IsMatch(LegendEvent.Text))
                {
                    LegendEvent.Text = ChartHelper.GetUniCode(LegendEvent.Text, Constants.SUPPATTERN, Constants.REGSUP);
                }

                legendOption.Render = !LegendEvent.Cancel;
                legendOption.Text = LegendEvent.Text;
                legendOption.Fill = LegendEvent.Fill;
                legendOption.Shape = LegendEvent.Shape;
                legendOption.MarkerShape = LegendEvent.MarkerShape;
                legendOption.TextSize = ChartHelper.MeasureText(legendOption.Text, LegendSettings.TextStyle.GetChartFontOptions());
                if (legendOption.Render && !string.IsNullOrEmpty(legendOption.Text))
                {
                    render = true;
                    double legendWidth = Legend.ShapeWidth + Legend.ShapePadding + legendOption.TextSize.Width + Legend.Padding;
                    rowWidth = rowWidth + legendWidth;
                    if (LegendBounds.Width < (Legend.Padding + rowWidth) || IsVertical)
                    {
                        maximumWidth = Math.Max(maximumWidth, rowWidth + Legend.Padding - (IsVertical ? 0 : legendWidth));
                        if (rowCount == 0 && (legendWidth != rowWidth))
                        {
                            rowCount = 1;
                        }

                        rowWidth = IsVertical ? 0 : legendWidth;
                        rowCount++;
                        columnHeight = (rowCount * (MaxItemHeight + Legend.Padding)) + Legend.Padding;
                    }
                }
            }

            columnHeight = Math.Max(columnHeight, MaxItemHeight + Legend.Padding + Legend.Padding);
            IsPaging = LegendBounds.Height < columnHeight && rowCount > 0;
            TotalPages = rowCount;
            if (render)
            {
                SetBounds(Math.Max(rowWidth + Legend.Padding, maximumWidth), columnHeight);
            }
            else
            {
                SetBounds(0, 0);
            }

            GetLocation(rect, availableSize, LegendSettings.Margin, Owner.Margin, LegendSettings.Border, LegendSettings.Location);
        }

        internal void MouseMove(ChartInternalMouseEventArgs args)
        {
            if (LegendSettings.Visible && !Owner.IsTouch && Owner.HighlightModule != null)
            {
                Click(args);
            }
        }

        internal void Click(ChartInternalMouseEventArgs eventArgs)
        {
            if (!LegendSettings.Visible)
            {
                return;
            }

            List<string> legendItemsId = new List<string>() { LegendID + "_text_", LegendID + "_shape_marker_", LegendID + "_shape_" };

            foreach (var id in legendItemsId)
            {
                if (eventArgs.Target.Contains(id, StringComparison.InvariantCulture))
                {
                    seriesIndex = int.Parse(eventArgs.Target.Split(id)[1], null);
                    LegendClick(eventArgs);
                    break;
                }
            }

            if (!string.IsNullOrEmpty(PageUpID) && eventArgs.Target.Contains(PageUpID, StringComparison.InvariantCulture))
            {
                ChangePage(true);
            }
            else if (!string.IsNullOrEmpty(PageDownID) && eventArgs.Target.Contains(PageDownID, StringComparison.InvariantCulture))
            {
                ChangePage(false);
            }
        }

        private void LegendClick(ChartInternalMouseEventArgs eventArgs)
        {
            ChartSeriesRenderer seriesRenderer = Owner.VisibleSeriesRenderers[seriesIndex];
            LegendOption legend = LegendCollection[seriesIndex];
            LegendClickEventArgs legendClickArgs = new LegendClickEventArgs(Constants.LEGENDCLICK, false, Owner, legend.Shape, seriesRenderer.Series, legend.Text);
            SfChart.InvokeEvent(Owner.ChartEvents?.OnLegendClick, legendClickArgs);
            if (!legendClickArgs.Cancel)
            {
                seriesRenderer.Series.SetLegendShape(legendClickArgs.LegendShape);

                if (seriesRenderer.Series.Fill != null)
                {
                    seriesRenderer.Interior = seriesRenderer.Series.Fill;
                }

                List<ChartSelectedDataIndex> selectedDataIndexes = new List<ChartSelectedDataIndex>();
                if (Owner.SelectionModule != null)
                {
                    selectedDataIndexes = Owner.SelectedDataIndexes;
                }

                if (LegendSettings.ToggleVisibility)
                {
                    Owner.Redraw = Owner.EnableAnimation;
                    if (seriesRenderer.Category() == SeriesCategories.TrendLine)
                    {
                        ChartTrendline trendLine = Owner.TrendlineContainer.Elements[seriesRenderer.Index] as ChartTrendline;
                        trendLine.SetVisibility(!trendLine.Visible);
                    }

                    RefreshSeriesPosition();
                    ChangeSeriesVisiblity(seriesRenderer, seriesRenderer.Series.Visible);
                    
                    if (selectedDataIndexes.Count > 0)
                    {
                        Owner.SelectionModule.SelectedDataIndexes = selectedDataIndexes;
                        Owner.SelectionModule.RedrawSelection(Owner.SelectionMode);
                    }
                }
                else if (Owner.SelectionModule != null)
                {
                    Owner.SelectionModule.LegendSelection(seriesIndex, eventArgs);
                }
                else if (Owner.HighlightModule != null)
                {
                    Owner.HighlightModule.LegendSelection(seriesIndex, eventArgs);
                }
            }
        }

        private void RefreshSeriesPosition()
        {
            Owner.SeriesContainer.Renderers.ForEach(renderer => (renderer as ChartSeriesRenderer).Position = double.NaN);
        }

        private void ChangeSeriesVisiblity(ChartSeriesRenderer series, bool visibility)
        {
            series.Series.OnLegendClick(!visibility);
            if (IsSecondaryAxis(series.XAxisRenderer))
            {
          // series.XAxisRenderer. = series.XAxisRenderer.Series.Any((x) => x.Visible);
            }

            if (IsSecondaryAxis(series.YAxisRenderer))
            {
           // series.YAxisRenderer.InternalVisibility = series.YAxisRenderer.Series.Any((x) => x.Visible);
            }
        }

        private bool IsSecondaryAxis(ChartAxisRenderer axis)
        {
            return Owner.AxisContainer.Renderers.Contains(axis);
        }

        protected void ChangePage(bool pageUp)
        {
            SvgText pageNumberElement = Owner.SvgRenderer.TextElementList.Find(element => element.Id == PageNumberID);
            int page = int.Parse(pageNumberElement.Text.Split('/')[0], null);
            if (pageUp && page > 1)
            {
                TranslatePage(page - 2, page - 1, pageNumberElement);
            }
            else if (!pageUp && page < TotalPageCount)
            {
                TranslatePage(page, page + 1, pageNumberElement);
            }
        }

        protected async void TranslatePage(double page, double pageNumber, SvgText pageNumberEle = null)
        {
            Transform = "translate(0,-" + (ClipPathHeight * page).ToString(culture) + ")";
            CurrentPageNumber = pageNumber;
            await Owner.InvokeMethod(Constants.SETATTRIBUTE, new object[] { LegendTranslateID, "transform", Transform });
            pageNumberEle?.ChangeText(CurrentPageNumber + "/" + TotalPageCount);
        }

        internal void UpdateLegendFill(ChartSeriesRenderer seriesRenderer, string seriesFill = null)
        {
            ChartSeries series = seriesRenderer.Series;
            if (seriesRenderer.Category() != SeriesCategories.Indicator && !string.IsNullOrEmpty(series.Name))
            {
                int index = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? seriesIndex : seriesRenderer.Index;
                bool visibility = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? seriesRenderer.TrendLineLegendVisibility : seriesRenderer.Series.Visible;
                LegendCollection[index].Visible = visibility;
                LegendCollection[index].Fill = !string.IsNullOrEmpty(seriesFill) ? seriesFill : LegendCollection[index].Fill;
                UpdateLegendOptions(LegendCollection[index], seriesRenderer.Index);
                LegendOptions[index].TextOption.Fill = visibility ? ChartHelper.FindThemeColor(LegendCollection[index].TextStyle.Color, ThemeStyle.LegendLabel) : "#D3D3D3";
                if (Owner.CustomLegendRenderer != null && Position == LegendPosition.Custom)
                {
                    Owner.CustomLegendRenderer.RendererShouldRender = true;
                    Owner.CustomLegendRenderer.ProcessRenderQueue();
                }
            }
        }

        private void UpdateLegendOptions(LegendOption legendOption, int index)
        {
            List<SymbolOptions> symbols = CalculateLegendOptions(legendOption, index);
            LegendOptions[index].FirstSymbol = symbols[0];
            LegendOptions[index].SecondSymbol = symbols[1];
        }

        internal void UpdateLegendShape(ChartSeriesRenderer seriesRenderer)
        {
            ChartSeries series = seriesRenderer.Series;
            if (seriesRenderer.Category() != SeriesCategories.Indicator && !string.IsNullOrEmpty(series.Name))
            {
                int index = (seriesRenderer.Category() == SeriesCategories.TrendLine) ? seriesIndex : seriesRenderer.Index;
                if (index < LegendCollection.Count)
                {
                    LegendCollection[index].Type = Owner.ChartAreaType == ChartAreaType.PolarAxes ? series.DrawType.ToString() : series.Type.ToString();
                    LegendCollection[index].MarkerShape = series.Marker.Shape;
                    LegendCollection[index].MarkerVisibility = series.Marker.Visible;
                    LegendCollection[index].Shape = series.LegendShape;
                    UpdateLegendOptions(LegendCollection[index], seriesRenderer.Index);
                }
                if (Owner.CustomLegendRenderer != null && Position == LegendPosition.Custom)
                {
                    Owner.CustomLegendRenderer.RendererShouldRender = true;
                    Owner.CustomLegendRenderer.ProcessRenderQueue();
                }
            }
        }

        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            string fill = LegendSettings.Visible ? ChartHelper.FindThemeColor(LegendSettings.TextStyle.Color, Owner.ChartThemeStyle.LegendLabel) : "#D3D3D3";
            LegendOptions.ForEach(option => option.TextOption.Fill = fill);
            ProcessRenderQueue();
        }
    }
}

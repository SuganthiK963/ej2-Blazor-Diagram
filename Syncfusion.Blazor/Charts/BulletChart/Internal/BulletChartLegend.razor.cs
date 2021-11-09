using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.BulletChart.Internal
{
    /// <summary>
    /// Specifies legend rendering of the bullet chart.
    /// </summary>
    public partial class BulletChartLegend
    {
        private const string SPACE = " ";
        private Rect legendBounds = new Rect();
        private Rect legendBorder = new Rect();
        private LegendPosition legendPosition;
        private bool isVertical;
        private double maxItemHeight;
        private bool isPaging;
        private int totalPages;
        private double rowCount;
        private Margin margin = new Margin();
        private TextStyle textStyle = new TextStyle();
        private int currentPage = 1;
        private PageInfo legendPage;
        private ClipModel clipath = new ClipModel();
        private string legendItemTransform = string.Empty;
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private StringComparison comparison = StringComparison.InvariantCulture;

        [CascadingParameter]
        internal IBulletChart BulletChart { get; set; }

        private static void SubtractThickness(Rect rect, Rect thickness)
        {
            rect.X += thickness.Height;
            rect.Y += thickness.X;
            rect.Width -= thickness.Height + thickness.Width;
            rect.Height -= thickness.X + thickness.Y;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (BulletChart.LegendSettings != null)
            {
                BulletChart.ChartLegend = this;
            }
        }

        private void SetLegendTextStyle()
        {
            BulletChartLegendTextStyle style = BulletChart.LegendSettings?.TextStyle;
            textStyle.Size = style == null ? "13px" : style.Size;
            if (style != null)
            {
                textStyle.Color = style.Color;
                textStyle.FontFamily = style.FontFamily;
                textStyle.FontStyle = style.FontStyle;
                textStyle.FontWeight = style.FontWeight;
                textStyle.Opacity = style.Opacity;
                textStyle.Size = style.Size;
            }
        }

        internal async Task CalculateLegendBounds(Rect initialClipRect, SizeInfo availableSize, SizeInfo maxLabelSize)
        {
            SetLegendTextStyle();
            legendPosition = BulletChart.LegendSettings.Position == LegendPosition.Auto ? LegendPosition.Bottom : BulletChart.LegendSettings.Position;
            legendBounds = new Rect(0, 0, initialClipRect.X, initialClipRect.Y);
            isVertical = legendPosition == LegendPosition.Left || legendPosition == LegendPosition.Right;
            if (isVertical)
            {
                double height = BulletChart.Helper.StringToNumber(BulletChart.LegendSettings?.Height, availableSize.Height - (initialClipRect.Y - BulletChart.Render.ChartMargin.Top));
                legendBounds.Height = height.Equals(double.NaN) ? initialClipRect.Height : height;
                legendBounds.Width = BulletChart.Helper.StringToNumber(string.IsNullOrEmpty(BulletChart.LegendSettings?.Width) ? "40%" : BulletChart.LegendSettings?.Width, availableSize.Width);
            }
            else
            {
                double width = BulletChart.Helper.StringToNumber(BulletChart.LegendSettings?.Width, availableSize.Width);
                legendBounds.Width = width.Equals(double.NaN) ? initialClipRect.Width : width;
                legendBounds.Height = BulletChart.Helper.StringToNumber(string.IsNullOrEmpty(BulletChart.LegendSettings?.Height) ? "40%" : BulletChart.LegendSettings?.Height, availableSize.Height);
            }

            await GetLegendBounds(availableSize, legendBounds);
            GetLocation(legendPosition, legendBounds, initialClipRect, availableSize, maxLabelSize);
        }

        private void GetLocation(LegendPosition legendPosition, Rect legendBounds, Rect rect, SizeInfo availableSize, SizeInfo maxLabelSize)
        {
            if (BulletChart.LegendSettings.Margin != null)
            {
                margin = new Margin(BulletChart.LegendSettings.Margin.Bottom, BulletChart.LegendSettings.Margin.Left, BulletChart.LegendSettings.Margin.Right, BulletChart.LegendSettings.Margin.Top);
            }

            bool labelIns = BulletChart.LabelPosition == LabelsPlacement.Inside,
            ticklIns = BulletChart.TickPosition == TickPosition.Inside,
            isChartVertical = BulletChart.Orientation == OrientationType.Vertical;
            double border = BulletChart.LegendSettings.Border != null ? BulletChart.LegendSettings.Border.Width : 1,
            legendHeight = legendBounds.Height + (border * 2) + margin.Top + margin.Bottom,
            legendWidth = legendBounds.Width + border + margin.Left + margin.Right,
            tickWidth = BulletChart.MajorTickLines != null ? BulletChart.MajorTickLines.Height : 12,
            categoryFieldValue = !string.IsNullOrEmpty(BulletChart.CategoryField) ? maxLabelSize.Width + (border * 2) : 0;
            if (legendPosition == LegendPosition.Bottom)
            {
                double extraPadding = BulletChart.Helper.MeasureTextSize(string.Format(culture, "{0}", BulletChart.Maximum), BulletChart.Render.LabelStyle).Height;
                legendBounds.X = AlignLegend(legendBounds.X, availableSize.Width, legendBounds.Width);
                legendBounds.Y = rect.Y + (rect.Height - legendHeight) + border + margin.Top;
                legendBounds.Y += (!BulletChart.OpposedPosition && !labelIns && !ticklIns && !isChartVertical) ? tickWidth + extraPadding :
                (isChartVertical && !string.IsNullOrEmpty(BulletChart.CategoryField)) ? maxLabelSize.Height + (border * 2) : 0;
                SubtractThickness(rect, new Rect(0, 0, 0, legendHeight));
            }
            else if (legendPosition == LegendPosition.Top)
            {
                legendBounds.X = AlignLegend(legendBounds.X, availableSize.Width, legendBounds.Width);
                legendBounds.Y = border + margin.Top;
                legendBounds.Y -= (BulletChart.OpposedPosition && !labelIns && !ticklIns && !isChartVertical) ? tickWidth + margin.Top : 0;
                SubtractThickness(rect, new Rect(0, 0, legendHeight, 0));
            }
            else if (legendPosition == LegendPosition.Right)
            {
                legendBounds.X = rect.X + (rect.Width - legendBounds.Width);
                legendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + margin.Bottom), legendBounds.Height);
                legendWidth += (BulletChart.OpposedPosition && !labelIns && !ticklIns && isChartVertical) ? (margin.Left + margin.Right + tickWidth) : 0;
                SubtractThickness(rect, new Rect(0, legendWidth, 0, 0));
            }
            else if (legendPosition == LegendPosition.Left)
            {
                legendBounds.X = (legendBounds.X - categoryFieldValue) + (border * 2) + margin.Left - 10;
                legendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + 15), legendBounds.Height);
                legendWidth += (!BulletChart.OpposedPosition && !labelIns && !ticklIns && isChartVertical) ? (legendBounds.X - margin.Left + border + tickWidth) : 0;
                SubtractThickness(rect, new Rect(legendWidth, 0, 0, 0));
            }
            else
            {
                SetLegendCustomLocation();
                SubtractThickness(rect, new Rect(0, 0, 0, 0));
            }
        }

        private void SetLegendCustomLocation()
        {
            legendBounds.X = BulletChart.LegendSettings.Location != null ? BulletChart.LegendSettings.Location.X : 0;
            legendBounds.Y = BulletChart.LegendSettings.Location != null ? BulletChart.LegendSettings.Location.Y : 0;
        }

        internal void ChangePage(bool isPageUp)
        {
            if (!isPageUp && currentPage < totalPages)
            {
                TranslatePage(currentPage, currentPage + 1);
            }
            else if (isPageUp && currentPage > 1)
            {
                TranslatePage(currentPage - 2, currentPage - 1);
            }
        }

        private void TranslatePage(int page, int pageNumber)
        {
            legendItemTransform = "translate(0,-" + ((clipath.Bound.Height * page) - 1).ToString(culture) + ")";
            currentPage = pageNumber;
            legendPage.Text = pageNumber.ToString(culture) + "/" + totalPages.ToString(culture);
            StateHasChanged();
        }

        private double AlignLegend(double start, double size, double legendSize)
        {
            switch (BulletChart.LegendSettings.Alignment)
            {
                case Alignment.Far:
                    return legendPosition == LegendPosition.Top || legendPosition == LegendPosition.Bottom ? BulletChart.Render.InitialClipRect.Width + start - legendSize : BulletChart.Render.InitialClipRect.Height + start - legendSize;
                case Alignment.Center:
                    return (size - legendSize) / 2;
            }

            return start;
        }

        private async Task GetLegendBounds(SizeInfo availableSize, Rect legendBounds)
        {
            double extraHeight = 0, extraWidth = 0, legendWidth = 0, legendRowWidth = 0, maximumWidth = 0, columnHeight = 0;
            int legendRowCount = 0;
            bool render = false;
            BulletChartLegendSettings legendSettings = BulletChart.LegendSettings;
            if (!isVertical)
            {
                extraHeight = string.IsNullOrEmpty(BulletChart.LegendSettings?.Height) ? ((availableSize.Height / 100) * 5) : 0;
            }
            else
            {
                extraWidth = string.IsNullOrEmpty(BulletChart.LegendSettings?.Width) ? ((availableSize.Width / 100) * 5) : 0;
            }

            legendBounds.Height += extraHeight;
            legendBounds.Width += extraWidth;
            maxItemHeight = Math.Max(BulletChart.Helper.MeasureTextSize("MeasureText", textStyle).Height, BulletChart.LegendSettings.ShapeHeight);
            for (int i = 0; i < BulletChart.Render?.LegendCollections?.Count; i++)
            {
                LegendModel legend = BulletChart.Render?.LegendCollections[i];
                BulletChartLegendRenderEventArgs args = new BulletChartLegendRenderEventArgs()
                {
                    Fill = legend.Fill,
                    Text = legend.Text,
                    Shape = legend.Shape,
                };
                await SfBaseUtils.InvokeEvent<BulletChartLegendRenderEventArgs>(BulletChart.Events?.LegendRender, args);
                SizeInfo textSize = BulletChart.Helper.MeasureTextSize(args.Text, textStyle);
                legend.Render = !args.Cancel;
                legend.TextSize = textSize;
                legend.Text = args.Text;
                legend.Shape = args.Shape;
                legend.Fill = args.Fill;
                if (!args.Cancel && !string.IsNullOrEmpty(args.Text))
                {
                    render = true;
                    legendWidth = legendSettings.ShapeWidth + legendSettings.ShapePadding + textSize.Width + legendSettings.Padding;
                    legendRowWidth = legendRowWidth + legendWidth;
                    if (legendBounds.Width < (legendSettings.Padding + legendRowWidth))
                    {
                        maximumWidth = Math.Max(maximumWidth, legendRowWidth + legendSettings.Padding - (isVertical ? 0 : legendWidth));
                        if (legendRowCount == 0 && legendWidth != legendRowWidth)
                        {
                            legendRowCount = 1;
                        }

                        legendRowWidth = isVertical ? 0 : legendWidth;
                        legendRowCount++;
                        columnHeight = (legendRowCount * (maxItemHeight + legendSettings.Padding)) + legendSettings.Padding;
                    }
                }
            }

            columnHeight = Math.Max(columnHeight, maxItemHeight + legendSettings.Padding + legendSettings.Padding);
            isPaging = legendBounds.Height < columnHeight;
            totalPages = legendRowCount;
            if (render)
            {
                SetBounds(Math.Max(legendRowWidth + legendSettings.Padding, maximumWidth), columnHeight, legendBounds);
            }
            else
            {
                SetBounds(0, 0, legendBounds);
            }
        }

        private void SetBounds(double computedWidth, double computedHeight, Rect legendBounds)
        {
            computedWidth = Math.Min(computedWidth, legendBounds.Width);
            computedHeight = Math.Min(computedHeight, legendBounds.Height);
            legendBounds.Width = string.IsNullOrEmpty(BulletChart.LegendSettings.Width) ? computedWidth : legendBounds.Width;
            legendBounds.Height = string.IsNullOrEmpty(BulletChart.LegendSettings.Height) ? computedHeight : legendBounds.Height;
            rowCount = Math.Max(1, Math.Ceiling((legendBounds.Height - BulletChart.LegendSettings.Padding - 0) / (maxItemHeight + BulletChart.LegendSettings.Padding)));
        }

        internal void RenderLegend()
        {
            List<LegendModel> legend = BulletChart.Render.LegendCollections;
            BulletChartLegendSettings legendSettings = BulletChart.LegendSettings;
            int firstLegend = FindFirstLegendPosition();
            maxItemHeight = Math.Max(legend.FirstOrDefault().TextSize.Height, BulletChart.LegendSettings.ShapeHeight);
            legendBorder = new Rect(legendBounds.Height, legendBounds.Width, legendBounds.X, legendBounds.Y);
            clipath = new ClipModel()
            {
                StrokeWidth = BulletChart.LegendSettings?.Border != null ? BulletChart.LegendSettings.Border.Width : 1,
                Stroke = BulletChart.LegendSettings?.Border != null ? BulletChart.LegendSettings.Border.Color : string.Empty,
                Bound = new Rect(legendBounds.Height, legendBounds.Width, legendBounds.X, legendBounds.Y),
            };
            clipath.Bound.Y += BulletChart.LegendSettings.Padding;
            if (firstLegend != BulletChart.Render.LegendCollections.Count)
            {
                Rect start = new Rect()
                {
                    X = legendBounds.X + BulletChart.LegendSettings.Padding + (BulletChart.LegendSettings.ShapeWidth / 2),
                    Y = legendBounds.Y + BulletChart.LegendSettings.Padding + (maxItemHeight / 2)
                };
                double textPadding = legendSettings.ShapePadding + legendSettings.Padding + legendSettings.ShapeWidth;
                legend.FirstOrDefault().Location = start;
                LegendModel prevLegend = legend.FirstOrDefault();
                for (int i = 0; i < legend.Count; i++)
                {
                    if (legend[i].Render && !string.IsNullOrEmpty(legend[i].Text))
                    {
                        GetRenderPoint(legend[i], start, textPadding, prevLegend, legendBounds, i, firstLegend);
                        legend[i].Fill = BulletChart.LegendSettings.Visible ? legend[i].Fill : "#D3D3D3";
                        legend[i].StrokeWidth = legend[i].Shape == LegendShape.Multiply ? 4 : 1;
                        legend[i].Path = CalculateShapes(legend[i], legend[i].Location, legend[i].Shape);
                        textStyle.Color = legendSettings.Visible ? string.IsNullOrEmpty(textStyle.Color) ? BulletChart.Style.LegendLabel : textStyle.Color : "#D3D3D3";
                        legend[i].TextInfo.X = legend[i].Location.X + (legendSettings.ShapeWidth / 2) + legendSettings.ShapePadding;
                        legend[i].TextInfo.Y = legend[i].Location.Y + (maxItemHeight / 4);
                        prevLegend = legend[i];
                    }
                }

                if (isPaging)
                {
                    RenderPagingElements(legendBounds);
                }
                else
                {
                    totalPages = 1;
                    legendPage = null;
                }
            }
        }

        private void RenderPagingElements(Rect bounds)
        {
            totalPages = (int)Math.Ceiling(totalPages / Math.Max(1, rowCount - 1));
            int tablIndex = !string.IsNullOrEmpty(BulletChart.Title) && !string.IsNullOrEmpty(BulletChart.Subtitle) ? BulletChart.TabIndex + 2 : !string.IsNullOrEmpty(BulletChart.Title) && string.IsNullOrEmpty(BulletChart.Subtitle) ? BulletChart.TabIndex + 1 : BulletChart.TabIndex; 
            legendPage = new PageInfo()
            {
                Color = "transparent",
                Fill = "#545454",
                Opacity = 1,
                LeftTabIndex = tablIndex + 1,
                RightTabIndex = tablIndex + 2
            };
            int count = (int)rowCount - 1;
            clipath.Bound.Height = (count * (maxItemHeight + BulletChart.LegendSettings.Padding)) - 1;
            double x = bounds.X + 4,
            clipPathHeight = count * (maxItemHeight + BulletChart.LegendSettings.Padding),
            y = bounds.Y + clipPathHeight + ((bounds.Height - clipPathHeight) / 2);
            string pageCount = Convert.ToString(totalPages, null);
            SizeInfo size = BulletChart.Helper.MeasureTextSize(pageCount + "/" + pageCount, textStyle);
            legendPage.LeftPagePath = DrawPageSymbol(new Rect(0, 0, x, y), "LeftArrow");
            legendPage.TextLocation.X = x + 12;
            legendPage.TextLocation.Y = y + (size.Height / 4);
            legendPage.Text = currentPage.ToString(culture) + "/" + pageCount;
            x = legendPage.TextLocation.X + 12 + size.Width;
            legendPage.RightPagePath = DrawPageSymbol(new Rect(0, 0, x, y), "RightArrow");
            legendPage.Transform = "translate(" + ((legendBounds.Width / 2) - (size.Width / 2) - 16).ToString(culture) + ",0)";
            legendItemTransform = "translate(" + 0 + ",-" + (clipPathHeight * (currentPage - 1)).ToString(culture) + ")";
        }

        private string DrawPageSymbol(Rect rect, string shape)
        {
            double lx = rect.X, ly = rect.Y;
            switch (shape)
            {
                case "LeftArrow":
                    return "M" + SPACE + (lx + 4).ToString(culture) + SPACE + (ly - 4).ToString(culture) + SPACE + "L" + SPACE + (lx - 4).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "L" + SPACE +
                        (lx + 4).ToString(culture) + SPACE + (ly + 4).ToString(culture) + SPACE + "L" + SPACE + (lx + 4).ToString(culture) + SPACE + (ly + 2).ToString(culture) + " L" + SPACE + lx.ToString(culture) + SPACE + ly.ToString(culture) + " L" + (lx + 4).ToString(culture) + SPACE + (ly - 2).ToString(culture) + " Z";
                case "RightArrow":
                    return "M" + SPACE + (lx - 4).ToString(culture) + SPACE + (ly - 4).ToString(culture) + SPACE + "L" + SPACE + (lx + 4).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "L" + SPACE +
                        (lx - 4).ToString(culture) + SPACE + (ly + 4).ToString(culture) + " L" + SPACE + (lx - 4).ToString(culture) + SPACE + (ly + 2).ToString(culture) + SPACE + "L" + SPACE + lx.ToString(culture) + SPACE + ly.ToString(culture) + " L" + (lx - 4).ToString(culture) + SPACE + (ly - 2).ToString(culture) + " Z";
            }

            return string.Empty;
        }

        private string CalculateShapes(LegendModel legend, Rect location, LegendShape shape)
        {
            double shapeWidth = BulletChart.LegendSettings.ShapeWidth,
            shapeHeight = BulletChart.LegendSettings.ShapeHeight,
            width = shape == LegendShape.Circle ? shapeWidth - 2 : shapeWidth,
            height = shape == LegendShape.Circle ? shapeHeight - 2 : shapeHeight,
            lx = location.X,
            ly = location.Y,
            y = location.Y - (height / 2),
            x = location.X - (width / 2),
            sizeBullet = BulletChart.TargetWidth;
            switch (shape)
            {
                case LegendShape.Circle:
                    legend.X = width / 2;
                    legend.Y = height / 2;
                    legend.Width = lx;
                    legend.Height = ly;
                    break;
                case LegendShape.Cross:
                    return "M" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "M" + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + lx.ToString(culture) + SPACE +
                    (ly + (-height / 2)).ToString(culture);
                case LegendShape.Multiply:
                    return "M " + (lx - sizeBullet).ToString(culture) + SPACE + (ly - sizeBullet).ToString(culture) + " L " + (lx + sizeBullet).ToString(culture) + SPACE + (ly + sizeBullet).ToString(culture) + " M " + (lx - sizeBullet).ToString(culture) + SPACE + (ly + sizeBullet).ToString(culture) + " L " + (lx + sizeBullet).ToString(culture) + SPACE + (ly - sizeBullet).ToString(culture);
                case LegendShape.HorizontalLine:
                    return "M" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture);
                case LegendShape.VerticalLine:
                    return "M" + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture);
                case LegendShape.Diamond:
                    return "M" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "L" + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "L" + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE +
                    "L" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + " z";
                case LegendShape.ActualRect:
                    return "M" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 8)).ToString(culture) + SPACE + "L" + SPACE + (lx + sizeBullet).ToString(culture) + SPACE + (ly + (-height / 8)).ToString(culture) + SPACE + "L" + SPACE + (lx + sizeBullet).ToString(culture) + SPACE + (ly + (height / 8)).ToString(culture) + SPACE +
                    "L" + SPACE + x.ToString(culture) + SPACE + (ly + (height / 8)).ToString(culture) + SPACE + "L" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 8)).ToString(culture) + " z";
                case LegendShape.TargetRect:
                    return "M" + SPACE + (x + sizeBullet).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + "L" + SPACE + (lx + (sizeBullet / 2)).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE +
                    "L" + SPACE + (lx + (sizeBullet / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + (x + sizeBullet).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + (x + sizeBullet).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + " z";
                case LegendShape.Rectangle:
                    return "M" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE +
                    "L" + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + " z";
                case LegendShape.Triangle:
                    return "M" + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + " z";
                case LegendShape.Pentagon:
                    string path = string.Empty;
                    for (int i = 0; i <= 5; i++)
                    {
                        double xval = (width / 2) * Math.Cos((Math.PI / 180) * (i * 72)),
                        yval = (height / 2) * Math.Sin((Math.PI / 180) * (i * 72));
                        if (i == 0)
                        {
                            path = "M" + SPACE + (lx + xval).ToString(culture) + SPACE + (ly + yval).ToString(culture) + SPACE;
                        }
                        else
                        {
                            path += "L" + SPACE + (lx + xval).ToString(culture) + SPACE + (ly + yval).ToString(culture) + SPACE;
                        }
                    }

                    return path;
                case LegendShape.InvertedTriangle:
                    return "M" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + "L" + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + "L" + SPACE + (lx - (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE +
                    "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + " z";
            }

            return string.Empty;
        }

        private void GetRenderPoint(LegendModel legend, Rect start, double textPadding, LegendModel prevLegend, Rect rect, int count, int firstLegend)
        {
            double previousBound = prevLegend.Location.X + textPadding + prevLegend.TextSize.Width;
            if ((previousBound + (legend.TextSize.Width + textPadding)) > (rect.X + rect.Width + (BulletChart.LegendSettings.ShapeWidth / 2)))
            {
                legend.Location.X = start.X;
                legend.Location.Y = count == firstLegend ? prevLegend.Location.Y : prevLegend.Location.Y + maxItemHeight + BulletChart.LegendSettings.Padding;
            }
            else
            {
                legend.Location.X = count == firstLegend ? prevLegend.Location.X : previousBound;
                legend.Location.Y = prevLegend.Location.Y;
            }

            double availwidth = (legendBounds.X + legendBounds.Width) - (legend.Location.X + textPadding - (BulletChart.LegendSettings.ShapeWidth / 2));
            legend.Text = BulletChart.Helper.TrimText(availwidth + 5, legend.Text, textStyle);
        }

        private int FindFirstLegendPosition()
        {
            int count = 0;
            foreach (LegendModel legend in BulletChart.Render.LegendCollections)
            {
                if (legend.Render && !string.IsNullOrEmpty(legend.Text))
                {
                    break;
                }

                count++;
            }

            return count;
        }

        private async Task LegendReRender()
        {
            await BulletChart.Render.CalculatePosition();
            RenderLegend();
        }

        internal async Task OnPropertyChanged(Dictionary<string, object> propertyChanges, string parent)
        {
            if (parent.Equals(nameof(BulletChartRange), comparison))
            {
                BulletChart.Render.GetLegendOptions();
                await LegendReRender();
            }

            if (parent.Equals(nameof(BulletChartLegendLocation), comparison))
            {
                SetLegendCustomLocation();
                RenderLegend();
            }

            if (parent.Equals(nameof(BulletChartLegendTextStyle), comparison))
            {
                SetLegendTextStyle();
                if (propertyChanges.ContainsKey("Size"))
                {
                    await LegendReRender();
                }
            }

            StateHasChanged();
        }

        internal override void ComponentDispose()
        {
            BulletChart = null;
            legendBounds = null;
            legendBorder = null;
            margin = null;
            textStyle = null;
            legendPage = null;
            clipath = null;
        }
    }
}
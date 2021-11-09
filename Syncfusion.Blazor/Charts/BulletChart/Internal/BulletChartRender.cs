using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.BulletChart.Internal
{
    /// <summary>
    /// Specifies rendering of the bullet chart.
    /// </summary>
    public class BulletChartRender
    {
        private IBulletChart bulletChart;
        private double interval;
        private SizeInfo maxTitleSize;
        private SizeInfo maxLabelSize;
        private Rect bulletChartRect;
        private BulletChartHelper helper;
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private StringComparison comparison = StringComparison.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulletChartRender"/> class.
        /// </summary>
        /// <param name="parent">Represents the <seealso cref="IBulletChart"/> class.</param>
        /// <param name="chartHelper">Represents the <seealso cref="BulletChartHelper"/> class.</param>
        public BulletChartRender(IBulletChart parent, BulletChartHelper chartHelper)
        {
            bulletChart = parent;
            helper = chartHelper;
            SetMarginStyle();
            SetLabelStyle();
            SetTitleStyle();
            SetSubTitleStyle();
            SetCategoryStyle();
        }

        internal static Dictionary<string, SizeInfo> SizePerCharacter { get; set; } = new Dictionary<string, SizeInfo>();

        internal Margin ChartMargin { get; set; } = new Margin(10, 15, 15, 15);

        internal TextStyle TitleStyle { get; set; } = new TextStyle();

        internal TextStyle SubTitleStyle { get; set; } = new TextStyle();

        internal List<BaseModel> MajorTicksCollection { get; set; }

        internal List<BaseModel> MinorTicksCollection { get; set; }

        internal List<LabelModel> TitleCollection { get; set; } = new List<LabelModel>();

        internal List<LabelModel> SubTitleCollection { get; set; } = new List<LabelModel>();

        internal Border ChartBorder { get; set; }

        internal List<LabelModel> LabelCollection { get; set; }

        internal TextStyle LabelStyle { get; set; } = new TextStyle();

        internal TextStyle CategoryStyle { get; set; } = new TextStyle();

        internal TextStyle DataLabelStyle { get; set; } = new TextStyle();

        internal ChartTitle TitleInfo { get; set; }

        internal ChartTitle SubTitleInfo { get; set; }

        internal List<ExpandoObject> ProcessedData { get; set; }

        internal double MinValue { get; set; }

        internal double MaxValue { get; set; }

        internal Rect InitialClipRect { get; set; }

        internal List<double> RangeCollection { get; set; }

        internal List<LegendModel> LegendCollections { get; set; }

        internal List<LabelModel> DataLabelCollection { get; set; } = new List<LabelModel>();

        internal void DataProcessing(IEnumerable dataSource)
        {
            if (dataSource != null)
            {
                ProcessedData = new List<ExpandoObject>();
                IEnumerator iterator = dataSource.GetEnumerator();
                while (iterator.MoveNext())
                {
                    PropertyInfo[] property = iterator.Current.GetType().GetProperties();
                    IDictionary<string, object> data = new ExpandoObject();
                    foreach (PropertyInfo propertyInfo in property)
                    {
                        data[propertyInfo.Name] = propertyInfo.GetValue(iterator.Current);
                    }

                    ProcessedData.Add(data as ExpandoObject);
                }
            }
        }

        internal void GetLegendOptions()
        {
            LegendCollections = new List<LegendModel>();
            int count = 0;
            string fill;
            for (int i = 0; i < bulletChart.Ranges?.Count; i++)
            {
                BulletChartRange range = bulletChart.Ranges[i];
                if (!string.IsNullOrEmpty(range.Name))
                {
                    fill = !string.IsNullOrEmpty(range.Color) ? range.Color : bulletChart.Style.RangeStrokes[i];
                    LegendCollections.Add(new LegendModel() { Text = range.Name, Fill = fill, Index = i, Shape = range.Shape });
                    count++;
                }
            }

            if (!string.IsNullOrEmpty(bulletChart.ValueField))
            {
                fill = !string.IsNullOrEmpty(bulletChart.ValueFill) ? bulletChart.ValueFill : "black";
                LegendShape shape = bulletChart.Orientation == OrientationType.Vertical ? LegendShape.TargetRect : LegendShape.ActualRect;
                LegendCollections.Add(new LegendModel() { Text = "Actual", Fill = fill, Index = count++, Shape = shape });
            }

            if (!string.IsNullOrEmpty(bulletChart.TargetField))
            {
                fill = !string.IsNullOrEmpty(bulletChart.TargetColor) ? bulletChart.TargetColor : "black";
                LegendShape shape = bulletChart.Orientation == OrientationType.Vertical ? LegendShape.ActualRect : LegendShape.TargetRect;
                for (int i = 0; i < bulletChart.Render?.ProcessedData?.Count; i++)
                {
                    IDictionary<string, object> data = ProcessedData[i];
                    data.TryGetValue(bulletChart.TargetField, out object targetValue);
                    if (targetValue is IEnumerable<double> && i == 0)
                    {
                        for (int j = 0; j < bulletChart.TargetTypes.Count; j++)
                        {
                            TargetType type = bulletChart.TargetTypes[j % bulletChart.TargetTypes.Count];
                            if (type == TargetType.Rect)
                            {
                                LegendCollections.Add(new LegendModel() { Text = "Target_" + string.Format(null, "{0}", j), Fill = fill, Index = count++, Shape = bulletChart.Orientation == OrientationType.Vertical ? LegendShape.ActualRect : LegendShape.TargetRect });
                            }
                            else
                            {
                                LegendCollections.Add(new LegendModel() { Text = "Target_" + string.Format(null, "{0}", j), Fill = fill, Index = count++, Shape = type == TargetType.Cross ? LegendShape.Multiply : LegendShape.Circle });
                            }
                        }
                    }
                    else if (i == 0)
                    {
                        LegendCollections.Add(new LegendModel() { Text = "Target", Fill = fill, Index = count++, Shape = shape });
                    }
                }
            }
        }

        internal void SetMarginStyle()
        {
            if (bulletChart.Margin != null)
            {
                ChartMargin = new Margin(bulletChart.Margin.Bottom, bulletChart.Margin.Left, bulletChart.Margin.Right, bulletChart.Margin.Top);
            }
        }

        internal void SetTitleStyle()
        {
            BulletChartTitleStyle style = bulletChart.TitleStyle;
            TitleStyle.Size = style == null ? "15px" : style.Size;
            TitleStyle.Color = bulletChart.Style.TitleFontColor;
            if (style != null)
            {
                TitleStyle.Color = style.Color;
                TitleStyle.FontFamily = style.FontFamily;
                TitleStyle.FontStyle = style.FontStyle;
                TitleStyle.FontWeight = style.FontWeight;
                TitleStyle.MaximumTitleWidth = style.MaximumTitleWidth;
                TitleStyle.Opacity = style.Opacity;
                TitleStyle.TextAlignment = style.TextAlignment;
                TitleStyle.TextOverflow = style.TextOverflow;
                TitleStyle.EnableRangeColor = style.EnableRangeColor;
            }
        }

        internal void SetLabelStyle()
        {
            BulletChartLabelStyle style = bulletChart.LabelStyle;
            if (style != null)
            {
                LabelStyle.Color = style.Color;
                LabelStyle.FontFamily = style.FontFamily;
                LabelStyle.FontStyle = style.FontStyle;
                LabelStyle.FontWeight = style.FontWeight;
                LabelStyle.Opacity = style.Opacity;
                LabelStyle.Size = style.Size;
                LabelStyle.EnableRangeColor = style.EnableRangeColor;
            }
        }

        internal void SetDataLabelStyle()
        {
            BulletChartDataLabelStyle style = bulletChart.DataLabel?.LabelStyle;
            if (style != null)
            {
                DataLabelStyle.Color = style.Color;
                DataLabelStyle.FontFamily = style.FontFamily;
                DataLabelStyle.FontStyle = style.FontStyle;
                DataLabelStyle.FontWeight = style.FontWeight;
                DataLabelStyle.Opacity = style.Opacity;
                DataLabelStyle.Size = style.Size;
            }
        }

        internal void SetCategoryStyle()
        {
            BulletChartCategoryLabelStyle style = bulletChart.CategoryLabelStyle;
            if (style != null)
            {
                CategoryStyle.Color = style.Color;
                CategoryStyle.FontFamily = style.FontFamily;
                CategoryStyle.FontStyle = style.FontStyle;
                CategoryStyle.FontWeight = style.FontWeight;
                CategoryStyle.Opacity = style.Opacity;
                CategoryStyle.Size = style.Size;
            }
        }

        internal void SetSubTitleStyle()
        {
            BulletChartSubTitleStyle style = bulletChart.SubtitleStyle;
            SubTitleStyle.Size = style == null ? "13px" : style.Size;
            SubTitleStyle.Color = bulletChart.Style.SubTitleFontColor;
            if (style != null)
            {
                SubTitleStyle.Color = style.Color;
                SubTitleStyle.FontFamily = style.FontFamily;
                SubTitleStyle.FontStyle = style.FontStyle;
                SubTitleStyle.FontWeight = style.FontWeight;
                SubTitleStyle.MaximumTitleWidth = style.MaximumTitleWidth;
                SubTitleStyle.Opacity = style.Opacity;
                SubTitleStyle.Size = style.Size;
                SubTitleStyle.TextAlignment = style.TextAlignment;
                SubTitleStyle.TextOverflow = style.TextOverflow;
                SubTitleStyle.EnableRangeColor = style.EnableRangeColor;
            }
        }

        internal void RenderBulletChartBackground()
        {
            ChartBorder = new Border()
            {
                ID = bulletChart.ID + "_ChartBorder",
                BackGround = bulletChart.Style.Background,
                StrokeWidth = bulletChart.Border != null ? bulletChart.Border.Width : 0,
                Stroke = bulletChart.Border != null ? bulletChart.Border.Color : Constant.TRANSPARENT,
            };
        }

        internal void FindRange()
        {
            MinValue = bulletChart.Minimum;
            MaxValue = bulletChart.Maximum;
            interval = bulletChart.Interval;
            if (MinValue == -1)
            {
                MinValue = 0;
            }

            if (MaxValue == -1)
            {
                for (int i = 0; i < bulletChart.Ranges?.Count; i++)
                {
                    MaxValue = MaxValue > bulletChart.Ranges[i].End ? MaxValue : bulletChart.Ranges[i].End;
                }
            }

            if (MaxValue == -1)
            {
                MaxValue = 10;
            }

            if (bulletChart.Interval == -1)
            {
                interval = CalculateNumericNiceInterval(MaxValue - MinValue);
            }
        }

        internal void RenderBulletElements()
        {
            RenderBulletChartTitle();
            bulletChart.ScaleRender.DrawScaleGroup();
            double size = bulletChart.Orientation == OrientationType.Horizontal ? InitialClipRect.Width : InitialClipRect.Height,
            intervalValue = size / ((MaxValue - MinValue) / interval);
            RenderMajorTickLines(intervalValue);
            RenderMinorTickLines(intervalValue);
            RenderAxisLabels(intervalValue);
            bulletChartRect.X = bulletChart.TitlePosition == TextPosition.Left || bulletChart.TitlePosition == TextPosition.Right || bulletChart.Orientation == OrientationType.Vertical ? bulletChartRect.X : 0;
            if (ProcessedData != null && ProcessedData.Count > 0)
            {
                bulletChart.ScaleRender.RenderFeatureBar(ProcessedData);
                bulletChart.ScaleRender.RenderComparativeSymbol(ProcessedData);
            }

            RenderDataLabel();
            if (LegendCollections != null && LegendCollections.Count > 0)
            {
                bulletChart.ChartLegend?.RenderLegend();
            }
        }

        internal void RenderDataLabel()
        {
            if (bulletChart.DataLabel != null)
            {
                SetDataLabelStyle();
                DataLabelCollection = new List<LabelModel>();
                for (int i = 0; i < ProcessedData?.Count; i++)
                {
                    IDictionary<string, object> data = ProcessedData[i];
                    BarInfo featureBar = bulletChart.ScaleRender.FeatureBars[i];
                    data.TryGetValue(bulletChart.ValueField, out object labelText);
                    string formatText = helper.GetText((labelText ?? string.Empty).ToString(), bulletChart.LabelFormat, bulletChart.Format, bulletChart.EnableGroupSeparator);
                    SizeInfo labelSize = helper.MeasureTextSize(formatText, DataLabelStyle);
                    string anchor;
                    double x, y;
                    if (bulletChart.Orientation == OrientationType.Horizontal)
                    {
                        anchor = bulletChart.Type == FeatureType.Rect ? "end" : (bulletChart.IsRtlEnabled() ? "end" : "start");
                        x = featureBar.X + (bulletChart.IsRtlEnabled() ? (bulletChart.Type == FeatureType.Rect ? labelSize.Width + 10 : -10) :
                            featureBar.Width) + (bulletChart.Type == FeatureType.Rect ? -5 : 5);
                        y = featureBar.Y + (featureBar.Height / 2) + (labelSize.Height / 4);
                    }
                    else
                    {
                        anchor = "middle";
                        x = featureBar.X + (featureBar.Width / 2);
                        y = featureBar.Y + (bulletChart.IsRtlEnabled() ? featureBar.Height + (bulletChart.Type == FeatureType.Rect ? -labelSize.Height : labelSize.Height) : 0) + (bulletChart.Type == FeatureType.Rect ? 10 : -10);
                    }

                    DataLabelCollection.Add(new LabelModel() { Text = formatText, X = x, Y = y, Anchor = anchor, Color = string.IsNullOrEmpty(DataLabelStyle.Color) ? bulletChart.Style.DataLabelFontColor : DataLabelStyle.Color });
                }
            }
            else
            {
                DataLabelCollection = null;
            }
        }

        private void RenderAxisLabels(double intervalValue)
        {
            LabelCollection = new List<LabelModel>();
            bool enableRtl = bulletChart.IsRtlEnabled();
            string strokeColor = string.IsNullOrEmpty(LabelStyle.Color) ? bulletChart.Style.LabelFontColor : LabelStyle.Color,
            maximumValue = string.Format(culture, "{0}", MaxValue);
            if (bulletChart.Orientation == OrientationType.Horizontal)
            {
                double tick = (bulletChart.TickPosition.ToString().Equals(bulletChart.LabelPosition.ToString(), comparison) ? bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Height : 12 : 0) + 10,
                y = InitialClipRect.Y + (bulletChart.OpposedPosition ? bulletChart.LabelPosition == LabelsPlacement.Inside ? tick : -tick : InitialClipRect.Height + (bulletChart.LabelPosition == LabelsPlacement.Inside ? -tick : tick)),
                x = InitialClipRect.X + (enableRtl ? InitialClipRect.Width : 0),
                size = InitialClipRect.X + (enableRtl ? InitialClipRect.Width : 0);
                y += helper.MeasureTextSize(maximumValue, LabelStyle).Height / 3;
                for (double i = MinValue; i <= MaxValue; i += interval)
                {
                    bool condition = !enableRtl ? i == MaxValue : i == MinValue;
                    if (bulletChart.LabelStyle != null && bulletChart.LabelStyle.EnableRangeColor)
                    {
                        strokeColor = BindingRangeStrokes(x - (condition ? (bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Width : 1) / 2 : 0), size);
                    }

                    if (i == MaxValue && !string.IsNullOrEmpty(bulletChart.LabelFormat))
                    {
                        x += enableRtl ? 5 : -5;
                    }

                    AddLabelCollection(new LabelModel() { X = x, Y = y, Text = helper.GetText(i.ToString(culture), bulletChart.LabelFormat, bulletChart.Format, bulletChart.EnableGroupSeparator), Color = strokeColor });
                    x += enableRtl ? -intervalValue : intervalValue;
                }
            }
            else
            {
                double tick = (bulletChart.TickPosition.ToString().Equals(bulletChart.LabelPosition.ToString(), comparison) ? bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Height : 12 : 0) + 10,
                y = InitialClipRect.Y + (!enableRtl ? InitialClipRect.Height : 0),
                x = InitialClipRect.X + (!bulletChart.OpposedPosition ? bulletChart.LabelPosition == LabelsPlacement.Inside ? tick + 10 : -tick : InitialClipRect.Width + (bulletChart.LabelPosition == LabelsPlacement.Inside ? -(tick + 10) : tick)),
                size = InitialClipRect.Y + (!enableRtl ? InitialClipRect.Height : 0);
                SizeInfo labelsize = helper.MeasureTextSize(maximumValue, LabelStyle);
                double labelWidth = labelsize.Width / 2, height = labelsize.Height / 3, i;
                y += height;
                for (i = MinValue; i <= MaxValue; i += interval)
                {
                    bool condition = bulletChart.IsRtlEnabled() ? i == MaxValue : i == MinValue;
                    if (bulletChart.LabelStyle != null && bulletChart.LabelStyle.EnableRangeColor)
                    {
                        strokeColor = BindingRangeStrokes(y - height - (condition ? (bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Width : 1) / 2 : 0), size);
                    }

                    AddLabelCollection(new LabelModel() { X = x - (!bulletChart.OpposedPosition ? labelWidth : -labelWidth), Y = y, Text = helper.GetText(i.ToString(culture), bulletChart.LabelFormat, bulletChart.Format, bulletChart.EnableGroupSeparator), Color = strokeColor });
                    y += !enableRtl ? -intervalValue : intervalValue;
                }
            }
        }

        private void AddLabelCollection(LabelModel labelModel)
        {
            BulletChartLabelRenderEventArgs args = new BulletChartLabelRenderEventArgs() { Text = labelModel.Text, X = labelModel.X, Y = labelModel.Y };
            bulletChart.Events?.OnLabelRender.Invoke(args);
            if (!args.Cancel)
            {
                labelModel.Text = args.Text;
                labelModel.X = args.X;
                labelModel.Y = args.Y;
                LabelCollection.Add(labelModel);
            }
        }

        private void RenderMinorTickLines(double intervalValue)
        {
            MinorTicksCollection = new List<BaseModel>();
            bool enableRtl = bulletChart.IsRtlEnabled();
            double minorTick = bulletChart.MinorTickLines != null ? bulletChart.MinorTickLines.Height : 8,
            minorTicksPerInterval = bulletChart.MinorTicksPerInterval;
            string strokeColor = bulletChart.MinorTickLines != null ? bulletChart.MinorTickLines.Color : bulletChart.Style.MinorTickLineColor;
            if (bulletChart.Orientation == OrientationType.Horizontal)
            {
                double majorPointX = InitialClipRect.X,
                y1 = InitialClipRect.Y + (bulletChart.OpposedPosition ? 0 : InitialClipRect.Height),
                y2 = y1 + (!bulletChart.OpposedPosition ? bulletChart.TickPosition != TickPosition.Inside ? minorTick : -minorTick :
                            bulletChart.TickPosition != TickPosition.Inside ? -minorTick : minorTick),
                size = InitialClipRect.X + (enableRtl ? InitialClipRect.Width : 0);
                for (double i = MinValue; i < MaxValue; i += interval)
                {
                    double minorPointX = intervalValue / minorTicksPerInterval;
                    for (double j = 1; j <= minorTicksPerInterval; j++)
                    {
                        double x = majorPointX + minorPointX - (minorPointX / (minorTicksPerInterval + 1));
                        if (bulletChart.MinorTickLines != null && bulletChart.MinorTickLines.EnableRangeColor)
                        {
                            strokeColor = BindingRangeStrokes(x, size);
                        }

                        MinorTicksCollection.Add(new BaseModel() { X1 = x, X2 = x, Y1 = y1, Y2 = y2, Stroke = strokeColor, StrokeWidth = bulletChart.MinorTickLines != null ? bulletChart.MinorTickLines.Width : 1 });
                        minorPointX = (intervalValue / minorTicksPerInterval) * (j + 1);
                    }

                    majorPointX += intervalValue;
                }
            }
            else
            {
                double majorPointY = InitialClipRect.Y + (!enableRtl ? InitialClipRect.Height : 0),
                x1 = InitialClipRect.X + (!bulletChart.OpposedPosition ? 0 : InitialClipRect.Width),
                x2 = x1 - (!bulletChart.OpposedPosition ? bulletChart.TickPosition != TickPosition.Inside ? minorTick : -minorTick : bulletChart.TickPosition != TickPosition.Inside ? -minorTick : minorTick),
                size = InitialClipRect.Y + (!enableRtl ? InitialClipRect.Height : 0), i;
                for (i = MinValue; i < MaxValue; i += interval)
                {
                    double minorPointY = intervalValue / minorTicksPerInterval, y, j;
                    for (j = 1; j <= minorTicksPerInterval; j++)
                    {
                        if (!enableRtl)
                        {
                            y = majorPointY - minorPointY + (minorPointY / (minorTicksPerInterval + 1));
                        }
                        else
                        {
                            y = majorPointY + minorPointY - (minorPointY / (minorTicksPerInterval + 1));
                        }

                        if (bulletChart.MinorTickLines != null && bulletChart.MinorTickLines.EnableRangeColor)
                        {
                            strokeColor = BindingRangeStrokes(y, size);
                        }

                        MinorTicksCollection.Add(new BaseModel() { X1 = x1, X2 = x2, Y1 = y, Y2 = y, Stroke = strokeColor, StrokeWidth = bulletChart.MinorTickLines != null ? bulletChart.MinorTickLines.Width : 1 });
                        minorPointY = (intervalValue / minorTicksPerInterval) * (j + 1);
                    }

                    majorPointY -= enableRtl ? -intervalValue : intervalValue;
                }
            }
        }

        private string BindingRangeStrokes(double position, double size)
        {
            double prevSize = size;
            if ((bulletChart.Orientation == OrientationType.Vertical && !bulletChart.IsRtlEnabled()) || (bulletChart.IsRtlEnabled() && bulletChart.Orientation == OrientationType.Horizontal))
            {
                for (int i = 0; i <= RangeCollection?.Count - 1; i++)
                {
                    prevSize -= i == 0 ? 0 : RangeCollection[i - 1];
                    if (Math.Round(position) >= Math.Round(prevSize - RangeCollection[i]) && position <= prevSize)
                    {
                        return bulletChart.Ranges[i].Color ?? string.Empty;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= RangeCollection?.Count - 1; i++)
                {
                    prevSize += i == 0 ? 0 : RangeCollection[i - 1];
                    if (position >= prevSize && position <= prevSize + RangeCollection[i])
                    {
                        return bulletChart.Ranges[i].Color ?? string.Empty;
                    }
                }
            }

            return string.Empty;
        }

        private void RenderMajorTickLines(double intervalValue)
        {
            MajorTicksCollection = new List<BaseModel>();
            double majorTickSize = bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Height : 12;
            string strokeColor = bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Color : bulletChart.Style.MajorTickLineColor;
            bool enableRtl = bulletChart.IsRtlEnabled();
            double tickWidth = bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Width : 1;
            if (bulletChart.Orientation == OrientationType.Horizontal)
            {
                double y1 = InitialClipRect.Y + (bulletChart.OpposedPosition ? 0 : InitialClipRect.Height),
                y2 = y1 + (!bulletChart.OpposedPosition ? (bulletChart.TickPosition != TickPosition.Inside ? majorTickSize : -majorTickSize) : (bulletChart.TickPosition != TickPosition.Inside ? -majorTickSize : majorTickSize)),
                size = InitialClipRect.X + (enableRtl ? InitialClipRect.Width : 0),
                majorPointX = InitialClipRect.X + (tickWidth / 2) + (enableRtl ? InitialClipRect.Width : 0);
                for (double i = MinValue; i <= MaxValue; i += interval)
                {
                    bool condition = !enableRtl ? i == MaxValue : i == MinValue;
                    if (condition)
                    {
                        majorPointX -= 1 / 2;
                    }

                    if (bulletChart.MajorTickLines != null && bulletChart.MajorTickLines.EnableRangeColor)
                    {
                        strokeColor = BindingRangeStrokes(majorPointX - (condition ? tickWidth / 2 : 0), size);
                    }

                    MajorTicksCollection.Add(new BaseModel() { X1 = majorPointX, X2 = majorPointX, Y1 = y1, Y2 = y2, Stroke = strokeColor, StrokeWidth = tickWidth });
                    majorPointX = majorPointX + (enableRtl ? -intervalValue : intervalValue);
                }
            }
            else
            {
                double x1 = InitialClipRect.X + (!bulletChart.OpposedPosition ? 0 : InitialClipRect.Width),
                x2 = x1 - (!bulletChart.OpposedPosition ? (bulletChart.TickPosition != TickPosition.Inside ? majorTickSize : -majorTickSize) : bulletChart.TickPosition != TickPosition.Inside ? -majorTickSize : majorTickSize),
                size = InitialClipRect.Y + (!enableRtl ? InitialClipRect.Height : 0),
                majorPointY = InitialClipRect.Y + (tickWidth / 2) + (!enableRtl ? InitialClipRect.Height : 0);
                for (double i = MinValue; i <= MaxValue; i += interval)
                {
                    bool condition = enableRtl ? i == MaxValue : i == MinValue;
                    if (condition)
                    {
                        majorPointY -= tickWidth / 2;
                    }

                    if (bulletChart.MajorTickLines != null && bulletChart.MajorTickLines.EnableRangeColor)
                    {
                        strokeColor = BindingRangeStrokes(majorPointY - (condition ? tickWidth / 2 : 0), size);
                    }

                    MajorTicksCollection.Add(new BaseModel() { X1 = x1, X2 = x2, Y1 = majorPointY, Y2 = majorPointY, Stroke = strokeColor, StrokeWidth = tickWidth });
                    majorPointY = majorPointY + (!enableRtl ? -intervalValue : intervalValue);
                }
            }
        }

        private void RenderBulletChartTitle()
        {
            double x = 0, y = 0;
            string anchor = "middle",
            transform = string.Empty;
            SizeInfo elementSize = helper.MeasureTextSize(bulletChart.Title, TitleStyle),
            subTitleSize = bulletChart.Subtitle != null ? helper.MeasureTextSize(bulletChart.Subtitle, SubTitleStyle) : new SizeInfo() { Height = 0, Width = 0 };
            if (!string.IsNullOrEmpty(bulletChart.Title))
            {
                if (bulletChart.Orientation == OrientationType.Horizontal)
                {
                    switch (bulletChart.TitlePosition)
                    {
                        case TextPosition.Top:
                            x = FindHorizontalAlignment();
                            anchor = TitleStyle.TextAlignment == Alignment.Far ? "end" : TitleStyle.TextAlignment == Alignment.Near ? "start" : "middle";
                            y = ChartMargin.Top + (elementSize.Height / 2) + 5;
                            break;
                        case TextPosition.Bottom:
                            x = FindHorizontalAlignment();
                            anchor = TitleStyle.TextAlignment == Alignment.Far ? "end" : TitleStyle.TextAlignment == Alignment.Near ? "start" : "middle";
                            y = bulletChart.AvailableSize.Height - ChartMargin.Bottom - (elementSize.Height / 3) + 10 - (subTitleSize.Height > 0 ? subTitleSize.Height + 5 : 0);
                            break;
                        case TextPosition.Left:
                            anchor = "end";
                            x = ChartMargin.Left + maxTitleSize.Width;
                            y = (ChartMargin.Top + (bulletChart.AvailableSize.Height / 2) - (elementSize.Height / 3)) - (subTitleSize.Height > 0 ? subTitleSize.Height : 0);
                            break;
                        case TextPosition.Right:
                            anchor = "start";
                            x = bulletChart.AvailableSize.Width - ChartMargin.Right - maxTitleSize.Width + 5;
                            y = (ChartMargin.Top + (bulletChart.AvailableSize.Height / 2) - (elementSize.Height / 3)) - (subTitleSize.Height > 0 ? subTitleSize.Height : 0);
                            break;
                    }
                }
                else
                {
                    switch (bulletChart.TitlePosition)
                    {
                        case TextPosition.Top:
                            x = (bulletChart.AvailableSize.Width / 2) + 10;
                            y = ChartMargin.Top + (elementSize.Height / 2) + 5;
                            break;
                        case TextPosition.Bottom:
                            x = bulletChart.AvailableSize.Width / 2;
                            y = bulletChart.AvailableSize.Height - ChartMargin.Bottom - (elementSize.Height / 3) + 10 - (subTitleSize.Height > 0 ? subTitleSize.Height + 5 : 0);
                            break;
                        case TextPosition.Left:
                            y = FindVerticalAlignment();
                            anchor = TitleStyle.TextAlignment == Alignment.Far ? "start" : (TitleStyle.TextAlignment == Alignment.Near ? "end" : "middle");
                            x = ChartMargin.Left;
                            break;
                        case TextPosition.Right:
                            x = bulletChart.AvailableSize.Width - ChartMargin.Right - (elementSize.Height / 3);
                            anchor = TitleStyle.TextAlignment == Alignment.Far ? "start" : (TitleStyle.TextAlignment == Alignment.Near ? "end" : "middle");
                            y = FindVerticalAlignment();
                            break;
                    }

                    transform = bulletChart.TitlePosition == TextPosition.Left ? "rotate(-90," + x.ToString(culture) + "," + y.ToString(culture) + ")" :
                            bulletChart.TitlePosition == TextPosition.Right ? "rotate(90," + x.ToString(culture) + "," + y.ToString(culture) + ")" : string.Empty;
                }

                TitleInfo = new ChartTitle() { X = x, Y = y, Anchor = anchor, Transform = transform, TabIndex = bulletChart.TabIndex + 1 };
                RenderTextElement("Title");
                if (!string.IsNullOrEmpty(bulletChart.Subtitle))
                {
                    RenderBulletChartSubTitle(x, TitleCollection.Count > 1 ? TitleCollection[TitleCollection.Count - 1].Y : y, anchor);
                }
            }
        }

        private void RenderTextElement(string type)
        {
            if (type.Equals("Title", comparison) && TitleCollection.Count > 1)
            {
                for (int i = 1, len = TitleCollection.Count; i < len; i++)
                {
                    double height = helper.MeasureTextSize(TitleCollection[i].Text, TitleStyle).Height,
                    dy = TitleInfo.Y + (i * height);
                    TitleCollection[i].X = TitleInfo.X;
                    TitleCollection[i].Y = dy;
                }
            }
            else if (type.Equals("Subtitle", comparison) && SubTitleCollection.Count > 1)
            {
                for (int i = 1, len = SubTitleCollection.Count; i < len; i++)
                {
                    double height = helper.MeasureTextSize(SubTitleCollection[i].Text, SubTitleStyle).Height,
                    dy = SubTitleInfo.Y + (i * height);
                    SubTitleCollection[i].X = SubTitleInfo.X;
                    SubTitleCollection[i].Y = dy;
                }
            }
        }

        private void RenderBulletChartSubTitle(double x, double y, string anchor)
        {
            string transform = string.Empty;
            SizeInfo elementSize = helper.MeasureTextSize(bulletChart.Subtitle, SubTitleStyle);
            if (bulletChart.Orientation == OrientationType.Horizontal)
            {
                switch (bulletChart.TitlePosition)
                {
                    case TextPosition.Top:
                        y += elementSize.Height + 2.5;
                        break;
                    case TextPosition.Bottom:
                        y = bulletChart.AvailableSize.Height - ChartMargin.Bottom - (elementSize.Height / 3) + 5;
                        break;
                    case TextPosition.Left:
                        y += elementSize.Height + 2.5;
                        break;
                    case TextPosition.Right:
                        y += elementSize.Height + 2.5;
                        break;
                }
            }
            else
            {
                switch (bulletChart.TitlePosition)
                {
                    case TextPosition.Top:
                        y += elementSize.Height + 2.5;
                        break;
                    case TextPosition.Bottom:
                        y = bulletChart.AvailableSize.Height - ChartMargin.Bottom - (elementSize.Height / 3) + 5;
                        break;
                    case TextPosition.Left:
                        x += elementSize.Height + 2.5;
                        break;
                    case TextPosition.Right:
                        x -= elementSize.Height + 2.5;
                        break;
                }

                transform = bulletChart.TitlePosition == TextPosition.Left ? "rotate(-90," + x.ToString(culture) + "," + y.ToString(culture) + ")" :
                        bulletChart.TitlePosition == TextPosition.Right ? "rotate(90," + x.ToString(culture) + "," + y.ToString(culture) + ")" : string.Empty;
            }

            SubTitleInfo = new ChartTitle() { X = x, Y = y, Anchor = anchor, Transform = transform, TabIndex = bulletChart.TabIndex + 2 };
            RenderTextElement("Subtitle");
        }

        private double CalculateNumericNiceInterval(double delta)
        {
            double actualDesiredIntervalsCount = GetActualDesiredIntervalsCount(bulletChart.AvailableSize),
            niceInterval = delta / actualDesiredIntervalsCount,
            minInterval = Math.Pow(10, Math.Floor(Math.Log(niceInterval) / Math.Log(10)));
            int[] intervalDivs = new int[] { 10, 5, 2, 1 };
            for (int j = 0; j < intervalDivs.Length; j++)
            {
                double currentInterval = minInterval * intervalDivs[j];
                if (actualDesiredIntervalsCount < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }

        private double FindHorizontalAlignment()
        {
            switch (TitleStyle.TextAlignment)
            {
                case Alignment.Center:
                    return bulletChart.AvailableSize.Width / 2;
                case Alignment.Near:
                    return ChartMargin.Left;
                case Alignment.Far:
                    return bulletChart.AvailableSize.Width - ChartMargin.Right;
            }

            return 0;
        }

        private double FindVerticalAlignment()
        {
            switch (TitleStyle.TextAlignment)
            {
                case Alignment.Center:
                    return (bulletChart.AvailableSize.Height - ChartMargin.Top - ChartMargin.Bottom) / 2;
                case Alignment.Near:
                    return ChartMargin.Top;
                case Alignment.Far:
                    return bulletChart.AvailableSize.Height - ChartMargin.Bottom;
            }

            return 0;
        }

        internal async Task CalculatePosition()
        {
            double titleHeight = 0, subTitleHeight = 0, maxTitlteWidth = 0, maxTitlteHeight = 0, maxVerticalTitlteHeight = 5;
            if (!string.IsNullOrEmpty(bulletChart.Title))
            {
                TitleCollection = helper.GetTitle(bulletChart.Title, TitleStyle);
                titleHeight = (helper.MeasureTextSize(bulletChart.Title, TitleStyle).Height * TitleCollection.Count) + 5;
                for (int i = 0; i < TitleCollection.Count; i++)
                {
                    SizeInfo titleSize = helper.MeasureTextSize(TitleCollection[i].Text, TitleStyle);
                    maxTitlteWidth = titleSize.Width > maxTitlteWidth ? titleSize.Width : maxTitlteWidth;
                    maxTitlteHeight = titleSize.Height > maxTitlteHeight ? titleSize.Height : maxTitlteHeight;
                }

                maxVerticalTitlteHeight += maxTitlteHeight;
                SubTitleCollection = helper.GetTitle(bulletChart.Subtitle, SubTitleStyle);
                if (!string.IsNullOrEmpty(bulletChart.Subtitle))
                {
                    for (int i = 0; i < SubTitleCollection.Count; i++)
                    {
                        SizeInfo titleSize = helper.MeasureTextSize(SubTitleCollection[i].Text, SubTitleStyle);
                        maxTitlteWidth = titleSize.Width > maxTitlteWidth ? titleSize.Width : maxTitlteWidth;
                        maxTitlteHeight = titleSize.Height > maxTitlteHeight ? titleSize.Height : maxTitlteHeight;
                    }

                    subTitleHeight = (helper.MeasureTextSize(bulletChart.Subtitle, SubTitleStyle).Height * SubTitleCollection.Count) + 5;
                    maxVerticalTitlteHeight += maxTitlteHeight;
                }
            }

            maxTitleSize = new SizeInfo() { Width = maxTitlteWidth, Height = bulletChart.Orientation == OrientationType.Vertical ? maxVerticalTitlteHeight : maxTitlteHeight };
            GetMaxLabelWidth();
            InitialClipRect = GetBulletBounds(bulletChart.Orientation == OrientationType.Vertical ? maxVerticalTitlteHeight : maxTitlteWidth, titleHeight, subTitleHeight, ChartMargin);
            bulletChartRect = new Rect(InitialClipRect.Height, InitialClipRect.Width, InitialClipRect.X, InitialClipRect.Y);
            if (bulletChart.LegendSettings != null && bulletChart.LegendSettings.Visible && bulletChart.ChartLegend != null)
            {
                await bulletChart.ChartLegend.CalculateLegendBounds(InitialClipRect, bulletChart.AvailableSize, maxLabelSize);
            }
        }

        private Rect GetBulletBounds(double maxTitlteWidth, double titleHeight, double subTitleHeight, Margin margin)
        {
            string maximumValue = string.Format(culture, "{0}", MaxValue);
            double tickSize = bulletChart.TickPosition == TickPosition.Inside ? 0 : bulletChart.MajorTickLines != null ? bulletChart.MajorTickLines.Height : 12,
            labelSize = bulletChart.LabelPosition == LabelsPlacement.Inside ? 0 : 5 + (bulletChart.LabelPosition == LabelsPlacement.Outside ? 0 : helper.MeasureTextSize(maximumValue, LabelStyle).Height),
            topAxisLabel = 0, bottomAxisLabel = 0, leftAxisLabel = 0, rightAxisLabel = 0, topCategory = 0, bottomCategory = 0, leftCategory = 0, rightCategory = 0, categoryLabelSize = 0,
            formatted = helper.MeasureTextSize(maximumValue, LabelStyle).Width;
            Rect rect = new Rect();
            if (bulletChart.Orientation == OrientationType.Horizontal)
            {
                categoryLabelSize = maxLabelSize.Width;
                topAxisLabel = bulletChart.OpposedPosition ? tickSize + labelSize + 5 : 0;
                bottomAxisLabel = !bulletChart.OpposedPosition ? tickSize + labelSize + 5 : 0;
                leftCategory = categoryLabelSize > 0 && !bulletChart.IsRtlEnabled() ? categoryLabelSize : 0;
                leftCategory += maxTitlteWidth > 0 && bulletChart.TitlePosition == TextPosition.Left ? 15 : 0;
                rightCategory = categoryLabelSize > 0 && bulletChart.IsRtlEnabled() ? categoryLabelSize : 0;
                rightCategory += maxTitlteWidth > 0 && bulletChart.TitlePosition == TextPosition.Right ? 15 : 0;
            }
            else
            {
                categoryLabelSize = maxLabelSize.Height;
                rightAxisLabel = bulletChart.OpposedPosition ? tickSize + 5 : 0;
                rightAxisLabel += bulletChart.OpposedPosition && bulletChart.LabelPosition != LabelsPlacement.Inside ? formatted : 0;
                leftAxisLabel = !bulletChart.OpposedPosition ? tickSize + 5 : 0;
                leftAxisLabel += !bulletChart.OpposedPosition && bulletChart.LabelPosition != LabelsPlacement.Inside ? formatted : 0;
                topCategory = categoryLabelSize > 0 && bulletChart.IsRtlEnabled() ? categoryLabelSize + 5 : 0;
                bottomCategory = categoryLabelSize > 0 && !bulletChart.IsRtlEnabled() ? categoryLabelSize + 5 : 0;
            }

            switch (bulletChart.TitlePosition)
            {
                case TextPosition.Left:
                    rect.X = margin.Left + maxTitlteWidth + leftCategory + leftAxisLabel;
                    rect.Width = bulletChart.AvailableSize.Width - margin.Right - rect.X - rightCategory - rightAxisLabel;
                    rect.Y = margin.Top + topAxisLabel + topCategory;
                    rect.Height = bulletChart.AvailableSize.Height - rect.Y - margin.Bottom - bottomAxisLabel - bottomCategory;
                    break;
                case TextPosition.Right:
                    rect.X = margin.Left + leftCategory + leftAxisLabel;
                    rect.Width = bulletChart.AvailableSize.Width - rightAxisLabel - margin.Right - rect.X - (maxTitlteWidth + 5) - rightCategory;
                    rect.Y = margin.Top + topAxisLabel + topCategory;
                    rect.Height = bulletChart.AvailableSize.Height - rect.Y - margin.Bottom - bottomAxisLabel - bottomCategory;
                    break;
                case TextPosition.Top:
                    rect.X = margin.Left + leftAxisLabel + leftCategory;
                    rect.Width = bulletChart.AvailableSize.Width - margin.Right - rect.X - rightCategory - rightAxisLabel;
                    rect.Y = margin.Top + titleHeight + subTitleHeight + topAxisLabel + topCategory;
                    rect.Height = bulletChart.AvailableSize.Height - rect.Y - margin.Bottom - bottomAxisLabel - bottomCategory;
                    break;
                case TextPosition.Bottom:
                    rect.X = margin.Left + leftAxisLabel + leftCategory;
                    rect.Y = margin.Top + topAxisLabel + topCategory;
                    rect.Width = bulletChart.AvailableSize.Width - margin.Right - rect.X - rightCategory - rightAxisLabel;
                    rect.Height = bulletChart.AvailableSize.Height - rect.Y - bottomCategory - margin.Bottom - bottomAxisLabel - (titleHeight + subTitleHeight);
                    break;
            }

            return rect;
        }

        private void GetMaxLabelWidth()
        {
            maxLabelSize = new SizeInfo();
            if (ProcessedData != null && !string.IsNullOrEmpty(bulletChart.CategoryField))
            {
                foreach (IDictionary<string, object> data in ProcessedData)
                {
                    data.TryGetValue(bulletChart.CategoryField, out object lable);
                    SizeInfo size = helper.MeasureTextSize((lable ?? string.Empty).ToString(), CategoryStyle);
                    if (size.Width > maxLabelSize.Width)
                    {
                        maxLabelSize.Width = size.Width;
                    }

                    if (size.Height > maxLabelSize.Height)
                    {
                        maxLabelSize.Height = size.Height;
                    }
                }
            }
        }

        private double GetActualDesiredIntervalsCount(SizeInfo availableSize)
        {
            double size = bulletChart.Orientation == OrientationType.Horizontal ? availableSize.Width : availableSize.Height;
            return Math.Max(size * ((bulletChart.Orientation == OrientationType.Horizontal ? 1.59 : 3) / 100), 1);
        }

        internal void Dispose()
        {
            bulletChart = null;
            DataLabelCollection = null;
            LegendCollections = null;
            RangeCollection = null;
            ProcessedData = null;
            SubTitleInfo = null;
            TitleInfo = null;
            DataLabelStyle = null;
            CategoryStyle = null;
            LabelStyle = null;
            LabelCollection = null;
            ChartBorder = null;
            SubTitleCollection = null;
            TitleCollection = null;
            MinorTicksCollection = null;
            MajorTicksCollection = null;
            SubTitleStyle = null;
            TitleStyle = null;
            ChartMargin = null;
            helper = null;
            maxTitleSize = null;
            maxLabelSize = null;
            bulletChartRect = null;
        }
    }
}
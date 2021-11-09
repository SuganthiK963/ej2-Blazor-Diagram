using System.Collections.Generic;
using Syncfusion.Blazor.Data;
using System;
using System.Threading.Tasks;
using System.Linq;
using Syncfusion.Blazor.Internal;
using System.Reflection;
using Syncfusion.Blazor.Sparkline.Internal;
using System.Collections;
using System.Dynamic;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSparkline<TValue>
    {
        private static List<string> chartFontKeys = new List<string>();
        private bool isNumericArray;
        private List<ExpandoObject> processedData;
        private List<string> fontKeys = new List<string>();

        internal static Dictionary<string, RectInfo> SizePerCharacter { get; set; } = new Dictionary<string, RectInfo>();

        internal double UnitX { get; set; }

        internal double UnitY { get; set; }

        internal double AxisHeight { get; set; }

        internal double AxisWidth { get; set; }

        internal double AxisValue { get; set; }

        internal string AxisColor { get; set; }

        internal List<SparklineValues> VisiblePoints { get; set; }

        internal IEnumerable SparklineData { get; set; }

        internal List<object> NumericPoints { get; set; } = new List<object>();

        internal List<SparklineDataModel> DataPoints { get; set; } = new List<SparklineDataModel>();

        private static RectInfo GetCharSize(char character, SparklineFont style)
        {
            FontInfo font = new FontInfo();
            string key = character + "_" + style.FontWeight + "_" + style.FontStyle + "_" + style.FontFamily;
            if (!SizePerCharacter.TryGetValue(key, out RectInfo charSize))
            {
                font.Chars.TryGetValue(character, out double charWidth);
                SizePerCharacter[key] = new RectInfo { Width = charWidth * 6.25, Height = 130 };
                SizePerCharacter.TryGetValue(key, out charSize);
            }

            return charSize;
        }

        internal static RectInfo MeasureTextSize(string text, SparklineFont style)
        {
            double width = 0, height = 0, fontSize = double.Parse(SparklineHelper.SizeConverter(style.Size), null);
            for (int i = 0; i < text?.Length; i++)
            {
                RectInfo charSize = GetCharSize(text[i], style);
                width += charSize.Width;
                height = Math.Max(charSize.Height, height);
            }

            return new RectInfo() { Width = (width * fontSize) / 100, Height = (height * fontSize) / 100 };
        }

        internal async Task ProcessData()
        {
            if (DataManager != null)
            {
                SparklineData = (List<object>)await DataManager.ExecuteQuery<TValue>(Query?.Clone() ?? new Data.Query());
            }
            else
            {
                SparklineData = DataSource;
            }

            DataPoints = new List<SparklineDataModel>();
            if (SparklineData == null)
            {
                return;
            }
            else if (ValueType == SparklineValueType.Numeric)
            {
                ProcessNumericData(SparklineData);
            }
            else if (ValueType == SparklineValueType.Category)
            {
                ProcessCategory(SparklineData);
            }
            else if (ValueType == SparklineValueType.DateTime)
            {
                ProcessDateTime(SparklineData);
            }
        }

        internal void ProcessNumericData(IEnumerable data)
        {
            DataProcessing(data);
            isNumericArray = DataSource != null ? DataSource.GetType().IsArray : false;
            if (isNumericArray || ((string.IsNullOrEmpty(XName) || string.IsNullOrEmpty(YName)) && ValueType == SparklineValueType.Numeric && processedData.Count > 0))
            {
                if (IsRtlEnabled())
                {
                    processedData?.Reverse();
                }

                for (int i = 0; i < processedData.Count; i++)
                {
                    IDictionary<string, object> item = processedData[i];
                    string itemValue = (item.FirstOrDefault().Value ?? string.Empty).ToString();
                    DataPoints.Add(new SparklineDataModel(i, Convert.ToDouble(itemValue, null), itemValue));
                    NumericPoints.Add(itemValue);
                }
            }
            else
            {
                foreach (IDictionary<string, object> item in processedData)
                {
                    item.TryGetValue(XName, out object xvalue);
                    item.TryGetValue(YName, out object yvalue);
                    DataPoints.Add(new SparklineDataModel(double.Parse((xvalue ?? string.Empty).ToString(), null), double.Parse((yvalue ?? string.Empty).ToString(), null), xvalue));
                }
            }
        }

        private void SetFontKeys()
        {
            List<string> keys = new List<string>();
            keys.Add(DataLabelSettings.TextStyle != null ? DataLabelSettings.TextStyle.GetFontKey() : "Segoe UI_Normal_Normal");

            if (TooltipSettings != null && Tooltip != null)
            {
                keys.Add(Tooltip.FontInfo.FontWeight + '_' + Tooltip.FontInfo.FontStyle + '_' + Tooltip.FontInfo.FontWeight);
            }

            fontKeys = keys.Distinct().ToList();
        }

        internal void ProcessDateTime(IEnumerable data)
        {
            DataProcessing(data);
            foreach (IDictionary<string, object> item in processedData)
            {
                item.TryGetValue(XName, out object xvalue);
                item.TryGetValue(YName, out object yvalue);
                DataPoints.Add(new SparklineDataModel(SparklineHelper.GetTime((DateTime)xvalue), double.Parse((yvalue ?? string.Empty).ToString(), null), xvalue));
            }
        }

        internal void ProcessCategory(IEnumerable data)
        {
            List<string> xvalues = new List<string>();
            DataProcessing(data);
            foreach (IDictionary<string, object> item in processedData)
            {
                item.TryGetValue(XName, out object xvalue);
                item.TryGetValue(YName, out object yvalue);
                string xval = (xvalue ?? string.Empty).ToString();
                if (!xvalues.Contains(xval))
                {
                    xvalues.Add(xval);
                }

                DataPoints.Add(new SparklineDataModel(xvalues.IndexOf(xval), double.Parse((yvalue ?? string.Empty).ToString(), null), xval));
            }
        }

        internal void DataProcessing(IEnumerable dataSource)
        {
            if (dataSource != null)
            {
                processedData = new List<ExpandoObject>();
                IEnumerator iterator = dataSource.GetEnumerator();
                int i = 0;
                while (iterator.MoveNext())
                {
                    PropertyInfo[] property = iterator.Current.GetType().GetProperties();
                    IDictionary<string, object> data = new ExpandoObject();
                    if (property.Length > 0)
                    {
                        foreach (PropertyInfo propertyInfo in property)
                        {
                            data[propertyInfo.Name] = propertyInfo.GetValue(iterator.Current);
                        }
                    }
                    else
                    {
                        data[i.ToString(culture)] = iterator.Current;
                    }

                    processedData.Add(data as ExpandoObject);
                    i++;
                }
            }
        }

        internal async Task FindRanges(List<SparklineDataModel> data, bool isNumericArray)
        {
            double x1, y1, y2, min = 0, max = 0, sumOfValues = 0, heightValue = AvailableSize.Height, widthValue = AvailableSize.Width,
            maxPointsLength = data.Count;
            List<SparklineDataModel> pointData = new List<SparklineDataModel>();
            SparklinePadding padValues = Padding;
            SparklineAxisSettings axis = AxisSettings;
            double axisValue = axis.Value;
            if (isNumericArray)
            {
                if (Type == SparklineType.Pie)
                {
                    sumOfValues += data.Sum(x => Math.Abs(x.YName));
                }
                else
                {
                    max = Convert.ToDouble(NumericPoints.Max(), null);
                    min = Convert.ToDouble(NumericPoints.Min(), null);
                    minXPOint = 0;
                    maxXPoint = maxPointsLength - 1;
                }
            }
            else
            {
                if (Type == SparklineType.Pie)
                {
                    sumOfValues += data.Sum(x => Math.Abs(x.YName));
                }
                else
                {
                    if (double.IsNaN(data[0].XName))
                    {
                        double[] xval = data.Select(z => z.YName).ToArray();
                        max = xval.Max();
                        min = xval.Min();
                    }
                    else
                    {
                        pointData = data.OrderBy(o => o.YName).ToList();
                        max = pointData[^1].YName;
                        min = pointData[0].YName;
                    }

                    if (!double.IsNaN(data[0].XName))
                    {
                        pointData = pointData.OrderBy(a => a.XName).ToList();
                        if (IsRtlEnabled())
                        {
                            pointData.Reverse();
                        }

                        maxXPoint = pointData[^1].XName;
                        minXPOint = pointData[0].XName;
                    }
                    else
                    {
                        minXPOint = 0;
                        maxXPoint = maxPointsLength - 1;
                    }
                }
            }

            if (Type != SparklineType.Pie)
            {
                heightValue = AvailableSize.Height - (padValues.Bottom + padValues.Top);
                widthValue = AvailableSize.Width - (padValues.Left + padValues.Right);
                maxXPoint = double.IsNaN(AxisSettings.MaxX) || AxisSettings.MaxX <= 0 ? maxXPoint : AxisSettings.MaxX;
                minXPOint = double.IsNaN(AxisSettings.MinX) || AxisSettings.MinX <= 0 ? minXPOint : AxisSettings.MinX;
                max = double.IsNaN(AxisSettings.MaxY) || AxisSettings.MaxY <= 0 ? max : AxisSettings.MaxY;
                min = double.IsNaN(AxisSettings.MinY) || AxisSettings.MinY <= 0 ? min : AxisSettings.MinY;
                string color = axis.LineSettings != null && !string.IsNullOrEmpty(axis.LineSettings.Color) ? axis.LineSettings.Color : ThemeStyle.AxisLine;
                AxisRenderingEventArgs args = new AxisRenderingEventArgs()
                {
                    MaxX = maxXPoint,
                    MinX = minXPOint,
                    MaxY = max,
                    MinY = min,
                    Value = axisValue,
                    LineColor = color,
                    LineWidth = axis.LineSettings != null ? axis.LineSettings.Width : 1
                };
                await SfBaseUtils.InvokeEvent<AxisRenderingEventArgs>(Events?.OnAxisRendering, args);
                if (args.Cancel)
                {
                    VisiblePoints = null;
                    return;
                }

                maxXPoint = args.MaxX;
                minXPOint = args.MinX;
                max = args.MaxY;
                min = args.MinY;
                axisValue = AxisValue = args.Value;
                AxisColor = args.LineColor;
                AxisWidth = args.LineWidth;
            }

            double unitX = maxXPoint - minXPOint, unitY = max - min;
            unitX = (unitX == 0) ? 1 : unitX;
            unitY = (unitY == 0) ? 1 : unitY;
            UnitX = unitX;
            UnitY = unitY;
            minPoint = min;
            maxPoint = max;
            x1 = 0;
            y1 = min < 0 && max <= 0 ? 0 : min < 0 && max > 0 ? (heightValue - ((heightValue / UnitY) * (-min))) : heightValue;
            if (axisValue >= min && axisValue <= max)
            {
                y1 = heightValue - Math.Round(heightValue * ((axisValue - min) / UnitY));
            }

            AxisHeight = y1 + padValues.Top;
            List<SparklineValues> visiblePoints = new List<SparklineValues>();
            double percent, x, y, delta = max - min,
            interval = GetInterval(data),
            intervalPadding = GetPaddingInterval(data, delta);
            for (int i = 0; i < maxPointsLength; i++)
            {
                SparklineValues point = new SparklineValues();
                if ((double.IsNaN(data[i].XName) && double.IsNaN(data[i].YName) && (data[i].YName != 0) && isNumericArray) || double.IsNaN(data[i].XName))
                {
                    x = i;
                    y = data[i].YName;
                }
                else
                {
                    x = data[i].XName;
                    y = data[i].YName;
                }

                if (double.IsNaN(x) || double.IsNaN(y))
                {
                    continue;
                }

                if (Type == SparklineType.Line || Type == SparklineType.Area)
                {
                    y2 = (min != max && maxPointsLength != 1) ? heightValue - Math.Round(heightValue * ((y - min) / UnitY)) : padValues.Top;
                    point.X = (minXPOint != maxXPoint) ? Math.Round(widthValue * ((x - minXPOint) / UnitX)) : widthValue / 2;
                    point.Y = y2;
                    point.MarkerPosition = y2;
                }
                else if (Type == SparklineType.Column || Type == SparklineType.WinLoss)
                {
                    double colWidth = (widthValue / (((maxXPoint - minXPOint) / interval) + 1)) - 1;
                    x1 = (((x - minXPOint) / interval) * (colWidth + 1)) + 0.5;
                    if (Type == SparklineType.WinLoss)
                    {
                        double winLossFactor = 0.5, drawHeightFactor = 40;
                        y2 = (y > axisValue) ? (heightValue / 4) : (y < axisValue) ? (heightValue * winLossFactor) : ((heightValue * winLossFactor) - (heightValue / drawHeightFactor));
                        point.X = x1;
                        point.Y = y2;
                        point.Height = y != axisValue ? heightValue / 4 : heightValue / 20;
                        point.Width = colWidth;
                        point.MarkerPosition = y2 > y1 ? y1 + Math.Abs(y2 - y1) : y2;
                    }
                    else
                    {
                        if ((y == min && RangePadding == SparklineRangePadding.Additional) || (y == max && RangePadding == SparklineRangePadding.Additional))
                        {
                            min -= intervalPadding + padValues.Top;
                            max += intervalPadding + padValues.Top;
                            unitX = (maxXPoint - minXPOint == 0) ? 1 : maxXPoint - minXPOint;
                            unitY = (max - min == 0) ? 1 : max - min;
                            UnitX = unitX;
                            UnitY = unitY;
                            minPoint = min;
                            maxPoint = max;
                        }
                        else if ((y == min && RangePadding == SparklineRangePadding.Normal) || (y == max && RangePadding == SparklineRangePadding.Normal))
                        {
                            min -= intervalPadding;
                            max += intervalPadding;
                            unitX = maxXPoint - minXPOint == 0 ? 1 : maxXPoint - minXPOint;
                            unitY = max - min == 0 ? 1 : max - min;
                            UnitX = unitX;
                            UnitY = unitY;
                            minPoint = min;
                            maxPoint = max;
                        }

                        double z = (heightValue / UnitY) * (y - min),
                        z1 = (y == min && y > axisValue) ? ((maxPointsLength != 1 && UnitY != 1) ? (heightValue / UnitY) * (min / 2) : (!double.IsNaN(z) ? z : 1)) : (y == max && y < axisValue && maxPointsLength != 1 && UnitY != 1) ? (heightValue / UnitY) * (-max / 2) : z;
                        y2 = Math.Abs(heightValue - z1);
                        point.X = x1;
                        point.Y = (y2 > y1) ? y1 : y2;
                        point.Height = Math.Abs(y2 - y1);
                        point.Width = colWidth;
                        point.MarkerPosition = (y2 > y1) ? (y1 + Math.Abs(y2 - y1)) : y2;
                    }
                }
                else if (Type == SparklineType.Pie)
                {
                    percent = (Math.Abs(y) / sumOfValues) * 100;
                    point.Percent = percent;
                    point.Degree = Math.Abs(y) / sumOfValues * 360;
                }

                if (Type != SparklineType.Pie)
                {
                    point.X += padValues.Left;
                    point.Y += padValues.Top;
                }

                if (Type != SparklineType.WinLoss)
                {
                    point.MarkerPosition += padValues.Top;
                }

                point.Location.X = point.X;
                point.Location.Y = point.Y;
                point.XVal = x;
                point.YVal = y;
                point.XName = data[i].XValue;
                visiblePoints.Add(point);
            }

            VisiblePoints = visiblePoints.OrderBy(a => a.X).ToList();
        }

        internal double GetInterval(List<SparklineDataModel> dataModel)
        {
            double x1 = dataModel[0].XName, x2 = dataModel.Count > 1 ? dataModel[1].XName : double.NaN;
            if (!double.IsNaN(x1) && !double.IsNaN(x2))
            {
                List<SparklineDataModel> validData = new List<SparklineDataModel>();
                foreach (SparklineDataModel data in dataModel)
                {
                    if (!double.IsNaN(data.XName))
                    {
                        validData.Add(data);
                    }
                }

                validData = validData.OrderBy(a => a.XName).ToList();
                if (IsRtlEnabled())
                {
                    validData.Reverse();
                }

                return validData[1].XName - validData[0].XName;
            }

            return 1;
        }

        internal double GetPaddingInterval(List<SparklineDataModel> data, double delta)
        {
            double interval = 1, size = AvailableSize.Height, intervalCount = Math.Max(size * ((interval * data.Count) / 100), 1),
            niceInterval = delta / intervalCount;
            foreach (double intervalVal in new double[] { 10, 5, 2, 1 })
            {
                double currentInterval = interval * intervalVal;
                if (intervalCount < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }

        internal bool IsRtlEnabled()
        {
            return EnableRtl || SyncfusionService.options.EnableRtl;
        }
    }
}
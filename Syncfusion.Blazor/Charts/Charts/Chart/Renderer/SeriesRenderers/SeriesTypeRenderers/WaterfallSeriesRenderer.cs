using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class WaterfallSeriesRenderer : ColumnBaseRenderer
    {
        protected List<PathOptions> WaterFallPathOptions { get; set; }

        private static bool IsSumIndex(ChartSeries series, double index)
        {
            if (series.SumIndexes != null && Array.IndexOf(series.SumIndexes, index) != -1)
            {
                return true;
            }

            return false;
        }

        private static bool IsIntermediateSum(ChartSeries series, double index)
        {
            if (series.IntermediateSumIndexes != null && Array.IndexOf(series.IntermediateSumIndexes, index) != -1)
            {
                return true;
            }

            return false;
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            if (Series.Visible)
            {
                GetSetWaterfallPathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            GetSetWaterfallPathOption();
            base.UpdateDirection();
        }

        private void GetSetWaterfallPathOption()
        {
            WaterFallPathOptions = new List<PathOptions>();
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, 0), prevEndValue = 0, currentEndValue = 0, originValue, y, intermediateOrigin = 0;
            string direction = string.Empty;
            string connectorDirection = string.Empty;
            Rect prevRegion = null;
            bool isInversed = Series.Container.RequireInvertedAxis;
            string pointId = Series.Container.ID + "_Series_" + Index + "_Point_", id;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            PathOptions option;
            foreach (Point point in Points)
            {
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(point.Index - 1 > -1 ? Points[point.Index - 1] : null, point, point.Index + 1 < Points.Count ? Points[point.Index + 1] : null, XAxisRenderer))
                {
                    bool isSum = IsIntermediateSum(Series, point.Index);
                    bool totalSum = IsSumIndex(Series, point.Index);
                    currentEndValue += isSum || totalSum == true ? 0 : point.YValue;
                    originValue = isSum == true ? intermediateOrigin : ((!double.IsNaN(prevEndValue) && !totalSum) ? prevEndValue : origin);
                    Rect rect = GetRectangle(point.XValue + sideBySideInfo.Start, currentEndValue, point.XValue + sideBySideInfo.End, originValue);
                    PointRenderEventArgs argsData = TriggerPointRenderEvent(Series, point);
                    if (isSum)
                    {
                        intermediateOrigin = currentEndValue;
                    }

                    prevEndValue = currentEndValue;
                    if (!argsData.Cancel)
                    {
                        UpdateSymbolLocation(point, rect);
                        id = pointId + point.Index;
                        direction = CalculateRectangle(point, rect, id);
                        if (direction != null)
                        {
                            option = new PathOptions(id, direction, Series.DashArray, argsData.Border.Width, argsData.Border.Color, Series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText)
                            {
                                Visibility = visibility
                            };
                            WaterFallPathOptions.Add(option);
                        }
                    }

                    Rect currentRegion = point.Regions[0];
                    if (prevRegion != null)
                    {
                        double prevBottom, currentBottom, prevLeft = isInversed ? prevRegion.X : prevRegion.Y, currentLeft = isInversed ? currentRegion.X : currentRegion.Y,
                        currentYValue = currentRegion.Y, currentXValue = currentRegion.X;
                        Point beforePoint = Points[point.Index - 1];
                        if (point.YValue == 0)
                        {
                            prevBottom = isInversed ? prevRegion.X + prevRegion.Width : prevRegion.Y + prevRegion.Height;
                            currentBottom = isInversed ? point.SymbolLocations[0].X : point.SymbolLocations[0].Y;
                        }
                        else
                        {
                            prevBottom = isInversed ? (beforePoint.YValue == 0) ? beforePoint.SymbolLocations[0].X : prevRegion.X + prevRegion.Width : (beforePoint.YValue == 0) ? beforePoint.SymbolLocations[0].Y : prevRegion.Y + prevRegion.Height;
                            currentBottom = isInversed ? currentRegion.X + currentRegion.Width : currentRegion.Y + currentRegion.Height;
                        }

                        if (Math.Round(prevLeft) == Math.Round(currentLeft) ||
                            Math.Round(prevBottom) == Math.Round(currentLeft))
                        {
                            y = isInversed ? (currentRegion.X == 0 && prevRegion.X == 0) ? currentBottom : currentRegion.X : currentRegion.Y;
                            y = point.YValue == 0 ? (isInversed ? point.SymbolLocations[0].X : point.SymbolLocations[0].Y) : y;
                        }
                        else
                        {
                            y = currentBottom;
                        }

                        if (isInversed)
                        {
                            if (beforePoint.YValue == 0)
                            {
                                prevRegion.Y = ((prevRegion.Y + (prevRegion.Height / 2)) + (rect.Height / 2)) - prevRegion.Height;
                            }

                            if (point.YValue == 0)
                            {
                                currentYValue = (currentRegion.Y + (currentRegion.Height / 2)) - (rect.Height / 2);
                            }

                            connectorDirection = string.Concat(connectorDirection, "M" + SPACE + y.ToString(Culture) + SPACE + (prevRegion.Y + prevRegion.Height).ToString(Culture) + SPACE + "L" + SPACE + y.ToString(Culture) + SPACE + currentYValue.ToString(Culture) + SPACE);
                        }
                        else
                        {
                            if (beforePoint.YValue == 0)
                            {
                                prevRegion.X = (prevRegion.X + (prevRegion.Width / 2)) - (rect.Width / 2);
                                currentXValue = ((currentRegion.X + (currentRegion.Width / 2)) + (rect.Width / 2)) - currentRegion.Width;
                            }

                            connectorDirection = string.Concat(connectorDirection, "M" + SPACE + prevRegion.X.ToString(Culture) + SPACE + y.ToString(Culture) + SPACE + "L" + SPACE + (currentXValue + currentRegion.Width).ToString(Culture) + SPACE + y.ToString(Culture) + SPACE);
                        }
                    }

                    prevRegion = point.Regions[0];
                }
            }

            string connectorId = Series.Container.ID + "_Series_" + Index + "_Connector_";

            if (direction != null)
            {
                option = new PathOptions(connectorId, connectorDirection, Series.Connector.DashArray, Series.Connector.Width, Series.Connector.Color, Series.Opacity, "none", string.Empty, string.Empty, AccessText);
                option.Visibility = "visible";
                WaterFallPathOptions.Add(option);
            }
        }

        internal override EmptyPointMode GetEmptyPointMode(EmptyPointMode mode)
        {
            return EmptyPointMode.Zero;
        }

        private PointRenderEventArgs TriggerPointRenderEvent(ChartSeries series, Point point)
        {
            string color;
            if (IsIntermediateSum(series, point.Index) || IsSumIndex(series, point.Index))
            {
                color = series.SummaryFillColor;
            }
            else if ((double)point.Y < 0)
            {
                color = series.NegativeFillColor;
            }
            else
            {
                color = Interior;
            }

            return TriggerEvent(point, color, new BorderModel { Color = series.Border.Color, Width = series.Border.Width });
        }

        public override string SetPointColor(Point point, string color)
        {
            color = !string.IsNullOrEmpty(color) ? color : point?.Interior;
            return (bool)point?.IsEmpty ? (Series.EmptyPointSettings.Fill != null ? Series.EmptyPointSettings.Fill : color) : color;
        }

        internal void ProcessInternalData(out Type type, out IEnumerable<object> currentViewData)
        {
            currentViewData = CurrentViewData;
            type = currentViewData.ToArray().First().GetType();
            object[] data = currentViewData.ToArray();
            double index;
            double[] intermediateSum = Series.IntermediateSumIndexes;
            double[] sumIndex = Series.SumIndexes;
            if (intermediateSum != null && intermediateSum.Length > 0)
            {
                for (int i = 0; i < intermediateSum.Length; i++)
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (j == intermediateSum[i])
                        {
                            if (i == 0)
                            {
                                index = ChartHelper.SubArraySum(data, -1, intermediateSum[i], null, Series, type);
                            }
                            else
                            {
                                index = ChartHelper.SubArraySum(data, intermediateSum[i - 1], intermediateSum[i], null, Series, type);
                            }

                            type.GetProperty(Series.YName).SetValue(data[j], index);
                        }
                    }
                }
            }

            if (sumIndex != null && sumIndex.Length > 0)
            {
                for (int k = 0; k < sumIndex.Length; k++)
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (j == sumIndex[k])
                        {
                            if (intermediateSum != null)
                            {
                                index = ChartHelper.SubArraySum(data, intermediateSum[k] - 1, sumIndex[k], sumIndex, Series, type);
                            }
                            else
                            {
                                index = ChartHelper.SubArraySum(data, -1, sumIndex[k], null, Series, type);
                            }

                            type.GetProperty(Series.YName).SetValue(data[j], index);
                        }
                    }
                }
            }

            currentViewData = data;
        }

        internal override void ProcessData()
        {
            Point point = new Point();
            SeriesRenderEventArgs eventArgs = new SeriesRenderEventArgs("OnSeriesRender", false, Interior, CurrentViewData, Series);
            SfChart.InvokeEvent(Owner.ChartEvents?.OnSeriesRender, eventArgs);
            CurrentViewData = eventArgs.Data;
            Interior = eventArgs.Fill;
            ProcessInternalData(out Type datasourceType, out IEnumerable<object> data);
            int length = data.Count();
            XData = new double[length].ToList();
            IEnumerable<object> waterfallData = data.ToArray();
            string dataType = DataVizCommonHelper.FindDataType(datasourceType);
            string x_Name = Series.XName;
            string y_Name = Series.YName;
            switch (dataType)
            {
                case Constants.JOBJECT:
                    ProcessJObjectData(datasourceType, x_Name, y_Name, waterfallData);
                    break;
                case Constants.EXPANDOOBJECT:
                    ProcessExpandoObjectData(datasourceType, x_Name, y_Name, waterfallData);
                    break;
                case Constants.DYNAMICOBJECT:
                    ProcessDynamicObjectData(datasourceType, x_Name, y_Name, waterfallData);
                    break;
                default:
                    ProcessObjectData(datasourceType, x_Name, y_Name, waterfallData);
                    break;
            }
        }

        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            WaterFallPathOptions.ForEach(option => option.Visibility = visibility);
            switch (property)
            {
                case "Fill":
                    Interior = Series.Fill;
                    WaterFallPathOptions.ForEach(option => option.Fill = Series.Fill);
                    break;
                case "DashArray":
                    WaterFallPathOptions.ForEach(option => option.StrokeDashArray = Series.DashArray);
                    break;
                case "Width":
                    WaterFallPathOptions.ForEach(option => option.StrokeWidth = Series.Border.Width);
                    break;
                case "Color":
                    WaterFallPathOptions.ForEach(option => option.Stroke = Series.Border.Color);
                    break;
                case "Opacity":
                    WaterFallPathOptions.ForEach(option => option.Opacity = Series.Opacity);
                    break;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            foreach (PathOptions option in WaterFallPathOptions)
            {
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }
    }
}
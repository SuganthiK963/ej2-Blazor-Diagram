using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Globalization;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartErrorBarRenderer : ChartRenderer
    {
        private const string SPACE = " ";

        private List<PathOptions> errorBarSpaceOption;

        private List<PathOptions> errorBarCapOption;

        private string seriesIndex;

        private CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        [Parameter]
        public ChartSeries Series { get; set; }

        internal ChartSeriesRenderer SeriesRenderer { get; set; }

        internal double PositiveHeight { get; set; }

        internal double NegativeHeight { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series.ErrorBar.Renderer = this;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = Series.ErrorBar.Visible;
            SeriesRenderer = Series.Renderer;
            seriesIndex = Convert.ToString(SeriesRenderer.Index, CultureInfo.InvariantCulture);
            if (RendererShouldRender)
            {
                RenderErrorBar();
                SeriesRenderer.CalculateErrorBarClipPath();
            }
        }

        private void RenderErrorBar()
        {
            if (!SeriesRenderer.Series.Visible)
            {
                return;
            }

            errorBarSpaceOption = new List<PathOptions>();
            errorBarCapOption = new List<PathOptions>();
            ChartErrorBarSettings errorbar = Series.ErrorBar;
            ChartErrorBarCapSettings errorBarCap = Series.ErrorBar.ErrorBarCap;
            string[] errorDirection = new string[] { string.Empty, string.Empty };
            double errorX, errorY;
            foreach (Point point in Series.Renderer.Points)
            {
                if (point.Visible && point.SymbolLocations.Count > 0 && point.SymbolLocations[0] != null)
                {
                    errorX = 0;
                    errorY = 0;
                    switch (errorbar.Mode)
                    {
                        case ErrorBarMode.Vertical:
                            errorY = errorbar.VerticalError;
                            break;
                        case ErrorBarMode.Horizontal:
                            errorX = errorbar.HorizontalError;
                            break;
                        case ErrorBarMode.Both:
                            errorX = errorbar.HorizontalError;
                            errorY = errorbar.VerticalError;
                            break;
                    }

                    switch (errorbar.Type)
                    {
                        case ErrorBarType.Custom:
                            errorDirection = CalculateCustomValue(point, Owner.RequireInvertedAxis, errorX, errorY);
                            break;
                        case ErrorBarType.Fixed:
                            errorDirection = CalculateFixedValue(point, Owner.RequireInvertedAxis, errorX, errorY);
                            break;
                        case ErrorBarType.Percentage:
                            errorDirection = CalculatePercentageValue(point, Owner.RequireInvertedAxis, errorX, errorY);
                            break;
                        case ErrorBarType.StandardDeviation:
                            errorDirection = CalculateStandardDeviationValue(point, Owner.RequireInvertedAxis, errorX, errorY);
                            break;
                        case ErrorBarType.StandardError:
                            errorDirection = CalculateStandardErrorValue(point, Owner.RequireInvertedAxis, errorX, errorY);
                            break;
                    }

                    PathOptions shapeOption = new PathOptions
                    {
                        Id = Owner.ID + "_Series_" + "_ErrorBarGroup_" + Series.Renderer.Index + "_Point_" + point.Index,
                        Fill = string.Empty,
                        StrokeWidth = errorbar.Width,
                        Stroke = !string.IsNullOrEmpty(errorbar.Color) ? errorbar.Color : Owner.ChartThemeStyle.ErrorBar,
                        Opacity = 1,
                        StrokeDashArray = string.Empty,
                        Direction = errorDirection[0]
                    };
                    shapeOption.Direction = ChartHelper.AppendPathElements(Owner, shapeOption.Direction, shapeOption.Id);
                    errorBarSpaceOption.Add(shapeOption);
                    Series.Renderer.DynamicOptions.ErrorShapeId.Add(shapeOption.Id);
                    Series.Renderer.DynamicOptions.ErrorShapeCDir.Add(errorDirection[0]);
                    PathOptions capOption = new PathOptions(Owner.ID + "_Series_" + "_ErrorBarCap_" + Series.Renderer.Index + "_Point_" + point.Index, errorDirection[1], string.Empty, errorBarCap.Width, !string.IsNullOrEmpty(errorBarCap.Color) ? errorBarCap.Color : Owner.ChartThemeStyle.ErrorBar, errorBarCap.Opacity, string.Empty);
                    capOption.Direction = ChartHelper.AppendPathElements(Owner, capOption.Direction, capOption.Id);
                    errorBarCapOption.Add(capOption);
                    Series.Renderer.DynamicOptions.ErrorCapId.Add(capOption.Id);
                    Series.Renderer.DynamicOptions.ErrorCapCDir.Add(errorDirection[1]);
                }
            }
        }

        private string[] FindLocation(Point point, bool isInverted, double x1, double y1)
        {
            ChartErrorBarSettings errorbar = Series.ErrorBar;
            ErrorBarDirection direction = errorbar.Direction;
            List<ChartInternalLocation> location = new List<ChartInternalLocation>();
            FinancialPoint financialPoint = point as FinancialPoint;
            double y_Value = Series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture) ? Series.Renderer.StackedValues.EndValues[point.Index] : (Series.Renderer.SeriesType() == SeriesValueType.HighLow || Series.Renderer.SeriesType() == SeriesValueType.HighLowOpenClose) ? (double)financialPoint.High : point.YValue;
            ChartInternalLocation startPoint = ChartHelper.GetPoint(
                Series.Renderer.XAxisRenderer.GetPointValue(point.XValue) + ((direction == ErrorBarDirection.Plus || direction == ErrorBarDirection.Both) ? (errorbar.Type == ErrorBarType.Custom && (errorbar.Mode == ErrorBarMode.Horizontal || errorbar.Mode == ErrorBarMode.Both)) ? x1 = errorbar.HorizontalPositiveError : x1 : 0),
                Series.Renderer.YAxisRenderer.GetPointValue(y_Value) + ((direction == ErrorBarDirection.Plus || direction == ErrorBarDirection.Both) ? (errorbar.Type == ErrorBarType.Custom && (errorbar.Mode == ErrorBarMode.Vertical || errorbar.Mode == ErrorBarMode.Both)) ? y1 = errorbar.VerticalPositiveError : y1 : 0),
                Series.Renderer.XAxisRenderer,
                Series.Renderer.YAxisRenderer,
                isInverted);
            location.Add(startPoint);
            if (Series.Renderer.IsRectSeries())
            {
                location.Add(point.SymbolLocations[0]);
            }
            else
            {
                location.Add(ChartHelper.GetPoint(Series.Renderer.XAxisRenderer.GetPointValue(point.XValue), Series.Renderer.YAxisRenderer.GetPointValue(point.YValue), Series.Renderer.XAxisRenderer, Series.Renderer.YAxisRenderer, isInverted));
            }

            ChartInternalLocation endPoint = ChartHelper.GetPoint(
                Series.Renderer.XAxisRenderer.GetPointValue(point.XValue) - ((direction == ErrorBarDirection.Minus || direction == ErrorBarDirection.Both) ? (errorbar.Type == ErrorBarType.Custom && (errorbar.Mode == ErrorBarMode.Horizontal || errorbar.Mode == ErrorBarMode.Both)) ? x1 = errorbar.HorizontalNegativeError : x1 : 0),
                Series.Renderer.YAxisRenderer.GetPointValue(y_Value) - ((direction == ErrorBarDirection.Minus || direction == ErrorBarDirection.Both) ? (errorbar.Type == ErrorBarType.Custom && (errorbar.Mode == ErrorBarMode.Vertical || errorbar.Mode == ErrorBarMode.Both)) ? y1 = errorbar.VerticalNegativeError : y1 : 0),
                Series.Renderer.XAxisRenderer,
                Series.Renderer.YAxisRenderer,
                isInverted);
            location.Add(endPoint);
            point.Error = (errorbar.Mode == ErrorBarMode.Vertical) ? errorbar.VerticalError : errorbar.HorizontalError;
            NegativeHeight = (errorbar.Mode == ErrorBarMode.Vertical || errorbar.Mode == ErrorBarMode.Both) ? (isInverted ? (location[1].X - location[2].X) : location[2].Y - location[1].Y) : 0;
            PositiveHeight = (errorbar.Mode == ErrorBarMode.Vertical || errorbar.Mode == ErrorBarMode.Both) ? (isInverted ? (location[0].X - location[1].X) : location[1].Y - location[0].Y) : 0;
            return GetErrorDirection(location[0], location[1], location[2], isInverted);
        }

        private string[] CalculateFixedValue(Point point, bool isInverted, double errorX, double errorY)
        {
            return FindLocation(point, isInverted, errorX, errorY);
        }

        private string[] CalculatePercentageValue(Point point, bool isInverted, double errorX, double errorY)
        {
            errorX = (errorX / 100) * point.XValue;
            errorY = (errorY / 100) * point.YValue;
            return FindLocation(point, isInverted, errorX, errorY);
        }

        private string[] CalculateStandardDeviationValue(Point point, bool isInverted, double errorX, double errorY)
        {
            Mean getMean = MeanCalculation(Series.ErrorBar.Mode);
            errorX = errorX * (getMean.HorizontalSquareRoot + getMean.HorizontalMean);
            errorY = errorY * (getMean.VerticalSquareRoot + getMean.VerticalMean);
            return FindLocation(point, isInverted, errorX, errorY);
        }

        private string[] CalculateStandardErrorValue(Point point, bool isInverted, double errorX, double errorY)
        {
            Mean getMean = MeanCalculation(Series.ErrorBar.Mode);
            errorX = (errorX * getMean.HorizontalSquareRoot) / Math.Sqrt(Series.Renderer.Points.Count);
            errorY = (errorY * getMean.VerticalSquareRoot) / Math.Sqrt(Series.Renderer.Points.Count);
            return FindLocation(point, isInverted, errorX, errorY);
        }

        private string[] CalculateCustomValue(Point point, bool isInverted, double errorX, double errorY)
        {
            return FindLocation(point, isInverted, errorX, errorY);
        }

#pragma warning disable CA1822
        private string[] GetHorizontalDirection(ChartInternalLocation start, ChartInternalLocation mid, ChartInternalLocation end, ErrorBarDirection direction, double capLength)
        {
            string path = string.Empty, capDirection = string.Empty;
            path += "M " + start.X.ToString(Culture) + SPACE + mid.Y.ToString(Culture) + " L " + end.X.ToString(Culture) + SPACE + mid.Y.ToString(Culture);
            capDirection += (direction == ErrorBarDirection.Plus || direction == ErrorBarDirection.Both) ? "M " + start.X.ToString(Culture) + SPACE + (mid.Y - capLength).ToString(Culture) + " L " + start.X.ToString(Culture) + SPACE + (mid.Y + capLength).ToString(Culture) : string.Empty;
            capDirection += (direction == ErrorBarDirection.Minus || direction == ErrorBarDirection.Both) ? "M " + end.X.ToString(Culture) + SPACE + (mid.Y - capLength).ToString(Culture) + " L " + end.X.ToString(Culture) + SPACE + (mid.Y + capLength).ToString(Culture) : SPACE;
            return new string[] { path, capDirection };
        }

        private string[] GetVerticalDirection(ChartInternalLocation start, ChartInternalLocation mid, ChartInternalLocation end, ErrorBarDirection direction, double capLength)
        {
            string path = string.Empty, capDirection = string.Empty;
            path += "M " + mid.X.ToString(Culture) + SPACE + start.Y.ToString(Culture) + " L " + mid.X.ToString(Culture) + SPACE + end.Y.ToString(Culture);
            capDirection += direction == ErrorBarDirection.Plus || direction == ErrorBarDirection.Both ? "M " + (mid.X - capLength).ToString(Culture) + SPACE + start.Y.ToString(Culture) + " L " + (mid.X + capLength).ToString(Culture) + SPACE + start.Y.ToString(Culture) : string.Empty;
            capDirection += direction == ErrorBarDirection.Minus || direction == ErrorBarDirection.Both ? "M " + (mid.X - capLength).ToString(Culture) + SPACE + end.Y.ToString(Culture) + " L " + (mid.X + capLength).ToString(Culture) + SPACE + end.Y.ToString(Culture) : string.Empty;
            return new string[] { path, capDirection };
        }

        private Mean MeanCalculation(ErrorBarMode mode)
        {
            double sumOfX = 0, sumOfY = 0, verticalMean = 0, horizontalMean = 0;
            int length = Series.Renderer.Points.Count;
            switch (mode)
            {
                case ErrorBarMode.Vertical:
                    sumOfY = Series.Renderer.YData.Sum();
                    verticalMean = sumOfY / length;
                    break;
                case ErrorBarMode.Horizontal:
                    sumOfX = Series.Renderer.XData.Sum();
                    horizontalMean = sumOfX / length;
                    break;
                case ErrorBarMode.Both:
                    sumOfY = Series.Renderer.YData.Sum();
                    verticalMean = sumOfY / length;
                    sumOfX = Series.Renderer.XData.Sum();
                    horizontalMean = sumOfX / length;
                    break;
            }

            foreach (Point point in Series.Renderer.Points)
            {
                if (mode == ErrorBarMode.Vertical)
                {
                    sumOfY = sumOfY + Math.Pow(point.YValue - verticalMean, 2);
                }
                else if (mode == ErrorBarMode.Horizontal)
                {
                    sumOfX = sumOfX + Math.Pow(point.XValue - horizontalMean, 2);
                }
                else
                {
                    sumOfY = sumOfY + Math.Pow(point.YValue - verticalMean, 2);
                    sumOfX = sumOfX + Math.Pow(point.XValue - horizontalMean, 2);
                }
            }

            return new Mean(sumOfY / (length - 1), Math.Sqrt(sumOfY / (length - 1)), sumOfX / (length - 1), Math.Sqrt(sumOfX / (length - 1)), verticalMean, horizontalMean);
        }

        private string[] GetBothDirection(ChartInternalLocation start, ChartInternalLocation mid, ChartInternalLocation end, ErrorBarDirection direction, double capLength)
        {
            string[] pathH = GetHorizontalDirection(start, mid, end, direction, capLength);
            string[] pathV = GetVerticalDirection(start, mid, end, direction, capLength);
            return new string[] { string.Concat(pathH[0], pathV[0]), string.Concat(pathH[1], pathV[1]) };
        }

        private string[] GetErrorDirection(ChartInternalLocation start, ChartInternalLocation mid, ChartInternalLocation end, bool isInverted)
        {
            ErrorBarDirection direction = Series.ErrorBar.Direction;
            ErrorBarMode mode = Series.ErrorBar.Mode;
            string[] paths = new string[] { string.Empty, string.Empty };
            ErrorBarMode errorMode = mode;
            double capLength = Series.ErrorBar.ErrorBarCap.Length;
            switch (mode)
            {
                case ErrorBarMode.Both:
                    errorMode = mode;
                    break;
                case ErrorBarMode.Horizontal:
                    errorMode = isInverted ? ErrorBarMode.Vertical : mode;
                    break;
                case ErrorBarMode.Vertical:
                    errorMode = isInverted ? ErrorBarMode.Horizontal : mode;
                    break;
            }

            switch (errorMode)
            {
                case ErrorBarMode.Horizontal:
                    paths = GetHorizontalDirection(start, mid, end, direction, capLength);
                    break;
                case ErrorBarMode.Vertical:
                    paths = GetVerticalDirection(start, mid, end, direction, capLength);
                    break;
                case ErrorBarMode.Both:
                    paths = GetBothDirection(start, mid, end, direction, capLength);
                    break;
            }

            return new string[] { paths[0], paths[1] };
        }

        public void InvalidateRender()
        {
            StateHasChanged();
        }

        internal void UpdateCustomization(string property)
        {
            RendererShouldRender = Series.ErrorBar.Visible;
            if (RendererShouldRender)
            {
                switch (property)
                {
                    case "Color":
                        UpdateErrorBarColor();
                        break;
                    case "Width":
                        UpdateErrorBarWidth();
                        break;
                }

                InvalidateRender();
            }
        }

        internal void UpdateCapCustomization(string property)
        {
            RendererShouldRender = Series.ErrorBar.Visible;
            if (RendererShouldRender)
            {
                switch (property)
                {
                    case "Color":
                        UpdateErrorBarCapColor();
                        break;
                    case "Width":
                        UpdateErrorBarCapWidth();
                        break;
                    case "Opacity":
                        UpdateErrorBarCapOpacity();
                        break;
                    case "Length":
                        UpdateErrorBarCapLength();
                        break;
                }

                InvalidateRender();
            }
        }

        private void UpdateErrorBarColor()
        {
            string fill = !string.IsNullOrEmpty(Series.ErrorBar.Color) ? Series.ErrorBar.Color : Owner.ChartThemeStyle.ErrorBar;
            foreach (PathOptions option in errorBarSpaceOption)
            {
                option.Stroke = fill;
            }
        }

        private void UpdateErrorBarCapColor()
        {
            string fill = !string.IsNullOrEmpty(Series.ErrorBar.ErrorBarCap.Color) ? Series.ErrorBar.ErrorBarCap.Color : Owner.ChartThemeStyle.ErrorBar;
            foreach (PathOptions option in errorBarCapOption)
            {
                option.Stroke = fill;
            }
        }

        internal void UpdateErrorBar()
        {
            RendererShouldRender = Series.ErrorBar.Visible;
            if (RendererShouldRender)
            {
                RenderErrorBar();
                Series.Marker.DataLabel.Renderer?.DatalabelValueChanged();
                ProcessRenderQueue();
            }
        }

        private void UpdateErrorBarCapOpacity()
        {
            double opacity = Series.ErrorBar.ErrorBarCap.Opacity;
            foreach (PathOptions option in errorBarCapOption)
            {
                option.Opacity = opacity;
            }
        }

        internal void UpdateErrorBarWidth()
        {
            double width = Series.ErrorBar.Width;
            foreach (PathOptions option in errorBarSpaceOption)
            {
                option.StrokeWidth = width;
            }
        }

        internal void UpdateErrorBarCapWidth()
        {
            double width = Series.ErrorBar.ErrorBarCap.Width;
            foreach (PathOptions option in errorBarCapOption)
            {
                option.StrokeWidth = width;
            }
        }

        internal void UpdateErrorBarCapLength()
        {
            double length = Series.ErrorBar.ErrorBarCap.Length;
            foreach (PathOptions option in errorBarCapOption)
            {
                option.StrokeWidth = length;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (SeriesRenderer != null && Series.ErrorBar.Visible)
            {
                SeriesRenderer.RenderErrorBarClipPath(builder);
                if (builder != null)
                {
                    foreach (PathOptions option in errorBarSpaceOption)
                    {
                        Owner.SvgRenderer.RenderPath(builder, option);
                    }

                    foreach (PathOptions option in errorBarCapOption)
                    {
                        Owner.SvgRenderer.RenderPath(builder, option);
                    }

                    builder.CloseElement();
                }
            }
        }
    }
}
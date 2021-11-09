using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartStriplineRenderer : ChartRenderer, IChartElementRenderer
    {
        private const string SPACE = " ";

        private Rect rect;

        private Rect seriesClipRect;

        private CultureInfo culture = CultureInfo.InvariantCulture;

        private ChartAxisRenderer axisRenderer;

        private ChartAxis axis;

        private PathOptions striplinePath;

        private List<RectOptions> striplineRect = new List<RectOptions>();

        private List<TextOptions> striplineText = new List<TextOptions>();

        internal ChartStripline Stripline { get; set; }

        internal int Index { get; set; }

        private static double FindValue(double range, double min, double max)
        {
            if (range < min)
            {
                return min;
            }
            else if (range > max)
            {
                return max;
            }

            return range;
        }

        private static double GetTextStart(double xy, double size, Anchor textAlignment)
        {
            switch (textAlignment)
            {
                case Anchor.Start:
                    xy = xy - (size * 0.5) + 5;
                    break;
                case Anchor.End:
                    xy = xy + (size * 0.5) - 5;
                    break;
            }

            return xy;
        }

        private static double Factor(Anchor anchor)
        {
            double factor = 0;
            if (anchor == Anchor.Middle)
            {
                return factor;
            }
            else
            {
                return anchor == Anchor.Start ? 1 : -1;
            }
        }

        private static Anchor InvertAlignment(Anchor anchor)
        {
            if (anchor == Anchor.Start)
            {
                anchor = Anchor.End;
            }
            else if (anchor == Anchor.End)
            {
                anchor = Anchor.Start;
            }

            return anchor;
        }

        public void InvalidateRender()
        {
        }

        public void HandleLayoutChange()
        {
        }

        internal void InitStripline()
        {
            seriesClipRect = Owner.AxisContainer.AxisLayout.SeriesClipRect;
            axis = Stripline.Parent.Axis;
            axisRenderer = axis.Renderer;
            CalculateRenderingOptions();
        }

        internal void CalculateRenderingOptions()
        {
            RendererShouldRender = true;
            ClearPerviousPathOptions();
            GetSetStriplineOptions();
        }

        private void ClearPerviousPathOptions()
        {
            striplineRect.Clear();
            striplineText.Clear();
        }

        private void GetSetStriplineOptions(bool isUpdateDirection = false)
        {
            if (Stripline.Visible)
            {
                ChartAxisRenderer segmentAxisRenderer = null;
                if (Stripline.IsSegmented && Stripline.SegmentStart != null && Stripline.SegmentEnd != null && Stripline.SizeType != SizeType.Pixel)
                {
                    segmentAxisRenderer = GetSegmentAxis();
                }

                if (Stripline.IsRepeat && Stripline.RepeatEvery != null && Stripline.Size != 0 && Stripline.SizeType != SizeType.Pixel)
                {
                    double limit = (Stripline.RepeatUntil != null) ? ((axis.ValueType == ValueType.DateTime) ? ChartHelper.GetTime((DateTime)Stripline.RepeatUntil) : Convert.ToDouble(Stripline.RepeatUntil, null)) : axisRenderer.ActualRange.End;
                    double startValue = (Stripline.Start != null && axis.ValueType == ValueType.DateTime) ? Convert.ToDouble(ChartHelper.GetTime((DateTime)Stripline.Start)) : Convert.ToDouble(Stripline.Start, null);
                    if ((Stripline.StartFromAxis && axis.ValueType == ValueType.DateTime && Stripline.SizeType == SizeType.Auto) || (startValue < axisRenderer.VisibleRange.Start))
                    {
                        startValue = axisRenderer.VisibleLabels[0].Value == axisRenderer.VisibleRange.Start ? axisRenderer.VisibleRange.Start : axisRenderer.VisibleLabels[0].Value - (axis.ValueType == ValueType.DateTime ? axisRenderer.DateTimeInterval : axisRenderer.VisibleInterval);
                    }

                    startValue = Stripline.StartFromAxis && axis.ValueType != ValueType.DateTime ? axisRenderer.VisibleRange.Start : startValue;
                    while (startValue < limit)
                    {
                        if ((startValue >= axisRenderer.VisibleRange.Start && startValue < axisRenderer.VisibleRange.End) || ChartHelper.WithIn(startValue + (axis.ValueType == ValueType.DateTime ? axisRenderer.DateTimeInterval * +Stripline.Size : Stripline.Size), axisRenderer.VisibleRange))
                        {
                            CalculateStriplineDrawOptions(segmentAxisRenderer, startValue, isUpdateDirection);
                        }

                        startValue = GetStartValue(startValue);
                    }
                }
                else
                {
                    CalculateStriplineDrawOptions(segmentAxisRenderer, double.NaN, isUpdateDirection);
                }

                if (!string.IsNullOrEmpty(Stripline.Text))
                {
                    CalculateStriplineTextOption(isUpdateDirection);
                }
            }
        }

        internal void UpdateDirection()
        {
            RendererShouldRender = true;
            GetSetStriplineOptions(true);
        }

        internal void UpdateCustomization(string property)
        {
            RendererShouldRender = true;
            bool isLineOption = Stripline.SizeType == SizeType.Pixel;
            switch (property)
            {
                case "Color":
                    if (isLineOption)
                    {
                        striplinePath.Stroke = Stripline.Color;
                    }
                    else
                    {
                        striplineRect.ForEach(rect => rect.Fill = Stripline.Color);
                    }

                    break;
                case "DashArray":
                    striplinePath.StrokeDashArray = Stripline.DashArray;
                    break;
                case "Text":
                    striplineText.ForEach(element => element.Text = Stripline.Text);
                    break;
            }
        }

        private void CalculateStriplineDrawOptions(ChartAxisRenderer segmentAxisRenderer, double startValue, bool isUpdateDirection = false)
        {
            rect = MeasureStripline(segmentAxisRenderer, startValue);
            string id = Owner.ID + "_stripline_" + Stripline.ZIndex + "_";
            if (Stripline.SizeType == SizeType.Pixel)
            {
                id += "path_" + axis.Name + '_' + Index;
                string direction = axisRenderer.Orientation == Orientation.Vertical ? ("M" + rect.X.ToString(culture) + SPACE + rect.Y.ToString(culture) + SPACE + "L" + (rect.X + rect.Width).ToString(culture) + SPACE + rect.Y.ToString(culture)) : ("M" + rect.X.ToString(culture) + SPACE + rect.Y.ToString(culture) + SPACE + "L" + rect.X.ToString(culture) + SPACE + (rect.Y + rect.Height).ToString(culture));
                direction = ChartHelper.AppendPathElements(Owner, direction, id);
                if (!isUpdateDirection)
                {
                    striplinePath = new PathOptions
                    {
                        Id = id,
                        Direction = direction,
                        StrokeDashArray = Stripline.DashArray,
                        StrokeWidth = Stripline.Size,
                        Stroke = Stripline.Color,
                        Opacity = Stripline.Opacity
                    };
                }
                else
                {
                    striplinePath.Direction = direction;
                }
            }
            else if (rect.Height != 0 && rect.Width != 0)
            {
                id += "rect_" + axis.Name + "_" + Index;
                if (!isUpdateDirection)
                {
                    Rect currentRect = ChartHelper.AppendRectElements(Owner, id, rect);
                    striplineRect.Add(new RectOptions(id, currentRect.X, currentRect.Y, currentRect.Width, currentRect.Height, Stripline.Border.Width, Stripline.Border.Color, Stripline.Color, 0, 0, Stripline.Opacity));
                }
                else
                {
                    RectOptions rectOption = striplineRect.Find(element => element.Id == id);
                    rectOption.X = rect.X;
                    rectOption.Y = rect.Y;
                    rectOption.Width = rect.Width;
                    rectOption.Height = rect.Height;
                }
            }
        }

        private void CalculateStriplineTextOption(bool isUpdateDirection = false)
        {
            string id = Owner.ID + "_stripline_" + Stripline.ZIndex + "_" + "text_" + axis.Name + "_" + Index;
            Size textSize = ChartHelper.MeasureText(Stripline.Text, Stripline.TextStyle.GetChartFontOptions());
            double textMid = 3 * (textSize.Height / 8);
            double ty = rect.Y + (rect.Height / 2) + textMid;
            double tx = rect.X + (rect.Width / 2);
            Anchor anchor;
            TextOptions textOption;
            if (axisRenderer.Orientation == Orientation.Horizontal)
            {
                tx = GetTextStart(tx + (textMid + Factor(Stripline.HorizontalAlignment)), rect.Width, Stripline.HorizontalAlignment);
                ty = GetTextStart(ty - textMid, rect.Height, Stripline.VerticalAlignment);
                anchor = InvertAlignment(Stripline.VerticalAlignment);
            }
            else
            {
                tx = GetTextStart(tx, rect.Width, Stripline.HorizontalAlignment);
                ty = GetTextStart(ty + (textMid * Factor(Stripline.VerticalAlignment)) - 5, rect.Height, Stripline.VerticalAlignment);
                anchor = Stripline.HorizontalAlignment;
            }

            if (isUpdateDirection)
            {
                textOption = striplineText.Find(element => element.Id == id);
                textOption.X = tx.ToString(culture);
                textOption.Y = ty.ToString(culture);
            }
            else
            {
                textOption = new TextOptions(tx.ToString(culture), ty.ToString(culture), Stripline.TextStyle.Color, Stripline.TextStyle.GetFontOptions(), Stripline.Text, anchor.ToString(), id, "rotate(" + (double.IsNaN(Stripline.Rotation) ? axisRenderer.Orientation == Orientation.Vertical ? 0 : -90 : Stripline.Rotation) + "," + tx.ToString(culture) + "," + ty.ToString(culture) + ")");
                string[] locations = ChartHelper.AppendTextElements(Owner, id, tx, ty);
                textOption.X = locations[0];
                textOption.Y = locations[1];
                striplineText.Add(textOption);
            }
        }

        private ChartAxisRenderer GetSegmentAxis()
        {
            List<ChartAxisRenderer> axes = Owner.AxisContainer.Renderers.Cast<ChartAxisRenderer>().ToList();
            if (Stripline.SegmentAxisName == null)
            {
                return axis.Renderer.Orientation == Orientation.Horizontal ? axes.ElementAt(1) : axes.First();
            }
            else
            {
                return axes.Where(axis => Stripline.SegmentAxisName == axis.Axis.Name).First();
            }
        }

        private Rect MeasureStripline(ChartAxisRenderer segmentAxis, double startValue = double.NaN)
        {
            double actualStart, actualEnd;
            if (Stripline.IsRepeat && Stripline.Size != 0)
            {
                actualStart = startValue;
                actualEnd = double.NaN;
            }
            else
            {
                if (axis.ValueType == ValueType.DateTimeCategory)
                {
                    actualStart = Stripline.Start.GetType().Equals(typeof(int)) ? (int)Stripline.Start : axisRenderer.Labels.IndexOf(ChartHelper.GetTime((DateTime)Stripline.Start).ToString(culture));
                    actualEnd = Stripline.End.GetType().Equals(typeof(int)) ? (int)Stripline.End : axisRenderer.Labels.IndexOf(ChartHelper.GetTime((DateTime)Stripline.End).ToString(culture));
                }
                else if (axis.ValueType == ValueType.DateTime)
                {
                    actualStart = Stripline.Start == null ? double.NaN : ChartHelper.GetTime((DateTime)Stripline.Start);
                    actualEnd = Stripline.End == null ? double.NaN : ChartHelper.GetTime((DateTime)Stripline.End);
                }
                else
                {
                    actualStart = Stripline.Start == null ? double.NaN : Convert.ToDouble(Stripline.Start, null);
                    actualEnd = Stripline.End == null ? double.NaN : Convert.ToDouble(Stripline.End, null);
                }
            }

            FromTo rect = GetFromToValue(actualStart, actualEnd, axisRenderer, Stripline.Size, Stripline.StartFromAxis);
            double height = axisRenderer.Orientation == Orientation.Vertical ? (rect.To - rect.From) * axisRenderer.Rect.Height : seriesClipRect.Height;
            double width = axisRenderer.Orientation == Orientation.Horizontal ? (rect.To - rect.From) * axisRenderer.Rect.Width : seriesClipRect.Width;
            double x = axisRenderer.Orientation == Orientation.Vertical ? seriesClipRect.X : (rect.From * axisRenderer.Rect.Width) + axisRenderer.Rect.X;
            double y = axisRenderer.Orientation == Orientation.Horizontal ? seriesClipRect.Y : (axisRenderer.Rect.Y + axisRenderer.Rect.Height - ((Stripline.SizeType == SizeType.Pixel ? rect.From : rect.To) * axisRenderer.Rect.Height));
            if (Stripline.IsSegmented && Stripline.SegmentStart != null && Stripline.SegmentEnd != null && Stripline.SizeType != SizeType.Pixel)
            {
                FromTo segRect = GetFromToValue(
                    segmentAxis.Axis.ValueType == ValueType.DateTime ? ChartHelper.GetTime((DateTime)Stripline.SegmentStart) : Convert.ToDouble(Stripline.SegmentStart, null),
                    segmentAxis.Axis.ValueType == ValueType.DateTime ? ChartHelper.GetTime((DateTime)Stripline.SegmentEnd) : Convert.ToDouble(Stripline.SegmentEnd, null),
                    segmentAxis);
                if (segmentAxis.Orientation == Orientation.Vertical)
                {
                    y = segmentAxis.Rect.Y + segmentAxis.Rect.Height - (segRect.To * segmentAxis.Rect.Height);
                    height = (segRect.To - segRect.From) * segmentAxis.Rect.Height;
                }
                else
                {
                    x = (segRect.From * segmentAxis.Rect.Width) + segmentAxis.Rect.X;
                    width = (segRect.To - segRect.From) * segmentAxis.Rect.Width;
                }
            }

            if ((height != 0 && width != 0) || (Stripline.SizeType == SizeType.Pixel && (Stripline.Start != null || Stripline.StartFromAxis)))
            {
                return new Rect(x, y, width, height);
            }

            return new Rect(0, 0, 0, 0);
        }

        private FromTo GetFromToValue(double start, double end, ChartAxisRenderer axis, double size = double.NaN, bool startFromAxis = false)
        {
            FromTo result = new FromTo();
            double from = (startFromAxis && !Stripline.IsRepeat) ? axis.VisibleRange.Start : start;
            double to = GetToValue(Math.Max(!double.IsNaN(start) ? start : double.NegativeInfinity, double.IsNaN(end) ? start : end), from, size, end);
            from = FindValue(from, axis.VisibleRange.Start, axis.VisibleRange.End);
            to = FindValue(to, axis.VisibleRange.Start, axis.VisibleRange.End);
            result.From = ChartHelper.ValueToCoefficient(axis.Axis.IsInversed ? to : from, axis);
            result.To = ChartHelper.ValueToCoefficient(axis.Axis.IsInversed ? from : to, axis);
            return result;
        }

        private double GetToValue(double to, double from, double size, double end)
        {
            SizeType sizeType = Stripline.SizeType;
            if (axis.ValueType == ValueType.DateTime)
            {
                DateTime fromValue = new DateTime(1970, 1, 1).AddMilliseconds(from);
                if (sizeType == SizeType.Auto)
                {
                    sizeType = (SizeType)Enum.Parse(typeof(SizeType), axisRenderer.ActualIntervalType.ToString());
                    size *= axisRenderer.VisibleInterval;
                }

                switch (sizeType)
                {
                    case SizeType.Years:
                        return double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddYears((int)size)) : to;
                    case SizeType.Months:
                        return double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddMonths((int)size)) : to;
                    case SizeType.Days:
                        return double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddDays((int)size)) : to;
                    case SizeType.Hours:
                        return double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddHours((int)size)) : to;
                    case SizeType.Minutes:
                        return double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddMinutes((int)size)) : to;
                    case SizeType.Seconds:
                        return double.IsNaN(end) ? ChartHelper.GetTime(fromValue.AddSeconds((int)size)) : to;
                    default:
                        return from;
                }
            }
            else
            {
                return Stripline.SizeType == SizeType.Pixel ? from : (double.IsNaN(end) ? from + size : to);
            }
        }

        private double GetStartValue(double startValue)
        {
            if (axis.ValueType == ValueType.DateTime)
            {
                return GetToValue(double.NaN, startValue, Convert.ToDouble(Stripline.RepeatEvery, null), double.NaN);
            }
            else
            {
                return startValue + Convert.ToDouble(Stripline.RepeatEvery, null);
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (rect != null && builder != null)
            {
                if (striplinePath != null)
                {
                    Owner.SvgRenderer.RenderPath(builder, striplinePath.Id, striplinePath.Direction, striplinePath.StrokeDashArray, striplinePath.StrokeWidth, striplinePath.Stroke, striplinePath.Opacity);
                }
                else
                {
                    foreach (RectOptions rect in striplineRect)
                    {
                        Owner.SvgRenderer.RenderRect(builder, rect);
                    }
                }

                foreach (TextOptions text in striplineText)
                {
                    Owner.SvgRenderer.RenderText(builder, text);
                }

                RendererShouldRender = false;
            }
        }
    }

    public class ChartStriplineBehindRenderer : ChartStriplineRenderer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.StriplineBehindContainer.AddRenderer(this);
            Stripline.Renderer = this;
        }
    }

    public class ChartStriplineOverRenderer : ChartStriplineRenderer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.StriplineOverContainer.AddRenderer(this);
            Stripline.Renderer = this;
        }
    }
}

using System.Collections.Generic;
using Syncfusion.Blazor.Data;
using System;
using Syncfusion.Blazor.Sparkline.Internal;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSparkline<TValue>
    {
        private const string SPACE = " ";

        private void DrawLine(RenderTreeBuilder builder, List<SparklineValues> points, SeriesRenderingEventArgs args)
        {
            Rendering.OpenGroupElement(builder, ID + "_Sparkline_G", string.Empty, ClipPath);
            string direction = string.Empty;
            for (int i = 0, len = points.Count; i < len; i++)
            {
                if (i == 0)
                {
                    direction = "M " + points[0].X.ToString(culture) + SPACE + points[i].Y.ToString(culture) + SPACE;
                }

                direction += "L " + points[i].X.ToString(culture) + SPACE + points[i].Y.ToString(culture) + SPACE;
            }

            Rendering.RenderPath(builder, ID + "_Sparkline_Line", direction, "0", args.LineWidth, Fill.Equals("#00bdae", comparison) && Theme == Theme.Bootstrap4 ? ThemeStyle.AxisLine : Fill, Opacity, "transparent");
            builder.CloseElement();
        }

        private void DrawArea(RenderTreeBuilder builder, List<SparklineValues> points, SeriesRenderingEventArgs args)
        {
            Rendering.OpenGroupElement(builder, ID + "_Sparkline_G", string.Empty, ClipPath);
            string direction = string.Empty, path = string.Empty;
            for (int i = 0, len = points.Count; i < len; i++)
            {
                if (i != 0)
                {
                    path += "L " + points[i].X.ToString(culture) + SPACE + points[i].Y.ToString(culture) + SPACE;
                }
                else
                {
                    direction = "M " + points[i].X.ToString(culture) + SPACE + AxisHeight.ToString(culture) + SPACE;
                    path = "M " + points[i].X.ToString(culture) + SPACE + points[i].Y.ToString(culture) + SPACE;
                }

                direction += "L " + points[i].X.ToString(culture) + SPACE + points[i].Y.ToString(culture) + SPACE;
                if (i == (len - 1))
                {
                    direction += "L " + points[i].X.ToString(culture) + SPACE + AxisHeight.ToString(culture) + " Z";
                }
            }

            Rendering.RenderPath(builder, ID + "_Sparkline_Area", direction, "0", 0, "transparent", Opacity, args.Fill);
            Rendering.RenderPath(builder, ID + "_Sparkline_Area_Str", path, "0", args.Border.Width, args.Border.Color, Opacity, "transparent");
            builder.CloseElement();
        }

        private void DrawColumn(RenderTreeBuilder builder, List<SparklineValues> points, SeriesRenderingEventArgs args)
        {
            List<SparklineValues> locations = points;
            Rendering.OpenGroupElement(builder, ID + "_Sparkline_G", string.Empty, ClipPath);
            double lowPos = double.NaN, highPos = double.NaN;
            if (!string.IsNullOrEmpty(HighPointColor) || !string.IsNullOrEmpty(LowPointColor))
            {
                double[] pointsYPos = locations.Select(a => a.MarkerPosition).ToArray();
                highPos = pointsYPos.Min();
                lowPos = pointsYPos.Max();
            }

            RectOptions rect = new RectOptions(string.Empty, 0, 0, 0, 0, args.Border.Width, args.Border.Color, string.Empty, 0, 0, Opacity);
            int paletteLength = Palette != null ? Palette.Length : 0, len = points.Count;
            for (int i = 0; i < len; i++)
            {
                SparklineValues point = points[i];
                rect.Id = ID + "_Sparkline_Column_" + i.ToString(culture);
                rect.Fill = (paletteLength > 0) ? palette[i % paletteLength] : args.Fill;
                rect.X = point.X;
                rect.Y = point.Y;
                rect.Height = point.Height;
                rect.Width = point.Width;
                GetSpecialPoint(true, point, rect, i, highPos, lowPos, len);
                point.Location.Y = (point.MarkerPosition <= AxisHeight) ? point.Y : (point.Y + point.Height);
                point.Location.X = point.X + (point.Width / 2);
                rect.Stroke = !string.IsNullOrEmpty(args.Border.Color) ? args.Border.Color : rect.Fill;
                rect.StrokeWidth = args.Border.Width;
                SparklinePointEventArgs pointArgs = new SparklinePointEventArgs()
                {
                    PointIndex = i,
                    Border = new Border() { Color = rect.Stroke, Width = args.Border.Width },
                    Fill = rect.Fill
                };
                Events?.OnPointRendering?.Invoke(pointArgs);
                rect.Fill = pointArgs.Fill;
                rect.Stroke = pointArgs.Border.Color;
                rect.StrokeWidth = pointArgs.Border.Width;
                if (!pointArgs.Cancel)
                {
                    DrawRectangle(builder, rect, points[i].XName.ToString() + " : " + points[i].YVal.ToString(culture));
                }
            }

            builder.CloseElement();
        }

        private void DrawPie(RenderTreeBuilder builder, List<SparklineValues> points, SeriesRenderingEventArgs args)
        {
            double pieHeight = AvailableSize.Height - (Padding.Top + Padding.Bottom),
            pieWidth = AvailableSize.Width - (Padding.Left + Padding.Right),
            area = pieHeight <= pieWidth ? pieHeight / 2 : pieWidth / 2,
            x = AvailableSize.Width / 2,
            y = AvailableSize.Height / 2,
            degree = 0, startRadius, endRadius, low = 0, high = 0, startX, startY, endX, endY, diffInRadius, startDegree = 90, endDegree;
            string stroke = args.Border.Color;
            double strokeWidth = args.Border.Width;
            string flag = string.Empty;
            string[] colors = Palette != null && Palette.Length > 0 ? Palette : new string[] { "#00bdae", "#404041", "#357cd2", "#e56590", "#f8b883", "#70ad47", "#dd8abd", "#7f84e8", "#7bb4eb", "#ea7a57" };
            if (!string.IsNullOrEmpty(HighPointColor) || !string.IsNullOrEmpty(LowPointColor))
            {
                double[] ypointvalues = points.Select(a => a.YVal).ToArray();
                low = ypointvalues.Min();
                high = ypointvalues.Max();
            }

            Rendering.OpenGroupElement(builder, ID + "_Sparkline_G");
            string pathArc, pieFill;
            for (int i = 0; i < points.Count; i++)
            {
                flag = string.Empty;
                startDegree += degree;
                degree = points[i].Degree;
                endDegree = startDegree + degree;
                startRadius = (startDegree - 90) * Math.PI / 180.0;
                endRadius = (endDegree - 90) * Math.PI / 180.0;
                points[i].StartAngle = startRadius;
                points[i].EndAngle = endRadius;
                flag = (degree < 180) ? "0" : "1";
                startX = x + (area * Math.Cos(startRadius));
                startY = y + (area * Math.Sin(startRadius));
                endX = x + (area * Math.Cos(endRadius));
                endY = y + (area * Math.Sin(endRadius));
                pathArc = "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + endX.ToString(culture) + SPACE + endY.ToString(culture) + " A " + area.ToString(culture) + SPACE + area.ToString(culture) + " 0 " + flag + ",0 " + startX.ToString(culture) + SPACE + startY.ToString(culture) + " Z";
                pieFill = GetPieSpecialPoint(points[i], i, high, low, points.Count, colors);
                SparklinePointEventArgs pointArgs = new SparklinePointEventArgs()
                {
                    PointIndex = i,
                    Border = new Border() { Color = stroke, Width = strokeWidth },
                    Fill = pieFill
                };
                Events?.OnPointRendering?.Invoke(pointArgs);
                pieFill = pointArgs.Fill;
                stroke = pointArgs.Border.Color;
                strokeWidth = pointArgs.Border.Width;
                if (!pointArgs.Cancel)
                {
                    Rendering.RenderPath(builder, ID + "_Sparkline_Pie_" + i, pathArc, "0", strokeWidth, stroke, Opacity, pieFill, points[i].XName.ToString() + " : " + points[i].YVal.ToString(culture));
                }

                diffInRadius = endRadius - startRadius;
                points[i].Location.X = x + ((area / 2) * Math.Cos(startRadius + (diffInRadius / 2)));
                points[i].Location.Y = y + ((area / 2) * Math.Sin(startRadius + (diffInRadius / 2)));
            }

            builder.CloseElement();
        }

        private void DrawWinLoss(RenderTreeBuilder builder, List<SparklineValues> points, SeriesRenderingEventArgs args)
        {
            Rendering.OpenGroupElement(builder, ID + "_Sparkline_G", string.Empty, ClipPath);
            RectOptions options = new RectOptions(ID + "_Sparkline_Winloss_", 0, 0, 0, 0, args.Border.Width, args.Border.Color, string.Empty, 0, 0, Opacity);
            int paletteLength = Palette != null ? Palette.Length : 0;
            for (int i = 0; i < points.Count; i++)
            {
                SparklineValues point = points[i];
                options.Id = ID + "_Sparkline_Winloss_" + i;
                options.Fill = (paletteLength > 0) ? Palette[i % paletteLength] : (point.YVal == AxisValue) ?
                    (!string.IsNullOrEmpty(TiePointColor) ? TiePointColor : "#a216f3") : (point.YVal > AxisValue) ? args.Fill :
                    (!string.IsNullOrEmpty(NegativePointColor) ? NegativePointColor : "#e20f07");
                options.Stroke = !string.IsNullOrEmpty(args.Border.Color) ? args.Border.Color : options.Fill;
                options.X = point.X;
                options.Y = point.Y;
                options.Width = point.Width;
                options.Height = point.Height;
                point.Location.X = point.X + (point.Width / 2);
                point.Location.Y = point.YVal >= AxisValue ? point.Y : (point.Y + point.Height);
                SparklinePointEventArgs pointArgs = new SparklinePointEventArgs()
                {
                    PointIndex = i,
                    Border = new Border() { Color = options.Stroke, Width = args.Border.Width },
                    Fill = options.Fill
                };
                Events?.OnPointRendering?.Invoke(pointArgs);
                options.Fill = pointArgs.Fill;
                options.Stroke = pointArgs.Border.Color;
                options.StrokeWidth = pointArgs.Border.Width;
                if (!pointArgs.Cancel)
                {
                    DrawRectangle(builder, options, points[i].XName.ToString() + " : " + points[i].YVal.ToString(culture));
                }
            }

            builder.CloseElement();
        }
    }
}
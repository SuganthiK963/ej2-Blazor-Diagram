using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DataVizCommon
{
    public class FontOptions
    {
        public string Color { get; set; }
        public string Size { get; set; }
        public string FontFamily { get; set; }
        public string FontWeight { get; set; }
        public string FontStyle { get; set; }
    }

    public class PatternOptions
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Id { get; set; }
        public string PatternUnits { get; set; }
#pragma warning disable CA2227
        public List<object> ShapeOptions { get; set; }
    }

    public class TextOptions
    {
        public List<string> TextCollection { get; set; } = new List<string>();
        public List<TextLocation> TextLocationCollection { get; set; } = new List<TextLocation>();
#pragma warning restore CA2227 
        public string X { get; set; }
        public string Y { get; set; }
        public string Id { get; set; }
        public string Fill { get; set; }
        public string FontSize { get; set; }
        public string FontStyle { get; set; }
        public string FontFamily { get; set; }
        public string FontWeight { get; set; }
        public string TextAnchor { get; set; }
        public object Font { get; set; }
        public string Text { get; set; }
        public string Transform { get; set; }
        public string LabelRotation { get; set; }
        public string DominantBaseline { get; set; }
        public RenderFragment ChildContent { get; set; }
        public string AccessibilityText { get; set; } = string.Empty;
        public string TabIndex { get; set; } = string.Empty;
        public string Style { get; set; }
        internal bool IsMinus { get; set; }
        internal bool IsRotatedLabelIntersect { get; set; }
        public TextOptions(string x, string y, string fill, FontOptions font, string text, string anchor, string id, string transform = "", string labelRotation = "0", string dominantBaseline = "undefined", string accessibilityText = "", string tabIndex = "", string style = "")
        {
            X = x;
            Y = y;
#pragma warning disable CA1062
            Fill = fill ?? font.Color;
#pragma warning restore CA1062
            FontSize = font.Size;
            FontFamily = font.FontFamily;
            FontWeight = font.FontWeight;
            FontStyle = font.FontStyle;
            TextAnchor = anchor;
            Text = text;
            Id = id;
            Transform = transform;
            LabelRotation = labelRotation;
            DominantBaseline = dominantBaseline;
            AccessibilityText = accessibilityText;
            TabIndex = tabIndex;
            Style = style;
            Font = font;
        }
        public TextOptions() { }
    }

    public class RectOptions
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double StrokeWidth { get; set; }
        public string Fill { get; set; }
        public string Id { get; set; }
        public string DashArray { get; set; }
        public string Transform { get; set; }
        public string Stroke { get; set; }
        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Opacity { get; set; }
        public string Style { get; set; }
        public string Filter { get; set; }
        public string Visibility { get; set; } = string.Empty;
        public RectOptions(string id, double x, double y, double width, double height, double strokeWidth, string stroke, string fill, double rx = 0, double ry = 0, double opacity = 1, string visibility = "", string style = "", string filter = "")
        {
            Id = id;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Fill = fill;
            Rx = rx;
            Ry = ry;
            Opacity = opacity;
            Visibility = visibility;
            Style = style;
            Filter = filter;
        }
        public RectOptions() { }
    }

    public class PathOptions
    {
        public string Id { get; set; }
        public string Direction { get; set; }
        public string StrokeDashArray { get; set; }
        public string Stroke { get; set; }
        public double StrokeWidth { get; set; } = 1;
        public double Opacity { get; set; }
        public string Fill { get; set; }
        public string StrokeMiterLimit { get; set; } = string.Empty;
        public string ClipPath { get; set; } = string.Empty;
        public string AccessibilityText { get; set; } = string.Empty;
        public string Visibility { get; set; } = string.Empty;
        public PathOptions(string id, string direction, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "none", string strokeMiterLimit = "", string clipPath = "", string accessText = "")
        {
            Id = id;
            Direction = direction;
            StrokeDashArray = strokeDasharray;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Opacity = opacity;
            Fill = fill;
            StrokeMiterLimit = strokeMiterLimit;
            ClipPath = clipPath;
            AccessibilityText = accessText;
        }
        public PathOptions() { }
    }

    public class EllipseOptions
    {
        public string Id { get; set; }
        public string Rx { get; set; }
        public string Ry { get; set; }
        public string Cx { get; set; }
        public string Cy { get; set; }
        public string StrokeDashArray { get; set; }
        public string Stroke { get; set; }
        public double StrokeWidth { get; set; } = 1;
        public double Opacity { get; set; }
        public string Fill { get; set; }
        public string Visibility { get; set; } = string.Empty;
        public EllipseOptions(string id, string rx, string ry, string cx, string cy, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "none")
        {
            Id = id;
            Rx = rx;
            Ry = ry;
            Cx = cx;
            Cy = cy;
            StrokeDashArray = strokeDasharray;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Opacity = opacity;
            Fill = fill;
        }
        public EllipseOptions() { }
    }

    public class CircleOptions
    {
        public string Id { get; set; }
        public string Cx { get; set; }
        public string Cy { get; set; }
        public string R { get; set; }
        public string StrokeDashArray { get; set; }
        public string Stroke { get; set; }
        public double StrokeWidth { get; set; } = 1;
        public double Opacity { get; set; } = 1;
        public string Fill { get; set; } = "none";
        public string Visibility { get; set; } = string.Empty;
        public string AccessibilityText { get; set; } = string.Empty;
        public CircleOptions(string id, string cx, string cy, string r, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "none", string visibility = "", string accessText = "")
        {
            Id = id;
            Cx = cx;
            Cy = cy;
            R = r;
            StrokeDashArray = strokeDasharray;
            StrokeWidth = strokeWidth;
            Stroke = stroke;
            Opacity = opacity;
            Fill = fill;
            Visibility = visibility;
            AccessibilityText = accessText;
        }
        public CircleOptions() { }
    }

    public class ImageOptions
    {
        public string Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Href { get; set; }
        public string Visibility { get; set; }
        public string PreserveAspectRatio { get; set; } = "none";
        public ImageOptions(string id, double x, double y, double width, double height, string url, string visibility = "", string preserveAspectRatio = "none")
        {
            Id = id;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Href = url;
            Visibility = visibility;
            PreserveAspectRatio = preserveAspectRatio;
        }
        public ImageOptions() { }
    }

    public class SymbolOptions
    {
        public PathOptions PathOption { get; set; } = new PathOptions();
        public EllipseOptions EllipseOption { get; set; } = new EllipseOptions();
        public ImageOptions ImageOption { get; set; } = new ImageOptions();
        public ShapeName ShapeName { get; set; }
    }

    public class Rect
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Rect() { }
        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public class TextLocation
    {
        public string Text { get; set; }
        public double Y { get; set; }
        public TextLocation(string text, double y)
        {
            Text = text;
            Y = y;
        }
    }

    /// <summary>
    /// Specifies the marker ShapeOptions.
    /// </summary>
    public enum ShapeName
    {
        /// <summary>
        /// Defines the path shape. 
        /// </summary>
        path,
        /// <summary>
        /// Defines the ellipse shape. 
        /// </summary>
        ellipse,
        /// <summary>
        /// Defines the image shape. 
        /// </summary>
        image
    }

    public class LocationModel : OffsetModel { }

    public class OffsetModel
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
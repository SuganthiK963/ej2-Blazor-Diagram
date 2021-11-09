using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class StyleAttributes
    {
        internal string Fill { get; set; }
        internal string Stroke { get; set; }
        internal double StrokeWidth { get; set; }
        internal string DashArray { get; set; }
        internal double Opacity { get; set; }
        internal Shadow Shadow { get; set; }
        internal string Gradient { get; set; } = string.Empty;
        internal string ClassValues { get; set; }
        internal StyleAttributes() { }
        protected StyleAttributes(ICommonElement element)
        {
            ShapeStyle style = element.Style;
            Fill = style.Fill;
            Stroke = style.StrokeColor;
            DashArray = (!string.IsNullOrEmpty(style.StrokeDashArray)) ? DiagramRenderer.ParseDashArray(style.StrokeDashArray).ToString(CultureInfo.InvariantCulture) : "none";
            Opacity = style.Opacity;
            StrokeWidth = style.StrokeWidth;
            Shadow = element.Shadow;
            if (element.IsExport)
            {
                StrokeWidth *= element.ExportScaleValue.X;
            }
            if (style.Gradient != null)
            {
                Gradient = element.ID + ((style.Gradient.BrushType == GradientType.Linear) ? "_linear" : "_radial");
            }
        }

        internal virtual void Dispose()
        {
            Fill = null;
            Stroke = null;
            DashArray = null;
            Gradient = null;
            ClassValues = null;
            if (Shadow != null)
            {
                Shadow.Dispose();
            }
        }
    }
    internal class BaseAttributes : StyleAttributes
    {
        internal string ID { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("x")]
        public double X { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("y")]
        public double Y { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("width")]
        public double Width { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("height")]
        public double Height { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("angle")]
        public double Angle { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("pivotX")]
        public double PivotX { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("pivotY")]
        public double PivotY { get; set; }
        internal bool Visible { get; set; }
        internal string Description { get; set; }
        internal bool CanApplyStyle { get; set; }
        internal FlipDirection Flip { get; set; }

        internal BaseAttributes()
        {
            ID = BaseUtil.RandomId();
        }
        public BaseAttributes(ICommonElement element, TransformFactor transform = null, bool? isPreviewNode = false) : base(element)
        {
            DiagramSize actualSize = element.ActualSize;
            Width = BaseUtil.GetDoubleValue(actualSize.Width);
            Height = BaseUtil.GetDoubleValue(actualSize.Height);
            X = element.OffsetX - Width * element.Pivot.X + 0.5;
            Y = element.OffsetY - Height * element.Pivot.Y + 0.5;
            Angle = element.RotationAngle + element.ParentTransform;
            PivotX = element.Pivot.X;
            PivotY = element.Pivot.Y;
            Visible = element.Visible;
            ID = element.ID;
            Description = element.Description;
            CanApplyStyle = element.CanApplyStyle;
            if (element.Flip != FlipDirection.None)
            {
                Flip = element.Flip;
            }
            if (isPreviewNode != null && isPreviewNode.Value)
            {
                X -= 0.5;
                Y -= 0.5;
            }
            if (element.IsExport)
            {
                Width *= element.ExportScaleValue.X;
                Height *= element.ExportScaleValue.Y;
                X *= element.ExportScaleValue.X;
                Y *= element.ExportScaleValue.Y;
            }
            if (transform != null)
            {
                X += transform.TX;
                Y += transform.TY;
            }
        }

        internal override void Dispose()
        {
            base.Dispose();
            ID = null;
            Description = null;
        }
    }


    internal class Alignment
    {
        internal string VAlign { get; set; }
        internal string HAlign { get; set; }
    }


    internal class SegmentInfo
    {
        internal DiagramPoint Point { get; set; }
        internal double Index { get; set; }
        internal double Angle { get; set; }
    }


    internal class RectAttributes : BaseAttributes
    {
        internal double CornerRadius { get; set; }
        internal RectAttributes() { }
        public RectAttributes(ICommonElement element, TransformFactor transform, bool? isPreviewNode = false) : base(element, transform, isPreviewNode)
        {
            CornerRadius = element.CornerRadius;
        }
        internal override void Dispose()
        {
            base.Dispose();
        }
    }
    internal class LineAttributes : BaseAttributes
    {
        public LineAttributes(DiagramElement element, TransformFactor transform, bool? isPreviewNode = false) : base(element, transform, isPreviewNode)
        {
        }
        public LineAttributes() { }
        [System.Text.Json.Serialization.JsonPropertyName("startPoint")]
        public DiagramPoint StartPoint { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("endPoint")]
        public DiagramPoint EndPoint { get; set; }

        internal override void Dispose()
        {
            base.Dispose();
            if (StartPoint != null)
            {
                StartPoint.Dispose();
                StartPoint = null;
            }
            if (EndPoint != null)
            {
                EndPoint.Dispose();
                EndPoint = null;
            }
        }

    }

    internal class CircleAttributes : BaseAttributes
    {
        public CircleAttributes() { }
        public CircleAttributes(DiagramElement element, TransformFactor transform, bool? isPreviewNode = false) : base(element, transform, isPreviewNode)
        {

        }
        [System.Text.Json.Serialization.JsonPropertyName("cx")]
        public double CenterX { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("cy")]
        public double CenterY { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("r")]
        public double Radius { get; set; }
        internal double Id { get; set; }

        internal override void Dispose()
        {
            base.Dispose();
        }
    }

    internal class PathAttributes : BaseAttributes
    {
        [System.Text.Json.Serialization.JsonPropertyName("data")]
        internal string Data { get; set; }
        internal PathAttributes() { }
        public PathAttributes(PathElement element, TransformFactor transform, bool? isPreviewNode = false) : base(element, transform, isPreviewNode)
        {
            Data = element.AbsolutePath;
        }

        internal override void Dispose()
        {
            Data = null;
            base.Dispose();
        }
    }

    internal class PathSegment
    {
        internal char? Command { get; set; }
        internal double? Angle { get; set; }
        internal bool? LargeArc { get; set; }
        internal double? X2 { get; set; }
        internal bool? Sweep { get; set; }
        internal double? X1 { get; set; }
        internal double? Y1 { get; set; }
        internal double? Y2 { get; set; }
        internal double? X0 { get; set; }
        internal double? Y0 { get; set; }
        internal double? X { get; set; }
        internal double? Y { get; set; }
        internal double? R1 { get; set; }
        internal double? R2 { get; set; }
        internal DiagramPoint Centp { get; set; }
        internal double? XAxisRotation { get; set; }
        internal double? Rx { get; set; }
        internal double? Ry { get; set; }
        internal double? A1 { get; set; }
        internal double? Ad { get; set; }
    }
    /// <summary> 
    /// RenderingParameters class is used to pass the parameter collection in render fragment
    /// </summary>
    internal class FragmentParameter
    {
        internal ICommonElement Element { get; set; }
        internal ICommonElement GroupElement { get; set; }
        internal ObservableCollection<Node> Nodes { get; set; }
        internal bool IsCreateGElement { get; set; }
        internal bool IsGroup { get; set; }
    }

    internal class SelectionFragmentParameter
    {
        internal DiagramSelectionSettings Selector { get; set; }
        internal TransformFactor Transform { get; set; }
    }

    internal class SnapSettingsFragmentParameter
    {
        internal SnapSettings SnapSettingsValue { get; set; }
        internal double CurrentZoom { get; set; }
    }

    /// <summary>
    /// Represents the properties of a sub text element.
    /// </summary>
    internal class SubTextElement
    {
        /// <summary>
        /// Returns the text from the sub text element.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("text")]
        public string Text { get; set; }
        /// <summary>
        /// Returns the start position where the text element is to be rendered.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("x")]
        public double X { get; set; }
        /// <summary>
        /// Returns the left position where the text is to be rendered.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("dy")]
        public double Dy { get; set; }
        /// <summary>
        /// Returns the width of the sub text element. 
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("width")]
        public double Width { get; set; }

        internal void Dispose()
        {
            Text = null;
        }
    }

    /// <summary>
    /// Represents the properties of text bounds.
    /// </summary>
    internal class TextBounds
    {
        /// <summary>
        /// Returns the width of the sub text element. 
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("x")]
        public double X { get; set; }
        /// <summary>
        /// Returns the start position where the text element is to be rendered.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("width")]
        public double Width { get; set; }
    }
    internal class TextAttributes : RectAttributes
    {
        internal string ParentID { get; set; }
        internal string WhiteSpace { get; set; }
        internal string Content { get; set; }
        internal string BreakWord { get; set; }
        internal double FontSize { get; set; }
        internal TextWrap TextWrapping { get; set; }
        internal string FontFamily { get; set; }
        internal bool Bold { get; set; }
        internal bool Italic { get; set; }
        internal string TextAlign { get; set; }
        internal string Color { get; set; }
        internal TextOverflow TextOverflow { get; set; }
        internal string TextDecoration { get; set; }
        internal TextBounds WrapBounds { get; set; }
        internal ObservableCollection<SubTextElement> ChildNodes { get; set; }
        internal bool IsHorizontalLane { get; set; }
        internal double ParentOffsetX { get; set; }
        internal double ParentOffsetY { get; set; }
        internal double ParentWidth { get; set; }
        internal double ParentHeight { get; set; }

        public TextAttributes(TextElement element, TransformFactor transform, bool? isPreviewNode = false) : base(element, transform, isPreviewNode)
        {
            CornerRadius = 0;
            if (element.Style is TextStyle style)
            {
                WhiteSpace = BaseUtil.WhiteSpaceToString(style.WhiteSpace, style.TextWrapping);
                Content = element.Content;
                BreakWord = BaseUtil.WordBreakToString(style.TextWrapping.ToString());
                TextAlign = BaseUtil.TextAlignToString(style.TextAlign);
                Color = style.Color;
                Italic = style.Italic;
                Bold = style.Bold;
                FontSize = style.FontSize;
                FontFamily = style.FontFamily;
                TextOverflow = style.TextOverflow;
                TextWrapping = style.TextWrapping;
                TextDecoration = style.TextDecoration.ToString() == "LineThrough"
                    ? "line-through"
                    : style.TextDecoration.ToString();
                WrapBounds = element.WrapBounds;
                ChildNodes = element.ChildNodes;
                Visible = element.Visible;
                DashArray = string.Empty;
                StrokeWidth = 0;
                Fill = style.Fill;
            }

            //Object ariaLabel = element.Description != null ? element.Description : element.Content != null ? element.Content : element.ID;
        }
    }
    internal class ImageAttributes : RectAttributes
    {
        internal string Source { get; set; }
        internal double SourceWidth { get; set; }
        internal double SourceHeight { get; set; }
        internal Scale Scale { get; set; }
        internal ImageAlignment Alignment { get; set; }

        public ImageAttributes(ImageElement element, TransformFactor transform, bool? isPreviewNode = false) : base(element, transform, isPreviewNode)
        {
            double imageWidth = 0, imageHeight = 0;
            double? sourceWid = null, sourceHei = null;
            if (element.Stretch == Stretch.Stretch)
            {
                imageWidth = BaseUtil.GetDoubleValue(element.ActualSize.Width);
                imageHeight = BaseUtil.GetDoubleValue(element.ActualSize.Height);
            }
            else
            {
                double contentWidth = BaseUtil.GetDoubleValue(element.ImageSize.Width);
                double contentHeight = BaseUtil.GetDoubleValue(element.ImageSize.Height);
                double widthRatio = BaseUtil.GetDoubleValue(element.Width / contentWidth);
                double heightRatio = BaseUtil.GetDoubleValue(element.Height / contentHeight);
                double ratio;
                switch (element.Stretch)
                {
                    case Stretch.Meet:
                        ratio = Math.Min(Convert.ToInt32(widthRatio), Convert.ToInt32(heightRatio));
                        imageWidth = contentWidth * ratio;
                        imageHeight = contentHeight * ratio;
                        element.OffsetX += Math.Abs(BaseUtil.GetDoubleValue(element.Width) - imageWidth) / 2;
                        element.OffsetY += Math.Abs(BaseUtil.GetDoubleValue(element.Height) - imageHeight) / 2;
                        break;
                    case Stretch.Slice:
                        widthRatio = BaseUtil.GetDoubleValue(element.Width / contentWidth);
                        heightRatio = BaseUtil.GetDoubleValue(element.Height / contentHeight);
                        ratio = Math.Max(widthRatio, heightRatio);
                        imageWidth = contentWidth * ratio;
                        imageHeight = contentHeight * ratio;
                        sourceWid = BaseUtil.GetDoubleValue(element.Width / imageWidth * contentWidth);
                        sourceHei = BaseUtil.GetDoubleValue(element.Height / imageHeight * contentHeight);
                        break;
                    case Stretch.None:
                        imageWidth = contentWidth;
                        imageHeight = contentHeight;
                        break;
                }
            }
            Width = imageWidth;
            Height = imageHeight;
            SourceWidth = BaseUtil.GetDoubleValue(sourceWid);
            SourceHeight = BaseUtil.GetDoubleValue(sourceHei);
            Source = element.Source;
            Alignment = element.ImageAlign;
            Scale = element.ImageScale;
        }
    }
}
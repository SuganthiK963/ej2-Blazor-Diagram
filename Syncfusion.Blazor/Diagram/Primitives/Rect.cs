using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the width, height and position of a rectangle.
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent @ref = "diagram" Width="1000px" Height="1000px" @bind-Nodes="Nodes">
    /// </SfDiagramComponent>    
    /// @code
    /// {
    ///     SfDiagramComponent diagram;
    ///     private async Task PageBounds()
    ///     {
    ///         Rect bounds = diagram.GetPageBounds();
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class DiagramRect
    {
        /// <summary>
        /// Gets the position of the top-left corner of the rectangle.
        /// </summary>
        internal DiagramPoint TopLeft => new DiagramPoint { X = this.Left, Y = this.Top };

        /// <summary>
        /// Gets the position of the top-right corner of the rectangle.
        /// </summary>
        internal DiagramPoint TopRight => new DiagramPoint { X = this.Right, Y = this.Top };

        /// <summary>
        /// Gets or sets the position of the top-Center corner of the rectangle.
        /// </summary>
        internal DiagramPoint TopCenter => new DiagramPoint { X = this.X + this.Width / 2, Y = this.Top };

        /// <summary>
        /// Gets or sets the position of the middle-left corner of the rectangle.
        /// </summary>
        internal DiagramPoint MiddleLeft => new DiagramPoint { X = this.Left, Y = this.Y + this.Height / 2 };

        /// <summary>
        /// Gets or sets the position of the middle-right corner of the rectangle.
        /// </summary>
        internal DiagramPoint MiddleRight => new DiagramPoint { X = this.Right, Y = this.Y + this.Height / 2 };

        /// <summary>
        /// Gets or sets the position of the center of the rectangle.
        /// </summary>
        internal DiagramPoint Center => new DiagramPoint { X = this.X + this.Width / 2, Y = this.Y + this.Height / 2 };

        /// <summary>
        /// Gets or sets the position of the bottom-left corner of the rectangle.
        /// </summary>
        internal DiagramPoint BottomLeft => new DiagramPoint { X = this.Left, Y = this.Bottom };

        /// <summary>
        /// Gets or sets the position of the bottom-right corner of the rectangle.
        /// </summary>
        internal DiagramPoint BottomRight => new DiagramPoint { X = this.Right, Y = this.Bottom };

        /// <summary>
        /// Gets or sets the position of the bottom-center corner of the rectangle.
        /// </summary>
        internal DiagramPoint BottomCenter => new DiagramPoint { X = this.X + this.Width / 2, Y = this.Bottom };

        /// <summary>
        /// Gets or sets the x-axis value on the rectangle's left side.
        /// </summary>
        [JsonPropertyName("x")]
        public double X { get; set; } = double.MaxValue;
        /// <summary>
        /// Gets or sets the y-axis value on the rectangle's top side.
        /// </summary>
        [JsonPropertyName("y")]
        public double Y { get; set; } = double.MaxValue;
        /// <summary>
        ///Gets or sets the rectangle's width.
        /// </summary>
        [JsonPropertyName("width")]
        public double Width { get; set; }
        /// <summary>
        /// Gets or sets the rectangle's height.
        /// </summary>
        [JsonPropertyName("height")]
        public double Height { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramRect"/>.
        /// </summary>
        public DiagramRect()
        {
        }
        /// <summary>
        /// Creates a new instance of the <see cref="DiagramRect"/> from the given Rect.
        /// </summary>
        /// <param name="src">Rect.</param>
        public DiagramRect(DiagramRect src)
        {
            if (src != null)
            {
                X = src.X;
                Y = src.Y;
                Width = src.Width;
                Height = src.Height;
            }
        }
        /// <summary>
        /// Creates a new <see cref="DiagramRect"/> instance with the specified x-coordinate, y-coordinate, width, and height.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public DiagramRect(double? x, double? y, double? width, double? height)
        {
            if (x == null || y == null)
            {
                x = y = double.MaxValue;
                width = height = 0;
            }
            else
            {
                width ??= 0;
                height ??= 0;
            }
            this.X = x.Value;
            this.Y = y.Value;
            this.Width = width.Value;
            this.Height = height.Value;
        }

        /// <summary>
        /// Gets the x-axis value on the rectangle's left side.
        /// </summary>
        public double Left => this.X;

        /// <summary>
        /// Gets the x-axis value on the rectangle's right side.
        /// </summary>
        public double Right => this.X + this.Width;

        /// <summary>
        /// Gets the y-axis location on the rectangle's top.
        /// </summary>
        public double Top => this.Y;

        /// <summary>
        /// Gets the bottom of the rectangle's y-axis value.
        /// </summary>
        public double Bottom => this.Y + this.Height;

        internal bool ContainsPoint(DiagramPoint point, double padding = 0)
        {
            return this.X - padding <= point.X && (this.X + this.Width) + padding >= point.X
            && this.Y - padding <= point.Y && (this.Y + this.Height) + padding >= point.Y;
        }

        internal void UnitePoint(DiagramPoint point)
        {
            if (this.X == double.MaxValue)
            {
                this.X = point.X;
                this.Y = point.Y;
                return;
            }
            double x = Math.Min(this.Left, point.X);
            double y = Math.Min(this.Top, point.Y);
            double right = Math.Max(this.Right, point.X);
            double bottom = Math.Max(this.Bottom, point.Y);
            this.X = x;
            this.Y = y;
            this.Width = right - this.X;
            this.Height = bottom - this.Y;
        }

        internal static DiagramRect ToBounds(List<DiagramPoint> points)
        {
            DiagramRect rect = new DiagramRect(); int i;
            for (i = 0; i < points.Count; i++)
            {
                rect.UnitePoint(points[i]);
            }
            return rect;
        }

        internal static bool Equals(DiagramRect rect1, DiagramRect rect2)
        {
            return rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y) && rect1.Width.Equals(rect2.Width) && rect1.Height.Equals(rect2.Height);
        }
        internal DiagramRect UniteRect(DiagramRect rect)
        {
            double right = Math.Max((double.IsNaN(this.Right) || this.X == double.MaxValue) ? rect.Right : this.Right, rect.Right);
            double bottom = Math.Max((double.IsNaN(this.Bottom) || this.Y == double.MaxValue) ? rect.Bottom : this.Bottom, rect.Bottom);
            this.X = Math.Min(this.Left, rect.Left);
            this.Y = Math.Min(this.Top, rect.Top);
            this.Width = right - this.X;
            this.Height = bottom - this.Y;
            return this;
        }

        internal DiagramRect Inflate(double padding)
        {
            this.X -= padding;
            this.Y -= padding;
            this.Width += padding * 2;
            this.Height += padding * 2;
            return this;
        }

        internal bool Intersects(DiagramRect rect)
        {
            if (this.Right < rect.Left || this.Left > rect.Right || this.Top > rect.Bottom || this.Bottom < rect.Top)
            {
                return false;
            }
            return true;
        }
        internal bool ContainsRect(DiagramRect rect)
        {
            return this.Left <= rect.Left && this.Right >= rect.Right && this.Top <= rect.Top && this.Bottom >= rect.Bottom;
        }
        /// <summary>
        /// Creates a new rect that is a copy of the current rect.
        /// </summary>
        /// <returns>Rect</returns>
        public object Clone()
        {
            return new DiagramRect(this);
        }
        internal void Dispose()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }
    }
}
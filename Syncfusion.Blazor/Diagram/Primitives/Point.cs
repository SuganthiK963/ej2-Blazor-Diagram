using System;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents an ordered pair of integer x- and y-coordinates that defines a point in a two-dimensional plane.
    /// </summary>
    public class DiagramPoint : DiagramObject
    {
        private double x;
        private double y;
        /// <summary>
        /// Initializes a new instance of the DiagramPoint.
        /// </summary>
        public DiagramPoint()
        {
        }
        /// <summary>
        /// Creates a new instance of the DiagramPoint from the given point.
        /// </summary>
        /// <param name="src">Point.</param>
        public DiagramPoint(DiagramPoint src) : base(src)
        {
            if (src != null)
            {
                x = src.x;
                y = src.y;
            }
        }
        /// <summary>
        /// Gets or sets the x-coordinate of this point.
        /// </summary>
        [JsonPropertyName("x")]
        public double X
        {
            get => x;
            set
            {
                if (!x.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(X), value, x, this);
                    x = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the y-coordinate of this point.
        /// </summary>
        [JsonPropertyName("y")]
        public double Y
        {
            get => y;
            set
            {
                if (!y.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Y), value, y, this);
                    y = value;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the DiagramPoint struct with the specified coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the point.</param>
        /// <param name="y">The vertical position of the point.</param>
        public DiagramPoint(double? x, double? y)
        {
            x ??= 0;
            y ??= 0;
            this.x = x.Value;
            this.y = y.Value;
        }
        /// <summary>
        /// Transform the point based on the rotate angle and length.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static DiagramPoint Transform(DiagramPoint point, double angle, double length)
        {
            DiagramPoint pt = new DiagramPoint() { x = 0, y = 0 };
            pt.x = Math.Round((point.x + length * Math.Cos(angle * Math.PI / 180)) * 100) / 100;
            pt.y = Math.Round((point.y + length * Math.Sin(angle * Math.PI / 180)) * 100) / 100;
            return pt;
        }
        /// <summary>
        /// Find the angle between the two points.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        internal static double FindAngle(DiagramPoint point1, DiagramPoint point2)
        {
            double angle = Math.Atan2(point2.y - point1.y, point2.x - point1.x);
            angle = (180 * angle / Math.PI);
            angle %= 360;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }

        internal static bool Equals(DiagramPoint point1, DiagramPoint point2)
        {
            if (point1 == point2) { return true; }
            if (point1 == null || point2 == null) { return false; }
            return point1.X.Equals(point2.X) && point1.Y.Equals(point2.Y);
        }

        /// <summary>
        /// Check whether the points are given 
        /// </summary>
        internal static bool IsEmptyPoint(DiagramPoint point)
        {
            return point.x == 0 && point.y == 0;
        }
        internal static double DistancePoints(DiagramPoint pt1, DiagramPoint pt2)
        {
            return Math.Sqrt(Math.Pow(pt2.x - pt1.x, 2) + Math.Pow(pt2.y - pt1.y, 2));
        }

        internal static DiagramPoint AdjustPoint(DiagramPoint source, DiagramPoint target, bool isStart, double length)
        {
            DiagramPoint pt = isStart ? new DiagramPoint { x = source.x, y = source.y } : new DiagramPoint { x = target.x, y = target.y };
            if (source.x.Equals(target.x))
            {
                if (source.y < target.y && isStart || source.y > target.y && !isStart)
                {
                    pt.y += length;
                }
                else
                {
                    pt.y -= length;
                }

            }
            else if (source.y.Equals(target.y))
            {
                if (source.x < target.x && isStart || source.x > target.x && !isStart)
                {
                    pt.x += length;
                }
                else
                {
                    pt.x -= length;
                }
            }
            else
            {
                double angle;
                if (isStart)
                {
                    angle = FindAngle(source, target);
                    pt = Transform(source, angle, length);
                }
                else
                {
                    angle = FindAngle(target, source);
                    pt = Transform(target, angle, length);
                }
            }
            return pt;
        }
        internal static Direction Direction(DiagramPoint pt1, DiagramPoint pt2)
        {
            if (Math.Abs(pt2.x - pt1.x) > Math.Abs(pt2.y - pt1.y))
            {
                return pt1.x < pt2.x ? Diagram.Direction.Right : Diagram.Direction.Left;
            }
            else
            {
                return pt1.y < pt2.y ? Diagram.Direction.Bottom : Diagram.Direction.Top;
            }
        }
        internal static double FindLength(DiagramPoint s, DiagramPoint e)
        {
            double length = Math.Sqrt(Math.Pow((s.x - e.x), 2) + Math.Pow((s.y - e.y), 2));
            return length;
        }
        /// <summary>
        /// Creates a new point that is a copy of the current point. 
        /// </summary>
        /// <returns>Point</returns>
        public override object Clone()
        {
            return new DiagramPoint(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            x = 0;
            y = 0;
        }
    }
}
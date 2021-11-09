using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal static class BaseUtil
    {
        private const string PRE = "pre";
        private const string NOWRAP = "nowrap";
        private const string PRELINE = "pre-line";
        private const string PREWRAP = "pre-wrap";
        private const string BREAKALL = "breakall";
        private const string KEEPALL = "keepall";
        private const string NORMAL = "normal";
        private const string LINETHROUGH = "line-through";
        private const string LEFT = "left";
        private const string CENTER = "center";
        private const string RIGHT = "right";
        private const string WRAP = "Wrap";
        private const string NO_WRAP = "NoWrap";
        private const string WRAPWITHOVERFLOW = "WrapWithOverflow";
        private const string LINE_THROUGH = "LineThrough";

        internal static string RandomId()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
#pragma warning disable CA5394 // Do not use insecure randomness
            return new string(Enumerable.Repeat(chars, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());
#pragma warning restore CA5394 // Do not use insecure randomness
        }
        internal static DiagramSize RotateSize(DiagramSize size, double angle)
        {
            Matrix matrix = Matrix.IdentityMatrix();
            Matrix.RotateMatrix(matrix, angle, 0, 0);
            DiagramPoint topLeft = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = 0, Y = 0 });
            DiagramPoint topRight = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = GetDoubleValue(size.Width), Y = 0 });
            DiagramPoint bottomLeft = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = 0, Y = GetDoubleValue(size.Height) });
            DiagramPoint bottomRight = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = GetDoubleValue(size.Width), Y = GetDoubleValue(size.Height) });
            double minX = Math.Min(Math.Min(Math.Min(topLeft.X, topRight.X), bottomLeft.X), bottomRight.X);
            double minY = Math.Min(Math.Min(Math.Min(topLeft.Y, topRight.Y), bottomLeft.Y), bottomRight.Y);
            double maxX = Math.Max(Math.Max(Math.Max(topLeft.X, topRight.X), bottomLeft.X), bottomRight.X);
            double maxY = Math.Max(Math.Max(Math.Max(topLeft.Y, topRight.Y), bottomLeft.Y), bottomRight.Y);
            return new DiagramSize() { Width = maxX - minX, Height = maxY - minY };
        }

        internal static DiagramPoint RotatePoint(double angle, double pivotX, double pivotY, DiagramPoint point)
        {
            if (angle != 0)
            {
                Matrix matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, angle, pivotX, pivotY);
                return Matrix.TransformPointByMatrix(matrix, point);
            }
            return point;
        }

        internal static DiagramPoint GetOffset(DiagramPoint topLeft, DiagramElement obj)
        {
            double offX = topLeft.X + BaseUtil.GetDoubleValue(obj.DesiredSize.Width) * obj.Pivot.X;
            double offY = topLeft.Y + BaseUtil.GetDoubleValue(obj.DesiredSize.Height) * obj.Pivot.Y;
            return new DiagramPoint(offX, offY);
        }

        internal static DiagramRect CornersPointsBeforeRotation(DiagramElement ele)
        {
            double top = ele.OffsetY - GetDoubleValue(ele.ActualSize.Height) * ele.Pivot.Y;
            double bottom = ele.OffsetY + GetDoubleValue(ele.ActualSize.Height) * (1 - ele.Pivot.Y);
            double left = ele.OffsetX - GetDoubleValue(ele.ActualSize.Width) * ele.Pivot.X;
            double right = ele.OffsetX + GetDoubleValue(ele.ActualSize.Width) * (1 - ele.Pivot.X);
            DiagramPoint topLeft = new DiagramPoint(left, top);
            DiagramPoint topRight = new DiagramPoint(right, top);
            DiagramPoint bottomLeft = new DiagramPoint(left, bottom);
            DiagramPoint bottomRight = new DiagramPoint(right, bottom);
            DiagramRect bounds = DiagramRect.ToBounds(new List<DiagramPoint> { topLeft, topRight, bottomLeft, bottomRight });
            return bounds;
        }
        internal static DiagramRect GetSelectorBounds(DiagramSelectionSettings selector)
        {
            DiagramRect bounds = new DiagramRect(selector.OffsetX - selector.Width / 2, selector.OffsetY - selector.Height / 2, selector.Width, selector.Height); ;
            return bounds;
        }
        internal static DiagramRect GetBounds(DiagramElement element)
        {
            DiagramRect corners = CornersPointsBeforeRotation(element);
            DiagramPoint middleLeft = corners.MiddleLeft;
            DiagramPoint topCenter = corners.TopCenter;
            DiagramPoint bottomCenter = corners.BottomCenter;
            DiagramPoint middleRight = corners.MiddleRight;
            DiagramPoint topLeft = corners.TopLeft;
            DiagramPoint topRight = corners.TopRight;
            DiagramPoint bottomLeft = corners.BottomLeft;
            DiagramPoint bottomRight = corners.BottomRight;
            element.Corners = new Corners()
            {
                TopLeft = topLeft,
                TopCenter = topCenter,
                TopRight = topRight,
                MiddleLeft = middleLeft,
                MiddleRight = middleRight,
                BottomLeft = bottomLeft,
                BottomCenter = bottomCenter,
                BottomRight = bottomRight
            };
            if (element.RotationAngle != 0 || element.ParentTransform != 0)
            {
                Matrix matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, element.RotationAngle + element.ParentTransform, element.OffsetX, element.OffsetY);
                element.Corners.TopLeft = topLeft = Matrix.TransformPointByMatrix(matrix, topLeft);
                element.Corners.TopCenter = Matrix.TransformPointByMatrix(matrix, topCenter);
                element.Corners.TopRight = topRight = Matrix.TransformPointByMatrix(matrix, topRight);
                element.Corners.MiddleLeft = Matrix.TransformPointByMatrix(matrix, middleLeft);
                element.Corners.MiddleRight = Matrix.TransformPointByMatrix(matrix, middleRight);
                element.Corners.BottomLeft = bottomLeft = Matrix.TransformPointByMatrix(matrix, bottomLeft);
                element.Corners.BottomCenter = Matrix.TransformPointByMatrix(matrix, bottomCenter);
                element.Corners.BottomRight = bottomRight = Matrix.TransformPointByMatrix(matrix, bottomRight);
            }
            DiagramRect bounds = DiagramRect.ToBounds(new List<DiagramPoint> { topLeft, topRight, bottomLeft, bottomRight });
            element.Corners.Left = bounds.Left;
            element.Corners.Right = bounds.Right;
            element.Corners.Top = bounds.Top;
            element.Corners.Bottom = bounds.Bottom;
            element.Corners.Center = bounds.Center;
            element.Corners.Width = bounds.Width;
            element.Corners.Height = bounds.Height;
            return bounds;
        }
        internal static string WhiteSpaceToString(WhiteSpace value, TextWrap wrap)
        {
            if (wrap == TextWrap.NoWrap && value == WhiteSpace.PreserveAll)
            {
                return PRE;
            }
            string state = string.Empty;
            switch (value)
            {
                case WhiteSpace.CollapseAll:
                    state = NOWRAP;
                    break;
                case WhiteSpace.CollapseSpace:
                    state = PRELINE;
                    break;
                case WhiteSpace.PreserveAll:
                    state = PREWRAP;
                    break;
            }
            return state;
        }

        internal static string WordBreakToString(string value)
        {
            string state = string.Empty;
            switch (value)
            {
                case WRAP:
                    state = BREAKALL;
                    break;
                case NO_WRAP:
                    state = KEEPALL;
                    break;
                case WRAPWITHOVERFLOW:
                    state = NORMAL;
                    break;
                case LINE_THROUGH:
                    state = LINETHROUGH;
                    break;
            }
            return state;
        }

        internal static string TextAlignToString(TextAlign value)
        {
            string state = string.Empty;
            switch (value)
            {
                case TextAlign.Center:
                    state = CENTER;
                    break;
                case TextAlign.Left:
                    state = LEFT;
                    break;
                case TextAlign.Right:
                    state = RIGHT;
                    break;
            }
            return state;
        }

        internal static double GetDoubleValue(double? value)
        {
            return value ?? 0;
        }

        internal static T UpdateDictionary<T>(string key, T privateValue, T publicValue, Dictionary<string, object> dictionary)
        {
            if (!SfBaseUtils.Equals(privateValue, publicValue))
            {
                SfBaseUtils.UpdateDictionary(key, publicValue, dictionary);
                return publicValue;
            }
            return privateValue;
        }
    }
}
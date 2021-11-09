using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal static class ConnectorUtil
    {
        private const string FLOW = "Flow";
        private const string SEQUENCE = "Sequence";
        private const string ASSOCIATION = "Association";
        private const string MESSAGE = "Message";
        internal static List<DiagramPoint> FindConnectorPoints(Connector element, Layout layout = null)
        {
            DiagramPoint sourcePoint;
            if (element.Type == ConnectorSegmentType.Straight || element.SourceWrapper == null)
            {
                sourcePoint = GetSourcePoint(element);
            }
            else
            {
                sourcePoint = element.SourceWrapper.Corners.Center;
            }
            List<DiagramPoint> intermediatePoints = TerminateConnection(element, sourcePoint, element.TargetPoint, layout);
            SetLineEndPoint(element, intermediatePoints[0], false);
            SetLineEndPoint(element, intermediatePoints[^1], true);
            return intermediatePoints;
        }

        internal static DiagramPoint GetSourcePoint(Connector element)
        {
            DiagramPoint srcPoint;
            if (element.SourcePortWrapper != null)
            {
                DiagramElement srcPort = element.SourcePortWrapper;
                DiagramPoint pt = new DiagramPoint { X = srcPort.OffsetX, Y = srcPort.OffsetY };
                srcPoint = pt;
            }
            else if (!string.IsNullOrEmpty(element.SourceID) && element.SourceWrapper != null)
            {
                if (element.TargetWrapper != null)
                {
                    DiagramPoint sPoint = element.SourceWrapper.Corners.Center;
                    DiagramPoint tPoint = element.TargetWrapper.Corners.Center;
                    srcPoint = GetIntersection(element, sPoint, tPoint, false);
                }
                else
                {
                    srcPoint = element.SourcePoint;
                }
            }
            else
            {
                srcPoint = element.SourcePoint;
            }
            return srcPoint;
        }

        internal static List<DiagramPoint> TerminateConnection(Connector element, DiagramPoint srcPoint, DiagramPoint tarPoint, Layout layout)
        {
            DiagramElement sourceNode = element.SourceWrapper;
            DiagramElement targetNode = element.TargetWrapper;
            DiagramElement sourcePort = element.SourcePortWrapper;
            DiagramElement targetPort = element.TargetPortWrapper;
            List<DiagramPoint> intermediatePoints;
            double minSpace = 13;
            Margin sourceMargin = new Margin { Left = 5, Right = 5, Bottom = 5, Top = 5 };
            Margin targetMargin = new Margin { Left = 5, Right = 5, Bottom = 5, Top = 5 };
            End source = new End() { Point = srcPoint, Margin = sourceMargin };
            End target = new End() { Point = tarPoint, Margin = targetMargin };
            DiagramRect sourceCorners = null; DiagramRect targetCorners = null;
            if (sourceNode != null && targetNode != null)
            {
                sourceCorners = BaseUtil.CornersPointsBeforeRotation(sourceNode);
                targetCorners = BaseUtil.CornersPointsBeforeRotation(targetNode);
                source.Corners = sourceNode.Corners;
                target.Corners = targetNode.Corners;
            }
            if (sourcePort != null && sourceNode != null)
            {
                DiagramPoint port = new DiagramPoint { X = sourcePort.OffsetX, Y = sourcePort.OffsetY };
                source.Direction = GetPortDirection(port, sourceCorners, sourceNode.Bounds);
            }
            if (targetPort != null && targetNode != null)
            {
                DiagramPoint tarPortPt = new DiagramPoint { X = targetPort.OffsetX, Y = targetPort.OffsetY };
                target.Direction = GetPortDirection(tarPortPt, targetCorners, targetNode.Bounds);
            }
            if (sourceNode != null && targetNode != null)
            {
                if (source.Direction == null || target.Direction == null)
                {
                    if (layout != null && layout.Type == LayoutType.HierarchicalTree)
                    {
                        GetDirection(source, target, layout.Orientation);
                    }
                    else
                    {
                        Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
                        Margin srcMargin = source.Margin;
                        if (srcCorners.Top > tarCorners.Bottom &&
                            Math.Abs(srcCorners.Top - tarCorners.Bottom) >
                            (srcMargin.Top + srcMargin.Bottom))
                        {
                            source.Direction ??= Direction.Top;
                            target.Direction ??= Direction.Bottom;
                        }
                        else if (srcCorners.Bottom < tarCorners.Top &&
                          Math.Abs(srcCorners.Bottom - tarCorners.Top) >
                          (srcMargin.Bottom + srcMargin.Top))
                        {
                            source.Direction ??= Direction.Bottom;
                            target.Direction ??= Direction.Top;
                        }
                        else if ((srcCorners.Right < tarCorners.Left &&
                          Math.Abs(srcCorners.Right - tarCorners.Left) >
                          (srcMargin.Right + srcMargin.Left)) ||
                          ((srcCorners.Right + minSpace < tarCorners.Left) ||
                              (tarCorners.Right >= srcCorners.Left - minSpace && srcCorners.Left > tarCorners.Left)))
                        {
                            source.Direction ??= Direction.Right;
                            target.Direction ??= Direction.Left;
                        }
                        else if ((srcCorners.Left > tarCorners.Right &&
                          Math.Abs(srcCorners.Left - tarCorners.Right) > (srcMargin.Left + srcMargin.Right)) ||
                          tarCorners.Right + minSpace < srcCorners.Left ||
                              (srcCorners.Right >= tarCorners.Left - minSpace
                                  && srcCorners.Left < tarCorners.Left))
                        {
                            source.Direction ??= Direction.Left;
                            target.Direction ??= Direction.Right;
                        }
                        else
                        {
                            if (targetCorners != null && sourceNode.ID != targetNode.ID && (!DiagramRect.Equals(sourceCorners, targetCorners)) && targetCorners.ContainsPoint(sourceCorners.TopCenter, srcMargin.Top))
                            {
                                source.Direction ??= Direction.Bottom;
                                target.Direction ??= Direction.Top;
                            }
                            else
                            {
                                source.Direction ??= Direction.Top;
                                target.Direction ??= Direction.Bottom;
                            }
                        }
                    }
                }
                return DefaultOrthoConnection(element, source.Direction.Value, target.Direction.Value, source.Point, target.Point);
            }
            //It will be called only when there is only one end node
            CheckLastSegmentAsTerminal(element);
            if (element.SourceWrapper != null || element.TargetWrapper != null)
            {
                ConnectToOneEnd(element, source, target);
            }
            if (element.Type == ConnectorSegmentType.Straight || element.Type == ConnectorSegmentType.Bezier)
            {
                intermediatePoints = IntermediatePointsForStraight(element, source, target);
            }
            else
            {
                if (element.Type == ConnectorSegmentType.Orthogonal && element.Segments != null && element.Segments.Count > 0 &&
                    ((OrthogonalSegment)element.Segments[0]).Length.HasValue &&
                    ((OrthogonalSegment)element.Segments[0]).Direction.HasValue)
                {
                    intermediatePoints = FindPointToPointOrtho(element, source, target, sourceNode, targetNode, sourcePort, targetPort);
                }
                else
                {
                    double? extra = null;
                    if (!source.Direction.HasValue)
                    {
                        source.Direction = target.Direction.HasValue ? (
                            (element.TargetPortWrapper != null) ? target.Direction : GetOppositeDirection(target.Direction.Value)) :
                            DiagramPoint.Direction(source.Point, target.Point);
                    }
                    else
                    {
                        if (sourceNode != null) extra = AdjustSegmentLength(sourceNode.Bounds, source, 20);
                    }
                  (element.Segments[0] as OrthogonalSegment).Points = intermediatePoints = OrthoConnection3Segment(
                      element, source, target, extra);
                }
            }
            return intermediatePoints;
        }

        internal static DiagramPoint UpdateSegmentPoints(End source, OrthogonalSegment segment)
        {
            source.Direction = segment.Direction;
            segment.Points = new List<DiagramPoint>
            {
                source.Point
            };
            double extra = (segment.Direction == Direction.Left || segment.Direction == Direction.Top) ? -(segment.Length.Value) : segment.Length.Value;
            double angle = (segment.Direction == Direction.Left || segment.Direction == Direction.Right) ? 0 : 90;
            DiagramPoint segPoint = AddLineSegment(source.Point, extra, angle);
            segment.Points.Add(segPoint);
            return segPoint;
        }

        internal static List<DiagramPoint> PointToPoint(Connector element, End source, End target)
        {
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            List<DiagramPoint> point = null; Direction? direction = null; Direction? portdirection = null;
            Corners srcCorners = element.SourceWrapper?.Corners;
            if (element.SourcePortWrapper != null)
            {
                DiagramPoint port = new DiagramPoint { X = element.SourcePortWrapper.OffsetX, Y = element.SourcePortWrapper.OffsetY };
                portdirection = GetPortDirection(port, BaseUtil.CornersPointsBeforeRotation(element.SourceWrapper), element.SourceWrapper.Bounds);
                if (srcCorners != null && (source.Direction == Direction.Bottom || source.Direction == Direction.Top))
                {
                    if (tarPointX > srcCorners.Left && tarPointX < srcCorners.Right)
                    {
                        direction = (source.Point.Y > tarPointY) ? Direction.Top : Direction.Bottom;
                    }
                }
                else if (srcCorners != null && (source.Direction == Direction.Left || source.Direction == Direction.Right))
                {
                    if (tarPointY > srcCorners.Top && tarPointY < srcCorners.Bottom)
                    {
                        direction = (srcPointX > tarPointX) ? Direction.Left : Direction.Right;
                    }
                }
            }
            if (element.SourcePortWrapper != null && direction != null && portdirection == GetOppositeDirection(direction.Value))
            {
                double length = 0;
                if ((portdirection == Direction.Left || portdirection == Direction.Right) && (srcPointY >= srcCorners.Top
                    && srcPointY <= srcCorners.Center.Y) &&
                    (tarPointY >= srcCorners.Top && tarPointY <= srcCorners.Center.Y))
                {
                    source.Direction = Direction.Top;
                    length = srcPointY - srcCorners.Top + 20;
                }
                else if ((portdirection == Direction.Left || portdirection == Direction.Right) && srcPointY > srcCorners.Center.Y
                  && srcPointY <= srcCorners.Bottom &&
                  tarPointY > srcCorners.Center.Y && tarPointY <= srcCorners.Bottom)
                {
                    source.Direction = Direction.Bottom;
                    length = srcCorners.Bottom - srcPointY + 20;
                }
                else if ((portdirection == Direction.Top || portdirection == Direction.Bottom) && (srcPointX >= srcCorners.Left
                  && srcPointX <= srcCorners.Center.X) &&
                  tarPointX >= srcCorners.Left && tarPointX <= srcCorners.Center.X)
                {
                    source.Direction = Direction.Left;
                    length = srcPointX - srcCorners.Left + 20;
                }
                else if ((portdirection == Direction.Top || portdirection == Direction.Bottom) && srcPointX <= srcCorners.Right
                  && srcPointX > srcCorners.Center.X &&
                  tarPointX <= srcCorners.Right && tarPointX < srcCorners.Center.X)
                {
                    source.Direction = Direction.Right;
                    length = srcCorners.Right - srcPointX + 20;
                }
                if (source.Direction.HasValue && length != 0)
                {
                    point = OrthoConnection3Segment(element, source, target, length, true);
                }
            }
            else
            {
                if (source.Direction != null)
                    source.Direction =
                        direction ?? FindSourceDirection(source.Direction.Value, source.Point, target.Point);
                point = OrthoConnection2Segment(source, target);
            }
            return point;
        }

        internal static List<DiagramPoint> PointToNode(Connector element, End source, End target)
        {
            List<DiagramPoint> point;
            target.Corners = element.TargetWrapper.Corners;
            FindDirection(element.TargetWrapper, source, target, element);
            Direction direction = FindSourceDirection(target.Direction.Value, source.Point, target.Point);
            if (source.Direction == target.Direction && (source.Direction == Direction.Left || source.Direction == Direction.Right))
            {
                source.Direction = direction;
                point = OrthoConnection3Segment(element, source, target, element.TargetWrapper.Width / 2 + 20);
                End source1 = source; source1.Point = point[1];
                FindDirection(element.TargetWrapper, source, target, element);
                point = OrthoConnection3Segment(element, source, target);
            }
            else
            {
                source.Direction = direction;
                point = OrthoConnection2Segment(source, target);
            }
            return point;
        }

        internal static List<DiagramPoint> AddPoints(Connector element, End source, End target)
        {
            target.Corners = element.TargetWrapper.Corners;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y; double tarPointY = target.Point.Y;
            Direction? direction = null; double length = 0;
            Corners tarCorners = target.Corners; Corners srcCorners = source.Corners;
            Direction? beforeLastSegmentDirection = (element.Segments[^2] as OrthogonalSegment)?.Direction;
            if (source.Direction != Direction.Left && source.Direction != Direction.Right)
            {
                if (tarCorners.Center.Y.Equals(srcPointY) &&
                    (!(tarCorners.Left <= srcPointX && srcPointX <= tarCorners.Right)))
                {
                    direction = Direction.Top;
                    length = tarCorners.Height / 2 + 20;
                }
                else if ((tarCorners.Center.Y.Equals(srcPointY) &&
                  beforeLastSegmentDirection == Direction.Bottom) ||
                  (tarCorners.Center.Y > srcPointY && srcPointY >= tarCorners.Top))
                {
                    direction = Direction.Top;
                    length = (srcPointY - tarCorners.Top) + 20;
                }
                else if ((tarCorners.Center.Y.Equals(srcPointY) &&
                  beforeLastSegmentDirection == Direction.Top) ||
                  (tarCorners.Center.Y < srcPointY && srcPointY <= tarCorners.Bottom))
                {
                    direction = Direction.Bottom;
                    length = tarCorners.Bottom - srcPointY + 20;
                }
                else if (element.SourcePortWrapper != null && element.TargetPortWrapper != null &&
                  srcCorners.Top <= srcPointY && srcPointY <= srcCorners.Bottom)
                {
                    if (source.Direction != null) direction = source.Direction.Value;
                    length = (srcPointY > tarPointY) ? (srcPointY - srcCorners.Top + 20) :
                        (srcCorners.Bottom - srcPointY + 20);
                }
            }
            else
            {
                if (tarCorners.Center.X.Equals(srcPointX) &&
                    (!(tarCorners.Top < srcPointY && srcPointY <= tarCorners.Bottom)))
                {
                    direction = Direction.Left;
                    length = tarCorners.Width / 2 + 20;
                }
                else if ((tarCorners.Center.X.Equals(srcPointX) &&
                  beforeLastSegmentDirection == Direction.Right)
                  || (tarCorners.Center.X > srcPointX && srcPointX >= tarCorners.Left))
                {
                    direction = Direction.Left;
                    length = srcPointX - tarCorners.Left + 20;
                }
                else if ((tarCorners.Center.X.Equals(srcPointX) &&
                  beforeLastSegmentDirection == Direction.Left) ||
                  (tarCorners.Center.X <= srcPointX && srcPointX <= tarCorners.Right))
                {
                    direction = Direction.Right;
                    length = tarCorners.Right - srcPointX + 20;
                }
                else if (element.SourcePortWrapper != null && element.TargetPortWrapper != null &&
                  srcCorners.Left <= srcPointX && srcPointX <= srcCorners.Right)
                {
                    direction = source.Direction.Value;
                    length = (srcPointX > target.Point.X) ? (srcPointX - srcCorners.Left + 20) :
                        (srcCorners.Right - srcPointX + 20);
                }
            }
            double extra = (direction == Direction.Left || direction == Direction.Top) ? -(length) : length;
            double angle = (direction == Direction.Left || direction == Direction.Right) ? 0 : 90;
            DiagramPoint refPoint = source.Point;
            source.Point = AddLineSegment(source.Point, extra, angle);
            source.Direction = DiagramPoint.Direction(source.Point, target.Point);
            if (element.SourcePortWrapper != null && element.TargetPortWrapper != null &&
                (srcCorners.Center.X.Equals(tarCorners.Center.X) || srcCorners.Center.Y.Equals(tarCorners.Center.Y)))
            {
                source.Direction = target.Direction;
            }
            List<DiagramPoint> point = OrthoConnection3Segment(element, source, target);
            point.Insert(0, refPoint);
            return point;
        }

        internal static bool FindSegmentDirection(Connector element, End source, End target, Direction? portDir)
        {
            bool update = false;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            Corners tarCorners = target.Corners; Corners srcCorners = source.Corners;
            switch (target.Direction)
            {
                case Direction.Left:
                    if (element.SourcePortWrapper != null && element.TargetPortWrapper != null && (portDir == Direction.Right &&
                       srcPointX > tarPointX && srcPointY >= srcCorners.Top && srcPointY <= srcCorners.Bottom)
                       || (((portDir == Direction.Bottom && srcPointY > tarPointY) || (portDir == Direction.Top && srcPointY < tarPointY)) &&
                       srcPointX >= srcCorners.Left && srcPointX <= srcCorners.Right))
                    {
                        source.Direction = (portDir == Direction.Right) ? ((srcPointY > tarPointY) ? Direction.Top : Direction.Bottom) :
                            (srcPointX < tarPointX ? Direction.Right : Direction.Left);
                        update = true;
                    }
                    else if (srcPointX > tarPointX && (srcPointY > tarPointY || srcPointY < tarPointY)
               && (!(tarCorners.Top > srcPointY && tarCorners.Bottom < srcPointY)))
                    {
                        source.Direction = Direction.Left;
                    }
                    else if ((srcPointX < tarPointX && srcPointY > tarPointY) ||
                      (srcPointX > tarPointX && (srcPointY <= tarPointY)
                          && tarCorners.Top < srcPointY && tarCorners.Center.Y >= srcPointY))
                    {
                        source.Direction = Direction.Top;
                    }
                    else if ((srcPointX < tarPointX && srcPointY < tarPointY) ||
                      (srcPointX > tarPointX && (srcPointY > tarPointY)
                          && ((tarCorners.Bottom < srcPointY && tarCorners.Center.Y > srcPointY))))
                    {
                        source.Direction = Direction.Bottom;
                    }
                    else if (srcPointY.Equals(tarPointY) && srcPointX < tarPointX)
                    {
                        source.Direction = Direction.Right;
                    }
                    break;
                case Direction.Right:
                    if (element.SourcePortWrapper != null && element.TargetPortWrapper != null &&
          ((portDir == Direction.Bottom && srcPointY > tarPointY) ||
              (portDir == Direction.Top && srcPointY < tarPointY)) && srcPointX > tarPointX &&
          srcPointX >= srcCorners.Left && srcPointX <= srcCorners.Right)
                    {
                        source.Direction = (srcPointX > tarPointX) ? Direction.Left : Direction.Right;
                        update = true;
                    }
                    else if (element.SourcePortWrapper != null && element.TargetPortWrapper != null &&
                      portDir == Direction.Left && srcPointX < tarPointX && srcPointY >= srcCorners.Top &&
                          srcPointY <= srcCorners.Bottom)
                    {
                        source.Direction = (srcPointY > tarPointY) ? Direction.Top : Direction.Bottom;
                        update = true;
                    }
                    else if (srcPointX < tarPointX && tarCorners.Top <= srcPointY
                      && tarCorners.Bottom >= srcPointY && srcPointY.Equals(tarPointY))
                    {
                        source.Direction = Direction.Top;
                    }
                    else if (srcPointY > tarPointY && srcPointX > tarPointX)
                    {
                        source.Direction = Direction.Top;
                    }
                    else if (srcPointY < tarPointY && srcPointX > tarPointX)
                    {
                        source.Direction = Direction.Bottom;
                    }
                    else if (srcPointX < tarPointX && (srcPointY > tarPointY ||
                      srcPointY < tarPointY))
                    {
                        source.Direction = Direction.Right;
                    }
                    else if (srcPointY.Equals(tarPointY) && srcPointX > tarPointX)
                    {
                        source.Direction = Direction.Left;
                    }
                    break;
                case Direction.Top:
                    if (element.SourcePortWrapper != null && element.TargetPortWrapper != null && (portDir == Direction.Bottom &&
            srcPointY > tarPointY && srcPointX >= srcCorners.Left &&
            srcPointX <= srcCorners.Right) || (((portDir == Direction.Right && srcPointX > tarPointX) ||
                (portDir == Direction.Left && tarPointY > srcPointY && tarPointX > srcPointX)) &&
                (srcPointY >= srcCorners.Top && srcPointY <= srcCorners.Bottom)))
                    {
                        source.Direction = (portDir == Direction.Bottom) ? ((srcPointX > tarPointX) ? Direction.Left : Direction.Right) :
                            (srcPointY < tarPointY) ? Direction.Bottom : Direction.Top;
                        update = true;
                    }
                    else if (srcPointX.Equals(tarPointX) && srcPointY < tarPointY)
                    {
                        source.Direction = Direction.Bottom;
                    }
                    else if (srcPointY > tarPointY && srcPointX > tarCorners.Left &&
                      srcPointX < tarCorners.Right)
                    {
                        source.Direction = Direction.Left;
                    }
                    else if (srcPointY >= tarPointY)
                    {
                        source.Direction = Direction.Top;
                    }
                    else if (srcPointY < tarPointY && srcPointX > tarPointX)
                    {
                        source.Direction = Direction.Left;
                    }
                    else if (srcPointY < tarPointY && srcPointX < tarPointX)
                    {
                        source.Direction = Direction.Right;
                    }
                    break;
                case Direction.Bottom:
                    if (element.SourcePortWrapper != null && element.TargetPortWrapper != null && ((((portDir == Direction.Right) ||
         (portDir == Direction.Left && tarPointX > srcPointX)) && (srcPointY > tarPointY) &&
         srcPointY >= srcCorners.Top && srcPointY <= srcCorners.Bottom) ||
         (portDir == Direction.Top && srcPointY < tarPointY &&
             srcPointX >= srcCorners.Left && srcPointX <= srcCorners.Right)))
                    {
                        if (portDir == Direction.Right || portDir == Direction.Left)
                        {
                            source.Direction = (srcPointY > tarPointY) ? Direction.Top : Direction.Bottom;
                        }
                        else
                        {
                            source.Direction = (srcPointX > tarPointX) ? Direction.Left : Direction.Right;
                        }
                        update = true;
                    }
                    else if (srcPointY < tarPointY && srcPointX > tarCorners.Left && tarCorners.Right > srcPointX)
                    {
                        if (srcPointY < tarPointY && srcPointX > tarCorners.Left && tarCorners.Center.X >= srcPointX)
                        {
                            source.Direction = Direction.Left;
                        }
                        else if (srcPointY < tarPointY && srcPointX < tarCorners.Right && tarCorners.Center.X < srcPointX)
                        {
                            source.Direction = Direction.Right;
                        }
                    }
                    else if (srcPointY > tarPointY && srcPointX > tarPointX)
                    {
                        source.Direction = Direction.Left;
                    }
                    else if (srcPointY > tarPointY && srcPointX < tarPointX)
                    {
                        source.Direction = Direction.Right;
                    }
                    else if (srcPointY <= tarPointY && (srcPointX > tarPointX || tarPointX > srcPointX))
                    {
                        source.Direction = Direction.Bottom;
                    }
                    break;
            }
            return update;
        }

        internal static List<DiagramPoint> PointToPort(Connector element, End source, End target)
        {
            List<DiagramPoint> point; target.Corners = element.TargetWrapper.Corners; Direction? portDirection = null;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;

            if (element.SourcePortWrapper != null)
            {
                DiagramPoint port = new DiagramPoint() { X = element.SourcePortWrapper.OffsetX, Y = element.SourcePortWrapper.OffsetY };
                portDirection = GetPortDirection(port, BaseUtil.CornersPointsBeforeRotation(element.SourceWrapper), element.SourceWrapper.Bounds);
            }
            bool update = false;
            update = FindSegmentDirection(element, source, target, portDirection != null ? portDirection.Value : portDirection);
            if (element.SourcePortWrapper != null && element.TargetPortWrapper != null && portDirection != null &&
                target.Direction == GetOppositeDirection(portDirection.Value) &&
                ((((target.Direction == Direction.Left && srcPointX > tarPointX) || (target.Direction == Direction.Right &&
                    srcPointX < tarPointX)) && srcPointY >= srcCorners.Top &&
                    srcPointY <= srcCorners.Bottom) || (target.Direction == Direction.Bottom && srcPointY < tarPointY &&
                        srcPointX >= srcCorners.Left && srcPointX <= srcCorners.Right)))
            {
                point = AddPoints(element, source, target);
            }
            else if (source.Direction == target.Direction)
            {
                point = OrthoConnection3Segment(element, source, target);
            }
            else if ((((target.Direction == Direction.Left && srcPointX > tarPointX) ||
              (target.Direction == Direction.Right && srcPointX < tarPointX)) && (source.Direction == Direction.Top || source.Direction == Direction.Bottom)
              && ((srcPointY <= tarPointY) &&
                  ((tarCorners.Top <= srcPointY && tarCorners.Bottom >= srcPointY)))) ||
              ((target.Direction == Direction.Top && srcPointY > tarPointY) ||
                  (target.Direction == Direction.Bottom && srcPointY < tarPointY) &&
                  ((tarCorners.Left <= srcPointX && tarCorners.Right >= srcPointX))))
            {
                point = AddPoints(element, source, target);
            }
            else
            {
                double length;
                if (element.SourceWrapper != null && element.TargetWrapper != null && element.TargetPortWrapper != null &&
                    ((source.Direction == Direction.Left || source.Direction == Direction.Right) &&
                     (srcPointY >= srcCorners.Top && srcPointY <= srcCorners.Bottom)
                     && (target.Direction == Direction.Top || target.Direction == Direction.Bottom) &&
                     (tarCorners.Center.X.Equals(srcCorners.Center.X))))
                {
                    source.Direction = (target.Direction == Direction.Top) ? Direction.Bottom : Direction.Top;
                    length = (target.Direction == Direction.Top) ? (srcCorners.Bottom - srcPointY + 20) :
                        (srcPointY - srcCorners.Top + 20);
                    point = OrthoConnection3Segment(element, source, target, length);
                }
                else if (element.SourceWrapper != null && element.TargetWrapper != null && element.TargetPortWrapper != null &&
                  ((source.Direction == Direction.Top || source.Direction == Direction.Bottom) &&
                      (srcPointX >= srcCorners.Left && srcPointX <= srcCorners.Right) &&
                      (target.Direction == Direction.Left || target.Direction == Direction.Right) && (tarCorners.Center.Y.Equals(srcCorners.Center.Y))))
                {
                    source.Direction = (target.Direction == Direction.Left) ? Direction.Right : Direction.Left;
                    length = (target.Direction == Direction.Left) ? (srcCorners.Right - srcPointX + 20) :
                        (srcPointX - srcCorners.Left + 20);
                    point = OrthoConnection3Segment(element, source, target, length);
                }
                else if (update)
                {
                    if (source.Direction == Direction.Left || source.Direction == Direction.Right)
                    {
                        length = (source.Direction == Direction.Left) ? (srcPointX - srcCorners.Left + 20) :
                            (srcCorners.Right - srcPointX + 20);
                    }
                    else
                    {
                        length = (source.Direction == Direction.Top) ? (srcPointY - srcCorners.Top + 20) :
                            (srcCorners.Bottom - srcPointY + 20);
                    }
                    point = OrthoConnection3Segment(element, source, target, length);
                }
                else
                {
                    point = OrthoConnection2Segment(source, target);
                }
            }
            return point;
        }

        internal static List<DiagramPoint> FindPointToPointOrtho(Connector element, End source, End target, DiagramElement sourceNode, DiagramElement targetNode, DiagramElement sourcePort, DiagramElement targetPort)
        {
            int j;
            List<DiagramPoint> intermediatePoints = new List<DiagramPoint>();
            CheckLastSegmentAsTerminal(element); int removeSegment = 0;
            if (element.Segments.Count > 0)
            {
                for (int i = 0; i < element.Segments.Count; i++)
                {
                    OrthogonalSegment seg = element.Segments[i] as OrthogonalSegment;
                    if (i == 0 && element.SourcePortWrapper != null)
                    {
                        DiagramPoint port = new DiagramPoint { X = sourcePort.OffsetX, Y = sourcePort.OffsetY };
                        Direction direction = GetPortDirection(port, BaseUtil.CornersPointsBeforeRotation(sourceNode), sourceNode.Bounds);
                        if (seg.Direction == GetOppositeDirection(direction))
                        {
                            seg.Direction = direction;
                        }
                    }
                    if (i > 0 && (element.Segments[i - 1] as OrthogonalSegment).Direction == seg.Direction)
                    {
                        i = CheckConsectiveSegmentAsSame(element, i, source);
                    }
                    else
                    {
                        if (seg.Direction.HasValue)
                        {
                            source.Point = UpdateSegmentPoints(source, seg);
                        }
                        else
                        {
                            OrthogonalSegment lastSegment = element.Segments[i - 1] as OrthogonalSegment;
                            source.Point = lastSegment.Points[^1];
                        }
                    }
                    if (i == element.Segments.Count - 1)
                    {
                        List<DiagramPoint> point;
                        if (targetPort == null && targetNode == null)
                        {
                            point = PointToPoint(element, source, target);
                        }
                        else if (element.TargetWrapper != null && element.TargetPortWrapper == null)
                        {
                            CheckSourcePointInTarget(element, source);
                            point = PointToNode(element, source, target);
                        }
                        else
                        {
                            point = PointToPort(element, source, target);
                        }
                        if (point != null)
                        {
                            CheckPreviousSegment(point, element);
                            seg.Points = new List<DiagramPoint>();
                            if (point.Count >= 2)
                            {
                                for (j = 0; j < point.Count; j++)
                                {
                                    seg.Points.Add(point[j]);
                                }
                            }
                            else
                            {
                                removeSegment = i;
                            }
                        }
                    }
                    if (sourcePort != null && i == 0)
                    {
                        DiagramPoint sourcePoint = CheckPortDirection(element, sourcePort, sourceNode);
                        if (sourcePoint != null)
                        {
                            source.Point = sourcePoint;
                        }
                    }
                }
                if (removeSegment != 0)
                {
                    if (removeSegment == element.Segments.Count - 1)
                    {
                        (element.Segments[removeSegment - 1] as OrthogonalSegment).Direction = null;
                        (element.Segments[removeSegment - 1] as OrthogonalSegment).Length = null;
                    }
                    element.Segments.RemoveAt(removeSegment);
                }
                intermediatePoints = ReturnIntermediatePoints(element, intermediatePoints);
            }
            return intermediatePoints;
        }

        internal static DiagramPoint CheckPortDirection(Connector element, DiagramElement sourcePort, DiagramElement sourceNode)
        {
            DiagramPoint port = new DiagramPoint { X = sourcePort.OffsetX, Y = sourcePort.OffsetY };
            DiagramPoint point = null; DiagramRect bounds = BaseUtil.CornersPointsBeforeRotation(sourceNode);
            Direction direction = GetPortDirection(port, bounds, sourceNode.Bounds);
            OrthogonalSegment seg = (element.Segments[0] as OrthogonalSegment);
            if (seg?.Direction != direction)
            {
                PointsFromNodeToPoint(seg, direction, bounds, seg.Points[0], seg.Points[^1], false);
                point = seg.Points[^1];
                seg.Direction = DiagramPoint.Direction(seg.Points[^2], seg.Points[^1]);
            }
            return point;
        }

        internal static void CheckPreviousSegment(List<DiagramPoint> tPoints, Connector connector)
        {
            OrthogonalSegment actualSegment = connector.Segments[^2] as OrthogonalSegment;
            DiagramPoint actualLastPoint = actualSegment?.Points[^1];
            DiagramElement srcWrapper = connector.SourceWrapper;
            if (actualLastPoint != null && (((actualSegment.Direction == Direction.Top || actualSegment.Direction == Direction.Bottom) && (actualLastPoint.X.Equals(tPoints[1].X))) ||
                                            ((actualSegment.Direction == Direction.Left || actualSegment.Direction == Direction.Right) && (actualLastPoint.Y.Equals(tPoints[1].Y)))))
            {

                actualSegment.Points[^1] = tPoints[1];
                Direction direction = DiagramPoint.Direction(
                    actualSegment.Points[0], actualSegment.Points[^1]);
                if (srcWrapper != null && connector.SourcePortWrapper == null &&
                    direction == GetOppositeDirection(actualSegment.Direction.Value))
                {
                    if (actualSegment.Direction == Direction.Left || actualSegment.Direction == Direction.Right)
                    {
                        actualSegment.Points[0].X = (actualSegment.Direction == Direction.Right) ?
                            actualSegment.Points[0].X - srcWrapper.Corners.Width :
                            actualSegment.Points[0].X + srcWrapper.Corners.Width;
                    }
                    else
                    {
                        actualSegment.Points[0].Y = (actualSegment.Direction == Direction.Bottom) ?
                        actualSegment.Points[0].Y - srcWrapper.Corners.Height :
                        actualSegment.Points[0].Y + srcWrapper.Corners.Height;
                    }
                }
                actualSegment.Direction = direction;
                actualSegment.Length = DiagramPoint.DistancePoints(actualSegment.Points[0], actualSegment.Points[^1]);
                tPoints.RemoveAt(0);
            }
        }

        internal static void GetDirection(End source, End target, LayoutOrientation layoutOrientation)
        {
            if (layoutOrientation == LayoutOrientation.LeftToRight)
            {
                source.Direction ??= Direction.Right;
                target.Direction ??= Direction.Left;
            }
            else if (layoutOrientation == LayoutOrientation.RightToLeft)
            {
                source.Direction ??= Direction.Left;
                target.Direction ??= Direction.Right;
            }
            else if (layoutOrientation == LayoutOrientation.TopToBottom)
            {
                source.Direction ??= Direction.Bottom;
                target.Direction ??= Direction.Top;
            }
            else if (layoutOrientation == LayoutOrientation.BottomToTop)
            {
                source.Direction ??= Direction.Top;
                target.Direction ??= Direction.Bottom;
            }
        }
        internal static void ConnectToOneEnd(Connector element, End source, End target)
        {
            DiagramElement sourcePort = element.SourcePortWrapper;
            DiagramElement targetPort = element.TargetPortWrapper;
            DiagramElement node = element.SourceWrapper;
            DiagramPoint fixedPoint = source.Point;
            Margin nodeMargin;
            DiagramPoint nodeConnectingPoint;
            Direction nodeDirection = Direction.Top;

            if (node == null)
            {
                node = element.TargetWrapper;
                nodeMargin = target.Margin;
            }
            else
            {
                fixedPoint = target.Point;
                nodeMargin = source.Margin;
            }

            if (element.Type == ConnectorSegmentType.Orthogonal)
            {
                if (element.Segments != null && element.Segments.Count > 0 && element.SourceWrapper != null &&
                    ((OrthogonalSegment)element.Segments[0]).Direction.HasValue)
                {
                    source.Direction = ((OrthogonalSegment)element.Segments[0]).Direction;
                    nodeConnectingPoint = FindPoint(node.Corners, source.Direction.Value);
                    DiagramPoint refPoint = FindPoint(node.Corners, GetOppositeDirection(source.Direction.Value).Value);
                    nodeConnectingPoint = GetIntersection(element, nodeConnectingPoint, refPoint, false);
                }
                else
                {
                    End sourceEnd = new End { Corners = null, Direction = null, Point = fixedPoint, Margin = nodeMargin };
                    End targetEnd = new End { Corners = null, Direction = null, Point = null, Margin = null };
                    FindDirection(node, sourceEnd, targetEnd, element);
                    nodeConnectingPoint = targetEnd.Point;
                    nodeDirection = targetEnd.Direction.Value;
                }
            }
            else
            {
                DiagramPoint segmentPoint = null;
                if (element.Segments != null && element.Segments.Count > 1)
                {
                    if (node == element.SourceWrapper)
                    {
                        segmentPoint = ((StraightSegment)element.Segments[0]).Point;
                    }
                    else
                    {
                        segmentPoint = ((StraightSegment)element.Segments[^2]).Point;
                    }
                }
                nodeConnectingPoint = GetIntersection(
                    element, node.Bounds.Center, (element.Segments != null && element.Segments.Count > 1) ? segmentPoint : fixedPoint, node == element.TargetWrapper);
            }
            if (node == element.SourceWrapper)
            {
                source.Direction ??= nodeDirection;
                source.Point = nodeConnectingPoint;
                if (element.SourcePortWrapper != null)
                {
                    source.Point = new DiagramPoint { X = sourcePort.OffsetX, Y = sourcePort.OffsetY };
                    if (element.SourcePadding != 0)
                    {
                        source.Point = AddPaddingToConnector(element, source, target, false);
                    }
                }
            }
            else
            {
                target.Direction ??= nodeDirection;
                target.Point = nodeConnectingPoint;
                if (element.TargetPortWrapper != null)
                {
                    target.Point = new DiagramPoint { X = targetPort.OffsetX, Y = targetPort.OffsetY };
                    if (element.TargetPadding != 0)
                    {
                        target.Point = AddPaddingToConnector(element, source, target, true);
                    }
                }
            }
        }
        internal static DiagramPoint AddPaddingToConnector(Connector element, End source, End target, bool isTarget)
        {
            DiagramElement sourcePort = element.SourcePortWrapper;
            DiagramElement targetPort = element.TargetPortWrapper;
            double padding = isTarget ? element.TargetPadding : element.SourcePadding;
            DiagramElement paddingPort = isTarget ? targetPort : sourcePort;
            DiagramRect rect = new DiagramRect
            {
                X = paddingPort.Bounds.X - padding,
                Y = paddingPort.Bounds.Y - padding,
                Width = BaseUtil.GetDoubleValue(paddingPort.ActualSize.Width) + 2 * padding,
                Height = BaseUtil.GetDoubleValue(paddingPort.ActualSize.Height) + 2 * padding
            };
            List<DiagramPoint> segmentPoints = new List<DiagramPoint> { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft };
            segmentPoints.Add(segmentPoints[0]);
            Segment thisSegment = new Segment { X1 = source.Point.X, Y1 = source.Point.Y, X2 = target.Point.X, Y2 = target.Point.Y };
            DiagramPoint point = isTarget ? target.Point : source.Point;
            DiagramPoint newPoint = GetIntersectionPoints(thisSegment, segmentPoints, point);
            return newPoint ?? point;
        }
        internal static bool CheckSourceAndTargetIntersect(DiagramElement sourceWrapper, DiagramElement targetWrapper, Connector connector)
        {
            List<Segment> sourceSegment = CreateSegmentsCollection(sourceWrapper, connector.SourcePadding);
            List<Segment> targetSegment = CreateSegmentsCollection(targetWrapper, connector.TargetPadding);
            for (int i = 0; i < sourceSegment.Count - 1; i++)
            {
                Segment srcSegment = sourceSegment[i];
                int j;
                for (j = 0; j < targetSegment.Count - 1; j++)
                {
                    Segment tarSegment = targetSegment[j];
                    if (DiagramUtil.Intersect3(srcSegment, tarSegment).Enabled)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        internal static List<Segment> CreateSegmentsCollection(DiagramElement sourceWrapper, double padding)
        {
            List<Segment> segments = new List<Segment>();
            List<DiagramPoint> points = DiagramUtil.GetPoints(sourceWrapper.Corners, padding);
            points.Add(points[0]);
            for (int i = 0; i < points.Count - 1; i++)
            {
                segments.Add(CreateLineSegment(points[i], points[i + 1]));
            }
            return segments;
        }

        internal static Segment CreateLineSegment(DiagramPoint sPt, DiagramPoint tPt)
        {
            return new Segment { X1 = sPt.X, Y1 = sPt.Y, X2 = tPt.X, Y2 = tPt.Y };
        }

        internal static Corners SwapBounds(DiagramElement obj, Corners bounds, DiagramRect outerBounds)
        {
            double rotateAngle = obj.RotationAngle + obj.ParentTransform;
            if (rotateAngle != 0)
            {
                Corners rectBounds;
                if (rotateAngle < 45)
                {
                    return bounds;
                }
                else if (rotateAngle <= 135)
                {
                    rectBounds = new Corners
                    {
                        Width = bounds.Width,
                        Height = bounds.Height,
                        TopLeft = bounds.BottomLeft,
                        TopCenter = bounds.MiddleLeft,
                        TopRight = bounds.TopLeft,
                        MiddleLeft = bounds.BottomCenter,
                        Center = outerBounds.Center,
                        MiddleRight = bounds.TopCenter,
                        BottomLeft = bounds.BottomRight,
                        BottomCenter = bounds.MiddleRight,
                        BottomRight = bounds.TopRight,
                        Left = outerBounds.Left,
                        Right = outerBounds.Right,
                        Top = outerBounds.Top,
                        Bottom = outerBounds.Bottom
                    };
                }
                else if (rotateAngle <= 225)
                {
                    rectBounds = new Corners
                    {
                        Width = bounds.Width,
                        Height = bounds.Height,
                        TopLeft = bounds.BottomLeft,
                        TopCenter = bounds.BottomCenter,
                        TopRight = bounds.BottomRight,
                        MiddleLeft = bounds.MiddleRight,
                        Center = outerBounds.Center,
                        MiddleRight = bounds.MiddleLeft,
                        BottomLeft = bounds.TopLeft,
                        BottomCenter = bounds.TopCenter,
                        BottomRight = bounds.TopRight,
                        Left = outerBounds.Left,
                        Right = outerBounds.Right,
                        Top = outerBounds.Top,
                        Bottom = outerBounds.Bottom
                    };
                }
                else if (rotateAngle <= 315)
                {
                    rectBounds = new Corners
                    {
                        Width = bounds.Width,
                        Height = bounds.Height,
                        TopLeft = bounds.TopRight,
                        TopCenter = bounds.MiddleRight,
                        TopRight = bounds.BottomRight,
                        MiddleLeft = bounds.TopCenter,
                        Center = outerBounds.Center,
                        MiddleRight = bounds.BottomCenter,
                        BottomLeft = bounds.TopLeft,
                        BottomCenter = bounds.MiddleLeft,
                        BottomRight = bounds.BottomLeft,
                        Left = outerBounds.Left,
                        Right = outerBounds.Right,
                        Top = outerBounds.Top,
                        Bottom = outerBounds.Bottom
                    };
                }
                else
                {
                    return bounds;
                }
                return rectBounds;
            }
            return bounds;
        }

        internal static List<DiagramPoint> DefaultOrthoConnection(Connector ele, Direction srcDir, Direction tarDir, DiagramPoint sPt, DiagramPoint tPt)
        {
            DiagramElement sourceEle = ele.SourceWrapper; DiagramElement targetEle = ele.TargetWrapper;
            DiagramElement srcPort = ele.SourcePortWrapper; DiagramElement tarPort = ele.TargetPortWrapper;
            List<DiagramPoint> intermediatePoints; DiagramPoint refPoint; Corners srcCor = sourceEle.Corners;
            Corners tarCor = targetEle.Corners;
            Margin sourceMargin = new Margin { Left = 5, Right = 5, Bottom = 5, Top = 5 };
            Margin targetMargin = new Margin { Left = 5, Right = 5, Bottom = 5, Top = 5 };
            End source = new End { Corners = srcCor, Point = sPt, Direction = srcDir, Margin = sourceMargin };
            End target = new End { Corners = tarCor, Point = tPt, Direction = tarDir, Margin = targetMargin };
            Corners srcBounds = SwapBounds(sourceEle, srcCor, ele.SourceWrapper.Bounds);
            Corners tarBounds = SwapBounds(targetEle, tarCor, ele.TargetWrapper.Bounds);
            bool isInterSect = false;
            if (ele.SourceWrapper != null && ele.TargetWrapper != null)
            {
                isInterSect = CheckSourceAndTargetIntersect(ele.SourceWrapper, ele.TargetWrapper, ele);
            }
            if (srcPort != null)
            {
                source.Point = new DiagramPoint { X = srcPort.OffsetX, Y = srcPort.OffsetY };
                switch (source.Direction)
                {
                    case Direction.Bottom:
                    case Direction.Top:
                        source.Point.Y = source.Point.Y;
                        break;
                    case Direction.Left:
                    case Direction.Right:
                        source.Point.X = source.Point.X;
                        break;
                }
                if (ele.SourcePadding != 0 && !isInterSect)
                {
                    if (tarPort != null)
                    {
                        target.Point = new DiagramPoint { X = tarPort.OffsetX, Y = tarPort.OffsetY };
                    }
                    source.Point = AddPaddingToConnector(ele, source, target, false);
                }
            }
            else
            {
                if (ele.Type == ConnectorSegmentType.Orthogonal)
                {
                    if (ele.Segments != null && ele.Segments.Count > 0 && ((OrthogonalSegment)ele.Segments[0]).Direction.HasValue)
                    {
                        source.Direction = ((OrthogonalSegment)ele.Segments[0]).Direction;
                    }
                    source.Point = FindPoint(srcBounds, source.Direction.Value);
                    refPoint = FindPoint(srcBounds, GetOppositeDirection(source.Direction.Value).Value);
                    source.Point = GetIntersection(ele, source.Point, refPoint, false);
                }
                else { source.Point = sourceEle.Corners.Center; }
            }
            if (tarPort != null)
            {
                target.Point = new DiagramPoint { X = tarPort.OffsetX, Y = tarPort.OffsetY };
                switch (target.Direction)
                {
                    case Direction.Bottom:
                    case Direction.Top:
                        target.Point.Y = target.Point.Y;
                        break;
                    case Direction.Left:
                    case Direction.Right:
                        target.Point.X = target.Point.X;
                        break;
                }
                if (ele.TargetPadding != 0 && !isInterSect)
                {
                    target.Point = AddPaddingToConnector(ele, source, target, true);
                }
            }
            else
            {
                if (ele.Type == ConnectorSegmentType.Orthogonal)
                {
                    target.Point = FindPoint(tarBounds, target.Direction.Value);
                    refPoint = FindPoint(tarBounds, GetOppositeDirection(target.Direction.Value).Value);
                    target.Point = GetIntersection(ele, target.Point, refPoint, true);
                }
                else { target.Point = targetEle.Corners.Center; }
            }
            if (ele.Type != ConnectorSegmentType.Orthogonal)
            {
                StraightSegment segment = null;
                StraightSegment first; Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
                CheckLastSegmentAsTerminal(ele);
                if (ele.SourcePortWrapper == null)
                {
                    source.Point = srcCorners.Center;
                    if (ele.Segments != null && ele.Segments.Count > 0)
                    {
                        first = ele.Segments[0] as StraightSegment;
                        segment = !DiagramPoint.IsEmptyPoint(first?.Point) ? first : null;
                    }
                    DiagramPoint tarPoint = (segment != null) ? segment.Point : target.Point;
                    if (ele.Segments != null && ele.Type == ConnectorSegmentType.Bezier && ele.Segments.Count > 0 && ((BezierSegment)ele.Segments[0]).Vector1.Angle != 0 && ((BezierSegment)ele.Segments[0]).Vector1.Distance != 0)
                    {
                        double value = Math.Max(srcCorners.Width, srcCorners.Height);
                        tarPoint = DiagramPoint.Transform(source.Point, ((BezierSegment)ele.Segments[0]).Vector1.Angle, value / 2);
                    }
                    source.Point = isInterSect ? ele.SourceWrapper.Bounds.Center : GetIntersection(ele, source.Point, tarPoint, false);
                }
                if (ele.TargetPortWrapper == null)
                {
                    target.Point = tarCorners.Center;
                    if (ele.Segments != null && ele.Segments.Count > 1)
                    {
                        first = ele.Segments[^2] as StraightSegment;
                        segment = (!DiagramPoint.IsEmptyPoint(first?.Point)) ? first : null;
                    }
                    DiagramPoint srcPoint = (segment != null) ? segment.Point : source.Point;
                    if (ele.Type == ConnectorSegmentType.Bezier && ele.Segments.Count > 0 &&
                        (ele.Segments[^1] as BezierSegment).Vector2.Angle != 0 &&
                        (ele.Segments[^1] as BezierSegment).Vector2.Distance != 0)
                    {
                        double value = Math.Max(srcCorners.Width, srcCorners.Height);
                        srcPoint = DiagramPoint.Transform(target.Point, (ele.Segments[0] as BezierSegment).Vector2.Angle, value / 2);
                    }
                    target.Point = isInterSect ? ele.TargetWrapper.Bounds.Center : GetIntersection(ele, srcPoint, target.Point, true);
                }
                intermediatePoints = IntermediatePointsForStraight(ele, source, target);
            }
            else
            {
                if (ele.Type == ConnectorSegmentType.Orthogonal && (ele.Segments != null && ele.Segments.Count > 0) &&
                    ((OrthogonalSegment)ele.Segments[0]).Direction != null)
                {
                    intermediatePoints = FindIntermediatePoints(ele, source, target, srcPort, tarPort, sourceEle);
                }
                else
                {
                    if (ele.Segments == null || ele.Segments.Count == 0)
                    {
                        OrthogonalSegment segment = new OrthogonalSegment { Type = ConnectorSegmentType.Orthogonal };
                        ele.Segments.Add(segment);
                    }
                    (ele.Segments[0] as OrthogonalSegment).Points = intermediatePoints = FindOrthoSegments(ele, source, target, null);
                }
            }
            return intermediatePoints;
        }
        internal static List<DiagramPoint> IntermediatePointsForStraight(Connector element, End source, End target)
        {
            List<DiagramPoint> intermediatePoints = new List<DiagramPoint>();
            if (element.Segments != null && element.Segments.Count > 0)
            {
                int i;
                DiagramPoint srcPoint = source.Point;
                for (i = 0; i < element.Segments.Count; i++)
                {
                    if (element.Segments[i] is StraightSegment seg)
                    {
                        List<DiagramPoint> segPoint = new List<DiagramPoint>
                        {
                            srcPoint
                        };
                        if (i != element.Segments.Count - 1)
                        {
                            segPoint.Add(seg.Point); srcPoint = seg.Point;
                        }
                        else
                        {
                            segPoint.Add(target.Point);
                        }
                        element.Segments[i].Points = segPoint;
                        if (element.Segments.Count > 1 && DiagramPoint.Equals(seg.Points[0], seg.Points[1]))
                        {
                            element.Segments.RemoveAt(i);
                        }

                        for (int j = 0; j < seg.Points.Count; j++)
                        {
                            if (j > 0 || i == 0)
                            {
                                intermediatePoints.Add(seg.Points[j]);
                            }
                        }
                    }
                }
            }
            return intermediatePoints;
        }

        internal static Direction FindSourceDirection(Direction dir, DiagramPoint srcPoint, DiagramPoint tarPoint)
        {
            Direction direction = (dir == Direction.Top || dir == Direction.Bottom) ?
                ((tarPoint.X > srcPoint.X) ? Direction.Right : Direction.Left) :
                ((tarPoint.Y > srcPoint.Y) ? Direction.Bottom : Direction.Top);
            return direction;
        }

        internal static void CheckLastSegmentAsTerminal(Connector ele)
        {
            if (ele.Type == ConnectorSegmentType.Straight || ele.Type == ConnectorSegmentType.Bezier)
            {
                if (ele.Segments.Count == 0 || (ele.Segments.Any() && ele.Segments[^1] is StraightSegment straghtSegment &&
                    (!DiagramPoint.IsEmptyPoint(straghtSegment.Point))))
                {
                    ele.Segments.Add(ele.Type == ConnectorSegmentType.Bezier
                        ? new BezierSegment { Type = ConnectorSegmentType.Bezier }
                        : new StraightSegment { Type = ConnectorSegmentType.Straight });
                }
            }
            else
            {
                if (ele.Segments.Count == 0 || ((OrthogonalSegment)ele.Segments[^1]).Direction.HasValue)
                {
                    OrthogonalSegment segment = new OrthogonalSegment { Type = ConnectorSegmentType.Orthogonal };
                    ele.Segments.Add(segment);
                }
            }
        }

        internal static int CheckConsectiveSegmentAsSame(Connector ele, int i, End source)
        {
            OrthogonalSegment seg = ele.Segments[i] as OrthogonalSegment;
            if (seg.Length != null)
            {
                double extra = (seg.Direction == Direction.Left || seg.Direction == Direction.Top) ? -(seg.Length.Value) : seg.Length.Value;
                double angle = (seg.Direction == Direction.Left || seg.Direction == Direction.Right) ? 0 : 90;
                DiagramPoint segPoint = AddLineSegment(source.Point, extra, angle);
                ((OrthogonalSegment)ele.Segments[i - 1]).Length += seg.Length;
                ((OrthogonalSegment)ele.Segments[i - 1]).Points[1] = source.Point = segPoint;
            }

            ele.Segments.RemoveAt(i); i--;
            return i;
        }

        internal static List<DiagramPoint> NodeOrPortToNode(Connector ele, End source, End target)
        {
            List<DiagramPoint> point; Direction? portdirection = null;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double srcPointY = source.Point.Y; double srcPointX = source.Point.X; double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            if (ele.SourcePortWrapper != null)
            {
                DiagramPoint port = new DiagramPoint { X = ele.SourcePortWrapper.OffsetX, Y = ele.SourcePortWrapper.OffsetY };
                portdirection = GetPortDirection(port, BaseUtil.CornersPointsBeforeRotation(ele.SourceWrapper), ele.SourceWrapper.Bounds);
            }
            FindDirection(ele.TargetWrapper, source, target, ele);
            Direction direction = FindSourceDirection(target.Direction.Value, source.Point, target.Point);
            if (ele.SourcePortWrapper != null && source.Direction == target.Direction &&
                ((source.Direction == Direction.Top || source.Direction == Direction.Bottom) && (srcCorners.Center.X.Equals(tarCorners.Center.X))
                    || (source.Direction == Direction.Left || source.Direction == Direction.Right) && (srcCorners.Center.Y.Equals(tarCorners.Center.Y))))
            {
                source.Direction = direction;
                point = (direction == Direction.Top || direction == Direction.Bottom) ?
                    OrthoConnection3Segment(ele, source, target, ele.SourceWrapper.Height / 2 + 20) :
                    OrthoConnection3Segment(ele, source, target, ele.SourceWrapper.Width / 2 + 20);
                End source1 = source; source1.Point = point[1];
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    target.Direction = direction;
                    target.Point = (direction == Direction.Left) ? tarCorners.MiddleLeft : tarCorners.MiddleRight;
                }
                else
                {
                    FindDirection(ele.TargetWrapper, source, target, ele);
                }
                point = OrthoConnection3Segment(ele, source, target);
            }
            else if (tarPointX >= srcCorners.Left && tarPointX <= srcCorners.Right &&
              srcPointY >= srcCorners.Top && srcPointY <= srcCorners.Bottom)
            {
                source.Direction = (tarPointY > srcPointY) ? Direction.Bottom : Direction.Top;
                double length = (source.Direction == Direction.Top) ? (srcPointY - srcCorners.Top + 20) :
                        (srcCorners.Bottom - srcPointY + 20);
                point = OrthoConnection3Segment(ele, source, target, length);
            }
            else if (ele.SourcePortWrapper != null && portdirection == GetOppositeDirection(direction))
            {
                double length = 0;
                if ((portdirection == Direction.Left || portdirection == Direction.Right) && srcPointY >= srcCorners.Top
                    && srcPointY <= srcCorners.Bottom)
                {
                    source.Direction = (tarPointY > srcPointY) ? Direction.Bottom : Direction.Top;
                    length = srcCorners.Height / 2 + 20;
                }
                else if ((portdirection == Direction.Top || portdirection == Direction.Bottom) && srcPointX >= srcCorners.Left
                  && srcPointX <= srcCorners.Right)
                {
                    source.Direction = (tarPointX > srcPointX) ? Direction.Right : Direction.Left;
                    length = srcCorners.Width / 2 + 20;
                }
                if (source.Direction.HasValue && length != 0)
                {
                    point = OrthoConnection3Segment(ele, source, target, length, true);
                }
                else
                {
                    source.Direction = direction;
                    point = OrthoConnection2Segment(source, target);
                }
            }
            else if (ele.SourcePortWrapper != null && portdirection == target.Direction && (portdirection == Direction.Top || portdirection == Direction.Bottom) &&
              (srcCorners.Center.X.Equals(tarCorners.Center.X)))
            {
                source.Direction = (tarPointY > srcPointY) ? Direction.Bottom : Direction.Top;
                double len = (source.Direction == Direction.Bottom) ? (srcCorners.Bottom - srcPointY + 20) :
                        (srcPointY - srcCorners.Top + 20);
                point = OrthoConnection3Segment(ele, source, target, len);
            }
            else
            {
                source.Direction = direction;
                point = OrthoConnection2Segment(source, target);
            }
            return point;
        }

        internal static void CheckSourcePointInTarget(Connector ele, End source)
        {
            if (ele.TargetWrapper != null && ele.TargetPortWrapper == null)
            {
                double padding = 1;
                if (BaseUtil.CornersPointsBeforeRotation(ele.TargetWrapper).ContainsPoint(source.Point, padding))
                {
                    DiagramElement target = ele.TargetWrapper; Corners tarCorners = target.Corners;
                    OrthogonalSegment segment = ele.Segments[^2] as OrthogonalSegment;
                    DiagramPoint lastPoint = segment.Points[^1];
                    Direction direction = GetOppositeDirection(segment.Direction.Value).Value;
                    if (direction == Direction.Bottom)
                    {
                        if (lastPoint.Y < tarCorners.Bottom + padding)
                        {
                            segment.Points[^1].Y = tarCorners.Bottom + 20;
                            segment.Length = DiagramPoint.DistancePoints(segment.Points[0], segment.Points[^1]);
                        }
                    }
                    else if (direction == Direction.Top)
                    {
                        if (lastPoint.Y > tarCorners.Top - padding)
                        {
                            segment.Points[^1].Y = tarCorners.Top - 20;
                            segment.Length = DiagramPoint.DistancePoints(segment.Points[0], segment.Points[^1]);
                        }
                    }
                    else if (direction == Direction.Left)
                    {
                        if (lastPoint.X > tarCorners.Left - padding)
                        {
                            segment.Points[^1].X = tarCorners.Left - 20;
                            segment.Length = DiagramPoint.DistancePoints(segment.Points[0], segment.Points[^1]);
                        }
                    }
                    else if (direction == Direction.Right)
                    {
                        if (lastPoint.X < tarCorners.Right + padding)
                        {
                            segment.Points[^1].X = tarCorners.Right + 20;
                            segment.Length = DiagramPoint.DistancePoints(segment.Points[0], segment.Points[^1]);
                        }
                    }
                    source.Point = segment.Points[^1];
                }
            }
        }

        internal static List<DiagramPoint> FindIntermediatePoints(Connector ele, End source, End target, DiagramElement srcPort, DiagramElement tarPort, DiagramElement sourceEle)
        {
            List<DiagramPoint> intermediatePoints = new List<DiagramPoint>();
            int j; int? removeSegment = null;
            CheckLastSegmentAsTerminal(ele);
            for (int i = 0; i < ele.Segments.Count; i++)
            {
                OrthogonalSegment seg = ele.Segments[i] as OrthogonalSegment;
                if (srcPort != null && seg.Direction.HasValue && source.Direction.Value == GetOppositeDirection(seg.Direction.Value))
                {
                    seg.Direction = source.Direction;
                }
                if (i > 0 && seg.Direction.HasValue && (ele.Segments[i - 1] as OrthogonalSegment).Direction == seg.Direction)
                {
                    i = CheckConsectiveSegmentAsSame(ele, i, source);
                }
                else
                {
                    if (seg.Direction.HasValue)
                    {
                        source.Point = UpdateSegmentPoints(source, (ele.Segments[i] as OrthogonalSegment));
                    }
                    else
                    {
                        OrthogonalSegment segment = ele.Segments[i - 1] as OrthogonalSegment;
                        source.Point = segment.Points[^1];
                    }
                }
                if (i == ele.Segments.Count - 1)
                {
                    CheckSourcePointInTarget(ele, source);
                    List<DiagramPoint> point;
                    point = tarPort == null ? NodeOrPortToNode(ele, source, target) : PointToPort(ele, source, target);
                    CheckPreviousSegment(point, ele);
                    seg.Points = new List<DiagramPoint>();
                    if (point.Count >= 2)
                    {
                        for (j = 0; j < point.Count; j++)
                        {
                            seg.Points.Add(point[j]);
                        }
                    }
                    else { removeSegment = i; }
                }
                if (removeSegment.HasValue)
                {
                    if (removeSegment == ele.Segments.Count - 1)
                    {
                        (ele.Segments[removeSegment.Value - 1] as OrthogonalSegment).Direction = null;
                        (ele.Segments[removeSegment.Value - 1] as OrthogonalSegment).Length = null;
                    }
                    ele.Segments.RemoveAt(removeSegment.Value);
                }
                if (srcPort != null && i == 0)
                {
                    DiagramPoint sourcePoint = CheckPortDirection(ele, srcPort, sourceEle);
                    if (sourcePoint != null)
                    {
                        source.Point = sourcePoint;
                    }
                }
            }
            return ReturnIntermediatePoints(ele, intermediatePoints);
        }

        internal static List<DiagramPoint> ReturnIntermediatePoints(Connector element, List<DiagramPoint> intermediatePoints)
        {
            int j;
            for (int i = 0; i < element.Segments.Count; i++)
            {
                OrthogonalSegment seg = element.Segments[i] as OrthogonalSegment;
                for (j = 0; j < seg.Points.Count; j++)
                {
                    if (j > 0 || i == 0)
                    {
                        intermediatePoints.Add(seg.Points[j]);
                    }
                }
            }
            return intermediatePoints;
        }

        internal static void FindDirection(DiagramElement node, End source, End target, Connector ele)
        {
            Direction nodeDirection; DiagramPoint nodeConnectingPoint;
            Corners nodeCorners = SwapBounds(node, node.Corners, node.Bounds);
            Margin nodeMargin = source.Margin;
            DiagramPoint fixedPoint = source.Point;
            if (nodeCorners.BottomCenter.Y + nodeMargin.Bottom < fixedPoint.Y)
            {
                nodeDirection = Direction.Bottom;
                nodeConnectingPoint = nodeCorners.BottomCenter;
            }
            else if (nodeCorners.TopCenter.Y - nodeMargin.Top > fixedPoint.Y)
            {
                nodeDirection = Direction.Top;
                nodeConnectingPoint = nodeCorners.TopCenter;
            }
            else if (nodeCorners.MiddleLeft.X - nodeMargin.Left > fixedPoint.X)
            {
                nodeDirection = Direction.Left;
                nodeConnectingPoint = nodeCorners.MiddleLeft;
            }
            else if (nodeCorners.MiddleRight.X + nodeMargin.Right < fixedPoint.X)
            {
                nodeDirection = Direction.Right;
                nodeConnectingPoint = nodeCorners.MiddleRight;
            }
            else
            {
                double top = Math.Abs(fixedPoint.Y - nodeCorners.TopCenter.Y);
                double right = Math.Abs(fixedPoint.X - nodeCorners.MiddleRight.X);
                double bottom = Math.Abs(fixedPoint.Y - nodeCorners.BottomCenter.Y);
                double left = Math.Abs(fixedPoint.X - nodeCorners.MiddleLeft.X);
                double shorts = top;

                nodeDirection = Direction.Top;
                nodeConnectingPoint = nodeCorners.TopCenter;

                if (shorts > right)
                {
                    shorts = right;
                    nodeDirection = Direction.Right;
                    nodeConnectingPoint = nodeCorners.MiddleRight;
                }
                if (shorts > bottom)
                {
                    shorts = bottom;
                    nodeDirection = Direction.Bottom;
                    nodeConnectingPoint = nodeCorners.BottomCenter;
                }
                if (shorts > left)
                {
                    //shorts = left;
                    nodeDirection = Direction.Left;
                    nodeConnectingPoint = nodeCorners.MiddleLeft;
                }
            }
            target.Point = nodeConnectingPoint;
            target.Direction = nodeDirection;
            DiagramPoint refPoint = FindPoint(nodeCorners, GetOppositeDirection(target.Direction.Value).Value);
            target.Point = GetIntersection(ele, target.Point, refPoint, node == ele.TargetWrapper);
        }


        internal static List<DiagramPoint> FindOrthoSegments(Connector ele, End source, End target, double? extra)
        {
            NoOfSegments? seg = null;
            bool swap = target.Direction != null && source.Direction != null && GetSwapping(source.Direction.Value, target.Direction.Value);
            if (swap) { SwapPoints(source, target); }
            if (source.Direction == Direction.Right && target.Direction == Direction.Left)
            {
                seg = GetRightToLeftSegmentCount(ele, source, target, swap);
            }
            else if (source.Direction == Direction.Right && target.Direction == Direction.Right)
            {
                seg = GetRightToRightSegmentCount(ele, source, target);
            }
            else if (source.Direction == Direction.Right && target.Direction == Direction.Top)
            {
                seg = GetRightToTopSegmentCount(ele, source, target, swap);
            }
            else if (source.Direction == Direction.Right && target.Direction == Direction.Bottom)
            {
                seg = GetRightToBottomSegmentCount(ele, source, target, swap);
            }
            else if (source.Direction == Direction.Bottom && target.Direction == Direction.Top)
            {
                seg = GetBottomToTopSegmentCount(source, target);
            }
            else if (source.Direction == Direction.Bottom && target.Direction == Direction.Bottom)
            {
                source.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
                target.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
                seg = GetBottomToBottomSegmentCount(ele, source, target);
            }
            else if (source.Direction == Direction.Bottom && target.Direction == Direction.Left)
            {
                seg = GetBottomToLeftSegmentCount(ele, source, target, swap);
            }
            else if (source.Direction == Direction.Left && target.Direction == Direction.Left)
            {
                seg = GetLeftToLeftSegmentCount(ele, source, target);
            }
            else if (source.Direction == Direction.Left && target.Direction == Direction.Top)
            {
                seg = GetLeftToTopSegmentCount(ele, source, target, swap);
            }
            else if (source.Direction == Direction.Top && target.Direction == Direction.Top)
            {
                seg = GetTopToTopSegmentCount(ele, source, target);
            }
            if (swap) { SwapPoints(source, target); }
            List<DiagramPoint> intermediatePoints = AddOrthoSegments(ele, seg.Value, source, target, extra);
            return intermediatePoints;
        }

        internal static double FindAngle(DiagramPoint s, DiagramPoint e)
        {
            DiagramPoint r = new DiagramPoint { X = e.X, Y = s.Y };
            double re = DiagramPoint.FindLength(r, e);
            double es = DiagramPoint.FindLength(e, s);
            double ang = Math.Asin(re / es);
            ang = ang * 180 / Math.PI;
            if (s.X < e.X)
            {
                if (s.Y > e.Y)
                {
                    ang = 360 - ang;
                }
            }
            else
            {
                if (s.Y < e.Y)
                {
                    ang = 180 - ang;
                }
                else
                {
                    ang = 180 + ang;
                }
            }
            return ang;
        }

        internal static DiagramPoint FindPoint(Corners cor, Direction direction)
        {
            DiagramPoint point = null;
            switch (direction)
            {
                case Direction.Left:
                    point = cor.MiddleLeft;
                    break;
                case Direction.Top:
                    point = cor.TopCenter;
                    break;
                case Direction.Right:
                    point = cor.MiddleRight;
                    break;
                case Direction.Bottom:
                    point = cor.BottomCenter;
                    break;
            }
            return point;
        }

        internal static void PointsFromNodeToPoint(OrthogonalSegment seg, Direction direction, DiagramRect bounds, DiagramPoint point, DiagramPoint endPoint, bool isTarget)
        {
            double minSpace = 13; double x;
            List<DiagramPoint> points = new List<DiagramPoint>(); double y;
            points.Add(point);
            bool straight = (point.Y.Equals(endPoint.Y) && (direction == Direction.Left && endPoint.X < point.X ||
                                                            direction == Direction.Right && endPoint.X > point.X)) ||
                            (point.X.Equals(endPoint.X) && (direction == Direction.Top && endPoint.Y < point.Y ||
                                                            direction == Direction.Bottom && endPoint.Y > point.Y));
            if (!straight)
            {
                if (direction == Direction.Top || direction == Direction.Bottom)
                {
                    if (direction == Direction.Top && endPoint.Y < point.Y && endPoint.Y > point.Y - minSpace ||
                        direction == Direction.Bottom && endPoint.Y > point.Y && endPoint.Y < point.Y + minSpace)
                    {
                        y = direction == Direction.Top ? bounds.Top - minSpace : bounds.Bottom + minSpace;
                        points.Add(new DiagramPoint { X = point.X, Y = y });
                        points.Add(new DiagramPoint { X = point.X + (endPoint.X - point.X) / 2, Y = y });
                        points.Add(new DiagramPoint { X = point.X + (endPoint.X - point.X) / 2, Y = endPoint.Y });
                    }
                    else if (Math.Abs(point.X - endPoint.X) > minSpace &&
                      (direction == Direction.Top && endPoint.Y < point.Y || direction == Direction.Bottom && endPoint.Y > point.Y))
                    {
                        //two segments
                        points.Add(new DiagramPoint { X = point.X, Y = endPoint.Y });
                    }
                    else
                    {
                        y = direction == Direction.Top ? bounds.Top - minSpace : bounds.Bottom + minSpace;
                        points.Add(new DiagramPoint { X = point.X, Y = y });
                        points.Add(new DiagramPoint { X = endPoint.X, Y = y });
                    }
                }
                else
                {
                    if (direction == Direction.Left && endPoint.X < point.X && endPoint.X > point.X - minSpace || direction == Direction.Right &&
                        endPoint.X > point.X && endPoint.X < point.X + minSpace)
                    {
                        x = direction == Direction.Left ? bounds.Left - minSpace : bounds.Right + minSpace;
                        points.Add(new DiagramPoint { X = x, Y = point.Y });
                        points.Add(new DiagramPoint { X = x, Y = point.Y + (endPoint.Y - point.Y) / 2 });
                        points.Add(new DiagramPoint { X = endPoint.X, Y = point.Y + (endPoint.Y - point.Y) / 2 });
                    }
                    else if (Math.Abs(point.Y - endPoint.Y) > minSpace &&
                      (direction == Direction.Left && endPoint.X < point.X || direction == Direction.Right && endPoint.X > point.X))
                    {
                        points.Add(new DiagramPoint { X = endPoint.X, Y = point.Y });
                        //two segments
                    }
                    else
                    {
                        x = direction == Direction.Left ? bounds.Left - minSpace : bounds.Right + minSpace;
                        points.Add(new DiagramPoint { X = x, Y = point.Y });
                        points.Add(new DiagramPoint { X = x, Y = endPoint.Y });
                    }
                }
                if (isTarget)
                {
                    points.Add(seg.Points[0]);
                    points.Reverse();
                }
                seg.Points = points;
            }
        }

        internal static DiagramPoint AddLineSegment(DiagramPoint point, double extra, double angle)
        {
            return DiagramPoint.Transform(point, angle, extra);
        }

        internal static DiagramPoint GetIntersection(Connector ele, DiagramPoint sPt, DiagramPoint tPt, bool isTar)
        {
            sPt = new DiagramPoint { X = sPt.X, Y = sPt.Y };
            tPt = new DiagramPoint { X = tPt.X, Y = tPt.Y };
            DiagramElement wrapper = isTar ? ele.TargetWrapper : ele.SourceWrapper;
            double padding = isTar ? ele.TargetPadding : ele.SourcePadding;
            DiagramRect rect = null;
            List<DiagramPoint> segmentPoints;
            DiagramPoint point = isTar || ele.Type == ConnectorSegmentType.Orthogonal ? sPt : tPt;
            PathElement child = wrapper as PathElement;
            DiagramPoint sPt1 = BaseUtil.RotatePoint(-wrapper.ParentTransform, wrapper.OffsetX, wrapper.OffsetY, sPt);
            DiagramPoint tPt1 = BaseUtil.RotatePoint(-wrapper.ParentTransform, wrapper.OffsetX, wrapper.OffsetY, tPt);
            if (ele.Type == ConnectorSegmentType.Orthogonal)
            {
                double constValue = 5;
                if (sPt1.X.Equals(tPt1.X))
                {
                    if (sPt1.Y < tPt1.Y)
                    {
                        sPt1.Y -= constValue;
                    }
                    else
                    {
                        sPt1.Y += constValue;
                    }
                }
                if (sPt1.Y.Equals(tPt1.Y))
                {
                    if (sPt1.X < tPt1.X)
                    {
                        sPt1.X -= constValue;
                    }
                    else
                    {
                        sPt1.X += constValue;
                    }
                }
                sPt = BaseUtil.RotatePoint(wrapper.ParentTransform, wrapper.OffsetX, wrapper.OffsetY, sPt1);
            }
            else
            {
                double angle;
                if (isTar)
                {
                    angle = DiagramPoint.FindAngle(sPt, tPt);
                    tPt = DiagramPoint.Transform(new DiagramPoint { X = tPt.X, Y = tPt.Y }, angle, Math.Max(BaseUtil.GetDoubleValue(wrapper.ActualSize.Width), BaseUtil.GetDoubleValue(wrapper.ActualSize.Height)));
                }
                else
                {
                    angle = DiagramPoint.FindAngle(tPt, sPt);
                    sPt = DiagramPoint.Transform(new DiagramPoint { X = sPt.X, Y = sPt.Y }, angle, Math.Max(BaseUtil.GetDoubleValue(wrapper.ActualSize.Width), BaseUtil.GetDoubleValue(wrapper.ActualSize.Height)));
                }
            }
            if (ele.SourcePadding != 0 || ele.TargetPadding != 0)
            {
                rect = new DiagramRect
                {
                    X = wrapper.Bounds.X - padding,
                    Y = wrapper.Bounds.Y - padding,
                    Width = BaseUtil.GetDoubleValue(wrapper.ActualSize.Width) + 2 * padding,
                    Height = BaseUtil.GetDoubleValue(wrapper.ActualSize.Height) + 2 * padding
                };
            }
            if (wrapper.GetType() == typeof(PathElement) && !string.IsNullOrEmpty((wrapper as PathElement).Data))
            {
                segmentPoints = rect != null ? new List<DiagramPoint> { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft } : child.GetPoints();
                if (((child.Data.Split("m").Length - 1) + (child.Data.Split("M").Length - 1)) == 1)
                {
                    segmentPoints.Add(segmentPoints[0]);
                }
            }
            else
            {
                segmentPoints = rect != null ? new List<DiagramPoint> { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft } : DiagramUtil.GetPoints(wrapper.Corners);
                segmentPoints.Add(segmentPoints[0]);
            }
            Segment thisSegment = new Segment { X1 = sPt.X, Y1 = sPt.Y, X2 = tPt.X, Y2 = tPt.Y };
            DiagramPoint newPoint = GetIntersectionPoints(thisSegment, segmentPoints, point);
            return newPoint ?? sPt;
        }


        internal static DiagramPoint SetLineEndPoint(Connector element, DiagramPoint point, bool isTarget)
        {
            point.X = Math.Round(point.X * 100) / 100;
            point.Y = Math.Round(point.Y * 100) / 100;
            if (isTarget)
            {
                element.TargetPoint = point;
            }
            else
            {
                element.SourcePoint = point;
            }
            return point;
        }

        internal static DiagramPoint GetIntersectionPoints(Segment thisSegment, List<DiagramPoint> pts, DiagramPoint point)
        {
            double length = pts.Count; double min = Double.NaN;
            Segment segment = new Segment
            {
                X1 = pts[0].X,
                Y1 = pts[0].Y,
                X2 = pts[1].X,
                Y2 = pts[1].Y
            };
            DiagramPoint intersection = IntersectSegment(thisSegment, segment);
            if (intersection != null)
            {
                min = DiagramPoint.DistancePoints(intersection, point);
            }
            if (double.IsNaN(min) || min > 0)
            {
                for (int i = 1; i < length - 1; i++)
                {
                    segment = new Segment
                    {
                        X1 = pts[i].X,
                        Y1 = pts[i].Y,
                        X2 = pts[i + 1].X,
                        Y2 = pts[i + 1].Y
                    };
                    DiagramPoint intersect = IntersectSegment(thisSegment, segment);
                    if (intersect != null)
                    {
                        double dist = DiagramPoint.DistancePoints(intersect, point);
                        if (double.IsNaN(min) || min > dist) { min = dist; intersection = intersect; }
                        if (min >= 0 && min <= 1)
                        {
                            break;
                        }
                    }
                }
            }
            return intersection;
        }

        internal static DiagramPoint IntersectSegment(Segment segment1, Segment segment2)
        {
            double x1 = segment1.X1; double y1 = segment1.Y1;
            double x2 = segment1.X2; double y2 = segment1.Y2;
            double x3 = segment2.X1; double y3 = segment2.Y1;
            double x4 = segment2.X2; double y4 = segment2.Y2;
            double x; double y;
            double a1 = y2 - y1; double b1 = x1 - x2; double c1 = (x2 * y1) - (x1 * y2);
            double r3 = (a1 * x3) + (b1 * y3) + c1; double r4 = (a1 * x4) + (b1 * y4) + c1;
            if ((r3 != 0) && (r4 != 0) && SameSign(r3, r4))
            {
                return null;
            }
            double a2 = y4 - y3; double b2 = x3 - x4; double c2 = (x4 * y3) - (x3 * y4);
            double r1 = (a2 * x1) + (b2 * y1) + c2; double r2 = (a2 * x2) + (b2 * y2) + c2;
            if ((r1 != 0) && (r2 != 0) && SameSign(r1, r2))
            {
                return null;
            }
            double denom = (a1 * b2) - (a2 * b1);

            if (denom == 0)
            {
                return null;
            }
            double offset = 0;
            double num = (b1 * c2) - (b2 * c1);
            if (num < 0)
            {
                x = (num - offset) / denom;
            }
            else
            {
                x = (num + offset) / denom;
            }
            num = (a2 * c1) - (a1 * c2);
            if (num < 0)
            {
                y = (num - offset) / denom;
            }
            else
            {
                y = (num + offset) / denom;
            }
            return new DiagramPoint { X = x, Y = y };
        }

        internal static bool SameSign(double a, double b)
        {
            return (a * b) >= 0;
        }

        internal static NoOfSegments GetRightToLeftSegmentCount(Connector element, End source, End target, bool swap)
        {
            DiagramElement srcPort = element.SourcePortWrapper;
            NoOfSegments pts;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            Corners tarCorners = target.Corners; Corners srcCorners = source.Corners;
            double diffY = Math.Round(Math.Abs(srcPointY - tarPointY));
            DiagramPoint right = new DiagramPoint { X = Math.Max(srcPointX, srcCorners.Right), Y = srcPointY };
            DiagramPoint left = new DiagramPoint { X = Math.Min(tarPointX, tarCorners.Left), Y = tarPointY };
            double margin = 10;
            if (swap)
            {
                (left, right) = (right, left);
            }
            if (!(srcCorners.Bottom + margin < tarCorners.Top - margin ||
                srcCorners.Top - margin > tarCorners.Bottom + margin))
            {
                margin = 0;
            }
            source.Margin = new Margin { Left = margin, Right = margin, Top = margin, Bottom = margin };
            target.Margin = new Margin { Left = margin, Right = margin, Top = margin, Bottom = margin };
            if (diffY == 0 && (srcCorners.Right < tarCorners.Left
                || (swap && srcCorners.Right < tarCorners.Left)))
            {
                pts = NoOfSegments.One;
            }
            else if (srcPointX + source.Margin.Right < tarPointX - target.Margin.Left)
            {
                pts = NoOfSegments.Three;
            }
            else if (element.SourceWrapper != element.TargetWrapper &&
              (BaseUtil.CornersPointsBeforeRotation(element.SourceWrapper).ContainsPoint(left) ||
                  BaseUtil.CornersPointsBeforeRotation(element.TargetWrapper).ContainsPoint(right)))
            {
                pts = NoOfSegments.Three;
            }
            else if (srcCorners.Bottom <= tarCorners.Top)
            {
                pts = NoOfSegments.Five;
            }
            else if (srcCorners.Top >= tarCorners.Top)
            {
                pts = NoOfSegments.Five;
            }
            else if ((srcPort != null && srcPort.OffsetY <= tarCorners.Top) ||
              (srcPort == null && srcCorners.Right <= tarCorners.Top))
            {
                pts = NoOfSegments.Five;
            }
            else if ((srcPort != null && srcPort.OffsetY >= tarCorners.Bottom) ||
              (srcPort == null && srcCorners.Right >= tarCorners.Bottom))
            {
                pts = NoOfSegments.Five;
            }
            else
            {
                pts = NoOfSegments.Five;
            }
            return pts;
        }

        internal static NoOfSegments GetRightToRightSegmentCount(Connector element, End sourceObj, End targetObj)
        {
            DiagramElement sourcePort = element.SourcePortWrapper;
            DiagramElement tarPort = element.TargetPortWrapper;
            NoOfSegments pts;
            double diffX = sourceObj.Point.X - targetObj.Point.X;
            double diffY = sourceObj.Point.Y - targetObj.Point.Y;
            targetObj.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            sourceObj.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            if (sourceObj.Corners.Right >= targetObj.Corners.Right)
            {
                if ((sourcePort != null && (sourcePort.OffsetY < targetObj.Corners.Top ||
                    sourcePort.OffsetY > targetObj.Corners.Bottom)) ||
                    (sourcePort == null && sourceObj.Corners.MiddleRight.Y < targetObj.Corners.Top))
                {
                    pts = NoOfSegments.Three;
                }
                else if ((sourcePort != null && sourcePort.OffsetY > targetObj.Corners.Bottom + targetObj.Margin.Bottom
                  && sourceObj.Corners.Top > targetObj.Corners.Bottom) ||
                  (sourcePort == null && sourceObj.Corners.MiddleRight.Y > targetObj.Corners.Bottom))
                {
                    pts = NoOfSegments.Three;
                }
                else if ((sourcePort != null && sourcePort.OffsetY < targetObj.Corners.Top
                  && sourceObj.Corners.Bottom > targetObj.Corners.Top) ||
                  (sourcePort == null && sourceObj.Corners.MiddleRight.Y > targetObj.Corners.Bottom))
                {
                    pts = NoOfSegments.Three;
                }
                else if (sourceObj.Corners.Right < targetObj.Corners.Left ||
                  targetObj.Corners.Right < sourceObj.Corners.Left)
                {
                    pts = NoOfSegments.Five;
                }
                else if (diffX == 0 || diffY == 0)
                {
                    pts = NoOfSegments.One;
                }
                else
                {
                    pts = NoOfSegments.Three;
                }
            }
            else if ((tarPort != null && sourceObj.Corners.Bottom < tarPort.OffsetY) ||
              (tarPort == null && sourceObj.Corners.Bottom < targetObj.Corners.MiddleRight.Y))
            {
                pts = NoOfSegments.Three;
            }
            else if ((tarPort != null && sourceObj.Corners.Top > tarPort.OffsetY) ||
              (tarPort == null && sourceObj.Corners.Top > targetObj.Corners.MiddleRight.Y))
            {
                pts = NoOfSegments.Three;
            }
            else if ((tarPort != null && ((sourcePort != null && sourcePort.OffsetX < targetObj.Corners.Left &&
              !sourcePort.OffsetX.Equals(tarPort.OffsetX) && !sourcePort.OffsetY.Equals(tarPort.OffsetY) &&
              (Math.Abs(sourceObj.Corners.Right - targetObj.Corners.Left) <= 20)) ||
              (sourcePort == null && sourceObj.Corners.Right < targetObj.Corners.Left &&
                  !sourceObj.Corners.Center.X.Equals(targetObj.Corners.Center.X) && !sourceObj.Corners.Center.Y.Equals(targetObj.Corners.Center.Y)))))
            {
                pts = NoOfSegments.Three;
            }
            else if (sourceObj.Corners.Right < targetObj.Corners.Left)
            {
                pts = NoOfSegments.Five;
            }
            else if (diffX == 0 || diffY == 0)
            {
                pts = NoOfSegments.One;
            }
            else
            {
                pts = NoOfSegments.Three;
            }
            return pts;
        }

        internal static NoOfSegments GetRightToTopSegmentCount(Connector element, End source, End target, bool? swap = null)
        {
            DiagramElement tarPort = element.TargetPortWrapper;
            DiagramElement srcPort = element.SourcePortWrapper;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            DiagramPoint right = new DiagramPoint { X = Math.Max(srcPointX, srcCorners.Right), Y = srcPointY };
            DiagramPoint top = new DiagramPoint { X = tarPointX, Y = Math.Min(tarPointY, tarCorners.Top) };
            NoOfSegments pts;
            target.Margin = new Margin { Left = 5, Right = 5, Top = 5, Bottom = 5 };
            source.Margin = new Margin { Top = 5, Bottom = 5, Left = 5, Right = 5 };
            if (swap.HasValue && swap.Value)
            {
                (srcPort, tarPort) = (tarPort, srcPort);
            }
            if ((srcPort != null && srcPort.OffsetY < tarCorners.Top - target.Margin.Top) ||
                (srcPort == null && srcCorners.Bottom < tarCorners.Top - target.Margin.Top))
            {
                if (srcCorners.Bottom < tarCorners.Top)
                {
                    if ((tarPort != null && srcCorners.Right + source.Margin.Right < tarPort.OffsetX) ||
                        (tarPort == null && srcCorners.Right + source.Margin.Right < tarCorners.TopCenter.X))
                    {
                        pts = NoOfSegments.Two;
                    }
                    else
                    {
                        pts = NoOfSegments.Four;
                    }
                }
                else if ((tarPort != null && srcCorners.Left > tarPort.OffsetX) ||
                  (tarPort == null && srcCorners.Left > tarCorners.TopCenter.X))
                {
                    pts = NoOfSegments.Four;
                }
                else
                {
                    pts = NoOfSegments.Two;
                }
            }
            else if (srcPort != null && Math.Abs(srcCorners.Right - tarCorners.Left) <= 25 &&
              Math.Abs(srcPort.OffsetY - tarCorners.Top) <= 25)
            {
                pts = NoOfSegments.Two;
            }
            else if (tarPort != null && Math.Abs(tarPort.OffsetX - srcCorners.TopCenter.X) >= 25 &&
              srcCorners.MiddleRight.Y < tarPort.OffsetY)
            {
                pts = NoOfSegments.Two;
            }
            else if (srcCorners.Right < tarCorners.Left)
            {
                pts = NoOfSegments.Four;
            }
            else if (element.SourceWrapper != element.TargetWrapper &&
              (BaseUtil.CornersPointsBeforeRotation(element.SourceWrapper).ContainsPoint(top) ||
                  BaseUtil.CornersPointsBeforeRotation(element.TargetWrapper).ContainsPoint(right)))
            {
                pts = NoOfSegments.Two;
            }
            else
            {
                pts = NoOfSegments.Four;
            }
            return pts;
        }

        internal static NoOfSegments GetRightToBottomSegmentCount(Connector element, End source, End target, bool? swap = null)
        {
            source.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            target.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            NoOfSegments pts;
            DiagramElement srcPort = element.SourcePortWrapper;
            DiagramElement tarPort = element.TargetPortWrapper;

            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            if (swap.HasValue && swap.Value)
            {
                (srcPort, tarPort) = (tarPort, srcPort);
            }
            if ((srcPort != null && srcPort.OffsetY > tarCorners.Bottom + target.Margin.Bottom) ||
                (srcPort == null && srcCorners.MiddleRight.Y > tarCorners.Bottom + target.Margin.Bottom))
            {
                if (srcCorners.Top > tarCorners.Bottom)
                {
                    if ((tarPort != null && srcCorners.Right + source.Margin.Right < tarPort.OffsetX) ||
                        (tarPort == null && srcCorners.Right + source.Margin.Right < tarCorners.BottomCenter.X))
                    {
                        pts = NoOfSegments.Two;
                    }
                    else
                    {
                        pts = NoOfSegments.Four;
                    }
                }
                else if ((tarPort != null && srcCorners.Left > tarPort.OffsetX) ||
                  (tarPort == null && srcCorners.Left > tarCorners.BottomCenter.X))
                {
                    pts = NoOfSegments.Four;
                }
                else
                {
                    pts = NoOfSegments.Two;
                }
            }
            else if (srcPort != null &&
              Math.Abs(srcCorners.Right - tarCorners.Left) <= 25 &&
              Math.Abs(srcPort.OffsetY - tarCorners.Bottom) <= 25)
            {
                pts = NoOfSegments.Two;
            }
            else if (srcCorners.Right < tarCorners.Left)
            {
                pts = NoOfSegments.Four;
            }
            else
            {
                pts = NoOfSegments.Four;
            }
            return pts;
        }

        internal static NoOfSegments GetBottomToTopSegmentCount(End source, End target)
        {
            NoOfSegments pts;
            double srcPointX = source.Point.X;
            double tarPointX = target.Point.X;
            double diffX = srcPointX - tarPointX;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double margin = 10;
            if (!(srcCorners.Right + margin < tarCorners.Left - margin ||
                srcCorners.Left - margin > tarCorners.Right + margin))
            {
                margin = 0;
            }
            source.Margin = new Margin { Left = margin, Right = margin, Top = margin, Bottom = margin };
            target.Margin = new Margin { Left = margin, Right = margin, Top = margin, Bottom = margin };
            if (diffX == 0 && srcCorners.Bottom < tarCorners.Top)
            {
                pts = NoOfSegments.One;
            }
            else if (srcCorners.Bottom + source.Margin.Bottom < tarCorners.Top - target.Margin.Top)
            {
                pts = NoOfSegments.Three;
            }
            else if (srcCorners.Right + source.Margin.Right < tarCorners.Left - target.Margin.Left)
            {
                pts = NoOfSegments.Five;
            }
            else if (srcCorners.Left - source.Margin.Left > tarCorners.Right + target.Margin.Right)
            {
                pts = NoOfSegments.Five;
            }
            else
            {
                pts = NoOfSegments.Five;
            }
            return pts;
        }

        internal static NoOfSegments GetBottomToLeftSegmentCount(Connector element, End source, End target, bool? swap)
        {
            DiagramElement srcPort = element.SourcePortWrapper;
            DiagramElement tarPort = element.TargetPortWrapper;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            NoOfSegments pts;
            if (swap.HasValue && swap.Value)
            {
                (srcPort, tarPort) = (tarPort, srcPort);
            }
            if ((srcPort != null && srcPort.OffsetX < tarCorners.Left - target.Margin.Left) ||
                (srcPort == null && srcCorners.BottomCenter.X < tarCorners.BottomLeft.X - target.Margin.Left))
            {
                if (srcCorners.Right < tarCorners.Left)
                {
                    if ((tarPort != null && srcCorners.Bottom + source.Margin.Bottom < tarPort.OffsetY) ||
                        (tarPort == null && srcCorners.Bottom + source.Margin.Bottom < tarCorners.MiddleLeft.Y))
                    {
                        pts = NoOfSegments.Two;
                    }
                    else
                    {
                        pts = NoOfSegments.Four;
                    }
                }
                else if ((tarPort != null && srcCorners.Top > tarPort.OffsetY) ||
                  (tarPort == null && srcCorners.Top > tarCorners.MiddleLeft.Y))
                {
                    pts = NoOfSegments.Four;
                }
                else
                {
                    pts = NoOfSegments.Two;
                }
            }
            else if (tarPort != null &&
              Math.Abs(srcCorners.Right - tarCorners.Left) <= 25 &&
              Math.Abs(tarPort.OffsetY - srcCorners.Bottom) <= 25)
            {
                pts = NoOfSegments.Two;
            }
            else
            {
                pts = NoOfSegments.Four;
            }
            return pts;
        }

        internal static NoOfSegments GetBottomToBottomSegmentCount(Connector element, End source, End target)
        {
            DiagramElement srcPort = element.SourcePortWrapper;
            DiagramElement tarPort = element.TargetPortWrapper;
            double difX = Math.Round(Math.Abs(source.Point.X - target.Point.X));
            double diffY = Math.Round(Math.Abs(source.Point.Y - target.Point.Y));
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            NoOfSegments pts;
            if (srcCorners.Bottom < tarCorners.Bottom)
            {
                if ((srcPort != null && srcPort.OffsetX < tarCorners.Left - target.Margin.Left) ||
                    (srcPort == null && srcCorners.BottomCenter.X < tarCorners.Left - target.Margin.Left))
                {
                    pts = NoOfSegments.Three;
                }
                else if ((srcPort != null && srcPort.OffsetX > tarCorners.Right + target.Margin.Right) ||
                  (srcPort == null && srcCorners.BottomCenter.X > tarCorners.Right + target.Margin.Right))
                {
                    pts = NoOfSegments.Three;
                }
                else if (srcCorners.Bottom < tarCorners.Top)
                {
                    pts = NoOfSegments.Five;
                }
                else if (difX == 0 || diffY == 0)
                {
                    pts = NoOfSegments.One;
                }
                else
                {
                    pts = NoOfSegments.Three;
                }
            }
            else if ((tarPort != null && srcCorners.Left > tarPort.OffsetX) ||
              (tarPort == null && srcCorners.Left > tarCorners.Left))
            {
                pts = NoOfSegments.Three;
            }
            else if ((tarPort != null && srcCorners.Right < tarPort.OffsetX) ||
              (tarPort == null &&
                  srcCorners.Right < tarCorners.Right))
            {
                pts = NoOfSegments.Three;
            }
            else if (srcCorners.Top > tarCorners.Bottom)
            {
                pts = NoOfSegments.Five;
            }
            else if (difX == 0 || diffY == 0)
            {
                pts = NoOfSegments.One;
            }
            else
            {
                pts = NoOfSegments.Three;
            }
            return pts;
        }

        internal static NoOfSegments GetLeftToTopSegmentCount(Connector element, End source, End target, bool? swap)
        {
            NoOfSegments pts;
            DiagramElement sourcePort = element.SourcePortWrapper;
            DiagramElement tarPort = element.TargetPortWrapper;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            DiagramPoint left = new DiagramPoint { X = Math.Min(srcPointX, srcCorners.Left), Y = srcPointY };
            DiagramPoint top = new DiagramPoint { X = tarPointX, Y = Math.Min(tarPointY, tarCorners.Top) };
            if (swap.HasValue && swap.Value)
            {
                (sourcePort, tarPort) = (tarPort, sourcePort);
            }
            if ((sourcePort != null && sourcePort.OffsetY < tarCorners.Top - target.Margin.Top) ||
                (sourcePort == null && (srcCorners.Bottom < tarCorners.Top - target.Margin.Top ||
                    srcCorners.MiddleLeft.Y < tarCorners.Top - target.Margin.Top)))
            {
                if (srcCorners.Bottom < tarCorners.Top)
                {
                    if ((tarPort != null && srcCorners.Left - source.Margin.Left > tarPort.OffsetX) ||
                        (tarPort == null && srcCorners.Left - source.Margin.Left > tarCorners.TopCenter.X))
                    {
                        pts = NoOfSegments.Two;
                    }
                    else
                    {
                        pts = NoOfSegments.Four;
                    }
                }
                else if ((tarPort != null && srcCorners.Right < tarPort.OffsetX) ||
                  (tarPort == null && srcCorners.Right < tarCorners.TopCenter.X))
                {
                    pts = NoOfSegments.Four;
                }
                else
                {
                    pts = NoOfSegments.Two;
                }
            }
            else if (sourcePort != null &&
              Math.Abs(srcCorners.Left - tarCorners.Right) <= 25 &&
              Math.Abs(sourcePort.OffsetY - tarCorners.Top) <= 25)
            {
                pts = NoOfSegments.Two;
            }
            else if (element.SourceWrapper != element.TargetWrapper &&
              (BaseUtil.CornersPointsBeforeRotation(element.SourceWrapper).ContainsPoint(top) ||
                  BaseUtil.CornersPointsBeforeRotation(element.TargetWrapper).ContainsPoint(left)))
            {
                pts = NoOfSegments.Two;
            }
            else if (srcCorners.Left > tarCorners.Right)
            {
                pts = NoOfSegments.Four;
            }
            else
            {
                pts = NoOfSegments.Four;
            }
            return pts;
        }

        internal static NoOfSegments GetLeftToLeftSegmentCount(Connector element, End source, End target)
        {
            DiagramElement srcPort = element.SourcePortWrapper;
            DiagramElement targetPort = element.TargetPortWrapper;

            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            source.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            target.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            double diffX = Math.Round(Math.Abs(srcPointX - tarPointX));
            double diffY = Math.Round(Math.Abs(srcPointY - tarPointY));
            NoOfSegments pts;
            if (srcCorners.Left < tarCorners.Left)
            {
                if ((targetPort != null && srcCorners.Bottom + source.Margin.Bottom < targetPort.OffsetY) ||
                    (targetPort == null && srcCorners.Bottom + source.Margin.Bottom < tarCorners.MiddleLeft.Y))
                {
                    pts = NoOfSegments.Three;
                }
                else if ((targetPort != null && srcCorners.Top - source.Margin.Top > targetPort.OffsetY) ||
                  (targetPort == null && srcCorners.Top - source.Margin.Top > tarCorners.MiddleLeft.Y))
                {
                    pts = NoOfSegments.Three;
                }
                else if (srcCorners.Right < tarCorners.Left ||
                  tarCorners.Right < srcCorners.Left)
                {
                    pts = NoOfSegments.Five;
                }
                else if (diffX == 0 || diffY == 0)
                {
                    pts = NoOfSegments.One;
                }
                else
                {
                    pts = NoOfSegments.Three;
                }
            }
            else if ((srcPort != null && srcPort.OffsetY < tarCorners.Top - target.Margin.Top) ||
              (srcPort == null && srcCorners.MiddleLeft.Y < tarCorners.Top))
            {
                pts = NoOfSegments.Three;
            }
            else if ((srcPort != null && srcPort.OffsetY > tarCorners.Bottom + target.Margin.Bottom) ||
              (srcPort == null && srcCorners.MiddleLeft.Y > tarCorners.Bottom + target.Margin.Bottom))
            {
                pts = NoOfSegments.Three;
            }
            else if (srcCorners.Left > tarCorners.Right)
            {
                pts = NoOfSegments.Five;
            }
            else if (diffX == 0 || diffY == 0)
            {
                pts = NoOfSegments.One;
            }
            else
            {
                pts = NoOfSegments.Three;
            }
            return pts;
        }

        internal static NoOfSegments GetTopToTopSegmentCount(Connector element, End source, End target)
        {
            DiagramElement srcPort = element.SourcePortWrapper;
            DiagramElement targetPort = element.TargetPortWrapper;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointY = target.Point.Y;
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double diffX = Math.Round(Math.Abs(srcPointX - target.Point.X));
            double diffY = Math.Round(Math.Abs(srcPointY - tarPointY));
            source.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            NoOfSegments pts;
            target.Margin = new Margin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
            if (srcCorners.Top < tarCorners.Top)
            {
                if ((targetPort != null && srcCorners.Left > targetPort.OffsetX) ||
                    (targetPort == null && srcCorners.Left > tarCorners.Left))
                {
                    pts = NoOfSegments.Three;
                }
                else if ((targetPort != null && srcCorners.Right < targetPort.OffsetX) ||
                  (targetPort == null && srcCorners.Right < tarCorners.Right))
                {
                    pts = NoOfSegments.Three;
                }
                else if (srcCorners.Bottom < tarCorners.Top)
                {
                    pts = NoOfSegments.Five;
                }
                else if (diffX == 0 || diffY == 0)
                {
                    pts = NoOfSegments.One;
                }
                else
                {
                    pts = NoOfSegments.Three;
                }
            }
            else if ((srcPort != null && srcPort.OffsetX > tarCorners.Right) ||
              (srcPort == null && srcCorners.Left > tarCorners.Right))
            {
                pts = NoOfSegments.Three;
            }
            else if ((srcPort != null && srcPort.OffsetX < tarCorners.Left) ||
              (srcPort == null && srcCorners.BottomRight.X < tarCorners.Left))
            {
                pts = NoOfSegments.Three;
            }
            else if (srcCorners.Top > tarCorners.Bottom)
            {
                pts = NoOfSegments.Five;
            }
            else if (diffX == 0 || diffY == 0)
            {
                pts = NoOfSegments.One;
            }
            else
            {
                pts = NoOfSegments.Three;
            }
            return pts;
        }

        internal static List<DiagramPoint> AddOrthoSegments(Connector element, NoOfSegments seg, End source, End target, double? segLength = null)
        {
            DiagramElement src = element.SourceWrapper;
            DiagramElement tar = element.TargetWrapper;
            List<DiagramPoint> intermediatePoints = new List<DiagramPoint>();
            Corners srcCorner = src.Corners;
            Corners tarCorner = tar.Corners;
            double extra = 20;
            if (source.Direction != target.Direction || seg == NoOfSegments.Five)
            {
                if (target.Direction != null && (source.Direction == GetOppositeDirection(target.Direction.Value) || seg == NoOfSegments.Three))
                {
                    double value;
                    switch (source.Direction)
                    {
                        case Direction.Left:
                            if (srcCorner.MiddleLeft.X > tarCorner.MiddleRight.X)
                            {
                                value = (srcCorner.MiddleLeft.X - tarCorner.MiddleRight.X) / 2;
                                extra = Math.Min(extra, value);
                            }
                            break;
                        case Direction.Right:
                            if (srcCorner.MiddleRight.X < tarCorner.MiddleLeft.X)
                            {
                                value = (tarCorner.MiddleLeft.X - srcCorner.MiddleRight.X) / 2;
                                extra = Math.Min(extra, value);
                            }
                            break;
                        case Direction.Top:
                            if (srcCorner.TopCenter.Y > tarCorner.BottomCenter.Y)
                            {
                                value = (srcCorner.TopCenter.Y - tarCorner.BottomCenter.Y) / 2;
                                extra = Math.Min(extra, value);
                            }
                            break;
                        case Direction.Bottom:
                            if (srcCorner.BottomCenter.Y < tarCorner.TopCenter.Y)
                            {
                                value = (tarCorner.TopCenter.Y - srcCorner.BottomCenter.Y) / 2;
                                extra = Math.Min(extra, value);
                            }
                            break;
                    }
                }
            }
            extra = AdjustSegmentLength(srcCorner, source, extra);
            if (segLength.HasValue)
            {
                extra = Math.Max(extra, segLength.Value);
            }
            if (seg == NoOfSegments.One)
            {
                intermediatePoints = new List<DiagramPoint> { source.Point, target.Point };
            }
            if (seg == NoOfSegments.Two)
            {
                intermediatePoints = OrthoConnection2Segment(source, target);
            }
            if (seg == NoOfSegments.Three)
            {
                intermediatePoints = OrthoConnection3Segment(element, source, target, extra);
            }
            if (seg == NoOfSegments.Four)
            {
                Direction? prevDir = null;
                intermediatePoints = OrthoConnection4Segment(source, target, intermediatePoints, extra, prevDir);
            }
            if (seg == NoOfSegments.Five)
            {
                intermediatePoints = OrthoConnection5Segment(source, target, extra);
            }
            return intermediatePoints;
        }

        internal static double AdjustSegmentLength(object connectionBounds, End source, double extra)
        {
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            if (connectionBounds.GetType() == typeof(DiagramRect))
            {
                DiagramRect bounds = connectionBounds as DiagramRect;
                switch (source.Direction)
                {
                    case Direction.Left:
                        if (bounds != null && srcPointX > bounds.Left)
                        {
                            extra = (srcPointX - bounds.Left) > extra ? ((srcPointX - bounds.Left) + extra) : extra;
                        }
                        break;
                    case Direction.Right:
                        if (bounds != null && srcPointX < bounds.Right)
                        {
                            extra = (bounds.Right - srcPointX) > extra ? ((bounds.Right - srcPointX) + extra) : extra;
                        }
                        break;
                    case Direction.Top:
                        if (bounds != null && srcPointY > bounds.Top)
                        {
                            extra = (srcPointY - bounds.Top) > extra ? ((srcPointY - bounds.Top) + extra) : extra;
                        }
                        break;
                    case Direction.Bottom:
                        if (bounds != null && srcPointY < bounds.Bottom)
                        {
                            extra = (bounds.Bottom - srcPointY) > extra ? ((bounds.Bottom - srcPointY) + extra) : extra;
                        }
                        break;
                }
            }
            else
            {
                Corners bounds = connectionBounds as Corners;
                switch (source.Direction)
                {
                    case Direction.Left:
                        if (bounds != null && srcPointX > bounds.Left)
                        {
                            extra = (srcPointX - bounds.Left) > extra ? ((srcPointX - bounds.Left) + extra) : extra;
                        }
                        break;
                    case Direction.Right:
                        if (bounds != null && srcPointX < bounds.Right)
                        {
                            extra = (bounds.Right - srcPointX) > extra ? ((bounds.Right - srcPointX) + extra) : extra;
                        }
                        break;
                    case Direction.Top:
                        if (bounds != null && srcPointY > bounds.Top)
                        {
                            extra = (srcPointY - bounds.Top) > extra ? ((srcPointY - bounds.Top) + extra) : extra;
                        }
                        break;
                    case Direction.Bottom:
                        if (bounds != null && srcPointY < bounds.Bottom)
                        {
                            extra = (bounds.Bottom - srcPointY) > extra ? ((bounds.Bottom - srcPointY) + extra) : extra;
                        }
                        break;
                }
            }
            return extra;
        }

        internal static List<DiagramPoint> OrthoConnection2Segment(End source, End target)
        {
            List<DiagramPoint> intermediatePoints = new List<DiagramPoint>();
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointY = target.Point.Y;
            switch (source.Direction)
            {
                case Direction.Left:
                case Direction.Right:
                    DiagramPoint point1 = new DiagramPoint { X = target.Point.X, Y = srcPointY };
                    intermediatePoints = (DiagramPoint.Equals(source.Point, point1) || DiagramPoint.Equals(target.Point, point1)) ?
                       new List<DiagramPoint> { source.Point, target.Point } : new List<DiagramPoint> { source.Point, point1, target.Point };
                    break;
                case Direction.Top:
                case Direction.Bottom:
                    DiagramPoint point2 = new DiagramPoint { X = srcPointX, Y = tarPointY };
                    intermediatePoints = (DiagramPoint.Equals(source.Point, point2) || DiagramPoint.Equals(target.Point, point2)) ?
                        new List<DiagramPoint> { source.Point, target.Point } : new List<DiagramPoint> { source.Point, point2, target.Point };
                    break;
            }
            return intermediatePoints;
        }

        internal static List<DiagramPoint> OrthoConnection3Segment(Connector element, End source, End target, double? extra = null, bool? allow = null)
        {
            if (extra == null)
            {
                extra = 20;
            }
            DiagramElement srcPort = element.SourcePortWrapper;
            List<DiagramPoint> intermediatePoints;
            DiagramPoint segmentValue = null;
            DiagramPoint next = null;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            double diffPointX = tarPointX - srcPointX;
            double diffPointY = tarPointY - srcPointY;
            double temp = 0;
            if (allow.HasValue && !allow.Value && (Math.Abs(diffPointX) < 0.001 || Math.Abs(diffPointY) < 0.001))
            {
                if (target.Direction == null)
                {
                    intermediatePoints = new List<DiagramPoint>() { source.Point, target.Point };
                    return intermediatePoints;
                }
            }
            if (element.TargetWrapper == null && Math.Abs(diffPointX) <= 31 && Math.Abs(diffPointY) <= 31)
            {
                if ((source.Direction == Direction.Left || source.Direction == Direction.Right))
                {
                    if (Math.Abs(diffPointY) < 12)
                    {
                        source.Direction = (srcPointY > tarPointY) ? Direction.Top : Direction.Bottom;
                    }
                }
                else
                {
                    if (Math.Abs(diffPointX) < 12)
                    {
                        source.Direction = (srcPointX > tarPointX) ? Direction.Left : Direction.Right;
                    }
                }
                if (Math.Abs(diffPointX) > 12 || Math.Abs(diffPointY) > 12)
                {
                    return OrthoConnection2Segment(source, target);
                }
                else
                {
                    extra += 5;
                }
            }
            if (source.Direction == Direction.Left || source.Direction == Direction.Right)
            {
                if (source.Direction == Direction.Right)
                {
                    if (target.Direction != null && target.Direction == Direction.Right)
                    {
                        extra = Math.Max(srcPointX, tarPointX) - srcPointX + extra;
                    }
                    if (srcPointX > tarPointX && srcPort == null)
                    {
                        extra = -extra;
                    }
                }
                else
                {
                    if (target.Direction != null && target.Direction == Direction.Left)
                    {
                        extra = srcPointX - Math.Min(srcPointX, tarPointX) + extra;
                    }
                    if (srcPointX > tarPointX || srcPort != null || source.Direction == Direction.Left)
                    {
                        extra = -extra;
                    }
                }
                segmentValue = AddLineSegment(source.Point, extra.Value, 0);
                temp = tarPointY - segmentValue.Y;
                if (temp != 0)
                {
                    next = AddLineSegment(segmentValue, tarPointY - segmentValue.Y, 90);
                }
            }
            else if (source.Direction == Direction.Top || source.Direction == Direction.Bottom)
            {
                if (source.Direction == Direction.Bottom)
                {
                    if (target.Direction != null && target.Direction == Direction.Bottom)
                    {
                        extra = Math.Max(srcPointY, tarPointY) - srcPointY + extra;
                    }
                }
                else
                {
                    if (target.Direction != null && target.Direction == Direction.Top)
                    {
                        extra = srcPointY - Math.Min(srcPointY, tarPointY) + extra;
                    }
                    if (srcPointY > tarPointY || (srcPort != null) || source.Direction == Direction.Top)
                    {
                        extra = -extra;
                    }
                }
                if (source.Direction == Direction.Top)
                {
                    segmentValue = AddLineSegment(source.Point, extra.Value, 90);
                }
                else
                {
                    segmentValue = AddLineSegment(source.Point, extra.Value, 90);
                }
                temp = tarPointX - segmentValue.X;
                if (temp != 0)
                {
                    next = AddLineSegment(segmentValue, tarPointX - segmentValue.X, 0);
                }
            }
            if (temp == 0)
            {
                return new List<DiagramPoint> { source.Point, target.Point };
            }
            intermediatePoints = new List<DiagramPoint>{
                source.Point, segmentValue,
                next, target.Point
            };
            return intermediatePoints;
        }

        internal static List<DiagramPoint> OrthoConnection5Segment(End source, End target, double extra = 20)
        {
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X; double tarPointY = target.Point.Y;
            Margin srcMargin = source.Margin; Margin tarMargin = target.Margin;
            double length = extra;
            double sLeft = srcCorners.Left - srcMargin.Left;
            double sRight = srcCorners.Right + srcMargin.Right;
            double sBottom = srcCorners.Bottom + srcMargin.Bottom;
            double sTop = srcCorners.Top - srcMargin.Top;
            double tLeft = tarCorners.Left - tarMargin.Left;
            double tRight = tarCorners.Right + tarMargin.Right;
            double tBottom = tarCorners.Bottom + tarMargin.Bottom;
            double tTop = tarCorners.Top - tarMargin.Top;
            DiagramPoint segmentValue = null;
            switch (source.Direction)
            {
                case Direction.Left:
                    if ((sTop > tTop && sTop < tBottom || sBottom < tBottom && sBottom > tTop) &&
                        sLeft > tLeft && sLeft <= tRight && extra >= 20)
                    {
                        length = srcPointX - tarCorners.Left + length;
                    }
                    segmentValue = AddLineSegment(source.Point, length, 180);
                    break;
                case Direction.Top:
                    if ((sLeft > tLeft && sLeft < tRight || sRight < tRight && sRight > tLeft) &&
                        sTop > tTop && sTop <= tBottom && extra >= 20)
                    {
                        length = srcPointY - tarCorners.Top + length;
                    }
                    segmentValue = AddLineSegment(source.Point, length, 270);
                    break;
                case Direction.Right:
                    if ((sTop > tTop && sTop < tBottom || sBottom < tBottom && sBottom > tTop) &&
                        sRight < tRight && sRight >= tLeft && extra >= 20)
                    {
                        length = tarCorners.Right - srcPointX + length;
                    }
                    segmentValue = AddLineSegment(source.Point, length, 0);
                    break;
                case Direction.Bottom:
                    if ((sLeft > tLeft && sLeft < tRight || sRight < tRight && sRight > tLeft) &&
                        sBottom < tBottom && sBottom >= tTop && extra >= 20)
                    {
                        length = tarCorners.Bottom - srcPointY + length;
                    }
                    segmentValue = AddLineSegment(source.Point, length, 90);
                    break;
            }
            List<DiagramPoint> intermediatePoints = new List<DiagramPoint>{
                source.Point,
                segmentValue
            };
            if (source.Direction == Direction.Top || source.Direction == Direction.Bottom)
            {
                Direction prevDir = source.Direction.Value;
                source.Direction = segmentValue != null && segmentValue.X > tarPointX ? Direction.Left : Direction.Right;
                source.Point = segmentValue;
                intermediatePoints = OrthoConnection4Segment(source, target, intermediatePoints, 20, prevDir);
            }
            else
            {
                Direction prevDir = source.Direction.Value;
                source.Direction = segmentValue != null && segmentValue.Y > tarPointY ? Direction.Top : Direction.Bottom;
                source.Point = segmentValue;
                intermediatePoints = OrthoConnection4Segment(source, target, intermediatePoints, 20, prevDir);
            }
            return intermediatePoints;
        }

        internal static List<DiagramPoint> OrthoConnection4Segment(End source, End target, List<DiagramPoint> interPt, double e = 20, Direction? prevDir = null)
        {
            DiagramPoint segmentValue = null; Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            if (prevDir == null)
            {
                source.Margin = new Margin { Left = 2, Right = 2, Top = 2, Bottom = 2 };
                target.Margin = new Margin { Left = 0, Right = 5, Top = 0, Bottom = 5 };
            }
            else
            {
                if (source.Direction == Direction.Bottom)
                {
                    if (tarCorners.Top > srcCorners.Bottom && tarCorners.Top - srcCorners.Bottom < 20)
                    {
                        e = (tarCorners.Top - srcCorners.Bottom) / 2;
                    }
                }
                else if (source.Direction == Direction.Top)
                {
                    if (tarCorners.Bottom < srcCorners.Top && srcCorners.Top - tarCorners.Bottom < 20)
                    {
                        e = (srcCorners.Top - tarCorners.Bottom) / 2;
                    }
                }
                else if (source.Direction == Direction.Right)
                {
                    if (tarCorners.Left > srcCorners.Right && tarCorners.Left - srcCorners.Right < 20)
                    {
                        e = (tarCorners.Left - srcCorners.Right) / 2;
                    }
                }
                else if (source.Direction == Direction.Left)
                {
                    if (tarCorners.Right < srcCorners.Left && srcCorners.Left - tarCorners.Right < 20)
                    {
                        e = (srcCorners.Left - tarCorners.Right) / 2;
                    }
                }
            }
            switch (source.Direction)
            {
                case Direction.Left:
                    e = GetLeftLength(source, target, e, prevDir);
                    segmentValue = AddLineSegment(source.Point, e, 180);
                    break;
                case Direction.Right:
                    e = GetRightLength(source, target, e, prevDir);
                    segmentValue = AddLineSegment(source.Point, e, 0);
                    break;
                case Direction.Top:
                    e = GetTopLength(source, target, e, prevDir);
                    segmentValue = AddLineSegment(source.Point, e, 270);
                    break;
                case Direction.Bottom:
                    e = GetBottomLength(source, target, e, prevDir);
                    segmentValue = AddLineSegment(source.Point, e, 90);
                    break;
            }
            if (interPt.Count > 0)
            {
                interPt.Add(segmentValue);
            }
            else
            {
                interPt = new List<DiagramPoint> {
                    source.Point,
                    segmentValue
                };
            }
            if (source.Direction == Direction.Top || source.Direction == Direction.Bottom)
            {
                GetOrtho3Seg(segmentValue, "horizontal", source, target, interPt);
            }
            else if (source.Direction == Direction.Right || source.Direction == Direction.Left)
            {
                GetOrtho3Seg(segmentValue, "vertical", source, target, interPt);
            }
            return interPt;
        }

        internal static void GetOrtho3Seg(DiagramPoint sPt, string orientation, End src, End tar, List<DiagramPoint> points)
        {
            Corners srcComers = src.Corners; Corners tarCorners = tar.Corners;
            if (orientation == "horizontal")
            {
                src.Margin = new Margin { Left = 0, Right = 10, Top = 0, Bottom = 10 };
                tar.Margin = new Margin { Left = 0, Right = 10, Top = 0, Bottom = 10 };
            }
            else if (orientation == "vertical")
            {
                src.Margin = new Margin { Left = 10, Right = 0, Top = 10, Bottom = 0 };
                tar.Margin = new Margin { Left = 10, Right = 0, Top = 10, Bottom = 0 };
            }
            double extra = 20;
            if (orientation == "horizontal")
            {
                UpdateHOrientation(sPt, src, tar, srcComers, tarCorners, points, extra);
            }
            else if (orientation == "vertical")
            {
                UpdateVOrientation(sPt, src, tar, srcComers, tarCorners, points, extra);
            }
        }

        private static double UpdateVOrientation(DiagramPoint sPt, End src, End tar, Corners srcCorners, Corners tarCorners, List<DiagramPoint> points, double extra)
        {
            switch (tar.Direction)
            {
                case Direction.Top:
                    if (srcCorners.Bottom + src.Margin.Bottom < tarCorners.Top - tar.Margin.Top &&
                        (tarCorners.Top - srcCorners.Bottom > extra || (srcCorners.Left - src.Margin.Left <= tar.Point.X &&
                            srcCorners.Right + src.Margin.Right >= tar.Point.X)))
                    {
                        double gap = Math.Min(Math.Abs(tarCorners.Top - srcCorners.Bottom) / 2, 20);
                        extra = srcCorners.Bottom - sPt.Y + gap;
                    }
                    else
                    {
                        if ((src.Direction == Direction.Left && sPt.X > tar.Point.X) || (src.Direction == Direction.Right && sPt.X < tar.Point.X))
                        {
                            extra = Math.Min(tarCorners.Top, sPt.Y) - sPt.Y - 20;
                        }
                        else if (sPt.Y >= srcCorners.Top - src.Margin.Top && sPt.Y <= srcCorners.Bottom + src.Margin.Bottom)
                        {
                            extra = Math.Min(tarCorners.Top, srcCorners.Top) - sPt.Y - 20;
                        }
                        else
                        {
                            extra = tarCorners.Top - sPt.Y - 20;
                        }
                    }
                    break;
                case Direction.Bottom:
                    if (srcCorners.Top - src.Margin.Top > tarCorners.Bottom + tar.Margin.Bottom &&
                        (srcCorners.Top - tarCorners.Bottom > extra || (srcCorners.Left - src.Margin.Left <= tar.Point.X &&
                            srcCorners.Right + src.Margin.Right >= tar.Point.X)))
                    {
                        double gap = Math.Min(Math.Abs(srcCorners.Top - tarCorners.Bottom) / 2, 20);
                        extra = srcCorners.Top - sPt.Y - gap;
                    }
                    else
                    {
                        if ((src.Direction == Direction.Left && sPt.X > tar.Point.X) || (src.Direction == Direction.Right && sPt.X < tar.Point.X))
                        {
                            extra = Math.Max(tarCorners.Bottom, sPt.Y) - sPt.Y + 20;
                        }
                        else if (sPt.Y >= srcCorners.Top - src.Margin.Top && sPt.Y <= srcCorners.Bottom + src.Margin.Bottom)
                        {
                            extra = Math.Max(tarCorners.Bottom, srcCorners.Bottom) - sPt.Y + 20;
                        }
                        else
                        {
                            extra = tarCorners.Bottom - sPt.Y + 20;
                        }
                    }
                    break;
            }
            DiagramPoint point1 = AddLineSegment(sPt, extra, 90);
            DiagramPoint point2 = AddLineSegment(point1, tar.Point.X - sPt.X, 0);
            DiagramPoint point3 = tar.Point;
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            return extra;
        }

        private static double UpdateHOrientation(DiagramPoint sPt, End src, End tar, Corners srcCorners, Corners tarCorners, List<DiagramPoint> points, double extra)
        {
            switch (tar.Direction)
            {
                case Direction.Left:
                    if (srcCorners.Right + src.Margin.Right < tarCorners.Left - tar.Margin.Left &&
                        (tarCorners.Left - srcCorners.Right > extra || (srcCorners.Top - src.Margin.Top <= tar.Point.Y &&
                            srcCorners.Bottom + src.Margin.Bottom >= tar.Point.Y)))
                    {
                        double gap = Math.Min(Math.Abs(tarCorners.Left - srcCorners.Right) / 2, 20);
                        extra = srcCorners.Right - sPt.X + gap;
                    }
                    else
                    {
                        if ((src.Direction == Direction.Top && sPt.Y > tar.Point.Y) || (src.Direction == Direction.Bottom && sPt.Y < tar.Point.Y))
                        {
                            extra = Math.Min(tarCorners.Left, sPt.X) - sPt.X - 20;
                        }
                        else if (sPt.X >= srcCorners.Left - src.Margin.Left && sPt.X <= srcCorners.Right + src.Margin.Right)
                        {
                            extra = Math.Min(tarCorners.Left, srcCorners.Left) - sPt.X - 20;
                        }
                        else
                        {
                            extra = tarCorners.Left - sPt.X - 20;
                        }
                    }
                    break;
                case Direction.Right:
                    if (srcCorners.Left - src.Margin.Left > tarCorners.Right + tar.Margin.Right &&
                        (srcCorners.Left - tarCorners.Right > extra || (srcCorners.Top - src.Margin.Top <= tar.Point.Y &&
                            srcCorners.Bottom + src.Margin.Bottom >= tar.Point.Y)))
                    {
                        double gap = Math.Min(Math.Abs(srcCorners.Left - tarCorners.Right) / 2, 20);
                        extra = srcCorners.Left - sPt.X - gap;
                    }
                    else
                    {
                        if ((src.Direction == Direction.Top && sPt.Y > tar.Point.Y) || (src.Direction == Direction.Bottom && sPt.Y < tar.Point.Y))
                        {
                            extra = Math.Max(tarCorners.Right, sPt.X) - sPt.X + 20;
                        }
                        else if (sPt.X >= srcCorners.Left - src.Margin.Left && sPt.X <= srcCorners.Right + src.Margin.Right)
                        {
                            extra = Math.Max(tarCorners.Right, srcCorners.Right) - sPt.X + 20;
                        }
                        else
                        {
                            extra = tarCorners.Right - sPt.X + 20;
                        }
                    }
                    break;
            }
            DiagramPoint point1 = AddLineSegment(sPt, extra, 0);
            DiagramPoint point2 = AddLineSegment(point1, tar.Point.Y - sPt.Y, 90);
            DiagramPoint point3 = tar.Point;
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            return extra;
        }

        internal static PathElement UpdatePathElement(List<DiagramPoint> anglePoints, Connector connector)
        {
            PathElement pathElement = new PathElement();
            PathInformation pathSeqData = new PathInformation();
            for (int j = 0; j < anglePoints.Count - 1; j++)
            {
                pathSeqData = FindPath(anglePoints[j], anglePoints[j + 1]);
                pathElement.Data = pathSeqData.Path;
                pathElement.ID = connector.ID + '_' + connector.Shape.Sequence;
                pathElement.OffsetX = pathSeqData.Points.X;
                pathElement.OffsetY = pathSeqData.Points.Y;
                pathElement.RotationAngle = 45;
                pathElement.Transform = Transform.Self;
                pathElement.IsBpmnSequenceDefault = true;
            }
            pathElement.AbsoluteBounds.X = pathSeqData.StartPoint.X < pathSeqData.TargetPoint.X ? pathSeqData.StartPoint.X : pathSeqData.TargetPoint.X;
            pathElement.AbsoluteBounds.Y = pathSeqData.StartPoint.Y < pathSeqData.TargetPoint.Y ? pathSeqData.StartPoint.Y : pathSeqData.TargetPoint.Y;
            pathElement.AbsoluteBounds.Width = Math.Abs(pathSeqData.StartPoint.X - pathSeqData.TargetPoint.X);
            pathElement.AbsoluteBounds.Height = Math.Abs(pathSeqData.StartPoint.Y - pathSeqData.TargetPoint.Y);
            return pathElement;
        }
        internal static PathInformation FindPath(DiagramPoint sourcePoint, DiagramPoint targetPoint)
        {
            DiagramPoint beginningPoint = new DiagramPoint() { X = sourcePoint.X, Y = sourcePoint.Y };
            double distance = FindDistance(sourcePoint, targetPoint);
            distance = Math.Min(30, distance / 2);
            int angle = Convert.ToInt32(FindAngle(sourcePoint, targetPoint));
            DiagramPoint transferee = DiagramPoint.Transform(new DiagramPoint() { X = beginningPoint.X, Y = beginningPoint.Y }, angle, distance);
            DiagramPoint startPoint = DiagramPoint.Transform(new DiagramPoint() { X = transferee.X, Y = transferee.Y }, angle, -12);
            DiagramPoint endpoint = DiagramPoint.Transform(new DiagramPoint() { X = startPoint.X, Y = startPoint.Y }, angle, 12 * 2);
            string path = "M" + startPoint.X + " " + startPoint.Y + " L" + endpoint.X + ' ' + endpoint.Y;
            PathInformation pathInformation = new PathInformation() { Path = path, Points = transferee, StartPoint = startPoint, TargetPoint = endpoint };
            return pathInformation;
        }
        internal static double FindDistance(DiagramPoint point1, DiagramPoint point2)
        {
            int value = Convert.ToInt32(Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2)));
            return value;
        }
        internal static double GetTopLength(End source, End target, double length, Direction? preDir)
        {
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointX = target.Point.X;
            if (srcCorners.Top - source.Margin.Top > tarCorners.Top + target.Margin.Top &&
                srcCorners.Top - source.Margin.Top <= tarCorners.Bottom + target.Margin.Bottom)
            {
                if (target.Direction == Direction.Right && srcPointX < tarPointX)
                {
                    length += srcCorners.Top - tarCorners.Top;
                }
                else if (target.Direction == Direction.Left && srcPointX > tarPointX)
                {
                    length += srcCorners.Top - tarCorners.Top;
                }
                length += srcPointY - srcCorners.Top;
            }
            else
            {
                if (preDir.HasValue && preDir != Direction.Left && target.Direction == Direction.Right && srcPointX < tarPointX)
                {
                    length += Math.Abs(srcPointY - tarCorners.Bottom);
                }
                else if (preDir.HasValue && preDir != Direction.Right && target.Direction == Direction.Left && tarPointX < srcPointX)
                {
                    length += Math.Abs(srcPointY - tarCorners.Bottom);
                }
                else
                {
                    length += srcPointY - srcCorners.Top;
                }
            }
            return length;
        }

        internal static double GetLeftLength(End source, End target, double segLength, Direction? prevDir)
        {
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointY = target.Point.Y;
            if (srcCorners.Left - source.Margin.Left > tarCorners.Left - target.Margin.Left &&
                srcCorners.Left - source.Margin.Left <= tarCorners.Right + target.Margin.Right)
            {
                if (target.Direction == Direction.Bottom && srcPointY < tarPointY)
                {
                    segLength += srcCorners.Left - tarCorners.Left;
                }
                else if (target.Direction == Direction.Top && srcPointY > tarPointY)
                {
                    segLength += srcCorners.Left - tarCorners.Left;
                }
                segLength += srcPointX - srcCorners.Left;
            }
            else
            {
                if (prevDir.HasValue && prevDir != Direction.Top && target.Direction == Direction.Bottom && srcPointY < tarPointY)
                {
                    segLength += Math.Abs(srcPointX - tarCorners.Right);
                }
                else if (prevDir.HasValue && prevDir != Direction.Bottom && target.Direction == Direction.Top && tarPointY < srcPointY)
                {
                    segLength += Math.Abs(srcPointX - tarCorners.Right);
                }
                else
                {
                    segLength += srcPointX - srcCorners.Left;
                }
            }
            return segLength;
        }

        internal static double GetRightLength(End source, End target, double length, Direction? prevDir)
        {
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double srcPointX = source.Point.X; double srcPointY = source.Point.Y;
            double tarPointY = target.Point.Y;
            if (srcCorners.Right + source.Margin.Right < tarCorners.Right + target.Margin.Right &&
                srcCorners.Right + source.Margin.Right >= tarCorners.Left - target.Margin.Left)
            {
                if (target.Direction == Direction.Bottom && srcPointY < tarPointY)
                {
                    length += tarCorners.Right - srcCorners.Right;
                }
                else if (target.Direction == Direction.Top && srcPointY > tarPointY)
                {
                    length += tarCorners.Right - srcCorners.Right;
                }
                length += srcCorners.Right - srcPointX;
            }
            else
            {
                if (prevDir.HasValue && prevDir != Direction.Top && target.Direction == Direction.Bottom && srcPointY < tarPointY)
                {
                    length += Math.Abs(srcPointX - tarCorners.Right);
                }
                else if (prevDir.HasValue && prevDir != Direction.Bottom && target.Direction == Direction.Top && tarPointY < srcPointY)
                {
                    length += Math.Abs(srcPointX - tarCorners.Right);
                }
                else
                {
                    length += srcCorners.Right - srcPointX;
                }
            }
            return length;
        }
        internal static void UpdateConnectorShape(Connector connector, Dictionary<string, object> shape)
        {
            foreach (string propertyName in shape.Keys)
            {
                object newValue = ((PropertyChangeValues)shape[propertyName]).NewValue;
                object oldValue = ((PropertyChangeValues)shape[propertyName]).OldValue;
                if (newValue != oldValue)
                {
                    BpmnFlows type = connector.Shape.Flow;
                    if (shape.ContainsKey(FLOW))
                    {
                        _ = Enum.TryParse(typeof(BpmnFlows), ((PropertyChangeValues)shape[FLOW]).NewValue.ToString(), out object bpmnFlows);
                        type = (BpmnFlows)bpmnFlows;
                    }
                    else
                    {
                        switch (type)
                        {
                            case BpmnFlows.Sequence:
                                if (Enum.TryParse(typeof(BpmnSequenceFlows), ((PropertyChangeValues)shape[SEQUENCE]).NewValue.ToString(), out object bpmnSequenceFlows))
                                    connector.Shape.Sequence = (BpmnSequenceFlows)bpmnSequenceFlows;
                                connector = connector.GetSequence(connector);
                                break;
                            case BpmnFlows.Association:
                                if (Enum.TryParse(typeof(BpmnAssociationFlows), ((PropertyChangeValues)shape[ASSOCIATION]).NewValue.ToString(), out object bpmnAssociationFlows))
                                    connector.Shape.Association = (BpmnAssociationFlows)bpmnAssociationFlows;
                                connector = connector.GetAssociation(connector);
                                break;
                            case BpmnFlows.Message:
                                if (Enum.TryParse(typeof(BpmnMessageFlows), ((PropertyChangeValues)shape[MESSAGE]).NewValue.ToString(), out object bpmnMessageFlows))
                                    connector.Shape.Message = (BpmnMessageFlows)bpmnMessageFlows;
                                connector = connector.GetMessage(connector);
                                break;
                        }
                    }
                }
            }
        }
        internal static double GetBottomLength(End source, End target, double segLength, Direction? prevDir)
        {
            Corners srcCorners = source.Corners; Corners tarCorners = target.Corners;
            double srcPointX = source.Point.X; double tarPointX = target.Point.X;
            double srcPointY = source.Point.Y;
            if (srcCorners.Bottom + source.Margin.Bottom < tarCorners.Bottom + target.Margin.Bottom &&
                srcCorners.Bottom + source.Margin.Bottom >= tarCorners.Top - target.Margin.Top)
            {
                if (target.Direction == Direction.Right && srcPointX < tarPointX)
                {
                    segLength += tarCorners.Bottom - srcCorners.Bottom;
                }
                else if (target.Direction == Direction.Left && srcPointX > tarPointX)
                {
                    segLength += tarCorners.Bottom - srcCorners.Bottom;
                }
                segLength += srcCorners.Bottom - srcPointY;
            }
            else
            {
                if (prevDir.HasValue && prevDir != Direction.Left && target.Direction == Direction.Right && srcPointX < tarPointX)
                {
                    segLength += Math.Abs(srcPointY - tarCorners.Bottom);
                }
                else if (prevDir.HasValue && prevDir != Direction.Right && target.Direction == Direction.Left && tarPointX < srcPointX)
                {
                    segLength += Math.Abs(srcPointY - tarCorners.Bottom);
                }
                else
                {
                    segLength += srcCorners.Bottom - srcPointY;
                }
            }
            return segLength;
        }
        internal static bool GetSwapping(Direction srcDir, Direction tarDir)
        {
            bool swap = false;
            switch (srcDir)
            {
                case Direction.Left:
                    if (tarDir == Direction.Right || tarDir == Direction.Bottom)
                    {
                        swap = true;
                    }
                    break;
                case Direction.Top:
                    if (tarDir == Direction.Left || tarDir == Direction.Right || tarDir == Direction.Bottom)
                    {
                        swap = true;
                    }
                    break;
                case Direction.Bottom:
                    if (tarDir == Direction.Right)
                    {
                        swap = true;
                    }
                    break;
            }
            return swap;
        }

        internal static void SwapPoints(End source, End target)
        {
            (source.Direction, target.Direction) = (target.Direction, source.Direction);
            (source.Point, target.Point) = (target.Point, source.Point);
            (source.Corners, target.Corners) = (target.Corners, source.Corners);
        }

        internal static Direction GetPortDirection(DiagramPoint point, DiagramRect corner, DiagramRect bounds)
        {
            Direction direction;
            DiagramRect boundsValue = corner ?? bounds;
            DiagramPoint one = boundsValue.TopLeft;
            DiagramPoint two = boundsValue.TopRight;
            DiagramPoint three = boundsValue.BottomRight;
            DiagramPoint four = boundsValue.BottomLeft;
            DiagramPoint center = boundsValue.Center;
            double angle = FindAngle(center, point);
            double fourty5 = FindAngle(center, three);
            double one35 = FindAngle(center, four);
            double two25 = FindAngle(center, one);
            double three15 = FindAngle(center, two);
            if (angle > two25 && angle < three15)
            {
                direction = Direction.Top;
            }
            else if (angle >= fourty5 && angle < one35)
            {
                direction = Direction.Bottom;
            }
            else if (angle >= one35 && angle <= two25)
            {
                direction = Direction.Left;
            }
            else if (angle >= three15 || angle < fourty5)
            {
                direction = Direction.Right;
            }
            else
            {
                direction = Direction.Right;
            }
            return direction;
        }
        internal static DiagramRect GetOuterBounds(Connector obj)
        {
            DiagramContainer wrapper = obj.Wrapper;
            DiagramRect outerBounds = wrapper.Children[0].Bounds;
            if (obj.SourceDecorator.Shape != DecoratorShape.None)
            {
                outerBounds.UniteRect(wrapper.Children[1].Bounds);
            }
            if (obj.TargetDecorator.Shape != DecoratorShape.None)
            {
                outerBounds.UniteRect(wrapper.Children[2].Bounds);
            }
            return outerBounds;
        }

        internal static Direction? GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return Direction.Bottom;
                case Direction.Bottom:
                    return Direction.Top;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    break;
            }
            return null;
        }

    }

    internal class Bridge
    {
        internal double Angle { get; set; }
        internal DiagramPoint EndPoint { get; set; }
        internal string Path { get; set; }
        internal double SegmentPointIndex { get; set; }
        internal DiagramPoint StartPoint { get; set; }
        internal double Sweep { get; set; }
        internal string Target { get; set; }
        internal bool Rendered { get; set; }
    }

    internal class End
    {
        internal Corners Corners { get; set; }
        internal DiagramPoint Point { get; set; }
        internal Direction? Direction { get; set; }
        internal Margin Margin { get; set; }
    }
    internal class Segment
    {
        internal double X1 { get; set; }
        internal double X2 { get; set; }
        internal double Y1 { get; set; }
        internal double Y2 { get; set; }
    }

    internal class Intersection
    {
        internal bool Enabled { get; set; }
        internal DiagramPoint IntersectPt { get; set; }
    }
    /// <summary>
    /// Defines the <see cref="PathInformation" />.
    /// </summary>
    internal class PathInformation
    {
        /// <summary>
        /// Gets or sets the Path.
        /// </summary>
        internal string Path { get; set; }

        /// <summary>
        /// Gets or sets the Points.
        /// </summary>
        internal DiagramPoint Points { get; set; }

        /// <summary>
        /// Gets or sets the Points.
        /// </summary>
        internal DiagramPoint StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the Points.
        /// </summary>
        internal DiagramPoint TargetPoint { get; set; }
    }
}

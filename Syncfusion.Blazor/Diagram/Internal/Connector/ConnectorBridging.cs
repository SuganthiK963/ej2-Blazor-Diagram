using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class ConnectorBridging
    {
        internal static void UpdateBridging(Connector conn, SfDiagramComponent diagram)
        {
            List<BridgeSegment> lastBridge = new List<BridgeSegment>();
            conn.Bridges = new List<Bridge>();
            if (ConstraintsUtil.CanBridge(conn, diagram))
            {
                List<DiagramPoint> points1 = GetPoints(conn);
                DiagramRect bounds = DiagramRect.ToBounds(points1);
                double bridgeSpacing = conn.BridgeSpace; Direction bridgeDirection = diagram.BridgeDirection;
                ObservableCollection<Connector> quads = diagram.Connectors;
                for (int q = 0; q < quads.Count; q++)
                {
                    Connector connector1 = quads[q];
                    if (conn != null && connector1 != null && conn.ID != connector1.ID)
                    {
                        List<DiagramPoint> points2 = GetPoints((Connector)connector1);
                        DiagramRect bounds1 = DiagramRect.ToBounds(points2);
                        if (IntersectsRect(bounds, bounds1))
                        {
                            List<DiagramPoint> intersectPts = Intersect(points1, points2, false, bridgeDirection, true);
                            if (intersectPts.Count > 0)
                            {
                                UpdateBridgingExtended(conn, lastBridge, points1, bridgeSpacing, bridgeDirection, connector1, intersectPts);
                            }
                        }
                    }
                }
                if (lastBridge.Count != 0)
                {
                    FirstBridge(lastBridge, conn, bridgeSpacing);
                }
            }
        }

        private static void UpdateBridgingExtended(Connector conn, List<BridgeSegment> lastBridge, List<DiagramPoint> points1, double bridgeSpacing, Direction bgedir, Connector connector, List<DiagramPoint> intersectPts)
        {
            for (int i = 0; i < intersectPts.Count; i++)
            {
                LengthFraction obj = GetLengthAtFractionPoint(conn, intersectPts[i]);
                if (obj.PointIndex != -1)
                {
                    double length = obj.LengthFractionIndex;
                    int segmentIndex = obj.SegmentIndex;
                    int pointIndex = obj.PointIndex;
                    DiagramPoint stBridge = GetPointAtLength((length - (bridgeSpacing / 2)), points1);
                    DiagramPoint enBridge = GetPointAtLength((length + (bridgeSpacing / 2)), points1);
                    if (DiagramPoint.IsEmptyPoint(enBridge))
                    {
                        enBridge = stBridge;
                    }

                    DiagramPoint start = conn.SourcePoint;
                    DiagramPoint end = conn.Type == ConnectorSegmentType.Straight ? conn.TargetPoint : conn.IntermediatePoints[pointIndex];
                    double angle = AngleCalculation(start, end);
                    if (lastBridge.Count > 0)
                    {
                        InsertingBridge(conn, lastBridge, bgedir, connector, segmentIndex, pointIndex, stBridge, enBridge, angle);
                    }
                    else
                    {
                        if (!double.IsNaN(stBridge.X) && !double.IsNaN(stBridge.Y) && !DiagramPoint.IsEmptyPoint(enBridge))
                        {
                            List<Bridge> bridge = new List<Bridge>();
                            List<DiagramPoint> bridgeSegment = new List<DiagramPoint>();
                            Bridge arcs = CreateSegment(stBridge, enBridge, angle, bgedir, pointIndex, conn);
                            BridgeSegment bridgeSeg = new BridgeSegment()
                            {
                                Bridges = bridge,
                                BridgeStartPoint = bridgeSegment,
                                SegmentIndex = segmentIndex
                            };
                            arcs.Target = connector.ID;
                            List<DiagramPoint> stPoints = new List<DiagramPoint>();
                            List<DiagramPoint> edPoints = new List<DiagramPoint>();
                            stPoints.Add(stBridge); edPoints.Add(enBridge);
                            lastBridge.Insert(segmentIndex, bridgeSeg);
                            lastBridge[segmentIndex].Bridges.Add(arcs);
                            lastBridge[segmentIndex].BridgeStartPoint = stPoints;
                            lastBridge[segmentIndex].SegmentIndex = segmentIndex;
                        }
                    }
                }
            }
        }

        private static void InsertingBridge(Connector conn, List<BridgeSegment> lastBridge, Direction bgedir, Connector connector1, int segmentIndex, int pointIndex, DiagramPoint stBridge, DiagramPoint enBridge, double angle)
        {
            DiagramPoint fixedPoint = conn.SourcePoint;
            double fix = Math.Abs(LengthCalculation(fixedPoint, enBridge));
            int insertAt = -1; int count = -1;
            for (int k = 0; k < lastBridge[segmentIndex].Bridges.Count; k++)
            {
                count++;
                Bridge arcSeg = lastBridge[segmentIndex].Bridges[k];
                double var1 = Math.Abs(LengthCalculation(fixedPoint, arcSeg.EndPoint));
                if (fix < var1)
                {
                    insertAt = count; break;
                }
            }
            if (insertAt >= 0)
            {
                Bridge paths = CreateSegment(stBridge, enBridge, angle, bgedir, pointIndex, conn);
                paths.Target = connector1.ID;
                lastBridge[segmentIndex].Bridges.Insert(insertAt, paths);
                lastBridge[segmentIndex].BridgeStartPoint.Insert(insertAt, stBridge);
                lastBridge[segmentIndex].SegmentIndex = segmentIndex;
            }
            else
            {
                Bridge paths = CreateSegment(stBridge, enBridge, angle, bgedir, pointIndex, conn);
                paths.Target = connector1.ID;
                lastBridge[segmentIndex].Bridges.Add(paths);
                lastBridge[segmentIndex].BridgeStartPoint.Add(stBridge);
                lastBridge[segmentIndex].SegmentIndex = segmentIndex;
            }
        }

        private static void FirstBridge(List<BridgeSegment> bridgeList, Connector connector, double bridgeSpacing)
        {
            for (int i = 0; i < bridgeList.Count; i++)
            {
                BridgeSegment bridge = bridgeList[i];
                for (int k = 1; k < bridge.Bridges.Count; k++)
                {
                    if (DiagramPoint.FindLength(bridge.Bridges[k].EndPoint, bridge.Bridges[k - 1].EndPoint) < bridgeSpacing)
                    {
                        bridge.Bridges[k - 1].EndPoint = bridge.Bridges[k].EndPoint;
                        Bridge subBridge = bridge.Bridges[k - 1];
                        string arc = CreateBridgeSegment(
                           subBridge.EndPoint, subBridge.Angle, bridgeSpacing, subBridge.Sweep);
                        bridge.Bridges[k - 1].Path = arc;
                        bridge.Bridges.RemoveRange(k, 1);
                        bridge.BridgeStartPoint.RemoveRange(k, 1); k--;
                    }
                }
                for (int j = 0; j < bridge.Bridges.Count; j++)
                {
                    Bridge subBridge = bridge.Bridges[j];
                    connector.Bridges.Add(subBridge);
                }
            }
        }
        private static Bridge CreateSegment(DiagramPoint st, DiagramPoint end, double angle, Direction direction, double index, Connector conn)
        {
            Bridge path = new Bridge()
            {
                Angle = 0,
                EndPoint = new DiagramPoint(0, 0),
                Target = string.Empty,
                Path = string.Empty,
                SegmentPointIndex = -1,
                StartPoint = new DiagramPoint(0, 0),
                Sweep = 1,
                Rendered = false
            };

            double sweep = SweepDirection(angle, direction);
            string arc = CreateBridgeSegment(end, angle, conn.BridgeSpace, sweep);
            path.Path = arc;
            path.StartPoint = st;
            path.EndPoint = end;
            path.Angle = angle;
            path.SegmentPointIndex = index;
            path.Sweep = sweep;
            return path;
        }

        private static string CreateBridgeSegment(DiagramPoint endPt, double angle, double bridgeSpace, double sweep)
        {
            string path = "A " + bridgeSpace / 2 + " " + bridgeSpace / 2 + " " + angle + " , 1 " + sweep + " " + endPt.X + "," + endPt.Y;
            return path;
        }

        private static double SweepDirection(double angle, Direction bridgeDirection)
        {
            double angle1 = Math.Abs(angle);
            double sweep = 0;
            switch (bridgeDirection)
            {
                case Direction.Top:
                case Direction.Bottom:
                    sweep = 1;
                    if (angle1 >= 0 && angle1 <= 90)
                    {
                        sweep = 0;
                    }
                    break;
                case Direction.Left:
                case Direction.Right:
                    sweep = 1;
                    if (angle < 0 && angle >= -180)
                    {
                        sweep = 0;
                    }
                    break;
            }
            if (bridgeDirection == Direction.Right || bridgeDirection == Direction.Bottom)
            {
                sweep = sweep == 0 ? 1 : 0;
            }
            return sweep;
        }

        private static DiagramPoint GetPointAtLength(double length, List<DiagramPoint> pts)
        {
            double run = 0;
            DiagramPoint pre = null;
            DiagramPoint found = new DiagramPoint(0, 0);
            for (int i = 0; i < pts.Count; i++)
            {
                DiagramPoint pt = pts[i];
                if (pre == null)
                {
                    pre = pt;
                    continue;
                }
                else
                {
                    double l = LengthCalculation(pre, pt);
                    if (run + l > length)
                    {
                        double r = length - run;
                        double deg = DiagramPoint.FindAngle(pre, pt);
                        double x = r * Math.Cos(deg * Math.PI / 180);
                        double y = r * Math.Sin(deg * Math.PI / 180);
                        found = new DiagramPoint(pre.X + x, pre.Y + y);
                        break;
                    }
                    else
                    {
                        run += l;
                    }
                }
                pre = pt;
            }
            return found;
        }

        private static List<DiagramPoint> GetPoints(Connector connector)
        {
            List<DiagramPoint> points = new List<DiagramPoint>();
            if (connector.IntermediatePoints != null && (connector.Type == ConnectorSegmentType.Straight || connector.Type == ConnectorSegmentType.Orthogonal))
            {
                for (int j = 0; j < connector.IntermediatePoints.Count; j++)
                {
                    points.Add(connector.IntermediatePoints[j]);
                }
            }
            return points;
        }

        private static Boolean IntersectsRect(DiagramRect rect1, DiagramRect rect2)
        {
            return ((((rect2.X < (rect1.X + rect1.Width)) && (rect1.X < (rect2.X + rect2.Width)))
                && (rect2.Y < (rect1.Y + rect1.Height))) && (rect1.Y < (rect2.Y + rect2.Height)));
        }

        private static List<DiagramPoint> Intersect(List<DiagramPoint> points1, List<DiagramPoint> points2, bool self, Direction bridgeDirection, bool zOrder)
        {
            if (self && points2.Count >= 2)
            {
                points2.RemoveAt(0);
                points2.RemoveAt(0);
            }
            List<DiagramPoint> points = new List<DiagramPoint>();
            for (int i = 0; i < points1.Count - 1; i++)
            {
                List<DiagramPoint> pt = Inter1(points1[i], points1[i + 1], points2, zOrder, bridgeDirection);
                if (pt.Count > 0)
                {
                    for (int k = 0; k < pt.Count; k++)
                    {
                        points.Add(pt[k]);
                    }
                }
                if (self && points2.Count >= 1)
                {
                    points2.RemoveAt(0);
                }
            }
            return points;
        }
        private static List<DiagramPoint> Inter1(DiagramPoint startPt, DiagramPoint endPt, List<DiagramPoint> pts, bool zOrder, Direction bridgeDirection)
        {
            List<DiagramPoint> points1 = new List<DiagramPoint>();
            for (int i = 0; i < pts.Count - 1; i++)
            {
                DiagramPoint point = DiagramUtil.Intersect2(startPt, endPt, pts[i], pts[i + 1]);
                if (!DiagramPoint.IsEmptyPoint(point))
                {
                    double angle = AngleCalculation(startPt, endPt);
                    double angle1 = AngleCalculation(pts[i], pts[i + 1]);
                    angle = CheckForHorizontalLine(angle);
                    angle1 = CheckForHorizontalLine(angle1);
                    switch (bridgeDirection)
                    {
                        case Direction.Left:
                        case Direction.Right:
                            if (angle > angle1)
                            {
                                points1.Add(point);
                            }
                            break;
                        case Direction.Top:
                        case Direction.Bottom:
                            if (angle < angle1)
                            {
                                points1.Add(point);
                            }
                            break;
                    }
                    if (angle.Equals(angle1) && zOrder)
                    {
                        points1.Add(point);
                    }
                }
            }
            return points1;
        }

        private static double CheckForHorizontalLine(double angle)
        {
            double roundedAngle = Math.Abs(angle);
            double temp;
            if (roundedAngle > 90)
            {
                temp = 180 - roundedAngle;
            }
            else
            {
                temp = roundedAngle;
            }
            return temp;
        }

        private static LengthFraction GetLengthAtFractionPoint(Connector connector, DiagramPoint pointAt)
        {
            double confirm = 100; int pointIndex = -1;
            double fullLength = 0; int segmentIndex = -1;
            int count = 0; double lengthAtFractionPt = 0;
            DiagramPoint pt1 = connector.SourcePoint;
            DiagramPoint previousPoint = pt1;
            List<DiagramPoint> points = new List<DiagramPoint>();
            for (int i = 0; i < connector.IntermediatePoints.Count; i++)
            {
                DiagramPoint point2 = connector.IntermediatePoints[i];
                points.Add(point2);
            }
            for (int j = 0; j < points.Count; j++)
            {
                DiagramPoint pt2 = points[j];
                double suspect = GetSlope(pt2, pt1, pointAt);
                if (suspect < confirm)
                {
                    confirm = suspect;
                    lengthAtFractionPt = fullLength + LengthCalculation(pointAt, previousPoint);
                    segmentIndex = count;
                    pointIndex = j;
                }
                fullLength += DiagramPoint.FindLength(pt2, pt1);
                pt1 = pt2;
                previousPoint = pt2;
            }

            LengthFraction lengthFraction = new LengthFraction()
            {
                LengthFractionIndex = lengthAtFractionPt,
                FullLength = fullLength,
                SegmentIndex = segmentIndex,
                PointIndex = pointIndex
            };
            return lengthFraction;
        }

        private static double GetSlope(DiagramPoint startPt, DiagramPoint endPt, DiagramPoint point)
        {
            double three = 3.0;
            double delX = Math.Abs(startPt.X - endPt.X);
            double delY = Math.Abs(startPt.Y - endPt.Y);
            double lhs = ((point.Y - startPt.Y) / (endPt.Y - startPt.Y));
            double rhs = ((point.X - startPt.X) / (endPt.X - startPt.X));
            if (!double.IsFinite(lhs) || !double.IsFinite(rhs) || double.IsNaN(lhs) || double.IsNaN(rhs))
            {
                if (startPt.X.Equals(endPt.X))
                {
                    if (startPt.Y.Equals(endPt.Y))
                    {
                        return 10000;
                    }
                    else if (((startPt.Y > point.Y) && (point.Y > endPt.Y)) || ((startPt.Y < point.Y) && (point.Y < endPt.Y)))
                    {
                        return Math.Abs(startPt.X - point.X);
                    }
                }
                else if (startPt.Y.Equals(endPt.Y))
                {
                    if (((startPt.X > point.X) && (point.X > endPt.X)) || ((startPt.X < point.X) && (point.X < endPt.X)))
                    {
                        return Math.Abs(startPt.Y - point.Y);
                    }
                }
            }
            else
            {
                if ((startPt.X >= point.X && point.X >= endPt.X) || (startPt.X <= point.X && point.X <= endPt.X) || delX < three)
                {
                    if ((startPt.Y >= point.Y && point.Y >= endPt.Y) || (startPt.Y <= point.Y && point.Y <= endPt.Y) || delY < three)
                    {
                        return Math.Abs(lhs - rhs);
                    }
                }
            }
            return 10000;
        }


        private static double AngleCalculation(DiagramPoint startPt, DiagramPoint endPt)
        {
            double xDiff = startPt.X - endPt.X;
            double yDiff = startPt.Y - endPt.Y;
            return Math.Atan2(yDiff, xDiff) * (180 / Math.PI);
        }

        private static double LengthCalculation(DiagramPoint startPt, DiagramPoint endPt)
        {
            double len = Math.Sqrt(((startPt.X - endPt.X) * (startPt.X - endPt.X)) + ((startPt.Y - endPt.Y) * (startPt.Y - endPt.Y)));
            return len;
        }

    }

    internal class LengthFraction
    {
        internal double LengthFractionIndex;
        internal double FullLength;
        internal int SegmentIndex;
        internal int PointIndex;
    }

    internal class BridgeSegment
    {
        internal List<DiagramPoint> BridgeStartPoint;
        internal List<Bridge> Bridges;
        internal double SegmentIndex;
    }
}

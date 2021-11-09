using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
namespace Syncfusion.Blazor.Diagram
{
    internal class HierarchicalTree : SfBaseComponent
    {
        private const string LEFT = "Left";
        private const string TOP = "Top";
        internal ILayout UpdateLayout(DiagramObjectCollection<Node> nodes, Dictionary<string, IDiagramObject> nameTable, Layout layoutProp, DiagramPoint viewport, string uniqueId = null)
        {
            ILayout layout = new ILayout()
            {
                FixedNode = layoutProp.FixedNode,
                Bounds = layoutProp.Bounds,
                Type = layoutProp.Type,
                AnchorX = 0,
                AnchorY = 0,
                NameTable = nameTable,
                MaxLevel = 0,
                Orientation = layoutProp.Orientation,
                HorizontalSpacing = layoutProp.HorizontalSpacing,
                VerticalSpacing = layoutProp.VerticalSpacing,
                HorizontalAlignment = layoutProp.HorizontalAlignment,
                VerticalAlignment = layoutProp.VerticalAlignment,
                LayoutInfo = layoutProp.LayoutInfo,
                LayoutMargin = layoutProp.Margin,
                Parent = layoutProp.Parent,
                GetLayoutInfo = layoutProp.GetLayoutInfo
            };
            this.DoLayout(layout, nodes, viewport, uniqueId);
            return layout;
        }
        internal static HierarchicalTree Initialize()
        {
            HierarchicalTree hierarchicalTree = new HierarchicalTree();
            return hierarchicalTree;
        }
        internal void DoLayout(ILayout layout, DiagramObjectCollection<Node> nodes, DiagramPoint viewport, string uniqueId = null)
        {
            Node node;
            int i;
            DiagramObjectCollection<Node> rootNodes = new DiagramObjectCollection<Node>();
            for (i = 0; i < nodes.Count; i++)
            {
                node = nodes[i];
                if (node.canShapeLayout)
                {
                    LayoutInfo layoutInfo = SetUpLayoutInfo(layout);
                    layout.GraphNodes.Add(node.ID, layoutInfo);
                    layoutInfo.Tree.HasSubTree = false;
                    if (node.InEdges == null || node.InEdges.Count == 0)
                    {
                        rootNodes.Add(node);
                    }
                    if (node.Data != null && uniqueId != null && node.Data.GetType().Name != "JsonElement" && ((string)node.Data.GetType().GetProperty(uniqueId)?.GetValue(node.Data) == layout.Root))
                    {
                        layout.FirstLevelNodes.Add(node);
                    }
                    else if (node.Data != null && uniqueId != null && node.Data.GetType().Name == "JsonElement")
                    {
                        Dictionary<string, object> data = JsonSerializer.Deserialize<Dictionary<string, object>>(node.Data.ToString());
                        if ((string)data[uniqueId] == layout.Root)
                        {
                            layout.FirstLevelNodes.Add(node);
                        }
                    }
                }
            }
            if (layout.FirstLevelNodes.Count == 0)
            {
                layout.FirstLevelNodes = rootNodes;
            }
            for (i = 0; i < layout.FirstLevelNodes.Count; i++)
            {
                node = layout.FirstLevelNodes[i];
                this.UpdateEdges(layout, node, 1, nodes);
            }
            if (layout.FirstLevelNodes.Count > 0)
            {
                layout.RootNode = layout.FirstLevelNodes[0];
                double x = 0;
                double y = 0;
                double minX = 0;
                double maxY = 0;
                double maxX = 0;
                double minY = 0;
                for (i = 0; i < layout.FirstLevelNodes.Count; i++)
                {
                    Bounds bounds = this.UpdateTree(layout, x, y, layout.FirstLevelNodes[i], null, i > 0 ? layout.FirstLevelNodes[i - 1] : null, false);
                    LayoutInfo rootInfo = layout.GraphNodes[layout.FirstLevelNodes[i].ID];
                    bounds.Y = Math.Min(bounds.Y, rootInfo.Y);
                    bounds.X = Math.Min(bounds.X, rootInfo.X);
                    if (layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1)
                    {
                        y = bounds.Right + layout.HorizontalSpacing;
                    }
                    else
                    {
                        x = bounds.Right + layout.HorizontalSpacing;
                    }
                    if (i == 0) { minX = bounds.X; minY = bounds.Y; maxX = bounds.Right; maxY = bounds.Bottom; }
                    else
                    {
                        minX = Math.Min(minX, bounds.X);
                        minY = Math.Min(minY, bounds.Y);
                        maxX = Math.Max(maxX, bounds.Right);
                        maxY = Math.Max(maxY, bounds.Bottom);
                    }
                }
                Bounds anchorBounds = new Bounds
                {
                    X = minX,
                    Y = minY,
                    Right = maxX,
                    Bottom = maxY
                };
                UpdateAnchor(layout, anchorBounds, viewport);
                for (i = 0; i < layout.FirstLevelNodes.Count; i++)
                {
                    this.UpdateNodes(layout, layout.FirstLevelNodes[i], 0);
                }

                for (i = 0; i < layout.FirstLevelNodes.Count; i++)
                {
                    this.UpdateConnectors(layout, layout.FirstLevelNodes[i], 1);
                }
            }
        }
        internal void UpdateNodes(ILayout layout, Node node, int mod, bool update = false, int dx = 0, int dy = 0)
        {
            if (node != null && node.canShapeLayout)
            {
                int width = Convert.ToInt32(BaseUtil.GetDoubleValue(node.Wrapper.ActualSize.Width));
                int height = Convert.ToInt32(BaseUtil.GetDoubleValue(node.Wrapper.ActualSize.Height));
                double offsetX = layout.AnchorX;
                double offsetY = layout.AnchorY;
                if (layout.Orientation == LayoutOrientation.LeftToRight)
                {
                    offsetX += layout.GraphNodes[node.ID].Y + width / 2;
                    offsetY += layout.GraphNodes[node.ID].X + mod + height / 2;
                }
                else if (layout.Orientation == LayoutOrientation.RightToLeft)
                {
                    offsetX -= layout.GraphNodes[node.ID].Y + width / 2;
                    offsetY += layout.GraphNodes[node.ID].X + mod + height / 2;
                }
                else if (layout.Orientation == LayoutOrientation.TopToBottom)
                {
                    offsetX += layout.GraphNodes[node.ID].X + mod + width / 2;
                    offsetY += layout.GraphNodes[node.ID].Y + height / 2;
                }
                else
                {
                    offsetX += layout.GraphNodes[node.ID].X + mod + width / 2;
                    offsetY -= layout.GraphNodes[node.ID].Y + height / 2;
                }
                if (layout.GraphNodes != null)
                {
                    offsetX += dx;
                    offsetY += dy;
                    node.OffsetX = offsetX;
                    node.OffsetY = offsetY;
                }
                INode objects = new INode
                {
                    ID = node.ID,
                    DifferenceX = Convert.ToInt32(BaseUtil.GetDoubleValue(offsetX - node.OffsetX)),
                    DifferenceY = Convert.ToInt32(BaseUtil.GetDoubleValue(offsetY - node.OffsetY))
                };
                layout.Objects.Add(objects);
                List<Node> list = new List<Node>();
                int i;
                Node child;
                if (HasChild(layout, node) > 0)
                {
                    for (i = 0; i < layout.GraphNodes[node.ID].Tree.Children.Count; i++)
                    {
                        child = layout.NameTable[layout.GraphNodes[node.ID].Tree.Children[i]] as Node;
                        this.UpdateNodes(layout, child, mod + (layout.GraphNodes[node.ID].SubTreeTranslation | 0), update, dx, dy);
                        list.Add(child);
                    }
                }

                if (layout.GraphNodes[node.ID].Tree.Assistants.Count > 0)
                {
                    for (i = 0; i < layout.GraphNodes[node.ID].Tree.Assistants.Count; i++)
                    {
                        child = layout.NameTable[layout.GraphNodes[node.ID].Tree.Assistants[i]] as Node;
                        this.UpdateNodes(layout, child, mod + (layout.GraphNodes[node.ID].SubTreeTranslation | 0), false, dx, dy);
                    }
                }
            }
        }
        internal void UpdateConnectors(ILayout layout, Node node, int level)
        {
            int i;
            Node target;
            Connector conn;
            LayoutInfo info = layout.GraphNodes.Keys.Contains(node.ID) ? layout.GraphNodes[node.ID] : null;
            if (node.OutEdges.Count > 0)
            {
                for (i = 0; i < node.OutEdges.Count; i++)
                {
                    conn = layout.NameTable[node.OutEdges[i]] as Connector;
                    if (conn != null)
                    {
                        target = layout.NameTable[conn.TargetID] as Node;
                        if (conn.IsVisible)
                        {
                            conn.Visited = true;
                            if (info != null && info.Tree.Children.IndexOf(conn.TargetID) != -1)
                            {
                                conn.SegmentCollection = new DiagramObjectCollection<ConnectorSegment>();
                                if (layout.Type == LayoutType.OrganizationalChart && conn.Type == ConnectorSegmentType.Orthogonal)
                                {
                                    UpdateSegments(layout, conn, node, target, i);
                                }
                            }

                            if (target != null && (target.IsExpanded || HasChild(layout, target) != -1))
                            {
                                this.UpdateConnectors(layout, target, level + 1);
                            }
                        }
                    }
                }
            }
            if (info != null && info.Tree.Assistants.Count > 0)
            {
                for (i = 0; i < info.Tree.Assistants.Count; i++)
                {
                    target = layout.NameTable[info.Tree.Assistants[i]] as Node;
                    if (target != null)
                    {
                        conn = layout.NameTable[target.InEdges[0]] as Connector;
                        Get3Points(layout, node, target, conn);
                        if (target.IsExpanded || HasChild(layout, target) != -1)
                        {
                            this.UpdateConnectors(layout, target, level + 1);
                        }
                    }
                }
            }
        }
        private static void UpdateSegments(ILayout layout, Connector conn, Node node, Node target, int i)
        {
            LayoutInfo info = layout.GraphNodes[node.ID];
            if (info.Tree.Assistants.Count > 0)
            {
                UpdateSegmentsForHorizontalOrientation(layout, node, target, conn);
            }
            else
            {
                if (info.Tree.Orientation == Orientation.Horizontal && info.Tree.AlignmentType == SubTreeAlignmentType.Balanced)
                {
                    UpdateSegmentsForBalancedTree(layout, conn, node, target, i);
                }
                else
                {
                    if (info.Tree.Orientation == Orientation.Horizontal)
                    {
                        UpdateSegmentsForHorizontalOrientation(layout, node, target, conn);
                    }
                    else
                    {
                        if (info.Tree.Offset < 5)
                        {
                            Get5Points(layout, node, target, conn);
                        }
                        else
                        {
                            Get3Points(layout, node, target, conn);
                        }
                    }
                }
            }
        }
        private static void UpdateSegmentsForBalancedTree(ILayout layout, Connector connector, Node node, Node target, int i)
        {
            LayoutInfo info = layout.GraphNodes[node.ID];
            if (info.Tree.Children.Count == 5 && i > 2)
            {
                string relative = info.Tree.Children[1];
                if (double.IsNaN(layout.GraphNodes[relative].TreeWidth))
                {
                    layout.GraphNodes[relative].TreeWidth = BaseUtil.GetDoubleValue((layout.NameTable[relative] as NodeBase).Wrapper.ActualSize.Width);
                }
                int factor = i != 3 ? 1 : -1;
                if (layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1)
                {
                    _ = (layout.NameTable[relative] as NodeBase).Wrapper.OffsetY - layout.GraphNodes[relative].TreeWidth / 2 -
                        (layout.VerticalSpacing * factor / 2);
                }
                else
                {
                    _ = (layout.NameTable[relative] as NodeBase).Wrapper.OffsetX +
                        layout.GraphNodes[relative].TreeWidth / 2 + (layout.HorizontalSpacing * factor) / 2;
                }
                GetSegmentsForMultipleRows(layout, node, target, connector);
            }
            else
            {
                if (info.Tree.Children.Count > 5)
                {
                    if (i < 4 || i < info.Tree.Rows)
                    {
                        GetSegmentsForMultipleRows(layout, node, target, connector);
                    }
                    else
                    {
                        UpdateSegmentsForHorizontalOrientation(layout, node, target, connector);
                    }
                }
                else if (info.Tree.Children.Count == 4)
                {
                    if (i < 2 || i < info.Tree.Rows)
                    {
                        GetSegmentsForMultipleRows(layout, node, target, connector);
                    }
                    else
                    {
                        UpdateSegmentsForHorizontalOrientation(layout, node, target, connector);
                    }
                }
                else
                {
                    GetSegmentsForMultipleRows(layout, node, target, connector);
                }
            }
        }
        private static void Get3Points(ILayout layout, Node node, Node target, Connector connector)
        {
            List<DiagramPoint> points = new List<DiagramPoint>();
            DiagramRect nodeBounds = GetBounds(node);
            DiagramRect targetBounds = GetBounds(target);
            if (layout.Orientation.ToString().IndexOf(TOP, StringComparison.InvariantCulture) != -1)
            {
                DiagramPoint startingPoint = layout.Orientation.ToString().IndexOf(TOP, StringComparison.InvariantCulture) == 0 ? nodeBounds.BottomCenter : nodeBounds.TopCenter;
                DiagramPoint endPoint = node.OffsetX > target.OffsetX ? targetBounds.MiddleRight : targetBounds.MiddleLeft;
                points.AddRange(new List<DiagramPoint>() { startingPoint, new DiagramPoint() { X = nodeBounds.BottomCenter.X, Y = endPoint.Y }, endPoint });
            }
            else
            {
                DiagramPoint startingPoint = layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) == 0 ? nodeBounds.MiddleRight : nodeBounds.MiddleLeft;
                DiagramPoint endPoint = node.OffsetY > target.OffsetY ? targetBounds.BottomCenter : targetBounds.TopCenter;
                points.AddRange(new List<DiagramPoint>() { startingPoint, new DiagramPoint() { X = targetBounds.BottomCenter.X, Y = nodeBounds.MiddleRight.Y }, endPoint });
            }
            GetSegmentsFromPoints(points, connector);
        }
        private static void GetSegmentsFromPoints(List<DiagramPoint> points, Connector connector)
        {
            DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>();
            if (connector.Type == ConnectorSegmentType.Orthogonal)
            {
                for (int i = 0; i < points.Count - 2; i++)
                {
                    OrthogonalSegment segment = new OrthogonalSegment
                    {
                        Type = ConnectorSegmentType.Orthogonal,
                        AllowDrag = true,
                        Direction = DiagramPoint.Direction(points[i], points[i + 1]),
                        Length = DiagramPoint.DistancePoints(points[i], points[i + 1])
                    };
                    segments.Add(segment);
                }
                connector.Segments = new DiagramObjectCollection<ConnectorSegment>(segments);
            }
        }
        private static void Get5Points(ILayout layout, Node node, Node target, Connector connector)
        {
            List<DiagramPoint> points = new List<DiagramPoint>();
            DiagramRect nodeBounds = GetBounds(node);
            DiagramRect targetBounds = GetBounds(target);
            DiagramPoint startingPoint; DiagramPoint endPoint;
            double horizontalSpacing; double verticalSpacing;
            if (layout.Orientation.ToString().IndexOf(TOP, StringComparison.InvariantCulture) != -1)
            {
                startingPoint = (node.OffsetY < target.OffsetY) ? nodeBounds.BottomCenter : nodeBounds.TopCenter;
                verticalSpacing = layout.VerticalSpacing / 4 * ((node.OffsetY < target.OffsetY) ? 1 : -1);
                horizontalSpacing = layout.HorizontalSpacing / 2 * ((node.OffsetX > target.OffsetX) ? 1 : -1);
                endPoint = (node.OffsetX > target.OffsetX) ? targetBounds.MiddleRight : targetBounds.MiddleLeft;
                points.AddRange(new List<DiagramPoint>()
                {
                    startingPoint,
                    new DiagramPoint() { X= startingPoint.X, Y= startingPoint.Y + verticalSpacing },
                    new DiagramPoint() { X= endPoint.X + horizontalSpacing, Y= startingPoint.Y + verticalSpacing },
                    new DiagramPoint()   { X= endPoint.X + horizontalSpacing, Y= endPoint.Y },
                    endPoint
                });
            }
            else
            {
                startingPoint = (node.OffsetX > target.OffsetX) ? nodeBounds.MiddleLeft : nodeBounds.MiddleRight;
                endPoint = node.OffsetY > target.OffsetY ? targetBounds.BottomCenter : targetBounds.TopCenter;
                horizontalSpacing = layout.HorizontalSpacing / 4 * ((node.OffsetX < target.OffsetX) ? 1 : -1);
                verticalSpacing = layout.VerticalSpacing / 2 * ((node.OffsetY > target.OffsetY) ? 1 : -1);
                points.AddRange(new List<DiagramPoint>()
                {
                    startingPoint,
                      new DiagramPoint() { X = startingPoint.X + horizontalSpacing, Y = startingPoint.Y },
                       new DiagramPoint() { X = startingPoint.X + horizontalSpacing, Y = startingPoint.Y + verticalSpacing },
                       new DiagramPoint() { X = endPoint.X, Y = startingPoint.Y + verticalSpacing },
                    endPoint
                });
            }
            GetSegmentsFromPoints(points, connector);
        }
        private static List<DiagramPoint> UpdateSegmentsForHorizontalOrientation(ILayout layout, Node node, Node target, Connector connector)
        {
            List<DiagramPoint> points = new List<DiagramPoint>();
            DiagramPoint point2;
            OrthogonalSegment segment;
            DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>();
            DiagramRect nodeBounds = GetBounds(node);
            DiagramRect targetBounds = GetBounds(target);
            switch (layout.Orientation)
            {
                case LayoutOrientation.TopToBottom:
                    point2 = new DiagramPoint() { X = nodeBounds.BottomCenter.X, Y = (targetBounds.TopCenter.Y - layout.VerticalSpacing / 2) };
                    segment = new OrthogonalSegment
                    {
                        Type = ConnectorSegmentType.Orthogonal,
                        AllowDrag = true,
                        Direction = DiagramPoint.Direction(nodeBounds.BottomCenter, point2),
                        Length = DiagramPoint.DistancePoints(nodeBounds.BottomCenter, point2)
                    };
                    segments.Add(segment);
                    break;
                case LayoutOrientation.BottomToTop:
                    point2 = new DiagramPoint() { X = nodeBounds.TopCenter.X, Y = (targetBounds.BottomCenter.Y + layout.VerticalSpacing / 2) };
                    segment = new OrthogonalSegment
                    {
                        Type = ConnectorSegmentType.Orthogonal,
                        AllowDrag = true,
                        Direction = DiagramPoint.Direction(nodeBounds.TopCenter, point2),
                        Length = DiagramPoint.DistancePoints(nodeBounds.TopCenter, point2)
                    };
                    segments.Add(segment);
                    break;
                case LayoutOrientation.LeftToRight:
                    point2 = new DiagramPoint() { X = (targetBounds.MiddleLeft.X - layout.VerticalSpacing / 2), Y = nodeBounds.MiddleRight.Y };
                    segment = new OrthogonalSegment
                    {
                        Type = ConnectorSegmentType.Orthogonal,
                        AllowDrag = true,
                        Direction = DiagramPoint.Direction(nodeBounds.MiddleRight, point2),
                        Length = DiagramPoint.DistancePoints(nodeBounds.MiddleRight, point2)
                    };
                    segments.Add(segment);
                    if (targetBounds.Center.Y != nodeBounds.Center.Y)
                    {
                        DiagramPoint point3 = new DiagramPoint() { X = (targetBounds.MiddleLeft.X - layout.VerticalSpacing / 2), Y = targetBounds.MiddleLeft.Y };
                        segment = new OrthogonalSegment
                        {
                            Type = ConnectorSegmentType.Orthogonal,
                            AllowDrag = true,
                            Direction = DiagramPoint.Direction(point2, point3),
                            Length = DiagramPoint.DistancePoints(point2, point3)
                        };
                        segments.Add(segment);
                    }
                    break;
                case LayoutOrientation.RightToLeft:
                    if (layout != null)
                    {
                        point2 = new DiagramPoint()
                        {
                            X = (targetBounds.MiddleRight.X + layout.VerticalSpacing / 2),
                            Y = nodeBounds.MiddleRight.Y
                        };
                        segment = new OrthogonalSegment
                        {
                            Type = ConnectorSegmentType.Orthogonal,
                            AllowDrag = true,
                            Direction = DiagramPoint.Direction(nodeBounds.MiddleLeft, point2),
                            Length = DiagramPoint.DistancePoints(nodeBounds.MiddleLeft, point2)
                        };
                        segments.Add(segment);
                        if (!targetBounds.Center.Y.Equals(nodeBounds.Center.Y))
                        {
                            DiagramPoint point = new DiagramPoint()
                            {
                                X = (targetBounds.MiddleRight.X + layout.VerticalSpacing / 2),
                                Y = targetBounds.MiddleLeft.Y
                            };
                            segment = new OrthogonalSegment
                            {
                                Type = ConnectorSegmentType.Orthogonal,
                                AllowDrag = true,
                                Direction = DiagramPoint.Direction(point2, point),
                                Length = DiagramPoint.DistancePoints(point2, point)
                            };
                            segments.Add(segment);
                        }
                    }

                    break;
            }
            connector.Segments = new DiagramObjectCollection<ConnectorSegment>(segments);
            return points;
        }
        private static void GetSegmentsForMultipleRows(ILayout layout, Node node, Node target, Connector connector)
        {
            DiagramObjectCollection<ConnectorSegment> segments = new DiagramObjectCollection<ConnectorSegment>(); DiagramPoint point; OrthogonalSegment segment;
            DiagramRect targetBounds = GetBounds(target);
            DiagramRect nodeBounds = GetBounds(node);
            switch (layout.Orientation)
            {
                case LayoutOrientation.TopToBottom:
                    point = new DiagramPoint() { X = nodeBounds.BottomCenter.X, Y = (nodeBounds.BottomCenter.Y + layout.VerticalSpacing / 4) };
                    segment = new OrthogonalSegment
                    {
                        Type = ConnectorSegmentType.Orthogonal,
                        AllowDrag = true,
                        Direction = DiagramPoint.Direction(nodeBounds.BottomCenter, point),
                        Length = DiagramPoint.DistancePoints(nodeBounds.BottomCenter, point)
                    };
                    segments.Add(segment);
                    break;
                case LayoutOrientation.BottomToTop:
                    point = new DiagramPoint() { X = nodeBounds.BottomCenter.X, Y = (nodeBounds.TopCenter.Y - layout.VerticalSpacing / 4) };
                    segment = new OrthogonalSegment
                    {
                        Type = ConnectorSegmentType.Orthogonal,
                        AllowDrag = true,
                        Direction = DiagramPoint.Direction(nodeBounds.TopCenter, point),
                        Length = DiagramPoint.DistancePoints(nodeBounds.TopCenter, point)
                    };
                    segments.Add(segment);
                    break;
                case LayoutOrientation.LeftToRight:
                    point = new DiagramPoint() { X = (nodeBounds.MiddleRight.X + layout.VerticalSpacing / 4), Y = nodeBounds.MiddleRight.Y };
                    segment = new OrthogonalSegment
                    {
                        Type = ConnectorSegmentType.Orthogonal,
                        AllowDrag = true,
                        Direction = DiagramPoint.Direction(nodeBounds.MiddleRight, point),
                        Length = DiagramPoint.DistancePoints(nodeBounds.MiddleRight, point)
                    };
                    segments.Add(segment);
                    if (!targetBounds.Center.Y.Equals(nodeBounds.Center.Y))
                    {
                        DiagramPoint point3 = new DiagramPoint() { X = (nodeBounds.MiddleRight.X + layout.VerticalSpacing / 4), Y = targetBounds.MiddleLeft.Y };
                        segment = new OrthogonalSegment
                        {
                            Type = ConnectorSegmentType.Orthogonal,
                            AllowDrag = true,
                            Direction = DiagramPoint.Direction(point, point3),
                            Length = DiagramPoint.DistancePoints(point, point3)
                        };
                        segments.Add(segment);
                    }
                    break;
                case LayoutOrientation.RightToLeft:
                    if (layout != null)
                    {
                        point = new DiagramPoint()
                        { X = (nodeBounds.MiddleLeft.X - layout.VerticalSpacing / 4), Y = nodeBounds.MiddleRight.Y };
                        segment = new OrthogonalSegment
                        {
                            Type = ConnectorSegmentType.Orthogonal,
                            AllowDrag = true,
                            Direction = DiagramPoint.Direction(nodeBounds.MiddleLeft, point),
                            Length = DiagramPoint.DistancePoints(nodeBounds.MiddleLeft, point)
                        };
                        segments.Add(segment);
                        if (targetBounds.Center.Y != nodeBounds.Center.Y)
                        {
                            point = new DiagramPoint()
                            {
                                X = (nodeBounds.MiddleLeft.X - layout.VerticalSpacing / 4),
                                Y = targetBounds.MiddleLeft.Y
                            };
                            segment = new OrthogonalSegment
                            {
                                Type = ConnectorSegmentType.Orthogonal,
                                AllowDrag = true,
                                Direction = DiagramPoint.Direction(point, point),
                                Length = DiagramPoint.DistancePoints(point, point)
                            };
                            segments.Add(segment);
                        }
                    }

                    break;
            }
            connector.Segments = new DiagramObjectCollection<ConnectorSegment>(segments);
        }

        private static DiagramRect GetBounds(Node node)
        {
            double x = node.OffsetX - BaseUtil.GetDoubleValue(node.Wrapper.ActualSize.Width) * node.Pivot.X;
            double y = node.OffsetY - BaseUtil.GetDoubleValue(node.Wrapper.ActualSize.Height) * node.Pivot.Y;
            return new DiagramRect(x, y, BaseUtil.GetDoubleValue(node.Wrapper.ActualSize.Width), BaseUtil.GetDoubleValue(node.Wrapper.ActualSize.Height));
        }

        internal static void UpdateAnchor(ILayout layout, Bounds bounds, DiagramPoint viewPort)
        {
            DiagramRect viewPortBounds = new DiagramRect
            {
                X = 0,
                Y = 0,
                Width = viewPort.X,
                Height = viewPort.Y
            };
            DiagramRect layoutBounds = layout.Bounds ?? viewPortBounds;
            LayoutOrientation orientation = layout.Orientation;
            if (!string.IsNullOrEmpty(layout.FixedNode))
            {
                Node fixedNode = layout.NameTable[layout.FixedNode] as Node;
                if (fixedNode != null)
                {
                    double width = BaseUtil.GetDoubleValue(fixedNode.Wrapper.ActualSize.Width);
                    double height = BaseUtil.GetDoubleValue(fixedNode.Wrapper.ActualSize.Height);
                    layout.AnchorX = fixedNode.OffsetX;
                    layout.AnchorY = fixedNode.OffsetY;
                    var pivot = fixedNode.Pivot;
                    layout.AnchorX += layout.Orientation == LayoutOrientation.RightToLeft ? width * pivot.X : -width * pivot.X;
                    layout.AnchorY += layout.Orientation == LayoutOrientation.BottomToTop ? height * pivot.Y : -height * pivot.Y;
                }
                Node node = fixedNode;
                double mod = 0;
                while (node != null && node.InEdges.Count > 0)
                {
                    node = GetParentNode(layout, node);
                    mod += layout.GraphNodes[node.ID].SubTreeTranslation;
                }
                double yValue;
                if (layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1)
                {
                    yValue = layout.GraphNodes[fixedNode.ID].Y;
                    if (orientation == LayoutOrientation.LeftToRight)
                        layout.AnchorX -= yValue;
                    else
                        layout.AnchorX += yValue;
                    layout.AnchorY -= layout.GraphNodes[fixedNode.ID].X + mod;
                }
                else
                {
                    yValue = layout.GraphNodes[fixedNode.ID].Y;
                    layout.AnchorX -= layout.GraphNodes[fixedNode.ID].X + mod;
                    if (orientation == LayoutOrientation.TopToBottom)
                        layout.AnchorY -= yValue;
                    else
                        layout.AnchorY += yValue;
                }
            }
            else
            {
                if (orientation == LayoutOrientation.TopToBottom || orientation == LayoutOrientation.BottomToTop)
                {
                    switch (layout.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            layout.AnchorX = Convert.ToInt32((layoutBounds.X - bounds.X) + layout.LayoutMargin.Left);
                            break;
                        case HorizontalAlignment.Right:
                            layout.AnchorX = Convert.ToInt32(layoutBounds.X + layoutBounds.Width - layout.LayoutMargin.Right - bounds.Right);
                            break;
                        case HorizontalAlignment.Auto:
                        case HorizontalAlignment.Center:
                            layout.AnchorX = Convert.ToInt32(layoutBounds.X + layoutBounds.Width / 2 - (bounds.X + bounds.Right) / 2);
                            break;
                    }
                    switch (layout.VerticalAlignment)
                    {
                        case VerticalAlignment.Auto:
                        case VerticalAlignment.Top:
                            double top;
                            top = Convert.ToInt32(layoutBounds.Y + layout.LayoutMargin.Top);
                            layout.AnchorY = orientation == LayoutOrientation.TopToBottom ? top : bounds.Bottom + top;
                            break;
                        case VerticalAlignment.Bottom:
                            int bottom;
                            bottom = Convert.ToInt32(layoutBounds.Y + layoutBounds.Height - layout.LayoutMargin.Bottom);
                            layout.AnchorY = orientation == LayoutOrientation.TopToBottom ? bottom - bounds.Bottom : bottom;
                            break;
                        case VerticalAlignment.Center:
                            int center;
                            center = Convert.ToInt32(layoutBounds.Y + layoutBounds.Height / 2);
                            layout.AnchorY = layout.Orientation == LayoutOrientation.TopToBottom ?
                                center - (bounds.Y + bounds.Bottom) / 2 : center + (bounds.Y + bounds.Bottom) / 2;
                            break;
                    }
                }
                else
                {
                    switch (layout.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Auto:
                        case HorizontalAlignment.Left:
                            int left;
                            left = Convert.ToInt32(layoutBounds.X + layout.LayoutMargin.Left);
                            layout.AnchorX = orientation == LayoutOrientation.LeftToRight ? left : bounds.Bottom + left;
                            break;
                        case HorizontalAlignment.Right:
                            int right;
                            right = Convert.ToInt32(layoutBounds.X + layoutBounds.Width - layout.LayoutMargin.Right);
                            layout.AnchorX = orientation == LayoutOrientation.LeftToRight ? right - bounds.Bottom : right;
                            break;
                        case HorizontalAlignment.Center:
                            int center;
                            center = Convert.ToInt32(layoutBounds.Width / 2 + layoutBounds.X);
                            layout.AnchorX = layout.Orientation == LayoutOrientation.LeftToRight ?
                                center - (bounds.Y + bounds.Bottom) / 2 : center + (bounds.Y + bounds.Bottom) / 2;
                            break;
                    }
                    switch (layout.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            layout.AnchorY = Convert.ToInt32(layoutBounds.Y + layout.LayoutMargin.Top - bounds.X);
                            break;
                        case VerticalAlignment.Auto:
                        case VerticalAlignment.Center:
                            layout.AnchorY = Convert.ToInt32(layoutBounds.Y + layoutBounds.Height / 2 - (bounds.Right + bounds.X) / 2);
                            break;
                        case VerticalAlignment.Bottom:
                            layout.AnchorY = Convert.ToInt32(layoutBounds.Y + layoutBounds.Height - layout.LayoutMargin.Bottom - bounds.Right);
                            break;
                    }
                }
            }
        }
        private static Node GetParentNode(ILayout layout, Node node)
        {
            //Return the first parent node
            return layout.NameTable[(layout.NameTable[node.InEdges[0]] as Connector)?.SourceID] as Node;
        }
        private static LayoutInfo SetUpLayoutInfo(ILayout layout)
        {
            LayoutInfo info = new LayoutInfo();

            if (layout.Type == LayoutType.OrganizationalChart)
            {
                info.Tree.Orientation = Orientation.Vertical;
                info.Tree.AlignmentType = SubTreeAlignmentType.Alternate;
                info.Tree.Offset = 20;
                info.Tree.EnableRouting = true;
            }
            else
            {
                info.Tree.Orientation = Orientation.Horizontal;
                info.Tree.AlignmentType = SubTreeAlignmentType.Center;
                info.Tree.EnableRouting = true;
            }
            info.Tree.Level = 0;
            info.Translate = true;
            return info;
        }
        internal void UpdateEdges(ILayout layout, Node node, int depth, DiagramObjectCollection<Node> nodes)
        {
            LayoutInfo layoutInfo = layout.GraphNodes[node.ID];
            if (node.OutEdges != null && node.OutEdges.Count > 0 && node.IsExpanded)
            {
                for (int j = 0; j < node.OutEdges.Count; j++)
                {
                    if (layout.NameTable[node.OutEdges[j]] is Connector connector && layout.NameTable[connector.TargetID] is Node { canShapeLayout: true } edge)
                    {
                        if (!layoutInfo.Tree.Children.Contains(edge.ID))
                        {
                            layoutInfo.Tree.Children.Add(edge.ID);
                        }
                        if (edge.OutEdges != null && edge.OutEdges.Count > 0 && edge.IsExpanded)
                        {
                            layoutInfo.Tree.HasSubTree = true;
                        }
                        this.UpdateEdges(layout, edge, depth + 1, nodes);
                    }
                }
            }
            layoutInfo.Tree.Level = depth;
            if (layoutInfo.Tree.HasSubTree)
            {
                layoutInfo.Tree.Orientation = Orientation.Horizontal;
                layoutInfo.Tree.AlignmentType = SubTreeAlignmentType.Center;
            }
            if ((layout.GetLayoutInfo != null || layout.LayoutInfo != null) && layout.Type == LayoutType.OrganizationalChart)
            {
                if (layout.GetLayoutInfo != null)
                    _ = layout.GetLayoutInfo(node, layoutInfo.Tree);
                if (layoutInfo.Tree.AlignmentType == SubTreeAlignmentType.Balanced && layoutInfo.Tree.HasSubTree)
                {
                    layoutInfo.Tree.AlignmentType = SubTreeAlignmentType.Center; layoutInfo.Tree.Orientation = Orientation.Horizontal;
                }
            }
            if (layout.Level > 0 && layoutInfo.Tree.AlignmentType != SubTreeAlignmentType.Alternate && depth >= layout.Level)
            {
                layoutInfo.Tree.HasSubTree = false;
            }
        }
        internal Bounds UpdateTree(ILayout layout, double x, double y, Node node, int? level, Node prev = null, bool doNotUpdate = false)
        {
            int? d = null;
            DiagramRect dimensions = GetDimensions(layout, node, x, y, level);
            LayoutInfo info = layout.GraphNodes[node.ID];
            layout.MaxLevel = layout.MaxLevel > level ? layout.MaxLevel : level ?? 0;
            int lev = level ?? 0;
            int hasChild = HasChild(layout, node);
            if (hasChild == 0 && info.Tree.Assistants.Count == 0)
            {
                node.TreeBounds = UpdateLeafNode(layout, node, prev, dimensions, level, doNotUpdate);
                return node.TreeBounds;
            }
            else
            {
                Bounds treeBounds = new Bounds();
                Bounds shapeBounds = new Bounds();
                double bottom = dimensions.Y + dimensions.Height + layout.VerticalSpacing;
                if (info.Tree.Assistants.Count > 0)
                {
                    LayoutObject obj = SetDepthSpaceForAssitants(layout, node, bottom, level, layout.VerticalSpacing);
                    lev = obj.Level;
                    bottom = obj.Bottom;
                }
                if (info.Tree.Assistants.Count == 0 && info.Tree.Orientation != Orientation.Horizontal)
                {
                    bottom = dimensions.Y + dimensions.Height + layout.VerticalSpacing / 2;
                }
                if (info.Tree.Children.Count > 0)
                {
                    if (info.Tree.Orientation == Orientation.Horizontal && (info.Tree.AlignmentType != SubTreeAlignmentType.Balanced || info.Tree.Children.Count == 1))
                    {
                        treeBounds = this.UpdateHorizontalTree(layout, node, dimensions.X, bottom, lev);
                    }
                    else if (info.Tree.AlignmentType == SubTreeAlignmentType.Balanced)
                    {
                        treeBounds = this.UpdateHorizontalTreeWithMultipleRows(layout, node, dimensions.X, bottom, lev);
                    }
                    else
                    {
                        treeBounds = this.UpdateVerticalTree(layout, node, dimensions.X, bottom, lev, doNotUpdate);
                    }
                }

                if (!(info.Y > dimensions.Y))
                {
                    info.Y = dimensions.Y;
                }
                if (info.Mid != null)
                {
                    x = info.Mid.Value;
                }
                if (info.Tree.Assistants.Count > 0)
                {
                    double? space = x != 0 ? x : dimensions.X;
                    Bounds asstBounds = this.SetBreadthSpaceForAssistants(layout, node, dimensions, space, level);
                    if (!(hasChild > 0))
                    {
                        Bounds levelBounds = asstBounds;
                        x = (levelBounds.X + levelBounds.Right) / 2 - dimensions.Width / 2;
                        treeBounds = levelBounds;
                    }
                    d = asstBounds?.CanMoveBy;
                }
                info.X = x;
                if (!info.Translate) { info.TreeWidth = treeBounds.Right - treeBounds.X; }
                {
                    shapeBounds.X = x;
                    shapeBounds.Y = dimensions.Y;
                    shapeBounds.Right = x + dimensions.Width;
                    shapeBounds.Bottom = dimensions.Y + dimensions.Height;
                }
                TranslateInfo translateInfo = new TranslateInfo
                {
                    Layout = layout,
                    Shape = node,
                    ShapeBounds = shapeBounds,
                    TreeBounds = treeBounds,
                    Dim = dimensions,
                    Level = level ?? 0
                };
                TranslateSubTree(translateInfo, d, prev != null, doNotUpdate);
                node.TreeBounds = treeBounds;
                return treeBounds;
            }
        }
        private Bounds SetBreadthSpaceForAssistants(ILayout layout, Node shape, DiagramRect dim, double? space, int? level)
        {
            int i;
            var info = layout.GraphNodes[shape.ID];
            var lev = level;
            int? diff = null;
            Bounds levelBounds = new Bounds();
            for (i = 0; i < info.Tree.Assistants.Count; i++)
            {
                LayoutInfo asst = layout.GraphNodes[info.Tree.Assistants[i]];
                if (asst != null)
                {
                    if (layout.NameTable[info.Tree.Assistants[i]] is Node asstElement)
                    {
                        double asstWidth = BaseUtil.GetDoubleValue(asstElement.Wrapper.ActualSize.Width);
                        if (layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1)
                        {
                            asstWidth = BaseUtil.GetDoubleValue(asstElement.Wrapper.ActualSize.Height);
                        }
                        double left;
                        if (i % 2 == 0)
                        {
                            left = BaseUtil.GetDoubleValue(space) + dim.Width / 2 - 20 - asstWidth;
                        }
                        else
                        {
                            left = BaseUtil.GetDoubleValue(space) + dim.Width / 2 + 20;
                        }
                        Bounds bounds = this.UpdateTree(layout, left, asst.Y, layout.NameTable[info.Tree.Assistants[i]] as Node, lev + 1);
                        if (!(HasChild(layout, shape) > 0))
                        {
                            if (i == 0)
                            {
                                levelBounds = bounds;
                            }
                            else
                            {
                                UniteRects(levelBounds, bounds);
                            }
                        }
                        if (i % 2 == 0 && asst.PrevBounds != null)
                        {
                            diff = diff == null ? asst.CanMoveBy : Math.Min((int)BaseUtil.GetDoubleValue(diff), (int)BaseUtil.GetDoubleValue(asst.CanMoveBy));
                        }
                        if (i % 2 == 1 || i == info.Tree.Assistants.Count - 1)
                        {
                            List<int> intersect = FindIntersectingLevels(layout, bounds, lev + 1, null);
                            UpdateRearBounds(layout, null, new List<Bounds>() { bounds }, intersect);
                            lev++;
                        }
                    }
                }
            }
            if (levelBounds != null)
            {
                levelBounds.CanMoveBy = (int)BaseUtil.GetDoubleValue(diff);
            }
            return levelBounds;
        }
        private Bounds UpdateVerticalTree(ILayout layout, Node shape, double x, double y, int level, bool doNotUpdate)
        {
            //declarations
            Bounds prevBounds = null; Bounds bounds = null;
            Bounds evenBounds = null;

            LayoutInfo info = layout.GraphNodes[shape.ID];
            Node firstChild = layout.NameTable[info.Tree.Children[0]] as Node;
            bool h = layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1;
            double factor = info.Tree.AlignmentType == SubTreeAlignmentType.Left ? -1 : 0;
            double bottom = y;
            int lev = level; int i;
            SubTreeAlignmentType? type = null; List<Bounds> levels = new List<Bounds>();
            List<Bounds> oddLevels = new List<Bounds>(); int? canMoveBy = null;
            for (i = 0; i < info.Tree.Children.Count; i++)
            {
                if (info.Tree.AlignmentType == SubTreeAlignmentType.Alternate)
                {
                    //arrange at both left and right
                    type = (i % 2 == 0 && info.Tree.Children.Count > 2) ? SubTreeAlignmentType.Left : SubTreeAlignmentType.Right;
                    factor = (i % 2 == 0 && info.Tree.Children.Count > 2) ? -1 : 0;
                }
                double right = x + FindOffset(layout, shape, info, type);
                Node child = layout.NameTable[info.Tree.Children[i]] as Node;
                double childWidth = h ? BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Height) : BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Width);
                double childHeight = h ? BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Width) : BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Height);
                //Update sub tree
                Bounds childBounds = this.UpdateTree(layout, right + factor * childWidth, bottom, child, level + 1, null, true);
                LayoutInfo childInfo = layout.GraphNodes[child.ID];
                Bounds actBounds = new Bounds() { X = childInfo.X, Y = childInfo.Y, Right = childInfo.X + childWidth, Bottom = childInfo.Y + childHeight };
                if (i == 0)
                {
                    UniteRects(childBounds, actBounds);
                    bounds = childBounds;
                }
                else
                {
                    UniteRects(bounds, childBounds);
                }
                //Check and adjust the space left from previous subtree/sibling
                if (childInfo.PrevBounds != null && !(info.Tree.AlignmentType == SubTreeAlignmentType.Alternate && i % 2 == 1 && info.Tree.Children.Count > 2))
                {
                    canMoveBy = canMoveBy != null ? Math.Min((int)BaseUtil.GetDoubleValue(childInfo.CanMoveBy), (int)BaseUtil.GetDoubleValue(canMoveBy)) : childInfo.CanMoveBy;
                }
                //Max level of the subtree node
                info.MaxLevel = Math.Max(info.MaxLevel, childInfo.MaxLevel);
                if (!(info.Tree.AlignmentType == SubTreeAlignmentType.Alternate && info.Tree.Children.Count > 2 && i % 2 == 0))
                {
                    if (info.Tree.AlignmentType == SubTreeAlignmentType.Alternate && info.Tree.Children.Count > 2)
                    {
                        //alternate - arrange children with even index(0,2,4,6,..) at the next level
                        bottom = Math.Max(childBounds.Bottom, prevBounds.Bottom) + layout.VerticalSpacing / 2;
                    }
                    else
                    {
                        // left/right - arrange next child at the nect level(bottom)
                        bottom = childBounds.Bottom + layout.VerticalSpacing / 2;
                    }
                    level = info.MaxLevel;
                    levels.Add(actBounds);
                    if (evenBounds == null)
                    {
                        evenBounds = new Bounds()
                        {
                            X = childInfo.X,
                            Y = childInfo.Y,
                            Right = childInfo.X + childWidth,
                            Bottom = childInfo.Y + childHeight
                        };
                    }
                    else
                    {
                        UniteRects(evenBounds, actBounds);
                    }
                    if (childInfo.LevelBounds != null) { _ = levels.Concat(childInfo.LevelBounds); }
                }
                else
                {
                    if (i != 0) { bottom = prevBounds.Bottom + layout.VerticalSpacing / 2; }
                    oddLevels.Add(actBounds);
                    if (childInfo.LevelBounds != null) { _ = oddLevels.Concat(childInfo.LevelBounds); }
                }
                if (i == 0) { info.FirstChild = new FirstChildInfo() { X = childInfo.X, CanMoveBy = childInfo.CanMoveBy, Child = child.ID }; }
                if (HasChild(layout, child) != 0)
                {
                    if (info.FirstChild == null || info.FirstChild.X >= childInfo.FirstChild.X)
                    {
                        if (childInfo.FirstChild != null && info.FirstChild.CanMoveBy < childInfo.CanMoveBy)
                        {
                            canMoveBy = info.FirstChild.CanMoveBy;
                            childInfo.CanMoveBy = canMoveBy;
                            layout.GraphNodes[info.FirstChild.Child].CanMoveBy = canMoveBy;
                            info.FirstChild.CanMoveBy = canMoveBy;
                        }
                        info.FirstChild = new FirstChildInfo()
                        {
                            X = childInfo.FirstChild.X,
                            CanMoveBy = canMoveBy != 0 ? canMoveBy : childInfo.CanMoveBy,
                            Child = child.ID
                        };
                    }
                    else if (childInfo.FirstChild != null && childInfo.Translated && info.FirstChild.CanMoveBy > childInfo.CanMoveBy)
                    {
                        info.FirstChild.CanMoveBy = layout.GraphNodes[info.FirstChild.Child].CanMoveBy = childInfo.CanMoveBy;
                    }
                }
                prevBounds = actBounds;
            }
            //To set level bounds(right most position of levels)
            if (!doNotUpdate)
            {
                List<int> intersect;
                if (info.Tree.AlignmentType == SubTreeAlignmentType.Alternate && info.Tree.Children.Count > 2)
                {
                    Bounds oddBounds = new Bounds()
                    {
                        X = oddLevels[0].X,
                        Y = oddLevels[0].Y,
                        Right = oddLevels[^1].Right,
                        Bottom = oddLevels[^1].Bottom
                    };
                    intersect = FindIntersectingLevels(layout, oddBounds, lev + 1, null);
                    UpdateRearBounds(layout, null, oddLevels, intersect);
                }
                intersect = FindIntersectingLevels(layout, evenBounds ?? bounds, lev + 1, null);
                UpdateRearBounds(layout, null, evenBounds != null ? levels : new List<Bounds>() { bounds }, intersect);
            }
            else { info.LevelBounds = levels; }
            if (canMoveBy != null)
            {
                layout.GraphNodes[firstChild.ID].CanMoveBy = canMoveBy;
            }
            info.ChildBounds = bounds;
            info.Mid = x;
            return bounds;
        }

        private static double FindOffset(ILayout layout, Node shape, LayoutInfo info, SubTreeAlignmentType? type)
        {
            double space = (layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1) ? BaseUtil.GetDoubleValue(shape.Wrapper.ActualSize.Height) :
              BaseUtil.GetDoubleValue(shape.Wrapper.ActualSize.Width);
            SubTreeAlignmentType treeType = type ?? info.Tree.AlignmentType;
            double offset = info.Tree.Offset != 0 ? info.Tree.Offset : 20;
            if (info.Tree.AlignmentType == SubTreeAlignmentType.Alternate)
            {
                if (offset >= layout.HorizontalSpacing) { offset = layout.HorizontalSpacing / 2; }
            }
            switch (treeType)
            {
                case SubTreeAlignmentType.Left:
                    offset = space / 2 - offset;
                    break;
                case SubTreeAlignmentType.Right:
                    offset += space / 2;
                    break;
            }
            return offset;
        }

        private static void AlignRowsToCenter(ILayout layout, int i, Node shape, MultipleRowInfo treeInfo, double rightX)
        {
            int max = 0;
            LayoutInfo info = layout.GraphNodes[shape.ID];
            List<List<string>> rows = treeInfo.Rows;
            List<List<string>> rightTree = treeInfo.RightTree;
            double leftCenter = treeInfo.LeftCenter;
            double rightCenter = treeInfo.RightCenter;
            bool align = treeInfo.Align;
            List<Bounds> rightBounds = treeInfo.RightBounds;
            DiagramRect dimensions = treeInfo.Dimensions;
            int lev = treeInfo.Level;
            bool unique = info.Tree.Children.Count == 5 && rows[0].Count == 3;
            if (info.Tree.Children.Count > 5)
            {
                unique = rows[0].Count == info.Tree.Children.Count / 2;
            }
            if (unique && i == 1)
            {
                max = (rightBounds[0].Right - rightBounds[0].X) >= (rightBounds[1].Right - rightBounds[1].X) ? 0 : 1;
            }
            if (i == rows.Count - 1)
            {
                if (rows[i].Count % 2 == 1 || unique && i == 1)
                {
                    string centered = rightTree[i][rightTree[i].Count / 2];
                    Node centerObject = layout.NameTable[centered] as Node;
                    double centeredX = layout.GraphNodes[centered].X;
                    double centeredY = layout.GraphNodes[centered].Y;
                    DiagramRect childDimension = GetDimensions(layout, centerObject, centeredX, centeredY, lev + 1);
                    double diff = 0;
                    if (!align && unique)
                    {
                        if (max == 1) { i = 0; }
                        diff = (rightBounds[max].X + rightBounds[max].Right) / 2 - (rightBounds[i].X + rightBounds[i].Right) / 2;
                        if (i == 0)
                        {
                            info.Mid += diff;
                        }
                    }
                    else if (!unique && rightX != 0)
                    {
                        diff = rightX - layout.HorizontalSpacing / 2 - (centeredX + childDimension.Width / 2);
                    }
                    if (diff != 0)
                    {
                        UpdateRearBoundsOfTree(layout, rightTree[i], diff, dimensions);
                    }

                    if (unique)
                    {
                        info.Mid = (rightCenter + leftCenter) / 2 + (i == 0 ? diff : 0) - dimensions.Width / 2;
                    }
                    if (info.Mid == 0 && layout.GraphNodes[centered] != null)
                    {
                        info.Mid = centeredX;
                    }
                }
            }
        }
        private static void UpdateRearBoundsOfTree(ILayout layout, List<string> rightTree, double diff, DiagramRect dimensions)
        {
            for (int j = 0; j < rightTree.Count; j++)
            {
                LayoutInfo childInfo = layout.GraphNodes[rightTree[j]];
                childInfo.X += diff;
                childInfo.CanMoveBy += (int)diff;
                if (j == rightTree.Count - 1)
                {
                    //removed child dimensions call calculation, since that is not used
                    Bounds childBounds = new Bounds()
                    {
                        X = childInfo.X,
                        Y = childInfo.Y,
                        Right = childInfo.X +
                            dimensions.Width,
                        Bottom = childInfo.Y + dimensions.Height
                    };
                    List<int> intersect = FindIntersectingLevels(layout, childBounds, childInfo.ActualLevel, null);

                    UpdateRearBounds(layout, null, new List<Bounds>() { childBounds }, intersect);
                }
            }
        }
        private static void UpdateRearBounds(ILayout layout, Node shape, List<Bounds> levelBounds, List<int> intersect)
        {
            int index;
            bool isLastLeaf = true;
            int i;
            if (shape != null)
            {
                LayoutInfo info = layout.GraphNodes[shape.ID];
                intersect = info.Intersect;
                isLastLeaf = info.Tree.Children == null && !(info.Tree.Assistants.Count > 0);
            }
            Bounds firstLevel = levelBounds[0];
            Bounds lastLevel = levelBounds[^1];
            if (intersect != null && intersect.Count > 0)
            {
                Bounds bounds = layout.Levels[intersect[0]].RBounds;
                double bottom = bounds.Bottom;
                if (bounds.Y < firstLevel.Y)
                {
                    bounds.Bottom = firstLevel.Y;
                    levelBounds.Insert(0, bounds);
                }
                if (bottom > lastLevel.Bottom)
                {
                    levelBounds.Add(new Bounds() { X = bounds.X, Right = bounds.Right, Y = firstLevel.Bottom, Bottom = bottom });
                }
                else
                {
                    bounds = layout.Levels[intersect[^1]].RBounds;
                    if (isLastLeaf && bounds.Bottom > lastLevel.Bottom)
                    {
                        bounds.Y = lastLevel.Bottom;
                        levelBounds.Add(bounds);
                    }
                }
                index = intersect[0];
                for (i = levelBounds.Count - 1; i >= 0; i--)
                {
                    layout.Levels.Insert(index, new LevelBounds() { RBounds = levelBounds[i] });
                }
                index += levelBounds.Count;
                layout.Levels.RemoveRange(index, intersect.Count);
            }
            else
            {
                index = FindLevel(layout, levelBounds[^1]);
                for (i = levelBounds.Count - 1; i >= 0; i--)
                {
                    layout.Levels.Insert(index, new LevelBounds() { RBounds = levelBounds[i] });
                    //levelBounds.RemoveAt(i);
                }
            }
        }
        private static int FindLevel(ILayout layout, Bounds bounds)
        {
            Bounds boundsLevel = bounds;
            int l = 0;
            Bounds rBounds = layout.Levels.Count > 0 && layout.Levels[l] != null ? layout.Levels[l].RBounds : null;
            while (l < layout.Levels.Count)
            {
                if (rBounds != null && boundsLevel.Bottom < rBounds.Y)
                {
                    return l;
                }
                else
                {
                    l++;
                }
                rBounds = l < layout.Levels.Count && layout.Levels[l] != null ? layout.Levels[l].RBounds : null;
            }
            return l;
        }
        private double UpdateLeftTree(ILayout layout, MultipleRowInfo treeInfo, Node shape, double x, double bottom, int lev)
        {
            List<List<string>> leftTree = treeInfo.LeftTree;
            LayoutInfo info = layout.GraphNodes[shape.ID];
            List<Bounds> leftBounds = new List<Bounds>();
            int minTranslation = 0;
            double rightMost = 0;
            Bounds bounds = null;
            //Arrange left side
            for (int i = 0; i < leftTree.Count; i++)
            {
                if (leftTree[i].Count > 0)
                {
                    double right = x;
                    if (i > 0 && leftBounds[i - 1] != null) { bottom = leftBounds[i - 1].Bottom + layout.VerticalSpacing; }
                    for (int j = 0; j < leftTree[i].Count; j++)
                    {
                        Node child = layout.NameTable[leftTree[i][j]] as Node;
                        //Update sub tree
                        LayoutInfo childInfo = layout.GraphNodes[child.ID];
                        childInfo.ActualLevel = lev + 1 + i;
                        Bounds childBounds = this.UpdateTree(layout, right, bottom, child, lev + 1, j > 0 ? layout.NameTable[leftTree[i][j - 1]] as Node : null);
                        if (j == 0)
                        {
                            leftBounds.Insert(i, new Bounds() { X = childBounds.X, Y = childBounds.Y, Right = childBounds.Right, Bottom = childBounds.Bottom });
                        }
                        else
                        {
                            UniteRects(leftBounds[i], childBounds);
                        }
                        if (i == 0 && j == 0)
                        {
                            minTranslation = (int)BaseUtil.GetDoubleValue(childInfo.CanMoveBy);
                            info.FirstChild = new FirstChildInfo() { X = childInfo.X, Child = child.ID, CanMoveBy = childInfo.CanMoveBy };
                        }
                        else if (j == 0 && childInfo.CanMoveBy != 0 && minTranslation > childInfo.CanMoveBy)
                        {
                            minTranslation = Math.Min(minTranslation, (int)BaseUtil.GetDoubleValue(childInfo.CanMoveBy));
                            info.FirstChild = new FirstChildInfo() { X = childInfo.X, Child = child.ID, CanMoveBy = childInfo.CanMoveBy };
                        }
                        right = childBounds.Right + layout.HorizontalSpacing;
                    }
                    rightMost = i == 0 ? leftBounds[i].Right : Math.Max(rightMost, leftBounds[i].Right);
                }
            }

            //Translate to same positions
            for (int i = 0; i < leftTree.Count; i++)
            {
                if (leftTree[i].Count > 0)
                {
                    if (!rightMost.Equals(leftBounds[i].Right))
                    {
                        double diff = rightMost - leftBounds[i].Right;
                        for (int j = 0; j < leftTree[i].Count; j++)
                        {
                            LayoutInfo elementInfo = layout.GraphNodes[leftTree[i][j]];
                            elementInfo.X += diff;
                        }
                    }
                    if (i == 0)
                    {
                        bounds = new Bounds() { X = leftBounds[0].X, Y = leftBounds[0].Y, Right = leftBounds[0].Right, Bottom = leftBounds[0].Bottom };
                    }
                    else { UniteRects(bounds, leftBounds[i]); }
                }
            }
            treeInfo.Bounds = bounds;
            return rightMost;
        }

        private static void SplitRows(List<List<string>> rows, List<List<string>> leftTree, List<List<string>> rightTree)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                leftTree.Insert(i, new List<string>()); rightTree.Insert(i, new List<string>());
                if (rows[i].Count % 2 != 1)
                {
                    int half = rows[i].Count / 2;
                    for (int k = 0; k < half; k++)
                    {
                        leftTree[i].Add(rows[i][k]);
                    }
                }
                for (int j = leftTree[i].Count; j < rows[i].Count; j++)
                {
                    rightTree[i].Add(rows[i][j]);
                }
            }
        }
        private static List<List<string>> SplitChildrenInRows(ILayout layout, Node shape)
        {
            LayoutInfo info = layout.GraphNodes[shape.ID];
            int column = 4;
            List<List<string>> rows = new List<List<string>>();
            int childNodes = info.Tree.Children.Count;
            List<string> children = new List<string>(info.Tree.Children);
            if (info.Tree.Rows != null)
            {
                int columns = info.Tree.Rows.Value;
                if (columns % 2 == 0)
                {
                    column = columns;
                }
                else
                {
                    column = columns - 1;
                }
            }
            else if (info.Tree.Children.Count == 2 || info.Tree.Children.Count == 3 || info.Tree.Children.Count == 4)
            {
                column = 2;
            }
            else if (info.Tree.Children.Count == 5)
            {
                column = 3;
            }
            while (childNodes > 0)
            {
                rows.Add(new List<string>(children.GetRange(0, column)));
                children.RemoveRange(0, column);
                childNodes -= column;
                if (childNodes > 0 && childNodes < column)
                {
                    if (childNodes % 2 == 0)
                    {
                        column = childNodes;
                    }
                    else if (childNodes != 1)
                    {
                        column = childNodes - 1;
                    }
                    if (childNodes < column)
                    {
                        column = childNodes;
                    }
                }
            }
            return rows;
        }
        private Bounds UpdateHorizontalTreeWithMultipleRows(ILayout layout, Node shape, double x, double y, int level)
        {
            //declarations
            Bounds bounds = null;
            double leftCenter = 0; double rightCenter = 0;
            //Get dimensions with respect to layout orientations
            DiagramRect dimensions = GetDimensions(layout, shape, x, y, level);
            LayoutInfo info = layout.GraphNodes[shape.ID];
            int lev = level;
            double bottom = y;
            int minTranslation = 0;
            if (HasChild(layout, shape) >= 0)
            {
                bool h = layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1;
                bool align = false;
                List<List<string>> rows = SplitChildrenInRows(layout, shape);
                bool unique = info.Tree.Children.Count == 5 && rows[0].Count == 3;
                List<List<string>> leftTree = new List<List<string>>(); List<List<string>> rightTree = new List<List<string>>();

                if (!unique)
                {
                    SplitRows(rows, leftTree, rightTree);
                }
                else { rightTree = rows; }

                MultipleRowInfo treeInfo = new MultipleRowInfo() { LeftTree = leftTree, Rows = rows, RightTree = rightTree, Dimensions = dimensions };

                double rightMost = this.UpdateLeftTree(layout, treeInfo, shape, x, bottom, lev);
                bounds = treeInfo.Bounds;
                double rightX = 0;
                double center = rightMost + (layout.HorizontalSpacing / 2);

                if (rightMost != 0)
                {
                    info.Mid = center - dimensions.Width / 2;
                    rightX = rightMost + layout.HorizontalSpacing;
                }

                bottom = y;
                List<Bounds> rightBounds = new List<Bounds>();
                int i;
                for (i = 0; i < rightTree.Count; i++)
                {
                    double right;
                    if (rows[i].Count % 2 == 1 && i == rightTree.Count - 1 || unique) { right = x; }
                    else
                    {
                        right = rightX != 0 ? rightX : x;
                    }
                    if (i != 0) { bottom = rightBounds[i - 1].Bottom + layout.VerticalSpacing; }

                    int j;
                    for (j = 0; j < rightTree[i].Count; j++)
                    {
                        Node child = layout.NameTable[rightTree[i][j]] as Node;
                        double width = BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Width);
                        double height = BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Height);
                        double childWidth = h ? height : width;
                        //Update sub tree
                        LayoutInfo childInfo = layout.GraphNodes[child.ID];
                        childInfo.ActualLevel = lev + 1 + i;
                        if (j == 0 && leftTree.Count > 0 && leftTree[i].Count > 0 && leftTree[i] != null) { childInfo.Translate = false; }
                        if (unique && i == 1)
                        {
                            if (j == 0 && leftCenter + childWidth + layout.HorizontalSpacing <= rightCenter)
                            {
                                align = true;
                                right = leftCenter - childWidth / 2;
                            }
                            if (align && j == 1) { right = rightCenter - childWidth / 2; }
                        }

                        Bounds childBounds = this.UpdateTree(layout, right, bottom, child, lev + 1, j > 0 ? layout.NameTable[rightTree[i][j - 1]] as Node : null);
                        if (unique && j <= 2 && i == 0)
                        {
                            if (j == 1)
                            {
                                leftCenter = childBounds.X - layout.HorizontalSpacing / 2;
                                rightCenter = childBounds.X + childWidth + layout.HorizontalSpacing / 2;
                            }
                        }
                        if (j == 0)
                        {
                            rightBounds.Insert(i, new Bounds() { X = childBounds.X, Y = childBounds.Y, Right = childBounds.Right, Bottom = childBounds.Bottom });
                        }
                        else
                        {
                            UniteRects(rightBounds[i], childBounds);
                        }
                        bounds ??= new Bounds()
                        {
                            X = rightBounds[i].X,
                            Y = rightBounds[i].Y,
                            Right = rightBounds[i].Right,
                            Bottom = rightBounds[i].Bottom
                        };
                        UniteRects(bounds, rightBounds[i]);
                        right = childBounds.Right + layout.HorizontalSpacing;
                        if (info.FirstChild == null || ((i == rightTree.Count - 1 && rows[i].Count % 2 == 1) || unique)
                            && j == 0 && childInfo.CanMoveBy != 0 && minTranslation > childInfo.CanMoveBy)
                        {
                            minTranslation = Math.Min(minTranslation, (int)BaseUtil.GetDoubleValue(childInfo.CanMoveBy));
                            info.FirstChild = new FirstChildInfo() { X = childInfo.X, Child = child.ID, CanMoveBy = childInfo.CanMoveBy };
                        }
                        treeInfo.LeftCenter = leftCenter; treeInfo.RightCenter = rightCenter; treeInfo.Align = align;
                        treeInfo.Level = lev; treeInfo.RightBounds = rightBounds;
                        AlignRowsToCenter(layout, i, shape, treeInfo, rightX);
                    }
                }
            }
            return bounds;
        }

        private static LayoutObject SetDepthSpaceForAssitants(ILayout layout, Node shape, double bottom, int? lev, double vSpace)
        {
            LayoutInfo info = layout.GraphNodes[shape.ID];
            double asstHeight = 0;
            int i;
            for (i = 0; i < info.Tree.Assistants.Count; i++)
            {
                LayoutInfo asst = layout.GraphNodes[info.Tree.Assistants[i]];
                if (asst != null)
                {
                    asst.Tree.Children = asst.Tree.Assistants = new List<string>();
                    asst.Y = bottom;
                    if (layout.NameTable[info.Tree.Assistants[i]] is Node asstElement)
                    {
                        asstHeight = BaseUtil.GetDoubleValue(asstElement.Wrapper.ActualSize.Height);
                        if (layout.Orientation.ToString()
                            .IndexOf(LEFT, StringComparison.InvariantCulture) != -1)
                        {
                            asstHeight = BaseUtil.GetDoubleValue(asstElement.Wrapper.ActualSize.Width);
                        }
                    }
                    double max = bottom + asstHeight + vSpace / 2;
                    layout.MaxLevel = (lev ?? 0) + 1;
                    if (i % 2 == 1 && i != info.Tree.Assistants.Count - 1)
                    {
                        bottom = max;
                        lev++;
                    }
                }
            }
            return new LayoutObject() { Level = layout.MaxLevel, Bottom = bottom + asstHeight + vSpace };
        }
        internal static DiagramRect GetDimensions(ILayout layout, Node shape, double x, double y, int? level)
        {
            int width = Convert.ToInt32(BaseUtil.GetDoubleValue(shape.Wrapper.ActualSize.Width));
            int height = Convert.ToInt32(BaseUtil.GetDoubleValue(shape.Wrapper.ActualSize.Height));
            if (layout.Orientation.ToString().IndexOf(LEFT, StringComparison.InvariantCulture) != -1)
            {
                if (level == null)
                {
                    (x, y) = (y, x);
                }
                height = Convert.ToInt32(BaseUtil.GetDoubleValue(shape.Wrapper.ActualSize.Width));
                width = Convert.ToInt32(BaseUtil.GetDoubleValue(shape.Wrapper.ActualSize.Height));
            }
            return new DiagramRect() { X = x, Y = y, Width = width, Height = height };
        }

        internal Bounds UpdateHorizontalTree(ILayout layout, Node shape, double x, double y, int level)
        {
            DiagramRect dimensions = GetDimensions(layout, shape, x, y, level);
            LayoutInfo info = layout.GraphNodes[shape.ID];
            SubTreeAlignmentType side = info.Tree.AlignmentType;
            int lev = level;
            double right = x;
            double bottom = y;
            Bounds bounds = new Bounds();
            Bounds actBounds = new Bounds();
            int maxLevel = 0;
            Bounds oldActBounds = new Bounds();
            int? canMoveBy = null;
            Single translateSiblingBy = Single.NaN;
            LayoutInfo firstChildInfo = new LayoutInfo();
            if (HasChild(layout, shape) != 0)
            {
                bool h = layout.Orientation.ToString().Contains(LEFT, StringComparison.InvariantCulture);
                int i;
                for (i = 0; i < info.Tree.Children.Count; i++)
                {
                    Node child = layout.NameTable[info.Tree.Children[i]] as Node;
                    int width = Convert.ToInt32(BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Width));
                    int height = Convert.ToInt32(BaseUtil.GetDoubleValue(child.Wrapper.ActualSize.Height));
                    int childWidth = h ? height : width;
                    int childHeight = h ? width : height;
                    Bounds childBounds = this.UpdateTree(layout, right, bottom, child, lev + 1, i == 0 ? null : layout.NameTable[info.Tree.Children[i - 1]] as Node);
                    LayoutInfo childInfo = layout.GraphNodes[child.ID];
                    info.MaxLevel = Math.Max(info.MaxLevel | 0, childInfo.MaxLevel | 0);
                    actBounds.X = childInfo.X;
                    actBounds.Y = childInfo.Y;
                    actBounds.Right = childInfo.X + childWidth;
                    actBounds.Bottom = childInfo.Y + childHeight;
                    if (i == 0)
                    {
                        bounds.X = Math.Min(childInfo.X, childBounds.X);
                        bounds.Y = Math.Min(childInfo.Y, childBounds.Y);
                        bounds.Right = childBounds.Right;
                        bounds.Bottom = childBounds.Bottom;
                        firstChildInfo = childInfo;
                    }
                    oldActBounds.X = actBounds.X;
                    oldActBounds.Y = actBounds.Y;
                    if (actBounds.Right > oldActBounds.Right)
                    {
                        oldActBounds.Right = actBounds.Right;
                    }
                    oldActBounds.Bottom = actBounds.Bottom;
                    if (i == 0)
                    {
                        info.FirstChild.X = childInfo.X;
                        info.FirstChild.CanMoveBy = childInfo.CanMoveBy;
                        info.FirstChild.Child = child.ID;
                    }
                    if (HasChild(layout, child) > 0)
                    {
                        if (info.FirstChild == null || info.FirstChild.X >= childInfo.FirstChild.X)
                        {
                            if (childInfo != null && childInfo.FirstChild != null && info.FirstChild.CanMoveBy < childInfo.CanMoveBy)
                            {
                                info.FirstChild.CanMoveBy = (int)BaseUtil.GetDoubleValue(canMoveBy);
                                childInfo.CanMoveBy = (int)BaseUtil.GetDoubleValue(canMoveBy);
                                canMoveBy = info.FirstChild.CanMoveBy;
                                layout.GraphNodes[info.FirstChild.Child].CanMoveBy = (int)BaseUtil.GetDoubleValue(canMoveBy);
                            }
                            int? canMoveValue = canMoveBy ?? childInfo.CanMoveBy;
                            info.FirstChild = new FirstChildInfo() { X = childInfo.FirstChild.X, CanMoveBy = canMoveValue, Child = child.ID };
                        }
                        else if (childInfo.FirstChild != null && childInfo.Translated && info.FirstChild.CanMoveBy > childInfo.CanMoveBy)
                        {
                            info.FirstChild.CanMoveBy = layout.GraphNodes[info.FirstChild.Child].CanMoveBy = childInfo.CanMoveBy;
                        }
                    }
                    maxLevel = maxLevel > 0 ? Math.Max(childInfo.MaxLevel, maxLevel) : childInfo.MaxLevel;
                    UniteRects(bounds, childBounds);
                    if (i != 0 && HasChild(layout, child) == 0 && childInfo.SubTreeTranslation < 0)
                    {
                        right = childBounds.Right - childInfo.SubTreeTranslation + layout.HorizontalSpacing;
                    }
                    else
                    {
                        right = childBounds.Right + layout.HorizontalSpacing;
                    }
                }
                if (!Single.IsNaN(translateSiblingBy))
                {
                    firstChildInfo.CanMoveBy = (int)translateSiblingBy;
                }
                info.Mid = (firstChildInfo.X + oldActBounds.Right) / 2 - dimensions.Width / 2;
                if (side == SubTreeAlignmentType.Left)
                {
                    info.Mid = actBounds.Right - dimensions.Width;
                }
                else if (side == SubTreeAlignmentType.Right)
                {
                    info.Mid = x;
                }
            }
            return bounds;
        }

        private static int HasChild(ILayout layout, NodeBase shape)
        {
            LayoutInfo shape1 = layout.GraphNodes[shape.ID];
            if (shape1.Tree.Children != null && shape1.Tree.Children.Count > 0)
            {
                return shape1.Tree.Children.Count;
            }
            else return 0;
        }
        internal static void UniteRects(Bounds rect1, Bounds rect2)
        {
            rect1.X = Math.Min(rect1.X, rect2.X);
            rect1.Right = Math.Max(rect1.Right, rect2.Right);
            rect1.Bottom = Math.Max(rect1.Bottom, rect2.Bottom);
        }
        private static Bounds UpdateLeafNode(ILayout layout, Node shape, Node prev, DiagramRect dimensions, int? level, bool doNotUpdate)
        {
            Bounds bounds = new Bounds();
            Bounds value = new Bounds();
            LayoutInfo info = layout.GraphNodes[shape.ID];
            info.X = dimensions.X;
            if ((info.Y == 0 && info.Y < dimensions.Y))
            {
                info.Y = dimensions.Y;
                info.MaxLevel = info.MaxLevel > level ? info.MaxLevel : level ?? 0;
            };
            bounds.X = dimensions.X;
            bounds.Y = dimensions.Y;
            bounds.Right = dimensions.X + dimensions.Width;
            bounds.Bottom = dimensions.Y + dimensions.Height;
            info.MaxLevel = info.MaxLevel > level ? info.MaxLevel : level ?? 0;
            TranslateInfo translateInfo = new TranslateInfo
            {
                Layout = layout,
                Shape = shape,
                ShapeBounds = bounds,
                TreeBounds = bounds,
                Dim = dimensions,
                Level = level ?? 0
            };
            TranslateSubTree(translateInfo, null, prev != null, doNotUpdate);
            value.X = info.X;
            value.Y = info.Y;
            value.Right = info.X + dimensions.Width;
            value.Bottom = info.Y + dimensions.Height;
            return value;
        }
        internal static void TranslateSubTree(TranslateInfo translateInfo, int? asstDif, bool translate, bool doNotUpdate)
        {
            ILayout layout = translateInfo.Layout;
            Node shape = translateInfo.Shape;
            Bounds shapeBounds = translateInfo.ShapeBounds;
            Bounds treeBounds = translateInfo.TreeBounds;
            int level = translateInfo.Level;
            DiagramRect dim = translateInfo.Dim;
            LayoutInfo info = layout.GraphNodes[shape.ID];
            Node firstChild = null;
            if (info.Tree.Children.Count > 0)
            {
                firstChild = layout.NameTable[info.FirstChild.ID != null ? info.FirstChild.Child : info.Tree.Children[0]] as Node;
            }
            LayoutInfo firstChildInfo = firstChild != null ? layout.GraphNodes[firstChild.ID] : null;
            int hasChild = HasChild(layout, shape);
            List<int> intersect = FindIntersectingLevels(layout, shapeBounds, level, info.ActualLevel);
            List<int> treeIntersect = FindIntersectingLevels(layout, treeBounds, level, info.ActualLevel);
            List<Bounds> levelBounds = new List<Bounds>();
            if (intersect.Count > 0 && info.Translate)
            {
                info.Intersect = intersect;
                SpaceLeftFromPrevSubTree(layout, shape, shapeBounds);
#pragma warning disable CA1305 // Specify IFormatProvider
                info.CanMoveBy = info.Diff == null ? 0 : Convert.ToInt32(info.Diff);
#pragma warning restore CA1305 // Specify IFormatProvider
                if (asstDif != null)
                {
                    info.CanMoveBy = Math.Min((int)BaseUtil.GetDoubleValue(asstDif), (int)BaseUtil.GetDoubleValue(info.CanMoveBy));
                }
                if (firstChild != null && firstChildInfo.CanMoveBy != null)
                {
                    if (firstChildInfo.CanMoveBy >= info.CanMoveBy) { info.Translated = true; }
                    info.CanMoveBy = Math.Min((int)BaseUtil.GetDoubleValue(info.CanMoveBy), (int)BaseUtil.GetDoubleValue(firstChildInfo.CanMoveBy));
                }
                if (translate)
                {
                    info.X -= (int)BaseUtil.GetDoubleValue(info.CanMoveBy);
                    info.SubTreeTranslation -= (int)BaseUtil.GetDoubleValue(info.CanMoveBy);
                    if (hasChild > 0)
                    {
                        ShiftSubordinates(layout, treeIntersect, info.CanMoveBy);
                        treeBounds.X = Math.Min(treeBounds.X, info.X);
                        treeBounds.Right = Math.Max(treeBounds.Right, info.X + dim.Width);
                        treeBounds.Bottom = Math.Max(treeBounds.Bottom, info.Y + dim.Height);
                        treeBounds.X -= (int)BaseUtil.GetDoubleValue(info.CanMoveBy);
                        treeBounds.Right -= (int)BaseUtil.GetDoubleValue(info.CanMoveBy);
                    }
                    if (firstChild != null && firstChildInfo.CanMoveBy > info.CanMoveBy)
                    {
                        info.CanMoveBy = firstChildInfo.CanMoveBy - info.CanMoveBy;
                    }
                    else if (firstChild != null)
                    {
                        info.CanMoveBy = 0;
                    }
                }
            }
            else
            {
                if (hasChild > 0)
                {
                    treeBounds.X = Math.Min(treeBounds.X, shapeBounds.X);
                    treeBounds.Right = Math.Max(treeBounds.Right, shapeBounds.X + dim.Width);
                    treeBounds.Bottom = Math.Max(treeBounds.Bottom, info.Y + dim.Height);
                }
                if (!info.Translate)
                {
                    info.CanMoveBy = 0;
                    info.SubTreeTranslation = 0;
                }
            }
            if (!doNotUpdate)
            {
                shapeBounds.X = info.X;
                shapeBounds.Y = info.Y;
                shapeBounds.Right = info.X + dim.Width;
                shapeBounds.Bottom = dim.Y + dim.Height;
                levelBounds.Add(shapeBounds);
                UpdateRearBounds(layout, shape, levelBounds, null);
            }
        }
        private static void ShiftSubordinates(ILayout layout, List<int> intersect, int? diff)
        {
            if (diff != 0)
            {
                int i;
                for (i = 0; i < intersect.Count; i++)
                {
                    if (layout.Levels[intersect[i]].RBounds != null)
                    {
                        if (diff != null)
                        {
                            layout.Levels[intersect[i]].RBounds.X -= diff.Value;
                            layout.Levels[intersect[i]].RBounds.Right -= diff.Value;
                        }
                    }
                }
            }
        }
        private static void SpaceLeftFromPrevSubTree(ILayout layout, Node shape, Bounds bounds)
        {
            LayoutInfo info = layout.GraphNodes[shape.ID];
            int k;
            int space = layout.HorizontalSpacing;
            for (k = 0; k < info.Intersect.Count; k++)
            {
                Bounds prevBounds = layout.Levels[info.Intersect[k]].RBounds;
                double dif = bounds.X - (prevBounds.Right + space);
                if (info.Diff == null || dif < info.Diff)
                {
                    info.Diff = dif;
                    info.PrevBounds = layout.Levels[info.Intersect[k]].RBounds;
                }
            }
        }
        private static List<int> FindIntersectingLevels(ILayout layout, Bounds bounds, int? level, int? actualLevel)
        {
            Bounds newBounds = new Bounds();
            List<int> intersectingLevels = new List<int>();
            newBounds.X = bounds.X;
            newBounds.Y = bounds.Y;
            newBounds.Right = bounds.Right;
            newBounds.Bottom = bounds.Bottom;
            newBounds.Y -= layout.VerticalSpacing / 2;
            newBounds.Bottom += layout.VerticalSpacing / 2;
            int l = actualLevel != null ? (int)BaseUtil.GetDoubleValue(actualLevel) : (int)BaseUtil.GetDoubleValue(level);
            Bounds rBounds = layout.Levels.Count > 0 && layout.Levels.Count > l && layout.Levels[l] != null ? layout.Levels[l].RBounds : null;
            do
            {
                if (rBounds != null && ((newBounds.Y < rBounds.Y && newBounds.Bottom > rBounds.Y)
                   || (newBounds.Y < rBounds.Bottom && rBounds.Bottom < newBounds.Bottom) ||
                   newBounds.Y >= rBounds.Y &&
                       newBounds.Bottom <= rBounds.Bottom || newBounds.Y < rBounds.Y && newBounds.Bottom > rBounds.Bottom))
                {
                    var index = 0;
                    intersectingLevels.Insert(index, l);
                }
                else if (rBounds != null && rBounds.Bottom < newBounds.Y)
                {
                    break;
                }
                l--;
                if (l == -1) break;
                rBounds = layout.Levels.Count > 0 && layout.Levels.Count > l && layout.Levels[l] != null ? layout.Levels[l].RBounds : null;
            } while (l >= 0);
            l = (actualLevel != null ? (int)BaseUtil.GetDoubleValue(actualLevel) : (int)BaseUtil.GetDoubleValue(level)) + 1;
            rBounds = layout.Levels.Count > 0 && layout.Levels.Count > l && layout.Levels[l] != null ? layout.Levels[l].RBounds : null;
            do
            {
                if (rBounds != null && ((newBounds.Y < rBounds.Y && newBounds.Bottom > rBounds.Y) ||
                    (newBounds.Y < rBounds.Bottom && rBounds.Bottom < newBounds.Bottom) ||
                    newBounds.Y >= rBounds.Y && newBounds.Bottom <= rBounds.Bottom || newBounds.Y < rBounds.Y && newBounds.Bottom > rBounds.Bottom))
                {
                    intersectingLevels.Add(l);
                }
                else if (rBounds != null && rBounds.Y > newBounds.Bottom) { break; }
                l++;
                if (l == -1) break;
                rBounds = layout.Levels.Count > 0 && layout.Levels.Count > l && layout.Levels[l] != null ? layout.Levels[l].RBounds : null;
            } while (l <= layout.Levels.Count);
            return intersectingLevels;
        }
    }

    internal class LayoutObject
    {
        internal int Level { get; set; }
        internal double Bottom { get; set; }
    }
    internal class MultipleRowInfo
    {
        internal double LeftCenter { get; set; }
        internal double RightCenter { get; set; }
        internal bool Align { get; set; }
        internal List<List<string>> Rows { get; set; }
        internal List<List<string>> RightTree { get; set; }
        internal List<List<string>> LeftTree { get; set; }
        internal Bounds Bounds { get; set; }
        internal List<Bounds> RightBounds { get; set; }
        internal DiagramRect Dimensions { get; set; }
        internal int Level { get; set; }
    }

}
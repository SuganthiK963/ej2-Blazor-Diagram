using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the object to be rendered in the diagram once the data has been processed.
    /// </summary>
    public partial class DiagramRenderer
    {
        const string LEFT = "left";
        const string CENTER = "center";
        const string RIGHT = "right";
        const string HANDLE = "_userhandle";
        const string CIRCLARHANDLECLASS = "e-diagram-userhandle-circle";
        const string HANDLEPATHID = "userhandle-path";
        const string HANDLEPATHCLASS = "e-diagram-userhandle-path";
        const string HANDLETEMPLATEID = "_shape";
        const string HELPERCSSCLASS = "e-diagram-helper";
        internal static PathAttributes GetPathAttributes(PathElement element, TransformFactor transform, bool isPreviewNode)
        {
            PathAttributes options = new PathAttributes(element, transform, isPreviewNode);
            if (element.ID.Contains("helper", StringComparison.InvariantCulture))
            {
                options.ClassValues = @HELPERCSSCLASS;
            }
            options.X = element.FlipOffset.X != 0 ? element.FlipOffset.X : options.X;
            options.Y = element.FlipOffset.Y != 0 ? element.FlipOffset.Y : options.Y;
            return options;
        }

        internal static ObservableCollection<BaseAttributes> GetSelectorAttributes(SelectionFragmentParameter parameters)
        {
            DiagramSize size = new DiagramSize();
            DiagramSelectionSettings selector = parameters.Selector;
            TransformFactor transform = parameters.Transform;
            if (selector.Nodes.Count == 1 && selector.Connectors.Count == 0)
            {
                selector.RotationAngle = selector.Nodes[0].RotationAngle;
                selector.Pivot = selector.Nodes[0].Pivot;
            }
            selector.Wrapper.Measure(size);
            selector.Wrapper.Arrange(selector.Wrapper.DesiredSize);
            selector.Width = selector.Wrapper.ActualSize.Width.Value;
            selector.Height = selector.Wrapper.ActualSize.Height.Value;
            selector.OffsetX = selector.Wrapper.OffsetX;
            selector.OffsetY = selector.Wrapper.OffsetY;
            ObservableCollection<BaseAttributes> attributes = new ObservableCollection<BaseAttributes>();
            if (selector.Nodes.Count + selector.Connectors.Count == 1)
            {
                if (selector.Nodes.Count > 0)
                {
                    Node node = selector.Nodes[0];
                    bool constraints = node.Constraints.HasFlag(NodeConstraints.HideThumbs);
                    RenderResizeHandle(attributes, node.Wrapper, selector.ThumbsConstraints, selector.Constraints, transform, false, ConstraintsUtil.CanMove(node), node.Constraints.HasFlag(NodeConstraints.HideThumbs));
                }
                else if (selector.Connectors.Count > 0)
                {
                    RenderEndPointHandle(selector.Connectors[0], selector.ThumbsConstraints, selector.Constraints, transform, attributes);
                }
            }
            else
            {
                RenderResizeHandle(attributes, selector.Wrapper, selector.ThumbsConstraints, selector.Constraints, transform, false, ConstraintsUtil.CanMove(selector), false);
            }
            if ((selector.Constraints.HasFlag(SelectorConstraints.UserHandle)) && selector.UserHandles.Count > 0)
            {
                RenderUserHandle(attributes, selector, transform);
            }
            return attributes;
        }



        internal static string ParseDashArray(string dashArray)
        {
            string separator = dashArray.IndexOf(' ', StringComparison.InvariantCulture) != -1 ? " " : ",";
            string[] splittedDashes = dashArray.Split(separator);
            double[] dashes = new double[splittedDashes.Length];
            for (int i = 0; i < splittedDashes.Length; i++)
            {
                _ = double.TryParse(splittedDashes[i], out double value);
                dashes[i] = value;
            }
            return string.Join(",", dashes);
        }
        internal static TextAttributes GetTextAttributes(TextElement element, TransformFactor transform, bool isPreviewNode)
        {
            TextAttributes options = new TextAttributes(element, transform, isPreviewNode);
            return options;
        }
        internal static ImageAttributes GetImageAttributes(ImageElement element, TransformFactor transform, bool isPreviewNode)
        {
            ImageAttributes options = new ImageAttributes(element, transform, isPreviewNode);
            return options;
        }
        internal static BaseAttributes GetNativeAttributes(DiagramSvgElement element, TransformFactor transform, bool isPreviewNode)
        {
            BaseAttributes options = new BaseAttributes(element, transform, isPreviewNode);
            return options;
        }
        internal static BaseAttributes GetHtmlAttributes(DiagramHtmlElement element, TransformFactor transform, bool isPreviewNode)
        {
            BaseAttributes options = new BaseAttributes(element, transform, isPreviewNode);
            return options;
        }
        internal static DiagramPoint SvgLabelAlign(TextAttributes text, TextBounds wrapBound, ObservableCollection<SubTextElement> childNodes)
        {
            DiagramSize bounds = new DiagramSize() { Width = wrapBound.Width, Height = childNodes.Count * (text.FontSize * 1.2) };
            DiagramPoint pos = new DiagramPoint() { X = 0, Y = 0 };
            double x = 0;
            double y = 1.2;
            double offsetX = text.Width * 0.5;
            double offsetY = text.Height * 0.5;
            double pointX = offsetX;
            double pointY = offsetY;
            if (text.TextAlign == LEFT)
            {
                pointX = 0;
            }
            else if (text.TextAlign == CENTER)
            {
                if (wrapBound.Width > text.Width && (text.TextOverflow == TextOverflow.Ellipsis || text.TextOverflow == TextOverflow.Clip))
                {
                    if (text.TextWrapping == TextWrap.NoWrap)
                    {
                        pointX = 0;
                    }
                    else
                    {
                        pointX = text.Width * 0.5;
                    }
                }
                else
                {
                    pointX = text.Width * 0.5;
                }
            }
            else if (text.TextAlign == RIGHT)
            {
                pointX = (text.Width * 1);
            }
            pos.X = x + pointX + (wrapBound.X);
            pos.Y = y + pointY - BaseUtil.GetDoubleValue(bounds.Height) / 2;
            return pos;
        }

        internal static ObservableCollection<TextElementBounds> GetTextElements(TextAttributes attributes)
        {
            ObservableCollection<TextElementBounds> textElementBounds = new ObservableCollection<TextElementBounds>();
            TextBounds wrapBounds = attributes.WrapBounds;
            ObservableCollection<SubTextElement> childNodes = attributes.ChildNodes;
            DiagramPoint position = null;
            if (attributes.WrapBounds != null)
                position = SvgLabelAlign(attributes, attributes.WrapBounds, attributes.ChildNodes);
            double? childNodesHeight = 0;
            for (int i = 0; i < childNodes.Count; i++)
            {
                SubTextElement child = childNodes[i];
                child.X = DomUtil.SetChildPosition(child, childNodes, i, attributes);
                if (position != null)
                {
                    double? offsetX = position.X + child.X - wrapBounds.X;
                    double? offsetY = position.Y + child.Dy * (i) + ((attributes.FontSize) * 0.8);
                    if ((attributes.TextOverflow == TextOverflow.Clip || attributes.TextOverflow == TextOverflow.Ellipsis) &&
                        (attributes.TextWrapping == TextWrap.WrapWithOverflow || attributes.TextWrapping == TextWrap.Wrap))
                    {
                        double size = attributes.ParentHeight;
                        if (offsetY < size)
                        {
                            if (attributes.TextOverflow == TextOverflow.Ellipsis && i < childNodes.Count - 1 && childNodes[i + 1] != null)
                            {
                                SubTextElement temp = childNodes[i + 1];
                                double y = position.Y + temp.Dy * (i + 1) + (attributes.FontSize * 0.8);
                                if (y > size)
                                {
                                    child.Text = child.Text.Remove(child.Text.Length - 3, 3);
                                    child.Text += "...";
                                }
                            }
                            childNodesHeight += child.Dy;
                            textElementBounds.Add(new TextElementBounds() { X = offsetX.ToString(), Y = offsetY.ToString(), Text = child.Text });
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        textElementBounds.Add(new TextElementBounds() { X = offsetX.ToString(), Y = offsetY.ToString(), Text = child.Text });
                    }
                }
            }
            return textElementBounds;
        }

        internal static void RenderPivotLine(DiagramContainer wrapper, TransformFactor transform, SelectorConstraints selectorConstraints, bool canMask, ObservableCollection<BaseAttributes> attributes)
        {
            bool visible = !canMask && (selectorConstraints.HasFlag(SelectorConstraints.Rotate));
            LineAttributes lineAttributes = new LineAttributes(wrapper, transform)
            {
                Fill = "None",
                Stroke = "black",
                StrokeWidth = 1,
                DashArray = "2,3",
                Visible = visible
            };
            double scale = transform.Scale;
            lineAttributes.X *= scale;
            lineAttributes.Y *= scale;
            lineAttributes.Width *= scale;
            lineAttributes.Height *= scale;
            lineAttributes.ID = "pivotLine";
            lineAttributes.ClassValues = "e-diagram-pivot-line";
            lineAttributes.StartPoint = new DiagramPoint(wrapper.ActualSize.Width * wrapper.Pivot.X * scale, -20);
            lineAttributes.EndPoint = new DiagramPoint(wrapper.ActualSize.Width * wrapper.Pivot.X * scale, 0);
            lineAttributes.Angle = wrapper.RotationAngle;
            attributes.Add(lineAttributes);
        }

        internal static void RenderRotateThumb(DiagramContainer element, TransformFactor transform, SelectorConstraints selectorConstraints, bool canMask, ObservableCollection<BaseAttributes> attributes)
        {
            string data = "M16.856144362449648,10.238890446662904 L18.000144362449646,3.437890446662903 L15.811144362449646,4.254890446662903 C14.837144362449646,2.5608904466629028,13.329144362449647,1.2598904466629026,11.485144362449645,0.5588904466629026 C9.375144362449646,0.24510955333709716,7.071144362449646,0.18010955333709716,5.010144362449646,0.7438904466629028 C2.942144362449646,1.6678904466629028,1.365144362449646,3.341890446662903,0.558144362449646,5.452890446662903 C0.244855637550354,7.567890446662903,0.17985563755035394,9.866890446662904,0.7431443624496461,11.930890446662904 C1.6681443624496461,13.994890446662904,3.343144362449646,15.575890446662903,5.457144362449647,16.380890446662903 C6.426144362449647,16.7518904466629,7.450144362449647,16.9348904466629,8.470144362449647,16.9348904466629 C9.815144362449647,16.9348904466629,11.155144362449647,16.6178904466629,12.367144362449647,15.986890446662901 L11.351144362449647,14.024890446662901 C9.767144362449647,14.8468904466629,7.906144362449647,14.953890446662902,6.237144362449647,14.3178904466629 C4.677144362449647,13.7218904466629,3.444144362449647,12.5558904466629,2.758144362449647,11.028890446662901 C2.078144362449646,9.501890446662903,2.031144362449646,7.802890446662903,2.622144362449646,6.243890446662903 C3.216144362449646,4.6798904466629025,4.387144362449646,3.442890446662903,5.914144362449646,2.760890446662903 C7.437144362449646,2.078890446662903,9.137144362449646,2.0298904466629026,10.700144362449645,2.6258904466629027 C11.946144362449646,3.100890446662903,12.971144362449646,3.9538904466629026,13.686144362449646,5.049890446662903 L11.540144362449645,5.850890446662903 L16.856144362449648,10.238890446662904 Z";
            DiagramSize size = new DiagramSize() { Width = 18, Height = 16 };
            double top = element.OffsetY - BaseUtil.GetDoubleValue(element.ActualSize.Height) * element.Pivot.Y;
            double left = element.OffsetX - BaseUtil.GetDoubleValue(element.ActualSize.Width) * element.Pivot.X;
            bool visible = !canMask && selectorConstraints.HasFlag(SelectorConstraints.Rotate);
            double pivotX = left + element.Pivot.X * BaseUtil.GetDoubleValue(element.ActualSize.Width);
            Double pivotY = top;
            pivotX = (pivotX + transform.TX) * transform.Scale;
            pivotY = (pivotY + transform.TY) * transform.Scale;
            DiagramPoint point = new DiagramPoint(pivotX - BaseUtil.GetDoubleValue(size.Width) * 0.5, pivotY - 30 - BaseUtil.GetDoubleValue(size.Height) * 0.5);

            if (element.RotationAngle != 0 || element.ParentTransform != 0)
            {
                Matrix matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, element.RotationAngle + element.ParentTransform,
                    (transform.TX + element.OffsetX) * transform.Scale, (transform.TY + element.OffsetY) * transform.Scale);
                point = Matrix.TransformPointByMatrix(matrix, point);
            }
            PathAttributes pathAttributes = new PathAttributes
            {
                X = point.X,
                Y = point.Y,
                ID = "rotateThumb",
                Data = data,
                Angle = element.RotationAngle + element.ParentTransform,
                Stroke = "Black",
                StrokeWidth = .5,
                Fill = "#231f20",
                Width = 20,
                Height = 20,
                PivotX = 0,
                PivotY = 0,
                ClassValues = "e-diagram-rotate-handle",
                Visible = visible,
                Opacity = 1,
                DashArray = string.Empty
            };
            attributes.Add(pathAttributes);
        }


        internal static void RenderCircularHandle(ObservableCollection<BaseAttributes> attributes, string id, DiagramContainer element, double cx, double cy, bool visible, bool enableSelector, TransformFactor transform, bool? connected = false, bool? canMask = false, string className = null)
        {
            DiagramPoint newPoint = new DiagramPoint(cx, cy);
            if (element.RotationAngle != 0 || element.ParentTransform != 0)
            {
                Matrix matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, element.RotationAngle + element.ParentTransform, element.OffsetX, element.OffsetY);
                newPoint = Matrix.TransformPointByMatrix(matrix, newPoint);
            }
            CircleAttributes options = new CircleAttributes(element, null, false) { Fill = "White", ID = id, StrokeWidth = 1, Stroke = "Black", Radius = 7, CenterX = cx, CenterY = cy, ClassValues = className };
            if (!enableSelector)
            {
                options.ClassValues += " e-disabled";
            }
            if (id.Contains("segementThumb", StringComparison.InvariantCulture))
            {
                options.Radius = 5;
                options.Fill = "#e2e2e2";
            }
            if (connected != null && connected.Value)
            {
                options.Fill = "#8CC63F";
                options.ClassValues += " e-connected";
            }
            options.CenterX = (newPoint.X + transform.TX) * transform.Scale;
            options.CenterY = (newPoint.Y + transform.TY) * transform.Scale;
            options.Visible = !canMask.Value && visible;
            attributes.Add(options);
        }

        internal static void RenderBezierLine(string id, DiagramContainer wrapper, DiagramPoint start, DiagramPoint end, TransformFactor transform, ObservableCollection<BaseAttributes> attributes)
        {
            LineAttributes options = new LineAttributes(wrapper, transform, false) { ID = id, Stroke = "Black", DashArray = "3,3", Fill = "None", ClassValues = "e-diagram-bezier-line", StrokeWidth = 1, X = 0, Y = 0 };
            options.StartPoint = new DiagramPoint { X = (start.X + transform.TX) * transform.Scale, Y = (start.Y + transform.TY) * transform.Scale };
            options.EndPoint = new DiagramPoint { X = (end.X + transform.TX) * transform.Scale, Y = (end.Y + transform.TY) * transform.Scale };
            attributes.Add(options);
        }

        internal static void RenderBorder(DiagramContainer element, TransformFactor transform, bool enableNode, bool isBorderTickness, ObservableCollection<BaseAttributes> attributes)
        {
            RectAttributes option = new RectAttributes(element, transform)
            {
                Fill = "transparent",
                Stroke = "#097F7F",
                StrokeWidth = 1.2,
                DashArray = "5,3",
                Gradient = null,
                Angle = 0,
                ClassValues = "e-diagram-border",
                CornerRadius = 0
            };
            option.X *= transform.Scale;
            option.Y *= transform.Scale;
            option.Width *= transform.Scale;
            option.Height *= transform.Scale;
            option.Angle = element.RotationAngle;
            if (!enableNode)
            {
                option.ClassValues += " e-disabled";
            }
            if (isBorderTickness)
            {
                option.ClassValues += " e-thick-border";
            }
            attributes.Add(option);
        }


        internal static void RenderEndPointHandle(Connector connector, ThumbsConstraints thumbsConstraints, SelectorConstraints selectorConstraints, TransformFactor transformFactor, ObservableCollection<BaseAttributes> attributes)
        {
            DiagramPoint sourcePoint = connector.SourcePoint;
            DiagramPoint targetPoint = connector.TargetPoint;
            DiagramContainer wrapper = connector.Wrapper;
            bool connectedSource = connector.SourceWrapper != null;
            bool connectedTarget = connector.TargetWrapper != null;
            RenderCircularHandle(attributes, "connectorSourceThumb", wrapper, sourcePoint.X, sourcePoint.Y, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ConnectorSourceThumb), thumbsConstraints.HasFlag(ThumbsConstraints.ConnectorSource), transformFactor, connectedSource, false, "e-diagram-endpoint-handle e-targetend");
            RenderCircularHandle(attributes, "connectorTargetThumb", wrapper, targetPoint.X, targetPoint.Y, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ConnectorTargetThumb), thumbsConstraints.HasFlag(ThumbsConstraints.ConnectorTarget), transformFactor, connectedTarget, false, "e-diagram-endpoint-handle e-targetend");
            int i;
            if (connector.Constraints.HasFlag(ConnectorConstraints.DragSegmentThumb))
            {
                if ((connector.Type == ConnectorSegmentType.Straight || connector.Type == ConnectorSegmentType.Bezier) && connector.Segments.Count > 0)
                {
                    for (i = 0; i < connector.Segments.Count - 1; i++)
                    {
                        StraightSegment segment = connector.Segments[i] as StraightSegment;
                        RenderCircularHandle(attributes, "segementThumb_" + (i + 1), wrapper, segment.Point.X, segment.Point.Y, true, thumbsConstraints.HasFlag(ThumbsConstraints.ConnectorSource), transformFactor, connectedSource, false, string.Empty);
                    }
                }
                else
                {
                    for (i = 0; i < connector.Segments.Count; i++)
                    {
                        OrthogonalSegment orthoSegment = connector.Segments[i] as OrthogonalSegment;
                        RenderOrthogonalThumbs("orthoThumb_" + (i + 1), orthoSegment, transformFactor, attributes);
                    }
                }
            }

            if (connector.Type == ConnectorSegmentType.Bezier)
            {
                for (i = 0; i < connector.Segments.Count; i++)
                {
                    if (connector.Segments[i] is BezierSegment)
                    {
                        BezierSegment segment = connector.Segments[i] as BezierSegment;
                        DiagramPoint bezierPoint = !DiagramPoint.IsEmptyPoint(segment.Point1) ? segment.Point1 : segment.BezierPoint1;
                        RenderCircularHandle(attributes, "bezierPoint_" + (i + 1) + "_1", wrapper, bezierPoint.X, bezierPoint.Y, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ConnectorSourceThumb), thumbsConstraints.HasFlag(ThumbsConstraints.ConnectorSource), transformFactor, false, false, "e-diagram-bezier-handle e-source");
                        RenderBezierLine("bezierLine_" + (i + 1) + "_1", wrapper, segment.Points[0], bezierPoint, transformFactor, attributes);

                        DiagramPoint bezierPoint2 = !DiagramPoint.IsEmptyPoint(segment.Point2) ? segment.Point2 : segment.BezierPoint2;
                        RenderCircularHandle(attributes, "bezierPoint_" + (i + 1) + "_2", wrapper, bezierPoint2.X, bezierPoint2.Y, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ConnectorTargetThumb), thumbsConstraints.HasFlag(ThumbsConstraints.ConnectorTarget), transformFactor, false, false, "e-diagram-bezier-handle e-target");
                        RenderBezierLine("bezierLine_" + (i + 1) + "_2", wrapper, segment.Points[1], bezierPoint2, transformFactor, attributes);
                    }
                }
            }
        }

        internal static void RenderOrthogonalThumbs(string id, OrthogonalSegment segment, TransformFactor transform, ObservableCollection<BaseAttributes> attributes)
        {
            int j;
            for (j = 0; j < segment.Points.Count - 1; j++)
            {
                double length = DiagramPoint.DistancePoints(segment.Points[j], segment.Points[j + 1]);
                string orientation = Math.Round(segment.Points[j].Y * Math.Pow(10, 2)) / Math.Pow(10, 2) == Math.Round(segment.Points[j + 1].Y * Math.Pow(10, 2)) / Math.Pow(10, 2) ? "horizontal" : "vertical";
                bool visible = (length >= 50 && segment.AllowDrag);
                double x = (segment.Points[j].X + segment.Points[j + 1].X) / 2;
                double y = (segment.Points[j].Y + segment.Points[j + 1].Y) / 2;
                string path;
                int h;
                int v;
                if (orientation == "horizontal")
                {
                    path = "M0,7 L15,0 L30,7 L15,14 z"; h = -15; v = -7;
                }
                else
                {
                    path = "M7,0 L0,15 L7,30 L14,15 z"; h = -7; v = -15;
                }
                PathAttributes options = new PathAttributes
                {
                    X = (x + transform.TX) * transform.Scale + h,
                    Y = ((y + transform.TY) * transform.Scale) + transform.Scale + v,
                    Angle = 0,
                    Fill = "#e2e2e2",
                    Stroke = "black",
                    StrokeWidth = 1,
                    DashArray = "",
                    Data = path,
                    Width = 20,
                    Height = 20,
                    PivotX = 0,
                    PivotY = 0,
                    Opacity = 1,
                    Visible = visible,
                    ID = id + '_' + (j + 1)
                };
                attributes.Add(options);
            }
        }
        internal static Node GetNativeNode(ICommonElement diagramNodes, ObservableCollection<Node> childNodes)
        {
            Node node = null;
            for (int i = 0; i < childNodes.Count; i++)
            {
                if (diagramNodes.ID.Contains(childNodes[i].ID, StringComparison.InvariantCulture))
                {
                    node = childNodes[i];
                }
            }
            return node;
        }
        internal static NodeGroup GetGroupNode(string childNodeId, ObservableCollection<Node> diagramNodes = null)
        {
            NodeGroup groupNode = null;
            for (int i = 0; i < diagramNodes.Count; i++)
            {
                if (childNodeId == diagramNodes[i].ID && diagramNodes[i] is NodeGroup)
                {
                    groupNode = diagramNodes[i] as NodeGroup;
                    break;

                }
            }
            return groupNode;
        }
        internal static ObservableCollection<Node> GetChildNodes(Node node, ObservableCollection<Node> diagramNodes = null, ObservableCollection<Node> nodes = null)
        {
            ObservableCollection<Node> childNodes;
            if (nodes == null)
            {
                childNodes = new ObservableCollection<Node>();
            }
            else
            {
                childNodes = nodes;
            }
            if (node is NodeGroup groupNode)
            {
                if (groupNode.Children != null)
                {
                    for (int i = 0; i < groupNode.Children.Length; i++)
                    {
                        for (int j = 0; j < diagramNodes.Count; j++)
                        {
                            if (groupNode.Children[i] == diagramNodes[j].ID)
                            {
                                NodeGroup checkGroupNode = GetGroupNode(groupNode.Children[i], diagramNodes);
                                if (checkGroupNode != null)
                                {
                                    GetChildNodes(checkGroupNode, diagramNodes, childNodes);
                                }
                                else
                                {
                                    childNodes.Add(diagramNodes[j]);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                childNodes.Add(node);
            }
            return childNodes;
        }
        internal static void RenderResizeHandle(ObservableCollection<BaseAttributes> attributes, DiagramContainer element, ThumbsConstraints constraints, SelectorConstraints selectorConstraints, TransformFactor transform, bool? canMask = false, bool enableNode = false, bool? hideThumbs = false)
        {
            if (constraints.HasFlag(ThumbsConstraints.Rotate))
            {
                RenderPivotLine(element, transform, selectorConstraints, canMask != null && canMask.Value, attributes);
                RenderRotateThumb(element, transform, selectorConstraints, canMask.Value, attributes);
            }
            RenderBorder(element, transform, enableNode, hideThumbs != null && hideThumbs.Value, attributes);
            double left = element.OffsetX - BaseUtil.GetDoubleValue(element.ActualSize.Width) * element.Pivot.X;
            double top = element.OffsetY - BaseUtil.GetDoubleValue(element.ActualSize.Height) * element.Pivot.Y;
            if (element.ActualSize.Height != null && element.ActualSize.Width != null)
            {
                double height = element.ActualSize.Height.Value;
                double width = element.ActualSize.Width.Value;
                if (hideThumbs != null && !hideThumbs.Value)
                {
                    if (width >= 40 && height >= 40)
                    {
                        if (selectorConstraints.HasFlag(SelectorConstraints.ResizeNorthWest))
                            RenderCircularHandle(attributes, "ResizeNorthWest", element, left, top, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeNorthWest), constraints.HasFlag(ThumbsConstraints.ResizeNorthWest), transform, false, canMask, "e-diagram-resize-handle e-northwest");
                        if (selectorConstraints.HasFlag(SelectorConstraints.ResizeNorthEast))
                            RenderCircularHandle(attributes, "ResizeNorthEast", element, left + width, top, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeNorthEast), constraints.HasFlag(ThumbsConstraints.ResizeNorthEast), transform, false, canMask, "e-diagram-resize-handle e-northeast");
                        if (selectorConstraints.HasFlag(SelectorConstraints.ResizeSouthWest))
                            RenderCircularHandle(attributes, "ResizeSouthWest", element, left, top + height, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeSouthWest), constraints.HasFlag(ThumbsConstraints.ResizeSouthWest), transform, false, canMask, "e-diagram-resize-handle e-southwest");
                        if (selectorConstraints.HasFlag(SelectorConstraints.ResizeSouthEast))
                            RenderCircularHandle(attributes, "ResizeSouthEast", element, left + width, top + height, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeSouthEast), constraints.HasFlag(ThumbsConstraints.ResizeSouthEast), transform, false, canMask, "e-diagram-resize-handle e-southeast");
                    }
                    if (selectorConstraints.HasFlag(SelectorConstraints.ResizeNorth))
                        RenderCircularHandle(attributes, "ResizeNorth", element, left + width / 2, top, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeNorth), constraints.HasFlag(ThumbsConstraints.ResizeNorth), transform, false, canMask, "e-diagram-resize-handle e-north");
                    if (selectorConstraints.HasFlag(SelectorConstraints.ResizeSouth))
                        RenderCircularHandle(attributes, "ResizeSouth", element, left + width / 2, top + height, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeSouth), constraints.HasFlag(ThumbsConstraints.ResizeSouth), transform, false, canMask, "e-diagram-resize-handle e-south");
                    if (selectorConstraints.HasFlag(SelectorConstraints.ResizeWest))
                        RenderCircularHandle(attributes, "ResizeWest", element, left, top + height / 2, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeWest), constraints.HasFlag(ThumbsConstraints.ResizeWest), transform, false, canMask, "e-diagram-resize-handle e-west");
                    if (selectorConstraints.HasFlag(SelectorConstraints.ResizeEast))
                        RenderCircularHandle(attributes, "ResizeEast", element, left + width, top + height / 2, ConstraintsUtil.CanShowCorner(selectorConstraints, SelectorConstraints.ResizeEast), constraints.HasFlag(ThumbsConstraints.ResizeEast), transform, false, canMask, "e-diagram-resize-handle e-east");
                }
            }
        }

        internal static void RenderUserHandle(ObservableCollection<BaseAttributes> attributes, DiagramSelectionSettings selectorItem, TransformFactor transform)
        {
            DiagramElement wrapper = selectorItem.Wrapper;
            int pathPadding = 5;
            ImageElement element = new ImageElement();
            DiagramHtmlElement templateContent = new DiagramHtmlElement();
            foreach (UserHandle obj in selectorItem.UserHandles)
            {
                DiagramPoint newPoint = DiagramUtil.GetUserHandlePosition(selectorItem, obj, transform);
                newPoint.X = (newPoint.X + transform.TX) * transform.Scale;
                newPoint.Y = (newPoint.Y + transform.TY) * transform.Scale;
                if (obj.Visible)
                {
                    obj.Visible = selectorItem.Constraints.HasFlag(SelectorConstraints.UserHandle);
                }
                if (!string.IsNullOrEmpty(obj.PathData))
                {
                    string data = !string.IsNullOrEmpty(obj.PathData) ? obj.PathData : string.Empty;
                    if (obj.Parent == null)
                    {
                        obj.Parent = selectorItem;
                    }
                    List<PathSegment> arrayCollection = PathUtil.ProcessPathData(data);
                    arrayCollection = PathUtil.SplitArrayCollection(arrayCollection);
                    DiagramRect pathSize = DomUtil.MeasurePath(!string.IsNullOrEmpty(data) ? data : string.Empty);
                    if (pathSize != null)
                    {
                        CircleAttributes pathCircle = new CircleAttributes(wrapper, null, false) { Fill = obj.BackgroundColor, ID = obj.Name + HANDLE, StrokeWidth = obj.BorderWidth, Stroke = obj.BorderColor, Radius = obj.Size * 0.5, CenterX = newPoint.X, CenterY = newPoint.Y, ClassValues = CIRCLARHANDLECLASS, Angle = 0, Visible = obj.Visible, Opacity = 1 };
                        if (pathCircle.Visible)
                            attributes.Add(pathCircle);
                        double scaleX = (obj.Size - 0.45 * obj.Size) / pathSize.Width;
                        double scaleY = (obj.Size - 0.45 * obj.Size) / pathSize.Height;
                        string newData = PathUtil.TransformPath(arrayCollection, scaleX, scaleY, true, pathSize.X, pathSize.Y, 0, 0);
                        PathAttributes path = new PathAttributes()
                        {
                            X = newPoint.X - ((scaleX * pathSize.Width) / 2),
                            Y = newPoint.Y - ((scaleY * pathSize.Height) / 2),
                            Angle = 0,
                            ID = obj.Name + HANDLEPATHID,
                            ClassValues = HANDLEPATHCLASS,
                            Fill = obj.PathColor,
                            Stroke = obj.BackgroundColor,
                            StrokeWidth = 0.5,
                            DashArray = string.Empty,
                            Data = newData,
                            Width = obj.Size - pathPadding,
                            Height = obj.Size - pathPadding,
                            PivotX = 0,
                            PivotY = 0,
                            Opacity = 1,
                            Visible = obj.Visible
                        };
                        attributes.Add(path);
                    }

                }
                else if (!string.IsNullOrEmpty(obj.Source))
                {
                    ImageAttributes image = GetImageAttributes(element, transform, false);
                    image.Width = obj.Size;
                    image.Height = obj.Size;
                    image.X = newPoint.X - (obj.Size / 2);
                    image.Y = newPoint.Y - (obj.Size / 2);
                    image.SourceWidth = obj.Size;
                    image.SourceHeight = obj.Size;
                    image.Alignment = element.ImageAlign;
                    image.Source = obj.Source;
                    image.Scale = element.ImageScale;
                    image.Visible = obj.Visible;
                    image.Description = string.IsNullOrEmpty(obj.Name) ? "User handle" : obj.Name;
                    image.ID = obj.Name + '_';
                    attributes.Add(image);
                }
                else
                {
                    templateContent.OffsetX = newPoint.X;
                    templateContent.OffsetY = newPoint.Y;
                    templateContent.ID = obj.Name + HANDLETEMPLATEID;
                    templateContent.Visible = obj.Visible;
                    templateContent.RelativeMode = RelativeMode.Object;
                    templateContent.Measure(new DiagramSize() { Height = obj.Size, Width = obj.Size });
                    templateContent.Arrange(templateContent.DesiredSize, null);
                    obj.Bounds = templateContent.Bounds;
                }
            }
        }
    }
}

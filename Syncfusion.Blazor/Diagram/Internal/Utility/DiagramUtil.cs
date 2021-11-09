using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents how to handle constraint related validation/checking to render objects in the diagram.
    /// </summary>
    internal static class DiagramUtil
    {
        internal static DiagramElement UpdatePortEdges(DiagramElement portContent, FlipDirection flip, PointPort port)
        {
            double offsetX = port.Offset.X;
            double offsetY = port.Offset.Y;
            if (flip == FlipDirection.Horizontal)
            {
                offsetX = 1 - port.Offset.X;
                offsetY = port.Offset.Y;
            }
            else if (flip == FlipDirection.Vertical)
            {
                offsetX = port.Offset.X;
                offsetY = 1 - port.Offset.Y;
            }
            else if (flip == FlipDirection.Both)
            {
                offsetX = 1 - port.Offset.X;
                offsetY = 1 - port.Offset.Y;
            }
            portContent.SetOffsetWithRespectToBounds(offsetX, offsetY, UnitMode.Fraction);
            return portContent;
        }

        internal static ICommonElement GetWrapper(NodeBase node, DiagramContainer nodeContainer, string id)
        {
            ICommonElement wrapper = null;
            id = nodeContainer.ID + "_" + id;
            DiagramContainer container = nodeContainer is Canvas ? nodeContainer : GetPortContainer(node);
            for (int i = 0; i < container.Children.Count; i++)
            {
                if (id == ((DiagramElement)container.Children[i]).ID)
                {
                    wrapper = (ICommonElement)container.Children[i];
                }
            }
            return wrapper;
        }
        internal static ICommonElement RemoveBpmnWrapper(NodeBase node, DiagramContainer nodeContainer, string id)
        {
            ICommonElement wrapper = null;
            string connectorId = node.ID + "_" + id + "_connector";
            string nodeId = node.ID + "_textannotation_" + id; ;
            DiagramContainer container = nodeContainer is Canvas ? nodeContainer : GetPortContainer(node);
            for (int i = 0; i < container.Children.Count; i++)
            {
                if (connectorId == ((DiagramElement)container.Children[i]).ID)
                {
                    wrapper = container.Children[i];
                    ((Canvas)node.Wrapper.Children[0]).Children.Remove(wrapper);
                }
                if (nodeId == ((DiagramElement)container.Children[i]).ID)
                {
                    wrapper = container.Children[i];
                    ((Canvas)node.Wrapper.Children[0]).Children.Remove(wrapper);
                }
            }

            if (node.Parent is SfDiagramComponent diagram)
                for (int i = 0; i < diagram.DiagramContent.TextAnnotationConnectors.Count; i++)
                {
                    if (connectorId == (diagram.DiagramContent.TextAnnotationConnectors[i]).ID)
                    {
                        diagram.DiagramContent.TextAnnotationConnectors.RemoveAt(i);
                    }
                }

            return wrapper;
        }
        internal static DiagramContainer GetPortContainer(NodeBase actualObject)
        {
            if (actualObject is Node nodeObj && nodeObj.Wrapper.Children != null)
            {
                for (int i = 0; i < nodeObj.Wrapper.Children.Count; i++)
                {
                    if (((DiagramElement)nodeObj.Wrapper.Children[i]).ID == nodeObj.ID + "group_container")
                    {
                        return nodeObj.Wrapper.Children[i] as DiagramContainer;
                    }
                }
            }
            return actualObject.Wrapper;
        }
        internal static DiagramPoint GetUserHandlePosition(DiagramSelectionSettings selectorItem, UserHandle handle, TransformFactor transform)
        {
            DiagramElement wrapper = selectorItem.Wrapper;
            DiagramRect bounds = wrapper.Bounds;
            Matrix matrix = Matrix.IdentityMatrix();
            double offset = handle.Offset;
            double size = handle.Size / transform.Scale;
            Margin margin = handle.Margin;
            double left = BaseUtil.GetDoubleValue(wrapper.OffsetX - wrapper.ActualSize.Width * wrapper.Pivot.X);
            double top = BaseUtil.GetDoubleValue(wrapper.OffsetY - wrapper.ActualSize.Height * wrapper.Pivot.Y);
            DiagramPoint point = new DiagramPoint() { X = 0, Y = 0 };

            if (selectorItem.Nodes.Count > 0)
            {
                switch (handle.Side)
                {
                    case Direction.Top:
                        point.X += left + bounds.Width * offset;
                        point.Y += top - (size / 2 + 12.5);
                        break;
                    case Direction.Bottom:
                        point.X += left + offset * bounds.Width;
                        point.Y += BaseUtil.GetDoubleValue(top + wrapper.ActualSize.Height + (size / 2 + 12.5));
                        break;
                    case Direction.Left:
                        point.X += left - (size / 2 + 12.5);
                        point.Y += top + offset * bounds.Height;
                        break;
                    case Direction.Right:
                        point.X += BaseUtil.GetDoubleValue(left + wrapper.ActualSize.Width + (size / 2 + 12.5));
                        point.Y += top + offset * bounds.Height;
                        break;
                }
                point.X += ((margin.Left - margin.Right) / transform.Scale) +
                    (size / 2) * (handle.HorizontalAlignment == HorizontalAlignment.Center ? 0 : (handle.HorizontalAlignment == HorizontalAlignment.Right ? 1 : -1));
                point.Y += ((margin.Top - margin.Bottom) / transform.Scale) +
                    (size / 2) * (handle.VerticalAlignment == VerticalAlignment.Center ? 0 : (handle.VerticalAlignment == VerticalAlignment.Top ? -1 : 1));
            }
            else if (selectorItem.Connectors.Count > 0)
            {
                Connector connector = selectorItem.Connectors[0] as Connector;
                PathAnnotation annotation = new PathAnnotation() { Offset = offset };
                GetLoop connectorOffset = GetOffsetOfConnector(connector.IntermediatePoints, annotation);
                int index = Convert.ToInt32(connectorOffset.Index);
                point = connectorOffset.Point;
                GetLoop getPointLoop = GetAnnotationPosition(connector.IntermediatePoints, annotation);
                double angle = getPointLoop.Angle;
                Matrix.RotateMatrix(matrix, -angle, connector.IntermediatePoints[index].X, connector.IntermediatePoints[index].Y);
                point = Matrix.TransformPointByMatrix(matrix, point);
                point.X += (margin.Left - margin.Right) +
                    (size / 2) * (handle.HorizontalAlignment == HorizontalAlignment.Center ? 0 : (handle.HorizontalAlignment == HorizontalAlignment.Right ? -1 : 1));
                point.Y += (margin.Top - margin.Bottom) +
                    (size / 2) * (handle.VerticalAlignment == VerticalAlignment.Center ? 0 : (handle.VerticalAlignment == VerticalAlignment.Top ? 1 : -1));
                matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, angle, connector.IntermediatePoints[index].X, connector.IntermediatePoints[index].Y);
                point = Matrix.TransformPointByMatrix(matrix, point);
            }
            if (wrapper.RotationAngle != 0 || wrapper.ParentTransform != 0)
            {
                matrix = Matrix.IdentityMatrix();
                Matrix.RotateMatrix(matrix, wrapper.RotationAngle + wrapper.ParentTransform, wrapper.OffsetX, wrapper.OffsetY);
                point = Matrix.TransformPointByMatrix(matrix, point);
            }
            return point;
        }
        internal static void UpdateShape(Node node, Dictionary<string, object> shape, object newValue)
        {
            bool isUndoRedo = node.Parent is SfDiagramComponent diagramComponent && diagramComponent.DiagramAction.HasFlag(DiagramAction.UndoRedo);
            Shapes type = node.Shape.Type;
            if (shape.ContainsKey("Type"))
            {
                type = (Shapes)Enum.Parse(typeof(Shapes), ((PropertyChangeValues)shape["Type"]).NewValue.ToString());
            }
            switch (type)
            {
                case Shapes.Path:
                    PathElement pathContent = new PathElement
                    {
                        Data = shape.ContainsKey("Data") ? ((PropertyChangeValues)shape["Data"]).NewValue.ToString() : string.Empty
                    };
                    UpdateShapeContent(pathContent, node);
                    if (isUndoRedo)
                    {
                        node.Shape = new PathShape
                        {
                            Type = Shapes.Path
                        };
                        ((PathShape)node.Shape).Data = pathContent.Data;
                    }
                    break;
                case Shapes.Basic:
                    DiagramElement element;
                    BasicShapeType basicShape = (BasicShapeType)Enum.Parse(typeof(BasicShapeType), ((PropertyChangeValues)shape["Shape"]).NewValue.ToString());
                    element = (basicShape == BasicShapeType.Rectangle) ? new DiagramElement() : new PathElement();
                    if (basicShape == BasicShapeType.Polygon)
                    {
                        ((PathElement)element).Data = shape.ContainsKey("Points") ? PathUtil.GetPolygonPath(((PropertyChangeValues)shape["Points"]).NewValue as DiagramPoint[]) : string.Empty;
                    }
                    else
                    {
                        if (element is PathElement pathElement)
                            pathElement.Data = Dictionary.GetShapeData(basicShape.ToString());
                    }
                    if (basicShape == BasicShapeType.Rectangle)
                    {
                        element.CornerRadius = shape.ContainsKey("CornerRadius") ? (double)((PropertyChangeValues)shape["CornerRadius"]).NewValue : 0;
                    }
                    UpdateShapeContent(element, node);
                    if (isUndoRedo)
                    {
                        node.Shape = new BasicShape();
                        ((BasicShape)node.Shape).Type = Shapes.Basic;
                        ((BasicShape)node.Shape).Shape = basicShape;
                    }
                    break;
                case Shapes.Flow:
                    PathElement flowShapeElement = new PathElement();
                    string shapes = ((PropertyChangeValues)shape["Shape"]).NewValue.ToString();
                    flowShapeElement.Data = Dictionary.GetShapeData(shapes);
                    UpdateShapeContent(flowShapeElement, node);
                    if (isUndoRedo)
                    {
                        node.Shape = new FlowShape();
                        ((FlowShape)node.Shape).Type = Shapes.Flow;
                        ((FlowShape)node.Shape).Shape = (FlowShapeType)Enum.Parse(typeof(FlowShapeType), ((PropertyChangeValues)shape["Shape"]).NewValue.ToString()); ;
                    }
                    break;
                case Shapes.Image:
                    ImageElement imageElement = new ImageElement();
                    if (shape.ContainsKey("Source"))
                    {
                        string source = ((PropertyChangeValues)shape["Source"]).NewValue.ToString();
                        imageElement.Source = source;
                    }
                    if (shape.ContainsKey("Scale") && node.Shape.Type.ToString() == "Image")
                    {
                        Scale scale = (Scale)Enum.Parse(typeof(Scale), ((PropertyChangeValues)shape["Scale"]).NewValue.ToString());
                        imageElement.ImageScale = scale;
                        imageElement.ImageAlign = ((ImageShape)node.Shape).ImageAlign;
                        imageElement.Source = ((ImageShape)node.Shape).Source;
                    }
                    if (shape.ContainsKey("Align") && node.Shape.Type.ToString() == "Image")
                    {
                        ImageAlignment align = (ImageAlignment)Enum.Parse(typeof(ImageAlignment), ((PropertyChangeValues)shape["Align"]).NewValue.ToString());
                        imageElement.ImageAlign = align;
                        imageElement.ImageScale = ((ImageShape)node.Shape).Scale;
                        imageElement.Source = ((ImageShape)node.Shape).Source;
                    }
                    UpdateShapeContent(imageElement, node);
                    if (isUndoRedo)
                    {
                        node.Shape = new ImageShape
                        {
                            Type = Shapes.Image
                        };
                        ((ImageShape)node.Shape).Source = imageElement.Source;
                    }
                    break;
                case Shapes.Bpmn:
                    BpmnDiagrams bpmn = new BpmnDiagrams();
                    bpmn.UpdateBpmn(node, shape, newValue);
                    if (isUndoRedo)
                        node.Shape.Type = Shapes.Bpmn;
                    break;
            }
        }
        internal static DiagramObjectCollection<NodeBase> SortByXY(DiagramObjectCollection<NodeBase> objects, DistributeOptions option)
        {
            int i;
            for (i = 0; i < objects.Count; i++)
            {
                DiagramRect b = BaseUtil.GetBounds(objects[i].Wrapper);
                int j;
                for (j = 0; j < objects.Count; j++)
                {
                    DiagramRect bounds = BaseUtil.GetBounds(objects[j].Wrapper);
                    object temp;
                    if (option == DistributeOptions.Top || option == DistributeOptions.Bottom || option == DistributeOptions.BottomToTop || option == DistributeOptions.Middle)
                    {
                        if (b.Center.Y > bounds.Center.Y)
                        {
                            temp = objects[i];
                            objects[i] = objects[j];
                            objects[j] = (NodeBase)temp;
                        }
                    }
                    else
                    {
                        if (b.Center.X > bounds.Center.X)
                        {
                            temp = objects[i];
                            objects[i] = objects[j];
                            objects[j] = (NodeBase)temp;
                        }
                    }
                }
            }
            return objects;
        }

        internal static void SetParentForObservableCollection(object collection, IDiagramObject parent, string propertyName)
        {
            IList childValue = (IList)(collection);
            if (childValue != null)
            {
                foreach (DiagramPoint item in childValue)
                {
                    item.SetParent(parent, propertyName);
                    //Type type = item.GetType();
                    //PropertyInfo property = item.GetType().GetProperty("Parent", BindingFlags.Instance | BindingFlags.NonPublic);
                    //if (property != null)
                    //{
                    //    MethodInfo methodInfoPptName = type.GetMethod("SetParent", BindingFlags.Instance | BindingFlags.NonPublic);
                    //    methodInfoPptName.Invoke(item, new object[] { parent, propertyName });
                    //}
                }
            }
        }

        internal static void UpdateShapeContent(DiagramElement content, Node actualObject)
        {
            ICommonElement childElement = actualObject.Wrapper.Children[0];
            ObservableCollection<ICommonElement> childCollection = actualObject.Wrapper.Children;

            content.Width = actualObject.Width;
            content.Height = actualObject.Height;
            content.MinHeight = actualObject.MinHeight;
            content.MaxHeight = actualObject.MaxHeight;
            content.MinWidth = actualObject.MinWidth;
            content.MaxWidth = actualObject.MaxWidth;

            content.HorizontalAlignment = childElement.HorizontalAlignment;
            content.VerticalAlignment = childElement.VerticalAlignment;
            content.RelativeMode = childElement.RelativeMode;
            content.Visible = childElement.Visible;

            content.ID = childElement.ID;
            content.Style = actualObject.Style;
            content.Shadow = childElement.Shadow;

            childCollection.RemoveAt(0);
            childCollection.Insert(0, content);
        }
        internal static List<DiagramPoint> GetPoints(Corners corners, double padding = 0)
        {
            DiagramPoint left = new DiagramPoint { X = corners.TopLeft.X - padding, Y = corners.TopLeft.Y };
            DiagramPoint right = new DiagramPoint { X = corners.TopRight.X + padding, Y = corners.TopRight.Y };
            DiagramPoint top = new DiagramPoint { X = corners.BottomRight.X, Y = corners.BottomRight.Y - padding };
            DiagramPoint bottom = new DiagramPoint { X = corners.BottomLeft.X, Y = corners.BottomLeft.Y + padding };

            List<DiagramPoint> line = new List<DiagramPoint> { left, right, top, bottom };
            return line;
        }
        internal static List<DiagramPoint> GetPathPoints(PathElement element)
        {
            List<DiagramPoint> line = Dictionary.GetPathPointsCollection(element.Data);
            DiagramRect pathBounds = new DiagramRect() { X = 150, Y = 150, Width = 300, Height = 300 };
            if (line == null)
            {
                line = new List<DiagramPoint>(Dictionary.GetCustomPathPointsCollection(element.Data));
            }
            List<DiagramPoint> secondList = line.Select(el => new DiagramPoint() { X = el.X, Y = el.Y }).ToList();
            PathElement refObject = new PathElement();
            double angle = element.RotationAngle + element.ParentTransform;
            double elementWidth = BaseUtil.GetDoubleValue(element.Width ?? element.ActualSize.Width);
            double elementHeight = BaseUtil.GetDoubleValue(element.Height ?? element.ActualSize.Height);
            DiagramRect tempBounds = new DiagramRect
            {
                X = BaseUtil.GetDoubleValue(element.OffsetX - (elementWidth * element.Pivot.X)),
                Y = BaseUtil.GetDoubleValue(element.OffsetY - (elementHeight * element.Pivot.Y)),
                Width = BaseUtil.GetDoubleValue(elementWidth),
                Height = BaseUtil.GetDoubleValue(elementHeight)
            };
            if (secondList != null)
            {
                int i;
                for (i = 0; i < secondList.Count; i++)
                {
                    double sw = BaseUtil.GetDoubleValue(elementWidth / pathBounds.Width);
                    double sh = BaseUtil.GetDoubleValue(elementHeight / pathBounds.Height);
                    refObject.OffsetX = secondList[i].X;
                    refObject.OffsetY = secondList[i].Y;
                    Matrix matrix = Matrix.IdentityMatrix();
                    Matrix.ScaleMatrix(matrix, sw, sh, pathBounds.X, pathBounds.Y);
                    DiagramPoint newPosition = Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = refObject.OffsetX, Y = refObject.OffsetY });
                    secondList[i].X = newPosition.X;
                    secondList[i].Y = newPosition.Y;
                }
                if (tempBounds.TopLeft.X != 0)
                {
                    for (int j = 0; j < secondList.Count; j++)
                    {
                        secondList[j].X += tempBounds.TopLeft.X;
                        secondList[j].Y += tempBounds.TopLeft.Y;
                    }
                }
                if (!Equals(elementWidth, pathBounds.Width) || !Equals(elementHeight, pathBounds.Height))
                {
                    for (int k = 0; k < secondList.Count; k++)
                    {
                        secondList[k].X += BaseUtil.GetDoubleValue((elementWidth - pathBounds.Width) / 2);
                        secondList[k].Y += BaseUtil.GetDoubleValue((elementHeight - pathBounds.Height) / 2);
                    }
                }
            }
            return secondList;
        }

        internal static Segment GetLineSegment(double x1, double y1, double x2, double y2)
        {
            return new Segment() { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
        }

        internal static DiagramPoint Intersect2(DiagramPoint start1, DiagramPoint end1, DiagramPoint start2, DiagramPoint end2)
        {
            DiagramPoint point = new DiagramPoint { X = 0, Y = 0 };
            Segment lineUtil1 = GetLineSegment(start1.X, start1.Y, end1.X, end1.Y);
            Segment lineUtil2 = GetLineSegment(start2.X, start2.Y, end2.X, end2.Y);
            Intersection line3 = Intersect3(lineUtil1, lineUtil2);
            if (line3.Enabled)
            {
                return line3.IntersectPt;
            }
            else
            {
                return point;
            }
        }
        internal static Intersection Intersect3(Segment lineUtil1, Segment lineUtil2)
        {
            DiagramPoint point = new DiagramPoint { X = 0, Y = 0 };
            Segment l1 = lineUtil1;
            Segment l2 = lineUtil2;
            double d = (l2.Y2 - l2.Y1) * (l1.X2 - l1.X1) - (l2.X2 - l2.X1) * (l1.Y2 - l1.Y1);
            double na = (l2.X2 - l2.X1) * (l1.Y1 - l2.Y1) - (l2.Y2 - l2.Y1) * (l1.X1 - l2.X1);
            double nb = (l1.X2 - l1.X1) * (l1.Y1 - l2.Y1) - (l1.Y2 - l1.Y1) * (l1.X1 - l2.X1);
            if (d == 0 || ((lineUtil1.X1.Equals(lineUtil2.X1) || lineUtil1.Y1.Equals(lineUtil2.Y1)) &&
                (lineUtil1.X2.Equals(lineUtil2.X2) || lineUtil1.Y2.Equals(lineUtil2.Y2)) && ((na == 0 || nb == 0) && d > 0)))
            {
                return new Intersection { Enabled = false, IntersectPt = point };
            }
            double ua = na / d;
            double ub = nb / d;
            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                point.X = l1.X1 + (ua * (l1.X2 - l1.X1));
                point.Y = l1.Y1 + (ua * (l1.Y2 - l1.Y1));
                return new Intersection { Enabled = true, IntersectPt = point };
            }
            return new Intersection { Enabled = false, IntersectPt = point };
        }

        internal static PointPort GetInOutConnectPorts(Node node, bool isInConnect)
        {
            PointPort port = null;
            if (node != null && node.Ports != null)
            {
                ObservableCollection<PointPort> ports = node.Ports;
                int i;
                for (i = 0; i < ports.Count; i++)
                {
                    if (isInConnect)
                    {
                        if (ConstraintsUtil.CheckPortConstraints(ports[i], PortConstraints.InConnect))
                        {
                            port = ports[i];
                        }
                    }
                    else
                    {
                        if (ConstraintsUtil.CheckPortConstraints(ports[i], PortConstraints.OutConnect))
                        {
                            port = ports[i];
                        }
                    }
                }
            }
            return port;
        }

        internal static void UpdateConnector(Connector connector, List<DiagramPoint> points)
        {
            List<DiagramPoint> anglePoint;
            connector.IntermediatePoints = points;
            connector.UpdateSegmentElement(connector, points, connector.Wrapper.Children[0] as PathElement);
            DecoratorSettings srcDecorator = connector.SourceDecorator;
            if (connector.Type == ConnectorSegmentType.Bezier)
            {
                BezierSegment firstSegment = (connector.Segments[0] as BezierSegment);
                BezierSegment lastSegment = connector.Segments[^1] as BezierSegment;
                anglePoint = new List<DiagramPoint>(){ !DiagramPoint.IsEmptyPoint(lastSegment?.Point2) ? lastSegment?.Point2 : lastSegment?.BezierPoint2,
                    !DiagramPoint.IsEmptyPoint(firstSegment?.Point1) ? firstSegment?.Point1 : firstSegment?.BezierPoint1 };
            }
            else
            {
                anglePoint = connector.IntermediatePoints;
            }
            points = Connector.ClipDecorators(connector, points);
            ICommonElement element = connector.Wrapper.Children[0];
            ((PathElement)element).CanMeasurePath = true;
            element = connector.Wrapper.Children[1];
            Connector.UpdateDecoratorElement(element as DiagramElement, points[0], anglePoint[1], srcDecorator);
            DecoratorSettings tarDecorator = connector.TargetDecorator;
            element = connector.Wrapper.Children[2];
            Connector.UpdateDecoratorElement(element as DiagramElement, points[^1], anglePoint[^2], tarDecorator);
        }
        internal static void RemoveItem(List<string> array, string item)
        {
            int index = array.IndexOf(item);
            if (index >= 0)
            {
                array.RemoveAt(index);
            }
        }

        internal static object AsDictionary(object source, object old)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            if (source != null)
            {
                Type sourcePropertyInfo = source.GetType().GetTypeInfo();
                if (sourcePropertyInfo.Name != "String" && !sourcePropertyInfo.IsValueType && !sourcePropertyInfo.IsGenericType)
                {
                    if (sourcePropertyInfo.Name == "Point[]" || sourcePropertyInfo.Name == "String[]")
                    {
                        return keyValuePairs;
                    }
                    else
                    {
                        foreach (PropertyInfo propertyInfo in source.GetType().GetProperties())
                        {
                            string name = propertyInfo.Name;
                            object value = propertyInfo.GetValue(source, null);
                            object oldvalue = (old != null && source.GetType() == old.GetType()) ? propertyInfo.GetValue(old, null) : null;
                            if (value != null && value.GetType().Name != "Point[]" && (!propertyInfo.PropertyType.IsValueType && propertyInfo.PropertyType.Name != "String" && !propertyInfo.PropertyType.IsGenericType || value is IList))
                            {
                                Dictionary<string, object> curProperty = AsDictionary(value, oldvalue) as Dictionary<string, object>;
                                keyValuePairs.Add(name, curProperty);
                            }
                            else
                            {
                                PropertyChangeValues propertyChangeValues = new PropertyChangeValues() { NewValue = value, OldValue = oldvalue };
                                keyValuePairs.Add(name, propertyChangeValues);
                            }
                        }
                    }
                }
                else
                {
                    if (source is IList list)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            object obj = list[i];
                            object oldObj = old is IList oldList && oldList.Count > 0 && oldList.Count > i && oldList[i] != null ? oldList[i] : null;
                            Dictionary<string, object> curProperty = AsDictionary(obj, oldObj) as Dictionary<string, object>;
                            keyValuePairs.Add(i.ToString(CultureInfo.InvariantCulture), curProperty);
                        }
                        return keyValuePairs;
                    }
                    return source;
                }
            }
            return keyValuePairs;
        }
        internal static GetLoop GetFixedUserHandlePosition(List<DiagramPoint> pts, ConnectorFixedUserHandle annotation)
        {
            GetLoop getLoop = GetOffsetOfConnector(pts, null, annotation);
            double angle = DiagramPoint.FindAngle(pts[Convert.ToInt32(getLoop.Index)], pts[Convert.ToInt32(getLoop.Index + 1)]);
            double alignedNumber = GetAlignedPosition(null, annotation);
            DiagramPoint point = DiagramPoint.Transform(getLoop.Point, angle + 45, alignedNumber);
            getLoop.Point = point;
            getLoop.Angle = angle;
            return getLoop;
        }
        internal static GetLoop GetAnnotationPosition(List<DiagramPoint> pts, PathAnnotation annotation)
        {
            GetLoop getLoop = GetOffsetOfConnector(pts, annotation);
            double angle = DiagramPoint.FindAngle(pts[Convert.ToInt32(getLoop.Index)], pts[Convert.ToInt32(getLoop.Index + 1)]);
            double alignedNumber = GetAlignedPosition(annotation);
            DiagramPoint point = DiagramPoint.Transform(getLoop.Point, angle + 45, alignedNumber);
            getLoop.Point = point;
            getLoop.Angle = angle;
            return getLoop;
        }
        private static double GetAlignedPosition(PathAnnotation annotation, ConnectorFixedUserHandle userHandle = null)
        {
            double cnst = 0;
            if (annotation != null)
            {
                cnst = annotation.Content != null ? 10 : 0;
            }
            double state = 0;
            if (annotation != null)
            {
                switch (annotation.Alignment)
                {
                    case AnnotationAlignment.Center:
                        state = 0;
                        break;
                    case AnnotationAlignment.Before:
                        state = -((0) / 2 + cnst);
                        break;
                    case AnnotationAlignment.After:
                        state = ((0) / 2 + cnst);
                        break;
                }
            }
            else
            {
                if (userHandle != null)
                    switch (userHandle.Alignment)
                    {
                        case FixedUserHandleAlignment.Center:
                            state = 0;
                            break;
                        case FixedUserHandleAlignment.Before:
                            state = -((0) / 2 + cnst);
                            break;
                        case FixedUserHandleAlignment.After:
                            state = ((0) / 2 + cnst);
                            break;
                    }
            }

            return state;
        }
        private static GetLoop GetOffsetOfConnector(List<DiagramPoint> points, PathAnnotation annotation, ConnectorFixedUserHandle userHandle = null)
        {
            double length = 0;
            double offset;
            if (annotation != null)
            {
                offset = annotation.Offset;
            }
            else
            {
                offset = userHandle.Offset;
            }
            DiagramPoint point = new DiagramPoint();
            List<double> lengths = new List<double>();
            double? prevLength = null;
            int kCount = 0;
            for (int j = 0; j < points.Count - 1; j++)
            {
                length += DiagramPoint.DistancePoints(points[j], points[j + 1]);
                lengths.Add(length);
            }
            double offsetLength = offset * length;
            for (int k = 0; k < lengths.Count; k++)
            {
                if (lengths[k] >= offsetLength)
                {
                    double angle = DiagramPoint.FindAngle(points[k], points[k + 1]);
                    point = DiagramPoint.Transform(points[k], angle, offsetLength - (prevLength != null ? prevLength.Value : 0));
                    kCount = k;
                    return new GetLoop() { Point = point, Index = kCount };
                }
                prevLength = lengths[k];
            }
            return new GetLoop() { Point = point, Index = kCount };
        }
        internal static GetAlignment AlignLabelOnSegments(PathAnnotation obj, double ang, List<DiagramPoint> pts)
        {
            ang %= 360;
            int fourty5 = 45;
            int one35 = 135;
            int two25 = 225;
            int three15 = 315;
            VerticalAlignment vAlign = obj.VerticalAlignment;
            HorizontalAlignment hAlign = obj.HorizontalAlignment;
            switch (obj.Alignment)
            {
                case AnnotationAlignment.Before:
                    if (ang >= fourty5 && ang <= one35)
                    {
                        hAlign = HorizontalAlignment.Right;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Top;
                    }
                    else if (ang >= two25 && ang <= three15)
                    {
                        hAlign = HorizontalAlignment.Left;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Bottom;
                    }
                    else if (ang > fourty5 && ang < two25)
                    {
                        vAlign = VerticalAlignment.Top;
                        hAlign = obj.Offset == 0.5 ? HorizontalAlignment.Center : HorizontalAlignment.Right;
                    }
                    else
                    {
                        vAlign = VerticalAlignment.Bottom;
                        hAlign = (obj.Offset == 0.5) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
                    }
                    break;
                case AnnotationAlignment.After:
                    if (ang >= fourty5 && ang <= one35)
                    {
                        hAlign = HorizontalAlignment.Left;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Top;
                    }
                    else if (ang >= two25 && ang <= three15)
                    {
                        hAlign = HorizontalAlignment.Right;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Bottom;
                    }
                    else if (ang > fourty5 && ang < two25)
                    {
                        vAlign = VerticalAlignment.Bottom;
                        hAlign = obj.Offset == 0.5 ? HorizontalAlignment.Center : HorizontalAlignment.Right;
                    }
                    else
                    {
                        vAlign = VerticalAlignment.Top;
                        hAlign = obj.Offset == 0.5 ? HorizontalAlignment.Center : HorizontalAlignment.Left;
                    }
                    break;
            }
            if (obj.Offset == 0 || obj.Offset == 1)
            {
                Direction direction = GetBezierDirection(pts[0], pts[1]);
                switch (direction)
                {
                    case Direction.Left:
                        hAlign = obj.Offset == 0 ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                        break;
                    case Direction.Right:
                        hAlign = obj.Offset == 0 ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                        break;
                    case Direction.Bottom:
                        vAlign = obj.Offset == 0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
                        break;
                    case Direction.Top:
                        vAlign = obj.Offset == 0 ? VerticalAlignment.Bottom : VerticalAlignment.Top;
                        break;
                }
            }
            return new GetAlignment() { HorizontalAlign = hAlign, VerticalAlign = vAlign };
        }
        internal static GetAlignment AlignLabelOnUserHandleSegments(ConnectorFixedUserHandle obj, double ang, List<DiagramPoint> pts)
        {
            ang %= 360;
            double fourty5 = 45;
            double one35 = 135;
            double two25 = 225;
            double three15 = 315;
            VerticalAlignment vAlign = VerticalAlignment.Center;
            HorizontalAlignment hAlign = HorizontalAlignment.Center;
            switch (obj.Alignment)
            {
                case FixedUserHandleAlignment.Before:
                    if (ang >= fourty5 && ang <= one35)
                    {
                        hAlign = HorizontalAlignment.Right;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Top;
                    }
                    else if (ang >= two25 && ang <= three15)
                    {
                        hAlign = HorizontalAlignment.Left;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Bottom;
                    }
                    else if (ang > fourty5 && ang < two25)
                    {
                        vAlign = VerticalAlignment.Top;
                        hAlign = obj.Offset == 0.5 ? HorizontalAlignment.Center : HorizontalAlignment.Right;
                    }
                    else
                    {
                        vAlign = VerticalAlignment.Bottom;
                        hAlign = (obj.Offset == 0.5) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
                    }
                    break;
                case FixedUserHandleAlignment.After:
                    if (ang >= fourty5 && ang <= one35)
                    {
                        hAlign = HorizontalAlignment.Left;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Top;
                    }
                    else if (ang >= two25 && ang <= three15)
                    {
                        hAlign = HorizontalAlignment.Right;
                        vAlign = obj.Offset == 0.5 ? VerticalAlignment.Center : VerticalAlignment.Bottom;
                    }
                    else if (ang > fourty5 && ang < two25)
                    {
                        vAlign = VerticalAlignment.Bottom;
                        hAlign = obj.Offset == 0.5 ? HorizontalAlignment.Center : HorizontalAlignment.Right;
                    }
                    else
                    {
                        vAlign = VerticalAlignment.Top;
                        hAlign = obj.Offset == 0.5 ? HorizontalAlignment.Center : HorizontalAlignment.Left;
                    }
                    break;
                case FixedUserHandleAlignment.Center:
                    hAlign = HorizontalAlignment.Center;
                    vAlign = VerticalAlignment.Center;
                    break;
            }
            if (obj.Offset == 0 || obj.Offset == 1)
            {
                Direction direction = GetBezierDirection(pts[0], pts[1]);
                switch (direction)
                {
                    case Direction.Left:
                        hAlign = obj.Offset == 0 ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                        break;
                    case Direction.Right:
                        hAlign = obj.Offset == 0 ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                        break;
                    case Direction.Bottom:
                        vAlign = obj.Offset == 0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
                        break;
                    case Direction.Top:
                        vAlign = obj.Offset == 0 ? VerticalAlignment.Bottom : VerticalAlignment.Top;
                        break;
                }
            }
            return new GetAlignment() { HorizontalAlign = hAlign, VerticalAlign = vAlign };
        }
        private static Direction GetBezierDirection(DiagramPoint src, DiagramPoint tar)
        {
            if (Math.Abs(tar.X - src.X) > Math.Abs(tar.Y - src.Y))
            {
                return src.X < tar.X ? Direction.Right : Direction.Left;
            }
            else
            {
                return src.Y < tar.Y ? Direction.Bottom : Direction.Top;
            }
        }
        internal static bool CheckPort(IDiagramObject node, ICommonElement element)
        {
            if (node is Node node1 && element != null && element.ID.Split("_").Length > 1)
            {
                for (int i = 0; i < node1.Ports.Count; i++)
                {
                    if (node1.Ports[i].ID == element.ID.Split("_")[1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static DiagramPoint FindNearestPoint(DiagramPoint reference, DiagramPoint start, DiagramPoint end)
        {
            DiagramPoint shortestPoint;
            double shortest = DiagramPoint.FindLength(start, reference);
            double shortest1 = DiagramPoint.FindLength(end, reference);
            if (shortest > shortest1)
            {
                shortestPoint = end;
            }
            else
            {
                shortestPoint = start;
            }
            double angleBWStAndEnd = DiagramPoint.FindAngle(start, end);
            double angleBWStAndRef = DiagramPoint.FindAngle(shortestPoint, reference);
            double r = DiagramPoint.FindLength(shortestPoint, reference);
            double vaAngle = angleBWStAndRef + ((angleBWStAndEnd - angleBWStAndRef) * 2);
            return new DiagramPoint
            {
                X =
                (shortestPoint.X + r * Math.Cos(vaAngle * Math.PI / 180)),
                Y = (shortestPoint.Y + r * Math.Sin(vaAngle * Math.PI / 180))
            };
        }

        internal static List<DiagramPoint> PointsForBezier(Connector connector)
        {
            List<DiagramPoint> points = new List<DiagramPoint>();
            if (connector.Type == ConnectorSegmentType.Bezier)
            {
                for (int i = 0; i < connector.Segments.Count; i++)
                {
                    double tolerance = 1.5;
                    if (connector.Segments[i] is BezierSegment segment)
                    {
                        DiagramPoint point1 = !DiagramPoint.IsEmptyPoint(segment.Point1) ? segment.Point1 : segment.BezierPoint1;
                        DiagramPoint point2 = !DiagramPoint.IsEmptyPoint(segment.Point2) ? segment.Point2 : segment.BezierPoint2;
                        double max = (Connector.Distance(point1, segment.Points[0]) +
                                      Connector.Distance(point2, point1) +
                                      Connector.Distance(segment.Points[1], point2)) / tolerance;
                        for (int j = 0; j < max - 1; j += 10)
                        {
                            points.Add(
                                BezierPoints(
                                    segment.Points[0], !DiagramPoint.IsEmptyPoint(segment.Point1) ? segment.Point1 : segment.BezierPoint1,
                                    !DiagramPoint.IsEmptyPoint(segment.Point2) ? segment.Point2 : segment.BezierPoint2, segment.Points[1], j, max));

                        }
                    }
                }
            }
            return points;
        }

        ///<summary>
        /// Get the intermediate bezier curve for point over connector
        ///</summary>
        internal static DiagramPoint BezierPoints(
            DiagramPoint startPoint, DiagramPoint point1, DiagramPoint point2, DiagramPoint endPoint, int i, double max)
        {
            DiagramPoint pt = new DiagramPoint { X = 0, Y = 0 };
            double t = i / max;
            double x = (1 - t) * (1 - t) * (1 - t) * startPoint.X +
                3 * t * (1 - t) * (1 - t) * point1.X +
                3 * t * t * (1 - t) * point2.X +
                t * t * t * endPoint.X;
            pt.X = x;
            double y = (1 - t) * (1 - t) * (1 - t) * startPoint.Y +
                3 * t * (1 - t) * (1 - t) * point1.Y +
                3 * t * t * (1 - t) * point2.Y +
                t * t * t * endPoint.Y;
            pt.Y = y;
            return pt;
        }

        internal static bool IsPointOverConnector(Connector connector, DiagramPoint reference)
        {
            List<DiagramPoint> intermediatePoints = connector.Type == ConnectorSegmentType.Bezier ? PointsForBezier(connector) :
                connector.IntermediatePoints;
            for (int i = 0; i < intermediatePoints.Count - 1; i++)
            {
                DiagramPoint start = intermediatePoints[i];
                DiagramPoint end = intermediatePoints[i + 1];
                DiagramRect rect = DiagramRect.ToBounds(new List<DiagramPoint> { start, end });
                rect.Inflate(connector.HitPadding);
                if (rect.ContainsPoint(reference))
                {
                    DiagramPoint intersectingPoint = FindNearestPoint(reference, start, end);
                    Segment segment1 = new Segment { X1 = start.X, X2 = end.X, Y1 = start.Y, Y2 = end.Y };
                    Segment segment2 = new Segment { X1 = reference.X, X2 = intersectingPoint.X, Y1 = reference.Y, Y2 = intersectingPoint.Y };
                    Intersection intersectDetails = Intersect3(segment1, segment2);
                    if (intersectDetails.Enabled)
                    {
                        double distance = DiagramPoint.FindLength(reference, intersectDetails.IntersectPt);
                        if (Math.Abs(distance) < connector.HitPadding)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        DiagramRect rect2 = DiagramRect.ToBounds(new List<DiagramPoint> { reference, reference });
                        rect2.Inflate(3);
                        if (rect2.ContainsPoint(start) || rect2.ContainsPoint(end))
                        {
                            return true;
                        }
                    }
                    if (DiagramPoint.Equals(reference, intersectingPoint))
                    {
                        return true;
                    }
                }
            }

            if (connector.Annotations.Count > 0)
            {
                ObservableCollection<ICommonElement> container = connector.Wrapper.Children;
                for (int j = 3; j < container.Count; j++)
                {
                    ICommonElement textElement = container[j];
                    if (textElement.Bounds.ContainsPoint(reference))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static ObservableCollection<IDiagramObject> CompleteRegion(DiagramRect region, ObservableCollection<IDiagramObject> selectedObjects)
        {
            ObservableCollection<IDiagramObject> collection = new DiagramObjectCollection<IDiagramObject>();
            for (int i = 0; i < selectedObjects.Count; i++)
            {
                IDiagramObject obj = selectedObjects[i];
                if (region.ContainsRect(((NodeBase)obj).Wrapper.Bounds))
                {
                    collection.Add(obj);
                }
            }
            return collection;
        }

        internal static void InsertObjectByZIndex(IDiagramObject obj, ObservableCollection<IDiagramObject> collection)
        {
            if (collection.Count == 0)
            {
                collection.Add(obj);
            }
            else if (collection.Count == 1)
            {
                if (((NodeBase)collection[0]).ZIndex > ((NodeBase)obj).ZIndex)
                {
                    collection.Insert(0, obj);
                }
                else
                {
                    collection.Add(obj);
                }
            }
            else if (collection.Count > 1)
            {
                int low = 0;
                int high = collection.Count - 1;
                int mid = (int)Math.Floor((decimal)((low + high) / 2));
                while (mid != low)
                {
                    if (((NodeBase)collection[mid]).ZIndex < ((NodeBase)obj).ZIndex)
                    {
                        low = mid;
                        mid = (int)Math.Floor((decimal)((low + high) / 2));
                    }
                    else if (((NodeBase)collection[mid]).ZIndex > ((NodeBase)obj).ZIndex)
                    {
                        high = mid;
                        mid = (int)Math.Floor((decimal)((low + high) / 2));
                    }
                }
                if (((NodeBase)collection[high]).ZIndex < ((NodeBase)obj).ZIndex)
                {
                    collection.Add(obj);
                }
                else if (((NodeBase)collection[low]).ZIndex > ((NodeBase)obj).ZIndex)
                {
                    collection.Insert(low, obj);
                }
                else if (((NodeBase)collection[low]).ZIndex < ((NodeBase)obj).ZIndex && ((NodeBase)collection[high]).ZIndex > ((NodeBase)obj).ZIndex)
                {
                    collection.Insert(high, obj);
                }
            }
        }
        internal static DiagramPoint GetPoint(double x, double y, double w, double h, double angle, double offsetX, double offsetY, DiagramPoint cornerPoint)
        {
            DiagramPoint pivot = new DiagramPoint() { X = 0, Y = 0 };
            Matrix trans = Matrix.IdentityMatrix();
            Matrix.RotateMatrix(trans, angle, offsetX, offsetY);
            switch (cornerPoint.X)
            {
                case 0:
                    switch (cornerPoint.Y)
                    {
                        case 0:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x, Y = y });
                            break;
                        case 0.5:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x, Y = y + h / 2 });
                            break;
                        case 1:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x, Y = y + h });
                            break;
                    }
                    break;
                case 0.5:
                    switch (cornerPoint.Y)
                    {
                        case 0:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x + w / 2, Y = y });
                            break;
                        case 0.5:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x + w / 2, Y = y + h / 2 });
                            break;
                        case 1:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x + w / 2, Y = y + h });
                            break;
                    }
                    break;
                case 1:
                    switch (cornerPoint.Y)
                    {
                        case 0:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x + w, Y = y });
                            break;
                        case 0.5:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x + w, Y = y + h / 2 });
                            break;
                        case 1:
                            pivot = Matrix.TransformPointByMatrix(trans, new DiagramPoint() { X = x + w, Y = y + h });
                            break;
                    }
                    break;
            }
            return new DiagramPoint() { X = pivot.X, Y = pivot.Y };
        }
    }

    internal class GetLoop
    {
        internal DiagramPoint Point;
        internal double Angle;
        internal int Index;
    }
    internal class GetAlignment
    {
        internal HorizontalAlignment HorizontalAlign;
        internal VerticalAlignment VerticalAlign;
    }
}
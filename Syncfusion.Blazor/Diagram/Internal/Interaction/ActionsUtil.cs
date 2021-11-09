
using System.Globalization;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal static class ActionsUtil
    {
        private const string RESIZE = "Resize";
        private const string N_RESIZE = "n-resize";
        private const string S_RESIZE = "s-resize";
        private const string NE_RESIZE = "ne-resize";
        private const string NW_RESIZE = "nw-resize";
        private const string SE_RESIZE = "se-resize";
        private const string W_RESIZE = "w-resize";
        private const string E_RESIZE = "e-resize";

        internal static Actions FindToolToActivate(object obj, ICommonElement wrapper, DiagramPoint position, SfDiagramComponent diagram, IDiagramObject target = null, List<ITouches> touchStart = null, List<ITouches> touchMove = null)
        {
            if(touchMove != null && touchMove.Count > 1 && touchStart != null && touchStart.Count > 1)
            {
                return Actions.PinchZoom;
            }
            if (target != null)
            {
                if (target is PointPort port && !HasSelection(diagram))
                {
                    return FindPortToolToActivate(diagram, port);
                }
            }
            if ((ConstraintsUtil.CanDrawOnce(diagram) || ConstraintsUtil.CanContinuous(diagram)) && diagram.DrawingObject != null)
            {
                if (diagram.DrawingObject is Node || ((diagram.DrawingObject is Connector) && !diagram.DiagramAction.HasFlag(DiagramAction.DrawingTool)))
                {
                    return Actions.Draw;
                }

            }
            if (diagram.PaletteInstance != null)
            {
                if (diagram.PaletteInstance.PaletteMouseDown && diagram.PaletteInstance.SelectedSymbol != null && diagram.PaletteInstance.AllowDrag)
                {
                    return Actions.Drag;
                }
            }
            if (wrapper is TextElement textElement && textElement.Hyperlink.Url != null)
            {
                return Actions.Hyperlink;
            }
            if (HasSelection(diagram))
            {
                DiagramContainer element = diagram.SelectionSettings.Wrapper;
                DiagramRect paddedBounds = new DiagramRect() { X = element.Bounds.X, Y = element.Bounds.Y, Width = element.Bounds.Width, Height = element.Bounds.Height };
                DiagramSelectionSettings handle = diagram.SelectionSettings;
                Actions actions;
                if (HasSingleConnection(diagram))
                {
                    Connector connector = diagram.SelectionSettings.Connectors[0];
                    actions = ToolToActivateForSingleSelection(connector, diagram, position, handle);
                }
                else
                {
                    actions = ToolToActivateForMultiSelection(diagram, element, paddedBounds, position, handle);
                }
                if (actions != Actions.Select)
                    return actions;
            }

            //Panning
            if (ConstraintsUtil.CanZoomPan(diagram) && obj == null)
            {
                return Actions.Pan;
            }
            if (target is PointPort pointPort && (!ConstraintsUtil.CanZoomPan(diagram)))
            {
                Actions action = FindPortToolToActivate(diagram, pointPort);
                if (action != Actions.None)
                {
                    return action;
                }
            }
            if (obj != null)
            {
                if (obj is Node || obj is Connector)
                {
                    if (wrapper != null && !string.IsNullOrEmpty(wrapper.ID))
                    {
                        string userid;
                        if (obj is Node node)
                        {
                            for (int i = 0; i < node.FixedUserHandles.Count; i++)
                            {
                                userid = node.FixedUserHandles[i].ID;
                                if (!string.IsNullOrEmpty(wrapper.ID) && (wrapper.ID.IndexOf(userid, System.StringComparison.InvariantCulture) > -1))
                                {
                                    return Actions.FixedUserHandle;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ((Connector)obj).FixedUserHandles.Count; i++)
                            {
                                userid = ((Connector)obj).FixedUserHandles[i].ID;
                                if (!string.IsNullOrEmpty(wrapper.ID) && (wrapper.ID.IndexOf(userid, System.StringComparison.InvariantCulture) > -1))
                                {
                                    return Actions.FixedUserHandle;
                                }
                            }
                        }
                    }
                    if (ConstraintsUtil.CanMove((IDiagramObject)obj) && IsSelected(diagram, obj))
                    {
                        if ((obj is Connector connector && !(Contains(position, connector.SourcePoint, connector.HitPadding) ||
                                                             Contains(position, connector.TargetPoint, connector.HitPadding)))
                            || !(obj is Connector))
                        {
                            return Actions.Drag;
                        }
                    }
                    else if (ConstraintsUtil.CanZoomPan(diagram) && !ConstraintsUtil.DefaultTool(diagram))
                    {
                        return Actions.Pan;
                    }
                    else { return Actions.Select; }
                }
                else { return Actions.Select; }
            }

            return Actions.Select;
        }

        private static Actions ToolToActivateForMultiSelection(SfDiagramComponent diagram, DiagramContainer element, DiagramRect paddedBounds, DiagramPoint position, DiagramSelectionSettings handle)
        {
            double ten = 10 / diagram.Scroller.CurrentZoom;
            Matrix matrix = Matrix.IdentityMatrix();
            Matrix.RotateMatrix(matrix, element.RotationAngle + element.ParentTransform, element.OffsetX, element.OffsetY);
            double x = BaseUtil.GetDoubleValue(element.OffsetX - (element.Pivot.X * element.ActualSize.Width));
            double y = BaseUtil.GetDoubleValue(element.OffsetY - (element.Pivot.Y * element.ActualSize.Height));
            DiagramPoint rotateThumb = new DiagramPoint()
            {
                X = x + BaseUtil.GetDoubleValue((element.Pivot.X.Equals(0.5) ? element.Pivot.X * 2 : element.Pivot.X) * element.ActualSize.Width / 2),
                Y = y - 35 / diagram.Scroller.CurrentZoom
            };
            rotateThumb = Matrix.TransformPointByMatrix(matrix, rotateThumb);
            if (ConstraintsUtil.CanShowCorner(handle.Constraints, SelectorConstraints.Rotate) && Contains(position, rotateThumb, ten, 13) &&
                    (diagram.SelectionSettings.ThumbsConstraints.HasFlag(ThumbsConstraints.Rotate)))
            {
                return Actions.Rotate;
            }
            paddedBounds.Inflate(ten);
            if (paddedBounds.ContainsPoint(position))
            {
                Actions action = CheckResizeHandles(diagram, element, position, matrix, (double)x, (double)y);
                if (action != Actions.None)
                {
                    return action;
                }
            }
            return Actions.Select;
        }
        private static Actions ToolToActivateForSingleSelection(Connector connector, SfDiagramComponent diagram, DiagramPoint position, DiagramSelectionSettings handle)
        {
            double sourcePaddingValue = 10 / diagram.Scroller.CurrentZoom;
            double targetPaddingValue = 10 / diagram.Scroller.CurrentZoom;
            if (ConstraintsUtil.CanShowCorner(handle.Constraints, SelectorConstraints.ResizeAll))
            {
                Connector drawingObj = (Connector)diagram.DrawingObject;
                if (drawingObj != null && diagram.SelectionSettings.Connectors.Count > 0 && (diagram.EventHandler.IsMouseDown || drawingObj.Type == ConnectorSegmentType.Polyline))
                {
                    return Actions.ConnectorTargetEnd;
                }
                if (ConstraintsUtil.CanShowCorner(handle.Constraints, SelectorConstraints.ConnectorSourceThumb) &&
                    ConstraintsUtil.CanDragSourceEnd(connector) && Contains(position, connector.SourcePoint, sourcePaddingValue))
                {
                    return Actions.ConnectorSourceEnd;
                }

                if (ConstraintsUtil.CanShowCorner(handle.Constraints, SelectorConstraints.ConnectorTargetThumb) &&
                    ConstraintsUtil.CanDragTargetEnd(connector) && Contains(position, connector.TargetPoint, targetPaddingValue))
                {
                    return Actions.ConnectorTargetEnd;
                }

                Actions action = CheckForConnectorSegment(connector, position, diagram);
                if (action != Actions.OrthogonalThumb)
                {
                    if ((ConstraintsUtil.CanShowCorner(handle.Constraints, SelectorConstraints.ConnectorSourceThumb))
                        && ConstraintsUtil.CanDragSourceEnd(connector))
                    {
                        if (action != Actions.None) { return action; }
                    }
                    if ((ConstraintsUtil.CanShowCorner(handle.Constraints, SelectorConstraints.ConnectorTargetThumb))
                        && ConstraintsUtil.CanDragTargetEnd(connector))
                    {
                        if (action != Actions.None) { return action; }
                    }
                }
                else
                {
                    return action;
                }
            }
            return Actions.Select;
        }
        private static Actions FindPortToolToActivate(SfDiagramComponent diagram, PointPort target)
        {
            if (target.Constraints.HasFlag(PortConstraints.Draw))
            {
                diagram.UpdateTool(InteractionController.DrawOnce);
                Connector newConnector = new Connector() { SourceID = ((Node)target.Parent).ID, SourcePortID = target.ID, Type = ConnectorSegmentType.Orthogonal };
                diagram.UpdateDrawingObject(newConnector);
                return Actions.PortDraw;
            }
            return Actions.None;
        }
        private static Actions CheckResizeHandles(SfDiagramComponent diagram, DiagramElement element, DiagramPoint position, Matrix matrix, double x, double y)
        {
            Actions action = CheckForResizeHandles(diagram, element, position, matrix, x, y);
            if (action != Actions.None) { return action; }
            return Actions.None;
        }
        private static Actions CheckForResizeHandles(SfDiagramComponent diagram, DiagramElement element, DiagramPoint position, Matrix matrix, double x, double y)
        {
            double forty = 40 / diagram.Scroller.CurrentZoom;
            double ten = 10 / diagram.Scroller.CurrentZoom;
            DiagramSelectionSettings selectedItems = diagram.SelectionSettings;
            if (diagram.DrawingObject != null && (diagram.DrawingObject is Node) && diagram.EventHandler.IsMouseDown)
            {
                return Actions.Draw;
            }
            if (element.ActualSize.Width >= forty && element.ActualSize.Height >= forty)
            {
                if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeSouthEast, selectedItems.ThumbsConstraints) && Contains(
                    position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = (double)(x + element.ActualSize.Width), Y = (double)(y + element.ActualSize.Height) }), ten))
                {
                    return Actions.ResizeSouthEast;
                }
                if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeSouthWest, selectedItems.ThumbsConstraints) &&
                    Contains(position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = x, Y = (double)(y + element.ActualSize.Height) }), ten))
                {
                    return Actions.ResizeSouthWest;
                }
                if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeNorthEast, selectedItems.ThumbsConstraints) &&
                    Contains(position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = (double)(x + element.ActualSize.Width), Y = y }), ten))
                {
                    return Actions.ResizeNorthEast;
                }
                if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeNorthWest, selectedItems.ThumbsConstraints) &&
                    Contains(position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = x, Y = y }), ten))
                {
                    return Actions.ResizeNorthWest;
                }
            }
            if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeEast, selectedItems.ThumbsConstraints) && Contains(
                position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = (double)(x + element.ActualSize.Width), Y = (double)(y + element.ActualSize.Height / 2) }), ten))
            {
                return Actions.ResizeEast;
            }
            if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeWest, selectedItems.ThumbsConstraints) &&
                Contains(position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = x, Y = (double)(y + element.ActualSize.Height / 2) }), ten))
            {
                return Actions.ResizeWest;
            }
            if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeSouth, selectedItems.ThumbsConstraints) && Contains(
                position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = (double)(x + element.ActualSize.Width / 2), Y = (double)(y + element.ActualSize.Height) }), ten))
            {
                return Actions.ResizeSouth;
            }
            if (CanResizeCorner(selectedItems.Constraints, Actions.ResizeNorth, selectedItems.ThumbsConstraints) &&
                Contains(position, Matrix.TransformPointByMatrix(matrix, new DiagramPoint() { X = (double)(x + element.ActualSize.Width / 2), Y = y }), ten))
            {
                return Actions.ResizeNorth;
            }

            return Actions.None;
        }

        private static bool CanResizeCorner(SelectorConstraints selectorConstraints, Actions action, ThumbsConstraints thumbsConstraints)
        {
            if (selectorConstraints.HasFlag(GetSelectorConstraints(action)) && thumbsConstraints.HasFlag(GetThumbConstraints(action)))
            {
                return true;
            }
            return false;
        }
        private static SelectorConstraints GetSelectorConstraints(Actions action)
        {
            switch (action)
            {
                case Actions.ResizeEast:
                    return SelectorConstraints.ResizeEast;
                case Actions.ResizeNorth:
                    return SelectorConstraints.ResizeNorth;
                case Actions.ResizeNorthEast:
                    return SelectorConstraints.ResizeNorthEast;
                case Actions.ResizeNorthWest:
                    return SelectorConstraints.ResizeNorthWest;
                case Actions.ResizeSouth:
                    return SelectorConstraints.ResizeSouth;
                case Actions.ResizeSouthEast:
                    return SelectorConstraints.ResizeSouthEast;
                case Actions.ResizeSouthWest:
                    return SelectorConstraints.ResizeSouthWest;
                case Actions.ResizeWest:
                    return SelectorConstraints.ResizeWest;
                default:
                    return SelectorConstraints.None;
            }
        }
        private static ThumbsConstraints GetThumbConstraints(Actions action)
        {
            switch (action)
            {
                case Actions.ResizeEast:
                    return ThumbsConstraints.ResizeEast;
                case Actions.ResizeNorth:
                    return ThumbsConstraints.ResizeNorth;
                case Actions.ResizeNorthEast:
                    return ThumbsConstraints.ResizeNorthEast;
                case Actions.ResizeNorthWest:
                    return ThumbsConstraints.ResizeNorthWest;
                case Actions.ResizeSouth:
                    return ThumbsConstraints.ResizeSouth;
                case Actions.ResizeSouthEast:
                    return ThumbsConstraints.ResizeSouthEast;
                case Actions.ResizeSouthWest:
                    return ThumbsConstraints.ResizeSouthWest;
                case Actions.ResizeWest:
                    return ThumbsConstraints.ResizeWest;
                default:
                    return ThumbsConstraints.None;
            }
        }
        internal static bool Contains(DiagramPoint mousePosition, DiagramPoint corner, double padding, int rotateThumb = 0)
        {
            if (mousePosition.X + rotateThumb >= corner.X - padding && mousePosition.X <= corner.X + padding)
            {
                if (mousePosition.Y >= corner.Y - padding && mousePosition.Y <= corner.Y + padding)
                {
                    return true;
                }
            }
            return false;
        }
        internal static string FindUserHandle(DiagramPoint position, SfDiagramComponent diagram)
        {
            string name = null;
            if (HasSelection(diagram))
            {
                DiagramSelectionSettings handle = diagram.SelectionSettings;
                if (handle.Wrapper != null)
                {
                    foreach (UserHandle userHandle in handle.UserHandles)
                    {
                        if (userHandle.Visible)
                        {
                            DiagramPoint paddedBounds = DiagramUtil.GetUserHandlePosition(handle, userHandle, diagram.Scroller.Transform);
                            if (Contains(position, paddedBounds, userHandle.Size / (2 * diagram.Scroller.Transform.Scale)))
                            {
                                return userHandle.Name;
                            }
                        }
                    }
                }
            }
            return name;
        }

        internal static bool IsSelected(SfDiagramComponent diagram, object element)
        {
            if (element is DiagramSelectionSettings)
            {
                return true;
            }
            if (element is Node node)
            {
                if (diagram.SelectionSettings.Nodes.IndexOf(node) != -1)
                {
                    return true;
                }
            }
            else if (element is Connector connector)
            {
                if (diagram.SelectionSettings.Connectors.IndexOf(connector) != -1)
                {
                    return true;
                }
            }
            return false;
        }
        internal static string GetCursor(Actions actions, double angle)
        {
            //to avoid angles less than 0 & angles greater than 360
            angle += 360;
            angle %= 360;

            if (!actions.ToString().Contains(RESIZE, System.StringComparison.InvariantCulture))
            {
                return Dictionary.GetCursorValue(actions);
            }
            else
            {
                string dir = Dictionary.GetCursorValue(actions);
                if ((angle >= 0 && angle < 25) || (angle >= 160 && angle <= 205) || (angle >= 340 && angle <= 360))
                {
                    return dir;
                }
                else if ((angle >= 25 && angle <= 70) || (angle >= 205 && angle <= 250))
                {
                    if (dir == N_RESIZE || dir == S_RESIZE)
                    {
                        return NE_RESIZE;
                    }
                    else if (dir == NW_RESIZE || dir == SE_RESIZE)
                    {
                        return N_RESIZE;
                    }
                    else if (dir == E_RESIZE || dir == W_RESIZE)
                    {
                        return NW_RESIZE;
                    }
                    else
                    {
                        return E_RESIZE;
                    }
                }
                else if ((angle >= 70 && angle <= 115) || (angle >= 250 && angle <= 295))
                {
                    if (dir == N_RESIZE || dir == S_RESIZE)
                    {
                        return E_RESIZE;
                    }
                    else if (dir == NW_RESIZE || dir == SE_RESIZE)
                    {
                        return NE_RESIZE;
                    }
                    else if (dir == E_RESIZE || dir == W_RESIZE)
                    {
                        return N_RESIZE;
                    }
                    else
                    {
                        return NW_RESIZE;
                    }
                }
                else if ((angle >= 115 && angle <= 155) || (angle >= 295 && angle <= 340))
                {
                    if (dir == N_RESIZE || dir == S_RESIZE)
                    {
                        return NW_RESIZE;
                    }
                    else if (dir == NW_RESIZE || dir == SE_RESIZE)
                    {
                        return E_RESIZE;
                    }
                    else if (dir == E_RESIZE || dir == W_RESIZE)
                    {
                        return NE_RESIZE;
                    }
                }
                else
                {
                    return N_RESIZE;
                }
            }
            return Dictionary.GetCursorValue(actions);
        }

        internal static bool HasSelection(SfDiagramComponent diagram)
        {
            if (diagram.SelectionSettings.Nodes.Count > 0 || diagram.SelectionSettings.Connectors.Count > 0)
            {
                return true;
            }
            return false;
        }

        internal static bool HasSingleConnection(SfDiagramComponent diagram)
        {
            if (diagram.SelectionSettings.Connectors.Count == 1 && diagram.SelectionSettings.Nodes.Count == 0)
            {
                return true;
            }
            return false;
        }

        internal static Actions CheckForConnectorSegment(Connector connector, DiagramPoint position, SfDiagramComponent diagram)
        {
            double targetPaddingValue = 10 / diagram.ScrollSettings.CurrentZoom;
            double sourcePaddingValue = 10 / diagram.ScrollSettings.CurrentZoom;
            if (connector.Type == ConnectorSegmentType.Bezier)
            {
                for (int i = 0; i < connector.Segments.Count; i++)
                {
                    BezierSegment segment = (connector.SegmentCollection)[i] as BezierSegment;
                    if (segment != null && Contains(
                        position, !DiagramPoint.IsEmptyPoint(segment.Point1) ? segment.Point1 : segment.BezierPoint1,
                        sourcePaddingValue))
                    {
                        return Actions.BezierSourceThumb;
                    }
                    if (segment != null && Contains(
                        position, !DiagramPoint.IsEmptyPoint(segment.Point2) ? segment.Point2 : segment.BezierPoint2,
                        targetPaddingValue))
                    {
                        return Actions.BezierTargetThumb;
                    }
                }
            }
            if (ConstraintsUtil.CanDragSegmentThumb(connector))
            {
                if (connector.Type == ConnectorSegmentType.Straight || connector.Type == ConnectorSegmentType.Bezier)
                {
                    for (int i = 0; i < connector.Segments.Count; i++)
                    {
                        StraightSegment segment = (StraightSegment)(connector.Segments)[i];
                        if (segment != null && Contains(position, segment.Point, 10))
                        {
                            return Actions.SegmentEnd;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < connector.Segments.Count; i++)
                    {
                        var segPoint = new DiagramPoint(0, 0);
                        var segment = (connector.SegmentCollection)[i];
                        if (segment.AllowDrag)
                        {
                            for (int j = 0; j < segment.Points.Count - 1; j++)
                            {
                                double length = DiagramPoint.DistancePoints(segment.Points[j], segment.Points[j + 1]);
                                if (length >= 50)
                                {
                                    segPoint.X = ((segment.Points[j].X + segment.Points[j + 1].X) / 2);
                                    segPoint.Y = ((segment.Points[j].Y + segment.Points[j + 1].Y) / 2);
                                    if (Contains(position, segPoint, 30))
                                    {
                                        return Actions.OrthogonalThumb;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Actions.None;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class Snapping
    {
        const string SIDEALIGN = "sideAlign";
        const string CENTERALIGN = "centerAlign";
        const string LINECOLOR = "#07EDE1";
        private SfDiagramComponent diagram;
        internal List<LineAttributes> Lines = new List<LineAttributes>();
        internal Snapping(SfDiagramComponent sfDiagram)
        {
            diagram = sfDiagram;
        }
        internal static Snapping Initialize(SfDiagramComponent diagram)
        {
            Snapping snapping = new Snapping(diagram);
            return snapping;
        }
        private static DiagramRect GetBounds(DiagramSelectionSettings selector)
        {
            DiagramRect bounds = new DiagramRect(selector.OffsetX - selector.Width / 2, selector.OffsetY - selector.Height / 2, selector.Width, selector.Height); ;
            return bounds;
        }
        public bool CanSnap()
        {
            return this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.ShowHorizontalLines) || this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.ShowVerticalLines) || this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToHorizontalLines) || this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToVerticalLines);
        }
        internal DiagramPoint SnapPoint(DiagramSelectionSettings selectedObject, bool towardsLeft, bool towardsTop, DiagramPoint delta, DiagramPoint startPoint, DiagramPoint endPoint)
        {
            SnapSettings snapSettings = this.diagram.SnapSettings;
            double zoomFactor = this.diagram.ScrollSettings.CZoom;
            DiagramPoint offset = new DiagramPoint(0, 0);
            DiagramRect bounds = GetBounds(selectedObject);
            Snap horizontallySnapped = new Snap() { Snapped = false, Offset = 0 };
            Snap verticallySnapped = new Snap() { Snapped = false, Offset = 0 };
            if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToObject))
            {
                this.SnapObject(selectedObject, horizontallySnapped, verticallySnapped, delta, startPoint == endPoint);
            }
            //original position
            double left = bounds.X + delta.X;
            double top = bounds.Y + delta.Y;
            double right = bounds.X + bounds.Width + delta.X;
            double bottom = bounds.Y + bounds.Height + delta.Y;
            double[] scaledIntervals = (snapSettings.VerticalGridLines as GridLines).ScaledInterval;
            //snapped positions
            double roundedRight = Round(right, scaledIntervals, zoomFactor);
            double roundedLeft = Round(left, scaledIntervals, zoomFactor);

            scaledIntervals = (snapSettings.HorizontalGridLines as GridLines).ScaledInterval;
            double roundedTop = Round(top, scaledIntervals, zoomFactor);
            double roundedBottom = Round(bottom, scaledIntervals, zoomFactor);
            //current position
            double currentRight = bounds.X + bounds.Width;
            double currentBottom = bounds.Y + bounds.Height;
            if (!horizontallySnapped.Snapped)
            {
                if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToVerticalLines))
                {
                    if (Math.Abs(delta.X) >= 1)
                    {
                        if (towardsLeft)
                        {
                            if (Math.Abs(roundedRight - currentRight) > Math.Abs(roundedLeft - bounds.X))
                            {
                                offset.X += roundedLeft - bounds.X;
                            }
                            else
                            {
                                offset.X += roundedRight - currentRight;
                            }
                        }
                        else
                        {
                            if (Math.Abs(roundedRight - currentRight) < Math.Abs(roundedLeft - bounds.X))
                            {
                                offset.X += roundedRight - currentRight;
                            }
                            else
                            {
                                offset.X += roundedLeft - bounds.X;
                            }
                        }
                    }
                }
                else
                {
                    offset.X = endPoint.X - startPoint.X;
                }
            }
            else
            {
                if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToObject))
                {
                    offset.X = horizontallySnapped.Offset;
                }
                else
                {
                    offset.X = endPoint.X - startPoint.X;
                }
            }
            if (!verticallySnapped.Snapped)
            {
                if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToHorizontalLines))
                {
                    if (Math.Abs(delta.Y) >= 1)
                    {
                        if (towardsTop)
                        {
                            if (Math.Abs(roundedBottom - currentBottom) > Math.Abs(roundedTop - bounds.Y))
                            {
                                offset.Y += roundedTop - bounds.Y;
                            }
                            else
                            {
                                offset.Y += roundedBottom - currentBottom;
                            }
                        }
                        else
                        {
                            if (Math.Abs(roundedBottom - currentBottom) < Math.Abs(roundedTop - bounds.Y))
                            {
                                offset.Y += roundedBottom - currentBottom;
                            }
                            else
                            {
                                offset.Y += roundedTop - bounds.Y;
                            }
                        }
                    }
                }
                else
                {
                    offset.Y = endPoint.Y - startPoint.Y;
                }
            }
            else
            {
                offset.Y = verticallySnapped.Offset;
            }
            return offset;
        }

        internal static double Round(double value, double[] snapIntervals, double scale)
        {
            if (scale.Equals(1))
            {
                scale = Math.Pow(2, Math.Floor(Math.Log(scale) / Math.Log(2)));
            }
            double cutoff = 0;
            int i;
            for (i = 0; i < snapIntervals.Length; i++)
            {
                cutoff += snapIntervals[i];
            }
            cutoff /= scale;
            double quotient = Math.Floor(Math.Abs(value) / cutoff);
            double bal = value % cutoff;
            double prev = quotient * cutoff;
            if (!prev.Equals(value))
            {
                if (value >= 0)
                {
                    for (i = 0; i < snapIntervals.Length; i++)
                    {
                        if (bal <= snapIntervals[i] / scale)
                        {
                            return prev + (bal < (snapIntervals[i] / (2 * scale)) ? 0 : snapIntervals[i] / scale);
                        }
                        else
                        {
                            prev += snapIntervals[i] / scale;
                            bal -= snapIntervals[i] / scale;
                        }
                    }
                }
                else
                {
                    prev *= -1;
                    for (i = snapIntervals.Length - 1; i >= 0; i--)
                    {
                        if (Math.Abs(bal) <= snapIntervals[i] / scale)
                        {
                            return prev - (Math.Abs(bal) < (snapIntervals[i] / (2 * scale)) ? 0 : snapIntervals[i] / scale);
                        }
                        else
                        {
                            prev -= snapIntervals[i] / scale;
                            bal += snapIntervals[i] / scale;
                        }
                    }
                }
            }
            return value;
        }
        //Snap to Object
        private void SnapObject(DiagramSelectionSettings selectedObject, Snap horizontalSnap, Snap verticalSnap, DiagramPoint delta, bool ended)
        {
            double lengthX = 0.0; double lengthY = 0.0;
            SnapObject hTarget = null;
            SnapObject vTarget = null;
            Scroller scrollable = this.diagram.Scroller;
            SnapSettings snapSettings = this.diagram.SnapSettings;
            List<SnapObjects> objectsAtLeft = new List<SnapObjects>();
            List<SnapObjects> objectsAtRight = new List<SnapObjects>();
            List<SnapObjects> objectsAtTop = new List<SnapObjects>();
            List<SnapObjects> objectsAtBottom = new List<SnapObjects>();
            DiagramRect bounds = GetBounds(selectedObject);
            double scale = diagram.Scroller.CurrentZoom;
            double hOffset = -scrollable.HorizontalOffset; double vOffset = -scrollable.VerticalOffset;
            double snapObjDistance = snapSettings.SnapDistance / scale;
            DiagramRect viewPort = new DiagramRect(0, 0, scrollable.ViewPortWidth, scrollable.ViewPortHeight);
            DiagramRect hIntersectRect = new DiagramRect(
            hOffset / scale, (bounds.Y - snapObjDistance - 5), viewPort.Width / scale, (bounds.Height + 2 * snapObjDistance + 10));
            DiagramRect vIntersectRect = new DiagramRect(
            (bounds.X - snapObjDistance - 5), vOffset / scale, (bounds.Width + 2 * snapObjDistance + 10), viewPort.Height / scale);
            viewPort = new DiagramRect(
                hOffset / scale, vOffset / scale, viewPort.Width / scale, viewPort.Height / scale);
            List<DiagramElement> nodes = this.FindNodes(diagram.SpatialSearch, vIntersectRect, viewPort, null);
            int i; DiagramElement target;
            _ = diagram.NameTable;
            for (i = 0; i < nodes.Count; i++)
            {
                target = nodes[i];
                if (this.CanBeTarget(target))
                {
                    ToSnapHorizontally(delta, ref lengthX, ref hTarget, objectsAtTop, objectsAtBottom, bounds, snapObjDistance, target);
                }
            }
            nodes = this.FindNodes(diagram.SpatialSearch, hIntersectRect, viewPort, null);
            for (int j = 0; j < nodes.Count; j++)
            {
                target = nodes[j];
                if (this.CanBeTarget(target))
                {
                    ToSnapVertically(delta, ref lengthY, ref vTarget, objectsAtLeft, objectsAtRight, bounds, snapObjDistance, target);
                }
            }
            this.CreateGuidelines(hTarget, vTarget, horizontalSnap, verticalSnap, ended);
            if (!horizontalSnap.Snapped)
            {
                this.CreateHSpacingLines(selectedObject, objectsAtLeft, objectsAtRight, horizontalSnap, ended, delta, snapObjDistance);
            }
            if (!verticalSnap.Snapped)
            {
                this.CreateVSpacingLines(selectedObject, objectsAtTop, objectsAtBottom, verticalSnap, ended, delta, snapObjDistance);
            }
        }

        private DiagramRect ToSnapVertically(DiagramPoint delta, ref double lengthY, ref SnapObject vTarget, List<SnapObjects> objectsAtLeft, List<SnapObjects> objectsAtRight, DiagramRect bounds, double snapObjDistance, DiagramElement target)
        {
            DiagramRect targetBounds = null;
            if (!(this.diagram.NameTable[target.ID] is Connector) && this.CanConsider(target))
            {
                targetBounds = target.Bounds;
                if (targetBounds.X + targetBounds.Width < bounds.X + delta.X)
                {
                    objectsAtLeft.Add(new SnapObjects() { Element = target, Distance = Math.Abs((bounds.X + delta.X) - targetBounds.X - targetBounds.Width) });
                }
                if (targetBounds.X > bounds.X + delta.X + bounds.Width)
                {
                    objectsAtRight.Add(new SnapObjects() { Element = target, Distance = Math.Abs(bounds.X + delta.X + bounds.Width - targetBounds.X) });
                }
                if (lengthY == 0.0 || lengthY > Math.Abs(targetBounds.X - bounds.X - delta.X))
                {
                    if (Math.Abs(targetBounds.Y + targetBounds.Height / 2 - (bounds.Y + bounds.Height / 2 + delta.Y))
                        <= snapObjDistance)
                    {
                        vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.CenterY);
                        lengthY = Math.Abs(targetBounds.X - bounds.X);
                    }
                    else if (Math.Abs(targetBounds.Y - bounds.Y - delta.Y) <= snapObjDistance)
                    {
                        vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Top);
                        lengthY = Math.Abs(targetBounds.X - bounds.X);
                    }
                    else if (Math.Abs(targetBounds.Y + targetBounds.Height - (bounds.Y + bounds.Height + delta.Y)) <=
                      snapObjDistance)
                    {
                        vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Bottom);
                        lengthY = Math.Abs(targetBounds.X - bounds.X);
                    }
                    else if (Math.Abs(targetBounds.Y + targetBounds.Height - bounds.Y - delta.Y) <= snapObjDistance)
                    {
                        vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.TopBottom);
                        lengthY = Math.Abs(targetBounds.X - bounds.X);
                    }
                    else if (Math.Abs(targetBounds.Y - (bounds.Y + bounds.Height + delta.Y)) <= snapObjDistance)
                    {
                        vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.BottomTop);
                        lengthY = Math.Abs(targetBounds.X - bounds.X);
                    }
                }
            }
            return targetBounds;
        }

        private DiagramRect ToSnapHorizontally(DiagramPoint delta, ref double lengthX, ref SnapObject hTarget, List<SnapObjects> objectsAtTop, List<SnapObjects> objectsAtBottom, DiagramRect bounds, double snapObjDistance, DiagramElement target)
        {
            DiagramRect targetBounds = null;
            if (!(this.diagram.NameTable[target.ID] is Connector) && this.CanConsider(target))
            {
                targetBounds = target.Bounds;
                if (targetBounds.Height + targetBounds.Y < delta.Y + bounds.Y)
                {
                    objectsAtTop.Add(new SnapObjects() { Element = target, Distance = Math.Abs(bounds.Y + delta.Y - targetBounds.Y - targetBounds.Height) });
                }
                else if (targetBounds.Y > bounds.Y + delta.Y + bounds.Height)
                {
                    objectsAtBottom.Add(new SnapObjects() { Element = target, Distance = Math.Abs(bounds.Y + delta.Y + bounds.Height - targetBounds.Y) });
                }
                if (lengthX == 0.0 || lengthX > Math.Abs(targetBounds.Y - bounds.Y - delta.Y))
                {
                    if (Math.Abs(targetBounds.X + targetBounds.Width / 2 - (bounds.X + bounds.Width / 2 + delta.X)) <=
                        snapObjDistance)
                    {
                        hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.CenterX);
                        lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                    }
                    else if (Math.Abs(targetBounds.X + targetBounds.Width - (bounds.X + bounds.Width + delta.X)) <= snapObjDistance)
                    {
                        hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Right);
                        lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                    }
                    else if (Math.Abs(targetBounds.X - (bounds.X + delta.X)) <= snapObjDistance)
                    {
                        hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Left);
                        lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                    }
                    else if (Math.Abs(targetBounds.X - (bounds.X + bounds.Width + delta.X)) <= snapObjDistance)
                    {
                        hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.RightLeft);
                        lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                    }
                    else if (Math.Abs(targetBounds.X + targetBounds.Width - (bounds.X + delta.X)) <= snapObjDistance)
                    {
                        hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.LeftRight);
                        lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                    }
                }
            }
            return targetBounds;
        }

        internal DiagramPoint SnapConnectorEnd(DiagramPoint point)
        {
            SnapSettings snapSettings = this.diagram.SnapSettings;
            double zoomFactor = this.diagram.Scroller.CurrentZoom;
            if (snapSettings.Constraints.HasFlag(SnapConstraints.SnapToHorizontalLines) || snapSettings.Constraints.HasFlag(SnapConstraints.SnapToVerticalLines))
            {
                point.X = Round(point.X, (snapSettings.VerticalGridLines as GridLines).ScaledInterval, zoomFactor);
                point.Y = Round(point.Y, (snapSettings.HorizontalGridLines as GridLines).ScaledInterval, zoomFactor);

            }
            return point;
        }
        private bool CanBeTarget(DiagramElement node)
        {
            IDiagramObject element = this.diagram.NameTable[node.ID];
            return !(Internal.ActionsUtil.IsSelected(this.diagram, element));
        }
        private void SnapSize(Snap horizontalSnap, Snap verticalSnap, double deltaX, double deltaY, DiagramSelectionSettings selectedObject, bool ended)
        {
            double lengthX = 0.0; double lengthY = 0.0;
            SnapSettings snapSettings = this.diagram.SnapSettings;
            Scroller scrolled = this.diagram.Scroller;
            SnapObject hTarget = null; SnapObject vTarget = null;
            DiagramRect bounds = GetBounds(selectedObject);
            List<SnapSize> sameWidth = new List<SnapSize>(); List<SnapSize> sameHeight = new List<SnapSize>(); double scale = diagram.Scroller.CurrentZoom;
            double hOffset = -scrolled.HorizontalOffset; double vOffset = -scrolled.VerticalOffset;
            double snapObjDistance = snapSettings.SnapDistance / scale;
            DiagramRect viewPort = new DiagramRect(0, 0, scrolled.ViewPortWidth, scrolled.ViewPortHeight);
            DiagramRect hIntersectedRect = new DiagramRect(
                hOffset / scale, (bounds.Y - 5) / scale, viewPort.Width / scale, (bounds.Height + 10) / scale);
            DiagramRect vIntersectedRect = new DiagramRect(
                (bounds.X - 5) / scale, vOffset / scale, (bounds.Width + 10) / scale, viewPort.Height / scale);
            viewPort = new DiagramRect(
                hOffset / scale, vOffset / scale, viewPort.Width / scale, viewPort.Height / scale);
            List<DiagramElement> nodesInView = new List<DiagramElement>();
            List<DiagramElement> nodes = this.FindNodes(diagram.SpatialSearch, vIntersectedRect, viewPort, nodesInView);
            int i; DiagramElement target; DiagramRect targetBounds;
            for (i = 0; i < nodes.Count; i++)
            {
                target = nodes[i];
                if (this.CanConsider(target) && !(this.diagram.NameTable[target.ID] is Connector))
                {
                    targetBounds = target.Bounds;
                    if (lengthX == 0.0 || lengthX > Math.Abs(targetBounds.Y - bounds.Y))
                    {
                        if (horizontalSnap.Left)
                        {
                            if (Math.Abs(bounds.X + deltaX - targetBounds.X) <= snapObjDistance)
                            {
                                hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Left);
                                lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                            }
                            else if (Math.Abs(bounds.X + deltaX - targetBounds.X - targetBounds.Width) <= snapObjDistance)
                            {
                                hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.LeftRight);
                                lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                            }
                        }
                        else if (horizontalSnap.Right)
                        {
                            if (Math.Abs(bounds.X + deltaX + bounds.Width - targetBounds.X - targetBounds.Width) <= snapObjDistance)
                            {
                                hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Right);
                                lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                            }
                            else if (Math.Abs(bounds.X + deltaX + bounds.Width - targetBounds.X) <= snapObjDistance)
                            {
                                hTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.RightLeft);
                                lengthX = Math.Abs(targetBounds.Y - bounds.Y);
                            }
                        }
                    }
                }
            }
            nodes = this.FindNodes(diagram.SpatialSearch, hIntersectedRect, viewPort, null);
            for (i = 0; i < nodes.Count; i++)
            {
                target = nodes[i];
                if (this.CanConsider(target) && !(this.diagram.NameTable[target.ID] is Connector))
                {
                    targetBounds = target.Bounds;
                    if (lengthY == 0.0 || lengthY > Math.Abs(targetBounds.X - bounds.X))
                    {
                        if (verticalSnap.Top)
                        {
                            if (Math.Abs(bounds.Y + deltaY - targetBounds.Y) <= snapObjDistance)
                            {
                                vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Top);
                                lengthY = Math.Abs(targetBounds.X - bounds.X);
                            }
                            else if (Math.Abs(bounds.Y + deltaY - targetBounds.Y - targetBounds.Height) <= snapObjDistance)
                            {
                                vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.TopBottom);
                                lengthY = Math.Abs(targetBounds.X - bounds.X);
                            }
                        }
                        else if (verticalSnap.Bottom)
                        {
                            if (Math.Abs(bounds.Y + bounds.Height + deltaY - targetBounds.Y - targetBounds.Height) <= snapObjDistance)
                            {
                                vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.Bottom);
                                lengthY = Math.Abs(targetBounds.X - bounds.X);
                            }
                            else if (Math.Abs(bounds.Y + bounds.Height + deltaY - targetBounds.Y) <= snapObjDistance)
                            {
                                vTarget = CreateSnapObject(targetBounds, bounds, SnapAlignment.BottomTop);
                                lengthY = Math.Abs(targetBounds.X - bounds.X);
                            }
                        }
                    }
                }
            }
            for (i = 0; i < nodesInView.Count; i++)
            {
                target = nodesInView[i];
                if (this.CanConsider(target))
                {
                    targetBounds = target.Bounds;
                    double delta = horizontalSnap.Left ? -deltaX : deltaX;
                    double difference = Math.Abs(bounds.Width + delta - targetBounds.Width);
                    double actualDiff;
                    if (difference <= snapObjDistance)
                    {
                        actualDiff = horizontalSnap.Left ? -targetBounds.Width + bounds.Width : targetBounds.Width - bounds.Width;
                        sameWidth.Add(new SnapSize() { Source = target, Difference = difference, Offset = actualDiff });
                    }
                    delta = verticalSnap.Top ? -deltaY : deltaY;
                    double differenceY = Math.Abs(bounds.Height + delta - targetBounds.Height);
                    if (differenceY <= snapObjDistance)
                    {
                        actualDiff = verticalSnap.Top ? -targetBounds.Height + bounds.Height : targetBounds.Height - bounds.Height;
                        sameHeight.Add(new SnapSize() { Source = target, Difference = differenceY, Offset = actualDiff });
                    }
                }
            }
            //if (!diagram.getTool)
            {
                this.CreateGuidelines(hTarget, vTarget, horizontalSnap, verticalSnap, ended);
            }
            if (!horizontalSnap.Snapped && sameWidth.Count > 0 && (horizontalSnap.Left || horizontalSnap.Right))
            {
                this.AddSameWidthLines(sameWidth, horizontalSnap, ended, selectedObject);
            }
            if (!verticalSnap.Snapped && sameHeight.Count > 0 && (verticalSnap.Top || verticalSnap.Bottom))
            {
                this.AddSameHeightLines(sameHeight, verticalSnap, ended, selectedObject);
            }
        }
        internal double SnapTop(Snap horizontalSnap, Snap verticalSnap, double deltaX, double deltaY, DiagramSelectionSettings shape, bool ended, DiagramRect initialBoundsT)
        {
            double differenceY = deltaY;
            verticalSnap.Top = true;
            double y = 0;
            horizontalSnap.Left = horizontalSnap.Right = false;
            double zoomFactor = this.diagram.Scroller.CurrentZoom;
            if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToObject) && shape.RotationAngle == 0)
            {
                y = initialBoundsT.Y - initialBoundsT.Height * shape.Pivot.Y + deltaY - (shape.OffsetY - shape.Height * shape.Pivot.Y);
                this.SnapSize(horizontalSnap, verticalSnap, deltaX, y, shape, ended);
            }
            if (!verticalSnap.Snapped)
            {
                if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToHorizontalLines))
                {
                    double top = initialBoundsT.Y - initialBoundsT.Height * shape.Pivot.Y;
                    double actualTop = top + deltaY;
                    double roundedTop = Round(
                actualTop, (this.diagram.SnapSettings.HorizontalGridLines as GridLines).ScaledInterval, zoomFactor);
                    differenceY = roundedTop - top;
                }
            }
            else
            {
                differenceY = (deltaY - y) + verticalSnap.Offset;
            }
            return differenceY;
        }
        internal double SnapRight(Snap horizontalSnap, Snap verticalSnap, double deltaX, double deltaY, DiagramSelectionSettings shape, bool ended, DiagramRect initialBound)
        {
            double differenceX = deltaX;
            double x = 0;
            horizontalSnap.Right = true;
            verticalSnap.Top = verticalSnap.Bottom = false;
            double zoomFactor = this.diagram.Scroller.CurrentZoom;
            if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToObject) && shape.RotationAngle == 0)
            {
                x = initialBound.X + initialBound.Width * (1 - shape.Pivot.X) + deltaX - (shape.OffsetX + shape.Width * (1 - shape.Pivot.X));
                this.SnapSize(horizontalSnap, verticalSnap, x, deltaY, shape, ended);
            }
            if (!horizontalSnap.Snapped)
            {
                if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToVerticalLines))
                {
                    double right = initialBound.X + initialBound.Width * (1 - shape.Pivot.X);
                    double actualRight = right + deltaX;
                    double roundedRight = Round(
                        actualRight, (this.diagram.SnapSettings.VerticalGridLines as GridLines).ScaledInterval, zoomFactor);
                    differenceX = roundedRight - right;
                }
            }
            else
            {
                differenceX = (deltaX - x) + horizontalSnap.Offset;
            }
            return differenceX;
        }
        internal double SnapLeft(Snap horizontalSnap, Snap verticalSnap, double deltaX, double deltaY, DiagramSelectionSettings shape, bool ended, DiagramRect initialBoundsB)
        {
            double differenceX = deltaX;
            double x = 0;
            horizontalSnap.Left = true;
            verticalSnap.Top = verticalSnap.Bottom = false;
            double zoomFactor = this.diagram.Scroller.CurrentZoom;
            if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToObject) && shape.RotationAngle == 0)
            {
                x = initialBoundsB.X - initialBoundsB.Width * shape.Pivot.X + deltaX - (shape.OffsetX - shape.Width * shape.Pivot.X);
                this.SnapSize(horizontalSnap, verticalSnap, x, deltaY, shape, ended);
            }
            if (!horizontalSnap.Snapped)
            {
                if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToVerticalLines))
                {
                    double left = initialBoundsB.X - initialBoundsB.Width * shape.Pivot.X;
                    double actualLeft = left + deltaX;
                    double roundedLeft = Round(
                        actualLeft, (this.diagram.SnapSettings.HorizontalGridLines as GridLines).ScaledInterval, zoomFactor);
                    differenceX = roundedLeft - left;
                }
            }
            else
            {
                differenceX = (deltaX - x) + horizontalSnap.Offset;
            }
            return differenceX;
        }

        internal double SnapBottom(Snap horizontalSnap, Snap verticalSnap, double deltaX, double deltaY, DiagramSelectionSettings shape, bool ended, DiagramRect initialRect)
        {
            double differenceY = deltaY;
            verticalSnap.Bottom = true;
            horizontalSnap.Left = horizontalSnap.Right = false;
            double zoomFactor = this.diagram.Scroller.CurrentZoom;
            double y = 0;
            if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToObject) && shape.RotationAngle == 0)
            {
                y = initialRect.Y + initialRect.Height * (1 - shape.Pivot.Y) + deltaY - (shape.OffsetY + shape.Height * (1 - shape.Pivot.Y));
                this.SnapSize(horizontalSnap, verticalSnap, deltaX, y, shape, ended);
            }

            _ = GetBounds(shape);
            if (!verticalSnap.Snapped)
            {
                if (this.diagram.SnapSettings.Constraints.HasFlag(SnapConstraints.SnapToHorizontalLines))
                {
                    double bottom = initialRect.Y + initialRect.Height * (1 - shape.Pivot.Y);
                    double actualBottom = bottom + deltaY;
                    double roundedBottom = Round(
                actualBottom, (this.diagram.SnapSettings.HorizontalGridLines as GridLines).ScaledInterval, zoomFactor);
                    differenceY = roundedBottom - bottom;
                }
            }
            else
            {
                differenceY = (deltaY - y) + verticalSnap.Offset;
            }
            return differenceY;
        }
        //To create the same width and same size lines
        private void CreateGuidelines(SnapObject hTarget, SnapObject vTarget, Snap horizontalSnap, Snap verticalSnap, bool ended)
        {
            this.Lines.Clear();
            if (hTarget != null)
            {
                horizontalSnap.Offset = hTarget.OffsetX;
                horizontalSnap.Snapped = true;
                if (!ended)
                {
                    if (hTarget.Type == SIDEALIGN)
                    {
                        this.CreateAlignmentLines(hTarget.Start, hTarget.End, diagram.Scroller.Transform);
                    }
                    else
                    {
                        this.CreateAlignmentLines(hTarget.Start, hTarget.End, diagram.Scroller.Transform);
                    }
                }
            }
            if (vTarget != null)
            {
                verticalSnap.Offset = vTarget.OffsetY;
                verticalSnap.Snapped = true;
                if (!ended)
                {
                    if (vTarget.Type == SIDEALIGN)
                    {
                        this.CreateAlignmentLines(vTarget.Start, vTarget.End, diagram.Scroller.Transform);
                    }
                    else
                    {
                        this.CreateAlignmentLines(vTarget.Start, vTarget.End, diagram.Scroller.Transform);
                    }
                }
            }
        }
        //To create the alignment lines
        private void CreateAlignmentLines(DiagramPoint start, DiagramPoint end, TransformFactor transform)
        {
            start = new DiagramPoint((start.X + transform.TX) * transform.Scale, (start.Y + transform.TY) * transform.Scale);
            end = new DiagramPoint((end.X + transform.TX) * transform.Scale, (end.Y + transform.TY) * transform.Scale);


            LineAttributes line1 = new LineAttributes()
            {
                Stroke = LINECOLOR,
                StrokeWidth = 1,
                StartPoint = new DiagramPoint() { X = start.X, Y = start.Y },
                EndPoint = new DiagramPoint() { X = end.X, Y = end.Y },
                Fill = LINECOLOR,
                DashArray = string.Empty,
                Width = 1,
                X = 0,
                Y = 0,
                Height = 0,
                Angle = 0,
                PivotX = 0,
                PivotY = 0,
                Visible = true,
                Opacity = 1,
                ID = new Random().ToString()
            };
            this.Lines.Add(line1);
        }
        //To create Horizontal spacing lines
        private void CreateHSpacingLines(DiagramSelectionSettings shape, List<SnapObjects> objectsAtLeft, List<SnapObjects> objectsAtRight, Snap horizontalSnap, bool ended, DiagramPoint delta, double snapObjDistance)
        {
            double top = 0;
            SortByDistance(objectsAtLeft, true);
            SortByDistance(objectsAtRight, true);
            List<SnapObjects> equallySpaced = new List<SnapObjects>();

            DiagramRect bounds = GetBounds(shape);
            DiagramRect nearestLeft = null; DiagramRect nearestRight = null;
            DiagramRect targetBounds = null;
            double equalDistance = 0;
            if (objectsAtLeft.Count > 0)
            {
                equallySpaced.Add(objectsAtLeft[0]);
                nearestLeft = ((objectsAtLeft[0].Element).Bounds);
                top = nearestLeft.Y;
                if (objectsAtLeft.Count > 1)
                {
                    targetBounds = ((objectsAtLeft[1].Element).Bounds);
                    equalDistance = nearestLeft.X - targetBounds.X - targetBounds.Width;
                    if (Math.Abs(equalDistance - objectsAtLeft[0].Distance) <= snapObjDistance)
                    {
                        top = FindEquallySpacedNodesAtLeft(objectsAtLeft, equalDistance, top, equallySpaced);

                    }
                    else
                    {
                        equalDistance = objectsAtLeft[0].Distance;
                    }
                }
                else
                {
                    equalDistance = objectsAtLeft[0].Distance;
                }
            }
            SortByDistance(equallySpaced);
            equallySpaced.Add(new SnapObjects() { Selector = shape, Distance = 0 });
            top = bounds.Y < top || top == 0 ? bounds.Y : top;
            if (objectsAtRight.Count > 0)
            {
                double dist = 0;
                nearestRight = ((objectsAtRight[0].Element).Bounds);
                top = nearestRight.Y < top ? nearestRight.Y : top;
                if (objectsAtRight.Count > 1)
                {
                    targetBounds = ((objectsAtRight[1].Element).Bounds);
                    dist = targetBounds.X - nearestRight.X - nearestRight.Width;
                }
                if (objectsAtLeft.Count > 0)
                {
                    if (Math.Abs(objectsAtRight[0].Distance - objectsAtLeft[0].Distance) <= snapObjDistance)
                    {
                        double adjustableValue = Math.Abs(objectsAtRight[0].Distance - objectsAtLeft[0].Distance) / 2;
                        if (objectsAtRight[0].Distance < objectsAtLeft[0].Distance)
                        {
                            equalDistance -= adjustableValue;
                        }
                        else
                        {
                            equalDistance += adjustableValue;
                        }
                        equallySpaced.Add(objectsAtRight[0]);
                    }
                    else if (objectsAtLeft.Count == 1)
                    {
                        nearestLeft = null;
                        equallySpaced.RemoveAt(0);
                        equallySpaced.Add(objectsAtRight[0]);
                        equalDistance = dist;
                    }
                }
                else
                {
                    equalDistance = dist;
                    equallySpaced.Add(objectsAtRight[0]);
                }
                if (targetBounds != null && objectsAtRight.Count > 1 && nearestRight.X + nearestRight.Width < targetBounds.X)
                {
                    top = FindEquallySpacedNodesAtRight(objectsAtRight, dist, top, equallySpaced, snapObjDistance);
                }
            }
            if (equallySpaced.Count > 2)
            {
                this.AddHSpacingLines(equallySpaced, ended, top);
                double deltaHorizontal = 0;
                if (ended)
                {
                    deltaHorizontal = delta.X;
                }
                if (nearestLeft != null)
                {
                    horizontalSnap.Offset = equalDistance - Math.Abs(bounds.X + deltaHorizontal - nearestLeft.X - nearestLeft.Width) + deltaHorizontal;
                }
                else if (nearestRight != null)
                {
                    horizontalSnap.Offset = Math.Abs(bounds.X + bounds.Width + deltaHorizontal - nearestRight.X) - equalDistance + deltaHorizontal;
                }
                horizontalSnap.Snapped = true;
            }
        }
        private void CreateVSpacingLines(DiagramSelectionSettings shape, List<SnapObjects> objectsAtTop, List<SnapObjects> objectsAtBottom, Snap verticalSnap, bool ended, DiagramPoint delta, double snapObjDistance)
        {
            double right = 0;
            SortByDistance(objectsAtTop, true);
            SortByDistance(objectsAtBottom, true);
            List<SnapObjects> equallySpaced = new List<SnapObjects>();
            DiagramRect bounds = GetBounds(shape);
            DiagramRect nearestTop = null;
            DiagramRect nearestBottom = null;
            DiagramRect targetBounds = null;
            double equalDistance = 0;
            if (objectsAtTop.Count > 0)
            {
                equallySpaced.Add(objectsAtTop[0]);
                nearestTop = ((objectsAtTop[0].Element).Bounds);
                right = nearestTop.X + nearestTop.Width;
                if (objectsAtTop.Count > 1)
                {
                    targetBounds = ((objectsAtTop[1].Element).Bounds);
                    equalDistance = nearestTop.Y - targetBounds.Y - targetBounds.Height;
                    if (Math.Abs(equalDistance - objectsAtTop[0].Distance) <= snapObjDistance)
                    {
                        right = FindEquallySpacedNodesAtTop(objectsAtTop, equalDistance, right, equallySpaced);
                    }
                    else
                    {
                        equalDistance = objectsAtTop[0].Distance;
                    }
                }
                else
                {
                    equalDistance = objectsAtTop[0].Distance;
                }
            }
            SortByDistance(equallySpaced);
            equallySpaced.Add(new SnapObjects() { Selector = shape as DiagramSelectionSettings, Distance = 0 });
            right = bounds.X + bounds.Width > right || right == 0 ? bounds.X + bounds.Width : right;
            double dist = 0;
            if (objectsAtBottom.Count > 0)
            {
                nearestBottom = ((objectsAtBottom[0].Element).Bounds);
                right = nearestBottom.X + nearestBottom.Width > right ? nearestBottom.X + nearestBottom.Width : right;
                if (objectsAtBottom.Count > 1)
                {
                    targetBounds = ((objectsAtBottom[1].Element).Bounds);
                    dist = targetBounds.Y - nearestBottom.Y - nearestBottom.Height;
                }

                if (objectsAtTop.Count > 0)
                {
                    if (Math.Abs(objectsAtBottom[0].Distance - objectsAtTop[0].Distance) <= snapObjDistance)
                    {
                        double adjustableValue = Math.Abs(objectsAtBottom[0].Distance - objectsAtTop[0].Distance) / 2;
                        if (objectsAtBottom[0].Distance < objectsAtTop[0].Distance)
                        {
                            equalDistance -= adjustableValue;
                        }
                        else
                        {
                            equalDistance += adjustableValue;
                        }
                        equallySpaced.Add(objectsAtBottom[0]);
                    }
                    else if (objectsAtTop.Count == 1)
                    {
                        nearestTop = null;
                        equallySpaced.RemoveAt(0);
                        equallySpaced.Add(objectsAtBottom[0]);
                        equalDistance = dist;
                    }
                }
                else
                {
                    equalDistance = dist;
                    equallySpaced.Add(objectsAtBottom[0]);
                }
                if (targetBounds != null && objectsAtBottom.Count > 1 && targetBounds.Y > nearestBottom.Y + nearestBottom.Height)
                {
                    right = FindEquallySpacedNodesAtBottom(objectsAtBottom, dist, right, equallySpaced, snapObjDistance);
                }

            }
            if (equallySpaced.Count > 2)
            {
                this.AddVSpacingLines(equallySpaced, ended, right);
                double deltaVertical = 0;
                if (ended)
                {
                    deltaVertical = delta.Y;
                }
                if (nearestTop != null)
                {
                    verticalSnap.Offset = equalDistance - Math.Abs(bounds.Y + deltaVertical - nearestTop.Y - nearestTop.Height) + deltaVertical;
                }
                else if (nearestBottom != null)
                {
                    verticalSnap.Offset = Math.Abs(bounds.Y + bounds.Height + deltaVertical - nearestBottom.Y) - equalDistance + deltaVertical;
                }
                verticalSnap.Snapped = true;
            }
        }
        //Add the Horizontal spacing lines
        private void AddHSpacingLines(List<SnapObjects> equallySpaced, bool ended, double top)
        {
            if (!ended)
            {
                int i;
                for (i = 0; i < equallySpaced.Count - 1; i++)
                {
                    DiagramRect cBounds = null;
                    DiagramRect nBounds = null;
                    if (equallySpaced[i].Selector != null)
                    {
                        DiagramSelectionSettings shape = equallySpaced[i].Selector;
                        cBounds = GetBounds(shape);
                    }
                    if (equallySpaced[i + 1].Selector != null)
                    {
                        DiagramSelectionSettings shape = equallySpaced[i + 1].Selector;
                        nBounds = GetBounds(shape);
                    }
                    DiagramRect current = equallySpaced[i].Selector != null ? cBounds : ((equallySpaced[i].Element).Bounds);
                    DiagramRect next = equallySpaced[i + 1].Selector != null ? nBounds : ((equallySpaced[i + 1].Element).Bounds);
                    if (current != null)
                    {
                        DiagramPoint start = new DiagramPoint() { X = current.X + current.Width, Y = top - 15 };
                        if (next != null)
                        {
                            DiagramPoint end = new DiagramPoint() { X = next.X, Y = top - 15 };
                            this.CreateSpacingLines(start, end, diagram.Scroller.Transform);
                        }
                    }
                }
            }
        }
        //Add the vertical spacing lines
        private void AddVSpacingLines(List<SnapObjects> equallySpacedObjects, bool ended, double right)
        {
            if (!ended)
            {
                for (int i = 0; i < equallySpacedObjects.Count - 1; i++)
                {
                    DiagramRect cBounds = null;
                    DiagramRect nBounds = null;
                    if (equallySpacedObjects[i].Selector != null)
                    {
                        DiagramSelectionSettings shape = equallySpacedObjects[i].Selector;
                        cBounds = GetBounds(shape);
                    }
                    if (equallySpacedObjects[i + 1].Selector != null)
                    {
                        DiagramSelectionSettings shape = equallySpacedObjects[i + 1].Selector;
                        nBounds = GetBounds(shape);
                    }

                    DiagramRect current = equallySpacedObjects[i].Selector != null ? cBounds : ((equallySpacedObjects[i].Element).Bounds);
                    DiagramRect next = equallySpacedObjects[i + 1].Selector != null ? nBounds : ((equallySpacedObjects[i + 1].Element).Bounds);

                    if (current != null)
                    {
                        DiagramPoint start = new DiagramPoint() { X = right + 15, Y = current.Y + current.Height };
                        if (next != null)
                        {
                            DiagramPoint end = new DiagramPoint() { X = right + 15, Y = next.Y };
                            this.CreateSpacingLines(start, end, diagram.Scroller.Transform);
                        }
                    }
                }
            }
        }
        //To add same width lines
        private void AddSameWidthLines(List<SnapSize> sameWidths, Snap horizontalSnap, bool ended, DiagramSelectionSettings shape)
        {
            SortByDistance(sameWidths);
            DiagramRect bounds = GetBounds(shape);
            SnapSize target = sameWidths[0];
            DiagramRect targetBounds = target.Source.Bounds;
            List<SnapSize> sameSizes = new List<SnapSize> { sameWidths[0] };
            int i;
            for (i = 1; i < sameWidths.Count; i++)
            {
                DiagramRect currentBounds = ((sameWidths[i].Source) as DiagramElement).Bounds;
                if (currentBounds.Width.Equals(targetBounds.Width))
                {
                    sameSizes.Add(sameWidths[i]);
                }
            }
            if (!ended)
            {
                DiagramPoint startPt = new DiagramPoint() { X = bounds.X + target.Offset, Y = bounds.Y - 15 };
                DiagramPoint endPt = new DiagramPoint() { X = bounds.X + bounds.Width + target.Offset, Y = bounds.Y - 15 };
                this.CreateSpacingLines(startPt, endPt, diagram.Scroller.Transform);
                for (i = 0; i < sameSizes.Count; i++)
                {
                    bounds = ((sameSizes[i].Source) as DiagramElement).Bounds;
                    startPt = new DiagramPoint() { X = bounds.X, Y = bounds.Y - 15 };
                    endPt = new DiagramPoint() { X = bounds.X + bounds.Width, Y = bounds.Y - 15 };
                    this.CreateSpacingLines(startPt, endPt, diagram.Scroller.Transform);
                }
            }
            horizontalSnap.Offset = target.Offset;
            horizontalSnap.Snapped = true;

        }
        //To add same height lines
        private void AddSameHeightLines(List<SnapSize> sameHeights, Snap verticalSnap, bool ended, DiagramSelectionSettings shape)
        {
            SortByDistance(sameHeights);
            DiagramRect bounds = GetBounds(shape);
            SnapSize target = sameHeights[0];
            DiagramRect targetBounds = target.Source.Bounds;
            List<SnapSize> sameSizes = new List<SnapSize> { sameHeights[0] };
            for (int i = 0; i < sameHeights.Count; i++)
            {
                DiagramRect currentBounds = sameHeights[i].Source.Bounds;
                if (currentBounds.Height.Equals(targetBounds.Height))
                {
                    sameSizes.Add(sameHeights[i]);
                }
            }
            if (!ended)
            {
                DiagramPoint start = new DiagramPoint() { X = bounds.X + bounds.Width + 15, Y = bounds.Y + target.Offset };
                DiagramPoint end = new DiagramPoint() { X = bounds.X + bounds.Width + 15, Y = bounds.Y + target.Offset + bounds.Height };
                this.CreateSpacingLines(start, end, diagram.Scroller.Transform);
                for (int i = 0; i < sameSizes.Count; i++)
                {
                    bounds = sameSizes[i].Source.Bounds;
                    start = new DiagramPoint() { X = bounds.X + bounds.Width + 15, Y = bounds.Y };
                    end = new DiagramPoint() { X = bounds.X + bounds.Width + 15, Y = bounds.Y + bounds.Height };
                    this.CreateSpacingLines(start, end, diagram.Scroller.Transform);
                }
            }
            verticalSnap.Offset = target.Offset;
            verticalSnap.Snapped = true;
        }
        //Render spacing lines
        private void CreateSpacingLines(DiagramPoint start, DiagramPoint end, TransformFactor transform)
        {
            object d;
            LineAttributes line1;
            PathElement element = new PathElement();
            PathAttributes options = new PathAttributes();
            start = new DiagramPoint() { X = (start.X + transform.TX) * transform.Scale, Y = (start.Y + transform.TY) * transform.Scale };
            end = new DiagramPoint() { X = (end.X + transform.TX) * transform.Scale, Y = (end.Y + transform.TY) * transform.Scale };
            if (start.X.Equals(end.X))
            {
                d = 'M' + (start.X - 5) + ' ' + (start.Y + 5) + 'L' + start.X + ' ' + start.Y +
                    'L' + (start.X + 5) + ' ' + (start.Y + 5) + 'z' + 'M' + (end.X - 5) + ' ' +
                    (end.Y - 5) + 'L' + end.X + ' ' + end.Y + 'L' +
                    (end.X + 5) + ' ' + (end.Y - 5) + 'z';
                line1 = new LineAttributes()
                {
                    StartPoint = new DiagramPoint() { X = start.X - 8, Y = start.Y - 1 },
                    EndPoint = new DiagramPoint() { X = start.X + 8, Y = start.Y - 1 },
                    Stroke = LINECOLOR,
                    StrokeWidth = 1,
                    Fill = LINECOLOR,
                    DashArray = string.Empty,
                    Width = 1,
                    X = 0,
                    Y = 0,
                    Height = 0,
                    Angle = 0,
                    PivotX = 0,
                    PivotY = 0,
                    Visible = true,
                    Opacity = 1,
                    ID = new Random().ToString()
                };
                element.Data = d.ToString();
                options.Data = element.Data;
                options.Angle = 0;
                options.PivotX = 0;
                options.PivotY = 0;
                options.X = 0;
                options.Y = 0;
                options.Height = 0;
                options.Width = 1;
                options.ID = new Random().ToString();
                this.Lines.Add(line1);
                line1 = new LineAttributes()
                {
                    StartPoint = new DiagramPoint() { X = end.X - 8, Y = end.Y + 1 },
                    EndPoint = new DiagramPoint() { X = end.X + 8, Y = end.Y + 1 },
                    Stroke = LINECOLOR,
                    StrokeWidth = 1,
                    Fill = LINECOLOR,
                    DashArray = string.Empty,
                    Width = 1,
                    X = 0,
                    Y = 0,
                    Height = 0,
                    Angle = 0,
                    PivotX = 0,
                    PivotY = 0,
                    Visible = true,
                    Opacity = 1,
                    ID = new Random().ToString() + "spacing"
                };
                this.Lines.Add(line1);
            }
            else
            {
                d = 'M' + (start.X + 5) + ' ' + (start.Y + 5) + 'L' + start.X + ' ' + start.Y +
                    'L' + (start.X + 5) + ' ' + (start.Y - 5) + 'z' + 'M' + (end.X - 5) + ' ' +
                    (end.Y - 5) + 'L' + end.X + ' ' + end.Y +
                    'L' + (end.X - 5) + ' ' + (end.Y + 5) + 'z';
                element.Data = d.ToString();
                options.Data = element.Data;
                options.Angle = 0;
                options.PivotX = 0;
                options.PivotY = 0;
                options.X = 0;
                options.Y = 0;
                options.Height = 0;
                options.Width = 1;
                options.ID = new Random().ToString();
                line1 = new LineAttributes()
                {
                    Visible = true,
                    Opacity = 1,
                    ID = new Random().ToString(),
                    StartPoint = new DiagramPoint() { X = start.X - 1, Y = start.Y - 8 },
                    EndPoint = new DiagramPoint() { X = start.X - 1, Y = start.Y + 8 },
                    Stroke = LINECOLOR,
                    StrokeWidth = 1,
                    Fill = LINECOLOR,
                    DashArray = "0",
                    Width = 1,
                    X = 0,
                    Y = 0,
                    Height = 0,
                    Angle = 0,
                    PivotX = 0,
                    PivotY = 0
                };
                this.Lines.Add(line1);
                line1 = new LineAttributes()
                {
                    Width = 1,
                    X = 0,
                    Y = 0,
                    Height = 0,
                    Angle = 0,
                    PivotX = 0,
                    PivotY = 0,
                    Visible = true,
                    Opacity = 1,
                    ID = new Random().ToString(),
                    StartPoint = new DiagramPoint() { X = end.X + 1, Y = end.Y - 8 },
                    EndPoint = new DiagramPoint() { X = end.X + 1, Y = end.Y + 8 },
                    Stroke = LINECOLOR,
                    StrokeWidth = 1,
                    Fill = LINECOLOR,
                    DashArray = "0"
                };
                this.Lines.Add(line1);
            }
            line1 = new LineAttributes()
            {
                StartPoint = new DiagramPoint() { X = start.X, Y = start.Y },
                EndPoint = new DiagramPoint() { X = end.X, Y = end.Y },
                Stroke = LINECOLOR,
                StrokeWidth = 1,
                Fill = LINECOLOR,
                DashArray = "0",
                Width = 1,
                X = 0,
                Y = 0,
                Height = 0,
                Angle = 0,
                PivotX = 0,
                PivotY = 0,
                Visible = true,
                Opacity = 1,
                ID = new Random().ToString()
            };
            this.Lines.Add(line1);
        }
        private static SnapObject CreateSnapObject(DiagramRect targetBounds, DiagramRect bounds, SnapAlignment snap)
        {
            SnapObject snapObject = null;
            switch (snap)
            {
                case SnapAlignment.Left:
                    snapObject = new SnapObject()
                    {
                        Start = new DiagramPoint() { X = (targetBounds.X), Y = Math.Min(targetBounds.Y, bounds.Y) },
                        End = new DiagramPoint() { X = (targetBounds.X), Y = Math.Max(targetBounds.Y + targetBounds.Height, bounds.Y + bounds.Height) },
                        OffsetX = targetBounds.X - bounds.X,
                        OffsetY = 0,
                        Type = SIDEALIGN
                    };
                    break;
                case SnapAlignment.Right:
                    snapObject = new SnapObject()
                    {
                        Type = SIDEALIGN,
                        Start = new DiagramPoint() { X = (targetBounds.X + targetBounds.Width), Y = Math.Min(targetBounds.Y, bounds.Y) },
                        OffsetX = targetBounds.X + targetBounds.Width - bounds.X - bounds.Width,
                        OffsetY = 0,
                        End = new DiagramPoint()
                        {
                            X = (targetBounds.X + targetBounds.Width),
                            Y = Math.Max(targetBounds.Y + targetBounds.Height, bounds.Y + bounds.Height)
                        }
                    };
                    break;
                case SnapAlignment.Top:
                    snapObject = new SnapObject()
                    {
                        OffsetY = targetBounds.Y - bounds.Y,
                        OffsetX = 0,
                        Type = SIDEALIGN,
                        Start = new DiagramPoint() { X = (Math.Min(targetBounds.X, bounds.X)), Y = targetBounds.Y },
                        End = new DiagramPoint() { X = (Math.Max(targetBounds.X + targetBounds.Width, bounds.X + bounds.Width)), Y = targetBounds.Y }
                    };
                    break;
                case SnapAlignment.Bottom:
                    snapObject = new SnapObject()
                    {
                        Type = SIDEALIGN,
                        OffsetY = targetBounds.Y + targetBounds.Height - bounds.Y - bounds.Height,
                        OffsetX = 0,
                        End = new DiagramPoint()
                        {
                            X = (Math.Max(targetBounds.X + targetBounds.Width, bounds.X + bounds.Width)),
                            Y = targetBounds.Y + targetBounds.Height
                        },
                        Start = new DiagramPoint() { X = (Math.Min(targetBounds.X, bounds.X)), Y = targetBounds.Y + targetBounds.Height }
                    };
                    break;
                case SnapAlignment.TopBottom:
                    snapObject = new SnapObject()
                    {
                        Start = new DiagramPoint() { X = (Math.Min(targetBounds.X, bounds.X)), Y = targetBounds.Y + targetBounds.Height },
                        End = new DiagramPoint()
                        {
                            X = (Math.Max(targetBounds.X + targetBounds.Width, bounds.X + bounds.Width)),
                            Y = targetBounds.Y + targetBounds.Height
                        },
                        OffsetY = targetBounds.Y + targetBounds.Height - bounds.Y,
                        OffsetX = 0,
                        Type = SIDEALIGN
                    };
                    break;
                case SnapAlignment.BottomTop:
                    snapObject = new SnapObject()
                    {
                        Start = new DiagramPoint() { X = (Math.Min(targetBounds.X, bounds.X)), Y = targetBounds.Y },
                        End = new DiagramPoint() { X = (Math.Max(targetBounds.X + targetBounds.Width, bounds.X + bounds.Width)), Y = targetBounds.Y },
                        OffsetY = targetBounds.Y - bounds.Y - bounds.Height,
                        OffsetX = 0,
                        Type = SIDEALIGN
                    };
                    break;
                case SnapAlignment.LeftRight:
                    snapObject = new SnapObject()
                    {
                        Start = new DiagramPoint() { X = (targetBounds.X + targetBounds.Width), Y = Math.Min(targetBounds.Y, bounds.Y) },
                        End = new DiagramPoint()
                        {
                            X = (targetBounds.X + targetBounds.Width),
                            Y = Math.Max(targetBounds.Y + targetBounds.Height, bounds.Y + bounds.Height)
                        },
                        OffsetX = targetBounds.X + targetBounds.Width - bounds.X,
                        OffsetY = 0,
                        Type = SIDEALIGN
                    };
                    break;
                case SnapAlignment.RightLeft:
                    snapObject = new SnapObject()
                    {
                        Start = new DiagramPoint() { X = (targetBounds.X), Y = (Math.Min(targetBounds.Y, bounds.Y)) },
                        End = new DiagramPoint() { X = (targetBounds.X), Y = Math.Max(targetBounds.Y + targetBounds.Height, bounds.Y + bounds.Height) },
                        OffsetX = targetBounds.X - bounds.X - bounds.Width,
                        OffsetY = 0,
                        Type = SIDEALIGN
                    };
                    break;
                case SnapAlignment.CenterX:
                    snapObject = new SnapObject()
                    {
                        Start = new DiagramPoint() { X = (targetBounds.X + targetBounds.Width / 2), Y = (Math.Min(targetBounds.Y, bounds.Y)) },
                        End = new DiagramPoint()
                        {
                            X = (targetBounds.X + targetBounds.Width / 2),
                            Y = Math.Max(targetBounds.Y + targetBounds.Height, bounds.Y + bounds.Height)
                        },
                        OffsetX = targetBounds.X + targetBounds.Width / 2 - (bounds.X + bounds.Width / 2),
                        OffsetY = 0,
                        Type = CENTERALIGN
                    };
                    break;
                case SnapAlignment.CenterY:
                    snapObject = new SnapObject()
                    {
                        Start = new DiagramPoint() { X = (Math.Min(targetBounds.X, bounds.X)), Y = targetBounds.Y + targetBounds.Height / 2 },
                        End = new DiagramPoint()
                        {
                            X = (Math.Max(targetBounds.X + targetBounds.Width, bounds.X + bounds.Width)),
                            Y = targetBounds.Y + targetBounds.Height / 2
                        },
                        OffsetY = targetBounds.Y + targetBounds.Height / 2 - (bounds.Y + bounds.Height / 2),
                        OffsetX = 0,
                        Type = CENTERALIGN
                    };
                    break;
            }
            return snapObject;
        }
        internal double SnapAngle(double angle)
        {
            SnapSettings snapSettings = this.diagram.SnapSettings;
            double snapAngle = snapSettings.SnapAngle;
            double width = angle % (snapAngle);
            if (width >= (snapAngle / 2))
            {
                return angle + snapAngle - width;
            }
            else
            {
                return angle - width;
            }
        }

        //Check whether the node to be snapped or not.
        private bool CanConsider(DiagramElement target)
        {
            if (this.diagram.SelectionSettings.Nodes.Count > 0 && this.diagram.SelectionSettings.Nodes[0].ID == (target as DiagramElement).ID)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Find the total number of nodes in diagram using SpatialSearch
        private List<DiagramElement> FindNodes(SpatialSearch spatialSearch, DiagramRect child, DiagramRect viewPort, List<DiagramElement> nodesInView)
        {
            List<DiagramElement> nodes = new List<DiagramElement>();
            DiagramRect quadRect = (nodesInView != null) ? viewPort : child;
            ObservableCollection<Quad> quads = spatialSearch.FindQuads(quadRect);
            for (int i = 0; i < quads.Count; i++)
            {
                Quad quad = quads[i];
                if (quad.Objects.Count > 0)
                {
                    for (int j = 0; j < quad.Objects.Count; j++)
                    {
                        DiagramElement nd = quad.Objects[j] as DiagramElement;
                        if (!(this.diagram.NameTable[nd.ID] is Connector) && nd.Visible)
                        {
                            DiagramRect bounds = BaseUtil.GetBounds(nd);
                            if (nodes.IndexOf(nd) == -1 && IntersectsRect(child, bounds))
                            {
                                nodes.Add(nd);
                            }
                            if (nodesInView != null && IntersectsRect(viewPort, bounds))
                            {
                                nodesInView.Add(nd);
                            }
                        }
                    }
                }
            }
            return nodes;
        }
        private static bool IntersectsRect(DiagramRect child, DiagramRect bounds)
        {
            return ((((bounds.X < (child.X + child.Width)) && (child.X < (bounds.X + bounds.Width)))
                && (bounds.Y < (child.Y + child.Height))) && (child.Y < (bounds.Y + bounds.Height)));
        }
        //Sort the objects by its distance
        private static void SortByDistance(List<SnapObjects> obj, bool ascending = false)
        {
            int i;
            int j;
            SnapObjects temp;
            if (ascending)
            {
                for (i = 0; i < obj.Count; i++)
                {
                    for (j = i + 1; j < obj.Count; j++)
                    {
                        if (obj[i].Distance > obj[j].Distance)
                        {
                            temp = obj[i];
                            obj[i] = obj[j];
                            obj[j] = temp;
                        }
                    }
                }
            }
            else
            {
                for (i = 0; i < obj.Count; i++)
                {
                    for (j = i + 1; j < obj.Count; j++)
                    {
                        if (obj[i].Distance < obj[j].Distance)
                        {
                            temp = obj[i];
                            obj[i] = obj[j];
                            obj[j] = temp;
                        }
                    }
                }
            }
        }
        private static void SortByDistance(List<SnapSize> obj, bool ascending = false)
        {
            int i;
            int j;
            SnapSize temp;
            if (ascending)
            {
                for (i = 0; i < obj.Count; i++)
                {
                    for (j = i + 1; j < obj.Count; j++)
                    {
                        if (obj[i].Offset > obj[j].Offset)
                        {
                            temp = obj[i];
                            obj[i] = obj[j];
                            obj[j] = temp;
                        }
                    }
                }
            }
            else
            {
                for (i = 0; i < obj.Count; i++)
                {
                    for (j = i + 1; j < obj.Count; j++)
                    {
                        if (obj[i].Offset < obj[j].Offset)
                        {
                            temp = obj[i];
                            obj[i] = obj[j];
                            obj[j] = temp;
                        }
                    }
                }
            }
        }
        //To find nodes that are equally placed at left of the selected node
        private static double FindEquallySpacedNodesAtLeft(List<SnapObjects> objectsAtLeft, double equalDistance, double top, List<SnapObjects> equallySpaced)
        {
            int i;
            for (i = 1; i < objectsAtLeft.Count; i++)
            {
                DiagramRect prevBounds = ((objectsAtLeft[i - 1].Element).Bounds);
                DiagramRect targetBounds = ((objectsAtLeft[i].Element).Bounds);
                Double dist = prevBounds.X - targetBounds.X - targetBounds.Width;
                if (Math.Abs(dist - equalDistance) <= 1)
                {
                    equallySpaced.Add(objectsAtLeft[i]);
                    if (targetBounds.Y < top)
                    {
                        top = targetBounds.Y;
                    }
                }
                else
                {
                    break;
                }
            }
            return top;
        }
        //To find nodes that are equally placed at right of the selected node
        private static double FindEquallySpacedNodesAtRight(List<SnapObjects> objectsAtRight, double equalDistance, double top, List<SnapObjects> equallySpaced, double snapObjDistance)
        {
            double actualDistance = objectsAtRight[0].Distance;
            if (Math.Abs(equalDistance - actualDistance) <= snapObjDistance)
            {
                for (int i = 0; i < objectsAtRight.Count - 1; i++)
                {
                    DiagramElement target = objectsAtRight[i].Element;
                    DiagramRect targetBounds = ((objectsAtRight[i + 1].Element).Bounds);
                    DiagramRect prevBounds = (target.Bounds);
                    double dist = targetBounds.X - prevBounds.X - prevBounds.Width;
                    if (Math.Abs(dist - equalDistance) <= 1)
                    {
                        equallySpaced.Add(objectsAtRight[i + 1]);
                        if (prevBounds.Y < top)
                        {
                            top = prevBounds.Y;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return top;
        }
        private static double FindEquallySpacedNodesAtTop(List<SnapObjects> objectsAtTop, double equalDistance, double right, List<SnapObjects> equallySpaced)
        {
            for (int i = 1; i < objectsAtTop.Count; i++)
            {
                DiagramRect prevBounds = ((objectsAtTop[i - 1].Element).Bounds);
                DiagramRect targetBounds = ((objectsAtTop[i].Element).Bounds);
                double dist = prevBounds.Y - targetBounds.Y - targetBounds.Height;
                if (Math.Abs(dist - equalDistance) <= 1)
                {
                    equallySpaced.Add(objectsAtTop[i]);
                    if (targetBounds.X + targetBounds.Width > right)
                    {
                        right = targetBounds.X + targetBounds.Width;
                    }
                }
                else
                {
                    break;
                }
            }
            return right;
        }
        //To find nodes that are equally placed at bottom of the selected node
        private static double FindEquallySpacedNodesAtBottom(List<SnapObjects> objectsAtBottom, double equalDistance, double right, List<SnapObjects> equallySpaced, double snapObjDistance)
        {
            double actualDistance = objectsAtBottom[0].Distance;
            if (Math.Abs(equalDistance - actualDistance) <= snapObjDistance)
            {
                for (int i = 0; i < objectsAtBottom.Count - 1; i++)
                {
                    DiagramElement target = objectsAtBottom[i].Element;
                    DiagramRect targetBounds = ((objectsAtBottom[i + 1].Element).Bounds);
                    DiagramRect prevBounds = (target.Bounds);
                    double dist = targetBounds.Y - prevBounds.Y - prevBounds.Height;
                    if (Math.Abs(dist - equalDistance) <= 1)
                    {
                        equallySpaced.Add(objectsAtBottom[i + 1]);
                        if (prevBounds.X + prevBounds.Width > right)
                        {
                            right = prevBounds.X + prevBounds.Width;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return right;

        }

        internal void Dispose()
        {
            if (diagram != null)
            {
                diagram = null;
            }
            if (Lines != null)
            {
                for (int i = 0; i < Lines.Count; i++)
                {
                    Lines[i].Dispose();
                    Lines[i] = null;
                }
                Lines.Clear();
                Lines = null;
            }
        }
    }
    internal class Snap
    {

        internal bool Snapped { get; set; }

        internal double Offset { get; set; }

        internal bool Left { get; set; }

        internal bool Bottom { get; set; }

        internal bool Right { get; set; }

        internal bool Top { get; set; }

    }
    internal class SnapObject
    {

        internal DiagramPoint Start { get; set; }

        internal DiagramPoint End { get; set; }

        internal double OffsetX { get; set; }

        internal double OffsetY { get; set; }

        internal string Type { get; set; }

    }
    internal class SnapObjects
    {

        internal DiagramElement Element { get; set; }
        internal DiagramSelectionSettings Selector { get; set; }

        internal double Distance { get; set; }

    }

    internal class SnapSize
    {

        internal DiagramElement Source { get; set; }

        internal double Difference { get; set; }

        internal double Offset { get; set; }

    }
}

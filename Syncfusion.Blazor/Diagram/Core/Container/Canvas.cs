using Syncfusion.Blazor.Diagram.Internal;
using System;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// A canvas is used to define a plane (canvas) and arrange children according to margins.
    /// </summary>
    public class Canvas : DiagramContainer
    {
        /// <summary>
        /// Measures the minimum space that the canvas requires
        /// </summary>
        internal override DiagramSize Measure(DiagramSize availableSize)
        {
            DiagramSize desired = null;
            if (this.HasChildren())
            {
                DiagramRect desiredBounds = MeasureChildElement(availableSize);
                if (desiredBounds != null)
                {
                    double leftMargin = Math.Max(desiredBounds.Left, 0);
                    double topMargin = Math.Max(desiredBounds.Top, 0);
                    desired = new DiagramSize() { Width = desiredBounds.Width + leftMargin, Height = desiredBounds.Height + topMargin };
                }
            }

            desired = base.ValidateDesiredSize(desired, availableSize);
            StretchChildren(desired);

            //Considering padding values
            desired.Width += this.Padding.Left + this.Padding.Right;
            desired.Height += this.Padding.Top + this.Padding.Bottom;
            this.DesiredSize = desired;
            return desired;
        }

        private DiagramRect MeasureChildElement(DiagramSize availableSize)
        {
            DiagramRect desiredBounds = null;
            //Measuring the children
            foreach (ICommonElement child in this.Children)
            {
                if (child is TextElement)
                {
                    if ((child as TextElement).CanMeasure)
                    {
                        (child as TextElement).Measure(availableSize);
                    }
                }
                else
                {
                    child?.Measure(availableSize);
                }

                if (child != null)
                {
                    DiagramSize childSize = child.DesiredSize.Clone();

                    if (child.RotationAngle != 0)
                    {
                        childSize = BaseUtil.RotateSize(childSize, child.RotationAngle);
                    }

                    double right = BaseUtil.GetDoubleValue(childSize.Width) + child.Margin.Right;
                    double bottom = BaseUtil.GetDoubleValue(childSize.Height) + child.Margin.Bottom;
                    DiagramRect childBounds = new DiagramRect(child.Margin.Left, child.Margin.Top, right, bottom);

                    if (child.Float)
                    {
                        DiagramPoint position = child.GetAbsolutePosition(childSize);
                        if (position != null)
                        {
                            continue;
                        }
                    }

                    if (!(child is TextElement element) || element.CanConsiderBounds)
                    {
                        if (desiredBounds == null)
                        {
                            desiredBounds = childBounds;
                        }
                        else
                        {
                            desiredBounds.UniteRect(childBounds);
                        }
                    }
                }
            }
            return desiredBounds;
        }

        /// <summary>
        /// Arranges the child elements of the canvas
        /// </summary>
        internal override DiagramSize Arrange(DiagramSize desiredSize, bool? isStack)
        {
            this.OuterBounds = new DiagramRect();
            if (this.HasChildren())
            {
                ArrangeChildElement(desiredSize, isStack);
            }
            this.ActualSize = desiredSize;
            this.UpdateBounds();
            this.OuterBounds.UniteRect(this.Bounds);
            return desiredSize;
        }

        private void ArrangeChildElement(DiagramSize desiredSize, bool? isStack)
        {
            double y = this.OffsetY - BaseUtil.GetDoubleValue(desiredSize.Height) * this.Pivot.Y + this.Padding.Top;
            double x = this.OffsetX - BaseUtil.GetDoubleValue(desiredSize.Width) * this.Pivot.X + this.Padding.Left;
            foreach (ICommonElement child in this.Children)
            {
                if ((child.Transform & Transform.Parent) != 0)
                {
                    child.ParentTransform = this.ParentTransform + this.RotationAngle;
                    if (this.Flip != FlipDirection.None || (ElementActions & ElementAction.ElementIsGroup) != 0)
                    {
                        child.ParentTransform = (this.Flip == FlipDirection.Horizontal || this.Flip == FlipDirection.Vertical) ?
                            -child.ParentTransform : child.ParentTransform;
                    }
                    DiagramSize childSize = child.DesiredSize.Clone();
                    double childX = x;
                    double childY = y;
                    if (child.RelativeMode == RelativeMode.Point)
                    {
                        DiagramPoint position = child.GetAbsolutePosition(desiredSize);
                        if (position != null)
                        {
                            childX += position.X;
                            childY += position.Y;
                        }
                    }

                    DiagramPoint topLeft = child.RelativeMode == RelativeMode.Object ? AlignChildBasedOnParent(child, childSize, desiredSize, childX, childY) : AlignChildBasedOnPoint(child as DiagramElement, childX, childY);
                    DiagramPoint center = new DiagramPoint(topLeft.X + BaseUtil.GetDoubleValue(childSize.Width) / 2, topLeft.Y + BaseUtil.GetDoubleValue(childSize.Height) / 2);
                    FindChildOffsetFromCenter(child as DiagramElement, center);
                }
                if (isStack == true && (child.HorizontalAlignment == HorizontalAlignment.Stretch || child.VerticalAlignment == VerticalAlignment.Stretch))
                {
                    child.Arrange(desiredSize, true);
                    //child.Arrange(desiredSize);
                }
                else
                {
                    if (child is TextElement element && element.CanMeasure)
                    {
                        element.Arrange(element.DesiredSize, false);
                        this.OuterBounds.UniteRect(element.OuterBounds);
                    }
                    else
                    {
                        child.Arrange(child.DesiredSize, false);
                        this.OuterBounds.UniteRect(child.OuterBounds);
                    }
                }
            }
        }

        /// <summary>
        /// Aligns the child element based on its parent
        /// </summary>
        private static DiagramPoint AlignChildBasedOnParent(ICommonElement child, DiagramSize childSize, DiagramSize parentSize, double x, double y)
        {
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Auto:
                case HorizontalAlignment.Left:
                    x += child.Margin.Left;
                    break;
                case HorizontalAlignment.Right:
                    x += BaseUtil.GetDoubleValue(parentSize.Width) - BaseUtil.GetDoubleValue(childSize.Width) - child.Margin.Right;
                    break;
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    x += BaseUtil.GetDoubleValue(parentSize.Width) / 2 - BaseUtil.GetDoubleValue(childSize.Width) / 2;
                    break;
            }

            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Auto:
                case VerticalAlignment.Top:
                    y += child.Margin.Top;
                    break;
                case VerticalAlignment.Bottom:
                    y += BaseUtil.GetDoubleValue(parentSize.Height) - BaseUtil.GetDoubleValue(childSize.Height) - child.Margin.Bottom;
                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    y += BaseUtil.GetDoubleValue(parentSize.Height) / 2 - BaseUtil.GetDoubleValue(childSize.Height) / 2;
                    break;
            }

            return new DiagramPoint(x, y);
        }

        /// <summary>
        /// Aligns the child elements based on a point
        /// </summary>
        private static DiagramPoint AlignChildBasedOnPoint(DiagramElement child, double x, double y)
        {
            x += child.Margin.Left - child.Margin.Right;
            y += child.Margin.Top - child.Margin.Bottom;
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Auto:
                case HorizontalAlignment.Left:
                    x = child.InversedAlignment ? x : (x - BaseUtil.GetDoubleValue(child.DesiredSize.Width));
                    break;
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Center:
                    x -= BaseUtil.GetDoubleValue(child.DesiredSize.Width) * child.Pivot.X;
                    break;
                case HorizontalAlignment.Right:
                    x = child.InversedAlignment ? (x - BaseUtil.GetDoubleValue(child.DesiredSize.Width)) : x;
                    break;
            }
            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Auto:
                case VerticalAlignment.Top:
                    y = child.InversedAlignment ? y : (y - BaseUtil.GetDoubleValue(child.DesiredSize.Height));
                    break;
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    y -= BaseUtil.GetDoubleValue(child.DesiredSize.Height) * child.Pivot.Y;
                    break;
                case VerticalAlignment.Bottom:
                    y = child.InversedAlignment ? (y - BaseUtil.GetDoubleValue(child.DesiredSize.Height)) : y;
                    break;
            }
            return new DiagramPoint(x, y);
        }
    }
}
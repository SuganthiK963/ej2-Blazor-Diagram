using Syncfusion.Blazor.Diagram.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// The DiagramContainer is used to group related objects.
    /// </summary>
    public class DiagramContainer : DiagramElement
    {
        private DiagramRect DesiredBounds { get; set; }
        internal bool MeasureChildren { get; set; } = true;
        internal double PrevRotateAngle { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="DiagramContainer"/> from the given <see cref="DiagramContainer"/>.
        /// </summary>
        /// <param name="src">DiagramContainer</param>
        public DiagramContainer(DiagramContainer src) : base(src)
        {
            if (src != null)
            {
                PrevRotateAngle = src.PrevRotateAngle;
                MeasureChildren = src.MeasureChildren;
                DesiredBounds = src.DesiredBounds;
                Padding = src.Padding;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramContainer"/>.
        /// </summary>
        public DiagramContainer() : base()
        {
        }

        /// <summary>
        /// Gets or sets the space between the container and its immediate children.
        /// </summary>
        public Thickness Padding { get; set; } = new Thickness() { Left = 0, Right = 0, Top = 0, Bottom = 0 };

        /// <summary>
        /// Gets or sets the collection of child elements (Canvas, Diagram Element). 
        /// </summary>
        public ObservableCollection<ICommonElement> Children { get; set; } = new ObservableCollection<ICommonElement>();

        /// <summary>
        /// Returns a value indiciate whether the container has child elements or not
        /// </summary>
        internal bool HasChildren()
        {
            return this.Children != null && this.Children.Count > 0;
        }

        /// <summary>
        /// Measures the minimum space that the container requires
        /// </summary>
        internal override DiagramSize Measure(DiagramSize availableSize)
        {
            // measure the element and find the desired size
            this.DesiredBounds = null; DiagramSize desired = null;
            if (this.HasChildren())
            {
                this.DesiredBounds = MeasureChildElement(availableSize);
                if (this.DesiredBounds != null && this.RotationAngle != 0)
                {
                    DiagramPoint offsetPt = new DiagramPoint()
                    {
                        X = this.DesiredBounds.X + this.DesiredBounds.Width * this.Pivot.X,
                        Y = this.DesiredBounds.Y + this.DesiredBounds.Height * this.Pivot.Y
                    };
                    DiagramPoint newPoint = BaseUtil.RotatePoint(this.RotationAngle, 0, 0, offsetPt);
                    this.DesiredBounds.X = newPoint.X - this.DesiredBounds.Width * this.Pivot.X;
                    this.DesiredBounds.Y = newPoint.Y - this.DesiredBounds.Height * this.Pivot.Y;
                }
                if (this.DesiredBounds != null)
                {
                    desired = new DiagramSize() { Width = this.DesiredBounds.Width, Height = this.DesiredBounds.Height };
                }
            }

            desired = this.ValidateDesiredSize(desired, availableSize);
            this.StretchChildren(desired);
            this.DesiredSize = desired;
            return desired;
        }

        private DiagramRect MeasureChildElement(DiagramSize availableSize)
        {
            this.DesiredBounds = null;
            //Measuring the children
            for (int i = 0; i < this.Children.Count; i++)
            {
                DiagramElement child = this.Children[i] as DiagramElement;
                if (child.HorizontalAlignment == HorizontalAlignment.Stretch && availableSize.Width != 0)
                {
                    availableSize.Width = child.Bounds.Width;
                }
                if (child.VerticalAlignment == VerticalAlignment.Stretch && availableSize.Height != 0)
                {
                    availableSize.Height = child.Bounds.Height;
                }
                bool force = child.HorizontalAlignment == HorizontalAlignment.Stretch || child.VerticalAlignment == VerticalAlignment.Stretch;
                if (this.MeasureChildren || force || (child is DiagramContainer && (child as DiagramContainer).MeasureChildren))
                {
                    child.Measure(availableSize);
                }
                DiagramRect childBounds = this.GetChildrenBounds(child);
                if (child.HorizontalAlignment != HorizontalAlignment.Stretch && child.VerticalAlignment != VerticalAlignment.Stretch)
                {
                    if (this.DesiredBounds == null)
                    {
                        this.DesiredBounds = childBounds;
                    }
                    else
                    {
                        this.DesiredBounds.UniteRect(childBounds);
                    }
                }
                else if (this.ActualSize != null && this.ActualSize.Width != 0 && this.ActualSize.Height != 0 &&
                  !child.PreventContainer && child.HorizontalAlignment == HorizontalAlignment.Stretch && child.VerticalAlignment == VerticalAlignment.Stretch)
                {
                    if (this.DesiredBounds == null)
                    {
                        this.DesiredBounds = child.Bounds;
                    }
                    else
                    {
                        this.DesiredBounds.UniteRect(child.Bounds);
                    }
                }
            }
            return DesiredBounds;
        }

        /// <summary>
        /// Arranges the container and its children
        /// </summary>
        internal override DiagramSize Arrange(DiagramSize desiredSize, bool? isStack = false)
        {
            DiagramRect childBounds = this.DesiredBounds;
            if (childBounds != null)
            {
                this.OffsetX = childBounds.X + childBounds.Width * this.Pivot.X;
                this.OffsetY = childBounds.Y + childBounds.Height * this.Pivot.Y;

                // container has rotateAngle
                if (this.HasChildren())
                {
                    //Measuring the children
                    for (int i = 0; i < this.Children.Count; i++)
                    {
                        DiagramElement child = this.Children[i] as DiagramElement;
                        bool arrange = false;
                        if (child != null && child.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            child.OffsetX = this.OffsetX;
                            child.ParentTransform = this.ParentTransform + this.RotationAngle;
                            if (((this.ElementActions & ElementAction.ElementIsGroup) != 0))
                            {
                                child.ParentTransform = (this.Flip == FlipDirection.Horizontal || this.Flip == FlipDirection.Vertical) ?
                                    -child.ParentTransform : child.ParentTransform;
                            }
                            arrange = true;
                        }
                        if (child.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            child.OffsetY = this.OffsetY;
                            child.ParentTransform = this.ParentTransform + this.RotationAngle;
                            arrange = true;
                        }
                        if (arrange || this.MeasureChildren || (child is DiagramContainer container && container.MeasureChildren))
                        {
                            child.Arrange(child.DesiredSize, isStack);
                        }
                    }
                }
            }
            this.ActualSize = desiredSize;
            this.UpdateBounds();
            this.PrevRotateAngle = this.RotationAngle;
            return desiredSize;
        }

        /// <summary>
        /// Stretches the child elements based on the size of the container.
        /// </summary>
        internal void StretchChildren(DiagramSize size)
        {
            if (size != null && this.HasChildren())
            {
                int i;
                for (i = 0; i < this.Children.Count; i++)
                {
                    ICommonElement child = this.Children[i];
                    if (child.HorizontalAlignment == HorizontalAlignment.Stretch || child.DesiredSize.Width == null)
                    {
                        child.DesiredSize.Width = size.Width - child.Margin.Left - child.Margin.Right;
                    }
                    if (child.VerticalAlignment == VerticalAlignment.Stretch || child.DesiredSize.Height == null)
                    {
                        child.DesiredSize.Height = size.Height - child.Margin.Top - child.Margin.Bottom;
                    }
                    if (child.GetType() == typeof(DiagramContainer))
                    {
                        (child as DiagramContainer)?.StretchChildren(child.DesiredSize);
                    }
                }
            }
        }

        /// <summary>
        /// Considers the padding of the element when measuring its desired size.
        /// </summary>
        internal void ApplyPadding(DiagramSize size)
        {
            if (size != null)
            {
                size.Width += this.Padding.Left + this.Padding.Right;
                size.Height += this.Padding.Top + this.Padding.Bottom;
            }
        }

        /// <summary>
        /// Finds the offset of the child element with respect to the container.
        /// </summary>
        internal void FindChildOffsetFromCenter(DiagramElement child, DiagramPoint center)
        {
            if (child != null && center != null)
            {
                DiagramPoint topLeft = new DiagramPoint(center.X - BaseUtil.GetDoubleValue(child.DesiredSize.Width) / 2, center.Y - BaseUtil.GetDoubleValue(child.DesiredSize.Height) / 2);

                DiagramPoint offset = BaseUtil.GetOffset(topLeft, child);
                //Rotate based on child rotate angle
                offset = BaseUtil.RotatePoint(child.RotationAngle, center.X, center.Y, offset);
                //Rotate based on parent pivot
                offset = BaseUtil.RotatePoint(this.RotationAngle + this.ParentTransform, this.OffsetX, this.OffsetY, offset);

                child.OffsetX = offset.X;
                child.OffsetY = offset.Y;
            }
        }

        private DiagramRect GetChildrenBounds(DiagramElement child)
        {
            DiagramSize childSize = child.DesiredSize.Clone();
            DiagramPoint refPoint = new DiagramPoint(BaseUtil.GetDoubleValue(child.OffsetX), BaseUtil.GetDoubleValue(child.OffsetY));

            double left = refPoint.X - BaseUtil.GetDoubleValue(childSize.Width) * child.Pivot.X;
            double top = refPoint.Y - BaseUtil.GetDoubleValue(childSize.Height) * child.Pivot.Y;
            double right = left + BaseUtil.GetDoubleValue(childSize.Width);
            double bottom = top + BaseUtil.GetDoubleValue(childSize.Height);

            DiagramPoint topLeft = new DiagramPoint(left, top);
            DiagramPoint topRight = new DiagramPoint(right, top);
            DiagramPoint bottomLeft = new DiagramPoint(left, bottom);
            DiagramPoint bottomRight = new DiagramPoint(right, bottom);

            topLeft = BaseUtil.RotatePoint(child.RotationAngle, child.OffsetX, child.OffsetY, topLeft);
            topRight = BaseUtil.RotatePoint(child.RotationAngle, child.OffsetX, child.OffsetY, topRight);
            bottomLeft = BaseUtil.RotatePoint(child.RotationAngle, child.OffsetX, child.OffsetY, bottomLeft);
            bottomRight = BaseUtil.RotatePoint(child.RotationAngle, child.OffsetX, child.OffsetY, bottomRight);

            if (this.RotationAngle != 0)
            {
                topLeft = BaseUtil.RotatePoint(-this.RotationAngle, 0, 0, topLeft);
                topRight = BaseUtil.RotatePoint(-this.RotationAngle, 0, 0, topRight);
                bottomLeft = BaseUtil.RotatePoint(-this.RotationAngle, 0, 0, bottomLeft);
                bottomRight = BaseUtil.RotatePoint(-this.RotationAngle, 0, 0, bottomRight);
            }
            return DiagramRect.ToBounds(new List<DiagramPoint> { topLeft, topRight, bottomLeft, bottomRight });
        }
        /// <summary>
        /// Creates a new element that is a copy of the current element.
        /// </summary>
        /// <returns>DiagramContainer</returns>
        public override object Clone()
        {
            return new DiagramContainer(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            if (DesiredBounds != null)
            {
                DesiredBounds.Dispose();
                DesiredBounds = null;
            }
            if (Padding != null)
            {
                Padding.Dispose();
                Padding = null;
            }
            if (this.HasChildren())
            {
                foreach (ICommonElement child in Children)
                {
                    child.Dispose();
                }
                Children.Clear();
                Children = null;
            }
        }
    }
}
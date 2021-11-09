using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    ///  Represents the class that arranges child elements into a single line that can be oriented horizontally or vertically.
    /// </summary>
    public class StackPanel : DiagramContainer
    {
        /// <summary>
        /// Padding of the element needs to be measured
        /// </summary>
        private readonly bool considerPadding = true;
        /// <summary>
        /// Gets or sets a value that indicates the dimension by which child elements are stacked.
        /// </summary>
        [JsonPropertyName("orientation")]
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        /// <summary>
        /// Creates a new instance of the <see cref="StackPanel"/> from the given StackPanel.
        /// </summary>
        /// <param name="src">StackPanel element.</param>
        public StackPanel(StackPanel src) : base(src)
        {
            if (src != null)
            {
                Orientation = src.Orientation;
                considerPadding = src.considerPadding;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanel"/>.
        /// </summary>
        public StackPanel() : base()
        {
        }
        /// <summary>
        /// Measures the minimum space that the panel needs
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>

        internal override DiagramSize Measure(DiagramSize availableSize)
        {
            this.DesiredSize = this.MeasureStackPanel(availableSize);
            return this.DesiredSize;
        }
        /// <summary>
        /// Arranges the child elements of the stack panel
        /// </summary>
        /// <param name="desiredSize"></param>
        /// <param name="isStack"></param>
        /// <returns></returns>
        internal override DiagramSize Arrange(DiagramSize desiredSize, bool? isStack = false)
        {
            this.ActualSize = this.ArrangeStackPanel(desiredSize);
            this.UpdateBounds();
            return this.ActualSize;
        }
        /// <summary>
        /// Measures the minimum space that the panel needs
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        private DiagramSize MeasureStackPanel(DiagramSize availableSize)
        {
            DiagramSize desired = null;
            if (this.Children != null && this.Children.Count > 0)
            {
                ObservableCollection<ICommonElement> a = this.Children;
                foreach (ICommonElement child in a)
                {
                    child.ParentTransform = this.RotationAngle + this.ParentTransform;
                    child.Measure(MeasureChildren ? child.DesiredSize : availableSize);
                    var childSize = child.DesiredSize.Clone();
                    ApplyChildMargin(child, childSize);
                    if (child.RotationAngle != 0)
                    {
                        childSize = BaseUtil.RotateSize(childSize, child.RotationAngle);
                    }
                    if (desired == null)
                    {
                        desired = childSize;
                    }
                    else
                    {
                        if (!child.PreventContainer)
                        {
                            UpdateSize(childSize, desired);
                        }
                    }
                }
            }
            desired = base.ValidateDesiredSize(desired, availableSize);
            this.StretchChildren(desired);
            if (this.considerPadding)
            {
                ApplyPadding(desired);
            }
            return desired;
        }
        private void UpdateSize(DiagramSize child, DiagramSize parent)
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    parent.Height = Math.Max(BaseUtil.GetDoubleValue(child.Height), BaseUtil.GetDoubleValue(parent.Height));
                    parent.Width += child.Width;
                    break;
                case Orientation.Vertical:
                    parent.Width = Math.Max(BaseUtil.GetDoubleValue(child.Width), BaseUtil.GetDoubleValue(parent.Width));
                    parent.Height += child.Height;
                    break;
            }
        }
        private static void ApplyChildMargin(ICommonElement child, DiagramSize size)
        {
            size.Height += child.Margin.Top + child.Margin.Bottom;
            size.Width += child.Margin.Left + child.Margin.Right;
        }
        /// <summary>
        /// Stretches the child elements based on the size of the panel.
        /// </summary>
        /// <param name="size">Size.</param>
        protected new void StretchChildren(DiagramSize size)
        {
            if (size != null && this.Children != null && this.Children.Count > 0)
            {
                foreach (ICommonElement child in Children)
                {
                    if (this.Orientation == Orientation.Vertical)
                    {
                        if (child.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            child.DesiredSize.Width = size.Width - (child.Margin.Left + child.Margin.Right);
                        }
                    }
                    else
                    {
                        if (child.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            child.DesiredSize.Height = size.Height - (child.Margin.Top + child.Margin.Bottom);
                        }
                    }
                }
            }
        }
        private DiagramSize ArrangeStackPanel(DiagramSize desiredSize)
        {
            if (this.Children != null && this.Children.Count > 0)
            {
                double x;
                double y;
                x = this.OffsetX - BaseUtil.GetDoubleValue(desiredSize.Width) * this.Pivot.X + this.Padding.Left;
                y = this.OffsetY - BaseUtil.GetDoubleValue(desiredSize.Height) * this.Pivot.Y + this.Padding.Top;
                foreach (ICommonElement child in this.Children)
                {
                    DiagramSize childSize = child.DesiredSize.Clone();
                    DiagramSize rotatedSize = childSize;

                    if (this.Orientation == Orientation.Vertical)
                    {
                        y += child.Margin.Top;
                    }
                    else
                    {
                        x += child.Margin.Left;
                    }

                    if (child.RotationAngle != 0)
                    {
                        rotatedSize = BaseUtil.RotateSize(childSize, child.RotationAngle);
                    }

                    DiagramPoint center = UpdatePosition(x, y, child, this, desiredSize, rotatedSize);
                    FindChildOffsetFromCenter(child as DiagramElement, center);
                    child.Arrange(childSize, true);

                    if (this.Orientation == Orientation.Vertical)
                    {
                        y += BaseUtil.GetDoubleValue(rotatedSize.Height) + child.Margin.Bottom;
                    }
                    else
                    {
                        x += BaseUtil.GetDoubleValue(rotatedSize.Width) + child.Margin.Right;
                    }
                }
            }
            return desiredSize;
        }

        private DiagramPoint UpdatePosition(double x, double y, ICommonElement child, StackPanel stackPanel, DiagramSize desiredSize, DiagramSize rotatedSize)
        {
            DiagramPoint point = new DiagramPoint();
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    point = ArrangeHorizontalStack(x, y, child, stackPanel, desiredSize, rotatedSize);
                    break;
                case Orientation.Vertical:
                    point = ArrangeVerticalStack(x, y, child, stackPanel, desiredSize, rotatedSize);
                    break;
            }
            return point;
        }

        private static DiagramPoint ArrangeHorizontalStack(double x, double y, ICommonElement child, StackPanel parent, DiagramSize parenBounds, DiagramSize childBounds)
        {
            double centerY;
            if (child.VerticalAlignment == VerticalAlignment.Top)
            {
                centerY = y + child.Margin.Top + BaseUtil.GetDoubleValue(childBounds.Height) / 2;
            }
            else if (child.VerticalAlignment == VerticalAlignment.Bottom)
            {
                double parentBottom = parent.OffsetY + BaseUtil.GetDoubleValue(parenBounds.Height) * (1 - parent.Pivot.Y);
                centerY = parentBottom - parent.Padding.Bottom - child.Margin.Bottom - BaseUtil.GetDoubleValue(childBounds.Height) / 2;
            }
            else
            {
                centerY = parent.OffsetY - BaseUtil.GetDoubleValue(parenBounds.Height) * parent.Pivot.Y + BaseUtil.GetDoubleValue(parenBounds.Height) / 2;
                if (child.Margin.Top != 0)
                {
                    centerY = y + child.Margin.Top + BaseUtil.GetDoubleValue(childBounds.Height) / 2;
                }
            }
            return new DiagramPoint() { X = x + BaseUtil.GetDoubleValue(childBounds.Width) / 2, Y = centerY };
        }

        private static DiagramPoint ArrangeVerticalStack(double x, double y, ICommonElement child, StackPanel parent, DiagramSize parentSize, DiagramSize childSize)
        {
            double centerX;
            if (child.HorizontalAlignment == HorizontalAlignment.Left)
            {
                centerX = x + child.Margin.Left + BaseUtil.GetDoubleValue(childSize.Width) / 2;
            }
            else if (child.HorizontalAlignment == HorizontalAlignment.Right)
            {
                double parentRight = parent.OffsetX + BaseUtil.GetDoubleValue(parentSize.Width) * (1 - parent.Pivot.X);
                centerX = parentRight - parent.Padding.Right - child.Margin.Right - BaseUtil.GetDoubleValue(childSize.Width) / 2;
            }
            else
            {
                centerX = parent.OffsetX - BaseUtil.GetDoubleValue(parentSize.Width) * parent.Pivot.X + BaseUtil.GetDoubleValue(parentSize.Width) / 2;
                if (child.Margin.Left != 0)
                {
                    centerX = x + child.Margin.Left + BaseUtil.GetDoubleValue(childSize.Width) / 2;
                }
            }
            return new DiagramPoint() { X = centerX, Y = y + BaseUtil.GetDoubleValue(childSize.Height) / 2 };
        }
        /// <summary>
        /// Creates a new element that is a copy of the current element.
        /// </summary>
        /// <returns>StackPanel</returns>
        public override object Clone()
        {
            return new StackPanel(this);
        }
    }
}

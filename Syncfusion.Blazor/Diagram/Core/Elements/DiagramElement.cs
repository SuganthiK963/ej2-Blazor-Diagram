using Syncfusion.Blazor.Diagram.Internal;
using System;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// The basic UI building blocks in a diagram node or connector are diagram elements. To create a node or connector, multiple DiagramElements can be combined.
    /// </summary>
    /// <remarks>
    /// A diagram element is responsible for sizing and positioning all nodes and connectors. For a node, it has more path element and text elements to render. (path element and text element are inherited from diagram element).
    /// </remarks>
    public class DiagramElement : ICommonElement
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DiagramElement"/> from the given <see cref="DiagramElement"/>.
        /// </summary>
        /// <param name="src">basic unit of diagram.</param>
        public DiagramElement(DiagramElement src)
        {
            if (src != null)
            {
                Float = src.Float;
                PreventContainer = src.PreventContainer;
                OffsetX = src.OffsetX;
                OffsetY = src.OffsetY;
                InversedAlignment = src.InversedAlignment;
                IsExport = src.IsExport;
                ID = src.ID;
                Visible = src.Visible;
                CornerRadius = src.CornerRadius;
                MinHeight = src.MinHeight;
                MinWidth = src.MinWidth;
                MaxWidth = src.MaxWidth;
                MaxHeight = src.MaxHeight;
                Width = src.Width;
                Height = src.Height;
                RotationAngle = src.RotationAngle;
                if (src.Margin != null)
                {
                    Margin = src.Margin.Clone() as Margin;
                }
                HorizontalAlignment = src.HorizontalAlignment;
                VerticalAlignment = src.VerticalAlignment;
                Flip = src.Flip;
                RelativeMode = src.RelativeMode;
                Transform = src.Transform;
                if (src.Style != null)
                {
                    Style = src.Style.Clone() as ShapeStyle;
                }
                ParentID = src.ParentID;
                DesiredSize = src.DesiredSize;
                ActualSize = src.ActualSize;
                ParentTransform = src.ParentTransform;
                Description = src.Description;
                StaticSize = src.StaticSize;
                IsRectElement = src.IsRectElement;
                if (src.Shadow != null)
                {
                    Shadow = src.Shadow.Clone() as Shadow;
                }
                if (src.Bounds != null)
                {
                    Bounds = src.Bounds.Clone() as DiagramRect;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramElement"/>.
        /// </summary>
        public DiagramElement()
        {
            ID = BaseUtil.RandomId();
        }
        /// <summary>
        /// Measures the minimum space that the element requires
        /// </summary>
        internal override DiagramSize Measure(DiagramSize availableSize)
        {
            double? width = this.Width ?? BaseUtil.GetDoubleValue(availableSize.Width) - this.Margin.Left - this.Margin.Right;
            double? height = this.Height ?? BaseUtil.GetDoubleValue(availableSize.Height) - this.Margin.Top - this.Margin.Bottom;
            this.DesiredSize = new DiagramSize() { Width = width, Height = height };
            if (this.IsCalculateDesiredSize)
            {
                this.DesiredSize = this.ValidateDesiredSize(this.DesiredSize, availableSize);
            }
            return this.DesiredSize;
        }
        /// <summary>
        /// Arranges the element
        /// </summary>
        internal override DiagramSize Arrange(DiagramSize desiredSize, bool? isStack)
        {
            this.ActualSize = desiredSize;
            this.UpdateBounds();
            return this.ActualSize;
        }

        /// <summary>
        /// Updates the bounds of the element
        /// </summary>
        internal void UpdateBounds()
        {
            this.Bounds = BaseUtil.GetBounds(this);
        }

        /// <summary>
        /// Validates the size of the element with respect to its minimum and maximum size
        /// </summary>
        internal DiagramSize ValidateDesiredSize(DiagramSize desiredSize, DiagramSize availableSize)
        {
            //Empty canvas
            if (desiredSize != null && this.IsRectElement && this.Width == null && this.MinWidth == null && this.MaxWidth == null)
            {
                desiredSize.Width = 50;
            }
            if (desiredSize != null && this.IsRectElement && this.Height == null && this.MinHeight == null && this.MaxHeight == null)
            {
                desiredSize.Height = 50;
            }
            if (availableSize != null && (desiredSize == null || this.Width != null && this.Height != null))
            {
                desiredSize ??= new DiagramSize();
                desiredSize.Width = this.Width ?? (availableSize.Width != 0 ? availableSize.Width : 0)
                    - this.Margin.Left - this.Margin.Right;
                desiredSize.Height = this.Height ?? (availableSize.Height != 0 ? availableSize.Height : 0)
                    - this.Margin.Top - this.Margin.Bottom;
            }

            //Considering min values
            if (desiredSize != null && this.MinWidth != null)
            {
                desiredSize.Width = Math.Max(BaseUtil.GetDoubleValue(desiredSize.Width), BaseUtil.GetDoubleValue(this.MinWidth));
            }
            if (desiredSize != null && this.MinHeight != null)
            {
                desiredSize.Height = Math.Max(BaseUtil.GetDoubleValue(desiredSize.Height), BaseUtil.GetDoubleValue(this.MinHeight));
            }

            //Considering max values
            if (desiredSize != null && this.MaxWidth != null)
            {
                desiredSize.Width = Math.Min(BaseUtil.GetDoubleValue(desiredSize.Width), BaseUtil.GetDoubleValue(this.MaxWidth));
            }
            if (desiredSize != null && this.MaxHeight != null)
            {
                desiredSize.Height = Math.Min(BaseUtil.GetDoubleValue(desiredSize.Height), BaseUtil.GetDoubleValue(this.MaxHeight));
            }
            return desiredSize;
        }
        /// <summary>
        /// Creates a new element that is a copy of the current element.
        /// </summary>
        /// <returns>DiagramElement</returns>
        public virtual object Clone()
        {
            return new DiagramElement(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
        }

    }
}
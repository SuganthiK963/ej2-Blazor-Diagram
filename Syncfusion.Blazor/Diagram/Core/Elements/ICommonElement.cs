
using Syncfusion.Blazor.Diagram.Internal;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// The basic UI building blocks in a diagram node or connector are elements. To create a node or connector, multiple elements can be combined
    /// </summary>
    /// <remarks>
    /// A Element is responsible for sizing and positioning of all nodes and connectors. For a node, it has more PathElement and TextElement to render
    /// </remarks>
    public abstract class ICommonElement
    {
        private DiagramPoint position;
        private UnitMode? unitModeValue;
        private DiagramRect floatingBounds;
        internal bool Float { get; set; }
        internal bool PreventContainer { get; set; }

        /// <summary>
        /// Gets or sets the corners of the rectangular bounds.
        /// </summary>
        internal Corners Corners { get; set; }

        internal bool IsCalculateDesiredSize { get; set; } = true;
        /// <summary>
        /// Gets or sets the offset values for container in flipping.
        /// </summary>
        internal DiagramPoint FlipOffset { get; set; } = new DiagramPoint() { X = 0, Y = 0 };

        /// <summary>
        /// Defines whether the element is group or port
        /// </summary>
        internal ElementAction ElementActions { get; set; } = ElementAction.None;

        internal bool InversedAlignment { get; set; } = true;
        /// <summary>
        /// Set to true during print and export
        /// </summary>
        internal bool IsExport { get; set; }

        /// <summary>
        /// Set scaling value for print and export
        /// </summary>
        internal DiagramPoint ExportScaleValue { get; set; } = new DiagramPoint { X = 0, Y = 0 };

        /// <summary>
        /// Set scaling value for print and export
        /// </summary>
        internal DiagramPoint ExportScaleOffset = new DiagramPoint() { X = 0, Y = 0 };

        /// <summary>
        /// Check whether style need to be apply or not
        /// </summary>
        internal bool CanApplyStyle { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the element has to be transformed based on its parent or not.
        /// </summary>
        internal Transform Transform { get; set; } = Transform.Self | Transform.Parent;

        /// <summary>
        /// Gets or sets the mirror image of diagram element in both horizontal and vertical directions.
        /// </summary>
        internal FlipDirection Flip { get; set; } = FlipDirection.None;

        /// <summary>
        /// Gets or sets the rotating angle that is set to the immediate parent of the element.
        /// </summary>
        internal double ParentTransform { get; set; }

        /// <summary>
        /// Defines the description of the element
        /// </summary>
        internal string Description { get; set; } = string.Empty;

        /// <summary>
        /// Defines whether the element has to be measured or not
        /// </summary>
        internal bool StaticSize { get; set; }

        /// <summary>
        /// Check whether the element is rect or not.
        /// </summary>
        internal bool IsRectElement { get; set; }

        /// <summary>
        /// Gets or sets the unique id of the element
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The element’s rotation angle will be based on pivot values, which range from 0 to 1 like offset values. By default, the Pivot values are set to X= 0.5 and Y=0.5.
        /// </summary>
        public DiagramPoint Pivot { get; set; } = new DiagramPoint() { X = 0.5, Y = 0.5 };

        /// <summary>
        /// Gets or sets whether the content of the element needs to be measured or not. If it is false, the element will not measure unnecessary scenarios
        /// </summary>
        protected bool IsDirt { get; set; } = true;
        /// <summary>
        /// Represents whether the content of the element is visible or not..
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the X-coordinate of the element. By default, it is 0. 
        /// </summary>
        public double OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the element. By default, it is 0.
        /// </summary>
        public double OffsetY { get; set; }

        /// <summary>
        /// Gets or sets a value that represents the degree to which the corners of a border are rounded. It is only applicable to Element. 
        /// </summary>
        public double CornerRadius { get; set; }

        /// <summary>
        /// Gets or sets the minimum height of the element. By default, it is undefined. 
        /// </summary>
        public double? MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum width of the element. By default, it is undefined. 
        /// </summary>
        public double? MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the element. By default, it is undefined. 
        /// </summary>
        public double? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum height of the element. By default, it is undefined.
        /// </summary>
        public double? MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the width of the element. If it is not specified, the element renders based on the content's width
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the element.
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Gets or sets the rotate angle of the element.
        /// </summary>
        public double RotationAngle { get; set; }

        /// <summary>
        /// Gets or sets the extra space around the outer boundaries of the element
        /// </summary>
        public Margin Margin { get; set; } = new Margin { Left = 0, Right = 0, Top = 0, Bottom = 0 };

        /// <summary>
        /// Gets or sets the horizontal alignment of the elements arranged to its immediate parent.
        /// Specifies how a Element in a control is horizontally aligned with respect to its parent element
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Auto;

        /// <summary>
        /// Gets or sets the vertical alignment of the element.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Auto;

        /// <summary>
        /// Gets or sets whether the element has to be aligned based on the offset values or its immediate parent
        /// </summary>
        public RelativeMode RelativeMode { get; set; } = RelativeMode.Point;

        /// <summary>
        /// Represents the appearance of the element.
        /// </summary>
        public ShapeStyle Style { get; set; } = new ShapeStyle() { Fill = "white", StrokeColor = "black", Opacity = 1, StrokeWidth = 1 };

        /// <summary>
        /// Gets or sets the parent id of the element
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// After taking into consideration the constraints gets or sets the exact size of which the element should be rendered
        /// </summary>
        public DiagramSize DesiredSize { get; set; } = new DiagramSize();

        /// <summary>
        /// Gets or sets the actual size of the Element that will be rendered
        /// </summary>
        public DiagramSize ActualSize { get; set; } = new DiagramSize();

        /// <summary>
        /// Gets the size and location of the element in pixels, including its nonclient elements  relative to the parent control.
        /// </summary>
        public DiagramRect Bounds { get; set; } = new DiagramRect();

        /// <summary>
        /// Gets or sets the shadow appearance of a node. 
        /// </summary>
        public Shadow Shadow { get; set; }

        /// <summary>
        /// Gets the element's outside bounds. It will consider all  the margin and padding properties as well.
        /// </summary>
        public DiagramRect OuterBounds
        {
            get
            {
                if (this.floatingBounds != null)
                {
                    return this.floatingBounds;
                }
                else { return this.Bounds; }
            }
            set
            {
                if (floatingBounds != value)
                {
                    this.floatingBounds = value;
                }
            }
        }

        internal virtual DiagramSize Measure(DiagramSize availableSize)
        {
            return availableSize;
        }
        internal virtual DiagramSize Arrange(DiagramSize desiredSize, bool? isStack)
        {
            return desiredSize;
        }
        /// <summary>
        /// Sets the offset of the element with respect to its parent.
        /// </summary>
        internal void SetOffsetWithRespectToBounds(double x, double y, UnitMode mode)
        {
            unitModeValue = mode;
            position = new DiagramPoint { X = x, Y = y };
        }
        /// <summary>
        /// Gets the position of the element with respect to its parent.
        /// </summary>
        internal virtual DiagramPoint GetAbsolutePosition(DiagramSize size)
        {
            if (this.position != null)
            {
                if (this.unitModeValue == UnitMode.Absolute)
                {
                    return this.position;
                }
                else
                {
                    return new DiagramPoint(this.position.X * BaseUtil.GetDoubleValue(size.Width), this.position.Y * BaseUtil.GetDoubleValue(size.Height));
                }
            }
            return null;
        }

        internal virtual void Dispose()
        {
            if (position != null)
            {
                position.Dispose();
                position = null;
            }

            if (unitModeValue != null)
            {
                unitModeValue = null;
            }

            if (floatingBounds != null)
            {
                floatingBounds.Dispose();
                floatingBounds = null;
            }

            if (Corners != null)
            {
                Corners.Dispose();
                Corners = null;
            }

            if (FlipOffset != null)
            {
                FlipOffset.Dispose();
                FlipOffset = null;
            }

            if (ExportScaleValue != null)
            {
                ExportScaleValue.Dispose();
                ExportScaleValue = null;
            }

            if (ExportScaleOffset != null)
            {
                ExportScaleOffset.Dispose();
                ExportScaleOffset = null;
            }

            if (Pivot != null)
            {
                Pivot.Dispose();
                Pivot = null;
            }

            MinHeight = null;
            MinWidth = null;
            MaxWidth = null;
            MaxHeight = null;
            Width = null;
            Height = null;

            if (Margin != null)
            {
                Margin.Dispose();
                Margin = null;
            }

            if (Style != null)
            {
                Style.Dispose();
                Style = null;
            }

            if (DesiredSize != null)
            {
                DesiredSize.Dispose();
                DesiredSize = null;
            }

            if (ActualSize != null)
            {
                ActualSize.Dispose();
                ActualSize = null;
            }

            if (Bounds != null)
            {
                Bounds.Dispose();
                Bounds = null;
            }

            if (Shadow != null)
            {
                Shadow.Dispose();
                Shadow = null;
            }

            ID = null;
            ParentID = null;
            Description = null;
        }
    }


    /// <summary>
    /// Define the Corners class.
    /// </summary>
    internal class Corners
    {
        /// <summary>
        /// Gets or sets the top left point of canvas corner
        /// </summary>
        internal DiagramPoint TopLeft { get; set; }
        /// <summary>
        /// Gets or sets the top center point of canvas corner
        /// </summary>
        internal DiagramPoint TopCenter { get; set; }
        /// <summary>
        /// Gets or sets the top right point of canvas corner.
        /// </summary>
        internal DiagramPoint TopRight { get; set; }
        /// <summary>
        /// Gets or sets the middle left point of canvas corner.
        /// </summary>
        internal DiagramPoint MiddleLeft { get; set; }
        /// <summary>
        /// Gets or sets the center point of canvas corner.
        /// </summary>
        internal DiagramPoint Center { get; set; }
        /// <summary>
        /// Gets or sets the middle left point of canvas corner.
        /// </summary>
        internal DiagramPoint MiddleRight { get; set; }
        /// <summary>
        /// Gets or sets the bottom left point of canvas corner.
        /// </summary>
        internal DiagramPoint BottomLeft { get; set; }
        /// <summary>
        /// Gets or sets the bottom center point of canvas corner.
        /// </summary>
        internal DiagramPoint BottomCenter { get; set; }
        /// <summary>
        /// Gets or sets the bottom right point of canvas corner.
        /// </summary>
        internal DiagramPoint BottomRight { get; set; }
        /// <summary>
        /// Gets or sets the left position of canvas corner.
        /// </summary>
        public double Left { get; set; }
        /// <summary>
        /// Gets or sets the right position of canvas corner.
        /// </summary>
        public double Right { get; set; }
        /// <summary>
        /// Gets or sets the top position of canvas corner.
        /// </summary>
        public double Top { get; set; }
        /// <summary>
        /// Gets or sets the bottom position of canvas corner.
        /// </summary>
        public double Bottom { get; set; }
        /// <summary>
        /// Gets or sets the width of canvas.
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Gets or sets the height of the element. By default, it is 50
        /// </summary>
        public double Height { get; set; }

        internal void Dispose()
        {
            if (TopLeft != null)
            {
                TopLeft.Dispose();
                TopLeft = null;
            }
            if (TopCenter != null)
            {
                TopCenter.Dispose();
                TopCenter = null;
            }
            if (TopRight != null)
            {
                TopRight.Dispose();
                TopRight = null;
            }
            if (BottomLeft != null)
            {
                BottomLeft.Dispose();
                BottomLeft = null;
            }
            if (BottomCenter != null)
            {
                BottomCenter.Dispose();
                BottomCenter = null;
            }
            if (BottomRight != null)
            {
                BottomRight.Dispose();
                BottomRight = null;
            }
        }
    }

}

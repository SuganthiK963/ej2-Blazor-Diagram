using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents how to execute the commonly or frequently used commands around the nodes, connectors and groups. 
    /// </summary>
    public class UserHandle : DiagramObject
    {
        private string id;
        private string pathData = string.Empty;
        private string imageSource;
        private string backgroundColor = "black";
        private Direction side = Direction.Top;
        private double borderWidth = 0.5;
        private string borderColor = string.Empty;
        private double size = 25;
        private string pathColor = "white";
        private double displacement = 10;
        private bool visible = true;
        private double offset;
        private Margin margin = new Margin();
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Center;
        private bool template;
        internal DiagramRect Bounds;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserHandle"/>.
        /// </summary>
        public UserHandle() : base()
        {
            id = BaseUtil.RandomId();
        }

        /// <summary>
        /// Gets or sets the name of the user handle.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique id of the diagram object. 
        /// </summary>
        [JsonPropertyName("id")]
        public string ID
        {
            get => id;
            set
            {
                if (!string.Equals(id, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(ID), value, id, this);
                    id = value;
                }
            }
        }

        /// <summary>
        /// Represents the custom geometry(shape) of the user handle.
        /// </summary>
        [JsonPropertyName("pathData")]
        public string PathData
        {
            get => this.pathData;
            set
            {
                if (!string.Equals(pathData, value, StringComparison.Ordinal))
                {
                    this.pathData = value;
                    Parent?.OnPropertyChanged(nameof(PathData), value, pathData, this as IDiagramObject);
                }
            }

        }
        /// <summary>
        /// Gets or sets the image source of the user handle. Applicable only if it is an image. 
        /// </summary>
        [JsonPropertyName("source")]
        public string Source
        {
            get => this.imageSource;
            set
            {
                if (!string.Equals(imageSource, value, StringComparison.Ordinal))
                {
                    this.imageSource = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the background color of the user handle.
        /// </summary>
        [JsonPropertyName("backgroundColor")]
        public string BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (!string.Equals(backgroundColor, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(BackgroundColor), value, backgroundColor, this as IDiagramObject);
                    backgroundColor = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the position of user Handle
        /// </summary>
        [JsonPropertyName("side")]
        public Direction Side
        {
            get => side;
            set
            {
                if (!Equals(side, value))
                {
                    Parent?.OnPropertyChanged(nameof(Side), value, side, this as IDiagramObject);
                    side = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets the border color of the user handle. 
        /// </summary>
        [JsonPropertyName("borderColor")]
        public string BorderColor
        {
            get => borderColor;
            set
            {
                if (!string.Equals(borderColor, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(BorderColor), value, borderColor, this);
                    borderColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the border width of the user handle. 
        /// </summary>
        [JsonPropertyName("borderWidth")]
        public double BorderWidth
        {
            get => borderWidth;
            set
            {
                if (!borderWidth.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(BorderWidth), value, borderWidth, this);
                    borderWidth = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the user handle.
        /// </summary>
        [JsonPropertyName("size")]
        public double Size
        {
            get => size;
            set
            {
                if (!size.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Size), value, size, this as IDiagramObject);
                    size = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the stroke color of the shape.
        /// </summary>
        [JsonPropertyName("pathColor")]
        public string PathColor
        {
            get => pathColor;
            set
            {
                if (!string.Equals(pathColor, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(PathColor), value, pathColor, this);
                    pathColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the displacement(margin) of the user handle. Applicable only if the parent is a connector. 
        /// </summary>
        [JsonPropertyName("displacement")]
        public double Displacement
        {
            get => displacement;
            set
            {
                if (!displacement.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Displacement), value, displacement, this);
                    displacement = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the user handle.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible
        {
            get => visible;
            set
            {
                if (!visible.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Visible), value, visible, this);
                    visible = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the X and Y coordinates of the user handle, by default it is 0,0.
        /// </summary>
        [JsonPropertyName("offset")]
        public double Offset
        {
            get => offset;
            set
            {
                if (!offset.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Offset), value, offset, this);
                    offset = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space around the outer boundaries of the user handle. Applicable only if the parent is a node.
        /// </summary>
        [JsonPropertyName("margin")]
        public Margin Margin
        {
            get
            {
                if (margin != null && margin.Parent == null)
                    margin.SetParent(this, nameof(Margin));
                return margin;
            }
            set
            {
                if (!Equals(margin, value))
                {
                    Parent?.OnPropertyChanged(nameof(Margin), value, margin, this);
                    margin = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the user handle. 
        /// </summary>
        [JsonPropertyName("horizontalAlignment")]
        public HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment != value)
                {
                    Parent?.OnPropertyChanged(nameof(HorizontalAlignment), value, horizontalAlignment, this);
                    horizontalAlignment = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of the user handle.
        /// </summary>
        [JsonPropertyName("verticalAlignment")]
        public VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (!Equals(verticalAlignment, value))
                {
                    Parent?.OnPropertyChanged(nameof(VerticalAlignment), value, verticalAlignment, this);
                    verticalAlignment = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the geometry of the html element.
        /// </summary>
        [JsonPropertyName("template")]
        public bool Template
        {
            get => template;
            set
            {
                if (!template.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Template), value, template, this);
                    template = value;
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UserHandle"/> from the given UserHandle.
        /// </summary>
        /// <param name="src">UserHandle.</param>
        public UserHandle(UserHandle src)
        {
            if (src != null)
            {
                PathData = src.PathData;
                Template = src.Template;
                VerticalAlignment = src.VerticalAlignment;
                HorizontalAlignment = src.HorizontalAlignment;
                Margin = src.Margin;
                Offset = src.Offset;
                Visible = src.Visible;
                Displacement = src.Displacement;
                Side = src.Side;
                PathColor = src.PathColor;
                Size = src.Size;
                BorderWidth = src.BorderWidth;
                BorderColor = src.BorderColor;
                BackgroundColor = src.BackgroundColor;
                Source = src.Source;
                Name = src.Name;
            }
        }
        /// <summary>
        /// Creates a new user handle that is a copy of the current user handle.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new UserHandle(this);
        }

        internal override void Dispose()
        {
            if (pathData != null)
            {
                pathData = null;
            }
            if (id != null)
            {
                id = null;
            }
            if (imageSource != null)
            {
                imageSource = null;
            }
            if (backgroundColor != null)
            {
                backgroundColor = null;
            }
            if (borderColor != null)
            {
                borderColor = null;
            }
            if (pathColor != null)
            {
                pathColor = null;
            }
            if (margin != null)
            {
                margin.Dispose();
                margin = null;
            }
            if (Bounds != null)
            {
                Bounds.Dispose();
                Bounds = null;
            }
        }
    }
}
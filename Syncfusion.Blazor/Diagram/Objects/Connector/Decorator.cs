using System;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the shape of the end points of the connector.
    /// </summary>
    public class DecoratorSettings : DiagramObject
    {
        private double width = 10;
        private double height = 10;
        private DecoratorShape shape = DecoratorShape.Arrow;
        private string pathData = string.Empty;
        private DiagramPoint pivot = new DiagramPoint() { X = 0, Y = 0.5 };
        private ShapeStyle style = new ShapeStyle() { Fill = "black", StrokeColor = "black", StrokeWidth = 1 };
        /// <summary>
        /// Creates a new instance of the Decorator from the given Decorator.
        /// </summary>
        /// <param name="src">Decorator</param>
        public DecoratorSettings(DecoratorSettings src) : base(src)
        {
            if (src != null)
            {
                width = src.width;
                height = src.height;
                pathData = src.pathData;
                if (src.pivot != null)
                {
                    pivot = src.pivot.Clone() as DiagramPoint;
                }
                if (src.style != null)
                {
                    style = src.style.Clone() as ShapeStyle;
                }
                shape = src.shape;
            }
        }
        /// <summary>
        /// Initializes a new instance of the Decorator.
        /// </summary>
        public DecoratorSettings()
        {

        }
        /// <summary>
        /// Gets or sets the width of the decorator. By default, it is 10.
        /// </summary>
        [JsonPropertyName("width")]
        public double Width
        {
            get => width;
            set
            {
                if (!width.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Width), value, width, this);
                    width = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the decorator. By default, it is 10.
        /// </summary>
        [JsonPropertyName("height")]
        public double Height
        {
            get => height;
            set
            {
                if (!height.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Height), value, height, this);
                    height = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the shape of the decorator. By default, it is an arrow.
        /// </summary>
        [JsonPropertyName("shape")]
        public DecoratorShape Shape
        {
            get => shape;
            set
            {
                if (!Equals(shape, value))
                {
                    Parent?.OnPropertyChanged(nameof(Shape), value, shape, this);
                    shape = value;
                }
            }
        }

        /// <summary>
        /// Allows setting a custom shape of the decorator.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// TargetDecorator = new DecoratorSettings()
        /// {
        ///     Shape = DecoratorShape.Custom,
        ///     PathData = "M80.5,12.5 C80.5,19.127417 62.59139,24.5 40.5,24.5 C18.40861,24.5 0.5,19.127417 0.5,12.5 C0.5,5.872583 18.40861,0.5 40.5,0.5 C62.59139,0.5 80.5,5.872583 80.5,12.5 z",
        ///     Style = new ShapeStyle()
        ///     {
        ///         StrokeColor = "#37909A",
        ///         Fill = "#37909A",
        ///         StrokeWidth = 1,
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        [JsonPropertyName("pathData")]
        public string PathData
        {
            get => pathData;
            set
            {
                if (!string.Equals(pathData, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(PathData), value, pathData, this);
                    pathData = value;
                }
            }
        }

        /// <summary>
        /// The decorator angle will be based on the pivot values, which range from 0 to 1.
        /// </summary>
        [JsonPropertyName("pivot")]
        public DiagramPoint Pivot
        {
            get
            {
                if (pivot != null && pivot.Parent == null)
                    pivot.SetParent(this, nameof(Pivot));
                return pivot;
            }
            set
            {
                if (!Equals(pivot, value))
                {
                    Parent?.OnPropertyChanged(nameof(Pivot), value, pivot, this);
                    pivot = value;
                }
            }
        }

        /// <summary>
        /// Represents the appearance of the decorator.
        /// </summary>
        [JsonPropertyName("style")]
        public ShapeStyle Style
        {
            get
            {
                if (style != null && style.Parent == null)
                    style.SetParent(this, nameof(Style));
                return style;
            }
            set
            {
                if (!style.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Style), value, style, this);
                    style = value;
                }
            }
        }
        /// <summary>
        /// Creates a new decorator that is a copy of the current decorator.
        /// </summary>
        /// <returns>Decorator</returns>
        public override object Clone()
        {
            return new DecoratorSettings(this);
        }

        internal override void Dispose()
        {
            width = 0;
            height = 0;
            pathData = null;
            if (pivot != null)
            {
                pivot.Dispose();
                pivot = null;
            }
            if (style != null)
            {
                style.Dispose();
                style = null;
            }
        }
    }
}

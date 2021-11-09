using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Describes the thickness of a frame around a rectangle.
    /// </summary>
    public class Thickness : DiagramObject
    {
        private double left;
        private double right;
        private double top;
        private double bottom;
        /// <summary>
        /// Creates a new instance of the <see cref="TextElement"/> from the given Thickness.
        /// </summary>
        /// <param name="src">Thickness.</param>
        public Thickness(Thickness src) : base(src)
        {
            if (src != null)
            {
                left = src.left;
                right = src.right;
                top = src.top;
                bottom = src.bottom;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TextElement"/>.
        /// </summary>
        public Thickness() : base()
        {

        }
        /// <summary>
        /// Gets or sets the left value of the thickness.
        /// </summary>
        [JsonPropertyName("left")]
        public double Left
        {
            get => left;
            set
            {
                if (!left.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Left), value, left, this);
                    left = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the right value of the thickness.
        /// </summary>
        [JsonPropertyName("right")]
        public double Right
        {
            get => right;
            set
            {
                if (!right.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Right), value, right, this);
                    right = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the top value of the thickness.
        /// </summary>
        [JsonPropertyName("top")]
        public double Top
        {
            get => top;
            set
            {
                if (!top.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Top), value, top, this);
                    top = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the bottom value of the thickness.
        /// </summary>
        [JsonPropertyName("bottom")]
        public double Bottom
        {
            get => bottom;
            set
            {
                if (!bottom.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Bottom), value, bottom, this);
                    bottom = value;
                }
            }
        }
        /// <summary>
        /// Creates a new Thickness that is a copy of the current Thickness.
        /// </summary>
        /// <returns>Thickness</returns>
        public override object Clone()
        {
            return new Thickness(this);
        }

        internal override void Dispose()
        {
            left = 0;
            right = 0;
            top = 0;
            bottom = 0;
        }

    }
    /// <summary>
    /// Specifies the extra space around the outer boundaries of an element.
    /// </summary>
    public class Margin : DiagramObject
    {
        private double bottom;
        private double left;
        private double right;
        private double top;
        /// <summary>
        /// Creates a new instance of the <see cref="Margin"/> from the given Margin.
        /// </summary>
        /// <param name="src">Margin.</param>
        public Margin(Margin src) : base(src)
        {
            if (src != null)
            {
                bottom = src.bottom;
                left = src.left;
                right = src.right;
                top = src.top;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/>.
        /// </summary>
        public Margin()
        {

        }
        /// <summary>
        /// Gets or sets the extra space at the bottom of an element
        /// </summary>
        [JsonPropertyName("bottom")]
        public double Bottom
        {
            get => bottom;
            set
            {
                if (!bottom.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Bottom), value, bottom, this);
                    bottom = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space at the left of an element.
        /// </summary>
        [JsonPropertyName("left")]
        public double Left
        {
            get => left;
            set
            {
                if (!left.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Left), value, left, this);
                    left = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space at the right of an element
        /// </summary>
        [JsonPropertyName("right")]
        public double Right
        {
            get => right;
            set
            {
                if (!right.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Right), value, right, this);
                    right = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space at the top of an element
        /// </summary>
        [JsonPropertyName("top")]
        public double Top
        {
            get => top;
            set
            {
                if (!top.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Top), value, top, this);
                    top = value;
                }
            }
        }
        /// <summary>
        /// Creates a new Margin that is a copy of the current Margin.
        /// </summary>
        /// <returns>Margin</returns>
        public override object Clone()
        {
            return new Margin(this);
        }

        internal override void Dispose()
        {
            left = 0;
            right = 0;
            top = 0;
            bottom = 0;
        }
    }
    /// <summary>
    /// Represents the shadow appearance of the diagram object. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node node = new Node()
    /// {
    ///     // Position of the node
    ///     OffsetX = 250,
    ///     OffsetY = 250,
    ///     // Size of the node
    ///     Width = 100,
    ///     Height = 100,
    ///     Style = new ShapeStyle() { Fill = "#6BA5D7", StrokeColor = "white" },
    ///     Constraints = NodeConstraints.Default | NodeConstraints.Shadow,
    ///     Shadow = new Shadow()
    ///     {
    ///         Angle = 50,
    ///         Color = "Blue",
    ///         Opacity = 0.8,
    ///         Distance = 20
    ///     }
    /// };
    /// ]]>
    /// </code>
    /// </example>
    public class Shadow : DiagramObject
    {

        private double angle = 45;
        private string color = "lightgrey";
        private double distance = 5;
        private double opacity = 0.7;

        /// <summary>
        /// Creates a new instance of the Shadow from the given Shadow.
        /// </summary>
        /// <param name="src">Shadow.</param>
        public Shadow(Shadow src) : base(src)
        {
            if (src != null)
            {
                angle = src.angle;
                color = src.color;
                distance = src.distance;
                opacity = src.opacity;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shadow"/>.
        /// </summary>
        public Shadow()
        {

        }

        /// <summary>
        /// Gets or sets the angle of the shadow. By default, it is 45.
        /// </summary>
        [JsonPropertyName("angle")]
        public double Angle
        {
            get => angle;
            set
            {
                if (!angle.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Angle), value, angle, this);
                    angle = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the shadow. By default, it is light grey.
        /// </summary>
        [JsonPropertyName("color")]
        public string Color
        {
            get => color;
            set
            {
                if (color != value)
                {
                    Parent?.OnPropertyChanged(nameof(Color), value, color, this);
                    color = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance of the shadow. By default, it is 5px.
        /// </summary>
        [JsonPropertyName("distance")]
        public double Distance
        {
            get => distance;
            set
            {
                if (!distance.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Distance), value, distance, this);
                    distance = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Opacity of the shadow. The opacity value ranges from 0 to 1. By default, it is 0.7.
        /// </summary>
        [JsonPropertyName("opacity")]
        public double Opacity
        {
            get => opacity;
            set
            {
                if (!opacity.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Opacity), value, opacity, this);
                    opacity = value;
                }
            }
        }
        /// <summary>
        /// Creates a new shadow that is a copy of the current shadow. 
        /// </summary>
        /// <returns>Shadow</returns>
        public override object Clone()
        {
            return new Shadow(this);
        }

        internal override void Dispose()
        {
            color = null;
        }

    }
    /// <summary>
    /// Defines a smooth transition from one color to the next while painting the node.
    /// </summary>
    [JsonConverter(typeof(GradientJsonConverter))]
    public abstract class GradientBrush : DiagramObject
    {
        private GradientType type;
        private DiagramObjectCollection<GradientStop> stops = new DiagramObjectCollection<GradientStop>();
        /// <summary>
        /// Creates a new instance of the <see cref="GradientBrush"/> from the given Gradient.
        /// </summary>
        /// <param name="src">Gradient.</param>
        protected GradientBrush(GradientBrush src)
        {
            if (src != null)
            {
                type = src.type;
                stops = new DiagramObjectCollection<GradientStop>();
                if (src.stops.Count > 0)
                {
                    foreach (GradientStop gradient in src.stops)
                    {
                        GradientStop gradient1 = gradient.Clone() as GradientStop;
                        stops.Add(gradient1);
                    }
                }
                stops.Parent = this;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Gradient.
        /// </summary>
        protected GradientBrush() { }
        /// <summary>
        /// Gets or sets the color and the position where the previous color transition ends, and a new color transition starts.
        /// </summary>
        [JsonPropertyName("gradientStops")]
        public DiagramObjectCollection<GradientStop> GradientStops
        {
            get => stops;
            set
            {
                if (value != null && stops != value)
                {
                    stops = value;
                    stops.Parent = this;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the gradient.
        /// </summary>
        [JsonPropertyName("brushType")]
        internal GradientType BrushType
        {
            get => type;
            set
            {
                if (!type.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(BrushType), value, type, this);
                    type = value;
                }
            }
        }

        internal override void Dispose()
        {
            if (stops != null)
            {
                for (int i = 0; i < stops.Count; i++)
                {
                    stops[i].Dispose();
                    stops[i] = null;
                }
                stops.Clear();
                stops = null;
            }
        }

    }
    /// <summary>
    /// Represents the class that defines to paints the node with linear color transitions
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// Node Node = new Node()
    /// {
    ///     // Position of the node
    ///     OffsetX = 250,
    ///     OffsetY = 250,
    ///     // Size of the node
    ///     Width = 100,
    ///     Height = 100,
    ///     // Add node
    ///     Style = new ShapeStyle()
    ///     {
    ///        Gradient = new LinearGradientBrush()
    ///        {
    ///            //Start point of linear gradient
    ///            X1 = 0,
    ///            Y1 = 0,
    ///            //End point of linear gradient
    ///            X2 = 50,
    ///            Y2 = 50,
    ///            //Sets an array of stop objects
    ///            GradientStops = new DiagramObjectCollection<GradientStop>()
    ///            {
    ///                new GradientStop(){ Color = "white", Offset = 0},
    ///                new GradientStop(){ Color = "#6BA5D7", Offset = 100}
    ///            },
    ///        }
    ///     },
    ///  };
    /// ]]>
    /// </code>
    /// </example> 
    public class LinearGradientBrush : GradientBrush
    {
        private double x1;
        private double x2;
        private double y1;
        private double y2;
        /// <summary>
        /// Creates a new <see cref="LinearGradientBrush"/> from the given <see cref="LinearGradientBrush"/>.
        /// </summary>
        /// <param name="src">LinearGradient.</param>
        public LinearGradientBrush(LinearGradientBrush src) : base(src)
        {
            if (src != null)
            {
                x1 = src.x1;
                x2 = src.x2;
                y1 = src.y1;
                y2 = src.y2;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/>.
        /// </summary>
        public LinearGradientBrush() : base()
        {
            BrushType = GradientType.Linear;
        }

        /// <summary>
        /// Gets or sets the start point of the Linear gradient.
        /// </summary>
        [JsonPropertyName("x1")]
        public double X1
        {
            get => x1;
            set
            {
                if (!x1.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(X1), value, x1, this);
                    x1 = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the start point of the Linear gradient.
        /// </summary>
        [JsonPropertyName("x2")]
        public double X2
        {
            get => x2;
            set
            {
                if (!x2.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(X2), value, x2, this);
                    x2 = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the endpoint of the Linear gradient.
        /// </summary>
        [JsonPropertyName("y1")]
        public double Y1
        {
            get => y1;
            set
            {
                if (!y1.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Y1), value, y1, this);
                    y1 = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the endpoint of the Linear gradient.
        /// </summary>
        [JsonPropertyName("y2")]
        public double Y2
        {
            get => y2;
            set
            {
                if (!y2.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Y2), value, y2, this);
                    y2 = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current linear gradient.
        /// </summary>
        /// <returns>LinearGradient</returns>
        public override object Clone()
        {
            return new LinearGradientBrush(this);
        }
    }
    /// <summary>
    /// Represents the focal point that defines the beginning of the gradient and a circle that defines the endpoint of the gradient
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// Node Node = new Node()
    /// {
    ///  // Position of the node
    ///  OffsetX = 250,
    ///  OffsetY = 250,
    ///  // Size of the node
    ///  Width = 100,
    ///  Height = 100,
    ///  // Add node
    ///  Style = new ShapeStyle()
    ///  {
    ///    Gradient = new RadialGradientBrush()
    ///    {
    ///     //Center point of the inner circle
    ///     FX = 20,
    ///     FY = 20,
    ///     //Center point of the outer circle
    ///     CX = 50,
    ///     CY = 50,
    ///     //Radius of a radial gradient
    ///     R = 50,
    ///     //Set an array of stop objects
    ///     GradientStops = new DiagramObjectCollection<GradientStop>()
    ///     {
    ///       new GradientStop(){ Color = "white", Offset = 0},
    ///       new GradientStop(){ Color = "#6BA5D7", Offset = 100}
    ///     },
    ///     }
    ///   },
    /// };
    /// ]]>
    /// </code>
    /// </example>
    public class RadialGradientBrush : GradientBrush
    {
        private double cx;
        private double cy;
        private double fx;
        private double fy;
        private double r = 50;
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGradientBrush"/> class.
        /// </summary>
        /// <param name="src">RadialGradient.</param>
        public RadialGradientBrush(RadialGradientBrush src) : base(src)
        {
            if (src != null)
            {
                cx = src.cx;
                cy = src.cy;
                fx = src.fx;
                fy = src.fy;
                r = src.r;
            }
        }
        /// <summary>
        /// Creates a new instance of the <see cref="RadialGradientBrush"/> from the given RadialGradient.
        /// </summary>
        public RadialGradientBrush() : base()
        {
            BrushType = GradientType.Radial;
        }

        /// <summary>
        /// Gets or sets the center point of the outer circle of the radial gradient. 
        /// </summary>
        [JsonPropertyName("cx")]
        public double CX
        {
            get => cx;
            set
            {
                if (!cx.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(CX), value, cx, this);
                    cx = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the center point of the outer circle of the radial gradient.
        /// </summary>
        [JsonPropertyName("cy")]
        public double CY
        {
            get => cy;
            set
            {
                if (!cy.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(CY), value, cy, this);
                    cy = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the center point of the inner circle of the radial gradient. 
        /// </summary>
        [JsonPropertyName("fx")]
        public double FX
        {
            get => fx;
            set
            {
                if (!fx.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(FX), value, fx, this);
                    fx = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the center point of the inner circle of the radial gradient. 
        /// </summary>
        [JsonPropertyName("fy")]
        public double FY
        {
            get => fy;
            set
            {
                if (!fy.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(FY), value, fy, this);
                    fy = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the radius of a radial gradient
        /// </summary>
        [JsonPropertyName("r")]
        public double R
        {
            get => r;
            set
            {
                if (!r.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(R), value, r, this);
                    r = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is the a copy of the current radial gradient
        /// </summary>
        public override object Clone()
        {
            return new RadialGradientBrush(this);
        }

    }
    /// <summary>
    /// Defines the different colors and the regions of color transitions. 
    /// </summary>
    public partial class GradientStop : DiagramObject
    {
        private string color = string.Empty;
        private double? offset = 0.0;
        private double opacity = 1.0;
        /// <summary>
        /// Creates a new instance of the GradientStop from the given GradientStop.
        /// </summary>
        /// <param name="src">GradientStop.</param>
        public GradientStop(GradientStop src) : base(src)
        {
            if (src != null)
            {
                color = src.color;
                offset = src.offset;
                opacity = src.opacity;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GradientStop"/>.
        /// </summary>
        public GradientStop()
        {

        }

        /// <summary>
        /// Gets or sets the color to be filled over the specified region.
        /// </summary>
        [JsonPropertyName("color")]
        public string Color
        {
            get => color;
            set
            {
                if (!string.Equals(color, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(Color), value, color, this);
                    color = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the position at which the previous color transition ends, and a new color transition starts.
        /// </summary>
        [JsonPropertyName("offset")]
        public double? Offset
        {
            get => offset;
            set
            {
                if (!Equals(offset, value))
                {
                    Parent?.OnPropertyChanged(nameof(Offset), value, offset, this);
                    offset = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the transparency level of the region.
        /// </summary>
        [JsonPropertyName("opacity")]
        public double Opacity
        {
            get => opacity;
            set
            {
                if (!opacity.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Opacity), value, opacity, this);
                    opacity = value;
                }
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current gradient stop.
        /// </summary>
        /// <returns>GradientStop</returns>
        public override object Clone()
        {
            return new GradientStop(this);
        }

        internal override void Dispose()
        {
            color = null;
            offset = null;
        }
    }

    /// <summary>
    /// Represents the appearance of the text.
    /// </summary>
    public class TextStyle : ShapeStyle
    {

        private bool bold;
        private string color = "black";
        private string fontFamily = "Arial";
        private double fontSize = 12.0;
        private bool italic;
        private TextAlign textAlign = TextAlign.Center;
        private TextDecoration textDecoration = TextDecoration.None;
        private TextOverflow textOverflow = TextOverflow.Wrap;
        private TextWrap textWrapping = TextWrap.WrapWithOverflow;
        private WhiteSpace whiteSpace = WhiteSpace.CollapseSpace;
        /// <summary>
        /// Initializes a new instance of the <see cref="TextStyle"/>.
        /// </summary>
        public TextStyle()
        {
            Fill = "transparent";
        }
        /// <summary>
        /// Creates a new instance of the <see cref="TextStyle"/> from the given <see cref="TextStyle"/>.
        /// </summary>
        /// <param name="src">TextShapeStyle.</param>
        public TextStyle(TextStyle src) : base(src)
        {
            if (src != null)
            {
                bold = src.bold;
                color = src.color;
                fontFamily = src.fontFamily;
                fontSize = src.fontSize;
                italic = src.italic;
                textAlign = src.textAlign;
                textDecoration = src.textDecoration;
                textOverflow = src.textOverflow;
                textWrapping = src.textWrapping;
                whiteSpace = src.whiteSpace;
            }
        }

        /// <summary>
        /// Enables or disables the bold style of a text. By default, it is false.
        /// </summary>
        [JsonPropertyName("bold")]
        public bool Bold
        {
            get => bold;
            set
            {
                if (!bold.Equals(value))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Bold), value, bold, this);
                    }
                    bold = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the font color of a text. By default, the font color is black. 
        /// </summary>
        [JsonPropertyName("color")]
        public string Color
        {
            get => color;
            set
            {
                if (!string.Equals(color, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(Color), value, color, this);
                    color = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the font type of a text.  By default, the font family is Arial. 
        /// </summary>
        [JsonPropertyName("fontFamily")]
        public string FontFamily
        {
            get => fontFamily;
            set
            {
                if (!string.Equals(fontFamily, value, StringComparison.Ordinal))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(FontFamily), value, fontFamily, this);
                    }
                    fontFamily = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the font size of a text. By default, the font size is 12px. 
        /// </summary>
        [JsonPropertyName("fontSize")]
        public double FontSize
        {
            get => fontSize;
            set
            {
                if (!fontSize.Equals(value))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(FontSize), value, fontSize, this);
                    }
                    fontSize = value;
                }
            }
        }

        /// <summary>
        /// Enables or disables the italic style of a text. By default, it is false. 
        /// </summary>
        [JsonPropertyName("italic")]
        public bool Italic
        {
            get => italic;
            set
            {
                if (!italic.Equals(value))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Italic), value, italic, this);
                    }
                    italic = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text inside the node bounds. By default, it is aligned at the center. 
        /// </summary>
        [JsonPropertyName("textAlign")]
        public TextAlign TextAlign
        {
            get => textAlign;
            set
            {
                if (!textAlign.Equals(value))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(TextAlign), value, textAlign, this);
                    }
                    textAlign = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the TextDecoration which contains the effects that should be applied to the text of a TextBlock. The default value is none. 
        /// </summary>
        [JsonPropertyName("textDecoration")]
        public TextDecoration TextDecoration
        {
            get => textDecoration;
            set
            {
                if (!Equals(textDecoration, value))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(TextDecoration), value, textDecoration, this);
                    }
                    textDecoration = value;
                }
            }
        }

        /// <summary>
        /// Specifies a value that indicates whether to render ellipses (...) to indicate text overflow. By default, it is wrapped.
        /// </summary>
        [JsonPropertyName("textOverflow")]
        public TextOverflow TextOverflow
        {
            get => textOverflow;
            set
            {
                if (!Equals(textOverflow, value))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(TextOverflow), value, textWrapping, this);
                    }
                    textOverflow = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a TextWrap to wrap the text. By default, it is WrapWithOverflow. 
        /// </summary>
        [JsonPropertyName("textWrapping")]
        public TextWrap TextWrapping
        {
            get => textWrapping;
            set
            {
                if (!Equals(textWrapping, value))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(TextWrapping), value, textWrapping, this);
                    }
                    textWrapping = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets how the white space and the new line characters  be handled. By default, it is CollapseSpace. 
        /// </summary>
        [JsonPropertyName("whiteSpace")]
        internal WhiteSpace WhiteSpace
        {
            get => whiteSpace;
            set
            {
                if (!Equals(whiteSpace, value))
                {
                    Parent?.OnPropertyChanged(nameof(WhiteSpace), value, whiteSpace, this);
                    whiteSpace = value;
                }
            }
        }
        /// <summary>
        /// Creates a new <see cref="TextStyle"/> that is a copy of the current style.
        /// </summary>
        /// <returns>TextShapeStyle</returns>
        public override object Clone()
        {
            return new TextStyle(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            color = null;
            fontFamily = null;

        }

    }
    /// <summary>
    /// Represents the appearance of a shape/path.
    /// </summary>
    public class ShapeStyle : DiagramObject
    {

        private string fill = "white";
        private GradientBrush gradient;
        private double opacity = 1.0;
        private string strokeColor = "black";
        private string strokeDashArray = string.Empty;
        private double strokeWidth = 1.0;
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeStyle"/>.
        /// </summary>
        public ShapeStyle()
        {

        }
        /// <summary>
        /// Creates a new instance of the <see cref="ShapeStyle"/> from the given ShapeStyle.
        /// </summary>
        /// <param name="src">ShapeStyle.</param>
        public ShapeStyle(ShapeStyle src) : base(src)
        {
            if (src != null)
            {
                fill = src.fill;
                opacity = src.opacity;
                strokeColor = src.strokeColor;
                strokeDashArray = src.strokeDashArray;
                strokeWidth = src.StrokeWidth;
                if (src.gradient != null)
                {
                    gradient = src.gradient.Clone() as GradientBrush;
                }
            }
        }

        /// <summary>
        /// Gets or sets the fill color of the shape or path. By default, it is white.
        /// </summary>
        [JsonPropertyName("fill")]
        public string Fill
        {
            get => fill;
            set
            {
                if (!string.Equals(fill, value, StringComparison.Ordinal))
                {
                    if (Parent != null)
                    {
                        if (Parent is Annotation annotation)
                            annotation.IsDirtAnnotation = true;
                        Parent.OnPropertyChanged(nameof(Fill), value, fill, this);
                    }
                    fill = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the gradient effect of the diagram elements.
        /// </summary>
        [JsonPropertyName("gradient")]
        public GradientBrush Gradient
        {
            get
            {
                if (gradient != null && gradient.Parent == null)
                    gradient.SetParent(this, nameof(Gradient));
                return gradient;
            }
            set
            {
                if (!Equals(gradient, value))
                {
                    Parent?.OnPropertyChanged(nameof(Gradient), value, gradient, this);
                    gradient = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the transparency level of the diagram elements. By default, opacity is 1px.
        /// </summary>
        [JsonPropertyName("opacity")]
        public double Opacity
        {
            get => opacity;
            set
            {
                if (!opacity.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(Opacity), value, opacity, this);
                    opacity = value;
                }
            }
        }

        /// <summary>
        ///  Gets or sets the stroke color of the diagram elements. By default, it is black.
        /// </summary>
        [JsonPropertyName("strokeColor")]
        public string StrokeColor
        {
            get => strokeColor;
            set
            {
                if (!string.Equals(strokeColor, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(StrokeColor), value, strokeColor, this);
                    strokeColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the pattern of dashes and space to the stroke of the diagram elements.
        /// </summary>
        [JsonPropertyName("strokeDashArray")]
        public string StrokeDashArray
        {
            get => strokeDashArray;
            set
            {
                if (!string.Equals(strokeDashArray, value, StringComparison.Ordinal))
                {
                    Parent?.OnPropertyChanged(nameof(StrokeDashArray), value, strokeDashArray, this);
                    strokeDashArray = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the stroke width of the diagram elements. By default, it is 1px.
        /// </summary>
        [JsonPropertyName("strokeWidth")]
        public double StrokeWidth
        {
            get => strokeWidth;
            set
            {
                if (!strokeWidth.Equals(value))
                {
                    Parent?.OnPropertyChanged(nameof(StrokeWidth), value, strokeWidth, this);
                    strokeWidth = value;
                }
            }
        }
        /// <summary>
        /// Creates a new <see cref="ShapeStyle"/> that is a copy of the current style.
        /// </summary>
        /// <returns>ShapeStyle</returns>
        public override object Clone()
        {
            return new ShapeStyle(this);
        }

        internal override void Dispose()
        {
            fill = null;
            strokeColor = null;
            strokeDashArray = null;
            if (gradient != null)
            {
                gradient.Dispose();
                gradient = null;
            }
        }
    }
}
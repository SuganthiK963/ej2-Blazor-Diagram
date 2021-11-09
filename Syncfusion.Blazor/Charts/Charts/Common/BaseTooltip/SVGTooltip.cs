using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class TextStyleModel
    {
        [JsonPropertyName("fontStyle")]
        public string FontStyle { get; set; }

        [JsonPropertyName("opacity")]
        public double Opacity { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("fontFamily")]
        public string FontFamily { get; set; }

        [JsonPropertyName("fontWeight")]
        public string FontWeight { get; set; }
    }

    public class TooltipBorderModel
    {
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }
    }

    public class ToolLocationModel
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }
    }

    public class AreaBoundsModel : ToolLocationModel
    {
        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("height")]
        public double Height { get; set; }
    }

    public class SVGTooltip
    {
        [JsonPropertyName("shared")]
        public bool Shared { get; set; }

        [JsonPropertyName("fill")]
        public string Fill { get; set; }

        [JsonPropertyName("header")]
        public string Header { get; set; }

        [JsonPropertyName("opacity")]
        public double Opacity { get; set; }

        [JsonPropertyName("textStyle")]
        public TextStyleModel TextStyle { get; set; }

        [JsonPropertyName("template")]
        public string Template { get; set; }

        [JsonPropertyName("enableAnimation")]
        public bool EnableAnimation { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("inverted")]
        public bool Inverted { get; set; }

        [JsonPropertyName("isNegative")]
        public bool IsNegative { get; set; }

        [JsonPropertyName("border")]
        public TooltipBorderModel Border { get; set; }

#pragma warning disable CA1819
        [JsonPropertyName("content")]
        public string[] Content { get; set; }

        [JsonPropertyName("clipBounds")]
        public ToolLocationModel ClipBounds { get; set; }

        [JsonPropertyName("palette")]
        public string[] Palette { get; set; }

        [JsonPropertyName("shapes")]
        public TooltipShape[] Shapes { get; set; }

#pragma warning restore CA1819
        [JsonPropertyName("location")]
        public ToolLocationModel Location { get; set; }

        [JsonPropertyName("offset")]
        public double Offset { get; set; }

        [JsonPropertyName("arrowPadding")]
        public double ArrowPadding { get; set; }

        [JsonPropertyName("data")]
        public TemplateData Data { get; set; }

        [JsonPropertyName("theme")]
        public string Theme { get; set; }

        [JsonPropertyName("areaBounds")]
        public AreaBoundsModel AreaBounds { get; set; }

        [JsonPropertyName("availableSize")]
        public Chart.Internal.Size AvailableSize { get; set; }

        [JsonPropertyName("isCanvas")]
        public bool IsCanvas { get; set; }

        [JsonPropertyName("isTextWrap")]
        public bool IsTextWrap { get; set; }

        [JsonPropertyName("enableRTL")]
        public bool EnableRTL { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TooltipShape
    {
        [EnumMember(Value = "Circle")]
        Circle,
        [EnumMember(Value = "Rectangle")]
        Rectangle,
        [EnumMember(Value = "Triangle")]
        Triangle,
        [EnumMember(Value = "Diamond")]
        Diamond,
        [EnumMember(Value = "Cross")]
        Cross,
        [EnumMember(Value = "HorizontalLine")]
        HorizontalLine,
        [EnumMember(Value = "VerticalLine")]
        VerticalLine,
        [EnumMember(Value = "Pentagon")]
        Pentagon,
        [EnumMember(Value = "InvertedTriangle")]
        InvertedTriangle,
        [EnumMember(Value = "Image")]
        Image,
        [EnumMember(Value = "None")]
        None
    }
}
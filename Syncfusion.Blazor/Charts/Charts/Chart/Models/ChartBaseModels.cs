using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.Chart.Models
{
    public class MarginModel
    {
        internal double Left { get; set; }

        internal double Right { get; set; }

        internal double Top { get; set; }

        internal double Bottom { get; set; }
    }

    public class FontModel
    {
        internal string Color { get; set; } = string.Empty;

        internal string FontFamily { get; set; }

        internal string FontStyle { get; set; } = "Normal";

        internal string FontWeight { get; set; } = "Normal";

        internal double Opacity { get; set; } = 1;

        internal string Size { get; set; } = "14px";

        internal Alignment TextAlignment { get; set; } = Alignment.Center;

        internal TextOverflow TextOverflow { get; set; } = TextOverflow.Trim;
    }

    public class BorderModel
    {
        internal string Color { get; set; } = string.Empty;

        internal double Width { get; set; } = 1;
    }

    public class LocationModel : OffsetModel
    {
    }

    public class OffsetModel
    {
        internal double X { get; set; }

        internal double Y { get; set; }
    }

    public class MarkerSettingModel
    {
        internal bool Visible { get; set; }

        internal ChartShape Shape { get; set; } = ChartShape.Circle;

        internal string ImageUrl { get; set; } = string.Empty;

        internal double Height { get; set; } = 5;

        internal double Width { get; set; } = 5;

        internal ChartEventBorder Border { get; set; } = new ChartEventBorder() { Width = 2 };

        internal OffsetModel Offset { get; set; } = new OffsetModel();

        internal string Fill { get; set; } = string.Empty;

        internal double Opacity { get; set; } = 1;

        internal DataLabelSettingModel DataLabel { get; set; } = new DataLabelSettingModel();
    }

    public class DataLabelSettingModel
    {
        internal bool Visible { get; set; }

        internal string Name { get; set; }

        internal string Fill { get; set; } = "transparent";

        internal double Opacity { get; set; } = 1;

        internal double Angle { get; set; }

        internal bool EnableRotation { get; set; }

        internal LabelPosition Position { get; set; } = LabelPosition.Auto;

        internal double Rx { get; set; } = 5;

        internal double Ry { get; set; } = 5;

        internal Alignment Alignment { get; set; } = Alignment.Center;

        internal BorderModel Border { get; set; } = new BorderModel() { Width = double.NaN, Color = null };

        internal MarginModel Margin { get; set; } = new MarginModel() { Left = 5, Right = 5, Top = 5, Bottom = 5 };

        internal FontModel Font { get; set; } = new FontModel() { Size = "11px", Color = string.Empty, FontStyle = "Normal", FontWeight = "Normal", FontFamily = "Segoe UI" };

        internal string Template { get; set; }

        internal string LabelIntersectAction { get; set; } = "Hide";
    }
}
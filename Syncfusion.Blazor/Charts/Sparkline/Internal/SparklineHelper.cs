using System;
using System.Globalization;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Sparkline.Internal
{
    internal static class SparklineHelper
    {
        private static StringComparison comparison = StringComparison.InvariantCulture;

        internal static double StringToNumber(string point, double containerSize, bool isCDNScript)
        {
            if (!string.IsNullOrEmpty(point))
            {
                return point.Contains('%', comparison) ? (isCDNScript ? (containerSize / 100) * int.Parse(point.Replace('%', ' '), null) : containerSize) : int.Parse(point.ToUpper(CultureInfo.InvariantCulture).Replace("PX", string.Empty, comparison), null);
            }

            return double.NaN;
        }

        internal static string SizeConverter(string sizeValue)
        {
            if (!string.IsNullOrEmpty(sizeValue))
            {
                return sizeValue.Contains("px", comparison) ? sizeValue.Replace("px", string.Empty, comparison) : sizeValue.Contains("%", comparison) ? sizeValue.Replace("%", string.Empty, comparison) : sizeValue;
            }

            return string.Empty;
        }

        internal static bool WithInBounds(double x, double y, RectInfo bounds)
        {
            return x >= bounds.X && x <= bounds.X + bounds.Width && y >= bounds.Y && y <= bounds.Y + bounds.Height;
        }

        internal static double GetTime(DateTime current)
        {
            return (current - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        internal static Style GetStyle(Theme theme)
        {
            switch (theme)
            {
                case Theme.BootstrapDark:
                case Theme.FabricDark:
                case Theme.MaterialDark:
                case Theme.HighContrast:
                    return new Style("#FFFFFF", "#FFFFFF", "#FFFFFF", "#FFFFFF", "#000000", "#363F4C", "#FFFFFF", "Inter", 1, 1, "Inter");
                case Theme.Bootstrap4:
                    return new Style("#6C757D", "#212529", "#212529", "#000000", "#FFFFFF", "#FFFFFF", "#212529", "HelveticaNeue-Medium", 1, 0.9, "HelveticaNeue");
                case Theme.Tailwind:
                    return new Style("#4B5563", "#212529", "#212529", "#111827", "#FFFFFF", "#F9FAFB", "#1F2937", "Inter", 1, 1, "Inter");
                case Theme.TailwindDark:
                    return new Style("#D1D5DB", "#F9FAFB", "#F9FAFB", "#F9FAFB", "transparent", "#1F2937", "#9CA3AF", "Inter", 1, 1, "Inter");
                case Theme.Bootstrap5:
                    return new Style("#D1D5DB", "#343A40", "#212529", "#212529", "rgba(255, 255, 255, 0.0)", "#F9FAFB", "#1F2937", "HelveticaNeue", 1, 1, "HelveticaNeue");
                case Theme.Bootstrap5Dark:
                    return new Style("#D1D5DB", "#E9ECEF", "#ADB5BD", "#E9ECEF", "rgba(255, 255, 255, 0.0)", "#212529", "#ADB5BD", "HelveticaNeue", 1, 1, "HelveticaNeue");
                default:
                    return new Style("#000000", "#424242", "#000000", "#363F4C", "#FFFFFF", "#FFFFFF", "#000000", "Inter", 1, 1, "Inter");
            }
        }

        internal static string FormatText(string xdata, string ydata, string xname, string yname, string format)
        {
            string data = string.Join(string.Format(CultureInfo.InvariantCulture, "{0}", xdata), format.Split("${" + xname.ToString() + "}"));
            return string.Join(string.Format(CultureInfo.InvariantCulture, "{0}", ydata), data.Split("${" + yname.ToString() + "}"));
        }
    }
}
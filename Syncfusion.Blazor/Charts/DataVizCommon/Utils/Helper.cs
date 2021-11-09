using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Charts")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.RangeNavigator")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.SmithChart")]
namespace Syncfusion.Blazor.DataVizCommon
{
    public class DataVizCommonHelper
    {
        private const string SPACE = " ";

        internal static string[] GetSeriesColor(string theme)
        {
            switch (theme)
            {
                case "Fabric":
                    return new string[] { "#4472c4", "#ed7d31", "#ffc000", "#70ad47", "#5b9bd5", "#c1c1c1", "#6f6fe2", "#e269ae", "#9e480e", "#997300" };
                case "Bootstrap4":
                    return new string[] { "#a16ee5", "#f7ce69", "#55a5c2", "#7ddf1e", "#ff6ea6", "#7953ac", "#b99b4f", "#407c92", "#5ea716", "#b91c52" };
                case "Bootstrap":
                    return new string[] { "#a16ee5", "#f7ce69", "#55a5c2", "#7ddf1e", "#ff6ea6", "#7953ac", "#b99b4f", "#407c92", "#5ea716", "#b91c52" };
                case "HighContrastLight":
                case "HighContrast":
                    return new string[] { "#79ECE4", "#E98272", "#DFE6B6", "#C6E773", "#BA98FF", "#FA83C3", "#00C27A", "#43ACEF", "#D681EF", "#D8BC6E" };
                case "MaterialDark":
                    return new string[] { "#9ECB08", "#56AEFF", "#C57AFF", "#61EAA9", "#EBBB3E", "#F45C5C", "#8A77FF", "#63C7FF", "#FF84B0", "#F7C928" };
                case "FabricDark":
                    return new string[] { "#4472c4", "#ed7d31", "#ffc000", "#70ad47", "#5b9bd5", "#c1c1c1", "#6f6fe2", "#e269ae", "#9e480e", "#997300" };
                case "BootstrapDark":
                    return new string[] { "#a16ee5", "#f7ce69", "#55a5c2", "#7ddf1e", "#ff6ea6", "#7953ac", "#b99b4f", "#407c92", "#5ea716", "#b91c52" };
                case "Tailwind":
                    return new string[] { "#5A61F6", "#65A30D", "#334155", "#14B8A6", "#8B5CF6", "#0369A1", "#F97316", "#9333EA", "#F59E0B", "#15803D" };
                case "TailwindDark":
                    return new string[] { "#8B5CF6", "#22D3EE", "#F87171", "#4ADE80", "#E879F9", "#FCD34D", "#F97316", "#2DD4BF", "#F472B6", "#10B981" };
                case "Bootstrap5":
                    return new string[] { "#262E0B", "#668E1F", "#AF6E10", "#862C0B", "#1F2D50", "#64680B", "#311508", "#4C4C81", "#0C7DA0", "#862C0B" };
                case "Bootstrap5Dark":
                    return new string[] { "#5ECB9B", "#A860F1", "#EBA844", "#557EF7", "#E9599B", "#BFC529", "#3BC6CF", "#7A68EC", "#74B706", "#EA6266" };
                default:
                    return new string[] { "#00bdae", "#404041", "#357cd2", "#e56590", "#f8b883", "#70ad47", "#dd8abd", "#7f84e8", "#7bb4eb", "#ea7a57" };
            }
        }

        internal static RenderFragment RenderTSpan(TextOptions option) => builder =>
        {
            for (int i = 0; i < option.TextLocationCollection.Count; i++)
            {
                builder.OpenElement(SvgRendering.Seq++, "tspan");
                Dictionary<string, object> svgattributes = new Dictionary<string, object> {
                    { "id", option.Id},
                    { "x", option.X.ToString(CultureInfo.InvariantCulture) },
                    { "y", option.TextLocationCollection[i].Y.ToString(CultureInfo.InvariantCulture) }
                };
                builder.AddMultipleAttributes(SvgRendering.Seq++, svgattributes);
                builder.AddContent(SvgRendering.Seq++, option.TextLocationCollection[i].Text);
                builder.CloseElement();
            }
        };

        internal static string FindDataType(Type dataType)
        {
            if (dataType.Equals(typeof(JObject)))
            {
                return "JObject";
            }
            else if (dataType.Equals(typeof(ExpandoObject)))
            {
                return "ExpandoObject";
            }
            else if (dataType.BaseType.Equals(typeof(DynamicObject)))
            {
                return "DynamicObject";
            }
            else
            {
                return string.Empty;
            }
        }

        internal static string FindThemeColor(string actualColor, string themeColor)
        {
            return string.IsNullOrEmpty(actualColor) ? themeColor : actualColor;
        }

        internal static double StringToNumber(string size, double containerSize)
        {
            if (!string.IsNullOrEmpty(size) && size != "auto")
            {
                return size.Contains('%', StringComparison.InvariantCulture) ? (containerSize / 100) * int.Parse(size.Replace("%", SPACE, StringComparison.InvariantCulture), null) : double.Parse(size.ToLower(CultureInfo.InvariantCulture).Replace("px", string.Empty, StringComparison.InvariantCulture), CultureInfo.InvariantCulture);
            }

            return double.NaN;
        }

        internal static double PixelToNumber(string size, Regex regexValue)
        {
            if (!string.IsNullOrEmpty(size))
            {
#pragma warning disable CA1304
                switch (regexValue.Match(size.ToLower()).ToString())
                {
                    case "px":
                        return Convert.ToDouble(regexValue.Replace(size.ToLower(), string.Empty), null);
                    case "rem":
                    case "em":
                        return Convert.ToDouble(regexValue.Replace(size.ToLower(), string.Empty), null) * 16;
                    case "pt":
                        return Convert.ToDouble(regexValue.Replace(size.ToLower(), string.Empty), null) * 1.3333333333333333;
                    default:
                        return 0;
                }
#pragma warning restore CA1304
            }
            return 0;
        }
        internal static bool BindObservable<T>(T parent, string name, IEnumerable<object> datasource, bool isObservableWired)
        {
            if (!isObservableWired && datasource is INotifyCollectionChanged)
            {
                (parent as SfBaseComponent).UpdateObservableEventsForObject(name, datasource, true);
                (parent as SfBaseComponent).UpdateObservableEventsForObject(name, datasource);
                return true;
            }
            return isObservableWired;
        }
    }
}
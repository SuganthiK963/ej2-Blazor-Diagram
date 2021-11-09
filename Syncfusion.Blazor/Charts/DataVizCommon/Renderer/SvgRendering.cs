
using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using System;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Charts")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.RangeNavigator")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Sparkline")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.SmithChart")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.StockChart")]
namespace Syncfusion.Blazor.DataVizCommon
{
    public class SvgRendering
    {
        internal static int Seq { get; set; }
        internal List<SvgText> TextElementList { get; set; } = new List<SvgText>();
        internal List<SvgPath> PathElementList { get; set; } = new List<SvgPath>();
        internal List<SvgEllipse> EllipseElementList { get; set; } = new List<SvgEllipse>();
        internal List<SvgRect> RectElementList { get; set; } = new List<SvgRect>();
        internal List<SvgLine> LineElementList { get; set; } = new List<SvgLine>();
        internal List<SvgImage> ImageCollection { get; set; } = new List<SvgImage>();
        internal List<SvgCircle> CircleCollection { get; set; } = new List<SvgCircle>();
        internal List<ElementReference> GroupCollection { get; set; } = new List<ElementReference>();
        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;
        internal void RefreshElementList()
        {
            TextElementList = new List<SvgText>();
            PathElementList = new List<SvgPath>();
            EllipseElementList = new List<SvgEllipse>();
            RectElementList = new List<SvgRect>();
            LineElementList = new List<SvgLine>();
            ImageCollection = new List<SvgImage>();
            CircleCollection = new List<SvgCircle>();
            GroupCollection = new List<ElementReference>();
        }
        
        internal void RenderText(RenderTreeBuilder renderTreeBuilder, TextOptions textOptions)
        {
            renderTreeBuilder.OpenComponent<SvgText>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(textOptions));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { TextElementList.Add((SvgText)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal void RenderRect(RenderTreeBuilder renderTreeBuilder, string id, double x, double y, double width, double height, double strokeWidth, string stroke, string fill, string style = "")
        {
            renderTreeBuilder.OpenComponent<SvgRect>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(new RectOptions(id, x, y, width, height, strokeWidth, stroke, fill, 0, 0, 1, style)));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { RectElementList.Add((SvgRect)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal void RenderRect(RenderTreeBuilder renderTreeBuilder, RectOptions rectOptions)
        {
            renderTreeBuilder.OpenComponent<SvgRect>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(rectOptions));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { RectElementList.Add((SvgRect)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal SvgRect RenderRectWithRef(RenderTreeBuilder renderTreeBuilder, RectOptions rectOptions)
        {
            SvgRect reference = null;
            renderTreeBuilder.OpenComponent<SvgRect>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(rectOptions));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { reference = (SvgRect)ins; });
            renderTreeBuilder.CloseComponent();
            return reference;
        }

        internal void RenderPath(RenderTreeBuilder renderTreeBuilder, PathOptions pathOptions, string style = "")
        {
            renderTreeBuilder.OpenComponent<SvgPath>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(pathOptions));
            renderTreeBuilder.AddAttribute(Seq++, "Style", style);
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { PathElementList.Add((SvgPath)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal SvgPath RenderPath(RenderTreeBuilder renderTreeBuilder, PathOptions pathOptions)
        {
            SvgPath reference = null;
            renderTreeBuilder.OpenComponent<SvgPath>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(pathOptions));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { PathElementList.Add((SvgPath)ins); });
            renderTreeBuilder.CloseComponent();
            return reference;
        }

        internal void OpenGroupElement(RenderTreeBuilder renderTreeBuilder, string id, string transform = "", string clippath = "", string style = "", string tabIndex = "", string accessText = "")
        {
            renderTreeBuilder.OpenElement(Seq++, "g");
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
            renderTreeBuilder.AddAttribute(Seq++, "transform", transform);
            renderTreeBuilder.AddAttribute(Seq++, "clip-path", clippath);
            renderTreeBuilder.AddAttribute(Seq++, "style", style);
            renderTreeBuilder.AddAttribute(Seq++, "tabindex", tabIndex);
            renderTreeBuilder.AddAttribute(Seq++, "aria-label", accessText);
            renderTreeBuilder.AddElementReferenceCapture(Seq++, ins => { GroupCollection.Add(ins); });
        }

        internal void RenderPath(RenderTreeBuilder renderTreeBuilder, string id, string direction, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "transparent", string accessText = "")
        {
            renderTreeBuilder.OpenComponent<SvgPath>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(new PathOptions(id, direction, strokeDasharray, strokeWidth, stroke, opacity, fill, string.Empty, string.Empty, accessText)));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { PathElementList.Add((SvgPath)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal void RenderClipPath(RenderTreeBuilder renderTreeBuilder, string id, Rect rect, string visibility = "visible")
        {
            renderTreeBuilder.OpenElement(Seq++, "defs");
            renderTreeBuilder.OpenElement(Seq++, "clipPath");
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
            RenderRect(renderTreeBuilder, new RectOptions(id + "_Rect", rect.X, rect.Y, rect.Width, rect.Height, 1, "transparent", "transparent", 0, 0, 1, visibility));
            renderTreeBuilder.CloseElement();
            renderTreeBuilder.CloseElement();
        }

        internal void RenderCircularClipPath(RenderTreeBuilder renderTreeBuilder, string id, CircleOptions options)
        {
            renderTreeBuilder.OpenElement(Seq++, "defs");
            renderTreeBuilder.OpenElement(Seq++, "clipPath");
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
            RenderCircle(renderTreeBuilder, options);
            renderTreeBuilder.CloseElement();
            renderTreeBuilder.CloseElement();
        }

#pragma warning disable CA1822 
        internal Dictionary<string, object> GetOptions(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
            Dictionary<string, object> attributes = new Dictionary<string, object> { };
            foreach (PropertyInfo property in propertyInfos)
            {
                attributes.Add(property.Name, property.GetValue(obj));
            }
            return attributes;
        }

        internal void RenderPolygon(RenderTreeBuilder renderTreeBuilder, string id, string fill, string points)
        {
            renderTreeBuilder.OpenComponent<SvgPolygon>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, new Dictionary<string, object>() { { "Id", id }, { "Fill", fill }, { "points", points } });
            renderTreeBuilder.CloseComponent();
        }

        internal void OpenClipPath(RenderTreeBuilder renderTreeBuilder, string id)
        {
            renderTreeBuilder.OpenElement(Seq++, "clipPath");
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
        }

        internal string CreateElement(RenderTreeBuilder renderTreeBuilder, string tag, string id, bool isClose = true)
#pragma warning restore CA1822
        {
            renderTreeBuilder.OpenElement(Seq++, tag);
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
            if (isClose)
            {
                renderTreeBuilder.CloseElement();
            }
            return id;
        }

        internal void RenderEllipse(RenderTreeBuilder renderTreeBuilder, EllipseOptions ellipesOption)
        {
            renderTreeBuilder.OpenComponent<SvgEllipse>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(ellipesOption));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { EllipseElementList.Add((SvgEllipse)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal void RenderCircle(RenderTreeBuilder renderTreeBuilder, CircleOptions ellipesOption)
        {
            renderTreeBuilder.OpenComponent<SvgCircle>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(ellipesOption));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { CircleCollection.Add((SvgCircle)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal void RenderImage(RenderTreeBuilder renderTreeBuilder, ImageOptions ImageOption)
        {
            renderTreeBuilder.OpenComponent<SvgImage>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(ImageOption));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { ImageCollection.Add((SvgImage)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal void RenderLine(RenderTreeBuilder renderTreeBuilder, string id, double x1, double y1, double x2, double y2, string stroke, double strokewidth)
        {
            renderTreeBuilder.OpenComponent<SvgLine>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, new Dictionary<string, object>() { { "id", id }, { "x1", x1 }, { "y1", y1 }, { "x2", x2 }, { "y2", y2 }, { "stroke", stroke }, { "strokeWidth", strokewidth } });
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { LineElementList.Add((SvgLine)ins); });
            renderTreeBuilder.CloseComponent();
        }

        internal void Dispose()
        {
            TextElementList.ForEach(item => item.Dispose());
            TextElementList = null;
            PathElementList = null;
            EllipseElementList = null;
            RectElementList = null;
            LineElementList = null;
            ImageCollection = null;
            CircleCollection = null;
            GroupCollection = null;
        }
    }
}
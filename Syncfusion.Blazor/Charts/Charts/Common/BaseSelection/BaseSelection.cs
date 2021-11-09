using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    class BaseSelection
    {
        internal string StyleId { get; set; }

        protected string Unselected { get; set; } = string.Empty;

        protected string InnerHTML { get; set; }

        protected CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        protected static List<PatternOptions> ReqPatterns { get; set; } = new List<PatternOptions>();

        internal static List<PatternOptions> GivenPatterns { get { return ReqPatterns; } }

        internal static SelectionStyleComponent StyleRender { get; set; } = new SelectionStyleComponent();

        internal static void CreateStylePatternComponent(RenderTreeBuilder builder, string elementId)
        {
            builder.OpenComponent<SelectionStyleComponent>(SvgRendering.Seq++);
            builder.AddAttribute(SvgRendering.Seq++, "ComponentID", elementId);
            builder.AddAttribute(SvgRendering.Seq++, "GivenPattern", new List<PatternOptions>());
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, instance => { StyleRender = (SelectionStyleComponent)instance; });
            builder.CloseComponent();
        }

        internal static void ApppendSelectionPattern()
        {
            StyleRender.DrawPattern(ReqPatterns);
        }

#pragma warning disable CA1822
        protected string FindPattern(string color, int index, SelectionPattern patternName, double opacity)
        {
            List<object> pathOptions = new List<object>() { };
            PatternOptions patternGroup = new PatternOptions() { Id = patternName + "_Selection" + "_" + index, PatternUnits = "userSpaceOnUse" };
            switch (patternName)
            {
                case SelectionPattern.Dots:
                    patternGroup.Width = patternGroup.Height = 6;
                    pathOptions.Add(new RectOptions("PatternStroke", 0, 0, 7, 7, 0, "0.0000001", "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new EllipseOptions(string.Empty, "2", "2", "3", "3", string.Empty, 1, color));
                    break;
                case SelectionPattern.Pacman:
                    patternGroup.Height = 18.384;
                    patternGroup.Width = 17.917;
                    pathOptions.Add(new RectOptions(string.Empty, 0, 0, 18.384, 17.917, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M9.081,9.194l5.806-3.08c-0.812-1.496-2.403-3.052-4.291-3.052H8.835C6.138,3.063,3,6.151,3,8.723v1.679   c0,2.572,3.138,5.661,5.835,5.661h1.761c2.085,0,3.835-1.76,4.535-3.514L9.081,9.194z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.Chessboard:
                    patternGroup.Height = patternGroup.Width = 10;
                    pathOptions.Add(new RectOptions(string.Empty, 0, 0, 10, 10, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new RectOptions(string.Empty, 0, 0, 5, 5, 0, null, color, 0, 0, opacity));
                    pathOptions.Add(new RectOptions(string.Empty, 5, 5, 5, 5, 0, null, color, 0, 0, opacity));
                    break;
                case SelectionPattern.Crosshatch:
                    patternGroup.Height = patternGroup.Width = 8;
                    pathOptions.Add(new RectOptions(string.Empty, 0, 0, 8, 8, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(string.Empty, "M0 0L8 8ZM8 0L0 8Z", string.Empty, 1, color, 1));
                    break;
                case SelectionPattern.DiagonalForward:
                    patternGroup.Height = patternGroup.Width = 6;
                    pathOptions.Add(new RectOptions(null, 0, 0, 6, 6, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M 3 -3 L 9 3 M 6 6 L 0 0 M 3 9 L -3 3", null, 2, color, opacity));
                    break;
                case SelectionPattern.DiagonalBackward:
                    patternGroup.Height = patternGroup.Width = 6;
                    pathOptions.Add(new RectOptions(null, 0, 0, 6, 6, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M 3 -3 L -3 3 M 0 6 L 6 0 M 9 3 L 3 9", null, 2, color, opacity));
                    break;
                case SelectionPattern.Grid:
                    patternGroup.Height = patternGroup.Width = 6;
                    pathOptions.Add(new RectOptions(null, 0, 0, 6, 6, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M1 3.5L11 3.5 M0 3.5L11 3.5 M0 7.5L11 7.5 M0 11.5L11 11.5 M5.5 0L5.5 12 M11.5 0L11.5 12Z", null, 1, color, opacity));
                    break;
                case SelectionPattern.Turquoise:
                    patternGroup.Height = patternGroup.Width = 17;
                    pathOptions.Add(new RectOptions(null, 0, 0, 17, 17, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M0.5739999999999998,2.643a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null, 1, color, opacity, color, "10"));
                    pathOptions.Add(new PathOptions(null, "M11.805,2.643a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null, 1, color, opacity, color, "10"));
                    pathOptions.Add(new PathOptions(null, "M6.19,2.643a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null, 1, color, opacity, color, "10"));
                    pathOptions.Add(new PathOptions(null, "M11.805,8.217a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null, 1, color, opacity, color, "10"));
                    pathOptions.Add(new PathOptions(null, "M6.19,8.217a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null, 1, color, opacity, color, "10"));
                    pathOptions.Add(new PathOptions(null, "M11.805,13.899a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null, 1, color, opacity, color, "10"));
                    pathOptions.Add(new PathOptions(null, "M6.19,13.899a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null, 1, color, opacity, color, "10"));
                    break;
                case SelectionPattern.Star:
                    patternGroup.Height = patternGroup.Width = 21;
                    pathOptions.Add(new RectOptions(null, 0, 0, 21, 21, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M15.913,18.59L10.762 12.842 5.613 18.75 8.291 11.422 0.325 9.91 8.154 8.33 5.337 0.91 10.488 6.658 15.637 0.75 12.959 8.078 20.925 9.59 13.096 11.17 z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.Triangle:
                    patternGroup.Height = patternGroup.Width = 10;
                    pathOptions.Add(new RectOptions(null, 0, 0, 10, 10, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M4.987,0L7.48 4.847 9.974 9.694 4.987 9.694 0 9.694 2.493 4.847 z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.Circle:
                    double circleNum = 9;
                    patternGroup.Height = patternGroup.Width = circleNum;
                    pathOptions.Add(new RectOptions(null, 0, 0, circleNum, circleNum, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new EllipseOptions(null, "3.625", "3.625", "5.125", "3.875", null, 1, null, opacity, color));
                    break;
                case SelectionPattern.Tile:
                    double tileNum = 18;
                    patternGroup.Height = patternGroup.Width = tileNum;
                    pathOptions.Add(new RectOptions(null, 0, 0, tileNum, tileNum, 0, null, "#ffffff", opacity));
                    pathOptions.Add(new PathOptions(null, "M0,9L0 0 9 0 z", null, 1, color, opacity, color));
                    pathOptions.Add(new PathOptions(null, "M9,9L9 0 18 0 z", null, 1, color, opacity, color));
                    pathOptions.Add(new PathOptions(null, "M0,18L0 9 9 9 z", null, 1, color, opacity, color));
                    pathOptions.Add(new PathOptions(null, "M9,18L9 9 18 9 z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.HorizontalDash:
                    patternGroup.Height = patternGroup.Width = 12;
                    pathOptions.Add(new RectOptions(null, 0, 0, 12, 12, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M0,1.5 L10 1.5 M0,5.5 L10 5.5 M0,9.5 L10 9.5 z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.VerticalDash:
                    patternGroup.Height = patternGroup.Width = 12;
                    pathOptions.Add(new RectOptions(null, 0, 0, 12, 12, 0, null, "#ffffff", 0, 9, opacity));
                    pathOptions.Add(new PathOptions(null, "M1.5,0 L1.5 10 M5.5,0 L5.5 10 M9.5,0 L9.5 10 z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.Rectangle:
                    patternGroup.Height = 12;
                    patternGroup.Width = 10;
                    pathOptions.Add(new RectOptions(null, 0, 0, 10, 12, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new RectOptions(null, 1, 2, 9, 4, 0, null, color, 0, 0, opacity));
                    pathOptions.Add(new RectOptions(null, 7, 2, 9, 4, 0, null, color, 0, 0, opacity));
                    break;
                case SelectionPattern.Box:
                    patternGroup.Height = patternGroup.Width = 10;
                    pathOptions.Add(new RectOptions(null, 0, 0, 10, 12, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new RectOptions(null, 1, 2, 9, 4, 0, null, color, 0, 0, opacity));
                    break;
                case SelectionPattern.HorizontalStripe:
                    patternGroup.Height = 12;
                    patternGroup.Width = 10;
                    pathOptions.Add(new RectOptions(null, 0, 0, 10, 12, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M0,0.5 L10 0.5 M0,4.5 L10 4.5 M0,8.5 L10 8.5 z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.VerticalStripe:
                    patternGroup.Height = 12;
                    patternGroup.Width = 10;
                    pathOptions.Add(new RectOptions(null, 0, 0, 10, 12, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new PathOptions(null, "M0.5,0 L0.5 10 M4.5,0 L4.5 10 M8.5,0 L8.5 10 z", null, 1, color, opacity, color));
                    break;
                case SelectionPattern.Bubble:
                    patternGroup.Height = patternGroup.Width = 20;
                    pathOptions.Add(new RectOptions(null, 0, 0, 20, 20, 0, null, "#ffffff", 0, 0, opacity));
                    pathOptions.Add(new EllipseOptions(null, "3.429", "3.429", "5.217", "11.325", null, 1, null, opacity, "#D0A6D1"));
                    pathOptions.Add(new EllipseOptions(null, "4.884", "4.884", "13.328", "6.24", null, 1, null, 1, color));
                    pathOptions.Add(new EllipseOptions(null, "3.018", "3.018", "13.277", "14.66", null, 1, null, opacity, "#D0A6D1"));
                    break;
            }

            patternGroup.ShapeOptions = pathOptions;
            ReqPatterns?.Add(patternGroup);
            return "url(#" + patternName + "_Selection_" + index + ")";
        }

        protected SvgClass FindDomElement(SvgRendering renderer, string id)
        {
            if (renderer.PathElementList.Find(item => item.Id == id) != null)
            {
                return renderer.PathElementList.Find(item => item.Id == id);
            }
            else if (renderer.RectElementList.Find(item => item.Id == id) != null)
            {
                return renderer.RectElementList.Find(item => item.Id == id);
            }
            else if (renderer.EllipseElementList.Find(item => item.Id == id) != null)
            {
                return renderer.EllipseElementList.Find(item => item.Id == id);
            }

            return null;
        }

        protected List<SvgClass> FindElementByClass(SvgRendering renderer, string className)
        {
            List<SvgClass> result = new List<SvgClass>();
            renderer.PathElementList.Where(x => x.Class == className).ToList().ForEach(y => result.Add(y));
            renderer.RectElementList.Where(x => x.Class == className).ToList().ForEach(y => result.Add(y));
            renderer.EllipseElementList.Where(x => x.Class == className).ToList().ForEach(y => result.Add(y));
            return result;
        }

        internal void RemoveSvgClass(SvgClass element, string className)
        {
            if (element != null && !string.IsNullOrEmpty(element.Class))
            {
                element.ChangeClass(className, false);
            }
        }

        internal void AddSvgClass(SvgClass element, string className)
        {
            if (className != null && !(string.IsNullOrEmpty(element.Class) ? string.Empty : element.Class).Contains(className, System.StringComparison.InvariantCulture))
            {
                element.ChangeClass(className, true);
            }
        }

        internal virtual void Dispose()
        {
            ReqPatterns.Clear();
            StyleRender?.GivenPattern?.Clear();
            StyleRender = null;
        }
    }
}
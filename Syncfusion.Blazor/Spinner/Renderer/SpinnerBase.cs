using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Spinner.Internal
{
    /// <summary>
    /// Represents the common methods that are used in all the themes.
    /// </summary>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SpinnerBase : SfBaseComponent
    {
        #region Constants
        internal const int DEFAULT_WIDTH = 30;
        internal const int DEFT_BOOT4_WIDTH = 36;
        internal const string PATH_CIRCLE_CLASS = "e-path-circle";
        internal const string DRAW_ARC_CONST = " 0 1 1 ";
        internal const double ONE_EIGHTY_ANGLE = 180.0;

        internal const string SHOW = "Show";
        internal const string COMMA = ",";
        internal const string SPACE = " ";
        internal const int ZERO = 0;
        internal const int ONE = 1;
        internal const string PX = "px";
        internal const string SEMICOLON = ";";
        internal const string WIDTH = "width";
        internal const string HEIGHT = "height";
        internal const string TYPEUPDATE = "TypeUpdate";
        internal const string SVG_ID = "SVG";
        internal const int TWO_DIVISION = 2;
        internal const int TEN_STROKE_SIZE = 10;
        internal const int HUNDRED_DIVISION = 100;
        internal const string OPEN_ROUND_BRACKET = "(";
        internal const string CLOSE_ROUND_BRACKET = ")";
        internal const int NINETY_ANGLE = 90;
        internal const string VIEWBOX_CONST = "0 0 ";
        internal const string PX_GAP = "px; ";
        internal const string COLON_GAP = ": ";
        internal const string TRANSFORM_ORIGIN = "transform-origin";
        #endregion

        #region SVG variables
        internal string SpinnerSvgClass { get; set; }

        internal string ViewBox { get; set; }

        internal string SvgStyle { get; set; }

        internal string SvgId { get; set; }

        #endregion

        internal string PathClass { get; set; } = PATH_CIRCLE_CLASS;

        #region Common Methods

        internal static int CalculateRadius(SpinnerType theme, string size)
        {
            int defaultSize;
            if (theme == SpinnerType.Bootstrap4)
            {
                defaultSize = DEFT_BOOT4_WIDTH;
            }
            else
            {
                defaultSize = DEFAULT_WIDTH;
            }

            float width = size != null ? float.Parse(size, null) : defaultSize;
            return theme == SpinnerType.Bootstrap ? Convert.ToInt32(width) : Convert.ToInt32(width / TWO_DIVISION);
        }

        internal static ArcPoints DefineArcPoints(int centerX, int centerY, int radius, int angle)
        {
            double radians = (angle - NINETY_ANGLE) * Math.PI / ONE_EIGHTY_ANGLE;
            ArcPoints arcPoints = new ArcPoints()
            {
                VarX = centerX + (radius * Math.Cos(radians)),
                VarY = centerY + (radius * Math.Sin(radians))
            };
            return arcPoints;
        }

        internal static string DrawArc(decimal diameter, decimal strokeSize)
        {
            decimal radius = diameter / TWO_DIVISION;
            string radiusVal = Convert.ToString(radius, System.Globalization.CultureInfo.InvariantCulture);
            decimal offset = strokeSize / TWO_DIVISION;
            string offsetVal = Convert.ToString(offset, System.Globalization.CultureInfo.InvariantCulture);
            string offsetRadiusVal = Convert.ToString(radius - offset, System.Globalization.CultureInfo.InvariantCulture);
            return "M" + radiusVal + COMMA + offsetVal + "A" + offsetRadiusVal + COMMA + offsetRadiusVal + DRAW_ARC_CONST + offsetVal + COMMA + radiusVal;
        }

        internal static decimal GetStrokeSize(decimal diameter)
        {
            return (decimal)TEN_STROKE_SIZE / HUNDRED_DIVISION * diameter;
        }
        #endregion
    }

    internal class ArcPoints
    {
        internal double VarX { get; set; }

        internal double VarY { get; set; }
    }

    internal class GlobalTimeOut
    {
        internal int TimeOut { get; set; }

        internal SpinnerType Type { get; set; }

        internal int Radius { get; set; }

        internal bool IsAnimate { get; set; }
    }
}
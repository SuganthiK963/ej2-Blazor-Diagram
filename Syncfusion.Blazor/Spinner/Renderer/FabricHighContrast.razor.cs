using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Spinner.Internal
{
    /// <summary>
    /// Represents the Spinner's Fabric and High-Contrast class.
    /// </summary>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class FabricHighContrast : SpinnerBase
    {
        private const string PATH_ARC_CLASS = "e-path-arc";
        private const string SVG_CLASS_FABRIC = "e-spin-fabric";
        private const string SVG_CLASS_TAILWIND = "e-spin-tailwind";
        private const string SVG_CLASS_HIGH_CONTRAST = "e-spin-high-contrast";
        private const int START_ARC_VALUE = 315;
        private const int END_ARC_VALUE = 45;
        private string pathArcClass = PATH_ARC_CLASS;
        private string pathCircleAttribute;
        private string pathArcAttribute;
        private int radius;

        [CascadingParameter]
        private SfSpinner BaseParent { get; set; }

        internal override void ComponentDispose()
        {
            BaseParent = null;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            radius = CalculateRadius(BaseParent.Type, BaseParent.Size);
            SpinnerSvgClass = BaseParent.Type == SpinnerType.Fabric ? SVG_CLASS_FABRIC : BaseParent.Type == SpinnerType.HighContrast ? SVG_CLASS_HIGH_CONTRAST : SVG_CLASS_TAILWIND;
            SvgId = SfBaseUtils.GenerateID(SVG_ID);
            int centerX = radius;
            int centerY = radius;
            int diameter = radius * TWO_DIVISION;
            string transformOrigin = (diameter / TWO_DIVISION) + PX;
            pathCircleAttribute = DefineCircle(centerX, centerY);
            pathArcAttribute = DefineArc(centerX, centerY, START_ARC_VALUE, END_ARC_VALUE);
            ViewBox = VIEWBOX_CONST + diameter + SPACE + diameter;
            SvgStyle = WIDTH + COLON_GAP + diameter + PX_GAP + HEIGHT + COLON_GAP + diameter + PX_GAP + TRANSFORM_ORIGIN + COLON_GAP + transformOrigin + SPACE + transformOrigin + SPACE + transformOrigin + SEMICOLON;
            await base.OnInitializedAsync();
        }

        private string DefineCircle(int varX, int varY)
        {
            dynamic[] circleValue = new dynamic[] { "M", varX, varY, "m", -radius, 0, "a", radius, radius, 0, 1, 0, radius * TWO_DIVISION, 0, "a", radius, radius, 0, 1, 0, -radius * TWO_DIVISION, 0 };
            return string.Join(SPACE, circleValue);
        }

        private string DefineArc(int x, int y, int startArc, int endArc)
        {
            ArcPoints startObj = DefineArcPoints(x, y, radius, endArc);
            ArcPoints endObj = DefineArcPoints(x, y, radius, startArc);
            dynamic[] arcValue = new dynamic[] { "M", Convert.ToString(startObj.VarX, System.Globalization.CultureInfo.InvariantCulture), Convert.ToString(startObj.VarY, System.Globalization.CultureInfo.InvariantCulture), "A", radius, radius, 0, 0, 0, Convert.ToString(endObj.VarX, System.Globalization.CultureInfo.InvariantCulture), Convert.ToString(endObj.VarY, System.Globalization.CultureInfo.InvariantCulture) };
            return string.Join(SPACE, arcValue);
        }
    }
}
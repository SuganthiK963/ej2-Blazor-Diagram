using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Spinner.Internal
{
    /// <summary>
    /// Represents the Spinner's Bootstrap 4 class.
    /// </summary>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class Bootstrap4 : SpinnerBase
    {
        private const string SVG_CLASS_BOOT4 = "e-spin-bootstrap4";

        private const string SVG_CLASS_BOOT5 = "e-spin-bootstrap5";

        [CascadingParameter]
        private SfSpinner BaseParent { get; set; }

        private string dAttribute { get; set; }

        private int Radius { get; set; }

        /// <summary>
        /// Dispose the unmanaged resources.
        /// </summary>
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
            Radius = CalculateRadius(BaseParent.Type, BaseParent.Size);
            SpinnerSvgClass = BaseParent.Type == SpinnerType.Bootstrap4 ? SVG_CLASS_BOOT4 : SVG_CLASS_BOOT5;
            SvgId = SfBaseUtils.GenerateID(SVG_ID);
            decimal diameter = Radius * TWO_DIVISION;
            decimal strokeSize = GetStrokeSize(diameter);
            string transformOrigin = (diameter / TWO_DIVISION) + PX;
            ViewBox = VIEWBOX_CONST + diameter + SPACE + diameter;
            SvgStyle = WIDTH + COLON_GAP + diameter + PX_GAP + HEIGHT + COLON_GAP + diameter + PX_GAP + TRANSFORM_ORIGIN + COLON_GAP + transformOrigin + SPACE + transformOrigin + SPACE + transformOrigin + SEMICOLON;
            dAttribute = DrawArc(diameter, strokeSize);
            await base.OnInitializedAsync();
        }
    }
}
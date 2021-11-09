using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DataVizCommon;
using System.Threading.Tasks;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class SvgSelectionPath
    {
        [CascadingParameter]
        public SvgSelectionRectCollection Parent { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Stroke { get; set; }

        [Parameter]
        public string StrokeWidth { get; set; }

        [Parameter]
        public string Fill { get; set; }

        [Parameter]
        public string Path { get; set; }

        [Parameter]
        public CloseOptions Close { get; set; }

        [Parameter]
        public EventCallback<string> PathChanged { get; set; }

        [Parameter]
        public EventCallback<CloseOptions> CloseChanged { get; set; }

        internal bool IsDrawCloseIcon { get; set; }

        private string cursorStyle { get; set; } = "cursor:move";

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        public async Task ChangePath(string path)
        {
            IsDrawCloseIcon = false;
            Path = path;
#pragma warning disable CA2007
            await PathChanged.InvokeAsync(Path);
        }

        public async Task DrawCloseIcon(CircleOptions circleAttr, PathOptions pathAttr)
        {
            Close.Circle = circleAttr;
            Close.Path = pathAttr;
            IsDrawCloseIcon = true;
            await CloseChanged.InvokeAsync(Close);
#pragma warning restore CA2007
        }

        private void RemoveCurrentElement()
        {
            Parent.RemoveElement(this);
        }
    }
}
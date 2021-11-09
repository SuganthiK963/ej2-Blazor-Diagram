using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DataVizCommon;
using System.Threading.Tasks;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class SvgSelectionRect
    {
        [CascadingParameter]
        private SvgSelectionRectCollection Parent { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public Rect DragRect { get; set; }

        [Parameter]
        public string Stroke { get; set; }

        [Parameter]
        public string StrokeWidth { get; set; }

        [Parameter]
        public string Fill { get; set; }

        [Parameter]
        public CloseOptions Close { get; set; }

        [Parameter]
        public EventCallback<Rect> DragRectChanged { get; set; }

        [Parameter]
        public EventCallback<CloseOptions> CloseChanged { get; set; }

        private string cursorStyle { get; set; } = "cursor:move";

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal bool IsDrawCloseIcon { get; set; }

        internal async Task ChangeRectangle(Rect rect)
        {
            DragRect = rect;
            IsDrawCloseIcon = false;
#pragma warning disable CA2007
            await DragRectChanged.InvokeAsync(DragRect);
        }

        internal async Task DrawCloseIcon(CircleOptions circleAttr, PathOptions pathAttr)
        {
            Close.Circle = circleAttr;
            Close.Path = pathAttr;
            IsDrawCloseIcon = true;
            await CloseChanged.InvokeAsync(Close);
#pragma warning restore CA2007
        }

        internal void ChangeCursor(string cursor)
        {
            cursorStyle = "cursor:" + cursor;
            InvokeAsync(StateHasChanged);
        }

        internal void RemoveCurrentElement()
        {
            IsDrawCloseIcon = false;
            InvokeAsync(StateHasChanged);
            Parent.RemoveElement(this);
        }
    }
}
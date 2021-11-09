using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class SvgSelectionRectCollection
    {
        [Parameter]
#pragma warning disable CA2227
        public List<SelectionOptions> SelectedRectangles { get; set; } = new List<SelectionOptions>();
#pragma warning restore CA2227

        internal List<SvgSelectionRect> RectsReference { get; set; } = new List<SvgSelectionRect>();

        internal List<SvgSelectionPath> PathsReference { get; set; } = new List<SvgSelectionPath>();

        private SvgSelectionRect componentRef
        {
            set { RectsReference.Add(value); }
        }

        private SvgSelectionPath componentRefPath
        {
            set { PathsReference.Add(value); }
        }

        internal void DrawNewRectangle(SelectionOptions rect)
        {
            SelectedRectangles.Add(rect);
            InvokeAsync(StateHasChanged);
        }

        internal void RemoveElement(SvgSelectionRect item)
        {
            SelectedRectangles.Remove(SelectedRectangles.Find(x => x.DragRect == item.DragRect));
            RectsReference.ForEach(item => item.IsDrawCloseIcon = true);
            RectsReference.Remove(RectsReference.Find(x => x.DragRect == item.DragRect));
            InvokeAsync(StateHasChanged);
        }

        internal void RemoveElement(SvgSelectionPath item)
        {
            SelectedRectangles.Remove(SelectedRectangles.Find(x => x.Path == item.Path));
            PathsReference.ForEach(item => item.IsDrawCloseIcon = true);
            PathsReference.Remove(PathsReference.Find(x => x.Id == item.Id));
            InvokeAsync(StateHasChanged);
        }

        internal void ClearElements()
        {
            SelectedRectangles.Clear();
            PathsReference.Clear();
            PathsReference.Clear();
            RectsReference.Clear();
            RectsReference.Clear();
            InvokeAsync(StateHasChanged);
        }
    }
}
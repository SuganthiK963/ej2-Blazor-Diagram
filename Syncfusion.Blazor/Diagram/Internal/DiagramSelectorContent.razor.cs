using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the selector objects that are rendered in the diagram.
    /// </summary>
    public partial class DiagramSelectorContent
    {
        [CascadingParameter]
        internal SfDiagramComponent Parent { get; set; }
        /// <summary>
        /// This method returns a boolean to indicate if a component’s UI can be rendered. 
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ShouldRender()
        {
            if (Parent.RealAction.HasFlag(RealAction.RefreshSelectorLayer))
            {
                return true;
            }
            else if ((Parent.RealAction.HasFlag(RealAction.PreventRefresh) || Parent.RealAction.HasFlag(RealAction.SymbolDrag) || (Parent.DiagramAction.HasFlag(DiagramAction.DrawingTool))) && !Parent.RealAction.HasFlag(RealAction.ScrollActions))
            {
                return false;
            }
            return true;
        }
    }
}

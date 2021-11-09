using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;


namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents how the diagram objects are rendered in the diagram. 
    /// </summary>
    public partial class DiagramContent
    {

        [CascadingParameter]
        internal SfDiagramComponent Parent { get; set; }

        internal DiagramLayerContent DiagramLayerContent;
        /// <summary>
        /// This method returns a boolean to indicate if a component’s UI can be rendered. 
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ShouldRender()
        {
            if (Parent.RealAction.HasFlag(RealAction.PreventRefresh) && !Parent.RealAction.HasFlag(RealAction.ScrollActions))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (Parent != null && DiagramLayerContent != null && Parent.DiagramContent == null)
            {
                Parent.DiagramContent = DiagramLayerContent;
            }
            if(!firstRender && Parent != null && Parent.DiagramContent != null && Parent.SnapSettings != null && Parent.SnapSettings.IsUpdateGridLine)
            {
                Parent.DiagramContent.UpdateGridlines();
                Parent.SnapSettings.IsUpdateGridLine = false;
            }
            return Task.CompletedTask;
        }
    }
}

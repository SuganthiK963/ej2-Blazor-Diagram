using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents how the basic HTML building blocks are rendered in a diagram.
    /// </summary>
    public partial class DiagramHtmlContainer
    {

        [CascadingParameter]
        internal SfDiagramComponent Parent { get; set; }
        /// <summary>
        /// This method returns a boolean to indicate if a component’s UI can be rendered. 
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ShouldRender()
        {
            if (Parent.RealAction.HasFlag(RealAction.PreventRefresh))
            {
                return false;
            }
            return true;
        }
    }
}

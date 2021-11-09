using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the frequently used commands that are rendered around the selector objects in the diagram.
    /// </summary>
    public partial class DiagramUserHandleContent
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

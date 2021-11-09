using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the vertical gridlines of the diagram.
    /// </summary>
    /// <remarks>
    /// VerticalGridlines provides the visual guidance while dragging or arranging the objects on the diagram surface.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfDiagramComponent Height="600px">
    ///     @* Shows vertical gridlines *@
    ///     <SnapSettings Constraints = "SnapConstraints.ShowLines" >
    ///         @* Customizes the line color and line style to the gridlines*@
    ///         <VerticalGridLines LineColor = "blue" LineDashArray="2,2" />        
    ///     </SnapSettings>
    /// </SfDiagramComponent>
    /// ]]>
    /// </code>
    /// </example>
    public class VerticalGridLines : GridLines
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            Parent.UpdateVerticalGridValues(this);
        }
        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any())
                Parent.UpdateVerticalGridValues(this);
        }
    }
}

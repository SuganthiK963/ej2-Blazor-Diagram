using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;


namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents a segment of UI content, implemented as a delegate that writes the content of a node.
    /// </summary>
    public partial class DiagramTemplates : SfBaseComponent
    {

        /// <summary>
        /// Represents a segment of UI content, implemented.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Represents a segment of the UI to be rendered for a node.
        /// </summary>  
        [Parameter]
        public RenderFragment<Node> NodeTemplate { get; set; }
        /// <summary>
        /// Represents a segment of the UI to be rendered for a Userhandle.
        /// </summary>
        [Parameter]
        public RenderFragment<UserHandle> UserHandleTemplate { get; set; }
        [CascadingParameter]
        internal SfDiagramComponent Parent { get; set; }
        internal static DiagramTemplates Initialize(DiagramTemplates template)
        {
            DiagramTemplates diagramTemplate = new DiagramTemplates();
            diagramTemplate.NodeTemplate = template.NodeTemplate ?? diagramTemplate.NodeTemplate;
            diagramTemplate.UserHandleTemplate = template.UserHandleTemplate ?? diagramTemplate.UserHandleTemplate;
            return diagramTemplate;
        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            Parent.UpdateDiagramTemplates(Initialize(this));
            await base.OnInitializedAsync();
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            ChildContent = null;
            if (NodeTemplate != null)
            {
                NodeTemplate = null;
            }

            if (UserHandleTemplate != null)
            {
                UserHandleTemplate = null;
            }

            if (Parent != null)
            {
                Parent = null;
            }
        }
    }
}
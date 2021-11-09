using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram.SymbolPalette
{
    /// <summary>
    /// Represents a segment of UI content, implemented as a delegate that writes the content of a Node.
    /// </summary>
    public partial class SymbolPaletteTemplates : SfBaseComponent
    {
        [CascadingParameter]
        internal SfSymbolPaletteComponent Parent { get; set; }

        /// <summary>
        /// Represents a segment of UI content, implemented.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// A NodeTemplate represents a segment of the UI to be rendered for a Node.
        /// </summary>  
        [Parameter]
        public RenderFragment<Node> NodeTemplate { get; set; }

       
        internal static SymbolPaletteTemplates Initialize(SymbolPaletteTemplates template)
        {
            SymbolPaletteTemplates symbolPaletteTemplate = new SymbolPaletteTemplates();
            symbolPaletteTemplate.NodeTemplate = template.NodeTemplate ?? symbolPaletteTemplate.NodeTemplate;
            return symbolPaletteTemplate;
        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            Parent.UpdateSymbolPaletteTemplates(Initialize(this));
            await base.OnInitializedAsync();
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            ChildContent = null;
            Parent = null;
            NodeTemplate = null;
        }
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Syncfusion.Blazor.Diagram.SymbolPalette;
namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the content of the palette.
    /// </summary>
    public partial class PaletteContent
    {
        private const string INITIALISE_MODULE = "sfBlazor.Diagram.initialiseModule";
        [CascadingParameter]
        internal SfSymbolPaletteComponent Parent { get; set; }
        /// <summary>
        /// This method returns a boolean to indicate if a component’s UI can be rendered. 
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ShouldRender()
        {
            if (this.Parent.SelectedSymbol != null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await Parent.JSRuntime.InvokeAsync<object>(INITIALISE_MODULE, Parent.SymbolPaletteContentRef, Parent.selfReference, Parent.AllowDrag).ConfigureAwait(true);

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}

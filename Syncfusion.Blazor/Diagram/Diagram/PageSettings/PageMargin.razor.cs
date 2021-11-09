using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the margin of the diagram page.
    /// </summary>
    public partial class PageMargin : DiagramMargin
    {
        [CascadingParameter]
        [JsonIgnore]
        internal PageSettings Parent { get; set; }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            Parent.UpdateMarginValues(this);
        }
        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            if (PropertyChanges.Any())
                Parent.UpdateMarginValues(this);
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (PropertyChanges.Any())
            {
                this.Parent.Parent.DiagramStateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
            if (Parent != null)
            {
                Parent = null;
            }
        }
    }
}
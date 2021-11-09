using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Defines the space between the viewport and the automatic layout. 
    /// </summary>
    public partial class LayoutMargin : DiagramMargin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutMargin"/>.
        /// </summary>
        public LayoutMargin()
        {
            Top = 50;
            Left = 50;
        }
        [CascadingParameter]
        [JsonIgnore]
        internal Layout Parent { get; set; }
        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            if (PropertyChanges.Any() || Parent.Parent.FirstRender)
            {
                Parent.UpdateMarginValues(this);
            }
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (PropertyChanges.Any() && !firstRender)
            {
                await this.Parent.Parent.DoLayout();
            }
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (Parent != null)
            {
                Parent = null;
            }
            base.Dispose();
        }
    }
}
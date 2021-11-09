using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the container area of the sparkline component.
    /// </summary>
    public partial class SparklineContainerArea
    {
        private string background;

        [CascadingParameter]
        internal ISparkline Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets the sparkline background color.
        /// </summary>
        [Parameter]
        public string Background { get; set; } = "transparent";

        internal SparklineContainerAreaBorder Border { get; set; } = new SparklineContainerAreaBorder();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.ContainerArea = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            background = NotifyPropertyChanges(nameof(Background), Background, background);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(SparklineContainerArea));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
        }
    }
}
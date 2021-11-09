using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the sparkline marker.
    /// </summary>
    public partial class SparklineMarkerSettings
    {
        private string fill;
        private double opacity;
        private double size;
        private List<VisibleType> visible;

        [CascadingParameter]
        internal ISparkline Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets the marker fill color.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = "#00bdae";

        /// <summary>
        /// Sets and gets the marker opacity.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Sets and gets the marker size.
        /// </summary>
        [Parameter]
        public double Size { get; set; } = 5;

        /// <summary>
        /// Sets and gets the marker visibility type.
        /// </summary>
        [Parameter]
        public List<VisibleType> Visible { get; set; }

        internal SparklineMarkerBorder Border { get; set; } = new SparklineMarkerBorder();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.MarkerSettings = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            size = NotifyPropertyChanges(nameof(Size), Size, size);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(SparklineMarkerSettings));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            Visible = visible = null;
        }
    }
}
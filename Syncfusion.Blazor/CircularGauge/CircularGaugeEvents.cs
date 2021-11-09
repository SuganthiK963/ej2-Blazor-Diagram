using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Specifies the events to be triggered in the Circular gauge component.
    /// </summary>
    public partial class CircularGaugeEvents : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the event to trigger it after the animation gets completed for pointers.
        /// </summary>
        [Parameter]
        public EventCallback<AnimationCompleteEventArgs> AnimationCompleted { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it before each annotation for the circular gauge gets rendered.
        /// </summary>
        [Parameter]
        public EventCallback<AnnotationRenderEventArgs> AnnotationRendering { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it before each axis label gets rendered.
        /// </summary>
        [Parameter]
        public EventCallback<AxisLabelRenderEventArgs> AxisLabelRendering { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it before the prints gets started.
        /// </summary>
        [Parameter]
        public EventCallback<PrintEventArgs> OnPrint { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it after the pointer is dragged.
        /// </summary>
        [Parameter]
        public EventCallback<PointerDragEventArgs> OnDragEnd { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it while dragging the pointers.
        /// </summary>
        [Parameter]
        public EventCallback<PointerDragEventArgs> OnDragMove { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it before the pointer is dragged.
        /// </summary>
        [Parameter]
        public EventCallback<PointerDragEventArgs> OnDragStart { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it before the circular gauge gets loaded.
        /// </summary>
        [Parameter]
        public EventCallback<LoadedEventArgs> OnLoad { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it after the circular gauge gets loaded.
        /// </summary>
        [Parameter]
        public EventCallback<LoadedEventArgs> Loaded { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it before the radius for the circular gauge gets calculated.
        /// </summary>
        [Parameter]
        public EventCallback<RadiusCalculateEventArgs> OnRadiusCalculate { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it on mouse down.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnGaugeMouseDown { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it while cursor leaves the circular gauge.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnGaugeMouseLeave { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it on hovering the circular gauge.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnGaugeMouseMove { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it on mouse up.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnGaugeMouseUp { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it after window resize.
        /// </summary>
        [Parameter]
        public EventCallback<ResizeEventArgs> Resizing { get; set; }

        /// <summary>
        /// Gets or sets the event to trigger it before the tooltip for pointer of the circular gauge gets rendered.
        /// </summary>
        [Parameter]
        public EventCallback<TooltipRenderEventArgs> TooltipRendering { get; set; }

        /// <summary>
        /// Gets or sets the properties of the Circular gauge.
        /// </summary>
        [CascadingParameter]
        protected SfCircularGauge Parent { get; set; }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.CircularGaugeEvents = this;
        }
    }
}

using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Configure handlers to handle the events of the Slider component.
    /// </summary>
    /// <typeparam name="TValue">"TValue".</typeparam>
    public partial class SliderEvents<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        internal SfSlider<TValue> Parent { get; set; }

        /// <summary>
        /// We can trigger change event whenever Slider value is changed.
        ///  In other term, this event will be triggered while drag the slider thumb.
        /// </summary>
        [Parameter]
        public EventCallback<SliderChangeEventArgs<TValue>> OnChange { get; set; }

        /// <summary>
        /// Fires whenever the Slider value is changed.
        /// In other term, this event will be triggered, while drag the slider thumb completed.
        /// </summary>
        [Parameter]
        public EventCallback<SliderChangeEventArgs<TValue>> ValueChange { get; set; }

        /// <summary>
        /// Triggers when the Slider is successfully created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the ticks are rendered on the Slider.
        /// </summary>
        [Parameter]
        public EventCallback<SliderTickRenderedEventArgs> TicksRendered { get; set; }

        /// <summary>
        /// Triggers on rendering the ticks element in the Slider,
        /// which is used to customize the ticks labels dynamically.
        /// </summary>
        [Parameter]
        public EventCallback<SliderTickEventArgs> TicksRendering { get; set; }

        /// <summary>
        /// Triggers when the Sider tooltip value is changed.
        /// </summary>
        [Parameter]
        public EventCallback<SliderTooltipEventArgs<TValue>> OnTooltipChange { get; set; }

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override Task OnInitializedAsync()
        {
            if (Parent != null)
            {
                Parent.SliderEvents = this;
            }

            return base.OnInitializedAsync();
        }
    }
}

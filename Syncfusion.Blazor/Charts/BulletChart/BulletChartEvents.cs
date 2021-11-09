using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the events to be triggered in the bullet chart component.
    /// </summary>
    public class BulletChartEvents : SfBaseComponent
    {
        [CascadingParameter]
        internal IBulletChart Parent { get; set; }

        /// <summary>
        /// Triggers before the prints gets started.
        /// </summary>
        [Parameter]
        public EventCallback<PrintEventArgs> OnPrintComplete { get; set; }

        /// <summary>
        /// Triggers while mouse to be click.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<BulletChartMouseEventArgs> OnBulletChartMouseClick { get; set; }

        /// <summary>
        /// Triggers before the bulletchart component is rendered.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<BulletChartLoadedEventArgs> Load { get; set; }

        /// <summary>
        /// Triggers after the bulletchart component is rendered.
        /// </summary>
        public EventCallback<System.EventArgs> Loaded { get; set; }

        /// <summary>
        /// Triggers before the bulletchart tooltip is rendered.
        /// </summary>
        [Parameter]
        public EventCallback<BulletChartTooltipEventArgs> TooltipRender { get; set; }

        /// <summary>
        /// Triggers before the bulletchart legend is rendered.
        /// </summary>
        [Parameter]
        public EventCallback<BulletChartLegendRenderEventArgs> LegendRender { get; set; }

        /// <summary>
        /// Triggers before the bulletchart label render.
        /// </summary>
        [Parameter]
        public Action<BulletChartLabelRenderEventArgs> OnLabelRender { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Events = this;
        }
    }
}
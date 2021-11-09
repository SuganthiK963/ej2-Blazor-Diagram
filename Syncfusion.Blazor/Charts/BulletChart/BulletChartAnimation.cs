using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the animation options of bullet chart component.
    /// </summary>
    public class BulletChartAnimation : SfBaseComponent
    {
        [CascadingParameter]
        internal IBulletChart Parent { get; set; }

        /// <summary>
        /// Sets and gets the option to delay animation of the series.
        /// </summary>
        [Parameter]
        public int Delay { get; set; }

        /// <summary>
        /// Sets and gets the duration of animation in milliseconds.
        /// </summary>
        [Parameter]
        public int Duration { get; set; } = 1000;

        /// <summary>
        /// Sets and gets to enable the animation.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Animation = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
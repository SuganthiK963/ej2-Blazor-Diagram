using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Linq;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets animation of thumb for the range slider in range navigator.
    /// </summary>
    public partial class RangeNavigatorAnimation : SfBaseComponent
    {
        private double delay;
        private double duration;
        private bool enable;

        [CascadingParameter]
        internal RangeNavigatorSeries Series { get; set; }

        /// <summary>
        /// The option to delay animation of the series.
        /// </summary>
        [Parameter]
        public double Delay { get; set; }

        /// <summary>
        /// The duration of animation in milliseconds.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 1000;

        /// <summary>
        /// If set to true, series gets animated on initial loading.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Series.UpdateSeriesProperties("Animation", this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            delay = NotifyPropertyChanges(nameof(Delay), Delay, delay);
            enable = NotifyPropertyChanges(nameof(Enable), Enable, enable);
            duration = NotifyPropertyChanges(nameof(Duration), Duration, duration);
            if (PropertyChanges.Any() && IsRendered)
            {
                SfBaseUtils.UpdateDictionary(nameof(Series.Animation), this, Series.PropertyChanges);
                PropertyChanges.Clear();
                await Series.SeriesPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            Series = null;
        }
    }
}
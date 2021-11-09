using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Sparkline.Internal;
using System.Globalization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.Sparkline.Internal
{
    /// <summary>
    /// Specifies the rendering of trackline in sparkline component.
    /// </summary>
    /// <typeparam name="TValue">Represents the generic data type of trackline in sparkline component.</typeparam>
    public partial class SparklineTrackline<TValue>
    {
        private string trackPath = string.Empty;
        private string trackColor = string.Empty;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        [CascadingParameter]
        internal SfSparkline<TValue> Parent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (Parent.TooltipSettings != null)
            {
                Parent.Trackline = this;
            }
        }

        internal void RenderTrackline(SparklineValues sparklineValues, Size availableSize, SparklinePadding padding)
        {
            double point = sparklineValues.Location.X;
            trackColor = string.IsNullOrEmpty(Parent.TooltipSettings.TrackLineSettings.Color) ? Parent.ThemeStyle.TrackerLine : Parent.TooltipSettings.TrackLineSettings.Color;
            trackPath = "M " + point.ToString(culture) + " " + padding.Top.ToString(culture) + " L " + point.ToString(culture) + " " + (availableSize.Height - padding.Bottom).ToString(culture);
            StateHasChanged();
        }

        internal void RemoveTrackline()
        {
            if (Parent.TooltipSettings.TrackLineSettings != null && Parent.TooltipSettings.TrackLineSettings.Visible && !string.IsNullOrEmpty(trackPath))
            {
                trackPath = string.Empty;
                StateHasChanged();
            }
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent = null;
        }
    }
}
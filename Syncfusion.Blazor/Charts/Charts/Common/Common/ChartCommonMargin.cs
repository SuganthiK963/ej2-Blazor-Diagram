using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the bottom, left, right, top margin of the chart component.
    /// </summary>
    public partial class ChartCommonMargin : SfBaseComponent
    {
        /// <summary>
        /// Sets and gets the bottom margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Bottom { get; set; } = 10;

        private double bottom { get; set; } = 10;

        /// <summary>
        /// Sets and gets the left margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Left { get; set; } = 10;

        private double left { get; set; } = 10;

        /// <summary>
        /// Sets and gets the right margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Right { get; set; } = 10;

        private double right { get; set; } = 10;

        /// <summary>
        /// Sets and gets the top margin for the chart component.
        /// </summary>
        [Parameter]
        public virtual double Top { get; set; } = 10;

        private double top { get; set; } = 10;

        protected override async Task OnParametersSetAsync()
        {
#pragma warning disable CA2007
            await base.OnParametersSetAsync();
#pragma warning restore CA2007
            Bottom = bottom = NotifyPropertyChanges(nameof(Bottom), Bottom, bottom);
            Top = top = NotifyPropertyChanges(nameof(Top), Top, top);
            Left = left = NotifyPropertyChanges(nameof(Left), Left, left);
            Right = right = NotifyPropertyChanges(nameof(Right), Right, right);
        }
    }
}
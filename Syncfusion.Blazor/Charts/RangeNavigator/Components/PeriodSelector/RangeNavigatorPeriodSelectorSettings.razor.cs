using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the periodselector for range navigator.
    /// </summary>
    public partial class RangeNavigatorPeriodSelectorSettings
    {
        private double height;
        private PeriodSelectorPosition position;

        [CascadingParameter]
        internal SfRangeNavigator Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Height for the period selector.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 43;

        internal List<RangeNavigatorPeriod> Periods { get; set; } = new List<RangeNavigatorPeriod>();

        /// <summary>
        /// vertical position of the period selector.
        /// </summary>
        [Parameter]
        public PeriodSelectorPosition Position { get; set; } = PeriodSelectorPosition.Bottom;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("PeriodSelectorSettings", this);
        }

        internal new async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            if (PropertyChanges.Any() && IsRendered)
            {
                SfBaseUtils.UpdateDictionary("PeriodSelectorSettings", this, Parent.PropertyChanges);
                PropertyChanges.Clear();
                await Parent.OnRangeNaivgatorParametersSet();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Periods = null;
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the periods that are to be added to the periodselector of the range navigator.
    /// </summary>
    public partial class RangeNavigatorPeriods
    {
        [CascadingParameter]
        internal RangeNavigatorPeriodSelectorSettings Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal void UpdateChildProperty(RangeNavigatorPeriod keyValue)
        {
            Parent.Periods.Add(keyValue);
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                SfBaseUtils.UpdateDictionary("Periods", Parent.Periods, Parent.PropertyChanges);
                PropertyChanges.Clear();
                await Parent.OnParametersSetAsync();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}
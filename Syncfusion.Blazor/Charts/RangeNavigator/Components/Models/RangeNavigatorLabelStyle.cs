using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options to customize the label style for range navigator.
    /// </summary>
    public class RangeNavigatorLabelStyle : ChartCommonFont
    {
        [CascadingParameter]
        internal SfRangeNavigator Parent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("LabelStyle", this);
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
                SfBaseUtils.UpdateDictionary("LabelStyle", this, Parent.PropertyChanges);
                PropertyChanges.Clear();
                await Parent.OnRangeNaivgatorParametersSet();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the text style in tooltip of the bullet chart component.
    /// </summary>
    public class BulletChartTooltipTextStyle : BulletChartCommonFont
    {
        [CascadingParameter]
        internal IBulletChartTooltip Parent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("TextStyle", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
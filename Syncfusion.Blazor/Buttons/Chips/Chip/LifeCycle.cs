using Syncfusion.Blazor.Internal;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Partial Class SfChip.
    /// </summary>
    public partial class SfChip : SfBaseComponent
    {
        /// <summary>
        /// Method gets invoked when the component is ready to start.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (SelectedChips.Length > 1 && Selection == SelectionType.Single)
            {
                string[] singleType = new string[] { SelectedChips[SelectedChips.Length - 1] };
                SelectedChips = singleType;
            }

            UpdateAttributes();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>="Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender || IsRefresh)
            {
                IsRefresh = false;
                StateHasChanged();
                if (firstRender)
                {
                    await SfBaseUtils.InvokeEvent<object>(ChipEvents?.Created, null);
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
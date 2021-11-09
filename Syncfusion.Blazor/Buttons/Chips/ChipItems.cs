using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Configures the collection of chipitem.
    /// </summary>
    public partial class ChipItems : SfBaseComponent
    {
        /// <summary>
        /// Indicates the SfChip component.
        /// </summary>
        [CascadingParameter]
        internal SfChip BaseParent { get; set; }

        /// <summary>
        /// Indicates the ChildContent.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        // This chips property helps to render chipitem component.

        /// <summary>
        /// Indicates the Chips List.
        /// </summary>
        public List<ChipItem> Chips { get; set; } = new List<ChipItem>();

        /// <summary>
        /// Updates the Chips property and returns the count.
        /// </summary>
        internal void UpdateChildProperty(ChipItem chipValue)
        {
            Chips.Add(chipValue);
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.UpdateChips(Chips);
        }

        internal override void ComponentDispose()
        {
            Chips = null;
            BaseParent = null;
        }
    }
}
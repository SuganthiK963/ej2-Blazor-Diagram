using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Partial Class SfChip.
    /// </summary>
    public partial class SfChip : SfBaseComponent
    {
        /// <summary>
        /// A function that finds chip based on given chip value.
        /// </summary>
        /// <param name="chipValue">Value of the chip need to be passed to get the corresponding chip model.</param>
        /// <returns>GetItem.</returns>
        public ChipItem FindChip(string chipValue)
        {
            return GetItem(chipValue);
        }

        /// <summary>
        /// A function that returns selected chips data.
        /// </summary>
        /// <returns>selectedChipModels.</returns>
        public List<ChipItem> GetSelectedChips()
        {
            List<ChipItem> selectedChipModels = new List<ChipItem>();
            if (SfBaseUtils.IsNotNullOrEmpty(Chips))
            {
                foreach (ChipItem item in Chips)
                {
                    if (Array.IndexOf(SelectedChips, item.Value) >= 0)
                    {
                        selectedChipModels.Add(item);
                    }
                }
            }

            return selectedChipModels;
        }

        /// <summary>
        /// A function that removes chip items based on given chip values.
        /// </summary>
        /// <param name="removableChipValues">Values of the chips which are to be removed should be passed.</param>
        public void RemoveChips(string[] removableChipValues)
        {
            if (removableChipValues != null)
            {
                ChipItem item;
                foreach (string chipValue in removableChipValues)
                {
                    item = GetItem(chipValue);
                    if (item != null)
                    {
                        Chips.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// A function that selects chip items based on given index.
        /// </summary>
        /// <param name="selectableChips">Values of the chips which are to be selected should be passed.</param>
        public void SelectChips(string[] selectableChips)
        {
            if (selectableChips != null && Selection != SelectionType.None)
            {
                ChipItem item;
                foreach (string chipValue in selectableChips)
                {
                    item = GetItem(chipValue);
                    if (item != null && Array.IndexOf(SelectedChips, item.Value) < 0)
                    {
                        SelectedChips = SfBaseUtils.AddArrayValue(SelectedChips, item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// A function that adds chip items based on given input.
        /// </summary>
        /// <param name="item">Chip Item to be added should be passed.</param>
        public void AddChip(ChipItem item)
        {
            if (item != null)
            {
                if (!SfBaseUtils.IsNotNullOrEmpty(Chips))
                {
                    Chips = new List<ChipItem>();
                }

                Chips.Add(item);
                StateHasChanged();
            }
        }
    }
}

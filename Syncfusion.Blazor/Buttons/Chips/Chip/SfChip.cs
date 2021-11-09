using System;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// The SfChip component displays a list of chip each of which contains a block of essential information that triggers an event on click action.
    /// </summary>
    public partial class SfChip : SfBaseComponent
    {
        internal const string PARENTCLASS = "e-chip-set";
        internal const string TRUE = "true";
        internal const string RTL = " e-rtl";
        internal const string FALSE = "false";
        internal const string CHIPSELECTION = " e-selection";
        internal const string MULTISELECTION = " e-multi-selection";
        internal const string ARIAMULTISELECT = "aria-multiselectable";
        internal const string CHIP = "e-chip";
        internal const string CHIPTEXT = "e-chip-text";
        internal const string CHIPDELETE = "e-chip-delete";
        internal const string CHIPICON = "e-chip-icon";
        internal const string CHIPAVATAR = "e-chip-avatar";
        internal const string IMGURL = "image-url";
        internal const string TRIALURL = "trailing-icon-url";
        internal const string DELBTN = "e-dlt-btn";
        internal const string AVATARWRAP = " e-chip-avatar-wrap";
        internal const string ICONWRAP = " e-chip-icon-wrap";
        internal const string DISABLED = " e-disabled";
        internal const string ACTIVE = " e-active";
        internal const string FOCUSED = " e-focused";
        internal const string ROOTAVATAR = "e-leading-avatar";
        internal const string SPACE = " ";
        internal const string EMPTY = "";

        private Dictionary<string, object> attributes = new Dictionary<string, object>();

        private Dictionary<string, object> chipAttributes = new Dictionary<string, object>();

        private ElementReference[] elementRefs = Array.Empty<ElementReference>();

        // Used to update the SelectedChips property.
        private string[] _selectedChips { get; set; }

        private bool isDeleteCalled;

        private bool restrictClick;

        private bool isDestroyed;

        private int? focussedIndex;

        private string textValue;
        private string ariaSelected = FALSE;
        private string chipClass;
        private string value;

        internal bool IsRefresh { get; set; }

        internal ChipEvents ChipEvents { get; set; }

        /// <summary>
        /// Updates the child properties of the component.
        /// </summary>
        internal void UpdateChips(List<ChipItem> propValue)
        {
            Chips = propValue;
        }

        /// <summary>
        /// Re-renders the complete chip component.
        /// </summary>
        internal async Task RefeshComponent()
        {
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Updates the class and multiselection attributes of the chip root element.
        /// </summary>
        protected void UpdateAttributes()
        {
            string rootClass = "e-chip-list e-control e-lib";
            string multiSelection = FALSE;
            if (Selection == SelectionType.Single)
            {
                rootClass = rootClass + CHIPSELECTION;
            }
            else if (Selection == SelectionType.Multiple)
            {
                rootClass = rootClass + MULTISELECTION;
                multiSelection = TRUE;
            }

            if (EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl))
            {
                rootClass = rootClass + RTL;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                rootClass = rootClass + " " + CssClass;
            }

            attributes = SfBaseUtils.GetAttribtues(rootClass, new Dictionary<string, object>(HtmlAttributes != null ? HtmlAttributes : new Dictionary<string, object>()));
            SfBaseUtils.UpdateDictionary(ARIAMULTISELECT, multiSelection, attributes);
        }

        internal async Task UpdateSelectedChips()
        {
            SelectedChips = await SfBaseUtils.UpdateProperty<string[]>(SelectedChips.ToArray(), _selectedChips, SelectedChipsChanged);
        }

        /// <summary>
        /// Returns a ChipItem based on a value of a chip.
        /// </summary>
        private ChipItem GetItem(string chipValue)
        {
            ChipItem item = null;
            if (SfBaseUtils.IsNotNullOrEmpty(Chips))
            {
                IEnumerable<ChipItem> chipInstance = Chips.Where(x => x.Value == chipValue);
                if (chipInstance.Any())
                {
                    item = chipInstance?.First();
                }
            }

            return item;
        }

        /// <summary>
        /// Updates the chip elements count.
        /// </summary>
        private void UpdateRefsCount(int count)
        {
            elementRefs = new ElementReference[count];
        }

        /// <summary>
        /// Updates the text, icons url/class and attributes of each chip if multiple chip rendered.
        /// </summary>
        private void GetAttributes(int index)
        {
            ChipItem chip = Chips[index];
            chipClass = CHIP;
            ariaSelected = FALSE;
            if (!string.IsNullOrEmpty(chip.LeadingText) || !string.IsNullOrEmpty(chip.LeadingIconCss) || !string.IsNullOrEmpty(chip.LeadingIconUrl))
            {
                chipClass = chipClass + (CssClass.IndexOf(ROOTAVATAR, StringComparison.CurrentCulture) < 0 ? ICONWRAP : AVATARWRAP);
            }

            if (!chip.Enabled || !Enabled)
            {
                chipClass = chipClass + DISABLED;
            }

            if (Chips != null && SelectedChips != null)
            {
                if (Array.IndexOf(SelectedChips, chip.Value) >= 0)
                {
                    chipClass = chipClass + ACTIVE;
                    ariaSelected = TRUE;
                }
            }

            textValue = !string.IsNullOrEmpty(chip.Text) ? chip.Text : EMPTY;
            value = !string.IsNullOrEmpty(chip.Value) ? chip.Value : chip.Text;
            chipClass += !string.IsNullOrEmpty(chip.CssClass) ? (SPACE + chip.CssClass) : EMPTY;
            chipClass += (focussedIndex != null && index == focussedIndex) ? FOCUSED : EMPTY;
            UpdateAttributes(chip.HtmlAttributes);
        }

        /// <summary>
        /// Updates the attributes value.
        /// </summary>
        /// <param name="htmlAttributes">The htmlAttributes.</param>
        protected void UpdateAttributes(Dictionary<string, object> htmlAttributes = null)
        {
            chipAttributes = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("class", chipClass, chipAttributes);
            SfBaseUtils.UpdateDictionary("tabindex", "0", chipAttributes);
            SfBaseUtils.UpdateDictionary("role", "option", chipAttributes);
            SfBaseUtils.UpdateDictionary("aria-label", textValue, chipAttributes);
            SfBaseUtils.UpdateDictionary("aria-selected", ariaSelected, chipAttributes);
            SfBaseUtils.UpdateDictionary("data-value", value, chipAttributes);
            if (htmlAttributes != null)
            {
                foreach (string key in htmlAttributes.Keys)
                {
                    if (key == "class")
                    {
                        SfBaseUtils.UpdateDictionary("class", SfBaseUtils.AddClass(chipAttributes["class"].ToString(), htmlAttributes[key].ToString()), chipAttributes);
                    }
                    else
                    {
                        chipAttributes[key] = htmlAttributes[key];
                    }
                }
            }
        }

        /// <summary>
        /// Delete click event handler.
        /// </summary>
        private async Task OnDeleteClick(int index, bool isKeyboardInteracted)
        {
            if (!restrictClick)
            {
                ChipItem model = Chips?.ElementAt(index);
                ChipEventArgs deleteEventArgs = GetEventArgs(index);
                await SfBaseUtils.InvokeEvent<ChipEventArgs>(ChipEvents?.OnDelete, deleteEventArgs);
                if (!deleteEventArgs.Cancel)
                {
                    isDeleteCalled = true;
                    if (Array.IndexOf(SelectedChips, model.Value) >= 0)
                    {
                        SelectedChips = SfBaseUtils.RemoveArrayValue(SelectedChips, model.Value);
                    }

                    Chips.Remove(model);
                }
            }

            RemoveFocus(index, isKeyboardInteracted);
        }

        private void RemoveFocus(int index, bool isKeyboardInteracted)
        {
            if (Selection != SelectionType.None && !isKeyboardInteracted && focussedIndex == index)
            {
                OnChipFocusOut();
            }
        }

        /// <summary>
        /// Chip element mouse down event handler.
        /// </summary>
        private async Task OnChipMouseDown(int index, bool isKeyboardInteracted)
        {
            ChipItem item = Chips.ElementAtOrDefault(index);
            if (item != null && item.Enabled && Enabled && !isDeleteCalled)
            {
                ChipEventArgs beforeClickArgs = GetEventArgs(index);
                await SfBaseUtils.InvokeEvent<ChipEventArgs>(ChipEvents?.OnBeforeClick, beforeClickArgs);
                restrictClick = beforeClickArgs.Cancel;
            }

            RemoveFocus(index, isKeyboardInteracted);
        }

        /// <summary>
        /// Chip element click event handler.
        /// </summary>
        private async Task OnChipClick(int index, bool isKeyboardInteracted)
        {
            ChipItem item = Chips?.ElementAtOrDefault(index);
            if (item != null && item.Enabled && Enabled)
            {
                if (!isDeleteCalled && !restrictClick)
                {
                    if (Selection == SelectionType.Single)
                    {
                        SelectedChips = (SelectedChips.Length > 0 && SelectedChips.ElementAt(0) == item.Value) ? Array.Empty<string>() : new string[] { item.Value };
                    }
                    else if (Selection == SelectionType.Multiple)
                    {
                        if (SelectedChips.Length > 0 && Array.IndexOf(SelectedChips, item.Value) >= 0)
                        {
                            SelectedChips = SfBaseUtils.RemoveArrayValue(SelectedChips, item.Value);
                        }
                        else if ((SelectedChips.Length > 0 && Array.IndexOf(SelectedChips, index) < 0) || SelectedChips.Length == 0)
                        {
                            SelectedChips = SfBaseUtils.AddArrayValue(SelectedChips, item.Value);
                        }
                    }
                    await UpdateSelectedChips();
                    await SfBaseUtils.InvokeEvent<ChipEventArgs>(ChipEvents?.OnClick, GetEventArgs(index));
                }
                else
                {
                    isDeleteCalled = false;
                    restrictClick = false;
                }
            }

            if (Chips != null)
            {
                RemoveFocus(index, isKeyboardInteracted);
            }
        }

        /// <summary>
        /// Returns the click event arguments.
        /// </summary>
        private ChipEventArgs GetEventArgs(int index)
        {
            ChipItem model = Chips.ElementAtOrDefault(index);
            ChipEventArgs clickEventArgs = new ChipEventArgs
            {
                Cancel = false,
                Element = elementRefs[index],
                Index = index,
                Selected = model != null ? Array.IndexOf(SelectedChips, model.Value) >= 0 : false,
                Text = model?.Text,
                Value = model?.Value
            };
            return clickEventArgs;
        }

        /// <summary>
        /// Chip element focus out handler.
        /// </summary>
        private void OnChipFocusOut()
        {
            focussedIndex = null;
        }

        /// <summary>
        /// Chip element focus handler.
        /// </summary>
        private void OnChipFocus(int index)
        {
            focussedIndex = index;
        }

        /// <summary>
        /// Chip element key down handler.
        /// </summary>
        private async Task OnKeyDown(int index, KeyboardEventArgs args)
        {
            if (SfBaseUtils.IsNotNullOrEmpty(Chips) && Chips[index] != null && Chips[index].Enabled && Enabled)
            {
                if (args.Code == "Delete" && EnableDelete)
                {
                    await OnDeleteClick(index, true);
                }
                else if (args.Code == "Enter" || args.Code == "Space")
                {
                    await OnChipMouseDown(index, true);
                    await OnChipClick(index, true);
                }
            }
        }

        /// <summary>
        /// Component dispose handled.
        /// </summary>
        internal override async void ComponentDispose()
        {
            if (IsRendered && !isDestroyed)
            {
                try
                {
                    await SfBaseUtils.InvokeEvent<object>(ChipEvents?.Destroyed, null);
                    isDestroyed = true;
                    Chips = null;
                    attributes = null;
                    ChipEvents = null;
                }
                catch (Exception e)
                {
                        await SfBaseUtils.InvokeEvent<object>(ChipEvents.Destroyed, e);
                        throw new InvalidOperationException(e.Message);
                }
            }
        }
    }
}

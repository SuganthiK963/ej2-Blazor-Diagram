using System;
using System.Threading.Tasks;
using Syncfusion.Blazor.Buttons;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Partial Class ChipItem.
    /// </summary>
    public partial class ChipItem : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
            ChipsCssClass = CssClass;
            ChipsEnabled = Enabled;
            ChipsLeadingIconCss = LeadingIconCss;
            ChipsLeadingText = LeadingText;
            ChipsText = Text;
            ChipsTrailingIconCss = TrailingIconCss;
            ChipsValue = Value = string.IsNullOrEmpty(Value) ? Text : Value;
            await BaseParent.RefeshComponent();
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            ChipsCssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, ChipsCssClass);
            ChipsEnabled = NotifyPropertyChanges(nameof(Enabled), Enabled, ChipsEnabled);
            ChipsLeadingIconCss = NotifyPropertyChanges(nameof(LeadingIconCss), LeadingIconCss,ChipsLeadingIconCss);
            ChipsLeadingText = NotifyPropertyChanges(nameof(LeadingText), LeadingText, ChipsLeadingText);
            ChipsText = NotifyPropertyChanges(nameof(Text), Text,ChipsText);
            ChipsTrailingIconCss = NotifyPropertyChanges(nameof(TrailingIconCss), TrailingIconCss, ChipsTrailingIconCss);
            ChipsValue = NotifyPropertyChanges(nameof(Value), Value, ChipsValue);
            if (PropertyChanges.Count > 0)
            {
                BaseParent.IsRefresh = true;
            }
        }
    }
}
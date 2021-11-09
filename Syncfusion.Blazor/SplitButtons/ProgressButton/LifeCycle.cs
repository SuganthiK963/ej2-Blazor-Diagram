using System;
using System.Threading.Tasks;
using System.Globalization;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// ProgressButton visualizes the progression of an operation to indicate the user that a process is happening in the background with visual representation.
    /// </summary>
    public partial class SfProgressButton : SfBaseComponent
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (!string.IsNullOrEmpty(IconCss))
            {
                iconCss = BTNICON;
                if (!string.IsNullOrEmpty(Content))
                {
                    iconCss += " e-icon-" + IconPosition.ToString().ToLower(CultureInfo.CurrentCulture);
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (buttonCss.Contains(ACTIVE, StringComparison.Ordinal))
            {
                buttonCss = PROGRESSBTN + SPACE + ACTIVE + SPACE + CssClass + SPACE + spinPos;
            }
            else
            {
                buttonCss = PROGRESSBTN + SPACE + CssClass + SPACE + spinPos;
            }

            isVertical = buttonCss.Contains(VERTICAL, StringComparison.Ordinal);
            isHide = buttonCss.Contains(HIDE, StringComparison.Ordinal);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = "Created" });
                spinPos = spinSettings != null ? SPACE + SPIN + spinSettings.Position.ToString().ToLower(CultureInfo.CurrentCulture) : SPACE + LEFT;
                buttonCss += spinPos;
                if (spinSettings != null)
                {
                    spinWidth = spinSettings.Width;
                }

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}

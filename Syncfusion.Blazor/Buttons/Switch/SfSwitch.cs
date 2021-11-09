using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// The Switch is a graphical user interface element that allows you to toggle between checked and unchecked states.
    /// </summary>
    public partial class SfSwitch<TChecked> : SfInputBase<TChecked>
    {
        private const string SPACE = " ";
        private const string RTL = "e-rtl";
        private const string ROLE = "switch";
        private const string CHECK = "check";
        private const string FOCUS = "e-focus";
        private const string UNCHECK = "uncheck";
        private const string INNER = "e-switch-inner";
        private const string HANDLE = "e-switch-handle";
        private const string ACTIVE = " e-switch-active";
        private const string RIPPLE = "e-ripple-container";
        private const string DISABLED = "e-switch-disabled";
        private const string RIPPLE_CHECK = "e-ripple-check";
        private const string SWITCH = "e-switch-wrapper e-wrapper";
        private const string POINTEREVENTS = "pointer-events: auto";
        private bool isDrag;
        private bool isFocused;
        private string rootClass;
        private string innerClass;
        private string rippleClass;
        private string handleClass;
        private bool preventDefault;

        /// <summary>
        /// Specifies a text that indicates the Switch is in checked state.
        /// </summary>
        [Parameter]
        public string OnLabel { get; set; }

        /// <summary>
        /// Specifies a text that indicates the Switch is in unchecked state.
        /// </summary>
        [Parameter]
        public string OffLabel { get; set; }

        /// <summary>
        /// Triggers when Switch state has been changed by user interaction.
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs<TChecked>> ValueChange { get; set; }

        protected override void InitRender()
        {
            rootClass = SWITCH;
            rippleClass = RIPPLE;
            ChangeState(Convert.ToBoolean(Checked, CultureInfo.InvariantCulture) ? CHECK : UNCHECK);
            if (!string.IsNullOrEmpty(CssClass))
            {
                rootClass += SPACE + CssClass;
            }

            if (Disabled)
            {
                rootClass += SPACE + DISABLED;
            }
            else
            {
                rootClass = rootClass.Replace(SPACE + DISABLED, string.Empty, StringComparison.Ordinal);
            }

            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                rootClass += SPACE + RTL;
            }
            else
            {
                rootClass = rootClass.Replace(SPACE + RTL, string.Empty, StringComparison.Ordinal);
            }
        }

        private async Task OnClickHandler(MouseEventArgs args)
        {
            if (!Disabled)
            {
                TChecked state = default;
                if (Convert.ToBoolean(Checked, CultureInfo.InvariantCulture))
                {
                    ChangeState(UNCHECK);
                    state = (TChecked)(object)false;
                }
                else
                {
                    ChangeState(CHECK);
                    state = (TChecked)(object)true;
                }

                await UpdateCheckState(state);
                if (EnablePersistence)
                {
                    await SetLocalStorage(idValue, Checked);
                }
                await SfBaseUtils.InvokeEvent(ValueChange, new ChangeEventArgs<TChecked> { Checked = Checked, Event = args });
            }
        }

        private void OnMouseDownHandler(MouseEventArgs args)
        {
            if (args.Type == "mousedown")
            {
                isDrag = true;
                isFocused = false;
            }
        }

        private async Task OnTouchHandler(TouchEventArgs args)
        {
            if (args.Type == "touchstart")
            {
                isDrag = true;
            }

            if (isDrag && (args.Type == "mouseup" || args.Type == "touchend"))
            {
                await OnClickHandler(null);
                preventDefault = true;
            }
        }

        private void OnKeyupHandler()
        {
            if (isFocused && rootClass.IndexOf(FOCUS, StringComparison.Ordinal) < 0)
            {
                rootClass += SPACE + FOCUS;
            }
        }

        private void OnFocusHandler()
        {
            isFocused = true;
        }

        private void OnFocusOutHandler()
        {
            rootClass = rootClass.Replace(SPACE + FOCUS, string.Empty, StringComparison.Ordinal);
            isFocused = false;
        }

        private void ChangeState(string state)
        {
            innerClass = INNER;
            handleClass = HANDLE;
            if (state == CHECK)
            {
                innerClass += ACTIVE;
                handleClass += ACTIVE;
                if (SyncfusionService.options.EnableRippleEffect && rippleClass.IndexOf(SPACE + RIPPLE_CHECK, StringComparison.Ordinal) < 0)
                {
                    rippleClass += SPACE + RIPPLE_CHECK;
                }
            }
            else
            {
                if (SyncfusionService.options.EnableRippleEffect)
                {
                    rippleClass = rippleClass.Replace(SPACE + RIPPLE_CHECK, string.Empty, StringComparison.Ordinal);
                }
            }
        }
    }
}
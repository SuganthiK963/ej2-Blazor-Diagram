using System;
using System.Globalization;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// The CheckBox is a graphical user interface element that allows you to select one or more options from the choices.
    /// It contains checked, unchecked, and indeterminate states.
    /// </summary>
    public partial class SfCheckBox<TChecked> : SfInputBase<TChecked>
    {
        private const string SPACE = " ";
        private const string RTL = "e-rtl";
        private const string TRUE = "true";
        private const string FALSE = "false";
        private const string CHECK = "check";
        private const string MIXED = "mixed";
        private const string LABEL = "e-label";
        private const string FOCUS = "e-focus";
        private const string UNCHECK = "uncheck";
        private const string CHECKBOX = "checkbox";
        private const string CHECK_CLASS = "e-check";
        private const string RIPPLE = "e-ripple-container";
        private const string INDETERMINATE_CLASS = "e-stop";
        private const string FRAME_CLASS = "e-icons e-frame";
        private const string RIPPLE_CHECK = "e-ripple-check";
        private const string DISABLED = "e-checkbox-disabled";
        private const string ROOT = "e-control e-checkbox e-lib";
        private const string RIPPLE_INDETERMINATE = "e-ripple-stop";
        private const string CHECKBOX_CLASS = "e-checkbox-wrapper e-wrapper";
        private const string POINTEREVENTS = "pointer-events: auto";
        private bool isFocused;
        private string frameClass;
        private string rippleClass;
        private string ariaChecked;
        private string checkboxClass;
        private Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        /// <summary>
        /// Defines the caption for the CheckBox, that describes the purpose of the CheckBox.
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Positions label before/after the CheckBox.
        /// The possible values are:
        /// - Before - The label is positioned to left of the CheckBox.
        /// - After - The label is positioned to right of the CheckBox.
        /// </summary>
        [Parameter]
        public LabelPosition LabelPosition { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the CheckBox is in indeterminate state or not.
        /// When set to true, the CheckBox will be in indeterminate state.
        /// </summary>
        [Parameter]
        public bool Indeterminate { get; set; }

        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCallback<bool> IndeterminateChanged { get; set; }

        /// <summary>
        /// Specifies a value to enable/disable tri state functionality in CheckBox.
        /// </summary>
        [Parameter]
        public bool EnableTriState { get; set; }

        /// <summary>
        /// Triggers when the CheckBox state has been changed by user interaction.
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs<TChecked>> ValueChange { get; set; }

        protected override void InitRender()
        {
            checkboxClass = CHECKBOX_CLASS;
            var isChecked = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture);
            UIChange(isChecked ? CHECK : UNCHECK);
            if (EnableTriState && isChecked == false)
            {
                Indeterminate = false;
            }

            if (Indeterminate || (EnableTriState && Checked == null))
            {
                Indeterminate = true;
                UIChange();
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                checkboxClass += SPACE + CssClass;
            }

            if (Disabled)
            {
                checkboxClass += SPACE + DISABLED;
            }
            else
            {
                checkboxClass = checkboxClass.Replace(DISABLED, string.Empty, StringComparison.Ordinal);
            }

            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                checkboxClass += SPACE + RTL;
            }
            else
            {
                checkboxClass = checkboxClass.Replace(RTL, string.Empty, StringComparison.Ordinal);
            }
        }

        private async Task UpdateState(string state = null)
        {
            if (state == CHECK || state == UNCHECK)
            {
                var isChecked = state == CHECK;
                TChecked checkedState;
                Type type = typeof(TChecked);
                if (type.Equals(typeof(byte)) || type.Equals(typeof(byte?)))
                {
                    checkedState = (TChecked)(object)Convert.ToByte(isChecked, CultureInfo.InvariantCulture);
                }
                else
                {
                    checkedState = (TChecked)(object)isChecked;
                }

                await UpdateCheckState(checkedState);
                await IndeterminateChanged.InvokeAsync(false);
                Indeterminate = false;
            }
            else
            {
                await CheckedChanged.InvokeAsync((TChecked)(object)null);
                Checked = (TChecked)(object)null;
                await IndeterminateChanged.InvokeAsync(true);
                Indeterminate = true;
            }
        }

        private async Task OnClickHandler(MouseEventArgs args)
        {
            if (EnableTriState)
            {
                if (Convert.ToBoolean(Checked, CultureInfo.InvariantCulture) && !Indeterminate)
                {
                    UIChange();
                    await UpdateState();
                }
                else if (Indeterminate)
                {
                    UIChange(UNCHECK);
                    await UpdateState(UNCHECK);
                }
                else if (!Convert.ToBoolean(Checked, CultureInfo.InvariantCulture) && !Indeterminate)
                {
                    UIChange(CHECK);
                    await UpdateState(CHECK);
                }
            }
            else if (Indeterminate)
            {
                var CheckedValue = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture) ? CHECK : UNCHECK;
                UIChange(CheckedValue);
                await UpdateState(CheckedValue);
            }
            else
            {
                var state = Convert.ToBoolean(Checked, CultureInfo.InvariantCulture) ? UNCHECK : CHECK;
                UIChange(state);
                await UpdateState(state);
            }
            if (EnablePersistence)
            {
                await SetLocalStorage(idValue, Checked);
            }
            await SfBaseUtils.InvokeEvent(ValueChange, new ChangeEventArgs<TChecked> { Checked = Checked, Event = args });
        }

        private void UIChange(string state = null)
        {
            frameClass = FRAME_CLASS;
            rippleClass = RIPPLE;
            if (state == CHECK)
            {
                frameClass = frameClass.Replace(SPACE + INDETERMINATE_CLASS, string.Empty, StringComparison.Ordinal);
                if (frameClass.IndexOf(SPACE + CHECK_CLASS, StringComparison.Ordinal) < 0)
                {
                    frameClass += SPACE + CHECK_CLASS;
                }

                if (SyncfusionService.options.EnableRippleEffect)
                {
                    rippleClass = rippleClass.Replace(SPACE + RIPPLE_INDETERMINATE, string.Empty, StringComparison.Ordinal);
                    rippleClass += SPACE + RIPPLE_CHECK;
                }

                ariaChecked = TRUE;
            }
            else if (state == UNCHECK)
            {
                frameClass = frameClass.Replace(SPACE + INDETERMINATE_CLASS, string.Empty, StringComparison.Ordinal);
                frameClass = frameClass.Replace(SPACE + CHECK_CLASS, string.Empty, StringComparison.Ordinal);
                if (SyncfusionService.options.EnableRippleEffect)
                {
                    rippleClass = rippleClass.Replace(SPACE + RIPPLE_INDETERMINATE, string.Empty, StringComparison.Ordinal);
                    rippleClass = rippleClass.Replace(SPACE + RIPPLE_CHECK, string.Empty, StringComparison.Ordinal);
                }

                ariaChecked = FALSE;
            }
            else
            {
                frameClass = frameClass.Replace(SPACE + CHECK_CLASS, string.Empty, StringComparison.Ordinal);
                if (frameClass.IndexOf(SPACE + INDETERMINATE_CLASS, StringComparison.Ordinal) < 0)
                {
                    frameClass += SPACE + INDETERMINATE_CLASS;
                }

                if (SyncfusionService.options.EnableRippleEffect)
                {
                    rippleClass = rippleClass.Replace(SPACE + RIPPLE_CHECK, string.Empty, StringComparison.Ordinal);
                    rippleClass += SPACE + RIPPLE_INDETERMINATE;
                }

                ariaChecked = MIXED;
            }
        }

        private void OnKeyupHandler()
        {
            if (isFocused && checkboxClass.IndexOf(FOCUS, StringComparison.Ordinal) < 0)
            {
                checkboxClass += SPACE + FOCUS;
            }
        }

        private void OnFocusOutHandler()
        {
            checkboxClass = checkboxClass.Replace(SPACE + FOCUS, string.Empty, StringComparison.Ordinal);
            isFocused = false;
        }

        private void OnFocusHandler()
        {
            isFocused = true;
        }

        private Dictionary<string, object> GetAttributes(object attributes)
        {
            if (htmlAttributes.ContainsKey("title"))
            {
                htmlAttributes["title"] = attributes;
            }
            else
            {
                htmlAttributes.Add("title", attributes);
            }
            return htmlAttributes;
        }
    }
}
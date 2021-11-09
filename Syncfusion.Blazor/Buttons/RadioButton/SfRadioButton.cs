using System;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.ComponentModel;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// The RadioButton is a graphical user interface element that allows you to select one option from the choices.
    /// It contains checked and unchecked states.
    /// </summary>
    public partial class SfRadioButton<TChecked> : SfInputBase<TChecked>
    {
        private const string SPACE = " ";
        private const string RTL = "e-rtl";
        private const string RIGHT = "e-right";
        private const string RADIOBUTTON = "radiobutton";
        private const string RIPPLE = "e-ripple-container";
        private const string ROOTCLASS = "e-radio-wrapper e-wrapper";
        private const string RADIOBUTTONCLASS = "e-control e-radio e-lib";
        private const string TYPE = "radio";
        private const string LABEL = "e-label";
        private const string DATARIPPLE = "true";
        private string rootClass = ROOTCLASS;
        private string labelClass;
        private bool? RadioChecked = false;

        /// <summary>
        /// Defines the caption for the RadioButton, that describes the purpose of the RadioButton.
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Positions label `before`/`after` the RadioButton.
        /// The possible values are:
        /// - Before: The label is positioned to left of the RadioButton.
        /// - After: The label is positioned to right of the RadioButton.
        /// </summary>
        [Parameter]
        public LabelPosition LabelPosition { get; set; }

        /// <summary>
        /// Event trigger when the RadioButton state has been changed by user interaction.
        /// </summary>
        [Parameter]
        public EventCallback<ChangeArgs<TChecked>> ValueChange { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (string.IsNullOrEmpty(idValue) || (htmlAttr != null && htmlAttr.ContainsKey("id")))
            {
                idValue = RADIOBUTTON + "-" + Guid.NewGuid().ToString();
            }
        }

        protected override void InitRender()
        {
            rootClass = ROOTCLASS;
            if (Checked != null && Value != null && Value != "null")
            {
                RadioChecked = Checked.Equals(TryParseValueFromString(Value));
            }
            else
            {
                RadioChecked = false;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                rootClass += SPACE + CssClass;
            }

            if (LabelPosition == LabelPosition.Before && EnableRtl)
            {
                labelClass = RIGHT + SPACE + RTL;
            }
            else if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                labelClass = RTL;
            }
            else if (LabelPosition == LabelPosition.Before)
            {
                labelClass = RIGHT;
            }
        }

        private async Task OnClickHandler(MouseEventArgs args)
        {
            TChecked state;
            if (Value == "null")
            {
                state = (TChecked)(object)null;
            }
            else if (Value == null)
            {
                state = (TChecked)(object)false;
            }
            else
            {
                state = TryParseValueFromString(Value);
            }

            await UpdateCheckState(state);
            await SfBaseUtils.InvokeEvent(ValueChange, new ChangeArgs<TChecked> { Value = Checked, Event = args });
        }

        private static TChecked TryParseValueFromString(string value)
        {
            TChecked parsedValue;
            if (typeof(TChecked) == typeof(bool?) || typeof(TChecked) == typeof(bool))
            {
                parsedValue = (TChecked)(object)Convert.ToBoolean(value, CultureInfo.InvariantCulture);
            }
            else
            {
                BindConverter.TryConvertTo<TChecked>(value, Intl.CurrentCulture, out parsedValue);
            }

            return parsedValue;
        }
    }
}
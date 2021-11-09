using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs.Internal
{
    public partial class ColorPickerTextBox
    {
        private string hexValue;
        private double? rValue;
        private double? gValue;
        private double? bValue;
        private double? opacityValue;
        private double rMax = 255;
        private double gbMax = 255;
        private bool readOnly;
        private string[] rgbLabel = new string[] { "R", "G", "B" };
        private Dictionary<string, object> hexAttributes = new Dictionary<string, object>() { { MAX_LENGTH, SEVEN }, { SPELL_CHECK, FALSE } };

        [CascadingParameter]
        protected SfColorPicker Parent { get; set; }

        [Parameter]
        public EventCallback<InputChange> ValueChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            readOnly = (object)ValueChanged == null;
            hexValue = Parent.textValue.Substring(0, 7);
            rValue = Parent.rgb[0];
            gValue = Parent.rgb[1];
            bValue = Parent.rgb[2];
            opacityValue = Parent.rgb[3] * 100;
        }

        private async Task HexInputHandler(InputEventArgs e)
        {
            hexValue = e.Value;
            if (string.IsNullOrEmpty(e.Value))
            {
                return;
            }

            var colorValue = string.Empty;
            if ((e.Value[0] == '#' && e.Value.Length != 5) || (e.Value[0] != '#' && e.Value.Length != 4))
            {
                colorValue = Utils.RoundValue(e.Value);
            }

            if (colorValue.Length == 9)
            {
                var pValue = Utils.RgbToHex(Parent.rgb);
                Parent.rgb = Utils.HexToRgb(colorValue);
                Parent.hsv = Utils.RgbToHsv(Parent.rgb);
                var cValue = Utils.RgbToHex(Parent.rgb);
                UpdateInput(null, false);
                await ValueChanged.InvokeAsync(new InputChange() { Label = "HEX", PreviousValue = pValue, CurrentValue = cValue });
            }
        }

        private async Task FirstInputHandler() => await InputHandler(0, rValue);

        private async Task SecondInputHandler() => await InputHandler(1, gValue);

        private async Task ThirdInputHandler() => await InputHandler(2, bValue);

        private async Task OpacityInputHandler()
        {
            if (opacityValue != null)
            {
                await ValueChanged.InvokeAsync(new InputChange() { Label = "A", Opacity = (double)opacityValue });
            }
        }

        private async Task InputHandler(int index, double? inputValue)
        {
            if (inputValue == null)
            {
                return;
            }

            var pValue = Utils.RgbToHex(Parent.rgb);
            if (rgbLabel[0] == "R")
            {
                Parent.rgb[index] = (double)inputValue;
                Parent.hsv = Utils.RgbToHsv(Parent.rgb);
            }
            else
            {
                Parent.hsv[index] = (double)inputValue;
                Parent.rgb = Utils.HsvToRgb(Parent.hsv[0], Parent.hsv[1], Parent.hsv[2], Parent.hsv[3]);
            }

            var cValue = Utils.RgbToHex(Parent.rgb);
            hexValue = cValue.Substring(0, 7);
            await ValueChanged.InvokeAsync(new InputChange() { Label = rgbLabel[0], CurrentValue = cValue, PreviousValue = pValue });
        }

        public void UpdateInput(string hex, bool refresh = true)
        {
            if (hex != null)
            {
                hexValue = hex.Substring(0, 7);
            }

            if (rMax == 255)
            {
                rValue = Parent.rgb[0];
                gValue = Parent.rgb[1];
                bValue = Parent.rgb[2];
            }
            else
            {
                rValue = Parent.hsv[0];
                gValue = Parent.hsv[1];
                bValue = Parent.hsv[2];
            }

            if (refresh)
            {
                StateHasChanged();
            }
        }

        public void UpdateOpacityInput(double opacityValue)
        {
            this.opacityValue = opacityValue;
            StateHasChanged();
        }

        private void FormatSwitchHandler()
        {
            if (rMax == 255)
            {
                rMax = 360;
                gbMax = 100;
                rValue = Parent.hsv[0];
                gValue = Parent.hsv[1];
                bValue = Parent.hsv[2];
                rgbLabel[0] = "H";
                rgbLabel[1] = "S";
                rgbLabel[2] = "V";
            }
            else
            {
                rMax = gbMax = 255;
                rValue = Parent.rgb[0];
                gValue = Parent.rgb[1];
                bValue = Parent.rgb[2];
                rgbLabel[0] = "R";
                rgbLabel[1] = "G";
                rgbLabel[2] = "B";
            }
        }
    }
}
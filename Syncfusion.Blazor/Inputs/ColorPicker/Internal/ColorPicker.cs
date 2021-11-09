using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Inputs.Internal
{
    public partial class ColorPicker
    {
        private Offset offset;
        private bool mouseDown;
        private double opacityValue;
        private string curPreview;
        private bool openTooltip;
        private double x;
        private double y;
        private string handlerClass = HANDLER;
        private ColorPickerTextBox inputs;
        private string opacityBgm = OPACITY_BGM;
        private string colorPickerBgm = DEFAULT_BGM;
        private string handleOffset = HANDLER_OFFSET;
        private string tooltipOffset;

        [CascadingParameter]
        protected SfColorPicker Parent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Initialize(false);
        }

        internal void Initialize(bool isRefresh)
        {
            opacityValue = Parent.rgb[3] * 100;
            if (Parent.textValue != DEFAULT_COLOR)
            {
                SetHueBackground(Parent.hsv[0]);
                SetOpacityBackground();
            }

            curPreview = BACKGROUND + Utils.ConvertToRgbString(Parent.rgb);
            if (isRefresh)
            {
                inputs?.UpdateInput(Parent.textValue, false);
            }
        }

        private async Task MouseDownHandler(double clientX, double clientY)
        {
            if (offset == null)
            {
                return;
            }

            mouseDown = true;
            x = clientX;
            y = clientY;
            SetHsv(clientX, clientY);
            UpdateHsv();
            handleOffset += HANDLE_TRANSITION;
            await UpdateAction();
        }

        private async Task MouseMoveHandler(double clientX, double clientY)
        {
            if (!mouseDown)
            {
                return;
            }

            SetHsv(clientX, clientY);
            if (!openTooltip && (Math.Abs(clientX - x) > 8 || Math.Abs(clientY - y) > 8))
            {
                openTooltip = true;
                handlerClass += HIDE_HANDLER;
            }

            UpdateHsv();
            handleOffset += TRANSITION_NONE;
            await UpdateAction();
        }

        internal async Task MouseUpHandler()
        {
            mouseDown = openTooltip = false;
            handlerClass = HANDLER;
            if (!Parent.ShowButtons)
            {
                await Parent.ClosePopup(true);
            }
        }

        private void SetHsv(double clientX, double clientY)
        {
            if (Parent.EnableRtl)
            {
                clientX = clientX > offset.Right ? 0 : Math.Abs(clientX - offset.Right);
            }
            else
            {
                clientX = clientX > offset.Left ? Math.Abs(clientX - offset.Left) : 0;
            }

            clientY = clientY > offset.Top ? Math.Abs(clientY - offset.Top) : 0;
            Parent.hsv[2] = Math.Round(100 * (offset.ClientHeight -
                Math.Max(0, Math.Min(offset.ClientHeight, clientY - offset.ClientTop))) / offset.ClientHeight * 10) / 10;
            Parent.hsv[1] =
                Math.Round(100 * Math.Max(0, Math.Min(offset.ClientWidth, clientX - offset.ClientLeft)) / offset.ClientWidth * 10) / 10;
        }

        private void UpdateHsv()
        {
            Parent.hsv[1] = Parent.hsv[1] > 100 ? 100 : Parent.hsv[1];
            Parent.hsv[2] = Parent.hsv[2] > 100 ? 100 : Parent.hsv[2];
            SetHandlerPosition();
        }

        private void SetHandlerPosition()
        {
            double left;
            double top;
            if (Parent.EnableRtl)
            {
                left = offset.ClientWidth * Math.Abs(100 - Parent.hsv[1]) / 100;
            }
            else
            {
                left = offset.ClientWidth * Parent.hsv[1] / 100;
            }

            top = offset.ClientHeight * (100 - Parent.hsv[2]) / 100;
            handleOffset = LEFT + left.ToString(CultureInfo.InvariantCulture) + PIXEL + TOP + top.ToString(CultureInfo.InvariantCulture) + PIXEL;
            if (openTooltip)
            {
                tooltipOffset = "transform: rotate(45deg); " + LEFT + (left - 14).ToString(CultureInfo.InvariantCulture) + PIXEL + TOP + (top - 36).ToString(CultureInfo.InvariantCulture) + PIXEL;
            }
        }

        private void SetHueBackground(double h)
        {
            colorPickerBgm = BACKGROUND + Utils.ConvertToRgbString(Utils.HsvToRgb(h, 100, 100, 1));
        }

        private void SetOpacityBackground()
        {
            var direction = Parent.EnableRtl ? "to left" : "to right";
            opacityBgm = "background: linear-gradient(" + direction + ", rgba(" + Parent.rgb[0] + "," + Parent.rgb[1] + "," + Parent.rgb[2] + ", 0) 0%, " + "rgb(" +
                 Parent.rgb[0] + "," + Parent.rgb[1] + "," + Parent.rgb[2] + ") 100%)";
        }

        private async Task HueChange(SliderChangeEventArgs<double> e)
        {
            if (Parent.hsv[0] == e.Value)
            {
                return;
            }

            Parent.hsv[0] = e.Value;
            SetHueBackground(Parent.hsv[0]);
            await UpdateAction();
        }

        private async Task OpacityChange(SliderChangeEventArgs<double> args)
        {
            if (opacityValue == args.Value)
            {
                return;
            }

            opacityValue = args.Value;
            var pValue = Utils.RgbToHex(Parent.rgb);
            Parent.hsv[3] = opacityValue / 100;
            Parent.rgb[3] = opacityValue / 100;
            var cValue = Utils.RgbToHex(Parent.rgb);
            inputs?.UpdateOpacityInput(opacityValue);
            var rgb = Utils.ConvertToRgbString(Parent.rgb);
            UpdatePreview(rgb);
            await Parent.TriggerEvent(cValue, pValue, rgb);
        }

        private async Task UpdateAction(bool isKey = false)
        {
            string pValue = Utils.RgbToHex(Parent.rgb);
            Parent.rgb = Utils.HsvToRgb(Parent.hsv[0], Parent.hsv[1], Parent.hsv[2], Parent.hsv[3]);
            string cValue = Utils.RgbToHex(Parent.rgb);
            string rgba = Utils.ConvertToRgbString(Parent.rgb);
            UpdatePreview(rgba);
            inputs?.UpdateInput(cValue);
            await Parent.TriggerEvent(cValue, pValue, rgba, isKey);
        }

        private void UpdatePreview(string value)
        {
            SetOpacityBackground();
            curPreview = BACKGROUND + value;
        }

        private async Task PickerKeyDown(KeyboardEventArgs e)
        {
            switch (e.Code)
            {
                case ARROW_RIGHT:
                    await MoveHandler(1, Parent.EnableRtl ? -1 : 1, e);
                    break;
                case ARROW_LEFT:
                    await MoveHandler(1, Parent.EnableRtl ? 1 : -1, e);
                    break;
                case ARROW_UP:
                    await MoveHandler(2, 1, e);
                    break;
                case ARROW_DOWN:
                    await MoveHandler(2, -1, e);
                    break;
                case ENTER:
                    await Parent.TriggerEvent(Utils.RgbToHex(Parent.rgb), null, Utils.ConvertToRgbString(Parent.rgb), true, true);
                    await Parent.ClosePopup(true);
                    break;
            }
        }

        private async Task MoveHandler(int index, double valueDiff, KeyboardEventArgs e)
        {
            Parent.hsv[index] += valueDiff * (e.CtrlKey ? 1 : 3);
            if (Parent.hsv[index] < 0)
            {
                Parent.hsv[index] = 0;
            }

            UpdateHsv();
            await UpdateAction(true);
        }

        private async Task PreviewHandler()
        {
            var pValue = Utils.RgbToHex(Parent.rgb);
            Parent.rgb = Utils.HexToRgb(Parent.textValue);
            var hsv = Utils.RgbToHsv(Parent.rgb);
            SetHueBackground(hsv[0]);
            if (hsv[3] != Parent.hsv[3])
            {
                SetOpacityBackground();
            }

            Parent.hsv = hsv;
            SetHandlerPosition();
            inputs?.UpdateInput(Parent.textValue);
            var rgba = Utils.ConvertToRgbString(Parent.rgb);
            curPreview = BACKGROUND + rgba;
            await Parent.TriggerEvent(Parent.textValue, pValue, rgba);
        }

        private async Task InputValueChange(InputChange e)
        {
            switch (e.Label)
            {
                case "HEX":
                case "R":
                case "G":
                case "B":
                    SetHandlerPosition();
                    break;
                case "H":
                    SetHueBackground(Parent.hsv[0]);
                    break;
                case "S":
                case "V":
                    UpdateHsv();
                    break;
                case "A":
                    await OpacityChange(new SliderChangeEventArgs<double>() { Value = e.Opacity });
                    break;
            }

            if (e.Label != "A")
            {
                var rgba = Utils.ConvertToRgbString(Parent.rgb);
                UpdatePreview(rgba);
                await Parent.TriggerEvent(e.CurrentValue, e.PreviousValue, rgba);
            }

            StateHasChanged();
        }

        internal void UpdateOffset(Offset offsetObj)
        {
            offset = offsetObj;
            if (offset != null)
            {
                SetHandlerPosition();
            }
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Inputs.Internal
{
    public partial class ColorPickerBase
    {
        private bool pickerMode;
        private bool triggerModeSwitchEvt;
        private string containerStyle;
        private string containerClass;
        private ColorPicker colorPicker;
        internal ColorPalette colorPalette;
        private ElementReference container;

        [CascadingParameter]
        protected SfColorPicker Parent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            pickerMode = Parent.Mode == ColorPickerMode.Picker;
            UpdateMode();
            InitializeValues();
        }

        private void UpdateMode()
        {
            if (pickerMode)
            {
                containerClass = CONTAINER + PICKER_CONTENT;
                containerStyle = WIDTH + string.Empty;
            }
            else
            {
                containerClass = CONTAINER + PALETTE_CONTENT;
                containerStyle = string.Empty;
            }
        }

        private void InitializeValues()
        {
            Parent.rgb = Utils.HexToRgb(Parent.textValue);
            Parent.hsv = Utils.RgbToHsv(Parent.rgb);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (triggerModeSwitchEvt)
            {
                triggerModeSwitchEvt = false;
                var offset = await Parent.InvokeMethod<Offset>(GET_OFFSET, false, Parent.element, container);
                if (pickerMode)
                {
                    Update(OFFSET, offset);
                }

                await SfBaseUtils.InvokeEvent(Parent.ModeSwitched, new ModeSwitchEventArgs() { Name = MODE_SWITCHED, Mode = pickerMode ? ColorPickerMode.Picker : ColorPickerMode.Palette });
            }
        }

        internal async Task ApplyClickHandler()
        {
            var cValue = Utils.RgbToHex(Parent.rgb);
            await Parent.TriggerEvent(cValue, null, Utils.ConvertToRgbString(Parent.rgb), false, true);
            await Parent.ClosePopup(true);
        }

        private async Task CancelClickHandler() => await Parent.ClosePopup(false);

        private async Task SwitchMode()
        {
            await SfBaseUtils.InvokeEvent(Parent.OnModeSwitch, new ModeSwitchEventArgs() { Name = BEFORE_MODE_SWITCH, Mode = pickerMode ? ColorPickerMode.Palette : ColorPickerMode.Picker });
            Parent.rgb = Utils.HexToRgb(Parent.textValue);
            Parent.hsv = Utils.RgbToHsv(Parent.rgb);
            pickerMode = !pickerMode;
            UpdateMode();
            triggerModeSwitchEvt = true;
        }

        internal void Update(string key, Offset keyValue)
        {
            if (key == OFFSET)
            {
                colorPicker?.UpdateOffset(keyValue);
            }
            else if (key == nameof(SfColorPicker.Value))
            {
                InitializeValues();
                colorPicker?.Initialize(true);
                colorPalette?.Initialize(true);
            }
            else if (key == nameof(SfColorPicker.Mode))
            {
                pickerMode = Parent.Mode == ColorPickerMode.Picker;
                UpdateMode();
                StateHasChanged();
            }
        }
    }
}
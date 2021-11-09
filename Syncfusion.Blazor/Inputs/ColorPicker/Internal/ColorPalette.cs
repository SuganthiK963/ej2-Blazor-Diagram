using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Inputs.Internal
{
    public partial class ColorPalette
    {
        private string selectedValue;
        private ColorPickerTextBox inputs;

        [CascadingParameter]
        protected SfColorPicker Parent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Initialize(false);
        }

        internal void Initialize(bool isRefresh)
        {
            selectedValue = Parent.textValue;
            if (isRefresh)
            {
                inputs?.UpdateInput(Parent.textValue, false);
            }
        }

        private string GetPaletteClass()
        {
            var paletteContainerCls = CUSTOM;
            double count = 0;
            for (int i = 0, len = Parent.PresetColors.Keys.Count; i < len; i++)
            {
                count += Parent.PresetColors.Values.ElementAt(i).Length / Parent.Columns;
                if (count > 10)
                {
                    paletteContainerCls += PALETTE_GROUP;
                    break;
                }
            }

            return paletteContainerCls;
        }

        private TileProps GetTileProps(string color, string presetKey, int index)
        {
            var props = new TileProps { Color = Utils.RoundValue(color).ToLower(CultureInfo.InvariantCulture) };
            props.BgColor = BACKGROUND + Utils.ConvertToRgbString(Utils.HexToRgb(props.Color));
            props.TileClass = props.Color == selectedValue ? TILE + SELECTED : TILE;
            if (string.IsNullOrEmpty(color) || (index == 0 && Parent.NoColor && Parent.Mode == ColorPickerMode.Palette && !Parent.ModeSwitcher))
            {
                props.TileClass += NO_COLOR;
                props.BgColor = props.Color = string.Empty;
            }

            Task.Run(async delegate
            {
                await SfBaseUtils.InvokeEvent(Parent.OnTileRender, new PaletteTileEventArgs() { Value = color, PresetName = presetKey });
            });
            return props;
        }

        internal async Task ClickHandler(string colorValue)
        {
            selectedValue = colorValue;
            var pValue = Utils.RgbToHex(Parent.rgb);
            Parent.rgb = Utils.HexToRgb(colorValue);
            Parent.hsv = Utils.RgbToHsv(Parent.rgb);
            if (Parent.containerClass.Contains(SHOW_VALUE, StringComparison.Ordinal) && !string.IsNullOrEmpty(colorValue))
            {
                inputs.UpdateInput(colorValue);
            }

            var rgba = Utils.ConvertToRgbString(Parent.rgb);
            await Parent.TriggerEvent(colorValue, pValue, rgba);
            if (!Parent.ShowButtons)
            {
                await Parent.ClosePopup(true);
            }
        }

        private class TileProps
        {
            public string Color { get; set; }

            public string TileClass { get; set; }

            public string BgColor { get; set; }
        }
    }
}
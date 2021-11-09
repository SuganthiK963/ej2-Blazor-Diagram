using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs
{
    public partial class SfColorPicker
    {
        private const string COLOR_PICKER = "e-lib e-colorpicker e-control";
        private const string CONTAINER_CLASS = "e-colorpicker-container";
        private const string SPLIT_PREVIEW = "e-split-preview";
        private const string SELECTED_CLR = "e-btn-icon e-selected-color";
        private const string SPLIT_PICKER = "e-icon-btn e-split-colorpicker";
        private const string POPUP = "e-colorpicker-popup";
        private const string COLOR = "color";
        private const string DEFAULT_COLOR = "#008000ff";
        private const string DISABLED = " e-disabled";
        private const string HIDE_OPACITY = " e-hide-opacity";
        private const string RTL = " e-rtl";
        private const string SPACE = " ";
        private const string OFFSET = "Offset";
        private const string BACKGROUND = "background-color: ";
        private const string ONCLOSE = "OnClose";
        private const string CHANGE = "ValueChange";
        private const string SELECT = "Selected";
        private const string INIT = "sfBlazor.ColorPicker.initialize";
        private const string FOCUS_IN = "sfBlazor.ColorPicker.focusIn";
        private const string DESTROY = "sfBlazor.ColorPicker.destroy";
    }
}

namespace Syncfusion.Blazor.Inputs.Internal
{
    public partial class ColorPickerBase
    {
        private const string CONTAINER = "e-container sf-colorpicker e-control e-lib";
        private const string APPLY_CLS = "e-btn e-css e-flat e-primary e-small e-apply";
        private const string CANCEL_CLS = "e-btn e-css e-flat e-small e-cancel";
        private const string CTRL_BTN = "e-ctrl-btn";
        private const string CTRL_SWITCH = "e-switch-ctrl-btn";
        private const string SWITCH_BTN_CLS = "e-icons e-btn e-flat e-icon-btn e-mode-switch-btn";
        private const string PALETTE_CONTENT = " e-color-palette";
        private const string PICKER_CONTENT = " e-color-picker";
        private const string APPLY_KEY = "ColorPicker_Apply";
        private const string APPLY = "Apply";
        private const string CANCEL_KEY = "ColorPicker_Cancel";
        private const string CANCEL = "Cancel";
        private const string MODE_SWITCHER_KEY = "ColorPicker_ModeSwitcher";
        private const string SWITCH_MODE = "Switch Mode";
        private const string OFFSET = "Offset";
        private const string BEFORE_MODE_SWITCH = "OnModeSwitch";
        private const string MODE_SWITCHED = "ModeSwitched";
        private const string WIDTH = "width: ";
        private const string DEVICE_MODEL = "e-colorpicker e-modal";
        private const string DISPLAY_NONE = "display: none;";
        private const string GET_OFFSET = "sfBlazor.ColorPicker.getOffset";
    }

    public partial class ColorPicker
    {
        private const string HANDLER = "e-handler";
        private const string HIDE_HANDLER = " e-hide-handler";
        private const string LEFT = "left: ";
        private const string TOP = " top: ";
        private const string PIXEL = "px;";
        private const string DEFAULT_COLOR = "#008000ff";
        private const string ARROW_RIGHT = "ArrowRight";
        private const string ARROW_LEFT = "ArrowLeft";
        private const string ARROW_UP = "ArrowUp";
        private const string ARROW_DOWN = "ArrowDown";
        private const string ENTER = "Enter";
        private const string HSV_AREA = "e-hsv-color";
        private const string HSV_CONTAINER = "e-hsv-container";
        private const string HSV_MODEL = "e-colorpicker e-hsv-model";
        private const string BACKGROUND = "background-color: ";
        private const string HANDLE_TRANSITION = " transition: left .4s cubic-bezier(.25, .8, .25, 1), top .4s cubic-bezier(.25, .8, .25, 1)";
        private const string TRANSITION_NONE = " transition: none 0s ease 0s;";
        private const string SLIDER_PREVIEW = "e-slider-preview";
        private const string SLIDER_Container = "e-colorpicker-slider";
        private const string OPACITY_EMPTY = "e-opacity-empty-track";
        private const string HUE_SLIDER = "e-hue-slider";
        private const string OPACITY_SLIDER = "e-opacity-slider";
        private const string PREVIEW = "e-preview-container";
        private const string CURRENT = "e-preview e-current";
        private const string PREVIOUS = "e-preview e-previous";
        private const string HIDE_VALUE = "e-hide-value";
        private const string TIP_CONTENT = "e-tip-content";
        private const string TIP_TRANSPARENT = "e-tip-transparent";
        private const string TIP_CLASS = "e-tooltip-wrap e-popup e-color-picker-tooltip e-lib e-control e-popup-open";
        private const string DEFAULT_BGM = "background-color: rgb(0, 255, 0);";
        private const string OPACITY_BGM = "background: linear-gradient(to right, rgba(0,128,0, 0) 0%, rgb(0,128,0) 100%)";
        private const string HANDLER_OFFSET = "left: 0px; top: 0px;";
    }

    public partial class ColorPalette
    {
        private const string ROW_CLS = "e-row";
        private const string ROW_ROLE = "row";
        private const string TILE = "e-tile";
        private const string ROLE = "gridcell";
        private const string DEFAULT = "default";
        private const string BACKGROUND = "background-color: ";
        private const string PALETTE = "e-palette";
        private const string SHOW_VALUE = "e-show-value";
        private const string SELECTED = " e-selected";
        private const string CUSTOM = "e-custom-palette";
        private const string NO_COLOR = " e-nocolor-item";
        private const string PALETTE_GROUP = " e-palette-group";
        private List<string> colors = new List<string>
        {
            "#000000", "#f44336", "#e91e63", "#9c27b0", "#673ab7", "#2196f3",
            "#03a9f4", "#00bcd4", "#009688", "#ffeb3b", "#ffffff", "#ffebee", "#fce4ec", "#f3e5f5", "#ede7f6", "#e3f2fd",
            "#e1f5fe", "#e0f7fa", "#e0f2f1", "#fffde7", "#f2f2f2", "#ffcdd2", "#f8bbd0", "#e1bee7", "#d1c4e9", "#bbdefb",
            "#b3e5fc", "#b2ebf2", "#b2dfdb", "#fff9c4", "#e6e6e6", "#ef9a9a", "#f48fb1", "#ce93d8", "#b39ddb", "#90caf9",
            "#81d4fa", "#80deea", "#80cbc4", "#fff59d", "#cccccc", "#e57373", "#f06292", "#ba68c8", "#9575cd", "#64b5f6",
            "#4fc3f7", "#4dd0e1", "#4db6ac", "#fff176", "#b3b3b3", "#ef5350", "#ec407a", "#ab47bc", "#7e57c2", "#42a5f5",
            "#29b6f6", "#26c6da", "#26a69a", "#ffee58", "#999999", "#e53935", "#d81b60", "#8e24aa", "#5e35b1", "#1e88e5",
            "#039be5", "#00acc1", "#00897b", "#fdd835", "#808080", "#d32f2f", "#c2185b", "#7b1fa2", "#512da8", "#1976d2",
            "#0288d1", "#0097a7", "#00796b", "#fbc02d", "#666666", "#c62828", "#ad1457", "#6a1b9a", "#4527a0", "#1565c0",
            "#0277bd", "#00838f", "#00695c", "#f9a825", "#4d4d4d", "#b71c1c", "#880e4f", "#4a148c", "#311b92", "#0d47a1",
            "#01579b", "#006064", "#004d40", "#f57f17"
        };
    }

    public partial class ColorPickerTextBox
    {
        private const string SELECTED_VALUE = "e-selected-value";
        private const string INPUT_CONTAINER = "e-input-container";
        private const string FORMAT_SWITCH = "e-icons e-css e-btn e-flat e-icon-btn e-value-switch-btn";
        private const string HIDE_VALUE_SWITCH = "e-hide-valueswitcher";
        private const string HIDE_HEX = "e-hide-hex-value";
        private const string HIDE_RGBA = "e-hide-switchable-value";
        private const string MAX_LENGTH = "maxlength";
        private const string SPELL_CHECK = "spellcheck";
        private const string SEVEN = "7";
        private const string FALSE = "false";
    }

    public class InputChange
    {
        public string Label { get; set; }

        public string PreviousValue { get; set; }

        public string CurrentValue { get; set; }

        public double Opacity { get; set; }
    }
}
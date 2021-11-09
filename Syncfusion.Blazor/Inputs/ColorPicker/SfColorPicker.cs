using Microsoft.JSInterop;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using Syncfusion.Blazor.SplitButtons;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// ColorPicker component is a user interface to select and adjust color values.
    /// It provides supports for various color specification like Red Green Blue, Hue Saturation Value and Hex codes.
    /// </summary>
    public partial class SfColorPicker : SfBaseComponent
    {
        private string dropdownClass;
        private string colorValue;
        private string inputValue;
        private string id;
        private string cssClass;
        private bool enableRtl;
        private bool disabled;
        private bool enableOpacity;
        private ColorPickerMode mode;
        private ColorPickerBase colorPickerBase;
        internal bool IsDevice;
        internal bool ShowModel;
        internal double[] rgb;
        internal double[] hsv;
        internal string textValue;
        internal string preview;
        internal string containerClass;
        internal ElementReference element;
        internal SfSplitButton splitBtnObj;
        private Dictionary<string, object> htmlAttributes;

        private void Initialize()
        {
            var containerCls = CONTAINER_CLASS;
            if (EnableRtl)
            {
                containerCls += RTL;
            }

            var dropdownCls = POPUP;
            if (Disabled)
            {
                containerCls += DISABLED;
                dropdownCls += DISABLED;
            }

            if (!EnableOpacity)
            {
                containerCls += HIDE_OPACITY;
                dropdownCls += HIDE_OPACITY;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                containerCls += SPACE + CssClass;
                dropdownCls += SPACE + CssClass;
            }

            containerClass = containerCls;
            dropdownClass = dropdownCls;

            // Unique class added for dynamically rendered Inplace-editor components.
            if (ColorPickerParent != null)
            {
                containerClass = SfBaseUtils.AddClass(containerClass, "e-editable-elements");
                dropdownClass = SfBaseUtils.AddClass(dropdownClass, "e-editable-elements");
            }
        }

        private async Task OpenHandler(OpenCloseMenuEventArgs e)
        {
            await SfBaseUtils.InvokeEvent(Opened, new OpenEventArgs() { Element = e.Element });
        }

        private async Task OnOpenClose(BeforeOpenCloseMenuEventArgs e)
        {
            var eventArgs = new BeforeOpenCloseEventArgs() { Element = e.Element, Name = e.Name };
            await SfBaseUtils.InvokeEvent(e.Name == ONCLOSE ? OnClose : OnOpen, eventArgs);
            e.Cancel = eventArgs.Cancel;
            if (!e.Cancel)
            {
                ShowModel = e.Name != ONCLOSE;
            }
        }

        private async Task OnClicked()
        {
            await SfBaseUtils.InvokeEvent(ValueChange, new ColorPickerEventArgs()
            {
                CurrentValue = new ColorPickerValue { Hex = textValue.Length > 6 ? textValue.Substring(0, 7) : string.Empty, Rgba = Utils.ConvertToRgbString(rgb) },
                Name = CHANGE,
                PreviousValue = new ColorPickerValue { Hex = null, Rgba = null },
            });
        }

        internal async Task ClosePopup(bool apply)
        {
            var eventArgs = new BeforeOpenCloseEventArgs() { Name = ONCLOSE };
            if (!Inline || !apply)
            {
                await SfBaseUtils.InvokeEvent(OnClose, eventArgs);
            }

            if (Inline)
            {
                return;
            }

            if (!eventArgs.Cancel)
            {
                if (apply)
                {
                    preview = BACKGROUND + Utils.ConvertToRgbString(rgb);
                }

                ShowModel = false;
                splitBtnObj.Toggle();
            }
        }

        internal async Task TriggerEvent(string cValue, string pValue, string rgba, bool isKey = false, bool change = false)
        {
            var hex = cValue.Length > 6 ? cValue.Substring(0, 7) : string.Empty;
            if (change || (!ShowButtons && !isKey))
            {
                if (cValue == textValue)
                {
                    return;
                }

                inputValue = hex;
                if (!string.IsNullOrEmpty(cValue) || NoColor)
                {
                    colorValue = Value = await SfBaseUtils.UpdateProperty(EnableOpacity ? cValue : hex, colorValue, ValueChanged, ColorPickerEditContext, ValueExpression);
                }

                await SfBaseUtils.InvokeEvent(ValueChange, new ColorPickerEventArgs
                {
                    CurrentValue = new ColorPickerValue { Hex = hex, Rgba = rgba },
                    Name = CHANGE,
                    PreviousValue = new ColorPickerValue { Hex = textValue.Length > 6 ? textValue.Substring(0, 7) : string.Empty, Rgba = Utils.ConvertToRgbString(Utils.HexToRgb(textValue)) }
                });
                if (!string.IsNullOrEmpty(cValue) || NoColor)
                {
                    textValue = cValue;
                }
            }
            else
            {
                await SfBaseUtils.InvokeEvent(Selected, new ColorPickerEventArgs
                {
                    CurrentValue = new ColorPickerValue { Hex = hex, Rgba = rgba },
                    Name = SELECT,
                    PreviousValue = new ColorPickerValue { Hex = pValue.Length > 6 ? pValue.Substring(0, 7) : string.Empty, Rgba = Utils.ConvertToRgbString(Utils.HexToRgb(pValue)) }
                });
            }
        }

        private void UpdateValue()
        {
            var colorValue = string.IsNullOrEmpty(Value) && string.IsNullOrEmpty(textValue) ? DEFAULT_COLOR : Utils.RoundValue(Value);
            var slicedValue = string.Empty;
            if (NoColor && Mode == ColorPickerMode.Palette && string.IsNullOrEmpty(Value))
            {
                colorValue = string.Empty;
            }
            else
            {
                if (colorValue.Length != 9)
                {
                    if (string.IsNullOrEmpty(textValue))
                    {
                        colorValue = DEFAULT_COLOR;
                    }
                    else
                    {
                        return;
                    }
                }

                slicedValue = colorValue.Substring(0, 7);
            }

            inputValue = slicedValue;
            textValue = colorValue;
            rgb = Utils.HexToRgb(textValue);
            preview = BACKGROUND + Utils.ConvertToRgbString(rgb);
        }

        /// <exclude/>

        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetOffset(Offset offsetObj)
        {
            colorPickerBase?.Update(OFFSET, offsetObj);
        }

        internal override void ComponentDispose()
        {
            if (IsRendered && Inline)
            {
                InvokeMethod(DESTROY, element).ContinueWith(t => { }, TaskScheduler.Current);
            }
        }
        
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Click(string colorValue)
        {
            colorPickerBase?.colorPalette?.ClickHandler(colorValue);
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Close()
        {
            colorPickerBase?.ApplyClickHandler();
        }
        
    }
}
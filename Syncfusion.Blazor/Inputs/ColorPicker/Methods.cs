using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using Syncfusion.Blazor.Inputs.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// ColorPicker component is a user interface to select and adjust color values.
    /// It provides supports for various color specification like Red Green Blue, Hue Saturation Value and Hex codes.
    /// </summary>
    public partial class SfColorPicker : SfBaseComponent
    {
        /// <summary>
        /// To get color value in specified type.
        /// </summary>
        /// <param name = "value">Specify the color value.</param>
        /// <param name = "type">Specify the type to which the specified color needs to be converted.</param>
        /// <returns>Returns the color value in specified type.</returns>
        public string GetValue(string value = null, ColorValueType type = ColorValueType.Hex)
        {
            if (value == null)
            {
                value = Value;
            }

            if (value[0] == 'r')
            {
                double[] cValue = Utils.ConvertRgbToNumberArray(value);
                if (type == ColorValueType.Hex || type == ColorValueType.Hexa)
                {
                    string hex = Utils.RgbToHex(cValue);
                    return type == ColorValueType.Hex ? hex.Substring(0, 7) : hex;
                }
                else
                {
                    if (type == ColorValueType.Hsv)
                    {
                        return Utils.ConvertToHsvString(Utils.RgbToHsv(Utils.Slice(cValue, 3)));
                    }
                    else
                    {
                        if (type == ColorValueType.Hsva)
                        {
                            return Utils.ConvertToHsvString(Utils.RgbToHsv(rgb));
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            else
            {
                if (value[0] == 'h')
                {
                    double[] num = Utils.ConvertRgbToNumberArray(value);
                    double[] cValue = Utils.HsvToRgb(num[0], num[1], num[2]);
                    Utils.HsvToRgb(2, 2, 2);
                    if (type == ColorValueType.Rgb)
                    {
                        return Utils.ConvertToRgbString(cValue);
                    }
                    else
                    {
                        if (type == ColorValueType.Hex || type == ColorValueType.Hexa)
                        {
                            string hex = Utils.RgbToHex(cValue);
                            return type == ColorValueType.Hex ? hex.Substring(0, 7) : hex;
                        }
                        else
                        {
                            if (type == ColorValueType.Rgb)
                            {
                                return Utils.ConvertToRgbString(Utils.Slice(cValue, 3));
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                else
                {
                    value = Utils.RoundValue(value);
                    double[] rgb = Utils.HexToRgb(value);
                    if (type == ColorValueType.Rgb || type == ColorValueType.Hsv)
                    {
                        rgb = Utils.Slice(rgb, 3);
                    }

                    if (type == ColorValueType.Rgba || type == ColorValueType.Rgb)
                    {
                        return Utils.ConvertToRgbString(rgb);
                    }
                    else
                    {
                        if (type == ColorValueType.Hsva || type == ColorValueType.Hsv)
                        {
                            return Utils.ConvertToHsvString(Utils.RgbToHsv(rgb));
                        }
                        else
                        {
                            if (type == ColorValueType.Hex)
                            {
                                return value.Substring(0, 7);
                            }
                            else
                            {
                                if (type == ColorValueType.Opacity)
                                {
                                    return rgb[3].ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  To show/hide ColorPicker popup based on current state of the SplitButton.
        /// </summary>
        public void Toggle()
        {
            ShowModel = !ShowModel;
            splitBtnObj?.Toggle();
        }

        /// <summary>
        /// Sets the focus to Colorpicker.
        /// its native method.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod(FOCUS_IN, element, Inline);
        }

        /// <summary>
        /// Sets the focus to Colorpicker.
        /// its native method.
        /// </summary>
        public async Task FocusAsync()
        {
            await FocusIn();
        }
    }
}
using System;
using System.Globalization;

namespace Syncfusion.Blazor.Inputs.Internal
{
    public static class Utils
    {
        public static string RgbToHex(double[] rgb)
        {
            if (rgb == null)
            {
                return "";
            }
            if (rgb.Length == 0)
            {
                return string.Empty;
            }

            string str = Convert.ToInt32(Math.Round(rgb[3] * 255) + 0x10000).ToString("x", CultureInfo.InvariantCulture);
            return "#" + Hex(rgb[0]) + Hex(rgb[1]) + Hex(rgb[2]) + str.Substring(str.Length - 2, 2);
        }

        public static string ConvertToRgbString(double[] rgb)
        {
            if (rgb == null)
            {
                return "";
            }
            return rgb.Length == 4 ? "rgba(" + string.Join(",", rgb[0].ToString(CultureInfo.InvariantCulture), rgb[1].ToString(CultureInfo.InvariantCulture), rgb[2].ToString(CultureInfo.InvariantCulture), rgb[3].ToString(CultureInfo.InvariantCulture)) + ")" : string.Empty;
        }

        public static double[] RgbToHsv(double[] rgb, double opacity = 1)
        {
            if (rgb == null || rgb.Length == 0)
            {
                return Array.Empty<double>();
            }

            double r = rgb[0], g = rgb[1], b = rgb[2];
            opacity = rgb[3];
            if (rgb.Length == 0)
            {
                return Array.Empty<double>();
            }

            r /= 255;
            g /= 255;
            b /= 255;
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double h = 0, s, v = max, d = max - min;
            s = max == 0 ? 0 : d / max;
            if (max == min)
            {
                h = 0;
            }
            else
            {
                if (max == r)
                {
                    h = ((g - b) / d) + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    h = ((b - r) / d) + 2;
                }
                else if (max == b)
                {
                    h = ((r - g) / d) + 4;
                }

                h /= 6;
            }

            double[] hsv = new double[4];
            hsv[0] = Math.Round(h * 360);
            hsv[1] = Math.Round(s * 1000) / 10;
            hsv[2] = Math.Round(v * 1000) / 10;
            hsv[3] = opacity;
            return hsv;
        }

        public static string RoundValue(string value)
        {
            if (value != null)
            {
                if (value.Length <= 1)
                {
                    return string.Empty;
                }
                if (value[0] != '#')
                {
                    value = '#' + value;
                }
                int len = value.Length;
                if (len == 4)
                {
                    value += 'f';
                    len = 5;
                }
                if (len == 5)
                {
                    string tempValue = string.Empty;
                    for (int i = 1, length = value.Length; i < length; i++)
                    {
                        var c = value[i] + value[i].ToString();
                        tempValue += c;
                    }

                    value = '#' + tempValue;
                    len = 9;
                }
                if (len == 7)
                {
                    value += "ff";
                }
            }
            return value;
        }

        public static double[] HsvToRgb(double h, double s, double v, double opacity = 1)
        {
            double r, g, b, i, f, p, q;
            s /= 100;
            v /= 100;
            if (s == 0)
            {
                r = g = b = v;
                return new double[] { Math.Round(r * 255), Math.Round(g * 255), Math.Round(b * 255), opacity };
            }

            h /= 60;
            i = Math.Floor(h);
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - (s * f));
            double t = v * (1 - (s * (1 - f)));
            switch (i)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                default: r = v; g = p; b = q; break;
            }

            return new double[] { Math.Round(r * 255), Math.Round(g * 255), Math.Round(b * 255), opacity };
        }

        public static double[] HexToRgb(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return Array.Empty<double>();
            }

            hex = hex.Trim();
            if (hex.Length != 9)
            {
                hex = RoundValue(hex);
            }

            double opacity = int.Parse(hex.Substring(hex.Length - 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            opacity = Math.Round(opacity / 255, 2);
            hex = hex.Substring(1, 6);
            int bigInt;
            try
            {
                bigInt = int.Parse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                bigInt = 0;
            }

            double[] h = new double[4];
            h[0] = (bigInt >> 16) & 255;
            h[1] = (bigInt >> 8) & 255;
            h[2] = bigInt & 255;
            h[3] = opacity;
            return h;
        }

        public static string ConvertToHsvString(double[] hsv)
        {
            if (hsv == null)
            {
                return "";
            }
            hsv[2] = (int)hsv[2];
            string hue = string.Join(",", hsv);
            if (hue[hue.Length - 1] == ',')
            {
                hue = hue.Remove(hue.Length - 1, 1);
            }

            return (hsv.Length == 4 && hsv[3] != 1) ? "hsva(" + hue + ")" : "hsv(" + hue + ")";
        }

        public static double[] ConvertRgbToNumberArray(string value)
        {
            double[] arr = new double[4];
            if (value == null)
            {
                return arr;
            }
            int start = value.IndexOf('(', StringComparison.Ordinal);
            if (value[value.Length - 1] == ')')
            {
                value = value.Remove(value.Length - 1, 1);
            }

            string[] array = value.Substring(start + 1).Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                arr[i] = Convert.ToDouble(array[i], CultureInfo.InvariantCulture);
            }

            return arr;
        }

        public static double[] Slice(double[] arr, int last)
        {
            double[] temp = new double[last];
            Array.Copy(arr, 0, temp, 0, last);
            return temp;
        }

        private static string Hex(double x)
        {
            int k = Convert.ToInt32(x);
            string str = "0" + k.ToString("x", CultureInfo.InvariantCulture);
            return str.Substring(str.Length - 2, 2);
        }
    }
}

using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Generates globalization details for the specific culture in JSON format.
    /// </summary>
    internal static class GlobalizeJsonGenerator
    {
        private static Dictionary<int, string> positiveCurrencyMapper = new Dictionary<int, string>()
        {
            { 0, "$n" },
            { 1, "n$" },
            { 2, "$ n" },
            { 3, "n $" }
        };

        private static Dictionary<int, string> negativeCurrencyMapper = new Dictionary<int, string>()
        {
            { 0, "($n)" },
            { 1, "-$n" },
            { 2, "$-n" },
            { 3, "$n-" },
            { 4, "(n$)" },
            { 5, "-n$" },
            { 6, "n-$" },
            { 7, "n$-" },
            { 8, "-n $" },
            { 9, "-$ n" },
            { 10, "n $-" },
            { 11, "$ n-" },
            { 12, "$ -n" },
            { 13, "n- $" },
            { 14, "($ n)" },
            { 15, "(n $)" }
        };

        private static Dictionary<int, string> positivePercentMapper = new Dictionary<int, string>()
        {
            { 0, "n %" },
            { 1, "n%" },
            { 2, "%n" },
            { 3, "% n" }
        };

        private static Dictionary<int, string> numberNegativePattern = new Dictionary<int, string>()
        {
            { 0, "(n)" },
            { 1, "-n" },
            { 2, "-n" },
            { 3, "n-" },
            { 4, "n-" }
        };

        private static Dictionary<int, string> negativePercentMapper = new Dictionary<int, string>()
        {
            { 0, "-n %" },
            { 1, "-n%" },
            { 2, "-%n" },
            { 3, " %-n" },
            { 4, "%n-" },
            { 5, "n-%" },
            { 6, "n%-" },
            { 7, "-% n " },
            { 8, "n %-" },
            { 9, "% n-" },
            { 10, "% -n" },
            { 11, "n- %" }
        };

        /// <summary>
        /// Returns the globalized JSON string.
        /// </summary>
        /// <param name="cultureData">Specific culture information.</param>
        /// <returns>Json serialized globalize string.</returns>
        public static string GetGlobalizeJsonString(CultureInfo cultureData)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(GetGlobalizeContent(cultureData));
        }

        /// <summary>
        /// Returns current culture information in the Dictionary format.
        /// </summary>
        /// <param name="cultureData">Specific culture information.</param>
        /// <returns>Localized contents.</returns>
        internal static Dictionary<string, object> GetGlobalizeContent(CultureInfo cultureData)
        {
            var number = cultureData.NumberFormat;
            var date = cultureData.DateTimeFormat;
            var timeSeparator = date.TimeSeparator;
            Dictionary<string, object> numberJson = new Dictionary<string, object>();
            numberJson.Add("mapper", ConvertStringArrayToString(cultureData.NumberFormat.NativeDigits, false, false));
            numberJson.Add("mapperDigits", string.Join(string.Empty, cultureData.NumberFormat.NativeDigits));
            numberJson.Add("numberSymbols", GenerateNumberSymbols(cultureData.NumberFormat, timeSeparator));
            numberJson.Add("timeSeparator", timeSeparator);
            var currencySymbol = number.CurrencySymbol;
            numberJson.Add("currencySymbol", number.CurrencySymbol);
            numberJson.Add("currencypData", GetPositiveCurrencyPercentData(number, false));
            numberJson.Add("percentpData", GetPositiveCurrencyPercentData(number, true));
            numberJson.Add("percentnData", GetNegativePercentData(number));
            numberJson.Add("currencynData", GetNegativeCurrencyData(number));
            numberJson.Add("decimalnData", DefaultPositiveNegativeData(number, true));
            numberJson.Add("decimalpData", DefaultPositiveNegativeData(number, false));
            var cultureObject = new Dictionary<string, object>()
            {
                {
                    cultureData.Name, new Dictionary<string, object>()
                    {
                        { "numbers", numberJson },
                        { "dates", GetDateFormatOptions(date) }
                    }
                }
            };
            return cultureObject;
        }

        private static Dictionary<string, object> GetDateFormatOptions(DateTimeFormatInfo date)
        {
            var dictionary = new Dictionary<string, object>();

            dictionary.Add("dayPeriods", new Dictionary<string, string>()
            {
                { "am", date.AMDesignator },
                { "pm", date.PMDesignator }
            });
            dictionary.Add("dateSeperator", date.DateSeparator);
            dictionary.Add("days", new Dictionary<string, object>()
            {
                { "abbreviated", ConvertStringArrayToString(date.AbbreviatedDayNames, true, false) },
                { "short", ConvertStringArrayToString(date.ShortestDayNames, true, false) },
                { "wide", ConvertStringArrayToString(date.DayNames, true, false) }
            });
            dictionary.Add("months", new Dictionary<string, object>()
                {
                    { "abbreviated", ConvertStringArrayToString(date.AbbreviatedMonthNames, false, true) },
                    { "wide", ConvertStringArrayToString(date.MonthNames, false, true) }
                });
            dictionary.Add("eras", EraData(date));
            return dictionary;
        }

        private static Dictionary<string, object> EraData(DateTimeFormatInfo date)
        {
            var eraCount = date.Calendar.Eras;
            var dictionary = new Dictionary<string, object>();
            foreach (var era in eraCount)
            {
                dictionary.Add(era.ToString(CultureInfo.CurrentCulture), date.GetAbbreviatedEraName(era));
            }

            return dictionary;
        }

        private static Dictionary<string, object> DefaultPositiveNegativeData(NumberFormatInfo numberFormat, bool isNegative)
        {
            string nlead = string.Empty;
            string nend = string.Empty;
            if (isNegative)
            {
                string curMapper = numberNegativePattern[numberFormat.NumberNegativePattern];
                string[] splitString = curMapper.Split("n");
                nlead = splitString[0];
                nend = splitString[1];
            }

            var numberData = new Dictionary<string, object>();
            numberData.Add("nlead", nlead);
            numberData.Add("nend", nend);
            numberData.Add("groupData", new Dictionary<string, int>() { { "primary", numberFormat.NumberGroupSizes[0] } });
            numberData.Add("maximumFraction", numberFormat.NumberDecimalDigits);
            numberData.Add("minimumFraction", numberFormat.NumberDecimalDigits);
            return numberData;
        }

        private static Dictionary<string, object> GetPositiveCurrencyPercentData(NumberFormatInfo numberFormat, bool isPercent)
        {
            var typeMapper = isPercent ? positivePercentMapper : positiveCurrencyMapper;
            string curMapper = typeMapper[isPercent ? numberFormat.PercentPositivePattern : numberFormat.CurrencyPositivePattern];
            string currencyString = curMapper.Replace("n", string.Empty, StringComparison.Ordinal);

            string nlead = string.Empty;
            string nend = string.Empty;
            char curSymbol = isPercent ? '%' : '$';
            if (curMapper[0].Equals(curSymbol))
            {
                nlead = currencyString.Replace(isPercent ? "%" : "$", numberFormat.CurrencySymbol, StringComparison.Ordinal);
            }
            else
            {
                nend = currencyString.Replace(isPercent ? "%" : "$", isPercent ? numberFormat.PercentSymbol : numberFormat.CurrencySymbol, StringComparison.Ordinal);
            }

            var percentData = new Dictionary<string, object>() { { "nlead", nlead }, { "nend", nend } };
            if (isPercent)
            {
                AddGroupandFractionPercentData(percentData, numberFormat);
            }
            else
            {
                AddGroupandFractionCurrencyData(percentData, numberFormat);
            }

            return percentData;
        }

        private static Dictionary<string, object> GetNegativeCurrencyData(NumberFormatInfo numberFormat)
        {
            string currencyMapper = negativeCurrencyMapper[numberFormat.CurrencyNegativePattern];
            var currencyData = NegativePatternProcessor(currencyMapper, "$", numberFormat.CurrencySymbol);
            AddGroupandFractionCurrencyData(currencyData, numberFormat);
            return currencyData;
        }

        private static Dictionary<string, object> NegativePatternProcessor(string mapper, string currencySymbol, string replacer)
        {
            string[] splitString = mapper.Split("n");
            string nlead = splitString[0].Replace(currencySymbol, replacer, StringComparison.Ordinal);
            string nend = splitString[1].Replace(currencySymbol, replacer, StringComparison.Ordinal);
            return new Dictionary<string, object>() { { "nlead", nlead }, { "nend", nend } };
        }

        private static Dictionary<string, object> GetNegativePercentData(NumberFormatInfo numberFormat)
        {
            string currencyMapper = negativePercentMapper[numberFormat.PercentNegativePattern];
            string[] splitString = currencyMapper.Split("n");
            var percentData = NegativePatternProcessor(currencyMapper, "%", numberFormat.PercentSymbol);
            AddGroupandFractionPercentData(percentData, numberFormat);
            return percentData;
        }

        private static void AddGroupandFractionCurrencyData(Dictionary<string, object> numberData, NumberFormatInfo numberFormat)
        {
            numberData.Add("groupSeparator", numberFormat.NumberGroupSeparator);
            numberData.Add("groupData", new Dictionary<string, int>() { { "primary", numberFormat.CurrencyGroupSizes[0] } });
            numberData.Add("maximumFraction", numberFormat.CurrencyDecimalDigits);
            numberData.Add("minimumFraction", numberFormat.CurrencyDecimalDigits);
        }

        private static void AddGroupandFractionPercentData(Dictionary<string, object> numberData, NumberFormatInfo numberFormat)
        {
            numberData.Add("groupSeparator", numberFormat.PercentGroupSeparator);
            numberData.Add("groupData", new Dictionary<string, int>() { { "primary", numberFormat.PercentGroupSizes[0] } });
            numberData.Add("maximumFraction", numberFormat.PercentDecimalDigits);
            numberData.Add("minimumFraction", numberFormat.PercentDecimalDigits);
        }

        private static Dictionary<string, string> GenerateNumberSymbols(NumberFormatInfo numberFormat, string timeSep)
        {
            var numberSymbols = new Dictionary<string, string>();
            numberSymbols.Add("decimal", numberFormat.NumberDecimalSeparator);
            numberSymbols.Add("group", numberFormat.NumberGroupSeparator);
            numberSymbols.Add("plusSign", numberFormat.PositiveSign);
            numberSymbols.Add("minusSign", numberFormat.NegativeSign);
            numberSymbols.Add("percentSign", numberFormat.PercentSymbol);
            numberSymbols.Add("nan", numberFormat.NaNSymbol);
            numberSymbols.Add("timeSeparator", timeSep);
            numberSymbols.Add("infinity", numberFormat.PositiveInfinitySymbol);
            return numberSymbols;
        }

        // Method used to convert string[] to Dictionary.
        private static Dictionary<string, string> ConvertStringArrayToString(string[] array, bool weekMode, bool? monthMode)
        {
            // Concatenate all the elements into a StringBuilder.
            string[] calendarWeekStrings = new string[] { "sun", "mon", "tue", "wed", "thu", "fri", "sat" };
            var digits = new Dictionary<string, string>();
            var i = 0;
            if (monthMode == true)
            {
                i = 1;
            }

            foreach (string item in array)
            {
                if (item.Length != 0)
                {
                    digits.Add(weekMode ? calendarWeekStrings[i] : i.ToString(CultureInfo.CurrentCulture), item);
                }

                i++;
            }

            return digits;
        }
    }

    /// <summary>
    /// A static class for the Internationalization common features and functionalities.
    /// </summary>
    internal static class Intl
    {
        /// <summary>
        /// Gets or sets current culture.
        /// </summary>
        internal static CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Gets or sets currency date.
        /// </summary>
        internal static Dictionary<string, string> CurrencyData { get; set; } = CultureInfo
           .GetCultures(CultureTypes.AllCultures)
           .Where(c => !c.IsNeutralCulture)
           .Select(culture =>
           {
               try
               {
                   return new RegionInfo(culture.Name);
               }
               catch
               {
                   return null;
               }
           })
           .Where(ri => ri != null)
           .GroupBy(ri => ri.ISOCurrencySymbol)
           .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);

        /// <summary>
        /// Gets or sets default culture.
        /// </summary>
        internal static CultureInfo DefaultCulture { get; set; } = new CultureInfo("en-US");

        /// <summary>
        /// Returns formatted date string based on the current culture.
        /// </summary>
        /// <typeparam name="T">The date format type.</typeparam>
        /// <param name="date">Date value to be formatted.</param>
        /// <param name="format">Format string for processing date format.</param>
        /// <param name="culture">Optional parameter to override the current culture.</param>
        /// <returns>Returns formatted string.</returns>
        internal static string GetDateFormat<T>(T date, string format = null, string culture = null)
        {
            try
            {
                var currentCulture = Intl.GetCulture(culture);
                IFormattable dateValue = date as IFormattable;
                var dateCulture = dateValue.ToString(format, currentCulture);
                dateCulture = Intl.GetNativeDigits(dateCulture, currentCulture.NumberFormat.NativeDigits);
                return dateCulture;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        /// <summary>
        /// Returns formatted number string based on the current culture.
        /// </summary>
        /// <typeparam name="T">The number format type.</typeparam>
        /// <param name="numberValue">Number value to be formatted.</param>
        /// <param name="format">Format string for processing number format.</param>
        /// <param name="culture">Optional parameter to override the current culture.</param>
        /// <param name="currencyCode">Optional parameter to process the currency code.</param>
        /// <returns>Returns formatted string.</returns>
        internal static string GetNumericFormat<T>(T numberValue, string format = null, string culture = null, string currencyCode = null)
        {
            try
            {
                CultureInfo currentCulture = (CultureInfo)Intl.GetCulture(culture).Clone();
                string cacheCurrency = currentCulture.NumberFormat.CurrencySymbol;
                if (currencyCode != null && CurrencyData.ContainsKey(currencyCode) && CurrencyData[currencyCode] != null)
                {
                    currentCulture.NumberFormat.CurrencySymbol = CurrencyData[currencyCode];
                }

                IFormattable numericValue = numberValue as IFormattable;
                var numericCulture = numericValue.ToString(format, currentCulture);
                numericCulture = Intl.GetNativeDigits(numericCulture, currentCulture.NumberFormat.NativeDigits);
                if (currencyCode != null)
                {
                    currentCulture.NumberFormat.CurrencySymbol = cacheCurrency;
                }

                return numericCulture;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        /// <summary>
        /// Returns the week of year based on the current culture.
        /// </summary>
        /// <param name="dateValue">Date value to be calculated.</param>
        /// <param name="culture">Optional parameter to override the current culture.</param>
        /// <param name="isLastDayOfWeek">Optional paramaeter to pass last day of week.</param>
        /// <param name="weekRule">Optional parameter to pass week rules.</param>
        /// <returns>Returns the week of year.</returns>
        internal static int GetWeekOfYear(DateTime dateValue, string culture = null, bool isLastDayOfWeek = false, CalendarWeekRule weekRule = CalendarWeekRule.FirstDay)
        {
            var currentCulture = Intl.GetCulture(culture);
            DayOfWeek dayOfWeek = isLastDayOfWeek ? dateValue.DayOfWeek : currentCulture.DateTimeFormat.FirstDayOfWeek;
            dateValue = isLastDayOfWeek ? dateValue.AddDays(6) : dateValue;
            int weekNumber = currentCulture.Calendar.GetWeekOfYear(dateValue, weekRule, dayOfWeek);
            return weekNumber;
        }

        /// <summary>
        /// Returns the narrow day names based on the current culture.
        /// </summary>
        /// <param name="culture">Optional parameter to override the current culture.</param>
        /// <returns>Returns the narrow day names.</returns>
        internal static List<string> GetNarrowDayNames(string culture = null)
        {
            var narrowDays = new List<string>();
            var currentCulture = Intl.GetCulture(culture);
            var shortDays = currentCulture.DateTimeFormat.ShortestDayNames;
            foreach (var dayName in shortDays)
            {
                narrowDays.Add(dayName[0].ToString());
            }

            return narrowDays;
        }

        /// <summary>
        /// Returns the current culture information.
        /// </summary>
        /// <param name="culture">Optional parameter to override the current culture.</param>
        /// <returns>Returns the current culture.</returns>
        internal static CultureInfo GetCulture(string culture = null)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                return new CultureInfo(culture);
            }
            else
            {
                return CultureInfo.CurrentCulture != null ? CultureInfo.CurrentCulture : Intl.DefaultCulture;
            }
        }

        /// <summary>
        /// Set culture info to Intl class.
        /// </summary>
        /// <param name="culture">Culture info needs to be set to the Intl.</param>
        internal static void SetCulture(CultureInfo culture)
        {
            Intl.CurrentCulture = culture;
            GlobalizeJsonGenerator.GetGlobalizeContent(culture);
        }

        /// <summary>
        /// Set culture string to Intl class.
        /// </summary>
        /// <param name="culture">Culture string needs to be set to the Intl.</param>
        internal static void SetCulture(string culture)
        {
            Intl.CurrentCulture = new CultureInfo(culture);
            Intl.CurrentCulture = new CultureInfo(culture);
        }

        /// <summary>
        /// Converts native digits based on the current culture.
        /// </summary>
        /// <param name="formatValue">Format to be converted to native digits.</param>
        /// <param name="nativeDigits">Native digits of the current culture.</param>
        /// <returns>Returns the current culture.</returns>
        internal static string GetNativeDigits(string formatValue, string[] nativeDigits)
        {
            return formatValue.Replace("0", nativeDigits[0], StringComparison.Ordinal)
                .Replace("1", nativeDigits[1], StringComparison.Ordinal)
                .Replace("2", nativeDigits[2], StringComparison.Ordinal)
                .Replace("3", nativeDigits[3], StringComparison.Ordinal)
                .Replace("4", nativeDigits[4], StringComparison.Ordinal)
                .Replace("5", nativeDigits[5], StringComparison.Ordinal)
                .Replace("6", nativeDigits[6], StringComparison.Ordinal)
                .Replace("7", nativeDigits[7], StringComparison.Ordinal)
                .Replace("8", nativeDigits[8], StringComparison.Ordinal)
                .Replace("9", nativeDigits[9], StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns the required patterns from the current culture.
        /// </summary>
        /// <param name="cultureCode">Culture code to be processed for the required patterns.</param>
        /// <returns>Returns culture patterns.</returns>
        internal static object GetCultureFormats(string cultureCode)
        {
            var culture = Intl.GetCulture(cultureCode);
            Dictionary<string, object> cultureFormats = new Dictionary<string, object>();

            // Get standard date formats
            var dateFormats = new Dictionary<string, object>();
            dateFormats["d"] = culture.DateTimeFormat.ShortDatePattern;
            dateFormats["D"] = culture.DateTimeFormat.LongDatePattern;
            dateFormats["f"] = culture.DateTimeFormat.LongDatePattern + " " + culture.DateTimeFormat.ShortTimePattern;
            dateFormats["F"] = culture.DateTimeFormat.FullDateTimePattern;
            dateFormats["g"] = culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern;
            dateFormats["G"] = culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.LongTimePattern;
            dateFormats["m"] = culture.DateTimeFormat.MonthDayPattern;
            dateFormats["M"] = culture.DateTimeFormat.MonthDayPattern;
            dateFormats["r"] = culture.DateTimeFormat.RFC1123Pattern;
            dateFormats["R"] = culture.DateTimeFormat.RFC1123Pattern;
            dateFormats["s"] = culture.DateTimeFormat.SortableDateTimePattern;
            dateFormats["t"] = culture.DateTimeFormat.ShortTimePattern;
            dateFormats["T"] = culture.DateTimeFormat.LongTimePattern;
            dateFormats["u"] = culture.DateTimeFormat.UniversalSortableDateTimePattern;
            dateFormats["U"] = culture.DateTimeFormat.FullDateTimePattern;
            dateFormats["y"] = culture.DateTimeFormat.YearMonthPattern;
            dateFormats["Y"] = culture.DateTimeFormat.YearMonthPattern;
            cultureFormats[cultureCode] = dateFormats;
            return cultureFormats;
        }
    }
}
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    internal class LocaleProvider
    {
        private ISyncfusionStringLocalizer localizer { get; set; }

        private readonly IDictionary<string, string> localeObj;

        internal LocaleProvider(ISyncfusionStringLocalizer localizer, IDictionary<string, string> localeObj)
        {
            this.localizer = localizer;
            this.localeObj = localeObj;
        }

        internal string GetText(string key)
        {
            return localizer?.GetText(key) ?? GetInternal(key);
        }

        internal string GetInternal(string key)
        {
            if (localeObj.ContainsKey(key))
            {
                return localeObj[key];
            }

            return null;
        }
    }

    internal static class LocaleStrings
    {
        internal static readonly IDictionary<string, string> Chart = new Dictionary<string, string>()
        {
            { "Chart_Pan", "Pan" },
            { "Chart_Reset", "Reset" },
            { "Chart_ResetZoom", "Reset Zoom" },
            { "Chart_Zoom", "Zoom" },
            { "Chart_ZoomIn", "Zoom In" },
            { "Chart_ZoomOut", "Zoom Out" }
        };
    }
}
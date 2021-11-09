using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class LocaleProvider
    {
        private ISyncfusionStringLocalizer _localizer;
        private IDictionary<string, string> _ui;

        public LocaleProvider(ISyncfusionStringLocalizer localizer, IDictionary<string, string> _uiStrings)
        {
            _localizer = localizer;
            _ui = _uiStrings;
        }

        public string GetText(string key)
        {
            return _localizer?.GetText(key) ?? GetInternal(key);
        }

        internal string GetInternal(string key)
        {
            if (_ui.ContainsKey(key))
            {
                return _ui[key];
            }

            return null;
        }
    }

    public static class LocaleStrings
    {
        public static readonly IDictionary<string, string> DiagramComponent =
            new Dictionary<string, string>()
            {
                { "DiagramComponent_X", "X" },
                { "DiagramComponent_Y", "Y" },
                { "DiagramComponent_W", "W" },
                { "DiagramComponent_H", "H" },
            };
    }
}

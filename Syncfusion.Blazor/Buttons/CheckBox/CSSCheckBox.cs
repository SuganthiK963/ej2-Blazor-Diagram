using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.FileManager")]

namespace Syncfusion.Blazor.Buttons.Internal
{
    internal class CSSCheckBox
    {
        public bool Checked { get; set; }

        public bool Indeterminate { get; set; }

        public string CssClass { get; set; }

        public bool EnableRtl { get; set; }

        public string Label { get; set; }

        public IDictionary<string, object> HtmlAttributes { get; set; }
    }

    public static partial class SfCheckBox
    {
        private const string SPAN = "span";
        private const string CLASSATTR = "class";
        private const string FRAME = "e-frame e-icons";
        private const string WRAPPER = "e-checkbox-wrapper e-css";

        [Inject]
        private static SyncfusionBlazorService SyncfusionService { get; set; }

        internal static RenderFragment CreateCheckBox(CSSCheckBox args = null)
        {
            int sqnes = 0;
            RenderFragment appendCheckBox;
            string _frameClass = FRAME;
            string _wrapperClass = WRAPPER;
            IDictionary<string, object> _cssAttributes = new Dictionary<string, object>();
            if (args == null)
            {
                args = new CSSCheckBox();
            }

            if (!string.IsNullOrEmpty(args.CssClass))
            {
                _wrapperClass += " " + args.CssClass;
            }

            if (args.EnableRtl)
            {
                _wrapperClass += " e-rtl";
            }

            if (args.Checked)
            {
                _frameClass += " e-check";
            }
            else if (args.Indeterminate)
            {
                _frameClass += " e-stop";
            }

            _cssAttributes.Add(CLASSATTR, _wrapperClass);
            appendCheckBox = builder =>
            {
                builder.OpenElement(sqnes++, "div");
                builder.AddMultipleAttributes(sqnes++, _cssAttributes);
                builder.AddMultipleAttributes(sqnes++, args.HtmlAttributes);
                builder.OpenElement(sqnes++, SPAN);
                builder.AddAttribute(sqnes++, CLASSATTR, _frameClass);
                builder.CloseElement();
                if (!string.IsNullOrEmpty(args.Label))
                {
                    builder.OpenElement(sqnes++, SPAN);
                    builder.AddAttribute(sqnes++, CLASSATTR, "e-label");
                    builder.AddContent(sqnes++, args.Label);
                    builder.CloseElement();
                }

                builder.CloseElement();
            };
            return appendCheckBox;
        }
    }
}

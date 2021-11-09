using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Buttons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// SplitButton component has primary and secondary button. Primary button is used to select 
    /// default action and secondary button is used to toggle contextual overlays for displaying list of 
    /// action items. It can contain both text and images.
    /// </summary>
    public partial class SfSplitButton : SfBaseComponent
    {
        private const string CREATED = "Created";
        private const string CONTAINER = "e-split-btn-wrapper";
        private const string SPLITBTN = "e-split-btn";
        private const string SFSPLITBTN = "sfsplitbutton";
        private const string CLICKED = "Clicked";
        private const string TABINDEX = "tabindex";
        private const string MINUSONE = "-1";
        private const string RTL = " e-rtl";
        private const string ICONBTN = "e-icon-btn";
        private const string SPACE = " ";

#pragma warning disable 0649
        private ElementReference wrapper;

        
#pragma warning restore 0649

        private SfButton primaryBtn { get; set; }

        internal SfDropDownButton secondryBtn { get; set; }

        private string cssClass;
        private bool enableRtl;
        private string id;
        private string dropdownBtnClass;
        private Dictionary<string, object> htmlAttributes;
        private Dictionary<string, object> htmlAttr;
        private string containerClass;

        internal SplitButtonEvents Delegates { get; set; }

        private void Initialize()
        {
            var containerCls = CONTAINER;
            var dropdownCls = ICONBTN;
            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                containerCls += RTL;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                containerCls += SPACE + CssClass;
                dropdownCls += SPACE + CssClass;
            }

            if (htmlAttributes != null && htmlAttributes.ContainsKey("id"))
            {
                id = (string)htmlAttributes["id"] + "_dropdownbutton";
            }
            dropdownBtnClass = dropdownCls;
            containerClass = containerCls;
        }

        private async Task KeyDownHandler(KeyboardEventArgs e)
        {
            await secondryBtn.KeyDownHandler(e);
        }

        internal void UpdateChildProperty(List<DropDownMenuItem> items)
        {
            Items = items;
            secondryBtn.itemsRendered = false;
        }
    }
}
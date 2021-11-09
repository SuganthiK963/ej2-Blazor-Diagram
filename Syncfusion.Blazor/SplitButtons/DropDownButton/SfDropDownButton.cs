using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// DropDownButton component is used to toggle contextual overlays for displaying list of action items.
    /// It can contain both text and images.
    /// </summary>
    public partial class SfDropDownButton : SfBaseComponent
    {
        private const string CREATED = "Created";
        private const string OPENED = "Opened";
        private const string CLOSED = "Closed";
        private const string ONCLOSE = "OnClose";
        private const string ONOPEN = "OnOpen";
        private const string ARROWDOWN = "ArrowDown";
        private const string DROPDOWNBTN = "e-dropdown-btn";
        private const string SFDROPDOWNBTN = "sfdropdownbutton";
        private const string SPACE = " ";
        private const string ACTIVE = " e-active";
        private const string VERTICAL = "e-vertical";
        private const string ICONBOTTOM = " e-icon-bottom";
        private const string POPUP = "e-dropdown-popup";
        private const string CONTROL = " e-control";
        private const string TRANSPARENT = " e-transparent";
        private const string RTL = " e-rtl";
        private const string ADDEVENT = "sfBlazor.DropDownButton.addEventListener";
        private const string OPENPOPUP = "sfBlazor.DropDownButton.openPopup";
        private const string REMOVEEVENT = "sfBlazor.DropDownButton.removeEventListener";
		private const string HASPOPUP = "aria-haspopup";
        private const string OWNS = "aria-owns";
        private const string LABEL = "aria-label";
        private const string DROPDOWN = "dropdownbutton";
        private const string EXPANDED = "aria-expanded";
        private const string TRUE = "true";
        private const string FALSE = "false";

        internal ElementReference popup { get; set; }

        internal DropDownButtonEvents Delegates { get; set; }

        internal bool hasIcon;
        internal bool itemsRendered;
        private bool renderPopup;
        private bool triggerOpenEvent;
        private bool triggerCloseEvent;
        private string popupId = POPUP + Guid.NewGuid().ToString();
        private string popupCls;
        private Dictionary<string, object> htmlAttributes;
        private Dictionary<string, object> htmlAttr;

        private SfButton btnRef { get; set; }

        [CascadingParameter]
        private ElementReference relateTo { get; set; }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ButtonClickAsync(MouseEventArgs e)
        {
            if (renderPopup)
            {
                var eventArgs = new BeforeOpenCloseMenuEventArgs()
                {
                    Cancel = false,
                    Name = ONCLOSE,
                    Element = popup,
                    Items = Items,
                    Event = e
                };
                await SfBaseUtils.InvokeEvent<BeforeOpenCloseMenuEventArgs>(Delegates?.OnClose, eventArgs);
                if (!eventArgs.Cancel)
                {
                    Close();
                    if (e == null)
                    {
                        StateHasChanged();
                    }
                }
                else
                {
                    await InvokeMethod(ADDEVENT, GetElement());
                }
            }
            else
            {
                await OpenPopup(e);
            }
        }

        private void Open()
        {
            var cls = POPUP + CONTROL + TRANSPARENT;
            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                cls += RTL;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                cls += SPACE + CssClass;
            }

            if (cls != popupCls)
            {
                popupCls = cls;
            }

            triggerOpenEvent = true;
            renderPopup = true;
            itemsRendered = false;
			if (htmlAttributes.ContainsKey(EXPANDED))
            {
                htmlAttributes.Remove(EXPANDED);
                htmlAttributes.Add(EXPANDED, TRUE);
            }
        }

        private void Close()
        {
            triggerCloseEvent = true;
            renderPopup = false;
            hasIcon = false;
			if (htmlAttributes.ContainsKey(EXPANDED))
            {
                htmlAttributes.Remove(EXPANDED);
                htmlAttributes.Add(EXPANDED, FALSE);
            }
        }

        internal async Task KeyDownHandler(KeyboardEventArgs e)
        {
            if (!renderPopup && e.AltKey && e.Code == ARROWDOWN)
            {
                await OpenPopup(e);
            }
        }

        private async Task OpenPopup(EventArgs e)
        {
            var eventArgs = new BeforeOpenCloseMenuEventArgs()
            {
                Cancel = false,
                Name = ONOPEN,
                Element = btnRef.btn,
                Items = Items,
                Event = e
            };
            await SfBaseUtils.InvokeEvent<BeforeOpenCloseMenuEventArgs>(Delegates?.OnOpen, eventArgs);
            if (!eventArgs.Cancel)
            {
                Open();
            }
        }

        private ElementReference GetElement()
        {
            return relateTo.Id == null ? btnRef.btn : relateTo;
        }

        internal void UpdateChildProperty(List<DropDownMenuItem> items)
        {
            Items = items;
            itemsRendered = false;
        }
    }
}
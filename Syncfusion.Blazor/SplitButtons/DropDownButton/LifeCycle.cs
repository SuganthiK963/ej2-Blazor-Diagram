using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.SplitButtons.Internal;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// DropDownButton component is used to toggle contextual overlays for displaying list of action items.
    /// It can contain both text and images.
    /// </summary>
    public partial class SfDropDownButton : SfBaseComponent
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            htmlAttr = htmlAttributes = Utils.GetBtnAttributes(htmlAttributes, SFDROPDOWNBTN, null);
            ScriptModules = SfScriptModules.SfDropDownButton;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.SplitbuttonsBase);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            htmlAttr = NotifyPropertyChanges(nameof(htmlAttributes), htmlAttributes, htmlAttr);
            if (PropertyChanges.Count > 0)
            {
                htmlAttr = htmlAttributes = Utils.GetBtnAttributes(htmlAttributes, SFDROPDOWNBTN, null);
            }
			if (!htmlAttributes.ContainsKey(HASPOPUP))
			{
				htmlAttributes.Add(HASPOPUP, TRUE);
			}
			if (!htmlAttributes.ContainsKey(EXPANDED))
			{
				htmlAttributes.Add(EXPANDED, FALSE);
			}
            if (!htmlAttributes.ContainsKey(OWNS))
			{
				htmlAttributes.Add(OWNS, "popup");
			}
			if (!htmlAttributes.ContainsKey(LABEL))
            {
                htmlAttributes.Add(LABEL, Content != null ? Content + SPACE + DROPDOWN : DROPDOWN);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = CREATED });
            }

            if (renderPopup)
            {
                if (triggerOpenEvent)
                {
                    triggerOpenEvent = false;
                    await InvokeMethod(OPENPOPUP, GetElement(), popup, DotnetObjectReference, hasIcon);
                    await SfBaseUtils.InvokeEvent(Delegates?.Opened, new OpenCloseMenuEventArgs() { Name = OPENED, Element = popup, Items = Items });
                }
            }
            else
            {
                if (triggerCloseEvent)
                {
                    triggerCloseEvent = false;
                    await SfBaseUtils.InvokeEvent<OpenCloseMenuEventArgs>(Delegates?.Closed, new OpenCloseMenuEventArgs() { Name = CLOSED, Element = btnRef.btn, Items = Items });
                }
            }
        }

        internal override void ComponentDispose()
        {
            if (IsRendered && renderPopup)
            {
                InvokeMethod(REMOVEEVENT, GetElement()).ContinueWith(t => { }, TaskScheduler.Current);
            }
        }
    }
}
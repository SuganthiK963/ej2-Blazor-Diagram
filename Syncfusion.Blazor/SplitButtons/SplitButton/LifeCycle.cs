using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.SplitButtons.Internal;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// SplitButton component has primary and secondary button. Primary button is used to select 
    /// default action and secondary button is used to toggle contextual overlays for displaying list of 
    /// action items. It can contain both text and images.
    /// </summary>
    public partial class SfSplitButton : SfBaseComponent
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            enableRtl = EnableRtl;
            cssClass = CssClass;
            Initialize();
            htmlAttr = htmlAttributes = Utils.GetBtnAttributes(htmlAttributes, SFSPLITBTN, id);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            cssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            htmlAttr = NotifyPropertyChanges(nameof(htmlAttributes), htmlAttributes, htmlAttr);
            if (PropertyChanges.Count > 0)
            {
                foreach (string key in PropertyChanges.Keys)
                {
                    if (key == nameof(htmlAttributes))
                    {
                        htmlAttr = htmlAttributes = Utils.GetBtnAttributes(htmlAttributes, SFSPLITBTN, id);
                    }
                    else
                    {
                        Initialize();
                    }
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = CREATED });
            }
        }
    }
}
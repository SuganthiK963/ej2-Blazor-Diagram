using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations.Internal
{
    public partial class SfMenuBase<TValue> : SfBaseComponent, IMenu
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            enableRtl = EnableRtl;
            cssClass = CssClass;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            cssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender && SyncfusionService.options.EnableRippleEffect)
            {
                await SfBaseUtils.RippleEffect(JSRuntime, Element, new RippleSettings() { Selector = RIPPLE });
            }
        }

        void IMenu.UpdateChildProperties(string key, object result)
        {
            UpdateChildProperties(key, result);
        }
    }
}
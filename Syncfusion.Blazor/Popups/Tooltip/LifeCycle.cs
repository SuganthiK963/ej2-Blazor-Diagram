using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    public partial class SfTooltip : SfBaseComponent
    {
        internal const string IDPREFIX = "tooltip-";
        internal const string WrapperClass = "e-blazor-hidden";

        // Override the initialized method to customize the component at server side.

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = IDPREFIX + Guid.NewGuid().ToString();
            }

            await base.OnInitializedAsync();
            TooltipContent = Content;
            TooltipCssClass = CssClass;
            TooltipEnableRtl = EnableRtl;
            TooltipHeight = Height;
            TooltipOffsetX = OffsetX;
            TooltipOffsetY = OffsetY;
            TooltipOpensOn = OpensOn;
            TooltipPosition = Position;
            TooltipTipPointerPosition = TipPointerPosition;
            TooltipWindowCollision = WindowCollision;
            TooltipWidth = Width;
            TooltipTarget = Target;
            TooltipIsSticky = IsSticky;
            attributes = SfBaseUtils.GetAttribtues(classList, new Dictionary<string, object>(TooltipHtmlAttributes != null ? TooltipHtmlAttributes : new Dictionary<string, object>()));
            ScriptModules = SfScriptModules.SfTooltip;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            TooltipContent = NotifyPropertyChanges(nameof(Content), Content, TooltipContent);
            TooltipCssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, TooltipCssClass);
            TooltipEnableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, TooltipEnableRtl);
            TooltipWindowCollision = NotifyPropertyChanges(nameof(WindowCollision), WindowCollision, TooltipWindowCollision);
            TooltipHeight = NotifyPropertyChanges(nameof(Height), Height, TooltipHeight);
            TooltipTarget = NotifyPropertyChanges(nameof(Target), Target, TooltipTarget);
            TooltipOffsetX = NotifyPropertyChanges(nameof(OffsetX), OffsetX, TooltipOffsetX);
            TooltipOffsetY = NotifyPropertyChanges(nameof(OffsetY), OffsetY, TooltipOffsetY);
            TooltipOpensOn = NotifyPropertyChanges(nameof(OpensOn), OpensOn, TooltipOpensOn);
            TooltipPosition = NotifyPropertyChanges(nameof(Position), Position, TooltipPosition);
            TooltipIsSticky = NotifyPropertyChanges(nameof(IsSticky), IsSticky, TooltipIsSticky);
            TooltipTipPointerPosition = NotifyPropertyChanges(nameof(TipPointerPosition), TipPointerPosition, TooltipTipPointerPosition);
            TooltipWidth = NotifyPropertyChanges(nameof(Width), Width, TooltipWidth);
            if (IsRendered)
            {
                attributes = SfBaseUtils.GetAttribtues(SfBaseUtils.RemoveClass(classList, WrapperClass), new Dictionary<string, object>(TooltipHtmlAttributes != null ? TooltipHtmlAttributes : new Dictionary<string, object>()));
            }

            if (PropertyChanges.Count > 0 && isScriptRendered)
            {
                await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.updateProperties", tooltipElement, GetProperties(), GetPorpertyChanges());
            }

            if (ContentTemplate != null)
            {
                Template.UpdateContent();
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>="Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Created, null);
            }

            if (renderWrapper)
            {
                await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.contentUpdated", new object[] { tooltipElement });
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            isScriptRendered = true;
            await InvokeMethod("sfBlazor.Tooltip.wireEvents", new object[] { tooltipElement,DotnetObjectReference, GetProperties(), GetEventsList() });
        }
    }
}
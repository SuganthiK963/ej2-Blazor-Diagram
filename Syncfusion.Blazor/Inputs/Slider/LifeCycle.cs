using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Popups;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Partial class SfSlider.
    /// </summary>
    public partial class SfSlider<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ScriptModules = SfScriptModules.SfSlider;
            DependentScripts.Add(Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Blazor.Internal.ScriptModules.Popup);
            SetLocale();
            ID = !string.IsNullOrEmpty(ID) ? ID : SfBaseUtils.GenerateID(IDPREFIX);
            FirstHandleTarget = HASH + ID + SPACE + HANDLEFIRST;
            SecondHandleTarget = HASH + ID + SPACE + HANDLESECOND;
            InputAttribute = new Dictionary<string, object>() { { SLIDERTYPE, INPUTTYPE }, { CLASS, HIDDENINPUT }, { TABINDEX, "-1" }, { NAME, ID }, { SLIDERCOMPONENTVALUE, Value } };
            componentAttribute = new Dictionary<string, object> { { IDATTR, ID }, { TABINDEX, "-1" }, { CLASS, CONTROLCLASS + SPACE + CssClass + SPACE } };
            if (SliderHtmlAttributes != null)
            {
                foreach (string key in SliderHtmlAttributes.Keys)
                {
                    if (key != CLASS)
                    {
                        SfBaseUtils.UpdateDictionary(key, SliderHtmlAttributes[key], componentAttribute);
                    }
                    else
                    {
                        componentAttribute[CLASS] = SfBaseUtils.AddClass(componentAttribute[CLASS].ToString(), SliderHtmlAttributes[CLASS].ToString());
                    }
                }
            }

            attributes = new Dictionary<string, object>();
            containerClass = (EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl)) ? containerClass += SPACE + RTL : containerClass;
            containerClass = ShowButtons ? containerClass += SPACE + SLIDERBUTTON : containerClass;
            containerClass = Orientation == SliderOrientation.Horizontal ? containerClass += SPACE + HORIZANTAL : containerClass += SPACE + VERTICAL;
            containerClass = !Enabled ? containerClass += SPACE + SLIDERCOMPONENTENABLED : containerClass;
            containerClass = ReadOnly ? containerClass += SPACE + SLIDERCOMPONENTREADONLY : containerClass;
            if (!string.IsNullOrEmpty(Width))
            {
                attributes.Add(STYLE, SLIDERWIDTH + ":" + Width);
            }

            attributes.Add(CLASS, containerClass);
            SliderCssClass = CssClass;
            SliderEnabled = Enabled;
            SliderIsImmediateValue = IsImmediateValue;
            SliderLocale  = SfSliderLocale;
            SliderMax = Max;
            SliderMin = Min;
            SliderReadOnly = ReadOnly;
            SliderShowButtons = ShowButtons;
            SliderStep = Step;
            SliderValue = Value;
            if (EnablePersistence)
            {
                PersistenceValues<TValue> persistenceValues = await InvokeMethod<PersistenceValues<TValue>>("window.localStorage.getItem", true, new object[] { ID });
                if (persistenceValues == null)
                {
                    await SetLocalStorage(ID, SerializeModel(this));
                }
                else
                {
                    PersistProperties(persistenceValues);
                }
            }
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>"Task".</returns>
        protected async override Task OnParametersSetAsync()
        {
            SliderCssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, SliderCssClass);
            SliderIsImmediateValue = NotifyPropertyChanges(nameof(IsImmediateValue), IsImmediateValue, SliderIsImmediateValue);
            SliderEnabled = NotifyPropertyChanges(nameof(Enabled), Enabled, SliderEnabled);
            SliderLocale = NotifyPropertyChanges(nameof(SfSliderLocale), SfSliderLocale, SliderLocale);
            SliderMax = NotifyPropertyChanges(nameof(Max), Max, SliderMax);
            SliderMin = NotifyPropertyChanges(nameof(Min), Min, SliderMin);
            SliderReadOnly = NotifyPropertyChanges(nameof(ReadOnly), ReadOnly, SliderReadOnly);
            SliderShowButtons = NotifyPropertyChanges(nameof(ShowButtons), ShowButtons, SliderShowButtons);
            SliderStep = NotifyPropertyChanges(nameof(Step), Step, SliderStep);
            NotifyPropertyChanges(nameof(Value), Value, SliderValue);
            if (!SfBaseUtils.Equals(Value, SliderValue))
            {
                if (EnablePersistence)
                {
                    await SetLocalStorage(ID, SerializeModel(this));
                }
            }

            Value = SliderValue = await SfBaseUtils.UpdateProperty<TValue>(Value, SliderValue, ValueChanged, SliderEditContext, ValueExpression);
            await DynamicPropertyChange();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>"Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender && SfSliderTooltip != null)
            {
                tooltipPos = Orientation == SliderOrientation.Horizontal ? SfSliderTooltip.Placement == TooltipPlacement.Before ? Position.TopCenter : Position.BottomCenter : SfSliderTooltip.Placement == TooltipPlacement.Before ? Position.LeftCenter : Position.RightCenter;
                tooltipOffSetX = (Orientation == SliderOrientation.Horizontal && Theme == MATERIAL) ? SfSliderTooltip.Format == null ? 3 : -10 : 0;
                tooltipOffSetY = (Orientation == SliderOrientation.Horizontal && Theme == MATERIAL) ? SfSliderTooltip.Placement == TooltipPlacement.After ? 10 : 0 : 0;
                StateHasChanged();
            }

            if (firstRender && SliderEvents != null && SliderEvents.Created.HasDelegate)
            {
                await SfBaseUtils.InvokeEvent<object>(SliderEvents.Created, null);
            }

            if (CallReposition && !firstRender)
            {
                await Reposition();
                CallReposition = false;
            }

            if (firstRender && SfSliderTicks?.Placement != Placement.None)
            {
                SfBaseUtils.UpdateDictionary(CLASS, containerClass + " e-scale-" + ((SfSliderTicks?.Placement == Placement.Before) ? "before" : (SfSliderTicks?.Placement == Placement.After) ? "after" : "both"), attributes);
                StateHasChanged();
            }
        }

        internal async override Task OnAfterScriptRendered()
        {
            Theme = await InvokeMethod<string>("sfBlazor.Slider.initialize", false, Slider, DotnetObjectReference, GetProperties());
            if (SfSliderTooltip != null && SfSliderTooltip.IsVisible)
            {
                IsMaterialTooltip = Theme == MATERIAL && Type != SliderType.Range;
                ShowTipPointer = Theme == MATERIAL || Theme == BOOTSTRAP || Theme == BOOTSTRAP4 || Theme == BOOTSTRAP5 || Theme == TAILWIND || Theme == TAILWINDDARK;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                SfSliderTooltip.ShowOn = IsMaterialTooltip ? TooltipShowOn.Always : (SfSliderTooltip.ShowOn == TooltipShowOn.Auto ? TooltipShowOn.Hover : SfSliderTooltip.ShowOn);
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
                attributes[CLASS] += IsMaterialTooltip ? SPACE + MATERIALSLIDER : string.Empty;
                if (Theme == MATERIAL && Type != SliderType.Range && SfSliderTooltip.Format == null)
                {
                    if (SfSliderTooltip.Placement == TooltipPlacement.Before)
                    {
                        tooltipClass = MATERIALTOOLTIPCLASS + SPACE + TOOLTIPPLACEMENTBEFORE;
                    }
                    else
                    {
                        tooltipClass = MATERIALTOOLTIPCLASS + SPACE + TOOLTIPPLACEMENTAFTER;
                    }
                }
                else
                {
                    tooltipClass = OTHERTOOLTIPCLASS;
                }
            }
        }
    }
}
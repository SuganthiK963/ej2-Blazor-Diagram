using System;
using System.Text.Json;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.Inputs.Slider.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The Slider component allows the user to select a value or range of values
    /// in-between the min and max range, by dragging the handle over the slider bar.
    /// </summary>
    public partial class SfSlider<TValue> : SfBaseComponent, ISlider
    {
        private int activeHandle = 1;
        private Position tooltipPos;
        private int tooltipOffSetX;
        private int tooltipOffSetY;
        private string prevTooltipContent;
        private string containerClass = "e-slider-container e-control-wrapper";
        private string tooltipClass;
        private string increase = INCREASE;
        private string decrease = DECREASE;
        private Dictionary<string, object> attributes = new Dictionary<string, object>();
        private Dictionary<string, object> componentAttribute = new Dictionary<string, object>();
        private List<ElementReference> ticksRef = new List<ElementReference>();

        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        public ElementReference Slider { get; set; }

        internal ElementReference FirstHandle { get; set; }

        internal ElementReference SecondHandle { get; set; }

        internal SfTooltip TooltipRef { get; set; }

        internal bool IsMaterialTooltip { get; set; }

        internal bool ShowTipPointer { get; set; }

        internal string FirstHandleTarget { get; set; }

        internal string SecondHandleTarget { get; set; }

        internal bool IsDestroyed { get; set; }

        internal bool ReRender { get; set; } = true;

        internal bool IsRePosition { get; set; } = true;

        internal bool CallReposition { get; set; }

        internal string Theme { get; set; }

        private Dictionary<string, object> InputAttribute { get; set; }

        internal SliderEvents<TValue> SliderEvents { get; set; }

        internal string TooltipContent { get; set; }

        private bool IsTooltipChange { get; set; }

        private static Dictionary<string, object> SetButtonAttr(string className, string title)
        {
            return new Dictionary<string, object>()
            {
                { CLASS, className },
                { TABINDEX, "-1" },
                { ARIALABEL, title },
                { TITLE, title }
            };
        }

        private Dictionary<string, object> SetHandleAttribute(string className)
        {
            return new Dictionary<string, object>()
            {
                { CLASS, className },
                { STYLE, "visibility:hidden" },
                { ROLE, "slider" }, { TABINDEX, "0" },
                { ARIAORIENTATION, Orientation.ToString().ToLower(CultureInfo.CurrentCulture) },
                { ARIAVALUEMIN, Min },
                { ARIAVALUEMAX, Max }
            };
        }

        private Dictionary<string, object> SetRangeBarAttr()
        {
            return new Dictionary<string, object>()
            {
                { CLASS, Type == SliderType.Range ? RANGEBAR + Orientation.ToString().ToLower(CultureInfo.CurrentCulture) : MINRANGE }
            };
        }

        /// <summary>
        /// Set Locale value for button.
        /// </summary>
        internal void SetLocale()
        {
            string incrementLocale = Localizer.GetText(INCREMENTTITLE);
            string decreaseLocale = Localizer.GetText(DECREMENTTITLE);
            increase = incrementLocale == null ? INCREASE : incrementLocale;
            decrease = decreaseLocale == null ? DECREASE : decreaseLocale;
        }

        /// <summary>
        /// Get Public property information.
        /// </summary>
        internal Dictionary<string, object> GetProperties()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add(nameof(Type), Type.ToString());
            properties.Add(nameof(Min), Min);
            properties.Add(nameof(Max), Max);
            properties.Add(nameof(Enabled), Enabled);
            properties.Add(nameof(Step), Step);
            properties.Add(nameof(ReadOnly), ReadOnly);
            properties.Add(nameof(Value), Value);
            properties.Add(nameof(Width), Width);
            properties.Add("Limits", SfSliderLimits != null ? GetLimitData() : null);
            properties.Add("Ticks", SfSliderTicks != null ? GetTicksData() : GetDefaultTickValue());
            properties.Add("Tooltip", SfSliderTooltip);
            properties.Add(nameof(EnableRtl), EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl));
            properties.Add(nameof(Orientation), Orientation);
            properties.Add(nameof(CustomValues), CustomValues);
            properties.Add("ColorRange", SfSliderColorRange);
            properties.Add(nameof(IsImmediateValue), IsImmediateValue);
            properties.Add("Events", SliderEvents);
            properties.Add("TooltipRef", TooltipRef);
            return properties;
        }

        /// <summary>
        /// Update Child Property Information.
        /// </summary>
        /// <exclude/>
        /// <param name="key">key.</param>
        /// <param name="value">value.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object value)
        {
            if (value != null)
            {
                if (key == "limits")
                {
                   SfSliderLimits = (SliderLimits)value;
                }
                else if (key == "ticks")
                {
                    SfSliderTicks =  (SliderTicks)value;
                    StateHasChanged();
                }
                else if (key == "tooltip")
                {
                    SfSliderTooltip  = (SliderTooltip)value;
                    StateHasChanged();
                }
                else
                {
                    SfSliderColorRange  = (List<ColorRangeDataModel>)value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get Slider Value.
        /// </summary>
        /// <returns>sliderValue.</returns>
        internal List<double> GetSliderValue()
        {
            Type valueType = typeof(TValue);
            List<double> sliderValue = new List<double>();
            if (valueType.IsArray)
            {
                if (valueType == typeof(double[]))
                {
                    foreach (double val in (double[])(object)Value)
                    {
                        sliderValue.Add(val);
                    }
                }
                else if (valueType == typeof(int[]))
                {
                    foreach (int val in (int[])(object)Value)
                    {
                        sliderValue.Add(val);
                    }
                }

                return sliderValue;
            }
            else
            {
                return new List<double>() { Convert.ToDouble(Value, CultureInfo.CurrentCulture) };
            }
        }

        /// <summary>
        /// Get Slider Limits data.
        /// </summary>
        /// <exclude/>
        /// <returns>SliderLimits().</returns>
        public SliderLimits GetLimitData()
        {
            return new SliderLimits()
            {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                MinEnd = SfSliderLimits.MinEnd,
                MinStart = SfSliderLimits.MinStart,
                StartHandleFixed = SfSliderLimits.StartHandleFixed,
                EndHandleFixed = SfSliderLimits.EndHandleFixed,
                Enabled = SfSliderLimits.Enabled,
                MaxEnd = SfSliderLimits.MaxEnd,
                MaxStart = SfSliderLimits.MaxStart
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            };
        }

        /// <summary>
        /// Get Slider ticks data.
        /// </summary>
        internal SliderTicks GetTicksData()
        {
            return new SliderTicks()
            {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                ShowSmallTicks = SfSliderTicks.ShowSmallTicks,
                SmallStep = SfSliderTicks.SmallStep,
                LargeStep = SfSliderTicks.LargeStep,
                Format = SfSliderTicks.Format,
                Placement = SfSliderTicks.Placement
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            };
        }

        /// <summary>
        /// Get Slider ticks default data.
        /// </summary>
        private static SliderTicks GetDefaultTickValue()
        {
            return new SliderTicks()
            {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                ShowSmallTicks = false,
                SmallStep = 1,
                LargeStep = 10,
                Format = null,
                Placement = Placement.None
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            };
        }

        /// <summary>
        /// Handling Dynamic Property changes.
        /// </summary>
        /// <exclude/>
        /// <returns>="Task".</returns>
        public async Task DynamicPropertyChange()
        {
            string classInfo = attributes[CLASS].ToString();
            if (PropertyChanges != null && PropertyChanges.Count > 0)
            {
                if (PropertyChanges.ContainsKey(nameof(CssClass)))
                {
                    componentAttribute[CLASS] = SfBaseUtils.AddClass(CONTROLCLASS, CssClass);
                }

                if (PropertyChanges.ContainsKey(nameof(Enabled)))
                {
                    attributes[CLASS] = classInfo = Enabled ? classInfo.Replace(SLIDERCOMPONENTENABLED, string.Empty, StringComparison.Ordinal) : SfBaseUtils.AddClass(classInfo, SLIDERCOMPONENTENABLED);
                }

                if (PropertyChanges.ContainsKey(nameof(Min)) || PropertyChanges.ContainsKey(nameof(Max)))
                {
                    CallReposition = true;
                    ReRender = false;
                }

                if (PropertyChanges.ContainsKey(nameof(ReadOnly)))
                {
                    attributes[CLASS] = classInfo = !ReadOnly ? classInfo.Replace(SLIDERCOMPONENTREADONLY, string.Empty, StringComparison.Ordinal) : SfBaseUtils.AddClass(classInfo, SLIDERCOMPONENTREADONLY);
                }

                if (PropertyChanges.ContainsKey(nameof(ShowButtons)))
                {
                    attributes[CLASS] = !ShowButtons ? classInfo.Replace(SLIDERBUTTON, string.Empty, StringComparison.Ordinal) : SfBaseUtils.AddClass(classInfo, SLIDERBUTTON);
                }

                if (PropertyChanges.Count > 1 || (PropertyChanges.Count == 1 && !PropertyChanges.ContainsKey(nameof(Value))))
                {
                    StateHasChanged();
                }

                await InvokeMethod("sfBlazor.Slider.updatedProperties", Slider, PropertyChanges);
            }
        }

        /// <summary>
        /// Update Value Property.
        /// </summary>
        /// <exclude/>
        /// <param name="args">args.</param>
        /// <param name="activeHandle">activeHandle.</param>
        /// <returns>="Task".</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task UpdateValue(TValue args, int activeHandle)
        {
            Value = SliderValue = await SfBaseUtils.UpdateProperty<TValue>(args, SliderValue, ValueChanged, SliderEditContext, ValueExpression);
            if (SfSliderTooltip != null && TooltipRef != null)
            {
                if (activeHandle != this.activeHandle)
                {
                    await TooltipRef.Close();
                    this.activeHandle = activeHandle;
                }

                TooltipContent = GetTooltipContent();
                if (TooltipContent != prevTooltipContent)
                {
                    await TooltipRef.Open(activeHandle == 1 ? FirstHandle : SecondHandle, null);
                }
            }
        }

        /// <summary>
        /// Trigger onChange / Value change event.
        /// </summary>
        /// <exclude/>
        /// <param name="args">args.</param>
        /// <returns>="Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerEvent(ChangeEventData<TValue> args)
        {
            try
            {
                if (args != null)
                {
                    SliderChangeEventArgs<TValue> data = new SliderChangeEventArgs<TValue>()
                    {
                        PreviousValue = args.PreviousValue,
                        Value = args.Value,
                        IsInteracted = true,
                        Text = GetTooltipContent()
                    };
                    data.SfSliderAction = data.SfSliderAction;
                    if (!args.IsValueChanged)
                    {
                        
                        data.Name = data.SfSliderAction = "OnChange";
                        await SfBaseUtils.InvokeEvent<SliderChangeEventArgs<TValue>>(SliderEvents?.OnChange, data);
                        IsTooltipChange = true;
                    }
                    else
                    {
                        data.Name = data.SfSliderAction = "ValueChange";
                        await SfBaseUtils.InvokeEvent<SliderChangeEventArgs<TValue>>(SliderEvents?.ValueChange, data);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Invalid Property Update", exception);
            }
        }

        /// <summary>
        /// Slider Handle Value.
        /// </summary>
        private async Task TooltipHandle(ElementReference handle)
        {
            if (TooltipRef != null)
            {
                await TooltipRef.Open(handle);
            }
        }

        /// <summary>
        /// update tooltip position.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>="Task".</returns>
        public async Task OnOpen(TooltipEventArgs args)
        {
            if (Theme == "material")
            {
                await InvokeMethod("sfBlazor.Slider.updateTooltipPosition", Slider, args?.Element.ID);
            }
        }

        /// <summary>
        /// Increase Slider Handle Value.
        /// </summary>
        /// <returns>="Task".</returns>
        private async Task IncreaseValue()
        {
            List<double> data = GetSliderValue();
            if (Type == SliderType.Range)
            {
                data[1] += Step;
                await TooltipHandle(SecondHandle);
                await InvokeMethod("sfBlazor.Slider.updateSliderValue", Slider, data);
            }
            else
            {
                data[0] += SliderStep;
                await TooltipHandle(FirstHandle);
                await InvokeMethod("sfBlazor.Slider.updateSliderValue", Slider, data[0]);
            }

            if (TooltipRef != null)
            {
                await TooltipRef.Close();
            }
        }

        /// <summary>
        /// Decrese Slider Value.
        /// </summary>
        private async Task DecreaseValue()
        {
            List<double> data = GetSliderValue();
            data[0] -= Step;
            await TooltipHandle(FirstHandle);
            if (Type != SliderType.Range)
            {
                await InvokeMethod("sfBlazor.Slider.updateSliderValue", Slider, data[0]);
            }
            else
            {
                await InvokeMethod("sfBlazor.Slider.updateSliderValue", Slider, data);
            }

            if (TooltipRef != null)
            {
                await TooltipRef.Close();
            }
        }

        /// <summary>
        /// Calling Ticks Rendered Event.
        /// </summary>
        /// <param name="ticksWrapperRef">args.</param>
        /// <param name="attributes">attributes.</param>
        /// <returns>="Task".</returns>
        public async Task<SliderTickRenderedEventArgs> TriggeredTicksRendered(ElementReference ticksWrapperRef, Dictionary<string, object> attributes)
        {
            SliderTickRenderedEventArgs eventArgs = new SliderTickRenderedEventArgs()
            {
                TickElementsRef = ticksRef,
                TicksContainerRef = ticksWrapperRef,
                HtmlAttributes = attributes
            };
            if (SliderEvents != null && SliderEvents.TicksRendered.HasDelegate)
            {
                await SfBaseUtils.InvokeEvent<SliderTickRenderedEventArgs>(SliderEvents.TicksRendered, eventArgs);
            }

            return eventArgs;
        }

        /// <summary>
        /// Calling Ticks Rendering Event.
        /// </summary>
        /// <param name="ticksRef">ticksRef.</param>
        /// <param name="text">text.</param>
        /// <param name="value">sliderValue.</param>
        /// <param name="attributes">attributes.</param>
        /// <returns>="Task".</returns>
        public async Task<SliderTickEventArgs> TriggeredTicksRendering(ElementReference ticksRef, string text, double value, Dictionary<string, object> attributes)
        {
            this.ticksRef.Add(ticksRef);
            SliderTickEventArgs eventArgs = new SliderTickEventArgs()
            {
                TickElementRef = ticksRef,
                Text = text,
                Value = value,
                HtmlAttributes = attributes
            };
            if (SliderEvents != null && SliderEvents.TicksRendering.HasDelegate)
            {
                await SfBaseUtils.InvokeEvent<SliderTickEventArgs>(SliderEvents.TicksRendering, eventArgs);
            }

            return eventArgs;
        }

        // get tooltip content based on slider value
        internal string GetTooltipContent()
        {
            if (SfSliderTooltip != null)
            {
                List<double> sliderValue = GetSliderValue();
                if (CustomValues != null)
                {
                    return Type.ToString() != RANGE ? CustomValues[(int)sliderValue[0]] : CustomValues[(int)sliderValue[0]] + " - " + CustomValues[(int)sliderValue[1]];
                }
                else
                {
                    string startValue = Intl.GetNumericFormat<double>(sliderValue[0], SfSliderTooltip.Format);
                    string endValue = Type.ToString() == RANGE ? Intl.GetNumericFormat<double>(sliderValue[1], SfSliderTooltip.Format) : null;
                    return Type.ToString() != RANGE ? startValue : startValue + " - " + endValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Update the Persistence value to local storage.
        /// </summary>
        private async Task SetLocalStorage(string persistId, string dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        /// <summary>
        /// Updating the persisting values to our component properties.
        /// </summary>
        internal static string SerializeModel(SfSlider<TValue> comp)
        {
            PersistenceValues<TValue> model = new PersistenceValues<TValue>
            {
                Value = comp.Value
            };
            return JsonSerializer.Serialize(model, new JsonSerializerOptions() { IgnoreNullValues = true });
        }

        // update the properties based on persistence value
        internal void PersistProperties(PersistenceValues<TValue> value)
        {
            try
            {
                if (value == null)
                {
                    return;
                }

                SliderValue = Value = value.Value;
                StateHasChanged();
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Invalid Property Update", exception);
            }
        }

        /// <summary>
        /// Closes the Tooltip Element.
        /// </summary>
        /// <returns>="Task".</returns>
        [JSInvokable]
        public async Task CloseTooltip()
        {
            await TooltipRef?.Close();
            StateHasChanged();
        }

        internal async Task OnRender()
        {
            SliderTooltipEventArgs<TValue> data = new SliderTooltipEventArgs<TValue>()
            {
                Value = Value,
                Text = GetTooltipContent()
            };
            if (IsImmediateValue || IsTooltipChange)
            {
                await SfBaseUtils.InvokeEvent<SliderTooltipEventArgs<TValue>>(SliderEvents?.OnTooltipChange, data);
                TooltipContent = data.Text;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                TooltipRef.Content = TooltipContent;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
                IsTooltipChange = false;
                prevTooltipContent = TooltipContent;
            }
        }

        internal async override void ComponentDispose()
        {
            if (IsRendered && !IsDestroyed)
            {
                IsDestroyed = true;
                if (SliderEvents != null && SliderEvents.Destroyed.HasDelegate)
                {
                    await SfBaseUtils.InvokeEvent<object>(SliderEvents.Destroyed, null);
                }

                await InvokeMethod("sfBlazor.Slider.destroy", Slider);
            }
        }
    }
}
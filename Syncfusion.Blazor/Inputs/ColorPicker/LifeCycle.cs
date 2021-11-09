using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// ColorPicker component is a user interface to select and adjust color values.
    /// It provides supports for various color specification like Red Green Blue, Hue Saturation Value and Hex codes.
    /// </summary>
    public partial class SfColorPicker : SfBaseComponent
    {
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfColorPicker;
            DependentScripts.Add(Blazor.Internal.ScriptModules.Popup);
            colorValue = Value;
            cssClass = CssClass;
            enableRtl = EnableRtl;
            disabled = Disabled;
            enableOpacity = EnableOpacity;
            mode = Mode;
            await base.OnInitializedAsync();
            Initialize();
            UpdateValue();

            // Used for In-place Editor Component.
            if (ColorPickerParent != null && Convert.ToString(ColorPickerParent.Type, CultureInfo.CurrentCulture) == "Color")
            {
                ColorPickerParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ColorPickerParent, this);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            NotifyPropertyChanges(nameof(Value), Value, colorValue);
            colorValue = Value = await SfBaseUtils.UpdateProperty(Value, colorValue, ValueChanged, ColorPickerEditContext, ValueExpression);
            cssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            disabled = NotifyPropertyChanges(nameof(Disabled), Disabled, disabled);
            enableOpacity = NotifyPropertyChanges(nameof(EnableOpacity), EnableOpacity, enableOpacity);
            mode = NotifyPropertyChanges(nameof(Mode), Mode, mode);
            if (PropertyChanges.Count > 0)
            {
                foreach (string key in PropertyChanges.Keys)
                {
                    if (key == nameof(Value))
                    {
                        UpdateValue();
                        colorPickerBase?.Update(nameof(Value), null);
                        StateHasChanged();
                    }
                    else if (key == nameof(Mode))
                    {
                        colorPickerBase?.Update(nameof(Mode), null);
                    }
                    else if (key == nameof(CssClass) || key == nameof(EnableRtl) || key == nameof(Disabled) || key == nameof(EnableOpacity))
                    {
                        Initialize();
                        StateHasChanged();
                    }
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Created, new { Name = "Created" });
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            var offset = await InvokeMethod<Offset>(INIT, false, element, DotnetObjectReference, Inline);
            if (offset != null)
            {
                if (offset.Height != 0 && offset.Width != 0)
                {
                    SetOffset(offset);
                }

                IsDevice = offset.IsDevice;
            }
        }
    }
}
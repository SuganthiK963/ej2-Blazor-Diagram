using System;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.CircularGauge.Internal;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// The circular gauge component is used to visualize the numeric values on the circular scale.
    /// The circular gauge contains labels, ticks, and an axis line to customize its appearance.
    /// </summary>
    public partial class SfCircularGauge : SfBaseComponent
    {
        /// <summary>
        /// OnAfterScriptRendered is an async method that is called after the script has been rendered.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        internal override async Task OnAfterScriptRendered()
        {
            if (AvailableSize != null && AvailableSize.Height == 0 && AvailableSize.Width == 0)
            {
                ElementInfo elementInfo = await InvokeMethod<ElementInfo>("sfBlazor.CircularGauge.initialize", false, new object[] { Element, GetInstance(), DotnetObjectReference, true });
                IsIE = elementInfo.IsIE;
                AvailableSize = ContainerSize(elementInfo.Width, elementInfo.Height);
                await SfBaseUtils.InvokeEvent<LoadedEventArgs>(CircularGaugeEvents?.OnLoad, new LoadedEventArgs() { Cancel = false });
                await ComponentRender();
            }
            else
            {
                await InvokeMethod<bool>("sfBlazor.CircularGauge.initialize", false, new object[] { Element, GetInstance(), DotnetObjectReference, false });
            }

            await InitiateAnimation();
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ScriptModules = SfScriptModules.SfCircularGauge;
            ThemeStyles = ThemeStyle.GetThemeStyle(Theme);
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("circulargauge");
            }
        }

        /// <summary>
        /// OnAfterRenderAsync is a lifecycle method that is invoked each time the component is rendered in the application.
        /// </summary>
        /// <param name="firstRender">Specifies the value indicating whether the component is rendered for the first time.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                SizeD elementSize = await InvokeMethod<SizeD>("sfBlazor.getElementBoundsById", false, new object[] { ID });
                if (elementSize != null && elementSize.Width != 0 && elementSize.Height != 0)
                {
                    elementSize.Width = !string.IsNullOrEmpty(Width) ? StringToNumber(Width, elementSize.Width) : elementSize.Width;
                    elementSize.Height = !string.IsNullOrEmpty(Height) ? StringToNumber(Height, elementSize.Height) : elementSize.Height;
                    AvailableSize = ContainerSize(elementSize.Width, elementSize.Height);
                    isInitialRender = true;
                    await ComponentRender();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
            if (AllowRefresh)
            {
                if (!isPropertyChanged)
                {
                    await ComponentRender();
                }
                else
                {
                    isPropertyChanged = false;
                }

                await InitiateAnimation();
            }
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (AllowPrint || AllowPdfExport || AllowImageExport)
            {
                DependentScripts.Add(Blazor.Internal.ScriptModules.SfSvgExport);
            }

            await OnGaugeParametersSet();
        }

        private async Task ComponentRender()
        {
            AllowRefresh = false;
            AnimationStarted = false;
            IsTooltip = false;
            AxisRenderer = new AxisRenderer(this);
#pragma warning disable CA1508
            await AxisRenderer?.Render();
#pragma warning restore CA1508
            StateHasChanged();
            await SfBaseUtils.InvokeEvent<LoadedEventArgs>(CircularGaugeEvents?.Loaded, new LoadedEventArgs() { Cancel = false });
        }

        private async Task InitiateAnimation()
        {
            if (IsRendered && !AnimationStarted)
            {
                BoundingClientRect gaugeRectBounds = await InvokeMethod<BoundingClientRect>("sfBlazor.CircularGauge.getElementBounds", false, new object[] { ID + "_AxesCollection" });
                if ((gaugeRectBounds != null && (gaugeRectBounds.Width != 0 || gaugeRectBounds.Height != 0)) || IsIE)
                {
                    await AnnotationRerenderLocation();
                }

                if (AllowAnimation)
                {
                    AnimationStarted = true;
                    await PointerAnimation();
                    AllowAnimation = false;
                }
            }
        }
    }
}

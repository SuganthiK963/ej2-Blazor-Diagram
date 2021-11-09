using System;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Resource manager component for loading init interop script in .NET Core 3.0 applications.
    /// </summary>
    public class ResourceManager : ComponentBase, IDisposable
    {
#if !NET5_0
        #region private property
        private bool isFirstResource;
        #endregion
#endif

        #region public properties

        /// <summary>
        /// Add the component and its dependent component locale keys from the LocaleService/GetMappingLocale method.
        /// </summary>
        [Parameter]
        public List<string> LocaleKeys { get; set; }

        #endregion

        #region internal properties

        /// <summary>
        /// Gets or sets parent component.
        /// </summary>
        [CascadingParameter]
        internal object Parent { get; set; }

        /// <summary>
        /// Gets or sets Syncfusion Blazor service.
        /// </summary>
        [Inject]
        internal SyncfusionBlazorService SyncfusionService { get; set; }

        /// <summary>
        /// Gets or sets IJSRuntime.
        /// </summary>
        [Inject]
        internal IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Gets or sets Syncfusion localizer.
        /// </summary>
        [Inject]
        [JsonIgnore]
        internal ISyncfusionStringLocalizer Localizer { get; set; }
        #endregion

        /// <summary>
        /// Dispose the unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the unmanaged resources.
        /// </summary>
        /// <param name="disposing">Boolean value to dispose the object.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                LocaleKeys = null;
            }
        }

        /// <summary>
        /// Renders the component to the supplied Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder.
        /// </summary>
        /// <param name="builder">A Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder that will receive the render output.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
#if !NET5_0
            /*
             * !SyncfusionService.options.IgnoreScriptIsolation - Prevent the script rendering based on end user input from SyncfusionService in Startup.cs.
             * isFirstResource - Ensure the component is first time alone render for the entire application.
             */
            if (builder != null && (!SyncfusionService.options.IgnoreScriptIsolation || SyncfusionService.IsEnabledScriptIsolation) && isFirstResource && !SyncfusionService.IsScriptRendered)
            {
                string scriptPath = "_content/Syncfusion.Blazor/scripts/syncfusion-blazor-" + SyncfusionService.ScriptHashKey + ".min.js";
#if SyncfusionBlazorCore
                scriptPath = "_content/Syncfusion.Blazor.Core/scripts/syncfusion-blazor-" + SyncfusionService.ScriptHashKey + ".min.js";
#endif
                builder.OpenElement(0, "script");
                builder.AddAttribute(1, "src", scriptPath);
                builder.AddAttribute(2, "async", true);
                builder.CloseElement();
            }
#endif
        }

        private void SetCulture()
        {
            var currentCulture = CultureInfo.CurrentCulture;

            // Set culture from default thread or current thread for WebAssembly application
            if (JsRuntime as IJSInProcessRuntime != null)
            {
                currentCulture = CultureInfo.DefaultThreadCurrentCulture;

                // Get culture info from client, if the default thread culture is not added explicitly.
                if (currentCulture == null)
                {
                    currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                }

                if (currentCulture.Calendar as GregorianCalendar == null)
                {
                    currentCulture.DateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
                }
            }

            Intl.SetCulture(currentCulture);
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (!SyncfusionService.IsCultureLoaded)
            {
                SyncfusionService.IsCultureLoaded = true;
                SetCulture();
            }

            if (Parent != null && LocaleKeys != null)
            {
                var parentType = Parent.GetType();
                var baseName = parentType.BaseType?.Name;
                if (baseName == "BaseComponent" || baseName == "SfBaseExtension")
                {
                    using LocalizerDetails localizer = new LocalizerDetails(Localizer.ResourceManager, Intl.CurrentCulture, SyncfusionService, LocaleKeys);
                    var locleText = parentType.GetProperty("LocaleText");
                    locleText?.SetValue(Parent, localizer.GetLocaleText());
                }
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
       protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && SyncfusionService.IsFirstResource)
            {
#if !NET5_0
                SyncfusionService.IsFirstResource = false;
                isFirstResource = true;
                StateHasChanged();
#endif
                if (SyncfusionService.options.IgnoreScriptIsolation)
                {
                     try
                    {
                        SyncfusionService.IsDeviceMode = await JsRuntime.InvokeAsync<bool>("sfBlazor.isDevice", SyncfusionService.options.EnableRtl);
                    }
#pragma warning disable CA1031
                    catch (Exception)
                    {
                    }
#pragma warning restore CA1031
                }
            }
        }
    }
}

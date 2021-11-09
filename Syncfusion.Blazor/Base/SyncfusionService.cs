using System;
using System.Linq;
using System.Net.Http;
using Microsoft.JSInterop;
using System.ComponentModel;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
#if !NETSTANDARD
using Microsoft.AspNetCore.Components;
#endif
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// An extension class controls methods to add the Syncfusion Blazor service to the <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection">service collection</see>.
    /// </summary>
    public static class SyncfusionBlazor
    {
        /// <summary>
        /// Adds Syncfusion Blazor service to the <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection">service collection</see>.
        /// </summary>
        [ObsoleteAttribute("This method is obsolete. Call AddSyncfusionBlazor method with options instead.", true)]
        public static IServiceCollection AddSyncfusionBlazor(this IServiceCollection services, bool disableScriptManager)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSyncfusionBlazor(options =>
            {
                options.IgnoreScriptIsolation = disableScriptManager;
            });
            return services;
        }

        /// <summary>
        /// Adds Syncfusion Blazor service to the <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection">service collection</see> and configure components global options.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        /// <param name="configure">A delegate that is used to configure <see cref="GlobalOptions"/>.</param>
        /// <returns>The collection of services.</returns>
        /// <example>
        /// <code lang="C#"><![CDATA[
        /// services.AddSyncfusionBlazor(options =>
        /// {
        ///     options.IgnoreScriptIsolation = true;
        /// });
        /// ]]></code>
        /// </example>
#pragma warning disable CS1573
        public static IServiceCollection AddSyncfusionBlazor(this IServiceCollection services, Action<GlobalOptions> configure = default)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
#if !NETSTANDARD
            services.Replace(ServiceDescriptor.Singleton<IComponentActivator, SfComponentActivator>());
#endif
            services.TryAddSingleton<ISyncfusionStringLocalizer, SyncfusionStringLocalizer>();
            services.AddScoped<SyncfusionBlazorService>();

            if (configure != null)
            {
                services.Configure<GlobalOptions>(configure);
            }

            // Server side Blazor doesn`t inject HttpClient https://github.com/aspnet/Blazor/issues/1588
            if (!services.Any(s => s.ServiceType == typeof(HttpClient)))
            {
                services.AddScoped<HttpClient>();
            }

            return services;
        }
#pragma warning restore CS1573
    }

    /// <summary>
    /// Represents an instance of Syncfusion Blazor service.
    /// </summary>
    public class SyncfusionBlazorService
    {
        internal readonly GlobalOptions options;

        /// <exclude />
        /// /// <summary>
        /// Initializes a new instance of the <see cref="SyncfusionBlazorService"/> class.
        /// </summary>
        /// <param name="configure">Configured global options for Syncfusion Blazor.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SyncfusionBlazorService(IOptions<GlobalOptions> configure)
        {
            options = configure != null ? configure.Value : new GlobalOptions();
            IsScriptRendered = options.IgnoreScriptIsolation;
            ScriptRendered = UpdateSyncfusionService;
        }

        internal static Action<bool> ScriptRendered { get; set; }

        /// <summary>
        /// Specifies global script rendering in the application, when IgnoreScriptIsolation is false.
        /// </summary>
        internal bool IsEnabledScriptIsolation { get; set; }

        /// <summary>
        /// Specifies the current culture is set to the library for all type of components.
        /// </summary>
        internal bool IsCultureLoaded { get; set; }

        /// <summary>
        /// Specifies the init JSInterop script is loaded, when DisableScriptManager is false.
        /// Specifies the IsDevice JSInterop call invoked, when DisableScriptManager is true.
        /// </summary>
        internal bool IsScriptRendered { get; set; }

        /// <summary>
        /// Specifies whether the license validated.
        /// </summary>
        internal bool IsLicenseValidated { get; set; }

        /// <summary>
        /// Specifies the application is rendering in device mode.
        /// </summary>
        internal bool IsDeviceMode { get; set; }

        /// <summary>
        /// Specifies the application is rendering in device mode.
        /// </summary>
        internal string ScriptHashKey { get; set; } = "2e50e1";

        /// <summary>
        /// Specifies the first component rendering in the application.
        /// </summary>
        internal bool IsFirstResource { get; set; } = true;

        /// <summary>
        /// Specifies the first BaseComponent inherited rendering in the application.
        /// </summary>
        internal bool IsFirstBaseResource { get; set; } = true;

        internal bool isNativeDependent { get; set; } 

#if NETSTANDARD
        internal List<ScriptDependencies> ScriptDependencies { get; set; } = new List<ScriptDependencies>();

        internal List<IBaseInit> SfBaseExtensions { get; set; } = new List<IBaseInit>();
#endif

        internal Dictionary<string, List<string>> LoadedLocale { get; set; } = new Dictionary<string, List<string>>();


        /// <summary>
        /// Enable Global Script to the Syncfusion Blazor components.
        /// </summary>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal void EnableScriptIsolation(bool isSfDependent = false)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (options.IgnoreScriptIsolation && !IsEnabledScriptIsolation)
            {
                IsScriptRendered = false;
                IsEnabledScriptIsolation = true;
            }
            if (isSfDependent)
            {
                this.IsFirstResource = true;
                this.isNativeDependent = true;
            }
        }

        /// <summary>
        /// Enable ripple effect to the Syncfusion Blazor components for material design theme.
        /// </summary>
        /// <param name="enable">Set false to disable ripple effect.</param>
        public void EnableRipple(bool enable = true)
        {
            options.EnableRippleEffect = enable;
        }

        /// <summary>
        /// Enable right-to-left text direction to the Syncfusion Blazor components.
        /// </summary>
        /// <param name="enable">Set false to disable right-to-left text direction.</param>
        public void EnableRtl(bool enable = true)
        {
            options.EnableRtl = enable;
        }

        /// <summary>
        /// Returns true when the application is running on a mobile or IPad device.
        /// This method should be called only in the OnAfterRenderAsync life cycle method.
        /// </summary>
        /// <returns>Returns true, if the application rendering in mobile or IPad devices.</returns>
        public async ValueTask<bool> IsDevice()
        {
            await Task.CompletedTask;
            return IsDeviceMode;
        }

#if NETSTANDARD
        internal async void UpdateSyncfusionService(bool isDevice)
#elif NET5_0 || NET6_0
        internal void UpdateSyncfusionService(bool isDevice)
#endif
        {
            IsDeviceMode = isDevice;
            IsScriptRendered = true;
#if NETSTANDARD
            ScriptDependencies.Reverse();
            if (!options.IgnoreScriptIsolation || this.isNativeDependent)
            {
                foreach (var dependency in ScriptDependencies)
                {
                    await dependency.ImportScripts();
                }
            }

            ScriptDependencies.Clear();
            SfBaseExtensions.Reverse();
            foreach (var component in SfBaseExtensions)
            {
                await component.OnInitRenderAsync();
            }

            SfBaseExtensions.Clear();
#endif
        }

        /// <exclude />
        /// <summary>Update script rendered property after the init script loaded in the web page.</summary>
        /// <param name="isDevice">Boolean value to identify whether the application is rendering in mobile or IPad devices.</param>
        /// <returns>Task.</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Task SetIsDevice(bool isDevice)
        {
            ScriptRendered.Invoke(isDevice);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// A class that provides options to configure global settings for our Syncfusion Blazor components.
    /// </summary>
    /// <example>
    /// <code lang="C#"><![CDATA[
    /// services.AddSyncfusionBlazor(options =>
    /// {
    ///     options.IgnoreScriptIsolation = true;
    /// });
    /// ]]></code>
    /// </example>
    public class GlobalOptions
    {
        /// <summary>
        /// Gets or sets whether the Syncfusion Blazor scripts are loaded internally using JavaScript Isolation or refer to the required scripts from the application-end for better performance. 
        /// </summary>
        /// <value>
        /// false, if the Blazor scripts are loaded from built-in source using JavaScript Isolation internally. The default value is false.
        /// </value>
        /// <remarks>
        /// When the property value is true, the scripts should be referenced externally in the application-end from NuGet or CDN or by generating from CRG.
        /// </remarks>
        /// <example>
        /// <para> You can add script reference in one of the following ways externally for better performance when <c>IgnoreScriptIsolation</c> is <c>true</c>.</para>
        /// <para><b>Reference scripts from NuGet</b></para>
        /// To add script reference for all component except PdfViewer and DocumentEditor.
        /// <code lang="html"><![CDATA[
        /// <script  src="_content/Syncfusion.Blazor/scripts/syncfusion-blazor.min.js"  type="text/javascript"></script>
        /// ]]></code>
        /// To add script reference for PdfViewer.
        /// <code lang="html"><![CDATA[
        /// <script  src="_content/Syncfusion.Blazor.PdfViewer/scripts/syncfusion-blazor-pdfviewer.min.js"  type="text/javascript"></script>
        /// ]]></code>
        /// To add script reference for DocumentEditor.
        /// <code lang="html"><![CDATA[
        /// <script  src="_content/Syncfusion.Blazor.WordProcessor/scripts/syncfusion-blazor-documenteditor.min.js"  type="text/javascript"></script>
        /// ]]></code>
        /// <para><b>Reference scripts from CDN</b></para>
        /// To add script reference for all component except PdfViewer and DocumentEditor.
        /// <code lang="html"><![CDATA[
        /// <script  src="https://cdn.syncfusion.com/blazor/19.3.43/syncfusion-blazor.min.js"  type="text/javascript"></script>
        /// ]]></code>
        /// To add script reference for PdfViewer.
        /// <code lang="html"><![CDATA[
        /// <script  src="https://cdn.syncfusion.com/blazor/19.3.43/syncfusion-blazor-pdfviewer.min.js"  type="text/javascript"></script>
        /// ]]></code>
        /// To add script reference for DocumentEditor.
        /// <code lang="html"><![CDATA[
        /// <script  src="https://cdn.syncfusion.com/blazor/19.3.43/syncfusion-blazor-documenteditor.min.js"  type="text/javascript"></script>
        /// ]]></code>
        /// <em>Note: Ensure to change the version in CDN link based on the syncfusion version you are using.</em>
        /// <para><b>Reference scripts by generating from <see href="https://blazor.syncfusion.com/crg/">Blazor CRG</see> for only used components</b></para>
        /// Generate a required component script and styles from Blazor custom resource generator(Blazor CRG) and refer to them in your application.
        /// </example>
        public bool IgnoreScriptIsolation { get; set; }

        /// <summary>
        /// Specifies whether the ripple effect is enabled in the application.
        /// </summary>
        /// <value>
        /// true, if the ripple effect is enabled in the application. The default value is false.
        /// </value>
        public bool EnableRippleEffect { get; set; }

        /// <summary>
        /// Specifies whether the Rtl mode enabled in the application.
        /// </summary>
        /// <value>
        /// true, if the Rtl mode enabled in the application. The default value is false.
        /// </value>
        public bool EnableRtl { get; set; }
    }
}

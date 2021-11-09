using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Menu is a graphical user interface that serve as navigation headers for your application.
    /// </summary>
    public partial class SfMenu<TValue> : SfMenuBase<TValue>
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            enableScrolling = EnableScrolling;
            hamburgerMode = HamburgerMode;
            Initialize();
            ScriptModules = SfScriptModules.SfMenu;
            DependentScripts.Add(Blazor.Internal.ScriptModules.NavigationsBase);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enableScrolling = NotifyPropertyChanges(nameof(EnableScrolling), EnableScrolling, enableScrolling);
            hamburgerMode = NotifyPropertyChanges(nameof(HamburgerMode), HamburgerMode, hamburgerMode);
            if (PropertyChanges.Count > 0)
            {
                foreach (string key in PropertyChanges.Keys)
                {
                    if (key == nameof(EnableScrolling))
                    {
                        await InvokeMethod(UPDATESCROLL, Element, EnableScrolling, EnableRtl || SyncfusionService.options.EnableRtl);
                    }
                    else if (key == nameof(HamburgerMode))
                    {
                        if (SubMenuOpen)
                        {
                            SubMenuOpen = false;
                        }

                        if (closeMenu)
                        {
                            closeMenu = false;
                        }

                        ClsCollection.Clear();
                        NavIdx.Clear();
                        Initialize();
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
                if (Fields == null)
                {
                    Fields = new MenuFieldSettings();
                }

                if (Items != null)
                {
                    StateHasChanged();
                }

                await SfBaseUtils.InvokeEvent<object>(Delegates == null ? SelfRefDelegates?.Created : Delegates.Created, new { Name = CREATED });
            }

            if (OpenEventArgs != null)
            {
                var eventArgs = OpenEventArgs;
                OpenEventArgs = null;
                await TriggerOpenCloseEvent(eventArgs, true, true);
            }

            if (OpenMenuEventArgs != null)
            {
                var eventArgs = OpenMenuEventArgs;
                OpenMenuEventArgs = null;
                await TriggerOpenCloseEvent(eventArgs, true, true);
            }

            if (CloseEventArgs != null)
            {
                var eventArgs = CloseEventArgs;
                CloseEventArgs = null;
                await TriggerOpenCloseEvent(eventArgs, false, false);
            }

            if (CloseMenuEventArgs != null)
            {
                var eventArgs = CloseMenuEventArgs;
                CloseMenuEventArgs = null;
                await TriggerOpenCloseEvent(eventArgs, false, false);
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            var args = new MenuOptions() { Element = Element, EnableScrolling = EnableScrolling, IsRtl = EnableRtl || SyncfusionService.options.EnableRtl };
            await InvokeMethod(INITIALIZE, args, DotnetObjectReference);
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                InvokeMethod(DESTROY, Element).ContinueWith(t => { }, TaskScheduler.Current);
            }
        }
    }
}

using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations 
{
    /// <summary>
    /// ContextMenu is a graphical user interface that appears on the user right click/touch hold operation.
    /// </summary>
    public partial class SfContextMenu<TValue> : SfMenuBase<TValue>
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            filter = Filter;
            target = Target;
            showOn = ShowOn;
            Initialize();
            ScriptModules = SfScriptModules.SfContextMenu;
            DependentScripts.Add(Blazor.Internal.ScriptModules.Popup);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            filter = NotifyPropertyChanges(nameof(Filter), Filter, filter);
            target = NotifyPropertyChanges(nameof(Target), Target, target);
            showOn = NotifyPropertyChanges(nameof(ShowOn), ShowOn, showOn);
            if (PropertyChanges.Count > 0)
            {
                foreach (string key in PropertyChanges.Keys)
                {
                    if (key == nameof(EnableRtl) || key == nameof(CssClass))
                    {
                        Initialize();
                    }
                    else
                    {
                        await InvokeMethod(PROPERTYCHANGED, Element, key, key == nameof(Target) ? Target : key == nameof(ShowOn) ? showOn : Filter);
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

                await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = CREATED });
            }
            else
            {
                if (CloseEventArgs != null)
                {
                    var eventArgs = CloseEventArgs;
                    CloseEventArgs = null;
                    await SfBaseUtils.InvokeEvent(Delegates?.Closed, eventArgs);
                }

                if (NavIdx.Count > 1 && OpenEventArgs != null)
                {
                    var eventArgs = OpenEventArgs;
                    OpenEventArgs = null;
                    if (IsDevice)
                    {
                        await InvokeMethod(CMENUPOS, Element, 0, 0, EnableRtl || SyncfusionService.options.EnableRtl, true, true);
                    }
                    else if (!IsMenu)
                    {
                        await InvokeMethod(SUBMENUPOS, Element, EnableRtl || SyncfusionService.options.EnableRtl, ShowItemOnClick, Top == null && Left == null);
                    }
                    if (Parent != null && IsMenu)
                    {
                        if (Parent.NavigationIndex == null || Parent.NavigationIndex.Count == 0)
                        {
                            Close();
                        }
                        var args = new MenuOptions()
                        {
                            Element = Parent.Element,
                            ItemIndex = Parent.NavigationIndex[0],
                            ShowItemOnClick = Parent.ShowItemOnClick,
                            EnableScrolling = Parent.EnableScrolling,
                            IsVertical = Parent.Orientation == Orientation.Vertical,
                            IsRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                            ScrollHeight = Parent.ScrollHeight
                        };
                        args.Popup = Element;
                        await InvokeMethod(MENUSUBMENUPOS, args);
                    }

                    await SfBaseUtils.InvokeEvent(Delegates?.Opened, eventArgs);
                    StateHasChanged();
                }
                else if (NavIdx.Count == 1 && OpenEventArgs != null)
                {
                    var eventArgs = OpenEventArgs;
                    if (manualOpen)
                    {
                        var cancel = manualOpen = false;
                        OpenEventArgs = null;
                        if (IsMenu)
                        {
                            var evtArgs = await TriggerBeforeOpenCloseEvent(Items[0], Items, ONOPEN, true);
                            cancel = evtArgs.Cancel;
                        }
                        if (!IsMenu)
                        {
                            await Task.Yield();
                            await InvokeMethod(CMENUPOS, Element, Left, Top, EnableRtl || SyncfusionService.options.EnableRtl, false, isCollision);
                        }

                        if (!cancel)
                        {
                            await SfBaseUtils.InvokeEvent(Delegates?.Opened, eventArgs);
                        }

                        StateHasChanged();
                    }
                    else if (!IsMenu)
                    {
                        OpenEventArgs = null;
                        await Task.Yield();
                        await InvokeMethod(CMENUPOS, Element, Left, Top, EnableRtl || SyncfusionService.options.EnableRtl, false, true);
                        await SfBaseUtils.InvokeEvent(Delegates?.Opened, eventArgs);
                        StateHasChanged();
                    }
                }
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            IsDevice = SyncfusionService.IsDeviceMode;
            await InvokeMethod(INITIALIZE, Element, Target, Filter, ShowOn, DotnetObjectReference);
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                InvokeMethod(DESTROY, Element, refElement).ContinueWith(t => { }, TaskScheduler.Current);
            }
        }
    }
}
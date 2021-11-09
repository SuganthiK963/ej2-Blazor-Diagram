using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The Toolbar control contains a group of commands that are aligned horizontally.
    /// </summary>
    public partial class SfToolbar : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfToolbar;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.NavigationsBase);
            UpdateLocalProperties();
            await base.OnInitializedAsync();
            allowKeyboard = AllowKeyboard;
            cssClass = CssClass;
            enableCollision = EnableCollision;
            enableRtl = EnableRtl;
            height = Height;
            overflowMode = OverflowMode;
            scrollStep = ScrollStep;
            width = Width;
            if (OverflowMode == OverflowMode.MultiRow)
            {
                IsInitialModeMultiRow = true;
            }
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            allowKeyboard = NotifyPropertyChanges(ALLOWKEYBOARD, AllowKeyboard, allowKeyboard);
            cssClass = NotifyPropertyChanges(CSSCLASS, CssClass, cssClass);
            enableCollision = NotifyPropertyChanges(ENABLECOLLISION, EnableCollision, enableCollision);
            enableRtl = NotifyPropertyChanges(ENABLERTL, EnableRtl, enableRtl);
            height = NotifyPropertyChanges(HEIGHT, Height, height);
            overflowMode = NotifyPropertyChanges(OVERFLOWMODE, OverflowMode, overflowMode);
            scrollStep = NotifyPropertyChanges(SCROLLSTEP, ScrollStep, scrollStep);
            width = NotifyPropertyChanges(WIDTH, Width, width);

            if (PropertyChanges.Count > 0)
            {
                await OnPropertyChangeHandler();
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                bool isStateChanged = SetItems();
                if (isStateChanged)
                {
                    StateHasChanged();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
            if (IsLoaded && IsItemChanged)
            {
                IsItemChanged = false;
                await InvokeMethod("sfBlazor.Toolbar.serverItemsRerender", new object[] { Element, Items });
                EventAggregator.Notify(ITEMS_CHANGED, null);
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            IsLoaded = true;
            await InvokeMethod("sfBlazor.Toolbar.initialize", new object[] { Element, GetInstance(), DotnetObjectReference });
            EventAggregator.Notify(INITIAL_LOAD, null);
            await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, null);
        }

#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
        internal override void ComponentDispose()
        {
            InvokeMethod("sfBlazor.Toolbar.destroy", Element).ContinueWith(async t =>
            {
                await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, null);
            });
        }
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
    }
}

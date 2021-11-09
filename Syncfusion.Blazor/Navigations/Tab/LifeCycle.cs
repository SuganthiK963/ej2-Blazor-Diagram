using System.Threading.Tasks;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Tab is a content panel to show multiple contents in a compact space. Also, only one tab is active at a time. Each Tab item has an associated content, that will be displayed based on the active Tab.
    /// </summary>
    public partial class SfTab : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = TABPREFIX + Guid.NewGuid().ToString();
            }

            ScriptModules = SfScriptModules.SfTab;
            UpdateLocalProperties();
            await base.OnInitializedAsync();
            UpdateAnimationProperties(Animation);
            cssClass = CssClass;
            enableRtl = EnableRtl;
            headerPlacement = HeaderPlacement;
            height = Height;
            tabitems = Items;
            overflowMode = OverflowMode;
            scrollStep = ScrollStep;
            selectedItem = SelectedItem;
            showCloseButton = ShowCloseButton;
            width = Width;
            allowDragAndDrop = AllowDragAndDrop;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            animation = NotifyPropertyChanges(ANIMATION, Animation, animation);
            cssClass = NotifyPropertyChanges(CSSCLASS, CssClass, cssClass);
            enableRtl = NotifyPropertyChanges(ENABLE_RTL, EnableRtl, enableRtl);
            headerPlacement = NotifyPropertyChanges(HEADER_PLACEMENT, HeaderPlacement, headerPlacement);
            height = NotifyPropertyChanges(HEIGHT, Height, height);
            tabitems = NotifyPropertyChanges(ITEMS, Items, tabitems);
            overflowMode = NotifyPropertyChanges(OVERFLOWMODE, OverflowMode, overflowMode);
            scrollStep = NotifyPropertyChanges(SCROLLSTEP, ScrollStep, scrollStep);
            selectedItem = NotifyPropertyChanges(SELECTED_ITEM, SelectedItem, selectedItem);
            showCloseButton = NotifyPropertyChanges(SHOWCLOSEBUTTON, ShowCloseButton, showCloseButton);
            width = NotifyPropertyChanges(WIDTH, Width, width);
            allowDragAndDrop = NotifyPropertyChanges(ALLOWDRAGANDDROP, AllowDragAndDrop, allowDragAndDrop);
            if (PropertyChanges.Count > 0 && !IsSelectedItemChanged)
            {
                await OnPropertyChangeHandler();
            }
            if (IsSelectedItemChanged)
            {
                PropertyChanges.Clear();
                IsSelectedItemChanged = false;
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            bool isTabItemPropertyChanged = IsTabItemChanged;
            if (firstRender || (PropertyChanges.Count > 0 || isTabItemPropertyChanged))
            {
                IJSInProcessRuntime runtime = JSRuntime as IJSInProcessRuntime;

                // Client side blazor will fail if it is single threaded https://github.com/dotnet/aspnetcore/issues/14253
                if (runtime != null)
                {
                    await Task.Yield();
                }

                if (firstRender && !(Items != null && Items.Count > 0))
                {
                    if (Items == null)
                    {
                        Items = new List<TabItem>();
                    }

                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = "Created" });
                    IsCreatedEvent = true;
                }

                if (firstRender || (PropertyChanges.ContainsKey(ITEMS) || PropertyChanges.ContainsKey(SELECTED_ITEM) || isTabItemPropertyChanged))
                {
                    await SetToolbarItems();
                    if ((Toolbar != null) && (PropertyChanges.ContainsKey(ITEMS) || PropertyChanges.ContainsKey(SELECTED_ITEM) || isTabItemPropertyChanged))
                    {
                        Toolbar.IsItemChanged = true;
                    }

                    IsTabItemChanged = false;
                    StateHasChanged();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        internal override async Task OnAfterScriptRendered()
        {
            IsTabScriptLoaded = true;
            if (EnablePersistence && !IsLoaded)
            {
                var localStorage = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { $"tab{ID}" });
                if (!string.IsNullOrEmpty(localStorage))
                {
                    var persistSelectedItem = JsonSerializer.Deserialize<int>(localStorage);
                    SelectedItem = selectedItem = persistSelectedItem;
                    SfBaseUtils.UpdateDictionary(SELECTED_ITEM, SelectedItem, PropertyChanges);
                    SelectContent();
                }
            }

            await InvokeMethod("sfBlazor.Tab.initialize", new object[] { Element, GetInstance(), DotnetObjectReference, IsLoaded, IsCreatedEvent });

            if (SyncfusionService.options.EnableRippleEffect && Toolbar != null)
            {
                await SfBaseUtils.RippleEffect(JSRuntime, Toolbar.Element, new RippleSettings() { Selector = ".e-tab-wrap" });
            }
        }

#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
        internal override void ComponentDispose()
        {
            InvokeMethod("sfBlazor.Tab.destroy", new object[] { Element, $"tab{ID}", SelectedItem }).ContinueWith(async t =>
            {
                await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, null);
            });
        }
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
    }
}

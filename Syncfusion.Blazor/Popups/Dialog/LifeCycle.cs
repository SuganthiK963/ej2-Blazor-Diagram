using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = DIALOG + "-" + Guid.NewGuid().ToString();
            }
            previousCssClass = CssClass;
            previousVisible = Visible;
            UpdateLocalProperties();
            UpdateLocale();
            await base.OnInitializedAsync();
            allowDragging = AllowDragging;
            UpdateChildProperties(ANIMATION_SETTINGS, AnimationSettingsValue);
            closeOnEscape = CloseOnEscape;
            cssClass = CssClass;
            enableResize = EnableResize;
            enableRtl = EnableRtl;
            height = Height;
            isModal = IsModal;
            minHeight = MinHeight;
            UpdateChildProperties(POSITION, PositionValue);
            target = Target;
            visible = Visible;
            width = Width;
            zIndex = ZIndex;
            isLoadOnDemand = IsLoadOnDemand;
            IsDemand = IsLoadOnDemand;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            ScriptModules = SfScriptModules.SfDialog;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            allowDragging = NotifyPropertyChanges(ALLOW_DRAGGING, AllowDragging, allowDragging);
            closeOnEscape = NotifyPropertyChanges(CLOSE_ON_ESCAPE, CloseOnEscape, closeOnEscape);
            cssClass = NotifyPropertyChanges(CSSCLASS, CssClass, cssClass);
            enableResize = NotifyPropertyChanges(ENABLE_RESIZE, EnableResize, enableResize);
            enableRtl = NotifyPropertyChanges(ENABLE_RTL, EnableRtl, enableRtl);
            height = NotifyPropertyChanges(HEIGHT, Height, height);
            isModal = NotifyPropertyChanges(IS_MODAL, IsModal, isModal);
            minHeight = NotifyPropertyChanges(MIN_HEIGHT, MinHeight, minHeight);
            target = NotifyPropertyChanges(TARGET, Target, target);
            visible = await SfBaseUtils.UpdateProperty(Visible, visible, VisibleChanged);
            width = NotifyPropertyChanges(WIDTH, Width, width);
            zIndex = NotifyPropertyChanges(ZINDEX, ZIndex, zIndex);
            if (IsLoadOnDemand && Visible && !preventVisibility && Visible != previousVisible)
            {
                this.IsDemand = true;
            }
            if (PropertyChanges.Count > 0)
            {
                List<string> changedKeys = new List<string>();
                foreach (string key in PropertyChanges.Keys)
                {
                    changedKeys.Add(key);
                }

                await ClientPropertyChangeHandler(changedKeys);
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender"> Set to true if this is the first time OnAfterRender(Boolean) has been invoked.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (IsRendered && previousVisible != Visible)
            {
                await ServerPropertyChangeHandler();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        internal override async Task OnAfterScriptRendered()
        {
            await InvokeMethod(JS_INITIALIZE, GetElementRef(), GetInstance(), DotnetObjectReference);
        }

        internal override async void ComponentDispose()
        {
            if (IsRendered)
            {
                try
                {
                    await InvokeMethod(JS_DESTROY, GetInstance(), JSRuntime is IJSInProcessRuntime);
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, new { Name = DESTROYED });
                    ChildContent = null;
                    AnimationSettingsValue = null;
                    ResizeHandles = null;
                    PositionValue = null;
                    onClosedArgs = null;
                    if (ButtonsValue != null)
                    {
                        ButtonsValue.Clear();
                    }

                    ButtonsValue = null;
                    headerRef = null;
                    modalHeaderRef = null;
                    contentRef = null;
                    modalContentRef = null;
                    footerRef = null;
                    modalFooterRef = null;
                    HeaderTemplate = null;
                    ContentTemplate = null;
                    FooterTemplates = null;
                    DialogElement = default(ElementReference);
                    ModalDialogElement = default(ElementReference);
                    Delegates = null;

                    if (dialogAttribute != null)
                    {
                        dialogAttribute.Clear();
                    }

                    dialogAttribute = null;
                    if (CloseIconAttributes != null)
                    {
                        CloseIconAttributes.Clear();
                    }

                    CloseIconAttributes = null;
                }
                catch (ObjectDisposedException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
                catch (TaskCanceledException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
                catch (InvalidOperationException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
            }
        }
    }
}
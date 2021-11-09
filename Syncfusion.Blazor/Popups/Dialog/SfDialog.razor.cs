using System;
using Microsoft.JSInterop;
using System.Globalization;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Popups.Internal;
using Microsoft.AspNetCore.Components.Web;


[assembly: InternalsVisibleTo("Syncfusion.Blazor.PivotView")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.FileManager")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Grids")]

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Represents the dialog component that displays the information and gets input from the user.
    /// Two types of dialog components are `Modal and Modeless(non-modal)` depending on their interaction with the parent application.
    /// </summary>
    public partial class SfDialog : SfBaseComponent
    {
        #region Element/Module reference
        private DialogHeader headerRef;

        private DialogFooter footerRef;

        private DialogContent contentRef;

        private DialogFooter modalFooterRef;

        private DialogHeader modalHeaderRef;

        private DialogContent modalContentRef;
        #endregion

        #region Internal variables
        private double index;

        private string styles;

        private string display;

        private bool previousVisible;

        private string previousCssClass;

        private string removedClass;

        private bool allowMaxHeight = true;

        private bool preventVisibility;

        private bool IsDemand;

        private BeforeCloseEventArgs onClosedArgs;

        private string dialogClass = "e-dialog e-lib e-blazor-hidden";

        private Dictionary<string, object> dialogAttribute = new Dictionary<string, object>();

        internal Dictionary<string, object> CloseIconAttributes { get; set; } = new Dictionary<string, object>();

        internal DialogEvents Delegates { get; set; }

        internal ElementReference DialogElement { get; set; }

        internal ElementReference ModalDialogElement { get; set; }

        internal RenderFragment HeaderTemplate { get; set; }

        internal RenderFragment ContentTemplate { get; set; }

        internal RenderFragment FooterTemplates { get; set; }
        #endregion

        #region Internal methods
        internal void UpdateTemplate(string name, RenderFragment template)
        {
            switch (name)
            {
                case nameof(Header):
                    HeaderTemplate = template;
                    break;
                case nameof(Content):
                    ContentTemplate = template;
                    break;
                case nameof(FooterTemplate):
                    FooterTemplates = template;
                    break;
            }
        }

        internal void UpdateButtons(List<DialogButton> buttons)
        {
            ButtonsValue = buttons;
            RefreshFooter();
        }

        internal void UpdateChildProperties(string key, object data)
        {
            if (key == POSITION)
            {
                PositionValue = (DialogPositionData)data;
            }
            else if (key == ANIMATION_SETTINGS)
            {
                AnimationSettings = (DialogAnimationSettings)data;
            }
        }

        internal void UpdateLocale()
        {
            CloseIconAttributes.Clear();
            CloseIconAttributes.Add(TYPE, BTN);
            CloseIconAttributes.Add(ARIA_LABEL, CLOSE);
            CloseIconAttributes.Add(TITLE, Localizer.GetText(DIALOG_CLOSE) ?? CLOSE);
        }

        internal void UpdateLocalProperties()
        {
            dialogClass += " " + POPUP_CLOSE;
            if (!string.IsNullOrEmpty(CssClass))
            {
                dialogClass = SfBaseUtils.AddClass(dialogClass, CssClass);
            }
            if (EnableResize)
            {
                dialogClass = SfBaseUtils.AddClass(dialogClass, RESIZABLE);
            }
            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                dialogClass = SfBaseUtils.AddClass(dialogClass, RTL);
            }
            if (IsModal)
            {
                dialogClass += " " + DIALOG_MODAL;
                styles = $"{Z_INDEX}: {ZIndex}";
                /* To show a modal dialog element at top of overlay element, we reduce overlay zindex by -1 */
                index = ZIndex - 1;
            }
            if (IsLoadOnDemand)
            {
                dialogClass = SfBaseUtils.AddClass(dialogClass, DIALOG_ON_LOAD_CLASS);
            }
            if (!Visible && IsModal)
            {
                display = NONE;
            }
            UpdateHtmlAttributes();
        }

        internal ElementReference GetElementRef()
        {
            return IsModal ? ModalDialogElement : DialogElement;
        }

        internal string GetTarget()
        {
            return Target ?? BODY;
        }

        internal string GetResizeDirection()
        {
            string direction = string.Empty;
            if (ResizeHandles != null)
            {
                for (int i = 0; i < ResizeHandles.Length; i++)
                {
                    if (ResizeHandles[i] == ResizeDirection.All)
                    {
                        direction = ALL_DIRECTIONS;
                        break;
                    }
                    else
                    {
                        string directionValue;
                        switch (ResizeHandles[i])
                        {
                            case ResizeDirection.SouthEast:
                                directionValue = SOUTH_EAST;
                                break;
                            case ResizeDirection.SouthWest:
                                directionValue = SOUTH_WEST;
                                break;
                            case ResizeDirection.NorthEast:
                                directionValue = NORTH_EAST;
                                break;
                            case ResizeDirection.NorthWest:
                                directionValue = NORTH_WEST;
                                break;
                            default:
                                directionValue = ResizeHandles[i].ToString();
                                break;
                        }
                        direction = direction + directionValue.ToLower(CultureInfo.InvariantCulture) + SPACE;
                    }
                }
                if (EnableRtl && direction.Trim() == SOUTH_EAST)
                {
                    direction = SOUTH_WEST;
                }
                else if (EnableRtl && direction.Trim() == SOUTH_WEST)
                {
                    direction = SOUTH_EAST;
                }
            }
            return direction;
        }

        internal bool IsHeaderContentExist()
        {
            return (HeaderTemplate != null && string.IsNullOrEmpty(Header)) || (HeaderTemplate == null && !string.IsNullOrEmpty(Header));
        }

        internal void UpdateHtmlAttributes()
        {
            #pragma warning disable CS0618
            Dictionary<string, object> attributes = HtmlAttributes;
            #pragma warning restore CS0618
            if (attributes != null)
            {
                foreach (var item in attributes)
                {
                    if (item.Key == CLASS)
                    {
                        dialogClass = SfBaseUtils.AddClass(dialogClass, (string)item.Value);
                    }
                    else if (item.Key == STYLE)
                    {
                        if (dialogAttribute.ContainsKey(STYLE))
                        {
                            dialogAttribute[item.Key] += item.Value.ToString();
                        }
                        else
                        {
                            dialogAttribute = SfBaseUtils.UpdateDictionary(item.Key, item.Value, dialogAttribute);
                        }
                    }
                    else
                    {
                        dialogAttribute = SfBaseUtils.UpdateDictionary(item.Key, item.Value, dialogAttribute);
                    }
                }
            }
        }

        internal Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> dlgObj = new Dictionary<string, object>
            {
                { ELEMENT, GetElementRef() },
                { DICTIONARY_TARGET, GetTarget() },
                { DICTIONARY_WIDTH, Width },
                { DICTIONARY_HEIGHT, Height },
                { DICTIONARY_ZINDEX, ZIndex },
                { DICTIONARY_VISIBLE, Visible },
                { DICTIONARY_IS_MODAL, IsModal },
                { DICTIONARY_CSSCLASS, CssClass },
                { ALLOWMAXHEIGHT, allowMaxHeight },
                { DICTIONARY_ENABLE_RTL, EnableRtl || SyncfusionService.options.EnableRtl },
                { DICTIONARY_MIN_HEIGHT, MinHeight },
                { PREVENT_VISIBILITY, preventVisibility },
                { DICTIONARY_ENABLE_RESIZE, EnableResize },
                { DICTIONARY_ALLOW_DRAGGING, AllowDragging },
                { DICTIONARY_CLOSE_ON_ESCAPE, CloseOnEscape },
                { RESIZE_ICON_DIRECTION, GetResizeDirection() },
                { CREATED_ENABLED, Delegates?.Created.HasDelegate },
                { DICTIONARY_LOAD_ON_DEMAND, IsLoadOnDemand }
            };
            Dictionary<string, string> position = new Dictionary<string, string>
            {
                
                { X, (Position != null && Position.X != null) ? Position.X : CENTER },
                { Y, (Position != null && Position.Y != null) ? Position.Y : CENTER }
            };
            dlgObj.Add(POSITION, position);
            Dictionary<string, object> animation = new Dictionary<string, object>()
            {
                { DELAY, AnimationSettings != null ? AnimationSettings.Delay : 0 },
                { DURATION, AnimationSettings != null ? AnimationSettings.Duration : 400 },
                { ANIMATE_EFFECT, AnimationSettings != null ? AnimationSettings.Effect.ToString() : FADE }
                
            };
            dlgObj.Add(ANIMATION_SETTINGS, animation);
            return dlgObj;
        }

        internal void Refresh()
        {
            RefreshHeader();
            RefreshContent();
            RefreshFooter();
        }

        internal void RefreshHeader()
        {
            if (IsModal)
            {
                modalHeaderRef?.Refresh();
            }
            else
            {
                headerRef?.Refresh();
            }
        }

        internal void RefreshContent()
        {
            if (IsModal)
            {
                modalContentRef?.Refresh();
            }
            else
            {
                contentRef?.Refresh();
            }
        }

        internal void RefreshFooter()
        {
            if (IsModal)
            {
                modalFooterRef?.Refresh();
            }
            else
            {
                footerRef?.Refresh();
            }
        }

        internal async Task HideDialog(string action, BeforeCloseEventArgs args = null)
        {
            if (preventVisibility)
            {
                BeforeCloseEventArgs eventArgs = new BeforeCloseEventArgs()
                {
                    Cancel = false,
                    ClosedBy = action ?? USER_ACTION,
                    Event = args?.Event,
                    IsInteracted = args != null && args.Event != null,
                    Element = new DOM() { ID = ID },
                    Container = new DOM() { ID = IsModal ? (MODAL + "-" + ID) : ID }

                };
                onClosedArgs = eventArgs;
                await SfBaseUtils.InvokeEvent<BeforeCloseEventArgs>(Delegates?.OnClose, eventArgs);
                if (!eventArgs.Cancel)
                {
                    await InvokeMethod(JS_HIDE, GetElementRef());
                    if (IsLoadOnDemand)
                    {
                        this.IsDemand = false;
                    }
                    preventVisibility = false;
                    previousVisible = Visible;
                    visible = await SfBaseUtils.UpdateProperty(false, visible, VisibleChanged);
                }
                else
                {
                    previousVisible = Visible;
                    visible = await SfBaseUtils.UpdateProperty(true, visible, VisibleChanged);
                }
            }
        }

        internal async Task ServerPropertyChangeHandler()
        {
            previousVisible = Visible;
            if (Visible && !preventVisibility)
            {
                if (isLoadOnDemand)
                {
                    await InvokeMethod(JS_INITIALIZE, GetElementRef(), GetInstance(), DotnetObjectReference);
                }
                await ShowDialog();
            }
            else if (!Visible)
            {
                await HideDialog(null);
            }
        }

        internal async Task ClientPropertyChangeHandler(List<string> changedKeys)
        {
            dialogClass = await InvokeMethod<string>(JS_GET_CLASS_LIST, false, GetElementRef());
            if (string.IsNullOrEmpty(dialogClass))
            {
                dialogClass = SfBaseUtils.AddClass(dialogClass, "e-popup" + " " + POPUP_CLOSE);
            }
            List<string> changedProps = new List<string>();
            List<string> simpleProps = new List<string>() { WIDTH, HEIGHT, MIN_HEIGHT, ZINDEX, TARGET, CLOSE_ON_ESCAPE };
            if (changedKeys.Contains(CSSCLASS))
            {
                if (!string.IsNullOrEmpty(previousCssClass))
                {
                    dialogClass = SfBaseUtils.RemoveClass(dialogClass, previousCssClass);
                    removedClass = previousCssClass;
                }
                if (!string.IsNullOrEmpty(CssClass))
                {
                    dialogClass = SfBaseUtils.AddClass(dialogClass, CssClass);
                }
                if (dialogClass.Contains(removedClass, StringComparison.Ordinal))
                {
                    dialogClass = SfBaseUtils.RemoveClass(dialogClass, removedClass);
                }
                previousCssClass = CssClass;
            }
            if (changedKeys.Contains(ISMODAL))
            {
                dialogClass = IsModal ? SfBaseUtils.AddClass(dialogClass, DIALOG_MODAL) : SfBaseUtils.RemoveClass(dialogClass, DIALOG_MODAL);
            }
            if (changedKeys.Contains(ENABLE_RTL))
            {
                dialogClass = EnableRtl ? SfBaseUtils.AddClass(dialogClass, RTL) : SfBaseUtils.RemoveClass(dialogClass, RTL);
                changedProps.Add(DICTIONARY_ENABLE_RTL);
            }
            if (changedKeys.Contains(ENABLE_RESIZE))
            {
                dialogClass = EnableResize ? SfBaseUtils.AddClass(dialogClass, RESIZABLE) : SfBaseUtils.RemoveClass(dialogClass, RESIZABLE);
                changedProps.Add(DICTIONARY_ENABLE_RESIZE);
            }
            for (int i = 0; i < simpleProps.Count; i++)
            {
                if (changedKeys.Contains(simpleProps[i]))
                {
#pragma warning disable CA1304
                    changedProps.Add(char.ToLower(simpleProps[i][0]) + simpleProps[i].Substring(1));
#pragma warning restore CA1304
                }
            }
            if (changedKeys.Contains(ALLOW_DRAGGING))
            {
                if (!AllowDragging)
                {
                    changedProps.Add(DESTROY_DRAGGABLE);
                }
                else if (AllowDragging && (IsHeaderContentExist() || ShowCloseIcon))
                {
                    changedProps.Add(DICTIONARY_ALLOW_DRAGGING);
                }
            }
            if (changedProps.Count > 0)
            {
                await InvokeMethod(JS_PROPERTY_CHANGED, GetInstance(), changedProps);
                if (changedProps.Contains(DESTROY_DRAGGABLE))
                {
                    dialogClass = await InvokeMethod<string>(JS_GET_CLASS_LIST, false, GetElementRef());
                }
            }
        }
        #endregion

        #region Event handler methods
        internal async Task OverlayClickHandler(MouseEventArgs args)
        {
            if (Delegates?.OnOverlayModalClick.HasDelegate == true)
            {
                OverlayModalClickEventArgs eventArgs = new OverlayModalClickEventArgs() { Event = args, PreventFocus = false };
                await SfBaseUtils.InvokeEvent<OverlayModalClickEventArgs>(Delegates?.OnOverlayModalClick, eventArgs);
                if (!eventArgs.PreventFocus)
                {
                    await InvokeMethod(JS_FOCUS_CONTENT, GetElementRef());
                }
            }
            else
            {
                await SfBaseUtils.InvokeEvent<MouseEventArgs>(Delegates?.OnOverlayClick, args);
                await InvokeMethod(JS_FOCUS_CONTENT, GetElementRef());
            }
        }

        internal async Task OverlayModalClickHandler(MouseEventArgs args)
        {
            OverlayModalClickEventArgs eventArgs = new OverlayModalClickEventArgs() { Event = args, PreventFocus = false };
            await SfBaseUtils.InvokeEvent<OverlayModalClickEventArgs>(Delegates?.OnOverlayModalClick, eventArgs);
            if (!eventArgs.PreventFocus)
            {
                await InvokeMethod(JS_FOCUS_CONTENT, GetElementRef());
            }
        }
        #endregion

        #region JSInterop methods

        /// <summary>
        /// Method invoked after component has been rendered.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreatedEvent()
        {
            await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = CREATED });
        }

        /// <summary>
        /// Method invoked after dialog opened.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OpenEvent(string classes)
        {
            dialogClass = classes;
            OpenEventArgs eventArgs = new OpenEventArgs()
            {
                Name = OPENED,
                Cancel = false,
                PreventFocus = false,
                Element = new DOM() { ID = ID },
                Container = new DOM() { ID = IsModal ? (MODAL + "-" + ID) : ID }
            };
            await SfBaseUtils.InvokeEvent<OpenEventArgs>(Delegates?.Opened, eventArgs);
            if (!eventArgs.PreventFocus)
            {
                await Task.Yield(); // it ensure that the child component rendered.
                await InvokeMethod(JS_FOCUS_CONTENT, GetElementRef());
            }
        }

        /// <summary>
        /// Method invoked after dialog closed.
        /// </summary>
        /// <param name="classes">Specifiy the class names.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CloseEvent(string classes)
        {
            dialogClass = classes;
            await SfBaseUtils.InvokeEvent<CloseEventArgs>(Delegates?.Closed, new CloseEventArgs()
            {
                Name = CLOSED,
                Event = onClosedArgs.Event,
                Cancel = onClosedArgs.Cancel,
                ClosedBy = onClosedArgs.ClosedBy,

                Element = onClosedArgs.Element,
                Container = onClosedArgs.Container,

                IsInteracted = onClosedArgs.IsInteracted,
            });
            await InvokeMethod(JS_POPUP_CLOSE, GetElementRef());
        }

        /// <summary>
        /// Method invoked when start to drag the dialog.
        /// </summary>
        /// <param name="args">Defines the DragStart Event arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DragStartEvent(DragStartEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            await SfBaseUtils.InvokeEvent<DragStartEventArgs>(Delegates?.OnDragStart, new DragStartEventArgs()
            {
                Name = DRAG_START,
                Event = args.Event,
                Target = args.Target,
                Element = new DOM() { ID = ID },
            });
        }

        /// <summary>
        /// Method invoked when drag the dialog.
        /// </summary>
        /// <param name="args">Defines the Drag Event arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DragEvent(DragEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            await SfBaseUtils.InvokeEvent<DragEventArgs>(Delegates?.OnDrag, new DragEventArgs()
            {
                Name = DRAG,
                Event = args.Event,
                Target = args.Target,
                Element = new DOM() { ID = ID }
            });
        }

        /// <summary>
        /// Method invoked when complete the drag action.
        /// </summary>
        /// <param name="args">Defines the DragStop Event arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DragStopEvent(DragStopEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            await SfBaseUtils.InvokeEvent<DragStopEventArgs>(Delegates?.OnDragStop, new DragStopEventArgs()
            {
                Name = DRAG_STOP,
                Event = args.Event,
                Target = args.Target,
                Helper = new DOM() { ID = ID },
                Element = new DOM() { ID = ID }
            });
        }

        /// <summary>
        /// Method invoked when start to resize the dialog.
        /// </summary>
        /// <param name="args">Defines the Mouse Event arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ResizeStartEvent(MouseEventArgs args)
        {
            await SfBaseUtils.InvokeEvent<MouseEventArgs>(Delegates?.OnResizeStart, args);
        }

        /// <summary>
        /// Method invoked while resizing the dialog.
        /// </summary>
        /// <param name="args">Defines the Mouse Event arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ResizingEvent(MouseEventArgs args)
        {
            await SfBaseUtils.InvokeEvent<MouseEventArgs>(Delegates?.Resizing, args);
        }

        /// <summary>
        /// Method invoked after the dialog resize.
        /// </summary>
        /// <param name="args">Defines the Mouse Event arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ResizeStopEvent(MouseEventArgs args)
        {
            await SfBaseUtils.InvokeEvent<MouseEventArgs>(Delegates?.OnResizeStop, args);
        }

        /// <summary>
        /// Method invoked after the dialog resize.
        /// </summary>
        /// <param name="isFullScreen">Specifies the dialog is opened on full screen or not.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ShowDialog(bool? isFullScreen = null)
        {
            string maxHeight = await InvokeMethod<string>(JS_GET_MAX_HEIGHT, false, GetElementRef());
            BeforeOpenEventArgs eventArgs = new BeforeOpenEventArgs()
            {
                Cancel = false,
                MaxHeight = maxHeight,
                Element = new DOM() { ID = ID },
                Container = new DOM() { ID = IsModal ? (MODAL + "-" + ID) : ID }
            };
            await SfBaseUtils.InvokeEvent<BeforeOpenEventArgs>(Delegates?.OnOpen, eventArgs);
            if (!eventArgs.Cancel)
            {
                if (maxHeight != eventArgs.MaxHeight)
                {
                    allowMaxHeight = false;
                }
                await InvokeMethod(JS_SHOW, isFullScreen, eventArgs.MaxHeight, GetInstance());
                preventVisibility = true;
                previousVisible = Visible;
                visible = await SfBaseUtils.UpdateProperty(true, visible, VisibleChanged);
            }
            else
            {
                previousVisible = Visible;
                visible = await SfBaseUtils.UpdateProperty(false, visible, VisibleChanged);
            }
        }

        /// <summary>
        /// Method invoked after dialog closed.
        /// </summary>
        /// <param name="args">Specifies KeyBoard arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CloseDialog(KeyboardEventArgs args)
        {
            await HideDialog(ESCAPE, new BeforeCloseEventArgs() { Event = args });
        }
        #endregion
    }
}
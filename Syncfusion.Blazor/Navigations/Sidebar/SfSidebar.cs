using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The SfSidebar component is an expandable and collapsible component that typically acts as a side container to place primary or secondary content alongside the main content.
    /// </summary>
    public partial class SfSidebar : SfBaseComponent
    {
        internal const string DOCKER = "e-dock";
        internal const string SPACE = " ";
        internal const string RTL = "e-rtl";
        internal const string VISIBILITY = "e-visibility";
        internal const string LEFT = "e-left";
        internal const string RIGHT = "e-right";
        internal const string OPEN = "e-open";
        internal const string TRANSITION = "e-transition";
        internal const string CLOSE = "e-close";
        internal const string SLIDE = "e-slide";
        internal const string ANIMATION = "e-disable-animation";
        internal const string ABSOLUTE = "e-sidebar-absolute";
        internal const string OVER = "e-over";
        internal const string PUSH = "e-push";
        internal const string OVERFLOW = " e-sidebar-overflow";
        internal const string IDPREFIX = "sidebar-";

        private string sidebarClass = string.Empty;

        private string styles = string.Empty;

        private ElementReference element;

        private bool isDeviceMode;

        private bool isMediaQueryOpen;

        private bool isDestroyed;

        private bool isInteracted;

        private bool openState;

        private bool isVisible;

        private bool isScriptRendered;

        private Dictionary<string, object> attributes = new Dictionary<string, object>();

        private double LeftPosition { get; set; }

        private double TopPosition { get; set; }

        // Returns the valid dimension for given width value.
        private static string SetDimension(string width)
        {
            return (width.Contains("px", StringComparison.Ordinal) || width.Contains("%", StringComparison.Ordinal) || width.Contains("em", StringComparison.Ordinal) || width.Contains("auto", StringComparison.Ordinal)) ? width : width + "px";
        }

        // Initial updates with properties like setting Dock width and media query values.
        [Obsolete]
        private async Task InitRender()
        {
            if ((Type == SidebarType.Auto && !isDeviceMode) || (Type != SidebarType.Auto && IsOpen && isMediaQueryOpen))
            {
                openState = true;
                await SidebarShow();
            }
            else if (!IsOpen && !sidebarClass.Contains(CLOSE, StringComparison.Ordinal))
            {
                sidebarClass = SfBaseUtils.AddClass(sidebarClass, CLOSE);
            }

            if (EnableDock)
            {
                SetDock();
            }
        }

        // Initial updates with properties like setting Dock width and media query values.
        internal async Task SidebarInitRender()
        {
            if (isMediaQueryOpen && ((Type == SidebarType.Auto && !isDeviceMode) || (Type != SidebarType.Auto && IsOpen)))
            {
                openState = true;
                await SidebarShow();
            }
            else if (!IsOpen && !sidebarClass.Contains(CLOSE, StringComparison.Ordinal))
            {
                sidebarClass = SfBaseUtils.AddClass(sidebarClass, CLOSE);
            }

            if (EnableDock)
            {
                SetDock();
            }
        }

        // Returns the style properties of sidebar component by adding default Z-index value.
        private void GetStyle()
        {
            styles = string.Empty;
            styles += " z-index: " + ZIndex + ";";
            if (!openState)
            {
                styles += SPACE + "width: " + SetDimension(EnableDock ? DockSize : Width) + ";";
            }
            else
            {
                styles += SPACE + "width: " + SetDimension(Width) + ";";
            }
        }

        // Returns the basic root classes to be added for the sidebar component.
        private void GetClass()
        {
            string classNames = "e-control e-sidebar e-lib e-hidden";
            if (EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl))
            {
                classNames = SfBaseUtils.AddClass(classNames, RTL);
            }

            if (EnableDock)
            {
                classNames = SfBaseUtils.AddClass(classNames, DOCKER);
            }
            else if (Type != SidebarType.Auto)
            {
                classNames = SfBaseUtils.AddClass(classNames, VISIBILITY);
            }

            classNames = SfBaseUtils.AddClass(classNames, Position == SidebarPosition.Left ? LEFT : RIGHT);
            if (!Animate)
            {
                classNames = SfBaseUtils.AddClass(classNames, ANIMATION);
            }

            if (!string.IsNullOrEmpty(Target))
            {
                classNames = SfBaseUtils.AddClass(classNames, ABSOLUTE);
            }

            classNames = SfBaseUtils.AddClass(classNames, TRANSITION);
            classNames = SfBaseUtils.AddClass(classNames, !openState ? CLOSE : OPEN);
            switch (Type)
            {
                case SidebarType.Push:
                    classNames = SfBaseUtils.AddClass(classNames, PUSH);
                    break;
                case SidebarType.Slide:
                    classNames = SfBaseUtils.AddClass(classNames, openState ? SLIDE + OVERFLOW : SLIDE);
                    break;
                case SidebarType.Over:
                    classNames = SfBaseUtils.AddClass(classNames, OVER);
                    break;
                default:
                    classNames = SfBaseUtils.AddClass(classNames, isDeviceMode ? OVER : PUSH);
                    break;
            }

            if (isScriptRendered)
            {
                classNames = SfBaseUtils.RemoveClass(classNames, "e-hidden");
            }

            sidebarClass = classNames;
        }

        private void UpdateClass()
        {
            GetClass();
            GetStyle();
            UpdateAttributes();
            StateHasChanged();
        }

        /// <summary>
        /// Update the Persistence value to local storage.
        /// </summary>
        private async Task SetLocalStorage(string persistId, string dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        /// <summary>
        /// Updating the persisting values to our component properties.
        /// </summary>
        private string SerializeModel()
        {
            return JsonSerializer.Serialize(new PersistenceValues { IsOpen = IsOpen });
        }

        /// <summay>
        /// Updates attributes added for the Sidebar component.
        /// </summay>
        private void UpdateAttributes()
        {
            attributes = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("id", ID, attributes);
            SfBaseUtils.UpdateDictionary("tabindex", "0", attributes);
            SfBaseUtils.UpdateDictionary("class", sidebarClass, attributes);
            SfBaseUtils.UpdateDictionary("style", styles, attributes);
            if (SidebarHtmlAttributes != null)
            {
                foreach (string key in SidebarHtmlAttributes.Keys)
                {
                    if (key == "class")
                    {
                        SfBaseUtils.UpdateDictionary("class", SfBaseUtils.AddClass(attributes["class"].ToString(), SidebarHtmlAttributes[key].ToString()), attributes);
                    }
                    else
                    {
                        attributes[key] = SidebarHtmlAttributes[key];
                    }
                }
            }
        }

        /// <summary>
        ///   Updates the dock styles and classes for the sidebar element.
        /// </summary>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetDock()
        {
            GetStyle();
            if (EnableDock && !openState)
            {
                int dimension = Position == SidebarPosition.Left ? -100 : 100;
                string transform = Position == SidebarPosition.Left ? SetDimension(DockSize) : "-" + SetDimension(DockSize);
                styles += " transform: translateX(" + dimension + "%) translateX(" + transform + ")";
            }

            UpdateAttributes();
        }

        /// <summary>
        /// Triggers change event.
        /// </summary>
        /// <exclude/>
        /// <param name="visible">visibles.</param>
        /// <param name="argsvalue">argsvalue.</param>
        /// <returns>"Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerChange(bool visible, ChangeEventArgs argsvalue = null)
        {
            if (isVisible != visible)
            {
                ChangeEventArgs eventArgs = new ChangeEventArgs
                {
                    Element = element,
                    Name = "Changed",
                    IsInteracted = argsvalue != null,
                };
                await SfBaseUtils.InvokeEvent<ChangeEventArgs>(Changed, eventArgs);
                isVisible = visible;
            }
        }

        // Updates the type of the sidebar component.
        private async Task SetSidebarType()
        {
            await InvokeMethod("sfBlazor.Sidebar.setType", new object[] { element, GetProperties() });
        }

        // Dynamic porperty changes handler
        [Obsolete]
        private async Task PropertyChange(Dictionary<string, object> propertyChanges)
        {
            if (propertyChanges.ContainsKey(nameof(Position)))
            {
                SetDock();
                GetClass();
                await SetSidebarType();
                UpdateAttributes();
            }

            if (propertyChanges.ContainsKey(nameof(Width)) && !isVisible)
            {
                SetDock();
            }

            if (propertyChanges.ContainsKey(nameof(Type)))
            {
                GetClass();
                await CheckType();
                UpdateAttributes();
            }

            if (propertyChanges.ContainsKey(nameof(IsOpen)))
            {
                if (IsOpen != openState)
                {
                    if (IsOpen)
                    {
                        await SidebarShow();
                    }
                    else
                    {
                        await Hide();
                    }
                }
            }

            if (propertyChanges.ContainsKey(nameof(CloseOnDocumentClick)) || propertyChanges.ContainsKey(nameof(ShowBackdrop)))
            {
                await InvokeMethod("sfBlazor.Sidebar.onPropertyChange", new object[] { element, propertyChanges });
            }
        }

        // Dynamic porperty changes handler
        internal async Task SidebarPropertyChange(Dictionary<string, object> propertyChanges)
        {
            if (propertyChanges.ContainsKey(nameof(Position)))
            {
                SetDock();
                GetClass();
                await SetSidebarType();
                UpdateAttributes();
            }

            if (propertyChanges.ContainsKey(nameof(Width)))
            {
                SetDock();
            }

            if (propertyChanges.ContainsKey(nameof(Type)))
            {
                GetClass();
                await CheckType();
                UpdateAttributes();
            }

            if (propertyChanges.ContainsKey(nameof(IsOpen)))
            {
                if (IsOpen != openState)
                {
                    if (IsOpen)
                    {
                        await SidebarShow();
                    }
                    else
                    {
                        await SidebarHide();
                    }
                }
            }

            if (propertyChanges.ContainsKey(nameof(CloseOnDocumentClick)) || propertyChanges.ContainsKey(nameof(ShowBackdrop)))
            {
                await InvokeMethod("sfBlazor.Sidebar.onPropertyChange", new object[] { element, propertyChanges });
            }
        }

        /// <summary>
        ///  Invoke show method from client.
        /// </summary>
        /// <exclude/>
        /// <param name="args">args.</param>
        /// <returns>"Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerShow(EventArgs args)
        {
            isMediaQueryOpen = true;
            isInteracted = args != null;
            await SidebarShow();
        }

        /// <summary>
        ///  Invoke hide method from client.
        ///  </summary>
        /// <exclude/>
        /// <param name="args">args.</param>
        /// <returns>"Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerHide(EventArgs args)
        {
            isMediaQueryOpen = false;
            isInteracted = args != null;
            if(args != null)
            {
                LeftPosition = (double)args.Left;
                TopPosition = (double)args.Top;
            }
            await SidebarHide();
        }

        // Returns event argument for the sidebar open/close events.
        private EventArgs SidebarEvent(string eventValue)
        {
            EventArgs eventArgs = new EventArgs
            {
                Cancel = false,
                Element = element,
                Name = eventValue,
                Left = LeftPosition,
                Top = TopPosition,
                IsInteracted = isInteracted
            };
            isInteracted = false;
            return eventArgs;
        }

        // Specifies the sidebar component dispose method.
        internal async override void ComponentDispose()
        {
            if (IsRendered && !isDestroyed)
            {
                try
                {
                    isDestroyed = true;
                    await InvokeMethod("sfBlazor.Sidebar.destroy", element);
                    await SfBaseUtils.InvokeEvent<object>(Destroyed, null);
                }
                catch (Exception e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Destroyed, e);
                    throw new InvalidOperationException(e.Message);
                }
            }
        }

        // Updates sidebar element state based in the type of the sidebar component.
        private async Task CheckType()
        {
            if (Type == SidebarType.Auto)
            {
                await SidebarShow();
            }
            else if (!sidebarClass.Contains(CLOSE, StringComparison.Ordinal))
            {
                await SidebarHide();
            }
        }

        // Returns the properties of the sidebar component to be sent to the client side handling.

        /// <summary>
        ///  Method to Get Properties.
        /// </summary>
        /// <returns>properties.</returns>
        protected Dictionary<string, object> GetProperties()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("DockSize", DockSize);
            properties.Add("Animate", Animate);
            properties.Add("IsOpen", IsOpen);
            properties.Add("EnableDock", EnableDock);
            properties.Add("MediaQuery", MediaQuery);
            properties.Add("Position", Position.ToString());
            properties.Add("Type", Type.ToString());
            properties.Add("CloseOnDocumentClick", CloseOnDocumentClick);
            properties.Add("Width", Width);
            properties.Add("EnableGestures", EnableGestures);
            properties.Add("ShowBackdrop", ShowBackdrop);
            properties.Add("Target", Target);
            return properties;
        }
    }
}
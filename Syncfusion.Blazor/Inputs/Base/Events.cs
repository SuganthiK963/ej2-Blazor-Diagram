using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Inputs.Internal
{
    /// <summary>
    /// The SfInputBase is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfInputBase : SfBaseComponent
    {
        /// <summary>
        /// Triggers when the content of input has changed and gets focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        /// <summary>
        /// Triggers each time when the value of input has changed.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Triggers when the content is paste into an input.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ClipboardEventArgs> OnPaste { get; set; }

        /// <summary>
        /// Triggers when the input has focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Triggers when the clear gets clicked.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<MouseEventArgs> BindClearBtnEvents { get; set; }

        /// <summary>
        /// Triggers when the clear icon gets touch.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<TouchEventArgs> BindClearBtnTouchEvents { get; set; }

        /// <summary>
        /// Triggers when the spin down gets click.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<MouseEventArgs> MouseDownOnSpinner { get; set; }

        /// <summary>
        /// Triggers when the spin down icon gets touch.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<TouchEventArgs> TouchDownOnSpinner { get; set; }

        /// <summary>
        /// Triggers when the spin up gets clicked.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<MouseEventArgs> MouseUpOnSpinner { get; set; }

        /// <summary>
        /// Triggers when the spin up icon gets touch.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<TouchEventArgs> TouchUpOnSpinner { get; set; }

        /// <summary>
        /// Triggers when the icon gets click.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<IconHandlerArgs> MouseIconHandler { get; set; }

        /// <summary>
        /// Triggers when the icon get touch.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<IconHandlerArgs> TouchIconHandler { get; set; }

        /// <summary>
        /// Triggers when the container gets click.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<MouseEventArgs> MouseContainerHandler { get; set; }

        /// <summary>
        /// Triggers when the container gets focus out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnContainerBlur { get; set; }

        /// <summary>
        /// Triggers when the container gets focused.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnContainerFocus { get; set; }

        /// <summary>
        /// Triggers when the container key pressed.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<KeyboardEventArgs> ContainerKeypress { get; set; }
    }
}
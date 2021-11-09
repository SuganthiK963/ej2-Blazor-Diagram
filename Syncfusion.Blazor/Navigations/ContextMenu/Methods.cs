using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// ContextMenu is a graphical user interface that appears on the user right click/touch hold operation.
    /// </summary>
    public partial class SfContextMenu<TValue> : SfMenuBase<TValue>
    {
        /// <summary>
        /// Closes the ContextMenu if it is opened.
        /// </summary>
        public void Close()
        {
            NavIdx = new List<int>();
            ClsCollection = new List<ClassCollection>();
            StateHasChanged();
        }

        /// <summary>
        /// This method is used to open the ContextMenu in specified position. If the positions are not specified, the context menu
        /// will open at its rendered position.
        /// </summary>
        /// <param name = "clientX">Specifies the horizontal position of the context menu.</param>
        /// <param name = "clientY">Specifies the vertical position of the context menu.</param>
        /// <param name = "enableCollision">Specifies the collision detection of the context menu.</param>
        public void Open(double? clientX = null, double? clientY = null, bool enableCollision = false)
        {
            if (Fields == null)
            {
                return;
            }

            Left = clientX;
            Top = clientY;
            if ((Left != null && Top != null) || IsMenu)
            {
                manualOpen = true;
                isCollision = enableCollision;
                OpenEventArgs = new OpenCloseMenuEventArgs<TValue>() { Name = OPENED, Element = Element, Items = Items, ParentItem = default };
            }

            ClsCollection = new List<ClassCollection>();
            NavIdx = new List<int> { 0 };
            StateHasChanged();
        }
    }
}
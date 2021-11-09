using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Navigations.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.FileManager")]

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// ContextMenu is a graphical user interface that appears on the user right click/touch hold operation.
    /// </summary>
    public partial class SfContextMenu<TValue> : SfMenuBase<TValue>
    {
        [CascadingParameter]
        private MenuOptions Parent { get; set; }
        private readonly string id = SfBaseUtils.GenerateID(SFCONTEXTMENU);
        private ElementReference refElement;
        private string containerClass;
        private bool manualOpen;
        private bool isCollision;

        internal void Initialize()
        {
            containerClass = Initialize(IsMenu ? CONTAINER + MENUCONTAINER : CONTAINER);
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OpenContextMenuAsync(double clientX, double clientY)
        {
            var eventArgs = await TriggerBeforeOpenCloseEvent(default, Items, ONOPEN, true, clientX, clientY);
            if (!eventArgs.Cancel)
            {
                Top = eventArgs.Top;
                Left = eventArgs.Left;
                OpenEventArgs = new OpenCloseMenuEventArgs<TValue>() { Name = OPENED, Element = Element, Items = Items, ParentItem = default, NavigationIndex = this.NavIdx.Count - 1 };
                ClsCollection = new List<ClassCollection>();
                NavIdx = new List<int> { 0 };
                StateHasChanged();
            }
        }

        private async Task ItemClickHandler(TValue item, MouseEventArgs e, bool isEnterKey = false, bool isUl = false, bool header = false)
        {
            await ClickHandler(Items, item, e, isEnterKey, isUl, header);
        }

        private async Task MouseOverHandler(TValue item)
        {
            if (!IsDevice)
            {
                await OpenCloseSubMenu(item);
            }
        }

        private async Task KeyDownHandler(TValue item, KeyboardEventArgs e, bool isUl = false)
        {
            await KeyActionHandler(Items, item, e, isUl);
        }
    }
}
using Microsoft.JSInterop;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Blazor.Diagram.SymbolPalette.Internal
{
    internal class SfSymbolPaletteEventHandler
    {
        internal SfSymbolPaletteComponent PaletteComponent;
        internal const string SELECTEDSYMBOLITEM = "_SelectedSymbol";
        internal SfSymbolPaletteEventHandler(SfSymbolPaletteComponent paletteComponent)
        {
            PaletteComponent = paletteComponent;
        }

        /// <summary>
        /// Used to invoke the diagram's touch events while drag and drop the objects from the palettte.
        /// </summary>
        [JSInvokable]
        public void InvokeDiagramEvents(JSMouseEventArgs args)
        {
            if (PaletteComponent.Targets != null)
            {
                for (int i = 0; i < PaletteComponent.Targets.Count; i++)
                {
                    if (PaletteComponent.Targets[i].ID == PaletteComponent.DiagramId)
                    {
                        PaletteComponent.Targets[i].EventHandler.InvokeDiagramEvents(args);
                    }
                }
            }
        }

        [JSInvokable]
        public void InvokePaletteEvents(JSMouseEventArgs args, string Id)
        {
            if (args != null)
            {
                switch (args.Type)
                {
                    case "mousedown":
                    case "touchstart":
                        MouseDown(Id);
                        break;
                    case "mousemove":
                    case "touchmove":
                        MouseMove(Id);
                        break;
                    case "mouseup":
                    case "touchend":
                        MouseUp();
                        break;
                    case "mouseleave":
                        MouseLeave();
                        break;
                }
                args.Dispose();
                args = null;
            }
        }
        [JSInvokable]
        public void SymbolPaletteDragEnter(object value)
        {
            if (value != null)
            {
                PaletteComponent.DiagramId = value.ToString();
                if (PaletteComponent.Targets != null)
                {
                    for (int i = 0; i < PaletteComponent.Targets.Count; i++)
                    {
                        if (PaletteComponent.Targets[i].ID == value.ToString())
                        {
                            PaletteComponent.Targets[i].DragEnterEvent(PaletteComponent.SelectedSymbol);
                        }
                    }
                }
            }
        }

        [JSInvokable]
        public void SymbolPaletteDragLeave()
        {
            if (PaletteComponent.Targets != null)
            {
                for (int i = 0; i < PaletteComponent.Targets.Count; i++)
                {
                    if (PaletteComponent.Targets[i].ID == PaletteComponent.DiagramId)
                    {
                        PaletteComponent.Targets[i].DragLeaveEvent();
                    }
                }
            }
        }
        [JSInvokable]
        public void SymbolPaletteDrop(object value, bool isTouch)
        {
            if (value != null)
            {
                if (PaletteComponent.Targets != null)
                {
                    for (int i = 0; i < PaletteComponent.Targets.Count; i++)
                    {
                        if (PaletteComponent.Targets[i].ID == value.ToString())
                        {
                            PaletteComponent.Targets[i].SymbolDrop(isTouch);
                        }
                    }
                }
            }
            PaletteComponent.PaletteMouseDown = false;
        }
        internal void MouseDown(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                if (PaletteComponent.SymbolTable.ContainsKey(Id))
                {
                    PaletteComponent.PaletteMouseDown = true;
                    NodeBase SelectedNode;
                    if (PaletteComponent.SymbolTable[Id.ToString()] is Connector)
                    {
                        SelectedNode = (Connector)(PaletteComponent.SymbolTable[Id.ToString()] as Connector).Clone();
                        SelectedNode.ID += BaseUtil.RandomId(); ;
                    }
                    else
                    {
                        if (PaletteComponent.SymbolTable[Id.ToString()] is NodeGroup)
                        {
                            SelectedNode = (NodeGroup)(PaletteComponent.SymbolTable[Id.ToString()] as NodeGroup).Clone();
                        }
                        else
                        {
                            SelectedNode = (Node)(PaletteComponent.SymbolTable[Id.ToString()] as Node).Clone();
                        }
                        Shape shape = (SelectedNode as Node).Shape;
                        Shapes type = shape.Type;
                        SelectedNode.ID += BaseUtil.RandomId(); ;
                        if (type == Shapes.SVG || type == Shapes.HTML)
                        {
                            SelectedNode.ID = (PaletteComponent.SymbolTable[Id.ToString()] as Node).ID + "_" + BaseUtil.RandomId();
                        }
                    }
                    PaletteComponent.SelectedSymbol = SelectedNode;
                }
            }
            PaletteComponent.RenderPreviewSymbol();
        }
        internal void MouseMove(string Id)
        {
            if (!string.IsNullOrEmpty(Id) && !PaletteComponent.PaletteMouseDown)
            {
                if (PaletteComponent.SymbolTable.ContainsKey(Id))
                {
                    if (PaletteComponent.SymbolTable[Id.ToString()] is Connector)
                    {
                        Connector SelectedNode = (Connector)(PaletteComponent.SymbolTable[Id.ToString()] as Connector).Clone();
                        SelectedNode.ID += SELECTEDSYMBOLITEM;
                        PaletteComponent.SelectedSymbol = SelectedNode;
                    }
                    else
                    {
                        Node SelectedNode = (Node)(PaletteComponent.SymbolTable[Id.ToString()] as Node).Clone();
                        SelectedNode.ID += SELECTEDSYMBOLITEM;
                        PaletteComponent.SelectedSymbol = SelectedNode;
                    }
                }
            }
        }
        internal void MouseLeave()
        {
            if (!PaletteComponent.PaletteMouseDown)
            {
                PaletteComponent.SelectedSymbol = null;
            }
        }
        internal void MouseUp()
        {
            PaletteComponent.PaletteMouseDown = false;
            if (PaletteComponent.SelectedSymbol != null)
            {
                PaletteSelectionChangedEventArgs args = new PaletteSelectionChangedEventArgs() { NewValue = (PaletteComponent.SelectedSymbol as NodeBase).ID, OldValue = PaletteComponent.OldSymbol };
                PaletteComponent.InvokeSymbolPaletteEvents(SymbolPaletteEvent.SelectionChange, args);
                PaletteComponent.OldSymbol = (PaletteComponent.SelectedSymbol as NodeBase).ID;
            }
            PaletteComponent.PreviewSymbol = null;
        }

        //BLAZ-14370_Fixed_Helper object draws in the diagram even the dragged object from palette and dropped outside of the diagram.
        [JSInvokable]
        public void ElementDropToOutSideDiagram()
        {
            PaletteComponent.PaletteMouseDown = false;
            PaletteComponent.SelectedSymbol = null;
            PaletteComponent.PreviewSymbol = null;
            if (PaletteComponent.Targets != null)
            {
                for (int i = 0; i < PaletteComponent.Targets.Count; i++)
                {
                    if (PaletteComponent.Targets[i].RealAction.HasFlag(RealAction.SymbolDrag))
                    {
                        PaletteComponent.Targets[i].RealAction &= ~RealAction.SymbolDrag;
                    }
                }
            }
        }
    }
}

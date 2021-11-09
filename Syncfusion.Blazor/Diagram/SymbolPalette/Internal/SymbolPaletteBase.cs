using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Diagram.Internal;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Container = Syncfusion.Blazor.Diagram.DiagramContainer;

namespace Syncfusion.Blazor.Diagram.SymbolPalette
{
    public partial class SfSymbolPaletteComponent : SfBaseComponent
    {
        internal void PrepareSymbols(IDiagramObject symbol, bool isMeasure)
        {
            Boolean isNode = symbol is Node;
            double width; double sw; double height; double sh;
            StackPanel panel = new StackPanel();
            IDiagramObject obj = symbol;
            DiagramElement content;
            if (isNode)
            {
                this.InvokeSymbolPaletteEvents(SymbolPaletteEvent.NodeDefaults, symbol as Node);
            }
            else
            {
                this.InvokeSymbolPaletteEvents(SymbolPaletteEvent.ConnectorDefaults, symbol as Connector);
            }

            Canvas symbolContainer = new Canvas();
            DiagramContainer container = new DiagramContainer();

            if (isNode)
            {
                container = ((Node)symbol).InitContainer();
            }
            if (container.Children == null)
            {
                container.Children = new DiagramObjectCollection<ICommonElement>() { };
            }
            if (symbol is NodeGroup)
            {
                content = this.GetContainer((Node)obj, container);
            }
            else
            {
                content = isNode ? (obj as Node).Init() : ((Connector)obj).Init();
                if (isNode && !string.IsNullOrEmpty(((Node)symbol).ParentId))
                {
                    container.Children.Add(content);
                }
            }
            if (isNode && string.IsNullOrEmpty(((Node)symbol).ParentId) || symbol is Connector)
            {
                SymbolInfo symbolInfo = new SymbolInfo();
                if (this.GetSymbolInfo != null && !this.PaletteMouseDown)
                {
                    symbolInfo = this.GetSymbolInfo(symbol);
                }

                if (symbolContainer.Children.Count == 0)
                {
                    symbolContainer.Children.Add(content);
                }
                content.Measure(new DiagramSize());
                content.Arrange(content.DesiredSize, false);

                if (isNode)
                {
                    width = symbolInfo.Width = ((Node)obj).Width != null ? (double)(content.ActualSize.Width) : this.SymbolWidth;
                    height = symbolInfo.Height = ((Node)obj).Height != null ? (double)(content.ActualSize.Height) : this.SymbolHeight;
                }
                else
                {
                    width = this.SymbolWidth;
                    height = this.SymbolHeight;
                }
                symbolContainer.Style.StrokeColor = symbolContainer.Style.Fill = "none";
                if (width != 0 && height != 0)
                {
                    content.RelativeMode = RelativeMode.Object;
                    content.HorizontalAlignment = HorizontalAlignment.Center;
                    content.VerticalAlignment = VerticalAlignment.Center;

                    double actualWidth = width; double actualHeight = height;
                    if (this.SymbolWidth != 0)
                    {
                        actualWidth = this.SymbolWidth - this.SymbolMargin.Left - this.SymbolMargin.Right;
                    }
                    else
                    {
                        width += isNode ? ((Node)obj).Style.StrokeWidth : ((Connector)obj).Style.StrokeWidth;
                    }
                    if (this.SymbolHeight != 0)
                    {
                        actualHeight = this.SymbolHeight - this.SymbolMargin.Top - this.SymbolMargin.Bottom;
                    }
                    else
                    {
                        height += isNode ? ((Node)obj).Style.StrokeWidth : ((Connector)obj).Style.StrokeWidth;
                    }
                    if (symbolInfo.Description != null && symbolInfo.Description.Text != null)
                    {
                        actualHeight -= 20;
                    }
                    actualHeight = actualHeight > 0 ? actualHeight : 1;
                    actualWidth = actualWidth > 0 ? actualWidth : 1;
                    sw = content.Width != null ? (double)(actualWidth / content.Width) : actualWidth / width;
                    sh = content.Height != null ? (double)(actualHeight / content.Height) : actualHeight / height;
                    if (symbolInfo.Fit)
                    {
                        sw = actualWidth / symbolInfo.Width; sh = actualHeight / symbolInfo.Height;
                    }
                    width = actualWidth; height = actualHeight; sw = sh = Math.Min(sw, sh);
                    symbolContainer.Width = width; symbolContainer.Height = height;
                    content.Width = symbolInfo.Width; content.Height = symbolInfo.Height;
                    if (isNode)
                    {
                        this.ScaleSymbol(symbol as Node, symbolContainer, sw, sh, width, height, true, false);
                    }
                    else
                    {
                        if (this.FirstRender || !this.PaletteMouseDown)
                        {
                            this.ScaleSymbol(symbol as Connector, symbolContainer, sw, sh, width, height, true, false);
                        }
                    }
                }
                else
                {
                    DiagramRect outerBounds = null;
                    if (symbol is Connector connector)
                    {
                        outerBounds = ConnectorUtil.GetOuterBounds(connector);
                    }
                    content.Width = (outerBounds?.Width ?? content.ActualSize.Width) - 2;
                    content.Height = (outerBounds?.Height ?? content.ActualSize.Height) - 2;
                }
                _ = isNode ? ((symbol as Node).Wrapper = panel) : ((symbol as Connector).Wrapper = panel);
                content.Height = content.Height > 0 ? content.Height : 1;
                content.Width = content.Width > 0 ? content.Width : 1;
                if (panel.Children.Count == 0)
                {
                    panel.Children.Add(symbolContainer);
                }
                content.Pivot = panel.Pivot = new DiagramPoint() { X = 0, Y = 0 };
                panel.ID = content.ID + SYMBOL;
                panel.Style.Fill = panel.Style.StrokeColor = PANEL_COLOR;
                panel.OffsetY = panel.OffsetX = (symbol is Node ? (symbol as Node).Style.StrokeWidth / 2 : 0.5);
                GetSymbolDescription(symbolInfo, width, panel, symbol, true);
                if (isMeasure)
                {
                    panel.Measure(new DiagramSize());
                    panel.Arrange(panel.DesiredSize);
                }
            }

            if (isNode && !string.IsNullOrEmpty((symbol as Node).ParentId))
            {
                container.Measure(new DiagramSize() { Width = (symbol as Node).Width, Height = (symbol as Node).Height });
                container.Arrange(container.DesiredSize);
            }
        }
        internal void GetSymbolDescription(SymbolInfo symbolInfo, double width, StackPanel parent, IDiagramObject symbol, bool isPanelAvailable)
        {
            bool isNode = symbol is Node;
            if (symbolInfo != null && symbolInfo.Description != null && symbolInfo.Description.Text != null)
            {
                TextElement textElement1 = new TextElement();
                TextElement textElement = textElement1;
                textElement.ID = (isNode ? ((Node)symbol).ID : ((Connector)symbol).ID) + "_content_symbol_text";
                textElement.Content = symbolInfo.Description.Text;
                textElement.Width = width;
                textElement.Height = 20;
                textElement.Style.StrokeColor = "transparent";
                ((TextStyle)textElement.Style).Fill = "transparent";
                ((TextStyle)textElement.Style).StrokeWidth = 0;
                ((TextStyle)textElement.Style).TextWrapping = symbolInfo.Description.TextWrapping;
                ((TextStyle)textElement.Style).TextOverflow = symbolInfo.Description.TextOverflow;
                textElement.Margin = new Margin() { Left = 0, Right = 0, Top = 0, Bottom = 5 };
                if (isPanelAvailable)
                {
                    parent.Children.Add(textElement);
                }
                else
                {
                    DiagramLayerContent.AddMeasureTextCollection(symbol as NodeBase, null, textElement, MeasureTextDataCollection);
                }
            }
        }
        internal DiagramSize GetPreviewSymbolSize(NodeBase symbol)
        {
            double strokeWidth = (symbol is Node node) ? node.Style.StrokeWidth : ((Connector)symbol).Style.StrokeWidth;
            double width = this.SymbolDragPreviewSize == null ? (double)(symbol.Wrapper.ActualSize.Width) : (double)(SymbolDragPreviewSize.Width);
            double height = this.SymbolDragPreviewSize == null ? (double)(symbol.Wrapper.ActualSize.Height) : (double)(SymbolDragPreviewSize.Height);
            return new DiagramSize() { Width = width + strokeWidth, Height = height + strokeWidth };
        }
        internal DiagramSize GetSymbolSize(NodeBase symbol)
        {
            double width = symbol.Wrapper.ActualSize.Width ?? 0;
            double height = symbol.Wrapper.ActualSize.Height ?? 0;
            if (this.SymbolWidth == 0 && this.SymbolHeight == 0)
            {
                double strokeWidth = (symbol is Node node) ? node.Style.StrokeWidth : ((Connector)symbol).Style.StrokeWidth;
                width += this.symbolMargin.Left + this.symbolMargin.Right + strokeWidth;
                height += this.symbolMargin.Top + this.symbolMargin.Bottom + strokeWidth;
            }
            else
            {
                width = this.symbolWidth;
                height = Math.Max(this.symbolHeight, height);
            }
            return new DiagramSize() { Width = width, Height = height };
        }
        private void ArrangeChild(NodeGroup obj, double x, double y, bool drop)
        {
            string[] child = obj.Children;
            for (int i = 0; i < child.Length; i++)
            {
                IDiagramObject node = this.SymbolTable[child[i]];
                if (node != null)
                {
                    if (node is NodeGroup groupObj && groupObj.Children != null)
                    {
                        ArrangeChild(groupObj, x, y, drop);
                    }
                    else
                    {
                        ((Node)node).OffsetX -= x;
                        (node as Node).OffsetY -= y;
                        if (!drop)
                        {
                            this.SymbolTable[(node as Node).ID] = node;
                            DiagramContainer container = (node as Node).InitContainer();
                            if (container.Children != null)
                            {
                                container.Children = new DiagramObjectCollection<ICommonElement>() { };
                            }
                            DiagramElement content = (node as Node).Init();
                            container.Children?.Add(content);
                            container.Measure(new DiagramSize() { Width = obj.Width, Height = obj.Height });
                            container.Arrange(container.DesiredSize);
                        }
                    }
                }
            }
        }
        internal void InitSymbols(DiagramObjectCollection<NodeBase> symbols)
        {
            for (int j = 0; j < symbols.Count; j++)
            {
                if (symbols[j] is Node symbol)
                {
                    if (!this.SymbolTable.ContainsKey(symbol.ID))
                    {
                        this.SymbolTable.Add(symbol.ID, symbol);
                    }
                }
                else
                {
                    if (!this.SymbolTable.ContainsKey(((Connector)symbols[j]).ID))
                    {
                        this.SymbolTable.Add(((Connector)symbols[j]).ID, symbols[j]);
                    }
                }

                if (symbols[j] is NodeGroup groupObj && groupObj.Children != null && groupObj.Children.Length > 0)
                {
                    for (int i = 0; i < groupObj.Children.Length; i++)
                    {
                        if (this.SymbolTable[groupObj.Children[i]] is Node node)
                        {
                            node.ParentId = symbols[j].ID;
                        }
                    }
                }
            }
            for (int j = 0; j < symbols.Count; j++)
            {
                if (symbols[j] is Node)
                {
                    this.PrepareSymbols(symbols[j], true);
                }
                else
                {
                    if (((Connector)symbols[j]).Wrapper is StackPanel panel)
                    {
                        panel.Measure(new DiagramSize());
                        panel.Arrange(panel.DesiredSize);
                    }
                }
            }
        }
        internal void UpdateDictionaryValue(NodeBase symbol)
        {
            SymbolInfo symbolInfo = new SymbolInfo();
            double actualWidth = 0;
            if (this.GetSymbolInfo != null)
            {
                symbolInfo = this.GetSymbolInfo((symbol));
                actualWidth = this.SymbolWidth - this.SymbolMargin.Left - this.SymbolMargin.Right;
            }
            if (symbol is Node node)
            {
                Shape shape = node.Shape;
                Shapes type = shape.Type;
                this.GetSymbolDescription(symbolInfo, actualWidth, null, node, false);
                if (type == Shapes.Path && !string.IsNullOrEmpty(((PathShape)shape).Data))
                {
                    if (!this.MeasurePathDataCollection.ContainsKey((shape as PathShape).Data))
                    {
                        DiagramLayerContent.GetMeasureNodeData(node, this.MeasurePathDataCollection, this.MeasureTextDataCollection, this.MeasureImageDataCollection, this.MeasureNativeDataCollection);
                    }
                }
                else if (type == Shapes.SVG)
                {
                    DiagramLayerContent.GetMeasureNodeData(node, this.MeasurePathDataCollection, this.MeasureTextDataCollection, this.MeasureImageDataCollection, this.MeasureNativeDataCollection);
                }
            }
            if (symbol is Connector connector)
            {
                this.PrepareSymbols(symbol, false);
                this.GetSymbolDescription(symbolInfo, actualWidth, null, connector, false);
                for (int i = 0; i < ((DiagramContainer)connector.Wrapper.Children[0]).Children.Count; i++)
                {
                    if (((DiagramContainer)((DiagramContainer)connector.Wrapper.Children[0]).Children[0]).Children[i] is PathElement child && Dictionary.GetMeasurePathBounds(child.Data) == null && !this.MeasurePathDataCollection.ContainsKey(child.Data))
                    {
                        DiagramLayerContent.AddMeasurePathDataCollection(child.Data, this.MeasurePathDataCollection);
                    }
                }
            }
        }
        internal void UpdateMeasureElements()
        {
            for (int i = 0; i < this.Palettes.Count; i++)
            {
                DiagramObjectCollection<NodeBase> symbols = this.Palettes[i].Symbols;
                this.UpdateElements(symbols);
            }
        }
        internal void UpdateElements(DiagramObjectCollection<NodeBase> symbols)
        {
            for (int j = 0; j < symbols.Count; j++)
            {
                this.UpdateDictionaryValue(symbols[j]);
            }
        }

        internal async Task PaletteMeasureBounds(NodeBase paletteSymbol, Palette symbolPaletteGroup = null)
        {
            await DomUtil.MeasureBounds(this.MeasurePathDataCollection, this.MeasureTextDataCollection, this.MeasureImageDataCollection, null);
            if (paletteSymbol != null)
            {
                if (paletteSymbol is Connector)
                {
                    StackPanel panel = paletteSymbol.Wrapper as StackPanel;
                    panel.Measure(new DiagramSize());
                    panel.Arrange(panel.DesiredSize);
                    this.PaletteRealAction |= RealAction.PreventEventRefresh;
                    symbolPaletteGroup.Symbols.Add(paletteSymbol);
                    this.PaletteRealAction &= ~RealAction.PreventEventRefresh;
                    this.SymbolPaletteStateHasChanged();
                }
                else
                {
                    this.PrepareSymbols(paletteSymbol, true);
                }
            }
        }
        internal async Task AddPaletteSymbol(DiagramObjectCollection<NodeBase> symbols)
        {
            await this.PaletteMeasureBounds(null);
            this.InitSymbols(symbols);
            this.SymbolPaletteStateHasChanged();
        }
        internal async Task AddPaletteSymbols(DiagramObjectCollection<Palette> Palettes)
        {
            await this.PaletteMeasureBounds(null);
            for (int i = 0; i < Palettes.Count; i++)
            {
                this.InitSymbols(Palettes[i].Symbols);
            }
            this.CanCallStateChange = true;
            this.SymbolPaletteStateHasChanged();
        }
        internal void UpdatePalettes()
        {
            for (int i = 0; i < this.Palettes.Count; i++)
            {
                DiagramObjectCollection<NodeBase> symbols = this.Palettes[i].Symbols;
                this.InitSymbols(symbols);
            }
        }
        internal DiagramContainer GetContainer(IDiagramObject obj, DiagramContainer container)
        {
            container.MeasureChildren = false;
            string[] child = ((NodeGroup)obj).Children;
            container.Children = new ObservableCollection<ICommonElement>();
            for (int i = 0; i < child.Length; i++)
            {
                if (this.SymbolTable[child[i]] != null)
                {
                    Node childNode = (Node)((Node)this.SymbolTable[child[i]]).Clone();
                    this.PrepareSymbols(childNode, true);
                    childNode.Wrapper.ID = ((Node)this.SymbolTable[child[i]]).ID;
                    childNode.Wrapper.MeasureChildren = false;
                    container.Children.Add(childNode.Wrapper);
                }
            }
            container.Measure(new DiagramSize() { Width = ((Node)obj).Width, Height = ((Node)obj).Height });
            container.Arrange(container.DesiredSize);
            if (container.Bounds.X != 0 || container.Bounds.Y != 0)
            {
                DiagramRect bounds = container.Bounds;
                this.ArrangeChild(obj as NodeGroup, bounds.X, bounds.Y, false);
                container = this.GetContainer(obj, container);
            }
            return container;
        }

        internal void GetSymbolPreview(NodeBase newNode)
        {
            if (this.SymbolDragPreviewSize != null && (this.SymbolDragPreviewSize.Width != null || this.SymbolDragPreviewSize.Height != null))
            {
                bool IsNode = newNode is Node;
                double symbolStrokeWidth = IsNode ? (newNode is NodeGroup ? (newNode as Node).Style.StrokeWidth + 1 : (newNode as Node).Style.StrokeWidth) : (newNode as Connector).Style.StrokeWidth;
                double symbolPreviewWidth = (double)(this.SymbolDragPreviewSize.Width) - symbolStrokeWidth;
                double symbolPreviewHeight = (double)(this.SymbolDragPreviewSize.Height) - symbolStrokeWidth;
                DiagramElement content = IsNode ? (((newNode as Node).Wrapper.Children[0] as Canvas).Children[0] as DiagramElement) : (((newNode as Connector).Wrapper.Children[0] as Canvas).Children[0] as DiagramElement);
                if (IsNode)
                {
                    double groupNodeWidth = 0;
                    if (newNode is NodeGroup)
                    {
                        string[] childnodes = (newNode as NodeGroup).Children;
                        Node Childnode;
                        for (int i = 0; i < childnodes.Length; i++)
                        {
                            Childnode = this.SymbolTable[childnodes[i]] as Node;
                            if (groupNodeWidth < Childnode.Style.StrokeWidth)
                            {
                                groupNodeWidth = Childnode.Style.StrokeWidth;
                            }
                        }
                    }
                    (newNode as Node).Width = symbolPreviewWidth;
                    (newNode as Node).Height = symbolPreviewHeight;
                }
                double sw = (double)(symbolPreviewWidth / content.ActualSize.Width);
                double sh = (double)(symbolPreviewHeight / content.ActualSize.Height);
                sw = sh = Math.Min(sw, sh);
                double symbolWidth = (double)(content.ActualSize.Width * sw);
                double symbolHeight = (double)(content.ActualSize.Height * sh);
                if (IsNode)
                {
                    (newNode as Node).Wrapper.Children[0].Width = symbolPreviewWidth;
                    (newNode as Node).Wrapper.Children[0].Height = symbolPreviewHeight;
                    Canvas symbolContainer = ((newNode as Node).Wrapper.Children[0] as Canvas);
                    this.ScaleSymbol(newNode as Node, symbolContainer, sw, sh, symbolWidth, symbolHeight, true, true);
                    content.OffsetX = content.OffsetY = (newNode as Node).Style.StrokeWidth / 2;
                }
                else
                {
                    (newNode as Connector).Wrapper = null;
                    (newNode as Connector).SourcePoint.X = (newNode as Connector).TargetDecorator.Width;
                    (newNode as Connector).SourcePoint.Y = (newNode as Connector).TargetDecorator.Height;
                    (newNode as Connector).TargetPoint.X = symbolPreviewWidth - (newNode as Connector).TargetDecorator.Width;
                    (newNode as Connector).TargetPoint.Y = symbolPreviewHeight - (newNode as Connector).TargetDecorator.Height;
                    this.PrepareSymbols(newNode, false);
                    Canvas connectorContainer = ((newNode as Connector).Wrapper.Children[0] as Canvas);
                    for (int i = 0; i < (connectorContainer.Children[0] as DiagramContainer).Children.Count; i++)
                    {
                        (connectorContainer.Children[0] as DiagramContainer).Children[i].StaticSize = true;
                    }
                }
                content.Pivot = new DiagramPoint() { X = 0, Y = 0 };
                MeasureAndArrangeSymbol(content);
            }
        }

        private static void MeasureAndArrangeSymbol(DiagramElement content)
        {
            if (content != null)
            {
                content.Measure(new DiagramSize());
                content.Arrange(content.DesiredSize, false);
            }
        }
        private static void ScaleConnectorSymbol(IDiagramObject symbol, DiagramContainer symbolContainer, double sw, double sh,
        double width, double height, bool check)
        {
            DiagramContainer wrapper = (symbol as Connector).Wrapper;
            (symbol as Connector).Wrapper = symbolContainer.Children[0] as DiagramContainer;
            DiagramPoint point = (symbol as Connector).Scale(sw, sh, width, height);
            double difX = width / 2 - (symbolContainer.Children[0] as DiagramContainer).Children[0].OffsetX + point.X / 2;
            double difY = height / 2 - (symbolContainer.Children[0] as DiagramContainer).Children[0].OffsetY + point.Y / 2;
            for (int i = 0; i < (symbolContainer.Children[0] as DiagramContainer).Children.Count; i++)
            {
                (symbolContainer.Children[0] as DiagramContainer).Children[i].OffsetX += difX;
                (symbolContainer.Children[0] as DiagramContainer).Children[i].OffsetY += difY;
                if (check)
                {
                    (symbolContainer.Children[0] as DiagramContainer).Children[i].StaticSize = false;
                }
            }
            ((Connector)symbol).Wrapper = wrapper;
        }
        private void MeasureChild(DiagramElement container)
        {
            for (int i = 0; i < ((DiagramContainer)container).Children.Count; i++)
            {
                DiagramElement childContainer = (DiagramElement)((DiagramContainer)container).Children[i];
                if (((DiagramContainer)((DiagramContainer)childContainer).Children[0]).Children != null)
                {
                    Node node = (Node)(SymbolTable[childContainer.ID]);
                    childContainer.Width = node.Width;
                    childContainer.Height = node.Height;
                    childContainer.Measure(new DiagramSize());
                    childContainer.Arrange((childContainer as DiagramContainer).Children[0].DesiredSize, null);
                }
                else
                {
                    this.MeasureChild(childContainer);
                }
            }
        }
        private static void ScaleGroup(DiagramElement child, double w, double h, IDiagramObject symbol, bool preview)
        {
            child.Width *= w;
            child.Height *= h;
            child.OffsetX = preview ? (child.OffsetX * w) - ((Node)symbol).Style.StrokeWidth : (child.OffsetX * w) + (symbol as Node).Style.StrokeWidth / 2;
            child.OffsetY = preview ? (child.OffsetY * h) - ((Node)symbol).Style.StrokeWidth : (child.OffsetY * h) + (symbol as Node).Style.StrokeWidth / 2;
            child.Measure(new DiagramSize());
            child.Arrange(((DiagramContainer)child).Children[0].DesiredSize, null);
        }
        private void ScaleChildren(DiagramElement container, double w, double h, IDiagramObject symbol, bool preview)
        {
            for (int i = 0; i < ((DiagramContainer)container).Children.Count; i++)
            {
                DiagramElement child = (DiagramElement)((DiagramContainer)container).Children[i];
                if ((((DiagramContainer)child).Children[0] as DiagramContainer).Children != null)
                {
                    ScaleGroup(child, w, h, symbol, preview);
                }
                else
                {
                    this.ScaleChildren(child, w, h, symbol, preview);
                }
            }
        }
        private void ScaleGroupNode(IDiagramObject symbol, DiagramContainer symbolContainer, double width, double height, bool preview)
        {
            string[] parentNode = ((NodeGroup)symbol).Children;
            if (!preview)
            {
                for (int i = 0; i < parentNode.Length; i++)
                {
                    DiagramElement container = (DiagramElement)((DiagramContainer)symbolContainer.Children[0]).Children[i];
                    if (container != null)
                    {
                        if (!((((DiagramContainer)container).Children[0]) is DiagramElement) && ((DiagramContainer)(container as DiagramContainer).Children[0]).Children.Count > 0)
                        {
                            this.MeasureChild(container);
                        }
                        Node node = (Node)SymbolTable[container.ID];
                        container.Width = node.Width;
                        container.Height = node.Height;
                        container.Measure(new DiagramSize());
                        container.Arrange((container as DiagramContainer).Children[0].DesiredSize, null);
                    }
                }
                double w = (double)(width / symbolContainer.Children[0].DesiredSize.Width);
                double h = (double)(height / symbolContainer.Children[0].DesiredSize.Height);
                symbolContainer.Children[0].Measure(new DiagramSize());
                symbolContainer.Children[0].Arrange(symbolContainer.Children[0].DesiredSize, null);
                DiagramElement children;
                for (int i = 0; i < parentNode.Length; i++)
                {
                    children = (DiagramElement)((DiagramContainer)symbolContainer.Children[0]).Children[i];
                    if (children != null)
                    {
                        if (!(((DiagramContainer)children).Children[0] is DiagramElement) && ((DiagramContainer)(children as DiagramContainer).Children[0]).Children != null)
                        {
                            this.ScaleChildren(children, w, h, symbol, preview);
                        }
                        ScaleGroup(children, w, h, symbol, preview);
                    }
                }
            }

            if (preview)
            {
                DiagramElement children;
                for (int i = 0; i < parentNode.Length; i++)
                {
                    double scaleWidth = (double)(width / ((Node)symbol).Wrapper.Children[0].DesiredSize.Width);
                    double scaleHeight = (double)(height / ((Node)symbol).Wrapper.Children[0].DesiredSize.Height);
                    children = (DiagramElement)((DiagramContainer)symbolContainer.Children[0]).Children[i];
                    if (children != null)
                    {
                        if (!(((DiagramContainer)children).Children[0] is DiagramElement) && ((DiagramContainer)(children as DiagramContainer).Children[0]).Children != null)
                        {
                            this.ScaleChildren(children, scaleWidth, scaleHeight, symbol, true);
                        }
                        ScaleGroup(children, scaleWidth, scaleHeight, symbol, true);
                    }
                }
                ((Node)symbol).Wrapper.Children[0].Measure(new DiagramSize());
                ((Node)symbol).Wrapper.Children[0].Arrange(((Node)symbol).Wrapper.Children[0].DesiredSize, null);
            }
        }
        private static void ScaleElement(DiagramElement element, double sw, double sh)
        {
            if (element != null && element.Width != null && element.Height != null)
            {
                element.Width *= sw;
                element.Height *= sh;
            }
        }
        internal void ScaleSymbol(IDiagramObject symbol, DiagramContainer symbolContainer, double sw, double sh,
        double width, double height, bool check, bool preview)
        {
            if (symbol is Connector)
            {
                ScaleConnectorSymbol(symbol, symbolContainer, sw, sh, width, height, check);
            }
            else if (((Node)symbol).Shape.Type == Shapes.Bpmn)
            {
                ((Node)symbol).Wrapper = symbolContainer;
                symbolContainer.Children[0].Width = width;
                symbolContainer.Children[0].Height = height;
                ((Node)symbol).Height = height;
                ((Node)symbol).Width = width;
                double symbolActualWidth = ((Node)symbol).Width ?? 0;
                double symbolActualHeight = ((Node)symbol).Height ?? 0;
                BpmnDiagrams bpmnDiagrams = new BpmnDiagrams();
                bpmnDiagrams.UpdateBpmnSize((Node)symbol);
                if (symbol != null)
                {
                    ((Node)symbol).Height = symbolActualHeight;
                    ((Node)symbol).Width = symbolActualWidth;
                }
            }

            else
            {
                if (symbol is NodeGroup && (symbol as NodeGroup).Children.Length > 0)
                {
                    this.ScaleGroupNode(symbol, symbolContainer, width, height, preview);
                }
                else
                {
                    ScaleElement(symbolContainer.Children[0] as DiagramElement, sw, sh);
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Diagram;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Diagram.SymbolPalette.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram.SymbolPalette
{
    /// <summary>
    /// Represents how to display a collection of palettes. The palette shows a set of nodes and connectors. It allows you to drag and drop the nodes and connectors into the diagram. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfSymbolPaletteComponent Height="600px" Palettes="@Palettes" SymbolDragPreviewSize="@SymbolPreview" SymbolHeight="@symbolSizeHeight" GetSymbolInfo="GetSymbolInfo" SymbolWidth="@symbolSizeWidth" SymbolMargin="@SymbolMargin">
    /// </SfSymbolPaletteComponent >
    /// @code
    /// { 
    ///     DiagramSize SymbolPreview;
    ///     SymbolMargin SymbolMargin = new SymbolMargin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
    ///     double symbolSizeWidth;
    ///     double symbolSizeHeight;
    ///     DiagramObjectCollection<Palette> Palettes = new DiagramObjectCollection<Palette>();
    ///     DiagramObjectCollection<NodeBase> Tnodes = new DiagramObjectCollection<NodeBase>();
    ///     private SymbolInfo GetSymbolInfo(IDiagramObject symbol)
    ///     {
    ///         SymbolInfo SymbolInfo = new SymbolInfo();
    ///         SymbolInfo.Fit = true;
    ///         return SymbolInfo;
    ///     }
    ///     protected override void OnInitialized()
    ///     {
    ///         SymbolPreview = new DiagramSize();
    ///         SymbolPreview.Width = 80;
    ///         SymbolPreview.Height = 80;
    ///         symbolSizeWidth = 50;
    ///         symbolSizeHeight = 50;
    ///         Tnodes = new DiagramObjectCollection<NodeBase>();
    ///         Node Tnode2 = new Node()
    ///         { 
    ///             ID = "node1", 
    ///             Shape = new FlowShape() { Type = Shapes.Flow, Shape = FlowShapeType.Decision } 
    ///         };
    ///         Tnodes.Add(Tnode2);
    ///         Palettes = new DiagramObjectCollection<Palette>()
    ///         {
    ///             new Palette(){Symbols =Tnodes,Title="Flow Shapes",ID="Flow Shapes" },
    ///         };
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public partial class SfSymbolPaletteComponent : SfBaseComponent, IDiagramObject
    {
        private const string UPDATE_ALLOW_DRAG_VALUE = "sfBlazor.Diagram.updateAllowDragValue";
        
		private DiagramObjectCollection<Palette> palettes = new DiagramObjectCollection<Palette>() { };
        private double symbolWidth;
        private bool allowDrag = true;
        private double symbolHeight;
        private bool isSymbolInitialized;

        internal const string PANEL_COLOR = "transparent";
        internal const string SELECTED_SYMBOL_ITEM = "_SelectedSymbol";
        internal BpmnDiagrams BpmnDiagrams = new BpmnDiagrams();
        internal Dictionary<string, string> MeasurePathDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal Dictionary<string, string> MeasureCustomPathDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal Dictionary<string, TextElementUtils> MeasureTextDataCollection { get; set; } = new Dictionary<string, TextElementUtils>() { };
        internal Dictionary<string, string> MeasureImageDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal Dictionary<string, string> MeasureNativeDataCollection { get; set; } = new Dictionary<string, string>() { };
        internal IDiagramObject SelectedSymbol;
        internal string OldSymbol;
        internal bool PaletteMouseDown { get; set; }
        internal Dictionary<string, IDiagramObject> SymbolTable = new Dictionary<string, IDiagramObject>();
        internal ElementReference SymbolPaletteContentRef { get; set; }
        internal SfSymbolPaletteEventHandler EventHandler { get; set; }
        internal DotNetObjectReference<SfSymbolPaletteEventHandler> selfReference;
        internal bool FirstRender;
        internal string ID = BaseUtil.RandomId();
        internal RealAction PaletteRealAction;
        internal const string SYMBOL = "_symbol";
        internal bool CanCallStateChange = true;

        internal IDiagramObject PreviewSymbol { get; set; }
        internal string DiagramId { get; set; }
        /// <summary>
        /// Represents the collection of diagram instances which are to be added to perform the drag and drop funcnalities with multiple diagrams. 
        /// </summary>
        public DiagramObjectCollection<SfDiagramComponent> Targets { get; set; }
        /// <summary>
        /// Gets or sets the width of symbol palette.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// Gets or sets the Height of the symbol palette.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "100%";

        /// <summary>
        /// Represents whether the symbols can be dragged from the palette or not. 
        /// </summary>
        [Parameter]
        public bool AllowDrag
        {
            get => allowDrag;
            set
            {
                if (allowDrag != value)
                {
                    allowDrag = value;
                    _ = this.UpdateAllowDrag();
                }
            }
        }

        /// <summary>
        /// Sets Child content for the symbol Palette component
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the width of the symbol which will be positioned inside the palette. 
        /// </summary>
        [Parameter]
        public double SymbolWidth
        {
            get => symbolWidth;
            set
            {
                bool canUpdate = symbolWidth != 0 && !SymbolWidth.Equals(value);
                symbolWidth = value;
                if (canUpdate)
                {
                    this.UpdatePalettes();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Height of the symbol which will be positioned inside the palette.
        /// </summary>
        [Parameter]
        public double SymbolHeight
        {
            get => symbolHeight;
            set
            {
                bool canUpdate = symbolHeight != 0 && !symbolHeight.Equals(value);
                symbolHeight = value;
                if (canUpdate)
                {
                    this.UpdatePalettes();
                }
            }
        }
        /// <summary>
        /// Represents the size of the symbol preview while dragging a symbol from the palette.
        /// </summary>
        [Parameter]
        public DiagramSize SymbolDragPreviewSize { get; set; }
        /// <summary>
        /// Represents the customization of the drag size of the individual palette items.
        /// </summary>
        [Parameter]
        public DiagramSize SymbolDiagramPreviewSize { get; set; }

        /// <summary>
        /// Sets a segment of the UI content, implemented as a delegate that writes the content of a Node . 
        /// </summary>
        [Parameter]
        public SymbolPaletteTemplates SymbolPaletteTemplates { get; set; }

        /// <summary>
        ///  Represents how to display a collection of similar symbols and annotates the group textually with its heading and the unique id of a symbol group. 
        /// </summary>
        [Parameter]
        public DiagramObjectCollection<Palette> Palettes
        {
            get
            {
                for (int i = 0; i < palettes.Count; i++)
                {
                    palettes[i].Parent ??= this;
                }
                palettes.Parent ??= this;
                return palettes;
            }
            set
            {
                if (value != null && palettes != value)
                {
                    if (palettes.Count != 0)
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            this.UpdateElements(value[i].Symbols);
                        }
                        _ = this.AddPaletteSymbols(value);
                    }
                    palettes = value;
                    palettes.Parent = this;
                }
            }
        }

        private SymbolMargin symbolMargin = new SymbolMargin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
        /// <summary>
        /// Sets an  extra space around the outer boundaries of an element.
        /// </summary>
        [Parameter]
        public SymbolMargin SymbolMargin
        {
            get { return symbolMargin; }
            set
            {
                if (SymbolMargin.Parent == null)
                {
                    symbolMargin.Parent = this;
                }
            }
        }
        private Syncfusion.Blazor.Navigations.ExpandMode expandMode = Syncfusion.Blazor.Navigations.ExpandMode.Multiple;
        /// <summary>
        /// Specifies the option to expand single or multiple palette at a time.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Blazor.Navigations.ExpandMode"/> enumeration. The default value is <see cref="Syncfusion.Blazor.Navigations.ExpandMode.Multiple"/>
        /// </value>
        /// <remarks>
        /// If the <c>ExpandMode</c> is <c>Multiple</c> when clicking on the collapsed icon, the clicked palette will get expanded and at the same time, other palettes are maintained in their previous state.
        /// If the <c>ExpandMode</c> is <c>Single</c> when clicking on the collapsed icon, the clicked palette will get expanded and the rest of all the palettes gets collapsed.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfSymbolPaletteComponent @ref="@palette"
        ///                            Width="80%"
        ///                            Height="445px"
        ///                            PaletteExpandMode="@expandMode"
        ///                            Palettes="@palettes">
        /// </SfSymbolPaletteComponent>
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public Syncfusion.Blazor.Navigations.ExpandMode PaletteExpandMode
        {
            get { return expandMode; }
            set
            {
                if (expandMode != value)
                {
                    expandMode = value;
                }
            }
        }

        /// <summary>
        /// Represents the default properties of the nodes to be returned.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<IDiagramObject> NodeCreating { get; set; }
        /// <summary>
        /// Represents the default properties of the connectors to be returned. 
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<IDiagramObject> ConnectorCreating { get; set; }

        /// <summary>
        /// Represents the size, appearance and description of a symbol.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public Func<IDiagramObject, SymbolInfo> GetSymbolInfo { get; set; }

        /// <summary>
        /// Triggers after the object selection changes in the symbol palette.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<PaletteSelectionChangedEventArgs> SelectionChanged { get; set; }
        /// <summary>
        /// The event will be Triggers before the item gets collapsed/expanded.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<PaletteExpandingEventArgs> Expanding { get; set; }

        internal async Task UpdateAllowDrag()
        {
            selfReference = DotNetObjectReference.Create(EventHandler);
            await JSRuntime.InvokeAsync<object>(UPDATE_ALLOW_DRAG_VALUE, this.SymbolPaletteContentRef, AllowDrag).ConfigureAwait(true);
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            
            this.FirstRender = firstRender;
            if (!firstRender)
            {
                if (isSymbolInitialized && this.Palettes.Count > 0 && MeasureNativeDataCollection.Count > 0)
                {
                    this.FirstRender = false;
                    for (int i = 0; i < this.Palettes.Count; i++)
                    {
                        for (int j = 0; j < this.Palettes[i].Symbols.Count; j++)
                        {
                            if (this.Palettes[i].Symbols[j] is Node symbol && symbol.Shape.Type == Shapes.SVG && symbol.NativeSize == null)
                            {
                                await DomUtil.MeasureBounds(MeasurePathDataCollection, MeasureTextDataCollection, MeasureImageDataCollection, MeasureNativeDataCollection);
                                symbol.NativeSize = DomUtil.MeasureNativeElement(symbol.ID);
                            }
                        }
                    }
                    MeasureNativeDataCollection.Clear();
                    this.SymbolPaletteStateHasChanged();
                }
            }
            if (this.Targets != null)
            {
                for (int i = 0; i < this.Targets.Count; i++)
                {
                    if (this.Targets[i] != null)
                    {
                        this.Targets[i].PaletteInstance = this;
                    }
                }
            }
        }
        internal override async Task OnAfterScriptRendered()
        {
            selfReference = DotNetObjectReference.Create(EventHandler);
            this.UpdateMeasureElements();
            this.FirstRender = true;
            isSymbolInitialized = true;
            DomUtil.CreateMeasureElements(JSRuntime, false, null, null, null, null, null);
            await DomUtil.MeasureBounds(MeasurePathDataCollection, MeasureTextDataCollection, MeasureImageDataCollection, null);
            this.UpdatePalettes();
            this.SymbolPaletteStateHasChanged();
            MeasurePathDataCollection.Clear();
            MeasureTextDataCollection.Clear();
            MeasureImageDataCollection.Clear();
        }
        /// <summary>
        /// This method returns a boolean to indicate if a component’s UI can be rendered. 
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ShouldRender()
        {
            if (this.PaletteRealAction.HasFlag(RealAction.PreventEventRefresh))
            {
                return false;
            }
            return true;
        }
        internal void InvokeSymbolPaletteEvents(SymbolPaletteEvent eventName, object args)
        {
            this.PaletteRealAction |= RealAction.PreventEventRefresh;

            switch (eventName)
            {
                case SymbolPaletteEvent.SelectionChange:
                    this.SelectionChanged.InvokeAsync(args as PaletteSelectionChangedEventArgs);
                    break;
                case SymbolPaletteEvent.NodeDefaults:
                    this.NodeCreating.InvokeAsync(args as IDiagramObject);
                    break;
                case SymbolPaletteEvent.ConnectorDefaults:
                    this.ConnectorCreating.InvokeAsync(args as IDiagramObject);
                    break;
                case SymbolPaletteEvent.OnExpanding:
                    this.Expanding.InvokeAsync(args as PaletteExpandingEventArgs);
                    break;
            }
            this.PaletteRealAction &= ~RealAction.PreventEventRefresh;
        }
        /// <summary>
        /// Used to add the palette item as nodes or connectors in palettes. 
        /// </summary>
        /// <param name="paletteName">string</param>
        /// <param name="paletteSymbol">NodeBase</param>
        /// <param name="isChild">bool</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Node Tnode2 = new Node()
        /// { 
        ///     Shape = new FlowShape() { Type = Shapes.Flow, Shape = FlowShapeType.Decision }
        /// };
        /// PaletteInstance.AddPaletteItem("Flow Shapes", Tnode2, false);
        /// ]]>
        /// </code>
        /// </example>
        public void AddPaletteItem(string paletteName, NodeBase paletteSymbol, bool isChild)
        {
            for (int i = 0; i < this.Palettes.Count; i++)
            {
                Palette symbolPaletteGroup = this.Palettes[i];
                if (symbolPaletteGroup.ID == paletteName)
                {
                    this.UpdateDictionaryValue(paletteSymbol);
                    _ = this.PaletteMeasureBounds(paletteSymbol, symbolPaletteGroup);
                    if (isChild && paletteSymbol is Node node && !this.SymbolTable.ContainsKey(node.ID))
                    {
                        this.SymbolTable.Add(node.ID, paletteSymbol);
                    }
                    if (!isChild && paletteSymbol is Node)
                    {
                        symbolPaletteGroup.Symbols.Add(paletteSymbol);
                    }
                }
            }

        }
        internal void AddPalette(Palette palette, bool canInitSymbols)
        {
            bool canAddPalette = true;
            for (int i = 0; i < this.Palettes.Count; i++)
            {
                if (this.Palettes[i].ID == palette.ID)
                {
                    canAddPalette = false;
                    break;
                }
            }
            if (canAddPalette)
            {
                this.Palettes.Add(palette);
            }
            if (canInitSymbols)
            {
                this.InitSymbols(palette.Symbols);
            }
        }
        /// <summary>
        /// Used to add particular palettes to the symbol palette at runtime.
        /// </summary>
        /// <param name="palettes">DiagramObjectCollection</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// DiagramObjectCollection<Palette> NewPalettes = new DiagramObjectCollection<Palette>()
        /// {
        ///     new Palette() { Symbols = T3nodes,Title = "New palette1",ID = "newPalette1" },
        ///     new Palette() { Symbols = T4nodes,Title = "New palette2",ID = "newPalette2" },
        /// };
        /// PaletteInstance.AddPalettes(NewPalettes);
        /// ]]>
        /// </code>
        /// </example>
        public void AddPalettes(DiagramObjectCollection<Palette> palettes)
        {
            this.CanCallStateChange = false;
            if (palettes != null)
            {
                for (int i = 0; i < palettes.Count; i++)
                {
                    this.AddPalette(palettes[i], false);
                }
                _ = this.AddPaletteSymbols(palettes);
            }
        }
        /// <summary>
        /// Used to remove particular palettes from the symbol palette at runtime.
        /// </summary>
        /// <param name="id">string</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// PaletteInstance.RemovePalettes("Flow Shapes");
        /// ]]>
        /// </code>
        /// </example>
        public void RemovePalettes(string id)
        {
            for (int i = 0; i < this.Palettes.Count; i++)
            {
                if (id == this.Palettes[i].ID)
                {
                    this.Palettes.Remove(this.Palettes[i]);
                }
            }
        }

        internal static string GetSizeValue(string real)
        {
            string value;
            if (real.ToString(CultureInfo.InvariantCulture).IndexOf("px", StringComparison.InvariantCulture) > 0)
            {
                value = real.ToString();
            }
            else if (real.ToString(CultureInfo.InvariantCulture).IndexOf('%', StringComparison.InvariantCulture) > 0)
            {
                value = real;
            }
            else
            {
                value = real.ToString(CultureInfo.InvariantCulture) + "px";
            }
            return value;
        }
        /// <summary>
        /// Used to remove the palette item such as nodes or connectors from the palettes.
        /// </summary>
        /// <param name="paletteName">string</param>
        /// <param name="symbolId">string</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// PaletteInstance.RemovePaletteItem("Flow Shapes", "node1");
        /// ]]>
        /// </code>
        /// </example>
        public void RemovePaletteItem(string paletteName, string symbolId)
        {
            for (int i = 0; i < this.Palettes.Count; i++)
            {
                Palette symbolPaletteGroup = this.Palettes[i];
                if (symbolPaletteGroup.ID == paletteName)
                {
                    for (int j = 0; j < symbolPaletteGroup.Symbols.Count; j++)
                    {
                        NodeBase symbol = symbolPaletteGroup.Symbols[j];
                        string nodeSymbolId = symbol.ID;
                        if (symbolId == nodeSymbolId)
                        {
                            symbolPaletteGroup.Symbols.Remove(symbol);
                            this.SymbolTable.Remove(symbolId);
                            break;
                        }
                    }
                }
            }
        }

        internal void SymbolPaletteStateHasChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        internal void RenderPreviewSymbol()
        {
            if (this.SelectedSymbol != null)
            {
                this.PrepareSymbols(this.SelectedSymbol, true);
                this.GetSymbolPreview(this.SelectedSymbol as NodeBase);
                this.PreviewSymbol = this.SelectedSymbol;
                this.SymbolPaletteStateHasChanged();
            }
        }

        private void UpdateSymbolDragSize()
        {
            if (this.SymbolDiagramPreviewSize == null)
            {
                this.SymbolDiagramPreviewSize = new DiagramSize();
            }
        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            this.ScriptModules = SfScriptModules.SfDiagramComponent;
            await base.OnInitializedAsync().ConfigureAwait(true);
            if (this.Targets == null || this.Targets.Count == 0)
            {
                DictionaryBase.InitializeDefaultValues();
                DiagramLayerContent.MeasurePathDataCollection = new Dictionary<string, string>() { };
                DiagramLayerContent.MeasureTextDataCollection = new Dictionary<string, TextElementUtils>() { };
                DiagramLayerContent.MeasureImageDataCollection = new Dictionary<string, string>() { };
                DiagramLayerContent.MeasureNativeDataCollection = new Dictionary<string, string>() { };
                DiagramLayerContent.MeasureCustomPathDataCollection = new Dictionary<string, string>() { };
            }
            //UpdateSymbolPaletteBase();
            UpdateSymbolDragSize();
            this.EventHandler ??= new SfSymbolPaletteEventHandler(this);
        }


        internal ObservableCollection<Node> GetPaletteSymbols(IDiagramObject node)
        {
            DiagramObjectCollection<Node> symbols = new DiagramObjectCollection<Node>();
            if (node is NodeGroup groupObj)
            {
                string[] childNodes = groupObj.Children;
                for (int i = 0; i < childNodes.Length; i++)
                {
                    symbols.Add(this.SymbolTable[childNodes[i]] as Node);
                }
            }
            return symbols;
        }
        internal void OnExpandingEvent(ExpandEventArgs args)
        {
            PaletteExpandingEventArgs paletteExpandArgs = new PaletteExpandingEventArgs()
            {
                Index = args.Index,
                Cancel = false,
                IsExpanded = args.IsExpanded,
                Palette = this.palettes[args.Index]
            };
            this.InvokeSymbolPaletteEvents(SymbolPaletteEvent.OnExpanding, paletteExpandArgs);
            args.Cancel = paletteExpandArgs.Cancel;
        }

        /// <summary>
        /// Invoked when the effective value of any property on this Symbol palette objects has been updated.
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="container">IDiagramObject</param>
        public void OnPropertyChanged(string propertyName, object newVal, object oldVal, IDiagramObject container)
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Creates a new palette that is a copy of the current palette. 
        /// </summary>
        /// <returns>Throws not implemented exception</returns>
        public object Clone()
        {
            return this;
        }
        internal void UpdateSymbolPaletteTemplates(SymbolPaletteTemplates symbolPaletteTemplates)
        {
            SymbolPaletteTemplates = symbolPaletteTemplates;
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            if (this.IsRendered && (Targets == null || Targets.Count == 0))
            {
                Dictionary.Dispose();

                if (DiagramLayerContent.MeasurePathDataCollection != null)
                {
                    DiagramLayerContent.MeasurePathDataCollection.Clear();
                    DiagramLayerContent.MeasurePathDataCollection = null;
                }

                if (DiagramLayerContent.MeasureTextDataCollection != null)
                {
                    DiagramLayerContent.MeasureTextDataCollection.Clear();
                    DiagramLayerContent.MeasureTextDataCollection = null;
                }

                if (DiagramLayerContent.MeasureImageDataCollection != null)
                {
                    DiagramLayerContent.MeasureImageDataCollection.Clear();
                    DiagramLayerContent.MeasureImageDataCollection = null;
                }

                if (DiagramLayerContent.MeasureNativeDataCollection != null)
                {
                    DiagramLayerContent.MeasureNativeDataCollection.Clear();
                    DiagramLayerContent.MeasureNativeDataCollection = null;
                }

                if (DiagramLayerContent.MeasureCustomPathDataCollection != null)
                {
                    DiagramLayerContent.MeasureCustomPathDataCollection.Clear();
                    DiagramLayerContent.MeasureCustomPathDataCollection = null;
                }
            }
            if (ChildContent != null)
            {
                ChildContent = null;
            }
            if (BpmnDiagrams != null)
            {
                BpmnDiagrams.Dispose();
                BpmnDiagrams = null;
            }
            Width = null;
            Height = null;
            if (SymbolDragPreviewSize != null)
            {
                SymbolDragPreviewSize.Dispose();
                SymbolDragPreviewSize = null;
            }
            if (SymbolDiagramPreviewSize != null)
            {
                SymbolDiagramPreviewSize.Dispose();
                SymbolDiagramPreviewSize = null;
            }
            ID = null;
            if (palettes != null)
            {
                for (int i = 0; i < palettes.Count; i++)
                {
                    palettes[i].Dispose();
                    palettes[i] = null;
                }
                palettes.Clear();
                palettes = null;
            }
            if (symbolMargin != null)
            {
                symbolMargin.Dispose();
                symbolMargin = null;
            }
            if (MeasurePathDataCollection != null)
            {
                MeasurePathDataCollection.Clear();
                MeasurePathDataCollection = null;
            }
            if (MeasureCustomPathDataCollection != null)
            {
                MeasureCustomPathDataCollection.Clear();
                MeasureCustomPathDataCollection = null;
            }
            if (MeasureTextDataCollection != null)
            {
                MeasureTextDataCollection.Clear();
                MeasureTextDataCollection = null;
            }
            if (MeasureImageDataCollection != null)
            {
                MeasureImageDataCollection.Clear();
                MeasureImageDataCollection = null;
            }
            if (MeasureNativeDataCollection != null)
            {
                MeasureNativeDataCollection.Clear();
                MeasureNativeDataCollection = null;
            }
            if (SelectedSymbol != null)
            {
                SelectedSymbol = null;
            }
            OldSymbol = null;
            if (SymbolTable != null)
            {
                SymbolTable.Clear();
                SymbolTable = null;
            }
            selfReference?.Dispose();
            if (PreviewSymbol != null)
            {
                PreviewSymbol = null;
            }
            DiagramId = null;
            if (Targets != null)
            {
                for (int i = 0; i < Targets.Count; i++)
                {
                    Targets[i] = null;
                }
                Targets.Clear();
                Targets = null;
            }

            if (SymbolPaletteTemplates != null)
            {
                SymbolPaletteTemplates.Dispose();
                SymbolPaletteTemplates = null;
            }
        }
    }
    /// <summary>
    /// A palette displays a set of similar symbols and annotates the group textually with its heading. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// new Palette()
    /// {
    ///     Id = "BasicShape",
    ///     Expanded = true,
    ///     Symbols = BasicShape,
    ///     Title = "Basic Shapes",
    ///     IconCss = "e-ddb-icons e-basic"
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class Palette : IPaletteObject
    {
        /// <summary>
        ///  Represents the unique id of a symbol group. By default, it is empty. 
        /// </summary>
        [Parameter]
        public string ID { get; set; }


        [JsonIgnore]
        internal SfSymbolPaletteComponent Parent { get; set; }

        //public string Height { get; set; } // Support is not there so we are removing
        /// <summary>
        /// Gets or sets whether the palette items are to be expanded or not. By default, it is true. 
        /// </summary>
        public bool IsExpanded { get; set; } = true;
        /// <summary>
        /// Represents the title of the symbol group. By default, it is empty.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Represents the class of the symbol group. By default, it is empty.
        /// </summary>
        public string IconCss { get; set; }

        private DiagramObjectCollection<NodeBase> symbols = new DiagramObjectCollection<NodeBase>() { };
        /// <summary>
        /// Represents the collection of predefined symbols.
        /// </summary>
        [Parameter]
        public DiagramObjectCollection<NodeBase> Symbols
        {
            get
            {
                symbols.Parent ??= this;
                return symbols;
            }
            set
            {
                if (value != null && symbols != value)
                {
                    if (symbols.Count != 0)
                    {
                        for (int i = 0; i < value.Count; i++)
                        {
                            this.Parent.UpdateDictionaryValue(value[i]);
                        }
                        _ = this.Parent.AddPaletteSymbol(value);
                    }
                    symbols = value;
                    symbols.Parent = this;
                }
            }
        }
        /// <summary>
        /// Invoked when the effective value of any property on this Symbol palette object has been updated.
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="container">IPaletteObject</param>
        public void OnPropertyChanged(string propertyName, object newVal, object oldVal, IPaletteObject container)
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Invoked when the effective value of any property on this Symbol palette object has been updated.
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="container">IPaletteObject</param>
        public void OnPropertyChanged(string propertyName, object newVal, object oldVal, IDiagramObject container)
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Creates a new palette that is a copy of the current palette. 
        /// </summary>
        /// <returns>Palette</returns>
        public object Clone()
        {
            return this;
        }

        internal void Dispose()
        {
            ID = null;
            Title = null;
            IconCss = null;
            if (Parent != null)
            {
                Parent = null;
            }
            if (symbols != null)
            {
                for (int i = 0; i < symbols.Count; i++)
                {
                    symbols[i].Dispose();
                    symbols[i] = null;
                }
                symbols.Clear();
                symbols = null;
            }

        }
    }
    /// <summary>
    /// Represents the size and description of a symbol. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfSymbolPaletteComponent Height="600px" Palettes="@Palettes" SymbolDragPreviewSize="@SymbolPreview" SymbolHeight="@symbolSizeHeight"  GetSymbolInfo="GetSymbolInfo" SymbolWidth="@symbolSizeWidth" SymbolMargin="@SymbolMargin">
    /// </SfSymbolPaletteComponent >
    /// @code
    /// { 
    ///     DiagramSize SymbolPreview;
    ///     SymbolMargin SymbolMargin = new SymbolMargin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
    ///     double symbolSizeWidth;
    ///     double symbolSizeHeight;
    ///     DiagramObjectCollection<Palette> Palettes = new DiagramObjectCollection<Palette>();
    ///     DiagramObjectCollection<NodeBase> Tnodes = new DiagramObjectCollection<NodeBase>();
    ///     private SymbolInfo GetSymbolInfo(IDiagramObject symbol)
    ///     {
    ///         SymbolInfo SymbolInfo = new SymbolInfo();
    ///         SymbolInfo.Fit = true;
    ///         return SymbolInfo;
    ///     }
    ///     protected override void OnInitialized()
    ///     {
    ///         SymbolPreview = new DiagramSize();
    ///         SymbolPreview.Width = 80;
    ///         SymbolPreview.Height = 80;
    ///         symbolSizeWidth = 50;
    ///         symbolSizeHeight = 50;
    ///         Tnodes = new DiagramObjectCollection<NodeBase>();
    ///         Node Tnode2 = new Node()
    ///         { 
    ///             ID = "node1", 
    ///             Shape = new FlowShape() { Type = Shapes.Flow, Shape = FlowShapesType.Decision } 
    ///         };
    ///         Tnodes.Add(Tnode2);
    ///         Palettes = new DiagramObjectCollection<Palette>()
    ///         {
    ///             new Palette(){Symbols =Tnodes,Title="Flow Shapes",Id="Flow Shapes" },
    ///         };
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class SymbolInfo
    {
        /// <summary>
        /// Represents the height of the symbol to be drawn over the palette. 
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Represents the width of the symbol to be drawn over the palette. 
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Represents whether the symbol has to fit into the size that is defined by the symbol palette. 
        /// </summary>
        public bool Fit { get; set; }

        /// <summary>
        /// Specifies the text to be displayed and how that is to be handled. 
        /// </summary>
        public SymbolDescription Description { get; set; }

        internal void Dispose()
        {
            if (Description != null)
            {
                Description.Dispose();
                Description = null;
            }
        }

    }

    /// <summary>
    /// Represents the textual description of a symbol. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfSymbolPaletteComponent Height="600px" Palettes="@Palettes" SymbolPreview="@SymbolPreview" SymbolHeight="@symbolSizeHeight"  GetSymbolInfo="GetSymbolInfo" SymbolWidth="@symbolSizeWidth" SymbolMargin="@SymbolMargin">
    /// </SfSymbolPaletteComponent >
    /// @code
    /// { 
    ///     Size SymbolPreview;
    ///     SymbolMargin SymbolMargin = new SymbolMargin { Left = 10, Right = 10, Top = 10, Bottom = 10 };
    ///     double symbolSizeWidth;
    ///     double symbolSizeHeight;
    ///     DiagramObjectCollection<Palette> Palettes = new DiagramObjectCollection<Palette>();
    ///     DiagramObjectCollection<NodeBase> Tnodes = new DiagramObjectCollection<NodeBase>();
    ///     private SymbolInfo GetSymbolInfo(IDiagramObject symbol)
    ///     {
    ///         SymbolInfo SymbolInfo = new SymbolInfo();
    ///         SymbolInfo.Fit = true;
    ///         string text = null;
    ///         if (symbol is Node)
    ///         {
    ///             text = ((symbol as Node).Shape as Shape).Type.ToString() + (symbol as Node).ID;
    ///         }
    ///         SymbolInfo.Description = new SymbolDescription() { Text = text };
    ///         return SymbolInfo;
    ///     }
    ///     protected override void OnInitialized()
    ///     {
    ///         SymbolPreview = new Size();
    ///         SymbolPreview.Width = 80;
    ///         SymbolPreview.Height = 80;
    ///         symbolSizeWidth = 50;
    ///         symbolSizeHeight = 50;
    ///         Tnodes = new DiagramObjectCollection<NodeBase>();
    ///         Node Tnode2 = new Node()
    ///         { 
    ///             ID = "node1", 
    ///             Shape = new FlowShape() { Type = Shapes.Flow, Shape = FlowShapesType.Decision } 
    ///         };
    ///         Tnodes.Add(Tnode2);
    ///         Palettes = new DiagramObjectCollection<Palette>()
    ///         {
    ///             new Palette(){Symbols =Tnodes,Title="Flow Shapes",Id="Flow Shapes" },
    ///         };
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class SymbolDescription
    {
        /// <summary>
        /// Represents the textual information to be displayed in the symbol. 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether to render ellipses (...) to indicate text overflow. By default, it is wrapped.
        /// </summary>
        public TextOverflow TextOverflow { get; set; }

        /// <summary>
        /// Wraps the text to the next line when it exceeds its bounds. 
        /// </summary>
        public TextWrap TextWrapping { get; set; }

        internal void Dispose()
        {
            Text = null;
        }
    }

    /// <summary>
    /// Notifies when the selection objects change in the symbol palette. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfSymbolPaletteComponent Width="@paletteWidth" Height="@paletteHeight"
    ///                           Palettes="@Palettes" SelectionChanged="PaletteSelectionChange">
    /// </SfSymbolPaletteComponent>
    /// private void PaletteSelectionChange(PaletteSelectionChangedEventArgs args)
    /// {
    ///     String oldID = args.OldValue;
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class PaletteSelectionChangedEventArgs
    {
        /// <summary>
        /// Returns the old palette item ID that is selected.
        /// </summary>
        public string OldValue { get; internal set; }
        /// <summary>
        /// Returns the new palette item ID that is selected.
        /// </summary>
        public string NewValue { get; internal set; }
    }
    /// <summary>
    /// Notifies when the palette items are expanded or collapsed in the symbol palette. 
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfSymbolPaletteComponent Width="@paletteWidth" Height="@paletteHeight"
    ///                           Palettes="@Palettes" Expanding="OnExpanding">
    /// </SfSymbolPaletteComponent>
    /// private void OnExpanding(PaletteExpandingEventArgs args)
    /// {
    ///     if (args.Cancel)
    ///     {
    ///         args.Cancel = true;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class PaletteExpandingEventArgs
    {
        /// <summary>
        /// Represents the index of the palette item being selected.
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// Represents the value if the palette item is expanded. 
        /// </summary>
        public bool IsExpanded { get; internal set; }
        /// <summary>
        /// Gets or Sets that indicate whether the palette item expand or collapse is to be canceled. 
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// Represents the properties of the palette item being selected.
        /// </summary>
        public Palette Palette { get; internal set; }
    }
    /// <summary>
    /// Specifies the extra space around the outer boundaries of the symbol.
    /// </summary>
    public class SymbolMargin : IPaletteObject
    {
        [JsonIgnore]
        internal SfSymbolPaletteComponent Parent { get; set; }

        private double bottom;
        private double left;
        private double right;
        private double top;

        /// <summary>
        /// Gets or sets the extra space at the bottom of the symbol.
        /// </summary>
        public double Bottom
        {
            get => bottom;
            set
            {
                bool canUpdate = bottom != 0 && !Bottom.Equals(value);
                bottom = value;
                if (canUpdate)
                {
                    this.OnPropertyChanged(null, null, null, null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space at the left of the symbol.
        /// </summary>
        public double Left
        {
            get => left;
            set
            {
                bool canUpdate = left != 0 && !Left.Equals(value);
                left = value;
                if (canUpdate)
                {
                    this.OnPropertyChanged(null, null, null, null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space at the right of the symbol.
        /// </summary>
        public double Right
        {
            get => right;

            set
            {
                bool canUpdate = right != 0 && !Right.Equals(value);
                right = value;
                if (canUpdate)
                {
                    this.OnPropertyChanged(null, null, null, null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the extra space at the top of the symbol.
        /// </summary>
        public double Top
        {
            get => top;
            set
            {
                bool canUpdate = top != 0 && !Top.Equals(value);
                top = value;
                if (canUpdate)
                {
                    this.OnPropertyChanged(null, null, null, null);
                }
            }
        }
        /// <summary>
        /// Invoked when the effective value of any property on this Symbol margin has been updated.
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="container">IPaletteObject</param>
        public void OnPropertyChanged(string propertyName, object newVal, object oldVal, IPaletteObject container)
        {
            this.Parent.UpdatePalettes();
        }
        /// <summary>
        /// Creates a new SymbolMargin that is a copy of the current SymbolMargin. 
        /// </summary>
        /// <returns>SymbolMargin</returns>
        public object Clone()
        {
            //throw new NotImplementedException();
            return this;
        }
        /// <summary>
        /// Invoked when the effective value of any property on this Symbol margin has been updated.
        /// </summary>
        /// <param name="propertyName">string</param>
        /// <param name="newVal">object</param>
        /// <param name="oldVal">object</param>
        /// <param name="container">IPaletteObject</param>
        public void OnPropertyChanged(string propertyName, object newVal, object oldVal, IDiagramObject container)
        {
            //throw new NotImplementedException();
        }
        internal void Dispose()
        {
            left = 0;
            right = 0;
            top = 0;
            bottom = 0;
        }
    }
}

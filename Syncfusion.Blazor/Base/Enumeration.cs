using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Script modules for the blazor components.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum ScriptModules
    {
        /// <summary>
        /// AccumulationChart.
        /// </summary>
        [EnumMember(Value = "accumulationchart")]
        [PackageName("WordProcessor")]
        AccumulationChart,

        /// <summary>
        /// Base.
        /// </summary>
        [EnumMember(Value = "base")]
        [PackageName("Core")]
        Base,
        /// <summary>
        /// Calendar.
        /// </summary>
        [EnumMember(Value = "calendar")]
        [PackageName("Calendars")]
        Calendar,

        /// <summary>
        /// DatePicker.
        /// </summary>
        [EnumMember(Value = "datepicker")]
        [PackageName("Calendars")]
        DatePicker,

        /// <summary>
        /// DateRangePicker.
        /// </summary>
        [EnumMember(Value = "daterangepicker")]
        [PackageName("Calendars")]
        DateRangePicker,

        /// <summary>
        /// DateTimePicker.
        /// </summary>
        [EnumMember(Value = "datetimepicker")]
        [PackageName("Calendars")]
        DateTimePicker,

        /// <summary>
        /// Timepicker.
        /// </summary>
        [EnumMember(Value = "timepicker")]
        [PackageName("Calendars")]
        Timepicker,

        /// <summary>
        /// Chart.
        /// </summary>
        [EnumMember(Value = "chart")]
        [PackageName("WordProcessor")]
        Chart,

        /// <summary>
        /// Compression.
        /// </summary>
        [EnumMember(Value = "compression")]
        [PackageName("Core")]
        Compression,
        [EnumMember(Value = "data")]
        [PackageName("Data")]
        DataManager,

        /// <summary>
        /// Drawings.
        /// </summary>
        [EnumMember(Value = "drawings")]
        [PackageName("PdfViewer")]
        Drawings,

        /// <summary>
        /// DropDownBase.
        /// </summary>
        [EnumMember(Value = "dropdownsbase")]
        [PackageName("DropDowns")]
        DropDownBase,

        /// <summary>
        /// DropDownList.
        /// </summary>
        [EnumMember(Value = "dropdownlist")]
        [PackageName("DropDowns")]
        DropDownList,

        /// <summary>
        /// AutoComplete.
        /// </summary>
        [EnumMember(Value = "autocomplete")]
        AutoComplete,

        /// <summary>
        /// ComboBox.
        /// </summary>
        [EnumMember(Value = "combobox")]
        [PackageName("DropDowns")]
        ComboBox,

        /// <summary>
        /// MultiSelect.
        /// </summary>
        [EnumMember(Value = "multiselect")]
        MultiSelect,

        /// <summary>
        /// ExcelExport.
        /// </summary>
        [EnumMember(Value = "excelexport")]
        [PackageName("Core")]
        ExcelExport,

        /// <summary>
        /// FileUtils.
        /// </summary>
        [EnumMember(Value = "fileutils")]
        [PackageName("Core")]
        FileUtils,

        /// <summary>
        /// Gantt.
        /// </summary>
        [EnumMember(Value = "gantt")]
        [PackageName("Gantt")]
        Gantt,
        /// <summary>
        /// InplaceEditor.
        /// </summary>
        [EnumMember(Value = "inplaceeditor")]
        [PackageName("PdfViewer")]
        InplaceEditor,

        /// <summary>
        /// InputBase.
        /// </summary>
        [EnumMember(Value = "inputsbase")]
        [PackageName("PdfViewer")]        
        InputBase,
        /// <summary>
        /// FormValidator.
        /// </summary>
        [EnumMember(Value = "formvalidator")]
        FormValidator,
        /// <summary>
        /// TextBox.
        /// </summary>
        [EnumMember(Value = "textbox")]
        [PackageName("PdfViewer")]
        TextBox,

        /// <summary>
        /// NumericTextBox.
        /// </summary>
        [EnumMember(Value = "numerictextbox")]
        [PackageName("Inputs")]
        NumericTextBox,

        /// <summary>
        /// MaskedTextBox.
        /// </summary>
        [EnumMember(Value = "maskedtextbox")]
        MaskedTextBox,

        /// <summary>
        /// Uploader.
        /// </summary>
        [EnumMember(Value = "uploader")]
        [PackageName("Inputs")]
        Uploader,

        /// <summary>
        /// Kanban.
        /// </summary>
        [EnumMember(Value = "kanban")]
        [PackageName("Kanban")]
        Kanban,

        /// <summary>
        /// OfficeChart.
        /// </summary>
        [EnumMember(Value = "officechart")]
        [PackageName("WordProcessor")]
        OfficeChart,

        /// <summary>
        /// PdfExport.
        /// </summary>
        [EnumMember(Value = "pdfexport")]
        [PackageName("Core")]
        PdfExport,

        /// <summary>
        /// PdfViewer.
        /// </summary>
        [EnumMember(Value = "pdfviewer")]
        [PackageName("PdfViewer")]
        PdfViewer,
        [EnumMember(Value = "schedule")]
        Schedule,
        [EnumMember(Value = "svgbase")]
        [PackageName("Core")]
        SvgBase,
        /// <summary>
        /// TreeGrid.
        /// </summary>
        [EnumMember(Value = "treegrid")]
        TreeGrid,

        /// <summary>
        /// Grid.
        /// </summary>
        [EnumMember(Value = "grid")]
        Grid,

        /// <summary>
        /// Pager.
        /// </summary>
        [EnumMember(Value = "pager")]
        Pager, 

        /// <summary>
        /// PopupsBase.
        /// </summary>
        [EnumMember(Value = "popupsbase")]
        [PackageName("Core")]
        PopupsBase,
        /// <summary>
        /// Popup.
        /// </summary>
        [EnumMember(Value = "popup")]
        [PackageName("Core")]
        Popup,

        /// <summary>
        /// Accordion.
        /// </summary>
        [EnumMember(Value = "accordion")]
        [PackageName("PdfViewer")]
        Accordion,

        /// <summary>
        /// ListView.
        /// </summary>
        [EnumMember(Value = "listview")]
        [PackageName("PdfViewer")]
        ListView,
        /// <summary>
        /// ListBase.
        /// </summary>
        [EnumMember(Value = "listsbase")]
        [PackageName("PdfViewer")]
        ListBase,

        /// <summary>
        /// ButtonBase.
        /// </summary>
        [EnumMember(Value = "buttonsbase")]
        [PackageName("PdfViewer")]
        ButtonBase,
        /// <summary>
        /// Button.
        /// </summary>
        [EnumMember(Value = "button")]
        [PackageName("PdfViewer")]
        Button,

        /// <summary>
        /// ContextMenu.
        /// </summary>
        [EnumMember(Value = "contextmenu")]
        [PackageName("PdfViewer")]
        ContextMenu,

        /// <summary>
        /// NavigationsBase.
        /// </summary>
        [EnumMember(Value = "navigationsbase")]
        [PackageName("Navigations")]
        NavigationsBase,

        /// <summary>
        /// SplitbuttonsBase.
        /// </summary>
        [EnumMember(Value = "splitbuttonsbase")]
        [PackageName("SplitButtons")]
        SplitbuttonsBase,
        /// <summary>
        /// Tooltip.
        /// </summary>
        [EnumMember(Value = "tooltip")]
        [PackageName("PdfViewer")]
        Tooltip,

        /// <summary>
        /// Diagram.
        /// </summary>
        [EnumMember(Value = "diagrams")]
        [PackageName("Diagrams")]
        Diagram,

        /// <summary>
        /// SymbolPalette.
        /// </summary>
        [EnumMember(Value = "symbolpalette")]
        [PackageName("Symbolpalette")]
        SymbolPalette,

        /// <summary>
        /// Overview.
        /// </summary>
        [EnumMember(Value = "overview")]
        [PackageName("Overview")]
        Overview,

        /// <summary>
        /// Dialog.
        /// </summary>
        [EnumMember(Value = "dialog")]
        [PackageName("PdfViewer")]
        Dialog,

        /// <summary>
        /// Spinner.
        /// </summary>
        [EnumMember(Value = "spinner")]
        [PackageName("Spinner")]
        Spinner,

        /// <summary>
        /// SfSvgExport.
        /// </summary>
        [EnumMember(Value = "sf-svg-export")]
        [PackageName("Core")]
        SfSvgExport,

        /// <summary>
        /// Sortable.
        /// </summary>
        [EnumMember(Value = "sortable")]
        [PackageName("DropDowns")]
        Sortable,
        /// <summary>
        /// SfTextbox.
        /// </summary>
        [EnumMember(Value = "sf-textbox")]
        [PackageName("Inputs")]
        SfTextBox,
    }

    /// <summary>
    /// Script modules for the native rendering components.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    internal enum SfScriptModules
    {
        /// <summary>
        /// None.
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        /// SfBase.
        /// </summary>
        [EnumMember(Value = "syncfusion-blazor")]
        [PackageName("Core")]
        SfBase,

        /// <summary>
        /// SfBaseExtended.
        /// </summary>
        [EnumMember(Value = "syncfusion-blazor-extended")]
        [PackageName("Core")]
        SfBaseExtended,

        /// <summary>
        /// SfGrid.
        /// </summary>
        [EnumMember(Value = "sf-grid")]
        [PackageName("Grid")]
        SfGrid,

        /// <summary>
        /// SfChart.
        /// </summary>
        [EnumMember(Value = "sf-chart")]
        [PackageName("Charts")]
        SfChart,

        /// <summary>
        /// SfAccumulationChart.
        /// </summary>
        [EnumMember(Value = "sf-accumulation-chart")]
        [PackageName("Charts")]
        SfAccumulationChart,

        /// <summary>
        /// SfRangeNavigator.
        /// </summary>
        [EnumMember(Value = "sf-range-navigator")]
        [PackageName("RangeNavigator")]
        SfRangeNavigator,

        /// <summary>
        /// SfProgressBar.
        /// </summary>
        [EnumMember(Value = "sf-progressbar")]
        [PackageName("ProgressBar")]
        SfProgressBar,

        /// <summary>
        /// SfGantt.
        /// </summary>
        [EnumMember(Value = "sf-gantt")]
        [PackageName("Gantt")]
        SfGantt,

        /// <summary>
        /// SfTreeGrid.
        /// </summary>
        [EnumMember(Value = "sf-treegrid")]
        [PackageName("TreeGrid")]
        SfTreeGrid,

        /// <summary>
        /// SfPivotView.
        /// </summary>
        [EnumMember(Value = "sf-pivotview")]
        [PackageName("PivotTable")]
        SfPivotView,

        /// <summary>
        /// SfTextBox.
        /// </summary>
        [EnumMember(Value = "sf-textbox")]
        [PackageName("Inputs")]
        SfTextBox,

        /// <summary>
        /// SfNumericTextBox.
        /// </summary>
        [EnumMember(Value = "sf-numerictextbox")]
        [PackageName("Inputs")]
        SfNumericTextBox,

        /// <summary>
        /// SfMaskedTextBox.
        /// </summary>
        [EnumMember(Value = "sf-maskedtextbox")]
        [PackageName("Inputs")]
        SfMaskedTextBox,

        /// <summary>
        /// SfUploader.
        /// </summary>
        [EnumMember(Value = "sf-uploader")]
        [PackageName("Inputs")]
        SfUploader,

        /// <summary>
        /// SfSlider.
        /// </summary>
        [EnumMember(Value = "sf-slider")]
        [PackageName("Inputs")]
        SfSlider,

        /// <summary>
        /// SfDropDownList.
        /// </summary>
        [EnumMember(Value = "sf-dropdownlist")]
        [PackageName("DropDowns")]
        SfDropDownList,

        /// <summary>
        /// SfMultiSelect.
        /// </summary>
        [EnumMember(Value = "sf-multiselect")]
        [PackageName("DropDowns")]
        SfMultiSelect,

        /// <summary>
        /// SfCalendarBase.
        /// </summary>
        [EnumMember(Value = "sf-calendarbase")]
        [PackageName("Calendars")]
        SfCalendarBase,

        /// <summary>
        /// SfDatePicker.
        /// </summary>
        [EnumMember(Value = "sf-datepicker")]
        [PackageName("Calendars")]
        SfDatePicker,

        /// <summary>
        /// SfTimePicker.
        /// </summary>
        [EnumMember(Value = "sf-timepicker")]
        [PackageName("Calendars")]
        SfTimePicker,

        /// <summary>
        /// SfToolbar.
        /// </summary>
        [EnumMember(Value = "sf-toolbar")]
        [PackageName("Navigations")]
        SfToolbar,

        /// <summary>
        /// SfSplitter.
        /// </summary>
        [EnumMember(Value = "sf-splitter")]
        [PackageName("Layouts")]
        SfSplitter,

        /// <summary>
        /// SfDashboardLayout.
        /// </summary>
        [EnumMember(Value = "sf-dashboard-layout")]
        [PackageName("Layouts")]
        SfDashboardLayout,

        /// <summary>
        /// SfDialog.
        /// </summary>
        [EnumMember(Value = "sf-dialog")]
        [PackageName("Popups")]
        SfDialog,

        /// <summary>
        /// SfTab.
        /// </summary>
        [EnumMember(Value = "sf-tab")]
        [PackageName("Navigations")]
        SfTab,

        /// <summary>
        /// SfDropDownButton.
        /// </summary>
        [EnumMember(Value = "sf-drop-down-button")]
        [PackageName("SplitButtons")]
        SfDropDownButton,

        /// <summary>
        /// SfTooltip.
        /// </summary>
        [EnumMember(Value = "sf-tooltip")]
        [PackageName("Popups")]
        SfTooltip,

        /// <summary>
        /// SfBarcode.
        /// </summary>
        [EnumMember(Value = "sf-barcode")]
        [PackageName("BarcodeGenerator")]
        SfBarcode,

        /// <summary>
        /// SfAccordion.
        /// </summary>
        [EnumMember(Value = "sf-accordion")]
        [PackageName("Navigations")]
        SfAccordion,

        /// <summary>
        /// SfContextMenu.
        /// </summary>
        [EnumMember(Value = "sf-contextmenu")]
        [PackageName("Navigations")]
        SfContextMenu,

        /// <summary>
        /// SfMenu.
        /// </summary>
        [EnumMember(Value = "sf-menu")]
        [PackageName("Navigations")]
        SfMenu,

        /// <summary>
        /// SfMaps.
        /// </summary>
        [EnumMember(Value = "sf-maps")]
        [PackageName("Maps")]
        SfMaps,

        /// <summary>
        /// SfListView.
        /// </summary>
        [EnumMember(Value = "sf-listview")]
        [PackageName("Lists")]
        SfListView,

        /// <summary>
        /// SfFileManager.
        /// </summary>
        [EnumMember(Value = "sf-filemanager")]
        [PackageName("FileManager")]
        SfFileManager,

        /// <summary>
        /// SfTreeView.
        /// </summary>
        [EnumMember(Value = "sf-treeview")]
        [PackageName("Navigations")]
        SfTreeView,

        /// <summary>
        /// SfSidebar.
        /// </summary>
        [EnumMember(Value = "sf-sidebar")]
        [PackageName("Navigations")]
        SfSidebar,

        /// <summary>
        /// SfToast.
        /// </summary>
        [EnumMember(Value = "sf-toast")]
        [PackageName("Notifications")]
        SfToast,

        /// <summary>
        /// SfDateRangePicker.
        /// </summary>
        [EnumMember(Value = "sf-daterangepicker")]
        [PackageName("Calendars")]
        SfDateRangePicker,

        /// <summary>
        /// SfRichTextEditor.
        /// </summary>
        [EnumMember(Value = "sf-richtexteditor")]
        [PackageName("RichTextEditor")]
        SfRichTextEditor,

        /// <summary>
        /// SfSpinner.
        /// </summary>
        [EnumMember(Value = "sf-spinner")]
        [PackageName("Spinner")]
        SfSpinner,

        /// <summary>
        /// SfCircularGauge.
        /// </summary>
        [EnumMember(Value = "sf-circulargauge")]
        [PackageName("CircularGauge")]
        SfCircularGauge,

        /// <summary>
        /// SfKanban.
        /// </summary>
        [EnumMember(Value = "sf-kanban")]
        [PackageName("Kanban")]
        SfKanban,

        /// <summary>
        /// SfSchedule.
        /// </summary>
        [EnumMember(Value = "sf-schedule")]
        [PackageName("Schedule")]
        SfSchedule,

        /// <summary>
        /// SfTreeMap.
        /// </summary>
        [EnumMember(Value = "sf-treemap")]
        [PackageName("TreeMap")]
        SfTreeMap,

        /// <summary>
        /// SfLinearGauge.
        /// </summary>
        [EnumMember(Value = "sf-lineargauge")]
        [PackageName("LinearGauge")]
        SfLinearGauge,

        /// <summary>
        /// SfListBox.
        /// </summary>
        [EnumMember(Value = "sf-listbox")]
        [PackageName("DropDowns")]
        SfListBox,

        /// <summary>
        /// SfInPlaceEditor.
        /// </summary>
        [EnumMember(Value = "sf-inplaceeditor")]
        [PackageName("InPlaceEditor")]
        SfInPlaceEditor,

        /// <summary>
        /// SfColorPicker.
        /// </summary>
        [EnumMember(Value = "sf-colorpicker")]
        [PackageName("Inputs")]
        SfColorPicker,

        /// <summary>
        /// SfSmithChart.
        /// </summary>
        [EnumMember(Value = "sf-smith-chart")]
        [PackageName("SmithChart")]
        SfSmithChart,

        /// <summary>
        /// SfBulletChart.
        /// </summary>
        [EnumMember(Value = "sf-bullet-chart")]
        [PackageName("BulletChart")]
        SfBulletChart,

        /// <summary>
        /// SfSparkline.
        /// </summary>
        [EnumMember(Value = "sf-sparkline")]
        [PackageName("Sparkline")]
        SfSparkline,

        /// <summary>
        /// SfStockChart.
        /// </summary>
        [EnumMember(Value = "sf-stock-chart")]
        [PackageName("StockChart")]
        SfStockChart,

        /// <summary>
        /// SfDocumentEditorContainer.
        /// </summary>
        [EnumMember(Value = "sf-documenteditorcontainer")]
        [PackageName("WordProcessor")]
        SfDocumentEditorContainer,

        /// <summary>
        /// SfDocumentEditor.
        /// </summary>
        [EnumMember(Value = "sf-documenteditor")]
        [PackageName("WordProcessor")]
        SfDocumentEditor,       

        /// <summary>
        /// HeatMap.
        /// </summary> 
        [EnumMember(Value = "sf-heatmap")]
        [PackageName("HeatMap")]
        SfHeatMap,

        /// <summary>
        /// SfDiagramComponent
        /// </summary>
        [EnumMember(Value = "sf-diagramcomponent")]
        [PackageName("Diagram")]
        SfDiagramComponent
    }
}

namespace Syncfusion.Blazor
{
    /// <summary>
    /// Specifies the Animation effects.
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum AnimationEffect
    {
        /// <summary>
        /// Defines the Animation effect as SlideLeftIn.
        /// </summary>
        [EnumMember(Value = "SlideLeftIn")]
        SlideLeftIn,
        /// <summary>
        /// Defines the Animation effect as SlideRightIn.
        /// </summary>
        [EnumMember(Value = "SlideRightIn")]
        SlideRightIn,
        /// <summary>
        /// Defines the Animation effect as FadeIn.
        /// </summary>
        [EnumMember(Value = "FadeIn")]
        FadeIn,
        /// <summary>
        /// Defines the Animation effect as FadeOut.
        /// </summary>
        [EnumMember(Value = "FadeOut")]
        FadeOut,
        /// <summary>
        /// Defines the Animation effect as FadeZoomIn.
        /// </summary>
        [EnumMember(Value = "FadeZoomIn")]
        FadeZoomIn,
        /// <summary>
        /// Defines the Animation effect as FadeZoomOut.
        /// </summary>
        [EnumMember(Value = "FadeZoomOut")]
        FadeZoomOut,
        /// <summary>
        /// Defines the Animation effect as ZoomIn.
        /// </summary>
        [EnumMember(Value = "ZoomIn")]
        ZoomIn,
        /// <summary>
        /// Defines the Animation effect as ZoomOut.
        /// </summary>
        [EnumMember(Value = "ZoomOut")]
        ZoomOut,
        /// <summary>
        /// Defines the Animation effect as SlideLeft.
        /// </summary>
        [EnumMember(Value = "SlideLeft")]
        SlideLeft,
        /// <summary>
        /// Defines the Animation effect as SlideRight.
        /// </summary>
        [EnumMember(Value = "SlideRight")]
        SlideRight,
        /// <summary>
        /// Defines the Animation effect as FlipLeftDownIn.
        /// </summary>
        [EnumMember(Value = "FlipLeftDownIn")]
        FlipLeftDownIn,
        /// <summary>
        /// Defines the Animation effect as FlipLeftDownOut.
        /// </summary>
        [EnumMember(Value = "FlipLeftDownOut")]
        FlipLeftDownOut,
        /// <summary>
        /// Defines the Animation effect as FlipLeftUpIn.
        /// </summary>
        [EnumMember(Value = "FlipLeftUpIn")]
        FlipLeftUpIn,
        /// <summary>
        /// Defines the Animation effect as FlipLeftUpOut.
        /// </summary>
        [EnumMember(Value = "FlipLeftUpOut")]
        FlipLeftUpOut,
        /// <summary>
        /// Defines the Animation effect as FlipRightDownIn.
        /// </summary>
        [EnumMember(Value = "FlipRightDownIn")]
        FlipRightDownIn,
        /// <summary>
        /// Defines the Animation effect as FlipRightDownOut.
        /// </summary>
        [EnumMember(Value = "FlipRightDownOut")]
        FlipRightDownOut,
        /// <summary>
        /// Defines the Animation effect as FlipRightUpIn.
        /// </summary>
        [EnumMember(Value = "FlipRightUpIn")]
        FlipRightUpIn,
        /// <summary>
        /// Defines the Animation effect as FlipRightUpOut.
        /// </summary>
        [EnumMember(Value = "FlipRightUpOut")]
        FlipRightUpOut,
        /// <summary>
        /// Defines the Animation effect as FlipXDownIn.
        /// </summary>
        [EnumMember(Value = "FlipXDownIn")]
        FlipXDownIn,
        /// <summary>
        /// Defines the Animation effect as FlipXDownOut.
        /// </summary>
        [EnumMember(Value = "FlipXDownOut")]
        FlipXDownOut,
        /// <summary>
        /// Defines the Animation effect as FlipXUpIn.
        /// </summary>
        [EnumMember(Value = "FlipXUpIn")]
        FlipXUpIn,
        /// <summary>
        /// Defines the Animation effect as FlipXUpOut.
        /// </summary>
        [EnumMember(Value = "FlipXUpOut")]
        FlipXUpOut,
        /// <summary>
        /// Defines the Animation effect as FlipYLeftIn.
        /// </summary>
        [EnumMember(Value = "FlipYLeftIn")]
        FlipYLeftIn,
        /// <summary>
        /// Defines the Animation effect as FlipYLeftOut.
        /// </summary>
        [EnumMember(Value = "FlipYLeftOut")]
        FlipYLeftOut,
        /// <summary>
        /// Defines the Animation effect as FlipYRightIn.
        /// </summary>
        [EnumMember(Value = "FlipYRightIn")]
        FlipYRightIn,
        /// <summary>
        /// Defines the Animation effect as FlipYRightOut.
        /// </summary>
        [EnumMember(Value = "FlipYRightOut")]
        FlipYRightOut,
        /// <summary>
        /// Defines the Animation effect as SlideBottomIn.
        /// </summary>
        [EnumMember(Value = "SlideBottomIn")]
        SlideBottomIn,
        /// <summary>
        /// Defines the Animation effect as SlideBottomOut.
        /// </summary>
        [EnumMember(Value = "SlideBottomOut")]
        SlideBottomOut,
        /// <summary>
        /// Defines the Animation effect as SlideDown.
        /// </summary>
        [EnumMember(Value = "SlideDown")]
        SlideDown,
        /// <summary>
        /// Defines the Animation effect as SlideUp.
        /// </summary>
        [EnumMember(Value = "SlideUp")]
        SlideUp,
        /// <summary>
        /// Defines the Animation effect as SlideLeftOut.
        /// </summary>
        [EnumMember(Value = "SlideLeftOut")]
        SlideLeftOut,
        /// <summary>
        /// Defines the Animation effect as SlideRightOut.
        /// </summary>
        [EnumMember(Value = "SlideRightOut")]
        SlideRightOut,
        /// <summary>
        /// Defines the Animation effect as SlideTopIn.
        /// </summary>
        [EnumMember(Value = "SlideTopIn")]
        SlideTopIn,
        /// <summary>
        /// Defines the Animation effect as SlideTopOut.
        /// </summary>
        [EnumMember(Value = "SlideTopOut")]
        SlideTopOut,
        /// <summary>
        /// Defines the Animation effect as None.
        /// </summary>
        [EnumMember(Value = "None")]
        None
    }
    /// <summary>
    /// Defines the theme style of the Syncfusion component.
    /// </summary>
    public enum Theme
    {
        /// <summary>
        /// Renders the Syncfusion component with material theme.
        /// </summary>
        Material,
        /// <summary>
        /// Renders the Syncfusion component with bootstrap theme.
        /// </summary>
        Bootstrap,
        /// <summary>
        /// Renders the Syncfusion component with high contrast light theme.
        /// </summary>
        HighContrastLight,
        /// <summary>
        /// Renders the Syncfusion component with fabric theme.
        /// </summary>
        Fabric,
        /// <summary>
        /// Renders the Syncfusion component with material dark theme.
        /// </summary>
        MaterialDark,
        /// <summary>
        /// Renders the Syncfusion component with fabric dark theme.
        /// </summary>
        FabricDark,
        /// <summary>
        /// Renders the Syncfusion component with high contrast theme.
        /// </summary>
        HighContrast,
        /// <summary>
        /// Renders the Syncfusion component with bootstrap dark theme.
        /// </summary>
        BootstrapDark,
        /// <summary>
        /// Renders the Syncfusion component with bootstrap4 theme.
        /// </summary>
        Bootstrap4,
        /// <summary>
        /// Renders the Syncfusion component with bootstrap5 theme.
        /// </summary>
        Bootstrap5,
        /// <summary>
        /// Renders the Syncfusion component with bootstrap5Dark theme.
        /// </summary>
        Bootstrap5Dark,
        /// <summary>
        /// Renders the Syncfusion component with tailwind theme.
        /// </summary>
        Tailwind,
        /// <summary>
        /// Renders the Syncfusion component with tailwind dark theme.
        /// </summary>
        TailwindDark
    }
}

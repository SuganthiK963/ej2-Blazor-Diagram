window.sf = window.sf || {};
window.sf.treegrid = (function (exports) {
'use strict';

/**
 * Represents TreeGrid `Column` model class.
 */
var Column = /** @class */ (function () {
    function Column(options) {
        /**
         * If `allowEditing` set to false, then it disables editing of a particular column.
         * By default all columns are editable.
         *
         * @default true
         */
        this.allowEditing = true;
        /**
         * Defines the `IEditCell` object to customize default edit cell.
         *
         * @default {}
         */
        this.edit = {};
        /**
         * If `disableHtmlEncode` is set to true, it encodes the HTML of the header and content cells.
         *
         * @default true
         */
        this.disableHtmlEncode = true;
        /**
         * If `allowReordering` set to false, then it disables reorder of a particular column.
         * By default all columns can be reorder.
         *
         * @default true
         */
        this.allowReordering = true;
        /**
         * If `showColumnMenu` set to false, then it disable the column menu of a particular column.
         * By default column menu will show for all columns
         *
         * @default true
         */
        this.showColumnMenu = true;
        /**
         * If `allowFiltering` set to false, then it disables filtering option and filter bar element of a particular column.
         * By default all columns are filterable.
         *
         * @default true
         */
        this.allowFiltering = true;
        /**
         * If `allowSorting` set to false, then it disables sorting option of a particular column.
         * By default all columns are sortable.
         *
         * @default true
         */
        this.allowSorting = true;
        /**
         * If `allowResizing` is set to false, it disables resize option of a particular column.
         * By default all the columns can be resized.
         *
         * @default true
         */
        this.allowResizing = true;
        /**
         *  It is used to customize the default filter options for a specific columns.
         * * type -  Specifies the filter type as menu.
         * * ui - to render custom component for specific column it has following functions.
         * * ui.create ??? It is used for creating custom components.
         * * ui.read -  It is used for read the value from the component.
         * * ui.write - It is used to apply component model as dynamically.
         *
         *  @default null
         */
        this.filter = {};
        sf.base.merge(this, options);
    }
    return Column;
}());

var __extends$1 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$1 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Represents the Tree Grid predicate for the filter column.
 */
var Predicate$1 = /** @class */ (function (_super) {
    __extends$1(Predicate$$1, _super);
    function Predicate$$1() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "field", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "operator", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "value", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "matchCase", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "ignoreAccent", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "predicate", void 0);
    __decorate$1([
        sf.base.Property({})
    ], Predicate$$1.prototype, "actualFilterValue", void 0);
    __decorate$1([
        sf.base.Property({})
    ], Predicate$$1.prototype, "actualOperator", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "type", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "ejpredicate", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "uid", void 0);
    __decorate$1([
        sf.base.Property()
    ], Predicate$$1.prototype, "isForeignKey", void 0);
    return Predicate$$1;
}(sf.base.ChildProperty));
/**
 * Configures the filtering behavior of the TreeGrid.
 */
var FilterSettings = /** @class */ (function (_super) {
    __extends$1(FilterSettings, _super);
    function FilterSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Collection([], Predicate$1)
    ], FilterSettings.prototype, "columns", void 0);
    __decorate$1([
        sf.base.Property('FilterBar')
    ], FilterSettings.prototype, "type", void 0);
    __decorate$1([
        sf.base.Property()
    ], FilterSettings.prototype, "mode", void 0);
    __decorate$1([
        sf.base.Property(true)
    ], FilterSettings.prototype, "showFilterBarStatus", void 0);
    __decorate$1([
        sf.base.Property(1500)
    ], FilterSettings.prototype, "immediateModeDelay", void 0);
    __decorate$1([
        sf.base.Property()
    ], FilterSettings.prototype, "operators", void 0);
    __decorate$1([
        sf.base.Property(false)
    ], FilterSettings.prototype, "ignoreAccent", void 0);
    __decorate$1([
        sf.base.Property('Parent')
    ], FilterSettings.prototype, "hierarchyMode", void 0);
    return FilterSettings;
}(sf.base.ChildProperty));

var __extends$2 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$2 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the textwrap behavior of the TreeGrid.
 */
var TextWrapSettings = /** @class */ (function (_super) {
    __extends$2(TextWrapSettings, _super);
    function TextWrapSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property('Both')
    ], TextWrapSettings.prototype, "wrapMode", void 0);
    return TextWrapSettings;
}(sf.base.ChildProperty));

var __extends$3 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/**
 * Logger module for TreeGrid
 *
 * @hidden
 */
var DOC_URL = 'https://ej2.syncfusion.com/documentation/treegrid';
var BASE_DOC_URL = 'https://ej2.syncfusion.com/documentation';
var ERROR = '[EJ2TreeGrid.Error]';
var IsRowDDEnabled = false;
var Logger$1 = /** @class */ (function (_super) {
    __extends$3(Logger$$1, _super);
    function Logger$$1(parent) {
        var _this = this;
        sf.grids.Grid.Inject(sf.grids.Logger);
        _this = _super.call(this, parent) || this;
        return _this;
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} - Returns Logger module name
     */
    Logger$$1.prototype.getModuleName = function () {
        return 'logger';
    };
    Logger$$1.prototype.log = function (types, args) {
        if (!(types instanceof Array)) {
            types = [types];
        }
        var type = types;
        for (var i = 0; i < type.length; i++) {
            var item = sf.grids.detailLists[type[i]];
            var cOp = item.check(args, this.parent);
            if (cOp.success) {
                var message = item.generateMessage(args, this.parent, cOp.options);
                message = message.replace('EJ2Grid', 'EJ2TreeGrid').replace('* Hierarchy Grid', '').replace('* Grouping', '');
                if (IsRowDDEnabled && type[i] === 'primary_column_missing') {
                    message = message.replace('Editing', 'Row DragAndDrop');
                    IsRowDDEnabled = false;
                }
                var index = message.indexOf('https');
                var gridurl = message.substring(index);
                if (type[i] === 'module_missing') {
                    message = message.replace(gridurl, DOC_URL + '/modules');
                }
                else if (type[i] === 'primary_column_missing' || type[i] === 'selection_key_missing') {
                    message = message.replace(gridurl, BASE_DOC_URL + '/api/treegrid/column/#isprimarykey');
                }
                else if (type[i] === 'grid_remote_edit') {
                    message = message.replace(gridurl, DOC_URL + '/edit');
                }
                else if (type[i] === 'virtual_height') {
                    message = message.replace(gridurl, DOC_URL + '/virtual');
                }
                else if (type[i] === 'check_datasource_columns') {
                    message = message.replace(gridurl, DOC_URL + '/columns');
                }
                else if (type[i] === 'locale_missing') {
                    message = message.replace(gridurl, DOC_URL + '/global-local/#localization');
                }
                if (type[i] === 'datasource_syntax_mismatch') {
                    if (!sf.base.isNullOrUndefined(this.treeGridObj) && !sf.base.isNullOrUndefined(this.treeGridObj.dataStateChange)) {
                        // eslint-disable-next-line no-console
                        console[item.logType](message);
                    }
                }
                else {
                    // eslint-disable-next-line no-console
                    console[item.logType](message);
                }
            }
        }
    };
    Logger$$1.prototype.treeLog = function (types, args, treeGrid) {
        this.treeGridObj = treeGrid;
        if (!(types instanceof Array)) {
            types = [types];
        }
        var type = types;
        if (treeGrid.allowRowDragAndDrop && !treeGrid.columns.filter(function (column) { return column.isPrimaryKey; }).length) {
            IsRowDDEnabled = true;
            this.log('primary_column_missing', args);
        }
        for (var i = 0; i < type.length; i++) {
            var item = treeGridDetails[type[i]];
            var cOp = item.check(args, treeGrid);
            if (cOp.success) {
                var message = item.generateMessage(args, treeGrid, cOp.options);
                // eslint-disable-next-line no-console
                console[item.logType](message);
            }
        }
    };
    return Logger$$1;
}(sf.grids.Logger));
var treeGridDetails = {
    // eslint-disable-next-line camelcase
    mapping_fields_missing: {
        type: 'mapping_fields_missing',
        logType: 'error',
        check: function (args, parent) {
            var opt = { success: false };
            if ((sf.base.isNullOrUndefined(parent.idMapping) && sf.base.isNullOrUndefined(parent.childMapping)
                && sf.base.isNullOrUndefined(parent.parentIdMapping)) ||
                (!sf.base.isNullOrUndefined(parent.idMapping) && sf.base.isNullOrUndefined(parent.parentIdMapping)) ||
                (sf.base.isNullOrUndefined(parent.idMapping) && !sf.base.isNullOrUndefined(parent.parentIdMapping))) {
                opt = { success: true };
            }
            return opt;
        },
        generateMessage: function () {
            return ERROR + ':' + ' MAPPING FIELDS MISSING \n' + 'One of the following fields is missing. It is ' +
                'required for the hierarchical relationship of records in TreeGrid:\n' +
                '* childMapping\n' + '* idMapping\n' + '* parentIdMapping\n' +
                'Refer to the following documentation links for more details.\n' +
                (BASE_DOC_URL + "/api/treegrid#childmapping") + '\n' +
                (BASE_DOC_URL + "/api/treegrid#idmapping") + '\n' +
                (BASE_DOC_URL + "/api/treegrid#$parentidmapping");
        }
    }
};

/**
 *  @hidden
 */
var load = 'load';
/** @hidden */
var rowDataBound = 'rowDataBound';
/** @hidden */
var dataBound = 'dataBound';
/** @hidden */
var queryCellInfo = 'queryCellInfo';
/** @hidden */
var beforeDataBound = 'beforeDataBound';
/** @hidden */
var actionBegin = 'actionBegin';
/** @hidden */
var dataStateChange = 'dataStateChange';
/** @hidden */
var actionComplete = 'actionComplete';
/** @hidden */
var rowSelecting = 'rowSelecting';
/** @hidden */
var rowSelected = 'rowSelected';
/** @hidden */
var checkboxChange = 'checkboxChange';
/** @hidden */
var rowDeselected = 'rowDeselected';
/** @hidden */
var toolbarClick = 'toolbarClick';
/** @hidden */
var beforeExcelExport = 'beforeExcelExport';
/** @hidden */
var beforePdfExport = 'beforePdfExport';
/** @hidden */
var resizeStop = 'resizeStop';
/** @hidden */
var expanded = 'expanded';
/** @hidden */
var expanding = 'expanding';
/** @hidden */
var collapsed = 'collapsed';
/** @hidden */
var collapsing = 'collapsing';
/** @hidden */
var remoteExpand = 'remoteExpand';
/** @hidden */
var localPagedExpandCollapse = 'localPagedExpandCollapse';
/** @hidden */
var pagingActions = 'pagingActions';
/** @hidden */
var printGridInit = 'printGrid-Init';
/** @hidden */
var contextMenuOpen = 'contextMenuOpen';
/** @hidden */
var contextMenuClick = 'contextMenuClick';
/** @hidden */
var beforeCopy = 'beforeCopy';
/** @hidden */
var beforePaste = 'beforePaste';
/** @hidden */
var savePreviousRowPosition = 'savePreviousRowPosition';
/** @hidden */
var crudAction = 'crudAction';
/** @hidden */
var beginEdit = 'beginEdit';
/** @hidden */
var beginAdd = 'beginAdd';
/** @hidden */
var recordDoubleClick = 'recordDoubleClick';
/** @hidden */
var cellSave = 'cellSave';
/** @hidden */
var cellSaved = 'cellSaved';
/** @hidden */
var cellEdit = 'cellEdit';
/** @hidden */
var batchDelete = 'batchDelete';
/** @hidden */
var batchCancel = 'batchCancel';
/** @hidden */
var batchAdd = 'batchAdd';
/** @hidden */
var beforeBatchDelete = 'beforeBatchDelete';
/** @hidden */
var beforeBatchAdd = 'beforeBatchAdd';
/** @hidden */
var beforeBatchSave = 'beforeBatchSave';
/** @hidden */
var batchSave = 'batchSave';
/** @hidden */
var keyPressed = 'key-pressed';
/** @hidden */
var updateData = 'update-data';
/** @hidden */
var doubleTap = 'double-tap';
/** @hidden */
var virtualColumnIndex = 'virtualColumnIndex';
/** @hidden */
var virtualActionArgs = 'virtual-action-args';
/** @hidden */
var dataListener = 'data-listener';
/** @hidden */
var indexModifier = 'index-modifier';
/** @hidden */
var beforeStartEdit = 'edit-form';
/** @hidden */
var beforeBatchCancel = 'before-batch-cancel';
/** @hidden */
var batchEditFormRendered = 'batcheditform-rendered';
/** @hidden */
var detailDataBound = 'detailDataBound';
/** @hidden */
var rowDrag = 'rowDrag';
/** @hidden */
var rowDragStartHelper = 'rowDragStartHelper';
/** @hidden */
var rowDrop = 'rowDrop';
/** @hidden */
var rowDragStart = 'rowDragStart';
/** @hidden */
var rowsAdd = 'rows-add';
/** @hidden */
var rowsRemove = 'rows-remove';
/** @hidden */
var rowdraging = 'row-draging';
/** @hidden */
var rowDropped = 'row-dropped';

var __extends$4 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/**
 * The `Clipboard` module is used to handle clipboard copy action.
 *
 * @hidden
 */
var TreeClipboard = /** @class */ (function (_super) {
    __extends$4(TreeClipboard, _super);
    function TreeClipboard(parent) {
        var _this = _super.call(this, parent.grid) || this;
        _this.treeCopyContent = '';
        _this.copiedUniqueIdCollection = [];
        _this.treeGridParent = parent;
        return _this;
    }
    TreeClipboard.prototype.setCopyData = function (withHeader) {
        var copyContent = 'copyContent';
        var getCopyData = 'getCopyData';
        var isSelect = 'isSelect';
        var uniqueID = 'uniqueID';
        var currentRecords = this.treeGridParent.getCurrentViewRecords();
        if (window.getSelection().toString() === '') {
            this.clipBoardTextArea.value = this[copyContent] = '';
            var rows = this.treeGridParent.grid.getRows();
            if (this.treeGridParent.selectionSettings.mode !== 'Cell') {
                var selectedIndexes = this.treeGridParent.getSelectedRowIndexes().sort(function (a, b) {
                    return a - b;
                });
                for (var i = 0; i < selectedIndexes.length; i++) {
                    if (i > 0) {
                        this.treeCopyContent += '\n';
                    }
                    if (!rows[selectedIndexes[i]].classList.contains('e-summaryrow')) {
                        var cells = [].slice.call(rows[selectedIndexes[i]].querySelectorAll('.e-rowcell'));
                        var uniqueid = this.treeGridParent.getSelectedRecords()[i][uniqueID];
                        if (this.copiedUniqueIdCollection.indexOf(uniqueid) === -1) {
                            if (this.treeGridParent.copyHierarchyMode === 'Parent' || this.treeGridParent.copyHierarchyMode === 'Both') {
                                this.parentContentData(currentRecords, selectedIndexes[i], rows, withHeader, i);
                            }
                            this[getCopyData](cells, false, '\t', withHeader);
                            this.treeCopyContent += this[copyContent];
                            this.copiedUniqueIdCollection.push(uniqueid);
                            this[copyContent] = '';
                            if (this.treeGridParent.copyHierarchyMode === 'Child' || this.treeGridParent.copyHierarchyMode === 'Both') {
                                this.childContentData(currentRecords, selectedIndexes[i], rows, withHeader);
                            }
                        }
                    }
                }
                if (withHeader) {
                    var headerTextArray = [];
                    for (var i = 0; i < this.treeGridParent.getVisibleColumns().length; i++) {
                        headerTextArray[i] = this.treeGridParent.getVisibleColumns()[i].headerText;
                    }
                    this[getCopyData](headerTextArray, false, '\t', withHeader);
                    this.treeCopyContent = this[copyContent] + '\n' + this.treeCopyContent;
                }
                var args = {
                    data: this.treeCopyContent,
                    cancel: false
                };
                this.treeGridParent.trigger(beforeCopy, args);
                if (args.cancel) {
                    return;
                }
                this.clipBoardTextArea.value = this[copyContent] = args.data;
                if (!sf.base.Browser.userAgent.match(/ipad|ipod|iphone/i)) {
                    this.clipBoardTextArea.select();
                }
                else {
                    this.clipBoardTextArea.setSelectionRange(0, this.clipBoardTextArea.value.length);
                }
                this[isSelect] = true;
                this.copiedUniqueIdCollection = [];
                this.treeCopyContent = '';
            }
            else {
                _super.prototype.setCopyData.call(this, withHeader);
            }
        }
    };
    TreeClipboard.prototype.parentContentData = function (currentRecords, selectedIndex, rows, withHeader, index) {
        var getCopyData = 'getCopyData';
        var copyContent = 'copyContent';
        var parentItem = 'parentItem';
        var uniqueID = 'uniqueID';
        var level = 'level';
        if (!sf.base.isNullOrUndefined(currentRecords[selectedIndex][parentItem])) {
            var treeLevel = currentRecords[selectedIndex][parentItem][level];
            for (var i = 0; i < treeLevel + 1; i++) {
                for (var j = 0; j < currentRecords.length; j++) {
                    if (!sf.base.isNullOrUndefined(currentRecords[selectedIndex][parentItem]) &&
                        currentRecords[j][uniqueID] === currentRecords[selectedIndex][parentItem][uniqueID]) {
                        selectedIndex = j;
                        var cells = [].slice.call(rows[selectedIndex].querySelectorAll('.e-rowcell'));
                        var uniqueid = currentRecords[j][uniqueID];
                        if (this.copiedUniqueIdCollection.indexOf(uniqueid) === -1) {
                            this[getCopyData](cells, false, '\t', withHeader);
                            if (index > 0) {
                                this.treeCopyContent = this.treeCopyContent + this[copyContent] + '\n';
                            }
                            else {
                                this.treeCopyContent = this[copyContent] + '\n' + this.treeCopyContent;
                            }
                            this.copiedUniqueIdCollection.push(uniqueid);
                            this[copyContent] = '';
                            break;
                        }
                    }
                }
            }
        }
    };
    TreeClipboard.prototype.copy = function (withHeader) {
        _super.prototype.copy.call(this, withHeader);
    };
    TreeClipboard.prototype.paste = function (data, rowIndex, colIndex) {
        _super.prototype.paste.call(this, data, rowIndex, colIndex);
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns clipboard module name
     */
    TreeClipboard.prototype.getModuleName = function () {
        return 'clipboard';
    };
    /**
     * To destroy the clipboard
     *
     * @returns {void}
     * @hidden
     */
    TreeClipboard.prototype.destroy = function () {
        _super.prototype.destroy.call(this);
    };
    TreeClipboard.prototype.childContentData = function (currentRecords, selectedIndex, rows, withHeader) {
        var getCopyData = 'getCopyData';
        var copyContent = 'copyContent';
        var childRecords = 'childRecords';
        var hasChildRecords = 'hasChildRecords';
        var uniqueID = 'uniqueID';
        if (currentRecords[selectedIndex][hasChildRecords]) {
            var childData = currentRecords[selectedIndex][childRecords];
            for (var i = 0; i < childData.length; i++) {
                for (var j = 0; j < currentRecords.length; j++) {
                    if (!sf.base.isNullOrUndefined(childData[i][uniqueID]) && currentRecords[j][uniqueID] === childData[i][uniqueID]) {
                        if ((!sf.base.isNullOrUndefined(rows[j])) && !rows[j].classList.contains('e-summaryrow')) {
                            var cells = [].slice.call(rows[j].querySelectorAll('.e-rowcell'));
                            var uniqueid = currentRecords[j][uniqueID];
                            if (this.copiedUniqueIdCollection.indexOf(uniqueid) === -1) {
                                this[getCopyData](cells, false, '\t', withHeader);
                                this.treeCopyContent += ('\n' + this[copyContent]);
                                this[copyContent] = '';
                                this.copiedUniqueIdCollection.push(uniqueid);
                                this.childContentData(currentRecords, j, rows, withHeader);
                            }
                        }
                        break;
                    }
                }
            }
        }
    };
    return TreeClipboard;
}(sf.grids.Clipboard));

/**
 * @param {TreeGrid} parent - Tree Grid instance
 * @returns {boolean} - Specifies whether remote data binding
 */
function isRemoteData(parent) {
    if (parent.dataSource instanceof sf.data.DataManager) {
        var adaptor = parent.dataSource.adaptor;
        return (adaptor instanceof sf.data.ODataAdaptor ||
            (adaptor instanceof sf.data.WebApiAdaptor) || (adaptor instanceof sf.data.WebMethodAdaptor) ||
            (adaptor instanceof sf.data.CacheAdaptor) || adaptor instanceof sf.data.UrlAdaptor);
    }
    return false;
}
/**
 * @param {TreeGrid | IGrid} parent - Tree Grid or Grid instance
 * @returns {boolean} - Returns whether custom binding
 */
function isCountRequired(parent) {
    if (parent.dataSource && 'result' in parent.dataSource) {
        return true;
    }
    return false;
}
/**
 * @param {TreeGrid} parent - Tree Grid instance
 * @returns {boolean} - Returns whether checkbox column is enabled
 */
function isCheckboxcolumn(parent) {
    for (var i = 0; i < parent.columns.length; i++) {
        if (parent.columns[i].showCheckbox) {
            return true;
        }
    }
    return false;
}
/**
 * @param {TreeGrid} parent - Tree Grid instance
 * @returns {boolean} - Returns whether filtering and searching done
 */
function isFilterChildHierarchy(parent) {
    if ((!sf.base.isNullOrUndefined(parent.grid.searchSettings.key) && parent.grid.searchSettings.key !== '' &&
        (parent.searchSettings.hierarchyMode === 'Child' || parent.searchSettings.hierarchyMode === 'None')) ||
        (parent.allowFiltering && parent.grid.filterSettings.columns.length &&
            (parent.filterSettings.hierarchyMode === 'Child' || parent.filterSettings.hierarchyMode === 'None'))) {
        return true;
    }
    return false;
}
/**
 * @param {Object} records - Define records for which parent records has to be found
 * @hidden
 * @returns {Object} - Returns parent records collection
 */
function findParentRecords(records) {
    var datas = [];
    var recordsLength = Object.keys(records).length;
    for (var i = 0, len = recordsLength; i < len; i++) {
        var hasChild = sf.grids.getObject('hasChildRecords', records[i]);
        if (hasChild) {
            datas.push(records[i]);
        }
    }
    return datas;
}
/**
 * @param {TreeGrid} parent - Tree Grid instance
 * @returns {boolean} - Returns the expand status of record
 * @param {ITreeData} record - Define the record for which expand status has be found
 * @param {ITreeData[]} parents - Parent Data collection
 * @hidden
 */
function getExpandStatus(parent, record, parents) {
    var parentRecord = sf.base.isNullOrUndefined(record.parentItem) ? null :
        getParentData(parent, record.parentItem.uniqueID);
    var childParent;
    if (parentRecord != null) {
        if (parent.initialRender && !sf.base.isNullOrUndefined(parentRecord[parent.expandStateMapping])
            && !parentRecord[parent.expandStateMapping]) {
            parentRecord.expanded = false;
            return false;
        }
        else if (parentRecord.expanded === false) {
            return false;
        }
        else if (parentRecord.parentItem) {
            childParent = getParentData(parent, parentRecord.parentItem.uniqueID);
            if (childParent && parent.initialRender && !sf.base.isNullOrUndefined(childParent[parent.expandStateMapping])
                && !childParent[parent.expandStateMapping]) {
                childParent.expanded = false;
                return false;
            }
            if (childParent && childParent.expanded === false) {
                return false;
            }
            else if (childParent) {
                return getExpandStatus(parent, childParent, parents);
            }
            return true;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}
/**
 * @param {ITreeData} records - Define the record for which child records has to be found
 * @returns {Object[]} - Returns child records collection
 * @hidden
 */
function findChildrenRecords(records) {
    var datas = [];
    if (sf.base.isNullOrUndefined(records) || (!records.hasChildRecords && !sf.base.isNullOrUndefined(records.childRecords)
        && !records.childRecords.length)) {
        return [];
    }
    if (!sf.base.isNullOrUndefined(records.childRecords)) {
        var childRecords = records.childRecords;
        for (var i = 0, len = Object.keys(childRecords).length; i < len; i++) {
            datas.push(childRecords[i]);
            if (childRecords[i].hasChildRecords || (!sf.base.isNullOrUndefined(childRecords[i].childRecords) &&
                childRecords[i].childRecords.length)) {
                datas = datas.concat(findChildrenRecords(childRecords[i]));
            }
        }
    }
    return datas;
}
/**
 * @param {TreeGrid} parent - Tree Grid instance
 * @returns {boolean} - Returns whether local data binding
 */
function isOffline(parent) {
    if (isRemoteData(parent)) {
        var dm = parent.dataSource;
        return !sf.base.isNullOrUndefined(dm.ready);
    }
    return true;
}
/**
 * @param {Object[]} array - Defines the array to be cloned
 * @returns {Object[]} - Returns cloned array collection
 */
function extendArray(array) {
    var objArr = [];
    var obj;
    var keys;
    for (var i = 0; array && i < array.length; i++) {
        keys = Object.keys(array[i]);
        obj = {};
        for (var j = 0; j < keys.length; j++) {
            obj[keys[j]] = array[i][keys[j]];
        }
        objArr.push(obj);
    }
    return objArr;
}
/**
 * @param {ITreeData} value - Defined the dirty data to be cleaned
 * @returns {ITreeData} - Returns cleaned original data
 */
function getPlainData(value) {
    delete value.hasChildRecords;
    delete value.childRecords;
    delete value.index;
    delete value.parentItem;
    delete value.level;
    delete value.taskData;
    delete value.uniqueID;
    return value;
}
/**
 * @param {TreeGrid} parent - TreeGrid instance
 * @param {string} value - IdMapping field name
 * @param {boolean} requireFilter - Specified whether treegrid data is filtered
 * @returns {ITreeData} - Returns IdMapping matched record
 */
function getParentData(parent, value, requireFilter) {
    if (requireFilter) {
        var idFilter = 'uniqueIDFilterCollection';
        return parent[idFilter][value];
    }
    else {
        var id = 'uniqueIDCollection';
        return parent[id][value];
    }
}
/**
 * @param {HTMLTableRowElement} el - Row element
 * @returns {boolean} - Returns whether hidden
 */
function isHidden(el) {
    var style = window.getComputedStyle(el);
    return ((style.display === 'none') || (style.visibility === 'hidden'));
}

/**
 * TreeGrid Selection module
 *
 * @hidden
 */
var Selection = /** @class */ (function () {
    /**
     * Constructor for Selection module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Selection(parent) {
        this.parent = parent;
        this.selectedItems = [];
        this.selectedIndexes = [];
        this.filteredList = [];
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Selection module name
     */
    Selection.prototype.getModuleName = function () {
        return 'selection';
    };
    Selection.prototype.addEventListener = function () {
        this.parent.on('dataBoundArg', this.headerCheckbox, this);
        this.parent.on('columnCheckbox', this.columnCheckbox, this);
        this.parent.on('updateGridActions', this.updateGridActions, this);
        this.parent.grid.on('colgroup-refresh', this.headerCheckbox, this);
        this.parent.on('checkboxSelection', this.checkboxSelection, this);
    };
    Selection.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('dataBoundArg', this.headerCheckbox);
        this.parent.off('columnCheckbox', this.columnCheckbox);
        this.parent.grid.off('colgroup-refresh', this.headerCheckbox);
        this.parent.off('checkboxSelection', this.checkboxSelection);
        this.parent.off('updateGridActions', this.updateGridActions);
    };
    /**
     * To destroy the Selection
     *
     * @returns {void}
     * @hidden
     */
    Selection.prototype.destroy = function () {
        this.removeEventListener();
    };
    Selection.prototype.checkboxSelection = function (args) {
        var target = sf.grids.getObject('target', args);
        var checkWrap = sf.grids.parentsUntil(target, 'e-checkbox-wrapper');
        var checkBox;
        if (checkWrap && checkWrap.querySelectorAll('.e-treecheckselect').length > 0) {
            checkBox = checkWrap.querySelector('input[type="checkbox"]');
            var rowIndex = [];
            rowIndex.push(target.closest('tr').rowIndex);
            this.selectCheckboxes(rowIndex);
            this.triggerChkChangeEvent(checkBox, checkBox.nextElementSibling.classList.contains('e-check'), target.closest('tr'));
        }
        else if (checkWrap && checkWrap.querySelectorAll('.e-treeselectall').length > 0 && this.parent.autoCheckHierarchy) {
            var checkBoxvalue = !checkWrap.querySelector('.e-frame').classList.contains('e-check')
                && !checkWrap.querySelector('.e-frame').classList.contains('e-stop');
            this.headerSelection(checkBoxvalue);
            checkBox = checkWrap.querySelector('input[type="checkbox"]');
            this.triggerChkChangeEvent(checkBox, checkBoxvalue, target.closest('tr'));
        }
    };
    Selection.prototype.triggerChkChangeEvent = function (checkBox, checkState, rowElement) {
        var data = this.parent.getCurrentViewRecords()[rowElement.rowIndex];
        var args = { checked: checkState, target: checkBox, rowElement: rowElement,
            rowData: checkBox.classList.contains('e-treeselectall')
                ? this.parent.getCheckedRecords() : data };
        this.parent.trigger(checkboxChange, args);
    };
    Selection.prototype.getCheckboxcolumnIndex = function () {
        var mappingUid;
        var columnIndex;
        var columns = (this.parent.columns);
        for (var col = 0; col < columns.length; col++) {
            if (columns[col].showCheckbox) {
                mappingUid = this.parent.columns[col].uid;
            }
        }
        var headerCelllength = this.parent.getHeaderContent().querySelectorAll('.e-headercelldiv').length;
        for (var j = 0; j < headerCelllength; j++) {
            var headercell = this.parent.getHeaderContent().querySelectorAll('.e-headercelldiv')[j];
            if (headercell.getAttribute('e-mappinguid') === mappingUid) {
                columnIndex = j;
            }
        }
        return columnIndex;
    };
    Selection.prototype.headerCheckbox = function () {
        this.columnIndex = this.getCheckboxcolumnIndex();
        if (this.columnIndex > -1 && this.parent.getHeaderContent().querySelectorAll('.e-treeselectall').length === 0) {
            var headerElement = this.parent.getHeaderContent().querySelectorAll('.e-headercelldiv')[this.columnIndex];
            var value = false;
            var rowChkBox = this.parent.createElement('input', { className: 'e-treeselectall', attrs: { 'type': 'checkbox' } });
            var checkWrap = sf.buttons.createCheckBox(this.parent.createElement, false, { checked: value, label: ' ' });
            checkWrap.classList.add('e-hierarchycheckbox');
            checkWrap.insertBefore(rowChkBox.cloneNode(), checkWrap.firstChild);
            if (!sf.base.isNullOrUndefined(headerElement)) {
                headerElement.insertBefore(checkWrap, headerElement.firstChild);
            }
            if (this.parent.autoCheckHierarchy) {
                this.headerSelection();
            }
        }
        else if (this.columnIndex > -1 && this.parent.getHeaderContent().querySelectorAll('.e-treeselectall').length > 0) {
            var checkWrap = this.parent.getHeaderContent().querySelectorAll('.e-checkbox-wrapper')[0];
            var checkBoxvalue = checkWrap.querySelector('.e-frame').classList.contains('e-check');
            if (this.parent.autoCheckHierarchy && checkBoxvalue) {
                this.headerSelection(checkBoxvalue);
            }
        }
    };
    Selection.prototype.renderColumnCheckbox = function (args) {
        var rowChkBox = this.parent.createElement('input', { className: 'e-treecheckselect', attrs: { 'type': 'checkbox' } });
        var data = args.data;
        args.cell.classList.add('e-treegridcheckbox');
        args.cell.setAttribute('aria-label', 'checkbox');
        var value = (sf.base.isNullOrUndefined(data.checkboxState) || data.checkboxState === 'uncheck') ? false : true;
        var checkWrap = sf.buttons.createCheckBox(this.parent.createElement, false, { checked: value, label: ' ' });
        checkWrap.classList.add('e-hierarchycheckbox');
        if (this.parent.allowTextWrap) {
            checkWrap.querySelector('.e-frame').style.width = '18px';
        }
        if (data.checkboxState === 'indeterminate') {
            var checkbox = checkWrap.querySelectorAll('.e-frame')[0];
            sf.base.removeClass([checkbox], ['e-check', 'e-stop', 'e-uncheck']);
            checkWrap.querySelector('.e-frame').classList.add('e-stop');
        }
        checkWrap.insertBefore(rowChkBox.cloneNode(), checkWrap.firstChild);
        return checkWrap;
    };
    Selection.prototype.columnCheckbox = function (container) {
        var checkWrap = this.renderColumnCheckbox(container);
        var containerELe = container.cell.querySelector('.e-treecolumn-container');
        if (!sf.base.isNullOrUndefined(containerELe)) {
            if (!container.cell.querySelector('.e-hierarchycheckbox')) {
                containerELe.insertBefore(checkWrap, containerELe.querySelectorAll('.e-treecell')[0]);
            }
        }
        else {
            var spanEle = this.parent.createElement('span', { className: 'e-treecheckbox' });
            var data = container.cell.innerHTML;
            container.cell.innerHTML = '';
            spanEle.innerHTML = data;
            var divEle = this.parent.createElement('div', { className: 'e-treecheckbox-container' });
            divEle.appendChild(checkWrap);
            divEle.appendChild(spanEle);
            container.cell.appendChild(divEle);
        }
    };
    Selection.prototype.selectCheckboxes = function (rowIndexes) {
        for (var i = 0; i < rowIndexes.length; i++) {
            var record = this.parent.getCurrentViewRecords()[rowIndexes[i]];
            var flatRecord = getParentData(this.parent, record.uniqueID);
            record = flatRecord;
            var checkboxState = (record.checkboxState === 'uncheck') ? 'check' : 'uncheck';
            record.checkboxState = checkboxState;
            var keys = Object.keys(record);
            for (var j = 0; j < keys.length; j++) {
                if (Object.prototype.hasOwnProperty.call(flatRecord, keys[j])) {
                    flatRecord[keys[j]] = record[keys[j]];
                }
            }
            this.traverSelection(record, checkboxState, false);
            if (this.parent.autoCheckHierarchy) {
                this.headerSelection();
            }
        }
    };
    Selection.prototype.traverSelection = function (record, checkboxState, ischildItem) {
        var length = 0;
        this.updateSelectedItems(record, checkboxState);
        if (!ischildItem && record.parentItem && this.parent.autoCheckHierarchy) {
            this.updateParentSelection(record.parentItem);
        }
        if (record.childRecords && this.parent.autoCheckHierarchy) {
            var childRecords = record.childRecords;
            if (!sf.base.isNullOrUndefined(this.parent.filterModule) &&
                this.parent.filterModule.filteredResult.length > 0 && this.parent.autoCheckHierarchy) {
                childRecords = this.getFilteredChildRecords(childRecords);
            }
            length = childRecords.length;
            for (var count = 0; count < length; count++) {
                if (!childRecords[count].isSummaryRow) {
                    if (childRecords[count].hasChildRecords) {
                        this.traverSelection(childRecords[count], checkboxState, true);
                    }
                    else {
                        this.updateSelectedItems(childRecords[count], checkboxState);
                    }
                }
            }
        }
    };
    Selection.prototype.getFilteredChildRecords = function (childRecords) {
        var _this = this;
        var filteredChildRecords = childRecords.filter(function (e) {
            return _this.parent.filterModule.filteredResult.indexOf(e) > -1;
        });
        return filteredChildRecords;
    };
    Selection.prototype.updateParentSelection = function (parentRecord) {
        var length = 0;
        var childRecords = [];
        var record = getParentData(this.parent, parentRecord.uniqueID);
        if (record && record.childRecords) {
            childRecords = record.childRecords;
        }
        if (!sf.base.isNullOrUndefined(this.parent.filterModule) &&
            this.parent.filterModule.filteredResult.length > 0 && this.parent.autoCheckHierarchy) {
            childRecords = this.getFilteredChildRecords(childRecords);
        }
        length = childRecords && childRecords.length;
        var indeter = 0;
        var checkChildRecords = 0;
        if (!sf.base.isNullOrUndefined(record)) {
            for (var i = 0; i < childRecords.length; i++) {
                var currentRecord = getParentData(this.parent, childRecords[i].uniqueID);
                var checkBoxRecord = currentRecord;
                if (!sf.base.isNullOrUndefined(checkBoxRecord)) {
                    if (checkBoxRecord.checkboxState === 'indeterminate') {
                        indeter++;
                    }
                    else if (checkBoxRecord.checkboxState === 'check') {
                        checkChildRecords++;
                    }
                }
            }
            if (indeter > 0 || (checkChildRecords > 0 && checkChildRecords !== length)) {
                record.checkboxState = 'indeterminate';
            }
            else if (checkChildRecords === 0 && indeter === 0) {
                record.checkboxState = 'uncheck';
            }
            else {
                record.checkboxState = 'check';
            }
            this.updateSelectedItems(record, record.checkboxState);
            if (record.parentItem) {
                this.updateParentSelection(record.parentItem);
            }
        }
    };
    Selection.prototype.headerSelection = function (checkAll) {
        var _this = this;
        var index = -1;
        var length = 0;
        if (!sf.base.isNullOrUndefined(this.parent.filterModule) && this.parent.filterModule.filteredResult.length > 0) {
            var filterResult = this.parent.filterModule.filteredResult;
            if (this.filteredList.length == 0) {
                this.filteredList = filterResult;
            }
            else {
                if (this.filteredList != filterResult) {
                    this.filteredList = filterResult;
                }
            }
        }
        if (this.filteredList.length > 0) {
            if (!this.parent.filterSettings.columns.length && this.filteredList.length) {
                this.filteredList = [];
            }
        }
        var data = (!sf.base.isNullOrUndefined(this.parent.filterModule) &&
            (this.filteredList.length > 0)) ? this.filteredList :
            this.parent.flatData;
        data = isRemoteData(this.parent) ? this.parent.getCurrentViewRecords() : data;
        if (!sf.base.isNullOrUndefined(checkAll)) {
            for (var i = 0; i < data.length; i++) {
                if (checkAll) {
                    if (data[i].checkboxState === 'check') {
                        continue;
                    }
                    data[i].checkboxState = 'check';
                    this.updateSelectedItems(data[i], data[i].checkboxState);
                }
                else {
                    index = this.selectedItems.indexOf(data[i]);
                    if (index > -1) {
                        data[i].checkboxState = 'uncheck';
                        this.updateSelectedItems(data[i], data[i].checkboxState);
                        if (this.parent.autoCheckHierarchy) {
                            this.updateParentSelection(data[i]);
                        }
                    }
                }
            }
        }
        if (checkAll === false && this.parent.enableVirtualization) {
            this.selectedItems = [];
            this.selectedIndexes = [];
            data.filter(function (rec) {
                rec.checkboxState = 'uncheck';
                _this.updateSelectedItems(rec, rec.checkboxState);
            });
        }
        length = this.selectedItems.length;
        var checkbox = this.parent.getHeaderContent().querySelectorAll('.e-frame')[0];
        if (length > 0 && data.length > 0) {
            if (length !== data.length && !checkAll) {
                sf.base.removeClass([checkbox], ['e-check']);
                checkbox.classList.add('e-stop');
            }
            else {
                sf.base.removeClass([checkbox], ['e-stop']);
                checkbox.classList.add('e-check');
            }
        }
        else {
            sf.base.removeClass([checkbox], ['e-check', 'e-stop']);
        }
    };
    Selection.prototype.updateSelectedItems = function (currentRecord, checkState) {
        var record = this.parent.getCurrentViewRecords().filter(function (e) {
            return e.uniqueID === currentRecord.uniqueID;
        });
        var checkedRecord;
        var recordIndex = this.parent.getCurrentViewRecords().indexOf(record[0]);
        var checkboxRecord = getParentData(this.parent, currentRecord.uniqueID);
        var checkbox;
        if (recordIndex > -1) {
            var tr = this.parent.getRows()[recordIndex];
            var movableTr = void 0;
            if (this.parent.frozenRows || this.parent.getFrozenColumns()) {
                movableTr = this.parent.getMovableDataRows()[recordIndex];
            }
            checkbox = tr.querySelectorAll('.e-frame')[0] ? tr.querySelectorAll('.e-frame')[0]
                : movableTr.querySelectorAll('.e-frame')[0];
            if (!sf.base.isNullOrUndefined(checkbox)) {
                sf.base.removeClass([checkbox], ['e-check', 'e-stop', 'e-uncheck']);
            }
        }
        checkedRecord = checkboxRecord;
        if (sf.base.isNullOrUndefined(checkedRecord)) {
            checkedRecord = currentRecord;
        }
        checkedRecord.checkboxState = checkState;
        if (checkState === 'check' && sf.base.isNullOrUndefined(currentRecord.isSummaryRow)) {
            if (recordIndex !== -1 && this.selectedIndexes.indexOf(recordIndex) === -1) {
                this.selectedIndexes.push(recordIndex);
            }
            if (this.selectedItems.indexOf(checkedRecord) === -1 && (recordIndex !== -1 &&
                (!sf.base.isNullOrUndefined(this.parent.filterModule) && this.parent.filterModule.filteredResult.length > 0))) {
                this.selectedItems.push(checkedRecord);
            }
            if (this.selectedItems.indexOf(checkedRecord) === -1 && (!sf.base.isNullOrUndefined(this.parent.filterModule) &&
                this.parent.filterModule.filteredResult.length === 0)) {
                this.selectedItems.push(checkedRecord);
            }
            if (this.selectedItems.indexOf(checkedRecord) === -1 && sf.base.isNullOrUndefined(this.parent.filterModule)) {
                this.selectedItems.push(checkedRecord);
            }
        }
        else if ((checkState === 'uncheck' || checkState === 'indeterminate') && sf.base.isNullOrUndefined(currentRecord.isSummaryRow)) {
            var index = this.selectedItems.indexOf(checkedRecord);
            if (index !== -1) {
                this.selectedItems.splice(index, 1);
            }
            if (this.selectedIndexes.indexOf(recordIndex) !== -1) {
                var checkedIndex = this.selectedIndexes.indexOf(recordIndex);
                this.selectedIndexes.splice(checkedIndex, 1);
            }
        }
        var checkBoxclass = checkState === 'indeterminate' ? 'e-stop' : 'e-' + checkState;
        if (recordIndex > -1) {
            if (!sf.base.isNullOrUndefined(checkbox)) {
                checkbox.classList.add(checkBoxclass);
            }
        }
    };
    Selection.prototype.updateGridActions = function (args) {
        var _this = this;
        var requestType = args.requestType;
        var childData;
        var childLength;
        if (isCheckboxcolumn(this.parent)) {
            if (this.parent.autoCheckHierarchy) {
                if ((requestType === 'sorting' || requestType === 'paging')) {
                    var rows = this.parent.grid.getRows();
                    childData = this.parent.getCurrentViewRecords();
                    childLength = childData.length;
                    this.selectedIndexes = [];
                    for (var i = 0; i < childLength; i++) {
                        if (!rows[i].classList.contains('e-summaryrow')) {
                            this.updateSelectedItems(childData[i], childData[i].checkboxState);
                        }
                    }
                }
                else if (requestType === 'delete' || args.action === 'add') {
                    var updatedData = [];
                    if (requestType === 'delete') {
                        updatedData = args.data;
                    }
                    else {
                        updatedData.push(args.data);
                    }
                    for (var i = 0; i < updatedData.length; i++) {
                        if (requestType === 'delete') {
                            var index = this.parent.flatData.indexOf(updatedData[i]);
                            var checkedIndex = this.selectedIndexes.indexOf(index);
                            this.selectedIndexes.splice(checkedIndex, 1);
                            this.updateSelectedItems(updatedData[i], 'uncheck');
                        }
                        if (!sf.base.isNullOrUndefined(updatedData[i].parentItem)) {
                            this.updateParentSelection(updatedData[i].parentItem);
                        }
                    }
                }
                else if (args.requestType === 'add' && this.parent.autoCheckHierarchy) {
                    args.data.checkboxState = 'uncheck';
                }
                else if (requestType === 'filtering' || requestType === 'searching' || requestType === 'refresh'
                    && !isRemoteData(this.parent)) {
                    this.selectedItems = [];
                    this.selectedIndexes = [];
                    childData = (!sf.base.isNullOrUndefined(this.parent.filterModule) && this.parent.filterModule.filteredResult.length > 0) ?
                        this.parent.getCurrentViewRecords() : this.parent.flatData;
                    childData.forEach(function (record) {
                        if (record.hasChildRecords) {
                            _this.updateParentSelection(record);
                        }
                        else {
                            _this.updateSelectedItems(record, record.checkboxState);
                        }
                    });
                    this.headerSelection();
                }
            }
        }
    };
    Selection.prototype.getCheckedrecords = function () {
        return this.selectedItems;
    };
    Selection.prototype.getCheckedRowIndexes = function () {
        return this.selectedIndexes;
    };
    return Selection;
}());

/**
 * TreeGrid Print module
 *
 * @hidden
 */
var Print$1 = /** @class */ (function () {
    /**
     * Constructor for Print module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Print$$1(parent) {
        this.parent = parent;
        sf.grids.Grid.Inject(sf.grids.Print);
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Print module name
     */
    Print$$1.prototype.getModuleName = function () {
        return 'print';
    };
    /**
     * @hidden
     * @returns {void}
     */
    Print$$1.prototype.addEventListener = function () {
        this.parent.grid.on(printGridInit, this.printTreeGrid, this);
    };
    Print$$1.prototype.removeEventListener = function () {
        this.parent.grid.off(printGridInit, this.printTreeGrid);
    };
    Print$$1.prototype.printTreeGrid = function (printGrid) {
        var grid = sf.grids.getObject('printgrid', printGrid);
        var gridElement = sf.grids.getObject('element', printGrid);
        grid.addEventListener(queryCellInfo, this.parent.grid.queryCellInfo);
        grid.addEventListener(rowDataBound, this.parent.grid.rowDataBound);
        grid.addEventListener(beforeDataBound, this.parent.grid.beforeDataBound);
        sf.base.addClass([gridElement], 'e-treegrid');
    };
    Print$$1.prototype.print = function () {
        this.parent.grid.print();
    };
    /**
     * To destroy the Print
     *
     * @returns {void}
     * @hidden
     */
    Print$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    return Print$$1;
}());

var __extends$5 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$3 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the filtering behavior of the TreeGrid.
 */
var SearchSettings = /** @class */ (function (_super) {
    __extends$5(SearchSettings, _super);
    function SearchSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$3([
        sf.base.Property()
    ], SearchSettings.prototype, "fields", void 0);
    __decorate$3([
        sf.base.Property(false)
    ], SearchSettings.prototype, "ignoreCase", void 0);
    __decorate$3([
        sf.base.Property('contains')
    ], SearchSettings.prototype, "operator", void 0);
    __decorate$3([
        sf.base.Property()
    ], SearchSettings.prototype, "key", void 0);
    __decorate$3([
        sf.base.Property()
    ], SearchSettings.prototype, "hierarchyMode", void 0);
    return SearchSettings;
}(sf.base.ChildProperty));

var __extends$6 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$4 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the selection behavior of the TreeGrid.
 */
var SelectionSettings = /** @class */ (function (_super) {
    __extends$6(SelectionSettings, _super);
    function SelectionSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$4([
        sf.base.Property('Row')
    ], SelectionSettings.prototype, "mode", void 0);
    __decorate$4([
        sf.base.Property('Flow')
    ], SelectionSettings.prototype, "cellSelectionMode", void 0);
    __decorate$4([
        sf.base.Property('Single')
    ], SelectionSettings.prototype, "type", void 0);
    __decorate$4([
        sf.base.Property(false)
    ], SelectionSettings.prototype, "persistSelection", void 0);
    __decorate$4([
        sf.base.Property('Default')
    ], SelectionSettings.prototype, "checkboxMode", void 0);
    __decorate$4([
        sf.base.Property(false)
    ], SelectionSettings.prototype, "checkboxOnly", void 0);
    __decorate$4([
        sf.base.Property(true)
    ], SelectionSettings.prototype, "enableToggle", void 0);
    return SelectionSettings;
}(sf.base.ChildProperty));

/**
 * TreeGrid render module
 *
 * @hidden
 */
var Render = /** @class */ (function () {
    /**
     * Constructor for render module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Render(parent) {
        this.parent = parent;
        this.templateResult = null;
        this.parent.grid.on('template-result', this.columnTemplateResult, this);
        this.parent.grid.on('reactTemplateRender', this.reactTemplateRender, this);
    }
    /**
     * Updated row elements for TreeGrid
     *
     * @param {RowDataBoundEventArgs} args - Row details before its bound to DOM
     * @returns {void}
     */
    Render.prototype.RowModifier = function (args) {
        if (!args.data) {
            return;
        }
        var data = args.data;
        var parentData = data.parentItem;
        if (!sf.base.isNullOrUndefined(data.parentItem) && !isFilterChildHierarchy(this.parent) &&
            (!(this.parent.allowPaging && !(this.parent.pageSettings.pageSizeMode === 'Root')) ||
                (isRemoteData(this.parent) && !isOffline(this.parent)))) {
            var collapsed$$1 = (this.parent.initialRender && (!(sf.base.isNullOrUndefined(parentData[this.parent.expandStateMapping]) ||
                parentData[this.parent.expandStateMapping]) || this.parent.enableCollapseAll)) ||
                !getExpandStatus(this.parent, args.data, this.parent.grid.getCurrentViewRecords());
            if (collapsed$$1) {
                args.row.style.display = 'none';
            }
        }
        if (isRemoteData(this.parent) && !isOffline(this.parent)) {
            var proxy_1 = this.parent;
            var parentrec = this.parent.getCurrentViewRecords().filter(function (rec) {
                return sf.base.getValue(proxy_1.idMapping, rec) === sf.base.getValue(proxy_1.parentIdMapping, data);
            });
            if (parentrec.length > 0) {
                var display = parentrec[0].expanded ? 'table-row' : 'none';
                args.row.setAttribute('style', 'display: ' + display + ';');
            }
        }
        //addClass([args.row], 'e-gridrowindex' + index + 'level' + (<ITreeData>args.data).level);
        var summaryRow = sf.grids.getObject('isSummaryRow', args.data);
        if (summaryRow) {
            sf.base.addClass([args.row], 'e-summaryrow');
        }
        if (args.row.querySelector('.e-treegridexpand')) {
            args.row.setAttribute('aria-expanded', 'true');
        }
        else if (args.row.querySelector('.e-treegridcollapse')) {
            args.row.setAttribute('aria-expanded', 'false');
        }
        if (this.parent.enableCollapseAll && this.parent.initialRender) {
            if (!sf.base.isNullOrUndefined(data.parentItem)) {
                args.row.style.display = 'none';
            }
        }
        this.parent.trigger(rowDataBound, args);
    };
    /**
     * cell renderer for tree column index cell
     *
     * @param {QueryCellInfoEventArgs} args - Cell detail before its bound to DOM
     * @returns {void}
     */
    Render.prototype.cellRender = function (args) {
        if (!args.data) {
            return;
        }
        var grid = this.parent.grid;
        var data = args.data;
        var index;
        var ispadfilter = sf.base.isNullOrUndefined(data.filterLevel);
        var pad = ispadfilter ? data.level : data.filterLevel;
        var totalIconsWidth = 0;
        var cellElement;
        var column = this.parent.getColumnByUid(args.column.uid);
        var summaryRow = data.isSummaryRow;
        var frozenColumns = this.parent.getFrozenColumns();
        if (!sf.base.isNullOrUndefined(data.parentItem)) {
            index = data.parentItem.index;
        }
        else {
            index = data.index;
        }
        if (grid.getColumnIndexByUid(args.column.uid) === this.parent.treeColumnIndex && (args.requestType === 'add' || args.requestType
            === 'rowDragAndDrop' || args.requestType === 'delete' || sf.base.isNullOrUndefined(args.cell.querySelector('.e-treecell')))) {
            var container = sf.base.createElement('div', { className: 'e-treecolumn-container' });
            var emptyExpandIcon = sf.base.createElement('span', {
                className: 'e-icons e-none',
                styles: 'width: 10px; display: inline-block'
            });
            for (var n = 0; n < pad; n++) {
                totalIconsWidth += 10;
                container.appendChild(emptyExpandIcon.cloneNode());
            }
            var iconRequired = !sf.base.isNullOrUndefined(data.hasFilteredChildRecords)
                ? data.hasFilteredChildRecords : data.hasChildRecords;
            if (iconRequired && !sf.base.isNullOrUndefined(data.childRecords)) {
                iconRequired = !(data.childRecords.length === 0);
            }
            if (iconRequired) {
                sf.base.addClass([args.cell], 'e-treerowcell');
                var expandIcon = sf.base.createElement('span', { className: 'e-icons' });
                var expand = void 0;
                if (this.parent.initialRender) {
                    expand = data.expanded &&
                        (sf.base.isNullOrUndefined(data[this.parent.expandStateMapping]) || data[this.parent.expandStateMapping]) &&
                        !this.parent.enableCollapseAll;
                }
                else {
                    expand = !(!data.expanded || !getExpandStatus(this.parent, data, this.parent.grid.getCurrentViewRecords()));
                }
                sf.base.addClass([expandIcon], (expand) ? 'e-treegridexpand' : 'e-treegridcollapse');
                totalIconsWidth += 18;
                container.appendChild(expandIcon);
                emptyExpandIcon.style.width = '7px';
                totalIconsWidth += 7;
                container.appendChild(emptyExpandIcon.cloneNode());
            }
            else if (pad || !pad && !data.level) {
                // icons width
                totalIconsWidth += 20;
                container.appendChild(emptyExpandIcon.cloneNode());
                container.appendChild(emptyExpandIcon.cloneNode());
            }
            //should add below code when paging funcitonality implemented
            // if (data.hasChildRecords) {
            //     addClass([expandIcon], data.expanded ? 'e-treegridexpand' : 'e-treegridcollapse');
            // }
            cellElement = sf.base.createElement('span', { className: 'e-treecell' });
            if (this.parent.allowTextWrap) {
                cellElement.style.width = 'Calc(100% - ' + totalIconsWidth + 'px)';
            }
            sf.base.addClass([args.cell], 'e-gridrowindex' + index + 'level' + data.level);
            this.updateTreeCell(args, cellElement);
            container.appendChild(cellElement);
            args.cell.appendChild(container);
        }
        else if (this.templateResult) {
            this.templateResult = null;
        }
        var freeze = (grid.getFrozenLeftColumnsCount() > 0 || grid.getFrozenRightColumnsCount() > 0) ? true : false;
        if (!freeze) {
            if (frozenColumns > this.parent.treeColumnIndex && frozenColumns > 0 &&
                grid.getColumnIndexByUid(args.column.uid) === frozenColumns) {
                sf.base.addClass([args.cell], 'e-gridrowindex' + index + 'level' + data.level);
            }
            else if (frozenColumns < this.parent.treeColumnIndex && frozenColumns > 0 &&
                (grid.getColumnIndexByUid(args.column.uid) === frozenColumns
                    || grid.getColumnIndexByUid(args.column.uid) === frozenColumns - 1)) {
                sf.base.addClass([args.cell], 'e-gridrowindex' + index + 'level' + data.level);
            }
            else if (frozenColumns === this.parent.treeColumnIndex && frozenColumns > 0 &&
                grid.getColumnIndexByUid(args.column.uid) === this.parent.treeColumnIndex - 1) {
                sf.base.addClass([args.cell], 'e-gridrowindex' + index + 'level' + data.level);
            }
        }
        else {
            var freezerightColumns = grid.getFrozenRightColumns();
            var freezeLeftColumns = grid.getFrozenLeftColumns();
            var movableColumns = grid.getMovableColumns();
            if ((freezerightColumns.length > 0) && freezerightColumns[0].field === args.column.field) {
                sf.base.addClass([args.cell], 'e-gridrowindex' + index + 'level' + data.level);
            }
            else if ((freezeLeftColumns.length > 0) && freezeLeftColumns[0].field === args.column.field) {
                sf.base.addClass([args.cell], 'e-gridrowindex' + index + 'level' + data.level);
            }
            else if ((movableColumns.length > 0) && movableColumns[0].field === args.column.field) {
                sf.base.addClass([args.cell], 'e-gridrowindex' + index + 'level' + data.level);
            }
        }
        if (!sf.base.isNullOrUndefined(column) && column.showCheckbox) {
            this.parent.notify('columnCheckbox', args);
            if (this.parent.allowTextWrap) {
                var checkboxElement = args.cell.querySelectorAll('.e-frame')[0];
                var width = parseInt(checkboxElement.style.width, 16);
                totalIconsWidth += width;
                totalIconsWidth += 10;
                if (grid.getColumnIndexByUid(args.column.uid) === this.parent.treeColumnIndex) {
                    cellElement = args.cell.querySelector('.e-treecell');
                }
                else {
                    cellElement = args.cell.querySelector('.e-treecheckbox');
                }
                cellElement.style.width = 'Calc(100% - ' + totalIconsWidth + 'px)';
            }
        }
        if (summaryRow) {
            sf.base.addClass([args.cell], 'e-summarycell');
            var summaryData = sf.grids.getObject(args.column.field, args.data);
            if (args.cell.querySelector('.e-treecell') != null) {
                args.cell.querySelector('.e-treecell').innerHTML = summaryData;
            }
            else {
                args.cell.innerHTML = summaryData;
            }
        }
        if (sf.base.isNullOrUndefined(this.parent.rowTemplate)) {
            this.parent.trigger(queryCellInfo, args);
        }
    };
    Render.prototype.updateTreeCell = function (args, cellElement) {
        var columnModel = sf.base.getValue('columnModel', this.parent);
        var treeColumn = columnModel[this.parent.treeColumnIndex];
        var templateFn = 'templateFn';
        var colindex = args.column.index;
        if (sf.base.isNullOrUndefined(treeColumn.field)) {
            args.cell.setAttribute('aria-colindex', colindex + '');
        }
        if (treeColumn.field === args.column.field && !sf.base.isNullOrUndefined(treeColumn.template)) {
            args.column.template = treeColumn.template;
            args.column[templateFn] = sf.grids.templateCompiler(args.column.template);
            args.cell.classList.add('e-templatecell');
        }
        var textContent = args.cell.querySelector('.e-treecell') != null ?
            args.cell.querySelector('.e-treecell').innerHTML : args.cell.innerHTML;
        if (typeof (args.column.template) === 'object' && this.templateResult) {
            sf.grids.appendChildren(cellElement, this.templateResult);
            this.templateResult = null;
            args.cell.innerHTML = '';
        }
        else if (args.cell.classList.contains('e-templatecell')) {
            var len = args.cell.children.length;
            var tempID = this.parent.element.id + args.column.uid;
            if (treeColumn.field === args.column.field && !sf.base.isNullOrUndefined(treeColumn.template)) {
                var portals = 'portals';
                var renderReactTemplates = 'renderReactTemplates';
                if (this.parent.isReact && typeof (args.column.template) !== 'string') {
                    args.column[templateFn](args.data, this.parent, 'template', tempID, null, null, cellElement);
                    if (sf.base.isNullOrUndefined(this.parent.grid[portals])) {
                        this.parent.grid[portals] = this.parent[portals];
                    }
                    this.parent.notify('renderReactTemplate', this.parent[portals]);
                    this.parent[renderReactTemplates]();
                }
                else {
                    var str = 'isStringTemplate';
                    var result = args.column[templateFn](sf.grids.extend({ 'index': '' }, args.data), this.parent, 'template', tempID, this.parent[str]);
                    sf.grids.appendChildren(cellElement, result);
                }
                delete args.column.template;
                delete args.column[templateFn];
                args.cell.innerHTML = '';
            }
            else {
                for (var i = 0; i < len; len = args.cell.children.length) {
                    cellElement.appendChild(args.cell.children[i]);
                }
            }
        }
        else {
            cellElement.innerHTML = textContent;
            args.cell.innerHTML = '';
        }
    };
    Render.prototype.columnTemplateResult = function (args) {
        this.templateResult = args.template;
    };
    Render.prototype.reactTemplateRender = function (args) {
        var renderReactTemplates = 'renderReactTemplates';
        var portals = 'portals';
        this.parent[portals] = args;
        this.parent.notify('renderReactTemplate', this.parent[portals]);
        this.parent[renderReactTemplates]();
    };
    Render.prototype.destroy = function () {
        this.parent.grid.off('template-result', this.columnTemplateResult);
        this.parent.grid.off('reactTemplateRender', this.reactTemplateRender);
    };
    return Render;
}());

/**
 * Internal dataoperations for tree grid
 *
 * @hidden
 */
var DataManipulation = /** @class */ (function () {
    function DataManipulation(grid) {
        this.addedRecords = 'addedRecords';
        this.parent = grid;
        this.parentItems = [];
        this.taskIds = [];
        this.hierarchyData = [];
        this.storedIndex = -1;
        this.sortedData = [];
        this.isSortAction = false;
        this.addEventListener();
        this.dataResults = {};
        this.isSelfReference = !sf.base.isNullOrUndefined(this.parent.parentIdMapping);
    }
    /**
     * @hidden
     * @returns {void}
     */
    DataManipulation.prototype.addEventListener = function () {
        this.parent.on('updateRemoteLevel', this.updateParentRemoteData, this);
        this.parent.grid.on('sorting-begin', this.beginSorting, this);
        this.parent.on('updateAction', this.updateData, this);
        this.parent.on(remoteExpand, this.collectExpandingRecs, this);
        this.parent.on('dataProcessor', this.dataProcessor, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    DataManipulation.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off(remoteExpand, this.collectExpandingRecs);
        this.parent.off('updateRemoteLevel', this.updateParentRemoteData);
        this.parent.off('updateAction', this.updateData);
        this.parent.off('dataProcessor', this.dataProcessor);
        this.parent.grid.off('sorting-begin', this.beginSorting);
    };
    /**
     * To destroy the dataModule
     *
     * @returns {void}
     * @hidden
     */
    DataManipulation.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * @hidden
     * @returns {boolean} -Returns whether remote data binding
     */
    DataManipulation.prototype.isRemote = function () {
        if (!(this.parent.dataSource instanceof sf.data.DataManager)) {
            return false;
        }
        return true;
        // let gridData:  DataManager = <DataManager>this.parent.dataSource;
        // return gridData.dataSource.offline !== true && gridData.dataSource.url !== undefined;
    };
    /**
     * Function to manipulate datasource
     *
     * @param {Object} data - Provide tree grid datasource to convert to flat data
     * @hidden
     * @returns {void}
     */
    DataManipulation.prototype.convertToFlatData = function (data) {
        var _this = this;
        this.parent.flatData = (Object.keys(data).length === 0 && !(this.parent.dataSource instanceof sf.data.DataManager) ?
            this.parent.dataSource : []);
        this.parent.parentData = [];
        if ((isRemoteData(this.parent) && !isOffline(this.parent)) && data instanceof sf.data.DataManager && !(data instanceof Array)) {
            var dm = this.parent.dataSource;
            if (this.parent.parentIdMapping) {
                this.parent.query = sf.base.isNullOrUndefined(this.parent.query) ?
                    new sf.data.Query() : this.parent.query;
                if (this.parent.parentIdMapping) {
                    var filterKey = this.parent.query.params.filter(function (param) { return param.key === 'IdMapping'; });
                    if (this.parent.initialRender && !filterKey.length) {
                        this.parent.query.where(this.parent.parentIdMapping, 'equal', null);
                        this.parent.query.addParams('IdMapping', this.parent.idMapping);
                    }
                }
                if (!this.parent.hasChildMapping) {
                    var qry = this.parent.query.clone();
                    qry.queries = [];
                    qry = qry.select([this.parent.parentIdMapping]);
                    qry.isCountRequired = true;
                    dm.executeQuery(qry).then(function (e) {
                        _this.parentItems = sf.data.DataUtil.distinct(e.result, _this.parent.parentIdMapping, false);
                        var req = sf.grids.getObject('dataSource.requests', _this.parent).filter(function (e) {
                            return e.httpRequest.statusText !== 'OK';
                        }).length;
                        if (req === 0) {
                            sf.base.setValue('grid.contentModule.isLoaded', true, _this.parent);
                            if (!sf.base.isNullOrUndefined(_this.zerothLevelData)) {
                                sf.base.setValue('cancel', false, _this.zerothLevelData);
                                sf.base.getValue('grid.renderModule', _this.parent).dataManagerSuccess(_this.zerothLevelData);
                                _this.zerothLevelData = null;
                            }
                            _this.parent.grid.hideSpinner();
                        }
                    });
                }
            }
        }
        else if (data instanceof Array) {
            this.convertJSONData(data);
        }
    };
    DataManipulation.prototype.convertJSONData = function (data) {
        this.hierarchyData = [];
        this.taskIds = [];
        if (!this.parent.idMapping) {
            this.hierarchyData = data;
        }
        else {
            for (var i = 0; i < Object.keys(data).length; i++) {
                var tempData = data[i];
                this.hierarchyData.push(sf.base.extend({}, tempData));
                if (!sf.base.isNullOrUndefined(tempData[this.parent.idMapping])) {
                    this.taskIds.push(tempData[this.parent.idMapping]);
                }
            }
        }
        if (this.isSelfReference) {
            var selfData = [];
            var mappingData = new sf.data.DataManager(this.hierarchyData).executeLocal(new sf.data.Query()
                .group(this.parent.parentIdMapping));
            for (var i = 0; i < mappingData.length; i++) {
                var groupData = mappingData[i];
                var index = this.taskIds.indexOf(groupData.key);
                if (!sf.base.isNullOrUndefined(groupData.key)) {
                    if (index > -1) {
                        var childData = (groupData.items);
                        this.hierarchyData[index][this.parent.childMapping] = childData;
                        continue;
                    }
                }
                selfData.push.apply(selfData, groupData.items);
            }
            this.hierarchyData = this.selfReferenceUpdate(selfData);
        }
        if (!Object.keys(this.hierarchyData).length) {
            var isGantt = 'isGantt';
            var referenceData = !(this.parent.dataSource instanceof sf.data.DataManager) && this.parent[isGantt];
            this.parent.flatData = referenceData ? (this.parent.dataSource) : [];
        }
        else {
            this.createRecords(this.hierarchyData);
        }
        this.storedIndex = -1;
    };
    // private crudActions(): void {
    //   if (this.parent.dataSource instanceof DataManager && (this.parent.dataSource.adaptor instanceof RemoteSaveAdaptor)) {
    //     let oldUpdate: Function = this.parent.dataSource.adaptor.update;
    //     this.parent.dataSource.adaptor.update =
    //         function (dm: DataManager, keyField: string, value: Object, tableName?: string, query?: Query, original?: Object): Object {
    //                value = getPlainData(value);
    //                return oldUpdate.apply(this, [dm, keyField, value, tableName, query, original]);
    //              }
    //   }
    // }
    DataManipulation.prototype.selfReferenceUpdate = function (selfData) {
        var result = [];
        while (this.hierarchyData.length > 0 && selfData.length > 0) {
            var index = selfData.indexOf(this.hierarchyData[0]);
            if (index === -1) {
                this.hierarchyData.shift();
            }
            else {
                result.push(this.hierarchyData.shift());
                selfData.splice(index, 1);
            }
        }
        return result;
    };
    /**
     * Function to update the zeroth level parent records in remote binding
     *
     * @param {BeforeDataBoundArgs} args - contains data before its bounds to tree grid
     * @hidden
     * @returns {void}
     */
    DataManipulation.prototype.updateParentRemoteData = function (args) {
        var records = args.result;
        if (!this.parent.hasChildMapping && !this.parentItems.length &&
            (!this.parent.loadChildOnDemand)) {
            this.zerothLevelData = args;
            sf.base.setValue('cancel', true, args);
        }
        else {
            if (!this.parent.loadChildOnDemand) {
                for (var rec = 0; rec < records.length; rec++) {
                    if (isCountRequired(this.parent) && records[rec].hasChildRecords && this.parent.initialRender) {
                        records[rec].expanded = false;
                    }
                    if (sf.base.isNullOrUndefined(records[rec].index)) {
                        records[rec].taskData = sf.base.extend({}, records[rec]);
                        records[rec].uniqueID = sf.grids.getUid(this.parent.element.id + '_data_');
                        sf.base.setValue('uniqueIDCollection.' + records[rec].uniqueID, records[rec], this.parent);
                        records[rec].level = 0;
                        records[rec].index = Math.ceil(Math.random() * 1000);
                        if ((records[rec][this.parent.hasChildMapping] ||
                            this.parentItems.indexOf(records[rec][this.parent.idMapping]) !== -1)) {
                            records[rec].hasChildRecords = true;
                        }
                        records[rec].checkboxState = 'uncheck';
                    }
                }
            }
            else {
                if (!sf.base.isNullOrUndefined(records)) {
                    this.convertToFlatData(records);
                }
            }
        }
        args.result = this.parent.loadChildOnDemand ? this.parent.flatData : records;
        this.parent.notify('updateResults', args);
    };
    /**
     * Function to manipulate datasource
     *
     * @param {{record: ITreeData, rows: HTMLTableRowElement[], parentRow: HTMLTableRowElement}} rowDetails - Row details for which child rows has to be fetched
     * @param {ITreeData} rowDetails.record - current expanding record
     * @param {HTMLTableRowElement[]} rowDetails.rows - Expanding Row element
     * @param {HTMLTableRowElement} rowDetails.parentRow  - Curent expanding row element
     * @param {boolean} isChild - Specified whether current record is already a child record
     * @hidden
     * @returns {void}
     */
    DataManipulation.prototype.collectExpandingRecs = function (rowDetails, isChild) {
        var gridRows = this.parent.getRows();
        if (this.parent.rowTemplate) {
            var rows = this.parent.getContentTable().rows;
            gridRows = [].slice.call(rows);
        }
        var childRecord;
        if (rowDetails.rows.length > 0) {
            if (!isChild) {
                rowDetails.record.expanded = true;
            }
            for (var i = 0; i < rowDetails.rows.length; i++) {
                rowDetails.rows[i].style.display = 'table-row';
                if (this.parent.loadChildOnDemand) {
                    var targetEle = rowDetails.rows[i].getElementsByClassName('e-treegridcollapse')[0];
                    childRecord = this.parent.rowTemplate ? this.parent.grid.getCurrentViewRecords()[rowDetails.rows[i].rowIndex] :
                        this.parent.grid.getRowObjectFromUID(rowDetails.rows[i].getAttribute('data-Uid')).data;
                    if (!sf.base.isNullOrUndefined(targetEle) && childRecord.expanded) {
                        sf.base.addClass([targetEle], 'e-treegridexpand');
                        sf.base.removeClass([targetEle], 'e-treegridcollapse');
                    }
                    var childRows = [];
                    childRows = gridRows.filter(function (r) {
                        return r.querySelector('.e-gridrowindex' + childRecord.index + 'level' + (childRecord.level + 1));
                    });
                    if (childRows.length && childRecord.expanded) {
                        this.collectExpandingRecs({ record: childRecord, rows: childRows, parentRow: rowDetails.parentRow }, true);
                    }
                }
                var expandingTd = rowDetails.rows[i].querySelector('.e-detailrowcollapse');
                if (!sf.base.isNullOrUndefined(expandingTd)) {
                    this.parent.grid.detailRowModule.expand(expandingTd);
                }
            }
        }
        else {
            this.fetchRemoteChildData({ record: rowDetails.record, rows: rowDetails.rows, parentRow: rowDetails.parentRow });
        }
    };
    DataManipulation.prototype.fetchRemoteChildData = function (rowDetails) {
        var _this = this;
        var args = { row: rowDetails.parentRow, data: rowDetails.record };
        var dm = this.parent.dataSource;
        var qry = this.parent.grid.getDataModule().generateQuery();
        var clonequries = qry.queries.filter(function (e) { return e.fn !== 'onPage' && e.fn !== 'onWhere'; });
        qry.queries = clonequries;
        qry.isCountRequired = true;
        qry.where(this.parent.parentIdMapping, 'equal', rowDetails.record[this.parent.idMapping]);
        sf.popups.showSpinner(this.parent.element);
        dm.executeQuery(qry).then(function (e) {
            var datas = _this.parent.grid.currentViewData.slice();
            var inx = datas.indexOf(rowDetails.record);
            if (inx === -1) {
                _this.parent.grid.getRowsObject().forEach(function (rows) {
                    if (rows.data.uniqueID === rowDetails.record.uniqueID) {
                        inx = rows.index;
                    }
                });
            }
            var haveChild = sf.grids.getObject('actual.nextLevel', e);
            var result = e.result;
            rowDetails.record.childRecords = result;
            for (var r = 0; r < result.length; r++) {
                result[r].taskData = sf.base.extend({}, result[r]);
                result[r].level = rowDetails.record.level + 1;
                result[r].index = Math.ceil(Math.random() * 1000);
                var parentData = sf.base.extend({}, rowDetails.record);
                delete parentData.childRecords;
                result[r].parentItem = parentData;
                result[r].parentUniqueID = rowDetails.record.uniqueID;
                result[r].uniqueID = sf.grids.getUid(_this.parent.element.id + '_data_');
                result[r].checkboxState = 'uncheck';
                sf.base.setValue('uniqueIDCollection.' + result[r].uniqueID, result[r], _this.parent);
                // delete result[r].parentItem.childRecords;
                if ((result[r][_this.parent.hasChildMapping] || _this.parentItems.indexOf(result[r][_this.parent.idMapping]) !== -1)
                    && !(haveChild && !haveChild[r])) {
                    result[r].hasChildRecords = true;
                    result[r].expanded = false;
                }
                datas.splice(inx + r + 1, 0, result[r]);
            }
            sf.base.setValue('result', datas, e);
            sf.base.setValue('action', 'beforecontentrender', e);
            _this.parent.trigger(actionComplete, e);
            sf.popups.hideSpinner(_this.parent.element);
            if (_this.parent.grid.aggregates.length > 0 && !_this.parent.enableVirtualization) {
                var gridQuery = sf.grids.getObject('query', e);
                var result_1 = 'result';
                if (sf.base.isNullOrUndefined(gridQuery)) {
                    gridQuery = sf.base.getValue('grid.renderModule.data', _this.parent).aggregateQuery(new sf.data.Query());
                }
                if (!sf.base.isNullOrUndefined(gridQuery)) {
                    var summaryQuery = gridQuery.queries.filter(function (q) { return q.fn === 'onAggregates'; });
                    e[result_1] = _this.parent.summaryModule.calculateSummaryValue(summaryQuery, e[result_1], true);
                }
            }
            e.count = _this.parent.grid.pageSettings.totalRecordsCount;
            var virtualArgs = {};
            if (_this.parent.enableVirtualization) {
                _this.remoteVirtualAction(virtualArgs);
            }
            var notifyArgs = { index: inx, childData: result };
            if (_this.parent.enableInfiniteScrolling) {
                _this.parent.notify('infinite-remote-expand', notifyArgs);
            }
            else {
                sf.base.getValue('grid.renderModule', _this.parent).dataManagerSuccess(e, virtualArgs);
            }
            _this.parent.trigger(expanded, args);
        });
    };
    DataManipulation.prototype.remoteVirtualAction = function (virtualArgs) {
        virtualArgs.requestType = 'refresh';
        sf.base.setValue('isExpandCollapse', true, virtualArgs);
        var contentModule = sf.base.getValue('grid.contentModule', this.parent);
        var currentInfo = sf.base.getValue('currentInfo', contentModule);
        var prevInfo = sf.base.getValue('prevInfo', contentModule);
        if (currentInfo.loadNext && this.parent.grid.pageSettings.currentPage === currentInfo.nextInfo.page) {
            this.parent.grid.pageSettings.currentPage = prevInfo.page;
        }
    };
    DataManipulation.prototype.beginSorting = function () {
        this.isSortAction = true;
    };
    DataManipulation.prototype.createRecords = function (data, parentRecords) {
        var treeGridData = [];
        for (var i = 0, len = Object.keys(data).length; i < len; i++) {
            var currentData = sf.base.extend({}, data[i]);
            currentData.taskData = data[i];
            var level = 0;
            this.storedIndex++;
            if (!Object.prototype.hasOwnProperty.call(currentData, 'index')) {
                currentData.index = this.storedIndex;
            }
            if (!sf.base.isNullOrUndefined(currentData[this.parent.childMapping]) ||
                (currentData[this.parent.hasChildMapping] && isCountRequired(this.parent))) {
                currentData.hasChildRecords = true;
                if (this.parent.enableCollapseAll || !sf.base.isNullOrUndefined(this.parent.dataStateChange)
                    && sf.base.isNullOrUndefined(currentData[this.parent.childMapping])) {
                    currentData.expanded = false;
                }
                else {
                    currentData.expanded = !sf.base.isNullOrUndefined(currentData[this.parent.expandStateMapping])
                        ? currentData[this.parent.expandStateMapping] : true;
                }
            }
            if (!Object.prototype.hasOwnProperty.call(currentData, 'index')) {
                currentData.index = currentData.hasChildRecords ? this.storedIndex : this.storedIndex;
            }
            if (this.isSelfReference && sf.base.isNullOrUndefined(currentData[this.parent.parentIdMapping])) {
                this.parent.parentData.push(currentData);
            }
            currentData.uniqueID = sf.grids.getUid(this.parent.element.id + '_data_');
            sf.base.setValue('uniqueIDCollection.' + currentData.uniqueID, currentData, this.parent);
            if (!sf.base.isNullOrUndefined(parentRecords)) {
                var parentData = sf.base.extend({}, parentRecords);
                delete parentData.childRecords;
                delete parentData[this.parent.childMapping];
                if (this.isSelfReference) {
                    delete parentData.taskData[this.parent.childMapping];
                }
                currentData.parentItem = parentData;
                currentData.parentUniqueID = parentData.uniqueID;
                level = parentRecords.level + 1;
            }
            if (!Object.prototype.hasOwnProperty.call(currentData, 'level')) {
                currentData.level = level;
            }
            currentData.checkboxState = 'uncheck';
            if (sf.base.isNullOrUndefined(currentData[this.parent.parentIdMapping]) || currentData.parentItem) {
                this.parent.flatData.push(currentData);
            }
            if (!this.isSelfReference && currentData.level === 0) {
                this.parent.parentData.push(currentData);
            }
            if (!sf.base.isNullOrUndefined(currentData[this.parent.childMapping] && currentData[this.parent.childMapping].length)) {
                var record = this.createRecords(currentData[this.parent.childMapping], currentData);
                currentData.childRecords = record;
            }
            treeGridData.push(currentData);
        }
        return treeGridData;
    };
    /**
     * Function to perform filtering/sorting action for local data
     *
     * @param {BeforeDataBoundArgs} args - data details to be processed before binding to grid
     * @hidden
     * @returns {void}
     */
    DataManipulation.prototype.dataProcessor = function (args) {
        var isExport = sf.grids.getObject('isExport', args);
        var expresults = sf.grids.getObject('expresults', args);
        var exportType = sf.grids.getObject('exportType', args);
        var isPrinting = sf.grids.getObject('isPrinting', args);
        var dataObj;
        var actionArgs = sf.grids.getObject('actionArgs', args);
        var requestType = sf.grids.getObject('requestType', args);
        var actionData = sf.grids.getObject('data', args);
        var action = sf.grids.getObject('action', args);
        var actionAddArgs = actionArgs;
        var primaryKeyColumnName = this.parent.getPrimaryKeyFieldNames()[0];
        var dataValue = sf.grids.getObject('data', actionAddArgs);
        if ((!sf.base.isNullOrUndefined(actionAddArgs)) && (!sf.base.isNullOrUndefined(actionAddArgs.action)) && (actionAddArgs.action === 'add')
            && (!sf.base.isNullOrUndefined(actionAddArgs.data)) && sf.base.isNullOrUndefined(actionAddArgs.data[primaryKeyColumnName])) {
            actionAddArgs.data[primaryKeyColumnName] = args.result[actionAddArgs.index][primaryKeyColumnName];
            dataValue.taskData[primaryKeyColumnName] = args.result[actionAddArgs.index][primaryKeyColumnName];
        }
        if ((!sf.base.isNullOrUndefined(actionArgs) && Object.keys(actionArgs).length) || requestType === 'save') {
            requestType = requestType ? requestType : actionArgs.requestType;
            actionData = actionData ? actionData : sf.grids.getObject('data', actionArgs);
            action = action ? action : sf.grids.getObject('action', actionArgs);
            if (this.parent.editSettings.mode === 'Batch') {
                this.batchChanges = this.parent.grid.editModule.getBatchChanges();
            }
            if (this.parent.isLocalData) {
                this.updateAction(actionData, action, requestType);
            }
        }
        if (isExport && !sf.base.isNullOrUndefined(expresults)) {
            dataObj = expresults;
        }
        else {
            dataObj = isCountRequired(this.parent) ? sf.base.getValue('result', this.parent.grid.dataSource)
                : this.parent.grid.dataSource;
        }
        var results = dataObj instanceof sf.data.DataManager ? dataObj.dataSource.json : dataObj;
        var count = isCountRequired(this.parent) ? sf.base.getValue('count', this.parent.dataSource)
            : results.length;
        var qry = new sf.data.Query();
        var gridQuery = sf.grids.getObject('query', args);
        var filterQuery;
        var searchQuery;
        if (!sf.base.isNullOrUndefined(gridQuery)) {
            filterQuery = gridQuery.queries.filter(function (q) { return q.fn === 'onWhere'; });
            searchQuery = gridQuery.queries.filter(function (q) { return q.fn === 'onSearch'; });
        }
        if ((this.parent.grid.allowFiltering && this.parent.grid.filterSettings.columns.length) ||
            (this.parent.grid.searchSettings.key.length > 0) || (!sf.base.isNullOrUndefined(gridQuery) &&
            (filterQuery.length || searchQuery.length) && this.parent.isLocalData)) {
            if (sf.base.isNullOrUndefined(gridQuery)) {
                gridQuery = new sf.data.Query();
                gridQuery = sf.base.getValue('grid.renderModule.data', this.parent).filterQuery(gridQuery);
                gridQuery = sf.base.getValue('grid.renderModule.data', this.parent).searchQuery(gridQuery);
            }
            var fltrQuery = gridQuery.queries.filter(function (q) { return q.fn === 'onWhere'; });
            var srchQuery = gridQuery.queries.filter(function (q) { return q.fn === 'onSearch'; });
            qry.queries = fltrQuery.concat(srchQuery);
            var filteredData = new sf.data.DataManager(results).executeLocal(qry);
            this.parent.notify('updateFilterRecs', { data: filteredData });
            results = this.dataResults.result;
            this.dataResults.result = null;
            if (this.parent.grid.aggregates.length > 0) {
                var query = sf.grids.getObject('query', args);
                if (sf.base.isNullOrUndefined(gridQuery)) {
                    gridQuery = sf.base.getValue('grid.renderModule.data', this.parent).aggregateQuery(new sf.data.Query());
                }
                if (!sf.base.isNullOrUndefined(query)) {
                    var summaryQuery = query.queries.filter(function (q) { return q.fn === 'onAggregates'; });
                    results = this.parent.summaryModule.calculateSummaryValue(summaryQuery, results, true);
                }
            }
        }
        if (this.parent.grid.aggregates.length && this.parent.grid.sortSettings.columns.length === 0
            && this.parent.grid.filterSettings.columns.length === 0 && !this.parent.grid.searchSettings.key.length) {
            var gridQuery_1 = sf.grids.getObject('query', args);
            if (sf.base.isNullOrUndefined(gridQuery_1)) {
                gridQuery_1 = sf.base.getValue('grid.renderModule.data', this.parent).aggregateQuery(new sf.data.Query());
            }
            var summaryQuery = gridQuery_1.queries.filter(function (q) { return q.fn === 'onAggregates'; });
            results = this.parent.summaryModule.calculateSummaryValue(summaryQuery, this.parent.flatData, true);
        }
        if (this.parent.grid.sortSettings.columns.length > 0 || this.isSortAction) {
            this.isSortAction = false;
            var parentData = this.parent.parentData;
            var query = sf.grids.getObject('query', args);
            var srtQry = new sf.data.Query();
            for (var srt = this.parent.grid.sortSettings.columns.length - 1; srt >= 0; srt--) {
                var col = this.parent.grid.getColumnByField(this.parent.grid.sortSettings.columns[srt].field);
                var compFun = col.sortComparer && isOffline(this.parent) ?
                    col.sortComparer.bind(col) :
                    this.parent.grid.sortSettings.columns[srt].direction;
                srtQry.sortBy(this.parent.grid.sortSettings.columns[srt].field, compFun);
            }
            var modifiedData = new sf.data.DataManager(parentData).executeLocal(srtQry);
            var sortArgs = { modifiedData: modifiedData, filteredData: results, srtQry: srtQry };
            this.parent.notify('createSort', sortArgs);
            results = sortArgs.modifiedData;
            this.dataResults.result = null;
            this.sortedData = results;
            this.parent.notify('updateModel', {});
            if (this.parent.grid.aggregates.length > 0 && !sf.base.isNullOrUndefined(query)) {
                var isSort = false;
                var query_1 = sf.grids.getObject('query', args);
                var summaryQuery = query_1.queries.filter(function (q) { return q.fn === 'onAggregates'; });
                results = this.parent.summaryModule.calculateSummaryValue(summaryQuery, this.sortedData, isSort);
            }
        }
        count = isCountRequired(this.parent) ? sf.base.getValue('count', this.parent.dataSource)
            : results.length;
        var temp = this.paging(results, count, isExport, isPrinting, exportType, args);
        results = temp.result;
        count = temp.count;
        args.result = results;
        args.count = count;
        this.parent.notify('updateResults', args);
    };
    DataManipulation.prototype.paging = function (results, count, isExport, isPrinting, exportType, args) {
        if (this.parent.allowPaging && (!isExport || exportType === 'CurrentPage')
            && (!isPrinting || this.parent.printMode === 'CurrentPage')) {
            this.parent.notify(pagingActions, { result: results, count: count });
            results = this.dataResults.result;
            count = isCountRequired(this.parent) ? sf.base.getValue('count', this.parent.dataSource)
                : this.dataResults.count;
        }
        else if ((this.parent.enableVirtualization || this.parent.enableInfiniteScrolling) && (!isExport || exportType === 'CurrentPage')
            && sf.base.getValue('requestType', args) !== 'save') {
            var actArgs = this.parent.enableInfiniteScrolling ? args : sf.base.getValue('actionArgs', args);
            this.parent.notify(pagingActions, { result: results, count: count, actionArgs: actArgs });
            results = this.dataResults.result;
            count = this.dataResults.count;
        }
        var isPdfExport = 'isPdfExport';
        var isCollapsedStatePersist = 'isCollapsedStatePersist';
        if ((isPrinting === true || (args[isPdfExport] && (sf.base.isNullOrUndefined(args[isCollapsedStatePersist])
            || args[isCollapsedStatePersist]))) && this.parent.printMode === 'AllPages') {
            var actualResults = [];
            for (var i = 0; i < results.length; i++) {
                var expandStatus = getExpandStatus(this.parent, results[i], this.parent.parentData);
                if (expandStatus) {
                    actualResults.push(results[i]);
                }
            }
            results = actualResults;
            count = results.length;
        }
        var value = { result: results, count: count };
        return value;
    };
    DataManipulation.prototype.updateData = function (dataResult) {
        this.dataResults = dataResult;
    };
    DataManipulation.prototype.updateAction = function (actionData, action, requestType) {
        if ((requestType === 'delete' || requestType === 'save')) {
            this.parent.notify(crudAction, { value: actionData, action: action || requestType });
        }
        if (requestType === 'batchsave' && this.parent.editSettings.mode === 'Batch') {
            this.parent.notify(batchSave, {});
        }
    };
    return DataManipulation;
}());

/**
 * Defines Predefined toolbar items.
 *
 * @hidden
 */

(function (ToolbarItem) {
    ToolbarItem[ToolbarItem["Add"] = 0] = "Add";
    ToolbarItem[ToolbarItem["Edit"] = 1] = "Edit";
    ToolbarItem[ToolbarItem["Update"] = 2] = "Update";
    ToolbarItem[ToolbarItem["Delete"] = 3] = "Delete";
    ToolbarItem[ToolbarItem["Cancel"] = 4] = "Cancel";
    ToolbarItem[ToolbarItem["Search"] = 5] = "Search";
    ToolbarItem[ToolbarItem["ExpandAll"] = 6] = "ExpandAll";
    ToolbarItem[ToolbarItem["CollapseAll"] = 7] = "CollapseAll";
    ToolbarItem[ToolbarItem["ExcelExport"] = 8] = "ExcelExport";
    ToolbarItem[ToolbarItem["PdfExport"] = 9] = "PdfExport";
    ToolbarItem[ToolbarItem["CsvExport"] = 10] = "CsvExport";
    ToolbarItem[ToolbarItem["Print"] = 11] = "Print";
    ToolbarItem[ToolbarItem["RowIndent"] = 12] = "RowIndent";
    ToolbarItem[ToolbarItem["RowOutdent"] = 13] = "RowOutdent";
})(exports.ToolbarItem || (exports.ToolbarItem = {}));
/**
 * Defines predefined contextmenu items.
 *
 * @hidden
 */

(function (ContextMenuItems) {
    ContextMenuItems[ContextMenuItems["AutoFit"] = 0] = "AutoFit";
    ContextMenuItems[ContextMenuItems["AutoFitAll"] = 1] = "AutoFitAll";
    ContextMenuItems[ContextMenuItems["SortAscending"] = 2] = "SortAscending";
    ContextMenuItems[ContextMenuItems["SortDescending"] = 3] = "SortDescending";
    ContextMenuItems[ContextMenuItems["Edit"] = 4] = "Edit";
    ContextMenuItems[ContextMenuItems["Delete"] = 5] = "Delete";
    ContextMenuItems[ContextMenuItems["Save"] = 6] = "Save";
    ContextMenuItems[ContextMenuItems["Cancel"] = 7] = "Cancel";
    ContextMenuItems[ContextMenuItems["PdfExport"] = 8] = "PdfExport";
    ContextMenuItems[ContextMenuItems["ExcelExport"] = 9] = "ExcelExport";
    ContextMenuItems[ContextMenuItems["CsvExport"] = 10] = "CsvExport";
    ContextMenuItems[ContextMenuItems["FirstPage"] = 11] = "FirstPage";
    ContextMenuItems[ContextMenuItems["PrevPage"] = 12] = "PrevPage";
    ContextMenuItems[ContextMenuItems["LastPage"] = 13] = "LastPage";
    ContextMenuItems[ContextMenuItems["NextPage"] = 14] = "NextPage";
    ContextMenuItems[ContextMenuItems["AddRow"] = 15] = "AddRow";
})(exports.ContextMenuItems || (exports.ContextMenuItems = {}));

var __extends$7 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$5 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the paging behavior of the TreeGrid.
 */
var PageSettings = /** @class */ (function (_super) {
    __extends$7(PageSettings, _super);
    function PageSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$5([
        sf.base.Property(12)
    ], PageSettings.prototype, "pageSize", void 0);
    __decorate$5([
        sf.base.Property(8)
    ], PageSettings.prototype, "pageCount", void 0);
    __decorate$5([
        sf.base.Property(1)
    ], PageSettings.prototype, "currentPage", void 0);
    __decorate$5([
        sf.base.Property()
    ], PageSettings.prototype, "totalRecordsCount", void 0);
    __decorate$5([
        sf.base.Property(false)
    ], PageSettings.prototype, "enableQueryString", void 0);
    __decorate$5([
        sf.base.Property(false)
    ], PageSettings.prototype, "pageSizes", void 0);
    __decorate$5([
        sf.base.Property(null)
    ], PageSettings.prototype, "template", void 0);
    __decorate$5([
        sf.base.Property('All')
    ], PageSettings.prototype, "pageSizeMode", void 0);
    return PageSettings;
}(sf.base.ChildProperty));

var __extends$8 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$6 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the TreeGrid's aggregate column.
 */
var AggregateColumn = /** @class */ (function (_super) {
    __extends$8(AggregateColumn, _super);
    function AggregateColumn() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.intl = new sf.base.Internationalization();
        _this.templateFn = {};
        return _this;
    }
    /**
     * Custom format function
     *
     * @hidden
     * @param {string} cultureName - culture name to format
     * @returns {void}
     */
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    AggregateColumn.prototype.setFormatter = function (cultureName) {
        if (this.format && (this.format.skeleton || this.format.format)) {
            this.formatFn = this.getFormatFunction(this.format);
        }
    };
    /**
     * @param {NumberFormatOptions | DateFormatOptions} format - formatting options for number and date values
     * @hidden
     * @returns {Function} - return formatter function
     */
    AggregateColumn.prototype.getFormatFunction = function (format) {
        if (format.type) {
            return this.intl.getDateFormat(format);
        }
        else {
            return this.intl.getNumberFormat(format);
        }
    };
    /**
     * @hidden
     * @returns {Function} - Returns formatter function
     */
    AggregateColumn.prototype.getFormatter = function () {
        return this.formatFn;
    };
    /**
     * @param {Object} helper - Specified the helper
     * @hidden
     * @returns {void}
     */
    AggregateColumn.prototype.setTemplate = function (helper) {
        if (helper === void 0) { helper = {}; }
        if (this.footerTemplate !== undefined) {
            this.templateFn[sf.base.getEnumValue(sf.grids.CellType, sf.grids.CellType.Summary)] = { fn: sf.base.compile(this.footerTemplate, helper),
                property: 'footerTemplate' };
        }
    };
    /**
     * @param {CellType} type - specifies the cell type
     * @returns {Object} returns the object
     * @hidden
     */
    AggregateColumn.prototype.getTemplate = function (type) {
        return this.templateFn[sf.base.getEnumValue(sf.grids.CellType, type)];
    };
    /**
     * @param {Object} prop - updates aggregate properties without change detection
     * @hidden
     * @returns {void}
     */
    AggregateColumn.prototype.setPropertiesSilent = function (prop) {
        this.setProperties(prop, true);
    };
    __decorate$6([
        sf.base.Property()
    ], AggregateColumn.prototype, "type", void 0);
    __decorate$6([
        sf.base.Property()
    ], AggregateColumn.prototype, "footerTemplate", void 0);
    __decorate$6([
        sf.base.Property()
    ], AggregateColumn.prototype, "field", void 0);
    __decorate$6([
        sf.base.Property()
    ], AggregateColumn.prototype, "format", void 0);
    __decorate$6([
        sf.base.Property()
    ], AggregateColumn.prototype, "columnName", void 0);
    __decorate$6([
        sf.base.Property()
    ], AggregateColumn.prototype, "customAggregate", void 0);
    return AggregateColumn;
}(sf.base.ChildProperty));
var AggregateRow = /** @class */ (function (_super) {
    __extends$8(AggregateRow, _super);
    function AggregateRow() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$6([
        sf.base.Collection([], AggregateColumn)
    ], AggregateRow.prototype, "columns", void 0);
    __decorate$6([
        sf.base.Property(true)
    ], AggregateRow.prototype, "showChildSummary", void 0);
    return AggregateRow;
}(sf.base.ChildProperty));

var __extends$9 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$7 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the edit behavior of the TreeGrid.
 */
var EditSettings = /** @class */ (function (_super) {
    __extends$9(EditSettings, _super);
    function EditSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$7([
        sf.base.Property(false)
    ], EditSettings.prototype, "allowAdding", void 0);
    __decorate$7([
        sf.base.Property(false)
    ], EditSettings.prototype, "allowEditing", void 0);
    __decorate$7([
        sf.base.Property(false)
    ], EditSettings.prototype, "allowDeleting", void 0);
    __decorate$7([
        sf.base.Property('Cell')
    ], EditSettings.prototype, "mode", void 0);
    __decorate$7([
        sf.base.Property('Top')
    ], EditSettings.prototype, "newRowPosition", void 0);
    __decorate$7([
        sf.base.Property(true)
    ], EditSettings.prototype, "allowEditOnDblClick", void 0);
    __decorate$7([
        sf.base.Property(true)
    ], EditSettings.prototype, "showConfirmDialog", void 0);
    __decorate$7([
        sf.base.Property(false)
    ], EditSettings.prototype, "showDeleteConfirmDialog", void 0);
    __decorate$7([
        sf.base.Property('')
    ], EditSettings.prototype, "template", void 0);
    __decorate$7([
        sf.base.Property({})
    ], EditSettings.prototype, "dialog", void 0);
    __decorate$7([
        sf.base.Property(false)
    ], EditSettings.prototype, "allowNextRowEdit", void 0);
    return EditSettings;
}(sf.base.ChildProperty));

var __extends$10 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$8 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Represents the field name and direction of sort column.
 */
var SortDescriptor = /** @class */ (function (_super) {
    __extends$10(SortDescriptor, _super);
    function SortDescriptor() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$8([
        sf.base.Property()
    ], SortDescriptor.prototype, "field", void 0);
    __decorate$8([
        sf.base.Property()
    ], SortDescriptor.prototype, "direction", void 0);
    return SortDescriptor;
}(sf.base.ChildProperty));
/**
 * Configures the sorting behavior of TreeGrid.
 */
var SortSettings = /** @class */ (function (_super) {
    __extends$10(SortSettings, _super);
    function SortSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$8([
        sf.base.Collection([], SortDescriptor)
    ], SortSettings.prototype, "columns", void 0);
    __decorate$8([
        sf.base.Property(true)
    ], SortSettings.prototype, "allowUnsort", void 0);
    return SortSettings;
}(sf.base.ChildProperty));

/**
 * Performs CRUD update to Tree Grid data source
 *
 * @param {{value: ITreeData, action: string }} details - Gets modified record value and CRUD action type
 * @param {TreeGrid} details.value - Gets modified record value
 * @param {string} details.action - CRUD action type
 * @param {TreeGrid} control - Tree Grid instance
 * @param {boolean} isSelfReference - Denotes whether Self Referential data binding
 * @param {number} addRowIndex - New add row index
 * @param {number} selectedIndex - Selected Row index
 * @param {string} columnName - Column field name
 * @param {ITreeData} addRowRecord - Newly added record
 * @returns {void}
 */
function editAction(details, control, isSelfReference, addRowIndex, selectedIndex, columnName, addRowRecord) {
    var value = details.value;
    var action = details.action;
    var changedRecords = 'changedRecords';
    var i;
    var j;
    var addedRecords = 'addedRecords';
    var batchChanges;
    var key = control.grid.getPrimaryKeyFieldNames()[0];
    var treeData = control.dataSource instanceof sf.data.DataManager ?
        control.dataSource.dataSource.json : control.dataSource;
    var modifiedData = [];
    var originalData = value;
    var isSkip = false;
    if (control.editSettings.mode === 'Batch') {
        batchChanges = control.grid.editModule.getBatchChanges();
    }
    if (action === 'add' || (action === 'batchsave' && (control.editSettings.mode === 'Batch'
        && batchChanges[addedRecords].length))) {
        var addAct = addAction(details, treeData, control, isSelfReference, addRowIndex, selectedIndex, addRowRecord);
        value = addAct.value;
        isSkip = addAct.isSkip;
    }
    if (value instanceof Array) {
        modifiedData = extendArray(value);
    }
    else {
        modifiedData.push(sf.base.extend({}, value));
    }
    if (!isSkip && (action !== 'add' ||
        (control.editSettings.newRowPosition !== 'Top' && control.editSettings.newRowPosition !== 'Bottom'))) {
        for (var k = 0; k < modifiedData.length; k++) {
            if (typeof (modifiedData[k][key]) === 'object') {
                modifiedData[k] = modifiedData[k][key];
            }
            var keys = modifiedData[k].taskData ? Object.keys(modifiedData[k].taskData) :
                Object.keys(modifiedData[k]);
            i = treeData.length;
            var _loop_1 = function () {
                if (treeData[i][key] === modifiedData[k][key]) {
                    if (action === 'delete') {
                        var currentData_1 = treeData[i];
                        treeData.splice(i, 1);
                        if (isSelfReference) {
                            if (!sf.base.isNullOrUndefined(currentData_1[control.parentIdMapping])) {
                                var parentData = control.flatData.filter(function (e) {
                                    return e[control.idMapping] === currentData_1[control.parentIdMapping];
                                })[0];
                                var childRecords = parentData ? parentData[control.childMapping] : [];
                                for (var p = childRecords.length - 1; p >= 0; p--) {
                                    if (childRecords[p][control.idMapping] === currentData_1[control.idMapping]) {
                                        childRecords.splice(p, 1);
                                        if (!childRecords.length) {
                                            parentData.hasChildRecords = false;
                                            updateParentRow(key, parentData, action, control, isSelfReference);
                                        }
                                        break;
                                    }
                                }
                            }
                            return "break";
                        }
                    }
                    else {
                        if (action === 'edit') {
                            for (j = 0; j < keys.length; j++) {
                                if (Object.prototype.hasOwnProperty.call(treeData[i], keys[j]) && ((control.editSettings.mode !== 'Cell'
                                    || (!sf.base.isNullOrUndefined(batchChanges) && batchChanges[changedRecords].length === 0))
                                    || keys[j] === columnName)) {
                                    var editedData = getParentData(control, modifiedData[k].uniqueID);
                                    treeData[i][keys[j]] = modifiedData[k][keys[j]];
                                    if (editedData && editedData.taskData) {
                                        editedData.taskData[keys[j]] = editedData[keys[j]] = treeData[i][keys[j]];
                                    }
                                }
                            }
                        }
                        else if (action === 'add' || action === 'batchsave') {
                            var index = void 0;
                            if (control.editSettings.newRowPosition === 'Child') {
                                if (isSelfReference) {
                                    originalData.taskData[control.parentIdMapping] = treeData[i][control.idMapping];
                                    treeData.splice(i + 1, 0, originalData.taskData);
                                }
                                else {
                                    if (!Object.prototype.hasOwnProperty.call(treeData[i], control.childMapping)) {
                                        treeData[i][control.childMapping] = [];
                                    }
                                    treeData[i][control.childMapping].push(originalData.taskData);
                                    updateParentRow(key, treeData[i], action, control, isSelfReference, originalData);
                                }
                            }
                            else if (control.editSettings.newRowPosition === 'Below') {
                                treeData.splice(i + 1, 0, originalData.taskData);
                                if (!sf.base.isNullOrUndefined(originalData.parentItem)) {
                                    updateParentRow(key, treeData[i + 1], action, control, isSelfReference, originalData);
                                }
                            }
                            else if (!addRowIndex) {
                                index = 0;
                                treeData.splice(index, 0, originalData.taskData);
                            }
                            else if (control.editSettings.newRowPosition === 'Above') {
                                treeData.splice(i, 0, originalData.taskData);
                                if (!sf.base.isNullOrUndefined(originalData.parentItem)) {
                                    updateParentRow(key, treeData[i], action, control, isSelfReference, originalData);
                                }
                            }
                        }
                        return "break";
                    }
                }
                else if (!sf.base.isNullOrUndefined(treeData[i][control.childMapping])) {
                    if (removeChildRecords(treeData[i][control.childMapping], modifiedData[k], action, key, control, isSelfReference, originalData, columnName)) {
                        updateParentRow(key, treeData[i], action, control, isSelfReference);
                    }
                }
            };
            while (i-- && i >= 0) {
                var state_1 = _loop_1();
                if (state_1 === "break")
                    break;
            }
        }
    }
}
/**
 * Performs Add action to Tree Grid data source
 *
 * @param {{value: ITreeData, action: string }} details - Gets modified record value and CRUD action type
 * @param {TreeGrid} details.value - Gets modified record value
 * @param {string} details.action - CRUD action type
 * @param {Object[]} treeData - Tree Grid data source
 * @param {TreeGrid} control - Tree Grid instance
 * @param {boolean} isSelfReference - Denotes whether Self Referential data binding
 * @param {number} addRowIndex - New add row index
 * @param {number} selectedIndex - Selected Row index
 * @param {ITreeData} addRowRecord - Newly added record
 * @returns {void}
 */
function addAction(details, treeData, control, isSelfReference, addRowIndex, selectedIndex, addRowRecord) {
    var value;
    var isSkip = false;
    var currentViewRecords = control.grid.getCurrentViewRecords();
    value = sf.base.extend({}, details.value);
    value = getPlainData(value);
    switch (control.editSettings.newRowPosition) {
        case 'Top':
            treeData.unshift(value);
            isSkip = true;
            break;
        case 'Bottom':
            treeData.push(value);
            isSkip = true;
            break;
        case 'Above':
            if (!sf.base.isNullOrUndefined(addRowRecord)) {
                value = sf.base.extend({}, addRowRecord);
                value = getPlainData(value);
            }
            else {
                value = sf.base.extend({}, currentViewRecords[addRowIndex + 1]);
                value = getPlainData(value);
            }
            break;
        case 'Below':
        case 'Child':
            if (!sf.base.isNullOrUndefined(addRowRecord)) {
                value = sf.base.extend({}, addRowRecord);
                value = getPlainData(value);
            }
            else {
                var primaryKeys = control.grid.getPrimaryKeyFieldNames()[0];
                var currentdata = currentViewRecords[addRowIndex];
                if (!sf.base.isNullOrUndefined(currentdata) && currentdata[primaryKeys] === details.value[primaryKeys] || selectedIndex !== -1) {
                    value = sf.base.extend({}, currentdata);
                }
                else {
                    value = sf.base.extend({}, details.value);
                }
                value = getPlainData(value);
                var internalProperty = 'internalProperties';
                control.editModule[internalProperty].taskData = value;
            }
            if (selectedIndex === -1) {
                treeData.unshift(value);
                isSkip = true;
            }
    }
    return { value: value, isSkip: isSkip };
}
/**
 * @param {ITreeData[]} childRecords - Child Records collection
 * @param {Object} modifiedData - Modified data in crud action
 * @param {string} action - crud action type
 * @param {string} key - Primary key field name
 * @param {TreeGrid} control - Tree Grid instance
 * @param {boolean} isSelfReference - Specified whether Self Referential data binding
 * @param {ITreeData} originalData - Non updated data from data source, of edited data
 * @param {string} columnName - column field name
 * @returns {boolean} Returns whether child records exists
 */
function removeChildRecords(childRecords, modifiedData, action, key, control, isSelfReference, originalData, columnName) {
    var isChildAll = false;
    var j = childRecords.length;
    while (j-- && j >= 0) {
        if (childRecords[j][key] === modifiedData[key] ||
            (isSelfReference && childRecords[j][control.parentIdMapping] === modifiedData[control.idMapping])) {
            if (action === 'edit') {
                var keys = Object.keys(modifiedData);
                var editedData = getParentData(control, modifiedData.uniqueID);
                for (var i = 0; i < keys.length; i++) {
                    if (Object.prototype.hasOwnProperty.call(childRecords[j], keys[i]) && (control.editSettings.mode !== 'Cell' || keys[i] === columnName)) {
                        editedData[keys[i]] = editedData.taskData[keys[i]] = childRecords[j][keys[i]] = modifiedData[keys[i]];
                        if (control.grid.editSettings.mode === 'Normal' && control.editSettings.mode === 'Cell') {
                            var editModule = 'editModule';
                            control.grid.editModule[editModule].editRowIndex = modifiedData.index;
                            control.grid.editModule[editModule].updateCurrentViewData(modifiedData);
                        }
                    }
                }
                break;
            }
            else if (action === 'add' || action === 'batchsave') {
                if (control.editSettings.newRowPosition === 'Child') {
                    if (isSelfReference) {
                        originalData[control.parentIdMapping] = childRecords[j][control.idMapping];
                        childRecords.splice(j + 1, 0, originalData);
                        updateParentRow(key, childRecords[j], action, control, isSelfReference, originalData);
                    }
                    else {
                        if (!Object.prototype.hasOwnProperty.call(childRecords[j], control.childMapping)) {
                            childRecords[j][control.childMapping] = [];
                        }
                        childRecords[j][control.childMapping].push(originalData.taskData);
                        updateParentRow(key, childRecords[j], action, control, isSelfReference, originalData);
                    }
                }
                else if (control.editSettings.newRowPosition === 'Above') {
                    childRecords.splice(j, 0, originalData.taskData);
                    if (!sf.base.isNullOrUndefined(originalData.parentItem)) {
                        updateParentRow(key, childRecords[j], action, control, isSelfReference, originalData);
                    }
                }
                else if (control.editSettings.newRowPosition === 'Below') {
                    childRecords.splice(j + 1, 0, originalData.taskData);
                    if (!sf.base.isNullOrUndefined(originalData.parentItem)) {
                        updateParentRow(key, childRecords[j], action, control, isSelfReference, originalData);
                    }
                }
            }
            else {
                childRecords.splice(j, 1);
                if (!childRecords.length) {
                    isChildAll = true;
                }
            }
        }
        else if (!sf.base.isNullOrUndefined(childRecords[j][control.childMapping])) {
            if (removeChildRecords(childRecords[j][control.childMapping], modifiedData, action, key, control, isSelfReference, originalData, columnName)) {
                updateParentRow(key, childRecords[j], action, control, isSelfReference);
            }
        }
    }
    return isChildAll;
}
/**
 * @param {string} key - Primary key field name
 * @param {ITreeData} record - Parent Record which has to be updated
 * @param {string} action - CRUD action type
 * @param {TreeGrid} control - Tree Grid instance
 * @param {boolean} isSelfReference - Specified whether self referential data binding
 * @param {ITreeData} child - Specifies child record
 * @returns {void}
 */
function updateParentRow(key, record, action, control, isSelfReference, child) {
    if ((control.editSettings.newRowPosition === 'Above' || control.editSettings.newRowPosition === 'Below')
        && ((action === 'add' || action === 'batchsave')) && !sf.base.isNullOrUndefined(child.parentItem)) {
        var parentData = getParentData(control, child.parentItem.uniqueID);
        parentData.childRecords.push(child);
    }
    else {
        var currentRecords = control.grid.getCurrentViewRecords();
        var index_1;
        currentRecords.map(function (e, i) { if (e[key] === record[key]) {
            index_1 = i;
            return;
        } });
        if (!sf.base.isNullOrUndefined(index_1)) {
            record = currentRecords[index_1];
        }
        if (control.enableVirtualization && sf.base.isNullOrUndefined(record) && !sf.base.isNullOrUndefined(child)) {
            record = sf.base.getValue('uniqueIDCollection.' + child.parentUniqueID, control);
        }
        if (!isSelfReference && !sf.base.isNullOrUndefined(record.childRecords) && record.childRecords.length) {
            record.hasChildRecords = true;
        }
        else {
            record.hasChildRecords = false;
        }
        if (action === 'add' || action === 'batchsave') {
            record.expanded = true;
            record.hasChildRecords = true;
            if (control.sortSettings.columns.length && sf.base.isNullOrUndefined(child)) {
                child = currentRecords.filter(function (e) {
                    if (e.parentUniqueID === record.uniqueID) {
                        return e;
                    }
                    else {
                        return null;
                    }
                });
            }
            var childRecords = child ? child instanceof Array ? child[0] : child : currentRecords[index_1 + 1];
            if (control.editSettings.newRowPosition !== 'Below') {
                if (!Object.prototype.hasOwnProperty.call(record, 'childRecords')) {
                    record.childRecords = [];
                }
                else {
                    if (!sf.base.isNullOrUndefined(child) && record[key] !== child[key]) {
                        record.childRecords.push(child);
                    }
                }
                if (record.childRecords.indexOf(childRecords) === -1 && record[key] !== child[key]) {
                    record.childRecords.unshift(childRecords);
                }
                if (isSelfReference) {
                    if (!Object.prototype.hasOwnProperty.call(record, control.childMapping)) {
                        record[control.childMapping] = [];
                    }
                    if (record[control.childMapping].indexOf(childRecords) === -1 && record[key] !== child[key]) {
                        record[control.childMapping].unshift(childRecords);
                    }
                }
            }
        }
        var primaryKeys = control.grid.getPrimaryKeyFieldNames()[0];
        var data = control.grid.dataSource instanceof sf.data.DataManager ?
            control.grid.dataSource.dataSource.json : control.grid.dataSource;
        for (var i = 0; i < data.length; i++) {
            if (data[i][primaryKeys] === record[primaryKeys]) {
                data[i] = record;
                break;
            }
        }
        control.grid.setRowData(key, record);
        var row = control.getRowByIndex(index_1);
        if (control.editSettings.mode === 'Batch') {
            row = control.getRows()[control.grid.getRowIndexByPrimaryKey(record[key])];
        }
        var movableRow = void 0;
        if (control.frozenRows || control.getFrozenColumns()) {
            movableRow = control.getMovableRowByIndex(index_1);
        }
        if (!control.enableVirtualization && !sf.base.isNullOrUndefined(row) || !sf.base.isNullOrUndefined(movableRow)) {
            var index_2 = control.treeColumnIndex;
            if (control.allowRowDragAndDrop && control.enableImmutableMode) {
                index_2 = index_2 + 1;
            }
            control.renderModule.cellRender({
                data: record, cell: row.cells[index_2] ? row.cells[index_2]
                    : movableRow.cells[index_2 - control.getFrozenColumns()],
                column: control.grid.getColumns()[control.treeColumnIndex],
                requestType: action
            });
        }
    }
}

var __extends$11 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$9 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the infinite scroll behavior of Tree Grid.
 */
var InfiniteScrollSettings = /** @class */ (function (_super) {
    __extends$11(InfiniteScrollSettings, _super);
    function InfiniteScrollSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$9([
        sf.base.Property(false)
    ], InfiniteScrollSettings.prototype, "enableCache", void 0);
    __decorate$9([
        sf.base.Property(3)
    ], InfiniteScrollSettings.prototype, "maxBlocks", void 0);
    __decorate$9([
        sf.base.Property(3)
    ], InfiniteScrollSettings.prototype, "initialBlocks", void 0);
    return InfiniteScrollSettings;
}(sf.base.ChildProperty));

var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Represents the TreeGrid component.
 * ```html
 * <div id='treegrid'></div>
 * <script>
 *  var treegridObj = new TreeGrid({ allowPaging: true });
 *  treegridObj.appendTo('#treegrid');
 * </script>
 * ```
 */
var TreeGrid = /** @class */ (function (_super) {
    __extends(TreeGrid, _super);
    function TreeGrid(options, element) {
        var _this = _super.call(this, options, element) || this;
        _this.dataResults = {};
        _this.uniqueIDCollection = {};
        _this.uniqueIDFilterCollection = {};
        _this.changedRecords = 'changedRecords';
        _this.deletedRecords = 'deletedRecords';
        _this.addedRecords = 'addedRecords';
        _this.objectEqualityChecker = function (old, current) {
            if (old) {
                var keys = Object.keys(old);
                var isEqual = true;
                var excludeKeys = ['Children', 'childRecords', 'taskData', 'uniqueID', 'parentItem', 'parentUniqueID', 'index'];
                for (var i = 0; i < keys.length; i++) {
                    if (old[keys[i]] !== current[keys[i]] && excludeKeys.indexOf(keys[i]) === -1) {
                        var isDate = old[keys[i]] instanceof Date && current[keys[i]] instanceof Date;
                        if (!isDate || (old[keys[i]].getTime() !== current[keys[i]].getTime())) {
                            isEqual = false;
                            break;
                        }
                    }
                }
                return isEqual;
            }
            else {
                return false;
            }
        };
        TreeGrid_1.Inject(Selection);
        sf.base.setValue('mergePersistData', _this.mergePersistTreeGridData, _this);
        var logger = 'Logger';
        if (!sf.base.isNullOrUndefined(_this.injectedModules[logger])) {
            sf.grids.Grid.Inject(sf.grids.Logger);
        }
        _this.grid = new sf.grids.Grid();
        return _this;
    }
    TreeGrid_1 = TreeGrid;
    /**
     * Export TreeGrid data to Excel file(.xlsx).
     *
     * @param  {ExcelExportProperties | TreeGridExcelExportProperties} excelExportProperties - Defines the export properties of the Tree Grid.
     * @param  {boolean} isMultipleExport - Define to enable multiple export.
     * @param  {workbook} workbook - Defines the Workbook if multiple export is enabled.
     * @param  {boolean} isBlob - If 'isBlob' set to true, then it will be returned as blob data.
     * @returns {Promise<any>} - Returns promise object of export action
     */
    /* eslint-disable */
    TreeGrid.prototype.excelExport = function (excelExportProperties, isMultipleExport, workbook, isBlob) {
        /* eslint-enable */
        return this.excelExportModule.Map(excelExportProperties, isMultipleExport, workbook, isBlob, false);
    };
    /**
     * Export TreeGrid data to CSV file.
     *
     * @param  {ExcelExportProperties} excelExportProperties - Defines the export properties of the TreeGrid.
     * @param  {boolean} isMultipleExport - Define to enable multiple export.
     * @param  {workbook} workbook - Defines the Workbook if multiple export is enabled.
     * @param  {boolean} isBlob - If 'isBlob' set to true, then it will be returned as blob data.
     * @returns {Promise<any>} - Returns promise object of export action
     */
    /* eslint-disable */
    TreeGrid.prototype.csvExport = function (excelExportProperties, isMultipleExport, workbook, isBlob) {
        /* eslint-enable */
        return this.excelExportModule.Map(excelExportProperties, isMultipleExport, workbook, isBlob, true);
    };
    /**
     * Export TreeGrid data to PDF document.
     *
     * @param {PdfExportProperties | TreeGridPdfExportProperties} pdfExportProperties - Defines the export properties of the Tree Grid.
     * @param {boolean} isMultipleExport - Define to enable multiple export.
     * @param {Object} pdfDoc - Defined the Pdf Document if multiple export is enabled.
     * @param {boolean} isBlob - If 'isBlob' set to true, then it will be returned as blob data.
     * @returns {Promise<any>} - Returns promise object of export action
     */
    TreeGrid.prototype.pdfExport = function (pdfExportProperties, isMultipleExport, pdfDoc, isBlob) {
        return this.pdfExportModule.Map(pdfExportProperties, isMultipleExport, pdfDoc, isBlob);
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns TreeGrid module name
     */
    TreeGrid.prototype.getModuleName = function () {
        return 'treegrid';
    };
    /**
     * For internal use only - Initialize the event handler;
     *
     * @private
     * @returns {void}
     */
    TreeGrid.prototype.preRender = function () {
        this.TreeGridLocale();
        this.initProperties();
        this.defaultLocale = {
            Above: 'Above',
            Below: 'Below',
            Child: 'Child',
            AddRow: 'Add Row',
            ExpandAll: 'Expand All',
            CollapseAll: 'Collapse All',
            RowIndent: 'Indent',
            RowOutdent: 'Outdent'
        };
        this.l10n = new sf.base.L10n('treegrid', this.defaultLocale, this.locale);
        if (this.isSelfReference && sf.base.isNullOrUndefined(this.childMapping)) {
            this.childMapping = 'Children';
        }
    };
    /**
     * Sorts a column with the given options.
     *
     * @param {string} columnName - Defines the column name to be sorted.
     * @param {SortDirection} direction - Defines the direction of sorting field.
     * @param {boolean} isMultiSort - Specifies whether the previous sorted columns are to be maintained.
     * @returns {void}
     */
    TreeGrid.prototype.sortByColumn = function (columnName, direction, isMultiSort) {
        this.sortModule.sortColumn(columnName, direction, isMultiSort);
    };
    /**
     * Clears all the sorted columns of the TreeGrid.
     *
     * @returns {void}
     */
    TreeGrid.prototype.clearSorting = function () {
        if (this.sortModule) {
            this.sortModule.clearSorting();
        }
    };
    /**
     * Remove sorted column by field name.
     *
     * @param {string} field - Defines the column field name to remove sort.
     * @returns {void}
     * @hidden
     */
    TreeGrid.prototype.removeSortColumn = function (field) {
        this.sortModule.removeSortColumn(field);
    };
    /**
     * Searches TreeGrid records using the given key.
     * You can customize the default search option by using the
     * [`searchSettings`](./#searchsettings/).
     *
     * @param  {string} searchString - Defines the key.
     * @returns {void}
     */
    TreeGrid.prototype.search = function (searchString) {
        this.grid.search(searchString);
    };
    /**
     * Changes the column width to automatically fit its content to ensure that the width shows the content without wrapping/hiding.
     * > * This method ignores the hidden columns.
     * > * Uses the `autoFitColumns` method in the `dataBound` event to resize at initial rendering.
     *
     * @param  {string |string[]} fieldNames - Defines the column names.
     * @returns {void}
     *
     *
     *
     */
    TreeGrid.prototype.autoFitColumns = function (fieldNames) {
        this.resizeModule.autoFitColumns(fieldNames);
        this.updateColumnModel();
    };
    /**
     * Changes the TreeGrid column positions by field names.
     *
     * @param  {string} fromFName - Defines the origin field name.
     * @param  {string} toFName - Defines the destination field name.
     * @returns {void}
     */
    TreeGrid.prototype.reorderColumns = function (fromFName, toFName) {
        this.grid.reorderColumns(fromFName, toFName);
    };
    TreeGrid.prototype.TreeGridLocale = function () {
        /* eslint-disable-next-line @typescript-eslint/no-explicit-any */
        var locale = sf.base.L10n.locale;
        var localeObject = {};
        sf.base.setValue(this.locale, {}, localeObject);
        var gridLocale;
        gridLocale = {};
        gridLocale = sf.grids.getObject(this.locale, locale);
        var treeGridLocale;
        treeGridLocale = {};
        treeGridLocale = sf.grids.getObject(this.getModuleName(), gridLocale);
        sf.base.setValue('grid', treeGridLocale, sf.grids.getObject(this.locale, localeObject));
        sf.base.L10n.load(localeObject);
    };
    /**
     * By default, prints all the pages of the TreeGrid and hides the pager.
     * > You can customize print options using the
     * [`printMode`](./#printmode).
     *
     * @returns {void}
     */
    TreeGrid.prototype.print = function () {
        this.printModule.print();
    };
    TreeGrid.prototype.treeGridkeyActionHandler = function (e) {
        if (this.allowKeyboard) {
            var target = void 0;
            var parentTarget = void 0;
            var column = void 0;
            var row = void 0;
            var summaryElement = void 0;
            switch (e.action) {
                case 'ctrlDownArrow':
                    this.expandAll();
                    break;
                case 'ctrlUpArrow':
                    this.collapseAll();
                    break;
                case 'ctrlShiftUpArrow':
                    target = e.target;
                    column = target.closest('.e-rowcell');
                    row = column.closest('tr');
                    if (row !== null && row !== undefined) {
                        this.expandCollapseRequest(row.querySelector('.e-treegridexpand'));
                    }
                    break;
                case 'ctrlShiftDownArrow':
                    target = e.target;
                    column = target.closest('.e-rowcell');
                    row = column.closest('tr');
                    if (row !== null && row !== undefined) {
                        this.expandCollapseRequest(row.querySelector('.e-treegridcollapse'));
                    }
                    break;
                case 'downArrow':
                    if (!this.enableVirtualization) {
                        parentTarget = e.target.parentElement;
                        summaryElement = this.findnextRowElement(parentTarget);
                        if (summaryElement !== null) {
                            var rowIndex = summaryElement.rowIndex;
                            this.selectRow(rowIndex);
                            var cellIndex = e.target.cellIndex;
                            var row_1 = summaryElement.children[cellIndex];
                            sf.base.addClass([row_1], 'e-focused');
                            sf.base.addClass([row_1], 'e-focus');
                        }
                        else {
                            this.clearSelection();
                        }
                    }
                    break;
                case 'upArrow':
                    if (!this.enableVirtualization) {
                        parentTarget = e.target.parentElement;
                        summaryElement = this.findPreviousRowElement(parentTarget);
                        if (summaryElement !== null) {
                            var rIndex = summaryElement.rowIndex;
                            this.selectRow(rIndex);
                            var cIndex = e.target.cellIndex;
                            var rows = summaryElement.children[cIndex];
                            sf.base.addClass([rows], 'e-focused');
                            sf.base.addClass([rows], 'e-focus');
                        }
                        else {
                            this.clearSelection();
                        }
                    }
            }
        }
    };
    // Get Proper Row Element from the summary
    TreeGrid.prototype.findnextRowElement = function (summaryRowElement) {
        var rowElement = summaryRowElement.nextElementSibling;
        if (rowElement !== null && (rowElement.className.indexOf('e-summaryrow') !== -1 ||
            rowElement.style.display === 'none')) {
            rowElement = this.findnextRowElement(rowElement);
        }
        return rowElement;
    };
    // Get Proper Row Element from the summary
    TreeGrid.prototype.findPreviousRowElement = function (summaryRowElement) {
        var rowElement = summaryRowElement.previousElementSibling;
        if (rowElement !== null && (rowElement.className.indexOf('e-summaryrow') !== -1 ||
            rowElement.style.display === 'none')) {
            rowElement = this.findPreviousRowElement(rowElement);
        }
        return rowElement;
    };
    TreeGrid.prototype.initProperties = function () {
        this.defaultLocale = {};
        this.flatData = [];
        this.parentData = [];
        this.columnModel = [];
        this.isExpandAll = false;
        this.isCollapseAll = false;
        this.keyConfigs = {
            ctrlDownArrow: 'ctrl+downarrow',
            ctrlUpArrow: 'ctrl+uparrow',
            ctrlShiftUpArrow: 'ctrl+shift+uparrow',
            ctrlShiftDownArrow: 'ctrl+shift+downarrow',
            downArrow: 'downArrow',
            upArrow: 'upArrow'
        };
        this.isLocalData = (!(this.dataSource instanceof sf.data.DataManager) || this.dataSource.dataSource.offline
            || (!sf.base.isNullOrUndefined(this.dataSource.ready)) || this.dataSource.adaptor instanceof sf.data.RemoteSaveAdaptor);
        this.isSelfReference = !sf.base.isNullOrUndefined(this.parentIdMapping);
    };
    /**
     * Binding events to the element while component creation.
     *
     * @hidden
     * @returns {void}
     */
    TreeGrid.prototype.wireEvents = function () {
        sf.base.EventHandler.add(this.grid.element, 'click', this.mouseClickHandler, this);
        sf.base.EventHandler.add(this.element, 'touchend', this.mouseClickHandler, this);
        this.keyboardModule = new sf.base.KeyboardEvents(this.element, {
            keyAction: this.treeGridkeyActionHandler.bind(this),
            keyConfigs: this.keyConfigs,
            eventName: 'keydown'
        });
        if (this.allowKeyboard) {
            this.element.tabIndex = this.element.tabIndex === -1 ? 0 : this.element.tabIndex;
        }
    };
    /**
     * To provide the array of modules needed for component rendering
     *
     * @returns {ModuleDeclaration[]} - Returns TreeGrid modules collection
     * @hidden
     */
    TreeGrid.prototype.requiredModules = function () {
        var modules = [];
        var splitFrozenCount = 'splitFrozenCount';
        this.grid[splitFrozenCount](this.getGridColumns(this.columns));
        if (this.isDestroyed) {
            return modules;
        }
        modules.push({
            member: 'filter', args: [this, this.filterSettings]
        });
        if (!sf.base.isNullOrUndefined(this.toolbar)) {
            modules.push({
                member: 'toolbar',
                args: [this]
            });
        }
        if (this.contextMenuItems) {
            modules.push({
                member: 'contextMenu',
                args: [this]
            });
        }
        if (this.allowPaging) {
            modules.push({
                member: 'pager',
                args: [this, this.pageSettings]
            });
        }
        if (this.allowReordering) {
            modules.push({
                member: 'reorder',
                args: [this]
            });
        }
        if (this.allowSorting) {
            modules.push({
                member: 'sort',
                args: [this]
            });
        }
        if (this.aggregates.length > 0) {
            modules.push({
                member: 'summary', args: [this]
            });
        }
        modules.push({
            member: 'resize', args: [this]
        });
        if (this.allowExcelExport) {
            modules.push({
                member: 'ExcelExport', args: [this]
            });
        }
        if (this.frozenColumns || this.frozenRows || this.getFrozenColumns() || this.grid.getFrozenLeftColumnsCount() || this.grid.getFrozenRightColumnsCount()) {
            modules.push({
                member: 'freeze', args: [this]
            });
        }
        if (this.detailTemplate) {
            modules.push({
                member: 'detailRow', args: [this]
            });
        }
        if (this.allowPdfExport) {
            modules.push({
                member: 'PdfExport', args: [this]
            });
        }
        if (this.showColumnMenu) {
            modules.push({
                member: 'columnMenu', args: [this]
            });
        }
        if (this.showColumnChooser) {
            modules.push({
                member: 'ColumnChooser', args: [this]
            });
        }
        this.extendRequiredModules(modules);
        return modules;
    };
    TreeGrid.prototype.extendRequiredModules = function (modules) {
        if (this.allowRowDragAndDrop) {
            modules.push({
                member: 'rowDragAndDrop',
                args: [this]
            });
        }
        if (this.editSettings.allowAdding || this.editSettings.allowDeleting || this.editSettings.allowEditing) {
            modules.push({
                member: 'edit',
                args: [this]
            });
        }
        if (this.isCommandColumn(this.columns)) {
            modules.push({
                member: 'commandColumn',
                args: [this]
            });
        }
        if (this.allowSelection) {
            modules.push({
                member: 'selection',
                args: [this]
            });
        }
        if (this.enableVirtualization) {
            modules.push({
                member: 'virtualScroll',
                args: [this]
            });
        }
        if (this.enableInfiniteScrolling) {
            modules.push({
                member: 'infiniteScroll',
                args: [this]
            });
        }
        modules.push({
            member: 'logger',
            args: [this.grid]
        });
    };
    TreeGrid.prototype.isCommandColumn = function (columns) {
        var _this = this;
        return columns.some(function (col) {
            if (col.columns) {
                return _this.isCommandColumn(col.columns);
            }
            return !!(col.commands || col.commandsTemplate);
        });
    };
    /**
     * Unbinding events from the element while component destroy.
     *
     * @hidden
     * @returns {void}
     */
    TreeGrid.prototype.unwireEvents = function () {
        if (this.grid && this.grid.element) {
            sf.base.EventHandler.remove(this.grid.element, 'click', this.mouseClickHandler);
        }
    };
    /**
     * Logs tree grid error message on console
     *
     * @param {string | string[]} types - Tree Grid error type
     * @param {object} args - Error details
     * @hidden
     * @private
     * @returns {void}
     */
    TreeGrid.prototype.log = function (types, args) {
        if (this.loggerModule) {
            this.loggerModule.treeLog(types, args, this);
        }
    };
    /**
     * For internal use only - To Initialize the component rendering.
     *
     * @private
     * @returns {void}
     */
    TreeGrid.prototype.render = function () {
        var _this = this;
        if (this.isReact) {
            this.grid.isReact = true;
            this.grid.portals = [];
        }
        sf.popups.createSpinner({ target: this.element }, this.createElement);
        this.log(['mapping_fields_missing']);
        this.renderModule = new Render(this);
        this.dataModule = new DataManipulation(this);
        this.printModule = new Print$1(this);
        this.trigger(load);
        this.autoGenerateColumns();
        this.initialRender = true;
        if (!sf.base.isNullOrUndefined(this.dataSource)) {
            this.convertTreeData(this.dataSource);
        }
        this.loadGrid();
        if (this.element.classList.contains('e-treegrid') && this.rowDropSettings.targetID) {
            this.grid.rowDropSettings.targetID += '_gridcontrol';
        }
        this.addListener();
        var gridContainer = sf.base.createElement('div', { id: this.element.id + '_gridcontrol' });
        sf.base.addClass([this.element], 'e-treegrid');
        if (!sf.base.isNullOrUndefined(this.height) && typeof (this.height) === 'string' && this.height.indexOf('%') !== -1) {
            this.element.style.height = this.height;
        }
        if (!sf.base.isNullOrUndefined(this.width) && typeof (this.width) === 'string' && this.width.indexOf('%') !== -1) {
            this.element.style.width = this.width;
        }
        this.element.appendChild(gridContainer);
        var gridRequiredModules = this.grid.requiredModules;
        this.grid.requiredModules = function () {
            var modules = [];
            modules = gridRequiredModules.apply(this);
            for (var i = 0; i < modules.length; i++) {
                if (modules[i].member === 'virtualscroll') {
                    modules[i].member = 'treeVirtualScroll';
                }
            }
            return modules;
        };
        this.grid.appendTo(gridContainer);
        this.wireEvents();
        this.renderComplete();
        var destroyTemplate = 'destroyTemplate';
        var destroyTemplateFn = this.grid[destroyTemplate];
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        this.grid[destroyTemplate] = function (args, index) {
            destroyTemplateFn.apply(_this.grid);
            var portals = 'portals';
            if (!(_this.isReact && sf.base.isNullOrUndefined(_this[portals]))) {
                _this.clearTemplate(args, index);
            }
        };
    };
    TreeGrid.prototype.afterGridRender = function () {
        if (!sf.base.isNullOrUndefined(this.grid.clipboardModule)) {
            this.grid.clipboardModule.destroy();
        }
        this.clipboardModule = this.grid.clipboardModule = new TreeClipboard(this);
    };
    TreeGrid.prototype.convertTreeData = function (data) {
        var _this = this;
        if (isCountRequired(this)) {
            data = sf.base.getValue('result', data);
        }
        if (data instanceof Array && data.length > 0 && Object.prototype.hasOwnProperty.call(data[0], 'level')) {
            this.flatData = data;
            this.flatData.filter(function (e) {
                sf.base.setValue('uniqueIDCollection.' + e.uniqueID, e, _this);
                if (e.level === 0) {
                    _this.parentData.push(e);
                }
            });
        }
        else {
            if (isCountRequired(this)) {
                var griddata = sf.base.getValue('result', this.dataSource);
                this.dataModule.convertToFlatData(griddata);
            }
            else {
                this.dataModule.convertToFlatData(data);
            }
        }
    };
    // private getGridData(): Object {
    //   if (isRemoteData(this)) {
    //     return this.dataSource;
    //   } else if (this.isLocalData && this.dataSource instanceof DataManager) {
    //     this.dataSource.dataSource.json = this.flatData;
    //     return this.dataSource;
    //   }
    //   return this.flatData;
    // }
    TreeGrid.prototype.bindGridProperties = function () {
        this.bindedDataSource();
        this.grid.enableRtl = this.enableRtl;
        this.grid.allowKeyboard = this.allowKeyboard;
        this.grid.columns = this.getGridColumns(this.columns);
        this.grid.allowExcelExport = this.allowExcelExport;
        this.grid.allowPdfExport = this.allowPdfExport;
        this.grid.query = this.query;
        this.grid.columnQueryMode = this.columnQueryMode;
        this.grid.allowPaging = this.allowPaging;
        this.grid.pageSettings = sf.grids.getActualProperties(this.pageSettings);
        this.grid.pagerTemplate = this.pagerTemplate;
        this.grid.showColumnMenu = this.showColumnMenu;
        this.grid.allowSorting = this.allowSorting;
        this.grid.allowFiltering = this.allowFiltering;
        this.grid.enableVirtualization = this.enableVirtualization;
        this.grid.enableInfiniteScrolling = this.enableInfiniteScrolling;
        this.grid.infiniteScrollSettings = this.infiniteScrollSettings;
        this.grid.width = this.width;
        this.grid.height = this.height;
        this.grid.enableAltRow = this.enableAltRow;
        this.grid.allowReordering = this.allowReordering;
        this.grid.allowTextWrap = this.allowTextWrap;
        this.grid.allowResizing = this.allowResizing;
        this.grid.enableHover = this.enableHover;
        this.grid.enableAutoFill = this.enableAutoFill;
        this.grid.enableImmutableMode = this.enableImmutableMode;
        this.grid.allowRowDragAndDrop = this.allowRowDragAndDrop;
        this.grid.rowDropSettings = sf.grids.getActualProperties(this.rowDropSettings);
        this.grid.rowHeight = this.rowHeight;
        this.grid.gridLines = this.gridLines;
        this.grid.allowSelection = this.allowSelection;
        this.grid.toolbar = sf.grids.getActualProperties(this.getGridToolbar());
        this.grid.toolbarTemplate = this.toolbarTemplate;
        this.grid.showColumnChooser = this.showColumnChooser;
        this.grid.filterSettings = sf.grids.getActualProperties(this.filterSettings);
        this.grid.selectionSettings = sf.grids.getActualProperties(this.selectionSettings);
        this.grid.sortSettings = sf.grids.getActualProperties(this.sortSettings);
        this.grid.searchSettings = sf.grids.getActualProperties(this.searchSettings);
        this.grid.aggregates = sf.grids.getActualProperties(this.aggregates);
        this.grid.textWrapSettings = sf.grids.getActualProperties(this.textWrapSettings);
        this.grid.printMode = sf.grids.getActualProperties(this.printMode);
        this.grid.locale = sf.grids.getActualProperties(this.locale);
        this.grid.selectedRowIndex = this.selectedRowIndex;
        this.grid.contextMenuItems = sf.grids.getActualProperties(this.getContextMenu());
        this.grid.columnMenuItems = sf.grids.getActualProperties(this.columnMenuItems);
        this.grid.editSettings = this.getGridEditSettings();
        this.grid.rowTemplate = sf.grids.getActualProperties(this.rowTemplate);
        this.grid.detailTemplate = sf.grids.getActualProperties(this.detailTemplate);
        this.grid.frozenRows = this.frozenRows;
        this.grid.frozenColumns = this.frozenColumns;
        this.grid.clipMode = sf.grids.getActualProperties(this.clipMode);
        var templateInstance = 'templateDotnetInstance';
        this.grid[templateInstance] = this[templateInstance];
        var isJsComponent = 'isJsComponent';
        this.grid[isJsComponent] = true;
    };
    TreeGrid.prototype.triggerEvents = function (args) {
        this.trigger(sf.grids.getObject('name', args), args);
    };
    TreeGrid.prototype.bindGridEvents = function () {
        var _this = this;
        this.grid.rowSelecting = function (args) {
            if (!sf.base.isNullOrUndefined(args.target) && (args.target.classList.contains('e-treegridexpand')
                || args.target.classList.contains('e-treegridcollapse') || args.target.classList.contains('e-summarycell'))) {
                args.cancel = true;
                return;
            }
            _this.trigger(rowSelecting, args);
        };
        this.grid.rowSelected = function (args) {
            _this.selectedRowIndex = _this.grid.selectedRowIndex;
            _this.notify(rowSelected, args);
            _this.trigger(rowSelected, args);
        };
        this.grid.rowDeselected = function (args) {
            _this.selectedRowIndex = _this.grid.selectedRowIndex;
            _this.trigger(rowDeselected, args);
        };
        this.grid.resizeStop = function (args) {
            _this.updateColumnModel();
            _this.trigger(resizeStop, args);
        };
        this.grid.excelQueryCellInfo = function (args) {
            _this.notify('excelCellInfo', args);
            args = _this.dataResults;
        };
        this.grid.pdfQueryCellInfo = function (args) {
            _this.notify('pdfCellInfo', args);
            args = _this.dataResults;
        };
        this.grid.checkBoxChange = function (args) {
            _this.trigger(checkboxChange, args);
        };
        this.grid.pdfExportComplete = this.triggerEvents.bind(this);
        this.grid.excelExportComplete = this.triggerEvents.bind(this);
        this.grid.excelHeaderQueryCellInfo = this.triggerEvents.bind(this);
        this.grid.pdfHeaderQueryCellInfo = this.triggerEvents.bind(this);
        this.grid.dataSourceChanged = this.triggerEvents.bind(this);
        this.grid.recordDoubleClick = this.triggerEvents.bind(this);
        this.grid.rowDeselecting = this.triggerEvents.bind(this);
        this.grid.cellDeselected = this.triggerEvents.bind(this);
        this.grid.cellDeselecting = this.triggerEvents.bind(this);
        this.grid.columnMenuOpen = this.triggerEvents.bind(this);
        this.grid.columnMenuClick = this.triggerEvents.bind(this);
        this.grid.cellSelected = this.triggerEvents.bind(this);
        this.grid.headerCellInfo = this.triggerEvents.bind(this);
        this.grid.resizeStart = this.triggerEvents.bind(this);
        this.grid.resizing = this.triggerEvents.bind(this);
        this.grid.columnDrag = this.triggerEvents.bind(this);
        this.grid.columnDragStart = this.triggerEvents.bind(this);
        this.grid.columnDrop = this.triggerEvents.bind(this);
        this.grid.beforePrint = this.triggerEvents.bind(this);
        this.grid.beforeCopy = this.triggerEvents.bind(this);
        this.grid.beforePaste = function (args) {
            var rows = _this.getRows();
            var rowIndex = 'rowIndex';
            while (rows[args[rowIndex]].classList.contains('e-summaryrow')) {
                args[rowIndex]++;
            }
            _this.trigger(beforePaste, args);
        };
        this.grid.load = function () {
            _this.grid.on('initial-end', _this.afterGridRender, _this);
            if (!sf.base.isNullOrUndefined(_this.loggerModule)) {
                var loggerModule = 'loggerModule';
                _this.loggerModule = _this.grid[loggerModule] = new Logger$1(_this.grid);
            }
        };
        this.grid.printComplete = this.triggerEvents.bind(this);
        this.grid.actionFailure = this.triggerEvents.bind(this);
        this.extendedGridDataBoundEvent();
        this.extendedGridEvents();
        this.extendedGridActionEvents();
        this.extendedGridEditEvents();
        this.bindGridDragEvents();
        this.bindCallBackEvents();
    };
    TreeGrid.prototype.lastRowBorder = function (visiblerow, isAddBorder) {
        for (var j = 0; j < visiblerow.cells.length; j++) {
            if (isAddBorder) {
                sf.base.addClass([visiblerow.cells[j]], 'e-lastrowcell');
            }
            else {
                sf.base.removeClass([visiblerow.cells[j]], 'e-lastrowcell');
            }
        }
    };
    TreeGrid.prototype.isPixelHeight = function () {
        if (this.height !== 'auto' && this.height.toString().indexOf('%') === -1) {
            return true;
        }
        else {
            return false;
        }
    };
    TreeGrid.prototype.extendedGridDataBoundEvent = function () {
        var _this = this;
        this.grid.dataBound = function (args) {
            _this.updateRowTemplate();
            _this.updateColumnModel();
            _this.updateAltRow(_this.getRows());
            _this.notify('dataBoundArg', args);
            if (isRemoteData(_this) && !isOffline(_this) && !_this.hasChildMapping) {
                var req = sf.grids.getObject('dataSource.requests', _this).filter(function (e) {
                    return e.httpRequest.statusText !== 'OK';
                }).length;
                sf.base.setValue('grid.contentModule.isLoaded', !(req > 0), _this);
            }
            if (_this.isPixelHeight() && _this.initialRender) {
                var rows = _this.getContentTable().rows;
                var totalRows = [].slice.call(rows);
                for (var i = totalRows.length - 1; i > 0; i--) {
                    if (!isHidden(totalRows[i])) {
                        if (totalRows[i].nextElementSibling) {
                            _this.lastRowBorder(totalRows[i], true);
                        }
                        break;
                    }
                }
            }
            _this.trigger(dataBound, args);
            _this.initialRender = false;
        };
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        var treeGrid = this;
        this.grid.beforeDataBound = function (args) {
            var dataSource = 'dataSource';
            var requestType = sf.grids.getObject('action', args);
            if (((isRemoteData(treeGrid) && !isOffline(treeGrid)) || isCountRequired(this)) && requestType !== 'edit') {
                treeGrid.notify('updateRemoteLevel', args);
                args = (treeGrid.dataResults);
            }
            else if (treeGrid.flatData.length === 0 && isOffline(treeGrid) && treeGrid.dataSource instanceof sf.data.DataManager) {
                var dm = treeGrid.dataSource;
                treeGrid.dataModule.convertToFlatData(dm.dataSource.json);
                args.result = treeGrid.grid.dataSource[dataSource].json = treeGrid.flatData;
            }
            if (!isRemoteData(treeGrid) && !isCountRequired(this) && !sf.base.isNullOrUndefined(treeGrid.dataSource)) {
                if (this.isPrinting) {
                    sf.base.setValue('isPrinting', true, args);
                }
                treeGrid.notify('dataProcessor', args);
                //args = treeGrid.dataModule.dataProcessor(args);
            }
            sf.base.extend(args, treeGrid.dataResults);
            if (treeGrid.enableImmutableMode) {
                args.result = args.result.slice();
            }
            if (treeGrid.initialRender) {
                this.contentModule.objectEqualityChecker = treeGrid.objectEqualityChecker;
            }
            // treeGrid.notify(events.beforeDataBound, args);
            if (!this.isPrinting) {
                var callBackPromise_1 = new sf.data.Deferred();
                treeGrid.trigger(beforeDataBound, args, function (beforeDataBoundArgs) {
                    callBackPromise_1.resolve(beforeDataBoundArgs);
                });
                return callBackPromise_1;
            }
        };
        this.grid.log = function (type, args) {
            if (_this.loggerModule) {
                _this.loggerModule.log(type, args);
            }
        };
    };
    TreeGrid.prototype.bindCallBackEvents = function () {
        var _this = this;
        this.grid.toolbarClick = function (args) {
            if ((args.item.id === _this.grid.element.id + '_excelexport' && _this.allowExcelExport === false) ||
                (args.item.id === _this.grid.element.id + '_pdfexport' && _this.allowPdfExport === false) ||
                (args.item.id === _this.grid.element.id + '_csvexport' && _this.allowExcelExport === false)) {
                return;
            }
            var callBackPromise = new sf.data.Deferred();
            _this.trigger(toolbarClick, args, function (toolbarargs) {
                if (!toolbarargs.cancel) {
                    _this.notify(toolbarClick, args);
                }
                callBackPromise.resolve(toolbarargs);
            });
            return callBackPromise;
        };
        this.grid.cellSelecting = function (args) {
            var callBackPromise = new sf.data.Deferred();
            _this.trigger(sf.grids.getObject('name', args), args, function (cellselectingArgs) {
                callBackPromise.resolve(cellselectingArgs);
            });
            return callBackPromise;
        };
        this.grid.beginEdit = function (args) {
            if (!sf.base.isNullOrUndefined(args.row) && args.row.classList.contains('e-summaryrow')) {
                args.cancel = true;
                return;
            }
            var callBackPromise = new sf.data.Deferred();
            _this.trigger(beginEdit, args, function (begineditArgs) {
                callBackPromise.resolve(begineditArgs);
            });
            return callBackPromise;
        };
    };
    TreeGrid.prototype.extendedGridEditEvents = function () {
        var _this = this;
        this.grid.dataStateChange = function (args) {
            if (_this.isExpandRefresh) {
                _this.isExpandRefresh = false;
                _this.grid.dataSource = { result: _this.flatData, count: sf.base.getValue('count', _this.grid.dataSource) };
            }
            else {
                _this.trigger(dataStateChange, args);
            }
        };
        this.grid.cellSave = function (args) {
            if (_this.grid.isContextMenuOpen()) {
                var contextitems = _this.grid.contextMenuModule.contextMenu.element.getElementsByClassName('e-selected')[0];
                if ((sf.base.isNullOrUndefined(contextitems) || contextitems.id !== _this.element.id + '_gridcontrol_cmenu_Save')) {
                    args.cancel = true;
                }
            }
            var callBackPromise = new sf.data.Deferred();
            _this.trigger(cellSave, args, function (cellsaveArgs) {
                if (!cellsaveArgs.cancel) {
                    _this.notify(cellSave, cellsaveArgs);
                }
                callBackPromise.resolve(cellsaveArgs);
            });
            return callBackPromise;
        };
        this.grid.cellSaved = function (args) {
            _this.trigger(cellSaved, args);
            _this.notify(cellSaved, args);
        };
        this.grid.cellEdit = function (args) {
            var prom = 'promise';
            var promise = new sf.data.Deferred();
            args[prom] = promise;
            _this.notify(cellEdit, args);
            return promise;
        };
        this.grid.batchAdd = function (args) {
            _this.trigger(batchAdd, args);
            _this.notify(batchAdd, args);
        };
        this.grid.beforeBatchSave = function (args) {
            _this.trigger(beforeBatchSave, args);
            _this.notify(beforeBatchSave, args);
        };
        this.grid.beforeBatchAdd = function (args) {
            _this.trigger(beforeBatchAdd, args);
            _this.notify(beforeBatchAdd, args);
        };
        this.grid.batchDelete = function (args) {
            _this.trigger(batchDelete, args);
            _this.notify(batchDelete, args);
        };
        this.grid.beforeBatchDelete = function (args) {
            _this.trigger(beforeBatchDelete, args);
            _this.notify(beforeBatchDelete, args);
        };
        this.grid.batchCancel = function (args) {
            if (_this.editSettings.mode !== 'Cell') {
                _this.trigger(batchCancel, args);
            }
            _this.notify(batchCancel, args);
        };
    };
    TreeGrid.prototype.updateRowTemplate = function () {
        this.treeColumnRowTemplate();
    };
    TreeGrid.prototype.bindedDataSource = function () {
        var dataSource = 'dataSource';
        var isDataAvailable = 'isDataAvailable';
        var adaptor = 'adaptor';
        var ready = 'ready';
        if (this.dataSource && isCountRequired(this)) {
            var data = this.flatData;
            var datacount = sf.base.getValue('count', this.dataSource);
            this.grid.dataSource = { result: data, count: datacount };
        }
        else {
            this.grid.dataSource = !(this.dataSource instanceof sf.data.DataManager) ?
                this.flatData : new sf.data.DataManager(this.dataSource.dataSource, this.dataSource.defaultQuery, this.dataSource.adaptor);
        }
        if (this.dataSource instanceof sf.data.DataManager && (this.dataSource.dataSource.offline || this.dataSource.ready)) {
            this.grid.dataSource[dataSource].json = extendArray(this.dataSource[dataSource].json);
            this.grid.dataSource[ready] = this.dataSource.ready;
            // eslint-disable-next-line @typescript-eslint/no-this-alias
            var proxy_1 = this;
            if (!sf.base.isNullOrUndefined(this.grid.dataSource[ready])) {
                this.grid.dataSource[ready].then(function (e) {
                    var dm = proxy_1.grid.dataSource;
                    dm[dataSource].offline = true;
                    dm[isDataAvailable] = true;
                    dm[dataSource].json = e.result;
                    dm[adaptor] = new sf.data.JsonAdaptor();
                });
            }
        }
    };
    TreeGrid.prototype.extendedGridActionEvents = function () {
        var _this = this;
        this.grid.actionBegin = function (args) {
            if (args.requestType === 'sorting' && args.target && args.target.parentElement &&
                args.target.parentElement.classList.contains('e-hierarchycheckbox')) {
                args.cancel = true;
            }
            var requestType = sf.grids.getObject('requestType', args);
            if (requestType === 'reorder') {
                _this.notify('getColumnIndex', {});
            }
            _this.notify('actionBegin', { editAction: args });
            if (!isRemoteData(_this) && !sf.base.isNullOrUndefined(_this.filterModule) && !isCountRequired(_this)
                && (_this.grid.filterSettings.columns.length === 0 || _this.grid.searchSettings.key.length === 0)) {
                _this.notify('clearFilters', { flatData: _this.grid.dataSource });
                _this.grid.setProperties({ dataSource: _this.dataResults.result }, true);
            }
            var callBackPromise = new sf.data.Deferred();
            _this.trigger(actionBegin, args, function (actionArgs) {
                if (!actionArgs.cancel) {
                    _this.notify(beginEdit, actionArgs);
                }
                callBackPromise.resolve(actionArgs);
            });
            return callBackPromise;
        };
        this.grid.actionComplete = function (args) {
            _this.notify('actioncomplete', args);
            _this.updateColumnModel();
            _this.updateTreeGridModel();
            if (args.requestType === 'reorder') {
                _this.notify('setColumnIndex', {});
            }
            _this.notify('actionComplete', { editAction: args });
            if (args.requestType === 'add' && (_this.editSettings.newRowPosition !== 'Top' && _this.editSettings.newRowPosition !== 'Bottom')) {
                _this.notify(beginAdd, args);
            }
            if (args.requestType === 'batchsave') {
                _this.notify(batchSave, args);
            }
            _this.notify('updateGridActions', args);
            if (args.requestType === 'save' && _this.aggregates.map(function (ag) { return ag.showChildSummary == true; }).length) {
                _this.grid.refresh();
            }
            _this.trigger(actionComplete, args);
        };
    };
    TreeGrid.prototype.extendedGridEvents = function () {
        var _this = this;
        // eslint-disable-next-line @typescript-eslint/no-this-alias
        var treeGrid = this;
        this.grid.recordDoubleClick = function (args) {
            _this.trigger(recordDoubleClick, args);
            _this.notify(recordDoubleClick, args);
        };
        this.grid.detailDataBound = function (args) {
            _this.notify('detaildataBound', args);
            _this.trigger(detailDataBound, args);
        };
        this.grid.rowDataBound = function (args) {
            if (sf.base.isNullOrUndefined(this.isPrinting)) {
                sf.base.setValue('isPrinting', false, args);
            }
            else {
                sf.base.setValue('isPrinting', this.isPrinting, args);
            }
            treeGrid.renderModule.RowModifier(args);
        };
        this.grid.queryCellInfo = function (args) {
            if (sf.base.isNullOrUndefined(this.isPrinting)) {
                sf.base.setValue('isPrinting', false, args);
            }
            else {
                sf.base.setValue('isPrinting', this.isPrinting, args);
            }
            treeGrid.renderModule.cellRender(args);
        };
        this.grid.contextMenuClick = function (args) {
            _this.notify(contextMenuClick, args);
            _this.trigger(contextMenuClick, args);
        };
        this.grid.contextMenuOpen = function (args) {
            _this.notify(contextMenuOpen, args);
            _this.trigger(contextMenuOpen, args);
        };
        this.grid.queryCellInfo = function (args) {
            _this.renderModule.cellRender(args);
        };
    };
    TreeGrid.prototype.bindGridDragEvents = function () {
        var _this = this;
        this.grid.rowDragStartHelper = function (args) {
            _this.trigger(rowDragStartHelper, args);
        };
        this.grid.rowDragStart = function (args) {
            _this.trigger(rowDragStart, args);
        };
        this.grid.rowDrag = function (args) {
            if (_this.grid.isEdit) {
                args.cancel = true;
                return;
            }
            _this.notify(rowdraging, args);
            _this.trigger(rowDrag, args);
        };
        this.grid.rowDrop = function (args) {
            if (_this.grid.isEdit) {
                args.cancel = true;
                return;
            }
            _this.notify(rowDropped, args);
            args.cancel = true;
        };
    };
    /**
     * Renders TreeGrid component
     *
     * @private
     * @returns {void}
     */
    TreeGrid.prototype.loadGrid = function () {
        this.bindGridProperties();
        this.bindGridEvents();
        sf.base.setValue('registeredTemplate', this.registeredTemplate, this.grid);
        var ref = 'viewContainerRef';
        sf.base.setValue('viewContainerRef', this[ref], this.grid);
    };
    /**
     * AutoGenerate TreeGrid columns from first record
     *
     * @hidden
     * @returns {void}
     */
    TreeGrid.prototype.autoGenerateColumns = function () {
        if (!this.columns.length && (!this.dataModule.isRemote() && Object.keys(this.dataSource).length)) {
            this.columns = [];
            // if (this.dataSource instanceof DataManager) {
            //   record = (<DataManager>this.dataSource).dataSource.json[0];
            // } else {
            var record = this.dataSource[0];
            // }
            var keys = Object.keys(record);
            for (var i = 0; i < keys.length; i++) {
                if ([this.childMapping, this.parentIdMapping].indexOf(keys[i]) === -1) {
                    this.columns.push(keys[i]);
                }
            }
        }
    };
    TreeGrid.prototype.getGridEditSettings = function () {
        var edit = {};
        var guid = 'guid';
        edit.allowAdding = this.editSettings.allowAdding;
        edit.allowEditing = this.editSettings.allowEditing;
        edit.allowDeleting = this.editSettings.allowDeleting;
        edit.newRowPosition = this.editSettings.newRowPosition === 'Bottom' ? 'Bottom' : 'Top';
        edit.allowEditOnDblClick = this.editSettings.allowEditOnDblClick;
        edit.showConfirmDialog = this.editSettings.showConfirmDialog;
        edit.template = this.editSettings.template;
        edit.showDeleteConfirmDialog = this.editSettings.showDeleteConfirmDialog;
        edit.allowNextRowEdit = this.editSettings.allowNextRowEdit;
        edit[guid] = this.editSettings[guid];
        edit.dialog = this.editSettings.dialog;
        switch (this.editSettings.mode) {
            case 'Dialog':
                edit.mode = this.editSettings.mode;
                break;
            case 'Batch':
                edit.mode = this.editSettings.mode;
                break;
            case 'Row':
                edit.mode = 'Normal';
                break;
            case 'Cell':
                edit.mode = 'Normal';
                edit.showConfirmDialog = false;
                break;
        }
        return edit;
    };
    /**
     * Defines grid toolbar from treegrid toolbar model
     *
     * @hidden
     * @returns {Object[]} - returns context menu items
     */
    TreeGrid.prototype.getContextMenu = function () {
        if (this.contextMenuItems) {
            var items = [];
            for (var i = 0; i < this.contextMenuItems.length; i++) {
                switch (this.contextMenuItems[i]) {
                    case 'AddRow':
                    case exports.ContextMenuItems.AddRow:
                        items.push({ text: this.l10n.getConstant('AddRow'),
                            target: '.e-content', id: this.element.id + '_gridcontrol_cmenu_AddRow',
                            items: [{ text: this.l10n.getConstant('Above'), id: 'Above' }, { text: this.l10n.getConstant('Below'), id: 'Below' }, { text: this.l10n.getConstant('Child'), id: 'Child' }] });
                        break;
                    default:
                        items.push(this.contextMenuItems[i]);
                }
            }
            return items;
        }
        else {
            return null;
        }
    };
    /**
     * Defines grid toolbar from treegrid toolbar model
     *
     * @hidden
     * @returns {Object[]} - Returns toolbar items
     */
    TreeGrid.prototype.getGridToolbar = function () {
        if (this.toolbar) {
            var items = [];
            var tooltipText = void 0;
            for (var i = 0; i < this.toolbar.length; i++) {
                switch (this.toolbar[i]) {
                    case 'Search':
                    case exports.ToolbarItem.Search:
                        items.push('Search');
                        break;
                    case 'Print':
                    case exports.ToolbarItem.Print:
                        items.push('Print');
                        break;
                    case 'ExpandAll':
                    case exports.ToolbarItem.ExpandAll:
                        tooltipText = this.l10n.getConstant('ExpandAll');
                        items.push({ text: tooltipText, tooltipText: tooltipText,
                            prefixIcon: 'e-expand', id: this.element.id + '_gridcontrol_expandall' });
                        break;
                    case 'CollapseAll':
                    case exports.ToolbarItem.CollapseAll:
                        tooltipText = this.l10n.getConstant('CollapseAll');
                        items.push({ text: tooltipText,
                            tooltipText: tooltipText, prefixIcon: 'e-collapse', id: this.element.id + '_gridcontrol_collapseall'
                        });
                        break;
                    case 'Indent':
                    case exports.ToolbarItem.RowIndent:
                        tooltipText = this.l10n.getConstant('RowIndent');
                        items.push({
                            text: tooltipText, tooltipText: tooltipText,
                            prefixIcon: 'e-indent', id: this.element.id + '_gridcontrol_indent'
                        });
                        break;
                    case 'Outdent':
                    case exports.ToolbarItem.RowOutdent:
                        tooltipText = this.l10n.getConstant('RowOutdent');
                        items.push({
                            text: tooltipText, tooltipText: tooltipText,
                            prefixIcon: 'e-outdent', id: this.element.id + '_gridcontrol_outdent'
                        });
                        break;
                    default:
                        items.push(this.toolbar[i]);
                }
            }
            return items;
        }
        else {
            return null;
        }
    };
    TreeGrid.prototype.getGridColumns = function (columns, isEmptyColumnModel, index) {
        if (isEmptyColumnModel === void 0) { isEmptyColumnModel = true; }
        if (index === void 0) { index = 0; }
        var column = columns;
        var stackedColumn = 'columns';
        if (isEmptyColumnModel) {
            this.columnModel = [];
        }
        var treeGridColumn;
        var gridColumn;
        index = index === 0 ? -1 : index;
        var gridColumnCollection = [];
        for (var i = 0; i < column.length; i++) {
            index = index + 1;
            var treeColumn = this.grid.getColumnByUid(column[i].uid);
            gridColumn = treeColumn ? treeColumn : {};
            treeGridColumn = {};
            if (typeof this.columns[i] === 'string') {
                gridColumn.field = treeGridColumn.field = this.columns[i];
            }
            else {
                for (var _i = 0, _a = Object.keys(column[i]); _i < _a.length; _i++) {
                    var prop = _a[_i];
                    if (index === this.treeColumnIndex && prop === 'template') {
                        treeGridColumn[prop] = column[i][prop];
                    }
                    else if (prop === 'columns' && !sf.base.isNullOrUndefined(column[i][prop])) {
                        gridColumn[prop] = this.getGridColumns(column[i][prop], false, index);
                        treeGridColumn[prop] = column[i][prop];
                    }
                    else {
                        gridColumn[prop] = treeGridColumn[prop] = column[i][prop];
                    }
                }
            }
            if (!treeGridColumn[stackedColumn]) {
                this.columnModel.push(new Column(treeGridColumn));
            }
            gridColumnCollection.push(gridColumn);
        }
        return gridColumnCollection;
    };
    /**
     * Called internally if any of the property value changed.
     *
     * @param {TreeGridModel} newProp - properties details which has to be modified
     * @hidden
     * @returns {void}
     */
    TreeGrid.prototype.onPropertyChanged = function (newProp) {
        var properties = Object.keys(newProp);
        var requireRefresh = false;
        for (var _i = 0, properties_1 = properties; _i < properties_1.length; _i++) {
            var prop = properties_1[_i];
            switch (prop) {
                case 'columns':
                    this.grid.columns = this.getGridColumns(this.columns);
                    break;
                case 'treeColumnIndex':
                    this.grid.refreshColumns();
                    break;
                case 'allowPaging':
                    this.grid.allowPaging = this.allowPaging;
                    break;
                case 'pageSettings':
                    this.grid.pageSettings = sf.grids.getActualProperties(this.pageSettings);
                    requireRefresh = true;
                    break;
                case 'enableVirtualization':
                    this.grid.enableVirtualization = this.enableVirtualization;
                    break;
                case 'toolbar':
                    this.grid.toolbar = this.getGridToolbar();
                    break;
                case 'allowSelection':
                    this.grid.allowSelection = this.allowSelection;
                    break;
                case 'selectionSettings':
                    this.grid.selectionSettings = sf.grids.getActualProperties(this.selectionSettings);
                    break;
                case 'allowSorting':
                    this.grid.allowSorting = this.allowSorting;
                    break;
                case 'allowMultiSorting':
                    this.grid.allowMultiSorting = this.allowMultiSorting;
                    break;
                case 'sortSettings':
                    this.grid.sortSettings = sf.grids.getActualProperties(this.sortSettings);
                    break;
                case 'searchSettings':
                    this.grid.searchSettings = sf.grids.getActualProperties(this.searchSettings);
                    break;
                case 'allowFiltering':
                    this.grid.allowFiltering = this.allowFiltering;
                    break;
                case 'filterSettings':
                    if (!this.initialRender) {
                        this.grid.filterSettings = sf.grids.getActualProperties(this.filterSettings);
                    }
                    break;
                case 'showColumnMenu':
                    this.grid.showColumnMenu = this.showColumnMenu;
                    break;
                case 'allowRowDragAndDrop':
                    this.grid.allowRowDragAndDrop = this.allowRowDragAndDrop;
                    break;
                case 'aggregates':
                    this.grid.aggregates = sf.grids.getActualProperties(this.aggregates);
                    break;
                case 'enableInfiniteScrolling':
                    this.grid.enableInfiniteScrolling = this.enableInfiniteScrolling;
                    break;
                case 'dataSource':
                    this.isLocalData = (!(this.dataSource instanceof sf.data.DataManager) || (!sf.base.isNullOrUndefined(this.dataSource.ready))
                        || this.dataSource.adaptor instanceof sf.data.RemoteSaveAdaptor);
                    this.convertTreeData(this.dataSource);
                    if (this.isLocalData) {
                        if (isCountRequired(this)) {
                            var count = sf.base.getValue('count', this.dataSource);
                            this.grid.dataSource = { result: this.flatData, count: count };
                        }
                        else {
                            var data = this.dataSource;
                            this.grid.dataSource = !(data instanceof sf.data.DataManager) ?
                                this.flatData : new sf.data.DataManager(data.dataSource, data.defaultQuery, data.adaptor);
                        }
                        if (this.enableVirtualization) {
                            this.grid.contentModule.isDataSourceChanged = true;
                        }
                    }
                    else {
                        this.bindedDataSource();
                        if (this.enableVirtualization) {
                            this.grid.contentModule.removeEventListener();
                            this.grid.contentModule.eventListener('on');
                            this.grid.contentModule.renderTable();
                        }
                    }
                    break;
                case 'query':
                    this.grid.query = this.query;
                    break;
                case 'enableCollapseAll':
                    if (newProp[prop]) {
                        this.collapseAll();
                    }
                    else {
                        this.expandAll();
                    }
                    break;
                case 'expandStateMapping':
                    this.grid.refresh();
                    break;
                case 'gridLines':
                    this.grid.gridLines = this.gridLines;
                    break;
                case 'rowTemplate':
                    this.grid.rowTemplate = sf.grids.getActualProperties(this.rowTemplate);
                    break;
                case 'frozenRows':
                    this.grid.frozenRows = this.frozenRows;
                    break;
                case 'frozenColumns':
                    this.grid.frozenColumns = this.frozenColumns;
                    break;
                case 'rowHeight':
                    this.grid.rowHeight = this.rowHeight;
                    break;
                case 'height':
                    if (!sf.base.isNullOrUndefined(this.height) && typeof (this.height) === 'string' && this.height.indexOf('%') !== -1) {
                        this.element.style.height = this.height;
                    }
                    this.grid.height = this.height;
                    break;
                case 'width':
                    if (!sf.base.isNullOrUndefined(this.width) && typeof (this.width) === 'string' && this.width.indexOf('%') !== -1) {
                        this.element.style.width = this.width;
                    }
                    this.grid.width = this.width;
                    break;
                case 'locale':
                    this.grid.locale = this.locale;
                    break;
                case 'selectedRowIndex':
                    this.grid.selectedRowIndex = this.selectedRowIndex;
                    break;
                case 'enableAltRow':
                    this.grid.enableAltRow = this.enableAltRow;
                    break;
                case 'enableHover':
                    this.grid.enableHover = this.enableHover;
                    break;
                case 'enableAutoFill':
                    this.grid.enableAutoFill = this.enableAutoFill;
                    break;
                case 'enableImmutableMode':
                    this.grid.enableImmutableMode = this.enableImmutableMode;
                    break;
                case 'allowExcelExport':
                    this.grid.allowExcelExport = this.allowExcelExport;
                    break;
                case 'allowPdfExport':
                    this.grid.allowPdfExport = this.allowPdfExport;
                    break;
                case 'enableRtl':
                    this.grid.enableRtl = this.enableRtl;
                    break;
                case 'allowReordering':
                    this.grid.allowReordering = this.allowReordering;
                    break;
                case 'allowResizing':
                    this.grid.allowResizing = this.allowResizing;
                    break;
                case 'textWrapSettings':
                    this.grid.textWrapSettings = sf.grids.getActualProperties(this.textWrapSettings);
                    break;
                case 'allowTextWrap':
                    this.grid.allowTextWrap = sf.grids.getActualProperties(this.allowTextWrap);
                    this.grid.refresh();
                    break;
                case 'contextMenuItems':
                    this.grid.contextMenuItems = this.getContextMenu();
                    break;
                case 'showColumnChooser':
                    this.grid.showColumnChooser = this.showColumnChooser;
                    break;
                case 'detailTemplate':
                    this.grid.detailTemplate = sf.grids.getActualProperties(this.detailTemplate);
                    break;
                case 'columnMenuItems':
                    this.grid.columnMenuItems = sf.grids.getActualProperties(this.columnMenuItems);
                    break;
                case 'editSettings':
                    if (this.grid.isEdit && this.grid.editSettings.mode === 'Normal' && newProp[prop].mode &&
                        (newProp[prop].mode === 'Cell' || newProp[prop].mode === 'Row')) {
                        this.grid.closeEdit();
                    }
                    this.grid.editSettings = this.getGridEditSettings();
                    break;
            }
            if (requireRefresh) {
                this.grid.refresh();
            }
        }
    };
    /**
     * Destroys the component (detaches/removes all event handlers, attributes, classes, and empties the component element).
     *
     * @method destroy
     * @returns {void}
     */
    TreeGrid.prototype.destroy = function () {
        var treeGridElement = this.element;
        if (!treeGridElement) {
            return;
        }
        var hasTreeGridChild = treeGridElement.querySelector('.' + 'e-gridheader') &&
            treeGridElement.querySelector('.' + 'e-gridcontent') ? true : false;
        if (hasTreeGridChild) {
            this.unwireEvents();
        }
        this.removeListener();
        if (hasTreeGridChild) {
            _super.prototype.destroy.call(this);
        }
        if (this.grid) {
            this.grid.destroy();
        }
        if (this.dataModule) {
            this.dataModule.destroy();
        }
        var modules = ['dataModule', 'sortModule', 'renderModule', 'filterModule', 'printModule', 'clipboardModule',
            'excelExportModule', 'pdfExportModule', 'toolbarModule', 'summaryModule', 'reorderModule', 'resizeModule',
            'pagerModule', 'keyboardModule', 'columnMenuModule', 'contextMenuModule', 'editModule', 'virtualScrollModule',
            'selectionModule', 'detailRow', 'rowDragAndDropModule', 'freezeModule'];
        for (var i = 0; i < modules.length; i++) {
            if (this[modules[i]]) {
                this[modules[i]] = null;
            }
        }
        this.element.innerHTML = '';
        this.grid = null;
    };
    /**
     * Update the TreeGrid model
     *
     * @method dataBind
     * @returns {void}
     * @private
     */
    TreeGrid.prototype.dataBind = function () {
        if (sf.base.isNullOrUndefined(this.grid)) {
            return;
        }
        _super.prototype.dataBind.call(this);
        this.grid.dataBind();
    };
    /**
     * Get the properties to be maintained in the persisted state.
     *
     * @returns {string} - Returns persist properties details
     * @hidden
     */
    TreeGrid.prototype.getPersistData = function () {
        var keyEntity = ['pageSettings', 'sortSettings',
            'filterSettings', 'columns', 'searchSettings', 'selectedRowIndex'];
        var ignoreOnPersist = {
            pageSettings: ['template', 'pageSizes', 'pageSizeMode', 'enableQueryString', 'totalRecordsCount', 'pageCount'],
            filterSettings: ['type', 'mode', 'showFilterBarStatus', 'immediateModeDelay', 'ignoreAccent', 'hierarchyMode'],
            searchSettings: ['fields', 'operator', 'ignoreCase'],
            sortSettings: [], columns: [], selectedRowIndex: []
        };
        var ignoreOnColumn = ['filter', 'edit', 'filterBarTemplate', 'headerTemplate', 'template',
            'commandTemplate', 'commands', 'dataSource'];
        for (var i = 0; i < keyEntity.length; i++) {
            var currentObject = this[keyEntity[i]];
            for (var _i = 0, _a = ignoreOnPersist[keyEntity[i]]; _i < _a.length; _i++) {
                var val = _a[_i];
                delete currentObject[val];
            }
        }
        this.ignoreInArrays(ignoreOnColumn, this.columns);
        return this.addOnPersist(keyEntity);
    };
    TreeGrid.prototype.ignoreInArrays = function (ignoreOnColumn, columns) {
        for (var i = 0; i < columns.length; i++) {
            if (columns[i].columns) {
                this.ignoreInColumn(ignoreOnColumn, columns[i]);
                this.ignoreInArrays(ignoreOnColumn, columns[i].columns);
            }
            else {
                this.ignoreInColumn(ignoreOnColumn, columns[i]);
            }
        }
    };
    TreeGrid.prototype.ignoreInColumn = function (ignoreOnColumn, column) {
        for (var i = 0; i < ignoreOnColumn.length; i++) {
            delete column[ignoreOnColumn[i]];
            column.filter = {};
        }
    };
    TreeGrid.prototype.mouseClickHandler = function (e) {
        if (!sf.base.isNullOrUndefined(e.touches)) {
            return;
        }
        var target = e.target;
        if ((target.classList.contains('e-treegridexpand') ||
            target.classList.contains('e-treegridcollapse')) && (!this.isEditCollapse && !this.grid.isEdit)) {
            this.expandCollapseRequest(target);
        }
        this.isEditCollapse = false;
        this.notify('checkboxSelection', { target: target });
    };
    /**
     * Returns TreeGrid rows
     *
     * @returns {HTMLTableRowElement[]} - Returns row elements collection
     */
    TreeGrid.prototype.getRows = function () {
        return this.grid.getRows();
    };
    /**
     * Gets the pager of the TreeGrid.
     *
     * @returns {Element} - Returns pager element
     */
    TreeGrid.prototype.getPager = function () {
        return this.grid.getPager(); //get element from pager
    };
    /**
     * Adds a new record to the TreeGrid. Without passing parameters, it adds empty rows.
     * > `editSettings.allowEditing` should be true.
     *
     * @param {Object} data - Defines the new add record data.
     * @param {number} index - Defines the row index to be added.
     * @param {RowPosition} position - Defines the new row position to be added.
     * @returns {void}
     */
    TreeGrid.prototype.addRecord = function (data, index, position) {
        if (this.editModule) {
            var isAddedRowByMethod = 'isAddedRowByMethod';
            this.editModule[isAddedRowByMethod] = true;
            this.editModule.addRecord(data, index, position);
        }
    };
    /**
     * Cancels edited state.
     *
     * @returns {void}
     */
    TreeGrid.prototype.closeEdit = function () {
        if (this.grid.editModule) {
            this.grid.editModule.closeEdit();
        }
    };
    /**
     * Saves the cell that is currently edited. It does not save the value to the DataSource.
     *
     * @returns {void}
     */
    TreeGrid.prototype.saveCell = function () {
        if (this.grid.editModule) {
            this.grid.editModule.saveCell();
        }
    };
    /**
     * To update the specified cell by given value without changing into edited state.
     *
     * @param {number} rowIndex Defines the row index.
     * @param {string} field Defines the column field.
     * @param {string | number | boolean | Date} value - Defines the value to be changed.
     * @returns {void}
     */
    TreeGrid.prototype.updateCell = function (rowIndex, field, value) {
        if (this.grid.editModule) {
            this.grid.editModule.updateCell(rowIndex, field, value);
        }
    };
    /**
     * To update the specified row by given values without changing into edited state.
     *
     * @param {number} index Defines the row index.
     * @param {Object} data Defines the data object to be updated.
     * @returns {void}
     */
    TreeGrid.prototype.updateRow = function (index, data) {
        if (this.grid.editModule) {
            if (!sf.base.isNullOrUndefined(index)) {
                var griddata = this.grid.getCurrentViewRecords()[index];
                sf.base.extend(griddata, data);
                this.grid.editModule.updateRow(index, griddata);
            }
            else {
                this.grid.editModule.updateRow(index, data);
            }
        }
    };
    /**
     * Delete a record with Given options. If fieldName and data is not given then TreeGrid will delete the selected record.
     * > `editSettings.allowDeleting` should be true.
     *
     * @param {string} fieldName - Defines the primary key field, 'Name of the column'.
     * @param {Object} data - Defines the JSON data of the record to be deleted.
     * @returns {void}
     */
    TreeGrid.prototype.deleteRecord = function (fieldName, data) {
        if (this.grid.editModule) {
            this.grid.editModule.deleteRecord(fieldName, data);
        }
    };
    /**
     * To edit any particular row by TR element.
     *
     * @param {HTMLTableRowElement} row - Defines the table row to be edited.
     * @returns {void}
     */
    TreeGrid.prototype.startEdit = function (row) {
        if (this.grid.editModule) {
            this.grid.editModule.startEdit(row);
        }
    };
    /**
     * To edit any particular cell using row index and cell index.
     *
     * @param {number} rowIndex - Defines row index to edit a particular cell.
     * @param {string} field - Defines the field name of the column to perform cell edit.
     * @returns {void}
     */
    TreeGrid.prototype.editCell = function (rowIndex, field) {
        if (this.editModule) {
            this.editModule.editCell(rowIndex, field);
        }
    };
    /**
     * Enables or disables ToolBar items.
     *
     * @param {string[]} items - Defines the collection of itemID of ToolBar items.
     * @param {boolean} isEnable - Defines the items to be enabled or disabled.
     * @returns {void}
     */
    TreeGrid.prototype.enableToolbarItems = function (items, isEnable) {
        if (this.grid.toolbarModule) {
            this.grid.toolbarModule.enableItems(items, isEnable);
        }
    };
    /**
     * If TreeGrid is in editable state, you can save a record by invoking endEdit.
     *
     * @returns {void}
     */
    TreeGrid.prototype.endEdit = function () {
        if (this.grid.editModule) {
            this.grid.editModule.endEdit();
        }
    };
    /**
     * Column chooser can be displayed on screen by given position(X and Y axis).
     *
     * @param {number} x - Defines the X axis.
     * @param {number} y - Defines the Y axis.
     * @returns {void}
     */
    TreeGrid.prototype.openColumnChooser = function (x, y) {
        if (this.columnChooserModule) {
            this.columnChooserModule.openColumnChooser(x, y);
        }
    };
    /**
     * Delete any visible row by TR element.
     *
     * @param {HTMLTableRowElement} tr - Defines the table row element.
     * @returns {void}
     */
    TreeGrid.prototype.deleteRow = function (tr) {
        if (this.grid.editModule) {
            this.grid.editModule.deleteRow(tr);
        }
    };
    /**
     * Get the names of the primary key columns of the TreeGrid.
     *
     * @returns {string[]} - Returns primary key collection
     */
    TreeGrid.prototype.getPrimaryKeyFieldNames = function () {
        return this.grid.getPrimaryKeyFieldNames();
    };
    /**
     * Updates particular cell value based on the given primary key value.
     * > Primary key column must be specified using `columns.isPrimaryKey` property.
     *
     * @param {string| number} key - Specifies the PrimaryKey value of dataSource.
     * @param {string } field - Specifies the field name which you want to update.
     * @param {string | number | boolean | Date} value - To update new value for the particular cell.
     * @returns {void}
     */
    TreeGrid.prototype.setCellValue = function (key, field, value) {
        this.grid.setCellValue(key, field, value);
        var rowIndex = this.grid.getRowIndexByPrimaryKey(key);
        var record = this.getCurrentViewRecords()[rowIndex];
        if (!sf.base.isNullOrUndefined(record)) {
            editAction({ value: record, action: 'edit' }, this, this.isSelfReference, record.index, this.grid.selectedRowIndex, field);
        }
    };
    /**
     * Updates and refresh the particular row values based on the given primary key value.
     * > Primary key column must be specified using `columns.isPrimaryKey` property.
     *
     *  @param {string| number} key - Specifies the PrimaryKey value of dataSource.
     *  @param {Object} rowData - To update new data for the particular row.
     * @returns {void}
     */
    TreeGrid.prototype.setRowData = function (key, rowData) {
        var currentRecords = this.getCurrentViewRecords();
        var primaryKey = this.grid.getPrimaryKeyFieldNames()[0];
        var level = 0;
        var record = {};
        currentRecords.some(function (value) {
            if (value[primaryKey] === key) {
                record = value;
                return true;
            }
            else {
                return false;
            }
        });
        level = record.level;
        rowData.level = level;
        rowData.index = record.index;
        rowData.childRecords = record.childRecords;
        rowData.taskData = record.taskData;
        rowData.uniqueID = record.uniqueID;
        rowData.parentItem = record.parentItem;
        rowData.checkboxState = record.checkboxState;
        rowData.hasChildRecords = record.hasChildRecords;
        rowData.parentUniqueID = record.parentUniqueID;
        rowData.expanded = record.expanded;
        this.grid.setRowData(key, rowData);
    };
    /**
     * Navigates to the specified target page.
     *
     * @param  {number} pageNo - Defines the page number to navigate.
     * @returns {void}
     */
    TreeGrid.prototype.goToPage = function (pageNo) {
        if (this.grid.pagerModule) {
            this.grid.pagerModule.goToPage(pageNo);
        }
    };
    /**
     * Defines the text of external message.
     *
     * @param  {string} message - Defines the message to update.
     * @returns {void}
     */
    TreeGrid.prototype.updateExternalMessage = function (message) {
        if (this.pagerModule) {
            this.grid.pagerModule.updateExternalMessage(message);
        }
    };
    /**
     * Gets a cell by row and column index.
     *
     * @param  {number} rowIndex - Specifies the row index.
     * @param  {number} columnIndex - Specifies the column index.
     * @returns {Element} - Returns cell element in grid content
     */
    TreeGrid.prototype.getCellFromIndex = function (rowIndex, columnIndex) {
        return this.grid.getCellFromIndex(rowIndex, columnIndex);
    };
    /**
     * Gets a Column by column name.
     *
     * @param  {string} field - Specifies the column name.
     * @returns {Column} - Returns tree grid column
     */
    TreeGrid.prototype.getColumnByField = function (field) {
        return sf.grids.iterateArrayOrObject(this.columnModel, function (item) {
            if (item.field === field) {
                return item;
            }
            return undefined;
        })[0];
    };
    /**
     * Gets a column by UID.
     *
     * @param  {string} uid - Specifies the column UID.
     * @returns {Column} - Returns tree grid column
     */
    TreeGrid.prototype.getColumnByUid = function (uid) {
        var Columns = this.initialRender ? this.grid.columns : this.columns;
        return sf.grids.iterateArrayOrObject(Columns, function (item) {
            if (item.uid === uid) {
                return item;
            }
            return undefined;
        })[0];
    };
    /**
     * Gets the collection of column fields.
     *
     * @returns {string[]} - Returns column field name as collection
     */
    TreeGrid.prototype.getColumnFieldNames = function () {
        return this.grid.getColumnFieldNames();
    };
    /**
     * Gets the footer div of the TreeGrid.
     *
     * @returns {Element} - Returns footer content div element
     */
    TreeGrid.prototype.getFooterContent = function () {
        return this.grid.getFooterContent();
    };
    /**
     * Gets the footer table element of the TreeGrid.
     *
     * @returns {Element} - Returns footer content table element
     */
    TreeGrid.prototype.getFooterContentTable = function () {
        return this.grid.getFooterContentTable();
    };
    /**
     * Shows a column by its column name.
     *
     * @param  {string|string[]} keys - Defines a single or collection of column names.
     * @param  {string} showBy - Defines the column key either as field name or header text.
     * @returns {void}
     */
    TreeGrid.prototype.showColumns = function (keys, showBy) {
        this.grid.showColumns(keys, showBy);
        this.updateColumnModel();
    };
    /**
     * Hides a column by column name.
     *
     * @param  {string|string[]} keys - Defines a single or collection of column names.
     * @param  {string} hideBy - Defines the column key either as field name or header text.
     * @returns {void}
     */
    TreeGrid.prototype.hideColumns = function (keys, hideBy) {
        this.grid.hideColumns(keys, hideBy);
        this.updateColumnModel();
    };
    /**
     * Gets a column header by column name.
     *
     * @param  {string} field - Specifies the column name.
     * @returns {Element} - Returns column header element
     */
    TreeGrid.prototype.getColumnHeaderByField = function (field) {
        return this.grid.getColumnHeaderByField(field);
    };
    /**
     * Gets a column header by column index.
     *
     * @param  {number} index - Specifies the column index.
     * @returns {Element} - Returns column header element
     */
    TreeGrid.prototype.getColumnHeaderByIndex = function (index) {
        return this.grid.getColumnHeaderByIndex(index);
    };
    /**
     * Gets a column header by UID.
     *
     * @param {string} uid - Specifies the column uid.
     * @returns {Element} - Returns column header element
     */
    TreeGrid.prototype.getColumnHeaderByUid = function (uid) {
        return this.grid.getColumnHeaderByUid(uid);
    };
    /**
     * Gets a column index by column name.
     *
     * @param  {string} field - Specifies the column name.
     * @returns {number} - Returns column index
     */
    TreeGrid.prototype.getColumnIndexByField = function (field) {
        return this.grid.getColumnIndexByField(field);
    };
    /**
     * Gets a column index by UID.
     *
     * @param  {string} uid - Specifies the column UID.
     * @returns {number} - Returns column index
     */
    TreeGrid.prototype.getColumnIndexByUid = function (uid) {
        return this.grid.getColumnIndexByUid(uid);
    };
    /**
     * Gets the columns from the TreeGrid.
     *
     * @param {boolean} isRefresh - Defined whether to update DOM
     * @returns {Column[]} - Returns treegrid columns collection
     */
    TreeGrid.prototype.getColumns = function (isRefresh) {
        this.updateColumnModel(this.grid.getColumns(isRefresh));
        return this.columnModel;
    };
    TreeGrid.prototype.updateColumnModel = function (column) {
        var temp;
        var field;
        var gridColumns = sf.base.isNullOrUndefined(column) ? this.grid.getColumns() : column;
        if (this.treeColumnIndex !== -1 && this.columnModel[this.treeColumnIndex] &&
            !sf.base.isNullOrUndefined(this.columnModel[this.treeColumnIndex].template)) {
            temp = this.columnModel[this.treeColumnIndex].template;
            field = this.columnModel[this.treeColumnIndex].field;
        }
        this.columnModel = [];
        var stackedHeader = false;
        var gridColumn;
        for (var i = 0; i < gridColumns.length; i++) {
            gridColumn = {};
            for (var _i = 0, _a = Object.keys(gridColumns[i]); _i < _a.length; _i++) {
                var prop = _a[_i];
                gridColumn[prop] = gridColumns[i][prop];
            }
            this.columnModel.push(new Column(gridColumn));
            if (field === this.columnModel[i].field && (!sf.base.isNullOrUndefined(temp) && temp !== '')) {
                this.columnModel[i].template = temp;
            }
        }
        var merge$$1 = 'deepMerge';
        this[merge$$1] = ['columns']; // Workaround for blazor updateModel
        if (this.grid.columns.length !== this.columnModel.length) {
            stackedHeader = true;
        }
        if (!stackedHeader) {
            this.setProperties({ columns: this.columnModel }, true);
        }
        this[merge$$1] = undefined; // Workaround for blazor updateModel
        return this.columnModel;
    };
    /**
     * Gets the content div of the TreeGrid.
     *
     * @returns {Element} - Return tree grid content element
     */
    TreeGrid.prototype.getContent = function () {
        return this.grid.getContent();
    };
    TreeGrid.prototype.mergePersistTreeGridData = function () {
        var persist1 = 'mergePersistGridData';
        this.grid[persist1].apply(this);
    };
    TreeGrid.prototype.mergeColumns = function (storedColumn, columns) {
        var persist2 = 'mergeColumns';
        this.grid[persist2].apply(this, [storedColumn, columns]);
    };
    TreeGrid.prototype.updateTreeGridModel = function () {
        this.setProperties({ filterSettings: sf.grids.getObject('properties', this.grid.filterSettings) }, true);
        this.setProperties({ pageSettings: sf.grids.getObject('properties', this.grid.pageSettings) }, true);
        this.setProperties({ searchSettings: sf.grids.getObject('properties', this.grid.searchSettings) }, true);
        this.setProperties({ sortSettings: sf.grids.getObject('properties', this.grid.sortSettings) }, true);
    };
    /**
     * Gets the content table of the TreeGrid.
     *
     * @returns {Element} - Returns content table element
     */
    TreeGrid.prototype.getContentTable = function () {
        return this.grid.getContentTable();
    };
    /**
     * Gets all the TreeGrid's data rows.
     *
     * @returns {Element[]} - Returns row elements
     */
    TreeGrid.prototype.getDataRows = function () {
        var dRows = [];
        var rows = this.grid.getDataRows();
        for (var i = 0, len = rows.length; i < len; i++) {
            if (!rows[i].classList.contains('e-summaryrow')) {
                dRows.push(rows[i]);
            }
        }
        return dRows;
    };
    /**
     * Get current visible data of TreeGrid.
     *
     * @returns {Object[]} - Returns current view records
     * @isGenericType true
     */
    TreeGrid.prototype.getCurrentViewRecords = function () {
        return this.grid.currentViewData;
    };
    /**
     * Gets the added, edited,and deleted data before bulk save to the DataSource in batch mode.
     *
     * @returns {Object} - Returns batch changes
     */
    TreeGrid.prototype.getBatchChanges = function () {
        return this.grid.editModule.getBatchChanges();
    };
    /**
     * Gets the header div of the TreeGrid.
     *
     * @returns {Element} - Returns Header content element
     */
    TreeGrid.prototype.getHeaderContent = function () {
        return this.grid.getHeaderContent();
    };
    /**
     * Gets the header table element of the TreeGrid.
     *
     * @returns {Element} - Return header table element
     */
    TreeGrid.prototype.getHeaderTable = function () {
        return this.grid.getHeaderTable();
    };
    /**
     * Gets a row by index.
     *
     * @param  {number} index - Specifies the row index.
     * @returns {Element} - Returns row element
     */
    TreeGrid.prototype.getRowByIndex = function (index) {
        return this.grid.getRowByIndex(index);
    };
    /**
     * Get a row information based on cell
     *
     * @param {Element | EventTarget} target - Target row element
     * @returns {RowInfo} - Returns row information in a JSON object
     */
    TreeGrid.prototype.getRowInfo = function (target) {
        return this.grid.getRowInfo(target);
    };
    /**
     * Gets UID by column name.
     *
     * @param  {string} field - Specifies the column name.
     * @returns {string} - Returns unique id based on column field name given
     */
    TreeGrid.prototype.getUidByColumnField = function (field) {
        return this.grid.getUidByColumnField(field);
    };
    /**
     * Gets the visible columns from the TreeGrid.
     *
     * @returns {Column[]} - Returns visible columns collection
     */
    TreeGrid.prototype.getVisibleColumns = function () {
        var cols = [];
        for (var _i = 0, _a = this.columnModel; _i < _a.length; _i++) {
            var col = _a[_i];
            if (col.visible) {
                cols.push(col);
            }
        }
        return cols;
    };
    /**
     * By default, TreeGrid shows the spinner for all its actions. You can use this method to show spinner at your needed time.
     *
     * @returns {void}
     */
    TreeGrid.prototype.showSpinner = function () {
        sf.popups.showSpinner(this.element);
    };
    /**
     * Manually shown spinner needs to hide by `hideSpinnner`.
     *
     * @returns {void}
     */
    TreeGrid.prototype.hideSpinner = function () {
        sf.popups.hideSpinner(this.element);
    };
    /**
     * Refreshes the TreeGrid header and content.
     *
     * @returns {void}
     */
    TreeGrid.prototype.refresh = function () {
        this.uniqueIDCollection = {};
        this.convertTreeData(this.dataSource);
        if (!isCountRequired(this)) {
            this.grid.dataSource = !(this.dataSource instanceof sf.data.DataManager) ? this.flatData :
                new sf.data.DataManager(this.dataSource.dataSource, this.dataSource.defaultQuery, this.dataSource.adaptor);
        }
        this.grid.refresh();
    };
    /**
     * Get the records of checked rows.
     *
     * @returns {Object[]} - Returns records that has been checked
     * @isGenericType true
     */
    TreeGrid.prototype.getCheckedRecords = function () {
        return this.selectionModule.getCheckedrecords();
    };
    /**
     * Get the visible records corresponding to rows visually displayed.
     *
     * @returns {Object[]} - Returns visible records based on collapse state of rows
     * @isGenericType true
     */
    TreeGrid.prototype.getVisibleRecords = function () {
        var visibleRecords = [];
        var currentViewRecords = this.getCurrentViewRecords();
        if (!this.allowPaging) {
            for (var i = 0; i < currentViewRecords.length; i++) {
                visibleRecords.push(currentViewRecords[i]);
                if (!currentViewRecords[i].expanded) {
                    i += findChildrenRecords(currentViewRecords[i]).length;
                }
            }
        }
        else {
            visibleRecords = currentViewRecords;
        }
        return visibleRecords;
    };
    /**
     * Get the indexes of checked rows.
     *
     * @returns {number[]} - Returns checked row indexes
     */
    TreeGrid.prototype.getCheckedRowIndexes = function () {
        return this.selectionModule.getCheckedRowIndexes();
    };
    /**
     * Checked the checkboxes using rowIndexes.
     *
     * @param {number[]} indexes - row indexes
     * @returns {void}
     */
    TreeGrid.prototype.selectCheckboxes = function (indexes) {
        this.selectionModule.selectCheckboxes(indexes);
    };
    /**
     * Refreshes the TreeGrid column changes.
     *
     * @param {boolean} refreshUI - Defined whether to refresh the DOM
     * @returns {void}
     */
    TreeGrid.prototype.refreshColumns = function (refreshUI) {
        if (sf.base.isNullOrUndefined(refreshUI) || refreshUI) {
            this.grid.columns = this.getGridColumns(this.columns);
            this.grid.refreshColumns();
        }
        else {
            this.grid.setProperties({ columns: this.getGridColumns(this.columns) }, true);
        }
    };
    /**
     * Refreshes the TreeGrid header.
     *
     * @returns {void}
     */
    TreeGrid.prototype.refreshHeader = function () {
        this.grid.refreshHeader();
    };
    /**
     * Expands or collapse child records
     *
     * @param {HTMLElement} target - Expand collapse icon cell as target element
     * @returns {void}
     * @hidden
     */
    TreeGrid.prototype.expandCollapseRequest = function (target) {
        if (this.editSettings.mode === 'Batch') {
            var obj = 'dialogObj';
            var showDialog = 'showDialog';
            if (this.getBatchChanges()[this.changedRecords].length ||
                this.getBatchChanges()[this.deletedRecords].length || this.getBatchChanges()[this.addedRecords].length) {
                var dialogObj = this.grid.editModule[obj];
                this.grid.editModule[showDialog]('CancelEdit', dialogObj);
                this.targetElement = target;
                return;
            }
        }
        if (this.rowTemplate) {
            var rowInfo = target.closest('.e-treerowcell').parentElement;
            var record = this.getCurrentViewRecords()[rowInfo.rowIndex];
            if (target.classList.contains('e-treegridexpand')) {
                this.collapseRow(rowInfo, record);
            }
            else {
                this.expandRow(rowInfo, record);
            }
        }
        else {
            var rowInfo = this.grid.getRowInfo(target);
            var record = rowInfo.rowData;
            if (this.enableImmutableMode) {
                record = this.getCurrentViewRecords()[rowInfo.rowIndex];
            }
            if (target.classList.contains('e-treegridexpand')) {
                this.collapseRow(rowInfo.row, record);
            }
            else {
                this.expandRow(rowInfo.row, record);
            }
        }
    };
    /**
     * Expands child rows
     *
     * @param {HTMLTableRowElement} row - Expands the given row
     * @param {Object} record - Expands the given record
     * @returns {void}
     */
    TreeGrid.prototype.expandRow = function (row, record) {
        var _this = this;
        record = this.getCollapseExpandRecords(row, record);
        if (!sf.base.isNullOrUndefined(row) && row.cells[0].classList.contains('e-lastrowcell')) {
            this.lastRowBorder(row, false);
        }
        var args = { data: record, row: row, cancel: false };
        this.trigger(expanding, args, function (expandingArgs) {
            if (!expandingArgs.cancel) {
                _this.expandCollapse('expand', row, record);
                var children = 'Children';
                if (!(isRemoteData(_this) && !isOffline(_this)) && (!isCountRequired(_this) || !sf.base.isNullOrUndefined(record[children]))) {
                    var collapseArgs = { data: record, row: row };
                    _this.setHeightForFrozenContent();
                    _this.trigger(expanded, collapseArgs);
                }
            }
        });
    };
    TreeGrid.prototype.setHeightForFrozenContent = function () {
        var freeze = (this.grid.getFrozenLeftColumnsCount() > 0 || this.grid.getFrozenRightColumnsCount() > 0) ? true : false;
        if (this.grid.getFrozenColumns() > 0 || freeze) {
            this.grid.contentModule.refreshScrollOffset();
        }
    };
    TreeGrid.prototype.getCollapseExpandRecords = function (row, record) {
        if (this.allowPaging && this.pageSettings.pageSizeMode === 'All' && this.isExpandAll && sf.base.isNullOrUndefined(record) &&
            !isRemoteData(this)) {
            record = this.flatData.filter(function (e) {
                return e.hasChildRecords;
            });
        }
        else if (sf.base.isNullOrUndefined(record)) {
            record = this.grid.getCurrentViewRecords()[row.rowIndex];
        }
        return record;
    };
    /**
     * Collapses child rows
     *
     * @param {HTMLTableRowElement} row - Collapse the given row
     * @param {Object} record - Collapse the given record
     * @returns {void}
     */
    TreeGrid.prototype.collapseRow = function (row, record) {
        var _this = this;
        record = this.getCollapseExpandRecords(row, record);
        var args = { data: record, row: row, cancel: false };
        this.trigger(collapsing, args, function (collapsingArgs) {
            if (!collapsingArgs.cancel) {
                _this.expandCollapse('collapse', row, record);
                var collapseArgs = { data: record, row: row };
                if (!isRemoteData(_this)) {
                    _this.setHeightForFrozenContent();
                    _this.trigger(collapsed, collapseArgs);
                    if (_this.enableInfiniteScrolling) {
                        var scrollHeight = _this.grid.getContent().firstElementChild.scrollHeight;
                        var scrollTop = _this.grid.getContent().firstElementChild.scrollTop;
                        if ((scrollHeight - scrollTop) < _this.grid.getRowHeight() + +_this.height) {
                            _this.grid.getContent().firstElementChild.scrollBy(0, _this.grid.getRowHeight());
                        }
                    }
                }
            }
        });
    };
    /**
     * Expands the records at specific hierarchical level
     *
     * @param {number} level - Expands the parent rows at given level
     * @returns {void}
     */
    TreeGrid.prototype.expandAtLevel = function (level) {
        if (((this.allowPaging && this.pageSettings.pageSizeMode === 'All') || this.enableVirtualization) && !isRemoteData(this)) {
            var rec = this.grid.dataSource.filter(function (e) {
                if (e.hasChildRecords && e.level === level) {
                    e.expanded = true;
                }
                return e.hasChildRecords && e.level === level;
            });
            this.expandRow(null, rec);
        }
        else {
            var rec = this.getRecordDetails(level);
            var row = sf.grids.getObject('rows', rec);
            var record = sf.grids.getObject('records', rec);
            for (var i = 0; i < record.length; i++) {
                var pindex = this.flatData[record[i].parentItem.index].index;
                if (this.flatData[pindex].expanded === false) {
                    record.push(this.flatData[pindex]);
                    this.flatData[pindex].expanded = true;
                }
                this.expandRow(row[i], record[i]);
            }
        }
    };
    TreeGrid.prototype.getRecordDetails = function (level) {
        var rows = this.getRows().filter(function (e) {
            return (e.className.indexOf('level' + level) !== -1
                && (e.querySelector('.e-treegridcollapse') || e.querySelector('.e-treegridexpand')));
        });
        var records = this.getCurrentViewRecords().filter(function (e) {
            return e.level === level && e.hasChildRecords;
        });
        var obj = { records: records, rows: rows };
        return obj;
    };
    /**
     * Collapses the records at specific hierarchical level
     *
     * @param {number} level - Define the parent row level which needs to be collapsed
     * @returns {void}
     */
    TreeGrid.prototype.collapseAtLevel = function (level) {
        if (((this.allowPaging && this.pageSettings.pageSizeMode === 'All') || this.enableVirtualization) && !isRemoteData(this)) {
            var record = this.grid.dataSource.filter(function (e) {
                if (e.hasChildRecords && e.level === level) {
                    e.expanded = false;
                }
                return e.hasChildRecords && e.level === level;
            });
            this.collapseRow(null, record);
        }
        else {
            var rec = this.getRecordDetails(level);
            var rows = sf.grids.getObject('rows', rec);
            var records = sf.grids.getObject('records', rec);
            for (var i = 0; i < records.length; i++) {
                this.collapseRow(rows[i], records[i]);
            }
        }
        if (!this.grid.contentModule.isDataSourceChanged && this.enableVirtualization && this.getRows()
            && this.parentData.length === this.getRows().length) {
            var endIndex = 'endIndex';
            this.grid.contentModule.startIndex = -1;
            this.grid.contentModule[endIndex] = -1;
        }
    };
    /**
     * Expands All the rows
     *
     * @returns {void}
     */
    TreeGrid.prototype.expandAll = function () {
        this.expandCollapseAll('expand');
    };
    /**
     * Collapses All the rows
     *
     * @returns {void}
     */
    TreeGrid.prototype.collapseAll = function () {
        this.expandCollapseAll('collapse');
    };
    TreeGrid.prototype.expandCollapseAll = function (action) {
        var rows = this.getRows().filter(function (e) {
            return e.querySelector('.e-treegrid' + (action === 'expand' ? 'collapse' : 'expand'));
        });
        if (!rows.length && this.getRows().length) {
            rows.push(this.getRows()[0]);
        }
        this.isExpandAll = true;
        this.isCollapseAll = true;
        if (((this.allowPaging && this.pageSettings.pageSizeMode === 'All') || this.enableVirtualization) && !isRemoteData(this)) {
            this.flatData.filter(function (e) {
                if (e.hasChildRecords) {
                    e.expanded = action === 'collapse' ? false : true;
                }
            });
            if (rows.length) {
                if (action === 'collapse') {
                    this.collapseRow(rows[0]);
                }
                else {
                    this.expandRow(rows[0]);
                }
            }
            else if (this.allowPaging) {
                var isExpandCollapseall = this.enableCollapseAll;
                this.setProperties({ enableCollapseAll: true }, true);
                this.grid.pagerModule.goToPage(1);
                this.setProperties({ enableCollapseAll: isExpandCollapseall }, true);
            }
        }
        else {
            for (var i = 0; i < rows.length; i++) {
                if (action === 'collapse') {
                    this.collapseRow(rows[i]);
                }
                else {
                    this.expandRow(rows[i]);
                }
            }
        }
        this.isExpandAll = false;
        this.isCollapseAll = false;
    };
    TreeGrid.prototype.expandCollapse = function (action, row, record, isChild) {
        var expandingArgs = { row: row, data: record, childData: [], requestType: action };
        var childRecords = this.getCurrentViewRecords().filter(function (e) {
            return e.parentUniqueID === record.uniqueID;
        });
        var targetEle;
        if (!isRemoteData(this) && action === 'expand' && this.isSelfReference && isCountRequired(this) && !childRecords.length) {
            this.updateChildOnDemand(expandingArgs);
        }
        var gridRows = this.getRows();
        if (this.rowTemplate) {
            var rows = this.getContentTable().rows;
            gridRows = [].slice.call(rows);
        }
        var rowIndex;
        if (sf.base.isNullOrUndefined(row)) {
            rowIndex = this.getCurrentViewRecords().indexOf(record);
            row = gridRows[rowIndex];
        }
        else {
            rowIndex = +row.getAttribute('aria-rowindex');
        }
        if (!sf.base.isNullOrUndefined(row)) {
            row.setAttribute('aria-expanded', action === 'expand' ? 'true' : 'false');
        }
        if (((this.allowPaging && this.pageSettings.pageSizeMode === 'All') || this.enableVirtualization) && !isRemoteData(this)
            && !isCountRequired(this)) {
            this.notify(localPagedExpandCollapse, { action: action, row: row, record: record });
        }
        else {
            var displayAction = void 0;
            if (action === 'expand') {
                displayAction = 'table-row';
                if (!isChild) {
                    record.expanded = true;
                    this.uniqueIDCollection[record.uniqueID].expanded = record.expanded;
                }
                if (!sf.base.isNullOrUndefined(row)) {
                    targetEle = row.getElementsByClassName('e-treegridcollapse')[0];
                }
                if (isChild && !sf.base.isNullOrUndefined(record[this.expandStateMapping]) &&
                    record[this.expandStateMapping] && sf.base.isNullOrUndefined(targetEle)) {
                    targetEle = row.getElementsByClassName('e-treegridexpand')[0];
                }
                if (sf.base.isNullOrUndefined(targetEle)) {
                    return;
                }
                if (!targetEle.classList.contains('e-treegridexpand')) {
                    sf.base.addClass([targetEle], 'e-treegridexpand');
                }
                sf.base.removeClass([targetEle], 'e-treegridcollapse');
            }
            else {
                displayAction = 'none';
                if (!isChild || isCountRequired(this)) {
                    record.expanded = false;
                    this.uniqueIDCollection[record.uniqueID].expanded = record.expanded;
                }
                if (!sf.base.isNullOrUndefined(row)) {
                    targetEle = row.getElementsByClassName('e-treegridexpand')[0];
                }
                if (isChild && !sf.base.isNullOrUndefined(record[this.expandStateMapping]) &&
                    !record[this.expandStateMapping] && sf.base.isNullOrUndefined(targetEle)) {
                    targetEle = row.getElementsByClassName('e-treegridcollapse')[0];
                }
                if (sf.base.isNullOrUndefined(targetEle)) {
                    return;
                }
                if (!targetEle.classList.contains('e-treegridcollapse')) {
                    sf.base.addClass([targetEle], 'e-treegridcollapse');
                }
                sf.base.removeClass([targetEle], 'e-treegridexpand');
            }
            var detailrows = gridRows.filter(function (r) {
                return r.classList.contains('e-griddetailrowindex' + record.index + 'level' + (record.level + 1));
            });
            if (isRemoteData(this) && !isOffline(this)) {
                this.remoteExpand(action, row, record);
            }
            else {
                if ((!isCountRequired(this) || childRecords.length) || action === 'collapse') {
                    this.localExpand(action, row, record);
                }
            }
            if (this.isPixelHeight() && !row.cells[0].classList.contains('e-lastrowcell')) {
                var totalRows = this.getRows();
                var rows = this.getContentTable().rows;
                totalRows = [].slice.call(rows);
                for (var i = totalRows.length - 1; i > 0; i--) {
                    if (!isHidden(totalRows[i])) {
                        var table = this.getContentTable();
                        var sHeight = table.scrollHeight;
                        var clientHeight = this.getContent().clientHeight;
                        this.lastRowBorder(totalRows[i], sHeight <= clientHeight);
                        break;
                    }
                }
            }
            this.notify('rowExpandCollapse', { detailrows: detailrows, action: displayAction, record: record, row: row });
            this.updateAltRow(gridRows);
        }
    };
    TreeGrid.prototype.updateChildOnDemand = function (expandingArgs) {
        var _this = this;
        var deff = new sf.data.Deferred();
        var childDataBind = 'childDataBind';
        expandingArgs[childDataBind] = deff.resolve;
        var record = expandingArgs.data;
        this.trigger(dataStateChange, expandingArgs);
        deff.promise.then(function () {
            if (expandingArgs.childData.length) {
                var currentData = (_this.flatData);
                var index = 0;
                for (var i = 0; i < currentData.length; i++) {
                    if (currentData[i].taskData === record.taskData) {
                        index = i;
                        break;
                    }
                }
                var data_1 = sf.base.getValue('result', _this.dataSource);
                var childData = extendArray(expandingArgs.childData);
                var length_1 = record[_this.childMapping] ? record[_this.childMapping].length > childData.length ?
                    record[_this.childMapping].length : childData.length : childData.length;
                for (var i = 0; i < length_1; i++) {
                    if (record[_this.childMapping]) {
                        data_1.filter(function (e, i) {
                            if (e[_this.parentIdMapping] === record[_this.idMapping]) {
                                data_1.splice(i, 1);
                            }
                        });
                    }
                    if (childData[i]) {
                        childData[i].level = record.level + 1;
                        childData[i].index = Math.ceil(Math.random() * 1000);
                        childData[i].parentItem = sf.base.extend({}, record);
                        childData[i].taskData = sf.base.extend({}, childData[i]);
                        delete childData[i].parentItem.childRecords;
                        delete childData[i].taskData.parentItem;
                        childData[i].parentUniqueID = record.uniqueID;
                        childData[i].uniqueID = sf.grids.getUid(_this.element.id + '_data_');
                        sf.base.setValue('uniqueIDCollection.' + childData[i].uniqueID, childData[i], _this);
                        if (!sf.base.isNullOrUndefined(childData[i][_this.childMapping]) ||
                            (childData[i][_this.hasChildMapping] && isCountRequired(_this))) {
                            childData[i].hasChildRecords = true;
                        }
                        currentData.splice(index + 1 + i, record[_this.childMapping] && record[_this.childMapping][i] ? 1 : 0, childData[i]);
                    }
                    else {
                        currentData.splice(index + 1 + i, 1);
                    }
                }
                currentData[index][_this.childMapping] = childData;
                currentData[index].childRecords = childData;
                currentData[index].expanded = true;
                sf.base.setValue('uniqueIDCollection.' + currentData[index].uniqueID, currentData[index], _this);
                for (var j = 0; j < expandingArgs.childData.length; j++) {
                    data_1.push(expandingArgs.childData[j]);
                }
            }
            _this.isExpandRefresh = true;
            _this.grid.refresh();
            _this.setHeightForFrozenContent();
            _this.trigger(expanded, expandingArgs);
        });
    };
    TreeGrid.prototype.remoteExpand = function (action, row, record) {
        var gridRows = this.getRows();
        if (this.rowTemplate) {
            var rows_1 = this.getContentTable().rows;
            gridRows = [].slice.call(rows_1);
        }
        var args = { data: record, row: row };
        var rows = [];
        rows = gridRows.filter(function (r) {
            return r.querySelector('.e-gridrowindex' + record.index + 'level' + (record.level + 1));
        });
        if (action === 'expand') {
            this.notify(remoteExpand, { record: record, rows: rows, parentRow: row });
            var args_1 = { row: row, data: record };
            if (rows.length > 0) {
                this.setHeightForFrozenContent();
                this.trigger(expanded, args_1);
            }
        }
        else {
            this.collapseRemoteChild({ record: record, rows: rows });
            this.setHeightForFrozenContent();
            this.trigger(collapsed, args);
        }
    };
    TreeGrid.prototype.localExpand = function (action, row, record) {
        var rows;
        var childRecords = this.getCurrentViewRecords().filter(function (e) {
            return e.parentUniqueID === record.uniqueID;
        });
        if (this.isPixelHeight() && row.cells[0].classList.contains('e-lastrowcell')) {
            this.lastRowBorder(row, false);
        }
        var movableRows;
        var freezeRightRows;
        var gridRows = this.getRows();
        if (this.rowTemplate) {
            var rows_2 = this.getContentTable().rows;
            gridRows = [].slice.call(rows_2);
        }
        var displayAction = (action === 'expand') ? 'table-row' : 'none';
        var primaryKeyField = this.getPrimaryKeyFieldNames()[0];
        if (this.enableImmutableMode && !this.allowPaging) {
            rows = [];
            for (var i = 0; i < childRecords.length; i++) {
                var rowIndex = this.grid.getRowIndexByPrimaryKey(childRecords[i][primaryKeyField]);
                rows.push(this.getRows()[rowIndex]);
            }
        }
        else {
            rows = gridRows.filter(function (r) {
                return r.querySelector('.e-gridrowindex' + record.index + 'level' + (record.level + 1));
            });
        }
        if (this.frozenRows || this.frozenColumns || this.getFrozenColumns()) {
            movableRows = this.getMovableRows().filter(function (r) {
                return r.querySelector('.e-gridrowindex' + record.index + 'level' + (record.level + 1));
            });
        }
        var freeze = (this.grid.getFrozenLeftColumnsCount() > 0 || this.grid.getFrozenRightColumnsCount() > 0) ? true : false;
        if (freeze) {
            freezeRightRows = this.getFrozenRightRows().filter(function (r) {
                return r.querySelector('.e-gridrowindex' + record.index + 'level' + (record.level + 1));
            });
        }
        for (var i = 0; i < rows.length; i++) {
            if (!sf.base.isNullOrUndefined(rows[i])) {
                rows[i].style.display = displayAction;
            }
            if (!sf.base.isNullOrUndefined(movableRows)) {
                movableRows[i].style.display = displayAction;
            }
            if (!sf.base.isNullOrUndefined(freezeRightRows)) {
                freezeRightRows[i].style.display = displayAction;
            }
            this.notify('childRowExpand', { row: rows[i] });
            if (!sf.base.isNullOrUndefined(childRecords[i].childRecords) && (action !== 'expand' ||
                sf.base.isNullOrUndefined(childRecords[i].expanded) || childRecords[i].expanded)) {
                this.expandCollapse(action, rows[i], childRecords[i], true);
                if (this.frozenColumns <= this.treeColumnIndex && !sf.base.isNullOrUndefined(movableRows)) {
                    this.expandCollapse(action, movableRows[i], childRecords[i], true);
                }
            }
        }
    };
    TreeGrid.prototype.updateAltRow = function (rows) {
        if (this.enableAltRow && !this.rowTemplate) {
            var visibleRowCount = 0;
            for (var i = 0; rows && i < rows.length; i++) {
                var gridRow = rows[i];
                if (gridRow.style.display !== 'none') {
                    if (gridRow.classList.contains('e-altrow')) {
                        sf.base.removeClass([gridRow], 'e-altrow');
                    }
                    if (visibleRowCount % 2 !== 0 && !gridRow.classList.contains('e-summaryrow') && !gridRow.classList.contains('e-detailrow')) {
                        sf.base.addClass([gridRow], 'e-altrow');
                    }
                    if (!gridRow.classList.contains('e-summaryrow') && !gridRow.classList.contains('e-detailrow')) {
                        visibleRowCount++;
                    }
                }
            }
        }
    };
    TreeGrid.prototype.treeColumnRowTemplate = function () {
        if (this.rowTemplate) {
            var rows = this.getContentTable().rows;
            rows = [].slice.call(rows);
            for (var i = 0; i < rows.length; i++) {
                var rcell = this.grid.getContentTable().rows[i].cells[this.treeColumnIndex];
                var row = rows[i];
                var rowData = this.grid.getRowsObject()[i].data;
                var arg = { data: rowData, row: row, cell: rcell, column: this.getColumns()[this.treeColumnIndex] };
                this.renderModule.cellRender(arg);
            }
        }
    };
    TreeGrid.prototype.collapseRemoteChild = function (rowDetails, isChild) {
        if (!isChild) {
            rowDetails.record.expanded = false;
        }
        var rows = rowDetails.rows;
        var row;
        var childRecord;
        var movablerows = [];
        var rightrows = [];
        var freeze = (this.getFrozenLeftColumnsCount() > 0 || this.getFrozenRightColumnsCount() > 0) ? true : false;
        if (freeze) {
            movablerows = this.getMovableRows().filter(function (r) {
                return r.querySelector('.e-gridrowindex' + rowDetails.record.index + 'level' + (rowDetails.record.level + 1));
            });
            rightrows = this.getFrozenRightRows().filter(function (r) {
                return r.querySelector('.e-gridrowindex' + rowDetails.record.index + 'level' + (rowDetails.record.level + 1));
            });
        }
        for (var i = 0; i < rows.length; i++) {
            rows[i].style.display = 'none';
            row = rows[i];
            var collapsingTd = rows[i].querySelector('.e-detailrowexpand');
            if (!sf.base.isNullOrUndefined(collapsingTd)) {
                this.grid.detailRowModule.collapse(collapsingTd);
            }
            if (freeze) {
                movablerows[i].style.display = 'none';
                rightrows[i].style.display = 'none';
                if (!rows[i].querySelector('.e-treecolumn-container .e-treegridexpand')) {
                    if (movablerows[i].querySelector('.e-treecolumn-container .e-treegridexpand')) {
                        row = movablerows[i];
                    }
                    else if (rightrows[i].querySelector('.e-treecolumn-container .e-treegridexpand')) {
                        row = rightrows[i];
                    }
                }
            }
            if (row.querySelector('.e-treecolumn-container .e-treegridexpand')) {
                var expandElement = row.querySelector('.e-treecolumn-container .e-treegridexpand');
                childRecord = this.rowTemplate ? this.grid.getCurrentViewRecords()[rows[i].rowIndex] :
                    this.grid.getRowObjectFromUID(rows[i].getAttribute('data-Uid')).data;
                if (!sf.base.isNullOrUndefined(expandElement) && childRecord.expanded) {
                    sf.base.removeClass([expandElement], 'e-treegridexpand');
                    sf.base.addClass([expandElement], 'e-treegridcollapse');
                }
                var cRow = [];
                var eRows = this.getRows();
                for (var i_1 = 0; i_1 < eRows.length; i_1++) {
                    if (eRows[i_1].querySelector('.e-gridrowindex' + childRecord.index + 'level' + (childRecord.level + 1))) {
                        cRow.push(eRows[i_1]);
                    }
                }
                if (cRow.length && childRecord.expanded) {
                    this.collapseRemoteChild({ record: childRecord, rows: cRow }, true);
                }
            }
        }
    };
    /**
     * @hidden
     * @returns {void}
     */
    TreeGrid.prototype.addListener = function () {
        this.on('updateResults', this.updateResultModel, this);
        this.grid.on('initial-end', this.afterGridRender, this);
    };
    TreeGrid.prototype.updateResultModel = function (returnResult) {
        this.dataResults = returnResult;
    };
    /**
     * @hidden
     * @returns {void}
     */
    TreeGrid.prototype.removeListener = function () {
        if (this.isDestroyed) {
            return;
        }
        this.off('updateResults', this.updateResultModel);
        this.grid.off('initial-end', this.afterGridRender);
    };
    /**
     * Filters TreeGrid row by column name with the given options.
     *
     * @param  {string} fieldName - Defines the field name of the column.
     * @param  {string} filterOperator - Defines the operator to filter records.
     * @param  {string | number | Date | boolean} filterValue - Defines the value used to filter records.
     * @param  {string} predicate - Defines the relationship between one filter query and another by using AND or OR predicate.
     * @param  {boolean} matchCase - If match case is set to true, the TreeGrid filters the records with exact match. if false, it filters
     * case insensitive records (uppercase and lowercase letters are treated the same).
     * @param  {boolean} ignoreAccent - If ignoreAccent is set to true,
     * then filter ignores diacritic characters or accents while filtering.
     * @param  {string} actualFilterValue - Defines the actual filter value for filter column.
     * @param  {string} actualOperator - Defines the actual filter operator for filter column.
     * @returns {void}
     */
    TreeGrid.prototype.filterByColumn = function (fieldName, filterOperator, filterValue, predicate, matchCase, ignoreAccent, actualFilterValue, actualOperator) {
        this.grid.filterByColumn(fieldName, filterOperator, filterValue, predicate, matchCase, ignoreAccent, actualFilterValue, actualOperator);
    };
    /**
     * Clears all the filtered rows of the TreeGrid.
     *
     * @returns {void}
     */
    TreeGrid.prototype.clearFiltering = function () {
        this.grid.clearFiltering();
    };
    /**
     * Removes filtered column by field name.
     *
     * @param  {string} field - Defines column field name to remove filter.
     * @param  {boolean} isClearFilterBar -  Specifies whether the filter bar value needs to be cleared.
     * @returns {void}
     * @hidden
     */
    TreeGrid.prototype.removeFilteredColsByField = function (field, isClearFilterBar) {
        this.grid.removeFilteredColsByField(field, isClearFilterBar);
    };
    /**
     * Selects a row by given index.
     *
     * @param  {number} index - Defines the row index.
     * @param  {boolean} isToggle - If set to true, then it toggles the selection.
     * @returns {void}
     */
    TreeGrid.prototype.selectRow = function (index, isToggle) {
        this.grid.selectRow(index, isToggle);
    };
    /**
     * Selects a collection of rows by indexes.
     *
     * @param  {number[]} rowIndexes - Specifies the row indexes.
     * @returns {void}
     */
    TreeGrid.prototype.selectRows = function (rowIndexes) {
        this.grid.selectRows(rowIndexes);
    };
    /**
     * Deselects the current selected rows and cells.
     *
     * @returns {void}
     */
    TreeGrid.prototype.clearSelection = function () {
        this.grid.clearSelection();
    };
    /**
     * Copy the selected rows or cells data into clipboard.
     *
     * @param {boolean} withHeader - Specifies whether the column header text needs to be copied along with rows or cells.
     * @returns {void}
     */
    TreeGrid.prototype.copy = function (withHeader) {
        this.clipboardModule.copy(withHeader);
    };
    /**
     * Paste data from clipboard to selected cells.
     *
     * @param {boolean} data - Specifies the date for paste.
     * @param {boolean} rowIndex - Specifies the row index.
     * @param {boolean} colIndex - Specifies the column index.
     * @returns {void}
     */
    TreeGrid.prototype.paste = function (data, rowIndex, colIndex) {
        this.clipboardModule.paste(data, rowIndex, colIndex);
    };
    /**
     * Selects a cell by the given index.
     *
     * @param  {IIndex} cellIndex - Defines the row and column indexes.
     * @param  {boolean} isToggle - If set to true, then it toggles the selection.
     * @returns {void}
     */
    TreeGrid.prototype.selectCell = function (cellIndex, isToggle) {
        this.grid.selectCell(cellIndex, isToggle);
    };
    /**
     * Gets the collection of selected rows.
     *
     * @returns {Element[]} - Returns selected row elements collection
     */
    TreeGrid.prototype.getSelectedRows = function () {
        return this.grid.getSelectedRows();
    };
    /**
     * Gets a movable table cell by row and column index.
     *
     * @param  {number} rowIndex - Specifies the row index.
     * @param  {number} columnIndex - Specifies the column index.
     * @returns {Element} - Returns movable cell element from the indexes passed
     */
    TreeGrid.prototype.getMovableCellFromIndex = function (rowIndex, columnIndex) {
        return this.grid.getMovableCellFromIndex(rowIndex, columnIndex);
    };
    /**
     * Gets all the TreeGrid's movable table data rows.
     *
     * @returns {Element[]} - Returns element collection of movable rows
     */
    TreeGrid.prototype.getMovableDataRows = function () {
        return this.grid.getMovableDataRows();
    };
    /**
     * Gets a movable tables row by index.
     *
     * @param  {number} index - Specifies the row index.
     * @returns {Element} - Returns movable row based on index passed
     */
    TreeGrid.prototype.getMovableRowByIndex = function (index) {
        return this.grid.getMovableRowByIndex(index);
    };
    /**
     * Gets the TreeGrid's movable content rows from frozen treegrid.
     *
     * @returns {Element[]}: Returns movable row element
     */
    TreeGrid.prototype.getMovableRows = function () {
        return this.grid.getMovableRows();
    };
    /**
     * Gets a frozen right tables row element by index.
     *
     * @param  {number} index - Specifies the row index.
     * @returns {Element} returns the element
     */
    TreeGrid.prototype.getFrozenRightRowByIndex = function (index) {
        return this.grid.getFrozenRightRowByIndex(index);
    };
    /**
     * Gets the Tree Grid's frozen right content rows from frozen Tree Grid.
     *
     * @returns {Element[]} returns the element
     */
    TreeGrid.prototype.getFrozenRightRows = function () {
        return this.grid.getFrozenRightRows();
    };
    /**
     * Gets all the Tree Grid's frozen right table data rows.
     *
     * @returns {Element[]} Returns the Element
     */
    TreeGrid.prototype.getFrozenRightDataRows = function () {
        return this.grid.getFrozenRightDataRows();
    };
    /**
     * Gets a frozen right table cell by row and column index.
     *
     * @param  {number} rowIndex - Specifies the row index.
     * @param  {number} columnIndex - Specifies the column index.
     * @returns {Element} Returns the Element
     */
    TreeGrid.prototype.getFrozenRightCellFromIndex = function (rowIndex, columnIndex) {
        return this.grid.getFrozenRightCellFromIndex(rowIndex, columnIndex);
    };
    /**
     * Gets a frozen left column header by column index.
     *
     * @param  {number} index - Specifies the column index.
     * @returns {Element} Returns the Element
     */
    TreeGrid.prototype.getFrozenLeftColumnHeaderByIndex = function (index) {
        return this.grid.getFrozenLeftColumnHeaderByIndex(index);
    };
    /**
     * Gets a frozen right column header by column index.
     *
     * @param  {number} index - Specifies the column index.
     * @returns {Element} Returns the Element
     */
    TreeGrid.prototype.getFrozenRightColumnHeaderByIndex = function (index) {
        return this.grid.getFrozenRightColumnHeaderByIndex(index);
    };
    /**
     * Gets a movable column header by column index.
     *
     * @param  {number} index - Specifies the column index.
     * @returns {Element} Returns the Element
     */
    TreeGrid.prototype.getMovableColumnHeaderByIndex = function (index) {
        return this.grid.getMovableColumnHeaderByIndex(index);
    };
    /**
     * @hidden
     * @returns {number} Returns the movable column count
     */
    TreeGrid.prototype.getMovableColumnsCount = function () {
        return this.grid.getMovableColumnsCount();
    };
    /**
     * @hidden
     * @returns {number} Returns the Frozen Left column
     */
    TreeGrid.prototype.getFrozenLeftColumnsCount = function () {
        return this.grid.getFrozenLeftColumnsCount();
    };
    /**
     * @hidden
     * @returns {number} Returns the Frozen Right column count
     */
    TreeGrid.prototype.getFrozenRightColumnsCount = function () {
        return this.grid.getFrozenRightColumnsCount();
    };
    /**
     * @hidden
     * @returns {Column[]} Returns the column
     */
    TreeGrid.prototype.getFrozenLeftColumns = function () {
        this.updateColumnModel(this.grid.getFrozenLeftColumns());
        return this.columnModel;
    };
    /**
     * @hidden
     * @returns {Column[]} Returns the column
     */
    TreeGrid.prototype.getFrozenRightColumns = function () {
        this.updateColumnModel(this.grid.getFrozenRightColumns());
        return this.columnModel;
    };
    /**
     * @hidden
     * @returns {number} Returns the visible movable count
     */
    TreeGrid.prototype.getVisibleMovableCount = function () {
        return this.grid.getVisibleMovableCount();
    };
    /**
     * @hidden
     * @returns {number} Returns the visible Frozen Right count
     */
    TreeGrid.prototype.getVisibleFrozenRightCount = function () {
        return this.grid.getVisibleFrozenRightCount();
    };
    /**
     * @hidden
     * @returns {number} Returns the visible Frozen left count
     */
    TreeGrid.prototype.getVisibleFrozenLeftCount = function () {
        return this.grid.getVisibleFrozenLeftCount();
    };
    /**
     * @hidden
     * @returns {Column[]} Returns the column
     */
    TreeGrid.prototype.getMovableColumns = function () {
        this.updateColumnModel(this.grid.getMovableColumns());
        return this.columnModel;
    };
    /**
     * Gets the number of frozen column in tree grid
     *
     * @hidden
     * @returns {number} - Returns frozen column count
     */
    TreeGrid.prototype.getFrozenColumns = function () {
        return this.getFrozenCount(this.columns, 0) + this.frozenColumns;
    };
    TreeGrid.prototype.getFrozenCount = function (cols, cnt) {
        for (var j = 0, len = cols.length; j < len; j++) {
            if (cols[j].columns) {
                cnt = this.getFrozenCount(cols[j].columns, cnt);
            }
            else {
                if (cols[j].isFrozen) {
                    cnt++;
                }
            }
        }
        return cnt;
    };
    /**
     * Gets the collection of selected row indexes.
     *
     * @returns {number[]} - Returns selected rows index collection
     */
    TreeGrid.prototype.getSelectedRowIndexes = function () {
        return this.grid.getSelectedRowIndexes();
    };
    /**
     * Gets the collection of selected row and cell indexes.
     *
     * @returns {ISelectedCell[]} - Returns selected cell's index details
     */
    TreeGrid.prototype.getSelectedRowCellIndexes = function () {
        return this.grid.getSelectedRowCellIndexes();
    };
    /**
     * Gets the collection of selected records.
     *
     * @isGenericType true
     * @returns {Object[]} - Returns selected records collection
     */
    TreeGrid.prototype.getSelectedRecords = function () {
        return this.grid.getSelectedRecords();
    };
    /**
     * Gets the data module.
     *
     * @returns {{baseModule: Data, treeModule: DataManipulation}}: Returns grid and treegrid data module
     */
    TreeGrid.prototype.getDataModule = function () {
        return { baseModule: this.grid.getDataModule(), treeModule: this.dataModule };
    };
    /**
     * Reorder the rows based on given indexes and position
     *
     * @param {number[]} fromIndexes - Source indexes of rows
     * @param {number} toIndex - Destination index of row
     * @param {string} position - Defines drop position as above or below or child
     * @returns {void}
     */
    TreeGrid.prototype.reorderRows = function (fromIndexes, toIndex, position) {
        this.rowDragAndDropModule.reorderRows(fromIndexes, toIndex, position);
    };
    var TreeGrid_1;
    __decorate([
        sf.base.Property(0)
    ], TreeGrid.prototype, "frozenRows", void 0);
    __decorate([
        sf.base.Property(0)
    ], TreeGrid.prototype, "frozenColumns", void 0);
    __decorate([
        sf.base.Property('Ellipsis')
    ], TreeGrid.prototype, "clipMode", void 0);
    __decorate([
        sf.base.Property([])
    ], TreeGrid.prototype, "columns", void 0);
    __decorate([
        sf.base.Property(null)
    ], TreeGrid.prototype, "childMapping", void 0);
    __decorate([
        sf.base.Property(null)
    ], TreeGrid.prototype, "hasChildMapping", void 0);
    __decorate([
        sf.base.Property(0)
    ], TreeGrid.prototype, "treeColumnIndex", void 0);
    __decorate([
        sf.base.Property(null)
    ], TreeGrid.prototype, "idMapping", void 0);
    __decorate([
        sf.base.Property(null)
    ], TreeGrid.prototype, "parentIdMapping", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "enableCollapseAll", void 0);
    __decorate([
        sf.base.Property(null)
    ], TreeGrid.prototype, "expandStateMapping", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowRowDragAndDrop", void 0);
    __decorate([
        sf.base.Property([])
    ], TreeGrid.prototype, "dataSource", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "query", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "cloneQuery", void 0);
    __decorate([
        sf.base.Property('AllPages')
    ], TreeGrid.prototype, "printMode", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowPaging", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "loadChildOnDemand", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowTextWrap", void 0);
    __decorate([
        sf.base.Complex({}, TextWrapSettings)
    ], TreeGrid.prototype, "textWrapSettings", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowReordering", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowResizing", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "autoCheckHierarchy", void 0);
    __decorate([
        sf.base.Complex({}, PageSettings)
    ], TreeGrid.prototype, "pageSettings", void 0);
    __decorate([
        sf.base.Complex({}, sf.grids.RowDropSettings)
    ], TreeGrid.prototype, "rowDropSettings", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "pagerTemplate", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "showColumnMenu", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "showColumnChooser", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowSorting", void 0);
    __decorate([
        sf.base.Property(true)
    ], TreeGrid.prototype, "allowMultiSorting", void 0);
    __decorate([
        sf.base.Complex({}, SortSettings)
    ], TreeGrid.prototype, "sortSettings", void 0);
    __decorate([
        sf.base.Collection([], AggregateRow)
    ], TreeGrid.prototype, "aggregates", void 0);
    __decorate([
        sf.base.Complex({}, EditSettings)
    ], TreeGrid.prototype, "editSettings", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowFiltering", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "detailTemplate", void 0);
    __decorate([
        sf.base.Complex({}, FilterSettings)
    ], TreeGrid.prototype, "filterSettings", void 0);
    __decorate([
        sf.base.Complex({}, SearchSettings)
    ], TreeGrid.prototype, "searchSettings", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "toolbar", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "toolbarTemplate", void 0);
    __decorate([
        sf.base.Property('Default')
    ], TreeGrid.prototype, "gridLines", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "contextMenuItems", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "columnMenuItems", void 0);
    __decorate([
        sf.base.Property()
    ], TreeGrid.prototype, "rowTemplate", void 0);
    __decorate([
        sf.base.Property('Parent')
    ], TreeGrid.prototype, "copyHierarchyMode", void 0);
    __decorate([
        sf.base.Property(null)
    ], TreeGrid.prototype, "rowHeight", void 0);
    __decorate([
        sf.base.Property(true)
    ], TreeGrid.prototype, "enableAltRow", void 0);
    __decorate([
        sf.base.Property(true)
    ], TreeGrid.prototype, "allowKeyboard", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "enableHover", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "enableAutoFill", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "enableImmutableMode", void 0);
    __decorate([
        sf.base.Property('auto')
    ], TreeGrid.prototype, "height", void 0);
    __decorate([
        sf.base.Property('auto')
    ], TreeGrid.prototype, "width", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "enableVirtualization", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "enableInfiniteScrolling", void 0);
    __decorate([
        sf.base.Complex({}, InfiniteScrollSettings)
    ], TreeGrid.prototype, "infiniteScrollSettings", void 0);
    __decorate([
        sf.base.Property('All')
    ], TreeGrid.prototype, "columnQueryMode", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "created", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "load", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "expanding", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "expanded", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "collapsing", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "collapsed", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "cellSave", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "cellSaved", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "actionBegin", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "actionComplete", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beginEdit", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "batchAdd", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "batchDelete", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "batchCancel", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforeBatchAdd", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforeBatchDelete", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforeBatchSave", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "cellEdit", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "actionFailure", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "dataBound", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "dataSourceChanged", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "dataStateChange", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "recordDoubleClick", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowDataBound", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "detailDataBound", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "queryCellInfo", void 0);
    __decorate([
        sf.base.Property(true)
    ], TreeGrid.prototype, "allowSelection", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowSelecting", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowSelected", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowDeselecting", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowDeselected", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "headerCellInfo", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "cellSelecting", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "columnMenuOpen", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "columnMenuClick", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "cellSelected", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "cellDeselecting", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "cellDeselected", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "resizeStart", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "resizing", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "resizeStop", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "columnDragStart", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "columnDrag", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "columnDrop", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "checkboxChange", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "printComplete", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforePrint", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "toolbarClick", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforeDataBound", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "contextMenuOpen", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "contextMenuClick", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforeCopy", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforePaste", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowDrag", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowDragStart", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowDragStartHelper", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "rowDrop", void 0);
    __decorate([
        sf.base.Property(-1)
    ], TreeGrid.prototype, "selectedRowIndex", void 0);
    __decorate([
        sf.base.Complex({}, SelectionSettings)
    ], TreeGrid.prototype, "selectionSettings", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowExcelExport", void 0);
    __decorate([
        sf.base.Property(false)
    ], TreeGrid.prototype, "allowPdfExport", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "pdfQueryCellInfo", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "pdfHeaderQueryCellInfo", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "excelQueryCellInfo", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "excelHeaderQueryCellInfo", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforeExcelExport", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "excelExportComplete", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "beforePdfExport", void 0);
    __decorate([
        sf.base.Event()
    ], TreeGrid.prototype, "pdfExportComplete", void 0);
    TreeGrid = TreeGrid_1 = __decorate([
        sf.base.NotifyPropertyChanges
    ], TreeGrid);
    return TreeGrid;
}(sf.base.Component));

/**
 * TreeGrid Reorder module
 *
 * @hidden
 */
var Reorder$1 = /** @class */ (function () {
    /**
     * Constructor for Reorder module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Reorder$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.Reorder);
        this.parent = parent;
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Reorder module name
     */
    Reorder$$1.prototype.getModuleName = function () {
        return 'reorder';
    };
    /**
     * @hidden
     * @returns {void}
     */
    Reorder$$1.prototype.addEventListener = function () {
        this.parent.on('getColumnIndex', this.getTreeColumn, this);
    };
    Reorder$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('getColumnIndex', this.getTreeColumn);
    };
    /**
     * To destroy the Reorder
     *
     * @returns {void}
     * @hidden
     */
    Reorder$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    Reorder$$1.prototype.getTreeColumn = function () {
        var columnModel = 'columnModel';
        var treeColumn = this.parent[columnModel][this.parent.treeColumnIndex];
        var treeIndex;
        var updatedCols = this.parent.getColumns();
        for (var f = 0; f < updatedCols.length; f++) {
            var treeColumnfield = sf.grids.getObject('field', treeColumn);
            var parentColumnfield = sf.grids.getObject('field', updatedCols[f]);
            if (treeColumnfield === parentColumnfield) {
                treeIndex = f;
                break;
            }
        }
        this.parent.setProperties({ treeColumnIndex: treeIndex }, true);
    };
    return Reorder$$1;
}());

/**
 * TreeGrid Resize module
 *
 * @hidden
 */
var Resize$1 = /** @class */ (function () {
    /**
     * Constructor for Resize module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Resize$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.Resize);
        this.parent = parent;
    }
    /**
     * Resize by field names.
     *
     * @param  {string|string[]} fName - Defines the field name.
     * @returns {void}
     */
    Resize$$1.prototype.autoFitColumns = function (fName) {
        this.parent.grid.autoFitColumns(fName);
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Resize module name
     */
    Resize$$1.prototype.getModuleName = function () {
        return 'resize';
    };
    /**
     * Destroys the Resize.
     *
     * @function destroy
     * @returns {void}
     */
    Resize$$1.prototype.destroy = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.grid.resizeModule.destroy();
    };
    return Resize$$1;
}());

/**
 * TreeGrid RowDragAndDrop module
 *
 * @hidden
 */
var RowDD$1 = /** @class */ (function () {
    /**
     * Constructor for render module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function RowDD$$1(parent) {
        /** @hidden */
        this.canDrop = true;
        /** @hidden */
        this.isDraggedWithChild = false;
        /** @hidden */
        this.isaddtoBottom = false;
        sf.grids.Grid.Inject(sf.grids.RowDD);
        this.parent = parent;
        this.addEventListener();
    }
    RowDD$$1.prototype.getChildrecordsByParentID = function (id) {
        var treeGridDataSource;
        if (this.parent.dataSource instanceof sf.data.DataManager && isOffline(this.parent)) {
            treeGridDataSource = this.parent.grid.dataSource.dataSource.json;
        }
        else {
            treeGridDataSource = this.parent.grid.dataSource;
        }
        var record = treeGridDataSource.filter(function (e) {
            return e.uniqueID === id;
        });
        return record;
    };
    /**
     * @hidden
     * @returns {void}
     */
    RowDD$$1.prototype.addEventListener = function () {
        this.parent.on(rowdraging, this.Rowdraging, this);
        this.parent.on(rowDropped, this.rowDropped, this);
        this.parent.on(rowsAdd, this.rowsAdded, this);
        this.parent.on(rowsRemove, this.rowsRemoved, this);
    };
    /**
     * Reorder the rows based on given indexes and position
     *
     * @returns {void}
     * @param {number[]} fromIndexes - source indexes of rows to be re-ordered
     * @param {number} toIndex - Destination row index
     * @param {string} position - Drop position as above or below or child
     */
    RowDD$$1.prototype.reorderRows = function (fromIndexes, toIndex, position) {
        var tObj = this.parent;
        if (fromIndexes[0] !== toIndex && ['above', 'below', 'child'].indexOf(position) !== -1) {
            if (position === 'above') {
                this.dropPosition = 'topSegment';
            }
            if (position === 'below') {
                this.dropPosition = 'bottomSegment';
            }
            if (position === 'child') {
                this.dropPosition = 'middleSegment';
            }
            var data = [];
            for (var i = 0; i < fromIndexes.length; i++) {
                data[i] = this.parent.getCurrentViewRecords()[fromIndexes[i]];
            }
            var isByMethod = true;
            var args = {
                data: data,
                dropIndex: toIndex
            };
            if (!isCountRequired(this.parent)) {
                this.dropRows(args, isByMethod);
            }
            //this.refreshGridDataSource();
            if (tObj.isLocalData) {
                tObj.flatData = this.orderToIndex(tObj.flatData);
            }
            this.parent.grid.refresh();
            if (this.parent.enableImmutableMode && this.dropPosition === 'middleSegment') {
                var index = this.parent.treeColumnIndex + 1;
                var row = this.parent.getRows()[fromIndexes[0]];
                var dropData = args.data[0];
                var totalRecord = [];
                var rows = [];
                totalRecord.push(dropData);
                rows.push(row);
                var parentUniqueID = 'parentUniqueID';
                var parentData = getParentData(this.parent, args.data[0][parentUniqueID]);
                var parentrow = this.parent.getRows()[toIndex];
                totalRecord.push(parentData);
                rows.push(parentrow);
                for (var i = 0; i < totalRecord.length; i++) {
                    this.parent.renderModule.cellRender({
                        data: totalRecord[i], cell: rows[i].cells[index],
                        column: this.parent.grid.getColumns()[this.parent.treeColumnIndex],
                        requestType: 'rowDragAndDrop'
                    });
                }
            }
        }
        else {
            return;
        }
    };
    RowDD$$1.prototype.orderToIndex = function (currentData) {
        var _loop_1 = function (i) {
            currentData[i].index = i;
            if (!sf.base.isNullOrUndefined(currentData[i].parentItem)) {
                var updatedParent = currentData.filter(function (data) {
                    return data.uniqueID === currentData[i].parentUniqueID;
                })[0];
                currentData[i].parentItem.index = updatedParent.index;
            }
        };
        for (var i = 0; i < currentData.length; i++) {
            _loop_1(i);
        }
        return currentData;
    };
    RowDD$$1.prototype.rowsAdded = function (e) {
        var draggedRecord;
        var dragRecords = e.records;
        for (var i = e.records.length - 1; i > -1; i--) {
            draggedRecord = dragRecords[i];
            if (draggedRecord.parentUniqueID) {
                var record = dragRecords.filter(function (data) {
                    return data.uniqueID === draggedRecord.parentUniqueID;
                });
                if (record.length) {
                    var index = record[0].childRecords.indexOf(draggedRecord);
                    var parentRecord = record[0];
                    if (index !== -1) {
                        if (sf.base.isNullOrUndefined(this.parent.idMapping)) {
                            parentRecord.childRecords.splice(index, 1);
                            if (!parentRecord.childRecords.length) {
                                parentRecord.hasChildRecords = false;
                                parentRecord.hasFilteredChildRecords = false;
                            }
                        }
                        this.isDraggedWithChild = true;
                    }
                }
            }
        }
        if (sf.base.isNullOrUndefined(this.parent.dataSource) || !this.parent.dataSource.length) {
            var tObj = this.parent;
            var draggedRecord_1;
            var dragRecords_1 = e.records;
            var dragLength = e.records.length;
            for (var i = dragLength - 1; i > -1; i--) {
                draggedRecord_1 = dragRecords_1[i];
                if (!i && draggedRecord_1.hasChildRecords) {
                    draggedRecord_1.taskData[this.parent.parentIdMapping] = null;
                }
                var recordIndex1 = 0;
                if (!sf.base.isNullOrUndefined(tObj.parentIdMapping)) {
                    tObj.childMapping = null;
                }
                if (!sf.base.isNullOrUndefined(draggedRecord_1.taskData) && !sf.base.isNullOrUndefined(tObj.childMapping) &&
                    !Object.prototype.hasOwnProperty.call(draggedRecord_1.taskData, tObj.childMapping)) {
                    draggedRecord_1.taskData[tObj.childMapping] = [];
                }
                if (Object.prototype.hasOwnProperty.call(draggedRecord_1, tObj.childMapping) &&
                    (draggedRecord_1[tObj.childMapping]).length && !this.isDraggedWithChild &&
                    !sf.base.isNullOrUndefined(tObj.parentIdMapping)) {
                    var childData = (draggedRecord_1[tObj.childMapping]);
                    for (var j = 0; j < childData.length; j++) {
                        if (dragRecords_1.indexOf(childData[j]) === -1) {
                            dragRecords_1.splice(j, 0, childData[j]);
                            childData[j].taskData = sf.base.extend({}, childData[j]);
                            i += 1;
                        }
                    }
                }
                if (Object.prototype.hasOwnProperty.call(draggedRecord_1, tObj.parentIdMapping) && draggedRecord_1[tObj.parentIdMapping] != null
                    && !this.isDraggedWithChild) {
                    draggedRecord_1.taskData[tObj.parentIdMapping] = null;
                    delete draggedRecord_1.parentItem;
                    delete draggedRecord_1.parentUniqueID;
                }
                if (sf.base.isNullOrUndefined(tObj.dataSource)) {
                    tObj.dataSource = [];
                }
                tObj.dataSource.splice(recordIndex1, 0, draggedRecord_1.taskData);
            }
            tObj.setProperties({ dataSource: tObj.dataSource }, false);
        }
        else {
            for (var i = 0; i < dragRecords.length; i++) {
                sf.base.setValue('uniqueIDCollection.' + dragRecords[i].uniqueID, dragRecords[i], this.parent);
            }
            var args = { data: e.records, dropIndex: e.toIndex };
            if (this.parent.dataSource instanceof sf.data.DataManager) {
                this.treeGridData = this.parent.dataSource.dataSource.json;
                this.treeData = this.parent.dataSource.dataSource.json;
            }
            else {
                this.treeGridData = this.parent.grid.dataSource;
                this.treeData = this.parent.dataSource;
            }
            if (sf.base.isNullOrUndefined(this.dropPosition)) {
                this.dropPosition = 'bottomSegment';
                args.dropIndex = this.parent.getCurrentViewRecords().length > 1 ? this.parent.getCurrentViewRecords().length - 1 :
                    args.dropIndex;
                args.data = args.data.map(function (i) {
                    if (i.hasChildRecords && sf.base.isNullOrUndefined(i.parentItem)) {
                        i.level = 0;
                        return i;
                    }
                    else {
                        delete i.parentItem;
                        delete i.parentUniqueID;
                        i.level = 0;
                        return i;
                    }
                });
            }
            this.dropRows(args);
        }
    };
    RowDD$$1.prototype.rowsRemoved = function (e) {
        for (var i = 0; i < e.records.length; i++) {
            this.draggedRecord = e.records[i];
            if (this.draggedRecord.hasChildRecords || this.draggedRecord.parentItem &&
                this.parent.grid.dataSource.
                    indexOf(this.getChildrecordsByParentID(this.draggedRecord.parentUniqueID)[0]) !== -1 ||
                this.draggedRecord.level === 0) {
                this.deleteDragRow();
            }
        }
    };
    RowDD$$1.prototype.refreshGridDataSource = function () {
        var draggedRecord = this.draggedRecord;
        var droppedRecord = this.droppedRecord;
        var proxy = this.parent;
        var tempDataSource;
        var idx;
        if (this.parent.dataSource instanceof sf.data.DataManager && isOffline(this.parent)) {
            tempDataSource = proxy.dataSource.dataSource.json;
        }
        else {
            tempDataSource = proxy.dataSource;
        }
        // eslint-disable-next-line max-len
        if (tempDataSource && (!sf.base.isNullOrUndefined(droppedRecord) && !droppedRecord.parentItem) && !sf.base.isNullOrUndefined(droppedRecord.taskData)) {
            for (var i = 0; i < Object.keys(tempDataSource).length; i++) {
                if (tempDataSource[i][this.parent.childMapping] === droppedRecord.taskData[this.parent.childMapping]) {
                    idx = i;
                }
            }
            if (this.dropPosition === 'topSegment') {
                if (!this.parent.idMapping) {
                    tempDataSource.splice(idx, 0, draggedRecord.taskData);
                }
            }
            else if (this.dropPosition === 'bottomSegment') {
                if (!this.parent.idMapping) {
                    tempDataSource.splice(idx + 1, 0, draggedRecord.taskData);
                }
            }
        }
        else if (!this.parent.parentIdMapping && (!sf.base.isNullOrUndefined(droppedRecord) && droppedRecord.parentItem)) {
            if (this.dropPosition === 'topSegment' || this.dropPosition === 'bottomSegment') {
                var record = this.getChildrecordsByParentID(droppedRecord.parentUniqueID)[0];
                var childRecords = record.childRecords;
                for (var i = 0; i < childRecords.length; i++) {
                    droppedRecord.parentItem.taskData[this.parent.childMapping][i] = childRecords[i].taskData;
                }
            }
        }
        if (this.parent.parentIdMapping) {
            if (draggedRecord.parentItem) {
                if (this.dropPosition === 'topSegment' || this.dropPosition === 'bottomSegment') {
                    draggedRecord[this.parent.parentIdMapping] = droppedRecord[this.parent.parentIdMapping];
                    draggedRecord.taskData[this.parent.parentIdMapping] = droppedRecord[this.parent.parentIdMapping];
                }
                else {
                    draggedRecord[this.parent.parentIdMapping] = droppedRecord[this.parent.idMapping];
                    draggedRecord.taskData[this.parent.parentIdMapping] = droppedRecord[this.parent.idMapping];
                }
            }
            else {
                draggedRecord.taskData[this.parent.parentIdMapping] = null;
                draggedRecord[this.parent.parentIdMapping] = null;
            }
        }
    };
    RowDD$$1.prototype.removeFirstrowBorder = function (element) {
        var canremove = this.dropPosition === 'bottomSegment';
        if (this.parent.element.getElementsByClassName('e-firstrow-border').length > 0 && element &&
            (element.rowIndex !== 0 || canremove)) {
            this.parent.element.getElementsByClassName('e-firstrow-border')[0].remove();
        }
    };
    RowDD$$1.prototype.removeLastrowBorder = function (element) {
        var isEmptyRow = element && (element.classList.contains('e-emptyrow') || element.classList.contains('e-columnheader')
            || element.classList.contains('e-detailrow'));
        var islastRowIndex = element && !isEmptyRow &&
            this.parent.getRowByIndex(this.parent.getCurrentViewRecords().length - 1).getAttribute('data-uid') !==
                element.getAttribute('data-uid');
        var canremove = islastRowIndex || this.dropPosition === 'topSegment';
        if (this.parent.element.getElementsByClassName('e-lastrow-border').length > 0 && element && (islastRowIndex || canremove)) {
            this.parent.element.getElementsByClassName('e-lastrow-border')[0].remove();
        }
    };
    RowDD$$1.prototype.updateIcon = function (row, index, args) {
        var rowEle = args.target ? sf.base.closest(args.target, 'tr') : null;
        this.dropPosition = undefined;
        var rowPositionHeight = 0;
        this.removeFirstrowBorder(rowEle);
        this.removeLastrowBorder(rowEle);
        for (var i = 0; i < args.rows.length; i++) {
            if (!sf.base.isNullOrUndefined(rowEle) && rowEle.getAttribute('data-uid') === args.rows[i].getAttribute('data-uid')
                || !sf.grids.parentsUntil(args.target, 'e-gridcontent')) {
                this.dropPosition = 'Invalid';
                this.addErrorElem();
            }
        }
        // To get the corresponding drop position related to mouse position
        var tObj = this.parent;
        var rowTop = 0;
        var roundOff = 0;
        var toolHeight = tObj.toolbar && tObj.toolbar.length ?
            document.getElementById(tObj.element.id + '_gridcontrol_toolbarItems').offsetHeight : 0;
        // tObj.lastRow = tObj.getRowByIndex(tObj.getCurrentViewRecords().length - 1);
        var positionOffSet = this.getOffset(tObj.element);
        // let contentHeight1: number = (tObj.element.offsetHeight  - (tObj.getContent() as HTMLElement).offsetHeight) + positionOffSet.top;
        var contentHeight = tObj.getHeaderContent().offsetHeight + positionOffSet.top + toolHeight;
        var scrollTop = tObj.getContent().firstElementChild.scrollTop;
        if (!sf.base.isNullOrUndefined(rowEle)) {
            rowPositionHeight = rowEle.offsetTop - scrollTop;
        }
        // let scrollTop = (tObj.grid.scrollModule as any).content.scrollTop;
        rowTop = rowPositionHeight + contentHeight + roundOff;
        var rowBottom = rowTop + row[0].offsetHeight;
        var difference = rowBottom - rowTop;
        var divide = difference / 3;
        var topRowSegment = rowTop + divide;
        var middleRowSegment = topRowSegment + divide;
        var bottomRowSegment = middleRowSegment + divide;
        var mouseEvent = sf.grids.getObject('originalEvent.event', args);
        var touchEvent = sf.grids.getObject('originalEvent.event', args);
        var posy = (mouseEvent.type == "mousemove") ? mouseEvent.pageY : ((!sf.base.isNullOrUndefined(touchEvent) && !sf.base.isNullOrUndefined(touchEvent.changedTouches)) ? touchEvent.changedTouches[0].pageY : null);
        var isTopSegment = posy <= topRowSegment;
        var isMiddleRowSegment = (posy > topRowSegment && posy <= middleRowSegment);
        var isBottomRowSegment = (posy > middleRowSegment && posy <= bottomRowSegment);
        if (isTopSegment || isMiddleRowSegment || isBottomRowSegment) {
            if (isTopSegment && this.dropPosition !== 'Invalid') {
                this.removeChildBorder();
                this.dropPosition = 'topSegment';
                this.removetopOrBottomBorder();
                this.addFirstrowBorder(rowEle);
                this.removeErrorElem();
                this.removeLastrowBorder(rowEle);
                this.topOrBottomBorder(args.target);
            }
            if (isMiddleRowSegment && this.dropPosition !== 'Invalid') {
                this.removetopOrBottomBorder();
                var rowElement = [];
                var element = sf.base.closest(args.target, 'tr');
                rowElement = [].slice.call(element.querySelectorAll('.e-rowcell,.e-rowdragdrop,.e-detailrowcollapse'));
                if (rowElement.length > 0) {
                    this.addRemoveClasses(rowElement, true, 'e-childborder');
                }
                this.addLastRowborder(rowEle);
                this.addFirstrowBorder(rowEle);
                this.dropPosition = 'middleSegment';
            }
            if (isBottomRowSegment && this.dropPosition !== 'Invalid') {
                this.removeErrorElem();
                this.removetopOrBottomBorder();
                this.removeChildBorder();
                this.dropPosition = 'bottomSegment';
                this.addLastRowborder(rowEle);
                this.removeFirstrowBorder(rowEle);
                this.topOrBottomBorder(args.target);
            }
        }
        return this.dropPosition;
    };
    RowDD$$1.prototype.removeChildBorder = function () {
        var borderElem = [];
        borderElem = [].slice.call(this.parent.element.querySelectorAll('.e-childborder'));
        if (borderElem.length > 0) {
            this.addRemoveClasses(borderElem, false, 'e-childborder');
        }
    };
    RowDD$$1.prototype.addFirstrowBorder = function (targetRow) {
        var node = this.parent.element;
        var tObj = this.parent;
        if (targetRow && targetRow.rowIndex === 0 && !targetRow.classList.contains('e-emptyrow')) {
            var div = this.parent.createElement('div', { className: 'e-firstrow-border' });
            var gridheaderEle = this.parent.getHeaderContent();
            var toolbarHeight = 0;
            if (tObj.toolbar) {
                toolbarHeight = tObj.toolbarModule.getToolbar().offsetHeight;
            }
            var multiplegrid = !sf.base.isNullOrUndefined(this.parent.rowDropSettings.targetID);
            if (multiplegrid) {
                div.style.top = this.parent.grid.element.getElementsByClassName('e-gridheader')[0].offsetHeight
                    + toolbarHeight + 'px';
            }
            div.style.width = multiplegrid ? node.offsetWidth + 'px' :
                node.offsetWidth - this.getScrollWidth() + 'px';
            if (!gridheaderEle.querySelectorAll('.e-firstrow-border').length) {
                gridheaderEle.appendChild(div);
            }
        }
    };
    RowDD$$1.prototype.addLastRowborder = function (trElement) {
        var isEmptyRow = trElement && (trElement.classList.contains('e-emptyrow') ||
            trElement.classList.contains('e-columnheader') || trElement.classList.contains('e-detailrow'));
        if (trElement && !isEmptyRow && this.parent.getRowByIndex(this.parent.getCurrentViewRecords().length - 1).getAttribute('data-uid') ===
            trElement.getAttribute('data-uid')) {
            var bottomborder = this.parent.createElement('div', { className: 'e-lastrow-border' });
            var gridcontentEle = this.parent.getContent();
            bottomborder.style.width = this.parent.element.offsetWidth - this.getScrollWidth() + 'px';
            if (!gridcontentEle.querySelectorAll('.e-lastrow-border').length) {
                gridcontentEle.classList.add('e-treegrid-relative');
                gridcontentEle.appendChild(bottomborder);
                bottomborder.style.bottom = this.getScrollWidth() + 'px';
            }
        }
    };
    RowDD$$1.prototype.getScrollWidth = function () {
        var scrollElem = this.parent.getContent().firstElementChild;
        return scrollElem.scrollWidth > scrollElem.offsetWidth ? sf.grids.Scroll.getScrollBarWidth() : 0;
    };
    RowDD$$1.prototype.addErrorElem = function () {
        var dragelem = document.getElementsByClassName('e-cloneproperties')[0];
        var errorelem = dragelem.querySelectorAll('.e-errorelem').length;
        if (!errorelem && !this.parent.rowDropSettings.targetID) {
            var ele = document.createElement('div');
            sf.base.classList(ele, ['e-errorcontainer'], []);
            sf.base.classList(ele, ['e-icons', 'e-errorelem'], []);
            var errorVal = dragelem.querySelector('.errorValue');
            var content = dragelem.querySelector('.e-rowcell').innerHTML;
            if (errorVal) {
                content = errorVal.innerHTML;
                errorVal.parentNode.removeChild(errorVal);
            }
            dragelem.querySelector('.e-rowcell').innerHTML = '';
            var spanContent = document.createElement('span');
            spanContent.className = 'errorValue';
            spanContent.style.paddingLeft = '16px';
            spanContent.innerHTML = content;
            dragelem.querySelector('.e-rowcell').appendChild(ele);
            dragelem.querySelector('.e-rowcell').appendChild(spanContent);
        }
    };
    RowDD$$1.prototype.removeErrorElem = function () {
        var errorelem = document.querySelector('.e-errorelem');
        if (errorelem) {
            errorelem.remove();
        }
    };
    RowDD$$1.prototype.topOrBottomBorder = function (target) {
        var rowElement = [];
        var element = sf.base.closest(target, 'tr');
        rowElement = element ? [].slice.call(element.querySelectorAll('.e-rowcell,.e-rowdragdrop,.e-detailrowcollapse')) : [];
        if (rowElement.length) {
            if (this.dropPosition === 'topSegment') {
                this.addRemoveClasses(rowElement, true, 'e-droptop');
                if (this.parent.element.getElementsByClassName('e-lastrow-dragborder').length > 0) {
                    this.parent.element.getElementsByClassName('e-lastrow-dragborder')[0].remove();
                }
            }
            if (this.dropPosition === 'bottomSegment') {
                this.addRemoveClasses(rowElement, true, 'e-dropbottom');
            }
        }
    };
    RowDD$$1.prototype.removetopOrBottomBorder = function () {
        var border = [];
        border = [].slice.call(this.parent.element.querySelectorAll('.e-dropbottom, .e-droptop'));
        if (border.length) {
            this.addRemoveClasses(border, false, 'e-dropbottom');
            this.addRemoveClasses(border, false, 'e-droptop');
        }
    };
    RowDD$$1.prototype.addRemoveClasses = function (cells, add, className) {
        for (var i = 0, len = cells.length; i < len; i++) {
            if (add) {
                cells[i].classList.add(className);
            }
            else {
                cells[i].classList.remove(className);
            }
        }
    };
    RowDD$$1.prototype.getOffset = function (element) {
        var box = element.getBoundingClientRect();
        var body = document.body;
        var docElem = document.documentElement;
        var scrollTop = window.pageYOffset || docElem.scrollTop || body.scrollTop;
        var scrollLeft = window.pageXOffset || docElem.scrollLeft || body.scrollLeft;
        var clientTop = docElem.clientTop || body.clientTop || 0;
        var clientLeft = docElem.clientLeft || body.clientLeft || 0;
        var top = box.top + scrollTop - clientTop;
        var left = box.left + scrollLeft - clientLeft;
        return { top: Math.round(top), left: Math.round(left) };
    };
    RowDD$$1.prototype.Rowdraging = function (args) {
        var tObj = this.parent;
        var cloneElement = this.parent.element.querySelector('.e-cloneproperties');
        cloneElement.style.cursor = '';
        var rowEle = args.target ? sf.base.closest(args.target, 'tr') : null;
        var rowIdx = rowEle ? rowEle.rowIndex : -1;
        var dragRecords = [];
        var droppedRecord = tObj.getCurrentViewRecords()[rowIdx];
        this.removeErrorElem();
        this.canDrop = true;
        if (!args.data[0]) {
            dragRecords.push(args.data);
        }
        else {
            dragRecords = args.data;
        }
        if (rowIdx !== -1) {
            this.ensuredropPosition(dragRecords, droppedRecord);
        }
        else {
            this.canDrop = false;
            this.addErrorElem();
        }
        if (!tObj.rowDropSettings.targetID && this.canDrop) {
            tObj.rowDragAndDropModule.updateIcon(args.rows, rowIdx, args);
        }
        if (tObj.rowDropSettings.targetID) {
            var dropElement = sf.grids.parentsUntil(args.target, 'e-treegrid');
            if (dropElement && dropElement.id === this.parent.rowDropSettings.targetID) {
                var srcControl = dropElement.ej2_instances[0];
                srcControl.rowDragAndDropModule.updateIcon(args.rows, rowIdx, args);
            }
        }
        if (args.target && sf.base.closest(args.target, '#' + tObj.rowDropSettings.targetID)) {
            var dropElement = sf.grids.parentsUntil(args.target, 'e-treegrid');
            if (!dropElement) {
                cloneElement.style.cursor = 'default';
            }
        }
    };
    RowDD$$1.prototype.rowDropped = function (args) {
        var tObj = this.parent;
        var parentItem = 'parentItem';
        if (!tObj.rowDropSettings.targetID) {
            if (sf.grids.parentsUntil(args.target, 'e-content')) {
                if (this.parent.element.querySelector('.e-errorelem')) {
                    this.dropPosition = 'Invalid';
                }
                sf.base.setValue('dropPosition', this.dropPosition, args);
                args.dropIndex = args.dropIndex === args.fromIndex ? this.getTargetIdx(args.target.parentElement) : args.dropIndex;
                tObj.trigger(rowDrop, args);
                if (!args.cancel) {
                    if (!isCountRequired(this.parent)) {
                        this.dropRows(args);
                    }
                    if (tObj.isLocalData) {
                        tObj.flatData = this.orderToIndex(tObj.flatData);
                    }
                    tObj.grid.refresh();
                    if (!sf.base.isNullOrUndefined(tObj.getHeaderContent().querySelector('.e-firstrow-border'))) {
                        tObj.getHeaderContent().querySelector('.e-firstrow-border').remove();
                    }
                }
            }
        }
        else {
            if (args.target && sf.base.closest(args.target, '#' + tObj.rowDropSettings.targetID) || sf.grids.parentsUntil(args.target, 'e-treegrid') &&
                sf.grids.parentsUntil(args.target, 'e-treegrid').id === tObj.rowDropSettings.targetID) {
                sf.base.setValue('dropPosition', this.dropPosition, args);
                tObj.trigger(rowDrop, args);
                if (!args.cancel && tObj.rowDropSettings.targetID) {
                    this.dragDropGrid(args);
                    if (tObj.isLocalData) {
                        tObj.flatData = this.orderToIndex(tObj.flatData);
                    }
                }
            }
        }
        this.removetopOrBottomBorder();
        this.removeChildBorder();
        if (!sf.base.isNullOrUndefined(this.parent.element.getElementsByClassName('e-firstrow-border')[0])) {
            this.parent.element.getElementsByClassName('e-firstrow-border')[0].remove();
        }
        else if (!sf.base.isNullOrUndefined(this.parent.element.getElementsByClassName('e-lastrow-border')[0])) {
            this.parent.element.getElementsByClassName('e-lastrow-border')[0].remove();
        }
        if (this.parent.enableImmutableMode && !this.parent.allowPaging && !sf.base.isNullOrUndefined(args.data[0][parentItem])) {
            var index = this.parent.treeColumnIndex;
            index = index + 1;
            var primaryKeyField = this.parent.getPrimaryKeyFieldNames()[0];
            var rowIndex = this.parent.grid.getRowIndexByPrimaryKey(args.data[0][primaryKeyField]);
            var row = this.parent.getRows()[rowIndex];
            var data = args.data[0];
            if (this.dropPosition === 'middleSegment') {
                var record = [];
                var rows = [];
                record.push(data);
                rows.push(row);
                var parentUniqueID = 'parentUniqueID';
                data = getParentData(this.parent, args.data[0][parentUniqueID]);
                rowIndex = this.parent.grid.getRowIndexByPrimaryKey(data[primaryKeyField]);
                var parentrow = this.parent.getRows()[rowIndex];
                record.push(data);
                rows.push(parentrow);
                for (var i = 0; i < record.length; i++) {
                    this.parent.renderModule.cellRender({
                        data: record[i], cell: rows[i].cells[index],
                        column: this.parent.grid.getColumns()[this.parent.treeColumnIndex],
                        requestType: 'rowDragAndDrop'
                    });
                }
                var targetEle = parentrow.getElementsByClassName('e-treegridcollapse')[0];
                if (!sf.base.isNullOrUndefined(targetEle)) {
                    sf.base.removeClass([targetEle], 'e-treegridcollapse');
                    sf.base.addClass([targetEle], 'e-treegridexpand');
                }
            }
            else {
                this.parent.renderModule.cellRender({
                    data: data, cell: row.cells[index],
                    column: this.parent.grid.getColumns()[this.parent.treeColumnIndex],
                    requestType: 'rowDragAndDrop'
                });
            }
        }
    };
    RowDD$$1.prototype.dragDropGrid = function (args) {
        var tObj = this.parent;
        var targetRow = sf.base.closest(args.target, 'tr');
        var targetIndex = isNaN(this.getTargetIdx(targetRow)) ? 0 : this.getTargetIdx(targetRow);
        var dropElement = sf.grids.parentsUntil(args.target, 'e-treegrid');
        var srcControl;
        if (dropElement && dropElement.id === this.parent.rowDropSettings.targetID && !isRemoteData(this.parent)
            && !isCountRequired(this.parent)) {
            srcControl = dropElement.ej2_instances[0];
            var records = tObj.getSelectedRecords();
            var indexes = [];
            for (var i = 0; i < records.length; i++) {
                indexes[i] = records[i].index;
            }
            var data = srcControl.dataSource;
            if (this.parent.idMapping != null && (sf.base.isNullOrUndefined(this.dropPosition) || this.dropPosition === 'bottomSegment' || this.dropPosition === 'Invalid') && !(data.length)) {
                var actualData = [];
                for (var i = 0; i < records.length; i++) {
                    if (records[i].hasChildRecords) {
                        actualData.push(records[i]);
                        var child = findChildrenRecords(records[i]);
                        for (var i_1 = 0; i_1 < child.length; i_1++) {
                            actualData.push(child[i_1]); // push child records to drop the parent record along with its child records
                        }
                    }
                }
                if (actualData.length) {
                    records = actualData;
                }
            }
            tObj.notify(rowsRemove, { indexes: indexes, records: records });
            srcControl.notify(rowsAdd, { toIndex: targetIndex, records: records });
            var srcControlFlatData = srcControl.rowDragAndDropModule.treeGridData;
            if (!sf.base.isNullOrUndefined(srcControlFlatData)) {
                for (var i = 0; i < srcControlFlatData.length; i++) {
                    srcControlFlatData[i].index = i;
                    if (!sf.base.isNullOrUndefined(srcControlFlatData[i].parentItem)) {
                        var actualIndex = sf.base.getValue('uniqueIDCollection.' + srcControlFlatData[i].parentUniqueID + '.index', srcControl);
                        srcControlFlatData[i].parentItem.index = actualIndex;
                    }
                }
            }
            tObj.grid.refresh();
            srcControl.grid.refresh();
            if (srcControl.grid.dataSource.length > 1) {
                srcControl.grid.refresh();
                if (!sf.base.isNullOrUndefined(srcControl.getHeaderContent().querySelector('.e-firstrow-border'))) {
                    srcControl.getHeaderContent().querySelector('.e-firstrow-border').remove();
                }
                if (!sf.base.isNullOrUndefined(srcControl.getContent().querySelector('.e-lastrow-border'))) {
                    srcControl.getContent().querySelector('.e-lastrow-border').remove();
                }
            }
        }
        if (isCountRequired(this.parent)) {
            srcControl = dropElement.ej2_instances[0];
            tObj.grid.refresh();
            srcControl.grid.refresh();
        }
    };
    RowDD$$1.prototype.getTargetIdx = function (targetRow) {
        return targetRow ? parseInt(targetRow.getAttribute('aria-rowindex'), 10) : 0;
    };
    RowDD$$1.prototype.getParentData = function (record) {
        var parentItem = record.parentItem;
        if (this.dropPosition === 'bottomSegment') {
            var selectedRecord = this.parent.getSelectedRecords()[0];
            this.droppedRecord = getParentData(this.parent, selectedRecord.parentItem.uniqueID);
        }
        if (this.dropPosition === 'middleSegment') {
            var level = this.parent.getSelectedRecords()[0].level;
            if (level === parentItem.level) {
                this.droppedRecord = getParentData(this.parent, parentItem.uniqueID);
            }
            else {
                this.getParentData(parentItem);
            }
        }
    };
    RowDD$$1.prototype.dropRows = function (args, isByMethod) {
        if (this.dropPosition !== 'Invalid' && !isRemoteData(this.parent)) {
            var tObj = this.parent;
            var draggedRecord = void 0;
            var droppedRecord = void 0;
            if (sf.base.isNullOrUndefined(args.dropIndex)) {
                var rowIndex = tObj.getSelectedRowIndexes()[0] - 1;
                var record = tObj.getCurrentViewRecords()[rowIndex];
                this.getParentData(record);
            }
            else {
                args.dropIndex = args.dropIndex === args.fromIndex ? this.getTargetIdx(args.target.parentElement) : args.dropIndex;
                this.droppedRecord = tObj.getCurrentViewRecords()[args.dropIndex];
            }
            var dragRecords = [];
            droppedRecord = this.droppedRecord;
            if (!args.data[0]) {
                dragRecords.push(args.data);
            }
            else {
                dragRecords = args.data;
            }
            var count = 0;
            var multiplegrid = this.parent.rowDropSettings.targetID;
            this.isMultipleGrid = multiplegrid;
            if (!multiplegrid) {
                this.ensuredropPosition(dragRecords, droppedRecord);
            }
            else {
                this.isaddtoBottom = multiplegrid && this.isDraggedWithChild;
            }
            var dragLength = dragRecords.length;
            if (!sf.base.isNullOrUndefined(this.parent.idMapping)) {
                dragRecords.reverse();
            }
            for (var i = 0; i < dragLength; i++) {
                draggedRecord = dragRecords[i];
                this.draggedRecord = draggedRecord;
                if (this.dropPosition !== 'Invalid') {
                    if (!tObj.rowDropSettings.targetID || isByMethod) {
                        this.deleteDragRow();
                    }
                    if (this.draggedRecord === this.droppedRecord) {
                        var correctIndex = this.getTargetIdx(args.target.offsetParent.parentElement);
                        if (isNaN(correctIndex)) {
                            correctIndex = this.getTargetIdx(args.target.parentElement);
                        }
                        args.dropIndex = correctIndex;
                        droppedRecord = this.droppedRecord = this.parent.getCurrentViewRecords()[args.dropIndex];
                    }
                    var recordIndex1 = this.treeGridData.indexOf(droppedRecord);
                    this.dropAtTop(recordIndex1);
                    if (this.dropPosition === 'bottomSegment') {
                        if (!droppedRecord.hasChildRecords) {
                            if (this.parent.parentIdMapping) {
                                this.treeData.splice(recordIndex1 + 1, 0, this.draggedRecord.taskData);
                            }
                            this.treeGridData.splice(recordIndex1 + 1, 0, this.draggedRecord);
                        }
                        else {
                            count = this.getChildCount(droppedRecord, 0);
                            if (this.parent.parentIdMapping) {
                                this.treeData.splice(recordIndex1 + count + 1, 0, this.draggedRecord.taskData);
                            }
                            this.treeGridData.splice(recordIndex1 + count + 1, 0, this.draggedRecord);
                        }
                        if (sf.base.isNullOrUndefined(droppedRecord.parentItem)) {
                            delete draggedRecord.parentItem;
                            draggedRecord.level = 0;
                            if (this.parent.parentIdMapping) {
                                draggedRecord[this.parent.parentIdMapping] = null;
                            }
                        }
                        if (droppedRecord.parentItem) {
                            var rec = this.getChildrecordsByParentID(droppedRecord.parentUniqueID);
                            var childRecords = rec[0].childRecords;
                            var droppedRecordIndex = childRecords.indexOf(droppedRecord) + 1;
                            childRecords.splice(droppedRecordIndex, 0, draggedRecord);
                            draggedRecord.parentItem = droppedRecord.parentItem;
                            draggedRecord.parentUniqueID = droppedRecord.parentUniqueID;
                            if (this.parent.parentIdMapping) {
                                draggedRecord[this.parent.parentIdMapping] = droppedRecord[this.parent.parentIdMapping];
                                draggedRecord.parentItem = droppedRecord.parentItem;
                                draggedRecord.level = droppedRecord.level;
                            }
                        }
                        if (draggedRecord.hasChildRecords) {
                            var level = 1;
                            this.updateChildRecordLevel(draggedRecord, level);
                            this.updateChildRecord(draggedRecord, recordIndex1 + count + 1);
                        }
                    }
                    this.dropMiddle(recordIndex1);
                }
                if (sf.base.isNullOrUndefined(draggedRecord.parentItem)) {
                    var parentRecords = tObj.parentData;
                    var newParentIndex = parentRecords.indexOf(this.droppedRecord);
                    if (this.dropPosition === 'bottomSegment') {
                        parentRecords.splice(newParentIndex + 1, 0, draggedRecord);
                    }
                    else if (this.dropPosition === 'topSegment') {
                        parentRecords.splice(newParentIndex, 0, draggedRecord);
                    }
                }
                tObj.rowDragAndDropModule.refreshGridDataSource();
            }
        }
    };
    RowDD$$1.prototype.dropMiddle = function (recordIndex) {
        var tObj = this.parent;
        var childRecords = findChildrenRecords(this.droppedRecord);
        var childRecordsLength = (sf.base.isNullOrUndefined(childRecords) ||
            childRecords.length === 0) ? recordIndex + 1 :
            childRecords.length + recordIndex + 1;
        if (this.dropPosition === 'middleSegment') {
            if (tObj.parentIdMapping) {
                this.treeData.splice(childRecordsLength, 0, this.draggedRecord.taskData);
                this.treeGridData.splice(childRecordsLength, 0, this.draggedRecord);
            }
            else {
                this.treeGridData.splice(childRecordsLength, 0, this.draggedRecord);
            }
            this.recordLevel();
            if (this.draggedRecord.hasChildRecords) {
                this.updateChildRecord(this.draggedRecord, childRecordsLength);
            }
        }
    };
    RowDD$$1.prototype.dropAtTop = function (recordIndex1) {
        var tObj = this.parent;
        if (this.dropPosition === 'topSegment') {
            if (tObj.parentIdMapping) {
                this.treeData.splice(recordIndex1, 0, this.draggedRecord.taskData);
            }
            this.draggedRecord.parentItem = this.treeGridData[recordIndex1].parentItem;
            this.draggedRecord.parentUniqueID = this.treeGridData[recordIndex1].parentUniqueID;
            this.draggedRecord.level = this.treeGridData[recordIndex1].level;
            this.treeGridData.splice(recordIndex1, 0, this.draggedRecord);
            if (this.draggedRecord.hasChildRecords) {
                var level = 1;
                this.updateChildRecord(this.draggedRecord, recordIndex1);
                this.updateChildRecordLevel(this.draggedRecord, level);
            }
            if (this.droppedRecord.parentItem) {
                var rec = this.getChildrecordsByParentID(this.droppedRecord.parentUniqueID);
                var childRecords = rec[0].childRecords;
                var droppedRecordIndex = childRecords.indexOf(this.droppedRecord);
                childRecords.splice(droppedRecordIndex, 0, this.draggedRecord);
            }
        }
    };
    RowDD$$1.prototype.recordLevel = function () {
        var tObj = this.parent;
        var draggedRecord = this.draggedRecord;
        var droppedRecord = this.droppedRecord;
        var childItem = tObj.childMapping;
        if (!droppedRecord.hasChildRecords) {
            droppedRecord.hasChildRecords = true;
            droppedRecord.hasFilteredChildRecords = true;
            if (sf.base.isNullOrUndefined(droppedRecord.childRecords) || droppedRecord.childRecords.length === 0) {
                droppedRecord.childRecords = [];
                if (!tObj.parentIdMapping && sf.base.isNullOrUndefined(droppedRecord.taskData[childItem])) {
                    droppedRecord.taskData[childItem] = [];
                }
            }
        }
        if (this.dropPosition === 'middleSegment') {
            var parentItem = sf.base.extend({}, droppedRecord);
            delete parentItem.childRecords;
            draggedRecord.parentItem = parentItem;
            draggedRecord.parentUniqueID = droppedRecord.uniqueID;
            droppedRecord.childRecords.splice(droppedRecord.childRecords.length, 0, draggedRecord);
            if (!sf.base.isNullOrUndefined(draggedRecord) && !tObj.parentIdMapping && !sf.base.isNullOrUndefined(droppedRecord.taskData[childItem])) {
                droppedRecord.taskData[tObj.childMapping].splice(droppedRecord.childRecords.length, 0, draggedRecord.taskData);
            }
            if (!draggedRecord.hasChildRecords) {
                draggedRecord.level = droppedRecord.level + 1;
            }
            else {
                var level = 1;
                draggedRecord.level = droppedRecord.level + 1;
                this.updateChildRecordLevel(draggedRecord, level);
            }
            droppedRecord.expanded = true;
        }
    };
    RowDD$$1.prototype.deleteDragRow = function () {
        if (this.parent.dataSource instanceof sf.data.DataManager && isOffline(this.parent)) {
            this.treeGridData = this.parent.grid.dataSource.dataSource.json;
            this.treeData = this.parent.dataSource.dataSource.json;
        }
        else {
            this.treeGridData = this.parent.grid.dataSource;
            this.treeData = this.parent.dataSource;
        }
        var deletedRow = getParentData(this.parent, this.draggedRecord.uniqueID);
        if (!sf.base.isNullOrUndefined(deletedRow.childRecords) && deletedRow.childRecords.length) {
            deletedRow.hasChildRecords = true;
        }
        this.removeRecords(deletedRow);
    };
    RowDD$$1.prototype.updateChildRecord = function (record, count) {
        var currentRecord;
        var tObj = this.parent;
        var length = 0;
        if (!record.hasChildRecords) {
            return 0;
        }
        length = record.childRecords.length;
        for (var i = 0; i < length; i++) {
            currentRecord = record.childRecords[i];
            count++;
            tObj.flatData.splice(count, 0, currentRecord);
            sf.base.setValue('uniqueIDCollection.' + currentRecord.uniqueID, currentRecord, this.parent);
            if (tObj.parentIdMapping) {
                this.treeData.splice(count, 0, currentRecord.taskData);
            }
            if (currentRecord.hasChildRecords) {
                count = this.updateChildRecord(currentRecord, count);
            }
        }
        return count;
    };
    RowDD$$1.prototype.updateChildRecordLevel = function (record, level) {
        var length = 0;
        var currentRecord;
        level++;
        if (!record.hasChildRecords) {
            return 0;
        }
        length = record.childRecords.length;
        for (var i = 0; i < length; i++) {
            currentRecord = record.childRecords[i];
            var parentData = void 0;
            if (record.parentItem) {
                parentData = getParentData(this.parent, record.parentItem.uniqueID);
            }
            if (sf.base.isNullOrUndefined(parentData) && !sf.base.isNullOrUndefined(record.parentItem)) {
                parentData = record.parentItem;
            }
            currentRecord.level = record.parentItem ? parentData.level + level : record.level + 1;
            if (currentRecord.hasChildRecords) {
                level--;
                level = this.updateChildRecordLevel(currentRecord, level);
            }
        }
        return level;
    };
    RowDD$$1.prototype.removeRecords = function (record) {
        var tObj = this.parent;
        var dataSource;
        if (this.parent.dataSource instanceof sf.data.DataManager && isOffline(this.parent)) {
            dataSource = this.parent.dataSource.dataSource.json;
        }
        else {
            dataSource = this.parent.dataSource;
        }
        var deletedRow = record;
        var isSelfReference = !sf.base.isNullOrUndefined(tObj.parentIdMapping);
        var flatParentData = this.getChildrecordsByParentID(deletedRow.parentUniqueID)[0];
        if (deletedRow) {
            if (deletedRow.parentItem) {
                var childRecords = flatParentData ? flatParentData.childRecords : [];
                var childIndex = 0;
                if (childRecords && childRecords.length > 0) {
                    childIndex = childRecords.indexOf(deletedRow);
                    flatParentData.childRecords.splice(childIndex, 1);
                    if (!this.parent.parentIdMapping) {
                        editAction({ value: deletedRow, action: 'delete' }, this.parent, isSelfReference, deletedRow.index, deletedRow.index);
                    }
                }
            }
            if (tObj.parentIdMapping) {
                if (deletedRow.hasChildRecords && deletedRow.childRecords.length > 0) {
                    this.removeChildItem(deletedRow);
                }
                var idx = void 0;
                var idz = void 0;
                var treeGridData = dataSource;
                for (var i = 0; i < treeGridData.length; i++) {
                    if (treeGridData[i][this.parent.idMapping] === deletedRow.taskData[this.parent.idMapping]) {
                        idx = i;
                    }
                }
                for (var i = 0; i < this.treeGridData.length; i++) {
                    if (this.treeGridData[i][this.parent.idMapping] === deletedRow.taskData[this.parent.idMapping]) {
                        idz = i;
                    }
                }
                if (idx !== -1 && !sf.base.isNullOrUndefined(idx)) {
                    dataSource.splice(idx, 1);
                }
                if (idz !== -1 && !sf.base.isNullOrUndefined(idz)) {
                    this.treeGridData.splice(idz, 1);
                }
            }
            var recordIndex = this.treeGridData.indexOf(deletedRow);
            if (!tObj.parentIdMapping) {
                var parentIndex = this.parent.parentData.indexOf(deletedRow);
                if (parentIndex !== -1) {
                    tObj.parentData.splice(parentIndex, 1);
                    dataSource.splice(parentIndex, 1);
                }
            }
            if (recordIndex === -1 && !tObj.parentIdMapping) {
                var primaryKeyField = tObj.getPrimaryKeyFieldNames()[0];
                for (var j = 0; j < this.treeGridData.length; j++) {
                    if (this.treeGridData[j][primaryKeyField] === deletedRow[primaryKeyField]) {
                        recordIndex = j;
                    }
                }
            }
            if (!tObj.parentIdMapping) {
                var deletedRecordCount = this.getChildCount(deletedRow, 0);
                this.treeGridData.splice(recordIndex, deletedRecordCount + 1);
            }
            if (deletedRow.parentItem && flatParentData && flatParentData.childRecords && !flatParentData.childRecords.length) {
                flatParentData.expanded = false;
                flatParentData.hasChildRecords = false;
                flatParentData.hasFilteredChildRecords = false;
            }
        }
    };
    RowDD$$1.prototype.removeChildItem = function (record) {
        var currentRecord;
        var idx;
        var idz;
        var dataSource;
        if (this.parent.dataSource instanceof sf.data.DataManager && isOffline(this.parent)) {
            dataSource = this.parent.dataSource.dataSource.json;
        }
        else {
            dataSource = this.parent.dataSource;
        }
        for (var i = 0; i < record.childRecords.length; i++) {
            currentRecord = record.childRecords[i];
            if (!sf.base.isNullOrUndefined(currentRecord.childRecords) && currentRecord.childRecords.length) {
                currentRecord.hasChildRecords = true;
            }
            var treeGridData = void 0;
            if (this.parent.dataSource instanceof sf.data.DataManager && isOffline(this.parent)) {
                treeGridData = this.parent.dataSource.dataSource.json;
            }
            else {
                treeGridData = this.parent.dataSource;
            }
            for (var i_2 = 0; i_2 < treeGridData.length; i_2++) {
                if (treeGridData[i_2][this.parent.idMapping] === currentRecord.taskData[this.parent.idMapping]) {
                    idx = i_2;
                }
            }
            for (var i_3 = 0; i_3 < this.treeGridData.length; i_3++) {
                if (this.treeGridData[i_3][this.parent.idMapping] === currentRecord.taskData[this.parent.idMapping]) {
                    idz = i_3;
                    break;
                }
            }
            if (idx !== -1 && !sf.base.isNullOrUndefined(idx)) {
                dataSource.splice(idx, 1);
            }
            if (idz !== -1 && !sf.base.isNullOrUndefined(idz)) {
                this.treeGridData.splice(idz, 1);
            }
            if (currentRecord.hasChildRecords) {
                this.removeChildItem(currentRecord);
            }
        }
    };
    RowDD$$1.prototype.getChildCount = function (record, count) {
        var currentRecord;
        if (!record.hasChildRecords) {
            return 0;
        }
        for (var i = 0; i < record.childRecords.length; i++) {
            currentRecord = record.childRecords[i];
            count++;
            if (currentRecord.hasChildRecords) {
                count = this.getChildCount(currentRecord, count);
            }
        }
        return count;
    };
    RowDD$$1.prototype.ensuredropPosition = function (draggedRecords, currentRecord) {
        var _this = this;
        draggedRecords.filter(function (e) {
            if (e.hasChildRecords && !sf.base.isNullOrUndefined(e.childRecords)) {
                var valid = e.childRecords.indexOf(currentRecord);
                if (valid === -1) {
                    _this.ensuredropPosition(e.childRecords, currentRecord);
                }
                else {
                    _this.dropPosition = 'Invalid';
                    _this.addErrorElem();
                    _this.canDrop = false;
                    return;
                }
            }
        });
    };
    RowDD$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * @hidden
     * @returns {void}
     */
    RowDD$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off(rowdraging, this.Rowdraging);
        this.parent.off(rowDropped, this.rowDropped);
        this.parent.off(rowsAdd, this.rowsAdded);
        this.parent.off(rowsRemove, this.rowsRemoved);
    };
    /**
     * hidden
     */
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns RowDragAndDrop module name
     */
    RowDD$$1.prototype.getModuleName = function () {
        return 'rowDragAndDrop';
    };
    return RowDD$$1;
}());

/**
 * Base export
 */

var __extends$12 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$10 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the row drop settings of the TreeGrid.
 */
var RowDropSettings$1 = /** @class */ (function (_super) {
    __extends$12(RowDropSettings$$1, _super);
    function RowDropSettings$$1() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$10([
        sf.base.Property()
    ], RowDropSettings$$1.prototype, "targetID", void 0);
    return RowDropSettings$$1;
}(sf.base.ChildProperty));

/**
 * Models export
 */

var __extends$13 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/**
 * RowModelGenerator is used to generate grid data rows.
 *
 * @hidden
 */
var TreeVirtualRowModelGenerator = /** @class */ (function (_super) {
    __extends$13(TreeVirtualRowModelGenerator, _super);
    function TreeVirtualRowModelGenerator(parent) {
        var _this = _super.call(this, parent) || this;
        _this.addEventListener();
        return _this;
    }
    TreeVirtualRowModelGenerator.prototype.addEventListener = function () {
        this.parent.on(dataListener, this.getDatas, this);
    };
    TreeVirtualRowModelGenerator.prototype.getDatas = function (args) {
        this.visualData = args.data;
    };
    TreeVirtualRowModelGenerator.prototype.generateRows = function (data, notifyArgs) {
        if ((this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.dataSource.url !== undefined
            && !this.parent.dataSource.dataSource.offline && this.parent.dataSource.dataSource.url !== '') || isCountRequired(this.parent)) {
            return _super.prototype.generateRows.call(this, data, notifyArgs);
        }
        else {
            if (!sf.base.isNullOrUndefined(notifyArgs.requestType) && notifyArgs.requestType.toString() === 'collapseAll') {
                notifyArgs.requestType = 'refresh';
            }
            var rows = _super.prototype.generateRows.call(this, data, notifyArgs);
            for (var r = 0; r < rows.length; r++) {
                rows[r].index = (this.visualData).indexOf(rows[r].data);
            }
            return rows;
        }
    };
    TreeVirtualRowModelGenerator.prototype.checkAndResetCache = function (action) {
        var clear = ['paging', 'refresh', 'sorting', 'filtering', 'searching', 'reorder',
            'save', 'delete'].some(function (value) { return action === value; });
        if ((this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.dataSource.url !== undefined
            && !this.parent.dataSource.dataSource.offline && this.parent.dataSource.dataSource.url !== '') || isCountRequired(this.parent)) {
            var model = 'model';
            var currentPage = this[model].currentPage;
            if (clear) {
                this.cache = {};
                this.data = {};
                this.groups = {};
            }
            else if (action === 'virtualscroll' && this.cache[currentPage] &&
                this.cache[currentPage].length > (this.parent.contentModule).getBlockSize()) {
                delete this.cache[currentPage];
            }
        }
        else {
            if (clear || action === 'virtualscroll') {
                this.cache = {};
                this.data = {};
                this.groups = {};
            }
        }
        return clear;
    };
    return TreeVirtualRowModelGenerator;
}(sf.grids.VirtualRowModelGenerator));

/**
 * Renderer export
 */

/**
 * TreeGrid Filter module will handle filtering action
 *
 * @hidden
 */
var Filter$1 = /** @class */ (function () {
    /**
     * Constructor for Filter module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Filter$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.Filter);
        this.parent = parent;
        this.isHierarchyFilter = false;
        this.filteredResult = [];
        this.flatFilteredData = [];
        this.filteredParentRecs = [];
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Filter module name
     */
    Filter$$1.prototype.getModuleName = function () {
        return 'filter';
    };
    /**
     * To destroy the Filter module
     *
     * @returns {void}
     * @hidden
     */
    Filter$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * @hidden
     * @returns {void}
     */
    Filter$$1.prototype.addEventListener = function () {
        this.parent.on('updateFilterRecs', this.updatedFilteredRecord, this);
        this.parent.on('clearFilters', this.clearFilterLevel, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    Filter$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('updateFilterRecs', this.updatedFilteredRecord);
        this.parent.off('clearFilters', this.clearFilterLevel);
    };
    /**
     * Function to update filtered records
     *
     * @param {{data: Object} } dataDetails - Filtered data collection
     * @param {Object} dataDetails.data - Fliltered data collection
     * @hidden
     * @returns {void}
     */
    Filter$$1.prototype.updatedFilteredRecord = function (dataDetails) {
        sf.base.setValue('uniqueIDFilterCollection', {}, this.parent);
        this.flatFilteredData = dataDetails.data;
        this.filteredParentRecs = [];
        this.filteredResult = [];
        this.isHierarchyFilter = false;
        for (var f = 0; f < this.flatFilteredData.length; f++) {
            var rec = this.flatFilteredData[f];
            this.addParentRecord(rec);
            var hierarchyMode = this.parent.grid.searchSettings.key === '' ? this.parent.filterSettings.hierarchyMode
                : this.parent.searchSettings.hierarchyMode;
            if (((hierarchyMode === 'Child' || hierarchyMode === 'None') &&
                (this.parent.grid.filterSettings.columns.length !== 0 || this.parent.grid.searchSettings.key !== ''))) {
                this.isHierarchyFilter = true;
            }
            var ischild = sf.grids.getObject('childRecords', rec);
            if (!sf.base.isNullOrUndefined(ischild) && ischild.length) {
                sf.base.setValue('hasFilteredChildRecords', this.checkChildExsist(rec), rec);
            }
            var parent_1 = sf.grids.getObject('parentItem', rec);
            if (!sf.base.isNullOrUndefined(parent_1)) {
                var parRecord = getParentData(this.parent, rec.parentItem.uniqueID, true);
                //let parRecord: Object = this.flatFilteredData.filter((e: ITreeData) => {
                //          return e.uniqueID === rec.parentItem.uniqueID; })[0];
                sf.base.setValue('hasFilteredChildRecords', true, parRecord);
                if (parRecord && parRecord.parentItem) {
                    this.updateParentFilteredRecord(parRecord);
                }
            }
        }
        if (this.flatFilteredData.length > 0 && this.isHierarchyFilter) {
            this.updateFilterLevel();
        }
        this.parent.notify('updateAction', { result: this.filteredResult });
    };
    Filter$$1.prototype.updateParentFilteredRecord = function (record) {
        var parRecord = getParentData(this.parent, record.parentItem.uniqueID, true);
        var uniqueIDValue = sf.base.getValue('uniqueIDFilterCollection', this.parent);
        if (parRecord && Object.prototype.hasOwnProperty.call(uniqueIDValue, parRecord.uniqueID)) {
            sf.base.setValue('hasFilteredChildRecords', true, parRecord);
        }
        if (parRecord && parRecord.parentItem) {
            this.updateParentFilteredRecord(parRecord);
        }
    };
    Filter$$1.prototype.addParentRecord = function (record) {
        var parent = getParentData(this.parent, record.parentUniqueID);
        //let parent: Object = this.parent.flatData.filter((e: ITreeData) => {return e.uniqueID === record.parentUniqueID; })[0];
        var hierarchyMode = this.parent.grid.searchSettings.key === '' ? this.parent.filterSettings.hierarchyMode
            : this.parent.searchSettings.hierarchyMode;
        if (hierarchyMode === 'None' && (this.parent.grid.filterSettings.columns.length !== 0
            || this.parent.grid.searchSettings.key !== '')) {
            if (sf.base.isNullOrUndefined(parent)) {
                if (this.flatFilteredData.indexOf(record) !== -1) {
                    if (this.filteredResult.indexOf(record) === -1) {
                        this.filteredResult.push(record);
                        sf.base.setValue('uniqueIDFilterCollection.' + record.uniqueID, record, this.parent);
                        record.hasFilteredChildRecords = true;
                    }
                    return;
                }
            }
            else {
                this.addParentRecord(parent);
                if (this.flatFilteredData.indexOf(parent) !== -1 || this.filteredResult.indexOf(parent) !== -1) {
                    if (this.filteredResult.indexOf(record) === -1) {
                        this.filteredResult.push(record);
                        sf.base.setValue('uniqueIDFilterCollection.' + record.uniqueID, record, this.parent);
                    }
                }
                else {
                    if (this.filteredResult.indexOf(record) === -1 && this.flatFilteredData.indexOf(record) !== -1) {
                        this.filteredResult.push(record);
                        sf.base.setValue('uniqueIDFilterCollection.' + record.uniqueID, record, this.parent);
                    }
                }
            }
        }
        else {
            if (!sf.base.isNullOrUndefined(parent)) {
                var hierarchyMode_1 = this.parent.grid.searchSettings.key === '' ?
                    this.parent.filterSettings.hierarchyMode : this.parent.searchSettings.hierarchyMode;
                if (hierarchyMode_1 === 'Child' && (this.parent.grid.filterSettings.columns.length !== 0
                    || this.parent.grid.searchSettings.key !== '')) {
                    if (this.flatFilteredData.indexOf(parent) !== -1) {
                        this.addParentRecord(parent);
                    }
                }
                else {
                    this.addParentRecord(parent);
                }
            }
            if (this.filteredResult.indexOf(record) === -1) {
                this.filteredResult.push(record);
                sf.base.setValue('uniqueIDFilterCollection.' + record.uniqueID, record, this.parent);
            }
        }
    };
    Filter$$1.prototype.checkChildExsist = function (records) {
        var childRec = sf.grids.getObject('childRecords', records);
        var isExist = false;
        for (var count = 0; count < childRec.length; count++) {
            var ischild = childRec[count].childRecords;
            var hierarchyMode = this.parent.grid.searchSettings.key === '' ?
                this.parent.filterSettings.hierarchyMode : this.parent.searchSettings.hierarchyMode;
            if (((hierarchyMode === 'Child' || hierarchyMode === 'Both') && (this.parent.grid.filterSettings.columns.length !== 0
                || this.parent.grid.searchSettings.key !== ''))) {
                var uniqueIDValue = sf.base.getValue('uniqueIDFilterCollection', this.parent);
                if (!Object.prototype.hasOwnProperty.call(uniqueIDValue, childRec[count].uniqueID)) {
                    this.filteredResult.push(childRec[count]);
                    sf.base.setValue('uniqueIDFilterCollection.' + childRec[count].uniqueID, childRec[count], this.parent);
                    isExist = true;
                }
            }
            if ((hierarchyMode === 'None')
                && (this.parent.grid.filterSettings.columns.length !== 0 || this.parent.grid.searchSettings.key !== '')) {
                if (this.flatFilteredData.indexOf(childRec[count]) !== -1) {
                    isExist = true;
                    break;
                }
            }
            if (!sf.base.isNullOrUndefined(ischild) && ischild.length) {
                isExist = this.checkChildExsist(childRec[count]);
            }
            if ((hierarchyMode === 'Child' || hierarchyMode === 'Both') && childRec.length) {
                isExist = true;
            }
        }
        return isExist;
    };
    Filter$$1.prototype.updateFilterLevel = function () {
        var record = this.filteredResult;
        var len = this.filteredResult.length;
        for (var c = 0; c < len; c++) {
            var parent_2 = getParentData(this.parent, record[c].parentUniqueID);
            var isPrst = record.indexOf(parent_2) !== -1;
            if (isPrst) {
                var parent_3 = getParentData(this.parent, record[c].parentUniqueID, true);
                record[c].filterLevel = parent_3.filterLevel + 1;
            }
            else {
                record[c].filterLevel = 0;
                this.filteredParentRecs.push(record[c]);
            }
        }
    };
    Filter$$1.prototype.clearFilterLevel = function (data) {
        var count = 0;
        var flatData = data.flatData;
        var len = flatData.length;
        var currentRecord;
        for (count; count < len; count++) {
            currentRecord = flatData[count];
            var fLevel = currentRecord.filterLevel;
            if (fLevel || fLevel === 0 || !sf.base.isNullOrUndefined(currentRecord.hasFilteredChildRecords)) {
                currentRecord.hasFilteredChildRecords = null;
                currentRecord.filterLevel = null;
            }
        }
        this.filteredResult = [];
        this.parent.notify('updateResults', { result: flatData, count: flatData.length });
    };
    return Filter$$1;
}());

/**
 * TreeGrid Excel Export module
 *
 * @hidden
 */
var ExcelExport$1 = /** @class */ (function () {
    /**
     * Constructor for Excel Export module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function ExcelExport$$1(parent) {
        this.isCollapsedStatePersist = false;
        sf.grids.Grid.Inject(sf.grids.ExcelExport);
        this.parent = parent;
        this.dataResults = {};
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns ExcelExport module name
     */
    ExcelExport$$1.prototype.getModuleName = function () {
        return 'ExcelExport';
    };
    /**
     * @hidden
     * @returns {void}
     */
    ExcelExport$$1.prototype.addEventListener = function () {
        this.parent.on('updateResults', this.updateExcelResultModel, this);
        this.parent.on('excelCellInfo', this.excelQueryCellInfo, this);
        this.parent.grid.on('export-RowDataBound', this.exportRowDataBound, this);
        this.parent.grid.on('finalPageSetup', this.finalPageSetup, this);
    };
    /**
     * To destroy the Excel Export
     *
     * @returns {void}
     * @hidden
     */
    ExcelExport$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * @hidden
     * @returns {void}
     */
    ExcelExport$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('updateResults', this.updateExcelResultModel);
        this.parent.off('excelCellInfo', this.excelQueryCellInfo);
        this.parent.grid.off('export-RowDataBound', this.exportRowDataBound);
        this.parent.grid.off('finalPageSetup', this.finalPageSetup);
    };
    ExcelExport$$1.prototype.updateExcelResultModel = function (returnResult) {
        this.dataResults = returnResult;
    };
    ExcelExport$$1.prototype.Map = function (excelExportProperties, 
    /* eslint-disable-next-line */
    isMultipleExport, workbook, isBlob, isCsv) {
        var _this = this;
        var dataSource = this.parent.dataSource;
        var property = Object();
        sf.base.setValue('isCsv', isCsv, property);
        sf.base.setValue('cancel', false, property);
        if (!sf.base.isNullOrUndefined(excelExportProperties)) {
            this.isCollapsedStatePersist = excelExportProperties.isCollapsedStatePersist;
        }
        return new Promise(function (resolve) {
            var dm = _this.isLocal() && !(dataSource instanceof sf.data.DataManager) ? new sf.data.DataManager(dataSource)
                : _this.parent.dataSource;
            var query = new sf.data.Query();
            if (!_this.isLocal()) {
                query = _this.generateQuery(query);
                sf.base.setValue('query', query, property);
            }
            _this.parent.trigger(beforeExcelExport, sf.base.extend(property, excelExportProperties));
            if (sf.grids.getObject('cancel', property)) {
                return null;
            }
            dm.executeQuery(query).then(function (e) {
                var customData = null;
                if (!sf.base.isNullOrUndefined(excelExportProperties) && !sf.base.isNullOrUndefined(excelExportProperties.dataSource)) {
                    customData = excelExportProperties.dataSource;
                }
                excelExportProperties = _this.manipulateExportProperties(excelExportProperties, dataSource, e);
                return _this.parent.grid.excelExportModule.Map(_this.parent.grid, excelExportProperties, isMultipleExport, workbook, isCsv, isBlob).then(function (book) {
                    if (customData != null) {
                        excelExportProperties.dataSource = customData;
                    }
                    else {
                        delete excelExportProperties.dataSource;
                    }
                    resolve(book);
                });
            });
        });
    };
    ExcelExport$$1.prototype.generateQuery = function (query, property) {
        if (!sf.base.isNullOrUndefined(property) && property.exportType === 'CurrentPage'
            && this.parent.allowPaging) {
            property.exportType = 'AllPages';
            query.addParams('ExportType', 'CurrentPage');
            query.where(this.parent.parentIdMapping, 'equal', null);
            query = sf.grids.getObject('grid.renderModule.data.pageQuery', this.parent)(query);
        }
        return query;
    };
    ExcelExport$$1.prototype.manipulateExportProperties = function (property, dtSrc, queryResult) {
        //count not required for this query
        var args = Object();
        sf.base.setValue('query', this.parent.grid.getDataModule().generateQuery(true), args);
        sf.base.setValue('isExport', true, args);
        if (!sf.base.isNullOrUndefined(property) && !sf.base.isNullOrUndefined(property.exportType)) {
            sf.base.setValue('exportType', property.exportType, args);
        }
        if (!this.isLocal()) {
            this.parent.parentData = [];
            this.parent.dataModule.convertToFlatData(sf.grids.getObject('result', queryResult));
            sf.base.setValue('expresults', this.parent.flatData, args);
        }
        this.parent.notify('dataProcessor', args);
        //args = this.parent.dataModule.dataProcessor(args);
        args = this.dataResults;
        dtSrc = sf.base.isNullOrUndefined(args.result) ? this.parent.flatData.slice(0) : args.result;
        if (!this.isLocal()) {
            this.parent.flatData = [];
        }
        if (property && property.dataSource && this.isLocal()) {
            var flatsData = this.parent.flatData;
            var dataSrc = property.dataSource instanceof sf.data.DataManager ? property.dataSource.dataSource.json : property.dataSource;
            this.parent.dataModule.convertToFlatData(dataSrc);
            dtSrc = this.parent.flatData;
            this.parent.flatData = flatsData;
        }
        property = sf.base.isNullOrUndefined(property) ? Object() : property;
        property.dataSource = new sf.data.DataManager({ json: dtSrc });
        return property;
    };
    /**
     * TreeGrid Excel Export cell modifier
     *
     * @param {ExcelQueryCellInfoEventArgs} args - current cell details
     * @hidden
     * @returns {void}
     */
    ExcelExport$$1.prototype.excelQueryCellInfo = function (args) {
        if (this.parent.grid.getColumnIndexByUid(args.column.uid) === this.parent.treeColumnIndex) {
            var style = {};
            var data = args.data;
            var ispadfilter = sf.base.isNullOrUndefined(data.filterLevel);
            var pad = ispadfilter ? data.level : data.filterLevel;
            style.indent = pad;
            args.style = style;
        }
        this.parent.notify('updateResults', args);
        this.parent.trigger('excelQueryCellInfo', args);
    };
    ExcelExport$$1.prototype.exportRowDataBound = function (excelRow) {
        if (excelRow.type === 'excel') {
            var excelrowobj = excelRow.rowObj.data;
            var filtercolumnlength = this.parent.grid.filterSettings.columns.length;
            if (excelrowobj.parentItem && getParentData(this.parent, excelrowobj.parentItem.uniqueID, Boolean(filtercolumnlength))) {
                var rowlength = excelRow.excelRows.length;
                var rowlevel = excelrowobj.level;
                var expandedStatus = false;
                var sublevelState = false;
                var state = getExpandStatus(this.parent, excelrowobj, this.parent.parentData);
                if (this.isCollapsedStatePersist && (!state || !this.parent.isLocalData)) {
                    expandedStatus = true;
                    sublevelState = excelrowobj.expanded ? false : true;
                }
                excelRow.excelRows[rowlength - 1].grouping = { outlineLevel: rowlevel, isCollapsed: sublevelState,
                    isHidden: expandedStatus };
            }
        }
    };
    /* eslint-disable-next-line */
    ExcelExport$$1.prototype.finalPageSetup = function (workbook) {
        for (var i = 0; i < workbook.worksheets.length; i++) {
            if (workbook.worksheets[i].rows) {
                workbook.worksheets[i].pageSetup = { isSummaryRowBelow: false };
            }
        }
    };
    ExcelExport$$1.prototype.isLocal = function () {
        return !isRemoteData(this.parent) && isOffline(this.parent);
    };
    return ExcelExport$$1;
}());

/**
 * TreeGrid PDF Export module
 *
 * @hidden
 */
var PdfExport$1 = /** @class */ (function () {
    /**
     * Constructor for PDF export module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function PdfExport$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.PdfExport);
        this.parent = parent;
        this.dataResults = {};
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} PdfExport module name
     */
    PdfExport$$1.prototype.getModuleName = function () {
        return 'PdfExport';
    };
    /**
     * @hidden
     * @returns {void}
     */
    PdfExport$$1.prototype.addEventListener = function () {
        this.parent.on('pdfCellInfo', this.pdfQueryCellInfo, this);
        this.parent.on('updateResults', this.updatePdfResultModel, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    PdfExport$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('pdfCellInfo', this.pdfQueryCellInfo);
        this.parent.off('updateResults', this.updatePdfResultModel);
    };
    /**
     * To destroy the PDF Export
     *
     * @returns {void}
     * @hidden
     */
    PdfExport$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    PdfExport$$1.prototype.updatePdfResultModel = function (returnResult) {
        this.dataResults = returnResult;
    };
    PdfExport$$1.prototype.Map = function (pdfExportProperties, isMultipleExport, pdfDoc, isBlob) {
        var _this = this;
        var dtSrc = this.parent.dataSource;
        var prop = Object();
        var isLocal = !isRemoteData(this.parent) && isOffline(this.parent);
        sf.base.setValue('cancel', false, prop);
        return new Promise(function (resolve) {
            var dm = isLocal && !(dtSrc instanceof sf.data.DataManager) ? new sf.data.DataManager(dtSrc)
                : _this.parent.dataSource;
            var query = new sf.data.Query();
            if (!isLocal) {
                query = _this.generateQuery(query);
                sf.base.setValue('query', query, prop);
            }
            _this.parent.trigger(beforePdfExport, sf.base.extend(prop, pdfExportProperties));
            if (sf.grids.getObject('cancel', prop)) {
                return null;
            }
            dm.executeQuery(query).then(function (e) {
                var customsData = null;
                if (!sf.base.isNullOrUndefined(pdfExportProperties) && !sf.base.isNullOrUndefined(pdfExportProperties.dataSource)) {
                    customsData = pdfExportProperties.dataSource;
                }
                pdfExportProperties = _this.manipulatePdfProperties(pdfExportProperties, dtSrc, e);
                return _this.parent.grid.pdfExportModule.Map(_this.parent.grid, pdfExportProperties, isMultipleExport, pdfDoc, isBlob).then(function (document) {
                    if (customsData != null) {
                        pdfExportProperties.dataSource = customsData;
                    }
                    else {
                        delete pdfExportProperties.dataSource;
                    }
                    resolve(document);
                });
            });
        });
    };
    PdfExport$$1.prototype.generateQuery = function (query, prop) {
        if (!sf.base.isNullOrUndefined(prop) && prop.exportType === 'CurrentPage'
            && this.parent.allowPaging) {
            prop.exportType = 'AllPages';
            query.addParams('ExportType', 'CurrentPage');
            query.where(this.parent.parentIdMapping, 'equal', null);
            query = sf.grids.getObject('grid.renderModule.data.pageQuery', this.parent)(query);
        }
        return query;
    };
    PdfExport$$1.prototype.manipulatePdfProperties = function (prop, dtSrc, queryResult) {
        var args = {};
        //count not required for this query
        var isLocal = !isRemoteData(this.parent) && isOffline(this.parent);
        sf.base.setValue('query', this.parent.grid.getDataModule().generateQuery(true), args);
        sf.base.setValue('isExport', true, args);
        sf.base.setValue('isPdfExport', true, args);
        if (!sf.base.isNullOrUndefined(prop) && !sf.base.isNullOrUndefined(prop.isCollapsedStatePersist)) {
            sf.base.setValue('isCollapsedStatePersist', prop.isCollapsedStatePersist, args);
        }
        if (!sf.base.isNullOrUndefined(prop) && !sf.base.isNullOrUndefined(prop.exportType)) {
            sf.base.setValue('exportType', prop.exportType, args);
        }
        if (!isLocal) {
            this.parent.parentData = [];
            this.parent.dataModule.convertToFlatData(sf.base.getValue('result', queryResult));
            sf.base.setValue('expresults', this.parent.flatData, args);
        }
        this.parent.notify('dataProcessor', args);
        //args = this.parent.dataModule.dataProcessor(args);
        args = this.dataResults;
        dtSrc = sf.base.isNullOrUndefined(args.result) ? this.parent.flatData.slice(0) : args.result;
        if (!isLocal) {
            this.parent.flatData = [];
        }
        if (prop && prop.dataSource && isLocal) {
            var flatDatas = this.parent.flatData;
            var dataSrc = prop.dataSource instanceof sf.data.DataManager ? prop.dataSource.dataSource.json : prop.dataSource;
            this.parent.dataModule.convertToFlatData(dataSrc);
            dtSrc = this.parent.flatData;
            this.parent.flatData = flatDatas;
        }
        prop = sf.base.isNullOrUndefined(prop) ? {} : prop;
        prop.dataSource = new sf.data.DataManager({ json: dtSrc });
        return prop;
    };
    /**
     * TreeGrid PDF Export cell modifier
     *
     * @param {PdfQueryCellInfoEventArgs} args - Current cell details
     * @hidden
     * @returns {void}
     */
    PdfExport$$1.prototype.pdfQueryCellInfo = function (args) {
        if (this.parent.grid.getColumnIndexByUid(args.column.uid) === this.parent.treeColumnIndex) {
            var style = {};
            var data = sf.grids.getObject('data', args);
            var ispadfilter = sf.base.isNullOrUndefined(data.filterLevel);
            var pad = ispadfilter ? data.level : data.filterLevel;
            style.paragraphIndent = pad * 3;
            args.style = style;
        }
        this.parent.notify('updateResults', args);
        this.parent.trigger('pdfQueryCellInfo', args);
    };
    return PdfExport$$1;
}());

/**
 * The `Page` module is used to render pager and handle paging action.
 *
 * @hidden
 */
var Page$1 = /** @class */ (function () {
    function Page$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.Page);
        this.parent = parent;
        this.addEventListener();
    }
    /**
     * @hidden
     * @returns {void}
     */
    Page$$1.prototype.addEventListener = function () {
        this.parent.on(localPagedExpandCollapse, this.collapseExpandPagedchilds, this);
        this.parent.on(pagingActions, this.pageAction, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    Page$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off(localPagedExpandCollapse, this.collapseExpandPagedchilds);
        this.parent.off(pagingActions, this.pageAction);
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Pager module name
     */
    Page$$1.prototype.getModuleName = function () {
        return 'pager';
    };
    /**
     * Refreshes the page count, pager information, and external message.
     *
     * @returns {void}
     */
    Page$$1.prototype.refresh = function () {
        this.parent.grid.pagerModule.refresh();
    };
    /**
     * To destroy the pager
     *
     * @returns {void}
     * @hidden
     */
    Page$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * Navigates to the target page according to the given number.
     *
     * @param  {number} pageNo - Defines the page number to navigate.
     * @returns {void}
     */
    Page$$1.prototype.goToPage = function (pageNo) {
        this.parent.grid.pagerModule.goToPage(pageNo);
    };
    /**
     * Defines the text of the external message.
     *
     * @param  {string} message - Defines the message to update.
     * @returns {void}
     */
    Page$$1.prototype.updateExternalMessage = function (message) {
        this.parent.grid.pagerModule.updateExternalMessage(message);
    };
    /**
     * @param {{action: string, row: HTMLTableRowElement, record: ITreeData, args: RowCollapsedEventArgs}} rowDetails - Expand Collapse details arguments
     * @param {string} rowDetails.action - Expand Collapse action type
     * @param {HTMLTableRowElement} rowDetails.row - Row element
     * @param {ITreeData} rowDetails.record - Row object data
     * @param {RowCollapsedEventArgs} rowDetails.args - Expand Collapse event arguments
     * @hidden
     * @returns {void}
     */
    Page$$1.prototype.collapseExpandPagedchilds = function (rowDetails) {
        rowDetails.record.expanded = rowDetails.action === 'collapse' ? false : true;
        if (this.parent.enableImmutableMode) {
            var primaryKeyField_1 = this.parent.getPrimaryKeyFieldNames()[0];
            var record = this.parent.flatData.filter(function (e) {
                return e[primaryKeyField_1] === rowDetails.record[primaryKeyField_1];
            });
            if (record.length) {
                record[0].expanded = rowDetails.record.expanded;
            }
        }
        var ret = {
            result: this.parent.flatData,
            row: rowDetails.row,
            action: rowDetails.action,
            record: rowDetails.record,
            count: this.parent.flatData.length
        };
        sf.base.getValue('grid.renderModule', this.parent).dataManagerSuccess(ret);
        if (this.parent.enableImmutableMode) {
            var row = 'row';
            var action = 'action';
            var targetEle = void 0;
            if (ret[action] === 'collapse') {
                targetEle = ret[row].getElementsByClassName('e-treegridexpand')[0];
                if (!sf.base.isNullOrUndefined(targetEle)) {
                    sf.base.removeClass([targetEle], 'e-treegridexpand');
                    sf.base.addClass([targetEle], 'e-treegridcollapse');
                }
            }
            else if (ret[action] === 'expand') {
                targetEle = ret[row].getElementsByClassName('e-treegridcollapse')[0];
                if (!sf.base.isNullOrUndefined(targetEle)) {
                    sf.base.removeClass([targetEle], 'e-treegridcollapse');
                    sf.base.addClass([targetEle], 'e-treegridexpand');
                }
            }
        }
    };
    Page$$1.prototype.pageRoot = function (pagedResults, temp, result) {
        var newResults = sf.base.isNullOrUndefined(result) ? [] : result;
        var _loop_1 = function (t) {
            newResults.push(temp[t]);
            var res = [];
            if (temp[t].hasChildRecords) {
                res = pagedResults.filter(function (e) {
                    return temp[t].uniqueID === e.parentUniqueID;
                });
                newResults = this_1.pageRoot(pagedResults, res, newResults);
            }
        };
        var this_1 = this;
        for (var t = 0; t < temp.length; t++) {
            _loop_1(t);
        }
        return newResults;
    };
    Page$$1.prototype.pageAction = function (pageingDetails) {
        var _this = this;
        var dm = new sf.data.DataManager(pageingDetails.result);
        if (this.parent.pageSettings.pageSizeMode === 'Root') {
            var temp = [];
            var propname = (this.parent.grid.filterSettings.columns.length > 0) &&
                (this.parent.filterSettings.hierarchyMode === 'Child' || this.parent.filterSettings.hierarchyMode === 'None') ?
                'filterLevel' : 'level';
            var query = new sf.data.Query().where(propname, 'equal', 0);
            temp = dm.executeLocal(query);
            pageingDetails.count = temp.length;
            var size = this.parent.grid.pageSettings.pageSize;
            var current = this.parent.grid.pageSettings.currentPage;
            var skip = size * (current - 1);
            query = query.skip(skip).take(size);
            temp = dm.executeLocal(query);
            var newResults = this.pageRoot(pageingDetails.result, temp);
            pageingDetails.result = newResults;
        }
        else {
            var dm_1 = new sf.data.DataManager(pageingDetails.result);
            var expanded$$1 = new sf.data.Predicate('expanded', 'notequal', null).or('expanded', 'notequal', undefined);
            var parents_1 = dm_1.executeLocal(new sf.data.Query().where(expanded$$1));
            var visualData = void 0;
            if (isFilterChildHierarchy(this.parent) && ((this.parent.searchSettings.key !== this.parent.grid.searchSettings.key) ||
                (this.parent.filterSettings.columns.length !== this.parent.grid.filterSettings.columns.length))) {
                visualData = parents_1;
            }
            else {
                visualData = parents_1.filter(function (e) {
                    return getExpandStatus(_this.parent, e, parents_1);
                });
            }
            pageingDetails.count = visualData.length;
            var query = new sf.data.Query();
            var size = this.parent.grid.pageSettings.pageSize;
            var current = this.parent.grid.pageSettings.currentPage;
            if (visualData.length < (current * size)) {
                current = (Math.floor(visualData.length / size)) + ((visualData.length % size) ? 1 : 0);
                current = current ? current : 1;
                this.parent.grid.setProperties({ pageSettings: { currentPage: current } }, true);
            }
            var skip = size * (current - 1);
            query = query.skip(skip).take(size);
            dm_1.dataSource.json = visualData;
            pageingDetails.result = dm_1.executeLocal(query);
        }
        this.parent.notify('updateAction', pageingDetails);
    };
    return Page$$1;
}());

/**
 * Toolbar Module for TreeGrid
 *
 * @hidden
 */
var Toolbar$1 = /** @class */ (function () {
    function Toolbar$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.Toolbar);
        this.parent = parent;
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} - Returns Toolbar module name
     */
    Toolbar$$1.prototype.getModuleName = function () {
        return 'toolbar';
    };
    /**
     * @hidden
     * @returns {void}
     */
    Toolbar$$1.prototype.addEventListener = function () {
        this.parent.on(rowSelected, this.refreshToolbar, this);
        this.parent.on(toolbarClick, this.toolbarClickHandler, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    Toolbar$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off(rowSelected, this.refreshToolbar);
        this.parent.off(toolbarClick, this.toolbarClickHandler);
    };
    Toolbar$$1.prototype.refreshToolbar = function (args) {
        var tObj = this.parent;
        if (args.row.rowIndex === 0 || tObj.getSelectedRecords().length > 1) {
            this.enableItems([tObj.element.id + '_gridcontrol_indent', tObj.element.id + '_gridcontrol_outdent'], false);
        }
        else {
            if (!sf.base.isNullOrUndefined(tObj.getCurrentViewRecords()[args.row.rowIndex])) {
                if (!sf.base.isNullOrUndefined(tObj.getCurrentViewRecords()[args.row.rowIndex]) &&
                    (tObj.getCurrentViewRecords()[args.row.rowIndex].level >
                        tObj.getCurrentViewRecords()[args.row.rowIndex - 1].level)) {
                    this.enableItems([tObj.element.id + '_gridcontrol_indent'], false);
                }
                else {
                    this.enableItems([tObj.element.id + '_gridcontrol_indent'], true);
                }
                if (tObj.getCurrentViewRecords()[args.row.rowIndex].level ===
                    tObj.getCurrentViewRecords()[args.row.rowIndex - 1].level) {
                    this.enableItems([tObj.element.id + '_gridcontrol_indent'], true);
                }
                if (tObj.getCurrentViewRecords()[args.row.rowIndex].level === 0) {
                    this.enableItems([tObj.element.id + '_gridcontrol_outdent'], false);
                }
                if (tObj.getCurrentViewRecords()[args.row.rowIndex].level !== 0) {
                    this.enableItems([tObj.element.id + '_gridcontrol_outdent'], true);
                }
            }
        }
        if (args.row.rowIndex === 0 && !sf.base.isNullOrUndefined(args.data.parentItem)) {
            this.enableItems([tObj.element.id + '_gridcontrol_outdent'], true);
        }
    };
    Toolbar$$1.prototype.toolbarClickHandler = function (args) {
        var tObj = this.parent;
        if (this.parent.editSettings.mode === 'Cell' && this.parent.grid.editSettings.mode === 'Batch' &&
            args.item.id === this.parent.grid.element.id + '_update') {
            args.cancel = true;
            this.parent.grid.editModule.saveCell();
        }
        if (args.item.id === this.parent.grid.element.id + '_expandall') {
            this.parent.expandAll();
        }
        if (args.item.id === this.parent.grid.element.id + '_collapseall') {
            this.parent.collapseAll();
        }
        if (args.item.id === tObj.grid.element.id + '_indent' && tObj.getSelectedRecords().length) {
            var record = tObj.getCurrentViewRecords()[tObj.getSelectedRowIndexes()[0] - 1];
            var dropIndex = void 0;
            if (record.level > tObj.getSelectedRecords()[0].level) {
                for (var i = 0; i < tObj.getCurrentViewRecords().length; i++) {
                    if (tObj.getCurrentViewRecords()[i].taskData === record.parentItem.taskData) {
                        dropIndex = i;
                    }
                }
            }
            else {
                dropIndex = tObj.getSelectedRowIndexes()[0] - 1;
            }
            tObj.reorderRows([tObj.getSelectedRowIndexes()[0]], dropIndex, 'child');
        }
        if (args.item.id === tObj.grid.element.id + '_outdent' && tObj.getSelectedRecords().length) {
            var index = tObj.getSelectedRowIndexes()[0];
            var dropIndex = void 0;
            var parentItem = tObj.getSelectedRecords()[0].parentItem;
            for (var i = 0; i < tObj.getCurrentViewRecords().length; i++) {
                if (tObj.getCurrentViewRecords()[i].taskData === parentItem.taskData) {
                    dropIndex = i;
                }
            }
            tObj.reorderRows([index], dropIndex, 'below');
        }
    };
    /**
     * Gets the toolbar of the TreeGrid.
     *
     * @returns {Element} - Returns Toolbar element
     * @hidden
     */
    Toolbar$$1.prototype.getToolbar = function () {
        return this.parent.grid.toolbarModule.getToolbar();
    };
    /**
     * Enables or disables ToolBar items.
     *
     * @param {string[]} items - Defines the collection of itemID of ToolBar items.
     * @param {boolean} isEnable - Defines the items to be enabled or disabled.
     * @returns {void}
     * @hidden
     */
    Toolbar$$1.prototype.enableItems = function (items, isEnable) {
        this.parent.grid.toolbarModule.enableItems(items, isEnable);
    };
    /**
     * Destroys the ToolBar.
     *
     * @method destroy
     * @returns {void}
     */
    Toolbar$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    return Toolbar$$1;
}());

/**
 * TreeGrid Aggregate module
 *
 * @hidden
 */
var Aggregate$1 = /** @class */ (function () {
    /**
     * Constructor for Aggregate module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Aggregate$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.Aggregate);
        this.parent = parent;
        this.flatChildRecords = [];
        this.summaryQuery = [];
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Summary module name
     */
    Aggregate$$1.prototype.getModuleName = function () {
        return 'summary';
    };
    Aggregate$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
    };
    /**
     * Function to calculate summary values
     *
     * @param {QueryOptions[]} summaryQuery - DataManager query for aggregate operations
     * @param {Object[]} filteredData - Filtered data collection
     * @param {boolean} isSort - Specified whether sorting operation performed
     * @hidden
     * @returns {Object[]} -  return flat records with summary values
     */
    Aggregate$$1.prototype.calculateSummaryValue = function (summaryQuery, filteredData, isSort) {
        this.summaryQuery = summaryQuery;
        var parentRecord;
        var parentDataLength = Object.keys(filteredData).length;
        var parentData = [];
        for (var p = 0, len = parentDataLength; p < len; p++) {
            var summaryRow = sf.grids.getObject('isSummaryRow', filteredData[p]);
            if (!summaryRow) {
                parentData.push(filteredData[p]);
            }
        }
        var parentRecords = findParentRecords(parentData);
        var flatRecords = parentData.slice();
        var summaryLength = Object.keys(this.parent.aggregates).length;
        var dataLength = Object.keys(parentRecords).length;
        var childRecordsLength;
        var columns = this.parent.getColumns();
        if (this.parent.aggregates.filter(function (x) { return x.showChildSummary; }).length) {
            for (var i = 0, len = dataLength; i < len; i++) {
                parentRecord = parentRecords[i];
                childRecordsLength = this.getChildRecordsLength(parentRecord, flatRecords);
                if (childRecordsLength) {
                    var _loop_1 = function (summaryRowIndex, len_1) {
                        var item = void 0;
                        item = {};
                        for (var i_1 = 0; i_1 < columns.length; i_1++) {
                            var field = (sf.base.isNullOrUndefined(sf.grids.getObject('field', columns[i_1]))) ?
                                columns[i_1] : sf.grids.getObject('field', (columns[i_1]));
                            item[field] = null;
                        }
                        item = this_1.createSummaryItem(item, this_1.parent.aggregates[summaryRowIndex - 1]);
                        if (this_1.parent.aggregates[summaryRowIndex - 1].showChildSummary) {
                            var idx_1;
                            flatRecords.map(function (e, i) {
                                if (e.uniqueID === parentRecord.uniqueID) {
                                    idx_1 = i;
                                    return;
                                }
                            });
                            var currentIndex = idx_1 + childRecordsLength + summaryRowIndex;
                            var summaryParent = sf.base.extend({}, parentRecord);
                            delete summaryParent.childRecords;
                            delete summaryParent[this_1.parent.childMapping];
                            sf.base.setValue('parentItem', summaryParent, item);
                            var level = sf.grids.getObject('level', summaryParent);
                            sf.base.setValue('level', level + 1, item);
                            sf.base.setValue('isSummaryRow', true, item);
                            sf.base.setValue('parentUniqueID', summaryParent.uniqueID, item);
                            if (isSort) {
                                var childRecords = sf.grids.getObject('childRecords', parentRecord);
                                if (childRecords.length) {
                                    childRecords.push(item);
                                }
                            }
                            flatRecords.splice(currentIndex, 0, item);
                        }
                        else {
                            return "continue";
                        }
                    };
                    var this_1 = this;
                    for (var summaryRowIndex = 1, len_1 = summaryLength; summaryRowIndex <= len_1; summaryRowIndex++) {
                        _loop_1(summaryRowIndex, len_1);
                    }
                    this.flatChildRecords = [];
                }
            }
        }
        else {
            var items = {};
            for (var columnIndex = 0, length_1 = columns.length; columnIndex < length_1; columnIndex++) {
                var fields = sf.base.isNullOrUndefined(sf.grids.getObject('field', columns[columnIndex])) ?
                    columns[columnIndex] : sf.grids.getObject('field', columns[columnIndex]);
                items[fields] = null;
            }
            for (var summaryRowIndex = 1, length_2 = summaryLength; summaryRowIndex <= length_2; summaryRowIndex++) {
                this.createSummaryItem(items, this.parent.aggregates[summaryRowIndex - 1]);
            }
        }
        return flatRecords;
    };
    Aggregate$$1.prototype.getChildRecordsLength = function (parentData, flatData) {
        var recordLength = Object.keys(flatData).length;
        var record;
        for (var i = 0, len = recordLength; i < len; i++) {
            record = flatData[i];
            var parent_1 = sf.base.isNullOrUndefined(record.parentItem) ? null :
                flatData.filter(function (e) { return e.uniqueID === record.parentItem.uniqueID; })[0];
            if (parentData === parent_1) {
                this.flatChildRecords.push(record);
                var hasChild = sf.grids.getObject('hasChildRecords', record);
                if (hasChild) {
                    this.getChildRecordsLength(record, flatData);
                }
                else {
                    continue;
                }
            }
        }
        return this.flatChildRecords.length;
    };
    Aggregate$$1.prototype.createSummaryItem = function (itemData, summary) {
        var summaryColumnLength = Object.keys(summary.columns).length;
        for (var i = 0, len = summaryColumnLength; i < len; i++) {
            var displayColumn = sf.base.isNullOrUndefined(summary.columns[i].columnName) ? summary.columns[i].field :
                summary.columns[i].columnName;
            var keys = Object.keys(itemData);
            for (var _i = 0, keys_1 = keys; _i < keys_1.length; _i++) {
                var key = keys_1[_i];
                if (key === displayColumn) {
                    if (this.flatChildRecords.length) {
                        itemData[key] = this.getSummaryValues(summary.columns[i], this.flatChildRecords);
                    }
                    else if (this.parent.isLocalData) {
                        var data = this.parent.dataSource instanceof sf.data.DataManager ? this.parent.dataSource.dataSource.json
                            : this.parent.flatData;
                        itemData[key] = this.getSummaryValues(summary.columns[i], data);
                    }
                }
                else {
                    continue;
                }
            }
        }
        return itemData;
    };
    Aggregate$$1.prototype.getSummaryValues = function (summaryColumn, summaryData) {
        var qry = new sf.data.Query();
        var single = {};
        var helper = {};
        var type = !sf.base.isNullOrUndefined(summaryColumn.field) ?
            this.parent.getColumnByField(summaryColumn.field).type : undefined;
        summaryColumn.setPropertiesSilent({ format: this.getFormatFromType(summaryColumn.format, type) });
        summaryColumn.setFormatter(this.parent.grid.locale);
        var formatFn = summaryColumn.getFormatter() || (function () { return function (a) { return a; }; })();
        summaryColumn.setTemplate(helper);
        var tempObj = summaryColumn.getTemplate(2);
        qry.queries = this.summaryQuery;
        qry.requiresCount();
        var sumData = new sf.data.DataManager(summaryData).executeLocal(qry);
        var types = summaryColumn.type;
        var summaryKey;
        types = [summaryColumn.type];
        for (var i = 0; i < types.length; i++) {
            summaryKey = types[i];
            var key = summaryColumn.field + ' - ' + types[i].toLowerCase();
            var val = types[i] !== 'Custom' ? sf.grids.getObject('aggregates', sumData) :
                sf.grids.calculateAggregate(types[i], sumData, summaryColumn, this.parent);
            var disp = summaryColumn.columnName;
            var value_1 = types[i] !== 'Custom' ? val[key] : val;
            single[disp] = single[disp] || {};
            single[disp][key] = value_1;
            single[disp][types[i]] = !sf.base.isNullOrUndefined(val) ? formatFn(value_1) : ' ';
        }
        helper.format = summaryColumn.getFormatter();
        var cellElement = sf.base.createElement('td', {
            className: 'e-summary'
        });
        if (this.parent.isReact) {
            var renderReactTemplates = 'renderReactTemplates';
            tempObj.fn(single[summaryColumn.columnName], this.parent, tempObj.property, '', null, null, cellElement);
            this.parent[renderReactTemplates]();
        }
        else {
            sf.grids.appendChildren(cellElement, tempObj.fn(single[summaryColumn.columnName], this.parent, tempObj.property));
        }
        var value = single[summaryColumn.columnName][summaryKey];
        var summaryValue;
        if (cellElement.innerHTML.indexOf(value) === -1) {
            summaryValue = cellElement.innerHTML + value;
            return summaryValue;
        }
        else {
            return cellElement.innerHTML;
        }
    };
    Aggregate$$1.prototype.getFormatFromType = function (summaryformat, type) {
        if (sf.base.isNullOrUndefined(type) || typeof summaryformat !== 'string') {
            return summaryformat;
        }
        var obj;
        switch (type) {
            case 'number':
                obj = { format: summaryformat };
                break;
            case 'datetime':
                obj = { type: 'dateTime', skeleton: summaryformat };
                break;
            case 'date':
                obj = { type: type, skeleton: summaryformat };
                break;
        }
        return obj;
    };
    /**
     * To destroy the Aggregate module
     *
     * @returns {void}
     * @hidden
     */
    Aggregate$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    return Aggregate$$1;
}());

/**
 * Internal dataoperations for TreeGrid
 *
 * @hidden
 */
var Sort$1 = /** @class */ (function () {
    function Sort$$1(grid) {
        sf.grids.Grid.Inject(sf.grids.Sort);
        this.parent = grid;
        this.taskIds = [];
        this.flatSortedData = [];
        this.storedIndex = -1;
        this.isSelfReference = !sf.base.isNullOrUndefined(this.parent.parentIdMapping);
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Sort module name
     */
    Sort$$1.prototype.getModuleName = function () {
        return 'sort';
    };
    /**
     * @hidden
     */
    Sort$$1.prototype.addEventListener = function () {
        this.parent.on('updateModel', this.updateModel, this);
        this.parent.on('createSort', this.createdSortedRecords, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    Sort$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('updateModel', this.updateModel);
        this.parent.off('createSort', this.createdSortedRecords);
    };
    Sort$$1.prototype.createdSortedRecords = function (sortParams) {
        var data = sortParams.modifiedData;
        var srtQry = sortParams.srtQry;
        this.iterateSort(data, srtQry);
        this.storedIndex = -1;
        sortParams.modifiedData = this.flatSortedData;
        this.flatSortedData = [];
    };
    Sort$$1.prototype.iterateSort = function (data, srtQry) {
        for (var d = 0; d < data.length; d++) {
            if (this.parent.grid.filterSettings.columns.length > 0 || this.parent.grid.searchSettings.key !== '') {
                if (!sf.base.isNullOrUndefined(getParentData(this.parent, data[d].uniqueID, true))) {
                    this.storedIndex++;
                    this.flatSortedData[this.storedIndex] = data[d];
                }
            }
            else {
                this.storedIndex++;
                this.flatSortedData[this.storedIndex] = data[d];
            }
            if (data[d].hasChildRecords) {
                var childSort = (new sf.data.DataManager(data[d].childRecords).executeLocal(srtQry));
                this.iterateSort(childSort, srtQry);
            }
        }
    };
    /**
     * Sorts a column with the given options.
     *
     * @param {string} columnName - Defines the column name to be sorted.
     * @param {SortDirection} direction - Defines the direction of sorting field.
     * @param {boolean} isMultiSort - Specifies whether the previous sorted columns are to be maintained.
     * @returns {void}
     */
    Sort$$1.prototype.sortColumn = function (columnName, direction, isMultiSort) {
        this.parent.grid.sortColumn(columnName, direction, isMultiSort);
    };
    Sort$$1.prototype.removeSortColumn = function (field) {
        this.parent.grid.removeSortColumn(field);
    };
    /**
     * The function used to update sortSettings of TreeGrid.
     *
     * @returns {void}
     * @hidden
     */
    Sort$$1.prototype.updateModel = function () {
        this.parent.setProperties({ sortSettings: sf.grids.getActualProperties(this.parent.grid.sortSettings) }, true);
    };
    /**
     * Clears all the sorted columns of the TreeGrid.
     *
     * @returns {void}
     */
    Sort$$1.prototype.clearSorting = function () {
        this.parent.grid.clearSorting();
        this.updateModel();
    };
    /**
     * Destroys the Sorting of TreeGrid.
     *
     * @function destroy
     * @returns {void}
     */
    Sort$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    return Sort$$1;
}());

/**
 * TreeGrid ColumnMenu module
 *
 * @hidden
 */
var ColumnMenu$1 = /** @class */ (function () {
    /**
     * Constructor for render module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function ColumnMenu$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.ColumnMenu);
        this.parent = parent;
    }
    ColumnMenu$$1.prototype.getColumnMenu = function () {
        return this.parent.grid.columnMenuModule.getColumnMenu();
    };
    ColumnMenu$$1.prototype.destroy = function () {
        //this.parent.grid.columnMenuModule.destroy();
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns ColumnMenu module name
     */
    ColumnMenu$$1.prototype.getModuleName = function () {
        return 'columnMenu';
    };
    return ColumnMenu$$1;
}());

/**
 * ContextMenu Module for TreeGrid
 *
 * @hidden
 */
var ContextMenu$1 = /** @class */ (function () {
    function ContextMenu$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.ContextMenu);
        this.parent = parent;
        this.addEventListener();
    }
    /**
     * @hidden
     * @returns {void}
     */
    ContextMenu$$1.prototype.addEventListener = function () {
        this.parent.on('contextMenuOpen', this.contextMenuOpen, this);
        this.parent.on('contextMenuClick', this.contextMenuClick, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    ContextMenu$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('contextMenuOpen', this.contextMenuOpen);
        this.parent.off('contextMenuClick', this.contextMenuClick);
    };
    ContextMenu$$1.prototype.contextMenuOpen = function (args) {
        var addRow = sf.base.select('#' + this.parent.element.id + '_gridcontrol_cmenu_AddRow', args.element);
        var editRecord = sf.base.select('#' + this.parent.element.id + '_gridcontrol_cmenu_Edit', args.element);
        if (addRow) {
            if (this.parent.grid.editSettings.allowAdding === false) {
                addRow.style.display = 'none';
            }
            else {
                addRow.style.display = 'block';
            }
        }
        if ((this.parent.editSettings.mode === 'Cell' || this.parent.editSettings.mode === 'Batch')
            && !(sf.base.isNullOrUndefined(editRecord)) && !(editRecord.classList.contains('e-menu-hide'))) {
            editRecord.style.display = 'none';
        }
    };
    ContextMenu$$1.prototype.contextMenuClick = function (args) {
        if (args.item.id === 'Above' || args.item.id === 'Below' || args.item.id === 'Child') {
            this.parent.notify('savePreviousRowPosition', args);
            this.parent.setProperties({ editSettings: { newRowPosition: args.item.id } }, true);
            this.parent.addRecord();
        }
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns ContextMenu module name
     */
    ContextMenu$$1.prototype.getModuleName = function () {
        return 'contextMenu';
    };
    /**
     * Destroys the ContextMenu.
     *
     * @function destroy
     * @returns {void}
     */
    ContextMenu$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * Gets the context menu element from the TreeGrid.
     *
     * @returns {Element} Return Context Menu root element.
     */
    ContextMenu$$1.prototype.getContextMenu = function () {
        return this.parent.grid.contextMenuModule.getContextMenu();
    };
    return ContextMenu$$1;
}());

/**
 * `BatchEdit` module is used to handle batch editing actions.
 *
 * @hidden
 */
var BatchEdit = /** @class */ (function () {
    function BatchEdit(parent) {
        this.batchChildCount = 0;
        this.addedRecords = 'addedRecords';
        this.deletedRecords = 'deletedRecords';
        this.batchAddedRecords = [];
        this.batchDeletedRecords = [];
        this.batchAddRowRecord = [];
        this.parent = parent;
        this.isSelfReference = !sf.base.isNullOrUndefined(parent.parentIdMapping);
        this.batchRecords = [];
        this.currentViewRecords = [];
        this.isAdd = false;
        this.addEventListener();
    }
    BatchEdit.prototype.addEventListener = function () {
        this.parent.on(cellSaved, this.cellSaved, this);
        this.parent.on(batchAdd, this.batchAdd, this);
        this.parent.on(beforeBatchAdd, this.beforeBatchAdd, this);
        this.parent.on(batchSave, this.batchSave, this);
        this.parent.on(beforeBatchDelete, this.beforeBatchDelete, this);
        this.parent.on(beforeBatchSave, this.beforeBatchSave, this);
        this.parent.on('batchPageAction', this.batchPageAction, this);
        this.parent.on('batchCancelAction', this.batchCancelAction, this);
        this.parent.grid.on('immutable-batch-cancel', this.immutableBatchAction, this);
        this.parent.grid.on('next-cell-index', this.nextCellIndex, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    BatchEdit.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off(cellSaved, this.cellSaved);
        this.parent.off(batchAdd, this.batchAdd);
        this.parent.off(batchSave, this.batchSave);
        this.parent.off(beforeBatchAdd, this.beforeBatchAdd);
        this.parent.off(beforeBatchDelete, this.beforeBatchDelete);
        this.parent.off(beforeBatchSave, this.beforeBatchSave);
        this.parent.off('batchPageAction', this.batchPageAction);
        this.parent.off('batchCancelAction', this.batchCancelAction);
        this.parent.grid.off('immutable-batch-cancel', this.immutableBatchAction);
        this.parent.grid.off('next-cell-index', this.nextCellIndex);
    };
    /**
     * To destroy the editModule
     *
     * @returns {void}
     * @hidden
     */
    BatchEdit.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * @hidden
     * @returns {Object[]} Returns modified records in batch editing.
     */
    BatchEdit.prototype.getBatchRecords = function () {
        return this.batchRecords;
    };
    /**
     * @hidden
     * @returns {number} Returns index of newly add row
     */
    BatchEdit.prototype.getAddRowIndex = function () {
        return this.addRowIndex;
    };
    /**
     * @hidden
     * @returns {number} Returns selected row index
     */
    BatchEdit.prototype.getSelectedIndex = function () {
        return this.selectedIndex;
    };
    /**
     * @hidden
     * @returns {number} Returns newly added child count
     */
    BatchEdit.prototype.getBatchChildCount = function () {
        return this.batchChildCount;
    };
    BatchEdit.prototype.batchPageAction = function () {
        var data = (this.parent.grid.dataSource instanceof sf.data.DataManager ?
            this.parent.grid.dataSource.dataSource.json : this.parent.grid.dataSource);
        var primaryKey = this.parent.grid.getPrimaryKeyFieldNames()[0];
        var index;
        if (!sf.base.isNullOrUndefined(this.batchAddedRecords) && this.batchAddedRecords.length) {
            for (var i = 0; i < this.batchAddedRecords.length; i++) {
                index = data.map(function (e) { return e[primaryKey]; }).indexOf(this.batchAddedRecords[i][primaryKey]);
                data.splice(index, 1);
            }
        }
        this.batchAddedRecords = this.batchRecords = this.batchAddRowRecord = this.batchDeletedRecords = this.currentViewRecords = [];
    };
    BatchEdit.prototype.cellSaved = function (args) {
        var actualCellIndex = args.cell.cellIndex;
        var frozenCols = this.parent.frozenColumns || this.parent.getFrozenColumns();
        if (frozenCols && args.columnObject.index > frozenCols) {
            actualCellIndex = actualCellIndex + frozenCols;
        }
        var freeze = (this.parent.getFrozenLeftColumnsCount() > 0 || this.parent.getFrozenRightColumnsCount() > 0) ? true : false;
        if (freeze) {
            var colCount = this.parent.getFrozenLeftColumnsCount() + actualCellIndex;
            if (colCount == this.parent.treeColumnIndex) {
                this.parent.renderModule.cellRender({ data: args.rowData, cell: args.cell,
                    column: this.parent.grid.getColumnByIndex(args.cell.cellIndex)
                });
            }
        }
        else if (actualCellIndex === this.parent.treeColumnIndex) {
            this.parent.renderModule.cellRender({ data: args.rowData, cell: args.cell,
                column: this.parent.grid.getColumnByIndex(args.cell.cellIndex)
            });
        }
        if (this.isAdd && this.parent.editSettings.mode === 'Batch' && this.parent.editSettings.newRowPosition !== 'Bottom') {
            var data = (this.parent.grid.dataSource instanceof sf.data.DataManager ?
                this.parent.grid.dataSource.dataSource.json : this.parent.grid.dataSource);
            var added = void 0;
            var level = 'level';
            var primaryKey_1 = this.parent.grid.getPrimaryKeyFieldNames()[0];
            var currentDataIndex = void 0;
            var indexvalue = void 0;
            var parentItem = 'parentItem';
            var uniqueID = 'uniqueID';
            var parentRecord = this.selectedIndex > -1 ? this.batchRecords[this.addRowIndex][parentItem] : null;
            var idMapping = void 0;
            var parentUniqueID = void 0;
            var parentIdMapping = void 0;
            var rowObjectIndex = this.parent.editSettings.newRowPosition === 'Top' || this.selectedIndex === -1 ? 0 :
                this.parent.editSettings.newRowPosition === 'Above' ? this.addRowIndex
                    : this.addRowIndex + 1;
            rowObjectIndex = this.getActualRowObjectIndex(rowObjectIndex);
            if (this.newBatchRowAdded) {
                if (this.batchRecords.length) {
                    idMapping = this.batchRecords[this.addRowIndex][this.parent.idMapping];
                    parentIdMapping = this.batchRecords[this.addRowIndex][this.parent.parentIdMapping];
                    if (this.batchRecords[this.addRowIndex][parentItem]) {
                        parentUniqueID = this.batchRecords[this.addRowIndex][parentItem][uniqueID];
                    }
                }
                this.batchAddedRecords = extendArray(this.batchAddedRecords);
                this.batchAddRowRecord = extendArray(this.batchAddRowRecord);
                this.batchAddRowRecord.push(this.batchRecords[this.addRowIndex]);
                added = this.parent.grid.getRowsObject()[rowObjectIndex].changes;
                added.uniqueID = sf.grids.getUid(this.parent.element.id + '_data_');
                sf.base.setValue('uniqueIDCollection.' + added.uniqueID, added, this.parent);
                if (!Object.prototype.hasOwnProperty.call(added, 'level')) {
                    this.batchIndex = this.selectedIndex === -1 ? 0 : this.batchIndex;
                    if (this.parent.editSettings.newRowPosition === 'Child') {
                        added.primaryParent = parentRecord;
                        if (this.selectedIndex > -1) {
                            added.parentItem = sf.base.extend({}, this.batchRecords[this.addRowIndex]);
                            added.parentUniqueID = added.parentItem.uniqueID;
                            delete added.parentItem.childRecords;
                            delete added.parentItem[this.parent.childMapping];
                            added.level = added.parentItem.level + 1;
                            added.index = this.batchIndex;
                            var childRecordCount = findChildrenRecords(this.batchRecords[this.addRowIndex]).length;
                            var record = findChildrenRecords(this.batchRecords[this.addRowIndex])[childRecordCount - 1];
                            record = sf.base.isNullOrUndefined(record) ? this.batchRecords[this.addRowIndex] : record;
                            currentDataIndex = data.map(function (e) { return e[primaryKey_1]; }).indexOf(record[primaryKey_1]);
                            if (this.isSelfReference) {
                                added[this.parent.parentIdMapping] = idMapping;
                            }
                            updateParentRow(primaryKey_1, added.parentItem, 'add', this.parent, this.isSelfReference, added);
                        }
                    }
                    else if ((this.parent.editSettings.newRowPosition === 'Above' || this.parent.editSettings.newRowPosition === 'Below')
                        && !sf.base.isNullOrUndefined(this.batchRecords[this.addRowIndex])) {
                        added.level = this.batchRecords[this.addRowIndex][level];
                        if (added.level && this.selectedIndex > -1) {
                            added.parentItem = parentRecord;
                            added.parentUniqueID = parentUniqueID;
                            delete added.parentItem.childRecords;
                            delete added.parentItem[this.parent.childMapping];
                        }
                        added.index = this.parent.editSettings.newRowPosition === 'Below' ? this.batchIndex : this.batchIndex - 1;
                        if (this.parent.editSettings.newRowPosition === 'Below' && this.selectedIndex > -1) {
                            var childRecordCount = findChildrenRecords(this.batchRecords[this.addRowIndex]).length;
                            var record = findChildrenRecords(this.batchRecords[this.addRowIndex])[childRecordCount - 1];
                            record = sf.base.isNullOrUndefined(record) ? this.batchRecords[this.addRowIndex] : record;
                            currentDataIndex = data.map(function (e) { return e[primaryKey_1]; }).indexOf(record[primaryKey_1]);
                        }
                        if (this.parent.editSettings.newRowPosition === 'Above' && this.selectedIndex > -1) {
                            var record = this.batchRecords[this.addRowIndex];
                            currentDataIndex = data.map(function (e) { return e[primaryKey_1]; }).indexOf(record[primaryKey_1]);
                        }
                        if (this.isSelfReference) {
                            added[this.parent.parentIdMapping] = parentIdMapping;
                        }
                    }
                    added.index = added.index === -1 ? 0 : added.index;
                    added.hasChildRecords = false;
                    added.childRecords = [];
                    this.batchRecords.splice(added.index, 0, added);
                    this.currentViewRecords.splice(added.index, 0, added);
                    if (currentDataIndex) {
                        indexvalue = currentDataIndex;
                    }
                    else {
                        indexvalue = added.index;
                    }
                    if (this.parent.editSettings.newRowPosition !== 'Above') {
                        indexvalue = added.index === 0 ? indexvalue : indexvalue + 1;
                    }
                    data.splice(indexvalue, 0, added);
                    this.batchAddedRecords.push(added);
                }
                this.parent.grid.getRowsObject()[rowObjectIndex].data = added;
                this.newBatchRowAdded = false;
            }
            if (this.parent.frozenColumns || this.parent.getFrozenColumns()
                && this.parent.grid.getRowsObject()[rowObjectIndex].edit === 'add') {
                sf.base.merge(this.currentViewRecords[rowObjectIndex], this.parent.grid.getRowsObject()[rowObjectIndex].changes);
            }
        }
    };
    BatchEdit.prototype.beforeBatchAdd = function (e) {
        var isTabLastRow = 'isTabLastRow';
        if (this.parent.editSettings.mode === 'Cell' && this.parent.editModule[isTabLastRow]) {
            e.cancel = true;
            this.parent.editModule[isTabLastRow] = false;
            return;
        }
        this.selectedIndex = this.parent.grid.selectedRowIndex;
        this.addRowIndex = this.parent.grid.selectedRowIndex > -1 ? this.parent.grid.selectedRowIndex : 0;
        this.addRowRecord = this.parent.getSelectedRecords()[0];
    };
    BatchEdit.prototype.batchAdd = function (e) {
        if (this.parent.editSettings.newRowPosition !== 'Bottom') {
            this.isAdd = true;
            this.newBatchRowAdded = true;
            var actualIndex = 0;
            if (!this.batchRecords.length) {
                this.batchAddedRecords = [];
                this.batchRecords = extendArray(this.parent.grid.getCurrentViewRecords());
                this.currentViewRecords = extendArray(this.parent.grid.getCurrentViewRecords());
            }
            if (this.parent.editSettings.newRowPosition !== 'Top') {
                var records = this.parent.grid.getCurrentViewRecords();
                if (this.parent.editSettings.mode === 'Batch' && (this.parent.getBatchChanges()[this.addedRecords].length > 1
                    || this.parent.getBatchChanges()[this.deletedRecords].length)) {
                    records = this.batchRecords;
                }
                this.updateChildCount(records);
                this.parent.notify(beginAdd, {});
                this.batchChildCount = 0;
            }
            this.updateRowIndex();
            // update focus module, need to refix this once grid source modified.
            var focusModule = sf.base.getValue('focusModule', this.parent.grid);
            var table = this.parent.getContentTable();
            if (this.parent.getBatchChanges()[this.deletedRecords].length && this.parent.editSettings.newRowPosition === 'Above') {
                actualIndex = e.row.rowIndex;
                focusModule.getContent().matrix.matrix = this.matrix;
            }
            else {
                actualIndex = table.getElementsByClassName('e-batchrow')[0].rowIndex;
                // if (this.parent.frozenRows || this.parent.frozenColumns) {
                //   actualIndex = this.batchIndex;
                // }
            }
            focusModule.getContent().matrix.current = [actualIndex, focusModule.getContent().matrix.current[1]];
        }
    };
    BatchEdit.prototype.beforeBatchDelete = function () {
        if (!this.batchRecords.length) {
            this.batchRecords = extendArray(this.parent.grid.getCurrentViewRecords());
            this.currentViewRecords = extendArray(this.parent.grid.getCurrentViewRecords());
        }
        var focusModule = sf.base.getValue('focusModule', this.parent.grid);
        this.matrix = focusModule.getContent().matrix.matrix;
        var row = [];
        var records = [];
        var primarykey = this.parent.grid.getPrimaryKeyFieldNames()[0];
        var data = this.parent.grid.getSelectedRecords()[this.parent.grid.getSelectedRecords().length - 1];
        var childs = findChildrenRecords(data);
        var uid = this.parent.getSelectedRows()[0].getAttribute('data-uid');
        var parentRowIndex = parseInt(this.parent.grid.getRowElementByUID(uid).getAttribute('aria-rowindex'), 10);
        if (childs.length) {
            var totalCount = parentRowIndex + childs.length;
            var firstChildIndex = parentRowIndex + 1;
            for (var i = firstChildIndex; i <= totalCount; i++) {
                row.push(this.parent.grid.getDataRows()[i]);
                if (this.parent.frozenRows || this.parent.frozenColumns || this.parent.getFrozenColumns()) {
                    row.push(this.parent.grid.getMovableRows()[i]);
                }
            }
        }
        if (!sf.base.isNullOrUndefined(data.parentItem)) {
            var parentItem = getParentData(this.parent, data.parentItem.uniqueID);
            if (!sf.base.isNullOrUndefined(parentItem) && parentItem.hasChildRecords) {
                var childIndex = parentItem.childRecords.indexOf(data);
                parentItem.childRecords.splice(childIndex, 1);
            }
            this.batchDeletedRecords = extendArray(this.batchDeletedRecords);
            this.batchDeletedRecords.push(data);
        }
        childs.push(data);
        records = childs;
        for (var i = 0; i < records.length; i++) {
            var indexvalue = this.batchRecords.map(function (e) { return e[primarykey]; }).indexOf(records[i][primarykey]);
            if (indexvalue !== -1) {
                this.batchRecords.splice(indexvalue, 1);
            }
        }
        for (var i = 0; i < row.length; i++) {
            if (!sf.base.isNullOrUndefined(row[i])) {
                this.parent.grid.selectionModule.selectedRecords.push(row[i]);
            }
        }
    };
    BatchEdit.prototype.updateRowIndex = function () {
        var rows = this.parent.grid.getDataRows();
        for (var i = 0; i < rows.length; i++) {
            rows[i].setAttribute('aria-rowindex', i.toString());
        }
        var freeze = (this.parent.getFrozenLeftColumnsCount() > 0 || this.parent.getFrozenRightColumnsCount() > 0) ? true : false;
        if (this.parent.frozenRows || this.parent.getFrozenColumns() || this.parent.frozenColumns || freeze) {
            var mRows = this.parent.grid.getMovableDataRows();
            var freezeRightRows = this.parent.grid.getFrozenRightDataRows();
            for (var i = 0; i < mRows.length; i++) {
                mRows[i].setAttribute('aria-rowindex', i.toString());
                if (freeze) {
                    freezeRightRows[i].setAttribute('aria-rowindex', i.toString());
                }
            }
        }
    };
    BatchEdit.prototype.updateChildCount = function (records) {
        var primaryKey = this.parent.grid.getPrimaryKeyFieldNames()[0];
        var addedRecords = 'addedRecords';
        var parentItem = this.parent.editSettings.newRowPosition === 'Child' ? 'primaryParent' : 'parentItem';
        for (var i = 0; i < this.parent.getBatchChanges()[addedRecords].length; i++) {
            if (!sf.base.isNullOrUndefined(this.parent.getBatchChanges()[addedRecords][i][parentItem])) {
                if (this.parent.getBatchChanges()[addedRecords][i][parentItem][primaryKey] === records[this.addRowIndex][primaryKey]) {
                    this.batchChildCount = this.batchChildCount + 1;
                }
            }
        }
    };
    BatchEdit.prototype.beforeBatchSave = function (e) {
        var changeRecords = 'changedRecords';
        var deleterecords = 'deletedRecords';
        var changedRecords = e.batchChanges[changeRecords];
        if (e.batchChanges[changeRecords].length) {
            var columnName = void 0;
            for (var i = 0; i < changedRecords.length; i++) {
                editAction({ value: changedRecords[i], action: 'edit' }, this.parent, this.isSelfReference, this.addRowIndex, this.selectedIndex, columnName);
            }
        }
        if (e.batchChanges[deleterecords].length) {
            var deletedRecords = e.batchChanges[deleterecords];
            var record = deletedRecords;
            for (var i = 0; i < record.length; i++) {
                this.deleteUniqueID(record[i].uniqueID);
                var childs = findChildrenRecords(record[i]);
                for (var c = 0; c < childs.length; c++) {
                    this.deleteUniqueID(childs[c].uniqueID);
                }
                e.batchChanges[deleterecords] = e.batchChanges[deleterecords].concat(childs);
            }
        }
        this.isAdd = false;
    };
    BatchEdit.prototype.deleteUniqueID = function (value) {
        var idFilter = 'uniqueIDFilterCollection';
        delete this.parent[idFilter][value];
        var id = 'uniqueIDCollection';
        delete this.parent[id][value];
    };
    BatchEdit.prototype.batchCancelAction = function () {
        var targetElement = 'targetElement';
        var index;
        var parentItem = 'parentItem';
        var indexvalue = 'index';
        var currentViewRecords = this.parent.grid.getCurrentViewRecords();
        var childRecords = 'childRecords';
        var data = (this.parent.grid.dataSource instanceof sf.data.DataManager ?
            this.parent.grid.dataSource.dataSource.json : this.parent.grid.dataSource);
        var primaryKey = this.parent.grid.getPrimaryKeyFieldNames()[0];
        if (!sf.base.isNullOrUndefined(this.parent[targetElement])) {
            var row = this.parent[targetElement].closest('tr');
            this.parent.collapseRow(row);
            this.parent[targetElement] = null;
        }
        if (!sf.base.isNullOrUndefined(this.batchAddedRecords)) {
            for (var i = 0; i < this.batchAddedRecords.length; i++) {
                index = data.map(function (e) { return e[primaryKey]; }).indexOf(this.batchAddedRecords[i][primaryKey]);
                data.splice(index, 1);
                if (this.parent.editSettings.newRowPosition === 'Child') {
                    index = currentViewRecords.map(function (e) { return e[primaryKey]; })
                        .indexOf(this.batchAddedRecords[i][parentItem] ? this.batchAddedRecords[i][parentItem][primaryKey]
                        : this.batchAddedRecords[i][primaryKey]);
                    if (!sf.base.isNullOrUndefined(currentViewRecords[index])) {
                        var children = currentViewRecords[index][childRecords];
                        for (var j = 0; children && j < children.length; j++) {
                            if (children[j][primaryKey] === this.batchAddedRecords[i][primaryKey]) {
                                currentViewRecords[index][childRecords].splice(j, 1);
                            }
                        }
                    }
                }
            }
        }
        if (!sf.base.isNullOrUndefined(this.batchDeletedRecords)) {
            for (var i = 0; i < this.batchDeletedRecords.length; i++) {
                if (!sf.base.isNullOrUndefined(this.batchDeletedRecords[i][parentItem])) {
                    index = currentViewRecords.map(function (e) { return e[primaryKey]; })
                        .indexOf(this.batchDeletedRecords[i][parentItem][primaryKey]);
                    var positionIndex = this.batchDeletedRecords[i][indexvalue] === 0 ? this.batchDeletedRecords[i][indexvalue] :
                        this.batchDeletedRecords[i][indexvalue] - 1;
                    if (!sf.base.isNullOrUndefined(currentViewRecords[index])) {
                        currentViewRecords[index][childRecords].splice(positionIndex, 0, this.batchDeletedRecords[i]);
                    }
                }
            }
        }
        this.batchAddedRecords = this.batchRecords = this.batchAddRowRecord = this.currentViewRecords = [];
        this.batchRecords = extendArray(this.parent.grid.getCurrentViewRecords());
        this.batchIndex = 0;
        this.currentViewRecords = extendArray(this.parent.grid.getCurrentViewRecords());
        this.batchDeletedRecords = [];
        this.parent.grid.renderModule.refresh();
    };
    BatchEdit.prototype.batchSave = function (args) {
        if (this.parent.editSettings.mode === 'Batch') {
            var i = void 0;
            var batchChanges = Object.hasOwnProperty.call(args, 'updatedRecords') ? args.updatedRecords : this.parent.getBatchChanges();
            var deletedRecords = 'deletedRecords';
            var addedRecords = 'addedRecords';
            var index = 'index';
            var uniqueID = 'uniqueID';
            var data = (this.parent.grid.dataSource instanceof sf.data.DataManager ?
                this.parent.grid.dataSource.dataSource.json : this.parent.grid.dataSource);
            var currentViewRecords = this.parent.grid.getCurrentViewRecords();
            var primarykey_1 = this.parent.grid.getPrimaryKeyFieldNames()[0];
            var level = 'level';
            var addRecords = batchChanges[addedRecords];
            var parentItem = 'parentItem';
            var selectedIndex = void 0;
            var addRowIndex = void 0;
            var columnName = void 0;
            var addRowRecord = void 0;
            var childRecords = 'childRecords';
            if (addRecords.length > 1 && this.parent.editSettings.newRowPosition !== 'Bottom') {
                addRecords.reverse();
            }
            if (this.parent.editSettings.newRowPosition !== 'Bottom' && !Object.hasOwnProperty.call(args, 'updatedRecords')) {
                data.splice(data.length - addRecords.length, addRecords.length);
                if (!this.parent.allowPaging && data.length !== currentViewRecords.length) {
                    if (currentViewRecords.length > addRecords.length) {
                        currentViewRecords.splice(currentViewRecords.length - addRecords.length, addRecords.length);
                    }
                }
                else {
                    var totalRecords = extendArray(data);
                    if (totalRecords.length) {
                        var startIndex = totalRecords.map(function (e) { return e[primarykey_1]; })
                            .indexOf(currentViewRecords[0][primarykey_1]);
                        var endIndex = startIndex + this.parent.grid.pageSettings.pageSize;
                        currentViewRecords = totalRecords.splice(startIndex, endIndex);
                    }
                }
            }
            if (this.batchAddRowRecord.length === 0) {
                this.batchAddRowRecord.push(this.parent.flatData[args.index]);
            }
            for (i = 0; i < addRecords.length; i++) {
                var taskData = sf.base.extend({}, addRecords[i]);
                delete taskData.parentItem;
                delete taskData.uniqueID;
                delete taskData.index;
                delete taskData.level;
                delete taskData.hasChildRecords;
                delete taskData.childRecords;
                delete taskData.parentUniqueID;
                if (!sf.base.isNullOrUndefined(taskData.primaryParent)) {
                    delete taskData.primaryParent;
                }
                addRecords[i].taskData = taskData;
                addRowRecord = this.batchAddRowRecord[i];
                if (sf.base.isNullOrUndefined(addRowRecord)) {
                    addRowRecord = this.batchAddRowRecord[i - 1];
                }
                if (this.isSelfReference) {
                    if (!sf.base.isNullOrUndefined(addRecords[i].parentItem)) {
                        updateParentRow(primarykey_1, addRecords[i].parentItem, 'add', this.parent, this.isSelfReference, addRecords[i]);
                    }
                }
                if (!sf.base.isNullOrUndefined(addRowRecord)) {
                    addRowIndex = addRowRecord.index;
                }
                if (this.parent.editSettings.newRowPosition !== 'Top' && this.parent.editSettings.newRowPosition !== 'Bottom') {
                    if (sf.base.isNullOrUndefined(addRecords[i].parentItem) && this.selectedIndex === -1) {
                        selectedIndex = -1;
                        addRowRecord = null;
                    }
                }
                editAction({ value: addRecords[i], action: 'add' }, this.parent, this.isSelfReference, addRowIndex, selectedIndex, columnName, addRowRecord);
                selectedIndex = null;
                if (this.parent.editSettings.newRowPosition === 'Child' && !sf.base.isNullOrUndefined(addRecords[i][parentItem])) {
                    var indexValue = currentViewRecords.map(function (e) { return e[primarykey_1]; })
                        .indexOf(addRecords[i][parentItem][primarykey_1]);
                    var children = currentViewRecords[indexValue][childRecords];
                    for (var j = 0; j < children.length; j++) {
                        if (children[j][primarykey_1] === addRecords[i][primarykey_1]) {
                            currentViewRecords[indexValue][childRecords].splice(j, 1);
                        }
                    }
                }
            }
            if (batchChanges[deletedRecords].length) {
                for (i = 0; i < batchChanges[deletedRecords].length; i++) {
                    editAction({ value: batchChanges[deletedRecords][i], action: 'delete' }, this.parent, this.isSelfReference, addRowIndex, selectedIndex, columnName, addRowRecord);
                }
            }
            this.parent.parentData = [];
            for (var i_1 = 0; i_1 < data.length; i_1++) {
                data[i_1][index] = i_1;
                sf.base.setValue('uniqueIDCollection.' + data[i_1][uniqueID] + '.index', i_1, this.parent);
                if (!data[i_1][level]) {
                    this.parent.parentData.push(data[i_1]);
                }
            }
        }
        this.batchAddRowRecord = this.batchAddedRecords = this.batchRecords = this.batchDeletedRecords = this.currentViewRecords = [];
    };
    BatchEdit.prototype.getActualRowObjectIndex = function (index) {
        var rows = this.parent.grid.getDataRows();
        if ((this.parent.editSettings.newRowPosition === 'Below' || this.parent.editSettings.newRowPosition === 'Child')
            && this.selectedIndex > -1) {
            if (!sf.base.isNullOrUndefined(this.batchRecords[this.addRowIndex]) && this.batchRecords[this.addRowIndex].expanded) {
                if (this.parent.getBatchChanges()[this.addedRecords].length > 1
                    || this.parent.getBatchChanges()[this.deletedRecords].length) {
                    index += findChildrenRecords(this.batchRecords[this.addRowIndex]).length;
                    if (this.parent.editSettings.newRowPosition !== 'Child') {
                        var batchChildCount = this.getBatchChildCount();
                        index = index + batchChildCount;
                    }
                }
                else {
                    index += findChildrenRecords(this.batchRecords[this.addRowIndex]).length;
                }
            }
            if (index >= rows.length) {
                index = rows.length - 1;
            }
            this.updateChildCount(this.parent.grid.getCurrentViewRecords());
            if (this.batchChildCount) {
                index += this.batchChildCount;
            }
            this.batchChildCount = 0;
        }
        return index;
    };
    BatchEdit.prototype.immutableBatchAction = function (e) {
        e.args.cancel = true;
        var changes = this.parent.grid.getBatchChanges();
        var addedRecords = [];
        var index = 'index';
        if (Object.keys(changes).length) {
            addedRecords = changes.addedRecords;
        }
        for (var i = 0; i < addedRecords.length; i++) {
            e.rows.splice(addedRecords[i][index], 1);
        }
    };
    BatchEdit.prototype.nextCellIndex = function (args) {
        var index = 'index';
        var rowIndex = 'rowIndex';
        args[index] = this.parent.getSelectedRows()[0][rowIndex];
    };
    return BatchEdit;
}());

/**
 * TreeGrid Edit Module
 * The `Edit` module is used to handle editing actions.
 */
var Edit$1 = /** @class */ (function () {
    /**
     * Constructor for Edit module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Edit$$1(parent) {
        this.addedRecords = 'addedRecords';
        this.deletedRecords = 'deletedRecords';
        this.prevAriaRowIndex = '-1';
        this.isAddedRowByMethod = false;
        sf.grids.Grid.Inject(sf.grids.Edit);
        this.parent = parent;
        this.isSelfReference = !sf.base.isNullOrUndefined(parent.parentIdMapping);
        this.previousNewRowPosition = null;
        this.internalProperties = {};
        this.batchEditModule = new BatchEdit(this.parent);
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Edit module name
     */
    Edit$$1.prototype.getModuleName = function () {
        return 'edit';
    };
    /**
     * @hidden
     * @returns {void}
     */
    Edit$$1.prototype.addEventListener = function () {
        this.parent.on(crudAction, this.crudAction, this);
        this.parent.on(beginEdit, this.beginEdit, this);
        this.parent.on(beginAdd, this.beginAdd, this);
        this.parent.on(recordDoubleClick, this.recordDoubleClick, this);
        this.parent.on(cellSave, this.cellSave, this);
        this.parent.on(batchCancel, this.batchCancel, this);
        this.parent.grid.on(keyPressed, this.keyPressed, this);
        this.parent.grid.on('batchedit-form', this.lastCellTab, this);
        this.parent.grid.on('content-ready', this.contentready, this);
        this.parent.on(cellEdit, this.cellEdit, this);
        this.parent.on('actionBegin', this.editActionEvents, this);
        this.parent.on('actionComplete', this.editActionEvents, this);
        this.parent.grid.on(doubleTap, this.recordDoubleClick, this);
        this.parent.grid.on('dblclick', this.gridDblClick, this);
        this.parent.grid.on('recordAdded', this.customCellSave, this);
        this.parent.on('savePreviousRowPosition', this.savePreviousRowPosition, this);
        // this.parent.on(events.beforeDataBound, this.beforeDataBound, this);
        this.parent.grid.on(beforeStartEdit, this.beforeStartEdit, this);
        this.parent.grid.on(beforeBatchCancel, this.beforeBatchCancel, this);
        this.parent.grid.on('reset-edit-props', this.resetIsOnBatch, this);
        this.parent.grid.on('get-row-position', this.getRowPosition, this);
    };
    Edit$$1.prototype.gridDblClick = function (e) {
        this.doubleClickTarget = e.target;
    };
    Edit$$1.prototype.getRowPosition = function (addArgs) {
        addArgs.newRowPosition = this.parent.editSettings.newRowPosition;
        addArgs.addRowIndex = this.addRowIndex;
        addArgs.ariaRowIndex = +this.prevAriaRowIndex;
    };
    Edit$$1.prototype.beforeStartEdit = function (args) {
        this.parent.trigger(actionBegin, args);
    };
    Edit$$1.prototype.beforeBatchCancel = function (args) {
        if (this.parent.editSettings.mode === 'Cell') {
            this.parent.trigger(actionComplete, args);
        }
    };
    /**
     * @hidden
     * @returns {void}
     */
    Edit$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off(crudAction, this.crudAction);
        this.parent.off(beginEdit, this.beginEdit);
        this.parent.off(beginAdd, this.beginAdd);
        this.parent.off(recordDoubleClick, this.recordDoubleClick);
        this.parent.off(batchCancel, this.batchCancel);
        this.parent.grid.off(keyPressed, this.keyPressed);
        this.parent.grid.off('batchedit-form', this.lastCellTab);
        this.parent.grid.off('content-ready', this.contentready);
        this.parent.off(cellEdit, this.cellEdit);
        this.parent.off('actionBegin', this.editActionEvents);
        this.parent.off('actionComplete', this.editActionEvents);
        this.parent.grid.off('recordAdded', this.customCellSave);
        this.parent.grid.off(doubleTap, this.recordDoubleClick);
        this.parent.off('savePreviousRowPosition', this.savePreviousRowPosition);
        this.parent.grid.off(beforeStartEdit, this.beforeStartEdit);
        this.parent.grid.off(beforeBatchCancel, this.beforeBatchCancel);
        this.parent.grid.off('dblclick', this.gridDblClick);
        this.parent.grid.off('reset-edit-props', this.resetIsOnBatch);
        this.parent.grid.off('get-row-position', this.getRowPosition);
        //this.parent.grid.off('click', this.gridSingleClick);
    };
    /**
     * To destroy the editModule
     *
     * @returns {void}
     * @hidden
     */
    Edit$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * @param {Column[]} cols - Column property Collection
     * @hidden
     * @returns {void}
     */
    Edit$$1.prototype.applyFormValidation = function (cols) {
        this.parent.grid.editModule.applyFormValidation(cols);
    };
    Edit$$1.prototype.editActionEvents = function (args) {
        var eventArgs = sf.grids.getObject('editAction', args);
        var eventName = sf.grids.getObject('name', eventArgs);
        var treeObj = this.parent;
        var adaptor = treeObj.dataSource.adaptor;
        if ((isRemoteData(treeObj) || adaptor instanceof sf.data.RemoteSaveAdaptor) &&
            (eventArgs.requestType === 'save' && eventArgs.action === 'add') &&
            (treeObj.editSettings.newRowPosition === 'Child' || treeObj.editSettings.newRowPosition === 'Below'
                || treeObj.editSettings.newRowPosition === 'Above')) {
            if (eventName === 'actionBegin') {
                var rowIndex = sf.base.isNullOrUndefined(eventArgs.row) || !Object.keys(eventArgs.row).length ? this.selectedIndex :
                    eventArgs.row.rowIndex - 1;
                var keyData = (!sf.base.isNullOrUndefined(rowIndex) && rowIndex !== -1) ?
                    treeObj.getCurrentViewRecords()[rowIndex][treeObj.getPrimaryKeyFieldNames()[0]] : -1;
                treeObj.grid.query.addParams('relationalKey', keyData);
            }
            else if (eventName === 'actionComplete') {
                var paramsLength = treeObj.grid.query.params.length;
                for (var i = 0; i < paramsLength; i++) {
                    if (treeObj.grid.query.params[i].key === 'relationalKey') {
                        treeObj.grid.query.params.splice(i);
                    }
                }
            }
        }
        if (this.parent.enableInfiniteScrolling && eventName === 'actionComplete') {
            this.infiniteAddAction(eventArgs);
        }
        if (this.parent.editSettings.mode === 'Batch' && eventArgs.requestType === 'paging') {
            this.parent.notify('batchPageAction', {});
        }
    };
    Edit$$1.prototype.infiniteAddAction = function (args) {
        if ((args.requestType === 'save' && args.action === 'add') || args.requestType === 'delete') {
            if (this.parent.editSettings.newRowPosition !== 'Top' && this.selectedIndex !== -1
                && (args.requestType === 'save' && args.action === 'add')) {
                var rowObjects = this.parent.grid.getRowsObject();
                var newRowObject = rowObjects.splice(0, 1)[0];
                var newRowObjectIndex = this.addRowIndex;
                var currentData = this.parent.getCurrentViewRecords();
                if (this.parent.editSettings.newRowPosition === 'Below' || this.parent.editSettings.newRowPosition === 'Child') {
                    newRowObjectIndex += findChildrenRecords(currentData[newRowObjectIndex + 1]).length;
                }
                newRowObjectIndex = this.parent.editSettings.newRowPosition === 'Below' ? newRowObjectIndex + 1 : newRowObjectIndex;
                rowObjects.splice(newRowObjectIndex, 0, newRowObject);
                var newRecord = currentData.splice(0, 1)[0];
                currentData.splice(newRowObjectIndex, 0, newRecord);
                this.updateInfiniteCurrentViewData(newRecord, this.addRowIndex);
            }
            var movableRows = this.parent.grid.getMovableRows();
            var movableRowsObject = this.parent.grid.getMovableRowsObject();
            var isCache = this.parent.infiniteScrollSettings.enableCache;
            if (!isCache) {
                sf.grids.resetRowIndex(this.parent.grid, this.parent.grid.getRowsObject(), this.parent.grid.getRows(), 0);
                this.updateIndex(this.parent.grid.dataSource, this.parent.getRows(), this.parent.getCurrentViewRecords());
            }
            if (!isCache && this.parent.getFrozenColumns() > 0) {
                sf.grids.resetRowIndex(this.parent.grid, movableRowsObject, movableRows, 0);
                this.updateIndex(this.parent.grid.dataSource, movableRows, this.parent.getCurrentViewRecords());
            }
        }
    };
    Edit$$1.prototype.updateInfiniteCurrentViewData = function (newRecord, newRowIndex) {
        var _this = this;
        var infiniteData = 'infiniteCurrentViewData';
        var updateCurrentViewData = 'updateCurrentViewData';
        var size = Math.ceil(newRowIndex / this.parent.grid.pageSettings.pageSize);
        var page = size > 0 ? size : 1;
        var dataIndex = newRowIndex - ((page - 1) * this.parent.pageSettings.pageSize);
        var infiniteCurrentViewData = this.parent.grid.infiniteScrollModule[infiniteData];
        infiniteCurrentViewData[1].splice(0, 1);
        var data = infiniteCurrentViewData[page];
        if (!sf.base.isNullOrUndefined(this.addRowRecord)) {
            data.filter(function (e, index) {
                if (e.uniqueID === _this.addRowRecord.uniqueID) {
                    dataIndex = index;
                }
            });
            if (this.addRowRecord.hasChildRecords && this.addRowRecord.childRecords.length &&
                this.parent.editSettings.newRowPosition === 'Below' || this.parent.editSettings.newRowPosition === 'Child') {
                dataIndex += findChildrenRecords(this.addRowRecord).length;
            }
        }
        if (dataIndex >= this.parent.pageSettings.pageSize) {
            page += 1;
            data = infiniteCurrentViewData[page];
            dataIndex = dataIndex - this.parent.pageSettings.pageSize >= 0 ? dataIndex - this.parent.pageSettings.pageSize : 0;
        }
        dataIndex = this.parent.editSettings.newRowPosition === 'Below' ? dataIndex + 1 : dataIndex;
        data.splice(dataIndex, 0, newRecord);
        this.parent.grid.infiniteScrollModule[updateCurrentViewData]();
    };
    Edit$$1.prototype.recordDoubleClick = function (args) {
        var target = args.target;
        if (sf.base.isNullOrUndefined(target.closest('td.e-rowcell'))) {
            return;
        }
        if (!(this.parent.grid.editSettings.allowEditing) || this.parent.grid.isEdit) {
            return;
        }
        var column = this.parent.grid.getColumnByIndex(+target.closest('td.e-rowcell').getAttribute('aria-colindex'));
        if (this.parent.editSettings.mode === 'Cell' && !this.isOnBatch && column && !column.isPrimaryKey &&
            this.parent.editSettings.allowEditing && column.allowEditing && !(target.classList.contains('e-treegridexpand') ||
            target.classList.contains('e-treegridcollapse')) && this.parent.editSettings.allowEditOnDblClick) {
            this.isOnBatch = true;
            this.parent.grid.setProperties({ selectedRowIndex: args.rowIndex }, true);
            if (this.parent.enableVirtualization) {
                var tr = sf.grids.parentsUntil(args.target, 'e-row');
                this.prevAriaRowIndex = tr.getAttribute('aria-rowindex');
                tr.setAttribute('aria-rowindex', tr.rowIndex + '');
            }
            this.updateGridEditMode('Batch');
        }
    };
    Edit$$1.prototype.updateGridEditMode = function (mode) {
        this.parent.grid.setProperties({ editSettings: { mode: mode } }, true);
        var updateMethod = sf.grids.getObject('updateEditObj', this.parent.grid.editModule);
        updateMethod.apply(this.parent.grid.editModule);
        this.parent.grid.isEdit = false;
    };
    Edit$$1.prototype.resetIsOnBatch = function () {
        if (this.parent.enableVirtualization && this.parent.editSettings.mode === 'Cell') {
            this.isOnBatch = false;
            this.updateGridEditMode('Normal');
        }
    };
    Edit$$1.prototype.keyPressed = function (args) {
        if (this.isOnBatch) {
            this.keyPress = args.action;
        }
        if (args.action === 'f2') {
            this.recordDoubleClick(args);
        }
        if (args.action == 'escape') {
            this.parent.closeEdit();
        }
    };
    Edit$$1.prototype.deleteUniqueID = function (value) {
        var idFilter = 'uniqueIDFilterCollection';
        delete this.parent[idFilter][value];
        var id = 'uniqueIDCollection';
        delete this.parent[id][value];
    };
    Edit$$1.prototype.cellEdit = function (args) {
        var _this = this;
        var promise = 'promise';
        var prom = args[promise];
        delete args[promise];
        if (this.parent.enableVirtualization && !sf.base.isNullOrUndefined(this.prevAriaRowIndex)) {
            args.row.setAttribute('aria-rowindex', this.prevAriaRowIndex);
            this.prevAriaRowIndex = undefined;
        }
        if (this.keyPress !== 'enter') {
            this.parent.trigger(cellEdit, args, function (celleditArgs) {
                if (!celleditArgs.cancel && _this.parent.editSettings.mode === 'Cell') {
                    _this.enableToolbarItems('edit');
                }
                else if (celleditArgs.cancel && _this.parent.editSettings.mode === 'Cell') {
                    _this.isOnBatch = false;
                    _this.updateGridEditMode('Normal');
                }
                if (!sf.base.isNullOrUndefined(prom)) {
                    prom.resolve(celleditArgs);
                }
            });
        }
        if (this.doubleClickTarget && (this.doubleClickTarget.classList.contains('e-treegridexpand') ||
            this.doubleClickTarget.classList.contains('e-treegridcollapse') || this.doubleClickTarget.classList.contains('e-summarycell'))) {
            args.cancel = true;
            this.doubleClickTarget = null;
            return;
        }
        if (this.parent.editSettings.mode === 'Cell') {
            if (this.keyPress === 'tab' || this.keyPress === 'shiftTab') {
                this.keyPress = null;
            }
            else if (this.keyPress === 'enter') {
                args.cancel = true;
                this.keyPress = null;
                sf.base.setValue('isEditCollapse', false, this.parent);
            }
            if (!args.columnObject.allowEditing) {
                args.cancel = true;
            }
        }
        // if (this.isAdd && this.parent.editSettings.mode === 'Batch' && !args.cell.parentElement.classList.contains('e-insertedrow')) {
        //   this.isAdd = false;
        // }
    };
    Edit$$1.prototype.enableToolbarItems = function (request) {
        if (!sf.base.isNullOrUndefined(this.parent.grid.toolbarModule)) {
            var toolbarID = this.parent.element.id + '_gridcontrol_';
            this.parent.grid.toolbarModule.enableItems([toolbarID + 'add', toolbarID + 'edit', toolbarID + 'delete'], request === 'save');
            this.parent.grid.toolbarModule.enableItems([toolbarID + 'update', toolbarID + 'cancel'], request === 'edit');
        }
    };
    Edit$$1.prototype.batchCancel = function () {
        if (this.parent.editSettings.mode === 'Cell') {
            var cellDetails = sf.base.getValue('editModule.cellDetails', this.parent.grid.editModule);
            var treeCell = this.parent.getCellFromIndex(cellDetails.rowIndex, this.parent.treeColumnIndex);
            this.parent.renderModule.cellRender({
                data: cellDetails.rowData,
                cell: treeCell,
                column: this.parent.grid.getColumns()[this.parent.treeColumnIndex]
            });
            this.updateGridEditMode('Normal');
            this.isOnBatch = false;
        }
        if (this.parent.editSettings.mode === 'Batch') {
            this.parent.notify('batchCancelAction', {});
        }
    };
    Edit$$1.prototype.customCellSave = function (args) {
        if (isCountRequired(this.parent) && this.parent.editSettings.mode === 'Cell' && args.action === 'edit') {
            this.updateCell(args, args.rowIndex);
            this.afterCellSave(args, args.row, args.rowIndex);
        }
    };
    Edit$$1.prototype.cellSave = function (args) {
        var _this = this;
        if (this.parent.editSettings.mode === 'Cell' && this.parent.element.querySelector('form')) {
            args.cancel = true;
            var editModule = 'editModule';
            sf.base.setValue('isEdit', false, this.parent.grid);
            sf.base.setValue('isEditCollapse', true, this.parent);
            args.rowData[args.columnName] = args.value;
            var row_1;
            if (sf.base.isNullOrUndefined(args.cell)) {
                row_1 = this.parent.grid.editModule[editModule].form.parentElement.parentNode;
            }
            else {
                row_1 = args.cell.parentNode;
            }
            var rowIndex_1;
            var primaryKeys_1 = this.parent.getPrimaryKeyFieldNames();
            if (sf.base.isNullOrUndefined(row_1)) {
                this.parent.grid.getCurrentViewRecords().filter(function (e, i) {
                    if (e[primaryKeys_1[0]] === args.rowData[primaryKeys_1[0]]) {
                        rowIndex_1 = i;
                        return;
                    }
                });
            }
            else {
                var freeze = (this.parent.getFrozenLeftColumnsCount() > 0 || this.parent.getFrozenRightColumnsCount() > 0) ? true : false;
                if (freeze) {
                    if (this.parent.getRows().indexOf(row_1) != -1) {
                        rowIndex_1 = this.parent.getRows().indexOf(row_1);
                    }
                    else if (this.parent.getFrozenRightRows().indexOf(row_1) != -1) {
                        rowIndex_1 = this.parent.getFrozenRightRows().indexOf(row_1);
                    }
                    else {
                        rowIndex_1 = this.parent.getMovableRows().indexOf(row_1);
                    }
                }
                else {
                    rowIndex_1 = (this.parent.getRows().indexOf(row_1) === -1 && (this.parent.getFrozenColumns() > 0)) ?
                        this.parent.grid.getMovableRows().indexOf(row_1) : this.parent.getRows().indexOf(row_1);
                }
            }
            var arg = {};
            sf.base.extend(arg, args);
            arg.cancel = false;
            arg.type = 'save';
            row_1 = this.parent.grid.getRows()[row_1.rowIndex];
            this.parent.trigger(actionBegin, arg);
            if (!arg.cancel) {
                if ((row_1.rowIndex === this.parent.getCurrentViewRecords().length - 1) && this.keyPress === 'tab') {
                    this.isTabLastRow = true;
                }
                if (!isRemoteData(this.parent) &&
                    !(this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.adaptor instanceof sf.data.RemoteSaveAdaptor)) {
                    if (isCountRequired(this.parent)) {
                        var eventPromise = 'eventPromise';
                        var editArgs = { requestType: 'save', data: args.rowData, action: 'edit', row: row_1,
                            rowIndex: rowIndex_1, rowData: args.rowData, columnName: args.columnName,
                            filterChoiceCount: null, excelSearchOperator: null };
                        this.parent.grid.getDataModule()[eventPromise](editArgs, this.parent.grid.query);
                    }
                    else {
                        this.updateCell(args, rowIndex_1);
                        this.afterCellSave(args, row_1, rowIndex_1);
                    }
                }
                else if (isRemoteData(this.parent) ||
                    (this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.adaptor instanceof sf.data.RemoteSaveAdaptor)) {
                    var query = this.parent.grid.query;
                    var crud = null;
                    crud = this.parent.grid.dataSource.update(primaryKeys_1[0], args.rowData, query.fromTable, query, args.previousValue);
                    crud.then(function (e) {
                        if (!sf.base.isNullOrUndefined(e)) {
                            args.rowData[args.columnName] = e[args.columnName];
                        }
                        _this.updateCell(args, rowIndex_1);
                        _this.afterCellSave(args, row_1, rowIndex_1);
                    });
                }
            }
            else {
                this.parent.grid.isEdit = true;
            }
        }
    };
    Edit$$1.prototype.afterCellSave = function (args, row, rowIndex) {
        var mRow;
        if (this.parent.grid.aggregateModule) {
            this.parent.grid.aggregateModule.refresh(args.rowData);
        }
        this.parent.grid.editModule.destroyWidgets([this.parent.grid.getColumnByField(args.columnName)]);
        this.parent.grid.editModule.formObj.destroy();
        if (this.keyPress !== 'tab' && this.keyPress !== 'shiftTab') {
            this.updateGridEditMode('Normal');
            this.isOnBatch = false;
        }
        this.enableToolbarItems('save');
        var freeze = (this.parent.getFrozenLeftColumnsCount() > 0 || this.parent.getFrozenRightColumnsCount() > 0) ? true : false;
        if (freeze) {
            if (args.cell.closest('.e-frozen-left-header') || args.cell.closest('.e-frozen-left-content')) {
                mRow = this.parent.grid.getRows()[rowIndex];
            }
            else if (args.cell.closest('.e-movableheader') || args.cell.closest('.e-movablecontent')) {
                mRow = this.parent.grid.getMovableRows()[rowIndex];
            }
            else {
                mRow = this.parent.grid.getFrozenRightRows()[rowIndex];
            }
            sf.base.removeClass([mRow], ['e-editedrow', 'e-batchrow']);
            sf.base.removeClass(mRow.querySelectorAll('.e-rowcell'), ['e-editedbatchcell', 'e-updatedtd']);
        }
        else if (this.parent.getFrozenColumns() > 0) {
            if (args.cell.closest('.e-frozenheader') || args.cell.closest('.e-frozencontent')) {
                mRow = this.parent.grid.getRows()[rowIndex];
            }
            else {
                mRow = this.parent.grid.getMovableRows()[rowIndex];
            }
            sf.base.removeClass([mRow], ['e-editedrow', 'e-batchrow']);
            sf.base.removeClass(mRow.querySelectorAll('.e-rowcell'), ['e-editedbatchcell', 'e-updatedtd']);
        }
        sf.base.removeClass([row], ['e-editedrow', 'e-batchrow']);
        sf.base.removeClass(row.querySelectorAll('.e-rowcell'), ['e-editedbatchcell', 'e-updatedtd']);
        if (this.parent['isCellSaveFocus'] !== false) {
            this.parent.grid.focusModule.restoreFocus();
        }
        editAction({ value: args.rowData, action: 'edit' }, this.parent, this.isSelfReference, this.addRowIndex, this.selectedIndex, args.columnName);
        if ((row.rowIndex === this.parent.getCurrentViewRecords().length - 1) && this.keyPress === 'enter') {
            this.keyPress = null;
        }
        var saveArgs = {
            type: 'save', column: this.parent.getColumnByField(args.columnName), data: args.rowData,
            previousData: args.previousValue, row: row, target: args.cell
        };
        if (this.parent.aggregates.map(function (ag) { return ag.showChildSummary == true; }).length) {
            this.parent.grid.refresh();
        }
        this.parent.trigger(actionComplete, saveArgs);
    };
    Edit$$1.prototype.lastCellTab = function () {
        if (!this.parent.grid.isEdit && this.isOnBatch && this.keyPress === 'tab' && this.parent.editSettings.mode === 'Cell') {
            if (!this.parent.editSettings.allowNextRowEdit) {
                this.updateGridEditMode('Normal');
                this.isOnBatch = false;
                this.keyPress = null;
            }
            else {
                this.enableToolbarItems('edit');
            }
        }
    };
    Edit$$1.prototype.updateCell = function (args, rowIndex) {
        this.parent.grid.editModule.updateCell(rowIndex, args.columnName, args.rowData[args.columnName]);
        this.parent.grid.getRowsObject()[rowIndex].data = args.rowData;
    };
    Edit$$1.prototype.crudAction = function (details, columnName) {
        editAction(details, this.parent, this.isSelfReference, this.addRowIndex, this.selectedIndex, columnName, this.addRowRecord);
        this.parent.parentData = [];
        var data = this.parent.grid.dataSource instanceof sf.data.DataManager ?
            this.parent.grid.dataSource.dataSource.json : this.parent.grid.dataSource;
        for (var i = 0; i < data.length; i++) {
            data[i].index = i;
            var key = this.parent.grid.getPrimaryKeyFieldNames()[0];
            if (details.value[key] === data[i][key]) {
                if (details.action === 'add') {
                    data[i].level = this.internalProperties.level;
                    data[i].taskData = this.internalProperties.taskData;
                    data[i].uniqueID = this.internalProperties.uniqueID;
                    if (!sf.base.isNullOrUndefined(this.internalProperties.parentItem)) {
                        data[i].parentItem = this.internalProperties.parentItem;
                        data[i].parentUniqueID = this.internalProperties.parentUniqueID;
                    }
                    data[i].childRecords = this.internalProperties.childRecords;
                }
            }
            sf.base.setValue('uniqueIDCollection.' + data[i].uniqueID + '.index', i, this.parent);
            if (!data[i].level) {
                this.parent.parentData.push(data[i]);
            }
        }
        if (details.action === 'add' && this.previousNewRowPosition != null) {
            this.parent.setProperties({ editSettings: { newRowPosition: this.previousNewRowPosition } }, true);
            this.previousNewRowPosition = null;
        }
    };
    Edit$$1.prototype.updateIndex = function (data, rows, records) {
        for (var j = 0; j < this.parent.getDataRows().length; j++) {
            var data1 = records[j];
            var index = sf.base.getValue('uniqueIDCollection.' + data1.uniqueID + '.index', this.parent);
            data1.index = index;
            if (!sf.base.isNullOrUndefined(data1.parentItem)) {
                var parentIndex = sf.base.getValue('uniqueIDCollection.' + data1.parentItem.uniqueID + '.index', this.parent);
                data1.parentItem.index = parentIndex;
            }
        }
        var count = -1;
        var treeColIndex = this.parent.treeColumnIndex;
        if (this.parent.getFrozenColumns() > 0) {
            var cells = rows[0].querySelectorAll('.e-rowcell');
            for (var l = 0; l < cells.length; l++) {
                if (cells[l].classList.contains('e-gridrowindex0level0')) {
                    treeColIndex = l;
                    break;
                }
            }
        }
        for (var k = 0; k < this.parent.getRows().length; k++) {
            if (!rows[k].classList.contains('e-detailrow')) {
                count++;
            }
            var data2 = records[count];
            var index = data2.index;
            var level = data2.level;
            var row = rows[k];
            if (!sf.base.isNullOrUndefined(data2.parentItem)) {
                index = sf.base.getValue('uniqueIDCollection.' + data2.parentItem.uniqueID + '.index', this.parent);
            }
            var treecell = row.cells[treeColIndex];
            if (!sf.base.isNullOrUndefined(treecell)) {
                for (var l = 0; l < treecell.classList.length; l++) {
                    var value = treecell.classList[l];
                    var remove$$1 = /e-gridrowindex/i;
                    var removed = /e-griddetailrowindex/i;
                    var result = value.match(remove$$1);
                    var results = value.match(removed);
                    if (result != null) {
                        sf.base.removeClass([treecell], value);
                    }
                    if (results != null) {
                        sf.base.removeClass([treecell], value);
                    }
                }
                if (!rows[k].classList.contains('e-detailrow')) {
                    sf.base.addClass([treecell], 'e-gridrowindex' + index + 'level' + level);
                }
                else {
                    sf.base.addClass([treecell], 'e-griddetailrowindex' + index + 'level' + level);
                }
            }
        }
    };
    Edit$$1.prototype.beginAdd = function () {
        var position;
        var index = this.addRowIndex;
        var records = this.parent.grid.getCurrentViewRecords();
        if (this.parent.editSettings.mode === 'Batch') {
            index = this.batchEditModule.getAddRowIndex();
            this.selectedIndex = this.batchEditModule.getSelectedIndex();
            if (this.parent.getBatchChanges()[this.addedRecords].length > 1
                || this.parent.getBatchChanges()[this.deletedRecords].length) {
                records = this.batchEditModule.getBatchRecords();
            }
        }
        var rows = this.parent.grid.getDataRows();
        var firstAriaIndex = rows.length ? +rows[0].getAttribute('aria-rowindex') : 0;
        var lastAriaIndex = rows.length ? +rows[rows.length - 1].getAttribute('aria-rowindex') : 0;
        var withinRange = this.selectedIndex >= firstAriaIndex && this.selectedIndex <= lastAriaIndex;
        var isVirtualization = this.parent.enableVirtualization && this.addRowIndex > -1 && this.prevAriaRowIndex !== '-1';
        if (this.parent.editSettings.mode !== 'Dialog') {
            if (this.parent.editSettings.newRowPosition === 'Above') {
                position = 'before';
            }
            else if ((this.parent.editSettings.newRowPosition === 'Below' || this.parent.editSettings.newRowPosition === 'Child')
                && (this.selectedIndex > -1 || isVirtualization) && withinRange) {
                position = 'after';
                if (!sf.base.isNullOrUndefined(records[index]) && records[index].expanded) {
                    if (this.parent.editSettings.mode === 'Batch' && (this.parent.getBatchChanges()[this.addedRecords].length > 1
                        || this.parent.getBatchChanges()[this.deletedRecords].length)) {
                        index += findChildrenRecords(records[index]).length;
                        if (this.parent.editSettings.newRowPosition !== 'Child') {
                            var batchChildCount = this.batchEditModule.getBatchChildCount();
                            index = index + batchChildCount;
                        }
                    }
                    else {
                        index += findChildrenRecords(records[index]).length;
                    }
                }
            }
            if ((this.selectedIndex > -1 || isVirtualization) && withinRange
                && (index || (this.parent.editSettings.newRowPosition === 'Child'
                    || this.parent.editSettings.newRowPosition === 'Below'))) {
                if (index >= rows.length - 1) {
                    index = rows.length - 2;
                }
                var r = 'rows';
                var newRowObject = this.parent.grid.contentModule[r][0];
                var focussedElement = document.activeElement;
                rows[index + 1][position](rows[0]);
                sf.base.setValue('batchIndex', index + 1, this.batchEditModule);
                var rowObjectIndex = this.parent.editSettings.newRowPosition === 'Above' ? index : index + 1;
                if (this.parent.editSettings.mode === 'Batch') {
                    this.parent.grid.contentModule[r].splice(0, 1);
                    this.parent.grid.contentModule[r].splice(rowObjectIndex, 0, newRowObject);
                }
                var freeze = (this.parent.getFrozenLeftColumnsCount() > 0 || this.parent.getFrozenRightColumnsCount() > 0) ? true : false;
                if (this.parent.frozenRows || this.parent.getFrozenColumns() || this.parent.frozenColumns || freeze) {
                    var movableRows = this.parent.getMovableDataRows();
                    var frows = 'freezeRows';
                    var newFreezeRowObject = this.parent.grid.getRowsObject()[0];
                    movableRows[index + 1][position](movableRows[0]);
                    if (freeze) {
                        var rightFrozenRows = this.parent.getFrozenRightDataRows();
                        rightFrozenRows[index + 1][position](rightFrozenRows[0]);
                    }
                    if (this.parent.editSettings.mode === 'Batch') {
                        this.parent.grid.contentModule[frows].splice(0, 1);
                        this.parent.grid.contentModule[frows].splice(rowObjectIndex, 0, newFreezeRowObject);
                    }
                    sf.base.setValue('batchIndex', index + 1, this.batchEditModule);
                }
                if (this.parent.editSettings.mode === 'Row' || this.parent.editSettings.mode === 'Cell') {
                    var errors = this.parent.grid.getContentTable().querySelectorAll('.e-griderror');
                    for (var i = 0; i < errors.length; i++) {
                        errors[i].remove();
                    }
                    sf.base.setValue('errorRules', [], this.parent.grid.editModule.formObj);
                }
                if (isVirtualization) {
                    this.prevAriaRowIndex = '-1';
                }
                focussedElement.focus();
            }
        }
    };
    // private beforeDataBound(args: BeforeDataBoundArgs): void {
    //   if (this.parent.grid.isEdit && this.parent.dataSource instanceof DataManager &&
    //         this.parent.dataSource.adaptor instanceof RemoteSaveAdaptor) {
    //     let action: string = getValue('action', args);
    //     let data: Object = getValue('data', args);
    //     if (action === 'edit' && !isNullOrUndefined(this.editedData)) {
    //       data = extend(this.editedData, data);
    //       this.editedData = null;
    //     }
    //     if (!isNullOrUndefined(this.addedData)) {
    //       let addedData: Object = args.result[args.result.length - 1];
    //       addedData = extend(this.addedData, addedData);
    //       this.addedData = null;
    //       args.result.splice(this.addedIndex, 0, addedData);
    //       args.result.splice(args.result.length, 1);
    //     }
    //   }
    // }
    Edit$$1.prototype.beginEdit = function (args) {
        if (args.requestType === 'refresh' && this.isOnBatch) {
            args.cancel = true;
            return;
        }
        if (this.parent.editSettings.mode === 'Cell' && args.requestType === 'beginEdit') {
            args.cancel = true;
            return;
        }
        if (this.doubleClickTarget && (this.doubleClickTarget.classList.contains('e-treegridexpand') ||
            this.doubleClickTarget.classList.contains('e-treegridcollapse') || this.doubleClickTarget.classList.contains('e-frame'))) {
            args.cancel = true;
            this.doubleClickTarget = null;
            return;
        }
        if (args.requestType === 'delete') {
            var data = args.data;
            for (var i = 0; i < data.length; i++) {
                this.deleteUniqueID(data[i].uniqueID);
                var childs = findChildrenRecords(data[i]);
                for (var c = 0; c < childs.length; c++) {
                    this.deleteUniqueID(childs[c].uniqueID);
                }
                args.data = data.concat(childs);
            }
        }
        if (args.requestType === 'add' || (this.isAddedRowByMethod && (this.parent.enableVirtualization || this.parent.enableInfiniteScrolling))) {
            if (!(this.parent.grid.selectedRowIndex === -1 && this.isAddedRowByMethod)
                && args.index === this.parent.grid.selectedRowIndex || args.index === 0) {
                this.selectedIndex = this.parent.grid.selectedRowIndex;
            }
            if (this.parent.enableVirtualization) {
                var selector = '.e-row[aria-rowindex="' + this.selectedIndex + '"]';
                var row = void 0;
                if (this.selectedIndex > -1 && this.parent.editSettings.newRowPosition !== 'Top' &&
                    this.parent.editSettings.newRowPosition !== 'Bottom') {
                    this.prevAriaRowIndex = this.selectedIndex.toString();
                    row = this.parent.getContent().querySelector(selector);
                    this.addRowIndex = row ? row.rowIndex : 0;
                }
                else {
                    if (this.prevAriaRowIndex && this.prevAriaRowIndex !== '-1') {
                        selector = '.e-row[aria-rowindex="' + this.prevAriaRowIndex + '"]';
                        row = this.parent.getContent().querySelector(selector);
                        this.addRowIndex = row ? row.rowIndex : 0;
                    }
                    else {
                        this.addRowIndex = 0;
                    }
                }
            }
            else {
                if (this.isAddedRowByMethod && (this.parent.enableVirtualization || this.parent.enableInfiniteScrolling)) {
                    this.addRowIndex = args.index;
                }
                else {
                    this.addRowIndex = this.parent.grid.selectedRowIndex > -1 ? this.parent.grid.selectedRowIndex : 0;
                }
            }
            if (this.isAddedRowByMethod && (this.parent.enableVirtualization || this.parent.enableInfiniteScrolling)) {
                this.addRowRecord = this.parent.flatData[args.index];
            }
            else {
                this.addRowRecord = this.parent.getSelectedRecords()[0];
            }
        }
        if (this.isAddedRowByMethod && args.index !== 0) {
            this.addRowRecord = this.parent.flatData[args.index];
        }
        if (this.parent.editSettings.newRowPosition === 'Child' && sf.base.isNullOrUndefined(this.addRowRecord)
            && !sf.base.isNullOrUndefined(this.parent.getSelectedRecords()[0])) {
            this.addRowRecord = this.parent.getSelectedRecords()[0];
        }
        this.isAddedRowByMethod = false;
        args = this.beginAddEdit(args);
        // if (args.requestType === 'save' &&
        //    ((this.parent.dataSource instanceof DataManager && this.parent.dataSource.adaptor instanceof RemoteSaveAdaptor))) {
        //      if (args.action === 'edit') {
        //           this.editedData = args.data;
        //      } else if (args.action === 'add') {
        //           this.addedData = value;
        //      }
        // }
    };
    Edit$$1.prototype.savePreviousRowPosition = function () {
        if (this.previousNewRowPosition === null) {
            this.previousNewRowPosition = this.parent.editSettings.newRowPosition;
        }
    };
    Edit$$1.prototype.beginAddEdit = function (args) {
        var value = args.data;
        if (args.action === 'add') {
            var key = this.parent.grid.getPrimaryKeyFieldNames()[0];
            var position = null;
            value.taskData = sf.base.isNullOrUndefined(value.taskData) ? sf.base.extend({}, args.data) : value.taskData;
            var currentData = this.parent.grid.getCurrentViewRecords();
            var index = this.addRowIndex;
            value.uniqueID = sf.grids.getUid(this.parent.element.id + '_data_');
            sf.base.setValue('uniqueIDCollection.' + value.uniqueID, value, this.parent);
            var level = 0;
            var idMapping = void 0;
            var parentUniqueID = void 0;
            var parentItem = void 0;
            var parentIdMapping = void 0;
            var isVirtualization = this.parent.enableVirtualization && this.addRowIndex > -1 && this.prevAriaRowIndex !== '-1';
            var rows = this.parent.getRows();
            var firstAriaIndex = rows.length ? +rows[0].getAttribute('aria-rowindex') : 0;
            var lastAriaIndex = rows.length ? +rows[rows.length - 1].getAttribute('aria-rowindex') : 0;
            var withinRange = this.selectedIndex >= firstAriaIndex && this.selectedIndex <= lastAriaIndex;
            if (currentData.length) {
                idMapping = currentData[this.addRowIndex][this.parent.idMapping];
                parentIdMapping = currentData[this.addRowIndex][this.parent.parentIdMapping];
                if (currentData[this.addRowIndex].parentItem) {
                    parentUniqueID = currentData[this.addRowIndex].parentItem.uniqueID;
                }
                parentItem = currentData[this.addRowIndex].parentItem;
            }
            if (this.parent.editSettings.newRowPosition !== 'Top' && currentData.length) {
                level = currentData[this.addRowIndex].level;
                if (this.parent.editSettings.newRowPosition === 'Above') {
                    position = 'before';
                    index = currentData[this.addRowIndex].index;
                }
                else if (this.parent.editSettings.newRowPosition === 'Below') {
                    position = 'after';
                    var childRecordCount = findChildrenRecords(currentData[this.addRowIndex]).length;
                    var currentDataIndex = currentData[this.addRowIndex].index;
                    index = (childRecordCount > 0) ? (currentDataIndex + childRecordCount) : (currentDataIndex);
                }
                else if (this.parent.editSettings.newRowPosition === 'Child') {
                    position = 'after';
                    if ((this.selectedIndex > -1 || isVirtualization) && withinRange) {
                        value.parentItem = sf.base.extend({}, currentData[this.addRowIndex]);
                        value.parentUniqueID = value.parentItem.uniqueID;
                        delete value.parentItem.childRecords;
                        delete value.parentItem[this.parent.childMapping];
                    }
                    var childRecordCount1 = findChildrenRecords(currentData[this.addRowIndex]).length;
                    var currentDataIndex1 = currentData[this.addRowIndex].index;
                    if (this.selectedIndex >= 0) {
                        value.level = level + 1;
                    }
                    index = (childRecordCount1 > 0) ? (currentDataIndex1 + childRecordCount1) : (currentDataIndex1);
                    if (this.isSelfReference) {
                        value.taskData[this.parent.parentIdMapping] = value[this.parent.parentIdMapping] = idMapping;
                        if (!sf.base.isNullOrUndefined(value.parentItem)) {
                            updateParentRow(key, value.parentItem, 'add', this.parent, this.isSelfReference, value);
                        }
                    }
                }
                if (this.parent.editSettings.newRowPosition === 'Above' || this.parent.editSettings.newRowPosition === 'Below') {
                    if ((this.selectedIndex > -1 || isVirtualization) && level && withinRange) {
                        value.parentUniqueID = parentUniqueID;
                        value.parentItem = sf.base.extend({}, parentItem);
                        delete value.parentItem.childRecords;
                        delete value.parentItem[this.parent.childMapping];
                    }
                    value.level = level;
                    if (this.isSelfReference) {
                        value.taskData[this.parent.parentIdMapping] = value[this.parent.parentIdMapping] = parentIdMapping;
                        if (!sf.base.isNullOrUndefined(value.parentItem)) {
                            updateParentRow(key, value.parentItem, 'add', this.parent, this.isSelfReference, value);
                        }
                    }
                }
                if (position != null && (this.selectedIndex > -1 || isVirtualization) && withinRange) {
                    args.index = position === 'before' ? index : index + 1;
                }
                if (this.parent.editSettings.newRowPosition === 'Bottom') {
                    var dataSource = (this.parent.grid.dataSource instanceof sf.data.DataManager ?
                        this.parent.grid.dataSource.dataSource.json : this.parent.grid.dataSource);
                    args.index = dataSource.length;
                }
            }
            if (sf.base.isNullOrUndefined(value.level)) {
                value.level = level;
            }
            value.hasChildRecords = false;
            value.childRecords = [];
            value.index = 0;
        }
        if (args.action === 'add') {
            this.internalProperties = { level: value.level, parentItem: value.parentItem, uniqueID: value.uniqueID,
                taskData: value.taskData, parentUniqueID: sf.base.isNullOrUndefined(value.parentItem) ? undefined : value.parentItem.uniqueID,
                childRecords: value.childRecords };
        }
        if (args.requestType === 'delete') {
            var deletedValues = args.data;
            for (var i = 0; i < deletedValues.length; i++) {
                if (deletedValues[i].parentItem) {
                    var parentItem = getParentData(this.parent, deletedValues[i].parentItem.uniqueID);
                    if (!sf.base.isNullOrUndefined(parentItem) && parentItem.hasChildRecords) {
                        var childIndex = parentItem.childRecords.indexOf(deletedValues[i]);
                        parentItem.childRecords.splice(childIndex, 1);
                    }
                }
            }
        }
        return args;
    };
    /**
     * If the data,index and position given, Adds the record to treegrid rows otherwise it will create edit form.
     *
     * @returns {void}
     */
    Edit$$1.prototype.addRecord = function (data, index, position) {
        if (this.parent.editSettings.newRowPosition === this.previousNewRowPosition || this.previousNewRowPosition === null) {
            this.previousNewRowPosition = this.parent.editSettings.newRowPosition;
        }
        if (!this.isSelfReference && !sf.base.isNullOrUndefined(data) && Object.hasOwnProperty.call(data, this.parent.childMapping)) {
            var addRecords = [];
            var previousEditMode = this.parent.editSettings.mode;
            var previousGridEditMode = this.parent.grid.editSettings.mode;
            addRecords.push(data);
            this.parent.setProperties({ editSettings: { mode: 'Batch' } }, true);
            this.parent.grid.setProperties({ editSettings: { mode: 'Batch' } }, true);
            if (!sf.base.isNullOrUndefined(position)) {
                this.parent.setProperties({ editSettings: { newRowPosition: position } }, true);
            }
            var updatedRecords = { addedRecords: addRecords, changedRecords: [], deletedRecords: [] };
            this.parent.notify(batchSave, { updatedRecords: updatedRecords, index: index });
            this.parent.setProperties({ editSettings: { mode: previousEditMode } }, true);
            this.parent.grid.setProperties({ editSettings: { mode: previousGridEditMode } }, true);
            this.parent.refresh();
        }
        else {
            if (data) {
                if (index > -1) {
                    this.selectedIndex = index;
                    this.addRowIndex = index;
                }
                else {
                    this.selectedIndex = this.parent.selectedRowIndex;
                    this.addRowIndex = this.parent.selectedRowIndex;
                }
                if (position) {
                    this.parent.setProperties({ editSettings: { newRowPosition: position } }, true);
                }
                this.parent.grid.editModule.addRecord(data, index);
            }
            else {
                this.parent.grid.editModule.addRecord(data, index);
            }
        }
    };
    /**
     * Checks the status of validation at the time of editing. If validation is passed, it returns true.
     *
     * @returns {boolean} Returns form validation results
     */
    Edit$$1.prototype.editFormValidate = function () {
        return this.parent.grid.editModule.editFormValidate();
    };
    /**
     * @hidden
     * @returns {void}
     */
    Edit$$1.prototype.destroyForm = function () {
        this.parent.grid.editModule.destroyForm();
    };
    Edit$$1.prototype.contentready = function (e) {
        if (!sf.base.isNullOrUndefined(e.args.requestType)
            && (e.args.requestType.toString() === 'delete' || e.args.requestType.toString() === 'save'
                || (this.parent.editSettings.mode === 'Batch' && e.args.requestType.toString() === 'batchsave'))) {
            this.updateIndex(this.parent.grid.dataSource, this.parent.getRows(), this.parent.getCurrentViewRecords());
            if (this.parent.frozenRows || this.parent.getFrozenColumns() || this.parent.frozenColumns) {
                if (this.parent.grid.dataSource.length === this.parent.getMovableDataRows().length) {
                    this.updateIndex(this.parent.grid.dataSource, this.parent.getMovableDataRows(), this.parent.getCurrentViewRecords());
                }
            }
        }
    };
    /**
     * If the row index and field is given, edits the particular cell in a row.
     *
     * @returns {void}
     */
    Edit$$1.prototype.editCell = function (rowIndex, field) {
        if (this.parent.editSettings.mode === 'Cell' || this.parent.editSettings.mode === 'Batch') {
            if (this.parent.editSettings.mode !== 'Batch') {
                this.isOnBatch = true;
                this.updateGridEditMode('Batch');
            }
            this.parent.grid.editModule.editCell(rowIndex, field);
        }
    };
    return Edit$$1;
}());

/**
 * Command Column Module for TreeGrid
 *
 * @hidden
 */
var CommandColumn$1 = /** @class */ (function () {
    function CommandColumn$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.CommandColumn);
        this.parent = parent;
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns CommandColumn module name
     */
    CommandColumn$$1.prototype.getModuleName = function () {
        return 'commandColumn';
    };
    /**
     * Destroys the ContextMenu.
     *
     * @function destroy
     * @returns {void}
     */
    CommandColumn$$1.prototype.destroy = function () {
        //this.removeEventListener();
    };
    return CommandColumn$$1;
}());

/**
 * TreeGrid Detail Row module
 *
 * @hidden
 */
var DetailRow$1 = /** @class */ (function () {
    function DetailRow$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.DetailRow);
        this.parent = parent;
        this.addEventListener();
    }
    /**
     * @hidden
     */
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns DetailRow module name
     */
    DetailRow$$1.prototype.getModuleName = function () {
        return 'detailRow';
    };
    DetailRow$$1.prototype.addEventListener = function () {
        this.parent.on('dataBoundArg', this.dataBoundArg, this);
        this.parent.on('detaildataBound', this.detaildataBound, this);
        this.parent.grid.on('detail-indentcell-info', this.setIndentVisibility, this);
        this.parent.on('childRowExpand', this.childRowExpand, this);
        this.parent.on('rowExpandCollapse', this.rowExpandCollapse, this);
        this.parent.on('actioncomplete', this.actioncomplete, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    DetailRow$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('dataBoundArg', this.dataBoundArg);
        this.parent.off('detaildataBound', this.detaildataBound);
        this.parent.off('childRowExpand', this.childRowExpand);
        this.parent.off('rowExpandCollapse', this.rowExpandCollapse);
        this.parent.off('actioncomplete', this.actioncomplete);
        this.parent.grid.off('detail-indentcell-info', this.setIndentVisibility);
    };
    DetailRow$$1.prototype.setIndentVisibility = function (args) {
        var visible = 'visible';
        args[visible] = false;
    };
    DetailRow$$1.prototype.dataBoundArg = function () {
        var detailele = this.parent.getRows().filter(function (e) {
            return !e.classList.contains('e-detailrow');
        });
        for (var i = 0; i < detailele.length; i++) {
            var elements = detailele[i].getElementsByClassName('e-detailrowcollapse');
            var detailData = this.parent.grid.getRowObjectFromUID(detailele[i].getAttribute('data-Uid'));
            var parentItem = sf.grids.getObject('parentItem', this.parent.grid.getCurrentViewRecords()[i]);
            if (sf.base.isNullOrUndefined(parentItem) || !sf.base.isNullOrUndefined(parentItem) &&
                getExpandStatus(this.parent, detailData.data, this.parent.grid.getCurrentViewRecords())) {
                this.parent.grid.detailRowModule.expand(elements[0]);
            }
        }
    };
    DetailRow$$1.prototype.childRowExpand = function (args) {
        var detailRowElement = args.row.getElementsByClassName('e-detailrowcollapse');
        if (!sf.base.isNullOrUndefined(detailRowElement[0])) {
            this.parent.grid.detailRowModule.expand(detailRowElement[0]);
        }
    };
    DetailRow$$1.prototype.rowExpandCollapse = function (args) {
        if (isRemoteData(this.parent)) {
            return;
        }
        for (var i = 0; i < args.detailrows.length; i++) {
            args.detailrows[i].style.display = args.action;
        }
    };
    DetailRow$$1.prototype.detaildataBound = function (args) {
        var data = args.data;
        var row = args.detailElement.parentElement.previousSibling;
        var index = !sf.base.isNullOrUndefined(data.parentItem) ? data.parentItem.index : data.index;
        var expandClass = 'e-gridrowindex' + index + 'level' + data.level;
        var classlist = row.querySelector('.' + expandClass).classList;
        var gridClas = [].slice.call(classlist).filter(function (gridclass) { return (gridclass === expandClass); });
        var newNo = gridClas[0].length;
        var slicedclas = gridClas.toString().slice(6, newNo);
        var detailClass = 'e-griddetail' + slicedclas;
        sf.base.addClass([args.detailElement.parentElement], detailClass);
    };
    DetailRow$$1.prototype.actioncomplete = function (args) {
        if (args.requestType === 'beginEdit' || args.requestType === 'add') {
            var spann = (args.row.querySelectorAll('.e-editcell')[0].getAttribute('colSpan'));
            var colum = parseInt(spann, 10) - 1;
            var updtdcolum = colum.toString();
            args.row.querySelectorAll('.e-editcell')[0].setAttribute('colSpan', updtdcolum);
        }
        var focusElement = this.parent.grid.contentModule.getRows();
        for (var i = 0; i < focusElement.length; i++) {
            focusElement[i].cells[0].visible = false;
        }
        var focusModule = sf.grids.getObject('focusModule', this.parent.grid);
        var matrix = 'refreshMatrix';
        focusModule[matrix](true)({ rows: this.parent.grid.contentModule.getRows() });
    };
    /**
     * Destroys the DetailModule.
     *
     * @function destroy
     * @returns {void}
     */
    DetailRow$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    return DetailRow$$1;
}());

var __extends$15 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var VirtualTreeContentRenderer = /** @class */ (function (_super) {
    __extends$15(VirtualTreeContentRenderer, _super);
    function VirtualTreeContentRenderer(parent, locator) {
        var _this = _super.call(this, parent, locator) || this;
        _this.isExpandCollapse = false;
        _this.translateY = 0;
        _this.maxiPage = 0;
        _this.recordAdded = false;
        /** @hidden */
        _this.startIndex = -1;
        _this.endIndex = -1;
        _this.preTranslate = 0;
        _this.isRemoteExpand = false;
        /** @hidden */
        _this.isDataSourceChanged = false;
        _this.addEventListener();
        return _this;
    }
    VirtualTreeContentRenderer.prototype.getModelGenerator = function () {
        return new TreeVirtualRowModelGenerator(this.parent);
    };
    VirtualTreeContentRenderer.prototype.getRowByIndex = function (index) {
        return this.parent.getDataRows().filter(function (e) { return parseInt(e.getAttribute('aria-rowindex'), 10) === index; })[0];
    };
    VirtualTreeContentRenderer.prototype.addEventListener = function () {
        this.parent.on(virtualActionArgs, this.virtualOtherAction, this);
        this.parent.on(indexModifier, this.indexModifier, this);
    };
    VirtualTreeContentRenderer.prototype.virtualOtherAction = function (args) {
        if (args.setTop) {
            this.translateY = 0;
            this.startIndex = 0;
            this.endIndex = this.parent.pageSettings.pageSize - 1;
        }
        else if (args.isExpandCollapse) {
            this.isExpandCollapse = true;
        }
    };
    VirtualTreeContentRenderer.prototype.indexModifier = function (args) {
        var content = this.parent.getContent().querySelector('.e-content');
        if (this.recordAdded && this.startIndex > -1 && this.endIndex > -1) {
            if (this.endIndex > args.count - this.parent.pageSettings.pageSize) {
                var nextSetResIndex = ~~(content.scrollTop / this.parent.getRowHeight());
                var lastIndex = nextSetResIndex + this.parent.getRows().length;
                if (lastIndex > args.count) {
                    lastIndex = nextSetResIndex +
                        (args.count - nextSetResIndex);
                }
                this.startIndex = lastIndex - this.parent.getRows().length;
                this.endIndex = lastIndex;
            }
            else {
                this.startIndex += 1;
                this.endIndex += 1;
            }
            this.recordAdded = false;
        }
        if (this.isDataSourceChanged) {
            this.startIndex = 0;
            this.endIndex = this.parent.pageSettings.pageSize - 1;
        }
        args.startIndex = this.startIndex;
        args.endIndex = this.endIndex;
    };
    VirtualTreeContentRenderer.prototype.eventListener = function (action) {
        var _this = this;
        if (!(this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.dataSource.url !== undefined
            && this.parent.dataSource.dataSource.offline && this.parent.dataSource.dataSource.url !== '') || !isCountRequired(this.parent)) {
            this.parent[action]('data-ready', this.onDataReady, this);
            //this.parent[action]('refresh-virtual-block', this.refreshContentRows, this);
            this.fn = function () {
                _this.observers.observes(function (scrollArgs) { return _this.scrollListeners(scrollArgs); });
                _this.parent.off('content-ready', _this.fn);
            };
            this.parent.addEventListener('dataBound', this.dataBoundEvent.bind(this));
            this.parent.addEventListener('rowSelected', this.rowSelectedEvent.bind(this));
            this.parent[action]('select-virtual-Row', this.toSelectVirtualRow, this);
            this.parent.on('content-ready', this.fn, this);
            this.parent.addEventListener(actionComplete, this.onActionComplete.bind(this));
            this.parent[action]('virtual-scroll-edit-action-begin', this.beginEdit, this);
            this.parent[action]('virtual-scroll-add-action-begin', this.beginAdd, this);
            this.parent[action]('virtual-scroll-edit-success', this.virtualEditSuccess, this);
            this.parent[action]('edit-reset', this.resetIseditValue, this);
            this.parent[action]('get-virtual-data', this.getData, this);
            this.parent[action]('virtual-scroll-edit-cancel', this.cancelEdit, this);
            this.parent[action]('select-row-on-context-open', this.toSelectRowOnContextOpen, this);
            this.parent[action]('refresh-virtual-editform-cells', this.refreshCell, this);
            this.parent[action]('virtaul-cell-focus', this.cellFocus, this);
        }
        else {
            _super.prototype.eventListener.call(this, 'on');
        }
    };
    VirtualTreeContentRenderer.prototype.cellFocus = function (e) {
        var virtualCellFocus = 'virtualCellFocus';
        _super.prototype[virtualCellFocus].call(this, e);
    };
    VirtualTreeContentRenderer.prototype.onDataReady = function (e) {
        _super.prototype.onDataReady.call(this, e);
        if (!(this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.dataSource.url !== undefined
            && this.parent.dataSource.dataSource.offline && this.parent.dataSource.dataSource.url !== '') || !isCountRequired(this.parent)) {
            if (!sf.base.isNullOrUndefined(e.count)) {
                this.totalRecords = e.count;
                sf.base.getValue('virtualEle', this).setVirtualHeight(this.parent.getRowHeight() * e.count, '100%');
            }
            if ((!sf.base.isNullOrUndefined(e.requestType) && e.requestType.toString() === 'collapseAll') || this.isDataSourceChanged) {
                this.contents.scrollTop = 0;
                this.isDataSourceChanged = false;
            }
        }
    };
    VirtualTreeContentRenderer.prototype.renderTable = function () {
        _super.prototype.renderTable.call(this);
        if (!(this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.dataSource.url !== undefined
            && this.parent.dataSource.dataSource.offline && this.parent.dataSource.dataSource.url !== '') || !isCountRequired(this.parent)) {
            sf.base.getValue('observer', this).options.debounceEvent = false;
            this.observers = new TreeInterSectionObserver(sf.base.getValue('observer', this).element, sf.base.getValue('observer', this).options);
            this.contents = this.getPanel().firstChild;
        }
    };
    VirtualTreeContentRenderer.prototype.getTranslateY = function (sTop, cHeight, info, isOnenter) {
        if ((this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.dataSource.url !== undefined
            && !this.parent.dataSource.dataSource.offline && this.parent.dataSource.dataSource.url !== '') || isCountRequired(this.parent)) {
            if (this.isRemoteExpand) {
                this.isRemoteExpand = false;
                return this.preTranslate;
            }
            else {
                this.preTranslate = _super.prototype.getTranslateY.call(this, sTop, cHeight, info, isOnenter);
                return _super.prototype.getTranslateY.call(this, sTop, cHeight, info, isOnenter);
            }
        }
        else {
            return _super.prototype.getTranslateY.call(this, sTop, cHeight, info, isOnenter);
        }
    };
    VirtualTreeContentRenderer.prototype.dataBoundEvent = function () {
        var dataBoundEve = 'dataBound';
        var initialRowTop = 'initialRowTop';
        if (this.parent.getRows().length && !this[initialRowTop] && !sf.base.isNullOrUndefined(this.parent.getRowByIndex(0))) {
            var rowTop = this.parent.getRowByIndex(0).getBoundingClientRect().top;
            var gridTop = this.parent.element.getBoundingClientRect().top;
            if (rowTop > 0) {
                this[initialRowTop] = this.parent.getRowByIndex(0).getBoundingClientRect().top - gridTop;
            }
            else {
                this[initialRowTop] = this.content.getBoundingClientRect().top -
                    this.parent.getRowByIndex(0).getBoundingClientRect().height;
            }
        }
        _super.prototype[dataBoundEve].call(this);
    };
    VirtualTreeContentRenderer.prototype.rowSelectedEvent = function (args) {
        var rowSelected$$1 = 'rowSelected';
        _super.prototype[rowSelected$$1].call(this, args);
    };
    VirtualTreeContentRenderer.prototype.toSelectVirtualRow = function (args) {
        if (this.parent.isEdit) {
            return;
        }
        var selectVirtualRow = 'selectVirtualRow';
        _super.prototype[selectVirtualRow].call(this, args);
    };
    VirtualTreeContentRenderer.prototype.refreshCell = function (rowObj) {
        rowObj.cells = this.generateCells();
    };
    VirtualTreeContentRenderer.prototype.generateCells = function () {
        var cells = [];
        for (var i = 0; i < this.parent.columns.length; i++) {
            cells.push(this.generateCell(this.parent.columns[i]));
        }
        return cells;
    };
    VirtualTreeContentRenderer.prototype.generateCell = function (col, rowId, cellType, colSpan, oIndex, foreignKeyData) {
        var opt = {
            'visible': col.visible,
            'isDataCell': !sf.base.isNullOrUndefined(col.field || col.template),
            'isTemplate': !sf.base.isNullOrUndefined(col.template),
            'rowID': rowId,
            'column': col,
            'cellType': !sf.base.isNullOrUndefined(cellType) ? cellType : sf.grids.CellType.Data,
            'colSpan': colSpan,
            'commands': col.commands,
            'isForeignKey': col.isForeignColumn && col.isForeignColumn(),
            'foreignKeyData': col.isForeignColumn && col.isForeignColumn() && sf.base.getValue(col.field, foreignKeyData)
        };
        if (opt.isDataCell || opt.column.type === 'checkbox' || opt.commands) {
            opt.index = oIndex;
        }
        return new sf.grids.Cell(opt);
    };
    VirtualTreeContentRenderer.prototype.beginEdit = function (e) {
        var selector = '.e-row[aria-rowindex="' + e.index + '"]';
        var index = this.parent.getContent().querySelector(selector).rowIndex;
        var rowData = this.parent.getCurrentViewRecords()[index];
        e.data = rowData;
    };
    VirtualTreeContentRenderer.prototype.beginAdd = function (args) {
        var addAction = 'addActionBegin';
        var isAdd = 'isAdd';
        var addArgs = { newRowPosition: this.rowPosition, addRowIndex: this.addRowIndex, ariaRowIndex: this.ariaRowIndex };
        this.parent.notify('get-row-position', addArgs);
        this.rowPosition = addArgs.newRowPosition;
        this.addRowIndex = addArgs.addRowIndex;
        this.ariaRowIndex = addArgs.ariaRowIndex;
        var rows = this.parent.getRows();
        var firstAriaIndex = rows.length ? +rows[0].getAttribute('aria-rowindex') : 0;
        var lastAriaIndex = rows.length ? +rows[rows.length - 1].getAttribute('aria-rowindex') : 0;
        var withInRange = this.parent.selectedRowIndex >= firstAriaIndex && this.parent.selectedRowIndex <= lastAriaIndex;
        if (!(this.rowPosition === 'Top' || this.rowPosition === 'Bottom')) {
            this[isAdd] = true;
        }
        if (this.rowPosition === 'Top' || this.rowPosition === 'Bottom' ||
            ((!this.addRowIndex || this.addRowIndex === -1) && (this.parent.selectedRowIndex === -1 || !withInRange))) {
            _super.prototype[addAction].call(this, args);
        }
    };
    VirtualTreeContentRenderer.prototype.restoreEditState = function () {
        var restoreEdit = 'restoreEdit';
        _super.prototype[restoreEdit].call(this);
    };
    VirtualTreeContentRenderer.prototype.resetIseditValue = function () {
        var resetIsEdit = 'resetIsedit';
        var isAdd = 'isAdd';
        this.parent.notify('reset-edit-props', {});
        if ((this.rowPosition === 'Top' || this.rowPosition === 'Bottom') && this[isAdd]) {
            _super.prototype[resetIsEdit].call(this);
        }
    };
    VirtualTreeContentRenderer.prototype.virtualEditSuccess = function () {
        var isAdd = 'isAdd';
        var content = this.parent.getContent().querySelector('.e-content');
        if (this[isAdd] && content.querySelector('.e-addedrow')) {
            this.recordAdded = true;
        }
    };
    VirtualTreeContentRenderer.prototype.cancelEdit = function (args) {
        var editCancel = 'editCancel';
        _super.prototype[editCancel].call(this, args);
    };
    VirtualTreeContentRenderer.prototype.toSelectRowOnContextOpen = function (args) {
        var selectRowOnContextOpen = 'selectRowOnContextOpen';
        _super.prototype[selectRowOnContextOpen].call(this, args);
    };
    VirtualTreeContentRenderer.prototype.restoreNewRow = function () {
        var isAdd = 'isAdd';
        var content = this.parent.getContent().querySelector('.e-content');
        if (this[isAdd] && !content.querySelector('.e-addedrow')) {
            this.parent.isEdit = false;
            this.parent.addRecord();
        }
    };
    VirtualTreeContentRenderer.prototype.getData = function (data) {
        var getVirtualData = 'getVirtualData';
        _super.prototype[getVirtualData].call(this, data);
    };
    VirtualTreeContentRenderer.prototype.onActionComplete = function (args) {
        if (args.requestType === 'add') {
            var addArgs = { newRowPosition: this.rowPosition, addRowIndex: this.addRowIndex, ariaRowIndex: this.ariaRowIndex };
            this.parent.notify('get-row-position', addArgs);
            this.rowPosition = addArgs.newRowPosition;
            this.addRowIndex = addArgs.addRowIndex;
            this.ariaRowIndex = addArgs.ariaRowIndex;
        }
        var actionComplete$$1 = 'actionComplete';
        _super.prototype[actionComplete$$1].call(this, args);
    };
    VirtualTreeContentRenderer.prototype.scrollListeners = function (scrollArgs) {
        var info = scrollArgs.sentinel;
        var outBuffer = 10; //this.parent.pageSettings.pageSize - Math.ceil(this.parent.pageSettings.pageSize / 1.5);
        var content = this.parent.getContent().querySelector('.e-content');
        var scrollHeight = outBuffer * this.parent.getRowHeight();
        var upScroll = (scrollArgs.offset.top - this.translateY) < 0;
        var downScroll = Math.ceil(scrollArgs.offset.top - this.translateY) >= scrollHeight;
        var selectedRowIndex = 'selectedRowIndex';
        if (upScroll) {
            var vHeight = +(this.parent.height.toString().indexOf('%') < 0 ? this.parent.height :
                this.parent.element.getBoundingClientRect().height);
            var index = (~~(content.scrollTop / this.parent.getRowHeight())
                + Math.ceil(vHeight / this.parent.getRowHeight()))
                - this.parent.pageSettings.pageSize;
            index = (index > 0) ? index : 0;
            if (!sf.base.isNullOrUndefined(this[selectedRowIndex]) && this[selectedRowIndex] !== -1 && index !== this[selectedRowIndex]) {
                index = this[selectedRowIndex];
            }
            this.startIndex = index;
            this.endIndex = index + this.parent.pageSettings.pageSize;
            if (this.endIndex > this.totalRecords) {
                var lastInx = this.totalRecords - 1;
                var remains = this.endIndex % lastInx;
                this.endIndex = lastInx;
                this.startIndex = (this.startIndex - remains) < 0 ? 0 : (this.startIndex - remains);
            }
            //var firsttdinx = parseInt(this.parent.getContent().querySelector('.e-content td').getAttribute('index'), 0);
            var rowPt = Math.ceil(scrollArgs.offset.top / this.parent.getRowHeight());
            rowPt = rowPt % this.parent.pageSettings.pageSize;
            var firsttdinx = 0;
            if (!sf.base.isNullOrUndefined(this.parent.getRows()[rowPt]) &&
                !sf.base.isNullOrUndefined(this.parent.getContent().querySelectorAll('.e-content tr')[rowPt])) {
                var attr = this.parent.getContent().querySelectorAll('.e-content tr')[rowPt]
                    .querySelector('td').getAttribute('index');
                firsttdinx = +attr; // this.parent.getContent().querySelector('.e-content tr').getAttribute('aria-rowindex');
            }
            if (firsttdinx === 0) {
                this.translateY = scrollArgs.offset.top;
            }
            else {
                var height = this.parent.getRowHeight();
                this.translateY = (scrollArgs.offset.top - (outBuffer * height) > 0) ?
                    scrollArgs.offset.top - (outBuffer * height) + 10 : 0;
            }
        }
        else if (downScroll) {
            var nextSetResIndex = ~~(content.scrollTop / this.parent.getRowHeight());
            var isLastBlock = (this[selectedRowIndex] + this.parent.pageSettings.pageSize) < this.totalRecords ? false : true;
            if (!sf.base.isNullOrUndefined(this[selectedRowIndex]) && this[selectedRowIndex] !== -1 &&
                nextSetResIndex !== this[selectedRowIndex] && !isLastBlock) {
                nextSetResIndex = this[selectedRowIndex];
            }
            var lastIndex = nextSetResIndex + this.parent.pageSettings.pageSize;
            if (lastIndex > this.totalRecords) {
                lastIndex = nextSetResIndex +
                    (this.totalRecords - nextSetResIndex);
            }
            this.startIndex = !isLastBlock ? lastIndex - this.parent.pageSettings.pageSize : nextSetResIndex;
            this.endIndex = lastIndex;
            if (scrollArgs.offset.top > (this.parent.getRowHeight() * this.totalRecords)) {
                this.translateY = this.getTranslateY(scrollArgs.offset.top, content.getBoundingClientRect().height);
            }
            else {
                this.translateY = scrollArgs.offset.top;
            }
        }
        if ((downScroll && (scrollArgs.offset.top < (this.parent.getRowHeight() * this.totalRecords)))
            || (upScroll)) {
            var viewInfo = sf.base.getValue('getInfoFromView', this).apply(this, [scrollArgs.direction, info, scrollArgs.offset]);
            this.previousInfo = viewInfo;
            var page = viewInfo.loadNext && !viewInfo.loadSelf ? viewInfo.nextInfo.page : viewInfo.page;
            this.parent.setProperties({ pageSettings: { currentPage: page } }, true);
            viewInfo.event = viewInfo.event === 'refresh-virtual-block' ? 'model-changed' : viewInfo.event;
            this.parent.notify(viewInfo.event, { requestType: 'virtualscroll', focusElement: scrollArgs.focusElement });
        }
    };
    VirtualTreeContentRenderer.prototype.appendContent = function (target, newChild, e) {
        if ((this.parent.dataSource instanceof sf.data.DataManager && this.parent.dataSource.dataSource.url !== undefined
            && !this.parent.dataSource.dataSource.offline && this.parent.dataSource.dataSource.url !== '') || isCountRequired(this.parent)) {
            if (sf.base.getValue('isExpandCollapse', e)) {
                this.isRemoteExpand = true;
            }
            _super.prototype.appendContent.call(this, target, newChild, e);
        }
        else {
            var info = e.virtualInfo.sentinelInfo && e.virtualInfo.sentinelInfo.axis === 'Y' &&
                sf.base.getValue('currentInfo', this).page && sf.base.getValue('currentInfo', this).page !== e.virtualInfo.page ?
                sf.base.getValue('currentInfo', this) : e.virtualInfo;
            var cBlock = (info.columnIndexes[0]) - 1;
            var cOffset = this.getColumnOffset(cBlock);
            this.virtualEle.setWrapperWidth(null, (sf.base.Browser.isIE || sf.base.Browser.info.name === 'edge'));
            target = this.parent.createElement('tbody');
            target.appendChild(newChild);
            var replace = 'replaceWith';
            this.getTable().querySelector('tbody')[replace](target);
            if (!this.isExpandCollapse || this.translateY === 0) {
                sf.base.getValue('virtualEle', this).adjustTable(cOffset, this.translateY);
            }
            else {
                this.isExpandCollapse = false;
            }
            sf.base.setValue('prevInfo', this.previousInfo ? this.previousInfo : info, this);
            var focusCell = 'focusCell';
            var restoreAdd = 'restoreAdd';
            _super.prototype[focusCell].call(this, e);
            var isAdd = 'isAdd';
            if (this[isAdd] && !this.parent.getContent().querySelector('.e-content').querySelector('.e-addedrow')) {
                if (!(this.rowPosition === 'Top' || this.rowPosition === 'Bottom')) {
                    if (this.ariaRowIndex >= this.startIndex) {
                        this.restoreNewRow();
                    }
                    else if (this.addRowIndex && this.addRowIndex > -1) {
                        this[isAdd] = false;
                        this.parent.isEdit = false;
                    }
                }
            }
            this.restoreEditState();
            _super.prototype[restoreAdd].call(this);
        }
    };
    VirtualTreeContentRenderer.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('data-ready', this.onDataReady);
        this.parent.off('content-ready', this.fn);
        this.parent.off('select-virtual-Row', this.toSelectVirtualRow);
        this.parent.off('dataBound', this.dataBoundEvent);
        this.parent.off('rowSelected', this.rowSelectedEvent);
        this.parent.off(virtualActionArgs, this.virtualOtherAction);
        this.parent.off(indexModifier, this.indexModifier);
        this.parent.off('virtual-scroll-edit-action-begin', this.beginEdit);
        this.parent.off('virtual-scroll-add-action-begin', this.beginAdd);
        this.parent.off('virtual-scroll-edit-success', this.virtualEditSuccess);
        this.parent.off('edit-reset', this.resetIseditValue);
        this.parent.off('get-virtual-data', this.getData);
        this.parent.off('virtual-scroll-edit-cancel', this.cancelEdit);
        this.parent.off('select-row-on-context-open', this.toSelectRowOnContextOpen);
        this.parent.off('refresh-virtual-editform-cells', this.refreshCell);
        this.parent.off('virtaul-cell-focus', this.cellFocus);
    };
    return VirtualTreeContentRenderer;
}(sf.grids.VirtualContentRenderer));
var TreeInterSectionObserver = /** @class */ (function (_super) {
    __extends$15(TreeInterSectionObserver, _super);
    function TreeInterSectionObserver() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.isWheeling = false;
        _this.newPos = 0;
        _this.lastPos = 0;
        _this.timer = 0;
        return _this;
    }
    TreeInterSectionObserver.prototype.observes = function (callback) {
        var containerRect = 'containerRect';
        _super.prototype[containerRect] = sf.base.getValue('options', this).container.getBoundingClientRect();
        sf.base.EventHandler.add(sf.base.getValue('options', this).container, 'scroll', this.virtualScrollHandlers(callback), this);
    };
    TreeInterSectionObserver.prototype.clear = function () {
        this.lastPos = null;
    };
    TreeInterSectionObserver.prototype.virtualScrollHandlers = function (callback) {
        var _this = this;
        var prevTop = 0;
        var prevLeft = 0;
        return function (e) {
            var scrollTop = e.target.scrollTop;
            var scrollLeft = e.target.scrollLeft;
            var direction = prevTop < scrollTop ? 'down' : 'up';
            direction = prevLeft === scrollLeft ? direction : prevLeft < scrollLeft ? 'right' : 'left';
            prevTop = scrollTop;
            prevLeft = scrollLeft;
            var current = sf.base.getValue('sentinelInfo', _this)[direction];
            var delta = 0;
            _this.newPos = scrollTop;
            if (_this.lastPos != null) { // && newPos < maxScroll
                delta = _this.newPos - _this.lastPos;
            }
            _this.lastPos = _this.newPos;
            if (_this.timer) {
                clearTimeout(_this.timer);
            }
            _this.timer = setTimeout(_this.clear, 0);
            /*if (this.options.axes.indexOf(current.axis) === -1) {
            return;
        }*/
            /*if(delta > 45 || delta < -45){
          this.isWheeling = true;
        }*/
            if ((delta > 100 || delta < -100) && (e && e.preventDefault)) {
                e.returnValue = false;
                e.preventDefault();
            }
            callback({ direction: direction, isWheel: _this.isWheeling,
                sentinel: current, offset: { top: scrollTop, left: scrollLeft },
                focusElement: document.activeElement });
        };
    };
    return TreeInterSectionObserver;
}(sf.grids.InterSectionObserver));

var __extends$14 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/**
 * TreeGrid Virtual Scroll module will handle Virtualization
 *
 * @hidden
 */
var VirtualScroll$1 = /** @class */ (function () {
    /**
     * Constructor for VirtualScroll module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function VirtualScroll$$1(parent) {
        this.prevstartIndex = -1;
        this.prevendIndex = -1;
        this.parent = parent;
        sf.grids.Grid.Inject(TreeVirtual);
        this.addEventListener();
    }
    VirtualScroll$$1.prototype.returnVisualData = function (args) {
        args.data = this.visualData;
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} - Returns VirtualScroll module name
     */
    VirtualScroll$$1.prototype.getModuleName = function () {
        return 'virtualScroll';
    };
    /**
     * @hidden
     * @returns {void}
     */
    VirtualScroll$$1.prototype.addEventListener = function () {
        this.parent.on(localPagedExpandCollapse, this.collapseExpandVirtualchilds, this);
        this.parent.on(pagingActions, this.virtualPageAction, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    VirtualScroll$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off(localPagedExpandCollapse, this.collapseExpandVirtualchilds);
        this.parent.off(pagingActions, this.virtualPageAction);
    };
    VirtualScroll$$1.prototype.collapseExpandVirtualchilds = function (row) {
        this.parent.grid.notify(virtualActionArgs, { isExpandCollapse: true });
        this.expandCollapseRec = row.record;
        row.record.expanded = row.action === 'collapse' ? false : true;
        var ret = {
            result: this.parent.flatData,
            row: row.row,
            action: row.action,
            record: row.record,
            count: this.parent.flatData.length
        };
        this.parent.grid.clearSelection();
        var requestType = sf.base.getValue('isCollapseAll', this.parent) ? 'collapseAll' : 'refresh';
        sf.base.getValue('grid.renderModule', this.parent).dataManagerSuccess(ret, { requestType: requestType });
    };
    VirtualScroll$$1.prototype.virtualPageAction = function (pageingDetails) {
        var _this = this;
        var dm = new sf.data.DataManager(pageingDetails.result);
        var expanded$$1 = new sf.data.Predicate('expanded', 'notequal', null).or('expanded', 'notequal', undefined);
        var parents = dm.executeLocal(new sf.data.Query().where(expanded$$1));
        var visualData = parents.filter(function (e) {
            return getExpandStatus(_this.parent, e, parents);
        });
        this.visualData = visualData;
        this.parent.grid.notify(dataListener, { data: visualData });
        var counts = { startIndex: -1, endIndex: -1, count: pageingDetails.count };
        this.parent.grid.notify(indexModifier, counts);
        var startIndex = counts.startIndex;
        var endIndex = counts.endIndex;
        pageingDetails.count = visualData.length;
        if (startIndex === -1 && endIndex === -1) {
            var query = new sf.data.Query();
            var size = this.parent.grid.pageSettings.pageSize;
            var current = this.parent.grid.pageSettings.currentPage;
            var skip = size * (current - 1);
            query = query.skip(skip).take(size);
            dm.dataSource.json = visualData;
            pageingDetails.result = dm.executeLocal(query);
        }
        else {
            var requestType = pageingDetails.actionArgs.requestType;
            if (requestType === 'filtering' || requestType === 'collapseAll' ||
                (requestType === 'refresh' && this.parent.enableCollapseAll && endIndex > visualData.length)) {
                startIndex = 0;
                endIndex = this.parent.grid.pageSettings.pageSize - 1;
                this.parent.grid.getContent().firstElementChild.scrollTop = 0;
                this.parent.grid.notify(virtualActionArgs, { setTop: true });
            }
            //if ((this.prevendIndex !== -1 && this.prevstartIndex !== -1) &&
            //this.prevendIndex === endIndex && this.prevstartIndex === startIndex) {
            if (!sf.base.isNullOrUndefined(this.expandCollapseRec)) {
                var resourceCount = this.parent.getRows();
                var sIndex = visualData.indexOf(this.expandCollapseRec);
                var tempdata = visualData.slice(sIndex, sIndex + resourceCount.length);
                if (tempdata.length < resourceCount.length && sIndex >= 0) {
                    sIndex = visualData.length - resourceCount.length;
                    sIndex = sIndex > 0 ? sIndex : 0;
                    startIndex = sIndex;
                    endIndex = visualData.length;
                }
                else if (sf.base.getValue('isCollapseAll', this.parent)) {
                    startIndex = 0;
                    endIndex = this.parent.grid.pageSettings.pageSize - 1;
                    this.parent.grid.notify(virtualActionArgs, { setTop: true });
                }
                this.expandCollapseRec = null;
            }
            //}
            if (!sf.base.isNullOrUndefined(this.expandCollapseRec) && this.parent.enableCollapseAll) {
                if (pageingDetails.count < this.parent.getRows()[0].getBoundingClientRect().height) {
                    startIndex = visualData[0].index;
                }
                else {
                    startIndex = this.prevstartIndex === -1 ? 0 : this.prevstartIndex;
                }
            }
            pageingDetails.result = visualData.slice(startIndex, endIndex);
            this.prevstartIndex = startIndex;
            this.prevendIndex = endIndex;
        }
        this.parent.notify('updateAction', pageingDetails);
    };
    /**
     * To destroy the virtualScroll module
     *
     * @returns {void}
     * @hidden
     */
    VirtualScroll$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    return VirtualScroll$$1;
}());
var TreeVirtual = /** @class */ (function (_super) {
    __extends$14(TreeVirtual, _super);
    function TreeVirtual(parent, locator) {
        var _this = _super.call(this, parent, locator) || this;
        sf.base.getValue('parent', _this).off('initial-load', sf.base.getValue('instantiateRenderer', _this), _this);
        sf.base.getValue('parent', _this).on('initial-load', _this.instantiateRenderers, _this);
        return _this;
    }
    TreeVirtual.prototype.getModuleName = function () {
        return 'treeVirtualScroll';
    };
    TreeVirtual.prototype.instantiateRenderers = function () {
        sf.base.getValue('parent', this).log(['limitation', 'virtual_height'], 'virtualization');
        var renderer = sf.base.getValue('locator', this).getService('rendererFactory');
        sf.base.getValue('addRenderer', renderer)
            .apply(renderer, [sf.grids.RenderType.Content, new VirtualTreeContentRenderer(sf.base.getValue('parent', this), sf.base.getValue('locator', this))]);
        //renderer.addRenderer(RenderType.Content, new VirtualTreeContentRenderer(getValue('parent', this), getValue('locator', this)));
        this.ensurePageSize();
    };
    TreeVirtual.prototype.ensurePageSize = function () {
        var parentGrid = sf.base.getValue('parent', this);
        var rowHeight = parentGrid.getRowHeight();
        if (!sf.base.isNullOrUndefined(parentGrid.height) && typeof (parentGrid.height) === 'string' && parentGrid.height.indexOf('%') !== -1) {
            parentGrid.element.style.height = parentGrid.height;
        }
        var vHeight = parentGrid.height.toString().indexOf('%') < 0 ? parentGrid.height :
            parentGrid.element.getBoundingClientRect().height;
        var blockSize = ~~(vHeight / rowHeight);
        var height = blockSize * 2;
        var size = parentGrid.pageSettings.pageSize;
        parentGrid.setProperties({ pageSettings: { pageSize: size < height ? height : size } }, true);
    };
    return TreeVirtual;
}(sf.grids.VirtualScroll));

/**
 * TreeGrid Freeze module
 *
 * @hidden
 */
var Freeze$1 = /** @class */ (function () {
    /**
     * Constructor for render module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function Freeze$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.Freeze);
        this.parent = parent;
        this.addEventListener();
    }
    Freeze$$1.prototype.addEventListener = function () {
        this.parent.on('rowExpandCollapse', this.rowExpandCollapse, this);
        this.parent.on('dataBoundArg', this.dataBoundArg, this);
        this.parent.grid.on('dblclick', this.dblClickHandler, this);
    };
    Freeze$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('rowExpandCollapse', this.rowExpandCollapse);
        this.parent.off('dataBoundArg', this.dataBoundArg);
        this.parent.grid.off('dblclick', this.dblClickHandler);
    };
    Freeze$$1.prototype.rowExpandCollapse = function (args) {
        var movableRows = this.parent.getMovableDataRows();
        var frozenrows = this.parent.getRows();
        var rows;
        var frozenRightRows;
        var freeze = (this.parent.getFrozenLeftColumnsCount() > 0 || this.parent.getFrozenRightColumnsCount() > 0) ? true : false;
        if (freeze) {
            frozenRightRows = this.parent.getFrozenRightRows().filter(function (e) {
                return e.querySelector('.e-gridrowindex' + args.record.index + 'level' + (args.record.level + 1));
            });
        }
        if (!args.detailrows.length) {
            rows = movableRows.filter(function (e) {
                return e.querySelector('.e-gridrowindex' + args.record.index + 'level' + (args.record.level + 1));
            });
        }
        else {
            rows = args.detailrows;
        }
        for (var i = 0; i < rows.length; i++) {
            var rData = this.parent.grid.getRowObjectFromUID(rows[i].getAttribute('data-Uid')).data;
            rows[i].style.display = args.action;
            if (freeze) {
                frozenRightRows[i].style.display = args.action;
            }
            var queryselector = args.action === 'none' ? '.e-treecolumn-container .e-treegridcollapse'
                : '.e-treecolumn-container .e-treegridexpand';
            if (frozenrows[rows[i].rowIndex].querySelector(queryselector)) {
                var cRow = [];
                for (var i_1 = 0; i_1 < movableRows.length; i_1++) {
                    if (movableRows[i_1].querySelector('.e-gridrowindex' + rData.index + 'level' + (rData.level + 1))) {
                        cRow.push(movableRows[i_1]);
                    }
                }
                if (cRow.length) {
                    this.rowExpandCollapse({ detailrows: cRow, action: args.action });
                }
            }
        }
    };
    Freeze$$1.prototype.dblClickHandler = function (e) {
        if (sf.grids.parentsUntil(e.target, 'e-rowcell') &&
            this.parent.grid.editSettings.allowEditOnDblClick && this.parent.editSettings.mode !== 'Cell') {
            this.parent.grid.editModule.startEdit(sf.grids.parentsUntil(e.target, 'e-row'));
        }
    };
    Freeze$$1.prototype.dataBoundArg = function () {
        var checkboxColumn = this.parent.getColumns().filter(function (e) {
            return e.showCheckbox;
        });
        if (checkboxColumn.length && this.parent.freezeModule && this.parent.initialRender) {
            sf.base.addClass([this.parent.element.getElementsByClassName('e-grid')[0]], 'e-checkselection');
        }
    };
    Freeze$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns Freeze module name
     */
    Freeze$$1.prototype.getModuleName = function () {
        return 'freeze';
    };
    return Freeze$$1;
}());

/**
 * TreeGrid ColumnChooser module
 *
 * @hidden
 */
var ColumnChooser$1 = /** @class */ (function () {
    /**
     * Constructor for render module
     *
     * @param {TreeGrid} parent - Tree Grid instance.
     */
    function ColumnChooser$$1(parent) {
        sf.grids.Grid.Inject(sf.grids.ColumnChooser);
        this.parent = parent;
    }
    /**
     * Column chooser can be displayed on screen by given position(X and Y axis).
     *
     * @param  {number} X - Defines the X axis.
     * @param  {number} Y - Defines the Y axis.
     * @returns {void}
     */
    ColumnChooser$$1.prototype.openColumnChooser = function (X, Y) {
        return this.parent.grid.columnChooserModule.openColumnChooser(X, Y);
    };
    /**
     * Destroys the openColumnChooser.
     *
     * @function destroy
     * @returns {void}
     */
    ColumnChooser$$1.prototype.destroy = function () {
        //this.parent.grid.ColumnChooserModule.destroy();
    };
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} Returns ColumnChooser module name
     */
    ColumnChooser$$1.prototype.getModuleName = function () {
        return 'ColumnChooser';
    };
    return ColumnChooser$$1;
}());

/**
 * TreeGrid Infinite Scroll module will handle Infinite Scrolling.
 *
 * @hidden
 */
var InfiniteScroll$1 = /** @class */ (function () {
    /**
     * Constructor for VirtualScroll module
     *
     * @param {TreeGrid} parent - Tree Grid instance
     */
    function InfiniteScroll$$1(parent) {
        this.parent = parent;
        sf.grids.Grid.Inject(sf.grids.InfiniteScroll);
        this.addEventListener();
    }
    /**
     * For internal use only - Get the module name.
     *
     * @private
     * @returns {string} - Returns Logger module name
     */
    InfiniteScroll$$1.prototype.getModuleName = function () {
        return 'infiniteScroll';
    };
    /**
     * @hidden
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.addEventListener = function () {
        this.parent.on(pagingActions, this.infinitePageAction, this);
        this.parent.on('infinite-remote-expand', this.infiniteRemoteExpand, this);
        this.parent.grid.on('delete-complete', this.infiniteDeleteHandler, this);
        this.parent.grid.on('infinite-edit-handler', this.infiniteEditHandler, this);
        this.parent.grid.on('infinite-crud-cancel', this.createRows, this);
        this.parent.grid.on('content-ready', this.contentready, this);
    };
    /**
     * @hidden
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.removeEventListener = function () {
        if (this.parent.isDestroyed) {
            return;
        }
        this.parent.off('infinite-remote-expand', this.infiniteRemoteExpand);
        this.parent.grid.off('delete-complete', this.infiniteDeleteHandler);
        this.parent.grid.off('infinite-edit-handler', this.infiniteEditHandler);
        this.parent.off(pagingActions, this.infinitePageAction);
        this.parent.grid.off('infinite-crud-cancel', this.createRows);
        this.parent.grid.off('content-ready', this.contentready);
    };
    /**
     * Handles the Expand Collapse action for Remote data with infinite scrolling.
     *
     * @param {{ index: number, childData: ITreeData[] }} args - expanded row index and its child data
     * @param { number } args.index - expanded row index
     * @param { ITreeData[] } args.childData - child data of expanded row
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.infiniteRemoteExpand = function (args) {
        var rowObjects = this.parent.grid.getRowsObject();
        var locator = 'serviceLocator';
        var generateRows = 'generateRows';
        var serviceLocator = this.parent.grid.infiniteScrollModule[locator];
        var rowRenderer = new sf.grids.RowRenderer(serviceLocator, null, this.parent.grid);
        var rows = this.parent.getRows();
        var position = args.index === rows.length - 1 ? 'after' : 'before';
        var cols = this.parent.grid.getColumns();
        var childRowObjects = this.parent.grid.infiniteScrollModule[generateRows](args.childData, args);
        var childRowElements = [];
        for (var i = 0; i < childRowObjects.length; i++) {
            childRowElements.push(rowRenderer.render(childRowObjects[i], cols));
        }
        rowObjects.splice.apply(rowObjects, [args.index + 1, 0].concat(childRowObjects));
        for (var i = 0; i < childRowElements.length; i++) {
            if (position === 'after') {
                rows[args.index + i][position](childRowElements[i]);
            }
            else {
                rows[args.index + i + 1][position](childRowElements[i]);
            }
            rows.splice(args.index + 1 + i, 0, childRowElements[i]);
        }
        sf.grids.resetRowIndex(this.parent.grid, this.parent.grid.getRowsObject(), this.parent.grid.getRows(), 0);
    };
    /**
     * Resetted the row index for expand collapse action for cache support.
     *
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.contentready = function () {
        if (this.parent.infiniteScrollSettings.enableCache && !sf.base.isNullOrUndefined(this.parent.editModule)) {
            var updateIndex = 'updateIndex';
            this.parent.editModule[updateIndex](this.parent.grid.dataSource, this.parent.getRows(), this.parent.getCurrentViewRecords());
            if (this.parent.getFrozenColumns()) {
                this.parent.editModule[updateIndex](this.parent.grid.dataSource, this.parent.getMovableDataRows(), this.parent.getCurrentViewRecords());
            }
        }
    };
    /**
     * Handles the page query for Data operations and CRUD actions.
     *
     * @param {{ result: ITreeData[], count: number, actionArgs: object }} pageingDetails - data, its count and action details
     * @param {ITreeData[]} pageingDetails.result - data on scroll action
     * @param {number} pageingDetails.count - data count on scroll action
     * @param {Object} pageingDetails.actionArgs - scroll action details
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.infinitePageAction = function (pageingDetails) {
        var dm = new sf.data.DataManager(pageingDetails.result);
        var expanded$$1 = new sf.data.Predicate('expanded', 'notequal', null).or('expanded', 'notequal', undefined);
        var visualData = dm.executeLocal(new sf.data.Query().where(expanded$$1));
        var actionArgs = sf.base.getValue('actionArgs', pageingDetails.actionArgs);
        var actions = sf.base.getValue('actions', this.parent.grid.infiniteScrollModule);
        var initial = actions.some(function (value) { return value === actionArgs.requestType; });
        var initialRender = initial ? true : this.parent.initialRender ? true : false;
        this.visualData = visualData;
        pageingDetails.count = visualData.length;
        if (sf.base.getValue('isPrinting', pageingDetails.actionArgs)) {
            pageingDetails.result = visualData;
        }
        else {
            var query = new sf.data.Query();
            var isCache = this.parent.infiniteScrollSettings.enableCache;
            if (isCache && this.parent.infiniteScrollSettings.initialBlocks > this.parent.infiniteScrollSettings.maxBlocks) {
                this.parent.infiniteScrollSettings.initialBlocks = this.parent.infiniteScrollSettings.maxBlocks;
            }
            var size = initialRender ?
                this.parent.pageSettings.pageSize * this.parent.infiniteScrollSettings.initialBlocks :
                this.parent.pageSettings.pageSize;
            var current = this.parent.grid.pageSettings.currentPage;
            if (!sf.base.isNullOrUndefined(actionArgs)) {
                var lastIndex = sf.base.getValue('lastIndex', this.parent.grid.infiniteScrollModule);
                var firstIndex = sf.base.getValue('firstIndex', this.parent.grid.infiniteScrollModule);
                if (!isCache && actionArgs.requestType === 'delete') {
                    var skip = lastIndex - actionArgs.data.length + 1;
                    var take = actionArgs.data.length;
                    query = query.skip(skip).take(take);
                }
                else if (isCache && actionArgs.requestType === 'delete' ||
                    (actionArgs.requestType === 'save' && actionArgs.action === 'add')) {
                    query = query.skip(firstIndex);
                    query = query.take(this.parent.infiniteScrollSettings.initialBlocks * this.parent.pageSettings.pageSize);
                }
                else {
                    query = query.page(current, size);
                }
            }
            else {
                query = query.page(current, size);
            }
            dm.dataSource.json = visualData;
            if (!isCache && !sf.base.isNullOrUndefined(actionArgs) && actionArgs.requestType === 'save' && actionArgs.action === 'add') {
                pageingDetails.result = [actionArgs.data];
            }
            else {
                pageingDetails.result = dm.executeLocal(query);
            }
        }
        this.parent.notify('updateAction', pageingDetails);
    };
    /**
     * Handles the currentviewdata for delete operation.
     *
     * @param {{ e: InfiniteScrollArgs, result: Object[] }} args - Scroller and data details
     * @param {InfiniteScrollArgs} args.e -  scroller details while CRUD
     * @param {Object[]} args.result - data details while CRUD
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.infiniteEditHandler = function (args) {
        var infiniteData = 'infiniteCurrentViewData';
        var infiniteCurrentViewData = this.parent.grid.infiniteScrollModule[infiniteData];
        var keys = Object.keys(infiniteCurrentViewData);
        if (args.e.requestType === 'delete' && args.result.length > 1) {
            for (var i = 1; i < args.result.length; i++) {
                infiniteCurrentViewData[keys[keys.length - 1]].push(args.result[i]);
            }
        }
    };
    /**
     * Handles the row objects for delete operation.
     *
     * @param {ActionEventArgs} args - crud action details
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.infiniteDeleteHandler = function (args) {
        if (args.requestType === 'delete') {
            var rows = this.parent.grid.getRowsObject();
            var rowElms = this.parent.getRows();
            var data = args.data instanceof Array ? args.data : [args.data];
            var keyField = this.parent.grid.getPrimaryKeyFieldNames()[0];
            this.removeRows(rowElms, rows, data, keyField, true);
            if (this.parent.getFrozenColumns() > 0) {
                var mRows = this.parent.grid.getMovableRowsObject();
                var mRowElms = this.parent.grid.getMovableRows();
                this.removeRows(mRowElms, mRows, data, keyField);
            }
        }
    };
    /**
     * Handles the row objects for delete operation.
     *
     * @param {Element[]} rowElms - row elements
     * @param {Row<Column>[]} rows - Row object collection
     * @param {Object[]} data - data collection
     * @param {string} keyField - primary key name
     * @param { boolean} isFrozen - Specifies whether frozen column enabled
     * @returns {void}
     */
    InfiniteScroll$$1.prototype.removeRows = function (rowElms, rows, data, keyField, isFrozen) {
        var _this = this;
        var resetInfiniteCurrentViewData = 'resetInfiniteCurrentViewData';
        var _loop_1 = function (i) {
            rows.filter(function (e, index) {
                if (e.data[keyField] === data[i][keyField]) {
                    if (isFrozen) {
                        var page = Math.ceil((index + 1) / _this.parent.grid.pageSettings.pageSize);
                        _this.parent.grid.infiniteScrollModule[resetInfiniteCurrentViewData](page, index);
                    }
                    rows.splice(index, 1);
                    sf.base.remove(rowElms[index]);
                    rowElms.splice(index, 1);
                }
            });
        };
        for (var i = 0; i < data.length; i++) {
            _loop_1(i);
        }
    };
    /**
     * Handles the row objects for Add operation.
     */
    InfiniteScroll$$1.prototype.createRows = function (eventArgs) {
        var locator = 'serviceLocator';
        var actionArgs = eventArgs.args.e;
        var row = eventArgs.row;
        var serviceLocator = this.parent.grid.infiniteScrollModule[locator];
        var rowRenderer = new sf.grids.RowRenderer(serviceLocator, null, this.parent.grid);
        var tbody;
        var currentData = this.parent.getCurrentViewRecords();
        var currentRows = eventArgs.isMovable ? this.parent.grid.getMovableRows()
            : this.parent.grid.getDataRows();
        if (eventArgs.isFrozenRight) {
            tbody = this.parent.element.querySelector('.e-frozen-right-content').querySelector('tbody');
        }
        else {
            tbody = !this.parent.grid.isFrozenGrid() ? this.parent.getContent().querySelector('tbody') : eventArgs.isMovable
                ? this.parent.grid.getMovableVirtualContent().querySelector('tbody')
                : this.parent.grid.getFrozenVirtualContent().querySelector('tbody');
        }
        if (this.parent.frozenRows) {
            tbody = eventArgs.isFrozenRows && this.parent.grid.infiniteScrollModule.requestType !== 'add'
                || !eventArgs.isFrozenRows && this.parent.grid.infiniteScrollModule.requestType === 'add'
                ? !this.parent.grid.isFrozenGrid() ? this.parent.getHeaderContent().querySelector('tbody')
                    : eventArgs.isMovable ? this.parent.grid.getMovableVirtualHeader().querySelector('tbody')
                        : eventArgs.isFrozenRight ? this.parent.element.querySelector('.e-frozen-right-header').querySelector('tbody')
                            : this.parent.grid.getFrozenVirtualHeader().querySelector('tbody') : tbody;
        }
        var position;
        var addRowIndex = 'addRowIndex';
        var newRowIndex = this.parent.editModule[addRowIndex];
        for (var i = 0; i < row.length; i++) {
            var newRow = rowRenderer.render(row[i], this.parent.grid.getColumns());
            if (actionArgs.requestType === 'save' && actionArgs.action === 'add') {
                if (sf.base.getValue('selectedIndex', this.parent.editModule) !== -1 && this.parent.editSettings.newRowPosition !== 'Top') {
                    if (this.parent.editSettings.newRowPosition === 'Below' || this.parent.editSettings.newRowPosition === 'Child') {
                        position = 'after';
                        newRowIndex += findChildrenRecords(currentData[newRowIndex + 1]).length;
                        if (this.parent.editSettings.newRowPosition === 'Child') {
                            newRowIndex -= 1; //// for child position already child record is added in childRecords so subtracting 1
                        }
                        currentRows[newRowIndex][position](newRow);
                    }
                    else if (this.parent.editSettings.newRowPosition === 'Above') {
                        position = 'before';
                        currentRows[this.parent.editModule[addRowIndex]][position](newRow);
                    }
                }
                else if (this.parent.editSettings.newRowPosition === 'Bottom') {
                    tbody.appendChild(newRow);
                }
                else {
                    tbody.insertBefore(newRow, tbody.firstElementChild);
                }
            }
            else if (actionArgs.requestType === 'delete') {
                tbody.appendChild(newRow);
            }
        }
        eventArgs.cancel = true;
    };
    /**
     * To destroy the infiniteScroll module
     *
     * @returns {void}
     * @hidden
     */
    InfiniteScroll$$1.prototype.destroy = function () {
        this.removeEventListener();
    };
    return InfiniteScroll$$1;
}());

/**
 * actions export
 */

/**
 * TreeGrid component exported items
 */

/**
 * Export TreeGrid component
 */

TreeGrid.Inject(Filter$1, Page$1, Sort$1, Reorder$1, Toolbar$1, Aggregate$1, Resize$1, ColumnMenu$1, ExcelExport$1, PdfExport$1, CommandColumn$1, ContextMenu$1, Edit$1, Selection, VirtualScroll$1, DetailRow$1, RowDD$1, Freeze$1, ColumnChooser$1, Logger$1, InfiniteScroll$1);

exports.TreeGrid = TreeGrid;
exports.load = load;
exports.rowDataBound = rowDataBound;
exports.dataBound = dataBound;
exports.queryCellInfo = queryCellInfo;
exports.beforeDataBound = beforeDataBound;
exports.actionBegin = actionBegin;
exports.dataStateChange = dataStateChange;
exports.actionComplete = actionComplete;
exports.rowSelecting = rowSelecting;
exports.rowSelected = rowSelected;
exports.checkboxChange = checkboxChange;
exports.rowDeselected = rowDeselected;
exports.toolbarClick = toolbarClick;
exports.beforeExcelExport = beforeExcelExport;
exports.beforePdfExport = beforePdfExport;
exports.resizeStop = resizeStop;
exports.expanded = expanded;
exports.expanding = expanding;
exports.collapsed = collapsed;
exports.collapsing = collapsing;
exports.remoteExpand = remoteExpand;
exports.localPagedExpandCollapse = localPagedExpandCollapse;
exports.pagingActions = pagingActions;
exports.printGridInit = printGridInit;
exports.contextMenuOpen = contextMenuOpen;
exports.contextMenuClick = contextMenuClick;
exports.beforeCopy = beforeCopy;
exports.beforePaste = beforePaste;
exports.savePreviousRowPosition = savePreviousRowPosition;
exports.crudAction = crudAction;
exports.beginEdit = beginEdit;
exports.beginAdd = beginAdd;
exports.recordDoubleClick = recordDoubleClick;
exports.cellSave = cellSave;
exports.cellSaved = cellSaved;
exports.cellEdit = cellEdit;
exports.batchDelete = batchDelete;
exports.batchCancel = batchCancel;
exports.batchAdd = batchAdd;
exports.beforeBatchDelete = beforeBatchDelete;
exports.beforeBatchAdd = beforeBatchAdd;
exports.beforeBatchSave = beforeBatchSave;
exports.batchSave = batchSave;
exports.keyPressed = keyPressed;
exports.updateData = updateData;
exports.doubleTap = doubleTap;
exports.virtualColumnIndex = virtualColumnIndex;
exports.virtualActionArgs = virtualActionArgs;
exports.dataListener = dataListener;
exports.indexModifier = indexModifier;
exports.beforeStartEdit = beforeStartEdit;
exports.beforeBatchCancel = beforeBatchCancel;
exports.batchEditFormRendered = batchEditFormRendered;
exports.detailDataBound = detailDataBound;
exports.rowDrag = rowDrag;
exports.rowDragStartHelper = rowDragStartHelper;
exports.rowDrop = rowDrop;
exports.rowDragStart = rowDragStart;
exports.rowsAdd = rowsAdd;
exports.rowsRemove = rowsRemove;
exports.rowdraging = rowdraging;
exports.rowDropped = rowDropped;
exports.DataManipulation = DataManipulation;
exports.Reorder = Reorder$1;
exports.Resize = Resize$1;
exports.RowDD = RowDD$1;
exports.Column = Column;
exports.EditSettings = EditSettings;
exports.Predicate = Predicate$1;
exports.FilterSettings = FilterSettings;
exports.PageSettings = PageSettings;
exports.SearchSettings = SearchSettings;
exports.SelectionSettings = SelectionSettings;
exports.AggregateColumn = AggregateColumn;
exports.AggregateRow = AggregateRow;
exports.SortDescriptor = SortDescriptor;
exports.SortSettings = SortSettings;
exports.RowDropSettings = RowDropSettings$1;
exports.InfiniteScrollSettings = InfiniteScrollSettings;
exports.Render = Render;
exports.TreeVirtualRowModelGenerator = TreeVirtualRowModelGenerator;
exports.isRemoteData = isRemoteData;
exports.isCountRequired = isCountRequired;
exports.isCheckboxcolumn = isCheckboxcolumn;
exports.isFilterChildHierarchy = isFilterChildHierarchy;
exports.findParentRecords = findParentRecords;
exports.getExpandStatus = getExpandStatus;
exports.findChildrenRecords = findChildrenRecords;
exports.isOffline = isOffline;
exports.extendArray = extendArray;
exports.getPlainData = getPlainData;
exports.getParentData = getParentData;
exports.isHidden = isHidden;
exports.Filter = Filter$1;
exports.ExcelExport = ExcelExport$1;
exports.PdfExport = PdfExport$1;
exports.Page = Page$1;
exports.Toolbar = Toolbar$1;
exports.Aggregate = Aggregate$1;
exports.Sort = Sort$1;
exports.TreeClipboard = TreeClipboard;
exports.ColumnMenu = ColumnMenu$1;
exports.ContextMenu = ContextMenu$1;
exports.Edit = Edit$1;
exports.CommandColumn = CommandColumn$1;
exports.Selection = Selection;
exports.DetailRow = DetailRow$1;
exports.VirtualScroll = VirtualScroll$1;
exports.TreeVirtual = TreeVirtual;
exports.Freeze = Freeze$1;
exports.ColumnChooser = ColumnChooser$1;
exports.Logger = Logger$1;
exports.treeGridDetails = treeGridDetails;
exports.InfiniteScroll = InfiniteScroll$1;

return exports;

});

    sf.treegrid = sf.treegrid({});

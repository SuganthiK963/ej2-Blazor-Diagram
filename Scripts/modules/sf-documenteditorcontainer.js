window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.DocumentEditorContainer = (function () {
'use strict';

/**
 * Document Editor container component.
 */
var SfDocumentEditorContainer = /** @class */ (function () {
    /**
     * Gets toolbar instance.
     * @blazorType Toolbar
     */
    // public get toolbar(): Toolbar {
    //     return this.toolbarModule;
    // }
    /**
     * Initialize the constructor of DocumentEditorContainer
     */
    function SfDocumentEditorContainer(element, options, editorOptions, dotnetRef) {
        /**
         * @private
         */
        this.previousContext = '';
        /**
         * @private
         */
        // public characterFormat: CharacterFormatProperties;
        /**
         * @private
         */
        // public paragraphFormat: ParagraphFormatProperties;
        // /**
        //  * @private
        //  */
        // public sectionFormat: SectionFormatProperties;
        /**
         * @private
         */
        this.showHeaderProperties = true;
        // debugger;        
        this.element = element;
        if (sf.base.isNullOrUndefined(element)) {
            return;
        }
        if (!sf.base.isNullOrUndefined(element)) {
            this.element.blazor__instance = this;
        }
        this.dotNetRef = dotnetRef;
        this.element = element;
        this.options = options;
        this.editorOptions = editorOptions;
        this.updateOptions(options);
    }
    Object.defineProperty(SfDocumentEditorContainer.prototype, "documentEditor", {
        /**
         * Gets DocumentEditor instance.
         * @aspType DocumentEditor
         * @blazorType DocumentEditor
         */
        get: function () {
            return this.documentEditorInternal;
        },
        enumerable: true,
        configurable: true
    });
    SfDocumentEditorContainer.prototype.updateOptions = function (options) {
        // debugger;
        this.showPropertiesPane = options.showPropertiesPane;
        this.currentUser = options.currentUser;
        this.enableComment = options.enableComment;
        this.enableLocalPaste = options.enableLocalPaste;
        this.enableSpellCheck = options.enableSpellCheck;
        this.enableTrackChanges = options.enableTrackChanges;
        // this.headers = options.headers;
        this.height = options.height;
        this.serviceUrl = options.serviceUrl;
        this.userColor = options.userColor;
        this.width = options.width;
        this.zIndex = options.zIndex;
        // this.layoutType = options.layoutType;
        this.requiredModules();
    };
    SfDocumentEditorContainer.prototype.setOptions = function (newOptions, options) {
        for (var _i = 0, _a = Object.keys(newOptions); _i < _a.length; _i++) {
            var prop = _a[_i];
            var value = newOptions[prop];
            if (!sf.base.isNullOrUndefined(value)) {
                switch (prop) {
                    case "zoomFactor":
                        this.documentEditor.zoomFactor = value;
                        this.documentEditor.onPropertyChanged({ "zoomFactor": value }, null);
                        break;
                    case "layoutType":
                        // this.layoutType = value;
                        this.documentEditor.layoutType = value;
                        this.documentEditor.onPropertyChanged({ "layoutType": value }, null);
                        break;
                    case "enableTrackChanges":
                        this.enableTrackChanges = value;
                        this.documentEditor.enableTrackChanges = value;
                        this.documentEditor.showRevisions = value;
                        this.documentEditor.onPropertyChanged({ "enableTrackChanges": value }, null);
                        this.documentEditor.onPropertyChanged({ "showRevisions": value }, null);
                        break;
                    case "restrictEditing":
                        this.restrictEditing = value;
                        this.documentEditor.isReadOnly = value;
                        this.documentEditor.onPropertyChanged({ "isReadOnly": value }, null);
                        break;
                    case "enableLocalPaste":
                        this.enableLocalPaste = value;
                        this.documentEditor.enableLocalPaste = value;
                        this.documentEditor.onPropertyChanged({ "enableLocalPaste": value }, null);
                        break;
                }
            }
        }
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.getModuleName = function () {
        return 'DocumentEditorContainer';
    };
    /**
     * @private
     */
    // tslint:disable:max-func-body-length
    SfDocumentEditorContainer.prototype.onPropertyChanged = function (newModel, oldModel) {
        for (var _i = 0, _a = Object.keys(newModel); _i < _a.length; _i++) {
            var prop = _a[_i];
            switch (prop) {
                case 'restrictEditing':
                    this.documentEditor.isReadOnly = newModel.restrictEditing;
                    break;
                case 'showPropertiesPane':
                    this.showHidePropertiesPane(newModel.showPropertiesPane);
                    break;
                case 'enableTrackChanges':
                    if (this.documentEditor) {
                        this.documentEditor.enableTrackChanges = newModel.enableTrackChanges;
                        this.documentEditor.showRevisions = newModel.enableTrackChanges;
                        this.documentEditor.resize();
                    }
                    break;
                case 'enableLocalPaste':
                    if (this.documentEditor) {
                        this.documentEditor.enableLocalPaste = newModel.enableLocalPaste;
                    }
                    break;
                case 'serviceUrl':
                    if (this.documentEditor) {
                        this.documentEditor.serviceUrl = newModel.serviceUrl;
                    }
                    break;
                case 'serverActionSettings':
                    if (this.documentEditor) {
                        this.setserverActionSettings();
                    }
                    break;
                case 'zIndex':
                    if (this.documentEditor) {
                        this.documentEditor.zIndex = newModel.zIndex;
                    }
                    break;
                case 'headers':
                    // if (this.documentEditor) {
                    //     this.documentEditor.headers = newModel.headers;
                    // }
                    break;
                case 'locale':
                case 'enableRtl':
                    // this.refresh();
                    break;
                case 'enableComment':
                    if (this.documentEditor) {
                        this.documentEditor.enableComment = newModel.enableComment;
                    }
                    // if (this.toolbarModule) {
                    //     this.toolbarModule.enableDisableInsertComment(newModel.enableComment);
                    // }
                    break;
                case 'enableSpellCheck':
                    if (this.documentEditor) {
                        this.documentEditor.enableSpellCheck = newModel.enableSpellCheck;
                    }
                    break;
                case 'documentEditorSettings':
                    if (this.documentEditor) {
                        this.customizeDocumentEditorSettings();
                    }
                    break;
                case 'toolbarItems':
                    // if (this.toolbarModule) {
                    //     this.toolbarModule.reInitToolbarItems(newModel.toolbarItems);
                    // }
                    break;
                case 'currentUser':
                    if (this.documentEditor) {
                        this.documentEditor.currentUser = newModel.currentUser;
                    }
                    break;
                case 'userColor':
                    if (this.documentEditor) {
                        this.documentEditor.userColor = newModel.userColor;
                    }
                    break;
                case 'layoutType':
                    if (this.documentEditor) {
                        this.documentEditor.layoutType = newModel.layoutType;
                    }
                    break;
                case 'enableToolbar':
                    if (this.documentEditor) {
                        this.documentEditor.resize();
                    }
                    break;
                case 'height':
                    var calcHeight = '';
                    if (this.height.indexOf('%') !== -1) {
                        calcHeight = (window.innerHeight * (parseInt(this.height) / 100)) + 'px';
                    }
                    else {
                        calcHeight = sf.base.formatUnit(this.height);
                    }
                    this.element.style.height = calcHeight;
                    if (this.documentEditor) {
                        this.documentEditor.resize();
                    }
                    break;
                case 'width':
                    this.element.style.width = sf.base.formatUnit(this.width);
                    if (this.documentEditor) {
                        this.documentEditor.resize();
                    }
                    break;
            }
        }
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.preRender = function (element) {
        var calcHeight = '';
        if (this.height.indexOf('%') !== -1) {
            calcHeight = (window.innerHeight * (parseInt(this.height) / 100)) + 'px';
        }
        else {
            calcHeight = sf.base.formatUnit(this.height);
        }
        this.element.style.height = calcHeight;
        if (this.width !== '') {
            this.element.style.width = sf.base.formatUnit(this.width);
        }
        this.element.style.minHeight = '320px';
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.render = function (element) {
        this.initializeDocumentEditor(element);
        this.setserverActionSettings();
        this.customizeDocumentEditorSettings();
    };
    SfDocumentEditorContainer.prototype.setFormat = function () {
    };
    SfDocumentEditorContainer.prototype.setserverActionSettings = function () {
    };
    SfDocumentEditorContainer.prototype.customizeDocumentEditorSettings = function () {
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.getPersistData = function () {
        return 'documenteditor-container';
    };
    //tslint:disable: max-func-body-length
    SfDocumentEditorContainer.prototype.requiredModules = function () {
        var modules = [];
        if (this.enableToolbar) {
            modules.push({
                member: 'toolbar', args: [this]
            });
        }
        return modules;
    };
    SfDocumentEditorContainer.prototype.initializeDocumentEditor = function (element) {
        var documentEditorTarget = element;
        this.documentEditorInternal = element.blazor__instance;
        //(this.element as HTMLElement).appendChild(documentEditorTarget);
        //this.documentEditorInternal = new SfDocumentEditor(documentEditorTarget, this.dotNetRef, this.editorOptions);
        sf.base.EventHandler.add(documentEditorTarget, 'selectionChange', this.onSelectionChange, this);
        this.documentEditor.enableAllModules();
        this.documentEditor.enableComment = this.enableComment;
        // this.editorContainer.insertBefore(documentEditorTarget, this.editorContainer.firstChild);
        this.setFormat();
        // this.documentEditor.appendTo(documentEditorTarget);
        this.documentEditor.resize();
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.showHidePropertiesPane = function (show) {
        if (this.showPropertiesPane) {
            this.showPropertiesPaneOnSelection();
        }
        //this.propertiesPaneContainer.style.display = show ? 'block' : 'none';
        // if (this.toolbarModule) {
        //     this.toolbarModule.propertiesPaneButton.element.style.opacity = show ? '1' : '0.5';
        // }
        this.documentEditor.resize();
    };
    /**
     * Resizes the container component and its sub elements based on given size or client size.
     * @param width
     * @param height
     */
    SfDocumentEditorContainer.prototype.resize = function (width, height) {
        //Since we have syncfusion licensing element wrapped around the container element we have taken element.parentElement.paraentElement
        if (this.element) {
            if (sf.base.isNullOrUndefined(height) && this.element && this.element.parentElement.parentElement) {
                height = this.element.parentElement.parentElement.clientHeight;
            }
            if (sf.base.isNullOrUndefined(width) && this.element && this.element.parentElement.parentElement) {
                width = this.element.parentElement.parentElement.clientWidth;
            }
            if (!sf.base.isNullOrUndefined(width) && width > 200) {
                this.width = width.toString();
                this.element.style.width = width + 'px';
            }
            if (!sf.base.isNullOrUndefined(height) && height > 200) {
                this.height = height.toString();
                this.element.style.height = height + 'px';
            }
            if (this.documentEditor) {
                this.documentEditor.resize();
            }
        }
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.onContentChange = function () {
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.onDocumentChange = function () {
        this.enableTrackChanges = this.documentEditor.enableTrackChanges;
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.onSelectionChange = function () {
        this.showPropertiesPaneOnSelection();
    };
    /**
     * @private
     */
    SfDocumentEditorContainer.prototype.showPropertiesPaneOnSelection = function () {
        var currentContext = this.documentEditor.selection.contextType;
        var isInHeaderFooter = currentContext.indexOf('Header') >= 0
            || currentContext.indexOf('Footer') >= 0;
        if (!isInHeaderFooter) {
            this.showHeaderProperties = true;
        }
        if (!this.showPropertiesPane) {
            this.showHidePropertiesPane(false);
            // this.propertiesPaneContainer.style.display = 'none';
        }
        else {
            //this.propertiesPaneContainer.style.display = 'block';
            if (isInHeaderFooter && this.showHeaderProperties) {
                this.showProperties('headerfooter');
            }
            else if (currentContext.indexOf('List') >= 0 || currentContext.indexOf('Text') >= 0
                && currentContext.indexOf('Table') < 0) {
                this.showProperties('text');
            }
            else if (currentContext.indexOf('Image') >= 0) {
                this.showProperties('image');
            }
            else if (currentContext.indexOf('TableOfContents') >= 0) {
                this.showProperties('toc');
            }
            else if (currentContext.indexOf('Table') >= 0) {
                this.showProperties('table');
            }
        }
        this.previousContext = this.documentEditor.selection.contextType;
    };
    /**
     * @private
     * @param property
     */
    SfDocumentEditorContainer.prototype.showProperties = function (property) {
        this.textProperties.showTextProperties(property === 'text');
        this.tableProperties.showTableProperties(property === 'table');
        this.imageProperties.showImageProperties(property === 'image');
        this.headerFooterProperties.showHeaderFooterPane(property === 'headerfooter');
        this.tocProperties.showTocPane(property === 'toc');
    };
    /**
     * Destroys all managed resources used by this object.
     */
    SfDocumentEditorContainer.prototype.destroy = function () {
        // super.destroy();
        // if (this.element) {
        //     this.element.classList.remove('e-documenteditorcontainer');
        //     this.element.innerHTML = '';
        // }
        // if (!this.refreshing) {
        //     this.element = undefined;
        // }
        // if (this.toolbarContainer && this.toolbarContainer.parentElement) {
        //     this.toolbarContainer.innerHTML = '';
        //     this.toolbarContainer.parentElement.removeChild(this.toolbarContainer);
        // }
        // this.toolbarContainer = undefined;
        // if (this.documentEditorInternal) {
        //     this.documentEditorInternal.destroy();
        // }
        // this.documentEditorInternal = undefined;
        // if (this.textProperties) {
        //     this.textProperties.destroy();
        // }
        // this.textProperties = undefined;
        // if (this.headerFooterProperties) {
        //     this.headerFooterProperties.destroy();
        // }
        // this.headerFooterProperties = undefined;
        // if (this.imageProperties) {
        //     this.imageProperties.destroy();
        // }
        // this.imageProperties = undefined;
        // if (this.tocProperties) {
        //     this.tocProperties.destroy();
        // }
        // this.tocProperties = undefined;
        // if (this.tableProperties) {
        //     this.tableProperties.destroy();
        // }
        // this.tableProperties = undefined;
        // if (this.statusBar) {
        //     this.statusBar.destroy();
        // }
        if (this.propertiesPaneContainer && this.editorContainer.parentElement) {
            this.propertiesPaneContainer.innerHTML = '';
            this.propertiesPaneContainer.parentElement.removeChild(this.propertiesPaneContainer);
        }
        this.propertiesPaneContainer = undefined;
        if (this.editorContainer && this.editorContainer.parentElement) {
            this.editorContainer.innerHTML = '';
            this.editorContainer.parentElement.removeChild(this.editorContainer);
        }
        if (this.statusBarElement && this.statusBarElement.parentElement) {
            this.statusBarElement.innerHTML = '';
            this.statusBarElement.parentElement.removeChild(this.statusBarElement);
        }
        if (this.containerTarget && this.containerTarget.parentElement) {
            this.containerTarget.innerHTML = '';
            this.containerTarget.parentElement.removeChild(this.containerTarget);
        }
        this.containerTarget = undefined;
        this.statusBarElement = undefined;
        this.editorContainer = undefined;
    };
    return SfDocumentEditorContainer;
}());

/**
 * Text Properties pane
 * @private
 */
var TextProperties = /** @class */ (function () {
    function TextProperties(ele) {
        var _this = this;
        this.showTextProperties = function (isShow) {
            // debugger;
            if (!isShow && _this.element.style.display === 'none' || (isShow && _this.element.style.display === 'block')) {
                return;
            }
            _this.element.style.display = isShow ? 'block' : 'none';
            // this.documentEditor.resize();
        };
        // debugger;
        this.element = ele;
    }
    return TextProperties;
}());

/**
 * Image Properties pane
 * @private
 */
var ImageProperties = /** @class */ (function () {
    function ImageProperties(ele) {
        var _this = this;
        this.showImageProperties = function (isShow) {
            if (!isShow && _this.element.style.display === 'none' || (isShow && _this.element.style.display === 'block')) {
                return;
            }
            _this.element.style.display = isShow ? 'block' : 'none';
            // this.documentEditor.resize();
        };
        this.element = ele;
    }
    return ImageProperties;
}());

/**
 * Table Properties pane
 * @private
 */
var TableProperties = /** @class */ (function () {
    function TableProperties(ele) {
        var _this = this;
        this.showTableProperties = function (isShow) {
            if (!isShow && _this.element.style.display === 'none' || (isShow && _this.element.style.display === 'block')) {
                return;
            }
            _this.element.style.display = isShow ? 'block' : 'none';
            // this.documentEditor.resize();
            // this.prevContext = this.documentEditor.selection.contextType;
        };
        this.element = ele;
    }
    return TableProperties;
}());

/**
 * TOC Properties pane
 * @private
 */
var TocProperties = /** @class */ (function () {
    function TocProperties(ele) {
        var _this = this;
        this.showTocPane = function (isShow) {
            if (!isShow && _this.element.style.display === 'none' || (isShow && _this.element.style.display === 'block')) {
                return;
            }
            _this.element.style.display = isShow ? 'block' : 'none';
            // tslint:disable-next-line:max-line-length
            // this.updateBtn.content = this.documentEditor.selection.contextType === 'TableOfContents' ? this.localObj.getConstant('Update') : this.localObj.getConstant('Insert');
            // this.prevContext = this.documentEditor.selection.contextType;
            // this.documentEditor.resize();
            // if (isShow) {
            //     this.updateBtn.element.focus();
            // }
        };
        this.element = ele;
    }
    return TocProperties;
}());

/**
 * Header Footer Properties pane
 * @private
 */
var HeaderFooterProperties = /** @class */ (function () {
    function HeaderFooterProperties(ele) {
        // debugger;
        this.element = ele;
    }
    HeaderFooterProperties.prototype.onSelectionChange = function () {
        //TODO: Bind the values to the UI elements.
        // this.headerFromTop.value = this.documentEditor.selection.sectionFormat.headerDistance;
        // this.footerFromTop.value = this.documentEditor.selection.sectionFormat.footerDistance;
        // if (this.documentEditor.selection.sectionFormat.differentFirstPage) {
        //     this.firstPage.checked = true;
        // } else {
        //     this.firstPage.checked = false;
        // }
        // if (this.documentEditor.selection.sectionFormat.differentOddAndEvenPages) {
        //     this.oddOrEven.checked = true;
        // } else {
        //     this.oddOrEven.checked = false;
        // }
    };
    HeaderFooterProperties.prototype.showHeaderFooterPane = function (isShow) {
        if (isShow) {
            // if (this.toolbar) {
            // this.toolbar.enableDisablePropertyPaneButton(false);
            // }
            this.onSelectionChange();
        }
        if (!isShow && this.element.style.display === 'none' || (isShow && this.element.style.display === 'block')) {
            return;
        }
        this.element.style.display = isShow ? 'block' : 'none';
        // this.documentEditor.resize();
    };
    return HeaderFooterProperties;
}());

var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (undefined && undefined.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var DocumentEditorContainer = {
    dotnetRef: null,
    filePicker: null,
    imagePicker: null,
    docEditElement: null,
    initialize: function (element, docEditElement, containerOptions, editorOptions, dotnetRef, elements) {
        // debugger
        sf.base.enableBlazorMode();
        var docEdit = new SfDocumentEditorContainer(element, containerOptions, editorOptions, dotnetRef);
        docEdit.textProperties = new TextProperties(elements[0]);
        docEdit.tableProperties = new TableProperties(elements[1]);
        docEdit.imageProperties = new ImageProperties(elements[2]);
        docEdit.headerFooterProperties = new HeaderFooterProperties(elements[3]);
        docEdit.tocProperties = new TocProperties(elements[4]);
        docEdit.preRender(docEditElement);
        docEdit.render(docEditElement);
        this.dotnetRef = dotnetRef;
        this.docEditElement = docEditElement;
        if (docEditElement.parentElement) {
            var filePickerElement = docEditElement.parentElement.getElementsByClassName('e-de-ctnr-file-picker');
            var imagePicker = docEditElement.parentElement.getElementsByClassName('e-de-ctnr-image-picker');
            if (filePickerElement.length > 0) {
                this.filePicker = filePickerElement[0];
                this.filePicker.addEventListener('change', this.fileReader.bind(this));
            }
            if (imagePicker.length > 0) {
                this.imagePicker = imagePicker[0];
                this.imagePicker.addEventListener('change', this.imageReader.bind(this));
            }
        }
    },
    updateOptions: function (element, options, dotnetRef) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            var instance = element.blazor__instance;
            instance.setOptions(options, instance.options);
            instance.options = options;
        }
    },
    imageReader: function () {
        return __awaiter(this, void 0, void 0, function () {
            var obj, file, fileReader;
            return __generator(this, function (_a) {
                obj = this;
                file = obj.imagePicker.files[0];
                fileReader = new FileReader();
                fileReader.onload = function () {
                    var data = fileReader.result;
                    var image = document.createElement('img');
                    image.addEventListener('load', function () {
                        obj.docEditElement.blazor__instance.editor.insertImage(data, this.width, this.height);
                    });
                    image.src = data;
                };
                fileReader.readAsDataURL(file);
                return [2 /*return*/];
            });
        });
    },
    fileReader: function () {
        return __awaiter(this, void 0, void 0, function () {
            var obj, file, documentName_1, format, fileReader_1;
            return __generator(this, function (_a) {
                this.dotnetRef.invokeMethodAsync('ShowSpinner');
                obj = this;
                file = obj.filePicker.files[0];
                if (file) {
                    documentName_1 = file.name;
                    format = documentName_1.substr(documentName_1.lastIndexOf('.'));
                    fileReader_1 = new FileReader();
                    fileReader_1.onload = function () {
                        var content = fileReader_1.result;
                        obj.dotnetRef.invokeMethodAsync('LoadDocument', content, documentName_1);
                    };
                    if (format === '.sfdt') {
                        fileReader_1.readAsText(file);
                    }
                    else {
                        fileReader_1.readAsDataURL(file);
                    }
                }
                return [2 /*return*/];
            });
        });
    },
    openDocument: function (element, type) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            if (type == 'File') {
                if (this.filePicker) {
                    this.filePicker.value = '';
                    this.filePicker.click();
                }
            }
            else if (type == 'Image') {
                if (this.imagePicker) {
                    this.imagePicker.value = '';
                    this.imagePicker.click();
                }
            }
        }
    },
    showProperties: function (element, property) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            element.blazor__instance.showProperties(property);
        }
    },
    resize: function (element, width, height) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            element.blazor__instance.resize(width, height);
        }
    },
    setShowHeaderProperties: function (element) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            element.blazor__instance.showHeaderProperties = !element.blazor__instance.showHeaderProperties;
        }
    },
    showPropertiesPaneOnSelection: function (element) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            element.blazor__instance.showPropertiesPaneOnSelection();
        }
    },
    getPreviousContext: function (element) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            return element.blazor__instance.previousContext;
        }
        return '';
    },
    navigateToUrl: function (link) {
        window.open(link);
    }
};

return DocumentEditorContainer;

}());

window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.RichTextEditor = (function () {
'use strict';

/**
 * Css class constants
 */
var CLS_RTE = 'e-richtexteditor';


var CLS_DISABLED = 'e-disabled';
var CLS_SCRIPT_SHEET = 'rte-iframe-script-sheet';
var CLS_STYLE_SHEET = 'rte-iframe-style-sheet';
var CLS_TOOLBAR = 'e-rte-toolbar';

var CLS_TB_FLOAT = 'e-rte-tb-float';
var CLS_TB_ABS_FLOAT = 'e-rte-tb-abs-float';



var CLS_FULL_SCREEN = 'e-rte-full-screen';

var CLS_POP = 'e-rte-pop';
var CLS_QUICK_POP = 'e-rte-quick-popup';


var CLS_INLINE_POP = 'e-rte-inline-popup';





var CLS_RTE_CONTENT = 'e-rte-content';
var CLS_TB_ITEM = 'e-toolbar-item';
var CLS_TB_EXTENDED = 'e-toolbar-extended';

var CLS_POPUP = 'e-popup';




var CLS_SHOW = 'e-show';
var CLS_HIDE = 'e-hide';
var CLS_VISIBLE = 'e-visible';
var CLS_FOCUS = 'e-focused';

var CLS_IMGRIGHT = 'e-imgright';
var CLS_IMGLEFT = 'e-imgleft';
var CLS_IMGCENTER = 'e-imgcenter';
var CLS_IMGBREAK = 'e-imgbreak';
var CLS_CAPTION = 'e-img-caption';
var CLS_RTE_CAPTION = 'e-rte-img-caption';
var CLS_CAPINLINE = 'e-caption-inline';
var CLS_IMGINLINE = 'e-imginline';
var CLS_COUNT = 'e-rte-character-count';
var CLS_WARNING = 'e-warning';
var CLS_ERROR = 'e-error';
var CLS_ICONS = 'e-icons';
var CLS_ACTIVE = 'e-active';
var CLS_EXPAND_OPEN = 'e-expand-open';
var CLS_RTE_ELEMENTS = 'e-rte-elements';


var CLS_TB_IOS_FIX = 'e-tbar-ios-fixed';












var CLS_RTE_READONLY = 'e-rte-readonly';
var CLS_TABLE_SEL = 'e-cell-select';
var CLS_TB_DASH_BOR = 'e-dashed-border';
var CLS_TB_ALT_BOR = 'e-alternate-border';
var CLS_TB_COL_RES = 'e-column-resize';
var CLS_TB_ROW_RES = 'e-row-resize';
var CLS_TB_BOX_RES = 'e-table-box';







var CLS_RTE_RES_HANDLE = 'e-resize-handle';
var CLS_RTE_RES_EAST = 'e-south-east';
var CLS_RTE_RES_WEST = 'e-south-west';
var CLS_RTE_IMAGE = 'e-rte-image';
var CLS_RESIZE = 'e-resize';
var CLS_IMG_FOCUS = 'e-img-focus';
var CLS_RTE_DRAG_IMAGE = 'e-rte-drag-image';
var CLS_RTE_UPLOAD_POPUP = 'e-rte-upload-popup';
var CLS_POPUP_OPEN = 'e-popup-open';
var CLS_IMG_RESIZE = 'e-img-resize';
var CLS_DROPAREA = 'e-droparea';
var CLS_IMG_INNER = 'e-img-inner';

var CLS_RTE_DIALOG_UPLOAD = 'e-rte-dialog-upload';
var CLS_RTE_RES_CNT = 'e-rte-resize';



var CLS_TABLE_BORDER = 'e-rte-table-border';
var CLS_RTE_TABLE_RESIZE = 'e-rte-table-resize';
var CLS_RTE_FIXED_TB_EXPAND = 'e-rte-fixed-tb-expand';
var CLS_RTE_QUICK_POPUP_HIDE = 'e-rte-quick-popup-hide';
var CLS_RTE_OVERFLOW = 'e-rte-overflow';
var CLS_PASTED_CONTENT_IMG = 'pasteContent_Img';
var CLS_FILE_SELECT_WRAP = 'e-file-select-wrap';
var CLS_RTE_PASTE_CONTENT = 'pasteContent_RTE';
var CLS_POPUP_CLOSE = 'e-popup-close';
var CLS_EXTENDED_TOOLBAR = 'e-extended-toolbar';
var CLS_EXPENDED_NAV = 'e-expended-nav';
var CLS_INSERT_TABLE_BTN = 'e-insert-table-btn';
var CLS_DROP_DOWN_BTN = 'e-dropdown-btn';
var CLS_RTE_IMG_BOX_MARK = 'e-rte-imageboxmark';

/**
 * Event constants
 */




var contentChanged = 'content-changed';
var initialEnd = 'initial-end';
var iframeMouseDown = 'iframe-click';
var destroy = 'destroy';
var afterPasteCleanUp = 'afterPasteCleanUp';

var toolbarRefresh = 'toolbar-refresh';
var refreshBegin = 'refresh-begin';
var toolbarUpdated = 'toolbar-updated';
var bindOnEnd = 'bind-on-end';

var htmlToolbarClick = 'html-toolbar-click';
var markdownToolbarClick = 'markdown-toolbar-click';

var modelChanged = 'model-changed';
var keyUp = 'keyUp';
var keyDown = 'keyDown';
var mouseUp = 'mouseUp';


var enableFullScreen = 'enableFullScreen';
var disableFullScreen = 'disableFullScreen';
var dropDownSelect = 'dropDownSelect';

var execCommandCallBack = 'execCommandCallBack';
var imageToolbarAction = 'image-toolbar-action';
var linkToolbarAction = 'link-toolbar-action';
var windowResize = 'resize';





var insertLink = 'insertLink';
var unLink = 'unLink';



var actionComplete = 'actionComplete';


var insertImage = 'insertImage';
var insertCompleted = 'insertCompleted';





var imageLink = 'insertImgLink';
var imageAlt = 'imgAltText';
var imageDelete = 'delete';
var imageCaption = 'caption';
var imageSize = 'imageSize';
var sourceCode = 'sourceCode';
var updateSource = 'updateSource';

var beforeDropDownOpen = 'beforeDropDownOpen';
var selectionSave = 'selection-save';
var selectionRestore = 'selection-restore';

var count = 'count';


var mouseDown = 'mouseDown';
var sourceCodeMouseDown = 'sourceCodeMouseDown';
var editAreaClick = 'editAreaClick';
var scroll = 'scroll';
var contentscroll = 'contentscroll';

var tableColorPickerChanged = 'tableColorPickerChanged';
var focusChange = 'focusChange';
var selectAll$1 = 'selectAll';
var selectRange = 'selectRange';
var getSelectedHtml = 'getSelectedHtml';

var paste = 'paste-content';


var createTable = 'createTable';
var docClick = 'docClick';
var tableToolbarAction = 'table-toolbar-action';
var checkUndo = 'checkUndoStack';
var readOnlyMode = 'readOnlyMode';
var pasteClean = 'pasteClean';














var drop = 'drop';
var xhtmlValidation = 'xhtmlValidation';

var resizeInitialized = 'resizeInitialized';

var MS_WORD_CLEANUP = 'ms_word_cleanup';
var beforePasteUpload = 'beforePasteUpload';
var beforePasteUploadCallBack = 'beforePasteUploadCallBack';
var uploading = 'uploading';
var success = 'success';
var failure = 'failure';
var canceling = 'canceling';
var removing = 'removing';
var selected = 'selected';
var updateTbItemsStatus = 'updateTbItemsStatus';
var outsideClicked = 'Outside Click';
var ON_BEGIN = 'onBegin';
var enterHandler = 'enterHandler';
//Blazor Event
var beforeQuickToolbarOpenEvent = 'BeforeQuickToolbarOpenEvent';
var quickToolbarCloseEvent = 'QuickToolbarCloseEvent';
var actionBeginEvent = 'ActionBeginEvent';
var actionCompleteEvent = 'ActionCompleteEvent';
var beforeUpload = 'BeforeUpload';
var pasteImageUploadFailed = 'PasteImageUploadFailed';
var pasteImageUploadSuccess = 'PasteImageUploadSuccess';
var resizeStartEvent = 'ResizeStartEvent';
var resizeStopEvent = 'ResizeStopEvent';
var updatedToolbarStatusEvent = 'UpdatedToolbarStatusEvent';
//Blazor Methods
var updateClass = 'UpdateClass';
var showFullScreenClient = 'ShowFullScreenClient';
var hideFullScreenClient = 'HideFullScreenClient';
var showImagePopup = 'ShowImagePopup';
var hideImagePopup = 'HideImagePopup';
var showLinkPopup = 'ShowLinkPopup';
var hideLinkPopup = 'HideLinkPopup';
var showTablePopup = 'ShowTablePopup';
var hideTablePopup = 'HideTablePopup';
var showInlinePopup = 'ShowInlinePopup';
var hideInlinePopup = 'HideInlinePopup';
var refreshToolbarOverflow = 'RefreshToolbarOverflow';
var updateUndoRedoStatus = 'UpdateUndoRedoStatus';
var showImageDialog = 'ShowImageDialog';
var closeImageDialog = 'CloseImageDialog';
//Blazor ID
var imageQuickPopup = '_Image_Quick_Popup';
var linkQuickPopup = '_Link_Quick_Popup';
var tableQuickPopup = '_Table_Quick_Popup';
var inlineQuickPopup = '_Inline_Quick_Popup';
var resizeID = '-resizable';
var toolbarCreateTable = '_toolbar_CreateTable';
var imgResizeId = '_imgResize';
/* eslint-disable */
var IFRAME_HEADER = "\n<!DOCTYPE html> \n    <html>\n         <head>\n            <meta charset='utf-8' /> \n            <style>\n                @charset \"UTF-8\";\n                body {\n                    font-family: \"Roboto\", sans-serif;\n                    font-size: 14px;\n                }\n                html, body{height: 100%;margin: 0;}\n                body.e-cursor{cursor:default}\n                span.e-selected-node\t{background-color: #939393;color: white;}\n                span.e-selected-node.e-highlight {background-color: #1d9dd8;}\n                body{color:#333;word-wrap:break-word;padding: 8px;box-sizing: border-box;}\n                .e-rte-image {border: 0;cursor: pointer;display: block;float: none;height: auto;margin: 5px auto;max-width: 100%;position: relative;}\n                .e-img-caption { display: inline-block; float: none; margin: 5px auto; max-width: 100%;position: relative;}\n                .e-img-caption.e-caption-inline {display: inline-block;float: none;margin: 5px auto;margin-left: 5px;margin-right: 5px;max-width: calc(100% - (2 * 5px));position: relativetext-align: center;vertical-align: bottom;}\n                .e-img-inner {box-sizing: border-box;display: block;font-size: 16px;font-weight: initial;margin: auto;opacity: .9;text-align: center;width: 100%;}\n                .e-img-wrap {display: inline-block;margin: auto;padding: 0;text-align: center;width: 100%;}\n                .e-imgleft {float: left;margin: 0 auto;margin-right: 5px;text-align: left;}\n                .e-imgright {float: right;margin: 0 auto;margin-left: 5px;text-align: right;}\n                .e-imgcenter {cursor: pointer;display: block;float: none;height: auto;margin: 5px auto;max-width: 100%;position: relative;}\n                .e-control img:not(.e-resize) {border: 2px solid transparent; z-index: 1000}\n                .e-imginline {display: inline-block;float: none;margin-left: 5px;margin-right: 5px;max-width: calc(100% - (2 * 5px));vertical-align: bottom;}\n                .e-imgbreak {border: 0;cursor: pointer;display: block;float: none;height: auto;margin: 5px auto;max-width: 100%;position: relative;}\n                .e-rte-image.e-img-focus:not(.e-resize) {border: solid 2px #4a90e2;}\n                img.e-img-focus::selection { background: transparent;color: transparent;}\n                span.e-rte-imageboxmark {  width: 10px; height: 10px; position: absolute; display: block; background: #4a90e2; border: 1px solid #fff; z-index: 1000;}\n                .e-mob-rte.e-mob-span span.e-rte-imageboxmark { background: #4a90e2; border: 1px solid #fff; }\n                .e-mob-rte span.e-rte-imageboxmark { background: #fff; border: 1px solid #4a90e2; border-radius: 15px; height: 20px; width: 20px; }\n                .e-mob-rte.e-mob-span span.e-rte-imageboxmark { background: #4a90e2; border: 1px solid #fff; }\n                .e-rte-content .e-content img.e-resize { z-index: 1000; }\n                .e-img-caption .e-img-inner { outline: 0; }\n                .e-img-caption a:focus-visible { outline: none; }\n                .e-rte-img-caption.e-imgcenter { display: block; margin-left: auto; margin-right: auto; }\n                .e-rte-img-caption.e-imgright { display: block; margin-left: auto; margin-right: 0; }\n                .e-rte-img-caption.e-imgleft { display: block; margin-left: 0; margin-right: auto; }\n                .e-img-caption .e-rte-image.e-imgright {float: none; margin-left: auto; margin-right: 0;}\n                .e-img-caption .e-rte-image.e-imgleft { float: none; margin: 0;}\n                .e-rte-img-caption.e-imgleft .e-img-inner {text-align: left;}\n                .e-rte-img-caption.e-imgright .e-img-inner {text-align: right;}\n                body{box-sizing: border-box;min-height: 100px;outline: 0 solid transparent;overflow-x: auto;padding: 16px;position: relative;text-align: inherit;z-index: 2;}\n                p{margin: 0 0 10px;margin-bottom: 10px;}\n                li{margin-bottom: 10px;}\n                h1{font-size: 2.17em;font-weight: 400;line-height: 1;margin: 10px 0;}\n                h2{font-size: 1.74em;font-weight: 400;margin: 10px 0;}\n                h3{font-size: 1.31em;font-weight: 400;margin: 10px 0;}\n                h4{font-size: 16px;font-weight: 400;line-height: 1.5;margin: 0;}\n                h5{font-size: 00.8em;font-weight: 400;margin: 0;}\n                h6{font-size: 00.65em;font-weight: 400;margin: 0;}\n                blockquote{margin: 10px 0;margin-left: 0;padding-left: 5px;border-left: solid 2px #5c5c5c;}\n                .e-rtl blockquote {padding-left: 0;padding-right: 5px;border-left: 0;border-right: 2px solid #5c5c5c;}\n                pre{background-color: inherit;border: 0;border-radius: 0;color: #333;font-size: inherit;line-height: inherit;margin: 0 0 10px;overflow: visible;padding: 0;white-space: pre-wrap;word-break: inherit;word-wrap: break-word;}\n                strong, b{font-weight: 700;}\n                a{text-decoration: none;user-select: auto;}\n                a:hover{text-decoration: underline;};\n                p:last-child, pre:last-child, blockquote:last-child{margin-bottom: 0;}\n                h3+h4, h4+h5, h5+h6{margin-top: 00.6em;}\n                ul:last-child{margin-bottom: 0;}\n                .e-rte-table { border-collapse: collapse; empty-cells: show;}\n                .e-rte-table td, .e-rte-table th {border: 1px solid #BDBDBD; height: 20px; vertical-align: middle;padding: 2px 5px; min-width: 20px;}\n                .e-rte-table.e-rte-table-border { border: 1px solid #BDBDBD; border-collapse: separate; }\n                table.e-alternate-border tbody tr:nth-child(2n) {background-color: #F5F5F5;}\n                table th {background-color: #E0E0E0;}\n                table.e-dashed-border td,table.e-dashed-border th { border: 1px dashed #BDBDBD} \n                table .e-cell-select {border: 1px double #4a90e2;}\n                span.e-table-box { cursor: nwse-resize; display: block; height: 10px; position: absolute; width: 10px; }\n                span.e-table-box.e-hide { display: none; }\n                span.e-table-box.e-rmob {height: 14px;width: 14px;}\n                .e-row-resize, .e-column-resize { background-color: transparent; background-repeat: repeat; bottom: 0;cursor: col-resize;height: 1px;overflow: visible;position: absolute;width: 1px; }\n                .e-row-resize { cursor: row-resize; height: 1px;}\n                .e-table-rhelper { cursor: col-resize; opacity: .87;position: absolute;}\n                .e-table-rhelper.e-column-helper { width: 1px; }\n                .e-table-rhelper.e-row-helper {height: 1px;}\n                .e-reicon::before { border-bottom: 6px solid transparent; border-right: 6px solid; border-top: 6px solid transparent; content: ''; display: block; height: 0; position: absolute; right: 4px; top: 4px; width: 20px; }\n                .e-reicon::after { border-bottom: 6px solid transparent; border-left: 6px solid; border-top: 6px solid transparent; content: ''; display: block; height: 0; left: 4px; position: absolute; top: 4px; width: 20px; z-index: 3; }\n                .e-row-helper.e-reicon::after { top: 10px; transform: rotate(90deg); }\n                .e-row-helper.e-reicon::before { left: 4px; top: -20px; transform: rotate(90deg); }\n                span.e-table-box { background-color: #ffffff; border: 1px solid #BDBDBD; }\n                span.e-table-box.e-rbox-select { background-color: #BDBDBD; border: 1px solid #BDBDBD; }\n                .e-table-rhelper { background-color: #4a90e2;}\n                .e-rtl { direction: rtl; }\n            </style>\n        </head>";
/* eslint-enable */

/**
 * `Count` module is used to handle Count actions.
 */
var Count = /** @class */ (function () {
    function Count(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    Count.prototype.renderCount = function () {
        if (this.parent.showCharCount) {
            this.addEventListener();
            this.countElement = this.parent.element.querySelector('.' + CLS_COUNT);
            this.appendCount();
            if (this.parent.maxLength !== -1) {
                this.charCountBackground(this.htmlLength);
            }
        }
    };
    Count.prototype.appendCount = function () {
        this.countElement = this.parent.element.querySelector('.' + CLS_COUNT);
        var htmlText = this.parent.editorMode === 'Markdown' ? this.parent.getEditPanel().value.trim() :
            this.parent.getEditPanel().textContent.trim();
        if (this.parent.editorMode !== 'Markdown' && htmlText.indexOf('\u200B') !== -1) {
            this.htmlLength = htmlText.replace(/\u200B/g, '').length;
        }
        else {
            this.htmlLength = htmlText.length;
        }
        var string = this.parent.maxLength === -1 ? this.htmlLength : this.htmlLength + ' / ' + this.parent.maxLength;
        this.countElement.innerHTML = string;
    };
    Count.prototype.charCountBackground = function (htmlLength) {
        this.countElement = this.parent.element.querySelector('.' + CLS_COUNT);
        var percentage = (htmlLength / this.parent.maxLength) * 100;
        if (percentage < 85) {
            this.countElement.classList.remove(CLS_WARNING);
            this.countElement.classList.remove(CLS_ERROR);
        }
        else if (percentage > 85 && percentage <= 90) {
            this.countElement.classList.remove(CLS_ERROR);
            this.countElement.classList.add(CLS_WARNING);
        }
        else if (percentage > 90) {
            this.countElement.classList.remove(CLS_WARNING);
            this.countElement.classList.add(CLS_ERROR);
        }
    };
    Count.prototype.refresh = function () {
        if (!sf.base.isNullOrUndefined(this.parent.element) && this.parent.showCharCount) {
            this.appendCount();
            if (this.parent.maxLength !== -1) {
                this.charCountBackground(this.htmlLength);
            }
        }
    };
    Count.prototype.destroy = function () {
        this.countElement = null;
        this.removeEventListener();
    };
    Count.prototype.toggle = function (e) {
        if (this.parent.showCharCount) {
            this.countElement.style.display = (e.member === 'viewSource') ? 'none' : 'block';
        }
    };
    Count.prototype.addEventListener = function () {
        this.parent.observer.on(initialEnd, this.renderCount, this);
        this.parent.observer.on(keyUp, this.refresh, this);
        this.parent.observer.on(count, this.refresh, this);
        this.parent.observer.on(refreshBegin, this.refresh, this);
        this.parent.observer.on(mouseDown, this.refresh, this);
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(sourceCode, this.toggle, this);
        this.parent.observer.on(updateSource, this.toggle, this);
    };
    Count.prototype.removeEventListener = function () {
        this.parent.observer.off(initialEnd, this.renderCount);
        this.parent.observer.off(keyUp, this.refresh);
        this.parent.observer.off(refreshBegin, this.refresh);
        this.parent.observer.off(count, this.refresh);
        this.parent.observer.off(mouseDown, this.refresh);
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(sourceCode, this.toggle);
        this.parent.observer.off(updateSource, this.toggle);
    };
    return Count;
}());

/**
 * `Resize` module is used to resize the editor
 */
var Resize = /** @class */ (function () {
    function Resize(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    Resize.prototype.addEventListener = function () {
        this.parent.observer.on(initialEnd, this.renderResizable, this);
        this.parent.observer.on(destroy, this.destroy, this);
    };
    Resize.prototype.renderResizable = function () {
        if (this.parent.enableResize) {
            var resizeClass = CLS_ICONS + ' ' + CLS_RTE_RES_HANDLE + ' ';
            resizeClass += this.parent.enableRtl ? CLS_RTE_RES_WEST : CLS_RTE_RES_EAST;
            this.resizer = sf.base.createElement('div', { id: this.parent.id + resizeID, className: resizeClass });
            this.parent.element.classList.add(CLS_RTE_RES_CNT);
            this.parent.element.appendChild(this.resizer);
            this.parent.setContentHeight();
            this.touchStartEvent = (sf.base.Browser.info.name === 'msie') ? 'pointerdown' : 'touchstart';
            sf.base.EventHandler.add(this.resizer, 'mousedown', this.resizeStart, this);
            sf.base.EventHandler.add(this.resizer, this.touchStartEvent, this.resizeStart, this);
        }
    };
    Resize.prototype.resizeStart = function (e) {
        var _this = this;
        if (e.cancelable) {
            e.preventDefault();
        }
        this.wireResizeEvents();
        this.parent.observer.notify(resizeInitialized, {});
        var args = { requestType: 'editor' };
        if (this.parent.onResizeStartEnabled) {
            // @ts-ignore-start
            this.parent.dotNetRef.invokeMethodAsync(resizeStartEvent, args).then(function (resizeStartArgs) {
                // @ts-ignore-end
                if (resizeStartArgs.cancel) {
                    _this.unwireResizeEvents();
                }
            });
        }
    };
    Resize.prototype.performResize = function (e) {
        var boundRect = this.parent.element.getBoundingClientRect();
        if (this.isMouseEvent(e)) {
            this.parent.element.style.height = e.clientY - boundRect.top + 'px';
            this.parent.element.style.width = (this.parent.enableRtl ? boundRect.right - e.clientX :
                e.clientX - boundRect.left) + 'px';
        }
        else {
            var eventType = sf.base.Browser.info.name !== 'msie' ? e.touches[0] : e;
            this.parent.element.style.height = eventType.clientY - boundRect.top + 'px';
            this.parent.element.style.width = (this.parent.enableRtl ? boundRect.right - eventType.clientX :
                eventType.clientX - boundRect.left) + 'px';
        }
        this.parent.refresh();
    };
    Resize.prototype.stopResize = function (e) {
        this.parent.refresh();
        this.unwireResizeEvents();
        var args = { requestType: 'editor' };
        if (this.parent.onResizeStopEnabled) {
            this.parent.dotNetRef.invokeMethodAsync(resizeStopEvent, args);
        }
    };
    Resize.prototype.getEventType = function (e) {
        return (e.indexOf('mouse') > -1) ? 'mouse' : 'touch';
    };
    Resize.prototype.isMouseEvent = function (e) {
        var isMouse = false;
        if (this.getEventType(e.type) === 'mouse' || (!sf.base.isNullOrUndefined(e.pointerType) &&
            this.getEventType(e.pointerType) === 'mouse')) {
            isMouse = true;
        }
        return isMouse;
    };
    Resize.prototype.wireResizeEvents = function () {
        sf.base.EventHandler.add(document, 'mousemove', this.performResize, this);
        sf.base.EventHandler.add(document, 'mouseup', this.stopResize, this);
        this.touchMoveEvent = (sf.base.Browser.info.name === 'msie') ? 'pointermove' : 'touchmove';
        this.touchEndEvent = (sf.base.Browser.info.name === 'msie') ? 'pointerup' : 'touchend';
        sf.base.EventHandler.add(document, this.touchMoveEvent, this.performResize, this);
        sf.base.EventHandler.add(document, this.touchEndEvent, this.stopResize, this);
    };
    Resize.prototype.unwireResizeEvents = function () {
        sf.base.EventHandler.remove(document, 'mousemove', this.performResize);
        sf.base.EventHandler.remove(document, 'mouseup', this.stopResize);
        sf.base.EventHandler.remove(document, this.touchMoveEvent, this.performResize);
        sf.base.EventHandler.remove(document, this.touchEndEvent, this.stopResize);
    };
    Resize.prototype.destroy = function () {
        this.removeEventListener();
    };
    Resize.prototype.removeEventListener = function () {
        this.parent.observer.off(initialEnd, this.renderResizable);
        this.parent.element.classList.remove(CLS_RTE_RES_CNT);
        sf.base.EventHandler.remove(this.resizer, 'mousedown', this.resizeStart);
        sf.base.EventHandler.remove(this.resizer, this.touchStartEvent, this.resizeStart);
        if (this.resizer) {
            sf.base.detach(this.resizer);
        }
        this.parent.observer.off(destroy, this.destroy);
    };
    return Resize;
}());

/**
 * Util functions
 */
var inlineNode = ['a', 'abbr', 'acronym', 'audio', 'b', 'bdi', 'bdo', 'big', 'br', 'button',
    'canvas', 'cite', 'code', 'data', 'datalist', 'del', 'dfn', 'em', 'embed', 'font', 'i', 'iframe', 'img', 'input',
    'ins', 'kbd', 'label', 'map', 'mark', 'meter', 'noscript', 'object', 'output', 'picture', 'progress',
    'q', 'ruby', 's', 'samp', 'script', 'select', 'slot', 'small', 'span', 'strong', 'strike', 'sub', 'sup', 'svg',
    'template', 'textarea', 'time', 'u', 'tt', 'var', 'video', 'wbr'];
var executeGroup = {
    'bold': {
        command: 'Style',
        subCommand: 'Bold',
        value: 'strong'
    },
    'italic': {
        command: 'Style',
        subCommand: 'Italic',
        value: 'em'
    },
    'underline': {
        command: 'Style',
        subCommand: 'Underline',
        value: 'span'
    },
    'strikeThrough': {
        command: 'Style',
        subCommand: 'StrikeThrough',
        value: 'span'
    },
    'insertCode': {
        command: 'Formats',
        subCommand: 'Pre',
        value: 'pre'
    },
    'superscript': {
        command: 'Effects',
        subCommand: 'SuperScript',
        value: 'sup'
    },
    'subscript': {
        command: 'Effects',
        subCommand: 'SubScript',
        value: 'sub'
    },
    'uppercase': {
        command: 'Casing',
        subCommand: 'UpperCase'
    },
    'lowercase': {
        command: 'Casing',
        subCommand: 'LowerCase'
    },
    'fontColor': {
        command: 'font',
        subCommand: 'fontcolor',
        value: '#ff0000'
    },
    'fontName': {
        command: 'font',
        subCommand: 'fontname',
        value: 'Segoe UI'
    },
    'fontSize': {
        command: 'font',
        subCommand: 'fontsize',
        value: '10pt'
    },
    'backColor': {
        command: 'font',
        subCommand: 'backgroundcolor',
        value: '#ffff00'
    },
    'justifyCenter': {
        command: 'Alignments',
        subCommand: 'JustifyCenter'
    },
    'justifyFull': {
        command: 'Alignments',
        subCommand: 'JustifyFull'
    },
    'justifyLeft': {
        command: 'Alignments',
        subCommand: 'JustifyLeft'
    },
    'justifyRight': {
        command: 'Alignments',
        subCommand: 'JustifyRight'
    },
    'undo': {
        command: 'Actions',
        subCommand: 'Undo'
    },
    'redo': {
        command: 'Actions',
        subCommand: 'Redo'
    },
    'createLink': {
        command: 'Links',
        subCommand: 'createLink'
    },
    'editLink': {
        command: 'Links',
        subCommand: 'createLink'
    },
    'createImage': {
        command: 'Images',
        subCommand: 'Images'
    },
    'formatBlock': {
        command: 'Formats',
        value: 'P'
    },
    'heading': {
        command: 'Formats',
        value: 'H1'
    },
    'indent': {
        command: 'Indents',
        subCommand: 'Indent'
    },
    'outdent': {
        command: 'Indents',
        subCommand: 'Outdent'
    },
    'insertHTML': {
        command: 'InsertHTML',
        subCommand: 'InsertHTML',
        value: ''
    },
    'insertText': {
        command: 'InsertText',
        subCommand: 'InsertText',
        value: ''
    },
    'insertHorizontalRule': {
        command: 'InsertHTML',
        subCommand: 'InsertHTML',
        value: '<hr/>'
    },
    'insertImage': {
        command: 'Images',
        subCommand: 'Image',
    },
    'editImage': {
        command: 'Images',
        subCommand: 'Image',
    },
    'insertTable': {
        command: 'Table',
        subCommand: 'CreateTable'
    },
    'insertBrOnReturn': {
        command: 'InsertHTML',
        subCommand: 'InsertHTML',
        value: '<br/>'
    },
    'insertOrderedList': {
        command: 'Lists',
        value: 'OL'
    },
    'insertUnorderedList': {
        command: 'Lists',
        value: 'UL'
    },
    'insertParagraph': {
        command: 'Formats',
        value: 'P'
    },
    'removeFormat': {
        command: 'Clear',
        subCommand: 'ClearFormat'
    }
};
function sanitizeHelper(value, parent) {
    if (parent.enableHtmlSanitizer) {
        var item_1 = sf.base.SanitizeHtmlHelper.beforeSanitize();
        var beforeEvent = {
            cancel: false,
            helper: null
        };
        sf.base.extend(item_1, item_1, beforeEvent);
        // @ts-ignore-start
        //  parent.dotNetRef.invokeMethodAsync('BeforeSanitizeHtmlEvent', item).then((sanitizeArgs: BeforeSanitizeHtmlArgs) => {
        // @ts-ignore-end
        if (!sf.base.isNullOrUndefined(parent.deniedSanitizeSelectors) && parent.deniedSanitizeSelectors.length > 0) {
            var _loop_1 = function (i) {
                for (var j = 0; j < item_1.selectors.tags.length; j++) {
                    if (item_1.selectors.tags[j] === parent.deniedSanitizeSelectors[i]) {
                        item_1.selectors.tags = item_1.selectors.tags.filter(function (values) { return values != parent.deniedSanitizeSelectors[i]; });
                    }
                }
                var _loop_2 = function (k) {
                    if ((item_1.selectors.attributes[k].attribute || item_1.selectors.attributes[k].selector) === parent.deniedSanitizeSelectors[i]) {
                        item_1.selectors.attributes = item_1.selectors.attributes.filter(function (values) { return values != item_1.selectors.attributes[k]; });
                    }
                };
                for (var k = 0; k < item_1.selectors.attributes.length; k++) {
                    _loop_2(k);
                }
            };
            for (var i = 0; i < parent.deniedSanitizeSelectors.length; i++) {
                _loop_1(i);
            }
        }
        if (!item_1.cancel) {
            value = sf.base.SanitizeHtmlHelper.serializeValue(item_1, value);
        }
        return value;
        // });
    }
    return value;
}
function getIndex(val, items) {
    var index = -1;
    items.some(function (item, i) {
        if (!sf.base.isNullOrUndefined(item) && typeof item.subCommand === 'string' && val === item.subCommand.toLocaleLowerCase()) {
            index = i;
            return true;
        }
        return false;
    });
    return index;
}
function getDropDownValue(items, value, type, returnType) {
    var data;
    var result;
    for (var k = 0; k < items.length; k++) {
        if (type === 'value' && items[k].value.toLocaleLowerCase() === value.toLocaleLowerCase()) {
            data = items[k];
            break;
        }
        else if (type === 'text' && items[k].text.toLocaleLowerCase() === value.toLocaleLowerCase()) {
            data = items[k];
            break;
        }
        else if (type === 'subCommand' && items[k].subCommand.toLocaleLowerCase() === value.toLocaleLowerCase()) {
            data = items[k];
            break;
        }
    }
    if (!sf.base.isNullOrUndefined(data)) {
        switch (returnType) {
            case 'text':
                result = data.text;
                break;
            case 'value':
                result = data.value;
                break;
            case 'cssClass':
                result = data.cssClass;
                break;
        }
    }
    return result;
}
function getEditValue(value, rteObj) {
    var val;
    if (value !== null && value !== '') {
        val = rteObj.enableHtmlEncode ? rteObj.encode(updateTextNode(decode(value), rteObj)) : updateTextNode(value, rteObj);
        rteObj.value = val;
    }
    else {
        if (rteObj.enterKey === 'DIV') {
            val = rteObj.enableHtmlEncode ? '&lt;div&gt;&lt;br/&gt;&lt;/div&gt;' : '<div><br/></div>';
        }
        else if (rteObj.enterKey === 'BR') {
            val = rteObj.enableHtmlEncode ? '&lt;br/&gt;' : '<br/>';
        }
        else {
            val = rteObj.enableHtmlEncode ? '&lt;p&gt;&lt;br/&gt;&lt;/p&gt;' : '<p><br/></p>';
        }
    }
    return val;
}
/**
 * @param {SfRichTextEditor} rteObj - specifies the rte object
 * @returns {string} - returns the value based on enter configuration.
 * @hidden
 */
function getDefaultValue(rteObj) {
    var currentVal;
    if (rteObj.enterKey === 'DIV') {
        currentVal = '<div><br/></div>';
    }
    else if (rteObj.enterKey === 'BR') {
        currentVal = '<br/>';
    }
    else {
        currentVal = '<p><br/></p>';
    }
    return currentVal;
}
/**
 * @param {string} value - specifies the value
 * @returns {string} - returns the string
 * @hidden
 */
function updateTextNode(value, rteObj) {
    var tempNode = document.createElement('div');
    var resultElm = document.createElement('div');
    var childNodes = tempNode.childNodes;
    tempNode.innerHTML = value;
    tempNode.setAttribute('class', 'tempDiv');
    if (childNodes.length > 0) {
        var isPreviousInlineElem = void 0;
        var previousParent = void 0;
        var insertElem = void 0;
        while (tempNode.firstChild) {
            if (rteObj.enterKey !== 'BR' && ((tempNode.firstChild.nodeName === '#text' &&
                (tempNode.firstChild.textContent.indexOf('\n') < 0 || tempNode.firstChild.textContent.trim() !== '')) ||
                inlineNode.indexOf(tempNode.firstChild.nodeName.toLocaleLowerCase()) >= 0)) {
                if (!isPreviousInlineElem) {
                    if (rteObj.enterKey === 'DIV') {
                        insertElem = sf.base.createElement('div');
                    }
                    else {
                        insertElem = sf.base.createElement('p');
                    }
                    resultElm.appendChild(insertElem);
                    insertElem.appendChild(tempNode.firstChild);
                }
                else {
                    previousParent.appendChild(tempNode.firstChild);
                }
                previousParent = insertElem;
                isPreviousInlineElem = true;
            }
            else if (tempNode.firstChild.nodeName === '#text' && (tempNode.firstChild.textContent === '\n' ||
                (tempNode.firstChild.textContent.indexOf('\n') >= 0 && tempNode.firstChild.textContent.trim() === ''))) {
                sf.base.detach(tempNode.firstChild);
            }
            else {
                resultElm.appendChild(tempNode.firstChild);
                isPreviousInlineElem = false;
            }
        }
        var imageElm = resultElm.querySelectorAll('img');
        for (var i = 0; i < imageElm.length; i++) {
            if (!imageElm[i].classList.contains(CLS_RTE_IMAGE)) {
                imageElm[i].classList.add(CLS_RTE_IMAGE);
            }
            if (!(imageElm[i].classList.contains(CLS_IMGINLINE) ||
                imageElm[i].classList.contains(CLS_IMGBREAK))) {
                imageElm[i].classList.add(CLS_IMGINLINE);
            }
        }
    }
    return resultElm.innerHTML;
}
function isIDevice() {
    return sf.base.Browser.isDevice && sf.base.Browser.isIos;
}
function decode(value) {
    return value.replace(/&amp;/g, '&').replace(/&amp;lt;/g, '<')
        .replace(/&lt;/g, '<').replace(/&amp;gt;/g, '>')
        .replace(/&gt;/g, '>').replace(/&nbsp;/g, ' ')
        .replace(/&amp;nbsp;/g, ' ').replace(/&quot;/g, '');
}
function dispatchEvent(element, type) {
    var evt = document.createEvent('HTMLEvents');
    evt.initEvent(type, false, true);
    element.dispatchEvent(evt);
}
function hasClass(element, className) {
    var hasClass = false;
    if (element.classList.contains(className)) {
        hasClass = true;
    }
    return hasClass;
}
function parseHtml(value) {
    var tempNode = sf.base.createElement('template');
    tempNode.innerHTML = value;
    if (tempNode.content instanceof DocumentFragment) {
        return tempNode.content;
    }
    else {
        return document.createRange().createContextualFragment(value);
    }
}
function setAttributes(htmlAttributes, rte, isFrame, initial) {
    var target;
    if (isFrame) {
        var iFrame = rte.getDocument();
        target = iFrame.querySelector('body');
    }
    else {
        target = rte.element;
    }
    if (Object.keys(htmlAttributes).length) {
        for (var _i = 0, _a = Object.keys(htmlAttributes); _i < _a.length; _i++) {
            var htmlAttr = _a[_i];
            if (htmlAttr === 'class') {
                target.classList.add(htmlAttributes[htmlAttr]);
            }
            else if (htmlAttr === 'disabled' && htmlAttributes[htmlAttr] === 'disabled') {
                rte.enabled = false;
                rte.setEnable();
            }
            else if (htmlAttr === 'readonly' && htmlAttributes[htmlAttr] === 'readonly') {
                rte.readonly = true;
                rte.setReadOnly(initial);
            }
            else if (htmlAttr === 'style') {
                target.setAttribute('style', htmlAttributes[htmlAttr]);
            }
            else if (htmlAttr === 'tabindex') {
                rte.inputElement.setAttribute('tabindex', htmlAttributes[htmlAttr]);
            }
            else if (htmlAttr === 'placeholder') {
                rte.placeholder = htmlAttributes[htmlAttr];
                rte.setPlaceHolder();
            }
            else {
                var validateAttr = ['name', 'required'];
                if (validateAttr.indexOf(htmlAttr) > -1) {
                    rte.valueContainer.setAttribute(htmlAttr, htmlAttributes[htmlAttr]);
                }
                else {
                    target.setAttribute(htmlAttr, htmlAttributes[htmlAttr]);
                }
            }
        }
    }
}
function convertToBlob(dataUrl) {
    var arr = dataUrl.split(',');
    var mime = arr[0].match(/:(.*?);/)[1];
    var bstr = atob(arr[1]);
    var n = bstr.length;
    var u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new Blob([u8arr], { type: mime });
}
function getFormattedFontSize(value) {
    if (sf.base.isNullOrUndefined(value)) {
        return '';
    }
    return value;
}
function setToolbarStatus(e, isPopToolbar) {
    var dropDown = e.dropDownModule;
    var data = e.args;
    var keys = Object.keys(e.args);
    for (var _i = 0, keys_1 = keys; _i < keys_1.length; _i++) {
        var key = keys_1[_i];
        for (var j = 0; j < e.tbItems.length; j++) {
            var item = e.tbItems[j] && e.tbItems[j].subCommand;
            var itemStr = item && item.toLocaleLowerCase();
            if (item && (itemStr === key) || (item === 'UL' && key === 'unorderedlist') || (item === 'OL' && key === 'orderedlist') ||
                (itemStr === 'pre' && key === 'insertcode')) {
                if (typeof data[key] === 'boolean') {
                    if (data[key] === true) {
                        sf.base.addClass([e.tbElements[j]], [CLS_ACTIVE]);
                    }
                    else {
                        sf.base.removeClass([e.tbElements[j]], [CLS_ACTIVE]);
                    }
                }
                else if ((typeof data[key] === 'string' || data[key] === null) &&
                    getIndex(key, e.parent.toolbarSettings.items) > -1) {
                    var value = ((data[key]) ? data[key] : '');
                    var result = '';
                    var dropdownBtnText = void 0;
                    switch (key) {
                        case 'formats':
                            if (isPopToolbar) {
                                break;
                            }
                            var formatItems = e.parent.format.items;
                            result = getDropDownValue(formatItems, value, 'subCommand', 'text');
                            var formatContent = (sf.base.isNullOrUndefined(e.parent.format.default) ? formatItems[0].text :
                                e.parent.format.default);
                            dropdownBtnText = e.tbElements[j].querySelector('.e-rte-dropdown-btn-text');
                            dropdownBtnText.innerText = (sf.base.isNullOrUndefined(result) ? formatContent : result);
                            dropdownBtnText.parentElement.style.width = e.parent.format.width;
                            break;
                        case 'alignments':
                            result = getDropDownValue(e.parent.alignments, value, 'subCommand', 'cssClass');
                            dropdownBtnText = e.tbElements[j].querySelector('.e-btn-icon.e-icons');
                            sf.base.removeClass([dropdownBtnText], ['e-justify-left', 'e-justify-center', 'e-justify-right', 'e-justify-full']);
                            sf.base.addClass([dropdownBtnText], (sf.base.isNullOrUndefined(result) ? ['e-icons', 'e-justify-left'] : result.split(' ')));
                            break;
                        case 'fontname':
                            if (isPopToolbar) {
                                break;
                            }
                            var fontNameItems = e.parent.fontFamily.items;
                            result = getDropDownValue(fontNameItems, value, 'value', 'text');
                            var fontNameContent = sf.base.isNullOrUndefined(e.parent.fontFamily.default) ? fontNameItems[0].text :
                                e.parent.fontFamily.default;
                            var name_1 = (sf.base.isNullOrUndefined(result) ? fontNameContent : result);
                            e.tbElements[j].title = name_1;
                            dropdownBtnText = e.tbElements[j].querySelector('.e-rte-dropdown-btn-text');
                            dropdownBtnText.innerText = name_1;
                            dropdownBtnText.parentElement.style.width = e.parent.fontFamily.width;
                            break;
                        case 'fontsize':
                            var fontSizeItems = e.parent.fontSize.items;
                            var fontSizeContent = (sf.base.isNullOrUndefined(e.parent.fontSize.default) ? fontSizeItems[1].text :
                                e.parent.fontSize.default);
                            result = getDropDownValue(fontSizeItems, (value === '' ? fontSizeContent.replace(/\s/g, '') : value), 'value', 'text');
                            dropdownBtnText = e.tbElements[j].querySelector('.e-rte-dropdown-btn-text');
                            dropdownBtnText.innerText = getFormattedFontSize(result);
                            dropdownBtnText.parentElement.style.width = e.parent.fontSize.width;
                            break;
                    }
                }
            }
        }
    }
}
function pageYOffset(e, parentElement, isIFrame) {
    var y = 0;
    if (isIFrame) {
        y = window.pageYOffset + parentElement.getBoundingClientRect().top + e.clientY;
    }
    else {
        y = e.pageY;
    }
    return y;
}

/**
 * Defines types of Render
 *
 * @hidden
 * @deprecated
 */
var RenderType;
(function (RenderType) {
    /* eslint-disable */
    /** Defines RenderType as Toolbar */
    RenderType[RenderType["Toolbar"] = 0] = "Toolbar";
    /** Defines RenderType as Content */
    RenderType[RenderType["Content"] = 1] = "Content";
    /** Defines RenderType as Popup */
    RenderType[RenderType["Popup"] = 2] = "Popup";
    /** Defines RenderType as LinkToolbar */
    RenderType[RenderType["LinkToolbar"] = 3] = "LinkToolbar";
    /** Defines RenderType as TextToolbar */
    RenderType[RenderType["TextToolbar"] = 4] = "TextToolbar";
    /** Defines RenderType as ImageToolbar */
    RenderType[RenderType["ImageToolbar"] = 5] = "ImageToolbar";
    /** Defines RenderType as InlineToolbar */
    RenderType[RenderType["InlineToolbar"] = 6] = "InlineToolbar";
    /** Defines RenderType as TableToolbar */
    RenderType[RenderType["TableToolbar"] = 7] = "TableToolbar";
    /* eslint-enable */
})(RenderType || (RenderType = {}));
/**
 * Defines types to be used as Toolbar.
 */
var ToolbarType;
(function (ToolbarType) {
    /* eslint-disable */
    /** Defines ToolbarType as Standard */
    ToolbarType["Expand"] = "Expand";
    /** Defines ToolbarType as MultiRow */
    ToolbarType["MultiRow"] = "MultiRow";
    /** Defines ToolbarType as Scrollable */
    ToolbarType["Scrollable"] = "Scrollable";
    /* eslint-enable */
})(ToolbarType || (ToolbarType = {}));
/**
 * Defines the type of dialog, which open or close in the Rich Text Editor.
 */
var DialogType;
(function (DialogType) {
    /* eslint-disable */
    /** Defines ToolbarType as Standard */
    DialogType["InsertLink"] = "InsertLink";
    /** Defines ToolbarType as MultiRow */
    DialogType["InsertImage"] = "InsertImage";
    /** Defines ToolbarType as Scrollable */
    DialogType["InsertTable"] = "InsertTable";
    /* eslint-enable */
})(DialogType || (DialogType = {}));

/**
 * `Toolbar` module is used to handle Toolbar actions.
 */
var Toolbar = /** @class */ (function () {
    function Toolbar(parent) {
        this.isToolbar = false;
        this.isTransformChild = false;
        this.parent = parent;
        this.tbElement = this.parent.getToolbarElement();
        this.checkIsTransformChild();
        this.addEventListener();
        this.wireEvents();
    }
    Toolbar.prototype.getToolbarHeight = function () {
        return this.parent.getToolbar().offsetHeight;
    };
    Toolbar.prototype.getExpandTBarPopHeight = function () {
        var popHeight = 0;
        if (this.parent.toolbarSettings.type === ToolbarType.Expand &&
            this.parent.getToolbar().classList.contains(CLS_EXTENDED_TOOLBAR)) {
            var expandPopup = sf.base.select('.' + CLS_TB_EXTENDED, this.parent.getToolbar());
            if (expandPopup && this.parent.getToolbar().classList.contains(CLS_EXPAND_OPEN)
                || expandPopup && expandPopup.classList.contains(CLS_POPUP_OPEN)) {
                sf.base.addClass([expandPopup], [CLS_VISIBLE]);
                popHeight = popHeight + expandPopup.offsetHeight;
                sf.base.removeClass([expandPopup], [CLS_VISIBLE]);
            }
            else {
                sf.base.removeClass([this.parent.getToolbar()], [CLS_EXPAND_OPEN]);
            }
        }
        return popHeight;
    };
    Toolbar.prototype.updateToolbarStatus = function (args) {
        if (!this.parent.getToolbarElement() || (this.parent.inlineMode.enable && (isIDevice() || !sf.base.Browser.isDevice))) {
            return;
        }
        var options = {
            args: args,
            dropDownModule: null,
            parent: this.parent,
            tbElements: sf.base.selectAll('.' + CLS_TB_ITEM, this.parent.getToolbarElement()),
            /* eslint-disable */
            tbItems: this.parent.toolbarSettings.items
            /* eslint-enable */
        };
        setToolbarStatus(options, (this.parent.inlineMode.enable ? true : false));
    };
    Toolbar.prototype.checkIsTransformChild = function () {
        this.isTransformChild = false;
        var transformElements = sf.base.selectAll('[style*="transform"]', document);
        for (var i = 0; i < transformElements.length; i++) {
            if (!sf.base.isNullOrUndefined(transformElements[i].contains) && transformElements[i].contains(this.parent.element)) {
                this.isTransformChild = true;
                break;
            }
        }
    };
    Toolbar.prototype.toggleFloatClass = function (e) {
        var topValue;
        var isBody = false;
        var isFloat = false;
        var scrollParent;
        var floatOffset = this.parent.floatingToolbarOffset;
        if (e && this.parent.iframeSettings.enable && this.parent.inputElement.ownerDocument === e.target) {
            scrollParent = e.target.body;
        }
        else if (e && e.target !== document) {
            scrollParent = e.target;
        }
        else {
            isBody = true;
            scrollParent = document.body;
        }
        var tbHeight = this.getToolbarHeight() + this.getExpandTBarPopHeight();
        if (this.isTransformChild) {
            topValue = 0;
            var scrollParentRelativeTop = 0;
            var trgHeight = this.parent.element.offsetHeight;
            if (isBody) {
                var bodyStyle = window.getComputedStyle(scrollParent);
                scrollParentRelativeTop = parseFloat(bodyStyle.marginTop.split('px')[0]) + parseFloat(bodyStyle.paddingTop.split('px')[0]);
            }
            var targetTop = this.parent.element.getBoundingClientRect().top;
            var scrollParentYOffset = (sf.base.Browser.isMSPointer && isBody) ? window.pageYOffset : scrollParent.parentElement.scrollTop;
            var scrollParentRect = scrollParent.getBoundingClientRect();
            var scrollParentTop = (!isBody) ? scrollParentRect.top : (scrollParentRect.top + scrollParentYOffset);
            var outOfRange = ((targetTop - ((!isBody) ? scrollParentTop : 0)) + trgHeight > tbHeight + floatOffset) ? false : true;
            if (targetTop > (scrollParentTop + floatOffset) || targetTop < -trgHeight || ((targetTop < 0) ? outOfRange : false)) {
                isFloat = false;
                sf.base.removeClass([this.tbElement], [CLS_TB_ABS_FLOAT]);
            }
            else if (targetTop < (scrollParentTop + floatOffset)) {
                if (targetTop < 0) {
                    topValue = (-targetTop) + scrollParentTop;
                }
                else {
                    topValue = scrollParentTop - targetTop;
                }
                topValue = (isBody) ? topValue - scrollParentRelativeTop : topValue;
                sf.base.addClass([this.tbElement], [CLS_TB_ABS_FLOAT]);
                isFloat = true;
            }
        }
        else {
            var parent_1 = this.parent.element.getBoundingClientRect();
            if (window.innerHeight < parent_1.top) {
                return;
            }
            topValue = (e && e.target !== document) ? scrollParent.getBoundingClientRect().top : 0;
            if ((parent_1.bottom < (floatOffset + tbHeight + topValue)) || parent_1.bottom < 0 || parent_1.top > floatOffset + topValue) {
                isFloat = false;
            }
            else if (parent_1.top < floatOffset) {
                isFloat = true;
            }
        }
        if (!isFloat) {
            sf.base.removeClass([this.tbElement], [CLS_TB_FLOAT]);
            sf.base.setStyleAttribute(this.tbElement, { top: 0 + 'px', width: '100%' });
        }
        else {
            sf.base.addClass([this.tbElement], [CLS_TB_FLOAT]);
            sf.base.setStyleAttribute(this.tbElement, { width: this.parent.element.offsetWidth + 'px', top: (floatOffset + topValue) + 'px' });
        }
    };
    Toolbar.prototype.getDOMVisibility = function (el) {
        if (!el.offsetParent && el.offsetWidth === 0 && el.offsetHeight === 0) {
            return false;
        }
        return true;
    };
    Toolbar.prototype.showFixedTBar = function () {
        this.tbElement = this.parent.getToolbarElement();
        sf.base.addClass([this.tbElement], [CLS_SHOW]);
        if (sf.base.Browser.isIos) {
            sf.base.addClass([this.tbElement], [CLS_TB_IOS_FIX]);
        }
    };
    Toolbar.prototype.hideFixedTBar = function () {
        this.tbElement = this.parent.getToolbarElement();
        (!this.isToolbar) ? sf.base.removeClass([this.tbElement], [CLS_SHOW, CLS_TB_IOS_FIX]) : this.isToolbar = false;
    };
    Toolbar.prototype.dropDownBeforeOpenHandler = function (args) {
        this.isToolbar = true;
        if (!sf.base.isNullOrUndefined(sf.base.closest(args.element, ".e-rte-toolbar")) && sf.base.isNullOrUndefined(sf.base.closest(args.element, ".e-rte-quick-toolbar"))) {
            this.parent.hideTableQuickToolbar();
        }
    };
    //#region Bind & Unbind Events
    Toolbar.prototype.addEventListener = function () {
        this.parent.observer.on(scroll, this.scrollHandler, this);
        this.parent.observer.on(refreshBegin, this.onRefresh, this);
        this.parent.observer.on(bindOnEnd, this.toolbarBindEvent, this);
        this.parent.observer.on(mouseDown, this.mouseDownHandler, this);
        this.parent.observer.on(focusChange, this.focusChangeHandler, this);
        this.parent.observer.on(toolbarUpdated, this.updateToolbarStatus, this);
        this.parent.observer.on(beforeDropDownOpen, this.dropDownBeforeOpenHandler, this);
    };
    Toolbar.prototype.removeEventListener = function () {
        this.parent.observer.off(scroll, this.scrollHandler);
        this.parent.observer.off(refreshBegin, this.onRefresh);
        this.parent.observer.off(bindOnEnd, this.toolbarBindEvent);
        this.parent.observer.off(mouseDown, this.mouseDownHandler);
        this.parent.observer.off(focusChange, this.focusChangeHandler);
        this.parent.observer.off(toolbarUpdated, this.updateToolbarStatus);
        this.parent.observer.off(beforeDropDownOpen, this.dropDownBeforeOpenHandler);
    };
    Toolbar.prototype.wireEvents = function () {
        if (this.parent.inlineMode.enable && isIDevice()) {
            return;
        }
        this.tbElement = this.parent.getToolbarElement();
        if (this.tbElement) {
            sf.base.EventHandler.add(this.tbElement, 'focusin', this.tbFocusHandler, this);
            sf.base.EventHandler.add(this.tbElement, 'keydown', this.tbKeydownHandler, this);
        }
    };
    Toolbar.prototype.unWireEvents = function () {
        if (this.tbElement) {
            sf.base.EventHandler.remove(this.tbElement, 'focusin', this.tbFocusHandler);
            sf.base.EventHandler.remove(this.tbElement, 'keydown', this.tbKeydownHandler);
        }
    };
    Toolbar.prototype.toolbarBindEvent = function () {
        if (!this.parent.inlineMode.enable) {
            this.keyBoardModule = new sf.base.KeyboardEvents(this.parent.getToolbarElement(), {
                keyAction: this.toolBarKeyDown.bind(this), keyConfigs: this.parent.formatter.keyConfig, eventName: 'keydown'
            });
        }
    };
    //#endregion
    //#region Event handler methods
    Toolbar.prototype.onRefresh = function () {
        this.parent.dotNetRef.invokeMethodAsync(refreshToolbarOverflow);
        this.parent.setContentHeight('', true);
    };
    Toolbar.prototype.tbFocusHandler = function (e) {
        var activeElm = document.activeElement;
        var isToolbaractive = sf.base.closest(activeElm, '.' + CLS_TOOLBAR);
        if (activeElm === this.parent.getToolbarElement() || isToolbaractive === this.parent.getToolbarElement()) {
            var toolbarItem = this.parent.getToolbarElement().querySelectorAll('.' + CLS_EXPENDED_NAV);
            for (var i = 0; i < toolbarItem.length; i++) {
                if (sf.base.isNullOrUndefined(this.parent.getToolbarElement().querySelector('.' + CLS_INSERT_TABLE_BTN))) {
                    toolbarItem[i].setAttribute('tabindex', '0');
                }
                else {
                    toolbarItem[i].setAttribute('tabindex', '1');
                }
            }
        }
    };
    Toolbar.prototype.tbKeydownHandler = function (e) {
        if (e.target.classList.contains(CLS_DROP_DOWN_BTN) ||
            e.target.getAttribute('id') === this.parent.id + toolbarCreateTable) {
            e.target.setAttribute('tabindex', '0');
        }
    };
    Toolbar.prototype.toolBarKeyDown = function (e) {
        switch (e.action) {
            case 'escape':
                this.parent.getEditPanel().focus();
                break;
        }
    };
    Toolbar.prototype.scrollHandler = function (e) {
        if (!this.parent.inlineMode.enable) {
            this.tbElement = this.parent.getToolbarElement();
            if (this.parent.toolbarSettings.enableFloating && this.getDOMVisibility(this.tbElement)) {
                this.toggleFloatClass(e.args);
            }
        }
    };
    Toolbar.prototype.mouseDownHandler = function () {
        if (sf.base.Browser.isDevice && this.parent.inlineMode.enable && !isIDevice()) {
            this.showFixedTBar();
        }
    };
    Toolbar.prototype.focusChangeHandler = function () {
        if (sf.base.Browser.isDevice && this.parent.inlineMode.enable && !isIDevice()) {
            this.isToolbar = false;
            this.hideFixedTBar();
        }
    };
    return Toolbar;
}());

/**
 * Rich Text Editor classes defined here.
 */
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var CLS_TB_ITEM$1 = 'e-toolbar-item';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var CLS_IMGBREAK$1 = 'e-imgbreak';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var CLS_IMGINLINE$1 = 'e-imginline';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var CLS_ACTIVE$1 = 'e-active';
/**
 * @hidden
 * @deprecated
 */
var CLS_EXPAND_OPEN$1 = 'e-expand-open';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var CLS_RTE_IMAGE$1 = 'e-rte-image';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var CLS_TABLE$1 = 'e-rte-table';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * Defines common util methods used by Rich Text Editor.
 */
var inlineNode$1 = ['a', 'abbr', 'acronym', 'audio', 'b', 'bdi', 'bdo', 'big', 'br', 'button',
    'canvas', 'cite', 'code', 'data', 'datalist', 'del', 'dfn', 'em', 'embed', 'font', 'i', 'iframe', 'img', 'input',
    'ins', 'kbd', 'label', 'map', 'mark', 'meter', 'noscript', 'object', 'output', 'picture', 'progress',
    'q', 'ruby', 's', 'samp', 'script', 'select', 'slot', 'small', 'span', 'strong', 'strike', 'sub', 'sup', 'svg',
    'template', 'textarea', 'time', 'u', 'tt', 'var', 'video', 'wbr'];
/**
 * @returns {void}
 * @hidden
 */
function isIDevice$1() {
    var result = false;
    if (sf.base.Browser.isDevice && sf.base.Browser.isIos) {
        result = true;
    }
    return result;
}
/**
 * @param {Element} editableElement - specifies the editable element.
 * @param {string} selector - specifies the string values.
 * @returns {void}
 * @hidden
 */
function setEditFrameFocus(editableElement, selector) {
    if (editableElement.nodeName === 'BODY' && !sf.base.isNullOrUndefined(selector)) {
        var iframe = top.window.document.querySelector(selector);
        if (!sf.base.isNullOrUndefined(iframe)) {
            iframe.contentWindow.focus();
        }
    }
}
/**
 * @param {string} value - specifies the string value
 * @returns {void}
 * @hidden
 */
function updateTextNode$1(value) {
    var tempNode = document.createElement('div');
    tempNode.innerHTML = value;
    tempNode.setAttribute('class', 'tempDiv');
    var resultElm = document.createElement('div');
    var childNodes = tempNode.childNodes;
    if (childNodes.length > 0) {
        var isPreviousInlineElem = void 0;
        var previousParent = void 0;
        var paraElm = void 0;
        while (tempNode.firstChild) {
            if ((tempNode.firstChild.nodeName === '#text' &&
                (tempNode.firstChild.textContent.indexOf('\n') < 0 || tempNode.firstChild.textContent.trim() !== '')) ||
                inlineNode$1.indexOf(tempNode.firstChild.nodeName.toLocaleLowerCase()) >= 0) {
                if (!isPreviousInlineElem) {
                    paraElm = sf.base.createElement('p');
                    resultElm.appendChild(paraElm);
                    paraElm.appendChild(tempNode.firstChild);
                }
                else {
                    previousParent.appendChild(tempNode.firstChild);
                }
                previousParent = paraElm;
                isPreviousInlineElem = true;
            }
            else if (tempNode.firstChild.nodeName === '#text' && (tempNode.firstChild.textContent === '\n' ||
                (tempNode.firstChild.textContent.indexOf('\n') >= 0 && tempNode.firstChild.textContent.trim() === ''))) {
                sf.base.detach(tempNode.firstChild);
            }
            else {
                resultElm.appendChild(tempNode.firstChild);
                isPreviousInlineElem = false;
            }
        }
        var tableElm = resultElm.querySelectorAll('table');
        for (var i = 0; i < tableElm.length; i++) {
            if (tableElm[i].getAttribute('border') === '0') {
                tableElm[i].removeAttribute('border');
            }
            var tdElm = tableElm[i].querySelectorAll('td');
            for (var j = 0; j < tdElm.length; j++) {
                if (tdElm[j].style.borderLeft === 'none') {
                    tdElm[j].style.removeProperty('border-left');
                }
                if (tdElm[j].style.borderRight === 'none') {
                    tdElm[j].style.removeProperty('border-right');
                }
                if (tdElm[j].style.borderBottom === 'none') {
                    tdElm[j].style.removeProperty('border-bottom');
                }
                if (tdElm[j].style.borderTop === 'none') {
                    tdElm[j].style.removeProperty('border-top');
                }
                if (tdElm[j].style.border === 'none') {
                    tdElm[j].style.removeProperty('border');
                }
            }
            if (!tableElm[i].classList.contains(CLS_TABLE$1)) {
                tableElm[i].classList.add(CLS_TABLE$1);
            }
        }
        var imageElm = resultElm.querySelectorAll('img');
        for (var i = 0; i < imageElm.length; i++) {
            if (!imageElm[i].classList.contains(CLS_RTE_IMAGE$1)) {
                imageElm[i].classList.add(CLS_RTE_IMAGE$1);
            }
            if (!(imageElm[i].classList.contains(CLS_IMGINLINE$1) ||
                imageElm[i].classList.contains(CLS_IMGBREAK$1))) {
                imageElm[i].classList.add(CLS_IMGINLINE$1);
            }
        }
    }
    return resultElm.innerHTML;
}
/**
 * @param {Node} startChildNodes - specifies the node
 * @returns {void}
 * @hidden
 */
function getLastTextNode(startChildNodes) {
    var finalNode = startChildNodes;
    do {
        if (finalNode.childNodes.length > 0) {
            finalNode = finalNode.childNodes[0];
        }
    } while (finalNode.childNodes.length > 0);
    return finalNode;
}
/**
 * @returns {void}
 * @hidden
 */
function getDefaultHtmlTbStatus() {
    return {
        bold: false,
        italic: false,
        subscript: false,
        superscript: false,
        strikethrough: false,
        orderedlist: false,
        unorderedlist: false,
        underline: false,
        alignments: null,
        backgroundcolor: null,
        fontcolor: null,
        fontname: null,
        fontsize: null,
        formats: null,
        createlink: false,
        insertcode: false
    };
}
/**
 * @returns {void}
 * @hidden
 */
function getDefaultMDTbStatus() {
    return {
        bold: false,
        italic: false,
        subscript: false,
        superscript: false,
        strikethrough: false,
        orderedlist: false,
        uppercase: false,
        lowercase: false,
        inlinecode: false,
        unorderedlist: false,
        formats: null
    };
}

var __assign$1 = (undefined && undefined.__assign) || function () {
    __assign$1 = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign$1.apply(this, arguments);
};
/**
 * `Link` module is used to handle undo actions.
 */
var Link = /** @class */ (function () {
    function Link(parent) {
        this.parent = parent;
        this.rteId = this.parent.element.id;
        this.addEventListener();
    }
    Link.prototype.addEventListener = function () {
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(keyDown, this.onKeyDown, this);
        this.parent.observer.on(unLink, this.removeLink, this);
        this.parent.observer.on(insertLink, this.linkDialog, this);
        this.parent.observer.on(linkToolbarAction, this.onToolbarAction, this);
        this.parent.observer.on(iframeMouseDown, this.onIframeMouseDown, this);
        this.parent.observer.on(editAreaClick, this.editAreaClickHandler, this);
        this.parent.observer.on(insertCompleted, this.showLinkQuickToolbar, this);
    };
    Link.prototype.removeEventListener = function () {
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(keyDown, this.onKeyDown);
        this.parent.observer.off(unLink, this.removeLink);
        this.parent.observer.off(insertLink, this.linkDialog);
        this.parent.observer.off(linkToolbarAction, this.onToolbarAction);
        this.parent.observer.off(iframeMouseDown, this.onIframeMouseDown);
        this.parent.observer.off(editAreaClick, this.editAreaClickHandler);
        this.parent.observer.off(insertCompleted, this.showLinkQuickToolbar);
        sf.base.EventHandler.remove(this.parent.element.ownerDocument, 'mousedown', this.onDocumentClick);
    };
    Link.prototype.linkDialog = function (e, inputDetails) {
        if (this.parent.editorMode === 'HTML' && (e.selectParent.length > 0 && !sf.base.isNullOrUndefined(e.selectParent[0].classList) &&
            e.selectParent[0].classList.contains('e-rte-anchor')) && sf.base.isNullOrUndefined(inputDetails)) {
            this.editLink(e);
            return;
        }
        this.selectionObj = { selection: e.selection, selectParent: e.selectParent, args: e.args };
        var model = { url: '', text: '', title: '', target: true };
        if (!sf.base.isNullOrUndefined(inputDetails)) {
            model = {
                url: inputDetails.url, text: inputDetails.text,
                title: inputDetails.title, target: (inputDetails.target !== '' ? true : false)
            };
        }
        var selectText = (this.parent.editorMode === 'HTML') ? e.selection.getRange(this.parent.getDocument()).toString() : e.text;
        if ((this.parent.editorMode === 'HTML' && sf.base.isNullOrUndefined(inputDetails) && ((!sf.base.isNullOrUndefined(selectText) && selectText !== '') &&
            (e.selection.range.startOffset === 0) || e.selection.range.startOffset !== e.selection.range.endOffset)) ||
            e.module === 'Markdown') {
            model.text = selectText;
        }
        sf.base.EventHandler.add(this.parent.element.ownerDocument, 'mousedown', this.onDocumentClick, this);
        if (sf.base.isNullOrUndefined(sf.base.select('.e-rte-link-dialog', this.parent.element))) {
            this.parent.dotNetRef.invokeMethodAsync('ShowLinkDialog', model, (!sf.base.isNullOrUndefined(inputDetails) ? 'Edit' : null));
        }
        else {
            this.parent.dotNetRef.invokeMethodAsync('CloseLinkDialog');
        }
        if (this.quickToolObj) {
            this.hideLinkQuickToolbar();
            this.quickToolObj.hideInlineQTBar();
        }
    };
    Link.prototype.insertLink = function (args) {
        var linkTitle;
        var linkUrl = args.url;
        var linkText = args.text;
        if (this.parent.editorMode === 'HTML') {
            linkTitle = args.title;
        }
        var target = args.target ? '_blank' : null;
        if (linkUrl === '') {
            this.checkUrl(true);
            return;
        }
        if (!this.isUrl(linkUrl)) {
            linkText = (linkText === '') ? linkUrl : linkText;
            linkUrl = (!this.parent.enableAutoUrl) ? (linkUrl.indexOf('http') > -1 ? linkUrl : 'http://' + linkUrl) : linkUrl;
        }
        else {
            this.checkUrl(false);
        }
        if (this.parent.editorMode === 'HTML' && sf.base.isNullOrUndefined(sf.base.closest(this.selectionObj.selection.range.startContainer.parentNode, '#' + this.parent.getPanel().id))) {
            this.parent.getEditPanel().focus();
            if (sf.base.Browser.isIE && this.parent.iframeSettings.enable) {
                this.selectionObj.selection.restore();
            }
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            this.selectionObj.selection = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
            this.selectionObj.selectParent = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
        }
        var value = {
            url: linkUrl, text: linkText, title: linkTitle, target: target,
            selection: this.selectionObj.selection, selectParent: this.selectionObj.selectParent
        };
        this.parent.dotNetRef.invokeMethodAsync('CloseLinkDialog');
        if (isIDevice$1() && this.parent.iframeSettings.enable) {
            sf.base.select('iframe', this.parent.element).contentWindow.focus();
        }
        if (this.parent.editorMode === 'HTML') {
            this.selectionObj.selection.restore();
        }
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        var argsValue;
        if (!sf.base.isNullOrUndefined(this.selectionObj.args) && this.selectionObj.args.code === 'KeyK') {
            var event_1 = this.selectionObj.args;
            event_1 = __assign$1({}, event_1, { target: null });
            var argsVal = { item: { command: 'Links', subCommand: 'CreateLink' }, originalEvent: event_1 };
            sf.base.extend(this.selectionObj.args, argsVal, true);
            argsValue = argsVal;
        }
        else {
            argsValue = this.selectionObj.args;
        }
        this.parent.formatter.process(this.parent, argsValue, (!sf.base.isNullOrUndefined(this.selectionObj.args) &&
            this.selectionObj.args.originalEvent), value);
        this.parent.getEditPanel().focus();
    };
    Link.prototype.isUrl = function (url) {
        var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/gi;
        return regexp.test(url);
    };
    Link.prototype.checkUrl = function (e) {
        var linkUrl = this.parent.element.querySelector('.e-rte-link-dialog .e-rte-linkurl');
        if (e) {
            sf.base.addClass([linkUrl], 'e-error');
            linkUrl.setSelectionRange(0, linkUrl.value.length);
            linkUrl.focus();
        }
        else {
            sf.base.removeClass([linkUrl], 'e-error');
        }
    };
    Link.prototype.cancelDialog = function () {
        if (isIDevice$1()) {
            this.selectionObj.selection.restore();
        }
        else {
            this.parent.getEditPanel().focus();
        }
    };
    Link.prototype.linkDialogClosed = function () {
        if (this.parent.editorMode === 'HTML') {
            this.selectionObj.selection.restore();
        }
        else {
            this.parent.formatter.editorManager.markdownSelection.restore(this.parent.getEditPanel());
        }
    };
    Link.prototype.getAnchorNode = function (element) {
        var selectParent = sf.base.closest(element, 'a');
        return (selectParent ? selectParent : element);
    };
    Link.prototype.openLink = function (e) {
        var selectParentEle = this.getAnchorNode(e.selectParent[0]);
        if (selectParentEle.classList.contains('e-rte-anchor') || selectParentEle.tagName === 'A') {
            this.parent.formatter.process(this.parent, e.args, e.args, {
                url: selectParentEle.href,
                target: selectParentEle.target === '' ? '_self' : '_blank', selectNode: e.selectNode,
                subCommand: e.args.item.subCommand
            });
        }
    };
    Link.prototype.editLink = function (e) {
        var selectedNode = this.getAnchorNode(e.selectNode[0]);
        var selectParentEle = this.getAnchorNode(e.selectParent[0]);
        selectParentEle = selectedNode.nodeName === 'A' ? selectedNode : selectParentEle;
        if (selectParentEle.classList.contains('e-rte-anchor') || selectParentEle.tagName === 'A') {
            var inputDetails = {
                url: selectParentEle.getAttribute('href'), text: selectParentEle.innerText,
                title: selectParentEle.title, target: selectParentEle.target,
            };
            this.linkDialog(e, inputDetails);
        }
    };
    Link.prototype.removeLink = function (e) {
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        this.parent.formatter.process(this.parent, e.args, e.args, {
            selectNode: e.selectNode, selectParent: e.selectParent, selection: e.selection,
            subCommand: e.args.item.subCommand
        });
        if (isIDevice$1() && this.parent.iframeSettings.enable) {
            sf.base.select('iframe', this.parent.element).contentWindow.focus();
        }
        else {
            this.parent.getEditPanel().focus();
        }
        this.hideLinkQuickToolbar();
    };
    Link.prototype.onToolbarAction = function (args) {
        var item = args.args.item;
        switch (item.subCommand) {
            case 'OpenLink':
                this.openLink(args);
                break;
            case 'EditLink':
                this.editLink(args);
                break;
            case 'RemoveLink':
                this.removeLink(args);
                break;
        }
    };
    Link.prototype.editAreaClickHandler = function (e) {
        if (this.parent.readonly) {
            this.hideLinkQuickToolbar();
            return;
        }
        var args = e.args;
        var showOnRightClick = this.parent.quickToolbarSettings.showOnRightClick;
        if (args.which === 2 || (showOnRightClick && args.which === 1) || (!showOnRightClick && args.which === 3)) {
            return;
        }
        if (this.parent.editorMode === 'HTML' && this.parent.quickToolbarModule) {
            var target = args.target;
            target = this.getAnchorNode(target);
            var isPopupOpen = void 0;
            isPopupOpen = document.body.querySelector('#' + this.rteId + '_Link_Quick_Popup').classList.contains('e-rte-pop');
            if (target.nodeName === 'A' && (target.childNodes.length > 0 && target.childNodes[0].nodeName !== 'IMG') &&
                e.args.target.nodeName !== 'IMG') {
                if (isPopupOpen) {
                    return;
                }
                this.showLinkQuickToolbar({
                    args: args,
                    isNotify: false,
                    type: 'Links',
                    elements: [args.target]
                });
            }
            else {
                this.hideLinkQuickToolbar();
            }
        }
    };
    Link.prototype.showLinkQuickToolbar = function (e) {
        if (e.args.action !== 'enter' && e.args.action !== 'space') {
            var pageX = void 0;
            var pageY = void 0;
            if (e.type !== 'Links' || sf.base.isNullOrUndefined(this.parent.quickToolbarModule)) {
                return;
            }
            this.quickToolObj = this.parent.quickToolbarModule;
            var parentTop = this.parent.element.getBoundingClientRect().top;
            var parentLeft = this.parent.element.getBoundingClientRect().left;
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            var tbElement = this.parent.getToolbarElement();
            var tbHeight = (tbElement) ? (tbElement.offsetHeight + this.parent.toolbarModule.getExpandTBarPopHeight()) : 0;
            var target_1;
            [].forEach.call(e.elements, function (element, index) {
                if (index === 0) {
                    target_1 = ((element.nodeName === '#text') ? (element.parentNode) : element);
                }
            });
            if (e.isNotify) {
                var targetRect = target_1.getBoundingClientRect();
                var linkTop = targetRect.top;
                var linkLeft = targetRect.left;
                var linkPos = linkTop - parentTop;
                pageX = this.parent.iframeSettings.enable ? parentLeft + linkLeft : linkLeft;
                pageY = window.pageYOffset + (this.parent.iframeSettings.enable ?
                    (parentTop + tbHeight + linkTop) : (parentTop + linkPos));
            }
            else {
                var args = void 0;
                args = e.args.touches ? e.args.changedTouches[0] : args = e.args;
                pageX = this.parent.iframeSettings.enable ? window.pageXOffset + parentLeft + args.clientX : args.pageX;
                pageY = this.parent.iframeSettings.enable ? window.pageYOffset + parentTop + tbHeight + args.clientY : args.pageY;
            }
            this.quickToolObj.showLinkQTBar(pageX, pageY, range.endContainer, 'Link');
        }
    };
    Link.prototype.hideLinkQuickToolbar = function () {
        if (this.quickToolObj) {
            this.quickToolObj.hideLinkQTBar();
        }
    };
    Link.prototype.showDialog = function (isExternal, event) {
        if (isExternal) {
            this.parent.getEditPanel().focus();
        }
        if (this.parent.editorMode === 'HTML') {
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            var save = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
            var selectNodeEle = this.parent.formatter.editorManager.nodeSelection.getNodeCollection(range);
            var selectParentEle = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
            var eventArgs = {
                args: (isExternal ? { item: { command: 'Links', subCommand: 'CreateLink' },
                    originalEvent: undefined } : event.args),
                selectNode: selectNodeEle, selection: save, selectParent: selectParentEle
            };
            this.linkDialog(eventArgs);
        }
        else {
            var textArea = this.parent.getEditPanel();
            this.parent.formatter.editorManager.markdownSelection.save(textArea.selectionStart, textArea.selectionEnd);
            this.linkDialog({
                args: {
                    item: { command: 'Links', subCommand: 'Link' },
                    originalEvent: isExternal ? undefined : event.args
                },
                member: 'link',
                text: this.parent.formatter.editorManager.markdownSelection.getSelectedText(this.parent.getEditPanel()),
                module: 'Markdown',
                name: 'insertLink'
            });
        }
    };
    Link.prototype.onKeyDown = function (event) {
        var originalEvent = event.args;
        switch (originalEvent.action) {
            case 'escape':
                this.parent.dotNetRef.invokeMethodAsync('CloseLinkDialog');
                break;
            case 'insert-link':
                this.showDialog(false, event);
                originalEvent.preventDefault();
                break;
        }
    };
    Link.prototype.onDocumentClick = function (e) {
        var target = e.target;
        var dlgId = '#' + this.rteId + '_rtelink';
        var tbEle = this.parent.getToolbarElement();
        var tbEleId = '#' + this.rteId + '_toolbar_CreateLink';
        var linkDlgEle = this.parent.element.querySelector(dlgId);
        if (!sf.base.isNullOrUndefined(linkDlgEle) && ((!sf.base.closest(target, dlgId) && this.parent.toolbarSettings.enable &&
            tbEle && !tbEle.contains(e.target)) || (((tbEle && tbEle.contains(e.target)) ||
            this.parent.inlineMode.enable && !sf.base.closest(target, dlgId)) && !sf.base.closest(target, tbEleId) && !target.querySelector(tbEleId)))) {
            this.parent.dotNetRef.invokeMethodAsync('CloseLinkDialog');
            sf.base.EventHandler.remove(this.parent.element.ownerDocument, 'mousedown', this.onDocumentClick);
            this.parent.isBlur = true;
            if (sf.base.Browser.isIE) {
                dispatchEvent(this.parent.element, 'focusout');
            }
        }
    };
    Link.prototype.onIframeMouseDown = function () {
        this.parent.dotNetRef.invokeMethodAsync('CloseLinkDialog');
    };
    Link.prototype.destroy = function () {
        this.removeEventListener();
    };
    return Link;
}());

/**
 * `Table` module is used to handle table actions.
 */
var Table = /** @class */ (function () {
    function Table(parent) {
        this.pageX = null;
        this.pageY = null;
        this.moveEle = null;
        this.rzProgress = false;
        this.ensureInsideTableList = true;
        this.parent = parent;
        this.addEventListener();
    }
    Table.prototype.addEventListener = function () {
        this.parent.observer.on(keyUp, this.keyUp, this);
        this.parent.observer.on(keyDown, this.keyDown, this);
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(docClick, this.docClick, this);
        this.parent.observer.on(initialEnd, this.afterRender, this);
        this.parent.observer.on(mouseUp, this.selectionTable, this);
        this.parent.observer.on(createTable, this.renderDlgContent, this);
        this.parent.observer.on(dropDownSelect, this.dropdownSelect, this);
        this.parent.observer.on(iframeMouseDown, this.onIframeMouseDown, this);
        this.parent.observer.on(editAreaClick, this.editAreaClickHandler, this);
        this.parent.observer.on(tableToolbarAction, this.onToolbarAction, this);
    };
    Table.prototype.removeEventListener = function () {
        this.parent.observer.off(keyUp, this.keyUp);
        this.parent.observer.off(keyDown, this.keyDown);
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(docClick, this.docClick);
        this.parent.observer.off(mouseDown, this.cellSelect);
        this.parent.observer.off(initialEnd, this.afterRender);
        this.parent.observer.off(mouseUp, this.selectionTable);
        this.parent.observer.off(createTable, this.renderDlgContent);
        this.parent.observer.off(dropDownSelect, this.dropdownSelect);
        this.parent.observer.off(iframeMouseDown, this.onIframeMouseDown);
        this.parent.observer.off(tableColorPickerChanged, this.setBGColor);
        this.parent.observer.off(editAreaClick, this.editAreaClickHandler);
        this.parent.observer.off(tableToolbarAction, this.onToolbarAction);
    };
    Table.prototype.afterRender = function () {
        this.parent.observer.on(mouseDown, this.cellSelect, this);
        this.parent.observer.on(tableColorPickerChanged, this.setBGColor, this);
        if (this.parent.tableSettings.resize) {
            sf.base.EventHandler.add(this.parent.getEditPanel(), sf.base.Browser.touchStartEvent, this.resizeStart, this);
        }
        if (!sf.base.Browser.isDevice && this.parent.tableSettings.resize) {
            sf.base.EventHandler.add(this.parent.getEditPanel(), 'mouseover', this.resizeHelper, this);
        }
    };
    Table.prototype.selectionTable = function (e) {
        var target = e.args.target;
        if (sf.base.Browser.info.name === 'mozilla' && !sf.base.isNullOrUndefined(sf.base.closest(target, 'table')) && sf.base.closest(target, 'table').tagName === 'TABLE') {
            this.parent.getEditPanel().setAttribute('contenteditable', 'true');
        }
    };
    Table.prototype.tabSelection = function (event, selection, ele) {
        var insideList = this.insideList(selection.range);
        if ((event.keyCode === 37 || event.keyCode === 39) && selection.range.startContainer.nodeType === 3 ||
            insideList) {
            return;
        }
        event.preventDefault();
        ele.classList.remove(CLS_TABLE_SEL);
        if (!event.shiftKey && event.keyCode !== 37) {
            var nextElement = (!sf.base.isNullOrUndefined(ele.nextSibling)) ? ele.nextSibling :
                (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'tr').nextSibling) ? sf.base.closest(ele, 'tr').nextSibling.childNodes[0] :
                    (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'table').nextSibling)) ?
                        (sf.base.closest(ele, 'table').nextSibling.nodeName.toLowerCase() === 'td') ?
                            sf.base.closest(ele, 'table').nextSibling : ele : ele);
            if (ele === nextElement && ele.nodeName === 'TH') {
                nextElement = sf.base.closest(ele, 'table').rows[1].cells[0];
            }
            if (event.keyCode === 39 && ele === nextElement) {
                nextElement = sf.base.closest(ele, 'table').nextSibling;
            }
            if (nextElement) {
                (nextElement.textContent.trim() !== '' && sf.base.closest(nextElement, 'td')) ?
                    selection.setSelectionNode(this.parent.getDocument(), nextElement) :
                    selection.setSelectionText(this.parent.getDocument(), nextElement, nextElement, 0, 0);
            }
            if (ele === nextElement && event.keyCode !== 39 && nextElement) {
                ele.classList.add(CLS_TABLE_SEL);
                this.addRow(selection, event, true);
                ele.classList.remove(CLS_TABLE_SEL);
                nextElement = nextElement.parentElement.nextSibling.firstChild;
                (nextElement.textContent.trim() !== '' && sf.base.closest(nextElement, 'td')) ?
                    selection.setSelectionNode(this.parent.getDocument(), nextElement) :
                    selection.setSelectionText(this.parent.getDocument(), nextElement, nextElement, 0, 0);
            }
        }
        else {
            var prevElement = (!sf.base.isNullOrUndefined(ele.previousSibling)) ? ele.previousSibling :
                (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'tr').previousSibling) ?
                    sf.base.closest(ele, 'tr').previousSibling.childNodes[sf.base.closest(ele, 'tr').previousSibling.childNodes.length - 1] :
                    (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'table').previousSibling)) ?
                        (sf.base.closest(ele, 'table').previousSibling.nodeName.toLowerCase() === 'td') ? sf.base.closest(ele, 'table').previousSibling :
                            ele : ele);
            if (ele === prevElement && ele.cellIndex === 0 &&
                sf.base.closest(ele, 'table').tHead) {
                var clsTable = sf.base.closest(ele, 'table');
                prevElement = clsTable.rows[0].cells[clsTable.rows[0].cells.length - 1];
            }
            if (event.keyCode === 37 && ele === prevElement) {
                prevElement = sf.base.closest(ele, 'table').previousSibling;
            }
            if (prevElement) {
                (prevElement.textContent.trim() !== '' && sf.base.closest(prevElement, 'td')) ?
                    selection.setSelectionNode(this.parent.getDocument(), prevElement) :
                    selection.setSelectionText(this.parent.getDocument(), prevElement, prevElement, 0, 0);
            }
        }
    };
    Table.prototype.insideList = function (range) {
        var blockNodes = this.parent.formatter.editorManager.domNode.blockNodes();
        var nodes = [];
        for (var i = 0; i < blockNodes.length; i++) {
            if (blockNodes[i].parentNode.tagName === 'LI') {
                nodes.push(blockNodes[i].parentNode);
            }
            else if (blockNodes[i].tagName === 'LI' && blockNodes[i].childNodes[0].tagName !== 'P' &&
                (blockNodes[i].childNodes[0].tagName !== 'OL' &&
                    blockNodes[i].childNodes[0].tagName !== 'UL')) {
                nodes.push(blockNodes[i]);
            }
        }
        if (nodes.length > 1 || nodes.length && ((range.startOffset === 0 && range.endOffset === 0))) {
            this.ensureInsideTableList = true;
            return true;
        }
        else {
            this.ensureInsideTableList = false;
            return false;
        }
    };
    Table.prototype.tableArrowNavigation = function (event, selection, ele) {
        var selText = selection.range.startContainer;
        if ((event.keyCode === 40 && selText.nodeType === 3 && (selText.nextSibling && selText.nextSibling.nodeName === 'BR' ||
            selText.parentNode && selText.parentNode.nodeName !== 'TD')) ||
            (event.keyCode === 38 && selText.nodeType === 3 && (selText.previousSibling && selText.previousSibling.nodeName === 'BR' ||
                selText.parentNode && selText.parentNode.nodeName !== 'TD'))) {
            return;
        }
        event.preventDefault();
        ele.classList.remove(CLS_TABLE_SEL);
        if (event.keyCode === 40) {
            ele = (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'tr').nextElementSibling)) ?
                sf.base.closest(ele, 'tr').nextElementSibling.children[ele.cellIndex] :
                (sf.base.closest(ele, 'table').tHead && ele.nodeName === 'TH') ?
                    sf.base.closest(ele, 'table').rows[1].cells[ele.cellIndex] :
                    (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'table').nextSibling)) ? sf.base.closest(ele, 'table').nextSibling :
                        ele;
        }
        else {
            ele = (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'tr').previousElementSibling)) ?
                sf.base.closest(ele, 'tr').previousElementSibling.children[ele.cellIndex] :
                (sf.base.closest(ele, 'table').tHead && ele.nodeName !== 'TH') ?
                    sf.base.closest(ele, 'table').tHead.rows[0].cells[ele.cellIndex] :
                    (!sf.base.isNullOrUndefined(sf.base.closest(ele, 'table').previousSibling)) ? sf.base.closest(ele, 'table').previousSibling :
                        ele;
        }
        if (ele) {
            selection.setSelectionText(this.parent.getDocument(), ele, ele, 0, 0);
        }
    };
    Table.prototype.cellSelect = function (e) {
        var target = e.args.target;
        var tdNode = sf.base.closest(target, 'td,th');
        target = (target.nodeName !== 'TD' && tdNode && this.parent.getEditPanel().contains(tdNode)) ?
            tdNode : target;
        sf.base.removeClass(this.parent.getEditPanel().querySelectorAll('table td, table th'), CLS_TABLE_SEL);
        if (target && (target.tagName === 'TD' || target.tagName === 'TH')) {
            target.removeAttribute('class');
            sf.base.addClass([target], CLS_TABLE_SEL);
            this.curTable = (this.curTable) ? this.curTable : sf.base.closest(target, 'table');
            this.removeResizeEle();
            if (this.helper && this.parent.getEditPanel().contains(this.helper)) {
                sf.base.detach(this.helper);
            }
        }
        if (sf.base.Browser.info.name === 'mozilla' && !sf.base.isNullOrUndefined(sf.base.closest(target, 'table')) &&
            sf.base.closest(target, 'table').tagName === 'TABLE' && this.parent.getEditPanel().contains(sf.base.closest(target, 'table'))) {
            this.parent.getEditPanel().setAttribute('contenteditable', 'false');
        }
    };
    Table.prototype.renderDlgContent = function (args) {
        this.tableNotifyArgs = args;
        this.parent.observer.notify(selectionSave, {});
        if (sf.base.Browser.isDevice || this.parent.inlineMode.enable) {
            this.insertTableDialog();
            return;
        }
        this.hideTableQuickToolbar();
        var tbEle = this.parent.getToolbarElement();
        var tbTableEle = this.parent.element.querySelector('#' + this.parent.element.id + '_toolbar_CreateTable');
        var top = 0;
        if (this.parent.getToolbar().classList.contains(CLS_EXPAND_OPEN)) {
            top = tbTableEle.offsetTop + tbTableEle.offsetHeight + tbEle.offsetHeight;
        }
        else {
            top = tbTableEle.offsetTop + tbTableEle.offsetHeight;
        }
        if (sf.base.isNullOrUndefined(sf.base.select('.e-rte-table-popup', this.parent.element))) {
            this.parent.dotNetRef.invokeMethodAsync('ShowCreateTableDialog', tbTableEle.offsetLeft, top);
        }
        else {
            this.parent.dotNetRef.invokeMethodAsync('CloseCreateTableDialog');
        }
    };
    Table.prototype.createTablePopupOpened = function () {
        var rowElements = sf.base.selectAll('.e-rte-table-popup .e-rte-table-row', this.parent.element);
        for (var i = 0; i < rowElements.length; i++) {
            sf.base.EventHandler.add(rowElements[i], 'mouseleave', this.tableCellLeave, this);
            var tableCells = sf.base.selectAll('.e-rte-tablecell', rowElements[i]);
            for (var j = 0; j < tableCells.length; j++) {
                sf.base.EventHandler.add(tableCells[j], 'mouseup', this.tableCellClick, this);
                sf.base.EventHandler.add(tableCells[j], 'mousemove', this.tableCellSelect, this);
            }
        }
        var dlgEle = this.parent.element.querySelector('.e-rte-table-popup');
        dlgEle && dlgEle.focus();
        if (!sf.base.isNullOrUndefined(this.parent.getToolbarElement().querySelector('.e-expended-nav'))) {
            this.parent.getToolbarElement().querySelector('.e-expended-nav').setAttribute('tabindex', '1');
        }
    };
    Table.prototype.insertTableDialog = function () {
        this.createDialog();
    };
    Table.prototype.createDialog = function (model, mode) {
        if (model && mode) {
            this.parent.dotNetRef.invokeMethodAsync('ShowEditTableDialog', model, mode);
        }
        else {
            this.parent.dotNetRef.invokeMethodAsync('ShowTableDialog');
        }
        if (this.quickToolObj && this.quickToolObj.inlineQTBar && document.body.contains(this.quickToolObj.inlineQTBar.element)) {
            this.quickToolObj.hideInlineQTBar();
        }
    };
    Table.prototype.tableCellSelect = function (e) {
        var target = e.target;
        var dlgDiv = this.parent.element.querySelector('.e-rte-table-popup');
        var tblHeader = this.parent.element.querySelector('.e-rte-table-popup .e-rte-popup-header');
        var row = Array.prototype.slice.call(target.parentElement.parentElement.children).indexOf(target.parentElement);
        var col = Array.prototype.slice.call(target.parentElement.children).indexOf(target);
        var list = dlgDiv.querySelectorAll('.e-rte-tablecell');
        Array.prototype.forEach.call(list, function (item) {
            var parentIndex = Array.prototype.slice.call(item.parentElement.parentElement.children).indexOf(item.parentElement);
            var cellIndex = Array.prototype.slice.call(item.parentElement.children).indexOf(item);
            sf.base.removeClass([item], 'e-active');
            if (parentIndex <= row && cellIndex <= col) {
                sf.base.addClass([item], 'e-active');
            }
        });
        tblHeader.innerHTML = (col + 1) + 'x' + (row + 1);
    };
    Table.prototype.tableCellLeave = function (e) {
        var dlgDiv = this.parent.element.querySelector('.e-rte-table-popup');
        var tblHeader = this.parent.element.querySelector('.e-rte-table-popup .e-rte-popup-header');
        sf.base.removeClass(dlgDiv.querySelectorAll('.e-rte-tablecell'), 'e-active');
        sf.base.addClass([dlgDiv.querySelector('.e-rte-tablecell')], 'e-active');
        tblHeader.innerHTML = 1 + 'x' + 1;
    };
    Table.prototype.tableCellClick = function (e) {
        var target = e.target;
        var row = Array.prototype.slice.call(target.parentElement.parentElement.children).indexOf(target.parentElement) + 1;
        var col = Array.prototype.slice.call(target.parentElement.children).indexOf(target) + 1;
        this.tableInsert(row, col, 'Create', this);
    };
    Table.prototype.tableInsert = function (row, col, dlgTarget, selectionObj) {
        var proxy = (selectionObj.self) ? selectionObj.self : this;
        var startContainer = this.tableNotifyArgs.selection.range.startContainer;
        if (startContainer.nodeName === 'P' && startContainer.textContent.trim() === '' && !(startContainer.childNodes.length > 0)) {
            startContainer.innerHTML = '<br />';
        }
        var parentNode = startContainer.parentNode;
        if (proxy.parent.editorMode === 'HTML' &&
            ((proxy.parent.iframeSettings.enable && !hasClass(parentNode.ownerDocument.querySelector('body'), 'e-lib')) ||
                (!proxy.parent.iframeSettings.enable && sf.base.isNullOrUndefined(sf.base.closest(parentNode, '#' + proxy.parent.getPanel().id))))) {
            proxy.parent.getEditPanel().focus();
            var range = proxy.parent.formatter.editorManager.nodeSelection.getRange(proxy.parent.getDocument());
            this.tableNotifyArgs.selection = proxy.parent.formatter.editorManager.nodeSelection.save(range, proxy.parent.getDocument());
        }
        var value = {
            rows: row, columns: col, width: {
                minWidth: proxy.parent.tableSettings.minWidth,
                maxWidth: proxy.parent.tableSettings.maxWidth,
                width: proxy.parent.tableSettings.width,
            },
            selection: this.tableNotifyArgs.selection
        };
        if (dlgTarget === 'Create') {
            this.parent.dotNetRef.invokeMethodAsync('CloseCreateTableDialog');
        }
        else {
            this.parent.dotNetRef.invokeMethodAsync('CloseTableDialog');
        }
        this.parent.observer.notify(selectionRestore, {});
        proxy.parent.formatter.process(proxy.parent, this.tableNotifyArgs.args, this.tableNotifyArgs.args.originalEvent, value);
        proxy.parent.getEditPanel().focus();
        proxy.parent.observer.on(mouseDown, proxy.cellSelect, proxy);
    };
    Table.prototype.customTable = function (rowValue, columnValue) {
        if (rowValue && columnValue) {
            var argument = ((sf.base.Browser.isDevice || (!sf.base.isNullOrUndefined(this.tableNotifyArgs.args)
                && !sf.base.isNullOrUndefined(this.tableNotifyArgs.args.originalEvent) &&
                this.tableNotifyArgs.args.originalEvent.action === 'insert-table')
                || this.parent.inlineMode.enable) ? this.tableNotifyArgs : this);
            this.tableInsert(rowValue, columnValue, '', argument);
        }
    };
    Table.prototype.applyTableProperties = function (model) {
        var table = sf.base.closest(this.tableNotifyArgs.selectNode[0], 'table');
        table.style.width = model.width + 'px';
        if (model.padding.toString() !== '') {
            var thElm = table.querySelectorAll('th');
            this.applyCellPadding(thElm, model.padding);
            var tdElm = table.querySelectorAll('td');
            this.applyCellPadding(tdElm, model.padding);
        }
        table.cellSpacing = model.spacing.toString();
        if (table.cellSpacing && table.cellSpacing !== '0') {
            sf.base.addClass([table], CLS_TABLE_BORDER);
        }
        else {
            sf.base.removeClass([table], CLS_TABLE_BORDER);
        }
        this.parent.formatter.saveData();
        this.parent.dotNetRef.invokeMethodAsync('CloseTableDialog');
    };
    Table.prototype.applyCellPadding = function (elements, padding) {
        for (var i = 0; i < elements.length; i++) {
            var padVal = '';
            if (elements[i].style.padding === '') {
                var styles = elements[i].getAttribute('style');
                padVal = (!sf.base.isNullOrUndefined(styles) ? styles : '') + ' padding:' + padding + 'px;';
            }
            else {
                elements[i].style.padding = padding + 'px';
                padVal = elements[i].getAttribute('style');
            }
            elements[i].setAttribute('style', padVal);
        }
    };
    //#region Resize methods
    Table.prototype.resizeHelper = function (e) {
        if (this.parent.readonly) {
            return;
        }
        var target = e.target || e.targetTouches[0].target;
        var closestTable = sf.base.closest(target, 'table');
        if (!sf.base.isNullOrUndefined(this.curTable) && !sf.base.isNullOrUndefined(closestTable) && closestTable !== this.curTable) {
            this.removeResizeEle();
            this.removeHelper(e);
            this.cancelResizeAction();
        }
        if ((target.nodeName === 'TABLE' || target.nodeName === 'TD' || target.nodeName === 'TH') && !this.rzProgress) {
            this.curTable = (closestTable && this.parent.getEditPanel().contains(closestTable))
                && (target.nodeName === 'TD' || target.nodeName === 'TH') ?
                closestTable : target;
            this.removeResizeEle();
            this.tableResizeEleCreation(this.curTable, e);
        }
    };
    Table.prototype.tableResizeEleCreation = function (table, e) {
        this.parent.defaultResize(e, false);
        var columns = Array.prototype.slice.call(table.rows[this.calMaxCol(table)].cells, 1);
        var rows = [];
        for (var i = 0; i < table.rows.length; i++) {
            rows.push(Array.prototype.slice.call(table.rows[i].cells, 0, 1)[0]);
        }
        var height = parseInt(getComputedStyle(table).height, 10);
        var width = parseInt(getComputedStyle(table).width, 10);
        var pos = this.calcPos(table);
        for (var i = 0; columns.length > i; i++) {
            var colReEle = sf.base.createElement('span', {
                attrs: {
                    'data-col': (i + 1).toString(), 'unselectable': 'on', 'contenteditable': 'false'
                }
            });
            colReEle.classList.add(CLS_RTE_TABLE_RESIZE, CLS_TB_COL_RES);
            colReEle.style.cssText = 'height: ' + height + 'px; width: 4px; top: ' + pos.top +
                'px; left:' + (pos.left + this.calcPos(columns[i]).left) + 'px;';
            this.parent.getEditPanel().appendChild(colReEle);
        }
        for (var i = 0; rows.length > i; i++) {
            var rowReEle = sf.base.createElement('span', {
                attrs: {
                    'data-row': (i).toString(), 'unselectable': 'on', 'contenteditable': 'false'
                }
            });
            rowReEle.classList.add(CLS_RTE_TABLE_RESIZE, CLS_TB_ROW_RES);
            var rowPosLeft = !sf.base.isNullOrUndefined(table.getAttribute('cellspacing')) || table.getAttribute('cellspacing') !== '' ?
                0 : this.calcPos(rows[i]).left;
            rowReEle.style.cssText = 'width: ' + width + 'px; height: 4px; top: ' +
                (this.calcPos(rows[i]).top + pos.top + rows[i].offsetHeight - 2) +
                'px; left:' + (rowPosLeft + pos.left) + 'px;';
            this.parent.getEditPanel().appendChild(rowReEle);
        }
        var tableReBox = sf.base.createElement('span', {
            className: CLS_TB_BOX_RES, attrs: {
                'data-col': columns.length.toString(), 'unselectable': 'on', 'contenteditable': 'false'
            }
        });
        tableReBox.style.cssText = 'top: ' + (pos.top + height - 4) +
            'px; left:' + (pos.left + width - 4) + 'px;';
        if (sf.base.Browser.isDevice) {
            tableReBox.classList.add('e-rmob');
        }
        this.parent.getEditPanel().appendChild(tableReBox);
    };
    Table.prototype.resizeStart = function (e) {
        var _this = this;
        if (this.parent.readonly) {
            return;
        }
        if (sf.base.Browser.isDevice) {
            this.resizeHelper(e);
        }
        var target = e.target;
        if (target.classList.contains(CLS_TB_COL_RES) ||
            target.classList.contains(CLS_TB_ROW_RES) ||
            target.classList.contains(CLS_TB_BOX_RES)) {
            e.preventDefault();
            this.parent.defaultResize(e, false);
            if (!target.classList.contains(CLS_TB_BOX_RES)) {
                var rzBox = this.parent.getEditPanel().querySelector('.e-table-box');
                if (!sf.base.isNullOrUndefined(rzBox)) {
                    rzBox.classList.add('e-hide');
                    this.rzProgress = true;
                }
            }
            sf.base.removeClass(this.curTable.querySelectorAll('td,th'), CLS_TABLE_SEL);
            this.parent.formatter.editorManager.nodeSelection.Clear(this.parent.getDocument());
            this.pageX = this.getPointX(e);
            this.pageY = this.getPointY(e);
            this.resizeBtnInit();
            this.hideTableQuickToolbar();
            if (target.classList.contains(CLS_TB_COL_RES)) {
                this.resizeBtnStat.column = true;
                this.columnEle = this.curTable.rows[this.calMaxCol(this.curTable)].cells[parseInt(target.getAttribute('data-col'), 10)];
                this.colIndex = this.columnEle.cellIndex;
                this.moveEle = e.target;
                this.appendHelper();
            }
            if (target.classList.contains(CLS_TB_ROW_RES)) {
                this.rowEle = this.curTable.rows[parseInt(target.getAttribute('data-row'), 10)];
                this.resizeBtnStat.row = true;
                this.appendHelper();
            }
            if (target.classList.contains(CLS_TB_BOX_RES)) {
                this.resizeBtnStat.tableBox = true;
            }
            if (sf.base.Browser.isDevice && this.helper && !this.helper.classList.contains('e-reicon')) {
                this.helper.classList.add('e-reicon');
                sf.base.EventHandler.add(document, sf.base.Browser.touchStartEvent, this.removeHelper, this);
                sf.base.EventHandler.add(this.helper, sf.base.Browser.touchStartEvent, this.resizeStart, this);
            }
            else {
                var args = { requestType: 'Table' };
                if (this.parent.onResizeStartEnabled) {
                    // @ts-ignore-start
                    this.parent.dotNetRef.invokeMethodAsync('ResizeStartEvent', args).then(function (resizeStartArgs) {
                        // @ts-ignore-end
                        if (resizeStartArgs.cancel) {
                            _this.cancelResizeAction();
                        }
                    });
                }
            }
            sf.base.EventHandler.add(this.parent.getDocument(), sf.base.Browser.touchMoveEvent, this.resizing, this);
            sf.base.EventHandler.add(this.parent.getDocument(), sf.base.Browser.touchEndEvent, this.resizeEnd, this);
        }
    };
    Table.prototype.calMaxCol = function (element) {
        var max = 0;
        var maxRowIndex;
        for (var i = 0; i < element.rows.length; i++) {
            if (max < element.rows[i].cells.length) {
                maxRowIndex = i;
                max = element.rows[i].cells.length;
            }
        }
        return maxRowIndex;
    };
    Table.prototype.resizing = function (e) {
        var pageX = this.getPointX(e);
        var pageY = this.getPointY(e);
        var mouseX = (this.parent.enableRtl) ? -(pageX - this.pageX) : (pageX - this.pageX);
        var mouseY = (this.parent.enableRtl) ? -(pageY - this.pageY) : (pageY - this.pageY);
        this.pageX = pageX;
        this.pageY = pageY;
        var tableReBox = this.parent.getEditPanel().querySelector('.e-table-box');
        var tableWidth = parseInt(getComputedStyle(this.curTable).width, 10);
        var tableHeight = parseInt(getComputedStyle(this.curTable).height, 10);
        var paddingSize = +getComputedStyle(this.parent.getEditPanel()).paddingRight.match(/\d/g).join('');
        var rteWidth = this.parent.getEditPanel().offsetWidth - paddingSize * 2;
        if (this.resizeBtnStat.column) {
            var cellColl = this.curTable.rows[this.calMaxCol(this.curTable)].cells;
            var width = parseFloat(this.columnEle.offsetWidth.toString());
            var actualWidth = width - mouseX;
            var totalWidth = parseFloat(this.columnEle.offsetWidth.toString()) +
                parseFloat(cellColl[this.colIndex - 1].offsetWidth.toString());
            for (var i = 0; i < this.curTable.rows.length; i++) {
                if ((totalWidth - actualWidth) > 20 && actualWidth > 20) {
                    var leftColumnWidth = totalWidth - actualWidth;
                    var rightColWidth = actualWidth;
                    if (this.curTable.rows[i].cells[this.colIndex - 1]) {
                        this.curTable.rows[i].cells[this.colIndex - 1].style.width =
                            this.convertPixelToPercentage(leftColumnWidth, tableWidth) + '%';
                    }
                    if (this.curTable.rows[i].cells[this.colIndex]) {
                        this.curTable.rows[i].cells[this.colIndex].style.width =
                            this.convertPixelToPercentage(rightColWidth, tableWidth) + '%';
                    }
                }
            }
            this.updateHelper();
        }
        else if (this.resizeBtnStat.row) {
            this.parent.defaultResize(e, false);
            var height = parseFloat(this.rowEle.clientHeight.toString()) + mouseY;
            if (height > 20) {
                this.rowEle.style.height = height + 'px';
            }
            this.curTable.style.height = '';
            if (!sf.base.isNullOrUndefined(tableReBox)) {
                tableReBox.style.cssText = 'top: ' + (this.calcPos(this.curTable).top + tableHeight - 4) +
                    'px; left:' + (this.calcPos(this.curTable).left + tableWidth - 4) + 'px;';
            }
            this.updateHelper();
        }
        else if (this.resizeBtnStat.tableBox) {
            if (!sf.base.Browser.isDevice) {
                sf.base.EventHandler.remove(this.parent.getEditPanel(), 'mouseover', this.resizeHelper);
            }
            var widthType = this.curTable.style.width.indexOf('%') > -1;
            this.curTable.style.width = widthType ? this.convertPixelToPercentage(tableWidth + mouseX, rteWidth) + '%'
                : tableWidth + mouseX + 'px';
            this.curTable.style.height = tableHeight + mouseY + 'px';
            if (!sf.base.isNullOrUndefined(tableReBox)) {
                tableReBox.classList.add('e-rbox-select');
                tableReBox.style.cssText = 'top: ' + (this.calcPos(this.curTable).top + tableHeight - 4) +
                    'px; left:' + (this.calcPos(this.curTable).left + tableWidth - 4) + 'px;';
            }
        }
    };
    Table.prototype.resizeEnd = function (e) {
        this.resizeBtnInit();
        var rzBox = this.parent.getEditPanel().querySelector('.e-table-box');
        if (!sf.base.isNullOrUndefined(rzBox)) {
            rzBox.classList.remove('e-hide');
            this.rzProgress = false;
        }
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchMoveEvent, this.resizing);
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchEndEvent, this.resizeEnd);
        if (rzBox && this.parent.getEditPanel().contains(rzBox)) {
            if (!sf.base.Browser.isDevice) {
                sf.base.EventHandler.add(this.parent.getEditPanel(), 'mouseover', this.resizeHelper, this);
            }
            this.removeResizeEle();
            if (this.helper && this.parent.getEditPanel().contains(this.helper)) {
                sf.base.detach(this.helper);
                this.helper = null;
            }
            this.pageX = null;
            this.pageY = null;
            this.moveEle = null;
        }
        var args = { requestType: 'Table' };
        if (this.parent.onResizeStopEnabled) {
            this.parent.dotNetRef.invokeMethodAsync('ResizeStopEvent', args);
        }
        this.parent.formatter.saveData();
    };
    Table.prototype.resizeBtnInit = function () {
        return this.resizeBtnStat = { column: false, row: false, tableBox: false };
    };
    Table.prototype.removeResizeEle = function () {
        var item = this.parent.getEditPanel().
            querySelectorAll('.e-column-resize, .e-row-resize, .e-table-box');
        if (item.length > 0) {
            for (var i = 0; i < item.length; i++) {
                sf.base.detach(item[i]);
            }
        }
    };
    Table.prototype.appendHelper = function () {
        this.helper = sf.base.createElement('div', {
            className: 'e-table-rhelper'
        });
        if (sf.base.Browser.isDevice) {
            this.helper.classList.add('e-reicon');
        }
        this.parent.getEditPanel().appendChild(this.helper);
        this.setHelperHeight();
    };
    Table.prototype.setHelperHeight = function () {
        var pos = this.calcPos(this.curTable);
        if (this.resizeBtnStat.column) {
            this.helper.classList.add('e-column-helper');
            this.helper.style.cssText = 'height: ' + getComputedStyle(this.curTable).height + '; top: ' +
                pos.top + 'px; left:' + (pos.left + this.calcPos(this.columnEle).left - 1) + 'px;';
        }
        else {
            this.helper.classList.add('e-row-helper');
            this.helper.style.cssText = 'width: ' + getComputedStyle(this.curTable).width + '; top: ' +
                (this.calcPos(this.rowEle).top + pos.top + this.rowEle.offsetHeight) +
                'px; left:' + (this.calcPos(this.rowEle).left + pos.left) + 'px;';
        }
    };
    Table.prototype.updateHelper = function () {
        var pos = this.calcPos(this.curTable);
        if (this.resizeBtnStat.column) {
            var left = pos.left + this.calcPos(this.columnEle).left - 1;
            this.helper.style.left = left + 'px';
        }
        else {
            var top_1 = this.calcPos(this.rowEle).top + pos.top + this.rowEle.offsetHeight;
            this.helper.style.top = top_1 + 'px';
        }
    };
    Table.prototype.removeHelper = function (e) {
        var cls = e.target.classList;
        if (!(cls.contains('e-reicon')) && this.helper) {
            sf.base.EventHandler.remove(document, sf.base.Browser.touchStartEvent, this.removeHelper);
            sf.base.EventHandler.remove(this.helper, sf.base.Browser.touchStartEvent, this.resizeStart);
            if (this.helper && this.parent.getEditPanel().contains(this.helper)) {
                sf.base.detach(this.helper);
            }
            this.pageX = null;
            this.helper = null;
        }
    };
    Table.prototype.convertPixelToPercentage = function (value, offsetValue) {
        return (value / offsetValue) * 100;
    };
    Table.prototype.cancelResizeAction = function () {
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchMoveEvent, this.resizing);
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchEndEvent, this.resizeEnd);
        this.removeResizeEle();
    };
    Table.prototype.calcPos = function (elem) {
        var parentOffset = {
            top: 0,
            left: 0
        };
        var offset = elem.getBoundingClientRect();
        var doc = elem.ownerDocument;
        var offsetParent = elem.offsetParent || doc.documentElement;
        while (offsetParent &&
            (offsetParent === doc.body || offsetParent === doc.documentElement) &&
            offsetParent.style.position === 'static') {
            offsetParent = offsetParent.parentNode;
        }
        if (offsetParent.nodeName === 'TD' && elem.nodeName === 'TABLE') {
            offsetParent = sf.base.closest(offsetParent, '.e-rte-content');
        }
        if (offsetParent && offsetParent !== elem && offsetParent.nodeType === 1) {
            parentOffset = offsetParent.getBoundingClientRect();
        }
        return {
            top: elem.offsetTop,
            left: elem.offsetLeft
        };
    };
    Table.prototype.getPointX = function (e) {
        if (e.touches && e.touches.length) {
            return e.touches[0].pageX;
        }
        else {
            return e.pageX;
        }
    };
    Table.prototype.getPointY = function (e) {
        if (e.touches && e.touches.length) {
            return e.touches[0].pageY;
        }
        else {
            return e.pageY;
        }
    };
    //#endregion
    //#region Quick toolbar related methods
    Table.prototype.editAreaClickHandler = function (e) {
        if (this.parent.readonly || !sf.base.isNullOrUndefined(sf.base.closest(e.args.target, '.e-img-caption'))) {
            return;
        }
        var args = e.args;
        var showOnRightClick = this.parent.quickToolbarSettings.showOnRightClick;
        if (args.which === 2 || (showOnRightClick && args.which === 1) || (!showOnRightClick && args.which === 3)) {
            return;
        }
        if (this.parent.editorMode === 'HTML' && this.parent.quickToolbarModule) {
            this.quickToolObj = this.parent.quickToolbarModule;
            var parentRect = this.parent.element.getBoundingClientRect();
            var target = args.target;
            var tbElement = this.parent.getToolbarElement();
            var tbHeight = (tbElement) ? (tbElement.offsetHeight + this.parent.toolbarModule.getExpandTBarPopHeight()) : 0;
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            var closestTable = sf.base.closest(target, 'table');
            if (target && target.nodeName !== 'A' && target.nodeName !== 'IMG' && (target.nodeName === 'TD' || target.nodeName === 'TH' ||
                target.nodeName === 'TABLE' || (closestTable && this.parent.getEditPanel().contains(closestTable)))
                && !(range.startContainer.nodeType === 3 && !range.collapsed)) {
                var range_1 = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
                this.parent.formatter.editorManager.nodeSelection.save(range_1, this.parent.getDocument());
                this.parent.formatter.editorManager.nodeSelection.Clear(this.parent.getDocument());
                var pageX = this.parent.iframeSettings.enable ? window.pageXOffset + parentRect.left + args.clientX : args.pageX;
                var pageY = this.parent.iframeSettings.enable ? window.pageYOffset + parentRect.top +
                    tbHeight + args.clientY : args.pageY;
                this.quickToolObj.showTableQTBar(pageX, pageY, target, 'Table');
                this.parent.formatter.editorManager.nodeSelection.restore();
            }
            else {
                this.hideTableQuickToolbar();
            }
        }
    };
    Table.prototype.hideTableQuickToolbar = function () {
        if (this.quickToolObj) {
            this.quickToolObj.hideTableQTBar();
        }
    };
    Table.prototype.onToolbarAction = function (args) {
        var item = args.args.item;
        switch (item.subCommand) {
            case 'TableHeader':
                this.tableHeader(args.selection, args.args);
                break;
            case 'TableRemove':
                this.removeTable(args.selection, args.args);
                break;
            case 'TableEditProperties':
                this.editTable(args);
                break;
        }
    };
    Table.prototype.tableHeader = function (selection, e) {
        this.parent.formatter.process(this.parent, e, e.originalEvent, { selection: selection, subCommand: e.item.subCommand });
    };
    Table.prototype.removeTable = function (selection, args, delKey) {
        var cmd;
        if (delKey) {
            cmd = { item: { command: 'Table', subCommand: 'TableRemove' } };
        }
        var value = {
            selection: selection,
            subCommand: (delKey) ? cmd.item.subCommand : args.item.subCommand
        };
        this.parent.formatter.process(this.parent, (delKey) ? cmd : args, args.originalEvent, value);
        this.parent.getEditPanel().focus();
        this.removeResizeEle();
        this.hideTableQuickToolbar();
    };
    Table.prototype.editTable = function (args) {
        var selectNode = args.selectParent[0];
        this.tableNotifyArgs.selectNode = args.selectParent;
        var width = sf.base.closest(selectNode, 'table').getClientRects()[0].width;
        var padding = sf.base.closest(selectNode, 'td').style.padding;
        var spacing = sf.base.closest(selectNode, 'table').getAttribute('cellspacing');
        this.hideTableQuickToolbar();
        this.createDialog({
            width: width,
            padding: parseFloat((padding !== '' ? parseInt(padding, null) : 0).toString()),
            spacing: parseFloat((spacing !== '' && !sf.base.isNullOrUndefined(spacing) ? parseInt(spacing, null) : 0).toString())
        }, 'Edit');
    };
    Table.prototype.dropdownSelect = function (e) {
        this.parent.observer.notify(selectionSave, {});
        var item = e.item;
        if (!document.body.contains(document.body.querySelector('.e-rte-quick-toolbar')) || item.command !== 'Table') {
            return;
        }
        var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
        var args = {
            args: e,
            selection: this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument()),
            selectParent: this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range)
        };
        switch (item.subCommand) {
            case 'InsertRowBefore':
            case 'InsertRowAfter':
                this.addRow(args.selection, e);
                break;
            case 'InsertColumnLeft':
            case 'InsertColumnRight':
                this.addColumn(args.selection, e);
                break;
            case 'DeleteColumn':
            case 'DeleteRow':
                this.removeRowColumn(args.selection, e);
                break;
            case 'AlignTop':
            case 'AlignMiddle':
            case 'AlignBottom':
                this.verticalAlign(args, e);
                break;
            case 'Dashed':
            case 'Alternate':
            case 'Custom':
                this.tableStyles(args, item.subCommand);
                break;
        }
    };
    Table.prototype.addRow = function (selectCell, e, tabKey) {
        var cmd;
        if (tabKey) {
            cmd = {
                item: { command: 'Table', subCommand: 'InsertRowAfter' }
            };
        }
        var value = {
            selection: selectCell,
            subCommand: (tabKey) ? cmd.item.subCommand : e.item.subCommand
        };
        this.parent.formatter.process(this.parent, (tabKey) ? cmd : e, e, value);
    };
    Table.prototype.addColumn = function (selectCell, e) {
        this.parent.formatter.process(this.parent, e, e, { selection: selectCell, width: this.parent.tableSettings.width, subCommand: e.item.subCommand });
    };
    Table.prototype.removeRowColumn = function (selectCell, e) {
        this.parent.observer.notify(selectionRestore, {});
        this.parent.formatter.process(this.parent, e, e, { selection: selectCell, subCommand: e.item.subCommand });
        this.hideTableQuickToolbar();
        this.parent.observer.notify(selectionSave, {});
    };
    Table.prototype.verticalAlign = function (args, e) {
        var tdEle = sf.base.closest(args.selectParent[0], 'td') || sf.base.closest(args.selectParent[0], 'th');
        if (tdEle) {
            this.parent.formatter.process(this.parent, e, e, { tableCell: tdEle, subCommand: e.item.subCommand });
        }
    };
    Table.prototype.tableStyles = function (args, command) {
        var table = sf.base.closest(args.selectParent[0], 'table');
        if (command === 'Dashed') {
            (this.parent.element.classList.contains(CLS_TB_DASH_BOR)) ?
                this.parent.element.classList.remove(CLS_TB_DASH_BOR) : this.parent.element.classList.add(CLS_TB_DASH_BOR);
            (table.classList.contains(CLS_TB_DASH_BOR)) ? table.classList.remove(CLS_TB_DASH_BOR) :
                table.classList.add(CLS_TB_DASH_BOR);
        }
        if (command === 'Alternate') {
            (this.parent.element.classList.contains(CLS_TB_ALT_BOR)) ?
                this.parent.element.classList.remove(CLS_TB_ALT_BOR) : this.parent.element.classList.add(CLS_TB_ALT_BOR);
            (table.classList.contains(CLS_TB_ALT_BOR)) ? table.classList.remove(CLS_TB_ALT_BOR) :
                table.classList.add(CLS_TB_ALT_BOR);
        }
        if (args.args && args.args.item.cssClass) {
            var classList = args.args.item.cssClass.split(' ');
            for (var i = 0; i < classList.length; i++) {
                (table.classList.contains(classList[i])) ? table.classList.remove(classList[i]) :
                    table.classList.add(classList[i]);
            }
        }
        this.parent.formatter.saveData();
        this.parent.formatter.editorManager.nodeSelection.restore();
        this.hideTableQuickToolbar();
    };
    Table.prototype.setBGColor = function (args) {
        var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
        var selection = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
        var selectedCell = selection.range.startContainer;
        selectedCell = (selectedCell.nodeType === 3) ? sf.base.closest(selectedCell.parentNode, 'td,th') : sf.base.closest(selectedCell, 'td, th');
        if (selectedCell && (selectedCell.nodeName === 'TD' || selectedCell.nodeName === 'TH')) {
            var items = sf.base.closest(selectedCell, 'table').querySelectorAll('.' + CLS_TABLE_SEL);
            for (var i = 0; i < items.length; i++) {
                items[i].style.backgroundColor = args.item.value;
            }
            this.parent.formatter.saveData();
        }
        this.hideTableQuickToolbar();
    };
    Table.prototype.closeOpenedDialog = function () {
        var createDlg = this.parent.element.querySelector('.e-rte-table-popup');
        var insertDlg = this.parent.element.querySelector('.e-rte-edit-table');
        if (createDlg) {
            this.parent.dotNetRef.invokeMethodAsync('CloseCreateTableDialog');
        }
        if (insertDlg) {
            this.parent.dotNetRef.invokeMethodAsync('CloseTableDialog');
        }
    };
    Table.prototype.showDialog = function (isExternal, e) {
        if (isExternal) {
            this.parent.getEditPanel().focus();
        }
        if (this.parent.editorMode === 'HTML') {
            var docElement = this.parent.getDocument();
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(docElement);
            var selection = this.parent.formatter.editorManager.nodeSelection.save(range, docElement);
            this.parent.observer.notify(selectionSave, {});
            var args = {
                originalEvent: isExternal ? undefined : e.args,
                item: { command: 'Table', subCommand: 'CreateTable' }
            };
            this.tableNotifyArgs = { args: args, selection: selection };
            this.insertTableDialog();
        }
    };
    //#endregion
    //#region Event handler methods
    Table.prototype.keyDown = function (e) {
        var event = e.args;
        var proxy = this;
        switch (event.action) {
            case 'escape':
                break;
            case 'insert-table':
                this.showDialog(false, e);
                event.preventDefault();
                break;
        }
        if (!sf.base.isNullOrUndefined(this.parent.formatter.editorManager.nodeSelection) && this.parent.getEditPanel()
            && event.code !== 'KeyK') {
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            var selection = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
            var nodes = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
            var ele = nodes[(range.endOffset !== 0 && event.keyCode === 9 ? nodes.length - 1 : 0)];
            ele = (ele && ele.tagName !== 'TD' && ele.tagName !== 'TH') ? ele.parentElement : ele;
            if ((event.keyCode === 8 || event.keyCode === 46) ||
                (event.ctrlKey && event.keyCode === 88)) {
                if (ele && ele.tagName === 'TBODY') {
                    event.preventDefault();
                    proxy.removeTable(selection, event, true);
                }
                else if (ele && ele.querySelectorAll('table').length > 0) {
                    this.removeResizeEle();
                }
            }
            if (ele && ele.tagName !== 'TD' && ele.tagName !== 'TH') {
                var closestTd = sf.base.closest(ele, 'td');
                ele = !sf.base.isNullOrUndefined(closestTd) && this.parent.inputElement.contains(closestTd) ? closestTd : ele;
            }
            if (ele && (ele.tagName === 'TD' || ele.tagName === 'TH')) {
                switch (event.keyCode) {
                    case 9:
                    case 37:
                    case 39:
                        proxy.tabSelection(event, selection, ele);
                        break;
                    case 40:
                    case 38:
                        proxy.tableArrowNavigation(event, selection, ele);
                        break;
                }
            }
        }
    };
    Table.prototype.docClick = function (e) {
        var target = e.args.target;
        var createDlg = this.parent.element.querySelector('.e-rte-table-popup');
        var insertDlg = this.parent.element.querySelector('.e-rte-edit-table');
        if (target && target.classList && ((createDlg && !sf.base.closest(target, '#' + createDlg.id) ||
            (insertDlg && !sf.base.closest(target, '#' + insertDlg.id)))) && !target.classList.contains('e-create-table') &&
            target.offsetParent && !target.offsetParent.classList.contains('e-rte-backgroundcolor-dropdown')) {
            this.closeOpenedDialog();
            this.parent.isBlur = true;
            if (sf.base.Browser.isIE) {
                dispatchEvent(this.parent.element, 'focusout');
            }
        }
        var closestEle = sf.base.closest(target, 'td');
        var isExist = closestEle && this.parent.getEditPanel().contains(closestEle) ? true : false;
        if (target && target.tagName !== 'TD' && target.tagName !== 'TH' && !isExist &&
            sf.base.closest(target, '.e-rte-quick-popup') === null && target.offsetParent &&
            !target.offsetParent.classList.contains('e-quick-dropdown') &&
            !target.offsetParent.classList.contains('e-rte-backgroundcolor-dropdown') && !sf.base.closest(target, '.e-rte-dropdown-popup')
            && !sf.base.closest(target, '.e-rte-elements')) {
            sf.base.removeClass(this.parent.inputElement.querySelectorAll('table td'), CLS_TABLE_SEL);
            if (!sf.base.Browser.isIE) {
                this.hideTableQuickToolbar();
            }
        }
        if (target && target.classList && !target.classList.contains(CLS_TB_COL_RES) &&
            !target.classList.contains(CLS_TB_ROW_RES) && !target.classList.contains(CLS_TB_BOX_RES)) {
            this.removeResizeEle();
        }
    };
    Table.prototype.onIframeMouseDown = function () {
        this.closeOpenedDialog();
    };
    Table.prototype.keyUp = function (e) {
        var args = e.args;
        if ((args.which === 8 && args.code === 'Backspace') || (args.which === 46 && args.code === 'Delete')) {
            this.hideTableQuickToolbar();
        }
    };
    //#endregion
    Table.prototype.destroy = function () {
        this.removeEventListener();
    };
    return Table;
}());

/**
 * `Image` module is used to handle image actions.
 */
var Image = /** @class */ (function () {
    function Image(parent) {
        this.pageX = null;
        this.pageY = null;
        this.deletedImg = [];
        this.parent = parent;
        this.rteId = parent.element.id;
        this.addEventListener();
    }
    Image.prototype.addEventListener = function () {
        this.parent.observer.on(keyUp, this.onKeyUp, this);
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(paste, this.imagePaste, this);
        this.parent.observer.on(keyDown, this.onKeyDown, this);
        this.parent.observer.on(imageSize, this.imageSize, this);
        this.parent.observer.on(imageCaption, this.caption, this);
        this.parent.observer.on(imageDelete, this.deleteImg, this);
        this.parent.observer.on(imageAlt, this.insertAltText, this);
        this.parent.observer.on(initialEnd, this.afterRender, this);
        this.parent.observer.on(imageLink, this.insertImgLink, this);
        this.parent.observer.on(insertImage, this.imageDialog, this);
        this.parent.observer.on(windowResize, this.onWindowResize, this);
        this.parent.observer.on(dropDownSelect, this.alignmentSelect, this);
        this.parent.observer.on(iframeMouseDown, this.onIframeMouseDown, this);
        this.parent.observer.on(imageToolbarAction, this.onToolbarAction, this);
        this.parent.observer.on(editAreaClick, this.editAreaClickHandler, this);
        this.parent.observer.on(insertCompleted, this.showImageQuickToolbar, this);
    };
    Image.prototype.removeEventListener = function () {
        this.parent.observer.off(keyUp, this.onKeyUp);
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(paste, this.imagePaste);
        this.parent.observer.off(keyDown, this.onKeyDown);
        this.parent.observer.off(imageSize, this.imageSize);
        this.parent.observer.off(imageCaption, this.caption);
        this.parent.observer.off(imageDelete, this.deleteImg);
        this.parent.observer.off(imageAlt, this.insertAltText);
        this.parent.observer.off(initialEnd, this.afterRender);
        this.parent.observer.off(imageLink, this.insertImgLink);
        this.parent.observer.off(insertImage, this.imageDialog);
        this.parent.observer.off(windowResize, this.onWindowResize);
        this.parent.observer.off(dropDownSelect, this.alignmentSelect);
        this.parent.observer.off(iframeMouseDown, this.onIframeMouseDown);
        this.parent.observer.off(imageToolbarAction, this.onToolbarAction);
        this.parent.observer.off(editAreaClick, this.editAreaClickHandler);
        this.parent.observer.off(insertCompleted, this.showImageQuickToolbar);
        var dropElement = this.parent.iframeSettings.enable ?
            this.parent.inputElement.ownerDocument : this.parent.inputElement;
        dropElement.removeEventListener('drop', this.dragDrop.bind(this), true);
        dropElement.removeEventListener('dragstart', this.dragStart.bind(this), true);
        dropElement.removeEventListener('dragenter', this.dragEnter.bind(this), true);
        dropElement.removeEventListener('dragover', this.dragOver.bind(this), true);
        if (!sf.base.isNullOrUndefined(this.parent.getEditPanel())) {
            sf.base.EventHandler.remove(this.parent.getEditPanel(), sf.base.Browser.touchEndEvent, this.imageClick);
            this.parent.formatter.editorManager.observer.off(checkUndo, this.undoStack);
            if (this.parent.insertImageSettings.resize) {
                sf.base.EventHandler.remove(this.parent.getEditPanel(), sf.base.Browser.touchStartEvent, this.resizeStart);
                sf.base.EventHandler.remove(this.parent.element.ownerDocument, 'mousedown', this.onDocumentClick);
                sf.base.EventHandler.remove(this.parent.getEditPanel(), 'cut', this.onCutHandler);
            }
        }
    };
    Image.prototype.afterRender = function () {
        sf.base.EventHandler.add(this.parent.getEditPanel(), sf.base.Browser.touchEndEvent, this.imageClick, this);
        if (this.parent.insertImageSettings.resize) {
            sf.base.EventHandler.add(this.parent.getEditPanel(), sf.base.Browser.touchStartEvent, this.resizeStart, this);
            sf.base.EventHandler.add(this.parent.element.ownerDocument, 'mousedown', this.onDocumentClick, this);
            sf.base.EventHandler.add(this.parent.getEditPanel(), 'cut', this.onCutHandler, this);
        }
        var dropElement = this.parent.iframeSettings.enable ? this.parent.inputElement.ownerDocument :
            this.parent.inputElement;
        dropElement.addEventListener('drop', this.dragDrop.bind(this), true);
        dropElement.addEventListener('dragstart', this.dragStart.bind(this), true);
        dropElement.addEventListener('dragenter', this.dragOver.bind(this), true);
        dropElement.addEventListener('dragover', this.dragOver.bind(this), true);
    };
    Image.prototype.imageDialog = function (e) {
        this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
        this.uploadUrl = { url: '' };
        this.selectionObj = { selfImage: this, selection: e.selection, args: e.args, selectParent: e.selectParent };
        if ((!sf.base.isNullOrUndefined(this.parent.insertImageSettings.path) && this.parent.editorMode === 'Markdown')
            || this.parent.editorMode === 'HTML') {
            var iframe = this.parent.iframeSettings.enable;
            if (this.parent.editorMode === 'HTML' && (!iframe && sf.base.isNullOrUndefined(sf.base.closest(e.selection.range.startContainer.parentNode, '#' +
                this.parent.getPanel().id))
                || (iframe && sf.base.isNullOrUndefined(e.selection.range.startContainer.parentNode.ownerDocument.querySelector('body'))))) {
                this.parent.getEditPanel().focus();
                var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
                this.imgUploadSave = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
                this.imgUploadSelectedParent = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
            }
            else {
                this.imgUploadSave = e.selection;
                this.imgUploadSelectedParent = e.selectParent;
            }
        }
        var obj = { mode: (e.selectNode && e.selectNode[0].nodeName === 'IMG' ? 'Edit' : 'Insert') };
        this.parent.dotNetRef.invokeMethodAsync(showImageDialog, obj);
        if (this.quickToolObj) {
            this.quickToolObj.hideImageQTBar();
            if (!sf.base.isNullOrUndefined(e.selectParent)) {
                sf.base.removeClass([e.selectParent[0]], 'e-img-focus');
            }
            this.quickToolObj.hideInlineQTBar();
        }
    };
    Image.prototype.checkImageBack = function (range) {
        if (range.startContainer.nodeName === '#text' && range.startOffset === 0 &&
            !sf.base.isNullOrUndefined(range.startContainer.previousSibling) && range.startContainer.previousSibling.nodeName === 'IMG') {
            this.deletedImg.push(range.startContainer.previousSibling);
        }
        else if (range.startContainer.nodeName !== '#text' && !sf.base.isNullOrUndefined(range.startContainer.childNodes[range.startOffset - 1]) &&
            range.startContainer.childNodes[range.startOffset - 1].nodeName === 'IMG') {
            this.deletedImg.push(range.startContainer.childNodes[range.startOffset - 1]);
        }
    };
    Image.prototype.checkImageDel = function (range) {
        if (range.startContainer.nodeName === '#text' && range.startOffset === range.startContainer.textContent.length &&
            !sf.base.isNullOrUndefined(range.startContainer.nextSibling) && range.startContainer.nextSibling.nodeName === 'IMG') {
            this.deletedImg.push(range.startContainer.nextSibling);
        }
        else if (range.startContainer.nodeName !== '#text' && !sf.base.isNullOrUndefined(range.startContainer.childNodes[range.startOffset]) &&
            range.startContainer.childNodes[range.startOffset].nodeName === 'IMG') {
            this.deletedImg.push(range.startContainer.childNodes[range.startOffset]);
        }
    };
    Image.prototype.getDropRange = function (x, y) {
        var startRange = this.parent.getDocument().createRange();
        this.parent.formatter.editorManager.nodeSelection.setRange(this.parent.getDocument(), startRange);
        var elem = this.parent.getDocument().elementFromPoint(x, y);
        var startNode = (elem.childNodes.length > 0 ? elem.childNodes[0] : elem);
        var startCharIndexCharacter = 0;
        if (this.parent.inputElement.firstChild.innerHTML === '<br>') {
            startRange.setStart(startNode, startCharIndexCharacter);
            startRange.setEnd(startNode, startCharIndexCharacter);
        }
        else {
            var rangeRect = void 0;
            do {
                startCharIndexCharacter++;
                startRange.setStart(startNode, startCharIndexCharacter);
                startRange.setEnd(startNode, startCharIndexCharacter + 1);
                rangeRect = startRange.getBoundingClientRect();
            } while (rangeRect.left < x && startCharIndexCharacter < startNode.length - 1);
        }
        return startRange;
    };
    Image.prototype.selectRange = function (e) {
        var range;
        if (this.parent.getDocument().caretRangeFromPoint) { //For chrome
            range = this.parent.getDocument().caretRangeFromPoint(e.clientX, e.clientY);
        }
        else if ((e.rangeParent)) { //For mozilla firefox
            range = this.parent.getDocument().createRange();
            range.setStart(e.rangeParent, e.rangeOffset);
        }
        else {
            range = this.getDropRange(e.clientX, e.clientY); //For internet explorer
        }
        this.parent.observer.notify(selectRange, { range: range });
    };
    Image.prototype.imageDropInitialized = function (isStream) {
        var e = this.imageDragArgs;
        if (this.parent.element.querySelector('.' + CLS_IMG_RESIZE)) {
            sf.base.detach(this.imgResizeDiv);
        }
        this.selectRange(this.imageDragArgs);
        if (this.dropFiles.length > 1) {
            return;
        }
        this.parent.observer.notify(drop, { args: e });
        var imgFiles = this.dropFiles;
        var fileName = imgFiles[0].name;
        var imgType = fileName.substring(fileName.lastIndexOf('.'));
        var allowedTypes = this.parent.insertImageSettings.allowedTypes;
        for (var i = 0; i < allowedTypes.length; i++) {
            if (imgType.toLocaleLowerCase() === allowedTypes[i].toLowerCase()) {
                if (this.parent.insertImageSettings.saveUrl || isStream) {
                    this.onSelect(this.dropFiles);
                }
                else {
                    var args = { text: '', file: imgFiles[0] };
                    this.imagePaste(args);
                }
            }
        }
    };
    Image.prototype.insertDragImage = function (e, dropFiles) {
        var _this = this;
        e.preventDefault();
        var activePopupElement = this.parent.element.querySelector('' + CLS_POPUP_OPEN);
        this.parent.observer.notify(drop, { args: e });
        if (activePopupElement) {
            activePopupElement.classList.add(CLS_HIDE);
        }
        if (dropFiles.length <= 0) { //For internal image drag and drop
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            var imgElement = this.parent.inputElement.ownerDocument.querySelector('.' + CLS_RTE_DRAG_IMAGE);
            if (imgElement && imgElement.tagName === 'IMG') {
                if (imgElement.nextElementSibling) {
                    if (imgElement.nextElementSibling.classList.contains(CLS_IMG_INNER)) {
                        range.insertNode(imgElement.parentElement.parentElement);
                    }
                    else {
                        range.insertNode(imgElement);
                    }
                }
                else {
                    range.insertNode(imgElement);
                }
                imgElement.classList.remove(CLS_RTE_DRAG_IMAGE);
                imgElement.addEventListener('load', function () {
                    if (_this.parent.actionCompleteEnabled) {
                        _this.parent.dotNetRef.invokeMethodAsync(actionCompleteEvent, null);
                    }
                });
                this.parent.formatter.editorManager.nodeSelection.Clear(this.parent.getDocument());
                var args = e;
                this.resizeStart(args, imgElement);
                this.hideImageQuickToolbar();
            }
        }
    };
    Image.prototype.onSelect = function (dropFiles) {
        var _this = this;
        var proxy = this;
        var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
        var validFiles = {
            name: '',
            size: 0,
            status: '',
            statusCode: '',
            type: '',
            rawFile: dropFiles[0],
            validationMessages: {}
        };
        this.droppedImage = sf.base.createElement('IMG');
        this.droppedImage.style.opacity = '0.5';
        this.droppedImage.classList.add(CLS_RTE_IMAGE);
        this.droppedImage.classList.add(CLS_IMGINLINE);
        this.droppedImage.classList.add(CLS_RESIZE);
        var file = validFiles.rawFile;
        var reader = new FileReader();
        reader.addEventListener('load', function () {
            var url = URL.createObjectURL(convertToBlob(reader.result));
            _this.droppedImage.src = proxy.parent.insertImageSettings.saveFormat === 'Blob' ? url : reader.result;
        });
        if (file) {
            reader.readAsDataURL(file);
        }
        range.insertNode(this.droppedImage);
        range.selectNodeContents(this.droppedImage);
        this.parent.formatter.editorManager.nodeSelection.setRange(this.parent.getDocument(), range);
        this.droppedImage.addEventListener('load', function () {
            if (_this.parent.actionCompleteEnabled) {
                _this.parent.dotNetRef.invokeMethodAsync(actionCompleteEvent, null);
            }
        });
    };
    Image.prototype.removeDroppedImage = function () {
        sf.base.detach(this.droppedImage);
    };
    Image.prototype.dropUploadSuccess = function (url, altText) {
        this.droppedImage.style.opacity = '1';
        this.droppedImage.classList.add(CLS_IMG_FOCUS);
        this.droppedImage.src = url;
        this.droppedImage.alt = altText;
        this.showImageQuickToolbar({
            args: this.imageDragArgs, type: 'Images', isNotify: undefined, elements: this.droppedImage
        });
        this.resizeStart(this.imageDragArgs, this.droppedImage);
    };
    Image.prototype.dropUploadChange = function (url, isStream) {
        if (isStream) {
            this.droppedImage.src = url;
            this.droppedImage.style.opacity = '1';
        }
    };
    Image.prototype.imagePaste = function (args) {
        var _this = this;
        if (args.text.length === 0 && !sf.base.isNullOrUndefined(args.file)) {
            var proxy_1 = this;
            var reader_1 = new FileReader();
            if (args.args) {
                args.args.preventDefault();
            }
            reader_1.addEventListener('load', function (e) {
                var url = {
                    cssClass: (proxy_1.parent.insertImageSettings.display === 'inline' ? CLS_IMGINLINE : CLS_IMGBREAK),
                    url: _this.parent.insertImageSettings.saveFormat === 'Base64' || !sf.base.isNullOrUndefined(args.callBack) ?
                        reader_1.result : URL.createObjectURL(convertToBlob(reader_1.result)),
                    width: {
                        width: proxy_1.parent.insertImageSettings.width, minWidth: proxy_1.parent.insertImageSettings.minWidth,
                        maxWidth: proxy_1.getMaxWidth()
                    },
                    height: {
                        height: proxy_1.parent.insertImageSettings.height, minHeight: proxy_1.parent.insertImageSettings.minHeight,
                        maxHeight: proxy_1.parent.insertImageSettings.maxHeight
                    }
                };
                if (!sf.base.isNullOrUndefined(args.callBack)) {
                    args.callBack(url);
                    return;
                }
                else {
                    proxy_1.parent.formatter.process(proxy_1.parent, { item: { command: 'Images', subCommand: 'Image' } }, args.args, url);
                    _this.showPopupToolBar(args, url);
                }
            });
            reader_1.readAsDataURL(args.file);
        }
    };
    Image.prototype.showPopupToolBar = function (e, url) {
        var _this = this;
        var imageSrc = 'img[src="' + url.url + '"]';
        var imageElement = this.parent.inputElement.querySelector(imageSrc);
        var args = {
            args: e.args,
            type: 'Images',
            isNotify: undefined,
            elements: imageElement
        };
        if (imageElement) {
            setTimeout(function () { _this.showImageQuickToolbar(args); _this.resizeStart(e.args, imageElement); }, 0);
        }
    };
    Image.prototype.undoStack = function (args) {
        if (args.subCommand.toLowerCase() === 'undo' || args.subCommand.toLowerCase() === 'redo') {
            for (var i = 0; i < this.parent.formatter.getUndoRedoStack().length; i++) {
                var temp = sf.base.createElement('div');
                var contentElem = parseHtml(this.parent.formatter.getUndoRedoStack()[i].text);
                temp.appendChild(contentElem);
                var img = temp.querySelectorAll('img');
                if (temp.querySelector('.' + CLS_IMG_RESIZE) && img.length > 0) {
                    for (var j = 0; j < img.length; j++) {
                        img[j].style.outline = '';
                    }
                    sf.base.detach(temp.querySelector('.' + CLS_IMG_RESIZE));
                    this.parent.formatter.getUndoRedoStack()[i].text = temp.innerHTML;
                }
            }
        }
    };
    //#region Resize methods
    Image.prototype.imageResize = function (e) {
        this.resizeBtnInit();
        this.imgEle = e;
        sf.base.addClass([this.imgEle], 'e-resize');
        this.imgResizeDiv = sf.base.createElement('span', { className: CLS_IMG_RESIZE, id: this.rteId + imgResizeId });
        this.imgResizeDiv.appendChild(sf.base.createElement('span', {
            className: CLS_RTE_IMG_BOX_MARK + ' e-rte-topLeft', styles: 'cursor: nwse-resize'
        }));
        this.imgResizeDiv.appendChild(sf.base.createElement('span', {
            className: CLS_RTE_IMG_BOX_MARK + ' e-rte-topRight', styles: 'cursor: nesw-resize'
        }));
        this.imgResizeDiv.appendChild(sf.base.createElement('span', {
            className: CLS_RTE_IMG_BOX_MARK + ' e-rte-botLeft', styles: 'cursor: nesw-resize'
        }));
        this.imgResizeDiv.appendChild(sf.base.createElement('span', {
            className: CLS_RTE_IMG_BOX_MARK + ' e-rte-botRight', styles: 'cursor: nwse-resize'
        }));
        if (sf.base.Browser.isDevice) {
            sf.base.addClass([this.imgResizeDiv], 'e-mob-rte');
        }
        e.style.outline = '2px solid #4a90e2';
        this.imgResizePos(e, this.imgResizeDiv);
        this.resizeImgDupPos(e);
        this.parent.getEditPanel().appendChild(this.imgResizeDiv);
        sf.base.EventHandler.add(this.parent.getDocument(), sf.base.Browser.touchMoveEvent, this.resizing, this);
    };
    Image.prototype.resizeBtnInit = function () {
        return this.resizeBtnStat = { botLeft: false, botRight: false, topRight: false, topLeft: false };
    };
    Image.prototype.imgResizePos = function (e, imgResizeDiv) {
        var pos = this.calcPos(e);
        var top = pos.top;
        var left = pos.left;
        var imgWid = e.width;
        var imgHgt = e.height;
        var borWid = (sf.base.Browser.isDevice) ? (4 * parseInt((e.style.outline.slice(-3)), 10)) + 2 :
            (2 * parseInt((e.style.outline.slice(-3)), 10)) + 2; //span border width + image outline width
        var devWid = ((sf.base.Browser.isDevice) ? 0 : 2); // span border width
        imgResizeDiv.querySelector('.e-rte-botLeft').style.left = (left - borWid) + 'px';
        imgResizeDiv.querySelector('.e-rte-botLeft').style.top = ((imgHgt - borWid) + top) + 'px';
        imgResizeDiv.querySelector('.e-rte-botRight').style.left = ((imgWid - (borWid - devWid)) + left) + 'px';
        imgResizeDiv.querySelector('.e-rte-botRight').style.top = ((imgHgt - borWid) + top) + 'px';
        imgResizeDiv.querySelector('.e-rte-topRight').style.left = ((imgWid - (borWid - devWid)) + left) + 'px';
        imgResizeDiv.querySelector('.e-rte-topRight').style.top = (top - (borWid)) + 'px';
        imgResizeDiv.querySelector('.e-rte-topLeft').style.left = (left - borWid) + 'px';
        imgResizeDiv.querySelector('.e-rte-topLeft').style.top = (top - borWid) + 'px';
    };
    Image.prototype.resizeImgDupPos = function (e) {
        this.imgDupPos = {
            width: (e.style.height !== '') ? this.imgEle.style.width : e.width + 'px',
            height: (e.style.height !== '') ? this.imgEle.style.height : e.height + 'px'
        };
    };
    Image.prototype.calcPos = function (elem) {
        var ignoreOffset = ['TD', 'TH', 'TABLE', 'A'];
        var parentOffset = { top: 0, left: 0 };
        var doc = elem.ownerDocument;
        var offsetParent = ((elem.offsetParent && (elem.offsetParent.classList.contains('e-img-caption') ||
            ignoreOffset.indexOf(elem.offsetParent.tagName) > -1)) ?
            sf.base.closest(elem, '#' + this.rteId + '_rte-edit-view') : elem.offsetParent) || doc.documentElement;
        while (offsetParent &&
            (offsetParent === doc.body || offsetParent === doc.documentElement) &&
            offsetParent.style.position === 'static') {
            offsetParent = offsetParent.parentNode;
        }
        if (offsetParent && offsetParent !== elem && offsetParent.nodeType === 1) {
            parentOffset = offsetParent.getBoundingClientRect();
        }
        return {
            top: elem.offsetTop,
            left: elem.offsetLeft
        };
    };
    Image.prototype.getPointX = function (e) {
        if (e.touches && e.touches.length) {
            return e.touches[0].pageX;
        }
        else {
            return e.pageX;
        }
    };
    Image.prototype.getPointY = function (e) {
        if (e.touches && e.touches.length) {
            return e.touches[0].pageY;
        }
        else {
            return e.pageY;
        }
    };
    Image.prototype.pixToPercent = function (expected, parentEle) {
        return expected / parseFloat(getComputedStyle(parentEle).width) * 100;
    };
    Image.prototype.imgDupMouseMove = function (width, height, e) {
        if ((parseInt(this.parent.insertImageSettings.minWidth, 10) >= parseInt(width, 10) ||
            parseInt(this.getMaxWidth(), 10) <= parseInt(width, 10))) {
            return;
        }
        if (!this.parent.insertImageSettings.resizeByPercent &&
            (parseInt(this.parent.insertImageSettings.minHeight, 10) >= parseInt(height, 10) ||
                parseInt(this.parent.insertImageSettings.maxHeight, 10) <= parseInt(height, 10))) {
            return;
        }
        this.imgEle.parentElement.style.cursor = 'pointer';
        this.setAspectRatio(this.imgEle, parseInt(width, 10), parseInt(height, 10));
        this.resizeImgDupPos(this.imgEle);
        this.imgResizePos(this.imgEle, this.imgResizeDiv);
        this.parent.setContentHeight('', false);
    };
    Image.prototype.setAspectRatio = function (img, expectedX, expectedY) {
        if (sf.base.isNullOrUndefined(img.width)) {
            return;
        }
        var width = img.style.width !== '' ? parseInt(img.style.width, 10) : img.width;
        var height = img.style.height !== '' ? parseInt(img.style.height, 10) : img.height;
        if (width > height) {
            if (this.parent.insertImageSettings.resizeByPercent) {
                img.style.width = this.pixToPercent((width / height * expectedY), (img.previousElementSibling || img.parentElement)) + '%';
                img.style.height = null;
                img.removeAttribute('height');
            }
            else if (img.style.width === '' && img.style.height !== '') {
                img.style.height = expectedY + 'px';
            }
            else if (img.style.width !== '' && img.style.height === '') {
                img.style.width = ((width / height * expectedY) + width / height).toString() + 'px';
            }
            else if (img.style.width !== '') {
                img.style.width = (width / height * expectedY) + 'px';
                img.style.height = expectedY + 'px';
            }
            else {
                img.setAttribute('width', ((width / height * expectedY) + width / height).toString());
            }
        }
        else if (height > width) {
            if (this.parent.insertImageSettings.resizeByPercent) {
                img.style.width = this.pixToPercent(expectedX, (img.previousElementSibling || img.parentElement)) + '%';
                img.style.height = null;
                img.removeAttribute('height');
            }
            else if (img.style.width !== '') {
                img.style.width = expectedX + 'px';
                img.style.height = (height / width * expectedX) + 'px';
            }
            else {
                img.setAttribute('width', expectedX.toString());
                img.setAttribute('height', (height / width * expectedX).toString());
            }
        }
        else {
            if (this.parent.insertImageSettings.resizeByPercent) {
                img.style.width = this.pixToPercent(expectedX, (img.previousElementSibling || img.parentElement)) + '%';
                img.style.height = null;
                img.removeAttribute('height');
            }
            else if (img.style.width !== '') {
                img.style.width = expectedX + 'px';
                img.style.height = expectedX + 'px';
            }
            else {
                img.setAttribute('width', expectedX.toString());
                img.setAttribute('height', expectedX.toString());
            }
        }
    };
    Image.prototype.getMaxWidth = function () {
        var maxWidth = this.parent.insertImageSettings.maxWidth;
        var imgPadding = 12;
        var imgResizeBorder = 2;
        var editEle = this.parent.getEditPanel();
        var eleStyle = window.getComputedStyle(editEle);
        var editEleMaxWidth = editEle.offsetWidth - (imgPadding + imgResizeBorder +
            parseFloat(eleStyle.paddingLeft.split('px')[0]) + parseFloat(eleStyle.paddingRight.split('px')[0]) +
            parseFloat(eleStyle.marginLeft.split('px')[0]) + parseFloat(eleStyle.marginRight.split('px')[0]));
        return sf.base.isNullOrUndefined(maxWidth) ? editEleMaxWidth : maxWidth;
    };
    Image.prototype.cancelResizeAction = function () {
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchMoveEvent, this.resizing);
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchEndEvent, this.resizeEnd);
        if (this.imgEle && this.imgResizeDiv && this.parent.getEditPanel().contains(this.imgResizeDiv)) {
            sf.base.detach(this.imgResizeDiv);
            this.imgEle.style.outline = '';
            this.imgResizeDiv = null;
            this.pageX = null;
            this.pageY = null;
        }
    };
    Image.prototype.removeResizeEle = function () {
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchMoveEvent, this.resizing);
        sf.base.EventHandler.remove(this.parent.getDocument(), sf.base.Browser.touchEndEvent, this.resizeEnd);
        sf.base.detach(this.parent.getEditPanel().querySelector('.e-img-resize'));
    };
    Image.prototype.onWindowResize = function () {
        if (!sf.base.isNullOrUndefined(this.parent) && !sf.base.isNullOrUndefined(this.parent.getEditPanel().querySelector('.e-img-resize'))) {
            this.cancelResizeAction();
        }
    };
    //#endregion
    //#region Quick toolbar related methods
    Image.prototype.editAreaClickHandler = function (e) {
        if (this.parent.readonly) {
            this.hideImageQuickToolbar();
            return;
        }
        var args = e.args;
        var showOnRightClick = this.parent.quickToolbarSettings.showOnRightClick;
        if (args.which === 2 || (showOnRightClick && args.which === 1) || (!showOnRightClick && args.which === 3)) {
            if ((showOnRightClick && args.which === 1) && !sf.base.isNullOrUndefined(args.target) &&
                args.target.tagName === 'IMG') {
                this.parent.formatter.editorManager.nodeSelection.Clear(this.parent.getDocument());
                this.parent.formatter.editorManager.nodeSelection.setSelectionContents(this.parent.getDocument(), args.target);
            }
            return;
        }
        if (this.parent.editorMode === 'HTML' && this.parent.quickToolbarModule) {
            this.quickToolObj = this.parent.quickToolbarModule;
            var target = args.target;
            var isPopupOpen = void 0;
            isPopupOpen = document.body.querySelector('#' + this.rteId + imageQuickPopup).classList.contains('e-rte-pop');
            if (target.nodeName === 'IMG' && this.parent.quickToolbarModule) {
                if (isPopupOpen) {
                    return;
                }
                this.parent.formatter.editorManager.nodeSelection.Clear(this.parent.getDocument());
                this.parent.formatter.editorManager.nodeSelection.setSelectionContents(this.parent.getDocument(), target);
                if (isIDevice()) {
                    this.parent.observer.notify(selectionSave, e);
                }
                sf.base.addClass([target], 'e-img-focus');
                this.showImageQuickToolbar({ args: args, type: 'Images', elements: [args.target] });
            }
            else {
                this.hideImageQuickToolbar();
            }
        }
    };
    Image.prototype.showImageQuickToolbar = function (e) {
        var _this = this;
        var type = 'ImageLink';
        if (e.type !== 'Images' || sf.base.isNullOrUndefined(this.parent.quickToolbarModule)) {
            return;
        }
        this.quickToolObj = this.parent.quickToolbarModule;
        var args = e.args;
        var target = e.elements;
        var parentRect = this.parent.element.getBoundingClientRect();
        var tbElement = this.parent.getToolbarElement();
        var tbHeight = (tbElement) ? (tbElement.offsetHeight + this.parent.toolbarModule.getExpandTBarPopHeight()) : 0;
        [].forEach.call(e.elements, function (element, index) {
            if (index === 0) {
                target = element;
            }
        });
        if (target && !sf.base.closest(target, 'a')) {
            type = 'Image';
        }
        if (target.nodeName === 'IMG') {
            sf.base.addClass([target], ['e-img-focus']);
        }
        var pageX = this.parent.iframeSettings.enable ? window.pageXOffset + parentRect.left + args.clientX : args.pageX;
        var pageY = this.parent.iframeSettings.enable ? window.pageYOffset + parentRect.top +
            tbHeight + args.clientY : args.pageY;
        if (this.quickToolObj) {
            if (e.isNotify) {
                setTimeout(function () { _this.quickToolObj.showImageQTBar(pageX, pageY, target, type); }, 400);
            }
            else {
                this.quickToolObj.showImageQTBar(pageX, pageY, target, type);
            }
        }
    };
    Image.prototype.hideImageQuickToolbar = function () {
        if (!sf.base.isNullOrUndefined(this.parent.getEditPanel().querySelector('.e-img-focus'))) {
            sf.base.removeClass([this.parent.getEditPanel().querySelector('.e-img-focus')], 'e-img-focus');
            if (this.quickToolObj) {
                this.quickToolObj.hideImageQTBar();
            }
        }
    };
    Image.prototype.onToolbarAction = function (args) {
        if (this.quickToolObj) {
            this.quickToolObj.hideImageQTBar();
            sf.base.removeClass([args.selectNode[0]], 'e-img-focus');
        }
        this.selectionObj = args;
        if (isIDevice()) {
            this.parent.observer.notify(selectionRestore, {});
        }
        var item = args.args.item;
        switch (item.subCommand) {
            case 'Replace':
                this.parent.observer.notify(insertImage, args);
                break;
            case 'Caption':
                this.parent.observer.notify(imageCaption, args);
                break;
            case 'InsertLink':
                this.parent.observer.notify(imageLink, args);
                break;
            case 'AltText':
                this.parent.observer.notify(imageAlt, args);
                break;
            case 'Remove':
                this.parent.observer.notify(imageDelete, args);
                break;
            case 'Dimension':
                this.parent.observer.notify(imageSize, args);
                break;
            case 'OpenImageLink':
                this.openImgLink(args);
                break;
            case 'EditImageLink':
                this.editImgLink(args);
                break;
            case 'RemoveImageLink':
                this.removeImgLink(args);
                break;
        }
    };
    Image.prototype.openImgLink = function (e) {
        var target = e.selectParent[0].parentNode.target === '' ? '_self' : '_blank';
        this.parent.formatter.process(this.parent, e.args, e.args, {
            url: e.selectParent[0].parentNode.href, target: target, selectNode: e.selectNode,
            subCommand: e.args.item.subCommand
        });
    };
    Image.prototype.editImgLink = function (e) {
        var selectParentEle = e.selectParent[0].parentNode;
        var inputDetails = {
            url: selectParentEle.href, target: selectParentEle.target
        };
        this.insertImgLink(e, inputDetails);
    };
    Image.prototype.removeImgLink = function (e) {
        if (sf.base.Browser.isIE) {
            this.parent.getEditPanel().focus();
        }
        e.selection.restore();
        var isCapLink = (this.parent.getEditPanel().contains(this.captionEle) && sf.base.select('a', this.captionEle)) ?
            true : false;
        var selectParent = isCapLink ? [this.captionEle] : [e.selectNode[0].parentElement];
        this.parent.formatter.process(this.parent, e.args, e.args, {
            insertElement: e.selectNode[0], selectParent: selectParent, selection: e.selection,
            subCommand: e.args.item.subCommand
        });
        if (this.quickToolObj) {
            this.quickToolObj.hideImageQTBar();
            if (!sf.base.isNullOrUndefined(e.selectParent)) {
                sf.base.removeClass([e.selectParent[0]], 'e-img-focus');
            }
        }
        if (isCapLink) {
            sf.base.select('.e-img-inner', this.captionEle).focus();
        }
    };
    Image.prototype.caption = function (e) {
        var selectNode = e.selectNode[0];
        if (selectNode.nodeName !== 'IMG') {
            return;
        }
        e.selection.restore();
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        this.cancelResizeAction();
        sf.base.addClass([selectNode], 'e-rte-image');
        var subCommand = (e.args.item) ?
            e.args.item.subCommand : 'Caption';
        if (!sf.base.isNullOrUndefined(sf.base.closest(selectNode, '.' + CLS_CAPTION))) {
            sf.base.detach(sf.base.closest(selectNode, '.' + CLS_CAPTION));
            if (sf.base.Browser.isIE) {
                this.parent.getEditPanel().focus();
                e.selection.restore();
            }
            if (selectNode.parentElement.tagName === 'A') {
                this.parent.formatter.process(this.parent, e.args, e.args, { insertElement: selectNode.parentElement, selectNode: e.selectNode, subCommand: subCommand });
            }
            else {
                this.parent.formatter.process(this.parent, e.args, e.args, { insertElement: selectNode, selectNode: e.selectNode, subCommand: subCommand });
            }
        }
        else {
            this.captionEle = sf.base.createElement('span', {
                className: CLS_CAPTION + ' ' + CLS_RTE_CAPTION,
                attrs: { contenteditable: 'false', draggable: 'false' }
            });
            var imgContain = sf.base.createElement('span', { className: 'e-img-wrap' });
            var imgInner = sf.base.createElement('span', { className: 'e-img-inner', attrs: { contenteditable: 'true' } });
            var parent_1 = e.selectNode[0].parentElement;
            if (parent_1.tagName === 'A') {
                parent_1.setAttribute('contenteditable', 'true');
            }
            imgContain.appendChild(parent_1.tagName === 'A' ? parent_1 : e.selectNode[0]);
            imgContain.appendChild(imgInner);
            /* eslint-disable */
            var imgCaption = this.parent.localeData['imageCaption'];
            /* eslint-enable */
            imgInner.innerHTML = imgCaption;
            this.captionEle.appendChild(imgContain);
            if (selectNode.classList.contains(CLS_IMGINLINE)) {
                sf.base.addClass([this.captionEle], CLS_CAPINLINE);
            }
            if (selectNode.classList.contains(CLS_IMGBREAK)) {
                sf.base.addClass([this.captionEle], CLS_IMGBREAK);
            }
            if (selectNode.classList.contains(CLS_IMGLEFT)) {
                sf.base.addClass([this.captionEle], CLS_IMGLEFT);
            }
            if (selectNode.classList.contains(CLS_IMGRIGHT)) {
                sf.base.addClass([this.captionEle], CLS_IMGRIGHT);
            }
            if (selectNode.classList.contains(CLS_IMGCENTER)) {
                sf.base.addClass([this.captionEle], CLS_IMGCENTER);
            }
            this.parent.formatter.process(this.parent, e.args, e.args, { insertElement: this.captionEle, selectNode: e.selectNode, subCommand: subCommand });
            this.parent.formatter.editorManager.nodeSelection.setSelectionText(this.parent.getDocument(), imgInner.childNodes[0], imgInner.childNodes[0], 0, imgInner.childNodes[0].textContent.length);
        }
        if (this.quickToolObj) {
            this.quickToolObj.hideImageQTBar();
            sf.base.removeClass([selectNode], 'e-img-focus');
        }
    };
    Image.prototype.insertImgLink = function (e, inputDetails) {
        if (e.selectNode[0].nodeName !== 'IMG') {
            return;
        }
        var obj;
        if (!sf.base.isNullOrUndefined(inputDetails)) {
            obj = { mode: 'EditLink', newWindow: inputDetails.target ? true : false, url: inputDetails.url };
        }
        else {
            obj = { mode: 'InsertLink', newWindow: true, url: '' };
        }
        this.parent.dotNetRef.invokeMethodAsync(showImageDialog, obj);
    };
    Image.prototype.isUrl = function (url) {
        var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/gi;
        return regexp.test(url);
    };
    Image.prototype.insertLink = function (url, target) {
        if (this.selectionObj.selectNode[0].nodeName !== 'IMG') {
            return;
        }
        var linkEle = document.querySelector('.e-rte-img-dialog .e-img-link');
        if (url === '') {
            sf.base.addClass([linkEle], 'e-error');
            linkEle.setSelectionRange(0, url.length);
            linkEle.focus();
            return;
        }
        if (!this.isUrl(url)) {
            url = 'http://' + url;
        }
        else {
            sf.base.removeClass([linkEle], 'e-error');
        }
        if (this.parent.editorMode === 'HTML') {
            this.selectionObj.selection.restore();
        }
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        if (this.selectionObj.selectNode[0].parentElement.nodeName === 'A') {
            this.parent.formatter.process(this.parent, this.selectionObj.args, this.selectionObj.args, {
                url: url, target: target, selectNode: this.selectionObj.selectNode,
                subCommand: this.selectionObj.args.item.subCommand
            });
            this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
            return;
        }
        this.parent.formatter.process(this.parent, this.selectionObj.args, this.selectionObj.args, {
            url: url, target: target, selectNode: this.selectionObj.selectNode,
            subCommand: this.selectionObj.args.item.subCommand,
            selection: this.selectionObj.selection
        });
        var captionEle = sf.base.closest(this.selectionObj.selectNode[0], '.e-img-caption');
        if (captionEle) {
            sf.base.select('.e-img-inner', captionEle).focus();
        }
        this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
    };
    Image.prototype.insertAltText = function (e) {
        if (e.selectNode[0].nodeName !== 'IMG') {
            return;
        }
        var selectNodeAltValue = e.selectNode[0].getAttribute('alt');
        var obj = { mode: 'AltText', altText: ((selectNodeAltValue === null) ? '' : selectNodeAltValue) };
        this.parent.dotNetRef.invokeMethodAsync(showImageDialog, obj);
    };
    Image.prototype.insertAlt = function (altText) {
        this.selectionObj.selection.restore();
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        this.parent.formatter.process(this.parent, this.selectionObj.args, this.selectionObj.args, {
            altText: altText, selectNode: this.selectionObj.selectNode,
            subCommand: this.selectionObj.args.item.subCommand
        });
        this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
    };
    Image.prototype.deleteImg = function (e, keyCode) {
        if (e.selectNode[0].nodeName !== 'IMG') {
            return;
        }
        var args = {
            src: e.selectNode[0].getAttribute('src')
        };
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        e.selection.restore();
        if (this.parent.getEditPanel().querySelector('.e-img-resize')) {
            this.removeResizeEle();
        }
        this.parent.formatter.process(this.parent, e.args, e.args, {
            selectNode: e.selectNode,
            captionClass: CLS_CAPTION,
            subCommand: e.args.item.subCommand
        });
        if (this.quickToolObj) {
            this.quickToolObj.hideImageQTBar();
        }
        this.cancelResizeAction();
        if (sf.base.isNullOrUndefined(keyCode) && this.parent.imageDeleteEnabled) {
            this.parent.dotNetRef.invokeMethodAsync('AfterImageDeleteEvent', args);
        }
    };
    Image.prototype.imageSize = function (e) {
        if (e.selectNode[0].nodeName !== 'IMG') {
            return;
        }
        var selectNode = e.selectNode[0];
        var width = (selectNode.style.width.toString() === 'auto' ||
            selectNode.style.width.toString()) ? selectNode.style.width.toString() : (parseInt(selectNode.getClientRects()[0].width.toString())).toString();
        var height = (selectNode.style.height.toString() === 'auto' ||
            selectNode.style.height.toString()) ? selectNode.style.height.toString() : (parseInt(selectNode.getClientRects()[0].height.toString()).toString());
        var obj = {
            mode: 'Dimension', width: width, height: height, maxWidth: this.getMaxWidth()
        };
        this.parent.dotNetRef.invokeMethodAsync(showImageDialog, obj);
    };
    Image.prototype.insertSize = function (width, height) {
        this.selectionObj.selection.restore();
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        this.parent.formatter.process(this.parent, this.selectionObj.args, this.selectionObj.args, {
            width: width, height: height, selectNode: this.selectionObj.selectNode,
            subCommand: this.selectionObj.args.item.subCommand
        });
        if (this.imgResizeDiv) {
            this.imgResizePos(this.selectionObj.selectNode[0], this.imgResizeDiv);
        }
        this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
    };
    Image.prototype.alignmentSelect = function (e) {
        var item = e.item;
        if (!document.body.contains(document.body.querySelector('.e-rte-quick-toolbar')) || item.command !== 'Images') {
            return;
        }
        var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
        var selectNodeEle = this.parent.formatter.editorManager.nodeSelection.getNodeCollection(range);
        selectNodeEle = (selectNodeEle[0].nodeName === 'IMG') ? selectNodeEle : [this.imgEle];
        var args = { args: e, selectNode: selectNodeEle };
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        switch (item.subCommand) {
            case 'JustifyLeft':
                this.alignImage(args, 'JustifyLeft');
                break;
            case 'JustifyCenter':
                this.alignImage(args, 'JustifyCenter');
                break;
            case 'JustifyRight':
                this.alignImage(args, 'JustifyRight');
                break;
            case 'Inline':
                this.inline(args);
                break;
            case 'Break':
                this.break(args);
                break;
        }
        if (this.quickToolObj) {
            this.quickToolObj.hideImageQTBar();
            sf.base.removeClass([selectNodeEle[0]], 'e-img-focus');
        }
        this.cancelResizeAction();
    };
    Image.prototype.alignImage = function (e, type) {
        var subCommand = (e.args.item) ?
            e.args.item.subCommand : type;
        this.parent.formatter.process(this.parent, e.args, e.args, { selectNode: e.selectNode, subCommand: subCommand });
    };
    Image.prototype.inline = function (e) {
        if (e.selectNode[0].nodeName !== 'IMG') {
            return;
        }
        var subCommand = (e.args.item) ?
            e.args.item.subCommand : 'Inline';
        this.parent.formatter.process(this.parent, e.args, e.args, { selectNode: e.selectNode, subCommand: subCommand });
    };
    Image.prototype.break = function (e) {
        if (e.selectNode[0].nodeName !== 'IMG') {
            return;
        }
        var subCommand = (e.args.item) ?
            e.args.item.subCommand : 'Break';
        this.parent.formatter.process(this.parent, e.args, e.args, { selectNode: e.selectNode, subCommand: subCommand });
    };
    Image.prototype.imageBrowseClick = function () {
        document.getElementById(this.rteId + '_image').querySelector('.e-rte-img-dialog .e-file-select-wrap button').click();
    };
    //#endregion
    //#region Interop methods
    Image.prototype.dialogOpened = function () {
        var dialogContent = this.parent.element.querySelector('.e-rte-img-dialog .e-img-content');
        if (sf.base.isNullOrUndefined(dialogContent)) {
            return;
        }
        var spanElement = document.getElementById(this.rteId + '_dropText');
        var buttonElement = document.getElementById(this.rteId + '_insertImage');
        this.buttonClickElement = sf.base.Browser.isDevice ? spanElement : buttonElement;
        sf.base.EventHandler.add(this.buttonClickElement, 'click', this.imageBrowseClick, this);
        if ((!sf.base.isNullOrUndefined(this.parent.insertImageSettings.path) && this.parent.editorMode === 'Markdown')
            || this.parent.editorMode === 'HTML') {
            dialogContent.querySelector('#' + this.rteId + '_insertImage').focus();
        }
        else {
            dialogContent.querySelector('.e-img-url').focus();
        }
    };
    Image.prototype.imageSelected = function () {
        this.inputUrl.setAttribute('disabled', 'true');
    };
    Image.prototype.imageUploadSuccess = function (url, altText) {
        this.inputUrl = this.parent.element.querySelector('.e-rte-img-dialog .e-img-url');
        if (!sf.base.isNullOrUndefined(this.parent.insertImageSettings.path)) {
            this.uploadUrl = {
                url: url, selection: this.imgUploadSave, altText: altText, selectParent: this.imgUploadSelectedParent,
                width: {
                    width: this.parent.insertImageSettings.width, minWidth: this.parent.insertImageSettings.minWidth,
                    maxWidth: this.getMaxWidth()
                }, height: {
                    height: this.parent.insertImageSettings.height, minHeight: this.parent.insertImageSettings.minHeight,
                    maxHeight: this.parent.insertImageSettings.maxHeight
                }
            };
            this.inputUrl.setAttribute('disabled', 'true');
        }
    };
    Image.prototype.imageUploadComplete = function (base64Str, altText) {
        if (this.parent.editorMode === 'HTML' && sf.base.isNullOrUndefined(this.parent.insertImageSettings.path)) {
            var url = this.parent.insertImageSettings.saveFormat === 'Base64' ? base64Str :
                URL.createObjectURL(convertToBlob(base64Str));
            this.uploadUrl = {
                url: url, selection: this.imgUploadSave, altText: altText, selectParent: this.imgUploadSelectedParent,
                width: {
                    width: this.parent.insertImageSettings.width, minWidth: this.parent.insertImageSettings.minWidth,
                    maxWidth: this.getMaxWidth()
                }, height: {
                    height: this.parent.insertImageSettings.height, minHeight: this.parent.insertImageSettings.minHeight,
                    maxHeight: this.parent.insertImageSettings.maxHeight
                }
            };
            this.inputUrl.setAttribute('disabled', 'true');
        }
    };
    Image.prototype.imageUploadChange = function (url, isStream) {
        this.modifiedUrl = url;
        this.isStreamUrl = isStream;
    };
    Image.prototype.removing = function () {
        this.inputUrl.removeAttribute('disabled');
        if (this.uploadUrl) {
            this.uploadUrl.url = '';
        }
    };
    Image.prototype.dialogClosed = function () {
        if (this.parent.editorMode === 'HTML') {
            this.selectionObj.selection.restore();
        }
        else {
            this.parent.formatter.editorManager.markdownSelection.restore(this.parent.getEditPanel());
        }
        if (this.buttonClickElement) {
            sf.base.EventHandler.remove(this.buttonClickElement, 'click', this.imageBrowseClick);
        }
    };
    Image.prototype.insertImageUrl = function () {
        if (!sf.base.Browser.isDevice) {
            this.inputUrl = this.parent.element.querySelector('.e-rte-img-dialog .e-img-url');
        }
        else {
            this.inputUrl = this.parent.inputElement.ownerDocument.querySelector('#' + this.rteId + '_image').querySelector('.e-rte-img-dialog .e-img-url');
        }
        var url = this.inputUrl.value;
        if (this.isStreamUrl && this.modifiedUrl !== '') {
            this.uploadUrl.url = this.modifiedUrl;
            this.modifiedUrl = '';
        }
        if (this.parent.formatter.getUndoRedoStack().length === 0) {
            this.parent.formatter.saveData();
        }
        if (!sf.base.isNullOrUndefined(this.uploadUrl) && this.uploadUrl.url !== '') {
            this.uploadUrl.cssClass = this.parent.insertImageSettings.display === 'inline' ? CLS_IMGINLINE : CLS_IMGBREAK;
            this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
            this.parent.formatter.process(this.parent, this.selectionObj.args, this.selectionObj.args.originalEvent, this.uploadUrl);
            if (this.parent.getEditPanel().querySelector('.e-img-resize')) {
                this.imgEle.style.outline = '';
                this.removeResizeEle();
            }
        }
        else if (url !== '') {
            if (this.parent.editorMode === 'HTML' && sf.base.isNullOrUndefined(sf.base.closest(this.selectionObj.selection.range.startContainer.parentNode, '#' + this.parent.getPanel().id))) {
                this.parent.getEditPanel().focus();
                var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
                this.selectionObj.selection = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
                this.selectionObj.selectParent = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
            }
            var regex = /[\w-]+.(jpg|png|jpeg|gif)/g;
            var matchUrl = (!sf.base.isNullOrUndefined(url.match(regex)) && this.parent.editorMode === 'HTML') ? url.match(regex)[0] : '';
            var value = {
                cssClass: (this.parent.insertImageSettings.display === 'inline' ? CLS_IMGINLINE : CLS_IMGBREAK),
                url: url, selection: this.selectionObj.selection, altText: matchUrl,
                selectParent: this.selectionObj.selectParent, width: {
                    width: this.parent.insertImageSettings.width, minWidth: this.parent.insertImageSettings.minWidth,
                    maxWidth: this.getMaxWidth()
                },
                height: {
                    height: this.parent.insertImageSettings.height, minHeight: this.parent.insertImageSettings.minHeight,
                    maxHeight: this.parent.insertImageSettings.maxHeight
                }
            };
            this.parent.formatter.process(this.parent, this.selectionObj.args, this.selectionObj.args.originalEvent, value);
            this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
        }
    };
    Image.prototype.showDialog = function (isExternal, event, selection, ele, parentEle) {
        var range;
        var save;
        var selectNodeEle;
        var selectParentEle;
        if (isExternal && !sf.base.isNullOrUndefined(this.parent.formatter.editorManager.nodeSelection)) {
            range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            save = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
            selectNodeEle = this.parent.formatter.editorManager.nodeSelection.getNodeCollection(range);
            selectParentEle = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
        }
        else {
            save = selection;
            selectNodeEle = ele;
            selectParentEle = parentEle;
        }
        if (this.parent.editorMode === 'HTML') {
            this.imageDialog({
                args: {
                    item: { command: 'Images', subCommand: 'Image' },
                    originalEvent: isExternal ? undefined : event
                },
                selectNode: selectNodeEle,
                selection: save,
                selectParent: selectParentEle
            });
        }
        else {
            this.imageDialog({
                args: {
                    item: { command: 'Images', subCommand: 'Image' },
                    originalEvent: isExternal ? undefined : event
                },
                member: 'image',
                text: this.parent.formatter.editorManager.markdownSelection.getSelectedText(this.parent.getEditPanel()),
                module: 'Markdown',
                name: 'insertImage'
            });
        }
    };
    //#endregion
    //#region Event handler methods
    Image.prototype.onCutHandler = function () {
        if (this.imgResizeDiv && this.parent.getEditPanel().contains(this.imgResizeDiv)) {
            this.cancelResizeAction();
        }
    };
    Image.prototype.onIframeMouseDown = function (e) {
        this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
        this.onDocumentClick(e);
    };
    Image.prototype.onDocumentClick = function (e) {
        var target = e.target;
        if (target.nodeName === 'IMG') {
            this.imgEle = target;
        }
        var dlgEle = document.body.querySelector('#' + this.rteId + '_image.e-rte-img-dialog');
        if (!sf.base.isNullOrUndefined(dlgEle) && ((!sf.base.closest(target, '#' + this.rteId + '_image') && this.parent.toolbarSettings.enable && this.parent.getToolbarElement() &&
            !this.parent.getToolbarElement().contains(e.target)) ||
            (this.parent.getToolbarElement() && this.parent.getToolbarElement().contains(e.target) &&
                !sf.base.closest(target, '#' + this.rteId + '_toolbar_Image') &&
                !target.querySelector('#' + this.rteId + '_toolbar_Image')))) {
            if (e.offsetX > e.target.clientWidth || e.offsetY > e.target.clientHeight) {
            }
            else {
                this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, outsideClicked);
                this.parent.isBlur = true;
                if (sf.base.Browser.isIE) {
                    dispatchEvent(this.parent.element, 'focusout');
                }
            }
        }
        if (e.target.tagName !== 'IMG' && this.imgResizeDiv && !this.quickToolObj &&
            this.parent.getEditPanel().contains(this.imgResizeDiv)) {
            this.cancelResizeAction();
        }
        if (this.parent.getEditPanel().querySelector('.e-img-resize')) {
            if (target.tagName !== 'IMG') {
                this.removeResizeEle();
            }
            if (target.tagName !== 'IMG' && !sf.base.isNullOrUndefined(this.imgEle)) {
                this.imgEle.style.outline = '';
            }
            else if (!sf.base.isNullOrUndefined(this.prevSelectedImgEle) && this.prevSelectedImgEle !== target) {
                this.prevSelectedImgEle.style.outline = '';
            }
        }
    };
    Image.prototype.imageClick = function (e) {
        if (sf.base.Browser.isDevice) {
            if ((e.target.tagName === 'IMG' &&
                e.target.parentElement.tagName === 'A') ||
                (e.target.tagName === 'IMG')) {
                this.parent.getEditPanel().setAttribute('contenteditable', 'false');
                e.target.focus();
            }
            else {
                if (!this.parent.readonly) {
                    this.parent.getEditPanel().setAttribute('contenteditable', 'true');
                }
            }
        }
        if (e.target.tagName === 'IMG' &&
            e.target.parentElement.tagName === 'A') {
            e.preventDefault();
        }
    };
    Image.prototype.onKeyDown = function (event) {
        var originalEvent = event.args;
        var range;
        var save;
        var selectNodeEle;
        var selectParentEle;
        this.deletedImg = [];
        var isCursor;
        var keyCodeValues = [27, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123,
            44, 45, 9, 16, 17, 18, 19, 20, 33, 34, 35, 36, 37, 38, 39, 40, 91, 92, 93, 144, 145, 182, 183];
        if (this.parent.editorMode === 'HTML') {
            range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            isCursor = range.startContainer === range.endContainer && range.startOffset === range.endOffset;
        }
        if (!isCursor && this.parent.editorMode === 'HTML' && keyCodeValues.indexOf(originalEvent.which) < 0) {
            var nodes = this.parent.formatter.editorManager.nodeSelection.getNodeCollection(range);
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].nodeName === 'IMG') {
                    this.deletedImg.push(nodes[i]);
                }
            }
        }
        if (this.parent.editorMode === 'HTML' && ((originalEvent.which === 8 && originalEvent.code === 'Backspace') ||
            (originalEvent.which === 46 && originalEvent.code === 'Delete'))) {
            var isCursor_1 = range.startContainer === range.endContainer && range.startOffset === range.endOffset;
            if ((originalEvent.which === 8 && originalEvent.code === 'Backspace' && isCursor_1)) {
                this.checkImageBack(range);
            }
            else if ((originalEvent.which === 46 && originalEvent.code === 'Delete' && isCursor_1)) {
                this.checkImageDel(range);
            }
        }
        if (!sf.base.isNullOrUndefined(this.parent.formatter.editorManager.nodeSelection) &&
            originalEvent.code !== 'KeyK') {
            range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            save = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
            selectNodeEle = this.parent.formatter.editorManager.nodeSelection.getNodeCollection(range);
            selectParentEle = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
            if (!originalEvent.ctrlKey && originalEvent.key && (originalEvent.key.length === 1 || originalEvent.action === 'enter') &&
                (selectParentEle[0].tagName === 'IMG') && selectParentEle[0].parentElement) {
                var prev = selectParentEle[0].parentElement.childNodes[0];
                if (this.parent.getEditPanel().querySelector('.e-img-resize')) {
                    this.removeResizeEle();
                }
                this.parent.formatter.editorManager.nodeSelection.setSelectionText(this.parent.getDocument(), prev, prev, prev.textContent.length, prev.textContent.length);
                sf.base.removeClass([selectParentEle[0]], 'e-img-focus');
                this.quickToolObj.hideImageQTBar();
            }
        }
        if (originalEvent.ctrlKey && (originalEvent.keyCode === 89 || originalEvent.keyCode === 90)) {
            this.undoStack({ subCommand: (originalEvent.keyCode === 90 ? 'undo' : 'redo') });
        }
        if (originalEvent.keyCode === 8 || originalEvent.keyCode === 46) {
            if (selectNodeEle && selectNodeEle[0].nodeName === 'IMG' && selectNodeEle.length < 1) {
                originalEvent.preventDefault();
                var event_1 = {
                    selectNode: selectNodeEle, selection: save, selectParent: selectParentEle,
                    args: {
                        item: { command: 'Images', subCommand: 'Remove' },
                        originalEvent: originalEvent
                    }
                };
                this.deleteImg(event_1, originalEvent.keyCode);
            }
            if (this.parent.getEditPanel().querySelector('.e-img-resize')) {
                this.removeResizeEle();
            }
        }
        switch (originalEvent.action) {
            case 'escape':
                this.parent.dotNetRef.invokeMethodAsync(closeImageDialog, null);
                break;
            case 'insert-image':
                this.showDialog(false, originalEvent, save, selectNodeEle, selectParentEle);
                originalEvent.preventDefault();
                break;
        }
    };
    Image.prototype.onKeyUp = function (event) {
        if (!sf.base.isNullOrUndefined(this.deletedImg) && this.deletedImg.length > 0) {
            for (var i = 0; i < this.deletedImg.length; i++) {
                var args = {
                    src: this.deletedImg[i].getAttribute('src')
                };
                if (this.parent.imageDeleteEnabled) {
                    this.parent.dotNetRef.invokeMethodAsync('AfterImageDeleteEvent', args);
                }
            }
        }
    };
    Image.prototype.resizeStart = function (e, ele) {
        var _this = this;
        if (this.parent.readonly) {
            return;
        }
        var target = ele ? ele : e.target;
        this.prevSelectedImgEle = this.imgEle;
        if (target.tagName === 'IMG') {
            this.parent.defaultResize(e, false);
            var img = target;
            if (this.imgResizeDiv && this.parent.getEditPanel().contains(this.imgResizeDiv)) {
                sf.base.detach(this.imgResizeDiv);
            }
            this.imageResize(img);
        }
        if (target.classList.contains('e-rte-imageboxmark')) {
            if (this.parent.formatter.getUndoRedoStack().length === 0) {
                this.parent.formatter.saveData();
            }
            this.pageX = this.getPointX(e);
            this.pageY = this.getPointY(e);
            e.preventDefault();
            e.stopImmediatePropagation();
            this.resizeBtnInit();
            if (this.quickToolObj) {
                this.quickToolObj.hideImageQTBar();
            }
            if (target.classList.contains('e-rte-topLeft')) {
                this.resizeBtnStat.topLeft = true;
            }
            if (target.classList.contains('e-rte-topRight')) {
                this.resizeBtnStat.topRight = true;
            }
            if (target.classList.contains('e-rte-botLeft')) {
                this.resizeBtnStat.botLeft = true;
            }
            if (target.classList.contains('e-rte-botRight')) {
                this.resizeBtnStat.botRight = true;
            }
            if (sf.base.Browser.isDevice && this.parent.getEditPanel().contains(this.imgResizeDiv) &&
                !this.imgResizeDiv.classList.contains('e-mob-span')) {
                sf.base.addClass([this.imgResizeDiv], 'e-mob-span');
            }
            else {
                var args = { requestType: 'Images' };
                if (this.parent.onResizeStartEnabled) {
                    // @ts-ignore-start
                    this.parent.dotNetRef.invokeMethodAsync('ResizeStartEvent', args).then(function (resizeStartArgs) {
                        // @ts-ignore-end
                        if (resizeStartArgs.cancel) {
                            _this.cancelResizeAction();
                        }
                    });
                }
            }
            sf.base.EventHandler.add(this.parent.getDocument(), sf.base.Browser.touchEndEvent, this.resizeEnd, this);
        }
    };
    Image.prototype.resizing = function (e) {
        if (this.imgEle.offsetWidth >= this.getMaxWidth()) {
            this.imgEle.style.maxHeight = this.imgEle.offsetHeight + 'px';
        }
        var pageX = this.getPointX(e);
        var pageY = this.getPointY(e);
        var mouseX = (this.resizeBtnStat.botLeft || this.resizeBtnStat.topLeft) ? -(pageX - this.pageX) : (pageX - this.pageX);
        var mouseY = (this.resizeBtnStat.topLeft || this.resizeBtnStat.topRight) ? -(pageY - this.pageY) : (pageY - this.pageY);
        var width = parseInt(this.imgDupPos.width, 10) + mouseX;
        var height = parseInt(this.imgDupPos.height, 10) + mouseY;
        this.pageX = pageX;
        this.pageY = pageY;
        if (this.resizeBtnStat.botRight) {
            this.imgDupMouseMove(width + 'px', height + 'px', e);
        }
        else if (this.resizeBtnStat.botLeft) {
            this.imgDupMouseMove(width + 'px', height + 'px', e);
        }
        else if (this.resizeBtnStat.topRight) {
            this.imgDupMouseMove(width + 'px', height + 'px', e);
        }
        else if (this.resizeBtnStat.topLeft) {
            this.imgDupMouseMove(width + 'px', height + 'px', e);
        }
    };
    Image.prototype.resizeEnd = function (e) {
        this.resizeBtnInit();
        this.imgEle.parentElement.style.cursor = 'auto';
        if (sf.base.Browser.isDevice) {
            sf.base.removeClass([e.target.parentElement], 'e-mob-span');
        }
        var args = { requestType: 'Images' };
        if (this.parent.onResizeStopEnabled) {
            this.parent.dotNetRef.invokeMethodAsync('ResizeStopEvent', args);
        }
        this.parent.formatter.editorManager.observer.on(checkUndo, this.undoStack, this);
        this.parent.formatter.saveData();
    };
    Image.prototype.dragStart = function (e) {
        if (e.target.nodeName === 'IMG') {
            // @ts-ignore-start
            this.parent.dotNetRef.invokeMethodAsync('ActionBeginEvent', e).then(function (actionBeginArgs) {
                // @ts-ignore-end
                if (actionBeginArgs.cancel) {
                    e.preventDefault();
                }
                else {
                    e.dataTransfer.effectAllowed = 'copyMove';
                    e.target.classList.add(CLS_RTE_DRAG_IMAGE);
                }
            });
        }
        else {
            return true;
        }
    };
    
    Image.prototype.dragOver = function (e) {
        if ((sf.base.Browser.info.name === 'edge' && e.dataTransfer.items[0].type.split('/')[0].indexOf('image') > -1) ||
            (sf.base.Browser.isIE && e.dataTransfer.types[0] === 'Files')) {
            e.preventDefault();
        }
        else {
            return true;
        }
    };
    
    Image.prototype.dragEnter = function (e) {
        e.dataTransfer.dropEffect = 'copy';
        e.preventDefault();
    };
    
    Image.prototype.dragDrop = function (e) {
        var _this = this;
        this.imageDragArgs = e;
        this.dropFiles = e.dataTransfer.files;
        var imgElement = this.parent.inputElement.ownerDocument.querySelector('.' + CLS_RTE_DRAG_IMAGE);
        if (imgElement && imgElement.tagName === 'IMG') {
            e.preventDefault();
            if (e.dataTransfer.files.length <= 0) {
                // @ts-ignore-start
                this.parent.dotNetRef.invokeMethodAsync('ActionBeginEvent', e).then(function (actionBeginArgs) {
                    // @ts-ignore-end
                    if (!actionBeginArgs.cancel) {
                        if (sf.base.closest(e.target, '#' + _this.rteId + '_toolbar') ||
                            _this.parent.inputElement.contentEditable === 'false') {
                            return;
                        }
                        if (_this.parent.element.querySelector('.' + CLS_IMG_RESIZE)) {
                            sf.base.detach(_this.imgResizeDiv);
                        }
                        _this.selectRange(e);
                        var uploadArea = _this.parent.element.querySelector('.' + CLS_DROPAREA);
                        if (uploadArea) {
                            return;
                        }
                        _this.insertDragImage(e, _this.dropFiles);
                    }
                });
            }
        }
        else {
            return true;
        }
    };
    //#endregion
    Image.prototype.destroy = function () {
        this.prevSelectedImgEle = undefined;
        this.removeEventListener();
    };
    return Image;
}());

/**
 * XhtmlValidation module called when set enableXhtml as true
 */
var XhtmlValidation = /** @class */ (function () {
    function XhtmlValidation(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    XhtmlValidation.prototype.addEventListener = function () {
        this.parent.observer.on(xhtmlValidation, this.enableXhtmlValidation, this);
        this.parent.observer.on(destroy, this.removeEventListener, this);
    };
    XhtmlValidation.prototype.removeEventListener = function () {
        this.parent.observer.off(xhtmlValidation, this.enableXhtmlValidation);
        this.parent.observer.off(destroy, this.removeEventListener);
    };
    XhtmlValidation.prototype.enableXhtmlValidation = function () {
        if (this.parent.enableXhtml) {
            if (sf.base.isNullOrUndefined(this.parent.inputElement)) {
                this.currentElement = this.parent.element;
            }
            else {
                this.currentElement = this.parent.inputElement;
            }
            this.clean(this.currentElement);
            this.AddRootElement();
            this.ImageTags();
            this.removeTags();
            this.RemoveUnsupported();
            this.currentElement.innerHTML = this.selfEncloseValidation(this.currentElement.innerHTML);
        }
    };
    XhtmlValidation.prototype.selfEncloseValidation = function (currentValue) {
        currentValue = currentValue.replace(/<br>/g, '<br/>').replace(/<hr>/g, '<hr/>').replace(/ /g, ' ');
        var valueTemp;
        var valueDupe = [];
        var valueOriginal = [];
        var imgRegexp = [/<img(.*?)>/gi, /<area(.*?)>/gi, /<base(.*?)>/gi, /<col (.*?)>/gi, /<embed(.*?)>/gi,
            /<input(.*?)>/gi, /<link(.*?)>/gi, /<meta(.*?)>/gi, /<param(.*?)>/gi, /<source(.*?)>/gi,
            /<track(.*?)>/gi, /<wbr(.*?)>/gi];
        for (var j = 0; j < imgRegexp.length; j++) {
            valueTemp = imgRegexp[j].exec(currentValue);
            while ((valueTemp) !== null) {
                valueDupe.push(valueTemp[0].toString());
                valueTemp = imgRegexp[j].exec(currentValue);
            }
            valueOriginal = valueDupe.slice(0);
            for (var i = 0; i < valueDupe.length; i++) {
                if (valueDupe[i].indexOf('/') === -1 || valueDupe[i].lastIndexOf('/') !== valueDupe[i].length - 2) {
                    valueDupe[i] = valueDupe[i].substr(0, valueDupe[i].length - 1) + ' /' +
                        valueDupe[i].substr(valueDupe[i].length - 1, valueDupe[i].length);
                }
            }
            for (var g = 0; g <= valueDupe.length - 1; g++) {
                currentValue = currentValue.replace(valueOriginal[g], valueDupe[g]);
            }
        }
        return currentValue;
    };
    XhtmlValidation.prototype.AddRootElement = function () {
        if ((this.currentElement.childNodes.length === 1 && this.currentElement.firstChild.nodeName !== 'DIV') ||
            this.currentElement.childNodes.length > 1) {
            var parentEle = document.createElement('div');
            while (this.currentElement.childNodes.length > 0) {
                parentEle.appendChild(this.currentElement.childNodes[0]);
            }
            this.currentElement.appendChild(parentEle);
        }
    };
    
    XhtmlValidation.prototype.clean = function (node) {
        for (var n = 0; n < node.childNodes.length; n++) {
            var child = node.childNodes[n];
            if (child.nodeType === 8 || child.nodeName === 'V:IMAGE') {
                node.removeChild(child);
                n--;
            }
            else if (child.nodeType === 1) {
                this.clean(child);
            }
        }
        return this.currentElement.innerHTML;
    };
    XhtmlValidation.prototype.ImageTags = function () {
        var imgNodes = this.currentElement.querySelectorAll('IMG');
        for (var i = imgNodes.length - 1; i >= 0; i--) {
            if (!imgNodes[i].hasAttribute('alt')) {
                var img = imgNodes[i];
                img.setAttribute('alt', '');
            }
        }
    };
    
    XhtmlValidation.prototype.removeTags = function () {
        var removeAttribute = [['br', 'ul'], ['br', 'ol'], ['table', 'span'], ['div', 'span'], ['p', 'span']];
        for (var i = 0; i < removeAttribute.length; i++) {
            this.RemoveElementNode(removeAttribute[i][0], removeAttribute[i][1]);
        }
    };
    
    XhtmlValidation.prototype.RemoveElementNode = function (rmvNode, parentNode) {
        var parentArray = this.currentElement.querySelectorAll(parentNode);
        for (var i = 0; i < parentArray.length; i++) {
            var rmvArray = parentArray[i].querySelectorAll(rmvNode);
            for (var j = rmvArray.length; j > 0; j--) {
                sf.base.detach(rmvArray[j - 1]);
            }
        }
    };
    
    XhtmlValidation.prototype.RemoveUnsupported = function () {
        var underlineEle = this.currentElement.querySelectorAll('u');
        for (var i = underlineEle.length - 1; i >= 0; i--) {
            var spanEle = document.createElement('span');
            spanEle.style.textDecoration = 'underline';
            spanEle.innerHTML = underlineEle[i].innerHTML;
            underlineEle[i].parentNode.insertBefore(spanEle, underlineEle[i]);
            sf.base.detach(underlineEle[i]);
        }
        var strongEle = this.currentElement.querySelectorAll('strong');
        for (var i = strongEle.length - 1; i >= 0; i--) {
            var boldEle = document.createElement('b');
            boldEle.innerHTML = strongEle[i].innerHTML;
            strongEle[i].parentNode.insertBefore(boldEle, strongEle[i]);
            sf.base.detach(strongEle[i]);
        }
        var attrArray = ['language', 'role', 'target', 'contenteditable', 'cellspacing',
            'cellpadding', 'border', 'valign', 'colspan'];
        for (var i = 0; i <= attrArray.length; i++) {
            this.RemoveAttributeByName(attrArray[i]);
        }
    };
    
    XhtmlValidation.prototype.RemoveAttributeByName = function (attrName) {
        if (this.currentElement.firstChild !== null) {
            if (this.currentElement.firstChild.nodeType !== 3) {
                for (var i = 0; i < this.currentElement.childNodes.length; i++) {
                    var ele = this.currentElement.childNodes[i];
                    if (ele.nodeType !== 3 && ele.nodeName !== 'TABLE' && ele.nodeName !== 'TBODY' && ele.nodeName !== 'THEAD' &&
                        ele.nodeName !== 'TH' && ele.nodeName !== 'TR' && ele.nodeName !== 'TD') {
                        if (ele.hasAttribute(attrName)) {
                            ele.removeAttribute(attrName);
                        }
                        if (ele.hasChildNodes()) {
                            for (var j = 0; j < ele.childNodes.length; j++) {
                                var childEle = ele.childNodes[j];
                                if (childEle.nodeType !== 3 && childEle.nodeName !== 'TABLE' && childEle.nodeName !== 'TBODY' &&
                                    childEle.nodeName !== 'THEAD' && childEle.nodeName !== 'TH' && childEle.nodeName !== 'TR' &&
                                    childEle.nodeName !== 'TD' && childEle.hasAttribute(attrName)) {
                                    childEle.removeAttribute(attrName);
                                }
                                if (childEle.hasChildNodes()) {
                                    for (var k = 0; k < childEle.childNodes.length; k++) {
                                        if (childEle.childNodes[k].nodeType !== 3 && childEle.childNodes[k].nodeName !== 'TABLE' &&
                                            childEle.childNodes[k].nodeName !== 'TBODY' && childEle.childNodes[k].nodeName !== 'THEAD' &&
                                            childEle.childNodes[k].nodeName !== 'TH' && childEle.childNodes[k].nodeName !== 'TR'
                                            && childEle.childNodes[k].nodeName !== 'TD'
                                            && childEle.childNodes[k].hasAttribute(attrName)) {
                                            childEle.childNodes[k].removeAttribute(attrName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    };
    
    return XhtmlValidation;
}());

/**
 * Is formatted or not.
 *
 * @hidden
 * @deprecated
 */
var IsFormatted = /** @class */ (function () {
    function IsFormatted() {
    }
    /**
     * getFormattedNode method
     *
     * @param {Node} node - specifies the node.
     * @param {string} format - specifies the string value.
     * @param {Node} endNode - specifies the end node
     * @returns {Node} - returns the node
     * @hidden
     * @deprecated
     */
    IsFormatted.prototype.getFormattedNode = function (node, format, endNode) {
        var parentNode = this.getFormatParent(node, format, endNode);
        if (parentNode !== null && parentNode !== endNode) {
            return parentNode;
        }
        return null;
    };
    IsFormatted.prototype.getFormatParent = function (node, format, endNode) {
        do {
            node = node.parentNode;
        } while (node && (node !== endNode) && !this.isFormattedNode(node, format));
        return node;
    };
    IsFormatted.prototype.isFormattedNode = function (node, format) {
        switch (format) {
            case 'bold':
                return IsFormatted.isBold(node);
            case 'italic':
                return IsFormatted.isItalic(node);
            case 'underline':
                return IsFormatted.isUnderline(node);
            case 'strikethrough':
                return IsFormatted.isStrikethrough(node);
            case 'superscript':
                return IsFormatted.isSuperscript(node);
            case 'subscript':
                return IsFormatted.isSubscript(node);
            case 'fontcolor':
                return this.isFontColor(node);
            case 'fontname':
                return this.isFontName(node);
            case 'fontsize':
                return this.isFontSize(node);
            case 'backgroundcolor':
                return this.isBackgroundColor(node);
            default:
                return false;
        }
    };
    /**
     * isBold method
     *
     * @param {Node} node - specifies the node value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    IsFormatted.isBold = function (node) {
        var validTags = ['strong', 'b'];
        if (validTags.indexOf(node.nodeName.toLowerCase()) !== -1) {
            return true;
        }
        else if (this.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            node.style && node.style.fontWeight === 'bold') {
            return true;
        }
        else {
            return false;
        }
    };
    /**
     * isItalic method
     *
     * @param {Node} node - specifies the node value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    IsFormatted.isItalic = function (node) {
        var validTags = ['em', 'i'];
        if (validTags.indexOf(node.nodeName.toLowerCase()) !== -1) {
            return true;
        }
        else if (this.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            node.style && node.style.fontStyle === 'italic') {
            return true;
        }
        else {
            return false;
        }
    };
    /**
     * isUnderline method
     *
     * @param {Node} node - specifies the node value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    IsFormatted.isUnderline = function (node) {
        var validTags = ['u'];
        if (validTags.indexOf(node.nodeName.toLowerCase()) !== -1) {
            return true;
        }
        else if (this.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            node.style && node.style.textDecoration === 'underline') {
            return true;
        }
        else {
            return false;
        }
    };
    /**
     * isStrikethrough method
     *
     * @param {Node} node - specifies the node value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    IsFormatted.isStrikethrough = function (node) {
        var validTags = ['del', 'strike'];
        if (validTags.indexOf(node.nodeName.toLowerCase()) !== -1) {
            return true;
        }
        else if (this.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            node.style && node.style.textDecoration === 'line-through') {
            return true;
        }
        else {
            return false;
        }
    };
    /**
     * isSuperscript method
     *
     * @param {Node} node - specifies the node value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    IsFormatted.isSuperscript = function (node) {
        var validTags = ['sup'];
        if (validTags.indexOf(node.nodeName.toLowerCase()) !== -1) {
            return true;
        }
        else {
            return false;
        }
    };
    /**
     * isSubscript method
     *
     * @param {Node} node - specifies the node value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    IsFormatted.isSubscript = function (node) {
        var validTags = ['sub'];
        if (validTags.indexOf(node.nodeName.toLowerCase()) !== -1) {
            return true;
        }
        else {
            return false;
        }
    };
    IsFormatted.prototype.isFontColor = function (node) {
        var color = node.style && node.style.color;
        if (IsFormatted.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            color !== null && color !== '' && color !== undefined) {
            return true;
        }
        else {
            return false;
        }
    };
    IsFormatted.prototype.isBackgroundColor = function (node) {
        var backColor = node.style && node.style.backgroundColor;
        if (IsFormatted.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            backColor !== null && backColor !== '' && backColor !== undefined) {
            return true;
        }
        else {
            return false;
        }
    };
    IsFormatted.prototype.isFontSize = function (node) {
        var size = node.style && node.style.fontSize;
        if (IsFormatted.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            size !== null && size !== '' && size !== undefined) {
            return true;
        }
        else {
            return false;
        }
    };
    IsFormatted.prototype.isFontName = function (node) {
        var name = node.style && node.style.fontFamily;
        if (IsFormatted.inlineTags.indexOf(node.nodeName.toLowerCase()) !== -1 &&
            name !== null && name !== '' && name !== undefined) {
            return true;
        }
        else {
            return false;
        }
    };
    // Get Formatted Node
    IsFormatted.inlineTags = [
        'a',
        'abbr',
        'acronym',
        'b',
        'bdo',
        'big',
        'cite',
        'code',
        'dfn',
        'em',
        'i',
        'kbd',
        'label',
        'q',
        'samp',
        'small',
        'span',
        'strong',
        'sub',
        'sup',
        'tt',
        'u',
        'var',
        'del'
    ];
    return IsFormatted;
}());

/**
 * Constant values for EditorManager
 */
/**
 * Image plugin events
 *
 * @hidden
 */
var IMAGE = 'INSERT-IMAGE';
var TABLE = 'INSERT-TABLE';
var LINK = 'INSERT-LINK';
var INSERT_ROW = 'INSERT-ROW';
var INSERT_COLUMN = 'INSERT-COLUMN';
var DELETEROW = 'DELETE-ROW';
var DELETECOLUMN = 'DELETE-COLUMN';
var REMOVETABLE = 'REMOVE-TABLE';
var TABLEHEADER = 'TABLE-HEADER';
var TABLE_VERTICAL_ALIGN = 'TABLE_VERTICAL_ALIGN';
var TABLE_MERGE = 'TABLE_MERGE';
var TABLE_VERTICAL_SPLIT = 'TABLE_VERTICAL_SPLIT';
var TABLE_HORIZONTAL_SPLIT = 'TABLE_HORIZONTAL_SPLIT';
var TABLE_MOVE = 'TABLE_MOVE';
/**
 * Alignments plugin events
 *
 * @hidden
 */
var ALIGNMENT_TYPE = 'alignment-type';
/**
 * Indents plugin events
 *
 * @hidden
 */
var INDENT_TYPE = 'indent-type';
/**
 * Constant tag names
 *
 * @hidden
 */
var DEFAULT_TAG = 'p';
/**
 * @hidden
 */
var BLOCK_TAGS = ['address', 'article', 'aside', 'audio', 'blockquote',
    'canvas', 'details', 'dd', 'div', 'dl', 'dt', 'fieldset', 'figcaption', 'figure', 'footer',
    'form', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'header', 'hgroup', 'hr', 'li', 'main', 'nav',
    'noscript', 'ol', 'output', 'p', 'pre', 'section', 'table', 'tbody', 'td', 'tfoot', 'th',
    'thead', 'tr', 'ul', 'video', 'body'];
/**
 * @hidden
 */
var IGNORE_BLOCK_TAGS = ['td', 'th'];
/**
 * @hidden
 */
var TABLE_BLOCK_TAGS = ['table', 'tbody', 'td', 'tfoot', 'th',
    'thead', 'tr'];
/**
 * Selection plugin events
 *
 * @hidden
 */
var SELECTION_TYPE = 'selection-type';
/**
 * Insert HTML plugin events
 *
 * @hidden
 */
var INSERTHTML_TYPE = 'inserthtml-type';
/**
 * Insert Text plugin events
 *
 * @hidden
 */
var INSERT_TEXT_TYPE = 'insert-text-type';
/**
 * Clear Format HTML plugin events
 *
 * @hidden
 */
var CLEAR_TYPE = 'clear-type';

/**
 * `Selection` module is used to handle RTE Selections.
 */
var NodeSelection = /** @class */ (function () {
    function NodeSelection() {
        this.startNodeName = [];
        this.endNodeName = [];
    }
    NodeSelection.prototype.saveInstance = function (range, body) {
        this.range = range.cloneRange();
        this.rootNode = this.documentFromRange(range);
        this.body = body;
        this.startContainer = this.getNodeArray(range.startContainer, true);
        this.endContainer = this.getNodeArray(range.endContainer, false);
        this.startOffset = range.startOffset;
        this.endOffset = range.endOffset;
        this.html = this.body.innerHTML;
        return this;
    };
    NodeSelection.prototype.documentFromRange = function (range) {
        return (9 === range.startContainer.nodeType) ? range.startContainer : range.startContainer.ownerDocument;
    };
    NodeSelection.prototype.getRange = function (docElement) {
        var select$$1 = this.get(docElement);
        var range = select$$1 && select$$1.rangeCount > 0 ? select$$1.getRangeAt(select$$1.rangeCount - 1) : docElement.createRange();
        return (range.startContainer !== docElement || range.endContainer !== docElement
            || range.startOffset || range.endOffset || (range.setStart(docElement.body, 0), range.collapse(!0)), range);
    };
    /**
     * get method
     *
     * @param {Document} docElement - specifies the get function
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.get = function (docElement) {
        return docElement.defaultView.getSelection();
    };
    /**
     * save method
     *
     * @param {Range} range - range value.
     * @param {Document} docElement - specifies the document.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.save = function (range, docElement) {
        range = (range) ? range.cloneRange() : this.getRange(docElement);
        return this.saveInstance(range, docElement.body);
    };
    /**
     * getIndex method
     *
     * @param {Node} node - specifies the node value.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getIndex = function (node) {
        var index;
        var num = 0;
        node = !node.previousSibling && node.tagName === 'BR' ? node : node.previousSibling;
        if (node) {
            for (var type = node.nodeType; node; null) {
                index = node.nodeType;
                num++;
                //eslint-disable-next-line
                type = index;
                node = node.previousSibling;
            }
        }
        return num;
    };
    NodeSelection.prototype.isChildNode = function (nodeCollection, parentNode) {
        for (var index = 0; index < parentNode.childNodes.length; index++) {
            if (nodeCollection.indexOf(parentNode.childNodes[index]) > -1) {
                return true;
            }
        }
        return false;
    };
    NodeSelection.prototype.getNode = function (startNode, endNode, nodeCollection) {
        if (endNode === startNode &&
            (startNode.nodeType === 3 || !startNode.firstChild || nodeCollection.indexOf(startNode.firstChild) !== -1
                || this.isChildNode(nodeCollection, startNode))) {
            return null;
        }
        if (nodeCollection.indexOf(startNode.firstChild) === -1 && startNode.firstChild && !this.isChildNode(nodeCollection, startNode)) {
            return startNode.firstChild;
        }
        if (startNode.nextSibling) {
            return startNode.nextSibling;
        }
        if (!startNode.parentNode) {
            return null;
        }
        else {
            return startNode.parentNode;
        }
    };
    /**
     * getNodeCollection method
     *
     * @param {Range} range -specifies the range.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getNodeCollection = function (range) {
        var startNode = range.startContainer.childNodes[range.startOffset]
            || range.startContainer;
        var endNode = range.endContainer.childNodes[(range.endOffset > 0) ? (range.endOffset - 1) : range.endOffset]
            || range.endContainer;
        if (startNode === endNode && startNode.childNodes.length === 0) {
            return [startNode];
        }
        if (range.startOffset === range.endOffset && range.startOffset !== 0 && range.startContainer.nodeName === 'PRE') {
            return [startNode.nodeName === 'BR' || startNode.nodeName === '#text' ? startNode : startNode.childNodes[0]];
        }
        var nodeCollection = [];
        do {
            if (nodeCollection.indexOf(startNode) === -1) {
                nodeCollection.push(startNode);
            }
            startNode = this.getNode(startNode, endNode, nodeCollection);
        } while (startNode);
        return nodeCollection;
    };
    /**
     * getParentNodeCollection method
     *
     * @param {Range} range - specifies the range value.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getParentNodeCollection = function (range) {
        return this.getParentNodes(this.getNodeCollection(range), range);
    };
    /**
     * getParentNodes method
     *
     * @param {Node[]} nodeCollection - specifies the collection of nodes.
     * @param {Range} range - specifies the range values.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getParentNodes = function (nodeCollection, range) {
        nodeCollection = nodeCollection.reverse();
        for (var index = 0; index < nodeCollection.length; index++) {
            if ((nodeCollection.indexOf(nodeCollection[index].parentNode) !== -1)
                || (nodeCollection[index].nodeType === 3 &&
                    range.startContainer !== range.endContainer &&
                    range.startContainer.parentNode !== range.endContainer.parentNode)) {
                nodeCollection.splice(index, 1);
                index--;
            }
            else if (nodeCollection[index].nodeType === 3) {
                nodeCollection[index] = nodeCollection[index].parentNode;
            }
        }
        return nodeCollection;
    };
    /**
     * getSelectionNodeCollection method
     *
     * @param {Range} range - specifies the range value.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getSelectionNodeCollection = function (range) {
        return this.getSelectionNodes(this.getNodeCollection(range));
    };
    /**
     * getSelectionNodeCollection along with BR node method
     *
     * @param {Range} range - specifies the range value.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getSelectionNodeCollectionBr = function (range) {
        return this.getSelectionNodesBr(this.getNodeCollection(range));
    };
    /**
     * getParentNodes method
     *
     * @param {Node[]} nodeCollection - specifies the collection of nodes.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getSelectionNodes = function (nodeCollection) {
        nodeCollection = nodeCollection.reverse();
        var regEx = new RegExp(String.fromCharCode(8203), 'g');
        for (var index = 0; index < nodeCollection.length; index++) {
            if (nodeCollection[index].nodeType !== 3 || (nodeCollection[index].textContent.trim() === '' ||
                (nodeCollection[index].textContent.length === 1 && nodeCollection[index].textContent.match(regEx)))) {
                nodeCollection.splice(index, 1);
                index--;
            }
        }
        return nodeCollection.reverse();
    };
    /**
     * Get selection text nodes with br method.
     *
     * @param {Node[]} nodeCollection - specifies the collection of nodes.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getSelectionNodesBr = function (nodeCollection) {
        nodeCollection = nodeCollection.reverse();
        var regEx = new RegExp(String.fromCharCode(8203), 'g');
        for (var index = 0; index < nodeCollection.length; index++) {
            if (nodeCollection[index].nodeName !== 'BR' &&
                (nodeCollection[index].nodeType !== 3 || (nodeCollection[index].textContent.trim() === '' ||
                    (nodeCollection[index].textContent.length === 1 && nodeCollection[index].textContent.match(regEx))))) {
                nodeCollection.splice(index, 1);
                index--;
            }
        }
        return nodeCollection.reverse();
    };
    /**
     * getInsertNodeCollection method
     *
     * @param {Range} range - specifies the range value.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getInsertNodeCollection = function (range) {
        return this.getInsertNodes(this.getNodeCollection(range));
    };
    /**
     * getInsertNodes method
     *
     * @param {Node[]} nodeCollection - specifies the collection of nodes.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getInsertNodes = function (nodeCollection) {
        nodeCollection = nodeCollection.reverse();
        for (var index = 0; index < nodeCollection.length; index++) {
            if ((nodeCollection[index].childNodes.length !== 0 &&
                nodeCollection[index].nodeType !== 3) ||
                (nodeCollection[index].nodeType === 3 &&
                    nodeCollection[index].textContent === '')) {
                nodeCollection.splice(index, 1);
                index--;
            }
        }
        return nodeCollection.reverse();
    };
    /**
     * getNodeArray method
     *
     * @param {Node} node - specifies the node content.
     * @param {boolean} isStart - specifies the boolean value.
     * @param {Document} root - specifies the root document.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getNodeArray = function (node, isStart, root) {
        var array = [];
        // eslint-disable-next-line
        ((isStart) ? (this.startNodeName = []) : (this.endNodeName = []));
        for (; node !== (root ? root : this.rootNode); null) {
            if (sf.base.isNullOrUndefined(node)) {
                break;
            }
            // eslint-disable-next-line
            (isStart) ? this.startNodeName.push(node.nodeName.toLowerCase()) : this.endNodeName.push(node.nodeName.toLowerCase());
            array.push(this.getIndex(node));
            node = node.parentNode;
        }
        return array;
    };
    NodeSelection.prototype.setRangePoint = function (range, isvalid, num, size) {
        var node = this.rootNode;
        var index = num.length;
        var constant = size;
        for (; index--; null) {
            node = node && node.childNodes[num[index]];
        }
        if (node && constant >= 0 && node.nodeName !== 'html') {
            range[isvalid ? 'setStart' : 'setEnd'](node, constant);
        }
        return range;
    };
    /**
     * restore method
     *
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.restore = function () {
        var range = this.range.cloneRange();
        range = this.setRangePoint(range, true, this.startContainer, this.startOffset);
        range = this.setRangePoint(range, false, this.endContainer, this.endOffset);
        this.selectRange(this.rootNode, range);
        return range;
    };
    NodeSelection.prototype.selectRange = function (docElement, range) {
        this.setRange(docElement, range);
        this.save(range, docElement);
    };
    /**
     * setRange method
     *
     * @param {Document} docElement - specifies the document.
     * @param {Range} range - specifies the range.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.setRange = function (docElement, range) {
        var selection = this.get(docElement);
        selection.removeAllRanges();
        selection.addRange(range);
    };
    /**
     * setSelectionText method
     *
     * @param {Document} docElement - specifies the documrent
     * @param {Node} startNode - specifies the starting node.
     * @param {Node} endNode - specifies the the end node.
     * @param {number} startIndex - specifies the starting index.
     * @param {number} endIndex - specifies the end index.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.setSelectionText = function (docElement, startNode, endNode, startIndex, endIndex) {
        var range = docElement.createRange();
        range.setStart(startNode, startIndex);
        range.setEnd(endNode, endIndex);
        this.setRange(docElement, range);
    };
    /**
     * setSelectionContents method
     *
     * @param {Document} docElement - specifies the document.
     * @param {Node} element - specifies the node.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.setSelectionContents = function (docElement, element) {
        var range = docElement.createRange();
        range.selectNode(element);
        this.setRange(docElement, range);
    };
    /**
     * setSelectionNode method
     *
     * @param {Document} docElement - specifies the document.
     * @param {Node} element - specifies the node.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.setSelectionNode = function (docElement, element) {
        var range = docElement.createRange();
        range.selectNodeContents(element);
        this.setRange(docElement, range);
    };
    /**
     * getSelectedNodes method
     *
     * @param {Document} docElement - specifies the document.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.getSelectedNodes = function (docElement) {
        return this.getNodeCollection(this.getRange(docElement));
    };
    /**
     * Clear method
     *
     * @param {Document} docElement - specifies the document.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.Clear = function (docElement) {
        this.get(docElement).removeAllRanges();
    };
    /**
     * insertParentNode method
     *
     * @param {Document} docElement - specifies the document.
     * @param {Node} newNode - specicfies the new node.
     * @param {Range} range - specifies the range.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.insertParentNode = function (docElement, newNode, range) {
        range.surroundContents(newNode);
        this.selectRange(docElement, range);
    };
    /**
     * setCursorPoint method
     *
     * @param {Document} docElement - specifies the document.
     * @param {Element} element - specifies the element.
     * @param {number} point - specifies the point.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    NodeSelection.prototype.setCursorPoint = function (docElement, element, point) {
        var range = docElement.createRange();
        var selection = docElement.defaultView.getSelection();
        range.setStart(element, point);
        range.collapse(true);
        selection.removeAllRanges();
        selection.addRange(range);
    };
    return NodeSelection;
}());

/**
 * `Selection` module is used to handle RTE Selections.
 */

/**
 * Update Toolbar Status
 *
 * @hidden
 * @deprecated
 */
var statusCollection = getDefaultHtmlTbStatus();
var ToolbarStatus = /** @class */ (function () {
    function ToolbarStatus() {
    }
    /**
     * get method
     *
     * @param {Document} docElement - specifies the document element
     * @param {Node} targetNode - specifies the target node
     * @param {string[]} formatNode - specifies the format node
     * @param {string[]} fontSize - specifies the font size
     * @param {string[]} fontName - specifies the font name.
     * @param {Node} documentNode - specifies the document node.
     * @returns {IToolbarStatus} - returns the toolbar status
     * @hidden
     * @deprecated
     */
    ToolbarStatus.get = function (docElement, targetNode, formatNode, fontSize, fontName, documentNode) {
        var formatCollection = JSON.parse(JSON.stringify(statusCollection));
        var nodeCollection = JSON.parse(JSON.stringify(statusCollection));
        var nodeSelection = new NodeSelection();
        var nodes = documentNode ? [documentNode] : nodeSelection.getNodeCollection(nodeSelection.getRange(docElement));
        var nodesLength = nodes.length;
        var isNodeChanged = false;
        var range = nodeSelection.getRange(docElement);
        for (var index = 0; index < nodes.length; index++) {
            while (nodes[index].nodeType === 3 && range.startContainer.nodeType === 3 && nodes[index].parentNode &&
                nodes[index].parentNode.lastElementChild && nodes[index].parentNode.lastElementChild.nodeName !== 'BR' &&
                (this.getImmediateBlockNode(nodes[index].parentNode)).textContent.replace(/\u200B/g, '').length === 0 &&
                range.startContainer.textContent.replace(/\u200B/g, '').length === 0 &&
                nodeSelection.get(docElement).toString().replace(/\u200B/g, '').length === 0) {
                nodes[index] = nodes[index].parentNode.lastElementChild.firstChild;
                isNodeChanged = true;
            }
            if (isNodeChanged && nodes[index]) {
                nodeSelection.setCursorPoint(docElement, nodes[index], nodes[index].textContent.length);
                isNodeChanged = false;
            }
            if ((nodes[index].nodeName !== 'BR' && nodes[index].nodeType !== 3) ||
                (nodesLength > 1 && nodes[index].nodeType === 3 && nodes[index].textContent.trim() === '')) {
                nodes.splice(index, 1);
                index--;
            }
        }
        for (var index = 0; index < nodes.length; index++) {
            formatCollection = this.getFormatParent(docElement, formatCollection, nodes[index], targetNode, formatNode, fontSize, fontName);
            if ((index === 0 && formatCollection.bold) || !formatCollection.bold) {
                nodeCollection.bold = formatCollection.bold;
            }
            if ((index === 0 && formatCollection.insertcode) || !formatCollection.insertcode) {
                nodeCollection.insertcode = formatCollection.insertcode;
            }
            if ((index === 0 && formatCollection.italic) || !formatCollection.italic) {
                nodeCollection.italic = formatCollection.italic;
            }
            if ((index === 0 && formatCollection.underline) || !formatCollection.underline) {
                nodeCollection.underline = formatCollection.underline;
            }
            if ((index === 0 && formatCollection.strikethrough) || !formatCollection.strikethrough) {
                nodeCollection.strikethrough = formatCollection.strikethrough;
            }
            if ((index === 0 && formatCollection.superscript) || !formatCollection.superscript) {
                nodeCollection.superscript = formatCollection.superscript;
            }
            if ((index === 0 && formatCollection.subscript) || !formatCollection.subscript) {
                nodeCollection.subscript = formatCollection.subscript;
            }
            if ((index === 0 && formatCollection.fontcolor) || !formatCollection.fontcolor) {
                nodeCollection.fontcolor = formatCollection.fontcolor;
            }
            if ((index === 0 && formatCollection.fontname) || !formatCollection.fontname) {
                nodeCollection.fontname = formatCollection.fontname;
            }
            if ((index === 0 && formatCollection.fontsize) || !formatCollection.fontsize) {
                nodeCollection.fontsize = formatCollection.fontsize;
            }
            if ((index === 0 && formatCollection.backgroundcolor) || !formatCollection.backgroundcolor) {
                nodeCollection.backgroundcolor = formatCollection.backgroundcolor;
            }
            if ((index === 0 && formatCollection.orderedlist) || !formatCollection.orderedlist) {
                nodeCollection.orderedlist = formatCollection.orderedlist;
            }
            if ((index === 0 && formatCollection.unorderedlist) || !formatCollection.unorderedlist) {
                nodeCollection.unorderedlist = formatCollection.unorderedlist;
            }
            if ((index === 0 && formatCollection.alignments) || !formatCollection.alignments) {
                nodeCollection.alignments = formatCollection.alignments;
            }
            if ((index === 0 && formatCollection.formats) || !formatCollection.formats) {
                nodeCollection.formats = formatCollection.formats;
            }
            if ((index === 0 && formatCollection.createlink) || !formatCollection.createlink) {
                nodeCollection.createlink = formatCollection.createlink;
            }
            if ((index === 0 && formatCollection.numberFormatList) || !formatCollection.numberFormatList) {
                nodeCollection.numberFormatList = formatCollection.numberFormatList;
            }
            if ((index === 0 && formatCollection.bulletFormatList) || !formatCollection.bulletFormatList) {
                nodeCollection.bulletFormatList = formatCollection.bulletFormatList;
            }
            formatCollection = JSON.parse(JSON.stringify(statusCollection));
        }
        return nodeCollection;
    };
    ToolbarStatus.getImmediateBlockNode = function (node) {
        do {
            node = node.parentNode;
        } while (node && BLOCK_TAGS.indexOf(node.nodeName.toLocaleLowerCase()) < 0);
        return node;
    };
    ToolbarStatus.getFormatParent = function (docElement, formatCollection, node, targetNode, formatNode, fontSize, fontName) {
        if (targetNode.contains(node) ||
            (node.nodeType === 3 && targetNode.nodeType !== 3 && targetNode.contains(node.parentNode))) {
            do {
                formatCollection = this.isFormattedNode(docElement, formatCollection, node, formatNode, fontSize, fontName);
                node = node.parentNode;
            } while (node && (node !== targetNode));
        }
        return formatCollection;
    };
    ToolbarStatus.isFormattedNode = function (docElement, formatCollection, node, formatNode, fontSize, fontName) {
        if (!formatCollection.bold) {
            formatCollection.bold = IsFormatted.isBold(node);
        }
        if (!formatCollection.italic) {
            formatCollection.italic = IsFormatted.isItalic(node);
        }
        if (!formatCollection.underline) {
            formatCollection.underline = IsFormatted.isUnderline(node);
        }
        if (!formatCollection.strikethrough) {
            formatCollection.strikethrough = IsFormatted.isStrikethrough(node);
        }
        if (!formatCollection.superscript) {
            formatCollection.superscript = IsFormatted.isSuperscript(node);
        }
        if (!formatCollection.subscript) {
            formatCollection.subscript = IsFormatted.isSubscript(node);
        }
        if (!formatCollection.fontcolor) {
            formatCollection.fontcolor = this.isFontColor(docElement, node);
        }
        if (!formatCollection.fontname) {
            formatCollection.fontname = this.isFontName(docElement, node, fontName);
        }
        if (!formatCollection.fontsize) {
            formatCollection.fontsize = this.isFontSize(node, fontSize);
        }
        if (!formatCollection.backgroundcolor) {
            formatCollection.backgroundcolor = this.isBackgroundColor(node);
        }
        if (!formatCollection.orderedlist) {
            formatCollection.orderedlist = this.isOrderedList(node);
        }
        if (!formatCollection.unorderedlist) {
            formatCollection.unorderedlist = this.isUnorderedList(node);
        }
        if (!formatCollection.alignments) {
            formatCollection.alignments = this.isAlignment(node);
        }
        if (!formatCollection.formats) {
            formatCollection.formats = this.isFormats(node, formatNode);
            if (formatCollection.formats === 'pre') {
                formatCollection.insertcode = true;
            }
        }
        if (!formatCollection.createlink) {
            formatCollection.createlink = this.isLink(node);
        }
        if (!formatCollection.numberFormatList) {
            formatCollection.numberFormatList = this.isNumberFormatList(node);
        }
        if (!formatCollection.bulletFormatList) {
            formatCollection.bulletFormatList = this.isBulletFormatList(node);
        }
        return formatCollection;
    };
    ToolbarStatus.isFontColor = function (docElement, node) {
        var color = node.style && node.style.color;
        if ((color === null || color === undefined || color === '') && node.nodeType !== 3) {
            color = this.getComputedStyle(docElement, node, 'color');
        }
        if (color !== null && color !== '' && color !== undefined) {
            return color;
        }
        else {
            return null;
        }
    };
    ToolbarStatus.isLink = function (node) {
        if (node.nodeName.toLocaleLowerCase() === 'a') {
            return true;
        }
        else {
            return false;
        }
    };
    ToolbarStatus.isBackgroundColor = function (node) {
        var backColor = node.style && node.style.backgroundColor;
        if (backColor !== null && backColor !== '' && backColor !== undefined) {
            return backColor;
        }
        else {
            return null;
        }
    };
    ToolbarStatus.isFontSize = function (node, fontSize) {
        var size = node.style && node.style.fontSize;
        if ((size !== null && size !== '' && size !== undefined)
            && (fontSize === null || fontSize === undefined || (fontSize.indexOf(size) > -1))) {
            return size;
        }
        else {
            return null;
        }
    };
    ToolbarStatus.isFontName = function (docElement, node, fontName) {
        var name = node.style && node.style.fontFamily;
        if ((name === null || name === undefined || name === '') && node.nodeType !== 3) {
            name = this.getComputedStyle(docElement, node, 'font-family');
        }
        var index = null;
        if ((name !== null && name !== '' && name !== undefined)
            && (fontName === null || fontName === undefined || (fontName.filter(function (value, pos) {
                var pattern = new RegExp(name, 'i');
                if ((value.replace(/"/g, '').replace(/ /g, '') === name.replace(/"/g, '').replace(/ /g, '')) ||
                    (value.search(pattern) > -1)) {
                    index = pos;
                }
            }) && (index !== null)))) {
            return (index !== null) ? fontName[index] : name.replace(/"/g, '');
        }
        else {
            return null;
        }
    };
    ToolbarStatus.isOrderedList = function (node) {
        if (node.nodeName.toLocaleLowerCase() === 'ol') {
            return true;
        }
        else {
            return false;
        }
    };
    ToolbarStatus.isUnorderedList = function (node) {
        if (node.nodeName.toLocaleLowerCase() === 'ul') {
            return true;
        }
        else {
            return false;
        }
    };
    ToolbarStatus.isAlignment = function (node) {
        var align = node.style && node.style.textAlign;
        if (align === 'left') {
            return 'justifyleft';
        }
        else if (align === 'center') {
            return 'justifycenter';
        }
        else if (align === 'right') {
            return 'justifyright';
        }
        else if (align === 'justify') {
            return 'justifyfull';
        }
        else {
            return null;
        }
    };
    ToolbarStatus.isFormats = function (node, formatNode) {
        if (((formatNode === undefined || formatNode === null)
            && BLOCK_TAGS.indexOf(node.nodeName.toLocaleLowerCase()) > -1)
            || (formatNode !== null && formatNode !== undefined
                && formatNode.indexOf(node.nodeName.toLocaleLowerCase()) > -1)) {
            return node.nodeName.toLocaleLowerCase();
        }
        else {
            return null;
        }
    };
    ToolbarStatus.getComputedStyle = function (docElement, node, prop) {
        return docElement.defaultView.getComputedStyle(node, null).getPropertyValue(prop);
    };
    ToolbarStatus.isNumberFormatList = function (node) {
        var list = node.style && node.style.listStyleType;
        if (list === 'lower-alpha') {
            return 'Lower Alpha';
        }
        else if (list === 'number') {
            return 'Number';
        }
        else if (list === 'upper-alpha') {
            return 'Upper Alpha';
        }
        else if (list === 'lower-roman') {
            return 'Lower Roman';
        }
        else if (list === 'upper-roman') {
            return 'Upper Roman';
        }
        else if (list === 'lower-greek') {
            return 'Lower Greek';
        }
        else if (list === 'none') {
            return 'None';
        }
        else {
            return null;
        }
    };
    ToolbarStatus.isBulletFormatList = function (node) {
        var list = node.style && node.style.listStyleType;
        if (list === 'circle') {
            return 'Circle';
        }
        else if (list === 'square') {
            return 'Square';
        }
        else if (list === 'none') {
            return 'None';
        }
        else if (list === 'disc') {
            return 'Disc';
        }
        else {
            return null;
        }
    };
    return ToolbarStatus;
}());

/**
 * HtmlToolbarStatus module for refresh the toolbar status
 */
var HtmlToolbarStatus = /** @class */ (function () {
    function HtmlToolbarStatus(parent) {
        this.parent = parent;
        this.toolbarStatus = this.prevToolbarStatus = getDefaultHtmlTbStatus();
        this.addEventListener();
    }
    HtmlToolbarStatus.prototype.addEventListener = function () {
        this.parent.observer.on(toolbarRefresh, this.onRefreshHandler, this);
        this.parent.observer.on(destroy, this.removeEventListener, this);
    };
    HtmlToolbarStatus.prototype.removeEventListener = function () {
        this.parent.observer.off(toolbarRefresh, this.onRefreshHandler);
        this.parent.observer.off(destroy, this.removeEventListener);
    };
    HtmlToolbarStatus.prototype.onRefreshHandler = function (args) {
        if (this.parent.readonly) {
            return;
        }
        var fontsize = [];
        var fontName = [];
        var formats = [];
        this.parent.fontSize.items.forEach(function (item) { fontsize.push(item.value); });
        this.parent.fontFamily.items.forEach(function (item) { fontName.push(item.value); });
        this.parent.format.items.forEach(function (item) {
            formats.push(item.value.toLocaleLowerCase());
        });
        this.toolbarStatus = ToolbarStatus.get(this.parent.getDocument(), this.parent.getEditPanel(), formats, fontsize, fontName, args.documentNode);
        var tbStatusString = JSON.stringify(this.toolbarStatus);
        this.parent.observer.notify(toolbarUpdated, this.toolbarStatus);
        if (JSON.stringify(this.prevToolbarStatus) !== tbStatusString) {
            this.parent.observer.notify(updateTbItemsStatus, { html: JSON.parse(tbStatusString), markdown: null });
            this.prevToolbarStatus = JSON.parse(tbStatusString);
        }
    };
    return HtmlToolbarStatus;
}());

/**
 * Constant values for Common
 */
/**
 * Keydown event trigger
 *
 * @hidden
 */
var KEY_DOWN = 'keydown';
/**
 * Undo and Redo action HTML plugin events
 *
 * @hidden
 */
var ACTION = 'action';
/**
 * Formats plugin events
 *
 * @hidden
 */
var FORMAT_TYPE = 'format-type';
/**
 * Keydown handler event trigger
 *
 * @hidden
 */
var KEY_DOWN_HANDLER = 'keydown-handler';
/**
 * List plugin events
 *
 * @hidden
 */
var LIST_TYPE = 'list-type';
/**
 * Keyup handler event trigger
 *
 * @hidden
 */
var KEY_UP_HANDLER = 'keyup-handler';
/**
 * Keyup event trigger
 *
 * @hidden
 */
var KEY_UP = 'keyup';
/**
 * Model changed plugin event trigger
 *
 * @hidden
 */
var MODEL_CHANGED_PLUGIN = 'model_changed_plugin';
/**
 * Model changed event trigger
 *
 * @hidden
 */
var MODEL_CHANGED = 'model_changed';
/**
 * PasteCleanup plugin for MSWord content
 *
 * @hidden
 */
var MS_WORD_CLEANUP_PLUGIN$1 = 'ms_word_cleanup_plugin';
/**
 * PasteCleanup for MSWord content
 *
 * @hidden
 */
var MS_WORD_CLEANUP$1 = 'ms_word_cleanup';
/**
 * ActionBegin event callback
 *
 * @hidden
 */
var ON_BEGIN$1 = 'onBegin';
/**
 * Callback for spacelist action
 *
 * @hidden
 */
var SPACE_ACTION = 'actionBegin';

/* eslint-disable */
/**
 * Export items model
 */

/* eslint-disable */
/**
 * Export default locale
 */

/**
 * Defines util methods used by Rich Text Editor.
 */
/**
 * @param {string} val - specifies the string value
 * @param {string} items - specifies the value
 * @returns {number} - returns the number value
 * @hidden
 */

/**
 * @param {Element} element - specifies the element
 * @param {string} className - specifies the string value
 * @returns {boolean} - returns the boolean value
 * @hidden
 */

/**
 * @param {IDropDownItemModel} items - specifies the item model
 * @param {string} value - specifies the string value
 * @param {string} type - specifies the string value
 * @param {string} returnType - specifies the return type
 * @returns {string} - returns the string value
 * @hidden
 */

/**
 * @returns {boolean} - returns the boolean value
 * @hidden
 */
function isIDevice$2() {
    var result = false;
    if (sf.base.Browser.isDevice && sf.base.Browser.isIos) {
        result = true;
    }
    return result;
}
/**
 * @param {string} value - specifies the value
 * @returns {string} - returns the string value
 * @hidden
 */

/**
 * @param {MouseEvent} e - specifies the mouse event
 * @param {HTMLElement} parentElement - specifies the parent element
 * @param {boolean} isIFrame - specifies the boolean value
 * @returns {number} - returns the number
 * @hidden
 */

/**
 * @param {string} item - specifies the string
 * @param {ServiceLocator} serviceLocator - specifies the service locator
 * @returns {string} - returns the string
 * @hidden
 */

/**
 * @param {ISetToolbarStatusArgs} e - specifies the e element
 * @param {boolean} isPopToolbar - specifies the boolean value
 * @param {IRichTextEditor} self - specifies the parent element
 * @returns {void}
 * @hidden
 */

/**
 * @param {string} items - specifies the string value
 * @returns {string[]} - returns the array value
 * @hidden
 */

/**
 * @param {string[]} items - specifies the array of string value
 * @param {IToolbarItemModel} toolbarItems - specifies the tool bar model
 * @returns {number} - returns the number
 * @hidden
 */

/**
 * @param {BaseToolbar} baseToolbar - specifies the base
 * @param {boolean} undoRedoStatus - specifies the boolean value
 * @returns {void}
 * @hidden
 */

/**
 * To dispatch the event manually
 *
 * @param {Element} element - specifies the element.
 * @param {string} type - specifies the string type.
 * @returns {void}
 * @hidden
 * @deprecated
 */

/**
 * To parse the HTML
 *
 * @param {string} value - specifies the string value
 * @returns {DocumentFragment} - returns the document
 * @hidden
 */

/**
 * @param {Document} docElement - specifies the document element
 * @param {Element} node - specifies the node
 * @returns {Node[]} - returns the node array
 * @hidden
 */
function getTextNodesUnder(docElement, node) {
    var nodes = [];
    for (node = node.firstChild; node; node = node.nextSibling) {
        if (node.nodeType === 3) {
            nodes.push(node);
        }
        else {
            nodes = nodes.concat(getTextNodesUnder(docElement, node));
        }
    }
    return nodes;
}
/**
 * @param {IToolsItemConfigs} obj - specifies the configuration
 * @returns {void}
 * @hidden
 */

/**
 * @param {string} value - specifies the string value
 * @param {IRichTextEditor} rteObj - specifies the rte object
 * @returns {string} - returns the string
 * @hidden
 */

/**
 * @param {string} value - specifies the value
 * @param {IRichTextEditor} rteObj - specifies the rich text editor instance.
 * @returns {string} - returns the string
 * @hidden
 */

/**
 * @param {IRichTextEditor} rteObj - specifies the rte object
 * @returns {string} - returns the value based on enter configuration.
 * @hidden
 */

/**
 * @param {string} value - specifies the value
 * @returns {boolean} - returns the boolean value
 * @hidden
 */

/**
 * @param {string} value - specifies the string value
 * @returns {string} - returns the string
 * @hidden
 */

/**
 * @param {string} value - specifies the string value
 * @param {IRichTextEditor} parent - specifies the rte
 * @returns {string} - returns the string value
 * @hidden
 */

/**
 * @param {string} dataUrl - specifies the string value
 * @returns {BaseToolbar} - returns the value
 * @hidden
 */
//Converting the base64 url to blob
function convertToBlob$1(dataUrl) {
    var arr = dataUrl.split(',');
    var mime = arr[0].match(/:(.*?);/)[1];
    var bstr = atob(arr[1]);
    var n = bstr.length;
    var u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new Blob([u8arr], { type: mime });
}
/**
 * @param {IRichTextEditor} self - specifies the rte
 * @param {string} localeItems - specifies the locale items
 * @param {IDropDownItemModel} item - specifies the dropdown item
 * @returns {string} - returns the value
 * @hidden
 */

/**
 * @param {IRichTextEditor} self - specifies the rte
 * @returns {void}
 * @hidden
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * Specifies Rich Text Editor internal events
 */
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var contentChanged$1 = 'content-changed';
/**
 * @hidden
 * @deprecated
 */
var initialEnd$1 = 'initial-end';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var destroy$1 = 'destroy';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var execCommandCallBack$1 = 'execCommandCallBack';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var insertCompleted$1 = 'insertCompleted';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var sourceCode$1 = 'sourceCode';
/**
 * @hidden
 * @deprecated
 */
var updateSource$1 = 'updateSource';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var sourceCodeMouseDown$1 = 'sourceCodeMouseDown';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */
var checkUndo$1 = 'checkUndoStack';
/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

/**
 * @hidden
 * @deprecated
 */

var __assign$2 = (undefined && undefined.__assign) || function () {
    __assign$2 = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign$2.apply(this, arguments);
};
/**
 * Formatter
 */
var Formatter = /** @class */ (function () {
    function Formatter() {
    }
    Formatter.prototype.process = function (self, args, event, value) {
        var _this = this;
        var selection = self.getDocument().getSelection();
        var range = (selection.rangeCount > 0) ? selection.getRangeAt(selection.rangeCount - 1) : null;
        var saveSelection;
        if (self.editorMode === 'HTML') {
            saveSelection = this.editorManager.nodeSelection.save(range, self.getDocument());
        }
        if (!sf.base.isNullOrUndefined(args)
            && args.item.command
            && args.item.command !== 'Table'
            && args.item.command !== 'Actions'
            && args.item.command !== 'Links'
            && args.item.command !== 'Images'
            && range
            && !(self.getEditPanel().contains(this.getAncestorNode(range.commonAncestorContainer))
                || self.getEditPanel() === range.commonAncestorContainer
                || self.getPanel() === range.commonAncestorContainer)) {
            return;
        }
        if (sf.base.isNullOrUndefined(args)) {
            var action_1 = event.action;
            if (action_1 !== 'tab' && action_1 !== 'enter' && action_1 !== 'space' && action_1 !== 'escape') {
                args = {};
                if (self.editorMode === 'Markdown' && action_1 === 'insert-table') {
                    value = {
                        'headingText': self.localeData.headingText,
                        'colText': self.localeData.colText
                    };
                }
                var items = {
                    originalEvent: event, cancel: false,
                    requestType: action_1 || (event.key + 'Key'),
                };
                sf.base.extend(args, args, items, true);
                delete args.item;
                args.originalEvent = __assign$2({}, args.originalEvent, { target: null });
                if (self.actionBeginEnabled) {
                    // @ts-ignore-start
                    self.dotNetRef.invokeMethodAsync(actionBeginEvent, args).then(function (actionBeginArgs) {
                        // @ts-ignore-end
                        if (args.cancel) {
                            if (action_1 === 'paste' || action_1 === 'cut' || action_1 === 'copy') {
                                event.preventDefault();
                            }
                        }
                    });
                }
            }
            var isTableModule = sf.base.isNullOrUndefined(self.tableModule) ? true : self.tableModule ?
                self.tableModule.ensureInsideTableList : false;
            if ((event.which === 9 && isTableModule) || event.which !== 9) {
                if (event.which === 13 && self.editorMode === 'HTML') {
                    value = {
                        'enterAction': self.enterKey
                    };
                }
                this.editorManager.observer.notify((event.type === 'keydown' ? KEY_DOWN : KEY_UP), {
                    event: event,
                    callBack: this.onSuccess.bind(this, self),
                    value: value,
                    enterAction: self.enterKey
                });
            }
        }
        else if (!sf.base.isNullOrUndefined(args) && args.item.command && args.item.subCommand && ((args.item.command !== args.item.subCommand
            && args.item.command !== 'Font')
            || ((args.item.subCommand === 'FontName' || args.item.subCommand === 'FontSize') && args.name === 'dropDownSelect')
            || ((args.item.subCommand === 'BackgroundColor' || args.item.subCommand === 'FontColor')
                && args.name === 'colorPickerChanged'))) {
            args.originalEvent = __assign$2({}, args.originalEvent, { target: null });
            sf.base.extend(args, args, { requestType: args.item.subCommand, cancel: false }, true);
            if (self.actionBeginEnabled) {
                // @ts-ignore-start
                self.dotNetRef.invokeMethodAsync(actionBeginEvent, args).then(function (actionBeginArgs) {
                    // @ts-ignore-end
                    if (!actionBeginArgs.cancel) {
                        _this.actionBeginCallBack(self, args, saveSelection, event, value);
                    }
                });
            }
            else {
                this.actionBeginCallBack(self, args, saveSelection, event, value);
            }
        }
        if (sf.base.isNullOrUndefined(event) || event && event.action !== 'copy') {
            this.enableUndo(self);
        }
    };
    Formatter.prototype.getAncestorNode = function (node) {
        node = node.nodeType === 3 ? node.parentNode : node;
        return node;
    };
    Formatter.prototype.onKeyHandler = function (self, e) {
        var _this = this;
        this.editorManager.observer.notify(KEY_UP, {
            event: e, callBack: function () {
                self.observer.notify(contentChanged$1, {});
                _this.enableUndo(self);
            }
        });
    };
    Formatter.prototype.onSuccess = function (self, events) {
        var _this = this;
        self.observer.notify(contentChanged$1, {});
        if (events && (sf.base.isNullOrUndefined(events.event) || events.event.action !== 'copy')) {
            if (events.requestType === 'Paste') {
                self.observer.notify(execCommandCallBack$1, events);
                this.enableUndo(self);
            }
            else {
                this.enableUndo(self);
                self.observer.notify(execCommandCallBack$1, events);
            }
        }
        events.event = __assign$2({}, events.event, { target: null });
        this.successArgs = __assign$2({}, events);
        delete events.elements;
        delete events.range;
        if (self.actionCompleteEnabled) {
            // @ts-ignore-start
            self.dotNetRef.invokeMethodAsync(actionCompleteEvent, events).then(function (callbackArgs) {
                // @ts-ignore-end
                _this.actionCompleteCallBack(self, callbackArgs);
            });
        }
        else {
            this.actionCompleteCallBack(self, events);
        }
    };
    Formatter.prototype.saveData = function (e) {
        this.editorManager.undoRedoManager.saveData(e);
    };
    Formatter.prototype.getUndoStatus = function () {
        return this.editorManager.undoRedoManager.getUndoStatus();
    };
    Formatter.prototype.getUndoRedoStack = function () {
        return this.editorManager.undoRedoManager.undoRedoStack;
    };
    Formatter.prototype.enableUndo = function (self) {
        if (self.undoRedoStatus) {
            var status_1 = this.getUndoStatus();
            if (self.inlineMode.enable && (!sf.base.Browser.isDevice || isIDevice$2())) {
                self.dotNetRef.invokeMethodAsync(updateUndoRedoStatus, status_1);
            }
            else {
                if (self.toolbarModule) {
                    self.dotNetRef.invokeMethodAsync(updateUndoRedoStatus, status_1);
                }
            }
        }
    };
    Formatter.prototype.actionBeginCallBack = function (self, args, selection, event, value) {
        if (this.getUndoRedoStack().length === 0 && args.item.command !== 'Links'
            && args.item.command !== 'Images') {
            this.saveData();
        }
        self.isBlur = false;
        self.getEditPanel().focus();
        if (self.editorMode === 'HTML') {
            selection.restore();
        }
        var command = args.item.subCommand.toLocaleLowerCase();
        if (command === 'paste' || command === 'cut' || command === 'copy') {
            self.clipboardAction(command, event);
        }
        else {
            this.editorManager.observer.notify(checkUndo$1, { subCommand: args.item.subCommand });
            this.editorManager.execCommand(args.item.command, args.item.subCommand, event, this.onSuccess.bind(this, self), args.item.value, args.item.subCommand === 'Pre' && args.name === 'dropDownSelect' ?
                { name: args.name } : value, ('#' + '' + ' iframe'), self.enterKey);
        }
    };
    Formatter.prototype.actionCompleteCallBack = function (self, args) {
        self.setPlaceHolder();
        if (args.requestType === 'Images' || args.requestType === 'Links' && args.editorMode === 'HTML') {
            var successArgs = this.successArgs;
            if (args.requestType === 'Links' && args.event && args.event.type === 'keydown' &&
                args.event.keyCode === 32) {
                return;
            }
            self.observer.notify(insertCompleted$1, {
                args: successArgs.event, type: args.requestType, isNotify: true, elements: successArgs.elements
            });
        }
        self.autoResize();
    };
    return Formatter;
}());

/**
 * Default Markdown formats config for adapter
 */
var markdownFormatTags = {
    'h6': '###### ',
    'h5': '##### ',
    'h4': '#### ',
    'h3': '### ',
    'h2': '## ',
    'h1': '# ',
    'blockquote': '> ',
    'pre': '```\n',
    'p': ''
};
/**
 * Default selection formats config for adapter
 */
var markdownSelectionTags = {
    'Bold': '**',
    'Italic': '*',
    'StrikeThrough': '~~',
    'InlineCode': '`',
    'SubScript': '<sub>',
    'SuperScript': '<sup>',
    'UpperCase': 'A-Z',
    'LowerCase': 'a-z'
};
/**
 * Default Markdown lists config for adapter
 */
var markdownListsTags = {
    'OL': '1. ',
    'UL': '- '
};
/**
 * Default html key config for adapter
 */
var htmlKeyConfig = {
    'toolbar-focus': 'alt+f10',
    'escape': '27',
    'insert-link': 'ctrl+k',
    'insert-image': 'ctrl+shift+i',
    'insert-table': 'ctrl+shift+e',
    'undo': 'ctrl+z',
    'redo': 'ctrl+y',
    'copy': 'ctrl+c',
    'cut': 'ctrl+x',
    'paste': 'ctrl+v',
    'bold': 'ctrl+b',
    'italic': 'ctrl+i',
    'underline': 'ctrl+u',
    'strikethrough': 'ctrl+shift+s',
    'uppercase': 'ctrl+shift+u',
    'lowercase': 'ctrl+shift+l',
    'superscript': 'ctrl+shift+=',
    'subscript': 'ctrl+=',
    'indents': 'ctrl+]',
    'outdents': 'ctrl+[',
    'html-source': 'ctrl+shift+h',
    'full-screen': 'ctrl+shift+f',
    'decrease-fontsize': 'ctrl+shift+<',
    'increase-fontsize': 'ctrl+shift+>',
    'justify-center': 'ctrl+e',
    'justify-full': 'ctrl+j',
    'justify-left': 'ctrl+l',
    'justify-right': 'ctrl+r',
    'clear-format': 'ctrl+shift+r',
    'ordered-list': 'ctrl+shift+o',
    'unordered-list': 'ctrl+alt+o',
    'space': '32',
    'enter': '13',
    'tab': 'tab',
    'delete': '46'
};
/**
 * Default  markdown key config for adapter
 */
var markdownKeyConfig = {
    'toolbar-focus': 'alt+f10',
    'escape': '27',
    'insert-link': 'ctrl+k',
    'insert-image': 'ctrl+shift+i',
    'insert-table': 'ctrl+shift+e',
    'undo': 'ctrl+z',
    'redo': 'ctrl+y',
    'copy': 'ctrl+c',
    'cut': 'ctrl+x',
    'paste': 'ctrl+v',
    'bold': 'ctrl+b',
    'italic': 'ctrl+i',
    'strikethrough': 'ctrl+shift+s',
    'uppercase': 'ctrl+shift+u',
    'lowercase': 'ctrl+shift+l',
    'superscript': 'ctrl+shift+=',
    'subscript': 'ctrl+=',
    'full-screen': 'ctrl+shift+f',
    'ordered-list': 'ctrl+shift+o',
    'unordered-list': 'ctrl+alt+o'
};
/**
 * PasteCleanup Grouping of similar functionality tags
 */
var pasteCleanupGroupingTags = {
    'b': ['strong'],
    'strong': ['b'],
    'i': ['emp', 'cite'],
    'emp': ['i', 'cite'],
    'cite': ['i', 'emp']
};
/**
 * PasteCleanup Grouping of similar functionality tags
 */

/**
 * Dom-Node Grouping of self closing tags
 *
 * @hidden
 */
var selfClosingTags = [
    'BR',
    'IMG'
];

var markerClassName = {
    startSelection: 'e-editor-select-start',
    endSelection: 'e-editor-select-end'
};
/**
 * DOMNode internal plugin
 *
 * @hidden
 * @deprecated
 */
var DOMNode = /** @class */ (function () {
    /**
     * Constructor for creating the DOMNode plugin
     *
     * @param {Element} parent - specifies the parent element
     * @param {Document} currentDocument - specifies the current document.
     * @hidden
     * @deprecated
     */
    function DOMNode(parent, currentDocument) {
        this.parent = parent;
        this.nodeSelection = new NodeSelection();
        this.currentDocument = currentDocument;
    }
    /**
     * contents method
     *
     * @param {Element} element - specifies the element.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.contents = function (element) {
        return (element && 'IFRAME' !== element.tagName ? Array.prototype.slice.call(element.childNodes || []) : []);
    };
    /**
     * isBlockNode method
     *
     * @param {Element} element - specifies the node element.
     * @returns {boolean} - sepcifies the boolean value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.isBlockNode = function (element) {
        return (!!element && (element.nodeType === Node.ELEMENT_NODE && BLOCK_TAGS.indexOf(element.tagName.toLowerCase()) >= 0));
    };
    /**
     * isLink method
     *
     * @param {Element} element - specifies the element
     * @returns {boolean} -  specifies the boolean value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.isLink = function (element) {
        return (!!element && (element.nodeType === Node.ELEMENT_NODE && 'a' === element.tagName.toLowerCase()));
    };
    /**
     * blockParentNode method
     *
     * @param {Element} element - specifies the element
     * @returns {Element} - returns the element value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.blockParentNode = function (element) {
        for (; element && element.parentNode !== this.parent && ((!element.parentNode ||
            !this.hasClass(element.parentNode, 'e-node-inner'))); null) {
            element = element.parentNode;
            if (this.isBlockNode(element)) {
                return element;
            }
        }
        return element;
    };
    /**
     * rawAttributes method
     *
     * @param {Element} element - specifies the element
     * @returns {string} - returns the string value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.rawAttributes = function (element) {
        var rawAttr = {};
        var attributes$$1 = element.attributes;
        if (attributes$$1.length > 0) {
            for (var d = 0; d < attributes$$1.length; d++) {
                var e = attributes$$1[d];
                rawAttr[e.nodeName] = e.value;
            }
        }
        return rawAttr;
    };
    /**
     * attributes method
     *
     * @param {Element} element - sepcifies the element.
     * @returns {string} - returns the string value.
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.attributes = function (element) {
        if (!element) {
            return '';
        }
        var attr = '';
        var rawAttr = this.rawAttributes(element);
        var orderRawAttr = Object.keys(rawAttr).sort();
        for (var e = 0; e < orderRawAttr.length; e++) {
            var attrKey = orderRawAttr[e];
            var attrValue = rawAttr[attrKey];
            /* eslint-disable */
            if (attrValue.indexOf("'") < 0 && attrValue.indexOf('"') >= 0) {
                attr += ' ' + attrKey + "='" + attrValue + "'";
            }
            else if (attrValue.indexOf('"') >= 0 && attrValue.indexOf("'") >= 0) {
                /* eslint-enable */
                attrValue = attrValue.replace(/"/g, '&quot;');
                attr += ' ' + attrKey + '="' + attrValue + '"';
            }
            else {
                attr += ' ' + attrKey + '="' + attrValue + '"';
            }
        }
        return attr;
    };
    /**
     * clearAttributes method
     *
     * @param {Element} element - specifies the element
     * @returns {void}
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.clearAttributes = function (element) {
        for (var attr = element.attributes, c = attr.length - 1; c >= 0; c--) {
            var key = attr[c];
            element.removeAttribute(key.nodeName);
        }
    };
    /**
     * openTagString method
     *
     * @param {Element} element - specifies the element.
     * @returns {string} - returns the string
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.openTagString = function (element) {
        return '<' + element.tagName.toLowerCase() + this.attributes(element) + '>';
    };
    /**
     * closeTagString method
     *
     * @param {Element} element - specifies the element
     * @returns {string} - returns the string value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.closeTagString = function (element) {
        return '</' + element.tagName.toLowerCase() + '>';
    };
    /**
     * createTagString method
     *
     * @param {string} tagName - specifies the tag name
     * @param {Element} relativeElement - specifies the relative element
     * @param {string} innerHTML - specifies the string value
     * @returns {string} - returns the string value.
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.createTagString = function (tagName, relativeElement, innerHTML) {
        return '<' + tagName.toLowerCase() + this.attributes(relativeElement) + '>' + innerHTML + '</' + tagName.toLowerCase() + '>';
    };
    /**
     * isList method
     *
     * @param {Element} element - specifes the element.
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.isList = function (element) {
        return !!element && ['UL', 'OL'].indexOf(element.tagName) >= 0;
    };
    /**
     * isElement method
     *
     * @param {Element} element - specifes the element.
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.isElement = function (element) {
        return element === this.parent;
    };
    /**
     * isEditable method
     *
     * @param {Element} element - specifes the element.
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.isEditable = function (element) {
        return ((!element.getAttribute || element.getAttribute('contenteditable') === 'true')
            && ['STYLE', 'SCRIPT'].indexOf(element.tagName) < 0);
    };
    /**
     * hasClass method
     *
     * @param {Element} element - specifes the element.
     * @param {string} className - specifies the class name value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.hasClass = function (element, className) {
        return element && element.classList && element.classList.contains(className);
    };
    /**
     * replaceWith method
     *
     * @param {Element} element - specifes the element.
     * @param {string} value - specifies the string value
     * @returns {void}
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.replaceWith = function (element, value) {
        var parentNode = element.parentNode;
        parentNode.insertBefore(this.parseHTMLFragment(value), element);
        sf.base.detach(element);
    };
    /**
     * parseHTMLFragment method
     *
     * @param {string} value - specifies the string value
     * @returns {Element} - returns the element
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.parseHTMLFragment = function (value) {
        /* eslint-disable */
        var temp = sf.base.createElement('template');
        temp.innerHTML = value;
        if (temp.content instanceof DocumentFragment) {
            return temp.content;
        }
        else {
            return document.createRange().createContextualFragment(value);
        }
        /* eslint-enable */
    };
    /**
     * wrap method
     *
     * @param {Element} element - specifies the element
     * @param {Element} wrapper - specifies the element.
     * @returns {Element} - returns the element
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.wrap = function (element, wrapper) {
        element.parentNode.insertBefore(wrapper, element);
        wrapper = element.previousSibling;
        wrapper.appendChild(element);
        return wrapper;
    };
    /**
     * insertAfter method
     *
     * @param {Element} newNode - specifies the new node element
     * @param {Element} referenceNode - specifies the referenece node
     * @returns {void}
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.insertAfter = function (newNode, referenceNode) {
        referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
    };
    /**
     * wrapInner method
     *
     * @param {Element} parent - specifies the parent element.
     * @param {Element} wrapper - specifies the wrapper element.
     * @returns {Element} - returns the element
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.wrapInner = function (parent, wrapper) {
        parent.appendChild(wrapper);
        wrapper = parent.querySelector('.e-rte-wrap-inner');
        wrapper.classList.remove('e-rte-wrap-inner');
        if (wrapper.classList.length === 0) {
            wrapper.removeAttribute('class');
        }
        while (parent.firstChild !== wrapper) {
            wrapper.appendChild(parent.firstChild);
        }
        return wrapper;
    };
    /**
     * unWrap method
     *
     * @param {Element} element - specifies the element.
     * @returns {Element} - returns the element.
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.unWrap = function (element) {
        var parent = element.parentNode;
        var unWrapNode = [];
        while (element.firstChild) {
            unWrapNode.push(element.firstChild);
            parent.insertBefore(element.firstChild, element);
        }
        unWrapNode = unWrapNode.length > 0 ? unWrapNode : [element.parentNode];
        parent.removeChild(element);
        return unWrapNode;
    };
    /**
     * getSelectedNode method
     *
     * @param {Element} element - specifies the element
     * @param {number} index - specifies the index value.
     * @returns {Element} - returns the element
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.getSelectedNode = function (element, index) {
        if (element.nodeType === Node.ELEMENT_NODE && element.childNodes.length > 0 &&
            element.childNodes[index - 1] && element.childNodes[index - 1].nodeType === Node.ELEMENT_NODE &&
            (element.childNodes[index - 1].classList.contains(markerClassName.startSelection) ||
                element.childNodes[index - 1].classList.contains(markerClassName.endSelection))) {
            element = element.childNodes[index - 1];
        }
        else if (element.nodeType === Node.ELEMENT_NODE && element.childNodes.length > 0 && element.childNodes[index]) {
            element = element.childNodes[index];
        }
        if (element.nodeType === Node.TEXT_NODE) {
            element = element.parentNode;
        }
        return element;
    };
    /**
     * nodeFinds method
     *
     * @param {Element} element - specifies the element.
     * @param {Element[]} elements - specifies the array of elements
     * @returns {Element[]} - returnts the array elements
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.nodeFinds = function (element, elements) {
        var existNodes = [];
        for (var i = 0; i < elements.length; i++) {
            if (element.contains(elements[i]) && element !== elements[i]) {
                existNodes.push(elements[i]);
            }
        }
        return existNodes;
    };
    /**
     * isEditorArea method
     *
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.isEditorArea = function () {
        var range = this.getRangePoint(0);
        var element;
        for (element = range.commonAncestorContainer; element && !this.isElement(element); null) {
            element = element.parentNode;
        }
        return !!this.isElement(element);
    };
    /**
     * getRangePoint method
     *
     * @param {number} point - specifies the number value.
     * @returns {Range} - returns the range.
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.getRangePoint = function (point) {
        var selection = this.getSelection();
        var ranges = [];
        if (selection && selection.getRangeAt && selection.rangeCount) {
            ranges = [];
            for (var f = 0; f < selection.rangeCount; f++) {
                ranges.push(selection.getRangeAt(f));
            }
        }
        else {
            ranges = [this.currentDocument.createRange()];
        }
        return 'undefined' !== typeof point ? ranges[point] : ranges;
    };
    DOMNode.prototype.getSelection = function () {
        return this.nodeSelection.get(this.currentDocument);
    };
    /**
     * getPreviousNode method
     *
     * @param {Element} element - specifies the element
     * @returns {Element} - returns the element
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.getPreviousNode = function (element) {
        element = element.previousElementSibling;
        for (; element && element.textContent === '\n'; null) {
            element = element.previousElementSibling;
        }
        return element;
    };
    /**
     * encode method
     *
     * @param {string} value - specifies the string value
     * @returns {string} - specifies the string value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.encode = function (value) {
        var divNode = document.createElement('div');
        divNode.innerText = value;
        // eslint-disable-next-line
        return divNode.innerHTML.replace(/<br\s*[\/]?>/gi, '\n');
    };
    /**
     * saveMarker method
     *
     * @param {NodeSelection} save - specifies the node selection,
     * @param {string} action - specifies the action  value.
     * @returns {NodeSelection} - returns the value
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.saveMarker = function (save, action) {
        var start = this.parent.querySelector('.' + markerClassName.startSelection);
        var end = this.parent.querySelector('.' + markerClassName.endSelection);
        var startTextNode;
        var endTextNode;
        if (start.textContent === '' && sf.base.isNullOrUndefined(end) && action !== 'tab') {
            if (start.childNodes.length === 1 && start.childNodes[0].nodeName === 'BR') {
                start.innerHTML = '&#65279;&#65279;<br>';
            }
            else {
                start.innerHTML = '&#65279;&#65279;';
            }
        }
        if (this.hasClass(start, markerClassName.startSelection) && start.classList.length > 1) {
            var replace = this.createTagString(DEFAULT_TAG, start, this.encode(start.textContent));
            this.replaceWith(start, replace);
            start = this.parent.querySelector('.' + markerClassName.startSelection);
            start.classList.remove(markerClassName.startSelection);
            startTextNode = start.childNodes[0];
        }
        else {
            startTextNode = this.unWrap(start)[0];
        }
        if (this.hasClass(end, markerClassName.endSelection) && end.classList.length > 1) {
            var replace = this.createTagString(DEFAULT_TAG, end, this.encode(end.textContent));
            this.replaceWith(end, replace);
            end = this.parent.querySelector('.' + markerClassName.endSelection);
            end.classList.remove(markerClassName.endSelection);
            endTextNode = end.childNodes[0];
        }
        else {
            endTextNode = end ? this.unWrap(end)[0] : startTextNode;
        }
        save.startContainer = save.getNodeArray(startTextNode, true);
        save.endContainer = save.getNodeArray(endTextNode, false);
        return save;
    };
    DOMNode.prototype.marker = function (className, textContent) {
        return '<span class="' + className + '">' + textContent + '</span>';
    };
    /**
     * setMarker method
     *
     * @param {NodeSelection} save - specifies the node selection.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.setMarker = function (save) {
        var range = save.range;
        var startChildNodes = range.startContainer.childNodes;
        var isTableStart = startChildNodes.length > 1 && startChildNodes[0].nodeName === 'TABLE';
        var start = ((isTableStart ? getLastTextNode(startChildNodes[range.startOffset + 1]) :
            startChildNodes[(range.startOffset > 0) ? (range.startOffset - 1) : range.startOffset]) || range.startContainer);
        var end = (range.endContainer.childNodes[(range.endOffset > 0) ? (range.endOffset - 1) : range.endOffset]
            || range.endContainer);
        if ((start.nodeType === Node.ELEMENT_NODE && end.nodeType === Node.ELEMENT_NODE) && (start.contains(end) || end.contains(start))) {
            var existNode = start.contains(end) ? start : end;
            var isElement = existNode.nodeType !== Node.TEXT_NODE;
            if (isElement) {
                var nodes = [];
                var textNodes = [];
                for (var node = existNode; existNode.contains(node); null) {
                    if (nodes.indexOf(node) < 0 && node.childNodes && node.childNodes.length) {
                        nodes.push(node);
                        node = node.childNodes[0];
                    }
                    else if (node.nextSibling) {
                        node = node.nextSibling;
                    }
                    else if (node.parentNode) {
                        node = node.parentNode;
                        nodes.push(node);
                    }
                    if (textNodes.indexOf(node) < 0 && (node.nodeType === Node.TEXT_NODE ||
                        (IGNORE_BLOCK_TAGS.indexOf(node.parentNode.tagName.toLocaleLowerCase()) >= 0
                            && (node.tagName === 'BR' || node.tagName === 'IMG')))) {
                        textNodes.push(node);
                    }
                }
                if (textNodes.length) {
                    start = start.contains(end) ? textNodes[0] : start;
                    end = textNodes[textNodes.length - 1];
                }
            }
        }
        if (start !== end) {
            if (start.nodeType !== Node.TEXT_NODE && ((start.tagName === 'BR' &&
                IGNORE_BLOCK_TAGS.indexOf(start.parentNode.tagName.toLocaleLowerCase()) >= 0) ||
                start.tagName === 'IMG')) {
                this.replaceWith(start, this.marker(markerClassName.startSelection, this.encode(start.textContent)));
                var markerStart = range.startContainer.querySelector('.' + markerClassName.startSelection);
                markerStart.appendChild(start);
            }
            else {
                this.replaceWith(start, this.marker(markerClassName.startSelection, this.encode(start.textContent)));
            }
            if (end.nodeType !== Node.TEXT_NODE && end.tagName === 'BR' &&
                IGNORE_BLOCK_TAGS.indexOf(end.parentNode.tagName.toLocaleLowerCase()) >= 0) {
                this.replaceWith(end, this.marker(markerClassName.endSelection, this.encode(end.textContent)));
                var markerEnd = range.endContainer.querySelector('.' + markerClassName.endSelection);
                markerEnd.appendChild(end);
            }
            else {
                this.ensureSelfClosingTag(end, markerClassName.endSelection, range);
            }
        }
        else {
            this.ensureSelfClosingTag(start, markerClassName.startSelection, range);
        }
    };
    /**
     * ensureSelfClosingTag method
     *
     * @param {Element} start - specifies the element.
     * @param {string} className - specifes the class name string value
     * @param {Range} range - specifies the range value
     * @returns {void}
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.ensureSelfClosingTag = function (start, className, range) {
        var isTable = false;
        if (start.nodeType === 3) {
            this.replaceWith(start, this.marker(className, this.encode(start.textContent)));
        }
        else if (start.tagName === 'BR') {
            this.replaceWith(start, this.marker(className, this.encode(start.textContent)));
            var markerStart = range.startContainer.querySelector('.' + className);
            markerStart.appendChild(start);
        }
        else {
            if (start.tagName === 'IMG') {
                var parNode = document.createElement('p');
                start.parentElement.insertBefore(parNode, start);
                parNode.appendChild(start);
                start = parNode.children[0];
            }
            if (start.tagName === 'TABLE') {
                isTable = true;
                if (start.textContent === '') {
                    var tdNode = start.querySelectorAll('td');
                    start = tdNode[tdNode.length - 1];
                    start = !sf.base.isNullOrUndefined(start.childNodes[0]) ? start.childNodes[0] : start;
                }
                else {
                    var lastNode = start.lastChild;
                    while (lastNode.nodeType !== 3 && lastNode.nodeName !== '#text' &&
                        lastNode.nodeName !== 'BR') {
                        lastNode = lastNode.lastChild;
                    }
                    start = lastNode;
                }
            }
            for (var i = 0; i < selfClosingTags.length; i++) {
                start = (start.tagName === selfClosingTags[i] && !isTable) ? start.parentNode : start;
            }
            if (start.nodeType === 3 && start.nodeName === '#text') {
                this.replaceWith(start, this.marker(className, this.encode(start.textContent)));
            }
            else if (start.nodeName === 'BR') {
                this.replaceWith(start, this.marker(markerClassName.endSelection, this.encode(start.textContent)));
                var markerEnd = range.endContainer.querySelector('.' + markerClassName.endSelection);
                markerEnd.appendChild(start);
            }
            else {
                var marker = this.marker(className, '');
                sf.base.append([this.parseHTMLFragment(marker)], start);
            }
        }
    };
    /**
     * createTempNode method
     *
     * @param {Element} element - specifies the element.
     * @returns {Element} - returns the element
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.createTempNode = function (element) {
        var textContent = element.textContent;
        if (element.tagName === 'BR') {
            var wrapper = '<' + DEFAULT_TAG + '></' + DEFAULT_TAG + '>';
            var node = element.parentNode;
            if (IGNORE_BLOCK_TAGS.indexOf(node.tagName.toLocaleLowerCase()) >= 0) {
                element = this.wrap(element, this.parseHTMLFragment(wrapper));
            }
        }
        else if (((element.nodeType !== Node.TEXT_NODE &&
            (element.classList.contains(markerClassName.startSelection) ||
                element.classList.contains(markerClassName.endSelection))) ||
            textContent.replace(/\n/g, '').replace(/(^ *)|( *$)/g, '').length > 0 ||
            textContent.length && textContent.indexOf('\n') < 0)) {
            var wrapper = '<' + DEFAULT_TAG + '></' + DEFAULT_TAG + '>';
            var target = element;
            element = this.wrap(element, this.parseHTMLFragment(wrapper));
            var ignoreBr = target.nodeType === Node.ELEMENT_NODE && target.firstChild && target.firstChild.nodeName === 'BR'
                && (target.classList.contains(markerClassName.startSelection) ||
                    target.classList.contains(markerClassName.endSelection));
            if (!ignoreBr && element.nextElementSibling && element.nextElementSibling.tagName === 'BR') {
                element.appendChild(element.nextElementSibling);
            }
        }
        return element;
    };
    /**
     * getImageTagInSelection method
     *
     * @returns {void}
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.getImageTagInSelection = function () {
        var selection = this.getSelection();
        if (this.isEditorArea() && selection.rangeCount) {
            return selection.focusNode.querySelectorAll('img');
        }
        return null;
    };
    /**
     * blockNodes method
     *
     * @returns {Node[]} - returns the node array values
     * @hidden
     * @deprecated
     */
    DOMNode.prototype.blockNodes = function () {
        var collectionNodes = [];
        var selection = this.getSelection();
        if (this.isEditorArea() && selection.rangeCount) {
            var ranges = this.getRangePoint();
            for (var j = 0; j < ranges.length; j++) {
                var parentNode = void 0;
                var range = ranges[j];
                var startNode = this.getSelectedNode(range.startContainer, range.startOffset);
                var endNode = this.getSelectedNode(range.endContainer, range.endOffset);
                if (this.isBlockNode(startNode) && collectionNodes.indexOf(startNode) < 0) {
                    collectionNodes.push(startNode);
                }
                parentNode = this.blockParentNode(startNode);
                if (parentNode && collectionNodes.indexOf(parentNode) < 0) {
                    if (IGNORE_BLOCK_TAGS.indexOf(parentNode.tagName.toLocaleLowerCase()) >= 0 && (startNode.tagName === 'BR' ||
                        startNode.nodeType === Node.TEXT_NODE ||
                        startNode.classList.contains(markerClassName.startSelection) ||
                        startNode.classList.contains(markerClassName.endSelection))) {
                        var tempNode = startNode.previousSibling &&
                            startNode.previousSibling.nodeType === Node.TEXT_NODE ?
                            startNode.previousSibling : startNode;
                        if (!startNode.nextSibling && !startNode.previousSibling && startNode.tagName === 'BR') {
                            collectionNodes.push(tempNode);
                        }
                        else {
                            collectionNodes.push(this.createTempNode(tempNode));
                        }
                    }
                    else {
                        collectionNodes.push(parentNode);
                    }
                }
                var nodes = [];
                for (var node = startNode; node !== endNode && node !== this.parent; null) {
                    if (nodes.indexOf(node) < 0 && node.childNodes && node.childNodes.length) {
                        nodes.push(node);
                        node = node.childNodes[0];
                    }
                    else if (node && node.nodeType !== 8 && (node.tagName === 'BR' || (node.nodeType === Node.TEXT_NODE &&
                        node.textContent.trim() !== '') || (node.nodeType !== Node.TEXT_NODE &&
                        (node.classList.contains(markerClassName.startSelection) ||
                            node.classList.contains(markerClassName.endSelection)))) &&
                        IGNORE_BLOCK_TAGS.indexOf(node.parentNode.tagName.toLocaleLowerCase()) >= 0) {
                        node = this.createTempNode(node);
                    }
                    else if (node.nextSibling && node.nextSibling.nodeType !== 8 &&
                        (node.nextSibling.tagName === 'BR' ||
                            node.nextSibling.nodeType === Node.TEXT_NODE ||
                            node.nextSibling.classList.contains(markerClassName.startSelection) ||
                            node.nextSibling.classList.contains(markerClassName.endSelection)) &&
                        IGNORE_BLOCK_TAGS.indexOf(node.nextSibling.parentNode.tagName.toLocaleLowerCase()) >= 0) {
                        node = this.createTempNode(node.nextSibling);
                    }
                    else if (node.nextSibling) {
                        node = node.nextSibling;
                    }
                    else if (node.parentNode) {
                        node = node.parentNode;
                        nodes.push(node);
                    }
                    if (collectionNodes.indexOf(node) < 0 && node.nodeType === Node.ELEMENT_NODE &&
                        IGNORE_BLOCK_TAGS.indexOf(node.parentNode.tagName.toLocaleLowerCase()) >= 0 &&
                        (node.classList.contains(markerClassName.startSelection) ||
                            node.classList.contains(markerClassName.endSelection))) {
                        collectionNodes.push(this.createTempNode(node));
                    }
                    if (this.isBlockNode(node) && this.ignoreTableTag(node) && nodes.indexOf(node) < 0 &&
                        collectionNodes.indexOf(node) < 0 && (node !== endNode || range.endOffset > 0)) {
                        collectionNodes.push(node);
                    }
                    if (node.nodeName === 'IMG' && node.parentElement.contentEditable === 'true') {
                        collectionNodes.push(node);
                    }
                }
                parentNode = this.blockParentNode(endNode);
                if (parentNode && this.ignoreTableTag(parentNode) && collectionNodes.indexOf(parentNode) < 0 &&
                    (!sf.base.isNullOrUndefined(parentNode.previousElementSibling) && parentNode.previousElementSibling.tagName !== 'IMG')) {
                    collectionNodes.push(parentNode);
                }
            }
        }
        for (var i = collectionNodes.length - 1; i > 0; i--) {
            var nodes = this.nodeFinds(collectionNodes[i], collectionNodes);
            if (nodes.length) {
                var listNodes = collectionNodes[i].querySelectorAll('ul, ol');
                if (collectionNodes[i].tagName === 'LI' && listNodes.length > 0) {
                    continue;
                }
                else {
                    collectionNodes.splice(i, 1);
                }
            }
        }
        return collectionNodes;
    };
    DOMNode.prototype.ignoreTableTag = function (element) {
        return !(TABLE_BLOCK_TAGS.indexOf(element.tagName.toLocaleLowerCase()) >= 0);
    };
    return DOMNode;
}());

/**
 * Lists internal component
 *
 * @hidden
 * @deprecated
 */
var Lists = /** @class */ (function () {
    /**
     * Constructor for creating the Lists plugin
     *
     * @param {EditorManager} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function Lists(parent) {
        this.parent = parent;
        this.domNode = this.parent.domNode;
        this.addEventListener();
    }
    Lists.prototype.addEventListener = function () {
        this.parent.observer.on(LIST_TYPE, this.applyListsHandler, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.keyDownHandler, this);
        this.parent.observer.on(SPACE_ACTION, this.spaceKeyAction, this);
    };
    Lists.prototype.testList = function (elem) {
        var olListRegex = [/^[\d]+[.]+$/,
            /^(?=[MDCLXVI])M*(C[MD]|D?C{0,3})(X[CL]|L?X{0,3})(I[XV]|V?I{0,3})[.]$/gi,
            /^[a-zA-Z][.]+$/];
        var elementStart = !sf.base.isNullOrUndefined(elem) ? elem.innerText.trim().split('.')[0] + '.' : null;
        if (!sf.base.isNullOrUndefined(elementStart)) {
            for (var i = 0; i < olListRegex.length; i++) {
                if (olListRegex[i].test(elementStart)) {
                    return true;
                }
            }
        }
        return false;
    };
    Lists.prototype.testCurrentList = function (range) {
        var olListStartRegex = [/^[1]+[.]+$/, /^[i]+[.]+$/, /^[a]+[.]+$/];
        if (!sf.base.isNullOrUndefined(range.startContainer.textContent.slice(0, range.startOffset))) {
            for (var i = 0; i < olListStartRegex.length; i++) {
                if (olListStartRegex[i].test(range.startContainer.textContent.replace(/\u200B/g, '').slice(0, range.startOffset).trim())) {
                    return true;
                }
            }
        }
        return false;
    };
    Lists.prototype.spaceList = function (e) {
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        this.saveSelection = this.parent.nodeSelection.save(range, this.parent.currentDocument);
        var startNode = this.parent.domNode.getSelectedNode(range.startContainer, range.startOffset);
        // eslint-disable-next-line
        var endNode = this.parent.domNode.getSelectedNode(range.endContainer, range.endOffset);
        var preElement = startNode.previousElementSibling;
        var nextElement = startNode.nextElementSibling;
        var preElemULStart = !sf.base.isNullOrUndefined(preElement) ?
            preElement.innerText.trim().substring(0, 1) : null;
        var nextElemULStart = !sf.base.isNullOrUndefined(nextElement) ?
            nextElement.innerText.trim().substring(0, 1) : null;
        var startElementOLTest = this.testCurrentList(range);
        var preElementOLTest = this.testList(preElement);
        var nextElementOLTest = this.testList(nextElement);
        if (!preElementOLTest && !nextElementOLTest && preElemULStart !== '*' && nextElemULStart !== '*') {
            if (startElementOLTest) {
                range.startContainer.textContent = range.startContainer.textContent.slice(range.startOffset, range.startContainer.textContent.length);
                this.applyListsHandler({ subCommand: 'OL', callBack: e.callBack });
                e.event.preventDefault();
            }
            else if (range.startContainer.textContent.replace(/\u200B/g, '').slice(0, range.startOffset).trim() === '*' ||
                range.startContainer.textContent.replace(/\u200B/g, '').slice(0, range.startOffset).trim() === '-') {
                range.startContainer.textContent = range.startContainer.textContent.slice(range.startOffset, range.startContainer.textContent.length);
                this.applyListsHandler({ subCommand: 'UL', callBack: e.callBack });
                e.event.preventDefault();
            }
        }
    };
    Lists.prototype.enterList = function (e) {
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        var startNode = range.startContainer.nodeName === 'LI' ? range.startContainer :
            range.startContainer.parentElement.closest('LI');
        var endNode = range.endContainer.nodeName === 'LI' ? range.endContainer :
            range.endContainer.parentElement.closest('LI');
        if (!sf.base.isNullOrUndefined(startNode) && !sf.base.isNullOrUndefined(endNode) && startNode === endNode && startNode.tagName === 'LI' && startNode.textContent.trim() === '') {
            if (startNode.textContent.charCodeAt(0) === 65279) {
                startNode.textContent = '';
            }
            var startNodeParent = startNode.parentElement;
            if (sf.base.isNullOrUndefined(startNodeParent.parentElement.closest('UL')) && sf.base.isNullOrUndefined(startNodeParent.parentElement.closest('OL'))) {
                if (!sf.base.isNullOrUndefined(startNode.nextElementSibling)) {
                    var nearBlockNode = this.parent.domNode.blockParentNode(startNode);
                    this.parent.nodeCutter.GetSpliceNode(range, nearBlockNode);
                }
                var insertTag = void 0;
                if (e.enterAction === 'DIV') {
                    insertTag = sf.base.createElement('div');
                    insertTag.innerHTML = '<br>';
                }
                else if (e.enterAction === 'P') {
                    insertTag = sf.base.createElement('p');
                    insertTag.innerHTML = '<br>';
                }
                else {
                    insertTag = sf.base.createElement('br');
                }
                this.parent.domNode.insertAfter(insertTag, startNodeParent);
                e.event.preventDefault();
                this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, insertTag, 0);
                if (startNodeParent.textContent === '') {
                    sf.base.detach(startNodeParent);
                }
                else {
                    sf.base.detach(startNode);
                }
            }
        }
    };
    // eslint-disable-next-line
    Lists.prototype.backspaceList = function (e) {
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        var startNode = this.parent.domNode.getSelectedNode(range.startContainer, range.startOffset);
        var endNode = this.parent.domNode.getSelectedNode(range.endContainer, range.endOffset);
        startNode = startNode.nodeName === 'BR' ? startNode.parentElement : startNode;
        endNode = endNode.nodeName === 'BR' ? endNode.parentElement : endNode;
        if (startNode === endNode && !sf.base.isNullOrUndefined(sf.base.closest(startNode, 'li')) &&
            ((startNode.textContent.trim() === '' && startNode.textContent.charCodeAt(0) === 65279) ||
                (startNode.textContent.length === 1 && startNode.textContent.charCodeAt(0) === 8203))) {
            startNode.textContent = '';
        }
        if (startNode === endNode && startNode.textContent === '') {
            if (startNode.parentElement.tagName === 'LI' && endNode.parentElement.tagName === 'LI') {
                sf.base.detach(startNode);
            }
            else if (startNode.closest('ul') || startNode.closest('ol')) {
                var parentList = !sf.base.isNullOrUndefined(startNode.closest('ul')) ? startNode.closest('ul') : startNode.closest('ol');
                if (parentList.firstElementChild === startNode && !sf.base.isNullOrUndefined(parentList.children[1]) &&
                    (parentList.children[1].tagName === 'OL' || parentList.children[1].tagName === 'UL')) {
                    if (parentList.tagName === parentList.children[1].tagName) {
                        while (parentList.children[1].lastChild) {
                            this.parent.domNode.insertAfter(parentList.children[1].lastChild, parentList.children[1]);
                        }
                        sf.base.detach(parentList.children[1]);
                    }
                    else {
                        parentList.parentElement.insertBefore(parentList.children[1], parentList);
                    }
                }
            }
        }
        else if (!sf.base.isNullOrUndefined(startNode.firstChild) && startNode.firstChild.nodeName === 'BR' &&
            (!sf.base.isNullOrUndefined(startNode.childNodes[1]) && (startNode.childNodes[1].nodeName === 'UL' ||
                startNode.childNodes[1].nodeName === 'OL'))) {
            var parentList = !sf.base.isNullOrUndefined(startNode.closest('ul')) ? startNode.closest('ul') : startNode.closest('ol');
            if (parentList.tagName === startNode.childNodes[1].nodeName) {
                while (startNode.childNodes[1].lastChild) {
                    this.parent.domNode.insertAfter(startNode.children[1].lastChild, startNode);
                }
                sf.base.detach(startNode.childNodes[1]);
            }
            else {
                parentList.parentElement.insertBefore(startNode.children[1], parentList);
            }
        }
    };
    Lists.prototype.keyDownHandler = function (e) {
        if (e.event.which === 13) {
            this.enterList(e);
        }
        if (e.event.which === 32) {
            this.spaceList(e);
        }
        if (e.event.which === 8) {
            this.backspaceList(e);
        }
        if (e.event.which === 46 && e.event.action === 'delete') {
            var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
            var commonAncestor = range.commonAncestorContainer;
            var startEle = range.startContainer;
            var endEle = range.endContainer;
            var startNode = startEle.nodeType === 3 ? startEle.parentElement : startEle;
            var endNode = endEle.nodeType === 3 ? endEle.parentElement : endEle;
            if ((commonAncestor.nodeName === 'UL' || commonAncestor.nodeName === 'OL') && startNode !== endNode
                && (!sf.base.isNullOrUndefined(sf.base.closest(startNode, 'ul')) || !sf.base.isNullOrUndefined(sf.base.closest(startNode, 'ol')))
                && (!sf.base.isNullOrUndefined(sf.base.closest(endNode, 'ul')) || !sf.base.isNullOrUndefined(sf.base.closest(endNode, 'ol')))
                && (commonAncestor.lastElementChild === sf.base.closest(endNode, 'li')) && !range.collapsed) {
                sf.base.detach(commonAncestor);
            }
        }
        if (e.event.which === 9) {
            var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
            if (!(e.event.action && e.event.action === 'indent')) {
                this.saveSelection = this.parent.nodeSelection.save(range, this.parent.currentDocument);
            }
            var blockNodes = void 0;
            var startOffset = range.startOffset;
            var endOffset = range.endOffset;
            var startNode = this.parent.domNode.getSelectedNode(range.startContainer, range.startOffset);
            var endNode = this.parent.domNode.getSelectedNode(range.endContainer, range.endOffset);
            if ((startNode === endNode && (startNode.nodeName === 'BR' || startNode.nodeName === '#text') &&
                IGNORE_BLOCK_TAGS.indexOf(startNode.parentNode.tagName.toLocaleLowerCase()) >= 0)) {
                return;
            }
            else {
                if (!(e.event.action && e.event.action === 'indent')) {
                    this.domNode.setMarker(this.saveSelection);
                }
                blockNodes = this.domNode.blockNodes();
            }
            var nodes = [];
            var isNested = true;
            for (var i = 0; i < blockNodes.length; i++) {
                if (blockNodes[i].parentNode.tagName === 'LI') {
                    nodes.push(blockNodes[i].parentNode);
                }
                else if (blockNodes[i].tagName === 'LI' && blockNodes[i].childNodes[0].tagName !== 'P' &&
                    (blockNodes[i].childNodes[0].tagName !== 'OL' &&
                        blockNodes[i].childNodes[0].tagName !== 'UL')) {
                    nodes.push(blockNodes[i]);
                }
            }
            if (nodes.length > 1 || nodes.length && ((startOffset === 0 && endOffset === 0) || e.ignoreDefault)) {
                e.event.preventDefault();
                e.event.stopPropagation();
                this.currentAction = this.getAction(nodes[0]);
                if (e.event.shiftKey) {
                    this.revertList(nodes);
                    this.revertClean();
                }
                else {
                    isNested = this.nestedList(nodes);
                }
                if (isNested) {
                    this.cleanNode();
                    this.parent.editableElement.focus();
                }
                if (!(e.event.action && e.event.action === 'indent')) {
                    this.saveSelection = this.domNode.saveMarker(this.saveSelection);
                    this.saveSelection.restore();
                    if (e.callBack) {
                        e.callBack({
                            requestType: this.currentAction,
                            editorMode: 'HTML',
                            range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                            elements: this.parent.domNode.blockNodes(),
                            event: e.event
                        });
                    }
                }
            }
            else {
                if (!(e.event.action && e.event.action === 'indent')) {
                    if (e.event && e.event.shiftKey && e.event.key === 'Tab') {
                        e.event.action = 'tab';
                    }
                    this.saveSelection = this.domNode.saveMarker(this.saveSelection, e.event.action);
                    this.saveSelection.restore();
                }
            }
        }
        else {
            switch (e.event.action) {
                case 'ordered-list':
                    this.applyListsHandler({ subCommand: 'OL', callBack: e.callBack });
                    e.event.preventDefault();
                    break;
                case 'unordered-list':
                    this.applyListsHandler({ subCommand: 'UL', callBack: e.callBack });
                    e.event.preventDefault();
                    break;
            }
        }
    };
    Lists.prototype.spaceKeyAction = function (e) {
        if (e.event.which === 32) {
            this.spaceList(e);
        }
    };
    Lists.prototype.getAction = function (element) {
        var parentNode = element.parentNode;
        return (parentNode.nodeName === 'OL' ? 'OL' : 'UL');
    };
    Lists.prototype.revertClean = function () {
        var collectionNodes = this.parent.editableElement.querySelectorAll('ul, ol');
        for (var i = 0; i < collectionNodes.length; i++) {
            var listNodes = collectionNodes[i].querySelectorAll('ul, ol');
            if (listNodes.length > 0) {
                for (var j = 0; j < listNodes.length; j++) {
                    var prevSibling = listNodes[j].previousSibling;
                    if (prevSibling && prevSibling.tagName === 'LI') {
                        prevSibling.appendChild(listNodes[j]);
                    }
                }
            }
        }
    };
    Lists.prototype.noPreviousElement = function (elements) {
        var firstNode;
        var firstNodeOL;
        var siblingListOL = elements.querySelectorAll('ol, ul');
        var siblingListLI = elements
            .querySelectorAll('li');
        var siblingListLIFirst = this.domNode.contents(siblingListLI[0])[0];
        if (siblingListLI.length > 0 && (siblingListLIFirst.nodeName === 'OL' || siblingListLIFirst.nodeName === 'UL')) {
            firstNode = siblingListLI[0];
        }
        else {
            firstNodeOL = siblingListOL[0];
        }
        if (firstNode) {
            for (var h = this.domNode.contents(elements)[0]; h && !this.domNode.isList(h); null) {
                var nextSibling = h.nextSibling;
                sf.base.prepend([h], firstNode);
                sf.base.setStyleAttribute(elements, { 'list-style-type': 'none' });
                sf.base.setStyleAttribute(firstNode, { 'list-style-type': '' });
                h = nextSibling;
            }
        }
        else if (firstNodeOL) {
            var nestedElement = sf.base.createElement('li');
            sf.base.prepend([nestedElement], firstNodeOL);
            for (var h = this.domNode.contents(elements)[0]; h && !this.domNode.isList(h); null) {
                var nextSibling = h.nextSibling;
                nestedElement.appendChild(h);
                h = nextSibling;
            }
            sf.base.prepend([firstNodeOL], elements.parentNode);
            sf.base.detach(elements);
            var nestedElementLI = sf.base.createElement('li', { styles: 'list-style-type: none;' });
            sf.base.prepend([nestedElementLI], firstNodeOL.parentNode);
            sf.base.append([firstNodeOL], nestedElementLI);
        }
        else {
            var nestedElementLI = sf.base.createElement('li', { styles: 'list-style-type: none;' });
            sf.base.prepend([nestedElementLI], elements.parentNode);
            var nestedElement = sf.base.createElement(elements.parentNode.tagName);
            sf.base.prepend([nestedElement], nestedElementLI);
            sf.base.append([elements], nestedElement);
        }
    };
    Lists.prototype.nestedList = function (elements) {
        var isNested = false;
        for (var i = 0; i < elements.length; i++) {
            var prevSibling = this.domNode.getPreviousNode(elements[i]);
            if (prevSibling) {
                isNested = true;
                var firstNode = void 0;
                var firstNodeLI = void 0;
                var siblingListOL = elements[i].querySelectorAll('ol, ul');
                var siblingListLI = elements[i]
                    .querySelectorAll('li');
                var siblingListLIFirst = this.domNode.contents(siblingListLI[0])[0];
                if (siblingListLI.length > 0 && (siblingListLIFirst.nodeName === 'OL' || siblingListLIFirst.nodeName === 'UL')) {
                    firstNodeLI = siblingListLI[0];
                }
                else {
                    firstNode = siblingListOL[0];
                }
                if (firstNode) {
                    var nestedElement = sf.base.createElement('li');
                    sf.base.prepend([nestedElement], firstNode);
                    for (var h = this.domNode.contents(elements[i])[0]; h && !this.domNode.isList(h); null) {
                        var nextSibling = h.nextSibling;
                        nestedElement.appendChild(h);
                        h = nextSibling;
                    }
                    sf.base.append([firstNode], prevSibling);
                    sf.base.detach(elements[i]);
                }
                else if (firstNodeLI) {
                    if (prevSibling.tagName === 'LI') {
                        for (var h = this.domNode.contents(elements[i])[0]; h && !this.domNode.isList(h); null) {
                            var nextSibling = h.nextSibling;
                            sf.base.prepend([h], firstNodeLI);
                            sf.base.setStyleAttribute(elements[i], { 'list-style-type': 'none' });
                            sf.base.setStyleAttribute(firstNodeLI, { 'list-style-type': '' });
                            h = nextSibling;
                        }
                        sf.base.append([firstNodeLI.parentNode], prevSibling);
                        sf.base.detach(elements[i]);
                    }
                }
                else {
                    if (prevSibling.tagName === 'LI') {
                        var nestedElement = sf.base.createElement(elements[i].parentNode.tagName);
                        sf.base.append([nestedElement], prevSibling);
                        sf.base.append([elements[i]], nestedElement);
                    }
                }
            }
            else {
                var element = elements[i];
                isNested = true;
                this.noPreviousElement(element);
            }
        }
        return isNested;
    };
    Lists.prototype.applyListsHandler = function (e) {
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        this.saveSelection = this.parent.nodeSelection.save(range, this.parent.currentDocument);
        this.currentAction = e.subCommand;
        this.currentAction = e.subCommand = this.currentAction === 'NumberFormatList' ? 'OL' : this.currentAction === 'BulletFormatList' ? 'UL' : this.currentAction;
        this.domNode.setMarker(this.saveSelection);
        var listsNodes = this.domNode.blockNodes();
        if (e.enterAction === 'BR') {
            this.setSelectionBRConfig();
            var allSelectedNode = this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument);
            var selectedNodes = this.parent.nodeSelection.getSelectionNodes(allSelectedNode);
            var currentFormatNodes = [];
            if (selectedNodes.length === 0) {
                selectedNodes.push(listsNodes[0]);
            }
            for (var i = 0; i < selectedNodes.length; i++) {
                var currentNode = selectedNodes[i];
                var previousCurrentNode = void 0;
                while (!this.parent.domNode.isBlockNode(currentNode) && currentNode !== this.parent.editableElement) {
                    previousCurrentNode = currentNode;
                    currentNode = currentNode.parentElement;
                }
                if (this.parent.domNode.isBlockNode(currentNode) && currentNode === this.parent.editableElement) {
                    currentFormatNodes.push(previousCurrentNode);
                }
            }
            for (var i = 0; i < currentFormatNodes.length; i++) {
                if (!this.parent.domNode.isBlockNode(currentFormatNodes[i])) {
                    var currentNode = currentFormatNodes[i];
                    var previousNode = currentNode;
                    while (currentNode === this.parent.editableElement) {
                        previousNode = currentNode;
                        currentNode = currentNode.parentElement;
                    }
                    var tempElem = void 0;
                    if (this.parent.domNode.isBlockNode(previousNode.parentElement) &&
                        previousNode.parentElement === this.parent.editableElement) {
                        tempElem = sf.base.createElement('p');
                        previousNode.parentElement.insertBefore(tempElem, previousNode);
                        tempElem.appendChild(previousNode);
                    }
                    else {
                        tempElem = previousNode;
                    }
                    var preNode = tempElem.previousSibling;
                    while (!sf.base.isNullOrUndefined(preNode) && preNode.nodeName !== 'BR' &&
                        !this.parent.domNode.isBlockNode(preNode)) {
                        tempElem.firstChild.parentElement.insertBefore(preNode, tempElem.firstChild);
                        preNode = tempElem.previousSibling;
                    }
                    if (!sf.base.isNullOrUndefined(preNode) && preNode.nodeName === 'BR') {
                        sf.base.detach(preNode);
                    }
                    var postNode = tempElem.nextSibling;
                    while (!sf.base.isNullOrUndefined(postNode) && postNode.nodeName !== 'BR' &&
                        !this.parent.domNode.isBlockNode(postNode)) {
                        tempElem.appendChild(postNode);
                        postNode = tempElem.nextSibling;
                    }
                    if (!sf.base.isNullOrUndefined(postNode) && postNode.nodeName === 'BR') {
                        sf.base.detach(postNode);
                    }
                }
            }
            this.setSelectionBRConfig();
            listsNodes = this.parent.domNode.blockNodes();
        }
        for (var i = 0; i < listsNodes.length; i++) {
            if (listsNodes[i].tagName === 'TABLE' && !range.collapsed) {
                listsNodes.splice(i, 1);
            }
            if (listsNodes.length > 0 && listsNodes[i].tagName !== 'LI'
                && 'LI' === listsNodes[i].parentNode.tagName) {
                listsNodes[i] = listsNodes[i].parentNode;
            }
        }
        this.applyLists(listsNodes, this.currentAction, e.selector, e.item);
        if (e.callBack) {
            e.callBack({
                requestType: this.currentAction,
                event: e.event,
                editorMode: 'HTML',
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.domNode.blockNodes()
            });
        }
    };
    Lists.prototype.setSelectionBRConfig = function () {
        var startElem = this.parent.editableElement.querySelector('.' + markerClassName.startSelection);
        var endElem = this.parent.editableElement.querySelector('.' + markerClassName.endSelection);
        if (sf.base.isNullOrUndefined(endElem)) {
            this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, startElem, 0);
        }
        else {
            this.parent.nodeSelection.setSelectionText(this.parent.currentDocument, startElem, endElem, 0, 0);
        }
    };
    Lists.prototype.applyLists = function (elements, type, selector, item) {
        if (this.isRevert(elements, type) && sf.base.isNullOrUndefined(item)) {
            this.revertList(elements);
            this.removeEmptyListElements();
        }
        else {
            this.checkLists(elements, type, item);
            for (var i = 0; i < elements.length; i++) {
                if (!sf.base.isNullOrUndefined(item) && !sf.base.isNullOrUndefined(item.listStyle)) {
                    if (item.listStyle === 'listImage') {
                        sf.base.setStyleAttribute(elements[i], { 'list-style-image': item.listImage });
                    }
                    else {
                        sf.base.setStyleAttribute(elements[i], { 'list-style-image': 'none' });
                        sf.base.setStyleAttribute(elements[i], { 'list-style-type': item.listStyle.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase() });
                    }
                }
                if (elements[i].getAttribute('contenteditable') === 'true'
                    && elements[i].childNodes.length === 1 && elements[i].childNodes[0].nodeName === 'TABLE') {
                    var listEle = document.createElement(type);
                    listEle.innerHTML = '<li><br/></li>';
                    elements[i].appendChild(listEle);
                }
                else if ('LI' !== elements[i].tagName && sf.base.isNullOrUndefined(item)) {
                    var elemAtt = elements[i].tagName === 'IMG' ? '' : this.domNode.attributes(elements[i]);
                    var openTag = '<' + type + '>';
                    var closeTag = '</' + type + '>';
                    var newTag = 'li' + elemAtt;
                    var replaceHTML = (elements[i].tagName.toLowerCase() === DEFAULT_TAG ? elements[i].innerHTML :
                        elements[i].outerHTML);
                    var innerHTML = this.domNode.createTagString(newTag, null, replaceHTML);
                    var collectionString = openTag + innerHTML + closeTag;
                    this.domNode.replaceWith(elements[i], collectionString);
                }
                else if (!sf.base.isNullOrUndefined(item) && 'LI' !== elements[i].tagName) {
                    // eslint-disable-next-line
                    var elemAtt = elements[i].tagName === 'IMG' ? '' : this.domNode.attributes(elements[i]);
                    var openTag = '<' + type + elemAtt + '>';
                    var closeTag = '</' + type + '>';
                    var newTag = 'li';
                    var replaceHTML = (elements[i].tagName.toLowerCase() === DEFAULT_TAG ? elements[i].innerHTML :
                        elements[i].outerHTML);
                    var innerHTML = this.domNode.createTagString(newTag, null, replaceHTML);
                    var collectionString = openTag + innerHTML + closeTag;
                    this.domNode.replaceWith(elements[i], collectionString);
                }
            }
        }
        this.cleanNode();
        this.parent.editableElement.focus();
        if (isIDevice$1()) {
            setEditFrameFocus(this.parent.editableElement, selector);
        }
        this.saveSelection = this.domNode.saveMarker(this.saveSelection);
        this.saveSelection.restore();
    };
    Lists.prototype.removeEmptyListElements = function () {
        var listElem = this.parent.editableElement.querySelectorAll('ol, ul');
        for (var i = 0; i < listElem.length; i++) {
            if (listElem[i].textContent.trim() === '') {
                sf.base.detach(listElem[i]);
            }
        }
    };
    Lists.prototype.isRevert = function (nodes, tagName) {
        var isRevert = true;
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].tagName !== 'LI') {
                return false;
            }
            if (nodes[i].parentNode.tagName !== tagName) {
                isRevert = false;
            }
        }
        return isRevert;
    };
    Lists.prototype.checkLists = function (nodes, tagName, item) {
        var nodesTemp = [];
        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i].parentNode;
            if (!sf.base.isNullOrUndefined(item) && 'LI' === nodes[i].tagName && !sf.base.isNullOrUndefined(item.listStyle)) {
                if (item.listStyle === 'listImage') {
                    sf.base.setStyleAttribute(node, { 'list-style-image': item.listImage });
                }
                else {
                    sf.base.setStyleAttribute(node, { 'list-style-image': 'none' });
                    sf.base.setStyleAttribute(node, { 'list-style-type': item.listStyle.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase() });
                }
            }
            if ((nodes[i].tagName === 'LI' && node.tagName !== tagName && nodesTemp.indexOf(node) < 0) ||
                (nodes[i].tagName === 'LI' && node.tagName === tagName && nodesTemp.indexOf(node) < 0 && item !== null)) {
                nodesTemp.push(node);
            }
        }
        for (var j = nodesTemp.length - 1; j >= 0; j--) {
            var h = nodesTemp[j];
            var replace = '<' + tagName.toLowerCase() + ' '
                + this.domNode.attributes(h) + '>' + h.innerHTML + '</' + tagName.toLowerCase() + '>';
            this.domNode.replaceWith(nodesTemp[j], replace);
        }
    };
    Lists.prototype.cleanNode = function () {
        var liParents = this.parent.editableElement.querySelectorAll('ol + ol, ul + ul');
        for (var c = 0; c < liParents.length; c++) {
            var node = liParents[c];
            if (this.domNode.isList(node.previousElementSibling) &&
                this.domNode.openTagString(node) === this.domNode.openTagString(node.previousElementSibling)) {
                var contentNodes = this.domNode.contents(node);
                for (var f = 0; f < contentNodes.length; f++) {
                    node.previousElementSibling.appendChild(contentNodes[f]);
                }
                node.parentNode.removeChild(node);
            }
        }
    };
    Lists.prototype.findUnSelected = function (temp, elements) {
        temp = temp.slice().reverse();
        if (temp.length > 0) {
            var rightIndent = [];
            var indentElements = [];
            var lastElement = elements[elements.length - 1];
            var lastElementChild = [];
            var childElements = [];
            lastElementChild = (lastElement.childNodes);
            for (var z = 0; z < lastElementChild.length; z++) {
                if (lastElementChild[z].tagName === 'OL' || lastElementChild[z].tagName === 'UL') {
                    var childLI = lastElementChild[z]
                        .querySelectorAll('li');
                    if (childLI.length > 0) {
                        for (var y = 0; y < childLI.length; y++) {
                            childElements.push(childLI[y]);
                        }
                    }
                }
            }
            for (var i = 0; i < childElements.length; i++) {
                var count = 0;
                for (var j = 0; j < temp.length; j++) {
                    if (!childElements[i].contains((temp[j]))) {
                        count = count + 1;
                    }
                }
                if (count === temp.length) {
                    indentElements.push(childElements[i]);
                }
            }
            if (indentElements.length > 0) {
                for (var x = 0; x < indentElements.length; x++) {
                    if (this.domNode.contents(indentElements[x])[0].nodeName !== 'OL' &&
                        this.domNode.contents(indentElements[x])[0].nodeName !== 'UL') {
                        rightIndent.push(indentElements[x]);
                    }
                }
            }
            if (rightIndent.length > 0) {
                this.nestedList(rightIndent);
            }
        }
    };
    Lists.prototype.revertList = function (elements) {
        var temp = [];
        for (var i = elements.length - 1; i >= 0; i--) {
            for (var j = i - 1; j >= 0; j--) {
                if (elements[j].contains((elements[i])) || elements[j] === elements[i]) {
                    temp.push(elements[i]);
                    elements.splice(i, 1);
                    break;
                }
            }
        }
        this.findUnSelected(temp, elements);
        var viewNode = [];
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            if (this.domNode.contents(element)[0].nodeType === 3 && this.domNode.contents(element)[0].textContent.trim().length === 0) {
                sf.base.detach(this.domNode.contents(element)[0]);
            }
            var parentNode = elements[i].parentNode;
            var className = element.getAttribute('class');
            if (temp.length === 0) {
                var siblingList = elements[i].querySelectorAll('ul, ol');
                var firstNode = siblingList[0];
                if (firstNode) {
                    var child = firstNode
                        .querySelectorAll('li');
                    if (child) {
                        var nestedElement = sf.base.createElement(firstNode.tagName);
                        sf.base.append([nestedElement], firstNode.parentNode);
                        var nestedElementLI = sf.base.createElement('li', { styles: 'list-style-type: none;' });
                        sf.base.append([nestedElementLI], nestedElement);
                        sf.base.append([firstNode], nestedElementLI);
                    }
                }
            }
            if (element.parentNode.insertBefore(this.closeTag(parentNode.tagName), element), 'LI' === parentNode.parentNode.tagName) {
                element.parentNode.insertBefore(this.closeTag('LI'), element);
            }
            else {
                if (DEFAULT_TAG && 0 === element.querySelectorAll(BLOCK_TAGS.join(', ')).length) {
                    var wrapperclass = sf.base.isNullOrUndefined(className) ? ' class="e-rte-wrap-inner"' :
                        ' class="' + className + ' e-rte-wrap-inner"';
                    var wrapper = '<' + DEFAULT_TAG + wrapperclass +
                        this.domNode.attributes(parentNode) + '></' + DEFAULT_TAG + '>';
                    this.domNode.wrapInner(element, this.domNode.parseHTMLFragment(wrapper));
                }
                else if (this.domNode.contents(element)[0].nodeType === 3) {
                    var replace = this.domNode.createTagString(DEFAULT_TAG, parentNode, this.parent.domNode.encode(this.domNode.contents(element)[0].textContent));
                    this.domNode.replaceWith(this.domNode.contents(element)[0], replace);
                }
                else if (this.domNode.contents(element)[0].classList.contains(markerClassName.startSelection) ||
                    this.domNode.contents(element)[0].classList.contains(markerClassName.endSelection)) {
                    var replace = this.domNode.createTagString(DEFAULT_TAG, parentNode, this.domNode.contents(element)[0].outerHTML);
                    this.domNode.replaceWith(this.domNode.contents(element)[0], replace);
                }
                else {
                    var childNode = element.firstChild;
                    className = childNode.getAttribute('class');
                    sf.base.attributes(childNode, this.domNode.rawAttributes(parentNode));
                    if (className && childNode.getAttribute('class')) {
                        sf.base.attributes(childNode, { 'class': className + ' ' + childNode.getAttribute('class') });
                    }
                }
                sf.base.append([this.openTag('LI')], element);
                sf.base.prepend([this.closeTag('LI')], element);
            }
            this.domNode.insertAfter(this.openTag(parentNode.tagName), element);
            if (parentNode.parentNode.tagName === 'LI') {
                parentNode = parentNode.parentNode.parentNode;
            }
            if (viewNode.indexOf(parentNode) < 0) {
                viewNode.push(parentNode);
            }
        }
        for (var i = 0; i < viewNode.length; i++) {
            var node = viewNode[i];
            var nodeInnerHtml = node.innerHTML;
            var closeTag = /<span class="e-rte-list-close-([a-z]*)"><\/span>/g;
            var openTag = /<span class="e-rte-list-open-([a-z]*)"><\/span>/g;
            nodeInnerHtml = nodeInnerHtml.replace(closeTag, '</$1>');
            nodeInnerHtml = nodeInnerHtml.replace(openTag, '<$1 ' + this.domNode.attributes(node) + '>');
            this.domNode.replaceWith(node, this.domNode.openTagString(node) + nodeInnerHtml.trim() + this.domNode.closeTagString(node));
        }
        var emptyUl = this.parent.editableElement.querySelectorAll('ul:empty, ol:empty');
        for (var i = 0; i < emptyUl.length; i++) {
            sf.base.detach(emptyUl[i]);
        }
        var emptyLi = this.parent.editableElement.querySelectorAll('li:empty');
        for (var i = 0; i < emptyLi.length; i++) {
            sf.base.detach(emptyLi[i]);
        }
    };
    Lists.prototype.openTag = function (type) {
        return this.domNode.parseHTMLFragment('<span class="e-rte-list-open-' + type.toLowerCase() + '"></span>');
    };
    Lists.prototype.closeTag = function (type) {
        return this.domNode.parseHTMLFragment('<span class="e-rte-list-close-' + type.toLowerCase() + '"></span>');
    };
    return Lists;
}());

/**
 * Node appending methods.
 *
 * @hidden
 */
var InsertMethods = /** @class */ (function () {
    function InsertMethods() {
    }
    /**
     * WrapBefore method
     *
     * @param {Text} textNode - specifies the text node
     * @param {HTMLElement} parentNode - specifies the parent node
     * @param {boolean} isAfter - specifies the boolean value
     * @returns {Text} - returns the text value
     * @hidden
     * @deprecated
     */
    InsertMethods.WrapBefore = function (textNode, parentNode, isAfter) {
        parentNode.innerText = textNode.textContent;
        //eslint-disable-next-line
        (!isAfter) ? this.AppendBefore(parentNode, textNode) : this.AppendBefore(parentNode, textNode, true);
        if (textNode.parentNode) {
            textNode.parentNode.removeChild(textNode);
        }
        return parentNode.childNodes[0];
    };
    /**
     * Wrap method
     *
     * @param {HTMLElement} childNode - specifies the child node
     * @param {HTMLElement} parentNode - specifies the parent node.
     * @returns {HTMLElement} - returns the element
     * @hidden
     * @deprecated
     */
    InsertMethods.Wrap = function (childNode, parentNode) {
        this.AppendBefore(parentNode, childNode);
        parentNode.appendChild(childNode);
        return childNode;
    };
    /**
     * unwrap method
     *
     * @param {Node} node - specifies the node element.
     * @returns {Node[]} - returns the array of value
     * @hidden
     * @deprecated
     */
    InsertMethods.unwrap = function (node) {
        var parent = node.parentNode;
        var child = [];
        for (; node.firstChild; null) {
            child.push(parent.insertBefore(node.firstChild, node));
        }
        parent.removeChild(node);
        return child;
    };
    /**
     * AppendBefore method
     *
     * @param {HTMLElement} textNode - specifies the element
     * @param {HTMLElement} parentNode - specifies the parent node
     * @param {boolean} isAfter - specifies the boolean value
     * @returns {void}
     * @hidden
     * @deprecated
     */
    InsertMethods.AppendBefore = function (textNode, parentNode, isAfter) {
        return (parentNode.parentNode) ? ((!isAfter) ? parentNode.parentNode.insertBefore(textNode, parentNode)
            : parentNode.parentNode.insertBefore(textNode, parentNode.nextSibling)) :
            parentNode;
    };
    return InsertMethods;
}());

/**
 * Split the Node based on selection
 *
 * @hidden
 * @deprecated
 */
var NodeCutter = /** @class */ (function () {
    function NodeCutter() {
        this.enterAction = 'P';
        this.position = -1;
        this.nodeSelection = new NodeSelection();
    }
    // Split Selection Node
    /**
     * GetSpliceNode method
     *
     * @param {Range} range - specifies the range
     * @param {HTMLElement} node - specifies the node element.
     * @returns {Node} - returns the node value
     * @hidden
     * @deprecated
     */
    NodeCutter.prototype.GetSpliceNode = function (range, node) {
        node = this.SplitNode(range, node, true);
        node = this.SplitNode(range, node, false);
        return node;
    };
    /**
     * @param {Range} range - specifies the range
     * @param {HTMLElement} node - specifies the node element.
     * @param {boolean} isCollapsed - specifies the boolean value
     * @returns {HTMLElement} - returns the element
     * @hidden
     * @deprecated
     */
    NodeCutter.prototype.SplitNode = function (range, node, isCollapsed) {
        if (node) {
            var clone = range.cloneRange();
            var parent_1 = node.parentNode;
            var index = this.nodeSelection.getIndex(node);
            clone.collapse(isCollapsed);
            // eslint-disable-next-line
            (isCollapsed) ? clone.setStartBefore(node) : clone.setEndAfter(node);
            var fragment = clone.extractContents();
            if (isCollapsed) {
                node = parent_1.childNodes[index];
                fragment = this.spliceEmptyNode(fragment, false);
                if (fragment && fragment.childNodes.length > 0) {
                    var isEmpty = (fragment.childNodes.length === 1 && fragment.childNodes[0].nodeName !== 'IMG'
                        && this.isImgElm(fragment) && fragment.textContent === '') ? true : false;
                    if (!isEmpty) {
                        if (node) {
                            InsertMethods.AppendBefore(fragment, node);
                        }
                        else {
                            parent_1.appendChild(fragment);
                            var divNode = document.createElement('div');
                            divNode.innerHTML = '&#65279;&#65279;';
                            node = divNode.firstChild;
                            parent_1.appendChild(node);
                        }
                    }
                }
            }
            else {
                node = parent_1.childNodes.length > 1 ? parent_1.childNodes[index] :
                    parent_1.childNodes[0];
                fragment = this.spliceEmptyNode(fragment, true);
                if (fragment && fragment.childNodes.length > 0) {
                    var isEmpty = (fragment.childNodes.length === 1 && fragment.childNodes[0].nodeName !== 'IMG'
                        && this.isImgElm(fragment) && fragment.textContent.trim() === '') ? true : false;
                    if (!isEmpty) {
                        if (node) {
                            InsertMethods.AppendBefore(fragment, node, true);
                        }
                        else {
                            parent_1.appendChild(fragment);
                            var divNode = document.createElement('div');
                            divNode.innerHTML = '&#65279;&#65279;';
                            parent_1.insertBefore(divNode.firstChild, parent_1.firstChild);
                            node = parent_1.firstChild;
                        }
                    }
                }
            }
            return node;
        }
        else {
            return null;
        }
    };
    NodeCutter.prototype.isImgElm = function (fragment) {
        var result = true;
        if (fragment.childNodes.length === 1 && fragment.childNodes[0].nodeName !== 'IMG') {
            var firstChild = fragment.childNodes[0];
            for (var i = 0; !sf.base.isNullOrUndefined(firstChild.childNodes) && i < firstChild.childNodes.length; i++) {
                if (firstChild.childNodes[i].nodeName === 'IMG') {
                    result = false;
                }
            }
        }
        else {
            result = true;
        }
        return result;
    };
    NodeCutter.prototype.spliceEmptyNode = function (fragment, isStart) {
        var len;
        if (fragment.childNodes.length === 1 && fragment.childNodes[0].nodeName === '#text' &&
            fragment.childNodes[0].textContent === '' || fragment.textContent === '') {
            len = -1;
        }
        else {
            len = fragment.childNodes.length - 1;
        }
        if (len > -1 && !isStart) {
            this.spliceEmptyNode(fragment.childNodes[len], isStart);
        }
        else if (len > -1) {
            this.spliceEmptyNode(fragment.childNodes[0], isStart);
        }
        else if (fragment.nodeType !== 3 && fragment.nodeType !== 11) {
            fragment.parentNode.removeChild(fragment);
        }
        return fragment;
    };
    // Cursor Position split
    NodeCutter.prototype.GetCursorStart = function (indexes, index, isStart) {
        indexes = (isStart) ? indexes : indexes.reverse();
        var position = indexes[0];
        for (var num = 0; num < indexes.length && ((isStart) ? (indexes[num] < index) : (indexes[num] >= index)); num++) {
            position = indexes[num];
        }
        return position;
    };
    /**
     * GetCursorRange method
     *
     * @param {Document} docElement - specifies the document
     * @param {Range} range - specifies the range
     * @param {Node} node - specifies the node.
     * @returns {Range} - returns the range value
     * @hidden
     * @deprecated
     */
    NodeCutter.prototype.GetCursorRange = function (docElement, range, node) {
        var cursorRange = docElement.createRange();
        var indexes = [];
        indexes.push(0);
        var str = this.TrimLineBreak(node.data);
        var index = str.indexOf(' ', 0);
        while (index !== -1) {
            if (indexes.indexOf(index) < 0) {
                indexes.push(index);
            }
            if (new RegExp('\\s').test(str[index - 1]) && (indexes.indexOf(index - 1) < 0)) {
                indexes.push(index - 1);
            }
            if (new RegExp('\\s').test(str[index + 1])) {
                indexes.push(index + 1);
            }
            index = str.indexOf(' ', (index + 1));
        }
        indexes.push(str.length);
        if ((indexes.indexOf(range.startOffset) >= 0)
            || ((indexes.indexOf(range.startOffset - 1) >= 0) && (range.startOffset !== 1
                || (range.startOffset === 1 && new RegExp('\\s').test(str[0])))
                || ((indexes[indexes.length - 1] - 1) === range.startOffset))) {
            cursorRange = range;
            this.position = 1;
        }
        else {
            var startOffset = this.GetCursorStart(indexes, range.startOffset, true);
            this.position = range.startOffset - startOffset;
            cursorRange.setStart(range.startContainer, startOffset);
            cursorRange.setEnd(range.startContainer, this.GetCursorStart(indexes, range.startOffset, false));
        }
        return cursorRange;
    };
    /**
     * GetCursorNode method
     *
     * @param {Document} docElement - specifies the document
     * @param {Range} range - specifies the range
     * @param {Node} node - specifies the node.
     * @returns {Node} - returns the node value
     * @hidden
     * @deprecated
     */
    NodeCutter.prototype.GetCursorNode = function (docElement, range, node) {
        return this.GetSpliceNode(this.GetCursorRange(docElement, range, node), node);
    };
    /**
     * TrimLineBreak method
     *
     * @param {string} line - specifies the string value.
     * @returns {string} - returns the string
     * @hidden
     * @deprecated
     */
    NodeCutter.prototype.TrimLineBreak = function (line) {
        return line.replace(/(\r\n\t|\n|\r\t)/gm, ' ');
    };
    return NodeCutter;
}());

/**
 * Formats internal component
 *
 * @hidden
 * @deprecated
 */
var Formats = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the parent element.
     * @hidden
     * @deprecated
     */
    function Formats(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    Formats.prototype.addEventListener = function () {
        this.parent.observer.on(FORMAT_TYPE, this.applyFormats, this);
        this.parent.observer.on(KEY_UP_HANDLER, this.onKeyUp, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.onKeyDown, this);
    };
    Formats.prototype.getParentNode = function (node) {
        for (; node.parentNode && node.parentNode !== this.parent.editableElement; null) {
            node = node.parentNode;
        }
        return node;
    };
    Formats.prototype.onKeyUp = function (e) {
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        var endCon = range.endContainer;
        var lastChild = endCon.lastChild;
        if (e.event.which === 13 && range.startContainer === endCon && endCon.nodeType !== 3) {
            var pTag = sf.base.createElement('p');
            pTag.innerHTML = '<br>';
            if (!sf.base.isNullOrUndefined(lastChild) && lastChild && lastChild.nodeName === 'BR' && (lastChild.previousSibling && lastChild.previousSibling.nodeName === 'TABLE')) {
                endCon.replaceChild(pTag, lastChild);
                this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, pTag, 0);
            }
            else {
                var brNode = this.parent.nodeSelection.getSelectionNodeCollectionBr(range)[0];
                if (!sf.base.isNullOrUndefined(brNode) && brNode.nodeName === 'BR' && (brNode.previousSibling && brNode.previousSibling.nodeName === 'TABLE')) {
                    endCon.replaceChild(pTag, brNode);
                    this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, pTag, 0);
                }
            }
        }
    };
    Formats.prototype.onKeyDown = function (e) {
        if (e.event.which === 13) {
            var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
            var startCon = (range.startContainer.textContent.length === 0 || range.startContainer.nodeName === 'PRE')
                ? range.startContainer : range.startContainer.parentElement;
            var endCon = (range.endContainer.textContent.length === 0 || range.endContainer.nodeName === 'PRE')
                ? range.endContainer : range.endContainer.parentElement;
            var preElem = sf.base.closest(startCon, 'pre');
            var endPreElem = sf.base.closest(endCon, 'pre');
            var liParent = !sf.base.isNullOrUndefined(preElem) && !sf.base.isNullOrUndefined(preElem.parentElement) && preElem.parentElement.tagName === 'LI';
            if (liParent) {
                return;
            }
            if (((sf.base.isNullOrUndefined(preElem) && !sf.base.isNullOrUndefined(endPreElem)) || (!sf.base.isNullOrUndefined(preElem) && sf.base.isNullOrUndefined(endPreElem)))) {
                e.event.preventDefault();
                this.deleteContent(range);
                this.removeCodeContent(range);
                range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
                this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, endCon, 0);
            }
            if (e.event.which === 13 && !sf.base.isNullOrUndefined(preElem) && !sf.base.isNullOrUndefined(endPreElem)) {
                e.event.preventDefault();
                this.deleteContent(range);
                this.removeCodeContent(range);
                range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
                var lastEmpty = range.startContainer.childNodes[range.endOffset];
                var lastBeforeBr = range.startContainer.childNodes[range.endOffset - 1];
                var startParent = range.startContainer;
                if (!sf.base.isNullOrUndefined(lastEmpty) && !sf.base.isNullOrUndefined(lastBeforeBr) && sf.base.isNullOrUndefined(lastEmpty.nextSibling) &&
                    lastEmpty.nodeName === 'BR' && lastBeforeBr.nodeName === 'BR') {
                    this.paraFocus(range.startContainer, e.enterAction);
                }
                else if ((startParent.textContent.charCodeAt(0) === 8203 &&
                    startParent.textContent.length === 1) || startParent.textContent.length === 0) {
                    //Double enter with any parent tag for the node
                    while (startParent.parentElement.nodeName !== 'PRE' &&
                        (startParent.textContent.length === 1 || startParent.textContent.length === 0)) {
                        startParent = startParent.parentElement;
                    }
                    if (!sf.base.isNullOrUndefined(startParent.previousSibling) && startParent.previousSibling.nodeName === 'BR' &&
                        sf.base.isNullOrUndefined(startParent.nextSibling)) {
                        this.paraFocus(startParent.parentElement);
                    }
                    else {
                        this.isNotEndCursor(preElem, range);
                    }
                }
                else {
                    //Cursor at start and middle
                    this.isNotEndCursor(preElem, range);
                }
            }
        }
    };
    Formats.prototype.removeCodeContent = function (range) {
        var regEx = new RegExp(String.fromCharCode(65279), 'g');
        if (!sf.base.isNullOrUndefined(range.endContainer.textContent.match(regEx))) {
            var pointer = range.endContainer.textContent.charCodeAt(range.endOffset - 1) === 65279 ?
                range.endOffset - 2 : range.endOffset;
            range.endContainer.textContent = range.endContainer.textContent.replace(regEx, '');
            if (range.endContainer.textContent === '') {
                this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, range.endContainer.parentElement, 0);
            }
            else {
                this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, range.endContainer, pointer);
            }
        }
    };
    Formats.prototype.deleteContent = function (range) {
        if (range.startContainer !== range.endContainer || range.startOffset !== range.endOffset) {
            range.deleteContents();
        }
    };
    Formats.prototype.paraFocus = function (referNode, enterAction) {
        var insertTag;
        if (enterAction === 'DIV') {
            insertTag = sf.base.createElement('div');
            insertTag.innerHTML = '<br>';
        }
        else if (enterAction === 'BR') {
            insertTag = sf.base.createElement('br');
        }
        else {
            insertTag = sf.base.createElement('p');
            insertTag.innerHTML = '<br>';
        }
        this.parent.domNode.insertAfter(insertTag, referNode);
        this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, insertTag, 0);
        sf.base.detach(referNode.lastChild);
    };
    Formats.prototype.isNotEndCursor = function (preElem, range) {
        var nodeCutter = new NodeCutter();
        var isEnd = range.startOffset === preElem.lastChild.textContent.length &&
            preElem.lastChild.textContent === range.startContainer.textContent;
        //Cursor at start point
        if (preElem.textContent.indexOf(range.startContainer.textContent) === 0 &&
            ((range.startOffset === 0 && range.endOffset === 0) || range.startContainer.nodeName === 'PRE')) {
            this.insertMarker(preElem, range);
            var brTag = sf.base.createElement('br');
            preElem.childNodes[range.endOffset].parentElement.insertBefore(brTag, preElem.childNodes[range.endOffset]);
        }
        else {
            //Cursor at middle
            var cloneNode = nodeCutter.SplitNode(range, preElem, true);
            this.insertMarker(preElem, range);
            var previousSib = preElem.previousElementSibling;
            if (previousSib.tagName === 'PRE') {
                previousSib.insertAdjacentHTML('beforeend', '<br>' + cloneNode.innerHTML);
                sf.base.detach(preElem);
            }
        }
        //To place the cursor position
        this.setCursorPosition(isEnd, preElem);
    };
    Formats.prototype.setCursorPosition = function (isEnd, preElem) {
        var isEmpty = false;
        var markerElem = this.parent.editableElement.querySelector('.tempSpan');
        var mrkParentElem = markerElem.parentElement;
        // eslint-disable-next-line
        markerElem.parentNode.textContent === '' ? isEmpty = true :
            this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, markerElem, 0);
        if (isEnd) {
            if (isEmpty) {
                //Enter press when pre element is empty
                if (mrkParentElem === preElem) {
                    this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, markerElem, 0);
                    sf.base.detach(markerElem);
                }
                else {
                    this.focusSelectionParent(markerElem, mrkParentElem);
                }
            }
            else {
                var brElm = sf.base.createElement('br');
                this.parent.domNode.insertAfter(brElm, markerElem);
                this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, markerElem, 0);
                sf.base.detach(markerElem);
            }
        }
        else {
            // eslint-disable-next-line
            isEmpty ? this.focusSelectionParent(markerElem, mrkParentElem) : sf.base.detach(markerElem);
        }
    };
    Formats.prototype.focusSelectionParent = function (markerElem, tempSpanPElem) {
        sf.base.detach(markerElem);
        tempSpanPElem.innerHTML = '\u200B';
        this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, tempSpanPElem, 0);
    };
    Formats.prototype.insertMarker = function (preElem, range) {
        var tempSpan = sf.base.createElement('span', { className: 'tempSpan' });
        if (range.startContainer.nodeName === 'PRE') {
            preElem.childNodes[range.endOffset].parentElement.insertBefore(tempSpan, preElem.childNodes[range.endOffset]);
        }
        else {
            range.startContainer.parentElement.insertBefore(tempSpan, range.startContainer);
        }
    };
    Formats.prototype.applyFormats = function (e) {
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        var isSelectAll = false;
        if (this.parent.editableElement === range.endContainer &&
            !sf.base.isNullOrUndefined(this.parent.editableElement.children[range.endOffset - 1]) &&
            this.parent.editableElement.children[range.endOffset - 1].tagName === 'TABLE' && !range.collapsed) {
            isSelectAll = true;
        }
        var save = this.parent.nodeSelection.save(range, this.parent.currentDocument);
        this.parent.domNode.setMarker(save);
        var formatsNodes = this.parent.domNode.blockNodes();
        if (e.enterAction === 'BR') {
            this.setSelectionBRConfig();
            var allSelectedNode = this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument);
            var selectedNodes = this.parent.nodeSelection.getSelectionNodes(allSelectedNode);
            var currentFormatNodes = [];
            if (selectedNodes.length === 0) {
                selectedNodes.push(formatsNodes[0]);
            }
            for (var i = 0; i < selectedNodes.length; i++) {
                var currentNode = selectedNodes[i];
                var previousCurrentNode = void 0;
                while (!this.parent.domNode.isBlockNode(currentNode) && currentNode !== this.parent.editableElement) {
                    previousCurrentNode = currentNode;
                    currentNode = currentNode.parentElement;
                }
                if (this.parent.domNode.isBlockNode(currentNode) && currentNode === this.parent.editableElement) {
                    currentFormatNodes.push(previousCurrentNode);
                }
            }
            for (var i = 0; i < currentFormatNodes.length; i++) {
                if (!this.parent.domNode.isBlockNode(currentFormatNodes[i])) {
                    var currentNode = currentFormatNodes[i];
                    var previousNode = currentNode;
                    while (currentNode === this.parent.editableElement) {
                        previousNode = currentNode;
                        currentNode = currentNode.parentElement;
                    }
                    var tempElem = void 0;
                    if (this.parent.domNode.isBlockNode(previousNode.parentElement) &&
                        previousNode.parentElement === this.parent.editableElement) {
                        tempElem = sf.base.createElement('div');
                        previousNode.parentElement.insertBefore(tempElem, previousNode);
                        tempElem.appendChild(previousNode);
                    }
                    else {
                        tempElem = previousNode;
                    }
                    var preNode = tempElem.previousSibling;
                    while (!sf.base.isNullOrUndefined(preNode) && preNode.nodeName !== 'BR' &&
                        !this.parent.domNode.isBlockNode(preNode)) {
                        tempElem.firstChild.parentElement.insertBefore(preNode, tempElem.firstChild);
                        preNode = tempElem.previousSibling;
                    }
                    if (!sf.base.isNullOrUndefined(preNode) && preNode.nodeName === 'BR') {
                        sf.base.detach(preNode);
                    }
                    var postNode = tempElem.nextSibling;
                    while (!sf.base.isNullOrUndefined(postNode) && postNode.nodeName !== 'BR' &&
                        !this.parent.domNode.isBlockNode(postNode)) {
                        tempElem.appendChild(postNode);
                        postNode = tempElem.nextSibling;
                    }
                    if (!sf.base.isNullOrUndefined(postNode) && postNode.nodeName === 'BR') {
                        sf.base.detach(postNode);
                    }
                }
            }
            this.setSelectionBRConfig();
            formatsNodes = this.parent.domNode.blockNodes();
        }
        for (var i = 0; i < formatsNodes.length; i++) {
            var parentNode = void 0;
            var replaceHTML = void 0;
            if (e.subCommand.toLowerCase() === 'blockquote') {
                parentNode = this.getParentNode(formatsNodes[i]);
                replaceHTML = this.parent.domNode.isList(parentNode) ||
                    parentNode.tagName === 'TABLE' ? parentNode.outerHTML : parentNode.innerHTML;
            }
            else {
                parentNode = formatsNodes[i];
                replaceHTML = parentNode.innerHTML;
            }
            if ((e.subCommand.toLowerCase() === parentNode.tagName.toLowerCase() &&
                (e.subCommand.toLowerCase() !== 'pre' ||
                    (!sf.base.isNullOrUndefined(e.exeValue) && e.exeValue.name === 'dropDownSelect'))) ||
                sf.base.isNullOrUndefined(parentNode.parentNode) ||
                (parentNode.tagName === 'TABLE' && e.subCommand.toLowerCase() === 'pre')) {
                continue;
            }
            this.cleanFormats(parentNode, e.subCommand);
            var replaceNode = (e.subCommand.toLowerCase() === 'pre' && parentNode.tagName.toLowerCase() === 'pre') ?
                'p' : e.subCommand;
            var replaceTag = this.parent.domNode.createTagString(replaceNode, parentNode, replaceHTML.replace(/>\s+</g, '><'));
            if (parentNode.tagName === 'LI') {
                parentNode.innerHTML = '';
                parentNode.insertAdjacentHTML('beforeend', replaceTag);
            }
            else {
                this.parent.domNode.replaceWith(parentNode, replaceTag);
            }
        }
        this.preFormatMerge();
        var startNode = this.parent.editableElement.querySelector('.' + markerClassName.startSelection);
        var endNode = this.parent.editableElement.querySelector('.' + markerClassName.endSelection);
        if (!sf.base.isNullOrUndefined(startNode) && !sf.base.isNullOrUndefined(endNode)) {
            startNode = startNode.lastChild;
            endNode = endNode.lastChild;
        }
        save = this.parent.domNode.saveMarker(save, null);
        if (isIDevice$1()) {
            setEditFrameFocus(this.parent.editableElement, e.selector);
        }
        if (isSelectAll) {
            this.parent.nodeSelection.setSelectionText(this.parent.currentDocument, startNode, endNode, 0, endNode.textContent.length);
        }
        else {
            save.restore();
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.domNode.blockNodes()
            });
        }
    };
    Formats.prototype.setSelectionBRConfig = function () {
        var startElem = this.parent.editableElement.querySelector('.' + markerClassName.startSelection);
        var endElem = this.parent.editableElement.querySelector('.' + markerClassName.endSelection);
        if (sf.base.isNullOrUndefined(endElem)) {
            this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, startElem, 0);
        }
        else {
            this.parent.nodeSelection.setSelectionText(this.parent.currentDocument, startElem, endElem, 0, 0);
        }
    };
    Formats.prototype.preFormatMerge = function () {
        var preNodes = this.parent.editableElement.querySelectorAll('PRE');
        if (!sf.base.isNullOrUndefined(preNodes)) {
            for (var i = 0; i < preNodes.length; i++) {
                var previousSib = preNodes[i].previousElementSibling;
                if (!sf.base.isNullOrUndefined(previousSib) && previousSib.tagName === 'PRE') {
                    previousSib.insertAdjacentHTML('beforeend', '<br>' + preNodes[i].innerHTML);
                    sf.base.detach(preNodes[i]);
                }
            }
        }
    };
    Formats.prototype.cleanFormats = function (element, tagName) {
        var ignoreAttr = ['display', 'font-size', 'margin-top', 'margin-bottom', 'margin-left', 'margin-right', 'font-weight'];
        tagName = tagName.toLowerCase();
        for (var i = 0; i < ignoreAttr.length && (tagName !== 'p' && tagName !== 'blockquote' && tagName !== 'pre'); i++) {
            element.style.removeProperty(ignoreAttr[i]);
        }
    };
    return Formats;
}());

/**
 * Insert a HTML Node or Text
 *
 * @hidden
 * @deprecated
 */
var InsertHtml = /** @class */ (function () {
    function InsertHtml() {
    }
    InsertHtml.Insert = function (docElement, insertNode, editNode, isExternal) {
        var node;
        if (typeof insertNode === 'string') {
            var divNode = document.createElement('div');
            divNode.innerHTML = insertNode;
            node = isExternal ? divNode : divNode.firstChild;
        }
        else {
            if (isExternal && !(!sf.base.isNullOrUndefined(insertNode) && !sf.base.isNullOrUndefined(insertNode.classList) &&
                insertNode.classList.contains('pasteContent'))) {
                var divNode = document.createElement('div');
                divNode.appendChild(insertNode);
                node = divNode;
            }
            else {
                node = insertNode;
            }
        }
        var nodeSelection = new NodeSelection();
        var nodeCutter = new NodeCutter();
        var range = nodeSelection.getRange(docElement);
        if (range.startContainer === editNode && range.startContainer === range.endContainer && range.startOffset === 0 &&
            range.startOffset === range.endOffset && editNode.textContent.length === 0 && editNode.children[0].tagName === 'P') {
            nodeSelection.setSelectionText(docElement, range.startContainer.children[0], range.startContainer.children[0], 0, 0);
            range = nodeSelection.getRange(docElement);
        }
        var isCursor = range.startOffset === range.endOffset && range.startOffset === 0 &&
            range.startContainer === range.endContainer;
        var isCollapsed = range.collapsed;
        var nodes = this.getNodeCollection(range, nodeSelection, node);
        var closestParentNode = (node.nodeName.toLowerCase() === 'table') ? this.closestEle(nodes[0].parentNode, editNode) : nodes[0];
        if (isExternal || (!sf.base.isNullOrUndefined(node) && !sf.base.isNullOrUndefined(node.classList) &&
            node.classList.contains('pasteContent'))) {
            this.pasteInsertHTML(nodes, node, range, nodeSelection, nodeCutter, docElement, isCollapsed, closestParentNode, editNode);
            return;
        }
        if (editNode !== range.startContainer && ((!isCollapsed && !(closestParentNode.nodeType === Node.ELEMENT_NODE &&
            TABLE_BLOCK_TAGS.indexOf(closestParentNode.tagName.toLocaleLowerCase()) !== -1))
            || (node.nodeName.toLowerCase() === 'table' && closestParentNode &&
                TABLE_BLOCK_TAGS.indexOf(closestParentNode.tagName.toLocaleLowerCase()) === -1))) {
            var preNode = nodeCutter.GetSpliceNode(range, closestParentNode);
            var sibNode = preNode.previousSibling;
            var parentNode = preNode.parentNode;
            if (nodes.length === 1 || (node.nodeName.toLowerCase() === 'table' && preNode.childElementCount === 0)) {
                nodeSelection.setSelectionContents(docElement, preNode);
                range = nodeSelection.getRange(docElement);
            }
            else {
                var lasNode = nodeCutter.GetSpliceNode(range, nodes[nodes.length - 1].parentElement);
                lasNode = sf.base.isNullOrUndefined(lasNode) ? preNode : lasNode;
                nodeSelection.setSelectionText(docElement, preNode, lasNode, 0, (lasNode.nodeType === 3) ?
                    lasNode.textContent.length : lasNode.childNodes.length);
                range = nodeSelection.getRange(docElement);
            }
            range.extractContents();
            if (insertNode.tagName === 'TABLE') {
                this.removeEmptyElements(editNode);
            }
            for (var index = 0; index < nodes.length; index++) {
                if (nodes[index].nodeType !== 3 && nodes[index].parentNode != null) {
                    if (nodes[index].nodeName === 'IMG') {
                        continue;
                    }
                    nodes[index].parentNode.removeChild(nodes[index]);
                }
            }
            if (sibNode) {
                InsertMethods.AppendBefore(node, sibNode, true);
            }
            else {
                var previousNode = null;
                while (parentNode !== editNode && parentNode.firstChild &&
                    (parentNode.textContent.trim() === '')) {
                    var parentNode1 = parentNode.parentNode;
                    previousNode = parentNode;
                    parentNode = parentNode1;
                }
                if (previousNode !== null) {
                    parentNode = previousNode;
                }
                if (parentNode.firstChild && (parentNode !== editNode ||
                    (node.nodeName === 'TABLE' && isCursor && parentNode === range.startContainer &&
                        parentNode === range.endContainer))) {
                    if (parentNode.textContent.trim() === '' && parentNode !== editNode) {
                        InsertMethods.AppendBefore(node, parentNode, false);
                        sf.base.detach(parentNode);
                    }
                    else {
                        InsertMethods.AppendBefore(node, parentNode.firstChild, false);
                    }
                }
                else {
                    parentNode.appendChild(node);
                }
            }
            if (node.nodeName === 'IMG') {
                this.imageFocus(node, nodeSelection, docElement);
            }
            else if (node.nodeType !== 3) {
                nodeSelection.setSelectionText(docElement, node, node, 0, node.childNodes.length);
            }
            else {
                nodeSelection.setSelectionText(docElement, node, node, 0, node.textContent.length);
            }
        }
        else {
            range.deleteContents();
            if (isCursor && range.startContainer.textContent === '' && range.startContainer.nodeName !== 'BR') {
                range.startContainer.innerHTML = '';
            }
            if (sf.base.Browser.isIE) {
                var frag = docElement.createDocumentFragment();
                frag.appendChild(node);
                range.insertNode(frag);
            }
            else if (range.startContainer.nodeType === 1 && range.startContainer.nodeName.toLowerCase() === 'hr'
                && range.endContainer.nodeName.toLowerCase() === 'hr') {
                var paraElem = range.startContainer.nextElementSibling;
                if (paraElem) {
                    if (paraElem.querySelector('br')) {
                        sf.base.detach(paraElem.querySelector('br'));
                    }
                    paraElem.appendChild(node);
                }
            }
            else {
                if (range.startContainer.nodeName === 'BR') {
                    range.startContainer.parentElement.insertBefore(node, range.startContainer);
                }
                else {
                    range.insertNode(node);
                }
            }
            if (node.nodeType !== 3 && node.childNodes.length > 0) {
                nodeSelection.setSelectionText(docElement, node, node, 1, 1);
            }
            else if (node.nodeName === 'IMG') {
                this.imageFocus(node, nodeSelection, docElement);
            }
            else if (node.nodeType !== 3) {
                nodeSelection.setSelectionContents(docElement, node);
            }
            else {
                nodeSelection.setSelectionText(docElement, node, node, node.textContent.length, node.textContent.length);
            }
        }
    };
    InsertHtml.pasteInsertHTML = function (nodes, node, range, nodeSelection, nodeCutter, docElement, isCollapsed, closestParentNode, editNode) {
        var isCursor = range.startOffset === range.endOffset &&
            range.startContainer === range.endContainer;
        if (isCursor && range.startContainer === editNode && editNode.textContent === '') {
            var currentBlockNode = this.getImmediateBlockNode(nodes[nodes.length - 1], editNode);
            nodeSelection.setSelectionText(docElement, currentBlockNode, currentBlockNode, 0, 0);
            range = nodeSelection.getRange(docElement);
        }
        var lasNode;
        var sibNode;
        var isSingleNode;
        var preNode;
        if (editNode !== range.startContainer && ((!isCollapsed && !(closestParentNode.nodeType === Node.ELEMENT_NODE &&
            TABLE_BLOCK_TAGS.indexOf(closestParentNode.tagName.toLocaleLowerCase()) !== -1))
            || (node.nodeName.toLowerCase() === 'table' && closestParentNode &&
                TABLE_BLOCK_TAGS.indexOf(closestParentNode.tagName.toLocaleLowerCase()) === -1))) {
            preNode = nodeCutter.GetSpliceNode(range, closestParentNode);
            sibNode = sf.base.isNullOrUndefined(preNode.previousSibling) ? preNode.parentNode.previousSibling : preNode.previousSibling;
            if (nodes.length === 1) {
                nodeSelection.setSelectionContents(docElement, preNode);
                range = nodeSelection.getRange(docElement);
                isSingleNode = true;
            }
            else {
                lasNode = nodeCutter.GetSpliceNode(range, nodes[nodes.length - 1].parentElement);
                lasNode = sf.base.isNullOrUndefined(lasNode) ? preNode : lasNode;
                nodeSelection.setSelectionText(docElement, preNode, lasNode, 0, (lasNode.nodeType === 3) ?
                    lasNode.textContent.length : lasNode.childNodes.length);
                range = nodeSelection.getRange(docElement);
                isSingleNode = false;
            }
        }
        var containsBlockNode = false;
        this.removingComments(node);
        var allChildNodes = node.childNodes;
        for (var i = 0; i < allChildNodes.length; i++) {
            if (BLOCK_TAGS.indexOf(allChildNodes[i].nodeName.toLocaleLowerCase()) >= 0) {
                containsBlockNode = true;
                break;
            }
        }
        var lastSelectionNode;
        var fragment = document.createDocumentFragment();
        if (!containsBlockNode) {
            if (!isCursor) {
                while (node.firstChild) {
                    lastSelectionNode = node.firstChild;
                    fragment.appendChild(node.firstChild);
                }
                if (isSingleNode) {
                    preNode.parentNode.replaceChild(fragment, preNode);
                }
                else {
                    range.deleteContents();
                    sf.base.detach(lasNode);
                    // eslint-disable-next-line
                    !sf.base.isNullOrUndefined(sibNode) ? sibNode.parentNode.appendChild(fragment) : editNode.appendChild(fragment);
                }
            }
            else {
                var tempSpan = sf.base.createElement('span', { className: 'tempSpan' });
                var nearestAnchor = sf.base.closest(range.startContainer.parentElement, 'a');
                if (range.startContainer.nodeType === 3 && nearestAnchor) {
                    var immediateBlockNode = this.getImmediateBlockNode(range.startContainer, editNode);
                    if (immediateBlockNode.querySelectorAll('br').length > 0) {
                        sf.base.detach(immediateBlockNode.querySelector('br'));
                    }
                    var rangeElement = sf.base.closest(nearestAnchor, 'span');
                    rangeElement.appendChild(tempSpan);
                }
                else {
                    range.insertNode(tempSpan);
                }
                while (node.firstChild) {
                    lastSelectionNode = node.firstChild;
                    fragment.appendChild(node.firstChild);
                }
                tempSpan.parentNode.replaceChild(fragment, tempSpan);
            }
        }
        else {
            this.insertTempNode(range, node, nodes, nodeCutter, editNode);
            var isFirstTextNode = true;
            var isPreviousInlineElem = void 0;
            var paraElm = void 0;
            var previousParent = void 0;
            range.deleteContents();
            while (node.firstChild) {
                if (node.firstChild.nodeName === '#text' && node.firstChild.textContent.trim() === '') {
                    sf.base.detach(node.firstChild);
                    continue;
                }
                if (node.firstChild.nodeName === '#text' && isFirstTextNode ||
                    (this.inlineNode.indexOf(node.firstChild.nodeName.toLocaleLowerCase()) >= 0 && isFirstTextNode)) {
                    lastSelectionNode = node.firstChild;
                    if (sf.base.isNullOrUndefined(node.previousElementSibling)) {
                        var firstParaElm = sf.base.createElement('p');
                        node.parentElement.insertBefore(firstParaElm, node);
                    }
                    node.previousElementSibling.appendChild(node.firstChild);
                }
                else {
                    lastSelectionNode = node.firstChild;
                    if (node.firstChild.nodeName === '#text' ||
                        (this.inlineNode.indexOf(node.firstChild.nodeName.toLocaleLowerCase()) >= 0)) {
                        if (!isPreviousInlineElem) {
                            paraElm = sf.base.createElement('p');
                            paraElm.appendChild(node.firstChild);
                            fragment.appendChild(paraElm);
                        }
                        else {
                            previousParent.appendChild(node.firstChild);
                            fragment.appendChild(previousParent);
                        }
                        previousParent = paraElm;
                        isPreviousInlineElem = true;
                    }
                    else {
                        fragment.appendChild(node.firstChild);
                        isPreviousInlineElem = false;
                    }
                    isFirstTextNode = false;
                }
            }
            node.parentNode.replaceChild(fragment, node);
        }
        if (lastSelectionNode.nodeName === '#text') {
            this.placeCursorEnd(lastSelectionNode, node, nodeSelection, docElement, editNode);
        }
        else {
            this.cursorPos(lastSelectionNode, node, nodeSelection, docElement, editNode);
        }
    };
    InsertHtml.placeCursorEnd = function (lastSelectionNode, node, nodeSelection, docElement, editNode) {
        lastSelectionNode = lastSelectionNode.nodeName === 'BR' ? (sf.base.isNullOrUndefined(lastSelectionNode.previousSibling) ? lastSelectionNode.parentNode
            : lastSelectionNode.previousSibling) : lastSelectionNode;
        while (!sf.base.isNullOrUndefined(lastSelectionNode) && lastSelectionNode.nodeName !== '#text' && lastSelectionNode.nodeName !== 'IMG' &&
            lastSelectionNode.nodeName !== 'BR' && lastSelectionNode.nodeName !== 'HR') {
            lastSelectionNode = lastSelectionNode.lastChild;
        }
        lastSelectionNode = sf.base.isNullOrUndefined(lastSelectionNode) ? node : lastSelectionNode;
        if (lastSelectionNode.nodeName === 'IMG') {
            this.imageFocus(lastSelectionNode, nodeSelection, docElement);
        }
        else {
            nodeSelection.setSelectionText(docElement, lastSelectionNode, lastSelectionNode, lastSelectionNode.textContent.length, lastSelectionNode.textContent.length);
        }
        this.removeEmptyElements(editNode);
    };
    InsertHtml.getNodeCollection = function (range, nodeSelection, node) {
        var nodes = [];
        if (range.startOffset === range.endOffset && range.startContainer === range.endContainer
            && (range.startContainer.nodeName === 'TD' || (range.startContainer.nodeType !== 3 &&
                node.classList && node.classList.contains('pasteContent')))) {
            nodes.push(range.startContainer.childNodes[range.endOffset]);
        }
        else {
            nodes = nodeSelection.getInsertNodeCollection(range);
        }
        return nodes;
    };
    InsertHtml.insertTempNode = function (range, node, nodes, nodeCutter, editNode) {
        if (range.startContainer === editNode && !sf.base.isNullOrUndefined(range.startContainer.childNodes[range.endOffset - 1]) &&
            range.startContainer.childNodes[range.endOffset - 1].nodeName === 'TABLE') {
            if (sf.base.isNullOrUndefined(range.startContainer.childNodes[range.endOffset - 1].nextSibling)) {
                range.startContainer.appendChild(node);
            }
            else {
                range.startContainer.insertBefore(node, range.startContainer.childNodes[range.endOffset - 1].nextSibling);
            }
        }
        else if (range.startContainer === editNode && !sf.base.isNullOrUndefined(range.startContainer.childNodes[range.endOffset]) &&
            range.startContainer.childNodes[range.endOffset].nodeName === 'TABLE') {
            range.startContainer.insertBefore(node, range.startContainer.childNodes[range.endOffset]);
        }
        else if (range.startContainer === range.endContainer && range.startContainer.nodeType !== 3
            && node.firstChild.nodeName === 'HR') {
            if (range.startContainer.classList.contains('e-content') || range.startContainer.nodeName === 'BODY') {
                range.startContainer.appendChild(node);
            }
            else {
                range.startContainer.parentNode.insertBefore(node, range.startContainer);
            }
        }
        else {
            var blockNode = this.getImmediateBlockNode(nodes[nodes.length - 1], editNode);
            if ((sf.base.isNullOrUndefined(blockNode) || sf.base.isNullOrUndefined(blockNode.parentElement)) && range.endContainer.nodeType !== 3) {
                blockNode = range.endContainer;
                range.setEnd(blockNode, range.endContainer.textContent.length);
            }
            if (blockNode.nodeName === 'BODY' && range.startContainer === range.endContainer && range.startContainer.nodeType === 1) {
                blockNode = range.startContainer;
            }
            if (blockNode.closest('LI') && node && node.firstElementChild &&
                ((node).firstElementChild.tagName === 'OL' || node.firstElementChild.tagName === 'UL')) {
                var liNode = void 0;
                while (node.firstElementChild.lastElementChild && node.firstElementChild.lastElementChild.tagName === 'LI') {
                    liNode = node.firstElementChild.lastElementChild;
                    liNode.style.removeProperty('margin-left');
                    liNode.style.removeProperty('margin-top');
                    liNode.style.removeProperty('margin-bottom');
                    node.firstElementChild.insertAdjacentElement('afterend', liNode);
                }
            }
            if (blockNode.nodeName === 'TD' || blockNode.nodeName === 'TH') {
                var tempSpan = sf.base.createElement('span', { className: 'tempSpan' });
                range.insertNode(tempSpan);
                tempSpan.parentNode.replaceChild(node, tempSpan);
            }
            else {
                var splitedElm = nodeCutter.GetSpliceNode(range, blockNode);
                splitedElm.parentNode.replaceChild(node, splitedElm);
            }
        }
    };
    InsertHtml.cursorPos = function (lastSelectionNode, node, nodeSelection, docElement, editNode) {
        lastSelectionNode.classList.add('lastNode');
        editNode.innerHTML = updateTextNode$1(editNode.innerHTML);
        lastSelectionNode = editNode.querySelector('.lastNode');
        if (!sf.base.isNullOrUndefined(lastSelectionNode)) {
            this.placeCursorEnd(lastSelectionNode, node, nodeSelection, docElement, editNode);
            lastSelectionNode.classList.remove('lastNode');
            if (lastSelectionNode.classList.length === 0) {
                lastSelectionNode.removeAttribute('class');
            }
        }
    };
    InsertHtml.imageFocus = function (node, nodeSelection, docElement) {
        var focusNode = document.createTextNode(' ');
        node.parentNode.insertBefore(focusNode, node.nextSibling);
        nodeSelection.setSelectionText(docElement, node.nextSibling, node.nextSibling, 0, 0);
    };
    // eslint-disable-next-line
    InsertHtml.getImmediateBlockNode = function (node, editNode) {
        do {
            node = node.parentNode;
        } while (node && BLOCK_TAGS.indexOf(node.nodeName.toLocaleLowerCase()) < 0);
        return node;
    };
    InsertHtml.removingComments = function (elm) {
        var innerElement = elm.innerHTML;
        innerElement = innerElement.replace(/<!--[\s\S]*?-->/g, '');
        elm.innerHTML = innerElement;
    };
    InsertHtml.findDetachEmptyElem = function (element) {
        var removableElement;
        if (!sf.base.isNullOrUndefined(element.parentElement)) {
            if (element.parentElement.textContent.trim() === '' && element.parentElement.contentEditable !== 'true') {
                removableElement = this.findDetachEmptyElem(element.parentElement);
            }
            else {
                removableElement = element;
            }
        }
        else {
            removableElement = null;
        }
        return removableElement;
    };
    InsertHtml.removeEmptyElements = function (element) {
        var emptyElements = element.querySelectorAll(':empty');
        for (var i = 0; i < emptyElements.length; i++) {
            if (emptyElements[i].tagName !== 'IMG' && emptyElements[i].tagName !== 'BR' &&
                emptyElements[i].tagName !== 'IFRAME' && emptyElements[i].tagName !== 'TD' &&
                emptyElements[i].tagName !== 'SOURCE' && emptyElements[i].tagName !== 'HR') {
                var detachableElement = this.findDetachEmptyElem(emptyElements[i]);
                if (!sf.base.isNullOrUndefined(detachableElement)) {
                    sf.base.detach(detachableElement);
                }
            }
        }
    };
    InsertHtml.closestEle = function (element, editNode) {
        var el = element;
        while (el && el.nodeType === 1) {
            if (el.parentNode === editNode ||
                (!sf.base.isNullOrUndefined(el.parentNode.tagName) &&
                    IGNORE_BLOCK_TAGS.indexOf(el.parentNode.tagName.toLocaleLowerCase()) !== -1)) {
                return el;
            }
            el = el.parentNode;
        }
        return null;
    };
    /**
     * Insert method
     *
     * @hidden
     * @deprecated
     */
    InsertHtml.inlineNode = ['a', 'abbr', 'acronym', 'audio', 'b', 'bdi', 'bdo', 'big', 'br', 'button',
        'canvas', 'cite', 'code', 'data', 'datalist', 'del', 'dfn', 'em', 'embed', 'font', 'i', 'iframe', 'img', 'input',
        'ins', 'kbd', 'label', 'map', 'mark', 'meter', 'noscript', 'object', 'output', 'picture', 'progress',
        'q', 'ruby', 's', 'samp', 'script', 'select', 'slot', 'small', 'span', 'strong', 'sub', 'sup', 'svg',
        'template', 'textarea', 'time', 'u', 'tt', 'var', 'video', 'wbr'];
    return InsertHtml;
}());

/**
 * Link internal component
 *
 * @hidden
 * @deprecated
 */
var LinkCommand = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the editor manager
     * @hidden
     * @deprecated
     */
    function LinkCommand(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    LinkCommand.prototype.addEventListener = function () {
        this.parent.observer.on(LINK, this.linkCommand, this);
    };
    LinkCommand.prototype.linkCommand = function (e) {
        switch (e.value.toString().toLocaleLowerCase()) {
            case 'createlink':
            case 'editlink':
                this.createLink(e);
                break;
            case 'openlink':
                this.openLink(e);
                break;
            case 'removelink':
                this.removeLink(e);
                break;
        }
    };
    LinkCommand.prototype.createLink = function (e) {
        var closestAnchor = (!sf.base.isNullOrUndefined(e.item.selectParent) && e.item.selectParent.length > 0) &&
            sf.base.closest(e.item.selectParent[0], 'a');
        closestAnchor = !sf.base.isNullOrUndefined(closestAnchor) ? closestAnchor :
            (!sf.base.isNullOrUndefined(e.item.selectParent) && e.item.selectParent.length > 0) ? (e.item.selectParent[0]) : null;
        if (!sf.base.isNullOrUndefined(closestAnchor) && closestAnchor.tagName === 'A') {
            var anchorEle = closestAnchor;
            var linkText = '';
            if (!sf.base.isNullOrUndefined(e.item.url)) {
                anchorEle.setAttribute('href', e.item.url);
            }
            if (!sf.base.isNullOrUndefined(e.item.title)) {
                anchorEle.setAttribute('title', e.item.title);
            }
            if (!sf.base.isNullOrUndefined(e.item.text) && e.item.text !== '') {
                linkText = anchorEle.innerText;
                anchorEle.innerText = e.item.text;
            }
            if (!sf.base.isNullOrUndefined(e.item.target)) {
                anchorEle.setAttribute('target', e.item.target);
            }
            else {
                anchorEle.removeAttribute('target');
            }
            if (linkText === e.item.text) {
                e.item.selection.setSelectionText(this.parent.currentDocument, anchorEle, anchorEle, 1, 1);
                e.item.selection.restore();
            }
            else {
                var startIndex = e.item.action === 'Paste' ? anchorEle.childNodes[0].textContent.length : 0;
                e.item.selection.setSelectionText(this.parent.currentDocument, anchorEle.childNodes[0], anchorEle.childNodes[0], startIndex, anchorEle.childNodes[0].textContent.length);
            }
        }
        else {
            var domSelection = new NodeSelection();
            var range = domSelection.getRange(this.parent.currentDocument);
            var text = sf.base.isNullOrUndefined(e.item.text) ? true : e.item.text.replace(/ /g, '').localeCompare(range.toString()
                .replace(/\n/g, ' ').replace(/ /g, '')) < 0;
            if (e.event && e.event.type === 'keydown' && (e.event.keyCode === 32
                || e.event.keyCode === 13) || e.item.action === 'Paste' || range.collapsed || text) {
                var anchor = this.createAchorNode(e);
                anchor.innerText = e.item.text === '' ? e.item.url : e.item.text;
                e.item.selection.restore();
                InsertHtml.Insert(this.parent.currentDocument, anchor, this.parent.editableElement);
                if (e.event && e.event.type === 'keydown' && (e.event.keyCode === 32
                    || e.event.keyCode === 13)) {
                    var startContainer = e.item.selection.range.startContainer;
                    startContainer.textContent = this.removeText(startContainer.textContent, e.item.text);
                }
                else {
                    var startIndex = e.item.action === 'Paste' ? anchor.childNodes[0].textContent.length : 0;
                    e.item.selection.setSelectionText(this.parent.currentDocument, anchor.childNodes[0], anchor.childNodes[0], startIndex, anchor.childNodes[0].textContent.length);
                }
            }
            else {
                this.createLinkNode(e);
            }
        }
        if (e.callBack) {
            e.callBack({
                requestType: 'Links',
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    LinkCommand.prototype.createLinkNode = function (e) {
        var domSelection = new NodeSelection();
        var nodeCutter = new NodeCutter();
        var range = domSelection.getRange(this.parent.currentDocument);
        var nodes = this.getSelectionNodes(domSelection.getNodeCollection(range));
        var save = domSelection.save(range, this.parent.currentDocument);
        var txtArray = [];
        var inlineNodes = [];
        var currentNode;
        var removeNodes = [];
        var anchorNodes = [];
        var finalinlineNodes = [];
        var cloneNode;
        for (var index = 0; index < nodes.length; index++) {
            nodes[index] = nodeCutter.GetSpliceNode(range, nodes[index]);
            txtArray[index] = nodes[index];
        }
        for (var i = 0; i < txtArray.length; i++) {
            var check = true;
            currentNode = txtArray[i];
            while (check === true) {
                if (currentNode.parentNode.nodeName === 'A') {
                    var anchorEle = currentNode.parentNode;
                    currentNode.parentNode.parentNode.insertBefore(anchorEle.firstChild, anchorEle);
                    currentNode.parentNode.removeChild(anchorEle);
                }
                if (this.isBlockNode(currentNode.parentNode) || txtArray.length === 0 || i === 0 || i === txtArray.length - 1
                    || range.startContainer.nodeType === 3) {
                    inlineNodes[i] = currentNode;
                    check = false;
                }
                else {
                    currentNode = currentNode.parentNode;
                }
            }
        }
        for (var i = 0, j_1 = 0; i < inlineNodes.length; i++) {
            if (i === 0) {
                finalinlineNodes[j_1] = inlineNodes[i];
            }
            if (inlineNodes.length > 1 && i < inlineNodes.length - 1) {
                if ((inlineNodes[i].parentElement === inlineNodes[i + 1].parentElement) && (inlineNodes[i] === inlineNodes[i + 1])) {
                    continue;
                }
                else {
                    finalinlineNodes[j_1 + 1] = inlineNodes[i + 1];
                    j_1++;
                }
            }
        }
        var j = 0;
        anchorNodes[j] = this.createAchorNode(e);
        for (var i = 0; i < finalinlineNodes.length; i++) {
            if (i === 0) {
                cloneNode = finalinlineNodes[i].cloneNode(true);
                anchorNodes[i].appendChild(cloneNode);
            }
            if (i < finalinlineNodes.length - 1) {
                if (finalinlineNodes[i].parentNode === finalinlineNodes[i + 1].parentNode) {
                    var cln = finalinlineNodes[i + 1].cloneNode(true);
                    anchorNodes[j].appendChild(cln);
                }
                else {
                    j = j + 1;
                    anchorNodes[j] = this.createAchorNode(e);
                    cloneNode = finalinlineNodes[i + 1].cloneNode(true);
                    anchorNodes[j].appendChild(cloneNode);
                }
            }
        }
        this.parent.nodeSelection.setRange(document, save.range);
        for (var i = 0, j_2 = 0, k = 0; i <= finalinlineNodes.length; i++) {
            if (i === 0) {
                finalinlineNodes[i].parentNode.insertBefore(anchorNodes[j_2], finalinlineNodes[i].nextSibling);
                if (this.parent.domNode.blockNodes().length === 1) {
                    this.parent.nodeSelection.setSelectionNode(this.parent.currentDocument, anchorNodes[j_2]);
                }
                removeNodes[k] = finalinlineNodes[i];
                k++;
            }
            if (i < finalinlineNodes.length - 1) {
                if (finalinlineNodes[i].parentNode === finalinlineNodes[i + 1].parentNode) {
                    removeNodes[k] = finalinlineNodes[i + 1];
                    k++;
                }
                else {
                    j_2 = j_2 + 1;
                    finalinlineNodes[i + 1].parentNode.insertBefore(anchorNodes[j_2], finalinlineNodes[i + 1]);
                    removeNodes[k] = finalinlineNodes[i + 1];
                    k++;
                }
            }
        }
        for (var i = 0; i < removeNodes.length; i++) {
            if (removeNodes[i].parentNode) {
                removeNodes[i].parentNode.removeChild(removeNodes[i]);
            }
        }
    };
    LinkCommand.prototype.createAchorNode = function (e) {
        var anchorEle = sf.base.createElement('a', {
            className: 'e-rte-anchor',
            attrs: {
                href: e.item.url,
                title: sf.base.isNullOrUndefined(e.item.title) || e.item.title === '' ? e.item.url : e.item.title
            }
        });
        if (!sf.base.isNullOrUndefined(e.item.target)) {
            anchorEle.setAttribute('target', e.item.target);
        }
        return anchorEle;
    };
    LinkCommand.prototype.getSelectionNodes = function (nodeCollection) {
        nodeCollection = nodeCollection.reverse();
        for (var index = 0; index < nodeCollection.length; index++) {
            if (nodeCollection[index].nodeType !== 3 || nodeCollection[index].textContent.trim() === '') {
                if (nodeCollection[index].nodeName !== 'IMG') {
                    nodeCollection.splice(index, 1);
                    index--;
                }
            }
        }
        return nodeCollection.reverse();
    };
    LinkCommand.prototype.isBlockNode = function (element) {
        return (!!element && (element.nodeType === Node.ELEMENT_NODE && BLOCK_TAGS.indexOf(element.tagName.toLowerCase()) >= 0));
    };
    LinkCommand.prototype.removeText = function (text, val) {
        var arr = text.split(' ');
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] === val) {
                arr.splice(i, 1);
                i--;
            }
        }
        return arr.join(' ') + ' ';
    };
    LinkCommand.prototype.openLink = function (e) {
        document.defaultView.open(e.item.url, e.item.target);
        this.callBack(e);
    };
    LinkCommand.prototype.removeLink = function (e) {
        var blockNodes = this.parent.domNode.blockNodes();
        if (blockNodes.length < 2) {
            this.parent.domNode.setMarker(e.item.selection);
            var closestAnchor = sf.base.closest(e.item.selectParent[0], 'a');
            var selectParent = closestAnchor ? closestAnchor : e.item.selectParent[0];
            var parent_1 = selectParent.parentNode;
            var child = [];
            for (; selectParent.firstChild; null) {
                child.push(parent_1.insertBefore(selectParent.firstChild, selectParent));
            }
            parent_1.removeChild(selectParent);
            if (child && child.length === 1) {
                e.item.selection.startContainer = e.item.selection.getNodeArray(child[child.length - 1], true);
                e.item.selection.endContainer = e.item.selection.startContainer;
            }
            e.item.selection = this.parent.domNode.saveMarker(e.item.selection);
        }
        else {
            for (var i = 0; i < blockNodes.length; i++) {
                var linkNode = blockNodes[i].querySelectorAll('a');
                for (var j = 0; j < linkNode.length; j++) {
                    if (document.getSelection().containsNode(linkNode[j], true)) {
                        linkNode[j].outerHTML = linkNode[j].innerHTML;
                    }
                }
            }
        }
        e.item.selection.restore();
        this.callBack(e);
    };
    LinkCommand.prototype.callBack = function (e) {
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    return LinkCommand;
}());

/**
 * Formats internal component
 *
 * @hidden
 * @deprecated
 */
var Alignments = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the parent element.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    function Alignments(parent) {
        this.alignments = {
            'JustifyLeft': 'left',
            'JustifyCenter': 'center',
            'JustifyRight': 'right',
            'JustifyFull': 'justify'
        };
        this.parent = parent;
        this.addEventListener();
    }
    Alignments.prototype.addEventListener = function () {
        this.parent.observer.on(ALIGNMENT_TYPE, this.applyAlignment, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.onKeyDown, this);
    };
    Alignments.prototype.onKeyDown = function (e) {
        switch (e.event.action) {
            case 'justify-center':
                this.applyAlignment({ subCommand: 'JustifyCenter', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'justify-full':
                this.applyAlignment({ subCommand: 'JustifyFull', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'justify-left':
                this.applyAlignment({ subCommand: 'JustifyLeft', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'justify-right':
                this.applyAlignment({ subCommand: 'JustifyRight', callBack: e.callBack });
                e.event.preventDefault();
                break;
        }
    };
    Alignments.prototype.getTableNode = function (range) {
        var startNode = range.startContainer.nodeType === Node.ELEMENT_NODE
            ? range.startContainer : range.startContainer.parentNode;
        var cellNode = sf.base.closest(startNode, 'td,th');
        return [cellNode];
    };
    Alignments.prototype.applyAlignment = function (e) {
        var isTableAlign = e.value === 'Table' ? true : false;
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        var save = this.parent.nodeSelection.save(range, this.parent.currentDocument);
        if (!isTableAlign) {
            this.parent.domNode.setMarker(save);
            var alignmentNodes = this.parent.domNode.blockNodes();
            for (var i = 0; i < alignmentNodes.length; i++) {
                var parentNode = alignmentNodes[i];
                sf.base.setStyleAttribute(parentNode, { 'text-align': this.alignments[e.subCommand] });
            }
            var imageTags = this.parent.domNode.getImageTagInSelection();
            for (var i = 0; i < imageTags.length; i++) {
                var elementNode = [];
                elementNode.push(imageTags[i]);
                this.parent.imgObj.imageCommand({
                    item: {
                        selectNode: elementNode
                    },
                    subCommand: e.subCommand,
                    value: e.subCommand,
                    callBack: e.callBack,
                    selector: e.selector
                });
            }
            this.parent.editableElement.focus();
            save = this.parent.domNode.saveMarker(save);
            if (isIDevice$1()) {
                setEditFrameFocus(this.parent.editableElement, e.selector);
            }
            save.restore();
        }
        else {
            sf.base.setStyleAttribute(this.getTableNode(range)[0], { 'text-align': this.alignments[e.subCommand] });
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: (isTableAlign ? this.getTableNode(range) : this.parent.domNode.blockNodes())
            });
        }
    };
    return Alignments;
}());

/**
 * Indents internal component
 *
 * @hidden
 * @deprecated
 */
var Indents = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function Indents(parent) {
        this.indentValue = 20;
        this.parent = parent;
        this.addEventListener();
    }
    Indents.prototype.addEventListener = function () {
        this.parent.observer.on(INDENT_TYPE, this.applyIndents, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.onKeyDown, this);
    };
    Indents.prototype.onKeyDown = function (e) {
        switch (e.event.action) {
            case 'indents':
                this.applyIndents({ subCommand: 'Indent', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'outdents':
                this.applyIndents({ subCommand: 'Outdent', callBack: e.callBack });
                e.event.preventDefault();
                break;
        }
    };
    Indents.prototype.applyIndents = function (e) {
        var editEle = this.parent.editableElement;
        var isRtl = editEle.classList.contains('e-rtl');
        var range = this.parent.nodeSelection.getRange(this.parent.currentDocument);
        var save = this.parent.nodeSelection.save(range, this.parent.currentDocument);
        this.parent.domNode.setMarker(save);
        var indentsNodes = this.parent.domNode.blockNodes();
        var parentNodes = indentsNodes.slice();
        var listsNodes = [];
        for (var i = 0; i < parentNodes.length; i++) {
            if (parentNodes[i].tagName !== 'LI' && 'LI' === parentNodes[i].parentNode.tagName) {
                indentsNodes.splice(indentsNodes.indexOf(parentNodes[i]), 1);
                listsNodes.push(parentNodes[i].parentNode);
            }
            else if (parentNodes[i].tagName === 'LI') {
                indentsNodes.splice(indentsNodes.indexOf(parentNodes[i]), 1);
                listsNodes.push(parentNodes[i]);
            }
        }
        if (listsNodes.length > 0) {
            this.parent.observer.notify(KEY_DOWN_HANDLER, {
                event: {
                    preventDefault: function () {
                        return;
                    },
                    stopPropagation: function () {
                        return;
                    },
                    shiftKey: (e.subCommand === 'Indent' ? false : true),
                    which: 9,
                    action: 'indent'
                },
                ignoreDefault: true
            });
        }
        for (var i = 0; i < indentsNodes.length; i++) {
            var parentNode = indentsNodes[i];
            var marginLeftOrRight = isRtl ? parentNode.style.marginRight : parentNode.style.marginLeft;
            var indentsValue = void 0;
            if (e.subCommand === 'Indent') {
                /* eslint-disable */
                indentsValue = marginLeftOrRight === '' ? this.indentValue + 'px' : parseInt(marginLeftOrRight, null) + this.indentValue + 'px';
                isRtl ? (parentNode.style.marginRight = indentsValue) : (parentNode.style.marginLeft = indentsValue);
            }
            else {
                indentsValue = (marginLeftOrRight === '' || marginLeftOrRight === '0px') ? '' : parseInt(marginLeftOrRight, null) - this.indentValue + 'px';
                isRtl ? (parentNode.style.marginRight = indentsValue) : (parentNode.style.marginLeft = indentsValue);
                /* eslint-enable */
            }
        }
        editEle.focus();
        if (isIDevice$1()) {
            setEditFrameFocus(editEle, e.selector);
        }
        save = this.parent.domNode.saveMarker(save);
        save.restore();
        if (e.callBack) {
            e.callBack({
                requestType: e.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.domNode.blockNodes()
            });
        }
    };
    return Indents;
}());

/**
 * Rich Text Editor classes defined here.
 */
/**
 * @hidden
 * @deprecated
 */
var CLASS_IMAGE_RIGHT = 'e-imgright';
var CLASS_IMAGE_LEFT = 'e-imgleft';
var CLASS_IMAGE_CENTER = 'e-imgcenter';
var CLASS_IMAGE_BREAK = 'e-imgbreak';
var CLASS_CAPTION = 'e-img-caption';

var CLASS_CAPTION_INLINE = 'e-caption-inline';
var CLASS_IMAGE_INLINE = 'e-imginline';

/**
 * Link internal component
 *
 * @hidden
 * @deprecated
 */
var ImageCommand = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function ImageCommand(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    ImageCommand.prototype.addEventListener = function () {
        this.parent.observer.on(IMAGE, this.imageCommand, this);
    };
    /**
     * imageCommand method
     *
     * @param {IHtmlItem} e - specifies the element
     * @returns {void}
     * @hidden
     * @deprecated
     */
    ImageCommand.prototype.imageCommand = function (e) {
        switch (e.value.toString().toLowerCase()) {
            case 'image':
            case 'replace':
                this.createImage(e);
                break;
            case 'insertlink':
                this.insertImageLink(e);
                break;
            case 'openimagelink':
                this.openImageLink(e);
                break;
            case 'editimagelink':
                this.editImageLink(e);
                break;
            case 'removeimagelink':
                this.removeImageLink(e);
                break;
            case 'remove':
                this.removeImage(e);
                break;
            case 'alttext':
                this.insertAltTextImage(e);
                break;
            case 'dimension':
                this.imageDimension(e);
                break;
            case 'caption':
                this.imageCaption(e);
                break;
            case 'justifyleft':
                this.imageJustifyLeft(e);
                break;
            case 'justifycenter':
                this.imageJustifyCenter(e);
                break;
            case 'justifyright':
                this.imageJustifyRight(e);
                break;
            case 'inline':
                this.imageInline(e);
                break;
            case 'break':
                this.imageBreak(e);
                break;
        }
    };
    ImageCommand.prototype.createImage = function (e) {
        var _this = this;
        var isReplaced = false;
        e.item.url = sf.base.isNullOrUndefined(e.item.url) || e.item.url === 'undefined' ? e.item.src : e.item.url;
        if (!sf.base.isNullOrUndefined(e.item.selectParent) && e.item.selectParent[0].tagName === 'IMG') {
            var imgEle = e.item.selectParent[0];
            this.setStyle(imgEle, e);
            isReplaced = true;
        }
        else {
            var imgElement = sf.base.createElement('img');
            this.setStyle(imgElement, e);
            if (!sf.base.isNullOrUndefined(e.item.selection)) {
                e.item.selection.restore();
            }
            if (!sf.base.isNullOrUndefined(e.selector) && e.selector === 'pasteCleanupModule') {
                e.callBack({ requestType: 'Images',
                    editorMode: 'HTML',
                    event: e.event,
                    range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                    elements: [imgElement]
                });
            }
            else {
                InsertHtml.Insert(this.parent.currentDocument, imgElement, this.parent.editableElement);
            }
        }
        if (e.callBack && (sf.base.isNullOrUndefined(e.selector) || !sf.base.isNullOrUndefined(e.selector) && e.selector !== 'pasteCleanupModule')) {
            var selectedNode = this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)[0];
            var imgElm_1 = (e.value === 'Replace' || isReplaced) ? e.item.selectParent[0] :
                (sf.base.Browser.isIE ? selectedNode.previousSibling : selectedNode.previousElementSibling);
            imgElm_1.addEventListener('load', function () {
                if (e.value !== 'Replace' || !isReplaced) {
                    e.callBack({
                        requestType: 'Images',
                        editorMode: 'HTML',
                        event: e.event,
                        range: _this.parent.nodeSelection.getRange(_this.parent.currentDocument),
                        elements: [imgElm_1]
                    });
                }
            });
        }
    };
    ImageCommand.prototype.setStyle = function (imgElement, e) {
        if (!sf.base.isNullOrUndefined(e.item.url)) {
            imgElement.setAttribute('src', e.item.url);
        }
        imgElement.setAttribute('class', 'e-rte-image' + (sf.base.isNullOrUndefined(e.item.cssClass) ? '' : ' ' + e.item.cssClass));
        if (!sf.base.isNullOrUndefined(e.item.altText)) {
            imgElement.setAttribute('alt', e.item.altText);
        }
        if (!sf.base.isNullOrUndefined(e.item.width) && !sf.base.isNullOrUndefined(e.item.width.width)) {
            imgElement.setAttribute('width', this.calculateStyleValue(e.item.width.width));
        }
        if (!sf.base.isNullOrUndefined(e.item.height) && !sf.base.isNullOrUndefined(e.item.height.height)) {
            imgElement.setAttribute('height', this.calculateStyleValue(e.item.height.height));
        }
        if (!sf.base.isNullOrUndefined(e.item.width) && !sf.base.isNullOrUndefined(e.item.width.minWidth)) {
            imgElement.style.minWidth = this.calculateStyleValue(e.item.width.minWidth);
        }
        if (!sf.base.isNullOrUndefined(e.item.width) && !sf.base.isNullOrUndefined(e.item.width.maxWidth)) {
            imgElement.style.maxWidth = this.calculateStyleValue(e.item.width.maxWidth);
        }
        if (!sf.base.isNullOrUndefined(e.item.height) && !sf.base.isNullOrUndefined(e.item.height.minHeight)) {
            imgElement.style.minHeight = this.calculateStyleValue(e.item.height.minHeight);
        }
        if (!sf.base.isNullOrUndefined(e.item.height) && !sf.base.isNullOrUndefined(e.item.height.maxHeight)) {
            imgElement.style.maxHeight = this.calculateStyleValue(e.item.height.maxHeight);
        }
    };
    ImageCommand.prototype.calculateStyleValue = function (value) {
        var styleValue;
        if (typeof (value) === 'string') {
            if (value.indexOf('px') || value.indexOf('%') || value.indexOf('auto')) {
                styleValue = value;
            }
            else {
                styleValue = value + 'px';
            }
        }
        else {
            styleValue = value + 'px';
        }
        return styleValue;
    };
    ImageCommand.prototype.insertImageLink = function (e) {
        var anchor = sf.base.createElement('a', {
            attrs: {
                href: e.item.url
            }
        });
        if (e.item.selectNode[0].parentElement.classList.contains('e-img-wrap')) {
            e.item.selection.restore();
            anchor.setAttribute('contenteditable', 'true');
        }
        anchor.appendChild(e.item.selectNode[0]);
        if (!sf.base.isNullOrUndefined(e.item.target)) {
            anchor.setAttribute('target', e.item.target);
        }
        InsertHtml.Insert(this.parent.currentDocument, anchor, this.parent.editableElement);
        this.callBack(e);
    };
    ImageCommand.prototype.openImageLink = function (e) {
        document.defaultView.open(e.item.url, e.item.target);
        this.callBack(e);
    };
    ImageCommand.prototype.removeImageLink = function (e) {
        var selectParent = e.item.selectParent[0];
        if (selectParent.classList.contains('e-img-caption')) {
            var capImgWrap = sf.base.select('.e-img-wrap', selectParent);
            var textEle = sf.base.select('.e-img-inner', selectParent);
            var newTextEle = textEle.cloneNode(true);
            sf.base.detach(sf.base.select('a', selectParent));
            sf.base.detach(textEle);
            capImgWrap.appendChild(e.item.insertElement);
            capImgWrap.appendChild(newTextEle);
        }
        else {
            sf.base.detach(selectParent);
            if (sf.base.Browser.isIE) {
                e.item.selection.restore();
            }
            InsertHtml.Insert(this.parent.currentDocument, e.item.insertElement, this.parent.editableElement);
        }
        this.callBack(e);
    };
    ImageCommand.prototype.editImageLink = function (e) {
        e.item.selectNode[0].parentElement.href = e.item.url;
        if (sf.base.isNullOrUndefined(e.item.target)) {
            e.item.selectNode[0].parentElement.removeAttribute('target');
        }
        else {
            e.item.selectNode[0].parentElement.target = e.item.target;
        }
        this.callBack(e);
    };
    ImageCommand.prototype.removeImage = function (e) {
        if (sf.base.closest(e.item.selectNode[0], 'a')) {
            if (e.item.selectNode[0].parentElement.nodeName === 'A' && !sf.base.isNullOrUndefined(e.item.selectNode[0].parentElement.innerText)) {
                sf.base.detach(e.item.selectNode[0]);
            }
            else {
                sf.base.detach(sf.base.closest(e.item.selectNode[0], 'a'));
            }
        }
        else if (!sf.base.isNullOrUndefined(sf.base.closest(e.item.selectNode[0], '.' + CLASS_CAPTION))) {
            sf.base.detach(sf.base.closest(e.item.selectNode[0], '.' + CLASS_CAPTION));
        }
        else {
            sf.base.detach(e.item.selectNode[0]);
        }
        this.callBack(e);
    };
    ImageCommand.prototype.insertAltTextImage = function (e) {
        e.item.selectNode[0].setAttribute('alt', e.item.altText);
        this.callBack(e);
    };
    ImageCommand.prototype.imageDimension = function (e) {
        var selectNode = e.item.selectNode[0];
        selectNode.style.height = '';
        selectNode.style.width = '';
        if (e.item.width !== 'auto') {
            selectNode.style.width = sf.base.formatUnit(e.item.width);
        }
        else {
            selectNode.removeAttribute('width');
        }
        if (e.item.height !== 'auto') {
            selectNode.style.height = sf.base.formatUnit(e.item.height);
        }
        else {
            selectNode.removeAttribute('height');
        }
        this.callBack(e);
    };
    ImageCommand.prototype.imageCaption = function (e) {
        InsertHtml.Insert(this.parent.currentDocument, e.item.insertElement, this.parent.editableElement);
        this.callBack(e);
    };
    ImageCommand.prototype.imageJustifyLeft = function (e) {
        var selectNode = e.item.selectNode[0];
        if (!sf.base.isNullOrUndefined(selectNode)) {
            selectNode.removeAttribute('class');
            sf.base.addClass([selectNode], 'e-rte-image');
            if (!sf.base.isNullOrUndefined(sf.base.closest(selectNode, '.' + CLASS_CAPTION))) {
                sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_RIGHT);
                sf.base.addClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_LEFT);
            }
            if (selectNode.parentElement.nodeName === 'A') {
                sf.base.removeClass([selectNode.parentElement], CLASS_IMAGE_RIGHT);
                sf.base.addClass([selectNode.parentElement], CLASS_IMAGE_LEFT);
                sf.base.addClass([selectNode], CLASS_IMAGE_LEFT);
            }
            else {
                sf.base.addClass([selectNode], CLASS_IMAGE_LEFT);
            }
            this.callBack(e);
        }
    };
    ImageCommand.prototype.imageJustifyCenter = function (e) {
        var selectNode = e.item.selectNode[0];
        if (!sf.base.isNullOrUndefined(selectNode)) {
            selectNode.removeAttribute('class');
            sf.base.addClass([selectNode], 'e-rte-image');
            if (!sf.base.isNullOrUndefined(sf.base.closest(selectNode, '.' + CLASS_CAPTION))) {
                sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_LEFT);
                sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_RIGHT);
                sf.base.addClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_CENTER);
            }
            if (selectNode.parentElement.nodeName === 'A') {
                sf.base.removeClass([selectNode.parentElement], CLASS_IMAGE_LEFT);
                sf.base.removeClass([selectNode.parentElement], CLASS_IMAGE_RIGHT);
                sf.base.addClass([selectNode.parentElement], CLASS_IMAGE_CENTER);
                sf.base.addClass([selectNode], CLASS_IMAGE_CENTER);
            }
            else {
                sf.base.addClass([selectNode], CLASS_IMAGE_CENTER);
            }
            this.callBack(e);
        }
    };
    ImageCommand.prototype.imageJustifyRight = function (e) {
        var selectNode = e.item.selectNode[0];
        if (!sf.base.isNullOrUndefined(selectNode)) {
            selectNode.removeAttribute('class');
            sf.base.addClass([selectNode], 'e-rte-image');
            if (!sf.base.isNullOrUndefined(sf.base.closest(selectNode, '.' + CLASS_CAPTION))) {
                sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_LEFT);
                sf.base.addClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_RIGHT);
            }
            if (selectNode.parentElement.nodeName === 'A') {
                sf.base.removeClass([selectNode.parentElement], CLASS_IMAGE_LEFT);
                sf.base.addClass([selectNode.parentElement], CLASS_IMAGE_RIGHT);
                sf.base.addClass([selectNode], CLASS_IMAGE_RIGHT);
            }
            else {
                sf.base.addClass([selectNode], CLASS_IMAGE_RIGHT);
            }
            this.callBack(e);
        }
    };
    ImageCommand.prototype.imageInline = function (e) {
        var selectNode = e.item.selectNode[0];
        selectNode.removeAttribute('class');
        sf.base.addClass([selectNode], 'e-rte-image');
        sf.base.addClass([selectNode], CLASS_IMAGE_INLINE);
        if (!sf.base.isNullOrUndefined(sf.base.closest(selectNode, '.' + CLASS_CAPTION))) {
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_BREAK);
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_CENTER);
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_LEFT);
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_RIGHT);
            sf.base.addClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_CAPTION_INLINE);
        }
        this.callBack(e);
    };
    ImageCommand.prototype.imageBreak = function (e) {
        var selectNode = e.item.selectNode[0];
        selectNode.removeAttribute('class');
        sf.base.addClass([selectNode], CLASS_IMAGE_BREAK);
        sf.base.addClass([selectNode], 'e-rte-image');
        if (!sf.base.isNullOrUndefined(sf.base.closest(selectNode, '.' + CLASS_CAPTION))) {
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_CAPTION_INLINE);
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_CENTER);
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_LEFT);
            sf.base.removeClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_RIGHT);
            sf.base.addClass([sf.base.closest(selectNode, '.' + CLASS_CAPTION)], CLASS_IMAGE_BREAK);
        }
        this.callBack(e);
    };
    ImageCommand.prototype.callBack = function (e) {
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    return ImageCommand;
}());

/**
 * Link internal component
 *
 * @hidden
 * @deprecated
 */
var TableCommand = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function TableCommand(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    TableCommand.prototype.addEventListener = function () {
        this.parent.observer.on(TABLE, this.createTable, this);
        this.parent.observer.on(INSERT_ROW, this.insertRow, this);
        this.parent.observer.on(INSERT_COLUMN, this.insertColumn, this);
        this.parent.observer.on(DELETEROW, this.deleteRow, this);
        this.parent.observer.on(DELETECOLUMN, this.deleteColumn, this);
        this.parent.observer.on(REMOVETABLE, this.removeTable, this);
        this.parent.observer.on(TABLEHEADER, this.tableHeader, this);
        this.parent.observer.on(TABLE_VERTICAL_ALIGN, this.tableVerticalAlign, this);
        this.parent.observer.on(TABLE_MERGE, this.cellMerge, this);
        this.parent.observer.on(TABLE_HORIZONTAL_SPLIT, this.HorizontalSplit, this);
        this.parent.observer.on(TABLE_VERTICAL_SPLIT, this.VerticalSplit, this);
        this.parent.observer.on(TABLE_MOVE, this.tableMove, this);
    };
    TableCommand.prototype.createTable = function (e) {
        var table = sf.base.createElement('table', { className: 'e-rte-table' });
        var tblBody = sf.base.createElement('tbody');
        if (!sf.base.isNullOrUndefined(e.item.width.width)) {
            table.style.width = this.calculateStyleValue(e.item.width.width);
        }
        if (!sf.base.isNullOrUndefined(e.item.width.minWidth)) {
            table.style.minWidth = this.calculateStyleValue(e.item.width.minWidth);
        }
        if (!sf.base.isNullOrUndefined(e.item.width.maxWidth)) {
            table.style.maxWidth = this.calculateStyleValue(e.item.width.maxWidth);
        }
        var tdWid = parseInt(e.item.width.width, 10) > 100 ?
            100 / e.item.columns : parseInt(e.item.width.width, 10) / e.item.columns;
        for (var i = 0; i < e.item.rows; i++) {
            var row = sf.base.createElement('tr');
            for (var j = 0; j < e.item.columns; j++) {
                var cell = sf.base.createElement('td');
                cell.appendChild(sf.base.createElement('br'));
                cell.style.width = tdWid + '%';
                row.appendChild(cell);
            }
            tblBody.appendChild(row);
        }
        table.appendChild(tblBody);
        e.item.selection.restore();
        InsertHtml.Insert(this.parent.currentDocument, table, this.parent.editableElement);
        this.removeEmptyNode();
        e.item.selection.setSelectionText(this.parent.currentDocument, table.querySelector('td'), table.querySelector('td'), 0, 0);
        if (table.nextElementSibling === null) {
            var insertElem = void 0;
            if (e.enterAction === 'DIV') {
                insertElem = sf.base.createElement('div');
                insertElem.appendChild(sf.base.createElement('br'));
            }
            else if (e.enterAction === 'BR') {
                insertElem = sf.base.createElement('br');
            }
            else {
                insertElem = sf.base.createElement('p');
                insertElem.appendChild(sf.base.createElement('br'));
            }
            this.insertAfter(insertElem, table);
        }
        table.querySelector('td').classList.add('e-cell-select');
        if (e.callBack) {
            e.callBack({
                requestType: 'Table',
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: [table]
            });
        }
        return table;
    };
    TableCommand.prototype.calculateStyleValue = function (value) {
        var styleValue;
        if (typeof (value) === 'string') {
            if (value.indexOf('px') || value.indexOf('%') || value.indexOf('auto')) {
                styleValue = value;
            }
            else {
                styleValue = value + 'px';
            }
        }
        else {
            styleValue = value + 'px';
        }
        return styleValue;
    };
    TableCommand.prototype.removeEmptyNode = function () {
        var emptyUl = this.parent.editableElement.querySelectorAll('ul:empty, ol:empty');
        for (var i = 0; i < emptyUl.length; i++) {
            sf.base.detach(emptyUl[i]);
        }
        var emptyLiChild = this.parent.editableElement.querySelectorAll('li *:empty');
        for (var i = 0; i < emptyLiChild.length; i++) {
            sf.base.detach(emptyLiChild[i]);
            if (emptyLiChild.length === i + 1) {
                emptyLiChild = this.parent.editableElement.querySelectorAll('li *:empty');
                i = -1;
            }
        }
        var emptyLi = this.parent.editableElement.querySelectorAll('li:empty');
        for (var i = 0; i < emptyLi.length; i++) {
            sf.base.detach(emptyLi[i]);
        }
    };
    TableCommand.prototype.insertAfter = function (newNode, referenceNode) {
        referenceNode.parentNode.insertBefore(newNode, referenceNode.nextSibling);
    };
    TableCommand.prototype.getSelectedCellMinMaxIndex = function (e) {
        var selectedCells = this.curTable.querySelectorAll('.e-cell-select');
        var a = 0;
        var minI = e.length;
        var maxI = 0;
        var minJ = e[0].length;
        var maxJ = 0;
        //eslint-disable-next-line
        for (; a < selectedCells.length; a++) {
            var selectedCellIndex = this.getCorrespondingIndex(selectedCells[a], e);
            var minMaxIndex = this.FindIndex(selectedCellIndex[0], selectedCellIndex[1], e);
            //eslint-disable-next-line
            minI = Math.min(selectedCellIndex[0], minI), maxI = Math.max(minMaxIndex[0], maxI), minJ = Math.min(selectedCellIndex[1], minJ), maxJ = Math.max(minMaxIndex[1], maxJ);
        }
        return {
            startRow: minI,
            endRow: maxI,
            startColumn: minJ,
            endColumn: maxJ
        };
    };
    TableCommand.prototype.insertRow = function (e) {
        var isBelow = e.item.subCommand === 'InsertRowBefore' ? false : true;
        var selectedCell = e.item.selection.range.startContainer;
        if (!(selectedCell.nodeName === 'TH' || selectedCell.nodeName === 'TD')) {
            selectedCell = sf.base.closest(selectedCell.parentElement, 'td,th');
        }
        if (selectedCell.nodeName.toLowerCase() === 'th' && e.item.subCommand === 'InsertRowBefore') {
            return;
        }
        this.curTable = sf.base.closest(this.parent.nodeSelection.range.startContainer.parentElement, 'table');
        if (this.curTable.querySelectorAll('.e-cell-select').length === 0) {
            var lastRow = this.curTable.rows[this.curTable.rows.length - 1];
            var cloneRow = lastRow.cloneNode(true);
            cloneRow.removeAttribute('rowspan');
            this.insertAfter(cloneRow, lastRow);
        }
        else {
            var allCells = this.getCorrespondingColumns();
            var minMaxIndex = this.getSelectedCellMinMaxIndex(allCells);
            var minVal = isBelow ? minMaxIndex.endRow : minMaxIndex.startRow;
            var newRow = sf.base.createElement('tr');
            var isHeaderSelect = this.curTable.querySelectorAll('th.e-cell-select').length > 0;
            for (var i = 0; i < allCells[minVal].length; i++) {
                if (isBelow && minVal < allCells.length - 1 && allCells[minVal][i] === allCells[minVal + 1][i] ||
                    !isBelow && 0 < minVal && allCells[minVal][i] === allCells[minVal - 1][i]) {
                    if (0 === i || 0 < i && allCells[minVal][i] !== allCells[minVal][i - 1]) {
                        allCells[minVal][i].setAttribute('rowspan', (parseInt(allCells[minVal][i].getAttribute('rowspan'), 10) + 1).toString());
                    }
                }
                else {
                    var tdElement = sf.base.createElement('td');
                    tdElement.appendChild(sf.base.createElement('br'));
                    newRow.appendChild(tdElement);
                    tdElement.setAttribute('style', allCells[(isHeaderSelect && isBelow) ? (minVal + 1) : minVal][i].getAttribute('style'));
                }
            }
            // eslint-disable-next-line
            var selectedRow = void 0;
            if (isHeaderSelect && isBelow) {
                selectedRow = this.curTable.querySelector('tbody').childNodes[0];
            }
            else {
                selectedRow = this.curTable.rows[minVal];
            }
            // eslint-disable-next-line
            (e.item.subCommand === 'InsertRowBefore') ? selectedRow.parentElement.insertBefore(newRow, selectedRow) :
                (isHeaderSelect ? selectedRow.parentElement.insertBefore(newRow, selectedRow) :
                    this.insertAfter(newRow, selectedRow));
        }
        e.item.selection.setSelectionText(this.parent.currentDocument, e.item.selection.range.startContainer, e.item.selection.range.startContainer, 0, 0);
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.insertColumn = function (e) {
        var selectedCell = e.item.selection.range.startContainer;
        if (!(selectedCell.nodeName === 'TH' || selectedCell.nodeName === 'TD')) {
            selectedCell = sf.base.closest(selectedCell.parentElement, 'td,th');
        }
        var curRow = sf.base.closest(selectedCell, 'tr');
        var curCell;
        var allRows = sf.base.closest((curRow), 'table').rows;
        var colIndex = Array.prototype.slice.call(curRow.querySelectorAll(':scope > td, :scope > th')).indexOf(selectedCell);
        var previousWidth = parseInt(e.item.width, 10) / (curRow.querySelectorAll(':scope > td, :scope > th').length);
        var currentWidth = parseInt(e.item.width, 10) / (curRow.querySelectorAll(':scope > td, :scope > th').length + 1);
        var currentTabElm = sf.base.closest(curRow, 'table');
        var thTdElm = sf.base.closest(curRow, 'table').querySelectorAll('th,td');
        for (var i = 0; i < thTdElm.length; i++) {
            thTdElm[i].dataset.oldWidth = (thTdElm[i].offsetWidth / currentTabElm.offsetWidth * 100) + '%';
        }
        for (var i = 0; i < allRows.length; i++) {
            curCell = allRows[i].querySelectorAll(':scope > td, :scope > th')[colIndex];
            var colTemplate = curCell.cloneNode(true);
            colTemplate.innerHTML = '';
            colTemplate.appendChild(sf.base.createElement('br'));
            colTemplate.removeAttribute('class');
            colTemplate.removeAttribute('colspan');
            colTemplate.removeAttribute('rowspan');
            // eslint-disable-next-line
            (e.item.subCommand === 'InsertColumnLeft') ? curCell.parentElement.insertBefore(colTemplate, curCell) :
                this.insertAfter(colTemplate, curCell);
            colTemplate.style.width = currentWidth.toFixed(4) + '%';
            delete colTemplate.dataset.oldWidth;
        }
        for (var i = 0; i < thTdElm.length; i++) {
            thTdElm[i].style.width = (Number(thTdElm[i].dataset.oldWidth.split('%')[0]) * currentWidth / previousWidth).toFixed(4) + '%';
            delete thTdElm[i].dataset.oldWidth;
        }
        e.item.selection.setSelectionText(this.parent.currentDocument, selectedCell, selectedCell, 0, 0);
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.deleteColumn = function (e) {
        var selectedCell = e.item.selection.range.startContainer;
        if (selectedCell.nodeType === 3) {
            selectedCell = sf.base.closest(selectedCell.parentElement, 'td,th');
        }
        var tBodyHeadEle = sf.base.closest(selectedCell, selectedCell.tagName === 'TH' ? 'thead' : 'tbody');
        var rowIndex = tBodyHeadEle && Array.prototype.indexOf.call(tBodyHeadEle.childNodes, selectedCell.parentNode);
        this.curTable = sf.base.closest(selectedCell, 'table');
        var curRow = sf.base.closest(selectedCell, 'tr');
        if (curRow.querySelectorAll('th,td').length === 1) {
            e.item.selection.restore();
            sf.base.detach(sf.base.closest(selectedCell.parentElement, 'table'));
        }
        else {
            var deleteIndex = void 0;
            var allCells = this.getCorrespondingColumns();
            //eslint-disable-next-line
            var selectedMinMaxIndex = this.getSelectedCellMinMaxIndex(allCells);
            var minCol = selectedMinMaxIndex.startColumn;
            var maxCol = selectedMinMaxIndex.endColumn;
            for (var i = 0; i < allCells.length; i++) {
                var currentRow = allCells[i];
                for (var j = 0; j < currentRow.length; j++) {
                    var currentCell = currentRow[j];
                    //eslint-disable-next-line
                    var currentCellIndex = this.getCorrespondingIndex(currentCell, allCells);
                    var colSpanVal = parseInt(currentCell.getAttribute('colspan'), 10) || 1;
                    if (currentCellIndex[1] + (colSpanVal - 1) >= minCol && currentCellIndex[1] <= maxCol) {
                        if (colSpanVal > 1) {
                            currentCell.setAttribute('colspan', (colSpanVal - 1).toString());
                        }
                        else {
                            sf.base.detach(currentCell);
                            deleteIndex = j;
                            if (sf.base.Browser.isIE) {
                                e.item.selection.setSelectionText(this.parent.currentDocument, this.curTable.querySelector('td'), this.curTable.querySelector('td'), 0, 0);
                                this.curTable.querySelector('td, th').classList.add('e-cell-select');
                            }
                        }
                    }
                }
            }
            if (deleteIndex > -1) {
                var rowHeadEle = tBodyHeadEle.children[rowIndex];
                var nextFocusCell = rowHeadEle &&
                    rowHeadEle.children[(deleteIndex <= rowHeadEle.children.length - 1 ? deleteIndex : deleteIndex - 1)];
                if (nextFocusCell) {
                    e.item.selection.setSelectionText(this.parent.currentDocument, nextFocusCell, nextFocusCell, 0, 0);
                    nextFocusCell.classList.add('e-cell-select');
                }
            }
        }
        if (e.callBack) {
            var sContainer = this.parent.nodeSelection.getRange(this.parent.currentDocument).startContainer;
            if (sContainer.nodeName !== 'TD') {
                var startChildLength = this.parent.nodeSelection.getRange(this.parent.currentDocument).startOffset;
                var focusNode = sContainer.children[startChildLength];
                if (focusNode) {
                    this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, focusNode, 0);
                }
            }
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.deleteRow = function (e) {
        var selectedCell = e.item.selection.range.startContainer;
        if (selectedCell.nodeType === 3) {
            selectedCell = sf.base.closest(selectedCell.parentElement, 'td,th');
        }
        var colIndex = Array.prototype.indexOf.call(selectedCell.parentNode.childNodes, selectedCell);
        this.curTable = sf.base.closest(selectedCell, 'table');
        var currentRow;
        var allCells = this.getCorrespondingColumns();
        var minMaxIndex = this.getSelectedCellMinMaxIndex(allCells);
        var maxI;
        var j;
        if (this.curTable.rows.length === 1) {
            e.item.selection.restore();
            sf.base.detach(sf.base.closest(selectedCell.parentElement, 'table'));
        }
        else {
            for (maxI = minMaxIndex.endRow; maxI >= minMaxIndex.startRow; maxI--) {
                currentRow = this.curTable.rows[maxI];
                for (j = 0; j < allCells[maxI].length; j++) {
                    if (j === 0 || allCells[maxI][j] !== allCells[maxI][j - 1]) {
                        if (1 < parseInt(allCells[maxI][j].getAttribute('rowspan'), 10)) {
                            var rowSpanVal = parseInt(allCells[maxI][j].getAttribute('rowspan'), 10) - 1;
                            //eslint-disable-next-line
                            1 === rowSpanVal ? allCells[maxI][j].removeAttribute('rowspan') : allCells[maxI][j].setAttribute('rowspan', rowSpanVal.toString());
                        }
                    }
                    if (maxI < allCells.length - 1 && allCells[maxI][j] === allCells[maxI + 1][j] && (0 === maxI ||
                        allCells[maxI][j] !== allCells[maxI - 1][j])) {
                        var element = allCells[maxI][j];
                        var index = void 0;
                        for (index = j; 0 < index && allCells[maxI][index] === allCells[maxI][index - 1]; index--) {
                            if (index === 0) {
                                this.curTable.rows[maxI + 1].prepend(element);
                            }
                            else {
                                allCells[maxI + 1][index - 1].insertAdjacentElement('afterend', element);
                            }
                        }
                    }
                }
                var deleteIndex = currentRow.rowIndex;
                this.curTable.deleteRow(deleteIndex);
                var focusTrEle = !sf.base.isNullOrUndefined(this.curTable.rows[deleteIndex]) ? this.curTable.querySelectorAll('tbody tr')[deleteIndex]
                    : this.curTable.querySelectorAll('tbody tr')[deleteIndex - 1];
                var nextFocusCell = focusTrEle && focusTrEle.querySelectorAll('td')[colIndex];
                if (nextFocusCell) {
                    e.item.selection.setSelectionText(this.parent.currentDocument, nextFocusCell, nextFocusCell, 0, 0);
                    nextFocusCell.classList.add('e-cell-select');
                }
                else {
                    e.item.selection.setSelectionText(this.parent.currentDocument, this.curTable.querySelector('td'), this.curTable.querySelector('td'), 0, 0);
                    this.curTable.querySelector('td, th').classList.add('e-cell-select');
                }
            }
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.removeTable = function (e) {
        var selectedCell = e.item.selection.range.startContainer;
        selectedCell = (selectedCell.nodeType === 3) ? selectedCell.parentNode : selectedCell;
        var selectedTable = sf.base.closest(selectedCell.parentElement, 'table');
        if (selectedTable) {
            e.item.selection.restore();
            sf.base.detach(selectedTable);
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.tableHeader = function (e) {
        var headerExit = false;
        var selectedCell = e.item.selection.range.startContainer;
        selectedCell = (selectedCell.nodeType === 3) ? selectedCell.parentNode : selectedCell;
        var table = sf.base.closest(selectedCell.parentElement, 'table');
        [].slice.call(table.childNodes).forEach(function (el) {
            if (el.nodeName === 'THEAD') {
                headerExit = true;
            }
        });
        if (table && !headerExit) {
            var cellCount = table.querySelector('tr').childElementCount;
            var colSpanCount = 0;
            for (var i = 0; i < cellCount; i++) {
                colSpanCount = colSpanCount + (parseInt(table.querySelector('tr').children[i].getAttribute('colspan'), 10) || 1);
            }
            var header = table.createTHead();
            var row = header.insertRow(0);
            for (var j = 0; j < colSpanCount; j++) {
                var th = sf.base.createElement('th');
                th.appendChild(sf.base.createElement('br'));
                row.appendChild(th);
            }
        }
        else {
            table.deleteTHead();
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.tableVerticalAlign = function (e) {
        if (e.item.subCommand === 'AlignTop') {
            e.item.tableCell.style.verticalAlign = 'top';
        }
        else if (e.item.subCommand === 'AlignMiddle') {
            e.item.tableCell.style.verticalAlign = 'middle';
        }
        else {
            e.item.tableCell.style.verticalAlign = 'bottom';
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.cellMerge = function (e) {
        if (sf.base.isNullOrUndefined(this.curTable)) {
            this.curTable = sf.base.closest(this.parent.nodeSelection.range.startContainer.parentElement, 'table');
        }
        var selectedCells = this.curTable.querySelectorAll('.e-cell-select');
        if (selectedCells.length < 2) {
            return;
        }
        this.mergeCellContent();
        var minMaxIndexes = this.getSelectedMinMaxIndexes(this.getCorrespondingColumns());
        var firstCell = selectedCells[0];
        var rowSelectedCells = firstCell.parentElement.querySelectorAll('.e-cell-select');
        if (minMaxIndexes.startColumn < minMaxIndexes.endColumn) {
            firstCell.setAttribute('colspan', (minMaxIndexes.endColumn - minMaxIndexes.startColumn + 1).toString());
        }
        if (minMaxIndexes.startRow < minMaxIndexes.endRow) {
            firstCell.setAttribute('rowspan', (minMaxIndexes.endRow - minMaxIndexes.startRow + 1).toString());
        }
        var totalWidth = 0;
        for (var j = rowSelectedCells.length - 1; j >= 0; j--) {
            totalWidth = totalWidth + parseFloat(rowSelectedCells[j].style.width);
        }
        firstCell.style.width = totalWidth + '%';
        for (var i = 1; i <= selectedCells.length - 1; i++) {
            sf.base.detach(selectedCells[i]);
        }
        for (var i = 0; i < this.curTable.rows.length; i++) {
            if (this.curTable.rows[i].innerHTML === '') {
                sf.base.detach(this.curTable.rows[i]);
            }
        }
        this.updateRowSpanStyle(minMaxIndexes.startRow, minMaxIndexes.endRow, this.getCorrespondingColumns());
        this.updateColSpanStyle(minMaxIndexes.startColumn, minMaxIndexes.endColumn, this.getCorrespondingColumns());
        e.item.selection.setSelectionText(this.parent.currentDocument, e.item.selection.range.startContainer, e.item.selection.range.startContainer, 0, 0);
        if (this.parent.nodeSelection.range) {
            this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, 
            // eslint-disable-next-line
            this.parent.nodeSelection.range.endContainer, 0);
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.updateColSpanStyle = function (min, max, elements) {
        var colValue;
        var colIndex;
        var colMin;
        var index = 0;
        var attrValue;
        var count = 0;
        var eleArray = elements;
        //eslint-disable-next-line
        if (min < (max = Math.min(max, eleArray[0].length - 1))) {
            for (colIndex === min; colIndex <= max; colIndex++) {
                // eslint-disable-next-line
                if (!(min < colIndex && eleArray[0][colIndex] === eleArray[0][colIndex - 1]) && 1 < (index =
                    Math.min(parseInt(eleArray[0][colIndex].getAttribute('colspan'), 10) || 1, max - min + 1)) &&
                    eleArray[0][colIndex] === eleArray[0][colIndex + 1]) {
                    for (count = index - 1, colValue = 1; colValue < eleArray.length; colValue++) {
                        if (eleArray[colValue][colIndex] !== eleArray[colValue - 1][colIndex]) {
                            /* eslint-disable */
                            for (colMin = colIndex; colMin < colIndex + index; colMin++) {
                                if (1 < (attrValue = parseInt(eleArray[colValue][colMin].getAttribute('colspan'), 10) || 1) &&
                                    eleArray[colValue][colMin] === eleArray[colValue][colMin + 1]) {
                                    colMin += count = Math.min(count, attrValue - 1);
                                }
                                else if (!(count = Math.max(0, count - 1))) {
                                    break;
                                }
                                /* eslint-enable */
                            }
                        }
                        if (!count) {
                            break;
                        }
                    }
                }
            }
            if (count) {
                this.updateCellAttribute(eleArray, count, 'colspan', 0, eleArray.length - 1, min, max);
            }
        }
    };
    TableCommand.prototype.updateRowSpanStyle = function (min, max, ele) {
        var rowValue;
        var colIndex;
        var rowMin;
        var index = 0;
        var attrValue;
        var count = 0;
        var eleArray = ele;
        // eslint-disable-next-line
        if (min < (max = Math.min(max, eleArray.length - 1))) {
            for (rowValue = min; rowValue <= max; rowValue++) {
                // eslint-disable-next-line
                if (!(min < rowValue && eleArray[rowValue][0] === eleArray[rowValue - 1][0]) && 1 < (index = Math.min(parseInt(eleArray[rowValue][0].getAttribute('rowspan'), 10) || 1, max - min + 1)) && eleArray[rowValue][0] === eleArray[rowValue + 1][0]) {
                    for (count = index - 1, colIndex = 1; colIndex < eleArray[0].length; colIndex++) {
                        if (eleArray[rowValue][colIndex] !== eleArray[rowValue][colIndex - 1]) {
                            for (rowMin = rowValue; rowMin < rowValue + index; rowMin++) {
                                // eslint-disable-next-line
                                if (1 < (attrValue = parseInt(eleArray[rowMin][colIndex].getAttribute('rowspan'), 10) || 1) && eleArray[rowMin][colIndex] === eleArray[rowMin + 1][colIndex]) {
                                    rowMin += count = Math.min(count, attrValue - 1);
                                }
                                // eslint-disable-next-line
                                else if (!(count = Math.max(0, count - 1))) {
                                    break;
                                }
                            }
                            if (!count) {
                                break;
                            }
                        }
                    }
                }
            }
            if (count) {
                this.updateCellAttribute(eleArray, count, 'rowspan', min, max, 0, eleArray[0].length - 1);
            }
        }
    };
    TableCommand.prototype.updateCellAttribute = function (elements, index, attr, min, max, firstIndex, length) {
        var rowIndex;
        var colIndex;
        var spanCount;
        for (rowIndex = min; rowIndex <= max; rowIndex++) {
            for (colIndex = firstIndex; colIndex <= length; colIndex++) {
                // eslint-disable-next-line
                min < rowIndex && elements[rowIndex][colIndex] === elements[rowIndex - 1][colIndex] ||
                    firstIndex < colIndex && elements[rowIndex][colIndex] === elements[rowIndex][colIndex - 1] ||
                    1 < (spanCount = parseInt(elements[rowIndex][colIndex].getAttribute(attr), 10) || 1) &&
                        (1 < spanCount - index ? elements[rowIndex][colIndex].setAttribute(attr, (spanCount - index).toString()) :
                            elements[rowIndex][colIndex].removeAttribute(attr));
            }
        }
    };
    TableCommand.prototype.mergeCellContent = function () {
        var selectedCells = this.curTable.querySelectorAll('.e-cell-select');
        var innerHtml = selectedCells[0].innerHTML;
        for (var i = 1; i < selectedCells.length - 1; i++) {
            if ('<br>' !== selectedCells[i].innerHTML) {
                innerHtml = innerHtml + selectedCells[i].innerHTML;
            }
        }
        selectedCells[0].innerHTML = innerHtml;
    };
    TableCommand.prototype.getSelectedMinMaxIndexes = function (correspondingCells) {
        var selectedCells = this.curTable.querySelectorAll('.e-cell-select');
        if (0 < selectedCells.length) {
            var minRow = correspondingCells.length;
            var maxRow = 0;
            var minCol = correspondingCells[0].length;
            var maxCol = 0;
            for (var i = 0; i < selectedCells.length; i++) {
                var currentRowCol = this.getCorrespondingIndex(selectedCells[i], correspondingCells);
                var targetRowCol = this.FindIndex(currentRowCol[0], currentRowCol[1], correspondingCells);
                minRow = Math.min(currentRowCol[0], minRow);
                maxRow = Math.max(targetRowCol[0], maxRow);
                minCol = Math.min(currentRowCol[1], minCol);
                maxCol = Math.max(targetRowCol[1], maxCol);
            }
            return {
                startRow: minRow,
                endRow: maxRow,
                startColumn: minCol,
                endColumn: maxCol
            };
        }
        return null;
    };
    TableCommand.prototype.HorizontalSplit = function (e) {
        var selectedCell = e.item.selection.range.startContainer;
        this.curTable = sf.base.closest(selectedCell.parentElement, 'table');
        if (this.curTable.querySelectorAll('.e-cell-select').length > 1) {
            return;
        }
        this.activeCell = this.curTable.querySelector('.e-cell-select');
        var newCell = this.activeCell.cloneNode(true);
        newCell.removeAttribute('class');
        newCell.innerHTML = '</br>';
        var activeCellIndex = this.getCorrespondingIndex(this.activeCell, this.getCorrespondingColumns());
        var correspondingCells = this.getCorrespondingColumns();
        var activeCellRowSpan = this.activeCell.getAttribute('rowspan') ? parseInt(this.activeCell.getAttribute('rowspan'), 10) : 1;
        if (activeCellRowSpan > 1) {
            var avgCount = Math.ceil(activeCellRowSpan / 2);
            // eslint-disable-next-line
            1 < avgCount ? this.activeCell.setAttribute('rowspan', avgCount.toString()) :
                this.activeCell.removeAttribute('rowspan');
            // eslint-disable-next-line
            1 < (activeCellRowSpan - avgCount) ? newCell.setAttribute('rowspan', (activeCellRowSpan - avgCount).toString()) : newCell.removeAttribute('rowspan');
            var avgRowIndex = void 0;
            var colIndex = void 0;
            for (avgRowIndex = activeCellIndex[0] + Math.ceil(activeCellRowSpan / 2), colIndex = 0 === activeCellIndex[1] ? activeCellIndex[1]
                    : activeCellIndex[1] - 1; 0 <= colIndex && (correspondingCells[avgRowIndex][colIndex] ===
                correspondingCells[avgRowIndex][colIndex - 1] || 0 < avgRowIndex && correspondingCells[avgRowIndex][colIndex]
                === correspondingCells[avgRowIndex - 1][colIndex]);) {
                colIndex--;
            }
            if (colIndex === -1) {
                // eslint-disable-next-line
                this.curTable.rows[avgRowIndex].firstChild ? this.curTable.rows[avgRowIndex].prepend(newCell) : this.curTable.appendChild(newCell);
            }
            else {
                correspondingCells[avgRowIndex][colIndex].insertAdjacentElement('afterend', newCell);
            }
        }
        else {
            var newTrEle = sf.base.createElement('tr');
            newTrEle.appendChild(newCell);
            var selectedRow = correspondingCells[activeCellIndex[0]];
            for (var j = 0; j <= selectedRow.length - 1; j++) {
                if (selectedRow[j] !== selectedRow[j - 1] && selectedRow[j] !== this.activeCell) {
                    selectedRow[j].setAttribute('rowspan', ((parseInt(selectedRow[j].getAttribute('rowspan'), 10) ?
                        parseInt(selectedRow[j].getAttribute('rowspan'), 10) : 1) + 1).toString());
                }
            }
            this.activeCell.parentNode.insertAdjacentElement('afterend', newTrEle);
        }
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.VerticalSplit = function (e) {
        var selectedCell = e.item.selection.range.startContainer;
        this.curTable = sf.base.closest(selectedCell.parentElement, 'table');
        if (this.curTable.querySelectorAll('.e-cell-select').length > 1) {
            return;
        }
        this.activeCell = this.curTable.querySelector('.e-cell-select');
        var allRows = this.curTable.rows;
        var newCell = this.activeCell.cloneNode(true);
        newCell.removeAttribute('class');
        newCell.innerHTML = '</br>';
        var avgWidth = parseFloat(this.activeCell.style.width) / 2;
        if (this.activeCell.tagName === 'TH' && isNaN(avgWidth)) {
            var cellCount = this.curTable.querySelector('tr').childElementCount;
            var colSpanCount = 0;
            for (var i = 0; i < cellCount; i++) {
                colSpanCount = colSpanCount + (parseInt(this.curTable.querySelector('tr').children[i].getAttribute('colspan'), 10) || 1);
            }
            avgWidth = parseFloat((((this.activeCell.offsetWidth / 2) / this.curTable.offsetWidth) * 100).toFixed(1));
        }
        var activeCellIndex = this.getCorrespondingIndex(this.activeCell, this.getCorrespondingColumns());
        var correspondingColumns = this.getCorrespondingColumns();
        var activeCellcolSpan = parseInt(this.activeCell.getAttribute('colspan'), 10);
        if (activeCellcolSpan > 1) {
            // eslint-disable-next-line
            1 < Math.ceil(activeCellcolSpan / 2) ? this.activeCell.setAttribute('colspan', (activeCellcolSpan / 2).toString())
                : this.activeCell.removeAttribute('colspan');
            // eslint-disable-next-line
            1 < (activeCellcolSpan - activeCellcolSpan / 2) ? newCell.setAttribute('colspan', 
            // eslint-disable-next-line
            (activeCellcolSpan - activeCellcolSpan / 2).toString()) : newCell.removeAttribute('colspan');
        }
        else {
            for (var i = 0; i <= allRows.length - 1; i++) {
                if (0 === i || correspondingColumns[i][activeCellIndex[1]] !== correspondingColumns[i - 1][activeCellIndex[1]]) {
                    var currentCell = correspondingColumns[i][activeCellIndex[1]];
                    if (currentCell !== this.activeCell) {
                        currentCell.setAttribute('colspan', ((parseInt(currentCell.getAttribute('colspan'), 10) ?
                            parseInt(currentCell.getAttribute('colspan'), 10) : 1) + 1).toString());
                    }
                }
            }
        }
        this.activeCell.style.width = avgWidth + '%';
        newCell.style.width = avgWidth + '%';
        this.activeCell.parentNode.insertBefore(newCell, this.activeCell.nextSibling);
        if (e.callBack) {
            e.callBack({
                requestType: e.item.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    TableCommand.prototype.getCorrespondingColumns = function () {
        var elementArray = [];
        // eslint-disable-next-line
        var _this = this;
        var colspan = 0;
        var allRows = _this.curTable.rows;
        for (var i = 0; i <= allRows.length - 1; i++) {
            var ele = allRows[i];
            var index = 0;
            for (var j = 0; j <= ele.children.length - 1; j++) {
                /* eslint-disable */
                var colEle = ele.children[j];
                for (var ele_1 = colEle, colspan_1 = parseInt(ele_1.getAttribute('colspan'), 10) || 1, rowSpan = parseInt(ele_1.getAttribute('rowspan'), 10) || 1, rowIndex = i; rowIndex < i + rowSpan; rowIndex++) {
                    for (var colIndex = index; colIndex < index + colspan_1; colIndex++) {
                        elementArray[rowIndex] || (elementArray[rowIndex] = []);
                        elementArray[rowIndex][colIndex] ? index++ : elementArray[rowIndex][colIndex] = colEle;
                    }
                }
                index += colspan;
            }
            /* eslint-enable */
        }
        return elementArray;
    };
    // eslint-disable-next-line
    TableCommand.prototype.FindIndex = function (rowIndex, columnIndex, cells) {
        var nextIndex;
        var nextCol;
        for (nextIndex = rowIndex + 1, nextCol = columnIndex + 1; nextIndex < cells.length;) {
            if (cells[nextIndex][columnIndex] !== cells[rowIndex][columnIndex]) {
                nextIndex--;
                break;
            }
            nextIndex++;
        }
        for (nextIndex === cells.length && nextIndex--; nextCol < cells[rowIndex].length;) {
            if (cells[rowIndex][nextCol] !== cells[rowIndex][columnIndex]) {
                nextCol--;
                break;
            }
            nextCol++;
        }
        return nextCol === cells[rowIndex].length && nextCol--, [
                nextIndex,
                nextCol
            ];
    };
    TableCommand.prototype.getCorrespondingIndex = function (cell, allCells) {
        //let value: RowCol = new RowCol();
        for (var i = 0; i < allCells.length; i++) {
            for (var j = 0; j < allCells[i].length; j++) {
                if (allCells[i][j] === cell) {
                    return [i, j];
                }
            }
        }
        return [];
    };
    TableCommand.prototype.highlightCells = function (minRow, maxRow, minCol, maxCol, eleArray) {
        var j;
        var k;
        var startCell;
        var endCell;
        var minRowIndex = minRow;
        var maxRowIndex = maxRow;
        var minColIndex = minCol;
        var maxColIndex = maxCol;
        var minMaxValues = new MinMax();
        for (j = minRowIndex; j <= maxRowIndex; j++) {
            /* eslint-disable */
            if ((1 < (parseInt(eleArray[j][minColIndex].getAttribute('rowspan'), 10) || 1) ||
                1 < (parseInt(eleArray[j][minColIndex].getAttribute('colspan'), 10) || 1)) &&
                (endCell = this.FindIndex((startCell = this.getCorrespondingIndex(eleArray[j][minColIndex], eleArray))[0], startCell[1], eleArray))) {
                minRowIndex = Math.min(startCell[0], minRowIndex);
                maxRowIndex = Math.max(endCell[0], maxRowIndex);
                minColIndex = Math.min(startCell[1], minColIndex);
                maxColIndex = Math.max(endCell[1], maxColIndex);
            }
            else if ((1 < (parseInt(eleArray[j][maxColIndex].getAttribute('rowspan'), 10) || 1) ||
                1 < (parseInt(eleArray[j][maxColIndex].getAttribute('colspan'), 10) || 1)) &&
                (endCell = this.FindIndex((startCell = this.getCorrespondingIndex(eleArray[j][maxColIndex], eleArray))[0], startCell[1], eleArray))) {
                minRowIndex = Math.min(startCell[0], minRowIndex);
                maxRowIndex = Math.max(endCell[0], maxRowIndex);
                minColIndex = Math.min(startCell[1], minColIndex);
                maxColIndex = Math.max(endCell[1], maxColIndex);
            }
            for (k = minColIndex; k <= maxColIndex; k++) {
                if ((1 < (parseInt(eleArray[minRowIndex][k].getAttribute('rowspan'), 10) || 1) ||
                    1 < (parseInt(eleArray[minRowIndex][k].getAttribute('colspan'), 10) || 1)) &&
                    (endCell = this.FindIndex((startCell = this.getCorrespondingIndex(eleArray[minRowIndex][k], eleArray))[0], startCell[1], eleArray))) {
                    minRowIndex = Math.min(startCell[0], minRowIndex);
                    maxRowIndex = Math.max(endCell[0], maxRowIndex);
                    minColIndex = Math.min(startCell[1], minColIndex);
                    maxColIndex = Math.max(endCell[1], maxColIndex);
                }
                else if ((1 < (parseInt(eleArray[maxRowIndex][k].getAttribute('rowspan'), 10) || 1) ||
                    1 < (parseInt(eleArray[maxRowIndex][k].getAttribute('colspan'), 10) || 1)) &&
                    (endCell = this.FindIndex((startCell = this.getCorrespondingIndex(eleArray[maxRowIndex][k], eleArray))[0], startCell[1], eleArray))) {
                    minRowIndex = Math.min(startCell[0], minRowIndex);
                    maxRowIndex = Math.max(endCell[0], maxRowIndex);
                    minColIndex = Math.min(startCell[1], minColIndex);
                    maxColIndex = Math.max(endCell[1], maxColIndex);
                }
            }
            minMaxValues = minRowIndex === minRow && maxRowIndex === maxRow && minColIndex === minCol && maxColIndex === maxCol ? {
                startRow: minRow,
                endRow: maxRow,
                startColumn: minCol,
                endColumn: maxCol
            } : this.highlightCells(minRowIndex, maxRowIndex, minColIndex, maxColIndex, eleArray);
        }
        return minMaxValues;
        /* eslint-enable */
    };
    TableCommand.prototype.tableMove = function (e) {
        this.activeCell = e.selectNode[0];
        var target = e.event.target;
        var activeCellTag = this.activeCell.tagName;
        var targetCellTag = target.tagName;
        this.curTable = sf.base.closest(target, 'table');
        if (this.curTable.querySelectorAll('.e-cell-select').length > 1) {
            this.parent.nodeSelection.Clear(this.parent.currentDocument);
        }
        if ((target.tagName !== 'TD' && target.tagName !== 'TH') && activeCellTag !== targetCellTag) {
            return;
        }
        var activeRowIndex = Array.prototype.slice.call((this.activeCell).parentElement.parentElement.children)
            .indexOf((this.activeCell).parentElement);
        var activeColumnIndex = Array.prototype.slice.call((this.activeCell).parentElement.children).indexOf(this.activeCell);
        var targetRowIndex = Array.prototype.slice.call(target.parentElement.parentElement.children)
            .indexOf(target.parentElement);
        var targetColumnIndex = Array.prototype.slice.call(target.parentElement.children).indexOf(target);
        var activeCellList = this.curTable.querySelectorAll('.e-cell-select');
        for (var i = activeCellList.length - 1; i >= 0; i--) {
            if (this.activeCell !== activeCellList[i]) {
                activeCellList[i].classList.remove('e-cell-select');
            }
        }
        if (activeRowIndex === targetRowIndex && activeColumnIndex === targetColumnIndex) {
            return;
        }
        var correspondingCells = this.getCorrespondingColumns();
        var activeIndexes = this.getCorrespondingIndex(this.activeCell, correspondingCells);
        var targetIndexes = this.getCorrespondingIndex(target, correspondingCells);
        var minMaxIndexes = this.highlightCells(Math.min(activeIndexes[0], targetIndexes[0]), Math.max(activeIndexes[0], 
        /* eslint-disable */
        targetIndexes[0]), Math.min(activeIndexes[1], targetIndexes[1]), Math.max(activeIndexes[1], targetIndexes[1]), correspondingCells);
        for (var rowIndex = minMaxIndexes.startRow; rowIndex <= minMaxIndexes.endRow; rowIndex++) {
            for (var colIndex = minMaxIndexes.startColumn; colIndex <= minMaxIndexes.endColumn; colIndex++) {
                correspondingCells[rowIndex][colIndex].classList.add('e-cell-select');
            }
        }
        if (this.parent.nodeSelection.range) {
            this.parent.nodeSelection.setSelectionText(this.parent.currentDocument, this.parent.nodeSelection.range.endContainer, this.parent.nodeSelection.range.endContainer, 0, 0);
            this.parent.nodeSelection.setCursorPoint(this.parent.currentDocument, this.parent.nodeSelection.range.endContainer, 0);
        }
    };
    
    return TableCommand;
}());
var MinMax = /** @class */ (function () {
    function MinMax() {
    }
    return MinMax;
}());

/**
 * `Selection` module is used to handle RTE Selections.
 */
var SelectionCommands = /** @class */ (function () {
    function SelectionCommands() {
    }
    /**
     * applyFormat method
     *
     * @param {Document} docElement - specifies the document
     * @param {string} format - specifies the string value
     * @param {Node} endNode - specifies the end node
     * @param {string} enterAction - specifies the enter key action
     * @param {string} value - specifies the string value
     * @param {string} selector - specifies the string
     * @returns {void}
     * @hidden
     * @deprecated
     */
    SelectionCommands.applyFormat = function (docElement, format, endNode, enterAction, value, selector) {
        this.enterAction = enterAction;
        var validFormats = ['bold', 'italic', 'underline', 'strikethrough', 'superscript',
            'subscript', 'uppercase', 'lowercase', 'fontcolor', 'fontname', 'fontsize', 'backgroundcolor'];
        if (validFormats.indexOf(format) > -1) {
            if (format === 'backgroundcolor' && value === '') {
                value = 'transparent';
            }
            var domSelection = new NodeSelection();
            var domNode = new DOMNode(endNode, docElement);
            var nodeCutter = new NodeCutter();
            var isFormatted = new IsFormatted();
            var range = domSelection.getRange(docElement);
            var save = domSelection.save(range, docElement);
            var nodes = range.collapsed ? domSelection.getSelectionNodeCollection(range) :
                domSelection.getSelectionNodeCollectionBr(range);
            var isCollapsed = false;
            var isFormat = false;
            var isCursor = false;
            var preventRestore = false;
            var isFontStyle = (['fontcolor', 'fontname', 'fontsize', 'backgroundcolor'].indexOf(format) > -1);
            if (range.collapsed) {
                if (nodes.length > 0) {
                    isCollapsed = true;
                    range = nodeCutter.GetCursorRange(docElement, range, nodes[0]);
                }
                else if (range.startContainer.nodeType === 3 && range.startContainer.parentElement.childElementCount > 0 &&
                    range.startOffset > 0 && range.startContainer.parentElement.firstElementChild.tagName.toLowerCase() !== 'br') {
                    isCollapsed = true;
                    range = nodeCutter.GetCursorRange(docElement, range, range.startContainer);
                    nodes.push(range.startContainer);
                }
                else {
                    var cursorNode = this.insertCursorNode(docElement, domSelection, range, isFormatted, nodeCutter, format, value, endNode);
                    domSelection.endContainer = domSelection.startContainer = domSelection.getNodeArray(cursorNode, true);
                    var childNodes = cursorNode.nodeName === 'BR' && cursorNode.parentNode.childNodes;
                    if (!sf.base.isNullOrUndefined(childNodes) && childNodes.length === 1 && childNodes[0].nodeName === 'BR' && nodes.length === 0) {
                        domSelection.setSelectionText(docElement, range.startContainer, range.endContainer, 0, 0);
                        preventRestore = true;
                    }
                    else {
                        domSelection.endOffset = domSelection.startOffset = 1;
                    }
                    if (cursorNode.nodeName === 'BR' && cursorNode.parentNode.textContent.length === 0) {
                        preventRestore = true;
                    }
                }
            }
            isCursor = range.collapsed;
            var isSubSup = false;
            for (var index = 0; index < nodes.length; index++) {
                var formatNode = isFormatted.getFormattedNode(nodes[index], format, endNode);
                if (formatNode === null) {
                    if (format === 'subscript') {
                        formatNode = isFormatted.getFormattedNode(nodes[index], 'superscript', endNode);
                        isSubSup = formatNode === null ? false : true;
                    }
                    else if (format === 'superscript') {
                        formatNode = isFormatted.getFormattedNode(nodes[index], 'subscript', endNode);
                        isSubSup = formatNode === null ? false : true;
                    }
                }
                if (index === 0 && formatNode === null) {
                    isFormat = true;
                }
                if (formatNode !== null && (!isFormat || isFontStyle)) {
                    nodes[index] = this.removeFormat(nodes, index, formatNode, isCursor, isFormat, isFontStyle, range, nodeCutter, format, value, domSelection, endNode, domNode);
                }
                else {
                    nodes[index] = this.insertFormat(docElement, nodes, index, formatNode, isCursor, isFormat, isFontStyle, range, nodeCutter, format, value);
                }
                domSelection = this.applySelection(nodes, domSelection, nodeCutter, index, isCollapsed);
            }
            if (isIDevice$1()) {
                setEditFrameFocus(endNode, selector);
            }
            if (!preventRestore) {
                save.restore();
            }
            if (isSubSup) {
                this.applyFormat(docElement, format, endNode, enterAction);
            }
        }
    };
    SelectionCommands.insertCursorNode = function (docElement, domSelection, range, isFormatted, nodeCutter, format, value, endNode) {
        var cursorNodes = domSelection.getNodeCollection(range);
        var cursorFormat = (cursorNodes.length > 0) ?
            (cursorNodes.length > 1 && range.startContainer === range.endContainer) ?
                this.getCursorFormat(isFormatted, cursorNodes, format, endNode) :
                isFormatted.getFormattedNode(cursorNodes[0], format, endNode) : null;
        var cursorNode = null;
        if (cursorFormat) {
            cursorNode = cursorNodes[0];
            if (cursorFormat.firstChild.textContent.charCodeAt(0) === 8203) {
                var regEx = new RegExp(String.fromCharCode(8203), 'g');
                var emptySpaceNode = void 0;
                if (cursorFormat.firstChild === cursorNode) {
                    cursorNode.textContent = cursorNode.textContent.replace(regEx, '');
                    emptySpaceNode = cursorNode;
                    
                }
                else {
                    cursorFormat.firstChild.textContent = cursorFormat.firstChild.textContent.replace(regEx, '');
                    emptySpaceNode = cursorFormat.firstChild;
                }
                var pointer = void 0;
                if (emptySpaceNode.textContent.length === 0) {
                    if (!sf.base.isNullOrUndefined(emptySpaceNode.previousSibling)) {
                        cursorNode = emptySpaceNode.previousSibling;
                        pointer = emptySpaceNode.textContent.length - 1;
                        domSelection.setCursorPoint(docElement, emptySpaceNode, pointer);
                    }
                    else if (!sf.base.isNullOrUndefined(emptySpaceNode.parentElement) && emptySpaceNode.parentElement.textContent.length === 0) {
                        var brElem = document.createElement('BR');
                        emptySpaceNode.parentElement.appendChild(brElem);
                        sf.base.detach(emptySpaceNode);
                        cursorNode = brElem;
                        domSelection.setCursorPoint(docElement, cursorNode.parentElement, 0);
                    }
                }
            }
            InsertMethods.unwrap(cursorFormat);
        }
        else {
            if (cursorNodes.length > 1 && range.startOffset > 0 && (cursorNodes[0].firstElementChild &&
                cursorNodes[0].firstElementChild.tagName.toLowerCase() === 'br')) {
                cursorNodes[0].innerHTML = '';
            }
            if (cursorNodes.length === 1 && range.startOffset === 0 && (cursorNodes[0].nodeName === 'BR' ||
                cursorNodes[0].nextSibling.nodeName === 'BR')) {
                sf.base.detach(cursorNodes[0].nodeName === '#text' ? cursorNodes[0].nextSibling : cursorNodes[0]);
            }
            cursorNode = this.getInsertNode(docElement, range, format, value).firstChild;
        }
        return cursorNode;
    };
    SelectionCommands.getCursorFormat = function (isFormatted, cursorNodes, format, endNode) {
        var currentNode;
        for (var index = 0; index < cursorNodes.length; index++) {
            currentNode = cursorNodes[index].lastElementChild ?
                cursorNodes[index].lastElementChild : cursorNodes[index];
        }
        return isFormatted.getFormattedNode(currentNode, format, endNode);
    };
    SelectionCommands.removeFormat = function (nodes, index, formatNode, isCursor, isFormat, isFontStyle, range, nodeCutter, format, value, domSelection, endNode, domNode) {
        var splitNode = null;
        if (!(range.startContainer === range.endContainer && range.startOffset === 0
            && range.endOffset === range.startContainer.length)) {
            var nodeIndex = [];
            var cloneNode = nodes[index];
            do {
                nodeIndex.push(domSelection.getIndex(cloneNode));
                cloneNode = cloneNode.parentNode;
            } while (cloneNode && (cloneNode !== formatNode));
            if (nodes[index].nodeName !== 'BR') {
                cloneNode = splitNode = (isCursor && (formatNode.textContent.length - 1) === range.startOffset) ?
                    nodeCutter.SplitNode(range, formatNode, true)
                    : nodeCutter.GetSpliceNode(range, formatNode);
            }
            if (!isCursor) {
                while (cloneNode && cloneNode.childNodes.length > 0 && ((nodeIndex.length - 1) >= 0)
                    && (cloneNode.childNodes.length > nodeIndex[nodeIndex.length - 1])) {
                    cloneNode = cloneNode.childNodes[nodeIndex[nodeIndex.length - 1]];
                    nodeIndex.pop();
                }
                if (nodes[index].nodeName !== 'BR') {
                    if (cloneNode.nodeType === 3 && !(isCursor && cloneNode.nodeValue === '')) {
                        nodes[index] = cloneNode;
                    }
                    else {
                        var divNode = document.createElement('div');
                        divNode.innerHTML = '&#8203;';
                        if (cloneNode.nodeType !== 3) {
                            cloneNode.insertBefore(divNode.firstChild, cloneNode.firstChild);
                            nodes[index] = cloneNode.firstChild;
                        }
                        else {
                            cloneNode.parentNode.insertBefore(divNode.firstChild, cloneNode);
                            nodes[index] = cloneNode.previousSibling;
                            cloneNode.parentNode.removeChild(cloneNode);
                        }
                    }
                }
            }
            else {
                var lastNode = splitNode;
                for (; lastNode.firstChild !== null && lastNode.firstChild.nodeType !== 3; null) {
                    lastNode = lastNode.firstChild;
                }
                lastNode.innerHTML = '&#8203;';
                nodes[index] = lastNode.firstChild;
            }
        }
        var fontStyle;
        if (format === 'backgroundcolor') {
            fontStyle = formatNode.style.fontSize;
        }
        var bgStyle;
        if (format === 'fontsize') {
            var bg = sf.base.closest(nodes[index].parentElement, 'span[style*=' + 'background-color' + ']');
            if (!sf.base.isNullOrUndefined(bg)) {
                bgStyle = bg.style.backgroundColor;
            }
        }
        var formatNodeStyles = formatNode.getAttribute('style');
        var formatNodeTagName = formatNode.tagName;
        var child = InsertMethods.unwrap(formatNode);
        if (child[0] && !isFontStyle) {
            var nodeTraverse = child[index] ? child[index] : child[0];
            var textNode = nodeTraverse;
            for (; nodeTraverse && nodeTraverse.parentElement && nodeTraverse.parentElement !== endNode; 
            // eslint-disable-next-line
            nodeTraverse = nodeTraverse) {
                if (nodeTraverse.parentElement && nodeTraverse.parentElement.tagName.toLocaleLowerCase()
                    === formatNode.tagName.toLocaleLowerCase() &&
                    (nodeTraverse.parentElement.childElementCount > 1 || range.startOffset > 1)) {
                    if (textNode.parentElement && textNode.parentElement.tagName.toLocaleLowerCase()
                        === formatNode.tagName.toLocaleLowerCase()) {
                        if ((range.startOffset === range.endOffset) && textNode.nodeType !== 1 &&
                            !sf.base.isNullOrUndefined(textNode.textContent) && textNode.parentElement.childElementCount > 1) {
                            range.setStart(textNode, 0);
                            range.setEnd(textNode, textNode.textContent.length);
                            nodeCutter.SplitNode(range, textNode.parentElement, false);
                        }
                    }
                    if (nodeTraverse.parentElement.tagName.toLocaleLowerCase() === 'span') {
                        if (formatNode.style.textDecoration === 'underline' &&
                            nodeTraverse.parentElement.style.textDecoration !== 'underline') {
                            nodeTraverse = nodeTraverse.parentElement;
                            continue;
                        }
                    }
                    InsertMethods.unwrap(nodeTraverse.parentElement);
                    nodeTraverse = !sf.base.isNullOrUndefined(nodeTraverse.parentElement) && !domNode.isBlockNode(nodeTraverse.parentElement) ? textNode :
                        nodeTraverse.parentElement;
                }
                else {
                    nodeTraverse = nodeTraverse.parentElement;
                }
            }
        }
        if (child.length > 0 && isFontStyle) {
            for (var num = 0; num < child.length; num++) {
                if (child[num].nodeType !== 3 || (child[num].textContent && child[num].textContent.trim().length > 0)) {
                    child[num] = InsertMethods.Wrap(child[num], this.GetFormatNode(format, value, formatNodeTagName, formatNodeStyles));
                    if (num === 0) {
                        range.setStartBefore(child[num]);
                    }
                    else if (num === child.length - 1) {
                        range.setEndAfter(child[num]);
                    }
                }
            }
            var currentNodeElem = nodes[index].parentElement;
            if (!sf.base.isNullOrUndefined(fontStyle) && fontStyle !== '') {
                currentNodeElem.style.fontSize = fontStyle;
            }
            if (!sf.base.isNullOrUndefined(bgStyle) && bgStyle !== '') {
                currentNodeElem.style.backgroundColor = bgStyle;
            }
            if ((format === 'backgroundcolor' && !sf.base.isNullOrUndefined(fontStyle) && fontStyle !== '') &&
                currentNodeElem.parentElement.innerHTML === currentNodeElem.outerHTML) {
                var curParentElem = currentNodeElem.parentElement;
                curParentElem.parentElement.insertBefore(currentNodeElem, curParentElem);
                sf.base.detach(curParentElem);
            }
            if (format === 'fontsize' || format === 'fontcolor') {
                var liElement = nodes[index].parentElement;
                var parentElement = nodes[index].parentElement;
                while (!sf.base.isNullOrUndefined(parentElement) && parentElement.tagName.toLowerCase() !== 'li') {
                    parentElement = parentElement.parentElement;
                    liElement = parentElement;
                }
                if (!sf.base.isNullOrUndefined(liElement) && liElement.tagName.toLowerCase() === 'li' &&
                    liElement.textContent.trim() === nodes[index].textContent.trim()) {
                    if (format === 'fontsize') {
                        liElement.style.fontSize = value;
                    }
                    else {
                        liElement.style.color = value;
                        liElement.style.textDecoration = 'inherit';
                    }
                }
            }
        }
        return nodes[index];
    };
    SelectionCommands.insertFormat = function (docElement, nodes, index, formatNode, isCursor, isFormat, isFontStyle, range, nodeCutter, format, value) {
        if (!isCursor) {
            if ((formatNode === null && isFormat) || isFontStyle) {
                if (nodes[index].nodeName !== 'BR') {
                    nodes[index] = nodeCutter.GetSpliceNode(range, nodes[index]);
                    nodes[index].textContent = nodeCutter.TrimLineBreak(nodes[index].textContent);
                }
                if (format === 'uppercase' || format === 'lowercase') {
                    nodes[index].textContent = (format === 'uppercase') ? nodes[index].textContent.toLocaleUpperCase()
                        : nodes[index].textContent.toLocaleLowerCase();
                }
                else if (!(isFontStyle === true && value === '')) {
                    var element = this.GetFormatNode(format, value);
                    if (format === 'fontsize' || format === 'fontcolor') {
                        var liElement = nodes[index].parentElement;
                        var parentElement = nodes[index].parentElement;
                        while (!sf.base.isNullOrUndefined(parentElement) && parentElement.tagName.toLowerCase() !== 'li') {
                            parentElement = parentElement.parentElement;
                            liElement = parentElement;
                        }
                        if (!sf.base.isNullOrUndefined(liElement) && liElement.tagName.toLowerCase() === 'li' &&
                            liElement.textContent.trim() === nodes[index].textContent.trim()) {
                            if (format === 'fontsize') {
                                liElement.style.fontSize = value;
                            }
                            else {
                                liElement.style.color = value;
                                liElement.style.textDecoration = 'inherit';
                            }
                        }
                        nodes[index] = this.applyStyles(nodes, index, element);
                        if (format === 'fontsize') {
                            var bg = sf.base.closest(nodes[index].parentElement, 'span[style*=' + 'background-color' + ']');
                            if (!sf.base.isNullOrUndefined(bg)) {
                                nodes[index].parentElement.style.backgroundColor = bg.style.backgroundColor;
                            }
                        }
                    }
                    else {
                        nodes[index] = this.applyStyles(nodes, index, element);
                    }
                }
            }
            else {
                nodes[index] = nodeCutter.GetSpliceNode(range, nodes[index]);
            }
        }
        else {
            if (format !== 'uppercase' && format !== 'lowercase') {
                var element = this.getInsertNode(docElement, range, format, value);
                nodes[index] = element.firstChild;
                nodeCutter.position = 1;
            }
            else {
                nodeCutter.position = range.startOffset;
            }
        }
        return nodes[index];
    };
    SelectionCommands.applyStyles = function (nodes, index, element) {
        if (!(nodes[index].nodeName === 'BR' && this.enterAction === 'BR')) {
            nodes[index] = (index === (nodes.length - 1)) || nodes[index].nodeName === 'BR' ?
                InsertMethods.Wrap(nodes[index], element)
                : InsertMethods.WrapBefore(nodes[index], element, true);
            nodes[index] = this.getChildNode(nodes[index], element);
        }
        return nodes[index];
    };
    SelectionCommands.getInsertNode = function (docElement, range, format, value) {
        var element = this.GetFormatNode(format, value);
        element.innerHTML = '&#8203;';
        if (sf.base.Browser.isIE) {
            var frag = docElement.createDocumentFragment();
            frag.appendChild(element);
            range.insertNode(frag);
        }
        else {
            range.insertNode(element);
        }
        return element;
    };
    SelectionCommands.getChildNode = function (node, element) {
        if (node === undefined || node === null) {
            element.innerHTML = '&#8203;';
            node = element.firstChild;
        }
        return node;
    };
    SelectionCommands.applySelection = function (nodes, domSelection, nodeCutter, index, isCollapsed) {
        if (nodes.length === 1 && !isCollapsed) {
            domSelection.startContainer = domSelection.getNodeArray(nodes[index], true);
            domSelection.endContainer = domSelection.startContainer;
            domSelection.startOffset = 0;
            domSelection.endOffset = nodes[index].textContent.length;
        }
        else if (nodes.length === 1 && isCollapsed) {
            domSelection.startContainer = domSelection.getNodeArray(nodes[index], true);
            domSelection.endContainer = domSelection.startContainer;
            domSelection.startOffset = nodeCutter.position;
            domSelection.endOffset = nodeCutter.position;
        }
        else if (index === 0) {
            domSelection.startContainer = domSelection.getNodeArray(nodes[index], true);
            domSelection.startOffset = 0;
        }
        else if (index === nodes.length - 1) {
            domSelection.endContainer = domSelection.getNodeArray(nodes[index], false);
            domSelection.endOffset = nodes[index].textContent.length;
        }
        return domSelection;
    };
    SelectionCommands.GetFormatNode = function (format, value, tagName, styles) {
        var node;
        switch (format) {
            case 'bold':
                return document.createElement('strong');
            case 'italic':
                return document.createElement('em');
            case 'underline':
                node = document.createElement('span');
                this.updateStyles(node, tagName, styles);
                node.style.textDecoration = 'underline';
                return node;
            case 'strikethrough':
                node = document.createElement('span');
                this.updateStyles(node, tagName, styles);
                node.style.textDecoration = 'line-through';
                return node;
            case 'superscript':
                return document.createElement('sup');
            case 'subscript':
                return document.createElement('sub');
            case 'fontcolor':
                node = document.createElement('span');
                this.updateStyles(node, tagName, styles);
                node.style.color = value;
                node.style.textDecoration = 'inherit';
                return node;
            case 'fontname':
                node = document.createElement('span');
                this.updateStyles(node, tagName, styles);
                node.style.fontFamily = value;
                return node;
            case 'fontsize':
                node = document.createElement('span');
                this.updateStyles(node, tagName, styles);
                node.style.fontSize = value;
                return node;
            default:
                node = document.createElement('span');
                this.updateStyles(node, tagName, styles);
                node.style.backgroundColor = value;
                return node;
        }
    };
    SelectionCommands.updateStyles = function (ele, tag, styles) {
        if (styles !== null && tag === 'SPAN') {
            ele.setAttribute('style', styles);
        }
    };
    SelectionCommands.enterAction = 'P';
    return SelectionCommands;
}());

/**
 * Selection EXEC internal component
 *
 * @hidden
 * @deprecated
 */
var SelectionBasedExec = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function SelectionBasedExec(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    SelectionBasedExec.prototype.addEventListener = function () {
        this.parent.observer.on(SELECTION_TYPE, this.applySelection, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.keyDownHandler, this);
    };
    SelectionBasedExec.prototype.keyDownHandler = function (e) {
        var validFormats = ['bold', 'italic', 'underline', 'strikethrough', 'superscript',
            'subscript', 'uppercase', 'lowercase'];
        if (e.event.ctrlKey && validFormats.indexOf(e.event.action) > -1) {
            e.event.preventDefault();
            SelectionCommands.applyFormat(this.parent.currentDocument, e.event.action, this.parent.editableElement, e.enterAction);
            this.callBack(e, e.event.action);
        }
    };
    SelectionBasedExec.prototype.applySelection = function (e) {
        SelectionCommands.applyFormat(this.parent.currentDocument, e.subCommand.toLocaleLowerCase(), this.parent.editableElement, e.enterAction, e.value, e.selector);
        this.callBack(e, e.subCommand);
    };
    SelectionBasedExec.prototype.callBack = function (event, action) {
        if (event.callBack) {
            event.callBack({
                requestType: action,
                event: event.event,
                editorMode: 'HTML',
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    return SelectionBasedExec;
}());

/**
 * Selection EXEC internal component
 *
 * @hidden
 * @deprecated
 */
var InsertHtmlExec = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - sepcifies the parent element
     * @hidden
     * @deprecated
     */
    function InsertHtmlExec(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    InsertHtmlExec.prototype.addEventListener = function () {
        this.parent.observer.on(INSERTHTML_TYPE, this.applyHtml, this);
    };
    InsertHtmlExec.prototype.applyHtml = function (e) {
        InsertHtml.Insert(this.parent.currentDocument, e.value, this.parent.editableElement, true);
        if (e.subCommand === 'pasteCleanup') {
            var pastedElements = this.parent.editableElement.querySelectorAll('.pasteContent_RTE');
            var allPastedElements = [].slice.call(pastedElements);
            var imgElements = this.parent.editableElement.querySelectorAll('.pasteContent_Img');
            var allImgElm = [].slice.call(imgElements);
            e.callBack({
                requestType: e.subCommand,
                editorMode: 'HTML',
                elements: allPastedElements,
                imgElem: allImgElm
            });
        }
        else {
            if (e.callBack) {
                e.callBack({
                    requestType: e.subCommand,
                    editorMode: 'HTML',
                    event: e.event,
                    range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                    elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
                });
            }
        }
    };
    return InsertHtmlExec;
}());

/**
 * `Clear Format` module is used to handle Clear Format.
 */
var ClearFormat = /** @class */ (function () {
    function ClearFormat() {
    }
    /**
     * clear method
     *
     * @param {Document} docElement - specifies the document element.
     * @param {Node} endNode - specifies the end node
     * @param {string} enterAction - specifies the enter key action
     * @param {string} selector - specifies the string value
     * @returns {void}
     * @hidden
     * @deprecated
     */
    ClearFormat.clear = function (docElement, endNode, enterAction, selector) {
        this.domNode = new DOMNode(endNode, docElement);
        this.defaultTag = enterAction === 'P' ? this.defaultTag : 'div';
        var nodeSelection = new NodeSelection();
        var nodeCutter = new NodeCutter();
        var range = nodeSelection.getRange(docElement);
        var isCollapsed = range.collapsed;
        var nodes = nodeSelection.getInsertNodeCollection(range);
        var save = nodeSelection.save(range, docElement);
        if (!isCollapsed) {
            var preNode = void 0;
            if (nodes[0].nodeName === 'BR' && sf.base.closest(nodes[0], 'table')) {
                preNode = nodeCutter.GetSpliceNode(range, sf.base.closest(nodes[0], 'table'));
            }
            else {
                preNode = nodeCutter.GetSpliceNode(range, nodes[nodes.length > 1 && nodes[0].nodeName === 'IMG' ? 1 : 0]);
            }
            if (nodes.length === 1) {
                nodeSelection.setSelectionContents(docElement, preNode);
                range = nodeSelection.getRange(docElement);
            }
            else {
                var i = 1;
                var lastText = nodes[nodes.length - i];
                while (nodes.length <= i && nodes[nodes.length - i].nodeName === 'BR') {
                    i++;
                    lastText = nodes[nodes.length - i];
                }
                var lasNode = nodeCutter.GetSpliceNode(range, lastText);
                nodeSelection.setSelectionText(docElement, preNode, lasNode, 0, (lasNode.nodeType === 3) ?
                    lasNode.textContent.length : lasNode.childNodes.length);
                range = nodeSelection.getRange(docElement);
            }
            var exactNodes = nodeSelection.getNodeCollection(range);
            var cloneSelectNodes = exactNodes.slice();
            this.clearInlines(nodeSelection.getSelectionNodes(cloneSelectNodes), cloneSelectNodes, nodeSelection.getRange(docElement), nodeCutter, endNode);
            this.reSelection(docElement, save, exactNodes);
            range = nodeSelection.getRange(docElement);
            exactNodes = nodeSelection.getNodeCollection(range);
            var cloneParentNodes = exactNodes.slice();
            this.clearBlocks(docElement, cloneParentNodes, endNode, nodeCutter, nodeSelection);
            if (isIDevice$1()) {
                setEditFrameFocus(endNode, selector);
            }
            this.reSelection(docElement, save, exactNodes);
        }
    };
    ClearFormat.reSelection = function (docElement, save, exactNodes) {
        var selectionNodes = save.getInsertNodes(exactNodes);
        save.startContainer = save.getNodeArray(selectionNodes[0], true, docElement);
        save.startOffset = 0;
        save.endContainer = save.getNodeArray(selectionNodes[selectionNodes.length - 1], false, docElement);
        var endIndexNode = selectionNodes[selectionNodes.length - 1];
        save.endOffset = (endIndexNode.nodeType === 3) ? endIndexNode.textContent.length
            : endIndexNode.childNodes.length;
        save.restore();
    };
    ClearFormat.clearBlocks = function (docElement, nodes, endNode, nodeCutter, nodeSelection) {
        var parentNodes = [];
        for (var index = 0; index < nodes.length; index++) {
            if (this.BLOCK_TAGS.indexOf(nodes[index].nodeName.toLocaleLowerCase()) > -1
                && parentNodes.indexOf(nodes[index]) === -1) {
                parentNodes.push(nodes[index]);
            }
            else if ((this.BLOCK_TAGS.indexOf(nodes[index].parentNode.nodeName.toLocaleLowerCase()) > -1)
                && parentNodes.indexOf(nodes[index].parentNode) === -1
                && endNode !== nodes[index].parentNode) {
                parentNodes.push(nodes[index].parentNode);
            }
        }
        parentNodes = this.spliceParent(parentNodes, nodes)[0];
        parentNodes = this.removeParent(parentNodes);
        this.unWrap(docElement, parentNodes, nodeCutter, nodeSelection);
    };
    ClearFormat.spliceParent = function (parentNodes, nodes) {
        for (var index1 = 0; index1 < parentNodes.length; index1++) {
            var len = parentNodes[index1].childNodes.length;
            for (var index2 = 0; index2 < len; index2++) {
                if ((nodes.indexOf(parentNodes[index1].childNodes[index2]) > 0)
                    && (parentNodes[index1].childNodes[index2].childNodes.length > 0)) {
                    nodes = this.spliceParent([parentNodes[index1].childNodes[index2]], nodes)[1];
                }
                if ((nodes.indexOf(parentNodes[index1].childNodes[index2]) <= -1) &&
                    (parentNodes[index1].childNodes[index2].textContent.trim() !== '')) {
                    for (var index3 = 0; index3 < len; index3++) {
                        if (nodes.indexOf(parentNodes[index1].childNodes[index3]) > -1) {
                            nodes.splice(nodes.indexOf(parentNodes[index1].childNodes[index3]), 1);
                        }
                    }
                    index2 = parentNodes[index1].childNodes.length;
                    var parentIndex = parentNodes.indexOf(parentNodes[index1].parentNode);
                    var nodeIndex = nodes.indexOf(parentNodes[index1].parentNode);
                    if (parentIndex > -1) {
                        parentNodes.splice(parentIndex, 1);
                    }
                    if (nodeIndex > -1) {
                        nodes.splice(nodeIndex, 1);
                    }
                    var elementIndex = nodes.indexOf(parentNodes[index1]);
                    if (elementIndex > -1) {
                        nodes.splice(elementIndex, 1);
                    }
                    parentNodes.splice(index1, 1);
                    index1--;
                }
            }
        }
        return [parentNodes, nodes];
    };
    ClearFormat.removeChild = function (parentNodes, parentNode) {
        var count = parentNode.childNodes.length;
        if (count > 0) {
            for (var index = 0; index < count; index++) {
                if (parentNodes.indexOf(parentNode.childNodes[index]) > -1) {
                    parentNodes = this.removeChild(parentNodes, parentNode.childNodes[index]);
                    parentNodes.splice(parentNodes.indexOf(parentNode.childNodes[index]), 1);
                }
            }
        }
        return parentNodes;
    };
    ClearFormat.removeParent = function (parentNodes) {
        for (var index = 0; index < parentNodes.length; index++) {
            if (parentNodes.indexOf(parentNodes[index].parentNode) > -1) {
                parentNodes = this.removeChild(parentNodes, parentNodes[index]);
                parentNodes.splice(index, 1);
                index--;
            }
        }
        return parentNodes;
    };
    ClearFormat.unWrap = function (docElement, parentNodes, nodeCutter, nodeSelection) {
        for (var index1 = 0; index1 < parentNodes.length; index1++) {
            if (this.NONVALID_TAGS.indexOf(parentNodes[index1].nodeName.toLowerCase()) > -1
                && parentNodes[index1].parentNode
                && this.NONVALID_PARENT_TAGS.indexOf(parentNodes[index1].parentNode.nodeName.toLowerCase()) > -1) {
                nodeSelection.setSelectionText(docElement, parentNodes[index1], parentNodes[index1], 0, parentNodes[index1].childNodes.length);
                InsertMethods.unwrap(nodeCutter.GetSpliceNode(nodeSelection.getRange(docElement), parentNodes[index1].parentNode));
            }
            if (parentNodes[index1].nodeName.toLocaleLowerCase() !== 'p') {
                if (this.NONVALID_PARENT_TAGS.indexOf(parentNodes[index1].nodeName.toLowerCase()) < 0
                    && parentNodes[index1].parentNode.nodeName.toLocaleLowerCase() !== 'p'
                    && !((parentNodes[index1].nodeName.toLocaleLowerCase() === 'blockquote'
                        || parentNodes[index1].nodeName.toLocaleLowerCase() === 'li')
                        && this.IGNORE_PARENT_TAGS.indexOf(parentNodes[index1].childNodes[0].nodeName.toLocaleLowerCase()) > -1)
                    && !(parentNodes[index1].childNodes.length === 1
                        && parentNodes[index1].childNodes[0].nodeName.toLocaleLowerCase() === 'p')) {
                    InsertMethods.Wrap(parentNodes[index1], docElement.createElement(this.defaultTag));
                }
                var childNodes = InsertMethods.unwrap(parentNodes[index1]);
                if (childNodes.length === 1
                    && childNodes[0].parentNode.nodeName.toLocaleLowerCase() === 'p') {
                    InsertMethods.Wrap(parentNodes[index1], docElement.createElement(this.defaultTag));
                    InsertMethods.unwrap(parentNodes[index1]);
                }
                for (var index2 = 0; index2 < childNodes.length; index2++) {
                    if (this.NONVALID_TAGS.indexOf(childNodes[index2].nodeName.toLowerCase()) > -1) {
                        this.unWrap(docElement, [childNodes[index2]], nodeCutter, nodeSelection);
                    }
                    else if (this.BLOCK_TAGS.indexOf(childNodes[index2].nodeName.toLocaleLowerCase()) > -1 &&
                        childNodes[index2].nodeName.toLocaleLowerCase() !== 'p') {
                        var blockNodes = this.removeParent([childNodes[index2]]);
                        this.unWrap(docElement, blockNodes, nodeCutter, nodeSelection);
                    }
                    else if (this.BLOCK_TAGS.indexOf(childNodes[index2].nodeName.toLocaleLowerCase()) > -1 &&
                        childNodes[index2].parentNode.nodeName.toLocaleLowerCase() === childNodes[index2].nodeName.toLocaleLowerCase()) {
                        InsertMethods.unwrap(childNodes[index2]);
                    }
                    else if (this.BLOCK_TAGS.indexOf(childNodes[index2].nodeName.toLocaleLowerCase()) > -1 &&
                        childNodes[index2].nodeName.toLocaleLowerCase() === 'p') {
                        InsertMethods.Wrap(childNodes[index2], docElement.createElement(this.defaultTag));
                        InsertMethods.unwrap(childNodes[index2]);
                    }
                }
            }
            else {
                InsertMethods.Wrap(parentNodes[index1], docElement.createElement(this.defaultTag));
                InsertMethods.unwrap(parentNodes[index1]);
            }
        }
    };
    ClearFormat.clearInlines = function (textNodes, nodes, range, nodeCutter, 
    // eslint-disable-next-line
    endNode) {
        for (var index = 0; index < textNodes.length; index++) {
            var currentInlineNode = textNodes[index];
            var currentNode = void 0;
            while (!this.domNode.isBlockNode(currentInlineNode)) {
                currentNode = currentInlineNode;
                currentInlineNode = currentInlineNode.parentElement;
            }
            if (currentNode &&
                IsFormatted.inlineTags.indexOf(currentNode.nodeName.toLocaleLowerCase()) > -1) {
                nodeCutter.GetSpliceNode(range, currentNode);
                this.removeInlineParent(currentNode);
            }
        }
    };
    ClearFormat.removeInlineParent = function (textNodes) {
        var nodes = InsertMethods.unwrap(textNodes);
        for (var index = 0; index < nodes.length; index++) {
            if (nodes[index].parentNode.childNodes.length === 1
                && IsFormatted.inlineTags.indexOf(nodes[index].parentNode.nodeName.toLocaleLowerCase()) > -1) {
                this.removeInlineParent(nodes[index].parentNode);
            }
            else if (IsFormatted.inlineTags.indexOf(nodes[index].nodeName.toLocaleLowerCase()) > -1) {
                this.removeInlineParent(nodes[index]);
            }
        }
    };
    ClearFormat.BLOCK_TAGS = ['address', 'article', 'aside', 'blockquote',
        'details', 'dd', 'div', 'dl', 'dt', 'fieldset', 'figcaption', 'figure', 'footer',
        'form', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'header', 'hgroup', 'li', 'main', 'nav',
        'noscript', 'ol', 'p', 'pre', 'section', 'ul'];
    ClearFormat.NONVALID_PARENT_TAGS = ['thead', 'tbody', 'ul', 'ol', 'table', 'tfoot', 'tr'];
    ClearFormat.IGNORE_PARENT_TAGS = ['ul', 'ol', 'table'];
    ClearFormat.NONVALID_TAGS = ['thead', 'tbody', 'figcaption', 'td', 'tr', 'th', 'tfoot', 'figcaption', 'li'];
    ClearFormat.defaultTag = 'p';
    return ClearFormat;
}());

/**
 * Clear Format EXEC internal component
 *
 * @hidden
 * @deprecated
 */
var ClearFormatExec = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {EditorManager} parent - specifies the parent element.
     * @returns {void}
     * @hidden
     * @deprecated
     */
    function ClearFormatExec(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    ClearFormatExec.prototype.addEventListener = function () {
        this.parent.observer.on(CLEAR_TYPE, this.applyClear, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.onKeyDown, this);
    };
    ClearFormatExec.prototype.onKeyDown = function (e) {
        switch (e.event.action) {
            case 'clear-format':
                this.applyClear({ subCommand: 'ClearFormat', callBack: e.callBack, enterAction: e.enterAction });
                e.event.preventDefault();
                break;
        }
    };
    ClearFormatExec.prototype.applyClear = function (e) {
        if (e.subCommand === 'ClearFormat') {
            ClearFormat.clear(this.parent.currentDocument, this.parent.editableElement, e.enterAction, e.selector);
            if (e.callBack) {
                e.callBack({
                    requestType: e.subCommand,
                    event: e.event,
                    editorMode: 'HTML',
                    range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                    elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
                });
            }
        }
    };
    return ClearFormatExec;
}());

/**
 * `Undo` module is used to handle undo actions.
 */
var UndoRedoManager = /** @class */ (function () {
    function UndoRedoManager(parent, options) {
        this.undoRedoStack = [];
        this.parent = parent;
        this.undoRedoSteps = !sf.base.isNullOrUndefined(options) ? options.undoRedoSteps : 30;
        this.undoRedoTimer = !sf.base.isNullOrUndefined(options) ? options.undoRedoTimer : 300;
        this.addEventListener();
    }
    UndoRedoManager.prototype.addEventListener = function () {
        var debounceListener = sf.base.debounce(this.keyUp, this.undoRedoTimer);
        this.parent.observer.on(KEY_UP_HANDLER, debounceListener, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.keyDown, this);
        this.parent.observer.on(ACTION, this.onAction, this);
        this.parent.observer.on(MODEL_CHANGED_PLUGIN, this.onPropertyChanged, this);
    };
    UndoRedoManager.prototype.onPropertyChanged = function (props) {
        for (var _i = 0, _a = Object.keys(props.newProp); _i < _a.length; _i++) {
            var prop = _a[_i];
            switch (prop) {
                case 'undoRedoSteps':
                    this.undoRedoSteps = props.newProp.undoRedoSteps;
                    break;
                case 'undoRedoTimer':
                    this.undoRedoTimer = props.newProp.undoRedoTimer;
                    break;
            }
        }
    };
    UndoRedoManager.prototype.removeEventListener = function () {
        this.parent.observer.off(KEY_UP_HANDLER, this.keyUp);
        this.parent.observer.off(KEY_DOWN_HANDLER, this.keyDown);
        this.parent.observer.off(ACTION, this.onAction);
    };
    /**
     * onAction method
     *
     * @param {IHtmlSubCommands} e - specifies the sub command
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoManager.prototype.onAction = function (e) {
        if (e.subCommand === 'Undo') {
            this.undo(e);
        }
        else {
            this.redo(e);
        }
    };
    /**
     * Destroys the ToolBar.
     *
     * @function destroy
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoManager.prototype.destroy = function () {
        this.removeEventListener();
    };
    UndoRedoManager.prototype.keyDown = function (e) {
        var event = e.event;
        // eslint-disable-next-line
        var proxy = this;
        switch (event.action) {
            case 'undo':
                event.preventDefault();
                proxy.undo(e);
                break;
            case 'redo':
                event.preventDefault();
                proxy.redo(e);
                break;
        }
    };
    UndoRedoManager.prototype.keyUp = function (e) {
        if (e.event.keyCode !== 17 && !e.event.ctrlKey) {
            this.saveData(e);
        }
    };
    /**
     * RTE collection stored html format.
     *
     * @function saveData
     * @param {KeyboardEvent} e - specifies the keyboard event
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoManager.prototype.saveData = function (e) {
        var range = new NodeSelection().getRange(this.parent.currentDocument);
        var save = new NodeSelection().save(range, this.parent.currentDocument);
        var htmlText = this.parent.editableElement.innerHTML;
        var changEle = { text: htmlText, range: save };
        if (this.undoRedoStack.length >= this.steps) {
            this.undoRedoStack = this.undoRedoStack.slice(0, this.steps + 1);
        }
        if (this.undoRedoStack.length > 1 && (this.undoRedoStack[this.undoRedoStack.length - 1].range.range.collapsed === range.collapsed)
            && (this.undoRedoStack[this.undoRedoStack.length - 1].range.startOffset === save.range.startOffset) &&
            (this.undoRedoStack[this.undoRedoStack.length - 1].range.endOffset === save.range.endOffset) &&
            (this.undoRedoStack[this.undoRedoStack.length - 1].range.range.startContainer === save.range.startContainer) &&
            (this.undoRedoStack[this.undoRedoStack.length - 1].text.trim() === changEle.text.trim())) {
            return;
        }
        this.undoRedoStack.push(changEle);
        this.steps = this.undoRedoStack.length - 1;
        if (this.steps > this.undoRedoSteps) {
            this.undoRedoStack.shift();
            this.steps--;
        }
        if (e && e.callBack) {
            e.callBack();
        }
    };
    /**
     * Undo the editable text.
     *
     * @function undo
     * @param {IHtmlSubCommands} e - specifies the sub commands
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoManager.prototype.undo = function (e) {
        if (this.steps > 0) {
            var range = this.undoRedoStack[this.steps - 1].range;
            var removedContent = this.undoRedoStack[this.steps - 1].text;
            this.parent.editableElement.innerHTML = removedContent;
            this.parent.editableElement.focus();
            if (isIDevice$1()) {
                setEditFrameFocus(this.parent.editableElement, e.selector);
            }
            range.restore();
            this.steps--;
            if (e.callBack) {
                e.callBack({
                    requestType: 'Undo',
                    editorMode: 'HTML',
                    range: range,
                    elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument),
                    event: e.event
                });
            }
        }
    };
    /**
     * Redo the editable text.
     *
     * @param {IHtmlSubCommands} e - specifies the sub commands
     * @function redo
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoManager.prototype.redo = function (e) {
        if (this.undoRedoStack[this.steps + 1] != null) {
            var range = this.undoRedoStack[this.steps + 1].range;
            this.parent.editableElement.innerHTML = this.undoRedoStack[this.steps + 1].text;
            this.parent.editableElement.focus();
            if (isIDevice$1()) {
                setEditFrameFocus(this.parent.editableElement, e.selector);
            }
            range.restore();
            this.steps++;
            if (e.callBack) {
                e.callBack({
                    requestType: 'Redo',
                    editorMode: 'HTML',
                    range: range,
                    elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument),
                    event: e.event
                });
            }
        }
    };
    /**
     * getUndoStatus method
     *
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    UndoRedoManager.prototype.getUndoStatus = function () {
        var status = { undo: false, redo: false };
        if (this.steps > 0) {
            status.undo = true;
        }
        if (this.undoRedoStack[this.steps + 1] != null) {
            status.redo = true;
        }
        return status;
    };
    return UndoRedoManager;
}());

/**
 * PasteCleanup for MsWord content
 *
 * @hidden
 * @deprecated
 */
var MsWordPaste = /** @class */ (function () {
    function MsWordPaste(parent) {
        this.olData = [
            'decimal',
            'lower-alpha',
            'lower-roman',
            'upper-alpha',
            'upper-roman',
            'lower-greek'
        ];
        this.ulData = [
            'disc',
            'square',
            'circle',
            'disc',
            'square',
            'circle'
        ];
        this.ignorableNodes = ['A', 'APPLET', 'B', 'BLOCKQUOTE', 'BR',
            'BUTTON', 'CENTER', 'CODE', 'COL', 'COLGROUP', 'DD', 'DEL', 'DFN', 'DIR', 'DIV',
            'DL', 'DT', 'EM', 'FIELDSET', 'FONT', 'FORM', 'FRAME', 'FRAMESET', 'H1', 'H2',
            'H3', 'H4', 'H5', 'H6', 'HR', 'I', 'IMG', 'IFRAME', 'INPUT', 'INS', 'LABEL',
            'LI', 'OL', 'OPTION', 'P', 'PARAM', 'PRE', 'Q', 'S', 'SELECT', 'SPAN', 'STRIKE',
            'STRONG', 'SUB', 'SUP', 'TABLE', 'TBODY', 'TD', 'TEXTAREA', 'TFOOT', 'TH',
            'THEAD', 'TITLE', 'TR', 'TT', 'U', 'UL'];
        this.blockNode = ['div', 'p', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
            'address', 'blockquote', 'button', 'center', 'dd', 'dir', 'dl', 'dt', 'fieldset',
            'frameset', 'hr', 'iframe', 'isindex', 'li', 'map', 'menu', 'noframes', 'noscript',
            'object', 'ol', 'pre', 'table', 'tbody', 'td', 'tfoot', 'th', 'thead', 'tr', 'ul',
            'header', 'article', 'nav', 'footer', 'section', 'aside', 'main', 'figure', 'figcaption'];
        this.borderStyle = ['border-top', 'border-right', 'border-bottom', 'border-left'];
        this.removableElements = ['o:p', 'style'];
        this.listContents = [];
        this.parent = parent;
        this.addEventListener();
    }
    MsWordPaste.prototype.addEventListener = function () {
        this.parent.observer.on(MS_WORD_CLEANUP_PLUGIN$1, this.wordCleanup, this);
    };
    MsWordPaste.prototype.wordCleanup = function (e) {
        var wordPasteStyleConfig = e.allowedStylePropertiesArray;
        var listNodes = [];
        var tempHTMLContent = e.args.clipboardData.getData('text/HTML');
        var rtfData = e.args.clipboardData.getData('text/rtf');
        var elm = sf.base.createElement('p');
        elm.setAttribute('id', 'MSWord-Content');
        elm.innerHTML = tempHTMLContent;
        var patern = /class='?Mso|style='[^ ]*\bmso-/i;
        var patern2 = /class="?Mso|style="[^ ]*\bmso-/i;
        var patern3 = /(class="?Mso|class='?Mso|class="?Xl|class='?Xl|class=Xl|style="[^"]*\bmso-|style='[^']*\bmso-|w:WordDocument)/gi;
        var pattern4 = /style='mso-width-source:/i;
        if (patern.test(tempHTMLContent) || patern2.test(tempHTMLContent) || patern3.test(tempHTMLContent) ||
            pattern4.test(tempHTMLContent)) {
            this.imageConversion(elm, rtfData);
            tempHTMLContent = tempHTMLContent.replace(/<img[^>]+>/i, '');
            this.addListClass(elm);
            listNodes = this.cleanUp(elm, listNodes);
            if (!sf.base.isNullOrUndefined(listNodes[0]) && listNodes[0].parentElement.tagName !== 'UL' &&
                listNodes[0].parentElement.tagName !== 'OL') {
                this.listConverter(listNodes);
            }
            this.styleCorrection(elm, wordPasteStyleConfig);
            this.removingComments(elm);
            this.removeUnwantedElements(elm);
            this.removeEmptyElements(elm);
            this.breakLineAddition(elm);
            this.removeClassName(elm);
            if (pattern4.test(tempHTMLContent)) {
                this.addTableBorderClass(elm);
            }
            e.callBack(elm.innerHTML);
        }
        else {
            e.callBack(elm.innerHTML);
        }
    };
    MsWordPaste.prototype.addListClass = function (elm) {
        var allNodes = elm.querySelectorAll('*');
        for (var index = 0; index < allNodes.length; index++) {
            if (!sf.base.isNullOrUndefined(allNodes[index].getAttribute('style')) && allNodes[index].getAttribute('style').replace(/ /g, '').replace('\n', '').indexOf('mso-list:l') >= 0 &&
                allNodes[index].className.toLowerCase().indexOf('msolistparagraph') === -1 && allNodes[index].tagName.charAt(0) !== 'H') {
                allNodes[index].classList.add('msolistparagraph');
            }
        }
    };
    MsWordPaste.prototype.addTableBorderClass = function (elm) {
        var allTableElm = elm.querySelectorAll('table');
        var hasTableBorder = false;
        for (var i = 0; i < allTableElm.length; i++) {
            for (var j = 0; j < this.borderStyle.length; j++) {
                if (allTableElm[i].innerHTML.indexOf(this.borderStyle[j]) >= 0) {
                    hasTableBorder = true;
                    break;
                }
            }
            if (hasTableBorder) {
                allTableElm[i].classList.add('e-rte-table-border');
                hasTableBorder = false;
            }
        }
    };
    MsWordPaste.prototype.imageConversion = function (elm, rtfData) {
        this.checkVShape(elm);
        var imgElem = elm.querySelectorAll('img');
        var imgSrc = [];
        var base64Src = [];
        var imgName = [];
        // eslint-disable-next-line
        var linkRegex = new RegExp(/([^\S]|^)(((https?\:\/\/)|(www\.))(\S+))/gi);
        if (imgElem.length > 0) {
            for (var i = 0; i < imgElem.length; i++) {
                imgSrc.push(imgElem[i].getAttribute('src'));
                imgName.push(imgElem[i].getAttribute('src').split('/')[imgElem[i].getAttribute('src').split('/').length - 1].split('.')[0]);
            }
            var hexValue = this.hexConversion(rtfData);
            for (var i = 0; i < hexValue.length; i++) {
                base64Src.push(this.convertToBase64(hexValue[i]));
            }
            for (var i = 0; i < imgElem.length; i++) {
                if (imgSrc[i].match(linkRegex)) {
                    imgElem[i].setAttribute('src', imgSrc[i]);
                }
                else {
                    imgElem[i].setAttribute('src', base64Src[i]);
                }
                imgElem[i].setAttribute('id', 'msWordImg-' + imgName[i]);
            }
        }
    };
    MsWordPaste.prototype.checkVShape = function (elm) {
        var allNodes = elm.querySelectorAll('*');
        for (var i = 0; i < allNodes.length; i++) {
            switch (allNodes[i].nodeName) {
                case 'V:SHAPETYPE':
                    sf.base.detach(allNodes[i]);
                    break;
                case 'V:SHAPE':
                    if (allNodes[i].firstElementChild.nodeName === 'V:IMAGEDATA') {
                        var src = allNodes[i].firstElementChild.getAttribute('src');
                        var imgElement = sf.base.createElement('img');
                        imgElement.setAttribute('src', src);
                        allNodes[i].parentElement.insertBefore(imgElement, allNodes[i]);
                        sf.base.detach(allNodes[i]);
                    }
                    break;
            }
        }
    };
    MsWordPaste.prototype.convertToBase64 = function (hexValue) {
        var byteArr = this.conHexStringToBytes(hexValue.hex);
        var base64String = this.conBytesToBase64(byteArr);
        var base64 = hexValue.type ? 'data:' + hexValue.type + ';base64,' + base64String : null;
        return base64;
    };
    MsWordPaste.prototype.conBytesToBase64 = function (byteArr) {
        var base64Str = '';
        var base64Char = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/';
        var byteArrLen = byteArr.length;
        for (var i = 0; i < byteArrLen; i += 3) {
            var array3 = byteArr.slice(i, i + 3);
            var array3length = array3.length;
            var array4 = [];
            if (array3length < 3) {
                for (var j = array3length; j < 3; j++) {
                    array3[j] = 0;
                }
            }
            array4[0] = (array3[0] & 0xFC) >> 2;
            array4[1] = ((array3[0] & 0x03) << 4) | (array3[1] >> 4);
            array4[2] = ((array3[1] & 0x0F) << 2) | ((array3[2] & 0xC0) >> 6);
            array4[3] = array3[2] & 0x3F;
            for (var j = 0; j < 4; j++) {
                if (j <= array3length) {
                    base64Str += base64Char.charAt(array4[j]);
                }
                else {
                    base64Str += '=';
                }
            }
        }
        return base64Str;
    };
    MsWordPaste.prototype.conHexStringToBytes = function (hex) {
        var byteArr = [];
        var byteArrLen = hex.length / 2;
        for (var i = 0; i < byteArrLen; i++) {
            byteArr.push(parseInt(hex.substr(i * 2, 2), 16));
        }
        return byteArr;
    };
    MsWordPaste.prototype.hexConversion = function (rtfData) {
        // eslint-disable-next-line
        var picHead = /\{\\pict[\s\S]+?\\bliptag\-?\d+(\\blipupi\-?\d+)?(\{\\\*\\blipuid\s?[\da-fA-F]+)?[\s\}]*?/;
        var pic = new RegExp('(?:(' + picHead.source + '))([\\da-fA-F\\s]+)\\}', 'g');
        var fullImg = rtfData.match(pic);
        var imgType;
        var result = [];
        if (!sf.base.isNullOrUndefined(fullImg)) {
            for (var i = 0; i < fullImg.length; i++) {
                if (picHead.test(fullImg[i])) {
                    if (fullImg[i].indexOf('\\pngblip') !== -1) {
                        imgType = 'image/png';
                    }
                    else if (fullImg[i].indexOf('\\jpegblip') !== -1) {
                        imgType = 'image/jpeg';
                    }
                    else {
                        continue;
                    }
                    result.push({
                        hex: imgType ? fullImg[i].replace(picHead, '').replace(/[^\da-fA-F]/g, '') : null,
                        type: imgType
                    });
                }
            }
        }
        return result;
    };
    MsWordPaste.prototype.removeClassName = function (elm) {
        var elmWithClass = elm.querySelectorAll('*[class]');
        for (var i = 0; i < elmWithClass.length; i++) {
            elmWithClass[i].removeAttribute('class');
        }
    };
    MsWordPaste.prototype.breakLineAddition = function (elm) {
        var allElements = elm.querySelectorAll('*');
        for (var i = 0; i < allElements.length; i++) {
            if (allElements[i].children.length === 0 && allElements[i].innerHTML === '&nbsp;' &&
                (allElements[i].innerHTML === '&nbsp;' && !allElements[i].closest('li')) &&
                !allElements[i].closest('td')) {
                var detachableElement = this.findDetachElem(allElements[i]);
                var brElement = sf.base.createElement('br');
                if (!sf.base.isNullOrUndefined(detachableElement.parentElement)) {
                    detachableElement.parentElement.insertBefore(brElement, detachableElement);
                    sf.base.detach(detachableElement);
                }
            }
        }
    };
    MsWordPaste.prototype.findDetachElem = function (element) {
        var removableElement;
        if (!sf.base.isNullOrUndefined(element.parentElement) &&
            element.parentElement.textContent.trim() === '' && element.parentElement.tagName !== 'TD' &&
            sf.base.isNullOrUndefined(element.parentElement.querySelector('img'))) {
            removableElement = this.findDetachElem(element.parentElement);
        }
        else {
            removableElement = element;
        }
        return removableElement;
    };
    MsWordPaste.prototype.removeUnwantedElements = function (elm) {
        var innerElement = elm.innerHTML;
        for (var i = 0; i < this.removableElements.length; i++) {
            var regExpStartElem = new RegExp('<' + this.removableElements[i] + '>', 'g');
            var regExpEndElem = new RegExp('</' + this.removableElements[i] + '>', 'g');
            innerElement = innerElement.replace(regExpStartElem, '');
            innerElement = innerElement.replace(regExpEndElem, '');
        }
        elm.innerHTML = innerElement;
        elm.querySelectorAll(':empty');
    };
    MsWordPaste.prototype.findDetachEmptyElem = function (element) {
        var removableElement;
        if (!sf.base.isNullOrUndefined(element.parentElement)) {
            if (element.parentElement.textContent.trim() === '' &&
                element.parentElement.getAttribute('id') !== 'MSWord-Content' &&
                sf.base.isNullOrUndefined(element.parentElement.querySelector('img'))) {
                removableElement = this.findDetachEmptyElem(element.parentElement);
            }
            else {
                removableElement = element;
            }
        }
        else {
            removableElement = null;
        }
        return removableElement;
    };
    MsWordPaste.prototype.removeEmptyElements = function (element) {
        var emptyElements = element.querySelectorAll(':empty');
        for (var i = 0; i < emptyElements.length; i++) {
            if (!sf.base.isNullOrUndefined(emptyElements[i].closest('td')) &&
                !sf.base.isNullOrUndefined(emptyElements[i].closest('td').querySelector('.MsoNormal'))) {
                emptyElements[i].innerHTML = '-';
            }
            if (emptyElements[i].tagName !== 'IMG' && emptyElements[i].tagName !== 'BR' &&
                emptyElements[i].tagName !== 'IFRAME' && emptyElements[i].tagName !== 'TD' &&
                emptyElements[i].tagName !== 'HR') {
                var detachableElement = this.findDetachEmptyElem(emptyElements[i]);
                if (!sf.base.isNullOrUndefined(detachableElement)) {
                    sf.base.detach(detachableElement);
                }
            }
        }
    };
    MsWordPaste.prototype.styleCorrection = function (elm, wordPasteStyleConfig) {
        var styleElement = elm.querySelectorAll('style');
        if (styleElement.length > 0) {
            var styles = styleElement[0].innerHTML.match(/[\S ]+\s+{[\s\S]+?}/gi);
            var styleClassObject_1 = !sf.base.isNullOrUndefined(styles) ? this.findStyleObject(styles) : null;
            var keys = Object.keys(styleClassObject_1);
            var values = keys.map(function (key) {
                return styleClassObject_1[key];
            });
            values = this.removeUnwantedStyle(values, wordPasteStyleConfig);
            this.filterStyles(elm, wordPasteStyleConfig);
            var resultElem = void 0;
            var fromClass = false;
            for (var i = 0; i < keys.length; i++) {
                if (keys[i].split('.')[0] === '') {
                    resultElem = elm.getElementsByClassName(keys[i].split('.')[1]);
                    fromClass = true;
                }
                else if (keys[i].split('.').length === 1 && keys[i].split('.')[0].indexOf('@') >= 0) {
                    continue;
                }
                else if (keys[i].split('.').length === 1 && keys[i].split('.')[0].indexOf('@') < 0) {
                    resultElem = elm.getElementsByTagName(keys[i]);
                }
                else {
                    resultElem = elm.querySelectorAll(keys[i]);
                }
                for (var j = 0; j < resultElem.length; j++) {
                    var styleProperty = resultElem[j].getAttribute('style');
                    if (!sf.base.isNullOrUndefined(styleProperty) && styleProperty.trim() !== '') {
                        var valueSplit = values[i].split(';');
                        if (!fromClass) {
                            for (var k = 0; k < valueSplit.length; k++) {
                                if (styleProperty.indexOf(valueSplit[k].split(':')[0]) >= 0) {
                                    valueSplit.splice(k, 1);
                                    k--;
                                }
                            }
                        }
                        values[i] = valueSplit.join(';') + ';';
                        var changedValue = styleProperty + values[i];
                        resultElem[j].setAttribute('style', changedValue);
                    }
                    else {
                        values[i] = values[i].replace(/text-indent:-(.*?)(?=;|$)/gm, '');
                        resultElem[j].setAttribute('style', values[i]);
                    }
                }
                fromClass = false;
            }
        }
    };
    MsWordPaste.prototype.filterStyles = function (elm, wordPasteStyleConfig) {
        var elmWithStyles = elm.querySelectorAll('*[style]');
        for (var i = 0; i < elmWithStyles.length; i++) {
            var elemStyleProperty = elmWithStyles[i].getAttribute('style').split(';');
            var styleValue = '';
            for (var j = 0; j < elemStyleProperty.length; j++) {
                if (wordPasteStyleConfig.indexOf(elemStyleProperty[j].split(':')[0].trim()) >= 0) {
                    styleValue += elemStyleProperty[j] + ';';
                }
            }
            elmWithStyles[i].setAttribute('style', styleValue);
        }
    };
    MsWordPaste.prototype.removeUnwantedStyle = function (values, wordPasteStyleConfig) {
        for (var i = 0; i < values.length; i++) {
            var styleValues = values[i].split(';');
            values[i] = '';
            for (var j = 0; j < styleValues.length; j++) {
                if (wordPasteStyleConfig.indexOf(styleValues[j].split(':')[0]) >= 0) {
                    values[i] += styleValues[j] + ';';
                }
            }
        }
        return values;
    };
    MsWordPaste.prototype.findStyleObject = function (styles) {
        var styleClassObject = {};
        for (var i = 0; i < styles.length; i++) {
            var tempStyle = styles[i];
            var classNameCollection = tempStyle.replace(/([\S ]+\s+){[\s\S]+?}/gi, '$1');
            var stylesCollection = tempStyle.replace(/[\S ]+\s+{([\s\S]+?)}/gi, '$1');
            classNameCollection = classNameCollection.replace(/^[\s]|[\s]$/gm, '');
            stylesCollection = stylesCollection.replace(/^[\s]|[\s]$/gm, '');
            classNameCollection = classNameCollection.replace(/\n|\r|\n\r/g, '');
            stylesCollection = stylesCollection.replace(/\n|\r|\n\r/g, '');
            for (var classNames = classNameCollection.split(', '), j = 0; j < classNames.length; j++) {
                styleClassObject[classNames[j]] = stylesCollection;
            }
        }
        return styleClassObject;
    };
    MsWordPaste.prototype.removingComments = function (elm) {
        var innerElement = elm.innerHTML;
        innerElement = innerElement.replace(/<!--[\s\S]*?-->/g, '');
        elm.innerHTML = innerElement;
    };
    MsWordPaste.prototype.cleanUp = function (node, listNodes) {
        // eslint-disable-next-line
        var tempCleaner = [];
        var prevflagState;
        var allNodes = node.querySelectorAll('*');
        for (var index = 0; index < allNodes.length; index++) {
            if (this.ignorableNodes.indexOf(allNodes[index].nodeName) === -1 ||
                (allNodes[index].nodeType === 3 && allNodes[index].textContent.trim() === '')) {
                tempCleaner.push(allNodes[index]);
                continue;
            }
            else if (allNodes[index].className &&
                allNodes[index].className.toLowerCase().indexOf('msolistparagraph') !== -1 &&
                allNodes[index].childElementCount !== 1 && !sf.base.isNullOrUndefined(allNodes[index].getAttribute('style')) &&
                allNodes[index].getAttribute('style').indexOf('mso-list:') >= 0) {
                if (allNodes[index].className.indexOf('MsoListParagraphCxSpFirst') >= 0 && listNodes.length > 0 &&
                    listNodes[listNodes.length - 1] !== null) {
                    listNodes.push(null);
                }
                listNodes.push(allNodes[index]);
            }
            if (prevflagState && (this.blockNode.indexOf(allNodes[index].nodeName.toLowerCase()) !== -1) &&
                !(allNodes[index].className &&
                    allNodes[index].className.toLowerCase().indexOf('msolistparagraph') !== -1 && !sf.base.isNullOrUndefined(allNodes[index].getAttribute('style')) &&
                    allNodes[index].getAttribute('style').indexOf('mso-list:') >= 0)) {
                listNodes.push(null);
            }
            if (this.blockNode.indexOf(allNodes[index].nodeName.toLowerCase()) !== -1) {
                if (allNodes[index].className &&
                    allNodes[index].className.toLowerCase().indexOf('msolistparagraph') !== -1 && !sf.base.isNullOrUndefined(allNodes[index].getAttribute('style')) &&
                    allNodes[index].getAttribute('style').indexOf('mso-list:') >= 0) {
                    prevflagState = true;
                }
                else {
                    prevflagState = false;
                }
            }
        }
        if (listNodes.length && (listNodes[listNodes.length - 1] !== null)) {
            listNodes.push(null);
        }
        return listNodes;
    };
    MsWordPaste.prototype.listConverter = function (listNodes) {
        var level;
        var data = [];
        var collection = [];
        var content = '';
        var stNode;
        var currentListStyle = '';
        for (var i = 0; i < listNodes.length; i++) {
            if (listNodes[i] === null) {
                data.push({ content: this.makeConversion(collection), node: listNodes[i - 1] });
                collection = [];
                continue;
            }
            if (listNodes[i].getAttribute('style') && listNodes[i].getAttribute('style').indexOf('mso-outline-level') !== -1) {
                listNodes[i].setAttribute('style', listNodes[i].getAttribute('style').replace('mso-outline-level', 'mso-outline'));
            }
            content = listNodes[i].getAttribute('style');
            if (content && content.indexOf('level') !== -1) {
                // eslint-disable-next-line
                level = parseInt(content.charAt(content.indexOf('level') + 5), null);
            }
            else {
                level = 1;
            }
            this.listContents = [];
            this.getListContent(listNodes[i]);
            var type = void 0;
            if (!sf.base.isNullOrUndefined(this.listContents[0])) {
                type = this.listContents[0].trim().length > 1 ? 'ol' : 'ul';
                var tempNode = [];
                for (var j = 1; j < this.listContents.length; j++) {
                    tempNode.push(this.listContents[j]);
                }
                var currentClassName = void 0;
                if (!sf.base.isNullOrUndefined(listNodes[i].className)) {
                    currentClassName = listNodes[i].className;
                }
                if (!sf.base.isNullOrUndefined(listNodes[i].getAttribute('style'))) {
                    listNodes[i].setAttribute('style', listNodes[i].getAttribute('style').replace('text-align:start;', ''));
                    if (listNodes[i].style.textAlign !== '') {
                        listNodes[i].setAttribute('style', 'text-align:' + listNodes[i].style.textAlign);
                        currentListStyle = listNodes[i].getAttribute('style');
                    }
                }
                collection.push({ listType: type, content: tempNode, nestedLevel: level, class: currentClassName,
                    listStyle: currentListStyle });
            }
        }
        stNode = listNodes.shift();
        while (stNode) {
            var elemColl = [];
            for (var temp1 = 0; temp1 < data.length; temp1++) {
                if (data[temp1].node === stNode) {
                    for (var index = 0; index < data[temp1].content.childNodes.length; index++) {
                        elemColl.push(data[temp1].content.childNodes[index]);
                    }
                    for (var index = 0; index < elemColl.length; index++) {
                        stNode.parentElement.insertBefore(elemColl[index], stNode);
                    }
                    break;
                }
            }
            stNode.remove();
            stNode = listNodes.shift();
            if (!stNode) {
                stNode = listNodes.shift();
            }
        }
    };
    MsWordPaste.prototype.makeConversion = function (collection) {
        var root = sf.base.createElement('div');
        var temp;
        var pLevel = 1;
        var prevList;
        var listCount = 0;
        var elem;
        for (var index = 0; index < collection.length; index++) {
            var pElement = sf.base.createElement('p');
            pElement.innerHTML = collection[index].content.join(' ');
            if ((collection[index].nestedLevel === 1) && listCount === 0 && collection[index].content) {
                root.appendChild(temp = sf.base.createElement(collection[index].listType));
                prevList = sf.base.createElement('li');
                prevList.appendChild(pElement);
                temp.appendChild(prevList);
                temp.setAttribute('level', collection[index].nestedLevel.toString());
                temp.style.listStyle = this.getListStyle(collection[index].listType, collection[index].nestedLevel);
            }
            else if (collection[index].nestedLevel === pLevel) {
                if (prevList.parentElement.tagName.toLowerCase() === collection[index].listType) {
                    prevList.parentElement.appendChild(prevList = sf.base.createElement('li'));
                    prevList.appendChild(pElement);
                }
                else {
                    temp = sf.base.createElement(collection[index].listType);
                    prevList.parentElement.parentElement.appendChild(temp);
                    prevList = sf.base.createElement('li');
                    prevList.appendChild(pElement);
                    temp.appendChild(prevList);
                    temp.setAttribute('level', collection[index].nestedLevel.toString());
                }
            }
            else if (collection[index].nestedLevel > pLevel) {
                if (!sf.base.isNullOrUndefined(prevList)) {
                    for (var j = 0; j < collection[index].nestedLevel - pLevel; j++) {
                        prevList.appendChild(temp = sf.base.createElement(collection[index].listType));
                        prevList = sf.base.createElement('li', { styles: 'list-style-type: none;' });
                        temp.appendChild(prevList);
                    }
                    prevList.appendChild(pElement);
                    temp.setAttribute('level', collection[index].nestedLevel.toString());
                    temp.style.listStyle = this.getListStyle(collection[index].listType, collection[index].nestedLevel);
                    temp.childNodes[0].style.listStyle =
                        this.getListStyle(collection[index].listType, collection[index].nestedLevel);
                }
                else {
                    root.appendChild(temp = sf.base.createElement(collection[index].listType));
                    prevList = sf.base.createElement('li');
                    prevList.appendChild(pElement);
                    temp.appendChild(prevList);
                    temp.setAttribute('level', collection[index].nestedLevel.toString());
                    temp.style.listStyle = this.getListStyle(collection[index].listType, collection[index].nestedLevel);
                }
            }
            else if (collection[index].nestedLevel === 1) {
                if (root.lastChild.tagName.toLowerCase() === collection[index].listType) {
                    temp = root.lastChild;
                }
                else {
                    root.appendChild(temp = sf.base.createElement(collection[index].listType));
                }
                prevList = sf.base.createElement('li');
                prevList.appendChild(pElement);
                temp.appendChild(prevList);
                temp.setAttribute('level', collection[index].nestedLevel.toString());
                temp.style.listStyle = this.getListStyle(collection[index].listType, collection[index].nestedLevel);
            }
            else {
                elem = prevList;
                while (elem.parentElement) {
                    elem = elem.parentElement;
                    if (elem.attributes.getNamedItem('level')) {
                        // eslint-disable-next-line
                        if (parseInt(elem.attributes.getNamedItem('level').textContent, null) === collection[index].nestedLevel) {
                            prevList = sf.base.createElement('li');
                            prevList.appendChild(pElement);
                            elem.appendChild(prevList);
                            break;
                            // eslint-disable-next-line
                        }
                        else if (collection[index].nestedLevel > parseInt(elem.attributes.getNamedItem('level').textContent, null)) {
                            elem.appendChild(temp = sf.base.createElement(collection[index].listType));
                            prevList = sf.base.createElement('li');
                            prevList.appendChild(pElement);
                            temp.appendChild(prevList);
                            temp.setAttribute('level', collection[index].nestedLevel.toString());
                            temp.style.listStyle = this.getListStyle(collection[index].listType, collection[index].nestedLevel);
                            break;
                        }
                    }
                    continue;
                }
            }
            prevList.setAttribute('class', collection[index].class);
            var currentStyle = prevList.getAttribute('style');
            prevList.setAttribute('style', (!sf.base.isNullOrUndefined(currentStyle) ? currentStyle : '') + collection[index].listStyle);
            pLevel = collection[index].nestedLevel;
            listCount++;
        }
        return root;
    };
    MsWordPaste.prototype.getListStyle = function (listType, nestedLevel) {
        nestedLevel = (nestedLevel > 0) ? nestedLevel - 1 : nestedLevel;
        if (listType === 'ol') {
            return (nestedLevel < this.olData.length ? this.olData[nestedLevel] : this.olData[0]);
        }
        else {
            return (nestedLevel < this.ulData.length ? this.ulData[nestedLevel] : this.ulData[0]);
        }
    };
    MsWordPaste.prototype.getListContent = function (elem) {
        var pushContent = '';
        var firstChild = elem.firstElementChild;
        if (firstChild.textContent.trim() === '' && !sf.base.isNullOrUndefined(firstChild.firstElementChild) &&
            firstChild.firstElementChild.nodeName === 'IMG') {
            pushContent = elem.innerHTML.trim();
            this.listContents.push('');
            this.listContents.push(pushContent);
        }
        else {
            var styleNodes = ['b', 'em'];
            if (firstChild.childNodes.length > 0 && (firstChild.querySelectorAll('b').length > 0
                || firstChild.querySelectorAll('em').length > 0)) {
                for (var i = 0; i < firstChild.childNodes.length; i++) {
                    var nodeName = firstChild.childNodes[i].nodeName.toLowerCase();
                    if (firstChild.childNodes[i].textContent.trim().length > 1 && styleNodes.indexOf(nodeName) !== -1) {
                        pushContent = '<' + nodeName + '>' + firstChild.childNodes[i].textContent + '</' + nodeName + '>';
                        this.listContents.push(pushContent);
                    }
                    else if (firstChild.childNodes[i].textContent.trim().length === 1) {
                        this.listContents.push(firstChild.childNodes[i].textContent.trim());
                    }
                }
            }
            else {
                pushContent = firstChild.textContent.trim();
                this.listContents.push(pushContent);
            }
        }
        sf.base.detach(firstChild);
        this.listContents.push(elem.innerHTML);
    };
    return MsWordPaste;
}());

/**
 * Insert a Text Node or Text
 *
 * @hidden
 * @deprecated
 */
var InsertTextExec = /** @class */ (function () {
    /**
     * Constructor for creating the InsertText plugin
     *
     * @param {EditorManager} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function InsertTextExec(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    InsertTextExec.prototype.addEventListener = function () {
        this.parent.observer.on(INSERT_TEXT_TYPE, this.insertText, this);
    };
    InsertTextExec.prototype.insertText = function (e) {
        var node = document.createTextNode(e.value);
        InsertHtml.Insert(this.parent.currentDocument, node);
        if (e.callBack) {
            e.callBack({
                requestType: e.subCommand,
                editorMode: 'HTML',
                event: e.event,
                range: this.parent.nodeSelection.getRange(this.parent.currentDocument),
                elements: this.parent.nodeSelection.getSelectedNodes(this.parent.currentDocument)
            });
        }
    };
    return InsertTextExec;
}());

/**
 * EditorManager internal component
 *
 * @hidden
 * @deprecated
 */
var EditorManager = /** @class */ (function () {
    /**
     * Constructor for creating the component
     *
     * @hidden
     * @deprecated
     * @param {ICommandModel} options - specifies the command Model
     */
    function EditorManager(options) {
        this.currentDocument = options.document;
        this.editableElement = options.editableElement;
        this.nodeSelection = new NodeSelection();
        this.nodeCutter = new NodeCutter();
        this.domNode = new DOMNode(this.editableElement, this.currentDocument);
        this.observer = new sf.base.Observer(this);
        this.listObj = new Lists(this);
        this.formatObj = new Formats(this);
        this.alignmentObj = new Alignments(this);
        this.indentsObj = new Indents(this);
        this.linkObj = new LinkCommand(this);
        this.imgObj = new ImageCommand(this);
        this.selectionObj = new SelectionBasedExec(this);
        this.inserthtmlObj = new InsertHtmlExec(this);
        this.insertTextObj = new InsertTextExec(this);
        this.clearObj = new ClearFormatExec(this);
        this.tableObj = new TableCommand(this);
        this.undoRedoManager = new UndoRedoManager(this, options.options);
        this.msWordPaste = new MsWordPaste(this);
        this.wireEvents();
    }
    EditorManager.prototype.wireEvents = function () {
        this.observer.on(KEY_DOWN, this.editorKeyDown, this);
        this.observer.on(KEY_UP, this.editorKeyUp, this);
        this.observer.on(KEY_UP, this.editorKeyUp, this);
        this.observer.on(MODEL_CHANGED, this.onPropertyChanged, this);
        this.observer.on(MS_WORD_CLEANUP$1, this.onWordPaste, this);
        this.observer.on(ON_BEGIN$1, this.onBegin, this);
    };
    EditorManager.prototype.onWordPaste = function (e) {
        this.observer.notify(MS_WORD_CLEANUP_PLUGIN$1, e);
    };
    EditorManager.prototype.onPropertyChanged = function (props) {
        this.observer.notify(MODEL_CHANGED_PLUGIN, props);
    };
    EditorManager.prototype.editorKeyDown = function (e) {
        this.observer.notify(KEY_DOWN_HANDLER, e);
    };
    EditorManager.prototype.editorKeyUp = function (e) {
        this.observer.notify(KEY_UP_HANDLER, e);
    };
    EditorManager.prototype.onBegin = function (e) {
        this.observer.notify(SPACE_ACTION, e);
    };
    /* eslint-disable */
    /**
     * execCommand
     *
     * @param {ExecCommand} command - specifies the execution command
     * @param {T} value - specifes the value.
     * @param {Event} event - specifies the call back event
     * @param {Function} callBack - specifies the function
     * @param {string} text - specifies the string value
     * @param {T} exeValue - specifies the values to be executed
     * @param {string} selector - specifies the selector values
     * @returns {void}
     * @hidden
     * @deprecated
     */
    /* eslint-enable */
    EditorManager.prototype.execCommand = function (command, value, event, callBack, text, exeValue, selector, enterAction) {
        switch (command.toLowerCase()) {
            case 'lists':
                this.observer.notify(LIST_TYPE, { subCommand: value, event: event, callBack: callBack,
                    selector: selector, item: exeValue, enterAction: enterAction });
                break;
            case 'formats':
                this.observer.notify(FORMAT_TYPE, { subCommand: value, event: event, callBack: callBack,
                    selector: selector, exeValue: exeValue, enterAction: enterAction
                });
                break;
            case 'alignments':
                this.observer.notify(ALIGNMENT_TYPE, {
                    subCommand: value, event: event, callBack: callBack,
                    selector: selector,
                    value: exeValue
                });
                break;
            case 'indents':
                this.observer.notify(INDENT_TYPE, { subCommand: value, event: event, callBack: callBack, selector: selector });
                break;
            case 'links':
                this.observer.notify(LINK, { command: command, value: value, item: exeValue, event: event, callBack: callBack });
                break;
            case 'files':
                this.observer.notify(IMAGE, {
                    command: command, value: 'Image', item: exeValue, event: event, callBack: callBack, selector: selector
                });
                break;
            case 'images':
                this.observer.notify(IMAGE, {
                    command: command, value: value, item: exeValue, event: event, callBack: callBack, selector: selector
                });
                break;
            case 'table':
                switch (value.toString().toLocaleLowerCase()) {
                    case 'createtable':
                        this.observer.notify(TABLE, { item: exeValue, event: event, callBack: callBack, enterAction: enterAction });
                        break;
                    case 'insertrowbefore':
                    case 'insertrowafter':
                        this.observer.notify(INSERT_ROW, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'insertcolumnleft':
                    case 'insertcolumnright':
                        this.observer.notify(INSERT_COLUMN, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'deleterow':
                        this.observer.notify(DELETEROW, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'deletecolumn':
                        this.observer.notify(DELETECOLUMN, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'tableremove':
                        this.observer.notify(REMOVETABLE, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'tableheader':
                        this.observer.notify(TABLEHEADER, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'aligntop':
                    case 'alignmiddle':
                    case 'alignbottom':
                        this.observer.notify(TABLE_VERTICAL_ALIGN, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'merge':
                        this.observer.notify(TABLE_MERGE, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'horizontalsplit':
                        this.observer.notify(TABLE_HORIZONTAL_SPLIT, { item: exeValue, event: event, callBack: callBack });
                        break;
                    case 'verticalsplit':
                        this.observer.notify(TABLE_VERTICAL_SPLIT, { item: exeValue, event: event, callBack: callBack });
                        break;
                }
                break;
            case 'font':
            case 'style':
            case 'effects':
            case 'casing':
                this.observer.notify(SELECTION_TYPE, { subCommand: value, event: event, callBack: callBack, value: text, selector: selector, enterAction: enterAction });
                break;
            case 'inserthtml':
                this.observer.notify(INSERTHTML_TYPE, { subCommand: value, callBack: callBack, value: text });
                break;
            case 'inserttext':
                this.observer.notify(INSERT_TEXT_TYPE, { subCommand: value, callBack: callBack, value: text });
                break;
            case 'clear':
                this.observer.notify(CLEAR_TYPE, { subCommand: value, event: event, callBack: callBack, selector: selector, enterAction: enterAction });
                break;
            case 'actions':
                this.observer.notify(ACTION, { subCommand: value, event: event, callBack: callBack, selector: selector });
                break;
        }
    };
    return EditorManager;
}());

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
/**
 * HTML adapter
 */
var HTMLFormatter = /** @class */ (function (_super) {
    __extends(HTMLFormatter, _super);
    function HTMLFormatter(options) {
        var _this = _super.call(this) || this;
        _this.initialize();
        sf.base.extend(_this, _this, options, true);
        if (_this.currentDocument && _this.element) {
            _this.updateFormatter(_this.element, _this.currentDocument, options.options);
        }
        return _this;
    }
    HTMLFormatter.prototype.initialize = function () {
        this.keyConfig = htmlKeyConfig;
    };
    HTMLFormatter.prototype.updateFormatter = function (editElement, doc, options) {
        if (editElement && doc) {
            this.editorManager = new EditorManager({
                document: doc,
                editableElement: editElement,
                options: options
            });
        }
    };
    return HTMLFormatter;
}(Formatter));

/**
 * `HtmlEditor` module is used to HTML editor
 */
var HtmlEditor = /** @class */ (function () {
    function HtmlEditor(parent) {
        this.rangeCollection = [];
        this.parent = parent;
        this.xhtmlValidation = new XhtmlValidation(parent);
        this.addEventListener();
    }
    HtmlEditor.prototype.addEventListener = function () {
        this.nodeSelectionObj = new NodeSelection();
        this.parent.observer.on(htmlToolbarClick, this.onToolbarClick, this);
        this.parent.observer.on(keyDown, this.onKeyDown, this);
        this.parent.observer.on(initialEnd, this.render, this);
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(selectAll$1, this.selectAll, this);
        this.parent.observer.on(selectRange, this.selectRange, this);
        this.parent.observer.on(getSelectedHtml, this.getSelectedHtml, this);
        this.parent.observer.on(selectionSave, this.onSelectionSave, this);
        this.parent.observer.on(selectionRestore, this.onSelectionRestore, this);
        this.parent.observer.on(readOnlyMode, this.updateReadOnly, this);
        this.parent.observer.on(paste, this.onPaste, this);
    };
    HtmlEditor.prototype.sanitizeHelper = function (value) {
        value = sanitizeHelper(value, this.parent);
        return value;
    };
    HtmlEditor.prototype.updateReadOnly = function () {
        if (this.parent.readonly) {
            sf.base.attributes(this.parent.getEditPanel(), { contenteditable: 'false' });
            sf.base.addClass([this.parent.element], CLS_RTE_READONLY);
        }
        else {
            sf.base.attributes(this.parent.getEditPanel(), { contenteditable: 'true' });
            sf.base.removeClass([this.parent.element], CLS_RTE_READONLY);
        }
    };
    HtmlEditor.prototype.onSelectionSave = function () {
        var currentDocument = this.parent.getDocument();
        var range = this.nodeSelectionObj.getRange(currentDocument);
        this.saveSelection = this.nodeSelectionObj.save(range, currentDocument);
    };
    HtmlEditor.prototype.onSelectionRestore = function (e) {
        this.parent.isBlur = false;
        this.parent.getEditPanel().focus();
        if (sf.base.isNullOrUndefined(e.items) || e.items) {
            this.saveSelection.restore();
        }
    };
    HtmlEditor.prototype.onKeyDown = function (e) {
        var _this = this;
        var currentRange;
        var args = e.args;
        if (sf.base.Browser.info.name === 'chrome') {
            currentRange = this.parent.getRange();
            this.backSpaceCleanup(e, currentRange);
            this.deleteCleanup(e, currentRange);
        }
        if (args.keyCode === 9 && sf.base.isNullOrUndefined(sf.base.closest(args.target, '.e-rte-toolbar')) && this.parent.enableTabKey) {
            var range = this.nodeSelectionObj.getRange(this.parent.getDocument());
            var parentNode = this.nodeSelectionObj.getParentNodeCollection(range);
            if (!((parentNode[0].nodeName === 'LI' || sf.base.closest(parentNode[0], 'li') ||
                sf.base.closest(parentNode[0], 'table')) && range.startOffset === 0)) {
                args.preventDefault();
                if (!args.shiftKey) {
                    InsertHtml.Insert(this.parent.getDocument(), '&nbsp;&nbsp;&nbsp;&nbsp;');
                    this.rangeCollection.push(this.nodeSelectionObj.getRange(this.parent.getDocument()));
                }
                else if (this.rangeCollection.length > 0 &&
                    this.rangeCollection[this.rangeCollection.length - 1].startContainer.textContent.length === 4) {
                    var textCont = this.rangeCollection[this.rangeCollection.length - 1].startContainer;
                    this.nodeSelectionObj.setSelectionText(this.parent.getDocument(), textCont, textCont, 0, textCont.textContent.length);
                    InsertHtml.Insert(this.parent.getDocument(), document.createTextNode(''));
                    this.rangeCollection.pop();
                }
            }
        }
        if (e.args.action === 'space') {
            var currentRange_1 = this.parent.getRange();
            var editorValue = currentRange_1.startContainer.textContent.slice(0, currentRange_1.startOffset);
            var orderedList_1 = this.isOrderedList(editorValue);
            var unOrderedList = this.isUnOrderedList(editorValue);
            if (orderedList_1 && !unOrderedList || unOrderedList && !orderedList_1) {
                var eventArgs_1 = {
                    callBack: null,
                    event: e.args,
                    name: 'keydown-handler'
                };
                this.parent.dotNetRef.invokeMethodAsync(
                // @ts-ignore-start
                actionBeginEvent, { requestType: orderedList_1 ? 'OL' : 'UL', cancel: false }).then(function (actionBeginArgs) {
                    // @ts-ignore-end
                    if (!actionBeginArgs.cancel) {
                        _this.parent.formatter.editorManager.observer.notify(ON_BEGIN, eventArgs_1);
                        _this.parent.dotNetRef.invokeMethodAsync(
                        // @ts-ignore-start
                        actionCompleteEvent, { editorMode: _this.parent.editorMode, name: actionCompleteEvent, requestType: orderedList_1 ? 'OL' : 'UL'
                            // @ts-ignore-end
                        });
                    }
                });
            }
        }
        if (e.args.action === 'space' ||
            e.args.action === 'enter' ||
            e.args.keyCode === 13) {
            this.spaceLink(args);
            if (this.parent.editorMode === 'HTML' && !((this.parent.shiftEnterKey === 'BR' && e.args.shiftKey))) {
                this.parent.observer.notify(enterHandler, { args: e.args });
            }
        }
        if (sf.base.Browser.info.name === 'chrome' && (!sf.base.isNullOrUndefined(this.rangeElement) && !sf.base.isNullOrUndefined(this.oldRangeElement) ||
            !sf.base.isNullOrUndefined(this.deleteRangeElement) && !sf.base.isNullOrUndefined(this.deleteOldRangeElement)) &&
            currentRange.startContainer.parentElement.tagName !== 'TD' && currentRange.startContainer.parentElement.tagName !== 'TH') {
            this.rangeElement = null;
            this.oldRangeElement = null;
            this.deleteRangeElement = null;
            this.deleteOldRangeElement = null;
            args.preventDefault();
        }
    };
    HtmlEditor.prototype.isOrderedList = function (editorValue) {
        editorValue = editorValue.replace(/\u200B/g, '');
        var olListStartRegex = [/^[1]+[.]+$/, /^[i]+[.]+$/, /^[a]+[.]+$/];
        if (!sf.base.isNullOrUndefined(editorValue)) {
            for (var i = 0; i < olListStartRegex.length; i++) {
                if (olListStartRegex[i].test(editorValue)) {
                    return true;
                }
            }
        }
        return false;
    };
    HtmlEditor.prototype.isUnOrderedList = function (editorValue) {
        editorValue = editorValue.replace(/\u200B/g, '');
        var ulListStartRegex = [/^[*]$/, /^[-]$/];
        if (!sf.base.isNullOrUndefined(editorValue)) {
            for (var i = 0; i < ulListStartRegex.length; i++) {
                if (ulListStartRegex[i].test(editorValue)) {
                    return true;
                }
            }
        }
        return false;
    };
    HtmlEditor.prototype.backSpaceCleanup = function (e, currentRange) {
        var isLiElement = false;
        if (e.args.code === 'Backspace' && e.args.keyCode === 8 && currentRange.startOffset === 0 &&
            currentRange.endOffset === 0 && this.parent.getSelection().length === 0 && currentRange.startContainer.textContent.length > 0 &&
            currentRange.startContainer.parentElement.tagName !== 'TD' && currentRange.startContainer.parentElement.tagName !== 'TH') {
            this.rangeElement = this.getRootBlockNode(currentRange.startContainer);
            if (this.rangeElement.tagName === 'OL' || this.rangeElement.tagName === 'UL') {
                var liElement = this.getRangeLiNode(currentRange.startContainer);
                if (liElement.previousElementSibling && liElement.previousElementSibling.childElementCount > 0) {
                    this.oldRangeElement = liElement.previousElementSibling.lastElementChild;
                    if (!sf.base.isNullOrUndefined(liElement.lastElementChild)) {
                        this.rangeElement = liElement.lastElementChild;
                        isLiElement = true;
                    }
                    else {
                        this.rangeElement = liElement;
                    }
                }
            }
            else if (this.rangeElement.tagName === 'TABLE' || (!sf.base.isNullOrUndefined(this.rangeElement.previousElementSibling) &&
                this.rangeElement.previousElementSibling.tagName === 'TABLE')) {
                return;
            }
            else {
                this.oldRangeElement = this.rangeElement.previousElementSibling;
            }
            if (sf.base.isNullOrUndefined(this.oldRangeElement)) {
                return;
            }
            else {
                if (this.oldRangeElement.tagName === 'OL' || this.oldRangeElement.tagName === 'UL') {
                    this.oldRangeElement = this.oldRangeElement.lastElementChild.lastElementChild ? this.oldRangeElement.lastElementChild.lastElementChild :
                        this.oldRangeElement.lastElementChild;
                }
                this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), this.oldRangeElement, this.oldRangeElement.childNodes.length);
                if (this.oldRangeElement.querySelector('BR')) {
                    sf.base.detach(this.oldRangeElement.querySelector('BR'));
                }
                if (!sf.base.isNullOrUndefined(this.rangeElement) && this.oldRangeElement != this.rangeElement) {
                    while (this.rangeElement.firstChild) {
                        this.oldRangeElement.appendChild(this.rangeElement.childNodes[0]);
                    }
                    !isLiElement ? sf.base.detach(this.rangeElement) : sf.base.detach(this.rangeElement.parentElement);
                    this.oldRangeElement.normalize();
                }
            }
        }
    };
    HtmlEditor.prototype.deleteCleanup = function (e, currentRange) {
        var isLiElement = false;
        var liElement;
        var rootElement;
        if (e.args.code === 'Delete' && e.args.keyCode === 46 &&
            this.parent.getText().trim().length !== 0 && this.parent.getSelection().length === 0 && currentRange.startContainer.parentElement.tagName !== 'TD' &&
            currentRange.startContainer.parentElement.tagName !== 'TH') {
            this.deleteRangeElement = rootElement = this.getRootBlockNode(currentRange.startContainer);
            if (this.deleteRangeElement.tagName === 'OL' || this.deleteRangeElement.tagName === 'UL') {
                liElement = this.getRangeLiNode(currentRange.startContainer);
                if (liElement.nextElementSibling && liElement.nextElementSibling.childElementCount > 0
                    && !liElement.nextElementSibling.querySelector('BR')) {
                    if (!sf.base.isNullOrUndefined(liElement.lastElementChild)) {
                        this.deleteRangeElement = liElement.lastElementChild;
                        isLiElement = true;
                    }
                    else {
                        this.deleteRangeElement = liElement;
                    }
                }
                else {
                    this.deleteRangeElement = this.getRangeElement(liElement);
                }
            }
            else if (this.deleteRangeElement.nodeType === 3 || (this.deleteRangeElement.tagName === 'TABLE' ||
                (!sf.base.isNullOrUndefined(this.deleteRangeElement.nextElementSibling) && this.deleteRangeElement.nextElementSibling.tagName === 'TABLE'))) {
                return;
            }
            if (this.getCaretIndex(currentRange, this.deleteRangeElement) === this.deleteRangeElement.textContent.length) {
                if (!sf.base.isNullOrUndefined(liElement)) {
                    if (isLiElement || !sf.base.isNullOrUndefined(liElement.nextElementSibling)) {
                        this.deleteOldRangeElement = this.getRangeElement(liElement.nextElementSibling);
                    }
                    else {
                        this.deleteOldRangeElement = rootElement.nextElementSibling;
                    }
                }
                else {
                    this.deleteOldRangeElement = this.deleteRangeElement.nextElementSibling;
                }
                if (sf.base.isNullOrUndefined(this.deleteOldRangeElement)) {
                    return;
                }
                else {
                    this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), this.deleteRangeElement, this.deleteRangeElement.childNodes.length);
                    if (this.deleteRangeElement.querySelector('BR')) {
                        sf.base.detach(this.deleteRangeElement.querySelector('BR'));
                    }
                    if (!sf.base.isNullOrUndefined(this.deleteRangeElement) && (this.deleteOldRangeElement.tagName !== 'OL' && this.deleteOldRangeElement.tagName !== 'UL')
                        && this.deleteOldRangeElement !== this.deleteRangeElement) {
                        while (this.deleteOldRangeElement.firstChild) {
                            this.deleteRangeElement.appendChild(this.deleteOldRangeElement.childNodes[0]);
                        }
                        !isLiElement ? sf.base.detach(this.deleteOldRangeElement) : sf.base.detach(this.deleteOldRangeElement.parentElement);
                        this.deleteRangeElement.normalize();
                    }
                    else {
                        this.deleteRangeElement = null;
                        this.deleteOldRangeElement = null;
                    }
                }
            }
            else {
                this.deleteRangeElement = null;
            }
        }
    };
    HtmlEditor.prototype.getCaretIndex = function (currentRange, element) {
        var position = 0;
        if (this.parent.getDocument().getSelection().rangeCount !== 0) {
            var preCaretRange = currentRange.cloneRange();
            preCaretRange.selectNodeContents(element);
            preCaretRange.setEnd(currentRange.endContainer, currentRange.endOffset);
            position = preCaretRange.toString().length;
        }
        return position;
    };
    HtmlEditor.prototype.getRangeElement = function (element) {
        var rangeElement = element.lastElementChild ? element.lastElementChild.tagName === 'BR' ?
            element.lastElementChild.previousElementSibling ? element.lastElementChild.previousElementSibling
                : element : element.lastElementChild : element;
        return rangeElement;
    };
    HtmlEditor.prototype.getRootBlockNode = function (rangeBlockNode) {
        for (; rangeBlockNode && this.parent && this.parent.inputElement != rangeBlockNode; rangeBlockNode = rangeBlockNode) {
            if (rangeBlockNode.parentElement === this.parent.inputElement) {
                break;
            }
            else {
                rangeBlockNode = rangeBlockNode.parentElement;
            }
        }
        return rangeBlockNode;
    };
    HtmlEditor.prototype.getRangeLiNode = function (rangeLiNode) {
        var node = rangeLiNode.parentElement;
        while (node != this.parent.inputElement) {
            if (node.nodeType === 1 && node.tagName === 'LI') {
                break;
            }
            node = node.parentElement;
        }
        return node;
    };
    HtmlEditor.prototype.onPaste = function (e) {
        var regex = new RegExp(/([^\S]|^)(((https?\:\/\/)|(www\.))(\S+))/gi);
        if (e.text.match(regex)) {
            e.args.preventDefault();
            var range = this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument());
            var saveSelection = this.parent.formatter.editorManager.nodeSelection.save(range, this.parent.getDocument());
            var httpRegex = new RegExp(/([^\S]|^)(((https?\:\/\/)))/gi);
            var wwwRegex = new RegExp(/([^\S]|^)(((www\.))(\S+))/gi);
            var enterSplitText = e.text.split('\n');
            var contentInnerElem = '';
            for (var i = 0; i < enterSplitText.length; i++) {
                if (enterSplitText[i].trim() === '') {
                    contentInnerElem += getDefaultValue(this.parent);
                }
                else {
                    var contentWithSpace = '';
                    var spaceBetweenContent = true;
                    var spaceSplit = enterSplitText[i].split(' ');
                    for (var j = 0; j < spaceSplit.length; j++) {
                        if (spaceSplit[j].trim() === '') {
                            contentWithSpace += spaceBetweenContent ? '&nbsp;' : ' ';
                        }
                        else {
                            spaceBetweenContent = false;
                            contentWithSpace += spaceSplit[j] + ' ';
                        }
                    }
                    if (i === 0) {
                        contentInnerElem += '<span>' + contentWithSpace.trim() + '</span>';
                    }
                    else {
                        contentInnerElem += '<p>' + contentWithSpace.trim() + '</p>';
                    }
                }
            }
            var divElement = document.createElement('div');
            divElement.setAttribute('class', 'pasteContent');
            divElement.style.display = 'inline';
            divElement.innerHTML = contentInnerElem;
            var paraElem = divElement.querySelectorAll('span, p');
            for (var i = 0; i < paraElem.length; i++) {
                var splitTextContent = paraElem[i].innerHTML.split(' ');
                var resultSplitContent = '';
                for (var j = 0; j < splitTextContent.length; j++) {
                    if (splitTextContent[j].match(httpRegex) || splitTextContent[j].match(wwwRegex)) {
                        resultSplitContent += '<a className="e-rte-anchor" href="' + splitTextContent[j] +
                            '" title="' + splitTextContent[j] + '">' + splitTextContent[j] + ' </a>';
                    }
                    else {
                        resultSplitContent += splitTextContent[j] + ' ';
                    }
                }
                paraElem[i].innerHTML = resultSplitContent.trim();
            }
            // if (!isNullOrUndefined(this.parent.pasteCleanupModule)) {
            e.callBack(divElement.innerHTML);
            // } else {
            //     this.parent.executeCommand('insertHTML', divElement);
            // }
        }
    };
    HtmlEditor.prototype.spaceLink = function (e) {
        var range = this.nodeSelectionObj.getRange(this.parent.getDocument());
        var selectNodeEle = this.nodeSelectionObj.getParentNodeCollection(range);
        var text = range.startContainer.textContent.substr(0, range.endOffset);
        var splitText = text.split(' ');
        var urlText = splitText[splitText.length - 1];
        var urlTextRange = range.startOffset - (text.length - splitText[splitText.length - 1].length);
        urlText = urlText.slice(0, urlTextRange);
        var regex = new RegExp(/([^\S]|^)(((https?\:\/\/)|(www\.))(\S+))/gi);
        if (selectNodeEle[0].nodeName !== 'A' && urlText.match(regex)) {
            var selection = this.nodeSelectionObj.save(range, this.parent.getDocument());
            var url = urlText.indexOf('http') > -1 ? urlText : 'http://' + urlText;
            var selectParent = this.parent.formatter.editorManager.nodeSelection.getParentNodeCollection(range);
            var value = {
                url: url,
                selection: selection, selectParent: selectParent,
                text: urlText,
                title: '',
                target: '_blank'
            };
            this.parent.formatter.process(this.parent, {
                item: {
                    'command': 'Links',
                    'subCommand': 'CreateLink'
                }
            }, e, value);
        }
    };
    HtmlEditor.prototype.onToolbarClick = function (args) {
        var save;
        var selectNodeEle;
        var selectParentEle;
        var item = args.item;
        var target = args.originalEvent.target;
        var closestElement = target && sf.base.closest(target, '.' + CLS_QUICK_POP);
        if (closestElement && !closestElement.classList.contains(CLS_INLINE_POP)) {
            if (!(item.subCommand === 'SourceCode' || item.subCommand === 'Preview' ||
                item.subCommand === 'FontColor' || item.subCommand === 'BackgroundColor')) {
                if (isIDevice$1() && item.command === 'Images') {
                    this.nodeSelectionObj.restore();
                }
                var range = this.nodeSelectionObj.getRange(this.parent.getDocument());
                save = this.nodeSelectionObj.save(range, this.parent.getDocument());
                selectNodeEle = this.nodeSelectionObj.getNodeCollection(range);
                selectParentEle = this.nodeSelectionObj.getParentNodeCollection(range);
            }
            if (item.command === 'Images') {
                this.parent.observer.notify(imageToolbarAction, {
                    member: 'image', args: args, selectNode: selectNodeEle, selection: save, selectParent: selectParentEle
                });
            }
            if (item.command === 'Links') {
                this.parent.observer.notify(linkToolbarAction, {
                    member: 'link', args: args, selectNode: selectNodeEle, selection: save, selectParent: selectParentEle
                });
            }
            if (item.command === 'Table') {
                this.parent.observer.notify(tableToolbarAction, {
                    member: 'table', args: args, selectNode: selectNodeEle, selection: save, selectParent: selectParentEle
                });
            }
        }
        else {
            if (!(item.subCommand === 'SourceCode' || item.subCommand === 'Preview' ||
                item.subCommand === 'FontColor' || item.subCommand === 'BackgroundColor')) {
                var range = this.nodeSelectionObj.getRange(this.parent.getDocument());
                save = this.nodeSelectionObj.save(range, this.parent.getDocument());
                selectNodeEle = this.nodeSelectionObj.getNodeCollection(range);
                selectParentEle = this.nodeSelectionObj.getParentNodeCollection(range);
            }
            switch (item.subCommand) {
                case 'Maximize':
                    this.parent.observer.notify(enableFullScreen, { args: args });
                    break;
                case 'Minimize':
                    this.parent.observer.notify(disableFullScreen, { args: args });
                    break;
                case 'CreateLink':
                    this.parent.observer.notify(insertLink, {
                        member: 'link', args: args, selectNode: selectNodeEle, selection: save, selectParent: selectParentEle
                    });
                    break;
                case 'RemoveLink':
                    this.parent.observer.notify(unLink, {
                        member: 'link', args: args, selectNode: selectNodeEle, selection: save, selectParent: selectParentEle
                    });
                    break;
                case 'Print':
                    this.parent.print();
                    break;
                case 'Image':
                    this.parent.observer.notify(insertImage, {
                        member: 'image', args: args, selectNode: selectNodeEle, selection: save, selectParent: selectParentEle
                    });
                    break;
                case 'CreateTable':
                    this.parent.observer.notify(createTable, {
                        member: 'table', args: args, selection: save
                    });
                    break;
                case 'SourceCode':
                    this.parent.observer.notify(sourceCode, { member: 'viewSource', args: args });
                    break;
                case 'Preview':
                    this.parent.observer.notify(updateSource, { member: 'updateSource', args: args });
                    break;
                case 'FontColor':
                case 'BackgroundColor':
                    break;
                default:
                    this.parent.formatter.process(this.parent, args, args.originalEvent, null);
                    break;
            }
        }
    };
    HtmlEditor.prototype.render = function () {
        var editElement = this.parent.getEditPanel();
        var option = { undoRedoSteps: this.parent.undoRedoSteps, undoRedoTimer: this.parent.undoRedoTimer };
        this.parent.formatter = new HTMLFormatter({
            currentDocument: this.parent.getDocument(),
            element: editElement,
            options: option
        });
        if (this.parent.toolbarSettings.enable) {
            this.toolbarUpdate = new HtmlToolbarStatus(this.parent);
        }
        if (this.parent.inlineMode.enable) {
            if (!sf.base.isNullOrUndefined(this.parent.fontFamily.default)) {
                editElement.style.fontFamily = this.parent.fontFamily.default;
            }
            if (!sf.base.isNullOrUndefined(this.parent.fontSize.default)) {
                editElement.style.fontSize = this.parent.fontSize.default;
            }
        }
        this.parent.observer.notify(bindOnEnd, {});
    };
    HtmlEditor.prototype.selectAll = function () {
        var nodes = getTextNodesUnder(this.parent.getDocument(), this.parent.getEditPanel());
        if (nodes.length > 0) {
            this.parent.formatter.editorManager.nodeSelection.setSelectionText(this.parent.getDocument(), nodes[0], nodes[nodes.length - 1], 0, nodes[nodes.length - 1].textContent.length);
        }
    };
    HtmlEditor.prototype.selectRange = function (e) {
        this.parent.formatter.editorManager.nodeSelection.setRange(this.parent.getDocument(), e.range);
    };
    HtmlEditor.prototype.getSelectedHtml = function (e) {
        e.callBack(this.parent.formatter.editorManager.nodeSelection.getRange(this.parent.getDocument()).toString());
    };
    HtmlEditor.prototype.removeEventListener = function () {
        this.parent.observer.off(initialEnd, this.render);
        this.parent.observer.off(htmlToolbarClick, this.onToolbarClick);
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(keyDown, this.onKeyDown);
        this.parent.observer.off(selectAll$1, this.selectAll);
        this.parent.observer.off(selectRange, this.selectRange);
        this.parent.observer.off(getSelectedHtml, this.getSelectedHtml);
        this.parent.observer.off(selectionSave, this.onSelectionSave);
        this.parent.observer.off(selectionRestore, this.onSelectionRestore);
        this.parent.observer.off(readOnlyMode, this.updateReadOnly);
        this.parent.observer.off(paste, this.onPaste);
    };
    HtmlEditor.prototype.destroy = function () {
        this.removeEventListener();
    };
    return HtmlEditor;
}());

/**
 * `FullScreen` module is used to maximize and minimize screen
 */
var FullScreen = /** @class */ (function () {
    function FullScreen(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    FullScreen.prototype.showFullScreen = function (event) {
        var _this = this;
        if (this.parent.toolbarSettings.enable === true && this.parent.editorMode !== 'Markdown') {
            this.parent.quickToolbarModule.hideQuickToolbars();
        }
        this.scrollableParent = sf.popups.getScrollableParent(this.parent.element);
        if (this.parent.actionBeginEnabled) {
            this.parent.dotNetRef.invokeMethodAsync(
            // @ts-ignore-start
            actionBeginEvent, { requestType: 'Maximize', cancel: false }).then(function (fullScreenArgs) {
                // @ts-ignore-end
                if (!fullScreenArgs.cancel) {
                    _this.showActionBeginCallback();
                }
            });
        }
        else {
            this.showActionBeginCallback();
        }
        setTimeout(function () {
            _this.parent.setContentHeight();
            _this.parent.inputElement.focus();
        }, 100);
    };
    FullScreen.prototype.showActionBeginCallback = function () {
        this.toggleParentOverflow(true);
        this.parent.setContentHeight();
        this.invokeActionComplete('Maximize');
    };
    FullScreen.prototype.hideFullScreen = function (event) {
        var _this = this;
        if (this.parent.toolbarSettings.enable === true && this.parent.editorMode !== 'Markdown') {
            this.parent.quickToolbarModule.hideQuickToolbars();
        }
        var elem = document.querySelectorAll('.' + CLS_RTE_OVERFLOW);
        for (var i = 0; i < elem.length; i++) {
            sf.base.removeClass([elem[i]], [CLS_RTE_OVERFLOW]);
        }
        if (this.parent.actionBeginEnabled) {
            this.parent.dotNetRef.invokeMethodAsync(
            // @ts-ignore-start
            actionBeginEvent, { requestType: 'Minimize', cancel: false }).then(function (fullScreenArgs) {
                // @ts-ignore-end
                if (!fullScreenArgs.cancel) {
                    _this.hideActionBeginCallback();
                }
            });
        }
        else {
            this.hideActionBeginCallback();
        }
        setTimeout(function () {
            _this.parent.setContentHeight();
            _this.parent.inputElement.focus();
        }, 100);
    };
    FullScreen.prototype.hideActionBeginCallback = function () {
        this.parent.setContentHeight();
        this.invokeActionComplete('Minimize');
    };
    FullScreen.prototype.invokeActionComplete = function (type) {
        if (this.parent.actionCompleteEnabled) {
            this.parent.dotNetRef.invokeMethodAsync(actionCompleteEvent, { requestType: type });
        }
    };
    FullScreen.prototype.toggleParentOverflow = function (isAdd) {
        if (sf.base.isNullOrUndefined(this.scrollableParent)) {
            return;
        }
        for (var i = 0; i < this.scrollableParent.length; i++) {
            if (this.scrollableParent[i].nodeName === '#document') {
                var elem = document.querySelector('body');
                sf.base.addClass([elem], [CLS_RTE_OVERFLOW]);
            }
            else {
                var elem = this.scrollableParent[i];
                sf.base.addClass([elem], [CLS_RTE_OVERFLOW]);
            }
        }
    };
    FullScreen.prototype.onKeyDown = function (event) {
        var originalEvent = event.args;
        switch (originalEvent.action) {
            case 'full-screen':
                this.parent.dotNetRef.invokeMethodAsync(showFullScreenClient);
                this.showFullScreen(event.args);
                originalEvent.preventDefault();
                break;
            case 'escape':
                this.parent.dotNetRef.invokeMethodAsync(hideFullScreenClient);
                this.hideFullScreen(event.args);
                originalEvent.preventDefault();
                break;
        }
    };
    FullScreen.prototype.addEventListener = function () {
        this.parent.observer.on(enableFullScreen, this.showFullScreen, this);
        this.parent.observer.on(disableFullScreen, this.hideFullScreen, this);
        this.parent.observer.on(keyDown, this.onKeyDown, this);
        this.parent.observer.on(destroy, this.destroy, this);
    };
    FullScreen.prototype.removeEventListener = function () {
        this.parent.observer.off(enableFullScreen, this.showFullScreen);
        this.parent.observer.off(disableFullScreen, this.hideFullScreen);
        this.parent.observer.off(keyDown, this.onKeyDown);
        this.parent.observer.off(destroy, this.destroy);
    };
    FullScreen.prototype.destroy = function () {
        if (this.parent.element.classList.contains(CLS_FULL_SCREEN)) {
            this.toggleParentOverflow(false);
        }
        var elem = document.querySelectorAll('.' + CLS_RTE_OVERFLOW);
        for (var i = 0; i < elem.length; i++) {
            sf.base.removeClass([elem[i]], [CLS_RTE_OVERFLOW]);
        }
        this.removeEventListener();
    };
    return FullScreen;
}());

/**
 * `EnterKey` module is used to handle enter key press actions.
 */
var EnterKeyAction = /** @class */ (function () {
    function EnterKeyAction(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    EnterKeyAction.prototype.addEventListener = function () {
        this.parent.observer.on(enterHandler, this.enterHandler, this);
        this.parent.observer.on(destroy, this.destroy, this);
    };
    EnterKeyAction.prototype.destroy = function () {
        this.removeEventListener();
    };
    EnterKeyAction.prototype.removeEventListener = function () {
        this.parent.observer.off(enterHandler, this.enterHandler);
        this.parent.observer.off(destroy, this.destroy);
    };
    EnterKeyAction.prototype.getRangeNode = function () {
        this.range = this.parent.getRange();
        this.startNode = this.range.startContainer.nodeName === '#text' ? this.range.startContainer.parentElement :
            this.range.startContainer;
        this.endNode = this.range.endContainer.nodeName === '#text' ? this.range.endContainer.parentElement :
            this.range.endContainer;
    };
    EnterKeyAction.prototype.enterHandler = function (e) {
        this.getRangeNode();
        var isTableEnter = true;
        if (!sf.base.isNullOrUndefined(this.startNode.closest('TABLE')) && !sf.base.isNullOrUndefined(this.endNode.closest('TABLE'))) {
            isTableEnter = false;
            var curElement = this.startNode;
            var blockElement = curElement;
            while (!this.parent.formatter.editorManager.domNode.isBlockNode(curElement)) {
                blockElement = curElement;
                curElement = curElement.parentElement;
            }
            isTableEnter = blockElement.tagName === 'TD' ? false : true;
        }
        if (e.args.which === 13 && e.args.code === 'Enter') {
            if (sf.base.isNullOrUndefined(this.startNode.closest('LI')) && sf.base.isNullOrUndefined(this.endNode.closest('LI')) && isTableEnter &&
                sf.base.isNullOrUndefined(this.startNode.closest('PRE')) && sf.base.isNullOrUndefined(this.endNode.closest('PRE'))) {
                var shiftKey = e.args.shiftKey;
                if (!(this.range.startOffset === this.range.endOffset && this.range.startContainer === this.range.endContainer)) {
                    this.range.deleteContents();
                    if (this.range.startContainer.nodeName === '#text' && this.range.startContainer.textContent.length === 0 &&
                        this.range.startContainer.parentElement !== this.parent.inputElement) {
                        if (this.parent.enterKey === 'BR') {
                            this.range.startContainer.parentElement.innerHTML = '&#8203;';
                        }
                        else {
                            this.range.startContainer.parentElement.innerHTML = '<br>';
                        }
                    }
                    else if (this.range.startContainer === this.parent.inputElement && this.range.startContainer.innerHTML === '') {
                        this.range.startContainer.innerHTML = '<br>';
                        var focusElem = this.range.startContainer.childNodes[this.range.startOffset];
                        this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), focusElem, 0);
                    }
                    else if (this.parent.inputElement === this.range.startContainer) {
                        var focusElem = this.range.startContainer.childNodes[this.range.startOffset];
                        if (focusElem.nodeName === '#text' && focusElem.textContent.length === 0) {
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), focusElem, focusElem.previousSibling.textContent.length);
                        }
                        else {
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), focusElem, focusElem.textContent.length >= 0 ? 0 : 1);
                            if (focusElem.previousSibling.textContent.length === 0) {
                                sf.base.detach(focusElem.previousSibling);
                            }
                            else if (focusElem.textContent.length === 0) {
                                var currentFocusElem = focusElem.previousSibling.lastChild;
                                var finalFocusElem = void 0;
                                while (currentFocusElem.nodeName !== '#text') {
                                    finalFocusElem = currentFocusElem;
                                    currentFocusElem = currentFocusElem.lastChild;
                                }
                                this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), currentFocusElem, currentFocusElem.textContent.length);
                                sf.base.detach(focusElem);
                            }
                            else if (this.parent.enterKey !== 'BR' &&
                                focusElem.previousSibling.textContent.length !== 0 && focusElem.textContent.length !== 0) {
                                e.args.preventDefault();
                                return;
                            }
                        }
                        this.getRangeNode();
                    }
                }
                if (this.range.startContainer === this.range.endContainer &&
                    this.range.startOffset === this.range.endOffset && this.range.startContainer === this.parent.inputElement) {
                    this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), this.range.startContainer.childNodes[this.range.startOffset], 0);
                    this.getRangeNode();
                }
                if (this.parent.enterKey === 'P' || this.parent.enterKey === 'DIV' ||
                    (this.parent.shiftEnterKey === 'P' && shiftKey) ||
                    (this.parent.shiftEnterKey === 'DIV' && shiftKey)) {
                    var nearBlockNode = void 0;
                    if (isTableEnter && this.parent.formatter.editorManager.domNode.isBlockNode(this.startNode)) {
                        nearBlockNode = this.startNode;
                    }
                    else {
                        nearBlockNode = this.parent.formatter.editorManager.domNode.blockParentNode(this.startNode);
                    }
                    var isImageNode = false;
                    if (this.range.startOffset === 0 && this.range.endOffset === 0 && !(!sf.base.isNullOrUndefined(this.range.startContainer.previousSibling) && this.range.startContainer.previousSibling.nodeName === 'IMG')) {
                        var isNearBlockLengthZero = void 0;
                        var newElem = void 0;
                        if (this.range.startContainer.nodeName === 'IMG') {
                            newElem = this.createInsertElement(shiftKey);
                            isImageNode = true;
                            isNearBlockLengthZero = false;
                        }
                        else {
                            if (nearBlockNode.textContent.length !== 0) {
                                newElem = this.parent.formatter.editorManager.nodeCutter.SplitNode(this.range, nearBlockNode, false).cloneNode(true);
                                isNearBlockLengthZero = false;
                            }
                            else {
                                newElem = this.parent.formatter.editorManager.nodeCutter.SplitNode(this.range, nearBlockNode, true).cloneNode(true);
                                isNearBlockLengthZero = true;
                            }
                        }
                        var insertElem = this.createInsertElement(shiftKey);
                        while (newElem.firstChild) {
                            insertElem.appendChild(newElem.firstChild);
                        }
                        nearBlockNode.parentElement.insertBefore(insertElem, nearBlockNode);
                        if (!isNearBlockLengthZero) {
                            var currentFocusElem = insertElem;
                            var finalFocusElem = void 0;
                            while (!sf.base.isNullOrUndefined(currentFocusElem) && currentFocusElem.nodeName !== '#text') {
                                finalFocusElem = currentFocusElem;
                                currentFocusElem = currentFocusElem.lastChild;
                            }
                            finalFocusElem.innerHTML = '<br>';
                            if (!isImageNode) {
                                sf.base.detach(nearBlockNode);
                            }
                        }
                    }
                    else if (nearBlockNode.textContent.length === 0 && !(!sf.base.isNullOrUndefined(nearBlockNode.childNodes[0]) && nearBlockNode.childNodes[0].nodeName === 'IMG')) {
                        if (!sf.base.isNullOrUndefined(nearBlockNode.children[0]) && nearBlockNode.children[0].tagName !== 'BR') {
                            var newElem = this.parent.formatter.editorManager.nodeCutter.SplitNode(this.range, nearBlockNode, false).cloneNode(true);
                            this.parent.formatter.editorManager.domNode.insertAfter(newElem, nearBlockNode);
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), newElem, newElem.textContent.length >= 0 ? 0 : 1);
                        }
                        else {
                            var insertElem = this.createInsertElement(shiftKey);
                            insertElem.innerHTML = '<br>';
                            this.parent.formatter.editorManager.domNode.insertAfter(insertElem, nearBlockNode);
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), insertElem, 0);
                        }
                    }
                    else {
                        var newElem = this.parent.formatter.editorManager.nodeCutter.SplitNode(this.range, nearBlockNode, true);
                        if (!sf.base.isNullOrUndefined(newElem.childNodes[0]) && newElem.childNodes[0].nodeName === '#text' &&
                            newElem.childNodes[0].textContent.length === 0) {
                            sf.base.detach(newElem.childNodes[0]);
                        }
                        if (newElem.textContent.trim().length === 0) {
                            var brElm = sf.base.createElement('br');
                            this.startNode.appendChild(brElm);
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), this.startNode, 0);
                        }
                        if (((this.parent.enterKey === 'P' || this.parent.enterKey === 'DIV') && !shiftKey) || ((this.parent.shiftEnterKey === 'DIV' ||
                            this.parent.shiftEnterKey === 'P') && shiftKey)) {
                            var insertElm = this.createInsertElement(shiftKey);
                            while (newElem.firstChild) {
                                insertElm.appendChild(newElem.firstChild);
                            }
                            this.parent.formatter.editorManager.domNode.insertAfter(insertElm, newElem);
                            sf.base.detach(newElem);
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), insertElm, insertElm.textContent.length >= 0 ? 0 : 1);
                        }
                    }
                    e.args.preventDefault();
                }
                if (this.parent.enterKey === 'BR' && !shiftKey) {
                    var currentParent = void 0;
                    if (!this.parent.formatter.editorManager.domNode.isBlockNode(this.startNode)) {
                        var currentNode = this.startNode;
                        var previousNode = currentNode;
                        while (!this.parent.formatter.editorManager.domNode.isBlockNode(currentNode)) {
                            previousNode = currentNode;
                            currentNode = currentNode.parentElement;
                        }
                        currentParent = currentNode === this.parent.inputElement ?
                            previousNode : currentNode;
                    }
                    else {
                        currentParent = this.startNode;
                    }
                    var isEmptyBrInserted = false;
                    if (currentParent !== this.parent.inputElement &&
                        this.parent.formatter.editorManager.domNode.isBlockNode(currentParent) &&
                        this.range.startOffset === this.range.endOffset &&
                        this.range.startOffset === currentParent.textContent.length) {
                        var outerBRElem = sf.base.createElement('br');
                        this.parent.formatter.editorManager.domNode.insertAfter(outerBRElem, currentParent);
                        this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), outerBRElem, 0);
                    }
                    else if (!sf.base.isNullOrUndefined(currentParent) && currentParent !== this.parent.inputElement && currentParent.nodeName !== 'BR') {
                        if (currentParent.textContent.trim().length === 0 || (currentParent.textContent.trim().length === 1 &&
                            currentParent.textContent.charCodeAt(0) === 8203)) {
                            var newElem = this.parent.formatter.editorManager.nodeCutter.SplitNode(this.range, currentParent, true).cloneNode(true);
                            this.parent.formatter.editorManager.domNode.insertAfter(newElem, currentParent);
                            var outerBRElem = sf.base.createElement('br');
                            newElem.parentElement.insertBefore(outerBRElem, newElem);
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), newElem, 0);
                        }
                        else {
                            var newElem = void 0;
                            var focusElem = void 0;
                            var outerBRElem = sf.base.createElement('br');
                            if (this.range.startOffset === 0 && this.range.endOffset === 0 &&
                                !sf.base.isNullOrUndefined(currentParent.previousSibling) && currentParent.previousSibling.nodeName === 'BR') {
                                focusElem = this.range.startContainer;
                                newElem = this.parent.formatter.editorManager.nodeCutter.SplitNode(this.range, currentParent, false).cloneNode(true);
                                this.parent.formatter.editorManager.domNode.insertAfter(outerBRElem, currentParent);
                                this.insertFocusContent();
                                var currentFocusElem = outerBRElem.nextSibling;
                                var finalFocusElem = void 0;
                                while (!sf.base.isNullOrUndefined(currentFocusElem) && currentFocusElem.nodeName !== '#text') {
                                    finalFocusElem = currentFocusElem;
                                    currentFocusElem = currentFocusElem.lastChild;
                                }
                                this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), currentFocusElem, 0);
                                isEmptyBrInserted = true;
                            }
                            else {
                                newElem = this.parent.formatter.editorManager.nodeCutter.SplitNode(this.range, currentParent, true);
                                newElem.parentElement.insertBefore(outerBRElem, newElem);
                                this.insertFocusContent();
                            }
                        }
                    }
                    else {
                        var brElm = sf.base.createElement('br');
                        if (this.startNode.nodeName === 'BR' && this.endNode.nodeName === 'BR' && this.range.startOffset === 0 && this.range.startOffset === this.range.endOffset) {
                            this.parent.formatter.editorManager.domNode.insertAfter(brElm, this.startNode);
                            isEmptyBrInserted = true;
                        }
                        else {
                            if (this.startNode === this.parent.inputElement && !sf.base.isNullOrUndefined(this.range.startContainer.previousSibling) &&
                                this.range.startContainer.previousSibling.nodeName === 'BR' && this.range.startContainer.textContent.length === 0) {
                                isEmptyBrInserted = true;
                            }
                            this.range.insertNode(brElm);
                        }
                        if (isEmptyBrInserted || (!sf.base.isNullOrUndefined(brElm.nextElementSibling) && brElm.nextElementSibling.tagName === 'BR') || (!sf.base.isNullOrUndefined(brElm.nextSibling) && brElm.nextSibling.textContent.length > 0)) {
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), !sf.base.isNullOrUndefined(brElm.nextSibling) ? brElm.nextSibling : brElm, 0);
                            isEmptyBrInserted = false;
                        }
                        else {
                            var brElm2 = sf.base.createElement('br');
                            this.range.insertNode(brElm2);
                            this.parent.formatter.editorManager.nodeSelection.setCursorPoint(this.parent.getDocument(), brElm, 0);
                        }
                    }
                    e.args.preventDefault();
                }
                var actionCompleteArgs = {
                    name: actionComplete,
                    requestType: shiftKey ? 'ShiftEnterAction' : 'EnterAction',
                    event: e.args
                };
                // @ts-ignore-start
                this.parent.dotNetRef.invokeMethodAsync('ActionCompleteEvent', actionCompleteArgs);
                // @ts-ignore-end
            }
        }
    };
    EnterKeyAction.prototype.insertFocusContent = function () {
        if (this.range.startContainer.textContent.length === 0) {
            if (this.range.startContainer.nodeName === '#text') {
                this.range.startContainer.parentElement.innerHTML = '&#8203;';
            }
            else {
                this.range.startContainer.innerHTML = '&#8203;';
            }
        }
    };
    EnterKeyAction.prototype.createInsertElement = function (shiftKey) {
        var insertElem;
        if ((this.parent.enterKey === 'DIV' && !shiftKey) || (this.parent.shiftEnterKey === 'DIV' && shiftKey)) {
            insertElem = sf.base.createElement('div');
        }
        else if ((this.parent.enterKey === 'P' && !shiftKey) || (this.parent.shiftEnterKey === 'P' && shiftKey)) {
            insertElem = sf.base.createElement('p');
        }
        return insertElem;
    };
    return EnterKeyAction;
}());

var keyCode = {
    'backspace': 8,
    'tab': 9,
    'enter': 13,
    'shift': 16,
    'control': 17,
    'alt': 18,
    'pause': 19,
    'capslock': 20,
    'space': 32,
    'escape': 27,
    'pageup': 33,
    'pagedown': 34,
    'end': 35,
    'home': 36,
    'leftarrow': 37,
    'uparrow': 38,
    'rightarrow': 39,
    'downarrow': 40,
    'insert': 45,
    'delete': 46,
    'f1': 112,
    'f2': 113,
    'f3': 114,
    'f4': 115,
    'f5': 116,
    'f6': 117,
    'f7': 118,
    'f8': 119,
    'f9': 120,
    'f10': 121,
    'f11': 122,
    'f12': 123,
    'semicolon': 186,
    'plus': 187,
    'comma': 188,
    'minus': 189,
    'dot': 190,
    'forwardslash': 191,
    'graveaccent': 192,
    'openbracket': 219,
    'backslash': 220,
    'closebracket': 221,
    'singlequote': 222,
    ']': 221,
    '[': 219,
    '=': 187
};
/**
 * Keyboard
 */
var KeyboardEvents$1 = /** @class */ (function () {
    function KeyboardEvents$$1(element, options) {
        var _this = this;
        this.keyPressHandler = function (e) {
            var isAltKey = e.altKey;
            var isCtrlKey = e.ctrlKey;
            var isShiftKey = e.shiftKey;
            var isMetaKey = e.metaKey;
            var curkeyCode = e.which;
            var keys = Object.keys(_this.keyConfigs);
            for (var _i = 0, keys_1 = keys; _i < keys_1.length; _i++) {
                var key = keys_1[_i];
                var configCollection = _this.keyConfigs[key].split(',');
                for (var _a = 0, configCollection_1 = configCollection; _a < configCollection_1.length; _a++) {
                    var rconfig = configCollection_1[_a];
                    var rKeyObj = KeyboardEvents$$1.getKeyConfigData(rconfig.trim());
                    if (isAltKey === rKeyObj.altKey && (isCtrlKey === rKeyObj.ctrlKey || isMetaKey) &&
                        isShiftKey === rKeyObj.shiftKey && curkeyCode === rKeyObj.keyCode) {
                        e.action = key;
                    }
                }
            }
            if (_this.keyAction) {
                _this.keyAction(e);
            }
        };
        this.element = element;
        sf.base.extend(this, this, options);
        this.onKeyPressHandler = this.keyPressHandler.bind(this);
        this.bind();
    }
    KeyboardEvents$$1.prototype.destroy = function () {
        this.unWireEvents();
    };
    KeyboardEvents$$1.prototype.bind = function () {
        this.wireEvents();
    };
    KeyboardEvents$$1.prototype.wireEvents = function () {
        this.element.addEventListener(this.eventName, this.onKeyPressHandler);
    };
    KeyboardEvents$$1.prototype.unWireEvents = function () {
        this.element.removeEventListener(this.eventName, this.onKeyPressHandler);
    };
    KeyboardEvents$$1.getKeyConfigData = function (config) {
        if (config in this.configCache) {
            return this.configCache[config];
        }
        var keys = config.toLowerCase().split('+');
        var keyData = {
            altKey: (keys.indexOf('alt') !== -1 ? true : false),
            ctrlKey: (keys.indexOf('ctrl') !== -1 ? true : false),
            shiftKey: (keys.indexOf('shift') !== -1 ? true : false),
            keyCode: null
        };
        if (keys[keys.length - 1].length > 1 && !!Number(keys[keys.length - 1])) {
            keyData.keyCode = Number(keys[keys.length - 1]);
        }
        else {
            keyData.keyCode = KeyboardEvents$$1.getKeyCode(keys[keys.length - 1]);
        }
        KeyboardEvents$$1.configCache[config] = keyData;
        return keyData;
    };
    KeyboardEvents$$1.getKeyCode = function (keyVal) {
        return keyCode[keyVal] || keyVal.toUpperCase().charCodeAt(0);
    };
    KeyboardEvents$$1.configCache = {};
    return KeyboardEvents$$1;
}());

/**
 * Content module is used to render Rich Text Editor content
 */
var ViewSource = /** @class */ (function () {
    function ViewSource(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    ViewSource.prototype.addEventListener = function () {
        this.parent.observer.on(sourceCode$1, this.sourceCode, this);
        this.parent.observer.on(initialEnd$1, this.onInitialEnd, this);
        this.parent.observer.on(updateSource$1, this.updateSourceCode, this);
        this.parent.observer.on(destroy$1, this.destroy, this);
    };
    ViewSource.prototype.onInitialEnd = function () {
        this.parent.formatter.editorManager.observer.on('keydown-handler', this.onKeyDown, this);
    };
    ViewSource.prototype.removeEventListener = function () {
        this.unWireEvent();
        this.parent.observer.off(sourceCode$1, this.sourceCode);
        this.parent.observer.off(updateSource$1, this.updateSourceCode);
        this.parent.observer.off(initialEnd$1, this.onInitialEnd);
        this.parent.observer.off(destroy$1, this.destroy);
        this.parent.formatter.editorManager.observer.off('keydown-handler', this.onKeyDown);
    };
    ViewSource.prototype.getSourceCode = function () {
        return sf.base.createElement('textarea', { className: 'e-rte-srctextarea' });
    };
    ViewSource.prototype.wireEvent = function (element) {
        this.keyboardModule = new KeyboardEvents$1(element, {
            keyAction: this.previewKeyDown.bind(this), keyConfigs: this.parent.formatter.keyConfig, eventName: 'keydown'
        });
        sf.base.EventHandler.add(this.previewElement, 'mousedown', this.mouseDownHandler, this);
    };
    ViewSource.prototype.unWireEvent = function () {
        if (this.keyboardModule) {
            this.keyboardModule.destroy();
        }
        if (this.previewElement) {
            sf.base.EventHandler.remove(this.previewElement, 'mousedown', this.mouseDownHandler);
        }
    };
    ViewSource.prototype.wireBaseKeyDown = function () {
        this.keyboardModule = new KeyboardEvents$1(this.parent.getEditPanel(), {
            keyAction: this.parent.keyDown.bind(this.parent), keyConfigs: this.parent.formatter.keyConfig, eventName: 'keydown'
        });
    };
    ViewSource.prototype.unWireBaseKeyDown = function () {
        this.parent.keyboardModule.destroy();
    };
    ViewSource.prototype.mouseDownHandler = function (e) {
        this.parent.observer.notify(sourceCodeMouseDown$1, { args: e });
    };
    ViewSource.prototype.previewKeyDown = function (event) {
        switch (event.action) {
            case 'html-source':
                this.parent.dotNetRef.invokeMethodAsync('PreviewCodeClient');
                this.updateSourceCode(event);
                event.preventDefault();
                break;
            case 'toolbar-focus':
                if (this.parent.toolbarSettings.enable) {
                    var selector = '.e-toolbar-item[aria-disabled="false"][title] [tabindex]';
                    this.parent.getToolbar().querySelector(selector).focus();
                }
                break;
        }
    };
    ViewSource.prototype.onKeyDown = function (e) {
        switch (e.event.action) {
            case 'html-source':
                e.event.preventDefault();
                this.parent.dotNetRef.invokeMethodAsync('ViewSourceClient');
                this.sourceCode(e);
                e.callBack({
                    requestType: 'SourceCode',
                    editorMode: 'HTML',
                    event: e.event
                });
                break;
        }
    };
    ViewSource.prototype.sourceCode = function (args) {
        var _this = this;
        this.parent.isBlur = false;
        // @ts-ignore-start
        this.parent.dotNetRef.invokeMethodAsync('ActionBeginEvent', { requestType: 'SourceCode', cancel: false }).then(function (sourceArgs) {
            // @ts-ignore-end
            if (!sourceArgs.cancel) {
                var tbItems = sf.base.selectAll('.' + CLS_TB_ITEM$1, _this.parent.element);
                if (sf.base.isNullOrUndefined(_this.previewElement)) {
                    _this.previewElement = _this.getSourceCode();
                }
                _this.parent.updateValueData();
                if (_this.parent.iframeSettings.enable) {
                    var rteContent = void 0;
                    if (sf.base.isNullOrUndefined(_this.parent.element.querySelector('#' + _this.parent.element.id + '_source-view'))) {
                        rteContent = sf.base.createElement('div', {
                            className: 'e-source-content', id: _this.parent.element.id + '_source-view'
                        });
                    }
                    else {
                        rteContent = _this.parent.element.querySelector('#' + _this.parent.element.id + '_source-view');
                    }
                    rteContent.appendChild(_this.previewElement);
                    _this.parent.element.appendChild(rteContent);
                    rteContent.style.height = _this.parent.getPanel().style.height;
                    rteContent.style.marginTop = _this.parent.getPanel().style.marginTop;
                    _this.getPanel().value = _this.getTextAreaValue(_this.parent.getEditPanel());
                    _this.parent.getPanel().style.display = 'none';
                    rteContent.style.display = 'block';
                    _this.getPanel().style.display = 'block';
                }
                else {
                    _this.parent.inputElement.parentElement.appendChild(_this.previewElement);
                    _this.getPanel().value = _this.getTextAreaValue(_this.parent.getEditPanel());
                    _this.parent.inputElement.style.display = 'none';
                    _this.previewElement.style.display = 'block';
                }
                _this.parent.isBlur = false;
                if (_this.parent.getToolbar()) {
                    sf.base.removeClass([_this.parent.getToolbar()], [CLS_EXPAND_OPEN$1]);
                }
                sf.base.removeClass(tbItems, [CLS_ACTIVE$1]);
                _this.parent.setContentHeight('sourceCode', true);
                _this.wireEvent(_this.previewElement);
                _this.unWireBaseKeyDown();
                _this.previewElement.focus();
                _this.parent.updateValue();
                if (!sf.base.isNullOrUndefined(_this.parent.placeholder) && !_this.parent.iframeSettings.enable) {
                    var placeHolderContainer = _this.parent.element.querySelector('.rte-placeholder');
                    placeHolderContainer.style.display = 'none';
                }
                _this.parent.dotNetRef.invokeMethodAsync(
                // @ts-ignore-start
                'ActionCompleteEvent', { requestType: 'SourceCode' }).then(function (completeArgs) {
                    // @ts-ignore-end
                    _this.parent.invokeChangeEvent();
                });
            }
        });
    };
    ViewSource.prototype.updateSourceCode = function (args) {
        var _this = this;
        this.parent.isBlur = false;
        // @ts-ignore-start
        this.parent.dotNetRef.invokeMethodAsync('ActionBeginEvent', { requestType: 'Preview', cancel: false }).then(function (previewArgs) {
            // @ts-ignore-end
            if (!previewArgs.cancel) {
                var editHTML = _this.getPanel();
                var serializeValue = _this.parent.serializeValue(editHTML.value);
                var value = void 0;
                if (serializeValue === null || serializeValue === '') {
                    if (_this.parent.enterKey === 'DIV') {
                        value = '<div><br/></div>';
                    }
                    else if (_this.parent.enterKey === 'BR') {
                        value = '<br/>';
                    }
                    else {
                        value = '<p><br/></p>';
                    }
                }
                else {
                    value = serializeValue;
                }
                if (_this.parent.iframeSettings.enable) {
                    editHTML.parentElement.style.display = 'none';
                    editHTML.style.display = 'none';
                    _this.parent.contentPanel.style.display = 'block';
                    _this.parent.getEditPanel().innerHTML = _this.parent.enableHtmlEncode ? decode(value) : value;
                }
                else {
                    editHTML.style.display = 'none';
                    _this.parent.getEditPanel().style.display = 'block';
                    _this.parent.getEditPanel().innerHTML = _this.parent.enableHtmlEncode ? decode(value) : value;
                }
                _this.parent.isBlur = false;
                if (_this.parent.getToolbar()) {
                    sf.base.removeClass([_this.parent.getToolbar()], [CLS_EXPAND_OPEN$1]);
                }
                _this.parent.setContentHeight('preview', true);
                _this.unWireEvent();
                _this.wireBaseKeyDown();
                _this.parent.getEditPanel().focus();
                _this.parent.updateValue();
                if (!sf.base.isNullOrUndefined(_this.parent.placeholder) && _this.parent.getEditPanel().innerText.length === 0) {
                    var placeHolderContainer = _this.parent.element.querySelector('.rte-placeholder');
                    placeHolderContainer.style.display = 'block';
                }
                _this.parent.dotNetRef.invokeMethodAsync(
                // @ts-ignore-start
                'ActionCompleteEvent', { requestType: 'Preview' }).then(function (previewArgs) {
                    // @ts-ignore-end
                    _this.parent.invokeChangeEvent();
                });
            }
        });
    };
    ViewSource.prototype.getTextAreaValue = function (element) {
        var currentValue;
        currentValue = this.parent.enableXhtml ? this.parent.getXhtmlString(this.parent.value) : this.parent.value;
        return (element.innerHTML === '<p><br></p>') || (element.innerHTML === '<div><br></div>') ||
            (element.innerHTML === '<br>') ||
            (element.childNodes.length === 1 &&
                ((element.childNodes[0].tagName === 'P' &&
                    element.innerHTML.length === 7) || (element.childNodes[0].tagName === 'DIV' &&
                    element.innerHTML.length === 11))) ? '' : currentValue;
    };
    ViewSource.prototype.getPanel = function () {
        return this.parent.element.querySelector('.e-rte-srctextarea');
    };
    ViewSource.prototype.getViewPanel = function () {
        return (this.parent.iframeSettings.enable && this.getPanel()) ? this.getPanel().parentElement : this.getPanel();
    };
    ViewSource.prototype.destroy = function () {
        this.removeEventListener();
    };
    return ViewSource;
}());

/**
 * `Popup renderer` module is used to render popup in RichTextEditor.
 */
var PopupRenderer = /** @class */ (function () {
    function PopupRenderer(parent) {
        this.parent = parent;
    }
    PopupRenderer.prototype.quickToolbarOpen = function () {
        this.parent.dotNetRef.invokeMethodAsync('QuickToolbarOpenEvent', this.popupObj.element.classList.toString(), this.targetType);
    };
    PopupRenderer.prototype.renderPopup = function (args, type) {
        this.targetType = type;
        args.popupObj = new sf.popups.Popup(args.element, {
            targetType: 'relative',
            relateTo: this.parent.element,
            open: this.quickToolbarOpen.bind(this)
        });
        this.popupObj = args.popupObj;
        args.popupObj.hide();
    };
    return PopupRenderer;
}());

/**
 * `Quick toolbar` module is used to handle Quick toolbar actions.
 */
var BaseQuickToolbar = /** @class */ (function () {
    function BaseQuickToolbar(parent) {
        this.parent = parent;
        this.isDOMElement = false;
        this.popupRenderer = new PopupRenderer(parent);
    }
    BaseQuickToolbar.prototype.render = function (element, type) {
        this.element = element;
        this.popupRenderer.renderPopup(this, type);
        this.addEventListener();
    };
    BaseQuickToolbar.prototype.setPosition = function (e) {
        var x;
        var y;
        var imgContainer = sf.base.closest(e.target, '.' + CLS_CAPTION);
        var target = !sf.base.isNullOrUndefined(imgContainer) ? imgContainer : e.target;
        var targetOffsetTop = target.offsetTop;
        var parentOffsetTop = window.pageYOffset + e.parentData.top;
        if ((targetOffsetTop - e.editTop) > e.popHeight) {
            y = parentOffsetTop + e.tBarElementHeight + (targetOffsetTop - e.editTop) - e.popHeight - 5;
        }
        else if (((e.editTop + e.editHeight) - (targetOffsetTop + target.offsetHeight)) > e.popHeight) {
            y = parentOffsetTop + e.tBarElementHeight + (targetOffsetTop - e.editTop) + target.offsetHeight + 5;
        }
        else {
            y = e.y;
        }
        if (target.offsetWidth > e.popWidth) {
            x = (target.offsetWidth / 2) - (e.popWidth / 2) + e.parentData.left + target.offsetLeft;
        }
        else {
            x = e.parentData.left + target.offsetLeft;
        }
        this.popupObj.position.X = ((x + e.popWidth) > e.parentData.right) ? e.parentData.right - e.popWidth : x;
        this.popupObj.position.Y = (y >= 0) ? y : e.y + 5;
        this.popupObj.dataBind();
    };
    BaseQuickToolbar.prototype.checkCollision = function (e, viewPort, type) {
        var x;
        var y;
        var parentTop = e.parentData.top;
        var contentTop = e.windowY + parentTop + e.tBarElementHeight;
        var collision = [];
        if (viewPort === 'document') {
            collision = sf.popups.isCollide(e.popup);
        }
        else {
            collision = sf.popups.isCollide(e.popup, e.parentElement);
        }
        for (var i = 0; i < collision.length; i++) {
            switch (collision[i]) {
                case 'top':
                    if (viewPort === 'document') {
                        y = e.windowY;
                    }
                    else {
                        y = (window.pageYOffset + parentTop) + e.tBarElementHeight;
                    }
                    break;
                case 'bottom':
                    var posY = void 0;
                    if (viewPort === 'document') {
                        if (type === 'inline') {
                            posY = (e.y - e.popHeight - 10);
                        }
                        else {
                            if ((e.windowHeight - (parentTop + e.tBarElementHeight)) > e.popHeight) {
                                if ((contentTop - e.windowHeight) > e.popHeight) {
                                    posY = (contentTop + (e.windowHeight - parentTop)) - e.popHeight;
                                }
                                else {
                                    posY = contentTop;
                                }
                            }
                            else {
                                posY = e.windowY + (parentTop + e.tBarElementHeight);
                            }
                        }
                    }
                    else {
                        if (e.target.tagName !== 'IMG') {
                            posY = (e.parentData.bottom + window.pageYOffset) - e.popHeight - 10;
                        }
                        else {
                            posY = (e.parentData.bottom + window.pageYOffset) - e.popHeight - 5;
                        }
                    }
                    y = posY;
                    break;
                case 'right':
                    if (type === 'inline') {
                        x = window.pageXOffset + (e.windowWidth - (e.popWidth + e.bodyRightSpace + 10));
                    }
                    else {
                        x = e.x - e.popWidth;
                    }
                    break;
                case 'left':
                    if (type === 'inline') {
                        x = 0;
                    }
                    else {
                        x = e.parentData.left;
                    }
                    break;
            }
        }
        this.popupObj.position.X = (x) ? x : this.popupObj.position.X;
        this.popupObj.position.Y = (y) ? y : this.popupObj.position.Y;
        this.popupObj.dataBind();
    };
    BaseQuickToolbar.prototype.showPopup = function (x, y, target, type) {
        var _this = this;
        if (this.parent.onQuickTbOpenEnabled) {
            // @ts-ignore-start
            this.parent.dotNetRef.invokeMethodAsync(beforeQuickToolbarOpenEvent).then(function (args) {
                // @ts-ignore-end
                if (!args.cancel) {
                    _this.onQuickTbOpenCallback(x, y, target, type);
                }
            });
        }
        else {
            this.onQuickTbOpenCallback(x, y, target, type);
        }
    };
    BaseQuickToolbar.prototype.onQuickTbOpenCallback = function (x, y, target, type) {
        var editPanelTop;
        var editPanelHeight;
        var bodyStyle = window.getComputedStyle(document.body);
        var bodyRight = parseFloat(bodyStyle.marginRight.split('px')[0]) + parseFloat(bodyStyle.paddingRight.split('px')[0]);
        var windowHeight = window.innerHeight;
        var windowWidth = window.innerWidth;
        var parent = this.parent.element;
        var toolbarAvail = !sf.base.isNullOrUndefined(this.parent.getToolbar());
        var tbHeight = toolbarAvail && this.parent.toolbarModule.getToolbarHeight();
        var expTBHeight = toolbarAvail && this.parent.toolbarModule.getExpandTBarPopHeight();
        var tBarHeight = (toolbarAvail) ? (tbHeight + expTBHeight) : 0;
        sf.base.addClass([this.element], [CLS_HIDE]);
        if (sf.base.Browser.isDevice && !isIDevice()) {
            sf.base.addClass([this.parent.getToolbar()], [CLS_HIDE]);
        }
        if (this.parent.iframeSettings.enable) {
            var cntEle = this.parent.getPanel().contentWindow;
            editPanelTop = cntEle.pageYOffset;
            editPanelHeight = cntEle.innerHeight;
        }
        else {
            var cntEle = sf.base.closest(target, '.' + CLS_RTE_CONTENT);
            editPanelTop = (cntEle) ? cntEle.scrollTop : 0;
            editPanelHeight = (cntEle) ? cntEle.offsetHeight : 0;
        }
        if (!this.parent.inlineMode.enable && !sf.base.closest(target, 'table')) {
            // this.parent.disableToolbarItem(this.parent.toolbarSettings.items as string[]);
            // this.parent.enableToolbarItem(['Undo', 'Redo']);
        }
        sf.base.append([this.element], document.body);
        this.popupObj.position.X = x + 20;
        this.popupObj.position.Y = y + 20;
        this.popupObj.dataBind();
        this.popupObj.element.classList.add(CLS_POPUP_OPEN);
        var showPopupData = {
            x: x, y: y,
            target: target,
            editTop: editPanelTop,
            editHeight: editPanelHeight,
            popup: this.popupObj.element,
            popHeight: this.popupObj.element.offsetHeight,
            popWidth: this.popupObj.element.offsetWidth,
            parentElement: parent,
            bodyRightSpace: bodyRight,
            windowY: window.pageYOffset,
            windowHeight: windowHeight,
            windowWidth: windowWidth,
            parentData: parent.getBoundingClientRect(),
            tBarElementHeight: tBarHeight
        };
        if (target.tagName === 'IMG') {
            this.setPosition(showPopupData);
        }
        if (!this.parent.inlineMode.enable) {
            this.checkCollision(showPopupData, 'parent', '');
        }
        this.checkCollision(showPopupData, 'document', ((this.parent.inlineMode.enable) ? 'inline' : ''));
        this.popupObj.element.classList.remove(CLS_POPUP_CLOSE);
        sf.base.removeClass([this.element], [CLS_HIDE]);
        this.popupObj.show({ name: 'ZoomIn', duration: 250 });
        sf.base.setStyleAttribute(this.element, {
            maxWidth: window.outerWidth + 'px'
        });
        sf.base.addClass([this.element], [CLS_POP]);
        this.isDOMElement = true;
        this.parent.dotNetRef.invokeMethodAsync(updateClass, this.popupObj.element.classList.toString(), type);
    };
    BaseQuickToolbar.prototype.hidePopup = function () {
        var viewSourcePanel = this.parent.viewSourceModule.getViewPanel();
        if (sf.base.Browser.isDevice && !isIDevice()) {
            sf.base.removeClass([this.parent.getToolbar()], [CLS_HIDE]);
        }
        if (!sf.base.isNullOrUndefined(this.parent.getToolbar()) && !this.parent.inlineMode.enable) {
            if (sf.base.isNullOrUndefined(viewSourcePanel) || viewSourcePanel.style.display === 'none') {
                //this.parent.enableToolbarItem(this.parent.toolbarSettings.items as string[]);
            }
        }
        this.removeEleFromDOM();
        this.isDOMElement = false;
    };
    BaseQuickToolbar.prototype.removeEleFromDOM = function () {
        this.popupObj.hide();
        this.element.classList.add(CLS_HIDE);
        this.element.classList.add(CLS_RTE_QUICK_POPUP_HIDE);
        if (this.isDOMElement) {
            sf.base.removeClass([this.element], [CLS_POP]);
            this.popupObj.element.removeAttribute('style');
            this.popupObj.destroy();
            if (this.parent.quickTbClosedEnabled) {
                this.parent.dotNetRef.invokeMethodAsync(quickToolbarCloseEvent);
            }
        }
    };
    BaseQuickToolbar.prototype.updateStatus = function (args) {
        var tbElements = sf.base.selectAll('.' + CLS_TB_ITEM, this.element);
        if (tbElements.length <= 0) {
            return;
        }
        var options = {
            args: args,
            dropDownModule: null,
            parent: this.parent,
            tbElements: tbElements,
            /* eslint-disable */
            tbItems: this.parent.toolbarSettings.items
            /* eslint-enable */
        };
        setToolbarStatus(options, true);
        // if (!select('.e-rte-srctextarea', this.parent.element)) {
        //     //updateUndoRedoStatus(this.parent.getBaseToolbarObject(),
        //           this.parent.formatter.editorManager.undoRedoManager.getUndoStatus());
        // }
    };
    BaseQuickToolbar.prototype.addEventListener = function () {
        this.parent.observer.on(destroy, this.destroy, this);
        if (this.parent.inlineMode.enable) {
            this.parent.observer.on(toolbarUpdated, this.updateStatus, this);
        }
    };
    BaseQuickToolbar.prototype.removeEventListener = function () {
        this.parent.observer.off(destroy, this.destroy);
        if (this.parent.inlineMode.enable) {
            this.parent.observer.off(toolbarUpdated, this.updateStatus);
        }
    };
    BaseQuickToolbar.prototype.destroy = function () {
        if (this.popupObj && !this.popupObj.isDestroyed) {
            this.popupObj.destroy();
            this.removeEleFromDOM();
        }
        this.removeEventListener();
    };
    return BaseQuickToolbar;
}());

/**
 * `Quick toolbar` module is used to handle Quick toolbar actions.
 */
var QuickToolbar = /** @class */ (function () {
    function QuickToolbar(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    QuickToolbar.prototype.showImageQTBar = function (x, y, target, type) {
        if (this.parent.readonly || !this.parent.quickToolbarSettings.enable) {
            return;
        }
        var popupTarget = document.querySelector('#' + this.parent.element.id + imageQuickPopup);
        this.imageQTBar = new BaseQuickToolbar(this.parent);
        this.imageQTBar.render(popupTarget, type);
        this.parent.dotNetRef.invokeMethodAsync(showImagePopup, this.imageQTBar.popupObj.element.classList.toString(), type);
        this.imageQTBar.showPopup(x, y, target, type);
    };
    QuickToolbar.prototype.hideImageQTBar = function () {
        if (this.imageQTBar) {
            this.imageQTBar.hidePopup();
            this.imageQTBar = undefined;
            this.parent.dotNetRef.invokeMethodAsync(hideImagePopup);
        }
    };
    QuickToolbar.prototype.showLinkQTBar = function (x, y, target, type) {
        if (this.parent.readonly || !this.parent.quickToolbarSettings.enable) {
            return;
        }
        var popupTarget = document.querySelector('#' + this.parent.element.id + linkQuickPopup);
        this.linkQTBar = new BaseQuickToolbar(this.parent);
        this.linkQTBar.render(popupTarget, type);
        this.parent.dotNetRef.invokeMethodAsync(showLinkPopup, this.linkQTBar.popupObj.element.classList.toString());
        this.linkQTBar.showPopup(x, y, target, type);
    };
    QuickToolbar.prototype.hideLinkQTBar = function () {
        if (this.linkQTBar) {
            this.linkQTBar.hidePopup();
            this.linkQTBar = undefined;
            this.parent.dotNetRef.invokeMethodAsync(hideLinkPopup);
        }
    };
    QuickToolbar.prototype.showTableQTBar = function (x, y, target, type) {
        if (this.parent.readonly || !this.parent.quickToolbarSettings.enable) {
            return;
        }
        var popupTarget = document.querySelector('#' + this.parent.element.id + tableQuickPopup);
        this.tableQTBar = new BaseQuickToolbar(this.parent);
        this.tableQTBar.render(popupTarget, type);
        this.parent.dotNetRef.invokeMethodAsync(showTablePopup, this.tableQTBar.popupObj.element.classList.toString());
        this.tableQTBar.showPopup(x, y, target, type);
    };
    QuickToolbar.prototype.hideTableQTBar = function () {
        if (this.tableQTBar) {
            this.tableQTBar.hidePopup();
            this.tableQTBar = undefined;
            this.parent.dotNetRef.invokeMethodAsync(hideTablePopup);
        }
    };
    QuickToolbar.prototype.showInlineQTBar = function (x, y, target) {
        var _this = this;
        if (this.parent.readonly || !this.parent.inlineMode.enable) {
            return;
        }
        if (this.parent.inlineMode.enable && (!sf.base.Browser.isDevice || isIDevice())) {
            var popupTarget = document.querySelector('#' + this.parent.element.id + inlineQuickPopup);
            this.inlineQTBar = new BaseQuickToolbar(this.parent);
            this.inlineQTBar.render(popupTarget, 'Inline');
            sf.base.EventHandler.add(this.inlineQTBar.element, 'mousedown', this.onMouseDown, this);
            var classes = this.inlineQTBar.popupObj.element.classList.toString();
            // @ts-ignore-start
            this.parent.dotNetRef.invokeMethodAsync(showInlinePopup, classes).then(function () {
                // @ts-ignore-end
                _this.inlineQTBar.showPopup(x, y, target, 'Inline');
            });
        }
    };
    QuickToolbar.prototype.hideInlineQTBar = function () {
        if (this.inlineQTBar) {
            this.inlineQTBar.hidePopup();
            this.inlineQTBar = undefined;
            this.parent.dotNetRef.invokeMethodAsync(hideInlinePopup);
        }
    };
    QuickToolbar.prototype.hideQuickToolbars = function () {
        if (!sf.base.isNullOrUndefined(this.linkQTBar)) {
            this.hideLinkQTBar();
        }
        if (!sf.base.isNullOrUndefined(this.imageQTBar)) {
            this.hideImageQTBar();
        }
        if (!sf.base.isNullOrUndefined(this.tableQTBar)) {
            this.hideTableQTBar();
        }
        if (this.parent.inlineMode.enable && (!sf.base.Browser.isDevice || isIDevice())) {
            this.hideInlineQTBar();
        }
    };
    QuickToolbar.prototype.scrollHandler = function () {
        if (this.parent.quickToolbarSettings.actionOnScroll.toLocaleLowerCase() === 'hide') {
            this.hideQuickToolbars();
        }
    };
    QuickToolbar.prototype.selectionChangeHandler = function (e) {
        var _this = this;
        if (!this.parent.inlineMode.onSelection) {
            return;
        }
        clearTimeout(this.deBouncer);
        this.deBouncer = window.setTimeout(function () { _this.onSelectionChange(e); }, 1000);
    };
    QuickToolbar.prototype.onSelectionChange = function (e) {
        if (!sf.base.isNullOrUndefined(sf.base.select('.' + CLS_INLINE_POP + '.' + CLS_POPUP, document.body))) {
            return;
        }
        var selection = this.parent.getDocument().getSelection();
        if (!selection.isCollapsed) {
            this.mouseUpHandler({ args: e });
        }
    };
    QuickToolbar.prototype.toolbarUpdated = function (args) {
        if (!sf.base.isNullOrUndefined(this.linkQTBar)) {
            this.hideLinkQTBar();
        }
        if (!sf.base.isNullOrUndefined(this.imageQTBar)) {
            this.hideImageQTBar();
        }
        if (!sf.base.isNullOrUndefined(this.tableQTBar)) {
            this.hideTableQTBar();
        }
    };
    QuickToolbar.prototype.deBounce = function (x, y, target) {
        var _this = this;
        clearTimeout(this.deBouncer);
        this.deBouncer = window.setTimeout(function () { _this.showInlineQTBar(x, y, target); }, 1000);
    };
    QuickToolbar.prototype.onMouseDown = function (e) {
        this.parent.isBlur = false;
        this.parent.isRTE = true;
    };
    QuickToolbar.prototype.mouseUpHandler = function (e) {
        if (this.parent.inlineMode.enable && (!sf.base.Browser.isDevice || isIDevice())) {
            var args = e.args.touches ? e.args.changedTouches[0]
                : e.args;
            var range = this.parent.getRange();
            var target = e.args.target;
            var inlinePopEle = sf.base.select('.' + CLS_INLINE_POP, document.body);
            if (sf.base.isNullOrUndefined(inlinePopEle) || inlinePopEle.classList.contains(CLS_HIDE)) {
                if (isIDevice() && e.touchData && e.touchData.prevClientX !== e.touchData.clientX
                    && e.touchData.prevClientY !== e.touchData.clientY) {
                    return;
                }
                this.hideInlineQTBar();
                var parentLeft = this.parent.element.getBoundingClientRect().left;
                this.offsetX = this.parent.iframeSettings.enable ? window.pageXOffset + parentLeft + args.clientX : args.pageX;
                this.offsetY = pageYOffset(args, this.parent.element, this.parent.iframeSettings.enable);
                if (target.nodeName === 'TEXTAREA') {
                    this.showInlineQTBar(this.offsetX, this.offsetY, target);
                }
                else {
                    var closestAnchor = sf.base.closest(target, 'a');
                    target = closestAnchor ? closestAnchor : target;
                    if (target.tagName !== 'IMG' && target.tagName !== 'A' && (!sf.base.closest(target, 'td,th') || !range.collapsed)) {
                        if (this.parent.inlineMode.onSelection && range.collapsed) {
                            return;
                        }
                        this.target = target;
                        this.showInlineQTBar(this.offsetX, this.offsetY, target);
                    }
                }
            }
        }
    };
    QuickToolbar.prototype.keyDownHandler = function () {
        if ((this.parent.inlineMode.enable && (!sf.base.Browser.isDevice || isIDevice()))
            && !sf.base.isNullOrUndefined(sf.base.select('.' + CLS_INLINE_POP + '.' + CLS_POPUP, document))) {
            this.hideInlineQTBar();
        }
    };
    QuickToolbar.prototype.inlineQTBarMouseDownHandler = function () {
        if ((this.parent.inlineMode.enable && (!sf.base.Browser.isDevice || isIDevice()))
            && !sf.base.isNullOrUndefined(sf.base.select('.' + CLS_INLINE_POP + '.' + CLS_POPUP, document))) {
            this.hideInlineQTBar();
        }
    };
    QuickToolbar.prototype.keyUpHandler = function (e) {
        if (this.parent.inlineMode.enable && !sf.base.Browser.isDevice) {
            if (this.parent.inlineMode.onSelection) {
                return;
            }
            var args = e.args;
            this.deBounce(this.offsetX, this.offsetY, args.target);
        }
    };
    QuickToolbar.prototype.onKeyDown = function (e) {
        var args = e.args;
        if (args.which === 8 || args.which === 46) {
            if (this.imageQTBar && !hasClass(this.imageQTBar.element, CLS_POPUP_CLOSE)) {
                this.imageQTBar.hidePopup();
            }
        }
    };
    QuickToolbar.prototype.onIframeMouseDown = function () {
        this.hideQuickToolbars();
        this.hideInlineQTBar();
    };
    QuickToolbar.prototype.addEventListener = function () {
        if (this.parent.inlineMode.enable && this.parent.inlineMode.onSelection && isIDevice()) {
            sf.base.EventHandler.add(this.parent.getDocument(), 'selectionchange', this.selectionChangeHandler, this);
        }
        this.parent.observer.on(toolbarUpdated, this.toolbarUpdated, this);
        this.wireInlineQTBarEvents();
        this.parent.observer.on(scroll, this.scrollHandler, this);
        this.parent.observer.on(contentscroll, this.scrollHandler, this);
        this.parent.observer.on(focusChange, this.hideQuickToolbars, this);
        this.parent.observer.on(iframeMouseDown, this.onIframeMouseDown, this);
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(keyDown, this.onKeyDown, this);
    };
    QuickToolbar.prototype.removeEventListener = function () {
        sf.base.EventHandler.remove(this.parent.getDocument(), 'selectionchange', this.selectionChangeHandler);
        this.parent.observer.off(toolbarUpdated, this.toolbarUpdated);
        this.unWireInlineQTBarEvents();
        this.parent.observer.off(scroll, this.scrollHandler);
        this.parent.observer.off(contentscroll, this.scrollHandler);
        this.parent.observer.off(focusChange, this.hideQuickToolbars);
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(iframeMouseDown, this.onIframeMouseDown);
        this.parent.observer.off(keyDown, this.onKeyDown);
    };
    QuickToolbar.prototype.wireInlineQTBarEvents = function () {
        this.parent.observer.on(mouseUp, this.mouseUpHandler, this);
        this.parent.observer.on(mouseDown, this.inlineQTBarMouseDownHandler, this);
        this.parent.observer.on(keyDown, this.keyDownHandler, this);
        this.parent.observer.on(keyUp, this.keyUpHandler, this);
        this.parent.observer.on(sourceCodeMouseDown, this.mouseUpHandler, this);
    };
    QuickToolbar.prototype.unWireInlineQTBarEvents = function () {
        this.parent.observer.off(mouseUp, this.mouseUpHandler);
        this.parent.observer.off(mouseDown, this.inlineQTBarMouseDownHandler);
        this.parent.observer.off(keyDown, this.keyDownHandler);
        this.parent.observer.off(keyUp, this.keyUpHandler);
        this.parent.observer.off(sourceCodeMouseDown, this.mouseUpHandler);
    };
    QuickToolbar.prototype.destroy = function () {
        if (this.linkQTBar) {
            sf.base.EventHandler.remove(this.linkQTBar.element, 'mousedown', this.onMouseDown);
            this.linkQTBar.destroy();
        }
        if (this.imageQTBar) {
            sf.base.EventHandler.remove(this.imageQTBar.element, 'mousedown', this.onMouseDown);
            this.imageQTBar.destroy();
        }
        if (this.tableQTBar) {
            sf.base.EventHandler.remove(this.tableQTBar.element, 'mousedown', this.onMouseDown);
            this.tableQTBar.destroy();
        }
        if (this.inlineQTBar) {
            sf.base.EventHandler.remove(this.inlineQTBar.element, 'mousedown', this.onMouseDown);
            if (isIDevice()) {
                sf.base.EventHandler.remove(document, 'selectionchange', this.selectionChangeHandler);
            }
            this.inlineQTBar.destroy();
        }
        this.removeEventListener();
    };
    return QuickToolbar;
}());

var RTL = 'e-rtl';
var STATUS = 'e-file-status';
var FILE_NAME = 'e-file-name';
var FILE_TYPE = 'e-file-type';
var FILE_SIZE = 'e-file-size';
var FILE = 'e-upload-file-list';
var FORM_UPLOAD = 'e-form-upload';
var LIST_PARENT = 'e-upload-files';
var SPINNER_PANE = 'e-spinner-pane';
var ABORT_ICON = 'e-file-abort-btn';
var INPUT_WRAPPER = 'e-file-select';
var INVALID_FILE = 'e-file-invalid';
var UPLOAD_FAILED = 'e-upload-fails';
var RETRY_ICON = 'e-file-reload-btn';
var RTL_CONTAINER = 'e-rtl-container';
var REMOVE_ICON = 'e-file-remove-btn';
var DELETE_ICON = 'e-file-delete-btn';
var INFORMATION = 'e-file-information';
var TEXT_CONTAINER = 'e-file-container';
var UPLOAD_SUCCESS = 'e-upload-success';
var RESTRICT_RETRY = 'e-restrict-retry';
var DROP_WRAPPER = 'e-file-select-wrap';
var HIDDEN_INPUT = 'e-hidden-file-input';
var PROGRESSBAR = 'e-upload-progress-bar';
var UPLOAD_INPROGRESS = 'e-upload-progress';
var VALIDATION_FAILS = 'e-validation-fails';
var PROGRESSBAR_TEXT = 'e-progress-bar-text';
var PROGRESS_WRAPPER = 'e-upload-progress-wrap';
var PROGRESS_INNER_WRAPPER = 'e-progress-inner-wrap';
var CONTROL_WRAPPER = 'e-upload e-lib e-control-wrapper';
/**
 * `Uploader` module is used to render uploader from RichTextEditor.
 */
var RteUploader = /** @class */ (function () {
    function RteUploader(options, element, parent) {
        this.fileList = [];
        this.filesData = [];
        this.files = [];
        this.minFileSize = 0;
        this.maxFileSize = 30000000;
        this.uploadedFilesData = [];
        this.base64String = [];
        this.isForm = false;
        this.allTypes = false;
        this.btnTabIndex = '0';
        this.disableKeyboardNavigation = false;
        this.actionCompleteCount = 0;
        this.flag = true;
        this.selectedFiles = [];
        this.uploaderName = 'UploadFiles';
        this.element = element;
        this.parent = parent;
        this.cssClass = options.cssClass;
        this.asyncSettings = options.asyncSettings;
        this.allowedExtensions = options.allowedExtensions;
        this.addEventListener();
        this.render();
    }
    RteUploader.prototype.addEventListener = function () {
        this.parent.observer.on(beforePasteUploadCallBack, this.beforePasteUploadCallBack, this);
    };
    RteUploader.prototype.removeEventListener = function () {
        this.parent.observer.off(beforePasteUploadCallBack, this.beforePasteUploadCallBack);
    };
    RteUploader.prototype.render = function () {
        this.browserName = sf.base.Browser.info.name;
        this.uploaderName = this.element.getAttribute('name');
        this.initializeUpload();
        this.wireEvents();
        this.setExtensions(this.allowedExtensions);
        this.setRTL();
        this.setCSSClass();
    };
    RteUploader.prototype.initializeUpload = function () {
        this.element.setAttribute('aria-label', 'Uploader');
        this.element.setAttribute('tabindex', '-1');
        var inputWrapper = sf.base.createElement('span', { className: INPUT_WRAPPER, attrs: { style: 'display: none;' } });
        this.element.parentElement.insertBefore(inputWrapper, this.element);
        this.dropAreaWrapper = sf.base.createElement('div', { className: DROP_WRAPPER });
        this.element.parentElement.insertBefore(this.dropAreaWrapper, this.element);
        inputWrapper.appendChild(this.element);
        this.dropAreaWrapper.appendChild(inputWrapper);
        this.uploadWrapper = sf.base.createElement('div', { className: CONTROL_WRAPPER, attrs: { style: 'background: white' } });
        this.dropAreaWrapper.parentElement.insertBefore(this.uploadWrapper, this.dropAreaWrapper);
        this.uploadWrapper.appendChild(this.dropAreaWrapper);
    };
    RteUploader.prototype.setExtensions = function (extensions) {
        if (extensions !== '' && !sf.base.isNullOrUndefined(extensions)) {
            this.element.setAttribute('accept', extensions);
        }
        else {
            this.element.removeAttribute('accept');
        }
    };
    RteUploader.prototype.setRTL = function () {
        this.enableRtl ? sf.base.addClass([this.uploadWrapper], RTL) : sf.base.removeClass([this.uploadWrapper], RTL);
    };
    RteUploader.prototype.setCSSClass = function (oldCSSClass) {
        var updatedCssClassValue = this.cssClass;
        if (!sf.base.isNullOrUndefined(this.cssClass) && this.cssClass !== '') {
            updatedCssClassValue = (this.cssClass.replace(/\s+/g, ' ')).trim();
        }
        if (!sf.base.isNullOrUndefined(this.cssClass) && updatedCssClassValue !== '') {
            sf.base.addClass([this.uploadWrapper], updatedCssClassValue.split(updatedCssClassValue.indexOf(',') > -1 ? ',' : ' '));
        }
        var updatedOldCssClass = oldCSSClass;
        if (!sf.base.isNullOrUndefined(oldCSSClass)) {
            updatedOldCssClass = (oldCSSClass.replace(/\s+/g, ' ')).trim();
        }
        if (!sf.base.isNullOrUndefined(oldCSSClass) && updatedOldCssClass !== '') {
            sf.base.removeClass([this.uploadWrapper], updatedOldCssClass.split(' '));
        }
    };
    RteUploader.prototype.createFileList = function (fileData, isSelectedFile) {
        this.createParentUL();
        if (this.isFormUpload()) {
            this.uploadWrapper.classList.add(FORM_UPLOAD);
            this.formFileList(fileData, this.element.files);
        }
        else {
            for (var _i = 0, fileData_1 = fileData; _i < fileData_1.length; _i++) {
                var listItem = fileData_1[_i];
                var liElement = sf.base.createElement('li', {
                    className: FILE,
                    attrs: { 'data-file-name': listItem.name, 'data-files-count': '1' }
                });
                var textContainer = sf.base.createElement('span', { className: TEXT_CONTAINER });
                var textElement = sf.base.createElement('span', { className: FILE_NAME, attrs: { 'title': listItem.name } });
                textElement.innerHTML = this.getFileNameOnly(listItem.name);
                var fileExtension = sf.base.createElement('span', { className: FILE_TYPE });
                fileExtension.innerHTML = '.' + this.getFileType(listItem.name);
                if (!this.enableRtl) {
                    textContainer.appendChild(textElement);
                    textContainer.appendChild(fileExtension);
                }
                else {
                    var rtlContainer = sf.base.createElement('span', { className: RTL_CONTAINER });
                    rtlContainer.appendChild(fileExtension);
                    rtlContainer.appendChild(textElement);
                    textContainer.appendChild(rtlContainer);
                }
                var fileSize = sf.base.createElement('span', { className: FILE_SIZE });
                fileSize.innerHTML = this.bytesToSize(listItem.size);
                textContainer.appendChild(fileSize);
                var statusElement = sf.base.createElement('span', { className: STATUS });
                textContainer.appendChild(statusElement);
                statusElement.innerHTML = listItem.status;
                liElement.appendChild(textContainer);
                var iconElement = sf.base.createElement('span', { className: ' e-icons', attrs: { 'tabindex': this.btnTabIndex } });
                /* istanbul ignore next */
                if (this.browserName === 'msie') {
                    iconElement.classList.add('e-msie');
                }
                iconElement.setAttribute('title', 'Remove');
                liElement.appendChild(iconElement);
                sf.base.EventHandler.add(iconElement, 'click', this.removeFiles, this);
                if (listItem.statusCode === '2') {
                    statusElement.classList.add(UPLOAD_SUCCESS);
                    iconElement.classList.add(DELETE_ICON);
                    iconElement.setAttribute('title', 'Delete file');
                }
                else if (listItem.statusCode !== '1') {
                    statusElement.classList.remove(UPLOAD_SUCCESS);
                    statusElement.classList.add(VALIDATION_FAILS);
                }
                if (listItem.statusCode === '1' && this.asyncSettings.saveUrl !== '') {
                    statusElement.innerHTML = '';
                }
                if (!iconElement.classList.contains(DELETE_ICON)) {
                    iconElement.classList.add(REMOVE_ICON);
                }
                this.listParent.appendChild(liElement);
                this.fileList.push(liElement);
                this.truncateName(textElement);
                var preventActionComplete = this.flag;
                if (this.isPreLoadFile(listItem)) {
                    this.flag = false;
                    this.checkActionComplete(true);
                    this.flag = preventActionComplete;
                }
            }
        }
    };
    RteUploader.prototype.createParentUL = function () {
        if (sf.base.isNullOrUndefined(this.listParent)) {
            this.listParent = sf.base.createElement('ul', { className: LIST_PARENT });
            this.uploadWrapper.appendChild(this.listParent);
        }
    };
    RteUploader.prototype.isFormUpload = function () {
        var isFormUpload = false;
        if (this.isForm && ((sf.base.isNullOrUndefined(this.asyncSettings.saveUrl) || this.asyncSettings.saveUrl === '')
            && (sf.base.isNullOrUndefined(this.asyncSettings.removeUrl) || this.asyncSettings.removeUrl === ''))) {
            isFormUpload = true;
        }
        return isFormUpload;
    };
    RteUploader.prototype.getFileType = function (name) {
        var extension;
        var index = name.lastIndexOf('.');
        if (index >= 0) {
            extension = name.substring(index + 1);
        }
        return extension ? extension : '';
    };
    RteUploader.prototype.getFileNameOnly = function (name) {
        var type = this.getFileType(name);
        var names = name.split('.' + type);
        return type = names[0];
    };
    RteUploader.prototype.truncateName = function (name) {
        var nameElement = name;
        if (this.browserName !== 'edge' && nameElement.offsetWidth < nameElement.scrollWidth) {
            this.getSlicedName(nameElement);
            /* istanbul ignore next */
        }
        else if (nameElement.offsetWidth + 1 < nameElement.scrollWidth) {
            this.getSlicedName(nameElement);
        }
    };
    RteUploader.prototype.getSlicedName = function (nameElement) {
        var text;
        text = nameElement.textContent;
        nameElement.dataset.tail = text.slice(text.length - 10);
    };
    RteUploader.prototype.setListToFileInfo = function (fileData, fileList) {
        for (var _i = 0, fileData_2 = fileData; _i < fileData_2.length; _i++) {
            var listItem = fileData_2[_i];
            listItem.list = fileList;
        }
    };
    RteUploader.prototype.getFileSize = function (fileData) {
        var fileSize = 0;
        for (var _i = 0, fileData_3 = fileData; _i < fileData_3.length; _i++) {
            var file = fileData_3[_i];
            fileSize += file.size;
        }
        return fileSize;
    };
    RteUploader.prototype.bytesToSize = function (bytes) {
        var i = -1;
        if (!bytes) {
            return '0.0 KB';
        }
        do {
            bytes = bytes / 1024;
            i++;
        } while (bytes > 99);
        if (i >= 2) {
            bytes = bytes * 1024;
            i = 1;
        }
        return Math.max(bytes, 0).toFixed(1) + ' ' + ['KB', 'MB'][i];
    };
    RteUploader.prototype.createFormInput = function (fileData) {
        var inputElement = this.element.cloneNode(true);
        inputElement.classList.add(HIDDEN_INPUT);
        for (var _i = 0, fileData_4 = fileData; _i < fileData_4.length; _i++) {
            var listItem = fileData_4[_i];
            listItem.input = inputElement;
        }
        inputElement.setAttribute('name', this.uploaderName);
        this.uploadWrapper.querySelector('.' + INPUT_WRAPPER).appendChild(inputElement);
        if (this.browserName !== 'msie' && this.browserName !== 'edge') {
            this.element.value = '';
        }
    };
    RteUploader.prototype.checkActionComplete = function (increment) {
        increment ? ++this.actionCompleteCount : --this.actionCompleteCount;
        if ((this.filesData.length === this.actionCompleteCount) && this.flag) {
            this.flag = false;
        }
    };
    RteUploader.prototype.isPreLoadFile = function (fileData) {
        var isPreload = false;
        for (var i = 0; i < this.files.length; i++) {
            if (this.files[i].name === fileData.name.slice(0, fileData.name.lastIndexOf('.')) && this.files[i].type === fileData.type) {
                isPreload = true;
            }
        }
        return isPreload;
    };
    RteUploader.prototype.validatedFileSize = function (fileSize) {
        var minSizeError = '';
        var maxSizeError = '';
        if (fileSize < this.minFileSize) {
            minSizeError = 'File size is too small';
        }
        else if (fileSize > this.maxFileSize) {
            maxSizeError = 'File size is too large';
        }
        else {
            minSizeError = '';
            maxSizeError = '';
        }
        var errorMessage = { minSize: minSizeError, maxSize: maxSizeError };
        return errorMessage;
    };
    RteUploader.prototype.isBlank = function (str) {
        return (!str || /^\s*$/.test(str));
    };
    RteUploader.prototype.addInvalidClass = function (fileList) {
        fileList.classList.add(INVALID_FILE);
    };
    RteUploader.prototype.checkExtension = function (files) {
        var dropFiles = files;
        if (!this.isBlank(this.allowedExtensions)) {
            var allowedExtensions = [];
            var extensions = this.allowedExtensions.split(',');
            for (var _i = 0, extensions_1 = extensions; _i < extensions_1.length; _i++) {
                var extension = extensions_1[_i];
                allowedExtensions.push(extension.trim().toLocaleLowerCase());
            }
            for (var i = 0; i < files.length; i++) {
                if (allowedExtensions.indexOf(('.' + files[i].type).toLocaleLowerCase()) === -1) {
                    files[i].status = 'File type is not allowed';
                    files[i].statusCode = '0';
                }
            }
        }
        return dropFiles;
    };
    RteUploader.prototype.getFilesData = function (index) {
        if (sf.base.isNullOrUndefined(index)) {
            return this.filesData;
        }
        else {
            return this.getSelectedFiles(index);
        }
    };
    RteUploader.prototype.getFilesInArray = function (files) {
        var uploadFiles = [];
        if (files instanceof Array) {
            uploadFiles = files;
        }
        else {
            uploadFiles.push(files);
        }
        return uploadFiles;
    };
    RteUploader.prototype.formValidateFileInfo = function (listItem, fileList) {
        var statusMessage = listItem.status;
        var validationMessages = this.validatedFileSize(listItem.size);
        if (validationMessages.minSize !== '' || validationMessages.maxSize !== '') {
            this.addInvalidClass(fileList);
            statusMessage = validationMessages.minSize !== '' ? 'File size is too small' :
                validationMessages.maxSize !== '' ? 'File size is too large' : statusMessage;
        }
        var typeValidationMessage = this.checkExtension(this.getFilesInArray(listItem))[0].status;
        if (typeValidationMessage === 'File type is not allowed') {
            this.addInvalidClass(fileList);
            statusMessage = typeValidationMessage;
        }
        return statusMessage;
    };
    RteUploader.prototype.clearAll = function () {
        if (sf.base.isNullOrUndefined(this.listParent)) {
            if (this.browserName !== 'msie') {
                this.element.value = '';
            }
            this.filesData = [];
            return;
        }
        this.clearData();
        this.actionCompleteCount = 0;
    };
    RteUploader.prototype.clearData = function (singleUpload) {
        if (!sf.base.isNullOrUndefined(this.listParent)) {
            sf.base.detach(this.listParent);
            this.listParent = null;
        }
        if (this.browserName !== 'msie' && !singleUpload) {
            this.element.value = '';
        }
        this.fileList = [];
        this.filesData = [];
        this.removeActionButtons();
    };
    RteUploader.prototype.removeActionButtons = function () {
        if (this.actionButtons) {
            sf.base.detach(this.actionButtons);
            this.actionButtons = null;
        }
    };
    RteUploader.prototype.checkActionButtonStatus = function () {
        if (this.actionButtons) {
            var length_1 = this.uploadWrapper.querySelectorAll('.' + VALIDATION_FAILS).length +
                this.uploadWrapper.querySelectorAll('.e-upload-fails:not(.e-upload-progress)').length +
                this.uploadWrapper.querySelectorAll('span.' + UPLOAD_SUCCESS).length +
                this.uploadWrapper.querySelectorAll('span.' + UPLOAD_INPROGRESS).length;
            if (length_1 > 0 && length_1 === this.uploadWrapper.querySelectorAll('li').length) {
                this.uploadButton.setAttribute('disabled', 'disabled');
            }
            else {
                this.uploadButton.removeAttribute('disabled');
            }
        }
    };
    RteUploader.prototype.getSelectedFiles = function (index) {
        var data = [];
        var liElement = this.fileList[index];
        var allFiles = this.getFilesData();
        var nameElements = +liElement.getAttribute('data-files-count');
        var startIndex = 0;
        for (var i = 0; i < index; i++) {
            startIndex += (+this.fileList[i].getAttribute('data-files-count'));
        }
        for (var j = startIndex; j < (startIndex + nameElements); j++) {
            data.push(allFiles[j]);
        }
        return data;
    };
    RteUploader.prototype.formFileList = function (fileData, files) {
        var fileList = sf.base.createElement('li', { className: FILE });
        fileList.setAttribute('data-files-count', fileData.length + '');
        var fileContainer = sf.base.createElement('span', { className: TEXT_CONTAINER });
        var statusMessage;
        for (var _i = 0, fileData_5 = fileData; _i < fileData_5.length; _i++) {
            var listItem = fileData_5[_i];
            var fileNameEle = sf.base.createElement('span', { className: FILE_NAME });
            fileNameEle.innerHTML = this.getFileNameOnly(listItem.name);
            var fileTypeEle = sf.base.createElement('span', { className: FILE_TYPE });
            fileTypeEle.innerHTML = '.' + this.getFileType(listItem.name);
            if (!this.enableRtl) {
                fileContainer.appendChild(fileNameEle);
                fileContainer.appendChild(fileTypeEle);
            }
            else {
                var rtlContainer = sf.base.createElement('span', { className: RTL_CONTAINER });
                rtlContainer.appendChild(fileTypeEle);
                rtlContainer.appendChild(fileNameEle);
                fileContainer.appendChild(rtlContainer);
            }
            this.truncateName(fileNameEle);
            statusMessage = this.formValidateFileInfo(listItem, fileList);
        }
        fileList.appendChild(fileContainer);
        this.setListToFileInfo(fileData, fileList);
        var infoEle = sf.base.createElement('span');
        if (fileList.classList.contains(INVALID_FILE)) {
            infoEle.classList.add(STATUS);
            infoEle.classList.add(INVALID_FILE);
            infoEle.innerText = fileData.length > 1 ? 'invalidFileSelection' : statusMessage;
        }
        else {
            infoEle.classList.add(fileData.length > 1 ? INFORMATION : FILE_SIZE);
            infoEle.innerText = fileData.length > 1 ? 'totalFiles: ' + fileData.length + ' , '
                + 'size: ' + this.bytesToSize(this.getFileSize(fileData)) : this.bytesToSize(fileData[0].size);
            this.createFormInput(fileData);
        }
        fileContainer.appendChild(infoEle);
        if (sf.base.isNullOrUndefined(fileList.querySelector('.e-icons'))) {
            var iconElement = sf.base.createElement('span', { className: 'e-icons', attrs: { 'tabindex': this.btnTabIndex } });
            /* istanbul ignore next */
            if (this.browserName === 'msie') {
                iconElement.classList.add('e-msie');
            }
            iconElement.setAttribute('title', 'Remove');
            fileList.appendChild(fileContainer);
            fileList.appendChild(iconElement);
            sf.base.EventHandler.add(iconElement, 'click', this.removeFiles, this);
            iconElement.classList.add(REMOVE_ICON);
        }
        this.listParent.appendChild(fileList);
        this.fileList.push(fileList);
    };
    RteUploader.prototype.removeFiles = function (args) {
        var selectedElement = args.target.parentElement;
        var index = this.fileList.indexOf(selectedElement);
        var liElement = this.fileList[index];
        var formUpload = this.isFormUpload();
        var fileData = formUpload ? this.getSelectedFiles(index) : this.getFilesInArray(this.filesData[index]);
        if (sf.base.isNullOrUndefined(fileData)) {
            return;
        }
        if (args.target.classList.contains(ABORT_ICON) && !formUpload) {
            fileData[0].statusCode = '5';
            if (!sf.base.isNullOrUndefined(liElement)) {
                var spinnerTarget = liElement.querySelector('.' + ABORT_ICON);
                sf.popups.createSpinner({ target: spinnerTarget, width: '20px' });
                sf.popups.showSpinner(spinnerTarget);
            }
            if (!(liElement.classList.contains(RESTRICT_RETRY))) {
                this.checkActionComplete(true);
            }
        }
        else if (!sf.base.closest(args.target, '.' + SPINNER_PANE)) {
            this.remove(fileData, false, false, true, args);
        }
        this.element.value = '';
        this.checkActionButtonStatus();
    };
    RteUploader.prototype.spliceFiles = function (liIndex) {
        var liElement = this.fileList[liIndex];
        var allFiles = this.getFilesData();
        var nameElements = +liElement.getAttribute('data-files-count');
        var startIndex = 0;
        for (var i = 0; i < liIndex; i++) {
            startIndex += (+this.fileList[i].getAttribute('data-files-count'));
        }
        var endIndex = (startIndex + nameElements) - 1;
        for (var j = endIndex; j >= startIndex; j--) {
            allFiles.splice(j, 1);
        }
    };
    RteUploader.prototype.remove = function (fileData, customTemplate, removeDirectly, postRawFile, args) {
        if (sf.base.isNullOrUndefined(postRawFile)) {
            postRawFile = true;
        }
        var eventArgs = {
            event: args,
            cancel: false,
            filesData: [],
            customFormData: [],
            postRawFile: postRawFile,
            currentRequest: null
        };
        if (this.isFormUpload()) {
            eventArgs.filesData = fileData;
            this.parent.observer.notify(removing, eventArgs);
            var removingFiles = this.getFilesInArray(fileData);
            var isLiRemoved = false;
            var liIndex = void 0;
            for (var _i = 0, removingFiles_1 = removingFiles; _i < removingFiles_1.length; _i++) {
                var data = removingFiles_1[_i];
                if (!isLiRemoved) {
                    liIndex = this.fileList.indexOf(data.list);
                }
                if (liIndex > -1) {
                    var inputElement = !sf.base.isNullOrUndefined(data.input) ? data.input : null;
                    if (inputElement) {
                        sf.base.detach(inputElement);
                    }
                    this.spliceFiles(liIndex);
                    sf.base.detach(this.fileList[liIndex]);
                    this.fileList.splice(liIndex, 1);
                    isLiRemoved = true;
                    liIndex = -1;
                }
            }
        }
        else if (this.isForm && (sf.base.isNullOrUndefined(this.asyncSettings.removeUrl) || this.asyncSettings.removeUrl === '')) {
            eventArgs.filesData = this.getFilesData();
            this.parent.observer.notify(removing, eventArgs);
            this.clearAll();
        }
        else {
            var removeFiles = [];
            fileData = !sf.base.isNullOrUndefined(fileData) ? fileData : this.filesData;
            if (fileData instanceof Array) {
                removeFiles = fileData;
            }
            else {
                removeFiles.push(fileData);
            }
            eventArgs.filesData = removeFiles;
            var removeUrl = this.asyncSettings.removeUrl;
            var validUrl = (removeUrl === '' || sf.base.isNullOrUndefined(removeUrl)) ? false : true;
            for (var _a = 0, removeFiles_1 = removeFiles; _a < removeFiles_1.length; _a++) {
                var files = removeFiles_1[_a];
                var fileUploadedIndex = this.uploadedFilesData.indexOf(files);
                if ((files.statusCode === '2' || files.statusCode === '4' || (files.statusCode === '0' &&
                    fileUploadedIndex !== -1)) && validUrl) {
                    this.removeUploadedFile(files, eventArgs, removeDirectly, customTemplate);
                }
                else {
                    if (!removeDirectly) {
                        this.parent.observer.notify(removing, eventArgs);
                        this.removeFilesData(files, customTemplate);
                    }
                    else {
                        this.removeFilesData(files, customTemplate);
                    }
                }
                if (args && !args.target.classList.contains(REMOVE_ICON)) {
                    this.checkActionComplete(false);
                }
            }
        }
    };
    RteUploader.prototype.updateCustomHeader = function (request, currentRequest) {
        if (currentRequest.length > 0 && currentRequest[0]) {
            var _loop_1 = function (i) {
                var data = currentRequest[i];
                // eslint-disable-next-line
                var value = Object.keys(data).map(function (e) {
                    return data[e];
                });
                request.setRequestHeader(Object.keys(data)[0], value);
            };
            for (var i = 0; i < currentRequest.length; i++) {
                _loop_1(i);
            }
        }
    };
    RteUploader.prototype.removeUploadedFile = function (file, eventArgs, removeDirectly, custom) {
        var _this = this;
        var selectedFiles = file;
        var ajax = new sf.base.Ajax(this.asyncSettings.removeUrl, 'POST', true, null);
        ajax.emitError = false;
        var formData = new FormData();
        ajax.beforeSend = function (e) {
            eventArgs.currentRequest = ajax.httpRequest;
            if (_this.currentRequestHeader) {
                _this.updateCustomHeader(ajax.httpRequest, _this.currentRequestHeader);
            }
            if (_this.customFormDatas) {
                _this.updateFormData(formData, _this.customFormDatas);
            }
            if (!removeDirectly) {
                _this.parent.observer.notify(removing, eventArgs);
                _this.removingEventCallback(eventArgs, formData, selectedFiles, file);
            }
            else {
                _this.removingEventCallback(eventArgs, formData, selectedFiles, file);
            }
        };
        ajax.onLoad = function (e) { _this.removeCompleted(e, selectedFiles, custom); return {}; };
        /* istanbul ignore next */
        ajax.onError = function (e) { _this.removeFailed(e, selectedFiles, custom); return {}; };
        ajax.send(formData);
    };
    RteUploader.prototype.getResponse = function (e) {
        // eslint-disable-next-line
        var target = e.currentTarget;
        var response = {
            readyState: target.readyState,
            statusCode: target.status,
            statusText: target.statusText,
            headers: target.getAllResponseHeaders(),
            withCredentials: target.withCredentials
        };
        return response;
    };
    RteUploader.prototype.removeCompleted = function (e, files, customTemplate) {
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var status = e.target;
        if (status.readyState === 4 && status.status >= 200 && status.status <= 299) {
            var args = {
                e: e, response: response, operation: 'Remove', file: this.updateStatus(files, 'File removed successfully', '2')
            };
            this.parent.observer.notify(success, args);
            this.removeFilesData(files, customTemplate);
            var index = this.uploadedFilesData.indexOf(files);
            this.uploadedFilesData.splice(index, 1);
        }
        else {
            this.removeFailed(e, files, customTemplate);
        }
    };
    RteUploader.prototype.removeFailed = function (e, files, customTemplate) {
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var args = {
            e: e, response: response, operation: 'Remove', file: this.updateStatus(files, 'Unable to remove file', '0')
        };
        if (!customTemplate) {
            var index = this.filesData.indexOf(files);
            var rootElement = this.fileList[index];
            if (rootElement) {
                rootElement.classList.remove(UPLOAD_SUCCESS);
                rootElement.classList.add(UPLOAD_FAILED);
                var statusElement = rootElement.querySelector('.' + STATUS);
                if (statusElement) {
                    statusElement.classList.remove(UPLOAD_SUCCESS);
                    statusElement.classList.add(UPLOAD_FAILED);
                }
            }
            this.checkActionButtonStatus();
        }
        this.parent.observer.notify(failure, args);
        var liElement = this.getLiElement(files);
        /* istanbul ignore next */
        if (!sf.base.isNullOrUndefined(liElement) && !sf.base.isNullOrUndefined(liElement.querySelector('.' + DELETE_ICON))) {
            var spinnerTarget = liElement.querySelector('.' + DELETE_ICON);
            sf.popups.hideSpinner(spinnerTarget);
            sf.base.detach(liElement.querySelector('.e-spinner-pane'));
        }
    };
    RteUploader.prototype.updateStatus = function (files, status, statusCode, updateLiStatus) {
        if (updateLiStatus === void 0) { updateLiStatus = true; }
        if (!(status === '' || sf.base.isNullOrUndefined(status)) && !(statusCode === '' || sf.base.isNullOrUndefined(statusCode))) {
            files.status = status;
            files.statusCode = statusCode;
        }
        if (updateLiStatus) {
            var li = this.getLiElement(files);
            if (!sf.base.isNullOrUndefined(li)) {
                if (!sf.base.isNullOrUndefined(li.querySelector('.' + STATUS)) && !((status === '' || sf.base.isNullOrUndefined(status)))) {
                    li.querySelector('.' + STATUS).textContent = status;
                }
            }
        }
        return files;
    };
    RteUploader.prototype.removingEventCallback = function (eventArgs, formData, selectedFiles, file) {
        /* istanbul ignore next */
        var name = this.element.getAttribute('name');
        var liElement = this.getLiElement(file);
        if (!sf.base.isNullOrUndefined(liElement) && (!sf.base.isNullOrUndefined(liElement.querySelector('.' + DELETE_ICON)) ||
            !sf.base.isNullOrUndefined(liElement.querySelector('.' + REMOVE_ICON)))) {
            var spinnerTarget = void 0;
            spinnerTarget = liElement.querySelector('.' + DELETE_ICON) ? liElement.querySelector('.' + DELETE_ICON) :
                liElement.querySelector('.' + REMOVE_ICON);
            sf.popups.createSpinner({ target: spinnerTarget, width: '20px' });
            sf.popups.showSpinner(spinnerTarget);
        }
        if (eventArgs.postRawFile && !sf.base.isNullOrUndefined(selectedFiles.rawFile) && selectedFiles.rawFile !== '') {
            formData.append(name, selectedFiles.rawFile, selectedFiles.name);
        }
        else {
            formData.append(name, selectedFiles.name);
        }
        this.updateFormData(formData, eventArgs.customFormData);
    };
    RteUploader.prototype.updateFormData = function (formData, customData) {
        if (customData.length > 0 && customData[0]) {
            var _loop_2 = function (i) {
                var data = customData[i];
                // eslint-disable-next-line
                var value = Object.keys(data).map(function (e) {
                    return data[e];
                });
                formData.append(Object.keys(data)[0], value);
            };
            for (var i = 0; i < customData.length; i++) {
                _loop_2(i);
            }
        }
    };
    RteUploader.prototype.getLiElement = function (files) {
        var index;
        for (var i = 0; i < this.filesData.length; i++) {
            if ((!sf.base.isNullOrUndefined(this.filesData[i].id) && !sf.base.isNullOrUndefined(files.id)) ? (this.filesData[i].name === files.name &&
                this.filesData[i].id === files.id) : this.filesData[i].name === files.name) {
                index = i;
            }
        }
        return this.fileList[index];
    };
    RteUploader.prototype.removeFilesData = function (file, customTemplate) {
        var index;
        if (customTemplate) {
            return;
        }
        var selectedElement = this.getLiElement(file);
        if (sf.base.isNullOrUndefined(selectedElement)) {
            return;
        }
        sf.base.detach(selectedElement);
        index = this.fileList.indexOf(selectedElement);
        this.fileList.splice(index, 1);
        this.filesData.splice(index, 1);
        if (this.fileList.length === 0 && !sf.base.isNullOrUndefined(this.listParent)) {
            sf.base.detach(this.listParent);
            this.listParent = null;
            this.removeActionButtons();
        }
    };
    RteUploader.prototype.upload = function (files, custom) {
        files = files ? files : this.filesData;
        this.beforeUploadCustom = sf.base.isNullOrUndefined(custom) ? false : custom;
        this.beforeUploadFiles = this.getFilesInArray(files);
        var eventArgs = {
            customFormData: [],
            currentRequest: null,
            cancel: false
        };
        this.parent.observer.notify(beforePasteUpload, eventArgs);
    };
    RteUploader.prototype.beforePasteUploadCallBack = function (eventArgs) {
        if (!eventArgs.cancel) {
            this.currentRequestHeader = eventArgs.currentRequest ? eventArgs.currentRequest : this.currentRequestHeader;
            this.customFormDatas = (eventArgs.customFormData && eventArgs.customFormData.length > 0) ?
                eventArgs.customFormData : this.customFormDatas;
            this.uploadFiles(this.beforeUploadFiles, this.beforeUploadCustom);
        }
    };
    RteUploader.prototype.filterfileList = function (files) {
        var filterFiles = [];
        var li;
        for (var i = 0; i < files.length; i++) {
            li = this.getLiElement(files[i]);
            if (!li.classList.contains(UPLOAD_SUCCESS)) {
                filterFiles.push(files[i]);
            }
        }
        return filterFiles;
    };
    RteUploader.prototype.uploadFiles = function (files, custom) {
        var selectedFiles = [];
        if (this.asyncSettings.saveUrl === '' || sf.base.isNullOrUndefined(this.asyncSettings.saveUrl)) {
            return;
        }
        if (!custom || sf.base.isNullOrUndefined(custom)) {
            selectedFiles = this.filterfileList(files);
        }
        else {
            selectedFiles = files;
        }
        for (var i = 0; i < selectedFiles.length; i++) {
            this.uploadFilesRequest(selectedFiles, i, custom);
        }
    };
    RteUploader.prototype.updateCustomheader = function (request, currentRequest) {
        if (currentRequest.length > 0 && currentRequest[0]) {
            var _loop_3 = function (i) {
                var data = currentRequest[i];
                // eslint-disable-next-line
                var value = Object.keys(data).map(function (e) {
                    return data[e];
                });
                request.setRequestHeader(Object.keys(data)[0], value);
            };
            for (var i = 0; i < currentRequest.length; i++) {
                _loop_3(i);
            }
        }
    };
    RteUploader.prototype.eventCancelByArgs = function (e, eventArgs, file) {
        var _this = this;
        e.cancel = true;
        if (eventArgs.fileData.statusCode === '5') {
            return;
        }
        eventArgs.fileData.statusCode = '5';
        eventArgs.fileData.status = 'File upload canceled';
        var liElement = this.getLiElement(eventArgs.fileData);
        if (liElement) {
            if (!sf.base.isNullOrUndefined(liElement.querySelector('.' + STATUS))) {
                liElement.querySelector('.' + STATUS).innerHTML = 'File upload canceled';
                liElement.querySelector('.' + STATUS).classList.add(UPLOAD_FAILED);
            }
            this.pauseButton = sf.base.createElement('span', { className: 'e-icons e-file-reload-btn', attrs: { 'tabindex': this.btnTabIndex } });
            var removeIcon = liElement.querySelector('.' + REMOVE_ICON);
            if (removeIcon) {
                removeIcon.parentElement.insertBefore(this.pauseButton, removeIcon);
            }
            this.pauseButton.setAttribute('title', 'Retry');
            /* istanbul ignore next */
            this.pauseButton.addEventListener('click', function (e) { _this.reloadcanceledFile(e, file, liElement); }, false);
            this.checkActionButtonStatus();
        }
    };
    RteUploader.prototype.reloadcanceledFile = function (e, file, liElement, custom) {
        file.statusCode = '1';
        file.status = 'Ready to upload';
        if (!custom) {
            liElement.querySelector('.' + STATUS).classList.remove(UPLOAD_FAILED);
            if (!sf.base.isNullOrUndefined(liElement.querySelector('.' + RETRY_ICON))) {
                sf.base.detach(liElement.querySelector('.' + RETRY_ICON));
            }
            this.pauseButton = null;
        }
        /* istanbul ignore next */
        liElement.classList.add(RESTRICT_RETRY);
        this.upload([file]);
    };
    RteUploader.prototype.uploadFilesRequest = function (selectedFiles, i, custom) {
        var _this = this;
        var ajax = new sf.base.Ajax(this.asyncSettings.saveUrl, 'POST', true, null);
        ajax.emitError = false;
        /* istanbul ignore next */
        var eventArgs = {
            fileData: selectedFiles[i],
            customFormData: [],
            cancel: false
        };
        var formData = new FormData();
        ajax.beforeSend = function (e) {
            eventArgs.currentRequest = ajax.httpRequest;
            /* istanbul ignore next */
            eventArgs.fileData.rawFile = _this.base64String[i];
            if (_this.currentRequestHeader) {
                _this.updateCustomheader(ajax.httpRequest, _this.currentRequestHeader);
            }
            if (_this.customFormDatas) {
                _this.updateFormData(formData, _this.customFormDatas);
            }
            _this.parent.observer.notify(uploading, eventArgs);
            /* istanbul ignore next */
            if (eventArgs.cancel) {
                _this.eventCancelByArgs(e, eventArgs, selectedFiles[i]);
            }
            _this.updateFormData(formData, eventArgs.customFormData);
        };
        if (selectedFiles[i].statusCode === '1') {
            var name_1 = this.element.getAttribute('name');
            formData.append(name_1, selectedFiles[i].rawFile, selectedFiles[i].name);
            ajax.onLoad = function (e) {
                _this.uploadComplete(e, selectedFiles[i], custom);
                return {};
            };
            ajax.onUploadProgress = function (e) {
                _this.uploadInProgress(e, selectedFiles[i], custom, ajax);
                return {};
            };
            /* istanbul ignore next */
            ajax.onError = function (e) { _this.uploadFailed(e, selectedFiles[i]); return {}; };
            ajax.send(formData);
        }
    };
    RteUploader.prototype.cancelUploadingFile = function (files, e, request, li) {
        var _this = this;
        if (files.statusCode === '5') {
            var eventArgs = {
                event: e,
                fileData: files,
                cancel: false,
                customFormData: []
            };
            this.parent.observer.notify(canceling, eventArgs);
            if (eventArgs.cancel) {
                files.statusCode = '3';
                if (!sf.base.isNullOrUndefined(li)) {
                    var spinnerTarget = li.querySelector('.' + ABORT_ICON);
                    if (!sf.base.isNullOrUndefined(spinnerTarget)) {
                        sf.popups.hideSpinner(spinnerTarget);
                        sf.base.detach(li.querySelector('.e-spinner-pane'));
                    }
                }
            }
            else {
                request.emitError = false;
                request.httpRequest.abort();
                var formData = new FormData();
                if (files.statusCode === '5') {
                    var name_2 = this.element.getAttribute('name');
                    formData.append(name_2, files.name);
                    formData.append('cancel-uploading', files.name);
                    this.updateFormData(formData, eventArgs.customFormData);
                    var ajax = new sf.base.Ajax(this.asyncSettings.removeUrl, 'POST', true, null);
                    ajax.emitError = false;
                    ajax.onLoad = function (e) { _this.removecanceledFile(e, files); return {}; };
                    ajax.send(formData);
                }
            }
        }
    };
    RteUploader.prototype.removecanceledFile = function (e, file) {
        var liElement = this.getLiElement(file);
        if (liElement.querySelector('.' + RETRY_ICON) || sf.base.isNullOrUndefined(liElement.querySelector('.' + ABORT_ICON))) {
            return;
        }
        this.updateStatus(file, 'File upload canceled', '5');
        this.renderFailureState(e, file, liElement);
        var spinnerTarget = liElement.querySelector('.' + REMOVE_ICON);
        if (!sf.base.isNullOrUndefined(liElement)) {
            sf.popups.hideSpinner(spinnerTarget);
            sf.base.detach(liElement.querySelector('.e-spinner-pane'));
        }
        var requestResponse = e && e.currentTarget ? this.getResponse(e) : null;
        var args = { event: e, response: requestResponse, operation: 'Cancel', file: file };
        this.parent.observer.notify(success, args);
    };
    RteUploader.prototype.uploadInProgress = function (e, files, customUI, request) {
        var li = this.getLiElement(files);
        if (sf.base.isNullOrUndefined(li) && (!customUI)) {
            return;
        }
        if (!sf.base.isNullOrUndefined(li)) {
            /* istanbul ignore next */
            if (files.statusCode === '5') {
                this.cancelUploadingFile(files, e, request, li);
            }
            if (!(li.querySelectorAll('.' + PROGRESS_WRAPPER).length > 0) && li.querySelector('.' + STATUS)) {
                li.querySelector('.' + STATUS).classList.add(UPLOAD_INPROGRESS);
                this.createProgressBar(li);
                this.updateProgressBarClasses(li, UPLOAD_INPROGRESS);
                li.querySelector('.' + STATUS).classList.remove(UPLOAD_FAILED);
            }
            this.updateProgressbar(e, li);
            var iconEle = li.querySelector('.' + REMOVE_ICON);
            if (!sf.base.isNullOrUndefined(iconEle)) {
                iconEle.classList.add(ABORT_ICON, UPLOAD_INPROGRESS);
                iconEle.setAttribute('title', 'Abort');
                iconEle.classList.remove(REMOVE_ICON);
            }
        }
        else {
            this.cancelUploadingFile(files, e, request);
        }
    };
    RteUploader.prototype.changeProgressValue = function (li, progressValue) {
        li.querySelector('.' + PROGRESSBAR).setAttribute('style', 'width:' + progressValue);
        li.querySelector('.' + PROGRESSBAR_TEXT).textContent = progressValue;
    };
    RteUploader.prototype.updateProgressbar = function (e, li) {
        if (!isNaN(Math.round((e.loaded / e.total) * 100)) && !sf.base.isNullOrUndefined(li.querySelector('.' + PROGRESSBAR))) {
            if (!sf.base.isNullOrUndefined(this.progressInterval) && this.progressInterval !== '') {
                var value = (Math.round((e.loaded / e.total) * 100)) % parseInt(this.progressInterval, 10);
                if (value === 0 || value === 100) {
                    this.changeProgressValue(li, Math.round((e.loaded / e.total) * 100).toString() + '%');
                }
            }
            else {
                this.changeProgressValue(li, Math.round((e.loaded / e.total) * 100).toString() + '%');
            }
        }
    };
    RteUploader.prototype.createProgressBar = function (liElement) {
        var progressbarWrapper = sf.base.createElement('span', { className: PROGRESS_WRAPPER });
        var progressBar = sf.base.createElement('progressbar', { className: PROGRESSBAR, attrs: { value: '0', max: '100' } });
        var progressbarInnerWrapper = sf.base.createElement('span', { className: PROGRESS_INNER_WRAPPER });
        progressBar.setAttribute('style', 'width: 0%');
        var progressbarText = sf.base.createElement('span', { className: PROGRESSBAR_TEXT });
        progressbarText.textContent = '0%';
        progressbarInnerWrapper.appendChild(progressBar);
        progressbarWrapper.appendChild(progressbarInnerWrapper);
        progressbarWrapper.appendChild(progressbarText);
        liElement.querySelector('.' + TEXT_CONTAINER).appendChild(progressbarWrapper);
    };
    RteUploader.prototype.updateProgressBarClasses = function (li, className) {
        var progressBar = li.querySelector('.' + PROGRESSBAR);
        if (!sf.base.isNullOrUndefined(progressBar)) {
            progressBar.classList.add(className);
        }
    };
    RteUploader.prototype.raiseSuccessEvent = function (e, file) {
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var statusMessage = 'File uploaded successfully';
        var args = {
            e: e, response: response, operation: 'upload', file: this.updateStatus(file, statusMessage, '2', false), statusText: statusMessage
        };
        var liElement = this.getLiElement(file);
        if (!sf.base.isNullOrUndefined(liElement)) {
            var spinnerEle = liElement.querySelector('.' + SPINNER_PANE);
            if (!sf.base.isNullOrUndefined(spinnerEle)) {
                sf.popups.hideSpinner(liElement);
                sf.base.detach(spinnerEle);
            }
        }
        this.parent.observer.notify(success, args);
        // eslint-disable-next-line
        this.updateStatus(file, args.statusText, '2');
        this.uploadedFilesData.push(file);
        this.checkActionButtonStatus();
        if (this.fileList.length > 0) {
            if ((!(this.getLiElement(file)).classList.contains(RESTRICT_RETRY))) {
                this.checkActionComplete(true);
            }
            else {
                /* istanbul ignore next */
                (this.getLiElement(file)).classList.remove(RESTRICT_RETRY);
            }
        }
    };
    RteUploader.prototype.uploadComplete = function (e, file, customUI) {
        var status = e.target;
        if (status.readyState === 4 && status.status >= 200 && status.status <= 299) {
            var li = this.getLiElement(file);
            if (sf.base.isNullOrUndefined(li) && (!customUI || sf.base.isNullOrUndefined(customUI))) {
                return;
            }
            if (!sf.base.isNullOrUndefined(li)) {
                this.updateProgressBarClasses(li, UPLOAD_SUCCESS);
                this.removeProgressbar(li, 'success');
                var iconEle = li.querySelector('.' + ABORT_ICON);
                if (!sf.base.isNullOrUndefined(iconEle)) {
                    iconEle.classList.add(DELETE_ICON);
                    iconEle.setAttribute('title', 'Delete file');
                    iconEle.classList.remove(ABORT_ICON);
                    iconEle.classList.remove(UPLOAD_INPROGRESS);
                }
            }
            this.raiseSuccessEvent(e, file);
        }
        else {
            this.uploadFailed(e, file);
        }
    };
    RteUploader.prototype.uploadFailed = function (e, file) {
        var li = this.getLiElement(file);
        var response = e && e.currentTarget ? this.getResponse(e) : null;
        var statusMessage = 'File failed to upload';
        var args = {
            e: e, response: response, operation: 'upload', file: this.updateStatus(file, statusMessage, '0', false), statusText: statusMessage
        };
        if (!sf.base.isNullOrUndefined(li)) {
            this.renderFailureState(e, file, li);
        }
        this.parent.observer.notify(failure, args);
        // eslint-disable-next-line
        this.updateStatus(file, args.statusText, '0');
        this.checkActionButtonStatus();
        this.checkActionComplete(true);
    };
    RteUploader.prototype.renderFailureState = function (e, file, liElement) {
        var _this = this;
        this.updateProgressBarClasses(liElement, UPLOAD_FAILED);
        this.removeProgressbar(liElement, 'failure');
        if (!sf.base.isNullOrUndefined(liElement.querySelector('.e-file-status'))) {
            liElement.querySelector('.e-file-status').classList.add(UPLOAD_FAILED);
        }
        var deleteIcon = liElement.querySelector('.' + ABORT_ICON);
        if (sf.base.isNullOrUndefined(deleteIcon)) {
            return;
        }
        deleteIcon.classList.remove(ABORT_ICON, UPLOAD_INPROGRESS);
        deleteIcon.classList.add(REMOVE_ICON);
        deleteIcon.setAttribute('title', 'Remove');
        this.pauseButton = sf.base.createElement('span', { className: 'e-icons e-file-reload-btn', attrs: { 'tabindex': this.btnTabIndex } });
        deleteIcon.parentElement.insertBefore(this.pauseButton, deleteIcon);
        this.pauseButton.setAttribute('title', 'Retry');
        var retryElement = liElement.querySelector('.' + RETRY_ICON);
        /* istanbul ignore next */
        retryElement.addEventListener('click', function (e) { _this.reloadcanceledFile(e, file, liElement, false); }, false);
    };
    RteUploader.prototype.removeProgressbar = function (li, callType) {
        var _this = this;
        if (!sf.base.isNullOrUndefined(li.querySelector('.' + PROGRESS_WRAPPER))) {
            this.progressAnimation = new sf.base.Animation({ duration: 1250 });
            this.progressAnimation.animate(li.querySelector('.' + PROGRESS_WRAPPER), { name: 'FadeOut' });
            this.progressAnimation.animate(li.querySelector('.' + PROGRESSBAR_TEXT), { name: 'FadeOut' });
            setTimeout(function () { _this.animateProgressBar(li, callType); }, 750);
        }
    };
    RteUploader.prototype.animateProgressBar = function (li, callType) {
        if (callType === 'success') {
            if (!sf.base.isNullOrUndefined(li.querySelector('.' + STATUS))) {
                li.querySelector('.' + STATUS).classList.remove(UPLOAD_INPROGRESS);
                this.progressAnimation.animate(li.querySelector('.' + STATUS), { name: 'FadeIn' });
                li.querySelector('.' + STATUS).classList.add(UPLOAD_SUCCESS);
            }
        }
        else {
            if (!sf.base.isNullOrUndefined(li.querySelector('.' + STATUS))) {
                li.querySelector('.' + STATUS).classList.remove(UPLOAD_INPROGRESS);
                this.progressAnimation.animate(li.querySelector('.' + STATUS), { name: 'FadeIn' });
                li.querySelector('.' + STATUS).classList.add(UPLOAD_FAILED);
            }
        }
        if (li.querySelector('.' + PROGRESS_WRAPPER)) {
            sf.base.detach(li.querySelector('.' + PROGRESS_WRAPPER));
        }
    };
    RteUploader.prototype.onSelectFiles = function (args) {
        var targetFiles;
        /* istanbul ignore next */
        if (args.type === 'drop') {
            var files = this.sortFilesList = args.dataTransfer.files;
            if (this.browserName !== 'msie' && this.browserName !== 'edge') {
                this.element.files = files;
            }
            if (files.length > 0) {
                targetFiles = this.sortFileList(files);
                this.renderSelectedFiles(args, targetFiles);
            }
        }
        else {
            targetFiles = [].slice.call(args.target.files);
            this.renderSelectedFiles(args, targetFiles);
        }
    };
    RteUploader.prototype.sortFileList = function (filesData) {
        filesData = filesData ? filesData : this.sortFilesList;
        var files = filesData;
        var fileNames = [];
        for (var i = 0; i < files.length; i++) {
            fileNames.push(files[i].name);
        }
        var sortedFileNames = fileNames.sort();
        var sortedFilesData = [];
        for (var _i = 0, sortedFileNames_1 = sortedFileNames; _i < sortedFileNames_1.length; _i++) {
            var name_3 = sortedFileNames_1[_i];
            for (var i = 0; i < files.length; i++) {
                if (name_3 === files[i].name) {
                    sortedFilesData.push(files[i]);
                }
            }
        }
        return sortedFilesData;
    };
    RteUploader.prototype.renderSelectedFiles = function (args, 
    // eslint-disable-next-line
    targetFiles, directory, paste$$1) {
        this.base64String = [];
        // eslint-disable-next-line
        var eventArgs = {
            event: args,
            cancel: false,
            filesData: [],
            isModified: false,
            modifiedFilesData: [],
            progressInterval: '',
            isCanceled: false,
            currentRequest: null,
            customFormData: null
        };
        /* istanbul ignore next */
        if (targetFiles.length < 1) {
            eventArgs.isCanceled = true;
            this.parent.observer.notify(selected, eventArgs);
            return;
        }
        this.flag = true;
        var fileData = [];
        for (var i = 0; i < targetFiles.length; i++) {
            var file = directory ? targetFiles[i].file : targetFiles[i];
            this.updateInitialFileDetails(args, targetFiles, file, i, fileData, directory, paste$$1);
        }
        eventArgs.filesData = fileData;
        if (this.allowedExtensions.indexOf('*') > -1) {
            this.allTypes = true;
        }
        if (!this.allTypes) {
            fileData = this.checkExtension(fileData);
        }
        this.parent.observer.notify(selected, eventArgs);
        eventArgs.cancel = true;
        this._internalRenderSelect(eventArgs, fileData);
    };
    RteUploader.prototype._internalRenderSelect = function (eventArgs, fileData) {
        if (!eventArgs.cancel) {
            /* istanbul ignore next */
            this.currentRequestHeader = eventArgs.currentRequest;
            this.customFormDatas = eventArgs.customFormData;
            this.selectedFiles = this.selectedFiles.concat(fileData);
            this.btnTabIndex = this.disableKeyboardNavigation ? '-1' : '0';
            if (eventArgs.isModified && eventArgs.modifiedFilesData.length > 0) {
                for (var j = 0; j < eventArgs.modifiedFilesData.length; j++) {
                    for (var k = 0; k < fileData.length; k++) {
                        if (eventArgs.modifiedFilesData[j].id === fileData[k].id) {
                            eventArgs.modifiedFilesData[j].rawFile = fileData[k].rawFile;
                        }
                    }
                }
                var dataFiles = this.allTypes ? eventArgs.modifiedFilesData :
                    this.checkExtension(eventArgs.modifiedFilesData);
                this.updateSortedFileList(dataFiles);
                this.filesData = dataFiles;
                if (!this.isForm || this.allowUpload()) {
                    this.checkAutoUpload(dataFiles);
                }
            }
            else {
                this.createFileList(fileData, true);
                this.filesData = this.filesData.concat(fileData);
                if (!this.isForm || this.allowUpload()) {
                    this.checkAutoUpload(fileData);
                }
            }
            if (!sf.base.isNullOrUndefined(eventArgs.progressInterval) && eventArgs.progressInterval !== '') {
                this.progressInterval = eventArgs.progressInterval;
            }
            if ((this.filesData.length === this.actionCompleteCount) && this.flag) {
                this.flag = false;
            }
        }
    };
    RteUploader.prototype.allowUpload = function () {
        var allowFormUpload = false;
        if (this.isForm && (!sf.base.isNullOrUndefined(this.asyncSettings.saveUrl) && this.asyncSettings.saveUrl !== '')) {
            allowFormUpload = true;
        }
        return allowFormUpload;
    };
    RteUploader.prototype.checkAutoUpload = function (fileData) {
        this.upload(fileData);
        this.removeActionButtons();
        this.checkActionButtonStatus();
    };
    RteUploader.prototype.updateSortedFileList = function (filesData) {
        var previousListClone = sf.base.createElement('div', { id: 'clonewrapper' });
        var added = -1;
        var removedList;
        if (this.listParent) {
            for (var i = 0; i < this.listParent.querySelectorAll('li').length; i++) {
                var liElement = this.listParent.querySelectorAll('li')[i];
                previousListClone.appendChild(liElement.cloneNode(true));
            }
            removedList = this.listParent.querySelectorAll('li');
            for (var _i = 0, removedList_1 = removedList; _i < removedList_1.length; _i++) {
                var item = removedList_1[_i];
                sf.base.detach(item);
            }
            this.removeActionButtons();
            var oldList = [].slice.call(previousListClone.childNodes);
            sf.base.detach(this.listParent);
            this.listParent = null;
            this.fileList = [];
            this.createParentUL();
            for (var index = 0; index < filesData.length; index++) {
                for (var j = 0; j < this.filesData.length; j++) {
                    if (this.filesData[j].name === filesData[index].name) {
                        this.listParent.appendChild(oldList[j]);
                        sf.base.EventHandler.add(oldList[j].querySelector('.e-icons'), 'click', this.removeFiles, this);
                        this.fileList.push(oldList[j]);
                        added = index;
                    }
                }
                if (added !== index) {
                    this.createFileList([filesData[index]]);
                }
            }
        }
        else {
            this.createFileList(filesData);
        }
    };
    RteUploader.prototype.updateInitialFileDetails = function (args, 
    // eslint-disable-next-line
    targetFiles, file, i, fileData, directory, paste$$1) {
        var fileName = directory ? targetFiles[i].path.substring(1, targetFiles[i].path.length) : paste$$1 ?
            sf.base.getUniqueID(file.name.substring(0, file.name.lastIndexOf('.'))) + '.' + this.getFileType(file.name) : file.name;
        var fileDetails = {
            name: fileName,
            rawFile: file,
            size: file.size,
            status: 'Ready to upload',
            type: this.getFileType(file.name),
            validationMessages: this.validatedFileSize(file.size),
            statusCode: '1',
            id: sf.base.getUniqueID(file.name.substring(0, file.name.lastIndexOf('.'))) + '.' + this.getFileType(file.name)
        };
        /* istanbul ignore next */
        if (paste$$1) {
            fileDetails.fileSource = 'paste';
        }
        fileDetails.status = fileDetails.validationMessages.minSize !== '' ? 'File size is too small' :
            fileDetails.validationMessages.maxSize !== '' ? 'File size is too large' : fileDetails.status;
        if (fileDetails.validationMessages.minSize !== '' || fileDetails.validationMessages.maxSize !== '') {
            fileDetails.statusCode = '0';
        }
        fileData.push(fileDetails);
    };
    RteUploader.prototype.wireEvents = function () {
        sf.base.EventHandler.add(this.element, 'change', this.onSelectFiles, this);
    };
    RteUploader.prototype.unWireEvents = function () {
        sf.base.EventHandler.remove(this.element, 'change', this.onSelectFiles);
    };
    RteUploader.prototype.destroy = function () {
        this.unWireEvents();
        this.removeEventListener();
    };
    return RteUploader;
}());

/**
 * PasteCleanup module called when pasting content in RichTextEditor
 */
var PasteCleanup = /** @class */ (function () {
    function PasteCleanup(parent) {
        this.containsHtml = false;
        this.isNotFromHtml = false;
        this.inlineNode = ['a', 'abbr', 'acronym', 'audio', 'b', 'bdi', 'bdo', 'big', 'br', 'button',
            'canvas', 'cite', 'code', 'data', 'datalist', 'del', 'dfn', 'em', 'embed', 'font', 'i', 'iframe', 'img', 'input',
            'ins', 'kbd', 'label', 'map', 'mark', 'meter', 'noscript', 'object', 'output', 'picture', 'progress',
            'q', 'ruby', 's', 'samp', 'script', 'select', 'slot', 'small', 'span', 'strong', 'sub', 'sup', 'svg',
            'template', 'textarea', 'time', 'u', 'tt', 'var', 'video', 'wbr'];
        this.blockNode = ['div', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
            'address', 'blockquote', 'button', 'center', 'dd', 'dir', 'dl', 'dt', 'fieldset',
            'frameset', 'hr', 'iframe', 'isindex', 'li', 'map', 'menu', 'noframes', 'noscript',
            'object', 'ol', 'pre', 'td', 'tr', 'th', 'tbody', 'tfoot', 'thead', 'table', 'ul',
            'header', 'article', 'nav', 'footer', 'section', 'aside', 'main', 'figure', 'figcaption'];
        this.parent = parent;
        this.addEventListener();
    }
    PasteCleanup.prototype.addEventListener = function () {
        this.nodeSelectionObj = new NodeSelection();
        this.parent.observer.on(success, this.success, this);
        this.parent.observer.on(failure, this.failure, this);
        this.parent.observer.on(selected, this.selected, this);
        this.parent.observer.on(uploading, this.uploading, this);
        this.parent.observer.on(pasteClean, this.pasteClean, this);
        this.parent.observer.on(removing, this.cancelingAndRemoving, this);
        this.parent.observer.on(canceling, this.cancelingAndRemoving, this);
        this.parent.observer.on(beforePasteUpload, this.beforeImageUpload, this);
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(afterPasteCleanUp, this.pasteUpdatedValue, this);
    };
    PasteCleanup.prototype.destroy = function () {
        this.removeEventListener();
    };
    PasteCleanup.prototype.removeEventListener = function () {
        this.parent.observer.off(success, this.success);
        this.parent.observer.off(failure, this.failure);
        this.parent.observer.off(selected, this.selected);
        this.parent.observer.off(uploading, this.uploading);
        this.parent.observer.off(pasteClean, this.pasteClean);
        this.parent.observer.off(removing, this.cancelingAndRemoving);
        this.parent.observer.off(canceling, this.cancelingAndRemoving);
        this.parent.observer.off(beforePasteUpload, this.beforeImageUpload);
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(afterPasteCleanUp, this.pasteUpdatedValue);
    };
    PasteCleanup.prototype.pasteClean = function (e) {
        var _this = this;
        var args = {
            requestType: 'Paste',
            editorMode: this.parent.editorMode,
            event: e
        };
        var value = null;
        var imageproperties;
        if (e.args && !sf.base.isNullOrUndefined(e.args.clipboardData)) {
            value = e.args.clipboardData.getData('text/html');
        }
        if (this.parent.beforePasteCleanupEnabled) {
            this.parent.dotNetRef.invokeMethodAsync('BeforePasteCleanUp', { value: value });
        }
        if (e.args && value !== null && this.parent.editorMode === 'HTML') {
            if (value.length === 0) {
                var htmlRegex = new RegExp(/<\/[a-z][\s\S]*>/i);
                value = e.args.clipboardData.getData('text/plain');
                this.isNotFromHtml = value !== '' ? true : false;
                value = value.replace(/</g, '&lt;');
                value = value.replace(/>/g, '&gt;');
                this.containsHtml = htmlRegex.test(value);
                var file = e && e.args.clipboardData &&
                    e.args.clipboardData.items.length > 0 ?
                    e.args.clipboardData.items[0].getAsFile() : null;
                this.parent.observer.notify(paste, {
                    file: file,
                    args: e.args,
                    text: value,
                    callBack: function (b) {
                        imageproperties = b;
                        if (typeof (imageproperties) === 'object') {
                            _this.parent.formatter.editorManager.execCommand('Images', 'Image', e.args, _this.imageFormatting.bind(_this, args), 'pasteCleanup', imageproperties, 'pasteCleanupModule');
                        }
                        else {
                            value = imageproperties;
                        }
                    }
                });
                if (!htmlRegex.test(value)) {
                    var divElement = sf.base.createElement('div');
                    divElement.innerHTML = this.splitBreakLine(value);
                    value = divElement.innerHTML;
                }
            }
            else if (value.length > 0) {
                this.parent.formatter.editorManager.observer.notify(MS_WORD_CLEANUP, {
                    args: e.args,
                    text: e.text,
                    allowedStylePropertiesArray: this.parent.pasteCleanupSettings.allowedStyleProps,
                    callBack: function (a) {
                        value = a;
                    }
                });
            }
            var currentDocument = document;
            var range = this.nodeSelectionObj.getRange(currentDocument);
            this.saveSelection = this.nodeSelectionObj.save(range, currentDocument);
            this.pasteArgs = args;
            e.args.preventDefault();
            if (!this.parent.afterPasteCleanupEnabled) {
                this.pasteUpdatedValue(value);
            }
            else {
                this.parent.dotNetRef.invokeMethodAsync('AfterPasteCleanUp', { value: value });
            }
        }
    };
    PasteCleanup.prototype.pasteUpdatedValue = function (args) {
        var value = this.parent.afterPasteCleanupEnabled ? args.text : args;
        if (this.parent.pasteCleanupSettings.prompt) {
            var tempDivElem = sf.base.createElement('div');
            tempDivElem.innerHTML = value;
            if (tempDivElem.textContent !== '' || !sf.base.isNullOrUndefined(tempDivElem.querySelector('img')) ||
                !sf.base.isNullOrUndefined(tempDivElem.querySelector('table'))) {
                this.pasteDialog(value, this.pasteArgs);
            }
        }
        else if (this.parent.pasteCleanupSettings.plainText) {
            this.plainFormatting(value, this.pasteArgs);
        }
        else if (this.parent.pasteCleanupSettings.keepFormat) {
            this.formatting(value, false, this.pasteArgs);
        }
        else {
            this.formatting(value, true, this.pasteArgs);
        }
    };
    PasteCleanup.prototype.splitBreakLine = function (value) {
        var enterSplitText = value.split('\n');
        var contentInnerElem = '';
        for (var i = 0; i < enterSplitText.length; i++) {
            if (enterSplitText[i].trim() === '') {
                contentInnerElem += getDefaultValue(this.parent);
            }
            else {
                var contentWithSpace = this.makeSpace(enterSplitText[i]);
                contentInnerElem += '<p>' + contentWithSpace.trim() + '</p>';
            }
        }
        return contentInnerElem;
    };
    PasteCleanup.prototype.makeSpace = function (enterSplitText) {
        var contentWithSpace = '';
        var spaceBetweenContent = true;
        var spaceSplit = enterSplitText.split(' ');
        for (var j = 0; j < spaceSplit.length; j++) {
            if (spaceSplit[j].trim() === '') {
                contentWithSpace += spaceBetweenContent ? '&nbsp;' : ' ';
            }
            else {
                spaceBetweenContent = false;
                contentWithSpace += spaceSplit[j] + ' ';
            }
        }
        return contentWithSpace;
    };
    PasteCleanup.prototype.imgUploading = function (elm) {
        var allImgElm = elm.querySelectorAll('.' + CLS_PASTED_CONTENT_IMG);
        if (allImgElm.length > 0) {
            var base64Src = [];
            var imgName = [];
            var uploadImg = [];
            for (var i = 0; i < allImgElm.length; i++) {
                if (allImgElm[i].getAttribute('src').split(',')[0].indexOf('base64') >= 0) {
                    base64Src.push(allImgElm[i].getAttribute('src'));
                    imgName.push(sf.base.getUniqueID('rte_image'));
                    uploadImg.push(allImgElm[i]);
                }
            }
            var fileList = [];
            for (var i = 0; i < base64Src.length; i++) {
                fileList.push(this.base64ToFile(base64Src[i], imgName[i]));
            }
            for (var i = 0; i < fileList.length; i++) {
                this.uploadMethod(fileList[i], uploadImg[i]);
            }
            if ((sf.base.isNullOrUndefined(this.parent.insertImageSettings.saveUrl) || sf.base.isNullOrUndefined(this.parent.insertImageSettings.path)) &&
                this.parent.insertImageSettings.saveFormat === 'Blob') {
                this.getBlob(allImgElm);
            }
        }
        else if (this.parent.insertImageSettings.saveFormat === 'Blob') {
            this.getBlob(allImgElm);
        }
        var allImgElmId = elm.querySelectorAll('.' + CLS_PASTED_CONTENT_IMG);
        for (var i = 0; i < allImgElmId.length; i++) {
            allImgElmId[i].classList.remove(CLS_PASTED_CONTENT_IMG);
            if (allImgElmId[i].getAttribute('class').trim() === '') {
                allImgElm[i].removeAttribute('class');
            }
        }
    };
    PasteCleanup.prototype.getBlob = function (allImgElm) {
        for (var i = 0; i < allImgElm.length; i++) {
            if (!sf.base.isNullOrUndefined(allImgElm[i].getAttribute('src')) &&
                allImgElm[i].getAttribute('src').split(',')[0].indexOf('base64') >= 0) {
                var blopUrl = URL.createObjectURL(convertToBlob$1(allImgElm[i].getAttribute('src')));
                allImgElm[i].setAttribute('src', blopUrl);
            }
        }
    };
    PasteCleanup.prototype.uploadMethod = function (fileList, imgElem) {
        var _this = this;
        this.uploadImgElem = imgElem;
        var uploadEle = document.createElement('div');
        document.body.appendChild(uploadEle);
        uploadEle.setAttribute('display', 'none');
        this.uploadImgElem.style.opacity = '0.5';
        var popupEle = sf.base.createElement('div');
        this.parent.element.appendChild(popupEle);
        var contentEle = sf.base.createElement('input', {
            id: this.parent.element.id + '_upload', attrs: { type: 'File', name: 'UploadFiles' }
        });
        var offsetY = this.parent.iframeSettings.enable ? -50 : -90;
        this.popupObj = new sf.popups.Popup(popupEle, {
            relateTo: this.uploadImgElem,
            height: '85px',
            width: '300px',
            offsetY: offsetY,
            content: contentEle,
            viewPortElement: this.parent.element,
            position: { X: 'center', Y: 'top' },
            enableRtl: this.parent.enableRtl,
            zIndex: 10001,
            close: function (event) {
                _this.parent.isBlur = false;
                _this.popupObj.destroy();
                sf.base.detach(_this.popupObj.element);
            }
        });
        this.popupObj.element.style.display = 'none';
        sf.base.addClass([this.popupObj.element], [CLS_POPUP_OPEN, CLS_RTE_UPLOAD_POPUP]);
        var timeOut = fileList.size > 1000000 ? 300 : 100;
        setTimeout(function () { _this.refreshPopup(imgElem, _this.popupObj); }, timeOut);
        this.rawFile = undefined;
        var uploaderOptions = {
            asyncSettings: {
                saveUrl: this.parent.insertImageSettings.saveUrl
            },
            cssClass: CLS_RTE_DIALOG_UPLOAD,
            enableRtl: this.parent.enableRtl,
            allowedExtensions: this.parent.insertImageSettings.allowedTypes.toString()
        };
        this.uploadObj = new RteUploader(uploaderOptions, this.popupObj.element.childNodes[0], this.parent);
        /* eslint-disable */
        var fileData = [{
                name: fileList.name,
                rawFile: fileList,
                size: fileList.size,
                type: fileList.type,
                validationMessages: { minSize: '', maxSize: '' },
                statusCode: '1'
            }];
        this.uploadObj.createFileList(fileData);
        this.uploadObj.filesData.push(fileData[0]);
        /* eslint-enable */
        this.rawFile = fileData;
        this.uploadObj.upload(fileData);
        this.popupObj.element.getElementsByClassName(CLS_FILE_SELECT_WRAP)[0].style.display = 'none';
        sf.base.detach(this.popupObj.element.querySelector('.' + CLS_RTE_DIALOG_UPLOAD + ' .' + CLS_FILE_SELECT_WRAP));
    };
    PasteCleanup.prototype.uploading = function (e) {
        this.parent.inputElement.contentEditable = 'false';
    };
    PasteCleanup.prototype.success = function (e) {
        var _this = this;
        setTimeout(function () { _this.popupClose(_this.popupObj, _this.uploadImgElem, e); }, 900);
    };
    PasteCleanup.prototype.failure = function (e) {
        var _this = this;
        setTimeout(function () { _this.uploadFailure(_this.uploadImgElem, _this.popupObj, e); }, 900);
    };
    PasteCleanup.prototype.selected = function (e) {
        this.rawFile = e.filesData;
    };
    PasteCleanup.prototype.cancelingAndRemoving = function (e) {
        this.parent.inputElement.contentEditable = 'true';
        if (this.uploadImgElem.nextSibling.textContent === ' ') {
            sf.base.detach(this.uploadImgElem.nextSibling);
        }
        sf.base.detach(this.uploadImgElem);
        this.popupObj.close();
    };
    /* eslint-disable */
    PasteCleanup.prototype.beforeImageUpload = function (args) {
        var _this = this;
        // @ts-ignore-end
        if (!this.parent.beforeUploadImageEnabled) {
            if (sf.base.isNullOrUndefined(this.parent.insertImageSettings.saveUrl)) {
                this.popupClose(this.popupObj, this.uploadImgElem, null);
            }
            else {
                this.parent.observer.notify(beforePasteUploadCallBack, args);
            }
            return;
        }
        var beforeUploadArgs;
        beforeUploadArgs = JSON.parse(JSON.stringify(args));
        beforeUploadArgs.filesData = this.rawFile;
        return new Promise(function (resolve) {
            _this.parent.dotNetRef.invokeMethodAsync(
            // @ts-ignore-start
            beforeUpload, beforeUploadArgs).then(function (args) {
                if (args.cancel) {
                    return;
                }
                _this.uploadObj.currentRequestHeader = args.currentRequest ?
                    args.currentRequest : _this.uploadObj.currentRequestHeader;
                _this.uploadObj.customFormDatas = args.customFormData && args.customFormData.length > 0 ?
                    args.customFormData : _this.uploadObj.customFormDatas;
                if (sf.base.isNullOrUndefined(_this.parent.insertImageSettings.saveUrl)) {
                    _this.popupClose(_this.popupObj, _this.uploadImgElem, null);
                }
                else {
                    _this.parent.observer.notify(beforePasteUploadCallBack, args);
                }
                /* eslint-enable */
                // @ts-ignore-end
                resolve();
            });
        });
    };
    PasteCleanup.prototype.uploadFailure = function (imgElem, popupObj, e) {
        this.parent.inputElement.contentEditable = 'true';
        sf.base.detach(imgElem);
        if (popupObj) {
            popupObj.close();
        }
        if (this.parent.onImageUploadFailedEnabled) {
            this.parent.dotNetRef.invokeMethodAsync(pasteImageUploadFailed, e);
        }
        this.uploadObj.destroy();
        this.uploadObj = undefined;
    };
    PasteCleanup.prototype.popupClose = function (popupObj, imgElem, e) {
        var _this = this;
        this.parent.inputElement.contentEditable = 'true';
        if (!sf.base.isNullOrUndefined(this.parent.insertImageSettings.saveUrl) && this.parent.onImageUploadSuccessEnabled) {
            // @ts-ignore-start
            this.parent.dotNetRef.invokeMethodAsync(pasteImageUploadSuccess, e).then(function (args) {
                // @ts-ignore-end
                _this.pasteUploadSuccessCallback(imgElem, e, args);
            });
        }
        else {
            this.pasteUploadSuccessCallback(imgElem, e);
        }
        popupObj.close();
        imgElem.style.opacity = '1';
        this.uploadObj.destroy();
        this.uploadObj = undefined;
    };
    // @ts-ignore-start
    PasteCleanup.prototype.pasteUploadSuccessCallback = function (imgElem, e, args) {
        // @ts-ignore-end
        if (!sf.base.isNullOrUndefined(this.parent.insertImageSettings.path)) {
            if (!sf.base.isNullOrUndefined(args) && !sf.base.isNullOrUndefined(args.file) && !sf.base.isNullOrUndefined(args.file.name)) {
                var url = this.parent.insertImageSettings.path + args.file.name;
                imgElem.src = url;
                imgElem.setAttribute('alt', args.file.name);
            }
            else {
                var url = this.parent.insertImageSettings.path + e.file.name;
                imgElem.src = url;
                imgElem.setAttribute('alt', e.file.name);
            }
        }
    };
    PasteCleanup.prototype.refreshPopup = function (imageElement, popupObj) {
        var imgPosition = this.parent.iframeSettings.enable ? this.parent.element.offsetTop +
            imageElement.offsetTop : imageElement.offsetTop;
        var rtePosition = this.parent.element.offsetTop + this.parent.element.offsetHeight;
        if (imgPosition > rtePosition) {
            popupObj.relateTo = this.parent.inputElement;
            popupObj.offsetY = this.parent.iframeSettings.enable ? -30 : -65;
            popupObj.element.style.display = 'block';
        }
        else {
            if (popupObj) {
                popupObj.refreshPosition(imageElement);
                popupObj.element.style.display = 'block';
            }
        }
    };
    PasteCleanup.prototype.base64ToFile = function (base64, filename) {
        var baseStr = base64.split(',');
        var typeStr = baseStr[0].match(/:(.*?);/)[1];
        var extension = typeStr.split('/')[1];
        var decodeStr = atob(baseStr[1]);
        var strLen = decodeStr.length;
        var decodeArr = new Uint8Array(strLen);
        while (strLen--) {
            decodeArr[strLen] = decodeStr.charCodeAt(strLen);
        }
        if (sf.base.Browser.isIE || navigator.appVersion.indexOf('Edge') > -1) {
            var blob = new Blob([decodeArr], { type: extension });
            sf.base.extend(blob, { name: filename + '.' + (!sf.base.isNullOrUndefined(extension) ? extension : '') });
            return blob;
        }
        else {
            return new File([decodeArr], filename + '.' + (!sf.base.isNullOrUndefined(extension) ? extension : ''), { type: extension });
        }
    };
    PasteCleanup.prototype.imageFormatting = function (pasteArgs, imgElement) {
        var imageElement = sf.base.createElement('span');
        imageElement.appendChild(imgElement.elements[0]);
        var imageValue = imageElement.innerHTML;
        var currentDocument = document;
        var range = this.nodeSelectionObj.getRange(currentDocument);
        this.saveSelection = this.nodeSelectionObj.save(range, currentDocument);
        if (this.parent.pasteCleanupSettings.prompt) {
            this.pasteDialog(imageValue, pasteArgs);
        }
        else if (this.parent.pasteCleanupSettings.plainText) {
            this.plainFormatting(imageValue, pasteArgs);
        }
        else if (this.parent.pasteCleanupSettings.keepFormat) {
            this.formatting(imageValue, false, pasteArgs);
        }
        else {
            this.formatting(imageValue, true, pasteArgs);
        }
    };
    PasteCleanup.prototype.selectFormatting = function (option) {
        if (this.newRTEHeight !== '') {
            this.parent.element.style.height = this.preRTEHeight;
        }
        if (option === 'KeepFormat') {
            this.formatting(this.currentValue, false, this.currentArgs);
        }
        else if (option === 'CleanFormat') {
            this.formatting(this.currentValue, true, this.currentArgs);
        }
        else if (option === 'PlainTextFormat') {
            this.plainFormatting(this.currentValue, this.currentArgs);
        }
    };
    PasteCleanup.prototype.pasteDialog = function (value, args) {
        this.currentValue = value;
        this.currentArgs = args;
        this.preRTEHeight = this.parent.height.toString();
        this.newRTEHeight = '';
        if (this.parent.element.offsetHeight < 265) {
            this.newRTEHeight = (265 + 40).toString();
        }
        if (this.newRTEHeight !== '') {
            this.parent.element.style.height = this.newRTEHeight + 'px';
        }
        this.parent.openPasteDialog();
    };
    PasteCleanup.prototype.formatting = function (value, clean, args) {
        var _this = this;
        var clipBoardElem = sf.base.createElement('div', { className: 'pasteContent', styles: 'display:inline;' });
        if (this.isNotFromHtml && this.containsHtml) {
            value = this.splitBreakLine(value);
        }
        clipBoardElem.innerHTML = value;
        if (this.parent.pasteCleanupSettings.deniedTags !== null) {
            clipBoardElem = this.deniedTags(clipBoardElem);
        }
        if (clean) {
            clipBoardElem = this.deniedAttributes(clipBoardElem, clean);
        }
        else if (this.parent.pasteCleanupSettings.deniedAttrs !== null) {
            clipBoardElem = this.deniedAttributes(clipBoardElem, clean);
        }
        if (this.parent.pasteCleanupSettings.allowedStyleProps !== null) {
            clipBoardElem = this.allowedStyle(clipBoardElem);
        }
        this.saveSelection.restore();
        clipBoardElem.innerHTML = this.sanitizeHelper(clipBoardElem.innerHTML);
        var allImg = clipBoardElem.querySelectorAll('img');
        for (var i = 0; i < allImg.length; i++) {
            allImg[i].classList.add(CLS_PASTED_CONTENT_IMG);
            if (this.parent.insertImageSettings.width != 'auto')
                allImg[i].setAttribute('width', this.parent.insertImageSettings.width);
            if (this.parent.insertImageSettings.minWidth != '0')
                allImg[i].style.minWidth = this.parent.insertImageSettings.minWidth.toString();
            if (this.parent.insertImageSettings.maxWidth != null)
                allImg[i].style.maxWidth = this.parent.imageModule.getMaxWidth().toString();
            if (this.parent.insertImageSettings.height != 'auto')
                allImg[i].setAttribute('height', this.parent.insertImageSettings.height);
            if (this.parent.insertImageSettings.minHeight != '0')
                allImg[i].style.minHeight = this.parent.insertImageSettings.minHeight.toString();
            if (this.parent.insertImageSettings.maxHeight != null)
                allImg[i].style.maxHeight = this.parent.insertImageSettings.maxHeight.toString();
        }
        this.addTempClass(clipBoardElem);
        if (clipBoardElem.textContent !== '' || !sf.base.isNullOrUndefined(clipBoardElem.querySelector('img')) ||
            !sf.base.isNullOrUndefined(clipBoardElem.querySelector('table'))) {
            this.parent.formatter.editorManager.execCommand('inserthtml', 'pasteCleanup', args, function (returnArgs) {
                sf.base.extend(args, { elements: returnArgs.elements, imageElements: returnArgs.imgElem }, true);
                _this.parent.formatter.onSuccess(_this.parent, args);
            }, clipBoardElem);
            this.removeTempClass();
            this.parent.observer.notify(toolbarRefresh, {});
            this.imgUploading(this.parent.inputElement);
        }
    };
    PasteCleanup.prototype.addTempClass = function (clipBoardElem) {
        var allChild = clipBoardElem.children;
        for (var i = 0; i < allChild.length; i++) {
            allChild[i].classList.add(CLS_RTE_PASTE_CONTENT);
        }
    };
    PasteCleanup.prototype.removeTempClass = function () {
        var classElm = this.parent.inputElement.querySelectorAll('.' + CLS_RTE_PASTE_CONTENT);
        for (var i = 0; i < classElm.length; i++) {
            classElm[i].classList.remove(CLS_RTE_PASTE_CONTENT);
            if (classElm[i].getAttribute('class') === '') {
                classElm[i].removeAttribute('class');
            }
        }
    };
    PasteCleanup.prototype.sanitizeHelper = function (value) {
        value = sanitizeHelper(value, this.parent);
        return value;
    };
    PasteCleanup.prototype.plainFormatting = function (value, args) {
        var _this = this;
        var clipBoardElem = sf.base.createElement('div', { className: 'pasteContent', styles: 'display:inline;' });
        clipBoardElem.innerHTML = value;
        this.detachInlineElements(clipBoardElem);
        this.getTextContent(clipBoardElem);
        if (clipBoardElem.textContent.trim() !== '') {
            if (!sf.base.isNullOrUndefined(clipBoardElem.firstElementChild) && clipBoardElem.firstElementChild.tagName !== 'BR') {
                var firstElm = clipBoardElem.firstElementChild;
                if (!sf.base.isNullOrUndefined(clipBoardElem.firstElementChild)) {
                    var spanElm = sf.base.createElement('span');
                    for (var i = 0, j = 0; i < firstElm.childNodes.length; i++, j++) {
                        if (firstElm.childNodes[i].nodeName === '#text') {
                            spanElm.appendChild(firstElm.childNodes[i]);
                            clipBoardElem.insertBefore(spanElm, clipBoardElem.firstElementChild);
                            i--;
                        }
                        else if (firstElm.childNodes[i].nodeName !== '#text' && j === 0) {
                            for (var k = 0; k < firstElm.childNodes[i].childNodes.length; k++) {
                                spanElm.appendChild(firstElm.childNodes[i].childNodes[k]);
                                clipBoardElem.insertBefore(spanElm, clipBoardElem.firstElementChild);
                                k--;
                            }
                            i--;
                        }
                        else {
                            break;
                        }
                    }
                    if (!firstElm.hasChildNodes()) {
                        sf.base.detach(firstElm);
                    }
                }
            }
            this.removeEmptyElements(clipBoardElem);
            this.saveSelection.restore();
            clipBoardElem.innerHTML = this.sanitizeHelper(clipBoardElem.innerHTML);
            this.addTempClass(clipBoardElem);
            this.parent.formatter.editorManager.execCommand('inserthtml', 'pasteCleanup', args, function (returnArgs) {
                sf.base.extend(args, { elements: returnArgs.elements, imageElements: returnArgs.imgElem }, true);
                _this.parent.formatter.onSuccess(_this.parent, args);
            }, clipBoardElem);
            this.removeTempClass();
        }
        else {
            this.saveSelection.restore();
            sf.base.extend(args, { elements: [] }, true);
            this.parent.formatter.onSuccess(this.parent, args);
        }
    };
    PasteCleanup.prototype.getTextContent = function (clipBoardElem) {
        for (var i = 0; i < this.blockNode.length; i++) {
            var inElem = clipBoardElem.querySelectorAll(this.blockNode[i]);
            for (var j = 0; j < inElem.length; j++) {
                var parElem = void 0;
                for (var k = 0, l = 0, preNode = void 0; k < inElem[j].childNodes.length; k++, l++) {
                    if (inElem[j].childNodes[k].nodeName === 'DIV' || inElem[j].childNodes[k].nodeName === 'P' ||
                        (inElem[j].childNodes[k].nodeName === '#text' &&
                            (inElem[j].childNodes[k].nodeValue.replace(/\u00a0/g, '&nbsp;') !== '&nbsp;') &&
                            inElem[j].childNodes[k].textContent.trim() === '')) {
                        parElem = inElem[j].childNodes[k].parentElement;
                        inElem[j].childNodes[k].parentElement.parentElement.insertBefore(inElem[j].childNodes[k], inElem[j].childNodes[k].parentElement);
                        k--;
                    }
                    else {
                        parElem = inElem[j].childNodes[k].parentElement;
                        if (preNode === 'text') {
                            var previousElem = parElem.previousElementSibling;
                            previousElem.appendChild(inElem[j].childNodes[k]);
                        }
                        else {
                            var divElement = sf.base.createElement('div', { id: 'newDiv' });
                            divElement.appendChild(inElem[j].childNodes[k]);
                            parElem.parentElement.insertBefore(divElement, parElem);
                        }
                        k--;
                        preNode = 'text';
                    }
                }
                if (!sf.base.isNullOrUndefined(parElem)) {
                    sf.base.detach(parElem);
                }
            }
        }
        var allElems = clipBoardElem.querySelectorAll('*');
        for (var i = 0; i < allElems.length; i++) {
            var allAtr = allElems[i].attributes;
            for (var j = 0; j < allAtr.length; j++) {
                allElems[i].removeAttribute(allAtr[j].name);
                j--;
            }
        }
    };
    PasteCleanup.prototype.detachInlineElements = function (clipBoardElem) {
        for (var i = 0; i < this.inlineNode.length; i++) {
            var inElem = clipBoardElem.querySelectorAll(this.inlineNode[i]);
            for (var j = 0; j < inElem.length; j++) {
                var parElem = void 0;
                for (var k = 0; k < inElem[j].childNodes.length; k++) {
                    parElem = inElem[j].childNodes[k].parentElement;
                    inElem[j].childNodes[k].parentElement.parentElement.insertBefore(inElem[j].childNodes[k], inElem[j].childNodes[k].parentElement);
                    k--;
                }
                if (!sf.base.isNullOrUndefined(parElem)) {
                    sf.base.detach(parElem);
                }
            }
        }
    };
    PasteCleanup.prototype.findDetachEmptyElem = function (element) {
        var removableElement;
        if (!sf.base.isNullOrUndefined(element.parentElement)) {
            if (element.parentElement.textContent.trim() === '' &&
                element.parentElement.getAttribute('class') !== 'pasteContent') {
                removableElement = this.findDetachEmptyElem(element.parentElement);
            }
            else {
                removableElement = element;
            }
        }
        else {
            removableElement = null;
        }
        return removableElement;
    };
    PasteCleanup.prototype.removeEmptyElements = function (element) {
        var emptyElements = element.querySelectorAll(':empty');
        for (var i = 0; i < emptyElements.length; i++) {
            if (emptyElements[i].tagName !== 'BR') {
                var detachableElement = this.findDetachEmptyElem(emptyElements[i]);
                if (!sf.base.isNullOrUndefined(detachableElement)) {
                    sf.base.detach(detachableElement);
                }
            }
        }
    };
    PasteCleanup.prototype.tagGrouping = function (deniedTags) {
        var groupingTags = deniedTags.slice();
        var keys = Object.keys(pasteCleanupGroupingTags);
        var values = keys.map(function (key) { return pasteCleanupGroupingTags[key]; });
        var addTags = [];
        for (var i = 0; i < groupingTags.length; i++) {
            //The value split using '[' because to reterive the tag name from the user given format which may contain tag with attributes
            if (groupingTags[i].split('[').length > 1) {
                groupingTags[i] = groupingTags[i].split('[')[0].trim();
            }
            if (keys.indexOf(groupingTags[i]) > -1) {
                for (var j = 0; j < values[keys.indexOf(groupingTags[i])].length; j++) {
                    if (groupingTags.indexOf(values[keys.indexOf(groupingTags[i])][j]) < 0 &&
                        addTags.indexOf(values[keys.indexOf(groupingTags[i])][j]) < 0) {
                        addTags.push(values[keys.indexOf(groupingTags[i])][j]);
                    }
                }
            }
        }
        return deniedTags = deniedTags.concat(addTags);
    };
    PasteCleanup.prototype.attributesfilter = function (deniedTags) {
        for (var i = 0; i < deniedTags.length; i++) {
            if (deniedTags[i].split('[').length > 1) {
                var userAttributes = deniedTags[i].split('[')[1].split(']')[0].split(',');
                var allowedAttributeArray = [];
                var deniedAttributeArray = [];
                for (var j = 0; j < userAttributes.length; j++) {
                    userAttributes[j].indexOf('!') < 0 ? allowedAttributeArray.push(userAttributes[j].trim())
                        : deniedAttributeArray.push(userAttributes[j].split('!')[1].trim());
                }
                var allowedAttribute = allowedAttributeArray.length > 1 ?
                    (allowedAttributeArray.join('][')) : (allowedAttributeArray.join());
                var deniedAttribute = deniedAttributeArray.length > 1 ?
                    deniedAttributeArray.join('][') : (deniedAttributeArray.join());
                if (deniedAttribute.length > 0) {
                    var select$$1 = allowedAttribute !== '' ? deniedTags[i].split('[')[0] +
                        '[' + allowedAttribute + ']' : deniedTags[i].split('[')[0];
                    deniedTags[i] = select$$1 + ':not([' + deniedAttribute + '])';
                }
                else {
                    deniedTags[i] = deniedTags[i].split('[')[0] + '[' + allowedAttribute + ']';
                }
            }
        }
        return deniedTags;
    };
    PasteCleanup.prototype.deniedTags = function (clipBoardElem) {
        var deniedTags = sf.base.isNullOrUndefined(this.parent.pasteCleanupSettings.deniedTags) ? [] : this.parent.pasteCleanupSettings.deniedTags.slice();
        deniedTags = this.attributesfilter(deniedTags);
        deniedTags = this.tagGrouping(deniedTags);
        for (var i = 0; i < deniedTags.length; i++) {
            var removableElement = clipBoardElem.querySelectorAll(deniedTags[i]);
            for (var j = removableElement.length - 1; j >= 0; j--) {
                var parentElem = removableElement[j].parentNode;
                while (removableElement[j].firstChild) {
                    parentElem.insertBefore(removableElement[j].firstChild, removableElement[j]);
                }
                parentElem.removeChild(removableElement[j]);
            }
        }
        return clipBoardElem;
    };
    PasteCleanup.prototype.deniedAttributes = function (clipBoardElem, clean) {
        var deniedAttrs = sf.base.isNullOrUndefined(this.parent.pasteCleanupSettings.deniedAttrs) ? [] : this.parent.pasteCleanupSettings.deniedAttrs.slice();
        if (clean) {
            deniedAttrs.push('style');
        }
        for (var i = 0; i < deniedAttrs.length; i++) {
            var removableAttrElement = clipBoardElem.
                querySelectorAll('[' + deniedAttrs[i] + ']');
            for (var j = 0; j < removableAttrElement.length; j++) {
                removableAttrElement[j].removeAttribute(deniedAttrs[i]);
            }
        }
        return clipBoardElem;
    };
    PasteCleanup.prototype.allowedStyle = function (clipBoardElem) {
        var allowedStyleProps = sf.base.isNullOrUndefined(this.parent.pasteCleanupSettings.allowedStyleProps) ? [] : this.parent.pasteCleanupSettings.allowedStyleProps.slice();
        allowedStyleProps.push('list-style-type', 'list-style');
        var styleElement = clipBoardElem.querySelectorAll('[style]');
        for (var i = 0; i < styleElement.length; i++) {
            var allowedStyleValue = '';
            var allowedStyleValueArray = [];
            var styleValue = styleElement[i].getAttribute('style').split(';');
            for (var k = 0; k < styleValue.length; k++) {
                if (allowedStyleProps.indexOf(styleValue[k].split(':')[0].trim()) >= 0) {
                    allowedStyleValueArray.push(styleValue[k]);
                }
            }
            styleElement[i].removeAttribute('style');
            allowedStyleValue = allowedStyleValueArray.join(';').trim() === '' ?
                allowedStyleValueArray.join(';') : allowedStyleValueArray.join(';') + ';';
            if (allowedStyleValue) {
                styleElement[i].setAttribute('style', allowedStyleValue);
            }
        }
        return clipBoardElem;
    };
    return PasteCleanup;
}());

/**
 * MarkdownSelection internal module
 *
 * @hidden
 * @deprecated
 */
var MarkdownSelection = /** @class */ (function () {
    function MarkdownSelection() {
    }
    /**
     * markdown getLineNumber method
     *
     * @param {HTMLTextAreaElement} textarea - specifies the text area element
     * @param {number} point - specifies the number value
     * @returns {number} - returns the value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.getLineNumber = function (textarea, point) {
        return textarea.value.substr(0, point).split('\n').length;
    };
    /**
     * markdown getSelectedText method
     *
     * @param {HTMLTextAreaElement} textarea - specifies the text area element
     * @returns {string} - specifies the string value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.getSelectedText = function (textarea) {
        var start = textarea.selectionStart;
        var end = textarea.selectionEnd;
        return textarea.value.substring(start, end);
    };
    /**
     * markdown getAllParents method
     *
     * @param {string} value - specifies the string value
     * @returns {string[]} - returns the string value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.getAllParents = function (value) {
        return value.split('\n');
    };
    /**
     * markdown getSelectedLine method
     *
     * @param {HTMLTextAreaElement} textarea - specifies the text area element
     * @returns {string} - returns the string value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.getSelectedLine = function (textarea) {
        var lines = this.getAllParents(textarea.value);
        var index = this.getLineNumber(textarea, textarea.selectionStart);
        return lines[index - 1];
    };
    /**
     * markdown getLine method
     *
     * @param {HTMLTextAreaElement} textarea - specifies the text area element
     * @param {number} index - specifies the number value
     * @returns {string} - returns the string value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.getLine = function (textarea, index) {
        var lines = this.getAllParents(textarea.value);
        return lines[index];
    };
    /**
     * markdown getSelectedParentPoints method
     *
     * @param {HTMLTextAreaElement} textarea - specifies the text area element
     * @returns {string} - returns the string value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.getSelectedParentPoints = function (textarea) {
        var lines = this.getAllParents(textarea.value);
        var start = this.getLineNumber(textarea, textarea.selectionStart);
        var end = this.getLineNumber(textarea, textarea.selectionEnd);
        var parents = this.getSelectedText(textarea).split('\n');
        var selectedPoints = [];
        var selectedLine = lines[start - 1];
        var startLength = lines.slice(0, start - 1).join('').length;
        var firstPoint = {};
        firstPoint.line = start - 1;
        firstPoint.start = startLength + firstPoint.line;
        firstPoint.end = selectedLine !== '' ? firstPoint.start +
            selectedLine.length + 1 : firstPoint.start + selectedLine.length;
        firstPoint.text = selectedLine;
        selectedPoints.push(firstPoint);
        if (parents.length > 1) {
            for (var i = 1; i < parents.length - 1; i++) {
                var points = {};
                points.line = selectedPoints[i - 1].line + 1;
                points.start = parents[i] !== '' ? selectedPoints[i - 1].end : selectedPoints[i - 1].end;
                points.end = points.start + parents[i].length + 1;
                points.text = parents[i];
                selectedPoints.push(points);
            }
            var lastPoint = {};
            lastPoint.line = selectedPoints[selectedPoints.length - 1].line + 1;
            lastPoint.start = selectedPoints[selectedPoints.length - 1].end;
            lastPoint.end = lastPoint.start + lines[end - 1].length + 1;
            lastPoint.text = lines[end - 1];
            selectedPoints.push(lastPoint);
        }
        return selectedPoints;
    };
    /**
     * markdown setSelection method
     *
     * @param {HTMLTextAreaElement} textarea - specifies the text area element
     * @param {number} start - specifies the start vaulue
     * @param {number} end - specifies the end value
     * @returns {void}
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.setSelection = function (textarea, start, end) {
        textarea.setSelectionRange(start, end);
        textarea.focus();
    };
    /**
     * markdown save method
     *
     * @param {number} start - specifies the start vaulue
     * @param {number} end - specifies the end value
     * @returns {void}
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.save = function (start, end) {
        this.selectionStart = start;
        this.selectionEnd = end;
    };
    /**
     * markdown restore method
     *
     * @param {HTMLTextAreaElement} textArea - specifies the text area element
     * @returns {void}
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.restore = function (textArea) {
        this.setSelection(textArea, this.selectionStart, this.selectionEnd);
    };
    /**
     * markdown isStartWith method
     *
     * @param {string} line - specifies the string value
     * @param {string} command - specifies the string value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.isStartWith = function (line, command) {
        var isStart = false;
        if (line) {
            var reg = line.trim() === command.trim() ?
                new RegExp('^(' + this.replaceSpecialChar(command.trim()) + ')', 'gim') :
                new RegExp('^(' + this.replaceSpecialChar(command) + ')', 'gim');
            isStart = reg.test(line.trim());
        }
        return isStart;
    };
    /**
     * markdown replaceSpecialChar method
     *
     * @param {string} value - specifies the string value
     * @returns {string} - returns the value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.replaceSpecialChar = function (value) {
        // eslint-disable-next-line
        return value.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/g, '\\$&');
    };
    /**
     * markdown isClear method
     *
     * @param {string} parents - specifies the parent element
     * @param {string} regex - specifies the regex value
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.isClear = function (parents, regex) {
        var isClear = false;
        for (var i = 0; i < parents.length; i++) {
            if (new RegExp(regex, 'gim').test(parents[i].text)) {
                return true;
            }
        }
        return isClear;
    };
    /**
     * markdown getSelectedInlinePoints method
     *
     * @param {HTMLTextAreaElement} textarea - specifies the text area
     * @returns {void}
     * @hidden
     * @deprecated
     */
    MarkdownSelection.prototype.getSelectedInlinePoints = function (textarea) {
        var start = textarea.selectionStart;
        var end = textarea.selectionEnd;
        var selection = this.getSelectedText(textarea);
        return { start: start, end: end, text: selection };
    };
    return MarkdownSelection;
}());

/**
 * MarkdownToolbarStatus module for refresh the toolbar status
 */
var MarkdownToolbarStatus = /** @class */ (function () {
    function MarkdownToolbarStatus(parent) {
        this.toolbarStatus = this.prevToolbarStatus = getDefaultMDTbStatus();
        this.selection = new MarkdownSelection();
        this.parent = parent;
        this.element = this.parent.getEditPanel();
        this.addEventListener();
    }
    MarkdownToolbarStatus.prototype.addEventListener = function () {
        this.parent.observer.on(toolbarRefresh, this.onRefreshHandler, this);
        this.parent.observer.on(destroy, this.removeEventListener, this);
    };
    MarkdownToolbarStatus.prototype.removeEventListener = function () {
        this.parent.observer.off(toolbarRefresh, this.onRefreshHandler);
        this.parent.observer.off(destroy, this.removeEventListener);
    };
    MarkdownToolbarStatus.prototype.onRefreshHandler = function (args) {
        var parentsLines = this.selection.getSelectedParentPoints(this.element);
        this.toolbarStatus = {
            orderedlist: args.documentNode ? false : this.isListsApplied(parentsLines, 'OL'),
            unorderedlist: args.documentNode ? false : this.isListsApplied(parentsLines, 'UL'),
            formats: this.currentFormat(parentsLines, args.documentNode),
            bold: args.documentNode ? false : this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('Bold'),
            italic: args.documentNode ? false : this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('Italic'),
            inlinecode: args.documentNode ? false : this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('InlineCode'),
            strikethrough: args.documentNode ? false :
                this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('StrikeThrough'),
            subscript: args.documentNode ? false : this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('SubScript'),
            superscript: args.documentNode ? false : this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('SuperScript'),
            uppercase: args.documentNode ? false : this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('UpperCase')
        };
        if (this.parent.formatter.editorManager.mdSelectionFormats.isAppliedCommand('InlineCode')) {
            this.toolbarStatus.formats = 'pre';
        }
        var tbStatusString = JSON.stringify(this.toolbarStatus);
        this.parent.observer.notify(toolbarUpdated, this.toolbarStatus);
        if (JSON.stringify(this.prevToolbarStatus) !== tbStatusString) {
            this.parent.observer.notify(updateTbItemsStatus, { html: null, markdown: JSON.parse(tbStatusString) });
            this.prevToolbarStatus = JSON.parse(tbStatusString);
        }
    };
    MarkdownToolbarStatus.prototype.isListsApplied = function (lines, type) {
        var isApply = true;
        if (type === 'OL') {
            for (var i = 0; i < lines.length; i++) {
                var lineSplit = lines[i].text.trim().split(' ', 2)[0] + ' ';
                if (!/^[\d.]+[ ]+$/.test(lineSplit)) {
                    isApply = false;
                    break;
                }
            }
        }
        else {
            for (var i = 0; i < lines.length; i++) {
                if (!this.selection.isStartWith(lines[i].text, this.parent.formatter.listSyntax[type])) {
                    isApply = false;
                    break;
                }
            }
        }
        return isApply;
    };
    MarkdownToolbarStatus.prototype.currentFormat = function (lines, documentNode) {
        var format = 'p';
        var keys = Object.keys(this.parent.formatter.formatSyntax);
        var direction = this.element.selectionDirection;
        var checkLine = direction === 'backward' ? lines[0].text : lines[lines.length - 1].text;
        for (var i = 0; !documentNode && i < keys.length; i++) {
            if (keys[i] !== 'pre' && this.selection.isStartWith(checkLine, this.parent.formatter.formatSyntax[keys[i]])) {
                format = keys[i];
                break;
            }
            else if (keys[i] === 'pre') {
                if (this.codeFormat()) {
                    format = keys[i];
                    break;
                }
            }
        }
        return format;
    };
    MarkdownToolbarStatus.prototype.codeFormat = function () {
        var isFormat = false;
        var textArea = this.parent.inputElement;
        var start = textArea.selectionStart;
        var splitAt = function (index) { return function (x) { return [x.slice(0, index), x.slice(index)]; }; };
        var splitText = splitAt(start)(textArea.value);
        var cmdPre = this.parent.formatter.formatSyntax.pre;
        var selectedText = this.getSelectedText(textArea);
        if (selectedText !== '' && selectedText === selectedText.toLocaleUpperCase()) {
            return true;
        }
        else if (selectedText === '') {
            var beforeText = textArea.value.substr(splitText[0].length - 1, 1);
            var afterText = splitText[1].substr(0, 1);
            if ((beforeText !== '' && afterText !== '' && beforeText.match(/[a-z]/i)) &&
                beforeText === beforeText.toLocaleUpperCase() && afterText === afterText.toLocaleUpperCase()) {
                return true;
            }
        }
        if ((this.isCode(splitText[0], cmdPre) && this.isCode(splitText[1], cmdPre)) &&
            (splitText[0].match(this.multiCharRegx(cmdPre)).length % 2 === 1 &&
                splitText[1].match(this.multiCharRegx(cmdPre)).length % 2 === 1)) {
            isFormat = true;
        }
        return isFormat;
    };
    MarkdownToolbarStatus.prototype.getSelectedText = function (textarea) {
        return textarea.value.substring(textarea.selectionStart, textarea.selectionEnd);
    };
    MarkdownToolbarStatus.prototype.isCode = function (text, cmd) {
        return text.search('\\' + cmd + '') !== -1;
    };
    MarkdownToolbarStatus.prototype.multiCharRegx = function (cmd) {
        return new RegExp('(\\' + cmd + ')', 'g');
    };
    return MarkdownToolbarStatus;
}());

/**
 * Constant values for Markdown Parser
 */
/**
 * List plugin events
 *
 * @hidden
 */
var LISTS_COMMAND = 'lists-commands';
/**
 * selectioncommand plugin events
 *
 * @hidden
 */
var selectionCommand = 'command-type';
/**
 * Link plugin events
 *
 * @hidden
 */
var LINK_COMMAND = 'link-commands';
/**
 * Clear plugin events
 *
 * @hidden
 */
var CLEAR_COMMAND = 'clear-commands';
/**
 * Table plugin events
 *
 * @hidden
 */
var MD_TABLE = 'insert-table';

/**
 * Lists internal component
 *
 * @hidden
 */
var MDLists = /** @class */ (function () {
    /**
     * Constructor for creating the Lists plugin
     *
     * @param {IMDFormats} options - specifies the options
     * @hidden
     */
    function MDLists(options) {
        sf.base.extend(this, this, options, true);
        this.selection = this.parent.markdownSelection;
        this.addEventListener();
    }
    MDLists.prototype.addEventListener = function () {
        this.parent.observer.on(LISTS_COMMAND, this.applyListsHandler, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.keyDownHandler, this);
        this.parent.observer.on(KEY_UP_HANDLER, this.keyUpHandler, this);
    };
    MDLists.prototype.keyDownHandler = function (event) {
        switch (event.event.which) {
            case 9:
                this.tabKey(event);
                break;
        }
        switch (event.event.action) {
            case 'ordered-list':
                this.applyListsHandler({ subCommand: 'OL', callBack: event.callBack });
                event.event.preventDefault();
                break;
            case 'unordered-list':
                this.applyListsHandler({ subCommand: 'UL', callBack: event.callBack });
                event.event.preventDefault();
                break;
        }
    };
    MDLists.prototype.keyUpHandler = function (event) {
        switch (event.event.which) {
            case 13:
                this.enterKey(event);
                break;
        }
    };
    MDLists.prototype.tabKey = function (event) {
        var textArea = this.parent.element;
        this.selection.save(textArea.selectionStart, textArea.selectionEnd);
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var parents = this.selection.getSelectedParentPoints(textArea);
        var addedLength = 0;
        var isNotFirst = this.isNotFirstLine(textArea, parents[0]);
        if (!isNotFirst && !event.event.shiftKey) {
            this.restore(textArea, start, end + addedLength, event);
            return;
        }
        var listFormat = this.olListType();
        var regex = this.getListRegex();
        this.currentAction = this.getAction(parents[0].text);
        for (var i = 0; i < parents.length; i++) {
            var prevIndex = event.event.shiftKey ? parents[i].line - 1 : parents[i].line - 1;
            var prevLine = this.selection.getLine(textArea, prevIndex);
            if (prevLine && (!event.event.shiftKey && isNotFirst || (event.event.shiftKey))) {
                var prevLineSplit = prevLine.split('. ');
                var tabSpace = '\t';
                var tabSpaceLength = event.event.shiftKey ? -tabSpace.length : tabSpace.length;
                var splitTab = parents[i].text.split('\t');
                if (event.event.shiftKey && splitTab.length === 1) {
                    break;
                }
                if (this.currentAction === 'OL' && /^\d+$/.test(prevLineSplit[0].trim()) && listFormat) {
                    event.event.preventDefault();
                    parents[i].text = event.event.shiftKey ? splitTab.splice(1, splitTab.length).join('\t') : tabSpace + parents[i].text;
                    var curTabSpace = this.getTabSpace(parents[i].text);
                    var prevTabSpace = this.getTabSpace(prevLine);
                    var splitText = parents[i].text.split('. ');
                    if (curTabSpace === prevTabSpace) {
                        this.changeTextAreaValue(splitText, this.nextOrderedListValue(prevLineSplit[0].trim()), event, textArea, parents, i, end);
                    }
                    else if (prevTabSpace < curTabSpace) {
                        this.changeTextAreaValue(splitText, '1. ', event, textArea, parents, i, end);
                    }
                    else {
                        for (; prevTabSpace.length > curTabSpace.length; null) {
                            prevIndex = prevIndex - 1;
                            prevLine = this.selection.getLine(textArea, prevIndex);
                            var prevLineSplit_1 = prevLine.trim().split('. ');
                            if (/^\d+$/.test(prevLineSplit_1[0])) {
                                prevTabSpace = this.getTabSpace(prevLine);
                                if (prevTabSpace.length <= curTabSpace.length) {
                                    this.changeTextAreaValue(splitText, this.nextOrderedListValue(prevLineSplit_1[0]), event, textArea, parents, i, end);
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (this.currentAction === 'UL' && regex.test(prevLine.trim()) || !listFormat) {
                    event.event.preventDefault();
                    parents[i].text = event.event.shiftKey ? splitTab.splice(1, splitTab.length).join('\t') : tabSpace + parents[i].text;
                    textArea.value = textArea.value.substr(0, parents[i].start) + parents[i].text + '\n' +
                        textArea.value.substr(parents[i].end, textArea.value.length);
                }
                start = i === 0 ? start + tabSpaceLength : start;
                addedLength += tabSpaceLength;
                if (parents.length !== 1) {
                    for (var j = i; j < parents.length; j++) {
                        parents[j].start = j !== 0 ? parents[j].start + tabSpaceLength : parents[j].start;
                        parents[j].end = parents[j].end + tabSpaceLength;
                    }
                }
            }
        }
        this.restore(textArea, start, end + addedLength, event);
    };
    MDLists.prototype.changeTextAreaValue = function (splitText, prefixValue, event, 
    // eslint-disable-next-line
    textArea, parents, k, end) {
        var prefix = prefixValue;
        splitText.splice(0, 1);
        var textAreaLength = this.selection.getAllParents(textArea.value).length;
        var changedList = '';
        var curTabSpace = this.getTabSpace(parents[k].text);
        // eslint-disable-next-line
        var prefixNumber = parseInt(prefix.split('.')[0], null);
        var nestedTabSpace = this.getTabSpace(parents[k].text);
        var nestedlistorder = true;
        var nestedListStart = true;
        var curTabSpaceLength;
        var nextPrefixValue = -1;
        var traversIncreased = true;
        var nextLineLength = 0;
        var lineBreak = '';
        changedList = (this.selection.getLine(textArea, parents[0].line + 1) !== '') ?
            '' : changedList + textArea.value.substr(parents[0].end, textArea.value.length);
        for (var i = 1; i < textAreaLength &&
            !sf.base.isNullOrUndefined(this.selection.getLine(textArea, parents[0].line + i))
            && this.selection.getLine(textArea, parents[0].line + i) !== ''; i++) {
            var nextLine = this.selection.getLine(textArea, parents[0].line + i);
            var nextTabSpace = this.getTabSpace(nextLine);
            var nextLineSplit = nextLine.split('. ');
            if (nextLineSplit.length === 1) {
                changedList += textArea.value.substr(parents[0].end + nextLineLength, textArea.value.length);
                break;
            }
            else {
                nextLineLength += nextLine.length;
                var shiftTabTargetList = false;
                curTabSpaceLength = event.event.shiftKey ? curTabSpace.length + 1 : curTabSpace.length - 1;
                if (nextTabSpace.length > nestedTabSpace.length) {
                    traversIncreased = false;
                }
                if (curTabSpace.length !== nextTabSpace.length && nextTabSpace.length < nestedTabSpace.length) {
                    nestedListStart = true;
                    nestedlistorder = false;
                    shiftTabTargetList = event.event.shiftKey &&
                        curTabSpace.length === nextTabSpace.length ? (nestedListStart = false, true) : false;
                }
                else if (traversIncreased && event.event.shiftKey &&
                    curTabSpace.length === nextTabSpace.length && nextTabSpace.length === nestedTabSpace.length) {
                    nestedListStart = false;
                    shiftTabTargetList = true;
                }
                lineBreak = changedList === '' ? '' : '\n';
                if (curTabSpaceLength === nextTabSpace.length && nestedListStart) {
                    var nextPrefix = event.event.shiftKey ?
                        (nextPrefixValue++, this.nextOrderedListValue(nextPrefixValue.toString()))
                        : this.previousOrderedListValue(nextLineSplit[0]);
                    nextLineSplit.splice(0, 1);
                    changedList = changedList + lineBreak + nextTabSpace + nextPrefix + nextLineSplit.join('. ');
                }
                else if (curTabSpace.length === nextTabSpace.length && nestedlistorder || shiftTabTargetList) {
                    var nextPrefix = this.nextOrderedListValue(prefixNumber.toString());
                    prefixNumber++;
                    nextLineSplit.splice(0, 1);
                    changedList = changedList + lineBreak + nextTabSpace + nextPrefix + nextLineSplit.join('. ');
                }
                else {
                    changedList = changedList + lineBreak + nextLine;
                    nestedListStart = false;
                }
                nestedTabSpace = this.getTabSpace(nextLine);
            }
        }
        parents[k].text = this.getTabSpace(parents[k].text) + prefix + splitText.join('. ') + '\n';
        textArea.value = textArea.value.substr(0, parents[k].start) + parents[k].text + changedList;
    };
    MDLists.prototype.getTabSpace = function (line) {
        var split = line.split('\t');
        var tabs = '';
        for (var i = 0; i < split.length; i++) {
            if (split[i] === '') {
                tabs += '\t';
            }
            else {
                break;
            }
        }
        return tabs;
    };
    MDLists.prototype.isNotFirstLine = function (textArea, points) {
        var currentLine = points.text;
        var prevIndex = points.line - 1;
        var prevLine = this.selection.getLine(textArea, prevIndex);
        var regex = this.getListRegex();
        var isNotFirst = false;
        var regexFirstCondition;
        if (prevLine) {
            this.currentAction = this.getAction(prevLine);
            var prevLineSplit = prevLine.split('. ');
            regexFirstCondition = this.currentAction === 'OL' ? /^\d+$/.test(prevLineSplit[0].trim()) : regex.test(prevLine.trim());
        }
        if (prevLine && regexFirstCondition) {
            var curTabSpace = this.getTabSpace(currentLine);
            var prevTabSpace = this.getTabSpace(prevLine);
            isNotFirst = curTabSpace === prevTabSpace ? true : isNotFirst;
            for (; prevTabSpace.length > curTabSpace.length; null) {
                prevIndex = prevIndex - 1;
                prevLine = this.selection.getLine(textArea, prevIndex);
                var prevLineSplit = prevLine.trim().split('. ');
                var regexSecondCondition = this.currentAction === 'OL' ?
                    /^\d+$/.test(prevLineSplit[0]) : regex.test(prevLine.trim());
                if (regexSecondCondition) {
                    prevTabSpace = this.getTabSpace(prevLine);
                    if (prevTabSpace.length <= curTabSpace.length) {
                        isNotFirst = true;
                        break;
                    }
                }
            }
        }
        return isNotFirst;
    };
    MDLists.prototype.getAction = function (line) {
        var ol = line.split('. ')[0];
        // eslint-disable-next-line
        var currentState = /^\d+$/.test(ol.trim());
        var ul = line.trim().split(new RegExp('^(' + this.selection.replaceSpecialChar(this.syntax.UL).trim() + ')'))[1];
        return (currentState ? 'OL' : ul ? 'UL' : 'NOTLIST');
    };
    MDLists.prototype.nextOrderedListValue = function (previousLine) {
        // eslint-disable-next-line
        var currentValue = parseInt(previousLine, null);
        var nextValue = currentValue + 1;
        return nextValue.toString() + '. ';
    };
    MDLists.prototype.previousOrderedListValue = function (previousLine) {
        // eslint-disable-next-line
        var currentValue = parseInt(previousLine, null);
        var nextValue = currentValue - 1;
        return nextValue.toString() + '. ';
    };
    MDLists.prototype.enterKey = function (event) {
        var textArea = this.parent.element;
        this.selection.save(textArea.selectionStart, textArea.selectionEnd);
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var parents = this.selection.getSelectedParentPoints(textArea);
        var prevLine = this.selection.getLine(textArea, parents[0].line - 1);
        var listFormat = this.olListType();
        var regex = this.getListRegex();
        var prevLineSplit = [];
        if (!sf.base.isNullOrUndefined(prevLine)) {
            prevLineSplit = prevLine.split('. ');
            this.currentAction = this.getAction(prevLine);
        }
        var addedLength = 0;
        if (this.currentAction === 'OL' && prevLineSplit.length > 1 && /^\d+$/.test(prevLineSplit[0].trim()) && listFormat
            && prevLineSplit[1] !== '') {
            var tabSpace = this.getTabSpace(prevLine);
            this.currentAction = this.getAction(prevLine);
            var prefix = this.nextOrderedListValue(prevLineSplit[0]);
            parents[0].text = tabSpace + prefix + parents[0].text;
            var textAreaLength = this.selection.getAllParents(textArea.value).length;
            var changedList = '\n';
            var curTabSpace = this.getTabSpace(prevLine);
            var nestedTabSpace = this.getTabSpace(parents[0].text);
            var nestedListOrder = true;
            for (var i = 1; i < textAreaLength &&
                textArea.value.substr(parents[0].end, textArea.value.length) !== ''; i++) {
                var nextLine = this.selection.getLine(textArea, parents[0].line + i);
                if (sf.base.isNullOrUndefined(nextLine)) {
                    changedList = changedList + '';
                }
                else {
                    var nextLineSplit = nextLine.split('. ');
                    var nextTabSpace = this.getTabSpace(nextLine);
                    if (nextTabSpace.length < nestedTabSpace.length) {
                        nestedListOrder = false;
                    }
                    if (nextLineSplit.length > 1 && /^\d+$/.test(nextLineSplit[0].trim()) &&
                        curTabSpace.length === nextTabSpace.length && nestedListOrder) {
                        var nextPrefix = this.nextOrderedListValue(nextLineSplit[0]);
                        nextLineSplit.splice(0, 1);
                        changedList = changedList + nextTabSpace + nextPrefix + nextLineSplit.join('. ') + '\n';
                    }
                    else {
                        changedList = changedList + nextLine + '\n';
                        nestedTabSpace = this.getTabSpace(nextLine);
                    }
                }
            }
            textArea.value = textArea.value.substr(0, parents[0].start) + curTabSpace +
                prefix + this.selection.getLine(textArea, parents[0].line) + changedList;
            start = start + prefix.length + tabSpace.length;
            addedLength += prefix.length + tabSpace.length;
        }
        else if (this.currentAction === 'UL' && (prevLine && regex.test(prevLine.trim())) &&
            prevLine.trim().replace(regex, '') !== '' || this.currentAction === 'OL' && !listFormat) {
            var tabSpace = this.getTabSpace(prevLine);
            var prefix = this.syntax[this.currentAction];
            parents[0].text = tabSpace + prefix + parents[0].text +
                (parents[0].text.trim().length > 0 ? '\n' : '');
            textArea.value = textArea.value.substr(0, parents[0].start) + parents[0].text +
                textArea.value.substr(parents[0].end, textArea.value.length);
            start = start + prefix.length + tabSpace.length;
            addedLength += prefix.length + tabSpace.length;
        }
        this.restore(textArea, start, end + addedLength, event);
    };
    MDLists.prototype.olListType = function () {
        var olSyntaxList = this.syntax.OL.split('.,');
        var listType = olSyntaxList.length === 1 ? null :
            // eslint-disable-next-line
            parseInt(olSyntaxList[2].trim(), null) - parseInt(olSyntaxList[0].trim(), null);
        if (listType) {
            return 1;
        }
        else {
            return 0;
        }
    };
    MDLists.prototype.applyListsHandler = function (e) {
        var textArea = this.parent.element;
        this.selection.save(textArea.selectionStart, textArea.selectionEnd);
        this.currentAction = e.subCommand;
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var addedLength = 0;
        var startLength = 0;
        var endLength = 0;
        var parents = this.selection.getSelectedParentPoints(textArea);
        var prefix = '';
        var listFormat = this.olListType();
        var regex;
        var perfixObj = {};
        for (var i = 0; i < parents.length; i++) {
            if (listFormat) {
                regex = this.currentAction === 'OL' ? i + listFormat + '. ' : this.syntax[this.currentAction];
            }
            else {
                regex = this.currentAction === 'OL' ? this.syntax.OL : this.syntax[this.currentAction];
            }
            if (!this.selection.isStartWith(parents[i].text, regex)) {
                if (parents[i].text === '' && i === 0) {
                    this.selection.save(start, end);
                    if (parents.length !== 1) {
                        for (var j = i; j < parents.length; j++) {
                            parents[j].start = j !== 0 ? 1 + parents[j].start : parents[j].start;
                            parents[j].end = 1 + parents[j].end;
                        }
                    }
                }
                var preLineTabSpaceLength = !sf.base.isNullOrUndefined(parents[i - 1]) ?
                    this.getTabSpace(parents[i - 1].text).length : 0;
                var replace = this.appliedLine(parents[i].text, regex, perfixObj, preLineTabSpaceLength);
                prefix = replace.line ? prefix : regex;
                parents[i].text = replace.line ? replace.line : prefix + parents[i].text;
                replace.space = replace.space ? replace.space : 0;
                textArea.value = textArea.value.substr(0, parents[i].start + endLength) + parents[i].text + '\n' +
                    textArea.value.substr(parents[i].end, textArea.value.length);
                start = i === 0 ? (start + prefix.length + replace.space) > 0 ?
                    start + prefix.length + replace.space : 0 : start;
                addedLength += prefix.length + replace.space;
                if (parents.length !== 1) {
                    for (var j = i; j < parents.length; j++) {
                        parents[j].start = j !== 0 ? prefix.length +
                            parents[j].start + replace.space : parents[j].start;
                        parents[j].end = prefix.length + parents[j].end + replace.space;
                    }
                }
                this.restore(textArea, start, end + addedLength, null);
            }
            else {
                parents[i].text = parents[i].text.replace(regex, '');
                textArea.value = textArea.value.substr(0, parents[i].start + endLength) + parents[i].text + '\n' +
                    textArea.value.substr(parents[i].end + endLength, textArea.value.length);
                endLength -= regex.length;
                startLength = regex.length;
                this.restore(textArea, start - startLength, end + endLength, null);
            }
        }
        this.restore(textArea, null, null, e);
    };
    MDLists.prototype.appliedLine = function (line, prefixPattern, perfixObj, preTabSpaceLength) {
        var points = {};
        var regex = new RegExp('^[' + this.syntax.UL.trim() + ']');
        var lineSplit = line.split('. ');
        var currentPrefix = lineSplit[0] + '. ';
        var isExist = regex.test(line.trim()) || line.trim() === this.syntax.OL.trim()
            || line.trim() === this.syntax.UL.trim() || /^\d+$/.test(lineSplit[0].trim());
        var listFormat = this.olListType();
        var curTabSpaceLength = this.getTabSpace(line).length;
        if (this.currentAction === 'OL' && listFormat) {
            perfixObj[curTabSpaceLength.toString()] = !sf.base.isNullOrUndefined(perfixObj[curTabSpaceLength.toString()]) ?
                perfixObj[curTabSpaceLength.toString()].valueOf() + 1 : 1;
            prefixPattern = perfixObj[curTabSpaceLength.toString()].valueOf().toString() + '. ';
            if (!sf.base.isNullOrUndefined(preTabSpaceLength) && preTabSpaceLength > curTabSpaceLength) {
                perfixObj[preTabSpaceLength.toString()] = 0;
            }
        }
        if (isExist) {
            var replace = void 0;
            var pattern = void 0;
            // eslint-disable-next-line
            if (regex.test(line.trim())) {
                pattern = this.syntax.UL;
                replace = prefixPattern;
                points.space = prefixPattern.trim().length - this.syntax.UL.trim().length;
            }
            else if (/^\d+$/.test(lineSplit[0].trim()) && listFormat) {
                pattern = lineSplit[0].trim() + '. ';
                replace = prefixPattern;
                points.space = this.syntax.UL.trim().length - currentPrefix.trim().length;
            }
            else if (/^\d+$/.test(lineSplit[0].trim())) {
                pattern = lineSplit[0].trim() + '. ';
                replace = this.syntax.UL;
                points.space = this.syntax.UL.trim().length - currentPrefix.trim().length;
            }
            points.line = line.replace(pattern, replace);
        }
        return points;
    };
    MDLists.prototype.restore = function (textArea, start, end, event) {
        if (!sf.base.isNullOrUndefined(start) && !sf.base.isNullOrUndefined(start)) {
            this.selection.save(start, end);
        }
        if (!sf.base.isNullOrUndefined(event)) {
            this.selection.restore(textArea);
        }
        if (event && event.callBack) {
            event.callBack({
                requestType: this.currentAction,
                selectedText: this.selection.getSelectedText(textArea),
                editorMode: 'Markdown',
                event: event.event
            });
        }
    };
    MDLists.prototype.getListRegex = function () {
        var regex = '';
        var configKey = Object.keys(this.syntax);
        for (var j = 0; j < configKey.length; j++) {
            var syntax = this.selection.replaceSpecialChar(this.syntax[configKey[j]]);
            regex += regex === '' ? '^(' + syntax + ')|^(' + syntax.trim() + ')' :
                '|^(' + syntax + ')|^(' + syntax.trim() + ')';
        }
        return new RegExp(regex);
    };
    return MDLists;
}());

/**
 * MDFormats internal plugin
 *
 * @hidden
 * @deprecated
 */
var MDFormats = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {IMDFormats} options - specifies the formats
     * @hidden
     * @deprecated
     */
    function MDFormats(options) {
        sf.base.extend(this, this, options, true);
        this.selection = this.parent.markdownSelection;
        this.addEventListener();
    }
    MDFormats.prototype.addEventListener = function () {
        this.parent.observer.on(FORMAT_TYPE, this.applyFormats, this);
    };
    MDFormats.prototype.applyFormats = function (e) {
        e.subCommand = e.subCommand.toLowerCase();
        var textArea = this.parent.element;
        this.selection.save(textArea.selectionStart, textArea.selectionEnd);
        var parents = this.selection.getSelectedParentPoints(textArea);
        if (this.isAppliedFormat(parents) === e.subCommand) {
            if (e.subCommand === 'pre') {
                if (parents.length > 1) {
                    this.applyCodeBlock(textArea, e, parents);
                }
                else {
                    return;
                }
            }
            this.cleanFormat(textArea);
            this.restore(textArea, textArea.selectionStart, textArea.selectionEnd, e);
            return;
        }
        if (e.subCommand === 'p') {
            this.cleanFormat(textArea);
            this.restore(textArea, textArea.selectionStart, textArea.selectionEnd, e);
            return;
        }
        else {
            if ((e.subCommand === 'pre' && parents.length !== 1) || e.subCommand !== 'pre') {
                this.cleanFormat(textArea, e.subCommand);
            }
        }
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var addedLength = 0;
        parents = this.selection.getSelectedParentPoints(textArea);
        if (e.subCommand === 'pre') {
            if (parents.length > 1) {
                this.applyCodeBlock(textArea, e, parents);
            }
            else {
                sf.base.extend(e, e, { subCommand: 'InlineCode' }, true);
                this.parent.observer.notify(selectionCommand, e);
            }
            return;
        }
        for (var i = 0; i < parents.length; i++) {
            if (parents[i].text !== '' && !this.selection.isStartWith(parents[i].text, '\\' + this.syntax[e.subCommand])) {
                parents[i].text = this.syntax[e.subCommand] + parents[i].text;
                textArea.value = textArea.value.substr(0, parents[i].start) + parents[i].text + '\n' +
                    textArea.value.substr(parents[i].end, textArea.value.length);
                start = i === 0 ? start + this.syntax[e.subCommand].length : start;
                addedLength += this.syntax[e.subCommand].length;
                if (parents.length !== 1) {
                    for (var j = i; j < parents.length; j++) {
                        parents[j].start = j !== 0 ?
                            this.syntax[e.subCommand].length + parents[j].start : parents[j].start;
                        parents[j].end = this.syntax[e.subCommand].length + parents[j].end;
                    }
                }
            }
            else if (parents[i].text === '' && i === 0) {
                this.selection.save(start, end);
                if (this.selection.getSelectedText(textArea).length === 0) {
                    parents[i].text = this.syntax[e.subCommand];
                    textArea.value = textArea.value.substr(0, parents[i].start) + this.syntax[e.subCommand] +
                        textArea.value.substr(parents[i].end, textArea.value.length);
                    start = i === 0 ? start + this.syntax[e.subCommand].length : start;
                    addedLength += this.syntax[e.subCommand].length;
                }
                if (parents.length !== 1) {
                    for (var j = i; j < parents.length; j++) {
                        parents[j].start = j !== 0 ? 1 + parents[j].start : parents[j].start;
                        parents[j].end = 1 + parents[j].end;
                    }
                }
            }
        }
        this.restore(textArea, start, end + addedLength, e);
    };
    MDFormats.prototype.clearRegex = function () {
        var regex = '';
        var configKey = Object.keys(this.syntax);
        for (var j = 0; j < configKey.length && configKey[j] !== 'pre' && configKey[j] !== 'p'; j++) {
            regex += regex === '' ? '^(' + this.selection.replaceSpecialChar(this.syntax[configKey[j]].trim()) + ')' :
                '|^(' + this.selection.replaceSpecialChar(this.syntax[configKey[j]].trim()) + ')';
        }
        return regex;
    };
    MDFormats.prototype.cleanFormat = function (textArea, command) {
        var parents = this.selection.getSelectedParentPoints(textArea);
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var removeLength = 0;
        if (this.selection.isClear(parents, this.clearRegex())) {
            for (var i = 0; i < parents.length; i++) {
                var configKey = Object.keys(this.syntax);
                for (var j = 0; parents[i].text !== '' && j < configKey.length; j++) {
                    var removeText = this.syntax[configKey[j]];
                    if (configKey[j] === command) {
                        continue;
                    }
                    var regex = new RegExp('^(' + this.selection.replaceSpecialChar(removeText) + ')', 'gim');
                    if (regex.test(parents[i].text)) {
                        parents[i].text = parents[i].text.replace(regex, '');
                        textArea.value = textArea.value.substr(0, parents[i].start) + parents[i].text + '\n' +
                            textArea.value.substr(parents[i].end, textArea.value.length);
                        start = i === 0 ? (start - (removeText.length)) > 0 ? start - (removeText.length) : 0 : start;
                        removeLength += removeText.length;
                        if (parents.length !== 1) {
                            for (var k = 0; k < parents.length; k++) {
                                parents[k].start = k !== 0 ?
                                    parents[k].start - removeText.length : parents[k].start;
                                parents[k].end = parents[k].end - removeText.length;
                            }
                        }
                        break;
                    }
                }
                if (parents[i].text === '' && i === 0) {
                    this.selection.save(start, end);
                    if (parents.length !== 1) {
                        for (var j = i; j < parents.length; j++) {
                            parents[j].start = j !== 0 ? 1 + parents[j].start : parents[j].start;
                            parents[j].end = 1 + parents[j].end;
                        }
                    }
                }
            }
            this.restore(textArea, start, end - removeLength);
        }
    };
    MDFormats.prototype.applyCodeBlock = function (textArea, event, parents) {
        var command = event.subCommand;
        var start = parents[0].start;
        var end = parents[parents.length - 1].end;
        var parentLines = this.selection.getAllParents(textArea.value);
        var firstPrevText = parentLines[parents[0].line - 1];
        var lastNextText = parentLines[(parents.length + 1) + 1];
        // eslint-disable-next-line
        if (!this.selection.isStartWith(firstPrevText, this.syntax.pre.split('\n')[0]) &&
            !this.selection.isStartWith(lastNextText, this.syntax.pre.split('\n')[0])) {
            var lines = textArea.value.substring(start, end).split('\n');
            var lastLine = lines[lines.length - 1] === '' ? '' : '\n';
            textArea.value = textArea.value.substr(0, start) + this.syntax[command] + textArea.value.substring(start, end) +
                lastLine + this.syntax[command] +
                textArea.value.substr(end, textArea.value.length);
            start = this.selection.selectionStart + this.syntax[command].length;
            end = this.selection.selectionEnd + this.syntax[command].length - 1;
        }
        else {
            var cmd = this.syntax[command];
            var selection = this.parent.markdownSelection.getSelectedInlinePoints(textArea);
            var startNo = textArea.value.substr(0, textArea.selectionStart).lastIndexOf(cmd);
            var endNo = textArea.value.substr(textArea.selectionEnd, textArea.selectionEnd).indexOf(cmd);
            endNo = endNo + selection.end;
            var repStartText = this.replaceAt(textArea.value.substr(0, selection.start), cmd, '', startNo, selection.start);
            var repEndText = this.replaceAt(textArea.value.substr(selection.end, textArea.value.length), cmd, '', 0, endNo);
            textArea.value = repStartText + selection.text + repEndText;
            start = this.selection.selectionStart - cmd.length;
            end = this.selection.selectionEnd - cmd.length;
        }
        this.restore(textArea, start, end, event);
    };
    MDFormats.prototype.replaceAt = function (input, search, replace, start, end) {
        return input.slice(0, start)
            + input.slice(start, end).replace(search, replace)
            + input.slice(end);
    };
    MDFormats.prototype.restore = function (textArea, start, end, event) {
        this.selection.save(start, end);
        this.selection.restore(textArea);
        if (event && event.callBack) {
            event.callBack({
                requestType: event.subCommand,
                selectedText: this.selection.getSelectedText(textArea),
                editorMode: 'Markdown',
                event: event.event
            });
        }
    };
    MDFormats.prototype.isAppliedFormat = function (lines, documentNode) {
        var format = 'p';
        // eslint-disable-next-line
        var configKey = Object.keys(this.syntax);
        var keys = Object.keys(this.syntax);
        var direction = this.parent.element.selectionDirection;
        var checkLine = direction === 'backward' ? lines[0].text : lines[lines.length - 1].text;
        for (var i = 0; !documentNode && i < keys.length; i++) {
            if (keys[i] !== 'pre' && this.selection.isStartWith(checkLine, this.syntax[keys[i]])) {
                format = keys[i];
                break;
            }
            else if (keys[i] === 'pre') {
                var parentLines = this.selection.getAllParents(this.parent.element.value);
                var firstPrevText = parentLines[lines[0].line - 1];
                var lastNextText = parentLines[lines.length + 1];
                if (this.selection.isStartWith(firstPrevText, this.syntax[keys[i]].split('\n')[0]) &&
                    this.selection.isStartWith(lastNextText, this.syntax[keys[i]].split('\n')[0])) {
                    format = keys[i];
                    break;
                }
            }
        }
        return format;
    };
    return MDFormats;
}());

/**
 * SelectionCommands internal component
 *
 * @hidden
 * @deprecated
 */
var MDSelectionFormats = /** @class */ (function () {
    function MDSelectionFormats(parent) {
        sf.base.extend(this, this, parent, true);
        this.selection = this.parent.markdownSelection;
        this.addEventListener();
    }
    MDSelectionFormats.prototype.addEventListener = function () {
        this.parent.observer.on(selectionCommand, this.applyCommands, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.keyDownHandler, this);
    };
    MDSelectionFormats.prototype.keyDownHandler = function (e) {
        switch (e.event.action) {
            case 'bold':
                this.applyCommands({ subCommand: 'Bold', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'italic':
                this.applyCommands({ subCommand: 'Italic', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'strikethrough':
                this.applyCommands({ subCommand: 'StrikeThrough', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'uppercase':
                this.applyCommands({ subCommand: 'UpperCase', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'lowercase':
                this.applyCommands({ subCommand: 'LowerCase', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'superscript':
                this.applyCommands({ subCommand: 'SuperScript', callBack: e.callBack });
                e.event.preventDefault();
                break;
            case 'subscript':
                this.applyCommands({ subCommand: 'SubScript', callBack: e.callBack });
                e.event.preventDefault();
                break;
        }
    };
    MDSelectionFormats.prototype.isBold = function (text, cmd) {
        return text.search('\\' + cmd + '\\' + cmd + '') !== -1;
    };
    MDSelectionFormats.prototype.isItalic = function (text, cmd) {
        return text.search('\\' + cmd) !== -1;
    };
    MDSelectionFormats.prototype.isMatch = function (text, cmd) {
        var matchText = [''];
        switch (cmd) {
            case this.syntax.Italic:
                matchText = text.match(this.singleCharRegx(cmd));
                break;
            case this.syntax.InlineCode:
                matchText = text.match(this.singleCharRegx(cmd));
                break;
            case this.syntax.StrikeThrough:
                matchText = text.match(this.singleCharRegx(cmd));
                break;
        }
        return matchText;
    };
    MDSelectionFormats.prototype.multiCharRegx = function (cmd) {
        return new RegExp('(\\' + cmd + '\\' + cmd + ')', 'g');
    };
    MDSelectionFormats.prototype.singleCharRegx = function (cmd) {
        return new RegExp('(\\' + cmd + ')', 'g');
    };
    MDSelectionFormats.prototype.isAppliedCommand = function (cmd) {
        // eslint-disable-next-line
        var isFormat = false;
        var textArea = this.parent.element;
        var start = textArea.selectionStart;
        var splitAt = function (index) { return function (x) { return [x.slice(0, index), x.slice(index)]; }; };
        var splitText = splitAt(start)(textArea.value);
        var cmdB = this.syntax.Bold.substr(0, 1);
        var cmdI = this.syntax.Italic;
        var selectedText = this.parent.markdownSelection.getSelectedText(textArea);
        if (selectedText !== '' && selectedText === selectedText.toLocaleUpperCase() && cmd === 'UpperCase') {
            return true;
        }
        else if (selectedText === '') {
            var beforeText = textArea.value.substr(splitText[0].length - 1, 1);
            var afterText = splitText[1].substr(0, 1);
            if ((beforeText !== '' && afterText !== '' && beforeText.match(/[a-z]/i)) &&
                beforeText === beforeText.toLocaleUpperCase() && afterText === afterText.toLocaleUpperCase() && cmd === 'UpperCase') {
                return true;
            }
        }
        if (!(this.isBold(splitText[0], cmdB)) && !(this.isItalic(splitText[0], cmdI)) && !(this.isBold(splitText[1], cmdB)) &&
            !(this.isItalic(splitText[1], cmdI))) {
            if ((!sf.base.isNullOrUndefined(this.isMatch(splitText[0], this.syntax.StrikeThrough)) &&
                !sf.base.isNullOrUndefined(this.isMatch(splitText[1], this.syntax.StrikeThrough))) &&
                (this.isMatch(splitText[0], this.syntax.StrikeThrough).length % 2 === 1 &&
                    this.isMatch(splitText[1], this.syntax.StrikeThrough).length % 2 === 1) && cmd === 'StrikeThrough') {
                isFormat = true;
            }
            if ((!sf.base.isNullOrUndefined(this.isMatch(splitText[0], this.syntax.InlineCode)) &&
                !sf.base.isNullOrUndefined(this.isMatch(splitText[1], this.syntax.InlineCode))) &&
                (this.isMatch(splitText[0], this.syntax.InlineCode).length % 2 === 1 &&
                    this.isMatch(splitText[1], this.syntax.InlineCode).length % 2 === 1) && cmd === 'InlineCode') {
                isFormat = true;
            }
            /* eslint-disable */
            if ((!sf.base.isNullOrUndefined(splitText[0].match(/\<sub>/g)) && !sf.base.isNullOrUndefined(splitText[1].match(/\<\/sub>/g))) &&
                (splitText[0].match(/\<sub>/g).length % 2 === 1 &&
                    splitText[1].match(/\<\/sub>/g).length % 2 === 1) && cmd === 'SubScript') {
                isFormat = true;
            }
            if ((!sf.base.isNullOrUndefined(splitText[0].match(/\<sup>/g)) && !sf.base.isNullOrUndefined(splitText[1].match(/\<\/sup>/g))) &&
                (splitText[0].match(/\<sup>/g).length % 2 === 1 && splitText[1].match(/\<\/sup>/g).length % 2 === 1) &&
                cmd === 'SuperScript') {
                isFormat = true;
            }
            /* eslint-enable */
        }
        if ((this.isBold(splitText[0], cmdB) && this.isBold(splitText[1], cmdB)) &&
            (splitText[0].match(this.multiCharRegx(cmdB)).length % 2 === 1 &&
                splitText[1].match(this.multiCharRegx(cmdB)).length % 2 === 1) && cmd === 'Bold') {
            isFormat = true;
        }
        splitText[0] = this.isBold(splitText[0], cmdB) ? splitText[0].replace(this.multiCharRegx(cmdB), '$%@') : splitText[0];
        splitText[1] = this.isBold(splitText[1], cmdB) ? splitText[1].replace(this.multiCharRegx(cmdB), '$%@') : splitText[1];
        if ((!sf.base.isNullOrUndefined(this.isMatch(splitText[0], this.syntax.Italic)) &&
            !sf.base.isNullOrUndefined(this.isMatch(splitText[1], this.syntax.Italic))) &&
            (this.isMatch(splitText[0], this.syntax.Italic).length % 2 === 1 &&
                this.isMatch(splitText[1], this.syntax.Italic).length % 2 === 1) && cmd === 'Italic') {
            isFormat = true;
        }
        if ((!sf.base.isNullOrUndefined(this.isMatch(splitText[0], this.syntax.StrikeThrough)) &&
            !sf.base.isNullOrUndefined(this.isMatch(splitText[1], this.syntax.StrikeThrough))) &&
            (this.isMatch(splitText[0], this.syntax.StrikeThrough).length % 2 === 1 &&
                this.isMatch(splitText[1], this.syntax.StrikeThrough).length % 2 === 1) && cmd === 'StrikeThrough') {
            isFormat = true;
        }
        if ((!sf.base.isNullOrUndefined(this.isMatch(splitText[0], this.syntax.InlineCode)) &&
            !sf.base.isNullOrUndefined(this.isMatch(splitText[1], this.syntax.InlineCode))) &&
            (this.isMatch(splitText[0], this.syntax.InlineCode).length % 2 === 1 &&
                this.isMatch(splitText[1], this.syntax.InlineCode).length % 2 === 1) && cmd === 'InlineCode') {
            isFormat = true;
        }
        /* eslint-disable */
        if ((!sf.base.isNullOrUndefined(splitText[0].match(/\<sub>/g)) && !sf.base.isNullOrUndefined(splitText[1].match(/\<\/sub>/g))) &&
            (splitText[0].match(/\<sub>/g).length % 2 === 1 && splitText[1].match(/\<\/sub>/g).length % 2 === 1) && cmd === 'SubScript') {
            isFormat = true;
        }
        if ((!sf.base.isNullOrUndefined(splitText[0].match(/\<sup>/g)) && !sf.base.isNullOrUndefined(splitText[1].match(/\<\/sup>/g))) &&
            (splitText[0].match(/\<sup>/g).length % 2 === 1 && splitText[1].match(/\<\/sup>/g).length % 2 === 1) && cmd === 'SuperScript') {
            isFormat = true;
            /* eslint-enable */
        }
        return isFormat;
    };
    MDSelectionFormats.prototype.applyCommands = function (e) {
        this.currentAction = e.subCommand;
        var textArea = this.parent.element;
        this.selection.save(textArea.selectionStart, textArea.selectionEnd);
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var addedLength = 0;
        var selection = this.parent.markdownSelection.getSelectedInlinePoints(textArea);
        if (this.isAppliedCommand(e.subCommand) && selection.text !== '') {
            var startCmd = this.syntax[e.subCommand];
            var endCmd = e.subCommand === 'SubScript' ? '</sub>' :
                e.subCommand === 'SuperScript' ? '</sup>' : this.syntax[e.subCommand];
            var startLength = (e.subCommand === 'UpperCase' || e.subCommand === 'LowerCase') ? 0 : startCmd.length;
            var startNo = textArea.value.substr(0, selection.start).lastIndexOf(startCmd);
            var endNo = textArea.value.substr(selection.end, textArea.value.length).indexOf(endCmd);
            endNo = endNo + selection.end;
            var repStartText = this.replaceAt(textArea.value.substr(0, selection.start), startCmd, '', startNo, selection.start);
            var repEndText = this.replaceAt(textArea.value.substr(selection.end, textArea.value.length), endCmd, '', 0, endNo);
            textArea.value = repStartText + selection.text + repEndText;
            this.restore(textArea, start - startLength, end - startLength, e);
            return;
        }
        if (selection.text !== '' && !this.isApplied(selection, e.subCommand)) {
            addedLength = (e.subCommand === 'UpperCase' || e.subCommand === 'LowerCase') ? 0 :
                this.syntax[e.subCommand].length;
            var repStart = textArea.value.substr(selection.start - this.syntax[e.subCommand].length, this.syntax[e.subCommand].length);
            var repEnd = void 0;
            if ((repStart === e.subCommand) || ((selection.start - this.syntax[e.subCommand].length ===
                textArea.value.indexOf(this.syntax[e.subCommand])) && (selection.end === textArea.value.lastIndexOf(this.syntax[e.subCommand]) || selection.end === textArea.value.lastIndexOf('</' + this.syntax[e.subCommand].substring(1, 5))))) {
                if (e.subCommand === 'SubScript' || e.subCommand === 'SuperScript') {
                    repEnd = textArea.value.substr(selection.end, this.syntax[e.subCommand].length + 1);
                }
                else {
                    repEnd = textArea.value.substr(selection.end, this.syntax[e.subCommand].length);
                }
                var repStartText = this.replaceAt(textArea.value.substr(0, selection.start), repStart, '', selection.start - this.syntax[e.subCommand].length, selection.start);
                var repEndText = this.replaceAt(textArea.value.substr(selection.end, textArea.value.length), repEnd, '', 0, repEnd.length);
                textArea.value = repStartText + selection.text + repEndText;
                this.restore(textArea, start - addedLength, end - addedLength, e);
            }
            else {
                if (e.subCommand === 'SubScript' || e.subCommand === 'SuperScript') {
                    selection.text = this.syntax[e.subCommand] + selection.text
                        + '</' + this.syntax[e.subCommand].substring(1, 5);
                }
                else if (e.subCommand === 'UpperCase' || e.subCommand === 'LowerCase') {
                    selection.text = (e.subCommand === 'UpperCase') ? selection.text.toUpperCase()
                        : selection.text.toLowerCase();
                }
                else {
                    selection.text = this.syntax[e.subCommand] + selection.text + this.syntax[e.subCommand];
                }
                textArea.value = textArea.value.substr(0, selection.start) + selection.text +
                    textArea.value.substr(selection.end, textArea.value.length);
                this.restore(textArea, start + addedLength, end + addedLength, e);
            }
        }
        else if (e.subCommand !== 'UpperCase' && e.subCommand !== 'LowerCase') {
            if (e.subCommand === 'SubScript' || e.subCommand === 'SuperScript') {
                selection.text = this.textReplace(selection.text, e.subCommand);
                selection.text = this.syntax[e.subCommand] + selection.text
                    + '</' + this.syntax[e.subCommand].substring(1, 5);
            }
            else {
                selection.text = this.textReplace(selection.text, e.subCommand);
                selection.text = this.syntax[e.subCommand] + selection.text + this.syntax[e.subCommand];
            }
            textArea.value = textArea.value.substr(0, selection.start)
                + selection.text + textArea.value.substr(selection.end, textArea.value.length);
            addedLength = this.syntax[e.subCommand].length;
            if (selection.start === selection.end) {
                this.restore(textArea, start + addedLength, end + addedLength, e);
            }
            else {
                this.restore(textArea, start + addedLength, end - addedLength, e);
            }
        }
        else {
            this.restore(textArea, start, end, e);
        }
        this.parent.undoRedoManager.saveData();
    };
    MDSelectionFormats.prototype.replaceAt = function (input, search, replace, start, end) {
        return input.slice(0, start)
            + input.slice(start, end).replace(search, replace)
            + input.slice(end);
    };
    MDSelectionFormats.prototype.restore = function (textArea, start, end, event) {
        this.selection.save(start, end);
        this.selection.restore(textArea);
        if (event && event.callBack) {
            event.callBack({
                requestType: this.currentAction,
                selectedText: this.selection.getSelectedText(textArea),
                editorMode: 'Markdown',
                event: event.event
            });
        }
    };
    MDSelectionFormats.prototype.textReplace = function (text, command) {
        var regx = this.singleCharRegx(this.syntax[command]);
        switch (command) {
            case 'Bold':
                regx = this.multiCharRegx(this.syntax[command].substr(0, 1));
                text = text.replace(regx, '');
                break;
            case 'Italic':
                if (!this.isBold(text, this.syntax[command].substr(0, 1))) {
                    text = text.replace(regx, '');
                }
                else {
                    var regxB = this.multiCharRegx(this.syntax[command].substr(0, 1));
                    var repText = text;
                    repText = repText.replace(regxB, '$%@').replace(regx, '');
                    var regxTemp = new RegExp('\\$%@', 'g');
                    text = repText.replace(regxTemp, this.syntax[command].substr(0, 1) + this.syntax[command].substr(0, 1));
                }
                break;
            case 'StrikeThrough':
                text = text.replace(regx, '');
                break;
            case 'InlineCode':
                text = text.replace(regx, '');
                break;
            case 'SubScript':
                text = text.replace(/<sub>/g, '').replace(/<\/sub>/g, '');
                break;
            case 'SuperScript':
                text = text.replace(/<sup>/g, '').replace(/<\/sup>/g, '');
                break;
        }
        return text;
    };
    MDSelectionFormats.prototype.isApplied = function (line, command) {
        var regx = this.singleCharRegx(this.syntax[command]);
        switch (command) {
            case 'SubScript':
            case 'SuperScript':
                regx = this.singleCharRegx(this.syntax[command]);
                return regx.test(line.text);
            case 'Bold':
            case 'StrikeThrough':
                regx = this.multiCharRegx(this.syntax[command].substr(0, 1));
                return regx.test(line.text);
            case 'UpperCase':
            case 'LowerCase':
                regx = new RegExp('^[' + this.syntax[command] + ']*$', 'g');
                return regx.test(line.text);
            case 'Italic': {
                var regTest = void 0;
                var regxB = this.multiCharRegx(this.syntax[command].substr(0, 1));
                if (regxB.test(line.text)) {
                    var repText = line.text;
                    repText = repText.replace(regxB, '$%#');
                    regTest = regx.test(repText);
                }
                else {
                    regTest = regx.test(line.text);
                }
                return regTest;
            }
            case 'InlineCode':
                return regx.test(line.text);
        }
    };
    return MDSelectionFormats;
}());

/**
 * `Undo` module is used to handle undo actions.
 */
var UndoRedoCommands = /** @class */ (function () {
    function UndoRedoCommands(parent, options) {
        this.undoRedoStack = [];
        this.parent = parent;
        this.undoRedoSteps = !sf.base.isNullOrUndefined(options) ? options.undoRedoSteps : 30;
        this.undoRedoTimer = !sf.base.isNullOrUndefined(options) ? options.undoRedoTimer : 300;
        this.selection = this.parent.markdownSelection;
        this.addEventListener();
    }
    UndoRedoCommands.prototype.addEventListener = function () {
        var debounceListener = sf.base.debounce(this.keyUp, this.undoRedoTimer);
        this.parent.observer.on(KEY_UP_HANDLER, debounceListener, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.keyDown, this);
        this.parent.observer.on(ACTION, this.onAction, this);
        this.parent.observer.on(MODEL_CHANGED_PLUGIN, this.onPropertyChanged, this);
    };
    UndoRedoCommands.prototype.onPropertyChanged = function (props) {
        for (var _i = 0, _a = Object.keys(props.newProp); _i < _a.length; _i++) {
            var prop = _a[_i];
            switch (prop) {
                case 'undoRedoSteps':
                    this.undoRedoSteps = props.newProp.undoRedoSteps;
                    break;
                case 'undoRedoTimer':
                    this.undoRedoTimer = props.newProp.undoRedoTimer;
                    break;
            }
        }
    };
    UndoRedoCommands.prototype.removeEventListener = function () {
        var debounceListener = sf.base.debounce(this.keyUp, 300);
        this.parent.observer.off(KEY_UP_HANDLER, debounceListener);
        this.parent.observer.off(KEY_DOWN_HANDLER, this.keyDown);
        this.parent.observer.off(ACTION, this.onAction);
        this.parent.observer.off(MODEL_CHANGED_PLUGIN, this.onPropertyChanged);
    };
    /**
     * Destroys the ToolBar.
     *
     * @function destroy
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoCommands.prototype.destroy = function () {
        this.removeEventListener();
    };
    /**
     * onAction method
     *
     * @param {IMarkdownSubCommands} e - specifies the sub commands
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoCommands.prototype.onAction = function (e) {
        if (e.subCommand === 'Undo') {
            this.undo(e);
        }
        else {
            this.redo(e);
        }
    };
    UndoRedoCommands.prototype.keyDown = function (e) {
        var event = e.event;
        // eslint-disable-next-line
        var proxy = this;
        switch (event.action) {
            case 'undo':
                event.preventDefault();
                proxy.undo(e);
                break;
            case 'redo':
                event.preventDefault();
                proxy.redo(e);
                break;
        }
    };
    UndoRedoCommands.prototype.keyUp = function (e) {
        if (e.event.keyCode !== 17 && !e.event.ctrlKey) {
            this.saveData(e);
        }
    };
    /**
     * MD collection stored string format.
     *
     * @param {KeyboardEvent} e - specifies the key board event
     * @function saveData
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoCommands.prototype.saveData = function (e) {
        var textArea = this.parent.element;
        this.selection.save(textArea.selectionStart, textArea.selectionEnd);
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var textValue = this.parent.element.value;
        var changEle = { text: textValue, start: start, end: end };
        if (this.undoRedoStack.length >= this.steps) {
            this.undoRedoStack = this.undoRedoStack.slice(0, this.steps + 1);
        }
        if (this.undoRedoStack.length > 1 && (this.undoRedoStack[this.undoRedoStack.length - 1].start === start) &&
            (this.undoRedoStack[this.undoRedoStack.length - 1].end === end)) {
            return;
        }
        this.undoRedoStack.push(changEle);
        this.steps = this.undoRedoStack.length - 1;
        if (this.steps > this.undoRedoSteps) {
            this.undoRedoStack.shift();
            this.steps--;
        }
        if (e && e.callBack) {
            e.callBack();
        }
    };
    /**
     * Undo the editable text.
     *
     * @param {IMarkdownSubCommands} e - specifies the sub commands
     * @function undo
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoCommands.prototype.undo = function (e) {
        if (this.steps > 0) {
            this.currentAction = 'Undo';
            var start = this.undoRedoStack[this.steps - 1].start;
            var end = this.undoRedoStack[this.steps - 1].end;
            var removedContent = this.undoRedoStack[this.steps - 1].text;
            this.parent.element.value = removedContent;
            this.parent.element.focus();
            this.steps--;
            this.restore(this.parent.element, start, end, e);
        }
    };
    /**
     * Redo the editable text.
     *
     * @param {IMarkdownSubCommands} e - specifies the sub commands
     * @function redo
     * @returns {void}
     * @hidden
     * @deprecated
     */
    UndoRedoCommands.prototype.redo = function (e) {
        if (this.undoRedoStack[this.steps + 1] != null) {
            this.currentAction = 'Redo';
            var start = this.undoRedoStack[this.steps + 1].start;
            var end = this.undoRedoStack[this.steps + 1].end;
            this.parent.element.value = this.undoRedoStack[this.steps + 1].text;
            this.parent.element.focus();
            this.steps++;
            this.restore(this.parent.element, start, end, e);
        }
    };
    UndoRedoCommands.prototype.restore = function (textArea, start, end, event) {
        this.selection.save(start, end);
        this.selection.restore(textArea);
        if (event && event.callBack) {
            event.callBack({
                requestType: this.currentAction,
                selectedText: this.selection.getSelectedText(textArea),
                editorMode: 'Markdown',
                event: event.event
            });
        }
    };
    /**
     * getUndoStatus method
     *
     * @returns {boolean} - returns the boolean value
     * @hidden
     * @deprecated
     */
    UndoRedoCommands.prototype.getUndoStatus = function () {
        var status = { undo: false, redo: false };
        if (this.steps > 0) {
            status.undo = true;
        }
        if (this.undoRedoStack[this.steps + 1] != null) {
            status.redo = true;
        }
        return status;
    };
    return UndoRedoCommands;
}());

/**
 * Link internal component
 *
 * @hidden
 * @deprecated
 */
var MDLink = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {MarkdownParser} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function MDLink(parent) {
        this.parent = parent;
        this.selection = this.parent.markdownSelection;
        this.addEventListener();
    }
    MDLink.prototype.addEventListener = function () {
        this.parent.observer.on(LINK_COMMAND, this.createLink, this);
    };
    MDLink.prototype.createLink = function (e) {
        var textArea = this.parent.element;
        textArea.focus();
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var text = (e.subCommand === 'Image') ? this.selection.getSelectedText(textArea) : e.item.text;
        var startOffset = (e.subCommand === 'Image') ? (start + 2) : (start + 1);
        var endOffset = (e.subCommand === 'Image') ? (end + 2) : (end + 1);
        text = (e.subCommand === 'Image') ? '![' + text + '](' + e.item.url + ')' : '[' + text + '](' + e.item.url + ')';
        textArea.value = textArea.value.substr(0, start)
            + text + textArea.value.substr(end, textArea.value.length);
        this.parent.markdownSelection.setSelection(textArea, startOffset, endOffset);
        this.restore(textArea, startOffset, endOffset, e);
    };
    MDLink.prototype.restore = function (textArea, start, end, event) {
        this.selection.save(start, end);
        this.selection.restore(textArea);
        if (event && event.callBack) {
            event.callBack({
                requestType: event.subCommand,
                selectedText: this.selection.getSelectedText(textArea),
                editorMode: 'Markdown',
                event: event.event
            });
        }
    };
    return MDLink;
}());

/**
 * Link internal component
 *
 * @hidden
 * @deprecated
 */
var MDTable = /** @class */ (function () {
    /**
     * Constructor for creating the Formats plugin
     *
     * @param {IMDTable} options - specifies the options
     * @hidden
     * @deprecated
     */
    function MDTable(options) {
        sf.base.extend(this, this, options, true);
        this.selection = this.parent.markdownSelection;
        this.addEventListener();
    }
    MDTable.prototype.addEventListener = function () {
        this.parent.observer.on(MD_TABLE, this.createTable, this);
        this.parent.observer.on(KEY_DOWN_HANDLER, this.onKeyDown, this);
    };
    MDTable.prototype.removeEventListener = function () {
        this.parent.observer.off(MD_TABLE, this.createTable);
        this.parent.observer.off(KEY_DOWN_HANDLER, this.onKeyDown);
    };
    /**
     * markdown destroy method
     *
     * @returns {void}
     * @hidden
     * @deprecated
     */
    MDTable.prototype.destroy = function () {
        this.removeEventListener();
    };
    MDTable.prototype.onKeyDown = function (e) {
        if (e.event.action === 'insert-table') {
            e.item = e.value;
            this.createTable(e);
        }
    };
    MDTable.prototype.createTable = function (e) {
        this.element = this.parent.element;
        var start = this.element.selectionStart;
        var end = this.element.selectionEnd;
        var textAreaInitial = this.element.value;
        this.locale = e;
        this.selection.save(start, end);
        this.restore(this.element.selectionStart, this.element.selectionEnd, null);
        this.insertTable(start, end, textAreaInitial, e);
    };
    MDTable.prototype.getTable = function () {
        var table = '';
        table += this.textNonEmpty();
        table += this.tableHeader(this.locale);
        table += this.tableCell(this.locale);
        return table;
    };
    MDTable.prototype.tableHeader = function (e) {
        var text = '';
        for (var i = 1; i <= 2; i++) {
            text += '|';
            for (var j = 1; j <= 2; j++) {
                if (i === 1) {
                    text += e.item.headingText + ' ' + j + '|';
                }
                else {
                    text += '---------|';
                }
            }
            text += this.insertLine();
        }
        return text;
    };
    MDTable.prototype.tableCell = function (e) {
        var text = '';
        for (var i = 1; i <= 2; i++) {
            text += '|';
            for (var j = 1; j <= 2; j++) {
                text += e.item.colText + ' ' + this.convertToLetters(i) + j + '|';
            }
            text += this.insertLine();
        }
        text += this.insertLine();
        return text;
    };
    MDTable.prototype.insertLine = function () {
        var dummyElement = document.createElement('div');
        dummyElement.innerHTML = '\n';
        return dummyElement.textContent;
    };
    MDTable.prototype.insertTable = function (start, end, textAreaInitial, e) {
        var parentText = this.selection.getSelectedParentPoints(this.element);
        var lastLineSplit = parentText[parentText.length - 1].text.split(' ', 2);
        var syntaxArr = this.getFormatTag();
        // eslint-disable-next-line
        if (lastLineSplit.length < 2) {
            this.element.value = this.updateValue(this.getTable());
            this.makeSelection(textAreaInitial, start, end);
        }
        else {
            if (this.ensureFormatApply(parentText[parentText.length - 1].text)) {
                this.checkValid(start, end, this.getTable(), textAreaInitial, e, lastLineSplit, parentText, syntaxArr);
            }
            else {
                this.element.value = this.updateValue(this.getTable());
                this.makeSelection(textAreaInitial, start, end);
            }
        }
        this.restore(this.element.selectionStart, this.element.selectionEnd, e);
    };
    MDTable.prototype.makeSelection = function (textAreaInitial, start, end) {
        end = start + (textAreaInitial.length > 0 ? 12 : 10); //end is added 12 or 10 because to make the table heading selected
        start += textAreaInitial.length > 0 ? 3 : 1; // Start is added 3 or 1 because new lines are added when inserting table
        this.selection.setSelection(this.element, start, end);
    };
    MDTable.prototype.getFormatTag = function () {
        var syntaxFormatKey = Object.keys(this.syntaxTag.Formats);
        var syntaxListKey = Object.keys(this.syntaxTag.List);
        var syntaxArr = [];
        for (var i = 0; i < syntaxFormatKey.length; i++) {
            syntaxArr.push(this.syntaxTag.Formats[syntaxFormatKey[i]]);
        }
        for (var j = 0; j < syntaxListKey.length; j++) {
            syntaxArr.push(this.syntaxTag.List[syntaxListKey[j]]);
        }
        return syntaxArr;
    };
    MDTable.prototype.ensureFormatApply = function (line) {
        var formatTags = this.getFormatTag();
        var formatSplitZero = line.trim().split(' ', 2)[0] + ' ';
        for (var i = 0; i < formatTags.length; i++) {
            if (formatSplitZero === formatTags[i] || /^[\d.]+[ ]+$/.test(formatSplitZero)) {
                return true;
            }
        }
        return false;
    };
    MDTable.prototype.ensureStartValid = function (firstLine, parentText) {
        var firstLineSplit = parentText[0].text.split(' ', 2);
        for (var i = firstLine + 1; i <= firstLine + firstLineSplit[0].length + 1; i++) {
            if (this.element.selectionStart === i || this.element.selectionEnd === i) {
                return false;
            }
        }
        return true;
    };
    MDTable.prototype.ensureEndValid = function (lastLine, formatSplitLength) {
        for (var i = lastLine + 1; i <= lastLine + formatSplitLength + 1; i++) {
            if (this.element.selectionEnd === i) {
                return false;
            }
        }
        return true;
    };
    MDTable.prototype.updateValueWithFormat = function (formatSplit, text) {
        var textApplyFormat = this.element.value.substring(this.element.selectionEnd, this.element.value.length);
        text += textApplyFormat.replace(textApplyFormat, (formatSplit[0] + ' ' + textApplyFormat));
        return this.element.value.substr(0, this.element.selectionStart) + text;
    };
    MDTable.prototype.updateValue = function (text) {
        return this.element.value.substr(0, this.element.selectionStart) + text +
            this.element.value.substr(this.element.selectionEnd, this.element.value.length);
    };
    MDTable.prototype.checkValid = function (start, end, text, textAreaInitial, 
    // eslint-disable-next-line
    e, formatSplit, parentText, syntaxArr) {
        if (this.ensureStartValid(parentText[0].start, parentText) &&
            this.ensureEndValid(parentText[parentText.length - 1].start, formatSplit[0].length)) {
            if (start === parentText[0].start) {
                if (start !== end && end !== (parentText[parentText.length - 1].end - 1)) {
                    this.element.value = this.updateValueWithFormat(formatSplit, text);
                }
                else {
                    this.element.value = this.updateValue(text);
                }
            }
            else if (end === parentText[parentText.length - 1].end - 1) {
                this.element.value = this.updateValue(text);
            }
            else {
                this.element.value = this.updateValueWithFormat(formatSplit, text);
            }
            this.makeSelection(textAreaInitial, start, end);
        }
    };
    MDTable.prototype.convertToLetters = function (rowNumber) {
        var baseChar = ('A').charCodeAt(0);
        var letters = '';
        do {
            rowNumber -= 1;
            letters = String.fromCharCode(baseChar + (rowNumber % 26)) + letters;
            rowNumber = (rowNumber / 26) >> 0;
        } while (rowNumber > 0);
        return letters;
    };
    MDTable.prototype.textNonEmpty = function () {
        var emptyText = '';
        if (this.isCursorBased() || this.isSelectionBased()) {
            if (this.element.value.length > 0) {
                emptyText += this.insertLine();
                emptyText += this.insertLine(); // to append two new line when textarea having content.
            }
        }
        return emptyText;
    };
    MDTable.prototype.isCursorBased = function () {
        return this.element.selectionStart === this.element.selectionEnd;
    };
    MDTable.prototype.isSelectionBased = function () {
        return this.element.selectionStart !== this.element.selectionEnd;
    };
    MDTable.prototype.restore = function (start, end, event) {
        this.selection.save(start, end);
        this.selection.restore(this.element);
        if (event && event.callBack) {
            event.callBack({
                requestType: event.subCommand,
                selectedText: this.selection.getSelectedText(this.element),
                editorMode: 'Markdown',
                event: event.event
            });
        }
    };
    return MDTable;
}());

/**
 * Link internal component
 *
 * @hidden
 * @deprecated
 */
var ClearFormat$1 = /** @class */ (function () {
    /**
     * Constructor for creating the clear format plugin
     *
     * @param {MarkdownParser} parent - specifies the parent element
     * @hidden
     * @deprecated
     */
    function ClearFormat(parent) {
        this.parent = parent;
        this.selection = this.parent.markdownSelection;
        this.addEventListener();
    }
    ClearFormat.prototype.addEventListener = function () {
        this.parent.observer.on(CLEAR_COMMAND, this.clear, this);
    };
    ClearFormat.prototype.replaceRegex = function (data) {
        /* eslint-disable */
        return data.replace(/\*/ig, '\\*').replace(/\&/ig, '\\&')
            .replace(/\-/ig, '\\-').replace(/\^/ig, '\\^')
            .replace(/\$/ig, '\\$').replace(/\./ig, '\\.')
            .replace(/\|/ig, '\\|').replace(/\?/ig, '\\?')
            .replace(/\+/ig, '\\+').replace(/\-/ig, '\\-')
            .replace(/\&/ig, '\\&');
        /* eslint-enable */
    };
    ClearFormat.prototype.clearSelectionTags = function (text) {
        var data = this.parent.selectionTags;
        var keys = Object.keys(data);
        for (var num = 0; num < keys.length; num++) {
            var key = keys[num];
            // eslint-disable-next-line
            if (data.hasOwnProperty(key) && data[key] !== '') {
                var expString = this.replaceRegex(data[key]);
                var regExp = void 0;
                var startExp = data[key].length;
                var endExp = (data[key] === '<sup>' || data[key] === '<sub>') ? data[key].length + 1 : data[key].length;
                if (data[key] === '<sup>') {
                    // eslint-disable-next-line
                    regExp = new RegExp('<sup>(.*?)<\/sup>', 'ig');
                }
                else if (data[key] === '<sub>') {
                    // eslint-disable-next-line
                    regExp = new RegExp('<sub>(.*?)<\/sub>', 'ig');
                }
                else {
                    regExp = new RegExp(expString + '(.*?)' + expString, 'ig');
                }
                var val = text.match(regExp);
                for (var index = 0; val && index < val.length && val[index] !== ''; index++) {
                    text = text.replace(val[index], val[index].substr(startExp, val[index].length - endExp - startExp));
                }
            }
        }
        return text;
    };
    ClearFormat.prototype.clearFormatTags = function (text) {
        var lines = text.split('\n');
        return this.clearFormatLines(lines);
    };
    ClearFormat.prototype.clearFormatLines = function (lines) {
        var tags = [this.parent.formatTags, this.parent.listTags];
        var str = '';
        for (var len = 0; len < lines.length; len++) {
            for (var num = 0; num < tags.length; num++) {
                var data = tags[num];
                var keys = Object.keys(data);
                for (var index = 0; index < keys.length; index++) {
                    var key = keys[index];
                    // eslint-disable-next-line
                    if (data.hasOwnProperty(key) && data[key] !== '') {
                        if (lines[len].indexOf(data[key]) === 0) {
                            lines[len] = lines[len].replace(data[key], '');
                            lines[len] = this.clearFormatLines([lines[len]]);
                        }
                    }
                }
            }
            str = str + lines[len] + ((len !== lines.length - 1) ? '\n' : '');
        }
        return str;
    };
    ClearFormat.prototype.clear = function (e) {
        var textArea = this.parent.element;
        textArea.focus();
        var start = textArea.selectionStart;
        var end = textArea.selectionEnd;
        var text = this.selection.getSelectedText(textArea);
        text = this.clearSelectionTags(text);
        text = this.clearFormatTags(text);
        textArea.value = textArea.value.substr(0, start)
            + text + textArea.value.substr(end, textArea.value.length);
        this.parent.markdownSelection.setSelection(textArea, start, start + text.length);
        this.restore(textArea, start, start + text.length, e);
    };
    ClearFormat.prototype.restore = function (textArea, start, end, event) {
        this.selection.save(start, end);
        this.selection.restore(textArea);
        if (event && event.callBack) {
            event.callBack({
                requestType: event.subCommand,
                selectedText: this.selection.getSelectedText(textArea),
                editorMode: 'Markdown',
                event: event.event
            });
        }
    };
    return ClearFormat;
}());

/**
 * MarkdownParser internal component
 *
 * @hidden
 * @deprecated
 */
var MarkdownParser = /** @class */ (function () {
    /**
     * Constructor for creating the component
     *
     * @param {IMarkdownParserModel} options - specifies the options
     * @hidden
     * @deprecated
     */
    function MarkdownParser(options) {
        this.initialize();
        sf.base.extend(this, this, options, true);
        this.observer = new sf.base.Observer(this);
        this.markdownSelection = new MarkdownSelection();
        this.listObj = new MDLists({ parent: this, syntax: this.listTags });
        this.formatObj = new MDFormats({ parent: this, syntax: this.formatTags });
        this.undoRedoManager = new UndoRedoCommands(this, options.options);
        this.mdSelectionFormats = new MDSelectionFormats({ parent: this, syntax: this.selectionTags });
        this.linkObj = new MDLink(this);
        this.tableObj = new MDTable({ parent: this, syntaxTag: ({ Formats: this.formatTags, List: this.listTags }) });
        this.clearObj = new ClearFormat$1(this);
        this.wireEvents();
    }
    MarkdownParser.prototype.initialize = function () {
        this.formatTags = markdownFormatTags;
        this.listTags = markdownListsTags;
        this.selectionTags = markdownSelectionTags;
    };
    MarkdownParser.prototype.wireEvents = function () {
        this.observer.on(KEY_DOWN, this.editorKeyDown, this);
        this.observer.on(KEY_UP, this.editorKeyUp, this);
        this.observer.on(MODEL_CHANGED, this.onPropertyChanged, this);
    };
    MarkdownParser.prototype.onPropertyChanged = function (props) {
        this.observer.notify(MODEL_CHANGED_PLUGIN, props);
    };
    MarkdownParser.prototype.editorKeyDown = function (e) {
        this.observer.notify(KEY_DOWN_HANDLER, e);
    };
    MarkdownParser.prototype.editorKeyUp = function (e) {
        this.observer.notify(KEY_UP_HANDLER, e);
    };
    /* eslint-disable */
    /**
     * markdown execCommand method
     *
     * @param {MarkdownExecCommand} command - specifies the command
     * @param {T} - specifies the value
     * @param {Event} event - specifies the event
     * @param {Function} callBack - specifies the call back function
     * @param {string} text - specifies the string value
     * @param {T} exeValue - specifies the value
     * @returns {void}
     * @hidden
     * @deprecated
     */
    /* eslint-enable */
    MarkdownParser.prototype.execCommand = function (command, value, event, callBack, text, exeValue) {
        switch (command.toLocaleLowerCase()) {
            case 'lists':
                this.observer.notify(LISTS_COMMAND, { subCommand: value, event: event, callBack: callBack });
                break;
            case 'formats':
                this.observer.notify(FORMAT_TYPE, { subCommand: value, event: event, callBack: callBack });
                break;
            case 'actions':
                this.observer.notify(ACTION, { subCommand: value, event: event, callBack: callBack });
                break;
            case 'style':
            case 'effects':
            case 'casing':
                this.observer.notify(selectionCommand, { subCommand: value, event: event, callBack: callBack });
                break;
            case 'links':
            case 'images':
                this.observer.notify(LINK_COMMAND, { subCommand: value, event: event, callBack: callBack, item: exeValue });
                break;
            case 'table':
                switch (value.toString().toLocaleLowerCase()) {
                    case 'createtable':
                        this.observer.notify(MD_TABLE, { subCommand: value, item: exeValue, event: event, callBack: callBack });
                        break;
                }
                break;
            case 'clear':
                this.observer.notify(CLEAR_COMMAND, { subCommand: value, event: event, callBack: callBack });
                break;
        }
    };
    return MarkdownParser;
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
/**
 * Markdown adapter
 */
var MarkdownFormatter = /** @class */ (function (_super) {
    __extends$1(MarkdownFormatter, _super);
    function MarkdownFormatter(options) {
        var _this = _super.call(this) || this;
        _this.initialize();
        sf.base.extend(_this, _this, options, true);
        if (options && _this.element) {
            _this.updateFormatter(_this.element, document, options.options);
        }
        return _this;
    }
    MarkdownFormatter.prototype.initialize = function () {
        this.keyConfig = markdownKeyConfig;
        this.formatSyntax = markdownFormatTags;
        this.listSyntax = markdownListsTags;
        this.selectionSyntax = markdownSelectionTags;
    };
    MarkdownFormatter.prototype.updateFormatter = function (editElement, doc, options) {
        if (editElement) {
            this.editorManager = new MarkdownParser({
                element: editElement,
                formatTags: this.formatSyntax,
                listTags: this.listSyntax,
                selectionTags: this.selectionSyntax,
                options: options
            });
        }
    };
    return MarkdownFormatter;
}(Formatter));

/**
 * `MarkdownEditor` module is used to markdown editor
 */
var MarkdownEditor = /** @class */ (function () {
    function MarkdownEditor(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    MarkdownEditor.prototype.addEventListener = function () {
        this.saveSelection = new MarkdownSelection();
        this.parent.observer.on(initialEnd, this.render, this);
        this.parent.observer.on(markdownToolbarClick, this.onToolbarClick, this);
        this.parent.observer.on(destroy, this.destroy, this);
        this.parent.observer.on(selectAll$1, this.selectAll, this);
        this.parent.observer.on(getSelectedHtml, this.getSelectedHtml, this);
        this.parent.observer.on(selectionSave, this.onSelectionSave, this);
        this.parent.observer.on(selectionRestore, this.onSelectionRestore, this);
        this.parent.observer.on(readOnlyMode, this.updateReadOnly, this);
    };
    MarkdownEditor.prototype.updateReadOnly = function () {
        if (this.parent.readonly) {
            this.parent.getEditPanel().setAttribute('readonly', 'readonly');
            sf.base.addClass([this.parent.element], CLS_RTE_READONLY);
        }
        else {
            this.parent.getEditPanel().removeAttribute('readonly');
            sf.base.removeClass([this.parent.element], CLS_RTE_READONLY);
        }
    };
    MarkdownEditor.prototype.onSelectionSave = function () {
        var textArea = this.parent.getEditPanel();
        this.saveSelection.save(textArea.selectionStart, textArea.selectionEnd);
    };
    MarkdownEditor.prototype.onSelectionRestore = function (e) {
        this.parent.getEditPanel().focus();
        var textArea = this.parent.getEditPanel();
        this.saveSelection.restore(textArea);
    };
    MarkdownEditor.prototype.onToolbarClick = function (args) {
        var text;
        var startOffset;
        var endOffset;
        var item = args.item;
        var textArea = this.parent.getEditPanel();
        textArea.focus();
        startOffset = textArea.selectionStart;
        endOffset = textArea.selectionEnd;
        text = textArea.value.substring(startOffset, endOffset);
        switch (item.subCommand) {
            case 'Maximize':
                this.parent.observer.notify(enableFullScreen, { args: args });
                break;
            case 'Minimize':
                this.parent.observer.notify(disableFullScreen, { args: args });
                break;
            case 'CreateLink':
                this.parent.observer.notify(insertLink, { member: 'link', args: args, text: text, module: 'Markdown' });
                break;
            case 'Image':
                this.parent.observer.notify(insertImage, { member: 'image', args: args, text: text, module: 'Markdown' });
                break;
            case 'CreateTable':
                var tableConstant = {
                    'headingText': this.parent.localeData.headingText,
                    'colText': this.parent.localeData.colText
                };
                this.parent.formatter.process(this.parent, args, args.originalEvent, tableConstant);
                break;
            default:
                this.parent.formatter.process(this.parent, args, args.originalEvent, null);
                break;
        }
    };
    MarkdownEditor.prototype.removeEventListener = function () {
        this.parent.observer.off(initialEnd, this.render);
        this.parent.observer.off(destroy, this.destroy);
        this.parent.observer.off(markdownToolbarClick, this.onToolbarClick);
        this.parent.observer.off(selectAll$1, this.selectAll);
        this.parent.observer.off(getSelectedHtml, this.getSelectedHtml);
        this.parent.observer.off(selectionSave, this.onSelectionSave);
        this.parent.observer.off(selectionRestore, this.onSelectionRestore);
        this.parent.observer.off(readOnlyMode, this.updateReadOnly);
    };
    MarkdownEditor.prototype.render = function () {
        var editElement = this.parent.getEditPanel();
        var option = { undoRedoSteps: this.parent.undoRedoSteps, undoRedoTimer: this.parent.undoRedoTimer };
        if (sf.base.isNullOrUndefined(this.parent.adapter)) {
            this.parent.formatter = new MarkdownFormatter({
                element: editElement,
                options: option
            });
        }
        else {
            this.parent.formatter = new MarkdownFormatter(sf.base.extend({}, this.parent.adapter, {
                element: editElement,
                options: option
            }));
        }
        if (this.parent.toolbarSettings.enable) {
            this.toolbarUpdate = new MarkdownToolbarStatus(this.parent);
        }
        this.parent.observer.notify(bindOnEnd, {});
    };
    MarkdownEditor.prototype.selectAll = function () {
        this.parent.formatter.editorManager.markdownSelection.setSelection(this.parent.getEditPanel(), 0, this.parent.getEditPanel().value.length);
    };
    MarkdownEditor.prototype.getSelectedHtml = function (e) {
        e.callBack(this.parent.formatter.editorManager.markdownSelection.getSelectedText(this.parent.getEditPanel()));
    };
    MarkdownEditor.prototype.destroy = function () {
        this.removeEventListener();
    };
    return MarkdownEditor;
}());

/**
 * `ExecCommandCallBack` module is used to run the editor manager command
 */
var ExecCommandCallBack = /** @class */ (function () {
    function ExecCommandCallBack(parent) {
        this.parent = parent;
        this.addEventListener();
    }
    ExecCommandCallBack.prototype.addEventListener = function () {
        this.parent.observer.on(execCommandCallBack, this.commandCallBack, this);
        this.parent.observer.on(destroy, this.removeEventListener, this);
    };
    ExecCommandCallBack.prototype.commandCallBack = function (args) {
        if (args.requestType !== 'Undo' && args.requestType !== 'Redo') {
            this.parent.formatter.saveData();
        }
        this.parent.observer.notify(toolbarRefresh, { args: args });
        this.parent.observer.notify(count, {});
    };
    ExecCommandCallBack.prototype.removeEventListener = function () {
        this.parent.observer.off(execCommandCallBack, this.commandCallBack);
        this.parent.observer.off(destroy, this.removeEventListener);
    };
    return ExecCommandCallBack;
}());

var __assign = (undefined && undefined.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
/**
 * RichTextEditor base
 */
var SfRichTextEditor = /** @class */ (function () {
    //#endregion
    function SfRichTextEditor(element, options, dotnetRef) {
        this.undoRedoSteps = 30;
        this.undoRedoTimer = 300;
        this.floatingToolbarOffset = 0;
        this.enableHtmlEncode = false;
        this.defaultResetValue = null;
        this.isInitial = false;
        this.blurEnabled = false;
        this.focusEnabled = false;
        this.undoRedoStatus = false;
        this.createdEnabled = false;
        this.isIframeRteElement = true;
        this.actionBeginEnabled = false;
        this.imageDeleteEnabled = false;
        this.onResizeStopEnabled = false;
        this.quickTbClosedEnabled = false;
        this.quickTbOpenedEnabled = false;
        this.onQuickTbOpenEnabled = false;
        this.onResizeStartEnabled = false;
        this.actionCompleteEnabled = false;
        this.beforeUploadImageEnabled = false;
        this.onImageUploadFailedEnabled = false;
        this.onImageUploadSuccessEnabled = false;
        this.beforePasteCleanupEnabled = false;
        this.afterPasteCleanupEnabled = false;
        this.inlineCloseItems = ['CreateLink', 'Image', 'CreateTable', 'Maximize', 'Minimize'];
        if (sf.base.isNullOrUndefined(element)) {
            return;
        }
        this.element = element;
        this.element.blazor__instance = this;
        this.dotNetRef = dotnetRef;
        this.updateContext(options);
        this.observer = new sf.base.Observer(this);
        this.dotNetRef.invokeMethodAsync('UpdateDeviceData', { isDevice: sf.base.Browser.isDevice, isIDevice: isIDevice() });
        this.initModules();
    }
    //#region Internal methods
    SfRichTextEditor.prototype.updateContext = function (rteObj) {
        sf.base.extend(this, this, rteObj);
    };
    SfRichTextEditor.prototype.initModules = function () {
        if (this.editorMode === 'HTML') {
            this.htmlEditorModule = new HtmlEditor(this);
            this.pasteCleanupModule = new PasteCleanup(this);
        }
        else {
            this.markdownEditorModule = new MarkdownEditor(this);
        }
        if (this.toolbarSettings.enable) {
            this.isUndoRedoStatus();
            this.toolbarModule = new Toolbar(this);
        }
        this.fullScreenModule = new FullScreen(this);
        this.enterKeyModule = new EnterKeyAction(this);
        this.viewSourceModule = new ViewSource(this);
        this.countModule = new Count(this);
        this.resizeModule = new Resize(this);
        this.linkModule = new Link(this);
        this.imageModule = new Image(this);
        this.tableModule = new Table(this);
        this.quickToolbarModule = new QuickToolbar(this);
    };
    SfRichTextEditor.prototype.initialize = function () {
        this.isInitial = true;
        this.onBlurHandler = this.blurHandler.bind(this);
        this.onFocusHandler = this.focusHandler.bind(this);
        this.onResizeHandler = this.resizeHandler.bind(this);
        var execCommandCallBack$$1 = new ExecCommandCallBack(this);
        this.id = this.element.id;
        this.clickPoints = { clientX: 0, clientY: 0 };
        this.updateContentElements();
        this.inputElement = this.getEditPanel();
        this.valueContainer = this.element.querySelector('textarea');
        if (this.readonly) {
            this.setReadOnly(true);
        }
        this.setHeight(this.height);
        this.setWidth(this.width);
        // setStyleAttribute(this.element, { 'width': formatUnit(this.width) });
        this.setContentHeight();
        if (this.iframeSettings && this.iframeSettings.enable) {
            this.setIframeSettings();
        }
        if (sf.base.isNullOrUndefined(this.value) || this.value === '') {
            this.value = this.element.querySelector('.e-rte-value-wrapper').innerHTML.replace(/<!--!-->/gi, '').trim();
        }
        this.setPanelValue(this.value);
        this.observer.notify(initialEnd, {});
        if (this.enableXhtml) {
            this.value = this.getXhtml();
        }
        if (this.value !== null) {
            this.defaultResetValue = this.value;
        }
        this.wireEvents();
        if (this.createdEnabled) {
            this.dotNetRef.invokeMethodAsync('CreatedEvent');
        }
    };
    SfRichTextEditor.prototype.isUndoRedoStatus = function () {
        for (var i = 0; i < this.toolbarSettings.items.length; i++) {
            if (!sf.base.isNullOrUndefined(this.toolbarSettings.items[i]) && (this.toolbarSettings.items[i].subCommand === 'Undo'
                || this.toolbarSettings.items[i].subCommand === 'Redo')) {
                this.undoRedoStatus = true;
                break;
            }
        }
    };
    SfRichTextEditor.prototype.setPanelValue = function (value, isReset) {
        value = this.serializeValue(value);
        if (this.editorMode === 'HTML' && this.enableXhtml) {
            this.inputElement.innerHTML = value;
            this.observer.notify(xhtmlValidation, {});
            value = this.inputElement.innerHTML;
        }
        if (this.editorMode === 'HTML' && !isReset) {
            this.value = getEditValue(sf.base.isNullOrUndefined(value) ? '' : value, this);
        }
        this.updatePanelValue();
        if (this.value !== this.cloneValue) {
            if (this.enableXhtml) {
                this.valueContainer.value = this.getXhtmlString(this.valueContainer.value);
            }
            dispatchEvent(this.valueContainer, 'change');
            if (!this.isInitial) {
                this.dotNetRef.invokeMethodAsync('UpdateValue', this.value);
            }
            else {
                this.isInitial = false;
            }
            this.cloneValue = this.value;
        }
        this.setPlaceHolder();
    };
    SfRichTextEditor.prototype.updateContentElements = function () {
        if ((this.editorMode === 'HTML' && (sf.base.isNullOrUndefined(this.iframeSettings) || !this.iframeSettings.enable)) || this.editorMode === 'Markdown') {
            this.contentPanel = this.element.querySelector('.e-rte-content');
        }
        else if (this.editorMode === 'HTML' && (this.iframeSettings && this.iframeSettings.enable)) {
            this.contentPanel = this.element.querySelector('iframe');
            this.updateIframeHtmlContents();
        }
    };
    SfRichTextEditor.prototype.updateIframeHtmlContents = function () {
        var iFrameBodyContent = '<body spellcheck="false" autocorrect="off" contenteditable="true"></body></html>';
        var iFrameContent = IFRAME_HEADER + iFrameBodyContent;
        var iframe = this.contentPanel;
        iframe.srcdoc = iFrameContent;
        iframe.contentDocument.body.id = this.id + '_rte-edit-view';
        iframe.contentDocument.body.setAttribute('aria-owns', this.id);
        iframe.contentDocument.open();
        iFrameContent = this.setThemeColor(iFrameContent, { color: '#333' });
        iframe.contentDocument.write(iFrameContent);
        iframe.contentDocument.close();
        if (this.enableRtl) {
            iframe.contentDocument.body.setAttribute('class', 'e-rtl');
        }
    };
    SfRichTextEditor.prototype.setThemeColor = function (content, styles) {
        var fontColor = getComputedStyle(this.element, '.e-richtexteditor').getPropertyValue('color');
        return content.replace(styles.color, fontColor);
    };
    SfRichTextEditor.prototype.refresh = function (e) {
        if (e === void 0) { e = { requestType: 'refresh' }; }
        this.observer.notify(e.requestType + "-begin", e);
    };
    SfRichTextEditor.prototype.setWidth = function (width) {
        if (width !== 'auto') {
            sf.base.setStyleAttribute(this.element, { 'width': sf.base.formatUnit(this.width) });
        }
        else {
            this.element.style.width = 'auto';
        }
    };
    SfRichTextEditor.prototype.setHeight = function (height) {
        if (height !== 'auto') {
            this.element.style.height = sf.base.formatUnit(height);
        }
        else {
            this.element.style.height = 'auto';
        }
        if (this.toolbarSettings.type === 'Expand' && (typeof (this.height) === 'string' &&
            this.height.indexOf('px') > -1 || typeof (this.height) === 'number')) {
            this.element.classList.add(CLS_RTE_FIXED_TB_EXPAND);
        }
        else {
            this.element.classList.remove(CLS_RTE_FIXED_TB_EXPAND);
        }
    };
    SfRichTextEditor.prototype.openPasteDialog = function () {
        this.dotNetRef.invokeMethodAsync('PasteDialog');
    };
    SfRichTextEditor.prototype.setContentHeight = function (target, isExpand) {
        var heightValue = '';
        var topValue = 0;
        var rteHeightPercent = '';
        var heightPercent = typeof (this.height) === 'string' && this.height.indexOf('%') > -1;
        var sourceCodeEle = this.element.querySelector('.e-rte-content .e-rte-srctextarea');
        var cntEle = (!sf.base.isNullOrUndefined(sourceCodeEle) &&
            sourceCodeEle.parentElement.style.display === 'block') ? sourceCodeEle.parentElement : this.getPanel();
        var rteHeight = this.element.offsetHeight;
        if (rteHeight === 0 && this.height !== 'auto' && !this.getToolbar()) {
            rteHeight = parseInt(this.height, 10);
            if (heightPercent) {
                rteHeightPercent = this.height;
            }
        }
        var tbHeight = this.getToolbar() ? this.toolbarModule.getToolbarHeight() : 0;
        var rzHandle = this.element.querySelector('.' + CLS_RTE_RES_HANDLE);
        var rzHeight = this.enableResize ? (!sf.base.isNullOrUndefined(rzHandle) ? (rzHandle.offsetHeight + 8) : 0) : 0;
        var expandPopHeight = this.getToolbar() ? this.toolbarModule.getExpandTBarPopHeight() : 0;
        if (this.toolbarSettings.type === 'Expand' && isExpand && target !== 'preview') {
            heightValue = (this.height === 'auto' && rzHeight === 0 && !this.element.classList.contains('e-rte-full-screen')) ?
                'auto' : rteHeight - (tbHeight + expandPopHeight + rzHeight) + 'px';
            topValue = (!this.toolbarSettings.enableFloating) ? expandPopHeight : 0;
        }
        else {
            if (this.height === 'auto' && !(this.element.classList.contains('e-rte-full-screen')) && !this.isResizeInitialized) {
                heightValue = 'auto';
            }
            else {
                heightValue = heightPercent && rteHeightPercent ? rteHeightPercent : (this.element.classList.contains('e-rte-full-screen') ?
                    window.innerHeight : rteHeight) - (tbHeight + expandPopHeight + rzHeight) + 'px';
            }
        }
        if (target !== 'windowResize') {
            sf.base.setStyleAttribute(cntEle, { height: heightValue, marginTop: topValue + 'px' });
        }
        if (this.iframeSettings.enable && target === 'sourceCode') {
            var codeElement = sf.base.select('.' + CLS_RTE_CONTENT, this.element);
            sf.base.setStyleAttribute(codeElement, { height: heightValue, marginTop: topValue + 'px' });
        }
        if (this.toolbarSettings.enableFloating && this.getToolbar() && !this.inlineMode.enable) {
            var isExpandOpened = !sf.base.isNullOrUndefined(this.element.querySelector('.e-rte-toolbar .e-hor-nav.e-nav-active'));
            var tbContainerHeight = (isExpandOpened ? (tbHeight + expandPopHeight) : tbHeight) + 'px';
            sf.base.setStyleAttribute(this.getToolbar().parentElement, { height: tbContainerHeight });
        }
        if (rzHeight === 0) {
            this.autoResize();
        }
    };
    SfRichTextEditor.prototype.getXhtml = function () {
        var currentValue = this.value;
        if (this.enableXhtml) {
            currentValue = this.htmlEditorModule.xhtmlValidation.selfEncloseValidation(currentValue);
        }
        return currentValue;
    };
    SfRichTextEditor.prototype.getXhtmlString = function (value) {
        var currentValue = value;
        if (this.enableXhtml) {
            currentValue = this.htmlEditorModule.xhtmlValidation.selfEncloseValidation(currentValue);
        }
        return currentValue;
    };
    SfRichTextEditor.prototype.getPanel = function () {
        return this.contentPanel;
    };
    SfRichTextEditor.prototype.saveSelection = function () {
        this.formatter.editorManager.nodeSelection.save(this.getRange(), this.getDocument());
    };
    SfRichTextEditor.prototype.restoreSelection = function () {
        this.formatter.editorManager.nodeSelection.restore();
    };
    SfRichTextEditor.prototype.getEditPanel = function () {
        var editNode;
        if (this.iframeSettings && this.iframeSettings.enable) {
            if (!sf.base.isNullOrUndefined(this.contentPanel.contentDocument)) {
                editNode = this.contentPanel.contentDocument.body;
            }
            else {
                editNode = this.inputElement;
            }
        }
        else {
            editNode = this.element.querySelector('.e-rte-content .e-content');
        }
        return editNode;
    };
    SfRichTextEditor.prototype.getText = function () {
        return this.getEditPanel().innerText;
    };
    SfRichTextEditor.prototype.getDocument = function () {
        return this.getEditPanel().ownerDocument;
    };
    SfRichTextEditor.prototype.getRange = function () {
        return this.formatter.editorManager.nodeSelection.getRange(this.getDocument());
    };
    SfRichTextEditor.prototype.updateValueContainer = function (val) {
        if (this.enableXhtml && !sf.base.isNullOrUndefined(val)) {
            val = this.getXhtmlString(val);
        }
        this.valueContainer.value = val;
        dispatchEvent(this.valueContainer, 'change');
    };
    SfRichTextEditor.prototype.getInputInnerHtml = function () {
        return this.inputElement.innerHTML.replace(/<!--!-->/gi, '');
    };
    SfRichTextEditor.prototype.refreshUI = function () {
        this.refresh();
    };
    SfRichTextEditor.prototype.getUpdatedValue = function () {
        var value;
        if (!sf.base.isNullOrUndefined(this.tableModule)) {
            this.tableModule.removeResizeEle();
        }
        var getTextArea = this.element.querySelector('.e-rte-srctextarea');
        if (this.editorMode === 'HTML') {
            var inputContent = this.getInputInnerHtml();
            value = (inputContent === getDefaultValue(this)) ? null : this.enableHtmlEncode ?
                this.encode(decode(inputContent)) : inputContent;
            if (getTextArea && getTextArea.style.display === 'block') {
                value = getTextArea.value;
            }
        }
        else {
            value = this.inputElement.value === '' ? null :
                this.inputElement.value;
        }
        return value;
    };
    SfRichTextEditor.prototype.countCalculate = function () {
        this.countModule.renderCount();
    };
    SfRichTextEditor.prototype.updateEnable = function () {
        if (this.enabled) {
            sf.base.removeClass([this.element], CLS_DISABLED);
            this.element.setAttribute('aria-disabled', 'false');
            if (!sf.base.isNullOrUndefined(this.htmlAttributes.tabindex)) {
                this.inputElement.setAttribute('tabindex', this.htmlAttributes.tabindex);
            }
            else {
                this.inputElement.setAttribute('tabindex', '0');
            }
        }
        else {
            if (this.getToolbar()) {
                sf.base.removeClass(this.getToolbar().querySelectorAll('.' + CLS_ACTIVE), CLS_ACTIVE);
                sf.base.removeClass([this.getToolbar()], [CLS_TB_FLOAT, CLS_TB_ABS_FLOAT]);
            }
            sf.base.addClass([this.element], CLS_DISABLED);
            this.element.tabIndex = -1;
            this.element.setAttribute('aria-disabled', 'true');
            this.inputElement.setAttribute('tabindex', '-1');
        }
    };
    SfRichTextEditor.prototype.setEnable = function () {
        this.updateEnable();
        (this.enabled) ? this.wireEvents() : this.unWireEvents();
    };
    SfRichTextEditor.prototype.executeCommand = function (commandName, value, option) {
        value = this.htmlPurifier(commandName, value);
        if (this.editorMode === 'HTML') {
            var range = this.getRange();
            if (this.iframeSettings.enable) {
                this.formatter.editorManager.nodeSelection.Clear(this.element.ownerDocument);
            }
            var toFocus = (this.iframeSettings.enable &&
                range.startContainer === this.inputElement) ? true : !this.inputElement.contains(range.startContainer);
            if (toFocus) {
                this.focusIn();
            }
        }
        var tool = executeGroup[commandName];
        if (option && option.undo) {
            if (option.undo && this.formatter.getUndoRedoStack().length === 0) {
                this.formatter.saveData();
            }
        }
        this.formatter.editorManager.execCommand(tool.command, tool.subCommand ? tool.subCommand : (value ? value : tool.value), null, null, (value ? value : tool.value), (value ? value : tool.value));
        if (option && option.undo) {
            this.formatter.saveData();
            this.formatter.enableUndo(this);
        }
        this.setPlaceHolder();
        this.observer.notify(contentChanged, {});
        this.value = this.inputElement.innerHTML;
        this.dotNetRef.invokeMethodAsync('UpdateValue', this.value);
    };
    SfRichTextEditor.prototype.htmlPurifier = function (command, value) {
        if (this.editorMode === 'HTML') {
            switch (command) {
                case 'insertTable':
                    if (sf.base.isNullOrUndefined(value.width)) {
                        value.width = {
                            minWidth: this.tableSettings.minWidth,
                            maxWidth: this.tableSettings.maxWidth, width: this.tableSettings.width
                        };
                    }
                    value.selection = this.formatter.editorManager.nodeSelection.save(this.getRange(), this.getDocument());
                    break;
                case 'insertImage':
                    var temp = sf.base.createElement('img', {
                        attrs: {
                            src: value.url
                        }
                    });
                    var imageValue = temp.outerHTML;
                    var url = (imageValue !== '' && (sf.base.createElement('div', {
                        innerHTML: imageValue
                    }).firstElementChild).getAttribute('src')) || null;
                    url = !sf.base.isNullOrUndefined(url) ? url : '';
                    value.url = url;
                    if (sf.base.isNullOrUndefined(value.width)) {
                        value.width = {
                            minWidth: this.insertImageSettings.minWidth,
                            maxWidth: this.insertImageSettings.maxWidth, width: this.insertImageSettings.width
                        };
                    }
                    if (sf.base.isNullOrUndefined(value.height)) {
                        value.height = {
                            minHeight: this.insertImageSettings.minHeight,
                            maxHeight: this.insertImageSettings.maxHeight, height: this.insertImageSettings.height
                        };
                    }
                    value.selection = this.formatter.editorManager.nodeSelection.save(this.getRange(), this.getDocument());
                    break;
                case 'createLink':
                    var tempNode = sf.base.createElement('a', {
                        attrs: {
                            href: value.url
                        }
                    });
                    var linkValue = tempNode.outerHTML;
                    var href = (linkValue !== '' && (sf.base.createElement('div', {
                        innerHTML: linkValue
                    }).firstElementChild).getAttribute('href')) || null;
                    href = !sf.base.isNullOrUndefined(href) ? href : '';
                    value.url = href;
                    value.selection = this.formatter.editorManager.nodeSelection.save(this.getRange(), this.getDocument());
                    break;
            }
        }
        return value;
    };
    SfRichTextEditor.prototype.serializeValue = function (value) {
        if (this.editorMode === 'HTML' && !sf.base.isNullOrUndefined(value)) {
            if (this.enableHtmlEncode) {
                value = this.htmlEditorModule.sanitizeHelper(decode(value));
                value = this.encode(value);
            }
            else {
                value = this.htmlEditorModule.sanitizeHelper(value);
            }
        }
        return value;
    };
    SfRichTextEditor.prototype.selectAll = function () {
        this.observer.notify(selectAll$1, {});
    };
    SfRichTextEditor.prototype.selectRange = function (range) {
        this.observer.notify(selectRange, { range: range });
    };
    SfRichTextEditor.prototype.showFullScreen = function () {
        this.fullScreenModule.showFullScreen();
    };
    SfRichTextEditor.prototype.sanitizeHtml = function (value) {
        return this.serializeValue(value);
    };
    SfRichTextEditor.prototype.updateValue = function (value) {
        if (sf.base.isNullOrUndefined(value)) {
            var inputVal = this.inputElement.innerHTML;
            //this.setProperties({ value: isEditableValueEmpty(inputVal) ? null : inputVal });
        }
        else {
            //this.setProperties({ value: value });
        }
    };
    SfRichTextEditor.prototype.clipboardAction = function (action, event) {
        switch (action.toLowerCase()) {
            case 'cut':
                this.onCut();
                this.formatter.onSuccess(this, {
                    requestType: 'Cut',
                    editorMode: this.editorMode,
                    event: event
                });
                break;
            case 'copy':
                this.onCopy();
                this.formatter.onSuccess(this, {
                    requestType: 'Copy',
                    editorMode: this.editorMode,
                    event: event
                });
                break;
            case 'paste':
                this.onPaste(event);
                break;
        }
    };
    SfRichTextEditor.prototype.getContent = function () {
        if (this.iframeSettings.enable) {
            return this.inputElement;
        }
        else {
            return this.getPanel();
        }
    };
    SfRichTextEditor.prototype.getSelectedHtml = function () {
        var range;
        var containerElm = sf.base.createElement('div');
        var selection = this.getDocument().getSelection();
        if (selection.rangeCount > 0) {
            range = selection.getRangeAt(0);
            var selectedHtml = range.cloneContents();
            containerElm.appendChild(selectedHtml);
        }
        return containerElm.innerHTML;
    };
    SfRichTextEditor.prototype.getSelection = function () {
        var str = '';
        this.observer.notify(getSelectedHtml, {
            callBack: function (txt) {
                str = txt;
            }
        });
        return str;
    };
    SfRichTextEditor.prototype.showInlineToolbar = function () {
        if (this.inlineMode.enable) {
            var currentRange = this.getRange();
            var targetElm = currentRange.endContainer.nodeName === '#text' ?
                currentRange.endContainer.parentElement : currentRange.endContainer;
            var x = currentRange.getClientRects()[0].left;
            var y = currentRange.getClientRects()[0].top;
            this.quickToolbarModule.showInlineQTBar(x, y, targetElm);
        }
    };
    SfRichTextEditor.prototype.hideInlineToolbar = function () {
        this.quickToolbarModule.hideInlineQTBar();
    };
    SfRichTextEditor.prototype.updateValueData = function () {
        if (this.enableHtmlEncode) {
            this.setPanelValue(this.encode(decode(this.inputElement.innerHTML)));
        }
        else {
            var value = /<[a-z][\s\S]*>/i.test(this.inputElement.innerHTML) ? this.inputElement.innerHTML :
                decode(this.inputElement.innerHTML);
            this.setPanelValue(value);
        }
    };
    SfRichTextEditor.prototype.removeSheets = function (srcList) {
        var i;
        for (i = 0; i < srcList.length; i++) {
            sf.base.detach(srcList[i]);
        }
    };
    SfRichTextEditor.prototype.updateReadOnly = function () {
        this.observer.notify(readOnlyMode, { editPanel: this.inputElement, mode: this.readonly });
    };
    SfRichTextEditor.prototype.setReadOnly = function (initial) {
        this.updateReadOnly();
        if (!initial) {
            if (this.readonly && this.enabled) {
                this.unBindEvents();
            }
            else if (this.enabled) {
                this.bindEvents();
            }
        }
    };
    SfRichTextEditor.prototype.setIframeSettings = function () {
        if (this.iframeSettings.resources) {
            var styleSrc = this.iframeSettings.resources.styles;
            var scriptSrc = this.iframeSettings.resources.scripts;
            if (this.iframeSettings.resources.styles.length > 0) {
                this.InjectSheet(false, styleSrc);
            }
            if (this.iframeSettings.resources.scripts.length > 0) {
                this.InjectSheet(true, scriptSrc);
            }
        }
        if (this.iframeSettings.attributes) {
            setAttributes(this.iframeSettings.attributes, this, true, false);
        }
    };
    SfRichTextEditor.prototype.InjectSheet = function (scriptSheet, srcList) {
        try {
            if (srcList && srcList.length > 0) {
                var iFrame = this.getDocument();
                var target = iFrame.querySelector('head');
                for (var i = 0; i < srcList.length; i++) {
                    if (scriptSheet) {
                        var scriptEle = this.createScriptElement();
                        scriptEle.src = srcList[i];
                        target.appendChild(scriptEle);
                    }
                    else {
                        var styleEle = this.createStyleElement();
                        styleEle.href = srcList[i];
                        target.appendChild(styleEle);
                    }
                }
            }
        }
        catch (e) {
            return;
        }
    };
    SfRichTextEditor.prototype.createScriptElement = function () {
        var scriptEle = sf.base.createElement('script', {
            className: CLS_SCRIPT_SHEET
        });
        scriptEle.type = 'text/javascript';
        return scriptEle;
    };
    SfRichTextEditor.prototype.createStyleElement = function () {
        var styleEle = sf.base.createElement('link', {
            className: CLS_STYLE_SHEET
        });
        styleEle.rel = 'stylesheet';
        return styleEle;
    };
    SfRichTextEditor.prototype.setValue = function () {
        var innerHtml = !sf.base.isNullOrUndefined(this.element.innerHTML) && this.element.innerHTML.replace(/<(\/?|\!?)(!--!--)>/g, '').trim();
        if (innerHtml !== '') {
            if (this.element.tagName === 'TEXTAREA') {
                // this.setProperties({ value: decode(innerHtml) });
            }
            else {
                //  this.setProperties({ value: innerHtml });
            }
        }
    };
    SfRichTextEditor.prototype.updateResizeFlag = function () {
        this.isResizeInitialized = true;
    };
    SfRichTextEditor.prototype.getHtml = function () {
        return this.serializeValue(this.getEditPanel().innerHTML);
    };
    SfRichTextEditor.prototype.showSourceCode = function () {
        if (this.readonly) {
            return;
        }
        this.observer.notify(sourceCode, {});
    };
    SfRichTextEditor.prototype.getCharCount = function () {
        var htmlText = this.editorMode === 'Markdown' ? this.getEditPanel().value.trim() :
            this.getEditPanel().textContent.trim();
        var htmlLength;
        if (this.editorMode !== 'Markdown' && htmlText.indexOf('\u200B') !== -1) {
            htmlLength = htmlText.replace(/\u200B/g, '').length;
        }
        else {
            htmlLength = htmlText.length;
        }
        return htmlLength;
    };
    SfRichTextEditor.prototype.focusOut = function () {
        if (this.enabled) {
            this.inputElement.blur();
            this.blurHandler({});
        }
    };
    // public getBaseToolbarObject(): BaseToolbar {
    //     let tbObj: BaseToolbar;
    //     if (this.inlineMode.enable && (!Browser.isDevice || isIDevice())) {
    //         tbObj = this.quickToolbarModule && this.quickToolbarModule.getInlineBaseToolbar();
    //     } else {
    //         tbObj = this.toolbarModule && this.toolbarModule.getBaseToolbar();
    //     }
    //     return tbObj;
    // }
    SfRichTextEditor.prototype.getToolbar = function () {
        return this.toolbarSettings.enable ? this.element.querySelector('#' + this.id + '_toolbar') : null;
    };
    SfRichTextEditor.prototype.getToolbarElement = function () {
        return this.toolbarSettings.enable ? this.element.querySelector('#' + this.id + '_toolbar') : null;
    };
    SfRichTextEditor.prototype.updateIntervalValue = function () {
        clearTimeout(this.idleInterval);
        this.idleInterval = setTimeout(this.updateValueOnIdle.bind(this), 0);
    };
    SfRichTextEditor.prototype.updateValueOnIdle = function () {
        if (!sf.base.isNullOrUndefined(this.tableModule) && !sf.base.isNullOrUndefined(this.inputElement.querySelector('.e-table-box.e-rbox-select'))) {
            return;
        }
        this.value = this.getUpdatedValue();
        this.updateValueContainer(this.value);
        this.invokeChangeEvent();
    };
    SfRichTextEditor.prototype.invokeChangeEvent = function () {
        if (this.enableXhtml && !sf.base.isNullOrUndefined(this.value)) {
            this.value = this.getXhtml();
        }
        if (this.value !== this.cloneValue) {
            if (this.enablePersistence) {
                window.localStorage.setItem(this.id, this.value);
            }
            this.dotNetRef.invokeMethodAsync('ChangeEvent');
            this.cloneValue = this.value;
        }
    };
    SfRichTextEditor.prototype.preventImgResize = function (e) {
        if (e.target.nodeName.toLocaleLowerCase() === 'img') {
            e.preventDefault();
        }
    };
    SfRichTextEditor.prototype.defaultResize = function (e, isDefault) {
        if (sf.base.Browser.info.name === 'msie') {
            if (isDefault) {
                this.getEditPanel().removeEventListener('mscontrolselect', this.preventImgResize);
            }
            else {
                this.getEditPanel().addEventListener('mscontrolselect', this.preventImgResize);
            }
        }
        else if (sf.base.Browser.info.name === 'mozilla') {
            var value = isDefault ? 'true' : 'false';
            this.getDocument().execCommand('enableObjectResizing', isDefault, value);
            this.getDocument().execCommand('enableInlineTableEditing', isDefault, value);
        }
    };
    SfRichTextEditor.prototype.encode = function (value) {
        var divNode = document.createElement('div');
        divNode.innerText = value.trim();
        return divNode.innerHTML.replace(/<br\s*[\/]?>/gi, '\n');
    };
    SfRichTextEditor.prototype.print = function () {
        var _this = this;
        var printWind;
        var printArgs = {
            requestType: 'print',
            cancel: false
        };
        // @ts-ignore-start
        this.dotNetRef.invokeMethodAsync('ActionBeginEvent', printArgs).then(function (printingArgs) {
            // @ts-ignore-end
            printWind = window.open('', 'print', 'height=' + window.outerHeight + ',width=' + window.outerWidth);
            if (sf.base.Browser.info.name === 'msie') {
                printWind.resizeTo(screen.availWidth, screen.availHeight);
            }
            printWind = sf.base.print(_this.inputElement, printWind);
            if (!printingArgs.cancel) {
                var actionArgs = {
                    requestType: 'print'
                };
                _this.dotNetRef.invokeMethodAsync('ActionCompleteEvent', actionArgs);
            }
        });
    };
    SfRichTextEditor.prototype.autoResize = function () {
        var _this = this;
        if (this.height === 'auto') {
            if (this.editorMode === 'Markdown') {
                setTimeout(function () { _this.setAutoHeight(_this.inputElement); }, 0);
            }
            else if (this.iframeSettings.enable) {
                var iframeElement_1 = this.element.querySelector('#' + this.element.id + '_rte-view');
                setTimeout(function () { _this.setAutoHeight(iframeElement_1); }, 100);
                this.inputElement.style.overflow = 'hidden';
            }
        }
        else {
            this.inputElement.style.overflow = null;
        }
    };
    SfRichTextEditor.prototype.setAutoHeight = function (element) {
        if (!sf.base.isNullOrUndefined(element)) {
            element.style.height = '';
            element.style.height = this.inputElement.scrollHeight + 'px';
            element.style.overflow = 'hidden';
        }
    };
    SfRichTextEditor.prototype.restrict = function (e) {
        if (this.maxLength >= 0) {
            var element = this.editorMode === 'Markdown' ? this.getText() :
                e.currentTarget.textContent.trim();
            var array = [8, 16, 17, 37, 38, 39, 40, 46, 65];
            var arrayKey = void 0;
            for (var i = 0; i <= array.length - 1; i++) {
                if (e.which === array[i]) {
                    if (e.ctrlKey && e.which === 65) {
                        return;
                    }
                    else if (e.which !== 65) {
                        arrayKey = array[i];
                        return;
                    }
                }
            }
            if ((element.length >= this.maxLength && this.maxLength !== -1) && e.which !== arrayKey) {
                e.preventDefault();
            }
        }
    };
    SfRichTextEditor.prototype.setPlaceHolder = function () {
        if (this.inputElement && this.placeholder && this.iframeSettings.enable !== true) {
            if (this.editorMode !== 'Markdown') {
                if (!this.placeHolderContainer) {
                    this.placeHolderContainer = this.element.querySelector('.e-rte-placeholder');
                }
                this.placeHolderContainer.innerHTML = this.placeholder;
                if (this.inputElement.textContent.length === 0 && !sf.base.isNullOrUndefined(this.inputElement.firstChild) && (this.inputElement.firstChild.nodeName === 'BR' ||
                    ((this.inputElement.firstChild.nodeName === 'P' || this.inputElement.firstChild.nodeName === 'DIV') && !sf.base.isNullOrUndefined(this.inputElement.firstChild.firstChild) &&
                        this.inputElement.firstChild.firstChild.nodeName === 'BR'))) {
                    this.placeHolderContainer.style.display = 'block';
                }
                else {
                    this.placeHolderContainer.style.display = 'none';
                }
            }
            else {
                this.inputElement.setAttribute('placeholder', this.placeholder);
            }
        }
    };
    SfRichTextEditor.prototype.updatePanelValue = function () {
        var value = this.value;
        value = (this.enableHtmlEncode && this.value) ? decode(value) : value;
        var getTextArea = this.element.querySelector('.e-rte-srctextarea');
        if (value) {
            if (getTextArea && getTextArea.style.display === 'block') {
                getTextArea.value = this.value;
            }
            if (this.valueContainer) {
                this.valueContainer.value = (this.enableHtmlEncode) ? this.value : value;
            }
            if (this.editorMode === 'HTML' && this.inputElement && this.inputElement.innerHTML.trim() !== value.trim()) {
                this.inputElement.innerHTML = value;
            }
            else if (this.editorMode === 'Markdown' && this.inputElement
                && this.inputElement.value.trim() !== value.trim()) {
                this.inputElement.value = value;
            }
        }
        else {
            if (getTextArea && getTextArea.style.display === 'block') {
                getTextArea.value = '';
            }
            if (this.editorMode === 'HTML') {
                this.inputElement.innerHTML = getDefaultValue(this);
            }
            else {
                this.inputElement.value = '';
            }
            if (this.valueContainer) {
                this.valueContainer.value = '';
            }
        }
        if (this.showCharCount) {
            this.countModule.refresh();
        }
    };
    SfRichTextEditor.prototype.contentChanged = function () {
        if (this.autoSaveOnIdle) {
            if (!sf.base.isNullOrUndefined(this.saveInterval)) {
                clearTimeout(this.timeInterval);
                this.timeInterval = setTimeout(this.updateIntervalValue.bind(this), this.saveInterval);
            }
        }
    };
    SfRichTextEditor.prototype.notifyMouseUp = function (e) {
        var touch = (e.touches ? e.changedTouches[0] : e);
        this.observer.notify(mouseUp, {
            member: 'mouseUp', args: e,
            touchData: {
                prevClientX: this.clickPoints.clientX, prevClientY: this.clickPoints.clientY,
                clientX: touch.clientX, clientY: touch.clientY
            }
        });
        if (this.inputElement && ((this.editorMode === 'HTML' && this.inputElement.textContent.length !== 0) ||
            (this.editorMode === 'Markdown' && this.inputElement.value.length !== 0))) {
            this.observer.notify(toolbarRefresh, { args: e });
        }
        this.triggerEditArea(e);
    };
    SfRichTextEditor.prototype.triggerEditArea = function (e) {
        if (!isIDevice()) {
            this.observer.notify(editAreaClick, { member: 'editAreaClick', args: e });
        }
        else {
            var touch = (e.touches ? e.changedTouches[0] : e);
            if (this.clickPoints.clientX === touch.clientX && this.clickPoints.clientY === touch.clientY) {
                this.observer.notify(editAreaClick, { member: 'editAreaClick', args: e });
            }
        }
    };
    SfRichTextEditor.prototype.updateStatus = function (e) {
        if (!sf.base.isNullOrUndefined(e.html) || !sf.base.isNullOrUndefined(e.markdown)) {
            var status_1 = this.formatter.editorManager.undoRedoManager.getUndoStatus();
            this.dotNetRef.invokeMethodAsync(updatedToolbarStatusEvent, {
                undo: status_1.undo,
                redo: status_1.redo,
                html: e.html,
                markdown: e.markdown
            });
        }
    };
    //#endregion
    //#region Interop interaction methods
    SfRichTextEditor.prototype.onListDropDownButtonClick = function (args) {
        if (sf.base.isNullOrUndefined(args)) {
            return;
        }
        args.item.subCommand = args.item.subCommand === "BulletFormatList" ? "UL" : "OL";
        if (this.inlineCloseItems.indexOf(args.item.subCommand) > -1) {
            this.quickToolbarModule.hideInlineQTBar();
        }
        if (this.editorMode === 'HTML') {
            this.observer.notify(htmlToolbarClick, args);
        }
    };
    
    SfRichTextEditor.prototype.toolbarItemClick = function (args, id, targetType) {
        if (sf.base.isNullOrUndefined(args)) {
            return;
        }
        var target;
        if (targetType === 'Root' && !this.inlineMode.enable) {
            target = sf.base.select('#' + id, this.element);
        }
        else {
            target = sf.base.select('#' + id, document.body);
        }
        args.originalEvent = __assign({}, args.originalEvent, { target: target });
        if (this.inlineCloseItems.indexOf(args.item.subCommand) > -1) {
            this.quickToolbarModule.hideInlineQTBar();
        }
        if (this.editorMode === 'HTML') {
            this.observer.notify(htmlToolbarClick, args);
        }
        else {
            this.observer.notify(markdownToolbarClick, args);
        }
    };
    SfRichTextEditor.prototype.toolbarClick = function (id) {
        var trg = sf.base.select('#' + id + ' .e-hor-nav', this.element);
        if (trg && this.toolbarSettings.type === 'Expand' && !sf.base.isNullOrUndefined(trg)) {
            if (!trg.classList.contains('e-nav-active')) {
                sf.base.removeClass([this.getToolbar()], [CLS_EXPAND_OPEN]);
                this.setContentHeight('toolbar', false);
            }
            else {
                sf.base.addClass([this.getToolbar()], [CLS_EXPAND_OPEN]);
                this.setContentHeight('toolbar', true);
            }
        }
        else if (sf.base.Browser.isDevice || this.inlineMode.enable) {
            //this.isToolbar = true;
        }
        if (sf.base.isNullOrUndefined(trg) && this.toolbarSettings.type === 'Expand') {
            sf.base.removeClass([this.getToolbar()], [CLS_EXPAND_OPEN]);
        }
    };
    SfRichTextEditor.prototype.hideTableQuickToolbar = function () {
        this.quickToolbarModule.hideTableQTBar();
    };
    SfRichTextEditor.prototype.dropDownBeforeOpen = function (args) {
        this.observer.notify(selectionSave, args);
        this.observer.notify(beforeDropDownOpen, args);
    };
    SfRichTextEditor.prototype.dropDownClose = function (args) {
        this.observer.notify(selectionRestore, args);
    };
    SfRichTextEditor.prototype.dropDownSelect = function (e) {
        e.name = 'dropDownSelect';
        if (!(document.body.contains(document.body.querySelector('.e-rte-quick-toolbar'))
            && e.item && (e.item.command === 'Images' || e.item.command === 'Display' || e.item.command === 'Table'))) {
            this.observer.notify(selectionRestore, {});
            var value = null;
            // let value: string = e.item.controlParent && this.quickToolbarModule && this.quickToolbarModule.tableQTBar
            //     && this.quickToolbarModule.tableQTBar.element.contains(e.item.controlParent.element) ? 'Table' : null;
            if (e.item.command === 'Lists') {
                var listItem = { listStyle: e.item.value, listImage: e.item.listImage, type: e.item.subCommand };
                this.formatter.process(this, e, e.originalEvent, listItem);
            }
            else {
                this.formatter.process(this, e, e.originalEvent, value);
            }
            this.observer.notify(selectionSave, {});
        }
        this.observer.notify(dropDownSelect, e);
    };
    SfRichTextEditor.prototype.colorDropDownBeforeOpen = function () {
        this.observer.notify(selectionSave, {});
    };
    SfRichTextEditor.prototype.colorIconSelected = function (args, value, container) {
        this.observer.notify(selectionSave, {});
        this.observer.notify(selectionRestore, {});
        args.value = sf.base.isNullOrUndefined(value) ? args.value : value;
        var range = this.formatter.editorManager.nodeSelection.getRange(this.getDocument());
        var parentNode = range.startContainer.parentNode;
        if ((range.startContainer.nodeName === 'TD' || range.startContainer.nodeName === 'TH' ||
            (sf.base.closest(range.startContainer.parentNode, 'td,th')) || (this.iframeSettings.enable &&
            !hasClass(parentNode.ownerDocument.querySelector('body'), 'e-lib'))) && range.collapsed &&
            args.subCommand === 'BackgroundColor' && container === 'quick') {
            this.observer.notify(tableColorPickerChanged, { item: args, name: 'colorPickerChanged' });
        }
        else {
            this.observer.notify(selectionRestore, {});
            this.formatter.process(this, { item: args, name: 'colorPickerChanged' }, undefined, null);
            this.observer.notify(selectionSave, {});
        }
    };
    SfRichTextEditor.prototype.colorChanged = function (args, value, container) {
        this.observer.notify(selectionRestore, {});
        args.value = sf.base.isNullOrUndefined(value) ? args.value : value;
        var range = this.formatter.editorManager.nodeSelection.getRange(this.getDocument());
        if ((range.startContainer.nodeName === 'TD' || range.startContainer.nodeName === 'TH' ||
            sf.base.closest(range.startContainer.parentNode, 'td,th')) && range.collapsed && args.subCommand === 'BackgroundColor'
            && container === 'quick') {
            this.observer.notify(tableColorPickerChanged, { item: args, name: 'colorPickerChanged' });
        }
        else {
            this.observer.notify(selectionRestore, {});
            this.formatter.process(this, { item: args, name: 'colorPickerChanged' }, undefined, null);
            this.observer.notify(selectionSave, {});
        }
    };
    SfRichTextEditor.prototype.cancelLinkDialog = function () {
        this.isBlur = false;
        this.linkModule.cancelDialog();
    };
    SfRichTextEditor.prototype.cancelImageDialog = function () {
        this.isBlur = false;
    };
    SfRichTextEditor.prototype.linkDialogClosed = function () {
        this.isBlur = false;
        this.linkModule.linkDialogClosed();
    };
    SfRichTextEditor.prototype.dialogClosed = function (type) {
        this.isBlur = false;
        if (type === 'restore') {
            this.observer.notify(selectionRestore, {});
        }
    };
    SfRichTextEditor.prototype.insertLink = function (args) {
        this.linkModule.insertLink(args);
    };
    SfRichTextEditor.prototype.imageRemoving = function () {
        this.imageModule.removing();
    };
    SfRichTextEditor.prototype.uploadSuccess = function (url, altText) {
        this.imageModule.imageUploadSuccess(url, altText);
    };
    SfRichTextEditor.prototype.imageSelected = function () {
        this.imageModule.imageSelected();
    };
    SfRichTextEditor.prototype.imageUploadComplete = function (base64Str, altText) {
        this.imageModule.imageUploadComplete(base64Str, altText);
    };
    SfRichTextEditor.prototype.imageUploadChange = function (url, isStream) {
        this.imageModule.imageUploadChange(url, isStream);
    };
    SfRichTextEditor.prototype.dropUploadChange = function (url, isStream) {
        this.imageModule.dropUploadChange(url, isStream);
    };
    SfRichTextEditor.prototype.insertImage = function () {
        this.imageModule.insertImageUrl();
    };
    SfRichTextEditor.prototype.imageDialogOpened = function () {
        this.imageModule.dialogOpened();
    };
    SfRichTextEditor.prototype.imageDialogClosed = function () {
        this.isBlur = false;
        this.imageModule.dialogClosed();
    };
    SfRichTextEditor.prototype.insertTable = function (row, column) {
        this.tableModule.customTable(row, column);
    };
    SfRichTextEditor.prototype.applyTableProperties = function (model) {
        this.tableModule.applyTableProperties(model);
    };
    SfRichTextEditor.prototype.createTablePopupOpened = function () {
        this.tableModule.createTablePopupOpened();
    };
    SfRichTextEditor.prototype.pasteContent = function (pasteOption) {
        this.pasteCleanupModule.selectFormatting(pasteOption);
    };
    SfRichTextEditor.prototype.updatePasteContent = function (value) {
        this.observer.notify(afterPasteCleanUp, { text: value });
    };
    SfRichTextEditor.prototype.imageDropInitialized = function (isStream) {
        this.imageModule.imageDropInitialized(isStream);
    };
    SfRichTextEditor.prototype.preventEditable = function () {
        this.inputElement.contentEditable = 'false';
    };
    SfRichTextEditor.prototype.enableEditable = function () {
        this.inputElement.contentEditable = 'true';
    };
    SfRichTextEditor.prototype.removeDroppedImage = function () {
        this.imageModule.removeDroppedImage();
    };
    SfRichTextEditor.prototype.dropUploadSuccess = function (url, altText) {
        this.imageModule.dropUploadSuccess(url, altText);
    };
    SfRichTextEditor.prototype.focusIn = function () {
        if (this.enabled) {
            this.inputElement.focus();
            this.focusHandler({});
        }
    };
    SfRichTextEditor.prototype.insertAlt = function (altText) {
        this.imageModule.insertAlt(altText);
    };
    SfRichTextEditor.prototype.insertSize = function (width, height) {
        this.imageModule.insertSize(width, height);
    };
    SfRichTextEditor.prototype.insertImageLink = function (url, target) {
        this.imageModule.insertLink(url, target);
    };
    SfRichTextEditor.prototype.showLinkDialog = function () {
        this.linkModule.showDialog(true);
    };
    SfRichTextEditor.prototype.showImageDialog = function () {
        this.imageModule.showDialog(true);
    };
    SfRichTextEditor.prototype.showTableDialog = function () {
        this.tableModule.showDialog(true);
    };
    SfRichTextEditor.prototype.destroy = function () {
        this.unWireEvents();
    };
    //#endregion
    //#region Event binding and unbinding function
    SfRichTextEditor.prototype.wireEvents = function () {
        this.element.addEventListener('focusin', this.onFocusHandler, true);
        this.element.addEventListener('focusout', this.onBlurHandler, true);
        this.observer.on(contentChanged, this.contentChanged, this);
        this.observer.on(modelChanged, this.refresh, this);
        this.observer.on(resizeInitialized, this.updateResizeFlag, this);
        this.observer.on(updateTbItemsStatus, this.updateStatus, this);
        if (this.readonly && this.enabled) {
            return;
        }
        this.bindEvents();
    };
    SfRichTextEditor.prototype.bindEvents = function () {
        this.keyboardModule = new KeyboardEvents$1(this.inputElement, {
            keyAction: this.keyDown.bind(this), keyConfigs: __assign({}, this.formatter.keyConfig, this.keyConfig), eventName: 'keydown'
        });
        var formElement = sf.base.closest(this.valueContainer, 'form');
        if (formElement) {
            sf.base.EventHandler.add(formElement, 'reset', this.resetHandler, this);
        }
        sf.base.EventHandler.add(this.inputElement, 'keyup', this.keyUp, this);
        sf.base.EventHandler.add(this.inputElement, 'paste', this.onPaste, this);
        sf.base.EventHandler.add(this.inputElement, sf.base.Browser.touchEndEvent, sf.base.debounce(this.mouseUp, 30), this);
        sf.base.EventHandler.add(this.inputElement, sf.base.Browser.touchStartEvent, this.mouseDownHandler, this);
        this.wireContextEvent();
        this.formatter.editorManager.observer.on('keydown-handler', this.editorKeyDown, this);
        this.element.ownerDocument.defaultView.addEventListener('resize', this.onResizeHandler, true);
        if (this.iframeSettings.enable) {
            sf.base.EventHandler.add(this.inputElement, 'focusin', this.focusHandler, this);
            sf.base.EventHandler.add(this.inputElement, 'focusout', this.blurHandler, this);
            sf.base.EventHandler.add(this.inputElement.ownerDocument, 'scroll', this.contentScrollHandler, this);
            sf.base.EventHandler.add(this.inputElement.ownerDocument, sf.base.Browser.touchStartEvent, this.onIframeMouseDown, this);
        }
        this.wireScrollElementsEvents();
    };
    SfRichTextEditor.prototype.wireContextEvent = function () {
        if (this.quickToolbarSettings.showOnRightClick) {
            sf.base.EventHandler.add(this.inputElement, 'contextmenu', this.contextHandler, this);
            if (sf.base.Browser.isDevice) {
                this.touchModule = new sf.base.Touch(this.inputElement, { tapHold: this.touchHandler.bind(this), tapHoldThreshold: 500 });
            }
        }
    };
    SfRichTextEditor.prototype.wireScrollElementsEvents = function () {
        this.scrollParentElements = sf.popups.getScrollableParent(this.element);
        for (var _i = 0, _a = this.scrollParentElements; _i < _a.length; _i++) {
            var element = _a[_i];
            sf.base.EventHandler.add(element, 'scroll', this.scrollHandler, this);
        }
        if (!this.iframeSettings.enable) {
            sf.base.EventHandler.add(this.getPanel(), 'scroll', this.contentScrollHandler, this);
        }
    };
    SfRichTextEditor.prototype.unWireEvents = function () {
        this.element.removeEventListener('focusin', this.onFocusHandler, true);
        this.element.removeEventListener('focusout', this.onBlurHandler, true);
        this.observer.off(contentChanged, this.contentChanged);
        this.observer.off(resizeInitialized, this.updateResizeFlag);
        this.observer.off(updateTbItemsStatus, this.updateStatus);
        this.unBindEvents();
    };
    SfRichTextEditor.prototype.unBindEvents = function () {
        if (this.keyboardModule) {
            this.keyboardModule.destroy();
        }
        var formElement = sf.base.closest(this.valueContainer, 'form');
        if (formElement) {
            sf.base.EventHandler.remove(formElement, 'reset', this.resetHandler);
        }
        sf.base.EventHandler.remove(this.inputElement, 'keyup', this.keyUp);
        sf.base.EventHandler.remove(this.inputElement, 'paste', this.onPaste);
        sf.base.EventHandler.remove(this.inputElement, sf.base.Browser.touchEndEvent, sf.base.debounce(this.mouseUp, 30));
        sf.base.EventHandler.remove(this.inputElement, sf.base.Browser.touchStartEvent, this.mouseDownHandler);
        this.unWireContextEvent();
        if (this.formatter) {
            this.formatter.editorManager.observer.off('keydown-handler', this.editorKeyDown);
        }
        this.element.ownerDocument.defaultView.removeEventListener('resize', this.onResizeHandler, true);
        if (this.iframeSettings.enable) {
            sf.base.EventHandler.remove(this.inputElement, 'focusin', this.focusHandler);
            sf.base.EventHandler.remove(this.inputElement, 'focusout', this.blurHandler);
            sf.base.EventHandler.remove(this.inputElement.ownerDocument, 'scroll', this.contentScrollHandler);
            sf.base.EventHandler.remove(this.inputElement.ownerDocument, sf.base.Browser.touchStartEvent, this.onIframeMouseDown);
        }
        this.unWireScrollElementsEvents();
    };
    SfRichTextEditor.prototype.unWireContextEvent = function () {
        sf.base.EventHandler.remove(this.inputElement, 'contextmenu', this.contextHandler);
        if (sf.base.Browser.isDevice && this.touchModule) {
            this.touchModule.destroy();
        }
    };
    SfRichTextEditor.prototype.unWireScrollElementsEvents = function () {
        this.scrollParentElements = sf.popups.getScrollableParent(this.element);
        for (var _i = 0, _a = this.scrollParentElements; _i < _a.length; _i++) {
            var element = _a[_i];
            sf.base.EventHandler.remove(element, 'scroll', this.scrollHandler);
        }
        if (!this.iframeSettings.enable) {
            sf.base.EventHandler.remove(this.getPanel(), 'scroll', this.contentScrollHandler);
        }
    };
    //#endregion
    //#region Event handler methods
    SfRichTextEditor.prototype.focusHandler = function (e) {
        if ((!this.isRTE || this.isFocusOut)) {
            this.isRTE = this.isFocusOut ? false : true;
            this.isFocusOut = false;
            sf.base.addClass([this.element], [CLS_FOCUS]);
            if (this.editorMode === 'HTML') {
                this.cloneValue = (this.inputElement.innerHTML === getDefaultValue(this)) ? null : this.enableHtmlEncode ?
                    this.encode(decode(this.inputElement.innerHTML)) : this.inputElement.innerHTML;
            }
            else {
                this.cloneValue = this.inputElement.value === '' ? null :
                    this.inputElement.value;
            }
            var active = document.activeElement;
            if (active === this.element || active === this.getToolbarElement() || active === this.getEditPanel()
                || ((this.iframeSettings.enable && active === this.getPanel()) &&
                    e.target && !e.target.classList.contains('e-img-inner')
                    && (e.target && e.target.parentElement
                        && !e.target.parentElement.classList.contains('e-img-wrap')))
                || sf.base.closest(active, '.e-rte-toolbar') === this.getToolbarElement()) {
                this.getEditPanel().focus();
                if (!sf.base.isNullOrUndefined(this.getToolbarElement())) {
                    this.getToolbarElement().setAttribute('tabindex', '-1');
                    var items = this.getToolbarElement().querySelectorAll('[tabindex="0"]');
                    for (var i = 0; i < items.length; i++) {
                        items[i].setAttribute('tabindex', '-1');
                    }
                }
            }
            this.defaultResize(e, false);
            var args = { isInteracted: Object.keys(e).length === 0 ? false : true };
            if (this.focusEnabled) {
                this.dotNetRef.invokeMethodAsync('FocusEvent', args);
            }
            if (!sf.base.isNullOrUndefined(this.saveInterval) && this.saveInterval > 0 && !this.autoSaveOnIdle) {
                this.timeInterval = setInterval(this.updateValueOnIdle.bind(this), this.saveInterval);
            }
            sf.base.EventHandler.add(document, 'mousedown', this.onDocumentClick, this);
        }
        if (!sf.base.isNullOrUndefined(this.getToolbarElement())) {
            var toolbarItem = this.getToolbarElement().querySelectorAll('input,select,button,a,[tabindex]');
            for (var i = 0; i < toolbarItem.length; i++) {
                if ((!toolbarItem[i].classList.contains('e-rte-dropdown-btn') &&
                    !toolbarItem[i].classList.contains('e-insert-table-btn')) &&
                    (!toolbarItem[i].hasAttribute('tabindex') ||
                        toolbarItem[i].getAttribute('tabindex') !== '-1')) {
                    toolbarItem[i].setAttribute('tabindex', '-1');
                }
            }
        }
    };
    SfRichTextEditor.prototype.blurHandler = function (e) {
        var trg = e.relatedTarget;
        if (trg) {
            var rteElement = sf.base.closest(trg, '.' + CLS_RTE);
            if (rteElement && rteElement === this.element) {
                this.isBlur = false;
                if (trg === this.getToolbarElement()) {
                    trg.setAttribute('tabindex', '-1');
                }
            }
            else if (sf.base.closest(trg, '[aria-owns="' + this.element.id + '"]') || !sf.base.isNullOrUndefined(sf.base.closest(trg, '.' + CLS_RTE_ELEMENTS))) {
                this.isBlur = false;
            }
            else {
                this.isBlur = true;
                trg = null;
            }
        }
        else if (!this.isIframeRteElement) {
            this.isBlur = true;
            this.isIframeRteElement = true;
        }
        if (this.isBlur && sf.base.isNullOrUndefined(trg)) {
            sf.base.removeClass([this.element], [CLS_FOCUS]);
            this.observer.notify(focusChange, {});
            this.value = this.getUpdatedValue();
            this.updateValueContainer(this.value);
            this.observer.notify(toolbarRefresh, { args: e, documentNode: document });
            this.invokeChangeEvent();
            this.isFocusOut = true;
            this.isBlur = false;
            if (this.enableXhtml) {
                this.valueContainer.value = this.getXhtmlString(this.valueContainer.value);
            }
            dispatchEvent(this.valueContainer, 'focusout');
            this.defaultResize(e, true);
            var args = { isInteracted: Object.keys(e).length === 0 ? false : true };
            if (this.blurEnabled) {
                this.dotNetRef.invokeMethodAsync('BlurEvent', args);
            }
            if (!sf.base.isNullOrUndefined(this.timeInterval)) {
                clearInterval(this.timeInterval);
                this.timeInterval = null;
            }
            sf.base.EventHandler.remove(document, 'mousedown', this.onDocumentClick);
        }
        else {
            this.isRTE = true;
        }
    };
    SfRichTextEditor.prototype.resizeHandler = function () {
        var isExpand = false;
        if (!document.body.contains(this.element)) {
            return;
        }
        if (this.toolbarSettings.enable && !this.inlineMode.enable) {
            this.dotNetRef.invokeMethodAsync('RefreshToolbarOverflow');
            var tbElement = this.element.querySelector('.e-rte-toolbar');
            isExpand = tbElement && tbElement.classList.contains(CLS_EXPAND_OPEN);
        }
        this.setContentHeight('windowResize', isExpand);
        this.observer.notify(windowResize, null);
    };
    SfRichTextEditor.prototype.touchHandler = function (e) {
        this.notifyMouseUp(e.originalEvent);
        this.triggerEditArea(e.originalEvent);
    };
    SfRichTextEditor.prototype.resetHandler = function () {
        var defaultValue = this.valueContainer.defaultValue.trim().replace(/<!--!-->/gi, '');
        this.value = (defaultValue === '' ? null : this.defaultResetValue);
        this.setPanelValue(this.value, true);
    };
    SfRichTextEditor.prototype.contextHandler = function (e) {
        var closestElem = sf.base.closest(e.target, 'a, table, img');
        if (this.inlineMode.onSelection === false || (!sf.base.isNullOrUndefined(closestElem) && this.inputElement.contains(closestElem)
            && (closestElem.tagName === 'IMG' || closestElem.tagName === 'TABLE' || closestElem.tagName === 'A'))) {
            e.preventDefault();
        }
    };
    SfRichTextEditor.prototype.scrollHandler = function (e) {
        this.observer.notify(scroll, { args: e });
    };
    SfRichTextEditor.prototype.contentScrollHandler = function (e) {
        this.observer.notify(contentscroll, { args: e });
    };
    SfRichTextEditor.prototype.mouseUp = function (e) {
        if (this.quickToolbarSettings.showOnRightClick && sf.base.Browser.isDevice) {
            var target = e.target;
            var closestTable = sf.base.closest(target, 'table');
            if (target && target.nodeName === 'A' || target.nodeName === 'IMG' || (target.nodeName === 'TD' || target.nodeName === 'TH' ||
                target.nodeName === 'TABLE' || (closestTable && this.getEditPanel().contains(closestTable)))) {
                return;
            }
        }
        this.notifyMouseUp(e);
    };
    SfRichTextEditor.prototype.mouseDownHandler = function (e) {
        var touch = (e.touches ? e.changedTouches[0] : e);
        sf.base.addClass([this.element], [CLS_FOCUS]);
        this.defaultResize(e, false);
        this.observer.notify(mouseDown, { args: e });
        this.clickPoints = { clientX: touch.clientX, clientY: touch.clientY };
    };
    SfRichTextEditor.prototype.onIframeMouseDown = function (e) {
        this.isBlur = false;
        this.observer.notify(iframeMouseDown, e);
    };
    SfRichTextEditor.prototype.keyDown = function (e) {
        this.observer.notify(keyDown, { member: 'keydown', args: e });
        this.restrict(e);
        if (this.iframeSettings.enable && !this.enableTabKey && (e.which === 9 && e.code === 'Tab')
            && e.target.nodeName === 'BODY') {
            this.isIframeRteElement = false;
        }
        if (this.editorMode === 'HTML' && ((e.which === 8 && e.code === 'Backspace') || (e.which === 46 && e.code === 'Delete'))) {
            var range = this.getRange();
            var startNode = range.startContainer.nodeName === '#text' ? range.startContainer.parentElement :
                range.startContainer;
            if (sf.base.closest(startNode, 'pre') &&
                (e.which === 8 && range.startContainer.textContent.charCodeAt(range.startOffset - 1) === 8203) ||
                (e.which === 46 && range.startContainer.textContent.charCodeAt(range.startOffset) === 8203)) {
                var regEx = new RegExp(String.fromCharCode(8203), 'g');
                var pointer = e.which === 8 ? range.startOffset - 1 : range.startOffset;
                range.startContainer.textContent = range.startContainer.textContent.replace(regEx, '');
                this.formatter.editorManager.nodeSelection.setCursorPoint(this.getDocument(), range.startContainer, pointer);
            }
            else if ((e.code === 'Backspace' && e.which === 8) &&
                range.startContainer.textContent.charCodeAt(0) === 8203 && range.collapsed) {
                var parentEle = range.startContainer.parentElement;
                var index = void 0;
                var i = void 0;
                for (i = 0; i < parentEle.childNodes.length; i++) {
                    if (parentEle.childNodes[i] === range.startContainer) {
                        index = i;
                    }
                }
                var bool = true;
                var removeNodeArray = [];
                for (i = index; i >= 0; i--) {
                    if (parentEle.childNodes[i].nodeType === 3 && parentEle.childNodes[i].textContent.charCodeAt(0) === 8203 && bool) {
                        removeNodeArray.push(i);
                    }
                    else {
                        bool = false;
                    }
                }
                if (removeNodeArray.length > 0) {
                    for (i = removeNodeArray.length - 1; i > 0; i--) {
                        parentEle.childNodes[removeNodeArray[i]].textContent = '';
                    }
                }
                this.formatter.editorManager.nodeSelection.setCursorPoint(this.getDocument(), range.startContainer, range.startOffset);
            }
        }
        if (this.formatter.getUndoRedoStack().length === 0) {
            this.formatter.saveData();
        }
        if (e.action !== 'insert-link' &&
            (e.action && e.action !== 'paste' && e.action !== 'space'
                || e.which === 9 || (e.code === 'Backspace' && e.which === 8))) {
            this.formatter.process(this, null, e);
            switch (e.action) {
                case 'toolbar-focus':
                    if (this.toolbarSettings.enable) {
                        var selector = '.e-toolbar-item[aria-disabled="false"][title] [tabindex]';
                        this.getToolbar().querySelector(selector).focus();
                    }
                    break;
                case 'escape':
                    this.getEditPanel().focus();
                    break;
            }
        }
        if (!sf.base.isNullOrUndefined(this.placeholder)) {
            if ((!sf.base.isNullOrUndefined(this.placeHolderContainer)) && (this.inputElement.textContent.length !== 1)) {
                this.placeHolderContainer.style.display = 'none';
            }
            else {
                this.setPlaceHolder();
            }
        }
        this.autoResize();
    };
    SfRichTextEditor.prototype.editorKeyDown = function (e) {
        switch (e.event.action) {
            case 'copy':
                this.onCopy();
                break;
            case 'cut':
                this.onCut();
                break;
        }
        if (e.callBack && (e.event.action === 'copy' || e.event.action === 'cut' || e.event.action === 'delete')) {
            e.callBack({
                requestType: e.event.action,
                editorMode: 'HTML',
                event: e.event
            });
        }
    };
    SfRichTextEditor.prototype.keyUp = function (e) {
        if (this.editorMode === 'HTML') {
            switch (e.which) {
                case 46:
                case 8:
                    var childNodes = this.getEditPanel().childNodes;
                    if ((childNodes.length === 0) ||
                        (childNodes.length === 1 && ((childNodes[0].tagName === 'BR') ||
                            ((childNodes[0].tagName === 'P' || childNodes[0].tagName === 'DIV') &&
                                (childNodes[0].childNodes.length === 0 || childNodes[0].textContent === ''))))) {
                        var node = this.getEditPanel();
                        node.innerHTML = getDefaultValue(this);
                        this.formatter.editorManager.nodeSelection.setCursorPoint(this.getDocument(), node.childNodes[0], 0);
                    }
                    break;
            }
        }
        this.observer.notify(keyUp, { member: 'keyup', args: e });
        if (e.code === 'KeyX' && e.which === 88 && e.keyCode === 88 && e.ctrlKey && (this.inputElement.innerHTML === '' ||
            this.inputElement.innerHTML === '<br>')) {
            this.inputElement.innerHTML = getEditValue(getDefaultValue(this), this);
        }
        var allowedKeys = e.which === 32 || e.which === 13 || e.which === 8 || e.which === 46;
        if (((e.key !== 'shift' && !e.ctrlKey) && e.key && e.key.length === 1 || allowedKeys) || (this.editorMode === 'Markdown'
            && ((e.key !== 'shift' && !e.ctrlKey) && e.key && e.key.length === 1 || allowedKeys)) && !this.inlineMode.enable) {
            this.formatter.onKeyHandler(this, e);
        }
        if (this.inputElement && this.inputElement.textContent.length !== 0
            || this.element.querySelectorAll('.e-toolbar-item.e-active').length > 0) {
            this.observer.notify(toolbarRefresh, { args: e });
        }
        if (!sf.base.isNullOrUndefined(this.placeholder)) {
            if (!(e.key === 'Enter' && e.keyCode === 13) && (this.inputElement.innerHTML === '<p><br></p>' || this.inputElement.innerHTML === '<div><br></div>' ||
                this.inputElement.innerHTML === '<br>')) {
                this.setPlaceHolder();
            }
        }
    };
    SfRichTextEditor.prototype.onCut = function () {
        this.getDocument().execCommand('cut', false, null);
    };
    SfRichTextEditor.prototype.onCopy = function () {
        this.getDocument().execCommand('copy', false, null);
    };
    SfRichTextEditor.prototype.onPaste = function (e) {
        if (!sf.base.isNullOrUndefined(sf.base.select('.e-rte-img-dialog', this.element))) {
            return;
        }
        var currentLength = this.getText().length;
        var selectionLength = this.getSelection().length;
        var pastedContentLength = (sf.base.isNullOrUndefined(e) || sf.base.isNullOrUndefined(e.clipboardData))
            ? 0 : e.clipboardData.getData('text/plain').length;
        var totalLength = (currentLength - selectionLength) + pastedContentLength;
        if (this.editorMode === 'Markdown') {
            if (!(this.maxLength === -1 || totalLength < this.maxLength)) {
                e.preventDefault();
            }
            return;
        }
        if (this.inputElement.contentEditable === 'true' &&
            (this.maxLength === -1 || totalLength < this.maxLength)) {
            this.observer.notify(pasteClean, { args: e });
        }
        else {
            e.preventDefault();
        }
    };
    SfRichTextEditor.prototype.onDocumentClick = function (e) {
        var target = e.target;
        var rteElement = sf.base.closest(target, '.' + CLS_RTE);
        if (!this.element.contains(e.target) && document !== e.target && rteElement !== this.element &&
            !sf.base.closest(target, '[aria-owns="' + this.element.id + '"]')) {
            this.isBlur = true;
            this.isRTE = false;
        }
        this.observer.notify(docClick, { args: e });
    };
    SfRichTextEditor.prototype.propertyChangeHandler = function (newProps) {
        var oldProps = {};
        for (var _i = 0, _a = Object.keys(newProps); _i < _a.length; _i++) {
            var prop = _a[_i];
            /* eslint-disable */
            oldProps[prop] = this[prop];
            /* eslint-enable */
        }
        var oldValue = this.value;
        this.updateContext(newProps);
        for (var _b = 0, _c = Object.keys(newProps); _b < _c.length; _b++) {
            var prop = _c[_b];
            switch (prop) {
                case 'enableXhtml':
                case 'enableHtmlSanitizer':
                case 'value':
                case 'enterKey':
                    if (prop === 'value' && oldValue === this.value) {
                        break;
                    }
                    if (prop === 'enterKey' && (this.value === '<p><br/></p>' ||
                        this.value === '<div><br/></div>' || this.value === '<br/>' || this.value === '<p><br></p>' ||
                        this.value === '<div><br></div>' || this.value === '<br>')) {
                        this.value = getDefaultValue(this);
                    }
                    this.setPanelValue(this.value);
                    if (this.enableXhtml) {
                        this.value = this.getXhtml();
                    }
                    break;
                case 'height':
                    this.setHeight(this.height);
                    this.setContentHeight();
                    this.autoResize();
                    break;
                case 'width':
                    this.setWidth(this.width);
                    if (this.toolbarSettings.enable) {
                        this.dotNetRef.invokeMethodAsync('RefreshToolbarOverflow');
                        //this.toolbarModule.refreshToolbarOverflow();
                        this.resizeHandler();
                    }
                    break;
                case 'readonly':
                    this.setReadOnly(false);
                    break;
                case 'enabled':
                    this.setEnable();
                    break;
                case 'placeholder':
                    this.placeholder = this.placeholder;
                    this.setPlaceHolder();
                    break;
                case 'showCharCount':
                    if (this.showCharCount && this.countModule) {
                        this.countModule.renderCount();
                    }
                    else if (this.showCharCount === false && this.countModule) {
                        this.countModule.destroy();
                    }
                    break;
                case 'maxLength':
                    if (this.showCharCount) {
                        this.countModule.refresh();
                    }
                    break;
                case 'enableHtmlEncode':
                    this.updateValueData();
                    this.updatePanelValue();
                    this.setPlaceHolder();
                    if (this.showCharCount) {
                        this.countModule.refresh();
                    }
                    break;
                case 'undoRedoSteps':
                case 'undoRedoTimer':
                    this.formatter.editorManager.observer.notify('model_changed', { newProp: newProps });
                    break;
                case 'adapter':
                    var editElement = this.getEditPanel();
                    var option = { undoRedoSteps: this.undoRedoSteps, undoRedoTimer: this.undoRedoTimer };
                    if (this.editorMode === 'Markdown') {
                        this.formatter = new MarkdownFormatter(sf.base.extend({}, this.adapter, {
                            element: editElement,
                            options: option
                        }));
                    }
                    break;
                case 'iframeSettings':
                    var frameSetting = oldProps[prop];
                    if (frameSetting.resources) {
                        var iframe = this.getDocument();
                        var header = iframe.querySelector('head');
                        var files = void 0;
                        if (frameSetting.resources.scripts) {
                            files = header.querySelectorAll('.' + CLS_SCRIPT_SHEET);
                            this.removeSheets(files);
                        }
                        if (frameSetting.resources.styles) {
                            files = header.querySelectorAll('.' + CLS_STYLE_SHEET);
                            this.removeSheets(files);
                        }
                    }
                    this.setIframeSettings();
                    break;
                case 'quickToolbarSettings':
                    this.quickToolbarSettings.showOnRightClick ? this.wireContextEvent() : this.unWireContextEvent();
                    break;
                default:
                    //this.observer.notify('model-changed', { newProp: newProp, oldProp: oldProp });
                    break;
            }
        }
    };
    return SfRichTextEditor;
}());

/**
 * Interop handler
 */
// eslint-disable-next-line
var RichTextEditor = {
    initialize: function (element, options, dotnetRef) {
        if (element) {
            new SfRichTextEditor(element, options, dotnetRef);
            element.blazor__instance.initialize();
        }
    },
    updateProperties: function (element, options) {
        if (element) {
            element.blazor__instance.updateContext(options);
        }
    },
    setPanelValue: function (element, value) {
        if (element) {
            element.blazor__instance.setPanelValue(value);
        }
    },
    toolbarItemClick: function (element, args, id, targetType) {
        if (element) {
            element.blazor__instance.toolbarItemClick(args, id, targetType);
        }
    },
    toolbarClick: function (element, id) {
        if (element) {
            element.blazor__instance.toolbarClick(id);
        }
    },
    listDropDownButtonClick: function (element, args) {
        if (element) {
            element.blazor__instance.onListDropDownButtonClick(args);
        }
    },
    dropDownBeforeOpen: function (element, args) {
        if (element) {
            element.blazor__instance.dropDownBeforeOpen(args);
        }
    },
    dropDownClose: function (element, args) {
        if (element) {
            element.blazor__instance.dropDownClose(args);
        }
    },
    dropDownButtonItemSelect: function (element, args) {
        if (element) {
            element.blazor__instance.dropDownSelect(args);
        }
    },
    colorDropDownBeforeOpen: function (element) {
        if (element) {
            element.blazor__instance.colorDropDownBeforeOpen();
        }
    },
    colorIconSelected: function (element, args, value, container) {
        if (element) {
            element.blazor__instance.colorIconSelected(args, value, container);
        }
    },
    colorPickerChanged: function (element, args, value, container) {
        if (element) {
            element.blazor__instance.colorChanged(args, value, container);
        }
    },
    cancelLinkDialog: function (element) {
        if (element) {
            element.blazor__instance.cancelLinkDialog();
        }
    },
    cancelImageDialog: function (element) {
        if (element) {
            element.blazor__instance.cancelImageDialog();
        }
    },
    linkDialogClosed: function (element) {
        if (element) {
            element.blazor__instance.linkDialogClosed();
        }
    },
    dialogClosed: function (element, type) {
        if (element) {
            element.blazor__instance.dialogClosed(type);
        }
    },
    insertLink: function (element, args) {
        if (element) {
            element.blazor__instance.insertLink(args);
        }
    },
    countCalculate: function (element) {
        if (element) {
            element.blazor__instance.countCalculate();
        }
    },
    imageRemoving: function (element) {
        if (element) {
            element.blazor__instance.imageRemoving();
        }
    },
    uploadSuccess: function (element, url, altText) {
        if (element) {
            element.blazor__instance.uploadSuccess(url, altText);
        }
    },
    imageSelected: function (element) {
        if (element) {
            element.blazor__instance.imageSelected();
        }
    },
    imageUploadComplete: function (element, base64Str, altText) {
        if (element) {
            element.blazor__instance.imageUploadComplete(base64Str, altText);
        }
    },
    imageUploadChange: function (element, url, isStream) {
        if (element) {
            element.blazor__instance.imageUploadChange(url, isStream);
        }
    },
    dropUploadChange: function (element, url, isStream) {
        if (element) {
            element.blazor__instance.dropUploadChange(url, isStream);
        }
    },
    insertImage: function (element) {
        if (element) {
            element.blazor__instance.insertImage();
        }
    },
    imageDialogOpened: function (element) {
        if (element) {
            element.blazor__instance.imageDialogOpened();
        }
    },
    imageDialogClosed: function (element) {
        if (element) {
            element.blazor__instance.imageDialogClosed();
        }
    },
    propertyChangeHandler: function (element, option) {
        if (element) {
            element.blazor__instance.propertyChangeHandler(option);
        }
    },
    insertTable: function (element, row, column) {
        if (element) {
            element.blazor__instance.insertTable(row, column);
        }
    },
    applyTableProperties: function (element, model) {
        if (element) {
            element.blazor__instance.applyTableProperties(model);
        }
    },
    createTablePopupOpened: function (element) {
        if (element) {
            element.blazor__instance.createTablePopupOpened();
        }
    },
    pasteContent: function (element, pasteOption) {
        if (element) {
            element.blazor__instance.pasteContent(pasteOption);
        }
    },
    updatePasteContent: function (element, value) {
        if (element) {
            element.blazor__instance.updatePasteContent(value);
        }
    },
    imageDropInitialized: function (element, isStream) {
        if (element) {
            element.blazor__instance.imageDropInitialized(isStream);
        }
    },
    preventEditable: function (element) {
        if (element) {
            element.blazor__instance.preventEditable();
        }
    },
    enableEditable: function (element) {
        if (element) {
            element.blazor__instance.enableEditable();
        }
    },
    removeDroppedImage: function (element) {
        if (element) {
            element.blazor__instance.removeDroppedImage();
        }
    },
    dropUploadSuccess: function (element, url, altText) {
        if (element) {
            element.blazor__instance.dropUploadSuccess(url, altText);
        }
    },
    executeCommand: function (element, commandName, value, option) {
        if (element) {
            element.blazor__instance.executeCommand(commandName, value, option);
        }
    },
    getCharCount: function (element) {
        return element && element.blazor__instance.getCharCount();
    },
    focusIn: function (element) {
        return element && element.blazor__instance.focusIn();
    },
    focusOut: function (element) {
        return element && element.blazor__instance.focusOut();
    },
    getContent: function (element) {
        return element && element.blazor__instance.getContent();
    },
    getHtml: function (element) {
        return element && element.blazor__instance.getHtml();
    },
    getSelectedHtml: function (element) {
        return element && element.blazor__instance.getSelectedHtml();
    },
    getSelection: function (element) {
        return element && element.blazor__instance.getSelection();
    },
    getText: function (element) {
        return element && element.blazor__instance.getText();
    },
    print: function (element) {
        return element && element.blazor__instance.print();
    },
    refreshUI: function (element) {
        return element && element.blazor__instance.refreshUI();
    },
    sanitizeHtml: function (element, value) {
        return element && element.blazor__instance.sanitizeHtml(value);
    },
    selectAll: function (element) {
        return element && element.blazor__instance.selectAll();
    },
    selectRange: function (element, range) {
        return element && element.blazor__instance.selectRange(range);
    },
    showFullScreen: function (element) {
        return element && element.blazor__instance.showFullScreen();
    },
    showSourceCode: function (element) {
        return element && element.blazor__instance.showSourceCode();
    },
    insertAlt: function (element, altText) {
        return element && element.blazor__instance.insertAlt(altText);
    },
    insertSize: function (element, width, height) {
        return element && element.blazor__instance.insertSize(width, height);
    },
    insertImageLink: function (element, url, target) {
        return element && element.blazor__instance.insertImageLink(url, target);
    },
    updateContentHeight: function (element) {
        return element && element.blazor__instance.setContentHeight();
    },
    saveSelection: function (element) {
        if (element) {
            element.blazor__instance.saveSelection();
        }
    },
    restoreSelection: function (element) {
        if (element) {
            element.blazor__instance.restoreSelection();
        }
    },
    getXhtml: function (element) {
        return element.blazor__instance.getXhtml();
    },
    showLinkDialog: function (element) {
        if (element) {
            element.blazor__instance.showLinkDialog();
        }
    },
    showImageDialog: function (element) {
        if (element) {
            element.blazor__instance.showImageDialog();
        }
    },
    showTableDialog: function (element) {
        if (element) {
            element.blazor__instance.showTableDialog();
        }
    },
    destroy: function (element) {
        if (element) {
            element.blazor__instance.destroy();
        }
    }
};

return RichTextEditor;

}());

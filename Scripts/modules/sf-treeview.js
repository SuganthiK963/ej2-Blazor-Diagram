window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.TreeView = (function () {
'use strict';

var LISTITEM = 'e-list-item';
var LISTWRAP = 'e-text-wrap';
var PARENTITEM = 'e-list-parent';
var HOVER = 'e-hover';
var COLLAPSIBLE = 'e-icon-collapsible';
var EXPANDABLE = 'e-icon-expandable';
var MOUSEOVER = 'mouseover';
var CLICK = 'Click';
var DBLCLICK = 'DoubleClick';
var FOCUSING = 'focus';
var BLUR = 'blur';
var MOUSEDOWN = 'mousedown';
var MOUSEUP = 'mouseup';
var MOUSEOUT = 'mouseout';
var EXPANDONNONE = 'None';
var EXPANDONAUTO = 'DoubleClick';
var ICON = 'e-icons';
var CHECK = 'e-check';
var BLOCK = 'block';
var HIDDEN = 'hidden';
var NONE = 'none';
var EMPTY = '';
var DISPLAYNONE = 'e-display-none';
var ACTIVE = 'e-active';
var CONTROL = 'e-control';
var ROOT = 'e-treeview';
var FOCUS = 'e-node-focus';
var PROCESS = 'e-process';
var CHECKBOXFRAME = 'e-frame';
var CHECKBOXWRAP = 'e-checkbox-wrapper';
var CHECKBOXRIPPLE = 'e-ripple-container';
var EDITING = 'e-editing';
var INPUT = 'e-input';
var INPUTGROUP = 'e-input-group';
var DISABLED = 'e-disabled';
var TEXTWRAP = 'e-text-content';
var FULLROW = 'e-fullrow';
var DRAGITEM = 'e-drag-item';
var DROPPABLE = 'e-droppable';
var DRAGGING = 'e-dragging';
var SIBLING = 'e-sibling';
var DROPIN = 'e-drop-in';
var DROPNEXT = 'e-drop-next';
var DROPOUT = 'e-drop-out';
var NODROP = 'e-no-drop';
var RTL = 'e-rtl';
var DROPCOUNT = 'e-drop-count';
var ITEM_ANIMATION_ACTIVE = 'e-animation-active';
var ALLOWDRAGANDDROP = 'allowDragAndDrop';
var ALLOWEDITING = 'allowEditing';
var SHOWCHECKBOX = 'showCheckBox';
var ALLOWTEXTWRAP = 'allowTextWrap';
var SETDISABLED = 'disabled';
var DRAGAREA = 'dragArea';
var CSSCLASS = 'cssClass';
var ANIMATION = 'animation';
var EXPANDONTYPE = 'expandOnType';
var ENABLERTL = 'enableRtl';
var DISABLE = 'e-disable';
var RIPPLE = 'e-ripple';
var RIPPLEELMENT = 'e-ripple-element';
var FULLROWSELECT = 'fullRowSelect';
var FULLROWWRAP = 'e-fullrow-wrap';
var INTERACTION = 'e-interaction';
var SfTreeView = /** @class */ (function () {
    function SfTreeView(element, options, dotnetRef) {
        this.isHelperElement = true;
        this.mouseDownStatus = false;
        this.mouseUpStatus = false;
        this.preventExpand = false;
        this.keyBoardAction = false;
        this.isNodeRendered = false;
        this.isEdited = false;
        this.element = element;
        // eslint-disable-next-line
        this.element.blazor__instance = this;
        this.dotNetRef = dotnetRef;
        this.options = options;
    }
    SfTreeView.prototype.render = function () {
        this.dragStartAction = false;
        this.listBaseOption = {
            expandCollapse: true,
            showIcon: true,
            expandIconClass: EXPANDABLE,
            expandIconPosition: 'Left'
        };
        this.keyConfigs = {
            escape: 'escape',
            end: 'end',
            enter: 'enter',
            f2: 'f2',
            home: 'home',
            moveDown: 'downarrow',
            moveLeft: 'leftarrow',
            moveRight: 'rightarrow',
            moveUp: 'uparrow',
            ctrlDown: 'ctrl+downarrow',
            ctrlUp: 'ctrl+uparrow',
            ctrlEnter: 'ctrl+enter',
            ctrlHome: 'ctrl+home',
            ctrlEnd: 'ctrl+end',
            ctrlA: 'ctrl+A',
            shiftDown: 'shift+downarrow',
            shiftUp: 'shift+uparrow',
            shiftEnter: 'shift+enter',
            shiftHome: 'shift+home',
            shiftEnd: 'shift+end',
            csDown: 'ctrl+shift+downarrow',
            csUp: 'ctrl+shift+uparrow',
            csEnter: 'ctrl+shift+enter',
            csHome: 'ctrl+shift+home',
            csEnd: 'ctrl+shift+end',
            space: 'space'
        };
        this.animationObj = new sf.base.Animation({});
        this.setDisabledMode(this.options.disabled);
        this.setMultiSelect(this.options.allowMultiSelection);
        if (this.options.hasTemplate) {
            this.element.classList.add(INTERACTION);
        }
    };
    SfTreeView.prototype.setDisabledMode = function (isEnabled) {
        this.setDragAndDrop(this.options.allowDragAndDrop);
        this.wireEditingEvents(this.options.allowEditing);
        this.checkAllDisabled(isEnabled);
        if (isEnabled) {
            this.unWireEvents();
        }
        else {
            this.wireEvents();
        }
    };
    SfTreeView.prototype.checkAllDisabled = function (isDisabled) {
        isDisabled ? this.element.classList.add(DISABLED) : this.element.classList.remove(DISABLED);
    };
    SfTreeView.prototype.setEnableRtl = function () {
        (this.options.enableRtl ? sf.base.addClass : sf.base.removeClass)([this.element], RTL);
    };
    SfTreeView.prototype.updateWrap = function (ulEle) {
        if (!this.options.fullRowSelect) {
            return;
        }
        var liEle = sf.base.selectAll('li', ulEle ? ulEle : this.element);
        var length = liEle.length;
        for (var i = 0; i < length; i++) {
            var element = sf.base.select('.' + FULLROW, liEle[i]);
            if (element && element.nextElementSibling) {
                element.style.height = this.options.allowTextWrap ? element.nextElementSibling.offsetHeight + 'px' : '';
            }
        }
    };
    SfTreeView.prototype.setTextWrap = function () {
        if (this.options.allowTextWrap && !this.element.classList.contains(LISTWRAP)) {
            sf.base.addClass([this.element], LISTWRAP);
        }
        else if (!this.options.allowTextWrap && this.element.classList.contains(LISTWRAP)) {
            sf.base.removeClass([this.element], LISTWRAP);
        }
        this.updateWrap();
    };
    SfTreeView.prototype.mouseDownHandler = function (e) {
        this.mouseDownStatus = true;
        if (e.shiftKey || e.ctrlKey) {
            e.preventDefault();
        }
        if (e.ctrlKey && this.options.allowMultiSelection) {
            sf.base.EventHandler.add(this.element, 'contextmenu', this.preventContextMenu, this);
        }
    };
    SfTreeView.prototype.mouseupHandler = function () {
        this.mouseUpStatus = true;
    };
    SfTreeView.prototype.onMouseLeave = function (e) {
        this.removeHover();
    };
    SfTreeView.prototype.unWireEvents = function () {
        this.wireClickEvent(false);
        this.wireExpandOnEvent(false);
        sf.base.EventHandler.remove(this.element, MOUSEDOWN, this.mouseDownHandler);
        sf.base.EventHandler.remove(this.element, FOCUSING, this.focusIn);
        sf.base.EventHandler.remove(this.element, BLUR, this.focusOut);
        sf.base.EventHandler.remove(this.element, MOUSEOVER, this.onMouseOver);
        sf.base.EventHandler.remove(this.element, MOUSEOUT, this.onMouseLeave);
        if (!this.options.disabled && this.keyboardModule) {
            this.keyboardModule.destroy();
        }
    };
    SfTreeView.prototype.keyboardActionHandler = function (e) {
        this.keyAction = e;
        var target = e.target;
        var focusedNode = this.getFocusedNode();
        if (target && target.classList.contains(INPUT)) {
            var inpEle = target;
            if (e.action === 'enter') {
                inpEle.blur();
                this.element.focus();
                sf.base.addClass([focusedNode], HOVER);
            }
            else if (e.action === 'escape') {
                inpEle.value = this.oldText;
                inpEle.blur();
                this.element.focus();
                sf.base.addClass([focusedNode], HOVER);
            }
            return;
        }
        e.preventDefault();
        var eventArgs = {
            cancel: false,
            event: e
        };
        var id = focusedNode.getAttribute('data-uid');
        this.dotNetRef.invokeMethodAsync('TriggerKeyboardEvent', eventArgs, id, e.action, e.key);
    };
    SfTreeView.prototype.setMultiSelect = function (isEnabled) {
        this.options.allowMultiSelection = isEnabled;
        if (isEnabled) {
            this.element.setAttribute('aria-multiselectable', 'true');
        }
        else {
            this.element.removeAttribute('aria-multiselectable');
        }
    };
    SfTreeView.prototype.setCssClass = function (cssClass) {
        if (this.options.cssClass) {
            sf.base.removeClass([this.element], this.options.cssClass.split(' '));
        }
        if (cssClass) {
            sf.base.addClass([this.element], cssClass.split(' '));
        }
        this.options.cssClass = cssClass;
    };
    SfTreeView.prototype.wireEditingEvents = function (toBind) {
        var _this_1 = this;
        if (toBind && !this.options.disabled) {
            // eslint-disable-next-line
            var proxy_1 = this;
            this.touchEditObj = new sf.base.Touch(this.element, {
                tap: function (e) {
                    if (_this_1.isDoubleTapped(e) && e.tapCount === 2) {
                        e.originalEvent.preventDefault();
                        proxy_1.editingHandler(e.originalEvent);
                    }
                }
            });
        }
        else if (this.touchEditObj) {
            this.touchEditObj.destroy();
        }
    };
    SfTreeView.prototype.setDragAndDrop = function (toBind) {
        if (toBind && !this.options.disabled) {
            this.initializeDrag();
        }
        else {
            this.destroyDrag();
        }
    };
    SfTreeView.prototype.setDragArea = function (dragArea) {
        if (this.options.allowDragAndDrop) {
            this.dragObj.dragArea = dragArea;
        }
    };
    SfTreeView.prototype.destroyDrag = function () {
        if (this.dragObj && this.dropObj) {
            this.dragObj.destroy();
            this.dropObj.destroy();
        }
    };
    SfTreeView.prototype.initializeDrag = function () {
        var _this_1 = this;
        var virtualEle;
        this.dragObj = new sf.base.Draggable(this.element, {
            enableTailMode: true, enableAutoScroll: true,
            dragArea: this.options.dropArea,
            distance: 5,
            dragTarget: '.' + TEXTWRAP,
            helper: function (e) {
                _this_1.dragTarget = e.sender.target;
                var dragRoot = sf.base.closest(_this_1.dragTarget, '.' + ROOT);
                var dragWrap = sf.base.closest(_this_1.dragTarget, '.' + TEXTWRAP);
                _this_1.dragLi = sf.base.closest(_this_1.dragTarget, '.' + LISTITEM);
                if (_this_1.options.fullRowSelect && !dragWrap && _this_1.dragTarget.classList.contains(FULLROW)) {
                    dragWrap = _this_1.dragTarget.nextElementSibling;
                }
                if (!_this_1.dragTarget || !e.element.isSameNode(dragRoot) || !dragWrap ||
                    _this_1.dragTarget.classList.contains(ROOT) || _this_1.dragTarget.classList.contains(PARENTITEM) ||
                    _this_1.dragTarget.classList.contains(LISTITEM) || _this_1.dragLi.classList.contains(DISABLE)) {
                    return false;
                }
                var cloneEle = (dragWrap.cloneNode(true));
                if (sf.base.isNullOrUndefined(sf.base.select('div.' + ICON, cloneEle))) {
                    var icon = sf.base.createElement('div', { className: ICON + ' ' + EXPANDABLE });
                    cloneEle.insertBefore(icon, cloneEle.children[0]);
                }
                var cssClass = DRAGITEM + ' ' + ROOT + ' ' + _this_1.options.cssClass + ' ' + (_this_1.options.enableRtl ? RTL : EMPTY);
                virtualEle = sf.base.createElement('div', { className: cssClass });
                virtualEle.appendChild(cloneEle);
                var selectedLI = _this_1.element.querySelectorAll('.' + ACTIVE);
                var length = selectedLI.length;
                if (length > 1 && _this_1.options.allowMultiSelection && _this_1.dragLi.classList.contains(ACTIVE)) {
                    var cNode = sf.base.createElement('span', { className: DROPCOUNT, innerHTML: EMPTY + length });
                    virtualEle.appendChild(cNode);
                }
                document.body.appendChild(virtualEle);
                document.body.style.cursor = EMPTY;
                _this_1.dragData = _this_1.getNodeData(_this_1.dragLi);
                _this_1.virtualEle = virtualEle;
                return virtualEle;
            },
            drag: function (e) {
                if (_this_1.mouseUpStatus) {
                    sf.base.detach(virtualEle);
                    sf.base.removeClass([_this_1.element], DRAGGING);
                    _this_1.removeVirtualEle();
                    document.body.style.cursor = EMPTY;
                    return;
                }
                _this_1.dragObj.setProperties({ cursorAt: { top: (!sf.base.isNullOrUndefined(e.event.targetTouches) || sf.base.Browser.isDevice) ? 60 : -20 } });
                _this_1.dragAction(e, virtualEle);
            },
            dragStart: function (e) {
                sf.base.addClass([_this_1.element], DRAGGING);
                var listItem = sf.base.closest(e.target, '.' + LISTITEM);
                var level;
                if (listItem) {
                    level = parseInt(listItem.getAttribute('aria-level'), 10);
                    sf.base.EventHandler.add(listItem, MOUSEUP, _this_1.mouseupHandler, _this_1);
                }
                var eventArgs = _this_1.getDragEvent(e.event, _this_1, null, e.target, null, virtualEle, level);
                if (eventArgs.draggedNode.classList.contains(EDITING)) {
                    _this_1.dragObj.intDestroy(e.event);
                    _this_1.dragCancelAction(virtualEle);
                }
                else {
                    _this_1.dragStartEventArgs = e;
                    var left = _this_1.getXYValue(e.event, 'X');
                    var top_1 = _this_1.getXYValue(e.event, 'Y');
                    virtualEle.style.display = NONE;
                    _this_1.dotNetRef.invokeMethodAsync('TriggerDragStartEvent', _this_1.updateObjectValues(eventArgs), left, top_1);
                }
            },
            dragStop: function (e) {
                sf.base.removeClass([_this_1.element], DRAGGING);
                _this_1.removeVirtualEle();
                var dropTarget = e.target;
                var preventTargetExpand = false;
                _this_1.dropRoot = (sf.base.closest(dropTarget, '.' + DROPPABLE));
                _this_1.isHelperElement = true;
                if (!dropTarget || !_this_1.dropRoot) {
                    if (e.helper && e.helper.parentNode) {
                        sf.base.remove(e.helper);
                    }
                    document.body.style.cursor = EMPTY;
                    _this_1.isHelperElement = false;
                }
                var listItem = sf.base.closest(dropTarget, '.' + LISTITEM);
                var level;
                if (listItem) {
                    level = parseInt(listItem.getAttribute('aria-level'), 10);
                    sf.base.EventHandler.remove(listItem, MOUSEUP, _this_1.mouseupHandler);
                    _this_1.mouseUpStatus = false;
                }
                var dropEle = dropTarget;
                var eventArgs = _this_1.getDragEvent(e.event, _this_1, dropTarget, dropEle, null, e.helper, level);
                _this_1.dragStopEventArgs = e;
                eventArgs.preventTargetExpand = preventTargetExpand;
                var left = _this_1.getXYValue(e.event, 'X');
                var top = _this_1.getXYValue(e.event, 'Y');
                if (sf.base.isNullOrUndefined(eventArgs.dropIndicator)) {
                    eventArgs.dropIndicator = NODROP;
                    document.body.style.cursor = 'not-allowed';
                }
                _this_1.dotNetRef.invokeMethodAsync('TriggerDragStopEvent', _this_1.updateObjectValues(eventArgs), left, top);
            }
        });
        this.dropObj = new sf.base.Droppable(this.element, {
            out: function (e) {
                if (!sf.base.isNullOrUndefined(e) && !e.target.classList.contains(SIBLING)) {
                    document.body.style.cursor = 'not-allowed';
                }
            },
            over: function (e) {
                document.body.style.cursor = EMPTY;
            }
        });
    };
    SfTreeView.prototype.updateObjectValues = function (evtArgs) {
        /* tslint:disable:no-string-literal */
        evtArgs['clonedNode'] = null;
        evtArgs['draggedNode'] = null;
        evtArgs['draggedParentNode'] = null;
        evtArgs['dropTarget'] = null;
        evtArgs['droppedNode'] = null;
        evtArgs['target'] = null;
        /* tslint:enable:no-string-literal */
        return evtArgs;
    };
    SfTreeView.prototype.dragNodeStop = function (eventArgs) {
        this.dragParent = eventArgs.draggedParentNode;
        this.preventExpand = eventArgs.preventTargetExpand;
        if (eventArgs.cancel || eventArgs.dropIndicator == NODROP) {
            if (this.dragStopEventArgs.helper.parentNode) {
                sf.base.remove(this.dragStopEventArgs.helper);
            }
            document.body.style.cursor = '';
            this.isHelperElement = false;
        }
        this.dragStartAction = false;
        if (this.isHelperElement) {
            this.dropAction(this.dragStopEventArgs, true);
        }
    };
    SfTreeView.prototype.dragStartActionContinue = function (cancel) {
        if (cancel) {
            this.dragObj.intDestroy(this.dragStartEventArgs.event);
            this.dragCancelAction(this.virtualEle);
        }
        else {
            this.virtualEle.style.display = BLOCK;
            this.dragStartAction = true;
            this.dragStartEventArgs.bindEvents(sf.base.getElement(this.dragStartEventArgs.dragElement));
        }
    };
    SfTreeView.prototype.getId = function (ele) {
        if (sf.base.isNullOrUndefined(ele)) {
            return null;
        }
        else if (typeof ele === 'string') {
            return ele;
        }
        else if (typeof ele === 'object') {
            return (sf.base.getElement(ele)).getAttribute('data-uid');
        }
        else {
            return null;
        }
    };
    // eslint-disable-next-line
    SfTreeView.prototype.getOffsetValue = function (e, direction) {
        var value;
        var classList = e.target.classList;
        if (sf.base.Browser.info.name === 'mozilla' && !sf.base.isNullOrUndefined(classList)) {
            var rect = e.target.getBoundingClientRect();
            value = Math.ceil((direction === 'Y') ? (e.event.clientY - rect.top) : (e.event.clientX - rect.left));
        }
        else {
            value = (direction === 'Y') ? e.event.offsetY : e.event.offsetX;
        }
        return value;
    };
    // eslint-disable-next-line
    SfTreeView.prototype.dropAction = function (e, isBlazorDrop) {
        var offsetY = this.getOffsetValue(e, 'Y');
        var dropTarget = e.target;
        var dragObj;
        var level;
        var drop = false;
        if (!isBlazorDrop) {
            dragObj = e.dragData.draggable.blazor__instance;
        }
        else {
            dragObj = e.element.blazor__instance;
        }
        if (dragObj && dragObj.dragTarget) {
            var dragTarget = dragObj.dragTarget;
            var dragLi = (sf.base.closest(dragTarget, '.' + LISTITEM));
            var dropLi = (sf.base.closest(dropTarget, '.' + LISTITEM));
            if (dropLi == null && dropTarget.classList.contains(ROOT)) {
                dropLi = dropTarget.firstElementChild;
            }
            if (!isBlazorDrop) {
                sf.base.remove(e.droppedElement);
            }
            else {
                sf.base.remove(e.helper);
            }
            document.body.style.cursor = EMPTY;
            if (!dropLi || dropLi.isSameNode(dragLi) || this.isDescendant(dragLi, dropLi)) {
                return;
            }
            if (dragObj.allowMultiSelection && dragLi.classList.contains(ACTIVE)) {
                var sNodes = sf.base.selectAll('.' + ACTIVE, dragObj.element);
                if (e.target.offsetHeight <= 33 && offsetY > e.target.offsetHeight - 10 && offsetY > 6) {
                    for (var i = sNodes.length - 1; i >= 0; i--) {
                        if (dropLi.isSameNode(sNodes[i]) || this.isDescendant(sNodes[i], dropLi)) {
                            continue;
                        }
                        this.appendNode(dropTarget, sNodes[i], dropLi, e, dragObj, offsetY);
                    }
                }
                else {
                    for (var i = 0; i < sNodes.length; i++) {
                        if (dropLi.isSameNode(sNodes[i]) || this.isDescendant(sNodes[i], dropLi)) {
                            continue;
                        }
                        this.appendNode(dropTarget, sNodes[i], dropLi, e, dragObj, offsetY);
                    }
                }
            }
            else {
                this.appendNode(dropTarget, dragLi, dropLi, e, dragObj, offsetY);
            }
            level = parseInt(dragLi.getAttribute('aria-level'), 10);
            drop = true;
        }
        var element = isBlazorDrop ? e.element : e.dragData.draggedElement;
        var eventArgs = this.getDragEvent(e.event, dragObj, dropTarget, e.target, element, null, level, drop);
        var left = this.getXYValue(e.event, 'X');
        var top = this.getXYValue(e.event, 'Y');
        this.dotNetRef.invokeMethodAsync('TriggerNodeDropped', this.updateObjectValues(eventArgs), left, top);
    };
    SfTreeView.prototype.isDoubleTapped = function (e) {
        var target = e.originalEvent.target;
        var secondTap;
        if (target && e.tapCount) {
            if (e.tapCount === 1) {
                this.firstTap = sf.base.closest(target, '.' + LISTITEM);
            }
            else if (e.tapCount === 2) {
                secondTap = sf.base.closest(target, '.' + LISTITEM);
            }
        }
        return (this.firstTap === secondTap);
    };
    SfTreeView.prototype.isDescendant = function (parent, child) {
        var node = child.parentNode;
        while (!sf.base.isNullOrUndefined(node)) {
            if (node === parent) {
                return true;
            }
            node = node.parentNode;
        }
        return false;
    };
    SfTreeView.prototype.appendNode = function (dropTarget, dragLi, dropLi, e, dragObj, offsetY) {
        var checkContainer = sf.base.closest(dropTarget, '.' + CHECKBOXWRAP);
        var collapse = sf.base.closest(e.target, '.' + COLLAPSIBLE);
        var expand = sf.base.closest(e.target, '.' + EXPANDABLE);
        var offsetX = this.getOffsetValue(e, 'X');
        if (!dragLi.classList.contains(DISABLE) && !checkContainer && ((expand && offsetY < 5) || (collapse && offsetX < 3)
            || (expand && offsetY > 19) || (collapse && offsetX > 19) || (!expand && !collapse))) {
            if (dropTarget.nodeName === 'LI') {
                this.dropAsSiblingNode(dragLi, dropLi, e, dragObj);
            }
            else if (dropTarget.firstElementChild && dropTarget.classList.contains(ROOT)) {
                if (dropTarget.firstElementChild.nodeName === 'UL') {
                    this.dropAsSiblingNode(dragLi, dropLi, e, dragObj);
                }
            }
            else if ((dropTarget.classList.contains(COLLAPSIBLE)) || (dropTarget.classList.contains(EXPANDABLE))) {
                this.dropAsSiblingNode(dragLi, dropLi, e, dragObj);
            }
            else {
                this.dropAsChildNode(dragLi, dropLi, dragObj, null, e, offsetY);
            }
        }
        else {
            this.dropAsChildNode(dragLi, dropLi, dragObj, null, e, offsetY, true);
        }
    };
    SfTreeView.prototype.dropAsSiblingNode = function (dragLi, dropLi, e, dragObj) {
        var dropUl = sf.base.closest(dropLi, '.' + PARENTITEM);
        var dragParentUl = sf.base.closest(dragLi, '.' + PARENTITEM);
        var dragParentLi = sf.base.closest(dragParentUl, '.' + LISTITEM);
        var dropParentLi = sf.base.closest(dropUl, '.' + LISTITEM);
        var dropParentLiId = null;
        var dragParentLiId = null;
        var pre;
        // eslint-disable-next-line
        var offsetX = this.getOffsetValue(e, 'X');
        // eslint-disable-next-line
        var offsetY = this.getOffsetValue(e, 'Y');
        if (e.target.offsetHeight > 0 && offsetY > e.target.offsetHeight - 2) {
            pre = false;
        }
        else if (offsetY < 2) {
            pre = true;
        }
        else if (e.target.classList.contains(EXPANDABLE) || (e.target.classList.contains(COLLAPSIBLE))) {
            if ((offsetY < 5) || (offsetX < 3)) {
                pre = true;
            }
            else if ((offsetY > 15) || (offsetX > 17)) {
                pre = false;
            }
        }
        if (dropParentLi) {
            dropParentLiId = dropParentLi.getAttribute('data-uid');
        }
        if (dragParentLi) {
            dragParentLiId = dragParentLi.getAttribute('data-uid');
        }
        var outerDrag = this.isExternalDrop(dragLi);
        var targetControl = this.dropRoot ? this.dropRoot.blazor__instance : null;
        var droppedInstance = outerDrag ? targetControl : this;
        var eventArgs = this.getDropArgs(dragLi, dropLi, dragParentLiId, dragObj, dropParentLiId, pre);
        droppedInstance.dotNetRef.invokeMethodAsync('DropNodeAsSibling', eventArgs);
        this.updateAriaLevel(dragLi);
    };
    SfTreeView.prototype.updateAriaLevel = function (dragLi) {
        var level = this.parents(dragLi, '.' + PARENTITEM).length;
        dragLi.setAttribute('aria-level', EMPTY + level);
        this.updateChildAriaLevel(sf.base.select('.' + PARENTITEM, dragLi), level + 1);
    };
    SfTreeView.prototype.updateChildAriaLevel = function (element, level) {
        if (!sf.base.isNullOrUndefined(element)) {
            var cNodes = element.querySelectorAll('li');
            for (var i = 0, len = cNodes.length; i < len; i++) {
                var liEle = cNodes[i];
                liEle.setAttribute('aria-level', EMPTY + level);
                this.updateChildAriaLevel(sf.base.select('.' + PARENTITEM, liEle), level + 1);
            }
        }
    };
    SfTreeView.prototype.dropAsChildNode = function (dragLi, dropLi, dragObj, index, e, pos, isCheck) {
        var dragParentUl = sf.base.closest(dragLi, '.' + PARENTITEM);
        var dragParentLi = sf.base.closest(dragParentUl, '.' + LISTITEM);
        var dropParentUl = sf.base.closest(dropLi, '.' + PARENTITEM);
        var dropParentLi = sf.base.closest(dropParentUl, '.' + LISTITEM);
        var dropParentLiId = null;
        var dragParentLiId = null;
        if (dropParentLi) {
            dropParentLiId = dropParentLi.getAttribute('data-uid');
        }
        if (dragParentLi) {
            dragParentLiId = dragParentLi.getAttribute('data-uid');
        }
        var outerDrag = this.isExternalDrop(dragLi);
        var srcControl = sf.base.closest(dragLi, '.' + ROOT) ? sf.base.closest(dragLi, '.' + ROOT).blazor__instance :
            (this.element ? this.element.blazor__instance : null);
        var targetControl = this.dropRoot ? this.dropRoot.blazor__instance : null;
        var droppedInstance = outerDrag ? targetControl : this;
        var eventArgs;
        if (e && (pos < 7) && !isCheck) {
            eventArgs = this.getDropArgs(dragLi, dropLi, dragParentLiId, srcControl, dropParentLiId, true);
            droppedInstance.dotNetRef.invokeMethodAsync('DropNodeAsSibling', eventArgs);
        }
        else if (e && (e.target.offsetHeight > 0 && pos > (e.target.offsetHeight - 10)) && !isCheck) {
            eventArgs = this.getDropArgs(dragLi, dropLi, dragParentLiId, srcControl, dropParentLiId, false);
            droppedInstance.dotNetRef.invokeMethodAsync('DropNodeAsSibling', eventArgs);
        }
        else {
            eventArgs = this.getDropArgs(dragLi, dropLi, dragParentLiId, srcControl);
            droppedInstance.dotNetRef.invokeMethodAsync('DropNodeAsChild', eventArgs);
        }
        this.updateAriaLevel(dragLi);
    };
    SfTreeView.prototype.isExternalDrop = function (dragLi) {
        var isExternalDrop = false;
        var srcElement = sf.base.closest(dragLi, '.' + ROOT) ? sf.base.closest(dragLi, '.' + ROOT) : this.element;
        var targetElement = this.dropRoot;
        if ((srcElement != null && targetElement != null && !srcElement.isSameNode(targetElement))) {
            isExternalDrop = true;
        }
        return isExternalDrop;
    };
    SfTreeView.prototype.getDropArgs = function (dragLi, dropLi, dragParentLiId, treeObj, dropParentLi, pre) {
        return {
            dragLi: dragLi.getAttribute('data-uid'),
            dropLi: dropLi.getAttribute('data-uid'),
            dragParentLi: dragParentLiId,
            dropParentLi: dropParentLi,
            pre: pre,
            srcTree: treeObj.dotNetRef,
            isExternalDrag: this.isExternalDrop(dragLi)
        };
    };
    SfTreeView.prototype.dragCancelAction = function (virtualEle) {
        sf.base.detach(virtualEle);
        sf.base.removeClass([this.element], DRAGGING);
        this.dragStartAction = false;
    };
    SfTreeView.prototype.removeVirtualEle = function () {
        var sibEle = sf.base.select('.' + SIBLING);
        if (sibEle) {
            sf.base.detach(sibEle);
        }
    };
    SfTreeView.prototype.dragAction = function (e, virtualEle) {
        var dropRoot = sf.base.closest(e.target, '.' + DROPPABLE);
        var dropWrap = sf.base.closest(e.target, '.' + TEXTWRAP);
        var icon = sf.base.select('div.' + ICON, virtualEle);
        var offsetX = this.getOffsetValue(e, 'X');
        var offsetY = this.getOffsetValue(e, 'Y');
        sf.base.removeClass([icon], [DROPIN, DROPNEXT, DROPOUT, NODROP]);
        this.removeVirtualEle();
        document.body.style.cursor = EMPTY;
        var classList = e.target.classList;
        if (this.options.fullRowSelect && !dropWrap && !sf.base.isNullOrUndefined(classList) && classList.contains(FULLROW)) {
            dropWrap = e.target.nextElementSibling;
        }
        if (dropRoot) {
            var dropLi = sf.base.closest(e.target, '.' + LISTITEM);
            var checkContainer = sf.base.closest(e.target, '.' + CHECKBOXWRAP);
            var collapse = sf.base.closest(e.target, '.' + COLLAPSIBLE);
            var expand = sf.base.closest(e.target, '.' + EXPANDABLE);
            if (!dropRoot.classList.contains(ROOT) || (dropWrap &&
                (!dropLi.isSameNode(this.dragLi) && !this.isDescendant(this.dragLi, dropLi)))) {
                if ((dropLi && e && (!expand && !collapse) && (offsetY < 7) && !checkContainer) ||
                    (((expand && offsetY < 5) || (collapse && offsetX < 3)))) {
                    sf.base.addClass([icon], DROPNEXT);
                    var element = sf.base.createElement('div', { className: SIBLING });
                    var index = this.options.fullRowSelect ? (1) : (0);
                    dropLi.insertBefore(element, dropLi.children[index]);
                }
                else if ((dropLi && e && (!expand && !collapse) && (e.target.offsetHeight > 0 && offsetY >
                    (e.target.offsetHeight - 10)) && !checkContainer) || (((expand && offsetY > 19) ||
                    (collapse && offsetX > 19)))) {
                    sf.base.addClass([icon], DROPNEXT);
                    var element = sf.base.createElement('div', { className: SIBLING });
                    var index = this.options.fullRowSelect ? (2) : (1);
                    dropLi.insertBefore(element, dropLi.children[index]);
                }
                else {
                    sf.base.addClass([icon], DROPIN);
                }
            }
            else if (e.target.nodeName === 'LI' && (!dropLi.isSameNode(this.dragLi) && !this.isDescendant(this.dragLi, dropLi))) {
                sf.base.addClass([icon], DROPNEXT);
                this.renderVirtualEle(e);
            }
            else if (e.target.classList.contains(SIBLING)) {
                sf.base.addClass([icon], DROPNEXT);
            }
            else if (e.target.classList.contains(DROPPABLE)) {
                sf.base.addClass([icon], DROPIN);
            }
            else {
                sf.base.addClass([icon], DROPOUT);
            }
        }
        else {
            sf.base.addClass([icon], NODROP);
            document.body.style.cursor = 'not-allowed';
        }
        var listItem = sf.base.closest(e.target, LISTITEM);
        var level;
        if (listItem) {
            level = parseInt(listItem.getAttribute('aria-level'), 10);
        }
        var eventArgs = this.getDragEvent(e.event, this, e.target, e.target, null, virtualEle, level);
        if (eventArgs.dropIndicator) {
            sf.base.removeClass([icon], eventArgs.dropIndicator);
        }
        this.iconElement = icon;
        this.draggingEventArgs = eventArgs;
        var left = this.getXYValue(e.event, 'X');
        var top = this.getXYValue(e.event, 'Y');
        this.dotNetRef.invokeMethodAsync('TriggerNodeDraggingEvent', this.updateObjectValues(eventArgs), left, top);
    };
    SfTreeView.prototype.nodeDragging = function (eventArgs) {
        if (this.draggingEventArgs.dropIndicator) {
            sf.base.addClass([this.iconElement], this.draggingEventArgs.dropIndicator);
        }
    };
    SfTreeView.prototype.renderVirtualEle = function (e) {
        // eslint-disable-next-line
        var offsetY = this.getOffsetValue(e, 'Y');
        var previous;
        if (offsetY > e.target.offsetHeight - 2) {
            previous = false;
        }
        else if (offsetY < 2) {
            previous = true;
        }
        var element = sf.base.createElement('div', { className: SIBLING });
        var index = this.options.fullRowSelect ? (previous ? 1 : 2) : (previous ? 0 : 1);
        e.target.insertBefore(element, e.target.children[index]);
    };
    SfTreeView.prototype.parents = function (element, selector) {
        var matched = [];
        var node = element.parentNode;
        while (!sf.base.isNullOrUndefined(node)) {
            if (sf.base.matches(node, selector)) {
                matched.push(node);
            }
            node = node.parentNode;
        }
        return matched;
    };
    SfTreeView.prototype.getDragEvent = function (event, obj, dropTarget, target, dragNode, cloneEle, level, drop) {
        var dropLi = dropTarget ? sf.base.closest(dropTarget, '.' + LISTITEM) : null;
        // eslint-disable-next-line
        var dropData = dropLi ? this.getNodeData(dropLi) : null;
        var draggedNode = obj ? obj.dragLi : dragNode;
        // eslint-disable-next-line
        var draggedNodeData = obj ? obj.dragData : null;
        var newParent = dropTarget ? this.parents(dropTarget, '.' + LISTITEM) : null;
        var dragLiParent = obj.dragLi.parentElement;
        var dragParent = obj.dragLi ? sf.base.closest(dragLiParent, '.' + LISTITEM) : null;
        var targetParent = null;
        var indexValue = null;
        var iconCss = [DROPNEXT, DROPIN, DROPOUT, NODROP];
        var iconClass = null;
        var node = drop ? draggedNode : dropLi;
        var index = node ? sf.base.closest(node, '.e-list-parent') : null;
        var i = 0;
        dragParent = (obj.dragLi && dragParent === null) ? sf.base.closest(dragLiParent, '.' + ROOT) : dragParent;
        dragParent = drop ? this.dragParent : dragParent;
        if (cloneEle) {
            while (i < 4) {
                if (sf.base.select('.' + ICON, cloneEle).classList.contains(iconCss[i])) {
                    iconClass = iconCss[i];
                    break;
                }
                i++;
            }
        }
        if (index) {
            var dropTar = 0;
            for (i = 0; i < index.childElementCount; i++) {
                dropTar = (!drop && index.children[i] === draggedNode && dropLi !== draggedNode) ? ++dropTar : dropTar;
                if ((!drop && index.children[i].classList.contains('e-hover'))) {
                    indexValue = (event.offsetY >= 23) ? i + 1 : i;
                    break;
                }
                else if (index.children[i] === node) {
                    indexValue = (event.offsetY >= 23) ? i : i;
                    break;
                }
            }
            indexValue = (dropTar !== 0) ? --indexValue : indexValue;
        }
        if (dropTarget) {
            if (newParent.length === 0) {
                targetParent = null;
            }
            else if (dropTarget.classList.contains(LISTITEM)) {
                targetParent = newParent[0];
            }
            else {
                targetParent = newParent[1];
            }
        }
        if (dropLi === draggedNode) {
            targetParent = dropLi;
        }
        if (dropTarget && target.offsetHeight <= 33 && event.offsetY < target.offsetHeight - 10 && event.offsetY > 6) {
            targetParent = dropLi;
            if (!drop) {
                level = ++level;
                var parent_1 = targetParent ? sf.base.select('.e-list-parent', targetParent) : null;
                indexValue = (parent_1) ? parent_1.children.length : 0;
            }
        }
        return {
            cancel: false,
            clonedNode: cloneEle,
            event: event,
            draggedNode: draggedNode,
            draggedNodeData: draggedNodeData,
            droppedNode: dropLi,
            droppedNodeData: dropData,
            dropIndex: indexValue,
            dropLevel: level,
            draggedParentNode: dragParent,
            dropTarget: targetParent,
            dropIndicator: iconClass,
            target: target
        };
    };
    SfTreeView.prototype.editingHandler = function (e) {
        var target = e.target;
        if (!target || target.classList.contains(ROOT) || target.classList.contains(PARENTITEM) ||
            target.classList.contains(LISTITEM) || target.classList.contains(ICON) ||
            target.classList.contains(INPUT) || target.classList.contains(INPUTGROUP)) {
            return;
        }
        else {
            this.createTextbox(sf.base.closest(target, '.' + LISTITEM), e);
        }
    };
    SfTreeView.prototype.createTextbox = function (liEle, e) {
        this.editEventArgs = this.getEditEvent(liEle, null, null);
        this.isEdited = true;
        this.dotNetRef.invokeMethodAsync('TriggerNodeEditingEvent', this.editEventArgs);
    };
    SfTreeView.prototype.getEditEvent = function (liEle, newText, inputEle) {
        // eslint-disable-next-line
        var data = this.getNodeData(liEle);
        return { newText: newText, nodeData: data, oldText: this.oldText, innerHtml: inputEle };
    };
    SfTreeView.prototype.focusIn = function () {
        if (!this.mouseDownStatus) {
            sf.base.addClass([this.getFocusedNode()], HOVER);
        }
        this.mouseDownStatus = false;
    };
    SfTreeView.prototype.focusOut = function () {
        sf.base.removeClass([this.getFocusedNode()], HOVER);
    };
    SfTreeView.prototype.wireEvents = function () {
        if (!this.options.disabled) {
            this.setExpandOnType();
            sf.base.EventHandler.add(this.element, MOUSEDOWN, this.mouseDownHandler, this);
            sf.base.EventHandler.add(this.element, MOUSEOVER, this.onMouseOver, this);
            sf.base.EventHandler.add(this.element, FOCUSING, this.focusIn, this);
            sf.base.EventHandler.add(this.element, BLUR, this.focusOut, this);
            sf.base.EventHandler.add(this.element, MOUSEOUT, this.onMouseLeave, this);
            if (this.options.showCheckBox) {
                var frame = sf.base.select('.' + CHECKBOXFRAME, this.element);
                if (!sf.base.isNullOrUndefined(frame)) {
                    sf.base.EventHandler.add(frame, 'mousedown', this.frameMouseHandler, this);
                    sf.base.EventHandler.add(frame, 'mouseup', this.frameMouseHandler, this);
                }
            }
            this.wireClickEvent(true);
            if (this.options.expandOnType !== EXPANDONNONE) {
                this.wireExpandOnEvent(true);
            }
            this.keyboardModule = new sf.base.KeyboardEvents(this.element, {
                keyAction: this.keyboardActionHandler.bind(this),
                keyConfigs: this.keyConfigs,
                eventName: 'keydown'
            });
        }
    };
    SfTreeView.prototype.frameMouseHandler = function (e) {
        var rippleSpan = sf.base.select('.' + CHECKBOXRIPPLE, e.target.parentElement);
        this.rippleMouseHandler(e, rippleSpan);
    };
    SfTreeView.prototype.rippleMouseHandler = function (e, rippleSpan) {
        if (rippleSpan) {
            var event_1 = document.createEvent('MouseEvents');
            event_1.initEvent(e.type, false, true);
            rippleSpan.dispatchEvent(event_1);
        }
    };
    SfTreeView.prototype.setExpandOnType = function () {
        var expandOnType = this.options.expandOnType;
        this.options.expandOnType = (expandOnType === EXPANDONAUTO) ? (sf.base.Browser.isDevice ? CLICK : DBLCLICK) : expandOnType;
    };
    SfTreeView.prototype.expandHandler = function (e) {
        var target = e.originalEvent.target;
        if (!target || target.classList.contains(INPUT) || target.classList.contains(ROOT) ||
            target.classList.contains(PARENTITEM) || target.classList.contains(LISTITEM) ||
            target.classList.contains(ICON) || this.options.showCheckBox && sf.base.closest(target, '.' + CHECKBOXWRAP)) {
            return;
        }
        else {
            this.expandCollapseAction(sf.base.closest(target, '.' + LISTITEM), e);
        }
    };
    SfTreeView.prototype.expandCollapseAction = function (currLi, e) {
        var icon = sf.base.select('div.' + ICON, currLi);
        if (!icon || icon.classList.contains(PROCESS)) {
            return;
        }
        else {
            var classList = icon.classList;
            if (classList.contains(EXPANDABLE)) {
                this.expandAction(currLi, e);
            }
            else if (classList.contains(COLLAPSIBLE)) {
                this.collapseAction(currLi, e, false);
            }
        }
    };
    SfTreeView.prototype.animateHeight = function (args, start, end) {
        var remaining = (args.duration - args.timeStamp) / args.duration;
        var currentHeight = (end - start) * remaining + start;
        args.element.parentElement.style.height = currentHeight + 'px';
    };
    SfTreeView.prototype.expandAction = function (currLi, e) {
        this.expandArgs = this.getExpandEvent(currLi, e);
        if (this.options.allowTextWrap) {
            var ul = sf.base.select('.' + PARENTITEM, currLi);
            this.isNodeRendered = ul ? true : false;
        }
        if (currLi && currLi.classList.contains(PROCESS)) {
            sf.base.removeClass([currLi], PROCESS);
        }
        this.dotNetRef.invokeMethodAsync('TriggerNodeExpandingEvent', this.expandArgs);
    };
    SfTreeView.prototype.collapseAction = function (currLi, e, fromClickHandler) {
        this.expandArgs = this.getExpandEvent(currLi, e);
        var start = 0;
        var end = 0;
        // eslint-disable-next-line
        var proxy = this;
        var ul = sf.base.select('.' + PARENTITEM, currLi);
        var liEle = currLi;
        var activeElement = sf.base.select('.' + LISTITEM + '.' + ACTIVE, currLi);
        if (ul) {
            var icon = sf.base.select('div.' + ICON, liEle);
            sf.base.removeClass([icon], COLLAPSIBLE);
            sf.base.addClass([icon], EXPANDABLE);
        }
        if (!sf.base.isNullOrUndefined(currLi.getAttribute("aria-expanded"))) {
            currLi.setAttribute("aria-expanded", "false");
        }
        if (this.options.animation.collapse.duration === 0) {
            ul.style.display = NONE;
            proxy.dotNetRef.invokeMethodAsync('TriggerNodeCollapsingEvent', proxy.expandArgs);
            if (fromClickHandler) {
                proxy.triggerClickEvent(e.originalEvent, currLi);
            }
        }
        else {
            this.animationObj.animate(ul, {
                name: this.options.animation.collapse.effect,
                duration: this.options.animation.collapse.duration,
                timingFunction: this.options.animation.collapse.easing,
                begin: function (args) {
                    liEle.style.overflow = HIDDEN;
                    if (!sf.base.isNullOrUndefined(activeElement) && activeElement instanceof HTMLElement) {
                        activeElement.classList.add(ITEM_ANIMATION_ACTIVE);
                    }
                    start = sf.base.select('.' + TEXTWRAP, currLi).offsetHeight;
                    end = liEle.offsetHeight;
                },
                progress: function (args) {
                    proxy.animateHeight(args, start, end);
                },
                end: function (args) {
                    args.element.style.display = NONE;
                    if (!sf.base.isNullOrUndefined(activeElement) && activeElement instanceof HTMLElement) {
                        activeElement.classList.remove(ITEM_ANIMATION_ACTIVE);
                    }
                    proxy.dotNetRef.invokeMethodAsync('TriggerNodeCollapsingEvent', proxy.expandArgs);
                    if (fromClickHandler) {
                        proxy.triggerClickEvent(e.originalEvent, currLi);
                    }
                }
            });
        }
    };
    SfTreeView.prototype.wireExpandOnEvent = function (toBind) {
        var _this_1 = this;
        if (toBind) {
            // eslint-disable-next-line
            var proxy_2 = this;
            this.touchExpandObj = new sf.base.Touch(this.element, {
                tap: function (e) {
                    if ((_this_1.options.expandOnType === CLICK || (_this_1.options.expandOnType === DBLCLICK
                        && _this_1.isDoubleTapped(e) && e.tapCount === 2))
                        && e.originalEvent.which !== 3) {
                        proxy_2.expandHandler(e);
                    }
                }
            });
        }
        else {
            if (this.touchExpandObj) {
                this.touchExpandObj.destroy();
            }
        }
    };
    // eslint-disable-next-line
    SfTreeView.prototype.getNodeData = function (currLi, fromDS) {
        if (!sf.base.isNullOrUndefined(currLi) && currLi.classList.contains(LISTITEM) &&
            !sf.base.isNullOrUndefined(sf.base.closest(currLi, '.' + CONTROL)) && sf.base.closest(currLi, '.' + CONTROL).classList.contains(ROOT)) {
            var id = currLi.getAttribute('data-uid');
            var pNode = sf.base.closest(currLi.parentNode, '.' + LISTITEM);
            var pid = pNode ? pNode.getAttribute('data-uid') : null;
            var selected = currLi.classList.contains(ACTIVE);
            var expanded = (currLi.getAttribute('aria-expanded') === 'true');
            var hasChildren = (currLi.getAttribute('aria-expanded') === null);
            var checked = null;
            if (this.options.showCheckBox) {
                checked = sf.base.select('.' + CHECKBOXWRAP, currLi).getAttribute('aria-checked');
            }
            return {
                id: id, text: null, parentID: pid, selected: selected, expanded: expanded,
                isChecked: checked, hasChildren: hasChildren
            };
        }
        return { id: EMPTY, text: EMPTY, parentID: EMPTY, selected: false, expanded: false, isChecked: EMPTY, hasChildren: false };
    };
    SfTreeView.prototype.getExpandEvent = function (currLi, e) {
        // eslint-disable-next-line
        var nodedata = this.getNodeData(currLi);
        return { isInteracted: !sf.base.isNullOrUndefined(e), nodeData: nodedata, event: e, isLoaded: currLi.querySelector('ul') != null ? true : false, nodeLevel: parseInt(currLi.getAttribute('aria-level'), 10) };
    };
    SfTreeView.prototype.updateSpinnerClass = function () {
        var spinnerEle = this.element.querySelector('.e-icons-spinner');
        sf.base.removeClass([spinnerEle], "e-icons-spinner");
    };
    SfTreeView.prototype.expandedNode = function (expandArgs) {
        var _this_1 = this;
        var li = this.element.querySelector('[data-uid="' + expandArgs.nodeData.id + '"]');
        this.focussedElement = li;
        var ulele = sf.base.select('.' + PARENTITEM, li);
        if (ulele) {
            ulele.classList.remove(DISPLAYNONE);
            var icon_1 = sf.base.select('div.' + ICON, li);
            this.expandArgs = this.getExpandEvent(li, expandArgs.event);
            var liEle_1 = li;
            var activeElement_1 = sf.base.select('.' + LISTITEM + '.' + ACTIVE, li);
            var start_1 = 0;
            var end_1 = 0;
            // eslint-disable-next-line
            var proxy_3 = this;
            this.setHeight(liEle_1, ulele);
            if (this.options.animation.expand.duration === 0) {
                sf.base.removeClass([icon_1], EXPANDABLE);
                sf.base.addClass([icon_1], COLLAPSIBLE);
                proxy_3.dotNetRef.invokeMethodAsync('TriggerNodeExpandedEvent', proxy_3.expandArgs);
                ulele.style.display = BLOCK;
                liEle_1.style.display = BLOCK;
                liEle_1.style.overflow = EMPTY;
                liEle_1.style.height = EMPTY;
                sf.base.removeClass([icon_1], "e-icons-spinner");
                if (this.options.allowTextWrap && (!this.isNodeRendered || this.isEdited)) {
                    this.isEdited = false;
                    this.updateWrap(ulele);
                }
            }
            else {
                this.animationObj.animate(ulele, {
                    name: this.options.animation.expand.effect,
                    duration: this.options.animation.expand.duration,
                    timingFunction: this.options.animation.expand.easing,
                    begin: function (args) {
                        liEle_1.style.overflow = HIDDEN;
                        if (!sf.base.isNullOrUndefined(activeElement_1) && activeElement_1 instanceof HTMLElement) {
                            activeElement_1.classList.add(ITEM_ANIMATION_ACTIVE);
                        }
                        start_1 = liEle_1.offsetHeight;
                        end_1 = sf.base.select('.' + TEXTWRAP, li).offsetHeight;
                    },
                    progress: function (args) {
                        sf.base.removeClass([icon_1], EXPANDABLE);
                        sf.base.addClass([icon_1], COLLAPSIBLE);
                        args.element.style.display = BLOCK;
                        proxy_3.animateHeight(args, start_1, end_1);
                    },
                    end: function (args) {
                        args.element.style.display = BLOCK;
                        if (!sf.base.isNullOrUndefined(activeElement_1) && activeElement_1 instanceof HTMLElement) {
                            activeElement_1.classList.remove(ITEM_ANIMATION_ACTIVE);
                        }
                        proxy_3.dotNetRef.invokeMethodAsync('TriggerNodeExpandedEvent', proxy_3.expandArgs);
                        ulele.style.display = BLOCK;
                        liEle_1.style.display = BLOCK;
                        liEle_1.style.overflow = EMPTY;
                        liEle_1.style.height = EMPTY;
                        sf.base.removeClass([icon_1], "e-icons-spinner");
                        if (_this_1.options.allowTextWrap && (!_this_1.isNodeRendered || _this_1.isEdited)) {
                            _this_1.isEdited = false;
                            _this_1.updateWrap(ulele);
                        }
                    }
                });
            }
        }
        this.setHover(this.getFocusedNode());
    };
    SfTreeView.prototype.setHeight = function (currli, ul) {
        ul.style.display = BLOCK;
        ul.style.visibility = HIDDEN;
        currli.style.height = currli.offsetHeight + 'px';
        ul.style.display = NONE;
        ul.style.visibility = EMPTY;
    };
    SfTreeView.prototype.collapsedNode = function (collapseArgs) {
        var li = this.element.querySelector('[data-uid="' + collapseArgs.nodeData.id + '"]');
        this.focussedElement = li;
        var ulelement = li.querySelector('ul');
        if (ulelement) {
            ulelement.style.display = NONE;
            ulelement.classList.add(DISPLAYNONE);
        }
        li.style.overflow = EMPTY;
        li.style.height = EMPTY;
        this.expandArgs = this.getExpandEvent(li, null);
        var icon = sf.base.select('div.' + ICON, li);
        sf.base.removeClass([icon], COLLAPSIBLE);
        sf.base.addClass([icon], EXPANDABLE);
        this.dotNetRef.invokeMethodAsync('TriggerNodeCollapsedEvent', this.expandArgs);
    };
    SfTreeView.prototype.preventContextMenu = function (e) {
        e.preventDefault();
    };
    SfTreeView.prototype.clickHandler = function (event) {
        var target = event.originalEvent.target;
        var isCollapsAction = false;
        sf.base.EventHandler.remove(this.element, 'contextmenu', this.preventContextMenu);
        if (!target) {
            return;
        }
        else {
            var classList = target.classList;
            var li = sf.base.closest(target, '.' + LISTITEM);
            if (!li) {
                return;
            }
            else if (event.originalEvent.which !== 3) {
                var rippleElement = sf.base.select('.' + RIPPLEELMENT, li);
                var rippleIcons = sf.base.select('.' + ICON, li);
                this.removeHover();
                this.focussedElement = li;
                this.setFocusElement(li);
                if (this.options.showCheckBox && !li.classList.contains(DISABLE)) {
                    var checkContainer = sf.base.closest(target, '.' + CHECKBOXWRAP);
                    if (!sf.base.isNullOrUndefined(checkContainer)) {
                        var checkElement = sf.base.select('.' + CHECKBOXFRAME, checkContainer);
                        this.validateCheckNode(checkContainer, checkElement.classList.contains(CHECK), li, event.originalEvent);
                        this.triggerClickEvent(event.originalEvent, li);
                        return;
                    }
                }
                if (classList.contains(EXPANDABLE)) {
                    this.expandAction(li, event);
                }
                else if (classList.contains(COLLAPSIBLE)) {
                    this.collapseAction(li, event, true);
                    isCollapsAction = true;
                }
                else if (rippleElement && rippleIcons) {
                    if (rippleIcons.classList.contains(RIPPLE) && rippleIcons.classList.contains(EXPANDABLE)) {
                        this.expandAction(li, event);
                    }
                    else if (rippleIcons.classList.contains(RIPPLE) && rippleIcons.classList.contains(COLLAPSIBLE)) {
                        this.collapseAction(li, event, true);
                        isCollapsAction = true;
                    }
                    else if (!classList.contains(PARENTITEM) && !classList.contains(LISTITEM)) {
                        this.toggleSelect(li, event.originalEvent, false);
                    }
                }
                else {
                    if (!classList.contains(PARENTITEM) && !classList.contains(LISTITEM)) {
                        this.toggleSelect(li, event.originalEvent, false);
                    }
                }
            }
            if (!isCollapsAction) {
                this.triggerClickEvent(event.originalEvent, li);
            }
        }
    };
    SfTreeView.prototype.getXYValue = function (e, direction) {
        var touchList = e.changedTouches;
        var value;
        if (direction === 'X') {
            value = touchList ? touchList[0].clientX : e.clientX;
        }
        else {
            value = touchList ? touchList[0].clientY : e.clientY;
        }
        if (!value && e.type === 'focus' && e.target) {
            var rect = e.target.getBoundingClientRect();
            value = rect ? (direction === 'X' ? rect.left : rect.top) : null;
        }
        return Math.ceil(value);
    };
    SfTreeView.prototype.triggerClickEvent = function (e, li) {
        var eventArgs = {
            event: e,
            node: null
        };
        this.dotNetRef.invokeMethodAsync('TriggerNodeClickingEvent', eventArgs, li.getAttribute('data-uid'), this.getXYValue(e, 'X'), this.getXYValue(e, 'Y'));
    };
    SfTreeView.prototype.getCheckEvent = function (currLi, action, e) {
        return { action: action, isInteracted: !sf.base.isNullOrUndefined(e), nodeData: this.getNodeData(currLi) };
    };
    SfTreeView.prototype.validateCheckNode = function (checkWrap, isCheck, li, e) {
        var currLi = sf.base.closest(checkWrap, '.' + LISTITEM);
        var ariaState = !isCheck ? 'true' : 'false';
        if (!sf.base.isNullOrUndefined(ariaState)) {
            checkWrap.setAttribute('aria-checked', ariaState);
        }
        var eventArgs = this.getCheckEvent(currLi, isCheck ? 'uncheck' : 'check', e);
        this.dotNetRef.invokeMethodAsync('TriggerNodeCheckingEvent', eventArgs);
    };
    SfTreeView.prototype.toggleSelect = function (li, e, multiSelect) {
        if (!li.classList.contains(DISABLE)) {
            if (this.options.allowMultiSelection && ((e && e.ctrlKey) || multiSelect) && this.isActive(li)) {
                this.unselectNode(li, e, multiSelect);
            }
            else {
                this.selectNode(li, e, multiSelect);
                if (this.options.allowMultiSelection && e && (e.ctrlKey || e.shiftKey)) {
                    this.setFocusElement(li);
                    this.focussedElement = li;
                }
            }
        }
    };
    SfTreeView.prototype.unselectNode = function (li, e, multiSelect) {
        var eventArgs = this.getSelectEvent(li, 'un-select', e, multiSelect, []);
        this.dotNetRef.invokeMethodAsync('TriggerNodeSelectingEvent', eventArgs);
    };
    SfTreeView.prototype.getSelectEvent = function (currLi, action, e, multiSelect, nodes) {
        // eslint-disable-next-line
        var detail = this.getNodeData(currLi);
        return { action: action, isInteracted: !sf.base.isNullOrUndefined(e), nodeData: detail,
            isMultiSelect: multiSelect, isCtrKey: !sf.base.isNullOrUndefined(e) && e.ctrlKey ? true : false, isShiftKey: !sf.base.isNullOrUndefined(e) && e.shiftKey ? true : false, nodes: nodes };
    };
    SfTreeView.prototype.selectNode = function (li, e, multiSelect) {
        if (sf.base.isNullOrUndefined(li) || (!this.options.allowMultiSelection && this.isActive(li) && !sf.base.isNullOrUndefined(e))) {
            this.setFocusElement(li);
            this.focussedElement = li;
            return;
        }
        var array = [];
        if (this.options.allowMultiSelection && e && e.shiftKey) {
            if (!this.startNode) {
                this.startNode = li;
            }
            var liList = Array.prototype.slice.call(sf.base.selectAll('.' + LISTITEM, this.element));
            var startIndex = liList.indexOf(this.startNode);
            var endIndex = liList.indexOf(li);
            if (startIndex > endIndex) {
                var temp = startIndex;
                startIndex = endIndex;
                endIndex = temp;
            }
            for (var i = startIndex; i <= endIndex; i++) {
                var currNode = liList[i];
                if (sf.base.isVisible(currNode) && !currNode.classList.contains(DISABLE)) {
                    array.push(currNode.getAttribute('data-uid'));
                }
            }
        }
        else {
            this.startNode = li;
        }
        var eventArgs = this.getSelectEvent(li, 'select', e, multiSelect, array);
        this.dotNetRef.invokeMethodAsync('TriggerNodeSelectingEvent', eventArgs);
    };
    SfTreeView.prototype.setFocusElement = function (li) {
        if (!sf.base.isNullOrUndefined(li)) {
            var focusedNode = this.getFocusedNode();
            if (focusedNode) {
                sf.base.removeClass([focusedNode], FOCUS);
            }
            sf.base.addClass([li], FOCUS);
            this.focussedElement = li;
            this.updateIdAttr(focusedNode, li);
        }
    };
    SfTreeView.prototype.updateIdAttr = function (preNode, nextNode) {
        this.element.removeAttribute('aria-activedescendant');
        if (preNode) {
            preNode.removeAttribute('id');
        }
        nextNode.setAttribute('id', this.element.id + '_active');
        this.element.setAttribute('aria-activedescendant', this.element.id + '_active');
    };
    SfTreeView.prototype.getFocusedNode = function () {
        var selectedItem;
        var fNode = this.focussedElement ? this.focussedElement :
            sf.base.select('.' + LISTITEM + '.' + FOCUS, this.element);
        if (sf.base.isNullOrUndefined(fNode)) {
            selectedItem = sf.base.select('.' + LISTITEM, this.element);
        }
        return sf.base.isNullOrUndefined(fNode) ? (sf.base.isNullOrUndefined(selectedItem) ? this.element.firstElementChild : selectedItem) : fNode;
    };
    SfTreeView.prototype.setFullRow = function (isEnabled) {
        (isEnabled ? sf.base.addClass : sf.base.removeClass)([this.element], FULLROWWRAP);
        this.options.fullRowSelect = isEnabled;
    };
    SfTreeView.prototype.isActive = function (li) {
        return li.classList.contains(ACTIVE);
    };
    SfTreeView.prototype.wireClickEvent = function (toBind) {
        if (toBind) {
            // eslint-disable-next-line
            var proxy_4 = this;
            this.touchClickObj = new sf.base.Touch(this.element, {
                tap: function (e) {
                    proxy_4.clickHandler(e);
                }
            });
        }
        else {
            if (this.touchClickObj) {
                this.touchClickObj.destroy();
            }
        }
    };
    SfTreeView.prototype.onMouseOver = function (e) {
        var target = e.target;
        var classList = target.classList;
        var currentLi = sf.base.closest(target, '.' + LISTITEM);
        if (!currentLi || classList.contains(PARENTITEM) || classList.contains(LISTITEM)) {
            this.removeHover();
            return;
        }
        else {
            if (currentLi && !currentLi.classList.contains(DISABLE)) {
                this.setHover(currentLi);
            }
        }
    };
    SfTreeView.prototype.setHover = function (li) {
        if (!li.classList.contains(HOVER)) {
            this.removeHover();
            sf.base.addClass([li], HOVER);
        }
    };
    SfTreeView.prototype.removeHover = function () {
        var hoveredNode = sf.base.selectAll('.' + HOVER, this.element);
        if (hoveredNode && hoveredNode.length) {
            sf.base.removeClass(hoveredNode, HOVER);
        }
    };
    SfTreeView.prototype.checkNode = function (e) {
        var focusedNode = this.getFocusedNode();
        var checkWrap = sf.base.select('.' + CHECKBOXWRAP, focusedNode);
        var isChecked = sf.base.select(' .' + CHECKBOXFRAME, checkWrap).classList.contains(CHECK);
        if (!focusedNode.classList.contains(DISABLE)) {
            if (focusedNode.getElementsByClassName('e-checkbox-disabled').length === 0) {
                this.validateCheckNode(checkWrap, isChecked, focusedNode, e);
            }
        }
    };
    SfTreeView.prototype.openNode = function (toBeOpened, e) {
        var focusedNode = this.getFocusedNode();
        var icon = sf.base.select('div.' + ICON, focusedNode);
        if (toBeOpened) {
            if (!icon) {
                return;
            }
            else if (icon.classList.contains(EXPANDABLE)) {
                this.expandAction(focusedNode, e);
            }
            else {
                this.focusNextNode(focusedNode, true);
            }
        }
        else {
            if (icon && icon.classList.contains(COLLAPSIBLE)) {
                this.collapseAction(focusedNode, e, false);
            }
            else {
                var parentLi = sf.base.closest(sf.base.closest(focusedNode, '.' + PARENTITEM), '.' + LISTITEM);
                if (!parentLi) {
                    return;
                }
                else {
                    if (!parentLi.classList.contains(DISABLE)) {
                        this.setNodeFocus(focusedNode, parentLi);
                        this.navigateToFocus(true);
                    }
                }
            }
        }
    };
    SfTreeView.prototype.getScrollParent = function (node) {
        if (sf.base.isNullOrUndefined(node)) {
            return null;
        }
        return (node.scrollHeight > node.clientHeight) ? node : this.getScrollParent(node.parentElement);
    };
    SfTreeView.prototype.navigateToFocus = function (isUp) {
        var focusNode = this.getFocusedNode().querySelector('.' + TEXTWRAP);
        var pos = focusNode.getBoundingClientRect();
        var parent = this.getScrollParent(this.element);
        if (!sf.base.isNullOrUndefined(parent)) {
            var parentPos = parent.getBoundingClientRect();
            if (pos.bottom > parentPos.bottom) {
                parent.scrollTop += pos.bottom - parentPos.bottom;
            }
            else if (pos.top < parentPos.top) {
                parent.scrollTop -= parentPos.top - pos.top;
            }
        }
        var isVisible$$1 = this.isVisibleInViewport(focusNode);
        if (!isVisible$$1) {
            focusNode.scrollIntoView(isUp);
        }
    };
    SfTreeView.prototype.isVisibleInViewport = function (txtWrap) {
        var pos = txtWrap.getBoundingClientRect();
        return (pos.top >= 0 && pos.left >= 0 && pos.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            pos.right <= (window.innerWidth || document.documentElement.clientWidth));
    };
    SfTreeView.prototype.setNodeFocus = function (preNode, nextNode) {
        sf.base.removeClass([preNode], [HOVER, FOCUS]);
        if (!nextNode.classList.contains(DISABLE)) {
            this.focussedElement = nextNode;
            sf.base.addClass([nextNode], [HOVER, FOCUS]);
            this.updateIdAttr(preNode, nextNode);
        }
    };
    SfTreeView.prototype.focusNextNode = function (li, isTowards) {
        var nextNode = isTowards ? this.getNextNode(li) : this.getPrevNode(li);
        this.setNodeFocus(li, nextNode);
        this.navigateToFocus(!isTowards);
        if (nextNode.classList.contains(DISABLE)) {
            var lastChild = nextNode.lastChild;
            if (nextNode.previousSibling == null && nextNode.classList.contains('e-level-1')) {
                this.focusNextNode(nextNode, true);
            }
            else if (nextNode.nextSibling == null && nextNode.classList.contains('e-node-collapsed')) {
                this.focusNextNode(nextNode, false);
            }
            else if (nextNode.nextSibling == null && lastChild.classList.contains(TEXTWRAP)) {
                this.focusNextNode(nextNode, false);
            }
            else {
                this.focusNextNode(nextNode, isTowards);
            }
        }
    };
    SfTreeView.prototype.shiftKeySelect = function (isTowards, e) {
        if (this.options.allowMultiSelection) {
            var focusedNode = this.getFocusedNode();
            var nextNode = isTowards ? this.getNextNode(focusedNode) : this.getPrevNode(focusedNode);
            this.removeHover();
            this.setFocusElement(nextNode);
            this.focussedElement = nextNode;
            this.toggleSelect(nextNode, e, false);
            this.navigateToFocus(!isTowards);
        }
        else {
            this.navigateNode(isTowards);
        }
    };
    SfTreeView.prototype.updateList = function () {
        this.liList = Array.prototype.slice.call(sf.base.selectAll('.' + LISTITEM, this.element));
    };
    SfTreeView.prototype.getNextNode = function (li) {
        var index = this.liList.indexOf(li);
        var nextNode;
        do {
            index++;
            nextNode = this.liList[index];
            if (sf.base.isNullOrUndefined(nextNode)) {
                return li;
            }
        } while (!sf.base.isVisible(nextNode));
        return nextNode;
    };
    SfTreeView.prototype.getPrevNode = function (li) {
        var index = this.liList.indexOf(li);
        var prevNode;
        do {
            index--;
            prevNode = this.liList[index];
            if (sf.base.isNullOrUndefined(prevNode)) {
                return li;
            }
        } while (!sf.base.isVisible(prevNode));
        return prevNode;
    };
    SfTreeView.prototype.getRootNode = function () {
        var index = 0;
        var rootNode;
        do {
            rootNode = this.liList[index];
            index++;
        } while (!sf.base.isVisible(rootNode));
        return rootNode;
    };
    SfTreeView.prototype.getEndNode = function () {
        var index = this.liList.length - 1;
        var endNode;
        do {
            endNode = this.liList[index];
            index--;
        } while (!sf.base.isVisible(endNode));
        return endNode;
    };
    SfTreeView.prototype.navigateNode = function (isTowards) {
        this.focusNextNode(this.getFocusedNode(), isTowards);
    };
    SfTreeView.prototype.updateOldText = function (oldText) {
        this.oldText = oldText;
    };
    SfTreeView.prototype.onPropertyChanged = function (newProp) {
        for (var _i = 0, _a = Object.keys(newProp); _i < _a.length; _i++) {
            var prop = _a[_i];
            switch (prop) {
                case SHOWCHECKBOX:
                    this.options.showCheckBox = newProp.showCheckBox;
                    break;
                case ALLOWDRAGANDDROP:
                    this.setDragAndDrop(newProp.allowDragAndDrop);
                    break;
                case ALLOWTEXTWRAP:
                    this.options.allowTextWrap = newProp.allowTextWrap;
                    this.setTextWrap();
                    break;
                case ALLOWEDITING:
                    this.wireEditingEvents(newProp.allowEditing);
                    break;
                case SETDISABLED:
                    this.options.disabled = newProp.disabled;
                    this.checkAllDisabled(newProp.disabled);
                    break;
                case DRAGAREA:
                    this.setDragArea(newProp.dropArea);
                    break;
                case CSSCLASS:
                    this.setCssClass(newProp.cssClass);
                    break;
                case FULLROWSELECT:
                    this.setFullRow(newProp.fullRowSelect);
                    break;
                case EXPANDONTYPE:
                    this.options.expandOnType = newProp.expandOnType;
                    break;
                case ENABLERTL:
                    this.options.enableRtl = newProp.enableRtl;
                    this.setEnableRtl();
                    break;
                case ANIMATION:
                    this.options.animation = newProp.animation;
            }
        }
    };
    SfTreeView.prototype.navigateRootNode = function (isBackwards) {
        var focusedNode = this.getFocusedNode();
        var rootNode = isBackwards ? this.getRootNode() : this.getEndNode();
        if (!rootNode.classList.contains(DISABLE)) {
            this.setNodeFocus(focusedNode, rootNode);
            this.navigateToFocus(isBackwards);
        }
    };
    SfTreeView.prototype.selectGivenNodes = function (sNodes) {
        for (var i = 0; i < sNodes.length; i++) {
            if (!sNodes[i].classList.contains(DISABLE)) {
                this.selectNode(sNodes[i], null, true);
            }
        }
    };
    SfTreeView.prototype.beginEdit = function (node) {
        var nodeElement = this.element.querySelector('[data-uid="' + node + '"]');
        if (sf.base.isNullOrUndefined(nodeElement) || this.options.disabled) {
            return;
        }
        this.createTextbox(nodeElement, null);
    };
    SfTreeView.prototype.ensureVisible = function (node) {
        var liEle = this.element.querySelector('[data-uid="' + node + '"]');
        if (sf.base.isNullOrUndefined(liEle)) {
            return;
        }
        var parents = this.parents(liEle, '.' + LISTITEM);
        var parentNodeId = [];
        for (var i = 0; i < parents.length; i++) {
            parentNodeId.push(parents[i].getAttribute('data-uid'));
        }
        this.dotNetRef.invokeMethodAsync('UpdateExpandedNode', parentNodeId);
        setTimeout(function () { liEle.scrollIntoView(true); }, 450);
    };
    SfTreeView.prototype.nodeCollapse = function (id) {
        var liElement = this.element.querySelector('[data-uid="' + id + '"]');
        this.collapseAction(liElement, null, false);
    };
    SfTreeView.prototype.nodeExpand = function (id) {
        var liElement = this.element.querySelector('[data-uid="' + id + '"]');
        this.expandAction(liElement, null);
    };
    SfTreeView.prototype.KeyActionHandler = function (e, nodeId) {
        this.updateList();
        var nodeElement = this.element.querySelector('[data-uid="' + nodeId + '"]');
        var focusedNode = sf.base.isNullOrUndefined(nodeElement) ? this.getFocusedNode() : nodeElement;
        switch (e.action) {
            case 'space':
                if (this.options.showCheckBox) {
                    this.checkNode(this.keyAction);
                }
                break;
            case 'moveRight':
                this.keyBoardAction = true;
                this.openNode(!this.options.enableRtl, this.keyAction);
                break;
            case 'moveLeft':
                this.keyBoardAction = true;
                this.openNode(this.options.enableRtl, this.keyAction);
                break;
            case 'shiftDown':
                this.shiftKeySelect(true, this.keyAction);
                break;
            case 'moveDown':
            case 'ctrlDown':
            case 'csDown':
                this.navigateNode(true);
                break;
            case 'shiftUp':
                this.shiftKeySelect(false, this.keyAction);
                break;
            case 'moveUp':
            case 'ctrlUp':
            case 'csUp':
                this.navigateNode(false);
                break;
            case 'home':
            case 'shiftHome':
            case 'ctrlHome':
            case 'csHome':
                this.navigateRootNode(true);
                break;
            case 'end':
            case 'shiftEnd':
            case 'ctrlEnd':
            case 'csEnd':
                this.navigateRootNode(false);
                break;
            case 'enter':
            case 'ctrlEnter':
            case 'shiftEnter':
            case 'csEnter':
                this.toggleSelect(focusedNode, this.keyAction, false);
                break;
            case 'f2':
                if (this.options.allowEditing && !focusedNode.classList.contains(DISABLE)) {
                    this.createTextbox(focusedNode, this.keyAction);
                }
                break;
            case 'ctrlA':
                if (this.options.allowMultiSelection) {
                    var sNodes = sf.base.selectAll('.' + LISTITEM + ':not(.' + ACTIVE + ')', this.element);
                    this.selectGivenNodes(sNodes);
                }
                break;
        }
        // eslint-disable-next-line
        var _this = this;
        // eslint-disable-next-line
        setTimeout(function () {
            if (_this.keyBoardAction) {
                _this.setHover(_this.getFocusedNode());
                _this.keyBoardAction = false;
            }
        }, 100);
    };
    return SfTreeView;
}());
// eslint-disable-next-line
var TreeView = {
    initialize: function (element, options, dotnetRef) {
        var instance = new SfTreeView(element, options, dotnetRef);
        instance.render();
        if (this.valid(element) && element.blazor__instance.options.allowTextWrap) {
            element.blazor__instance.updateWrap();
        }
        if (document.getElementById(element.id)) {
            instance.dotNetRef.invokeMethodAsync('CreatedEvent', null);
        }
    },
    updateTextWrap: function (element) {
        if (this.valid(element) && element.blazor__instance.options.allowTextWrap) {
            element.blazor__instance.updateWrap();
        }
    },
    dataSourceChanged: function (element) {
        element.blazor__instance.unWireEvents();
        element.blazor__instance.wireEvents();
    },
    collapseAction: function (element, nodeId) {
        if (this.valid(element)) {
            // eslint-disable-next-line
            var currentLi = element.querySelector('[data-uid="' + nodeId + '"]');
            element.blazor__instance.collapseAction(currentLi, null, false);
        }
    },
    expandAction: function (element, nodeId) {
        if (this.valid(element)) {
            // eslint-disable-next-line
            var currentLi = element.querySelector('[data-uid="' + nodeId + '"]');
            element.blazor__instance.expandAction(currentLi, null);
        }
    },
    expandedNode: function (element, args) {
        if (this.valid(element)) {
            element.blazor__instance.expandedNode(args);
        }
    },
    collapsedNode: function (element, args) {
        if (this.valid(element)) {
            element.blazor__instance.collapsedNode(args);
        }
    },
    KeyActionHandler: function (element, args, nodeId) {
        if (this.valid(element)) {
            element.blazor__instance.KeyActionHandler(args, nodeId);
        }
    },
    setMultiSelect: function (element, args) {
        if (this.valid(element)) {
            element.blazor__instance.setMultiSelect(args);
        }
    },
    dragStartActionContinue: function dragStartActionContinue(element, cancel) {
        if (this.valid(element)) {
            element.blazor__instance.dragStartActionContinue(cancel);
        }
    },
    dragNodeStop: function dragNodeStop(element, args) {
        if (this.valid(element)) {
            element.blazor__instance.dragNodeStop(args);
        }
    },
    nodeDragging: function nodeDragging(element, args) {
        if (this.valid(element)) {
            element.blazor__instance.nodeDragging(args);
        }
    },
    setFocus: function setFocus(element, liElement) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(liElement)) {
            var inputEle = (document.getElementById(element.id));
            inputEle.focus();
            inputEle.setSelectionRange(0, inputEle.value.length);
        }
    },
    updateOldText: function updateOldText(element, text) {
        if (this.valid(element)) {
            element.blazor__instance.updateOldText(text);
        }
    },
    updateSpinnerClass: function updateSpinnerClass(element) {
        if (this.valid(element)) {
            element.blazor__instance.updateSpinnerClass();
        }
    },
    onPropertyChanged: function onPropertyChanged(element, properties) {
        if (this.valid(element)) {
            element.blazor__instance.onPropertyChanged(properties);
        }
    },
    // eslint-disable-next-line
    valid: function (element) {
        return (element && element.blazor__instance);
    },
    beginEdit: function beginEdit(element, node) {
        if (this.valid(element)) {
            element.blazor__instance.beginEdit(node);
        }
    },
    ensureVisible: function ensureVisible(element, node) {
        if (this.valid(element)) {
            element.blazor__instance.ensureVisible(node);
        }
    },
    nodeCollapse: function nodeCollapse(element, id) {
        if (this.valid(element)) {
            element.blazor__instance.nodeCollapse(id);
        }
    },
    nodeExpand: function nodeCollapse(element, id) {
        if (this.valid(element)) {
            element.blazor__instance.nodeExpand(id);
        }
    }
};

return TreeView;

}());

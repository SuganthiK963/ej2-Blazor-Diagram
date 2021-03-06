window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Kanban = (function () {
'use strict';

/**
 * Kanban Constants
 */
var ROOT_CLASS = 'e-kanban';
var DEVICE_CLASS = 'e-device';
var ICON_CLASS = 'e-icons';
var CARD_CLASS = 'e-card';
var HEADER_CLASS = 'e-kanban-header';
var CONTENT_CLASS = 'e-kanban-content';
var HEADER_ICON_CLASS = 'e-header-icon';
var CLONED_CARD_CLASS = 'e-cloned-card';
var TARGET_MULTI_CLONE_CLASS = 'e-target-multi-clone';
var DROPPED_CLONE_CLASS = 'e-target-dropped-clone';
var CARD_SELECTION_CLASS = 'e-selection';
var CONTENT_ROW_CLASS = 'e-content-row';
var DRAGGED_CLONE_CLASS = 'e-target-dragged-clone';
var DRAGGED_CARD_CLASS = 'e-kanban-dragged-card';
var SWIMLANE_HEADER_CLASS = 'e-swimlane-header';
var SWIMLANE_ROW_CLASS = 'e-swimlane-row';
var SWIMLANE_ROW_EXPAND_CLASS = 'e-swimlane-row-expand';
var SWIMLANE_ROW_COLLAPSE_CLASS = 'e-swimlane-row-collapse';
var CONTENT_CELLS_CLASS = 'e-content-cells';
var CARD_CONTAINER_CLASS = 'e-card-container';
var COLLAPSED_CLASS = 'e-collapsed';
var DROPPING_CLASS = 'e-dropping';
var MULTI_COLUMN_KEY_CLASS = 'e-column-key';
var MULTI_ACTIVE_CLASS = 'e-multi-active';
var SHOW_ADD_BUTTON = 'e-show-add-button';
var MULTI_CARD_CONTAINER_CLASS = 'e-multi-card-container';
var LIMITS_CLASS = 'e-limits';
var SHOW_ADD_FOCUS = 'e-show-add-focus';

var POPUP_HEADER_CLASS = 'e-popup-header';
var POPUP_CONTENT_CLASS = 'e-popup-content';
var CLOSE_CLASS = 'e-close';
var CLOSE_ICON_CLASS = 'e-close-icon';

var bottomSpace = 25;
var DRAGGABLE_CLASS = 'e-draggable';
var DROPPABLE_CLASS = 'e-droppable';
var DRAG_CLASS = 'e-drag';
var DROP_CLASS = 'e-drop';
var DISABLED_CLASS = 'e-disabled';
var CLS_RTL = 'e-rtl';

var SWIMLANE_OVERLAY_CLASS = 'e-swimlane-overlay';
var SWIMLANE_CONTENT_CLASS = 'e-swimlane-content';
var SWIMLANE_RESOURCE_CLASS = 'e-swimlane-resource';

var WINDOW_WIDTH = 'WindowWidth';
var TOOLTIP_TEXT_CLASS = 'e-tooltip-text';
var POPUP_WRAPPER_CLASS = 'e-mobile-popup-wrapper';
var FROZEN_SWIMLANE_ROW_CLASS = 'e-frozen-swimlane-row';
var FROZEN_ROW_CLASS = 'e-frozen-row';

/* eslint-disable @typescript-eslint/no-explicit-any */
/**
 * Drag and Drop module
 */
var DragAndDrop = /** @class */ (function () {
    function DragAndDrop(parent) {
        this.parent = parent;
        this.dragObj = {
            element: null, cloneElement: null, instance: null, targetClone: null, draggedClone: null, targetCloneMulti: null,
            selectedCards: [], pageX: 0, pageY: 0, navigationInterval: null, cardDetails: [], modifiedData: []
        };
        this.isDragging = false;
        this.dragEdges = { left: false, right: false, top: false, bottom: false };
        this.isExternalDrop = false;
    }
    DragAndDrop.prototype.wireDragEvents = function (element) {
        var dragContainment;
        if (!this.parent.element != null && this.parent.externalDropId && this.parent.externalDropId.length === 0) {
            dragContainment = this.parent.element.querySelector('.' + CONTENT_CLASS);
        }
        this.dragObj.instance = new sf.base.Draggable(element, {
            clone: true,
            enableTapHold: this.parent.isAdaptive,
            enableTailMode: true,
            cursorAt: { top: -10, left: -10 },
            dragArea: dragContainment,
            dragStart: this.dragStart.bind(this),
            drag: this.drag.bind(this),
            dragStop: this.dragStop.bind(this),
            enableAutoScroll: false,
            helper: this.dragHelper.bind(this)
        });
    };
    DragAndDrop.prototype.dragHelper = function (e) {
        this.dragObj.element = sf.base.closest(e.sender.target, '.' + CARD_CLASS);
        if (sf.base.isNullOrUndefined(this.dragObj.element)) {
            return null;
        }
        this.dragObj.element.style.width = sf.base.formatUnit(this.dragObj.element.offsetWidth);
        var cloneContainer = this.dragObj.element.cloneNode(true);
        this.dragObj.cloneElement = cloneContainer;
        sf.base.addClass([this.dragObj.cloneElement], CLONED_CARD_CLASS);
        this.dragObj.element.parentElement.appendChild(this.dragObj.cloneElement);
        this.dragObj.targetCloneMulti = sf.base.createElement('div', { className: TARGET_MULTI_CLONE_CLASS });
        this.dragObj.targetClone = sf.base.createElement('div', {
            className: DROPPED_CLONE_CLASS,
            styles: 'width:100%;height:' + sf.base.formatUnit(this.dragObj.element.offsetHeight)
        });
        this.dragObj.modifiedData = [];
        return this.dragObj.cloneElement;
    };
    DragAndDrop.prototype.dragStart = function (e) {
        var _this = this;
        this.dragObj.selectedCards = this.dragObj.element;
        if (this.dragObj.element.classList.contains(CARD_SELECTION_CLASS)) {
            var className = '.' + CARD_CLASS + '.' + CARD_SELECTION_CLASS + ':not(.' + CLONED_CARD_CLASS + ')';
            var closestEle = sf.base.closest(this.dragObj.element, '.' + CONTENT_ROW_CLASS);
            this.dragObj.selectedCards = [].slice.call(closestEle.querySelectorAll(className));
        }
        var cardId = [];
        if (this.dragObj.selectedCards instanceof HTMLElement) {
            var card = this.dragObj.selectedCards.getAttribute('data-id');
            cardId.push(card);
        }
        else {
            this.dragObj.selectedCards.forEach(function (element) {
                cardId.push(element.getAttribute('data-id'));
            });
        }
        var left = this.getXYValue(e, 'X');
        var top = this.getXYValue(e, 'Y');
        this.dragObj.cloneElement.style.display = 'none';
        // @ts-ignore-start
        this.parent.dotNetRef.invokeMethodAsync('DragStart', cardId, left, top).then(function (dragArgs) {
            // @ts-ignore-end
            if (dragArgs.cancel) {
                _this.removeElement(_this.dragObj.cloneElement);
                _this.dragObj.instance.intDestroy(e);
                _this.dragObj.element = null;
                _this.dragObj.targetClone = null;
                _this.dragObj.draggedClone = null;
                _this.dragObj.cloneElement = null;
                _this.dragObj.targetCloneMulti = null;
                return;
            }
            _this.dragObj.cloneElement.style.display = 'block';
            e.bindEvents(e.dragElement);
            if (_this.dragObj.element.classList.contains(CARD_SELECTION_CLASS)) {
                _this.dragObj.selectedCards.forEach(function (element) { _this.draggedClone(element); });
                if (_this.dragObj.selectedCards.length > 1) {
                    _this.dragObj.cloneElement.innerHTML = '';
                    var drag = sf.base.createElement('div', {
                        className: 'e-multi-card-text',
                        innerHTML: _this.dragObj.selectedCards.length + ' Cards'
                    });
                    _this.dragObj.cloneElement.appendChild(drag);
                    sf.base.classList(_this.dragObj.cloneElement, ['e-multi-card-clone'], [CARD_SELECTION_CLASS]);
                    _this.dragObj.cloneElement.style.width = '90px';
                }
            }
            else {
                _this.draggedClone(_this.dragObj.element);
            }
            sf.base.EventHandler.add(document.body, 'keydown', _this.keydownHandler, _this);
        });
    };
    DragAndDrop.prototype.drag = function (e) {
        var _this = this;
        if (!e.target) {
            return;
        }
        var cardElement = sf.base.closest(e.target, '.' + CARD_CLASS);
        var target = cardElement || e.target;
        var selector = '.' + CONTENT_ROW_CLASS + ':not(.' + SWIMLANE_ROW_CLASS + ') .' + CONTENT_CELLS_CLASS
            + '.' + DROPPABLE_CLASS;
        var contentCell = sf.base.closest(target, selector);
        this.externalDrop(target, 'drag');
        this.target = this.parent.isExternalKanbanDrop ? this.externalTarget : this.parent.element;
        this.calculateArgs(e);
        if (contentCell && document.body.style.cursor !== 'not-allowed') {
            var targetKey = this.getColumnKey(contentCell);
            var keys = targetKey.split(',');
            this.multiCloneRemove();
            var isDrag = (targetKey === this.getColumnKey(sf.base.closest(this.dragObj.draggedClone, '.' + CONTENT_CELLS_CLASS)))
                ? true : false;
            if (keys.length === 1 || isDrag) {
                if (target.classList.contains(CARD_CLASS) || target.classList.contains(DRAGGED_CLONE_CLASS)) {
                    var element = target.classList.contains(DRAGGED_CLONE_CLASS) ?
                        (target.previousElementSibling.classList.contains(DRAGGED_CARD_CLASS) ? null : target.previousElementSibling)
                        : target.previousElementSibling;
                    var insertClone = 'afterend';
                    if (sf.base.isNullOrUndefined(element)) {
                        var pageY = target.classList.contains(DRAGGED_CLONE_CLASS) ? (this.dragObj.pageY / 2) :
                            this.dragObj.pageY;
                        var height = target.classList.contains(DRAGGED_CLONE_CLASS) ? target.offsetHeight :
                            (target.offsetHeight / 2);
                        if ((pageY - (this.target.getBoundingClientRect().top + target.offsetTop)) < height) {
                            insertClone = 'beforebegin';
                        }
                    }
                    target.insertAdjacentElement(insertClone, this.dragObj.targetClone);
                }
                else if (target.classList.contains(CONTENT_CELLS_CLASS) && !sf.base.closest(target, '.' + SWIMLANE_ROW_CLASS)) {
                    target.querySelector('.' + CARD_CONTAINER_CLASS).appendChild(this.dragObj.targetClone);
                }
                else if (target.classList.contains(CARD_CONTAINER_CLASS) && !sf.base.closest(target, '.' + SWIMLANE_ROW_CLASS)
                    && contentCell.querySelectorAll('.' + CARD_CLASS).length === 0) {
                    target.appendChild(this.dragObj.targetClone);
                }
            }
            else if (keys.length > 1 && contentCell.classList.contains(DROPPING_CLASS)) {
                this.multiCloneCreate(keys, contentCell);
            }
        }
        if (this.target.querySelectorAll('.' + DROPPING_CLASS).length === 0) {
            this.cellDropping();
        }
        var multiKeyTarget = sf.base.closest(target, '.' + MULTI_COLUMN_KEY_CLASS);
        if (multiKeyTarget) {
            var columnKeys = [].slice.call(this.target.querySelectorAll('.' + MULTI_COLUMN_KEY_CLASS + ':not(.' +
                DISABLED_CLASS + ')')).filter(function (element) { return _this.getColumnKey(element) === _this.getColumnKey(multiKeyTarget); });
            if (columnKeys.length > 0) {
                sf.base.addClass(columnKeys, MULTI_ACTIVE_CLASS);
                if (columnKeys[0].previousElementSibling) {
                    sf.base.addClass([columnKeys[0].previousElementSibling], 'e-multi-bottom-border');
                }
            }
        }
        document.body.style.cursor = (contentCell && contentCell.classList.contains(DROPPING_CLASS)) ? '' : 'not-allowed';
        if (cardElement && !(sf.base.closest(cardElement, '.' + CONTENT_CELLS_CLASS)).classList.contains(DROPPING_CLASS)) {
            cardElement.style.cursor = 'not-allowed';
            document.body.style.cursor = 'not-allowed';
        }
        if (this.isExternalDrop) {
            this.removeElement(this.dragObj.targetClone);
        }
        if (this.isExternalDrop && document.body.style.cursor === 'not-allowed') {
            document.body.style.cursor = '';
        }
        if (document.body.style.cursor === 'not-allowed') {
            this.removeElement(this.dragObj.targetClone, this.target);
            this.multiCloneRemove();
        }
        this.updateScrollPosition();
        this.parent.isExternalKanbanDrop = false;
        this.isExternalDrop = false;
    };
    DragAndDrop.prototype.dragStop = function (e) {
        var contentCell = sf.base.closest(this.dragObj.targetClone, '.' + CONTENT_CELLS_CLASS);
        var columnKey;
        var dropIndex = 0;
        sf.base.EventHandler.remove(document.body, 'keydown', this.keydownHandler);
        this.externalDrop(e.target, 'dragStop');
        if (this.dragObj.targetClone.parentElement) {
            var className = '.' + CARD_CLASS + ':not(.' + DRAGGED_CARD_CLASS + '),.' + DROPPED_CLONE_CLASS;
            var element = [].slice.call(this.dragObj.targetClone.parentElement.querySelectorAll(className));
            dropIndex = element.indexOf(this.dragObj.targetClone);
        }
        if (this.target && this.target.querySelector('.' + TARGET_MULTI_CLONE_CLASS)) {
            columnKey = sf.base.closest(e.target, '.' + MULTI_COLUMN_KEY_CLASS + ':not(.' + DISABLED_CLASS + ')');
        }
        var left = this.getXYValue(e, 'X');
        var top = this.getXYValue(e, 'Y');
        var cardId = [];
        if (this.dragObj.selectedCards instanceof HTMLElement) {
            var card = this.dragObj.selectedCards.getAttribute('data-id');
            cardId.push(card);
        }
        else {
            this.dragObj.selectedCards.forEach(function (element) {
                cardId.push(element.getAttribute('data-id'));
            });
        }
        if (contentCell || columnKey) {
            var cardStatus = void 0;
            if (contentCell) {
                cardStatus = this.getColumnKey(contentCell);
            }
            else {
                cardStatus = this.getColumnKey(columnKey);
                contentCell = sf.base.closest(columnKey, '.' + CONTENT_CELLS_CLASS);
            }
            var swimData = void 0;
            if (this.parent.swimlaneSettings.keyField && this.parent.swimlaneSettings.allowDragAndDrop) {
                var prev = sf.base.closest(contentCell, '.' + CONTENT_ROW_CLASS).previousElementSibling;
                swimData = this.getColumnKey(prev);
            }
            var prevele = false;
            var cloneElement = void 0;
            var cloneElementId = "";
            if (this.parent.sortSettings.field && this.parent.sortSettings.sortBy === 'Index') {
                cloneElement = this.parent.sortSettings.direction === 'Ascending' ?
                    this.dragObj.targetClone.previousElementSibling : this.dragObj.targetClone.nextElementSibling;
                if (cloneElement && !cloneElement.classList.contains(DRAGGED_CARD_CLASS) && !cloneElement.classList.contains(CLONED_CARD_CLASS)
                    && !cloneElement.classList.contains(DRAGGED_CLONE_CLASS)) {
                    prevele = true;
                }
                else if (this.dragObj.targetClone.nextElementSibling && this.parent.sortSettings.direction === 'Ascending') {
                    cloneElement = this.dragObj.targetClone.nextElementSibling;
                }
                else if (this.dragObj.targetClone.previousElementSibling && this.parent.sortSettings.direction === 'Descending') {
                    cloneElement = this.dragObj.targetClone.previousElementSibling;
                }
            }
            if (cloneElement) {
                cloneElementId = cloneElement.getAttribute('data-id');
            }
            this.parent.dotNetRef.invokeMethodAsync('DragStop', cardId, cardStatus, swimData, dropIndex, prevele, cloneElementId, this.parent.isExternalKanbanDrop, left, top);
        }
        else if (this.isExternalDrop) {
            this.parent.dotNetRef.invokeMethodAsync('DragStop', cardId, "", "", 0, false, "", true, left, top);
        }
        this.dragStopClear();
    };
    DragAndDrop.prototype.dragStopClear = function () {
        this.removeElement(this.dragObj.draggedClone);
        this.removeElement(this.dragObj.targetClone, this.target);
        this.removeElement(this.dragObj.cloneElement);
        var dragMultiClone = [].slice.call(this.parent.element.querySelectorAll('.' + DRAGGED_CLONE_CLASS));
        dragMultiClone.forEach(function (clone) { return sf.base.remove(clone); });
        this.dragObj.element.style.removeProperty('width');
        this.multiCloneRemove();
        if (this.dragObj.selectedCards instanceof HTMLElement) {
            sf.base.removeClass([this.dragObj.selectedCards], DRAGGED_CARD_CLASS);
        }
        else {
            sf.base.removeClass(this.dragObj.selectedCards, DRAGGED_CARD_CLASS);
        }
        clearInterval(this.dragObj.navigationInterval);
        this.dragObj.navigationInterval = null;
        if (document.body.style.cursor === 'not-allowed') {
            document.body.style.cursor = '';
        }
        var styleCards = [].slice.call(this.parent.element.querySelectorAll('.' + CARD_CLASS + '[style]'));
        styleCards.forEach(function (styleCard) { styleCard.style.cursor = ''; });
        var className = '.' + CONTENT_ROW_CLASS + ':not(.' + SWIMLANE_ROW_CLASS + ')';
        var cells = [].slice.call(this.parent.element.querySelectorAll(className + ' .' + CONTENT_CELLS_CLASS));
        cells.forEach(function (cell) { return sf.base.removeClass([cell], DROPPING_CLASS); });
        this.dragObj.cardDetails = this.dragObj.modifiedData = [];
        this.isDragging = false;
        this.parent.isExternalKanbanDrop = false;
        this.target = null;
        this.externalTarget = null;
    };
    DragAndDrop.prototype.keydownHandler = function (e) {
        if (e.code === 'Escape' && this.dragObj.cloneElement) {
            sf.base.EventHandler.remove(this.dragObj.cloneElement, 'keydown', this.keydownHandler);
            this.dragObj.element.removeAttribute('aria-grabbed');
            this.dragStopClear();
        }
    };
    DragAndDrop.prototype.draggedClone = function (element) {
        this.dragObj.draggedClone = sf.base.createElement('div', {
            className: DRAGGED_CLONE_CLASS,
            styles: 'width:' + sf.base.formatUnit(element.offsetWidth - 1) + ';height:' + sf.base.formatUnit(element.offsetHeight)
        });
        element.insertAdjacentElement('afterend', this.dragObj.draggedClone);
        sf.base.addClass([element], DRAGGED_CARD_CLASS);
    };
    DragAndDrop.prototype.calculateArgs = function (e) {
        var eventArgs = this.getPageCoordinates(e);
        this.dragObj.pageY = eventArgs.pageY;
        this.dragObj.pageX = eventArgs.pageX;
        this.isDragging = true;
    };
    DragAndDrop.prototype.getPageCoordinates = function (e) {
        var eventArgs = e.event;
        return eventArgs && eventArgs.changedTouches ? eventArgs.changedTouches[0] : e.changedTouches ? e.changedTouches[0] :
            eventArgs || e;
    };
    DragAndDrop.prototype.getColumnKey = function (target) {
        if (target && target.getAttribute('data-key')) {
            return target.getAttribute('data-key').trim();
        }
        return '';
    };
    DragAndDrop.prototype.multiCloneRemove = function () {
        if (this.target) {
            var cloneMulti = [].slice.call(this.target.querySelectorAll('.' + TARGET_MULTI_CLONE_CLASS));
            if (cloneMulti.length > 0) {
                var columnKey = [].slice.call(this.target.querySelectorAll('.' + MULTI_COLUMN_KEY_CLASS));
                columnKey.forEach(function (node) { return sf.base.remove(node); });
                cloneMulti.forEach(function (node) {
                    var cell = sf.base.closest(node, '.' + CONTENT_CELLS_CLASS);
                    if (cell) {
                        cell.style.borderStyle = '';
                        if (cell.querySelector('.' + SHOW_ADD_BUTTON)) {
                            sf.base.removeClass([cell.querySelector('.' + SHOW_ADD_BUTTON)], MULTI_CARD_CONTAINER_CLASS);
                        }
                        sf.base.removeClass([cell.querySelector('.' + CARD_CONTAINER_CLASS)], MULTI_CARD_CONTAINER_CLASS);
                    }
                });
                this.removeElement(this.dragObj.targetCloneMulti, this.target);
            }
        }
    };
    DragAndDrop.prototype.removeElement = function (element, externalElement) {
        externalElement = externalElement ? externalElement : this.parent.element;
        if (element && externalElement) {
            if (externalElement.getElementsByClassName(element.className).length > 0) {
                sf.base.remove(element);
            }
        }
    };
    DragAndDrop.prototype.externalDrop = function (target, state) {
        var _this = this;
        if (this.parent.externalDropId) {
            this.parent.externalDropId.forEach(function (externalDropId) {
                _this.externalTarget = sf.base.closest(target, externalDropId);
                if (_this.externalTarget) {
                    if (_this.externalTarget.classList.contains('e-kanban')) {
                        _this.parent.isExternalKanbanDrop = true;
                        var className = '.' + CONTENT_ROW_CLASS + ':not(.' + SWIMLANE_ROW_CLASS +
                            '):not(.' + COLLAPSED_CLASS + ') .' + CONTENT_CELLS_CLASS;
                        var cells = [].slice.call(_this.externalTarget.querySelectorAll(className));
                        if (state === 'drag') {
                            sf.base.addClass(cells, DROPPING_CLASS);
                        }
                        else {
                            sf.base.removeClass(cells, DROPPING_CLASS);
                        }
                    }
                    else {
                        _this.isExternalDrop = true;
                    }
                }
            });
        }
    };
    DragAndDrop.prototype.multiCloneCreate = function (keys, contentCell) {
        var offsetHeight = contentCell.offsetHeight;
        var limitEle = contentCell.querySelector('.' + LIMITS_CLASS);
        if (limitEle) {
            offsetHeight -= limitEle.offsetHeight;
        }
        this.dragObj.targetCloneMulti.style.height = sf.base.formatUnit(offsetHeight);
        if (contentCell.querySelector('.' + SHOW_ADD_BUTTON)) {
            sf.base.addClass([contentCell.querySelector('.' + SHOW_ADD_BUTTON)], MULTI_CARD_CONTAINER_CLASS);
        }
        sf.base.addClass([contentCell.querySelector('.' + CARD_CONTAINER_CLASS)], MULTI_CARD_CONTAINER_CLASS);
        contentCell.querySelector('.' + CARD_CONTAINER_CLASS).style.height = 'auto';
        contentCell.style.borderStyle = 'none';
        this.removeElement(this.dragObj.targetClone);
        for (var _i = 0, keys_1 = keys; _i < keys_1.length; _i++) {
            var key = keys_1[_i];
            var dragCell = sf.base.closest(this.dragObj.draggedClone, '.' + CONTENT_CELLS_CLASS);
            var transition = this.parent.transition[dragCell.cellIndex].transitionColumn;
            var allowTransition = this.allowedTransition(this.dragObj.element.getAttribute('data-key'), key, transition);
            var name_1 = allowTransition ? '' : ' ' + DISABLED_CLASS;
            var colKey = sf.base.createElement('div', {
                className: MULTI_COLUMN_KEY_CLASS + name_1,
                attrs: { 'data-key': key.trim() }
            });
            var text = sf.base.createElement('div', { className: 'e-text', innerHTML: key.trim() });
            contentCell.appendChild(this.dragObj.targetCloneMulti).appendChild(colKey).appendChild(text);
            colKey.style.cursor = allowTransition ? '' : 'not-allowed';
            colKey.style.lineHeight = colKey.style.height = sf.base.formatUnit((offsetHeight / keys.length));
            text.style.top = sf.base.formatUnit((offsetHeight / 2) - (text.offsetHeight / 2));
        }
    };
    DragAndDrop.prototype.allowedTransition = function (currentCardKey, targetCardKey, allowedKey) {
        var allowTransition = true;
        var targetKey = targetCardKey.split(',');
        for (var i = 0; i < targetKey.length; i++) {
            if (currentCardKey === targetKey[i].trim()) {
                return true;
            }
            if (allowedKey) {
                if (allowedKey.length === 1 && allowedKey[0].length === 0) {
                    return true;
                }
                for (var j = 0; j < allowedKey.length; j++) {
                    if (targetKey[i].trim() === allowedKey[j].trim()) {
                        return true;
                    }
                    else {
                        allowTransition = false;
                    }
                }
            }
        }
        return allowTransition;
    };
    DragAndDrop.prototype.cellDropping = function () {
        var _this = this;
        var dragCell = sf.base.closest(this.dragObj.draggedClone, '.' + CONTENT_CELLS_CLASS);
        var dragRow = sf.base.closest(this.dragObj.draggedClone, '.' + CONTENT_ROW_CLASS);
        this.addDropping(dragRow, dragCell);
        if (dragCell && dragCell.classList.contains(DROP_CLASS)) {
            sf.base.addClass([dragCell], DROPPING_CLASS);
        }
        if (this.parent.swimlaneSettings.keyField && this.parent.swimlaneSettings.allowDragAndDrop) {
            var className = '.' + CONTENT_ROW_CLASS + ':not(.' + SWIMLANE_ROW_CLASS + '):not(.' + COLLAPSED_CLASS + ')';
            var rows = [].slice.call(this.target.querySelectorAll(className));
            [].slice.call(rows).forEach(function (row) {
                if (dragRow !== row) {
                    _this.addDropping(row, dragCell);
                }
            });
        }
    };
    DragAndDrop.prototype.addDropping = function (dragRow, dragCell) {
        var _this = this;
        if (dragCell && dragRow) {
            [].slice.call(dragRow.children).forEach(function (cell) {
                var transition = _this.parent.transition[dragCell.cellIndex].transitionColumn;
                if (cell !== dragCell && cell.classList.contains(DROP_CLASS) &&
                    _this.allowedTransition(dragCell.getAttribute('data-key'), cell.getAttribute('data-key'), transition)) {
                    sf.base.addClass([cell], DROPPING_CLASS);
                }
            });
        }
    };
    DragAndDrop.prototype.updateScrollPosition = function () {
        var _this = this;
        if (sf.base.isNullOrUndefined(this.dragObj.navigationInterval)) {
            this.dragObj.navigationInterval = window.setInterval(function () { _this.autoScroll(); }, 100);
        }
    };
    DragAndDrop.prototype.autoScrollValidation = function () {
        var pageY = this.dragObj.pageY;
        var pageX = this.dragObj.pageX;
        var autoScrollDistance = 30;
        var dragEdges = { left: false, right: false, top: false, bottom: false };
        var viewBoundaries = this.target.querySelector('.' + CONTENT_CLASS).getBoundingClientRect();
        if ((pageY < viewBoundaries.top + autoScrollDistance + window.pageYOffset) &&
            (pageY > viewBoundaries.top + window.pageYOffset)) {
            dragEdges.top = true;
        }
        if ((pageY > (viewBoundaries.bottom - autoScrollDistance) + window.pageYOffset) &&
            (pageY < viewBoundaries.bottom + window.pageYOffset)) {
            dragEdges.bottom = true;
        }
        if ((pageX < viewBoundaries.left + autoScrollDistance + window.pageXOffset) &&
            (pageX > viewBoundaries.left + window.pageXOffset)) {
            dragEdges.left = true;
        }
        if ((pageX > (viewBoundaries.right - autoScrollDistance) + window.pageXOffset) &&
            (pageX < viewBoundaries.right + window.pageXOffset)) {
            dragEdges.right = true;
        }
        this.dragEdges = dragEdges;
    };
    DragAndDrop.prototype.autoScroll = function () {
        this.autoScrollValidation();
        var scrollSensitivity = 30;
        if (this.parent.isAdaptive) {
            var parent_1;
            if (this.dragEdges.top || this.dragEdges.bottom) {
                if (this.dragObj.targetClone) {
                    parent_1 = sf.base.closest(this.dragObj.targetClone, '.' + CARD_CONTAINER_CLASS);
                }
                else {
                    parent_1 = sf.base.closest(this.dragObj.draggedClone, '.' + CARD_CONTAINER_CLASS);
                }
            }
            else if (this.dragEdges.right || this.dragEdges.left) {
                parent_1 = this.target.querySelector('.' + CONTENT_CLASS);
            }
            if (parent_1) {
                var yIsScrollable = parent_1.offsetHeight <= parent_1.scrollHeight;
                var xIsScrollable = parent_1.offsetWidth <= parent_1.scrollWidth;
                var yInBounds = parent_1.scrollTop >= 0 && parent_1.scrollTop + parent_1.offsetHeight <= parent_1.scrollHeight;
                var xInBounds = parent_1.scrollLeft >= 0 && parent_1.scrollLeft + parent_1.offsetWidth <= parent_1.scrollWidth;
                if (yIsScrollable && yInBounds && (this.dragEdges.top || this.dragEdges.bottom)) {
                    parent_1.scrollTop += this.dragEdges.top ? -(scrollSensitivity + 36) : scrollSensitivity;
                }
                if (xIsScrollable && xInBounds && (this.dragEdges.left || this.dragEdges.right)) {
                    var scroll_1 = (this.getWidth() * (this.target.querySelector('.' + CONTENT_ROW_CLASS + ':not(.' + SWIMLANE_ROW_CLASS + ')').childElementCount - 1)) > parent_1.scrollLeft;
                    if (scroll_1 || this.dragEdges.left) {
                        parent_1.scrollLeft += this.dragEdges.left ? -scrollSensitivity : scrollSensitivity;
                    }
                }
            }
        }
        else {
            var parent_2 = this.target.querySelector('.' + CONTENT_CLASS);
            var column = this.dragObj.targetClone.parentElement;
            var yScrollable = parent_2.offsetHeight <= parent_2.scrollHeight;
            var xScrollable = parent_2.offsetWidth <= parent_2.scrollWidth;
            var yBounds = yScrollable && parent_2.scrollTop >= 0 && parent_2.scrollTop + parent_2.offsetHeight <= parent_2.scrollHeight;
            var xBounds = xScrollable && parent_2.scrollLeft >= 0 && parent_2.scrollLeft + parent_2.offsetWidth <= parent_2.scrollWidth;
            if (yBounds && (this.dragEdges.top || this.dragEdges.bottom)) {
                parent_2.scrollTop += this.dragEdges.top ? -scrollSensitivity : scrollSensitivity;
                if (column) {
                    column.scrollTop += this.dragEdges.top ? -scrollSensitivity : scrollSensitivity;
                }
            }
            if (xBounds && (this.dragEdges.left || this.dragEdges.right)) {
                parent_2.scrollLeft += this.dragEdges.left ? -scrollSensitivity : scrollSensitivity;
                if (column) {
                    column.scrollLeft += this.dragEdges.left ? -scrollSensitivity : scrollSensitivity;
                }
            }
            if (this.dragObj.pageY - window.scrollY < scrollSensitivity) {
                window.scrollTo(window.scrollX, window.scrollY - scrollSensitivity);
            }
            else if (window.innerHeight - (this.dragObj.pageY - window.scrollY) < scrollSensitivity) {
                window.scrollTo(window.scrollX, window.scrollY + scrollSensitivity);
            }
        }
    };
    DragAndDrop.prototype.getWidth = function () {
        return (window.innerWidth * 80) / 100;
    };
    DragAndDrop.prototype.unWireDragEvents = function () {
        if (this.dragObj.instance && !this.dragObj.instance.isDestroyed) {
            this.dragObj.instance.destroy();
        }
    };
    DragAndDrop.prototype.destroy = function () {
        this.unWireDragEvents();
    };
    DragAndDrop.prototype.getXYValue = function (e, direction) {
        var eventArgs = e.event;
        var touchList = eventArgs.changedTouches;
        var value;
        if (direction === 'X') {
            value = touchList ? touchList[0].clientX : eventArgs.clientX;
        }
        else {
            value = touchList ? touchList[0].clientY : eventArgs.clientY;
        }
        if (!value && e.type === 'focus' && e.target) {
            var rect = e.target.getBoundingClientRect();
            value = rect ? (direction === 'X' ? rect.left : rect.top) : null;
        }
        return Math.ceil(value);
    };
    return DragAndDrop;
}());

/**
 * Keyboard interaction module
 */
var Keyboard = /** @class */ (function () {
    function Keyboard(parent) {
        this.keyConfigs = {
            firstCardSelection: '36',
            lastCardSelection: '35',
            upArrow: '38',
            downArrow: '40',
            rightArrow: '39',
            leftArrow: '37',
            multiSelectionEnter: 'ctrl+13',
            multiSelectionSpace: 'ctrl+32',
            multiSelectionByUpArrow: 'shift+38',
            multiSelectionByDownArrow: 'shift+40',
            shiftTab: 'shift+tab',
            enter: '13',
            tab: 'tab',
            delete: '46',
            escape: '27',
            space: '32'
        };
        this.parent = parent;
        this.parent.element.tabIndex = this.parent.element.tabIndex === -1 ? 0 : this.parent.element.tabIndex;
        this.keyboardModule = new sf.base.KeyboardEvents(this.parent.element, {
            keyAction: this.keyActionHandler.bind(this),
            keyConfigs: this.keyConfigs,
            eventName: 'keydown'
        });
        this.multiSelection = false;
    }
    Keyboard.prototype.keyActionHandler = function (e) {
        var selectedCard = this.parent.element.querySelectorAll("." + CARD_CLASS + "." + CARD_SELECTION_CLASS).item(0);
        if (!selectedCard && !sf.base.closest(document.activeElement, "." + ROOT_CLASS)) {
            return;
        }
        var contentCell;
        switch (e.action) {
            case 'upArrow':
            case 'downArrow':
            case 'multiSelectionByUpArrow':
            case 'multiSelectionByDownArrow':
                e.preventDefault();
                this.processUpDownArrow(e.action, selectedCard);
                break;
            case 'rightArrow':
            case 'leftArrow':
                this.processLeftRightArrow(e);
                break;
            case 'firstCardSelection':
            case 'lastCardSelection':
                this.processCardSelection(e.action, selectedCard);
                break;
            case 'multiSelectionEnter':
            case 'multiSelectionSpace':
                if (document.activeElement) {
                    this.parent.cardSelection(document.activeElement, true, false);
                }
                break;
            case 'space':
            case 'enter':
                this.processEnter(e, selectedCard);
                break;
            case 'escape':
                if (document.activeElement.classList.contains(CARD_CLASS) ||
                    document.activeElement.classList.contains(SHOW_ADD_BUTTON)) {
                    if (document.activeElement.classList.contains(CARD_SELECTION_CLASS)) {
                        sf.base.removeClass([document.activeElement], CARD_SELECTION_CLASS);
                        document.activeElement.focus();
                    }
                    else {
                        var ele = sf.base.closest(document.activeElement, '.' + CONTENT_CELLS_CLASS);
                        var cards = [].slice.call(ele.querySelectorAll('.' + CARD_CLASS));
                        sf.base.removeClass(cards, CARD_SELECTION_CLASS);
                        ele.focus();
                        this.cardTabIndexRemove();
                        this.addRemoveTabIndex('Add');
                    }
                }
                break;
            case 'tab':
            case 'shiftTab':
                contentCell = sf.base.closest(document.activeElement, '.' + CONTENT_CELLS_CLASS);
                if (document.activeElement.classList.contains(CARD_CLASS)) {
                    if (!document.activeElement.nextElementSibling && e.action === 'tab') {
                        e.preventDefault();
                    }
                    if (!document.activeElement.previousElementSibling && contentCell.querySelector('.' + SHOW_ADD_BUTTON)
                        && e.action === 'tab') {
                        sf.base.addClass([contentCell.querySelector('.' + SHOW_ADD_BUTTON)], SHOW_ADD_FOCUS);
                    }
                }
                if (document.activeElement.classList.contains(SHOW_ADD_BUTTON)) {
                    if ((!contentCell.querySelector('.' + CARD_CLASS) && e.action === 'tab') || e.action === 'shiftTab') {
                        e.preventDefault();
                    }
                }
                if (document.activeElement.classList.contains(ROOT_CLASS)) {
                    this.cardTabIndexRemove();
                    this.addRemoveTabIndex('Add');
                }
                break;
            case 'delete':
                break;
        }
    };
    Keyboard.prototype.processCardSelection = function (action, selectedCard) {
        if (selectedCard) {
            sf.base.removeClass([selectedCard], CARD_SELECTION_CLASS);
            this.parent.disableAttributeSelection(selectedCard);
            var selection = this.parent.selectionArray;
            selection.splice(selection.indexOf(selectedCard.getAttribute('data-id')), 1);
        }
        this.cardTabIndexRemove();
        var cards = [].slice.call(this.parent.element.querySelectorAll('.' + CARD_CLASS));
        var element = action === 'firstCardSelection' ? cards[0] : cards[cards.length - 1];
        this.parent.cardSelection(element, false, false);
        this.addRemoveTabIndex('Remove');
        element.focus();
        var card = [].slice.call(sf.base.closest(element, '.' + CONTENT_CELLS_CLASS).querySelectorAll('.' + CARD_CLASS));
        card.forEach(function (element) { element.setAttribute('tabindex', '0'); });
    };
    Keyboard.prototype.processLeftRightArrow = function (e) {
        if (document.activeElement.classList.contains(CONTENT_CELLS_CLASS)) {
            if (e.action === 'rightArrow' && document.activeElement.nextElementSibling) {
                document.activeElement.nextElementSibling.focus();
            }
            else if (e.action === 'leftArrow' && document.activeElement.previousElementSibling) {
                document.activeElement.previousElementSibling.focus();
            }
        }
    };
    Keyboard.prototype.processUpDownArrow = function (action, selectedCard) {
        if (action === 'upArrow' && document.activeElement) {
            if (document.activeElement.classList.contains(CARD_CLASS) && document.activeElement.previousElementSibling) {
                document.activeElement.previousElementSibling.focus();
            }
            else if (document.activeElement.classList.contains(SHOW_ADD_BUTTON)) {
                document.activeElement.setAttribute('tabindex', '-1');
                sf.base.removeClass([document.activeElement], SHOW_ADD_FOCUS);
                var cell = sf.base.closest(document.activeElement, '.' + CONTENT_CELLS_CLASS);
                if (cell.querySelectorAll('.' + CARD_CLASS).length > 0) {
                    [].slice.call(cell.querySelectorAll('.' + CARD_CLASS)).slice(-1)[0].focus();
                }
            }
            this.removeSelection();
        }
        else if (action === 'downArrow' && document.activeElement &&
            document.activeElement.classList.contains(CARD_CLASS)) {
            if (document.activeElement.nextElementSibling) {
                document.activeElement.nextElementSibling.focus();
            }
            else if (sf.base.closest(document.activeElement, '.' + CARD_CONTAINER_CLASS).nextElementSibling) {
                var ele = sf.base.closest(document.activeElement, '.' + CARD_CONTAINER_CLASS).nextElementSibling;
                ele.setAttribute('tabindex', '0');
                sf.base.addClass([ele], SHOW_ADD_FOCUS);
                ele.focus();
            }
            this.removeSelection();
        }
        if ((action === 'multiSelectionByUpArrow' || action === 'multiSelectionByDownArrow')
            && selectedCard && this.parent.cardSettings.selectionType === 'Multiple') {
            var card = void 0;
            if (action === 'multiSelectionByUpArrow') {
                card = document.activeElement.previousElementSibling;
            }
            else {
                card = document.activeElement.nextElementSibling;
            }
            if (card) {
                this.parent.cardSelection(card, false, true);
                card.focus();
                this.multiSelection = true;
            }
        }
    };
    Keyboard.prototype.removeSelection = function () {
        if (this.multiSelection) {
            var cards = this.parent.getSelectedCards();
            if (cards.length > 0) {
                sf.base.removeClass(cards, CARD_SELECTION_CLASS);
                this.parent.disableAttributeSelection(cards);
            }
            this.multiSelection = false;
        }
    };
    Keyboard.prototype.cardTabIndexRemove = function () {
        var cards = [].slice.call(this.parent.element.querySelectorAll('.' + CARD_CLASS));
        cards.forEach(function (card) { card.setAttribute('tabindex', '-1'); });
        var addButton = [].slice.call(this.parent.element.querySelectorAll('.' + SHOW_ADD_BUTTON));
        addButton.forEach(function (add) {
            add.setAttribute('tabindex', '-1');
            sf.base.removeClass([add], SHOW_ADD_FOCUS);
        });
    };
    Keyboard.prototype.processEnter = function (e, selectedCard) {
        if (e.action === 'space') {
            e.preventDefault();
        }
        var element = (e.target);
        if (element.classList.contains(HEADER_ICON_CLASS)) {
            element.click();
        }
        if (element.classList.contains(SWIMLANE_ROW_EXPAND_CLASS) || element.classList.contains(SWIMLANE_ROW_COLLAPSE_CLASS)) {
            element.click();
        }
        if (document.activeElement.classList.contains(CARD_CLASS)) {
            this.parent.cardSelection(document.activeElement, false, false);
        }
        if (document.activeElement.classList.contains(SHOW_ADD_BUTTON)) {
            document.activeElement.focus();
        }
        if (element.classList.contains(CONTENT_CELLS_CLASS)) {
            var cards = [].slice.call(element.querySelectorAll('.' + CARD_CLASS));
            this.addRemoveTabIndex('Remove');
            if (cards.length > 0) {
                element.querySelector('.' + CARD_CLASS).focus();
                cards.forEach(function (element) { element.setAttribute('tabindex', '0'); });
            }
            if (element.querySelector('.' + SHOW_ADD_BUTTON)) {
                element.querySelector('.' + SHOW_ADD_BUTTON).setAttribute('tabindex', '0');
                element.querySelector('.' + SHOW_ADD_BUTTON).focus();
            }
        }
        if (selectedCard === document.activeElement && this.parent.element.querySelectorAll('.' + CARD_SELECTION_CLASS).length === 1) {
            this.parent.dotNetRef.invokeMethodAsync('OpenDialog', selectedCard.getAttribute('data-id'));
            selectedCard.focus();
        }
    };
    Keyboard.prototype.addRemoveTabIndex = function (action) {
        var attribute = action === 'Add' ? '0' : '-1';
        var headerIcon = [].slice.call(this.parent.element.querySelectorAll('.' + HEADER_ICON_CLASS));
        if (headerIcon.length > 0) {
            headerIcon.forEach(function (element) { element.setAttribute('tabindex', attribute); });
        }
        var swimlaneIcon = [].slice.call(this.parent.element.querySelectorAll('.' + SWIMLANE_ROW_EXPAND_CLASS));
        if (swimlaneIcon.length > 0) {
            swimlaneIcon.forEach(function (element) { element.setAttribute('tabindex', attribute); });
        }
        var className = '.' + CONTENT_ROW_CLASS + ':not(.' + SWIMLANE_ROW_CLASS + ') .' + CONTENT_CELLS_CLASS;
        var contentCell = [].slice.call(this.parent.element.querySelectorAll(className));
        contentCell.forEach(function (element) { element.setAttribute('tabindex', attribute); });
    };
    Keyboard.prototype.destroy = function () {
        this.keyboardModule.destroy();
    };
    return Keyboard;
}());

/**
 * kanban touch module
 */
var KanbanTouch = /** @class */ (function () {
    function KanbanTouch(parent) {
        this.parent = parent;
        this.tabHold = false;
    }
    KanbanTouch.prototype.wireTouchEvents = function () {
        this.element = this.parent.element.querySelector('.' + CONTENT_CLASS);
        this.touchObj = new sf.base.Touch(this.element, { tapHold: this.tapHoldHandler.bind(this) });
    };
    KanbanTouch.prototype.tapHoldHandler = function (e) {
        this.tabHold = true;
        var target = sf.base.closest(e.originalEvent.target, '.' + CARD_CLASS);
        if (target && this.parent.cardSettings.selectionType === 'Multiple') {
            if (!this.mobilePopup) {
                this.renderMobilePopup();
                this.mobilePopup.show();
            }
            this.updatePopupContent();
        }
    };
    KanbanTouch.prototype.renderMobilePopup = function () {
        if (this.parent.cardSettings.selectionType === 'Multiple') {
            var mobilePopupWrapper = sf.base.createElement('div', {
                className: POPUP_WRAPPER_CLASS + ' e-popup-close',
                innerHTML: "<div class=\"" + POPUP_HEADER_CLASS + "\"><button class=\"e-btn " + CLOSE_CLASS + " e-flat e-round e-small\">" +
                    ("<span class=\"" + ICON_CLASS + " " + CLOSE_ICON_CLASS + "\"></span></button></div>") +
                    ("<div class=\"" + POPUP_CONTENT_CLASS + "\"></div>")
            });
            document.body.appendChild(mobilePopupWrapper);
            sf.base.addClass([mobilePopupWrapper], DEVICE_CLASS);
            this.mobilePopup = new sf.popups.Popup(mobilePopupWrapper, {
                targetType: 'container',
                enableRtl: this.parent.enableRtl,
                hideAnimation: { name: 'ZoomOut' },
                showAnimation: { name: 'ZoomIn' },
                collision: { X: 'fit', Y: 'fit' },
                position: { X: 'left', Y: 'top' },
                viewPortElement: document.body,
                zIndex: 1004,
                close: this.popupClose.bind(this)
            });
            var closeIcon = this.mobilePopup.element.querySelector('.' + CLOSE_CLASS);
            sf.base.EventHandler.add(closeIcon, 'click', this.closeClick, this);
        }
    };
    KanbanTouch.prototype.closeClick = function () {
        this.mobilePopup.hide();
    };
    KanbanTouch.prototype.popupClose = function () {
        this.popupDestroy();
    };
    KanbanTouch.prototype.getPopupContent = function () {
        var popupContent;
        var selectedCards = this.parent.getSelectedCards();
        if (selectedCards.length > 1) {
            popupContent = '(' + selectedCards.length + ') Cards Selected';
        }
        else if (selectedCards.length === 1) {
            popupContent = ' ' + selectedCards[0].getAttribute('data-id');
        }
        return popupContent;
    };
    KanbanTouch.prototype.updatePopupContent = function () {
        if (!this.mobilePopup) {
            return;
        }
        var popupContent = this.getPopupContent();
        if (popupContent) {
            this.mobilePopup.element.querySelector('.' + POPUP_CONTENT_CLASS).textContent = popupContent;
        }
        else {
            this.mobilePopup.hide();
        }
    };
    KanbanTouch.prototype.popupDestroy = function () {
        if (this.mobilePopup && this.mobilePopup.element) {
            this.mobilePopup.destroy();
            sf.base.remove(this.mobilePopup.element);
            this.mobilePopup = null;
        }
    };
    KanbanTouch.prototype.unWireTouchEvents = function () {
        if (this.touchObj) {
            this.touchObj.destroy();
        }
        this.touchObj = null;
        this.element = null;
    };
    KanbanTouch.prototype.destroy = function () {
        this.popupDestroy();
        this.unWireTouchEvents();
        this.tabHold = false;
    };
    return KanbanTouch;
}());

/* eslint-disable @typescript-eslint/no-explicit-any */
/**
 * kanban base
 */
var SfKanban = /** @class */ (function () {
    function SfKanban(element, options, dotnetRef) {
        this.element = element;
        this.dotNetRef = dotnetRef;
        this.updateContext(options);
        this.columnToggleArray = [];
        this.selectionArray = [];
        this.lastCardSelection = null;
        this.lastSelectionRow = null;
        this.lastCard = null;
        this.selectedCardsElement = [];
        this.selectedCardsData = [];
        this.hideColumnKeys = [];
        this.frozenOrder = 0;
        this.scrollPosition = { content: {}, column: {} };
        this.initializeModules();
        this.scrollUiUpdate();
        this.wireEvents();
        if (this.element) {
            // eslint-disable-next-line camelcase
            this.element.blazor__instance = this;
        }
    }
    SfKanban.prototype.initializeModules = function () {
        if (this.isAdaptive || sf.base.Browser.isTouch) {
            this.touchModule = new KanbanTouch(this);
        }
        if (this.allowDragAndDrop) {
            this.dragAndDropModule = new DragAndDrop(this);
        }
        if (this.allowKeyboard) {
            this.keyboardModule = new Keyboard(this);
        }
        this.scrollPosition.content = { left: 0, top: 0 };
        this.initializeSwimlaneTree();
    };
    SfKanban.prototype.updateContext = function (kanbanObj) {
        sf.base.extend(this, this, kanbanObj);
    };
    SfKanban.prototype.getSelectedCards = function () {
        return [].slice.call(this.element.querySelectorAll('.' + CARD_CLASS + '.' + CARD_SELECTION_CLASS));
    };
    SfKanban.prototype.documentClick = function (args) {
        if (args.target && this.element.querySelector('.' + SWIMLANE_RESOURCE_CLASS)) {
            if (args.target.classList.contains(SWIMLANE_OVERLAY_CLASS) &&
                this.element.querySelector('.' + SWIMLANE_RESOURCE_CLASS).classList.contains('e-popup-open')) {
                this.treePopup.hide();
                sf.base.removeClass([this.popupOverlay], 'e-enable');
            }
        }
        if (sf.base.closest(args.target, "." + ROOT_CLASS)) {
            return;
        }
        var cards = [].slice.call(this.element.querySelectorAll("." + CARD_CLASS + "." + CARD_SELECTION_CLASS));
        sf.base.removeClass(cards, CARD_SELECTION_CLASS);
        this.disableAttributeSelection(cards);
    };
    SfKanban.prototype.disableAttributeSelection = function (cards) {
        if (cards instanceof Element) {
            cards.setAttribute('aria-selected', 'false');
        }
        else {
            cards.forEach(function (card) { card.setAttribute('aria-selected', 'false'); });
        }
    };
    SfKanban.prototype.wireDragEvent = function () {
        var _this = this;
        if (this.allowDragAndDrop) {
            var cards = [].slice.call(this.element.querySelectorAll('.' + CONTENT_CELLS_CLASS
                + '.' + DRAG_CLASS + ' .' + CARD_CLASS + ':not(' + DRAGGABLE_CLASS + ')'));
            sf.base.addClass(cards, DROPPABLE_CLASS);
            cards.forEach(function (card) { return _this.dragAndDropModule.wireDragEvents(card); });
        }
    };
    SfKanban.prototype.initializeSwimlaneTree = function () {
        if (this.swimlaneSettings.keyField && this.isAdaptive) {
            var height = this.element.querySelector('.' + SWIMLANE_HEADER_CLASS).offsetHeight;
            var treeHeight = window.innerHeight - height;
            this.popupOverlay = this.element.querySelector('.' + SWIMLANE_CONTENT_CLASS + ' .' + SWIMLANE_OVERLAY_CLASS);
            sf.base.setStyleAttribute(this.element.querySelector('.' + SWIMLANE_OVERLAY_CLASS), { 'height': treeHeight + 'px' });
            sf.base.setStyleAttribute(this.element.querySelector('.' + SWIMLANE_CONTENT_CLASS), { 'top': height + 'px' });
            var treeWrapper = this.element.querySelector('.' + SWIMLANE_RESOURCE_CLASS);
            sf.base.setStyleAttribute(treeWrapper, { 'height': treeHeight + 'px' });
            var popupObj = {
                targetType: 'relative',
                actionOnScroll: 'none',
                enableRtl: this.enableRtl,
                zIndex: 10,
                hideAnimation: { name: 'SlideLeftOut', duration: 500 },
                showAnimation: { name: 'SlideLeftIn', duration: 500 },
                viewPortElement: this.element.querySelector('.' + CONTENT_CLASS)
            };
            this.treePopup = new sf.popups.Popup(treeWrapper, popupObj);
        }
    };
    SfKanban.prototype.cardSelection = function (target, isCtrl, isShift) {
        var _this = this;
        if (!target) {
            return;
        }
        var cards = this.getSelectedCards();
        if (this.cardSettings.selectionType !== 'None') {
            var contentRow = sf.base.closest(target, '.' + CONTENT_ROW_CLASS);
            var index = !sf.base.isNullOrUndefined(this.lastSelectionRow) ? this.lastSelectionRow.rowIndex : contentRow.rowIndex;
            if (index !== contentRow.rowIndex && (isCtrl || isShift) && this.cardSettings.selectionType === 'Multiple') {
                return;
            }
            if (cards.length !== 0 && (!isCtrl || this.cardSettings.selectionType === 'Single')) {
                sf.base.removeClass(cards, CARD_SELECTION_CLASS);
                this.disableAttributeSelection(cards);
                cards.forEach(function (el) {
                    _this.selectionArray.splice(_this.selectionArray.indexOf(el.getAttribute('data-id')), 1);
                    _this.selectedCardsElement.splice(_this.selectedCardsElement.indexOf(el), 1);
                });
            }
            if (cards.length > 0 && isShift && this.cardSettings.selectionType === 'Multiple') {
                var curCards_1 = [];
                var start = void 0;
                var end = void 0;
                var i = void 0;
                var allCards = [].slice.call(contentRow.querySelectorAll('.' + CARD_CLASS));
                allCards.forEach(function (el) { return curCards_1.push(el.getAttribute('data-id')); });
                var curId = target.getAttribute('data-id');
                var lastId = this.lastCard.getAttribute('data-id');
                var curIndex = end = curCards_1.indexOf(curId);
                var lastIndex = start = curCards_1.indexOf(lastId);
                var select = curIndex > lastIndex ? 'next' : 'prev';
                if (select === 'prev') {
                    start = curIndex;
                    end = lastIndex;
                }
                for (i = start; i <= end; i++) {
                    var card = allCards[i];
                    sf.base.addClass([card], CARD_SELECTION_CLASS);
                    card.setAttribute('aria-selected', 'true');
                    card.setAttribute('tabindex', '0');
                    this.selectionArray.push(card.getAttribute('data-id'));
                    this.selectedCardsElement.push(card);
                    this.lastCardSelection = card;
                    if (select === 'prev') {
                        this.lastCardSelection = allCards[start];
                    }
                }
            }
            else {
                sf.base.addClass([target], CARD_SELECTION_CLASS);
                target.setAttribute('aria-selected', 'true');
                target.setAttribute('tabindex', '0');
                this.selectionArray.push(target.getAttribute('data-id'));
                this.selectedCardsElement.push(target);
                this.lastCard = this.lastCardSelection = target;
                this.lastSelectionRow = sf.base.closest(target, '.' + CONTENT_ROW_CLASS);
                if (this.lastSelectionRow.previousElementSibling) {
                    var elementSelector = "." + SWIMLANE_ROW_EXPAND_CLASS + ",." + SWIMLANE_ROW_COLLAPSE_CLASS;
                    var parentEle = this.lastSelectionRow.previousElementSibling.querySelector(elementSelector);
                    if (parentEle && parentEle.classList.contains(SWIMLANE_ROW_COLLAPSE_CLASS)) {
                        parentEle.click();
                    }
                }
            }
        }
    };
    SfKanban.prototype.scrollUiUpdate = function () {
        var _this = this;
        var header = this.element.querySelector('.' + HEADER_CLASS);
        var content = this.element.querySelector('.' + CONTENT_CLASS);
        var height = this.element.offsetHeight - header.offsetHeight;
        if (this.isAdaptive) {
            height = window.innerHeight - (header.offsetHeight + bottomSpace);
            var swimlaneToolbar = this.element.querySelector('.' + SWIMLANE_HEADER_CLASS);
            if (swimlaneToolbar) {
                height -= swimlaneToolbar.offsetHeight;
            }
            var cardContainers = [].slice.call(this.element.querySelectorAll('.' + CONTENT_CELLS_CLASS));
            cardContainers.forEach(function (cell) {
                var cardContainer = cell.querySelector('.' + CARD_CONTAINER_CLASS);
                if (!cardContainer.classList.contains(MULTI_CARD_CONTAINER_CLASS)) {
                    cardContainer.style.height = sf.base.formatUnit(height);
                    sf.base.EventHandler.add(cell, 'touchmove', _this.onAdaptiveScroll, _this);
                }
            });
        }
        if (this.height !== 'auto' && this.height !== '100%') {
            content.style.height = sf.base.formatUnit(height);
        }
        [].slice.call(header.children).forEach(function (node) {
            var paddingValue = 0;
            if ((content.offsetWidth - content.clientWidth) > 0) {
                paddingValue = 17;
                if ((content.offsetHeight - content.clientHeight) > 0) {
                    node.style.width = sf.base.formatUnit(content.clientWidth);
                }
            }
            if (_this.enableRtl) {
                node.style.paddingLeft = sf.base.formatUnit(paddingValue);
            }
            else {
                node.style.paddingRight = sf.base.formatUnit(paddingValue);
            }
        });
    };
    SfKanban.prototype.onContentScroll = function (e) {
        var target = e.target;
        var header = this.element.querySelector('.' + HEADER_CLASS);
        [].slice.call(header.children).forEach(function (node) { node.scrollLeft = target.scrollLeft; });
        this.scrollPosition.content = { left: target.scrollLeft, top: target.scrollTop };
        if (!sf.base.isNullOrUndefined(this.swimlaneSettings.keyField) && this.swimlaneSettings.enableFrozenRows) {
            this.frozenRows(e);
        }
    };
    SfKanban.prototype.frozenRows = function (e) {
        var firstSwimlane = this.element.querySelector('.' + SWIMLANE_ROW_CLASS);
        var header = this.element.querySelector('.' + HEADER_CLASS);
        var content = this.element.querySelector('.' + CONTENT_CLASS);
        if (sf.base.isNullOrUndefined(this.frozenSwimlaneRow)) {
            this.frozenSwimlaneRow = sf.base.createElement('div', { className: FROZEN_SWIMLANE_ROW_CLASS });
            var frozenRow = sf.base.createElement('div', { className: FROZEN_ROW_CLASS });
            this.frozenSwimlaneRow.appendChild(frozenRow);
            this.element.insertBefore(this.frozenSwimlaneRow, this.element.firstElementChild);
            if (firstSwimlane) {
                frozenRow.appendChild(firstSwimlane.querySelector('.' + SWIMLANE_HEADER_CLASS).cloneNode(true));
                sf.base.addClass([frozenRow.querySelector('.' + ICON_CLASS)], 'e-frozen-icons');
                sf.base.setStyleAttribute(this.frozenSwimlaneRow, { height: sf.base.formatUnit(firstSwimlane.getBoundingClientRect().height),
                    width: sf.base.formatUnit(content.querySelector('.e-swimlane').getBoundingClientRect().width),
                    top: sf.base.formatUnit(header.getBoundingClientRect().height.toString())
                });
                sf.base.setStyleAttribute(header, { position: 'relative', top: sf.base.formatUnit((-this.frozenSwimlaneRow.getBoundingClientRect().height)) });
                sf.base.setStyleAttribute(content, { position: 'relative', top: sf.base.formatUnit((-this.frozenSwimlaneRow.getBoundingClientRect().height)) });
            }
        }
        else {
            var swimlaneRows = [].slice.call(this.element.querySelectorAll('.' + SWIMLANE_ROW_CLASS));
            var curSwim = swimlaneRows[this.frozenOrder];
            var prevSwim = swimlaneRows[this.frozenOrder - 1];
            var nextSwim = swimlaneRows[this.frozenOrder + 1];
            var curSwimHeight = void 0;
            var prevSwimHeight = void 0;
            var nextSwimHeight = void 0;
            if (curSwim) {
                curSwimHeight = curSwim.getBoundingClientRect().top + curSwim.getBoundingClientRect().height;
            }
            if (prevSwim) {
                prevSwimHeight = prevSwim.getBoundingClientRect().top + prevSwim.getBoundingClientRect().height;
            }
            if (nextSwim) {
                nextSwimHeight = nextSwim.getBoundingClientRect().top + nextSwim.getBoundingClientRect().height;
            }
            var frozenSwimHeight = content.getBoundingClientRect().top + this.frozenSwimlaneRow.getBoundingClientRect().height;
            var frozenRowsElement = this.frozenSwimlaneRow.querySelector('.' + FROZEN_ROW_CLASS);
            if (nextSwimHeight && frozenSwimHeight >= nextSwimHeight && this.frozenOrder < swimlaneRows.length - 1) {
                if (frozenRowsElement) {
                    sf.base.remove(frozenRowsElement.querySelector('.' + SWIMLANE_HEADER_CLASS));
                    if (nextSwim) {
                        frozenRowsElement.appendChild(nextSwim.querySelector('.' + SWIMLANE_HEADER_CLASS).cloneNode(true));
                    }
                    sf.base.addClass([frozenRowsElement.querySelector('.' + ICON_CLASS)], 'e-frozen-icons');
                }
                ++this.frozenOrder;
            }
            else if (prevSwimHeight && frozenSwimHeight < curSwimHeight && frozenSwimHeight > prevSwimHeight && this.frozenOrder > 0) {
                if (frozenRowsElement) {
                    sf.base.remove(frozenRowsElement.querySelector('.' + SWIMLANE_HEADER_CLASS));
                    if (prevSwim) {
                        frozenRowsElement.appendChild(prevSwim.querySelector('.' + SWIMLANE_HEADER_CLASS).cloneNode(true));
                    }
                    sf.base.addClass([frozenRowsElement.querySelector('.' + ICON_CLASS)], 'e-frozen-icons');
                }
                --this.frozenOrder;
            }
        }
        if (e && e.target.scrollTop == 0) {
            this.removeFrozenRows();
        }
    };
    SfKanban.prototype.removeFrozenRows = function () {
        sf.base.remove(this.frozenSwimlaneRow);
        this.frozenSwimlaneRow = null;
        var header = this.element.querySelector('.' + HEADER_CLASS);
        var content = this.element.querySelector('.' + CONTENT_CLASS);
        sf.base.setStyleAttribute(header, { position: '', top: '' });
        sf.base.setStyleAttribute(content, { position: '', top: '' });
        this.scrollPosition.content = { left: this.scrollPosition.content.left, top: 0 };
        content.scrollTop = 0;
    };
    SfKanban.prototype.onColumnScroll = function (e) {
        var target = e.target;
        if (target.offsetParent) {
            var columnKey = target.offsetParent.getAttribute('data-key');
            this.scrollPosition.column[columnKey] = { left: target.scrollLeft, top: target.scrollTop };
        }
    };
    SfKanban.prototype.onAdaptiveScroll = function (e) {
        if (this.touchModule.tabHold && !this.touchModule.mobilePopup) {
            e.preventDefault();
        }
    };
    SfKanban.prototype.updateScrollPosition = function () {
        var _this = this;
        var content = this.element.querySelector('.' + CONTENT_CLASS);
        if (content) {
            content.scrollTo(this.scrollPosition.content.left, this.scrollPosition.content.top);
        }
        var cardContainer = [].slice.call(this.element.querySelectorAll('.' + CARD_CONTAINER_CLASS));
        cardContainer.forEach(function (container) {
            if (container.offsetParent) {
                var scrollData = _this.scrollPosition.column[container.offsetParent.getAttribute('data-key')];
                if (scrollData) {
                    container.scrollTo(scrollData.left, scrollData.top);
                }
            }
        });
    };
    SfKanban.prototype.wireEvents = function () {
        var _this = this;
        var content = this.element.querySelector('.' + CONTENT_CLASS);
        sf.base.EventHandler.add(content, 'scroll', this.onContentScroll, this);
        sf.base.EventHandler.add(document, sf.base.Browser.touchStartEvent, this.documentClick, this);
        var cardContainer = [].slice.call(this.element.querySelectorAll('.' + CARD_CONTAINER_CLASS));
        cardContainer.forEach(function (container) {
            if (container.offsetParent) {
                _this.scrollPosition.column[container.offsetParent.getAttribute('data-key')] = { left: 0, top: 0 };
            }
            sf.base.EventHandler.add(container, 'scroll', _this.onColumnScroll, _this);
        });
        if (this.isAdaptive) {
            this.touchModule.wireTouchEvents();
        }
        this.wireDragEvent();
    };
    SfKanban.prototype.unWireEvents = function () {
        var _this = this;
        var content = this.element.querySelector('.' + CONTENT_CLASS);
        sf.base.EventHandler.remove(content, 'scroll', this.onContentScroll);
        sf.base.EventHandler.remove(document, sf.base.Browser.touchStartEvent, this.documentClick);
        var cardContainer = [].slice.call(this.element.querySelectorAll('.' + CARD_CONTAINER_CLASS));
        cardContainer.forEach(function (container) { sf.base.EventHandler.remove(container, 'scroll', _this.onColumnScroll); });
        if (this.isAdaptive) {
            var cardContainers = [].slice.call(this.element.querySelectorAll('.' + CONTENT_CELLS_CLASS));
            cardContainers.forEach(function (cell) { sf.base.EventHandler.remove(cell, 'touchmove', _this.onAdaptiveScroll); });
            this.touchModule.unWireTouchEvents();
        }
    };
    SfKanban.prototype.onCardClick = function (target, e) {
        if (target) {
            if (target.classList.contains(CARD_SELECTION_CLASS)) {
                sf.base.removeClass([target], CARD_SELECTION_CLASS);
                this.disableAttributeSelection(target);
            }
            else {
                var isCtrlKey = e.ctrlKey;
                if (this.isAdaptive && this.touchModule) {
                    isCtrlKey = (this.touchModule.mobilePopup && this.touchModule.tabHold) || isCtrlKey;
                }
                this.cardSelection(target, isCtrlKey, e.shiftKey);
            }
            if (this.isAdaptive && this.touchModule) {
                this.touchModule.updatePopupContent();
            }
            var cell = sf.base.closest(target, '.' + CONTENT_CELLS_CLASS);
            if (this.allowKeyboard) {
                var element = [].slice.call(cell.querySelectorAll('.' + CARD_CLASS));
                element.forEach(function (e) {
                    e.setAttribute('tabindex', '0');
                });
                this.keyboardModule.addRemoveTabIndex('Remove');
            }
        }
    };
    SfKanban.prototype.onMenuClick = function () {
        if (this.element.querySelector('.' + SWIMLANE_RESOURCE_CLASS).classList.contains('e-popup-open')) {
            this.treePopup.hide();
            sf.base.removeClass([this.popupOverlay], 'e-enable');
        }
        else {
            this.treePopup.show();
            sf.base.addClass([this.popupOverlay], 'e-enable');
        }
    };
    SfKanban.prototype.onListViewClick = function () {
        this.treePopup.hide();
        sf.base.removeClass([this.popupOverlay], 'e-enable');
    };
    SfKanban.prototype.onPropertyChanged = function (props) {
        for (var _i = 0, props_1 = props; _i < props_1.length; _i++) {
            var key = props_1[_i];
            switch (key) {
                case 'width':
                    this.setWidth();
                    break;
                case 'height':
                    this.setHeight();
                    break;
                case 'enableRtl':
                    if (this.enableRtl) {
                        sf.base.addClass([this.element], CLS_RTL);
                    }
                    else {
                        sf.base.removeClass([this.element], CLS_RTL);
                    }
                    break;
            }
        }
    };
    SfKanban.prototype.onSwimlaneProperties = function (isDrag, keyField, isFrozenRows) {
        this.swimlaneSettings.allowDragAndDrop = isDrag;
        this.swimlaneSettings.keyField = keyField;
        this.swimlaneSettings.enableFrozenRows = isFrozenRows;
        if (this.frozenSwimlaneRow && !this.swimlaneSettings.enableFrozenRows) {
            this.removeFrozenRows();
        }
    };
    SfKanban.prototype.setWidth = function () {
        if (this.width === '100%') {
            this.element.style.width = '';
        }
        else {
            sf.base.setStyleAttribute(this.element, { 'width': sf.base.formatUnit(this.width) });
        }
    };
    SfKanban.prototype.setHeight = function () {
        sf.base.setStyleAttribute(this.element, { 'height': sf.base.formatUnit(this.height) });
    };
    SfKanban.prototype.destroy = function () {
        if (this.touchModule) {
            this.touchModule.destroy();
        }
        if (this.dragAndDropModule) {
            this.dragAndDropModule.destroy();
        }
        if (this.keyboardModule) {
            this.keyboardModule.destroy();
        }
        this.unWireEvents();
    };
    return SfKanban;
}());

/* eslint-disable @typescript-eslint/no-explicit-any */
/**
 * Blazor kanban scripts
 */
var Kanban = {
    initialize: function (element, options, dotnetRef) {
        if (element) {
            new SfKanban(element, options, dotnetRef);
        }
    },
    isDevice: function (dotnetRef) {
        dotnetRef.invokeMethodAsync(WINDOW_WIDTH, ((window.innerWidth * 70) / 100));
    },
    updateScrollPosition: function (element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.updateScrollPosition();
        }
    },
    wireDragEvents: function (element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.wireDragEvent();
        }
    },
    propertyChanged: function (element, changedProps) {
        if (!sf.base.isNullOrUndefined(element)) {
            element.blazor__instance.onPropertyChanged(changedProps);
        }
    },
    swimlaneProperties: function (element, isDrag, keyField, isFrozenRows) {
        if (!sf.base.isNullOrUndefined(element)) {
            element.blazor__instance.onSwimlaneProperties(isDrag, keyField, isFrozenRows);
        }
    },
    cardClick: function (element, card, e) {
        element.blazor__instance.onCardClick(card, e);
    },
    menuClick: function (element) {
        element.blazor__instance.onMenuClick();
    },
    listViewClick: function (element) {
        element.blazor__instance.onListViewClick();
    },
    getTargetDetails: function (element, left, top, isTemplate, type) {
        if (element && element.blazor__instance) {
            var target = document.elementFromPoint(left, top);
            var targetElement = void 0;
            var text = null;
            if (type === "tooltip") {
                if (isTemplate) {
                    targetElement = sf.base.closest(target, '.' + CARD_CLASS);
                    text = JSON.stringify(targetElement.getAttribute('data-id'));
                }
                else if (target.classList.contains(TOOLTIP_TEXT_CLASS)) {
                    text = JSON.stringify(target.innerHTML);
                }
                return text;
            }
            else {
                targetElement = sf.base.closest(target, '.' + CONTENT_CELLS_CLASS);
                if (targetElement) {
                    var clickArgs = {};
                    clickArgs.columnKeyField = targetElement.getAttribute('data-key');
                    if (targetElement.parentElement.previousElementSibling) {
                        clickArgs.swimlaneKeyField = targetElement.parentElement.previousElementSibling.getAttribute('data-key');
                    }
                    var cardElement = sf.base.closest(target, '.' + CARD_CLASS);
                    if (cardElement) {
                        var cardElements = [].slice.call(targetElement.querySelectorAll('.' + CARD_CLASS));
                        clickArgs.index = cardElements.indexOf(cardElement);
                        if (cardElement.previousElementSibling) {
                            clickArgs.previousCardId = cardElement.previousElementSibling.getAttribute('data-id');
                        }
                        else {
                            clickArgs.previousCardId = null;
                        }
                    }
                    else {
                        var target_1 = targetElement;
                        var cardMargin = parseInt(getComputedStyle(element.querySelector('.e-card')).marginBottom.replace("px", ""));
                        if (top - targetElement.getBoundingClientRect().top >= cardMargin) {
                            target_1 = document.elementFromPoint(left, top - cardMargin);
                        }
                        var cardElement_1 = sf.base.closest(target_1, '.' + CARD_CLASS);
                        if (cardElement_1) {
                            var cardElements = [].slice.call(targetElement.querySelectorAll('.' + CARD_CLASS));
                            clickArgs.index = cardElements.indexOf(cardElement_1);
                            clickArgs.previousCardId = cardElement_1.getAttribute('data-id');
                        }
                        else {
                            var cardElements = [].slice.call(targetElement.querySelectorAll('.' + CARD_CLASS));
                            if (top - targetElement.getBoundingClientRect().top >= cardMargin && cardElements.length > 0) {
                                clickArgs.previousCardId = cardElements[cardElements.length - 1].getAttribute('data-id');
                            }
                            else {
                                clickArgs.previousCardId = null;
                            }
                        }
                    }
                    return JSON.stringify(clickArgs);
                }
            }
        }
        return null;
    },
    destroy: function (element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.destroy();
        }
    }
};

return Kanban;

}());

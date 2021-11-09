window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.ListBox = (function () {
'use strict';

var CONTAINER = 'e-listbox-container';
var LIST = '.e-ul';
var LISTITEM = 'e-list-item';
var SELECTED = 'e-selected';
var DATAVALUE = 'data-value';
var DRAGEND = 'DragEndAsync';
var DOT = '.';
var PLACEHOLDER = 'e-placeholder';
var SPAN = 'span';
var BADGE = 'e-list-badge';
var PREVENT = 'e-drag-prevent';
var KEYDOWN = 'keydown';
var SORTABLE = 'sortable';
var EMPTY = '';
var UP = 38;
var DOWN = 40;
var KEYA = 65;
/**
 * Client side scripts for SfListBox
 */
var SfListBox = /** @class */ (function () {
    function SfListBox(element, scope, allowDragDrop, dotnetRef) {
        this.element = element;
        this.dotnetRef = dotnetRef;
        this.allowDragAndDrop = allowDragDrop;
        this.element.blazor__instance = this;
        this.scope = scope;
        sf.base.EventHandler.add(this.element, KEYDOWN, this.keyDownHandler, this);
        if (allowDragDrop) {
            this.initializeDraggable();
        }
    }
    SfListBox.prototype.initializeDraggable = function () {
        var _this = this;
        if (!this.allowDragAndDrop) {
            return;
        }
        var ul = sf.base.select(LIST, this.element);
        var sortable = new sf.lists.Sortable(ul, {
            scope: this.scope,
            itemClass: LISTITEM,
            beforeDragStart: this.triggerBeforeDragStart.bind(this),
            dragStart: this.triggerDragStart.bind(this),
            beforeDrop: this.dragEnd.bind(this),
            placeHolder: function () { return sf.base.createElement(SPAN, { className: PLACEHOLDER }); },
            helper: function (e) {
                var element = _this.element.cloneNode();
                var target = e.sender.cloneNode(true);
                element.appendChild(target);
                var refEle = sf.base.select(DOT + LISTITEM, _this.element);
                element.style.width = refEle.offsetWidth + 'px';
                element.style.height = refEle.offsetHeight + 'px';
                var selectedList = [].slice.call(sf.base.selectAll(DOT + LISTITEM + DOT + SELECTED, _this.element));
                if (selectedList.length && selectedList.length > 1 && selectedList.indexOf(e.sender) > -1) {
                    target.appendChild(sf.base.createElement(SPAN, { className: BADGE, innerHTML: selectedList.length.toString() }));
                }
                element.style.zIndex = sf.popups.getZindexPartial(_this.element).toString();
                return element;
            }
        });
        this.updateDraggable(sortable, this.allowDragAndDrop);
    };
    SfListBox.prototype.triggerBeforeDragStart = function (args) {
        args.cancel = args.target.classList.contains(PREVENT);
    };
    SfListBox.prototype.triggerDragStart = function (args) {
        args.bindEvents(args.dragElement);
    };
    SfListBox.prototype.dragEnd = function (args) {
        var isOutsideListbox = false;
        var scopedListBox = false;
        var sameListBox = false;
        var targetScope;
        var targetRef;
        var targetElement;
        targetElement = args.target.parentElement.parentElement;
        if (args.target.tagName == "UL" && args.target.className.indexOf("e-list-parent") > -1) {
            targetElement = args.target.parentElement;
        }
        // when filtering and toolbar enabled
        if (!targetElement.blazor__instance) {
            targetElement = targetElement.parentElement;
        }
        args.cancel = true;
        if (targetElement.blazor__instance) {
            targetScope = targetElement.blazor__instance.scope;
            targetRef = targetElement.blazor__instance.dotnetRef;
            if (this.scope === targetScope) {
                scopedListBox = true;
            }
            else {
                scopedListBox = false;
            }
            if (this.element === targetElement) {
                sameListBox = true;
                scopedListBox = false;
            }
        }
        else {
            isOutsideListbox = true;
        }
        this.dotnetRef.invokeMethodAsync(DRAGEND, isOutsideListbox, args.droppedElement.getAttribute(DATAVALUE), sameListBox, scopedListBox, args.previousIndex, args.currentIndex, targetRef);
    };
    SfListBox.prototype.keyDownHandler = function (e) {
        var target = e.target;
        if (e.keyCode === UP || e.keyCode === DOWN) {
            e.preventDefault();
            if (target.classList.contains(CONTAINER)) {
                var listEle = sf.base.select(DOT + LISTITEM, this.element);
                if (listEle) {
                    listEle.focus();
                }
            }
            else {
                var list = [].slice.call(sf.base.selectAll(DOT + LISTITEM, this.element));
                var index = list.indexOf(target);
                if (index < 0) {
                    return;
                }
                index = e.keyCode === UP ? index - 1 : index + 1;
                if (index < 0 || index > list.length - 1) {
                    return;
                }
                list[index].focus();
            }
        }
        else if (e.keyCode === KEYA && e.ctrlKey) {
            e.preventDefault();
        }
    };
    SfListBox.prototype.updateDraggable = function (sortable, allowDragAndDrop) {
        if (sortable) {
            var draggable = sf.base.getComponent(sortable.element, sf.base.Draggable);
            if (allowDragAndDrop) {
                draggable.abort = EMPTY;
            }
            else {
                draggable.abort = DOT + LISTITEM;
            }
        }
    };
    SfListBox.prototype.destroyDraggable = function () {
        var sortable = sf.base.getComponent(sf.base.select(LIST, this.element), SORTABLE);
        if (!sf.base.isNullOrUndefined(sortable)) {
            this.updateDraggable(sortable, this.allowDragAndDrop);
            sortable.destroy();
        }
    };
    SfListBox.prototype.destroy = function () {
        this.destroyDraggable();
        sf.base.EventHandler.remove(this.element, KEYDOWN, this.keyDownHandler);
    };
    SfListBox.prototype.onPropertyChanged = function (result) {
        this.allowDragAndDrop = result;
        if (result) {
            this.initializeDraggable();
        }
        else {
            this.destroyDraggable();
        }
    };
    SfListBox.prototype.getScopedElement = function (scope) {
        return sf.base.select("#" + scope).blazor__instance.dotnetRef;
    };
    return SfListBox;
}());
// eslint-disable-next-line @typescript-eslint/naming-convention, no-underscore-dangle, id-blacklist, id-match
var ListBox = {
    initialize: function (element, scope, allowDragDrop, dotnetRef) {
        if (!sf.base.isNullOrUndefined(element)) {
            new SfListBox(element, scope, allowDragDrop, dotnetRef);
        }
    },
    destroy: function (element) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            element.blazor__instance.destroy();
        }
    },
    onPropertyChanged: function (element, result) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) {
            element.blazor__instance.onPropertyChanged(result);
        }
    },
    getScopedListBox: function (element, scope) {
        return element.blazor__instance.getScopedElement(scope);
    }
};

return ListBox;

}());

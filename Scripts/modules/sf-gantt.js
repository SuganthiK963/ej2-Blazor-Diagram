window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Gantt = (function () {
'use strict';

/**
 * Splitter module is used to define the splitter position in Gantt layout.
 */
var Splitter = /** @class */ (function () {
    function Splitter(parent) {
        this.parent = parent;
    }
    Splitter.prototype.getTotalColumnWidthByIndex = function (index) {
        var width = 0;
        var tr = this.parent.treeGridElement.querySelectorAll('.e-headercell');
        index = tr.length > index ? index : tr.length;
        for (var column = 0; column < index; column++) {
            width = width + tr[column].offsetWidth;
        }
        return width;
    };
    return Splitter;
}());

/**
 * Specifies SfGantt class for native blazor rendering.
 * @hidden
 */
var SfGantt = /** @class */ (function () {
    function SfGantt(element, options, dotnetRef) {
        this.toolbarHeight = 0;
        this.chartPreviousScroll = { top: 0, left: 0 };
        this.treegridPreviousScroll = { top: 0, left: 0 };
        this.spinnerShown = false;
        this.scrollbarUpdate = false;
        this.element = element;
        this.element.blazor_instance = this;
        this.dotNetRef = dotnetRef;
        this.isFromTreeGrid = false;
        this.options = options;
        this.initModules();
    }
    SfGantt.prototype.initModules = function () {
        this.splitterModule = new Splitter(this);
    };
    SfGantt.prototype.getOffsetRect = function (element) {
        var box = element.getBoundingClientRect();
        var scrollTop = window.pageYOffset || document.documentElement.scrollTop
            || document.body.scrollTop;
        var scrollLeft = window.pageXOffset || document.documentElement.scrollLeft ||
            document.body.scrollLeft;
        var clientTop = document.documentElement.clientTop || document.body.clientTop || 0;
        var clientLeft = document.documentElement.clientLeft || document.body.clientLeft || 0;
        var top = box.top + scrollTop - clientTop;
        var left = box.left + scrollLeft - clientLeft;
        return { top: Math.round(top), left: Math.round(left), width: box.width, height: box.height };
    };
    SfGantt.prototype.getParentElement = function (elem, selector, isID) {
        var parent = elem;
        while (parent) {
            if (isID ? parent.id === selector : parent.classList.contains(selector)) {
                break;
            }
            parent = parent.parentElement;
        }
        return parent;
    };
    return SfGantt;
}());

/**
 * CSS Constants
 */
/** @hidden */

// Timeline-Class
var timelineSingleHeaderOuterDiv = 'e-timeline-single-header-outer-div';
var holidayLabel = 'e-span';
var taskBarMainContainer = 'e-taskbar-main-container';
var taskBarLeftResizer = 'e-taskbar-left-resizer';
var taskBarRightResizer = 'e-taskbar-right-resizer';
var childProgressResizer = 'e-child-progress-resizer';



var leftResizeGripper = 'e-left-resize-gripper';
var rightResizeGripper = 'e-right-resize-gripper';
var progressResizeGripper = 'e-progress-resize-gripper';
var chartBodyContainer = 'e-chart-root-container';
var chartRow = 'e-chart-row';
var leftLabelContainer = 'e-left-label-container';
var rightLabelContainer = 'e-right-label-container';
var traceChildProgressBar = 'e-gantt-child-progressbar';
var traceChildTaskBar = 'e-gantt-child-taskbar';
var traceParentTaskBar = 'e-gantt-parent-taskbar';
var traceParentProgressBar = 'e-gantt-parent-progressbar';
var Active = 'e-active-container';
var LeftLabel = 'e-left-label-inner-div';
var RightLabel = 'e-right-label-inner-div';
var connectorPointLeft = 'e-connectorpoint-left';
var connectorPointRight = 'e-connectorpoint-right';
var connectorPointLeftHover = 'e-connectorpoint-left-hover';
var connectorPointRightHover = 'e-connectorpoint-right-hover';
var falseLine = 'e-gantt-false-line';
var rightConnectorPointOuterDiv = 'e-right-connectorpoint-outer-div';
var connectorLineContainer = 'e-connector-line-container';
var connectorLineZIndex = 'e-connector-line-z-index';
var connectorPointAllowBlock = 'e-connectorpoint-allow-block';

/**
 * Splitter module is used to define the splitter position in Gantt layout.
 */
var ChartScroll = /** @class */ (function () {
    function ChartScroll(ganttParent, element, ganttHeight, contentHeight) {
        this.parent = ganttParent;
        this.chartElement = element.querySelector('.e-gantt-chart-pane');
        this.element = element.querySelector('.e-chart-scroll-container');
        this.timelineHeaderElement = element.querySelector('.e-timeline-header-container');
        this.holidaySpanElement = element.querySelectorAll('.' + holidayLabel);
        this.addEventListeners();
        this.ChartHeight(element, ganttHeight);
        if (!sf.base.isNullOrUndefined(this.holidaySpanElement)) {
            this.UpdateHolidayLabel(contentHeight);
        }
    }
    ChartScroll.prototype.addEventListeners = function () {
        sf.base.EventHandler.add(this.element, 'scroll', this.onScroll, this);
        sf.base.EventHandler.add(this.chartElement, 'mousedown', this.mouseDownHandler, this);
        sf.base.EventHandler.add(this.chartElement, sf.base.Browser.touchEndEvent, this.ganttChartMouseUp, this);
    };
    ChartScroll.prototype.removeEventListeners = function () {
        sf.base.EventHandler.remove(this.element, 'scroll', this.onScroll);
        sf.base.EventHandler.remove(this.chartElement, 'mousedown', this.mouseDownHandler);
        sf.base.EventHandler.remove(this.chartElement, sf.base.Browser.touchEndEvent, this.ganttChartMouseUp);
    };
    ChartScroll.prototype.gridScrollHandler = function (top) {
        this.element.scrollTop = top;
        this.parent.isFromTreeGrid = true;
    };
    ChartScroll.prototype.onScroll = function () {
        var parent = this.parent;
        parent.scrollbarUpdate = true;
        var isScrolling;
        if (this.element.scrollTop !== parent.chartPreviousScroll.top) {
            parent.dotNetRef.invokeMethodAsync('ShowSpinner');
        }
        window.clearTimeout(isScrolling);
        isScrolling = setTimeout(function () {
            if (!parent.onMouseDown && !parent.spinnerShown) {
                parent.dotNetRef.invokeMethodAsync("HideSpinner");
            }
        }, 500);
        if (this.element.scrollLeft !== this.parent.chartPreviousScroll.left) {
            this.timelineHeaderElement.scrollLeft = this.element.scrollLeft;
            this.parent.chartPreviousScroll.left = this.element.scrollLeft;
        }
        if (this.element.scrollTop !== this.parent.chartPreviousScroll.top) {
            if (!this.parent.isFromTreeGrid) {
                this.parent.treeGridModule.updateScrollTop(this.element.scrollTop);
            }
            this.parent.chartPreviousScroll.top = this.element.scrollTop;
            this.parent.isFromTreeGrid = false;
        }
    };
    ChartScroll.prototype.ganttChartMouseUp = function (e) {
        var parent = this.parent;
        parent.onMouseDown = false;
        parent.dotNetRef.invokeMethodAsync("HideSpinner");
    };
    ChartScroll.prototype.mouseDownHandler = function (e) {
        this.parent.onMouseDown = true;
        var ChartElement = this.parent.getParentElement(e.target, 'e-gantt-chart');
        if (ChartElement != null) {
            var target = null;
            var cellUid = null;
            if (this.parent.getParentElement(e.target, 'e-timeline-header-container')) {
                target = "Header";
            }
            else if (this.parent.getParentElement(e.target, 'e-content')) {
                target = "Content";
                cellUid = this.parent.getParentElement(e.target, 'e-chart-row-cell') ? this.parent.getParentElement(e.target, 'e-chart-row-cell').getAttribute('data-uid') : null;
            }
            if (target == "Header" || target == "Content") {
                this.parent.dotNetRef.invokeMethodAsync("ChartMouseDownHandler", target, cellUid);
            }
        }
    };
    ChartScroll.prototype.ChartHeight = function (element, ganttHeight) {
        var toolbarHeight = 0;
        this.toolbarElement = element.querySelector('#' + element.id + '_Gantt_Toolbar');
        if (!sf.base.isNullOrUndefined(this.toolbarElement)) {
            toolbarHeight = this.toolbarElement.offsetHeight;
        }
        this.viewPortHeight = element.blazor_instance.height - element.blazor_instance.toolbarHeight - this.timelineHeaderElement.offsetHeight;
        this.element.style.height = 'calc(100% - ' + this.timelineHeaderElement.offsetHeight + 'px)';
    };
    
    ChartScroll.prototype.UpdateHolidayLabel = function (contentHeight) {
        var length = this.holidaySpanElement.length;
        for (var label = 0; label < length; label++) {
            this.holidaySpanElement[label].style.top = sf.base.formatUnit((this.viewPortHeight < contentHeight) ? this.viewPortHeight / 2 : contentHeight / 2);
        }
    };
    ChartScroll.prototype.destroy = function () {
        this.removeEventListeners();
    };
    return ChartScroll;
}());

var TreeGrid = /** @class */ (function () {
    /**
    * Constructor for the Grid scrolling.
    * @hidden
    */
    function TreeGrid(ganttParent, content) {
        this.parent = ganttParent;
        this.content = content;
        this.addEventListeners();
    }
    TreeGrid.prototype.addEventListeners = function () {
        sf.base.EventHandler.add(this.content, 'scroll', this.scrollHandler, this);
    };
    TreeGrid.prototype.removeEventListeners = function () {
        sf.base.EventHandler.remove(this.content, 'scroll', this.scrollHandler);
    };
    TreeGrid.prototype.scrollHandler = function (e) {
        if (this.content.scrollTop !== this.parent.treegridPreviousScroll.top) {
            this.parent.chartScrollModule.gridScrollHandler(this.content.scrollTop);
        }
        this.parent.treegridPreviousScroll.top = this.content.scrollTop;
    };
    TreeGrid.prototype.updateScrollTop = function (top) {
        this.content.scrollTop = top;
        this.parent.treegridPreviousScroll.top = this.content.scrollTop;
    };
    TreeGrid.prototype.destroy = function () {
        this.removeEventListeners();
    };
    return TreeGrid;
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
var TaskbarEdit = /** @class */ (function () {
    /**
    * Constructor for the taskbar editing
    * @hidden
    */
    function TaskbarEdit(ganttParent) {
        this.elementOffsetLeft = 0;
        this.elementOffsetTop = 0;
        this.elementOffsetWidth = 0;
        this.elementOffsetHeight = 0;
        this.parent = ganttParent;
        this.initPublicProp();
        this.ganttChartTableBody = this.parent.element.querySelector("#" + this.parent.element.id + "_chartContentBody");
        this.chartPane = this.parent.element.querySelector(".e-gantt-chart-pane");
        this.chartBodyContainer = this.parent.element.querySelector("." + chartBodyContainer);
        this.addEventListeners();
    }
    TaskbarEdit.prototype.initPublicProp = function () {
        this.taskBarEditElement = null;
        this.taskBarEditRecord = null;
        this.taskBarEditAction = null;
        this.dragMouseLeave = false;
        this.isMouseDragged = false;
        this.connectorPointWidth = null;
        this.connectorSecondAction = null;
        this.toPredecessorText = null;
        this.highlightedSecondElement = null;
        this.falseLine = null;
    };
    TaskbarEdit.prototype.addEventListeners = function () {
        var isIE11Pointer = sf.base.Browser.isPointer;
        sf.base.EventHandler.add(this.chartPane, sf.base.Browser.touchStartEvent, this.ganttMouseDown, this);
        sf.base.EventHandler.add(this.chartPane, sf.base.Browser.touchMoveEvent, this.ganttMouseMove, this);
        // EventHandler.add(this.chartPane, mouseLeave, this.ganttMouseLeave, this);
        sf.base.EventHandler.add(this.chartPane, sf.base.Browser.touchEndEvent, this.ganttChartMouseUp, this);
    };
    TaskbarEdit.prototype.removeEventListeners = function () {
        var isIE11Pointer = sf.base.Browser.isPointer;
        sf.base.EventHandler.remove(this.chartPane, sf.base.Browser.touchStartEvent, this.ganttMouseDown);
        sf.base.EventHandler.remove(this.chartPane, sf.base.Browser.touchMoveEvent, this.ganttMouseMove);
        //  EventHandler.remove(this.chartPane, mouseLeave, this.ganttMouseLeave);
        sf.base.EventHandler.remove(this.chartPane, sf.base.Browser.touchEndEvent, this.ganttChartMouseUp);
    };
    TaskbarEdit.prototype.ganttMouseDown = function (e) {
        this.updateTaskBarEditElement(e);
    };
    TaskbarEdit.prototype.ganttChartMouseUp = function (e) {
        this.parent.dotNetRef.invokeMethodAsync('UpdateResizedData', this.dataGuid, this.taskBarEditAction, this.taskBarEditRecord);
        if (this.falseLine) {
            this.removeFalseLine(true);
        }
        this.initPublicProp();
    };
    TaskbarEdit.prototype.updateTaskBarEditElement = function (e) {
        var _this = this;
        var target = this.getElementByPosition(e);
        var element = this.parent.getParentElement(target, taskBarMainContainer);
        this.taskBarEditElement = element;
        if (this.taskBarEditElement) {
            if (e.type === 'mousedown' || e.type === 'touchstart' || e.type === 'click') {
                this.dataGuid = this.taskBarEditElement.getAttribute('rowuniqueid');
                this.parent.dotNetRef.invokeMethodAsync('GetConnectorValues').then(function (width) {
                    _this.connectorPointWidth = width;
                });
                this.parent.dotNetRef.invokeMethodAsync('GetEditedRecord', this.dataGuid).then(function (record) {
                    _this.taskBarEditRecord = record;
                    _this.updateMouseDownProperties(e);
                    _this.taskBarEditAction = _this.getTaskBarAction(e);
                });
            }
        }
    };
    TaskbarEdit.prototype.updateMouseDownProperties = function (event) {
        var e = this.getCoordinate(event);
        if (e.pageX || e.pageY) {
            var containerPosition = this.parent.getOffsetRect(this.chartBodyContainer);
            this.mouseDownX = (e.pageX - containerPosition.left) +
                this.parent.chartPreviousScroll.left;
            this.mouseDownY = e.pageY - containerPosition.top +
                this.parent.chartPreviousScroll.top;
        }
    };
    TaskbarEdit.prototype.getTaskBarAction = function (e) {
        var mouseDownElement = this.getElementByPosition(e);
        var data = this.taskBarEditRecord;
        var action = '';
        if (mouseDownElement.classList.contains(taskBarLeftResizer)) {
            action = 'LeftResizing';
        }
        else if (mouseDownElement.classList.contains(taskBarRightResizer)) {
            action = 'RightResizing';
        }
        else if (mouseDownElement.classList.contains(childProgressResizer) ||
            sf.base.closest(mouseDownElement, '.' + childProgressResizer)) {
            action = 'ProgressResizing';
        }
        else if (mouseDownElement.classList.contains(connectorPointLeft)) {
            action = 'ConnectorPointLeftDrag';
        }
        else if (mouseDownElement.classList.contains(connectorPointRight)) {
            action = 'ConnectorPointRightDrag';
        }
        else if (data) {
            action = data.hasChildRecords ? this.parent.options.taskMode === 'Auto' ? 'ParentDrag' : ''
                : data.isMilestone ? 'MilestoneDrag' : 'ChildDrag';
        }
        return action;
    };
    TaskbarEdit.prototype.getElementByPosition = function (event) {
        var e = this.getCoordinate(event);
        e.pageX = e.pageX != null ? e.pageX : 0;
        e.pageY = e.pageY != null ? e.pageY : 0;
        return document.elementFromPoint((e.pageX - window.pageXOffset), (e.pageY - window.pageYOffset));
    };
    // Get XY coordinates for touch and non-touch device
    TaskbarEdit.prototype.getCoordinate = function (event) {
        var coordinates = {};
        if (event) {
            var e = event;
            coordinates.pageX = e.pageX;
            coordinates.pageY = e.pageY;
        }
        return coordinates;
    };
    TaskbarEdit.prototype.ganttMouseMove = function (event) {
        var containerPosition = this.parent.getOffsetRect(this.chartBodyContainer);
        var e = this.getCoordinate(event);
        this.mouseMoveX = e.pageX - containerPosition.left +
            this.parent.chartPreviousScroll.left;
        this.mouseMoveY = e.pageY - containerPosition.top +
            this.parent.chartPreviousScroll.top;
        this.dragMouseLeave = false;
        this.isMouseDragCheck();
        if (this.isMouseDragged && this.taskBarEditAction) {
            this.taskBarEditingAction(event, false);
        }
    };
    TaskbarEdit.prototype.isMouseDragCheck = function () {
        if (!this.isMouseDragged && this.taskBarEditAction && ((this.mouseDownX !== this.mouseMoveX) &&
            ((this.mouseDownX + 3) < this.mouseMoveX || (this.mouseDownX - 3) > this.mouseMoveX)
            || (this.mouseDownY !== this.mouseMoveY) &&
                ((this.mouseDownY + 3) < this.mouseMoveY || (this.mouseDownY - 3) > this.mouseMoveY))) {
            this.isMouseDragged = true;
            var item = this.taskBarEditRecord;
            this.previousItem = __assign({}, item);
            this.taskBarEditElement.setAttribute('aria-grabbed', 'true');
        }
    };
    TaskbarEdit.prototype.updateMouseMoveProperties = function (event) {
        var containerPosition = this.parent.getOffsetRect(this.chartBodyContainer);
        var e = this.getCoordinate(event);
        if (e.pageX || e.pageY) {
            this.mouseMoveX = e.pageX - containerPosition.left +
                this.parent.chartPreviousScroll.left;
            this.mouseMoveY = e.pageY - containerPosition.top +
                this.parent.chartPreviousScroll.top;
        }
        if ((this.taskBarEditRecord.width > 3 && !((this.taskBarEditAction === 'ProgressResizing' &&
            (this.taskBarEditRecord.progress === 0 || this.taskBarEditRecord.progress === 100))))) {
            var mouseX = this.mouseMoveX - this.parent.chartPreviousScroll.left +
                containerPosition.left;
            var mouseY = this.mouseMoveY - this.parent.chartPreviousScroll.top +
                containerPosition.top;
            if ((mouseX + 20) >
                containerPosition.left + this.chartBodyContainer.offsetWidth) {
                this.timerCount = this.parent.chartPreviousScroll.left;
            }
            else if ((mouseX - 20) < containerPosition.left) {
                this.timerCount = this.parent.chartPreviousScroll.left;
            }
        }
    };
    TaskbarEdit.prototype.taskBarEditingAction = function (e, isMouseClick) {
        // let args: ITaskbarEditedEventArgs = {} as ITaskbarEditedEventArgs;
        this.updateMouseMoveProperties(e);
        if (this.taskBarEditAction === 'ProgressResizing') {
            this.performProgressResize(e);
        }
        else if (this.taskBarEditAction === 'LeftResizing') {
            this.enableLeftResizing(e);
        }
        else if (this.taskBarEditAction === 'RightResizing' || this.taskBarEditAction === 'ParentResizing') {
            this.enableRightResizing(e);
        }
        else if (this.taskBarEditAction === 'ParentDrag' || this.taskBarEditAction === 'ChildDrag' ||
            this.taskBarEditAction === 'MilestoneDrag') {
            this.enableDragging(e);
        }
        else if (this.taskBarEditAction === 'ConnectorPointLeftDrag' || this.taskBarEditAction === 'ConnectorPointRightDrag') {
            this.updateConnectorLineSecondProperties(e);
            this.triggerDependencyEvent(e);
            this.drawFalseLine();
        }
        this.setItemPosition();
    };
    TaskbarEdit.prototype.setItemPosition = function () {
        var item = this.taskBarEditRecord;
        var width = this.taskBarEditAction === 'MilestoneDrag' || item.isMilestone ?
            this.milestoneHeight : item.width;
        var rightResizer = (width - 10);
        var connectorResizer = (width - 2);
        var progressResizer = (item.progress / 100) * item.width;
        /* tslint:disable-next-line */
        var taskBarMainContainer$$1 = (!this.taskBarEditElement.classList.contains(taskBarMainContainer)) ? sf.base.closest(this.taskBarEditElement, 'tr.' + chartRow)
            .querySelector('.' + taskBarMainContainer) : this.taskBarEditElement;
        var leftLabelContainer$$1 = sf.base.closest(this.taskBarEditElement, 'tr.' + chartRow)
            .querySelector('.' + leftLabelContainer);
        var rightLabelContainer$$1 = sf.base.closest(this.taskBarEditElement, 'tr.' + chartRow)
            .querySelector('.' + rightLabelContainer);
        var traceChildProgressBar$$1 = this.taskBarEditElement.querySelector('.' + traceChildProgressBar);
        var traceChildTaskBar$$1 = this.taskBarEditElement.querySelector('.' + traceChildTaskBar);
        var childProgressResizer$$1 = this.taskBarEditElement.querySelector('.' + childProgressResizer);
        var taskBarRightResizer$$1 = this.taskBarEditElement.querySelector('.' + taskBarRightResizer);
        var traceParentTaskBar$$1 = this.taskBarEditElement.querySelector('.' + traceParentTaskBar);
        var traceParentProgressBar$$1 = this.taskBarEditElement.querySelector('.' + traceParentProgressBar);
        var rightConnectorPointOuterDiv$$1 = this.taskBarEditElement.querySelector('.' + rightConnectorPointOuterDiv);
        if (this.taskBarEditAction !== 'ParentResizing') {
            taskBarMainContainer$$1.style.width = (width) + 'px';
            taskBarMainContainer$$1.style.left = (item.left) + 'px';
            leftLabelContainer$$1.style.width = (item.left) + 'px';
            if (!sf.base.isNullOrUndefined(rightLabelContainer$$1)) {
                rightLabelContainer$$1.style.left = (item.left + width) + 'px';
            }
        }
        if (rightConnectorPointOuterDiv$$1) {
            rightConnectorPointOuterDiv$$1.style.left = connectorResizer + 'px';
        }
        if (taskBarRightResizer$$1) {
            taskBarRightResizer$$1.style.left = rightResizer + 'px';
        }
        if (this.taskBarEditAction === 'MilestoneDrag' || item.isMilestone) {
            taskBarMainContainer$$1.style.left = (item.left - (width / 2)) + 'px';
            leftLabelContainer$$1.style.width = (item.left - (width / 2)) + 'px';
            if (!sf.base.isNullOrUndefined(rightLabelContainer$$1)) {
                rightLabelContainer$$1.style.left = (item.left + (width / 2)) + 'px';
            }
        }
        else if (this.taskBarEditAction === 'ProgressResizing') {
            traceChildTaskBar$$1.style.left = (item.left + item.progressWidth - 10) + 'px';
            if (!sf.base.isNullOrUndefined(traceChildProgressBar$$1)) {
                traceChildProgressBar$$1.style.width = item.progressWidth + 'px';
                traceChildProgressBar$$1.style.borderBottomRightRadius = this.progressBorderRadius + 'px';
                traceChildProgressBar$$1.style.borderTopRightRadius = this.progressBorderRadius + 'px';
                childProgressResizer$$1.style.left = item.progressWidth - 8 + 'px';
            }
        }
        else if (this.taskBarEditAction === 'RightResizing') {
            traceChildTaskBar$$1.style.width = (width) + 'px';
            if (!sf.base.isNullOrUndefined(traceChildProgressBar$$1)) {
                traceChildProgressBar$$1.style.width = (progressResizer) + 'px';
                taskBarRightResizer$$1.style.left = rightResizer + 'px';
                childProgressResizer$$1.style.left = (progressResizer - 5) + 'px';
            }
        }
        else if (this.taskBarEditAction === 'ParentDrag') {
            if (!sf.base.isNullOrUndefined(traceParentTaskBar$$1)) {
                traceParentTaskBar$$1.style.width = (width) + 'px';
            }
            if (!sf.base.isNullOrUndefined(traceChildProgressBar$$1)) {
                traceParentProgressBar$$1.style.width = (item.progressWidth) + 'px';
            }
        }
        else {
            if (!sf.base.isNullOrUndefined(traceChildTaskBar$$1)) {
                traceChildTaskBar$$1.style.width = (width) + 'px';
            }
            if (!sf.base.isNullOrUndefined(traceChildProgressBar$$1)) {
                taskBarRightResizer$$1.style.left = rightResizer + 'px';
                traceChildProgressBar$$1.style.width = (progressResizer) + 'px';
                childProgressResizer$$1.style.left = progressResizer - 5 + 'px';
            }
        }
    };
    /**
     * To update left and width while perform taskbar left resize operation.
     * @return {void}
     * @private
     */
    TaskbarEdit.prototype.enableLeftResizing = function (e) {
        var item = this.taskBarEditRecord;
        var diffrenceWidth = 0;
        if (this.mouseDownX > this.mouseMoveX) {
            if (this.mouseMoveX < (item.left + item.width)) {
                diffrenceWidth = this.mouseDownX - this.mouseMoveX;
                if (item.left > 0) {
                    item.left = this.previousItem.left - diffrenceWidth;
                    item.width = this.previousItem.width + diffrenceWidth;
                }
            }
            else {
                if (this.mouseMoveX > (item.left + item.width)) {
                    diffrenceWidth = this.mouseDownX - this.mouseMoveX;
                    item.left = this.previousItem.left - diffrenceWidth;
                    item.width = 3;
                }
            }
        }
        else {
            if (this.mouseMoveX < (item.left + item.width)) {
                diffrenceWidth = this.mouseMoveX - this.mouseDownX;
                if ((item.left) < (item.left + item.width) &&
                    ((this.previousItem.left + diffrenceWidth) <= (this.previousItem.left + this.previousItem.width))) {
                    item.left = this.previousItem.left + diffrenceWidth;
                    item.width = this.previousItem.width - diffrenceWidth;
                }
            }
            else {
                diffrenceWidth = this.mouseMoveX - this.mouseDownX;
                item.left = this.previousItem.left + diffrenceWidth;
                item.width = 3;
            }
        }
        this.updateEditPosition(e, item);
        item.left = this.previousItem.left + this.previousItem.width - item.width;
    };
    TaskbarEdit.prototype.enableRightResizing = function (e) {
        var item = this.taskBarEditRecord;
        var differenceWidth = 0;
        if (this.mouseDownX > this.mouseMoveX) {
            if (this.mouseMoveX > item.left && (this.mouseDownX - this.mouseMoveX) > 3) {
                differenceWidth = this.mouseDownX - this.mouseMoveX;
                item.width = this.previousItem.width - differenceWidth;
            }
            else {
                if (this.mouseMoveX < item.left) {
                    item.width = 3;
                }
            }
        }
        else {
            if (this.mouseMoveX > item.left) {
                differenceWidth = this.mouseMoveX - this.mouseDownX;
                item.width = this.previousItem.width + differenceWidth;
            }
        }
        this.updateEditPosition(e, item);
    };
    TaskbarEdit.prototype.enableDragging = function (e) {
        var item = this.taskBarEditRecord;
        var differenceWidth = 0;
        if (this.mouseDownX > this.mouseMoveX) {
            differenceWidth = this.mouseDownX - this.mouseMoveX;
            if (differenceWidth > 0) {
                item.left = this.previousItem.left - differenceWidth;
            }
        }
        else {
            differenceWidth = this.mouseMoveX - this.mouseDownX;
            item.left = this.previousItem.left + differenceWidth;
        }
        var left = item.left < 0 ? 0 : (item.left + item.width) >= this.parent.totalTimelineWidth ?
            (this.parent.totalTimelineWidth - item.width) : item.left;
        item.left = left;
    };
    TaskbarEdit.prototype.performProgressResize = function (e) {
        var item = this.taskBarEditRecord;
        var diffrenceWidth = 0;
        if (this.mouseDownX > this.mouseMoveX) {
            if (this.mouseMoveX > item.left &&
                (this.mouseMoveX < (item.left + item.width)) && item.left > 0) {
                diffrenceWidth = this.mouseMoveX - item.left;
                item.progressWidth = diffrenceWidth;
            }
            else {
                if (this.mouseMoveX >= (item.left + item.width)) {
                    item.progressWidth = item.width;
                }
                else {
                    item.progressWidth = 0;
                }
            }
        }
        else {
            if (this.mouseMoveX > item.left &&
                (this.mouseMoveX < (item.left + item.width))) {
                diffrenceWidth = this.mouseMoveX - item.left;
                item.progressWidth = diffrenceWidth;
            }
            else {
                if (this.mouseMoveX <= item.left) {
                    item.progressWidth = 0;
                }
                else {
                    item.progressWidth = item.width;
                }
            }
        }
        var widthValue = item.progressWidth > item.width ?
            item.width : item.progressWidth;
        widthValue = item.progressWidth < 0 ? 0 : item.progressWidth;
        item.progressWidth = widthValue;
        var diff = item.width - item.progressWidth;
        if (diff <= 4) {
            this.progressBorderRadius = 4 - diff;
        }
        else {
            this.progressBorderRadius = 0;
        }
    };
    TaskbarEdit.prototype.updateEditPosition = function (e, item) {
        this.updateIsMilestone(item);
    };
    TaskbarEdit.prototype.updateIsMilestone = function (item) {
        if (item.width <= 3) {
            item.width = 3;
            item.isMilestone = true;
        }
        else {
            item.isMilestone = false;
        }
    };
    TaskbarEdit.prototype.updateConnectorLineSecondProperties = function (e) {
        var target = this.getElementByPosition(e);
        var element = this.parent.getParentElement(target, taskBarMainContainer);
        this.connectorSecondAction = null;
        var scrollTop = 0;
        if (this.parent.getParentElement(target, connectorPointLeft)) {
            this.connectorSecondAction = 'ConnectorPointLeftDrag';
            this.toPredecessorText = 'Start';
        }
        else if (this.parent.getParentElement(target, connectorPointRight)) {
            this.connectorSecondAction = 'ConnectorPointRightDrag';
            this.toPredecessorText = 'Finish';
        }
        else {
            this.connectorSecondAction = null;
            this.toPredecessorText = null;
        }
        if (this.taskBarEditElement !== element && this.taskBarEditElement !== this.highlightedSecondElement) {
            this.elementOffsetLeft = this.taskBarEditElement.offsetLeft;
            this.elementOffsetTop = this.taskBarEditElement.offsetTop + scrollTop;
            this.elementOffsetWidth = this.taskBarEditElement.offsetWidth;
            this.elementOffsetHeight = this.taskBarEditElement.offsetHeight;
            this.showHideTaskBarEditingElements(element, this.highlightedSecondElement);
        }
        if (sf.base.isNullOrUndefined(this.connectorSecondAction) && !sf.base.isNullOrUndefined(this.connectorSecondElement)) {
            this.connectorSecondElement.querySelector('.' + connectorPointLeft).classList.remove(connectorPointAllowBlock);
            this.connectorSecondElement.querySelector('.' + connectorPointRight).classList.remove(connectorPointAllowBlock);
        }
        this.connectorSecondElement = this.connectorSecondAction ? element : null;
        this.toDataGuid = sf.base.isNullOrUndefined(this.connectorSecondElement) ? null : this.connectorSecondElement.getAttribute('rowuniqueid');
    };
    
    TaskbarEdit.prototype.showHideTaskBarEditingElements = function (element, secondElement) {
        secondElement = secondElement ? secondElement : this.taskBarEditElement;
        if (secondElement && element !== secondElement) {
            if (secondElement.querySelector('.' + taskBarLeftResizer)) {
                secondElement.querySelector('.' + taskBarLeftResizer).classList.remove(leftResizeGripper);
                secondElement.querySelector('.' + taskBarRightResizer).classList.remove(rightResizeGripper);
                if (secondElement.querySelector('.' + childProgressResizer)) {
                    secondElement.querySelector('.' + childProgressResizer).classList.remove(progressResizeGripper);
                }
            }
            if ((secondElement.querySelector('.' + connectorPointLeft)
                || secondElement.parentElement.querySelector('.' + connectorPointLeft))) {
                var connectorElement = !sf.base.isNullOrUndefined(secondElement.querySelector('.' + connectorPointLeft)) ?
                    secondElement : secondElement.parentElement;
                connectorElement.querySelector('.' + connectorPointLeft).classList.remove(connectorPointLeftHover);
                connectorElement.querySelector('.' + connectorPointRight).classList.remove(connectorPointRightHover);
            }
        }
    };
    
    TaskbarEdit.prototype.triggerDependencyEvent = function (e, mouseUp) {
        var _this = this;
        if (this.toDataGuid != null) {
            this.parent.dotNetRef.invokeMethodAsync("DrawConnectorLine", this.taskBarEditAction, this.connectorSecondAction, this.toDataGuid).then(function (drawLine) {
                if (!drawLine) {
                    var target = _this.getElementByPosition(e);
                    target.classList.add(connectorPointAllowBlock);
                }
            });
        }
    };
    
    TaskbarEdit.prototype.drawFalseLine = function () {
        var x1 = this.mouseDownX;
        var y1 = this.mouseDownY;
        var x2 = this.mouseMoveX;
        var y2 = this.mouseMoveY;
        var length = Math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        var angle = Math.atan2(y2 - y1, x2 - x1) * 180 / Math.PI;
        var transform = 'rotate(' + angle + 'deg)';
        var left;
        if (this.taskBarEditAction === 'ConnectorPointLeftDrag') {
            left = (this.elementOffsetLeft - (this.connectorPointWidth / 2)) -
                this.parent.chartPreviousScroll.left;
        }
        if (this.taskBarEditAction === 'ConnectorPointRightDrag') {
            left = (this.elementOffsetLeft + this.elementOffsetWidth) +
                (this.connectorPointWidth / 2) - this.parent.chartPreviousScroll.left;
        }
        var top = ((this.elementOffsetTop) + (this.elementOffsetHeight / 2) + this.chartBodyContainer.offsetTop) - this.parent.chartPreviousScroll.top;
        this.removeFalseLine(false);
        this.falseLine = document.createElement('div');
        this.falseLine.className = falseLine;
        this.falseLine.id = 'ganttfalseline' + this.parent.element.id;
        this.falseLine.style.transformOrigin = '0% 100%';
        this.falseLine.style.right = 'auto';
        this.falseLine.style.position = 'absolute';
        this.falseLine.style.transform = transform;
        this.falseLine.style.borderTopWidth = '1px';
        this.falseLine.style.borderTopStyle = 'dashed';
        this.falseLine.style.zIndex = '5';
        this.falseLine.style.width = (length - 3) + 'px';
        this.falseLine.style.left = left + 'px';
        this.falseLine.style.top = top + 'px';
        this.chartBodyContainer.appendChild(this.falseLine);
    };
    
    TaskbarEdit.prototype.removeFalseLine = function (isRemoveConnectorPointDisplay) {
        if (this.falseLine) {
            sf.base.remove(this.falseLine);
            this.falseLine = null;
            if (isRemoveConnectorPointDisplay) {
                this.elementOffsetLeft = 0;
                this.elementOffsetTop = 0;
                this.elementOffsetWidth = 0;
                this.elementOffsetHeight = 0;
                if (!sf.base.isNullOrUndefined(this.chartBodyContainer.querySelector('.' + connectorLineContainer))) {
                    this.chartBodyContainer.querySelector('.' + connectorLineContainer).classList.remove(connectorLineZIndex);
                }
            }
        }
    };
    
    TaskbarEdit.prototype.destroy = function () {
        this.removeEventListeners();
    };
    return TaskbarEdit;
}());

/**
 * Keyboard module is used to define the keyboard interactions.
 */
var KeyboardHandler = /** @class */ (function () {
    function KeyboardHandler(parent) {
        this.parent = parent;
    }
    KeyboardHandler.prototype.taskbarFocus = function (tdElem) {
        var taskbarElem = tdElem.querySelector('.' + traceChildTaskBar);
        var parentTaskbarElem = tdElem.querySelector('.' + traceParentTaskBar);
        if (taskbarElem) {
            taskbarElem.focus();
            tdElem.querySelector('.' + taskBarMainContainer).querySelector('.' + traceChildTaskBar).classList.add(Active);
        }
        else {
            parentTaskbarElem.focus();
            tdElem.querySelector('.' + taskBarMainContainer).querySelector('.' + traceParentTaskBar).classList.add(Active);
        }
        return "isNextTaskbar";
    };
    KeyboardHandler.prototype.RightLabelFocus = function (tdElem) {
        var rightLabelElem = tdElem.querySelector('.' + rightLabelContainer);
        rightLabelElem.focus();
        rightLabelElem.querySelector('.e-label').classList.add(Active);
        return "isNextRightLabel";
    };
    KeyboardHandler.prototype.LeftLabelFocus = function (tdElem) {
        var leftLabelElem = tdElem.querySelector('.' + leftLabelContainer);
        leftLabelElem.focus();
        leftLabelElem.querySelector('.e-label').classList.add(Active);
        return "isNextLeftLabel";
    };
    return KeyboardHandler;
}());

/**
 * Blazor gantt interop handler
 */
// tslint:disable
var Gantt = {
    initialize: function (element, options, dotnetRef) {
        new SfGantt(element, options, dotnetRef);
        this.dotnetRef = dotnetRef;
        var offset = {};
        element.blazor_instance.height = offset.height = element.offsetHeight;
        element.blazor_instance.width = offset.width = element.offsetWidth;
        element.blazor_instance.toolbarElement = element.querySelector("#" + element.id + "_Gantt_Toolbar");
        if (!sf.base.isNullOrUndefined(element.blazor_instance.toolbarElement)) {
            element.blazor_instance.toolbarHeight = offset.toolbarHeight = element.blazor_instance.toolbarElement.offsetHeight;
        }
        return offset;
    },
    updatesplitterHeight: function (element) {
        var splitterElement = element.querySelector("#" + element.id + "_Gantt_Splitter");
        splitterElement.style.height = 'calc(100% - ' + element.blazor_instance.toolbarHeight + 'px)';
    },
    getTotalColumnWidthByIndex: function (element, index) {
        return element.blazor_instance.splitterModule.getTotalColumnWidthByIndex(index);
    },
    treegridDataBound: function (element, isSingleTier) {
        if (element.blazor_instance.scrollbarUpdate) {
            element.blazor_instance.spinnerShown = true;
        }
        this.treegridHeaderAlign(element, isSingleTier);
        var treegrid = document.getElementById("treeGrid" + element.id + "_gridcontrol");
        this.adjustTable(treegrid, element);
    },
    adjustTable: function adjustTable(treeElement, ganttElement) {
        var content = treeElement.querySelector(".e-gridcontent").querySelector('.e-virtualtable');
        var chartElement = document.getElementById(ganttElement.id + "_chart").querySelector('.e-virtualtable');
        if (!sf.base.isNullOrUndefined(content)) {
            chartElement.style.transform = content.style.transform;
        }
    },
    setChartHeight: function setChartHeight(element) {
        var treegrid = document.getElementById("treeGrid" + element.id + "_gridcontrol").querySelector(".e-gridcontent").querySelector('.e-virtualtrack');
        if (sf.base.isNullOrUndefined(treegrid)) {
            treegrid = document.getElementById("treeGrid" + element.id + "_gridcontrol_content_table");
        }
        if (treegrid && treegrid.clientHeight) {
            var chart = document.getElementById(element.id + "_chart");
            var chartContent = chart.getElementsByClassName("e-chart-rows-container")[0];
            chartContent.style.height = treegrid.clientHeight + "px";
        }
    },
    treegridHeaderAlign: function (element, isSingleTier) {
        if (isSingleTier) {
            sf.base.addClass(element.blazor_instance.treeGridElement.querySelectorAll('.e-headercell'), timelineSingleHeaderOuterDiv);
            sf.base.addClass(element.blazor_instance.treeGridElement.querySelectorAll('.e-columnheader'), timelineSingleHeaderOuterDiv);
            var gridHeaderHeight = element.querySelector('.e-gridheader').offsetHeight;
            var gridContent = element.querySelector('.e-gridcontent');
            gridContent.style.height = 'calc(100% - ' + gridHeaderHeight + 'px)';
        }
        else {
            sf.base.removeClass(element.blazor_instance.treeGridElement.querySelectorAll('.e-headercell'), timelineSingleHeaderOuterDiv);
            sf.base.removeClass(element.blazor_instance.treeGridElement.querySelectorAll('.e-columnheader'), timelineSingleHeaderOuterDiv);
        }
    },
    getTreeGrid: function (element) {
        this.hideTreeGridScrollBar(element);
        if (!sf.base.isNullOrUndefined(element.blazor_instance.treeGridElement)) {
            element.blazor_instance.treeGrid = element.blazor_instance.treeGridElement.blazor_instance;
        }
    },
    hideTreeGridScrollBar: function (element) {
        element.blazor_instance.treeGridElement = element.querySelector('#treeGrid' + element.id);
        var content = element.blazor_instance.treeGridElement.querySelector('.e-content');
        if (content) {
            content.style.height = '100%';
            element.querySelector('.e-gridcontent').style.height = 'calc(100% - ' + element.querySelector('.e-gridheader').offsetHeight + 'px)';
            element.blazor_instance.treeGridModule = new TreeGrid(element.blazor_instance, content);
        }
        var scrollWidth = this.getScrollbarWidth(element);
        if (scrollWidth !== 0) {
            content.style.cssText += 'width: calc(100% + ' + scrollWidth + 'px);';
        }
    },
    getScrollbarWidth: function (element) {
        var outer = document.createElement('div');
        outer.style.visibility = 'hidden';
        outer.style.overflow = 'scroll';
        outer.style.msOverflowStyle = 'scrollbar';
        var inner = document.createElement('div');
        outer.appendChild(inner);
        element.appendChild(outer);
        var scrollbarWidth = (outer.offsetWidth - inner.offsetWidth);
        outer.parentNode.removeChild(outer);
        return scrollbarWidth;
    },
    getOffsetLeft: function (element) {
        var box = element.getBoundingClientRect();
        var scrollLeft = window.pageXOffset || document.documentElement.scrollLeft ||
            document.body.scrollLeft;
        var clientLeft = document.documentElement.clientLeft || document.body.clientLeft || 0;
        var left = box.left + scrollLeft - clientLeft;
        return Math.round(left);
    },
    ChartInitialize: function (element, contentHeight, totalTimelineWidth, milestoneHeight) {
        element.blazor_instance.totalTimelineWidth = totalTimelineWidth;
        var ganttHeight = element.offsetHeight;
        if (element.blazor_instance.options.allowTaskbarEditing) {
            element.blazor_instance.taskbarEditModule = new TaskbarEdit(element.blazor_instance);
            element.blazor_instance.taskbarEditModule.milestoneHeight = milestoneHeight;
        }
        element.blazor_instance.chartScrollModule = new ChartScroll(element.blazor_instance, element, ganttHeight, contentHeight);
        element.blazor_instance.spinnerShown = false;
        return element.blazor_instance.chartScrollModule.viewPortHeight;
    },
    UpdateScroll: function (element, scrollBarValue) {
        this.scrollBarElement = element.querySelector('.e-chart-scroll-container');
        if (this.scrollBarElement.offsetWidth < scrollBarValue || this.scrollBarElement.scrollLeft > scrollBarValue) {
            this.scrollBarElement.scrollLeft = scrollBarValue;
        }
    },
    calcRowHeight: function (element) {
        return element.querySelector('.e-row').offsetHeight;
    },
    chartFocusOutHandler: function (id, element) {
        element.blazor_instance.keyboardModule = new KeyboardHandler(element.blazor_instance);
        var tdElement = element.querySelector(".e-chart-row-cell[data-uid=" + id + "]");
        var focusElement = tdElement.querySelector('.' + Active) ? tdElement.querySelector('.' + Active).parentElement : null;
        if (focusElement) {
            focusElement.querySelector('.e-label').classList.remove(Active);
            focusElement.blur();
        }
    },
    focusOutHandler: function (elem) {
        if (!sf.base.isNullOrUndefined(elem.classList)) {
            elem.classList.remove(Active);
        }
    },
    tabFocusHandler: function (id, elem, isShift) {
        elem.blazor_instance.keyboardModule = new KeyboardHandler(elem.blazor_instance);
        var tdElem = elem.querySelector(".e-chart-row-cell[data-uid=" + id + "]");
        var focusElem = tdElem.querySelector('.' + Active) ? tdElem.querySelector('.' + Active).parentElement : null;
        if (!focusElem) {
            var result = "isNextLeftLabel";
            if (!isShift && tdElem.querySelector('.' + LeftLabel)) {
                result = elem.blazor_instance.keyboardModule.LeftLabelFocus(tdElem);
            }
            else if (isShift && tdElem.querySelector('.' + RightLabel)) {
                result = elem.blazor_instance.keyboardModule.RightLabelFocus(tdElem);
            }
            else {
                result = elem.blazor_instance.keyboardModule.taskbarFocus(tdElem);
            }
            return result;
        }
        else if (focusElem.classList.contains(LeftLabel)) {
            if (isShift) {
                focusElem.querySelector('.e-label').classList.remove(Active);
                focusElem.blur();
                return "isNextGrid";
            }
            else {
                return elem.blazor_instance.keyboardModule.taskbarFocus(tdElem);
            }
        }
        else if (!focusElem.classList.length) {
            if (!isShift) {
                if (tdElem.querySelector('.e-right-label-inner-div')) {
                    return elem.blazor_instance.keyboardModule.RightLabelFocus(tdElem);
                }
                else {
                    return "isNextGrid";
                }
            }
            else {
                if (tdElem.querySelector('.e-left-label-inner-div')) {
                    return elem.blazor_instance.keyboardModule.LeftLabelFocus(tdElem);
                }
                else {
                    return "isNextGrid";
                }
            }
        }
        else if (focusElem.classList.contains(RightLabel)) {
            if (isShift) {
                return elem.blazor_instance.keyboardModule.taskbarFocus(tdElem);
            }
            else {
                return "isNextGrid";
            }
        }
        else {
            return "";
        }
    },
    getChartWidth: function (element) {
        return element.querySelector('.e-gantt-chart-pane').clientWidth;
    }
};

return Gantt;

}());

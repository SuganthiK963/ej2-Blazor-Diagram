window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.StockChart = (function () {
'use strict';

/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable max-len */
/**
 * StockChart native blazor source file
 */
var throttle = sf.base.isNullOrUndefined(window['_']) ? null : window['_'].throttle;
var SfStockChart = /** @class */ (function () {
    function SfStockChart(id, element, dotnetRef) {
        this.mouseY = 0;
        this.mouseX = 0;
        this.eventInterval = 80;
        this.mouseMoveRef = null;
        this.mouseLeaveRef = null;
        this.chartMouseWheelRef = null;
        this.domMouseMoveRef = null;
        this.domMouseUpRef = null;
        this.id = id;
        this.element = element;
        this.dotnetref = dotnetRef;
        // eslint-disable-next-line camelcase
        this.element.blazor__instance = this;
    }
    SfStockChart.prototype.render = function () {
        this.unWireEvents(this.id, this.dotnetref);
        this.wireEvents(this.id, this.dotnetref);
    };
    SfStockChart.prototype.destroy = function () {
        this.unWireEvents(this.id, this.dotnetref);
    };
    SfStockChart.prototype.unWireEvents = function (id, dotnetref) {
        var element = document.getElementById(id);
        if (!element) {
            return;
        }
        this.dotnetref = dotnetref;
        StockChart.dotnetrefCollection = StockChart.dotnetrefCollection.filter(function (item) {
            return item.id !== id;
        });
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        element.removeEventListener('mousemove', this.mouseMoveRef);
        element.removeEventListener('touchmove', this.mouseMoveRef);
        sf.base.EventHandler.remove(element, cancelEvent, this.mouseLeaveRef);
        var resize = sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        sf.base.EventHandler.remove(window, resize, StockChart.resizeBound);
    };
    SfStockChart.prototype.wireEvents = function (id, dotnetref) {
        var _this = this;
        var element = document.getElementById(id);
        if (!element) {
            return;
        }
        this.dotnetref = dotnetref;
        StockChart.dotnetrefCollection.push({ id: id, dotnetref: dotnetref });
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        this.mouseMoveRef = this.mouseMove.bind(this, dotnetref, id);
        this.mouseLeaveRef = this.mouseLeave.bind(this, dotnetref, id);
        element.addEventListener('mousemove', throttle(function (e) {
            _this.mouseMoveRef(e);
        }, this.eventInterval));
        element.addEventListener('touchmove', throttle(function (e) {
            _this.mouseMoveRef(e);
        }, this.eventInterval));
        sf.base.EventHandler.add(element, cancelEvent, this.mouseLeaveRef);
        StockChart.resizeBound = StockChart.chartResize.bind(this, StockChart.dotnetrefCollection);
        var resize = sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        sf.base.EventHandler.add(window, resize, StockChart.resizeBound);
    };
    SfStockChart.prototype.getEventArgs = function (e, id) {
        var clientX = e.changedTouches ? e.changedTouches[0].clientX : e.clientX;
        var clientY = e.changedTouches ? e.changedTouches[0].clientY : e.clientY;
        this.setMouseXY(clientX, clientY, id);
        var touches = e.touches; //pointerId
        var touchList = [];
        if (e.type.indexOf('touch') > -1) {
            for (var i = 0, length_1 = touches.length; i < length_1; i++) {
                touchList.push({ pageX: touches[i].clientX, pageY: touches[i].clientY, pointerId: e.pointerId || 0 });
            }
        }
        return {
            type: e.type,
            clientX: e.clientX,
            clientY: e.clientY,
            mouseX: this.mouseX,
            mouseY: this.mouseY,
            pointerType: e.pointerType,
            target: e.target.id,
            changedTouches: {
                clientX: e.changedTouches ? e.changedTouches[0].clientX : 0,
                clientY: e.changedTouches ? e.changedTouches[0].clientY : 0
            },
            touches: touchList,
            pointerId: e.pointerId
        };
    };
    SfStockChart.prototype.setMouseXY = function (pageX, pageY, id) {
        var svgRect = document.getElementById(id.replace('_stockChart_chart', '') + '_svg').getBoundingClientRect();
        var rect = document.getElementById(id).getBoundingClientRect();
        this.mouseY = (pageY - rect.top) - Math.max(svgRect.top - rect.top, 0);
        this.mouseX = (pageX - rect.left) - Math.max(svgRect.left - rect.left, 0);
    };
    SfStockChart.prototype.mouseMove = function (dotnetref, id, e) {
        var pageX;
        var pageY;
        var touchArg;
        if (e.type === 'touchmove') {
            this.isTouch = true;
            touchArg = e;
            pageX = touchArg.changedTouches[0].clientX;
            pageY = touchArg.changedTouches[0].clientY;
            e.preventDefault();
        }
        else {
            this.isTouch = e.pointerType === 'touch' || e.pointerType === '2' || this.isTouch;
            pageX = e.clientX;
            pageY = e.clientY;
        }
        this.dotnetref = dotnetref;
        if (document.getElementById(id.replace('_stockChart_chart', '') + '_svg')) {
            this.setMouseXY(pageX, pageY, id);
            dotnetref.invokeMethodAsync('OnStockChartMouseMove', this.getEventArgs(e, id));
        }
        return false;
    };
    SfStockChart.prototype.mouseLeave = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        dotnetref.invokeMethodAsync('OnStockChartMouseLeave', this.getEventArgs(e, id));
        return false;
    };
    return SfStockChart;
}());
var StockChart = {
    initialize: function (element, dotnetRef) {
        var instance = new SfStockChart(element.id, element, dotnetRef);
        instance.render();
    },
    destroy: function (element) {
        var currentInstance = element.blazor__instance;
        if (!sf.base.isNullOrUndefined(currentInstance)) {
            currentInstance.destroy();
        }
    },
    eventInterval: 80,
    dotnetref: {},
    dotnetrefCollection: [],
    renderTooltip: function (tooltipOptions, elementId, tooltipModule, element) {
        var svgElement = document.getElementById(elementId + '_svg');
        var firstRender = svgElement && parseInt(svgElement.getAttribute('opacity'), 10) > 0 ? false : true;
        var options = JSON.parse(tooltipOptions);
        var currentInstance = element.blazor__instance;
        if (firstRender && !sf.base.isNullOrUndefined(currentInstance)) {
            currentInstance.tooltip = new sf.svgbase.Tooltip(options);
            currentInstance.tooltip.appendTo('#' + elementId);
        }
        else if (!sf.base.isNullOrUndefined(currentInstance.tooltip)) {
            currentInstance.tooltip.location = new sf.svgbase.TooltipLocation(options.location.x, options.location.y);
            currentInstance.tooltip.content = options.content;
            currentInstance.tooltip.header = options.header;
            currentInstance.tooltip.offset = options.offset;
            currentInstance.tooltip.palette = options.palette;
            currentInstance.tooltip.shapes = options.shapes;
            currentInstance.tooltip.data = options.data;
            currentInstance.tooltip.template = options.template;
            currentInstance.tooltip.textStyle.color = options.textStyle.color || currentInstance.tooltip.textStyle.color;
            currentInstance.tooltip.textStyle.fontFamily = options.textStyle.fontFamily || currentInstance.tooltip.textStyle.fontFamily;
            currentInstance.tooltip.textStyle.fontStyle = options.textStyle.fontStyle || currentInstance.tooltip.textStyle.fontStyle;
            currentInstance.tooltip.textStyle.fontWeight = options.textStyle.fontWeight || currentInstance.tooltip.textStyle.fontWeight;
            currentInstance.tooltip.textStyle.opacity = options.textStyle.opacity || currentInstance.tooltip.textStyle.opacity;
            currentInstance.tooltip.textStyle.size = options.textStyle.size || currentInstance.tooltip.textStyle.size;
            currentInstance.tooltip.isNegative = options.isNegative;
            currentInstance.tooltip.clipBounds = new sf.svgbase.TooltipLocation(options.clipBounds.x, options.clipBounds.y);
            currentInstance.tooltip.arrowPadding = options.arrowPadding;
            currentInstance.tooltip.dataBind();
        }
    },
    fadeOut: function (element) {
        if (sf.base.isNullOrUndefined(element.blazor__instance) ||
            (!sf.base.isNullOrUndefined(element.blazor__instance) && sf.base.isNullOrUndefined(element.blazor__instance.tooltip))) {
            return;
        }
        var groupElement = document.getElementById(element.blazor__instance.tooltip.element.id + '_svg');
        if (groupElement && groupElement.getAttribute('opacity') === '0') {
            return;
        }
        element.blazor__instance.tooltip.fadeOut();
    },
    getParentElementBoundsById: function (id) {
        var element = document.getElementById(id);
        if (element && element.parentElement) {
            var width = element.parentElement.style.width;
            element.parentElement.style.width = '100%';
            var elementRect = element.parentElement.getBoundingClientRect();
            var styles = window.getComputedStyle(element.parentElement);
            var padding = parseFloat(styles.paddingLeft) + parseFloat(styles.paddingRight);
            var size = {
                width: Math.max(0, (elementRect.width - padding)),
                height: element.clientHeight || element.offsetHeight,
                left: elementRect.left,
                top: elementRect.top,
                right: elementRect.right,
                bottom: elementRect.bottom
            };
            element.parentElement.style.width = width;
            return size;
        }
        return { width: 0, height: 0, left: 0, top: 0, right: 0, bottom: 0 };
    },
    getElementBoundsById: function (id) {
        var element = document.getElementById(id);
        if (element) {
            var elementRect = element.getBoundingClientRect();
            return {
                width: element.clientWidth || element.offsetWidth,
                height: element.clientHeight || element.offsetHeight,
                left: elementRect.left,
                top: elementRect.top,
                right: elementRect.right,
                bottom: elementRect.bottom
            };
        }
        return { width: 0, height: 0, left: 0, top: 0, right: 0, bottom: 0 };
    },
    getBrowserDeviceInfo: function () {
        return {
            browserName: sf.base.Browser.info.name,
            isPointer: sf.base.Browser.isPointer,
            isDevice: sf.base.Browser.isDevice,
            isTouch: sf.base.Browser.isTouch,
            isIos: sf.base.Browser.isIos || sf.base.Browser.isIos7
        };
    },
    setAttribute: function (id, attribute, value) {
        var element = document.getElementById(id);
        if (element) {
            element.setAttribute(attribute, value);
        }
    },
    createTooltip: function (id, text, top, left, fontSize) {
        var tooltip = document.getElementById(id);
        var style = 'top:' + top.toString() + 'px;' +
            'left:' + left.toString() + 'px;' +
            'color:black !important; ' +
            'background:#FFFFFF !important; ' +
            'position:absolute;border:1px solid #707070;font-size:' + fontSize + ';border-radius:2px; z-index:1';
        if (!tooltip) {
            tooltip = sf.base.createElement('div', {
                id: id, innerHTML: '&nbsp;' + text + '&nbsp;', styles: style
            });
            document.body.appendChild(tooltip);
        }
        else {
            tooltip.setAttribute('innerHTML', '&nbsp;' + text + '&nbsp;');
            tooltip.setAttribute('styles', style);
        }
    },
    removeElement: function (id) {
        var element = this.getElement(id);
        if (element) {
            sf.base.remove(element);
        }
    },
    resizeBound: {},
    resize: {},
    chartResize: function (dotnetrefCollection, e) {
        var _this = this;
        if (this.resize) {
            clearTimeout(this.resize);
        }
        this.resize = setTimeout(function () {
            var count = dotnetrefCollection.length;
            var tempDotnetref;
            for (var i = 0; i < count; i++) {
                tempDotnetref = dotnetrefCollection[i].dotnetref;
                tempDotnetref.invokeMethodAsync('OnStockChartResize', e);
            }
            clearTimeout(_this.resize);
        }, 500);
        return false;
    }
};

return StockChart;

}());

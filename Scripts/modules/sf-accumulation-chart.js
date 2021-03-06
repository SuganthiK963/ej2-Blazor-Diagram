window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.AccumulationChart = (function () {
'use strict';

/**
 * AccumulationChart blazor source file
 */
var throttle = sf.base.isNullOrUndefined(window['_']) ? null : window['_'].throttle;
var ChartLocation = /** @class */ (function () {
    function ChartLocation(x, y) {
        this.x = x;
        this.y = y;
    }
    return ChartLocation;
}());
var SfAccumulationChart = /** @class */ (function () {
    function SfAccumulationChart(id, element, dotnetRef) {
        this.chartOnMouseDownRef = null;
        this.mouseMoveRef = null;
        this.mouseEndRef = null;
        this.chartOnMouseClickRef = null;
        this.chartRightClickRef = null;
        this.mouseLeaveRef = null;
        this.chartMouseWheelRef = null;
        this.domMouseMoveRef = null;
        this.domMouseUpRef = null;
        this.longPressBound = null;
        this.touchObject = null;
        this.mouseY = 0;
        this.mouseX = 0;
        this.eventInterval = 80;
        this.id = id;
        this.element = element;
        this.dotnetref = dotnetRef;
        this.element.blazor__instance = this;
    }
    SfAccumulationChart.prototype.render = function () {
        this.unWireEvents();
        this.wireEvents();
    };
    SfAccumulationChart.prototype.destroy = function () {
        this.unWireEvents();
    };
    SfAccumulationChart.prototype.wireEvents = function () {
        var _this = this;
        var element = document.getElementById(this.id);
        if (!element) {
            return;
        }
        AccumulationChart.dotnetrefCollection.push({ id: this.id, dotnetref: this.dotnetref });
        /*! Find the Events type */
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        this.chartOnMouseDownRef = this.chartOnMouseDown.bind(this, this.dotnetref, this.id);
        this.mouseMoveRef = this.mouseMove.bind(this, this.dotnetref, this.id);
        this.mouseEndRef = this.mouseEnd.bind(this, this.dotnetref, this.id);
        this.chartOnMouseClickRef = this.chartOnMouseClick.bind(this, this.dotnetref, this.id);
        this.chartRightClickRef = this.chartRightClick.bind(this, this.dotnetref, this.id);
        this.mouseLeaveRef = this.mouseLeave.bind(this, this.dotnetref, this.id);
        /*! Bind the Event handler */
        element.addEventListener('mousemove', throttle(function (e) {
            _this.mouseMoveRef(e);
        }, this.eventInterval));
        element.addEventListener('touchmove', throttle(function (e) {
            _this.mouseMoveRef(e);
        }, this.eventInterval));
        sf.base.EventHandler.add(element, sf.base.Browser.touchEndEvent, this.mouseEndRef);
        sf.base.EventHandler.add(element, 'click', this.chartOnMouseClickRef);
        sf.base.EventHandler.add(element, 'contextmenu', this.chartRightClickRef);
        sf.base.EventHandler.add(element, cancelEvent, this.mouseLeaveRef);
        AccumulationChart.resizeBound = AccumulationChart.chartResize.bind(this, AccumulationChart.dotnetrefCollection);
        var resize = (sf.base.Browser.isTouch && ('orientation' in window && 'onorientationchange' in window)) ? 'orientationchange' :
            'resize';
        sf.base.EventHandler.add(window, resize, AccumulationChart.resizeBound);
        this.longPressBound = this.longPress.bind(this, this.dotnetref, this.id);
        this.touchObject = new sf.base.Touch(element, { tapHold: this.longPressBound, tapHoldThreshold: 500 });
        /*! Apply the style for chart */
    };
    SfAccumulationChart.prototype.unWireEvents = function () {
        var _this = this;
        var element = document.getElementById(this.id);
        if (!element) {
            return;
        }
        AccumulationChart.dotnetrefCollection = AccumulationChart.dotnetrefCollection.filter(function (item) {
            return item.id !== _this.id;
        });
        /*! Find the Events type */
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        /*! Bind the Event handler */
        sf.base.EventHandler.remove(element, sf.base.Browser.touchStartEvent, this.chartOnMouseDownRef);
        element.removeEventListener('mousemove', this.mouseMoveRef);
        element.removeEventListener('touchmove', this.mouseMoveRef);
        sf.base.EventHandler.remove(element, sf.base.Browser.touchEndEvent, this.mouseEndRef);
        sf.base.EventHandler.remove(element, 'click', this.chartOnMouseClickRef);
        sf.base.EventHandler.remove(element, 'contextmenu', this.chartRightClickRef);
        sf.base.EventHandler.remove(element, cancelEvent, this.mouseLeaveRef);
        var resize = sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        sf.base.EventHandler.remove(window, resize, AccumulationChart.resizeBound);
        if (this.touchObject) {
            this.touchObject.destroy();
            this.touchObject = null;
        }
        /*! Apply the style for chart */
    };
    SfAccumulationChart.prototype.getEventArgs = function (e) {
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
            }
        };
    };
    SfAccumulationChart.prototype.setMouseXY = function (pageX, pageY) {
        var svgRect = document.getElementById(this.id + '_svg').getBoundingClientRect();
        var rect = document.getElementById(this.id).getBoundingClientRect();
        this.mouseY = (pageY - rect.top) - Math.max(svgRect.top - rect.top, 0);
        this.mouseX = (pageX - rect.left) - Math.max(svgRect.left - rect.left, 0);
    };
    SfAccumulationChart.prototype.chartOnMouseDown = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        this.id = id;
        this.dotnetref.invokeMethodAsync('OnChartMouseDown', this.getEventArgs(e));
        return false;
    };
    SfAccumulationChart.prototype.mouseMove = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        this.id = id;
        var pageX;
        var pageY;
        var touchArg;
        if (e.type === 'touchmove') {
            this.isTouch = true;
            touchArg = e;
            pageX = touchArg.changedTouches[0].clientX;
            pageY = touchArg.changedTouches[0].clientY;
        }
        else {
            this.isTouch = e.pointerType === 'touch' || e.pointerType === '2' || this.isTouch;
            pageX = e.clientX;
            pageY = e.clientY;
        }
        if (document.getElementById(this.id + '_svg')) {
            this.setMouseXY(pageX, pageY);
            this.dotnetref.invokeMethodAsync('OnChartMouseMove', this.getEventArgs(e));
        }
        return false;
    };
    SfAccumulationChart.prototype.mouseEnd = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        this.id = id;
        this.dotnetref.invokeMethodAsync('OnChartMouseEnd', this.getEventArgs(e));
        return false;
    };
    SfAccumulationChart.prototype.chartOnMouseClick = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        this.id = id;
        this.dotnetref.invokeMethodAsync('OnChartMouseClick', this.getEventArgs(e));
        return false;
    };
    SfAccumulationChart.prototype.chartRightClick = function (dotnetref, id, event) {
        this.dotnetref = dotnetref;
        this.id = id;
        this.dotnetref.invokeMethodAsync('OnChartRightClick', this.getEventArgs(event));
        return false;
    };
    SfAccumulationChart.prototype.mouseLeave = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        this.id = id;
        this.dotnetref.invokeMethodAsync('OnChartMouseLeave', this.getEventArgs(e));
        return false;
    };
    SfAccumulationChart.prototype.longPress = function (dotnetref, id, e) {
        this.dotnetref = dotnetref;
        this.id = id;
        this.dotnetref.invokeMethodAsync('OnChartLongPress', e);
        return false;
    };
    return SfAccumulationChart;
}());
var AccumulationChart = {
    initialize: function (element, dotnetRef) {
        var instance = new SfAccumulationChart(element.id, element, dotnetRef);
        instance.render();
    },
    destroy: function (element) {
        if (!sf.base.isNullOrUndefined(element.blazor__instance)) {
            element.blazor__instance.destroy();
        }
    },
    id: '',
    getParentElementBoundsById: function (id) {
        var element = document.getElementById(id);
        if (element) {
            element.style.width = '100%';
            element.style.height = '100%';
            var elementRect = element.getBoundingClientRect();
            return {
                width: elementRect.width || element.clientWidth || element.offsetWidth,
                height: elementRect.height || element.clientHeight || element.offsetHeight,
                left: elementRect.left,
                top: elementRect.top,
                right: elementRect.right,
                bottom: elementRect.bottom
            };
        }
        return { width: 0, height: 0, left: 0, top: 0, right: 0, bottom: 0 };
    },
    getElementBoundsById: function (id, isSetId) {
        if (isSetId === void 0) { isSetId = true; }
        if (isSetId) {
            this.id = id;
        }
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
    getAllCharacters: function () {
        var charCollection = [];
        for (var i = 33; i < 591; i++) {
            charCollection.push(String.fromCharCode(i));
        }
        return charCollection;
    },
    measureText: function (text, fontWeight, fontStyle, fontFamily) {
        var textObject = document.getElementById('chartmeasuretext');
        if (textObject === null) {
            textObject = sf.base.createElement('text', { id: 'chartmeasuretext' });
            document.body.appendChild(textObject);
        }
        if (text === ' ') {
            text = '&nbsp;';
        }
        textObject.innerHTML = text;
        textObject.style.position = 'fixed';
        textObject.style.fontSize = '100px';
        textObject.style.fontWeight = fontWeight;
        textObject.style.fontStyle = fontStyle;
        textObject.style.fontFamily = fontFamily;
        textObject.style.visibility = 'hidden';
        textObject.style.top = '-100';
        textObject.style.left = '0';
        textObject.style.whiteSpace = 'nowrap';
        textObject.style.lineHeight = 'normal';
        return {
            Width: textObject.clientWidth,
            Height: textObject.clientHeight
        };
    },
    getCharCollectionSize: function (fontkeys) {
        var charSizeList = [];
        var charSize;
        var tempSizeList = {};
        var charList = this.getAllCharacters();
        var charLength = charList.length;
        var fontKeysLength = fontkeys.length;
        for (var i = 0; i < fontKeysLength; i++) {
            var fontValues = fontkeys[i].split('_');
            var fontWeight = fontValues[0];
            var fontStyle = fontValues[1];
            var fontFamily = fontValues[2];
            for (var j = 0; j < charLength; j++) {
                charSize = this.measureText(charList[j], fontWeight, fontStyle, fontFamily);
                tempSizeList[charList[j]] = { X: charSize.Width, Y: charSize.Height };
            }
            charSizeList.push(tempSizeList);
        }
        var result = JSON.stringify(charSizeList);
        return result;
    },
    dotnetref: {},
    dotnetrefCollection: [],
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
                tempDotnetref.invokeMethodAsync('RemoveElements');
            }
            for (var i = 0; i < count; i++) {
                tempDotnetref = dotnetrefCollection[i].dotnetref;
                tempDotnetref.invokeMethodAsync('OnChartResize', e);
            }
            clearTimeout(_this.resize);
        }, 500);
        return false;
    },
    performAnimation: function (index, sliceId, startX, startY, endX, endY, duration, transform, isReverse) {
        var _this = this;
        var result = /translate\((-?\d+\.?\d*),?\s*(-?\d+[.]?\d*)?\)/.exec(transform);
        if (!sf.base.isNullOrUndefined(transform) && transform !== '') {
            endX = +result[1];
            endY = +result[2];
        }
        if (duration <= 0) {
            this.setElementTransform(sliceId, index, 'transform', 'translate(' + (endX) + ', ' + (endY) + ')');
            return null;
        }
        var xValue;
        var yValue;
        new sf.base.Animation({}).animate(sf.base.createElement('div'), {
            duration: duration,
            progress: function (args) {
                xValue = _this.linear(args.timeStamp, startX, endX, args.duration);
                yValue = _this.linear(args.timeStamp, startY, endY, args.duration);
                _this.setElementTransform(sliceId, index, 'transform', 'translate(' + (isReverse ? endX - xValue : xValue) + ', ' + (isReverse ? endY - yValue : yValue) + ')');
            },
            end: function (model) {
                _this.setElementTransform(sliceId, index, 'transform', 'translate(' + (isReverse ? startX : endX) + ', ' + (isReverse ? startX : endY) + ')');
            }
        });
    },
    setElementTransform: function (sliceId, index, attribute, value) {
        var chartID = sliceId.replace('Series_0', 'datalabel').replace('Point', 'Series_0');
        this.setElementAttribute(sliceId + index, 'transform', value);
        this.setElementAttribute(chartID + 'shape_' + index, 'transform', value);
        this.setElementAttribute(chartID + 'text_' + index, 'transform', value);
        this.setElementAttribute(chartID + 'connector_' + index, 'transform', value);
    },
    linear: function (currentTime, startValue, endValue, duration) {
        return -endValue * Math.cos(currentTime / duration * (Math.PI / 2)) + endValue + startValue;
    },
    setElementAttribute: function (id, attribute, value) {
        var element = document.getElementById(id);
        if (element) {
            element.setAttribute(attribute, value);
        }
    },
    getElementAttribute: function (id, attribute) {
        var element = document.getElementById(id);
        if (element) {
            return (element.getAttribute(attribute));
        }
        return '';
    },
    createStyleElement: function (styleId, styleInnerHTML) {
        document.body.appendChild(sf.base.createElement('style', { id: styleId, innerHTML: styleInnerHTML }));
    },
    renderTooltip: function (tooltipOptions, elementId, tooltipModule, element) {
        var svgElement = document.getElementById(elementId + '_svg');
        var firstRender = (svgElement && parseInt(svgElement.getAttribute('opacity'), 10) > 0) ? false : true;
        var options = JSON.parse(tooltipOptions);
        var currentInstance = element.blazor__instance;
        if (firstRender && !sf.base.isNullOrUndefined(currentInstance)) {
            currentInstance.tooltip = new sf.svgbase.Tooltip(options);
            currentInstance.tooltip.enableRTL = options.enableRTL;
            currentInstance.tooltip.tooltipRender = function () {
                tooltipModule.invokeMethodAsync('TooltipRender');
            };
            currentInstance.tooltip.animationComplete = function (args) {
                if (args.tooltip.fadeOuted) {
                    tooltipModule.invokeMethodAsync('TooltipAnimationComplete');
                }
            };
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
        this.removeTooltipCommentElement(element);
        element.blazor__instance.tooltip.fadeOut();
    },
    removeTooltipCommentElement: function (chartInstance) {
        var tooltipDivElement = document.getElementById(chartInstance.blazor__instance.tooltip.element.id);
        if (tooltipDivElement && !chartInstance.blazor__instance.isRemoveCommentElement && tooltipDivElement.childNodes.length > 1) {
            var tooltipElements = tooltipDivElement.childNodes;
            var commentElements = [];
            for (var i = 0; i < tooltipElements.length; i++) {
                if (tooltipElements[i].nodeName.match('#comment') || tooltipElements[i].nodeName.match('#text')) {
                    commentElements.push(tooltipElements[i]);
                }
            }
            for (var _i = 0, commentElements_1 = commentElements; _i < commentElements_1.length; _i++) {
                var element = commentElements_1[_i];
                sf.base.remove(element);
                chartInstance.blazor__instance.isRemoveCommentElement = true;
            }
        }
    },
    animateRedrawElement: function (elementId, duration, startX, startY, endX, endY, x, y) {
        var _this = this;
        if (x === void 0) { x = 'x'; }
        if (y === void 0) { y = 'y'; }
        var element = document.getElementById(elementId);
        if (!element) {
            return null;
        }
        var isDiv = element.tagName === 'DIV';
        var setStyle = function (xValue, yValue) {
            if (isDiv) {
                element.style[x] = xValue + 'px';
                element.style[y] = yValue + 'px';
            }
            else {
                element.setAttribute(x, xValue + '');
                element.setAttribute(y, yValue + '');
            }
        };
        setStyle(startX, startY);
        new sf.base.Animation({}).animate(sf.base.createElement('div'), {
            duration: duration,
            progress: function (args) {
                setStyle(_this.linear(args.timeStamp, startX, endX - startX, args.duration), _this.linear(args.timeStamp, startY, endY - startY, args.duration));
            },
            end: function () {
                setStyle(endX, endY);
            }
        });
    },
    //Pie Animation starts here
    doAnimation: function (sliceId, startAngle, totalAngle, animationDuration, animationDelay, legendDuration, radius, center) {
        var _this = this;
        var slice = document.getElementById(sliceId);
        startAngle -= 90;
        var duration = legendDuration ? legendDuration : animationDuration;
        var value;
        center['x'] += 1;
        radius += radius * (0.414); // formula r + r / 2 * (1.414 -1)
        // need to check animation type
        new sf.base.Animation({}).animate(slice, {
            duration: duration,
            delay: animationDelay,
            progress: function (args) {
                value = _this.linear(args.timeStamp, startAngle, totalAngle, args.duration);
                slice.setAttribute('d', _this.getPathArc(center, startAngle, value, radius, 0));
            },
            end: function (args) {
                center.x -= 1;
                slice.setAttribute('d', _this.getPathArc(center, startAngle, startAngle - 0.00009, radius, 0));
                var datalabels = document.getElementById(slice.id.split('_')[0] + '_datalabel_Series_0');
                if (datalabels) {
                    datalabels.setAttribute('style', 'visibility: visible');
                }
            }
        });
    },
    getPathArc: function (center, start, end, radius, innerRadius) {
        var degree = end - start;
        degree = degree < 0 ? (degree + 360) : degree;
        var flag = (degree < 180) ? 0 : 1;
        if (!innerRadius && innerRadius === 0) {
            return this.getPiePath(center, this.degreeToLocation(start, radius, center), this.degreeToLocation(end, radius, center), radius, flag);
        }
        else {
            return this.getDoughnutPath(center, this.degreeToLocation(start, radius, center), this.degreeToLocation(end, radius, center), radius, this.degreeToLocation(start, innerRadius, center), this.degreeToLocation(end, innerRadius, center), innerRadius, flag);
        }
    },
    getPiePath: function (center, start, end, radius, clockWise) {
        return 'M ' + center.x + ' ' + center.y + ' L ' + start.x + ' ' + start.y + ' A ' + radius + ' ' +
            radius + ' 0 ' + clockWise + ' 1 ' + end.x + ' ' + end.y + ' Z';
    },
    getDoughnutPath: function (center, start, end, radius, innerStart, innerEnd, innerRadius, clockWise) {
        return 'M ' + start.x + ' ' + start.y + ' A ' + radius + ' ' + radius + ' 0 ' + clockWise +
            ' 1 ' + end.x + ' ' + end.y + ' L ' + innerEnd.x + ' ' + innerEnd.y + ' A ' + innerRadius +
            ' ' + innerRadius + ' 0 ' + clockWise + ',0 ' + innerStart.x + ' ' + innerStart.y + ' Z';
    },
    degreeToLocation: function (degree, radius, center) {
        var radian = (degree * Math.PI) / 180;
        return new ChartLocation(Math.cos(radian) * radius + center.x, Math.sin(radian) * radius + center.y);
    },
    //Pie Animation end here
    /**
     * Pie Series Legend Click Animation
     */
    ChangePiePath: function (pointOptions, center, duration) {
        for (var _i = 0, pointOptions_1 = pointOptions; _i < pointOptions_1.length; _i++) {
            var point = pointOptions_1[_i];
            this.ChangePointPath(point.point, point.degree, point.start, point.pathOption, duration, center, point.radius, point.innerRadius);
        }
    },
    GetPathOption: function (center, degree, startAngle, radius, innerRadius) {
        if (!degree) {
            return '';
        }
        return this.getPathArc(center, startAngle, (startAngle + degree) % 360, radius, innerRadius);
    },
    ChangePointPath: function (point, degree, start, option, duration, center, radius, innerRadius) {
        var _this = this;
        var seriesElement = document.getElementById(option.id);
        var currentStartAngle;
        var curentDegree;
        new sf.base.Animation({}).animate(sf.base.createElement('div'), {
            duration: duration,
            delay: 0,
            progress: function (args) {
                curentDegree = _this.linear(args.timeStamp, point.degree, (degree - point.degree), args.duration);
                currentStartAngle = _this.linear(args.timeStamp, point.start, start - point.start, args.duration);
                currentStartAngle = ((currentStartAngle / (Math.PI / 180)) + 360) % 360;
                seriesElement.setAttribute('d', _this.GetPathOption(center, curentDegree, currentStartAngle, radius, innerRadius));
                if (point.isExplode) {
                    //chart.accBaseModule.explodePoints(point.index, chart, true);
                }
                seriesElement.style.visibility = 'visible';
            },
            end: function (args) {
                seriesElement.style.visibility = point.visible ? 'visible' : 'hidden';
                seriesElement.setAttribute('d', option.direction);
                point.degree = degree;
                point.start = start;
            }
        });
    }
};

return AccumulationChart;

}());

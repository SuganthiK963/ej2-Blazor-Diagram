window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.SmithChart = (function () {
'use strict';

/* eslint-disable max-len */
var SfSmithChart = /** @class */ (function () {
    function SfSmithChart(id, element, dotNetRef) {
        this.mouseY = 0;
        this.mouseX = 0;
        this.resizeTo = 0;
        this.id = id;
        this.element = element;
        this.dotNetRef = dotNetRef;
        // eslint-disable-next-line camelcase
        this.element.blazor__instance = this;
    }
    SfSmithChart.prototype.unWireEvents = function () {
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        sf.base.EventHandler.remove(this.element, sf.base.Browser.touchMoveEvent, this.mouseMove);
        sf.base.EventHandler.remove(this.element, 'click', this.chartOnMouseClick);
        sf.base.EventHandler.remove(this.element, cancelEvent, this.mouseLeave);
        window.removeEventListener((sf.base.Browser.isTouch && ('orientation' in window && 'onorientationchange' in window)) ? 'orientationchange' : 'resize', this.rangeResize.bind(this));
        sf.base.EventHandler.remove(this.element, 'mousedown', this.mouseDown);
        var keyboardModule = sf.base.getInstance(this.element, this.keyActionHandler);
        if (keyboardModule) {
            keyboardModule.destroy();
        }
        this.element = null;
        this.dotNetRef = null;
    };
    SfSmithChart.prototype.wireEvents = function () {
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        sf.base.EventHandler.add(this.element, sf.base.Browser.touchMoveEvent, this.mouseMove, this);
        sf.base.EventHandler.add(this.element, 'click', this.chartOnMouseClick, this);
        sf.base.EventHandler.add(this.element, cancelEvent, this.mouseLeave, this);
        window.addEventListener((sf.base.Browser.isTouch && ('orientation' in window && 'onorientationchange' in window)) ? 'orientationchange' : 'resize', this.rangeResize.bind(this));
        new sf.base.KeyboardEvents(this.element, { keyAction: this.keyActionHandler.bind(this), keyConfigs: { enter: 'enter' }, eventName: 'keydown' });
        sf.base.EventHandler.add(this.element, 'mousedown', this.mouseDown.bind(this), this);
    };
    SfSmithChart.prototype.mouseDown = function (event) {
        event.preventDefault();
    };
    SfSmithChart.prototype.keyActionHandler = function (event) {
        if (event.action === 'enter') {
            this.chartOnMouseClickProcess(event);
        }
    };
    SfSmithChart.prototype.mouseMove = function (e) {
        if (document.getElementById(this.id + '_svg')) {
            this.setMouseXY(e);
        }
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('OnSmithChartMouseMove', this.getEventArgs(e));
        }
    };
    SfSmithChart.prototype.chartOnMouseClick = function (e) {
        this.chartOnMouseClickProcess(e);
    };
    SfSmithChart.prototype.chartOnMouseClickProcess = function (e) {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('OnSmithChartMouseClick', this.getEventArgs(e));
        }
    };
    SfSmithChart.prototype.mouseLeave = function (e) {
        this.setMouseXY(e);
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('OnSmithChartMouseLeave', this.getEventArgs(e));
        }
    };
    SfSmithChart.prototype.rangeResize = function (e) {
        var _this = this;
        if (this.dotNetRef) {
            if (this.resizeTo) {
                clearTimeout(this.resizeTo);
            }
            this.resizeTo = window.setTimeout(function () { _this.dotNetRef.invokeMethodAsync('OnSmithChartResize', e); }, 500);
        }
    };
    SfSmithChart.prototype.setMouseXY = function (e) {
        var pageX;
        var pageY;
        var touchArg;
        if (e.type === 'touchmove') {
            touchArg = e;
            pageX = touchArg.changedTouches[0].clientX;
            pageY = touchArg.changedTouches[0].clientY;
        }
        else {
            pageX = e.clientX;
            pageY = e.clientY;
        }
        var svgRect = document.getElementById(this.id + '_svg').getBoundingClientRect();
        var rect = document.getElementById(this.id).getBoundingClientRect();
        this.mouseY = (pageY - rect.top) - Math.max(svgRect.top - rect.top, 0);
        this.mouseX = (pageX - rect.left) - Math.max(svgRect.left - rect.left, 0);
    };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    SfSmithChart.prototype.getEventArgs = function (e) {
        return {
            type: e.type,
            clientX: e.clientX ? e.clientX : 0,
            clientY: e.clientY ? e.clientY : 0,
            mouseX: this.mouseX,
            mouseY: this.mouseY,
            pointerType: e.pointerType ? e.pointerType : e.action ? e.action : '',
            target: e.target.id,
            changedTouches: {
                clientX: e.changedTouches ? e.changedTouches[0].clientX : 0,
                clientY: e.changedTouches ? e.changedTouches[0].clientY : 0
            }
        };
    };
    return SfSmithChart;
}());
// eslint-disable-next-line @typescript-eslint/no-explicit-any
var SmithChart = {
    id: '',
    mouseY: 0,
    mouseX: 0,
    initialize: function (id, dotNetRef, height, width, element) {
        if (element) {
            var navigator_1 = new SfSmithChart(id, element, dotNetRef);
            navigator_1.wireEvents();
            return this.getElementBoundsById(element, height, width);
        }
        return { width: 0, height: 0 };
    },
    getElementBoundsById: function (element, height, width) {
        if (element) {
            element.style.height = height;
            element.style.width = width;
            return { width: element.clientWidth || element.offsetWidth, height: element.clientHeight || element.offsetHeight };
        }
        return { width: 0, height: 0 };
    },
    charCollection: [
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '"', '#', '$', '%', '&', '(', ')', '*', '+', ',', '-', '.', '/', ':',
        ';', '<', '=', '>', '?', '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', '[', ']', '^', '_', '`', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
        'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~', ' '
    ],
    measureText: function (text, size, fontWeight, fontStyle, fontFamily) {
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
    getCharSizeByFontKeys: function (fontkeys) {
        var charSizeList = {};
        var charList = this.charCollection;
        var charLength = charList.length;
        var fontKeysLength = fontkeys.length;
        for (var i = 0; i < fontKeysLength; i++) {
            var fontValues = fontkeys[i].split('_');
            var fontWeight = fontValues[0];
            var fontStyle = fontValues[1];
            var fontFamily = fontValues[2];
            var charKey = '_' + fontWeight + '_' + fontStyle + '_' + fontFamily;
            for (var j = 0; j < charLength; j++) {
                charSizeList[charList[j] + charKey] = this.measureText(charList[j], '', fontWeight, fontStyle, fontFamily);
            }
        }
        return JSON.stringify(charSizeList);
    },
    getCharSizeByCharKey: function (charkey) {
        var fontValues = charkey.split('_');
        var char = fontValues[0];
        var size = fontValues[1];
        var fontWeight = fontValues[2];
        var fontStyle = fontValues[3];
        var fontFamily = fontValues[4];
        return this.measureText(char, size, fontWeight, fontStyle, fontFamily);
    },
    resizeTo: {},
    linear: function (currentTime, startValue, endValue, duration) {
        return -endValue * Math.cos(currentTime / duration * (Math.PI / 2)) + endValue + startValue;
    },
    reverselinear: function (currentTime, startValue, endValue, duration) {
        return -startValue * Math.sin(currentTime / duration * (Math.PI / 2)) + endValue + startValue;
    },
    doLinearAnimation: function (id, duration, isInverted) {
        var _this = this;
        var clipRect = document.getElementById(id);
        if (clipRect) {
            var animation = new sf.base.Animation({});
            var x_1 = +clipRect.getAttribute('x');
            var width_1 = +clipRect.getAttribute('width');
            animation.animate(clipRect, {
                duration: duration,
                delay: 0,
                progress: function (args) {
                    if (args.timeStamp >= args.delay) {
                        clipRect.setAttribute('visibility', 'visible');
                        if (isInverted) {
                            clipRect.setAttribute('width', (_this.linear(args.timeStamp - args.delay, 0, width_1, args.duration)).toString());
                        }
                        else {
                            clipRect.setAttribute('x', (_this.reverselinear(args.timeStamp - args.delay, width_1, 0, args.duration)).toString());
                        }
                    }
                },
                end: function () {
                    if (isInverted) {
                        clipRect.setAttribute('width', width_1.toString());
                    }
                    else {
                        clipRect.setAttribute('x', x_1.toString());
                    }
                }
            });
        }
        return clipRect == null;
    },
    getTemplateSize: function (id) {
        var element = document.getElementById(id);
        if (element) {
            return {
                width: element.offsetWidth,
                height: element.offsetHeight
            };
        }
        return null;
    },
    fadeOut: function (element) {
        if (sf.base.isNullOrUndefined(element.blazor__instance) ||
            (!sf.base.isNullOrUndefined(element.blazor__instance) && sf.base.isNullOrUndefined(element.blazor__instance.tooltip))) {
            return;
        }
        element.blazor__instance.tooltip.fadeOut();
    },
    tooltip: {},
    renderTooltip: function (tooltipOptions, elementId, element) {
        var svgElement = document.getElementById(elementId + '_svg');
        var firstRender = (svgElement && parseInt(svgElement.getAttribute('opacity'), 10) > 0) ? false : true;
        var options = JSON.parse(tooltipOptions);
        var currentInstance = element.blazor__instance;
        if (firstRender) {
            currentInstance.tooltip = new sf.svgbase.Tooltip(options);
            currentInstance.tooltip.appendTo('#' + elementId);
        }
        else if (!sf.base.isNullOrUndefined(currentInstance.tooltip)) {
            currentInstance.tooltip.location = new sf.svgbase.TooltipLocation(options.location.x, options.location.y);
            currentInstance.tooltip.content = options.content;
            currentInstance.tooltip.header = options.header;
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
            currentInstance.tooltip.dataBind();
        }
    },
    destroy: function (element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.unWireEvents();
        }
    }
};

return SmithChart;

}());

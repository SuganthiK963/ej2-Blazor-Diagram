window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.RangeNavigator = (function () {
'use strict';

/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable max-len */
/* eslint-disable camelcase */
/**
 * RangeNavigator blazor script file.
 */
var SfRangeNavigator = /** @class */ (function () {
    function SfRangeNavigator(id, element, dotNetRef) {
        this.mouseY = 0;
        this.mouseX = 0;
        this.reSizeTo = 0;
        this.isTooltipHide = true;
        this.isDrag = false;
        this.tooltip = [];
        this.id = id;
        this.element = element;
        this.dotNetRef = dotNetRef;
        this.element.blazor__instance = this;
    }
    SfRangeNavigator.prototype.destroy = function () {
        this.unWireEvents(this.id, this.dotNetRef);
    };
    SfRangeNavigator.prototype.unWireEvents = function (id, dotnetref) {
        this.dotNetRef = dotnetref;
        RangeNavigator.dotnetrefCollection = RangeNavigator.dotnetrefCollection.filter(function (item) {
            return item.id !== id;
        });
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        sf.base.EventHandler.remove(this.element, sf.base.Browser.touchStartEvent, this.rangeOnMouseDown);
        sf.base.EventHandler.remove(this.element, sf.base.Browser.touchMoveEvent, this.mouseMove);
        sf.base.EventHandler.remove(this.element, sf.base.Browser.touchEndEvent, this.mouseEnd);
        sf.base.EventHandler.remove(this.element, 'click', this.rangeOnMouseClick);
        sf.base.EventHandler.remove(this.element, cancelEvent, this.mouseLeave);
        // tslint:disable-next-line:max-line-length
        var resize = sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        sf.base.EventHandler.remove(window, resize, RangeNavigator.resizeBound);
    };
    SfRangeNavigator.prototype.wireEvents = function (id, dotnetref) {
        this.dotNetRef = dotnetref;
        RangeNavigator.dotnetrefCollection.push({ id: id, dotnetref: dotnetref });
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        sf.base.EventHandler.add(this.element, sf.base.Browser.touchStartEvent, this.rangeOnMouseDown, this);
        sf.base.EventHandler.add(this.element, sf.base.Browser.touchMoveEvent, this.mouseMove, this);
        sf.base.EventHandler.add(this.element, sf.base.Browser.touchEndEvent, this.mouseEnd, this);
        sf.base.EventHandler.add(this.element, 'click', this.rangeOnMouseClick, this);
        sf.base.EventHandler.add(this.element, cancelEvent, this.mouseLeave, this);
        // tslint:disable-next-line:max-line-length
        RangeNavigator.resizeBound = RangeNavigator.rangeResize.bind(this, RangeNavigator.dotnetrefCollection);
        var resize = sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        sf.base.EventHandler.add(window, resize, RangeNavigator.resizeBound);
    };
    SfRangeNavigator.prototype.rangeOnMouseDown = function (e) {
        this.setMouseX(this.getPageX(e));
        if (e.target.id.indexOf('_Thumb') > -1) {
            this.element.blazor__instance.isDrag = true;
        }
        this.dotNetRef.invokeMethodAsync('OnRangeMouseDown', this.getEventArgs(e));
    };
    SfRangeNavigator.prototype.mouseMove = function (e) {
        if (document.getElementById(this.id.replace('_stockChart_rangeSelector', '') + '_svg')) {
            this.setMouseX(this.getPageX(e));
            if (this.element.blazor__instance.isDrag && this.sliderChangeValue) {
                this.element.blazor__instance.changeSlider();
            }
        }
    };
    SfRangeNavigator.prototype.changeSlider = function () {
        var start;
        var end;
        if (this.sliderChangeValue.isDrag && this.mouseX >= this.sliderChangeValue.boundsX) {
            switch (this.sliderChangeValue.currentSlider) {
                case 'Left':
                    this.sliderChangeValue.startValue = this.getRangeValue(Math.abs(this.mouseX - this.sliderChangeValue.boundsX));
                    break;
                case 'Right':
                    this.sliderChangeValue.endValue = this.getRangeValue(Math.abs(this.mouseX - this.sliderChangeValue.boundsX));
                    break;
                case 'Middle':
                    start = Math.max(this.getRangeValue(Math.abs(this.sliderChangeValue.startX - (this.sliderChangeValue.previousMoveX - this.mouseX) - this.sliderChangeValue.boundsX)), this.sliderChangeValue.rangeMin);
                    end = Math.min(this.getRangeValue(Math.abs(this.sliderChangeValue.endX - (this.sliderChangeValue.previousMoveX - this.mouseX) - this.sliderChangeValue.boundsX)), this.sliderChangeValue.rangeMax);
                    if (Math.floor(Math.abs(this.getXLocation(end) - this.getXLocation(start))) === Math.floor(this.sliderChangeValue.sliderWidth)) {
                        this.sliderChangeValue.startValue = start;
                        this.sliderChangeValue.endValue = end;
                    }
                    break;
            }
            this.setSlider(this.sliderChangeValue.startValue, this.sliderChangeValue.endValue);
            this.sliderChangeValue.previousMoveX = this.mouseX;
        }
    };
    SfRangeNavigator.prototype.setSlider = function (start, end) {
        var blazor__instance = this.element.blazor__instance;
        var selectedElement = document.getElementById(blazor__instance.id + '_SelectedArea');
        var leftUnSelectedElement = document.getElementById(blazor__instance.id + '_leftUnSelectedArea');
        var rightUnSelectedElement = document.getElementById(blazor__instance.id + '_rightUnSelectedArea');
        var leftSlider = document.getElementById(blazor__instance.id + '_LeftSlider');
        var rightSlider = document.getElementById(blazor__instance.id + '_RightSlider');
        if (!(end >= start)) {
            start = [end, end = start][0];
        }
        var padding = this.sliderChangeValue.boundsX;
        start = end >= start ? start : [end, end = start][0];
        start = Math.max(start, this.sliderChangeValue.rangeMin);
        end = Math.min(end, this.sliderChangeValue.rangeMax);
        this.sliderChangeValue.startX = padding + this.getXLocation(start);
        this.sliderChangeValue.endX = padding + this.getXLocation(end);
        var selectedX = this.sliderChangeValue.enableRtl ? this.sliderChangeValue.endX : this.sliderChangeValue.startX;
        var rightPadding = this.sliderChangeValue.enableRtl ? this.sliderChangeValue.startX : this.sliderChangeValue.endX;
        this.sliderChangeValue.sliderWidth = Math.abs(this.sliderChangeValue.endX - this.sliderChangeValue.startX);
        selectedElement.setAttribute('x', (selectedX) + '');
        selectedElement.setAttribute('width', this.sliderChangeValue.sliderWidth + '');
        leftUnSelectedElement.setAttribute('width', (selectedX - padding) + '');
        rightUnSelectedElement.setAttribute('x', rightPadding + '');
        rightUnSelectedElement.setAttribute('width', (this.sliderChangeValue.boundsWidth - (rightPadding - padding)) + '');
        leftSlider.setAttribute('transform', 'translate(' + (this.sliderChangeValue.startX - this.sliderChangeValue.thumpPadding) + ', 0)');
        rightSlider.setAttribute('transform', 'translate(' + (this.sliderChangeValue.endX - this.sliderChangeValue.thumpPadding) + ', 0)');
        var left = 0;
        var leftX = this.sliderChangeValue.enableRtl ? this.sliderChangeValue.endX : this.sliderChangeValue.startX;
        var rightX = this.sliderChangeValue.enableRtl ? this.sliderChangeValue.startX : this.sliderChangeValue.endX;
        var leftRect = {
            x: this.sliderChangeValue.isLeightWeight ? left + padding : padding,
            y: this.sliderChangeValue.isLeightWeight ? 0 : this.sliderChangeValue.boundsY,
            width: this.sliderChangeValue.isLeightWeight ? leftX - padding : leftX,
            height: this.sliderChangeValue.isLeightWeight ? this.sliderChangeValue.thumpY : this.sliderChangeValue.boundsHeight
        };
        var rightRect = {
            x: this.sliderChangeValue.isLeightWeight ? left + rightX : rightX,
            y: this.sliderChangeValue.isLeightWeight ? 0 : this.sliderChangeValue.boundsY,
            width: (this.sliderChangeValue.boundsWidth - (rightPadding - padding)),
            height: this.sliderChangeValue.isLeightWeight ? this.sliderChangeValue.thumpY : this.sliderChangeValue.boundsHeight
        };
        var midRect = {
            x: this.sliderChangeValue.isLeightWeight ? leftX + left : 0,
            y: this.sliderChangeValue.isLeightWeight ? 0 : this.sliderChangeValue.boundsY,
            width: this.sliderChangeValue.isLeightWeight ? Math.abs(this.sliderChangeValue.endX - this.sliderChangeValue.startX) : rightX,
            height: this.sliderChangeValue.isLeightWeight ? this.sliderChangeValue.thumpY : this.sliderChangeValue.boundsHeight
        };
        if (blazor__instance.tooltip.length > 0) {
            this.updateTooltip(leftRect, rightRect, midRect, this.sliderChangeValue.startX, this.sliderChangeValue.endX);
        }
    };
    SfRangeNavigator.prototype.updateTooltip = function (leftRect, rightRect, midRect, start, end) {
        var blazor__instance = this.element.blazor__instance;
        var content = this.getTooltipContent(this.sliderChangeValue.endValue);
        var rect = this.sliderChangeValue.enableRtl ? leftRect : rightRect;
        var textStyle = blazor__instance.tooltip[0].textStyle;
        if (RangeNavigator.measureText(content, textStyle.size, textStyle.fontWeight, textStyle.fontStyle, textStyle.fontFamily).Width
            > rect.width) {
            rect = midRect;
        }
        blazor__instance.tooltip[0].location.x = end;
        blazor__instance.tooltip[0].areaBounds = rect;
        blazor__instance.tooltip[0].content = [content];
        blazor__instance.tooltip[0].dataBind();
        content = this.getTooltipContent(this.sliderChangeValue.startValue);
        rect = this.sliderChangeValue.enableRtl ? rightRect : leftRect;
        textStyle = blazor__instance.tooltip[1].textStyle;
        if (RangeNavigator.measureText(content, textStyle.size, textStyle.fontWeight, textStyle.fontStyle, textStyle.fontFamily).Width
            > rect.width) {
            rect = midRect;
            rect.x = blazor__instance.sliderChangeValue.isLeightWeight ? 0 : rect.x;
        }
        blazor__instance.tooltip[1].location.x = start;
        blazor__instance.tooltip[1].content = [content];
        blazor__instance.tooltip[1].areaBounds = rect;
        blazor__instance.tooltip[1].dataBind();
    };
    SfRangeNavigator.prototype.getTooltipContent = function (point) {
        var format = this.sliderChangeValue.format;
        var isCustom = format.match('{value}') !== null;
        if (this.sliderChangeValue.valueType === 'DateTime') {
            return (new sf.base.Internationalization().getDateFormat({ format: format || 'MM/dd/yyyy' })(new Date(point)));
        }
        else {
            return new sf.base.Internationalization().getNumberFormat({
                format: isCustom ? '' : format,
                useGrouping: this.sliderChangeValue.useGrouping
            })(this.sliderChangeValue.valueType === 'Logarithmic' ? Math.pow(this.sliderChangeValue.logBase, point) : point);
        }
    };
    SfRangeNavigator.prototype.getRangeValue = function (x) {
        return (!this.sliderChangeValue.enableRtl ? x / this.sliderChangeValue.boundsWidth : (1 - (x / this.sliderChangeValue.boundsWidth))) * this.sliderChangeValue.rangeDelta + this.sliderChangeValue.rangeMin;
    };
    SfRangeNavigator.prototype.getXLocation = function (x) {
        var result = (x - this.sliderChangeValue.rangeMin) / this.sliderChangeValue.rangeDelta;
        return (this.sliderChangeValue.enableRtl ? (1 - result) : result) * this.sliderChangeValue.boundsWidth;
    };
    SfRangeNavigator.prototype.mouseEnd = function (e) {
        if (this.element.blazor__instance.isDrag && this.sliderChangeValue) {
            this.dotNetRef.invokeMethodAsync('SetStartEndValue', this.sliderChangeValue.startValue, this.sliderChangeValue.endValue, true, this.sliderChangeValue.enableTooltip);
            this.element.blazor__instance.isDrag = false;
        }
        this.dotNetRef.invokeMethodAsync('OnRangeMouseEnd', this.getEventArgs(e));
    };
    SfRangeNavigator.prototype.rangeOnMouseClick = function (e) {
        this.dotNetRef.invokeMethodAsync('OnRangeMouseClick', this.getEventArgs(e));
    };
    SfRangeNavigator.prototype.mouseLeave = function (e) {
        this.setMouseX(this.getPageX(e));
        this.element.blazor__instance.isDrag = false;
        this.dotNetRef.invokeMethodAsync('OnRangeMouseLeave', this.getEventArgs(e));
        if (this.isTooltipHide) {
            this.fadeOutTooltip();
        }
    };
    SfRangeNavigator.prototype.fadeOutTooltip = function () {
        if (this.sliderChangeValue && this.sliderChangeValue.isTooltipHide) {
            window.clearInterval(this.toolTipInterval);
            var blazor__instance_1 = this.element.blazor__instance;
            if (blazor__instance_1.tooltip[1]) {
                this.toolTipInterval = window.setTimeout(function () {
                    blazor__instance_1.tooltip[0].fadeOut();
                    blazor__instance_1.tooltip[1].fadeOut();
                }, 1000);
            }
        }
    };
    SfRangeNavigator.prototype.getPageX = function (e) {
        if (e.type === 'touchmove') {
            return e.changedTouches[0].clientX;
        }
        else {
            return e.clientX;
        }
    };
    SfRangeNavigator.prototype.setMouseX = function (pageX) {
        var svgElement = document.getElementById(this.id.replace('_stockChart_rangeSelector', '') + '_svg');
        if (svgElement) {
            var svgRect = svgElement.getBoundingClientRect();
            var rect = document.getElementById(this.id).getBoundingClientRect();
            this.mouseX = (pageX - rect.left) - Math.max(svgRect.left - rect.left, 0);
        }
    };
    SfRangeNavigator.prototype.getEventArgs = function (e) {
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
    return SfRangeNavigator;
}());
// tslint:disable
var RangeNavigator = {
    getParentElementBoundsById: function (id, dotNetRef) {
        var element = document.getElementById(id);
        if (element) {
            var navigator_1 = new SfRangeNavigator(id, element, dotNetRef);
            navigator_1.unWireEvents(id, dotNetRef);
            navigator_1.wireEvents(id, dotNetRef);
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
    getElementBoundsById: function (id, dotNetRef, element, height, width) {
        element = document.getElementById(id);
        if (element) {
            var navigator_2 = new SfRangeNavigator(id, element, dotNetRef);
            navigator_2.unWireEvents(id, dotNetRef);
            navigator_2.wireEvents(id, dotNetRef);
            return this.getElementSize(element, height, width);
        }
        return { width: 0, height: 0 };
    },
    getElementSize: function (element, height, width) {
        if (element) {
            element.style.height = height;
            element.style.width = width;
            return { width: element.clientWidth || element.offsetWidth, height: element.clientHeight || element.offsetHeight };
        }
        return { width: 0, height: 0 };
    },
    destroy: function (element) {
        var currentInstance = element.blazor__instance;
        if (!sf.base.isNullOrUndefined(currentInstance)) {
            currentInstance.destroy();
        }
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
        textObject.style.fontSize = size;
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
        var fontKeysLength = fontkeys.length;
        for (var i = 0; i < fontKeysLength; i++) {
            var fontValues = fontkeys[i].split('_');
            var fontWeight = fontValues[0];
            var fontStyle = fontValues[1];
            var fontFamily = fontValues[2];
            var charKey = '_' + fontWeight + '_' + fontStyle + '_' + fontFamily;
            for (var j = 0; j < charList.length; j++) {
                charSizeList[charList[j] + charKey] = this.measureText(charList[j], '100px', fontWeight, fontStyle, fontFamily);
            }
        }
        return JSON.stringify(charSizeList);
    },
    getCharSizeByCharKey: function (charkey) {
        var fontValues = charkey.split('_');
        return this.measureText(fontValues[0], '100px', fontValues[2], fontValues[3], fontValues[4]);
    },
    dotnetref: {},
    setValueOnSliderSelect: function (element, sliderChangeValue) {
        element = document.getElementById(element.id);
        var blazor__instance = element.blazor__instance;
        if (!sf.base.isNullOrUndefined(blazor__instance)) {
            blazor__instance.sliderChangeValue = sliderChangeValue;
            blazor__instance.isTooltipHide = sliderChangeValue.isTooltipHide;
            blazor__instance.isDrag = true;
        }
    },
    getElementRect: function (id) {
        var element = document.getElementById(id);
        var rect;
        if (element) {
            rect = element.getBoundingClientRect();
            sf.base.remove(element);
        }
        return {
            Left: rect.left,
            Right: rect.right,
            Top: rect.top,
            Bottom: rect.bottom,
            Width: rect.width,
            Height: rect.height
        };
    },
    dotnetrefCollection: [],
    renderTooltip: function (leftTooltipOption, rightTooltipOption, leftElementId, rightElementId, element) {
        var svgElement;
        var firstRender;
        var idCollection = [leftElementId, rightElementId];
        var tooltipOptions = [leftTooltipOption, rightTooltipOption];
        var id;
        var options;
        element = document.getElementById(element.id);
        for (var i = 1; i >= 0; i--) {
            id = idCollection[i];
            svgElement = document.getElementById(id.replace('_stockChart_rangeSelector', '') + '_svg');
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            firstRender = svgElement && parseInt(svgElement.getAttribute('opacity'), 10) > 0 ? false : true;
            options = JSON.parse(tooltipOptions[i]);
            element.blazor__instance.tooltip[i] = new sf.svgbase.Tooltip(options);
            element.blazor__instance.tooltip[i].appendTo('#' + id);
        }
    },
    performSliderAnimation: function (element, start, end, animationDuration, currentStart, currentEnd, enableTooltip, allowSnapping, rangeSliderModule, sliderChangeValue) {
        var _this = this;
        element = document.getElementById(element.id);
        var blazor__instance = element.blazor__instance;
        blazor__instance.sliderChangeValue = sliderChangeValue;
        new sf.base.Animation({}).animate(sf.base.createElement('div'), {
            duration: animationDuration,
            progress: function (args) {
                var aStart = _this.linear(args.timeStamp, 0, start - currentStart, args.duration) + currentStart;
                var aEnd = _this.linear(args.timeStamp, 0, end - currentEnd, args.duration) + currentEnd;
                blazor__instance.setSlider(aStart, aEnd);
            },
            end: function () {
                blazor__instance.setSlider(start, end);
                if (allowSnapping) {
                    rangeSliderModule.invokeMethodAsync('PerformSnapping', start, end, true, enableTooltip);
                }
                else {
                    blazor__instance.dotNetRef.invokeMethodAsync('SetStartEndValue', start, end, true, enableTooltip);
                }
            }
        });
    },
    linear: function (currentTime, startValue, endValue, duration) {
        return -endValue * Math.cos(currentTime / duration * (Math.PI / 2)) + endValue + startValue;
    },
    setAttribute: function (id, attribute, value) {
        var element = document.getElementById(id);
        if (element) {
            element.setAttribute(attribute, value);
        }
    },
    getAndSetTextContent: function (id, get, value) {
        var element = document.getElementById(id);
        if (element) {
            if (get) {
                return element.textContent;
            }
            else {
                element.textContent = value;
            }
        }
        return null;
    },
    resizeBound: {},
    resize: {},
    rangeResize: function (dotnetrefCollection, e) {
        var _this = this;
        this.element.blazor__instance.isDrag = false;
        if (this.resize) {
            clearTimeout(this.resize);
        }
        this.resize = setTimeout(function () {
            var count = dotnetrefCollection.length;
            var tempDotnetref;
            for (var i = 0; i < count; i++) {
                tempDotnetref = dotnetrefCollection[i].dotnetref;
                if (dotnetrefCollection[i].id.indexOf('_stockChart_') < 0) {
                    tempDotnetref.invokeMethodAsync('OnRangeResize', e);
                }
            }
            clearTimeout(_this.resize);
        }, 500);
        return false;
    }
};

return RangeNavigator;

}());

window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.HeatMap = (function () {
'use strict';

var ABSOLUTE = 'absolute';
var NOWRAP = 'nowrap';
var DEFAULT_WIDTH = 600;
var DEFAULT_HEIGHT = 450;
var TRIPLE_DOT = '...';
var SfHeatMap = /** @class */ (function () {
    function SfHeatMap(element, dotnetRef) {
        this.resizeTimer = 0;
        this.previousCellID = "";
        this.element = element;
        this.element.blazorInstance = this;
        this.dotnetRef = dotnetRef;
    }
    SfHeatMap.prototype.initialize = function (property) {
        this.dotnetRef.invokeMethodAsync('CalculateSize', this.getElementBounds(this.element), this.getMaxLabelSize(property.XAxisLabels, property.XAxisTextStyles), this.getMaxLabelSize(property.YAxisLabels, property.YAxisTextStyles), this.getMaxLabelSize([property.TitleText], property.TitleStyle), this.getMaxLabelSize(property.LegendLabels, property.LegendTextStyle));
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        sf.base.EventHandler.add(this.element, 'click', this.heatMapMouseClick, this);
        sf.base.EventHandler.add(this.element, sf.base.Browser.touchStartEvent, this.heatMapMouseMove, this);
        sf.base.EventHandler.add(this.element, cancelEvent, this.heatMapMouseLeave, this);
        sf.base.EventHandler.add(this.element, sf.base.Browser.touchMoveEvent, this.heatMapMouseMove, this);
        var resize = sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        sf.base.EventHandler.add(window, resize, this.resizeBound.bind(this), this);
    };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    SfHeatMap.prototype.heatMapMouseClick = function (e) {
        this.dotnetRef.invokeMethodAsync('HeatMapMouseClick', this.getEventArgs(e), this.getElementBounds(this.element));
    };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    SfHeatMap.prototype.heatMapMouseLeave = function (e) {
        this.dotnetRef.invokeMethodAsync('HeatMapMouseLeave', this.getEventArgs(e), this.getElementBounds(this.element));
    };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    SfHeatMap.prototype.heatMapMouseMove = function (e) {
        this.dotnetRef.invokeMethodAsync('HeatMapMouseMove', this.getEventArgs(e), this.getElementBounds(this.element));
    };
    SfHeatMap.prototype.resizeBound = function () {
        var _this = this;
        if (this.resizeTimer) {
            clearTimeout(this.resizeTimer);
        }
        if (this.element) {
            this.resizeTimer = setTimeout(function () { return _this.dotnetRef.invokeMethodAsync('ResizeBound', _this.getElementBounds(_this.element).height, _this.getElementBounds(_this.element).width, _this.getElementBounds(_this.element).parentHeight); }, 500);
        }
    };
    SfHeatMap.prototype.setMouseXY = function (pageX, pageY, id) {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        var svgRect = document.getElementById(id).getBoundingClientRect();
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        var rect = document.getElementById(id).getBoundingClientRect();
        this.mouseY = pageY - rect.top - Math.max(svgRect.top - rect.top, 0);
        this.mouseX = pageX - rect.left - Math.max(svgRect.left - rect.left, 0);
    };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    SfHeatMap.prototype.renderTooltip = function (element, elementId, tooltipOptions, args, cellHighlighting, isCellSelected, textStyle, border, theme) {
        var svgElement = document.getElementById(elementId + '_svg');
        var firstRender = svgElement && parseInt(svgElement.getAttribute('opacity'), 10) > 0 ? false : true;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        var options = typeof tooltipOptions == 'string' ? JSON.parse(tooltipOptions) : tooltipOptions;
        if (cellHighlighting && !isCellSelected) {
            this.updateCellHighlight(tooltipOptions.id);
        }
        if (theme && theme == "TailwindDark") {
            options.fill = "#F9FAFB";
        }
        if (theme && theme == "Bootstrap5Dark") {
            options.fill = "#E9ECEF";
        }
        var currentInstance = element.blazorInstance;
        options.content = options.xLabel + options.yLabel + options.value;
        if (firstRender && !sf.base.isNullOrUndefined(currentInstance)) {
            options.content = options.displayText;
            options.location = { x: options.x + (options.width / 2), y: options.y + (options.height / 2) };
            options.areaBounds = { height: args.height + args.y, width: args.width, x: args.x };
            options.opacity = (theme == 'Tailwind' || theme === 'TailwindDark' || theme === 'Bootstrap5' || theme === 'Bootstrap5Dark') ? 1 : 0.75;
            currentInstance.tooltip = new sf.svgbase.Tooltip(options);
            currentInstance.tooltip.appendTo('#' + elementId);
        }
        else if (!sf.base.isNullOrUndefined(currentInstance.tooltip)) {
            if (!sf.base.isNullOrUndefined(textStyle)) {
                currentInstance.tooltip.border.color = border.color || currentInstance.tooltip.border.color;
                currentInstance.tooltip.textStyle.color = textStyle.color || currentInstance.tooltip.textStyle.color;
                currentInstance.tooltip.textStyle.fontFamily = textStyle.fontFamily || currentInstance.tooltip.textStyle.fontFamily;
                currentInstance.tooltip.textStyle.fontStyle = textStyle.fontStyle || currentInstance.tooltip.textStyle.fontStyle;
                currentInstance.tooltip.textStyle.fontWeight = textStyle.fontWeight || currentInstance.tooltip.textStyle.fontWeight;
                currentInstance.tooltip.textStyle.size = textStyle.size || currentInstance.tooltip.textStyle.size;
            }
            if (theme && theme == "Tailwind") {
                currentInstance.tooltip.textStyle.color = "#FFFFFF";
                currentInstance.tooltip.textStyle.fontWeight = "500";
                currentInstance.tooltip.textStyle.size = "12px";
                currentInstance.tooltip.textStyle.fontFamily = "Inter";
            }
            if (theme && theme == "TailwindDark") {
                currentInstance.tooltip.textStyle.color = "#1F2937";
                currentInstance.tooltip.textStyle.fontWeight = "500";
                currentInstance.tooltip.textStyle.size = "12px";
                currentInstance.tooltip.textStyle.fontFamily = "Inter";
            }
            if (theme && theme == "Bootstrap5") {
                currentInstance.tooltip.textStyle.color = "#F9FAFB";
                currentInstance.tooltip.textStyle.fontWeight = "500";
                currentInstance.tooltip.textStyle.size = "14px";
                currentInstance.tooltip.textStyle.fontFamily = "Helvetica Neue";
            }
            if (theme && theme == "Bootstrap5Dark") {
                currentInstance.tooltip.textStyle.color = "#212529";
                currentInstance.tooltip.textStyle.fontWeight = "500";
                currentInstance.tooltip.textStyle.size = "14px";
                currentInstance.tooltip.textStyle.fontFamily = "system-ui, -apple-system, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, 'Noto Sans', 'Liberation Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji'";
            }
            options.toolTipX = options.x + options.width / 2;
            options.toolTipY = options.y + options.height / 2;
            options.content = options.displayText;
            currentInstance.tooltip.location = new sf.svgbase.TooltipLocation(options.toolTipX, options.toolTipY);
            currentInstance.tooltip.content = [options.content];
            currentInstance.tooltip.dataBind();
        }
    };
    SfHeatMap.prototype.updateCellHighlight = function (cellID) {
        if (this.previousCellID !== cellID) {
            if (this.previousCellID != "") {
                this.setOpacity(document.getElementById(this.previousCellID), '1');
            }
            this.setOpacity(document.getElementById(cellID), '0.65');
            this.previousCellID = cellID;
        }
    };
    SfHeatMap.prototype.setOpacity = function (element, value) {
        element.setAttribute('opacity', value);
    };
    SfHeatMap.prototype.getElementBounds = function (element) {
        var width = this.stringToNumber(this.width, element.offsetWidth) || element.offsetWidth || DEFAULT_WIDTH;
        var height = this.stringToNumber(this.height, element.offsetHeight) || DEFAULT_HEIGHT;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        var elementRect = element.getBoundingClientRect();
        return {
            width: width,
            height: height,
            parentHeight: (element.parentElement) ? element.parentElement.offsetHeight : height,
            left: Math.ceil(elementRect.left),
            top: Math.ceil(elementRect.top),
            right: Math.ceil(elementRect.right),
            bottom: Math.ceil(elementRect.bottom)
        };
    };
    SfHeatMap.prototype.stringToNumber = function (value, containerSize) {
        if (!sf.base.isNullOrUndefined(value)) {
            return value.indexOf('%') !== -1 ? (containerSize / 100) * parseInt(value, 10) : parseInt(value, 10);
        }
        return null;
    };
    SfHeatMap.prototype.getMaxLabelSize = function (AxisLabels, fontModel) {
        var LabelSize = {
            height: 0,
            width: 0,
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            parentHeight: 0
        };
        for (var i = 0; i < AxisLabels.length; i++) {
            var currentLabelSize = this.measureText(AxisLabels[i], fontModel);
            if (LabelSize.height < currentLabelSize.height) {
                LabelSize.height = currentLabelSize.height;
            }
            if (LabelSize.width < currentLabelSize.width) {
                LabelSize.width = currentLabelSize.width;
            }
        }
        return LabelSize;
    };
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    SfHeatMap.prototype.getEventArgs = function (e) {
        var clientX = e.changedTouches ? e.changedTouches[0].clientX : e.clientX;
        var clientY = e.changedTouches ? e.changedTouches[0].clientY : e.clientY;
        this.setMouseXY(clientX, clientY, e.currentTarget.id);
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        var touches = e.touches;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        var touchList = [];
        if (e.type.indexOf('touch') > -1) {
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            for (var i = 0, length1 = touches.length; i < length1; i++) {
                touchList.push({
                    pageX: touches[i].clientX,
                    pageY: touches[i].clientY,
                    pointerId: e.pointerId || 0
                });
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
            pointerId: e.pointerId,
            ctrlKey: e.ctrlKey
        };
    };
    SfHeatMap.prototype.labelInterSect = function (dotnetRef, xAxis, yAxis, CellWidth) {
        var isInterSect = false;
        for (var i = 0; i < xAxis[0].length; i++) {
            var size = this.measureText(xAxis[0][i], xAxis[1]).width;
            if (size > CellWidth) {
                isInterSect = true;
                break;
            }
        }
        var xlabels = this.returnLabels(xAxis[0], xAxis[1], xAxis[2]);
        var ylabels = this.returnLabels(yAxis[0], yAxis[1], yAxis[2]);
        this.dotnetRef.invokeMethodAsync('LabelInterSect', isInterSect, xlabels, ylabels);
    };
    SfHeatMap.prototype.returnLabels = function (labels, textStyle, maxLabelWidth) {
        for (var i = 0; i < labels.length; i++) {
            var text = labels[i];
            var size = this.measureText(text, textStyle).width;
            if (size > maxLabelWidth) {
                var textLength = text.length;
                for (var index = textLength - 1; index >= 0; --index) {
                    text = text.substring(0, index) + TRIPLE_DOT;
                    size = this.measureText(text, textStyle).width;
                    if (size <= maxLabelWidth) {
                        break;
                    }
                }
            }
            labels[i] = text;
        }
        return labels;
    };
    SfHeatMap.prototype.measureText = function (text, fontValues) {
        var htmlObject = document.getElementById('heatmapmeasuretext');
        if (htmlObject === null) {
            htmlObject = sf.base.createElement('text', { id: 'heatmapmeasuretext' });
            document.body.appendChild(htmlObject);
        }
        htmlObject.setAttribute('style', 'position:' + ABSOLUTE + ';visibility:hidden;font-size:' + fontValues.size + ';font-weight:' + fontValues.fontWeight + ';font-style:' + fontValues.fontStyle + ';font-family:' + fontValues.fontFamily + ';top:-100;left:0;white-space:' + NOWRAP + ';line-height:normal');
        htmlObject.innerText = text;
        var currentSize = {
            height: htmlObject.clientHeight,
            width: htmlObject.clientWidth,
            top: null,
            left: null,
            right: null,
            bottom: null,
            parentHeight: null
        };
        return currentSize;
    };
    SfHeatMap.prototype.destroy = function () {
        var cancelEvent = sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave';
        sf.base.EventHandler.remove(this.element, 'click', this.heatMapMouseClick);
        sf.base.EventHandler.remove(this.element, sf.base.Browser.touchStartEvent, this.heatMapMouseMove);
        sf.base.EventHandler.remove(this.element, cancelEvent, this.heatMapMouseLeave);
        sf.base.EventHandler.remove(this.element, sf.base.Browser.touchMoveEvent, this.heatMapMouseMove);
        var resize = sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize';
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        sf.base.EventHandler.remove(window, resize, this.resizeBound.bind(this));
        this.element = null;
    };
    return SfHeatMap;
}());
// tslint:disable-next-line
var HeatMap = {
    initialize: function (element, dotnetRef, property) {
        new SfHeatMap(element, dotnetRef);
        if (element && element.blazorInstance) {
            element.blazorInstance.initialize(property);
        }
    },
    labelInterSect: function labelInterSect(element, dotnetRef, xAxis, yAxis, CellWidth) {
        if (element && element.blazorInstance) {
            element.blazorInstance.labelInterSect(dotnetRef, xAxis, yAxis, CellWidth);
        }
    },
    renderTooltip: function renderTooltip(element, elementId, 
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    tooltipModule, tooltipOptions, args, cellHighlighting, isCellSelected, textStyle, border, theme) {
        if (element && element.blazorInstance) {
            element.blazorInstance.renderTooltip(element, elementId, tooltipOptions, args, cellHighlighting, isCellSelected, textStyle, border, theme);
        }
    },
    fadeOut: function fadeOut(element) {
        if (element && element.blazorInstance) {
            if (sf.base.isNullOrUndefined(element.blazorInstance) || !sf.base.isNullOrUndefined(element.blazorInstance)
                && sf.base.isNullOrUndefined(element.blazorInstance.tooltip)) {
                return;
            }
            if (element.blazorInstance.previousCellID != "") {
                element.blazorInstance.setOpacity(document.getElementById(element.blazorInstance.previousCellID), '1');
                element.blazorInstance.previousCellID = "";
            }
            element.blazorInstance.tooltip.fadeOut();
        }
    },
    destroy: function destroy(element) {
        if (element && element.blazorInstance) {
            element.blazorInstance.destroy();
        }
    }
};

return HeatMap;

}());

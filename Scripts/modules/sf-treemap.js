window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Treemap = (function () {
'use strict';

/* eslint-disable @typescript-eslint/member-delimiter-style */
/* eslint-disable brace-style */
/* eslint-disable @typescript-eslint/naming-convention */
/* eslint-disable @typescript-eslint/explicit-member-accessibility */
/* eslint-disable max-len */
var SELECTION = 'Selection';
var HIGHLIGHT = 'Highlight';
var TREEMAPHIGHLIGHT = 'treeMapHighlight';
var TREEMAPSELECTION = 'treeMapSelection';
var RECTPATH = '_RectPath';
var LEGENDHIGHLIGHT = 'LegendHighlight';
var TEXT = '_Text';
var SfTreemap = /** @class */ (function () {
    function SfTreemap(element, dotNetRef) {
        this.resizeTo = 0;
        this.element = element;
        this.dotNetRef = dotNetRef;
        this.element.blazor__instance = this;
    }
    SfTreemap.prototype.initializeEvents = function () {
        sf.base.EventHandler.add(this.element, 'mouseup', this.mouseUp.bind(this), this);
        sf.base.EventHandler.add(this.element, 'mousemove', this.mouseMove.bind(this), this);
        sf.base.EventHandler.add(this.element, 'mousedown', this.mouseDown.bind(this), this);
        sf.base.EventHandler.add(this.element, 'mouseleave', this.mouseLeave.bind(this), this);
        sf.base.EventHandler.add(this.element, 'contextmenu', this.contextMenuEvent.bind(this), this);
        window.addEventListener('resize', this.resize.bind(this));
        new sf.base.KeyboardEvents(this.element, { keyAction: this.keyActionHandler.bind(this), keyConfigs: { enter: 'enter' }, eventName: 'keydown' });
    };
    SfTreemap.prototype.unWireEvents = function () {
        sf.base.EventHandler.remove(this.element, 'mouseup', this.mouseUp);
        sf.base.EventHandler.remove(this.element, 'mousemove', this.mouseMove);
        sf.base.EventHandler.remove(this.element, 'mousedown', this.mouseDown);
        sf.base.EventHandler.remove(this.element, 'mouseleave', this.mouseLeave);
        sf.base.EventHandler.remove(this.element, 'contextmenu', this.contextMenuEvent);
        window.removeEventListener('resize', this.resize.bind(this));
        sf.base.EventHandler.remove(this.element, "keydown", this.keyActionHandler);
        var keyboardModule = sf.base.getInstance(this.element, this.keyActionHandler);
        if (keyboardModule) {
            keyboardModule.destroy();
        }
        this.element = null;
        this.dotNetRef = null;
    };
    SfTreemap.prototype.keyActionHandler = function (event) {
        if (event.action === 'enter') {
            this.mouseDownProcess(event);
            this.mouseUpProcess(event);
        }
    };
    SfTreemap.prototype.contextMenuEvent = function () {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('TriggerRightClick');
        }
    };
    SfTreemap.prototype.resize = function () {
        var _this = this;
        var width;
        var height;
        if (this.element != null) {
            width = this.element.getBoundingClientRect().width;
            height = this.element.getBoundingClientRect().height;
        }
        if (this.dotNetRef) {
            if (this.resizeTo) {
                clearTimeout(this.resizeTo);
            }
            this.resizeTo = window.setTimeout(function () { _this.dotNetRef.invokeMethodAsync('TriggerResize', width, height); }, 500);
        }
    };
    SfTreemap.prototype.mouseDown = function (event) {
        this.mouseDownProcess(event);
    };
    SfTreemap.prototype.mouseDownProcess = function (event) {
        event.preventDefault();
        var contentText = this.getElementId(event.target.id);
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('TriggerMouseDown', event.target.id, contentText);
        }
    };
    SfTreemap.prototype.mouseUp = function (event) {
        this.mouseUpProcess(event);
    };
    SfTreemap.prototype.mouseUpProcess = function (event) {
        var contentText = this.getElementId(event.target.id);
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('TriggerMouseUp', event.target.id, contentText, event.which === 3);
        }
    };
    SfTreemap.prototype.mouseMove = function (event) {
        var mouseX;
        var mouseY;
        if (this.element != null) {
            var element = this.element.children[1];
            var elementRect = element.getBoundingClientRect();
            var pageXOffset_1 = element.ownerDocument.defaultView.pageXOffset;
            var pageYOffset_1 = element.ownerDocument.defaultView.pageYOffset;
            var clientTop = element.ownerDocument.documentElement.clientTop;
            var clientLeft = element.ownerDocument.documentElement.clientLeft;
            var positionX = elementRect.left + pageXOffset_1 - clientLeft;
            var positionY = elementRect.top + pageYOffset_1 - clientTop;
            mouseX = event.pageX - positionX;
            mouseY = event.pageY - positionY;
        }
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('TriggerMouseMove', event.target.id, mouseX, mouseY);
        }
    };
    SfTreemap.prototype.mouseLeave = function (event) {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('TriggerMouseLeave');
        }
    };
    SfTreemap.prototype.getElementId = function (id) {
        var contentText;
        if (!sf.base.isNullOrUndefined(id) && id !== '') {
            contentText = document.getElementById(id).textContent;
        }
        else {
            contentText = '';
        }
        return contentText;
    };
    return SfTreemap;
}());
// eslint-disable-next-line @typescript-eslint/no-explicit-any
var Treemap = {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    initialize: function (element, dotNetRef, height, width) {
        var layout = new SfTreemap(element, dotNetRef);
        layout.initializeEvents();
        return this.getElementSize(element, height, width);
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    getElementSize: function (element, height, width) {
        var elementWidth;
        var elementHeight;
        if (element != null) {
            element.style.height = height;
            element.style.width = width;
            var elementRect = element.getBoundingClientRect();
            elementWidth = elementRect.width;
            elementHeight = elementRect.height;
        }
        return { width: elementWidth, height: elementHeight, isIE: sf.base.Browser.isIE };
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    setElementAttribute: function (dotNetRef, legendItems, items, fill, opacity, borderColor, borderWidth, type, blazorElement) {
        for (var j = 0; j < items.length; j++) {
            var element = document.getElementById(items[j] + RECTPATH);
            if (element !== null) {
                var elementClass = element.getAttribute('class');
                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                var attribute = void 0;
                if (sf.base.isNullOrUndefined(elementClass)) {
                    attribute = document.createAttribute('class');
                    elementClass = '';
                }
                if (type === SELECTION && elementClass === TREEMAPHIGHLIGHT) {
                    element.setAttribute('class', '');
                    elementClass = '';
                }
                if (elementClass !== TREEMAPSELECTION) {
                    for (var i = 0; i < legendItems.length; i++) {
                        var legendElement = document.getElementById(legendItems[i]);
                        if (legendElement !== null) {
                            legendElement.setAttribute('fill', fill);
                            legendElement.setAttribute('opacity', opacity);
                            legendElement.setAttribute('stroke', borderColor);
                            legendElement.setAttribute('stroke-width', borderWidth);
                        }
                    }
                    element.setAttribute('fill', fill);
                    element.setAttribute('opacity', opacity);
                    element.setAttribute('stroke', borderColor);
                    element.setAttribute('stroke-width', borderWidth);
                    elementClass = (type === HIGHLIGHT || type === LEGENDHIGHLIGHT) ? TREEMAPHIGHLIGHT : TREEMAPSELECTION;
                    if (!sf.base.isNullOrUndefined(attribute) && sf.base.isNullOrUndefined(element.getAttribute('class'))) {
                        attribute.value = elementClass;
                        element.setAttributeNode(attribute);
                    }
                    else {
                        element.setAttribute('class', elementClass);
                    }
                    var contentText = blazorElement.blazor__instance.getElementId(items[j] + TEXT);
                    if (type === SELECTION) {
                        dotNetRef.invokeMethodAsync('TriggerItemSelect', contentText);
                    }
                    else if (type === HIGHLIGHT) {
                        dotNetRef.invokeMethodAsync('TriggerItemHighlight');
                    }
                }
            }
        }
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    removeElementAttribute: function (legendItems, legendFill, legendOpacity, legendBorderColor, legendBorderWidth, items, fill, opacity, borderColor, borderWidth, type) {
        for (var j = 0; j < items.length; j++) {
            var element = document.getElementById(items[j] + RECTPATH);
            if (element != null) {
                var elementClass = element.getAttribute('class');
                if (type === HIGHLIGHT && elementClass !== TREEMAPSELECTION ||
                    type === SELECTION && elementClass === TREEMAPSELECTION) {
                    for (var i = 0; i < legendItems.length; i++) {
                        var legendElement = document.getElementById(legendItems[i]);
                        if (legendElement != null) {
                            legendElement.setAttribute('fill', legendFill);
                            legendElement.setAttribute('opacity', legendOpacity);
                            legendElement.setAttribute('stroke', legendBorderColor);
                            legendElement.setAttribute('stroke-width', legendBorderWidth);
                        }
                    }
                    element.setAttribute('fill', fill[j]);
                    element.setAttribute('opacity', opacity[j]);
                    element.setAttribute('stroke', borderColor[j]);
                    element.setAttribute('stroke-width', borderWidth[j]);
                    element.setAttribute('class', '');
                }
            }
        }
    },
    templateElementSize: function (id, position) {
        var templateElement = document.getElementById(id);
        var width = templateElement.clientWidth;
        var height = templateElement.clientHeight;
        var textSizeWidth;
        var textSizeHeight;
        var styleProp = templateElement.getAttribute('style').split(';');
        var stylePropChanged;
        var stylePropJoin;
        for (var i = 0; i < styleProp.length; i++) {
            if (styleProp[i].indexOf('left') !== -1) {
                var itemLeftSplit = styleProp[i].split(':');
                var leftValue = parseFloat(itemLeftSplit[(itemLeftSplit.length - 1)]);
                textSizeWidth = position.indexOf('Left') !== -1 ? leftValue : position.indexOf('Right') === -1 ? leftValue - (width / 2) : leftValue - width;
                styleProp[i] = 'left:' + textSizeWidth + 'px';
            }
            else if (styleProp[i].indexOf('top') !== -1) {
                var itemTopSplit = styleProp[i].split(':');
                var topValue = parseFloat(itemTopSplit[(itemTopSplit.length - 1)]);
                textSizeHeight = position.indexOf('Top') !== -1 ? topValue : position.indexOf('Bottom') === -1 ?
                    (topValue) - (height / 2) : topValue - height;
                styleProp[i] = 'top:' + textSizeHeight + 'px';
            }
            stylePropJoin = styleProp[i] + ';';
            if (i === 0) {
                stylePropChanged = stylePropJoin;
            }
            else {
                stylePropChanged = stylePropChanged.concat(stylePropJoin);
            }
        }
        templateElement.setAttribute('style', stylePropChanged);
    },
    setTemplateTooltipLocation: function (element, mouseX, mouseY) {
        var tooltipElement = document.getElementById(element.id + '_Tooltip');
        var treemapBorder = document.getElementById(element.id + '_TreeMap_Border');
        if (tooltipElement && treemapBorder) {
            var x = Number(treemapBorder.getAttribute('x'));
            var y = Number(treemapBorder.getAttribute('y'));
            var width = Number(treemapBorder.getAttribute('width'));
            var tooltipRect = tooltipElement.getBoundingClientRect();
            var left = void 0;
            var top_1;
            if ((mouseY - tooltipRect.height) < y) {
                if ((mouseX + tooltipRect.width / 2) > width) {
                    left = (width - tooltipRect.width).toString() + 'px';
                }
                else if ((mouseX - tooltipRect.width / 2) < x) {
                    left = tooltipElement.style.left = x.toString() + 'px';
                }
                else {
                    left = (mouseX - (tooltipRect.width / 2)).toString() + 'px';
                }
                top_1 = (mouseY).toString() + 'px';
            }
            else if ((mouseX + tooltipRect.width / 2) > width) {
                left = (width - tooltipRect.width).toString() + 'px';
                top_1 = (mouseY - tooltipRect.height).toString() + 'px';
            }
            else if ((mouseX - tooltipRect.width / 2) < x) {
                left = x.toString() + 'px';
                top_1 = (mouseY - tooltipRect.height).toString() + 'px';
            }
            else {
                left = (mouseX - (tooltipRect.width / 2)).toString() + 'px';
                top_1 = (mouseY - tooltipRect.height).toString() + 'px';
            }
            tooltipElement.style.left = left ? left : '0';
            tooltipElement.style.top = top_1 ? top_1 : '0';
            tooltipElement.style.visibility = 'visible';
        }
    },
    destroy: function (element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.unWireEvents();
        }
    }
};

return Treemap;

}());

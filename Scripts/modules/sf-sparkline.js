window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Sparkline = (function () {
'use strict';

/* eslint-disable max-len */
var SfSparkline = /** @class */ (function () {
    function SfSparkline(element, dotNetRef) {
        this.resizeTo = 0;
        this.element = element;
        this.dotNetRef = dotNetRef;
        // eslint-disable-next-line camelcase
        this.element.blazor__instance = this;
    }
    SfSparkline.prototype.wireEvents = function () {
        sf.base.EventHandler.add(this.element, sf.base.Browser.touchMoveEvent, this.mousemove.bind(this), this);
        sf.base.EventHandler.add(this.element, sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave', this.mouseleave.bind(this), this);
        sf.base.EventHandler.add(this.element, 'click', this.click, this);
        window.addEventListener(sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize', this.resize.bind(this));
    };
    SfSparkline.prototype.unWireEvents = function () {
        sf.base.EventHandler.remove(this.element, sf.base.Browser.touchMoveEvent, this.mousemove);
        sf.base.EventHandler.remove(this.element, 'click', this.click);
        sf.base.EventHandler.remove(this.element, sf.base.Browser.isPointer ? 'pointerleave' : 'mouseleave', this.mouseleave);
        window.removeEventListener(sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize', this.resize.bind(this));
        this.element = null;
        this.dotNetRef = null;
    };
    SfSparkline.prototype.resize = function () {
        var _this = this;
        if (this.dotNetRef) {
            if (this.resizeTo) {
                clearTimeout(this.resizeTo);
            }
            this.resizeTo = window.setTimeout(function () { _this.dotNetRef.invokeMethodAsync('OnResize'); }, 500);
        }
    };
    SfSparkline.prototype.mouseleave = function () {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('OnMouseLeave');
        }
    };
    SfSparkline.prototype.click = function () {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('OnClick');
        }
    };
    SfSparkline.prototype.mousemove = function (e) {
        var rect = this.element.getBoundingClientRect();
        var svgElement = document.getElementById(this.element.id + '_svg');
        var svgRect;
        if (svgElement) {
            svgRect = svgElement.getBoundingClientRect();
        }
        var pY;
        var pX;
        var secondaryElement = document.getElementById(this.element.id + '_Secondary_Element');
        if (secondaryElement) {
            secondaryElement.style.left = Math.max((svgRect ? svgRect.left : 0) - (rect ? rect.left : 0), 0) + 'px';
            secondaryElement.style.top = Math.max((svgRect ? svgRect.top : 0) - (rect ? rect.top : 0), 0) + 'px';
        }
        if (e.type.indexOf('touch') > -1) {
            var touchArg = e;
            if (touchArg.changedTouches) {
                pX = touchArg.changedTouches[0].clientX;
                pY = touchArg.changedTouches[0].clientY;
            }
            else {
                pY = e.clientY;
                pX = e.clientX;
            }
        }
        else {
            pY = e.clientY;
            pX = e.clientX;
        }
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('OnMouseMove', pX, pY, rect ? rect.top : 0, rect ? rect.left : 0, svgRect ? svgRect.top : 0, svgRect ? svgRect.left : 0, e.target ? e.target.id : '', sf.base.Browser.isIE);
        }
    };
    return SfSparkline;
}());
// eslint-disable-next-line @typescript-eslint/no-explicit-any
var Sparkline = {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    initialize: function (element, dotNetRef, height, width) {
        if (element) {
            var layout = new SfSparkline(element, dotNetRef);
            layout.wireEvents();
            return this.getElementSize(element, height, width);
        }
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    getElementSize: function (element, height, width) {
        if (element) {
            element.style.height = height;
            element.style.width = width;
            var parentWidth = !element.parentElement ? 100 : element.parentElement.clientWidth || 0;
            var parentHeight = !element.parentElement ? 50 : element.parentElement.clientHeight || 0;
            return {
                width: element.clientWidth || element.offsetWidth,
                height: element.clientHeight || element.offsetHeight,
                parentWidth: parentWidth,
                parentHeight: parentHeight,
                isDevice: sf.base.Browser.isDevice,
                windowWidth: window.innerWidth,
                windowHeight: window.innerHeight
            };
        }
    },
    destroy: function (element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.unWireEvents();
        }
    }
};

return Sparkline;

}());

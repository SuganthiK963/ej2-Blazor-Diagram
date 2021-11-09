window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.BulletChart = (function () {
'use strict';

/* eslint-disable max-len */
var SfBulletChart = /** @class */ (function () {
    function SfBulletChart(element, dotNetRef) {
        this.resizeTo = 0;
        this.element = element;
        this.dotNetRef = dotNetRef;
        // eslint-disable-next-line camelcase
        this.element.blazor__instance = this;
    }
    SfBulletChart.prototype.wireEvents = function () {
        sf.base.EventHandler.add(this.element, 'click', this.click.bind(this), this);
        sf.base.EventHandler.add(this.element, 'mousemove', this.mousemove.bind(this), this);
        window.addEventListener((sf.base.Browser.isTouch && ('orientation' in window && 'onorientationchange' in window)) ? 'orientationchange' : 'resize', this.resize.bind(this));
        new sf.base.KeyboardEvents(this.element, { keyAction: this.keyActionHandler.bind(this), keyConfigs: { enter: 'enter' }, eventName: 'keydown' });
        sf.base.EventHandler.add(this.element, 'mousedown', this.mouseDown.bind(this), this);
    };
    SfBulletChart.prototype.unWireEvents = function () {
        sf.base.EventHandler.remove(this.element, 'mousemove', this.mousemove);
        sf.base.EventHandler.remove(this.element, 'click', this.click);
        window.removeEventListener(sf.base.Browser.isTouch && 'orientation' in window && 'onorientationchange' in window ? 'orientationchange' : 'resize', this.resize.bind(this));
        sf.base.EventHandler.remove(this.element, 'mousedown', this.mouseDown);
        var keyboardModule = sf.base.getInstance(this.element, this.keyActionHandler);
        if (keyboardModule) {
            keyboardModule.destroy();
        }
        this.element = null;
        this.dotNetRef = null;
    };
    SfBulletChart.prototype.mouseDown = function (event) {
        event.preventDefault();
    };
    SfBulletChart.prototype.keyActionHandler = function (event) {
        if (event.action === 'enter') {
            this.clickProcess(event);
        }
    };
    SfBulletChart.prototype.resize = function () {
        var _this = this;
        if (this.dotNetRef) {
            if (this.resizeTo) {
                clearTimeout(this.resizeTo);
            }
            this.resizeTo = window.setTimeout(function () { _this.dotNetRef.invokeMethodAsync('TriggerResize'); }, 500);
        }
    };
    SfBulletChart.prototype.click = function (event) {
        this.clickProcess(event);
    };
    SfBulletChart.prototype.clickProcess = function (event) {
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('TriggerClick', event.target.id);
        }
    };
    SfBulletChart.prototype.mousemove = function (event) {
        var elementRect;
        var svgRect;
        if (this.element) {
            elementRect = this.element.getBoundingClientRect();
        }
        var svgElement = document.getElementById(this.element.id + '_svg');
        if (svgElement) {
            svgRect = svgElement.getBoundingClientRect();
        }
        var x = event.clientX - elementRect.left - Math.max(svgRect.left - elementRect.left, 0);
        var y = event.clientY - elementRect.top - Math.max(svgRect.top - elementRect.top, 0);
        if (this.dotNetRef) {
            this.dotNetRef.invokeMethodAsync('TriggerMouseMove', event.target.id, x, y);
        }
    };
    SfBulletChart.prototype.valueBarAnimation = function (data, barElement, valueX) {
        var animation = new sf.base.Animation({});
        animation.animate((barElement), {
            duration: data.duration,
            delay: data.delay,
            progress: function (args) {
                if (args.timeStamp >= args.delay) {
                    barElement.style.visibility = 'visible';
                    var effValue = -1 * Math.cos((args.timeStamp - args.delay) / args.duration * (Math.PI / 2)) + 1;
                    barElement.setAttribute('transform', 'translate(' + valueX.toString() + ' ' + data.valueY.toString() + ') scale(' + (effValue / 1).toString() + ', 1) translate(' + -(valueX).toString() + ' ' + -(data.valueY).toString() + ')');
                }
            },
            end: function () {
                barElement.setAttribute('transform', 'translate(0,0)');
                barElement.style.visibility = 'visible';
            }
        });
    };
    SfBulletChart.prototype.targetBarAnimation = function (data, barElement, valueX, valueY) {
        var animation = new sf.base.Animation({});
        animation.animate((barElement), {
            duration: data.duration,
            delay: data.delay,
            progress: function (args) {
                if (args.timeStamp >= args.delay) {
                    barElement.style.visibility = 'visible';
                    var effValue = -1 * Math.cos((args.timeStamp - args.delay) / args.duration * (Math.PI / 2)) + 1;
                    barElement.setAttribute('transform', 'translate(' + valueX.toString() + ' ' + (valueY).toString() + ') scale(1, ' + (effValue / 1).toString() + ') translate(' + -(valueX).toString() + ' ' + -(valueY).toString() + ')');
                }
            },
            end: function () {
                barElement.setAttribute('transform', 'translate(0,0)');
                barElement.style.visibility = 'visible';
            }
        });
    };
    return SfBulletChart;
}());
// eslint-disable-next-line @typescript-eslint/no-explicit-any
var BulletChart = {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    initialize: function (element, height, width, dotNetRef) {
        if (element) {
            var layout = new SfBulletChart(element, dotNetRef);
            layout.wireEvents();
            return this.getElementSize(element, height, width);
        }
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    getElementSize: function (element, height, width) {
        var elementWidth;
        var elementHeight;
        if (element) {
            element.style.height = height;
            element.style.width = width;
            elementWidth = element.clientWidth || element.offsetWidth;
            elementHeight = element.clientHeight;
        }
        return { width: elementWidth, height: elementHeight };
    },
    updateElementOpacity: function (elementId, remove, prevId) {
        var element = document.getElementById(elementId);
        if (element) {
            if (!sf.base.isNullOrUndefined(prevId) && prevId !== '') {
                var prevElement = document.getElementById(prevId);
                prevElement.setAttribute('opacity', '1');
            }
            if (remove === true) {
                element.setAttribute('opacity', '1');
            }
            else {
                element.setAttribute('opacity', '0.6');
            }
        }
    },
    doValueBarAnimation: function (element, data, id, valueX) {
        for (var i = 0; i < id.length; i++) {
            var barElement = document.getElementById(id[i]);
            if (barElement) {
                element.blazor__instance.valueBarAnimation(data, barElement, valueX[i]);
            }
        }
    },
    doTargetBarAnimation: function (element, data, id, valueX, valueY) {
        for (var i = 0; i < id.length; i++) {
            var barElement = document.getElementById(id[i]);
            if (barElement) {
                element.blazor__instance.targetBarAnimation(data, barElement, valueX[i], valueY[i]);
            }
        }
    },
    destroy: function (element) {
        if (element && element.blazor__instance) {
            element.blazor__instance.unWireEvents();
        }
    }
};

return BulletChart;

}());

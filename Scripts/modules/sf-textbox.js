window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.TextBox = (function () {
'use strict';

/**
 * Blazor texbox interop handler
 */
var BLUR = 'blur';
var MOUSE_DOWN = 'mousedown';
var MOUSE_UP = 'mouseup';
var INPUT_GROUP = 'e-input-group-icon';
var DISABLED = 'e-disabled';
var RIPPLE_BTN = 'e-input-btn-ripple';
var SfTextBox = /** @class */ (function () {
    function SfTextBox(element, dotnetRef, containerEle) {
        this.element = element;
        this.container = containerEle;
        this.element.blazor_input_instance = this;
        this.dotNetRef = dotnetRef;
        this.isDestroyed = false;
    }
    SfTextBox.prototype.initialize = function () {
        sf.base.EventHandler.add(this.element, BLUR, this.blurHandler, this);
        var buttons = this.container ? this.container.querySelectorAll('.' + INPUT_GROUP) : null;
        if (buttons && buttons.length > 0) {
            for (var index = 0; index < buttons.length; index++) {
                sf.base.EventHandler.add(buttons[index], MOUSE_DOWN, this.onMouseDownRipple, this);
                sf.base.EventHandler.add(buttons[index], MOUSE_UP, this.onMouseUpRipple, this);
            }
        }
    };
    SfTextBox.prototype.onMouseDownRipple = function (args) {
        var button = args ? args.currentTarget : null;
        if (button && !this.container.classList.contains(DISABLED) && !this.container.querySelector('input').readOnly) {
            button.classList.add(RIPPLE_BTN);
        }
    };
    SfTextBox.prototype.onMouseUpRipple = function (args) {
        var button = args ? args.currentTarget : null;
        if (button) {
            setTimeout(function () {
                button.classList.remove(RIPPLE_BTN);
            }, 500);
        }
    };
    SfTextBox.prototype.blurHandler = function () {
        if (!this.isDestroyed) {
            // tslint:disable
            this.dotNetRef.invokeMethodAsync('BlurHandler').catch(function () { });
        }
        else {
            sf.base.EventHandler.remove(this.element, BLUR, this.blurHandler);
        }
    };
    SfTextBox.prototype.destroy = function () {
        sf.base.EventHandler.remove(this.element, BLUR, this.blurHandler);
    };
    return SfTextBox;
}());
// tslint:disable
var TextBox = {
    initialize: function (element, dotnetRef, containerEle) {
        if (element) {
            new SfTextBox(element, dotnetRef, containerEle);
        }
        if (element && element.blazor_input_instance) {
            element.blazor_input_instance.initialize();
        }
    },
    focusIn: function (element) {
        element.focus();
    },
    focusOut: function (element) {
        element.blur();
    },
    destroyInput: function (element) {
        if (element && element.blazor_input_instance) {
            element.blazor_input_instance.isDestroyed = true;
            element.blazor_input_instance.destroy();
        }
    }
};

return TextBox;

}());

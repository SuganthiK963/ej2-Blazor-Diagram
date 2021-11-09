window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Spinner = (function () {
'use strict';

var SfSpinner = /** @class */ (function () {
    function SfSpinner(element) {
        this.element = element;
        this.element.blazor__instance = this;
    }
    SfSpinner.prototype.initialize = function (element) {
        var theme = window.getComputedStyle(element, ':after').getPropertyValue('content');
        return theme.replace(/['"]+/g, '');
    };
    return SfSpinner;
}());
// eslint-disable-next-line
var Spinner = {
    initialize: function (element) {
        if (!sf.base.isNullOrUndefined(element)) {
            new SfSpinner(element);
            return (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance)) ?
                element.blazor__instance.initialize(element) : null;
        }
        else {
            return null;
        }
    }
};

return Spinner;

}());

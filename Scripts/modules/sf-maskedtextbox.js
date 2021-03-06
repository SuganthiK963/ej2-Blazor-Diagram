window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.MaskedTextBox = (function () {
'use strict';

/**
 * Blazor masked textbox interop handler
 */
var PASTE_HANDLER = 'UpdatePasteValue';
var KEY_DOWN = 'keydown';
var FOCUS = 'focus';
var BLUR = 'blur';
var PASTE = 'paste';
var CUT = 'cut';
var MOUSE_DOWN = 'mousedown';
var MOUSE_UP = 'mouseup';
var INPUT = 'input';
var SELECT = 'select';
var CLEAR_ICON = 'e-clear-icon';
var CLEAR_ICON_HIDE = 'e-clear-icon-hide';
var SfMaskedTextBox = /** @class */ (function () {
    function SfMaskedTextBox(wrapperElement, element, dotnetRef, options) {
        this.ismultipledelete = false;
        this.isClicked = false;
        this.previousVal = '';
        this.inputData = null;
        this.isFocusInput = false;
        this.wrapperElement = wrapperElement;
        this.isClicked = false;
        this.sIndex = 0;
        this.eIndex = 0;
        this.element = element;
        this.options = options;
        this.element.blazor__instance = this;
        this.dotNetRef = dotnetRef;
        this.previousVal = this.options.maskedValue;
    }
    SfMaskedTextBox.prototype.initialize = function () {
        sf.base.EventHandler.add(this.element, FOCUS, this.focusHandler, this);
        sf.base.EventHandler.add(this.element, BLUR, this.focusOutHandler, this);
        sf.base.EventHandler.add(this.element, PASTE, this.pasteHandler, this);
        sf.base.EventHandler.add(this.element, CUT, this.cutHandler, this);
        sf.base.EventHandler.add(this.element, MOUSE_DOWN, this.mouseDownHandler, this);
        sf.base.EventHandler.add(this.element, MOUSE_UP, this.mouseUpHandler, this);
        sf.base.EventHandler.add(this.element, KEY_DOWN, this.keyDownHandler, this);
        sf.base.EventHandler.add(this.element, INPUT, this.inputHandler, this);
        sf.base.EventHandler.add(this.element, SELECT, this.selectionHandler, this);
    };
    SfMaskedTextBox.prototype.focusOutHandler = function (event) {
        var clearIconEle = this.wrapperElement.querySelector('.' + CLEAR_ICON);
        if (clearIconEle !== null) {
            clearIconEle.classList.add(CLEAR_ICON_HIDE);
        }
    };
    SfMaskedTextBox.prototype.mouseDownHandler = function (event) {
        this.isClicked = true;
    };
    SfMaskedTextBox.prototype.mouseUpHandler = function (event) {
        this.isClicked = false;
        if (sf.base.Browser.isDevice) {
            this.ismultipledelete = false;
            this.sIndex = this.eIndex = 0;
        }
    };
    SfMaskedTextBox.prototype.selectionHandler = function (event) {
        var inputElement = this.element;
        if (inputElement.selectionStart !== inputElement.selectionEnd && sf.base.Browser.isDevice) {
            this.sIndex = inputElement.selectionStart;
            this.eIndex = inputElement.selectionEnd;
        }
    };
    SfMaskedTextBox.prototype.keyDownHandler = function (event) {
        var inputElement = this.element;
        this.previousVal = inputElement.value;
        if (!sf.base.Browser.isDevice) {
            if (inputElement.selectionStart !== inputElement.selectionEnd) {
                this.sIndex = inputElement.selectionStart;
                this.eIndex = inputElement.selectionEnd;
            }
            else {
                this.ismultipledelete = false;
                this.sIndex = this.eIndex = 0;
            }
        }
    };
    // tslint:disable-next-line:no-any
    SfMaskedTextBox.prototype.inputHandler = function (event) {
        if (this.options.mask) {
            var inputElement = this.element;
            if (event.data || event.inputType) {
                var inputVal = event.data ? event.data : event.inputType === 'deleteContentBackward' ?
                    'Backspace' : event.inputType === 'deleteContentForward' ? 'Delete' : null;
                var startIndex = inputElement.selectionStart;
                var endIndex = inputElement.selectionEnd;
                if (this.sIndex !== this.eIndex) {
                    this.ismultipledelete = true;
                    startIndex = this.sIndex;
                    endIndex = this.eIndex;
                    this.sIndex = this.eIndex = 0;
                }
                else {
                    startIndex = event.data ? startIndex - 1 : event.inputType === 'deleteContentBackward' ?
                        startIndex + 1 : startIndex;
                    endIndex = startIndex;
                }
                inputElement.value = this.previousVal;
                inputElement.selectionStart = inputElement.selectionEnd = startIndex;
                if (!this.ismultipledelete) {
                    this.updateMask(inputVal, startIndex);
                }
                else {
                    this.ismultipledelete = false;
                    var inputValue = this.multipleDeletion(this.previousVal, startIndex, endIndex);
                    inputElement.value = inputValue;
                    this.previousVal = inputElement.value;
                    inputElement.selectionStart = inputElement.selectionEnd = startIndex;
                    if (inputVal !== null && inputVal !== 'Backspace' && inputVal !== 'Delete') {
                        this.updateMask(inputVal, startIndex);
                    }
                }
                this.previousVal = inputElement.value;
                this.updateServerValue();
            }
            else {
                this.inputData = inputElement.value;
                inputElement.value = this.previousVal;
                inputElement.selectionStart = inputElement.selectionEnd = inputElement.selectionStart;
                this.pasteHandler(event);
            }
        }
    };
    SfMaskedTextBox.prototype.multipleDeletion = function (inputVal, startIndex, endIndex) {
        var elementVal = inputVal;
        for (var i = startIndex; i < endIndex; i++) {
            elementVal = elementVal.substring(0, i) + this.options.promptMask[i] + elementVal.substring(i + 1);
        }
        return elementVal;
    };
    SfMaskedTextBox.prototype.updateServerValue = function () {
        var _this = this;
        var inputElement = this.element;
        setTimeout(function () { _this.dotNetRef.invokeMethodAsync('UpdateInputValue', inputElement.value, false); }, 20);
    };
    SfMaskedTextBox.prototype.updateMask = function (inputChar, startIndex) {
        var inputElement = this.element;
        if (!this.ismultipledelete) {
            var elementValue = this.previousVal;
            var regx = '';
            if (startIndex >= 0 && startIndex < this.options.customRegExpCollec.length) {
                regx = this.options.customRegExpCollec[startIndex].length === 1 ?
                    this.getRegex(this.options.customRegExpCollec[startIndex]) : this.options.customRegExpCollec[startIndex];
            }
            var checkRegex = new RegExp(regx);
            if (inputChar === 'Backspace' || inputChar === 'Delete') {
                if (inputChar === 'Backspace') {
                    if (this.options.promptMask[startIndex - 1] === this.options.promptCharacter) {
                        inputElement.value = elementValue.substring(0, startIndex - 1) + this.options.promptCharacter
                            + elementValue.substring(startIndex);
                    }
                    else {
                        inputElement.value = elementValue.substring(0, startIndex - 1) + this.options.promptMask[startIndex - 1]
                            + elementValue.substring(startIndex);
                    }
                    inputElement.selectionStart = inputElement.selectionEnd = startIndex - 1;
                }
                else {
                    if (this.options.promptMask[startIndex] === this.options.promptCharacter) {
                        inputElement.value = elementValue.substring(0, startIndex) + this.options.promptCharacter +
                            elementValue.substring(startIndex + 1);
                    }
                    else {
                        inputElement.value = elementValue.substring(0, startIndex) + this.options.promptMask[startIndex] +
                            elementValue.substring(startIndex + 1);
                    }
                    inputElement.selectionStart = inputElement.selectionEnd = startIndex;
                }
            }
            else {
                if (startIndex < this.options.promptMask.length) {
                    if (this.options.promptMask[startIndex] === this.options.promptCharacter) {
                        if (checkRegex.test(inputChar)) {
                            var modifiedinputchar = this.getLetterCase(inputChar, startIndex);
                            inputElement.value = elementValue.substring(0, startIndex) + modifiedinputchar +
                                elementValue.substring(startIndex + 1);
                            inputElement.selectionStart = inputElement.selectionEnd = startIndex + 1;
                        }
                    }
                    else {
                        inputElement.selectionStart = inputElement.selectionEnd = startIndex + 1;
                        this.updateMask(inputChar, inputElement.selectionStart);
                    }
                }
            }
        }
    };
    SfMaskedTextBox.prototype.getRegex = function (maskChar) {
        var regularExpression = '';
        switch (maskChar) {
            case '0':
                regularExpression = '\\d';
                break;
            case '9':
                regularExpression = '[0-9 ]';
                break;
            case 'L':
                regularExpression = '[A-Za-z]';
                break;
            case 'A':
                regularExpression = '[A-Za-z0-9]';
                break;
            case 'a':
                regularExpression = '[A-Za-z0-9 ]';
                break;
            case 'C':
                regularExpression = '[^\x7f]+';
                break;
            case '#':
                regularExpression = '[0-9 +-]';
                break;
            case '?':
                regularExpression = '[A-Za-z ]';
                break;
            case '&':
                regularExpression = '[^\x7f ]+';
                break;
            default:
                if (maskChar === '(' || maskChar === '+' || maskChar === ')') {
                    regularExpression = '\\' + maskChar;
                }
                else {
                    regularExpression = maskChar;
                }
                break;
        }
        return regularExpression;
    };
    SfMaskedTextBox.prototype.getLetterCase = function (inputChar, startIndex) {
        var isLower = false;
        var isUpper = false;
        var isIndependent = false;
        var isCasing = false;
        var hiddenMaskChar = this.options.hiddenMask;
        var lowerArray = [];
        var upperArray = [];
        for (var i = 0; i < hiddenMaskChar.length; i++) {
            if ((hiddenMaskChar[i] === '<' || isLower) && (hiddenMaskChar[i] !== '>') && (hiddenMaskChar[i] !== '|')) {
                isLower = isCasing = true;
                isUpper = isIndependent = false;
                lowerArray[i] = true;
                upperArray[i] = false;
                if (hiddenMaskChar[i] === '<')
                    hiddenMaskChar = hiddenMaskChar.replace(hiddenMaskChar[i], '');
            }
            else if ((hiddenMaskChar[i] === '>' || isUpper) && (hiddenMaskChar[i] !== '<') && (hiddenMaskChar[i] !== '|')) {
                isUpper = isCasing = true;
                isLower = isIndependent = false;
                lowerArray[i] = false;
                upperArray[i] = true;
                if (hiddenMaskChar[i] === '>')
                    hiddenMaskChar = hiddenMaskChar.replace(hiddenMaskChar[i], '');
            }
            else if ((hiddenMaskChar[i] === '|' || isIndependent) && (hiddenMaskChar[i] !== '<') && (hiddenMaskChar[i] !== '>')) {
                isIndependent = isCasing = true;
                isUpper = isLower = false;
                lowerArray[i] = false;
                upperArray[i] = false;
                if (hiddenMaskChar[i] === '|')
                    hiddenMaskChar = hiddenMaskChar.replace(hiddenMaskChar[i], '');
            }
            else if (!isCasing) {
                lowerArray[i] = false;
                upperArray[i] = false;
            }
        }
        var inputCharacter = upperArray[startIndex] ? inputChar.toUpperCase() : lowerArray[startIndex] ? inputChar.toLowerCase() : inputChar;
        return inputCharacter;
    };
    SfMaskedTextBox.prototype.focusHandler = function (event) {
        var _this = this;
        var inputElement = this.element;
        var startIndex = 0;
        var modelValue = this.stripValue(inputElement.value);
        var toAllowForward = false;
        var toAllowBackward = false;
        if (this.options.mask !== null) {
            if (!(!(modelValue === null || modelValue === '') || this.options.floatLabelType === 'Always' ||
                this.options.placeHolder === null || this.options.placeHolder === '')) {
                inputElement.value = this.options.maskedValue;
            }
            setTimeout(function () {
                if (inputElement.selectionStart === _this.options.mask.length ||
                    inputElement.value[inputElement.selectionStart] === _this.options.promptCharacter) {
                    toAllowForward = true;
                }
                else {
                    for (var i = inputElement.selectionStart; i < _this.options.mask.length; i++) {
                        if (inputElement.value[i] !== _this.options.promptCharacter) {
                            if ((inputElement.value[i] !== _this.options.mask[i])) {
                                toAllowForward = false;
                                break;
                            }
                        }
                        else {
                            toAllowForward = true;
                            break;
                        }
                    }
                }
            });
            setTimeout(function () {
                var backSelectionStart = inputElement.selectionStart - 1;
                if (backSelectionStart === _this.options.mask.length - 1 ||
                    inputElement.value[backSelectionStart] === _this.options.promptCharacter) {
                    toAllowBackward = true;
                }
                else {
                    for (var i = backSelectionStart; i >= 0; i--) {
                        if (inputElement.value[i] !== _this.options.promptCharacter) {
                            if ((inputElement.value[i] !== _this.options.mask[i])) {
                                toAllowBackward = false;
                                break;
                            }
                        }
                        else {
                            toAllowBackward = true;
                            break;
                        }
                    }
                }
            });
            if (((this.isClicked || this.isFocusInput) || (this.options.floatLabelType !== 'Always' &&
                ((modelValue === null || modelValue === '') &&
                    (this.options.placeHolder !== null && this.options.placeHolder !== ''))))) {
                this.isFocusInput = true;
                for (startIndex = 0; startIndex < this.options.mask.length; startIndex++) {
                    if (inputElement.value[startIndex] === this.options.promptCharacter) {
                        setTimeout(function () {
                            if (toAllowForward || toAllowBackward) {
                                inputElement.selectionEnd = startIndex;
                                inputElement.selectionStart = startIndex;
                            }
                        });
                        break;
                    }
                }
                this.isClicked = false;
            }
        }
    };
    SfMaskedTextBox.prototype.stripValue = function (inputEleValue) {
        var stripVal = '';
        if (this.options.mask !== null && inputEleValue != null && inputEleValue !== '') {
            for (var i = 0; i < this.options.mask.length; i++) {
                if (this.options.mask[i] !== inputEleValue[i]) {
                    stripVal += inputEleValue[i];
                }
            }
        }
        return stripVal;
    };
    SfMaskedTextBox.prototype.pasteHandler = function (event) {
        var _this = this;
        if (this.options.mask !== null && !this.options.readonly && this.options.enabled) {
            var inputElement_1 = this.element;
            var pasteValue = void 0;
            if (event.clipboardData !== undefined && !this.inputData) {
                pasteValue = event.clipboardData.getData('text/plain');
            }
            else {
                pasteValue = this.inputData;
                inputElement_1.selectionStart = inputElement_1.selectionEnd = 0;
                this.inputData = null;
            }
            if (pasteValue !== null) {
                var eventArgs = {
                    Readonly: false,
                    Enabled: true,
                    Value: inputElement_1.value,
                    selectionEnd: inputElement_1.selectionEnd,
                    selectionStart: inputElement_1.selectionStart,
                    IsMultipleDelete: this.ismultipledelete,
                    PasteValue: pasteValue
                };
                event.preventDefault();
                // @ts-ignore-start
                // tslint:disable-next-line:no-any
                this.dotNetRef.invokeMethodAsync(PASTE_HANDLER, eventArgs).then(function (args) {
                    // @ts-ignore-end
                    inputElement_1.value = args.inputValue;
                    inputElement_1.selectionStart = inputElement_1.selectionEnd = args.cursorPosition;
                    _this.previousVal = inputElement_1.value;
                });
            }
        }
    };
    SfMaskedTextBox.prototype.cutHandler = function (event) {
        if (this.options.mask) {
            var inputElement = this.element;
            var startIndex = inputElement.selectionStart;
            var endIndex = inputElement.selectionEnd;
            if (startIndex !== endIndex && this.options.mask !== null) {
                this.ismultipledelete = true;
                var selectedText = inputElement.value.substring(startIndex, endIndex);
                event.clipboardData.setData('text', selectedText);
                // @ts-ignore-start
                window.clipboardData.setData('text', selectedText);
                // @ts-ignore-end
                inputElement.value = this.multipleDeletion(inputElement.value, startIndex, endIndex);
                inputElement.selectionStart = inputElement.selectionEnd = startIndex;
                this.previousVal = inputElement.value;
                this.updateServerValue();
            }
        }
    };
    SfMaskedTextBox.prototype.propertyChange = function (options) {
        this.options = options;
        this.previousVal = this.options.maskedValue;
    };
    return SfMaskedTextBox;
}());
// tslint:disable
var MaskedTextBox = {
    initialize: function initialize(wrapperElement, element, dotnetRef, options) {
        if (element) {
            new SfMaskedTextBox(wrapperElement, element, dotnetRef, options);
        }
        if (element && element.blazor__instance) {
            element.blazor__instance.initialize();
        }
    },
    propertyChange: function propertyChange(inputEle, options) {
        if (inputEle && inputEle.blazor__instance) {
            inputEle.blazor__instance.propertyChange(options);
        }
    },
    focusIn: function focusIn(inputEle) {
        if (inputEle) {
            inputEle.blazor__instance.isFocusInput = true;
            inputEle.focus();
        }
    },
    focusOut: function focusOut(inputEle) {
        if (inputEle) {
            inputEle.blur();
        }
    }
};

return MaskedTextBox;

}());

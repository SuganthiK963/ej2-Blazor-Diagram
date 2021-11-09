window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.ColorPicker = (function () {
'use strict';

var HSV_CONTAINER = '.e-hsv-container';
var COLOR_PICKER = '.e-split-colorpicker';
var COLOR_PALETTE = 'e-color-palette';
var PALETTE = '.e-palette';
var HSV_COLOR = '.e-hsv-color';
var SET_OFFSET = 'SetOffset';
var DROPDOWN_BTN = '.e-dropdown-btn';
var HANDLER = '.e-handler';
var MODEL = '.e-colorpicker.e-modal';
var EMPTY = '';
var CONTAINER = '.e-container';
var SPLIT_BUTTON = '.sf-colorpicker';
var INPUTS = '.e-selected-value';
var CONTROL_BTN = '.e-switch-ctrl-btn';
var PIXEL = 'px';
var SCROLL = 'scroll';
var DEFAULT = '100%';
var PALETTES = 'e-palette';
var TILE = "e-tile";
var SELECT = 'e-selected';
var CLICK = 'Click';
var CLOSE = 'Close';
/**
 * Client side scripts for Blazor color picker
 */
var SfColorPicker = /** @class */ (function () {
    function SfColorPicker(element, dotnetRef, inline) {
        this.element = element;
        this.dotnetRef = dotnetRef;
        this.element.blazor__instance = this;
        if (inline) {
            this.addScrollEvents(true);
            this.setPaletteWidth(this.element.querySelector(CONTAINER), false);
        }
    }
    SfColorPicker.prototype.getOffset = function (element) {
        var colorPicker = element.querySelector(HSV_CONTAINER);
        if (!colorPicker) {
            return { IsDevice: sf.base.Browser.isDevice };
        }
        var offset = colorPicker.getBoundingClientRect();
        var color = colorPicker.querySelector(HSV_COLOR);
        var handler = colorPicker.querySelector(HANDLER);
        if (handler) {
            handler.focus();
        }
        return { Left: offset.left, Top: offset.top, Width: offset.width, Height: offset.height, Right: offset.right, ClientLeft: color.offsetLeft, ClientTop: color.offsetTop, ClientWidth: color.offsetWidth, ClientHeight: color.offsetHeight,
            IsDevice: sf.base.Browser.isDevice };
    };
    SfColorPicker.prototype.setOffset = function (element, zIndex) {
        var offset = this.getOffset(element);
        if (zIndex && !sf.base.Browser.isDevice) {
            this.setZIndex(this.element, zIndex, COLOR_PICKER);
            this.setZIndex(this.element, zIndex, DROPDOWN_BTN);
        }
        if (offset) {
            this.dotnetRef.invokeMethodAsync(SET_OFFSET, offset);
        }
    };
    SfColorPicker.prototype.setZIndex = function (element, zIndex, cls) {
        var btnEle = element.querySelector(cls);
        if (btnEle) {
            btnEle.style.zIndex = zIndex + 1 + EMPTY;
        }
    };
    SfColorPicker.prototype.setPaletteWidth = function (container, modeSwitch, zIndex) {
        if (sf.base.Browser.isDevice && !sf.base.isNullOrUndefined(zIndex)) {
            var model = container.querySelector(MODEL);
            if (model) {
                model.style.zIndex = (zIndex - 1) + EMPTY;
                document.body.insertBefore(model, container.parentElement);
                model.style.display = EMPTY;
            }
        }
        if (container.querySelector(HSV_CONTAINER)) {
            container.style.width = EMPTY;
        }
        else {
            var width = parseInt(getComputedStyle(container).borderBottomWidth, 10);
            container.style.width = container.children[0].offsetWidth + width + width + PIXEL;
            var containers = container.querySelector(INPUTS);
            if (containers) {
                containers.style.width = DEFAULT;
                containers = container.querySelector(CONTROL_BTN);
                if (containers) {
                    containers.style.width = DEFAULT;
                }
            }
        }
        if (modeSwitch) {
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            var colorPickerBtn = this.element.querySelector(SPLIT_BUTTON);
            if (colorPickerBtn && !sf.base.isNullOrUndefined(colorPickerBtn.blazor__instance)) {
                colorPickerBtn.blazor__instance.setPosition(true);
            }
        }
    };
    SfColorPicker.prototype.scrollHandler = function (e) {
        if (!this.element.parentElement) {
            sf.base.EventHandler.remove(e.target, SCROLL, this.scrollHandler);
            return;
        }
        this.setOffset(this.element);
    };
    SfColorPicker.prototype.addScrollEvents = function (add) {
        var elements = sf.popups.getScrollableParent(this.element);
        for (var _i = 0, elements_1 = elements; _i < elements_1.length; _i++) {
            var element = elements_1[_i];
            add ? sf.base.EventHandler.add(element, SCROLL, this.scrollHandler, this) :
                sf.base.EventHandler.remove(element, SCROLL, this.scrollHandler);
        }
    };
    SfColorPicker.prototype.focusIn = function (element, inline) {
        if (inline) {
            var container = element.querySelector(CONTAINER);
            if (container) {
                if (container.classList.contains(COLOR_PALETTE)) {
                    this.setFocus(PALETTE);
                }
                else {
                    this.setFocus(HANDLER);
                }
            }
        }
        else {
            this.setFocus(COLOR_PICKER);
        }
    };
    SfColorPicker.prototype.setFocus = function (cls) {
        var btn = this.element.querySelector(cls);
        if (btn) {
            btn.focus();
        }
    };
    SfColorPicker.prototype.paletteKeyDown = function (e, Rtl) {
        var target = e.target;
        if (!target.classList.contains(PALETTES)) {
            return;
        }
        var selectedEle;
        var idx;
        var colorvalue;
        var tiles = [].slice.call(sf.base.selectAll('.' + TILE, target));
        var prevSelectedEle = (tiles.filter(function (tile) { return tile.classList.contains('e-selected'); })).pop();
        var container = target.parentElement;
        switch (!e.altKey && e.keyCode) {
            case 39:
                e.preventDefault();
                selectedEle = prevSelectedEle ? tiles[this.tilePosition(tiles, prevSelectedEle, Rtl ? -1 : 1)]
                    : tiles[Rtl ? tiles.length - 1 : 0];
                colorvalue = selectedEle.getAttribute("aria-label");
                this.keySelectionChanges(selectedEle, container, colorvalue);
                break;
            case 37:
                e.preventDefault();
                selectedEle = prevSelectedEle ? tiles[this.tilePosition(tiles, prevSelectedEle, Rtl ? 1 : -1)]
                    : tiles[Rtl ? 0 : tiles.length - 1];
                colorvalue = selectedEle.getAttribute("aria-label");
                this.keySelectionChanges(selectedEle, container, colorvalue);
                break;
            case 38:
                e.preventDefault();
                idx = prevSelectedEle ? this.tilePosition(tiles, prevSelectedEle, -10) : 0;
                selectedEle = tiles[idx] ? tiles[idx] : tiles[idx - 10];
                colorvalue = selectedEle.getAttribute("aria-label");
                this.keySelectionChanges(selectedEle, container, colorvalue);
                break;
            case 40:
                e.preventDefault();
                idx = prevSelectedEle ? this.tilePosition(tiles, prevSelectedEle, 10) : tiles.length - 1;
                if (tiles[idx]) {
                    selectedEle = tiles[idx];
                }
                else {
                    idx %= tiles.length;
                    idx += tiles[tiles.length - 1].parentElement.childElementCount;
                    selectedEle = tiles[idx];
                }
                colorvalue = selectedEle.getAttribute("aria-label");
                this.keySelectionChanges(selectedEle, container, colorvalue);
                break;
            case 13:
                e.preventDefault();
                if (prevSelectedEle) {
                    var cValue = prevSelectedEle.getAttribute('aria-label');
                    this.dotnetRef.invokeMethodAsync(CLOSE);
                }
        }
    };
    
    SfColorPicker.prototype.tilePosition = function (items, element, cIdx) {
        items = Array.prototype.slice.call(items);
        var n = items.length;
        var emptyCount = 10 - items[n - 1].parentElement.childElementCount;
        var idx = items.indexOf(element);
        idx += cIdx;
        if (idx < 0) {
            idx += n + emptyCount;
        }
        else {
            idx %= n + emptyCount;
        }
        return idx;
    };
    
    SfColorPicker.prototype.keySelectionChanges = function (newEle, container, colorvalue) {
        this.removeTileSelection(container);
        this.addTileSelection(newEle);
        this.dotnetRef.invokeMethodAsync(CLICK, colorvalue);
    };
    
    SfColorPicker.prototype.removeTileSelection = function (container) {
        var selectedEle = [].slice.call(sf.base.selectAll('.' + SELECT, container.children[0]));
        selectedEle.forEach(function (ele) {
            ele.classList.remove(SELECT);
            ele.setAttribute('aria-selected', 'false');
        });
    };
    
    SfColorPicker.prototype.addTileSelection = function (ele) {
        ele.classList.add(SELECT);
        ele.setAttribute('aria-selected', 'true');
    };
    
    return SfColorPicker;
}());
// eslint-disable-next-line @typescript-eslint/naming-convention, no-underscore-dangle, id-blacklist, id-match
var ColorPicker = {
    initialize: function (element, dotnetRef, inline) {
        if (!sf.base.isNullOrUndefined(element)) {
            new SfColorPicker(element, dotnetRef, inline);
            return element.blazor__instance.getOffset(element);
        }
        return null;
    },
    getOffset: function (element, container) {
        if (!sf.base.isNullOrUndefined(element) && !sf.base.isNullOrUndefined(element.blazor__instance) && !sf.base.isNullOrUndefined(container)) {
            element.blazor__instance.setPaletteWidth(container, true);
            return element.blazor__instance.getOffset(container);
        }
        return null;
    },
    focusIn: function (element, inline) {
        if (!sf.base.isNullOrUndefined(element)) {
            element.blazor__instance.focusIn(element, inline);
        }
    },
    destroy: function (element) {
        if (!sf.base.isNullOrUndefined(element)) {
            element.blazor__instance.addScrollEvents(false);
        }
    }
};

return ColorPicker;

}());

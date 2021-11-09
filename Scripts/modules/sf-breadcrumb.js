window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.Breadcrumb = (function () {
'use strict';

/**
 * Client side scripts for Blazor Breadcrumb
 */
var SfBreadcrumb = /** @class */ (function () {
    function SfBreadcrumb(element, dotnetRef, overflowMode, maxItems, popup, menu) {
        this.element = element;
        this.menu = menu;
        this.popup = popup;
        this.overflowMode = overflowMode;
        this.maxItems = maxItems;
        this.dotnetRef = dotnetRef;
        this.element.blazor__instance = this;
        this.calculateMaxItems();
        this.wireEvents();
    }
    SfBreadcrumb.prototype.calculateMaxItems = function () {
        this.prevWidth = this.element.offsetWidth;
        //this.element.style.visibility = '';
        if (this.overflowMode === 'Default' || this.overflowMode === 'Collapsed' || this.overflowMode === 'Menu') {
            var maxItems = -1;
            var width = this.element.offsetWidth;
            var liElems = [].slice.call(this.element.children[0].children).reverse();
            var liWidth = this.overflowMode === 'Menu' ? 0 : liElems[liElems.length - 1].offsetWidth + liElems[liElems.length - 2].offsetWidth;
            if (this.overflowMode === 'Menu') {
                var menuEle = this.getMenuElement();
                this.element.append(menuEle);
                liWidth += menuEle.offsetWidth;
                menuEle.remove();
            }
            for (var i = 0; i < liElems.length - 2; i++) {
                if (liWidth > width) {
                    maxItems = Math.ceil((i - 1) / 2) + ((this.overflowMode === 'Menu' && i <= 2) ? 0 : 1);
                    break;
                }
                else {
                    if (this.overflowMode === 'Menu' && i === 2) {
                        liWidth += liElems[liElems.length - 1].offsetWidth + liElems[liElems.length - 2].offsetWidth;
                        if (liWidth > width) {
                            maxItems = 1;
                            break;
                        }
                    }
                    if (!(this.overflowMode === 'Menu' && liElems[i].classList.contains('e-breadcrumb-menu'))) {
                        liWidth += liElems[i].offsetWidth;
                    }
                }
            }
            this.dotnetRef.invokeMethodAsync('ChangeMaxItems', maxItems);
        }
        else if ((this.overflowMode === 'Wrap' || this.overflowMode === 'Scroll') && this.maxItems > 0) {
            var width = 0;
            var liElems = this.element.querySelectorAll('.e-breadcrumb-item,.e-breadcrumb-separator');
            for (var i = 0; i < this.maxItems + this.maxItems - 1; i++) {
                width += liElems[i].offsetWidth;
            }
            this.element.style.width = width + 5 + (parseInt(getComputedStyle(this.element.children[0]).paddingLeft, 10) * 2) + 'px';
        }
    };
    SfBreadcrumb.prototype.resize = function () {
        //this.element.style.visibility = 'hidden';
        if (this.prevWidth != this.element.offsetWidth) {
            this.dotnetRef.invokeMethodAsync('ResizeHandler');
        }
        //this.calculateMaxItems();
        //this.dotnetRef.invokeMethodAsync('ChangeMaxItems', -1)
    };
    SfBreadcrumb.prototype.getMenuElement = function () {
        return sf.base.createElement('li', { className: 'e-icons e-breadcrumb-menu' });
    };
    SfBreadcrumb.prototype.openPopup = function (menu, popup) {
        var left;
        var top;
        document.body.appendChild(popup);
        var menuOffset = menu.getBoundingClientRect();
        var popupOffset = popup.getBoundingClientRect();
        left = menuOffset.left + pageXOffset;
        top = menuOffset.bottom + pageYOffset;
        if (menuOffset.bottom + popupOffset.height > document.documentElement.clientHeight) {
            if (top - menuOffset.height - popupOffset.height > document.documentElement.clientTop) {
                top = top - menuOffset.height - popupOffset.height;
            }
        }
        if (menuOffset.left + popupOffset.width > document.documentElement.clientWidth) {
            if (menuOffset.right - popupOffset.width > document.documentElement.clientLeft) {
                left = (left + menuOffset.width) - popupOffset.width;
            }
        }
        this.addEventListener();
        popup.style.left = Math.ceil(left) + 'px';
        popup.style.top = Math.ceil(top) + 'px';
        popup.style.zIndex = sf.popups.getZindexPartial(this.element) + '';
        popup.firstElementChild.focus();
    };
    SfBreadcrumb.prototype.addEventListener = function () {
        sf.base.EventHandler.add(document, 'mousedown', this.mousedownHandler, this);
        if (this.popup) {
            sf.base.EventHandler.add(this.popup, 'keydown', this.popupKeyDownHandler, this);
        }
    };
    SfBreadcrumb.prototype.popupKeyDownHandler = function (e) {
        if (e.key === 'Escape') {
            this.dotnetRef.invokeMethodAsync('ClosePopup', null);
        }
    };
    SfBreadcrumb.prototype.mousedownHandler = function (e) {
        if (this.popup && this.popup.parentElement) {
            var target = e.target;
            if ((!sf.base.closest(target, '#' + this.menu.id) && !sf.base.closest(e.target, '#' + this.popup.id))) {
                this.dotnetRef.invokeMethodAsync('ClosePopup', null);
                this.removeEventListener();
            }
        }
        else {
            this.removeEventListener();
        }
    };
    SfBreadcrumb.prototype.removeEventListener = function () {
        sf.base.EventHandler.remove(document, 'mousedown', this.mousedownHandler);
        if (this.popup) {
            sf.base.EventHandler.remove(this.popup, 'keydown', this.popupKeyDownHandler);
        }
    };
    SfBreadcrumb.prototype.wireEvents = function () {
        window.addEventListener('resize', this.resize.bind(this));
    };
    SfBreadcrumb.prototype.unWireEvents = function () {
        window.removeEventListener('resize', this.resize.bind(this));
    };
    SfBreadcrumb.prototype.destroy = function () {
        this.unWireEvents();
    };
    return SfBreadcrumb;
}());
var Breadcrumb = {
    initialize: function (element, dotnetRef, overflowMode, maxItems) {
        if (element) {
            new SfBreadcrumb(element, dotnetRef, overflowMode, maxItems);
        }
    },
    calculateMaxItems: function (element) {
        if (element) {
            if (element.blazor__instance) {
                element.blazor__instance.calculateMaxItems();
            }
        }
    },
    openPopup: function (element, menu, popup, dotnetRef) {
        if (element) {
            if (!element.blazor__instance) {
                new SfBreadcrumb(element, dotnetRef, 'Menu', -1, popup, menu);
            }
            else {
                element.blazor__instance.popup = popup;
                element.blazor__instance.menu = menu;
            }
            element.blazor__instance.openPopup(menu, popup);
        }
    },
    destroy: function () {
        this.element.blazor__instance.destroy();
    }
};

return Breadcrumb;

}());

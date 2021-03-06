window.sf = window.sf || {};
var sfbreadcrumb = (function (exports) {
'use strict';

var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ICONRIGHT = 'e-icon-right';
var ITEMTEXTCLASS = 'e-breadcrumb-text';
var ICONCLASS = 'e-breadcrumb-icon';
var BreadcrumbItem = /** @class */ (function (_super) {
    __extends(BreadcrumbItem, _super);
    function BreadcrumbItem() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property('')
    ], BreadcrumbItem.prototype, "text", void 0);
    __decorate([
        sf.base.Property('')
    ], BreadcrumbItem.prototype, "url", void 0);
    __decorate([
        sf.base.Property(null)
    ], BreadcrumbItem.prototype, "iconCss", void 0);
    return BreadcrumbItem;
}(sf.base.ChildProperty));
/**
 * Breadcrumb is a graphical user interface that helps to identify or highlight the current location within a hierarchical structure of websites.
 * The aim is to make the user aware of their current position in a hierarchy of website links.
 * ```html
 * <nav id='breadcrumb'></nav>
 * ```
 * ```typescript
 * <script>
 * var breadcrumbObj = new Breadcrumb({ items: [{ text: 'Home', url: '/' }, { text: 'Index', url: './index.html }]});
 * breadcrumbObj.appendTo("#breadcrumb");
 * </script>
 * ```
 */
var Breadcrumb = /** @class */ (function (_super) {
    __extends(Breadcrumb, _super);
    /**
     * Constructor for creating the widget.
     *
     * @private
     * @param {BreadcrumbModel} options - Specifies the Breadcrumb model.
     * @param {string | HTMLElement} element - Specifies the element.
     */
    function Breadcrumb(options, element) {
        return _super.call(this, options, element) || this;
    }
    /**
     * @private
     * @returns {void}
     */
    Breadcrumb.prototype.preRender = function () {
        // pre render code
    };
    /**
     * Initialize the control rendering.
     *
     * @private
     * @returns {void}
     */
    Breadcrumb.prototype.render = function () {
        this.initialize();
        this.renderItems(this.items);
        this.wireEvents();
    };
    Breadcrumb.prototype.initialize = function () {
        this._maxItems = this.maxItems;
        this.element.setAttribute('aria-label', 'breadcrumb');
        if (this.cssClass) {
            sf.base.addClass([this.element], this.cssClass.split(' '));
        }
        if (this.enableRtl) {
            this.element.classList.add('e-rtl');
        }
        if (this.overflowMode === 'Wrap') {
            this.element.classList.add('e-breadcrumb-wrap-mode');
        }
        else if (this.overflowMode === 'Scroll') {
            this.element.classList.add('e-breadcrumb-scroll-mode');
        }
        this.setWidth();
        this.initItems();
        this.initPvtProps();
    };
    Breadcrumb.prototype.initPvtProps = function () {
        var _this = this;
        if (this.overflowMode === 'Default' && this._maxItems > 0) {
            this.startIndex = this.items.length - (this._maxItems - 1);
            this.endIndex = this.items.length - 1;
        }
        if (this.overflowMode === 'Menu' && this._maxItems > 0) {
            this.startIndex = this._maxItems > 1 ? 1 : 0;
            if (this.activeItem) {
                this.items.forEach(function (item, idx) {
                    if (item.url === _this.activeItem) {
                        _this.endIndex = idx;
                    }
                });
            }
            else {
                this.endIndex = this.items.length - 1;
            }
            this.popupUl = this.createElement('ul');
        }
    };
    Breadcrumb.prototype.setWidth = function () {
        if (this.width) {
            this.element.style.width = this.width;
        }
    };
    Breadcrumb.prototype.initItems = function () {
        if (!this.items.length) {
            var baseUri = void 0;
            var uri = void 0;
            var items = [];
            if (this.url) {
                var url = new URL(this.url);
                baseUri = url.origin + '/';
                uri = url.href.split(baseUri)[1].split('/');
            }
            else {
                baseUri = window.location.origin + '/';
                uri = window.location.href.split(baseUri)[1].split('/');
            }
            items.push({ iconCss: 'e-icons e-home', url: baseUri });
            for (var i = 0; i < uri.length; i++) {
                if (uri[i]) {
                    items.push({ text: uri[i], url: baseUri + uri[i] });
                    baseUri += uri[i] + '/';
                }
            }
            this.setProperties({ items: items }, true);
        }
    };
    Breadcrumb.prototype.renderItems = function (items) {
        var _this = this;
        var item;
        var isSingleLevel;
        var isIconRight = this.element.classList.contains(ICONRIGHT);
        var itemsLength = items.length;
        if (itemsLength) {
            var isActiveItem = void 0;
            var isLastItem = void 0;
            var j_1 = 0;
            var wrapDiv = void 0;
            var len = (itemsLength * 2) - 1;
            var ol = this.createElement('ol', { className: this.overflowMode === 'Wrap' ? 'e-breadcrumb-wrapped-ol' : '' });
            var firstOl = this.createElement('ol', { className: this.overflowMode === 'Wrap' ? 'e-breadcrumb-first-ol' : '' });
            var showIcon = this.hasField(items, 'iconCss');
            var isDisabled_1 = this.element.classList.contains('e-disabled');
            var isCollasped = (this.overflowMode === 'Collapsed' && this._maxItems > 0 && itemsLength > this.maxItems && !this.isExpanded);
            var isDefaultOverflowMode_1 = (this.overflowMode === 'Default' && this._maxItems > 0);
            if (this.overflowMode === 'Menu' && this.popupUl) {
                this.popupUl.innerHTML = '';
            }
            var listBaseOptions = {
                moduleName: this.getModuleName(),
                showIcon: showIcon,
                itemNavigable: true,
                itemCreated: function (args) {
                    var isLastItem = args.curData.isLastItem;
                    if (isLastItem && args.item.children.length && !_this.itemTemplate) {
                        delete args.curData.isLastItem;
                        args.item.innerHTML = _this.createElement('span', { className: ITEMTEXTCLASS, innerHTML: args.item.children[0].innerHTML }).outerHTML;
                    }
                    if (args.curData.iconCss && !args.curData.text && !_this.itemTemplate) {
                        args.item.classList.add('e-icon-item');
                    }
                    if (isDefaultOverflowMode_1) {
                        args.item.setAttribute('item-index', j_1.toString());
                    }
                    if (args.item.querySelector('.' + ITEMTEXTCLASS)) {
                        sf.base.EventHandler.add(args.item.querySelector('.' + ITEMTEXTCLASS), 'focus', function () {
                            args.item.classList.add('e-focus');
                        }, _this);
                        sf.base.EventHandler.add(args.item.querySelector('.' + ITEMTEXTCLASS), 'focusout', function () {
                            args.item.classList.remove('e-focus');
                        }, _this);
                    }
                    var eventArgs = {
                        item: sf.base.extend({}, args.curData.properties ?
                            args.curData.properties : args.curData), element: args.item
                    };
                    _this.trigger('beforeItemRender', eventArgs);
                    var isItemDisabled = isDisabled_1 || eventArgs.element.classList.contains('e-disabled');
                    var containsRightIcon = (isIconRight || eventArgs.element.classList.contains(ICONRIGHT));
                    if (containsRightIcon && args.curData.iconCss && !_this.itemTemplate) {
                        args.item.querySelector('.e-anchor-wrap').append(args.item.querySelector('.' + ICONCLASS));
                    }
                    if (isItemDisabled) {
                        args.item.setAttribute('aria-disabled', 'true');
                    }
                    if (args.curData.isEmptyUrl) {
                        args.item.children[0].removeAttribute('href');
                        if ((!isLastItem || (isLastItem && _this.enableActiveItemNavigation)) && !isItemDisabled) {
                            args.item.children[0].setAttribute('tabindex', '0');
                            sf.base.EventHandler.add(args.item.children[0], 'keydown', _this.keyDownHandler, _this);
                        }
                    }
                    if (isLastItem) {
                        args.item.setAttribute('data-active-item', '');
                    }
                    if (!_this.itemTemplate) {
                        _this.beforeItemRenderChanges(args.curData, eventArgs.item, args.item, containsRightIcon);
                    }
                }
            };
            for (var i = 0; i < len; i % 2 && j_1++, i++) {
                isActiveItem = (this.activeItem && this.activeItem === items[j_1].url);
                if (isCollasped && i > 1 && i < len - 2) {
                    continue;
                }
                else if (isDefaultOverflowMode_1 && ((j_1 < this.startIndex || j_1 > this.endIndex)
                    && (i % 2 ? j_1 !== this.startIndex - 1 : true)) && j_1 !== 0) {
                    continue;
                }
                if (i % 2) {
                    // separator item
                    wrapDiv = this.createElement('div', { className: 'e-breadcrumb-item-wrapper' });
                    listBaseOptions.template = this.separatorTemplate ? this.separatorTemplate : '/';
                    listBaseOptions.itemClass = 'e-breadcrumb-separator';
                    isSingleLevel = false;
                    item = [{ previousItem: items[j_1], nextItem: items[j_1 + 1] }];
                }
                else {
                    // list item
                    listBaseOptions.itemClass = '';
                    if (this.itemTemplate) {
                        listBaseOptions.template = this.itemTemplate;
                        isSingleLevel = false;
                    }
                    else {
                        isSingleLevel = true;
                    }
                    item = [sf.base.extend({}, items[j_1].properties ? items[j_1].properties
                            : items[j_1])];
                    if (!item[0].url && !this.itemTemplate) {
                        item = [sf.base.extend({}, item[0], { isEmptyUrl: true, url: '#' })];
                    }
                    isLastItem = (isDefaultOverflowMode_1 || this.overflowMode === 'Menu') && (j_1 === this.endIndex);
                    if ((((i === len - 1 || isLastItem) && !this.itemTemplate) || isActiveItem) && !this.enableActiveItemNavigation) {
                        item[0].isLastItem = true;
                    }
                }
                var parent_1 = ol;
                var lastPopupItemIdx = this.startIndex + this.endIndex - this._maxItems;
                if (this.overflowMode === 'Menu' && j_1 >= this.startIndex && (j_1 <= lastPopupItemIdx && (i % 2 ? !(j_1 === lastPopupItemIdx) : true)) && this.endIndex >= this._maxItems && this._maxItems > 0) {
                    if (i % 2) {
                        continue;
                    }
                    else {
                        parent_1 = this.popupUl;
                    }
                }
                else if (this.overflowMode === 'Wrap') {
                    if (i === 0) {
                        parent_1 = firstOl;
                    }
                    else {
                        parent_1 = wrapDiv;
                    }
                }
                sf.base.append(sf.lists.ListBase.createList(this.createElement, item, listBaseOptions, isSingleLevel, this)
                    .childNodes, parent_1);
                if (this.overflowMode === 'Wrap' && i !== 0 && i % 2 === 0) {
                    ol.append(wrapDiv);
                }
                if (isCollasped && i === 1) {
                    var li = this.createElement('li', { className: 'e-icons e-breadcrumb-collapsed', attrs: { 'tabindex': '0' } });
                    sf.base.EventHandler.add(li, 'keyup', this.expandHandler, this);
                    ol.append(li);
                }
                if (this.overflowMode === 'Menu' && this.startIndex === i && this.endIndex >= this._maxItems && this._maxItems > 0) {
                    ol.append(this.getMenuElement());
                }
                if (isActiveItem || isLastItem) {
                    break;
                }
            }
            if (this.isReact) {
                this.renderReactTemplates();
            }
            if (this.overflowMode === 'Wrap') {
                this.element.append(firstOl);
            }
            this.element.append(ol);
            this.calculateMaxItems();
        }
    };
    Breadcrumb.prototype.calculateMaxItems = function () {
        if (!this._maxItems) {
            if (this.overflowMode === 'Default' || this.overflowMode === 'Collapsed' || this.overflowMode === 'Menu') {
                var width = this.element.offsetWidth;
                var liElems = [].slice.call(this.element.children[0].children).reverse();
                var liWidth = liElems[liElems.length - 1].offsetWidth + liElems[liElems.length - 2].offsetWidth;
                if (this.overflowMode === 'Menu') {
                    var menuEle = this.getMenuElement();
                    this.element.append(menuEle);
                    liWidth += menuEle.offsetWidth;
                    menuEle.remove();
                }
                for (var i = 0; i < liElems.length - 2; i++) {
                    if (liWidth > width) {
                        this._maxItems = Math.ceil((i - 1) / 2) + 1;
                        this.initPvtProps();
                        return this.reRenderItems();
                    }
                    else {
                        liWidth += liElems[i].offsetWidth;
                    }
                }
            }
        }
        else if (this.overflowMode === 'Wrap' && !this.element.style.width) {
            var width = 0;
            var liElems = this.element.querySelectorAll('.e-breadcrumb-item');
            for (var i = 0; i < this._maxItems + this._maxItems - 1; i++) {
                width += liElems[i].offsetWidth;
            }
            this.element.style.width = width + 5 + (parseInt(getComputedStyle(this.element.children[0]).paddingLeft, 10) * 2) + 'px';
        }
    };
    Breadcrumb.prototype.hasField = function (items, field) {
        for (var i = 0, len = items.length; i < len; i++) {
            if (items[i][field]) {
                return true;
            }
        }
        return false;
    };
    Breadcrumb.prototype.getMenuElement = function () {
        return this.createElement('li', { className: 'e-icons e-breadcrumb-menu', attrs: { 'tabindex': '0' } });
    };
    Breadcrumb.prototype.beforeItemRenderChanges = function (prevItem, currItem, elem, isRightIcon) {
        var wrapElem = elem.querySelector('.e-anchor-wrap');
        if (currItem.text !== prevItem.text) {
            wrapElem.childNodes.forEach(function (child) {
                if (child.nodeType === Node.TEXT_NODE) {
                    child.textContent = currItem.text;
                }
            });
        }
        if (currItem.iconCss !== prevItem.iconCss) {
            var iconElem = elem.querySelector('.' + ICONCLASS);
            if (iconElem) {
                if (currItem.iconCss) {
                    sf.base.removeClass([iconElem], prevItem.iconCss.split(' '));
                    sf.base.addClass([iconElem], currItem.iconCss.split(' '));
                }
                else {
                    iconElem.remove();
                }
            }
            else if (currItem.iconCss) {
                var iconElem_1 = this.createElement('span', { className: ICONCLASS + ' ' + currItem.iconCss });
                if (isRightIcon) {
                    sf.base.append([iconElem_1], wrapElem);
                }
                else {
                    wrapElem.insertBefore(iconElem_1, wrapElem.childNodes[0]);
                }
            }
        }
        if (currItem.url !== prevItem.url && this.enableNavigation) {
            var anchor = elem.querySelector('a.' + ITEMTEXTCLASS);
            if (anchor) {
                if (currItem.url) {
                    anchor.setAttribute('href', currItem.url);
                }
                else {
                    anchor.removeAttribute('href');
                }
            }
        }
    };
    Breadcrumb.prototype.reRenderItems = function () {
        this.element.innerHTML = '';
        this.renderItems(this.items);
    };
    Breadcrumb.prototype.clickHandler = function (e) {
        var li = sf.base.closest(e.target, '.e-breadcrumb-item');
        if (li && (sf.base.closest(e.target, '.' + ITEMTEXTCLASS) || this.itemTemplate)) {
            var idx = void 0;
            if (this.overflowMode === 'Wrap') {
                idx = [].slice.call(this.element.querySelectorAll('.e-breadcrumb-item')).indexOf(li);
            }
            else {
                idx = [].slice.call(li.parentElement.children).indexOf(li);
            }
            if (this.overflowMode === 'Menu') {
                if (sf.base.closest(e.target, '.e-breadcrumb-popup')) {
                    idx += this.startIndex;
                    this.endIndex = idx;
                }
                else if (this.element.querySelector('.e-breadcrumb-menu') && idx > [].slice.call(this.element.children[0].children).indexOf(this.element.querySelector('.e-breadcrumb-menu'))) {
                    idx += (this.popupUl.childElementCount * 2) - 2;
                    idx = Math.floor(idx / 2);
                }
            }
            else {
                idx = Math.floor(idx / 2);
            }
            if (this.overflowMode === 'Default' && this._maxItems > 0 && this.endIndex !== 0) {
                idx = parseInt(li.getAttribute('item-index'), 10);
                if (this.startIndex > 1) {
                    this.startIndex -= (this.endIndex - idx);
                }
                this.endIndex = idx;
                this.reRenderItems();
            }
            this.trigger('itemClick', { element: li, item: this.items[idx], event: e });
            if (this.items[idx].url) {
                this.activeItem = this.items[idx].url;
                this.dataBind();
            }
        }
        if (!this.enableNavigation) {
            e.preventDefault();
        }
        if (e.target.classList.contains('e-breadcrumb-collapsed')) {
            this.isExpanded = true;
            this.reRenderItems();
        }
        if (e.target.classList.contains('e-breadcrumb-menu')) {
            this.renderPopup();
        }
    };
    Breadcrumb.prototype.renderPopup = function () {
        var wrapper = this.createElement('div', { className: 'e-breadcrumb-popup' });
        document.body.appendChild(wrapper);
        this.popupObj = new sf.popups.Popup(wrapper, {
            content: this.popupUl,
            relateTo: this.element.querySelector('.e-breadcrumb-menu'),
            enableRtl: this.enableRtl,
            position: { X: 'left', Y: 'bottom' }
        });
        this.popupWireEvents();
        this.popupObj.show();
    };
    Breadcrumb.prototype.documentClickHandler = function (e) {
        if (this.overflowMode === 'Menu' && this.popupObj && this.popupObj.element.classList.contains('e-popup-open') && !sf.base.closest(e.target, '.e-breadcrumb-menu')) {
            this.popupObj.hide();
            this.popupObj.destroy();
            sf.base.detach(this.popupObj.element);
        }
    };
    Breadcrumb.prototype.resize = function () {
        this._maxItems = this.maxItems;
        this.initPvtProps();
        this.reRenderItems();
    };
    Breadcrumb.prototype.expandHandler = function (e) {
        if (e.key === 'Enter') {
            this.isExpanded = true;
            this.reRenderItems();
        }
    };
    Breadcrumb.prototype.keyDownHandler = function (e) {
        if (e.key === 'Enter') {
            this.clickHandler(e);
        }
    };
    /**
     * Called internally if any of the property value changed.
     *
     * @private
     * @param {BreadcrumbModel} newProp - Specifies the new properties.
     * @param {BreadcrumbModel} oldProp - Specifies the old properties.
     * @returns {void}
     */
    Breadcrumb.prototype.onPropertyChanged = function (newProp, oldProp) {
        for (var _i = 0, _a = Object.keys(newProp); _i < _a.length; _i++) {
            var prop = _a[_i];
            switch (prop) {
                case 'activeItem':
                case 'items':
                case 'enableActiveItemNavigation':
                    this.reRenderItems();
                    break;
                case 'overflowMode':
                case 'maxItems':
                    this.initPvtProps();
                    this.reRenderItems();
                    if (oldProp.overflowMode === 'Wrap') {
                        this.element.classList.remove('e-breadcrumb-wrap-mode');
                    }
                    else if (newProp.overflowMode === 'Wrap') {
                        this.element.classList.add('e-breadcrumb-wrap-mode');
                    }
                    if (oldProp.overflowMode === 'Scroll') {
                        this.element.classList.remove('e-breadcrumb-scroll-mode');
                    }
                    else if (newProp.overflowMode === 'Scroll') {
                        this.element.classList.add('e-breadcrumb-scroll-mode');
                    }
                    break;
                case 'url':
                    this.initItems();
                    this.reRenderItems();
                    break;
                case 'width':
                    this.setWidth();
                    this._maxItems = this.maxItems;
                    this.initPvtProps();
                    this.reRenderItems();
                    break;
                case 'cssClass':
                    if (oldProp.cssClass) {
                        sf.base.removeClass([this.element], oldProp.cssClass.split(' '));
                    }
                    if (newProp.cssClass) {
                        sf.base.addClass([this.element], newProp.cssClass.split(' '));
                    }
                    if ((oldProp.cssClass && oldProp.cssClass.indexOf(ICONRIGHT) > -1) && !(newProp.cssClass &&
                        newProp.cssClass.indexOf(ICONRIGHT) > -1) || !(oldProp.cssClass && oldProp.cssClass.indexOf(ICONRIGHT) > -1) &&
                        (newProp.cssClass && newProp.cssClass.indexOf(ICONRIGHT) > -1)) {
                        this.reRenderItems();
                    }
                    break;
                case 'enableRtl':
                    this.element.classList.toggle('e-rtl');
                    break;
            }
        }
    };
    Breadcrumb.prototype.wireEvents = function () {
        this.delegateClickHanlder = this.documentClickHandler.bind(this);
        sf.base.EventHandler.add(document, 'click', this.delegateClickHanlder, this);
        sf.base.EventHandler.add(this.element, 'click', this.clickHandler, this);
        window.addEventListener('resize', this.resize.bind(this));
    };
    Breadcrumb.prototype.popupWireEvents = function () {
        sf.base.EventHandler.add(this.popupObj.element, 'click', this.clickHandler, this);
    };
    Breadcrumb.prototype.unWireEvents = function () {
        sf.base.EventHandler.remove(document, 'click', this.delegateClickHanlder);
        sf.base.EventHandler.remove(this.element, 'click', this.clickHandler);
        window.removeEventListener('resize', this.resize.bind(this));
        if (this.popupObj) {
            sf.base.EventHandler.remove(this.popupObj.element, 'click', this.clickHandler);
        }
    };
    /**
     * Get the properties to be maintained in the persisted state.
     *
     * @returns {string} - Persist data
     */
    Breadcrumb.prototype.getPersistData = function () {
        return this.addOnPersist(['activeItem']);
    };
    /**
     * Get module name.
     *
     * @private
     * @returns {string} - Module Name
     */
    Breadcrumb.prototype.getModuleName = function () {
        return 'breadcrumb';
    };
    /**
     * Destroys the widget.
     *
     * @returns {void}
     */
    Breadcrumb.prototype.destroy = function () {
        this.unWireEvents();
        this.element.innerHTML = '';
        if (this.cssClass) {
            sf.base.removeClass([this.element], this.cssClass.split(' '));
        }
    };
    __decorate([
        sf.base.Property('')
    ], Breadcrumb.prototype, "url", void 0);
    __decorate([
        sf.base.Collection([], BreadcrumbItem)
    ], Breadcrumb.prototype, "items", void 0);
    __decorate([
        sf.base.Property('')
    ], Breadcrumb.prototype, "activeItem", void 0);
    __decorate([
        sf.base.Property(0)
    ], Breadcrumb.prototype, "maxItems", void 0);
    __decorate([
        sf.base.Property('Default')
    ], Breadcrumb.prototype, "overflowMode", void 0);
    __decorate([
        sf.base.Property('')
    ], Breadcrumb.prototype, "cssClass", void 0);
    __decorate([
        sf.base.Property('')
    ], Breadcrumb.prototype, "width", void 0);
    __decorate([
        sf.base.Property(null)
    ], Breadcrumb.prototype, "itemTemplate", void 0);
    __decorate([
        sf.base.Property('/')
    ], Breadcrumb.prototype, "separatorTemplate", void 0);
    __decorate([
        sf.base.Property(true)
    ], Breadcrumb.prototype, "enableNavigation", void 0);
    __decorate([
        sf.base.Property(false)
    ], Breadcrumb.prototype, "enableActiveItemNavigation", void 0);
    __decorate([
        sf.base.Property('')
    ], Breadcrumb.prototype, "locale", void 0);
    __decorate([
        sf.base.Event()
    ], Breadcrumb.prototype, "beforeItemRender", void 0);
    __decorate([
        sf.base.Event()
    ], Breadcrumb.prototype, "itemClick", void 0);
    __decorate([
        sf.base.Event()
    ], Breadcrumb.prototype, "created", void 0);
    Breadcrumb = __decorate([
        sf.base.NotifyPropertyChanges
    ], Breadcrumb);
    return Breadcrumb;
}(sf.base.Component));

/**
 * Breadcrumb modules
 */

exports.BreadcrumbItem = BreadcrumbItem;
exports.Breadcrumb = Breadcrumb;

return exports;

});

    sf.navigations = sf.base.extend({}, sf.navigations, sfbreadcrumb({}));
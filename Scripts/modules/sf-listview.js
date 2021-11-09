window.sfBlazor = window.sfBlazor || {};
window.sfBlazor.ListView = (function () {
'use strict';

var effectsConfig = {
    'None': [],
    'SlideLeft': ['SlideRightOut', 'SlideLeftOut', 'SlideLeftIn', 'SlideRightIn'],
    'SlideDown': ['SlideTopOut', 'SlideBottomOut', 'SlideBottomIn', 'SlideTopIn'],
    'Zoom': ['FadeOut', 'FadeZoomOut', 'FadeZoomIn', 'FadeIn'],
    'Fade': ['FadeOut', 'FadeOut', 'FadeIn', 'FadeIn']
};
var effectsRTLConfig = {
    'None': [],
    'SlideLeft': ['SlideLeftOut', 'SlideRightOut', 'SlideRightIn', 'SlideLeftIn'],
    'SlideDown': ['SlideBottomOut', 'SlideTopOut', 'SlideTopIn', 'SlideBottomIn'],
    'Zoom': ['FadeZoomOut', 'FadeOut', 'FadeIn', 'FadeZoomIn'],
    'Fade': ['FadeOut', 'FadeOut', 'FadeIn', 'FadeIn']
};
var SWIPEVELOCITY = 0.5;
var DATASOURCEKEY = 'defaultData_Key';
var HOVER = 'e-hover';
var FOCUSED = 'e-focused';
var LISTITEM = 'e-list-item';
var GROUPLISTITEM = 'e-list-group-item';
var HASCHILD = 'e-has-child';
var HEADER = 'e-list-header';
var HEADERTEXT = 'e-headertext';
var DISABLE = 'e-disabled';
var BACKICON = 'e-icon-back';
var CHECKBOXWRAPPER = 'e-checkbox-wrapper';
var CHECKED = 'e-check';
var CHECKBOXICON = 'e-frame';
var NONE = 'none';
var VIRTUALULCONTAINER = 'e-list-virtualcontainer';
var SfListView = /** @class */ (function () {
    // tslint:disable
    function SfListView(element, dotnetRef, properties, enableSelectEvent, datasource) {
        this.dataSourceLevel = [DATASOURCEKEY];
        this.currentDataSourceKey = DATASOURCEKEY;
        this.isWindow = false;
        this.liDifference = 0;
        this.liHeight = 0;
        this.virtualListDifference = 0;
        this.isCheckAll = false;
        // used to identify the focus action for list element 
        this.isFocus = false;
        // tslint:enable
        this.element = element;
        this.dotNetRef = dotnetRef;
        this.showCheckBox = properties.ShowCheckBox;
        this.showHeader = properties.ShowHeader;
        this.enable = properties.Enabled;
        this.currentUlElement = element.querySelector('ul');
        this.enableVirtualization = properties.EnableVirtualization;
        this.isWindow = properties.Height ? false : true;
        this.enableSelectEvent = enableSelectEvent;
        this.height = properties.Height;
        this.headerTitleInfo = [properties.HeaderTitle];
        this.selectedItems = { defaultData_Key: properties.SelectedElementIdInfo };
        this.enableRtl = properties.EnableRtl;
        this.animation = properties.Animation;
        this.isTemplate = properties.Template;
        this.focusId = [];
        this.element.blazor__instance = this;
        this.datasource = datasource;
    }
    SfListView.prototype.initialize = function () {
        if (this.enableVirtualization) {
            if (this.isWindow) {
                this.dotNetRef.invokeMethodAsync('GetComponenetHeight', window.innerHeight);
            }
            else if (this.height.indexOf('%') !== -1) {
                var parentContainerHeight = this.element.parentElement.getBoundingClientRect().height;
                this.dotNetRef.invokeMethodAsync('GetComponenetHeight', ((parentContainerHeight / 100) * parseFloat(this.height)));
            }
            this.updateLiElementHeight();
        }
        this.headerElement = this.element.querySelector('.' + HEADERTEXT);
        this.animationObject = new sf.base.Animation(this.animateOptions);
        this.wireEvents();
    };
    SfListView.prototype.wireEvents = function () {
        sf.base.EventHandler.add(this.element, 'focus', this.focusHandler, this);
        sf.base.EventHandler.add(this.element, 'mousedown', this.mouseDownHandler, this);
        sf.base.EventHandler.add(this.element, 'keydown', this.keyActionHandler, this);
        sf.base.EventHandler.add(this.element, 'click', this.clickHandler, this);
        sf.base.EventHandler.add(this.element, 'focusout', this.removeFocus, this);
        this.touchModule = new sf.base.Touch(this.element, { swipe: this.swipeActionHandler.bind(this) });
        if (this.enableVirtualization) {
            sf.base.EventHandler.add(this.element, 'scroll', this.scrollHandler, this);
            if (this.isWindow) {
                window.addEventListener('scroll', this.scrollHandler.bind(this));
            }
        }
        else {
            sf.base.EventHandler.add(this.element, 'mouseover', this.mouseHoverHandler, this);
            sf.base.EventHandler.add(this.element, 'mouseout', this.mouseOutHandler, this);
        }
    };
    SfListView.prototype.unWireEvents = function () {
        sf.base.EventHandler.remove(this.element, 'focus', this.focusHandler);
        sf.base.EventHandler.remove(this.element, 'mousedown', this.mouseDownHandler);
        sf.base.EventHandler.remove(this.element, 'keydown', this.keyActionHandler);
        sf.base.EventHandler.remove(this.element, 'click', this.clickHandler);
        sf.base.EventHandler.remove(this.element, 'focusout', this.removeFocus);
        if (this.enableVirtualization) {
            sf.base.EventHandler.remove(this.element, 'scroll', this.scrollHandler);
            if (this.isWindow) {
                window.removeEventListener('scroll', this.scrollHandler.bind(this));
            }
        }
        else {
            sf.base.EventHandler.remove(this.element, 'mouseover', this.mouseHoverHandler);
            sf.base.EventHandler.remove(this.element, 'mouseout', this.mouseOutHandler);
        }
        this.touchModule.destroy();
    };
    SfListView.prototype.swipeActionHandler = function (event) {
        if (event.swipeDirection === 'Right' && event.velocity > SWIPEVELOCITY && event.originalEvent.type === 'touchend') {
            if (this.showCheckBox && this.dataSourceLevel[this.dataSourceLevel.length - 1]) {
                this.uncheckAllItems();
            }
            this.back();
        }
    };
    SfListView.prototype.showHideItem = function (item, display) {
        var liElement = this.getLi(item);
        if (liElement) {
            liElement.style.display = display;
        }
    };
    SfListView.prototype.enableState = function (item, isEnable) {
        var liElement = this.getLi(item);
        if (liElement) {
            isEnable ? liElement.classList.remove('e-disabled') : liElement.classList.add('e-disabled');
        }
    };
    SfListView.prototype.getLi = function (item) {
        var liElement;
        if (this.element) {
            liElement = this.element.querySelector('[data-uid="' + item.id + '"]');
        }
        return liElement;
    };
    SfListView.prototype.scrollHandler = function (event) {
        var listDiff;
        // tslint:disable
        var scrollTop = this.isWindow ? event.target.documentElement.scrollTop : event.target.scrollTop;
        // tslint:enable
        if (!this.liHeight) {
            this.updateLiElementHeight();
        }
        listDiff = Math.round(scrollTop / this.liHeight);
        if (listDiff - this.liDifference >= this.virtualListDifference || listDiff - this.liDifference <= (-1)) {
            var focuseElement = this.currentUlElement.querySelector('.' + FOCUSED);
            if (focuseElement) {
                this.focusedElementId = focuseElement.getAttribute('data-uid');
            }
            var virtualElementContainer = this.element.querySelector('.' + VIRTUALULCONTAINER);
            if (virtualElementContainer) {
                virtualElementContainer.style.top = (((listDiff - 1) * this.liHeight) < 0) ? '0px' : (listDiff - 2) * this.liHeight + 'px';
                this.liDifference = listDiff;
                this.dotNetRef.invokeMethodAsync('VirtualScrolling', (listDiff - 2));
            }
        }
    };
    SfListView.prototype.preventSelection = function (cancelArgs) {
        var target = this.mouseEvent.target;
        var liElement = sf.base.closest(target.parentNode, '.' + LISTITEM);
        if (cancelArgs) {
            if (this.dataSourceLevel.length > 1) {
                this.dataSourceLevel.pop();
            }
        }
        else {
            this.performSelection(liElement);
        }
    };
    SfListView.prototype.performSelection = function (liElement) {
        if (liElement !== null) {
            if (!liElement.classList.contains(DISABLE) && this.enable) {
                this.removeFocus();
                this.focusId = [];
                this.addFocusId(liElement.getAttribute('data-uid'));
                if (!this.showCheckBox) {
                    if (this.currentUlElement.querySelector('.' + FOCUSED)) {
                        this.currentUlElement.querySelector('.' + FOCUSED).classList.remove(FOCUSED);
                    }
                    if (liElement.classList.contains(HASCHILD)) {
                        this.setSelectLI(liElement, this.mouseEvent);
                    }
                    else {
                        if (!liElement.classList.contains(FOCUSED)) {
                            liElement.classList.add(FOCUSED);
                            this.selectedItems[this.currentDataSourceKey][0] = liElement.getAttribute('data-uid');
                        }
                    }
                }
                else if (this.mouseEvent.target.classList.contains(CHECKBOXICON)) {
                    this.checkUncheckItem(liElement);
                }
                else if (liElement.classList.contains(HASCHILD)) {
                    this.removeHover();
                    this.removeFocus();
                    this.setSelectLI(liElement, this.mouseEvent);
                }
                else {
                    this.checkUncheckItem(liElement);
                }
            }
        }
    };
    SfListView.prototype.updateLiElementHeight = function () {
        var virtualElementContainer = this.element.querySelector('.' + VIRTUALULCONTAINER);
        if (virtualElementContainer) {
            if (!this.currentUlElement) {
                this.currentUlElement = this.element.querySelector('ul');
            }
            var liElement = virtualElementContainer.children[0];
            if (liElement) {
                this.liHeight = liElement.getBoundingClientRect().height;
                if (this.liHeight) {
                    if (this.enableVirtualization) {
                        var parentContainerHeight = virtualElementContainer.getBoundingClientRect().height;
                        virtualElementContainer.parentElement.style.height = parentContainerHeight + "px";
                    }
                    this.dotNetRef.invokeMethodAsync('UpdateLiElementHeight', this.liHeight);
                }
            }
        }
    };
    SfListView.prototype.updateElementDifference = function (listDifference) {
        this.virtualListDifference = listDifference;
        if (this.enableVirtualization) {
            this.updateLiElementHeight();
        }
    };
    SfListView.prototype.selectItem = function (item) {
        var listItem = this.getLi(item);
        if (this.showCheckBox) {
            this.setChecked(listItem, listItem.querySelector('.' + CHECKBOXWRAPPER));
        }
        else {
            sf.base.isNullOrUndefined(listItem) ? this.removeFocus() : this.setSelectLI(listItem, null);
            this.selectedItems[this.currentDataSourceKey][0] = listItem.getAttribute('data-uid');
        }
    };
    SfListView.prototype.clickHandler = function (event) {
        if (this.currentUlElement && !this.element.classList.contains("e-disabled")) {
            var target = event.target;
            var classList = target.classList;
            this.mouseEvent = event;
            if (classList.contains(BACKICON) || classList.contains(HEADERTEXT)) {
                this.back();
            }
            else {
                var liElement = sf.base.closest(target.parentNode, '.' + LISTITEM) ?
                    sf.base.closest(target.parentNode, '.' + LISTITEM) : sf.base.closest(target, '.' + LISTITEM);
                if (!this.enableSelectEvent) {
                    this.performSelection(liElement);
                }
                if (liElement !== null && !liElement.classList.contains(DISABLE) && this.enable) {
                    if (liElement.classList.contains(HASCHILD)) {
                        var uID = liElement.getAttribute('data-uid');
                        if (this.dataSourceLevel.indexOf(uID) === -1)
                            this.dataSourceLevel.push(uID);
                    }
                    if (this.enableSelectEvent) {
                        this.getSelectEventData(liElement, event);
                    }
                }
            }
        }
    };
    SfListView.prototype.checkUncheckItem = function (item) {
        item.classList.add(FOCUSED);
        (!item.querySelector('.' + CHECKED)) ? this.setChecked(item, item.querySelector('.' + CHECKBOXWRAPPER)) : this.uncheckItem(item);
        this.dotNetRef.invokeMethodAsync('UpdateData', !sf.base.isNullOrUndefined(item.querySelector('.' + CHECKED)), item.getAttribute("data-uid"));
    };
    SfListView.prototype.back = function () {
        if (this.dataSourceLevel.length > 1) {
            var ulElement = this.element.querySelectorAll('ul');
            var headerElement = this.element.querySelector('.' + HEADER);
            for (var i = 0; i < ulElement.length; i++) {
                if (this.dataSourceLevel.length > 2) {
                    if (ulElement[i].getAttribute('pid') === this.dataSourceLevel[this.dataSourceLevel.length - 2]) {
                        this.switchView(this.currentUlElement, ulElement[i], true);
                        this.currentUlElement = ulElement[i];
                    }
                    else {
                        ulElement[i].style.display = NONE;
                    }
                }
                else if (ulElement[i].getAttribute('pid') === null) {
                    this.switchView(this.currentUlElement, ulElement[i], true);
                    this.currentUlElement = ulElement[i];
                }
                else {
                    ulElement[i].style.display = NONE;
                }
            }
            this.dataSourceLevel.pop();
            if (!this.showCheckBox) {
                var focused_element = this.currentUlElement.querySelectorAll('.' + FOCUSED);
                focused_element[0].setAttribute('aria-selected', "false");
            }
            if (!this.isTemplate) {
                if (this.headerTitleInfo.length > 1)
                    this.headerTitleInfo.pop();
                if (this.headerElement) {
                    this.headerElement.innerText = this.headerTitleInfo[this.headerTitleInfo.length - 1];
                }
            }
            this.currentDataSourceKey = this.dataSourceLevel[this.dataSourceLevel.length - 1];
            if (this.dataSourceLevel.length === 1 && headerElement) {
                headerElement.children[0].style.display = NONE;
            }
            var backEventArgs = {
                IsInteracted: event || this.isRootFocus ? true : false, Level: this.dataSourceLevel.length > 0 ? this.dataSourceLevel.length - 1 : this.dataSourceLevel.length
            };
            this.dotNetRef.invokeMethodAsync('TriggerBackEvent', backEventArgs);
        }
    };
    SfListView.prototype.setHoverLI = function (liElement) {
        if (!this.element.classList.contains(DISABLE) && !liElement.classList.contains(DISABLE)) {
            liElement.classList.add(HOVER);
        }
    };
    SfListView.prototype.mouseHoverHandler = function (event) {
        var currentLiElemet = sf.base.closest(event.target.parentNode, '.' + LISTITEM);
        if (currentLiElemet) {
            this.setHoverLI(currentLiElemet);
        }
    };
    SfListView.prototype.mouseOutHandler = function (event) {
        this.removeHover();
    };
    SfListView.prototype.removeHover = function () {
        var hoverLI = this.element.querySelector('.' + HOVER);
        if (hoverLI) {
            hoverLI.classList.remove(HOVER);
        }
    };
    SfListView.prototype.removeFocus = function () {
        if (!this.currentUlElement) {
            this.currentUlElement = this.element.querySelector('ul');
        }
        var focusedLI = this.currentUlElement.querySelectorAll('.' + FOCUSED);
        for (var _i = 0, focusedLI_1 = focusedLI; _i < focusedLI_1.length; _i++) {
            var element = focusedLI_1[_i];
            element.classList.remove(FOCUSED);
        }
        this.isRestrictFocus = undefined;
    };
    SfListView.prototype.isValidLI = function (liElement) {
        return (liElement && liElement.classList.contains(LISTITEM)
            && !liElement.classList.contains(GROUPLISTITEM)
            && !liElement.classList.contains(DISABLE));
    };
    SfListView.prototype.setSelectLI = function (liElement, e) {
        if (this.enable && this.isValidLI(liElement) && !liElement.classList.contains(FOCUSED)) {
            var focusedElement = this.currentUlElement.querySelector('.' + FOCUSED);
            if (focusedElement) {
                this.removeFocusId(focusedElement.getAttribute('data-uid'));
            }
            this.removeFocus();
            this.addAriaAttribute(true, liElement);
            this.removeHover();
            if (liElement.classList.contains(HASCHILD)) {
                this.renderSubList(liElement);
            }
        }
    };
    SfListView.prototype.addAriaAttribute = function (isSelected, element) {
        if (isSelected) {
            if (!this.isCheckAll) {
                element.classList.add(FOCUSED);
            }
            this.addFocusId(element.getAttribute('data-uid'));
        }
        else if (!this.showCheckBox && (!isSelected && element.classList.contains(FOCUSED))) {
            element.classList.remove(FOCUSED);
            if (this.focusId.indexOf(element.getAttribute('data-uid')) >= 0) {
                this.focusId.splice(this.focusId.indexOf(element.getAttribute('data-uid')), 1);
            }
        }
        if (this.showCheckBox || element.classList.contains(HASCHILD)) {
            element.setAttribute('aria-selected', isSelected.toString());
        }
    };
    SfListView.prototype.renderSubList = function (listItem) {
        var liElement = listItem;
        var uID = listItem.getAttribute('data-uid');
        var headerElement = this.element.querySelector('.' + HEADER);
        listItem.classList.remove(FOCUSED);
        listItem.classList.add(FOCUSED);
        if (this.showHeader && headerElement) {
            headerElement.children[0].style.display = null;
        }
        if (liElement.classList.contains(HASCHILD) && uID) {
            var ulElement = this.element.querySelector('[pid=\'' + uID + '\']');
            if (!ulElement) {
                var args = { ElementId: uID, Key: this.currentDataSourceKey };
                // tslint:disable
                this.dotNetRef.invokeMethodAsync('ListChildDataSource', args);
                // tslint:enable   
            }
            else {
                this.renderChildList(uID, [uID]);
            }
            if (!this.isTemplate) {
                this.headerTitleInfo.push(liElement.innerText.trim());
                if (this.headerElement) {
                    this.headerElement.innerText = this.headerTitleInfo[this.headerTitleInfo.length - 1];
                }
            }
            this.dataSourceLevel.push(uID);
            this.currentDataSourceKey = uID;
        }
    };
    SfListView.prototype.renderChildList = function (id, selectedItems) {
        var ulElement = this.element.querySelectorAll('ul');
        if (!ulElement[ulElement.length - 1].getAttribute('pid')) {
            ulElement[ulElement.length - 1].setAttribute('pid', id);
        }
        for (var i = 0; i < ulElement.length; i++) {
            if (ulElement[i].getAttribute('pid') === id) {
                this.switchView(this.currentUlElement, ulElement[i], false);
                this.currentUlElement = ulElement[i];
                var list_item = ulElement[i].querySelectorAll(".e-list-item");
                if (list_item[0].getAttribute("aria-level") !== i.toString()) {
                    for (var j = 0; j < list_item.length; j++) {
                        list_item[j].setAttribute('aria-level', i.toString());
                    }
                }
                if (selectedItems) {
                    this.selectedItems[id] = selectedItems;
                }
            }
        }
    };
    SfListView.prototype.mouseDownHandler = function () {
        this.isRestrictFocus = this.isRestrictFocus == undefined ? true : this.isRestrictFocus;
    };
    SfListView.prototype.focusHandler = function (e) {
        if (!this.isRestrictFocus) {
            if (sf.base.isNullOrUndefined(this.focusId) || this.focusId.length == 0) {
                this.isRootFocus = true;
                this.arrowKeyHandler(e);
                this.isRootFocus = false;
            }
            else {
                this.isFocus = true;
                this.updateFocusElement(e);
            }
        }
        else {
            this.isRestrictFocus = false;
        }
    };
    SfListView.prototype.keyActionHandler = function (event) {
        switch (event.keyCode) {
            case 36:
                this.homeKeyHandler(event);
                break;
            case 35:
                this.homeKeyHandler(event, true);
                break;
            case 40:
                this.arrowKeyHandler(event);
                break;
            case 38:
                this.arrowKeyHandler(event, true);
                break;
            case 39:
                this.arrowKeyHandler(event);
                break;
            case 37:
                this.arrowKeyHandler(event, true);
                break;
            case 13:
                this.enterKeyHandler(event);
                break;
            case 8:
                if (this.showCheckBox && this.currentDataSourceKey) {
                    this.uncheckAllItems();
                }
                this.back();
                break;
            case 32:
                this.spaceKeyHandler(event);
                break;
        }
    };
    SfListView.prototype.addFocusId = function (id) {
        if (this.focusId.indexOf(id) < 0) {
            this.focusId.push(id);
        }
    };
    SfListView.prototype.removeFocusId = function (id) {
        if (this.focusId.indexOf(id) >= 0) {
            this.focusId.splice(this.focusId.indexOf(id), 1);
        }
    };
    SfListView.prototype.updateFocusElement = function (event) {
        var focusedElement = this.currentUlElement.querySelector('.' + FOCUSED);
        if (focusedElement) {
            focusedElement.classList.remove(FOCUSED);
            if (this.focusId.indexOf(focusedElement.getAttribute('data-uid')) >= 0) {
                this.focusId.splice(this.focusId.indexOf(focusedElement.getAttribute('data-uid')), 1);
            }
        }
        var liCollection = this.currentUlElement.children;
        for (var i = 0; i < liCollection.length; i++) {
            if (this.focusId.indexOf(liCollection[i].getAttribute('data-uid')) >= 0) {
                liCollection[i].classList.add(FOCUSED);
                this.addFocusId(liCollection[i].getAttribute('data-uid'));
                this.addAriaAttribute(true, liCollection[i]);
                this.element.setAttribute('aria-activedescendant', liCollection[i].id.toString());
                if (this.enableSelectEvent) {
                    this.getSelectEventData(liCollection[i], event);
                    this.isFocus = false;
                }
            }
        }
    };
    SfListView.prototype.homeKeyHandler = function (event, end) {
        event.preventDefault();
        var focusedElement = this.currentUlElement.querySelector('.' + FOCUSED);
        if (focusedElement) {
            focusedElement.classList.remove(FOCUSED);
            this.removeFocusId(focusedElement.getAttribute('data-uid'));
        }
        var index = !end ? 0 : this.currentUlElement.children.length - 1;
        index = this.currentUlElement.children[index].classList.contains(GROUPLISTITEM) && this.currentUlElement.children.length > 1 ? (index + 1) : index;
        var liElement = this.currentUlElement.children[index];
        this.addAriaAttribute(true, liElement);
        liElement.classList.add(FOCUSED);
        this.addFocusId(liElement.getAttribute('data-uid'));
        if (this.currentUlElement.children[index]) {
            this.element.setAttribute('aria-activedescendant', this.currentUlElement.children[index].id.toString());
        }
        else {
            this.element.removeAttribute('aria-activedescendant');
        }
        if (this.enableSelectEvent) {
            this.getSelectEventData(liElement, event);
        }
    };
    SfListView.prototype.onArrowKeyDown = function (event, previouse) {
        var siblingLI;
        var liElement;
        var hasChildElement = !sf.base.isNullOrUndefined(this.currentUlElement.querySelector('.' + HASCHILD)) ? true : false;
        if (hasChildElement || this.showCheckBox) {
            liElement = this.currentUlElement.querySelector('.' + FOCUSED) || this.currentUlElement.querySelector('.' + FOCUSED);
            if (liElement) {
                this.removeFocusId(liElement.getAttribute('data-uid'));
            }
            siblingLI = this.getSiblingLI(this.currentUlElement.querySelectorAll('.' + LISTITEM), liElement, previouse);
            if (!sf.base.isNullOrUndefined(siblingLI)) {
                if (liElement) {
                    liElement.classList.remove(FOCUSED);
                    if (!this.showCheckBox) {
                        liElement.classList.remove(FOCUSED);
                    }
                    this.removeFocusId(liElement.getAttribute('data-uid'));
                }
                if (siblingLI.classList.contains(HASCHILD) || this.showCheckBox) {
                    siblingLI.classList.add(FOCUSED);
                }
                else {
                    this.setSelectLI(siblingLI, event);
                }
            }
        }
        else {
            liElement = this.currentUlElement.querySelector('.' + FOCUSED);
            siblingLI = this.getSiblingLI(this.currentUlElement.querySelectorAll('.' + LISTITEM), liElement, previouse);
            this.setSelectLI(siblingLI, event);
        }
        if (siblingLI) {
            this.element.setAttribute('aria-activedescendant', siblingLI.id.toString());
        }
        else {
            this.element.removeAttribute('aria-activedescendant');
        }
        return siblingLI;
    };
    SfListView.prototype.getSiblingLI = function (elementArray, element, isPrevious) {
        var licollection = Array.prototype.slice.call(elementArray);
        var currentIndex = licollection.indexOf(element);
        return isPrevious ? licollection[currentIndex - 1] : licollection[currentIndex + 1];
    };
    SfListView.prototype.arrowKeyHandler = function (event, previous) {
        if (!this.isRootFocus) {
            event.preventDefault();
        }
        if (this.currentUlElement) {
            var siblingLI = this.onArrowKeyDown(event, previous);
            if (siblingLI) {
                this.addFocusId(siblingLI.getAttribute('data-uid'));
            }
            var elementTop = this.element.getBoundingClientRect().top;
            var elementHeight = this.element.getBoundingClientRect().height;
            var heightDiff = void 0;
            if (siblingLI) {
                var siblingTop = siblingLI.getBoundingClientRect().top;
                var siblingHeight = siblingLI.getBoundingClientRect().height;
                if (!previous) {
                    var height = this.isWindow ? window.innerHeight : elementHeight;
                    heightDiff = this.isWindow ? (siblingTop + siblingHeight) :
                        ((siblingTop - elementTop) + siblingHeight);
                    if (heightDiff > height) {
                        this.isWindow ? window.scroll(0, pageYOffset + (heightDiff - height)) :
                            this.element.scrollTop = this.element.scrollTop + (heightDiff - height);
                    }
                }
                else {
                    heightDiff = this.isWindow ? siblingTop : (siblingTop - elementTop);
                    if (heightDiff < 0) {
                        this.isWindow ? window.scroll(0, pageYOffset + heightDiff) :
                            this.element.scrollTop = this.element.scrollTop + heightDiff;
                    }
                }
            }
        }
    };
    SfListView.prototype.enterKeyHandler = function (event) {
        if (this.currentUlElement) {
            var liElement = this.currentUlElement.querySelector('.' + FOCUSED);
            if ((this.currentUlElement.querySelector('.' + HASCHILD)) && liElement) {
                liElement.classList.remove(FOCUSED);
                if (this.showCheckBox) {
                    this.removeFocus();
                    this.removeHover();
                }
                this.setSelectLI(liElement, event);
            }
            else if (!sf.base.isNullOrUndefined(liElement)) {
                this.spaceKeyHandler(event);
            }
        }
    };
    SfListView.prototype.checkAllItems = function () {
        this.isCheckAll = true;
        this.updateCheckBoxState(true);
        this.isCheckAll = false;
    };
    SfListView.prototype.uncheckAllItems = function () {
        this.updateCheckBoxState(false);
    };
    SfListView.prototype.updateCheckBoxState = function (isChecked) {
        if (this.showCheckBox) {
            var liCollection = this.currentUlElement.querySelectorAll('li');
            var liElementCount = !this.enableVirtualization ?
                this.currentUlElement.childElementCount : this.currentUlElement.querySelector('.' + VIRTUALULCONTAINER).childElementCount;
            for (var i = 0; i < liElementCount; i++) {
                var checkIcon = liCollection[i].querySelector('.' + CHECKBOXICON);
                if (checkIcon) {
                    if (isChecked && !checkIcon.classList.contains(CHECKED)) {
                        this.checkItem(liCollection[i]);
                    }
                    else if (!isChecked && checkIcon.classList.contains(CHECKED)) {
                        this.uncheckItem(liCollection[i]);
                    }
                }
            }
        }
    };
    SfListView.prototype.checkItem = function (item) {
        this.toggleCheckBox(item, true);
    };
    //tslint:disable-next-line
    SfListView.prototype.getCheckData = function (item, isCheck, fieldId) {
        var id = item.id;
        if (fieldId !== item.id || sf.base.isNullOrUndefined(item.id)) {
            fieldId = fieldId.toLowerCase();
            //tslint:disable-next-line
            for (var _i = 0, _a = Object.entries(item); _i < _a.length; _i++) {
                var _b = _a[_i], key = _b[0], value = _b[1];
                var tempItem = "" + key;
                var tempVal = "" + value;
                tempItem = tempItem.toLowerCase();
                if (tempItem === fieldId) {
                    id = tempVal;
                }
            }
        }
        var liItem = this.currentUlElement.querySelector('[data-uid=\'' + id + '\']');
        isCheck ? this.checkItem(liItem) : this.uncheckItem(liItem);
        this.removeFocus();
    };
    SfListView.prototype.spaceKeyHandler = function (event) {
        event.preventDefault();
        if (this.enable && this.showCheckBox && this.currentUlElement) {
            var liElement = this.currentUlElement.querySelector('.' + FOCUSED);
            if (!sf.base.isNullOrUndefined(liElement) && sf.base.isNullOrUndefined(liElement.querySelector('.' + CHECKED))) {
                this.setChecked(liElement, liElement.querySelector('.' + CHECKBOXWRAPPER));
            }
            else {
                this.uncheckItem(liElement);
            }
            if (this.enableSelectEvent) {
                this.getSelectEventData(liElement, event);
            }
        }
    };
    SfListView.prototype.setChecked = function (item, checkboxElement) {
        this.removeFocus();
        item.classList.add(FOCUSED);
        this.addAriaAttribute(true, item);
        if (checkboxElement) {
            checkboxElement.querySelector('.' + CHECKBOXICON).classList.add(CHECKED);
            checkboxElement.setAttribute('aria-checked', 'true');
        }
        if (this.selectedItems[this.currentDataSourceKey] && this.selectedItems[this.currentDataSourceKey].indexOf(item.getAttribute('data-uid')) === -1) {
            this.selectedItems[this.currentDataSourceKey].push(item.getAttribute('data-uid'));
        }
    };
    SfListView.prototype.toggleCheckBox = function (item, isChecked) {
        if (this.showCheckBox) {
            var liElement = item;
            if (!sf.base.isNullOrUndefined(liElement)) {
                var checkboxIconElement = liElement.querySelector('.' + CHECKBOXICON);
                this.addAriaAttribute(isChecked, liElement);
                if (!sf.base.isNullOrUndefined(checkboxIconElement)) {
                    isChecked ? checkboxIconElement.classList.add(CHECKED) : checkboxIconElement.classList.remove(CHECKED);
                    checkboxIconElement.parentElement.setAttribute('aria-checked', isChecked ? 'true' : 'false');
                }
            }
        }
    };
    SfListView.prototype.uncheckItem = function (item) {
        if (item) {
            if (this.selectedItems[this.currentDataSourceKey] && this.selectedItems[this.currentDataSourceKey].indexOf(item.getAttribute('data-uid')) !== -1) {
                this.selectedItems[this.currentDataSourceKey].splice(this.selectedItems[this.currentDataSourceKey].indexOf(item.getAttribute('data-uid')), 1);
            }
            this.toggleCheckBox(item, false);
        }
    };
    SfListView.prototype.addCheckClass = function () {
        if (!this.currentUlElement) {
            this.currentUlElement = this.element.querySelector('ul');
        }
        var liCollection = this.enableVirtualization ?
            this.currentUlElement.querySelector('.' + VIRTUALULCONTAINER).children : this.currentUlElement.children;
        var selectedItemsId = this.selectedItems[this.currentDataSourceKey];
        for (var i = 0; i < liCollection.length; i++) {
            if (!this.showCheckBox) {
                if (selectedItemsId[0] === liCollection[i].getAttribute('data-uid')) {
                    liCollection[i].classList.add(FOCUSED);
                }
                else {
                    liCollection[i].classList.remove(FOCUSED);
                }
            }
            else {
                if (this.focusedElementId) {
                    this.focusedElementId === liCollection[i].getAttribute('data-uid') ?
                        liCollection[i].classList.add(FOCUSED) : liCollection[i].classList.remove(FOCUSED);
                }
                if (selectedItemsId.length > 0) {
                    if (selectedItemsId.indexOf(liCollection[i].getAttribute('data-uid')) !== -1) {
                        this.toggleCheckBox(liCollection[i], true);
                    }
                    else {
                        this.toggleCheckBox(liCollection[i], false);
                    }
                }
            }
        }
        for (var i = 0; i < liCollection.length; i++) {
            if (!this.showCheckBox) {
                if (selectedItemsId && selectedItemsId[0] === liCollection[i].getAttribute('data-uid')) {
                    liCollection[i].classList.add(FOCUSED);
                }
                else {
                    liCollection[i].classList.remove(FOCUSED);
                }
            }
            else {
                if (this.focusedElementId) {
                    this.focusedElementId === liCollection[i].getAttribute('data-uid') ?
                        liCollection[i].classList.add(FOCUSED) : liCollection[i].classList.remove(FOCUSED);
                }
                if (selectedItemsId.length > 0) {
                    if (selectedItemsId.indexOf(liCollection[i].getAttribute('data-uid')) !== -1) {
                        this.toggleCheckBox(liCollection[i], true);
                    }
                    else {
                        this.toggleCheckBox(liCollection[i], false);
                    }
                }
            }
        }
    };
    
    SfListView.prototype.getSelectedItems = function () {
        var liCollection = this.currentUlElement.querySelectorAll(".e-check");
        this.selectedItems[this.currentDataSourceKey] = [];
        for (var i = 0; i < liCollection.length; i++) {
            var item = sf.base.closest(liCollection[i], '.' + LISTITEM);
            if (this.selectedItems[this.currentDataSourceKey] && this.selectedItems[this.currentDataSourceKey].indexOf(item.getAttribute('data-uid')) === -1) {
                this.selectedItems[this.currentDataSourceKey].push(item.getAttribute('data-uid'));
            }
        }
        return { ElementId: this.selectedItems[this.currentDataSourceKey], Key: this.currentDataSourceKey };
    };
    
    SfListView.prototype.getSelectEventData = function (liElement, event) {
        if (liElement) {
            var checked = (liElement).querySelector('.' + CHECKED) ? true : false;
            var clickEventArgs = {
                ElementId: liElement.getAttribute('data-uid'), IsChecked: checked,
                Key: liElement.classList.contains(HASCHILD) && !(event.target.classList.contains(CHECKBOXICON)) ? this.dataSourceLevel[this.dataSourceLevel.length - 2] :
                    this.currentDataSourceKey, IsInteracted: event || this.isRootFocus ? true : false, Level: this.dataSourceLevel.length > 0 ? this.dataSourceLevel.length - 1 : this.dataSourceLevel.length
            };
            if (!this.isFocus) {
                this.dotNetRef.invokeMethodAsync('TriggerClickEvent', clickEventArgs);
            }
        }
    };
    // Animation Related Functions
    SfListView.prototype.switchView = function (fromView, toView, reverse) {
        var _this = this;
        if (fromView && toView) {
            var fromViewPosition_1 = fromView.style.position;
            var overflow_1 = (this.element.style.overflow !== 'hidden') ? this.element.style.overflow : '';
            var animationEffect = void 0;
            var duration = this.animation.duration;
            fromView.style.position = 'absolute';
            fromView.classList.add('e-view');
            if (this.animation.effect) {
                animationEffect = (this.enableRtl ? effectsRTLConfig[this.animation.effect] : effectsConfig[this.animation.effect]);
            }
            else {
                var slideLeft = 'SlideLeft';
                animationEffect = effectsConfig[slideLeft];
                reverse = this.enableRtl;
                duration = 0;
            }
            this.element.style.overflow = 'hidden';
            this.animationObject.animate(fromView, {
                name: (reverse ? animationEffect[0] : animationEffect[1]),
                duration: duration,
                timingFunction: this.animation.easing,
                end: function (model) {
                    fromView.style.display = NONE;
                    _this.element.style.overflow = overflow_1;
                    fromView.style.position = fromViewPosition_1;
                    fromView.classList.remove('e-view');
                }
            });
            toView.style.display = '';
            this.animationObject.animate(toView, {
                name: (reverse ? animationEffect[2] : animationEffect[3]),
                duration: duration,
                timingFunction: this.animation.easing,
                end: function () {
                    _this.dotNetRef.invokeMethodAsync('TriggerActionComplete', _this.datasource);
                }
            });
            this.currentUlElement = toView;
        }
    };
    SfListView.prototype.setAnimation = function (animation) {
        this.animation = animation;
    };
    SfListView.prototype.setSelectedItems = function (selectedElementIdInfo) {
        var headerElement = this.element.querySelector('.' + HEADER);
        if (!sf.base.isNullOrUndefined(selectedElementIdInfo)) {
            this.selectedItems = { defaultData_Key: selectedElementIdInfo };
        }
        if (selectedElementIdInfo.length > 0) {
            this.currentUlElement = this.element.querySelector('[data-uid="' + selectedElementIdInfo + '"]').closest("ul");
        }
        else {
            this.currentUlElement = this.element.querySelector("ul");
        }
        var isChild = this.element.querySelector('[data-uid="' + selectedElementIdInfo + '"]').classList.contains("e-has-child");
        if (isChild) {
            this.dataSourceLevel = [DATASOURCEKEY];
            this.currentUlElement.style.removeProperty('display');
            this.addCheckClass();
            this.currentDataSourceKey = DATASOURCEKEY;
        }
        else {
            this.selectedItems[this.currentDataSourceKey] = [this.currentDataSourceKey];
        }
        if (this.showCheckBox) {
            this.removeFocus();
        }
        if (this.headerElement && isChild) {
            this.headerTitleInfo = this.headerTitleInfo.splice(0, 1);
            this.headerElement.innerText = this.headerTitleInfo[this.headerTitleInfo.length - 1];
        }
        if (this.dataSourceLevel.length === 1 && headerElement && isChild) {
            headerElement.children[0].style.display = NONE;
        }
    };
    SfListView.prototype.updateHeaderTitle = function (title) {
        this.headerTitleInfo[0] = title;
        if (this.headerElement) {
            this.headerElement.innerText = title;
        }
    };
    SfListView.prototype.destroy = function () {
        this.element.style.display = NONE;
        this.unWireEvents();
    };
    return SfListView;
}());
var listView = {
    // tslint:disable
    initialize: function (element, dotnetRef, properties, isSelect, liDiff, datasource) {
        // tslint:enable
        if (this.isValid(element)) {
            new SfListView(element, dotnetRef, properties, isSelect, datasource);
            element.blazor__instance.initialize();
            element.blazor__instance.updateElementDifference(liDiff);
        }
    },
    renderChildList: function (element, parentId, selectedItems) {
        if (this.isValid(element)) {
            element.blazor__instance.renderChildList(parentId, selectedItems);
        }
    },
    addActiveClass: function (element) {
        if (this.isValid(element)) {
            element.blazor__instance.addCheckClass();
        }
    },
    // tslint:disable
    showHideItem: function (element, item, display) {
        // tslint:enable
        if (this.isValid(element)) {
            element.blazor__instance.showHideItem(item, display);
        }
    },
    // tslint:disable
    enableState: function (element, item, isEnable) {
        // tslint:enable
        if (this.isValid(element)) {
            element.blazor__instance.enableState(item, isEnable);
        }
    },
    back: function (element) {
        if (this.isValid(element)) {
            element.blazor__instance.back();
        }
    },
    checkAllItems: function (element) {
        if (this.isValid(element)) {
            element.blazor__instance.checkAllItems();
        }
    },
    uncheckAllItems: function (element) {
        if (this.isValid(element)) {
            element.blazor__instance.uncheckAllItems();
        }
    },
    // tslint:disable
    getCheckData: function (element, item, isCheck, fieldId) {
        // tslint:enable
        if (this.isValid(element) && item != null) {
            for (var i = 0; i < item.length; i++) {
                element.blazor__instance.getCheckData(item[i], isCheck, fieldId);
            }
        }
    },
    // tslint:disable
    selectItem: function (element, item) {
        // tslint:enable
        if (element && item != null) {
            for (var i = 0; i < item.length; i++) {
                element.blazor__instance.selectItem(item[i]);
            }
        }
    },
    preventSelection: function (element, cancelArgs) {
        // tslint:enable
        element.blazor__instance.preventSelection(cancelArgs);
    },
    updateLiElementHeight: function (element) {
        if (this.isValid(element)) {
            element.blazor__instance.updateLiElementHeight();
        }
    },
    getCheckedItems: function getCheckedItems(element) {
        return this.isValid(element) ? element.blazor__instance.getSelectedItems() : {};
    },
    setAnimation: function setAnimation(element, animaton) {
        if (this.isValid(element)) {
            element.blazor__instance.setAnimation(animaton);
        }
    },
    setCheckedItems: function setCheckedItems(element, selectedElementIdInfo) {
        if (this.isValid(element)) {
            element.blazor__instance.setSelectedItems(selectedElementIdInfo);
        }
    },
    updateHeaderTitle: function updateHeaderTitle(element, title) {
        if (this.isValid(element)) {
            element.blazor__instance.updateHeaderTitle(title);
        }
    },
    destroy: function destroy(element) {
        if (this.isValid(element)) {
            element.blazor__instance.destroy();
        }
    },
    updateElementDifference: function updateElementDifference(element, listDiff) {
        if (this.isValid(element)) {
            element.blazor__instance.updateElementDifference(listDiff);
        }
    },
    isValid: function (element) {
        return (element) ? true : false;
    }
};

return listView;

}());

window.sf = window.sf || {};
var popupsbase = (function (exports) {
'use strict';

/**
 * Position library
 */
var elementRect;
var popupRect;
var element;
var parentDocument$1;
var fixedParent = false;
/**
 *
 * @param {HTMLElement} anchor - specifies the element
 * @param {HTMLElement} element - specifies the element
 * @returns {OffsetPosition} - returns the value
 */
function calculateRelativeBasedPosition(anchor, element) {
    var fixedElement = false;
    var anchorPos = { left: 0, top: 0 };
    var tempAnchor = anchor;
    if (!anchor || !element) {
        return anchorPos;
    }
    if (sf.base.isNullOrUndefined(element.offsetParent) && element.style.position === 'fixed') {
        fixedElement = true;
    }
    while ((element.offsetParent || fixedElement) && anchor && element.offsetParent !== anchor) {
        anchorPos.left += anchor.offsetLeft;
        anchorPos.top += anchor.offsetTop;
        anchor = anchor.offsetParent;
    }
    anchor = tempAnchor;
    while ((element.offsetParent || fixedElement) && anchor && element.offsetParent !== anchor) {
        anchorPos.left -= anchor.scrollLeft;
        anchorPos.top -= anchor.scrollTop;
        anchor = anchor.parentElement;
    }
    return anchorPos;
}
/**
 *
 * @param {Element} currentElement - specifies the element
 * @param {string} positionX - specifies the position
 * @param {string} positionY - specifies the position
 * @param {boolean} parentElement - specifies the boolean
 * @param {ClientRect} targetValues - specifies the client
 * @returns {OffsetPosition} - returns the position
 */
function calculatePosition(currentElement, positionX, positionY, parentElement, targetValues) {
    //eslint-disable-next-line
    popupRect = undefined;
    popupRect = targetValues;
    fixedParent = parentElement ? true : false;
    if (!currentElement) {
        return { left: 0, top: 0 };
    }
    if (!positionX) {
        positionX = 'left';
    }
    if (!positionY) {
        positionY = 'top';
    }
    parentDocument$1 = currentElement.ownerDocument;
    element = currentElement;
    var pos = { left: 0, top: 0 };
    return updatePosition(positionX.toLowerCase(), positionY.toLowerCase(), pos);
}
/**
 *
 * @param {number} value - specifies the number
 * @param {OffsetPosition} pos - specifies the position
 * @returns {void}
 */
function setPosx(value, pos) {
    pos.left = value;
}
/**
 *
 * @param {number} value - specifies the number
 * @param {OffsetPosition} pos - specifies the position
 * @returns {void}
 */
function setPosy(value, pos) {
    pos.top = value;
}
/**
 *
 * @param {string} posX - specifies the position
 * @param {string} posY - specifies the position
 * @param {OffsetPosition} pos - specifies the position
 * @returns {OffsetPosition} - returns the postion
 */
function updatePosition(posX, posY, pos) {
    elementRect = element.getBoundingClientRect();
    switch (posY + posX) {
        case 'topcenter':
            setPosx(getElementHCenter(), pos);
            setPosy(getElementTop(), pos);
            break;
        case 'topright':
            setPosx(getElementRight(), pos);
            setPosy(getElementTop(), pos);
            break;
        case 'centercenter':
            setPosx(getElementHCenter(), pos);
            setPosy(getElementVCenter(), pos);
            break;
        case 'centerright':
            setPosx(getElementRight(), pos);
            setPosy(getElementVCenter(), pos);
            break;
        case 'centerleft':
            setPosx(getElementLeft(), pos);
            setPosy(getElementVCenter(), pos);
            break;
        case 'bottomcenter':
            setPosx(getElementHCenter(), pos);
            setPosy(getElementBottom(), pos);
            break;
        case 'bottomright':
            setPosx(getElementRight(), pos);
            setPosy(getElementBottom(), pos);
            break;
        case 'bottomleft':
            setPosx(getElementLeft(), pos);
            setPosy(getElementBottom(), pos);
            break;
        default:
        case 'topleft':
            setPosx(getElementLeft(), pos);
            setPosy(getElementTop(), pos);
            break;
    }
    return pos;
}
/**
 * @returns {number} - specifies the number value
 */
function getBodyScrollTop$1() {
    return parentDocument$1.documentElement.scrollTop || parentDocument$1.body.scrollTop;
}
/**
 * @returns {number} - specifies the number value
 */
function getBodyScrollLeft$1() {
    return parentDocument$1.documentElement.scrollLeft || parentDocument$1.body.scrollLeft;
}
/**
 * @returns {number} - specifies the number value
 */
function getElementBottom() {
    return fixedParent ? elementRect.bottom : elementRect.bottom + getBodyScrollTop$1();
}
/**
 * @returns {number} - specifies the number value
 */
function getElementVCenter() {
    return getElementTop() + (elementRect.height / 2);
}
/**
 * @returns {number} - specifies the number value
 */
function getElementTop() {
    return fixedParent ? elementRect.top : elementRect.top + getBodyScrollTop$1();
}
/**
 * @returns {number} - specifies the number value
 */
function getElementLeft() {
    return elementRect.left + getBodyScrollLeft$1();
}
/**
 * @returns {number} - specifies the number value
 */
function getElementRight() {
    var popupWidth = (element && element.classList.contains('e-date-range-wrapper')) ? (popupRect ? popupRect.width : 0) :
        (popupRect && (elementRect.width >= popupRect.width) ? popupRect.width : 0);
    return elementRect.right + getBodyScrollLeft$1() - popupWidth;
}
/**
 * @returns {number} - specifies the number value
 */
function getElementHCenter() {
    return getElementLeft() + (elementRect.width / 2);
}

/**
 * Collision module.
 */
var parentDocument;
var targetContainer;
/**
 *
 * @param {HTMLElement} element - specifies the element
 * @param {HTMLElement} viewPortElement - specifies the element
 * @param {CollisionCoordinates} axis - specifies the collision coordinates
 * @param {OffsetPosition} position - specifies the position
 * @returns {void}
 */
function fit(element, viewPortElement, axis, position) {
    if (viewPortElement === void 0) { viewPortElement = null; }
    if (axis === void 0) { axis = { X: false, Y: false }; }
    if (!axis.Y && !axis.X) {
        return { left: 0, top: 0 };
    }
    var elemData = element.getBoundingClientRect();
    targetContainer = viewPortElement;
    parentDocument = element.ownerDocument;
    if (!position) {
        position = calculatePosition(element, 'left', 'top');
    }
    if (axis.X) {
        var containerWidth = targetContainer ? getTargetContainerWidth() : getViewPortWidth();
        var containerLeft = ContainerLeft();
        var containerRight = ContainerRight();
        var overLeft = containerLeft - position.left;
        var overRight = position.left + elemData.width - containerRight;
        if (elemData.width > containerWidth) {
            if (overLeft > 0 && overRight <= 0) {
                position.left = containerRight - elemData.width;
            }
            else if (overRight > 0 && overLeft <= 0) {
                position.left = containerLeft;
            }
            else {
                position.left = overLeft > overRight ? (containerRight - elemData.width) : containerLeft;
            }
        }
        else if (overLeft > 0) {
            position.left += overLeft;
        }
        else if (overRight > 0) {
            position.left -= overRight;
        }
    }
    if (axis.Y) {
        var containerHeight = targetContainer ? getTargetContainerHeight() : getViewPortHeight();
        var containerTop = ContainerTop();
        var containerBottom = ContainerBottom();
        var overTop = containerTop - position.top;
        var overBottom = position.top + elemData.height - containerBottom;
        if (elemData.height > containerHeight) {
            if (overTop > 0 && overBottom <= 0) {
                position.top = containerBottom - elemData.height;
            }
            else if (overBottom > 0 && overTop <= 0) {
                position.top = containerTop;
            }
            else {
                position.top = overTop > overBottom ? (containerBottom - elemData.height) : containerTop;
            }
        }
        else if (overTop > 0) {
            position.top += overTop;
        }
        else if (overBottom > 0) {
            position.top -= overBottom;
        }
    }
    return position;
}
/**
 *
 * @param {HTMLElement} element - specifies the html element
 * @param {HTMLElement} viewPortElement - specifies the html element
 * @param {number} x - specifies the number
 * @param {number} y - specifies the number
 * @returns {string[]} - returns the string value
 */
function isCollide(element, viewPortElement, x, y) {
    if (viewPortElement === void 0) { viewPortElement = null; }
    var elemOffset = calculatePosition(element, 'left', 'top');
    if (x) {
        elemOffset.left = x;
    }
    if (y) {
        elemOffset.top = y;
    }
    var data = [];
    targetContainer = viewPortElement;
    parentDocument = element.ownerDocument;
    var elementRect = element.getBoundingClientRect();
    var top = elemOffset.top;
    var left = elemOffset.left;
    var right = elemOffset.left + elementRect.width;
    var bottom = elemOffset.top + elementRect.height;
    // eslint-disable-next-line
    var yAxis = topCollideCheck(top, bottom);
    var xAxis = leftCollideCheck(left, right);
    if (yAxis.topSide) {
        data.push('top');
    }
    if (xAxis.rightSide) {
        data.push('right');
    }
    if (xAxis.leftSide) {
        data.push('left');
    }
    if (yAxis.bottomSide) {
        data.push('bottom');
    }
    return data;
}
/**
 *
 * @param {HTMLElement} element - specifies the element
 * @param {HTMLElement} target - specifies the element
 * @param {number} offsetX - specifies the number
 * @param {number} offsetY - specifies the number
 * @param {string} positionX - specifies the string value
 * @param {string} positionY - specifies the string value
 * @param {HTMLElement} viewPortElement - specifies the element
 * @param {CollisionCoordinates} axis - specifies the collision axis
 * @param {boolean} fixedParent - specifies the boolean
 * @returns {void}
 */
function flip(element, target, offsetX, offsetY, positionX, positionY, viewPortElement, 
/* eslint-disable */
axis, fixedParent) {
    if (viewPortElement === void 0) { viewPortElement = null; }
    if (axis === void 0) { axis = { X: true, Y: true }; }
    if (!target || !element || !positionX || !positionY || (!axis.X && !axis.Y)) {
        return;
    }
    // eslint-disable-next-line
    var tEdge = { TL: null,
        TR: null,
        BL: null,
        BR: null
    }, eEdge = {
        TL: null,
        TR: null,
        BL: null,
        BR: null
        /* eslint-enable */
    };
    var elementRect;
    if (window.getComputedStyle(element).display === 'none') {
        var oldVisibility = element.style.visibility;
        element.style.visibility = 'hidden';
        element.style.display = 'block';
        elementRect = element.getBoundingClientRect();
        element.style.removeProperty('display');
        element.style.visibility = oldVisibility;
    }
    else {
        elementRect = element.getBoundingClientRect();
    }
    var pos = {
        posX: positionX, posY: positionY, offsetX: offsetX, offsetY: offsetY, position: { left: 0, top: 0 }
    };
    targetContainer = viewPortElement;
    parentDocument = target.ownerDocument;
    updateElementData(target, tEdge, pos, fixedParent, elementRect);
    setPosition(eEdge, pos, elementRect);
    if (axis.X) {
        leftFlip(target, eEdge, tEdge, pos, elementRect, true);
    }
    if (axis.Y && tEdge.TL.top > -1) {
        topFlip(target, eEdge, tEdge, pos, elementRect, true);
    }
    setPopup(element, pos, elementRect);
}
/**
 *
 * @param {HTMLElement} element - specifies the element
 * @param {PositionLocation} pos - specifies the location
 * @param {ClientRect} elementRect - specifies the client rect
 * @returns {void}
 */
function setPopup(element, pos, elementRect) {
    //eslint-disable-next-line
    var left = 0, top = 0;
    if (element.offsetParent != null
        && (getComputedStyle(element.offsetParent).position === 'absolute' ||
            getComputedStyle(element.offsetParent).position === 'relative')) {
        var data = calculatePosition(element.offsetParent, 'left', 'top', false, elementRect);
        left = data.left;
        top = data.top;
    }
    element.style.top = (pos.position.top + pos.offsetY - (top)) + 'px';
    element.style.left = (pos.position.left + pos.offsetX - (left)) + 'px';
}
/**
 *
 * @param {HTMLElement} target - specifies the element
 * @param {EdgeOffset} edge - specifies the offset
 * @param {PositionLocation} pos - specifies theloaction
 * @param {boolean} fixedParent - specifies the boolean
 * @param {ClientRect} elementRect - specifies the client rect
 * @returns {void}
 */
function updateElementData(target, edge, pos, fixedParent, elementRect) {
    pos.position = calculatePosition(target, pos.posX, pos.posY, fixedParent, elementRect);
    edge.TL = calculatePosition(target, 'left', 'top', fixedParent, elementRect);
    edge.TR = calculatePosition(target, 'right', 'top', fixedParent, elementRect);
    edge.BR = calculatePosition(target, 'left', 'bottom', fixedParent, elementRect);
    edge.BL = calculatePosition(target, 'right', 'bottom', fixedParent, elementRect);
}
/**
 *
 * @param {EdgeOffset} eStatus - specifies the status
 * @param {PositionLocation} pos - specifies the location
 * @param {ClientRect} elementRect - specifies the client
 * @returns {void}
 */
function setPosition(eStatus, pos, elementRect) {
    eStatus.TL = { top: pos.position.top + pos.offsetY, left: pos.position.left + pos.offsetX };
    eStatus.TR = { top: eStatus.TL.top, left: eStatus.TL.left + elementRect.width };
    eStatus.BL = { top: eStatus.TL.top + elementRect.height,
        left: eStatus.TL.left };
    eStatus.BR = { top: eStatus.TL.top + elementRect.height,
        left: eStatus.TL.left + elementRect.width };
}
/**
 *
 * @param {number} left - specifies the  number
 * @param {number} right - specifies the number
 * @returns {LeftCorners} - returns the value
 */
function leftCollideCheck(left, right) {
    //eslint-disable-next-line
    var leftSide = false, rightSide = false;
    if (((left - getBodyScrollLeft()) < ContainerLeft())) {
        leftSide = true;
    }
    if (right > ContainerRight()) {
        rightSide = true;
    }
    return { leftSide: leftSide, rightSide: rightSide };
}
/**
 *
 * @param {HTMLElement} target - specifies the element
 * @param {EdgeOffset} edge - specifes the element
 * @param {EdgeOffset} tEdge - specifies the edge offset
 * @param {PositionLocation} pos - specifes the location
 * @param {ClientRect} elementRect - specifies the client
 * @param {boolean} deepCheck - specifies the boolean value
 * @returns {void}
 */
function leftFlip(target, edge, tEdge, pos, elementRect, deepCheck) {
    var collideSide = leftCollideCheck(edge.TL.left, edge.TR.left);
    if ((tEdge.TL.left - getBodyScrollLeft()) <= ContainerLeft()) {
        collideSide.leftSide = false;
    }
    if (tEdge.TR.left > ContainerRight()) {
        collideSide.rightSide = false;
    }
    if ((collideSide.leftSide && !collideSide.rightSide) || (!collideSide.leftSide && collideSide.rightSide)) {
        if (pos.posX === 'right') {
            pos.posX = 'left';
        }
        else {
            pos.posX = 'right';
        }
        pos.offsetX = pos.offsetX + elementRect.width;
        pos.offsetX = -1 * pos.offsetX;
        pos.position = calculatePosition(target, pos.posX, pos.posY, false);
        setPosition(edge, pos, elementRect);
        if (deepCheck) {
            leftFlip(target, edge, tEdge, pos, elementRect, false);
        }
    }
}
/**
 *
 * @param {HTMLElement} target - specifies the element
 * @param {EdgeOffset} edge - specifies the offset
 * @param {EdgeOffset} tEdge - specifies the offset
 * @param {PositionLocation} pos - specifies the location
 * @param {ClientRect} elementRect - specifies the client rect
 * @param {boolean} deepCheck - specifies the boolean
 * @returns {void}
 */
function topFlip(target, edge, tEdge, pos, elementRect, deepCheck) {
    var collideSide = topCollideCheck(edge.TL.top, edge.BL.top);
    if ((tEdge.TL.top - getBodyScrollTop()) <= ContainerTop()) {
        collideSide.topSide = false;
    }
    if (tEdge.BL.top >= ContainerBottom() && target.getBoundingClientRect().bottom < window.innerHeight) {
        collideSide.bottomSide = false;
    }
    if ((collideSide.topSide && !collideSide.bottomSide) || (!collideSide.topSide && collideSide.bottomSide)) {
        if (pos.posY === 'top') {
            pos.posY = 'bottom';
        }
        else {
            pos.posY = 'top';
        }
        pos.offsetY = pos.offsetY + elementRect.height;
        pos.offsetY = -1 * pos.offsetY;
        pos.position = calculatePosition(target, pos.posX, pos.posY, false, elementRect);
        setPosition(edge, pos, elementRect);
        if (deepCheck) {
            topFlip(target, edge, tEdge, pos, elementRect, false);
        }
    }
}
/**
 *
 * @param {number} top - specifies the number
 * @param {number} bottom - specifies the number
 * @returns {TopCorners} - retyrns the value
 */
function topCollideCheck(top, bottom) {
    //eslint-disable-next-line
    var topSide = false, bottomSide = false;
    if ((top - getBodyScrollTop()) < ContainerTop()) {
        topSide = true;
    }
    if (bottom > ContainerBottom()) {
        bottomSide = true;
    }
    return { topSide: topSide, bottomSide: bottomSide };
}
/**
 * @returns {void}
 */
function getTargetContainerWidth() {
    return targetContainer.getBoundingClientRect().width;
}
/**
 * @returns {void}
 */
function getTargetContainerHeight() {
    return targetContainer.getBoundingClientRect().height;
}
/**
 * @returns {void}
 */
function getTargetContainerLeft() {
    return targetContainer.getBoundingClientRect().left;
}
/**
 * @returns {void}
 */
function getTargetContainerTop() {
    return targetContainer.getBoundingClientRect().top;
}
//eslint-disable-next-line
function ContainerTop() {
    if (targetContainer) {
        return getTargetContainerTop();
    }
    return 0;
}
//eslint-disable-next-line
function ContainerLeft() {
    if (targetContainer) {
        return getTargetContainerLeft();
    }
    return 0;
}
//eslint-disable-next-line
function ContainerRight() {
    if (targetContainer) {
        return (getBodyScrollLeft() + getTargetContainerLeft() + getTargetContainerWidth());
    }
    return (getBodyScrollLeft() + getViewPortWidth());
}
//eslint-disable-next-line
function ContainerBottom() {
    if (targetContainer) {
        return (getBodyScrollTop() + getTargetContainerTop() + getTargetContainerHeight());
    }
    return (getBodyScrollTop() + getViewPortHeight());
}
/**
 * @returns {void}
 */
function getBodyScrollTop() {
    // if(targetContainer)
    //     return targetContainer.scrollTop;
    return parentDocument.documentElement.scrollTop || parentDocument.body.scrollTop;
}
/**
 * @returns {void}
 */
function getBodyScrollLeft() {
    // if(targetContainer)
    //     return targetContainer.scrollLeft;
    return parentDocument.documentElement.scrollLeft || parentDocument.body.scrollLeft;
}
/**
 * @returns {void}
 */
function getViewPortHeight() {
    return window.innerHeight;
}
/**
 * @returns {void}
 */
function getViewPortWidth() {
    var windowWidth = window.innerWidth;
    var documentReact = document.documentElement.getBoundingClientRect();
    var offsetWidth = (sf.base.isNullOrUndefined(document.documentElement)) ? 0 : documentReact.width;
    return windowWidth - (windowWidth - offsetWidth);
}

/**
 * Resize library
 */
/* eslint-disable */
var elementClass = ['north-west', 'north', 'north-east', 'west', 'east', 'south-west', 'south', 'south-east'];
var RESIZE_HANDLER = 'e-resize-handle';
var FOCUSED_HANDLER = 'e-focused-handle';
var RESTRICT_LEFT = ['e-restrict-left'];
var RESIZE_WITHIN_VIEWPORT = 'e-resize-viewport';
var dialogBorderResize = ['north', 'west', 'east', 'south'];
var targetElement;
var selectedHandler;
var originalWidth = 0;
var originalHeight = 0;
var originalX = 0;
var originalY = 0;
var originalMouseX = 0;
var originalMouseY = 0;
var minHeight;
var maxHeight;
var minWidth;
var maxWidth;
var containerElement;
/* eslint-disable */
var resizeStart = null;
var resize = null;
var resizeEnd = null;
/* eslint-enable */
var resizeWestWidth;
var setLeft = true;
var previousWidth = 0;
var setWidth = true;
// eslint-disable-next-line
var proxy;
/**
 *
 * @param {ResizeArgs} args - specifies the resize args
 * @returns {void}
 */
function createResize(args) {
    resizeStart = args.resizeBegin;
    resize = args.resizing;
    resizeEnd = args.resizeComplete;
    targetElement = getDOMElement(args.element);
    containerElement = getDOMElement(args.boundary);
    var directions = args.direction.split(' ');
    for (var i = 0; i < directions.length; i++) {
        if (dialogBorderResize.indexOf(directions[i]) >= 0 && directions[i]) {
            setBorderResizeElm(directions[i]);
        }
        else if (directions[i].trim() !== '') {
            var resizeHandler = sf.base.createElement('div', { className: 'e-icons ' + RESIZE_HANDLER + ' ' + 'e-' + directions[i] });
            targetElement.appendChild(resizeHandler);
        }
    }
    minHeight = args.minHeight;
    minWidth = args.minWidth;
    maxWidth = args.maxWidth;
    maxHeight = args.maxHeight;
    if (args.proxy && args.proxy.element && args.proxy.element.classList.contains('e-dialog')) {
        wireEvents(args.proxy);
    }
    else {
        wireEvents();
    }
}
/**
 *
 * @param {string} direction - specifies the string
 * @returns {void}
 */
function setBorderResizeElm(direction) {
    calculateValues();
    var borderBottom = sf.base.createElement('span', {
        attrs: {
            'unselectable': 'on', 'contenteditable': 'false'
        }
    });
    borderBottom.setAttribute('class', 'e-dialog-border-resize e-' + direction);
    if (direction === 'south') {
        borderBottom.style.height = '2px';
        borderBottom.style.width = '100%';
        borderBottom.style.bottom = '0px';
        borderBottom.style.left = '0px';
    }
    if (direction === 'north') {
        borderBottom.style.height = '2px';
        borderBottom.style.width = '100%';
        borderBottom.style.top = '0px';
        borderBottom.style.left = '0px';
    }
    if (direction === 'east') {
        borderBottom.style.height = '100%';
        borderBottom.style.width = '2px';
        borderBottom.style.right = '0px';
        borderBottom.style.top = '0px';
    }
    if (direction === 'west') {
        borderBottom.style.height = '100%';
        borderBottom.style.width = '2px';
        borderBottom.style.left = '0px';
        borderBottom.style.top = '0px';
    }
    targetElement.appendChild(borderBottom);
}
/**
 *
 * @param {string} element - specifies the element
 * @returns {HTMLElement} - returns the element
 */
function getDOMElement(element) {
    var domElement;
    if (!sf.base.isNullOrUndefined(element)) {
        if (typeof (element) === 'string') {
            domElement = document.querySelector(element);
        }
        else {
            domElement = element;
        }
    }
    return domElement;
}
// eslint-disable-next-line
function wireEvents(args) {
    if (sf.base.isNullOrUndefined(args)) {
        args = this;
    }
    var resizers = targetElement.querySelectorAll('.' + RESIZE_HANDLER);
    for (var i = 0; i < resizers.length; i++) {
        selectedHandler = resizers[i];
        sf.base.EventHandler.add(selectedHandler, 'mousedown', onMouseDown, args);
        var eventName = (sf.base.Browser.info.name === 'msie') ? 'pointerdown' : 'touchstart';
        sf.base.EventHandler.add(selectedHandler, eventName, onTouchStart, args);
    }
    var borderResizers = targetElement.querySelectorAll('.e-dialog-border-resize');
    if (!sf.base.isNullOrUndefined(borderResizers)) {
        for (var i = 0; i < borderResizers.length; i++) {
            selectedHandler = borderResizers[i];
            sf.base.EventHandler.add(selectedHandler, 'mousedown', onMouseDown, args);
            var eventName = (sf.base.Browser.info.name === 'msie') ? 'pointerdown' : 'touchstart';
            sf.base.EventHandler.add(selectedHandler, eventName, onTouchStart, args);
        }
    }
}
/* istanbul ignore next */
/**
 *
 * @param {string} e - specifies the string
 * @returns {string} - returns the string
 */
function getEventType(e) {
    return (e.indexOf('mouse') > -1) ? 'mouse' : 'touch';
}
/* istanbul ignore next */
/**
 *
 * @param {MouseEvent} e - specifies the mouse event
 * @returns {void}
 */
function onMouseDown(e) {
    e.preventDefault();
    targetElement = e.target.parentElement;
    calculateValues();
    originalMouseX = e.pageX;
    originalMouseY = e.pageY;
    e.target.classList.add(FOCUSED_HANDLER);
    if (!sf.base.isNullOrUndefined(resizeStart)) {
        proxy = this;
        if (resizeStart(e, proxy) === true) {
            return;
        }
    }
    var target = (sf.base.isNullOrUndefined(containerElement)) ? document : containerElement;
    sf.base.EventHandler.add(target, 'mousemove', onMouseMove, this);
    sf.base.EventHandler.add(document, 'mouseup', onMouseUp, this);
    for (var i = 0; i < RESTRICT_LEFT.length; i++) {
        if (targetElement.classList.contains(RESTRICT_LEFT[i])) {
            setLeft = false;
        }
        else {
            setLeft = true;
        }
    }
}
/* istanbul ignore next */
/**
 *
 * @param {MouseEvent} e - specifies the event
 * @returns {void}
 */
function onMouseUp(e) {
    var touchMoveEvent = (sf.base.Browser.info.name === 'msie') ? 'pointermove' : 'touchmove';
    var touchEndEvent = (sf.base.Browser.info.name === 'msie') ? 'pointerup' : 'touchend';
    var target = (sf.base.isNullOrUndefined(containerElement)) ? document : containerElement;
    var eventName = (sf.base.Browser.info.name === 'msie') ? 'pointerdown' : 'touchstart';
    sf.base.EventHandler.remove(target, 'mousemove', onMouseMove);
    sf.base.EventHandler.remove(target, touchMoveEvent, onMouseMove);
    sf.base.EventHandler.remove(target, eventName, onMouseMove);
    if (!sf.base.isNullOrUndefined(document.body.querySelector('.' + FOCUSED_HANDLER))) {
        document.body.querySelector('.' + FOCUSED_HANDLER).classList.remove(FOCUSED_HANDLER);
    }
    if (!sf.base.isNullOrUndefined(resizeEnd)) {
        proxy = this;
        resizeEnd(e, proxy);
    }
    sf.base.EventHandler.remove(document, 'mouseup', onMouseUp);
    sf.base.EventHandler.remove(document, touchEndEvent, onMouseUp);
}
/* istanbul ignore next */
/**
 * @returns {void}
 */
function calculateValues() {
    originalWidth = parseFloat(getComputedStyle(targetElement, null).getPropertyValue('width').replace('px', ''));
    originalHeight = parseFloat(getComputedStyle(targetElement, null).getPropertyValue('height').replace('px', ''));
    originalX = targetElement.getBoundingClientRect().left;
    originalY = targetElement.getBoundingClientRect().top;
}
/* istanbul ignore next */
/**
 *
 * @param {MouseEvent} e - specifies the event
 * @returns {void}
 */
function onTouchStart(e) {
    targetElement = e.target.parentElement;
    calculateValues();
    var coordinates = e.touches ? e.changedTouches[0] : e;
    originalMouseX = coordinates.pageX;
    originalMouseY = coordinates.pageY;
    if (!sf.base.isNullOrUndefined(resizeStart)) {
        proxy = this;
        if (resizeStart(e, proxy) === true) {
            return;
        }
    }
    var touchMoveEvent = (sf.base.Browser.info.name === 'msie') ? 'pointermove' : 'touchmove';
    var touchEndEvent = (sf.base.Browser.info.name === 'msie') ? 'pointerup' : 'touchend';
    var target = (sf.base.isNullOrUndefined(containerElement)) ? document : containerElement;
    sf.base.EventHandler.add(target, touchMoveEvent, onMouseMove, this);
    sf.base.EventHandler.add(document, touchEndEvent, onMouseUp);
}
/* istanbul ignore next */
/**
 *
 * @param {MouseEvent} e - specifies the event
 * @returns {void}
 */
function onMouseMove(e) {
    if (e.target.classList.contains(RESIZE_HANDLER) && e.target.classList.contains(FOCUSED_HANDLER)) {
        selectedHandler = e.target;
    }
    else if (!sf.base.isNullOrUndefined(document.body.querySelector('.' + FOCUSED_HANDLER))) {
        selectedHandler = document.body.querySelector('.' + FOCUSED_HANDLER);
    }
    if (!sf.base.isNullOrUndefined(selectedHandler)) {
        var resizeTowards = '';
        for (var i = 0; i < elementClass.length; i++) {
            if (selectedHandler.classList.contains('e-' + elementClass[i])) {
                resizeTowards = elementClass[i];
            }
        }
        if (!sf.base.isNullOrUndefined(resize)) {
            proxy = this;
            resize(e, proxy);
        }
        switch (resizeTowards) {
            case 'south':
                resizeSouth(e);
                break;
            case 'north':
                resizeNorth(e);
                break;
            case 'west':
                resizeWest(e);
                break;
            case 'east':
                resizeEast(e);
                break;
            case 'south-east':
                resizeSouth(e);
                resizeEast(e);
                break;
            case 'south-west':
                resizeSouth(e);
                resizeWest(e);
                break;
            case 'north-east':
                resizeNorth(e);
                resizeEast(e);
                break;
            case 'north-west':
                resizeNorth(e);
                resizeWest(e);
                break;
            default: break;
        }
    }
}
/* istanbul ignore next */
/**
 *
 * @param {HTMLElement} element - specifies the eleemnt
 * @returns {ClientRect} - returns the client
 */
function getClientRectValues(element) {
    return element.getBoundingClientRect();
}
/* istanbul ignore next */
// eslint-disable-next-line
function resizeSouth(e) {
    var documentHeight = document.documentElement.clientHeight;
    var calculateValue = false;
    var coordinates = e.touches ? e.changedTouches[0] : e;
    var currentpageY = coordinates.pageY;
    var targetRectValues = getClientRectValues(targetElement);
    var containerRectValues;
    if (!sf.base.isNullOrUndefined(containerElement)) {
        containerRectValues = getClientRectValues(containerElement);
    }
    if (!sf.base.isNullOrUndefined(containerElement)) {
        calculateValue = true;
    }
    else if (sf.base.isNullOrUndefined(containerElement) && ((documentHeight - currentpageY) >= 0 || (targetRectValues.top < 0))) {
        calculateValue = true;
    }
    var calculatedHeight = originalHeight + (currentpageY - originalMouseY);
    calculatedHeight = (calculatedHeight > minHeight) ? calculatedHeight : minHeight;
    var containerTop = 0;
    if (!sf.base.isNullOrUndefined(containerElement)) {
        containerTop = containerRectValues.top;
    }
    var borderValue = sf.base.isNullOrUndefined(containerElement) ? 0 : containerElement.offsetHeight - containerElement.clientHeight;
    var topWithoutborder = (targetRectValues.top - containerTop) - (borderValue / 2);
    topWithoutborder = (topWithoutborder < 0) ? 0 : topWithoutborder;
    if (targetRectValues.top > 0 && (topWithoutborder + calculatedHeight) > maxHeight) {
        calculateValue = false;
        if (targetElement.classList.contains(RESIZE_WITHIN_VIEWPORT)) {
            return;
        }
        targetElement.style.height = (maxHeight - parseInt(topWithoutborder.toString(), 10)) + 'px';
        return;
    }
    var targetTop = 0;
    if (calculateValue) {
        if (targetRectValues.top < 0 && (documentHeight + (targetRectValues.height + targetRectValues.top) > 0)) {
            targetTop = targetRectValues.top;
            if ((calculatedHeight + targetTop) <= 30) {
                calculatedHeight = (targetRectValues.height - (targetRectValues.height + targetRectValues.top)) + 30;
            }
        }
        if (((calculatedHeight + targetRectValues.top) >= maxHeight)) {
            targetElement.style.height = targetRectValues.height +
                (documentHeight - (targetRectValues.height + targetRectValues.top)) + 'px';
        }
        var calculatedTop = (sf.base.isNullOrUndefined(containerElement)) ? targetTop : topWithoutborder;
        if (calculatedHeight >= minHeight && ((calculatedHeight + calculatedTop) <= maxHeight)) {
            targetElement.style.height = calculatedHeight + 'px';
        }
    }
}
/* istanbul ignore next */
// eslint-disable-next-line
function resizeNorth(e) {
    var calculateValue = false;
    var boundaryRectValues;
    var pageY = (getEventType(e.type) === 'mouse') ? e.pageY : e.touches[0].pageY;
    var targetRectValues = getClientRectValues(targetElement);
    if (!sf.base.isNullOrUndefined(containerElement)) {
        boundaryRectValues = getClientRectValues(containerElement);
    }
    if (!sf.base.isNullOrUndefined(containerElement) && (targetRectValues.top - boundaryRectValues.top) > 0) {
        calculateValue = true;
    }
    else if (sf.base.isNullOrUndefined(containerElement) && pageY > 0) {
        calculateValue = true;
    }
    var currentHeight = originalHeight - (pageY - originalMouseY);
    if (calculateValue) {
        if (currentHeight >= minHeight && currentHeight <= maxHeight) {
            var containerTop = 0;
            if (!sf.base.isNullOrUndefined(containerElement)) {
                containerTop = boundaryRectValues.top;
            }
            var top_1 = (originalY - containerTop) + (pageY - originalMouseY);
            top_1 = top_1 > 0 ? top_1 : 1;
            targetElement.style.height = currentHeight + 'px';
            targetElement.style.top = top_1 + 'px';
        }
    }
}
/* istanbul ignore next */
// eslint-disable-next-line
function resizeWest(e) {
    var documentWidth = document.documentElement.clientWidth;
    var calculateValue = false;
    var rectValues;
    if (!sf.base.isNullOrUndefined(containerElement)) {
        rectValues = getClientRectValues(containerElement);
    }
    var pageX = (getEventType(e.type) === 'mouse') ? e.pageX : e.touches[0].pageX;
    var targetRectValues = getClientRectValues(targetElement);
    var borderValue = sf.base.isNullOrUndefined(containerElement) ? 0 : containerElement.offsetWidth - containerElement.clientWidth;
    /* eslint-disable */
    var left = sf.base.isNullOrUndefined(containerElement) ? 0 : rectValues.left;
    var containerWidth = sf.base.isNullOrUndefined(containerElement) ? 0 : rectValues.width;
    /* eslint-enable */
    if (sf.base.isNullOrUndefined(resizeWestWidth)) {
        if (!sf.base.isNullOrUndefined(containerElement)) {
            resizeWestWidth = (((targetRectValues.left - left) - borderValue / 2)) + targetRectValues.width;
            resizeWestWidth = resizeWestWidth + (containerWidth - borderValue - resizeWestWidth);
        }
        else {
            resizeWestWidth = documentWidth;
        }
    }
    if (!sf.base.isNullOrUndefined(containerElement) &&
        (Math.floor((targetRectValues.left - rectValues.left) + targetRectValues.width +
            (rectValues.right - targetRectValues.right)) - borderValue) <= maxWidth) {
        calculateValue = true;
    }
    else if (sf.base.isNullOrUndefined(containerElement) && pageX >= 0) {
        calculateValue = true;
    }
    var calculatedWidth = originalWidth - (pageX - originalMouseX);
    if (setLeft) {
        calculatedWidth = (calculatedWidth > resizeWestWidth) ? resizeWestWidth : calculatedWidth;
    }
    if (calculateValue) {
        if (calculatedWidth >= minWidth && calculatedWidth <= maxWidth) {
            var containerLeft = 0;
            if (!sf.base.isNullOrUndefined(containerElement)) {
                containerLeft = rectValues.left;
            }
            var left_1 = (originalX - containerLeft) + (pageX - originalMouseX);
            left_1 = (left_1 > 0) ? left_1 : 1;
            if (calculatedWidth !== previousWidth && setWidth) {
                targetElement.style.width = calculatedWidth + 'px';
            }
            if (setLeft) {
                targetElement.style.left = left_1 + 'px';
                if (left_1 === 1) {
                    setWidth = false;
                }
                else {
                    setWidth = true;
                }
            }
        }
    }
    previousWidth = calculatedWidth;
}
/* istanbul ignore next */
// eslint-disable-next-line
function resizeEast(e) {
    var documentWidth = document.documentElement.clientWidth;
    var calculateValue = false;
    var containerRectValues;
    if (!sf.base.isNullOrUndefined(containerElement)) {
        containerRectValues = getClientRectValues(containerElement);
    }
    var coordinates = e.touches ? e.changedTouches[0] : e;
    var pageX = coordinates.pageX;
    var targetRectValues = getClientRectValues(targetElement);
    if (!sf.base.isNullOrUndefined(containerElement) && (((targetRectValues.left - containerRectValues.left) + targetRectValues.width) <= maxWidth
        || (targetRectValues.right - containerRectValues.left) >= targetRectValues.width)) {
        calculateValue = true;
    }
    else if (sf.base.isNullOrUndefined(containerElement) && (documentWidth - pageX) > 0) {
        calculateValue = true;
    }
    var calculatedWidth = originalWidth + (pageX - originalMouseX);
    var containerLeft = 0;
    if (!sf.base.isNullOrUndefined(containerElement)) {
        containerLeft = containerRectValues.left;
    }
    if (((targetRectValues.left - containerLeft) + calculatedWidth) > maxWidth) {
        calculateValue = false;
        if (targetElement.classList.contains(RESIZE_WITHIN_VIEWPORT)) {
            return;
        }
        targetElement.style.width = maxWidth - (targetRectValues.left - containerLeft) + 'px';
    }
    if (calculateValue) {
        if (calculatedWidth >= minWidth && calculatedWidth <= maxWidth) {
            targetElement.style.width = calculatedWidth + 'px';
        }
    }
}
/* istanbul ignore next */
/**
 *
 * @param {number} minimumHeight - specifies the number
 * @returns {void}
 */
function setMinHeight(minimumHeight) {
    minHeight = minimumHeight;
}
/**
 *
 * @param {number} value - specifies the number value
 * @returns {void}
 */
function setMaxWidth(value) {
    maxWidth = value;
}
/**
 *
 * @param {number} value - specifies the number value
 * @returns {void}
 */
function setMaxHeight(value) {
    maxHeight = value;
}
/**
 * @returns {void}
 */
function removeResize() {
    var handlers = targetElement.querySelectorAll('.' + RESIZE_HANDLER);
    for (var i = 0; i < handlers.length; i++) {
        sf.base.detach(handlers[i]);
    }
    var borderResizers = targetElement.querySelectorAll('.e-dialog-border-resize');
    if (!sf.base.isNullOrUndefined(borderResizers)) {
        for (var i = 0; i < borderResizers.length; i++) {
            sf.base.detach(borderResizers[i]);
        }
    }
}

exports.fit = fit;
exports.isCollide = isCollide;
exports.flip = flip;
exports.calculateRelativeBasedPosition = calculateRelativeBasedPosition;
exports.calculatePosition = calculatePosition;
exports.createResize = createResize;
exports.setMinHeight = setMinHeight;
exports.setMaxWidth = setMaxWidth;
exports.setMaxHeight = setMaxHeight;
exports.removeResize = removeResize;

return exports;

});
window.sf.popups = window.sf.base.extend({}, window.sf.popups, popupsbase({}));

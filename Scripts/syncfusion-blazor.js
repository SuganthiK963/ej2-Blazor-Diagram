var sfBlazorBase = {
    getElementByXpath: function (xPath) {
        return document.evaluate(xPath, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
    },
    getElement: function (elementID, id, xPath) {
        var dom = (elementID != null && window[elementID] != null) ? window[elementID][id] : null;
        return (dom != null ? dom : window.sfBlazor.getElementByXpath(xPath));
    },
    getAttribute: function (elementID, dom, xPath, propName) {
        var element = window.sfBlazor.getElement(elementID, dom, xPath);
        if (element != null)
            return element.getAttribute(propName);
    },
    setAttribute: function (elementID, dom, xPath, propName, value) {
        (window.sfBlazor.getElement(elementID, dom, xPath)).setAttribute(propName, value);
    },
    addClass: function (elementID, dom, xPath, classList) {
        sf.base.addClass([window.sfBlazor.getElement(elementID, dom, xPath)], classList);
    },
    removeClass: function (elementID, dom, xPath, classList) {
        sf.base.removeClass([window.sfBlazor.getElement(elementID, dom, xPath)], classList);
    },
    getClassList: function (elementID, dom, xPath) {
        return Array.prototype.slice.call((window.sfBlazor.getElement(elementID, dom, xPath)).classList);
    },
    enableRipple: function (isRipple) {
        sf.base.enableRipple(isRipple);
    },
    isDevice: function (isRtl) {
        if (isRtl) {
           this.enableRtl(isRtl);
        }
        return sf.base.Browser.isDevice;
    },
    animate: function (elementRef, animationSettings) {
        var animationObj = new sf.base.Animation(animationSettings);
        animationObj.animate(elementRef);
    },
    callRipple: function (elementRef, rippleSettings) {
        sf.base.rippleEffect(elementRef, rippleSettings);
    },
    createXPathFromElement: function (elm) {
        var allNodes = document.getElementsByTagName('*');
        for (var segs = []; elm && elm.nodeType === 1; elm = elm.parentNode) {
            if (elm.hasAttribute('id')) {
                var uniqueIdCount = 0;
                for (var n = 0; n < allNodes.length; n++) {
                    if (allNodes[n].hasAttribute('id') && allNodes[n].id === elm.id) uniqueIdCount++;
                    if (uniqueIdCount > 1) break;
                };
                if (uniqueIdCount === 1) {
                    segs.unshift('id("' + elm.getAttribute('id') + '")');
                    return segs.join('/');
                } else {
                    segs.unshift(elm.localName.toLowerCase() + '[@id="' + elm.getAttribute('id') + '"]');
                }
            } else {
                for (var i = 1, sib = elm.previousSibling; sib; sib = sib.previousSibling) {
                    if (sib.localName === elm.localName) i++;
                }
                segs.unshift(elm.localName.toLowerCase() + '[' + i + ']');
            }
        }
        return segs.length ? '/' + segs.join('/') : null;
    },
    getDomObject: function (key, node, object) {
        var uuid = key + sf.base.getUniqueID(key);
        var domObject = {
            id: node.id,
            class: node.className,
            xPath: window.sfBlazor.createXPathFromElement(node),
            domUUID: uuid
        };
        var elementID = object && object["elementID"];
        if (elementID) {
            window[elementID] = sf.base.isNullOrUndefined(window[elementID]) ? {} : window[elementID];
            window[elementID][uuid] = node;
            domObject["elementID"] = elementID;
        }
        return domObject;
    },
    focusButton: function (element) {
        element.focus();
    },
    //sf-progressbutton interop start
    setProgress: function (progressElem, contElem, spinnerElem, percent, enableProgress, isVertical) {
		spinnerElem = spinnerElem.querySelector('.e-spinner');
        return window.requestAnimationFrame(function () {
            if (enableProgress) {
                progressElem.style[isVertical ? 'height' : 'width'] = percent + '%';
            }
            contElem.parentElement.setAttribute('aria-valuenow', percent.toString());
            if (percent === 100) {
                contElem.classList.remove('e-cont-animate', 'e-animate-end');
                spinnerElem.style.width = 'auto';
                spinnerElem.style.height = 'auto';
            }
        });
    },
    setAnimation: function (contElem, spinnerElem, effect, duration, easing, isCenter) {
		spinnerElem = spinnerElem.querySelector('.e-spinner');
        new sf.base.Animation({}).animate(contElem, {
            duration: duration,
            name: 'Progress' + effect,
            timingFunction: easing,
            begin: function () {
                if (isCenter) {
                    spinnerElem.style.width = Math.max(spinnerElem.offsetWidth, contElem.offsetWidth) + 'px';
                    spinnerElem.style.height = Math.max(spinnerElem.offsetHeight, contElem.offsetHeight) + 'px';
                    contElem.classList.add('e-cont-animate');
                }
            },
            end: function () {
                contElem.classList.add('e-animate-end');
            }
        });
    },
    cancelAnimation: function(timerId) {
        window.cancelAnimationFrame(timerId);
    }
    //sf-progressbutton interop end
};
(function () {
    sf.base.enableBlazorMode();
})();

window.sfBlazor = window.sfBlazor || {};
Object.assign(window.sfBlazor, sfBlazorBase);

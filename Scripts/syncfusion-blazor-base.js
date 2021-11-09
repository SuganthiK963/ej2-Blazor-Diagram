window.sfBlazor = {
    getCharCollectionSize: function (fontKeys) {
        var charSizeList = [], charList = this.getAllCharacters(), charLength = charList.length, fontKeysLength = fontKeys.length;
        for (var keyIndex = 0; keyIndex < fontKeysLength; keyIndex++) {
            var fontValues = fontKeys[keyIndex].split('_'), fontWeight = fontValues[0], fontStyle = fontValues[1], fontFamily = fontValues[2], tempList = {};
            for (var charIndex = 0; charIndex < charLength; charIndex++) {
                tempList[charList[charIndex]] = this.measureText(charList[charIndex], fontWeight, fontStyle, fontFamily);
            }
            charSizeList.push(tempList);
        }
        return JSON.stringify(charSizeList);
    },
    getCharSizeByFontKeys: function (fontkeys) {
        var charSizeList = {}, fontValues = [], fontKeysLength = fontkeys.length;
        for (var i = 0; i < fontKeysLength; i++) {
            fontValues = fontkeys[i].split('_');
            charSizeList[fontkeys[i]] = this.measureText(fontValues[0], fontValues[1], fontValues[2], fontValues[3]);
        }
        var result = JSON.stringify(charSizeList);
        return result;
    },
    getElementBoundsById: function (id) {
        var element = document.getElementById(id);
        var svgelement = document.getElementById(id + "_svg");
        var avl_Width;
        var avl_Height;
        if (element) {
            if (svgelement) {
               svgelement.style.display = "none";
            }
            element.style.width = '100%';
            element.style.height = '100%';
            var elementRect = element.getBoundingClientRect();
            avl_Width = element.clientWidth || element.offsetWidth;
            avl_Height = element.clientHeight || element.offsetHeight;
            if (svgelement) {
               svgelement.style.display = "";
            }
            return {
                width: avl_Width, height: avl_Height,
                left: elementRect.left, top: elementRect.top, right: elementRect.right, bottom: elementRect.bottom
            };
        }
        return { width: 0, height: 0, left: 0, top: 0, right: 0, bottom: 0 };
    },
    charCollection: [],
    getAllCharacters: function () {
        if (!this.charCollection.length) {
            var allCharCollection = [];
            for (var charIndex = 33; charIndex < 591; charIndex++) {
                allCharCollection.push(String.fromCharCode(charIndex));
            }
            this.charCollection = allCharCollection;
        }
        return this.charCollection;
    },
    measureText: function (text, fontWeight, fontStyle, fontFamily) {
        var textObject = document.getElementById('sfblazor_measuretext');
        if (textObject === null) {
            textObject = document.createElement('text');
            textObject.id = 'sfblazor_measuretext';
            document.body.appendChild(textObject);
        }
        if (text === ' ') {
            text = '&nbsp;';
        }
        textObject.innerHTML = text;
        textObject.style.position = 'fixed';
        textObject.style.fontSize = '100px';
        textObject.style.fontWeight = fontWeight;
        textObject.style.fontStyle = fontStyle;
        textObject.style.fontFamily = fontFamily;
        textObject.style.visibility = 'hidden';
        textObject.style.top = '-100';
        textObject.style.left = '0';
        textObject.style.whiteSpace = 'nowrap';
        textObject.style.lineHeight = 'normal';
        return {
            X: textObject.clientWidth,
            Y: textObject.clientHeight
        };
    },
	setSvgDimensions: function (chartSVG, width, height) {
      chartSVG.setAttribute("width", width);
      chartSVG.setAttribute("height", height);
    }
}
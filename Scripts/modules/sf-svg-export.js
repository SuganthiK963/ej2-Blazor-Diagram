window.sfExport = {
    exportToImage: async function (type, fileName, elementId, allowDownload) {
        var returnValue = await window.sfExport.imageExport(type, fileName, elementId, allowDownload);
        if (returnValue instanceof Promise) {
            await returnValue.then(async function (data) {
                return data;
            });
        } else {
            return returnValue;
        }
    },
    validateExport: async function (returnValue) {
        if (returnValue instanceof Promise) {
            await returnValue.then(async function (data) {
                return data;
            });
        }
    },
    imageExport: async function (type, fileName, elementId, allowDownload) {
        return new Promise(function (resolve, reject) {
        var element = document.getElementById(elementId);
        var svgData = '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">' + element.outerHTML + '</svg>';
        var url = window.URL.createObjectURL(new Blob(type === 'SVG' ? [svgData] : [(new XMLSerializer()).serializeToString(element)], { type: 'image/svg+xml' }));
        if (type === 'SVG') {
            if (allowDownload) {
                window.sfExport.triggerDownload(type, fileName, url);
            }
            resolve(null);
        } else {
            var canvasElement = document.createElement('canvas');
            canvasElement.height = element.clientHeight;
            canvasElement.width = element.clientWidth;
            var context = canvasElement.getContext('2d');
            var image = new Image();
            image.onload = function () {
                context.drawImage(image, 0, 0);
                window.URL.revokeObjectURL(url);
                if (allowDownload) {
                    window.sfExport.triggerDownload(type, fileName, canvasElement.toDataURL('image/png').replace('image/png', 'image/octet-stream'));
                    resolve(null);
                } else {
                    var base64String = (type === 'JPEG') ? canvasElement.toDataURL('image/jpeg') : (type === 'PNG') ? canvasElement.toDataURL('image/png') : '';
                    resolve(base64String);
                }
            };
            image.src = url;
        }
    });
    },
    triggerDownload : function(type, fileName, url)  {
        var anchorElement = document.createElement('a');
        anchorElement.download = fileName + '.' + type.toLocaleLowerCase();
        anchorElement.href = url;
        anchorElement.click();
    },
    downloadPdf: function (base64String, fileName) {
        var sliceSize = 512;
        var byteCharacters = atob(base64String);
        var byteArrays = [];
        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);
            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }
        var blob = new Blob(byteArrays, { type: 'application/pdf' });
        var Url = URL || webkitURL;
        var blobUrl = Url.createObjectURL(blob);
        sfExport.triggerDownload("PDF", fileName, blobUrl);
    },
    print: function (printElement) {
        var printWindow;
        printWindow = window.open('', 'print', 'height=' + window.outerHeight + ',width=' + window.outerWidth + ',tabbar=no');
        printWindow.moveTo(0, 0);
        printWindow.resizeTo(screen.availWidth, screen.availHeight);
        var div = document.createElement('div');
        div.appendChild(printElement.cloneNode(true));
        printWindow.document.write('<!DOCTYPE html> <html><head> </head><body>' + div.innerHTML +
            '<script> (function() { window.ready = true; })(); </script>' + '</body></html>');
        printWindow.document.close();
        printWindow.focus();
        var interval = setInterval(function () {
            if (printWindow.ready) {
                printWindow.print();
                printWindow.close();
                clearInterval(interval);
            }
        }, 500);
    }
};
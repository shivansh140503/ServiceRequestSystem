// Use addEventListener instead of window.onload as that will be overwritten by default swagger ui index.html
window.addEventListener("load", function () {
    // Use setTimeout to wait until window.onload has executed
    setTimeout(function () {
        window.ui.getConfigs().syntaxHighlight.activated = false;
    }, 0);
});
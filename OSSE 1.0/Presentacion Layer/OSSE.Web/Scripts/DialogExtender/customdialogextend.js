var dialogIndex = 999999;
var minimizedElements = 0;
var minimizedDialogWidth = 200;

function repositionDialogs() {
    var count = 0;
    jQuery('.ui-dialog.mini').each(function(i, element) {
        var positionLeft = i * minimizedDialogWidth;
        
        $(element).css('left', positionLeft).addClass('ddddddddd');
    });
}

//jQuery.ui.dialog.prototype._moveToTop = function (e, silent) {
//    if (e) {
//        dialogIndex++;
//        $(e.currentTarget).css('z-index', dialogIndex);
//    }
//    if (!silent) {
//        this._trigger("focus", event);
//    }
//    return true;
//};

jQuery.ui.dialogExtend.prototype.minimize = function () {
    if (this._state == 'maximized') {
        this._restore();
    }

    this._trigger("beforeMinimize");
    this._saveSnapshot();
    var positionLeft = minimizedElements * minimizedDialogWidth;
    $(this.element[0]).dialog("widget").addClass('mini');
  
    repositionDialogs();

    this._setState("minimized");
    this._toggleButtons();
    return this._trigger("minimize");
};

jQuery.ui.dialogExtend.prototype._restore_minimized = function () {
    var element = $(this.element[0]).dialog("widget");
    var original = this._loadSnapshot();
    element.css({
            "float": "none",
            "margin": 0,
            "position": original.position.mode
        }).removeClass('mini')
        .find(".ui-dialog-content").dialog("option", {
            "resizable": original.config.resizable,
            "draggable": original.config.draggable,
            "height": original.size.height,
            "width": original.size.width,
            "maxHeight": original.size.maxHeight,
            "position": {
                my: "left top",
                at: "left+" + original.position.left + " top+" + original.position.top
            }
        });
    
    repositionDialogs();
    return element;
};

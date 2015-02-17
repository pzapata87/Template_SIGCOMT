jQuery.fn.dataTable.Api.register('fnFilterOnReturn()', function () {
    var that = this;

    this.tables().nodes().each(function (value, i) {
        var anControl = $('input', that.settings()[i].aanFeatures.f);
        anControl
            .unbind('keyup search input')
            .bind('keypress', function (e) {
                if (e.which == 13) {
                    that.search(anControl.val()).draw();
                }
            });
        return this;
    });
    return this;
});
function MakeValidationSummaryRealTime(form)
{
    var validator = $(form).data('validator');
    if (validator) {
        validator.settings.showErrors = function (map, errors) {
            this.defaultShowErrors();
            this.checkForm();
            if (this.errorList.length)
                $(this.currentForm).triggerHandler("invalid-form", [this]);
            else
                $(this.currentForm).resetSummary();
        };
    }

    jQuery.fn.resetSummary = function () {
        var form = this.is('form') ? this : this.closest('form');
        form.find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul")
            .empty();
        return this;
    };
}
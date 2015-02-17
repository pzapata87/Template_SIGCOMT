$(function() {
    if ($.fn.dataTable != null) {
        $.extend($.fn.dataTable.defaults, {
            language: {
                url: "/Content/Admin/ajax/jquery.dataTables.es.txt"
            },
            responsive: true,
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Todo"]],
            "bProcessing": true,
            "dom": 'fltip'
        });
    }

    registrarCSSError();
});

function registrarCSSError() {
    $('form').submit(function() {
        if ($(this).valid()) {
            $(this).find('div.input-group').each(function() {
                if ($(this).find('.input-validation-error').length == 0) {
                    $(this).removeClass('has-error');
                }
            });
        } else {
            $(this).find('div.input-group').each(function() {
                if ($(this).find('.input-validation-error').length > 0) {
                    $(this).addClass('has-error');
                }
            });
        }
    });

    $('form').each(function() {
        $(this).find('div.input-group').each(function() {
            if ($(this).find('.input-validation-error').length > 0) {
                $(this).addClass('has-error');
            }
        });
    });

    $("input[type='password'], input[type='text']").blur(function() {
        if ($(this).hasClass('input-validation-error') == true || $(this).closest(".input-group").find('.input-validation-error').length > 0) {
            $(this).addClass('has-error');
            $(this).closest(".input-group").addClass("has-error");
        } else {
            $(this).removeClass('has-error');
            $(this).closest(".input-group").removeClass("has-error");
        }
    });

    $('form').submit(function() {
        if ($(this).valid()) {
            $(this).find('div.form-group').each(function() {
                if ($(this).find('.input-validation-error').length == 0) {
                    $(this).removeClass('has-error');
                }
            });
        } else {
            $(this).find('div.form-group').each(function() {
                if ($(this).find('.input-validation-error').length > 0) {
                    $(this).addClass('has-error');
                }
            });
        }
    });

    $('form').each(function() {
        $(this).find('div.form-group').each(function() {
            if ($(this).find('.input-validation-error').length > 0) {
                $(this).addClass('has-error');
            }
        });
    });

    $("input[type='password'], input[type='text']").blur(function() {
        if ($(this).hasClass('input-validation-error') == true || $(this).closest(".form-group").find('.input-validation-error').length > 0) {
            $(this).addClass('has-error');
            $(this).closest(".form-group").addClass("has-error");
        } else {
            $(this).removeClass('has-error');
            $(this).closest(".form-group").removeClass("has-error");
        }
    });

    $("input[type='password'], input[type='text']").blur(function() {
        if ($(this).hasClass('input-validation-error') == true || $(this).closest(".form-group").find('span.field-validation-error').length > 0) {
            $(this).addClass('has-error');
            $(this).closest(".form-group").addClass("has-error");
        } else {
            $(this).removeClass('has-error');
            $(this).closest(".form-group").removeClass("has-error");
        }
    });
}
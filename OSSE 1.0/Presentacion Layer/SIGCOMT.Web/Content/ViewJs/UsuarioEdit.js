jQuery(document).ready(function () {

    jQuery("#IdiomaId").select2({ minimumResultsForSearch: 10 });

    $('#frmUsuario').on('submit', function (e) {
        var that = this;

        if (!$(this).valid()) {
            e.preventDefault();
            return;
        }

        Utils.ShowMessage("Usuario", "Desea realizar la operacion?", "Si", function(modal) {
            var form = Utils.GetForm(that);

            $.ajax({
                url: form.url,
                type: form.type,
                data: form.data,
                success: function (response) {
                    Utils.NotificationMessage({
                        tipo: response.Success ? 1 : 2,
                        title: 'Usuario',
                        message: response.Message
                    });

                    if (response.Success == true) {
                        setTimeout(function() {
                            window.location = form.listUrl;
                        }, 2000);
                    } 
                },
                error: function (x, xh, xhr) {
                    Utils.NotificationMessage({
                        tipo: 2,
                        title: 'Usuario',
                        message: x.responseText
                    });
                    console.error(xh);
                }
            });
        });

        e.preventDefault();
    });
});
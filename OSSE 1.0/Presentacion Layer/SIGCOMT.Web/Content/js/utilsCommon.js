var $msgModal = jQuery('#commonConfirmModal').modal({
    backdrop: true,
    show: false,
    keyboard: false
});


Utils = {
    Grilla: function (opciones) {

        if (opciones.dom == null)
            opciones.dom = '<"left"f><"right"l>trip';

        if (opciones.actualizarClass == null)
            opciones.actualizarClass = "a.actualizarReg";

        if (opciones.eliminarClass == null)
            opciones.eliminarClass = "a.eliminarReg";

        if (opciones.responsive == null)
            opciones.responsive = true;
        
        if (opciones.processing == null)
            opciones.processing = true;
        
        if (opciones.serverSide == null)
            opciones.serverSide = true;
        
        if (opciones.order == null)
            opciones.order = [[1, "desc"]];
        
        if (opciones.lengthMenu == null)
            opciones.lengthMenu = [[5, 25, 50, -1], [5, 25, 50, "Todos"]];

        var grilla = jQuery(opciones.grilla).dataTable({
            "dom": opciones.dom,
            "responsive": opciones.responsive,
            "processing": opciones.processing,
            "serverSide": opciones.serverSide,
            "language": {
                "url": opciones.languageUrl
            },
            "order": opciones.order,
            "lengthMenu": opciones.lengthMenu,
            "ajax": {
                "url": opciones.url,
                "type": "POST",
                "data": opciones.dataAjax
            },
            "columns": opciones.columns,
            "fnInitComplete": function() {
                // Permite reemplazar el evento de filtro por defecto del "keypress" por "enter"
                grilla.fnFilterOnReturn();
            }
        });

        jQuery('body').on('click', opciones.actualizarClass, function () {
            var that = this;

            eventoActualizar({
                grilla: grilla,
                button: that,
                urlActualizar: opciones.crud.actualizar.urlActualizar
            });
        });

        jQuery('body').on('click', opciones.eliminarClass, function () {
            var that = this;

            eventoEliminar({
                grilla: grilla,
                button: that,
                alertEliminar: {
                    title: opciones.crud.eliminar.alertEliminar.title,
                    message: opciones.crud.eliminar.alertEliminar.message,
                    textButtonCommand: opciones.crud.eliminar.alertEliminar.textButtonCommand
                },
                urlEliminar: opciones.crud.eliminar.urlEliminar
            });
        });

        function eventoActualizar(opcionesButton) {
            var aPos = opcionesButton.grilla.fnGetPosition(opcionesButton.button.parentNode);
            var aData = opcionesButton.grilla.fnGetData(aPos[0]);
            var rowId = opciones.crud.getKey(aData);

            window.location.href = opcionesButton.urlActualizar + '/' +  rowId;
        }

        function eventoEliminar(opcionesButton) {
            var aPos = opcionesButton.grilla.fnGetPosition(opcionesButton.button.parentNode);
            var aData = opcionesButton.grilla.fnGetData(aPos[0]);
            var rowId = opciones.crud.getKey(aData);

            Utils.ShowMessage(opcionesButton.alertEliminar.title,
                opcionesButton.alertEliminar.message,
                opcionesButton.alertEliminar.textButtonCommand,
                function(evt) {
                    $.ajax({
                        type: 'POST',
                        url: opcionesButton.urlEliminar + '/' + rowId,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        async: false,
                        cache: false,
                        success: function(response) {
                            Utils.NotificationMessage({
                                tipo: response.Success ? 1 : 2,
                                title: opcionesButton.alertEliminar.title,
                                message: response.Message
                            });

                            opcionesButton.grilla.fnClearTable(0);
                            opcionesButton.grilla.fnDraw();
                        },
                        error: function (data, error) {
                            Utils.NotificationMessage({
                                tipo: 2,
                                title: opcionesButton.alertEliminar.title,
                                message: data.responseText
                            });

                            console.error(error);
                        }
                    });
                });
        }
    },

    ShowMessage: function (header, body, btnSubmitText, callback) {
        $msgModal
            .find('.modal-content .alert > h4').text(header).end()
            .find('.modal-content .alert > p').text(body).end()
            .find('.callback-btn').html(btnSubmitText).end()
            .find('.callback-btn')
            .off("click.callback-btn")
            .on("click.callback-btn", function(ev) {
                callback($msgModal);
                $msgModal.modal('hide');
            });
        
        $msgModal.modal('show');
    },

    GetForm: function (form) {
        var that = jQuery(form);
        var url = that.attr('action');
        var type = that.attr('method');
        var listUrl = that.attr('datalistUrl');
        var data = {};

        that.find('[name]').each(function (index, value) {
            var innerthat = jQuery(this);
            var name = innerthat.attr('name');
            var datavalue = innerthat.val();

            data[name] = datavalue;
        });

        var obj = {
            url: url,
            type: type,
            data: data,
            listUrl: listUrl
        };

        return obj;
    },

    NotificationMessage: function (parametros) {

        var notificationType = '';
        switch (parametros.tipo) {
        case 1:
            notificationType = 'growl-success';
            break;
        case 2:
            notificationType = 'growl-danger';
            break;
        case 3:
            notificationType = 'growl-warning';
            break;
        }

        jQuery.gritter.add({
            title: parametros.title,
            text: parametros.message,
            class_name: notificationType,
            sticky: false,
            time: ''
        });
    }
}
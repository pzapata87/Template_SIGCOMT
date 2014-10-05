OSSE = {
    PadLeft: function (value, len, character) {
        len = len - value.length;
        for (var i = 1; i <= len; i++) {
            value = character + value;
        }
        return value;
    },
    ValidarByRegExp: function (evt, reg) {
        evt = evt || window.event;
        var code = evt.which || evt.keyCode;
        var texto = String.fromCharCode(code);
        var cadena = texto != null ? texto.toString() : '';
        if (!(reg instanceof RegExp)) {
            return false;
        }
        return reg.test(cadena);
    },
    Ajax: function (opciones, successCallback, failureCallback, errorCallback) {

        if (opciones.url == null)
            opciones.url = "";

        if (opciones.parametros == null)
            opciones.parametros = {};

        if (opciones.async == null)
            opciones.async = true;

        if (opciones.datatype == null)
            opciones.datatype = "json";

        if (opciones.contentType == null)
            opciones.contentType = "application/json; charset=utf-8";

        if (opciones.type == null)
            opciones.type = "POST";

        $.ajax({
            type: opciones.type,
            url: opciones.url,
            contentType: opciones.contentType,
            dataType: opciones.datatype,
            async: opciones.async,
            data: opciones.datatype == "json" ? JSON.stringify(opciones.parametros) : opciones.parametros,
            success: function (response) {
                if (successCallback != null && typeof (successCallback) == "function")
                    successCallback(response);
            },
            failure: function (msg) {
                alert(msg);
                if (failureCallback != null && typeof (failureCallback) == "function")
                    failureCallback(msg);
            },
            error: function (xhr, status, error) {
                alert(error);
                if (errorCallback != null && typeof (errorCallback) == "function")
                    errorCallback(xhr);
            }
        });
    },
    Ajax3: function (url, parameters, async) {
        var rsp;
        $.ajax({
            type: "POST",
            url: url,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (response) {
                rsp = response;
            },
            failure: function () {
                rsp = -1;
            }
        });
        return rsp;
    },
    CustomRange: function (input) {
        var fechaInicio = "FechaInicio";
        var fechaFin = "FechaFin";
        var min = new Date(2008, 11 - 1, 1), //Set this to your absolute minimum date
            dateMin = min,
            dateMax = new Date(3008, 11 - 1, 1),
            dayRange = 1000; // Set this to the range of days you want to restrict to

        if (input.id === fechaInicio) {
            if ($("#" + fechaFin).datepicker("getDate") != null) {
                dateMax = $("#" + fechaFin).datepicker("getDate");
                dateMin = $("#" + fechaFin).datepicker("getDate");
                dateMin.setDate(dateMin.getDate() - dayRange);
                if (dateMin < min) {
                    dateMin = min;
                }
            }
            else {
                dateMax = new Date(3008, 11 - 1, 1); //Set this to your absolute maximum date
            }
        }
        else if (input.id === fechaFin) {
            dateMax = new Date(3008, 11 - 1, 1); //Set this to your absolute maximum date
            if ($("#" + fechaInicio).datepicker("getDate") != null) {
                dateMin = $("#" + fechaInicio).datepicker("getDate");
                var rangeMax = new Date(dateMin.getFullYear(), dateMin.getMonth(), dateMin.getDate() + dayRange);

                if (rangeMax < dateMax) {
                    dateMax = rangeMax;
                }
            }
        }
        return {
            minDate: dateMin,
            maxDate: dateMax
        };
    },
    CrearTooltipError: function (opciones) {

        if (opciones.SelectorErrores == null)
            opciones.SelectorErrores = "#valSumModel";

        $(opciones.formulario).qtip({
            content: {
                text: function () {
                    return $(opciones.SelectorErrores).html();
                }
            },
            style: {
                classes: 'qtip-rounded qtip-red qtip-shadow',
                width: 280
            },
            position: {
                my: opciones.positionMy != null ? opciones.positionMy : 'right top',
                at: opciones.positionAt != null ? opciones.positionAt : 'top left'
            },
            show: false,
            hide: {
                event: 'unfocus',
                effect: function () {
                    $(this).slideUp();
                }
            }
        });
    },
    CreateDialogs: function (arrayDialog) {

        for (var i = 0; i < arrayDialog.length; i++) {
            if (arrayDialog[i].resizable == null)
                arrayDialog[i].resizable = false;
            $("#" + arrayDialog[i].name).dialog({
                autoOpen: false,
                resizable: arrayDialog[i].resizable,
                closeOnEscape: true,
                height: arrayDialog[i].height,
                width: arrayDialog[i].width,
                title: arrayDialog[i].title,
                modal: true,
                appendTo: arrayDialog[i].contenedor,
                close: function () {
                    var name = $(this).attr('id');
                    $.each(arrayDialog, function (index, v) {
                        if (v.name == name) {
                            if (v.closePopUp != null && typeof (v.closePopUp) == "function") {
                                v.closePopUp(name);
                                //return false;
                            }
                            else {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            });
        }
    },
    EditDialogs: function (arrayDialog) {

        for (var i = 0; i < arrayDialog.length; i++) {
            $("#" + arrayDialog[i].name).dialog('option', 'title', arrayDialog[i].title);
            $("#" + arrayDialog[i].name).dialog('option', 'height', arrayDialog[i].height);
            $("#" + arrayDialog[i].name).dialog('option', 'width', arrayDialog[i].width);
        }
    },
    CreateDialogPopUpForm: function (opciones) {
        var guardarButtonConfig, cancelarButtonConfig;
        var myButtons = [];

        var dialogForm = $('<div style="padding: 10px; min-width: 250px; word-wrap: break-word;"></div>');
        var iframe = $('<iframe frameborder="0" marginwidth="0" marginheight="0"></iframe>');
        iframe.attr({ src: opciones.url });

        if (opciones.customCloseAction == null)
            opciones.customCloseAction = false;

        if (opciones.buttonsConfig.showGuardarButton == null)
            opciones.buttonsConfig.showGuardarButton = true;

        if (opciones.buttonsConfig.showCancelarButton == null)
            opciones.buttonsConfig.showCancelarButton = true;

        if (opciones.buttonsConfig.showGuardarButton) {
            guardarButtonConfig = {
                text: opciones.buttonsConfig.guardarText,
                icons: { primary: opciones.buttonsConfig.guardarIcon != null ? opciones.buttonsConfig.guardarIcon : "ui-icon ui-icon-disk" },
                click: function () {
                    var functionOk = window[opciones.buttonsConfig.GuardarHandlerFunction];
                    if (opciones.frameOptions != null) {
                        functionOk = iframe.get(0).contentWindow[opciones.buttonsConfig.GuardarHandlerFunction];
                    }

                    if (typeof (functionOk) == 'function') {
                        functionOk(this, opciones.buttonsConfig.parametroHandlers);
                    }
                }
            };
            myButtons.push(guardarButtonConfig);
        }
        if (opciones.buttonsConfig.showCancelarButton) {
            cancelarButtonConfig = {
                text: opciones.buttonsConfig.cancelarText,
                icons: { primary: opciones.buttonsConfig.cancelarIcon != null ? opciones.buttonsConfig.cancelarIcon : "ui-icon ui-icon-close" },
                click: function() {
                    $(this).dialog("destroy").remove();
                }
            };
            myButtons.push(cancelarButtonConfig);
        }

        if (opciones.customButtons != null)
            $.merge(myButtons, opciones.customButtons);

        if (opciones.frameOptions != null) {
            iframe.height(opciones.frameOptions.frameHeight != null ? opciones.frameOptions.frameHeight : '100%');
            iframe.width(opciones.frameOptions.frameWidth != null ? opciones.frameOptions.frameWidth : '100%');

            dialogForm = dialogForm.append(iframe);
        }

        dialogForm = dialogForm.dialog({
            modal: opciones.modal == null ? true : opciones.modal,
            resizable: false,
            title: opciones.title,
            height: opciones.height,
            width: opciones.width,
            minHeight: opciones.minHeight != null ? opciones.minHeight : 75,
            appendTo: opciones.container,
            open: function () {
                if (opciones.frameOptions == null) {
                    var stopHandler = IniciarAnimacion($(this).parent());
                    $(this).load(opciones.url, function () {
                        stopHandler();
                    });
                }
            },
            beforeClose: function (event, ui) {
                if (opciones.beforeClosePopUp != null && typeof (opciones.beforeClosePopUp) == 'function') {
                    opciones.beforeClosePopUp(dialogForm);
                }
                return !opciones.customCloseAction;
            },
            close: function() {
                if (opciones.closePopUp != null && typeof (opciones.closePopUp) == 'function') {
                    opciones.closePopUp(dialogForm);
                } else {
                    $(this).dialog("destroy").remove();
                    if (opciones.frameOptions != null) {
                        repositionDialogs();
                    }
                }
            }
        });
       
        dialogForm.dialog('option', 'buttons', myButtons);
        dialogForm.parent().draggable({
            containment: opciones.dragContainer == null ? '.tabpanel_content' : opciones.dragContainer
        });

        dialogForm.parent().position({
            my: opciones.positionMy != null ? opciones.positionMy : 'center',
            at: opciones.positionAt != null ? opciones.positionAt : 'center',
            of: opciones.dragContainer == null ? '.tabpanel_content' : opciones.dragContainer
        });

        if (opciones.dialogExtenderOptions != null) {
            dialogForm.dialogExtend(opciones.dialogExtenderOptions);
        }

        return dialogForm;
    },
    CreateDialogsConfirm: function (arrayDialog) {
        for (var i = 0; i < arrayDialog.length; i++) {
            $("#" + arrayDialog[i].name).dialog({
                autoOpen: false,
                resizable: false,
                height: arrayDialog[i].height,
                width: arrayDialog[i].width,
                title: arrayDialog[i].title,
                modal: true,
                appendTo: arrayDialog[i].contenedor,
                buttons: [{
                    text: arrayDialog[i].titleBtn1,
                    click: function () {
                        var name = $(this).attr('id');
                        $.each(arrayDialog, function (index, v) {
                            if (v.name == name) {
                                var fun = window[v.strFun];
                                fun();
                                return;
                            }
                        });
                    }
                }, {
                    text: arrayDialog[i].titleBtn2,
                    click: function () {
                        $(this).dialog("close");
                    }
                }]
            });
        }
    },
    ObtenerFormulario: function (url, parameters, contenedorInformacion) {
        $.ajax({
            url: url,
            data: parameters,
            cache: false,
            dataType: 'html',
            success: function (result) {
                $('#' + contenedorInformacion).show();
                $('#' + contenedorInformacion).html(result);
            },
            error: function (request) {
                $('#' + contenedorInformacion).hide();
                alert(request.responseText);
            }
        });
    },
    Grilla: function (opciones) {
        $.jgrid.nav = true;

        var grid = jQuery('#' + opciones.grilla);
        var estadoSubGrid = false;

        if (opciones.hidegrid == null)
            opciones.hidegrid = false;

        if (opciones.noregistro == null) {
            opciones.noregistro = false;
        }
        if (opciones.sort == null) {
            opciones.sort = 'desc';
        }
        if (opciones.subGrid != null) {
            estadoSubGrid = true;
        }
        if (opciones.rowNumber == null) {
            opciones.rowNumber = 15;
        }
        if (opciones.rowNumbers == null) {
            opciones.rowNumbers = [opciones.rowNumber, 20, 50, 100, 150];
        }

        if (opciones.showRowNumbers == null)
            opciones.showRowNumbers = true;

        if (opciones.rules == null) {
            opciones.rules = false;
        }
        if (opciones.dialogDelete == null) {
            opciones.dialogDelete = 'dialog-delete';
        }
        if (opciones.dialogAlert == null) {
            opciones.dialogAlert = 'dialog-alert';
        }
        if (opciones.search == null) {
            opciones.search = false;
        }
        if (opciones.footerrow == null) {
            opciones.footerrow = false;
        }

        if (opciones.multiselect == null) {
            opciones.multiselect = false;
        }
        if (opciones.agregarBotones == null) {
            opciones.agregarBotones = false;
        }
        if (opciones.gridLocal == null) {
            opciones.gridLocal = false;

            if (opciones.cellEdit == null) {
                opciones.cellEdit = false;
            }
        }

        if (opciones.mostrarCaption == null)
            opciones.mostrarCaption = true;

        if (opciones.nuevoCaption == null)
            opciones.nuevoCaption = "Nuevo";

        if (opciones.editarCaption == null)
            opciones.editarCaption = "Editar";

        if (opciones.eliminarCaption == null)
            opciones.eliminarCaption = "Eliminar";

        if (opciones.lenguaje == null)
            opciones.lenguaje = "es-pe";

        if (opciones.alertTitle == null)
            opciones.alertTitle = "Alterta";

        if (opciones.sinRegistro == null)
            opciones.sinRegistro = "Sin registros";

        if (opciones.emptyrecords == null)
            opciones.emptyrecords = 'Sin registros que mostrar';

        if (opciones.refreshtext == null)
            opciones.refreshtext = 'Actualizar';

        if (opciones.customButtons == null)
            opciones.customButtons = {};

        if (opciones.canEventSameRow == null)
            opciones.canEventSameRow = true;

        if (opciones.pgbuttons == null)
            opciones.pgbuttons = true;

        if (opciones.pginput == null)
            opciones.pginput = true;

        if (opciones.height == null)
            opciones.height = 'auto';

        if (opciones.width == null)
            opciones.width = 'auto';

        if (opciones.caption == null)
            opciones.caption = '';

        if (opciones.autowidth == null)
            opciones.autowidth = true;

        if (opciones.treeGrid == null)
            opciones.treeGrid = false;

        var rowKey;
        var lastRowKey;

        if (!opciones.gridLocal) {
            grid.jqGrid({
                prmNames: {
                    search: 'isSearch',
                    nd: null,
                    rows: 'rows',
                    page: 'page',
                    sort: 'sortField',
                    order: 'sortOrder',
                    filters: 'filters'
                },
                postData: { searchString: '', searchField: '', searchOper: '', filters: '' },
                jsonReader: {
                    root: 'Rows',
                    page: 'Page',
                    total: 'Total',
                    records: 'Records',
                    cell: 'Cell',
                    id: opciones.id, //index of the column with the PK in it
                    userdata: 'userdata',
                    repeatitems: true
                },
                sortable: opciones.sortable != null ? opciones.sortable : true,
                emptyrecords: opciones.emptyrecords,
                pgbuttons: opciones.pgbuttons,
                pginput: opciones.pginput,
                rowNum: opciones.rowNumber,
                rowList: opciones.rowNumbers,
                pager: '#' + opciones.pager,
                sortname: opciones.sortName,
                viewrecords: true,
                multiselect: opciones.multiselect,
                rownumbers: true,
                hidegrid: opciones.hidegrid,
                sortorder: opciones.sort,
                height: opciones.height,
                footerrow: opciones.footerrow,
                width: opciones.width,
                autowidth: opciones.autowidth,
                colNames: opciones.colsNames,
                colModel: opciones.colsModel,
                caption: opciones.caption,
                subGrid: estadoSubGrid,
                editurl: opciones.EditingOptions ? opciones.EditingOptions.editUrl : '',
                ajaxSelectOptions: { type: "POST", contentType: 'application/json; charset=utf-8', dataType: 'json', },
                subGridRowColapsed: function(subgridId) {
                    var subgridTableId, pagerId;
                    subgridTableId = subgridId + "_t";
                    pagerId = "p_" + subgridTableId;
                    jQuery("#" + subgridTableId).remove();
                    jQuery("#" + pagerId).remove();
                },
                subGridRowExpanded: function(subgridId, rowId) {
                    var subGrid = opciones.subGrid;

                    var subgridTableId, pagerId;
                    subgridTableId = subgridId + "_t";
                    pagerId = "p_" + subgridTableId;

                    $("#" + subgridId).html("<table id='" + subgridTableId + "' class='scroll'></table><div id='" + pagerId + "' class='scroll'></div>");

                    var parameters = { cDocNro: rowId };
                    $.ajax({
                        type: "POST",
                        url: subGrid.Url,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(parameters),
                        success: function(rsp) {
                            var data = (typeof rsp.d) == 'string' ? eval('(' + rsp.d + ')') : rsp.d;
                            $("#" + subgridTableId).jqGrid({
                                datatype: "local",
                                colNames: subGrid.ColNames,
                                colModel: subGrid.ColModels,
                                rowNum: 10,
                                rowList: [10, 20, 50, 100],
                                sortorder: "desc",
                                viewrecords: true,
                                rownumbers: true,
                                pager: "#" + pagerId,
                                loadonce: true,
                                sortable: true,
                                height: subGrid.Height,
                                width: subGrid.Width
                            });

                            for (var i = 0; i <= data.length; i++)
                                jQuery("#" + subgridTableId).jqGrid('addRowData', i + 1, data[i]);

                            $("#" + subgridTableId).trigger("reloadGrid");
                        }
                    });
                },
                ondblClickRow: function(rowid) {
                    if (opciones.search) {
                        var ret = grid.getRowData(rowid);
                        SelectRow(ret);
                    }

                    if (opciones.EditingOptions != null && opciones.EditingOptions.canEditRowInline) {
                        if (opciones.BeforeEditHandler != null && typeof (opciones.BeforeEditHandler) == "function")
                            opciones.BeforeEditHandler(rowKey);

                        var editparameters = {
                            "keys": true,
                            "oneditfunc": null,
                            "successfunc": opciones.EditingOptions.SaveRowInline != null ? opciones.EditingOptions.SaveRowInline : null,
                            "url": opciones.EditingOptions.editUrl != null ? opciones.EditingOptions.editUrl : null,
                            "extraparam": {},
                            "aftersavefunc": opciones.EditingOptions.AfterSaveFunc != null ? opciones.EditingOptions.AfterSaveFunc : null,
                            "errorfunc": null,
                            "afterrestorefunc": opciones.EditingOptions.AfterSaveRowInline != null ? opciones.EditingOptions.AfterSaveRowInline : null,
                            "restoreAfterError": true,
                            "mtype": "POST"
                        };
                        grid.jqGrid("editRow", rowKey, editparameters);
                    }

                    if (opciones.DblClickHandler != null && typeof (opciones.DblClickHandler) == 'function') {
                        opciones.DblClickHandler(rowid);
                    }
                    lastRowKey = rowKey;
                },
                beforeSelectRow: function(rowid, e) {
                    if (opciones.BeforeSelectRow != null && typeof (opciones.BeforeSelectRow) == 'function') {
                        opciones.BeforeSelectRow(rowid, e);
                    }
                    return true;
                },
                onSelectRow: function(id) {
                    rowKey = grid.getGridParam('selrow');

                    if (rowKey != null) {
                        $("#" + opciones.identificador).val(rowKey);
                    }

                    if (opciones.EditingOptions != null && opciones.EditingOptions.canEditRowInline) {
                        if (lastRowKey !== rowKey) {
                            if (lastRowKey != null) {
                                var ind = grid.jqGrid('getInd', lastRowKey, true);
                                if (ind != false) {
                                    if ($(ind).attr("editable") === "1") {
                                        grid.jqGrid('restoreRow', lastRowKey);
                                        grid.jqGrid('resetSelection');
                                        rowKey = null;
                                    }
                                }
                            }
                        }
                    }

                    if (opciones.OnSelectRow != null && typeof (opciones.OnSelectRow) == 'function') {
                        if (opciones.canEventSameRow || (lastRowKey !== rowKey))
                            if (opciones.multiselect == false)
                                opciones.OnSelectRow(id);
                            else
                                opciones.OnSelectRow(id, opciones.nameArraySelected);
                    }
                    lastRowKey = rowKey;
                },
                onSelectAll: opciones.OnSelectAll,
                gridComplete: function() {
                    if (grid.getGridParam('records') == 0) {
                        if (opciones.noregistro == true) {
                            OSSE.Alert(opciones.alertTitle, opciones.sinRegistro, null);
                        }
                    }

                    rowKey = null;
                    if (opciones.agregarBotones == true) {
                        AgregarBotones();
                    }

                    if (opciones.GridCompleteHandler != null && typeof (opciones.GridCompleteHandler) == "function")
                        opciones.GridCompleteHandler(grid);
                },
                datatype: function(postdata) {
                    var migrilla = new Object();
                    migrilla.Page = postdata.page;
                    migrilla.Rows = postdata.rows;
                    migrilla.Sidx = postdata.sortField;
                    migrilla.Sord = postdata.sortOrder;
                    migrilla.Search = postdata.isSearch;
                    migrilla.Filters = postdata.filters;

                    if (opciones.rules != false) {

                        if (opciones.GetRulesMethod == null)
                            opciones.GetRulesMethod = GetRules;

                        var parametroExtra = null;
                        var customRules = opciones.GetRulesMethod(opciones.grilla, migrilla);

                        if (migrilla.Filters != "") {

                            parametroExtra = JSON.parse(migrilla.Filters);

                            if (customRules != null) {
                                parametroExtra.Groups = new Array();

                                if (opciones.AdvancedSearch == true) {
                                    parametroExtra.Groups.push(customRules);
                                } else {
                                    var nuevoSubGrupo = { GroupOp: 'AND', Rules: {} };
                                    nuevoSubGrupo.Rules = customRules;

                                    parametroExtra.Groups.push(nuevoSubGrupo);
                                }
                            }
                        } else {
                            if (opciones.AdvancedSearch == true && customRules != null)
                                parametroExtra = customRules;
                            else if (customRules != null && customRules.length > 0) {
                                parametroExtra = { groupOp: 'AND', rules: {} };
                                parametroExtra.rules = customRules;
                            }
                        }

                        migrilla.Filters = parametroExtra == null ? "" : JSON.stringify(parametroExtra);
                    }

                    if (migrilla._search == true) {
                        migrilla.SearchField = postdata.searchField;
                        migrilla.SearchOper = postdata.searchOper;
                        migrilla.SearchString = postdata.searchString;
                    }

                    var params = { grid: migrilla };

                    $.ajax({
                        url: opciones.urlListar,
                        type: 'post',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(params),
                        async: false,
                        success: function(data, st) {
                            if (st == 'success') {
                                var jq = grid[0];
                                jq.addJSONData(data);
                            }
                        },
                        error: function(a, b, c) {

                            alert('Error with AJAX callback:' + a.responseText);
                        }
                    });
                }
            });
        }
        else if (opciones.gridLocal) {
            //opciones.pivotGrid
            var settingsGrid = {
                colNames: opciones.colsNames,
                colModel: opciones.colsModel,
                sortorder: opciones.sort,
                emptyrecords: opciones.emptyrecords,
                pgbuttons: opciones.pgbuttons,
                pginput: opciones.pginput,
                rowNum: opciones.rowNumber,
                rowList: opciones.rowNumbers,
                rownumbers: opciones.showRowNumbers,
                hidegrid: opciones.hidegrid,
                cellEdit: opciones.cellEdit,
                cellsubmit: "clientArray",
                pager: '#' + opciones.pager,
                sortname: opciones.sortName,
                viewrecords: true,
                multiselect: opciones.multiselect,
                footerrow: opciones.footerrow,
                height: opciones.height,
                width: opciones.width,
                gridview: true,
                autowidth: opciones.autowidth,
                caption: opciones.caption,
                subGrid: estadoSubGrid,
                editurl: opciones.editurl,
                treeGrid: opciones.treeGrid,
                gridComplete: function() {
                    if (opciones.GridCompleteHandler != null && typeof (opciones.GridCompleteHandler) == "function")
                        opciones.GridCompleteHandler();
                },
                loadComplete: function() {
                    if (opciones.LoadCompleteHandler != null && typeof (opciones.LoadCompleteHandler) == "function")
                        opciones.LoadCompleteHandler();
                },
                afterSaveCell: function(rowid, cellname, value, iRow, iCol) {
                    if (opciones.AfterSaveCellHandler != null)
                        opciones.AfterSaveCellHandler(rowid, cellname, value, iRow, iCol);
                },
                ondblClickRow: function(rowid) {
                    if (opciones.search) {
                        var ret = grid.getRowData(rowid);
                        SelectRow(ret);
                    }

                    if (opciones.EditingOptions != null && opciones.EditingOptions.canEditRowInline) {
                        if (opciones.BeforeEditHandler != null && typeof (opciones.BeforeEditHandler) == "function")
                            opciones.BeforeEditHandler(rowKey);

                        var editparameters = {
                            "keys": true,
                            "oneditfunc": null,
                            "successfunc": opciones.EditingOptions.SaveRowInline != null ? opciones.EditingOptions.SaveRowInline : null,
                            "url": opciones.EditingOptions.editUrl != null ? opciones.EditingOptions.editUrl : null,
                            "extraparam": {},
                            "aftersavefunc": opciones.EditingOptions.AfterSaveFunc != null ? opciones.EditingOptions.AfterSaveFunc : null,
                            "errorfunc": null,
                            "afterrestorefunc": opciones.EditingOptions.AfterSaveRowInline != null ? opciones.EditingOptions.AfterSaveRowInline : null,
                            "restoreAfterError": true,
                            "mtype": "POST"
                        };
                        grid.jqGrid("editRow", rowKey, editparameters);
                    }

                    if (opciones.DblClickHandler != null && typeof (opciones.DblClickHandler) == 'function') {
                        opciones.DblClickHandler(rowid);
                    }
                    lastRowKey = rowKey;
                },
                onSelectRow: function(id) {
                    rowKey = grid.getGridParam('selrow');

                    if (rowKey != null) {
                        $("#" + opciones.identificador).val(rowKey);
                    }

                    if (opciones.EditingOptions != null && opciones.EditingOptions.canEditRowInline) {
                        if (lastRowKey !== rowKey) {
                            if (lastRowKey != null) {
                                var ind = grid.jqGrid('getInd', lastRowKey, true);
                                if (ind != false) {
                                    if ($(ind).attr("editable") === "1") {
                                        grid.jqGrid('restoreRow', lastRowKey);
                                        grid.jqGrid('resetSelection');
                                        rowKey = null;
                                    }
                                }
                            }
                        }
                    }

                    if (opciones.OnSelectRow != null && typeof (opciones.OnSelectRow) == 'function') {
                        if (opciones.canEventSameRow || (lastRowKey !== rowKey))
                            if (opciones.multiselect == false)
                                opciones.OnSelectRow(id);
                            else
                                opciones.OnSelectRow(id, opciones.nameArraySelected);
                    }
                    lastRowKey = rowKey;
                },
                onSelectAll: opciones.OnSelectAll,
                beforeEditCell: function(rowid, cellname, value, iRow, iCol) {
                    if (opciones.BeforeEditCellHandler != null)
                        opciones.BeforeEditCellHandler(rowid, cellname, value, iRow, iCol);
                },
                afterEditCell: function(rowid, cellname, value, iRow, iCol) {
                    if (opciones.AfterEditCellHandler != null)
                        opciones.AfterEditCellHandler(rowid, cellname, value, iRow, iCol);
                },
                rowattr: function(rowId) {
                    if (opciones.RowAttrHandler != null)
                        opciones.RowAttrHandler(rowId);
                }
            };

            if (opciones.pivotGridOptions == null) {
                settingsGrid.datatype = "local";

                grid.jqGrid(settingsGrid);
            } else {
                if (opciones.pivotGridOptions.colTotals)
                    settingsGrid.footerrow = true;

                grid.jqGrid('jqPivot', opciones.pivotGridOptions.urlData, {
                    xDimension: opciones.pivotGridOptions.xDimensionColumns,
                    yDimension: opciones.pivotGridOptions.yDimensionColumns,
                    aggregates: opciones.pivotGridOptions.aggregateColumns,
                    groupSummaryPos: opciones.pivotGridOptions.groupSummaryPos == null ? 'header' : opciones.pivotGridOptions.groupSummaryPos,
                    colTotals: opciones.pivotGridOptions.colTotals == null ? false : opciones.pivotGridOptions.colTotals,
                    frozenStaticCols: opciones.pivotGridOptions.frozenStaticCols == null ? false : opciones.pivotGridOptions.frozenStaticCols,
                    groupSummary: opciones.pivotGridOptions.groupSummary == null ? true : opciones.pivotGridOptions.groupSummary,
                    rowTotals: opciones.pivotGridOptions.rowTotals == null ? false : opciones.pivotGridOptions.rowTotals,
                    rowTotalsText: opciones.pivotGridOptions.rowTotalsText == null ? "Total" : opciones.pivotGridOptions.rowTotalsText
                }, settingsGrid, opciones.pivotGridOptions.ajaxOptions);
            }
        }

        grid.jqGrid('navGrid', "#" + opciones.pager, {
            edit: false,
            add: false,
            del: false,
            search: opciones.search,
            refresh: opciones.refresh,
            refreshtext: opciones.mostrarCaption ? opciones.refreshtext : '',
            refreshtitle: opciones.refreshtext
        },
        {}, // use default settings for edit
        {}, // use default settings for add
        {}, // delete instead that del:false we need this
        {
            multipleSearch: true,
            beforeShowSearch: function() {
                return true;
            }
        });

        if (opciones.eliminar) {
            grid.navButtonAdd('#' + opciones.pager, {
                id: opciones.eliminarId != null ? opciones.eliminarId : '',
                caption: opciones.mostrarCaption ? opciones.eliminarCaption : '',
                title: opciones.eliminarCaption,
                buttonicon: 'ui-icon-trash',
                position: 'first',
                onClickButton: function () {
                    if (rowKey != null) {
                        var dialogDelete = opciones.dialogDelete;
                        if (dialogDelete != null) {
                            var continuar = true;
                            if (dialogDelete.ContinueDeletingHandler != null && typeof (dialogDelete.ContinueDeletingHandler) == "function")
                                continuar = dialogDelete.ContinueDeletingHandler(rowKey);

                            if (continuar) {
                                var rowData = grid.getRowData(rowKey);

                                var parametrosConfirm = {
                                    dialogText: dialogDelete.dialogText == null ? dialogDelete.DialogGetTextHandler(rowData) : dialogDelete.dialogText,
                                    OkHandlerFunction: dialogDelete.OkHandler,
                                    CancelHandlerFunction: dialogDelete.CancelHandler,
                                    dialogTitle: dialogDelete.dialogTitle,
                                    dialogOkText: dialogDelete.okText,
                                    dialogCancelText: dialogDelete.cancelText,
                                    parametroHandlers: rowData
                                };
                                OSSE.confirm(parametrosConfirm);
                            }
                        }
                    } else {
                        OSSE.Alert(opciones.alertTitle, opciones.mensaje, null);
                    }
                }
            });
        }

        if (opciones.editar) {
            grid.navButtonAdd('#' + opciones.pager, {
                id: opciones.editarId != null ? opciones.editarId : '',
                caption: opciones.mostrarCaption ? opciones.editarCaption : '',
                title: opciones.editarCaption,
                buttonicon: opciones.editarButtonIcon == null ? 'ui-icon-pencil' : opciones.editarButtonIcon,
                position: 'first',
                onClickButton: function () {
                    if (rowKey != null) {
                        if (opciones.EditarCommand != null && typeof (opciones.EditarCommand) == "function")
                            opciones.EditarCommand(rowKey);
                        else
                            Editar(rowKey);
                    } else {
                        OSSE.Alert(opciones.alertTitle, opciones.mensaje, null);
                    }
                }
            });
        }

        if (opciones.nuevo) {
            grid.navButtonAdd('#' + opciones.pager, {
                id: opciones.nuevoId != null ? opciones.nuevoId : '',
                caption: opciones.mostrarCaption ? opciones.nuevoCaption : '',
                title: opciones.nuevoCaption,
                buttonicon: 'ui-icon-plus',
                position: 'first',
                onClickButton: function () {
                    if (opciones.NuevoCommand != null && typeof (opciones.NuevoCommand) == "function")
                        opciones.NuevoCommand(rowKey);
                    else
                        Nuevo(rowKey);
                }
            });
        }

        if (opciones.customButtons) {
            $.each(opciones.customButtons, function (index, botonNuevo) {
                grid.navButtonAdd('#' + opciones.pager, {
                    id: botonNuevo.Id != null ? botonNuevo.Id : '',
                    caption: botonNuevo.Caption,
                    title: botonNuevo.Title,
                    buttonicon: botonNuevo.ButtonIcon ? botonNuevo.ButtonIcon : 'ui-icon-plus',
                    position: botonNuevo.Position ? botonNuevo.Position : 'first',
                    onClickButton: function () {
                        if (botonNuevo.ClickFunction != null && typeof (botonNuevo.ClickFunction) == "function")
                            botonNuevo.ClickFunction(rowKey);
                    }
                });
            });
        }

        //grid.jqGrid('gridResize', { minWidth: 450, minHeight: 150 });
    },
    LoadDropDownList: function (name, url, parameters, selected, isValIndex, async) {
        var combo = document.getElementById(name);
        combo.options.length = 0;
        combo.options[0] = new Option("");
        combo.selectedIndex = 0;

        $('#' + name).ajaxError(function () {
            combo.options[0] = new Option("Error al cargar.");
        });
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (items) {
                $.each(items, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (selected == undefined) selected = 0;

                if (isValIndex) {
                    $('#' + name).attr('selectedIndex', selected);
                } else {
                    $('#' + name).val(selected);
                }
            }
        });
    },
    LoadDropDownListMulti: function (name, url, parameters, selected, async) {
        var combo = document.getElementById(name);

        $('#' + name).ajaxError(function () {
            combo.options[0] = new Option("Error al cargar.");
        });
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (items) {
                var list = items;
                $.each(list, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (selected == undefined) selected = 0;
                $('#' + name).val(selected);
            }
        });
    },
    LoadDropDownListSinFormato: function (name, url, parameters, selected, async) {
        var combo = document.getElementById(name);
        combo.options.length = 0;
        combo.options[0] = new Option("");
        combo.selectedIndex = 0;

        //        $('#' + name).ajaxError(function (event, request, settings) {
        //            combo.options[0] = new Option("Error al cargar.");
        //        });
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (items) {
                var list = items;
                $.each(list, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (selected == undefined) selected = 0;
                $('#' + name).val(selected);
            }
        });
    },
    LoadDropDownListItems: function (name, url, parameters, selected) {
        var combo = document.getElementById(name);
        combo.options.length = 0;
        combo.options[0] = new Option("");
        combo.selectedIndex = 0;

        var opciones = {
            url: url,
            parametros: parameters
        };

        OSSE.Ajax(opciones, function (response) {
            if (response.Success) {
                combo.disabled = true;
                $.each(response.Data, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                combo.disabled = false;
                if (selected == undefined)
                    selected = 0;
                $('#' + name).val(selected);
            } else {
                OSSE.Alert('Alerta', response.Message, null);
            }
        });
    },
    confirm: function (opciones) {

        var esperaRespuesta = new $.Deferred();

        var dialog = $('<div style="padding: 10px; max-width: 500px; min-width: 300px; word-wrap: break-word;">' + opciones.dialogText + '</div>').dialog({
            draggable: true,
            modal: true,
            resizable: false,
            width: 'auto',
            position: 'center',
            dialogClass: opciones.dialogClass,
            appendTo: opciones.container,
            title: opciones.dialogTitle || 'Confirmación',
            minHeight: 75,
            buttons: [{
                text: opciones.dialogOkText,
                click: function () {
                    if (typeof (opciones.OkHandlerFunction) == 'function') {
                        $(this).dialog("destroy");
                        var respuestaHandler;
                        
                        $.when(opciones.OkHandlerFunction(opciones.parametroHandlers)).then(function () {
                            respuestaHandler = esperaRespuesta.resolve();
                        }, function () {
                            respuestaHandler = esperaRespuesta.reject();
                        });

                        return respuestaHandler;
                    } else {
                        alert("Debe definir un handler para la accion OK");
                    }
                    
                    return esperaRespuesta.reject();
                }
            }, {
                text: opciones.dialogCancelText,
                click: function () {
                    if (typeof (opciones.CancelHandlerFunction) == 'function') {
                        $(this).dialog("destroy").remove();
                        opciones.CancelHandlerFunction(opciones.parametroHandlers);
                    } else {
                        $(this).dialog("destroy").remove();
                    }

                    return esperaRespuesta.reject();
                }
            }]
        });

        dialog.parent().draggable({
            containment: opciones.dragContainer == null ? '.tabpanel_content' : opciones.dragContainer
        });

        dialog.parent().position({
            my: opciones.positionMy != null ? opciones.positionMy : 'center',
            at: opciones.positionAt != null ? opciones.positionAt : 'center',
            of: opciones.dragContainer == null ? '.tabpanel_content' : opciones.dragContainer
        });

        return esperaRespuesta.promise();
    },
    Alert: function (dialogTitle, dialogText, okFunc) {
        $('<div style="padding: 10px; min-width: 250px; word-wrap: break-word;">' + dialogText + '</div>').dialog({
            draggable: true,
            modal: true,
            resizable: false,
            width: '400px',
            position: 'center',
            title: dialogTitle || 'Alert',
            minHeight: 75,
            buttons: {
                OK: function () {
                    if (okFunc != null) {
                        if (typeof (okFunc) == 'function') { setTimeout(okFunc, 50); }
                    }
                    $(this).dialog('destroy');
                }
            }
        });
    },
    msgConfirm: function (rpt, message, funcionout, title) {
        $.jGrowl.defaults.closerTemplate = "<div>[ Cerrar todo ]</div>";

        var theme;

        if (title == null)
            title = 'Notification';

        if (rpt) {
            setTimeout(funcionout, 200);
            theme = "default";

        } else {
            theme = "error";
        }

        $.jGrowl(message,
            {
                themeState: theme,
                life: 7000,
                closeTemplate: '<a href="#" class="ui-dialog-titlebar-close ui-corner-all" role="button"><span class="ui-icon ui-icon-closethick">close</span></a>',
                header: title
            });
    },
    AlertNotificacion: function (rpt, title, message, funcionout) {
        var theme = "default";

        if (title == null)
            title = 'Notificación';
        
        if (rpt)
            funcionout();
        else
            theme = "error";

        $.jGrowl.defaults.closerTemplate = "<div>[ Cerrar todo ]</div>";
        $.jGrowl(message,
            {
                themeState: theme,
                life: 7000,
                closeTemplate: '<a href="#" class="ui-dialog-titlebar-close ui-corner-all" role="button"><span class="ui-icon ui-icon-closethick">close</span></a>',
                header: title
            });
    },
    msgConfirmPopup: function (frmId, rpt, message, funcionout, title) {
        if (frmId != null) {
            // Se agregó esta linea por que no se eliminaba los errores despues del Post, setea los estilos originales.
            $("#" + frmId + " .merror").removeClass().addClass("validation-summary-valid merror");
        }

        $.jGrowl.defaults.closerTemplate = "<div>[ Cerrar todo ]</div>";

        var theme;

        if (title == null)
            title = 'Notification';

        if (rpt) {
            theme = "default";
            if (typeof (funcionout) == 'function') {
                setTimeout(funcionout, 200);
            }
        } else {
            theme = "error";
        }

        $.jGrowl(message,
            {
                themeState: theme,
                life: 7000,
                closeTemplate: '<a href="#" class="ui-dialog-titlebar-close ui-corner-all" role="button"><span class="ui-icon ui-icon-closethick">close</span></a>',
                header: title
            });
    },
    LimpiarTilde: function (text) {
        text = text.replace("&#225;", 'á');
        text = text.replace("&#233;", 'é');
        text = text.replace("&#237;", 'í');
        text = text.replace("&#243;", 'ó');
        text = text.replace("&#250;", 'ú');
        text = text.replace("&#241;", 'ñ');

        text = text.replace("&#193;", 'Á');
        text = text.replace("&#201;", 'É');
        text = text.replace("&#205;", 'Í');
        text = text.replace("&#211;", 'Ó');
        text = text.replace("&#218;", 'Ú');
        text = text.replace("&#209;", 'Ñ');
        return text;
    },
    //Funcion utilizada para actualizar la seleccion de filas de una grilla
    UpdateIdsOfSelectedRows: function (id, idsOfSelectedRows) {
        var index = $.inArray(id, idsOfSelectedRows);
        if (index >= 0) {
            var removeMe = -1;
            $.each(idsOfSelectedRows, function (i, v) {
                if (v == id) {
                    removeMe = i;
                }
            });

            if (removeMe != -1) {
                idsOfSelectedRows.splice(removeMe, 1);
            }
        } else if (index < 0) {
            if (id != "")
                idsOfSelectedRows.push(id);
        }
    },
    ConfigAutoComplete: function(parameters) {
        $(parameters.TextField).focusout(function () {
            if ($(parameters.ValueField).val() == 0) {
                $(parameters.TextField).val("");
            }
        });

        $(parameters.TextField).bind('paste', function (e) {
            setTimeout(function () {
                $(parameters.TextField).trigger('autocomplete');
            }, 0);
        });
        
        $(parameters.TextField).change(function (e) {
            var txt = $(this).val();
            if (txt.trim() == "") {
                $(parameters.ValueField).val("0");
            }
        });

        var autoComplete = $(parameters.TextField).autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: parameters.Url,
                    dataType: "json",
                    data: parameters.Data != null ? parameters.Data : { term: request.term },
                    success: function (jsonResponse) {
                        response($.map(jsonResponse.Data, parameters.MapResponse));
                    }
                });
            },
            minLength: parameters.MinLength == null ? 2 : parameters.MinLength,
            messages: {
                noResults: "",
                results: function (resultsCount) { }
            },
            select: function(event, ui) {
                $(parameters.ValueField).val(ui.item.id);

                if (parameters.SelectCallback != null && typeof(parameters.SelectCallback) == "function" )
                    parameters.SelectCallback(ui.item);
            },
            response: function(event, ui) {
                $(parameters.ValueField).val('0');
            }
        });

        return autoComplete;
    }
};
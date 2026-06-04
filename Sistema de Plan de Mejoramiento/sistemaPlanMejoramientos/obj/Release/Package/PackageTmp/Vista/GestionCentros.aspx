<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionCentros.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.GestionCentros" ValidateRequest="false" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gestión de Centros - SENA</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <style>
        body {
            background: linear-gradient(135deg, #0b0f19 0%, #111827 100%);
            min-height: 100vh;
            color: #f8fafc;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .card-custom {
            background: rgba(17, 24, 39, 0.7);
            backdrop-filter: blur(12px);
            -webkit-backdrop-filter: blur(12px);
            border: 1px solid rgba(255, 255, 255, 0.08);
            border-radius: 16px;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
            transition: all 0.3s ease;
        }

            .card-custom:hover {
                border-color: rgba(57, 181, 74, 0.25);
                box-shadow: 0 8px 32px 0 rgba(57, 181, 74, 0.1);
            }

            .card-custom label,
            .form-label-custom,
            .card-custom .form-label {
                color: #ffffff !important;
                font-weight: 500 !important;
                margin-bottom: 5px;
                display: inline-block;
                opacity: 1 !important;
            }

            .card-custom h4,
            .card-custom h4 span,
            #lblTituloForm {
                color: #ffffff !important;
                font-weight: 600 !important;
            }

        .form-control, .form-select {
            background-color: rgba(31, 41, 55, 0.7) !important;
            border: 1px solid rgba(255, 255, 255, 0.2) !important;
            color: #ffffff !important;
            border-radius: 8px;
            padding: 10px;
        }

            .form-control:focus, .form-select:focus {
                background-color: #1f2937 !important;
                border-color: #39b54a !important;
                box-shadow: 0 0 0 0.25rem rgba(57, 181, 74, 0.25) !important;
                color: #ffffff !important;
            }

            .form-control::placeholder {
                color: #94a3b8 !important;
            }

        .btn-sena {
            background-color: #39b54a;
            color: #ffffff;
            font-weight: 600;
            border: none;
            padding: 12px;
            border-radius: 8px;
            transition: all 0.2s ease;
        }

            .btn-sena:hover {
                background-color: #2e943c;
                color: #ffffff;
                transform: translateY(-1px);
            }

        .table-responsive-custom {
            border-radius: 12px;
            overflow: hidden;
            border: 1px solid rgba(255, 255, 255, 0.1);
        }

        .table-custom {
            width: 100%;
            margin-bottom: 0 !important;
            background-color: transparent !important;
        }

            .table-custom th {
                background-color: #1e293b !important;
                color: #39b54a !important;
                font-weight: 600 !important;
                text-transform: uppercase;
                font-size: 0.85rem;
                letter-spacing: 0.5px;
                padding: 14px !important;
                border-bottom: 2px solid rgba(57, 181, 74, 0.3) !important;
                border-top: none !important;
                border-left: none !important;
                border-right: none !important;
            }

            .table-custom td {
                background-color: rgba(15, 23, 42, 0.6) !important;
                color: #ffffff !important;
                padding: 14px !important;
                border-color: rgba(255, 255, 255, 0.06) !important;
                font-size: 0.9rem;
            }

            .table-custom tr:nth-child(even) td {
                background-color: rgba(30, 41, 59, 0.4) !important;
            }

            .table-custom tr:hover td {
                background-color: rgba(57, 181, 74, 0.1) !important;
                color: #ffffff !important;
            }

        h2 {
            color: #39b54a;
            font-weight: 700;
        }

        .badge-activo {
            background-color: #1a7f37;
            color: #fff;
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
        }

        .badge-inactivo {
            background-color: #4b5563;
            color: #fff;
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
        }

        .pagination .page-link {
            background-color: rgba(31, 41, 55, 0.7);
            border-color: rgba(255, 255, 255, 0.15);
            color: #ffffff;
        }

        .pagination .page-item.active .page-link {
            background-color: #39b54a;
            border-color: #39b54a;
            color: #ffffff;
        }

        .pagination .page-link:hover {
            background-color: rgba(57, 181, 74, 0.2);
            border-color: #39b54a;
            color: #39b54a;
        }

        .pagination .page-item.disabled .page-link {
            background-color: rgba(31, 41, 55, 0.4);
            border-color: rgba(255, 255, 255, 0.08);
            color: #4b5563;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">

        <asp:HiddenField ID="hfMensajeTipo" runat="server" Value="" />
        <asp:HiddenField ID="hfMensajeTxt" runat="server" Value="" />

        <div class="container mt-5">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2><i class="bi bi-building me-2"></i>Centros de Formación</h2>
                <asp:LinkButton ID="lnkVolver" runat="server" CssClass="btn btn-outline-light btn-sm" OnClick="lnkVolver_Click">
                    <i class="bi bi-arrow-left"></i> Volver al Dashboard
                </asp:LinkButton>
            </div>

            <div class="row">

                <div class="col-md-4 mb-4">
                    <div class="card card-custom p-4">
                        <h4 class="mb-3 text-center">
                            <asp:Label ID="lblTituloForm" runat="server" Text="Registrar Centro"></asp:Label>
                        </h4>

                        <asp:HiddenField ID="hfIdCentro" runat="server" />

                        <div class="mb-2">
                            <label class="form-label small">Código del Centro</label>
                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" placeholder="Ej: 54001"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Nombre del Centro</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Centro de Comercio y Servicios"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Regional</label>
                            <asp:TextBox ID="txtRegional" runat="server" CssClass="form-control" placeholder="Ej: Regional Boyacá"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Municipio</label>
                            <asp:TextBox ID="txtMunicipio" runat="server" CssClass="form-control" placeholder="Ej: Sogamoso"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Departamento</label>
                            <asp:TextBox ID="txtDepartamento" runat="server" CssClass="form-control" placeholder="Ej: Boyacá"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label small">Estado</label>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                <asp:ListItem Value="Activo" Text="Activo"></asp:ListItem>
                                <asp:ListItem Value="Inactivo" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Centro"
                                CssClass="btn btn-sena"
                                OnClick="btnGuardar_Click"
                                OnClientClick="return confirmarGuardar(this);" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                                CssClass="btn btn-secondary btn-sm"
                                Visible="false"
                                OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                </div>

                <div class="col-md-8">
                    <div class="card card-custom p-4">
                        <h4 class="mb-3" style="color: #ffffff;">Centros Registrados</h4>

                        <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                            <div class="row mb-4">
                                <div class="col-md-9 col-lg-8">
                                    <div class="input-group input-group-sm">
                                        <span class="input-group-text" style="background: rgba(31,41,55,0.7); border-color: rgba(255,255,255,0.2); color: #94a3b8;">
                                            <i class="bi bi-search"></i>
                                        </span>

                                        <asp:TextBox ID="txtBuscar" runat="server"
                                            CssClass="form-control"
                                            placeholder="Buscar por código, nombre, regional, municipio...">
                                        </asp:TextBox>

                                        <asp:Button ID="btnBuscar" runat="server"
                                            Text="Buscar"
                                            CssClass="btn btn-sena"
                                            CausesValidation="false"
                                            OnClick="btnBuscar_Click" />

                                        <asp:LinkButton ID="btnLimpiarBuscar" runat="server"
                                            CssClass="btn btn-outline-secondary"
                                            CausesValidation="false"
                                            OnClick="btnLimpiarBuscar_Click">
                    <i class="bi bi-arrow-clockwise"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="table-responsive table-responsive-custom">
                            <asp:GridView ID="gvCentros" runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table table-custom table-hover table-bordered align-middle"
                                OnRowCommand="gvCentros_RowCommand"
                                OnRowDataBound="gvCentros_RowDataBound"
                                OnPageIndexChanging="gvCentros_PageIndexChanging"
                                DataKeyNames="idCentro"
                                AllowPaging="True"
                                PageSize="10"
                                PagerStyle-CssClass="d-none">
                                <Columns>
                                    <asp:BoundField DataField="idCentro" HeaderText="ID" ItemStyle-Width="45px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="codigoCentro" HeaderText="Código" ItemStyle-Width="75px" />
                                    <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="regional" HeaderText="Regional" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="municipio" HeaderText="Municipio" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="departamento" HeaderText="Depto." ItemStyle-Width="90px" />
                                    <asp:TemplateField HeaderText="Estado" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <span class='<%# Eval("estado").ToString() == "Activo" ? "badge-activo" : "badge-inactivo" %>'>
                                                <%# Eval("estado") %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar"
                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                CssClass="btn btn-warning btn-sm me-1"
                                                OnClientClick="return confirmarEditar(this);">
                                                <i class="bi bi-pencil-square"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar"
                                                CommandArgument='<%# Eval("idCentro") %>'
                                                CssClass="btn btn-danger btn-sm"
                                                OnClientClick="return confirmarEliminar(this);">
                                                <i class="bi bi-trash-fill"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="d-flex justify-content-between align-items-center mt-3 px-1">
                            <small style="color: #94a3b8;">Página <strong style="color: #39b54a;">
                                <asp:Literal ID="litPaginaActual" runat="server" /></strong>
                                de <strong style="color: #39b54a;">
                                    <asp:Literal ID="litTotalPaginas" runat="server" /></strong>
                                &nbsp;·&nbsp;
                                <asp:Literal ID="litTotalRegistros" runat="server" />
                                registros
                            </small>
                            <asp:Repeater ID="rptPaginacion" runat="server" OnItemCommand="rptPaginacion_ItemCommand">
                                <HeaderTemplate>
                                    <ul class="pagination pagination-sm mb-0">
                                        <li class='page-item <%# Convert.ToInt32(ViewState["PaginaActual"]) == 0 ? "disabled" : "" %>'>
                                            <asp:LinkButton runat="server" CssClass="page-link" CommandName="Pagina" CommandArgument="anterior">
                                                <i class="bi bi-chevron-left"></i>
                                            </asp:LinkButton>
                                        </li>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li class='page-item <%# Convert.ToInt32(Container.DataItem) == Convert.ToInt32(ViewState["PaginaActual"]) ? "active" : "" %>'>
                                        <asp:LinkButton runat="server" CssClass="page-link"
                                            CommandName="Pagina"
                                            CommandArgument='<%# Container.DataItem %>'><%# Convert.ToInt32(Container.DataItem) + 1 %></asp:LinkButton>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <li class='page-item <%# Convert.ToInt32(ViewState["PaginaActual"]) >= Convert.ToInt32(ViewState["TotalPaginas"]) - 1 ? "disabled" : "" %>'>
                                        <asp:LinkButton runat="server" CssClass="page-link" CommandName="Pagina" CommandArgument="siguiente">
                                                <i class="bi bi-chevron-right"></i>
                                        </asp:LinkButton>
                                    </li>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>

                    </div>
                </div>

            </div>
        </div>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script type="text/javascript">

        var eliminacionConfirmada = false;
        var edicionConfirmada = false;
        var guardadoConfirmado = false;

        function confirmarEliminar(btn) {
            if (eliminacionConfirmada) { eliminacionConfirmada = false; return true; }
            var hrefCapturado = btn.href;
            Swal.fire({
                title: '¿Eliminar centro?',
                text: 'Esta acción no se puede deshacer y puede afectar programas o fichas asociadas.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#e3342f',
                cancelButtonColor: '#6c757d',
                confirmButtonText: '<i class="bi bi-trash-fill"></i> Sí, eliminar',
                cancelButtonText: 'Cancelar',
                background: '#111827',
                color: '#ffffff'
            }).then(function (result) {
                if (result.isConfirmed) {
                    eliminacionConfirmada = true;
                    eval(hrefCapturado.replace('javascript:', ''));
                }
            });
            return false;
        }

        function confirmarEditar(btn) {
            if (edicionConfirmada) { edicionConfirmada = false; return true; }
            var hrefCapturado = btn.href;
            Swal.fire({
                title: '¿Cargar centro para modificar?',
                text: 'Los datos actuales en el formulario serán reemplazados.',
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#ffc107',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, editar',
                cancelButtonText: 'Cancelar',
                background: '#111827',
                color: '#ffffff'
            }).then(function (result) {
                if (result.isConfirmed) {
                    edicionConfirmada = true;
                    eval(hrefCapturado.replace('javascript:', ''));
                }
            });
            return false;
        }

        function confirmarGuardar(btn) {
            if (guardadoConfirmado) { guardadoConfirmado = false; return true; }
            var codigo = document.getElementById('<%= txtCodigo.ClientID %>').value.trim();
            var nombre = document.getElementById('<%= txtNombre.ClientID %>').value.trim();
            if (codigo === '' || nombre === '') { return true; }
            var esActualizar = btn.value.indexOf('Actualizar') >= 0;
            var btnName = btn.name;
            Swal.fire({
                title: esActualizar ? '¿Actualizar cambios?' : '¿Registrar nuevo centro?',
                text: esActualizar ? 'Se modificarán los datos del centro seleccionado.' : 'El centro se guardará en el sistema.',
                icon: 'info',
                showCancelButton: true,
                confirmButtonColor: '#39b54a',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Confirmar',
                cancelButtonText: 'Cancelar',
                background: '#111827',
                color: '#ffffff'
            }).then(function (result) {
                if (result.isConfirmed) {
                    guardadoConfirmado = true;
                    document.getElementsByName(btnName)[0].click();
                }
            });
            return false;
        }

        window.addEventListener('DOMContentLoaded', function () {
            var tipo = document.getElementById('<%= hfMensajeTipo.ClientID %>').value;
            var txt = document.getElementById('<%= hfMensajeTxt.ClientID %>').value;
            if (!tipo) return;
            if (tipo === 'success') {
                Swal.fire({
                    icon: 'success', title: '¡Éxito!', text: txt,
                    confirmButtonColor: '#39b54a',
                    background: '#111827', color: '#ffffff',
                    timer: 2500, showConfirmButton: false
                });
            } else if (tipo === 'error' || tipo === 'warning') {
                Swal.fire({
                    icon: tipo,
                    title: tipo === 'error' ? 'Error' : 'Atención',
                    text: txt,
                    confirmButtonColor: '#39b54a',
                    background: '#111827', color: '#ffffff'
                });
            }
        });

    </script>
</body>
</html>

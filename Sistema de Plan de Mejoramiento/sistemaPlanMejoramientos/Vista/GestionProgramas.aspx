<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionProgramas.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.GestionProgramas" ValidateRequest="false" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gestión de Programas - SENA</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="Css/gestionProgramas.css" rel="stylesheet" />
    <style>
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
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfMensajeTipo" runat="server" Value="" />
        <asp:HiddenField ID="hfMensajeTxt" runat="server" Value="" />

        <div class="container mt-5">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2><i class="bi bi-journal-bookmark-fill me-2"></i>Programas de Formación</h2>
                <asp:LinkButton ID="lnkVolver" runat="server" CssClass="btn btn-outline-light btn-sm" OnClick="lnkVolver_Click">
                    <i class="bi bi-arrow-left"></i> Volver al Dashboard
                </asp:LinkButton>
            </div>

            <div class="row">

                <div class="col-md-4 mb-4">
                    <div class="card card-custom p-4 text-white">
                        <h4 class="mb-3 text-center">
                            <asp:Label ID="lblTituloForm" runat="server" Text="Registrar Programa"></asp:Label>
                        </h4>

                        <asp:HiddenField ID="hfIdPrograma" runat="server" />

                        <div class="mb-2">
                            <label class="form-label small">Código del Programa <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" placeholder="Ej: 228106"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Nombre del Programa <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: ADSO"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Versión</label>
                            <asp:TextBox ID="txtVersion" runat="server" CssClass="form-control" placeholder="Ej: 1 o 2"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Nivel de Formación <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-select">
                                <asp:ListItem Value="" Text="-- Seleccione Nivel --"></asp:ListItem>
                                <asp:ListItem Value="Tecnólogo" Text="Tecnólogo"></asp:ListItem>
                                <asp:ListItem Value="Técnico" Text="Técnico"></asp:ListItem>
                                <asp:ListItem Value="Operario" Text="Operario"></asp:ListItem>
                                <asp:ListItem Value="Auxiliar" Text="Auxiliar"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Duración (Meses u Horas)</label>
                            <asp:TextBox ID="txtDuracion" runat="server" CssClass="form-control" placeholder="Ej: 27 meses"></asp:TextBox>
                        </div>
                        <div class="mb-2">
                            <label class="form-label small">Centro de Formación <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlCentro" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label class="form-label small">Estado</label>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                <asp:ListItem Value="Activo" Text="Activo"></asp:ListItem>
                                <asp:ListItem Value="Inactivo" Text="Inactivo"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Programa" CssClass="btn btn-sena"
                                OnClick="btnGuardar_Click" OnClientClick="return confirmarGuardar(this);" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary btn-sm"
                                Visible="false" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                </div>

                <div class="col-md-8">
                    <div class="card card-custom p-4">
                        <h4 class="mb-3 text-light">Programas Registrados</h4>

                        <div class="row mb-4">
                            <div class="col-md-9 col-lg-8">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-text bg-dark border-secondary text-secondary">
                                        <i class="bi bi-search"></i>
                                    </span>
                                    <asp:TextBox ID="txtBuscar" runat="server"
                                        CssClass="form-control bg-dark border-secondary text-white"
                                        placeholder="Buscar por código, nombre, nivel, estado...">
                                    </asp:TextBox>
                                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-sena" OnClick="btnBuscar_Click">
                                        Buscar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiarBuscar" runat="server" CssClass="btn btn-outline-secondary"
                                        OnClick="btnLimpiarBuscar_Click" ToolTip="Limpiar Filtro">
                                        <i class="bi bi-arrow-clockwise"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gvProgramas" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-custom table-hover table-bordered align-middle text-white"
                                OnRowCommand="gvProgramas_RowCommand"
                                OnRowDataBound="gvProgramas_RowDataBound"
                                OnPageIndexChanging="gvProgramas_PageIndexChanging"
                                DataKeyNames="idPrograma"
                                AllowPaging="True"
                                PageSize="10"
                                PagerStyle-CssClass="d-none">
                                <Columns>
                                    <asp:BoundField DataField="idPrograma" HeaderText="ID" ItemStyle-Width="45px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="codigoPrograma" HeaderText="Código" ItemStyle-Width="80px" />
                                    <asp:BoundField DataField="nombre" HeaderText="Programa" />
                                    <asp:BoundField DataField="version" HeaderText="Ver." ItemStyle-Width="45px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="nivel" HeaderText="Nivel" ItemStyle-Width="110px" />
                                    <asp:BoundField DataField="nombreCentro" HeaderText="Centro" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="duracion" HeaderText="Duracion" ItemStyle-Width="140px" />
                                    <asp:BoundField DataField="estado" HeaderText="Estado" ItemStyle-Width="75px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar"
                                                CommandArgument='<%# Eval("idPrograma") %>' CssClass="btn btn-warning btn-sm me-1"
                                                OnClientClick="return confirmarEditar(this);">
                                                <i class="bi bi-pencil-square"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar"
                                                CommandArgument='<%# Eval("idPrograma") %>' CssClass="btn btn-danger btn-sm"
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

        function confirmarGuardar(btn) {
            if (guardadoConfirmado) { guardadoConfirmado = false; return true; }
            var codigo = document.getElementById('<%= txtCodigo.ClientID %>').value.trim();
            var nombre = document.getElementById('<%= txtNombre.ClientID %>').value.trim();
            if (codigo === '' || nombre === '') { return true; }
            var esActualizar = btn.value.indexOf('Actualizar') >= 0;
            Swal.fire({
                title: esActualizar ? '¿Actualizar cambios?' : '¿Registrar nuevo programa?',
                text: esActualizar ? 'Se modificarán los datos del programa seleccionado.' : 'El programa se guardará en el sistema.',
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
                    btn.click();
                }
            });
            return false;
        }

        function confirmarEliminar(btn) {
            if (eliminacionConfirmada) { eliminacionConfirmada = false; return true; }
            Swal.fire({
                title: '¿Eliminar programa?',
                text: 'Esta acción no se puede deshacer y puede afectar fichas asociadas.',
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
                    if (btn.href && btn.href.indexOf('javascript:') === 0) {
                        eval(btn.href.substr(11));
                    } else { btn.click(); }
                }
            });
            return false;
        }

        function confirmarEditar(btn) {
            if (edicionConfirmada) { edicionConfirmada = false; return true; }
            Swal.fire({
                title: '¿Cargar programa para modificar?',
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
                    if (btn.href && btn.href.indexOf('javascript:') === 0) {
                        eval(btn.href.substr(11));
                    } else { btn.click(); }
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

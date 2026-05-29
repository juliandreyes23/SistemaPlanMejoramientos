<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionAprendices.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.GestionAprendices" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Gestión de Aprendices</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="Css/gestionAprendices.css" rel="stylesheet" />
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

        <asp:HiddenField ID="hfAbrirModal" runat="server" Value="" />
        <asp:HiddenField ID="hfIdAprendiz" runat="server" />
        <asp:HiddenField ID="hfMensajeTipo" runat="server" Value="" />
        <asp:HiddenField ID="hfMensajeTxt" runat="server" Value="" />

        <div class="container-fluid py-4">

            <div class="d-flex align-items-center mb-3 gap-2">
                <a href="Dashboard.aspx" class="btn btn-outline-secondary btn-sm px-3">
                    <i class="bi bi-arrow-left-circle me-1"></i>Volver al Dashboard
                </a>
            </div>

            <div class="card-custom p-4">
                <div class="d-flex justify-content-between align-items-center container-header mb-4">
                    <span>Listado General de Aprendices</span>
                    <asp:LinkButton ID="btnNuevo" runat="server" CssClass="btn btn-sena btn-sm px-4" OnClick="btnCancelar_Click">
                        <i class="bi bi-person-plus-fill me-1"></i> Nuevo Aprendiz
                    </asp:LinkButton>
                </div>

                <div class="row mb-4">
                    <div class="col-md-6 col-lg-4">
                        <div class="input-group input-group-sm">
                            <span class="input-group-text bg-dark border-secondary text-secondary">
                                <i class="bi bi-search"></i>
                            </span>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control bg-dark border-secondary text-white"
                                placeholder="Buscar por documento, nombre, ficha..."
                                AutoPostBack="true" OnTextChanged="txtBuscar_TextChanged"></asp:TextBox>
                            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-sena" OnClick="btnBuscar_Click">
                                Buscar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnLimpiarBuscar" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnLimpiarBuscar_Click" ToolTip="Limpiar Filtro">
                                <i class="bi bi-arrow-clockwise"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>

                <div class="table-responsive-custom">
                    <asp:GridView ID="gvAprendices" runat="server" AutoGenerateColumns="False"
                        CssClass="table-custom text-white"
                        OnRowCommand="gvAprendices_RowCommand"
                        OnRowDataBound="gvAprendices_RowDataBound"
                        OnPageIndexChanging="gvAprendices_PageIndexChanging"
                        DataKeyNames="idFicha,idUsuario,idAprendiz"
                        AllowPaging="True"
                        PageSize="10"
                        PagerStyle-CssClass="d-none"
                        EmptyDataText="<div class='text-center p-4 text-muted'><i class='bi bi-exclamation-circle me-2'></i>No se encontraron aprendices registrados en el sistema.</div>"
                        EmptyDataRowStyle-CssClass="border-0">
                        <Columns>
                            <asp:BoundField DataField="idAprendiz" HeaderText="ID" ItemStyle-Width="40px" />
                            <asp:BoundField DataField="tipoDocumento" HeaderText="Tipo" />
                            <asp:BoundField DataField="numeroDocumento" HeaderText="Documento" />
                            <asp:BoundField DataField="nombres" HeaderText="Nombres" />
                            <asp:BoundField DataField="apellidos" HeaderText="Apellidos" />
                            <asp:BoundField DataField="correo" HeaderText="Correo" />
                            <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                            <asp:BoundField DataField="estadoAcademico" HeaderText="Estado" />
                            <asp:BoundField DataField="codigoFicha" HeaderText="Ficha" />
                            <asp:BoundField DataField="CorreoUsuario" HeaderText="Usuario" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server"
                                        CommandName="Editar"
                                        CommandArgument='<%# Container.DataItemIndex %>'
                                        CssClass="btn btn-sm btn-warning border-0 me-1"
                                        ToolTip="Editar aprendiz">
                                        <i class="bi bi-pencil-fill"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminar" runat="server"
                                        CommandName="Eliminar"
                                        CommandArgument='<%# Eval("idAprendiz") %>'
                                        CssClass="btn btn-sm btn-danger border-0"
                                        ToolTip="Eliminar aprendiz">
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

        <div class="modal fade" id="modalAprendiz" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content modal-dark">
                    <div class="modal-header border-bottom-sena">
                        <h5 class="modal-title">
                            <asp:Label ID="lblTituloForm" runat="server" Text="Registrar Aprendiz" CssClass="text-white"></asp:Label>
                        </h5>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="pnlAvisoUsuario" runat="server" CssClass="alert alert-info py-2 mt-2 mb-0" Visible="false">
                            <i class="bi bi-info-circle me-1"></i>
                            Se creará automáticamente un usuario con correo del aprendiz
                            y <strong>contraseña = número de documento</strong>.
                        </asp:Panel>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label">Tipo Documento</label>
                                <asp:DropDownList ID="ddlTipoDoc" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="" Text="-- Seleccione --"></asp:ListItem>
                                    <asp:ListItem Value="Cédula de Ciudadanía" Text="Cédula de Ciudadanía"></asp:ListItem>
                                    <asp:ListItem Value="Tarjeta de Identidad" Text="Tarjeta de Identidad"></asp:ListItem>
                                    <asp:ListItem Value="Cédula de Extranjería" Text="Cédula de Extranjería"></asp:ListItem>
                                    <asp:ListItem Value="PEP" Text="PEP"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Número Documento</label>
                                <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" placeholder="Ej: 1023..."></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Nombres</label>
                                <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Apellidos</label>
                                <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Correo Electrónico</label>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Teléfono</label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Estado Académico</label>
                                <asp:DropDownList ID="ddlEstadoAcademico" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="" Text="-- Seleccione Estado --"></asp:ListItem>
                                    <asp:ListItem Value="En formación" Text="En formación"></asp:ListItem>
                                    <asp:ListItem Value="Aplazado" Text="Aplazado"></asp:ListItem>
                                    <asp:ListItem Value="Desertado" Text="Desertado"></asp:ListItem>
                                    <asp:ListItem Value="Retiro Voluntario" Text="Retiro Voluntario"></asp:ListItem>
                                    <asp:ListItem Value="Condicionado" Text="Condicionado"></asp:ListItem>
                                    <asp:ListItem Value="Cancelado" Text="Cancelado"></asp:ListItem>
                                    <asp:ListItem Value="Certificado" Text="Certificado"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Ficha de Formación</label>
                                <asp:DropDownList ID="ddlFicha" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer border-0 gap-2">
                        <asp:Button ID="btnCancelar" runat="server"
                            Text="Cancelar"
                            CssClass="btn btn-secondary px-4"
                            OnClick="btnCancelar_Click"
                            formnovalidate="formnovalidate"
                            OnClientClick="cerrarModal();" />
                        <asp:Button ID="btnGuardar" runat="server"
                            Text="Guardar Aprendiz"
                            CssClass="btn btn-sena px-4"
                            OnClick="btnGuardar_Click" />
                    </div>
                </div>
            </div>
        </div>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
    <script type="text/javascript">

        var modalInstance = null;
        var eliminacionConfirmada = false;

        function getModal() {
            var el = document.getElementById('modalAprendiz');
            if (!modalInstance && el) {
                modalInstance = new bootstrap.Modal(el, { backdrop: 'static', keyboard: false });
            }
            return modalInstance;
        }

        function abrirModalCrear() {
            getModal().show();
        }

        function cerrarModal() {
            if (modalInstance) modalInstance.hide();
        }

        function confirmarEliminar(btn) {
            if (eliminacionConfirmada) {
                eliminacionConfirmada = false;
                return true;
            }
            Swal.fire({
                title: '¿Eliminar aprendiz?',
                text: 'Esta acción no se puede deshacer.',
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
                    btn.click();
                }
            });
            return false;
        }

        window.addEventListener('DOMContentLoaded', function () {

            var hfAbrirModal = document.getElementById('<%= hfAbrirModal.ClientID %>');
            var hfMensajeTipo = document.getElementById('<%= hfMensajeTipo.ClientID %>');
            var hfMensajeTxt = document.getElementById('<%= hfMensajeTxt.ClientID %>');

            var tipo = hfMensajeTipo ? hfMensajeTipo.value : '';
            var txt = hfMensajeTxt ? hfMensajeTxt.value : '';

            var debeAbrirModal = hfAbrirModal && (hfAbrirModal.value === '1' || hfAbrirModal.value === 'editar' || hfAbrirModal.value === 'crear');

            if (tipo === 'success') {
                Swal.fire({
                    icon: 'success',
                    title: '¡Éxito!',
                    text: txt,
                    confirmButtonColor: '#39b54a',
                    background: '#111827',
                    color: '#ffffff',
                    timer: 2500,
                    showConfirmButton: false
                });
            } else if (tipo === 'error' || tipo === 'warning') {
                Swal.fire({
                    icon: tipo,
                    title: tipo === 'error' ? 'Error' : 'Atención',
                    text: txt,
                    confirmButtonColor: '#39b54a',
                    background: '#111827',
                    color: '#ffffff'
                }).then(function () {
                    if (debeAbrirModal) {
                        getModal().show();
                    }
                });
                return;
            }

            if (debeAbrirModal && tipo === '') {
                getModal().show();
            }
        });

    </script>
</body>
</html>

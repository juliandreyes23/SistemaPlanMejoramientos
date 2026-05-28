<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmUsuarios.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.FrmUsuarios" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Gestión de Usuarios - SENA</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="Css/frmUsuarios.css" rel="stylesheet" />
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

        <div class="container-fluid mt-5 px-4">

            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2><i class="bi bi-people-fill me-2"></i>Administración de Usuarios</h2>
                <asp:LinkButton ID="lnkVolver" runat="server" CssClass="btn btn-outline-light btn-sm" OnClick="lnkVolver_Click">
                    <i class="bi bi-arrow-left"></i> Volver al Dashboard
                </asp:LinkButton>
            </div>

            <div class="row">

                <div class="col-xl-4 col-lg-5 mb-4">
                    <div class="card card-custom p-4">
                        <h4 class="mb-4 text-start container-header">
                            <i class="bi bi-person-plus-fill me-2" style="color: #a4cc29;"></i>
                            <asp:Label ID="lblTituloForm" runat="server" Text="Registrar Usuario"></asp:Label>
                        </h4>

                        <asp:HiddenField ID="hfIdUsuario" runat="server" />

                        <div class="mb-3">
                            <label class="form-label small">Correo Electrónico</label>
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email" placeholder="ejemplo@correo.com"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="form-label small">Contraseña</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Mínimo 4 caracteres"></asp:TextBox>
                            <small id="lblInfoPassword" runat="server" visible="false" class="text-info d-block mt-1">Dejar en blanco si no desea cambiarla.</small>
                        </div>
                        <div class="mb-4">
                            <label class="form-label small">Rol del Usuario</label>
                            <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                                <asp:ListItem Value="" Text="-- Seleccione un Rol --"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Administrador"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Instructor"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Aprendiz"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Usuario" CssClass="btn btn-sena" OnClick="btnGuardar_Click" OnClientClick="return confirmarGuardar(this);" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary btn-sm" Visible="false" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                </div>

                <div class="col-xl-8 col-lg-7">
                    <div class="card card-custom p-4">
                        <h4 class="mb-4 text-light container-header">
                            <i class="bi bi-shield-lock-fill me-2" style="color: #a4cc29;"></i>Usuarios Registrados
                        </h4>

                        <div class="row mb-4">
                            <div class="col-md-8 col-lg-6">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-text bg-dark border-secondary text-secondary">
                                        <i class="bi bi-search"></i>
                                    </span>
                                    <asp:TextBox ID="txtBuscar" runat="server"
                                        CssClass="form-control bg-dark border-secondary text-white"
                                        placeholder="Buscar por ID, correo, rol..."
                                        AutoPostBack="true"
                                        OnTextChanged="txtBuscar_TextChanged">
                                    </asp:TextBox>
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
                            <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-custom align-middle"
                                OnRowCommand="gvUsuarios_RowCommand"
                                OnRowDataBound="gvUsuarios_RowDataBound"
                                OnPageIndexChanging="gvUsuarios_PageIndexChanging"
                                DataKeyNames="idUsuario"
                                AllowPaging="True"
                                PageSize="10"
                                PagerStyle-CssClass="d-none">
                                <Columns>
                                    <asp:BoundField DataField="idUsuario" HeaderText="ID" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="correo" HeaderText="Correo Electrónico" />
                                    <asp:BoundField DataField="nombreRol" HeaderText="Rol Asignado" ItemStyle-Width="160px" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar"
                                                CommandArgument='<%# Eval("idUsuario") %>' CssClass="btn btn-warning btn-sm me-1">
                                                <i class="bi bi-pencil-square"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar"
                                                CommandArgument='<%# Eval("idUsuario") %>' CssClass="btn btn-danger btn-sm">
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

        function confirmarEditar(btn) {
            if (edicionConfirmada) {
                edicionConfirmada = false;
                return true;
            }
            Swal.fire({
                title: '¿Editar usuario?',
                text: 'Se cargarán los datos para modificarlos.',
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#f59e0b',
                cancelButtonColor: '#6c757d',
                confirmButtonText: '<i class="bi bi-pencil-square"></i> Sí, editar',
                cancelButtonText: 'Cancelar',
                background: '#111827',
                color: '#ffffff'
            }).then(function (result) {
                if (result.isConfirmed) {
                    edicionConfirmada = true;
                    btn.click();
                }
            });
            return false;
        }

        function confirmarEliminar(btn) {
            if (eliminacionConfirmada) {
                eliminacionConfirmada = false;
                return true;
            }
            Swal.fire({
                title: '¿Eliminar usuario?',
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

        function confirmarGuardar(btn) {
            if (guardadoConfirmado) {
                guardadoConfirmado = false;
                return true;
            }

            var correo = document.getElementById('<%= txtCorreo.ClientID %>').value.trim();
            var rol = document.getElementById('<%= ddlRol.ClientID %>').value;
            var idUsuario = document.getElementById('<%= hfIdUsuario.ClientID %>').value;

            if (correo === "" || rol === "") {
                return true;
            }

            var esNuevo = (idUsuario === "");
            var tituloAlert = esNuevo ? '¿Registrar nuevo usuario?' : '¿Guardar cambios del usuario?';
            var textoAlert = esNuevo ? 'El usuario será añadido al sistema.' : 'Se sobrescribirán los datos actuales.';
            var btnColor = esNuevo ? '#39b54a' : '#f59e0b';
            var textoBtn = esNuevo ? '<i class="bi bi-person-plus-fill"></i> Sí, registrar' : '<i class="bi bi-check-circle-fill"></i> Sí, actualizar';

            Swal.fire({
                title: tituloAlert,
                text: textoAlert,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: btnColor,
                cancelButtonColor: '#6c757d',
                confirmButtonText: textoBtn,
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

        window.addEventListener('DOMContentLoaded', function () {
            var hfMensajeTipo = document.getElementById('<%= hfMensajeTipo.ClientID %>');
            var hfMensajeTxt = document.getElementById('<%= hfMensajeTxt.ClientID %>');

            var tipo = hfMensajeTipo ? hfMensajeTipo.value : '';
            var txt = hfMensajeTxt ? hfMensajeTxt.value : '';

            if (hfMensajeTipo) hfMensajeTipo.value = '';
            if (hfMensajeTxt) hfMensajeTxt.value = '';

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
                });
            }
        });

    </script>
</body>
</html>

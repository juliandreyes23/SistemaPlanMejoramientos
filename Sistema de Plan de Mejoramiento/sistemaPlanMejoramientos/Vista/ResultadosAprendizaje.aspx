<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadosAprendizaje.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.ResultadosAprendizaje" ValidateRequest="false" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Resultados de Aprendizaje - SENA</title>
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
                <h2><i class="bi bi-journal-check me-2"></i>Resultados de Aprendizaje</h2>
                <a href="Dashboard.aspx" class="btn btn-outline-light btn-sm">
                    <i class="bi bi-arrow-left"></i> Volver al Dashboard
                </a>
            </div>

            <div class="row">

                <div class="col-md-4 mb-4">
                    <div class="card card-custom p-4 text-white">
                        <h4 class="mb-3 text-center">
                            <asp:Label ID="lblTituloForm" runat="server" Text="Registrar Resultado"></asp:Label>
                        </h4>

                        <asp:HiddenField ID="hfIdResultado" runat="server" Value="0" />

                        <div class="mb-2">
                            <label class="form-label small">Programa <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlPrograma" runat="server"
                                CssClass="form-select"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlPrograma_SelectedIndexChanged">
                                <asp:ListItem Value="0">-- Seleccione un programa --</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="mb-2">
                            <label class="form-label small">Competencia <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlCompetencia" runat="server" CssClass="form-select">
                                <asp:ListItem Value="0">-- Seleccione una competencia --</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="mb-3">
                            <label class="form-label small">Descripción del Resultado <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control"
                                TextMode="MultiLine" Rows="4"
                                placeholder="Ej: Implementar bases de datos relacionales según requerimientos"></asp:TextBox>
                        </div>

                        <div class="d-grid gap-2">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Resultado" CssClass="btn btn-sena"
                                OnClick="btnGuardar_Click" OnClientClick="return confirmarGuardar(this);" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary btn-sm"
                                Visible="false" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                </div>

                <div class="col-md-8">
                    <div class="card card-custom p-4">
                        <h4 class="mb-3 text-light">Resultados Registrados</h4>

                        <div class="row mb-4">
                            <div class="col-md-9 col-lg-8">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-text bg-dark border-secondary text-secondary">
                                        <i class="bi bi-search"></i>
                                    </span>
                                    <asp:TextBox ID="txtBuscar" runat="server"
                                        CssClass="form-control bg-dark border-secondary text-white"
                                        placeholder="Buscar por descripción o competencia...">
                                    </asp:TextBox>
                                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-sena" OnClick="btnBuscar_Click">
                                        Buscar
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiarBuscar" runat="server" CssClass="btn btn-outline-secondary"
                                        OnClick="btnRefresh_Click" ToolTip="Limpiar Filtro">
                                        <i class="bi bi-arrow-clockwise"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gvResultados" runat="server"
                                AutoGenerateColumns="False"
                                CssClass="table table-custom table-hover table-bordered align-middle text-white"
                                OnRowCommand="gvResultados_RowCommand"
                                OnRowDataBound="gvResultados_RowDataBound"
                                OnPageIndexChanging="gvResultados_PageIndexChanging"
                                DataKeyNames="idResultadoAprendizaje"
                                AllowPaging="True"
                                PageSize="10"
                                PagerStyle-CssClass="d-none"
                                GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="idResultadoAprendizaje" HeaderText="ID" ItemStyle-Width="45px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="DescripcionResultado" HeaderText="Resultado de Aprendizaje" />
                                    <asp:BoundField DataField="DescripcionCompetencia" HeaderText="Competencia" ItemStyle-Width="180px" />
                                    <asp:BoundField DataField="NombrePrograma" HeaderText="Programa" ItemStyle-Width="140px" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar"
                                                CommandArgument='<%# Eval("idResultadoAprendizaje") %>' CssClass="btn btn-warning btn-sm me-1"
                                                OnClientClick="return confirmarEditar(this);">
                                                <i class="bi bi-pencil-square"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar"
                                                CommandArgument='<%# Eval("idResultadoAprendizaje") %>' CssClass="btn btn-danger btn-sm"
                                                OnClientClick="return confirmarEliminar(this);">
                                                <i class="bi bi-trash-fill"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="text-center text-secondary py-4">
                                        No hay resultados de aprendizaje registrados.
                                    </div>
                                </EmptyDataTemplate>
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
            var desc = document.getElementById('<%= txtDescripcion.ClientID %>').value.trim();
            if (desc === '') { return true; }
            var esActualizar = btn.value.indexOf('Actualizar') >= 0;
            Swal.fire({
                title: esActualizar ? '¿Actualizar cambios?' : '¿Registrar nuevo resultado?',
                text: esActualizar ? 'Se modificarán los datos del resultado seleccionado.' : 'El resultado se guardará en el sistema.',
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
                title: '¿Eliminar resultado?',
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
                title: '¿Cargar resultado para modificar?',
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

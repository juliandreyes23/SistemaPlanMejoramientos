<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AsignacionInstructores.aspx.cs" Inherits="sistemaPlanMejoramientos.Vista.AsignacionInstructores" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Asignación Instructores - Fichas</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <style>
        body {
            background-color: #0f172a;
            color: #ffffff;
            overflow-x: hidden;
            min-height: 100vh;
        }

        .glass-card {
            background: rgba(30, 41, 59, 0.7);
            backdrop-filter: blur(10px);
            border: 1px solid rgba(255,255,255,0.1);
            border-radius: 18px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.25);
        }

        .table {
            min-width: 650px;
        }

            .table thead th {
                border-bottom: 1px solid rgba(255,255,255,0.1);
            }

        .form-select, .form-control {
            height: 45px;
        }

        .btn-success {
            background-color: #39b54a;
            border: none;
        }

            .btn-success:hover {
                background-color: #2f9c3e;
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

        @media (max-width: 991px) {
            h2 {
                font-size: 1.5rem;
            }

            .glass-card {
                padding: 1rem !important;
            }

            .table {
                font-size: 0.9rem;
            }
        }

        @media (max-width: 768px) {
            h2 {
                font-size: 1.2rem;
            }

            h4 {
                font-size: 1rem;
            }

            .btn {
                font-size: 0.85rem;
            }

            .table {
                font-size: 0.8rem;
            }

            .form-select, .form-control {
                font-size: 0.9rem;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:HiddenField ID="hfMensajeTipo" runat="server" Value="" />
        <asp:HiddenField ID="hfMensajeTxt" runat="server" Value="" />

        <div class="container-fluid px-3 px-md-4 mt-4 mt-md-5">

            <div class="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-4">
                <h2 class="fw-bold m-0">
                    <i class="bi bi-diagram-3-fill me-2" style="color: #39b54a;"></i>
                    Asignación de Instructores a Fichas
                </h2>
                <asp:LinkButton ID="lnkVolver" runat="server" CssClass="btn btn-outline-light btn-sm" OnClick="lnkVolver_Click">
                    <i class="bi bi-arrow-left me-1"></i> Volver al Dashboard
                </asp:LinkButton>
            </div>

            <div class="row g-4">

                <div class="col-12 col-lg-4">
                    <div class="card glass-card p-3 p-md-4 h-100">
                        <h4 class="mb-4 text-center text-lg-start" style="color: #38bdf8;">Nueva Asignación</h4>

                        <div class="mb-3">
                            <label class="form-label small text-light">Seleccionar Instructor</label>
                            <asp:DropDownList ID="ddlInstructores" runat="server" CssClass="form-select bg-dark text-white border-secondary"></asp:DropDownList>
                        </div>

                        <div class="mb-4">
                            <label class="form-label small text-light">Seleccionar Ficha</label>
                            <asp:DropDownList ID="ddlFichas" runat="server" CssClass="form-select bg-dark text-white border-secondary"></asp:DropDownList>
                        </div>

                        <asp:Button ID="btnAsignar" runat="server" Text="Vincular Instructor"
                            CssClass="btn btn-success w-100 py-2"
                            OnClick="btnAsignar_Click"
                            OnClientClick="return confirmarAsignar(this);" />
                    </div>
                </div>

                <div class="col-12 col-lg-8">
                    <div class="card glass-card p-3 p-md-4">
                        <h4 class="mb-4 text-center text-lg-start" style="color: #39b54a;">Vínculos Actuales</h4>

                        <div class="table-responsive">
                            <asp:GridView ID="gvAsignaciones" runat="server"
                                AutoGenerateColumns="False"
                                DataKeyNames="idFichaInstructor"
                                CssClass="table table-dark table-striped table-hover align-middle"
                                GridLines="None"
                                OnRowCommand="gvAsignaciones_RowCommand"
                                OnPageIndexChanging="gvAsignaciones_PageIndexChanging"
                                EmptyDataText="No hay asignaciones registradas."
                                AllowPaging="True"
                                PageSize="10"
                                PagerStyle-CssClass="d-none">
                                <Columns>
                                    <asp:BoundField DataField="Instructor" HeaderText="Instructor" />
                                    <asp:BoundField DataField="Ficha" HeaderText="Ficha" />
                                    <asp:BoundField DataField="Programa" HeaderText="Programa" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEliminar" runat="server"
                                                CommandName="Eliminar"
                                                CommandArgument='<%# Eval("idFichaInstructor") %>'
                                                CssClass="btn btn-danger btn-sm border-0"
                                                ToolTip="Quitar asignación"
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
        var asignacionConfirmada = false;

        function confirmarEliminar(btn) {
            if (eliminacionConfirmada) {
                eliminacionConfirmada = false;
                return true;
            }
            Swal.fire({
                title: '¿Quitar asignación?',
                text: 'Se removerá el vínculo entre el instructor y la ficha.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#e3342f',
                cancelButtonColor: '#6c757d',
                confirmButtonText: '<i class="bi bi-trash-fill"></i> Sí, quitar',
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

        function confirmarAsignar(btn) {
            if (asignacionConfirmada) {
                asignacionConfirmada = false;
                return true;
            }
            Swal.fire({
                title: '¿Vincular instructor?',
                text: 'Se asignará el instructor a la ficha seleccionada.',
                icon: 'info',
                showCancelButton: true,
                confirmButtonColor: '#39b54a',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, vincular',
                cancelButtonText: 'Cancelar',
                background: '#111827',
                color: '#ffffff'
            }).then(function (result) {
                if (result.isConfirmed) {
                    asignacionConfirmada = true;
                    btn.click();
                }
            });
            return false;
        }

        window.addEventListener('DOMContentLoaded', function () {
            var hfTipo = document.getElementById('<%= hfMensajeTipo.ClientID %>');
            var hfTxt = document.getElementById('<%= hfMensajeTxt.ClientID %>');
            var tipo = hfTipo ? hfTipo.value : '';
            var txt = hfTxt ? hfTxt.value : '';
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
            } else if (tipo === 'error' || tipo === 'warning' || tipo === 'info') {
                Swal.fire({
                    icon: tipo,
                    title: tipo === 'error' ? 'Error' : tipo === 'warning' ? 'Atención' : 'Información',
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

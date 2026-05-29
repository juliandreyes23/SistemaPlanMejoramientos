<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMisPlanes.aspx.cs" Inherits="sistemaPlanMejoramientos.Aprendiz.FrmMisPlanes" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mis Planes | SENA</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Syne:wght@400;600;700;800&family=DM+Sans:wght@300;400;500&display=swap');

        :root {
            --sena-dark: #042940;
            --sena-mid: #005C53;
            --sena-accent: #9FC131;
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            font-family: 'DM Sans','Segoe UI',sans-serif;
            background: #f4f7fb;
            min-height: 100vh;
        }

        .nav {
            background: #042940;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 0 28px;
            height: 60px;
        }

        .nav-brand {
            display: flex;
            align-items: center;
            gap: 10px;
            font-family: 'Syne',sans-serif;
            font-weight: 800;
            font-size: 16px;
            color: #fff;
            text-decoration: none;
        }

        .nav-dot {
            width: 8px;
            height: 8px;
            background: #9FC131;
            border-radius: 50%;
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0%,100% {
                opacity: 1;
                transform: scale(1)
            }

            50% {
                opacity: .5;
                transform: scale(1.4)
            }
        }

        .nav-back {
            border: 1px solid rgba(255,255,255,.25);
            background: transparent;
            color: rgba(255,255,255,.8);
            font-size: 12px;
            padding: 6px 16px;
            border-radius: 20px;
            cursor: pointer;
            transition: all .2s;
            text-decoration: none;
            display: flex;
            align-items: center;
            gap: 6px;
        }

            .nav-back:hover {
                background: rgba(255,255,255,.1);
                color: #fff;
            }

        .page-header {
            background: #042940;
            padding: 28px 28px 24px;
        }

        .page-title {
            font-family: 'Syne',sans-serif;
            font-weight: 800;
            font-size: 22px;
            color: #fff;
            margin-bottom: 6px;
        }

        .page-sub {
            font-size: 13px;
            color: rgba(255,255,255,.5);
        }

        .page-body {
            padding: 24px 28px;
        }

        .plans-table-wrap {
            background: #fff;
            border: 1px solid #e0e6ed;
            border-radius: 16px;
            overflow: hidden;
        }

        .plans-table {
            width: 100%;
            border-collapse: collapse;
        }

            .plans-table thead tr {
                background: #f8fafc;
            }

            .plans-table th {
                padding: 12px 16px;
                text-align: left;
                font-size: 11px;
                font-weight: 700;
                color: #6c757d;
                text-transform: uppercase;
                letter-spacing: .8px;
                border-bottom: 1px solid #e0e6ed;
            }

            .plans-table td {
                padding: 14px 16px;
                font-size: 13px;
                color: #333;
                border-bottom: 1px solid #f0f4f8;
                vertical-align: top;
            }

            .plans-table tbody tr:last-child td {
                border-bottom: none;
            }

            .plans-table tbody tr:hover td {
                background: #f8fafc;
            }

        .badge {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            font-size: 11px;
            font-weight: 600;
            padding: 3px 10px;
            border-radius: 20px;
            white-space: nowrap;
        }

        .badge-pendiente {
            background: #FFF3CD;
            color: #856404;
        }

        .badge-aprobado {
            background: #D1E7DD;
            color: #0A5239;
        }

        .badge-noaprobado {
            background: #F8D7DA;
            color: #721C24;
        }

        .badge-interno {
            background: #E6F1FB;
            color: #185FA5;
        }

        .badge-comite {
            background: #EEEDFE;
            color: #534AB7;
        }

        .btn-subir {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: #005C53;
            color: #fff;
            font-size: 12px;
            font-weight: 600;
            padding: 6px 14px;
            border-radius: 20px;
            text-decoration: none;
            border: none;
            cursor: pointer;
            transition: all .2s;
        }

            .btn-subir:hover {
                background: #042940;
            }

            .btn-subir:disabled, .btn-subir.disabled {
                background: #ccc;
                color: #888;
                pointer-events: none;
            }

        .rap-list {
            list-style: none;
        }

            .rap-list li {
                font-size: 11px;
                color: #555;
                padding: 2px 0;
            }

                .rap-list li::before {
                    content: "• ";
                    color: #9FC131;
                    font-weight: 700;
                }

        .empty-state {
            padding: 48px 24px;
            text-align: center;
            color: #aab;
        }

            .empty-state i {
                font-size: 40px;
                display: block;
                margin-bottom: 12px;
                color: #ccd;
            }

            .empty-state p {
                font-size: 14px;
            }

        .eval-panel {
            background: #f8fafc;
            border: 1px solid #e0e6ed;
            border-radius: 10px;
            padding: 10px 12px;
            margin-top: 6px;
            font-size: 12px;
        }

        .eval-row {
            display: flex;
            gap: 8px;
            flex-wrap: wrap;
            margin-top: 4px;
        }

        @media(max-width:768px) {
            .page-body {
                padding: 16px;
            }

            .plans-table th:nth-child(4),
            .plans-table td:nth-child(4) {
                display: none;
            }
        }

        .nav {
            height: 60px;
            padding: 0 28px;
        }

        .page-header {
            padding: 28px 28px 24px;
        }

        .page-title {
            font-size: 22px;
        }

        .page-sub {
            font-size: 13px;
        }

        .page-body {
            padding: 24px 28px;
        }

        .plans-table-wrap {
            border-radius: 16px;
        }

        .plans-table th {
            padding: 12px 16px;
            font-size: 11px;
        }

        .plans-table td {
            padding: 14px 16px;
            font-size: 13px;
        }

        .badge {
            font-size: 11px;
            padding: 3px 10px;
            border-radius: 20px;
        }

        .btn-subir {
            font-size: 12px;
            padding: 6px 14px;
            border-radius: 20px;
        }

        .eval-panel {
            padding: 10px 12px;
            border-radius: 10px;
            margin-top: 6px;
        }

        .empty-state {
            padding: 48px 24px;
        }

            .empty-state i {
                font-size: 40px;
            }

        @media(max-width:768px) {
            .page-body {
                padding: 16px;
            }

            .plans-table td,
            .plans-table th {
                padding: 12px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="nav">
            <div class="nav-brand">
                <div class="nav-dot"></div>
                SENA Aprendiz
       
            </div>
            <a href="DashboardAprendiz.aspx" class="nav-back">
                <i class="bi bi-arrow-left"></i>Volver al Dashboard
        </a>
        </nav>

        <div class="page-header">
            <div class="page-title"><i class="bi bi-journal-text" style="margin-right: 8px;"></i>Mis Planes de Mejoramiento</div>
            <div class="page-sub">Aquí puedes ver todos los planes que te han asignado y subir tus evidencias.</div>
        </div>

        <div class="page-body">

            <asp:Label ID="lblMensaje" runat="server" Visible="false"
                Style="display: block; padding: 12px 16px; border-radius: 10px; margin-bottom: 16px; font-size: 13px;"></asp:Label>

            <div class="plans-table-wrap">
                <asp:Repeater ID="rptPlanes" runat="server" OnItemDataBound="rptPlanes_ItemDataBound">
                    <HeaderTemplate>
                        <table class="plans-table">
                            <thead>
                                <tr>
                                    <th>Tipo</th>
                                    <th>Instructor</th>
                                    <th>Actividades</th>
                                    <th>Fecha Límite</th>
                                    <th>Estado</th>
                                    <th>Evidencias</th>
                                    <th>Acción</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <span class='<%# ObtenerCssTipo(Eval("tipoPlan").ToString()) %>'>
                                    <%# Eval("tipoPlan") %>
                            </span>
                            </td>
                            <td><%# Eval("nombreInstructor") %></td>
                            <td style="max-width: 200px;">
                                <%# Eval("actividades") %>
                                <asp:Literal ID="litResultados" runat="server"></asp:Literal>
                            </td>
                            <td><%# Convert.ToDateTime(Eval("fechaLimite")).ToString("dd/MM/yyyy") %></td>
                            <td>
                                <span class='<%# ObtenerCssEstado(Eval("estadoPlan").ToString()) %>'>
                                    <%# Eval("estadoPlan") %>
                            </span>
                                <asp:Literal ID="litEvaluacion" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: center;">
                                <asp:Label ID="lblEvidencias" runat="server"
                                    Text='<%# Eval("totalEvidencias") %>'
                                    Style="font-weight: 700; color: #042940; font-size: 15px;"></asp:Label>
                            </td>
                            <td>
                                <asp:HyperLink ID="hlSubir" runat="server"
                                    NavigateUrl='<%# "FrmSubirEvidencia.aspx?idPlan=" + Eval("idPlanMejoramiento") %>'
                                    CssClass='<%# Eval("estadoPlan").ToString()=="Aprobado" || Eval("estadoPlan").ToString()=="No Aprobado" ? "btn-subir disabled" : "btn-subir" %>'>
                                <i class="bi bi-cloud-upload"></i> Subir
                            </asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody></table>
                   
                        <asp:Literal ID="litEmpty" runat="server"></asp:Literal>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

        </div>

    </form>
</body>
</html>

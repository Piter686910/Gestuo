<%@ Page Title="Dashboard Amministratore" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="GestUO._Dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="container-fluid mt-4">
        <div class="dashboard-header">
            <h1><span class="fa-solid fa-gear fa-spin"></span><u><%= TitoloUO %> </u></h1>
        </div>

        <asp:Panel ID="pnlGestUtenti" runat="server">

            <div class="row">
                <div id="divNewUtente" runat="server" visible="false" class="col-md-6">
                    <div class="section-box">
                        <h4 class="mb-4" style="color: #337ab7; font-weight: bold;">Dati Operatore</h4>
                        <div class="form-group mb-3">
                            <asp:Label ID="lblm" runat="server" Text="Matricola" />
                            <asp:TextBox ID="TxtMatricola" runat="server" CssClass="form-control" />
                        </div>
                        <%--                        <div class="form-group mb-3">
                            <asp:Label ID="Label1" runat="server" Text="Profilo" />
                            <asp:TextBox ID="TxtProfilo" runat="server" CssClass="form-control" />
                        </div>--%>
                        <%--<div class="form-group mb-3">
                            <asp:Label ID="Label2" runat="server" Text="Area" />
                            <asp:TextBox ID="txtArea" runat="server" CssClass="form-control" />
                        </div>--%>

                        <div class="form-group mb-3">
                            <asp:Label ID="Label7" runat="server" Text="Nominativo" />
                            <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label1" runat="server" Text="Grado" />
                            <asp:DropDownList ID="ddlGrado" runat="server" CssClass="form-control">
                                <asp:ListItem Text="AG" />
                                <asp:ListItem Text="AC" />
                                <asp:ListItem Text="ASS" />
                                <asp:ListItem Text="M.LLO" />
                                <asp:ListItem Text="L.TEN" />
                                <asp:ListItem Text="S.TEN" />
                                <asp:ListItem Text="CAP" />
                            </asp:DropDownList>
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label4" runat="server" Text="Unità Operativa" />
                            <asp:TextBox ID="txtUO" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <div id="divDestra" runat="server" visible="false" class="col-md-6">
                    <div class="section-box">
                        <h4 class="mb-4" style="color: #337ab7; font-weight: bold;">Configurazione Ruolo</h4>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label5" runat="server" Text="Nota" />
                            <asp:TextBox ID="TxtNota" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
                        </div>
                        <div class="form-group mb-3">
                            <asp:Label ID="Label6" runat="server" Text="Ruolo" />
                            <asp:DropDownList ID="DdlRuolo" runat="server" CssClass="form-control">
                                <asp:ListItem Text="admin" />
                                <asp:ListItem Text="operatori" />
                                <asp:ListItem Text="archivio" />
                                <asp:ListItem Text="coordinamentoatti" />
                                <asp:ListItem Text="Fureria" />
                                <asp:ListItem Text="PG" />

                            </asp:DropDownList>
                        </div>
                        <div class="form-group mb-4">
                            <asp:Label ID="Label8" runat="server" Text="Elenco Personale" />
                            <asp:DropDownList ID="DdlPersonale" runat="server" CssClass="form-control" />
                        </div>
                        <asp:Button Text="SALVA OPERATORE" runat="server" OnClick="InsOpetratore_Click" CssClass="btn btn-primary w-100" />
                    </div>
                </div>
            </div>

            <div id="divCheck" runat="server" visible="false" class="section-box">
                <div class="row d-flex align-items-end">
                    <div class="col-md-4">
                        <div class="form-group mb-0">
                            <asp:Label ID="Label9" runat="server" Text="Numero Pratica" />
                            <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-0">
                            <asp:Label ID="Label10" runat="server" Text="Anno" />
                            <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-0">
                            <asp:Button Text="AVVIA RICERCA" runat="server" OnClick="Cerca_Click" CssClass="btn btn-primary w-100 btn-allineato" />
                        </div>
                    </div>
                </div>

                <div class="table-responsive mt-4">
                    <asp:GridView ID="GVcheck" runat="server"
                        CssClass="table table-bordered table-hover compact-grid"
                        OnRowDataBound="GVcheck_RowDataBound"
                        OnRowCommand="GVcheck_RowCommand"
                        AllowPaging="true" PageSize="10"
                        OnPageIndexChanging="GVcheck_PageIndexChanging"
                        RowStyle-CssClass="GridViewRow"
                        AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                    </asp:GridView>
                </div>
            </div>

            <div id="divReset" runat="server" visible="false" class="section-box text-center">
                <div class="row justify-content-center">
                    <div class="col-md-4">
                        <asp:Label ID="Label3" runat="server" Text="Matricola per Reset" CssClass="fw-bold" />
                        <asp:TextBox ID="txtResetMatricola" runat="server" CssClass="form-control mt-2" />
                        <div class="mt-3">
                            <%--<asp:Button Text="RESET PASSWORD" runat="server" OnClick="ModificaP_Click" CssClass="btn btn-warning px-4" />--%>

                            <%--<asp:Button Text="ELIMINA UTENTE" runat="server" OnClick="Elimina_Click" CssClass="btn btn-danger px-4" />--%>
                            <asp:Button Text="" runat="server" OnClick="Elimina_Click" CssClass="btn btn-danger px-4" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group text-center mt-4 mb-5">
                <asp:Button Text="Reset Password" runat="server" OnClick="Reset_Click" CssClass="btn btn-primary mx-2" Visible="false" />


                <asp:Panel ID="pnlButton" runat="server" CssClass="" Visible="true">
                    <div class="d-flex justify-content-center align-items-center w-100 my-4">
                        <table style="margin: 0 auto; border-collapse: separate; border-spacing: 6px;">
                            <tr>
                                <td>
                                    <asp:Button Text="Nuovo Utente" runat="server" OnClick="NuovoUt_Click" CssClass="btn btn-primary btn-custom-size" /></td>
                                <td>
                                    <asp:Button Text="Verifica Accessi" runat="server" OnClick="Check_Click" CssClass="btn btn-primary btn-custom-size" /></td>
                                <td>
                                    <asp:Button Text="Lista" runat="server" OnClick="Lista_Click" CssClass="btn btn-primary btn-custom-size" /></td>
                            </tr>

                        </table>
                    </div>

                </asp:Panel>







            </div>
            <%-- Repeater operatori --%>
            <!-- FILTRI DI RICERCA -->
            <!-- CONTROLLI PAGINAZIONE -->
            <div class="d-flex justify-content-between align-items-center mt-3 pt-2 border-top">
                <div>
                    <asp:Label ID="lblPageInfo" runat="server" CssClass="text-muted fw-bold"></asp:Label>
                </div>

            </div>
            <div class="card mb-3 p-3 shadow-sm bg-light">
                <!-- RIGA FILTRI E PULSANTI AFFIANCATI -->
                <div class="row g-2 align-items-center">
                    <div class="col-md-4">
                        <asp:TextBox ID="txtFiltroNominativo" runat="server" CssClass="form-control" Placeholder="Filtra per Nominativo..."></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtFiltroMatricola" runat="server" CssClass="form-control" Placeholder="Filtra per Matricola..."></asp:TextBox>
                    </div>
                    <!-- Pulsanti Cerca e Reset affiancati -->
                    <div class="col-md-5 d-flex gap-2">
                        <asp:Button ID="btnCercaFiltri" runat="server" Text="Cerca" CssClass="btn btn-primary w-50" OnClick="btnCercaFiltri_Click" />
                        <asp:Button ID="btnResetFiltri" runat="server" Text="Reset" CssClass="btn btn-outline-secondary w-50" OnClick="btnResetFiltri_Click" />
                    </div>
                </div>

                <div class="d-flex gap-1">
                    <asp:Button ID="btnPrev" runat="server" Text="« Precedente" OnClick="btnPrev_Click" CssClass="btn btn-sm btn-outline-secondary" ForeColor="#0066ff" Font-Bold="true" />
                    <asp:Button ID="btnNext" runat="server" Text="Successivo »" OnClick="btnNext_Click" CssClass="btn btn-sm btn-outline-secondary" ForeColor="#0066ff" Font-Bold="true" />
                </div>
            </div>
            <%-- Repeater operatori --%>
            <div id="divRepeater" runat="server" class="table-responsive shadow-sm rounded">
                <asp:Repeater ID="rptOperatori" runat="server" OnItemCommand="rptOperatori_ItemCommand">
                    <HeaderTemplate>
                        <table class="table table-hover table-striped align-middle mb-0 bg-white">
                            <thead class="table-dark">
                                <tr>
                                    <th scope="col">Nominativo</th>
                                    <th scope="col">Matricola</th>
                                    <th scope="col">Ruolo</th>
                                    <th scope="col">Grado</th>
                                    

                                    <%-- <th scope="col">Macro Area</th>--%>
                                    <th scope="col" style="width: 10%;">Abilitato</th>
                                    <th scope="col" class="text-center" style="width: 15%;">Azioni</th>
                                      <th></th>  <%-- Cella vuota per allineamento con la colonna Abilitato --%>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="d-flex align-items-center">
                                    <i class="bi bi-person-circle fs-5 me-2 text-primary"></i>
                                    <span class="fw-semibold"><%# Eval("Nominativo") %> </span>
                                </div>
                            </td>
                            <td>
                                <span class="badge bg-light text-dark border"><%# Eval("Matricola") %></span>
                            </td>

                            <asp:PlaceHolder ID="phView" runat="server" Visible="true">
                                <%-- VISUALIZZAZIONE --%>
                                <td><code><%# Eval("ruolo") %></code></td>
                                <td><code><%# Eval("grado") %></code></td>
                                <%--<td><code><%# Eval("macroarea") %></code></td>--%>
                                <td>
                                    <span class="badge <%# Eval("abilitato").ToString() == "True" ? "bg-success" : "bg-danger" %>">
                                        <%# Eval("abilitato").ToString() == "True" ? "Si" : "No" %>
                                    </span>
                                </td>
                                <td class="text-center">
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-warning" ToolTip="Modifica">
                            <i class="bi bi-pencil-square"></i>Modifica
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDisabilita" runat="server" CssClass="btn btn-sm btn-outline-primary ms-1"
                                        CommandArgument='<%# Eval("Matricola") + "|" + Eval("abilitato")  %>' OnClick="btnDisabilita_Click">
                                        <i class="bi bi-power"></i>Abilita/Disabilita
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-sm btn-outline-primary ms-1"
                                        CommandArgument='<%# Eval("Matricola")  %>' OnClick="ModificaPass_Click">
                                        <i class="bi bi-power"></i>Reset Password
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-outline-primary ms-1"
                                        CommandArgument='<%# Eval("Matricola")  %>' OnClick="EliminaMatricola_Click">
                                        <i class="bi bi-power" style="color:orangered"></i>Elimina
                                    </asp:LinkButton>
                                </td>
                               <%-- Cella vuota per allineamento con la colonna Abilitato --%>
                                <td></td>
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="phEdit" runat="server" Visible="false">
                                <%-- MODIFICA --%>
                                <td>
                                    <asp:TextBox ID="txtRuolo" runat="server" Text='<%# Eval("ruolo") %>' CssClass="form-control form-control-sm" /></td>
                                <td>
                                    <asp:TextBox ID="txtGrado" runat="server" Text='<%# Eval("grado") %>' CssClass="form-control form-control-sm" /></td>

                                <%-- Cella vuota per allineamento con la colonna Abilitato --%>
                                 <td></td>
                                <%--<td>
                                    <asp:TextBox ID="txtMacroArea" runat="server" Text='<%# Eval("macroarea") %>' CssClass="form-control form-control-sm" /></td>--%>
                                <td></td>
                                <td class="text-center">
                                    <asp:LinkButton ID="btnSave" runat="server" CommandName="Update" CommandArgument='<%# Eval("Matricola") %>' CssClass="btn btn-sm btn-success">
                            <i class="bi bi-check-lg"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-danger ms-1">
                            <i class="bi bi-x-lg"></i>
                                    </asp:LinkButton>
                                </td>
                            </asp:PlaceHolder>
                        </tr>
                    </ItemTemplate>

                    <FooterTemplate>
                        </tbody>
            </table>
            <asp:PlaceHolder ID="phEmpty" runat="server" Visible='<%# rptOperatori.Items.Count == 0 %>'>
                <div class="p-4 text-center border-top">
                    <i class="bi bi-info-circle text-muted fs-2"></i>
                    <p class="text-muted mt-2">Nessun dato trovato.</p>
                </div>
            </asp:PlaceHolder>
                    </FooterTemplate>
                </asp:Repeater>
            </div>


        </asp:Panel>
    </div>



</asp:Content>

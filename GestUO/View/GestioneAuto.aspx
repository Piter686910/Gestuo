<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestioneAuto.aspx.cs" Inherits="GestUO.GestioneAuto" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        /* 1. Sgancia tutti i contenitori padri di Bootstrap compresa la MasterPage */
        html, body, form, .container, .body-content, main, [class*="container"] {
            max-width: 100% !important;
            width: 100% !important;
            padding-left: 10px !important;
            padding-right: 10px !important;
            margin-left: 0 !important;
            margin-right: 0 !important;
        }

        /* 2. Forza le schede bianche interne ad allargarsi completamente */
        .section-box, #DivGrid, .jumbotron {
            width: 100% !important;
            max-width: 100% !important;
            box-sizing: border-box !important;
        }

        /* 3. Estende la tabella a tutto lo spazio disponibile */
        .table-responsive {
            width: 100% !important;
            max-width: 100% !important;
            overflow-x: auto !important;
        }

        #gvDett, .gridview-full {
            width: 100% !important;
            max-width: 100% !important;
            table-layout: auto !important; /* Espande le colonne su tutta la larghezza */
        }

        /* 4. Filtri sulle intestazioni */
        .larghezzaText {
            width: 100% !important;
            box-sizing: border-box;
            font-size: 0.8rem !important;
            padding: 2px 4px !important;
        }
    </style>

    <script>
        function ShowErrorMessage(message) {
            $('#errorMessage').text(message);
            $('#errorModal').modal('show');
        }
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }
        function ShowAvvertenze(message) {
            $('#txtAvvertenze').text(message);
            $('#ModalAvvertenze').modal('show');
        }
    </script>

    <!-- Usato container-fluid full-width per occupare il 100% effettivo della pagina -->
    <div class="container-fluid full-width-container">


        <div class="dashboard-header">
            <h1><span class="fa-solid fa-gear fa-spin"></span><u><%= TitoloUO %> </u></h1>
        </div>

        <div class="section-box w-100">
            <div class="row d-flex align-items-end">
                <div class="col-md-2">
                    <div class="form-group mb-0">
                        <label>Mese</label>
                        <asp:TextBox ID="txtMese" runat="server" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group mb-0">
                        <label>Anno</label>
                        <asp:TextBox ID="txtAnno" runat="server" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="col-md-8 text-right">
                    <asp:Button ID="btSalva" Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary ml-2" ValidationGroup="bt" />
                    <asp:Button ID="btCerca" Text="Cerca" runat="server" OnClick="btCerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary ml-2" />
                    <asp:Button ID="btStampa" Text="Stampa" runat="server" OnClick="btStampa_Click" ToolTip="Stampa" CssClass="btn btn-primary ml-2" />
                </div>
            </div>
        </div>

        <div class="section-box w-100" id="divInserimento" runat="server">
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="DdlSigla">Sigla</label>
                        <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlSigla_SelectedIndexChanged1" AutoPostBack="true" />
                    </div>
                    <div class="form-group">
                        <label for="txtAutista">Autista</label>
                        <asp:TextBox ID="txtAutista" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAutista" ValidationGroup="bt" ErrorMessage="Inserire il nome dell'autista" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="txtTarga">Targa</label>
                        <asp:TextBox ID="txtTarga" runat="server" CssClass="form-control" Font-Bold="true" />
                    </div>
                    <div class="form-group">
                        <label for="TxtData">Data</label>
                        <asp:TextBox ID="TxtData" runat="server" CssClass="form-control data-auto" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtData" ValidationGroup="bt" ErrorMessage="Inserire data" ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtData" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$" ErrorMessage="Usa dd/mm/aaaa" ForeColor="Red" ValidationGroup="bt" Display="Dynamic" />
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group">
                        <label for="DdlCarburante">Tipo Carburante</label>
                        <asp:DropDownList ID="DdlCarburante" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Benzina" />
                            <asp:ListItem Text="Diesel" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtOra">Ora</label>
                        <asp:TextBox ID="txtOra" runat="server" CssClass="form-control" placeholder="HH:MM" />
                        <asp:RegularExpressionValidator ID="RegexOra" runat="server" ControlToValidate="txtOra" ValidationExpression="^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$" ErrorMessage="Formato non valido" ForeColor="Red" ValidationGroup="bt" Display="Dynamic" />
                    </div>
                </div>
            </div>

            <div class="row mt-3">
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Stan</label>
                        <asp:TextBox ID="txtStan" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStan" ValidationGroup="bt" ErrorMessage="Inserire Stan" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Litri</label>
                        <asp:TextBox ID="txtLitri" runat="server" CssClass="form-control" placeholder="es: 10,1"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLitri" ValidationGroup="bt" ErrorMessage="Inserire Litri" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Euro</label>
                        <asp:TextBox ID="txtEuro" runat="server" CssClass="form-control" placeholder="es: 10,1"/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEuro" ValidationGroup="bt" ErrorMessage="Inserire Euro" ForeColor="Red" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <label>Indirizzo Distributore</label>
                        <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Sezione GridView estesa a 100% -->
        <div id="DivGrid" runat="server" class="section-box w-100">
            <div class="d-flex justify-content-between align-items-center mb-2 px-1">
                <div class="small text-muted">
                    <asp:Label ID="lblInfoPagine" runat="server" Text="Pagina"></asp:Label>
                    <strong>
                        <asp:Label ID="lblNumRighe" runat="server" Text=""></asp:Label>
                    </strong>
                </div>
            </div>

            <div class="table-responsive w-100">
                <asp:GridView ID="gvDett" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-bordered gridview-autofit gridview-full"
                    DataKeyNames="id" Width="100%"
                    OnRowDataBound="gvDett_RowDataBound"
                    OnRowCommand="gvDett_RowCommand"
                    OnDataBound="gvDett_DataBound"
                    OnRowEditing="gvDett_RowEditing"
                    OnRowCancelingEdit="gvDett_RowCancelingEdit"
                    OnRowUpdating="gvDett_RowUpdating"
                    AllowPaging="true" PageSize="10"
                    OnPageIndexChanging="gvDett_PageIndexChanging"
                    RowStyle-CssClass="GridViewRow"
                    AlternatingRowStyle-CssClass="GridViewAlternatingRow"
                    PagerSettings-Position="Top"
                    PagerSettings-Mode="NextPreviousFirstLast"
                    PagerSettings-FirstPageText="&laquo; Prima"
                    PagerSettings-LastPageText="Ultima &raquo;"
                    PagerSettings-NextPageText="Succ. &rsaquo;"
                    PagerSettings-PreviousPageText="&lsaquo; Prec.">

                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />

                        <asp:TemplateField HeaderText="Sigla">
                            <HeaderTemplate>
                                Sigla<br />
                                <asp:TextBox ID="txtFilterSigla" runat="server" OnTextChanged="txtFilterSigla_TextChanged" AutoPostBack="True" CssClass="larghezzaText" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("sigla") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSiglaMod" runat="server" Text='<%# Bind("sigla") %>' CssClass="form-control input-sm" OnTextChanged="txtSiglaMod_TextChanged"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Targa">
                            <HeaderTemplate>
                                Targa<br />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Targa") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTargaMod" runat="server" Text='<%# Bind("Targa") %>' CssClass="form-control input-sm" Enabled="false"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Stan">
                            <HeaderTemplate>
                                Stan<br />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("Stan") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStanMod" runat="server" Text='<%# Bind("Stan") %>' CssClass="form-control input-sm"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Data">
                            <HeaderTemplate>
                                Data<br />
                                <asp:TextBox ID="txtFilterData" runat="server" OnTextChanged="txtFilterData_TextChanged" AutoPostBack="True" CssClass="larghezzaText" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("data", "{0:dd/MM/yyyy}") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDataMod" runat="server" Text='<%# Bind("data", "{0:dd/MM/yyyy}") %>' CssClass="form-control input-sm"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Ora">
                            <ItemTemplate>
                                <%# Eval("ora") != null ? Eval("ora").ToString().Substring(0, 5) : "" %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOraMod" runat="server" Text='<%# Bind("ora") %>' CssClass="form-control input-sm"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Litri">
                            <HeaderTemplate>
                                Litri<br />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("litri") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLitriMod" runat="server" Text='<%# Bind("litri") %>' CssClass="form-control input-sm"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Euro">
                            <HeaderTemplate>
                                Euro<br />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("euro") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEuroMod" runat="server" Text='<%# Bind("euro") %>' CssClass="form-control input-sm"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Indirizzo">
                            <HeaderTemplate>
                                Indirizzo<br />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("indirizzo") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtindirizzoMod" runat="server" Text='<%# Bind("indirizzo") %>' CssClass="form-control input-sm"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:BoundField DataField="indirizzo" HeaderText="Indirizzo" />--%>

                        <asp:TemplateField HeaderText="Progressivo">
                            <HeaderTemplate>
                                Progressivo<br />
                                <asp:TextBox ID="txtFilterProgressivo" runat="server" OnTextChanged="txtFilterProgressivo_TextChanged" AutoPostBack="True" CssClass="larghezzaText" placeholder="Filtra..." />
                            </HeaderTemplate>
                            <ItemTemplate><%# Eval("progressivo") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtProgressivoMod" runat="server" Text='<%# Bind("progressivo") %>' CssClass="form-control input-sm"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Verificato" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <span>
                                    <%# Eval("verificato").ToString() == "True" ? "SÌ" : "NO" %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnSelect" runat="server" Text="OK"
                                    CommandName="Select"
                                    CommandArgument='<%# Eval("id") + "|" + Eval("targa") + "|" + Eval("data") + "|" + Eval("autista") %>'
                                    CssClass="btn btn-success btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Button ID="btnModifica" runat="server" Text="Mod." CommandName="Edit" CssClass="btn btn-warning btn-sm" CommandArgument='<%# Eval("id")%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnSalva" runat="server" Text="Salva" CommandName="Update" CssClass="btn btn-primary btn-sm" ValidationGroup="EditVG" />
                                <asp:Button ID="btnAnnulla" runat="server" Text="X" CommandName="Cancel" CssClass="btn btn-secondary btn-sm" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                </asp:GridView>
            </div>

            <div class="modal-footer bg-light">
                <asp:Button ID="btBack" runat="server" class="btn btn-secondary" Text="Azzera Filtri" OnClick="btBack_Click" ToolTip="Torna alla lista completa" Visible="false" />
            </div>
        </div>

        <asp:HiddenField ID="Hfuser" runat="server" />
        <asp:HiddenField ID="HfFiltroData" runat="server" />
        <asp:HiddenField ID="HfFiltroSigla" runat="server" />
        <asp:HiddenField ID="HfProgressivo" runat="server" />
        <asp:HiddenField ID="Hfid" runat="server" />

    </div>

    <!-- Modali -->
    <div class="modal fade" id="errorModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">ATTENZIONE</h5>
                </div>
                <div class="modal-body text-center">
                    <p id="errorMessage" style="color: red; font-size: 1.2rem; font-weight: bold;"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" onclick="HideErrorMessage()">Chiudi</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalAvvertenze" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h5 class="modal-title">AVVISO</h5>
                </div>
                <div class="modal-body text-center">
                    <p id="txtAvvertenze" style="color: #856404; font-size: 1.2rem; font-weight: bold;"></p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btChiudiAvvertenze" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiAvvertenze_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

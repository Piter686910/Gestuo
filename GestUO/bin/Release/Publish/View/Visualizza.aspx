<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Visualizza.aspx.cs" Inherits="GestUO.Visualizza" %>

<%@ Import Namespace="GestUO.Classi" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        function openPopup() {
            document.getElementById("popupModal").style.display = "block";
            document.getElementById("overlay").style.display = "block";
        }
        // Mostra il popup
        function showModalNote() {
            $('#ModalRicercaNote').modal('show');
        }

        // Nasconde il popup
        function hideModalNote() {
            $('#ModalRicercaNote').modal('hide');
        }

        // Mostra il popup ricerca
        function showModal() {
            $('#ModalRicerca').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicerca').modal('hide');
        }



    </script>


    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
            <%--<p class="text-center lead">Ricerca Atti</p>--%>
            <div class="dashboard-header">
                <h1><span class="fa-solid fa-gear fa-spin"></span><u><%= TitoloUO %> </u></h1>
            </div>
            <!-- Contenitore per centrare -->

            <asp:Panel ID="pnlButton" runat="server" CssClass="" Visible="true">
                <div class="d-flex justify-content-center align-items-center w-100 my-4">
                    <table style="margin: 0 auto; border-collapse: separate; border-spacing: 6px;">
                        <tr>
                            <td>
                                <asp:Button ID="btNCarico" runat="server" OnClick="btNCarico_Click" Text="Nr. Carico" CssClass="btn btn-primary btn-custom-size" ToolTip="Ricerca per numero carico" /></td>
                            <td>
                                <asp:Button ID="btProcPenale" runat="server" OnClick="btProcPenale_Click" Text="Proc. Penale" ToolTip="Ricerca Procedimento Penale" CssClass="btn btn-primary btn-custom-size" /></td>
                            <td>
                                <asp:Button ID="btProtGen" runat="server" OnClick="btProtGen_Click" Text="Rif. Prot. Gen." ToolTip="Ricerca Protocollo Generale" CssClass="btn btn-primary btn-custom-size" /></td>
                            <td>
                                <asp:Button ID="btEvaseAg" runat="server" OnClick="btEvaseAg_Click" Text="Evase Ag." ToolTip="Ricerca Evase AG" CssClass="btn btn-primary btn-custom-size" /></td>
                            <td>
                                <asp:Button ID="btNpratica" runat="server" OnClick="btNpratica_Click" Text="Nr. Pratica" ToolTip="Ricerca Pratica" CssClass="btn btn-primary btn-custom-size" /></td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btGiudice" runat="server" OnClick="btGiudice_Click" Text="Giudice" ToolTip="Ricerca Giudice" CssClass="btn btn-primary btn-custom-size" />

                            </td>
                            <td>
                                <asp:Button ID="btProvenienza" runat="server" OnClick="btProvenienza_Click" Text="Provenienza" ToolTip="Ricerca Per ProvenienzaG" CssClass="btn btn-primary btn-custom-size" />

                            </td>
                            <td>
                                <asp:Button ID="btNominativo" runat="server" OnClick="btNominativo_Click" Text="Nominativo" ToolTip="Ricerca Nominativo" CssClass="btn btn-primary btn-custom-size" />

                            </td>

                            <td>
                                <asp:Button ID="btBuCodEd" runat="server" OnClick="btBuCodEd_Click" Text="Cod. BU/Edificio" ToolTip="Ricerca Cod. BU/Edificio" CssClass="btn btn-primary btn-custom-size" />

                            </td>
                            <td>
                                <asp:Button ID="btNote" runat="server" OnClick="btNote_Click" Text="Note" ToolTip="Ricerca Note" CssClass="btn btn-primary btn-custom-size" />

                            </td>
                        </tr>
                        <tr>

                            <td>
                                <asp:Button ID="btDataCarico" runat="server" OnClick="btDataCarico_Click" Text="Data Inserimento" ToolTip="Ricerca Data Inserimento" CssClass="btn btn-primary btn-custom-size" />

                            </td>
                            <td>
                                <asp:Button ID="btOperatori" runat="server" OnClick="btOperatori_Click" Text="Operatori" ToolTip="Ricerca Operatori" CssClass="btn btn-primary btn-custom-size" />

                            </td>
                            <td>
                                <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Per Indirizzo" CssClass="btn btn-primary btn-custom-size" />

                            </td>

                            <td>
                                <asp:Button ID="btcarico" runat="server" OnClick="btcarico_Click" Text="Stampa Carico" ToolTip="Stampa Carico" CssClass="btn btn-primary btn-custom-size" />
                            </td>
                            <td>
                                <asp:Button ID="btValidaPratica" runat="server" OnClick="btValidaPratica_Click" Text="Valida pratica" ToolTip="Ricerca Pratica" CssClass="btn btn-primary btn-custom-size" />

                            </td>
                        </tr>
                    </table>

                    <asp:CheckBox ID="ckRicercaGeneralizzata" runat="server" Text="Effettua Ricerca Generalizzata" Visible="false" />
                </div>

            </asp:Panel>

        </div>

        <div id="DivRicerca" runat="server" visible="false" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 ">

                <%-- DIV RICERCA Carico --%>
                <div id="DivCaricoRicerca" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="lblm" runat="server" Text="Nr Carico" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNCarico" ErrorMessage="Inserire numero pratica" ForeColor="Red" ValidationGroup="bt">

                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtNCarico" runat="server" CssClass="form-control" placeholder="Nr Carico" autofocus="" />


                    <asp:Label ID="Label1" runat="server" Text="Anno" CssClass="form-label d-block mb-2"></asp:Label>
                    <%-- <asp:RequiredFieldValidator ID="rqanno" runat="server" ControlToValidate="txtAnnoRicerca" ErrorMessage="Inserire l'anno per la ricerca" ForeColor="Red" ValidationGroup="bt">

                    </asp:RequiredFieldValidator>--%>
                    <asp:TextBox ID="txtAnnoRicerca" runat="server" CssClass="form-control" placeholder="Anno" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA PROocedimento penale --%>
                <div id="DivProcPenale" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label2" runat="server" Text="Nr Procediemnto Penale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProcPenale" runat="server" CssClass="form-control" placeholder="Nr Procedimento Penale" autofocus="" />



                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA evasa ag --%>
                <div id="DivEvasaAg" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label3" runat="server" Text="Data Inizio Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataDa" runat="server" CssClass="form-control data-auto" placeholder="Data Inizio" autofocus="" />

                    <asp:Label ID="Label4" runat="server" Text="Data Fine Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataA" runat="server" CssClass="form-control data-auto" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA rif protocollo generale --%>
                <div id="DivProtGen" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="Rif. Protocollo Generale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProtGen" runat="server" CssClass="form-control" placeholder="Rif. Prot. Gen." autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label6" runat="server" Text="Nr. Pratica Edilizia" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" placeholder="Nr. Pratica Edilizia" autofocus="" />
                    <%-- I mod 26/08/2026 ricerca per numero cartellina patrimonio --%>
                    <asp:Label ID="Label18" runat="server" Text="Nr. Cartellina Patrimonio" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtCartellinaRic" runat="server" CssClass="form-control" placeholder="Nr. Cartellina Patrimonio" />
                    <asp:Label ID="Label20" runat="server" Text="Quartiere" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtQuart" runat="server" CssClass="form-control" placeholder="Quartiere" />

                    <%-- F mod 26/08/2026 ricerca per numero cartellina patrimonio --%>
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV Validazione --%>
                <div id="DivValidazione" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label14" runat="server" Text="Pratica" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicPraticaVal" runat="server" CssClass="form-control" placeholder="Pratica da validare" autofocus="" />
                    <asp:Label ID="Label15" runat="server" Text="Anno" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicAnnoVal" runat="server" CssClass="form-control" placeholder="Anno" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA giudice --%>
                <div id="DivGiudice" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label7" runat="server" Text="Giudice" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicGiudice" runat="server" CssClass="form-control" placeholder="Giudice" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA provenienza --%>
                <div id="DivProvenienza" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label8" runat="server" Text="Provenienza" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicProvenienza" runat="server" CssClass="form-control" placeholder="Provenienza" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA nominativo --%>
                <div id="DivNominativo" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label9" runat="server" Text="Nominativo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicNominativo" runat="server" CssClass="form-control" placeholder="Nominativo" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA operatori --%>
                <div id="DivOperatori" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label10" runat="server" Text="Operatori" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicOperatori" runat="server" CssClass="form-control" placeholder="Operatori" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Indirizzo --%>
                <div id="DivIndirizzo" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Indirizzo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicIndirizzo" runat="server" CssClass="form-control" placeholder="Indirizzo" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA data carico --%>
                <div id="DivDataArrivo" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label12" runat="server" Text="Data Inserimento Da" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoDa" runat="server" CssClass="form-control data-auto" placeholder="Data Inizio" />

                    <asp:Label ID="Label13" runat="server" Text="Data Inserimento A" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoA" runat="server" CssClass="form-control data-auto" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA PER STAMPARE IL CARICO --%>
                <div id="DivCarico" runat="server" class="form-group text-center" style="text-align: left !important">

                    <label cssclass="form-label d-block mb-2" for="DdlMacroArea">Area Competenza</label>
                    <asp:DropDownList ID="DdlMacroArea" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="DdlMacroArea_SelectedIndexChanged">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>MA1</asp:ListItem>
                        <asp:ListItem>MA2</asp:ListItem>
                        <asp:ListItem>MA3</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label19" runat="server" Text="Responsabile Area" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:TextBox ID="txtCapoArea" runat="server" CssClass="form-control" placeholder="Responsabile" autofocus="" />

                    <label cssclass="form-label d-block mb-2" for="DdlSigla">Sigla</label>
                    <asp:DropDownList ID="DdlSigla" runat="server" AutoPostBack="true" CssClass="form-control">
                        <asp:ListItem>AG</asp:ListItem>
                        <asp:ListItem>ED</asp:ListItem>
                        <asp:ListItem>TP</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label17" runat="server" Text="Data Carico" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:TextBox ID="txtStampaDataCarico" runat="server" CssClass="form-control data-auto" placeholder="Data Carico" autofocus="" />


                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Note --%>
                <div id="DivNote" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label16" runat="server" Text="Note" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtNote" runat="server" CssClass="form-control" placeholder="Note" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA BU / CODICE EDIFICIO --%>
                <div id="DivBuCodEd" runat="server" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label21" runat="server" Text="Cod. BU" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtBuRic" runat="server" CssClass="form-control" placeholder="Codice BU" autofocus="" />

                    <asp:Label ID="Label22" runat="server" Text="Cod. Edificio" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtCodEdRic" runat="server" CssClass="form-control" placeholder="Codice Edificio" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>
        </div>
        <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
        <div class="container" id="DivDettagli" runat="server">

            <div class="tab-content">
                <p style="font-weight: bold; font-size: medium">Dati Generali</p>
                <div class="row custom-border">
                    <div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">

                            <label for="txtProt">Nr Carico</label>
                            <asp:TextBox ID="txtProt" runat="server" CssClass="form-control mb-3" ForeColor="Red" Enabled="false" Font-Bold="true" />
                            <%--<asp:TextBox ID="txtSigla" runat="server" CssClass="col-md-2" Enabled="false" />--%>
                        </div>
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtTipoAtto">Tipologia Atto</label>
                            <asp:TextBox ID="txtTipoAtto" runat="server" CssClass="form-control mb-3" Enabled="false" />
                            <label for="txtUltTipoAtto">Ulteriore Atto</label>
                            <asp:TextBox ID="txtUltTipoAtto" runat="server" CssClass="form-control mb-3" Enabled="false" />
                        </div>

                    </div>


                    <%-- seconda colonna --%>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtDataInsCarico">Data Inserimento</label>
                            <asp:TextBox ID="txtDataInsCarico" runat="server" CssClass="form-control mb-3 data-auto" Enabled="false" />

                        </div>
                        <div class="form-group mb-3">
                            <label for="txtProvenienza">Provenienza</label>
                            <asp:TextBox ID="txtProvenienza" runat="server" CssClass="form-control mb-3" Enabled="false" />
                        </div>
                    </div>
                    <%-- terza colonna --%>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtRifProtGen">Protocollo Generale</label>
                            <asp:TextBox ID="txtRifProtGen" runat="server" CssClass="form-control mb-3" Enabled="false" />

                        </div>
                        <div class="form-group mb-3">
                            <label for="txtNumProtRicStessoCarico">Numeri protocollo</label>
                            <asp:TextBox ID="txtNumProtRicStessoCarico" runat="server" CssClass="form-control larghezzaText70" Enabled="false" />

                        </div>
                    </div>

                </div>
                <p style="font-weight: bold; font-size: medium">Dati Relativi Alla Pratica</p>
                <div class="row custom-border">
                    <div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtIndirizzo">Indirizzo</label>
                            <asp:TextBox ID="txtIndirizzo" runat="server" CssClass="form-control mb-3" Enabled="false" />

                        </div>
                        <%--                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtPraticaOut">Pratica</label>
                            <asp:TextBox ID="txtPraticaOut" runat="server" CssClass="form-control mb-3" Enabled="false" />
                        </div>
                        --%>
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtPraticaOut">Pratica Edilizia</label>
                            <asp:TextBox ID="txtPraticaOut" runat="server" CssClass="form-control" Enabled="false" />
                        </div>

                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtCartellina">Cartellina Patrimonio</label>
                            <asp:TextBox ID="txtCartellina" runat="server" CssClass="form-control" Enabled="false" />
                        </div>




                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="TxtQuartiere">Quartiere</label>
                            <asp:TextBox ID="TxtQuartiere" runat="server" CssClass="form-control mb-3" Enabled="false" />
                        </div>
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtAreaCompetenza">Area Competenza</label>
                            <asp:TextBox ID="txtAreaCompetenza" runat="server" CssClass="form-control mb-3" Enabled="false" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtNominativo">Nominativo</label>
                            <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control mb-3" Enabled="false" />
                        </div>
                        <div class="form-group mb-3">
                            <label for="txtDataCarico">Data Carico</label>
                            <asp:TextBox ID="txtDataCarico" runat="server" CssClass="form-control mb-3 data-auto" Enabled="false" />
                        </div>
                    </div>
                    <div class="col-md-4" id="divbu" runat="server" style="display: none;">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtBU">BU</label>
                            <asp:TextBox ID="txtBU" runat="server" CssClass="form-control" Enabled="false" />
                        </div>

                    </div>

                    <div class="col-md-4" id="divcd" runat="server" style="display: none;">
                        <div class="form-group mb-3">
                            <label for="txtCodEdificio">Codice Edificio</label>
                            <asp:TextBox ID="txtCodEdificio" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
                    </div>
                </div>

                <p style="font-weight: bold; font-size: medium">Esito Accertamento</p>
                <div class="row custom-border">
                    <div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtEsito">Esito</label>
                            <asp:TextBox ID="txtEsito" runat="server" CssClass="form-control" Enabled="false" />
                        </div>

                        <div class="form-group mb-3 d-flex align-items-center" style="margin-top: 25px;">
                            <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" Enabled="false" />
                            <label class="form-check-label ms-2 mb-0" for="CkEvasa">Trasmesso</label>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txtDataDataEvasa">Data Esito</label>
                            <asp:TextBox ID="txtDataDataEvasa" runat="server" CssClass="form-control data-auto" Enabled="false" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-4">
                            <label for="TxtDataEsito" class="fw-bold">Protocollo Uscita</label>
                            <asp:TextBox ID="txtProtUscita" runat="server" CssClass="form-control" Enabled="false" />
                        </div>

                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="ListOperatori">Operatori</label>
                            <asp:ListBox ID="ListOperatori" runat="server" CssClass="form-control" Enabled="false" BackColor="#e9ecef" Rows="3"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div id="divAg" runat="server" visible="false">

                    <p style="font-weight: bold; font-size: medium">Dati AG</p>
                    <div class="row custom-border">
                        <div class="col-md-4">
                            <div class="form-group mb-3" style="margin-left: -25px">
                                <label for="txtGiudice">Giudice</label>
                                <asp:TextBox ID="txtGiudice" runat="server" CssClass="form-control mb-3" Enabled="false" />
                            </div>
                            <div class="form-group mb-3" style="margin-left: -25px">
                                <label for="txtDataDelega">Data Delega</label>
                                <asp:TextBox ID="txtDataDelega" runat="server" CssClass="form-control data-auto" Enabled="false"></asp:TextBox>

                            </div>

                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-3">
                                <label for="TxtTipoProvvAg">Tipo Provvedimento AG</label>
                                <asp:TextBox ID="TxtTipoProvvAg" runat="server" CssClass="form-control mb-3" Enabled="false" />

                            </div>
                            <div class="form-group mb-3">
                                <label for="txtGgDelega">Termine gg. delega</label>
                                <asp:TextBox ID="txtGgDelega" runat="server" AutoPostBack="false" Enabled="false" CssClass="form-control"></asp:TextBox>
                            </div>

                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-3" style="margin-left: -25px">
                                <label for="txtProdPenNr">Procedimento Penale nr</label>
                                <asp:TextBox ID="txtProdPenNr" runat="server" CssClass="form-control mb-3" Enabled="false" />

                            </div>

                        </div>

                    </div>
                </div>
                <div id="divDecretazione" runat="server">
                    <p style="font-weight: bold; font-size: medium">Decretazione</p>
                    <div class="row custom-border">
                        <div class="col-md-12">
                            <div class="form-group mb-3" style="margin-left: -25px">
                            </div>
                            <div class="form-group">
                                <!-- GridView nel popup -->
                                <asp:GridView ID="GVDecretazione" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered gridview-autofit"
                                    OnRowDataBound="GVDecretazione_RowDataBound" OnRowCommand="GVDecretazione_RowCommand" AllowPaging="true" PageSize="15" OnPageIndexChanging="GVDecretazione_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="decr_id" HeaderText="ID" Visible="false" />
                                        <asp:BoundField DataField="decr_idPratica" HeaderText="ID" Visible="false" />
                                        <asp:BoundField DataField="decr_pratica" HeaderText="Pratica" Visible="false" />
                                        <asp:BoundField DataField="decr_decretante" HeaderText="Decretante">
                                            <HeaderStyle CssClass="colonna-descrizione" />
                                            <ItemStyle CssClass="uppercase-text" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="decr_data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />

                                        <asp:BoundField DataField="decr_decretato" HeaderText="Decretato">
                                            <HeaderStyle CssClass="colonna-descrizione" />
                                            <ItemStyle CssClass="uppercase-text" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="decr_nota" HeaderText="Nota">
                                            <HeaderStyle CssClass="colonna-descrizione" />
                                            <ItemStyle CssClass="uppercase-text colonna-descrizione" />
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Data Chiusura">
                                            <ItemTemplate>
                                                <%# Routine.FormatMyDate(Eval("decr_dataChiusura")) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                    <PagerStyle HorizontalAlign="Center" />
                                    <PagerTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 50%; text-align: left;">
                                                    <asp:Label ID="lblPageInfo" runat="server" />
                                                </td>

                                            </tr>
                                        </table>
                                        <div style="padding: 5px;">
                                            <asp:Button ID="btnFirst" runat="server" CommandName="Page" CommandArgument="First" Text="<< Prima" CssClass="pager-button" />
                                            <asp:Button ID="btnPrev" runat="server" CommandName="Page" CommandArgument="Prev" Text="< Precedente" CssClass="pager-button" />

                                            <span style="margin: 0 10px;">Pagina:
               
                                            </span>

                                            <%-- Contenitore per i link numerici delle pagine --%>
                                            <asp:PlaceHolder ID="phPagerNumbers" runat="server" />

                                            <asp:Button ID="btnNext" runat="server" CommandName="Page" CommandArgument="Next" Text="Successiva >" CssClass="pager-button" />
                                            <asp:Button ID="btnLast" runat="server" CommandName="Page" CommandArgument="Last" Text="Ultima >>" CssClass="pager-button" />
                                        </div>
                                    </PagerTemplate>

                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="d-flex justify-content-center align-items-center w-100 my-4">
                    <table style="margin: 0 auto; border-collapse: separate; border-spacing: 6px;">
                        <tr>
                            <td>
                                <asp:Button Text="Nuova Ricerca" runat="server" OnClick="NuovaRicerca_Click" ToolTip="Nuova Ricerca" CssClass="btn btn-primary btn-custom-size" /></td>
                            <td>
                                <asp:Button ID="btModifica" Text="Modifica" runat="server" OnClick="btModifica_Click" ToolTip="Modifica" CssClass="btn btn-primary btn-custom-size" /></td>
                            <td>
                                <asp:Button ID="btDecreta" Text="Decreta" runat="server" OnClick="btDecreta_Click" ToolTip="Decreta" CssClass="btn btn-primary btn-custom-size" /></td>

                            <td>
                                <asp:Button ID="BtDuplica" Text="Duplica" runat="server" OnClick="BtDuplica_Click1" ToolTip="Copia il contenuto in un nuovo carico" CssClass="btn btn-primary btn-custom-size" /></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%-- GRIGLIA VALIDAZIONE--%>
    <div id="DivGridVal" runat="server" visible="false" class="row " style="margin-left: 140px">
        <%--<div class="form-group">--%>
        <!-- GridView nel popup -->
        <asp:GridView ID="GVPratica" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered  grid-fissa"
            OnRowDataBound="GVPratica_RowDataBound" OnRowCommand="GVPratica_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVPratica_PageIndexChanging" RowStyle-CssClass="GridViewRow"
            AlternatingRowStyle-CssClass="GridViewAlternatingRow">

            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                <asp:BoundField DataField="nr_Pratica" HeaderText="N. Pratica" ItemStyle-Width="190px" HeaderStyle-Width="190px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center" />
                <asp:BoundField DataField="Anno" HeaderText="Anno" ItemStyle-Width="190px" HeaderStyle-Width="190px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center" />
                <asp:BoundField DataField="Nr_Carico" HeaderText="Nr. Carico" ItemStyle-Width="190px" HeaderStyle-Width="190px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center" />

                <asp:TemplateField HeaderText="Evasa" ItemStyle-Width="190px" HeaderStyle-Width="190px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                    <ItemTemplate>
                        <%# Eval("evasa").ToString() == "True" ? "Si" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>

                <%--                   <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                           <ItemTemplate>
                               <asp:Button ID="btnSelect" runat="server" Text="Seleziona"
                                   CommandName="Select"
                                   CommandArgument='<%# Eval("Nr_Protocollo") + "|" + Eval("ID")  %>'
                                   CssClass="btn btn-success btn-sm" />
                           </ItemTemplate>
                       </asp:TemplateField>--%>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" Position="Top" />
            <PagerStyle HorizontalAlign="Center" />
            <PagerTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 50%; text-align: left;">
                            <asp:Label ID="lblPageInfo" runat="server" />
                        </td>

                    </tr>
                </table>

                <div style="padding: 5px;">
                    <asp:Button ID="btnFirst" runat="server" CommandName="Page" CommandArgument="First" Text="<< Prima" CssClass="pager-button" />
                    <asp:Button ID="btnPrev" runat="server" CommandName="Page" CommandArgument="Prev" Text="< Precedente" CssClass="pager-button" />

                    <span style="margin: 0 10px;">Pagina:
      
                    </span>

                    <%-- Contenitore per i link numerici delle pagine --%>
                    <asp:PlaceHolder ID="phPagerNumbers" runat="server" />

                    <asp:Button ID="btnNext" runat="server" CommandName="Page" CommandArgument="Next" Text="Successiva >" CssClass="pager-button" />
                    <asp:Button ID="btnLast" runat="server" CommandName="Page" CommandArgument="Last" Text="Ultima >>" CssClass="pager-button" />
                </div>
            </PagerTemplate>

        </asp:GridView>

        <div style="margin-left: 300px">
            <!-- Bottone per avviare la ricerca -->
            <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
            <%--<asp:Button ID="btchiudiGriglia" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />--%>
            <asp:Button ID="btValidazione" runat="server" class="btn btn-primary mt-3" Text="Convalida" OnClick="btValidazione_Click" ToolTip="Esegue la validazione della pratica" />
        </div>
        <%--</div>--%>
    </div>
    <%-- Modale ricerca fascicolo --%>
    <div class="modal fade" id="ModalRicerca" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Fascicolo</h5>

                </div>
                <div class="modal-body">
                    <div id="DivGrid" runat="server" visible="false" class="section-box">
                        <div class="d-flex justify-content-between align-items-center mb-2 px-1">
                            <div class="small text-muted">
                                <asp:Label ID="lblInfoPagine" runat="server" Text="Pagina"></asp:Label>
                                <strong>
                                    <asp:Label ID="lblNumRighe" runat="server" Text=""></asp:Label></strong>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <!-- GridView nel popup -->
                            <asp:GridView ID="gvPopup" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-bordered table-hover fs-1"
                                OnRowDataBound="gvPopup_RowDataBound"
                                OnRowCommand="gvPopup_RowCommand"
                                OnDataBound="gvPopup_DataBound"
                                AllowPaging="true" PageSize="10"
                                OnPageIndexChanging="gvPopup_PageIndexChanging"
                                RowStyle-CssClass="GridViewRow"
                                AlternatingRowStyle-CssClass="GridViewAlternatingRow"
                                PagerSettings-Position="Top"
                                PagerSettings-Mode="NextPreviousFirstLast"
                                PagerSettings-FirstPageText="&laquo; Prima"
                                PagerSettings-LastPageText="Ultima &raquo;"
                                PagerSettings-NextPageText="Succ. &rsaquo;"
                                PagerSettings-PreviousPageText="&lsaquo; Prec.">

                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                    <asp:BoundField DataField="Nr_Carico" HeaderText="Nr. Carico" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="Anno" HeaderText="Anno" ItemStyle-Width="10px" />


<%--                                    <asp:TemplateField HeaderText="Sigla" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="10px">
                                        <HeaderTemplate>
                                            Sigla
                                          <br />
                                            <asp:TextBox ID="txtFilterSigla" runat="server" OnTextChanged="txtFilterSigla_TextChanged" AutoPostBack="True" ToolTip="Filtra..." Width="50px" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("Sigla") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <%--<asp:BoundField DataField="nr_Pratica" HeaderText="N. Pratica" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />--%>
                                    <asp:TemplateField HeaderText="Nominativo" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                        <HeaderTemplate>
                                            N. Pratica
                                               <br />
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <asp:HyperLink ID="lnkScheda" runat="server"
                                                NavigateUrl='<%# String.Format("~/View/GestionePratica.aspx?idscheda={0}&nrPratica={1}", Eval("ID"), Eval("nr_Pratica")) %>'
                                                Target="_blank"
                                                Text='<%# Eval("nr_Pratica") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Nominativo" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                        <HeaderTemplate>
                                            Nominativo
                                          <br />
                                            <asp:TextBox ID="txtFilterNominativo" runat="server" OnTextChanged="txtFilterNominativo_TextChanged" AutoPostBack="True" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("Nominativo") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField HeaderText="Indirizzo" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                        <HeaderTemplate>
                                            Indirizzo
                                          <br />
                                            <asp:TextBox ID="txtFilterIndirizzo" runat="server" OnTextChanged="txtFilterIndirizzo_TextChanged" AutoPostBack="True" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("indirizzo") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--                                <asp:BoundField DataField="Indirizzo" HeaderText="Indirizzo" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>--%>
                                    <asp:BoundField DataField="ProcedimentoPen" HeaderText="Proc. Penale" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TipoProvvedimentoAG" HeaderText="Tipo Prov. AG" ItemStyle-Width="30px" />

                                    <asp:BoundField DataField="Tipologia_atto" HeaderText="Tipologia Atto" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                        <ItemStyle CssClass="uppercase-text" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UlterioreTipoAtto" HeaderText="Ulteriore Tipo Atto" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                        <ItemStyle CssClass="uppercase-text" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Operatori" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="70px">
                                        <HeaderTemplate>
                                            Operatori
                                          <br />
                                            <asp:TextBox ID="txtFilterOperatori" runat="server" OnTextChanged="txtFilterOperatori_TextChanged" AutoPostBack="True" CssClass="form-control form-control-sm" placeholder="Filtra..."></asp:TextBox>

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("Operatori")   %>
                                            <%# Eval("Operatori2")   %>
                                            <%# Eval("Operatori3")   %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Prot. Generale" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div style="word-break: break-all; width: 100px;">
                                                <%# Eval("Rif_Prot_Gen") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Evasa" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Eval("evasa").ToString() == "True" ? "Si" : "No" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NomeOperatore" HeaderText="Operatore" Visible="true" ItemStyle-Width="20px" />
                                    <asp:TemplateField HeaderText="Provenienza" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%# EvalSafe(Container.DataItem, "uo") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DataInserimento" HeaderText="Data Inserimento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="20px" Visible="false" />
                                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Seleziona"
                                                CommandName="Select"
                                                CommandArgument='<%# Eval("Nr_Carico") + "|" + Eval("Matricola") + "|" + Eval("DataInserimento") + "|" + Eval("ID")+ "|" + EvalSafe(Container.DataItem, "DbOrigine")  %>'
                                                CssClass="btn btn-success btn-sm" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                            </asp:GridView>
                            <div class="modal-footer">
                                <!-- Bottone per avviare la ricerca -->
                                <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                                <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                                <asp:Button ID="btBack" runat="server" class="btn btn-secondary" Text="Azzera Filtri" OnClick="btBack_Click" ToolTip="Torna alla lista completa" />
                            </div>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="HidPratica" runat="server" />
                <asp:HiddenField ID="HfIdScheda" runat="server" />
                <asp:HiddenField ID="HfFiltroIndirizzo" runat="server" />
                <asp:HiddenField ID="HfFiltroNominativo" runat="server" />
                <asp:HiddenField ID="HfFiltroOperatori" runat="server" />
                <asp:HiddenField ID="HfFiltroSigla" runat="server" />
                <asp:HiddenField ID="HfFiltroNota" runat="server" />

            </div>
        </div>
    </div>
    <div class="modal fade" id="ModalRicercaNote" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" style="width: 100%">
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="modalLabel22">Ricerca Note</h5>

                </div>
                <div class="modal-body">
                    <div id="DivRicNote" runat="server" visible="false" class="section-box">
                        <div class="d-flex justify-content-between align-items-center mb-2 px-1">
                            <div class="small text-muted">
                                <asp:Label ID="lblInfoPagineNote" runat="server" Text="Pagina 1 di 10 "></asp:Label>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <!-- GridView nel popup -->
                            <asp:GridView ID="gvNote" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-bordered table-hover fs-1"
                                OnRowDataBound="gvNote_RowDataBound"
                                OnDataBound="gvNote_DataBound"
                                AllowPaging="true" PageSize="10"
                                OnPageIndexChanging="gvNote_PageIndexChanging"
                                RowStyle-CssClass="GridViewRow"
                                AlternatingRowStyle-CssClass="GridViewAlternatingRow"
                                PagerSettings-Position="Top"
                                PagerSettings-Mode="NextPreviousFirstLast"
                                PagerSettings-FirstPageText="&laquo; Prima"
                                PagerSettings-LastPageText="Ultima &raquo;"
                                PagerSettings-NextPageText="Succ. &rsaquo;"
                                PagerSettings-PreviousPageText="&lsaquo; Prec.">

                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                    <%--<asp:BoundField DataField="Nr_Protocollo" HeaderText="Nr. Carico" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />--%>
                                    <asp:TemplateField HeaderText="Carico" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true">
                                        <HeaderTemplate>
                                            Carico
                                             <br />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkScheda" runat="server"
                                                NavigateUrl='<%# String.Format("~/View/Visualizza.aspx?idscheda={0}&Nr_Carico={1}&Anno={2}", Eval("ID"), Eval("Nr_Carico"), Eval("Anno")) %>'
                                                Target="_blank"
                                                Text='<%# Eval("Nr_Carico") %>'>
                                            </asp:HyperLink>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Anno" HeaderText="Anno" ItemStyle-Width="10px" />

                                    <asp:BoundField DataField="Sigla" HeaderText="Sigla" ItemStyle-Width="10px" />

                                    <asp:BoundField DataField="Nominativo" HeaderText="Nominativo" ItemStyle-Width="10px" />

                                    <asp:BoundField DataField="ProcedimentoPen" HeaderText="Proc. Penale" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="TipoProvvedimentoAG" HeaderText="Tipo Prov. AG" ItemStyle-Width="30px" />

                                    <asp:BoundField DataField="Tipologia_atto" HeaderText="Tipologia Atto" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                        <ItemStyle CssClass="uppercase-text" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rif_Prot_Gen" HeaderText="Prot. Generale" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                        <ItemStyle CssClass="uppercase-text" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Nota" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true">
                                        <HeaderTemplate>
                                            Nota
                                             <br />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("decr_nota") %> <%# Eval("note") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>

                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                            </asp:GridView>
                            <div class="modal-footer">
                                <!-- Bottone per avviare la ricerca -->
                                <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                                <asp:Button ID="btChiudiPop" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiPop_Click" />
                                <%--<asp:Button ID="Button2" runat="server" class="btn btn-secondary" Text="Back" OnClick="btBack_Click" ToolTip="Torna alla lista completa" />--%>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </div>


</asp:Content>

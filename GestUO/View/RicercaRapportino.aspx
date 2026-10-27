<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RicercaRapportino.aspx.cs" Inherits="GestUO.RicercaRapportino" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        function CloseErrorMessage(message) {
            $('#errorModal').modal('hide');
        }
        // Mostra il popup ricerca
        function showModal() {
            $('#ModalRicerca').modal('show');
        }

        // Nasconde il popup
        function hideModal1() {
            $('#ModalRicerca').modal('hide');
        }
        // Mostra il popup
        function showModal() {
            $('#PopStampa').modal('show');
        }

        // Nasconde il popup
        function hideModalS() {
            $('#PopStampa').modal('hide');
        }

        // Funzione per aggiungere testo a un TextBox
        function appendToTextBox(TextPattugliaCompleta, DdlPattuglia) {
            // Ottieni il TextBox tramite il suo ID
            const textBox = document.getElementById(TextPattugliaCompleta);
            const dropDown = document.getElementById(DdlPattuglia);
            // Aggiungi il valore al contenuto corrente
            if (textBox && dropDown) {
                // Ottieni il valore selezionato nella DropDownList
                const selectedValue = dropDown.value;

                // Aggiungi il valore selezionato al contenuto del TextBox
                textBox.value += selectedValue;
            }
        }


    </script>

    <div class="panel panel-default">
        <div class="dashboard-header">
            <h1><span class="fa-solid fa-gear fa-spin"></span><u><%= TitoloUo %> </u></h1>
        </div>


        <div class="row">
            <!-- Colonna sinistra -->
            <div class="col-md-3 form-check">
                <asp:CheckBox ID="CkAttivita" runat="server" AutoPostBack="true" OnCheckedChanged="CkAttivita_CheckedChanged" />
                <label class="form-check-label ms-3" for="CkAttivita">Attività Interna</label>

                <asp:CheckBox ID="CkAppiedata" runat="server" AutoPostBack="true" OnCheckedChanged="CkAppiedata_CheckedChanged" />
                <label class="form-check-label ms-3" for="CkAppiedata">Appiedata</label>
            </div>
        </div>

        <div class="panel-body" id="divTesta" runat="server" style="margin-top: -40px">
            <div class="jumbotron">

                <div class="container-fluid p-3">

                    <div class="row align-items-end mb-4">

                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="TxtDataIntervento" class="fw-bold">Data Intervento</label>
                                <asp:TextBox ID="TxtDataIntervento" runat="server" CssClass="form-control data-auto" ClientIDMode="Static" placeholder="gg/mm/yyyy" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtDataIntervento" ValidationGroup="bt" ErrorMessage="Inserire data intervento" ForeColor="Red" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtDataIntervento" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$" ErrorMessage="Formato dd/mm/aaaa" ForeColor="Red" ValidationGroup="bt" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtDataConsegna" class="fw-bold">Data Consegna</label>
                                <asp:TextBox ID="txtDataConsegna" runat="server" CssClass="form-control data-auto" ClientIDMode="Static" Enabled="false" Font-Bold="true" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDataConsegna" ValidationGroup="bt" ErrorMessage="Inserire data consegna" ForeColor="Red" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtDataConsegna" ValidationExpression="^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/(19|20)\d{2}$" ErrorMessage="Formato dd/mm/aaaa" ForeColor="Red" ValidationGroup="bt" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtOrarioServizio" class="fw-bold">Orario</label>
                                <asp:TextBox ID="txtOrarioServizio" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="row align-items-end mb-4">
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="DdlSiglaVettura" class="fw-bold">Vettura</label>
                                <asp:DropDownList ID="DdlSiglaVettura" runat="server" CssClass="form-control" />
                            </div>
                        </div>





                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtItinerario" class="fw-bold">Itinerario</label>
                                <asp:TextBox ID="txtItinerario" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="txtNote" class="fw-bold">Note</label>
                                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control "  TextMode="MultiLine" MaxLength="255" Rows="5" Style="width: 100%; min-width: 500px;"/>
                            </div>
                        </div>
                    </div>

                    <div class="row align-items-end">
                        <div class="col-md-3">

                            <div class="form-group">
                                <label for="DdlPattuglia" class="fw-bold">Pattuglia</label>
                                <asp:DropDownList ID="DdlPattuglia" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="col-md-1 text-center" style="margin-top: 30px">
                            <div class="form-group">
                                <asp:Button ID="btAggiungi" runat="server" Text="➕" CssClass="btn btn-success w-100 mb-2" OnClick="Aggiungi_Click" ToolTip="Aggiungi" />
                                <%--<asp:Button ID="btElimina" runat="server" Text="➖" CssClass="btn btn-danger w-100" OnClick="btElimina_Click" ToolTip="Elimina" />--%>
                            </div>
                        </div>

                        <div class="col-md-8" style="margin-top: 30px">
                            <%--<div class="form-group">
                          <label class="fw-bold text-muted small">Lista Pattuglia Completa</label>
                          <asp:ListBox ID="LPattugliaCompleta" runat="server" CssClass="form-control" Rows="4"></asp:ListBox>
                      </div>--%>



                            <%-- Repeater operatori --%>
                            <div id="divRepeater" runat="server" class="table-responsive shadow-sm rounded">
                                <asp:Repeater ID="rptOperatori" runat="server" OnItemDataBound="rptOperatori_ItemDataBound"
                                    OnItemCommand="rptOperatori_ItemCommand">

                                    <HeaderTemplate>
                                        <table class="table table-hover table-striped align-middle mb-0 bg-white">
                                            <thead class="table-dark">
                                                <tr>

                                                    <th scope="col">Grado</th>
                                                    <th scope="col">Nominativo</th>
                                                    <th scope="col">Matricola</th>
                                                    <th scope="col" class="text-center" style="width: 80px;">Elimina</th>

                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <tr>

                                            <td>
                                                <span id="lblGrado" runat="server"><%# Eval("Grado") %></span>
                                            </td>
                                            <td>
                                                <span id="lblNominativo" runat="server"><%# Eval("Nominativo") %></span>
                                            </td>

                                            <td>
                                                <span id="lblMatricola" runat="server"><%# Eval("Matricola") %></span>
                                            </td>

                                            <td class="text-center">
                                                <asp:LinkButton ID="btnDelete" runat="server"
                                                    CommandName="Delete"
                                                    CommandArgument='<%# Eval("Matricola") %>'
                                                    CssClass="btn btn-sm btn-outline-danger"
                                                    ToolTip="Elimina operatore">
                                                  <i class="bi bi-trash"></i>
                                                </asp:LinkButton>
                                            </td>

                                        </tr>
                                    </ItemTemplate>

                                    <FooterTemplate>
                                        </tbody>
                          </table>
                                 
                                    </FooterTemplate>
                                </asp:Repeater>
                                <asp:PlaceHolder ID="phEmpty" runat="server" Visible="false">
                                    <div class="p-4 text-center border-top bg-white">
                                        <i class="bi bi-info-circle text-muted fs-2"></i>
                                        <p class="text-muted mt-2 mb-0">Nessun dato presente.</p>
                                    </div>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Bottone Salva -->
                <div class="row">
                    <div class="col-6">
                        <asp:Button ID="btSalva" runat="server" ValidationGroup="bt" Text="💾 Salva Scheda" CssClass="btn btn-primary me-3" OnClick="btSalva_Click" />
                        <asp:Button ID="btStampa" runat="server" Text="🖨️  Stampa" CssClass="btn btn-primary me-3" OnClick="btStampa_Click" ValidationGroup="bottoni" />
                        <asp:Button ID="btModificaScheda" runat="server"  Text="📄 Modifica Scheda" CssClass="btn btn-primary me-3" OnClick="btModificaScheda_Click" ValidationGroup="bottoni"/>
                    </div>

                </div>
            </div>
        </div>


    </div>


    <!-- Bottone Salva -->
    <div cssclass="text-center" style="margin-bottom: 15px!important">
        <div class="col-12">
            <%--            <asp:Button ID="btModificaScheda" runat="server" ValidationGroup="bottoni" Text="Modifica Scheda" CssClass="btn btn-primary me-3" OnClick="btModificaScheda_Click" />--%>
        </div>
    </div>


    <%-- panel dei dettagli --%>
    <div class="container" id="divDettagli" runat="server">

        <div class="container-fluid py-3" style="margin-top: -40px">

            <!-- NAVIGAZIONE A SCHEDE (TAB STRIP) -->
            <ul class="nav nav-tabs nav-fill mb-3" id="interventiTab" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active fw-bold" id="stradale-tab" data-bs-toggle="tab" data-bs-target="#tab-stradale" type="button" role="tab" aria-controls="tab-stradale" aria-selected="true">
                        <i class="bi bi-shield-lock me-1"></i>Polizia Stradale
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link fw-bold" id="ambientale-tab" data-bs-toggle="tab" data-bs-target="#tab-ambientale" type="button" role="tab" aria-controls="tab-ambientale" aria-selected="false">
                        <i class="bi bi-tree me-1"></i>Polizia Ambientale
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link fw-bold" id="edilizia-tab" data-bs-toggle="tab" data-bs-target="#tab-edilizia" type="button" role="tab" aria-controls="tab-edilizia" aria-selected="false">
                        <i class="bi bi-building me-1"></i>Edilizia
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link fw-bold" id="amministrativa-tab" data-bs-toggle="tab" data-bs-target="#tab-amministrativa" type="button" role="tab" aria-controls="tab-amministrativa" aria-selected="false">
                        <i class="bi bi-shop me-1"></i>Amministrativa
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link fw-bold" id="sommario-tab" data-bs-toggle="tab" data-bs-target="#tab-sommario" type="button" role="tab" aria-controls="tab-sommario" aria-selected="false">
                        <i class="bi bi-shop me-1"></i>Note
                    </button>
                </li>
            </ul>

            <!-- CONTENUTO DELLE SCHEDE (PANEL ASP.NET) -->
            <div class="tab-content" id="interventiTabContent">

                <!-- TAB 1: POLIZIA STRADALE -->
                <div class="tab-pane show active" id="tab-stradale" role="tabpanel" aria-labelledby="stradale-tab">
                    <asp:Panel ID="pnlPoliziaStradale" runat="server" CssClass="custom-border p-3 bg-white rounded shadow-sm">
                        <h5 class="fw-bold mb-3 text-dark"><i class="bi bi-table me-2"></i><strong>INTERVENTI POLIZIA STRADALE</strong></h5>
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered table-striped align-middle mb-0">
                                <thead class="table-dark text-center">
                                    <tr>
                                        <th scope="col" style="width: 50px;">N°</th>
                                        <th scope="col" style="width: 35%;">Tipologia</th>
                                        <th scope="col" style="width: 20%;">Controlli</th>
                                        <th scope="col" style="width: 20%;">Pv non contestati</th>
                                        <th scope="col" style="width: 20%;">Pv contestati</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">1</th>
                                        <td class="fw-bold">Codice della strada</td>
                                        <td>
                                            <asp:TextBox ID="txtContrCds" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvNoContestatiContrCds" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvContestatiContrCds" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">2</th>
                                        <td class="fw-bold">Occupazione di suolo pubblico</td>
                                        <td>
                                            <asp:TextBox ID="txtOccupSuoloPub" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvNoContestatiOccupSuoloPub" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvContestatiOccupSuoloPub" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">3</th>
                                        <td class="fw-bold">Autotrasporti (tutte le tipologie: edili, rifiuti, merci, etc.)</td>
                                        <td>
                                            <asp:TextBox ID="txtAutotrasporti" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvNoContestatiAutotrasporti" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvContestatiAutotrasporti" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">4</th>
                                        <td class="fw-bold">Targhe estere</td>
                                        <td>
                                            <asp:TextBox ID="txtTargheEstere" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvNoContestatiTargheEstere" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvContestatiTargheEstere" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">5</th>
                                        <td class="fw-bold">Parcheggiatori abusivi di cui art. 6 R.S.P.U. n° 75/22</td>
                                        <td>
                                            <asp:TextBox ID="txtParcheggiatoriAbusivi" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvNoContestatiParcheggiatoriAbusivi" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvContestatiParcheggiatoriAbusivi" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">6</th>
                                        <td class="fw-bold">Controlli altre forze polizia</td>
                                        <td>
                                            <asp:TextBox ID="txtContrAltreForze" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvNoContestatiContrAltreForze" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvContestatiContrAltreForze" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">7</th>
                                        <td class="fw-bold">Controlli micromobilità - Monopattini</td>
                                        <td>
                                            <asp:TextBox ID="txtContrMicroMobilita" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvNoContestatiContrMicroMobilita" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtPvContestatiContrMicroMobilita" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </asp:Panel>
                </div>

                <!-- TAB 2: POLIZIA AMBIENTALE -->
                <div class="tab-pane " id="tab-ambientale" role="tabpanel" aria-labelledby="ambientale-tab">
                    <asp:Panel ID="pnlPoliziaAmbientale" runat="server" CssClass="custom-border p-3 bg-white rounded shadow-sm">
                        <h5 class="fw-bold mb-3 text-dark"><i class="bi bi-table me-2"></i><strong>INTERVENTI POLIZIA AMBIENTALE</strong></h5>
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered table-striped align-middle mb-0">
                                <thead class="table-dark text-center">
                                    <tr>
                                        <th scope="col" style="width: 50px;">N°</th>
                                        <th scope="col" style="width: 50%;">Tipologia</th>
                                        <th scope="col" style="width: 25%;">Controlli</th>
                                        <th scope="col" style="width: 25%;">Sanzioni</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">8</th>
                                        <td class="fw-bold">Orario di sversamento e modalità di conferimento dei rifiuti solidi urbani</td>
                                        <td>
                                            <asp:TextBox ID="txtConferimentoRifiuti" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniConferimentoRifiuti" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">9</th>
                                        <td class="fw-bold">Deiezione canina</td>
                                        <td>
                                            <asp:TextBox ID="txtDeiezioneCanina" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniDeiezioneCanina" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">10</th>
                                        <td class="fw-bold">Legge antifumo</td>
                                        <td>
                                            <asp:TextBox ID="txtLeggeAntifumo" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniLeggeAntifumo" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">11</th>
                                        <td class="fw-bold">Residui abbandonati in area mercatale e pulizia area antistante attività commerciale</td>
                                        <td>
                                            <asp:TextBox ID="txtAreaMercatale" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniAreaMercatale" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">12</th>
                                        <td class="fw-bold">Controlli per inquinamento acustico</td>
                                        <td>
                                            <asp:TextBox ID="txtInquinamentoAcustico" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniInquinamentoAcustico" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </asp:Panel>
                </div>

                <!-- TAB 3: EDILIZIA -->
                <div class="tab-pane " id="tab-edilizia" role="tabpanel" aria-labelledby="edilizia-tab">
                    <asp:Panel ID="pnlEdilizia" runat="server" CssClass="custom-border p-3 bg-white rounded shadow-sm">
                        <h5 class="fw-bold mb-3 text-dark"><i class="bi bi-building me-2"></i><strong>INTERVENTI IN MATERIA EDILIZIA</strong></h5>
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered table-striped align-middle mb-0">
                                <thead class="table-dark text-center">
                                    <tr>
                                        <th scope="col" style="width: 50px;">N°</th>
                                        <th scope="col" style="width: 50%;">Tipologia</th>
                                        <th scope="col" style="width: 25%;">Controlli</th>
                                        <th scope="col" style="width: 25%;">Sanzioni</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">13</th>
                                        <td class="fw-bold">Controlli sui cantieri anche sottoposti a sequestro</td>
                                        <td>
                                            <asp:TextBox ID="txtContrCantieriSeq" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniContrCantieriSeq" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">14</th>
                                        <td class="fw-bold">Lavori edili senza protezione</td>
                                        <td>
                                            <asp:TextBox ID="txtLavoriSenzaProtez" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniLavoriSenzaProtez" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">15</th>
                                        <td class="fw-bold">Notifiche espletate in materia di sicurezza abitativa ed idrogeologica e provvedimenti a tutela della pubblica incolumità</td>
                                        <td>
                                            <asp:TextBox ID="txtNotificheSicurezza" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniNotificheSicurezza" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </asp:Panel>
                </div>

                <!-- TAB 4: AMMINISTRATIVA / COMMERCIALE -->
                <div class="tab-pane" id="tab-amministrativa" role="tabpanel" aria-labelledby="amministrativa-tab">
                    <asp:Panel ID="pnlAmministrativa" runat="server" CssClass="custom-border p-3 bg-white rounded shadow-sm">
                        <h5 class="fw-bold mb-3 text-dark"><i class="bi bi-shop me-2"></i><strong>INTERVENTI IN MATERIA AMMINISTRATIVA/COMMERCIALE</strong></h5>
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered table-striped align-middle mb-0">
                                <thead class="table-dark text-center">
                                    <tr>
                                        <th scope="col" style="width: 50px;">N°</th>
                                        <th scope="col" style="width: 50%;">Tipologia</th>
                                        <th scope="col" style="width: 25%;">Controlli</th>
                                        <th scope="col" style="width: 25%;">Sanzioni</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">16</th>
                                        <td class="fw-bold">Ambulanti</td>
                                        <td>
                                            <asp:TextBox ID="txtAmbulanti" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniAmbulanti" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">17</th>
                                        <td class="fw-bold">Indicazione di provenienza dei prodotti alimentari</td>
                                        <td>
                                            <asp:TextBox ID="txtProvenienzaAlimentare" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniProvenienzaAlimentare" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">18</th>
                                        <td class="fw-bold">Modalità igienico sanitarie e caratteristiche mezzi per trasporto alimentare</td>
                                        <td>
                                            <asp:TextBox ID="txtTrasportoAlimentare" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniTrasportoAlimentare" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">19</th>
                                        <td class="fw-bold">Officine auto moto riparatore</td>
                                        <td>
                                            <asp:TextBox ID="txtOfficine" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniOfficine" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <th scope="row" class="text-center bg-light">20</th>
                                        <td class="fw-bold">Controlli B&amp;B – LR n°5/2001 e attività extra alberghiere LR n° 17/2001</td>
                                        <td>
                                            <asp:TextBox ID="txtContrBnB" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                        <td>
                                            <asp:TextBox ID="txtSanzioniContrBnB" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </asp:Panel>
                </div>

                <!-- TAB 5: NOTE -->
                <div class="tab-pane" id="tab-sommario" role="tabpanel" aria-labelledby="sommario-tab">
                    <asp:Panel ID="pnlSommario" runat="server" CssClass="custom-border p-3 bg-white rounded shadow-sm">
                        <h5 class="fw-bold mb-3 text-dark"><i class="bi bi-shop me-2"></i><strong>NOTE</strong></h5>
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtNumControlliCds" class="fw-bold">N° Controlli CDS:</label>
                                <asp:TextBox ID="txtNumControlliCds" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtNumAutoveicoli" class="fw-bold">N° Autoveicoli:</label>
                                <asp:TextBox ID="txtNumAutoveicoli" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtNumMotoveicoli" class="fw-bold">N° Motoveicoli:</label>
                                <asp:TextBox ID="txtNumMotoveicoli" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <label for="txtNumCiclomotori" class="fw-bold">N° Ciclomotori:</label>
                                <asp:TextBox ID="txtNumCiclomotori" runat="server" CssClass="form-control form-control-sm text-end" TextMode="Number" min="0" placeholder="0"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row ">
                            <div class="form-group mb-12" style="margin-left: 30px">
                                <label for="txtAltreAttivita" class="fw-bold">Altre attività espletate:</label>
                                <asp:TextBox ID="txtAltreAttivita" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" MaxLength="255" Rows="12" Style="width: 100%; max-width: 800px;"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

    </div>



    <%-- Modale ricerca scheda --%>
    <div class="modal fade" id="ModalRicerca" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Schede</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtModPratica">Nr. Pratica/Cartellina:</label>
                        <asp:TextBox ID="txtModPratica" runat="server" CssClass="form-control" placeholder="Numero Pratica" autofocus="" />

                    </div>

                    <div class="form-group">
                        <label for="txtModPratica">Pattuglia:</label>
                        <asp:TextBox ID="txtModPattuglia" runat="server" CssClass="form-control" placeholder="Pattuglia" />

                    </div>

                    <div class="form-group">
                        <label for="txtModDataIntervento">Data Intervento:</label>
                        <asp:TextBox ID="txtModDataIntervento" runat="server" CssClass="form-control data-auto" placeholder="gg/mm/aaaa" />
                    </div>
                    <div class="form-group">
                        <label for="txtModAttivitàInterna">Attività Interna:</label>
                        <asp:CheckBox ID="ckModAttivitaInterna" runat="server" />
                    </div>
                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="GVRicercaRapportino" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVRicercaRapportino_PageIndexChanging" RowStyle-CssClass="GridViewRow"
                            AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                            <Columns>
                                <asp:BoundField DataField="ida" HeaderText="ID" Visible="false" />
                                <asp:BoundField DataField="pattuglia" HeaderText="Pattuglia" />
                                <asp:BoundField DataField="orario" HeaderText="Orario" />
                                <asp:BoundField DataField="itinerario" HeaderText="Itinerario" />
                                <asp:BoundField DataField="dataConsegna" HeaderText="Data Consegna" DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("pattuglia") + ";" + Eval("orario") + ";" + Eval("id") + ";" + Eval("itinerario")   %>' CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" Position="Top" />
                            <PagerStyle HorizontalAlign="Center" />
                            <PagerTemplate>
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
                <asp:HiddenField ID="HfIdScheda" runat="server" />
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>

    <%-- popup stampa scheda --%>
    <div class="modal fade" id="PopStampa" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-modal="true">
        <div class="modal-dialog"
            role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabelS">STAMPA SCHEDA</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">

                        <p id="MsgStampa" style="color: red"></p>

                    </div>

                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la stampa -->
                    <asp:Button ID="btPopStampa" runat="server" class="btn btn-primary me-3" Text="🖨️  Stampa" OnClick="btPopStampa_Click" ValidationGroup="bottoni" />

                    <asp:Button ID="Button1" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="hideModalS()" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

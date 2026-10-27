<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inserimento.aspx.cs" Inherits="GestUO.Inserimento" %>

<%@ Import Namespace="GestUO.Classi" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function showModal() {
            $('#ModalDataEvasa').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalDataEvasa').modal('hide');

        }
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        // Nasconde il popup
        function HideErrorMessage() {
            $('#errorModal').modal('hide');
        }
        // Mostra il popup
        function showModal() {
            $('#ModalQuartiere').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalQuartiere').modal('hide');
        }
        // Mostra il popup ricerca
        function showModal() {
            $('#ModalAvvertenze').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalAvvertenze').modal('hide');
        }

        //giudice
        function filterDropdownGiudice() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtGiudice");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlGiudice.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsList.ClientID %>');
            suggestionsListDiv.innerHTML = ""; // Pulisci la lista dei suggerimenti precedenti

            var suggestionsFound = false;

            if (filter.length > 0) { // Esegui il filtro solo se c'è testo nell'input
                for (i = 0; i < options.length; i++) {
                    txtValue = options[i].textContent || options[i].innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        suggestionsFound = true;
                        var suggestionElement = document.createElement("div");
                        suggestionElement.textContent = txtValue;
                        suggestionElement.classList.add("suggestion-item"); // Aggiungi una classe CSS per lo stile
                        suggestionElement.addEventListener('mouseover', function () { this.classList.add('suggestion-hover'); }); // Effetto hover con classe CSS
                        suggestionElement.addEventListener('mouseout', function () { this.classList.remove('suggestion-hover'); });
                        suggestionElement.addEventListener('click', function () {
                            input.value = this.textContent;
                            suggestionsListDiv.style.display = "none";
                        });
                        suggestionsListDiv.appendChild(suggestionElement);
                    }
                }
            }

            if (suggestionsFound) {
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }

        //tipo provvedimento
        function filterDropdownTipoProv() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtTipoProv");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlTipoProvvAg.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            // var suggestionsListDiv = document.getElementById("MainContent_suggestionsList");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListTipoProv.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //quartiere
        function filterDropdownQuartiere() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtQuartiere");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlQuartiere.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListQuartiere.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //provenienza
        function filterDropdownProvenienza() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtProvenienza");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlProvenienza.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListProvenienza.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //tipo atto
      <%--  function filterDropdownTipoAtto() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("DdlTipoAtto");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlTipoAtto.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListTipoAtto.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }--%>
        //Inviata
        <%--function filterDropdownInviata() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtInviata");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlInviati.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListInviata.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }--%>
        //Indirizzo
        function filterDropdownIndirizzo() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtIndirizzo");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlIndirizzo.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListIndirizzo.ClientID %>');
            // Pulisci la lista dei suggerimenti precedenti
            suggestionsListDiv.innerHTML = "";

            var suggestionsFound = false; // Flag per verificare se sono stati trovati suggerimenti

            for (i = 0; i < options.length; i++) {
                txtValue = options[i].textContent || options[i].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    suggestionsFound = true; // Trovato almeno un suggerimento
                    var suggestionElement = document.createElement("div"); // Crea un div per ogni suggerimento
                    suggestionElement.textContent = txtValue;
                    suggestionElement.style.padding = "5px";
                    suggestionElement.style.cursor = "pointer";
                    suggestionElement.onmouseover = function () { this.style.backgroundColor = '#e0e0e0'; }; // Effetto hover
                    suggestionElement.onmouseout = function () { this.style.backgroundColor = '#f9f9f9'; };

                    suggestionElement.addEventListener('click', function () {
                        input.value = this.textContent;
                        suggestionsListDiv.style.display = "none";
                        return false;
                    });
                    suggestionsListDiv.appendChild(suggestionElement); // Aggiungi il suggerimento alla lista
                }
            }

            // Mostra o nascondi la lista dei suggerimenti in base a se sono stati trovati suggerimenti
            if (suggestionsFound && filter.length > 0) { // Mostra solo se ci sono suggerimenti e c'è testo nel textbox
                suggestionsListDiv.style.display = "block";
            } else {
                suggestionsListDiv.style.display = "none";
            }
        }
        //nel textbox data sostituisce lo spazio con lo / 
       <%-- document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtDataInvio');
        // Se non usi ClientIDMode="Static", dovresti usare:
              // var textBox = document.getElementById('<%= txtDataInvio.ClientID %>');

            if (textBox) {
                textBox.addEventListener('input', function (event) {
                    // Salva la posizione attuale del cursore
                    var cursorPos = this.selectionStart;
                    var originalLength = this.value.length;

                    // Sostituisci tutti gli spazi con trattini
                    this.value = this.value.replace(/ /g, '/');

                    // Se la lunghezza è cambiata (cioè uno spazio è stato sostituito),
                    // e se l'ultimo carattere digitato era uno spazio (ora un trattino),
                    // riposiziona il cursore.
                    // Questa logica semplice funziona bene per sostituzioni 1 a 1.
                    if (this.value.length === originalLength) {
                        this.setSelectionRange(cursorPos, cursorPos);
                    } else {
                        // Se più spazi sono stati sostituiti o incollati,
                        // il cursore potrebbe andare alla fine.
                        // Per la semplice digitazione di uno spazio,
                        // cursorPos dovrebbe essere corretto.
                        this.setSelectionRange(cursorPos, cursorPos);
                    }
                });
            } else {
                console.error("Textbox con ID 'txtDataInvio' non trovata.");
            }
        });--%>
       <%-- document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtDataDataEvasa');
        // Se non usi ClientIDMode="Static", dovresti usare:
            // var textBox = document.getElementById('<%= txtDataDataEvasa.ClientID %>');

            if (textBox) {
                textBox.addEventListener('input', function (event) {
                    // Salva la posizione attuale del cursore
                    var cursorPos = this.selectionStart;
                    var originalLength = this.value.length;

                    // Sostituisci tutti gli spazi con trattini
                    this.value = this.value.replace(/ /g, '/');

                    // Se la lunghezza è cambiata (cioè uno spazio è stato sostituito),
                    // e se l'ultimo carattere digitato era uno spazio (ora un trattino),
                    // riposiziona il cursore.
                    // Questa logica semplice funziona bene per sostituzioni 1 a 1.
                    if (this.value.length === originalLength) {
                        this.setSelectionRange(cursorPos, cursorPos);
                    } else {
                        // Se più spazi sono stati sostituiti o incollati,
                        // il cursore potrebbe andare alla fine.
                        // Per la semplice digitazione di uno spazio,
                        // cursorPos dovrebbe essere corretto.
                        this.setSelectionRange(cursorPos, cursorPos);
                    }
                });
            } else {
                console.error("Textbox con ID 'txtDataDataEvasa' non trovata.");
            }
        });--%>

    </script>


    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <%--<asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>--%>
            <div class="dashboard-header">
                <h1><span class="fa-solid fa-gear fa-spin"></span><u><%= TitoloUO %> </u></h1>
            </div>
        </div>

        <div class="container">

            <div class="tab-content">
                <p style="font-weight: bold; font-size: medium">Dati Generali</p>

                <div class="row custom-border">
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtProt">Nr Carico</label>
                            <asp:TextBox ID="txtProt" runat="server" CssClass="form-control" ForeColor="Red" Enabled="false" Font-Bold="true" ClientIDMode="Static" />
                        </div>
                    </div>

                   <%-- <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="DdlSigla">Sigla</label>
                            <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control" onchange="gestisciVisibilita();">
                                <asp:ListItem Text="ED" Value="ED"></asp:ListItem>
                                <asp:ListItem Text="TP" Value="TP"></asp:ListItem>
                                <asp:ListItem Text="AG" Value="AG"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>--%>

                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtDataInsCarico">Data Inserimento</label>
                            <asp:TextBox ID="txtDataInsCarico" runat="server" CssClass="form-control data-auto" Font-Bold="true" />
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtRifProtGen">Protocollo Generale</label>
                            <asp:TextBox ID="txtRifProtGen" runat="server" CssClass="form-control" autofocus="" onkeyup="contaPuntiVirgola();" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRifProtGen" ErrorMessage="* Inserire Riferimento Prot. Gen." ValidationGroup="bt" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>
                </div>

                <div class="row custom-border mt-3">
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtSearchAtto">Tipologia Atto</label>
                            <asp:HiddenField ID="HfTipoAtto" runat="server" />
                            <input type="text" id="txtSearchAtto" runat="server" class="form-control" placeholder="Cerca..." onkeyup="filterAndHighlight(event)" autocomplete="off" />
                            <asp:DropDownList ID="DdlTipoAtto" runat="server" Style="display: none;"></asp:DropDownList>
                            <div id="suggestionsListTipoAtto"></div>
                            <div id="dropdownList" class="dropdown-content"></div>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtTipoAtto">Ulteriore Atto</label>
                            <asp:TextBox ID="txtTipoAtto" runat="server" MaxLength="100" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtProvenienza">Provenienza</label>
                            <asp:TextBox ID="txtProvenienza" runat="server" onkeyup="filterDropdownProvenienza()" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListProvenienza" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 100%;">
                                <asp:HiddenField ID="HfProvenienza" runat="server" />
                            </div>
                            <asp:DropDownList ID="DdlProvenienza" runat="server" Style="display: none" />
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="txtNumProtRicStessoCarico">Numeri protocollo</label>
                            <asp:TextBox ID="txtNumProtRicStessoCarico" runat="server" CssClass="form-control larghezzaText70" MaxLength="3" ClientIDMode="Static" />

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNumProtRicStessoCarico" ErrorMessage="* Inserire numero prot." ValidationGroup="bt" ForeColor="Red" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtNumProtRicStessoCarico" ErrorMessage="* Solo numeri" ForeColor="Red" ValidationExpression="\d{1,3}" Display="Dynamic" />
                        </div>
                    </div>
                </div>

                <p style="font-weight: bold; font-size: medium">Dati Relativi Alla Pratica</p>

                <div class="row custom-border">
                    <div class="col-md-4">
                        <!-- Indirizzo e TextBox sulla stessa riga -->
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="DdlIndirizzo">Indirizzo</label>
                            <div class="row">
                                <!-- DropDownList occupa metà spazio -->
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtIndirizzo" runat="server" AutoPostBack="false" onkeyup="filterDropdownIndirizzo()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    <div id="suggestionsListIndirizzo" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                        <asp:HiddenField ID="HfIndirizzo" runat="server" />
                                    </div>
                                    <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" Style="display: none" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtQuartiere">Quartiere</label>
                            <asp:TextBox ID="txtQuartiere" runat="server" AutoPostBack="false" onkeyup="filterDropdownQuartiere()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListQuartiere" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                            </div>
                            <asp:DropDownList ID="DdlQuartiere" runat="server" CssClass="form-control" Style="display: none" />
                        </div>

                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtNominativo">Nominativo</label>
                            <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control" />
                        </div>


                    </div>
                    <div class="col-md-4">
                        <div class="row">
                            <!-- Prima TextBox -->
                            <div class="col-6">
                                <div class="form-group mb-3" style="margin-left: -10px">
                                    <label for="txPratica">Pratica Edilizia</label>
                                    <asp:TextBox ID="txPratica" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <!-- Nuova TextBox -->
<%--                            <div class="col-6">
                                <div class="form-group mb-3" style="margin-left: -10px">
                                    <label for="txtCartellina">Cartellina Patrimonio</label>
                                    <asp:TextBox ID="txtCartellina" runat="server" CssClass="form-control" />
                                </div>
                            </div>--%>
                        </div>
                    </div>
                    <%--<div class="col-md-4">
                        <div class="form-group mb-3" style="margin-left: -25px">
                            <label for="txPratica">Pratica</label>
                            <asp:TextBox ID="txPratica" runat="server" CssClass="form-control" />
                        </div>


                    </div>--%>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="DdlAreaCompetenza">Area Competenza</label>
                            <%--<asp:TextBox ID="txtAreaCompetenza" runat="server" CssClass="form-control mb-3" />--%>
                            <asp:DropDownList ID="DdlAreaCompetenza" runat="server" CssClass="form-control">
                                <asp:ListItem Text=""> </asp:ListItem>
                                <asp:ListItem Text="ARCHIVIO"> </asp:ListItem>
                                <asp:ListItem Text="ATTI"> </asp:ListItem>
                                <asp:ListItem Text="CDR"> </asp:ListItem>
                                <asp:ListItem Text="FURERIA"> </asp:ListItem>
                                <asp:ListItem Text="NOTIFICATORI"> </asp:ListItem>
                                <asp:ListItem Text="PG"> </asp:ListItem>
                                <asp:ListItem Text="SOPRALLUOGO"> </asp:ListItem>
                                <asp:ListItem Text="UFFICIO TRASMISSIONI"> </asp:ListItem>
                                <asp:ListItem Text="URP"> </asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtDataCarico">Data Carico</label>
                            <asp:TextBox ID="txtDataCarico" runat="server" CssClass="form-control data-auto"></asp:TextBox>
                        </div>

                    </div>
                    <div class="col-md-4" id="divbu" runat="server" style="display: none;">
                        <div class="form-group mb-3" >
                            <label for="txtBU">BU</label>
                            <asp:TextBox ID="txtBU" runat="server" CssClass="form-control" />
                        </div>

                    </div>

                    <div class="col-md-4" id="divcd" runat="server" style="display: none;">
                        <div class="form-group mb-3">
                            <label for="txtCodEdificio">Codice Edificio</label>
                            <asp:TextBox ID="txtCodEdificio" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>
                <div id="divAg" runat="server" style="display: none;">

                    <p style="font-weight: bold; font-size: medium">Dati AG</p>
                    <div class="row custom-border">
                        <div class="col-md-4">
                            <div class="form-group mb-3" style="margin-left: -25px">
                                <label for="txtGiudice">Giudice</label>
                                <asp:TextBox ID="txtGiudice" runat="server" AutoPostBack="false" onkeyup="filterDropdownGiudice()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                <div id="suggestionsList" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                    <asp:HiddenField ID="HfGiudice" runat="server" />
                                </div>
                                <%--<asp:Button ID="btSalvaGiudice" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaGiudice_Click" Visible="false" />--%>

                                <asp:DropDownList ID="DdlGiudice" runat="server" Style="display: none;" CssClass="form-control" />
                            </div>
                            <div class="form-group mb-3" style="margin-left: -25px">
                                <label for="txtDataDelega">Data Delega</label>
                                <asp:TextBox ID="txtDataDelega" runat="server" CssClass="form-control data-auto"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-3">
                                <label for="DdlTipoProvvAg">Tipo Provvedimento AG</label>

                                <asp:TextBox ID="txtTipoProv" runat="server" AutoPostBack="false" onkeyup="filterDropdownTipoProv()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control" Visible="false"></asp:TextBox>
                                <div id="suggestionsListTipoProv" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                    <asp:HiddenField ID="HfTipoProv" runat="server" />

                                </div>
                                <%--<asp:Button ID="btSalvaTipoProvv" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoProvv_Click" Visible="false" />--%>
                                <asp:DropDownList ID="DdlTipoProvvAg" runat="server" CssClass="form-control" />
                            </div>

                            <div class="form-group mb-3">
                                <label for="txtGgDelega">Termine gg. delega</label>
                                <asp:TextBox ID="txtGgDelega" runat="server" AutoPostBack="false" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtGgDelega" ErrorMessage="* Solo numeri" ForeColor="Red" ValidationExpression="\d{1,3}" Display="Dynamic" />
                            </div>


                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-3">
                                <label for="txtProdPenNr">Procedimento Penale nr</label>
                                <asp:TextBox ID="txtProdPenNr" runat="server" CssClass="form-control" />

                            </div>

                        </div>
                    </div>
                </div>

                <%--               <div id="div1" runat="server">

                    <p style="font-weight: bold; font-size: medium">Esiti</p>
                    <div class="row custom-border">
                        <div class="col-md-4">
                            <div class="form-group mb-3" style="margin-left: -25px">

                                <div class="form-check">
                                    <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" Enabled="false" />
                                    <label class="form-check-label ms-2" for="CkEvasa">Trasmessa</label>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>--%>
            </div>






            <asp:HiddenField ID="Hid" runat="server" />

            <div class="row">
                <div class="col-12 text-center">
                    <asp:Button ID="btSalva" Text="Salva" runat="server" OnClick="Salva_Click" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    <asp:Button ID="btNewIns" Text="Nuovo Inserimento" runat="server" OnClick="btNewIns_Click" CssClass="btn btn-primary mt-3" Visible="false" />
                    <asp:Button Text="Cerca Quartiere" runat="server" OnClick="apripopup_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" />



                </div>
            </div>
        </div>
    </div>


    <!-- Modale Bootstrap quartiere -->
    <div class="modal fade" id="ModalQuartiere" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Quartiere</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtIndirizzoQuartiere">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzoQuartiere" runat="server" CssClass="form-control" ClientIDMode="Static" placeholder="Campo obbligatorio" autofocus="" />

                    </div>

                    <div class="form-group">
                        <!-- GridView nel popup -->
                        <asp:GridView ID="gvPopup" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                            OnRowDataBound="gvPopup_RowDataBound" OnRowCommand="gvPopup_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID_quartiere" HeaderText="ID" />
                                <asp:BoundField DataField="Toponimo" HeaderText="Toponimo" />
                                <asp:BoundField DataField="Quartiere" HeaderText="Quartiere" />
                                <asp:BoundField DataField="Specie" HeaderText="Specie" />
                                <asp:BoundField DataField="Nota" HeaderText="Nota" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("Quartiere") %>' CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="RicercaQuartiere_Click" />
                    <asp:Button ID="btnchiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClientClick="hideModal()" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modale Richiesta decretazione -->
    <div class="modal fade" id="ModalRicDecretazione" tabindex="-1" aria-labelledby="modalLabel">
        <div class="modal-dialog">
            <div class="modal-content">
                <div id="modalHeaderColor" runat="server" class="modal-header" style="background-color: #DFF0D8">
                    <h4 class="modal-title">
                        <asp:Label ID="modalLabel1" runat="server" Text="✅ Inserimento Carico" />
                    </h4>

                </div>
                <div class="modal-body text-center">
                    <p id="Message" class="lead"></p>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="Decreta" runat="server" CssClass="btn btn-primary" Text="Decreta" data-toggle="modal" data-target="#ModalDecretazione" data-dismiss="modal" />
                    <!-- Nel pulsante "Sì" dentro il primo popup OnClick="Decreta_Click"-->
                    <%--<button type="button" CssClass="btn btn-primary" data-dismiss="modal" data-toggle="modal" data-target="#ModalDecretazione">Decreta</button>--%>
                    <asp:Button ID="Button4" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupModalRicDecretazione_Click" />
                </div>
            </div>
        </div>

    </div>

    <%-- popup avvertenze --%>
    <div class="modal fade" id="ModalAvvertenze" tabindex="-1" role="dialog" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog"
            role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel6">ATTENZIONE</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">

                        <p id="errorAvvertenze" style="color: red"></p>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btChiudiAvvertenze" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="btChiudiAvvertenze_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Popup Modale inserimento data evasa -->
    <div class="modal fade" id="ModalDataEvasa" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 20%">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel4">Inserisci La Data Evasa</h5>

                </div>
                <div id="Div3" runat="server" class="row" style="margin-left: 30px!important">
                    <div class="form-group mb-3">
                        <label for="txtdataEvasaPopup">Data Evasa</label>
                        <asp:TextBox ID="txtdataEvasaPopup" runat="server" CssClass="form-control data-auto"></asp:TextBox>
                    </div>
                </div>

                <div class="modal-footer">
                    <!-- Bottone per avviare chiousura decretazione -->
                    <asp:Button ID="ModalChiudiDecretazione" runat="server" class="btn btn-secondary" Text="Salva" OnClick="ModalChiudiDecretazione_Click" />
                </div>
            </div>
        </div>
    </div>
    <%-- Modale ModalDecretazione --%>
    <div class="modal fade" id="ModalDecretazione" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" style="width: 100%">
            <%--<div class="container" id="DivDettagli" runat="server">--%>
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel3">Decretazione</h5>

                </div>
                <div id="DivDecretazione" runat="server">
                    <div class="row custom-border" style="margin-left: 0px!important">
                        <div class="col-md-3 " style="margin-left: 20px!important">
                            <div class="form-group mb-3">
                                <label for="txtPraticaDecr">Pratica</label>
                                <asp:TextBox ID="txtPraticaDecr" runat="server" Enabled="false" CssClass="form-control mb-3" Width="120px" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDecretante">Decretante</label>
                                <asp:TextBox ID="txtDecretante" runat="server" CssClass="form-control mb-3" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDecretato">Decretato</label>
                                <%--<asp:TextBox ID="txtDecretato" runat="server" CssClass="form-control mb-3"></asp:TextBox>--%>


                                <input type="text" id="txtSearchOperatore" runat="server" class="form-control"
                                    placeholder="cerca..."
                                    onkeyup="filterAndHighlightOp(event)"
                                    autocomplete="off" />

                                <!-- 2. DROPDOWNLIST REALE (NASCOSTA) - Serve per il C# -->
                                <asp:DropDownList ID="ddlOperatore" runat="server" Style="display: none"></asp:DropDownList>
                                <ul id="suggestionsListoperatore"></ul>
                                <!-- 3. LISTA VISIVA (Finta Dropdown) -->
                                <div id="dropdownList1" class="dropdown-content">
                                    <!-- Verrà riempita da Javascript -->
                                </div>





                                <asp:RequiredFieldValidator ID="RfDecretato" runat="server" ControlToValidate="txtSearchOperatore" ValidationGroup="btDecretazione" ErrorMessage="Inserire decretato" ForeColor="Red">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDataDecretazione">Data</label>
                                <asp:TextBox ID="txtDataDecretazione" runat="server" CssClass="form-control mb-3 data-auto" ClientIDMode="Static"></asp:TextBox>

                            </div>

                            <div class="form-group mb-3">
                                <asp:Button ID="btAggiungiDecretazione" runat="server" CssClass="btn btn-primary mt-3" Text="Aggiungi" OnClick="btAggiungiDecretazione_Click" ValidationGroup="btDecretazione" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-3" style="margin-left: -100px!important">
                                <div class="form-group">
                                    <!-- GridView nel popup -->
                                    <asp:GridView ID="GVDecretazione" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered gridview-autofit"
                                        OnRowDataBound="GVDecretazione_RowDataBound" OnRowCommand="GVDecretazione_RowCommand" AllowPaging="true" PageSize="5" OnPageIndexChanging="GVDecretazione_PageIndexChanging">
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
                                            <%--<asp:BoundField DataField="decr_dataChiusura" HeaderText="Data Chiusura" DataFormatString="{0:dd/MM/yyyy}" />--%>

                                            <asp:TemplateField HeaderText="Data Chiusura">
                                                <ItemTemplate>
                                                    <%# Routine.FormatMyDate(Eval("decr_dataChiusura")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                             <ItemTemplate>
                                                 <asp:Button ID="Button1" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("Quartiere") %>' CssClass="btn btn-success btn-sm" />
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
                                    <asp:Button ID="btChiudiDecretazione" runat="server" Text="Chiudi Decretazione" OnClick="btChiudiDecretazione_Click" CommandName="Select" CommandArgument='<%# Eval("decr_pratica") + "|" + Eval("decr_idPratica") %>' CssClass="btn btn-success btn-sm" />

                                </div>
                            </div>
                        </div>
                        <div class="col-md-3" style="margin-left: 400px!important">
                            <div class="form-group mb-3">
                                <label for="txtNotaDecretazione">Nota</label>
                                <asp:TextBox ID="txtNotaDecretazione" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" MaxLength="255" Rows="12" Style="width: 100%; max-width: 600px;"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>

                <%-- FOOTER --%>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="Button3" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupDecretazione_Click" />
                </div>
            </div>
        </div>
    </div>
   <%-- <script type="text/javascript">
        // Funzione per gestire la visibilità
        function gestisciVisibilita() {
            var divag = document.getElementById('<%= divAg.ClientID %>');
            var divbu = document.getElementById('<%= divbu.ClientID %>');
            var divcd = document.getElementById('<%= divcd.ClientID %>');
            // Recupera gli altri elementi, verificando sempre
            var ddl = document.getElementById('<%= DdlSigla.ClientID %>');

            // mostrare/nascondere
            if (ddl.value === 'AG') {
                divag.style.display = 'block';

            } else {
                divag.style.display = 'none';
            }
            if (ddl.value === 'TP') {
                divbu.style.display = 'block';
                divcd.style.display = 'block';
            } else {
                divbu.style.display = 'none';
                divcd.style.display = 'none';
            }
        }
        //al caricamento della pagian effettua il primno controllo
        document.addEventListener('DOMContentLoaded', gestisciVisibilita);
        var ddl = document.getElementById('<%= DdlSigla.ClientID %>');
        if (ddl) {
            ddl.onchange = gestisciVisibilita;
        }
        /////
        var allOptionsTipoAtto = [];

        // Funzione per caricare i dati (chiamata solo quando serve)
        function caricaOpzioni() {
            var ddl = document.getElementById('<%= DdlTipoAtto.ClientID %>');

            if (!ddl) {
                console.error("Errore: DropDownList 'DdlTipoAtto' non trovata!");
                return;
            }

            var options = ddl.options;
            allOptionsTipoAtto = []; // Resetta

            for (var i = 0; i < options.length; i++) {
                // Carica tutto tranne i valori vuoti
                if (options[i].value !== "" && options[i].value !== "0") {
                    allOptionsTipoAtto.push({ text: options[i].text, value: options[i].value });
                }
            }
            console.log("Opzioni caricate in memoria atti: " + allOptionsTipoAtto.length);
        }

        // Carica i dati appena la pagina è pronta
        document.addEventListener("DOMContentLoaded", caricaOpzioni);

        // Funzione Filtro
        function filterAndHighlight(e) {
            var input = document.getElementById('<%= txtSearchAtto.ClientID %>');

            var listDiv = document.getElementById("suggestionsListTipoAtto");
            //var filter = input.value.toUpperCase();
            var filter = (input.value || "").toUpperCase();

            // Se l'array è vuoto (es. UpdatePanel ha resettato), ricaricalo
            if (allOptionsTipoAtto.length === 0) {
                caricaOpzioni();
            }

            // Tasto INVIO (13)
            if (e.keyCode === 13) {
                var activeItem = listDiv.querySelector(".active");
                if (activeItem) {
                    // Simula il click
                    activeItem.click();
                    e.preventDefault(); // Ferma il postback del form se presente
                }
                return;
            }

            listDiv.innerHTML = "";

            // Se input vuoto, nascondi
            if (filter.length === 0) {
                listDiv.style.display = "none";
                return;
            }

            var foundCount = 0;

            for (var i = 0; i < allOptionsTipoAtto.length; i++) {
                var item = allOptionsTipoAtto[i];

                // LOGICA DI FILTRO (Contiene il testo?)
                if (item.text.toUpperCase().indexOf(filter) > -1) {

                    var div = document.createElement("div");
                    div.className = "suggestion-item";
                    div.innerText = item.text;

                    // Usiamo attributi data- per passare il valore
                    div.setAttribute("data-val", item.value);

                    // Evidenzia il primo risultato
                    if (foundCount === 0) {
                        div.classList.add("active");
                    }

                    // Click Mouse
                    div.onclick = function () {
                        seleziona(this.innerText, this.getAttribute("data-val"));
                    };

                    listDiv.appendChild(div);
                    foundCount++;
                }
            }

            console.log("Risultati trovati: " + foundCount);

            if (foundCount > 0) {
                listDiv.style.display = "block";
            } else {
                listDiv.style.display = "none";
            }
        }

        function seleziona(text, value) {
            console.log("Selezionato: " + text + " (ID: " + value + ")");

            var input = document.getElementById('<%= txtSearchAtto.ClientID %>');//document.getElementById("txtSearchAtto");
            var ddl = document.getElementById('<%= DdlTipoAtto.ClientID %>');
            var listDiv = document.getElementById("suggestionsListTipoAtto");

            input.value = text;
            if (ddl) ddl.value = value;
            listDiv.style.display = "none";
        }

        // Chiudi se clicchi fuori
        document.addEventListener('click', function (e) {
            if (e.target.id !== document.getElementById('<%= txtSearchAtto.ClientID %>')) {
                document.getElementById("suggestionsListTipoAtto").style.display = "none";
            }
        });

        // Funzione per caricare i dati (chiamata solo quando serve)
        function caricaOpzioniOperatore() {
            var ddl = document.getElementById('<%= ddlOperatore.ClientID %>');

            if (!ddl) {
                console.error("Errore: DropDownList 'ddlOperatore' non trovata!");
                return;
            }

            var options = ddl.options;
            allOptions = []; // Resetta

            for (var i = 0; i < options.length; i++) {
                // Carica tutto tranne i valori vuoti
                if (options[i].value !== "" && options[i].value !== "0") {
                    allOptions.push({ text: options[i].text, value: options[i].value });
                }
            }
            console.log("Opzioni caricate in memoria: " + allOptions.length);
        }

        // Carica i dati appena la pagina è pronta
        document.addEventListener("DOMContentLoaded", caricaOpzioniOperatore);

        function filterAndHighlightOp(e) {
            var input = document.getElementById('<%= txtSearchOperatore.ClientID %>');

            var listDiv = document.getElementById("suggestionsListoperatore");
            //var filter = input.value.toUpperCase();
            var filter = (input.value || "").toUpperCase();

            // Se l'array è vuoto (es. UpdatePanel ha resettato), ricaricalo
            if (allOptions.length === 0) {
                caricaOpzioniOperatore();
            }

            // Tasto INVIO (13)
            if (e.keyCode === 13) {
                var activeItem = listDiv.querySelector(".active");
                if (activeItem) {
                    // Simula il click
                    activeItem.click();
                    e.preventDefault(); // Ferma il postback del form se presente
                }
                return;
            }

            listDiv.innerHTML = "";

            // Se input vuoto, nascondi
            if (filter.length === 0) {
                listDiv.style.display = "none";
                return;
            }

            var foundCount = 0;

            for (var i = 0; i < allOptions.length; i++) {
                var item = allOptions[i];

                // LOGICA DI FILTRO (Contiene il testo?)
                if (item.text.toUpperCase().indexOf(filter) > -1) {

                    var div = document.createElement("div");
                    div.className = "suggestion-item";
                    div.innerText = item.text;

                    // Usiamo attributi data- per passare il valore
                    div.setAttribute("data-val", item.value);

                    // Evidenzia il primo risultato
                    if (foundCount === 0) {
                        div.classList.add("active");
                    }

                    // Click Mouse
                    div.onclick = function () {
                        selezionaOperatore(this.innerText, this.getAttribute("data-val"));
                    };

                    listDiv.appendChild(div);
                    foundCount++;
                }
            }

            console.log("Risultati trovati: " + foundCount);

            if (foundCount > 0) {
                listDiv.style.display = "block";
            } else {
                listDiv.style.display = "none";
            }
        }
        function selezionaOperatore(text, value) {
            console.log("Selezionato: " + text + " (ID: " + value + ")");

            var input = document.getElementById('<%= txtSearchOperatore.ClientID %>');
            var ddl = document.getElementById('<%= ddlOperatore.ClientID %>');
            var listDiv = document.getElementById("suggestionsListoperatore");

            input.value = text;
            if (ddl) ddl.value = value;
            listDiv.style.display = "none";
        }

        // Chiudi se clicchi fuori
        document.addEventListener('click', function (e) {
            if (e.target.id !== document.getElementById('<%= txtSearchOperatore.ClientID %>')) {
                suggestionsListoperatore
            }
        });
        function contaPuntiVirgola() {
            var sorgente = document.getElementById('<%= txtRifProtGen.ClientID %>');
            var destinazione = document.getElementById('<%= txtNumProtRicStessoCarico.ClientID %>');

            if (sorgente && destinazione) {
                var testo = sorgente.value.trim();

                // Se il campo è vuoto, il conteggio è 0
                if (testo === "") {
                    destinazione.value = 0;
                } else {
                    // Dividiamo per ";"
                    // Esempio: "A;B" -> ["A", "B"] -> lunghezza 2
                    // Esempio: "A"   -> ["A"]      -> lunghezza 1
                    var parti = testo.split(";");

                    // Opzionale: filtriamo eventuali spazi vuoti se l'utente mette ;; per errore
                    // var conteggio = parti.filter(function(x) { return x.trim() !== "" }).length;

                    var conteggio = parti.length;
                    destinazione.value = conteggio;
                }
            }
        }
        ////
        $(document).ready(function () {

            $('#Decreta').on('click', function () {

                // 1. txtPraticaDecr.Text = txtProt.Text;
                // Prendiamo il valore dall'ID del textbox di origine e lo copiamo in quello di destinazione
                var protocollo = $('#<%= txtProt.ClientID %>').val();
                $('#<%= txtPraticaDecr.ClientID %>').val(protocollo);

                // 2. txtDataDecretazione.Text = DateTime.Now.ToString("dd/MM/yyyy");
                // Calcoliamo la data di oggi in formato italiano direttamente col browser
                var oggi = new Date();
                var dd = String(oggi.getDate()).padStart(2, '0');
                var mm = String(oggi.getMonth() + 1).padStart(2, '0'); // Gennaio è 0!
                var yyyy = oggi.getFullYear();
                var dataFormattata = dd + '/' + mm + '/' + yyyy;

                $('#<%= txtDataDecretazione.ClientID %>').val(dataFormattata);

                // 3. txtDecretante.Text = operatore...
                // Recuperiamo il nome dell'operatore che C# ha stampato nella Session
                var nomeOperatore = '<%= Session["NomeOperatore"] %>';
                if (nomeOperatore !== '') {
                    $('#<%= txtDecretante.ClientID %>').val(nomeOperatore);
                }

            });

        });
    </script>--%>
</asp:Content>

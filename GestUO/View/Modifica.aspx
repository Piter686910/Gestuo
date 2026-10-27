<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Modifica.aspx.cs" Inherits="GestUO.Modifica" %>

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
        //function ShowErrorMessage(message) {
        //    $('#errorModal').modal('show');
        //}
        //function openPopup() {
        //    document.getElementById("popupModal").style.display = "block";
        //    document.getElementById("overlay").style.display = "block";
        //}
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
            $('#ModalRicerca').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalRicerca').modal('hide');
        }
        // Mostra il popup ricerca
        function showModal() {
            $('#ModalAvvertenze').modal('show');
        }

        // Nasconde il popup
        function hideModal() {
            $('#ModalAvvertenze').modal('hide');
        }

        // Mostra il popup Decretazione
        function showModalDecretazione() {
            $('#ModalDecretazione').modal('show');
        }

        // Nasconde il popup
        function hideModalDecretazione() {
            $('#ModalDecretazione').modal('hide');
        }
        function hideModalRichiestaSalva() {
            $('#ModalRichiestaSalva').modal('hide');
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
        //ESITO
        function filterDropdownEsito() {
            var input, filter, dropdown, options, i, txtValue;
            input = document.getElementById("txtEsito");
            filter = input.value.toUpperCase();
            dropdown = document.getElementById('<%= DdlEsito.ClientID %>');
            options = dropdown.getElementsByTagName("option");
            var suggestionsListDiv = document.getElementById('<%= suggestionsListEsito.ClientID %>');
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
        //nel textbox data sostituisce lo spazio con lo / 
        document.addEventListener('DOMContentLoaded', function () {
            var textBox = document.getElementById('txtDataCarico');
        // Se non usi ClientIDMode="Static", dovresti usare:
             // var textBox = document.getElementById('<%= txtDataCarico.ClientID %>');

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
                console.error("Textbox con ID 'txtDataCarico' non trovata.");
            }
        });
        //document.addEventListener('DOMContentLoaded', function () {
        //    var textBox = document.getElementById('txtDataInvio');


        //    if (textBox) {
        //        textBox.addEventListener('input', function (event) {
        //            // Salva la posizione attuale del cursore
        //            var cursorPos = this.selectionStart;
        //            var originalLength = this.value.length;

        //            // Sostituisci tutti gli spazi con trattini
        //            this.value = this.value.replace(/ /g, '/');

        //            // Se la lunghezza è cambiata (cioè uno spazio è stato sostituito),
        //            // e se l'ultimo carattere digitato era uno spazio (ora un trattino),
        //            // riposiziona il cursore.
        //            // Questa logica semplice funziona bene per sostituzioni 1 a 1.
        //            if (this.value.length === originalLength) {
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            } else {
        //                // Se più spazi sono stati sostituiti o incollati,
        //                // il cursore potrebbe andare alla fine.
        //                // Per la semplice digitazione di uno spazio,
        //                // cursorPos dovrebbe essere corretto.
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            }
        //        });
        //    } else {
        //        console.error("Textbox con ID 'txtDataInvio' non trovata.");
        //    }
        //});
        //document.addEventListener('DOMContentLoaded', function () {
        //    var textBox = document.getElementById('txtDataDataEvasa');


        //    if (textBox) {
        //        textBox.addEventListener('input', function (event) {
        //            // Salva la posizione attuale del cursore
        //            var cursorPos = this.selectionStart;
        //            var originalLength = this.value.length;

        //            // Sostituisci tutti gli spazi con trattini
        //            this.value = this.value.replace(/ /g, '/');

        //            // Se la lunghezza è cambiata (cioè uno spazio è stato sostituito),
        //            // e se l'ultimo carattere digitato era uno spazio (ora un trattino),
        //            // riposiziona il cursore.
        //            // Questa logica semplice funziona bene per sostituzioni 1 a 1.
        //            if (this.value.length === originalLength) {
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            } else {
        //                // Se più spazi sono stati sostituiti o incollati,
        //                // il cursore potrebbe andare alla fine.
        //                // Per la semplice digitazione di uno spazio,
        //                // cursorPos dovrebbe essere corretto.
        //                this.setSelectionRange(cursorPos, cursorPos);
        //            }
        //        });
        //    } else {
        //        console.error("Textbox con ID 'txtDataDataEvasa' non trovata.");
        //    }
        //});

        //rende visibile un textbox su onclientclick
       <%-- function TextBoxVisibility1(buttonReference) {
        // Otteniamo un riferimento alla TextBox usando il suo ClientID generato da ASP.NET
            // `<%= div1.ClientID %>` inserisce l'ID reale della TextBox nell'HTML
            var textBox = document.getElementById('<%= div1.ClientID %>');

            if (textBox) {
                if (textBox.style.visibility === 'hidden') {
                    // Se la TextBox è nascosta, la rendiamo visibile
                    textBox.style.visibility = 'visible'; // O 'inline', 'inline-block' a seconda del contesto
                    //buttonReference.value = 'Nascondi Textbox'; // Cambia il testo del bottone
                } else {
                    // Se la TextBox è visibile, la nascondiamo
                    textBox.style.display = 'none';
                    //buttonReference.value = 'Mostra Textbox'; // Cambia il testo del bottone
                }
            }

            // Restituire 'false' impedisce il postback al server.
            // Se non restituisci 'false', il click attiverà ANCHE l'evento OnClick lato server.
            return false;
        }--%>
        //tipo atto
<%--        function filterDropdownTipoAtto() {
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

    </script>

    <div class="jumbotron">
        <div style="margin-top: -50px!important">
            <%--  <asp:Literal ID="ProtocolloLiteral" runat="server"></asp:Literal>
            <p class="text-center lead">MODIFICA CARICO</p>--%>
            <div class="dashboard-header">
                <h1><span class="fa-solid fa-gear fa-spin"></span>MODIFICA CARICO</h1>
            </div>
            <asp:HiddenField ID="HfButtonProv" runat="server" />

            <!-- Contenitore per centrare -->

            <asp:Panel ID="pnlButton" runat="server" CssClass="" Visible="true">
                <div class="d-flex justify-content-center align-items-center w-100 my-4">
                    <table style="margin: 0 auto; border-collapse: separate; border-spacing: 6px;">
                        <tr>
                            <!-- Pulsanti -->
                            <td>
                                <asp:Button ID="btNProtocollo" runat="server" OnClick="btNProtocollo_Click" Text="Nr. Carico" CssClass="btn btn-primary mx-2" ToolTip="Ricerca per numero carico" />
                            <td>
                            <td>
                                <asp:Button ID="btProcPenale" runat="server" OnClick="btProcPenale_Click" Text="Proc. Penale" ToolTip="Ricerca Procedimento Penale" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btProtGen" runat="server" OnClick="btProtGen_Click" Text="Rif. Prot. Gen." ToolTip="Ricerca Protocollo Generale" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btEvaseAg" runat="server" OnClick="btEvaseAg_Click" Text="Evase Ag." ToolTip="Ricerca Evase AG" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btNpratica" runat="server" OnClick="btNpratica_Click" Text="Nr. Pratica" ToolTip="Ricerca Pratica" CssClass="btn btn-primary mx-2" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btGiudice" runat="server" OnClick="btGiudice_Click" Text="Giudice" ToolTip="Ricerca Giudice" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btProvenienza" runat="server" OnClick="btProvenienza_Click" Text="Provenienza" ToolTip="Ricerca Per ProvenienzaG" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btNominativo" runat="server" OnClick="btNominativo_Click" Text="Nominativo" ToolTip="Ricerca Nominativo" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btDataArrivo" runat="server" OnClick="btDataArrivo_Click" Text="Data Inserimento" ToolTip="Ricerca Data Inserimento" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btOperatori" runat="server" OnClick="btOperatori_Click" Text="operatori" ToolTip="Ricerca Operatori" CssClass="btn btn-primary mx-2" />

                            </td>
                            <td>
                                <asp:Button ID="btIndirizzo" runat="server" OnClick="btIndirizzo_Click" Text="Indirizzo" ToolTip="Ricerca Per Indirizzo" CssClass="btn btn-primary mx-2" />

                            </td>
                        </tr>
                    </table>

                </div>

            </asp:Panel>
        </div>

        <%-- < SEZIONE PANNELLI RICERCA --%>
        <div id="DivRicerca" runat="server" class="row d-flex justify-content-center align-items-center vh-100" style="height: 300px; margin-left: 400px!important">
            <!-- Righe di input  -->
            <div class="col-md-4 ">
                <%-- DIV RICERCA PROTOCOLLO --%>
                <div id="DivProtocollo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="lblm" runat="server" Text="Nr Protocollo" CssClass="form-label d-block mb-2"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNProtocollo" ErrorMessage="Inserire numero pratica" ForeColor="Red" ValidationGroup="bt" autofocus="">

                    </asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtNProtocollo" runat="server" CssClass="form-control" placeholder="Nr Protocollo" autofocus="" />


                    <asp:Label ID="Label1" runat="server" Text="Anno" CssClass="form-label d-block mb-2"></asp:Label>
                    <%--<asp:RequiredFieldValidator ID="rqanno" runat="server" ControlToValidate="txtAnnoRicerca" ErrorMessage="Inserire l'anno per la ricerca" ForeColor="Red" ValidationGroup="bt">

                    </asp:RequiredFieldValidator>--%>
                    <asp:TextBox ID="txtAnnoRicerca" runat="server" CssClass="form-control" placeholder="Anno" />
                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA PROocedimento penale --%>
                <div id="DivProcPenale" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label2" runat="server" Text="Nr Procediemnto Penale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProcPenale" runat="server" CssClass="form-control" placeholder="Nr Procedimento Penale" autofocus="" />



                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA evasa ag --%>
                <div id="DivEvasaAg" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label3" runat="server" Text="Data Inizio Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataDa" runat="server" CssClass="form-control data-auto" placeholder="Data Inizio" autofocus="" />

                    <asp:Label ID="Label4" runat="server" Text="Data Fine Ricerca" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDataA" runat="server" CssClass="form-control data-auto" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA rif protocollo generale --%>
                <div id="DivProtGen" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label5" runat="server" Text="Rif. Protocollo Generale" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtProtGen" runat="server" CssClass="form-control" placeholder="Rif. Prot. Gen." autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA pratica --%>
                <div id="DivPratica" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label6" runat="server" Text="Nr. Pratica" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtPratica" runat="server" CssClass="form-control" placeholder="Nr. Pratica" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA giudice --%>
                <div id="DivGiudice" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label7" runat="server" Text="Giudice" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicGiudice" runat="server" CssClass="form-control" placeholder="Giudice" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA provenienza --%>
                <div id="DivProvenienza" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label8" runat="server" Text="Provenienza" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicProvenienza" runat="server" CssClass="form-control" placeholder="Provenienza" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA nominativo --%>
                <div id="DivNominativo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label9" runat="server" Text="Nominativo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicNominativo" runat="server" CssClass="form-control" placeholder="Nominativo" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA operatori --%>
                <div id="DivOperatori" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label10" runat="server" Text="Operatori" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicOperatori" runat="server" CssClass="form-control" placeholder="Operatori" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA Indirizzo --%>
                <div id="DivIndirizzo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label11" runat="server" Text="Indirizzo" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtRicIndirizzo" runat="server" CssClass="form-control" placeholder="Indirizzo" autofocus="" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
                <%-- DIV RICERCA data carico --%>
                <div id="DivDataArrivo" runat="server" visible="false" class="form-group text-center" style="text-align: left !important">

                    <asp:Label ID="Label12" runat="server" Text="Data Inserimento Da" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoDa" runat="server" CssClass="form-control data-auto" placeholder="Data Inizio" />

                    <asp:Label ID="Label13" runat="server" Text="Data Inserimento A" CssClass="form-label d-block mb-2"></asp:Label>

                    <asp:TextBox ID="txtDatArrivoA" runat="server" CssClass="form-control data-auto" placeholder="Data Fine" />

                    <div style="margin-left: 1px!important; margin-top: 30px!important">
                        <asp:Button Text="Ricerca" runat="server" OnClick="Ricerca_Click" ToolTip="Ricerca" CssClass="btn btn-primary mt-3" ValidationGroup="bt" />
                    </div>
                </div>
            </div>
        </div>
        <%--  --%>
        <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
        <div class="container" id="DivDettagli" runat="server">

            <div class="tab-content">
                <p style="font-weight: bold; font-size: medium">Dati Generali</p>
                <div class="row custom-border">
                    <div class="col-md-3 ">
                        <div class="form-group mb-3">
                            <label for="txtProt">Nr Carico</label>
                            <asp:TextBox ID="txtProt" runat="server" CssClass="form-control1" ForeColor="Red" Enabled="false" Font-Bold="true" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group mb-3">
                            <label for="DdlSigla">Sigla</label>
                            <asp:DropDownList ID="DdlSigla" runat="server" CssClass="form-control" onchange="gestisciVisibilita();">
                                <asp:ListItem Text="ED" Value="ED"></asp:ListItem>
                                <asp:ListItem Text="TP" Value="TP"></asp:ListItem>
                                <asp:ListItem Text="AG" Value="AG"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRifProtGen" ErrorMessage="* Inserire Riferimento Prot. Gen." ValidationGroup="bt" ForeColor="Red" Display="Dynamic" />
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

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNumProtRicStessoCarico" ErrorMessage="* Inserire numero prot." ValidationGroup="bt" ForeColor="Red" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtNumProtRicStessoCarico" ErrorMessage="* Solo numeri" ForeColor="Red" ValidationExpression="\d{1,3}" Display="Dynamic" />
                        </div>
                    </div>


                </div>


                <p style="font-weight: bold; font-size: medium">Dati Relativi Alla Pratica</p>
                <div class="row custom-border">
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtIndirizzo">Indirizzo</label>
                            <asp:TextBox ID="txtIndirizzo" runat="server" AutoPostBack="false" onkeyup="filterDropdownIndirizzo()" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListIndirizzo" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                <asp:HiddenField ID="HfIndirizzo" runat="server" />
                            </div>
                            <asp:DropDownList ID="DdlIndirizzo" runat="server" CssClass="form-control" Style="display: none" />

                        </div>

                        <div class="form-group mb-3">
                            <label for="txPratica">Pratica</label>
                            <asp:TextBox ID="txPratica" runat="server" CssClass="form-control mb-3" MaxLength="50" />

                        </div>
                        <div class="form-group mb-3">
                            <label for="txtCartellina">Cartellina Patrimonio</label>
                            <asp:TextBox ID="txtCartellina" runat="server" CssClass="form-control" />
                        </div>

                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtQuartiere">Quartiere</label>
                            <asp:TextBox ID="txtQuartiere" runat="server" AutoPostBack="false" onkeyup="filterDropdownQuartiere()" Style="width: 300px;" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                            <div id="suggestionsListQuartiere" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                <asp:HiddenField ID="HfQuartiere" runat="server" />
                            </div>
                            <asp:DropDownList ID="DdlQuartiere" runat="server" CssClass="form-control" Style="display: none" />

                        </div>
                        <div class="form-group mb-3">
                            <label for="DdlMacroArea">Area Competenza</label>
                            <%--<asp:TextBox ID="txtAreaCompetenza" runat="server" CssClass="form-control mb-3" />--%>
                            <asp:DropDownList ID="DdlMacroArea" runat="server" CssClass="form-control">
                                <asp:ListItem Text="&nbsp;" Value=""></asp:ListItem>
                                <asp:ListItem Text="ARCHIVIO"> </asp:ListItem>
                                <asp:ListItem Text="ATTI"> </asp:ListItem>
                                <asp:ListItem Text="CDR"> </asp:ListItem>
                                <asp:ListItem Text="FURERIA"> </asp:ListItem>
                                <asp:ListItem Text="MA1"> </asp:ListItem>
                                <asp:ListItem Text="MA2"> </asp:ListItem>
                                <asp:ListItem Text="MA3"> </asp:ListItem>
                                <asp:ListItem Text="NOTIFICATORI"> </asp:ListItem>
                                <asp:ListItem Text="PG"> </asp:ListItem>
                                <asp:ListItem Text="SOPRALLUOGO"> </asp:ListItem>
                                <asp:ListItem Text="UFFICIO TRASMISSIONI"> </asp:ListItem>
                                <asp:ListItem Text="URP"> </asp:ListItem>
                            </asp:DropDownList>

                            <%--<div id="bt1" runat="server" class="col-md-3" style="margin-top: 25px!important; margin-left: -15px!important">--%>

                            <%--<asp:Button ID="Add" Text="+" runat="server" ToolTip="Aggiungi accertatore 2" CssClass="btn btn-primary mt-3" OnClick="Add_Click" />--%>
                        </div>
                        <div class="form-group mb-3" id="divbu" runat="server" style="display: none;">
                            <div class="form-group mb-3">
                                <label for="txtBU">BU</label>
                                <asp:TextBox ID="txtBU" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                    </div>
                    <div class="col-md-4">
                        <div class="form-group mb-3">
                            <label for="txtNominativo">Nominativo</label>
                            <asp:TextBox ID="txtNominativo" runat="server" CssClass="form-control mb-3" />


                        </div>
                        <div class="form-group mb-3">
                            <label for="txtDataCarico">Data Carico</label>
                            <asp:TextBox ID="txtDataCarico" runat="server" CssClass="form-control mb-3 data-auto" ClientIDMode="Static" />
                        </div>
                        <div class="form-group mb-3" id="divcd" runat="server" style="display: none;">
                            <label for="txtCodEdificio">Codice Edificio</label>
                            <asp:TextBox ID="txtCodEdificio" runat="server" CssClass="form-control" />
                        </div>
                    </div>


                </div>

                <p style="font-weight: bold; font-size: medium">Esito Accertamento</p>
                <div class="row custom-border p-3">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group mb-4">
                                <label for="TxtDataEsito" class="fw-bold">Data Esito</label>
                                <asp:TextBox ID="TxtDataEsito" runat="server" CssClass="form-control data-auto" ClientIDMode="Static" DataFormatString="{0:dd/MM/yyyy}" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-4">
                                <label for="txtEsito" class="fw-bold">Esito</label>
                                <asp:TextBox ID="txtEsito" runat="server" onkeyup="filterDropdownEsito()" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                <div id="suggestionsListEsito" runat="server" class="dropdown-content" style="display: none;">
                                    <asp:HiddenField ID="HfEsito" runat="server" />
                                </div>
                                <asp:DropDownList ID="DdlEsito" runat="server" CssClass="form-control" Style="display: none" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-4">
                                <label for="TxtDataEsito" class="fw-bold">Protocollo Uscita</label>
                                <asp:TextBox ID="txtProtUscita" runat="server" CssClass="form-control" ClientIDMode="Static" />
                            </div>

                        </div>
                    </div>
                    <div class="row ">

                        <div class="col-md-4">
                            <div class="form-group mb-4">
                                <label class="fw-bold">Operatore</label>
                                <asp:DropDownList ID="DdlOperatori" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="col-md-1 text-center" style="margin-top: 30px">
                            <div class="form-group mb-4">
                                <asp:Button ID="btAggiungi" runat="server" Text="➕" CssClass="btn btn-success w-100" OnClick="btAggiungi_Click" ToolTip="Aggiungi" />
                            </div>
                        </div>
                        <div class="col-md-1 text-center" style="margin-top: 30px">
                            <div class="form-group mb-4">
                                <asp:Button ID="btElimina" runat="server" Text="🗑️" CssClass="btn btn-danger w-100" OnClick="btElimina_Click" ToolTip="Elimina" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group mb-4">
                                <label class="fw-bold text-muted small">Operatori Selezionati</label>
                                <asp:ListBox ID="ListOperatori" runat="server" CssClass="form-control" Rows="2"></asp:ListBox>
                            </div>
                        </div>


                    </div>
                </div>


                <div id="divAg" runat="server" style="display: none;">

                    <p style="font-weight: bold; font-size: medium">Dati AG</p>
                    <div class="row custom-border">
                        <div class="col-md-4">
                            <div class="form-group mb-3">
                                <label for="txtGiudice">Giudice</label>
                                <asp:TextBox ID="txtGiudice" runat="server" CssClass="form-control mb-3" />
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDataDelega">Data Delega</label>
                                <asp:TextBox ID="txtDataDelega" runat="server" CssClass="form-control data-auto"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group mb-3">
                                <label for="TxtTipoProvvAg">Tipo Provvedimento AG</label>
                                <asp:TextBox ID="TxtTipoProvvAg" runat="server" CssClass="form-control mb-3" AutoPostBack="false" onkeyup="filterDropdownTipoProv()" Style="width: 300px;" ClientIDMode="Static" Visible="false"></asp:TextBox>

                                <div id="suggestionsListTipoProv" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
                                    <asp:HiddenField ID="HfTipoProv" runat="server" />

                                </div>
                                <%--                                <asp:Button ID="btSalvaTipoProvv" runat="server" CssClass="btn btn-primary" Text="Inserisci il nuovo valore" OnClick="btSalvaTipoProvv_Click" Visible="false" />--%>
                                <asp:DropDownList ID="DdlTipoProvvAg" runat="server" CssClass="form-control" OnSelectedIndexChanged="DdlTipoProvvAg_SelectedIndexChanged" AutoPostBack="true" />

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
                                <asp:TextBox ID="txtProdPenNr" runat="server" CssClass="form-control mb-3" />

                            </div>
                        </div>
                        <%--                        <div class="col-md-3" style="margin-top: -25px">
                            <div class="form-check">
                                <asp:CheckBox ID="CkEvasa" runat="server" CssClass="form-check-input" Enabled="false" />
                                <label class="form-check-label ms-2" for="CkEvasa">Evasa</label>
                            </div>
                        </div>--%>
                    </div>
                </div>


                <div class="row">
                    <div class="d-flex justify-content-center align-items-center w-100 my-4">
                        <table style="margin: 0 auto; border-collapse: separate; border-spacing: 6px;">
                            <tr>
                                <td>
                                    <asp:Button ID="btSalva" Text="Salva" runat="server" OnClick="Salva_Click" ToolTip="salva" CssClass="btn btn-primary btn-custom-size" /></td>
                                <td>
                                    <asp:Button ID="btCercaQuartiere" Text="Cerca Quartiere" runat="server" OnClick="apripopup_Click" ToolTip="Ricerca" CssClass="btn btn-primary btn-custom-size" /></td>
                                <td>
                                    <asp:Button Text="Decretazione" runat="server" OnClick="Decretazione_Click" ToolTip="Decrertazione" CssClass="btn btn-primary btn-custom-size" /></td>
                                <td>
                                    <asp:Button Text="Nuova Modifica" runat="server" OnClick="NuovaModifica_Click" ToolTip="Nuova Modifica" CssClass="btn btn-primary btn-custom-size" /></td>
                            </tr>
                        </table>
                    </div>

                </div>

            </div>

        </div>
    </div>
    <!-- Modale Bootstrap quartiere -->


    <div class="modal fade" id="ModalQuartiere" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalLabel1">Ricerca quartiere</h5>

                </div>
                <div class="modal-body">
                    <!-- Campi di input per la ricerca -->
                    <div class="form-group">
                        <label for="txtIndirizzoQuartiere">Indirizzo:</label>
                        <asp:TextBox ID="txtIndirizzoQuartiere" runat="server" CssClass="form-control" placeholder="Campo obbligatorio" />

                    </div>
                    <%--<div class="form-group">
                                <label for="txtSpecie">Toponimo:</label>
                                <asp:TextBox ID="txtSpecie" runat="server" CssClass="form-control" />
                            </div>--%>
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
                                        <asp:Button ID="Button1" runat="server" Text="Seleziona" CommandName="Select" CommandArgument='<%# Eval("Quartiere") %>' CssClass="btn btn-success btn-sm" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="RicercaQuartiere_Click" />
                    <asp:Button ID="btnchiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
                </div>
            </div>
        </div>
    </div>

    <%-- Modale ricerca fascicolo --%>
    <div class="modal fade" id="ModalRicerca" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" style="width: 100%">
            <%--<div class="container" id="DivDettagli" runat="server">--%>
            <div class="modal-content">
                <div class="modal-header bg-dark text-white">
                    <h5 class="modal-title" id="modalLabel2">Ricerca Fascicolo</h5>

                </div>
                <div class="modal-body">
                    <div id="DivGrid" runat="server" class="section-box" visible="false">
                        <div class="table-responsive">
                            <!-- GridView nel popup -->
                            <asp:GridView ID="gvPopupD" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                OnRowDataBound="gvPopupD_RowDataBound" OnRowCommand="gvPopupD_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvPopupD_PageIndexChanging" RowStyle-CssClass="GridViewRow"
                                AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                    <asp:BoundField DataField="Nr_Protocollo" HeaderText="Nr. Carico" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                    <asp:BoundField DataField="Anno" HeaderText="Anno" ItemStyle-Width="20px" />
                                    <asp:BoundField DataField="Sigla" HeaderText="Sigla" ItemStyle-Width="20px" Visible="false" />
                                    <asp:BoundField DataField="nr_Pratica" HeaderText="N. Pratica" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />

                                    <%--                                <asp:BoundField DataField="Nominativo" HeaderText="Nominativo" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>--%>

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
                                    <%-- <asp:BoundField DataField="Indirizzo" HeaderText="Indirizzo" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                    <ItemStyle CssClass="uppercase-text" />
                                </asp:BoundField>--%>

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

                                    <asp:BoundField DataField="ProcedimentoPen" HeaderText="Proc. Penale" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Tipologia_atto" HeaderText="Tipologia Atto" ItemStyle-Wrap="true" ItemStyle-Width="50px">
                                        <ItemStyle CssClass="uppercase-text" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UlterioreTipoAtto" HeaderText="Ulteriore Tipo Atto" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Operatori" ItemStyle-CssClass="uppercase-text" ItemStyle-Wrap="true" ItemStyle-Width="80px">
                                        <HeaderTemplate>
                                            Operatori
                                          <br />
                                            <asp:TextBox ID="txtFilterOperatori" runat="server" OnTextChanged="txtFilterOperatori_TextChanged" AutoPostBack="True"></asp:TextBox>
                                            Filtro
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("operatori") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </asp:BoundField>--%>
                                    <asp:BoundField DataField="Rif_Prot_Gen" HeaderText="Prot. Generale" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Evasa" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Eval("evasa").ToString() == "True" ? "Si" : "No" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Matricola" HeaderText="Matricola" Visible="false" />
                                    <asp:BoundField DataField="NomeOperatore" HeaderText="Operatore" Visible="true" ItemStyle-Width="20px" />
                                    <asp:BoundField DataField="DataInserimento" HeaderText="Data Inserimento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="20px" Visible="false" />
                                    <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelect" runat="server" Text="Seleziona"
                                                CommandName="Select"
                                                CommandArgument='<%# Eval("Nr_Protocollo") + "|" + Eval("Matricola") + "|" + Eval("DataInserimento") + "|" + Eval("Sigla") + "|" + Eval("ID")  %>'
                                                CssClass="btn btn-success btn-sm" />
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
                                <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                            </asp:GridView>

                        </div>
                    </div>
                </div>
                <%-- campi nascosti --%>
                <asp:HiddenField ID="Holdmat" runat="server" />
                <asp:HiddenField ID="HolDate" runat="server" />
                <asp:HiddenField ID="Hid" runat="server" />
                <asp:HiddenField ID="HfFiltroNote" runat="server" />
                <asp:HiddenField ID="HfFiltroIndirizzo" runat="server" />
                <asp:HiddenField ID="HfFiltroNominativo" runat="server" />
                <asp:HiddenField ID="HfFiltroOperatori" runat="server" />
                <asp:HiddenField ID="Hfdecretazione" runat="server" />

                <%-- campi nascosti --%>
                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="btChiudi" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopup_Click" />
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
                                <asp:TextBox ID="txtPraticaDecr" runat="server" Enabled="false" CssClass="form-control mb-3" Width="120px"></asp:TextBox>
                                <asp:CheckBox ID="ckUnire" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="ckUnire">Unire</label>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDecretante">Decretante</label>
                                <asp:TextBox ID="txtDecretante" runat="server" CssClass="form-control mb-3"></asp:TextBox>
                            </div>
                            <div class="form-group mb-3">
                                <label for="txtDecretato">Decretato</label>
                                <%--<asp:TextBox ID="txtDecretato" runat="server" CssClass="form-control mb-3"></asp:TextBox>--%>


                                <input type="text" id="txtSearchOperatore" runat="server" class="form-control"
                                    placeholder="cerca..."
                                    onkeyup="filterAndHighlightOp(event)"
                                    onblur="validaOperatoreOnBlur()"
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
                                <asp:TextBox ID="txtDataDecretazione" runat="server" CssClass="form-control mb-3 data-auto"></asp:TextBox>

                            </div>
                            <div class="form-group mb-3">
                                <label for="txtNotaDecretazione">Nota</label>
                                <asp:TextBox ID="txtNotaDecretazione" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" MaxLength="255" Rows="12" Style="width: 100%; max-width: 600px;"></asp:TextBox>
                            </div>

                            <div class="form-group mb-3">
                                <asp:Button ID="btAggiungiDecretazione" runat="server" CssClass="btn btn-primary mt-3" Text="Aggiungi" OnClick="btAggiungiDecretazione_Click" ValidationGroup="btDecretazione" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group mb-3">
                                <div class="form-group">
                                    <!-- GridView nel popup -->
                                    <asp:GridView ID="GVDecretazione" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered gridview-autofit" DataKeyNames="decr_id"
                                        OnRowDataBound="GVDecretazione_RowDataBound"
                                        OnRowCommand="GVDecretazione_RowCommand"
                                        AllowPaging="true" PageSize="5"
                                        OnRowEditing="GVDecretazione_RowEditing"
                                        OnRowCancelingEdit="GVDecretazione_RowCancelingEdit"
                                        OnRowUpdating="GVDecretazione_RowUpdating"
                                        OnPageIndexChanging="GVDecretazione_PageIndexChanging"
                                        AlternatingRowStyle-CssClass="GridViewAlternatingRow">
                                        <Columns>
                                            <asp:BoundField DataField="decr_id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="decr_idPratica" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="decr_pratica" HeaderText="Pratica" Visible="false" />

                                            <%--                                            <asp:BoundField DataField="decr_decretante" HeaderText="Decretante">

                                                <HeaderStyle CssClass="colonna-descrizione" />
                                                <ItemStyle CssClass="uppercase-text" />
                                            </asp:BoundField>--%>



                                            <asp:TemplateField HeaderText="Decretante" ItemStyle-Width="150px" ItemStyle-CssClass="uppercase-text" HeaderStyle-CssClass="colonna-descrizione">
                                                <ItemTemplate>
                                                    <%# Eval("decr_decretante") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDecretanteMod" runat="server" Text='<%# Bind("decr_decretante") %>' CssClass="form-control input-sm"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>




                                            <%--<asp:BoundField DataField="decr_data" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />--%>


                                            <%--  CAMPO EDITABILE: Decretato --%>
                                            <asp:TemplateField HeaderText="Decretato" ItemStyle-Width="150px" ItemStyle-CssClass="uppercase-text" HeaderStyle-CssClass="colonna-descrizione">
                                                <ItemTemplate>
                                                    <%# Eval("decr_decretato") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDecretatoMod" runat="server" Text='<%# Bind("decr_decretato") %>' CssClass="form-control input-sm"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <%--  CAMPO EDITABILE: data --%>
                                            <asp:TemplateField HeaderText="Data">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblData" runat="server"
                                                        Text='<%# Eval("decr_data", "{0:dd/MM/yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDataMod" runat="server" Text='<%# Bind("decr_data", "{0:dd/MM/yyyy}") %>' CssClass="form-control input-sm" Width="100px"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>




                                            <%--   <asp:BoundField DataField="decr_decretato" HeaderText="Decretato">
                                                <HeaderStyle CssClass="colonna-descrizione" />
                                                <ItemStyle CssClass="uppercase-text" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="decr_nota" HeaderText="Nota">
                                                <HeaderStyle CssClass="colonna-descrizione" />
                                                <ItemStyle CssClass="uppercase-text colonna-descrizione" />
                                            </asp:BoundField>--%>

                                            <%--  CAMPO EDITABILE: Nota --%>
                                            <asp:TemplateField HeaderText="Nota" ItemStyle-Width="150px" ItemStyle-CssClass="uppercase-text" HeaderStyle-CssClass="colonna-descrizione">
                                                <ItemTemplate>
                                                    <%# Eval("decr_nota") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtNotaMod" runat="server" Text='<%# Bind("decr_nota") %>' CssClass="form-control input-sm"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <%--<asp:BoundField DataField="decr_dataChiusura" HeaderText="Data Chiusura" DataFormatString="{0:dd/MM/yyyy}" />--%>

                                            <asp:TemplateField HeaderText="Data Chiusura">
                                                <ItemTemplate>
                                                    <%# Routine.FormatMyDate(Eval("decr_dataChiusura")) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>





                                            <%-- COLONNA COMANDI (MODIFICA / SALVA / ANNULLA) --%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <!-- Il pulsante per entrare in modifica deve avere CommandName="Edit" -->
                                                    <asp:Button ID="btnModifica" runat="server" Text="Mod." CommandName="Edit" CssClass="btn btn-warning btn-sm" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <!-- Pulsanti visibili quando sei in modifica -->
                                                    <asp:Button ID="btnSalva" runat="server" Text="Salva" CommandName="Update" CssClass="btn btn-primary btn-sm" ValidationGroup="EditVG" />
                                                    <asp:Button ID="btnAnnulla" runat="server" Text="X" CommandName="Cancel" CssClass="btn btn-secondary btn-sm" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>






                                            <%--                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="Button1" runat="server" Text="Modifica" CommandName="Select" CommandArgument='<%# Eval("decr_id") + "|" + Eval("decr_decretato") + "|" + Eval("decr_data") + "|" + Eval("decr_nota")%>' CssClass="btn btn-success btn-sm" />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button ID="Button4" runat="server" Text="Salva" CommandName="Save" CommandArgument='<%# Eval("decr_id") + "|" + Eval("decr_decretato") + "|" + Eval("decr_data") + "|" + Eval("decr_nota")%>' CssClass="btn btn-success btn-sm" />

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
                    </div>

                </div>

                <div class="modal-footer">
                    <!-- Bottone per avviare la ricerca -->
                    <%--<asp:Button ID="btRicScheda" runat="server" CssClass="btn btn-primary" Text="Cerca" OnClick="btRicScheda_Click" />--%>
                    <asp:Button ID="Button3" runat="server" class="btn btn-secondary" Text="Chiudi" OnClick="chiudipopupDecretazione_Click" />
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
                        <asp:TextBox ID="txtdataEvasaPopup" runat="server" CssClass="form-control data-auto" autofocus=""></asp:TextBox>
                    </div>
                </div>

                <div class="modal-footer">
                    <!-- Bottone per avviare chiousura decretazione -->
                    <asp:Button ID="ModalChiudiDecretazione" runat="server" class="btn btn-secondary" Text="Salva" OnClick="ModalChiudiDecretazione_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Popup Modale Richiesta salva -->
    <div class="modal fade" id="ModalRichiestaSalva" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true" role="dialog">
        <div class="modal-dialog" style="width: 50%"
            role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeaderColor" runat="server">
                    <h4 class="modal-title" id="ModalLabel">
                        <asp:Label ID="lblModalTitolo" runat="server" Text="Avviso"></asp:Label>
                    </h4>

                </div>
                <div class="modal-body">
                    <div id="Div1" runat="server" class="row" style="margin-left: 30px!important">
                        <div class="form-group mb-3">

                            <p id="TxtMessage" runat="server" style="color: red"></p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <!-- Bottone per avviare chiousura decretazione -->
                    <asp:Button ID="BtConferma" runat="server" class="btn btn-secondary" Text="Si" OnClick="BtConferma_Click" />
                    <asp:Button ID="BtNo" runat="server" class="btn btn-secondary" Text="No" OnClick="BtNo_Click" />
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        // Funzione per gestire la visibilità
        function gestisciVisibilita() {
            var divag = document.getElementById('<%= divAg.ClientID %>');
                var divbu = document.getElementById('<%= divbu.ClientID %>');
                var divcd = document.getElementById('<%= divcd.ClientID %>');
           <%-- var txtgiudice = document.getElementById('<%= txtGiudice.ClientID %>');
            var txtprocpen = document.getElementById('<%= txtProdPenNr.ClientID %>');
            var ddlProvAg = document.getElementById('<%= DdlTipoProvvAg.ClientID %>');--%>
                // Recupera gli altri elementi, verificando sempre
                var ddl = document.getElementById('<%= DdlSigla.ClientID %>');

            // mostrare/nascondere
            if (ddl.value === 'AG') {
                divag.style.display = 'block';
            } else {
                divag.style.display = 'none';
            }
            if (ddl.value === 'TP') {
                //txtgiudice.value = '';
                //txtprocpen.value = '';
                //ddlProvAg.selectedIndex = 0;
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
        ///////////////
        var allOptions = [];

        // Funzione per caricare i dati (chiamata solo quando serve)
        function caricaOpzioni() {
            var ddl = document.getElementById('<%= DdlTipoAtto.ClientID %>');

            if (!ddl) {
                console.error("Errore: DropDownList 'DdlTipoAtto' non trovata!");
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
        document.addEventListener("DOMContentLoaded", caricaOpzioni);
        // Funzione Filtro
        function filterAndHighlight(e) {
            var input = document.getElementById('<%= txtSearchAtto.ClientID %>');

            var listDiv = document.getElementById("suggestionsListTipoAtto");
            //var filter = input.value.toUpperCase();
            var filter = (input.value || "").toUpperCase();

            // Se l'array è vuoto (es. UpdatePanel ha resettato), ricaricalo
            if (allOptions.length === 0) {
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
        // Funzione Filtro


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

            var optionsOp = ddl.options;
            allOptionsOp = []; // Resetta

            for (var i = 0; i < optionsOp.length; i++) {
                // Carica tutto tranne i valori vuoti
                if (optionsOp[i].value !== "" && optionsOp[i].value !== "0") {
                    allOptionsOp.push({ text: optionsOp[i].text, value: optionsOp[i].value });
                }
            }
            console.log("Opzioni caricate in memoria: " + allOptionsOp.length);
        }

        // Carica i dati appena la pagina è pronta
        document.addEventListener("DOMContentLoaded", caricaOpzioniOperatore);

        function filterAndHighlightOp(e) {
            var input = document.getElementById('<%= txtSearchOperatore.ClientID %>');
                var listDiv = document.getElementById("suggestionsListoperatore");
                var filter = (input.value || "").toUpperCase();

                // Se l'array è vuoto (es. UpdatePanel ha resettato), ricaricalo
                if (typeof allOptionsOp !== "undefined" && allOptionsOp.length === 0) {
                    caricaOpzioniOperatore();
                }

                // Tasto INVIO (13)
                if (e.keyCode === 13) {
                    var activeItem = listDiv.querySelector(".active");
                    if (activeItem) {
                        activeItem.click(); // Simula il click
                        e.preventDefault(); // Ferma il postback del form
                    }
                    return;
                }

                listDiv.innerHTML = "";

                // Se input vuoto, svuota anche la DDL nascosta e nascondi la lista
                if (filter.length === 0) {
                    var ddl = document.getElementById('<%= ddlOperatore.ClientID %>');
                if (ddl) ddl.value = "";
                listDiv.style.display = "none";
                return;
            }

            var foundCount = 0;

            for (var i = 0; i < allOptionsOp.length; i++) {
                var item = allOptionsOp[i];

                // LOGICA DI FILTRO (Contiene il testo?)
                if (item.text.toUpperCase().indexOf(filter) > -1) {

                    var div = document.createElement("div");
                    div.className = "suggestion-item";
                    div.innerText = item.text;
                    div.setAttribute("data-val", item.value);

                    // Evidenzia il primo risultato per permettere l'invio rapido
                    if (foundCount === 0) {
                        div.classList.add("active");
                    }

                    // mouse down, evita che al primo click sulla selezione scatti l'alert
                    div.onmousedown = function () {
                        e.preventDefault();
                        selezionaOperatore(this.innerText, this.getAttribute("data-val"));
                    };

                    listDiv.appendChild(div);
                    foundCount++;
                }
            }

            if (foundCount > 0) {
                listDiv.style.display = "block";
            } else {
                listDiv.style.display = "none";
            }
        }
        var eSelezioneValida = false; //evita che al primo click sulla selezione scatti l'alert

        function selezionaOperatore(text, value) {
            console.log("Selezionato: " + text + " (ID: " + value + ")");

            var input = document.getElementById('<%= txtSearchOperatore.ClientID %>');
                var ddl = document.getElementById('<%= ddlOperatore.ClientID %>');
            var listDiv = document.getElementById("suggestionsListoperatore");

            input.value = text;
            if (ddl) ddl.value = value;
            eSelezioneValida = true;
            listDiv.style.display = "none";
        }

        // -------------------------------------------------------------
        // NUOVA FUNZIONE: Valida l'input quando l'utente esce dal campo (blur)
        // -------------------------------------------------------------
        function validaOperatoreOnBlur() {

            // Se l'utente ha appena cliccato su un elemento, ignoriamo il controllo di blur
            if (eSelezioneValida) {
                eSelezioneValida = false; // Resettiamo per le future modifiche
                return;
            }

            var input = document.getElementById('<%= txtSearchOperatore.ClientID %>');
                var ddl = document.getElementById('<%= ddlOperatore.ClientID %>');
            var testoDigitato = (input.value || "").trim().toUpperCase();

            if (testoDigitato === "") {
                if (ddl) ddl.value = "";
                return;
            }

            var trovato = false;

            // Cerca una corrispondenza ESATTA tra il testo scritto e le opzioni
            for (var i = 0; i < allOptionsOp.length; i++) {
                if (allOptionsOp[i].text.trim().toUpperCase() === testoDigitato) {
                    // Trovato! Assicuriamoci che la DDL e l'Input abbiano il valore corretto
                    selezionaOperatore(allOptionsOp[i].text, allOptionsOp[i].value);
                    trovato = true;
                    break;
                }
            }

            // Se l'utente ha scritto del testo non presente nell'elenco (o parziale senza cliccare)
            if (!trovato) {
                alert("Attenzione: Selezionare un operatore valido presente nell'elenco.");
                input.value = "";
                if (ddl) ddl.value = "";
            }

        }
        // -------------------------------------------------------------
        // Gestione della chiusura del popup se si clicca fuori dall'input/lista
        // -------------------------------------------------------------
        document.addEventListener('click', function (e) {
            var input = document.getElementById('<%= txtSearchOperatore.ClientID %>');
                var listDiv = document.getElementById("suggestionsListoperatore");

                if (input && listDiv && e.target !== input && !listDiv.contains(e.target)) {
                    listDiv.style.display = "none";
                }
            });

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
        //////////////
    </script>
</asp:Content>

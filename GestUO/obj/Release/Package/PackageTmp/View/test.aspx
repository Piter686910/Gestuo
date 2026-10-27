<%@ Page Title="Comandi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Uote.test" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <style>
        /* Reset e Font base */
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-size: 13px;
            color: #333;
        }

        /* Contenitore con scroll orizzontale se necessario */
        .container {
            width: 98%;
            margin: 10px auto;
            overflow-x: auto; /* Permette lo scroll se i giorni sono tanti */
            background: #fff;
            padding: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }

        /* --- TABELLA --- */
        table.tabella-turni {
            border-collapse: separate;
            border-spacing: 0;
            border: 1px solid #999;
            min-width: 100%;
        }

            /* Tutte le celle: bordi netti per effetto "Riquadro" */
            table.tabella-turni th,
            table.tabella-turni td {
                border-right: 1px solid #ccc;
                border-bottom: 1px solid #ccc;
                padding: 4px 2px; /* Padding ridotto per compattare */
                text-align: center;
                vertical-align: middle;
            }

                /* --- COLONNA DIPENDENTE (Sticky a sinistra) --- */
                /* Header Dipendente */
                table.tabella-turni th.col-dip-header {
                    position: sticky;
                    left: 0;
                    z-index: 20; /* Sopra tutto */
                    background-color: #34495e;
                    color: white;
                    width: 140px; /* Larghezza fissa ristretta */
                    min-width: 140px;
                    max-width: 140px;
                    border-right: 2px solid #555; /* Separatore marcato */
                }

                /* Celle Nome Dipendente */
                table.tabella-turni td.col-dipendente {
                    position: sticky;
                    left: 0;
                    z-index: 10; /* Sopra le celle normali */
                    background-color: #fff; /* Necessario per coprire le celle che scorrono sotto */
                    text-align: left !important;
                    padding-left: 8px !important;
                    /* Gestione testo lungo */
                    width: 140px;
                    min-width: 140px;
                    max-width: 140px;
                    white-space: nowrap; /* Tutto su una riga */
                    overflow: hidden; /* Nascondi eccesso */
                    text-overflow: ellipsis; /* Aggiungi "..." se tagliato */

                    font-weight: 600;
                    font-size: 12px; /* Font un po' più piccolo */
                    color: #2c3e50;
                    border-right: 2px solid #555; /* Separatore marcato */
                }

        .badge-q {
            font-size: 10px;
            background: #eee;
            border: 1px solid #ccc;
            color: #333;
            padding: 0 3px;
            border-radius: 3px;
            float: right;
            margin-right: 2px;
        }

        /* --- GIORNI (Intestazioni) --- */
        table.tabella-turni th.giorno-header {
            position: sticky;
            top: 0;
            z-index: 5;
            background-color: #34495e; /* Blu scuro */
            color: white;
            height: 40px;
            width: 28px; /* Larghezza fissa per ogni giorno: quadrato */
            min-width: 28px;
        }

        .weekend-h {
            background-color: #c0392b !important;
        }
        /* Rosso header weekend */

        /* --- CELLE TURNI --- */
        /* Colori Sfondo */
        .t-1 {
            background-color: #e3f2fd;
            color: #0277bd;
            font-weight: bold;
        }

        .t-2 {
            background-color: #fff8e1;
            color: #f57f17;
            font-weight: bold;
        }

        /* LA Q: Deve risaltare nel giorno specifico */
        .t-q {
            background-color: #ffcdd2;
            color: #b71c1c;
            font-weight: 900;
            border: 2px solid #d32f2f !important; /* Riquadro rosso marcato interno */
        }

        /* Colonna weekend verticale (grigio chiaro) */
        .weekend-col {
            background-color: #f2f2f2;
        }

        /* Riga Ufficio */
        .tr-ufficio td {
            background-color: #636e72;
            color: white;
            text-align: left;
            padding: 5px 10px;
            font-size: 12px;
            font-weight: bold;
            letter-spacing: 1px;
        }

        .t-rf {
            background-color: #c8e6c9; /* Verde pastello chiaro */
            color: #2e7d32; /* Verde foresta scuro */
            font-weight: bold;
            border: 2px solid #a5d6a7 !important; /* Bordo verde */
        }

        /* ... assicurati che le altre classi esistano ancora ... */
        .t-q {
            background-color: #ffcdd2;
            color: #b71c1c;
            font-weight: 900;
            border: 2px solid #d32f2f !important;
        }

        .t-1 {
            background-color: #e3f2fd;
            color: #0277bd;
            font-weight: bold;
        }

        .t-2 {
            background-color: #fff8e1;
            color: #f57f17;
            font-weight: bold;
        }
        /* Intestazione della colonna percentuale */
        th.col-stats-header {
            background-color: #444; /* Un grigio diverso per staccare */
            color: #fff;
            width: 50px;
            min-width: 50px;
            border-right: 2px solid #777;
            position: sticky;
            left: 140px; /* Deve essere uguale alla width della colonna dipendente */
            z-index: 20;
        }

        /* Cella del valore percentuale */
        td.col-stats {
            background-color: #f9f9f9;
            font-weight: bold;
            color: #333;
            font-size: 11px;
            border-right: 2px solid #777;
            position: sticky;
            left: 140px; /* Si incolla dopo il nome */
            z-index: 10;
        }
        /* Stile per gli input modificabili dentro la cella */
        .shift-input {
            width: 100%;
            height: 100%;
            border: none;
            background: transparent;
            text-align: center;
            font-weight: bold;
            font-family: inherit;
            font-size: inherit;
            color: inherit; /* Prende il colore dalla classe CSS del genitore (es rosso per Q) */
            text-transform: uppercase; /* Forza maiuscolo */
            cursor: pointer;
        }

            /* Evidenzia la cella quando la modifichi */
            .shift-input:focus {
                background-color: #ffffcc;
                outline: 2px solid #007bff;
            }

        /* La colonna percentuale che aggiorneremo via JS */
        .col-stats {
            font-weight: bold;
            transition: color 0.3s;
        }

        .btn-load {
            background-color: #17a2b8;
            color: white;
            border: none;
            padding: 5px 15px;
            cursor: pointer;
            margin-left: 10px;
        }

            .btn-load:hover {
                background-color: #138496;
            }

        .btn-excel {
            background-color: #217346;
            color: white;
            border: none;
            padding: 5px 15px;
            cursor: pointer;
            margin-left: 10px;
        }

            .btn-excel:hover {
                background-color: #1e6b41;
            }

        .btn-pdf {
            background-color: #dc3545;
            color: white;
            border: none;
            padding: 5px 15px;
            cursor: pointer;
            margin-left: 10px;
        }

            .btn-pdf:hover {
                background-color: #c82333;
            }

        .grid-view {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

            .grid-view th {
                background: #0056b3;
                color: white;
                padding: 10px;
                text-align: left;
            }

            .grid-view td {
                padding: 8px;
                border-bottom: 1px solid #ddd;
                font-size: 13px;
            }

        .btn-search {
            background: #28a745;
            color: white;
            border: none;
            padding: 6px 15px;
            cursor: pointer;
        }
    </style>
    <!-- 1. CSS di jQuery UI -->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

    <!-- 2. jQuery Base (una versione 3.x stabile e super compatibile) -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- 3. Il foglio di compatibilità (MIGRATE) - RISOLVE I VECCHI SELETTORI SILENZIOSAMENTE -->
    <script src="https://code.jquery.com/jquery-migrate-3.4.0.min.js"></script>

    <!-- 4. jQuery UI (versione 1.12.1, la più stabile per Web Forms) -->
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

    <div>
        <label for="txtInput">Digita un nome:</label>
        <asp:TextBox ID="txtInput" runat="server" CssClass="form-control" AutoPostBack="false" onkeyup="filterDropdown()" Style="width: 200px;" ClientIDMode="Static"></asp:TextBox>
        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" AutoPostBack="false" onkeyup="filterDropdown()" Style="width: 200px;" ClientIDMode="Static"></asp:TextBox>
        <div id="suggestionsList" runat="server" style="display: none; border: 1px solid #ccc; background-color: #f9f9f9; position: absolute; z-index: 1000; width: 200px;">
            <!-- Stili base per la lista suggerimenti -->
        </div>
        <div>
            <h3>Ricerca Stradario Comune di Napoli (Real-time API)</h3>
            <asp:TextBox ID="txtStrada" runat="server" Placeholder="Es. Toledo, Chiaia..." ClientIDMode="Static"></asp:TextBox>
            <asp:Button ID="btnCerca" runat="server" Text="Cerca Online" OnClick="btnCerca_Click" />

            <br />
            <br />
            <asp:Label ID="lblInfo" runat="server" ForeColor="Green"></asp:Label>
            <asp:Label ID="lblErrore" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <br />

            <asp:GridView ID="gvRisultati" runat="server" AutoGenerateColumns="False" CellPadding="6" AllowPaging="True" PageSize="15"
                OnPageIndexChanging="gvRisultati_PageIndexChanging"
                OnRowCommand="gvRisultati_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NomeCompleto" HeaderText="Indirizzo" />
                    <asp:BoundField DataField="Quartiere" HeaderText="Quartiere" />
                    <asp:BoundField DataField="Municipalita" HeaderText="Municipalità" />
                    <asp:TemplateField HeaderText="Azione">
                        <ItemTemplate>
                            <asp:Button ID="btnSeleziona" runat="server" Text="Seleziona"
                                CommandName="SelezionaStrada"
                                CommandArgument='<%# Eval("NomeCompleto") + "|" + Eval("Quartiere")  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </div>

        <div class="box-ricerca">
            <!-- 1. Campo finale in cui viene copiato l'immobile scelto -->
            <label style="font-weight: bold; color: #333;">Immobile Selezionato: </label>
            <%--<asp:Label ID="lbl1" runat="server" " BackColor="#e9ecef" Font-Bold="true" ReadOnly="true"></asp:Label>--%>
            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <hr />
            <br />

            <!-- 2. Pannello di ricerca con Bottone -->
            <label>Filtra per indirizzo o descrizione: </label>
            <asp:TextBox ID="txtFiltro" runat="server" Width="300px" Placeholder="Es. Vittorio Emanuele, Scuola..."></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Cerca Immobile" OnClick="btnCercaP_Click" Height="28px" />

            <asp:GridView ID="GridView1" runat="server"
                AutoGenerateColumns="False"
                CssClass="grid-style"
                GridLines="None"
                EmptyDataText="Nessun immobile trovato con i criteri di ricerca inseriti.">

                <Columns>
                    <%-- Dati Identificativi --%>
                    <asp:BoundField DataField="CodiceUnita" HeaderText="Cod. Unità" ItemStyle-Font-Bold="true" />
                    <asp:BoundField DataField="Edificio" HeaderText="Edificio" />
                    <asp:BoundField DataField="Denominazione" HeaderText="Denominazione" />

                    <%-- Localizzazione --%>
                    <asp:TemplateField HeaderText="Indirizzo">
                        <ItemTemplate>
                            <%# Eval("Indirizzo") %>, <%# Eval("Civico") %>
                            <small style="color: #666;">(<%# Eval("Quartiere") %>)</small>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Piano / Int.">
                        <ItemTemplate>
                            P: <%# Eval("Piano") %> / I: <%# Eval("Interno") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- Dati Catastali Raggruppati --%>
                    <asp:TemplateField HeaderText="Dati Catastali (S/F/P/Sub)">
                        <ItemTemplate>
                            <span class="badge-catasto">
                                <%# Eval("Sezione") %> / <%# Eval("Foglio") %> / <%# Eval("Particella") %> / <b><%# Eval("Sub") %></b>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <HeaderStyle CssClass="grid-header" />
                <RowStyle CssClass="grid-row" />
                <AlternatingRowStyle CssClass="grid-row-alt" />
            </asp:GridView>
        </div>

    </div>
    <script type="text/javascript">
        // Usiamo jQuery.noConflict() se nella pagina ci sono script di ASP.NET che sovrascrivono il simbolo $
        var $j = jQuery.noConflict();

        $j(document).ready(function () {
            $j('#<%= txtStrada.ClientID %>').autocomplete({
                source: function (request, response) {
                    $j.ajax({
                        url: 'test/CercaStradeAjax',
                        data: JSON.stringify({ termine: request.term }),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (xhr) {
                            console.log("Errore AJAX: ", xhr.responseText);
                        }
                    });
                },
                minLength: 3,
                delay: 300
            });
        });


    </script>

    <style>
        /* Forza il menu a tendina ad essere visibile e sopra tutto il resto */
        .ui-autocomplete {
            z-index: 99999 !important;
            background-color: white !important;
            border: 1px solid #ccc !important;
            max-height: 250px;
            overflow-y: auto;
        }
    </style>
</asp:Content>




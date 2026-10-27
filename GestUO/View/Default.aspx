<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.cs" Inherits="GestUO._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root {
            --ink: #337ab7;
            --muted: #65727d;
            --line: #d8e1e6;
            --surface: #ffffff;
            --accent: #337ab7;
            --accent-dark: #085957;
            --warm: #f5efe5;
        }

        /* Annulla i margini e i padding del container di default della Master Page */
        .body-content,
        .container,
        .container-fluid {
            width: 100% !important;
            max-width: 100% !important;
            padding: 0 !important;
            margin: 0 !important;
        }

        /* Layout che occupa tutto lo spazio verticale disponibile tra Navbar e Footer */
        .login-page-wrapper {
            display: grid;
            grid-template-columns: minmax(320px, 0.9fr) minmax(420px, 1.1fr);
            width: 100%;
            /* Sottrae l'altezza approssimativa di Navbar + Footer per evitare lo scroll */
            min-height: calc(100vh - 120px);
            background-color: var(--warm);
            margin: 0;
            border-radius: 0;
            box-shadow: none;
            overflow: hidden;
        }

        .intro-panel {
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            padding: 3rem;
            background-color: var(--accent);
            color: #ffffff;
        }

        .login-panel {
            display: grid;
            place-items: center;
            padding: 3rem;
            background-color: var(--surface);
        }

        .form-wrap {
            width: 100%;
            max-width: 380px;
        }

            .form-wrap h2 {
                margin: 0 0 10px 0;
                font-size: 2rem;
                color: var(--ink);
            }

        .form-control-custom {
            width: 100%;
            border: 1px solid var(--line);
            border-radius: 4px;
            padding: 12px;
            margin-bottom: 15px;
            font-size: 1rem;
        }

        .btn-submit {
            width: 100%;
            background-color: var(--accent);
            color: white;
            border: none;
            padding: 12px;
            font-weight: bold;
            border-radius: 4px;
            cursor: pointer;
            transition: background 0.2s;
        }

            .btn-submit:hover {
                background-color: var(--accent-dark);
            }

        .intro-copy {
            position: relative;
            padding: 20px 0;
        }

        .sfondo-personalizzato-default {
            width: 100%;
            height: 180px;
            background-image: url('<%= Page.ResolveClientUrl("~/FileComuni/ComuneNapoli.jpeg") %>');
            background-repeat: no-repeat;
            background-position: center;
            background-size: contain;
            opacity: 0.85;
            margin: 15px 0;
            margin-top: -50px;
        }

        @media (max-width: 768px) {
            .login-page-wrapper {
                grid-template-columns: 1fr;
                min-height: calc(100vh - 100px);
            }

            .intro-panel {
                display: none;
            }
        }
        /* 1. BLOCCO SCROLL GLOBALE */
        html, body {
            height: 100% !important;
            overflow: hidden !important;
            margin: 0 !important;
            padding: 0 !important;
        }

        .intro-panel .brand h3 {
            margin: 0;
            color: #ffffff !important; /* Forza il colore bianco per contrasto */
            font-size: 1.3rem;
            font-weight: 700;
            line-height: 1.2;
        }

        .intro-panel .brand i {
            color: #ffffff !important;
        }

        .centrato {
            margin-left: 150px;
        }

        .password-field {
            position: relative;
        }

            .password-field input {
                padding-right: 4rem;
            }

        .toggle-password {
            position: absolute;
            right: 0.7rem;
            top: 50%;
            border: 0;
            padding: 0.3rem;
            color: var(--accent-dark);
            background: transparent;
            font: 700 0.7rem 'Trebuchet MS', sans-serif;
            cursor: pointer;
            transform: translateY(-50%);
        }
    </style>
    <%--  --%>
    <script>
        function ShowErrorMessage(message) {
            $('#errorModal').modal('show');
        }
        document.addEventListener("DOMContentLoaded", function () {
            // Funzione generica per mostrare/nascondere
            function setupToggle(buttonId, fieldId, iconId) {
                const btn = document.getElementById(buttonId);
                const field = document.getElementById(fieldId);
                const icon = document.getElementById(iconId);

                if (btn && field) {
                    btn.addEventListener('click', function () {
                        const type = field.getAttribute('type') === 'password' ? 'text' : 'password';
                        field.setAttribute('type', type);
                        icon.classList.toggle('fa-eye');
                        icon.classList.toggle('fa-eye-slash');
                    });
                }
            }

            // Configura password attuale
            setupToggle('btnToggle', '<%= txtPassword.ClientID %>', 'eyeIcon');

            // Configura nuova password (se presente nel DOM)
            setupToggle('btnToggleNew', '<%= txtNewPassw.ClientID %>', 'eyeIconNew');
        });
    </script>
    <%-- LOGIN --%>
    <asp:Panel ID="pnlLogin" runat="server">
        <div class="login-page-wrapper">
            <div class="intro-panel">
                <div class="brand centrato" style="margin-top: 20px">
                    <i class="fa-solid fa-shield-halved fa-2x"></i>
                    <h3 style="font-style: italic; font-family: Arial; font-size: 40px;">POLIZIA MUNICIPALE DI NAPOLI</h3>
                    
                </div>
                <div class="intro-copy">
                    <div class="sfondo-personalizzato-default"></div>
                    <%--<p>Accesso riservato al personale autorizzato della Polizia Municipale di Napoli.</p>--%>
                </div>
                <div>
                    <small>&copy; Comune di Napoli, Accesso riservato al personale autorizzato della Polizia Municipale di Napoli.</small>
                </div>
            </div>

            <div class="login-panel">
                <div class="form-wrap">
                    <h2>Accedi</h2>
                    <p class="text-muted mb-4">Inserisci le tue credenziali per accedere al sistema</p>
                    
                    <!-- CAMPO UTENTE -->
                    <div class="form-group mb-3">
                        <label for="txtMatricola">Utente</label>
                        <div style="display: flex; align-items: center;">
                            <asp:TextBox ID="txtMatricola" runat="server" CssClass="form-control form-control-custom" placeholder="Inserisci matricola" TabIndex="1" Style="border-top-right-radius: 0; border-bottom-right-radius: 0;" />
                            <span class="input-group-text bg-white border-info-subtle" style="cursor: pointer; padding: 0.375rem 0.75rem; display: flex; align-items: center;">
                                <i class="fa-solid fa-user"></i>
                            </span>
                        </div>
                        <asp:HiddenField ID="Hmatricola" runat="server" />
                    </div>

                    <!-- CAMPO PASSWORD -->
                    <div class="form-group mb-3">
                        <label for="txtPassword" CssClass="form-label fw-bold text-info">Password</label>
                        <div style="display: flex; align-items: center;">
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••" TabIndex="2" Style="border-top-right-radius: 0; border-bottom-right-radius: 0;" />
                            <span class="input-group-text bg-white border-info-subtle" id="btnToggle" style="cursor: pointer; padding: 0.375rem 0.75rem; display: flex; align-items: center;">
                                <i class="fa-solid fa-eye text-secondary" id="eyeIcon"></i>
                            </span>
                        </div>
                    </div>
                    <div id="DivNewPassw" runat="server" class="form-group mb-3" visible="false">
                        <label Text="Nuova Password" CssClass="form-label fw-bold text-info"> </label>
                        <div style="display: flex; align-items: center;">
                            <asp:TextBox ID="txtNewPassw" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••" TabIndex="3" Style="border-top-right-radius: 0; border-bottom-right-radius: 0;">
                            </asp:TextBox>
                            <span class="input-group-text bg-white border-info-subtle" id="btnToggleNew" style="cursor: pointer; padding: 0.375rem 0.75rem; display: flex; align-items: center;">
                                <i class="fa-solid fa-eye text-secondary" id="eyeIconNew"></i>
                            </span>
                        </div>
                    </div>
                    <asp:Button ID="btnLogin" runat="server" Text="Accedi al Portale" CssClass="btn-submit" OnClick="btnAccedi_Click" />
                    <asp:Button ID="btsave" Text="Salva Nuova Password" runat="server" OnClick="SalvaPassw_Click" CssClass="btn btn-success btn-custom" Visible="false" />
                    <div class="form-options">

                        <div class="reset-link-container">
                            <asp:LinkButton ID="lkreset" OnClick="lkreset_Click" runat="server">Dimenticato la password? Reset</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

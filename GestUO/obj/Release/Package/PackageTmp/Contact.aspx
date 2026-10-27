<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="GestUO.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>PAGINA DI ERRORE</h2>
    <h3>Si è verificato il seguente errore</h3> <asp:Label ID="LabelMessaggioErrore" runat="server" ForeColor="red"></asp:Label>
    <asp:Panel ID="pnlButton" runat="server" CssClass="text-center" Visible="true" >
        <div class="d-flex justify-content-center mt-4" style="margin-top:150px">
            
        </div>
        <asp:Button ID="ButtonTornaIndietro" runat="server" Text="Torna alla Pagina Precedente" OnClick="ButtonTornaIndietro_Click" />
    </asp:Panel>
</asp:Content>

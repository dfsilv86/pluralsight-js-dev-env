<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="Walmart.Sgp.WebApp.Reports.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Visualização de Relatório</title>
    <script src="../Scripts/bower/jquery/dist/jquery.js"></script>
    <script src="jquery-ui.js"></script>

    <!-- 

        Tema gerado em:

        http://jqueryui.com/download/#!version=1.11.4&components=1100000000001000000001000000000000000&zThemeParams=5d00000100d905000000000000003d8888d844329a8dfe02723de3e5700bbb34ecf36cdef1e1654faa0015427bdb9eb45ebe89aaede0ec5f0d92417fcc6530a6bf3bafbd789674159f38b91b06cafc7229778bae048db723f0699797515e3a6b2c243dee0a57a6cf8da5c12059bad0a8c2eaf02645bbb4ea2b1b35042958f7b5bb3a47beb6acaa9727f1e69352a19ad6bfe5cd61e0839741b187df81151ae5e2a3dc42fcdb289b1487ad5c47baec4714de7b9e00eb202f5b00eec5501f95d6dfc81318b788ed7aa84e6946f3b442211f12c1e4f69322961f5026e80565ce2910ab12ef2f6783262049bab0067e9bbb15054eed885922c550013a069cdcb4fe14401f9804d3a7a409469ef8774a1c07c5c49ac42cbd8bdf277d0bd6cde0e5b90ca6861a12ea7fff6fdce9ff383f8bdfbcff62e51aa64ef6ec365211c0e3dfff5e723ea9b664433b1ca2f2586114f354d70c02e22ab7ce933405907a79727c1ca24d8719db2567a01c76c4803fe0128ac8e60338c55e105c16dc8910044efb80b87400ed7149e80868e7dcc4373645758f012de27c498ecab172128304547201987d51ca5a77697fd6ad7186a32ab1c9517d14f88eeeaf68b6abc269bed75cbf90916609ce4b70af19a4c12a3ee8365b797e6affe05948d8

    -->

    <link rel="stylesheet" href="jquery-ui.min.css" />
    <link rel="stylesheet" href="jquery-ui.structure.min.css" />
    <link rel="stylesheet" href="jquery-ui.theme.min.css" />
</head>
<body>
    <form runat="server">
        <!-- BEGIN HACK: http://www.rajbandi.net/fixing-ssrs-report-viewer-control-date-picker-in-google-chrome/ -->
        <asp:HiddenField ID="DatePickers" runat="server" />
        <!-- END HACK-->
        <asp:ScriptManager ID="scriptManager" AsyncPostBackTimeout="720000" runat="server" />
        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="630px" />

        <script src="ReportViewer.Datepicker.js" type="text/javascript" defer="defer"></script>
        <script src="datepicker-pt-BR.js" type="text/javascript" defer="defer"></script>
        <script src="jquery.mask.js" type="text/javascript" defer="defer"></script>
    </form>
</body>
</html>

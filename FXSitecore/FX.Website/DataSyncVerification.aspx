<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataSyncVerification.aspx.cs" Inherits="FX.Website.DataSyncVerification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label>Server Hostname:</label><asp:TextBox ID="url" runat="server"></asp:TextBox>
        <asp:Button ID="pruneBtn" runat="server"  Text="Run"/>
        <asp:TextBox runat="server" ID="itemsnum"></asp:TextBox>
    </div>
    </form>
</body>
</html>

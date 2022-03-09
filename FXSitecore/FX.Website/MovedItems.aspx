<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MovedItems.aspx.cs" Inherits="FX.Website.MovedItems" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server"> 
    <div>
        <asp:Button runat="server" ID="startBtn" Text="start"/> <br />
        <asp:Repeater ID="listOfPages" runat="server">
            <ItemTemplate>
                <asp:Literal runat="server" ID="path"></asp:Literal><br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
        <hr />
    <div>
        <asp:Button runat="server" ID="findMediaBtn" Text="Find Media"/> <br />
        <asp:Repeater ID="listOfPages2" runat="server">
            <ItemTemplate>
                <asp:Literal runat="server" ID="path"></asp:Literal><br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>

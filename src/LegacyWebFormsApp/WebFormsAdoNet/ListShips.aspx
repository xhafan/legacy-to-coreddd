<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListShips.aspx.cs" Inherits="LegacyWebFormsApp.WebFormsAdoNet.ListShips" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <a href="CreateShip.aspx">Create new ship</a>
    <br />
    <a href="UpdateShip.aspx">Update ship</a>
    <br />
    <form id="form1" runat="server">
        <div>
            Existing ships:
            <br />
            <asp:ListBox ID="ExistingShipsListBox" runat="server" Width="800" Height="200"></asp:ListBox>
        </div>
    </form>
</body>
</html>

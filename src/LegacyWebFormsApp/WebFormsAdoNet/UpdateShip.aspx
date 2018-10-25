<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateShip.aspx.cs" Inherits="LegacyWebFormsApp.WebFormsAdoNet.UpdateShip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Ship Id:
            <asp:TextBox ID="ShipIdTextBox" runat="server"></asp:TextBox>
            <br />
            Ship name:
            <asp:TextBox ID="ShipNameTextBox" runat="server"></asp:TextBox>
            <br />
            Tonnage:
            <asp:TextBox ID="TonnageTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Button 
                ID="UpdateShipButton" 
                runat="server" 
                OnClick="UpdateShipButton_OnClick" 
                Text="Update ship data" 
                ToolTip="Clicking the button will execute a database stored procedure to update an existing ship" 
                />
        </div>
    </form>
    <a href="ListShips.aspx">Back to the list of ships</a>
</body>
</html>

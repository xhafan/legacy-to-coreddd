<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateShip.aspx.cs" Inherits="LegacyWebFormsApp.WebFormsAdoNet.CreateShip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Ship name:
            <asp:TextBox ID="ShipNameTextBox" runat="server"></asp:TextBox>
            <br />
            Tonnage:
            <asp:TextBox ID="TonnageTextBox" runat="server"></asp:TextBox>
            <br />
            IMO (International Maritime Organization) Number:
            <asp:TextBox ID="ImoNumberTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Button 
                ID="CreateShipButton" 
                runat="server" 
                OnClick="CreateShipButton_Click" 
                Text="Create new ship" 
                ToolTip="Clicking the button will execute a database stored procedure to create a new ship" 
            />
            <br />
            Last ShipId created:
            <asp:Label ID="LastShipIdCreatedLabel" runat="server" Text=""></asp:Label>
        </div>
    </form>
    <a href="ListShips.aspx">Back to the list of ships</a>
</body>
</html>

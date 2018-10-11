<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageShipsLegacyAdoNet.aspx.cs" Inherits="LegacyWebFormsApp.ManageShipsLegacyAdoNet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Ship name:
            <asp:TextBox ID="CreateShipNameTextBox" runat="server"></asp:TextBox>
            <br />
            Tonnage:
            <asp:TextBox ID="CreateTonnageTextBox" runat="server"></asp:TextBox>
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
            <hr />
            Ship Id:
            <asp:TextBox ID="UpdateShipIdTextBox" runat="server"></asp:TextBox>
            <br />
            Ship name:
            <asp:TextBox ID="UpdateShipNameTextBox" runat="server"></asp:TextBox>
            <br />
            Tonnage:
            <asp:TextBox ID="UpdateTonnageTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Button 
                ID="UpdateShipButton" 
                runat="server" 
                OnClick="UpdateShipButton_OnClickShipButton_Click" 
                Text="Update ship data" 
                ToolTip="Clicking the button will execute a database stored procedure to update an existing ship" 
                />
            <hr />
            Existing ships:
            <br />
            <asp:ListBox ID="ExistingShipsListBox" runat="server" Width="500" Height="200"></asp:ListBox>
        </div>
    </form>
</body>
</html>

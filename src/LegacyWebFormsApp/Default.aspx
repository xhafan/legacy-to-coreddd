<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LegacyWebFormsApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="CreateShipButton" runat="server" OnClick="CreateShipButton_Click" Text="Create new ship" ToolTip="Clicking the button will execute a database stored procedure to create a new ship" />            
            <asp:ListBox ID="GeneratedShipIdsListBox" runat="server"></asp:ListBox>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisclaimerPage.aspx.cs" Inherits="SeerSpot3.DisclaimerPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            do you want to accept cookies?</div>
        <asp:Button ID="AcceptBtn" runat="server" OnClick="AcceptBtn_Click" Text="Accept" />
        <asp:Label ID="piumal" runat="server" Text="label"></asp:Label>
    </form>
</body>
</html>


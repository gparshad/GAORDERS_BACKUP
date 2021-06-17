<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewCV3Orders.aspx.cs" Inherits="ViewCV3Orders" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Start: <asp:TextBox runat="server" ID="uxStart" Text="" /><br />
    End: <asp:TextBox runat="server" ID="uxEnd" Text="" /><br />
    StoreCode: <asp:TextBox runat="server" ID="uxStoreCode" Text="" /><br />
    <asp:TextBox runat="server" ID="uxResponse" Text="" TextMode="MultiLine" Width="800" Height="600" /><br />
    <asp:Button runat="server" ID="uxGo" Text="Fetch" onclick="uxGo_Click" />
    </div>
    </form>
</body>
</html>

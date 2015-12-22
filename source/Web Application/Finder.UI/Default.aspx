<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Finder.UI._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Finder</title>
    <link href="css/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Page_Load() {
        }
        
        function OnHelpClick() {
            window.open('Help.aspx', null, 'height=200,width=700,toolbar=no');
        }
        
    </script>
</head>
<body onload="Page_Load()">
    <form id="form1" runat="server">
    <div class="hat">
        <a class="help" onclick="OnHelpClick()">Help</a>
    </div>
    <table>
        <tr>
            <td>Target Address:</td>
            <td>
                <asp:DropDownList ID="addressList" runat="server"></asp:DropDownList>
                <asp:TextBox ID="targetAddress" runat="server" Width="300"></asp:TextBox>
                <asp:RequiredFieldValidator ID="targetAddressValidator" runat="server" ControlToValidate="targetAddress" ErrorMessage="Target address is not defined"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="targetAddressRegularExpressionValidator" runat="server" ControlToValidate="targetAddress" ValidationExpression="http://(www)?[-A-Za-z0-9_.]+[-A-Za-z0-9/_.:@&?=+%]*" ErrorMessage="Target address is not valid. Valid format is 'http://site.com'"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>Regex:</td>
            <td>
                <asp:TextBox ID="param" runat="server" TextMode="MultiLine" Rows="5" Width="300"></asp:TextBox>
                <asp:RequiredFieldValidator ID="paramValidator" runat="server" ControlToValidate="param" ErrorMessage="Parameters is not defined"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><asp:Button ID="find" runat="server" Text="Find" OnClick="OnFindClickHandler" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="getXml" runat="server" Text="Get result in XML" OnClick="OnGetResultClick" /></td>
            <td><asp:Button ID="getHtml" runat="server" Text="Get result in Browser" OnClick="OnGetResultClick" /></td>
        </tr>
    </table>
    <asp:Literal ID="result" runat="server"></asp:Literal>
    <asp:HiddenField ID="isPostBack" runat="server" Value="false" />
    </form>
</body>
</html>

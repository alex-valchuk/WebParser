<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Searcher.UI.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Searcher</title>
    <link href="~/css/default.css" rel="stylesheet" type="text/css" />
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
    <table style="float:left;width:400px;">
        <tr>
            <td>Address:</td>
            <td>
                <asp:DropDownList ID="addressList" runat="server" Width="200" ></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Parameters:</td>
            <td>
                <asp:TextBox ID="param" runat="server" Width="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td><asp:Button ID="find" runat="server" Text="Find" OnClick="OnFindClickHandler" /></td>
        </tr>
    </table>
    <table style="float:left;width:500px;">
        <tr>
            <td><asp:ListBox ID="fileListBox" runat="server" Width="300"></asp:ListBox></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="getXml" runat="server" Text="Get result in XML" OnClick="OnGetResultClick" />
                <asp:Button ID="getHtml" runat="server" Text="Get result in Browser" OnClick="OnGetResultClick" />
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="message" runat="server" CssClass="count"></asp:Label></td>
        </tr>
    </table>
    <asp:Literal ID="result" runat="server"></asp:Literal>
    <asp:HiddenField ID="isPostBack" runat="server" Value="false" />
    </form>
</body>
</html>

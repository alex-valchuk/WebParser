<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="Finder.UI.Help" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Help</title>
    <link href="css/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul>
            <li>Поле Target Address - адрес web-ресурса, на котором будет произведен поиск</li>
            <li>Адрес должeн быть в формате 'http:// + адрес'</li>
            <li>Поле Regex - регулярное выражение для поиска</li>
            <li>Параметры '<' и '>' указываются как "амперсанд+lt;" "амперсанд+gt;" соответственно </li>
        </ul>
    </div>
    </form>
</body>
</html>

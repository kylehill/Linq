<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InsertWordList.aspx.cs" Inherits="InsertWordList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Add words:<br />
        <asp:TextBox ID="txtWordList" runat="server" TextMode="MultiLine" Rows="8" /><br />
        <asp:Button ID="btnAddWords" runat="server" Text="Submit new words" 
            onclick="btnAddWords_Click" />
        <br /><br />
        Random word:<asp:Label ID="lblRandomWord" runat="server" />
    </div>
    </form>
</body>
</html>

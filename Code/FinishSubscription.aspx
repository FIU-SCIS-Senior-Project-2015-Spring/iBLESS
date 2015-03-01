<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinishSubscription.aspx.cs" Inherits="WebApplication1.FinishSubscription" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body style="background:#eee">
    
</body>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery.validate/1.11.1/jquery.validate.js" type="text/javascript"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>
    <script src="UtilityScript.js"></script>

    <script>
        $(document).ready(function ()
        {
            var parameter = { ID: getURLParameter("id") }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "FinishSubscription.aspx/Subscribe",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true) {
                        alert("Subscribed")
                        window.location.href = '/UserManagement.aspx';
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        })
    </script>

</html>

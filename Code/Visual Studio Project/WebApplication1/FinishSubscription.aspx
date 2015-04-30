<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinishSubscription.aspx.cs" Inherits="WebApplication1.FinishSubscription" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet" />
    <link href="ChangeInfoCSS.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="container">
    		<p><br/></p>
  		<div class="row">
            <div class="col-md-4"></div>
  			<div class="col-md-4" id ="workspace">
  			</div>
		</div>
    </div>
</body>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery.validate/1.11.1/jquery.validate.js" type="text/javascript"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>
    <script src="UtilityScript.js?ver=5"></script>

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
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-success" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> Payment successfully confirmed! Thank you! Redirecting to user management...</div>');
                        setTimeout(function () { window.location.href = '/UserManagement.aspx' }, 3000);
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        })
    </script>

</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="WebApplication1.UserManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
</head>

<style>
    label {
  width:100px;
  font-size:larger;
}
    span { width: 20px;}
</style>

<body style="background:#eee; display:none" >
    <div class="container">
        <p><br /></p>
		<div class="col-md-12 column">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" alt="Smiley face" width="60" height="60" /></a>
            <div class="panel-body">
			    <ul class="nav nav-tabs" style="padding-top:1.5em">
				    <li>
					    <a href="/ChangeInformation.aspx">Change Information</a>
				    </li>
				    <li>
					    <a href="/Subscriptions.aspx">Subscription</a>
				    </li>
				    <li id="CreateUser">
					    <a href="/CreateUsers.aspx">Create User</a>
				    </li>
                    <li class="pull-right" style="cursor:pointer">
                        <a onclick="deleteCookies()">Log out</a>
                    </li>
			    </ul>
                <br />
                <span class="glyphicon glyphicon-user"></span><label for="username">Username:</label><p style="display:inline"> </p><pre style="display:inline" id="username"></pre>
                <p></p>
                <br />
                <span class="glyphicon glyphicon-font"></span><label for="firstName">First Name:</label><p style="display:inline"> </p><pre style="display:inline" id="firstName"></pre>
                <p></p>
                <br />
                <span class="glyphicon glyphicon-bold"></span><label for="lastName">Last Name:</label><p style="display:inline"> </p><pre style="display:inline" id="lastName"></pre>
                <p></p>
                <br />
                <span class="glyphicon glyphicon-envelope"></span><label for="email">E-mail:</label><p style="display:inline"> </p><pre style="display:inline" id="email"></pre>
            </div>
        </div>
		</div>
    </div>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery.validate/1.11.1/jquery.validate.js" type="text/javascript"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>
    <script src="UtilityScript.js"></script>

    <script>
        function getInformation ()
        {
            var parameter = { UserName: "Alberto" };

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "UserManagement.aspx/GetInformation",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = JSON.parse(response.d);
                    
                    $("#username").text(data.UserName);
                    $("#firstName").text(data.FirstName);
                    $("#lastName").text(data.LastName);
                    $("#email").text(data.Email);
                    $("body").fadeIn("slow");
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        $(document).ready(function () {
            GetUserName();
            getInformation();
        })
    </script>

</body>
</html>

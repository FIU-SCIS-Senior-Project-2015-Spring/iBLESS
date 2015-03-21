<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="WebApplication1.UserManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
</head>

<style>
    label {
        width: 100px;
        font-size: larger;
        }
}
    span { width: 20px;}
</style>

<body style="background:#eee;opacity:0" >
    <div class="container">
        <p><br /></p>
		<div class="col-md-12 column">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" alt="Smiley face" width="60" height="60" /></a>
            <div class="panel-body" id="workspace">
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
                    <li>
					    <a href="/CreateTable.aspx">Create Hierarchy</a>
				    </li>
                    <li class="pull-right" style="cursor:pointer">
                        <a onclick="deleteCookies()">Log out</a>
                    </li>
			    </ul>
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
    <script src="UtilityScript.js?ver=2"></script>

    <script>
        function getInformation() {
            $.ajax({
                type: "POST",
                url: "UserManagement.aspx/GetInformation",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = JSON.parse(response.d);

                    CreateFields(data);
                    $("body").css("opacity", 1);
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        $(document).ready(function () {
            eraseSession();
            GetUserName();
            getInformation();
        })

        function CreateFields(data) {
            var i = 0;

            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    if (i % 2 === 0)
                        $("#workspace").append('<p></p><br />')
                    if (i % 2 === 0)
                        $("#workspace").append('<div style="display:inline" id="div' + i + '"><label for="' + key + '">' + key + ':</label><p style="display:inline"> </p><pre style="display:inline">' + data[key] + '</pre></div>');
                    if (i % 2 === 1) {
                        $("#workspace").append('<div style="display:inline" id="div' + i + '"><label for="' + key + '" id="label' + i + '">' + key + ':</label><p style="display:inline"> </p><pre style="display:inline">' + data[key] + '</pre></div>');
                        $("#label" + i).width(300 - $("#div" + i).width());
                        $("#div" + (i - 1)).css("margin-right", 500 - $("#div" + (i - 1)).width());
                    }

                    i++;
                }
            }
        }
    </script>

</body>
</html>

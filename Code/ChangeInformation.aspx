<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeInformation.aspx.cs" Inherits="WebApplication1.ChangeInformation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ChangeInformation</title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <link href="ChangeInfoCSS.css" rel="stylesheet" type="text/css" />
</head>

<style>
    label 
    {
        width:130px;
        font-size:medium;
        float:left;
    }
    span
    {
        width:20px;
    }
    .error { border: 1px solid #b94a48!important; background-color: #fee!important; }
</style>

<body style="background:#eee; display:none">
    <form class="form-inline">
    <div class="container">
        <p><br /></p>
		<div class="col-md-12 column" id="workspace">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" alt="Smiley face" width="60" height="60" /></a>
            <div class="panel-body">
			    <ul class="nav nav-tabs" style="padding-top:1.5em">
				    <li class="active">
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
                <br />
                <p style="font-size:larger"><strong>Note: Any field left blank will not produce any change in data. Only change the fields you want to change.</strong></p>
                </br>
                <label for="firstName">First Name:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input type="text" placeholder="First Name" class="form-control" id="firstName" />
                    <label for="firstName" class="glyphicon glyphicon-font"></label>
                </div>
                </div>
                <p></p>
                <br />
                <label for="lastName">Last Name:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input type="text" placeholder="Last Name" class="form-control" id="lastName" />
                    <label for="lastName" class="glyphicon glyphicon-bold"></label>
                </div>
                </div>
                <p></p>
                <br />
                <label for="oldPass">Old Password:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input type="password" placeholder="Old Password" class="form-control" id="oldPass" />
                    <label for="oldPass" class="glyphicon glyphicon-lock"></label>
                </div>
                </div>
                <p></p>
                <br />
                <label for="newPass">New Password:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input type="password" placeholder="New Password" class="form-control" id="newPass" />
                    <label for="newPass" class="glyphicon glyphicon-lock"></label>
                </div>
                </div>
                <p></p>
                <br />
                <label for="newPass">New Password Confirmation:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input type="password" placeholder="New Password Confirmation" class="form-control" id="newPassConfirmation" />
                    <label for="newPassConfirmation" class="glyphicon glyphicon-lock"></label>
                </div>
                </div>
                 <hr/>

	            <button class="btn btn-primary btn-md" type="submit"><span class="glyphicon glyphicon-send"></span> Submit</button>
                <p><br/></p>
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
        $("form").submit(function () { updateInformation(); return false; } )

        function oldAndNewPresent() {
            var boolean = ($("#newPass").val() === "" || $("#oldPass").val() !== "") && ($("#oldPass").val() === "" || $("#newPass").val() !== "");

            if (!boolean) {
                $("#newPass").tooltip("destroy").data("title", "New Password must be present if you want to change your password!").addClass("error").tooltip();
                $("#oldPass").tooltip("destroy").data("title", "Old Password must be present if you want to change your password!").addClass("error").tooltip();
                $("#newPassConfirmation").tooltip("destroy").data("title", "New Password Confirmation must be present if you want to change your password!").addClass("error").tooltip();
            }

            return boolean;
        }

        function updateInformation() {
            var shouldReturn = !passwordsNotEqual() && oldAndNewPresent();

            if (!shouldReturn)
                return;

            var parameter = { FirstName: $("#firstName").val(), LastName: $("#lastName").val(), OldPassword: $("#oldPass").val(), NewPassword: $("#newPass").val() };

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "ChangeInformation.aspx/UpdateInformation",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === 0) {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> No information submitted!.</div>');
                    }
                    else if (response.d === 2) {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> Old password is incorrect!.</div>');
                    }
                    else {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-success" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> New information saved!.</div>');
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        $(document).ready(function () {
            eraseSession();
            GetUserName();
            $("body").fadeIn("slow");
        })

        function passwordsNotEqual() {
            $("#oldPass").removeClass("error").tooltip("destroy");
            $("#newPass").removeClass("error").tooltip("destroy");
            $("#newPassConfirmation").removeClass("error").tooltip("destroy");

            if ($("#newPassConfirmation").val() !== $("#newPass").val()) {
                $("#newPass").tooltip("destroy").data("title", "Passwords are not equal!").addClass("error").tooltip();
                $("#newPassConfirmation").tooltip("destroy").data("title", "Passwords are not equal!").addClass("error").tooltip();

                return true;
            }

            return false;
        }
    </script>
</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="WebApplication1.UserManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title></title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <link href="ChangeInfoCSS.css" rel="stylesheet" type="text/css" />
</head>

<style>
    label {
        width: 127px;
        font-size: larger;
        }
    input {
        width: 200px !important;
    }

    span { width: 20px;}
</style>

<body style="opacity:0" >
    <form class="form-inline">
    <div class="container">
        <p><br /></p>
		<div class="col-md-12 column">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" alt="Smiley face" width="60" height="60" /></a>
            <div onclick="deleteCookies()"" id="logout" style="float:right; cursor:pointer; margin-top:1em; margin-right: 1em; padding-right: 20px; padding-left: 20px"> 
                <a><label id="welcome" style="font-size:smaller; color: rebeccapurple; width: auto ; cursor:pointer"></label><br />Log out</a>
            </div>
            <div class="panel-body" id="workspace">
			    <ul class="nav nav-tabs" style="padding-top:1.5em">
				    <li id="changeInformation">
					    <a href="/ChangeInformation.aspx">Change Information</a>
				    </li>
				    <li id="subscriptions">
					    <a href="/Subscriptions.aspx">Subscription</a>
				    </li>
				    <li id="createUser">
					    <a href="/CreateUsers.aspx">Create User</a>
				    </li>
                    <li id="createTable">
					    <a href="/CreateTable.aspx">Create Hierarchy</a>
				    </li>
                    <li id="vibrationPattern">
					    <a href="/VibrationPattern.aspx">Select Vibration</a>
				    </li>
                    <li id="checkSPL">
                        <a href="/CheckSPL.aspx">Check SPL Values</a>
                    </li>
			    </ul>
                <div id="adminDiv" hidden>
                <p></p><br /><label style="font-size: x-large;text-decoration: underline;width: auto">Subscription Information</label><p></p><br />
                <label class="adminLabels" for="companyName">Company Name</label>
                <div class="form-group">
                    <input readonly="true" data-msg-required="Required!" data-rule-required="true" type="text" class="form-control" id="companyName" name="company" />
                </div>
                <label class="adminLabels" for="address">Address</label>
                <div class="form-group">
                    <input readonly="true" data-msg-required="Required!" data-rule-required="true" type="text" class="form-control" id="address" name="address" />
                </div>
                <p></p>
                <br />
                <label class="adminLabels" for="type">Company Type</label>
                <div class="form-group">
                    <input readonly="true" data-msg-required="Required!" data-rule-required="true" type="text" class="form-control" id="type" name="type" />
                </div>
                <label class="adminLabels" for="subscription">Subscription</label>
                <div class="form-group">
                    <input readonly="true" data-msg-required="Required!" data-rule-required="true" type="text" class="form-control" id="subscription" name="subscription" />
                </div>
                <p></p>
                <br />
                <label class="adminLabels" for="webLink">Website</label>
                <div class="form-group">
                    <input readonly="true" data-msg-required="Required!" data-rule-required="true" type="text" class="form-control" id="webLink" name="webLink" />
                </div>
                <label class="adminLabels" for="splType">dBA Regulation</label>
                <div class="form-group">
                    <input readonly="true" data-msg-required="Required!" data-rule-required="true" type="text" class="form-control" id="splType" name="splType" />
                </div>
                <p></p>
                <br />
                <label class="adminLabels" for="tagLine">Tagline</label>
                <div class="form-group">
                    <input readonly="true" data-msg-required="Required!" data-rule-required="true" type="text" class="form-control" id="tagLine" name="tagLine" />
                </div>
                <p></p>
                <br />
                <label style="width: 200px" class="adminLabels" for="description">Company Description</label>
                <div class="form-group">
                    <textarea readonly style="height: auto" rows="5" class="form-control col-xs-12" id="description"></textarea>
                </div>
                </div>
            </div>
        </div>
		</div>
    </div>
    </form>
</body>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery.validate/1.11.1/jquery.validate.js" type="text/javascript"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>
    <script src="UtilityScript.js?ver=6"></script>

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
            setNavigationBar();
            setWelcome();
            getInformation();
        })

        function CreateFields(data) {
            var i = 0;

            SetUserType();

            for (var key in data) {
                if (data.hasOwnProperty(key)) {

                    if (key === "Username") {
                        $("#workspace").append('<p></p><br />')
                        $("#workspace").append('<div style="display:inline" id="usernameDiv"><div style="display:inline" id="username"><label for="' + key + '">' + key + '</label><p style="display:inline"> </p><pre style="display:inline">' + data[key] + '</pre></div></div>');
                        continue;
                    }
                    else if (data[key] === "")
                        continue;
                    else if (key === "EmployeeID")
                    {
                        $("#usernameDiv").append('<div style="display:inline"><label for="' + key + '" id="empID">' + key + '</label><p style="display:inline"> </p><pre style="display:inline">' + data[key] + '</pre></div>');
                        $("#empID").width(100);
                        $("#username").css("margin-right", $("#workspace").width() - $("#username").width() - 300);
                        continue;
                    }

                    var label = "";

                    switch (key)
                    {
                        case "SPL_Type":
                            label = "dBA Regulation";
                            break;
                        case "FirstName":
                            label = "First Name";
                            break;
                        case "LastName":
                            label = "Last Name";
                            break;
                        case "ID":
                            label = "User ID";
                            break;
                        case "EmployeeID":
                            label = "Employee ID";
                            break;
                        default:
                            label = key;
                    }

                    if (i % 2 === 0)
                        $("#workspace").append('<p></p><br />')
                    if (i % 2 === 0)
                        $("#workspace").append('<div style="display:inline" id="div' + i + '"><label for="' + label + '">' + label + '</label><p style="display:inline"> </p><pre id=' + i + ' style="display:inline">' + data[key] + '</pre></div>');
                    if (i % 2 === 1) {
                        $("#workspace").append('<div style="display:inline" id="div' + i + '"><label for="' + label + '" id="label' + i + '">' + label + '</label><p style="display:inline"> </p><pre style="display:inline">' + data[key] + '</pre></div>');
                        $("#" + (i - 1)).css("margin-right", 200 - $("#" + (i - 1)).width());
                    }

                    i++;
                }
            }

            setAdminInformation();
        }

        function SetUserType ()
        {
            var parameter = { Username: GetUserName() }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "Subscriptions.aspx/GetUserType",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#workspace").append('<p></p><br /><label style="font-size: x-large;text-decoration: underline;width: auto">' + response.d + ' Information</label>')
                },
                failure: function (response) {
                    alert(response.d);
                },
                async: false
            })
        }

        function setAdminInformation () {
            $.ajax({
                type: "POST",
                url: "Subscriptions.aspx/GetInformationAdmin",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d !== " ") {
                        var information = JSON.parse(response.d);

                        $("#companyName").val(information.Name);
                        $("#address").val(information.Address);
                        $("#type").val(information.Type);
                        $("#subscription").val(information.Subscription);
                        $("#splType").val(information.SPL_Type);
                        $("#tagLine").val(information.Tagline);
                        $("#description").val(information.Description);
                        $("#webLink").val(information.Website);
                        $("#adminDiv").show();
                        $("#description").width($("#workspace").width() - 30);
                    }
                },
                failure: function (response) {
                    alert(response.d);
                },
                async: false
            })
        }
    </script>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckSPL.aspx.cs" Inherits="WebApplication1.CheckSPL" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <link href="ChangeInfoCSS.css" rel="stylesheet" type="text/css" />
</head>

<style>
    label 
    {
        width: 80px;
        font-size:medium;
    }
    span
    {
        width:20px;
    }
    .error { border: 1px solid #b94a48!important; background-color: #fee!important; }
    input { width:100px!important; }
</style>

<body style="opacity:0">
    <form class="form-inline">
        <div class="container">
        <p><br /></p>
		<div class="col-md-12 column" id="workspace">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" width="60" height="60" /></a>
            <div onclick="deleteCookies()"" id="logout" style="float:right; cursor:pointer; margin-top:1em; margin-right: 1em; padding-right: 20px; padding-left: 20px"> 
                <a><label id="welcome" style="font-size:smaller; color: rebeccapurple; width: auto ; cursor:pointer"></label><br />Log out</a>
            </div>
            <div class="panel-body">
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
                    <li class="active" id="checkSPL">
                        <a href="/CheckSPL.aspx">Check SPL Values</a>
                    </li>
			    </ul>
                <br />
                <div id ="body">
                <div class="bs-example" id="tableDiv">
                    <table class="table table-hover" id="table">
                        <thead>
                            <tr>
                                <th>Row</th>
                                <th>Name</th>
                                <th>Location</th>
                                <th>SPL</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                 <hr/>
                <p></p>
                <button class="btn ibless-button btn-md" type="submit"><span class="glyphicon glyphicon-send"></span>Notify Safety Managers</button>
                </div>
            </div>
        </div>
		</div>
    </div>
    </form>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery.validate/1.11.1/jquery.validate.js" type="text/javascript"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>
    <script src="UtilityScript.js?ver=5"></script>

    <script>
        function HasBoss() {
            $.ajax({
                type: "POST",
                url: "CreateUsers.aspx/HasBoss",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true)
                        LoadTable();
                    else
                        window.location.href = "/UserManagement.aspx"
                },
                failure: function (response) {
                    alert(response.d);
                }
            })

        }

        $("form").submit(function ()
        {
            $.ajax(
                {
                    type: "POST",
                    url: "CheckSPL.aspx/AlertSafetyManagers",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                })

            $("#alert").remove();
            $("#workspace").prepend('<div class="alert alert-success" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> SMS Sent!</div>');

            return false;
        })

        $(document).ready(function ()
        {
            eraseSession();
            GetUserName();
            setNavigationBar();
            setWelcome();
            HasBoss();
        })

        function LoadTable ()
        {
            $.ajax(
                {
                    type: "POST",
                    url: "CheckSPL.aspx/GetSPLValues",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d === "[]")
                        {
                            $("#alert").remove();
                            $("#workspace").prepend('<div class="alert alert-info" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Note!</strong> No SPL readings!</div>');
                            $("#body").hide();
                        }
                        SetUpTable(JSON.parse(response.d))
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                })
        }

        function SetUpTable (data)
        {
            for (i = 0 ; i < data.length ; i++)
                $("#table").append("<tr><td> " + (i + 1) + "</td><td>" + data[i].Name + "</td><td>" + data[i].Location + "</td><td>" + data[i].SPLValue + "</td></tr>");

            $("body").css("opacity", 1);
        }
    </script>
</body>
</html>

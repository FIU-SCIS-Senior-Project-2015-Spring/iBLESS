<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VibrationPattern.aspx.cs" Inherits="WebApplication1.VibrationPattern" %>

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

<body style="background:#eee;opacity:0">
    <form class="form-inline">
        <div class="container">
        <p><br /></p>
		<div class="col-md-12 column" id="workspace">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" width="60" height="60" /></a>
            <div class="panel-body">
			    <ul class="nav nav-tabs" style="padding-top:1.5em">
				    <li>
					    <a href="/ChangeInformation.aspx">Change Information</a>
				    </li>
				    <li>
					    <a href="/Subscriptions.aspx">Subscription</a>
				    </li>
				    <li>
					    <a href="/CreateUsers.aspx">Create User</a>
				    </li>
                    <li>
					    <a href="/CreateTable.aspx">Create Hierarchy</a>
				    </li>
                    <li class="active">
					    <a href="/VibrationPattern.aspx">Select Vibration</a>
				    </li>
                    <li>
                        <a href="/CheckSPL.aspx">Check SPL Values</a>
                    </li>
                    <li class="pull-right" style="cursor:pointer">
                        <a onclick="deleteCookies()">Log out</a>
                    </li>
			    </ul>
                <br />
                <div class="bs-example" id="tableDiv">
                    <table class="table table-hover" id="table">
                        <thead>
                            <tr>
                                <th>Row</th>
                                <th>Range</th>
                                <th>Vibration</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                 <hr/>
                <label for="low">Low dB:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="40" class="form-control" id="low" name="low" />
                    <label for="low" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="high">High dB:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="40" class="form-control" id="high" name="high" />
                    <label for="high" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="vibrations">Vibration: </label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select name="vibrations" class="form-control" id="vibrations">
                        
                    </select>
                    <label for="vibrations" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <button class="btn btn-primary btn-md" type="submit"><span class="glyphicon glyphicon-send"></span> Submit</button>
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
    <script src="UtilityScript.js?ver=2"></script>

    <script>
        function HasBoss() {
            $.ajax({
                type: "POST",
                url: "CreateUsers.aspx/HasBoss",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true) {
                        GetVibrations();
                        GetTable();
                    }
                    else
                        window.location.href = "/UserManagement.aspx"
                },
                failure: function (response) {
                    alert(response.d);
                }
            })

        }

        $(document).ready(function ()
        {
            eraseSession();
            GetUserName();
            HasBoss();
        })

        function GetTable ()
        {
            $.ajax({
                type: "POST",
                url: "VibrationPattern.aspx/GetTable",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    SetUpTable(JSON.parse(response.d).Vibrations);
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        function SetUpTable (vibrations)
        {
            $("#table").append("<tbody>");

            for (i = 0 ; i < vibrations.length ; i++)
                $("#table").append("<tr><td>" + (i+1) + "</td><td>" + vibrations[i].Low + "-" + vibrations[i].High + "</td><td>" + vibrations[i].Name + "</td>" + 
                    "<td><button type='button' class='btn btn-primary' onclick=\"Delete(" + vibrations[i].ID + ")\">Delete</button></td></tr>");
            
            $("#table").append("</tbody>");

            $("body").css("opacity", 1);
        }

        function Delete (id)
        {
            var parameter = { ID: id }

            $.ajax(
            {
                type: "POST",
                url: "VibrationPattern.aspx/DeleteVibration",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(parameter),
                success: function (response) {
                    if (response.d === 0)
                        window.location.href = "/VibrationPattern.aspx"
                    else
                    {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> Error deleting vibration!</div>');
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        function GetVibrations ()
        {
            $.ajax(
            {
                type: "POST",
                url: "VibrationPattern.aspx/GetVibrations",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    SetUpVibrations(JSON.parse(response.d).Vibrations)
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        function SetUpVibrations (vibrations)
        {
            for (i = 0 ; i < vibrations.length ; i++)
                $("#vibrations").append("<option value=\"" + vibrations[i].Setting + "\">" + vibrations[i].Name + "</option>")
        }

        $("form").validate({

            showErrors: function (errorMap, errorList) {

                // Clean up any tooltips for valid elements
                $.each(this.validElements(), function (index, element) {
                    var $element = $(element);

                    $element.data("title", "") // Clear the title - there is no error associated anymore
                        .removeClass("error")
                        .tooltip("destroy");
                });

                // Create new tooltips for invalid elements
                $.each(errorList, function (index, error) {
                    var $element = $(error.element);

                    $element.tooltip("destroy") // Destroy any pre-existing tooltip so we can repopulate with new tooltip content
                        .data("title", error.message)
                        .addClass("error")
                        .tooltip(); // Create a new tooltip based on the error messsage we just set in the title
                });
            },

            submitHandler: function (form) {
                    submit();
            }
        });

        function submit() {
            if (Number($("#low").val()) >= Number($("#high").val()))
            {
                $("#low").data("title", "Low dB must be lower than High dB!").addClass("error").tooltip();
                $("#high").data("title", "High dB must be higher than Low dB!").addClass("error").tooltip();

                return;
            }

            var parameter = { Low: $("#low").val(), High: $("#high").val(), Setting: $("#vibrations").val() }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "VibrationPattern.aspx/AddVibration",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === 0)
                        window.location.href = "/VibrationPattern.aspx"
                    else if (response.d === -1) {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> Range overlaps with existent value!</div>');
                    }
                    else {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> There was a problem with the database. Try again!</div>');
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }
    </script>
</body>
</html>

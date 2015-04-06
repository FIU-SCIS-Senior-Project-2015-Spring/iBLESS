<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateTable.aspx.cs" Inherits="WebApplication1.CreateTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />
    <link href="ChangeInfoCSS.css" rel="stylesheet" type="text/css" />
</head>

<style>
    .error { border: 1px solid #b94a48!important; background-color: #fee!important; }
</style>

<body style="background:#eee; display:none">
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
				    <li id="CreateUser">
					    <a href="/CreateUsers.aspx">Create User</a>
				    </li>
                    <li class="active">
					    <a href="/CreateTable.aspx">Create Hierarchy</a>
				    </li>
                    <li>
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
                <div class="bs-example">
                    <table class="table table-hover" id="table">
                        <thead>
                            <tr>
                                <th>Row</th>
                                <th>ID</th>
                                <th>Type</th>
                                <th>Can Create</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                 <hr/>
                <div id="AddNew" hidden="hidden">
                <p id="insert"><strong>Inserting into position: </strong></p>
                <label for="type">User Type:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Type" class="form-control" id="type" name="type" />
                    <label for="type" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="canCreate">Can Create Users:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="canCreate">
                      <option value="true">True</option>
                      <option value="false">False</option>
                    </select>
                    <label for="canCreate" class="glyphicon glyphicon-ok"></label>
                </div>  
                </div>
                <p></p>
                <button class="btn btn-primary btn-md" type="submit"><span class="glyphicon glyphicon-send"></span> Submit</button>
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
    <script src="UtilityScript.js?ver=2"></script>

    <script>
        var id = 0;
        var jsonData;
        var index = 0;

        function CreateTable (data)
        {
            jsonData = data;

            $("#table").append("<tbody>");

            for (i = 0 ; i < data.length ; i++)
            {
                var type = data[i].Type;

                $("#table").append("<tr><td>" + (i + 1) + "</td><td>" + data[i].ID + "</td><td>" + type + "</td><td>" + data[i].Can_Create + "</td>" +
                    "<td><button type='button' class='btn btn-primary' onclick=\"Insert(" + data[i].ID + ")\">Insert</button><button type='button' class='btn btn-primary' onclick=\"Delete('" + type + "', " + data[i].ID + ")\">Delete</button></td></tr>");
            }

            $("#table").append("</tbody>");
        }

        function Insert(ID) {
            id = ID;
            $("#insert").text("Inserting into position: " + (id + 1));
            $("#insert").css("font-weight","Bold");
            $("#AddNew").show();
        }

        function Delete (type, typeID)
        {
            var parameter = { Type: type, TypeID: typeID }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "CreateTable.aspx/Delete",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#table tbody tr").remove();
                    if (response.d !== " ") {
                        id--;
                        Insert(id);
                        CreateTable(JSON.parse(response.d).result);
                    }
                    else
                    {
                        id = 0;
                        Insert(id);
                        jsonData = undefined;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
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

        function submit ()
        {
            if (jsonData !== undefined && typeDuplicated())
                return;

            var parameter = { row_ID: id, CanCreate: ($("#canCreate").val() === 'true'), NewType: $("#type").val() }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "CreateTable.aspx/InsertAfter",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#table tbody tr").remove();
                    CreateTable(JSON.parse(response.d).result);
                    id++;
                    Insert(id);
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }
        
        function typeDuplicated ()
        {
            for (i = 0 ; i < jsonData.length ; i++)
                if ($("#type").val() === jsonData[i].Type)
                {
                    $("#alert").remove();
                    $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> Type already exists!.</div>');
                    return true;
                }

            return false;
        }

        function CheckSubscription ()
        {
            $.ajax({
                type: "POST",
                url: "CreateTable.aspx/CheckSubscription",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === false)
                        window.location.href = "/UserManagement.aspx";
                    else
                        PrepareTable();
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        $(document).ready(function ()
        {
            $("*").attr("disabled", "disabled");
            eraseSession();
            GetUserName();
            CheckSubscription();
        })

        function PrepareTable ()
        {
            var parameter = { create: true }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "CreateTable.aspx/GetReadyTable",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d !== " ")
                        CreateTable(JSON.parse(response.d).result);
                    else {
                        id = 0;
                        Insert(id);
                        $("#AddNew").show();
                    }

                    $("*").attr("disabled", false);
                    $("body").fadeIn("slow");
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }
    </script>

</body>
</html>

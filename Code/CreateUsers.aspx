<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUsers.aspx.cs" Inherits="WebApplication1.CreateUsers" %>

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
        width: 150px;
        font-size:medium;
    }
    span
    {
        width:20px;
    }
    .error { border: 1px solid #b94a48!important; background-color: #fee!important; }
    input { margin-right: 50px; }
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
				    <li class="active" id="CreateUser">
					    <a href="/CreateUsers.aspx">Create User</a>
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
                                <th>Name</th>
                                <th>Type</th>
                                <th>E-mail</th>
                                <th>Parent</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                 <hr/>
                <div id="AddNew" hidden="hidden">
                <label for="userName">Username:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="User Name" class="form-control" id="userName" name="userName" />
                    <label for="userName" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="firstName">First Name:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="First Name" class="form-control" id="firstName" name="firstName" />
                    <label for="firstName" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="lastName">Last Name:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Last Name" class="form-control" id="lastName" name="lastName" />
                    <label for="lastName" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="email">E-mail:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="email" placeholder="E-Mail" class="form-control" id="email" name="email" />
                    <label for="email" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="roles">User Type:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="roles">

                    </select>
                    <label for="roles" class="glyphicon glyphicon-ok"></label>
                </div> 
                </div>
                <p></p>
                <label for="parents">Parent:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="parents">
                        <option value="-1">No Parent</option>
                    </select>
                    <label for="parents" class="glyphicon glyphicon-ok"></label>
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
    <script src="UtilityScript.js"></script>

    <script>
        function HasBoss ()
        {
            $.ajax({
                type: "POST",
                url: "CreateUsers.aspx/HasBoss",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true)
                    {
                        GetHierarchy();
                        GetParents();
                        PopulateTable();
                    }
                    else
                        window.location.href = "/UserManagement.aspx"
                },
                failure: function (response) {
                    alert(response.d);
                }
            })

        }

        function GetHierarchy ()
        {
            $.ajax({
                type: "POST",
                url: "CreateUsers.aspx/GetHierarchy",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d !== " ") {
                        setUpDropDown(JSON.parse(response.d).result);
                        $("#AddNew").show();
                    }
                    else {
                        $("#tableDiv").remove();
                        $("#workspace").append('<div class="alert alert-danger alert-error" id="alert"><a>&times;</a><strong>Error!</strong> There is no User Hierarchy Created!.</div>');
                    }

                    $("body").fadeIn("slow");
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        function GetParents ()
        {
            $.ajax({
                type: "POST",
                url: "CreateUsers.aspx/GetParents",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    setUpParents(JSON.parse(response.d).result)
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        function PopulateTable ()
        {
            $.ajax({
                type: "POST",
                url: "CreateUsers.aspx/PopulateTable",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    CreateTable(JSON.parse(response.d).result)
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }

        $(document).ready(function () {
            GetUserName();
            HasBoss();
        })

        function setUpParents(data) {
            for (i = 0 ; i < data.length ; i++)
                $("#parents").append("<option value=\"" + data[i].ID + "\">" + data[i].ID + "</option>");
        }

        function CreateTable(data) {
            $("#table").append("<tbody>");

            for (i = 0 ; i < data.length ; i++) {
                if (data[i].ID === getCookie("ID"))
                    continue;

                $("#table").append("<tr><td>" + (i + 1) + "</td><td>" + data[i].Name + "</td><td>" + data[i].Type + "</td>" + "</td><td>" + data[i].Email + "</td>" + "</td><td>" + data[i].Parent + "</td>" +
                    "<td><button type='button' class='btn btn-primary' onclick=\"Delete(" + data[i].ID + ")\">Delete</button></td></tr>");
            }

            $("#table").append("</tbody>");
        }

        function setUpDropDown(data) {
            for (i = 0 ; i < data.length ; i++)
                $("#roles").append("<option value=\"" + data[i].ID + "\">" + data[i].Type + "</option>")
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

        function Delete(id) {
            console.log(id);
            var parameter = { ID: id }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "CreateUsers.aspx/Delete",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true)
                        window.location.href = "/CreateUsers.aspx"
                },
                failure: function (response) {

                }
            })
        }

        function submit() {
            var parameter = { Username: $("#userName").val(), FirstName: $("#firstName").val(), LastName: $("#lastName").val(), Email: $("#email").val(), Type: $("#roles").val(), Parent: ($("#parents").val() === "-1" ? null : $("#parents").val()) }
            var mail = $("#email").val();

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "CreateUsers.aspx/CreateUser",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === -1) {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> E-mail or Username already in use!.</div>');
                    }
                    else {

                        var email = { Email: mail }
                        $.ajax({
                            type: "POST",
                            data: JSON.stringify(parameter),
                            url: "Forgot.aspx/SendMail",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (response.d === false) {
                                    $("#alert").remove();
                                    $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> Error sending e-mail! Send again using Forgot Password Section!.</div>');
                                }
                                else
                                    window.location.href = "/CreateUsers.aspx"
                            },
                            failure: function (response) {
                                alert(response.d);
                            }
                        })

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

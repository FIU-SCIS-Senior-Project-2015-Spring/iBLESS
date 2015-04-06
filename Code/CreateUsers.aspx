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
                    <li>
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
                <label for="phone">Phone:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="tel" placeholder="XXXXXXXXXX" class="form-control" id="phone" name="phone" />
                    <label for="phone" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="employeeID">Employee ID:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="number" placeholder="9999" class="form-control" id="employeeID" name="employeeID" />
                    <label for="employeeID" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="location">City:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Location" class="form-control" id="location" name="location" />
                    <label for="location" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="state">State:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select name="state" class="form-control" id="state">
                        <option value="AL">AL</option>
                        <option value="AK">AK</option>
                        <option value="AZ">AZ</option>
                        <option value="AR">AR</option>
                        <option value="CA">CA</option>
                        <option value="CO">CO</option>
                        <option value="CT">CT</option>
                        <option value="DE">DE</option>
                        <option value="DC">DC</option>
                        <option value="FL">FL</option>
                        <option value="GA">GA</option>
                        <option value="HI">HI</option>
                        <option value="ID">ID</option>
                        <option value="IL">IL</option>
                        <option value="IN">IN</option>
                        <option value="IA">IA</option>
                        <option value="KS">KS</option>
                        <option value="KY">KY</option>
                        <option value="LA">LA</option>
                        <option value="ME">ME</option>
                        <option value="MD">MD</option>
                        <option value="MA">MA</option>
                        <option value="MI">MI</option>
                        <option value="MN">MN</option>
                        <option value="MS">MS</option>
                        <option value="MO">MO</option>
                        <option value="MT">MT</option>
                        <option value="NE">NE</option>
                        <option value="NV">NV</option>
                        <option value="NH">NH</option>
                        <option value="NJ">NJ</option>
                        <option value="NM">NM</option>
                        <option value="NY">NY</option>
                        <option value="NC">NC</option>
                        <option value="ND">ND</option>
                        <option value="OH">OH</option>
                        <option value="OK">OK</option>
                        <option value="OR">OR</option>
                        <option value="PA">PA</option>
                        <option value="RI">RI</option>
                        <option value="SC">SC</option>
                        <option value="SD">SD</option>
                        <option value="TN">TN</option>
                        <option value="TX">TX</option>
                        <option value="UT">UT</option>
                        <option value="VT">VT</option>
                        <option value="VA">VA</option>
                        <option value="WA">WA</option>
                        <option value="WV">WV</option>
                        <option value="WI">WI</option>
                        <option value="WY">WY</option>
                    </select>
                    <label for="state" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="title">Job Title:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Title" class="form-control" id="title" name="title" />
                    <label for="title" class="glyphicon glyphicon-briefcase"></label>
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
    <script src="UtilityScript.js?ver=2"></script>

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
            eraseSession();
            GetUserName();
            HasBoss();
        })

        function CreateTable(data) {
            $("#table").append("<tbody>");

            for (i = 0, j = 0 ; i < data.length ; i++) {
                if (data[i].ID === getCookie("ID"))
                    continue;

                $("#table").append("<tr><td>" + ++j + "</td><td>" + data[i].Name + "</td><td>" + data[i].Type + "</td>" + "</td><td>" + data[i].Email + "</td>" + "</td><td>" + data[i].Parent + "</td>" +
                    "<td><button type='button' class='btn btn-primary' onclick=\"Delete(" + data[i].ID + ")\">Delete</button>" +
                    "<button type='button' class='btn btn-primary' onclick=\"Modify(" + data[i].ID + ")\">Modify Info</button>" + 
                    "<button type='button' class='btn btn-primary' onclick=\"ActivateToggle(" + data[i].ID + ")\" id=\"activeButton" + data[i].ID + "\">" + (data[i].IsInactive === "True" ? "Inactive" : "Active") + "</button></td></tr>");
            }

            $("#table").append("</tbody>");
        }

        function ActivateToggle (ID)
        {
            var id = { UserID: ID }

            $.ajax({
                type: "POST",
                url: "CreateUsers.aspx/ActivateToggle",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(id),
                dataType: "json",
                success: function (response) {
                    $("#activeButton" + ID).text(response.d === true ? "Inactive" : "Active");
                },
                faliure: function (response) {

                }
            })
        }

        function Modify (ID)
        {
            sessionStorage.setItem("ID", ID);
            window.location.href = "ModifyInfo.aspx";
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

                (valid = ValidatePhone()) === false ? InvalidPhone() : true;
            },

            submitHandler: function (form) {
                if (valid)
                    submit();
            }
        });

        function InvalidPhone()
        {
            $("#phone").tooltip("destroy").data("title", "Invalid Telephone Number").addClass("error").tooltip();
        }

        function ValidatePhone ()
        {
            var phone = $("#phone").val();

            if (phone.length != 10)
                return false;

            for (i = 0 ; i < 10 ; i++)
                if (!phone[i].match(/[0-9]/i))
                    return false;

            return true;
        }

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
            var parameter = {
                Username: $("#userName").val(), FirstName: $("#firstName").val(), LastName: $("#lastName").val(), Email: $("#email").val(), Type: $("#roles").val(), Parent: ($("#parents").val() === "-1" ? null : $("#parents").val()),
                Phone: $("#phone").val(), EmployeeID: $("#employeeID").val(), City: $("#location").val(), State: $("#state").val(), Title: $("#title").val()}

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "CreateUsers.aspx/CreateUser",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d !== "") {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> There was a problem with: "' + response.d + '"</div>');
                    }
                    else
                        window.location.href = "/CreateUsers.aspx"
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }
    </script>
</body>
</html>

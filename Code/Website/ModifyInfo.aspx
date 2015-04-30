<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyInfo.aspx.cs" Inherits="WebApplication1.ModifyInfo" %>

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

<body style="display:none">
    <form class="form-inline">
        <div class="container">
        <p><br /></p>
		<div class="col-md-12 column" id="workspace">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" width="60" height="60" /></a>
            <div id="logout" style="float:right; cursor:pointer; margin-top:1em; margin-right: 1em; padding-right: 20px; padding-left: 20px"> 
                <a onclick="deleteCookies()""><label id="welcome" style="font-size:smaller; color: rebeccapurple; width: auto ; cursor:pointer"></label><br />Log out</a>
            </div>
            <div class="panel-body">
			    <ul class="nav nav-tabs" style="padding-top:1.5em">
				    <li id="changeInformation">
					    <a href="/ChangeInformation.aspx">Change Information</a>
				    </li>
				    <li id="subscriptions">
					    <a href="/Subscriptions.aspx">Subscription</a>
				    </li>
				    <li class="active" id="createUser">
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
                <br />
                 <hr/>
                <div id="AddNew">
                <label for="username">Username</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Username" class="form-control" id="username" name="username" />
                    <label for="username" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="employeeID">Employee ID</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="number" placeholder="9999" class="form-control" id="employeeID" name="employeeID" />
                    <label for="employeeID" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="firstName">First Name</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="First Name" class="form-control" id="firstName" name="firstName" />
                    <label for="firstName" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="lastName">Last Name</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Last Name" class="form-control" id="lastName" name="lastName" />
                    <label for="lastName" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="email">E-mail</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="email" placeholder="E-Mail" class="form-control" id="email" name="email" />
                    <label for="email" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="phone">Phone</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="tel" placeholder="(123) 456-7890" class="form-control" id="phone" name="phone" />
                    <label for="phone" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="location">City</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Location" class="form-control" id="location" name="location" />
                    <label for="location" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="title">Job Title</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Title" class="form-control" id="title" name="title" />
                    <label for="title" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <p></p>
                <label for="state">State</label>
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
                <label for="roles">User Type</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="roles">

                    </select>
                    <label for="roles" class="glyphicon glyphicon-ok"></label>
                </div> 
                </div>
                <p></p>
                <label for="parents">Parent</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="parents">
                        <option value="-1">No Parent</option>
                    </select>
                    <label for="parents" class="glyphicon glyphicon-ok"></label>
                </div> 
                </div>
                <p></p>
                <button class="btn ibless-button btn-md" type="submit"><span class="glyphicon glyphicon-send"></span> Submit</button>
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
        $(document).ready(function () {
            setNavigationBar();
            setWelcome();
            
            var parameter = { ID: sessionStorage.getItem("ID") };
            var shouldShow = true;

            $.ajax({
                type: "POST",
                url: "ModifyInfo.aspx/GetRolesInfo",
                data: JSON.stringify(parameter),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d !== " ")
                        setUpDropDown(JSON.parse(response.d).result);
                    else
                        shouldShow = false;
                },
                failure: function (response) {
                    alert(response.d);
                }
            })

            $.ajax({
                type: "POST",
                url: "ModifyInfo.aspx/GetParentsInfo",
                data: JSON.stringify(parameter),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d !== " ")
                        setUpParents(JSON.parse(response.d).result, sessionStorage.getItem("ID"));
                    else
                        shouldShow = false;
                },
                failure: function (response) {
                    alert(response.d);
                }
            })

            $.ajax({
                type: "POST",
                url: "ModifyInfo.aspx/GetInfo",
                data: JSON.stringify(parameter),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d !== " ")
                        populateFields(JSON.parse(response.d));
                    else
                        shouldShow = false;

                    if (shouldShow)
                        $("body").fadeIn("slow");
                    else
                        window.location.href = "/UserManagement.aspx";
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        })

        function populateFields(data)
        {
            $("#username").val(data.Username);
            $("#firstName").val(data.FirstName);
            $("#lastName").val(data.LastName);
            $("#email").val(data.Email);
            $("#phone").val(data.Phone);
            $("#employeeID").val(data.EmployeeID);
            $("#location").val(data.City);
            $("#state").val(data.State);
            $("#title").val(data.Title);
            $("#roles").val(data.Type);
            $("#parents").val(data.Parent);
        }

        function ValidatePhone() {
            var phone = $("#phone").val();

            if (phone.match(/^\s*(\(\d{3}\)|\d{3})\s?\d{3}[- ]?\d{4}\s*$/))
                return true;
            else
                return false;
        }

        function InvalidPhone() {
            $("#phone").tooltip("destroy").data("title", "Invalid Telephone Number").addClass("error").tooltip();
        }

        function parsePhone(phone) {
            var parsedPhone = "";

            for (i = 0 ; i < phone.length ; i++)
                if (phone[i].match(/[0-9]/))
                    parsedPhone += phone[i];

            return parsedPhone;
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

        function submit() {
            var parameter = {
                Username: $("#username").val(), ID: sessionStorage.getItem("ID"), FirstName: $("#firstName").val(), LastName: $("#lastName").val(), Email: $("#email").val(), Type: $("#roles").val(), Parent: ($("#parents").val() === "-1" ? null : $("#parents").val()),
                Phone: parsePhone($("#phone").val()), EmployeeID: $("#employeeID").val(), City: $("#location").val(), State: $("#state").val(), Title: $("#title").val()
            }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "ModifyInfo.aspx/UpdateInfo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === -1)
                    {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong>The E-mail already exists! Change E-mail!</div>');
                    }
                    else if (response.d === -2)
                    {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong>The phone number or employee ID already exists!</div>');
                    }
                    else
                    {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-success" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong>The changes have been changed successfully!</div>');
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

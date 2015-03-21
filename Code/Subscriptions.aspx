<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subscriptions.aspx.cs" Inherits="WebApplication1.Subscriptions" %>

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
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" alt="Smiley face" width="60" height="60" /></a>
            <div class="panel-body">
			    <ul class="nav nav-tabs" style="padding-top:1.5em">
				    <li>
					    <a href="/ChangeInformation.aspx">Change Information</a>
				    </li>
				    <li class="active">
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
                <label for="companyName">Company's Name:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Company" class="form-control" id="companyName" name="company" />
                    <label for="companyName" class="glyphicon glyphicon-font"></label>
                </div>
                </div>
                <label for="address">Address:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Address" class="form-control" id="address" name="address" />
                    <label for="address" class="glyphicon glyphicon-bold"></label>
                </div>
                </div>
                <p></p>
                <br />
                <label for="type">Company Type:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Type" class="form-control" id="type" name="type" />
                    <label for="type" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="subscription">Subscription:</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="subscription" onchange="updateCost()">
                      <option value="0">Bronze</option>
                      <option value="1">Silver</option>
                      <option value="2">Gold</option>
                      <option value="3">Black</option>
                    </select>
                    <label for="subscription" class="glyphicon glyphicon-ok"></label>
                </div>  
                </div>
                <label id="cost"><b>$30.00</b></label>
                 <hr/>

	            <button class="btn btn-primary btn-md" type="submit"><span class="glyphicon glyphicon-send"></span> Submit</button>
                <p><br/></p>
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
        $("#CreateUser").hide();

        function updateCost() {
            switch ($("#subscription").val()) {
                case '0':
                    $("#cost").text("$30.00");
                    break;
                case '1':
                    $("#cost").text("$50.00");
                    break;
                case '2':
                    $("#cost").text("$80.00");
                    break;
                case '3':
                    $("#cost").text("$100.00");
                    break;
            }
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
            var parameter = { Company: $("#companyName").val(), Address: $("#address").val(), Type: $("#type").val(), Subs: $("#subscription").val() };

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "Subscriptions.aspx/MakeSubscription",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === 0) window.location.href = "https://testingtests.chargify.com/subscribe/x8jcmd9x/test";
                    else {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> An error occurred while processing your request!.</div>');
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

            $.ajax({
                type: "POST",
                url: "Subscriptions.aspx/IsSubscribed",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true)
                        $("#CreateUser").show();

                    $("body").fadeIn("slow");
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        })
    </script>
</body>
</html>

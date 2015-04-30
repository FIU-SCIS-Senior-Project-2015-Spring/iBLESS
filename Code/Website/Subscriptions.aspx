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
    textarea { margin-right: 70px; }
</style>

<body style="display:none">
    <form class="form-inline">
    <div class="container">
        <p><br /></p>
		<div class="col-md-12 column" id="workspace">
        <div class="panel panel-default">
            <a href="/UserManagement.aspx" style="float:left; padding-left:1em; padding-top:1em"><img src="tractouch.jpg" alt="Smiley face" width="60" height="60" /></a>
            <div onclick="deleteCookies()"" id="logout" style="float:right; cursor:pointer; margin-top:1em; margin-right: 1em; padding-right: 20px; padding-left: 20px"> 
                <a><label id="welcome" style="font-size:smaller; color: rebeccapurple; width: auto ; cursor:pointer"></label><br />Log out</a>
            </div>
            <div class="panel-body">
			    <ul class="nav nav-tabs" style="padding-top:1.5em">
				    <li id="changeInformation">
					    <a href="/ChangeInformation.aspx">Change Information</a>
				    </li>
				    <li class="active" id="subscriptions">
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
                <br />
                <label for="companyName">Company Name</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Company" class="form-control" id="companyName" name="company" />
                    <label for="companyName" class="glyphicon glyphicon-font"></label>
                </div>
                </div>
                <label for="address">Address</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Address" class="form-control" id="address" name="address" />
                    <label for="address" class="glyphicon glyphicon-bold"></label>
                </div>
                </div>
                <p></p>
                <br />
                <label for="type">Company Type</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="Type" class="form-control" id="type" name="type" />
                    <label for="type" class="glyphicon glyphicon-briefcase"></label>
                </div>
                </div>
                <label for="subscription">Subscription</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="subscription" onchange="updateCost()">
                      <option value="Bronze">Bronze</option>
                      <option value="Silver">Silver</option>
                      <option value="Gold">Gold</option>
                      <option value="Black">Black</option>
                    </select>
                    <label for="subscription" class="glyphicon glyphicon-ok"></label>
                </div>  
                </div>
                <label id="cost"><b>$30.00</b></label>
                <p></p>
                <br />
                <label for="webLink">Website</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="http://www.example.com" class="form-control" id="webLink" name="webLink" />
                    <label for="webLink" class="glyphicon glyphicon-font"></label>
                </div>
                </div>
                <label for="splType">dBA Regulation</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <select class="form-control" id="splType">
                      <option value="OSHA">OSHA</option>
                      <option value="NIOSH">NIOSH</option>
                    </select>
                    <label for="splType" class="glyphicon glyphicon-ok"></label>
                </div>  
                </div>
                <p></p>
                <br />
                <label for="tagLine">Tagline</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <input data-msg-required="Required!" data-rule-required="true" type="text" placeholder="We are awesome!" class="form-control" id="tagLine" name="tagLine" />
                    <label for="tagLine" class="glyphicon glyphicon-font"></label>
                </div>
                </div>
                <p></p>
                <br />
                <label for="description">Company Description</label>
                <div class="form-group">
                <div class="icon-addon addon-lg">
                    <textarea style="height: auto" rows="5" data-msg-required="Required!" data-rule-required="true" placeholder="Description..." class="form-control" id="description" name="description"></textarea>
                    <label for="description" class="glyphicon glyphicon-font"></label>
                </div>
                </div>
                 <hr/>
	            <button onclick="setButtonTo('buy')" class="btn ibless-button btn-md" type="submit" id="buyButton"><span class="glyphicon glyphicon-send"></span> Submit</button>
                <button onclick="setButtonTo('update')" class="btn ibless-button btn-md" type="submit" id="updateButton"><span class="glyphicon glyphicon-send"></span> Update</button>
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
    <script src="UtilityScript.js?ver=10"></script>

    <script>
        var buttonClicked;
        var subscription;
        var chargify;

        function setButtonTo(button) {
            buttonClicked = button;
        }

        function updateCost() {
            switch ($("#subscription").val()) {
                case "Bronze":
                    $("#cost").text("$30.00");
                    break;
                case "Silver":
                    $("#cost").text("$50.00");
                    break;
                case "Gold":
                    $("#cost").text("$80.00");
                    break;
                case "Black":
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
            var parameter;

            if (buttonClicked === "buy") {
                parameter = {
                Company: $("#companyName").val(), Address: $("#address").val(), Type: $("#type").val(), SubscriptionType: $("#subscription").val(), SPL_Type: $("#splType").val(),
                Tagline: $("#tagLine").val(), Description: $("#description").val(), Weblink: $("#webLink").val(), CharID: 0
            };

                $.ajax({
                    type: "POST",
                    data: JSON.stringify(parameter),
                    url: "Subscriptions.aspx/MakeSubscription",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d === 0)
                            window.location.href = "https://testingtests.chargify.com/subscribe/x8jcmd9x/test";
                        else {
                            $("#alert").remove();
                            $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> An error occurred while processing your request!</div>');
                        }
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                })
            }
            else {
                parameter = {
                Company: $("#companyName").val(), Address: $("#address").val(), Type: $("#type").val(), SubscriptionType: $("#subscription").val(), SPL_Type: $("#splType").val(),
                Tagline: $("#tagLine").val(), Description: $("#description").val(), Weblink: $("#webLink").val(), CharID: chargify
            };
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(parameter),
                    url: "Subscriptions.aspx/MakeSubscription",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d === 0 && subscription !== $("#subscription").val())
                            window.location.href = "https://testingtests.chargify.com/subscribe/x8jcmd9x/test";
                        else if (response.d === 0) {
                            $("#alert").remove();
                            $("#workspace").prepend('<div class="alert alert-success" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> Your new information has been stored!</div>');
                        }
                        else {
                            $("#alert").remove();
                            $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> An error occurred while processing your request!</div>');
                        }
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                })
            }
        }

        $(document).ready(function () {
            eraseSession();
            GetUserName();
            setNavigationBar();
            setWelcome();
            IsSubscribed();
            $("body").fadeIn("slow");
        })

        function IsSubscribed() {
            $.ajax({
                type: "POST",
                url: "Subscriptions.aspx/IsSubscribed",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true) {
                        $("#buyButton").remove();
                        $("#updateButton").show();
                        fillInformation();
                    }
                    else
                        $("#updateButton").hide();
                },
                failure: function (response) {
                    alert(response.d);
                },
                async: false
            })
        }

        function fillInformation() {
            var information;

            $.ajax({
                type: "POST",
                url: "Subscriptions.aspx/GetInformationAdmin",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    information = JSON.parse(response.d);
                },
                failure: function (response) {
                    alert(response.d);
                },
                async: false
            })

            $("#companyName").val(information.Name);
            $("#address").val(information.Address);
            $("#type").val(information.Type);
            $("#subscription").val(information.Subscription);
            $("#splType").val(information.SPL_Type);
            $("#tagLine").val(information.Tagline);
            $("#description").val(information.Description);
            $("#webLink").val(information.Website);
            subscription = information.Subscription;
            chargify = information.Chargify;
            updateCost();
        }
    </script>
</body>
</html>

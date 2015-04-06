<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="WebApplication1.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>

<style>
  form { padding: 10px; }
  .error { border: 1px solid #b94a48!important; background-color: #fee!important; }
</style>

<body style="background:#eee;opacity:0">
    
    <div class="container">
    		<p><br/></p>
  		<div class="row">
            <div class="col-md-4"></div>
  			<div class="col-md-4" id ="workspace">
  				<div class="panel panel-default" id="panel">
  					<div class="panel-body">
                            
    						<div class="page-header">
  							<h3>Forgot Password</h3>
						    </div>
                            <form>
  							<div class="form-group">
                                <label for="newPass">New Password:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-envelope"></span></span>
                                    <input 
                                      data-msg-required="This field is required!" 
                                      data-rule-required="true"
                                      id="newPass" name="newPass" type="password" placeholder="Enter New Password" class="form-control"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="newPassCon">New Password Confirmation:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-envelope"></span></span>
                                    <input 
                                      data-msg-required="This field is required!" 
                                      data-rule-required="true"
                                      id="newPassCon" name="newPassCon" type="password" placeholder="Enter New Password" class="form-control"/>
                                </div>
                            </div>
  							<hr/>
  							<button class="btn btn-info" type="submit" ><span class="glyphicon glyphicon-lock"></span> Recover Password</button>
  							<p><br/></p>
                          </form>
  					</div>
                    <div class="col-md-4"></div>
				</div>
  			</div>
		</div>
    </div>
</body>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery.validate/1.11.1/jquery.validate.js" type="text/javascript"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>
    <script src="UtilityScript.js?ver=2"></script>
    

    <script>
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

        $(document).ready(function ()
        {
            var parameter = { Email: getURLParameter("Email"), Code: getURLParameter("Code") }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "ChangePassword.aspx/ValidateCode",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true)
                        $("body").css("opacity", 1);
                    else
                    {
                        $("body").css("opacity", 1);
                        $("#panel").hide();
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> Invalid information!</div>');
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        })

        function submit() {

            if ($("#newPass").val() !== $("#newPassCon").val())
            {
                $("#newPass").tooltip("destroy").data("title", "Password must be equal!").addClass("error").tooltip();
                $("#newPassCon").tooltip("destroy").data("title", "Password must be equal!").addClass("error").tooltip();

                return;
            }

            var parameter = { Email: getURLParameter("Email"), Password: $("#newPass").val() }

            $.ajax({
                type: "POST",
                data: JSON.stringify(parameter),
                url: "ChangePassword.aspx/ChangeUserPassword",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === true) {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-success" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> Password successfully changed! Going back to login...</div>');
                        setTimeout(function () { window.location.href = '/Login.aspx'; }, 5000);
                    }
                    else {
                        $("#alert").remove();
                        $("#workspace").prepend('<div class="alert alert-danger alert-error" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Error!</strong> A problem has occurred while submitting your data.</div>');
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            })
        }
    </script>
</html>

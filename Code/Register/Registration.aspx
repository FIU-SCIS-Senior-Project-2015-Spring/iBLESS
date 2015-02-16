<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="WebApplication1.Registration" %>

<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Member Login</title>

    <!-- Bootstrap -->
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

  <body style="background:#eee">
    
    <div class="container">
    		<p><br/></p>
  		<div class="row">
            <div class="col-md-4"></div>
  			<div class="col-md-4">
  				<div class="panel panel-default">
  					<div class="panel-body">
    						<div class="page-header">
  							<h3>Registration</h3>
						</div>
                        <form>
                        <div class="form-group">
                            <label for="userName">Username</label>
                            <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                            <input 
                              data-msg-required="The Username field is required!" 
	                          data-rule-required="true" 
                              id="userName" name="userName" type="text" placeholder="Enter Username" class="form-control"/>
                                </div>
                        </div>
	
                        <div class="form-group">
                            <label for="password">Password</label>
                            <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-lock"></span></span>
                            <input 
                              data-msg-required="The Password field is required!" 
	                          data-rule-required="true" 
                              id="password" name="password" type="password" placeholder="Enter Password" class="form-control"/>
                                </div>
                        </div>

                        <div class="form-group">
                            <label for="PassConfirm">Password Confirmation</label>
                            <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-lock"></span></span>
                            <input 
                              data-msg-required="The Password confirmation field is required!" 
	                          data-rule-required="true" 
                              id="PassConfirm" name="PassConfirm" type="password" placeholder="Enter Password" class="form-control"/>
                                </div>
                        </div>

                        <div class="form-group">
                            <label for="first">First Name</label>
                            <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-font"></span></span>
                            <input 
                              data-msg-required="The First Name field is required!" 
	                          data-rule-required="true" 
                              id="first" name="first" type="text" placeholder="Enter First Name" class="form-control"/>
                                </div>
                        </div>

                        <div class="form-group">
                            <label for="last">Last Name</label>
                            <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-bold"></span></span>
                            <input 
                              data-msg-required="The Last Name field is required!" 
	                          data-rule-required="true" 
                              id="last" name="last" type="text" placeholder="Enter Last Name" class="form-control"/>
                                </div>
                        </div>

                        <div class="form-group">
                            <label for="email">E-mail</label>
                            <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-envelope"></span></span>
                            <input 
                              data-msg-required="The E-mail field is required!" 
	                          data-rule-email="true" 
                              data-rule-required="true"
                              id="email" name="email" type="email" placeholder="Enter E-mail" class="form-control"/>
                            </div>
                        </div>
	                    <hr/>

	                    <button class="btn btn-primary" type="submit"><span class="glyphicon glyphicon-lock"></span> Register</button>
                        <p><br/></p>
                      </form>
  					</div>
                    <div class="col-md-4"></div>
				</div>
  			</div>
		</div>
    </div>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="//ajax.aspnetcdn.com/ajax/jQuery.validate/1.11.1/jquery.validate.js" type="text/javascript"></script>
    <script src="//netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"></script>

    <script>
        var hasErrors = false;

        function submit()
        {
            if (hasErrors)
                return;

            var parameter = { Email: $("#email").val(), Password: $("#password").val(), UserName: $("#userName").val(), FirstName: $("#first").val(), LastName: $("#last").val() }

             $.ajax({
                 type: "POST",
                 data: JSON.stringify(parameter),
                 url: "Registration.aspx/Register",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (response) {
                     if (response.d === false)
                         alert("Error creating account!");
                     else
                         window.location.href = '/Login.aspx';
                 },
                 failure: function (response) {
                     alert(response.d);
                 }
             })
        }

        function passwordsDifferent()
        {
            if ($("#password").val() !== $("#PassConfirm").val())
                return true;

            return false;
        }

        $("form").validate({

            showErrors: function (errorMap, errorList) {
                hasErrors = false;

                $("#password").removeClass("error").tooltip("destroy");
                $("#PassConfirm").removeClass("error").tooltip("destroy");

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

                if (passwordsDifferent()) {
                    $("#password").tooltip("destroy").data("title", "Passwords are not equal!").addClass("error").tooltip();
                    $("#PassConfirm").tooltip("destroy").data("title", "Passwords are not equal!").addClass("error").tooltip();

                    hasErrors = true;
                }

            },

            submitHandler: function (form) {
                  submit();
              }
          });
    </script>

    <script type="text/javascript">
        $(function () {
            $(".tip").tooltip();
        });
    </script>
  </body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgot.aspx.cs" Inherits="WebApplication1.Forgot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet">
    <link href="ChangeInfoCSS.css" rel="stylesheet" type="text/css" />

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

<body>
    
    <div class="container">
    		<p><br/></p>
  		<div class="row">
            <div class="col-md-4"></div>
  			<div class="col-md-4" id ="workspace">
  				<div class="panel panel-default">
  					<div class="panel-body">
                            
    						<div class="page-header">
  							<h3>Forgot Password</h3>
						    </div>
                            <p><b>An e-mail will be sent to your e-mail address with instructions on how to recover your password.</b></p>
                            <form>
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
  							<button class="btn ibless-button" type="submit" ><span class="glyphicon glyphicon-lock"></span> Recover Password</button>
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

    function submit() {
        var parameter = { Email: $("#email").val() }

        $.ajax({
            type: "POST",
            data: JSON.stringify(parameter),
            url: "Forgot.aspx/SendMail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d === true){
                    $("#alert").remove();
                    $("#workspace").prepend('<div class="alert alert-success" id="alert"><a class="close" data-dismiss="alert">&times;</a><strong>Success!</strong> E-mail successfully sent with instructions on how to recover your password! Going back to login...</div>');
                    setTimeout(function () { window.location.href = '/Login.aspx'; }, 5000);
                }  
                else
                {
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

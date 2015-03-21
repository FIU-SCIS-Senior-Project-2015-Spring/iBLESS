<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

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
      body { display:none !important; }
  </style>

  <body style="background:#eee">
    <form>
    <div class="container">
    		<p><br/></p>
  		<div class="row">
  			<div class="col-md-4">
  				<div class="panel panel-default">
  					<div class="panel-body">
    						<div class="page-header">
  							<h3>Login</h3>
						    </div>
  							<div class="form-group">
    								<label for="exampleInputEmail1">Username</label>
    								<div class="input-group">
  									<span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
  									<input type="text" class="form-control" id="email" placeholder="Enter username">
								</div>
  							</div>
  							<div class="form-group">
    								<label for="exampleInputPassword1">Password</label>
    								<div class="input-group">
  									<span class="input-group-addon"><span class="glyphicon glyphicon glyphicon-lock"></span></span>
  									<input type="password" class="form-control" id="password" placeholder="Password">
								</div>
  							</div>
  							<hr/>
                            <p id="InfoError" style="color:red" hidden></p>
                            <a href="/Registration.aspx">Not a user?</a><a href="Forgot.aspx"> Forgot your password?</a>
                            <br />
                            <br />
  							<button type="submit" class="btn btn-primary"><span class="glyphicon glyphicon-lock"></span> Login</button>
  							<p><br/></p>
  					</div>
				</div>
  			</div>
			<div class="col-md-8"></div>
		</div>
    </div>
    </form>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>

    <script>
        $("form").submit(function () { validate(); return false; })

        function validate ()
        {
            if ($("#email").val() != "" && $("#password").val() != "")
            {
                $("#InfoError").hide();
                submit();
            }
            else
            {
                $("#InfoError").text("Please fill all the information!");
                $("#InfoError").show();
            }
        }

        function submit()
        {
            var parameter = { Email: $("#email").val(), Password: $("#password").val() }

             $.ajax({
                 type: "POST",
                 data: JSON.stringify(parameter),
                 url: "Login.aspx/Validate",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (response) {
                     if (response.d === 0)
                         window.location.href = '/UserManagement.aspx';
                     else if (response.d === 1)
                     {
                         $("#InfoError").text("Wrong Username or Password!");
                         $("#InfoError").show();
                     }
                     else
                     {
                         $("#InfoError").text("User is inactive!");
                         $("#InfoError").show();
                     }
                 },
                 failure: function (response) {
                     alert(response.d);
                 }
             })
        }

        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1);
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }

        function GetUserName(fadeIn) {
            var username = getCookie("MyTestCookie");

            if (username !== "") {
                window.location.href = '/UserManagement.aspx';
                return false;
            }

            return true;
        }

        $(document).ready(function () {
            if (GetUserName())
                $('body').attr('style', 'display: block !important; opacity: 0; background: #eee;').animate({ opacity: 1 });
        })
    </script>
  </body>
</html>

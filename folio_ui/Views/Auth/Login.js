/*
 * Auth - Login View
 * JS
*/

import "./Login.css";
import Auth from "../Auth.js";

// hide login messageon startup
$("#login-message").hide();

// trigger form submit on button click 
$("#login-btn-submit").click(() => {
    $("#login.form").submit();
});

// submit the login form to perform login 
$("#login-form").submit(async (event) => {
    event.preventDefault();

    // collect login credientials from form
    const emailAddr = $("#login-EmailAddr").val();
    const password = $("#login-Password").val();

    // perform login
    const auth = new Auth("http://" + process.env.API_INGRESS);
    if(await auth.login(emailAddr, password) == false){
        // login was not successful
        $("#login-message").show();
        $("#login-message").text("Wrong email or password.");
    }
});

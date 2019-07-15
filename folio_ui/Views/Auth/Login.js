/*
 * Auth - Login View
 * JS
*/

import "./Login.css";
import Auth from "../Auth.js";

// trigger form submit on button click 
$("#login-btn-submit").click(() => {
    $("#login.form").submit();
});

// submit the login form to perform login 
$("#login-form").submit((event) => {
    event.preventDefault();

    // collect login credientials from form
    const emailAddr = $("#login-EmailAddr").val();
    const password = $("#login-Password").val();

    // perform login
    const auth = new Auth("http://" + process.env.API_INGRESS);
    auth.login(emailAddr, password);
});

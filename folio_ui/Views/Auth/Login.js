/*
 * API - Login View
 * JS
*/

import "./Login.css";
import API from "../API.js";

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
    const api = new API();
    if(await api.login(emailAddr, password) == false){
        // login was not successful
        $("#login-message").show();
        $("#login-message").text("Wrong email or password.");
        return;
    } 

    // obtain info about the user being authenticated
    const userinfo = await api.getUser();
    console.log("logged in as:", userinfo);
    console.log(userinfo);
    //TODO: go somewhere after being logggein
});

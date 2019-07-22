/*
 * Student Navbar JS
 * NP Web Assignment 
*/

import API from "../../API.js"

// perform logout on click logout button
$("#btn-logout").click(() => {
    const api = new API();
    api.logout();

    // redirect to homepage
    window.location.href = "/";
});

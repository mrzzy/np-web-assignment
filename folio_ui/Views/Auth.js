/*
 * Authentication
 * JS
*/

import * as Cookies from "js-cookie";
import fetch from 'cross-fetch';

// defines client side authentication functionality
// for working with Auth API
export default class Auth {
    /* Contructs a new auth instance that talks to the given API endpoint
     * Attempts to read state in document.cookie 
    */
    constructor(endpoint) {
        this.endpoint = endpoint;
        
        // load session token from cookie
        this.token = null;
        const gotToken = Cookies.get("Auth.token");
        if(gotToken != null) this.token = gotToken;
    }

    /* Perform API login with the given email and password asyncronously 
     * using the given API's login function
     * Saves the session token
     * Returns true if login is successful otherwise false
     */
    async login(email, password) {
        // build credentials object with provided credentials
        const credentials = {
            EmailAddr: email,
            Password: password
        };

        // call auth login api with provided credentials
        const response = await fetch(this.endpoint + "/api/auth/login", {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            headers : {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials) // convert creds to json
        });
        // check if authentication with credientials successful
        if(response.status != 200) return false;

        // parse reply from response
        const reply = await response.json();
        this.token = reply.sessionToken;

        // save token for future object authentications
        Cookies.set("Auth.token", this.token);

        return true;
    }

    /* Checks if authenticationed using API's check function asyncronously 
     * Returns  if authentication check is successful otherwise false 
    */
    async check() {
        // call auth check api with session token
        const response = await fetch(this.endpoint + "/api/auth/check", {
            method: "GET",
            mode: "cors",
            cache: "no-cache",
            headers : {
                "Authorization": "Bearer " + this.token
            },
        });
        
        // check if authentication with credientials successful
        if(response.status == 401) return false;
        else if(response.status == 200) return true;
        else throw "Failed to check session token with API /api/auth/check";
    }
}

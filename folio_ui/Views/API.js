/*
 * API client
 * JS
*/

import * as Cookies from "js-cookie";
import fetch from 'cross-fetch';

// defines API client for talking to the api
export default class API {
    // constant api status codes
    static get status() {
        return {
            success: 200,
            badRequest: 400,
            unauthorized: 401,
            notFound: 404,
            conflict: 409,
            serverError: 500
        }
    }
    
    /* Contructs a new auth instance that talks to the given API endpoint
     * Attempts to read state in document.cookie 
    */
    constructor(endpoint=null) {
        this.endpoint = "http://"  + ( 
            (endpoint == null) ? process.env.API_ENDPOINT : endpoint
        );
        
        // load session token from cookie
        this.token = null;
        const gotToken = Cookies.get(process.env.API_TOKEN_KEY);
        if(gotToken != null) this.token = gotToken;
    }

    /* API calls */
    /* Bless the given fetch API request object with the authentication 
     * token, setting its authentication bearer token
     * Use this if the API request you are making requires authentication
     * Returns the blessed request
    */
    bless(request) {
        // create headers if does not already exist
        if(request.headers == null) request.headers = {}
        // assign authentication bearer token
        request.headers["Authorization"] =  "Bearer " + this.token;
        return request;
    }

    /* perform an API call using the given http method on the given
     * API call route, optionally, passing http headers, content body
     * Automatically attaches auth token as bearer token if present
     * Throws exception when API call givens http status that is not success
     * Returns the response with response.status as the status code
     * response.content as the content body of the request
    */
    async call(method, route, headers={}, content=null) {
        // build the request
        var request = {}
        request.headers = headers;
        this.bless(request); // set auth header
        request.method = method;
        request.mode = "cors";
        if(content != null) request.body = content;

        // make the API call
        const fetchResponse = await fetch(this.endpoint + route, request);

        // process fetch response to create response object
        const response = {
            status: fetchResponse.status,
            content: await fetchResponse.text()
        };
        
        return response;
    }

    /* Authorization & Authentication */
    /* Checks if authenticationed using API's check function asyncronously 
     * Returns if authentication check is successful otherwise false 
    */
    async check() {
        if(this.token == null) return false; // no token means not authenticated
        // call auth check api with session token
        var request = {
            method: "GET",
            mode: "cors",
            cache: "no-cache"
        };
        this.bless(request);
        const response = await fetch(this.endpoint + "/api/auth/check", request);
        
        // check if authentication with credientials successful
        if(response.status == API.status.unauthorized) return false;
        else if(response.status == API.status.success) return true;
        else throw "Failed to check session token with API /api/auth/check";
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
        if(response.status != API.status.success) return false;

        // parse reply from response
        const reply = await response.json();
        this.token = reply.sessionToken;

        // save token for future object authentications
        Cookies.set(process.env.API_TOKEN_KEY, this.token);

        return true;
    }

    /* Perform logout of the currently authenticated user 
     * If not already authenticated, would do nothing
    */
    async logout(){
        // clear bearer auth tokens to reset to state before login
        this.token = null;
        Cookies.remove(process.env.API_TOKEN_KEY);
    }

    /* Checks if authenticationed using API's check function asyncronously 
     * Returns if authentication check is successful otherwise false 
    */
    async check() {
        if(this.token == null) return false; // no token means not authenticated
        // call auth check api with session token
        var request = {
            method: "GET",
            mode: "cors",
            cache: "no-cache"
        };
        this.bless(request);
        const response = await fetch(this.endpoint + "/api/auth/check", request);
        
        // check if authentication with credientials successful
        if(response.status == API.status.unauthorized) return false;
        else if(response.status == API.status.success) return true;
        else throw "Failed to check session token with API /api/auth/check";
    }

    /* Utility methods */
    /* Obtain user info of the user currently authenticated by the API asyncronously
     * Returns the user info of the user currently authenticated,
     * throws an exception if not authenticated at all
    */
    async getUser() {
        // call auth info  api with session token
        const response = await this.call("GET", "/api/auth/info");
        // parse response
        const userinfo = JSON.parse(response.content);
        return userinfo;
    }
}

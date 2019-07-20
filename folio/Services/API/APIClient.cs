/*
 * NP Web Assignment
 * Services - APIClient
*/

using System;
using DotNetEnv;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using folio.Models;
using Newtonsoft.Json;

namespace folio.Services.API
{
    public class APIResponse {
        public int StatusCode { get; set; } // status code of the response
        public string Content { get; set; } // content of the response if any
    }

    // defines an exception that occurs when doing an api call
    public class APIClientCallException : Exception 
    {
        public APIClientCallException(string message)
            : base(message) { }
    }

    // Defines a API client for requestin data from the api
    public class APIClient
    {
        /* constructor */
        public string APIService { get; } // access api from internal network
        public string APIEndpoint { get; } // access api from external network
        private string AuthToken { get; set; }
        private static HttpClient Client = new HttpClient();

        /* constructor */
        // construct a new API client that talks to the given api service. 
        // If provided, use the given token for authentication when making calls
        // If service is null, attempts to obtain service from environment 
        // variable API_SERVICE
        public APIClient(string token=null, string service=null)
        {
            this.APIService = "http://" + ((service == null) ?  
                    Environment.GetEnvironmentVariable("API_SERVICE") : service);
            this.APIEndpoint = 
                "http://" + Environment.GetEnvironmentVariable("API_ENDPOINT");
            this.AuthToken = token;
        }
        
        // construct a new API client that talks to the given api service. 
        // Attempts to extract the authentication token from the http context if given
        // If service is null, attempts to obtain service from environment 
        // variable API_SERVICE
        public APIClient(HttpContext context, string service=null) 
            : this(APIClient.LoadToken(context), service) { }
        
        /* API calls */
        // make an API call specified by the given call route using the given http method
        // Includes the content as the request body
        // Attaches an authentication token if APIClient has authentication token
        // throws and APIClientCallException if the call fail
        // Returns the response as an APIResponse
        public APIResponse CallAPI(string method, string callRoute, HttpContent content=null)
        {
            // construct the request
            HttpRequestMessage request = new HttpRequestMessage {
                Method = new HttpMethod(method),
                RequestUri = new Uri(this.APIService + callRoute)
            };

            // configure headers 
            // - add authorization token if required
            if(this.AuthToken != null)
            { 
                request.Headers.Authorization = 
                    new AuthenticationHeaderValue("Bearer", this.AuthToken);
            }

            // add request content if provided
            if(content != null) request.Content = content;

            // perform api call and capture response
            HttpResponseMessage httpResponse = APIClient.Client.SendAsync(request).Result;
            APIResponse response = new APIResponse
            {
                StatusCode = (int) httpResponse.StatusCode,
                Content = httpResponse.Content.ReadAsStringAsync().Result
            };
            
            return response;
        }
        
        // get the user info of the user that owns this api clients's api token
        // returns the user info or null if token is invalid or not present
        public UserInfo GetUserInfo() 
        {
            if(this.AuthToken == null) return null;
            // pull user infomation from using api
            APIResponse response = this.CallAPI("GET", "/api/auth/info");
            // check if token is valid
            if(response.StatusCode == 401) return null;
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(response.Content);
            return userInfo;
        }
        
        /* private utilities */
        // extracts the API authentication token from the givnn http context
        // returns the extracted token or null if no token could be extracted
        private static string LoadToken(HttpContext context)
        {
            // check token present to extract
            string authTokenKey = Environment.GetEnvironmentVariable("API_TOKEN_KEY");
            string authToken = context.Request.Cookies[authTokenKey];
            if(context == null || authToken == null)
            {
                return null;
            }
            
            return authToken;
        }
    }
}

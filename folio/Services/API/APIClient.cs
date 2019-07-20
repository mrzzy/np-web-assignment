/*
 * NP Web Assignment
 * Services - APIClient
*/

using System;
using DotNetEnv;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Collections.Generic;

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
        public string APIEndpoint { get; }
        private string AuthToken { get; set; }
        private static HttpClient Client = new HttpClient();

        /* constructor */
        // construct a new API client that talks to the given endpoint. 
        // If endpoint is null, attempts to obtain endpoint from environment 
        // variable API_SERVICE
        // If provided, will use authentication token to authenticate requests
        public APIClient(string token=null, string endpoint=null)
        {
            this.APIEndpoint = "http://" + ((endpoint == null) ?  
                    Environment.GetEnvironmentVariable("API_SERVICE") : endpoint);
            this.AuthToken = token;
        }
        
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
                RequestUri = new Uri(this.APIEndpoint + callRoute)
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
        
    }
}

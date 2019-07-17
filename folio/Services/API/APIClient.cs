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
        // variable API_HOST
        // If provided, will use authentication token to authenticate requests
        public APIClient(string token=null, string endpoint=null)
        {
            this.APIEndpoint = "http://" + ((endpoint == null) ?  
                    Environment.GetEnvironmentVariable("API_HOST") : endpoint);
            this.AuthToken = token;
        }
        
        // make an API call specified by the given call route using the given
        // the given content if provided with the given http method
        // Attaches an authentication token if APIClient has authentication token
        // throws and APIClientCallException if the call fail
        // Returns the response of the call as a string
        public string CallAPI(string method, string callRoute, string content=null)
        {
            // construct the request
            HttpRequestMessage request = new HttpRequestMessage {
                Method = new HttpMethod(method),
                RequestUri = new Uri(this.APIEndpoint + callRoute)
            };

            // configure headers - add authorization token if required
            if(this.AuthToken != null)
            { 
                request.Headers.Authorization = 
                    new AuthenticationHeaderValue("Bearer", this.AuthToken);
            }

            // add request content 
            if(content != null)  request.Content = new StringContent(content);
        

            // perform api call and capture response
            HttpResponseMessage response = APIClient.Client.SendAsync(request).Result;
            if(response.StatusCode != HttpStatusCode.OK)
            {
                throw new APIClientCallException(
                    "API call failed: got status code: " 
                    + response.StatusCode 
                    + " got response: " 
                    + response.Content.ToString());
            }

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}

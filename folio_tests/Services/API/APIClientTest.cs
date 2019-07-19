using Xunit;
using System;
using DotNetEnv;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Collections.Generic;
using folio.Services.API;
using Newtonsoft.Json;

namespace folio.Tests.Services.API
{
    public class APIClientTests
    {
        /* API Client intergration tests */
        [Fact]
        public void TestCallAPI() 
        {
            // load environment
            DotNetEnv.Env.Load();
            APIClient apiClient = new APIClient();
            
            // make get students api call
            APIResponse response = apiClient.CallAPI("GET", "/api/students");
            Assert.Equal(200, response.StatusCode);
            Console.WriteLine("Test APIClient: TestCallAPI: got response for students: " 
                    + response.Content);
            
            // make login api call
            string credientialsJson = 
                "{ 'EmailAddr': 's1234112@ap.edu.sg', 'Password': 'p@55Student' }";
            StringContent content = new StringContent(
                    credientialsJson, Encoding.UTF8, "application/json");

            response = apiClient.CallAPI("POST", "/api/auth/login", 
                    content);
            Assert.Equal(200, response.StatusCode);
            Console.WriteLine("Test APIClient: TestCallAPI: got response for login: " 
                    + response.Content);
        }
    }
}

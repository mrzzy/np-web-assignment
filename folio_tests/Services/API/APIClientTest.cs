using Xunit;
using System;
using DotNetEnv;
using System.Net;
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
            string content = apiClient.CallAPI("GET", "/api/students");
            Console.WriteLine("TestCallAPI: got response: " + content);
        }
    }
}

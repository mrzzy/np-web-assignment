using Xunit;
using System;
using DotNetEnv;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Collections.Generic;


namespace folio.Tests.API
{
    public class APITest
    {
        /* API intergration tests */
        [Fact]
        public async void TestAccessAPI()
        {
            // load api host form host
            DotNetEnv.Env.Load();
            string apiHost = Environment.GetEnvironmentVariable("API_HOST");
            
            // Attempt to access the api at the /api/students route
            string apiUrl = apiHost + "/api/students";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client
                .GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
        }
    }
}

/*
 * Google Cloud Storage Content Service Test
 * Web Assignment 1
*/

using Xunit;
using System;
using System.IO;
using Google.Cloud.Storage.V1;
using DotNetEnv;

using folio.Services.Content;

namespace folio.Tests.Services.Content
{
    public class GCSContentServiceTest
    {
        [Fact]
        public void TestGCSContentSeviceConstructor()
        {
            DotNetEnv.Env.Load();
            GCSContentService contentService = new GCSContentService();
        }
    }
}

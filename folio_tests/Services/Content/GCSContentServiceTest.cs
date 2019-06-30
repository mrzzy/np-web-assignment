/*
 * Google Cloud Storage Content Service Test
 * Web Assignment 1
*/

using Xunit;
using System;
using System.IO;
using System.Text;
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
        
        [Fact]
        public void TestGCSContentSeviceUploadContent()
        {
            DotNetEnv.Env.Load();
            GCSContentService contentService = new GCSContentService();
            
            // create content stream to upload to gcp
            string content = "test content: upload me to google";
            Byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            MemoryStream contentStream = new MemoryStream(contentBytes);
        
            // perform upload of test content stream
            string contentId = contentService.UploadContent(contentStream, "text/plain");
            
            Assert.False(string.IsNullOrWhiteSpace(contentId),
                        "content id returned from upload is null or empty");
        
            Console.WriteLine("GCSContentServiceTest: " +
                    " TestGCSContentSeviceUploadContent(): " +
                    " uploaded test content to: "  +
                    contentService.EncodeUrl(contentId));
        }
    }
}

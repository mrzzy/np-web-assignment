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
        public void TestGCSContentSeviceInsert()
        {
            DotNetEnv.Env.Load();
            IContentService contentService = new GCSContentService();
            
            string contentId = contentService.Insert(
                    this.CreateTestContentStream(), "text/plain");
            
            Assert.False(string.IsNullOrWhiteSpace(contentId),
                        "content id returned from upload is null or empty");
        
            Console.WriteLine("GCSContentServiceTest: " +
                    " TestGCSContentSeviceInsert(): " +
                    " uploaded test content to: "  +
                    contentService.EncodeUrl(contentId));

            // clean up
            contentService.Delete(contentId);
        }

        [Fact]
        public void TestGCSContentSeviceUpdate()
        {
            DotNetEnv.Env.Load();
            IContentService contentService = new GCSContentService();

            // insert content
            string contentId = contentService.Insert(
                    this.CreateTestContentStream());

            // update content
            contentId = contentService.Update(
                    contentId, this.CreateTestContentStream());
            
            Assert.False(string.IsNullOrWhiteSpace(contentId),
                        "content id returned from upload is null or empty");
        
            Console.WriteLine("GCSContentServiceTest: " +
                    " TestGCSContentSeviceUpdate(): " +
                    " uploaded test content to: "  +
                    contentService.EncodeUrl(contentId));

            // clean up
            contentService.Delete(contentId);
        }
        
        [Fact]
        public void TestGCSContentSeviceDelete()
        {
            DotNetEnv.Env.Load();
            IContentService contentService = new GCSContentService();

            // insert content
            string contentId = contentService.Insert(
                    this.CreateTestContentStream(), "text/plain");

            // delete content
            contentService.Delete(contentId);
            
            bool hasDeleted = false;
            try
            {
                contentService.EncodeUrl(contentId);
            }
            catch
            {
                hasDeleted = true;
            }

            Assert.True(hasDeleted, "Delete() did not delete object");
        }
        
        /* private utilities */
        // create a test in memory stream 
        private Stream CreateTestContentStream()
        {
            // create content stream to upload to gcp
            string content = "test content: upload me to google";
            Byte[] contentBytes = Encoding.ASCII.GetBytes(content);
            MemoryStream contentStream = new MemoryStream(contentBytes);

            return contentStream;
        }
    }
}


/*
 * Google Cloud Storage Content Service
 * Content Service that uses GCS as backend
 * Web Assignment 1
*/
using System;
using System.IO;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;

namespace folio.Services.Content
{
    public class GCSContentService: IContentService
    {
        private StorageClient storage;
        private string bucketName;

        /* constructor */
        public GCSContentService()
        {
            this.SetupClient();
            
            // read storage bucket name from the environment
            this.bucketName = Environment.GetEnvironmentVariable("GCS_BUCKET");
            if(string.IsNullOrWhiteSpace(this.bucketName))
            {
                throw new ArgumentNullException("GCS_BUCKET environment variable" +
                    " with GCS bucket name not set");
            }
        }
    
        /* IContentService interface */
        // Upload the content in the given contentStream to the GCS
        // returns a content id string which can be used to retrieve the content 
        // Optionally provide a MIME content type 
        public string UploadContent(Stream contentStream, string contentType=null)
        {
            // generate id for the new content
            string contentId = Guid.NewGuid().ToString();
            // perform the upload using GCS
            this.storage.UploadObject(
                    this.bucketName, contentId, contentType, contentStream);
            
            return contentId;
        }

        // Encode the given content id in to a url so that can be used to 
        // retrieve the content given the content id returned by UploadContent()
        public string EncodeUrl(string contentId)
        {
            var storageObject = this.storage.GetObject(this.bucketName, contentId);
            return storageObject.MediaLink;
        }
        
        /* private utilities */
        // Setups storage client by authenticating with Google Cloud
        private void SetupClient ()
        {
            // write GCP secret in temporary file
            string secretPath = Path.GetTempFileName();
            string secret = Environment.GetEnvironmentVariable("GCP_SECRET");
            // check if secret can be obtained from environment
            if(string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentNullException(
                        "GCP_SECRET environment variable not set with requried" +
                        " service account credientials");
            }
            File.WriteAllText(secretPath, secret);
        
            // load storage client using secret
            this.storage = StorageClient.Create(
                GoogleCredential.FromFile(secretPath));
            
            // delete secret file
            File.Delete(secretPath);
        }
    }
}

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
        private StorageClient client;
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
        public string UploadContent(Stream contentStream)
        {
            return "";
        }

        // Decode the url that can be used to retrieve the content given the 
        // content id returned by UploadContent()
        public string decodeUrl(string contentId)
        {
            return "";
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
            this.client = StorageClient.Create(
                GoogleCredential.FromFile(secretPath));
            
            // delete secret file
            File.Delete(secretPath);
        }
    }
}

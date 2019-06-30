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
    public class GCSContentService
    {
        StorageClient client;

        // Setups storage client by authenticating with Google Cloud
        public GCSContentService()
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

/*
 * Google Cloud Storage Content Service
 * Content Service that uses GCS as backend
 * Web Assignment 1
*/
using System;
using System.IO;
using System.Text.RegularExpressions;

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
        // Insert the content in the given contentStream to the content service
        // returns a content id string which can be used to retrieve the content 
        // Optionally provide a MIME content type 
        public string Insert(Stream contentStream, string contentType = null)
        {
            // generate id for the new content
            string contentId = Guid.NewGuid().ToString();
            // perform the upload using GCS
            this.storage.UploadObject(
                    this.bucketName, contentId, contentType, contentStream);
            
            return contentId;
        }
        
        // Updates the content in the given contentStream to the content service
        // specified by contentId
        // returns a content id string which can be used to retrieve the content 
        // Optionally provide a MIME content type 
        public string Update(string contentId, Stream contentStream)
        {
            // perform the upload of updated file using GCS
            var storageObject = this.storage.GetObject(this.bucketName, contentId);
            // remove existing object
            this.Delete(contentId);
            // update object
            this.storage.UploadObject(
                    this.bucketName, contentId,
                    storageObject.ContentType, contentStream);

            return contentId;
        }


        // deletes the content specified by content id from the contentService
        public void Delete(string contentId)
        {
            // peform removal of object using GCS
            this.storage.DeleteObject(this.bucketName, contentId);
        }

        // Encode the given content id in to a url so that can be used to 
        // retrieve the content given the content id returned by UploadContent()
        public string EncodeUrl(string contentId)
        {
            var storageObject = this.storage.GetObject(this.bucketName, contentId);
            return storageObject.MediaLink;
        }
        
        // Decode the content id from the given url (ie from EncodeUrl)
        public string DecodeContentId(string url)
        {
            // use regular expressions to extract content id
            Regex regex = new Regex(@"\/o\/([0-9a-z-]+)\?");
            Match match = regex.Match(url);
            
            string contentId = match.Groups[0].Value;
            return contentId;
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

/*
 * Google Cloud Storage Content Service
 * Content Service that uses GCS as backend
 * Web Assignment 1
*/
using System;
using System.Linq;
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
        // contentType provides a MIME content type & a prefix to the path
        // Optionally a directory prefix to the object being inserted
        public string Insert(Stream contentStream, string contentType,
                string prefix="")
        {
            // generate object nae  for the new content
            string contentId = Guid.NewGuid().ToString();
            string objectName = GCSContentService.BuildObjectName(prefix, contentId);
            
            // perform the upload using GCS
            this.storage.UploadObject(
                    this.bucketName, objectName, contentType, contentStream);
            
            return contentId;
        }
        
        // Updates the content in the given contentStream to the content service
        // specified by contentId
        // returns a content id string which can be used to retrieve the content 
        // Optionally a directory prefix to the object being inserted
        public string Update(string contentId, Stream contentStream, string prefix="")
        {
            // perform the upload of updated file using GCS
            string objectName = GCSContentService.BuildObjectName(prefix, contentId);
            var storageObject = this.storage.GetObject(this.bucketName, objectName);
            // remove existing object
            this.Delete(contentId, prefix);
            // update object
            this.storage.UploadObject(
                    this.bucketName, objectName,
                    storageObject.ContentType, contentStream);

            return contentId;
        }

        // deletes the content specified by content id from the contentService
        // Optionally a directory prefix to the object being inserted
        public void Delete(string contentId, string prefix="")
        {
            // peform removal of object using GCS
            string objectName = GCSContentService.BuildObjectName(prefix, contentId);
            this.storage.DeleteObject(this.bucketName, objectName);
        }

        // Encode the given content id in to a url so that can be used to 
        // retrieve the content given the content id returned by 
        // Insert()/update()
        // Optionally a directory prefix to the object being inserted
        public string EncodeUrl(string contentId, string prefix="")
        {
            // get media link using GCS
            string objectName = GCSContentService.BuildObjectName(prefix, contentId);
            var storageObject = this.storage.GetObject(this.bucketName, objectName);
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

        // prepare and standardise the given prefix
        private static string BuildObjectName(string prefix, string contentId)
        {
            // check if prefix has whitespace
            if(prefix.Any((c) => Char.IsWhiteSpace(c)))
            {
                throw new ArgumentNullException("Prefix cannnot contain whitespace");
            }
                
            // parse prefix & standardise prefix
            if(string.IsNullOrWhiteSpace(prefix)) prefix = "";
            else prefix = (prefix.EndsWith("/")) ? prefix : prefix + "/";

            // construct content id from content name
            return prefix + contentId;
        }
    }
}

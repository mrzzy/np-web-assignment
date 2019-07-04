/*
 * Content Service Interface
 * Web Assignment 1
 */

using System.IO;

namespace folio.Services.Content
{
    public interface IContentService
    {
        // Insert the content in the given contentStream to the content service
        // returns a content id string which can be used to retrieve the content 
        // contentType provides a MIME content type & a prefix to the path
        // Optionally a directory prefix to the object being inserted
        string Insert(Stream contentStream, string contentType = null,
                string prefix="");
        
        // Updates the content in the given contentStream to the content service
        // specified by contentId
        // returns a content id string which can be used to retrieve the content 
        // Optionally a directory prefix to the object being inserted
        string Update(string contentId, Stream contentStream, string prefix="");

        // deletes the content specified by content id from the contentService
        // Optionally a directory prefix to the object being inserted
        void Delete(string contentId, string prefix="");

        // Encode the given content id in to a url so that can be used to 
        // retrieve the content given the content id returned by 
        // Insert()/Update()
        // Optionally a directory prefix to the object being inserted
        string EncodeUrl(string contentId, string prefix="");
        
        // Decode the content id from the given url (ie from EncodeUrl)
        string DecodeContentId(string url);

        // Check if the content service has the content given by content id 
        // returns true if the content exists, otherwise false
        bool HasObject(string contentId, string prefix="");
    }
}

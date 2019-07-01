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
        // Optionally provide a MIME content type 
        string Insert(Stream contentStream, string contentType = null);
        
        // Updates the content in the given contentStream to the content service
        // specified by contentId
        // returns a content id string which can be used to retrieve the content 
        // Optionally provide a MIME content type 
        string Update(string contentId, Stream contentStream);

        // deletes the content specified by content id from the contentService
        void Delete(string contentId);

        // Encode the given content id in to a url so that can be used to 
        // retrieve the content given the content id returned by 
        // InsertContent()/UpdateContent()
        string EncodeUrl(string contentId);
        
        // Decode the content id from the given url (ie from EncodeUrl)
        string DecodeContentId(string url);
    }
}

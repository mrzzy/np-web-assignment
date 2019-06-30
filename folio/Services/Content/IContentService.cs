/*
 * Content Service Interface
 * Web Assignment 1
 */

using System.IO;

namespace folio.Services.Content
{
    public interface IContentService
    {
        // Upload the content in the given contentStream to the content service
        // returns a content id string which can be used to retrieve the content 
        // Optionally provide a MIME content type 
        string UploadContent(Stream contentStream, string contentType = null);

        // Encode the given content id in to a url so that can be used to 
        // retrieve the content given the content id returned by UploadContent()
        string DecodeUrl(string contentId);
    }
}

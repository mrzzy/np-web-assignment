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
        string UploadContent(Stream contentStream);

        // Decode the url that can be used to retrieve the content given the 
        // content id returned by UploadContent()
        string decodeUrl(string contentId);
    }
}

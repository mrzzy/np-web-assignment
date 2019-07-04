/*
 * Web Assignment 
 * Folio API
 * File API controller 
*/

using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

using folio.Models;
using folio.FormModels;
using folio.Services.Auth;
using folio.Services.Content;

namespace folio.API.Controllers
{
    // controller for the /api/file/ route
    public class FileController: Controller
    {   
        /* API Actions */
        // route to upload the given file to the server
        // on success, returns a url to the uploaded file
        // authentication is required.
        [Authenticate]
        [HttpPost("/api/file/upload")]
        [Produces("application/json")]
        public ActionResult UploadFile(IFormFile file)
        {
            // build stream of the uploaded file
            MemoryStream contentStream = new MemoryStream();
            file.CopyTo(contentStream);
        
            // insert file into content service
            IContentService contentService = new GCSContentService();
            string contentId = contentService
                .Insert(contentStream, file.ContentType, prefix: "usr");
            
            // respond with content url
            string contentUrl = 
                contentService.EncodeUrl(contentId, prefix: "usr");
            return Json(new Dictionary<string, string>
            {
                {"FileUrl", contentUrl}
            });
        }
    
        // route to update a file with contents of update file form model
        // on success, returns a url to the updated file
        // authentication is required.
        [Authenticate]
        [HttpPost("/api/file/update")]
        [Produces("application/json")]
        public ActionResult UpdateFile([FromBody] FileUpdateFormModel formModel)
        {
            // validate contents of form model
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }
            
            // build stream of the uploaded file
            MemoryStream contentStream = new MemoryStream();
            formModel.File.CopyTo(contentStream);

            // update file with content service
            IContentService contentService = new GCSContentService();
            string contentId = contentService.DecodeContentId(formModel.FileUrl);
            contentId = contentService.Update(contentId, contentStream, prefix: "usr");
        
            // respond with content url
            string contentUrl = 
                contentService.EncodeUrl(contentId, prefix: "usr");
            return Json(new Dictionary<string, string>
            {
                {"FileUrl", contentUrl}
            });
        }
    
        // route to delete the file with the given url
        // authentication is required.
        [Authenticate]
        [HttpPost("/api/file/delete")]
        public ActionResult DeleteFile([FromBody] FileDeleteFormModel formModel)
        {
            // validate contents of form model
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }
        
            // remove file using content service
            IContentService contentService = new GCSContentService();
            string contentId = 
                contentService.DecodeContentId(formModel.FileUrl);
            contentService.Delete(contentId, prefix: "usr");

            return Ok();

        }
    }
}

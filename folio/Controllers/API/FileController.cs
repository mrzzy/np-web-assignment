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
        private const string ContentPrefix = "usr";
        private IContentService ContentService  = new GCSContentService();

        /* API Actions */
        // get the file for the given file id 
        [HttpGet("/api/file/{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            // determine url for file with the given file id using content service
            string fileUrl = this.ContentService
                .EncodeUrl(fileId, FileController.ContentPrefix);

            return RedirectPermanent(fileUrl);
        }

        // route to upload the given file to the server
        // on success, returns the content id of the file
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
            string fileId = this.ContentService.Insert(contentStream, 
                    file.ContentType, FileController.ContentPrefix);
            
            return Json(new Dictionary<string, string>
            {
                {"FileId", fileId}
            });
        }
    
        // route to update a file with contents of update file form model
        // on success, returns a url to the updated file
        // authentication is required.
        [Authenticate]
        [HttpPost("/api/file/update")]
        [Produces("application/json")]
        public ActionResult UpdateFile([FromForm] FileUpdateFormModel formModel)
        {
            // validate contents of form model
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }
            
            
            // build stream of the uploaded file
            MemoryStream contentStream = new MemoryStream();
            formModel.File.CopyTo(contentStream);

            // check if content actually exists
            string fileId = formModel.FileId;
            if(!this.ContentService
                    .HasObject(formModel.FileId, FileController.ContentPrefix))
            { return NotFound(); }

            // update file with content service
            fileId = this.ContentService
                .Update(fileId, contentStream, FileController.ContentPrefix);
        
            return Json(new Dictionary<string, string>
            {
                {"FileId", fileId}
            });
        }
    
        // route to delete the file with the given url
        // authentication is required.
        [Authenticate]
        [HttpPost("/api/file/delete")]
        public ActionResult DeleteFile([FromForm] FileDeleteFormModel formModel)
        {
            // validate contents of form model
            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }
        
            // remove file using content service
            string fileId = formModel.FileId;
            // delete only if content actually exists
            if(this.ContentService
                    .HasObject(fileId, FileController.ContentPrefix))
            {
                this.ContentService.Delete(fileId, FileController.ContentPrefix);
            }

            return Ok();
        }
    }
}

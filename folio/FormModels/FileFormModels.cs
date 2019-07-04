/*
 * Web Assignment 
 * Folio API
 * File Form Models
*/


using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

using folio.Models;

namespace folio.FormModels
{
    // model defines the properties required to delete a file
    public class FileUpdateFormModel
    {
        [MinLength(1)]
        [DataType(DataType.Url)]
        [Required]
        public string FileUrl  { get; set; }
    
        [Required]
        public IFormFile File { get; set; }
    }

    // model defines the properties required to delete a file
    public class FileDeleteFormModel
    {
        [MinLength(1)]
        [DataType(DataType.Url)]
        [Required]
        public string FileUrl  { get; set; }
    }
}

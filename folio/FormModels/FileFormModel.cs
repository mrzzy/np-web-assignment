/*
 * Web Assignment 
 * Folio API
 * File Form Model
*/


using System;
using System.ComponentModel.DataAnnotations;

using folio.Models;

namespace folio.FormModels
{
    // model defines the properties required to update delete a file
    public class FileFormModel
    {
        [MinLength(1)]
        [DataType(DataType.Url)]
        [Required(ErrorMessage="FileUrl is required to identify which file to delete")]
        public string FileUrl  { get; set; }
    }
}

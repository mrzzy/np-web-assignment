using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace folio.Models
{
    public class LecturerViewModel
    {
        public int LecturerId { get; set; }
        public string Name { get; set; }
        public string EmailAddr { get; set; }
        public string Password { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}

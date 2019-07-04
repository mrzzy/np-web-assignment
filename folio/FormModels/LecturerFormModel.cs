using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using folio.Models;
using System.ComponentModel.DataAnnotations;

namespace folio.FormModels
{
    public class LecturerCreateFormModel
    {
        [DataType(DataType.Text)]
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage ="This is a invalid Email address")]
        public string EmailAddr { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(500)]
        public string Description { get; set; }

        public Lecturer Create()
        {
            Lecturer l = new Lecturer();
            l.Name = this.Name;
            l.EmailAddr = this.EmailAddr;
            l.Description = this.Description;
            return l;
        }
    }

    public class LecturerUpdateFormModel
    {
        [DataType(DataType.Text)]
        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "This is a invalid Email address")]
        public string EmailAddr { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(500)]
        public string Description { get; set; }

        public void Apply(Lecturer l)
        {
            if (this.Name != null) l.Name = this.Name;
            if (this.EmailAddr != null) l.EmailAddr = this.EmailAddr;
            l.Description = this.Description;

        }
    }
}

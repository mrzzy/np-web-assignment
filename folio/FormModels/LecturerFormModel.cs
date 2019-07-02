using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using folio.Models;

namespace folio.FormModels
{
    public class LecturerFormModel
    {       

        public string Name { get; set; }
        public string EmailAddr { get; set; }
        public string Description { get; set; }

        public void Apply(Lecturer l)
        {
            l.Name = this.Name;
            l.EmailAddr = this.EmailAddr;
            l.Description = this.Description;
        }
    }
}

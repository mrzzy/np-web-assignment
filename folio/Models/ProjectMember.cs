/*
 * Folio - NP Web Assignment
 * ProjectMember Model
*/

using System;
using System.Collections.Generic;

namespace folio.Models
{   
    /* ProjectMember models the many to many relationship
     * between project and member
    */
    public partial class ProjectMember
    {        
        // One ProjectMember to many Project mapping 
        public int ProjectId  { get; set; }
        public Project Project  { get; set; }
        
        // One ProjectMember to many Member(student) mapping
        public int StudentId  { get; set; }
        public Student Member  { get; set; }
    }
}

/*
 * Folio - NP Web Assignment
 * StudentSkillSet Model
*/

using System;
using System.Collections.Generic;

namespace folio.Models
{   
    /* StudentSkillset models the many to many relationshipn
     * between student and skillset
    */
    public partial class StudentSkillSet
    {        
        // One StudentSkillset to many Student mapping 
        public int StudentId  { get; set; }
        public Student Student  { get; set; }
        
        // One StudentSkillset to many SkillSet mapping 
        public int SkillSetId  { get; set; }
        public SkillSet SkillSet  { get; set; }
    }
}

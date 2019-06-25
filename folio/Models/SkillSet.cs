using System;
using System.Collections.Generic;

namespace folio.Models
{
    public partial class SkillSet
    {
        public SkillSet()
        {
            this.StudentSkillSets = new HashSet<StudentSkillSet>();
        }

        public int SkillSetId { get; set; }
        public string SkillSetName { get; set; }
        
        // Foreign model relationships
        public virtual ICollection<StudentSkillSet> StudentSkillSets { get; set; }
    }
}

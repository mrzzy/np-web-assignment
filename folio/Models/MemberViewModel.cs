using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace folio.Models
{
    public class MemberViewModel
    {
        public List<Project> projectList { get; set; }
        public List<ProjectMember> projectMemberList { get; set; }
        public List<Student> studentList { get; set; }
    }
}

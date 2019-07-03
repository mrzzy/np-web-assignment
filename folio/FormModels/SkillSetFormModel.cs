/*
 * Web Assignment 
 * Folio API
 * SkillSet Create Form Model
*/


using System;
using System.ComponentModel.DataAnnotations;

using folio.Models;

namespace folio.FormModels
{
    // model defines the properties required to create/update a skillset model
    public class SkillSetFormModel
    {
        [MinLength(1)]
        [DataType(DateType.Text)]
        [Required(ErrorMessage="Skillset name is required")]
        public string SkillSetName  { get; set; }

        // Apply the the values of the properties of the SkillSet form model
        // to the given SkillSet model 
        public void Apply(SkillSet skillset)
        {
            skillset.SkillSetName = this.SkillSetName;
        }
    }
}

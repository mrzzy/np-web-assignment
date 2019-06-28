/*
 * Web Assignment 
 * Folio API
 * SkillSet Create Form Model
*/


using System;
using folio.Models;

namespace folio.FormModels
{
    // model defines the properties required to create/update a skillset modlel
    public class SkillSetFormModel
    {
        public string SkillSetName  { get; set; }

        // Apply the the values of the properties of the SkillSet form model
        // to the given SkillSet model 
        public void Apply(SkillSet skillset)
        {
            skillset.SkillSetName = this.SkillSetName;
        }
    }
}

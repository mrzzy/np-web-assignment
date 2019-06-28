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
        public int? SkillSetId  { get; set; }
        public string SkillSetName  { get; set; }

        // creates and return a SkillSet model given properties
        // of this SkillSet form model
        public SkillSet Create()
        {
            return new SkillSet { SkillSetName = this.SkillSetName };
        }

        // updates thegiven SkillSet model given properties of this 
        // SkillSet form model
        // Throws ArgumentException if given skillset does not match the form
        // skillset
        public void Update(SkillSet skillset)
        {
            // check if skillset id matches to validate if update is valid 
            if(skillset.SkillSetId != SkillSetId.Value)
                throw new ArgumentException(
                        "Refusing to update SkillSet with non matching SkillSetId");
                        
            skillset.SkillSetName = this.SkillSetName;
        }
    }
}

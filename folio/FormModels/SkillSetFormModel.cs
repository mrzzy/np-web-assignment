/*
 * Web Assignment 
 * Folio API
 * SkillSet Create Form Model
*/

using folio.Models;

namespace folio.FormModels
{
    // model defines the properties required to create a model
    public class SkillSetFormModel
    {
        public string SkillSetName  { get; set; }

        // Generates and return a SkillSet model given properties
        // of this SkillSEt form model
        public SkillSet Generate()
        {
            return new SkillSet { SkillSetName = this.SkillSetName };
        }
    }
}
